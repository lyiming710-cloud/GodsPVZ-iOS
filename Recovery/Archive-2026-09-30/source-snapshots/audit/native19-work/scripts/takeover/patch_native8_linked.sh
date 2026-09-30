#!/usr/bin/env bash
set -euo pipefail

REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
INPUT="${1:?usage: patch_native8_linked.sh <postlink-native7.dll> <postlink-native8.dll>}"
OUTPUT="${2:?usage: patch_native8_linked.sh <postlink-native7.dll> <postlink-native8.dll>}"
PROGRAM="$REPO_ROOT/scripts/takeover/PatcherNative8/Program.cs"
PATCHER="$REPO_ROOT/scripts/takeover/PatcherNative8/PatcherNative8.csproj"
FALLBACKS="$REPO_ROOT/scripts/takeover/PatcherNative8/ReferenceFallbacks.cs"
CECIL="$REPO_ROOT/Tools/Stage9Native4Recovery/tools/lib/netstandard2.0/Mono.Cecil.dll"

fail(){ echo "[native8-linked] ERROR: $*" >&2; exit 1; }
sha(){ sha256sum "$1" | awk '{print $1}'; }

[[ -f "$INPUT" ]] || fail "input missing: $INPUT"
[[ -f "$CECIL" ]] || fail "Mono.Cecil missing"
[[ -f "$FALLBACKS" ]] || fail "ReferenceFallbacks.cs missing"
command -v dotnet >/dev/null 2>&1 || fail "dotnet missing"

# Normalize a private build copy: repeated runs must not rewrite tracked source.
BUILD_DIR="$(mktemp -d)"
trap 'rm -rf -- "$BUILD_DIR"' EXIT
cp "$PROGRAM" "$BUILD_DIR/Program.cs"
cp "$FALLBACKS" "$BUILD_DIR/ReferenceFallbacks.cs"
cp "$PATCHER" "$BUILD_DIR/PatcherNative8.csproj"
PROGRAM="$BUILD_DIR/Program.cs"
PATCHER="$BUILD_DIR/PatcherNative8.csproj"
python3 - "$PATCHER" "$CECIL" <<'PY'
from pathlib import Path
import sys, xml.etree.ElementTree as ET
p=Path(sys.argv[1]); tree=ET.parse(p)
ref=tree.getroot().find(".//Reference[@Include='Mono.Cecil']")
assert ref is not None
ref.set('HintPath',sys.argv[2]); tree.write(p,encoding='unicode')
PY

# Same source-binding normalization used by the native8 unlinked materializer.
python3 - "$PROGRAM" <<'PY'
from pathlib import Path
import sys
p=Path(sys.argv[1]); s=p.read_text()
def once(old,new,label):
    global s
    if old not in s: raise SystemExit(label+' patch site missing')
    s=s.replace(old,new,1)
once('var forceMesh = RM(mod, "TMP_Text", "ForceMeshUpdate", 0);','var forceMesh = RM(mod, "TMP_Text", "ForceMeshUpdate", 2);','ForceMesh lookup')
once('il.Emit(OpCodes.Ldloc, self); il.Emit(OpCodes.Ldfld, fText); il.Emit(OpCodes.Callvirt, forceMesh);','il.Emit(OpCodes.Ldloc, self); il.Emit(OpCodes.Ldfld, fText); LdcI4(il,0); LdcI4(il,0); il.Emit(OpCodes.Callvirt, forceMesh);','ForceMesh call')
once('''    static FieldReference RF(ModuleDefinition m, string declaringName, string name) =>\n        m.GetMemberReferences().OfType<FieldReference>().First(f => f.DeclaringType.Name == declaringName && f.Name == name);''','''    static FieldReference RF(ModuleDefinition m, string declaringName, string name) =>\n        m.GetMemberReferences().OfType<FieldReference>().FirstOrDefault(f => f.DeclaringType.Name == declaringName && f.Name == name)\n        ?? ReferenceFallbacks.Field(m, declaringName, name);''','RF fallback')
once('''    static MethodReference RM(ModuleDefinition m, string declaringName, string name, int argc) =>\n        m.GetMemberReferences().OfType<MethodReference>().First(x => x.DeclaringType.Name == declaringName && x.Name == name && x.Parameters.Count == argc);''','''    static MethodReference RM(ModuleDefinition m, string declaringName, string name, int argc) =>\n        m.GetMemberReferences().OfType<MethodReference>().FirstOrDefault(x => x.DeclaringType.Name == declaringName && x.Name == name && x.Parameters.Count == argc)\n        ?? ReferenceFallbacks.Method(m, declaringName, name, argc);''','RM fallback')
p.write_text(s)
print('SOURCE_BINDING_NORMALIZATION_PASS')
PY

mkdir -p "$(dirname "$OUTPUT")"
A="${OUTPUT}.run1"; B="${OUTPUT}.run2"
rm -f "$A" "$B" "$OUTPUT"
dotnet run --project "$PATCHER" -- "$INPUT" "$A" linked | tee "${OUTPUT}.run1.log"
dotnet run --project "$PATCHER" -- "$INPUT" "$B" linked | tee "${OUTPUT}.run2.log"
SA="$(sha "$A")"; SB="$(sha "$B")"
[[ "$SA" = "$SB" ]] || fail "linked output non-deterministic SHA: $SA != $SB"
cmp -s "$A" "$B" || fail "linked output non-deterministic bytes"
mv "$A" "$OUTPUT"; rm -f "$B"

printf '%s\n' \
  "linked_input_sha256=$(sha "$INPUT")" \
  "linked_native8_sha256=$SA" \
  "effective_patcher_program_sha256=$(sha "$PROGRAM")" \
  "reference_fallbacks_sha256=$(sha "$FALLBACKS")" \
  "linked_deterministic_pair=PASS" \
  "target=TMPro.Examples.SkewTextExample/<WarpText>d__7::MoveNext" \
  "production_promotion=NO"
echo NATIVE8_LINKED_PATCH_PASS
