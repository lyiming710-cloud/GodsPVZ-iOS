using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.Stage9SkillCopyPatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "fdf417e8f5fbc33fa14e012e5db6a4887fa0d3307104c5117afec234ca1e67fb";
const int ExpectedMethodDefCount = 2317;

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
    return sb.ToString();
}
static Instruction At(MethodDefinition m, int offset)
    => m.Body.Instructions.SingleOrDefault(i => i.Offset == offset)
       ?? throw new InvalidDataException($"{m.FullName}: missing IL_{offset:X4}");

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(input))).ToLowerInvariant();
Console.WriteLine($"INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256) throw new InvalidDataException($"input SHA mismatch: {inputSha}");

using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var methods = AllTypes(module.Types).SelectMany(t => t.Methods).ToList();
if (methods.Count != ExpectedMethodDefCount) throw new InvalidDataException($"MethodDef count drifted: {methods.Count}");
var target = methods.SingleOrDefault(m => m.FullName == "Skill Skill::Copy()")
    ?? throw new InvalidDataException("Skill.Copy target missing or ambiguous");
var targetToken = Raw(target);
Console.WriteLine($"TARGET_METHOD token=0x{targetToken:X8} name={target.FullName}");
if (target.Body.Variables.Count <= 10 || target.Body.Variables[5].VariableType.FullName != "System.Object" || target.Body.Variables[6].VariableType.FullName != "System.Object" || target.Body.Variables[10].VariableType.FullName != "System.IndexOutOfRangeException")
    throw new InvalidDataException("Skill.Copy local layout drift");

var changed = new HashSet<uint> { targetToken };
var untouchedBefore = methods.Where(m => !changed.Contains(Raw(m))).ToDictionary(Raw, MethodSemantic);

var init = At(target, 0x012C);
var store5 = At(target, 0x0131);
if (init.OpCode.Code != Code.Ldc_I4 || init.Operand is not int initVal || initVal != 0 || store5.OpCode.Code != Code.Stloc || store5.Operand is not VariableDefinition sv5 || sv5.Index != 5)
    throw new InvalidDataException("Skill.Copy index initialization drift");
var load5 = At(target, 0x018E);
var one = At(target, 0x0192);
var add = At(target, 0x0197);
var store6 = At(target, 0x0198);
var load6 = At(target, 0x01B6);
var store5Next = At(target, 0x01C9);
if (load5.OpCode.Code != Code.Ldloc || load5.Operand is not VariableDefinition lv5 || lv5.Index != 5 ||
    one.OpCode.Code != Code.Ldc_I4 || one.Operand is not int oneVal || oneVal != 1 || add.OpCode.Code != Code.Add ||
    store6.OpCode.Code != Code.Stloc || store6.Operand is not VariableDefinition sv6 || sv6.Index != 6 ||
    load6.OpCode.Code != Code.Ldloc || load6.Operand is not VariableDefinition lv6 || lv6.Index != 6 ||
    store5Next.OpCode.Code != Code.Stloc || store5Next.Operand is not VariableDefinition sv5n || sv5n.Index != 5)
    throw new InvalidDataException("Skill.Copy index arithmetic drift");
target.Body.Variables[5].VariableType = module.TypeSystem.Int32;
target.Body.Variables[6].VariableType = module.TypeSystem.Int32;
Console.WriteLine("PATCH_SKILL_COPY_INDEX locals=V5,V6 type=System.Int32");

var fallbackNew = At(target, 0x0247);
var fallbackStore = At(target, 0x024C);
var fallbackLoad = At(target, 0x0250);
var fallbackRet = At(target, 0x0254);
if (fallbackNew.OpCode.Code != Code.Newobj || fallbackNew.Operand is not MethodReference ctor || ctor.DeclaringType.FullName != "System.IndexOutOfRangeException" ||
    fallbackStore.OpCode.Code != Code.Stloc || fallbackStore.Operand is not VariableDefinition fsv || fsv.Index != 10 ||
    fallbackLoad.OpCode.Code != Code.Ldloc || fallbackLoad.Operand is not VariableDefinition flv || flv.Index != 10 || fallbackRet.OpCode.Code != Code.Ret)
    throw new InvalidDataException("Skill.Copy bounds-failure sequence drift");
fallbackRet.OpCode = OpCodes.Throw;
fallbackRet.Operand = null;
Console.WriteLine("PATCH_SKILL_COPY_BOUNDS action=throw exception=System.IndexOutOfRangeException");

module.Write(output);
var outputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(output))).ToLowerInvariant();
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");

using var reopen = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var after = AllTypes(reopen.Types).SelectMany(t => t.Methods).ToList();
if (after.Count != ExpectedMethodDefCount) throw new InvalidDataException("reopen MethodDef count drift");
var rt = after.SingleOrDefault(m => Raw(m) == targetToken) ?? throw new InvalidDataException("reopen target missing");
if (rt.Body.Variables[5].VariableType.FullName != "System.Int32" || rt.Body.Variables[6].VariableType.FullName != "System.Int32")
    throw new InvalidDataException("reopen Skill.Copy index repair missing");
var rtThrow = rt.Body.Instructions.SingleOrDefault(i => i.Offset == 0x0254);
if (rtThrow == null || rtThrow.OpCode.Code != Code.Throw) throw new InvalidDataException("reopen Skill.Copy bounds throw missing");
Console.WriteLine($"REOPEN_SKILL_COPY_PASS index_types=System.Int32 bounds=throw token=0x{targetToken:X8}");

var untouchedAfter = after.Where(m => !changed.Contains(Raw(m))).ToDictionary(Raw, MethodSemantic);
if (untouchedAfter.Count != untouchedBefore.Count) throw new InvalidDataException("untouched method count drift");
var drift = untouchedBefore.Where(kv => !untouchedAfter.TryGetValue(kv.Key, out var sem) || sem != kv.Value).Select(kv => kv.Key).ToList();
if (drift.Count != 0) throw new InvalidDataException("semantic drift outside Skill.Copy: " + string.Join(',', drift.Select(t => $"0x{t:X8}")));
Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={untouchedAfter.Count} changed_methods=1");
return 0;
