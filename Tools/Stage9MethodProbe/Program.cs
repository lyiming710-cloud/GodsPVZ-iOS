using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;
using System.Text;

if (args.Length < 2)
{
    Console.Error.WriteLine("Usage: Stage9MethodProbe <candidate.dll> <Type::Method|Type::.ctor> [...]");
    return 2;
}

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}

static string Sha256(string p) => Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(p))).ToLowerInvariant();
static string StableOperand(object? operand)
{
    return operand switch
    {
        null => "",
        MethodReference mr => $"M:{mr.FullName}",
        FieldReference fr => $"F:{fr.FullName}",
        TypeReference tr => $"T:{tr.FullName}",
        VariableDefinition vd => $"V:{vd.Index}:{vd.VariableType.FullName}",
        ParameterDefinition pd => $"P:{pd.Index}:{pd.ParameterType.FullName}",
        Instruction ins => $"IL_{ins.Offset:X4}",
        Instruction[] arr => string.Join(",", arr.Select(x => $"IL_{x.Offset:X4}")),
        _ => operand.ToString() ?? ""
    };
}

var path = Path.GetFullPath(args[0]);
Console.WriteLine($"INPUT path={path}");
Console.WriteLine($"INPUT_SHA256 {Sha256(path)}");
using var module = ModuleDefinition.ReadModule(path, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var allTypes = AllTypes(module.Types).ToDictionary(t => t.FullName, StringComparer.Ordinal);

foreach (var selector in args.Skip(1))
{
    var split = selector.LastIndexOf("::", StringComparison.Ordinal);
    if (split <= 0 || split >= selector.Length - 2)
    {
        Console.Error.WriteLine($"BAD_SELECTOR {selector}");
        return 3;
    }
    var typeName = selector[..split];
    var methodName = selector[(split + 2)..];
    if (!allTypes.TryGetValue(typeName, out var type))
    {
        Console.Error.WriteLine($"TYPE_NOT_FOUND {typeName}");
        return 4;
    }

    Console.WriteLine($"TYPE name={type.FullName} token=0x{type.MetadataToken.ToUInt32():X8} base={type.BaseType?.FullName ?? "<none>"} fields={type.Fields.Count} methods={type.Methods.Count}");
    var matches = methodName == ".ctor"
        ? type.Methods.Where(m => m.IsConstructor && !m.IsStatic).ToList()
        : type.Methods.Where(m => m.Name == methodName).ToList();
    if (matches.Count == 0)
    {
        Console.Error.WriteLine($"METHOD_NOT_FOUND {selector}");
        return 5;
    }

    foreach (var method in matches.OrderBy(m => m.MetadataToken.RID))
    {
        Console.WriteLine($"METHOD selector={selector} token=0x{method.MetadataToken.ToUInt32():X8} rid={method.MetadataToken.RID} rva=0x{method.RVA:X} params={method.Parameters.Count} return={method.ReturnType.FullName} has_body={method.HasBody}");
        if (!method.HasBody) continue;
        Console.WriteLine($"BODY code_size={method.Body.CodeSize} maxstack={method.Body.MaxStackSize} initlocals={method.Body.InitLocals} locals={method.Body.Variables.Count} eh={method.Body.ExceptionHandlers.Count}");
        foreach (var v in method.Body.Variables)
            Console.WriteLine($"LOCAL index={v.Index} type={v.VariableType.FullName}");

        var fingerprint = new StringBuilder();
        var privateExternal = 0;
        foreach (var i in method.Body.Instructions)
        {
            var operand = StableOperand(i.Operand);
            fingerprint.Append(i.OpCode.Code).Append('|').Append(operand).Append('\n');
            Console.WriteLine($"IL offset=0x{i.Offset:X4} opcode={i.OpCode.Name} operand={operand}");
            if (i.Operand is FieldReference fr)
            {
                try
                {
                    var fd = fr.Resolve();
                    if (fd != null && fd.IsPrivate && fd.DeclaringType.FullName != type.FullName)
                    {
                        privateExternal++;
                        Console.WriteLine($"PRIVATE_EXTERNAL_FIELD offset=0x{i.Offset:X4} opcode={i.OpCode.Name} field={fr.FullName}");
                    }
                }
                catch (AssemblyResolutionException) { }
                catch (ResolutionException) { }
            }
        }
        var fpBytes = SHA256.HashData(Encoding.UTF8.GetBytes(fingerprint.ToString()));
        Console.WriteLine($"METHOD_FINGERPRINT token=0x{method.MetadataToken.ToUInt32():X8} sha256={Convert.ToHexString(fpBytes).ToLowerInvariant()} private_external_fields={privateExternal}");
    }
}
return 0;
