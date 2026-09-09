using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF27Patch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "afa0e052902139c09cee9c715fe9a75c6f124af3af8a5647c799dc6b457fefac";
var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(input))).ToLowerInvariant();
Console.WriteLine($"FORMAL_INPUT_SHA256 {inputSha}");
if (!string.Equals(inputSha, ExpectedInputSha256, StringComparison.Ordinal))
    throw new InvalidDataException($"HF27 formal input SHA mismatch: expected {ExpectedInputSha256}, got {inputSha}");

var self = typeof(Template.Projectile).Assembly.Location;
if (string.IsNullOrEmpty(self) || !File.Exists(self))
    throw new InvalidOperationException("HF27 requires a multi-file publish so the template assembly is readable.");

using var target = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadSymbols = false });
using var template = ModuleDefinition.ReadModule(self, new ReaderParameters { InMemory = true, ReadSymbols = false });

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var type in roots)
    {
        yield return type;
        foreach (var nested in AllTypes(type.NestedTypes))
            yield return nested;
    }
}

var targetTypes = AllTypes(target.Types).ToList();
var targetDefs = targetTypes.ToDictionary(t => t.FullName, StringComparer.Ordinal);
var targetTypeRefs = target.GetTypeReferences().GroupBy(t => t.FullName).ToDictionary(g => g.Key, g => g.First(), StringComparer.Ordinal);

string TargetName(string name) => name.StartsWith("Template.", StringComparison.Ordinal) ? name[9..] : name;

TypeReference MapType(TypeReference type, MethodDefinition? destination = null)
{
    if (type is ByReferenceType byRef) return new ByReferenceType(MapType(byRef.ElementType, destination));
    if (type is PointerType ptr) return new PointerType(MapType(ptr.ElementType, destination));
    if (type is ArrayType array) return new ArrayType(MapType(array.ElementType, destination), array.Rank);
    if (type is GenericParameter gp)
    {
        if (destination != null && gp.Type == GenericParameterType.Method && gp.Position < destination.GenericParameters.Count)
            return destination.GenericParameters[gp.Position];
        throw new InvalidDataException($"HF27 unsupported generic parameter {gp.FullName}");
    }

    var name = TargetName(type.FullName);
    if (targetDefs.TryGetValue(name, out var def)) return def;
    if (targetTypeRefs.TryGetValue(name, out var tr)) return tr;
    return name switch
    {
        "System.Void" => target.TypeSystem.Void,
        "System.Boolean" => target.TypeSystem.Boolean,
        "System.Int32" => target.TypeSystem.Int32,
        "System.Single" => target.TypeSystem.Single,
        "System.String" => target.TypeSystem.String,
        "System.Object" => target.TypeSystem.Object,
        _ => throw new InvalidDataException($"HF27 cannot map type {type.FullName} -> {name}")
    };
}

FieldReference MapField(FieldReference field, MethodDefinition destination)
{
    var declaringName = TargetName(field.DeclaringType.FullName);
    if (!targetDefs.TryGetValue(declaringName, out var declaring))
        throw new InvalidDataException($"HF27 cannot resolve declaring type for field {field.FullName}");
    var matches = declaring.Fields.Where(f => f.Name == field.Name).ToList();
    if (matches.Count != 1)
        throw new InvalidDataException($"HF27 field {declaringName}.{field.Name} count={matches.Count}");
    return matches[0];
}

string ParameterSignature(TypeReference type) => TargetName(type.FullName);

MethodReference MapMethod(MethodReference method, MethodDefinition destination)
{
    var declaringName = TargetName(method.DeclaringType.FullName);
    if (!targetDefs.TryGetValue(declaringName, out var declaring))
        throw new InvalidDataException($"HF27 cannot resolve declaring type for method {method.FullName}");

    var wanted = method.Parameters.Select(p => ParameterSignature(p.ParameterType)).ToArray();
    var candidates = declaring.Methods
        .Where(m => m.Name == method.Name && m.Parameters.Count == method.Parameters.Count)
        .ToList();
    if (candidates.Count == 1) return candidates[0];

    var exact = candidates.Where(m => m.Parameters.Select(p => ParameterSignature(p.ParameterType)).SequenceEqual(wanted)).ToList();
    if (exact.Count != 1)
        throw new InvalidDataException($"HF27 method {declaringName}.{method.Name}/{method.Parameters.Count} candidates={candidates.Count} exact={exact.Count}");
    return exact[0];
}

Instruction CopyInstruction(Instruction old, MethodDefinition destination, Dictionary<VariableDefinition, VariableDefinition> variables)
{
    return old.Operand switch
    {
        null => Instruction.Create(old.OpCode),
        sbyte x => Instruction.Create(old.OpCode, x),
        byte x => Instruction.Create(old.OpCode, (sbyte)x),
        int x => Instruction.Create(old.OpCode, x),
        long x => Instruction.Create(old.OpCode, x),
        float x => Instruction.Create(old.OpCode, x),
        double x => Instruction.Create(old.OpCode, x),
        string x => Instruction.Create(old.OpCode, x),
        FieldReference x => Instruction.Create(old.OpCode, MapField(x, destination)),
        MethodReference x => Instruction.Create(old.OpCode, MapMethod(x, destination)),
        TypeReference x => Instruction.Create(old.OpCode, MapType(x, destination)),
        VariableDefinition x => Instruction.Create(old.OpCode, variables[x]),
        ParameterDefinition x => Instruction.Create(old.OpCode, destination.Parameters[x.Index]),
        Instruction x => Instruction.Create(old.OpCode, x),
        Instruction[] x => Instruction.Create(old.OpCode, x),
        _ => throw new NotSupportedException($"HF27 operand {old.Operand.GetType().FullName} at {old}")
    };
}

