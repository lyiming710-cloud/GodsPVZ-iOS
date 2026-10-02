using Mono.Cecil;
using CI=Mono.Cecil.Cil.Instruction;
using RO=System.Reflection.Emit.OpCodes;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.Json;
using System.Security.Cryptography;
public class Clip {public Action<Clip>? OnChangeClipEvent,OnChangeSequenceEvent,OnChangeCurrentFrameEvent;}
public class Controller {public Action<Controller>? OnStopPlayingEvent,OnPlayStoppedEvent,OnRewindPlayingEvent;}
public class Callback {public int Id;public void C(Clip c){}public void K(Controller c){}}
public static class Fixture {
 static Dictionary<string,Type> Map=new(){["FTRuntime.SwfClip"]=typeof(Clip),["FTRuntime.SwfClipController"]=typeof(Controller),["System.Action`1"]=typeof(Action<>),["System.Delegate"]=typeof(Delegate),["System.Threading.Interlocked"]=typeof(Interlocked),["System.Int32"]=typeof(int),["System.UInt32"]=typeof(uint),["System.Single"]=typeof(float),["System.Boolean"]=typeof(bool),["System.Void"]=typeof(void),["System.Object"]=typeof(object),["System.IntPtr"]=typeof(IntPtr),["System.String"]=typeof(string)};
 static Type T(TypeReference t)=>t is GenericInstanceType g?T(g.ElementType).MakeGenericType(g.GenericArguments.Select(T).ToArray()):Map.TryGetValue(t.FullName,out var type)?type:throw new Exception("Unmapped type "+t.FullName);
 static int Attempts,Injected;static Queue<Delegate> Competitors=new();
 public static A? CheckedExchange<A>(ref A? location,A? value,A? comparand)where A:class{
  Attempts++;if(Competitors.Count>0){Interlocked.Exchange(ref location,(A)(object)Competitors.Dequeue());Injected++;}return Interlocked.CompareExchange(ref location,value,comparand);
 }
 static DynamicMethod Emit(MethodDefinition m,string mutation,bool instrumented){
  var dm=new DynamicMethod(m.Name+mutation+instrumented,T(m.ReturnType),new[]{T(m.DeclaringType),T(m.Parameters[0].ParameterType)},typeof(Fixture).Module,false){InitLocals=m.Body.InitLocals};var il=dm.GetILGenerator();var locals=m.Body.Variables.Select(v=>il.DeclareLocal(T(v.VariableType))).ToArray();var labels=m.Body.Instructions.ToDictionary(i=>i,i=>il.DefineLabel());var ops=typeof(RO).GetFields().Where(f=>f.FieldType==typeof(System.Reflection.Emit.OpCode)).Select(f=>(System.Reflection.Emit.OpCode)f.GetValue(null)!).ToDictionary(o=>o.Value);
  bool changed=false;if(mutation=="illegal_stack"){il.Emit(RO.Pop);changed=true;}
  foreach(var i in m.Body.Instructions){il.MarkLabel(labels[i]);var op=ops[i.OpCode.Value];var a=i.Operand;
   if(mutation=="no_retry"&&op==RO.Bne_Un_S){il.Emit(RO.Pop);il.Emit(RO.Pop);changed=true;continue;}
   if(mutation=="value_equality_retry"&&op==RO.Bne_Un_S){il.Emit(RO.Call,typeof(Delegate).GetMethod("op_Equality")!);il.Emit(RO.Brfalse_S,labels[(CI)a]);changed=true;continue;}
   if(a is CI target){il.Emit(op,labels[target]);continue;}
   if(a is Mono.Cecil.Cil.VariableDefinition v){il.Emit(op,locals[v.Index]);continue;}
   if(a is ParameterDefinition p){il.Emit(op,(short)(p.Index+1));continue;}
   if(a is GenericInstanceMethod gm){if(gm.ElementMethod.FullName!="!!0 System.Threading.Interlocked::CompareExchange(!!0&,!!0,!!0)")throw new Exception("Atomic method binding "+gm);var definition=instrumented?typeof(Fixture).GetMethod(nameof(CheckedExchange))!:typeof(Interlocked).GetMethods().Single(m=>m.Name=="CompareExchange"&&m.IsGenericMethodDefinition&&m.GetParameters().Length==3);il.Emit(op,definition.MakeGenericMethod(T(gm.GenericArguments.Single())));continue;}
   if(a is MethodReference mr){string name=mr.Name;if(mutation=="opposite_operator"&&name is "Combine" or "Remove"){name=name=="Combine"?"Remove":"Combine";changed=true;}var mi=T(mr.DeclaringType).GetMethod(name,BindingFlags.Public|BindingFlags.Instance|BindingFlags.Static,null,mr.Parameters.Select(p=>T(p.ParameterType)).ToArray(),null)??throw new Exception("Missing public method "+mr);il.Emit(op,mi);continue;}
   if(a is FieldReference f){il.Emit(op,T(f.DeclaringType).GetField(f.Name)!);continue;}
   if(a is TypeReference tr){il.Emit(op,T(tr));continue;}
   if(a is int n){il.Emit(op,n);continue;}if(a==null){il.Emit(op);continue;}throw new Exception("Unsupported operand "+a);
  }
  if(mutation!="none"&&!changed)throw new Exception("Mutation not applied");return dm;
 }
 static Delegate? Build(int[] labels,Dictionary<int,Delegate> callbacks){Delegate? d=null;foreach(int label in labels)d=Delegate.Combine(d,callbacks[label]);return d;}
 static int[] Sequence(Delegate? d)=>d?.GetInvocationList().Select(v=>((Callback)v.Target!).Id).ToArray()??Array.Empty<int>();
 record Row(int[] Initial,int[] Value,int Forced,int[] Competitor,int Attempts,int[] Final);
 static Row Parse(long[] r){int p=0;int[] Seq(){int n=(int)r[p++];return r.Skip(p).Take(n).Select(v=>(int)v).ToArray().Tap(()=>p+=n);}var a=Seq();var v=Seq();int forced=(int)r[p++];var c=Seq();int attempts=(int)r[p++];var z=Seq();if(p!=r.Length)throw new Exception("Bad native row");return new(a,v,forced,c,attempts,z);}
 static A Tap<A>(this A v,Action a){a();return v;}
 static List<string> Cases(MethodDefinition m,DynamicMethod instrumented,DynamicMethod natural,List<Row> rows,bool stop=false){
  var failures=new List<string>();var owner=T(m.DeclaringType);var field=owner.GetField(m.Body.Instructions.Select(i=>i.Operand).OfType<FieldReference>().First().Name)!;var fields=owner.GetFields();int ix=0;
  foreach(var r in rows){object h=Activator.CreateInstance(owner)!;var callbacks=Enumerable.Range(0,4).ToDictionary(i=>i,i=>{var c=new Callback{Id=i};return owner==typeof(Clip)?(Delegate)new Action<Clip>(c.C):new Action<Controller>(c.K);});Delegate? initial=Build(r.Initial,callbacks),value=Build(r.Value,callbacks),competitor=Build(r.Competitor,callbacks);
   foreach(var f in fields)f.SetValue(h,callbacks[3]);field.SetValue(h,initial);var before=fields.ToDictionary(f=>f.Name,f=>f.GetValue(h));Competitors=new(Enumerable.Range(0,r.Forced).Select(_=>(Delegate)competitor!.Clone()));Attempts=Injected=0;instrumented.Invoke(null,new object?[]{h,value});bool match=Sequence((Delegate?)field.GetValue(h)).SequenceEqual(r.Final)&&Attempts==r.Attempts&&Injected==r.Forced&&Competitors.Count==0&&fields.Where(f=>f!=field).All(f=>ReferenceEquals(before[f.Name],f.GetValue(h)));
   if(r.Forced==0){field.SetValue(h,initial);natural.Invoke(null,new object?[]{h,value});match&=Sequence((Delegate?)field.GetValue(h)).SequenceEqual(r.Final)&&fields.Where(f=>f!=field).All(f=>ReferenceEquals(before[f.Name],f.GetValue(h)));}
   if(!match){failures.Add("row:"+ix);if(stop)return failures;}ix++;
  }
  foreach(var dm in new[]{instrumented,natural}){Competitors.Clear();try{dm.Invoke(null,new object?[]{null,null});failures.Add("null-this-no-exception");}catch(TargetInvocationException e)when(e.InnerException is NullReferenceException){}}
  return failures;
 }
 static object Stress(Mono.Cecil.ModuleDefinition module){var results=new List<object>();foreach(var pair in new[]{(0x06000822,0x06000823),(0x06000824,0x06000825),(0x06000826,0x06000827),(0x06000858,0x06000859),(0x0600085A,0x0600085B),(0x0600085C,0x0600085D)}){var add=(MethodDefinition)module.LookupToken(pair.Item1);var remove=(MethodDefinition)module.LookupToken(pair.Item2);var dmA=Emit(add,"none",false);var dmR=Emit(remove,"none",false);var owner=T(add.DeclaringType);var field=owner.GetField(add.Body.Instructions.Select(i=>i.Operand).OfType<FieldReference>().First().Name)!;object h=Activator.CreateInstance(owner)!;int[] counts=new int[256];Delegate[] callbacks=Enumerable.Range(0,256).Select(i=>owner==typeof(Clip)?(Delegate)new Action<Clip>(_=>Interlocked.Increment(ref counts[i])):new Action<Controller>(_=>Interlocked.Increment(ref counts[i]))).ToArray();Parallel.For(0,8,worker=>{for(int i=worker*32;i<(worker+1)*32;i++)dmA.Invoke(null,new object?[]{h,callbacks[i]});});var d=(Delegate?)field.GetValue(h);bool combined=d?.GetInvocationList().Length==256;d?.DynamicInvoke(h);bool invoked=counts.All(v=>v==1);Parallel.For(0,8,worker=>{for(int i=worker*32;i<(worker+1)*32;i++)dmR.Invoke(null,new object?[]{h,callbacks[i]});});bool removed=field.GetValue(h)==null;if(!combined||!invoked||!removed)throw new Exception("Real CLR concurrent event accessor failure "+add.Name);results.Add(new{add=pair.Item1.ToString("X8"),remove=pair.Item2.ToString("X8"),workers=8,subscriptions=256,combined,invoked,removed});}return results;}
 public static int Main(string[] args){try{using var a=AssemblyDefinition.ReadAssembly(args[0]);var rows=File.ReadLines(args[1]).Select(l=>l.Split(' ')).GroupBy(l=>l[0]).ToDictionary(g=>g.Key,g=>g.Select(l=>Parse(l.Skip(1).Select(long.Parse).ToArray())).ToList());if(rows.Count!=12)throw new Exception("Expected12 native accessors");int bad=0,errors=0,total=0,naturalCases=0;bool detected=true;var positives=new List<object>();var controls=new List<object>();
  foreach(var(key,rs)in rows){int token=Convert.ToInt32(key,16);var m=(MethodDefinition)a.MainModule.LookupToken(token);if(!(m.Name.StartsWith("add_")||m.Name.StartsWith("remove_"))||m.Parameters.Count!=1)throw new Exception("Accessor signature pin");var failures=Cases(m,Emit(m,"none",true),Emit(m,"none",false),rs);bad+=failures.Count;int natural=rs.Count(r=>r.Forced==0);naturalCases+=natural;total+=rs.Count+natural+2;positives.Add(new{token=key,cases=rs.Count,natural_bcl_cases=natural,failures});
   foreach(string mut in(args.Length>3?Array.Empty<string>():new[]{"opposite_operator","no_retry","value_equality_retry","illegal_stack"})){bool reject=false;string? err=null;try{reject=Cases(m,Emit(m,mut,true),Emit(m,mut,false),rs,true).Count>0;}catch(TargetInvocationException e)when(e.InnerException is InvalidProgramException){reject=true;}catch(InvalidProgramException){reject=true;}catch(Exception e){err=e.ToString();errors++;}detected&=reject&&err==null;controls.Add(new{token=key,mutation=mut,detected=reject,tool_error=err});}
  }
  object? stress=bad==0?Stress(a.MainModule):null;bool pass=bad==0&&errors==0&&detected;var report=new{candidate_sha256=Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(args[0]))).ToLowerInvariant(),methods=12,positive_cases=total,native_rows=rows.Sum(r=>r.Value.Count),natural_bcl_cases=naturalCases,positive_failures=bad,negative_controls=controls.Count,tool_errors=errors,gate_pass=pass,scope="Unchanged original native event caller and CAS instructions execute under metadata and delegate/typecast/GC-barrier dependency doubles. Native observations test atomic retry CFG and field isolation. Actual CLR candidate runs public BCL Delegate.Combine/Remove and Interlocked generic CAS; deterministic competing updates use a wrapper around real CLR CAS. Natural paths and six concurrent 8-worker event groups use actual BCL CAS directly. No original delegate-engine/Unity startup or iOS/device equivalence claim",positives,controls,stress};File.WriteAllText(args[2],JsonSerializer.Serialize(report,new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine(JsonSerializer.Serialize(new{report.positive_cases,report.native_rows,report.natural_bcl_cases,report.positive_failures,report.negative_controls,report.tool_errors,report.gate_pass}));return pass?0:2;
 }catch(Exception e){Console.Error.WriteLine(e);return 3;}}
}
