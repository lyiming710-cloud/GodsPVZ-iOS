using System.Security.Cryptography;
using Mono.Cecil;
using Mono.Cecil.Cil;

static class Stage9MemberRefAudit
{
    private sealed record Target(uint Token, string Family, int ExpectedUses);

    private static readonly Target[] Targets =
    {
        new(0x0A0000B4, "GetEnumerator", 11), new(0x0A0000B5, "get_Current", 11),
        new(0x0A0000C3, "get_Current", 0),  new(0x0A0000C9, "GetEnumerator", 5),
        new(0x0A0000CA, "get_Current", 5),  new(0x0A0000D0, "GetEnumerator", 3),
        new(0x0A0000D1, "get_Current", 3),  new(0x0A0000D6, "GetEnumerator", 9),
        new(0x0A0000D7, "get_Current", 9),  new(0x0A0000E4, "GetEnumerator", 1),
        new(0x0A0000E5, "get_Current", 1),  new(0x0A0000E7, "get_Current", 15),
        new(0x0A0000FB, "GetEnumerator", 2), new(0x0A0000FC, "get_Current", 2),
        new(0x0A000140, "Insert", 2),        new(0x0A000141, "GetEnumerator", 1),
        new(0x0A000142, "get_Current", 1),  new(0x0A000149, "get_transform", 2),
        new(0x0A000162, "GetEnumerator", 1), new(0x0A000163, "get_Current", 1),
        new(0x0A00021D, "GetEnumerator", 1), new(0x0A00021E, "get_Current", 1),
        new(0x0A00022B, "Insert", 1),        new(0x0A000284, "get_transform", 1),
    };

    private static readonly Dictionary<string, string> KnownHashes = new(StringComparer.OrdinalIgnoreCase)
    {
        ["f47b1b37f1d4d3a1ddeb44bb6dbe399c58dc6dee3d4602618b2790f839d3e321"] = "run8-pre-B001",
        ["f144dd624d2b0c05b5ff01fc12ff455ac394c87852a4fc3c87e8c3611582a433"] = "B001-only",
        ["f01cbf6cc768b48388d8f37fe665914e8f5954902a1f0328357965d762c24bc5"] = "GetEnumerator-family-candidate",
        ["b07254d5c4077e013bbc934734f7b0949c0942a4e667d6d2b23d2af6c830f720"] = "all-24-static-candidate",
    };

    private static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
    {
        foreach (var t in roots)
        {
            yield return t;
            foreach (var n in AllTypes(t.NestedTypes)) yield return n;
        }
    }

    private static bool Orphan(TypeReference? t)
    {
        if (t is null) return false;
        if (t is GenericParameter gp) return gp.Owner is null;
        if (t is GenericInstanceType gi)
            return Orphan(gi.ElementType) || gi.GenericArguments.Any(Orphan);
        if (t is TypeSpecification ts) return Orphan(ts.ElementType);
        return false;
    }

    private static bool Orphan(MethodReference m) =>
        Orphan(m.DeclaringType) || Orphan(m.ReturnType) || m.Parameters.Any(p => Orphan(p.ParameterType));

    private static string MethodSig(MethodReference m) =>
        $"{m.ReturnType.FullName} {m.DeclaringType.FullName}::{m.Name}({string.Join(",", m.Parameters.Select(p => p.ParameterType.FullName))})";

    private static TypeReference? GenericArg0(TypeReference t) =>
        t is GenericInstanceType gi && gi.GenericArguments.Count >= 1 ? gi.GenericArguments[0] : null;

    private static bool IsTypeVar0(TypeReference t) =>
        t is GenericParameter gp && gp.Type == GenericParameterType.Type && gp.Position == 0;

    private static bool Var0OrEquivalent(TypeReference actual, TypeReference? contextArg) =>
        IsTypeVar0(actual) ||
        (contextArg is not null && (actual.FullName == contextArg.FullName ||
                                    (IsTypeVar0(contextArg) && IsTypeVar0(actual))));

