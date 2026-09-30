exec((__import__('pathlib').Path(__file__).parent/'audit_generic_remaining.py').read_text())
from capstone.x86 import X86_OP_MEM,X86_REG_RIP
md=Cs(CS_ARCH_X86,CS_MODE_64);md.detail=True
def method_name(idx):
 p=mo+idx*36;return types[u(meta,p+4)]['name']+'::'+st(u(meta,p))
for va,size in [(0x1804c4f70,729),(0x1809d3850,570),(0x1809d52c0,215)]:
 print('\nANNOTATE',hex(va))
 for ins in md.disasm(n(va,size),va):
  for operand in ins.operands:
   if operand.type!=X86_OP_MEM or operand.mem.base!=X86_REG_RIP:continue
   addr=ins.address+ins.size+operand.mem.disp
   if len(n(addr,8))!=8:continue
   val=q(addr);kind=val>>29;idx=(val&0x1fffffff)>>1
   detail=''
   try:
    if kind==1:
     tp=q(q(reg+56)+8*idx);definition=q(tp);detail='TYPE '+types[definition]['name'] if definition<len(types) else ''
    elif kind==3:detail='METHOD '+method_name(idx)
    elif kind==5:
     p=u(meta,8)+idx*8;size=u(meta,p);off=u(meta,p+4);detail='STRING '+repr(meta[u(meta,16)+off:u(meta,16)+off+size].decode())
    elif kind==6:detail='METHODSPEC '+str(idx)+' '+method_name(specs[idx][0])+' '+str(specs[idx])
    if not detail and ins.mnemonic in ['addss','subss','mulss']:detail='FLOAT '+str(struct.unpack('<f',n(addr,4))[0])
   except Exception as e:detail=str(e)
   if detail:print(hex(ins.address),hex(addr),hex(val),detail)
maps=json.loads((R/'stage9-native4/evidence/native-method-map.json').read_text())
print('CALLS',*[x for x in maps if x['va'] in ['0x18031b790','0x18030ec90','0x18030da70','0x1812fb400','0x1812fb450']],sep='\n')
fields=json.loads((R/'stage9-native4/evidence/native-fields.json').read_text())
print('FIELDS',*[x for x in fields if (x['type']=='Plant' and x['offset'] in [0x20,0x24,0x1e8])or x['type']=='SwfAssocList`1' or (x['type']=='Board' and x['offset']==0x28)],sep='\n')
