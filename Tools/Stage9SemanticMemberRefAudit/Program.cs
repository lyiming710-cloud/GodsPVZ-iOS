using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

static class Stage9SemanticMemberRefAudit
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

    private static readonly HashSet<uint> AllowedChangedMethods = new()
    {
        0x0600017F, // DeviceManager.Start_BoardEntry
        0x060001DE, // MouseManager.GetPlantUnderMouse
    };

    private static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
    {
        foreach (var t in roots)
        {
            yield return t;
            foreach (var n in AllTypes(t.NestedTypes)) yield return n;
        }
    }

    private static string Sha(string path) =>
        Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path))).ToLowerInvariant();

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

    private static bool Orphan(FieldReference f) => Orphan(f.DeclaringType) || Orphan(f.FieldType);

    private static string MethodSig(MethodReference m) =>
        $"{m.ReturnType.FullName} {m.DeclaringType.FullName}::{m.Name}({string.Join(",", m.Parameters.Select(p => p.ParameterType.FullName))})";

    private static string FieldSig(FieldReference f) =>
        $"{f.FieldType.FullName} {f.DeclaringType.FullName}::{f.Name}";

    private static TypeReference? GenericArg0(TypeReference t) =>
        t is GenericInstanceType gi && gi.GenericArguments.Count >= 1 ? gi.GenericArguments[0] : null;

    private static bool IsTypeVar0(TypeReference t) =>
        t is GenericParameter gp && gp.Type == GenericParameterType.Type && gp.Position == 0;

    private static bool Var0OrEquivalent(TypeReference actual, TypeReference? contextArg) =>
        IsTypeVar0(actual) ||
        (contextArg is not null && (actual.FullName == contextArg.FullName ||
                                    (IsTypeVar0(contextArg) && IsTypeVar0(actual))));

    private static bool StructuralMatch(MethodReference m, string family)
    {
        var declaringArg = GenericArg0(m.DeclaringType);
        return family switch
        {
            "GetEnumerator" =>
                m.Name == "GetEnumerator" && m.Parameters.Count == 0 &&
                m.DeclaringType.FullName.Contains("System.Collections.Generic.List`1", StringComparison.Ordinal) &&
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
                m.ReturnType.FullName == "UnityEngine.Transform" &&
                (m.DeclaringType.FullName == "UnityEngine.Component" || m.DeclaringType.FullName == "UnityEngine.GameObject"),

            _ => false,
        };
    }

    private static string SemanticKey(MethodReference m, string family)
    {
        return family switch
        {
            "GetEnumerator" => $"GetEnumerator|{GenericArg0(m.DeclaringType)?.FullName ?? "<none>"}",
            "get_Current" => $"get_Current|{GenericArg0(m.DeclaringType)?.FullName ?? "<none>"}",
            "Insert" => $"Insert|{GenericArg0(m.DeclaringType)?.FullName ?? "<none>"}",
            "get_transform" => $"get_transform|{m.DeclaringType.FullName}",
            _ => throw new ArgumentOutOfRangeException(nameof(family), family, null),
        };
    }

    private static string? SemanticKeyAny(MethodReference m)
    {
        if (m.Name == "GetEnumerator" && m.DeclaringType.FullName.Contains("System.Collections.Generic.List`1", StringComparison.Ordinal))
            return SemanticKey(m, "GetEnumerator");
        if (m.Name == "get_Current" && m.DeclaringType.FullName.Contains("System.Collections.Generic.List`1/Enumerator", StringComparison.Ordinal))
            return SemanticKey(m, "get_Current");
        if (m.Name == "Insert" && m.DeclaringType.FullName.Contains("System.Collections.Generic.List`1", StringComparison.Ordinal))
            return SemanticKey(m, "Insert");
        if (m.Name == "get_transform" && (m.DeclaringType.FullName == "UnityEngine.Component" || m.DeclaringType.FullName == "UnityEngine.GameObject"))
            return SemanticKey(m, "get_transform");
        return null;
    }

    private static Dictionary<uint, int> UseCounts(ModuleDefinition module)
    {
        var counts = new Dictionary<uint, int>();
        foreach (var m in AllTypes(module.Types).SelectMany(t => t.Methods).Where(m => m.HasBody))
        {
            foreach (var ins in m.Body.Instructions)
            {
                if (ins.Operand is not IMetadataTokenProvider op) continue;
                var tok = op.MetadataToken.ToUInt32();
                if ((tok & 0xFF000000u) != 0x0A000000u) continue;
                counts[tok] = counts.GetValueOrDefault(tok) + 1;
            }
        }
        return counts;
    }

    private static Code NormalizeCode(Code c) => c switch
    {
        Code.Br_S => Code.Br, Code.Brfalse_S => Code.Brfalse, Code.Brtrue_S => Code.Brtrue,
        Code.Beq_S => Code.Beq, Code.Bge_S => Code.Bge, Code.Bgt_S => Code.Bgt,
        Code.Ble_S => Code.Ble, Code.Blt_S => Code.Blt, Code.Bne_Un_S => Code.Bne_Un,
        Code.Bge_Un_S => Code.Bge_Un, Code.Bgt_Un_S => Code.Bgt_Un,
        Code.Ble_Un_S => Code.Ble_Un, Code.Blt_Un_S => Code.Blt_Un,
        Code.Leave_S => Code.Leave,
        _ => c,
    };

    private static string OperandKey(object? operand, MethodDefinition method, Dictionary<Instruction, int> index)
    {
        if (operand is null) return "-";
        return operand switch
        {
            Instruction i => "I:" + index[i],
            Instruction[] a => "SW:" + string.Join(",", a.Select(i => index[i])),
            MethodReference mr => "M:" + MethodSig(mr),
            FieldReference fr => "F:" + FieldSig(fr),
            TypeReference tr => "T:" + tr.FullName,
            ParameterDefinition pd => "P:" + pd.Index + ":" + pd.ParameterType.FullName,
            VariableDefinition vd => "V:" + method.Body.Variables.IndexOf(vd) + ":" + vd.VariableType.FullName,
            string s => "S:" + s,
            float f => "R4:" + f.ToString("R", CultureInfo.InvariantCulture),
            double d => "R8:" + d.ToString("R", CultureInfo.InvariantCulture),
            IFormattable formattable => operand.GetType().Name + ":" + formattable.ToString(null, CultureInfo.InvariantCulture),
            _ => operand.GetType().Name + ":" + operand,
        };
    }

    private static string CanonBody(MethodDefinition m)
    {
        if (!m.HasBody) return "<no-body>";
        var body = m.Body;
        var index = body.Instructions.Select((ins, i) => (ins, i)).ToDictionary(x => x.ins, x => x.i);
        int Idx(Instruction? ins) => ins is null ? -1 : index[ins];
        var sb = new StringBuilder();
        sb.Append("init=").Append(body.InitLocals).Append('|');
        sb.Append("locals=").Append(string.Join(";", body.Variables.Select(v => v.VariableType.FullName))).Append('|');
        foreach (var eh in body.ExceptionHandlers)
        {
            sb.Append("EH:").Append(eh.HandlerType).Append(':').Append(eh.CatchType?.FullName ?? "-")
              .Append(':').Append(Idx(eh.TryStart)).Append('-').Append(Idx(eh.TryEnd))
              .Append(':').Append(Idx(eh.HandlerStart)).Append('-').Append(Idx(eh.HandlerEnd))
              .Append(':').Append(Idx(eh.FilterStart)).Append('|');
        }
        foreach (var ins in body.Instructions)
        {
            sb.Append(NormalizeCode(ins.OpCode.Code)).Append('(')
              .Append(OperandKey(ins.Operand, m, index)).Append(")|");
        }
        return sb.ToString();
    }

    public static int Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.Error.WriteLine("Usage: Stage9SemanticMemberRefAudit <baseline-59bb.dll> <candidate.dll> [--require-resolve] [resolver-dir ...]");
            return 2;
        }

        var baselinePath = Path.GetFullPath(args[0]);
        var candidatePath = Path.GetFullPath(args[1]);
        var requireResolve = args.Skip(2).Any(a => a == "--require-resolve");
        var resolverDirs = args.Skip(2).Where(a => a != "--require-resolve").Select(Path.GetFullPath).Distinct().ToList();
        if (!File.Exists(baselinePath)) throw new FileNotFoundException(baselinePath);
        if (!File.Exists(candidatePath)) throw new FileNotFoundException(candidatePath);

        Console.WriteLine($"BASELINE_SHA256={Sha(baselinePath)}");
        Console.WriteLine($"CANDIDATE_SHA256={Sha(candidatePath)}");

        using var resolver = new DefaultAssemblyResolver();
        resolver.AddSearchDirectory(Path.GetDirectoryName(baselinePath)!);
        resolver.AddSearchDirectory(Path.GetDirectoryName(candidatePath)!);
        foreach (var d in resolverDirs)
        {
            if (!Directory.Exists(d)) throw new DirectoryNotFoundException(d);
            resolver.AddSearchDirectory(d);
            Console.WriteLine("RESOLVER_DIR=" + d);
        }

        using var baselineAsm = AssemblyDefinition.ReadAssembly(baselinePath, new ReaderParameters { InMemory = true, ReadSymbols = false, AssemblyResolver = resolver });
        using var candidateAsm = AssemblyDefinition.ReadAssembly(candidatePath, new ReaderParameters { InMemory = true, ReadSymbols = false, AssemblyResolver = resolver });
        var baseline = baselineAsm.MainModule;
        var candidate = candidateAsm.MainModule;
        var failures = new List<string>();

        var baselineTypes = AllTypes(baseline.Types).ToList();
        var candidateTypes = AllTypes(candidate.Types).ToList();
        var baselineMethods = baselineTypes.SelectMany(t => t.Methods).ToList();
        var candidateMethods = candidateTypes.SelectMany(t => t.Methods).ToList();
        var baselineFields = baselineTypes.SelectMany(t => t.Fields).ToList();
        var candidateFields = candidateTypes.SelectMany(t => t.Fields).ToList();

        Console.WriteLine($"BASELINE module={baseline.Kind} types={baselineTypes.Count} methods={baselineMethods.Count} fields={baselineFields.Count}");
        Console.WriteLine($"CANDIDATE module={candidate.Kind} types={candidateTypes.Count} methods={candidateMethods.Count} fields={candidateFields.Count}");
        if (candidate.Kind != ModuleKind.Dll) failures.Add("candidate module kind is not Dll");
        if (candidateMethods.Count != 2317) failures.Add($"candidate MethodDef drift: {candidateMethods.Count}");
        if (baselineMethods.Count != candidateMethods.Count) failures.Add("MethodDef count differs from baseline");
        if (baselineFields.Count != candidateFields.Count) failures.Add("FieldDef count differs from baseline");
        if (baselineTypes.Count != candidateTypes.Count) failures.Add("TypeDef count differs from baseline");

        var candidateMethodByToken = candidateMethods.ToDictionary(m => m.MetadataToken.ToUInt32());
        var methodIdentityDrift = 0;
        var nonTargetBodyDiffs = 0;
        foreach (var bm in baselineMethods)
        {
            var tok = bm.MetadataToken.ToUInt32();
            if (!candidateMethodByToken.TryGetValue(tok, out var cm) || cm.FullName != bm.FullName)
            {
                methodIdentityDrift++;
                if (methodIdentityDrift <= 20) Console.WriteLine($"METHOD_IDENTITY_DRIFT token=0x{tok:x8} baseline={bm.FullName} candidate={cm?.FullName ?? "<missing>"}");
                continue;
            }
            if (AllowedChangedMethods.Contains(tok)) continue;
            if (CanonBody(bm) != CanonBody(cm))
            {
                nonTargetBodyDiffs++;
                if (nonTargetBodyDiffs <= 20) Console.WriteLine($"NON_TARGET_BODY_DIFF token=0x{tok:x8} method={bm.FullName}");
            }
        }
        Console.WriteLine($"METHOD_IDENTITY_DRIFT={methodIdentityDrift}");
        Console.WriteLine($"NON_TARGET_METHOD_SEMANTIC_DIFFS={nonTargetBodyDiffs}");
        if (methodIdentityDrift != 0) failures.Add($"method identity drift={methodIdentityDrift}");
        if (nonTargetBodyDiffs != 0) failures.Add($"non-target semantic body diffs={nonTargetBodyDiffs}");

        var candidateFieldByToken = candidateFields.ToDictionary(f => f.MetadataToken.ToUInt32());
        var fieldIdentityDrift = 0;
        foreach (var bf in baselineFields)
        {
            var tok = bf.MetadataToken.ToUInt32();
            if (!candidateFieldByToken.TryGetValue(tok, out var cf) || cf.FullName != bf.FullName)
            {
                fieldIdentityDrift++;
                if (fieldIdentityDrift <= 20) Console.WriteLine($"FIELD_IDENTITY_DRIFT token=0x{tok:x8} baseline={bf.FullName} candidate={cf?.FullName ?? "<missing>"}");
            }
        }
        Console.WriteLine($"FIELD_IDENTITY_DRIFT={fieldIdentityDrift}");
        if (fieldIdentityDrift != 0) failures.Add($"field identity drift={fieldIdentityDrift}");

        var baselineRefs = baseline.GetMemberReferences().OfType<MethodReference>().ToList();
        var candidateRefs = candidate.GetMemberReferences().OfType<MethodReference>().ToList();
        var baselineRefByToken = baselineRefs.ToDictionary(r => r.MetadataToken.ToUInt32());
        var baselineUses = UseCounts(baseline);
        var candidateUses = UseCounts(candidate);

        var targetKeys = new Dictionary<uint, string>();
        var targetFamilyByKey = new Dictionary<string, string>(StringComparer.Ordinal);
        var baselineTargetOk = 0;
        foreach (var t in Targets)
        {
            if (!baselineRefByToken.TryGetValue(t.Token, out var mr))
            {
                failures.Add($"baseline target missing 0x{t.Token:x8}");
                continue;
            }
            var key = SemanticKey(mr, t.Family);
            targetKeys[t.Token] = key;
            targetFamilyByKey[key] = t.Family;
            var uses = baselineUses.GetValueOrDefault(t.Token);
            var structural = StructuralMatch(mr, t.Family);
            var orphan = Orphan(mr);
            Console.WriteLine($"BASE_TARGET token=0x{t.Token:x8} key={key} uses={uses}/{t.ExpectedUses} structural={structural} orphan={orphan} sig={MethodSig(mr)}");
            if (uses != t.ExpectedUses) failures.Add($"baseline target 0x{t.Token:x8} use drift {uses}!={t.ExpectedUses}");
            if (!structural) failures.Add($"baseline target 0x{t.Token:x8} structural mismatch");
            if (orphan) failures.Add($"baseline target 0x{t.Token:x8} orphan generic");
            if (uses == t.ExpectedUses && structural && !orphan) baselineTargetOk++;
        }
        Console.WriteLine($"BASELINE_TARGET_TOKEN_CHECK={baselineTargetOk}/{Targets.Length}");

        var uniqueKeys = targetKeys.Values.Distinct(StringComparer.Ordinal).OrderBy(x => x, StringComparer.Ordinal).ToList();
        var candidateResolvedKeys = new HashSet<string>(StringComparer.Ordinal);
        var candidateResolveRowsOk = 0;
        var candidateResolveRowsFail = 0;
        var transformBadUses = new List<string>();

        foreach (var key in uniqueKeys)
        {
            var family = targetFamilyByKey[key];
            var baselineMatches = baselineRefs.Where(r => SemanticKeyAny(r) == key).ToList();
            var candidateMatches = candidateRefs.Where(r => SemanticKeyAny(r) == key).ToList();
            var baselineTotalUses = baselineMatches.Sum(r => baselineUses.GetValueOrDefault(r.MetadataToken.ToUInt32()));
            var candidateTotalUses = candidateMatches.Sum(r => candidateUses.GetValueOrDefault(r.MetadataToken.ToUInt32()));
            Console.WriteLine($"SEMANTIC_KEY key={key} family={family} baseline_rows={baselineMatches.Count} candidate_rows={candidateMatches.Count} baseline_uses={baselineTotalUses} candidate_uses={candidateTotalUses}");
            if (candidateMatches.Count == 0) failures.Add($"candidate missing semantic key {key}");
            if (candidateTotalUses != baselineTotalUses) failures.Add($"candidate use aggregate drift for {key}: {candidateTotalUses}!={baselineTotalUses}");

            var keyHasResolve = false;
            foreach (var mr in candidateMatches)
            {
                var tok = mr.MetadataToken.ToUInt32();
                var uses = candidateUses.GetValueOrDefault(tok);
                var structural = StructuralMatch(mr, family);
                var orphan = Orphan(mr);
                Console.WriteLine($"CANDIDATE_ROW token=0x{tok:x8} key={key} uses={uses} structural={structural} orphan={orphan} sig={MethodSig(mr)}");
                if (!structural) failures.Add($"candidate row 0x{tok:x8} structural mismatch for {key}");
                if (orphan) failures.Add($"candidate row 0x{tok:x8} orphan generic for {key}");
                try
                {
                    var resolved = mr.Resolve();
                    if (resolved is null)
                    {
                        candidateResolveRowsFail++;
                        failures.Add($"candidate row 0x{tok:x8} Resolve returned null for {key}");
                    }
                    else
                    {
                        candidateResolveRowsOk++;
                        keyHasResolve = true;
                        Console.WriteLine($"CANDIDATE_RESOLVE token=0x{tok:x8} OK -> {resolved.FullName}");
                    }
                }
                catch (AssemblyResolutionException ex)
                {
                    candidateResolveRowsFail++;
                    Console.WriteLine($"CANDIDATE_RESOLVE token=0x{tok:x8} DEPENDENCY_MISSING {ex.Message}");
                    if (requireResolve) failures.Add($"candidate row 0x{tok:x8} dependency missing for {key}: {ex.Message}");
                }
                catch (Exception ex)
                {
                    candidateResolveRowsFail++;
                    failures.Add($"candidate row 0x{tok:x8} Resolve failed for {key}: {ex.GetType().Name}: {ex.Message}");
                }
            }
            if (keyHasResolve) candidateResolvedKeys.Add(key);
        }

        var targetsAccounted = Targets.Count(t => targetKeys.TryGetValue(t.Token, out var key) && candidateResolvedKeys.Contains(key));
        Console.WriteLine($"SEMANTIC_TARGET_KEYS={uniqueKeys.Count}");
        Console.WriteLine($"SEMANTIC_TARGETS_ACCOUNTED={targetsAccounted}/{Targets.Length}");
        Console.WriteLine($"SEMANTIC_RESOLVE_KEYS={candidateResolvedKeys.Count}/{uniqueKeys.Count}");
        Console.WriteLine($"SEMANTIC_RESOLVE_ROWS ok={candidateResolveRowsOk} fail={candidateResolveRowsFail} require_resolve={requireResolve}");
        if (targetsAccounted != Targets.Length) failures.Add($"semantic target accounting {targetsAccounted}/{Targets.Length}");
        if (requireResolve && candidateResolvedKeys.Count != uniqueKeys.Count) failures.Add($"semantic resolve keys {candidateResolvedKeys.Count}/{uniqueKeys.Count}");

        foreach (var m in candidateMethods.Where(m => m.HasBody))
        {
            foreach (var ins in m.Body.Instructions)
            {
                if (ins.Operand is not MethodReference mr) continue;
                var key = SemanticKeyAny(mr);
                if (key is not ("get_transform|UnityEngine.Component" or "get_transform|UnityEngine.GameObject")) continue;
                if (ins.OpCode.Code != Code.Callvirt)
                    transformBadUses.Add($"{m.FullName} IL_{ins.Offset:x4} token=0x{mr.MetadataToken.ToUInt32():x8} opcode={ins.OpCode.Code} key={key}");
            }
        }
        Console.WriteLine($"TRANSFORM_BAD_USES={transformBadUses.Count}");
        foreach (var x in transformBadUses) Console.WriteLine("TRANSFORM_BAD " + x);
        if (transformBadUses.Count != 0) failures.Add($"transform non-callvirt uses={transformBadUses.Count}");

        var orphanHits = new List<string>();
        foreach (var t in candidateTypes)
        {
            foreach (var f in t.Fields)
                if (Orphan(f.FieldType)) orphanHits.Add($"field {f.FullName}");
            foreach (var m in t.Methods)
            {
                if (Orphan(m.ReturnType)) orphanHits.Add($"method-return {m.FullName}");
                foreach (var p in m.Parameters)
                    if (Orphan(p.ParameterType)) orphanHits.Add($"method-param {m.FullName}::{p.Name}");
                if (m.HasBody)
                    for (var i = 0; i < m.Body.Variables.Count; i++)
                        if (Orphan(m.Body.Variables[i].VariableType)) orphanHits.Add($"local {m.FullName} [{i}] {m.Body.Variables[i].VariableType.FullName}");
            }
        }
        foreach (var r in candidate.GetMemberReferences())
        {
            if (r is MethodReference mr && Orphan(mr)) orphanHits.Add($"memberref 0x{mr.MetadataToken.ToUInt32():x8} {MethodSig(mr)}");
            if (r is FieldReference fr && Orphan(fr)) orphanHits.Add($"fieldref 0x{fr.MetadataToken.ToUInt32():x8} {FieldSig(fr)}");
        }
        Console.WriteLine($"ORPHAN_GENERIC_HITS={orphanHits.Count}");
        foreach (var hit in orphanHits.Take(50)) Console.WriteLine("ORPHAN " + hit);
        if (orphanHits.Count != 0) failures.Add($"ownerless generic hits remain={orphanHits.Count}");

        if (failures.Count != 0)
        {
            Console.WriteLine($"STAGE9_SEMANTIC_MEMBERREF_AUDIT_FAIL count={failures.Count}");
            foreach (var f in failures) Console.WriteLine("FAIL " + f);
            return 1;
        }

        Console.WriteLine("STAGE9_SEMANTIC_MEMBERREF_AUDIT_OK");
        return 0;
    }
}
