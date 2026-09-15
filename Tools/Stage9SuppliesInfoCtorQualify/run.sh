#!/usr/bin/env bash
set -euo pipefail
PROM="$1"; INPUT="$2"; REFS="$3"; ILSPY_DIR="$4"; O="$5"; E="$O/evidence"
mkdir -p "$E"
A=$(find "$PROM" -type f -name 'system3-ctor-natural-runtime-audit.txt' -print -quit); test -n "$A"
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
'^SYSTEM3_CTOR_FIELDACCESS_COUNT 0$' \
'^SYSTEM3_CTOR_INVALID_IL_COUNT 0$' \
'^SYSTEM3_CTOR_MISSINGMETHOD_COUNT 0$' \
'^CORELIB_TEXT_COUNT 0$' \
'^ZEROVECTOR_TEXT_COUNT 0$' \
'^GENERIC_LIST_MISSINGMETHOD_COUNT 0$' \
'^STAGE9_SKILLPROGRESS_ALL_PHASES_PASS phases=5$' \
'^SYSTEM3_CTOR_NATURAL_RUNTIME_PASS candidate=27b27746df674dd7ddb967f3822dca77a102738e65ba58463d72f1dc6b537057 system3_fieldaccess=0 system3_invalid=0 system3_missingmethod=0 five_state_paths=5 corelib_pollution=0 zerovector_pollution=0$'; do grep -q "$g" "$A"; done
grep -Eq '^NEXT_SUPPLIESINFO_CTOR_INVALID_IL_COUNT [1-9][0-9]*$' "$A"
grep -Eq '^FIRST_INVALID_IL line=[0-9]+ type=SuppliesInfo method=\.ctor$' "$T"
IN=$(find "$INPUT" -type f -name 'GodsPVZRuntime1-system3-ctor.dll' -print -quit); test -n "$IN"
test "$(sha256sum "$IN"|awk '{print $1}')" = '27b27746df674dd7ddb967f3822dca77a102738e65ba58463d72f1dc6b537057'
OUT="$O/GodsPVZRuntime1-suppliesinfo-ctor.dll"
dotnet build Tools/Stage9SuppliesInfoCtorPatch/Stage9SuppliesInfoCtorPatch.csproj -c Release --nologo
dotnet run --project Tools/Stage9SuppliesInfoCtorPatch/Stage9SuppliesInfoCtorPatch.csproj -c Release --no-build -- \
 "$IN" "$OUT" '27b27746df674dd7ddb967f3822dca77a102738e65ba58463d72f1dc6b537057' | tee "$E/patch.log"
grep -q '^NATIVE_AUTHORITY target_token=0x06000150 rid=336 method_pointer_entry=0x181B837D8 va=0x180325180 end=0x1803251E8 native_slice_sha256=8f3ec66297f376a23bbc8330f86db9030203d2f8ba4b3c5a2cbc26c49f1a437a$' "$E/patch.log"
grep -q '^REOPEN_SUPPLIESINFO_CTOR_PASS token=0x06000150 name_token=0x04000174 id_token=0x04000175 string_empty=1 id_minus_one=1 object_ctor=1 id_arg=1 fake_diagnostics=0$' "$E/patch.log"
grep -q '^SEMANTIC_ISOLATION_PASS untouched_methods=2316 changed_methods=1 target=0x06000150$' "$E/patch.log"
grep -q '^FIELD_METADATA_ISOLATION_PASS unchanged_fields=2802 changed_fields=0$' "$E/patch.log"
grep -q '^FRAMEWORK_REFERENCE_GATE_PASS system_private_corelib_refs=0 assembly_reference_set_unchanged=1$' "$E/patch.log"
sha256sum "$OUT" | tee "$E/output-candidate.sha256"
cp "$OUT" "$E/"; cp Tools/Stage9SuppliesInfoCtorPatch/Program.cs "$E/"; cp Tools/Stage9NativeProbe/SuppliesInfo.ctor.native-evidence.md "$E/"; cp "$A" "$E/"; cp "$T" "$E/"
ILSPY=$(find "$ILSPY_DIR" -type f -name ilspycmd -print -quit); DOTNET_BIN=$(find "$ILSPY_DIR" -type f -path '*/ilspy-runtime/dotnet' -print -quit)
chmod +x "$ILSPY" "$DOTNET_BIN"; export DOTNET_ROOT="$(dirname "$DOTNET_BIN")"; export PATH="$DOTNET_ROOT:$PATH"
RDIR=$(find "$REFS" -type f -name UnityEngine.CoreModule.dll -printf '%h\n' | head -1); test -n "$RDIR"
"$ILSPY" -r "$RDIR" -il -t SuppliesInfo "$OUT" > "$E/SuppliesInfo.il" 2> "$E/SuppliesInfo.il.stderr"; test ! -s "$E/SuppliesInfo.il.stderr"
python3 - "$E/SuppliesInfo.il" <<'PY' | tee "$E/exact-ref-gate.txt"
from pathlib import Path
import sys
il=Path(sys.argv[1]).read_text(errors='replace'); e=il.find('} // end of method SuppliesInfo::.ctor'); assert e>=0
b=il.rfind('.method ',0,e); assert b>=0; ctor=il[b:e]; Path(sys.argv[1]+'.ctor.txt').write_text(ctor)
assert 'ldsfld string [mscorlib]System.String::Empty' in ctor or 'ldsfld string System.String::Empty' in ctor,ctor
assert 'stfld string SuppliesInfo::name' in ctor,ctor
assert 'ldc.i4.m1' in ctor and 'ldc.i8' not in ctor,ctor
assert ctor.count('stfld int32 SuppliesInfo::ID')==2,ctor
assert 'System.Object::.ctor()' in ctor,ctor
assert 'ldarg.1' in ctor,ctor
assert 'System.Private.CoreLib' not in ctor and 'Unmanaged memory load' not in ctor and 'Method not found @' not in ctor,ctor
print('SUPPLIESINFO_CTOR_EXACT_REF_PASS token=0x06000150 name_StringEmpty=1 id_minus_one=1 object_ctor=1 id_arg=1 fake_diagnostics=0 corelib_pollution=0')
PY
