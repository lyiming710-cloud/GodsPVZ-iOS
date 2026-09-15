#!/usr/bin/env bash
set -euo pipefail
PROM="$1"; INPUT="$2"; REFS="$3"; ILSPY_DIR="$4"; O="$5"; E="$O/evidence"
mkdir -p "$E"
A=$(find "$PROM" -type f -name 'suppliesinfo-ctor-natural-runtime-audit.txt' -print -quit); test -n "$A"
T=$(find "$PROM" -type f -name 'first-invalid-triage.txt' -print -quit); test -n "$T"
for g in \
'^ZOMBIE_CTOR_INVALID_IL_COUNT 0$' \
'^ZOMBIEINFO_CTOR_INVALID_IL_COUNT 0$' \
'^DEVICE_CTOR_INVALID_IL_COUNT 0$' \
'^SUPPLIES_CTOR_INVALID_IL_COUNT 0$' \
'^ALMANAC_CTOR_INVALID_IL_COUNT 0$' \
'^POPUP_CTOR_INVALID_IL_COUNT 0$' \
'^ALMANAC_TALENT_CTOR_INVALID_IL_COUNT 0$' \
'^WINDOW_T_CTOR_INVALID_IL_COUNT 0$' \
'^FLAGMETER_CTOR_INVALID_IL_COUNT 0$' \
'^ALMANAC_DEVICEWINDOW_CTOR_INVALID_IL_COUNT 0$' \
'^WINDOW_Q_CTOR_INVALID_IL_COUNT 0$' \
'^PATHDATAEDITOR_CTOR_INVALID_IL_COUNT 0$' \
'^SYSTEM0_CTOR_INVALID_IL_COUNT 0$' \
'^ADMINISTRATOR_CTOR_INVALID_IL_COUNT 0$' \
'^SYSTEM3_CTOR_INVALID_IL_COUNT 0$' \
'^SUPPLIESINFO_CTOR_FIELDACCESS_COUNT 0$' \
'^SUPPLIESINFO_CTOR_INVALID_IL_COUNT 0$' \
'^SUPPLIESINFO_CTOR_MISSINGMETHOD_COUNT 0$' \
'^CORELIB_TEXT_COUNT 0$' \
'^ZEROVECTOR_TEXT_COUNT 0$' \
'^GENERIC_LIST_MISSINGMETHOD_COUNT 0$' \
'^STAGE9_SKILLPROGRESS_ALL_PHASES_PASS phases=5$'; do grep -q "$g" "$A"; done
grep -q '^SUPPLIESINFO_CTOR_NATURAL_RUNTIME_PASS candidate=342c0a94f90371415c7f782604d9976bb91befec20500894dfd8af607b8ac5c4 ' "$A"
grep -Eq '^NEXT_TEXTLINK_CTOR_INVALID_IL_COUNT [1-9][0-9]*$' "$A"
grep -Eq '^FIRST_INVALID_IL line=[0-9]+ type=TextLink method=\.ctor$' "$T"

IN=$(find "$INPUT" -type f -name 'GodsPVZRuntime1-suppliesinfo-ctor.dll' -print -quit); test -n "$IN"
test "$(sha256sum "$IN"|awk '{print $1}')" = '342c0a94f90371415c7f782604d9976bb91befec20500894dfd8af607b8ac5c4'
OUT="$O/GodsPVZRuntime1-textlink-ctor.dll"
dotnet build Tools/Stage9TextLinkCtorPatch/Stage9TextLinkCtorPatch.csproj -c Release --nologo
dotnet run --project Tools/Stage9TextLinkCtorPatch/Stage9TextLinkCtorPatch.csproj -c Release --no-build -- \
 "$IN" "$OUT" '342c0a94f90371415c7f782604d9976bb91befec20500894dfd8af607b8ac5c4' | tee "$E/patch.log"
