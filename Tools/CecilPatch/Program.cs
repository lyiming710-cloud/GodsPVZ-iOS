using Mono.Cecil;
using Mono.Cecil.Cil;

if (args.Length != 3)
{
    Console.Error.WriteLine("Usage: GodsPVZ.CecilPatch <android-recovered.dll> <pc-recovered.dll> <output.dll>");
    return 2;
}

var sourcePath = Path.GetFullPath(args[0]);
var destinationPath = Path.GetFullPath(args[1]);
var outputPath = Path.GetFullPath(args[2]);

using var source = ModuleDefinition.ReadModule(sourcePath, new ReaderParameters { InMemory = true, ReadSymbols = false });
using var destination = ModuleDefinition.ReadModule(destinationPath, new ReaderParameters { InMemory = true, ReadSymbols = false });

var targets = new[]
{
    new MethodKey("Button_Event", "DragSkillTest", new[] { "Plant_DetaiPage" }),
    new MethodKey("Button_Event", "DragCardTest", new[] { "Card" }),
    new MethodKey("Button_Event", "DragPropTest", new[] { "Prop" }),
};

foreach (var key in targets)
{
    var sourceMethod = FindMethod(source, key);
    var destinationMethod = FindMethod(destination, key);

    if (sourceMethod.MetadataToken.RID != destinationMethod.MetadataToken.RID)
        throw new InvalidOperationException($"MethodDef RID mismatch for {key}: src={sourceMethod.MetadataToken}, dst={destinationMethod.MetadataToken}");
    if (!sourceMethod.HasBody)
        throw new InvalidOperationException($"Android source has no body: {key}");

    var before = destinationMethod.HasBody ? destinationMethod.Body.Instructions.Count : 0;
    CloneBody(source, destination, sourceMethod, destinationMethod);
    var after = destinationMethod.Body.Instructions.Count;
    Console.WriteLine($"PATCH {key} token=0x{destinationMethod.MetadataToken.ToUInt32():X8} instructions {before} -> {after}");
}

Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
destination.Write(outputPath, new WriterParameters { WriteSymbols = false });

// Round-trip verification: re-open the written assembly and ensure the three methods are non-trivial.
using var verify = ModuleDefinition.ReadModule(outputPath, new ReaderParameters { InMemory = true, ReadSymbols = false });
foreach (var key in targets)
{
    var method = FindMethod(verify, key);
    if (!method.HasBody || method.Body.Instructions.Count < 4)
        throw new InvalidOperationException($"Patched method is still trivial after write: {key}");
    Console.WriteLine($"VERIFY {key}: {method.Body.Instructions.Count} IL instructions");
}

Console.WriteLine($"WROTE {outputPath} ({new FileInfo(outputPath).Length} bytes)");
return 0;

static MethodDefinition FindMethod(ModuleDefinition module, MethodKey key)
{
    var type = AllTypes(module.Types).SingleOrDefault(t => t.FullName == key.TypeName || t.Name == key.TypeName)
        ?? throw new InvalidOperationException($"Type not found: {key.TypeName}");

    var matches = type.Methods.Where(m =>
        m.Name == key.MethodName &&
        m.Parameters.Count == key.ParameterTypeNames.Length &&
        m.Parameters.Select(p => SimpleTypeName(p.ParameterType)).SequenceEqual(key.ParameterTypeNames))
        .ToList();

    return matches.Count switch
    {
        1 => matches[0],
        0 => throw new InvalidOperationException($"Method not found: {key}"),
        _ => throw new InvalidOperationException($"Method is ambiguous: {key}")
    };
}

static string SimpleTypeName(TypeReference type)
{
    if (type is ByReferenceType br) return SimpleTypeName(br.ElementType) + "&";
    if (type is ArrayType ar) return SimpleTypeName(ar.ElementType) + "[]";
    return type.Name;
}

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var type in roots)
    {
        yield return type;
        foreach (var nested in AllTypes(type.NestedTypes))
            yield return nested;
    }
}

static void CloneBody(
    ModuleDefinition sourceModule,
    ModuleDefinition destinationModule,
    MethodDefinition sourceMethod,
    MethodDefinition destinationMethod)
{
    var srcBody = sourceMethod.Body;
    var dstBody = new MethodBody(destinationMethod)
    {
        InitLocals = srcBody.InitLocals,
        MaxStackSize = srcBody.MaxStackSize,
    };
    destinationMethod.Body = dstBody;

    var variableMap = new Dictionary<VariableDefinition, VariableDefinition>();
    foreach (var srcVar in srcBody.Variables)
    {
        var dstVar = new VariableDefinition(MapType(sourceModule, destinationModule, destinationMethod, srcVar.VariableType));
        dstBody.Variables.Add(dstVar);
        variableMap[srcVar] = dstVar;
    }

    var instructionMap = new Dictionary<Instruction, Instruction>();
    foreach (var srcInstruction in srcBody.Instructions)
    {
        var clone = CreateInstructionSkeleton(srcInstruction);
        dstBody.Instructions.Add(clone);
        instructionMap[srcInstruction] = clone;
    }

    for (var i = 0; i < srcBody.Instructions.Count; i++)
    {
        var srcInstruction = srcBody.Instructions[i];
        var dstInstruction = dstBody.Instructions[i];
        dstInstruction.Operand = MapOperand(
            sourceModule,
            destinationModule,
            sourceMethod,
            destinationMethod,
            srcInstruction.Operand,
            instructionMap,
            variableMap);
    }

    foreach (var srcHandler in srcBody.ExceptionHandlers)
    {
        var dstHandler = new ExceptionHandler(srcHandler.HandlerType)
        {
            TryStart = MapInstruction(srcHandler.TryStart, instructionMap),
            TryEnd = MapInstruction(srcHandler.TryEnd, instructionMap),
            HandlerStart = MapInstruction(srcHandler.HandlerStart, instructionMap),
            HandlerEnd = MapInstruction(srcHandler.HandlerEnd, instructionMap),
            FilterStart = MapInstruction(srcHandler.FilterStart, instructionMap),
            CatchType = srcHandler.CatchType is null
                ? null
                : MapType(sourceModule, destinationModule, destinationMethod, srcHandler.CatchType),
        };
        dstBody.ExceptionHandlers.Add(dstHandler);
    }
}

