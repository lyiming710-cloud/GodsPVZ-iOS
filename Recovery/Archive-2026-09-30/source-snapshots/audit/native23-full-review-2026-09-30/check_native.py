from pathlib import Path
import struct,json,hashlib,sys,collections
base=Path(__file__).resolve().parents[1]
sys.path.insert(0,str(base/'stage9-native4/python-deps'))
import pefile,capstone
path=base/'stage9-native4/inputs/GameAssembly.dll';binary=path.read_bytes();pe=pefile.PE(data=binary)
imagebase=pe.OPTIONAL_HEADER.ImageBase
table=pe.OPTIONAL_HEADER.DATA_DIRECTORY[3];o=pe.get_offset_from_rva(table.VirtualAddress)
entries=[struct.unpack_from('<III',binary,k) for k in range(o,o+table.Size-11,12)]
def unwind(e,seen=()):
 b,end,u=e
 if u in seen:return {'cycle':True}
 off=pe.get_offset_from_rva(u);vflags,prolog,codes,frame=struct.unpack_from('<BBBB',binary,off);flags=vflags>>3
 d={'begin':hex(imagebase+b),'end':hex(imagebase+end),'unwind':hex(imagebase+u),'version':vflags&7,'flags':flags,'code_count':codes}
 if flags&4:
  chain=struct.unpack_from('<III',binary,off+4+((codes+1)&~1)*2)
  d['chained']=unwind(chain,seen+(u,));d['root']=d['chained'].get('root',d['chained'].get('begin'))
 else:d['root']=d['begin']
 return d
decoded=[unwind(e) for e in entries];bybegin={d['begin']:d for d in decoded}
methods=json.loads((base/'stage9-native4/evidence/native-method-map.json').read_text());byva=collections.defaultdict(list)
for m in methods:byva[int(m['va'],16)].append(m)
cs=capstone.Cs(capstone.CS_ARCH_X86,capstone.CS_MODE_64);cs.detail=True
targets=[]
for tok in (0x06000348,0x0600034C,0x06000339,0x06000338,0x060000D9,0x060005C8):
 m=next(x for x in methods if x['image']=='Assembly-CSharp.dll' and int(x['token'],16)==tok);va=int(m['va'],16);start=bybegin.get(hex(va));family=[d for d in decoded if d['root']==hex(va)]
 assert start
 ix=entries.index(next(e for e in entries if imagebase+e[0]==va));end=entries[ix][1];j=ix
 while j+1<len(entries) and entries[j+1][0]==end:j+=1;end=entries[j][1]
 adjacent=decoded[ix:j+1]
 spans=family if len(family)>1 else [start]
 asm=[];calls=[]
 for span in spans:
  begin=int(span['begin'],16);stop=int(span['end'],16);off=pe.get_offset_from_rva(begin-imagebase)
  for i in cs.disasm(binary[off:off+stop-begin],begin):
   row={'va':hex(i.address),'op':i.mnemonic,'args':i.op_str}
   if i.mnemonic in ('call','jmp') and i.operands and i.operands[0].type==capstone.x86.X86_OP_IMM:
    dest=i.operands[0].imm;row['destination']=hex(dest);row['mapped']=byva.get(dest,[]);calls.append(row)
   asm.append(row)
 targets.append({'method':m,'first_unwind':start,'chained_family':family,'adjacent_algorithm':adjacent,'adjacent_all_share_root':all(x['root']==hex(va) for x in adjacent),'other_method_entries_in_adjacent':[x for addr in byva if va<addr<imagebase+end for x in byva[addr] if x['image']=='Assembly-CSharp.dll'],'assembly':asm,'calls':calls})
fields=json.loads((base/'stage9-native4/evidence/native-fields.json').read_text())
result={'pc_sha256':hashlib.sha256(binary).hexdigest(),'targets':targets,'fields':[f for f in fields if f['type'] in ('Plant','Device','Board','Grid')],'note':'Ranges follow decoded UNW_FLAG_CHAININFO root membership, not address adjacency.'}
(Path(__file__).parent/'native-independent.json').write_text(json.dumps(result,indent=2))
print(json.dumps({'sha256':result['pc_sha256'],'targets':[{'method':t['method'],'family':len(t['chained_family']),'adjacent':len(t['adjacent_algorithm']),'same_root':t['adjacent_all_share_root'],'other_method_entries':t['other_method_entries_in_adjacent']} for t in targets]},indent=2))
