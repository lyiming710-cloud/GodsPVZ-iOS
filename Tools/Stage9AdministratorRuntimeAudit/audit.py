#!/usr/bin/env python3
from pathlib import Path
import re,sys
if len(sys.argv)!=3: raise SystemExit('usage: audit.py <playmode.log> <candidate_sha>')
lines=Path(sys.argv[1]).read_text(errors='replace').splitlines(); text='\n'.join(lines); sha=sys.argv[2]
def invalid(m): return len(re.findall(r'InvalidProgramException: Invalid IL code in '+re.escape(m),text))
def exc(e,m): return sum(e in x and m in x for x in lines)
counts={
'ZOMBIE_CTOR_INVALID_IL_COUNT':invalid('Zombie:.ctor'),
'ZOMBIEINFO_CTOR_INVALID_IL_COUNT':invalid('ZombieInfo:.ctor'),
'DEVICE_CTOR_INVALID_IL_COUNT':invalid('Device:.ctor'),
'SUPPLIES_CTOR_INVALID_IL_COUNT':invalid('SuppliesInitialValue:.ctor'),
'ALMANAC_CTOR_INVALID_IL_COUNT':invalid('Almanac_ZombieWindow:.ctor'),
'POPUP_CTOR_INVALID_IL_COUNT':invalid('Popup:.ctor'),
'ALMANAC_TALENT_CTOR_INVALID_IL_COUNT':invalid('Almanac_TalentSystem:.ctor'),
'WINDOW_T_CTOR_INVALID_IL_COUNT':invalid('Window_T:.ctor'),
'FLAGMETER_CTOR_INVALID_IL_COUNT':invalid('FlagMeter:.ctor'),
'ALMANAC_DEVICEWINDOW_CTOR_INVALID_IL_COUNT':invalid('Almanac_DeviceWindow:.ctor'),
'WINDOW_Q_CTOR_INVALID_IL_COUNT':invalid('Window_Q:.ctor'),
'PATHDATAEDITOR_CTOR_INVALID_IL_COUNT':invalid('PathDataEditor:.ctor'),
'SYSTEM0_CTOR_INVALID_IL_COUNT':invalid('System0:.ctor'),
'ADMINISTRATOR_CTOR_FIELDACCESS_COUNT':exc('FieldAccessException','Administrator:.ctor'),
'ADMINISTRATOR_CTOR_INVALID_IL_COUNT':invalid('Administrator:.ctor'),
'ADMINISTRATOR_CTOR_MISSINGMETHOD_COUNT':exc('MissingMethodException','Administrator:.ctor'),
'CORELIB_TEXT_COUNT':text.count('System.Private.CoreLib'),
'ZEROVECTOR_TEXT_COUNT':text.count('zeroVector'),
'GENERIC_LIST_MISSINGMETHOD_COUNT':sum('MissingMethodException' in x and 'System.Collections.Generic.List' in x for x in lines),
'NEXT_SYSTEM3_CTOR_INVALID_IL_COUNT':invalid('System3:.ctor'),
'NEXT_SUPPLIESINFO_CTOR_INVALID_IL_COUNT':invalid('SuppliesInfo:.ctor (int)'),
'NEXT_TEXTLINK_CTOR_INVALID_IL_COUNT':invalid('TextLink:.ctor'),
'NEXT_LEVELITEM_CTOR_INVALID_IL_COUNT':invalid('LevelItem:.ctor'),
'NEXT_TEXTLINK_UPDATE_INVALID_IL_COUNT':invalid('TextLink:Update'),
'NEXT_ADMINISTRATOR_START_INVALID_IL_COUNT':invalid('Administrator:Start'),
'NEXT_ADMINISTRATOR_UPDATE_INVALID_IL_COUNT':invalid('Administrator:Update'),
'NEXT_BGMVOLUME_INVALID_IL_COUNT':invalid('GlobalStaticVars:BGMVolume')}
for k,v in counts.items(): print(k,v)
hard=['ZOMBIE_CTOR_INVALID_IL_COUNT','ZOMBIEINFO_CTOR_INVALID_IL_COUNT','DEVICE_CTOR_INVALID_IL_COUNT','SUPPLIES_CTOR_INVALID_IL_COUNT','ALMANAC_CTOR_INVALID_IL_COUNT','POPUP_CTOR_INVALID_IL_COUNT','ALMANAC_TALENT_CTOR_INVALID_IL_COUNT','WINDOW_T_CTOR_INVALID_IL_COUNT','FLAGMETER_CTOR_INVALID_IL_COUNT','ALMANAC_DEVICEWINDOW_CTOR_INVALID_IL_COUNT','WINDOW_Q_CTOR_INVALID_IL_COUNT','PATHDATAEDITOR_CTOR_INVALID_IL_COUNT','SYSTEM0_CTOR_INVALID_IL_COUNT','ADMINISTRATOR_CTOR_FIELDACCESS_COUNT','ADMINISTRATOR_CTOR_INVALID_IL_COUNT','ADMINISTRATOR_CTOR_MISSINGMETHOD_COUNT','CORELIB_TEXT_COUNT','ZEROVECTOR_TEXT_COUNT','GENERIC_LIST_MISSINGMETHOD_COUNT']
for k in hard: assert counts[k]==0,(k,counts[k])
markers=[x for x in lines if x.startswith('STAGE9_SKILLPROGRESS_')]; print('SKILLPROGRESS_MARKER_COUNT',len(markers)); [print(x) for x in markers]
for x in ['STAGE9_SKILLPROGRESS_SAVE_READY ok=1','STAGE9_SKILLPROGRESS_BOARD_READY ok=1','STAGE9_SKILLPROGRESS_READY_INVOKE ok=1','STAGE9_SKILLPROGRESS_PHASE phase=ready ok=1','STAGE9_SKILLPROGRESS_PHASE phase=active_charge ok=1','STAGE9_SKILLPROGRESS_PHASE phase=passive ok=1','STAGE9_SKILLPROGRESS_PHASE phase=ongoing_high ok=1','STAGE9_SKILLPROGRESS_PHASE phase=ongoing_low ok=1','STAGE9_SKILLPROGRESS_ALL_PHASES_PASS phases=5']: assert x in markers,(x,markers)
print(f'ADMINISTRATOR_CTOR_NATURAL_RUNTIME_PASS candidate={sha} administrator_fieldaccess=0 administrator_invalid=0 administrator_missingmethod=0 five_state_paths=5 corelib_pollution=0 zerovector_pollution=0')
