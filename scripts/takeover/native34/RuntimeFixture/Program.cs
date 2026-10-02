using Mono.Cecil;
using CI=Mono.Cecil.Cil.Instruction;
using RO=System.Reflection.Emit.OpCodes;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.Json;
using System.Security.Cryptography;
public class Save {public int adventureLevel;public int[]? rescuePassNum;public bool[]? adventureHardPassed,almanac_DeviceLock;}
public class PlantSave {public string?[]? talentNames;}
public class EnemyManager {public int theWave;public bool finish;public float nextWaveTime,nextTestTime,testWavelength,shortWavelength,longWavelength;}
static class Fixture {
 static Dictionary<string,Type> Map=new(){["Save"]=typeof(Save),["PlantSave"]=typeof(PlantSave),["EnemyManager"]=typeof(EnemyManager),["System.Int32"]=typeof(int),["System.UInt32"]=typeof(uint),["System.Single"]=typeof(float),["System.Boolean"]=typeof(bool),["System.Void"]=typeof(void),["System.Object"]=typeof(object),["System.IntPtr"]=typeof(IntPtr),["System.String"]=typeof(string),["System.String[]"]=typeof(string[]),["System.Int32[]"]=typeof(int[]),["System.Boolean[]"]=typeof(bool[]),["System.NullReferenceException"]=typeof(NullReferenceException),["System.IndexOutOfRangeException"]=typeof(IndexOutOfRangeException),["ChallengeType"]=typeof(int)};
 static Type T(TypeReference t)=>t.IsByReference?T(((ByReferenceType)t).ElementType).MakeByRefType():Map.TryGetValue(t.FullName,out var type)?type:throw new Exception("Unmapped type "+t.FullName);
 delegate void U(uint p,uint q,ref float r,ref float g,ref float b,ref float a);
 static DynamicMethod Emit(MethodDefinition m,string mutation){
  var dm=new DynamicMethod(m.Name+mutation,T(m.ReturnType),(m.HasThis?new[]{m.DeclaringType.IsValueType?T(m.DeclaringType).MakeByRefType():T(m.DeclaringType)}:Array.Empty<Type>()).Concat(m.Parameters.Select(p=>T(p.ParameterType))).ToArray(),typeof(Fixture).Module,true){InitLocals=m.Body.InitLocals};
  var il=dm.GetILGenerator();var locals=m.Body.Variables.Select(v=>il.DeclareLocal(T(v.VariableType))).ToArray();var labels=m.Body.Instructions.ToDictionary(i=>i,i=>il.DefineLabel());
  var ops=typeof(RO).GetFields().Where(f=>f.FieldType==typeof(System.Reflection.Emit.OpCode)).Select(f=>(System.Reflection.Emit.OpCode)f.GetValue(null)!).ToDictionary(o=>o.Value);
  if(mutation=="illegal_stack")il.Emit(RO.Pop);
  bool changed=mutation=="illegal_stack";
  foreach(var i in m.Body.Instructions){
   il.MarkLabel(labels[i]);var op=ops[i.OpCode.Value];var a=i.Operand;
   if(mutation=="wrong_return"&&op==RO.Ret&&m.ReturnType.FullName!="System.Void"){
    il.Emit(RO.Pop);if(T(m.ReturnType)==typeof(float))il.Emit(RO.Ldc_R4,123f);else il.Emit(RO.Ldc_I4,0);changed=true;
   }
   if(mutation=="wrong_write"&&op==RO.Stfld&&a is FieldReference wf&&wf.Name=="nextWaveTime"){il.Emit(RO.Pop);il.Emit(RO.Ldc_R4,123f);changed=true;}
   if(mutation=="unordered_skip"&&op==RO.Bge_S){op=RO.Bge_Un_S;changed=true;}
   if(mutation=="invert_talent_test"&&op==RO.Brtrue_S){op=RO.Brfalse_S;changed=true;}
   if(mutation=="wrong_rescue_threshold"&&op==RO.Cgt){op=RO.Clt;changed=true;}
   if(mutation=="no_device_write"&&op==RO.Stelem_I1){il.Emit(RO.Pop);il.Emit(RO.Ldc_I4_0);changed=true;}
   if(a is CI target){il.Emit(op,labels[target]);continue;}
   if(a is CI[] ts){il.Emit(op,ts.Select(t=>labels[t]).ToArray());continue;}
   if(a is Mono.Cecil.Cil.VariableDefinition v){il.Emit(op,locals[v.Index]);continue;}
   if(a is ParameterDefinition p){il.Emit(op,(short)(p.Index+(m.HasThis?1:0)));continue;}
   if(a is TypeReference tr){il.Emit(op,T(tr));continue;}
   if(a is MethodReference mr){var mi=T(mr.DeclaringType).GetMethod(mr.Name,mr.Parameters.Select(p=>T(p.ParameterType)).ToArray())??throw new Exception("Missing method "+mr);il.Emit(op,mi);continue;}
   if(a is FieldReference field){il.Emit(op,T(field.DeclaringType).GetField(field.Name)!);continue;}
   if(a is int n){il.Emit(op,n);continue;}if(a is float f){il.Emit(op,f);continue;}if(a is sbyte b){il.Emit(op,b);continue;}
   if(a==null){il.Emit(op);continue;}throw new Exception("Unmapped operand "+a);
  }
  if(mutation!="none"&&!changed)throw new Exception("Mutation not applied "+mutation);
  return dm;
 }
 static float F(long n)=>BitConverter.Int32BitsToSingle(unchecked((int)n));
 static uint B(float f)=>unchecked((uint)BitConverter.SingleToInt32Bits(f));
 static bool Same(float a,long b)=>B(a)==(uint)b||(float.IsNaN(a)&&float.IsNaN(F(b)));
 static void MustThrow(DynamicMethod dm,object?[] args,Type kind,List<string> failures){try{dm.Invoke(null,args);failures.Add("missing-throw:"+kind.Name);}catch(TargetInvocationException e)when(e.InnerException?.GetType()==kind){}}
 static List<string> Cases(MethodDefinition m,DynamicMethod dm,List<long[]> rows,bool stop=false){
  var failures=new List<string>();int tok=m.MetadataToken.ToInt32(),ix=0;
  foreach(var r in rows){bool match;
   if(tok==0x060004C5){var save=new Save{adventureLevel=(int)r[1],rescuePassNum=r.Skip(3).Take(8).Select(v=>(int)v).ToArray(),adventureHardPassed=r.Skip(3).Take(8).Select(v=>v!=0).ToArray()};var z=(bool)dm.Invoke(null,new object?[]{save,(int)r[0],(int)r[2]})!;match=(z?1:0)==r[^1];}
   else if(tok==0x060004C6){var arr=Enumerable.Repeat(r[1]!=0,128).ToArray();var save=new Save{almanac_DeviceLock=arr};bool first=(bool)dm.Invoke(null,new object?[]{save,(int)r[0]})!,second=(bool)dm.Invoke(null,new object?[]{save,(int)r[0]})!;match=(first?1:0)==r[2]&&(second?1:0)==r[3]&&ReferenceEquals(arr,save.almanac_DeviceLock)&&arr.Select((v,k)=>v==(k==(int)r[0]?true:r[1]!=0)).All(v=>v);}
   else if(tok==0x060004D1){var save=new PlantSave{talentNames=r.Skip(1).Take((int)r[0]).Select(v=>v==0?null:v==1?string.Empty:"A").ToArray()};match=Convert.ToInt64(dm.Invoke(null,new object?[]{save}))==r[^1];}
   else if(tok==0x060001BA){var state=new EnemyManager{theWave=(int)r[1],finish=r[2]!=0,nextWaveTime=F(r[3]),testWavelength=F(r[4]),shortWavelength=F(r[5]),longWavelength=F(r[6]),nextTestTime=F(r[7])};dm.Invoke(null,new object?[]{state,(int)r[0]});
    bool arithmeticWave=(r[0]==0&&(int)r[1]%10==9)||r[0]==1;bool wave=arithmeticWave?Same(state.nextWaveTime,r[8]):B(state.nextWaveTime)==(uint)r[8];
    match=wave&&B(state.nextTestTime)==(uint)r[9]&&state.theWave==(int)r[1]&&state.finish==(r[2]!=0)&&B(state.testWavelength)==(uint)r[4]&&B(state.shortWavelength)==(uint)r[5]&&B(state.longWavelength)==(uint)r[6];
   }else throw new Exception("Unknown token");
   if(!match){failures.Add("row:"+ix);if(stop)break;}ix++;
  }
  if(tok==0x060004C5){
   foreach(int type in new[]{0,1,2})MustThrow(dm,new object?[]{null,type,0},typeof(NullReferenceException),failures);
   foreach(int type in new[]{1,2}){MustThrow(dm,new object?[]{new Save(),type,0},typeof(NullReferenceException),failures);foreach(int ix2 in new[]{-1,8})MustThrow(dm,new object?[]{new Save{rescuePassNum=new int[8],adventureHardPassed=new bool[8]},type,ix2},typeof(IndexOutOfRangeException),failures);}
   if((bool)dm.Invoke(null,new object?[]{null,3,0})!)failures.Add("unknown-mode-must-false");
  }else if(tok==0x060004C6){
   MustThrow(dm,new object?[]{null,0},typeof(NullReferenceException),failures);
   foreach(int index in new[]{0,127}){var save=new Save();bool z=(bool)dm.Invoke(null,new object?[]{save,index})!;if(z||save.almanac_DeviceLock?.Length!=128||!save.almanac_DeviceLock[index]||save.almanac_DeviceLock.Count(v=>v)!=1)failures.Add("allocation-size-or-write");}
   foreach(int index in new[]{-1,128}){var save=new Save();MustThrow(dm,new object?[]{save,index},typeof(IndexOutOfRangeException),failures);if(save.almanac_DeviceLock?.Length!=128||save.almanac_DeviceLock.Any(v=>v))failures.Add("failed-allocation-path-effects");}
   foreach(int index in new[]{-1,128})MustThrow(dm,new object?[]{new Save{almanac_DeviceLock=new bool[128]},index},typeof(IndexOutOfRangeException),failures);
  }else if(tok==0x060004D1){MustThrow(dm,new object?[]{null},typeof(NullReferenceException),failures);MustThrow(dm,new object?[]{new PlantSave()},typeof(NullReferenceException),failures);
  }else if(tok==0x060001BA){foreach(int mode in new[]{0,1,2})MustThrow(dm,new object?[]{null,mode},typeof(NullReferenceException),failures);foreach(int mode in new[]{3,int.MinValue,int.MaxValue})dm.Invoke(null,new object?[]{null,mode});}
  return failures;
 }
 static int Main(string[] args){try{
  using var a=AssemblyDefinition.ReadAssembly(args[0]);var all=File.ReadLines(args[1]).Select(l=>l.Split(' ')).GroupBy(l=>l[0]).ToDictionary(g=>g.Key,g=>g.Select(l=>l.Skip(1).Select(long.Parse).ToArray()).ToList());
  if(all.Count!=4)throw new Exception("Expected four original-native method oracles");
  var expected=new Dictionary<int,string>{[0x060004C5]="System.Boolean Save::LevelPassed(ChallengeType,System.Int32)",[0x060004C6]="System.Boolean Save::RecordDevice(System.Int32)",[0x060004D1]="System.Int32 PlantSave::GetTalentNum()",[0x060001BA]="System.Void EnemyManager::FlushedWavelenth(System.Int32)"};
  var positives=new List<object>();var controls=new List<object>();int bad=0,total=0,errors=0;bool detected=true;
  foreach(var (key,rows) in all){int token=Convert.ToInt32(key,16);var m=(MethodDefinition)a.MainModule.LookupToken(token);if(m.FullName!=expected[token])throw new Exception("Signature pin failed "+key);
   var failures=Cases(m,Emit(m,"none"),rows);bad+=failures.Count;int count=rows.Count+(token==0x060004C5?10:token==0x060004C6?7:token==0x060004D1?2:6);total+=count;positives.Add(new{token=key,signature=m.FullName,cases=count,failures});
   foreach(string mut in (args.Length>3?Array.Empty<string>():new[]{token==0x060001BA?"wrong_write":"wrong_return","illegal_stack"}.Concat(token==0x060004C5?new[]{"wrong_rescue_threshold"}:token==0x060004C6?new[]{"no_device_write"}:token==0x060004D1?new[]{"invert_talent_test"}:new[]{"unordered_skip"}))){
    bool reject=false;string? error=null;try{reject=Cases(m,Emit(m,mut),rows,true).Count>0;}catch(TargetInvocationException e)when(e.InnerException is InvalidProgramException){reject=true;}catch(InvalidProgramException){reject=true;}catch(Exception e){error=e.ToString();errors++;}
    detected&=reject&&error==null;controls.Add(new{token=key,mutation=mut,detected=reject,tool_error=error});
   }
  }
  bool pass=bad==0&&errors==0&&detected;var report=new{candidate_sha256=Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(args[0]))).ToLowerInvariant(),oracle_sha256=Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(args[1]))).ToLowerInvariant(),methods=all.Count,positive_cases=total,positive_failures=bad,negative_controls=controls.Count,tool_errors=errors,gate_pass=pass,scope="Actual CLR candidate bodies versus original PC state/array bodies. Initialized String.Empty and metadata preconditions; original inequality helper sees canonical empty/null/nonempty names only. Record native execution covers existing arrays; allocation and exceptions have separate CLR/native-CFG assertions. Copied/unmodified float fields compare bits; arithmetic NaNs compare classification without payload guarantee. Finite inputs; not Unity/iOS/device acceptance",positives,controls};
  File.WriteAllText(args[2],JsonSerializer.Serialize(report,new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine(JsonSerializer.Serialize(new{report.methods,report.positive_cases,report.positive_failures,report.negative_controls,report.tool_errors,report.gate_pass}));return pass?0:2;
 }catch(Exception e){Console.Error.WriteLine(e);return 3;}}
}
