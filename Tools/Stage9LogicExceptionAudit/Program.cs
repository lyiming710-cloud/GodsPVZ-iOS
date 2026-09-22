using System.Security.Cryptography;
using Mono.Cecil;
using Mono.Cecil.Cil;

static class Stage9LogicExceptionAudit
{
    private sealed record Target(
        uint Token,
        string Id,
        string ExpectedReturn,
        string ExceptionType,
        int NewobjOffset,
        int StlocOffset,
        int LdlocOffset,
        int RetOffset,
        string Priority);

    private static readonly Target[] Targets =
    {
        new(0x0600008C, "L001", "Grid", "System.NullReferenceException", 0x021F, 0x0224, 0x0228, 0x022C, "P0_CORE_GRID"),
        new(0x060000D4, "L002", "System.Boolean", "System.NullReferenceException", 0x0189, 0x018E, 0x0192, 0x0196, "P0_CORE_COMBAT_RANGE"),
        new(0x06000903, "L156", "System.String", "System.IndexOutOfRangeException", 0x00AE, 0x00B3, 0x00B7, 0x00BB, "P2_DEMO_CODE"),
    };

    private static readonly Dictionary<string,string> KnownHashes = new(StringComparer.OrdinalIgnoreCase)
    {
        ["047054e0db594b6e3385fe4d2555c5932dcd4c28e4b2cb6fb39acbc4336d43ee"] = "formal-runtime",
        ["f47b1b37f1d4d3a1ddeb44bb6dbe399c58dc6dee3d4602618b2790f839d3e321"] = "run8-workcopy",
        ["f144dd624d2b0c05b5ff01fc12ff455ac394c87852a4fc3c87e8c3611582a433"] = "b001-workcopy",
        ["f01cbf6cc768b48388d8f37fe665914e8f5954902a1f0328357965d762c24bc5"] = "getenumerator-family-candidate",
        ["b07254d5c4077e013bbc934734f7b0949c0942a4e667d6d2b23d2af6c830f720"] = "all24-static-candidate",
    };

    private static int? LocalIndex(Instruction ins, bool store)
    {
        switch (ins.OpCode.Code)
        {
            case Code.Stloc_0 when store: return 0;
            case Code.Stloc_1 when store: return 1;
            case Code.Stloc_2 when store: return 2;
            case Code.Stloc_3 when store: return 3;
            case Code.Ldloc_0 when !store: return 0;
            case Code.Ldloc_1 when !store: return 1;
            case Code.Ldloc_2 when !store: return 2;
            case Code.Ldloc_3 when !store: return 3;
            case Code.Stloc when store:
            case Code.Stloc_S when store:
            case Code.Ldloc when !store:
            case Code.Ldloc_S when !store:
                return ins.Operand is VariableDefinition v ? v.Index : null;
            default:
                return null;
        }
    }

    private static IEnumerable<Instruction> BranchTargets(Instruction ins)
    {
        if (ins.Operand is Instruction one)
            yield return one;
        else if (ins.Operand is Instruction[] many)
            foreach (var t in many) yield return t;
    }

    private static IEnumerable<(string Kind, Instruction? Boundary)> EhBoundaries(MethodBody body)
    {
        foreach (var eh in body.ExceptionHandlers)
        {
            yield return ("TryStart", eh.TryStart);
            yield return ("TryEnd", eh.TryEnd);
            yield return ("HandlerStart", eh.HandlerStart);
            yield return ("HandlerEnd", eh.HandlerEnd);
            yield return ("FilterStart", eh.FilterStart);
        }
    }

