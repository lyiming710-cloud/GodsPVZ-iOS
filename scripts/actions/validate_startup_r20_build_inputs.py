"""Require exact candidate/compiler and actual Editor evidence for a test IPA."""
from pathlib import Path
import json,sys
sys.dont_write_bytecode=True
from bind_startup_r20_diagnostic import validate_checkpoint,CANDIDATE,require,sha
EDITOR=Path(__file__).resolve().parents[2]/'Recovery/Startup-R20-Editor-Diagnostic-2026-10-26'
def validate_editor(root=EDITOR):
    root=Path(root).resolve();rows=json.loads((root/'MANIFEST.json').read_bytes());names={p.relative_to(root).as_posix() for p in root.rglob('*') if p.is_file() and p!=root/'MANIFEST.json'}
    require(len(rows)==len({r['file'] for r in rows}) and names=={r['file'] for r in rows},'Editor evidence inventory mismatch')
    for r in rows:
        p=root/r['file'];require(p.resolve().is_relative_to(root) and not p.is_symlink(),'Unsafe Editor evidence path');b=p.read_bytes();require(len(b)==r['bytes'] and sha(b)==r['sha256'],'Changed Editor evidence: '+r['file'])
    actual=json.loads((root/'ACTUAL-EDITOR-READBACK.json').read_bytes());run=json.loads((root/'RUN-40224994102.json').read_bytes())
    require(actual['candidate_sha256']==CANDIDATE and actual['run_id']==40224994102,'Wrong actual Editor candidate/run')
    require(actual['source_commit']==run['headSha']=='e3b7e3d998bf1100cf7995bcce59cc474f316d47' and run['status']=='completed' and run['conclusion']=='success','Actual Editor source/result mismatch')
    startup,asset,shader=actual['startup'],actual['asset'],actual['shader']
    require(startup['pass'] and startup['frames']>=3 and startup['active_canvases']>=1 and startup['main_ui_components']>=1 and not startup['errors'],'Real startup probe did not pass')
    require(asset['pass'] and asset['resolved_plugin_scripts']==148 and asset['original_serialization_markers']==130 and asset['missing_components']==0 and not asset['errors'] and not asset['unresolved_custom_dummy_shaders'],'Real asset binding audit did not pass')
    require(shader['pass'] and len(shader['compilations'])==76 and len(shader['pixels'])==20 and shader['negative_compile_control'] and not shader['errors'],'Real shader audit did not pass')
    require(all(r['success'] and r['bytes']>0 for r in shader['compilations']) and all(r['pass'] for r in shader['pixels']),'Shader compilation/pixels failed')
    require(len(actual['raw_shader_and_image_files'])==96 and actual['personal_seat_return_exit']==0,'Raw shader evidence or seat return missing')
    require(actual['ipa'] is None and not actual['ipad_acceptance'] and not shader['metal_device_execution'] and not shader['game_pixel_equivalence'],'Keep product/device evidence boundaries explicit')
    return actual
def validate():
    data,compiler=validate_checkpoint();actual=validate_editor()
    return dict(scope='EXACT_R20_TEST_IPA_BUILD_INPUT_ONLY_NOT_GAME_ACCEPTANCE',candidate_sha256=sha(data),bytes=len(data),
        compiler_linux_run=compiler['linux_run'],compiler_apple_run=compiler['apple_run'],actual_editor_run=actual['run_id'],
        editor_frames=actual['startup']['frames'],retained_failed_or_unsupported_type_entries=70,
        remaining_runtime_profiles=compiler['current']['r19_runtime_profiles_still_pending'],
        test_build_allowed=True,game_semantics='NOT_PROVEN',device_execution='NOT_PERFORMED',ipa=None)
if __name__=='__main__':print(json.dumps(validate()))
