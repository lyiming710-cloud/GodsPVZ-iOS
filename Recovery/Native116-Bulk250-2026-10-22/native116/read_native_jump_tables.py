"""Decode original jump table data and string cells, with managed literal bindings. No execution."""
from pathlib import Path
import sys,json,struct
sys.dont_write_bytecode=True
ROOT=Path(__file__).resolve().parents[2];P=Path(__file__).resolve().parent
sys.path.insert(0,str(ROOT/'audit/stage9-native3/python-deps'))
import dnfile,pefile
from capstone import Cs,CS_ARCH_X86,CS_MODE_64
base=0x180000000
pe=pefile.PE(str(ROOT/'audit/offline-test-handoff-2026-10-14/payload/inputs/original-pc/GameAssembly.dll'))
managed=dnfile.dnPE(str(ROOT/'audit/native93-codex-2026-10-17/verification-2026-10-17/candidates/candidate1.dll'))
o=json.loads((P/'ORIGINAL-SOURCE.json').read_text(encoding='utf-8'));cells={int(x['address'],16):x for x in o['cells']}
heap=managed.net.user_strings;us={};p=1;raw=heap.__data__
while p<len(raw):
    v=raw[p];prefix=1
    if v&128==0:n=v
    elif v&192==128:n=((v&63)<<8)|raw[p+1];prefix=2
    else:n=((v&31)<<24)|(raw[p+1]<<16)|(raw[p+2]<<8)|raw[p+3];prefix=4
    if n:
        try:us.setdefault(heap.get(p).value,0x70000000+p)
        except Exception:pass
    p+=prefix+n
dec=Cs(CS_ARCH_X86,CS_MODE_64);rows=[]
for name,addr,count in [('Popup',0x1803A5CA0,15),('ArmorNames',0x1803921FC,7),('ArmorDescriptions',0x180392218,7)]:
    entries=struct.unpack('<'+'I'*count,pe.get_data(addr-base,count*4));result=[]
    for i,rva in enumerate(entries):
        ins=next(dec.disasm(pe.get_data(rva,12),base+rva));disp=struct.unpack('<i',ins.bytes[3:7])[0];cell=ins.address+ins.size+disp;x=cells.get(cell)
        result.append({'case':i,'target':hex(base+rva),'instruction_bytes':ins.bytes.hex(),'cell':hex(cell),'literal':x.get('text') if x else None,'managed_token':f'0x{us[x["text"]]:08X}' if x and x.get('text') in us else None})
    rows.append({'name':name,'table':hex(addr),'table_bytes':pe.get_data(addr-base,count*4).hex(),'entries':result})
for x in o['cells']:
    if 'text' in x:x['managed_token']=f'0x{us[x["text"]]:08X}' if x['text'] in us else None
out={'scope':'READ_ONLY_ORIGINAL_DATA_AND_METADATA','jump_tables':rows,'string_cells':[x for x in o['cells'] if 'text' in x]}
(P/'JUMP-TABLES-AND-LITERALS.json').write_text(json.dumps(out,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
print(json.dumps(rows,ensure_ascii=False,indent=2))