void CloneBody(MethodDefinition source, MethodDefinition destination)
{
    if (!source.HasBody) throw new InvalidDataException($"HF27 template missing body {source.FullName}");
    var sourceBody = source.Body;
    var destinationBody = new Mono.Cecil.Cil.MethodBody(destination)
    {
        InitLocals = sourceBody.InitLocals,
        MaxStackSize = Math.Max(sourceBody.MaxStackSize, 8)
    };
    destination.Body = destinationBody;

    var variables = new Dictionary<VariableDefinition, VariableDefinition>();
    foreach (var variable in sourceBody.Variables)
    {
        var mapped = new VariableDefinition(MapType(variable.VariableType, destination));
        destinationBody.Variables.Add(mapped);
        variables[variable] = mapped;
    }

    var instructions = new Dictionary<Instruction, Instruction>();
    foreach (var instruction in sourceBody.Instructions)
    {
        var mapped = CopyInstruction(instruction, destination, variables);
        destinationBody.Instructions.Add(mapped);
        instructions[instruction] = mapped;
    }

    foreach (var instruction in sourceBody.Instructions)
    {
        if (instruction.Operand is Instruction branch)
            instructions[instruction].Operand = instructions[branch];
        else if (instruction.Operand is Instruction[] branches)
            instructions[instruction].Operand = branches.Select(x => instructions[x]).ToArray();
    }

    foreach (var handler in sourceBody.ExceptionHandlers)
    {
        destinationBody.ExceptionHandlers.Add(new ExceptionHandler(handler.HandlerType)
        {
            CatchType = handler.CatchType == null ? null : MapType(handler.CatchType, destination),
            TryStart = handler.TryStart == null ? null : instructions[handler.TryStart],
            TryEnd = handler.TryEnd == null ? null : instructions[handler.TryEnd],
            HandlerStart = handler.HandlerStart == null ? null : instructions[handler.HandlerStart],
            HandlerEnd = handler.HandlerEnd == null ? null : instructions[handler.HandlerEnd],
            FilterStart = handler.FilterStart == null ? null : instructions[handler.FilterStart]
        });
    }
}

var patches = new (uint token, string name, int parameters)[]
{
    (0x060003E6, "Collision_Device", 1),
    (0x060003EA, "Collision_Zombie", 1),
};

var templateProjectile = AllTypes(template.Types).Single(t => t.FullName == "Template.Projectile");
foreach (var patch in patches)
{
    var destination = target.LookupToken(new MetadataToken(TokenType.Method, (int)(patch.token & 0x00FFFFFF))) as MethodDefinition
        ?? throw new InvalidDataException($"HF27 target token missing 0x{patch.token:X8}");
    if (destination.DeclaringType.FullName != "Projectile" || destination.Name != patch.name || destination.Parameters.Count != patch.parameters)
        throw new InvalidDataException($"HF27 token drift 0x{patch.token:X8}: {destination.FullName}");

    var sources = templateProjectile.Methods.Where(m => m.Name == patch.name && m.Parameters.Count == patch.parameters).ToList();
    if (sources.Count != 1)
        throw new InvalidDataException($"HF27 template method {patch.name}/{patch.parameters} count={sources.Count}");

    CloneBody(sources[0], destination);
    Console.WriteLine($"PATCH 0x{patch.token:X8} Projectile.{patch.name}/{patch.parameters} il={destination.Body.Instructions.Count} bytes={destination.Body.CodeSize} eh={destination.Body.ExceptionHandlers.Count}");
}

target.Write(output);

using (var reopen = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadSymbols = false }))
{
    foreach (var patch in patches)
    {
        var method = reopen.LookupToken(new MetadataToken(TokenType.Method, (int)(patch.token & 0x00FFFFFF))) as MethodDefinition
            ?? throw new InvalidDataException($"HF27 reopen missing 0x{patch.token:X8}");
        if (!method.HasBody || method.Body.Instructions.Count < 10)
            throw new InvalidDataException($"HF27 reopen invalid {method.FullName}: {method.Body.Instructions.Count} IL");
        if (method.Body.Instructions.Any(i => i.Operand is MethodReference mr && mr.DeclaringType.FullName.Contains("Cpp2IL", StringComparison.Ordinal)))
            throw new InvalidDataException($"HF27 Cpp2IL helper remained in {method.FullName}");
        Console.WriteLine($"REOPEN 0x{patch.token:X8} il={method.Body.Instructions.Count} bytes={method.Body.CodeSize} eh={method.Body.ExceptionHandlers.Count}");
    }
}

return 0;
