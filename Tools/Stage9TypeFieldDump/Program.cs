using Mono.Cecil;

static class Program
{
    static IEnumerable<TypeDefinition> All(IEnumerable<TypeDefinition> ts)
    {
        foreach (var t in ts) { yield return t; foreach (var n in All(t.NestedTypes)) yield return n; }
    }
    static string Tok(IMetadataTokenProvider p) => $"0x{p.MetadataToken.ToUInt32():X8}";
    public static int Main(string[] args)
    {
        if (args.Length != 1) return 2;
        using var asm = AssemblyDefinition.ReadAssembly(args[0], new ReaderParameters{InMemory=true,ReadSymbols=false});
        var all = All(asm.MainModule.Types).ToList();
        string[] targets={"MouseManager","Plant","PlantManager","Board","BoardConfig","BoardEntry","DeviceManager","BoardEntryData"};
        foreach (var name in targets)
        {
            var hits=all.Where(t=>t.Name==name).ToList();
            Console.WriteLine($"TYPE_TARGET {name} matches={hits.Count}");
            foreach (var t in hits)
            {
                Console.WriteLine($"TYPE {Tok(t)} {t.FullName} base={t.BaseType?.FullName}");
                int i=0;
                foreach(var f in t.Fields)
                    Console.WriteLine($"FIELD index={i++} token={Tok(f)} static={f.IsStatic} type={f.FieldType.FullName} name={f.Name}");
            }
        }
        Console.WriteLine("STAGE9_TYPE_FIELD_DUMP_OK");
        Console.WriteLine("WRITE_STATUS=NOT_AUTHORIZED_AUDIT_ONLY");
        return 0;
    }
}
