using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.Stage9LoadSkillLogoPatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "5a1b4e0554fa0163283a477efb606685588012397c39d877ea2179a4de07fcca";
const int ExpectedMethodDefCount = 2317;
const uint ExpectedToken = 0x0600021Eu;

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}
static uint Raw(IMetadataTokenProvider p) => p.MetadataToken.ToUInt32();
static string OperandSemantic(object? operand, MethodDefinition owner)
{
    if (operand is null) return "";
    if (operand is Instruction bi) return $"I#{owner.Body.Instructions.IndexOf(bi)}";
    if (operand is Instruction[] sw) return "SW[" + string.Join(',', sw.Select(x => owner.Body.Instructions.IndexOf(x))) + "]";
    if (operand is MethodReference mr) return $"M:{mr.FullName}";
    if (operand is FieldReference fr) return $"F:{fr.FullName}";
    if (operand is TypeReference tr) return $"T:{tr.FullName}";
    if (operand is VariableDefinition vr) return $"V:{vr.Index}:{vr.VariableType.FullName}";
    if (operand is ParameterDefinition pr) return $"P:{pr.Index}:{pr.ParameterType.FullName}";
    if (operand is string s) return $"S:{Convert.ToBase64String(Encoding.UTF8.GetBytes(s))}";
    return $"C:{Convert.ToString(operand, System.Globalization.CultureInfo.InvariantCulture)}";
}
static string MethodSemantic(MethodDefinition m)
{
    var sb = new StringBuilder();
    sb.Append(m.FullName).Append('|').Append(m.Attributes).Append('|').Append(m.ImplAttributes).Append('|');
    if (!m.HasBody) return sb.Append("NOBODY").ToString();
    sb.Append("init=").Append(m.Body.InitLocals).Append(";max=").Append(m.Body.MaxStackSize).Append(';');
    foreach (var v in m.Body.Variables) sb.Append("L:").Append(v.Index).Append(':').Append(v.VariableType.FullName).Append(';');
    foreach (var i in m.Body.Instructions) sb.Append("I:").Append(i.OpCode.Code).Append(':').Append(OperandSemantic(i.Operand, m)).Append(';');
    foreach (var h in m.Body.ExceptionHandlers)
    {
        int Idx(Instruction? x) => x is null ? -1 : m.Body.Instructions.IndexOf(x);
        sb.Append("EH:").Append(h.HandlerType).Append(':').Append(Idx(h.TryStart)).Append(':').Append(Idx(h.TryEnd)).Append(':').Append(Idx(h.HandlerStart)).Append(':').Append(Idx(h.HandlerEnd)).Append(':').Append(Idx(h.FilterStart)).Append(':').Append(h.CatchType?.FullName ?? "").Append(';');
    }
    return sb.ToString();
}

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(input))).ToLowerInvariant();
Console.WriteLine($"INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256) throw new InvalidDataException($"input SHA mismatch: {inputSha}");

using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var methods = AllTypes(module.Types).SelectMany(t => t.Methods).ToList();
if (methods.Count != ExpectedMethodDefCount) throw new InvalidDataException($"MethodDef count drifted: {methods.Count}");
var target = methods.SingleOrDefault(m => m.DeclaringType.FullName == "ResourceManager" && m.Name == "LoadSkillLogo" && m.Parameters.Count == 2 && m.Parameters[0].ParameterType.FullName == "System.Int32" && m.Parameters[1].ParameterType.FullName == "System.String")
    ?? throw new InvalidDataException("ResourceManager.LoadSkillLogo(int,string) target missing or ambiguous");
var token = Raw(target);
Console.WriteLine($"TARGET_METHOD token=0x{token:X8} name={target.FullName}");
if (token != ExpectedToken) throw new InvalidDataException($"target token drifted: 0x{token:X8}");
if (!target.Body.Instructions.Any(i => i.Operand is FieldReference f && f.DeclaringType.FullName.StartsWith("System.Collections.Generic.List`1") && (f.Name == "_size" || f.Name == "_version" || f.Name == "_items")))
    throw new InvalidDataException("expected corrupt private List<T> access missing");
if (!target.Body.Instructions.Any(i => i.Operand is MethodReference m && m.Name == "set_Item" && m.DeclaringType.FullName == "System.Collections.Generic.List`1<System.Object>"))
    throw new InvalidDataException("expected malformed List<object>.set_Item reference missing");

var resourceManager = target.DeclaringType;
var skillLogos = resourceManager.Fields.SingleOrDefault(f => f.Name == "skillLogos") ?? throw new InvalidDataException("ResourceManager.skillLogos missing");
if (skillLogos.FieldType is not GenericInstanceType listSprite || listSprite.GenericArguments.Count != 1 || listSprite.GenericArguments[0].FullName != "UnityEngine.Sprite")
    throw new InvalidDataException($"skillLogos type drifted: {skillLogos.FieldType.FullName}");
var listDecl = module.ImportReference(listSprite);
var gp = listSprite.ElementType.GenericParameters.SingleOrDefault(p => p.Type == GenericParameterType.Type && p.Position == 0)
    ?? throw new InvalidDataException("List<T> type generic parameter missing");
var gpRef = module.ImportReference(gp);

MethodReference ListCount() => new("get_Count", module.TypeSystem.Int32, listDecl) { HasThis = true, ExplicitThis = false, CallingConvention = MethodCallingConvention.Default };
MethodReference ListGetItem()
{
    var m = new MethodReference("get_Item", gpRef, listDecl) { HasThis = true, ExplicitThis = false, CallingConvention = MethodCallingConvention.Default };
    m.Parameters.Add(new ParameterDefinition(module.TypeSystem.Int32));
    return m;
}
MethodReference ListAdd()
{
    var m = new MethodReference("Add", module.TypeSystem.Void, listDecl) { HasThis = true, ExplicitThis = false, CallingConvention = MethodCallingConvention.Default };
    m.Parameters.Add(new ParameterDefinition(gpRef));
    return m;
}
MethodReference ListSetItem()
{
    var m = new MethodReference("set_Item", module.TypeSystem.Void, listDecl) { HasThis = true, ExplicitThis = false, CallingConvention = MethodCallingConvention.Default };
    m.Parameters.Add(new ParameterDefinition(module.TypeSystem.Int32));
    m.Parameters.Add(new ParameterDefinition(gpRef));
    return m;
}

var oldRefs = target.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>().ToList();
MethodReference OldRef(string fullName) => oldRefs.FirstOrDefault(m => m.FullName == fullName) ?? throw new InvalidDataException($"required original MethodRef missing: {fullName}");
var pathCombine4 = OldRef("System.String System.IO.Path::Combine(System.String,System.String,System.String,System.String)");
var objectInequality = OldRef("System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)");
var objectImplicit = OldRef("System.Boolean UnityEngine.Object::op_Implicit(UnityEngine.Object)");
var stringConcat3 = OldRef("System.String System.String::Concat(System.String,System.String,System.String)");
var debugLog = OldRef("System.Void UnityEngine.Debug::Log(System.Object)");
var loadSprite = oldRefs.OfType<GenericInstanceMethod>().SingleOrDefault(m => m.Name == "Load" && m.ElementMethod.DeclaringType.FullName == "InternalResourceLoader" && m.GenericArguments.Count == 1 && m.GenericArguments[0].FullName == "UnityEngine.Sprite" && m.Parameters.Count == 1 && m.Parameters[0].ParameterType.FullName == "System.String")
    ?? throw new InvalidDataException("InternalResourceLoader.Load<Sprite>(string) MethodRef missing");

var untouchedBefore = methods.Where(m => Raw(m) != token).ToDictionary(Raw, MethodSemantic);
var body = target.Body;
body.ExceptionHandlers.Clear();
body.Instructions.Clear();
body.Variables.Clear();
body.InitLocals = true;
body.MaxStackSize = 4;
var sprite = new VariableDefinition(module.ImportReference(listSprite.GenericArguments[0]));
var path = new VariableDefinition(module.TypeSystem.String);
body.Variables.Add(sprite);
body.Variables.Add(path);
var il = body.GetILProcessor();
var getCount = ListCount();
var getItem = ListGetItem();
var add = ListAdd();
var setItem = ListSetItem();

// PC native 0x1803339E0..0x180333DA0:
// if id is in range and cached sprite is non-null, return cached entry;
// otherwise load sprites/Skill/SkillLogo/<skillName>, log if missing;
// for id>=0 grow skillLogos with null via List<Sprite>.Add(!0) until Count>id, then set item.
var load = il.Create(OpCodes.Ldstr, "sprites");
var afterCache = il.Create(OpCodes.Nop);
var afterLog = il.Create(OpCodes.Nop);
var returnSprite = il.Create(OpCodes.Ldloc, sprite);
var growCheck = il.Create(OpCodes.Ldsfld, skillLogos);
var setCache = il.Create(OpCodes.Ldsfld, skillLogos);

// if (id >= 0 && skillLogos.Count > id)
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldc_I4_0));
il.Append(il.Create(OpCodes.Blt, load));
il.Append(il.Create(OpCodes.Ldsfld, skillLogos));
il.Append(il.Create(OpCodes.Callvirt, getCount));
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ble, load));
// sprite = skillLogos[id]; if (sprite != null) return skillLogos[id];
il.Append(il.Create(OpCodes.Ldsfld, skillLogos));
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Callvirt, getItem));
il.Append(il.Create(OpCodes.Stloc, sprite));
il.Append(il.Create(OpCodes.Ldloc, sprite));
il.Append(il.Create(OpCodes.Ldnull));
il.Append(il.Create(OpCodes.Call, objectInequality));
il.Append(il.Create(OpCodes.Brfalse, load));
il.Append(il.Create(OpCodes.Ldsfld, skillLogos));
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Callvirt, getItem));
il.Append(il.Create(OpCodes.Ret));

