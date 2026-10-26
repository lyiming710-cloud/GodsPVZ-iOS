"""Resolve actual case collisions by path only, preserving every asset/meta byte and GUID."""
from pathlib import Path
import argparse,collections,hashlib,json,re,zipfile

def sha_file(p):
    h=hashlib.sha256()
    with p.open('rb') as f:
        for b in iter(lambda:f.read(1024*1024),b''):h.update(b)
    return h.hexdigest()
def write(p,v):p.parent.mkdir(exist_ok=True,parents=True);p.write_text(json.dumps(v,indent=2)+'\n','utf-8')
def inventory(project):
    root=Path(project).resolve();assets=root/'Assets';assert assets.is_dir();result=[]
    for p in sorted(assets.rglob('*')):
        if not p.is_file():continue
        assert not p.is_symlink() and p.resolve().is_relative_to(root)
        result.append(dict(file=p.relative_to(root).as_posix(),bytes=p.stat().st_size,sha256=sha_file(p)))
    return result
def collisions(rows):
    by=collections.defaultdict(list)
    for row in rows:by[row['file'].casefold()].append(row['file'])
    return sorted([sorted(v) for v in by.values() if len(v)>1])
def plan(project,candidate=None):
    root=Path(project).resolve();rows=inventory(root);by={r['file']:r for r in rows};groups=collisions(rows);renames=[];handled=set();guids=set()
    for group in groups:
        assets=[p for p in group if not p.endswith('.meta')]
        if not assets:continue
        for name in assets:
            p=root/name;meta=name+'.meta';assert meta in by,'Every asset collision needs its original meta: '+name
            assert all(part.casefold() not in ['resources','streamingassets'] for part in p.relative_to(root).parts),'Cannot rename runtime path-addressed assets without a separate contract'
            matches=re.findall(rb'^guid: ([0-9a-fA-F]{32})\s*$',(root/meta).read_bytes(),re.M);assert len(matches)==1,'Exactly one original asset GUID required: '+name
            guid=matches[0].decode().lower();assert guid not in guids,'Duplicated GUID in collision assets';guids.add(guid)
            # Preserve the basename too: TextureImporter derives Texture2D.name
            # from it. GUID-specific folders separate paths without renaming assets.
            new=(p.parent/('__r3guid_'+guid)/p.name).relative_to(root).as_posix()
            for before,after in [(name,new),(meta,new+'.meta')]:
                assert before not in handled and after not in by and after.casefold() not in {r['file'].casefold() for r in rows},after
                handled.add(before);renames.append(dict(before=before,after=after,guid=guid,bytes=by[before]['bytes'],sha256=by[before]['sha256']))
    assert handled=={p for group in groups for p in group},'Unpaired file or directory casing collision requires separate review'
    paths={r['before'] for r in renames if not r['before'].endswith('.meta')};needles={}
    for p in paths:
        for variant in [p,p.replace('/','\\'),p.removeprefix('Assets/'),p.removeprefix('Assets/').replace('/','\\')]:needles.setdefault(variant.encode(),set()).add(p)
    mentions=[];text_suffixes={'.cs','.unity','.prefab','.asset','.mat','.controller','.overridecontroller','.json','.xml','.txt','.asmdef','.asmref','.shader','.meta','.anim'}
    for row in rows:
        p=root/row['file']
        if p.suffix.casefold() not in text_suffixes:continue
        data=p.read_bytes()
        for needle,owners in needles.items():
            if needle in data:
                for name in owners:mentions.append(dict(file=row['file'],old_asset_path=name,matched_literal=needle.decode()))
    if candidate:
        data=Path(candidate).read_bytes()
        for needle,owners in needles.items():
            for encoding in ['utf-8','utf-16-le']:
                if needle.decode().encode(encoding) in data:
                    for name in owners:mentions.append(dict(file='CANDIDATE_DLL_USER_STRING_OR_BINARY',old_asset_path=name,encoding=encoding,matched_literal=needle.decode()))
    assert not mentions,'Explicit old asset-path references require separate review: '+json.dumps(mentions[:10])
    translated=[dict(row,file=next((r['after'] for r in renames if r['before']==row['file']),row['file'])) for row in rows];assert not collisions(translated)
    return dict(scope='EXACT_R3_ASSET_CASE_PATH_ONLY_PLAN',asset_files=len(rows),collision_file_groups=groups,collision_asset_groups=sum(not g[0].endswith('.meta') for g in groups),renamed_files=len(renames),renamed_assets=len(guids),renames=renames,before_inventory=rows,after_inventory=sorted(translated,key=lambda r:r['file']),old_explicit_path_reference_mentions=mentions,candidate_literals_scanned=bool(candidate),asset_contents_changed=False,meta_contents_or_guids_changed=False,internal_names_changed=False,new_game_dll_changes=0,not_a_unity_build_or_device_pass=True)

