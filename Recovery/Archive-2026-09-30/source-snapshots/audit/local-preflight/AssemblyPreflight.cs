using System;using System.IO;using System.Linq;using System.Collections.Generic;using System.Web.Script.Serialization;using Mono.Cecil;using Mono.Cecil.Cil;
class AssemblyPreflight {
 class Finding {public string category,offset,detail;public Finding(string c,Instruction i,string d){category=c;offset="IL_"+i.Offset.ToString("X4");detail=d;}}
 static IEnumerable<TypeDefinition> Types(IEnumerable<TypeDefinition> ts){foreach(var t in ts){yield return t;foreach(var n in Types(t.NestedTypes))yield return n;}}
 static VariableDefinition Local(MethodBody b,Instruction i){var v=i.Operand as VariableDefinition;if(v!=null)return v;string n=i.OpCode.Name;int index;if(int.TryParse(n.Substring(n.LastIndexOf('.')+1),out index)&&index<b.Variables.Count)return b.Variables[index];return null;}
 static string Kind(TypeReference t){
  if(t==null||t is GenericParameter)return null;
  if(t is ByReferenceType)return "PTR:"+((ByReferenceType)t).ElementType.FullName;
  var def=t as TypeDefinition;if(def==null&&t.Scope==t.Module)def=t.Module.Types.FirstOrDefault(x=>x.FullName==t.FullName);
  if(def==null&&t.IsValueType&&!(t is GenericInstanceType)){try{def=t.Resolve();}catch{}}
  if(def!=null&&def.IsEnum)return Kind(def.Fields.Single(f=>f.Name=="value__").FieldType);
  switch(t.MetadataType){case MetadataType.Boolean:case MetadataType.Char:case MetadataType.Byte:case MetadataType.SByte:case MetadataType.Int16:case MetadataType.UInt16:case MetadataType.Int32:case MetadataType.UInt32:return "I4";case MetadataType.Int64:case MetadataType.UInt64:return "I8";case MetadataType.Single:case MetadataType.Double:return "F";case MetadataType.IntPtr:case MetadataType.UIntPtr:return "NATIVE";case MetadataType.Void:return null;}
  return t.IsValueType?"VALUE:"+t.FullName:"REF";
 }
 static string Produced(MethodDefinition m,Instruction i){
  string op=i.OpCode.Name;if(op.StartsWith("ldc.i4"))return "I4";if(op=="ldc.i8")return "I8";if(op=="ldc.r4"||op=="ldc.r8")return "F";
  if(op=="ldnull"||op=="ldstr"||op=="newarr")return "REF";
  if(op=="conv.i"||op=="conv.u")return "NATIVE";
  if(op=="conv.r4"||op=="conv.r8"||op=="conv.r.un")return "F";
  if(op.StartsWith("ldloca")){var v=Local(m.Body,i);return v==null?null:"PTR:"+v.VariableType.FullName;}
  if(op.StartsWith("ldloc")){var v=Local(m.Body,i);return v==null?null:Kind(v.VariableType);}
  if(op=="ldfld"||op=="ldsfld")return Kind(((FieldReference)i.Operand).FieldType);
  if(op=="ldflda"||op=="ldsflda")return "PTR:"+((FieldReference)i.Operand).FieldType.FullName;
  if(op=="newobj")return Kind(((MethodReference)i.Operand).DeclaringType);
  if(op=="call"||op=="callvirt"){
   var r=(MethodReference)i.Operand;string k=Kind(r.ReturnType);if(k==null)return null;
   var owner=r.DeclaringType as GenericInstanceType;
   if(owner!=null&&k.Contains("!")&&!k.Contains("!!"))for(int n=owner.GenericArguments.Count-1;n>=0;n--)k=k.Replace("!"+n,owner.GenericArguments[n].FullName);
   return k.Contains("!")?null:k;
  }
  return null;
 }
 static int Count(StackBehaviour behaviour){string s=behaviour.ToString();if(s=="Pop0"||s=="Push0")return 0;if(s=="Varpop"||s=="Varpush"||s=="PopAll")return -1;return s.Split('_').Length;}
 static object Scan(MethodDefinition m){
  var body=m.Body;var ins=body.Instructions;var issues=new List<Finding>();var hints=new List<Finding>();var unsupported=new HashSet<string>();var index=ins.Select((i,n)=>new{i,n}).ToDictionary(x=>x.i,x=>x.n);var heights=new Dictionary<Instruction,int>();var todo=new Queue<Instruction>();int peak=0;
  Action<string,Instruction,string> add=(c,i,d)=>{if(!issues.Any(x=>x.category==c&&x.offset=="IL_"+i.Offset.ToString("X4")&&x.detail==d))issues.Add(new Finding(c,i,d));};
  Action<Instruction,int> queue=(i,h)=>{if(i==null){return;}int prior;if(heights.TryGetValue(i,out prior)){if(prior!=h)add("stack_merge",i,"incoming depths "+prior+" and "+h);}else{heights[i]=h;todo.Enqueue(i);}};
  if(ins.Count>0)queue(ins[0],0);
  foreach(var eh in body.ExceptionHandlers){queue(eh.HandlerStart,eh.HandlerType==ExceptionHandlerType.Catch||eh.HandlerType==ExceptionHandlerType.Filter?1:0);if(eh.FilterStart!=null)queue(eh.FilterStart,1);}
  while(todo.Count>0){
   var i=todo.Dequeue();string op=i.OpCode.Name;int h=heights[i];peak=Math.Max(peak,h);int pop=Count(i.OpCode.StackBehaviourPop),push=Count(i.OpCode.StackBehaviourPush);
   var method=i.Operand as MethodReference;var site=i.Operand as CallSite;
   if(method!=null&&(op=="call"||op=="callvirt"||op=="newobj")){pop=method.Parameters.Count+(method.HasThis&&op!="newobj"?1:0);push=op=="newobj"||method.ReturnType.MetadataType!=MetadataType.Void?1:0;}
   else if(op=="calli"&&site!=null){pop=site.Parameters.Count+(site.HasThis?1:0)+1;push=site.ReturnType.MetadataType==MetadataType.Void?0:1;}
   else if(op=="ret"){pop=m.ReturnType.MetadataType==MetadataType.Void?0:1;push=0;}
   else if(op=="leave"||op=="leave.s"){pop=h;push=0;}
   if(pop<0||push<0){unsupported.Add(op);continue;}
   if(h<pop){add("stack_underflow",i,"depth "+h+", consumes "+pop);continue;}
   int next=h-pop+push;peak=Math.Max(peak,next);
   if(op=="ret"||op=="endfinally"||op=="endfilter"){if(next!=0)add("nonempty_exit",i,"remaining depth "+next);continue;}
   if(i.OpCode.FlowControl==FlowControl.Throw||op=="jmp")continue;
   if(i.OpCode.FlowControl==FlowControl.Branch){queue(i.Operand as Instruction,next);continue;}
   if(i.OpCode.FlowControl==FlowControl.Cond_Branch){var targets=i.Operand as Instruction[];if(targets!=null){foreach(var t in targets)queue(t,next);}else queue(i.Operand as Instruction,next);}
   int ix=index[i];if(ix+1<ins.Count)queue(ins[ix+1],next);else add("fallthrough_end",i,"control falls past final instruction");
  }
  if(peak>body.MaxStackSize&&ins.Count>0)add("maxstack",ins[0],"declared "+body.MaxStackSize+", observed "+peak);
  // Carry known operand kinds within basic blocks. Incoming types are unknown;
  // no speculative propagation across joins. This catches struct arithmetic even
  // when placeholder strings/pop/conversions separate a producer from its use.
  var starts=new HashSet<Instruction>();if(ins.Count>0)starts.Add(ins[0]);
  foreach(var x in ins){var target=x.Operand as Instruction;if(target!=null)starts.Add(target);var many=x.Operand as Instruction[];if(many!=null)foreach(var t in many)starts.Add(t);if(x.Next!=null&&(x.OpCode.FlowControl==FlowControl.Branch||x.OpCode.FlowControl==FlowControl.Cond_Branch||x.OpCode.FlowControl==FlowControl.Return||x.OpCode.FlowControl==FlowControl.Throw))starts.Add(x.Next);}
  foreach(var eh in body.ExceptionHandlers){starts.Add(eh.HandlerStart);if(eh.FilterStart!=null)starts.Add(eh.FilterStart);}
  var stack=new List<string>();
  var binary=new HashSet<string>(new[]{"add","add.ovf","add.ovf.un","sub","sub.ovf","sub.ovf.un","mul","mul.ovf","mul.ovf.un","div","div.un","rem","rem.un","and","or","xor","shl","shr","shr.un","ceq","cgt","cgt.un","clt","clt.un","beq","beq.s","bne.un","bne.un.s","bge","bge.s","bge.un","bge.un.s","bgt","bgt.s","bgt.un","bgt.un.s","ble","ble.s","ble.un","ble.un.s","blt","blt.s","blt.un","blt.un.s"});
  foreach(var x in ins){
   if(!heights.ContainsKey(x))continue;int h=heights[x];if(starts.Contains(x)||stack.Count!=h){stack.Clear();for(int k=0;k<h;k++)stack.Add(null);}
   string op=x.OpCode.Name;
   if(op=="dup"){if(stack.Count>0)stack.Add(stack[stack.Count-1]);continue;}
   int pop=Count(x.OpCode.StackBehaviourPop),push=Count(x.OpCode.StackBehaviourPush);var callee=x.Operand as MethodReference;var cs=x.Operand as CallSite;
   if(callee!=null&&(op=="call"||op=="callvirt"||op=="newobj")){pop=callee.Parameters.Count+(callee.HasThis&&op!="newobj"?1:0);push=op=="newobj"||callee.ReturnType.MetadataType!=MetadataType.Void?1:0;}
   else if(op=="calli"&&cs!=null){pop=cs.Parameters.Count+(cs.HasThis?1:0)+1;push=cs.ReturnType.MetadataType==MetadataType.Void?0:1;}
   else if(op=="ret"){pop=m.ReturnType.MetadataType==MetadataType.Void?0:1;push=0;}
   else if(op=="leave"||op=="leave.s"){stack.Clear();continue;}
   if(pop<0||push<0||stack.Count<pop){stack.Clear();continue;}
   var args=stack.Skip(stack.Count-pop).ToArray();stack.RemoveRange(stack.Count-pop,pop);
   if(binary.Contains(op)&&args.Any(k=>k!=null&&k.StartsWith("VALUE:")))add("struct_primitive_operation",x,op+" consumes "+string.Join(",",args.Select(k=>k??"unknown")));
   if((op.StartsWith("conv.")||op=="neg"||op=="not")&&args.Any(k=>k!=null&&k.StartsWith("VALUE:")))add("struct_primitive_operation",x,op+" consumes "+string.Join(",",args.Select(k=>k??"unknown")));
   string produced=Produced(m,x);if(op=="ceq"||op.StartsWith("cgt")||op.StartsWith("clt"))produced="I4";
   if(produced==null&&binary.Contains(op)&&args.Length==2&&args[0]==args[1]&&(args[0]=="I4"||args[0]=="I8"||args[0]=="F"||args[0]=="NATIVE"))produced=args[0];
   for(int k=0;k<push;k++)stack.Add(produced);
  }
  foreach(var i in ins){
   if(i.OpCode.Name.StartsWith("stloc")&&i.Previous!=null&&heights.ContainsKey(i)&&heights.ContainsKey(i.Previous)){
    var v=Local(body,i);string a=Produced(m,i.Previous),b=v==null?null:Kind(v.VariableType);
    // This is deliberately a high-confidence adjacent-producer check, not a full type verifier.
    if(a!=null&&b!=null&&a!=b&&a!="NATIVE"&&b!="NATIVE")add("local_type",i,"immediate producer "+a+" stored into V"+v.Index+" "+b);
   }
   if(i.OpCode==OpCodes.Ldstr){string s=(string)i.Operand;if(s.StartsWith("Method not found")||s.StartsWith("Unmanaged memory load")||s.StartsWith("Warning: Method ends with non empty stack"))hints.Add(new Finding("decompiler_placeholder",i,s));}
  }
  foreach(var v in body.Variables){
   if(!v.VariableType.FullName.Contains("/Enumerator<"))continue;
   bool stored=ins.Any(i=>i.OpCode.Name.StartsWith("stloc")&&Local(body,i)==v);
   var use=ins.FirstOrDefault(i=>i.OpCode.Name.StartsWith("ldloca")&&Local(body,i)==v);
   bool addressInit=ins.Any(i=>i.OpCode==OpCodes.Initobj||i.OpCode==OpCodes.Stobj||i.OpCode==OpCodes.Cpobj);
   if(use!=null&&!stored&&!addressInit)hints.Add(new Finding("enumerator_without_local_store",use,"V"+v.Index+" has address use but no stloc; inspect call-based initialization manually"));
  }
  return new{token="0x"+m.MetadataToken.ToUInt32().ToString("X8"),name=m.FullName,instructions=ins.Count,visited=heights.Count,unreachable=ins.Count-heights.Count,maxStack=body.MaxStackSize,observedPeak=peak,unsupported=unsupported.ToArray(),errors=issues,hints=hints};
 }
 public static void Main(string[] a){var resolver=new DefaultAssemblyResolver();foreach(var dir in a.Skip(2))resolver.AddSearchDirectory(Path.GetFullPath(dir));using(var asm=AssemblyDefinition.ReadAssembly(a[0],new ReaderParameters{AssemblyResolver=resolver})){
  var methods=Types(asm.MainModule.Types).SelectMany(t=>t.Methods).ToArray();var bodies=methods.Where(m=>m.HasBody).ToArray();var result=bodies.Select(Scan).ToArray();
  var json=new JavaScriptSerializer{MaxJsonLength=int.MaxValue};File.WriteAllText(a[1],json.Serialize(new{scope="all method bodies; CFG stack-height and adjacent-producer local-store checks; not complete ECMA verifier or native semantic proof",input=Path.GetFileName(a[0]),methodDefs=methods.Length,methodBodies=bodies.Length,results=result}));Console.WriteLine("SCANNED "+methods.Length+" MethodDef / "+bodies.Length+" bodies -> "+a[1]);
 }}
}
