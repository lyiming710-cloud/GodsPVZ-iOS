using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: Stage9TextLinkProbe <input.dll> <expected-sha256>");
    return 2;
}

static string Sha(string p) => Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(p))).ToLowerInvariant();
static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}
static string Operand(object? o) => o switch
{
    null => "",
    Instruction i => $"IL_{i.Offset:X4}",
    Instruction[] a => string.Join(',', a.Select(i => $"IL_{i.Offset:X4}")),
    VariableDefinition v => $"V_{v.Index}",
    ParameterDefinition p => p.Name ?? $"arg{p.Index}",
    _ => o.ToString() ?? ""
};
static string Scope(IMetadataScope? s) => s switch
{
    AssemblyNameReference a => a.Name,
    ModuleDefinition m => m.Assembly?.Name?.Name ?? m.Name,
    ModuleReference mr => mr.Name,
    _ => s?.ToString() ?? "<null>"
};

var input = Path.GetFullPath(args[0]);
var expected = args[1].Trim().ToLowerInvariant();
var actual = Sha(input);
if (actual != expected) throw new InvalidDataException($"input SHA mismatch actual={actual} expected={expected}");
Console.WriteLine($"INPUT_SHA256 {actual}");
using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate });
var types = AllTypes(module.Types).ToList();
var methods = types.SelectMany(x => x.Methods).ToList();
var t = types.Single(x => x.FullName == "TextLink");
Console.WriteLine($"TYPE token=0x{t.MetadataToken.ToUInt32():X8} rid={t.MetadataToken.RID} name={t.FullName}");
foreach (var f in t.Fields)
    Console.WriteLine($"FIELD name={f.Name} token=0x{f.MetadataToken.ToUInt32():X8} rid={f.MetadataToken.RID} type={f.FieldType.FullName} static={f.IsStatic}");
var ctors = t.Methods.Where(m => m.IsConstructor && !m.IsStatic).ToList();
foreach (var m in ctors)
{
    Console.WriteLine($"CTOR token=0x{m.MetadataToken.ToUInt32():X8} rid={m.MetadataToken.RID} rva=0x{m.RVA:X} params={m.Parameters.Count} code_size={(m.HasBody?m.Body.CodeSize:0)} locals={(m.HasBody?m.Body.Variables.Count:0)} initlocals={(m.HasBody&&m.Body.InitLocals)}");
    if (!m.HasBody) continue;
    for (int i=0;i<m.Body.Instructions.Count;i++)
    {
        var ins=m.Body.Instructions[i];
        Console.WriteLine($"IL index={i} offset=0x{ins.Offset:X4} op={ins.OpCode.Code} operand={Operand(ins.Operand)}");
    }
}
var target = ctors.Single(m => m.Parameters.Count == 0);
if (target.MetadataToken.ToUInt32() != 0x060006E5) throw new InvalidDataException($"TextLink ctor token drift 0x{target.MetadataToken.ToUInt32():X8}");

var allRefs = methods.Where(m => m.HasBody)
    .SelectMany(m => m.Body.Instructions)
    .Select(i => i.Operand)
    .OfType<MethodReference>()
    .ToList();
var colorRefs = allRefs
    .Where(m => m.DeclaringType.FullName == "UnityEngine.Color")
    .GroupBy(m => m.FullName + "@" + Scope(m.DeclaringType.Scope), StringComparer.Ordinal)
    .Select(g => g.First()).OrderBy(m => m.FullName, StringComparer.Ordinal).ToList();
var whiteRefs = colorRefs
    .Where(m => m.Name == "get_white" && !m.HasThis && m.Parameters.Count == 0 && m.ReturnType.FullName == "UnityEngine.Color")
    .ToList();
var rgbaCtorRefs = colorRefs
    .Where(m => m.Name == ".ctor" && m.HasThis && m.Parameters.Count == 4 && m.Parameters.All(p => p.ParameterType.FullName == "System.Single"))
    .ToList();
var monoCtorRefs = allRefs
    .Where(m => m.DeclaringType.FullName == "UnityEngine.MonoBehaviour" && m.Name == ".ctor" && m.HasThis && m.Parameters.Count == 0)
    .GroupBy(m => m.FullName + "@" + Scope(m.DeclaringType.Scope), StringComparer.Ordinal)
    .Select(g => g.First()).ToList();
Console.WriteLine($"COLOR_METHOD_REF_COUNT {colorRefs.Count}");
foreach (var r in colorRefs) Console.WriteLine($"COLOR_METHOD_REF {r.FullName}@{Scope(r.DeclaringType.Scope)}");
Console.WriteLine($"COLOR_WHITE_REF_COUNT {whiteRefs.Count}");
foreach (var r in whiteRefs) Console.WriteLine($"COLOR_WHITE_REF {r.FullName}@{Scope(r.DeclaringType.Scope)}");
Console.WriteLine($"COLOR_RGBA_CTOR_REF_COUNT {rgbaCtorRefs.Count}");
foreach (var r in rgbaCtorRefs) Console.WriteLine($"COLOR_RGBA_CTOR_REF {r.FullName}@{Scope(r.DeclaringType.Scope)}");
Console.WriteLine($"MONOBEHAVIOUR_CTOR_REF_COUNT {monoCtorRefs.Count}");
foreach (var r in monoCtorRefs) Console.WriteLine($"MONOBEHAVIOUR_CTOR_REF {r.FullName}@{Scope(r.DeclaringType.Scope)}");
Console.WriteLine("READONLY_TEXTLINK_CTOR_PROBE_PASS mutation=0 token=0x060006E5");
return 0;
