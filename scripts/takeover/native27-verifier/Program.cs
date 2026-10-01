using Mono.Cecil;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.Json;
using CI=Mono.Cecil.Cil.Instruction;
using RO=System.Reflection.Emit.OpCodes;
public class TestInfrastructureException:Exception {public TestInfrastructureException(string text):base(text){}}
public static class Trace {
 public static bool InjectToolFault;public static List<string> Events=new();public static string Fail="";public static WaitBase? Made;public static object?[] ActionArgs=Array.Empty<object?>();public static Clip? ActionReceiver,SubscribeReceiver;public static Clip? CtorArg;
 public static void Reset(string fail){Events.Clear();Fail=fail;Made=null;ActionArgs=Array.Empty<object?>();ActionReceiver=SubscribeReceiver=CtorArg=null;}
 public static void Event(string s){if(InjectToolFault)throw new TestInfrastructureException("Injected invocation fault");Events.Add(s);if(Fail==s)throw new ApplicationException(s);}
}
public class Clip {
 void Act(string name,params object?[] args){Trace.ActionReceiver=this;Trace.ActionArgs=args;Trace.Event(name);}
 public void Play(string? s)=>Act("Play",s);public void GotoAndPlay(int i)=>Act("GotoAndPlay",i);public void GotoAndPlay(string? s,int i)=>Act("GotoAndPlay",s,i);public void GotoAndStop(int i)=>Act("GotoAndStop",i);
}
public class WaitBase {
 public Clip? subscribed;
 protected WaitBase(Clip? c){Trace.CtorArg=c;Trace.Made=this;Trace.Event("ctor");}
 protected void Sub(Clip? c){Trace.SubscribeReceiver=c;subscribed=c;Trace.Event("subscribe");}
}
public class StopWait:WaitBase {public StopWait(Clip? c):base(c){} public StopWait Subscribe(Clip? c){Sub(c);return null!;}}
public class RewindWait:WaitBase {public RewindWait(Clip? c):base(c){} public RewindWait Subscribe(Clip? c){Sub(c);return null!;}}
public class EitherWait:WaitBase {public EitherWait(Clip? c):base(c){} public EitherWait Subscribe(Clip? c){Sub(c);return null!;}}
public class PlayWait:WaitBase {public PlayWait(Clip? c):base(c){} public PlayWait Subscribe(Clip? c){Sub(c);return null!;}}
public class ClipData {} public class Assoc<T>{public int count;public int get_Count(){Trace.Event("count");return count;}}
public class Manager{public Assoc<ClipData>? _clips;public Assoc<Clip>? _controllers;}
static class Native27Verifier {
 static readonly Dictionary<string,Type> Types=new(){["FTRuntime.SwfClipController"]=typeof(Clip),["FTRuntime.SwfClip"]=typeof(ClipData),["FTRuntime.SwfManager"]=typeof(Manager),["FTRuntime.Yields.SwfWaitStopPlaying"]=typeof(StopWait),["FTRuntime.Yields.SwfWaitRewindPlaying"]=typeof(RewindWait),["FTRuntime.Yields.SwfWaitStopOrRewindPlaying"]=typeof(EitherWait),["FTRuntime.Yields.SwfWaitPlayStopped"]=typeof(PlayWait),["System.Object"]=typeof(object),["System.Int32"]=typeof(int),["System.Boolean"]=typeof(bool),["System.Void"]=typeof(void),["System.String"]=typeof(string),["System.NullReferenceException"]=typeof(NullReferenceException)};
 static Type T(TypeReference r){if(r is GenericInstanceType g&&g.ElementType.FullName=="FTRuntime.Internal.SwfAssocList`1")return typeof(Assoc<>).MakeGenericType(T(g.GenericArguments[0]));return Types[r.FullName];}

