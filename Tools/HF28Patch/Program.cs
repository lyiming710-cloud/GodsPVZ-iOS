using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF28Patch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "18d9efebdcfcccbbe6c805a9276c4427b4e6baa33673fb71b96a04245bb0e8de";
var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var actualInputSha256 = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(input))).ToLowerInvariant();
Console.WriteLine($"FORMAL_INPUT_SHA256 {actualInputSha256}");
if (!string.Equals(actualInputSha256, ExpectedInputSha256, StringComparison.Ordinal))
    throw new InvalidDataException($"HF28 formal input SHA mismatch: expected {ExpectedInputSha256}, got {actualInputSha256}");

var self = typeof(Template.Projectile).Assembly.Location;
if (string.IsNullOrEmpty(self) || !File.Exists(self))
    throw new InvalidOperationException("HF28 requires a multi-file publish so the template assembly is readable.");

using var target = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadSymbols = false });
using var template = ModuleDefinition.ReadModule(self, new ReaderParameters { InMemory = true, ReadSymbols = false });

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}

var targetTypes = AllTypes(target.Types).ToList();
var targetDefs = targetTypes.ToDictionary(t => t.FullName, StringComparer.Ordinal);
var targetTypeRefs = target.GetTypeReferences().GroupBy(t => t.FullName).ToDictionary(g => g.Key, g => g.First(), StringComparer.Ordinal);
var targetMethodRefs = targetTypes.SelectMany(t => t.Methods).Where(m => m.HasBody)
    .SelectMany(m => m.Body.Instructions).Select(i => i.Operand).OfType<MethodReference>()
    .Select(m => m is GenericInstanceMethod gim ? gim.ElementMethod : m).ToList();
var targetFieldRefs = targetTypes.SelectMany(t => t.Methods).Where(m => m.HasBody)
    .SelectMany(m => m.Body.Instructions).Select(i => i.Operand).OfType<FieldReference>().ToList();

string TargetName(string fullName) => fullName.StartsWith("Template.", StringComparison.Ordinal) ? fullName[9..] : fullName;

TypeReference MapType(TypeReference t, MethodDefinition? targetMethod = null, GenericInstanceType? typeContext = null)
{
    if (t is GenericParameter gp)
    {
        if (gp.Type == GenericParameterType.Type && typeContext != null && gp.Position < typeContext.GenericArguments.Count)
            return MapType(typeContext.GenericArguments[gp.Position], targetMethod);
        if (targetMethod != null && gp.Type == GenericParameterType.Method && gp.Position < targetMethod.GenericParameters.Count)
            return targetMethod.GenericParameters[gp.Position];
        throw new InvalidDataException($"HF28 unsupported generic parameter {gp.FullName}");
    }
    if (t is ByReferenceType br) return new ByReferenceType(MapType(br.ElementType, targetMethod, typeContext));
    if (t is PointerType pt) return new PointerType(MapType(pt.ElementType, targetMethod, typeContext));
    if (t is ArrayType at) return new ArrayType(MapType(at.ElementType, targetMethod, typeContext), at.Rank);
    if (t is GenericInstanceType git)
    {
        var mapped = new GenericInstanceType(MapType(git.ElementType, targetMethod, typeContext));
        foreach (var a in git.GenericArguments) mapped.GenericArguments.Add(MapType(a, targetMethod, typeContext));
        return mapped;
    }
    if (t is OptionalModifierType omt) return new OptionalModifierType(MapType(omt.ModifierType, targetMethod, typeContext), MapType(omt.ElementType, targetMethod, typeContext));
    if (t is RequiredModifierType rmt) return new RequiredModifierType(MapType(rmt.ModifierType, targetMethod, typeContext), MapType(rmt.ElementType, targetMethod, typeContext));
    if (t is PinnedType pin) return new PinnedType(MapType(pin.ElementType, targetMethod, typeContext));

    var name = TargetName(t.FullName);
    if (targetDefs.TryGetValue(name, out var def)) return def;
    if (targetTypeRefs.TryGetValue(name, out var tr)) return tr;
    return name switch
    {
        "System.Void" => target.TypeSystem.Void,
        "System.Boolean" => target.TypeSystem.Boolean,
        "System.Byte" => target.TypeSystem.Byte,
        "System.SByte" => target.TypeSystem.SByte,
        "System.Int16" => target.TypeSystem.Int16,
        "System.UInt16" => target.TypeSystem.UInt16,
        "System.Int32" => target.TypeSystem.Int32,
        "System.UInt32" => target.TypeSystem.UInt32,
        "System.Int64" => target.TypeSystem.Int64,
        "System.UInt64" => target.TypeSystem.UInt64,
        "System.Single" => target.TypeSystem.Single,
        "System.Double" => target.TypeSystem.Double,
        "System.Char" => target.TypeSystem.Char,
        "System.String" => target.TypeSystem.String,
        "System.Object" => target.TypeSystem.Object,
        _ => throw new InvalidDataException($"HF28 cannot map type {t.FullName} -> {name}")
    };
}

