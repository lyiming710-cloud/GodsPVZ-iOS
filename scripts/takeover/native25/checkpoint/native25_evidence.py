from pathlib import Path
import json,hashlib,subprocess,struct,bisect
w=Path('/workspaces/GodsPVZ-native24-codespace');old=Path('/workspaces/GodsPVZ-native19');n=w/'.validation/native25-2026-10-01';native=old/'.validation/native22/pcnative';dll=native/'GameAssembly.dll';mp=native/'native-method-map.json'
def sha(p):return hashlib.sha256(Path(p).read_bytes()).hexdigest()
assert sha(dll)=='9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d'
scope={'__name__':'native25_reference'};exec((w/'scripts/codespaces/native23_native.py').read_text(),scope);pe=scope['PE'](dll);records=json.loads(mp.read_text());known=sorted({int(r['va'],16) for r in records if int(r['va'],16)>0});entries=pe.pdata_entries();results=[]
for token in [0x06000338,0x060000D9,0x060005C8]:
 hits=[r for r in records if r['image']=='Assembly-CSharp.dll' and int(r['token'],16)==token];assert len(hits)==1
 record=hits[0];va=int(record['va'],16);limit=known[bisect.bisect_right(known,va)];first=next(i for i,x in enumerate(entries) if x[0]==va);end=entries[first][1];parts=[entries[first]]
 for start,stop in entries[first+1:]:
  if start!=end or start>=limit or stop>limit:break
  parts.append((start,stop));end=stop
 assert end<=limit
 raw=pe.data[pe.va_to_offset(va):pe.va_to_offset(va)+end-va];p=n/('native-'+f'{token:08X}'+'.bin');p.write_bytes(raw)
 cmd=['objdump','-D','-b','binary','-m','i386:x86-64','--adjust-vma='+hex(va),str(p)];text=subprocess.check_output(cmd,text=True);(n/(f'native-{token:08X}'+'.asm')).write_text(text)
 calls=[]
 import re
 for line in text.splitlines():
  match=re.search(r'\b(?:call|jmp)\s+(?:0x)?([0-9a-f]{8,})\b',line)
  if match:
   dest=int(match[1],16);names=[r for r in records if int(r['va'],16)==dest];calls.append({'instruction':line.strip(),'va':hex(dest),'metadata_targets':names})
 results.append({'record':record,'start':hex(va),'end':hex(end),'next_mapped_method':hex(limit),'pdata_fragments':[(hex(a),hex(b)) for a,b in parts],'native_body_sha256':hashlib.sha256(raw).hexdigest(),'calls':calls,'disassembly':text})
report={'GameAssembly_sha256':sha(dll),'native_map_sha256':sha(mp),'boundary_scope':'Contiguous pdata limited by next distinct metadata method pointer; does not alone prove noncontiguous funclets','methods':results};(n/'NATIVE-EVIDENCE.json').write_text(json.dumps(report,indent=2));print(json.dumps(report,indent=2))
