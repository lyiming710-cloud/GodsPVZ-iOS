using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
using Mono.Cecil;
using Mono.Cecil.Cil;

// Generates specifications first; writes a baseline-derived candidate only with an explicit output argument.
class SpecBuilder {
 static ModuleDefinition m;
 static string outdir;
 static bool build;
 static TypeDefinition T(string n){return m.Types.Single(t=>t.Name==n);}
 static FieldDefinition F(string t,string n){return T(t).Fields.Single(f=>f.Name==n);}
 static MethodDefinition M(string t,string n){return T(t).Methods.Single(f=>f.Name==n);}
 static MethodReference R(string t,string n){return m.GetMemberReferences().OfType<MethodReference>().First(f=>f.DeclaringType.FullName==t && f.Name==n);}
 static GenericInstanceType GI(TypeReference t,TypeReference e){var g=new GenericInstanceType(t);g.GenericArguments.Add(e);return g;}
 static TypeReference Enum(TypeReference e){return GI(((GenericInstanceType)R("System.Collections.Generic.List`1/Enumerator<Zombie>","get_Current").DeclaringType).ElementType,e);}
 static MethodReference Ref(TypeReference t,string n,TypeReference ret,params TypeReference[] a){var r=new MethodReference(n,ret,t){HasThis=true};foreach(var x in a)r.Parameters.Add(new ParameterDefinition(x));return r;}
 static MethodReference GetEnum(TypeReference list){var template=R("System.Collections.Generic.List`1<Zombie>","GetEnumerator");return Ref(list,"GetEnumerator",template.ReturnType);}
 static MethodReference Current(TypeReference en){return Ref(en,"get_Current",((GenericInstanceType)R("System.Collections.Generic.List`1/Enumerator<Zombie>","get_Current").DeclaringType).ElementType.GenericParameters[0]);}
 static MethodReference Move(TypeReference en){return Ref(en,"MoveNext",m.TypeSystem.Boolean);}
 static MethodReference Dispose(TypeReference en){return Ref(en,"Dispose",m.TypeSystem.Void);}
 static string Ty(TypeReference t){return t.FullName;}
 class B {
  public MethodDefinition md; public MethodBody body; public ILProcessor il;
  public Dictionary<Instruction,string> evidence=new Dictionary<Instruction,string>();
  public B(MethodDefinition x){md=x;body=new MethodBody(x){InitLocals=true,MaxStackSize=8};il=body.GetILProcessor();}
  public VariableDefinition V(TypeReference t){var v=new VariableDefinition(t);body.Variables.Add(v);return v;}
  public Instruction L(){return Instruction.Create(OpCodes.Nop);}
  public void Mark(Instruction l,string source){il.Append(l);if(source!=null)evidence[l]=source;}
  public void E(OpCode o){il.Append(Instruction.Create(o));}
  public void E(OpCode o,Instruction x){il.Append(Instruction.Create(o,x));}
  public void E(OpCode o,VariableDefinition x){il.Append(Instruction.Create(o,x));}
  public void E(OpCode o,MethodReference x){il.Append(Instruction.Create(o,x));}
  public void E(OpCode o,FieldReference x){il.Append(Instruction.Create(o,x));}
  public void E(OpCode o,int x){il.Append(Instruction.Create(o,x));}
  public void E(OpCode o,float x){il.Append(Instruction.Create(o,x));}
  public void E(OpCode o,TypeReference x){il.Append(Instruction.Create(o,x));}
  public void E(OpCode o,string x){il.Append(Instruction.Create(o,x));}
  public void Finally(Instruction start,Instruction handler,Instruction end){body.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally){TryStart=start,TryEnd=handler,HandlerStart=handler,HandlerEnd=end});}
  public void Dump(string dependency){
   var offsets=new Dictionary<Instruction,int>();int offset=0;foreach(var i in body.Instructions){offsets[i]=offset;offset+=i.GetSize();}
   var lines=new List<string>();lines.Add(md.MetadataToken+" "+md.FullName+"\nSTATUS=SPECIFICATION_ONLY\nDEPENDENCY="+dependency+"\nCodeSize="+offset+" InitLocals=true");
   foreach(var v in body.Variables)lines.Add("V_"+v.Index+" "+v.VariableType.FullName);
   var instructions=new List<object>();
   foreach(var i in body.Instructions){
    string op="";object operand=null;
    if(i.Operand is Instruction){var x=(Instruction)i.Operand;op="IL_"+offsets[x].ToString("X4");operand=new{kind="branch",target=offsets[x]};}
    else if(i.Operand is VariableDefinition){var v=(VariableDefinition)i.Operand;op="V_"+v.Index;operand=new{kind="local",index=v.Index};}
    else if(i.Operand is FieldReference){var f=(FieldReference)i.Operand;op=f.FullName;operand=new{kind="field",name=f.Name,owner=Ty(f.DeclaringType),type=Ty(f.FieldType),identity=f.FullName};}
    else if(i.Operand is MethodReference){var r=(MethodReference)i.Operand;op=r.FullName;operand=new{kind="method",name=r.Name,owner=Ty(r.DeclaringType),ret=Ty(r.ReturnType),args=r.Parameters.Select(p=>Ty(p.ParameterType)).ToArray(),hasThis=r.HasThis,identity=r.FullName};}
    else if(i.Operand is TypeReference){op=Ty((TypeReference)i.Operand);operand=new{kind="type",type=op};}
    else if(i.Operand!=null){op=Convert.ToString(i.Operand,System.Globalization.CultureInfo.InvariantCulture);operand=new{kind="literal",value=i.Operand};}
    if(evidence.ContainsKey(i))lines.Add("// native "+evidence[i]);
    lines.Add("IL_"+offsets[i].ToString("X4")+": "+i.OpCode.Name+" "+op);
    instructions.Add(new{offset=offsets[i],opcode=i.OpCode.Name,operand=operand,native=evidence.ContainsKey(i)?evidence[i]:null});
   }
   var handlers=body.ExceptionHandlers.Select(h=>new{kind=h.HandlerType.ToString(),tryStart=offsets[h.TryStart],tryEnd=offsets[h.TryEnd],handlerStart=offsets[h.HandlerStart],handlerEnd=offsets[h.HandlerEnd]}).ToArray();
   foreach(var h in handlers)lines.Add("EH finally try [IL_"+h.tryStart.ToString("X4")+", IL_"+h.tryEnd.ToString("X4")+") handler [IL_"+h.handlerStart.ToString("X4")+", IL_"+h.handlerEnd.ToString("X4")+")");
   string name=md.DeclaringType.Name+"."+md.Name;
   File.WriteAllLines(Path.Combine(outdir,name+".spec.il"),lines);
   var data=new{token=md.MetadataToken.ToUInt32(),name=md.FullName,owner=md.DeclaringType.FullName,ret=Ty(md.ReturnType),maxStack=body.MaxStackSize,args=md.Parameters.Select(p=>Ty(p.ParameterType)).ToArray(),locals=body.Variables.Select(v=>Ty(v.VariableType)).ToArray(),instructions=instructions,handlers=handlers,dependency=dependency};
   File.WriteAllText(Path.Combine(outdir,name+".spec.json"),new JavaScriptSerializer().Serialize(data));
   if(build)md.Body=body;
   Console.WriteLine(name+" instructions="+instructions.Count+" locals="+body.Variables.Count+" EH="+handlers.Length);
  }
 }
 static void Zombie(){
  var b=new B(M("MouseManager","GetZombieUnderMouse"));var enType=Enum(T("Zombie"));
  var result=b.V(T("Zombie"));var en=b.V(enType);var z=b.V(T("Zombie"));
  var falsePath=b.L();var done=b.L();
  b.E(OpCodes.Ldnull);b.E(OpCodes.Stloc,result);b.E(OpCodes.Ldarg_0);b.E(OpCodes.Ldfld,F("MouseManager","onBoard"));b.E(OpCodes.Brfalse,falsePath);
  b.Mark(b.L(),"18031EEAC-18031EEE7");b.E(OpCodes.Ldarg_0);b.E(OpCodes.Ldfld,F("MouseManager","board"));b.E(OpCodes.Ldfld,F("Board","zombieManager"));b.E(OpCodes.Ldfld,F("ZombieManager","zombieList"));b.E(OpCodes.Callvirt,GetEnum(F("ZombieManager","zombieList").FieldType));b.E(OpCodes.Stloc,en);
  ZombieLoop(b,result,en,z,enType,done,true);
  b.Mark(falsePath,"18031F054-18031F117");
  b.E(OpCodes.Call,M("PrepareUIController","get_Instance"));b.E(OpCodes.Ldfld,F("PrepareUIController","boardPreview"));b.E(OpCodes.Call,R("UnityEngine.Object","op_Implicit"));b.E(OpCodes.Brfalse,done);
  b.E(OpCodes.Call,M("PrepareUIController","get_Instance"));b.E(OpCodes.Ldfld,F("PrepareUIController","boardPreview"));b.E(OpCodes.Ldfld,F("BoardPreview","enemyPreviews"));b.E(OpCodes.Callvirt,GetEnum(F("BoardPreview","enemyPreviews").FieldType));b.E(OpCodes.Stloc,en);
  ZombieLoop(b,result,en,z,enType,done,false);
  b.Mark(done,"18031F264-18031F289");b.E(OpCodes.Ldloc,result);b.E(OpCodes.Ret);b.Dump("Existing PrepareUIController.get_Instance and Zombie.CanAttacked retain baseline bodies");
 }
 static void ZombieLoop(B b,VariableDefinition result,VariableDefinition en,VariableDefinition z,TypeReference enType,Instruction done,bool onBoard){
  var start=b.L();var loop=b.L();var test=b.L();var handler=b.L();var after=b.L();var bounds=b.L();
  b.Mark(start,onBoard?"18031EEE7-18031EF20":"18031F11C-18031F15C");b.E(OpCodes.Br,test);
  b.Mark(loop,onBoard?"18031EF2D-18031EF93":"18031F169-18031F1BC");b.E(OpCodes.Ldloca,en);b.E(OpCodes.Call,Current(enType));b.E(OpCodes.Stloc,z);
  b.E(OpCodes.Ldloc,z);b.E(OpCodes.Ldnull);b.E(OpCodes.Call,R("UnityEngine.Object","op_Equality"));b.E(OpCodes.Brtrue,test);
  b.E(OpCodes.Ldloc,z);b.E(OpCodes.Callvirt,R("UnityEngine.Component","get_gameObject"));b.E(OpCodes.Callvirt,R("UnityEngine.GameObject","get_activeSelf"));b.E(OpCodes.Brfalse,test);
  if(onBoard){b.E(OpCodes.Ldarg_1);b.E(OpCodes.Brtrue,bounds);b.E(OpCodes.Ldloc,z);b.E(OpCodes.Callvirt,M("Zombie","CanAttacked"));b.E(OpCodes.Brfalse,test);}
  b.Mark(bounds,onBoard?"18031EF99-18031F005":"18031F1BE-18031F22B");
  var pos=F("MouseManager","mouseWorldPosition");var vx=new FieldReference("x",m.TypeSystem.Single,pos.FieldType);var vy=new FieldReference("y",m.TypeSystem.Single,pos.FieldType);
  Action<FieldReference> mouse=c=>{b.E(OpCodes.Ldarg_0);b.E(OpCodes.Ldflda,pos);b.E(OpCodes.Ldfld,c);};
  // Ordered comparisons match COMISS+JA: unordered (NaN) does not reject.
  b.E(OpCodes.Ldloc,z);b.E(OpCodes.Ldfld,F("Zombie","fX"));b.E(OpCodes.Ldloc,z);b.E(OpCodes.Ldfld,F("Zombie","fW"));b.E(OpCodes.Ldc_R4,.5f);b.E(OpCodes.Mul);b.E(OpCodes.Sub);mouse(vx);b.E(OpCodes.Bgt,test);
  mouse(vx);b.E(OpCodes.Ldloc,z);b.E(OpCodes.Ldfld,F("Zombie","fW"));b.E(OpCodes.Ldc_R4,.5f);b.E(OpCodes.Mul);b.E(OpCodes.Ldloc,z);b.E(OpCodes.Ldfld,F("Zombie","fX"));b.E(OpCodes.Add);b.E(OpCodes.Bgt,test);
  b.E(OpCodes.Ldloc,z);b.E(OpCodes.Ldfld,F("Zombie","fY"));b.E(OpCodes.Ldloc,z);b.E(OpCodes.Ldfld,F("Zombie","fD"));b.E(OpCodes.Ldc_R4,.5f);b.E(OpCodes.Mul);b.E(OpCodes.Sub);mouse(vy);b.E(OpCodes.Bgt,test);
  mouse(vy);b.E(OpCodes.Ldloc,z);b.E(OpCodes.Ldfld,F("Zombie","fY"));b.E(OpCodes.Ldloc,z);b.E(OpCodes.Ldfld,F("Zombie","fH"));b.E(OpCodes.Add);b.E(OpCodes.Bgt,test);b.E(OpCodes.Ldloc,z);b.E(OpCodes.Stloc,result);
  b.Mark(test,onBoard?"18031EF14-18031EF27":"18031F150-18031F163");b.E(OpCodes.Ldloca,en);b.E(OpCodes.Call,Move(enType));b.E(OpCodes.Brtrue,loop);b.E(OpCodes.Leave,after);
  b.Mark(handler,onBoard?"18031F012-18031F044 normal and exceptional Dispose":"18031F238-18031F262 normal and exceptional Dispose");b.E(OpCodes.Ldloca,en);b.E(OpCodes.Call,Dispose(enType));b.E(OpCodes.Endfinally);
  b.Mark(after,null);b.E(OpCodes.Br,done);b.Finally(start,handler,after);
 }
 static void Pass(){
  var b=new B(M("SavesManager","PassLevel"));var count=b.V(m.TypeSystem.Int32);var et=Enum(T("BoardEntry"));var en=b.V(et);var entry=b.V(T("BoardEntry"));
  var adventure=b.L();var rescue=b.L();var hard=b.L();var done=b.L();
  b.Mark(b.L(),"18033FE48-18033FE68 challenge dispatch");
  b.E(OpCodes.Ldarg_1);b.E(OpCodes.Ldc_I4_0);b.E(OpCodes.Beq,adventure);b.E(OpCodes.Ldarg_1);b.E(OpCodes.Ldc_I4_1);b.E(OpCodes.Beq,rescue);b.E(OpCodes.Ldarg_1);b.E(OpCodes.Ldc_I4_2);b.E(OpCodes.Beq,hard);b.E(OpCodes.Br,done);
  Action<string> array=n=>{b.E(OpCodes.Ldarg_0);b.E(OpCodes.Ldfld,F("SavesManager","playerSave"));b.E(OpCodes.Ldfld,F("Save",n));};
  b.Mark(adventure,"18033FFDA-18033FFFA");b.E(OpCodes.Ldarg_0);b.E(OpCodes.Ldfld,F("SavesManager","playerSave"));b.E(OpCodes.Ldfld,F("Save","adventureLevel"));b.E(OpCodes.Ldc_I4,180);b.E(OpCodes.Bgt,done);
  b.E(OpCodes.Ldarg_0);b.E(OpCodes.Ldfld,F("SavesManager","playerSave"));b.E(OpCodes.Dup);b.E(OpCodes.Ldfld,F("Save","adventureLevel"));b.E(OpCodes.Ldc_I4_1);b.E(OpCodes.Add);b.E(OpCodes.Stfld,F("Save","adventureLevel"));b.E(OpCodes.Br,done);
  b.Mark(rescue,"18033FFB1-18033FFD9");array("rescuePassNum");b.E(OpCodes.Ldarg_2);b.E(OpCodes.Ldelema,m.TypeSystem.Int32);b.E(OpCodes.Dup);b.E(OpCodes.Ldind_I4);b.E(OpCodes.Ldc_I4_1);b.E(OpCodes.Add);b.E(OpCodes.Stind_I4);b.E(OpCodes.Br,done);
  b.Mark(hard,"18033FE6E-18033FE99");b.E(OpCodes.Ldc_I4_0);b.E(OpCodes.Stloc,count);b.E(OpCodes.Ldarg_3);b.E(OpCodes.Ldfld,F("BoardConfig","boardEntries"));b.E(OpCodes.Callvirt,GetEnum(F("BoardConfig","boardEntries").FieldType));b.E(OpCodes.Stloc,en);
  var start=b.L();var loop=b.L();var test=b.L();var handler=b.L();var after=b.L();var passed=b.L();
  b.Mark(start,"18033FE9E-18033FEC3");b.E(OpCodes.Br,test);
  b.Mark(loop,"18033FED8-18033FEFB");b.E(OpCodes.Ldloca,en);b.E(OpCodes.Call,Current(et));b.E(OpCodes.Stloc,entry);b.E(OpCodes.Ldloc,entry);b.E(OpCodes.Ldfld,F("BoardEntry","must"));b.E(OpCodes.Brtrue,test);b.E(OpCodes.Ldloc,entry);b.E(OpCodes.Ldfld,F("BoardEntry","select"));b.E(OpCodes.Brfalse,test);b.E(OpCodes.Ldloc,count);b.E(OpCodes.Ldc_I4_1);b.E(OpCodes.Add);b.E(OpCodes.Stloc,count);
  b.Mark(test,"18033FEC3-18033FED6");b.E(OpCodes.Ldloca,en);b.E(OpCodes.Call,Move(et));b.E(OpCodes.Brtrue,loop);b.E(OpCodes.Leave,after);
  b.Mark(handler,"18033FEFD-18033FF27 normal and exceptional Dispose");b.E(OpCodes.Ldloca,en);b.E(OpCodes.Call,Dispose(et));b.E(OpCodes.Endfinally);
  b.Mark(after,"18033FF49-18033FF81");array("adventureHardMaxStarNum");b.E(OpCodes.Ldarg_2);b.E(OpCodes.Ldelem_I4);b.E(OpCodes.Ldloc,count);b.E(OpCodes.Bge,passed);array("adventureHardMaxStarNum");b.E(OpCodes.Ldarg_2);b.E(OpCodes.Ldloc,count);b.E(OpCodes.Stelem_I4);
  b.Mark(passed,"18033FF85-18033FFA0");array("adventureHardPassed");b.E(OpCodes.Ldarg_2);b.E(OpCodes.Ldc_I4_1);b.E(OpCodes.Stelem_I1);
  b.Mark(done,"18033FFA5-18033FFB0");b.E(OpCodes.Ret);b.Finally(start,handler,after);b.Dump("No new custom managed callee");
 }
 static void Enemy(){
  var b=new B(M("EnemyManager","CreateEnemySelecter"));var s=b.V(T("EnemySelecter"));var ei=Enum(T("ZombieInfo"));var infos=b.V(ei);var info=b.V(T("ZombieInfo"));var be=Enum(T("BoardEntry"));var entries=b.V(be);var entry=b.V(T("BoardEntry"));var mult=b.V(m.TypeSystem.Single);var index=b.V(m.TypeSystem.Int32);var ids=b.V(F("EnemyManager","zombieTypes").FieldType);
  var selects=F("EnemySelecter","zombieSelects");
  b.Mark(b.L(),"180316B85-180316BF4 EnemySelecter allocation and constructor");b.E(OpCodes.Newobj,M("EnemySelecter",".ctor"));b.E(OpCodes.Stloc,s);
  b.Mark(b.L(),"180316BFC-180316C30");b.E(OpCodes.Call,M("ResourceManager","Load_zombieInfo_all"));b.E(OpCodes.Callvirt,GetEnum(M("ResourceManager","Load_zombieInfo_all").ReturnType));b.E(OpCodes.Stloc,infos);
  var start=b.L();var loop=b.L();var test=b.L();var handler=b.L();var after=b.L();
  b.Mark(start,"180316C35-180316C60");b.E(OpCodes.Br,test);
  b.Mark(loop,"180316C79-180316DF0 includes inlined ZombieSelect constructor and List.Add");b.E(OpCodes.Ldloca,infos);b.E(OpCodes.Call,Current(ei));b.E(OpCodes.Stloc,info);b.E(OpCodes.Ldloc,s);b.E(OpCodes.Ldfld,selects);b.E(OpCodes.Ldloc,info);b.E(OpCodes.Newobj,M("ZombieSelect",".ctor"));b.E(OpCodes.Callvirt,R(selects.FieldType.FullName,"Add"));
  b.Mark(test,"180316C60-180316C73");b.E(OpCodes.Ldloca,infos);b.E(OpCodes.Call,Move(ei));b.E(OpCodes.Brtrue,loop);b.E(OpCodes.Leave,after);
  b.Mark(handler,"180316DF5-180316E1F normal and exceptional Dispose");b.E(OpCodes.Ldloca,infos);b.E(OpCodes.Call,Dispose(ei));b.E(OpCodes.Endfinally);b.Mark(after,"180316E37-180316E60");b.Finally(start,handler,after);
  b.E(OpCodes.Ldloc,s);b.E(OpCodes.Ldarg_0);b.E(OpCodes.Ldfld,F("EnemyManager","board"));b.E(OpCodes.Stfld,F("EnemySelecter","board"));b.E(OpCodes.Ldloc,s);b.E(OpCodes.Ldarg_0);b.E(OpCodes.Stfld,F("EnemySelecter","enemyManager"));
  b.Mark(b.L(),"180316E65-180316E90");b.E(OpCodes.Ldc_R4,1f);b.E(OpCodes.Stloc,mult);b.E(OpCodes.Ldarg_0);b.E(OpCodes.Ldfld,F("EnemyManager","boardEntries"));b.E(OpCodes.Callvirt,GetEnum(F("EnemyManager","boardEntries").FieldType));b.E(OpCodes.Stloc,entries);
  start=b.L();loop=b.L();test=b.L();handler=b.L();after=b.L();
  b.Mark(start,"180316E95-180316EC0");b.E(OpCodes.Br,test);
  b.Mark(loop,"180316ED5-180316EFD");b.E(OpCodes.Ldloca,entries);b.E(OpCodes.Call,Current(be));b.E(OpCodes.Stloc,entry);b.E(OpCodes.Ldloc,entry);b.E(OpCodes.Ldfld,F("BoardEntry","boardEntryType"));b.E(OpCodes.Ldc_I4_1);b.E(OpCodes.Bne_Un,test);b.E(OpCodes.Ldloc,mult);b.E(OpCodes.Ldloc,entry);b.E(OpCodes.Callvirt,M("BoardEntry","GetValue"));b.E(OpCodes.Add);b.E(OpCodes.Stloc,mult);
  b.Mark(test,"180316EC0-180316ED3");b.E(OpCodes.Ldloca,entries);b.E(OpCodes.Call,Move(be));b.E(OpCodes.Brtrue,loop);b.E(OpCodes.Leave,after);
  b.Mark(handler,"180316EFF-180316F29 normal and exceptional Dispose");b.E(OpCodes.Ldloca,entries);b.E(OpCodes.Call,Dispose(be));b.E(OpCodes.Endfinally);b.Mark(after,"180316F4A-180316F5C");b.Finally(start,handler,after);
  b.E(OpCodes.Ldloc,s);b.E(OpCodes.Ldarg_0);b.E(OpCodes.Ldfld,F("EnemyManager","enemyPoint_Base"));b.E(OpCodes.Conv_R4);b.E(OpCodes.Ldloc,mult);b.E(OpCodes.Mul);b.E(OpCodes.Conv_I4);b.E(OpCodes.Stfld,F("EnemySelecter","enemyPoint_Base"));
  b.Mark(b.L(),"180316F60-180316FB8");b.E(OpCodes.Ldarg_0);b.E(OpCodes.Ldfld,F("EnemyManager","zombieTypes"));b.E(OpCodes.Stloc,ids);b.E(OpCodes.Ldc_I4_0);b.E(OpCodes.Stloc,index);
  var test1=b.L();var loop1=b.L();b.E(OpCodes.Br,test1);
  b.Mark(loop1,"180316FCA-180317006");b.E(OpCodes.Ldloc,s);b.E(OpCodes.Ldfld,selects);b.E(OpCodes.Ldloc,ids);b.E(OpCodes.Ldloc,index);b.E(OpCodes.Callvirt,R(ids.VariableType.FullName,"get_Item"));b.E(OpCodes.Callvirt,R(selects.FieldType.FullName,"get_Item"));b.E(OpCodes.Ldc_I4_1);b.E(OpCodes.Stfld,F("ZombieSelect","exist"));Inc(b,index,1);
  b.Mark(test1,"180316FC0-180316FC8");b.E(OpCodes.Ldloc,index);b.E(OpCodes.Ldloc,ids);b.E(OpCodes.Callvirt,Ref(ids.VariableType,"get_Count",m.TypeSystem.Int32));b.E(OpCodes.Blt,loop1);
  b.E(OpCodes.Ldc_I4_0);b.E(OpCodes.Stloc,index);var test2=b.L();var loop2=b.L();var next=b.L();b.E(OpCodes.Br,test2);
  b.Mark(loop2,"180317018-180317059");b.E(OpCodes.Ldloc,s);b.E(OpCodes.Ldfld,selects);b.E(OpCodes.Ldloc,index);b.E(OpCodes.Callvirt,R(selects.FieldType.FullName,"get_Item"));b.E(OpCodes.Ldfld,F("ZombieSelect","exist"));b.E(OpCodes.Brtrue,next);b.E(OpCodes.Ldloc,s);b.E(OpCodes.Ldfld,selects);b.E(OpCodes.Ldloc,index);b.E(OpCodes.Callvirt,Ref(selects.FieldType,"RemoveAt",m.TypeSystem.Void,m.TypeSystem.Int32));Inc(b,index,-1);
  b.Mark(next,"18031705B-180317068");Inc(b,index,1);
  b.Mark(test2,"180317008-180317016");b.E(OpCodes.Ldloc,index);b.E(OpCodes.Ldloc,s);b.E(OpCodes.Ldfld,selects);b.E(OpCodes.Callvirt,Ref(selects.FieldType,"get_Count",m.TypeSystem.Int32));b.E(OpCodes.Blt,loop2);
  b.Mark(b.L(),"18031706A-180317087");b.E(OpCodes.Ldloc,s);b.E(OpCodes.Ret);
  b.Dump("Requires native-backed 0x060001A5 constructor in the user-approved four-method scope");
 }
 static void Constructor(){
  var b=new B(M("ZombieSelect",".ctor"));var i=b.V(m.TypeSystem.Int32);var req=b.V(new ArrayType(m.TypeSystem.Boolean));var flag=b.L();var test=b.L();var loop=b.L();
  b.Mark(b.L(),"18032529C-1803252C2 allocation before System.Object constructor");
  b.E(OpCodes.Ldarg_0);b.E(OpCodes.Ldc_I4,10);b.E(OpCodes.Newarr,m.TypeSystem.Boolean);b.E(OpCodes.Stfld,F("ZombieSelect","requirements"));b.E(OpCodes.Ldarg_0);b.E(OpCodes.Call,R("System.Object",".ctor"));
  b.Mark(b.L(),"1803252C7-1803252EA");
  string[] dst={"id","point","weight","minWave"};string[] src={"id","enemyPoint","enemyWeight","minWave"};
  for(int n=0;n<4;n++){
   b.E(OpCodes.Ldarg_0);b.E(OpCodes.Ldarg_1);b.E(OpCodes.Ldfld,F("ZombieInfo",src[n]));b.E(OpCodes.Stfld,F("ZombieSelect",dst[n]));
   if(n==0){b.E(OpCodes.Ldarg_0);b.E(OpCodes.Ldc_I4_0);b.E(OpCodes.Stfld,F("ZombieSelect","exist"));}
   if(n==2){b.E(OpCodes.Ldarg_0);b.E(OpCodes.Ldc_I4_0);b.E(OpCodes.Stfld,F("ZombieSelect","minFlag"));}
  }
  b.Mark(b.L(),"1803252ED-180325300");b.E(OpCodes.Ldarg_0);b.E(OpCodes.Ldarg_1);b.E(OpCodes.Ldfld,F("ZombieInfo","isElite"));b.E(OpCodes.Dup);b.E(OpCodes.Brtrue,flag);b.E(OpCodes.Pop);b.E(OpCodes.Ldarg_1);b.E(OpCodes.Ldfld,F("ZombieInfo","isBoss"));b.Mark(flag,null);b.E(OpCodes.Ldc_I4_0);b.E(OpCodes.Cgt_Un);b.E(OpCodes.Stfld,F("ZombieSelect","elite"));
  b.E(OpCodes.Ldc_I4_0);b.E(OpCodes.Stloc,i);b.E(OpCodes.Br,test);
  b.Mark(loop,"180325303-180325330 source access then destination store; bool normalization");
  b.E(OpCodes.Ldarg_1);b.E(OpCodes.Ldfld,F("ZombieInfo","enemyRequirements"));b.E(OpCodes.Stloc,req);
  // Keep array operations explicit, including null/bounds exceptions. No defaulting or truncation.
  b.E(OpCodes.Ldarg_0);b.E(OpCodes.Ldfld,F("ZombieSelect","requirements"));b.E(OpCodes.Ldloc,i);b.E(OpCodes.Ldloc,req);b.E(OpCodes.Ldloc,i);b.E(OpCodes.Ldelem_U1);b.E(OpCodes.Ldc_I4_0);b.E(OpCodes.Cgt_Un);b.E(OpCodes.Stelem_I1);Inc(b,i,1);
  b.Mark(test,"180325335-180325338");b.E(OpCodes.Ldloc,i);b.E(OpCodes.Ldc_I4,10);b.E(OpCodes.Blt,loop);b.Mark(b.L(),"18032533A-180325344");b.E(OpCodes.Ret);b.Dump("User approved scope extension to 0x060001A5; native body has no EH");
 }
 static void Planting(){
  var b=new B(M("Plant","SetAnimationState_Planting"));var noAttack=b.L();var done=b.L();
  b.Mark(b.L(),"180358341-180358350 direct dereference; native null target 1803583E8 throws");b.E(OpCodes.Ldarg_0);b.E(OpCodes.Ldfld,F("Plant","attackRangeUIController"));b.E(OpCodes.Callvirt,M("AttackRangeUIController","CollapseView"));
  b.Mark(b.L(),"180358355-18035836A shadow.SetActive(true)");b.E(OpCodes.Ldarg_0);b.E(OpCodes.Ldfld,F("Plant","shadow"));b.E(OpCodes.Ldc_I4_1);b.E(OpCodes.Callvirt,R("UnityEngine.GameObject","SetActive"));
  b.Mark(b.L(),"18035836F-18035838D literal slot 181BCADF0 decoded to speed");b.E(OpCodes.Ldarg_0);b.E(OpCodes.Ldfld,F("Plant","animator"));b.E(OpCodes.Ldstr,"speed");b.E(OpCodes.Ldarg_0);b.E(OpCodes.Ldfld,F("Plant","updateRate"));b.E(OpCodes.Callvirt,R("UnityEngine.Animator","SetFloat"));
  b.Mark(b.L(),"180358392-18035839C IDs 4 or 49 disable attack");b.E(OpCodes.Ldarg_0);b.E(OpCodes.Ldfld,F("Plant","ID"));b.E(OpCodes.Ldc_I4_4);b.E(OpCodes.Beq,noAttack);b.E(OpCodes.Ldarg_0);b.E(OpCodes.Ldfld,F("Plant","ID"));b.E(OpCodes.Ldc_I4,49);b.E(OpCodes.Beq,noAttack);
  b.Mark(b.L(),"18035839E-1803583A9 enable attack; ID 2 enters trigger branch");b.E(OpCodes.Ldarg_0);b.E(OpCodes.Ldc_I4_1);b.E(OpCodes.Stfld,F("Plant","attackable"));b.E(OpCodes.Ldarg_0);b.E(OpCodes.Ldfld,F("Plant","ID"));b.E(OpCodes.Ldc_I4_2);b.E(OpCodes.Bne_Un,done);
  b.Mark(b.L(),"1803583AB-1803583D0 capture animator before state=1; tail SetTrigger(ExplodeTrigger)");b.E(OpCodes.Ldarg_0);b.E(OpCodes.Ldfld,F("Plant","animator"));b.E(OpCodes.Ldarg_0);b.E(OpCodes.Ldc_I4_1);b.E(OpCodes.Stfld,F("Plant","state"));b.E(OpCodes.Ldstr,"ExplodeTrigger");b.E(OpCodes.Callvirt,R("UnityEngine.Animator","SetTrigger"));b.E(OpCodes.Br,done);
  b.Mark(noAttack,"1803583DB-1803583E7");b.E(OpCodes.Ldarg_0);b.E(OpCodes.Ldc_I4_0);b.E(OpCodes.Stfld,F("Plant","attackable"));
  b.Mark(done,"1803583D5-1803583DA / 1803583E2-1803583E7");b.E(OpCodes.Ret);b.Dump("Original PC complete 222-byte body; preserves direct null exceptions and ID 2 side effects");
 }
 static void SunTime(){
  var b=new B(M("SunManager","SetMaxSunFallTime"));var state=b.V(m.TypeSystem.Int32);var time=b.V(m.TypeSystem.Single);var et=Enum(T("BoardEntry"));var en=b.V(et);var entry=b.V(T("BoardEntry"));
  var done=b.L();var enumerate=b.L();var day=b.L();var snow=b.L();var twilight=b.L();var dark=b.L();
  b.Mark(b.L(),"1803409F2-180340A08 null boardConfig returns unchanged; map argument unused; config.map null throws");b.E(OpCodes.Ldarg_1);b.E(OpCodes.Brfalse,done);b.E(OpCodes.Ldarg_1);b.E(OpCodes.Ldfld,F("BoardConfig","map"));b.E(OpCodes.Ldfld,F("Map","senarioState"));b.E(OpCodes.Stloc,state);
  b.Mark(b.L(),"180340A0B-180340A56 states 0/1/8/9/10 -> 8/0/16/12/24; others 0");b.E(OpCodes.Ldc_R4,0f);b.E(OpCodes.Stloc,time);
  b.E(OpCodes.Ldloc,state);b.E(OpCodes.Ldc_I4_0);b.E(OpCodes.Beq,day);b.E(OpCodes.Ldloc,state);b.E(OpCodes.Ldc_I4,8);b.E(OpCodes.Beq,snow);b.E(OpCodes.Ldloc,state);b.E(OpCodes.Ldc_I4,9);b.E(OpCodes.Beq,twilight);b.E(OpCodes.Ldloc,state);b.E(OpCodes.Ldc_I4,10);b.E(OpCodes.Beq,dark);b.E(OpCodes.Br,enumerate);
  foreach(var pair in new[]{Tuple.Create(day,8f),Tuple.Create(snow,16f),Tuple.Create(twilight,12f),Tuple.Create(dark,24f)}){b.Mark(pair.Item1,null);b.E(OpCodes.Ldc_R4,pair.Item2);b.E(OpCodes.Stloc,time);b.E(OpCodes.Br,enumerate);}
  b.Mark(enumerate,"180340A59-180340A90 boardConfig.boardEntries.GetEnumerator");b.E(OpCodes.Ldarg_1);b.E(OpCodes.Ldfld,F("BoardConfig","boardEntries"));b.E(OpCodes.Callvirt,GetEnum(F("BoardConfig","boardEntries").FieldType));b.E(OpCodes.Stloc,en);
  var start=b.L();var loop=b.L();var test=b.L();var selected=b.L();var other=b.L();var handler=b.L();var after=b.L();
  b.Mark(start,"180340A96-180340AA4");b.E(OpCodes.Br,test);
  b.Mark(loop,"180340AC5-180340ADD current; apply iff must OR select");b.E(OpCodes.Ldloca,en);b.E(OpCodes.Call,Current(et));b.E(OpCodes.Stloc,entry);b.E(OpCodes.Ldloc,entry);b.E(OpCodes.Ldfld,F("BoardEntry","must"));b.E(OpCodes.Brtrue,selected);b.E(OpCodes.Ldloc,entry);b.E(OpCodes.Ldfld,F("BoardEntry","select"));b.E(OpCodes.Brfalse,test);
  b.Mark(selected,"180340ADF-180340AF2 type 2 doubles through addition");b.E(OpCodes.Ldloc,entry);b.E(OpCodes.Ldfld,F("BoardEntry","boardEntryType"));b.E(OpCodes.Ldc_I4_2);b.E(OpCodes.Bne_Un,other);b.E(OpCodes.Ldloc,time);b.E(OpCodes.Ldloc,time);b.E(OpCodes.Add);b.E(OpCodes.Stloc,time);b.E(OpCodes.Br,test);
  b.Mark(other,"180340AF4-180340B0F type 3 sets positive zero, other types unchanged");b.E(OpCodes.Ldloc,entry);b.E(OpCodes.Ldfld,F("BoardEntry","boardEntryType"));b.E(OpCodes.Ldc_I4_3);b.E(OpCodes.Bne_Un,test);b.E(OpCodes.Ldc_R4,0f);b.E(OpCodes.Stloc,time);
  b.Mark(test,"180340AB0-180340AC3 MoveNext");b.E(OpCodes.Ldloca,en);b.E(OpCodes.Call,Move(et));b.E(OpCodes.Brtrue,loop);b.E(OpCodes.Leave,after);
  b.Mark(handler,"180340B11-180340B3B normal and exceptional Dispose; exception rethrow 180340B7A");b.E(OpCodes.Ldloca,en);b.E(OpCodes.Call,Dispose(et));b.E(OpCodes.Endfinally);
  b.Mark(after,"180340B4E commit only after enumeration and Dispose");b.E(OpCodes.Ldarg_0);b.E(OpCodes.Ldloc,time);b.E(OpCodes.Stfld,F("SunManager","sunFallTimeMax"));
  b.Mark(done,"180340B53-180340B6D");b.E(OpCodes.Ret);b.Finally(start,handler,after);b.Dump("Original PC 496-byte body; Map parameter intentionally unused, BoardConfig.map drives selection");
 }
 static void Inc(B b,VariableDefinition i,int n){b.E(OpCodes.Ldloc,i);b.E(OpCodes.Ldc_I4,n);b.E(OpCodes.Add);b.E(OpCodes.Stloc,i);}
 public static void Main(string[] args){
  if(args[0]=="--reopen"){
   outdir=args[2];Directory.CreateDirectory(outdir);using(var a=AssemblyDefinition.ReadAssembly(args[1])){
    m=a.MainModule;foreach(uint token in new uint[]{0x060001E0,0x060001B2,0x06000248,0x060001A5,0x0600039F,0x06000267}){
     var md=(MethodDefinition)m.LookupToken((int)token);var b=new B(md);b.body=md.Body;b.il=md.Body.GetILProcessor();b.Dump("Reopened actual candidate bytes");
    }
   }return;
  }
  string sha;using(var h=SHA256.Create())sha=BitConverter.ToString(h.ComputeHash(File.ReadAllBytes(args[0]))).Replace("-","").ToLowerInvariant();
  if(sha!="18e44a5a50f1da09cf2f1fc0aace0c18d5cf981606bedaacc879b96e8d2f18bd")throw new Exception("BASELINE_HASH_MISMATCH");
  outdir=args[1];build=args.Length==3;Directory.CreateDirectory(outdir);
  using(var a=AssemblyDefinition.ReadAssembly(args[0])){m=a.MainModule;Zombie();Pass();Enemy();Constructor();Planting();SunTime();if(build)a.Write(args[2]);}
 }
}

