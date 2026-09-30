from pathlib import Path
import sys,json,bisect
sys.dont_write_bytecode=True
base=Path(__file__).resolve().parents[1]
sys.path.insert(0,str(Path(__file__).parent/'evidence/original-sources'))
import native23_native as N
import native23_tiers as T
import native23_behave as B
out={'native_patterns':{s:bool(T.RE_TEST.match(s) or T.RE_CMPQ.match(s)) for s in ['test %al,%al','test %eax,%eax','cmpq $0x20,(%rax)']}}
d={'name':'field-write probe','owner':'Owner','hasThis':False,'args':['Owner'],'locals':[],'ret':'System.Void','handlers':[],'maxStack':8,'instructions':[
 {'offset':0,'opcode':'ldarg','operand':{'index':0}},
 {'offset':2,'opcode':'ldc.i4','operand':7},
 {'offset':4,'opcode':'stfld','operand':{'owner':'Owner','type':'System.Int32','identity':'System.Int32 Owner::n'}},
 {'offset':6,'opcode':'ret','operand':None}]}
types={'Owner':{'baseType':'System.Object'},'System.Object':{},'System.Int32':{'value':True},'System.Void':{'value':True}}
import random
m=B.Machine(d,types,B.WORLDS[0],set(),random.Random(0)).setup()
m.heap.append({'System.Int32 Owner::n':('I4',3,'init')});m.args[0]=('REF',0,'test');m.run()
out['interpreter_stfld']={'expected':7,'actual':m.heap[0]['System.Int32 Owner::n'][1],'status':m.status}
pe=N.PE(base/'stage9-native4/inputs/GameAssembly.dll');entries=pe.pdata_entries();begins=[x[0] for x in entries]
methods=json.loads((base/'stage9-native4/evidence/native-method-map.json').read_text())
own=[x for x in methods if x.get('image')=='Assembly-CSharp.dll' and x.get('va')]
byva={int(x['va'],16):x for x in own};vas=sorted(byva);examples=[]
for va in vas:
 i=bisect.bisect_left(begins,va)
 if i==len(entries) or begins[i]!=va:continue
 end=entries[i][1];j=i
 while j+1<len(entries) and entries[j+1][0]==end:j+=1;end=entries[j][1]
 nexti=bisect.bisect_right(vas,va)
 if nexti<len(vas) and vas[nexti]<end:
  a,b=byva[va],byva[vas[nexti]]
  examples.append({'method':a['type']+'::'+a['name'],'start':hex(va),'end':hex(end),'includes_next_method':b['type']+'::'+b['name'],'next':hex(vas[nexti])})
out['contiguous_pdata_cross_method_examples']=examples[:8]
out['contiguous_pdata_cross_method_count']=len(examples)
(Path(__file__).parent/'evidence-tool-probes.json').write_text(json.dumps(out,indent=2),encoding='utf-8')
print(json.dumps(out,indent=2))
