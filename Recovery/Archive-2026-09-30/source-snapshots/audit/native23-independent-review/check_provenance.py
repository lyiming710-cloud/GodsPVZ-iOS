"""Independent synthetic probes; never reads or writes a candidate DLL."""
from pathlib import Path
import sys,json
sys.dont_write_bytecode=True
root=Path(__file__).resolve().parents[1]/'native22-work/scripts/codespaces'
sys.path.insert(0,str(root))
import native23_prov as P
import native23_afam3 as C
T={'Owner':{'baseType':'System.Object'},'GameObject':{'baseType':'System.Object'},'System.Object':{},'System.Int32':{'value':True},'System.Boolean':{'value':True},'System.String':{},'System.Void':{'value':True}}
def body(name,ops,locals=(),args=()):
    return {'token':'0x06000001','name':name,'owner':'Owner','hasThis':False,'args':list(args),'locals':list(locals),'ret':'System.Void','handlers':[],'maxStack':8,'instructions':[{'offset':2*n,'opcode':op,'operand':arg} for n,(op,arg) in enumerate(ops)]}
field={'owner':'Owner','type':'GameObject','identity':'GameObject Owner::f'}
cases=[
body('invalid store -> local receiver -> FIELD', [('ldc.i4.1',None),('stloc',{'index':0}),('ldloc',{'index':0}),('ldfld',field),('ldc.i4.0',None),('ceq',None),('pop',None),('ret',None)],locals=['Owner']),
body('invalid starg -> ARG', [('ldc.i4.1',None),('starg',{'index':0}),('ldarg',{'index':0}),('ldc.i4.0',None),('ceq',None),('pop',None),('ret',None)],args=['GameObject']),
body('invalid call argument -> CALLRET -> FIELD', [('ldc.i4.1',None),('call',{'owner':'Owner','hasThis':False,'args':['System.String'],'ret':'Owner','name':'GetOwner','identity':'Owner Owner::GetOwner(System.String)'}),('ldfld',field),('ldc.i4.0',None),('ceq',None),('pop',None),('ret',None)]),
body('incompatible merge -> accepted predecessor survives', [('ldarg',{'index':0}),('brtrue',{'target':8}),('ldarg',{'index':1}),('br',{'target':10}),('ldc.i4.1',None),('ldfld',field),('ldc.i4.0',None),('ceq',None),('pop',None),('ret',None)],args=['System.Boolean','Owner'])]
out=[]
for d in cases:
    errors=P.verify_prov(d,T);sol=C.solve(d,T)
    result={'case':d['name'],'errors':errors,'solver':sol}
    out.append(result);print(json.dumps(result,ensure_ascii=False))
(Path(__file__).parent/'provenance-counterexamples.json').write_text(json.dumps(out,indent=2),encoding='utf-8')
