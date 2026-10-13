"""Disassembly-only source inspection of native metadata and exception helpers."""
from pathlib import Path
import struct,sys,json,hashlib
R=Path(__file__).resolve().parents[2]
sys.path.insert(0,str(R/'audit/stage9-native3/python-deps'))
import pefile
from capstone import Cs,CS_ARCH_X86,CS_MODE_64
pe=pefile.PE(str(R/'audit/stage9-native3/inputs/GameAssembly.dll'));base=pe.OPTIONAL_HEADER.ImageBase
md=Cs(CS_ARCH_X86,CS_MODE_64)
for va,size in [(0x18024fef0,19),(0x180298710,7),(0x180232f00,285),(0x18024f340,5),(0x180265580,160),(0x180250150,10),(0x180209500,23),(0x1802094e0,20),(0x1802078e0,100)]:
 raw=pe.get_data(va-base,size)
 print('FRAGMENT',json.dumps({'va':hex(va),'size':size,'sha256':hashlib.sha256(raw).hexdigest(),'scope':'bounded source fragment, not independent MethodBody or test'}))
 for i in md.disasm(raw,va):print(hex(i.address),i.mnemonic,i.op_str)
print('METADATA_USAGE_JUMP_TABLE',[(i+1,hex(base+struct.unpack('<I',pe.get_data(0x233100+4*i,4))[0]))for i in range(7)])
for va in [0x1819a7ae8,0x1819a7998]:print('EXCEPTION_FACTORY_NAME',hex(va),pe.get_data(va-base,100).split(b'\0')[0].decode('ascii'))
