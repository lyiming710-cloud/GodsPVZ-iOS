using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;

if (args.Length != 1)
{
    Console.Error.WriteLine("usage: Stage9AlmanacCtorProbe <GodsPVZRuntime1.dll>");
    return 2;
}

var input = Path.GetFullPath(args[0]);
if (!File.Exists(input)) throw new FileNotFoundException(input);
var inputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(input))).ToLowerInvariant();
Console.WriteLine($"INPUT_SHA256 {inputSha}");

using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { ReadSymbols = false, InMemory = true });
var type = module.Types.SingleOrDefault(t => t.FullName == "Almanac_ZombieWindow")
    ?? throw new InvalidOperationException("Almanac_ZombieWindow not found");
var ctor = type.Methods.SingleOrDefault(m => m.IsConstructor && !m.IsStatic && m.Parameters.Count == 0)
    ?? throw new InvalidOperationException("Almanac_ZombieWindow::.ctor() not found or ambiguous");
var zombieId = type.Fields.SingleOrDefault(f => f.Name == "zombieID")
    ?? throw new InvalidOperationException("Almanac_ZombieWindow::zombieID not found");

Console.WriteLine($"TYPE_TOKEN 0x{type.MetadataToken.ToInt32():X8} RID={type.MetadataToken.RID}");
Console.WriteLine($"CTOR_TOKEN 0x{ctor.MetadataToken.ToInt32():X8} RID={ctor.MetadataToken.RID} RVA=0x{ctor.RVA:X} HAS_BODY={(ctor.HasBody ? 1 : 0)}");
Console.WriteLine($"ZOMBIEID_FIELD_TOKEN 0x{zombieId.MetadataToken.ToInt32():X8} RID={zombieId.MetadataToken.RID} TYPE={zombieId.FieldType.FullName}");

if (!ctor.HasBody) throw new InvalidOperationException("constructor has no body");
Console.WriteLine($"CTOR_BODY code_size={ctor.Body.CodeSize} maxstack={ctor.Body.MaxStackSize} locals={ctor.Body.Variables.Count} init_locals={(ctor.Body.InitLocals ? 1 : 0)}");
foreach (var ins in ctor.Body.Instructions)
{
    string operand = ins.Operand switch
    {
        null => "",
        MethodReference mr => $"{mr.FullName} token=0x{mr.MetadataToken.ToInt32():X8}",
        FieldReference fr => $"{fr.FullName} token=0x{fr.MetadataToken.ToInt32():X8}",
        TypeReference tr => $"{tr.FullName} token=0x{tr.MetadataToken.ToInt32():X8}",
        Instruction target => $"IL_{target.Offset:X4}",
        Instruction[] targets => string.Join(",", targets.Select(t => $"IL_{t.Offset:X4}")),
        _ => ins.Operand.ToString() ?? ""
    };
    Console.WriteLine($"IL_{ins.Offset:X4} {ins.OpCode.Code} {operand}".TrimEnd());
}

var ldcI8 = ctor.Body.Instructions.Count(i => i.OpCode.Code == Code.Ldc_I8);
var zombieStores = ctor.Body.Instructions.Count(i => i.OpCode.Code == Code.Stfld && i.Operand is FieldReference f && f.Name == "zombieID");
var monoBehaviourCtors = ctor.Body.Instructions.Count(i => i.OpCode.Code == Code.Call && i.Operand is MethodReference m && m.Name == ".ctor" && m.DeclaringType.FullName == "UnityEngine.MonoBehaviour");
Console.WriteLine($"READONLY_ALMANAC_CTOR_PROBE_PASS ldc_i8={ldcI8} zombieID_stfld={zombieStores} monoBehaviour_ctor={monoBehaviourCtors} mutation=0");
return 0;
