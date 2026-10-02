using Mono.Cecil;
using CI=Mono.Cecil.Cil.Instruction;
using RO=System.Reflection.Emit.OpCodes;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.Json;
using System.Security.Cryptography;
public class Zombie {}
public class EnemySelecter {public int enemyPoint_Base;}
public class Map {public float cameraSize;}
public class Board {public Map? map;}
public class Projectile {public Board? board;public float fX,fY;}
public struct SwfSettingsData {public int MaxAtlasSize,AtlasPadding;public float PixelsPerUnit;public bool BitmapTrimming,GenerateMipMaps,AtlasPowerOfTwo,AtlasForceSquare;public int AtlasTextureFilter,AtlasTextureFormat;}
public static class Mathf {public static float Epsilon;}
static class Fixture {
 static Dictionary<string,Type> Map=new(){["Zombie"]=typeof(Zombie),["Projectile"]=typeof(Projectile),["EnemySelecter"]=typeof(EnemySelecter),["Board"]=typeof(Board),["Map"]=typeof(Map),["FTRuntime.SwfSettingsData"]=typeof(SwfSettingsData),["FTRuntime.SwfSettingsData/AtlasFilter"]=typeof(int),["FTRuntime.SwfSettingsData/AtlasFormat"]=typeof(int),["UnityEngine.Mathf"]=typeof(Mathf),["System.Int32"]=typeof(int),["System.UInt32"]=typeof(uint),["System.Single"]=typeof(float),["System.Boolean"]=typeof(bool),["System.Void"]=typeof(void),["System.Object"]=typeof(object),["System.IntPtr"]=typeof(IntPtr),["System.NullReferenceException"]=typeof(NullReferenceException),["Armor1Type"]=typeof(int)};
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
   if(mutation=="unsigned_div"&&op==RO.Div){op=RO.Div_Un;changed=true;}
   if(mutation=="inclusive_rect"&&op==RO.Cgt){op=RO.Clt_Un;changed=true;}
   if(mutation=="wrong_scale"&&op==RO.Ldc_R4&&Equals(a,540f)){a=541f;changed=true;}
   if(mutation=="wrong_epsilon_scale"&&op==RO.Ldc_R4&&Equals(a,8f)){a=0f;changed=true;}
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
  var failures=new List<string>();int tok=m.MetadataToken.ToInt32(),ix=0;
  foreach(var r in rows){object?[] args;
   if(tok==0x060004B5)args=new object?[]{(int)r[0]};
   else if(tok==0x060001AB)args=new object?[]{new EnemySelecter{enemyPoint_Base=(int)r[2]},(int)r[0],(int)r[1]};
   else if(tok==0x060003FC)args=new object?[]{new Projectile{fX=F(r[0]),fY=F(r[1]),board=new Board{map=new Map{cameraSize=F(r[2])}}}};
   else if(tok==0x06000470)args=new object?[]{null,F(r[0]),F(r[1]),F(r[2]),F(r[3])};
   else if(tok==0x060008A2){Mathf.Epsilon=F(r[0]);var a=new SwfSettingsData{MaxAtlasSize=1024,AtlasPadding=2,PixelsPerUnit=F(r[1])};var b=a;b.PixelsPerUnit=F(r[2]);int v=(int)r[4];switch((int)r[3]){case 0:b.MaxAtlasSize=v;break;case 1:b.AtlasPadding=v;break;case 2:b.BitmapTrimming=v!=0;break;case 3:b.GenerateMipMaps=v!=0;break;case 4:b.AtlasPowerOfTwo=v!=0;break;case 5:b.AtlasForceSquare=v!=0;break;case 6:b.AtlasTextureFilter=v;break;case 7:b.AtlasTextureFormat=v;break;}args=new object?[]{a,b};}
   else throw new Exception("Unexpected token");
   var result=dm.Invoke(null,args);bool match=result is float f?Same(f,r[^1]):result is bool z?(z?1:0)==r[^1]:Convert.ToInt64(result)==r[^1];
   if(!match){failures.Add("row:"+ix+" expected:"+r[^1]+" actual:"+result);if(stop)break;}ix++;
  }
  if(tok==0x060001AB||tok==0x060003FC){
   var cases=tok==0x060001AB?new[]{new object?[]{null,0,0}}:new[]{new object?[]{null},new object?[]{new Projectile()},new object?[]{new Projectile{board=new Board()}}};
   foreach(var a in cases){try{dm.Invoke(null,a);failures.Add("null-chain-not-rejected");}catch(TargetInvocationException e)when(e.InnerException is NullReferenceException){}}
  }
  return failures;
 }
 static int Main(string[] args){try{
  using var a=AssemblyDefinition.ReadAssembly(args[0]);var all=File.ReadLines(args[1]).Select(l=>l.Split(' ')).GroupBy(l=>l[0]).ToDictionary(g=>g.Key,g=>g.Select(l=>l.Skip(1).Select(long.Parse).ToArray()).ToList());
  if(all.Count!=5)throw new Exception("Expected five original-native method oracles");
  var expected=new Dictionary<int,string>{[0x060004B5]="System.Single Zombie::ZA_GetArmor1Def(Armor1Type)",[0x060001AB]="System.Int32 EnemySelecter::GetWavePoint(System.Int32,System.Int32)",[0x060003FC]="System.Boolean Projectile::TestOutofMap()",[0x06000470]="System.Boolean Zombie::RectXTest(System.Single,System.Single,System.Single,System.Single)",[0x060008A2]="System.Boolean FTRuntime.SwfSettingsData::CheckEquals(FTRuntime.SwfSettingsData)"};
  var positives=new List<object>();var controls=new List<object>();int bad=0,total=0,errors=0;bool detected=true;
  foreach(var (key,rows) in all){int token=Convert.ToInt32(key,16);var m=(MethodDefinition)a.MainModule.LookupToken(token);if(m.FullName!=expected[token])throw new Exception("Signature pin failed "+key);
   var failures=Cases(m,Emit(m,"none"),rows);bad+=failures.Count;int count=rows.Count+(token==0x060001AB?1:token==0x060003FC?3:0);total+=count;positives.Add(new{token=key,signature=m.FullName,cases=count,failures});
   foreach(string mut in (args.Length>3?Array.Empty<string>():new[]{"wrong_return","illegal_stack"}.Concat(token==0x060001AB?new[]{"unsigned_div"}:token==0x060003FC?new[]{"wrong_scale"}:token==0x06000470?new[]{"inclusive_rect"}:token==0x060008A2?new[]{"wrong_epsilon_scale"}:Array.Empty<string>()))){
    bool reject=false;string? error=null;try{reject=Cases(m,Emit(m,mut),rows,true).Count>0;}catch(TargetInvocationException e)when(e.InnerException is InvalidProgramException){reject=true;}catch(InvalidProgramException){reject=true;}catch(Exception e){error=e.ToString();errors++;}
    detected&=reject&&error==null;controls.Add(new{token=key,mutation=mut,detected=reject,tool_error=error});
   }
  }
  bool pass=bad==0&&errors==0&&detected;var report=new{candidate_sha256=Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(args[0]))).ToLowerInvariant(),oracle_sha256=Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(args[1]))).ToLowerInvariant(),methods=all.Count,positive_cases=total,positive_failures=bad,negative_controls=controls.Count,tool_errors=errors,gate_pass=pass,scope="Real CLR candidate CIL versus original PC machine code under controlled object layouts; Rect and Settings explicitly initialized Mathf class shim, Settings tested with two Epsilon preconditions. Non-NaN float bits exact; NaN classification compared without payload promise. Finite input sampling except exhaustive 16-bit channel coverage; not Unity/iOS/device acceptance",positives,controls};
  File.WriteAllText(args[2],JsonSerializer.Serialize(report,new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine(JsonSerializer.Serialize(new{report.methods,report.positive_cases,report.positive_failures,report.negative_controls,report.tool_errors,report.gate_pass}));return pass?0:2;
 }catch(Exception e){Console.Error.WriteLine(e);return 3;}}
}