// sprite = InternalResourceLoader.Load<Sprite>(Path.Combine("sprites","Skill","SkillLogo",skillName));
il.Append(load);
il.Append(il.Create(OpCodes.Ldstr, "Skill"));
il.Append(il.Create(OpCodes.Ldstr, "SkillLogo"));
il.Append(il.Create(OpCodes.Ldarg_1));
il.Append(il.Create(OpCodes.Call, pathCombine4));
il.Append(il.Create(OpCodes.Stloc, path));
il.Append(il.Create(OpCodes.Ldloc, path));
il.Append(il.Create(OpCodes.Call, loadSprite));
il.Append(il.Create(OpCodes.Stloc, sprite));
// if (!sprite) Debug.Log("未找到技能" + skillName + "的图标");
il.Append(il.Create(OpCodes.Ldloc, sprite));
il.Append(il.Create(OpCodes.Call, objectImplicit));
il.Append(il.Create(OpCodes.Brtrue, afterLog));
il.Append(il.Create(OpCodes.Ldstr, "未找到技能"));
il.Append(il.Create(OpCodes.Ldarg_1));
il.Append(il.Create(OpCodes.Ldstr, "的图标"));
il.Append(il.Create(OpCodes.Call, stringConcat3));
il.Append(il.Create(OpCodes.Call, debugLog));
il.Append(afterLog);
// if (id < 0) return sprite;
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldc_I4_0));
il.Append(il.Create(OpCodes.Blt, returnSprite));
// while (skillLogos.Count <= id) skillLogos.Add(null);
il.Append(growCheck);
il.Append(il.Create(OpCodes.Callvirt, getCount));
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Bgt, setCache));
il.Append(il.Create(OpCodes.Ldsfld, skillLogos));
il.Append(il.Create(OpCodes.Ldnull));
il.Append(il.Create(OpCodes.Callvirt, add));
il.Append(il.Create(OpCodes.Br, growCheck));
// skillLogos[id] = sprite;
il.Append(setCache);
il.Append(il.Create(OpCodes.Ldarg_0));
il.Append(il.Create(OpCodes.Ldloc, sprite));
il.Append(il.Create(OpCodes.Callvirt, setItem));
il.Append(returnSprite);
il.Append(il.Create(OpCodes.Ret));