    // Mono.Cecil may expose the same canonical MemberRef in either definition-level
    // form (!0) or context-inflated closed form (Buff, Device, Plant, ...).  Both are
    // semantically acceptable here.  The raw candidate builder separately locks the
    // underlying signature blob to the canonical VAR !0 encoding.
    private static bool StructuralMatch(MethodReference m, string family)
    {
        var declaringArg = GenericArg0(m.DeclaringType);
        return family switch
        {
            "GetEnumerator" =>
                m.Name == "GetEnumerator" && m.Parameters.Count == 0 &&
                m.ReturnType.FullName.Contains("System.Collections.Generic.List`1/Enumerator", StringComparison.Ordinal) &&
                (GenericArg0(m.ReturnType) is TypeReference retArg
                    ? Var0OrEquivalent(retArg, declaringArg)
                    : m.ReturnType.FullName.Contains("!0", StringComparison.Ordinal)),

            "get_Current" =>
                m.Name == "get_Current" && m.Parameters.Count == 0 &&
                m.DeclaringType.FullName.Contains("System.Collections.Generic.List`1/Enumerator", StringComparison.Ordinal) &&
                Var0OrEquivalent(m.ReturnType, declaringArg),

            "Insert" =>
                m.Name == "Insert" && m.Parameters.Count == 2 &&
                m.DeclaringType.FullName.Contains("System.Collections.Generic.List`1", StringComparison.Ordinal) &&
                m.Parameters[0].ParameterType.MetadataType == MetadataType.Int32 &&
                Var0OrEquivalent(m.Parameters[1].ParameterType, declaringArg),

            "get_transform" =>
                m.Name == "get_transform" && m.Parameters.Count == 0 &&
                m.ReturnType.FullName == "UnityEngine.Transform",

            _ => false,
        };
    }

