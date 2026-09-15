#!/usr/bin/env bash
set -euo pipefail
PROM="$1"; INPUT="$2"; REFS="$3"; ILSPY_DIR="$4"; O="$5"; E="$O/evidence"
mkdir -p "$E"
A=$(find "$PROM" -type f -name 'administrator-ctor-natural-runtime-audit.txt' -print -quit); test -n "$A"
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
'^SYSTEM0_CTOR_FIELDACCESS_COUNT 0$' \
'^SYSTEM0_CTOR_INVALID_IL_COUNT 0$' \
'^SYSTEM0_CTOR_MISSINGMETHOD_COUNT 0$' \
'^ADMINISTRATOR_CTOR_FIELDACCESS_COUNT 0$' \
'^ADMINISTRATOR_CTOR_INVALID_IL_COUNT 0$' \
'^ADMINISTRATOR_CTOR_MISSINGMETHOD_COUNT 0$' \
'^CORELIB_TEXT_COUNT 0$' \
'^ZEROVECTOR_TEXT_COUNT 0$' \
'^GENERIC_LIST_MISSINGMETHOD_COUNT 0$' \
'^STAGE9_SKILLPROGRESS_ALL_PHASES_PASS phases=5$' \
'^ADMINISTRATOR_CTOR_NATURAL_RUNTIME_PASS candidate=ef0e435d545429557c5cd260ee0d6b9348f3ab8de4197836741b993a1fe9a5a2 administrator_fieldaccess=0 administrator_invalid=0 administrator_missingmethod=0 five_state_paths=5 corelib_pollution=0 zerovector_pollution=0$'; do grep -q "$g" "$A"; done
grep -Eq '^NEXT_SYSTEM3_CTOR_INVALID_IL_COUNT [1-9][0-9]*$' "$A"
grep -Eq '^FIRST_INVALID_IL line=[0-9]+ type=System3 method=\.ctor$' "$T"
IN=$(find "$INPUT" -type f -name 'GodsPVZRuntime1-administrator-ctor.dll' -print -quit); test -n "$IN"
test "$(sha256sum "$IN"|awk '{print $1}')" = 'ef0e435d545429557c5cd260ee0d6b9348f3ab8de4197836741b993a1fe9a5a2'
OUT="$O/GodsPVZRuntime1-system3-ctor.dll"
dotnet build Tools/Stage9Int32MinusOnePatch/Stage9Int32MinusOnePatch.csproj -c Release --nologo
dotnet run --project Tools/Stage9Int32MinusOnePatch/Stage9Int32MinusOnePatch.csproj -c Release --no-build -- \
 "$IN" "$OUT" 'ef0e435d545429557c5cd260ee0d6b9348f3ab8de4197836741b993a1fe9a5a2' \
 'System3' '0x060000A2' 'suppliesID' '0x040000BD' '38' '2' \
 '0x181B83268' '0x18030D720' '0x18030D779' '8d2257bd236bd71ff8ebe92ec8ed16084e34889b00196f01121db926da60041e' | tee "$E/patch.log"
grep -q '^NATIVE_AUTHORITY target_token=0x060000A2 rid=162 method_pointer_entry=0x181B83268 va=0x18030D720 end=0x18030D779 native_slice_sha256=8d2257bd236bd71ff8ebe92ec8ed16084e34889b00196f01121db926da60041e$' "$E/patch.log"
grep -q '^SEMANTIC_ISOLATION_PASS untouched_methods=2316 changed_methods=1 target=0x060000A2$' "$E/patch.log"
grep -q '^FIELD_METADATA_ISOLATION_PASS unchanged_fields=2802 changed_fields=0$' "$E/patch.log"
grep -q '^FRAMEWORK_REFERENCE_GATE_PASS system_private_corelib_refs=0 assembly_reference_set_unchanged=1$' "$E/patch.log"
grep -q '^REOPEN_INT32_MINUS_ONE_PASS type=System3 token=0x060000A2 field=suppliesID field_token=0x040000BD ldc_i4_m1_present=1 ldc_i8_minus_one=0 target_stfld=1 ' "$E/patch.log"
sha256sum "$OUT" | tee "$E/output-candidate.sha256"
cp "$OUT" "$E/"; cp Tools/Stage9Int32MinusOnePatch/Program.cs "$E/"; cp Tools/Stage9NativeProbe/System3.ctor.native-evidence.md "$E/"; cp "$A" "$E/"; cp "$T" "$E/"
ILSPY=$(find "$ILSPY_DIR" -type f -name ilspycmd -print -quit); DOTNET_BIN=$(find "$ILSPY_DIR" -type f -path '*/ilspy-runtime/dotnet' -print -quit)
chmod +x "$ILSPY" "$DOTNET_BIN"; export DOTNET_ROOT="$(dirname "$DOTNET_BIN")"; export PATH="$DOTNET_ROOT:$PATH"
RDIR=$(find "$REFS" -type f -name UnityEngine.CoreModule.dll -printf '%h\n' | head -1); test -n "$RDIR"
"$ILSPY" -r "$RDIR" -il -t System3 "$OUT" > "$E/System3.il" 2> "$E/System3.il.stderr"; test ! -s "$E/System3.il.stderr"
python3 - "$E/System3.il" <<'PY' | tee "$E/exact-ref-gate.txt"
from pathlib import Path
import sys
il=Path(sys.argv[1]).read_text(errors='replace'); e=il.find('} // end of method System3::.ctor'); assert e>=0
b=il.rfind('.method ',0,e); assert b>=0; ctor=il[b:e]; Path(sys.argv[1]+'.ctor.txt').write_text(ctor)
assert 'ldc.i4.m1' in ctor and 'ldc.i8' not in ctor,ctor
assert 'stfld int32 System3::suppliesID' in ctor,ctor
assert 'System.String::Empty' in ctor or 'string::Empty' in ctor or 'System.String::get_Empty' in ctor or 'ldsfld string [mscorlib]System.String::Empty' in ctor,ctor
assert 'UnityEngine.MonoBehaviour::.ctor()' in ctor,ctor
assert 'System.Private.CoreLib' not in ctor and 'Unmanaged memory load' not in ctor,ctor
print('SYSTEM3_CTOR_EXACT_REF_PASS token=0x060000A2 suppliesID_i4_minus_one=1 ldc_i8=0 corelib_pollution=0')
PY
