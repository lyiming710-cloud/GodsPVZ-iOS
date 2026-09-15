#!/usr/bin/env python3
from pathlib import Path
import re,sys
if len(sys.argv)!=3: raise SystemExit('usage: audit.py <playmode.log> <candidate_sha>')
lines=Path(sys.argv[1]).read_text(errors='replace').splitlines(); text='\n'.join(lines); sha=sys.argv[2]
def invalid(method): return len(re.findall(r'InvalidProgramException: Invalid IL code in '+re.escape(method),text))
def linecount(exc,method): return sum(exc in x and method in x for x in lines)
counts={
'ZOMBIE_CTOR_INVALID_IL_COUNT':invalid('Zombie:.ctor'),
'ZOMBIEINFO_CTOR_INVALID_IL_COUNT':invalid('ZombieInfo:.ctor'),
'DEVICE_CTOR_FIELDACCESS_COUNT':linecount('FieldAccessException','Device:.ctor'),
'DEVICE_CTOR_INVALID_IL_COUNT':invalid('Device:.ctor'),
'DEVICE_CTOR_MISSINGMETHOD_COUNT':linecount('MissingMethodException','Device:.ctor'),
'SUPPLIES_CTOR_FIELDACCESS_COUNT':linecount('FieldAccessException','SuppliesInitialValue:.ctor'),
'SUPPLIES_CTOR_INVALID_IL_COUNT':invalid('SuppliesInitialValue:.ctor'),
'SUPPLIES_CTOR_MISSINGMETHOD_COUNT':linecount('MissingMethodException','SuppliesInitialValue:.ctor'),
'ALMANAC_CTOR_FIELDACCESS_COUNT':linecount('FieldAccessException','Almanac_ZombieWindow:.ctor'),
'ALMANAC_CTOR_INVALID_IL_COUNT':invalid('Almanac_ZombieWindow:.ctor'),
'ALMANAC_CTOR_MISSINGMETHOD_COUNT':linecount('MissingMethodException','Almanac_ZombieWindow:.ctor'),
'POPUP_CTOR_FIELDACCESS_COUNT':linecount('FieldAccessException','Popup:.ctor'),
'POPUP_CTOR_INVALID_IL_COUNT':invalid('Popup:.ctor'),
'POPUP_CTOR_MISSINGMETHOD_COUNT':linecount('MissingMethodException','Popup:.ctor'),
'CORELIB_TEXT_COUNT':text.count('System.Private.CoreLib'),
'ZEROVECTOR_TEXT_COUNT':text.count('zeroVector'),
'GENERIC_LIST_MISSINGMETHOD_COUNT':sum('MissingMethodException' in x and 'System.Collections.Generic.List' in x for x in lines),
'NEXT_ALMANAC_TALENTSYSTEM_CTOR_INVALID_IL_COUNT':invalid('Almanac_TalentSystem:.ctor'),
'NEXT_WINDOW_T_CTOR_INVALID_IL_COUNT':invalid('Window_T:.ctor'),
'NEXT_FLAGMETER_CTOR_INVALID_IL_COUNT':invalid('FlagMeter:.ctor'),
'NEXT_ALMANAC_DEVICEWINDOW_CTOR_INVALID_IL_COUNT':invalid('Almanac_DeviceWindow:.ctor'),
'NEXT_WINDOW_Q_CTOR_INVALID_IL_COUNT':invalid('Window_Q:.ctor'),
'NEXT_PATHDATAEDITOR_CTOR_INVALID_IL_COUNT':invalid('PathDataEditor:.ctor'),
'NEXT_SYSTEM0_CTOR_INVALID_IL_COUNT':invalid('System0:.ctor'),
'NEXT_ADMINISTRATOR_CTOR_INVALID_IL_COUNT':invalid('Administrator:.ctor'),
'NEXT_SYSTEM3_CTOR_INVALID_IL_COUNT':invalid('System3:.ctor'),
'NEXT_SUPPLIESINFO_CTOR_INVALID_IL_COUNT':invalid('SuppliesInfo:.ctor (int)'),
'NEXT_TEXTLINK_CTOR_INVALID_IL_COUNT':invalid('TextLink:.ctor'),
'NEXT_LEVELITEM_CTOR_INVALID_IL_COUNT':invalid('LevelItem:.ctor'),
'NEXT_TEXTLINK_UPDATE_INVALID_IL_COUNT':invalid('TextLink:Update'),
'NEXT_ADMINISTRATOR_START_INVALID_IL_COUNT':invalid('Administrator:Start'),
'NEXT_ADMINISTRATOR_UPDATE_INVALID_IL_COUNT':invalid('Administrator:Update'),
'NEXT_BGMVOLUME_INVALID_IL_COUNT':invalid('GlobalStaticVars:BGMVolume'),
}
for k,v in counts.items(): print(k,v)
for k in ['ZOMBIE_CTOR_INVALID_IL_COUNT','ZOMBIEINFO_CTOR_INVALID_IL_COUNT','DEVICE_CTOR_FIELDACCESS_COUNT','DEVICE_CTOR_INVALID_IL_COUNT','DEVICE_CTOR_MISSINGMETHOD_COUNT','SUPPLIES_CTOR_FIELDACCESS_COUNT','SUPPLIES_CTOR_INVALID_IL_COUNT','SUPPLIES_CTOR_MISSINGMETHOD_COUNT','ALMANAC_CTOR_FIELDACCESS_COUNT','ALMANAC_CTOR_INVALID_IL_COUNT','ALMANAC_CTOR_MISSINGMETHOD_COUNT','POPUP_CTOR_FIELDACCESS_COUNT','POPUP_CTOR_INVALID_IL_COUNT','POPUP_CTOR_MISSINGMETHOD_COUNT','CORELIB_TEXT_COUNT','ZEROVECTOR_TEXT_COUNT','GENERIC_LIST_MISSINGMETHOD_COUNT']:
    assert counts[k]==0,(k,counts[k])
markers=[x for x in lines if x.startswith('STAGE9_SKILLPROGRESS_')]
print('SKILLPROGRESS_MARKER_COUNT',len(markers)); [print(x) for x in markers]
required=['STAGE9_SKILLPROGRESS_SAVE_READY ok=1','STAGE9_SKILLPROGRESS_BOARD_READY ok=1','STAGE9_SKILLPROGRESS_READY_INVOKE ok=1','STAGE9_SKILLPROGRESS_PHASE phase=ready ok=1','STAGE9_SKILLPROGRESS_PHASE phase=active_charge ok=1','STAGE9_SKILLPROGRESS_PHASE phase=passive ok=1','STAGE9_SKILLPROGRESS_PHASE phase=ongoing_high ok=1','STAGE9_SKILLPROGRESS_PHASE phase=ongoing_low ok=1','STAGE9_SKILLPROGRESS_ALL_PHASES_PASS phases=5']
for x in required: assert x in markers,(x,markers)
print(f'POPUP_CTOR_NATURAL_RUNTIME_PASS candidate={sha} popup_fieldaccess=0 popup_invalid=0 popup_missingmethod=0 five_state_paths=5 corelib_pollution=0 zerovector_pollution=0')