 // Expected outer contracts are pinned to the reviewed original PC instructions, not inferred from candidate calls.
 record Spec(int Token,string Signature,string? Action,string[] Arguments,Type Result,bool HasThis,string? CountField,string NativeCall);
 static Spec Getter(int token,string name,string field)=>new(token,"System.Int32 FTRuntime.SwfManager::"+name+"()",null,Array.Empty<string>(),typeof(int),true,field,"0x18065BAA0");
 static Spec Wrapper(int token,string name,string action,string[] arguments,Type result,string resultName,string nativeCall)=>new(token,"FTRuntime.Yields."+resultName+" FTRuntime.Yields.SwfWaitExtensions::"+name+"(FTRuntime.SwfClipController,"+string.Join(",",arguments)+")",action,arguments,result,false,null,nativeCall);
 static readonly Spec[] Contracts={
  Getter(0x0600087E,"get_clipCount","_clips"),Getter(0x0600087F,"get_controllerCount","_controllers"),
  Wrapper(0x060008AB,"PlayAndWaitStop","Play",new[]{"System.String"},typeof(StopWait),"SwfWaitStopPlaying","0x1803C68A0"),
  Wrapper(0x060008AD,"PlayAndWaitRewind","Play",new[]{"System.String"},typeof(RewindWait),"SwfWaitRewindPlaying","0x1803C68A0"),
  Wrapper(0x060008AF,"PlayAndWaitStopOrRewind","Play",new[]{"System.String"},typeof(EitherWait),"SwfWaitStopOrRewindPlaying","0x1803C68A0"),
  Wrapper(0x060008B0,"GotoAndPlayAndWaitStop","GotoAndPlay",new[]{"System.Int32"},typeof(StopWait),"SwfWaitStopPlaying","0x1803C60D0"),
  Wrapper(0x060008B2,"GotoAndPlayAndWaitRewind","GotoAndPlay",new[]{"System.Int32"},typeof(RewindWait),"SwfWaitRewindPlaying","0x1803C60D0"),
  Wrapper(0x060008B4,"GotoAndPlayAndWaitStopOrRewind","GotoAndPlay",new[]{"System.Int32"},typeof(EitherWait),"SwfWaitStopOrRewindPlaying","0x1803C60D0"),
  Wrapper(0x060008B8,"GotoAndStopAndWaitPlay","GotoAndStop",new[]{"System.Int32"},typeof(PlayWait),"SwfWaitPlayStopped","0x1803C6240"),
  Wrapper(0x060008B1,"GotoAndPlayAndWaitStop","GotoAndPlay",new[]{"System.String","System.Int32"},typeof(StopWait),"SwfWaitStopPlaying","0x1803C6030"),
  Wrapper(0x060008B3,"GotoAndPlayAndWaitRewind","GotoAndPlay",new[]{"System.String","System.Int32"},typeof(RewindWait),"SwfWaitRewindPlaying","0x1803C6030"),
  Wrapper(0x060008B5,"GotoAndPlayAndWaitStopOrRewind","GotoAndPlay",new[]{"System.String","System.Int32"},typeof(EitherWait),"SwfWaitStopOrRewindPlaying","0x1803C6030")
 };
 static Spec Contract(MethodDefinition m){var spec=Contracts.Single(x=>x.Token==m.MetadataToken.ToInt32());if(m.FullName!=spec.Signature||m.HasThis!=spec.HasThis||T(m.ReturnType)!=spec.Result)throw new TestInfrastructureException("Pinned signature mismatch: "+m.FullName);return spec;}
 static DynamicMethod Emit(MethodDefinition m,string mutant="none"){
  Contract(m);if(mutant=="fixture_fault_emit")throw new TestInfrastructureException("Injected emitter fault");
  var args=(m.HasThis?new[]{T(m.DeclaringType)}:Array.Empty<Type>()).Concat(m.Parameters.Select(p=>T(p.ParameterType))).ToArray();var dm=new DynamicMethod(m.Name+mutant,T(m.ReturnType),args,typeof(Native27Verifier).Module,true){InitLocals=m.Body.InitLocals};var il=dm.GetILGenerator();var locals=m.Body.Variables.Select(v=>il.DeclareLocal(T(v.VariableType))).ToArray();var labels=m.Body.Instructions.ToDictionary(i=>i,i=>il.DefineLabel());var ops=typeof(RO).GetFields(BindingFlags.Public|BindingFlags.Static).Where(f=>f.FieldType==typeof(System.Reflection.Emit.OpCode)).Select(f=>(System.Reflection.Emit.OpCode)f.GetValue(null)!).ToDictionary(o=>o.Value);
  if(mutant=="clr_invalid_stack")il.Emit(RO.Pop);
  foreach(var i in m.Body.Instructions){il.MarkLabel(labels[i]);var op=ops[i.OpCode.Value];var a=i.Operand;
   if(mutant=="invert_null"&&op==RO.Brtrue)op=RO.Brfalse;
   if(mutant=="exception_return"&&op==RO.Throw){il.Emit(RO.Pop);il.Emit(RO.Ldnull);il.Emit(RO.Ret);continue;}
   if(mutant=="ctor_controller"&&op==RO.Ldnull&&i.Next?.OpCode.Code==Mono.Cecil.Cil.Code.Newobj){il.Emit(RO.Ldarg_0);continue;}
   if(a is MethodReference c){var owner=T(c.DeclaringType);var ps=c.Parameters.Select(p=>T(p.ParameterType)).ToArray();bool action=c.DeclaringType.FullName=="FTRuntime.SwfClipController";
    if(mutant=="skip_action"&&action||mutant=="skip_subscribe"&&c.Name=="Subscribe"){foreach(var parameter in ps)il.Emit(RO.Pop);if(c.HasThis)il.Emit(RO.Pop);if(c.ReturnType.FullName!="System.Void")il.Emit(RO.Ldnull);continue;}
    if(c.Name==".ctor")il.Emit(op,owner.GetConstructor(ps)!);else il.Emit(op,owner.GetMethod(c.Name,ps)!);continue;
   }
   if(a is FieldReference field){var name=mutant=="wrong_count_field"?(field.Name=="_clips"?"_controllers":"_clips"):field.Name;il.Emit(op,T(field.DeclaringType).GetField(name)!);continue;}
   if(a is CI jump){il.Emit(op,labels[jump]);continue;}if(a is CI[] table){il.Emit(op,table.Select(j=>labels[j]).ToArray());continue;}
   if(a is Mono.Cecil.Cil.VariableDefinition v){int ix=v.Index;if(mutant=="return_subscribe"&&op==RO.Ldloc&&i.Next?.OpCode.Code==Mono.Cecil.Cil.Code.Ret&&T(v.VariableType).IsSubclassOf(typeof(WaitBase)))ix++;il.Emit(op,locals[ix]);continue;}
   if(a is ParameterDefinition p){il.Emit(op,(short)(p.Index+(m.HasThis?1:0)));continue;}if(a is int x){il.Emit(op,x);continue;}if(a is sbyte b){il.Emit(op,b);continue;}if(a is string s){il.Emit(op,s);continue;}if(a==null){il.Emit(op);continue;}throw new Exception("Unsupported "+a);
  }return dm;
 }
 static (object? Value,string? Error) Invoke(DynamicMethod dm,object?[] args){
  try{return(dm.Invoke(null,args),null);}
  catch(TargetInvocationException e) when(e.InnerException is NullReferenceException or ApplicationException or InvalidProgramException or System.Security.VerificationException){return(null,e.InnerException!.GetType().Name);}
  catch(InvalidProgramException e){return(null,e.GetType().Name);}
  catch(System.Security.VerificationException e){return(null,e.GetType().Name);}
  // Reflection/emitter/harness faults are never treated as evidence that a mutant was detected.
 }
 static List<object> Check(MethodDefinition m,DynamicMethod dm){var spec=Contract(m);var rows=new List<object>();if(spec.HasThis){foreach(int count in new[]{int.MinValue,0,1,9,int.MaxValue})foreach(string fail in new[]{"","count"}){var manager=new Manager{_clips=new(){count=count},_controllers=new(){count=unchecked(count+13)}};var expected=spec.CountField=="_clips"?manager._clips.count:manager._controllers.count;Trace.Reset(fail);var(v,e)=Invoke(dm,new object?[]{manager});rows.Add(new{token=$"0x{spec.Token:X8}",count,fail,error=e,pass=fail==""?Equals(v,expected)&&e==null&&Trace.Events.SequenceEqual(new[]{"count"}):e=="ApplicationException"&&Trace.Events.SequenceEqual(new[]{"count"})});}
   foreach(bool nullThis in new[]{false,true}){var manager=new Manager();Trace.Reset("");var(v,e)=Invoke(dm,new object?[]{nullThis?null:manager});rows.Add(new{token=$"0x{spec.Token:X8}",nullThis,error=e,pass=e=="NullReferenceException"&&Trace.Events.Count==0});}return rows;}
  var variants=spec.Arguments.Length==2?new object?[][]{new object?[]{null,int.MinValue},new object?[]{"",0},new object?[]{"run",int.MaxValue}}:spec.Arguments[0]=="System.String"?new object?[][]{new object?[]{null},new object?[]{""},new object?[]{"run"}}:new object?[][]{new object?[]{int.MinValue},new object?[]{0},new object?[]{int.MaxValue}};
  foreach(var values in variants)foreach(var fail in new[]{"",spec.Action!,"ctor","subscribe"}){var clip=new Clip();Trace.Reset(fail);var args=new object?[]{clip}.Concat(values).ToArray();var(v,e)=Invoke(dm,args);var expected=new List<string>{spec.Action!};if(fail!=spec.Action)expected.Add("ctor");if(fail!=spec.Action&&fail!="ctor")expected.Add("subscribe");bool pass=Trace.Events.SequenceEqual(expected)&&ReferenceEquals(Trace.ActionReceiver,clip)&&Trace.ActionArgs.SequenceEqual(values);if(fail=="")pass&=e==null&&ReferenceEquals(v,Trace.Made)&&v!=null&&v.GetType()==spec.Result&&v is WaitBase wait&&ReferenceEquals(wait.subscribed,clip)&&Trace.CtorArg==null&&ReferenceEquals(Trace.SubscribeReceiver,clip);else pass&=e=="ApplicationException";if(expected.Contains("ctor"))pass&=Trace.CtorArg==null;rows.Add(new{token=$"0x{spec.Token:X8}",values,fail,error=e,events=Trace.Events.ToArray(),expected_events=expected,pass});}
  foreach(var values in variants){Trace.Reset("");var(v,e)=Invoke(dm,new object?[]{null}.Concat(values).ToArray());rows.Add(new{token=$"0x{spec.Token:X8}",nullController=true,error=e,pass=e=="NullReferenceException"&&Trace.Events.Count==0});}return rows;
 }
 static bool Pass(object o)=>(bool)o.GetType().GetProperty("pass")!.GetValue(o)!;
 record MutantResult(string Token,string Mutation,bool Detected,string Status,int FailingCases,int ClrRejectedCases,string? ToolError);
 static MutantResult Evaluate(MethodDefinition m,string name){try{
  Trace.InjectToolFault=name=="fixture_fault_invoke";var rows=Check(m,Emit(m,name));int invalid=rows.Count(o=>o.GetType().GetProperty("error")?.GetValue(o) is string error&&error is "InvalidProgramException" or "VerificationException");int failed=rows.Count(o=>!Pass(o));return new($"0x{m.MetadataToken.ToInt32():X8}",name,failed>0,invalid>0?"CLR_REJECTED":failed>0?"BEHAVIOR_MISMATCH":"UNDETECTED",failed,invalid,null);
 }catch(Exception e){return new($"0x{m.MetadataToken.ToInt32():X8}",name,false,"TOOL_ERROR",0,0,e.GetType().Name+": "+e.Message);}finally{Trace.InjectToolFault=false;}}
 static MethodReference SwapAction(MethodDefinition m){var call=m.Body.Instructions.Single(i=>i.Operand is MethodReference c&&c.DeclaringType.FullName=="FTRuntime.SwfClipController");var before=(MethodReference)call.Operand;string other=before.Name=="GotoAndPlay"?"GotoAndStop":"GotoAndPlay";var replacement=m.Module.Types.Single(t=>t.FullName=="FTRuntime.SwfClipController").Methods.Single(c=>c.Name==other&&c.Parameters.Count==1&&c.Parameters[0].ParameterType.FullName=="System.Int32");call.Operand=replacement;return before;}
 static void RestoreAction(MethodDefinition m,MethodReference old){m.Body.Instructions.Single(i=>i.Operand is MethodReference c&&c.DeclaringType.FullName=="FTRuntime.SwfClipController").Operand=old;}
 static bool LegacyPass(MethodDefinition m,string oldFixture){var assembly=Assembly.LoadFrom(oldFixture);var type=assembly.GetType("Native27Fixture",true)!;var emit=type.GetMethod("Emit",BindingFlags.NonPublic|BindingFlags.Static)!;var check=type.GetMethod("Check",BindingFlags.NonPublic|BindingFlags.Static)!;var dynamic=(DynamicMethod)emit.Invoke(null,new object[]{m,"none"})!;var rows=(System.Collections.IEnumerable)check.Invoke(null,new object[]{m,dynamic})!;return rows.Cast<object>().All(Pass);}
 static void Main(string[] args){try{Run(args);}catch(Exception e){Console.Error.WriteLine("TOOL_ERROR: "+e);Environment.Exit(2);}}
 static void Run(string[] args){
  using var asm=AssemblyDefinition.ReadAssembly(args[0]);var rows=new List<object>();var mutants=new List<MutantResult>();var legacy=new List<object>();string mode=args.Length>3?args[3]:"normal";
  foreach(var spec in Contracts){var m=(MethodDefinition)asm.MainModule.LookupToken(spec.Token);Contract(m);rows.AddRange(Check(m,Emit(m)));foreach(var name in spec.HasThis?new[]{"wrong_count_field","invert_null","exception_return"}:new[]{"invert_null","exception_return","skip_action","ctor_controller","skip_subscribe","return_subscribe"})mutants.Add(Evaluate(m,name));
   if(!spec.HasThis&&spec.Arguments.SequenceEqual(new[]{"System.Int32"})){var original=SwapAction(m);try{bool oldAccepted=LegacyPass(m,args[2]);var result=Evaluate(m,"same_signature_wrong_action");mutants.Add(result);legacy.Add(new{token=$"0x{spec.Token:X8}",native_expected_action=spec.Action,legacy_accepted=oldAccepted,hardened_rejected=result.Detected,status=result.Status});}finally{RestoreAction(m,original);}}
   if(spec.Token is 0x0600087E or 0x060008AB)mutants.Add(Evaluate(m,"clr_invalid_stack"));
  }
  if(mode!="normal"){var m=(MethodDefinition)asm.MainModule.LookupToken(0x060008AB);mutants.Add(Evaluate(m,mode));}
  bool oldReproduced=legacy.All(o=>(bool)o.GetType().GetProperty("legacy_accepted")!.GetValue(o)!&&(bool)o.GetType().GetProperty("hardened_rejected")!.GetValue(o)!);int faults=mutants.Count(o=>o.Status=="TOOL_ERROR");bool passed=rows.All(Pass)&&mutants.All(o=>o.Detected)&&oldReproduced&&faults==0;
  var report=new{runtime=Environment.Version.ToString(),scope="Actual candidate outer CIL; native-pinned expected action/arguments/return/field; explicit helper doubles; original helper bodies and UnityPlayer not executed",mode,methods=Contracts.Length,positive_cases=rows.Count,positive_pass=rows.All(Pass),mutants,legacy_counterexamples=legacy,legacy_counterexamples_reproduced=oldReproduced,tool_errors=faults,gate_pass=passed,cases=rows,contracts=Contracts.Select(c=>new{token=$"0x{c.Token:X8}",c.Signature,c.Action,c.Arguments,result=c.Result.Name,c.CountField,c.NativeCall})};File.WriteAllText(args[1],JsonSerializer.Serialize(report,new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine(JsonSerializer.Serialize(new{report.mode,report.methods,report.positive_cases,report.positive_pass,mutants=mutants.Count,report.tool_errors,report.gate_pass,report.legacy_counterexamples_reproduced}));if(!passed)Environment.Exit(faults>0?2:1);
 }
}
