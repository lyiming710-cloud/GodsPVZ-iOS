using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 3)
{
    Console.Error.WriteLine("Usage: Stage9SuppliesInfoCtorPatch <input.dll> <output.dll> <expected-input-sha256>");
    return 2;
}

const uint TargetToken = 0x06000150;
const int ExpectedRid = 336;
const uint NameFieldToken = 0x04000174;
const uint IdFieldToken = 0x04000175;
const int ExpectedMethods = 2317;
const int ExpectedFields = 2802;
const int ExpectedOldCodeSize = 74;
const int ExpectedOldLocals = 6;
const string NativeSha = "8f3ec66297f376a23bbc8330f86db9030203d2f8ba4b3c5a2cbc26c49f1a437a";

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}
static uint Raw(IMetadataTokenProvider p) => p.MetadataToken.ToUInt32();
static string Sha(string p) => Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(p))).ToLowerInvariant();
static string Scope(IMetadataScope? s) => s switch
{
    AssemblyNameReference a => a.Name,
    ModuleDefinition m => m.Assembly?.Name?.Name ?? m.Name,
    ModuleReference mr => mr.Name,
    _ => s?.ToString() ?? "<null>"
};
static string TypeSig(TypeReference t) => $"{t.FullName}@{Scope(t.Scope)}";
static string OpSig(object? o, MethodDefinition owner)
{
    if (o is null) return "";
    if (o is Instruction i) return $"I#{owner.Body.Instructions.IndexOf(i)}";
    if (o is Instruction[] sw) return "SW[" + string.Join(',', sw.Select(i => owner.Body.Instructions.IndexOf(i))) + "]";
    if (o is MethodReference mr) return $"M:{mr.FullName}@{Scope(mr.DeclaringType.Scope)}";
    if (o is FieldReference fr) return $"F:{fr.FullName}@{Scope(fr.DeclaringType.Scope)}";
    if (o is TypeReference tr) return $"T:{TypeSig(tr)}";
    if (o is VariableDefinition v) return $"V:{v.Index}:{TypeSig(v.VariableType)}";
    if (o is ParameterDefinition p) return $"P:{p.Index}:{TypeSig(p.ParameterType)}";
    if (o is string s) return "S:" + Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
    if (o is float f) return $"R4:{BitConverter.SingleToInt32Bits(f):X8}";
    if (o is double d) return $"R8:{BitConverter.DoubleToInt64Bits(d):X16}";
    return "C:" + Convert.ToString(o, CultureInfo.InvariantCulture);
}
static string MethodSig(MethodDefinition m)
{
    var sb = new StringBuilder();
    sb.Append(m.FullName).Append('|').Append(m.Attributes).Append('|').Append(m.ImplAttributes).Append('|');
    if (!m.HasBody) return sb.Append("NOBODY").ToString();
    sb.Append("init=").Append(m.Body.InitLocals).Append(";max=").Append(m.Body.MaxStackSize).Append(';');
    foreach (var v in m.Body.Variables) sb.Append("L:").Append(v.Index).Append(':').Append(TypeSig(v.VariableType)).Append(';');
    foreach (var i in m.Body.Instructions) sb.Append("I:").Append(i.OpCode.Code).Append(':').Append(OpSig(i.Operand, m)).Append(';');
    foreach (var h in m.Body.ExceptionHandlers)
    {
        int Idx(Instruction? x) => x is null ? -1 : m.Body.Instructions.IndexOf(x);
        sb.Append("EH:").Append(h.HandlerType).Append(':').Append(Idx(h.TryStart)).Append(':').Append(Idx(h.TryEnd)).Append(':')
          .Append(Idx(h.HandlerStart)).Append(':').Append(Idx(h.HandlerEnd)).Append(':').Append(Idx(h.FilterStart)).Append(':')
          .Append(h.CatchType is null ? "" : TypeSig(h.CatchType)).Append(';');
    }
    return sb.ToString();
}
static string FieldSig(FieldDefinition f)
{
    var attrs = string.Join(',', f.CustomAttributes.Select(a => a.AttributeType.FullName).OrderBy(x => x, StringComparer.Ordinal));
    var c = f.HasConstant ? Convert.ToString(f.Constant, CultureInfo.InvariantCulture) ?? "<null>" : "<none>";
    return $"{f.FullName}|{f.Attributes}|{TypeSig(f.FieldType)}|const={c}|ca={attrs}";
}
static MethodReference FindObjectCtor(IEnumerable<MethodDefinition> methods)
{
    var refs = methods.Where(m => m.HasBody).SelectMany(m => m.Body.Instructions)
        .Select(i => i.Operand).OfType<MethodReference>()
        .Where(m => m.DeclaringType.FullName == "System.Object" && m.Name == ".ctor" && m.Parameters.Count == 0)
        .GroupBy(m => m.FullName + "@" + Scope(m.DeclaringType.Scope), StringComparer.Ordinal)
        .Select(g => g.First()).ToList();
    if (refs.Count != 1) throw new InvalidDataException($"System.Object::.ctor refs={refs.Count}");
    return refs[0];
}

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var expectedInputSha = args[2].Trim().ToLowerInvariant();
if (expectedInputSha.Length != 64 || expectedInputSha.Any(c => !Uri.IsHexDigit(c))) throw new InvalidDataException("expected input SHA invalid");
if (Sha(input) != expectedInputSha) throw new InvalidDataException("input SHA mismatch");
Console.WriteLine($"INPUT_SHA256 {Sha(input)}");

