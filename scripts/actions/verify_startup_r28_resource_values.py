"""Read back actual Unity serialization; constructor-looking defaults are not PASS.
Only compares the fixed original source plans to returned persistent objects.
Does not certify gameplay, trajectories, attacks, device input or an IPA.
"""
from pathlib import Path
import argparse,hashlib,json,re,struct
EXPECTED='9081408a2226a79e23991355108540a0260a3966a500cecf4524f3cba9dfb364'
PLANT='82e46ff8f080a25bb837cd557692ed586da1c7e42d22f2c45401c99992ba3e5b'
ZOMBIE='7ce35a7d8cf476845104dec8d45713b06096398b443f84002e963f9d4274a2ad'
IDENTITIES='22b1f6b91d951ec906c01e03a54a0340534aa39025f7c171e0cfc501e51c26e8'
sha=lambda b:hashlib.sha256(b).hexdigest()

def fixed(path,expected):
 b=Path(path).read_bytes()
 if sha(b)!=expected:raise ValueError('Unreviewed source input: '+str(path))
 return json.loads(b)

def compare(expected,actual,path,guid,passed,failed,unresolved):
 if isinstance(expected,dict)and'fileID'in expected:
  if not expected['fileID']:
   ok=actual is None or isinstance(actual,dict)and(actual.get('instanceID')==0 or actual.get('fileID')==0)
  elif isinstance(actual,dict)and'fileID'in actual:
   ok=actual['fileID']==expected['fileID']and actual.get('guid')==expected.get('guid',guid)and actual.get('type')==3
  elif isinstance(actual,dict)and actual.get('instanceID')==0:ok=False
  else:unresolved.append(dict(path=path,expected=expected,actual=actual,reason='PERSISTENT_GUID_OR_LOCAL_ID_UNOBSERVED'));return
  (passed if ok else failed).append(dict(path=path,kind='reference',expected=expected,expected_local_guid=guid,actual=actual));return
 if isinstance(expected,dict):
  if not isinstance(actual,dict):failed.append(dict(path=path,kind='wrong_object_shape',actual=actual));return
  for k,v in expected.items():
   if k not in actual:failed.append(dict(path=path+'.'+k,kind='missing_serialized_field'))
   else:compare(v,actual[k],path+'.'+k,guid,passed,failed,unresolved)
  return
 if isinstance(expected,list):
  if not isinstance(actual,list)or len(actual)!=len(expected):failed.append(dict(path=path,kind='array_length',expected=len(expected),actual=actual));return
  passed.append(dict(path=path,kind='array_length',count=len(expected)))
  for i,v in enumerate(expected):compare(v,actual[i],path+'['+str(i)+']',guid,passed,failed,unresolved)
  return
 if isinstance(expected,float):
  try:ok=struct.pack('<f',expected)==struct.pack('<f',float(actual))
  except (TypeError,ValueError,OverflowError,struct.error):ok=False
 else:ok=expected==actual
 (passed if ok else failed).append(dict(path=path,kind='scalar_f32'if isinstance(expected,float)else'scalar',expected=expected,actual=actual))

def verify(probe,targets):
 if probe['candidate_sha256']!=EXPECTED:raise ValueError('Wrong actual candidate')
 rows=[];missing=[]
 for owner,wanted,guid in targets:
  for route in ['AssetDatabase','Resources']:
   actual=[o for o in probe['objects']if o['phase']=='after-force-import'and o['route']==route and o['type']==owner and o['path'].lower()==wanted['source_path'].lower()]
   if len(actual)!=1:missing.append(dict(owner=owner,resource=wanted['resource'],route=route,actual_object_count=len(actual)));continue
   o=actual[0];passed=[];failed=[];unresolved=[]
   if not re.search(r' guid='+guid+r' localID='+str(wanted['source_owner'])+r'$',o['identity']):failed.append(dict(path='<owner>',kind='WRONG_OWNER_IDENTITY',actual=o['identity']))
   try:
    actual=json.loads(o['editor_json']);compare(wanted['fields'],actual.get('MonoBehaviour',actual),wanted['resource'],guid,passed,failed,unresolved)
   except (ValueError,TypeError)as e:failed.append(dict(path='<editor_json>',kind='INVALID_ACTUAL_DATA',error=repr(e)))
   rows.append(dict(owner=owner,resource=wanted['resource'],route=route,original_fields=len(wanted['fields']),pass_fields=not failed and not unresolved,passed_leaf_checks=len(passed),failed_leaf_checks=len(failed),unresolved_reference_checks=len(unresolved),checks=passed,failures=failed,unresolved=unresolved))
 complete=probe['collection_complete']and not probe.get('exceptions')and probe['source_bytes_unchanged']and probe['game_dll_unchanged']and len(rows)==len(targets)*2 and not missing
 return dict(scope='ACTUAL_UNITY_RESOURCE_SERIALIZED_FIELD_READBACK_ONLY_NOT_GAMEPLAY_OR_IPAD',candidate_sha256=EXPECTED,collection_complete=complete,expected_resource_routes=len(targets)*2,actual_resource_routes=len(rows),all_actual_fields_match_original=complete and all(r['pass_fields']for r in rows),passed_leaf_checks=sum(r['passed_leaf_checks']for r in rows),failed_leaf_checks=sum(r['failed_leaf_checks']for r in rows),unresolved_reference_checks=sum(r['unresolved_reference_checks']for r in rows),missing_objects=missing,objects=rows,gameplay_proven=False,qualified_methods_added=0)

if __name__=='__main__':
 p=argparse.ArgumentParser();p.add_argument('--probe',required=True);p.add_argument('--plant-plan',required=True);p.add_argument('--plant-identities',required=True);p.add_argument('--zombie-plan');p.add_argument('--output',required=True);a=p.parse_args();output=Path(a.output)
 if output.exists():raise ValueError('Never overwrite an actual readback report')
 plants=fixed(a.plant_plan,PLANT);registry=fixed(a.plant_identities,IDENTITIES);identities={r['source_path']:r['guid']for r in registry['entries']};targets=[('Plant',r,identities[r['source_path']])for r in plants['plans']]
 if a.zombie_plan:
  zombies=fixed(a.zombie_plan,ZOMBIE);targets += [('Zombie',r,r['source_guid'])for r in zombies['plans']]
 raw=Path(a.probe).read_bytes();result=verify(json.loads(raw),targets);result.update(actual_probe_sha256=sha(raw),original_plant_plan_sha256=PLANT,original_zombie_plan_sha256=ZOMBIE if a.zombie_plan else None);output.write_bytes((json.dumps(result,indent=2)+'\n').encode());print(json.dumps({k:v for k,v in result.items()if k not in['objects','missing_objects']}));raise SystemExit(0 if result['all_actual_fields_match_original']else 79)
