using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.HF29Patch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "8e342bcf856b638d551e286034a5acf32d46a8a07e35e48333f480499bd705dd";
var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(input))).ToLowerInvariant();
Console.WriteLine($"FORMAL_INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256)
    throw new InvalidDataException($"HF29 formal input SHA mismatch: expected {ExpectedInputSha256}, got {inputSha}");

var self = typeof(Template.Projectile).Assembly.Location;
if (string.IsNullOrEmpty(self) || !File.Exists(self))
    throw new InvalidOperationException("HF29 requires a multi-file publish so the template assembly is readable.");

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

string TargetName(string s) => s.StartsWith("Template.", StringComparison.Ordinal) ? s[9..] : s;

TypeReference MapType(TypeReference t, MethodDefinition? dst = null, GenericInstanceType? context = null)
{
    if (t is GenericParameter gp)
    {
        if (gp.Type == GenericParameterType.Type && context != null && gp.Position < context.GenericArguments.Count)
            return MapType(context.GenericArguments[gp.Position], dst);
        if (gp.Type == GenericParameterType.Method && dst != null && gp.Position < dst.GenericParameters.Count)
            return dst.GenericParameters[gp.Position];
        throw new InvalidDataException($"HF29 unsupported generic parameter {gp.FullName}");
    }
    if (t is ByReferenceType br) return new ByReferenceType(MapType(br.ElementType, dst, context));
    if (t is ArrayType at) return new ArrayType(MapType(at.ElementType, dst, context), at.Rank);
    if (t is GenericInstanceType git)
    {
        var g = new GenericInstanceType(MapType(git.ElementType, dst, context));
        foreach (var a in git.GenericArguments) g.GenericArguments.Add(MapType(a, dst));
        return g;
    }

    var name = TargetName(t.FullName);
    if (targetDefs.TryGetValue(name, out var td)) return td;
    if (targetTypeRefs.TryGetValue(name, out var tr)) return tr;
    return name switch
    {
        "System.Void" => target.TypeSystem.Void,
        "System.Boolean" => target.TypeSystem.Boolean,
        "System.Int32" => target.TypeSystem.Int32,
        "System.Single" => target.TypeSystem.Single,
        "System.String" => target.TypeSystem.String,
        "System.Object" => target.TypeSystem.Object,
        _ => throw new InvalidDataException($"HF29 cannot map type {t.FullName} -> {name}")
    };
}

string Sig(TypeReference t) => TargetName(t.FullName);

FieldReference MapField(FieldReference f, MethodDefinition dst)
{
    var decl = TargetName(f.DeclaringType.FullName);
    if (targetDefs.TryGetValue(decl, out var td))
    {
        var q = td.Fields.Where(x => x.Name == f.Name).ToList();
        if (q.Count != 1) throw new InvalidDataException($"HF29 field {decl}.{f.Name} count={q.Count}");
        return q[0];
    }
    var mappedDecl = MapType(f.DeclaringType, dst);
    var mappedType = MapType(f.FieldType, dst, f.DeclaringType as GenericInstanceType);
    var ext = targetFieldRefs.FirstOrDefault(x => x.DeclaringType.FullName == mappedDecl.FullName && x.Name == f.Name);
    return ext ?? new FieldReference(f.Name, mappedType, mappedDecl);
}

MethodReference ConstructMethod(MethodReference m, MethodDefinition dst)
{
    var ctx = m.DeclaringType as GenericInstanceType;
    var x = new MethodReference(m.Name, MapType(m.ReturnType, dst, ctx), MapType(m.DeclaringType, dst))
    {
        HasThis = m.HasThis,
        ExplicitThis = m.ExplicitThis,
        CallingConvention = m.CallingConvention
    };
    foreach (var p in m.GenericParameters) x.GenericParameters.Add(new GenericParameter(p.Name, x));
    foreach (var p in m.Parameters) x.Parameters.Add(new ParameterDefinition(MapType(p.ParameterType, dst, ctx)));
    return x;
}

MethodReference MapMethod(MethodReference m, MethodDefinition dst)
{
    if (m is GenericInstanceMethod gim)
    {
        var e = MapMethod(gim.ElementMethod, dst);
        var g = new GenericInstanceMethod(e);
        foreach (var a in gim.GenericArguments) g.GenericArguments.Add(MapType(a, dst));
        return g;
    }

    var declName = TargetName(m.DeclaringType.FullName);
    if (targetDefs.TryGetValue(declName, out var td))
    {
        var candidates = td.Methods.Where(x => x.Name == m.Name && x.Parameters.Count == m.Parameters.Count && x.GenericParameters.Count == m.GenericParameters.Count).ToList();
        if (candidates.Count == 1) return candidates[0];
        var want = m.Parameters.Select(p => Sig(MapType(p.ParameterType, dst, m.DeclaringType as GenericInstanceType))).ToArray();
        var exact = candidates.Where(x => x.Parameters.Select(p => Sig(p.ParameterType)).SequenceEqual(want)).ToList();
        if (exact.Count == 1) return exact[0];
        throw new InvalidDataException($"HF29 method {declName}.{m.Name}/{m.Parameters.Count} candidates={candidates.Count} exact={exact.Count}");
    }

    var ctx = m.DeclaringType as GenericInstanceType;
    var mappedDecl = MapType(m.DeclaringType, dst);
    var wantParams = m.Parameters.Select(p => Sig(MapType(p.ParameterType, dst, ctx))).ToArray();
    var refs = targetMethodRefs.Where(x => x.DeclaringType.FullName == mappedDecl.FullName && x.Name == m.Name && x.Parameters.Count == wantParams.Length)
        .Where(x => x.Parameters.Select(p => Sig(p.ParameterType)).SequenceEqual(wantParams)).ToList();
    return refs.Count > 0 ? refs[0] : ConstructMethod(m, dst);
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
        _ => throw new NotSupportedException($"HF29 operand {old.Operand.GetType().FullName} at {old}")
    };
}

