using Mono.Cecil;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.Json;
using CI=Mono.Cecil.Cil.Instruction;
using RO=System.Reflection.Emit.OpCodes;
public static class Trace {
 public static List<string> Events=new();public static string Fail="";public static WaitBase? Made;public static object?[] ActionArgs=Array.Empty<object?>();public static Clip? ActionReceiver,SubscribeReceiver;public static Clip? CtorArg;
 public static void Reset(string fail){Events.Clear();Fail=fail;Made=null;ActionArgs=Array.Empty<object?>();ActionReceiver=SubscribeReceiver=CtorArg=null;}
 public static void Event(string s){Events.Add(s);if(Fail==s)throw new ApplicationException(s);}
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
static class Native27Fixture {
 static readonly Dictionary<string,Type> Types=new(){["FTRuntime.SwfClipController"]=typeof(Clip),["FTRuntime.SwfClip"]=typeof(ClipData),["FTRuntime.SwfManager"]=typeof(Manager),["FTRuntime.Yields.SwfWaitStopPlaying"]=typeof(StopWait),["FTRuntime.Yields.SwfWaitRewindPlaying"]=typeof(RewindWait),["FTRuntime.Yields.SwfWaitStopOrRewindPlaying"]=typeof(EitherWait),["FTRuntime.Yields.SwfWaitPlayStopped"]=typeof(PlayWait),["System.Object"]=typeof(object),["System.Int32"]=typeof(int),["System.Boolean"]=typeof(bool),["System.Void"]=typeof(void),["System.String"]=typeof(string),["System.NullReferenceException"]=typeof(NullReferenceException)};
 static Type T(TypeReference r){if(r is GenericInstanceType g&&g.ElementType.FullName=="FTRuntime.Internal.SwfAssocList`1")return typeof(Assoc<>).MakeGenericType(T(g.GenericArguments[0]));return Types[r.FullName];}
 static readonly int[] Tokens={0x0600087E,0x0600087F,0x060008AB,0x060008AD,0x060008AF,0x060008B0,0x060008B2,0x060008B4,0x060008B8,0x060008B1,0x060008B3,0x060008B5};
 static DynamicMethod Emit(MethodDefinition m,string mutant="none"){
  var args=(m.HasThis?new[]{T(m.DeclaringType)}:Array.Empty<Type>()).Concat(m.Parameters.Select(p=>T(p.ParameterType))).ToArray();var dm=new DynamicMethod(m.Name+mutant,T(m.ReturnType),args,typeof(Native27Fixture).Module,true){InitLocals=m.Body.InitLocals};var il=dm.GetILGenerator();var locals=m.Body.Variables.Select(v=>il.DeclareLocal(T(v.VariableType))).ToArray();var labels=m.Body.Instructions.ToDictionary(i=>i,i=>il.DefineLabel());var ops=typeof(RO).GetFields(BindingFlags.Public|BindingFlags.Static).Where(f=>f.FieldType==typeof(System.Reflection.Emit.OpCode)).Select(f=>(System.Reflection.Emit.OpCode)f.GetValue(null)!).ToDictionary(o=>o.Value);
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
 static (object? Value,string? Error) Invoke(DynamicMethod dm,object?[] args){try{return(dm.Invoke(null,args),null);}catch(TargetInvocationException e){return(null,e.InnerException!.GetType().Name);}catch(Exception e){return(null,e.GetType().Name);}}
 static List<object> Check(MethodDefinition m,DynamicMethod dm){var rows=new List<object>();if(m.HasThis){foreach(int count in new[]{int.MinValue,0,1,9,int.MaxValue})foreach(string fail in new[]{"","count"}){var manager=new Manager{_clips=new(){count=count},_controllers=new(){count=count+unchecked(13)}};bool clips=m.Name=="get_clipCount";var expected=clips?manager._clips.count:manager._controllers.count;Trace.Reset(fail);var(v,e)=Invoke(dm,new object?[]{manager});rows.Add(new{token=m.MetadataToken.ToString(),count,fail,pass=fail==""?Equals(v,expected)&&e==null&&Trace.Events.SequenceEqual(new[]{"count"}):e=="ApplicationException"&&Trace.Events.SequenceEqual(new[]{"count"})});}
   foreach(bool nullThis in new[]{false,true}){var manager=new Manager();Trace.Reset("");var(v,e)=Invoke(dm,new object?[]{nullThis?null:manager});rows.Add(new{token=m.MetadataToken.ToString(),nullThis,pass=e=="NullReferenceException"&&Trace.Events.Count==0});}return rows;}
  var action=m.Body.Instructions.Select(i=>i.Operand).OfType<MethodReference>().Single(c=>c.DeclaringType.FullName=="FTRuntime.SwfClipController");var variants=action.Parameters.Count==2?new object?[][]{new object?[]{null,int.MinValue},new object?[]{"",0},new object?[]{"run",int.MaxValue}}:action.Parameters[0].ParameterType.FullName=="System.String"?new object?[][]{new object?[]{null},new object?[]{""},new object?[]{"run"}}:new object?[][]{new object?[]{int.MinValue},new object?[]{0},new object?[]{int.MaxValue}};
  foreach(var values in variants)foreach(var fail in new[]{"",action.Name,"ctor","subscribe"}){var clip=new Clip();Trace.Reset(fail);var args=new object?[]{clip}.Concat(values).ToArray();var(v,e)=Invoke(dm,args);var expected=new List<string>{action.Name};if(fail!=action.Name)expected.Add("ctor");if(fail!=action.Name&&fail!="ctor")expected.Add("subscribe");bool pass=Trace.Events.SequenceEqual(expected)&&ReferenceEquals(Trace.ActionReceiver,clip)&&Trace.ActionArgs.SequenceEqual(values);if(fail=="")pass&=e==null&&ReferenceEquals(v,Trace.Made)&&v is WaitBase wait&&ReferenceEquals(wait.subscribed,clip)&&Trace.CtorArg==null&&ReferenceEquals(Trace.SubscribeReceiver,clip);else pass&=e=="ApplicationException";if(expected.Contains("ctor"))pass&=Trace.CtorArg==null;rows.Add(new{token=m.MetadataToken.ToString(),values,fail,pass});}
  foreach(var values in variants){Trace.Reset("");var(v,e)=Invoke(dm,new object?[]{null}.Concat(values).ToArray());rows.Add(new{token=m.MetadataToken.ToString(),nullController=true,pass=e=="NullReferenceException"&&Trace.Events.Count==0});}return rows;
 }
 static bool Pass(object o)=>(bool)o.GetType().GetProperty("pass")!.GetValue(o)!;
 static void Main(string[] args){using var asm=AssemblyDefinition.ReadAssembly(args[0]);var rows=new List<object>();var mutants=new List<object>();foreach(int token in Tokens){var m=(MethodDefinition)asm.MainModule.LookupToken(token);rows.AddRange(Check(m,Emit(m)));foreach(var name in m.HasThis?new[]{"wrong_count_field","invert_null","exception_return"}:new[]{"invert_null","exception_return","skip_action","ctor_controller","skip_subscribe","return_subscribe"}){bool detected;string? rejection=null;try{detected=Check(m,Emit(m,name)).Any(o=>!Pass(o));}catch(Exception e){detected=true;rejection=e.GetType().Name+": "+e.Message;}mutants.Add(new{token=$"0x{token:X8}",mutation=name,detected,rejection});}}
  var report=new{runtime=Environment.Version.ToString(),scope="Actual candidate CIL with explicit Swf list/controller/yield helper doubles; these original helper bodies and Unity player are not executed",methods=Tokens.Length,positive_cases=rows.Count,positive_pass=rows.All(Pass),mutants_detected=mutants.All(o=>(bool)o.GetType().GetProperty("detected")!.GetValue(o)!),cases=rows,mutants};File.WriteAllText(args[1],JsonSerializer.Serialize(report,new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine(JsonSerializer.Serialize(new{report.methods,report.positive_cases,report.positive_pass,mutants=mutants.Count,report.mutants_detected}));if(!report.positive_pass||!report.mutants_detected)Environment.Exit(1);
 }
}
