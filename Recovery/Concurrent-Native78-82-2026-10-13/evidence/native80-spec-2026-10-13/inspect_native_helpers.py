"""Disassemble source helper fragments and original native exception metadata."""
from pathlib import Path
import sys,struct,json,hashlib
R=Path(__file__).resolve().parents[2]
sys.path.insert(0,str(R/'audit/stage9-native3/python-deps'))
import pefile
from capstone import Cs,CS_ARCH_X86,CS_MODE_64
pe=pefile.PE(str(R/'audit/stage9-native3/inputs/GameAssembly.dll'));base=pe.OPTIONAL_HEADER.ImageBase
md=Cs(CS_ARCH_X86,CS_MODE_64)
for va,size in [(0x1808197f0,64),(0x180669280,200),(0x18046dc80,128),(0x180825710,128),(0x180302170,1),(0x180358548,43)]:
 raw=pe.get_data(va-base,size)
 print('HELPER_FRAGMENT',json.dumps({'va':hex(va),'size':size,'bytes':raw.hex(),'sha256':hashlib.sha256(raw).hexdigest(),'scope':'source fragment, not acceptance execution'}))
 for i in md.disasm(raw,va):print(hex(i.address),i.mnemonic,i.op_str)
for va in [0x1803583f0,0x180323840]:
 e=next(e for e in pe.DIRECTORY_ENTRY_EXCEPTION if e.struct.BeginAddress<=va-base<e.struct.EndAddress)
 u=e.struct.UnwindData;d=pe.get_data(u,64);v,pro,cnt,fr=struct.unpack('<BBBB',d[:4]);ofs=4+((cnt+1)&~1)*2
 out={'start':hex(e.struct.BeginAddress+base),'end':hex(e.struct.EndAddress+base),'unwind_info':hex(u+base),'unwind_flags':v>>3,'unwind_version':v&7,'unwind_codes':cnt,'unwind_info_bytes':d.hex()}
 if v>>3&3:
  handler,func=struct.unpack_from('<II',d,ofs)
  out.update({'native_handler':hex(base+handler),'funcinfo_address':hex(base+func),'funcinfo_first_32_bytes':pe.get_data(func,32).hex(),'note':'Compressed native FuncInfo not interpreted as the old fixed-width FuncInfo3 layout.'})
 print('EXCEPTION_METADATA',json.dumps(out))
for va,size,code_end in [(0x1803583f0,419,0x180358593),(0x180323840,484,0x180323a00)]:
 print('TARGET_DISASSEMBLY',hex(va),'code_fragment_end',hex(code_end),'full_recorded_range_bytes',size)
 for i in md.disasm(pe.get_data(va-base,code_end-va),va):print(hex(i.address),i.mnemonic,i.op_str)
 if code_end<va+size:print('EMBEDDED_SWITCH_BYTES',pe.get_data(code_end-base,va+size-code_end).hex())
