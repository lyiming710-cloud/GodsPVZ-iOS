using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
class Gate {
 static HashSet<uint> allowed=new HashSet<uint>(new uint[]{0x060001a5,0x060001b2,0x060001e0,0x06000248,0x0600039f,0x06000267});
 static List<string> failures=new List<string>();
 static IEnumerable<TypeDefinition> All(IEnumerable<TypeDefinition> ts){foreach(var t in ts){yield return t;foreach(var n in All(t.NestedTypes))yield return n;}}
 static void Check(string n,int v,int expected){Console.WriteLine(n+"="+v+"/"+expected);if(v!=expected)failures.Add(n);}
 static string Key(object x,Dictionary<Instruction,int> ix){
  if(x==null)return "-";
  if(x is Instruction)return "B"+ix[(Instruction)x];
  if(x is Instruction[])return string.Join(",",((Instruction[])x).Select(i=>ix[i]));
  if(x is MethodReference)return ((MethodReference)x).FullName;
  if(x is FieldReference)return ((FieldReference)x).FullName;
  if(x is TypeReference)return ((TypeReference)x).FullName;
  if(x is VariableDefinition)return "V"+((VariableDefinition)x).Index;
  if(x is ParameterDefinition)return "A"+((ParameterDefinition)x).Index;
  return Convert.ToString(x,System.Globalization.CultureInfo.InvariantCulture);
 }
 static string Canon(MethodDefinition d){
  if(!d.HasBody)return "NO_BODY";
  var b=d.Body;var ix=b.Instructions.Select((i,n)=>new{i,n}).ToDictionary(x=>x.i,x=>x.n);
  Func<Instruction,int> id=i=>i==null?-1:ix[i];
  var s=new StringBuilder();s.Append(b.InitLocals).Append('|').Append(string.Join(",",b.Variables.Select(v=>v.VariableType.FullName)));
  foreach(var i in b.Instructions)s.Append('|').Append(i.OpCode.Name).Append(':').Append(Key(i.Operand,ix));
  foreach(var h in b.ExceptionHandlers)s.Append("|EH:").Append(h.HandlerType).Append(':').Append(id(h.TryStart)).Append(':').Append(id(h.TryEnd)).Append(':').Append(id(h.HandlerStart)).Append(':').Append(id(h.HandlerEnd)).Append(':').Append(id(h.FilterStart)).Append(':').Append(h.CatchType==null?"-":h.CatchType.FullName);
  return s.ToString();
 }
 static bool Orphan(TypeReference t){if(t==null)return false;var p=t as GenericParameter;if(p!=null)return p.Owner==null;var g=t as GenericInstanceType;if(g!=null)return Orphan(g.ElementType)||g.GenericArguments.Any(Orphan);var s=t as TypeSpecification;return s!=null&&Orphan(s.ElementType);}
 static bool Orphan(MethodReference r){return Orphan(r.DeclaringType)||Orphan(r.ReturnType)||r.Parameters.Any(p=>Orphan(p.ParameterType));}
 static string Sem(MethodReference r){var g=r.DeclaringType as GenericInstanceType;return r.Name+"|"+(g==null?r.DeclaringType.FullName:g.GenericArguments[0].FullName);}
 static string Id(MethodDefinition d){return d.MetadataToken+"|"+d.FullName+"|"+d.Attributes+"|"+d.ImplAttributes;}
 static Dictionary<string,int> Uses(MethodDefinition d){return d.Body.Instructions.Where(i=>i.Operand is MethodReference).GroupBy(i=>((MethodReference)i.Operand).FullName).ToDictionary(g=>g.Key,g=>g.Count());}
 static int N(Dictionary<string,int> d,string k){return d.ContainsKey(k)?d[k]:0;}
 public static int Main(string[] a){
  var resolver=new DefaultAssemblyResolver();resolver.AddSearchDirectory(a[2]);
  using(var before=AssemblyDefinition.ReadAssembly(a[0],new ReaderParameters{AssemblyResolver=resolver}))
  using(var after=AssemblyDefinition.ReadAssembly(a[1],new ReaderParameters{AssemblyResolver=resolver}))
  using(var historical=AssemblyDefinition.ReadAssembly(a[3],new ReaderParameters{AssemblyResolver=resolver})){
   var bm=before.MainModule;var cm=after.MainModule;var bt=All(bm.Types).ToArray();var ct=All(cm.Types).ToArray();var bms=bt.SelectMany(t=>t.Methods).ToArray();var cms=ct.SelectMany(t=>t.Methods).ToArray();
   Check("METHODDEF",cms.Length,2317);Check("FIELDDEF",ct.Sum(t=>t.Fields.Count),2802);Check("TYPEDEF",ct.Length,bt.Length);if(cm.Kind!=ModuleKind.Dll)failures.Add("ModuleKind");
   Check("METHOD_IDENTITY_DRIFT",bms.Zip(cms,(b,c)=>Id(b)==Id(c)?0:1).Sum(),0);
   Check("FIELD_IDENTITY_DRIFT",bt.SelectMany(t=>t.Fields).Zip(ct.SelectMany(t=>t.Fields),(b,c)=>b.MetadataToken==c.MetadataToken&&b.FullName==c.FullName&&b.Attributes==c.Attributes?0:1).Sum(),0);
   Check("TYPE_IDENTITY_DRIFT",bt.Zip(ct,(b,c)=>b.MetadataToken==c.MetadataToken&&b.FullName==c.FullName&&b.Attributes==c.Attributes?0:1).Sum(),0);
   var changes=bms.Zip(cms,(b,c)=>new{b,c}).Where(x=>Canon(x.b)!=Canon(x.c)).ToArray();
   foreach(var x in changes)Console.WriteLine("CHANGED "+x.b.MetadataToken+" "+x.b.FullName);
   Check("CHANGED_METHODS",changes.Length,6);Check("NON_TARGET_METHOD_SEMANTIC_DIFFS",changes.Count(x=>!allowed.Contains(x.b.MetadataToken.ToUInt32())),0);
   using(var prior=AssemblyDefinition.ReadAssembly(a[5])){
    foreach(uint token in new uint[]{0x060001a5,0x060001b2,0x060001e0,0x06000248})Check("PRESERVED_NATIVE3_"+token.ToString("X8"),Canon((MethodDefinition)prior.MainModule.LookupToken((int)token))==Canon((MethodDefinition)cm.LookupToken((int)token))?1:0,1);
   }
   foreach(uint token in new uint[]{0x060001de,0x0600017f})Check("PRESERVED_"+token.ToString("X8"),Canon((MethodDefinition)bm.LookupToken((int)token))==Canon((MethodDefinition)cm.LookupToken((int)token))?1:0,1);
   int orphan=0;foreach(var d in cms){if(Orphan((MethodReference)d))orphan++;if(d.HasBody)orphan+=d.Body.Variables.Count(v=>Orphan(v.VariableType));}orphan+=cm.GetMemberReferences().OfType<MethodReference>().Count(Orphan);Check("ORPHAN_GENERIC_HITS",orphan,0);
   uint[] tokens={0x0A0000B4,0x0A0000B5,0x0A0000C3,0x0A0000C9,0x0A0000CA,0x0A0000D0,0x0A0000D1,0x0A0000D6,0x0A0000D7,0x0A0000E4,0x0A0000E5,0x0A0000E7,0x0A0000FB,0x0A0000FC,0x0A000140,0x0A000141,0x0A000142,0x0A000149,0x0A000162,0x0A000163,0x0A00021D,0x0A00021E,0x0A00022B,0x0A000284};
   var keys=tokens.Select(t=>Sem((MethodReference)historical.MainModule.LookupToken((int)t))).ToArray();var ok=new HashSet<string>();var refs=cm.GetMemberReferences().OfType<MethodReference>().ToArray();
   foreach(string key in keys.Distinct()){
    var found=refs.Where(r=>Sem(r)==key).ToArray();bool valid=found.Length>0;
    foreach(var r in found){try{var def=r.Resolve();if(def==null||Orphan(r))valid=false;}catch(Exception ex){valid=false;Console.WriteLine("RESOLVE_FAIL "+r.FullName+" "+ex.Message);}}
    if(valid)ok.Add(key);Console.WriteLine("SEMANTIC_KEY "+key+" rows="+found.Length+" ok="+valid);
   }
   Check("SEMANTIC_TARGET_KEYS",keys.Distinct().Count(),23);Check("SEMANTIC_TARGETS_ACCOUNTED",keys.Count(k=>ok.Contains(k)),24);Check("SEMANTIC_RESOLVE_KEYS",ok.Count,23);
   int transform=0;foreach(var pair in new[]{new[]{"EnemyManager","PlayBoardAudio"},new[]{"FlagMeter","Update"}}){var d=cms.Single(x=>x.DeclaringType.Name==pair[0]&&x.Name==pair[1]);transform+=d.Body.Instructions.Count(i=>i.OpCode==OpCodes.Callvirt&&i.Operand is MethodReference&&((MethodReference)i.Operand).Name=="get_transform");}Check("TRANSFORM_REPAIR_SITES",transform,3);
   int unresolved=0;var deltas=new List<string>();
   foreach(var x in changes){
    var bu=Uses(x.b);var cu=Uses(x.c);
    foreach(var k in bu.Keys.Union(cu.Keys).OrderBy(k=>k)){int delta=N(cu,k)-N(bu,k);if(delta!=0)deltas.Add(x.b.MetadataToken+"\t"+delta+"\t"+k);}
    foreach(var r in x.c.Body.Instructions.Select(i=>i.Operand).OfType<MethodReference>()){
     try{if(r is MethodDefinition)continue;if(r.Resolve()==null)throw new Exception("null");}catch(Exception ex){unresolved++;Console.WriteLine("TARGET_METHODREF_FAIL "+r.FullName+" "+ex.Message);}
    }
    foreach(var f in x.c.Body.Instructions.Select(i=>i.Operand).OfType<FieldReference>()){
     try{if(f is FieldDefinition)continue;if(f.Resolve()==null)throw new Exception("null");}catch(Exception ex){unresolved++;Console.WriteLine("TARGET_FIELDREF_FAIL "+f.FullName+" "+ex.Message);}
    }
   }
   File.WriteAllLines(a[4],deltas);Check("TARGET_UNRESOLVED_REFS",unresolved,0);
  }
  foreach(var f in failures)Console.WriteLine("FAIL "+f);Console.WriteLine(failures.Count==0?"NATIVE4_LOCAL_SEMANTIC_GATE_PASS":"NATIVE4_LOCAL_SEMANTIC_GATE_FAIL");return failures.Count==0?0:1;
 }
}