string Sig(TypeReference t) => TargetName(t.FullName);

FieldReference MapField(FieldReference f, MethodDefinition targetMethod)
{
    var declName = TargetName(f.DeclaringType.FullName);
    if (targetDefs.TryGetValue(declName, out var td))
    {
        var q = td.Fields.Where(x => x.Name == f.Name).ToList();
        if (q.Count != 1) throw new InvalidDataException($"HF28 field {declName}.{f.Name} count={q.Count}");
        return q[0];
    }

    var mappedDecl = MapType(f.DeclaringType, targetMethod);
    var mappedFieldType = MapType(f.FieldType, targetMethod, f.DeclaringType as GenericInstanceType);
    var ext = targetFieldRefs.FirstOrDefault(x => x.DeclaringType.FullName == mappedDecl.FullName && x.Name == f.Name);
    return ext ?? new FieldReference(f.Name, mappedFieldType, mappedDecl);
}

MethodReference ConstructMethod(MethodReference m, MethodDefinition targetMethod)
{
    var typeContext = m.DeclaringType as GenericInstanceType;
    var x = new MethodReference(m.Name, MapType(m.ReturnType, targetMethod, typeContext), MapType(m.DeclaringType, targetMethod))
    {
        HasThis = m.HasThis,
        ExplicitThis = m.ExplicitThis,
        CallingConvention = m.CallingConvention
    };
    for (var i = 0; i < m.GenericParameters.Count; i++) x.GenericParameters.Add(new GenericParameter(m.GenericParameters[i].Name, x));
    foreach (var p in m.Parameters) x.Parameters.Add(new ParameterDefinition(MapType(p.ParameterType, targetMethod, typeContext)));
    return x;
}

MethodReference MapMethod(MethodReference m, MethodDefinition targetMethod)
{
    if (m is GenericInstanceMethod gim)
    {
        var element = MapMethod(gim.ElementMethod, targetMethod);
        var mapped = new GenericInstanceMethod(element);
        foreach (var a in gim.GenericArguments) mapped.GenericArguments.Add(MapType(a, targetMethod));
        return mapped;
    }

    var customDeclName = TargetName(m.DeclaringType.FullName);
    if (targetDefs.TryGetValue(customDeclName, out var td))
    {
        var q = td.Methods.Where(x => x.Name == m.Name && x.Parameters.Count == m.Parameters.Count && x.GenericParameters.Count == m.GenericParameters.Count).ToList();
        if (q.Count == 1) return q[0];
        var want = m.Parameters.Select(p => p.ParameterType is GenericParameter gp ? $"GP:{gp.Type}:{gp.Position}" : Sig(MapType(p.ParameterType, targetMethod))).ToArray();
        var r = q.Where(x => x.Parameters.Select(p => p.ParameterType is GenericParameter gp ? $"GP:{gp.Type}:{gp.Position}" : Sig(p.ParameterType)).SequenceEqual(want)).ToList();
        if (r.Count == 1) return r[0];
        throw new InvalidDataException($"HF28 custom MethodRef {customDeclName}::{m.Name}/{m.Parameters.Count} count={q.Count} matched={r.Count}");
    }

    var typeContext = m.DeclaringType as GenericInstanceType;
    var mappedDecl = MapType(m.DeclaringType, targetMethod);
    var wantParams = m.Parameters.Select(p => Sig(MapType(p.ParameterType, targetMethod, typeContext))).ToArray();
    var ext = targetMethodRefs.Where(x => x.DeclaringType.FullName == mappedDecl.FullName && x.Name == m.Name && x.Parameters.Count == wantParams.Length)
        .Where(x => x.Parameters.Select(p => Sig(p.ParameterType)).SequenceEqual(wantParams)).ToList();
    return ext.Count > 0 ? ext[0] : ConstructMethod(m, targetMethod);
}