var beforeM = new Dictionary<uint,string>();
var beforeF = new Dictionary<uint,string>();
string beforeRefs;
using (var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate }))
{
    var types = AllTypes(module.Types).ToList();
    var methods = types.SelectMany(t => t.Methods).ToList();
    var fields = types.SelectMany(t => t.Fields).ToList();
    if (methods.Count != ExpectedMethods || fields.Count != ExpectedFields) throw new InvalidDataException("metadata count drift");
    if (module.AssemblyReferences.Any(a => a.Name == "System.Private.CoreLib")) throw new InvalidDataException("input references System.Private.CoreLib");
    foreach (var m in methods) beforeM[Raw(m)] = MethodSig(m);
    foreach (var f in fields) beforeF[Raw(f)] = FieldSig(f);
    beforeRefs = string.Join("\n", module.AssemblyReferences.Select(a => a.FullName).OrderBy(x => x, StringComparer.Ordinal));

    var t = types.Single(x => x.FullName == "SuppliesInfo");
    var target = methods.Single(m => Raw(m) == TargetToken);
    if (target.DeclaringType != t || !target.IsConstructor || target.IsStatic || target.Parameters.Count != 1 || target.Parameters[0].ParameterType.FullName != "System.Int32")
        throw new InvalidDataException("target ctor identity drift");
    if (target.MetadataToken.RID != ExpectedRid || !target.HasBody || target.Body.CodeSize != ExpectedOldCodeSize || target.Body.Variables.Count != ExpectedOldLocals)
        throw new InvalidDataException($"target body fingerprint drift rid={target.MetadataToken.RID} size={target.Body.CodeSize} locals={target.Body.Variables.Count}");
    var name = fields.Single(f => Raw(f) == NameFieldToken);
    var id = fields.Single(f => Raw(f) == IdFieldToken);
    if (name.DeclaringType != t || name.Name != "name" || name.FieldType.FullName != "System.String") throw new InvalidDataException("name field identity drift");
    if (id.DeclaringType != t || id.Name != "ID" || id.FieldType.FullName != "System.Int32") throw new InvalidDataException("ID field identity drift");
    var stringEmpty = target.Body.Instructions.Select(i => i.Operand).OfType<FieldReference>()
        .Single(fr => fr.DeclaringType.FullName == "System.String" && fr.Name == "Empty" && fr.FieldType.FullName == "System.String");
    var objectCtor = FindObjectCtor(methods);
    if (!target.Body.Instructions.Any(i => i.OpCode == OpCodes.Ldc_I8 && i.Operand is long n && n == 4294967295L)) throw new InvalidDataException("damaged I8 -1 missing");
    if (!target.Body.Instructions.Any(i => i.Operand is string s && s.Contains("Unmanaged memory load:", StringComparison.Ordinal))) throw new InvalidDataException("fake unmanaged diagnostic missing");
    if (!target.Body.Instructions.Any(i => i.Operand is string s && s.Contains("Method not found @180302170", StringComparison.Ordinal))) throw new InvalidDataException("fake object ctor diagnostic missing");

    Console.WriteLine($"NATIVE_AUTHORITY target_token=0x{TargetToken:X8} rid={ExpectedRid} method_pointer_entry=0x181B837D8 va=0x180325180 end=0x1803251E8 native_slice_sha256={NativeSha}");
    Console.WriteLine($"RECOVERY_STRATEGY full_single_MethodDef_rebuild=1 semantics=name_String.Empty_ID_minus1_Object_ctor_ID_arg metadata_changes=0 guards=0 exception_swallowing=0");

    var body = target.Body;
    body.Instructions.Clear(); body.Variables.Clear(); body.ExceptionHandlers.Clear();
    body.InitLocals = false; body.MaxStackSize = 2;
    var il = body.GetILProcessor();
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldsfld, stringEmpty));
    il.Append(il.Create(OpCodes.Stfld, name));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldc_I4_M1));
    il.Append(il.Create(OpCodes.Stfld, id));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Call, objectCtor));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldarg_1));
    il.Append(il.Create(OpCodes.Stfld, id));
    il.Append(il.Create(OpCodes.Ret));
    Directory.CreateDirectory(Path.GetDirectoryName(output)!);
    module.Write(output);
}

