from pathlib import Path
import copy,json,verify_types as V
types={'Damage':{'value':False},'Other':{'value':False},'System.Object':{'value':False},'System.String':{'value':False},'Value':{'value':True},'T':{'unbound':True},'Damage&':{'byref':True}}
def method(arg='Damage&',ret='Damage',ops=None):
 return {'name':'ldind.ref coverage','token':'control','instructions':ops or [{'offset':0,'opcode':'ldarg.0','operand':None},{'offset':1,'opcode':'ldind.ref','operand':None},{'offset':2,'opcode':'ret','operand':None}],'owner':'Other','hasThis':False,'args':[arg],'locals':[],'handlers':[],'ret':ret,'maxStack':1}
rows=[]
for label,arg,ret,pass_expected in [('damage-ref','Damage&','Damage',True),('object-ref','System.Object&','System.Object',True),('string-ref','System.String&','System.Object',True),('array-ref','Damage[]&','Damage[]',True),('integer-ref','System.Int32&','Damage',False),('value-ref','Value&','Damage',False),('native-address','System.IntPtr','Damage',False),('unmanaged-pointer','Damage*','Damage',False),('object-value','Damage','Damage',False),('unbound-ref','T&','Damage',False),('pointer-pointer','Damage&&','Damage',False),('wrong-return','Damage&','Other',False)]:
 ok=True;error=None
 try:V.verify(method(arg,ret),types)
 except V.InvalidIL as e:ok=False;error=str(e)
 assert ok==pass_expected,(label,ok,error)
 rows.append({'name':label,'expected_pass':pass_expected,'actual_pass':ok,'error':error})
for label,ops in [('underflow',[{'offset':0,'opcode':'ldind.ref','operand':None},{'offset':1,'opcode':'ret','operand':None}]),('primitive-op-remains-unsupported',[{'offset':0,'opcode':'ldarg.0','operand':None},{'offset':1,'opcode':'ldind.i4','operand':None},{'offset':2,'opcode':'ret','operand':None}])]:
 try:V.verify(method(ops=ops),types);raise AssertionError(label+' accepted')
 except V.InvalidIL as e:rows.append({'name':label,'expected_pass':False,'actual_pass':False,'error':str(e)})
print(json.dumps({'controls':rows,'pass':True,'scope':'Managed reference elements only; unmanaged addresses and remaining unsupported opcodes still reject'},indent=2))
