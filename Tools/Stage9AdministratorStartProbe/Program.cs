using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: Stage9AdministratorStartProbe <input.dll> <expected-sha256>");
    return 2;
}

const uint TargetToken = 0x06000004;
const uint ModeFieldToken = 0x0400000A;
const long ImageBaseLiteral = 0x180000000L;

static string Sha(string p) => Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(p))).ToLowerInvariant();
static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}
static uint Raw(IMetadataTokenProvider p) => p.MetadataToken.ToUInt32();
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
using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate });
var types = AllTypes(module.Types).ToList();
var methods = types.SelectMany(t => t.Methods).ToList();
var fields = types.SelectMany(t => t.Fields).ToList();
var type = types.Single(x => x.FullName == "Administrator");
var target = methods.Single(m => Raw(m) == TargetToken);
if (target.DeclaringType != type || target.Name != "Start" || target.IsStatic || target.Parameters.Count != 0 || !target.HasBody)
    throw new InvalidDataException("Administrator.Start identity drift");
var mode = fields.Single(f => Raw(f) == ModeFieldToken);
if (mode.DeclaringType != type || mode.Name != "mode") throw new InvalidDataException("Administrator.mode identity drift");
Console.WriteLine($"TYPE token=0x{type.MetadataToken.ToUInt32():X8} rid={type.MetadataToken.RID}");
Console.WriteLine($"METHOD token=0x{TargetToken:X8} rid={target.MetadataToken.RID} rva=0x{target.RVA:X} code_size={target.Body.CodeSize} maxstack={target.Body.MaxStackSize} locals={target.Body.Variables.Count} initlocals={target.Body.InitLocals} handlers={target.Body.ExceptionHandlers.Count}");
Console.WriteLine($"FIELD token=0x{ModeFieldToken:X8} name={mode.Name} type={mode.FieldType.FullName}");
foreach (var v in target.Body.Variables) Console.WriteLine($"LOCAL index={v.Index} type={v.VariableType.FullName}");
for (int i=0;i<target.Body.Instructions.Count;i++)
{
    var ins=target.Body.Instructions[i];
    Console.WriteLine($"IL index={i} offset=0x{ins.Offset:X4} op={ins.OpCode.Code} operand={Operand(ins.Operand)}");
}

var imageBaseLiterals = target.Body.Instructions.Count(i => i.OpCode == OpCodes.Ldc_I8 && i.Operand is long v && v == ImageBaseLiteral);
var unmanagedDiagnostics = target.Body.Instructions.Count(i => i.OpCode == OpCodes.Ldstr && i.Operand is string s && s.Contains("Unmanaged memory load:", StringComparison.Ordinal));
var indirectDiagnostics = target.Body.Instructions.Count(i => i.OpCode == OpCodes.Ldstr && i.Operand is string s && s.Contains("Indirect jump:", StringComparison.Ordinal));
var convI = target.Body.Instructions.Count(i => i.OpCode == OpCodes.Conv_I);
var modeLoads = target.Body.Instructions.Count(i => i.Operand is FieldReference f && f.MetadataToken.ToUInt32() == ModeFieldToken);
Console.WriteLine($"SWITCH_DAMAGE_FINGERPRINT image_base_ldc_i8={imageBaseLiterals} unmanaged_load_strings={unmanagedDiagnostics} indirect_jump_strings={indirectDiagnostics} conv_i={convI} mode_field_refs={modeLoads}");
if (imageBaseLiterals < 2 || unmanagedDiagnostics < 1 || indirectDiagnostics < 1 || convI < 1 || modeLoads < 1)
    throw new InvalidDataException("Administrator.Start switch damage fingerprint drift");
Console.WriteLine($"READONLY_ADMINISTRATOR_START_PROBE_PASS mutation=0 token=0x{TargetToken:X8} rid={target.MetadataToken.RID} switch_damage=1");
return 0;
