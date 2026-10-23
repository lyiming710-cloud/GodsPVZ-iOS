"""Read candidate CIL only, without executing any method."""
from pathlib import Path
import sys,json,hashlib
ROOT=Path(__file__).resolve().parents[2]
sys.path.insert(0,str(ROOT/'audit/stage9-native3/python-deps'))
import dnfile
from dncil.cil.body import CilMethodBody
from dncil.cil.body.reader import CilMethodBodyReaderBytes
p=ROOT/'audit/bulk250-r2-evidence-39731970762/bulk250-r2-run/candidates/candidate1.dll'
raw=p.read_bytes()
if hashlib.sha256(raw).hexdigest()!='4732b0c1ee64be4224649ee43c88ddd805f5e1ea461e43ecf52febd7a756f903':raise ValueError('Candidate identity')
pe=dnfile.dnPE(data=raw);md=pe.net.mdtables
out=Path(__file__).resolve().parent/'cil';out.mkdir(exist_ok=True)
tokens=[int(x,16) for x in sys.argv[1:]]
for token in tokens:
 m=md.MethodDef.rows[(token&0xFFFFFF)-1];body=CilMethodBody(CilMethodBodyReaderBytes(pe.get_data(m.Rva,32768)))
 text=[f'0x{token:08X} {m.Name} locals={body.local_var_sig_tok}']
 for i in body.instructions:
  op=i.operand;detail=str(op)
  if hasattr(op,'value'):
   tab,rid=op.value>>24,op.value&0xFFFFFF
   if tab in [4,6,10]:detail+=' '+str({4:md.Field,6:md.MethodDef,10:md.MemberRef}[tab].rows[rid-1].Name)
   elif tab==0x70:detail+=' '+repr(pe.net.user_strings.get(rid).value)
  text.append(f'{i.offset-body.header_size:04X} {i.opcode} {detail}')
 (out/f'{token:08X}.txt').write_text('\n'.join(text)+'\n',encoding='utf-8')
 print('\n'.join(text))