def apply(project,frozen):
    root=Path(project).resolve();before=inventory(root);assert before==frozen['before_inventory'],'Exact original R3 asset inventory required before rename'
    assert frozen['scope']=='EXACT_R3_ASSET_CASE_PATH_ONLY_PLAN'
    by={r['file']:r for r in before};groups=collisions(before);assert groups==frozen['collision_file_groups']
    moves=frozen['renames'];assert len(moves)==len({r['before'] for r in moves})==len({r['after'].casefold() for r in moves})
    assert {r['before'] for r in moves}=={p for group in groups for p in group}
    mapping={r['before']:r['after'] for r in moves};expected=sorted([dict(r,file=mapping.get(r['file'],r['file'])) for r in before],key=lambda r:r['file'])
    assert expected==frozen['after_inventory'] and not collisions(expected),'Validate full translated inventory before any move'
    for row in frozen['renames']:
        src=root/row['before'];dst=root/row['after'];assert src.resolve().is_relative_to(root) and dst.parent.resolve().is_relative_to(root) and not dst.exists()
        assert src.stat().st_size==row['bytes']==by[row['before']]['bytes'] and sha_file(src)==row['sha256']==by[row['before']]['sha256']
        assert src.name==dst.name and dst.parent.name=='__r3guid_'+row['guid'] and dst.parent.parent==src.parent
        meta=src if src.name.endswith('.meta') else Path(str(src)+'.meta');found=re.findall(rb'^guid: ([0-9a-fA-F]{32})\s*$',meta.read_bytes(),re.M)
        assert len(found)==1 and found[0].decode().lower()==row['guid']
    # All validation above completes before the first move.
    for row in frozen['renames']:
        dst=root/row['after'];dst.parent.mkdir(exist_ok=True);(root/row['before']).rename(dst)
    after=inventory(root);assert after==frozen['after_inventory'] and not collisions(after)
    return dict(scope='APPLIED_EXACT_R3_CASE_PATH_ONLY_RENAME',renamed_files=len(frozen['renames']),renamed_assets=frozen['renamed_assets'],all_asset_and_meta_byte_hashes_preserved=True,all_guids_and_internal_names_preserved=True,non_target_file_bytes_and_paths_unchanged=True,case_collision_groups_after=0,new_game_dll_changes=0,unity_build_accepted=False)

if __name__=='__main__':
    p=argparse.ArgumentParser();p.add_argument('--project',required=True);p.add_argument('--output',required=True);p.add_argument('--candidate');p.add_argument('--apply-plan');a=p.parse_args();out=Path(a.output);out.mkdir(exist_ok=True,parents=True)
    if a.apply_plan:
        frozen=json.loads(Path(a.apply_plan).read_bytes());receipt=apply(a.project,frozen);write(out/'asset-case-transition.json',receipt);print(json.dumps(receipt))
    else:
        try:result=plan(a.project,a.candidate)
        except Exception as ex:
            write(out/'ASSET-CASE-BLOCKER.json',dict(scope='SOURCE_ONLY_BLOCKER_NO_UNITY_BUILD',error=repr(ex),new_game_changes=0));raise
        write(out/'ASSET-CASE-PLAN.json',result)
        with zipfile.ZipFile(out/'ORIGINAL-COLLIDING-ASSETS.zip','w',zipfile.ZIP_DEFLATED) as z:
            for row in result['renames']:z.write(Path(a.project)/row['before'],row['before'])
        print(json.dumps({k:v for k,v in result.items() if k not in ['before_inventory','after_inventory','renames']}))
