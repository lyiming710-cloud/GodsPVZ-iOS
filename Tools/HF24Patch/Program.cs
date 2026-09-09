using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF24Patch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "35d13b0d7fc5f82e04b837b812fab501a933b00e21f3431b2f6759c8e0c2b39c";
var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var actualInputSha256 = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(input))).ToLowerInvariant();
Console.WriteLine($"FORMAL_INPUT_SHA256 {actualInputSha256}");
if (!string.Equals(actualInputSha256, ExpectedInputSha256, StringComparison.Ordinal))
    throw new InvalidDataException($"HF24 formal input SHA mismatch: expected {ExpectedInputSha256}, got {actualInputSha256}");

var self = typeof(Template.Projectile).Assembly.Location;
if (string.IsNullOrEmpty(self) || !File.Exists(self))
    throw new InvalidOperationException("HF24 requires a multi-file publish so the template assembly is readable.");

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
        throw new InvalidDataException($"Unsupported generic parameter {gp.FullName} owner={gp.Owner} context={typeContext}");
    }
    if (t is ByReferenceType br) return new ByReferenceType(MapType(br.ElementType, targetMethod, typeContext));
    if (t is PointerType pt) return new PointerType(MapType(pt.ElementType, targetMethod, typeContext));
    if (t is ArrayType at) return new ArrayType(MapType(at.ElementType, targetMethod, typeContext), at.Rank);
    if (t is GenericInstanceType git)
    {
        var x = new GenericInstanceType(MapType(git.ElementType, targetMethod, typeContext));
        foreach (var a in git.GenericArguments) x.GenericArguments.Add(MapType(a, targetMethod, typeContext));
        return x;
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
        _ => throw new InvalidDataException($"HF24 cannot map type {t.FullName} -> {name}")
    };
}

string Sig(TypeReference t) => TargetName(t.FullName);

