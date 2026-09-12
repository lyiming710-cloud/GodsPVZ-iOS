using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Security.Cryptography;
using System.Text;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: GodsPVZ.Stage9MainMenuPopupPathPatch <input.dll> <output.dll>");
    return 2;
}

const string ExpectedInputSha256 = "52ec4dac6d7290a218836a9e977bb5f7bb8bff08252b8feaf2f67d94d8e136f4";
const int ExpectedMethodDefCount = 2317;
const uint CreateListToken = 0x0600023Bu;
const uint TryCreateToken = 0x0600024Fu;
const uint GetterToken = 0x06000636u;
const uint WindowCtorToken = 0x060006FBu;
const uint BackingFieldToken = 0x04000843u;

static IEnumerable<TypeDefinition> AllTypes(IEnumerable<TypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}
static uint Raw(IMetadataTokenProvider p) => p.MetadataToken.ToUInt32();
static string OperandSemantic(object? operand, MethodDefinition owner)
{
    if (operand is null) return "";
    if (operand is Instruction bi) return $"I#{owner.Body.Instructions.IndexOf(bi)}";
    if (operand is Instruction[] sw) return "SW[" + string.Join(',', sw.Select(x => owner.Body.Instructions.IndexOf(x))) + "]";
    if (operand is MethodReference mr) return $"M:{mr.FullName}";
    if (operand is FieldReference fr) return $"F:{fr.FullName}";
    if (operand is TypeReference tr) return $"T:{tr.FullName}";
    if (operand is VariableDefinition vr) return $"V:{vr.Index}:{vr.VariableType.FullName}";
    if (operand is ParameterDefinition pr) return $"P:{pr.Index}:{pr.ParameterType.FullName}";
    if (operand is string s) return $"S:{Convert.ToBase64String(Encoding.UTF8.GetBytes(s))}";
    return $"C:{Convert.ToString(operand, System.Globalization.CultureInfo.InvariantCulture)}";
}
static string MethodSemantic(MethodDefinition m)
{
    var sb = new StringBuilder();
    sb.Append(m.FullName).Append('|').Append(m.Attributes).Append('|').Append(m.ImplAttributes).Append('|');
    if (!m.HasBody) return sb.Append("NOBODY").ToString();
    sb.Append("init=").Append(m.Body.InitLocals).Append(";max=").Append(m.Body.MaxStackSize).Append(';');
    foreach (var v in m.Body.Variables) sb.Append("L:").Append(v.Index).Append(':').Append(v.VariableType.FullName).Append(';');
    foreach (var i in m.Body.Instructions) sb.Append("I:").Append(i.OpCode.Code).Append(':').Append(OperandSemantic(i.Operand, m)).Append(';');
    return sb.ToString();
}
static Instruction At(MethodDefinition m, int offset)
    => m.Body.Instructions.SingleOrDefault(i => i.Offset == offset)
       ?? throw new InvalidDataException($"{m.FullName}: missing IL_{offset:X4}");

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var inputSha = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(input))).ToLowerInvariant();
Console.WriteLine($"INPUT_SHA256 {inputSha}");
if (inputSha != ExpectedInputSha256) throw new InvalidDataException($"input SHA mismatch: {inputSha}");

using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate });
var methods = AllTypes(module.Types).SelectMany(t => t.Methods).ToList();
if (methods.Count != ExpectedMethodDefCount) throw new InvalidDataException($"MethodDef count drifted: {methods.Count}");
MethodDefinition M(uint token) => module.LookupToken(new MetadataToken(TokenType.Method,(int)(token&0x00FFFFFF))) as MethodDefinition
    ?? throw new InvalidDataException($"method 0x{token:X8} missing");
var createList=M(CreateListToken); var tryCreate=M(TryCreateToken); var getter=M(GetterToken); var ctor=M(WindowCtorToken);
if(createList.FullName!="System.Void SavesManager::CreateNewPlayerSaveList()" || tryCreate.FullName!="System.Void SavesManager::TryCreateNewPlayerSave()") throw new InvalidDataException("SavesManager target drift");
if(getter.FullName!="MainUIController MainUIController::get_instance()" || !getter.IsStatic || !getter.IsPublic) throw new InvalidDataException($"getter drift: {getter.FullName} static={getter.IsStatic} public={getter.IsPublic}");
if(ctor.FullName!="System.Void Window_I::.ctor()") throw new InvalidDataException("Window_I ctor drift");

