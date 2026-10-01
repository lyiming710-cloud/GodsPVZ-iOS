from pathlib import Path
import json,hashlib,subprocess,bisect,re
w=Path('/workspaces/GodsPVZ-native24-codespace');old=Path('/workspaces/GodsPVZ-native19');base=w/'.validation/native26-2026-10-01';out=w/'.validation/native27-2026-10-01';native=old/'.validation/native22/pcnative';queue=json.loads((out/'INITIAL-33-REVIEW-QUEUE.json').read_text())['candidates'];data=json.loads((base/'all-methods.json').read_text());methods={m['token']:m for m in data['methods']}
def sha(p):return hashlib.sha256(p.read_bytes()).hexdigest()
assert sha(native/'GameAssembly.dll')=='9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d'
scope={'__name__':'native27_reference'};exec((w/'scripts/codespaces/native23_native.py').read_text(),scope);pe=scope['PE'](native/'GameAssembly.dll');mapping=json.loads((native/'native-method-map.json').read_text());fields=json.loads((native/'native-fields.json').read_text());starts=sorted({int(r['va'],16) for r in mapping if int(r['va'],16)>0});pdata=pe.pdata_entries();rows=[];folder=out/'native';folder.mkdir(exist_ok=True)
for entry in queue:
 token=entry['token'];hit=[r for r in mapping if r['image']=='Assembly-CSharp.dll' and int(r['token'],16)==int(token,16)];assert len(hit)==1,(token,hit);record=hit[0];va=int(record['va'],16);limit=starts[bisect.bisect_right(starts,va)];first=next(i for i,x in enumerate(pdata) if x[0]==va);end=pdata[first][1];parts=[pdata[first]]
 for begin,stop in pdata[first+1:]:
  if begin!=end or begin>=limit or stop>limit:break
  end=stop;parts.append((begin,stop))
 assert end<=limit;raw=pe.data[pe.va_to_offset(va):pe.va_to_offset(va)+end-va];path=folder/(token+'.bin');path.write_bytes(raw);asm=subprocess.check_output(['objdump','-D','-b','binary','-m','i386:x86-64','--adjust-vma='+hex(va),str(path)],text=True);(folder/(token+'.asm')).write_text(asm)
 calls=[]
 for line in asm.splitlines():
  found=re.search(r'\b(call|jmp)\s+(?:0x)?([0-9a-f]{8,})\b',line)
  if found:
   dest=int(found[2],16);hits=[r for r in mapping if int(r['va'],16)==dest];calls.append({'instruction':line.strip(),'va':hex(dest),'methods':hits})
 relevant_fields=[]
 for i in methods[token]['instructions']:
  if i['opcode'] not in ['ldfld','ldsfld','stfld','stsfld']:continue
  f=i['operand'];identity=f['identity'];name=identity.rsplit('::',1)[-1];owner=f['owner'].rsplit('.',1)[-1];owner=owner.rsplit('/',1)[-1];matching=[r for r in fields if r['type']==owner and r['name']==name];relevant_fields.append({'cil_offset':i['offset'],'field':f,'original_field_records':matching})
 rows.append({'token':token,'method':entry['method'],'record':record,'start':hex(va),'end':hex(end),'next_mapped_method':hex(limit),'pdata_parts':[[hex(a),hex(b)] for a,b in parts],'body_bytes':len(raw),'native_sha256':hashlib.sha256(raw).hexdigest(),'exception_helper_calls':sum(c['va']=='0x180250150' for c in calls),'calls':calls,'fields':relevant_fields,'disassembly':asm,'managed_instructions':methods[token]['instructions'],'recipe':entry,'native_site_verdict':'PENDING_REVIEW'})
report={'input_sha256':sha(base/'native26-final1.dll'),'GameAssembly_sha256':sha(native/'GameAssembly.dll'),'method_map_sha256':sha(native/'native-method-map.json'),'field_map_sha256':sha(native/'native-fields.json'),'boundary_scope':'Contiguous pdata bounded by next distinct mapped PC method pointer; separate funclets are not proved by this rule alone','methods':rows};(out/'NATIVE-BATCH-EVIDENCE.json').write_text(json.dumps(report,indent=2));print(json.dumps(report,indent=2))
