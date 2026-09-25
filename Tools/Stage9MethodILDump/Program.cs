using System.Security.Cryptography;
using Mono.Cecil;
using Mono.Cecil.Cil;

static class Stage9MethodILDump
{
    private sealed record Target(string TypeName, string MethodName);

    private static readonly Target[] Targets =
    {
        new("MouseManager", "GetPlantUnderMouse"),
        new("DeviceManager", "Start_BoardEntry"),
        new("MouseManager", "GetZombieUnderMouse"),
        new("EnemyManager", "CreateEnemySelecter"),
    };

    private static string Sha256(string path) =>
        Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path))).ToLowerInvariant();

    private static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
    {
        foreach (var t in roots)
        {
            yield return t;
            foreach (var n in AllTypes(t.NestedTypes)) yield return n;
        }
    }

    private static string Token(IMetadataTokenProvider? p) =>
        p is null ? "-" : $"0x{p.MetadataToken.ToUInt32():X8}";

    private static string SafeResolve(MethodReference mr)
    {
        try
        {
            var d = mr.Resolve();
            return d is null ? "NULL" : $"OK:{Token(d)}:{d.FullName}";
        }
        catch (Exception ex)
        {
            return $"FAIL:{ex.GetType().Name}:{ex.Message.Replace('\n',' ').Replace('\r',' ')}";
        }
    }

    private static string SafeResolve(FieldReference fr)
    {
        try
        {
            var d = fr.Resolve();
            return d is null ? "NULL" : $"OK:{Token(d)}:{d.FullName}";
        }
        catch (Exception ex)
        {
            return $"FAIL:{ex.GetType().Name}:{ex.Message.Replace('\n',' ').Replace('\r',' ')}";
        }
    }

    private static string FormatOperand(object? operand)
    {
        return operand switch
        {
            null => "",
            Instruction i => $"IL_{i.Offset:X4}",
            Instruction[] many => string.Join(",", many.Select(i => $"IL_{i.Offset:X4}")),
            VariableDefinition v => $"V_{v.Index}:{v.VariableType.FullName}",
            ParameterDefinition p => $"A_{p.Index}:{p.ParameterType.FullName}:{p.Name}",
            MethodReference m => $"{Token(m)} {m.FullName} RESOLVE={SafeResolve(m)}",
            FieldReference f => $"{Token(f)} {f.FullName} RESOLVE={SafeResolve(f)}",
            TypeReference t => $"{Token(t)} {t.FullName}",
            CallSite c => c.FullName,
            string s => $"\"{s.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"",
            _ => operand.ToString() ?? "",
        };
    }

    private static IEnumerable<Instruction> BranchTargets(Instruction ins)
    {
        if (ins.Operand is Instruction one) yield return one;
        else if (ins.Operand is Instruction[] many)
            foreach (var x in many) yield return x;
    }

    private static void DumpMethod(MethodDefinition m)
    {
        Console.WriteLine($"METHOD token={Token(m)} rva=0x{m.RVA:X8} full={m.FullName}");
        Console.WriteLine($"ATTRS={m.Attributes} IMPL={m.ImplAttributes} STATIC={m.IsStatic} GENERIC_PARAMS={m.GenericParameters.Count}");
        if (!m.HasBody)
        {
            Console.WriteLine("NO_BODY");
            return;
        }

        var b = m.Body;
        Console.WriteLine($"BODY code_size={b.CodeSize} max_stack={b.MaxStackSize} init_locals={b.InitLocals} locals={b.Variables.Count} eh={b.ExceptionHandlers.Count}");
        foreach (var v in b.Variables)
            Console.WriteLine($"LOCAL V_{v.Index} type={v.VariableType.FullName} token={Token(v.VariableType)} pinned={v.IsPinned}");

        foreach (var eh in b.ExceptionHandlers.Select((x, i) => (x, i)))
        {
            static string Off(Instruction? x) => x is null ? "END" : $"IL_{x.Offset:X4}";
            Console.WriteLine($"EH {eh.i} type={eh.x.HandlerType} catch={eh.x.CatchType?.FullName ?? "-"} try={Off(eh.x.TryStart)}..{Off(eh.x.TryEnd)} handler={Off(eh.x.HandlerStart)}..{Off(eh.x.HandlerEnd)} filter={Off(eh.x.FilterStart)}");
        }

        var inbound = new Dictionary<Instruction,List<string>>();
        foreach (var dst in b.Instructions) inbound[dst] = new List<string>();
        foreach (var src in b.Instructions)
            foreach (var dst in BranchTargets(src))
                if (inbound.TryGetValue(dst, out var list)) list.Add($"IL_{src.Offset:X4}:{src.OpCode.Name}");

        foreach (var i in b.Instructions)
        {
            var edges = inbound[i].Count == 0 ? "" : $" IN=[{string.Join(',', inbound[i])}]";
            Console.WriteLine($"IL_{i.Offset:X4}: {i.OpCode.Name,-12} {FormatOperand(i.Operand)}{edges}");
        }

        var methods = b.Instructions
            .Select(i => i.Operand)
            .OfType<MethodReference>()
            .GroupBy(x => x.MetadataToken.ToUInt32())
            .Select(g => g.First())
            .OrderBy(x => x.MetadataToken.ToUInt32());
        foreach (var mr in methods)
            Console.WriteLine($"METHODREF token={Token(mr)} name={mr.FullName} declaring={mr.DeclaringType.FullName} resolve={SafeResolve(mr)}");

        var fields = b.Instructions
            .Select(i => i.Operand)
            .OfType<FieldReference>()
            .GroupBy(x => x.MetadataToken.ToUInt32())
            .Select(g => g.First())
            .OrderBy(x => x.MetadataToken.ToUInt32());
        foreach (var fr in fields)
            Console.WriteLine($"FIELDREF token={Token(fr)} name={fr.FullName} resolve={SafeResolve(fr)}");
    }

    public static int Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.Error.WriteLine("Usage: GodsPVZ.Stage9MethodILDump <assembly> [assembly...]");
            return 2;
        }

        var failed = false;
        foreach (var raw in args)
        {
            var path = Path.GetFullPath(raw);
            if (!File.Exists(path))
            {
                Console.WriteLine($"ASSEMBLY_MISSING path={path}");
                failed = true;
                continue;
            }
            Console.WriteLine("================================================================================");
            Console.WriteLine($"ASSEMBLY path={path} sha256={Sha256(path)}");
            using var asm = AssemblyDefinition.ReadAssembly(path, new ReaderParameters { InMemory = true, ReadSymbols = false });
            Console.WriteLine($"MODULE name={asm.MainModule.Name} kind={asm.MainModule.Kind} methods={AllTypes(asm.MainModule.Types).Sum(t => t.Methods.Count)}");
            var types = AllTypes(asm.MainModule.Types).ToList();

            foreach (var target in Targets)
            {
                var matches = types.SelectMany(t => t.Methods)
                    .Where(m => m.DeclaringType.Name == target.TypeName && m.Name == target.MethodName)
                    .ToList();
                Console.WriteLine($"TARGET type={target.TypeName} method={target.MethodName} matches={matches.Count}");
                if (matches.Count != 1)
                {
                    foreach (var m in matches) Console.WriteLine($"AMBIGUOUS {Token(m)} {m.FullName}");
                    failed = true;
                    continue;
                }
                DumpMethod(matches[0]);
            }
        }

        Console.WriteLine(failed ? "STAGE9_METHOD_IL_DUMP_INCOMPLETE" : "STAGE9_METHOD_IL_DUMP_OK");
        Console.WriteLine("WRITE_STATUS=NOT_AUTHORIZED_AUDIT_ONLY");
        return failed ? 1 : 0;
    }
}
