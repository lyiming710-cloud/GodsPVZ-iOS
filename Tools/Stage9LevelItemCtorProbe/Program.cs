using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: Stage9LevelItemCtorProbe <input.dll> <expected-sha256>");
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
static string Scope(IMetadataScope? s) => s switch
{
    AssemblyNameReference a => a.Name,
    ModuleDefinition m => m.Assembly?.Name?.Name ?? m.Name,
    ModuleReference mr => mr.Name,
    _ => s?.ToString() ?? "<null>"
};
static string Operand(object? o) => o switch
{
    null => "",
    Instruction i => $"IL_{i.Offset:X4}",
    Instruction[] a => string.Join(',', a.Select(i => $"IL_{i.Offset:X4}")),
    VariableDefinition v => $"V_{v.Index}:{v.VariableType.FullName}",
    ParameterDefinition p => p.Name ?? $"arg{p.Index}",
    MethodReference m => $"{m.FullName}@{Scope(m.DeclaringType.Scope)}",
    FieldReference f => $"{f.FullName}@{Scope(f.DeclaringType.Scope)}",
    TypeReference t => $"{t.FullName}@{Scope(t.Scope)}",
    _ => o.ToString() ?? ""
};

var input = Path.GetFullPath(args[0]);
var expected = args[1].Trim().ToLowerInvariant();
var actual = Sha(input);
if (actual != expected) throw new InvalidDataException($"input SHA mismatch actual={actual} expected={expected}");
Console.WriteLine($"INPUT_SHA256 {actual}");

using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var types = AllTypes(module.Types).ToList();
var methods = types.SelectMany(t => t.Methods).ToList();
var fields = types.SelectMany(t => t.Fields).ToList();
Console.WriteLine($"MODULE methods={methods.Count} fields={fields.Count} assembly_refs={module.AssemblyReferences.Count}");
foreach (var a in module.AssemblyReferences.OrderBy(a => a.Name, StringComparer.Ordinal))
    Console.WriteLine($"ASSEMBLY_REF {a.FullName}");

var t = types.Single(x => x.FullName == "LevelItem");
Console.WriteLine($"TYPE token=0x{t.MetadataToken.ToUInt32():X8} rid={t.MetadataToken.RID} name={t.FullName}");
foreach (var f in t.Fields)
    Console.WriteLine($"FIELD name={f.Name} token=0x{f.MetadataToken.ToUInt32():X8} rid={f.MetadataToken.RID} type={f.FieldType.FullName} static={f.IsStatic}");

var ctors = t.Methods.Where(m => m.IsConstructor && !m.IsStatic && m.Parameters.Count == 0).ToList();
if (ctors.Count != 1) throw new InvalidDataException($"LevelItem instance parameterless ctors={ctors.Count}");
var target = ctors[0];
Console.WriteLine($"CTOR token=0x{target.MetadataToken.ToUInt32():X8} rid={target.MetadataToken.RID} rva=0x{target.RVA:X} code_size={(target.HasBody ? target.Body.CodeSize : 0)} maxstack={(target.HasBody ? target.Body.MaxStackSize : 0)} locals={(target.HasBody ? target.Body.Variables.Count : 0)} initlocals={(target.HasBody && target.Body.InitLocals)} handlers={(target.HasBody ? target.Body.ExceptionHandlers.Count : 0)}");
if (!target.HasBody) throw new InvalidDataException("LevelItem ctor has no body");
for (int i = 0; i < target.Body.Instructions.Count; i++)
{
    var ins = target.Body.Instructions[i];
    Console.WriteLine($"IL index={i} offset=0x{ins.Offset:X4} op={ins.OpCode.Code} operand={Operand(ins.Operand)}");
}

var allRefs = methods.Where(m => m.HasBody).SelectMany(m => m.Body.Instructions)
    .Select(i => i.Operand).OfType<MethodReference>().ToList();
var listIntRefs = allRefs.Where(m => m.DeclaringType.FullName == "System.Collections.Generic.List`1<System.Int32>")
    .GroupBy(m => m.FullName + "@" + Scope(m.DeclaringType.Scope), StringComparer.Ordinal)
    .Select(g => g.First()).OrderBy(m => m.FullName, StringComparer.Ordinal).ToList();
Console.WriteLine($"LIST_INT_METHOD_REF_COUNT {listIntRefs.Count}");
foreach (var r in listIntRefs) Console.WriteLine($"LIST_INT_METHOD_REF {r.FullName}@{Scope(r.DeclaringType.Scope)}");
var listCtor = listIntRefs.Where(m => m.Name == ".ctor" && m.HasThis && m.Parameters.Count == 0).ToList();
var listAdd = listIntRefs.Where(m => m.Name == "Add" && m.HasThis && m.Parameters.Count == 1 && m.Parameters[0].ParameterType.FullName == "System.Int32").ToList();
var listAddWithResize = listIntRefs.Where(m => m.Name == "AddWithResize").ToList();
Console.WriteLine($"LIST_INT_CTOR_REF_COUNT {listCtor.Count}");
Console.WriteLine($"LIST_INT_ADD_REF_COUNT {listAdd.Count}");
Console.WriteLine($"LIST_INT_ADDWITHRESIZE_REF_COUNT {listAddWithResize.Count}");

var monoCtors = allRefs.Where(m => m.DeclaringType.FullName == "UnityEngine.MonoBehaviour" && m.Name == ".ctor" && m.HasThis && m.Parameters.Count == 0)
    .GroupBy(m => m.FullName + "@" + Scope(m.DeclaringType.Scope), StringComparer.Ordinal)
    .Select(g => g.First()).ToList();
Console.WriteLine($"MONOBEHAVIOUR_CTOR_REF_COUNT {monoCtors.Count}");
foreach (var r in monoCtors) Console.WriteLine($"MONOBEHAVIOUR_CTOR_REF {r.FullName}@{Scope(r.DeclaringType.Scope)}");

var targetText = string.Join("\n", target.Body.Instructions.Select(i => $"{i.OpCode.Code} {Operand(i.Operand)}"));
Console.WriteLine($"TARGET_UNMANAGED_DIAGNOSTIC_COUNT {target.Body.Instructions.Count(i => i.Operand is string s && s.Contains("Unmanaged memory load:", StringComparison.Ordinal))}");
Console.WriteLine($"TARGET_LDC_I8_4294967295_COUNT {target.Body.Instructions.Count(i => i.OpCode == OpCodes.Ldc_I8 && i.Operand is long n && n == 4294967295L)}");
Console.WriteLine($"TARGET_ADDWITHRESIZE_CALL_COUNT {target.Body.Instructions.Count(i => i.Operand is MethodReference m && m.Name == "AddWithResize")}");
if (!targetText.Contains("Unmanaged memory load:", StringComparison.Ordinal)) throw new InvalidDataException("expected unmanaged diagnostics absent");
Console.WriteLine($"READONLY_LEVELITEM_CTOR_PROBE_PASS mutation=0 token=0x{target.MetadataToken.ToUInt32():X8} rid={target.MetadataToken.RID}");
return 0;
