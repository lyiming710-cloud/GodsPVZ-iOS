"""Negative controls on the actual Linux R3 snapshot before applying a path-only plan."""
from pathlib import Path
import argparse,copy,json
from normalize_startup_r20_asset_case import apply,inventory,write

if __name__=='__main__':
    p=argparse.ArgumentParser();p.add_argument('--project',required=True);p.add_argument('--plan',required=True);p.add_argument('--output',required=True);a=p.parse_args();root=Path(a.project).resolve();frozen=json.loads(Path(a.plan).read_bytes());before=inventory(root);assert frozen['before_inventory']==before and frozen['renames'];results=[]
    for kind in ['wrong_source_hash','missing_after_entry','escaping_destination','changed_basename']:
        trial=copy.deepcopy(frozen)
        if kind=='wrong_source_hash':trial['renames'][0]['sha256']='0'*64
        elif kind=='missing_after_entry':trial['after_inventory'].pop()
        else:
            row=trial['renames'][0];old=row['after'];new='../../outside' if kind=='escaping_destination' else old+'.wrong';row['after']=new
            for item in trial['after_inventory']:
                if item['file']==old:item['file']=new
            trial['after_inventory'].sort(key=lambda r:r['file'])
        failure=None
        try:apply(root,trial)
        except AssertionError as ex:failure=repr(ex)
        assert failure and inventory(root)==before,('CONTROL_CHANGED_ORIGINAL_FILES',kind)
        results.append(dict(name=kind,rejected_before_any_move=True,error=failure))
    receipt=apply(root,frozen);assert inventory(root)==frozen['after_inventory']
    write(Path(a.output)/'ASSET-CASE-CONTROLS.json',dict(scope='REAL_LINUX_SOURCE_PATH_AND_INTEGRITY_CONTROLS_NO_UNITY',controls=results,rejected=len(results),positive_application=receipt,new_game_changes=0));print(json.dumps(receipt))
