from pathlib import Path
import shutil,json,subprocess
w=Path('/workspaces/GodsPVZ-native24-codespace').resolve();base=w/'.validation';targets=[base/'native26-2026-10-01/restore-native26-check',base/'native25-2026-10-01/restore-native25-check']
print('DISK_BEFORE',subprocess.check_output(['df','-h',str(w)],text=True),flush=True)
for target in targets:
 resolved=target.resolve();assert resolved.is_relative_to(base.resolve()) and resolved.name in ['restore-native26-check','restore-native25-check'] and not resolved.is_symlink()
 if resolved.exists():
  sizes=sum(p.stat().st_size for p in resolved.rglob('*') if p.is_file());print('REMOVING_ONLY_RESTORE_TEST_COPY',str(resolved),sizes,flush=True);shutil.rmtree(resolved)
assert (base/'native25-2026-10-01/native24-restored/work/raw-1.dll').is_file()
assert (base/'native25-2026-10-01/native25-codespace-2026-10-01-evidence.zip').is_file()
assert (base/'native26-2026-10-01/native26-final1.dll').is_file()
print('DISK_AFTER',subprocess.check_output(['df','-h',str(w)],text=True),flush=True)
