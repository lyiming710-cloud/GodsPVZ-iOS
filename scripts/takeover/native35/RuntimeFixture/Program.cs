using Mono.Cecil;
using CI=Mono.Cecil.Cil.Instruction;
using RO=System.Reflection.Emit.OpCodes;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.Json;
using System.Security.Cryptography;
public class Event3<A,B,C>{public int Id;public Action<A,B,C>? Action;public void Invoke(A a,B b,C c)=>Action?.Invoke(a,b,c);}
public class Handler{
 public class WordEvent:Event3<string,int,int>{}
 public class LineEvent:Event3<string,int,int>{}
 public class LinkEvent:Event3<string,string,int>{}
 public WordEvent? m_OnWordSelection;public LineEvent? m_OnLineSelection;public LinkEvent? m_OnLinkSelection;
}
public class CallbackFailure:Exception{}
static class Fixture {
 static Dictionary<string,Type> Map=new(){["TMPro.TMP_TextEventHandler"]=typeof(Handler),["TMPro.TMP_TextEventHandler/WordSelectionEvent"]=typeof(Handler.WordEvent),["TMPro.TMP_TextEventHandler/LineSelectionEvent"]=typeof(Handler.LineEvent),["TMPro.TMP_TextEventHandler/LinkSelectionEvent"]=typeof(Handler.LinkEvent),["UnityEngine.Events.UnityEvent`3"]=typeof(Event3<,,>),["System.Int32"]=typeof(int),["System.UInt32"]=typeof(uint),["System.Char"]=typeof(char),["System.Single"]=typeof(float),["System.Boolean"]=typeof(bool),["System.Void"]=typeof(void),["System.Object"]=typeof(object),["System.IntPtr"]=typeof(IntPtr),["System.String"]=typeof(string)};
 static Type T(TypeReference t)=>t is GenericInstanceType g?T(g.ElementType).MakeGenericType(g.GenericArguments.Select(T).ToArray()):Map.TryGetValue(t.FullName,out var type)?type:throw new Exception("Unmapped type "+t.FullName);
 static Type Bound(TypeReference t,MethodReference m)=>t is GenericParameter g&&m.DeclaringType is GenericInstanceType owner?T(owner.GenericArguments[g.Position]):T(t);
 static DynamicMethod Emit(MethodDefinition m,string mutation){
  // Public fields and public BCL methods only. No SkipVisibility private access.
  var dm=new DynamicMethod(m.Name+mutation,T(m.ReturnType),(m.HasThis?new[]{T(m.DeclaringType)}:Array.Empty<Type>()).Concat(m.Parameters.Select(p=>T(p.ParameterType))).ToArray(),typeof(Fixture).Module,false){InitLocals=m.Body.InitLocals};
  var il=dm.GetILGenerator();var locals=m.Body.Variables.Select(v=>il.DeclareLocal(T(v.VariableType))).ToArray();var labels=m.Body.Instructions.ToDictionary(i=>i,i=>il.DefineLabel());
  var ops=typeof(RO).GetFields().Where(f=>f.FieldType==typeof(System.Reflection.Emit.OpCode)).Select(f=>(System.Reflection.Emit.OpCode)f.GetValue(null)!).ToDictionary(o=>o.Value);
  if(mutation=="illegal_stack")il.Emit(RO.Pop);bool changed=mutation=="illegal_stack";
  foreach(var i in m.Body.Instructions){
   il.MarkLabel(labels[i]);var op=ops[i.OpCode.Value];var a=i.Operand;
   if(mutation=="wrong_seed"&&op==RO.Ldc_I4&&a is int seed&&seed==unchecked((int)0x811C9DC5)){a=seed+1;changed=true;}
   if(mutation=="wrong_index"&&(op==RO.Ldloc_S||op==RO.Ldloc)&&a is Mono.Cecil.Cil.VariableDefinition vix&&vix.Index==8&&i.Next?.Operand is MethodReference next&&next.Name=="get_Chars"){il.Emit(RO.Ldc_I4_0);changed=true;continue;}
   if(mutation=="wrong_guard"&&op==RO.Brfalse_S){op=RO.Brtrue_S;changed=true;}
   if(mutation=="drop_callback"&&a is MethodReference inv&&inv.Name=="Invoke"){for(int k=0;k<4;k++)il.Emit(RO.Pop);changed=true;continue;}
   if(mutation=="swap_arguments"){
    if(m.MetadataToken.ToInt32()==0x0600073C&&(op==RO.Ldarg_1||op==RO.Ldarg_2)){op=op==RO.Ldarg_1?RO.Ldarg_2:RO.Ldarg_1;changed=true;}
    else if(m.MetadataToken.ToInt32()!=0x0600073C&&(op==RO.Ldarg_2||op==RO.Ldarg_3)){op=op==RO.Ldarg_2?RO.Ldarg_3:RO.Ldarg_2;changed=true;}
   }
   if(a is CI target){il.Emit(op,labels[target]);continue;}
   if(a is CI[] ts){il.Emit(op,ts.Select(t=>labels[t]).ToArray());continue;}
   if(a is Mono.Cecil.Cil.VariableDefinition v){il.Emit(op,locals[v.Index]);continue;}
   if(a is ParameterDefinition p){il.Emit(op,(short)(p.Index+(m.HasThis?1:0)));continue;}
   if(a is MethodReference mr){var mi=T(mr.DeclaringType).GetMethod(mr.Name,BindingFlags.Public|BindingFlags.Instance|BindingFlags.Static,null,mr.Parameters.Select(p=>Bound(p.ParameterType,mr)).ToArray(),null)??throw new Exception("Missing public method "+mr);il.Emit(op,mi);continue;}
   if(a is FieldReference f){il.Emit(op,T(f.DeclaringType).GetField(f.Name,BindingFlags.Public|BindingFlags.Instance|BindingFlags.Static)??throw new Exception("Missing public field "+f));continue;}
   if(a is TypeReference tr){il.Emit(op,T(tr));continue;}
   if(a is int n){il.Emit(op,n);continue;}if(a is sbyte b){il.Emit(op,b);continue;}
   if(a==null){il.Emit(op);continue;}throw new Exception("Unmapped operand "+a);
  }
  if(mutation!="none"&&!changed)throw new Exception("Mutation not applied "+mutation);return dm;
 }
 static int Label(string? s,string?[] values){for(int i=0;i<values.Length;i++)if(ReferenceEquals(s,values[i]))return i;throw new Exception("String reference was not forwarded");}
 static List<string> Cases(MethodDefinition m,DynamicMethod dm,List<long[]> rows,bool stop=false){
  var failures=new List<string>();int tok=m.MetadataToken.ToInt32(),ix=0;
  foreach(var r in rows){bool match;
   if(tok==0x0600090C){string? str=r[0]<0?null:new string(r.Skip(1).Take((int)r[0]).Select(v=>(char)v).ToArray());match=Convert.ToUInt32(dm.Invoke(null,new object?[]{str}))==(uint)r[^1];}
   else {
    string?[] vals={null,new string('A',1),new string('B',1),new string('A',1)};int count=0,receiver=0,a=0,b=0,x=0,y=0;
    void Word(int id,string s,int i,int j){count++;receiver=id;a=Label(s,vals);x=i;y=j;}
    void Link(string s,string t,int i){count++;receiver=3;a=Label(s,vals);b=Label(t,vals);x=i;}
    var h=new Handler{m_OnWordSelection=new Handler.WordEvent{Id=1,Action=(s,i,j)=>Word(1,s,i,j)},m_OnLineSelection=new Handler.LineEvent{Id=2,Action=(s,i,j)=>Word(2,s,i,j)},m_OnLinkSelection=new Handler.LinkEvent{Id=3,Action=Link}};
    if(r[0]==0){if(tok==0x0600073A)h.m_OnWordSelection=null;else if(tok==0x0600073B)h.m_OnLineSelection=null;else h.m_OnLinkSelection=null;}
    var oldW=h.m_OnWordSelection;var oldL=h.m_OnLineSelection;var oldK=h.m_OnLinkSelection;
    try{dm.Invoke(null,tok==0x0600073C?new object?[]{h,vals[(int)r[1]],vals[(int)r[2]],(int)r[3]}:new object?[]{h,vals[(int)r[1]],(int)r[3],(int)r[4]});match=count==r[5]&&receiver==r[6]&&a==r[7]&&b==r[8]&&x==r[9]&&y==r[10]&&ReferenceEquals(oldW,h.m_OnWordSelection)&&ReferenceEquals(oldL,h.m_OnLineSelection)&&ReferenceEquals(oldK,h.m_OnLinkSelection);}
    catch(TargetInvocationException e)when(e.InnerException is NullReferenceException){match=false;}
   }
   if(!match){failures.Add("row:"+ix);if(stop)return failures;}ix++;
  }
  if(tok!=0x0600090C){
   object?[] Args(Handler? h)=>tok==0x0600073C?new object?[]{h,null,null,0}:new object?[]{h,null,0,0};
   try{dm.Invoke(null,Args(null));failures.Add("null-this-missing-throw");}catch(TargetInvocationException e)when(e.InnerException is NullReferenceException){}
   dm.Invoke(null,Args(new Handler()));
   var sentinel=new CallbackFailure();int count=0;var holder=new Handler();
   if(tok==0x0600073A)holder.m_OnWordSelection=new Handler.WordEvent{Action=(a,b,c)=>{count++;throw sentinel;}};
   else if(tok==0x0600073B)holder.m_OnLineSelection=new Handler.LineEvent{Action=(a,b,c)=>{count++;throw sentinel;}};
   else holder.m_OnLinkSelection=new Handler.LinkEvent{Action=(a,b,c)=>{count++;throw sentinel;}};
   try{dm.Invoke(null,Args(holder));failures.Add("callback-exception-missing");}catch(TargetInvocationException e)when(ReferenceEquals(e.InnerException,sentinel)){}
   if(count!=1)failures.Add("callback-exception-count");
  }
  return failures;
 }
 static int Main(string[] args){try{
  using var a=AssemblyDefinition.ReadAssembly(args[0]);var all=File.ReadLines(args[1]).Select(l=>l.Split(' ')).GroupBy(l=>l[0]).ToDictionary(g=>g.Key,g=>g.Select(l=>l.Skip(1).Select(long.Parse).ToArray()).ToList());
  if(all.Count!=4)throw new Exception("Expected four native oracles");
  var expected=new Dictionary<int,string>{[0x0600090C]="System.UInt32 <PrivateImplementationDetails>::ComputeStringHash(System.String)",[0x0600073A]="System.Void TMPro.TMP_TextEventHandler::SendOnWordSelection(System.String,System.Int32,System.Int32)",[0x0600073B]="System.Void TMPro.TMP_TextEventHandler::SendOnLineSelection(System.String,System.Int32,System.Int32)",[0x0600073C]="System.Void TMPro.TMP_TextEventHandler::SendOnLinkSelection(System.String,System.String,System.Int32)"};
  var positives=new List<object>();var controls=new List<object>();int bad=0,total=0,errors=0;bool detected=true;
  foreach(var(key,rows)in all){int token=Convert.ToInt32(key,16);var m=(MethodDefinition)a.MainModule.LookupToken(token);if(m.FullName!=expected[token])throw new Exception("Signature pin "+key);var failures=Cases(m,Emit(m,"none"),rows);bad+=failures.Count;int count=rows.Count+(token==0x0600090C?0:3);total+=count;positives.Add(new{token=key,signature=m.FullName,cases=count,failures});
   foreach(string mut in(args.Length>3?Array.Empty<string>():token==0x0600090C?new[]{"wrong_seed","wrong_index","illegal_stack"}:new[]{"drop_callback","swap_arguments","wrong_guard","illegal_stack"})){
    bool reject=false;string? error=null;try{reject=Cases(m,Emit(m,mut),rows,true).Count>0;}catch(TargetInvocationException e)when(e.InnerException is InvalidProgramException){reject=true;}catch(InvalidProgramException){reject=true;}catch(Exception e){error=e.ToString();errors++;}
    detected&=reject&&error==null;controls.Add(new{token=key,mutation=mut,detected=reject,tool_error=error});
   }
  }
  bool pass=bad==0&&errors==0&&detected;var report=new{candidate_sha256=Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(args[0]))).ToLowerInvariant(),oracle_sha256=Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(args[1]))).ToLowerInvariant(),methods=all.Count,positive_cases=total,positive_failures=bad,negative_controls=controls.Count,tool_errors=errors,gate_pass=pass,scope="Hash actual CLR versus unchanged original hash/get_Chars machine code, exhaustive single UTF16 units plus finite sequences. Events unchanged original caller code versus actual CLR forwarding, receiver and argument identity/call count; two engine Invoke dependency recording doubles and initialized metadata preconditions. CLR adds null-this and callback exception assertions. Does not execute UnityEvent engine implementation or Unity startup; no iOS/device acceptance",positives,controls};File.WriteAllText(args[2],JsonSerializer.Serialize(report,new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine(JsonSerializer.Serialize(new{report.methods,report.positive_cases,report.positive_failures,report.negative_controls,report.tool_errors,report.gate_pass}));return pass?0:2;
 }catch(Exception e){Console.Error.WriteLine(e);return 3;}}
}