static Instruction CreateInstructionSkeleton(Instruction instruction)
{
    var op = instruction.OpCode;
    var value = instruction.Operand;
    return value switch
    {
        null => Instruction.Create(op),
        sbyte v => Instruction.Create(op, v),
        byte v => Instruction.Create(op, (sbyte)v),
        int v => Instruction.Create(op, v),
        long v => Instruction.Create(op, v),
        float v => Instruction.Create(op, v),
        double v => Instruction.Create(op, v),
        string v => Instruction.Create(op, v),
        Instruction v => Instruction.Create(op, v),
        Instruction[] v => Instruction.Create(op, v),
        VariableDefinition v => Instruction.Create(op, v),
        ParameterDefinition v => Instruction.Create(op, v),
        MethodReference v => Instruction.Create(op, v),
        FieldReference v => Instruction.Create(op, v),
        TypeReference v => Instruction.Create(op, v),
        CallSite v => Instruction.Create(op, v),
        _ => throw new NotSupportedException($"Unsupported IL operand {value.GetType().FullName} for {instruction}")
    };
}

static object? MapOperand(
    ModuleDefinition sourceModule,
    ModuleDefinition destinationModule,
    MethodDefinition sourceMethod,
    MethodDefinition destinationMethod,
    object? operand,
    IReadOnlyDictionary<Instruction, Instruction> instructionMap,
    IReadOnlyDictionary<VariableDefinition, VariableDefinition> variableMap)
{
    return operand switch
    {
        null => null,
        Instruction i => instructionMap[i],
        Instruction[] a => a.Select(i => instructionMap[i]).ToArray(),
        VariableDefinition v => variableMap[v],
        ParameterDefinition p => destinationMethod.Parameters[p.Index],
        MethodReference m => MapMethod(sourceModule, destinationModule, destinationMethod, m),
        FieldReference f => MapField(sourceModule, destinationModule, destinationMethod, f),
        TypeReference t => MapType(sourceModule, destinationModule, destinationMethod, t),
        CallSite c => destinationModule.ImportReference(c),
        _ => operand,
    };
}

static MethodReference MapMethod(ModuleDefinition sourceModule, ModuleDefinition destinationModule, MethodDefinition context, MethodReference method)
{
    if (method is MethodDefinition definition && definition.Module == sourceModule)
    {
        var mapped = destinationModule.LookupToken(definition.MetadataToken) as MethodDefinition
            ?? throw new InvalidOperationException($"Could not map self MethodDef token {definition.MetadataToken}: {definition.FullName}");
        return mapped;
    }

    // Generic instance methods can wrap a MethodDef from Assembly-CSharp. Rebuild the wrapper on the mapped element method.
    if (method is GenericInstanceMethod generic)
    {
        var element = MapMethod(sourceModule, destinationModule, context, generic.ElementMethod);
        var mapped = new GenericInstanceMethod(element);
        foreach (var argument in generic.GenericArguments)
            mapped.GenericArguments.Add(MapType(sourceModule, destinationModule, context, argument));
        return mapped;
    }

    return destinationModule.ImportReference(method, context);
}

static FieldReference MapField(ModuleDefinition sourceModule, ModuleDefinition destinationModule, MethodDefinition context, FieldReference field)
{
    if (field is FieldDefinition definition && definition.Module == sourceModule)
    {
        return destinationModule.LookupToken(definition.MetadataToken) as FieldDefinition
            ?? throw new InvalidOperationException($"Could not map self FieldDef token {definition.MetadataToken}: {definition.FullName}");
    }
    return destinationModule.ImportReference(field, context);
}

static TypeReference MapType(ModuleDefinition sourceModule, ModuleDefinition destinationModule, MethodDefinition context, TypeReference type)
{
    if (type is TypeDefinition definition && definition.Module == sourceModule)
    {
        return destinationModule.LookupToken(definition.MetadataToken) as TypeDefinition
            ?? throw new InvalidOperationException($"Could not map self TypeDef token {definition.MetadataToken}: {definition.FullName}");
    }

    if (type is ByReferenceType byRef)
        return new ByReferenceType(MapType(sourceModule, destinationModule, context, byRef.ElementType));
    if (type is PointerType pointer)
        return new PointerType(MapType(sourceModule, destinationModule, context, pointer.ElementType));
    if (type is ArrayType array)
        return new ArrayType(MapType(sourceModule, destinationModule, context, array.ElementType), array.Rank);
    if (type is GenericInstanceType generic)
    {
        var mapped = new GenericInstanceType(MapType(sourceModule, destinationModule, context, generic.ElementType));
        foreach (var argument in generic.GenericArguments)
            mapped.GenericArguments.Add(MapType(sourceModule, destinationModule, context, argument));
        return mapped;
    }

    return destinationModule.ImportReference(type, context);
}

static Instruction? MapInstruction(Instruction? instruction, IReadOnlyDictionary<Instruction, Instruction> map)
    => instruction is null ? null : map[instruction];

readonly record struct MethodKey(string TypeName, string MethodName, string[] ParameterTypeNames)
{
    public override string ToString() => $"{TypeName}.{MethodName}({string.Join(",", ParameterTypeNames)})";
}