Console.WriteLine($"OUTPUT_SHA256 {Sha(output)}");
using (var module = ModuleDefinition.ReadModule(output, new ReaderParameters { InMemory=true, ReadingMode=ReadingMode.Immediate }))
{
    var types=AllTypes(module.Types).ToList(); var methods=types.SelectMany(t=>t.Methods).ToList(); var fields=types.SelectMany(t=>t.Fields).ToList();
    if(methods.Count!=ExpectedMethods || fields.Count!=ExpectedFields) throw new InvalidDataException("metadata count drift after reopen");
    var target=methods.Single(m=>Raw(m)==TargetToken); var name=fields.Single(f=>Raw(f)==NameFieldToken); var id=fields.Single(f=>Raw(f)==IdFieldToken);
    if(target.Body.Variables.Count!=0 || target.Body.ExceptionHandlers.Count!=0) throw new InvalidDataException("rebuild locals/handlers drift");
    var ins=target.Body.Instructions;
    if(ins.Count!=12) throw new InvalidDataException($"rebuild instruction count={ins.Count}");
    if(!(ins[0].OpCode==OpCodes.Ldarg_0 && ins[1].OpCode==OpCodes.Ldsfld && ins[1].Operand is FieldReference e && e.DeclaringType.FullName=="System.String" && e.Name=="Empty" && ins[2].OpCode==OpCodes.Stfld && Raw(((FieldReference)ins[2].Operand).Resolve())==NameFieldToken)) throw new InvalidDataException("name=String.Empty sequence drift");
    if(!(ins[3].OpCode==OpCodes.Ldarg_0 && ins[4].OpCode==OpCodes.Ldc_I4_M1 && ins[5].OpCode==OpCodes.Stfld && Raw(((FieldReference)ins[5].Operand).Resolve())==IdFieldToken)) throw new InvalidDataException("ID=-1 sequence drift");
    if(!(ins[6].OpCode==OpCodes.Ldarg_0 && ins[7].OpCode==OpCodes.Call && ins[7].Operand is MethodReference oc && oc.DeclaringType.FullName=="System.Object" && oc.Name==".ctor" && oc.Parameters.Count==0)) throw new InvalidDataException("Object ctor sequence drift");
    if(!(ins[8].OpCode==OpCodes.Ldarg_0 && (ins[9].OpCode==OpCodes.Ldarg_1 || (ins[9].OpCode==OpCodes.Ldarg && ins[9].Operand is ParameterDefinition)) && ins[10].OpCode==OpCodes.Stfld && Raw(((FieldReference)ins[10].Operand).Resolve())==IdFieldToken && ins[11].OpCode==OpCodes.Ret)) throw new InvalidDataException("ID=id sequence drift");
    var text=string.Join("\n",ins.Select(i=>$"{i.OpCode} {i.Operand}"));
    foreach(var bad in new[]{"Unmanaged memory load:","Method not found @","System.Private.CoreLib"}) if(text.Contains(bad,StringComparison.Ordinal)) throw new InvalidDataException("corruption remains: "+bad);
    var changedM=methods.Where(m=>beforeM[Raw(m)]!=MethodSig(m)).Select(Raw).OrderBy(x=>x).ToList();
    if(changedM.Count!=1 || changedM[0]!=TargetToken) throw new InvalidDataException("semantic isolation failed: "+string.Join(',',changedM.Select(x=>$"0x{x:X8}")));
    var changedF=fields.Where(f=>beforeF[Raw(f)]!=FieldSig(f)).Select(Raw).ToList();
    if(changedF.Count!=0) throw new InvalidDataException("field metadata isolation failed");
    var afterRefs=string.Join("\n", module.AssemblyReferences.Select(a=>a.FullName).OrderBy(x=>x,StringComparer.Ordinal));
    if(beforeRefs!=afterRefs) throw new InvalidDataException("assembly reference set changed");
    if(module.AssemblyReferences.Any(a=>a.Name=="System.Private.CoreLib")) throw new InvalidDataException("System.Private.CoreLib pollution");
    Console.WriteLine($"REOPEN_SUPPLIESINFO_CTOR_PASS token=0x{TargetToken:X8} name_token=0x{NameFieldToken:X8} id_token=0x{IdFieldToken:X8} string_empty=1 id_minus_one=1 object_ctor=1 id_arg=1 fake_diagnostics=0");
    Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={ExpectedMethods-1} changed_methods=1 target=0x{TargetToken:X8}");
    Console.WriteLine($"FIELD_METADATA_ISOLATION_PASS unchanged_fields={ExpectedFields} changed_fields=0");
    Console.WriteLine("FRAMEWORK_REFERENCE_GATE_PASS system_private_corelib_refs=0 assembly_reference_set_unchanged=1");
}
return 0;
