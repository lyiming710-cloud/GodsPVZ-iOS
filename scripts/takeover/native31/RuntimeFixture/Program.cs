using Mono.Cecil;
using CI=Mono.Cecil.Cil.Instruction;
using RO=System.Reflection.Emit.OpCodes;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.Json;
using System.Security.Cryptography;
public class Plant {public int ID,state;}
public class Zombie {}
public class Projectile {}
public class SunManager {}
static class Fixture {
 static Dictionary<string,Type> Map=new(){["Plant"]=typeof(Plant),["Zombie"]=typeof(Zombie),["Projectile"]=typeof(Projectile),["SunManager"]=typeof(SunManager),["Enemy"]=typeof(Fixture),["FTRuntime.Internal.SwfUtils"]=typeof(Fixture),["System.Int32"]=typeof(int),["System.UInt32"]=typeof(uint),["System.Single"]=typeof(float),["System.Boolean"]=typeof(bool),["System.Void"]=typeof(void),["System.Object"]=typeof(object),["System.IntPtr"]=typeof(IntPtr),["System.NullReferenceException"]=typeof(NullReferenceException),["SenarioState"]=typeof(int),["Armor1Type"]=typeof(int),["Armor2Type"]=typeof(int)};
 static Type T(TypeReference t)=>t.IsByReference?T(((ByReferenceType)t).ElementType).MakeByRefType():Map.TryGetValue(t.FullName,out var type)?type:throw new Exception("Unmapped type "+t.FullName);
 delegate void U(uint p,uint q,ref float r,ref float g,ref float b,ref float a);
 static DynamicMethod Emit(MethodDefinition m,string mutation){
  var dm=new DynamicMethod(m.Name+mutation,T(m.ReturnType),(m.HasThis?new[]{T(m.DeclaringType)}:Array.Empty<Type>()).Concat(m.Parameters.Select(p=>T(p.ParameterType))).ToArray(),typeof(Fixture).Module,true){InitLocals=m.Body.InitLocals};
  var il=dm.GetILGenerator();var locals=m.Body.Variables.Select(v=>il.DeclareLocal(T(v.VariableType))).ToArray();var labels=m.Body.Instructions.ToDictionary(i=>i,i=>il.DefineLabel());
  var ops=typeof(RO).GetFields().Where(f=>f.FieldType==typeof(System.Reflection.Emit.OpCode)).Select(f=>(System.Reflection.Emit.OpCode)f.GetValue(null)!).ToDictionary(o=>o.Value);
  if(mutation=="illegal_stack")il.Emit(RO.Pop);
  bool changed=mutation=="illegal_stack";
  foreach(var i in m.Body.Instructions){
   il.MarkLabel(labels[i]);var op=ops[i.OpCode.Value];var a=i.Operand;
   if(mutation=="wrong_return"&&op==RO.Ret&&m.ReturnType.FullName!="System.Void"){
    il.Emit(RO.Pop);if(T(m.ReturnType)==typeof(float))il.Emit(RO.Ldc_R4,123f);else il.Emit(RO.Ldc_I4,0);changed=true;
   }
   if(mutation=="unsigned_channel"&&op==RO.Conv_I2){op=RO.Conv_U2;changed=true;}
   if(mutation=="divide_unsigned"&&op==RO.Div){op=RO.Div_Un;changed=true;}
   if(mutation=="reverse_remainder"&&op==RO.Rem){op=RO.Rem_Un;changed=true;}
   if(mutation=="wrong_threshold"&&op==RO.Ldc_I4_4){op=RO.Ldc_I4_5;changed=true;}
   if(mutation=="inverted_weight_branch"&&op==RO.Bgt_S){op=RO.Ble_Un_S;changed=true;}
   if(a is CI target){il.Emit(op,labels[target]);continue;}
   if(a is CI[] ts){il.Emit(op,ts.Select(t=>labels[t]).ToArray());continue;}
   if(a is Mono.Cecil.Cil.VariableDefinition v){il.Emit(op,locals[v.Index]);continue;}
   if(a is ParameterDefinition p){il.Emit(op,(short)(p.Index+(m.HasThis?1:0)));continue;}
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
 static List<string> Cases(MethodDefinition m,DynamicMethod dm,List<long[]> rows,bool stop=false){
  var failures=new List<string>();int tok=m.MetadataToken.ToInt32();
  if(tok==0x060008F3){
   var fn=(U)dm.CreateDelegate(typeof(U));int k=0;
   foreach(var row in rows){float r=11,g=12,b=13,a=14;fn((uint)row[0],(uint)row[1],ref r,ref g,ref b,ref a);if(!Same(r,row[2])||!Same(g,row[3])||!Same(b,row[4])||!Same(a,row[5])){failures.Add("unpack:"+k);if(stop)break;}k++;}
   return failures;
  }
  int ix=0;
  foreach(var r in rows){object?[] args;
   if(tok==0x060003DF)args=new object?[]{null,F(r[0]),F(r[1])};
   else if(tok==0x0600048A)args=new object?[]{null,new Plant{ID=(int)r[0],state=(int)r[1]}};
   else if(tok==0x06000375)args=new object?[]{new Plant{ID=(int)r[0],state=99}};
   else args=new object?[]{(int)r[0]};
   object? result=dm.Invoke(null,args);bool match=result is float f?Same(f,r[^1]):result is bool b?(b?1:0)==r[^1]:Convert.ToInt64(result)==r[^1];
   if(!match){failures.Add("row:"+ix+" expected:"+r[^1]+" actual:"+(result is float v?B(v):result));if(stop)break;}ix++;
  }
  if(tok==0x0600048A||tok==0x06000375){
   try{dm.Invoke(null,tok==0x0600048A?new object?[]{null,null}:new object?[]{null});failures.Add("null-receiver-not-rejected");}
   catch(TargetInvocationException e) when(e.InnerException is NullReferenceException){}
  }
  return failures;
 }
 static List<string> AliasCases(DynamicMethod dm,string path){
  var fn=(U)dm.CreateDelegate(typeof(U));int[][] alias={new[]{0,0,0,0},new[]{0,0,1,1},new[]{0,1,0,1},new[]{0,1,1,0},new[]{0,0,0,1}};var bad=new List<string>();int ix=0;
  foreach(var line in File.ReadLines(path)){var r=line.Split(' ').Select(long.Parse).ToArray();var map=alias[(int)r[0]];float[] v={11,12,13,14};fn((uint)r[1],(uint)r[2],ref v[map[0]],ref v[map[1]],ref v[map[2]],ref v[map[3]]);if(Enumerable.Range(0,4).Any(i=>B(v[i])!=(uint)r[i+3]))bad.Add("alias:"+ix);ix++;}
  if(ix!=35)throw new Exception("Alias oracle must contain 35 cases");return bad;
 }
 static int Main(string[] args){try{
  using var a=AssemblyDefinition.ReadAssembly(args[0]);var all=File.ReadLines(args[1]).Select(l=>l.Split(' ')).GroupBy(l=>l[0]).ToDictionary(g=>g.Key,g=>g.Select(l=>l.Skip(1).Select(long.Parse).ToArray()).ToList());
  if(all.Count!=12)throw new Exception("Expected twelve original-native method oracles");
  var expected=new Dictionary<int,string>{[0x060001A3]="System.Int32 Enemy::GetFlagIndex(System.Int32)",[0x060001A4]="System.Int32 Enemy::GetWaveIndex(System.Int32)",[0x06000375]="System.Int32 Plant::GetNormalProjectileID()",[0x060003DF]="System.Single Projectile::CalculateWeight(System.Single,System.Single)",[0x0600026C]="System.Single SunManager::GetMaxSunFallTime(SenarioState)",[0x060004B6]="System.Single Zombie::ZA_GetArmor1Hp(Armor1Type)",[0x060004B7]="System.Single Zombie::ZA_GetArmor1Toughness(Armor1Type)",[0x060004B8]="System.Single Zombie::ZA_GetArmor2Def(Armor2Type)",[0x060004B9]="System.Single Zombie::ZA_GetArmor2Hp(Armor2Type)",[0x060004BA]="System.Single Zombie::ZA_GetArmor2Toughness(Armor2Type)",[0x0600048A]="System.Boolean Zombie::CanAttack_Plant(Plant)",[0x060008F3]="System.Void FTRuntime.Internal.SwfUtils::UnpackFColorFromUInts(System.UInt32,System.UInt32,System.Single&,System.Single&,System.Single&,System.Single&)"};
  var positives=new List<object>();var controls=new List<object>();int bad=0,total=0,errors=0;bool detected=true;
  foreach(var (key,rows) in all){int token=Convert.ToInt32(key,16);var m=(MethodDefinition)a.MainModule.LookupToken(token);if(m.FullName!=expected[token])throw new Exception("Signature pin failed "+key);
   var failures=Cases(m,Emit(m,"none"),rows);bad+=failures.Count;int count=rows.Count+(token is 0x06000375 or 0x0600048A?1:0);total+=count;positives.Add(new{token=key,signature=m.FullName,cases=count,failures});
   foreach(string mut in new[]{token==0x060008F3?"unsigned_channel":"wrong_return","illegal_stack"}.Concat(token==0x060001A3?new[]{"divide_unsigned"}:token==0x060001A4?new[]{"reverse_remainder"}:token==0x0600048A?new[]{"wrong_threshold"}:token==0x060003DF?new[]{"inverted_weight_branch"}:Array.Empty<string>())){
    bool reject=false;string? error=null;try{reject=Cases(m,Emit(m,mut),rows,true).Count>0;}catch(TargetInvocationException e)when(e.InnerException is InvalidProgramException){reject=true;}catch(InvalidProgramException){reject=true;}catch(Exception e){error=e.ToString();errors++;}
    detected&=reject&&error==null;controls.Add(new{token=key,mutation=mut,detected=reject,tool_error=error});
   }
  }
  var aliasMethod=(MethodDefinition)a.MainModule.LookupToken(0x060008F3);var aliasPath=Path.Combine(Path.GetDirectoryName(args[1])!,"NATIVE-ALIAS.tsv");var aliasFailures=AliasCases(Emit(aliasMethod,"none"),aliasPath);bad+=aliasFailures.Count;total+=35;positives.Add(new{token="0x060008F3",suite="original-native-byref-alias",cases=35,failures=aliasFailures});
  bool pass=bad==0&&errors==0&&detected;var report=new{candidate_sha256=Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(args[0]))).ToLowerInvariant(),oracle_sha256=Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(args[1]))).ToLowerInvariant(),methods=all.Count,positive_cases=total,positive_failures=bad,negative_controls=controls.Count,tool_errors=errors,gate_pass=pass,scope="Real CLR candidate CIL against executed original PC leaf machine code; no Unity helpers. Non-NaN float bits exact; NaN classification compared without payload promise. Finite input sampling except exhaustive 16-bit channel coverage; not Unity/iOS/device acceptance",positives,controls};
  File.WriteAllText(args[2],JsonSerializer.Serialize(report,new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine(JsonSerializer.Serialize(new{report.methods,report.positive_cases,report.positive_failures,report.negative_controls,report.tool_errors,report.gate_pass}));return pass?0:2;
 }catch(Exception e){Console.Error.WriteLine(e);return 3;}}
}
