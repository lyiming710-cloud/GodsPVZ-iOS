#!/usr/bin/env bash
set -euo pipefail
PROM="$1"; INPUT="$2"; REFS="$3"; ILSPY_DIR="$4"; O="$5"; E="$O/evidence"
mkdir -p "$E"
A=$(find "$PROM" -type f -name 'almanac-ctor-natural-runtime-audit.txt' -print -quit); test -n "$A"
for g in \
'^ZOMBIE_CTOR_INVALID_IL_COUNT 0$' \
'^ZOMBIEINFO_CTOR_INVALID_IL_COUNT 0$' \
'^DEVICE_CTOR_FIELDACCESS_COUNT 0$' \
'^DEVICE_CTOR_INVALID_IL_COUNT 0$' \
'^SUPPLIES_CTOR_FIELDACCESS_COUNT 0$' \
'^SUPPLIES_CTOR_INVALID_IL_COUNT 0$' \
'^ALMANAC_CTOR_FIELDACCESS_COUNT 0$' \
'^ALMANAC_CTOR_INVALID_IL_COUNT 0$' \
'^CORELIB_TEXT_COUNT 0$' \
'^ZEROVECTOR_TEXT_COUNT 0$' \
'^GENERIC_LIST_MISSINGMETHOD_COUNT 0$' \
'^PREFETCH_POPUP_CTOR_INVALID_IL_COUNT 2$' \
'^STAGE9_SKILLPROGRESS_ALL_PHASES_PASS phases=5$' \
'^ALMANAC_CTOR_NATURAL_RUNTIME_PASS candidate=2ad9b7db97262c5a2e3177a605704350cfe73f33a2ff15832defc254db3cdb3d almanac_fieldaccess=0 almanac_invalid=0 almanac_missingmethod=0 five_state_paths=5 corelib_pollution=0 zerovector_pollution=0$'; do grep -q "$g" "$A"; done

IN=$(find "$INPUT" -type f -name 'GodsPVZRuntime1-almanac-ctor.dll' -print -quit); test -n "$IN"
test "$(sha256sum "$IN"|awk '{print $1}')" = '2ad9b7db97262c5a2e3177a605704350cfe73f33a2ff15832defc254db3cdb3d'
OUT="$O/GodsPVZRuntime1-popup-ctor.dll"
dotnet build Tools/Stage9PopupCtorPatch/Stage9PopupCtorPatch.csproj -c Release --nologo
dotnet run --project Tools/Stage9PopupCtorPatch/Stage9PopupCtorPatch.csproj -c Release --no-build -- "$IN" "$OUT" '2ad9b7db97262c5a2e3177a605704350cfe73f33a2ff15832defc254db3cdb3d' | tee "$E/patch.log"
for g in \
'^INPUT_SHA256 2ad9b7db97262c5a2e3177a605704350cfe73f33a2ff15832defc254db3cdb3d$' \
'^NATIVE_AUTHORITY target_token=0x06000676 rid=1654 method_pointer_entry=0x181B86108 va=0x1803A6450 end=0x1803A645E native_slice_sha256=4a7c3b3f5728880eb795eab8b5ede2a4d9b7ce6ef4de9f0d6b308376245813a3$' \
'^NATIVE_SEMANTICS info0_int_offset=0x60 store_width=32 value=-1 base_ctor_tailcall=1 target=0x1812E22A0$' \
'^RECOVERY_STRATEGY single_instruction_replace=1 old=ldc.i8_4294967295 new=ldc.i4.m1 metadata_changes=0 guards=0 exception_swallowing=0$' \
'^SEMANTIC_ISOLATION_PASS untouched_methods=2316 changed_methods=1 target=0x06000676$' \
'^FIELD_METADATA_ISOLATION_PASS unchanged_fields=2802 changed_fields=0$' \
'^FRAMEWORK_REFERENCE_GATE_PASS system_private_corelib_refs=0 assembly_reference_set_unchanged=1$'; do grep -q "$g" "$E/patch.log"; done
grep -Eq '^REOPEN_POPUP_CTOR_PASS token=0x06000676 instructions=6 code_size=[0-9]+ ldc_i4_m1=1 info0_int_stfld=1 monoBehaviour_ctor=1 exception_handlers=0 locals=1$' "$E/patch.log"
sha256sum "$OUT" | tee "$E/output-candidate.sha256"
cp "$OUT" "$E/"; cp Tools/Stage9PopupCtorPatch/Program.cs "$E/"; cp Tools/Stage9NativeProbe/Popup.ctor.native-evidence.md "$E/"; cp "$A" "$E/"

ILSPY=$(find "$ILSPY_DIR" -type f -name ilspycmd -print -quit)
DOTNET_BIN=$(find "$ILSPY_DIR" -type f -path '*/ilspy-runtime/dotnet' -print -quit)
chmod +x "$ILSPY" "$DOTNET_BIN"; export DOTNET_ROOT="$(dirname "$DOTNET_BIN")"; export PATH="$DOTNET_ROOT:$PATH"
RDIR=$(find "$REFS" -type f -name UnityEngine.CoreModule.dll -printf '%h\n' | head -1); test -n "$RDIR"
"$ILSPY" -r "$RDIR" -t Popup "$OUT" > "$E/Popup.cs" 2> "$E/Popup.cs.stderr"
"$ILSPY" -r "$RDIR" -il -t Popup "$OUT" > "$E/Popup.il" 2> "$E/Popup.il.stderr"
test ! -s "$E/Popup.cs.stderr"; test ! -s "$E/Popup.il.stderr"
python3 - "$E/Popup.cs" "$E/Popup.il" <<'PY' | tee "$E/exact-ref-gate.txt"
from pathlib import Path
import sys
cs=Path(sys.argv[1]).read_text(errors='replace'); il=Path(sys.argv[2]).read_text(errors='replace')
e=il.find('} // end of method Popup::.ctor'); assert e>=0
b=il.rfind('.method ',0,e); assert b>=0
ctor=il[b:e]; Path(sys.argv[2]+'.ctor.txt').write_text(ctor)
for x in ['ldc.i4.m1','stfld int32 Popup::info0_int','UnityEngine.MonoBehaviour::.ctor()','.locals init','[0] class Popup']: assert x in ctor,(x,ctor)
for x in ['ldc.i8','System.Private.CoreLib','Unmanaged memory load','.try','catch']: assert x not in ctor,(x,ctor)
assert 'info0_int = -1;' in cs,cs
print('POPUP_CTOR_EXACT_REF_PASS token=0x06000676 ldc_i4_m1=1 info0_int_stfld=1 monoBehaviour_ctor=1 local_popup=1 ldc_i8=0 corelib_pollution=0 exception_handlers=0')
PY
