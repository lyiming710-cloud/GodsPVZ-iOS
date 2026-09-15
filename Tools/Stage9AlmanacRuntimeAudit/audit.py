from pathlib import Path
import re, sys

log = Path(sys.argv[1])
sha = sys.argv[2]
lines = log.read_text(errors='replace').splitlines()
text = '\n'.join(lines)

def inv(name):
    return len(re.findall(r'InvalidProgramException: Invalid IL code in ' + re.escape(name) + r':\.ctor', text))

def fam(exc, method):
    return sum(exc in x and method in x for x in lines)

counts = {
    'ZOMBIE_CTOR_INVALID_IL_COUNT': inv('Zombie'),
    'ZOMBIEINFO_CTOR_INVALID_IL_COUNT': inv('ZombieInfo'),
    'DEVICE_CTOR_FIELDACCESS_COUNT': fam('FieldAccessException','Device:.ctor'),
    'DEVICE_CTOR_INVALID_IL_COUNT': inv('Device'),
    'DEVICE_CTOR_MISSINGMETHOD_COUNT': fam('MissingMethodException','Device:.ctor'),
    'SUPPLIES_CTOR_FIELDACCESS_COUNT': fam('FieldAccessException','SuppliesInitialValue:.ctor'),
    'SUPPLIES_CTOR_INVALID_IL_COUNT': inv('SuppliesInitialValue'),
    'SUPPLIES_CTOR_MISSINGMETHOD_COUNT': fam('MissingMethodException','SuppliesInitialValue:.ctor'),
    'ALMANAC_CTOR_FIELDACCESS_COUNT': fam('FieldAccessException','Almanac_ZombieWindow:.ctor'),
    'ALMANAC_CTOR_INVALID_IL_COUNT': inv('Almanac_ZombieWindow'),
    'ALMANAC_CTOR_MISSINGMETHOD_COUNT': fam('MissingMethodException','Almanac_ZombieWindow:.ctor'),
    'CORELIB_TEXT_COUNT': text.count('System.Private.CoreLib'),
    'ZEROVECTOR_TEXT_COUNT': text.count('zeroVector'),
    'GENERIC_LIST_MISSINGMETHOD_COUNT': sum('MissingMethodException' in x and 'System.Collections.Generic.List' in x for x in lines),
}
for k,v in counts.items(): print(k, v)

for name in ['SuppliesInfo','PathDataEditor','System0','Administrator','System3','TextLink','Popup','Almanac_TalentSystem','Window_T','FlagMeter','Almanac_DeviceWindow','Window_Q','LevelItem']:
    print(f'PREFETCH_{name.upper()}_CTOR_INVALID_IL_COUNT', inv(name))
print('PREFETCH_TEXTLINK_UPDATE_INVALID_IL_COUNT', len(re.findall(r'InvalidProgramException: Invalid IL code in TextLink:Update', text)))
print('PREFETCH_ADMINISTRATOR_UPDATE_INVALID_IL_COUNT', len(re.findall(r'InvalidProgramException: Invalid IL code in Administrator:Update', text)))

markers = [x for x in lines if x.startswith('STAGE9_SKILLPROGRESS_')]
print('SKILLPROGRESS_MARKER_COUNT', len(markers))
for x in markers: print(x)
for k in counts: assert counts[k] == 0, (k, counts)
required = [
    'STAGE9_SKILLPROGRESS_SAVE_READY ok=1',
    'STAGE9_SKILLPROGRESS_BOARD_READY ok=1',
    'STAGE9_SKILLPROGRESS_READY_INVOKE ok=1',
    'STAGE9_SKILLPROGRESS_PHASE phase=ready ok=1',
    'STAGE9_SKILLPROGRESS_PHASE phase=active_charge ok=1',
    'STAGE9_SKILLPROGRESS_PHASE phase=passive ok=1',
    'STAGE9_SKILLPROGRESS_PHASE phase=ongoing_high ok=1',
    'STAGE9_SKILLPROGRESS_PHASE phase=ongoing_low ok=1',
    'STAGE9_SKILLPROGRESS_ALL_PHASES_PASS phases=5',
]
for x in required: assert x in markers, (x, markers)
assert sha == '2ad9b7db97262c5a2e3177a605704350cfe73f33a2ff15832defc254db3cdb3d', sha
print(f'ALMANAC_CTOR_NATURAL_RUNTIME_PASS candidate={sha} almanac_fieldaccess=0 almanac_invalid=0 almanac_missingmethod=0 five_state_paths=5 corelib_pollution=0 zerovector_pollution=0')
