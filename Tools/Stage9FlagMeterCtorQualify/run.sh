#!/usr/bin/env bash
set -euo pipefail
PROM="$1"; INPUT="$2"; REFS="$3"; ILSPY_DIR="$4"; O="$5"; E="$O/evidence"
mkdir -p "$E"
A=$(find "$PROM" -type f -name 'window-t-ctor-natural-runtime-audit.txt' -print -quit); test -n "$A"
for g in \
'^ZOMBIE_CTOR_INVALID_IL_COUNT 0$' \
'^ZOMBIEINFO_CTOR_INVALID_IL_COUNT 0$' \
'^DEVICE_CTOR_INVALID_IL_COUNT 0$' \
'^SUPPLIES_CTOR_INVALID_IL_COUNT 0$' \
'^ALMANAC_CTOR_INVALID_IL_COUNT 0$' \
'^POPUP_CTOR_INVALID_IL_COUNT 0$' \
'^ALMANAC_TALENT_CTOR_INVALID_IL_COUNT 0$' \
'^WINDOW_T_CTOR_FIELDACCESS_COUNT 0$' \
'^WINDOW_T_CTOR_INVALID_IL_COUNT 0$' \
'^WINDOW_T_CTOR_MISSINGMETHOD_COUNT 0$' \
'^CORELIB_TEXT_COUNT 0$' \
'^ZEROVECTOR_TEXT_COUNT 0$' \
'^GENERIC_LIST_MISSINGMETHOD_COUNT 0$' \
'^NEXT_FLAGMETER_CTOR_INVALID_IL_COUNT 2$' \
'^STAGE9_SKILLPROGRESS_ALL_PHASES_PASS phases=5$' \
'^WINDOW_T_CTOR_NATURAL_RUNTIME_PASS candidate=51d3a4ac4132fe8933f034287591ee80a6c884877b3ae4f23e799f8217e40b63 window_t_fieldaccess=0 window_t_invalid=0 window_t_missingmethod=0 five_state_paths=5 corelib_pollution=0 zerovector_pollution=0$'; do grep -q "$g" "$A"; done
IN=$(find "$INPUT" -type f -name 'GodsPVZRuntime1-window-t-ctor.dll' -print -quit); test -n "$IN"
test "$(sha256sum "$IN"|awk '{print $1}')" = '51d3a4ac4132fe8933f034287591ee80a6c884877b3ae4f23e799f8217e40b63'
OUT="$O/GodsPVZRuntime1-flagmeter-ctor.dll"
dotnet build Tools/Stage9Int32MinusOnePatch/Stage9Int32MinusOnePatch.csproj -c Release --nologo
dotnet run --project Tools/Stage9Int32MinusOnePatch/Stage9Int32MinusOnePatch.csproj -c Release --no-build -- \
 "$IN" "$OUT" '51d3a4ac4132fe8933f034287591ee80a6c884877b3ae4f23e799f8217e40b63' \
 'FlagMeter' '0x0600061B' 'theFlagID' '0x04000811' '52' '2' \
 '0x181B85E30' '0x18039D230' '0x18039D2AE' '3b9b04d203dad93fff7dcfa459f4b17cd9bd7f779d7418c9c2d6e5b4ca5d1fee' | tee "$E/patch.log"
grep -q '^NATIVE_AUTHORITY target_token=0x0600061B rid=1563 method_pointer_entry=0x181B85E30 va=0x18039D230 end=0x18039D2AE native_slice_sha256=3b9b04d203dad93fff7dcfa459f4b17cd9bd7f779d7418c9c2d6e5b4ca5d1fee$' "$E/patch.log"
grep -q '^SEMANTIC_ISOLATION_PASS untouched_methods=2316 changed_methods=1 target=0x0600061B$' "$E/patch.log"
grep -q '^FIELD_METADATA_ISOLATION_PASS unchanged_fields=2802 changed_fields=0$' "$E/patch.log"
grep -q '^FRAMEWORK_REFERENCE_GATE_PASS system_private_corelib_refs=0 assembly_reference_set_unchanged=1$' "$E/patch.log"
grep -q '^REOPEN_INT32_MINUS_ONE_PASS type=FlagMeter token=0x0600061B field=theFlagID field_token=0x04000811 ldc_i4_m1_present=1 ldc_i8_minus_one=0 target_stfld=1 ' "$E/patch.log"
sha256sum "$OUT" | tee "$E/output-candidate.sha256"
cp "$OUT" "$E/"; cp Tools/Stage9Int32MinusOnePatch/Program.cs "$E/"; cp Tools/Stage9NativeProbe/FlagMeter.ctor.native-evidence.md "$E/"; cp "$A" "$E/"
ILSPY=$(find "$ILSPY_DIR" -type f -name ilspycmd -print -quit); DOTNET_BIN=$(find "$ILSPY_DIR" -type f -path '*/ilspy-runtime/dotnet' -print -quit)
chmod +x "$ILSPY" "$DOTNET_BIN"; export DOTNET_ROOT="$(dirname "$DOTNET_BIN")"; export PATH="$DOTNET_ROOT:$PATH"
RDIR=$(find "$REFS" -type f -name UnityEngine.CoreModule.dll -printf '%h\n' | head -1); test -n "$RDIR"
"$ILSPY" -r "$RDIR" -il -t FlagMeter "$OUT" > "$E/FlagMeter.il" 2> "$E/FlagMeter.il.stderr"; test ! -s "$E/FlagMeter.il.stderr"
python3 - "$E/FlagMeter.il" <<'PY' | tee "$E/exact-ref-gate.txt"
from pathlib import Path
import sys
il=Path(sys.argv[1]).read_text(errors='replace'); e=il.find('} // end of method FlagMeter::.ctor'); assert e>=0
b=il.rfind('.method ',0,e); assert b>=0; ctor=il[b:e]; Path(sys.argv[1]+'.ctor.txt').write_text(ctor)
assert 'ldc.i4.m1' in ctor and 'ldc.i8' not in ctor,ctor
assert 'stfld int32 FlagMeter::theFlagID' in ctor,ctor
assert 'UnityEngine.MonoBehaviour::.ctor()' in ctor,ctor
assert 'System.Private.CoreLib' not in ctor and 'Unmanaged memory load' not in ctor,ctor
print('FLAGMETER_CTOR_EXACT_REF_PASS token=0x0600061B theFlagID_i4_minus_one=1 ldc_i8=0 corelib_pollution=0')
PY
