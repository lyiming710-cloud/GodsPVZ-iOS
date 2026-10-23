using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text.Json;
static string Sha(byte[] b)=>Convert.ToHexString(SHA256.HashData(b)).ToLowerInvariant();
if(args.Length!=3)throw new ArgumentException("patcher.dll Native93.dll report.json");
byte[] parent=File.ReadAllBytes(args[1]);
if(Sha(parent)!="b074224112d46a959de074d3090ab6d0614b036959c66c63b4f49954472e2162")throw new Exception("Parent SHA mismatch");
using var pe=new PEReader(new MemoryStream(parent));var reader=pe.GetMetadataReader();
var assembly=Assembly.LoadFrom(Path.GetFullPath(args[0]));
var type=assembly.GetType("Program",true)!;
var method=type.GetMethod("N120_SetSortingGroup",BindingFlags.Static|BindingFlags.NonPublic)!;
byte[] code=(byte[])method.Invoke(null,null)!;
var opcodes=typeof(OpCodes).GetFields(BindingFlags.Static|BindingFlags.Public).Where(f=>f.FieldType==typeof(OpCode))
    .Select(f=>(OpCode)f.GetValue(null)!).ToDictionary(o=>o.Value);
List<(int Offset,int Token)> Strings(byte[] bytes)
{
    var strings=new List<(int,int)>();int pos=0;
    while(pos<bytes.Length)
    {
        int offset=pos;short value=bytes[pos++];if(value==0xFE)value=unchecked((short)(0xFE00|bytes[pos++]));
        var op=opcodes[value];int size=op.OperandType switch{
            OperandType.InlineNone=>0,OperandType.ShortInlineI or OperandType.ShortInlineVar or OperandType.ShortInlineBrTarget=>1,
            OperandType.InlineVar=>2,OperandType.InlineI or OperandType.InlineBrTarget or OperandType.InlineField or
            OperandType.InlineMethod or OperandType.InlineSig or OperandType.InlineString or OperandType.InlineTok or
            OperandType.InlineType or OperandType.ShortInlineR=>4,OperandType.InlineI8 or OperandType.InlineR=>8,
            _=>throw new NotSupportedException(op.Name)};
        if(pos+size>bytes.Length)throw new Exception("Truncated emitted CIL");
        if(op==OpCodes.Ldstr)strings.Add((offset,BitConverter.ToInt32(bytes,pos)));
        pos+=size;
    }
    return strings;
}
bool Accept(byte[] bytes)
{
    var strings=Strings(bytes);
    return strings.Count==1&&strings[0].Token==0x70001E24&&reader.GetUserString(MetadataTokens.UserStringHandle(0x1E24))=="Entity";
}
if(!Accept(code))throw new Exception("Source emitter did not bind the original Entity literal");
var at=Strings(code).Single().Offset;
var wrong=(byte[])code.Clone();wrong[at+1]^=1;
var absent=(byte[])code.Clone();absent[at]=0x20; // balanced ldc.i4 operand shape, not ldstr
if(Accept(wrong)||Accept(absent))throw new Exception("Emitter controls failed");
var result=new{scope="SOURCE_EMITTER_LITERAL_BINDING_ONLY",parent_sha256=Sha(parent),
    patcher_sha256=Sha(File.ReadAllBytes(args[0])),emitted_cil_sha256=Sha(code),emitted_cil_bytes=code.Length,
    ldstr_offset=at,user_string_token="0x70001E24",original_literal="Entity",positive_literal_binding=true,
    wrong_token_rejected=true,ldstr_changed_to_integer_rejected=true,
    framework=System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
    game_candidate_constructed=false,game_cil_executed=false,native_semantics="UNPROVEN"};
if(File.Exists(args[2]))throw new IOException("Never overwrite evidence");
File.WriteAllText(args[2],JsonSerializer.Serialize(result,new JsonSerializerOptions{WriteIndented=true}));
Console.WriteLine(JsonSerializer.Serialize(result));
