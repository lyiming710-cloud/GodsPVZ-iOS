using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;

if (args.Length != 2) return 2;
static string Sha(string p)=>Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(p))).ToLowerInvariant();
static IEnumerable<TypeDefinition> All(IEnumerable<TypeDefinition> ts){foreach(var t in ts){yield return t;foreach(var n in All(t.NestedTypes))yield return n;}}
static string Scope(IMetadataScope? s)=>s switch{AssemblyNameReference a=>a.Name,ModuleDefinition m=>m.Assembly?.Name?.Name??m.Name,ModuleReference r=>r.Name,_=>s?.ToString()??"<null>"};
var path=Path.GetFullPath(args[0]);var expected=args[1].ToLowerInvariant();var actual=Sha(path);if(actual!=expected)throw new InvalidDataException($"sha={actual}");
Console.WriteLine($"INPUT_SHA256 {actual}");
using var module=ModuleDefinition.ReadModule(path,new ReaderParameters{InMemory=true,ReadingMode=ReadingMode.Immediate});
var types=All(module.Types).ToList();var methods=types.SelectMany(t=>t.Methods).ToList();var fields=types.SelectMany(t=>t.Fields).ToList();
MethodDefinition MD(uint t)=>methods.Single(m=>m.MetadataToken.ToUInt32()==t);
FieldDefinition FD(uint t)=>fields.Single(f=>f.MetadataToken.ToUInt32()==t);
void M(uint t,string label){var m=MD(t);Console.WriteLine($"METHOD_DEF label={label} token=0x{t:X8} rid={m.MetadataToken.RID} method={m.FullName} attrs={m.Attributes}");}
void F(uint t,string label){var f=FD(t);Console.WriteLine($"FIELD_DEF label={label} token=0x{t:X8} rid={f.MetadataToken.RID} field={f.FullName} attrs={f.Attributes}");}

F(0x04000002,"Administrator.mainSystem");F(0x04000003,"Administrator.systems");F(0x04000004,"Administrator.system0");F(0x04000005,"Administrator.system1");F(0x04000006,"Administrator.system2");F(0x04000007,"Administrator.system3");F(0x04000008,"Administrator.boardEdior");F(0x04000009,"Administrator.skipLevel");F(0x0400000A,"Administrator.mode");
F(0x040000C8,"SystemSkipLevel.playerSave");F(0x040000C9,"SystemSkipLevel.level");F(0x040000CA,"SystemSkipLevel.text_Level");F(0x040000CB,"SystemSkipLevel.challenge");
M(0x06000005,"Administrator.Start_Mode");M(0x06000007,"Administrator.Start_植物存档数据");M(0x06000008,"Administrator.Start_出怪挑选器数据");M(0x06000009,"Administrator.Start_关卡配置文件");M(0x0600000A,"Administrator.Start_材料基础数据");
M(0x06000073,"System0.LoadPlantData");M(0x06000081,"System1.LoadZombieInfo");M(0x06000088,"System2.Start2");M(0x0600009E,"System3.LoadSuppliesInfo");M(0x060000A4,"SystemSkipLevel.Start0");M(0x06000169,"BoardManager.get_Instance");M(0x06000636,"MainUIController.get_instance");M(0x06000647,"MainUIController.SetGloveButtons");

foreach(var tname in new[]{"GlobalStaticVars","GlobalStaticVars/LawnApp","SavesManager","Save","BoardManager"}){
  var t=types.Single(x=>x.FullName==tname);Console.WriteLine($"TYPE_DEF token=0x{t.MetadataToken.ToUInt32():X8} type={t.FullName}");
  foreach(var f in t.Fields) Console.WriteLine($"RELATED_FIELD token=0x{f.MetadataToken.ToUInt32():X8} field={f.FullName} attrs={f.Attributes}");
}

var refs=methods.Where(m=>m.HasBody).SelectMany(m=>m.Body.Instructions).Select(i=>i.Operand).OfType<MethodReference>().ToList();
var wanted=new[]{
 "UnityEngine.Component::get_transform()","UnityEngine.Transform::GetChild(System.Int32)","UnityEngine.Component::GetComponent<UnityEngine.Canvas>()","UnityEngine.Camera::get_main()","UnityEngine.Canvas::set_worldCamera(UnityEngine.Camera)",
 "UnityEngine.GameObject::SetActive(System.Boolean)","UnityEngine.Component::get_gameObject()","UnityEngine.Camera::set_orthographicSize(System.Single)","UnityEngine.Object::op_Implicit(UnityEngine.Object)","UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)","Board::GamePause(System.Boolean)",
 "System.Collections.Generic.List`1<UnityEngine.GameObject>::GetEnumerator()","System.Collections.Generic.List`1<UnityEngine.GameObject>::get_Item(System.Int32)"
};
foreach(var needle in wanted){var xs=refs.Where(r=>r.FullName.Contains(needle,StringComparison.Ordinal)).GroupBy(r=>r.FullName+"@"+Scope(r.DeclaringType.Scope)).Select(g=>g.First()).ToList();Console.WriteLine($"METHOD_REF_MATCH needle={needle} count={xs.Count}");foreach(var r in xs)Console.WriteLine($"METHOD_REF method={r.FullName} scope={Scope(r.DeclaringType.Scope)} token=0x{r.MetadataToken.ToUInt32():X8}");}
var enumRefs=refs.Where(r=>r.DeclaringType.FullName.Contains("System.Collections.Generic.List`1/Enumerator",StringComparison.Ordinal) && r.FullName.Contains("UnityEngine.GameObject",StringComparison.Ordinal)).GroupBy(r=>r.FullName+"@"+Scope(r.DeclaringType.Scope)).Select(g=>g.First()).OrderBy(r=>r.FullName).ToList();
Console.WriteLine($"LIST_GAMEOBJECT_ENUMERATOR_REF_COUNT {enumRefs.Count}");foreach(var r in enumRefs)Console.WriteLine($"LIST_GAMEOBJECT_ENUMERATOR_REF method={r.FullName} scope={Scope(r.DeclaringType.Scope)} token=0x{r.MetadataToken.ToUInt32():X8}");
if(enumRefs.Count==0) throw new InvalidDataException("no List<GameObject>.Enumerator refs");
Console.WriteLine("READONLY_ADMINISTRATOR_START_EXACT_REF_PROBE_PASS mutation=0");
return 0;