grep -q '^NATIVE_AUTHORITY target_token=0x060006E5 rid=1765 method_pointer_entry=0x181B86480 va=0x1803ADFA0 end=0x1803ADFBF native_slice_sha256=699419627a7893a66b38156beaf1ff5b0fe8530e3015e6fab90d976d7b70669c color_constant_va=0x1815A7B70 rgba=1,1,1,1$' "$E/patch.log"
grep -q '^NATIVE_SEMANTICS linkIndex=-1 lastLinkIndex=-1 hoverColor=Color.white originalColor=Color.white base_ctor=UnityEngine.MonoBehaviour::.ctor$' "$E/patch.log"
grep -q '^REOPEN_TEXTLINK_CTOR_PASS token=0x060006E5 linkIndex_token=0x040008E2 lastLinkIndex_token=0x040008E3 hoverColor_token=0x040008E4 originalColor_token=0x040008E5 int32_minus_one=2 color_white_rgba=2 base_ctor=1 fake_diagnostics=0$' "$E/patch.log"
grep -q '^SEMANTIC_ISOLATION_PASS untouched_methods=2316 changed_methods=1 target=0x060006E5$' "$E/patch.log"
grep -q '^FIELD_METADATA_ISOLATION_PASS unchanged_fields=2802 changed_fields=0$' "$E/patch.log"
grep -q '^FRAMEWORK_REFERENCE_GATE_PASS system_private_corelib_refs=0 assembly_reference_set_unchanged=1 unique_method_ref_set_unchanged=1$' "$E/patch.log"
sha256sum "$OUT" | tee "$E/output-candidate.sha256"
cp "$OUT" "$E/"; cp Tools/Stage9TextLinkCtorPatch/Program.cs "$E/"; cp Tools/Stage9NativeProbe/TextLink.ctor.native-evidence.md "$E/"; cp "$A" "$E/"; cp "$T" "$E/"

ILSPY=$(find "$ILSPY_DIR" -type f -name ilspycmd -print -quit); DOTNET_BIN=$(find "$ILSPY_DIR" -type f -path '*/ilspy-runtime/dotnet' -print -quit)
chmod +x "$ILSPY" "$DOTNET_BIN"; export DOTNET_ROOT="$(dirname "$DOTNET_BIN")"; export PATH="$DOTNET_ROOT:$PATH"
RDIR=$(find "$REFS" -type f -name UnityEngine.CoreModule.dll -printf '%h\n' | head -1); test -n "$RDIR"
"$ILSPY" -r "$RDIR" -il -t TextLink "$OUT" > "$E/TextLink.il" 2> "$E/TextLink.il.stderr"; test ! -s "$E/TextLink.il.stderr"
python3 - "$E/TextLink.il" <<'PY' | tee "$E/exact-ref-gate.txt"
from pathlib import Path
import re,sys
il=Path(sys.argv[1]).read_text(errors='replace'); e=il.find('} // end of method TextLink::.ctor'); assert e>=0
b=il.rfind('.method ',0,e); assert b>=0; ctor=il[b:e]; Path(sys.argv[1]+'.ctor.txt').write_text(ctor)
assert ctor.count('ldc.i4.m1') == 2, ctor
assert ctor.count('TextLink::linkIndex') == 1, ctor
assert ctor.count('TextLink::lastLinkIndex') == 1, ctor
assert ctor.count('TextLink::hoverColor') == 1, ctor
assert ctor.count('TextLink::originalColor') == 1, ctor
assert ctor.count('ldc.r4 1') >= 8, ctor
assert ctor.count('UnityEngine.Color::.ctor(float32, float32, float32, float32)') == 2 or ctor.count('UnityEngine.Color::.ctor(float32,float32,float32,float32)') == 2, ctor
assert ctor.count('UnityEngine.MonoBehaviour::.ctor()') == 1, ctor
for bad in ('Unmanaged memory load:', 'conv.i', 'ldc.i8', 'System.Private.CoreLib'):
    assert bad not in ctor, (bad,ctor)
print('TEXTLINK_CTOR_EXACT_REF_PASS token=0x060006E5 link_minus_one=1 last_minus_one=1 white_rgba=2 color_ctor_existing_ref=1 base_ctor=1 fake_diagnostics=0 corelib_pollution=0')
PY