Instruction CopyInstruction(Instruction old, MethodDefinition dst, Dictionary<VariableDefinition, VariableDefinition> vars)
{
    var op = old.OpCode;
    return old.Operand switch
    {
        null => Instruction.Create(op),
        sbyte x => Instruction.Create(op, x),
        byte x => Instruction.Create(op, (sbyte)x),
        int x => Instruction.Create(op, x),
        long x => Instruction.Create(op, x),
        float x => Instruction.Create(op, x),
        double x => Instruction.Create(op, x),
        string x => Instruction.Create(op, x),
        FieldReference x => Instruction.Create(op, MapField(x, dst)),
        MethodReference x => Instruction.Create(op, MapMethod(x, dst)),
        TypeReference x => Instruction.Create(op, MapType(x, dst)),
        VariableDefinition x => Instruction.Create(op, vars[x]),
        ParameterDefinition x => Instruction.Create(op, dst.Parameters[x.Index]),
        Instruction x => Instruction.Create(op, x),
        Instruction[] x => Instruction.Create(op, x),
        _ => throw new NotSupportedException($"HF28 operand {old.Operand.GetType().FullName} at {old}")
    };
}

void CloneBody(MethodDefinition src, MethodDefinition dst)
{
    if (!src.HasBody) throw new InvalidDataException($"HF28 template missing body {src.FullName}");
    var sb = src.Body;
    var db = new Mono.Cecil.Cil.MethodBody(dst) { InitLocals = sb.InitLocals, MaxStackSize = Math.Max(sb.MaxStackSize, 8) };
    dst.Body = db;
    var vars = new Dictionary<VariableDefinition, VariableDefinition>();
    foreach (var v in sb.Variables)
    {
        var nv = new VariableDefinition(MapType(v.VariableType, dst));
        db.Variables.Add(nv);
        vars[v] = nv;
    }
    var imap = new Dictionary<Instruction, Instruction>();
    foreach (var i in sb.Instructions)
    {
        var ni = CopyInstruction(i, dst, vars);
        db.Instructions.Add(ni);
        imap[i] = ni;
    }
    foreach (var i in sb.Instructions)
    {
        if (i.Operand is Instruction b) imap[i].Operand = imap[b];
        else if (i.Operand is Instruction[] sw) imap[i].Operand = sw.Select(x => imap[x]).ToArray();
    }
    foreach (var eh in sb.ExceptionHandlers)
    {
        db.ExceptionHandlers.Add(new ExceptionHandler(eh.HandlerType)
        {
            CatchType = eh.CatchType == null ? null : MapType(eh.CatchType, dst),
            TryStart = eh.TryStart == null ? null : imap[eh.TryStart],
            TryEnd = eh.TryEnd == null ? null : imap[eh.TryEnd],
            HandlerStart = eh.HandlerStart == null ? null : imap[eh.HandlerStart],
            HandlerEnd = eh.HandlerEnd == null ? null : imap[eh.HandlerEnd],
            FilterStart = eh.FilterStart == null ? null : imap[eh.FilterStart]
        });
    }
}

const uint Token = 0x060003E3;
var dstMethod = target.LookupToken(new MetadataToken(TokenType.Method, (int)(Token & 0x00FFFFFF))) as MethodDefinition
    ?? throw new InvalidDataException("HF28 target token missing");
if (dstMethod.DeclaringType.FullName != "Projectile" || dstMethod.Name != "CollisionDetect_Plant" || dstMethod.Parameters.Count != 1)
    throw new InvalidDataException($"HF28 token drift: {dstMethod.FullName}");
var srcType = AllTypes(template.Types).Single(t => t.FullName == "Template.Projectile");
var srcMethod = srcType.Methods.Single(m => m.Name == "CollisionDetect_Plant" && m.Parameters.Count == 1);
CloneBody(srcMethod, dstMethod);
Console.WriteLine($"PATCH 0x{Token:X8} Projectile.CollisionDetect_Plant/1 il={dstMethod.Body.Instructions.Count} bytes={dstMethod.Body.CodeSize} eh={dstMethod.Body.ExceptionHandlers.Count}");
target.Write(output);

using (var reopen = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadSymbols = false }))
{
    var m = reopen.LookupToken(new MetadataToken(TokenType.Method, (int)(Token & 0x00FFFFFF))) as MethodDefinition
        ?? throw new InvalidDataException("HF28 reopen missing target");
    if (!m.HasBody || m.Body.Instructions.Count < 50) throw new InvalidDataException($"HF28 reopen invalid: {m.Body.Instructions.Count} IL");
    if (m.Body.Instructions.Any(i => i.Operand is MethodReference mr && mr.DeclaringType.FullName.Contains("Cpp2IL", StringComparison.Ordinal)))
        throw new InvalidDataException("HF28 Cpp2IL helper remained");
    Console.WriteLine($"REOPEN 0x{Token:X8} il={m.Body.Instructions.Count} bytes={m.Body.CodeSize} eh={m.Body.ExceptionHandlers.Count}");
}

return 0;
