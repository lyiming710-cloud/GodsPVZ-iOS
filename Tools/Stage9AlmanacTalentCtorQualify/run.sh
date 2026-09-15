#!/usr/bin/env bash
set -euo pipefail
PROM="$1"; INPUT="$2"; REFS="$3"; ILSPY_DIR="$4"; O="$5"; E="$O/evidence"
mkdir -p "$E"
A=$(find "$PROM" -type f -name 'popup-ctor-natural-runtime-audit.txt' -print -quit); test -n "$A"
for g in \
'^ZOMBIE_CTOR_INVALID_IL_COUNT 0$' \
'^ZOMBIEINFO_CTOR_INVALID_IL_COUNT 0$' \
'^DEVICE_CTOR_FIELDACCESS_COUNT 0$' \
'^DEVICE_CTOR_INVALID_IL_COUNT 0$' \
'^SUPPLIES_CTOR_FIELDACCESS_COUNT 0$' \
'^SUPPLIES_CTOR_INVALID_IL_COUNT 0$' \
'^ALMANAC_CTOR_FIELDACCESS_COUNT 0$' \
'^ALMANAC_CTOR_INVALID_IL_COUNT 0$' \
'^POPUP_CTOR_FIELDACCESS_COUNT 0$' \
'^POPUP_CTOR_INVALID_IL_COUNT 0$' \
'^CORELIB_TEXT_COUNT 0$' \
'^ZEROVECTOR_TEXT_COUNT 0$' \
'^GENERIC_LIST_MISSINGMETHOD_COUNT 0$' \
'^STAGE9_SKILLPROGRESS_ALL_PHASES_PASS phases=5$'; do grep -q "$g" "$A"; done

IN=$(find "$INPUT" -type f -name 'GodsPVZRuntime1-popup-ctor.dll' -print -quit); test -n "$IN"
test "$(sha256sum "$IN"|awk '{print $1}')" = '805a177be099c923daf93a93d98d461312c76711f112c8f778800a69f5874253'
OUT="$O/GodsPVZRuntime1-almanac-talent-ctor.dll"
dotnet build Tools/Stage9Int32MinusOnePatch/Stage9Int32MinusOnePatch.csproj -c Release --nologo
dotnet run --project Tools/Stage9Int32MinusOnePatch/Stage9Int32MinusOnePatch.csproj -c Release --no-build -- \
  "$IN" "$OUT" '805a177be099c923daf93a93d98d461312c76711f112c8f778800a69f5874253' \
  'Almanac_TalentSystem' '0x0600059D' 'previewID' '0x0400073D' '107' '4' \
  '0x181B85A40' '0x180391AC0' '0x180391BCA' 'cd8b7cd491c67f65e18def6efc23352a030a43a97862afc6c2eea7c7c5b46645' | tee "$E/patch.log"
for g in \
'^INPUT_SHA256 805a177be099c923daf93a93d98d461312c76711f112c8f778800a69f5874253$' \
'^NATIVE_AUTHORITY target_token=0x0600059D rid=1437 method_pointer_entry=0x181B85A40 va=0x180391AC0 end=0x180391BCA native_slice_sha256=cd8b7cd491c67f65e18def6efc23352a030a43a97862afc6c2eea7c7c5b46645$' \
'^SEMANTIC_ISOLATION_PASS untouched_methods=2316 changed_methods=1 target=0x0600059D$' \
'^FIELD_METADATA_ISOLATION_PASS unchanged_fields=2802 changed_fields=0$' \
'^FRAMEWORK_REFERENCE_GATE_PASS system_private_corelib_refs=0 assembly_reference_set_unchanged=1$'; do grep -q "$g" "$E/patch.log"; done
grep -q '^REOPEN_INT32_MINUS_ONE_PASS type=Almanac_TalentSystem token=0x0600059D field=previewID field_token=0x0400073D ldc_i4_m1_present=1 ldc_i8_minus_one=0 target_stfld=1 ' "$E/patch.log"
sha256sum "$OUT" | tee "$E/output-candidate.sha256"
cp "$OUT" "$E/"; cp Tools/Stage9Int32MinusOnePatch/Program.cs "$E/"; cp "$A" "$E/"

ILSPY=$(find "$ILSPY_DIR" -type f -name ilspycmd -print -quit)
DOTNET_BIN=$(find "$ILSPY_DIR" -type f -path '*/ilspy-runtime/dotnet' -print -quit)
chmod +x "$ILSPY" "$DOTNET_BIN"; export DOTNET_ROOT="$(dirname "$DOTNET_BIN")"; export PATH="$DOTNET_ROOT:$PATH"
RDIR=$(find "$REFS" -type f -name UnityEngine.CoreModule.dll -printf '%h\n' | head -1); test -n "$RDIR"
"$ILSPY" -r "$RDIR" -t Almanac_TalentSystem "$OUT" > "$E/Almanac_TalentSystem.cs" 2> "$E/Almanac_TalentSystem.cs.stderr"
"$ILSPY" -r "$RDIR" -il -t Almanac_TalentSystem "$OUT" > "$E/Almanac_TalentSystem.il" 2> "$E/Almanac_TalentSystem.il.stderr"
test ! -s "$E/Almanac_TalentSystem.cs.stderr"; test ! -s "$E/Almanac_TalentSystem.il.stderr"
python3 - "$E/Almanac_TalentSystem.cs" "$E/Almanac_TalentSystem.il" <<'PY' | tee "$E/exact-ref-gate.txt"
from pathlib import Path
import sys
cs=Path(sys.argv[1]).read_text(errors='replace'); il=Path(sys.argv[2]).read_text(errors='replace')
e=il.find('} // end of method Almanac_TalentSystem::.ctor'); assert e>=0
b=il.rfind('.method ',0,e); assert b>=0
ctor=il[b:e]; Path(sys.argv[2]+'.ctor.txt').write_text(ctor)
assert 'ldc.i4.m1' in ctor and 'ldc.i8' not in ctor,ctor
assert 'stfld int32 Almanac_TalentSystem::previewID' in ctor,ctor
assert 'UnityEngine.MonoBehaviour::.ctor()' in ctor,ctor
assert 'System.Private.CoreLib' not in ctor and 'Unmanaged memory load' not in ctor,ctor
assert '.try' not in ctor and 'catch' not in ctor,ctor
assert 'previewID = -1;' in cs,cs
print('ALMANAC_TALENT_CTOR_EXACT_REF_PASS token=0x0600059D previewID_i4_minus_one=1 ldc_i8=0 corelib_pollution=0 exception_handlers=0')
PY