FieldReference MapField(FieldReference f, MethodDefinition targetMethod)
{
    var declName = TargetName(f.DeclaringType.FullName);
    if (targetDefs.TryGetValue(declName, out var td))
    {
        var q = td.Fields.Where(x => x.Name == f.Name).ToList();
        if (q.Count != 1) throw new InvalidDataException($"HF24 field {declName}.{f.Name} count={q.Count}");
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
    var decl = MapType(m.DeclaringType, targetMethod);
    var ret = MapType(m.ReturnType, targetMethod, typeContext);
    var x = new MethodReference(m.Name, ret, decl)
    {
        HasThis = m.HasThis,
        ExplicitThis = m.ExplicitThis,
        CallingConvention = m.CallingConvention
    };
    for (int i = 0; i < m.GenericParameters.Count; i++) x.GenericParameters.Add(new GenericParameter(m.GenericParameters[i].Name, x));
    foreach (var p in m.Parameters) x.Parameters.Add(new ParameterDefinition(MapType(p.ParameterType, targetMethod, typeContext)));
    return x;
}

MethodReference MapMethod(MethodReference m, MethodDefinition targetMethod)
{
    if (m is GenericInstanceMethod gim)
    {
        var e = MapMethod(gim.ElementMethod, targetMethod);
        var g = new GenericInstanceMethod(e);
        foreach (var a in gim.GenericArguments) g.GenericArguments.Add(MapType(a, targetMethod));
        return g;
    }

    var customDeclName = TargetName(m.DeclaringType.FullName);
    if (targetDefs.TryGetValue(customDeclName, out var td))
    {
        var q = td.Methods.Where(x => x.Name == m.Name && x.Parameters.Count == m.Parameters.Count
            && x.GenericParameters.Count == m.GenericParameters.Count).ToList();
        if (q.Count == 1) return q[0];

        var wantParams = m.Parameters.Select(p =>
            p.ParameterType is GenericParameter ? $"GP:{((GenericParameter)p.ParameterType).Type}:{((GenericParameter)p.ParameterType).Position}"
            : Sig(MapType(p.ParameterType, targetMethod))).ToArray();
        var r = q.Where(x => x.Parameters.Select(p =>
            p.ParameterType is GenericParameter ? $"GP:{((GenericParameter)p.ParameterType).Type}:{((GenericParameter)p.ParameterType).Position}"
            : Sig(p.ParameterType)).SequenceEqual(wantParams)).ToList();
        if (r.Count == 1) return r[0];
        throw new InvalidDataException($"HF24 custom MethodRef {customDeclName}::{m.Name}/{m.Parameters.Count} count={q.Count} matched={r.Count}");
    }

    var typeContext = m.DeclaringType as GenericInstanceType;
    var mappedDecl = MapType(m.DeclaringType, targetMethod);
    var declName = mappedDecl.FullName;
    var wantParamsExternal = m.Parameters.Select(p => Sig(MapType(p.ParameterType, targetMethod, typeContext))).ToArray();
    var ext = targetMethodRefs.Where(x => x.DeclaringType.FullName == declName && x.Name == m.Name && x.Parameters.Count == wantParamsExternal.Length)
        .Where(x => x.Parameters.Select(p => Sig(p.ParameterType)).SequenceEqual(wantParamsExternal)).ToList();
    if (ext.Count > 0) return ext[0];
    return ConstructMethod(m, targetMethod);
}

Instruction NewInstruction(Instruction old, MethodDefinition targetMethod, Dictionary<VariableDefinition, VariableDefinition> vars)
{
    var op = old.OpCode;
    var v = old.Operand;
    if (v == null) return Instruction.Create(op);
    return v switch
    {
        sbyte x => Instruction.Create(op, x),
        byte x => Instruction.Create(op, (sbyte)x),
        int x => Instruction.Create(op, x),
        long x => Instruction.Create(op, x),
        float x => Instruction.Create(op, x),
        double x => Instruction.Create(op, x),
        string x => Instruction.Create(op, x),
        FieldReference x => Instruction.Create(op, MapField(x, targetMethod)),
        MethodReference x => Instruction.Create(op, MapMethod(x, targetMethod)),
        TypeReference x => Instruction.Create(op, MapType(x, targetMethod)),
        VariableDefinition x => Instruction.Create(op, vars[x]),
        ParameterDefinition x => Instruction.Create(op, targetMethod.Parameters[x.Index]),
        Instruction x => Instruction.Create(op, x),
        Instruction[] x => Instruction.Create(op, x),
        CallSite x => throw new NotSupportedException($"HF24 CallSite operand {x}"),
        _ => throw new NotSupportedException($"HF24 operand {v.GetType().FullName} at {old}")
    };
}

void CloneBody(MethodDefinition src, MethodDefinition dst)
{
    if (!src.HasBody) throw new InvalidDataException($"Template missing body {src.FullName}");
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
        var ni = NewInstruction(i, dst, vars);
        db.Instructions.Add(ni);
        imap[i] = ni;
    }
    foreach (var i in sb.Instructions)
    {
        var ni = imap[i];
        if (i.Operand is Instruction b) ni.Operand = imap[b];
        else if (i.Operand is Instruction[] sw) ni.Operand = sw.Select(x => imap[x]).ToArray();
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

var pairs = new (uint token, string type, string method, int parameters)[]
{
    (0x060003D2, "Projectile", "Update", 0),
};

var templateTypes = AllTypes(template.Types).ToDictionary(t => t.FullName, StringComparer.Ordinal);
foreach (var p in pairs)
{
    var dst = target.LookupToken(new MetadataToken(TokenType.Method, (int)(p.token & 0x00FFFFFF))) as MethodDefinition
        ?? throw new InvalidDataException($"HF24 target token missing 0x{p.token:X8}");
    if (dst.DeclaringType.FullName != p.type || dst.Name != p.method || dst.Parameters.Count != p.parameters)
        throw new InvalidDataException($"HF24 token drift 0x{p.token:X8}: {dst.FullName}");
    var st = templateTypes[$"Template.{p.type}"];
    var srcs = st.Methods.Where(m => m.Name == p.method && m.Parameters.Count == p.parameters).ToList();
    if (srcs.Count != 1) throw new InvalidDataException($"HF24 template {p.type}.{p.method}/{p.parameters} count={srcs.Count}");
    CloneBody(srcs[0], dst);
    Console.WriteLine($"PATCH 0x{p.token:X8} {p.type}.{p.method}/{p.parameters} il={dst.Body.Instructions.Count} bytes={dst.Body.CodeSize} eh={dst.Body.ExceptionHandlers.Count}");
}

target.Write(output);

using (var reopen = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadSymbols = false }))
{
    foreach (var p in pairs)
    {
        var m = reopen.LookupToken(new MetadataToken(TokenType.Method, (int)(p.token & 0x00FFFFFF))) as MethodDefinition
            ?? throw new InvalidDataException($"HF24 reopen missing 0x{p.token:X8}");
        if (!m.HasBody || m.Body.Instructions.Count < 5) throw new InvalidDataException($"HF24 reopen invalid {m.FullName}");
        var helper = m.Body.Instructions.Any(i => i.Operand is MethodReference mr && mr.DeclaringType.FullName.Contains("Cpp2IL", StringComparison.Ordinal));
        if (helper) throw new InvalidDataException($"HF24 Cpp2IL helper remained in {m.FullName}");
        Console.WriteLine($"REOPEN 0x{p.token:X8} il={m.Body.Instructions.Count} bytes={m.Body.CodeSize} eh={m.Body.ExceptionHandlers.Count}");
    }
}

return 0;
