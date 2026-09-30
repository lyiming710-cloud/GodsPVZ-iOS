import pathlib,sys,struct,json,hashlib
p=pathlib.Path(__file__).parent;sys.path.insert(0,str(p.parents[1]/'audit/stage9-native4/python-deps'))
import pefile
e=p/'review-evidence'
targets={0x6000418,0x600041e,0x6000427,0x6000429,0x600042f,0x6000430,0x6000432,0x6000433,0x6000434,0x6000436,0x6000437}
def read(n):
 data=(e/(n+'-unlinked.dll')).read_bytes();pe=pefile.PE(data=data);out={}
 for line in (e/(n+'.map.tsv')).read_text(encoding='utf-8-sig').splitlines():
  tok,rva,name=line.split('\t');tok=int(tok);rva=int(rva)
  if not rva:continue
  start=pe.get_offset_from_rva(rva);first=data[start];flags=0
  if first&3==2:hdr=1;size=first>>2
  else:flags=struct.unpack_from('<H',data,start)[0];hdr=(flags>>12)*4;size=struct.unpack_from('<I',data,start+4)[0]
  end=start+hdr+size
  if flags&8:
   pos=(end+3)&~3
   while True:
    kind=data[pos];secsize=int.from_bytes(data[pos+1:pos+4],'little') if kind&0x40 else data[pos+1]
    end=pos+secsize
    if not kind&0x80:break
    pos=(end+3)&~3
  out[tok]=(name,data[start:end],data[start+hdr:start+hdr+size])
 return out
a,b=read('native17'),read('native18');changes=[];codechanges=[]
for t in a:
 if t in targets:continue
 if a[t][1]!=b[t][1]:changes.append({'token':hex(t),'name':a[t][0],'before_bytes':len(a[t][1]),'after_bytes':len(b[t][1]),'raw_code_changed':a[t][2]!=b[t][2]})
 if a[t][2]!=b[t][2]:codechanges.append(hex(t))
r={'body_count':len(a),'non_target_count':len(a)-len(targets),'non_target_raw_body_changes':len(changes),'non_target_raw_cil_changes':len(codechanges),'changes':changes}
(e/'raw-comparison.json').write_text(json.dumps(r,indent=2));print(json.dumps({k:v for k,v in r.items() if k!='changes'},indent=2));print(json.dumps(changes[:5],indent=2))
def normalized(n):return {int(t): (h,name) for t,h,name in [x.split('\t') for x in (e/(n+'.normalized.tsv')).read_text(encoding='utf-8-sig').splitlines()]}
na,nb=normalized('native17'),normalized('native18'); changed=[{'token':hex(t),'name':na[t][1]} for t in na if na[t]!=nb[t]]
nr={'changed_count':len(changed),'non_target_changed':[x for x in changed if int(x['token'],16) not in targets],'changed':changed};(e/'normalized-comparison.json').write_text(json.dumps(nr,indent=2));print(json.dumps(nr,indent=2))
pc=pefile.PE(str(p.parents[1]/'audit/stage9-native4/inputs/GameAssembly.dll'));table={hex(rva):[hex(pc.OPTIONAL_HEADER.ImageBase+x) for x in struct.unpack('<6I',pc.get_data(rva,24))] for rva in [0x35ed74,0x35ed8c]};(e/'Die-jump-tables.json').write_text(json.dumps(table,indent=2));print(json.dumps(table))
fields=json.loads((e/'Zombie.fields.json').read_text());print(json.dumps([f for f in fields if f['offset'] in [0x30,0x34,0x38,0x148,0x1a0,0x1f0,0x200,0x210,0x220]],indent=2))
print('Ashe y additive constant',struct.unpack('<f',pc.get_data(0x35da20+0x124a10c,4))[0])