    public static int Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.Error.WriteLine("Usage: GodsPVZ.Stage9MemberRefAudit <Assembly-CSharp.dll> [--require-resolve] [resolver-dir ...]");
            return 2;
        }

        var path = Path.GetFullPath(args[0]);
        var requireResolve = args.Skip(1).Any(a => a == "--require-resolve");
        var resolverDirs = args.Skip(1).Where(a => a != "--require-resolve").Select(Path.GetFullPath).Distinct().ToList();
        if (!File.Exists(path)) throw new FileNotFoundException(path);

        var bytes = File.ReadAllBytes(path);
        var sha = Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
        KnownHashes.TryGetValue(sha, out var label);
        Console.WriteLine($"SHA256={sha} LABEL={label ?? "unknown"} SIZE={bytes.Length}");

        using var resolver = new DefaultAssemblyResolver();
        resolver.AddSearchDirectory(Path.GetDirectoryName(path)!);
        foreach (var d in resolverDirs)
        {
            if (!Directory.Exists(d)) throw new DirectoryNotFoundException(d);
            resolver.AddSearchDirectory(d);
            Console.WriteLine("RESOLVER_DIR=" + d);
        }

        using var asm = AssemblyDefinition.ReadAssembly(path, new ReaderParameters
        {
            InMemory = true,
            ReadSymbols = false,
            AssemblyResolver = resolver,
        });
        var module = asm.MainModule;
        var types = AllTypes(module.Types).ToList();
        var methods = types.SelectMany(t => t.Methods).ToList();
        var methodDefs = methods.Count;
        Console.WriteLine($"MODULE_KIND={module.Kind} METHODDEFS={methodDefs} TYPES={types.Count}");

        var failures = new List<string>();
        if (module.Kind != ModuleKind.Dll) failures.Add("module kind is not Dll");
        if (methodDefs != 2317) failures.Add($"MethodDef drift: {methodDefs}");

        var refs = module.GetMemberReferences().ToDictionary(r => r.MetadataToken.ToUInt32());
        var useCounts = Targets.ToDictionary(t => t.Token, _ => 0);
        var badTransformUses = new List<string>();

        foreach (var m in methods.Where(m => m.HasBody))
        {
            foreach (var ins in m.Body.Instructions)
            {
                if (ins.Operand is not IMetadataTokenProvider op) continue;
                var token = op.MetadataToken.ToUInt32();
                if (!useCounts.ContainsKey(token)) continue;
                useCounts[token]++;
                if ((token == 0x0A000149 || token == 0x0A000284) && ins.OpCode.Code != Code.Callvirt)
                    badTransformUses.Add($"{m.FullName} IL_{ins.Offset:x4} token=0x{token:x8} opcode={ins.OpCode.Code}");
            }
        }

        var resolveOk = 0;
        var resolveDependencyMissing = 0;
        var resolveHardFail = 0;

        foreach (var t in Targets)
        {
            if (!refs.TryGetValue(t.Token, out var raw))
            {
                failures.Add($"missing MemberRef 0x{t.Token:x8}");
                continue;
            }
            if (raw is not MethodReference mr)
            {
                failures.Add($"0x{t.Token:x8} remains {raw.GetType().Name}, expected MethodReference");
                Console.WriteLine($"TARGET 0x{t.Token:x8} family={t.Family} kind={raw.GetType().Name} uses={useCounts[t.Token]}");
                continue;
            }

            var structural = StructuralMatch(mr, t.Family);
            var orphan = Orphan(mr);
            var uses = useCounts[t.Token];
            Console.WriteLine($"TARGET 0x{t.Token:x8} family={t.Family} structural={structural} orphan={orphan} uses={uses}/{t.ExpectedUses} sig={MethodSig(mr)}");
            if (!structural) failures.Add($"0x{t.Token:x8} structural signature mismatch: {MethodSig(mr)}");
            if (orphan) failures.Add($"0x{t.Token:x8} contains ownerless generic parameter");
            if (uses != t.ExpectedUses) failures.Add($"0x{t.Token:x8} IL use count {uses}, expected {t.ExpectedUses}");

            try
            {
                var resolved = mr.Resolve();
                if (resolved is null)
                {
                    resolveHardFail++;
                    failures.Add($"0x{t.Token:x8} Resolve() returned null");
                    Console.WriteLine($"RESOLVE 0x{t.Token:x8} NULL");
                }
                else
                {
                    resolveOk++;
                    Console.WriteLine($"RESOLVE 0x{t.Token:x8} OK -> {resolved.FullName}");
                }
            }
            catch (AssemblyResolutionException ex)
            {
                resolveDependencyMissing++;
                Console.WriteLine($"RESOLVE 0x{t.Token:x8} DEPENDENCY_MISSING {ex.Message}");
                if (requireResolve) failures.Add($"0x{t.Token:x8} Resolve() dependency missing: {ex.Message}");
            }
            catch (Exception ex)
            {
                resolveHardFail++;
                failures.Add($"0x{t.Token:x8} Resolve() failed: {ex.GetType().Name}: {ex.Message}");
                Console.WriteLine($"RESOLVE 0x{t.Token:x8} FAIL {ex.GetType().Name}: {ex.Message}");
            }
        }

        foreach (var bad in badTransformUses) failures.Add("transform use is not callvirt: " + bad);

        var orphanHits = new List<string>();
        foreach (var m in methods)
        {
            if (Orphan(m.ReturnType)) orphanHits.Add($"method-return {m.FullName}");
            foreach (var p in m.Parameters)
                if (Orphan(p.ParameterType)) orphanHits.Add($"method-param {m.FullName}::{p.Name}");
            if (m.HasBody)
                for (var i = 0; i < m.Body.Variables.Count; i++)
                    if (Orphan(m.Body.Variables[i].VariableType)) orphanHits.Add($"local {m.FullName} [{i}] {m.Body.Variables[i].VariableType.FullName}");
        }
        foreach (var r in refs.Values.OfType<MethodReference>())
            if (Orphan(r)) orphanHits.Add($"memberref 0x{r.MetadataToken.ToUInt32():x8} {MethodSig(r)}");

        Console.WriteLine($"ORPHAN_GENERIC_HITS={orphanHits.Count}");
        foreach (var hit in orphanHits.Take(50)) Console.WriteLine("ORPHAN " + hit);
        if (orphanHits.Count != 0) failures.Add($"ownerless generic hits remain: {orphanHits.Count}");

        Console.WriteLine($"RESOLVE_SUMMARY ok={resolveOk} dependency_missing={resolveDependencyMissing} hard_fail={resolveHardFail} require_resolve={requireResolve}");
        if (requireResolve && resolveOk != Targets.Length)
            failures.Add($"require-resolve expected {Targets.Length} successes, got {resolveOk}");

        if (failures.Count != 0)
        {
            Console.WriteLine($"STAGE9_MEMBERREF_AUDIT_FAIL count={failures.Count}");
            foreach (var f in failures) Console.WriteLine("FAIL " + f);
            return 1;
        }

        Console.WriteLine("STAGE9_MEMBERREF_AUDIT_OK");
        return 0;
    }
}
