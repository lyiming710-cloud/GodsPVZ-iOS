#!/usr/bin/env bash
set -euo pipefail
PROM="$1"; INPUT="$2"; REFS="$3"; ILSPY_DIR="$4"; O="$5"; E="$O/evidence"
mkdir -p "$E"
A=$(find "$PROM" -type f -name 'levelitem-ctor-natural-runtime-audit.txt' -print -quit); test -n "$A"
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
'^SUPPLIESINFO_CTOR_FIELDACCESS_COUNT 0$' \
'^SUPPLIESINFO_CTOR_INVALID_IL_COUNT 0$' \
'^SUPPLIESINFO_CTOR_MISSINGMETHOD_COUNT 0$' \
'^TEXTLINK_CTOR_FIELDACCESS_COUNT 0$' \
'^TEXTLINK_CTOR_INVALID_IL_COUNT 0$' \
'^TEXTLINK_CTOR_MISSINGMETHOD_COUNT 0$' \
'^LEVELITEM_CTOR_FIELDACCESS_COUNT 0$' \
'^LEVELITEM_CTOR_INVALID_IL_COUNT 0$' \
'^LEVELITEM_CTOR_MISSINGMETHOD_COUNT 0$' \
'^CORELIB_TEXT_COUNT 0$' \
'^ZEROVECTOR_TEXT_COUNT 0$' \
'^GENERIC_LIST_MISSINGMETHOD_COUNT 0$' \
'^STAGE9_SKILLPROGRESS_ALL_PHASES_PASS phases=5$'; do grep -q "$g" "$A"; done
grep -q '^LEVELITEM_CTOR_NATURAL_RUNTIME_PASS candidate=fe67249cf3bdb30b94aa44045548c3de5ce6861e7686c7acebd4c97e4a3ead7b ' "$A"
grep -Eq '^NEXT_TEXTLINK_UPDATE_INVALID_IL_COUNT [1-9][0-9]*$' "$A"
grep -Eq '^FIRST_INVALID_IL line=[0-9]+ type=TextLink method=Update$' "$T"

IN=$(find "$INPUT" -type f -name 'GodsPVZRuntime1-levelitem-ctor.dll' -print -quit); test -n "$IN"
test "$(sha256sum "$IN"|awk '{print $1}')" = 'fe67249cf3bdb30b94aa44045548c3de5ce6861e7686c7acebd4c97e4a3ead7b'
OUT="$O/GodsPVZRuntime1-textlink-update.dll"
dotnet build Tools/Stage9TextLinkUpdatePatch/Stage9TextLinkUpdatePatch.csproj -c Release --nologo
dotnet run --project Tools/Stage9TextLinkUpdatePatch/Stage9TextLinkUpdatePatch.csproj -c Release --no-build -- \
 "$IN" "$OUT" 'fe67249cf3bdb30b94aa44045548c3de5ce6861e7686c7acebd4c97e4a3ead7b' | tee "$E/patch.log"
grep -q '^NATIVE_AUTHORITY target_token=0x060006DE rid=1758 method_pointer_entry=0x181B86448 va=0x1803ADEB0 end=0x1803ADF93 native_slice_sha256=5bf83d68b7d1c545595f58f52527bf0d29dfb7da03f76220fc74e4ed4c48b0a6$' "$E/patch.log"
grep -q '^RECOVERY_STRATEGY single_instruction_repair=1 index=3 old=Ldloca_V8_Object new=Ldloc_V1_Vector3 rebuild=0 metadata_changes=0 new_method_refs=0 guards=0 exception_swallowing=0$' "$E/patch.log"
grep -q '^REOPEN_TEXTLINK_UPDATE_PASS token=0x060006DE instruction_index=3 vector_arg=V_1_Vector3 camera_arg=V_2_Camera findIntersectingLink=1 SetLink=1 ResetLink=1$' "$E/patch.log"
grep -q '^TARGET_INSTRUCTION_ISOLATION_PASS changed_instruction_count=1 changed_index=3 old=Ldloca_V8_Object new=Ldloc_V1_Vector3$' "$E/patch.log"
grep -q '^SEMANTIC_ISOLATION_PASS untouched_methods=2316 changed_methods=1 target=0x060006DE$' "$E/patch.log"
grep -q '^FIELD_METADATA_ISOLATION_PASS unchanged_fields=2802 changed_fields=0$' "$E/patch.log"
grep -q '^FRAMEWORK_REFERENCE_GATE_PASS system_private_corelib_refs=0 assembly_reference_set_unchanged=1 new_used_method_refs=0$' "$E/patch.log"
sha256sum "$OUT" | tee "$E/output-candidate.sha256"
cp "$OUT" "$E/"; cp Tools/Stage9TextLinkUpdatePatch/Program.cs "$E/"; cp Tools/Stage9NativeProbe/TextLink.Update.native-evidence.md "$E/"; cp "$A" "$E/"; cp "$T" "$E/"

ILSPY=$(find "$ILSPY_DIR" -type f -name ilspycmd -print -quit); DOTNET_BIN=$(find "$ILSPY_DIR" -type f -path '*/ilspy-runtime/dotnet' -print -quit)
chmod +x "$ILSPY" "$DOTNET_BIN"; export DOTNET_ROOT="$(dirname "$DOTNET_BIN")"; export PATH="$DOTNET_ROOT:$PATH"
RDIR=$(find "$REFS" -type f -name UnityEngine.CoreModule.dll -printf '%h\n' | head -1); test -n "$RDIR"
"$ILSPY" -r "$RDIR" -il -t TextLink "$OUT" > "$E/TextLink.il" 2> "$E/TextLink.il.stderr"; test ! -s "$E/TextLink.il.stderr"
python3 - "$E/TextLink.il" <<'PY' | tee "$E/exact-ref-gate.txt"
from pathlib import Path
import re,sys
il=Path(sys.argv[1]).read_text(errors='replace'); e=il.find('} // end of method TextLink::Update'); assert e>=0
b=il.rfind('.method ',0,e); assert b>=0; m=il[b:e]; Path(sys.argv[1]+'.Update.txt').write_text(m)
assert 'TMP_TextUtilities::FindIntersectingLink' in m,m
assert m.count('TextLink::SetLink(int32)')==1,m
assert m.count('TextLink::ResetLink(int32)')==1,m
assert m.count('TextLink::linkIndex')>=2,m
assert m.count('TextLink::lastLinkIndex')>=4,m
assert 'ldloca' not in m,m
lines=m.splitlines(); ci=next(i for i,x in enumerate(lines) if 'TMP_TextUtilities::FindIntersectingLink' in x)
prev='\n'.join(lines[max(0,ci-5):ci])
assert re.search(r'ldloc(?:\.1|\s+1)\b',prev),prev
assert re.search(r'ldloc(?:\.2|\s+2)\b',prev),prev
assert 'System.Private.CoreLib' not in m
print('TEXTLINK_UPDATE_EXACT_REF_PASS token=0x060006DE vector_arg=V_1_Vector3 camera_arg=V_2_Camera findIntersectingLink=1 SetLink=1 ResetLink=1 ldloca=0 corelib_pollution=0')
PY