    private static string Sha256(string path) =>
        Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path))).ToLowerInvariant();

    public static int Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.Error.WriteLine("Usage: GodsPVZ.Stage9LogicExceptionAudit <Assembly-CSharp.dll>");
            return 2;
        }

        var path = Path.GetFullPath(args[0]);
        if (!File.Exists(path)) throw new FileNotFoundException(path);
        var sha = Sha256(path);
        KnownHashes.TryGetValue(sha, out var label);
        Console.WriteLine($"SHA256={sha} LABEL={label ?? "unknown"}");
        if (label is null)
        {
            Console.WriteLine("STAGE9_EXCEPTION_PATH_AUDIT_FAIL unknown_input_hash");
            return 3;
        }

        using var asm = AssemblyDefinition.ReadAssembly(path, new ReaderParameters
        {
            InMemory = true,
            ReadSymbols = false,
        });
        var module = asm.MainModule;
        var failures = new List<string>();

        foreach (var t in Targets)
        {
            var provider = module.LookupToken((int)t.Token);
            if (provider is not MethodDefinition method)
            {
                failures.Add($"{t.Id}: token 0x{t.Token:X8} is not a MethodDefinition");
                continue;
            }
            if (!method.HasBody)
            {
                failures.Add($"{t.Id}: method has no body");
                continue;
            }
            if (method.ReturnType.FullName != t.ExpectedReturn)
                failures.Add($"{t.Id}: return type {method.ReturnType.FullName} != {t.ExpectedReturn}");

            var ins = method.Body.Instructions;
            var i0 = ins.FirstOrDefault(i => i.Offset == t.NewobjOffset);
            var i1 = ins.FirstOrDefault(i => i.Offset == t.StlocOffset);
            var i2 = ins.FirstOrDefault(i => i.Offset == t.LdlocOffset);
            var i3 = ins.FirstOrDefault(i => i.Offset == t.RetOffset);
            if (i0 is null || i1 is null || i2 is null || i3 is null)
            {
                failures.Add($"{t.Id}: expected IL offsets not found");
                continue;
            }

            var p0 = ins.IndexOf(i0);
            var p1 = ins.IndexOf(i1);
            var p2 = ins.IndexOf(i2);
            var p3 = ins.IndexOf(i3);
            var consecutive = p1 == p0 + 1 && p2 == p1 + 1 && p3 == p2 + 1;
            if (!consecutive)
                failures.Add($"{t.Id}: target instructions are not consecutive");

            var ctorOk = i0.OpCode.Code == Code.Newobj &&
                         i0.Operand is MethodReference ctor &&
                         ctor.Name == ".ctor" && ctor.Parameters.Count == 0 &&
                         ctor.DeclaringType.FullName == t.ExceptionType;
            if (!ctorOk)
                failures.Add($"{t.Id}: newobj is not parameterless {t.ExceptionType}::.ctor()");

            var storeLocal = LocalIndex(i1, true);
            var loadLocal = LocalIndex(i2, false);
            if (storeLocal is null || loadLocal is null || storeLocal != loadLocal)
                failures.Add($"{t.Id}: stloc/ldloc local mismatch store={storeLocal?.ToString() ?? "?"} load={loadLocal?.ToString() ?? "?"}");
            if (i3.OpCode.Code != Code.Ret)
                failures.Add($"{t.Id}: trailing opcode is {i3.OpCode.Code}, expected ret");

            var sequence = new HashSet<Instruction> { i0, i1, i2, i3 };
            var incomingStloc = new List<string>();
            var incomingLdloc = new List<string>();
            var incomingRet = new List<string>();
            foreach (var src in ins)
            {
                if (sequence.Contains(src)) continue;
                foreach (var dst in BranchTargets(src))
                {
                    var edge = $"IL_{src.Offset:X4}->{dst.Offset:X4}";
                    if (ReferenceEquals(dst, i1)) incomingStloc.Add(edge);
                    if (ReferenceEquals(dst, i2)) incomingLdloc.Add(edge);
                    if (ReferenceEquals(dst, i3)) incomingRet.Add(edge);
                }
            }
            if (incomingStloc.Count != 0)
                failures.Add($"{t.Id}: external branch enters stloc: {string.Join(",", incomingStloc)}");
            if (incomingLdloc.Count != 0)
                failures.Add($"{t.Id}: external branch enters ldloc: {string.Join(",", incomingLdloc)}");

            var dangerousEh = EhBoundaries(method.Body)
                .Where(x => ReferenceEquals(x.Boundary, i1) || ReferenceEquals(x.Boundary, i2))
                .Select(x => x.Kind + "@IL_" + x.Boundary!.Offset.ToString("X4"))
                .ToList();
            if (dangerousEh.Count != 0)
                failures.Add($"{t.Id}: EH boundary on rewrite instruction: {string.Join(",", dangerousEh)}");

            var allEh = EhBoundaries(method.Body)
                .Where(x => x.Boundary is not null && sequence.Contains(x.Boundary))
                .Select(x => x.Kind + "@IL_" + x.Boundary!.Offset.ToString("X4"))
                .ToList();

            Console.WriteLine(
                $"TARGET {t.Id} token=0x{t.Token:X8} priority={t.Priority} method={method.FullName} " +
                $"pattern={(consecutive && ctorOk && storeLocal is not null && storeLocal == loadLocal && i3.OpCode.Code == Code.Ret)} " +
                $"local={storeLocal?.ToString() ?? "?"} external_stloc={incomingStloc.Count} external_ldloc={incomingLdloc.Count} " +
                $"external_ret={incomingRet.Count} eh_in_sequence={allEh.Count}");
            foreach (var edge in incomingRet)
                Console.WriteLine($"RET_INBOUND {t.Id} {edge}");
            foreach (var boundary in allEh)
                Console.WriteLine($"EH_BOUNDARY {t.Id} {boundary}");
        }

        if (failures.Count != 0)
        {
            Console.WriteLine($"STAGE9_EXCEPTION_PATH_AUDIT_FAIL count={failures.Count}");
            foreach (var f in failures) Console.WriteLine("FAIL " + f);
            return 1;
        }

        Console.WriteLine("STAGE9_EXCEPTION_PATH_AUDIT_READY");
        Console.WriteLine("WRITE_STATUS=NOT_AUTHORIZED_AUDIT_ONLY");
        return 0;
    }
}