void CloneBody(MethodDefinition src, MethodDefinition dst)
{
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

const uint Token = 0x060003E4;
var dstMethod = target.LookupToken(new MetadataToken(TokenType.Method, (int)(Token & 0x00FFFFFF))) as MethodDefinition
    ?? throw new InvalidDataException("HF29 target token missing");
if (dstMethod.DeclaringType.FullName != "Projectile" || dstMethod.Name != "CollisionDetect_Zombie" || dstMethod.Parameters.Count != 1)
    throw new InvalidDataException($"HF29 token drift: {dstMethod.FullName}");
var srcType = AllTypes(template.Types).Single(t => t.FullName == "Template.Projectile");
var srcMethod = srcType.Methods.Single(m => m.Name == "CollisionDetect_Zombie" && m.Parameters.Count == 1);
CloneBody(srcMethod, dstMethod);
Console.WriteLine($"PATCH 0x{Token:X8} Projectile.CollisionDetect_Zombie/1 il={dstMethod.Body.Instructions.Count} bytes={dstMethod.Body.CodeSize} eh={dstMethod.Body.ExceptionHandlers.Count}");
target.Write(output);

using (var reopen = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory = true, ReadSymbols = false }))
{
    var m = reopen.LookupToken(new MetadataToken(TokenType.Method, (int)(Token & 0x00FFFFFF))) as MethodDefinition
        ?? throw new InvalidDataException("HF29 reopen missing target");
    if (!m.HasBody || m.Body.Instructions.Count < 100) throw new InvalidDataException($"HF29 reopen invalid: {m.Body.Instructions.Count} IL");
    if (m.Body.Instructions.Any(i => i.Operand is MethodReference mr && mr.DeclaringType.FullName.Contains("Cpp2IL", StringComparison.Ordinal)))
        throw new InvalidDataException("HF29 Cpp2IL helper remained");
    Console.WriteLine($"REOPEN 0x{Token:X8} il={m.Body.Instructions.Count} bytes={m.Body.CodeSize} eh={m.Body.ExceptionHandlers.Count}");
}

return 0;