var changed = new HashSet<uint>{CreateListToken,TryCreateToken,WindowCtorToken};
var untouchedBefore=methods.Where(m=>!changed.Contains(Raw(m))).ToDictionary(Raw,MethodSemantic);

static void ReplacePrivateInstanceRead(MethodDefinition m,int offset,MethodDefinition getter)
{
    var i=At(m,offset);
    if(i.OpCode.Code!=Code.Ldsfld || i.Operand is not FieldReference fr || Raw(fr)!=BackingFieldToken)
        throw new InvalidDataException($"{m.FullName} IL_{offset:X4}: expected backing-field ldsfld");
    i.OpCode=OpCodes.Call; i.Operand=getter;
}
ReplacePrivateInstanceRead(createList,0x000A,getter);
ReplacePrivateInstanceRead(tryCreate,0x0005,getter);
Console.WriteLine("PATCH_MAINUI_INSTANCE sites=2 backing_field=0x04000843 getter=0x06000636");

var c11=At(ctor,0x0011); var c1A=At(ctor,0x001A);
if(c11.OpCode.Code!=Code.Ldc_I8 || Convert.ToInt64(c11.Operand)!=4294967295L)
    throw new InvalidDataException($"Window_I::.ctor IL_0011 precondition drift: {c11.OpCode} {c11.Operand}");
if(c1A.OpCode.Code!=Code.Stfld || c1A.Operand is not FieldReference info || info.FullName!="System.Int32 Window_I::info0_int")
    throw new InvalidDataException("Window_I::.ctor info0_int store drift");
c11.OpCode=OpCodes.Ldc_I4_M1; c11.Operand=null;
Console.WriteLine("PATCH_WINDOWI_CTOR info0_int=-1 opcode=ldc.i4.m1");

module.Write(output);
var outputSha=Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(output))).ToLowerInvariant();
Console.WriteLine($"OUTPUT_SHA256 {outputSha}");

using var reopen=ModuleDefinition.ReadModule(output,new ReaderParameters{InMemory=true,ReadingMode=ReadingMode.Immediate});
var after=AllTypes(reopen.Types).SelectMany(t=>t.Methods).ToList();
if(after.Count!=ExpectedMethodDefCount) throw new InvalidDataException("reopen MethodDef count drift");
MethodDefinition RM(uint token)=>reopen.LookupToken(new MetadataToken(TokenType.Method,(int)(token&0x00FFFFFF))) as MethodDefinition ?? throw new InvalidDataException("reopen method missing");
var rCreate=RM(CreateListToken);var rTry=RM(TryCreateToken);var rCtor=RM(WindowCtorToken);
foreach(var (m,o) in new[]{(rCreate,0x000A),(rTry,0x0005)}){
    var i=At(m,o);if(i.OpCode.Code!=Code.Call || i.Operand is not MethodReference mr || Raw(mr)!=GetterToken) throw new InvalidDataException($"reopen getter repair missing at 0x{Raw(m):X8}/IL_{o:X4}");
}
var rc11=At(rCtor,0x0011);if(rc11.OpCode.Code!=Code.Ldc_I4_M1) throw new InvalidDataException("reopen Window_I ctor repair missing");
Console.WriteLine("REOPEN_MAINMENU_POPUP_PATH_PASS instance_getter_sites=2 windowi_ctor_int32=1");
var untouchedAfter=after.Where(m=>!changed.Contains(Raw(m))).ToDictionary(Raw,MethodSemantic);
if(untouchedAfter.Count!=untouchedBefore.Count) throw new InvalidDataException("untouched method count drift");
var drift=untouchedBefore.Where(kv=>!untouchedAfter.TryGetValue(kv.Key,out var s)||s!=kv.Value).Select(kv=>kv.Key).ToList();
if(drift.Count!=0) throw new InvalidDataException("semantic drift outside popup-path targets: "+string.Join(',',drift.Select(t=>$"0x{t:X8}")));
Console.WriteLine($"SEMANTIC_ISOLATION_PASS untouched_methods={untouchedAfter.Count} changed_methods=3");
return 0;
