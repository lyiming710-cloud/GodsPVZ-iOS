#!/usr/bin/env bash
set -euo pipefail
PROM="$1"; INPUT="$2"; REFS="$3"; ILSPY_DIR="$4"; O="$5"; E="$O/evidence"
mkdir -p "$E"
A=$(find "$PROM" -type f -name 'textlink-ctor-natural-runtime-audit.txt' -print -quit); test -n "$A"
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
'^TEXTLINK_CTOR_FIELDACCESS_COUNT 0$' \
'^TEXTLINK_CTOR_INVALID_IL_COUNT 0$' \
'^TEXTLINK_CTOR_MISSINGMETHOD_COUNT 0$' \
'^CORELIB_TEXT_COUNT 0$' \
'^ZEROVECTOR_TEXT_COUNT 0$' \
'^GENERIC_LIST_MISSINGMETHOD_COUNT 0$' \
'^STAGE9_SKILLPROGRESS_ALL_PHASES_PASS phases=5$'; do grep -q "$g" "$A"; done
grep -q '^TEXTLINK_CTOR_NATURAL_RUNTIME_PASS candidate=76c1c48219af73c73e48fcfb3d0a53254b567c07db7450a80ed51b61a2f6ce19 ' "$A"
grep -Eq '^NEXT_LEVELITEM_CTOR_INVALID_IL_COUNT [1-9][0-9]*$' "$A"
grep -Eq '^FIRST_INVALID_IL line=[0-9]+ type=LevelItem method=\.ctor$' "$T"

IN=$(find "$INPUT" -type f -name 'GodsPVZRuntime1-textlink-ctor.dll' -print -quit); test -n "$IN"
test "$(sha256sum "$IN"|awk '{print $1}')" = '76c1c48219af73c73e48fcfb3d0a53254b567c07db7450a80ed51b61a2f6ce19'
OUT="$O/GodsPVZRuntime1-levelitem-ctor.dll"
dotnet build Tools/Stage9LevelItemCtorPatch/Stage9LevelItemCtorPatch.csproj -c Release --nologo
dotnet run --project Tools/Stage9LevelItemCtorPatch/Stage9LevelItemCtorPatch.csproj -c Release --no-build -- \
 "$IN" "$OUT" '76c1c48219af73c73e48fcfb3d0a53254b567c07db7450a80ed51b61a2f6ce19' | tee "$E/patch.log"
grep -q '^NATIVE_AUTHORITY target_token=0x0600062D rid=1581 method_pointer_entry=0x181B85EC0 va=0x18039F130 end=0x18039F365 native_slice_sha256=00de368a2072ce9b97ae6e5211dd9262b61537bbd1be1d608638c09cdf7e295c$' "$E/patch.log"
grep -q '^NATIVE_SEMANTICS rescueSeedID=List<int>{-1,3,5,6,7} base_ctor=UnityEngine.MonoBehaviour::.ctor$' "$E/patch.log"
grep -q '^REOPEN_LEVELITEM_CTOR_PASS token=0x0600062D rescueSeed_token=0x04000838 list_values=-1,3,5,6,7 public_add_calls=5 base_ctor=1 fake_diagnostics=0 addWithResize=0$' "$E/patch.log"
grep -q '^SEMANTIC_ISOLATION_PASS untouched_methods=2316 changed_methods=1 target=0x0600062D$' "$E/patch.log"
grep -q '^FIELD_METADATA_ISOLATION_PASS unchanged_fields=2802 changed_fields=0$' "$E/patch.log"
grep -q '^FRAMEWORK_REFERENCE_GATE_PASS system_private_corelib_refs=0 assembly_reference_set_unchanged=1 new_used_method_refs=0$' "$E/patch.log"
sha256sum "$OUT" | tee "$E/output-candidate.sha256"
cp "$OUT" "$E/"; cp Tools/Stage9LevelItemCtorPatch/Program.cs "$E/"; cp Tools/Stage9NativeProbe/LevelItem.ctor.native-evidence.md "$E/"; cp "$A" "$E/"; cp "$T" "$E/"

ILSPY=$(find "$ILSPY_DIR" -type f -name ilspycmd -print -quit); DOTNET_BIN=$(find "$ILSPY_DIR" -type f -path '*/ilspy-runtime/dotnet' -print -quit)
chmod +x "$ILSPY" "$DOTNET_BIN"; export DOTNET_ROOT="$(dirname "$DOTNET_BIN")"; export PATH="$DOTNET_ROOT:$PATH"
RDIR=$(find "$REFS" -type f -name UnityEngine.CoreModule.dll -printf '%h\n' | head -1); test -n "$RDIR"
"$ILSPY" -r "$RDIR" -il -t LevelItem "$OUT" > "$E/LevelItem.il" 2> "$E/LevelItem.il.stderr"; test ! -s "$E/LevelItem.il.stderr"
python3 - "$E/LevelItem.il" <<'PY' | tee "$E/exact-ref-gate.txt"
from pathlib import Path
import sys
il=Path(sys.argv[1]).read_text(errors='replace'); e=il.find('} // end of method LevelItem::.ctor'); assert e>=0
b=il.rfind('.method ',0,e); assert b>=0; ctor=il[b:e]; Path(sys.argv[1]+'.ctor.txt').write_text(ctor)
for bad in ('Unmanaged memory load:', 'AddWithResize', 'ldc.i8', 'conv.i', 'System.Private.CoreLib'):
    assert bad not in ctor,(bad,ctor)
assert ctor.count('System.Collections.Generic.List`1<int32>::.ctor()') == 1, ctor
assert ctor.count('System.Collections.Generic.List`1<int32>::Add(!0)') == 5, ctor
assert ctor.count('LevelItem::rescueSeedID') == 1, ctor
assert ctor.count('UnityEngine.MonoBehaviour::.ctor()') == 1, ctor
need=['ldc.i4.m1','ldc.i4.3','ldc.i4.5','ldc.i4.6','ldc.i4.7']
pos=[]
for x in need:
    p=ctor.find(x); assert p>=0,(x,ctor); pos.append(p)
assert pos==sorted(pos), (pos,ctor)
print('LEVELITEM_CTOR_EXACT_REF_PASS token=0x0600062D rescueSeedID=1 list_ctor=1 public_add_calls=5 values=-1,3,5,6,7 base_ctor=1 fake_diagnostics=0 corelib_pollution=0')
PY
