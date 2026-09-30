import sys,json,pathlib,hashlib
root=pathlib.Path(__file__).resolve().parents[2]
sys.path.insert(0,str(root/'audit/stage9-native4/python-deps'))
import pefile,capstone
base=root/'audit/stage9-native4'
data=(base/'inputs/GameAssembly.dll').read_bytes(); pe=pefile.PE(data=data); ib=pe.OPTIONAL_HEADER.ImageBase
methods=json.loads((base/'evidence/native-method-map.json').read_text())
fields=json.loads((base/'evidence/native-fields.json').read_text())
byva={int(m['va'],16):m['type']+'::'+m['name'] for m in methods if m.get('va')}
targets=[m for m in methods if (m['type']=='ICEUIController' and m['name'] in ['Update','Broken']) or (m['type']=='Project' and m['name'] in ['Start','LoopAddAnimation'])]
out=pathlib.Path(__file__).parent/'review-evidence';out.mkdir(exist_ok=True)
md=capstone.Cs(capstone.CS_ARCH_X86,capstone.CS_MODE_64)
for m in targets:
 va=int(m['va'],16);rf=next((x.struct for x in pe.DIRECTORY_ENTRY_EXCEPTION if x.struct.BeginAddress<=va-ib<x.struct.EndAddress),None)
 end=min([int(x['va'],16) for x in methods if x['type']==m['type'] and int(x['va'],16)>va]+[va+0x3000])
 code=pe.get_data(va-ib,end-va)
 lines=[json.dumps(m),'PC_SHA256='+hashlib.sha256(data).hexdigest()]
 for i in md.disasm(code,va):
  label=''
  if i.mnemonic in ('call','jmp') and i.op_str.startswith('0x'):label=byva.get(int(i.op_str,16),'')
  lines.append(f'{i.address:016x} {i.mnemonic:9} {i.op_str:48} {label}')
 (out/(m['type']+'.'+m['name']+'.native.txt')).write_text('\n'.join(lines),encoding='utf8')
(out/'fields.json').write_text(json.dumps([f for f in fields if f['type'] in ['Project','ICEUIController']],indent=2))
print('Extracted',len(targets),'native methods to',out)
