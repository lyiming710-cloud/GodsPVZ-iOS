from pathlib import Path
import json,hashlib,bisect,subprocess
w=Path('/workspaces/GodsPVZ-native24-codespace');base=w/'.validation/native28-2026-10-01';n=w/'.validation/native29-2026-10-01';native=Path('/workspaces/GodsPVZ-native19/.validation/native22/pcnative')
tokens=['0x0600019B','0x060008AA','0x060008AC','0x060008AE','0x060008B6','0x060008B7','0x0600082C','0x0600082D','0x060008A3','0x060008A1','0x06000820','0x06000160','0x06000161','0x06000162','0x0600015D','0x0600015E','0x0600015F','0x060000DF','0x0600013C','0x060000A8','0x060000AF','0x060000BB','0x060002C6','0x0600046C','0x060006D3','0x06000203','0x0600088B','0x0600088C','0x0600088D','0x0600088E','0x0600088F']
assert hashlib.sha256((native/'GameAssembly.dll').read_bytes()).hexdigest()=='9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d'
scope={'__name__':'native29_reader'};exec((w/'scripts/codespaces/native23_native.py').read_text(),scope);pe=scope['PE'](native/'GameAssembly.dll');mapping=json.loads((native/'native-method-map.json').read_text());fields=json.loads((native/'native-fields.json').read_text());starts=sorted({int(m['va'],16) for m in mapping if int(m['va'],16)>0});pdata=pe.pdata_entries();data=json.loads((base/'all-methods.json').read_text());methods={m['token']:m for m in data['methods']};cpp=json.loads((base/'CPP-METHODS.json').read_text());rows=[];folder=n/'native';folder.mkdir(exist_ok=True)
for token in tokens:
 hit=[r for r in mapping if r['image']=='Assembly-CSharp.dll' and int(r['token'],16)==int(token,16)];assert len(hit)==1,(token,hit);record=hit[0];va=int(record['va'],16);limit=starts[bisect.bisect_right(starts,va)];matching=[(i,x) for i,x in enumerate(pdata) if x[0]==va]
 if matching:
  first,entry=matching[0];end=entry[1];parts=[entry]
  for begin,stop in pdata[first+1:]:
   if begin!=end or begin>=limit or stop>limit:break
   end=stop;parts.append((begin,stop))
  boundary='contiguous pdata bounded by next mapped pointer'
 else:end=limit;parts=[];boundary='inspection window to next distinct mapped pointer; no pdata function-end claim'
 assert end<=limit and end-va<16000,(token,end-va);raw=pe.data[pe.va_to_offset(va):pe.va_to_offset(va)+end-va];p=folder/(token+'.bin');p.write_bytes(raw);asm=subprocess.check_output(['objdump','-D','-b','binary','-m','i386:x86-64','--adjust-vma='+hex(va),str(p)],text=True);(folder/(token+'.asm')).write_text(asm)
 ff=[]
 for i in methods[token]['instructions']:
  if i['opcode'] not in ['ldfld','stfld','ldsfld','stsfld','ldflda','ldsflda']:continue
  f=i['operand'];name=f['identity'].rsplit('::',1)[-1];owner=f['owner'].rsplit('.',1)[-1].rsplit('/',1)[-1];ff.append({'cil_offset':i['offset'],'field':f,'original_records':[r for r in fields if r['type']==owner and r['name']==name]})
 rows.append({'token':token,'method':methods[token]['name'],'record':record,'start':hex(va),'end':hex(end),'boundary_scope':boundary,'pdata_parts':parts,'native_sha256':hashlib.sha256(raw).hexdigest(),'disassembly':asm,'managed':methods[token],'fields':ff,'diagnostics_before':len(cpp['methods'][token]['errors'])})
report={'GameAssembly_sha256':hashlib.sha256((native/'GameAssembly.dll').read_bytes()).hexdigest(),'method_map_sha256':hashlib.sha256((native/'native-method-map.json').read_bytes()).hexdigest(),'field_map_sha256':hashlib.sha256((native/'native-fields.json').read_bytes()).hexdigest(),'candidate_input_sha256':hashlib.sha256((base/'native28-final1.dll').read_bytes()).hexdigest(),'methods':rows};(n/'NATIVE-EVIDENCE.json').write_text(json.dumps(report,indent=2));print(json.dumps(report))
