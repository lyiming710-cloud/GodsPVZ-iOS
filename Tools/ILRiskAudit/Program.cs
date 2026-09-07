using Mono.Cecil;
using Mono.Cecil.Cil;

if (args.Length != 1) { Console.Error.WriteLine("Usage: GodsPVZ.ILRiskAudit <assembly.dll>"); return 2; }
using var module = ModuleDefinition.ReadModule(Path.GetFullPath(args[0]), new ReaderParameters { InMemory = true, ReadSymbols = false });
IEnumerable<TypeDefinition> All(IEnumerable<TypeDefinition> roots) { foreach (var t in roots) { yield return t; foreach (var n in All(t.NestedTypes)) yield return n; } }
var rows = new List<Row>();
int methods = 0, noteCalls = 0, listPrivateMethods = 0, dangerousMethods = 0, nreMethods = 0, pointerSignatureMethods = 0;
foreach (var t in All(module.Types))
{
    foreach (var m in t.Methods)
    {
        methods++;
        if (!m.HasBody) continue;
        int notes = 0, privateList = 0, dangerous = 0, nre = 0;
        foreach (var i in m.Body.Instructions)
        {
            if (i.Operand is MethodReference mr)
            {
                if (mr.DeclaringType.FullName.Contains("Cpp2ILHelpers") && mr.Name.Contains("NoteDecompilerIssue")) notes++;
                if (mr.DeclaringType.FullName == "System.NullReferenceException" && mr.Name == ".ctor") nre++;
            }
            if (i.Operand is FieldReference fr && fr.DeclaringType.FullName.StartsWith("System.Collections.Generic.List`1", StringComparison.Ordinal) && (fr.Name == "_items" || fr.Name == "_size" || fr.Name == "_version")) privateList++;
            if (IsDangerous(i.OpCode.Code)) dangerous++;
        }
        bool ptrSig = HasPointer(m.ReturnType) || m.Parameters.Any(p => HasPointer(p.ParameterType)) || m.Body.Variables.Any(v => HasPointer(v.VariableType));
        if (notes + privateList + dangerous + nre > 0 || ptrSig)
            rows.Add(new Row(t.FullName, m.Name, m.MetadataToken.ToInt32(), notes, privateList, dangerous, nre, ptrSig));
        noteCalls += notes;
        if (privateList > 0) listPrivateMethods++;
        if (dangerous > 0) dangerousMethods++;
        if (nre > 0) nreMethods++;
        if (ptrSig) pointerSignatureMethods++;
    }
}
Console.WriteLine($"METHODS={methods}");
Console.WriteLine($"NOTE_CALLS={noteCalls}");
Console.WriteLine($"METHODS_WITH_PRIVATE_LIST_FIELDS={listPrivateMethods}");
Console.WriteLine($"METHODS_WITH_DANGEROUS_OPCODES={dangerousMethods}");
Console.WriteLine($"METHODS_WITH_NRE_CONSTRUCTION={nreMethods}");
Console.WriteLine($"METHODS_WITH_POINTER_SIGNATURE_OR_LOCALS={pointerSignatureMethods}");
Console.WriteLine("---RISK_ROWS---");
foreach (var r in rows.OrderByDescending(r => r.Score).ThenBy(r => r.Type).ThenBy(r => r.Method))
    Console.WriteLine($"{r.Score}\t{r.Type}::{r.Method}\t0x{r.Token:X8}\tnote={r.Notes}\tlist={r.PrivateList}\tdanger={r.Dangerous}\tnre={r.Nre}\tptrsig={(r.PtrSig?1:0)}");
return 0;

static bool HasPointer(TypeReference t)
{
    if (t is PointerType or FunctionPointerType) return true;
    if (t is ByReferenceType br) return HasPointer(br.ElementType);
    if (t is ArrayType ar) return HasPointer(ar.ElementType);
    if (t is GenericInstanceType gi) return gi.GenericArguments.Any(HasPointer);
    return false;
}
static bool IsDangerous(Code c) => c is Code.Calli or Code.Jmp or Code.Localloc or Code.Cpblk or Code.Initblk or Code.Ldind_I or Code.Ldind_I1 or Code.Ldind_I2 or Code.Ldind_I4 or Code.Ldind_I8 or Code.Ldind_R4 or Code.Ldind_R8 or Code.Ldind_Ref or Code.Ldind_U1 or Code.Ldind_U2 or Code.Ldind_U4 or Code.Stind_I or Code.Stind_I1 or Code.Stind_I2 or Code.Stind_I4 or Code.Stind_I8 or Code.Stind_R4 or Code.Stind_R8 or Code.Stind_Ref;
record Row(string Type, string Method, int Token, int Notes, int PrivateList, int Dangerous, int Nre, bool PtrSig)
{
    public int Score => Notes * 2 + PrivateList * 3 + Dangerous * 4 + Nre * 2 + (PtrSig ? 4 : 0);
}
