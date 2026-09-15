#!/usr/bin/env bash
set -euo pipefail
PROM="$1"; INPUT="$2"; REFS="$3"; ILSPY_DIR="$4"; O="$5"; E="$O/evidence"
mkdir -p "$E"
A=$(find "$PROM" -type f -name 'pathdataeditor-ctor-natural-runtime-audit.txt' -print -quit); test -n "$A"
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
'^PATHDATAEDITOR_CTOR_FIELDACCESS_COUNT 0$' \
'^PATHDATAEDITOR_CTOR_INVALID_IL_COUNT 0$' \
'^PATHDATAEDITOR_CTOR_MISSINGMETHOD_COUNT 0$' \
'^CORELIB_TEXT_COUNT 0$' \
'^ZEROVECTOR_TEXT_COUNT 0$' \
'^GENERIC_LIST_MISSINGMETHOD_COUNT 0$' \
'^NEXT_SYSTEM0_CTOR_INVALID_IL_COUNT 5$' \
'^STAGE9_SKILLPROGRESS_ALL_PHASES_PASS phases=5$' \
'^PATHDATAEDITOR_CTOR_NATURAL_RUNTIME_PASS candidate=bcb601b890ca773ba8e7821e1abe6e0614a82264f123307efed91aecb1c6e22c pathdataeditor_fieldaccess=0 pathdataeditor_invalid=0 pathdataeditor_missingmethod=0 five_state_paths=5 corelib_pollution=0 zerovector_pollution=0$'; do grep -q "$g" "$A"; done
IN=$(find "$INPUT" -type f -name 'GodsPVZRuntime1-pathdataeditor-ctor.dll' -print -quit); test -n "$IN"
test "$(sha256sum "$IN"|awk '{print $1}')" = 'bcb601b890ca773ba8e7821e1abe6e0614a82264f123307efed91aecb1c6e22c'
OUT="$O/GodsPVZRuntime1-system0-ctor.dll"
dotnet build Tools/Stage9DualInt32MinusOnePatch/Stage9DualInt32MinusOnePatch.csproj -c Release --nologo
dotnet run --project Tools/Stage9DualInt32MinusOnePatch/Stage9DualInt32MinusOnePatch.csproj -c Release --no-build -- \
 "$IN" "$OUT" 'bcb601b890ca773ba8e7821e1abe6e0614a82264f123307efed91aecb1c6e22c' \
 'System0' '0x0600007B' 'plantID' '0x0400006F' 'order_check' '0x04000082' '138' '5' \
 '0x181B83130' '0x18030A030' '0x18030A14C' '713893ee2471f7f22e47ecdfdac1f46f5cf2aab5711a9158d543d5b8cfb85bda' | tee "$E/patch.log"
grep -q '^NATIVE_AUTHORITY target_token=0x0600007B rid=123 method_pointer_entry=0x181B83130 va=0x18030A030 end=0x18030A14C native_slice_sha256=713893ee2471f7f22e47ecdfdac1f46f5cf2aab5711a9158d543d5b8cfb85bda$' "$E/patch.log"
grep -q '^SEMANTIC_ISOLATION_PASS untouched_methods=2316 changed_methods=1 target=0x0600007B$' "$E/patch.log"
grep -q '^FIELD_METADATA_ISOLATION_PASS unchanged_fields=2802 changed_fields=0$' "$E/patch.log"
grep -q '^FRAMEWORK_REFERENCE_GATE_PASS system_private_corelib_refs=0 assembly_reference_set_unchanged=1$' "$E/patch.log"
grep -q '^REOPEN_DUAL_INT32_MINUS_ONE_PASS type=System0 token=0x0600007B field1=plantID field1_token=0x0400006F field2=order_check field2_token=0x04000082 ldc_i4_m1_pairs=2 ldc_i8_minus_one=0 ' "$E/patch.log"
sha256sum "$OUT" | tee "$E/output-candidate.sha256"
cp "$OUT" "$E/"; cp Tools/Stage9DualInt32MinusOnePatch/Program.cs "$E/"; cp Tools/Stage9NativeProbe/System0.ctor.native-evidence.md "$E/"; cp "$A" "$E/"
ILSPY=$(find "$ILSPY_DIR" -type f -name ilspycmd -print -quit); DOTNET_BIN=$(find "$ILSPY_DIR" -type f -path '*/ilspy-runtime/dotnet' -print -quit)
chmod +x "$ILSPY" "$DOTNET_BIN"; export DOTNET_ROOT="$(dirname "$DOTNET_BIN")"; export PATH="$DOTNET_ROOT:$PATH"
RDIR=$(find "$REFS" -type f -name UnityEngine.CoreModule.dll -printf '%h\n' | head -1); test -n "$RDIR"
"$ILSPY" -r "$RDIR" -il -t System0 "$OUT" > "$E/System0.il" 2> "$E/System0.il.stderr"; test ! -s "$E/System0.il.stderr"
python3 - "$E/System0.il" <<'PY' | tee "$E/exact-ref-gate.txt"
from pathlib import Path
import sys,re
il=Path(sys.argv[1]).read_text(errors='replace'); e=il.find('} // end of method System0::.ctor'); assert e>=0
b=il.rfind('.method ',0,e); assert b>=0; ctor=il[b:e]; Path(sys.argv[1]+'.ctor.txt').write_text(ctor)
assert ctor.count('ldc.i4.m1') >= 2 and 'ldc.i8' not in ctor,ctor
assert 'stfld int32 System0::plantID' in ctor,ctor
assert 'stfld int32 System0::order_check' in ctor,ctor
assert 'UnityEngine.MonoBehaviour::.ctor()' in ctor,ctor
assert 'System.Private.CoreLib' not in ctor and 'Unmanaged memory load' not in ctor,ctor
print('SYSTEM0_CTOR_EXACT_REF_PASS token=0x0600007B plantID_i4_minus_one=1 order_check_i4_minus_one=1 ldc_i8=0 corelib_pollution=0')
PY
