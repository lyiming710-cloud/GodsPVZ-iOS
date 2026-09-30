using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;
using System.Text.Json;
using CM = Mono.Cecil.Cil.MethodBody;

// This is a pinned six-site materializer, never a general automated null repair.
internal static class Program
{
    const string InputHash = "0d6bc587c98ff05b733d9d5bced8b96726d2dd5d2ba82cb949c6ca90b8f86e8f";
    const string EditsHash = "57825a4b301608936b1a3c9386596e2fc6c4d314f372d0369407bb331d6349e2";
    static string Hash(string path) => Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path))).ToLowerInvariant();
    static void Require(bool ok, string message) { if (!ok) throw new InvalidOperationException(message); }
    static IEnumerable<TypeDefinition> Types(TypeDefinition t) { yield return t; foreach (var n in t.NestedTypes) foreach (var x in Types(n)) yield return x; }
    static int Main(string[] args)
    {
        try
        {
            if (args.Length == 4 && args[0] == "--declarations") { Declarations.Write(args[1],args[2],args[3]); return 0; }
            if (args.Length == 4 && args[0] == "--raw") { RawMaterialize.Write(args[1],args[2],args[3]); return 0; }
            if (args.Length == 2 && args[0] == "--self-test") { SelfTest(args[1]); return 0; }
            Require(args.Length == 4, "usage: input.dll output.dll fixed-edits.json report.json | --self-test report.json");
            var input = Path.GetFullPath(args[0]); var output = Path.GetFullPath(args[1]);
            Require(input != output && !File.Exists(output), "Output must be new and distinct from input");
            Require(Hash(input) == InputHash, "Pinned input hash mismatch");
            Require(Hash(args[2]) == EditsHash, "Pinned six-site manifest hash mismatch");
            var support = Environment.GetEnvironmentVariable("GODSPVZ_RESOLVER");
            Require(!string.IsNullOrEmpty(support) && Directory.Exists(support), "Explicit locked support directory required");
            var resolver = new DefaultAssemblyResolver();
            foreach (var dir in resolver.GetSearchDirectories()) resolver.RemoveSearchDirectory(dir);
            resolver.AddSearchDirectory(support!);
            using var asm = AssemblyDefinition.ReadAssembly(input, new ReaderParameters { AssemblyResolver = resolver, InMemory = true, ReadSymbols = false });
            var module = asm.MainModule;
            var methods = module.Types.SelectMany(Types).SelectMany(t => t.Methods).ToDictionary(m => m.MetadataToken.ToUInt32());
            Require(methods.Count == 2317, "MethodDef invariant mismatch");
            using var edits = JsonDocument.Parse(File.ReadAllText(args[2]));
            var records = new List<object>();
            foreach (var e in edits.RootElement.GetProperty("edits").EnumerateArray())
            {
                var tok = Convert.ToUInt32(e.GetProperty("token").GetString()![2..], 16);
                var offset = e.GetProperty("offset").GetInt32();
                var action = e.GetProperty("action").GetString()!;
                var m = methods[tok]; var body = m.Body;
                var ins = body.Instructions.Single(i => i.Offset == offset);
                var before = ins.ToString();
                CheckTargets(body);
                switch (action)
                {
                    case "to_ldnull":
                        Require(tok == 0x06000339 && (offset == 6 || offset == 58), "Null action outside pinned native sites");
                        ToNull(ins);
                        break;
                    case "ret_to_throw":
                        Require(tok == 0x06000339 && offset == 161 && m.ReturnType.FullName == "System.Boolean", "Throw action outside native exception path");
                        var load = ins.Previous; var store = load?.Previous; var create = store?.Previous;
                        Require(load?.OpCode == OpCodes.Ldloc && store?.OpCode == OpCodes.Stloc && load.Operand is VariableDefinition lv && store.Operand is VariableDefinition sv && ReferenceEquals(lv,sv) && lv.Index == 5 && create?.OpCode == OpCodes.Newobj && create.Operand is MethodReference ctor && ctor.DeclaringType.FullName == "System.NullReferenceException", "Expected new exception/store/load native null path absent");
                        ToThrow(ins);
                        break;
                    case "retype_call":
                        Require((tok == 0x06000348 && (offset == 7 || offset == 35)) || (tok == 0x0600034C && offset == 225), "Call action outside native GameObject receivers");
                        Retype(module, ins, e.GetProperty("newDeclaringType").GetString()!);
                        break;
                    default: throw new InvalidOperationException("Unknown action");
                }
                CheckTargets(body);
                records.Add(new { token = $"0x{tok:X8}", offset, action, before, after = ins.ToString() });
            }
            Require(records.Count == 6, "Expected six edits");
            Directory.CreateDirectory(Path.GetDirectoryName(output)!);
            asm.Write(output);
            using var reopened = AssemblyDefinition.ReadAssembly(output);
            foreach (var m in reopened.MainModule.Types.SelectMany(Types).SelectMany(t => t.Methods).Where(m => m.HasBody)) CheckTargets(m.Body);
            var result = new { scope = "six native-supported sites only; not full game qualification", input = Hash(input), edits = Hash(args[2]), output = Hash(output), instructionIdentityPreserved = true, reopenedTargetsValid = true, records };
            File.WriteAllText(args[3], JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
            Console.WriteLine(JsonSerializer.Serialize(new { output = Hash(output), sites = records.Count, targets = 3 }));
            return 0;
        }
        catch (Exception e) { Console.Error.WriteLine("REJECT: " + e.Message); return 3; }
    }
    internal static void ToNull(Instruction ins)
    {
        // Require the exact original encoding, not a generic stack-producing instruction.
        Require(ins.OpCode == OpCodes.Ldc_I4 && ins.Operand is int value && value == 0, "Expected pinned ldc.i4 0 encoding");
        ins.OpCode = OpCodes.Ldnull;
        ins.Operand = null;
    }
    internal static void ToThrow(Instruction ins)
    {
        Require(ins.OpCode == OpCodes.Ret && ins.Operand == null, "Expected ret with no operand");
        ins.OpCode = OpCodes.Throw;
        ins.Operand = null;
    }
    static string Scope(TypeReference t) => t.Scope switch { AssemblyNameReference a => a.FullName, ModuleDefinition m => m.Assembly.Name.FullName, _ => t.Scope?.ToString() ?? "" };
    static string Signature(TypeReference t) => t switch {
        GenericParameter g => (g.Type == GenericParameterType.Method ? "!!" : "!") + g.Position,
        GenericInstanceType g => Signature(g.ElementType) + "<" + string.Join(",", g.GenericArguments.Select(Signature)) + ">",
        ByReferenceType b => Signature(b.ElementType) + "&", PointerType b => Signature(b.ElementType) + "*",
        ArrayType b => Signature(b.ElementType) + "[" + new string(',', b.Rank - 1) + "]", _ => t.FullName + " @" + Scope(t)
    };
    internal static bool SameShape(MethodReference a, MethodReference b)
    {
        a = a is GenericInstanceMethod left ? left.ElementMethod : a;
        var element = b is GenericInstanceMethod g ? g.ElementMethod : b;
        return a.Name == element.Name && a.HasThis == element.HasThis && a.ExplicitThis == element.ExplicitThis &&
            a.CallingConvention == element.CallingConvention && a.GenericParameters.Count == element.GenericParameters.Count &&
            Signature(a.ReturnType) == Signature(element.ReturnType) && a.Parameters.Count == element.Parameters.Count &&
            a.Parameters.Select(p => Signature(p.ParameterType)).SequenceEqual(element.Parameters.Select(p => Signature(p.ParameterType)));
    }
    static void Retype(ModuleDefinition module, Instruction ins, string type)
    {
        Require(ins.OpCode == OpCodes.Callvirt && ins.Operand is MethodReference, "Expected instance callvirt");
        var old = (MethodReference)ins.Operand;
        Require(type == "UnityEngine.GameObject" && old.DeclaringType.FullName == "UnityEngine.Component", "Expected Component-to-GameObject native receiver correction");
        Require(old.Name is "GetComponent" or "get_transform", "Unexpected called method");
        var owner = old.DeclaringType.Resolve().Module.GetType(type);
        Require(owner != null, "Target type missing in locked Unity assembly");
        var found = owner!.Methods.Where(c => SameShape(c, old)).ToList();
        Require(found.Count == 1, "Target full signature ambiguous or mismatched");
        MethodReference target = module.ImportReference(found[0]);
        if (old is GenericInstanceMethod gi)
        {
            Require(gi.GenericArguments.Count == target.GenericParameters.Count, "Generic arity mismatch");
            var imported = new GenericInstanceMethod(target);
            foreach (var a in gi.GenericArguments) imported.GenericArguments.Add(module.ImportReference(a));
            target = imported;
        }
        Require(SameShape(target, old) && SameInstantiation(target,old), "Imported signature or instantiation changed");
        ins.Operand = target;
    }
    internal static bool SameInstantiation(MethodReference a, MethodReference b)
    {
        if (a is GenericInstanceMethod ga && b is GenericInstanceMethod gb) return ga.GenericArguments.Select(Signature).SequenceEqual(gb.GenericArguments.Select(Signature));
        return a is not GenericInstanceMethod && b is not GenericInstanceMethod;
    }
    static void CheckTargets(CM body)
    {
        var set = body.Instructions.ToHashSet();
        void Check(Instruction? ins) => Require(ins == null || set.Contains(ins), "Dangling instruction target");
        foreach (var i in body.Instructions)
        {
            if (i.Operand is Instruction one) Check(one);
            if (i.Operand is Instruction[] all) foreach (var x in all) { Require(x != null, "Null switch entry"); Check(x); }
        }
        foreach (var h in body.ExceptionHandlers) { Check(h.TryStart); Check(h.TryEnd); Check(h.HandlerStart); Check(h.HandlerEnd); Check(h.FilterStart); }
    }
    static void SelfTest(string output)
    {
        var results = new List<object>();
        void Pass(string name, bool condition) { Require(condition, name); results.Add(new { name, passed = true }); }
        void Reject(string name, Action f) { bool rejected = false; try { f(); } catch (InvalidOperationException) { rejected = true; } Pass(name, rejected); }
        var z = Instruction.Create(OpCodes.Ldc_I4,0); ToNull(z); Pass("zero positive", z.OpCode == OpCodes.Ldnull);
        Reject("ret cannot become null", () => ToNull(Instruction.Create(OpCodes.Ret)));
        Reject("integer one cannot become null", () => ToNull(Instruction.Create(OpCodes.Ldc_I4_1)));
        Reject("wrong operand cannot become null", () => ToNull(Instruction.Create(OpCodes.Ldc_I4,1)));
        Reject("different zero encoding rejected", () => ToNull(Instruction.Create(OpCodes.Ldc_I4_0)));
        Reject("non-ret cannot throw", () => ToThrow(Instruction.Create(OpCodes.Ldnull)));
        using var a = AssemblyDefinition.CreateAssembly(new AssemblyNameDefinition("TargetProbe", new Version(1,0)), "TargetProbe", ModuleKind.Dll);
        var mod = a.MainModule; var t = new TypeDefinition("", "T", TypeAttributes.Public, mod.TypeSystem.Object); mod.Types.Add(t);
        var m = new MethodDefinition("M", MethodAttributes.Public | MethodAttributes.Static, mod.TypeSystem.Void); t.Methods.Add(m);
        var target = Instruction.Create(OpCodes.Ldc_I4,0); var branch = Instruction.Create(OpCodes.Br, target); var sw = Instruction.Create(OpCodes.Switch, new[] { target, target });
        foreach (var x in new[] { branch, sw, target, Instruction.Create(OpCodes.Pop), Instruction.Create(OpCodes.Ret) }) m.Body.Instructions.Add(x);
        // Deliberate structural fixture for all EH reference slots; not executable EH or behavior proof.
        m.Body.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Filter) { TryStart=target, TryEnd=target, HandlerStart=target, HandlerEnd=target, FilterStart=target });
        ToNull(target); CheckTargets(m.Body);
        Pass("single branch identity", ReferenceEquals(branch.Operand, target));
        Pass("all switch entries identity", ((Instruction[])sw.Operand).All(x => ReferenceEquals(x,target)));
        var eh = m.Body.ExceptionHandlers[0]; Pass("all five EH slots identity", new[] {eh.TryStart,eh.TryEnd,eh.HandlerStart,eh.HandlerEnd,eh.FilterStart}.All(x => ReferenceEquals(x,target)));
        m.Body.ExceptionHandlers.Clear(); a.Write(Path.ChangeExtension(output,"dll"));
        using var reopened = AssemblyDefinition.ReadAssembly(Path.ChangeExtension(output,"dll")); var rb = reopened.MainModule.GetType("T").Methods[0].Body; CheckTargets(rb);
        var rt = rb.Instructions.Single(x => x.OpCode == OpCodes.Ldnull); Pass("reopened switch and branch", ReferenceEquals(rb.Instructions[0].Operand,rt) && ((Instruction[])rb.Instructions[1].Operand).All(x=>ReferenceEquals(x,rt)));
        var good = new MethodReference("F", mod.TypeSystem.Int32, t) { HasThis = true };
        MethodReference Clone() => new("F", mod.TypeSystem.Int32, t) { HasThis = true };
        Pass("signature positive", SameShape(good,Clone()));
        var changed = Clone(); changed.ReturnType = mod.TypeSystem.String; Pass("return signature negative", !SameShape(good,changed));
        changed=Clone(); changed.ExplicitThis=true; Pass("ExplicitThis negative", !SameShape(good,changed));
        changed=Clone(); changed.HasThis=false; Pass("HasThis negative", !SameShape(good,changed));
        changed=Clone(); changed.CallingConvention=MethodCallingConvention.VarArg; Pass("calling convention negative", !SameShape(good,changed));
        changed=Clone(); changed.Parameters.Add(new ParameterDefinition(mod.TypeSystem.Int32)); Pass("parameter negative", !SameShape(good,changed));
        changed=Clone(); changed.GenericParameters.Add(new GenericParameter(changed)); Pass("generic arity negative", !SameShape(good,changed));
        var generic = Clone(); generic.CallingConvention=MethodCallingConvention.Generic; generic.GenericParameters.Add(new GenericParameter(generic)); generic.ReturnType=generic.GenericParameters[0];
        var ga=new GenericInstanceMethod(generic);ga.GenericArguments.Add(mod.TypeSystem.String);
        var gb=new GenericInstanceMethod(generic);gb.GenericArguments.Add(mod.TypeSystem.String);
        Pass("generic instance signature positive", SameShape(ga,gb) && SameInstantiation(ga,gb));
        gb.GenericArguments[0]=mod.TypeSystem.Int32;Pass("generic instance argument negative", !SameInstantiation(ga,gb));
        File.WriteAllText(output,JsonSerializer.Serialize(new { passed=results.Count, scope="patcher structural/signature controls; not game behavior",results },new JsonSerializerOptions {WriteIndented=true}));
        Console.WriteLine($"PATCHER_CONTROLS_PASS {results.Count}");
    }
}
