using Mono.Cecil;
using Mono.Cecil.Cil;

static class Program
{
    static readonly uint[] Targets = { 0x0A0000C3u, 0x0A0000C4u, 0x0A0000C5u, 0x0A0000E7u };

    static string Tok(IMetadataTokenProvider p) => $"0x{p.MetadataToken.ToUInt32():x8}";

    static string TypeDetail(TypeReference? t)
    {
        if (t == null) return "<null>";
        if (t is GenericParameter gp)
        {
            var owner = gp.Owner == null ? "<null>" : gp.Owner.ToString();
            return $"GP(name={gp.Name},pos={gp.Position},kind={gp.Type},owner={owner})";
        }
        if (t is GenericInstanceType gi)
        {
            var args = string.Join(",", gi.GenericArguments.Select(TypeDetail));
            return $"GI({gi.ElementType.FullName}<{args}>) token={Tok(gi)}";
        }
        return $"{t.FullName} token={Tok(t)}";
    }

    static int Uses(ModuleDefinition m, uint token)
    {
        int n = 0;
        foreach (var type in m.Types)
            n += UsesInType(type, token);
        return n;
    }

    static int UsesInType(TypeDefinition type, uint token)
    {
        int n = 0;
        foreach (var method in type.Methods)
        {
            if (!method.HasBody) continue;
            foreach (var ins in method.Body.Instructions)
                if (ins.Operand is IMetadataTokenProvider p && p.MetadataToken.ToUInt32() == token)
                    n++;
        }
        foreach (var nested in type.NestedTypes)
            n += UsesInType(nested, token);
        return n;
    }

    static void DumpMember(ModuleDefinition m, uint token, string prefix)
    {
        var obj = m.LookupToken(new MetadataToken(token));
        Console.WriteLine($"{prefix} token=0x{token:x8} object={obj?.GetType().Name ?? "<null>"}");
        if (obj is not MemberReference mr) return;
        Console.WriteLine($"  full={mr.FullName}");
        Console.WriteLine($"  name={mr.Name} uses={Uses(m, token)}");
        Console.WriteLine($"  parent={TypeDetail(mr.DeclaringType)} parent_token={Tok(mr.DeclaringType)}");
        if (mr is MethodReference mm)
        {
            Console.WriteLine($"  return={TypeDetail(mm.ReturnType)}");
            Console.WriteLine($"  params=[{string.Join("; ", mm.Parameters.Select(p => TypeDetail(p.ParameterType)))}]");
        }
    }

    static int Main(string[] args)
    {
        if (args.Length != 1 || !File.Exists(args[0]))
        {
            Console.Error.WriteLine("usage: Stage9OrphanGenericDiagnostic <Assembly-CSharp.dll>");
            return 2;
        }

        using var asm = AssemblyDefinition.ReadAssembly(args[0], new ReaderParameters { ReadingMode = ReadingMode.Immediate });
        var m = asm.MainModule;
        Console.WriteLine($"MODULE={m.Name} KIND={m.Kind} METHODDEFS={m.Types.SelectMany(AllTypes).Sum(t => t.Methods.Count)}");

        foreach (var token in Targets)
            DumpMember(m, token, "TARGET");

        var e7 = m.LookupToken(new MetadataToken(0x0A0000E7u)) as MemberReference
                 ?? throw new InvalidOperationException("0x0A0000E7 missing");
        string canonicalParent = e7.DeclaringType.FullName;
        uint canonicalParentToken = e7.DeclaringType.MetadataToken.ToUInt32();
        Console.WriteLine($"CANONICAL_ELEMENT_PARENT full={canonicalParent} token=0x{canonicalParentToken:x8}");

        foreach (var mr in m.GetMemberReferences()
                            .Where(x => x.DeclaringType.FullName == canonicalParent)
                            .Where(x => x.Name is "get_Current" or "MoveNext" or "Dispose")
                            .OrderBy(x => x.MetadataToken.RID))
        {
            DumpMember(m, mr.MetadataToken.ToUInt32(), "ELEMENT_PARENT_MEMBER");
        }

        Console.WriteLine("NEIGHBOR_C0_C9_BEGIN");
        for (uint rid = 0xC0; rid <= 0xC9; rid++)
        {
            var token = 0x0A000000u | rid;
            var obj = m.LookupToken(new MetadataToken(token));
            if (obj is MemberReference)
                DumpMember(m, token, "NEIGHBOR");
        }
        Console.WriteLine("NEIGHBOR_C0_C9_END");
        Console.WriteLine("STAGE9_ORPHAN_DIAGNOSTIC_OK");
        return 0;
    }

    static IEnumerable<TypeDefinition> AllTypes(TypeDefinition t)
    {
        yield return t;
        foreach (var n in t.NestedTypes)
            foreach (var x in AllTypes(n))
                yield return x;
    }
}