Console.WriteLine("NATIVE_AUTHORITY token=0x0600021E address=0x00000001803339E0 function_end=0x0000000180333DA1 gameassembly_sha256=9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d metadata_sha256=ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9");
Console.WriteLine("PATCH_LOADSKILLLOGO cache=List<Sprite>.Count/get_Item(!0); load=InternalResourceLoader.Load<Sprite>; grow=List<Sprite>.Add(!0); assign=List<Sprite>.set_Item(!0); private_list_fields=0 malformed_list_object_refs=0");
module.Write(output);
var outputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(output))).ToLowerInvariant();
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");

using var reopen = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var after = AllTypes(reopen.Types).SelectMany(t => t.Methods).ToList();
if (after.Count != ExpectedMethodDefCount) throw new InvalidDataException("reopen MethodDef count drift");
var rt = after.Single(m => Raw(m) == token);
if (rt.Body.Instructions.Any(i => i.Operand is FieldReference f && f.DeclaringType.FullName.StartsWith("System.Collections.Generic.List`1") && (f.Name == "_size" || f.Name == "_version" || f.Name == "_items"))) throw new InvalidDataException("private List<T> field access survived repair");
if (rt.Body.Instructions.Any(i => i.Operand is MethodReference m && m.DeclaringType.FullName == "System.Collections.Generic.List`1<System.Object>")) throw new InvalidDataException("malformed List<object> MethodRef survived repair");
var listCalls = rt.Body.Instructions.Where(i => i.Operand is MethodReference m && m.DeclaringType.FullName == "System.Collections.Generic.List`1<UnityEngine.Sprite>").Select(i => (MethodReference)i.Operand).ToList();
if (listCalls.Count(m => m.Name == "get_Count" && m.ReturnType.FullName == "System.Int32") != 2) throw new InvalidDataException("expected two List<Sprite>.Count calls");
var gets = listCalls.Where(m => m.Name == "get_Item").ToList();
if (gets.Count != 2 || gets.Any(m => m.ReturnType is not GenericParameter p || p.Type != GenericParameterType.Type || p.Position != 0)) throw new InvalidDataException("List<Sprite>.get_Item return is not !0");
var adds = listCalls.Where(m => m.Name == "Add").ToList();
if (adds.Count != 1 || adds[0].Parameters.Count != 1 || adds[0].Parameters[0].ParameterType is not GenericParameter ap || ap.Type != GenericParameterType.Type || ap.Position != 0) throw new InvalidDataException("List<Sprite>.Add parameter is not !0");
var sets = listCalls.Where(m => m.Name == "set_Item").ToList();
if (sets.Count != 1 || sets[0].Parameters.Count != 2 || sets[0].Parameters[1].ParameterType is not GenericParameter sp || sp.Type != GenericParameterType.Type || sp.Position != 0) throw new InvalidDataException("List<Sprite>.set_Item value parameter is not !0");
Console.WriteLine($"REOPEN_LOADSKILLLOGO_PASS locals={rt.Body.Variables.Count} count_calls=2 get_item_calls=2 get_item_return=!0 add_signature=!0 set_item_signature=!0 private_list_fields=0 malformed_list_object_refs=0 token=0x{token:X8}");

var untouchedAfter = after.Where(m => Raw(m) != token).ToDictionary(Raw, MethodSemantic);
if (untouchedAfter.Count != untouchedBefore.Count) throw new InvalidDataException("untouched method count drift");
var drift = untouchedBefore.Where(kv => !untouchedAfter.TryGetValue(kv.Key, out var sem) || sem != kv.Value).Select(kv => kv.Key).ToList();
if (drift.Count != 0) throw new InvalidDataException("semantic drift outside ResourceManager.LoadSkillLogo: " + string.Join(',', drift.Select(t => $"0x{t:X8}")));
Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={untouchedAfter.Count} changed_methods=1");
return 0;
