using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: Stage9TextLinkUpdateProbe <input.dll> <expected-sha256>");
    return 2;
}

const uint TargetToken = 0x060006DE;
const uint TmpFieldToken = 0x040008E1;
const uint LinkFieldToken = 0x040008E2;
const uint LastFieldToken = 0x040008E3;

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
var t = types.Single(x => x.FullName == "TextLink");
var target = methods.Single(m => Raw(m) == TargetToken);
if (target.DeclaringType != t || target.Name != "Update" || target.IsStatic || target.Parameters.Count != 0 || !target.HasBody)
    throw new InvalidDataException("TextLink.Update identity drift");
Console.WriteLine($"TYPE token=0x{t.MetadataToken.ToUInt32():X8} rid={t.MetadataToken.RID}");
Console.WriteLine($"METHOD token=0x{TargetToken:X8} rid={target.MetadataToken.RID} rva=0x{target.RVA:X} code_size={target.Body.CodeSize} maxstack={target.Body.MaxStackSize} locals={target.Body.Variables.Count} initlocals={target.Body.InitLocals} handlers={target.Body.ExceptionHandlers.Count}");
foreach (var v in target.Body.Variables) Console.WriteLine($"LOCAL index={v.Index} type={v.VariableType.FullName}");
foreach (var ftoken in new[]{TmpFieldToken,LinkFieldToken,LastFieldToken})
{
    var f=fields.Single(x=>Raw(x)==ftoken);
    Console.WriteLine($"FIELD token=0x{ftoken:X8} name={f.Name} type={f.FieldType.FullName}");
}
for (int i=0;i<target.Body.Instructions.Count;i++)
{
    var ins=target.Body.Instructions[i];
    Console.WriteLine($"IL index={i} offset=0x{ins.Offset:X4} op={ins.OpCode.Code} operand={Operand(ins.Operand)}");
}

var findCalls=target.Body.Instructions.Where(i=>i.Operand is MethodReference m && m.DeclaringType.FullName=="TMPro.TMP_TextUtilities" && m.Name=="FindIntersectingLink").ToList();
if(findCalls.Count!=1) throw new InvalidDataException($"FindIntersectingLink calls={findCalls.Count}");
var callIndex=target.Body.Instructions.IndexOf(findCalls[0]);
if(callIndex<3) throw new InvalidDataException("FindIntersectingLink prelude too short");
var vecIns=target.Body.Instructions[callIndex-2];
var camIns=target.Body.Instructions[callIndex-1];
Console.WriteLine($"FINDLINK_VECTOR_ARG index={callIndex-2} op={vecIns.OpCode.Code} operand={Operand(vecIns.Operand)}");
Console.WriteLine($"FINDLINK_CAMERA_ARG index={callIndex-1} op={camIns.OpCode.Code} operand={Operand(camIns.Operand)}");
if(vecIns.OpCode != OpCodes.Ldloca || vecIns.Operand is not VariableDefinition bad || bad.Index!=8 || bad.VariableType.FullName!="System.Object")
    throw new InvalidDataException("expected damaged FindIntersectingLink Vector3 argument not found");
if(camIns.OpCode != OpCodes.Ldloc || camIns.Operand is not VariableDefinition cam || cam.Index!=2 || cam.VariableType.FullName!="UnityEngine.Camera")
    throw new InvalidDataException("camera argument drift");
var mouseCalls=target.Body.Instructions.Count(i=>i.Operand is MethodReference m && m.DeclaringType.FullName=="UnityEngine.Input" && m.Name=="get_mousePosition" && m.ReturnType.FullName=="UnityEngine.Vector3");
var mainCalls=target.Body.Instructions.Count(i=>i.Operand is MethodReference m && m.DeclaringType.FullName=="UnityEngine.Camera" && m.Name=="get_main");
var setCalls=target.Body.Instructions.Count(i=>i.Operand is MethodReference m && m.DeclaringType.FullName=="TextLink" && m.Name=="SetLink");
var resetCalls=target.Body.Instructions.Count(i=>i.Operand is MethodReference m && m.DeclaringType.FullName=="TextLink" && m.Name=="ResetLink");
Console.WriteLine($"CALL_COUNTS mousePosition={mouseCalls} cameraMain={mainCalls} findIntersectingLink={findCalls.Count} setLink={setCalls} resetLink={resetCalls}");
if(mouseCalls!=1 || mainCalls!=1 || setCalls!=1 || resetCalls!=1) throw new InvalidDataException("expected call fingerprint drift");
Console.WriteLine($"READONLY_TEXTLINK_UPDATE_PROBE_PASS mutation=0 token=0x{TargetToken:X8} damaged_vector_arg=V_8_object_byref");
return 0;
