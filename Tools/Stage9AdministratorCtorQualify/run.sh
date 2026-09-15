#!/usr/bin/env bash
set -euo pipefail
PROM="$1"; INPUT="$2"; REFS="$3"; ILSPY_DIR="$4"; O="$5"; E="$O/evidence"
mkdir -p "$E"
A=$(find "$PROM" -type f -name 'system0-ctor-natural-runtime-audit.txt' -print -quit); test -n "$A"
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
'^CORELIB_TEXT_COUNT 0$' \
'^ZEROVECTOR_TEXT_COUNT 0$' \
'^GENERIC_LIST_MISSINGMETHOD_COUNT 0$' \
'^STAGE9_SKILLPROGRESS_ALL_PHASES_PASS phases=5$' \
'^SYSTEM0_CTOR_NATURAL_RUNTIME_PASS candidate=fdd1ded12e78abaa660f0c788c6fa4a3c80f8a37a84ae81808edd37ed269fde2 system0_fieldaccess=0 system0_invalid=0 system0_missingmethod=0 five_state_paths=5 corelib_pollution=0 zerovector_pollution=0$'; do grep -q "$g" "$A"; done
grep -Eq '^NEXT_ADMINISTRATOR_CTOR_INVALID_IL_COUNT [1-9][0-9]*$' "$A"
grep -Eq '^FIRST_INVALID_IL line=[0-9]+ type=Administrator method=\.ctor$' "$T"
IN=$(find "$INPUT" -type f -name 'GodsPVZRuntime1-system0-ctor.dll' -print -quit); test -n "$IN"
test "$(sha256sum "$IN"|awk '{print $1}')" = 'fdd1ded12e78abaa660f0c788c6fa4a3c80f8a37a84ae81808edd37ed269fde2'
OUT="$O/GodsPVZRuntime1-administrator-ctor.dll"
dotnet build Tools/Stage9AdministratorCtorPatch/Stage9AdministratorCtorPatch.csproj -c Release --nologo
dotnet run --project Tools/Stage9AdministratorCtorPatch/Stage9AdministratorCtorPatch.csproj -c Release --no-build -- \
 "$IN" "$OUT" 'fdd1ded12e78abaa660f0c788c6fa4a3c80f8a37a84ae81808edd37ed269fde2' | tee "$E/patch.log"
grep -q '^NATIVE_AUTHORITY target_token=0x0600000C rid=12 method_pointer_entry=0x181B82DB8 va=0x1802FEB60 end=0x1802FED13 native_slice_sha256=0fc07b9a066f57d91a828c58f90fca975de1e77bebf89d15b1f404c7b3f51da5$' "$E/patch.log"
grep -q '^REOPEN_ADMINISTRATOR_CTOR_PASS token=0x0600000C mode_i4_m1=1 system3_suppliesID_i4_m1=1 ldc_i8_minus_one=0 ' "$E/patch.log"
grep -q '^SEMANTIC_ISOLATION_PASS untouched_methods=2316 changed_methods=1 target=0x0600000C$' "$E/patch.log"
grep -q '^FIELD_METADATA_ISOLATION_PASS unchanged_fields=2802 changed_fields=0$' "$E/patch.log"
grep -q '^FRAMEWORK_REFERENCE_GATE_PASS system_private_corelib_refs=0 assembly_reference_set_unchanged=1$' "$E/patch.log"
sha256sum "$OUT" | tee "$E/output-candidate.sha256"
cp "$OUT" "$E/"; cp Tools/Stage9AdministratorCtorPatch/Program.cs "$E/"; cp Tools/Stage9NativeProbe/Administrator.ctor.native-evidence.md "$E/"; cp "$A" "$E/"; cp "$T" "$E/"
ILSPY=$(find "$ILSPY_DIR" -type f -name ilspycmd -print -quit); DOTNET_BIN=$(find "$ILSPY_DIR" -type f -path '*/ilspy-runtime/dotnet' -print -quit)
chmod +x "$ILSPY" "$DOTNET_BIN"; export DOTNET_ROOT="$(dirname "$DOTNET_BIN")"; export PATH="$DOTNET_ROOT:$PATH"
RDIR=$(find "$REFS" -type f -name UnityEngine.CoreModule.dll -printf '%h\n' | head -1); test -n "$RDIR"
"$ILSPY" -r "$RDIR" -il -t Administrator "$OUT" > "$E/Administrator.il" 2> "$E/Administrator.il.stderr"; test ! -s "$E/Administrator.il.stderr"
python3 - "$E/Administrator.il" <<'PY' | tee "$E/exact-ref-gate.txt"
from pathlib import Path
import sys
il=Path(sys.argv[1]).read_text(errors='replace'); e=il.find('} // end of method Administrator::.ctor'); assert e>=0
b=il.rfind('.method ',0,e); assert b>=0; ctor=il[b:e]; Path(sys.argv[1]+'.ctor.txt').write_text(ctor)
assert 'ldc.i8' not in ctor,ctor
assert ctor.count('ldc.i4.m1') >= 2,ctor
assert 'stfld int32 Administrator::mode' in ctor,ctor
assert 'stfld int32 System3::suppliesID' in ctor,ctor
assert 'newobj instance void System1::.ctor()' in ctor,ctor
assert 'newobj instance void System3::.ctor()' in ctor,ctor
assert 'UnityEngine.MonoBehaviour::.ctor()' in ctor,ctor
assert 'System.Private.CoreLib' not in ctor and 'Unmanaged memory load' not in ctor,ctor
print('ADMINISTRATOR_CTOR_EXACT_REF_PASS token=0x0600000C mode_i4_minus_one=1 system3_suppliesID_i4_minus_one=1 ldc_i8=0 corelib_pollution=0')
PY
