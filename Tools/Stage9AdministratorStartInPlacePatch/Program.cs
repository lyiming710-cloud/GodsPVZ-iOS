using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using Mono.Cecil;
using Mono.Cecil.Cil;
using CecilFieldDefinition = Mono.Cecil.FieldDefinition;
using CecilFieldReference = Mono.Cecil.FieldReference;
using CecilGenericInstanceMethod = Mono.Cecil.GenericInstanceMethod;
using CecilMethodDefinition = Mono.Cecil.MethodDefinition;
using CecilMethodReference = Mono.Cecil.MethodReference;
using CecilModule = Mono.Cecil.ModuleDefinition;
using CecilTypeDefinition = Mono.Cecil.TypeDefinition;
using CecilTypeReference = Mono.Cecil.TypeReference;

const string ExpectedBaselineSha = "26064265fdba4e9b501f0c3312260503fdde43b9e111072c19dd39891e5ca022";
const uint TargetToken = 0x06000004;
const uint ReusedLocalSigToken = 0x11000101;

static IEnumerable<CecilTypeDefinition> AllTypes(IEnumerable<CecilTypeDefinition> roots)
{
    foreach (var t in roots)
    {
        yield return t;
        foreach (var n in AllTypes(t.NestedTypes)) yield return n;
    }
}

static uint Raw(IMetadataTokenProvider p) => p.MetadataToken.ToUInt32();
static string Sha(byte[] b) => Convert.ToHexString(SHA256.HashData(b)).ToLowerInvariant();

static int RvaToOffset(PEHeaders headers, int rva)
{
    foreach (var s in headers.SectionHeaders)
    {
        var span = Math.Max(s.VirtualSize, s.SizeOfRawData);
        if (rva >= s.VirtualAddress && rva < s.VirtualAddress + span)
            return s.PointerToRawData + (rva - s.VirtualAddress);
    }
    throw new InvalidDataException($"RVA 0x{rva:X} not mapped");
}

static int MethodBodyLength(byte[] bytes, int offset)
{
    ushort flags = BitConverter.ToUInt16(bytes, offset);
    if ((flags & 3) != 3) throw new InvalidDataException("expected fat method body");
    int headerSize = ((flags >> 12) & 0xF) * 4;
    int codeSize = BitConverter.ToInt32(bytes, offset + 4);
    int end = headerSize + codeSize;
    if ((flags & 8) == 0) return end;

    int p = (end + 3) & ~3;
    bool more;
    do
    {
        byte kind = bytes[offset + p];
        bool fat = (kind & 0x40) != 0;
        more = (kind & 0x80) != 0;
        int dataSize = fat
            ? bytes[offset + p + 1] | (bytes[offset + p + 2] << 8) | (bytes[offset + p + 3] << 16)
            : bytes[offset + p + 1];
        if (dataSize < 4) throw new InvalidDataException("invalid extra method section");
        p += dataSize;
        if (more) p = (p + 3) & ~3;
    } while (more);
    return p;
}

static uint MapToken(IMetadataTokenProvider p)
{
    if (p is CecilMethodDefinition or CecilFieldDefinition or CecilTypeDefinition)
        return Raw(p);

    if (p is CecilGenericInstanceMethod gim &&
        gim.DeclaringType.FullName == "UnityEngine.Component" &&
        gim.Name == "GetComponent" &&
        gim.GenericArguments.Count == 1 &&
        gim.GenericArguments[0].FullName == "UnityEngine.Canvas")
        return 0x2B000001;

    if (p is CecilMethodReference m)
    {
        var key = (m.DeclaringType.FullName, m.Name, m.Parameters.Count);
        return key switch
        {
            ("UnityEngine.Transform", "GetChild", 1) => 0x0A000001,
            ("UnityEngine.Camera", "get_main", 0) => 0x0A000003,
            ("UnityEngine.Canvas", "set_worldCamera", 1) => 0x0A000004,
            ("UnityEngine.Object", "op_Implicit", 1) => 0x0A000005,
            ("UnityEngine.Object", "op_Inequality", 2) => 0x0A000006,
            ("UnityEngine.Component", "get_transform", 0) => 0x0A000008,
            ("UnityEngine.Component", "get_gameObject", 0) => 0x0A000015,
            ("UnityEngine.GameObject", "SetActive", 1) => 0x0A000016,
            ("UnityEngine.Camera", "set_orthographicSize", 1) => 0x0A000025,
            ("System.Collections.Generic.List`1<UnityEngine.GameObject>", "GetEnumerator", 0) => 0x0A0000FB,
            ("System.Collections.Generic.List`1/Enumerator<UnityEngine.GameObject>", "get_Current", 0) => 0x0A0000FC,
            ("System.Collections.Generic.List`1/Enumerator<UnityEngine.GameObject>", "MoveNext", 0) => 0x0A0000FD,
            ("System.Collections.Generic.List`1/Enumerator<UnityEngine.GameObject>", "Dispose", 0) => 0x0A000126,
            ("System.Collections.Generic.List`1<UnityEngine.GameObject>", "get_Item", 1) => 0x0A0001EC,
            _ => throw new InvalidDataException($"unmapped method operand {m.FullName}")
        };
    }

    if (p is CecilFieldReference f)
        throw new InvalidDataException($"unexpected external field operand {f.FullName}");
    if (p is CecilTypeReference t)
        throw new InvalidDataException($"unexpected type operand {t.FullName}");
    throw new InvalidDataException($"unmapped metadata operand {p.GetType().Name} {p}");
}

static (int Rva, int Offset, int NextRva, int NextOffset, int MetadataOffset, int MetadataSize) Layout(string path, int targetRid)
{
    using var fs = File.OpenRead(path);
    using var pe = new PEReader(fs);
    var md = pe.GetMetadataReader();
    int rva = md.GetMethodDefinition(MetadataTokens.MethodDefinitionHandle(targetRid)).RelativeVirtualAddress;
    int nextRva = Enumerable.Range(1, md.GetTableRowCount(TableIndex.MethodDef))
        .Select(r => md.GetMethodDefinition(MetadataTokens.MethodDefinitionHandle(r)).RelativeVirtualAddress)
        .Where(x => x > rva)
        .Min();
    var cor = pe.PEHeaders.CorHeader ?? throw new InvalidDataException("missing CLR header");
    int metadataOffset = RvaToOffset(pe.PEHeaders, cor.MetadataDirectory.RelativeVirtualAddress);
    return (rva, RvaToOffset(pe.PEHeaders, rva), nextRva, RvaToOffset(pe.PEHeaders, nextRva), metadataOffset, cor.MetadataDirectory.Size);
}

if (args.Length != 3)
{
    Console.Error.WriteLine("usage: Stage9AdministratorStartInPlacePatch <baseline.dll> <temporary-cecil.dll> <output.dll>");
    return 2;
}

var baseline = Path.GetFullPath(args[0]);
var temporary = Path.GetFullPath(args[1]);
var output = Path.GetFullPath(args[2]);
var baselineBytes = File.ReadAllBytes(baseline);
var temporaryBytes = File.ReadAllBytes(temporary);
if (Sha(baselineBytes) != ExpectedBaselineSha) throw new InvalidDataException("baseline SHA drift");

var baseLayout = Layout(baseline, (int)(TargetToken & 0x00FFFFFF));
var tempLayout = Layout(temporary, (int)(TargetToken & 0x00FFFFFF));
int temporaryBodyLength = MethodBodyLength(temporaryBytes, tempLayout.Offset);
int capacity = baseLayout.NextOffset - baseLayout.Offset;
if (temporaryBodyLength > capacity)
    throw new InvalidDataException($"body does not fit tempLen={temporaryBodyLength} capacity={capacity}");

var body = new byte[temporaryBodyLength];
Buffer.BlockCopy(temporaryBytes, tempLayout.Offset, body, 0, body.Length);
ushort bodyFlags = BitConverter.ToUInt16(body, 0);
int headerSize = ((bodyFlags >> 12) & 0xF) * 4;
uint oldTemporaryLocalSig = BitConverter.ToUInt32(body, 8);
BitConverter.GetBytes(ReusedLocalSigToken).CopyTo(body, 8);

using (var tempModule = CecilModule.ReadModule(temporary, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate }))
{
    var start = AllTypes(tempModule.Types).SelectMany(t => t.Methods).Single(m => Raw(m) == TargetToken);
    if (!start.HasBody || start.Body.Variables.Count != 3 || start.Body.ExceptionHandlers.Count != 1 || start.Body.ExceptionHandlers[0].HandlerType != ExceptionHandlerType.Finally)
        throw new InvalidDataException("temporary Start structural drift");
    string[] expectedLocals =
    {
        "System.Collections.Generic.List`1/Enumerator<UnityEngine.GameObject>",
        "UnityEngine.GameObject",
        "UnityEngine.GameObject"
    };
    if (!start.Body.Variables.Select(v => v.VariableType.FullName).SequenceEqual(expectedLocals))
        throw new InvalidDataException("temporary Start local shape drift");

    foreach (var instruction in start.Body.Instructions)
    {
        if (instruction.Operand is not IMetadataTokenProvider provider) continue;
        uint mapped = MapToken(provider);
        int operandPosition = headerSize + instruction.Offset + instruction.OpCode.Size;
        if (operandPosition < headerSize || operandPosition + 4 > body.Length)
            throw new InvalidDataException($"operand patch range invalid at IL_{instruction.Offset:X4}");
        BitConverter.GetBytes(mapped).CopyTo(body, operandPosition);
    }
}

var outputBytes = (byte[])baselineBytes.Clone();
Buffer.BlockCopy(body, 0, outputBytes, baseLayout.Offset, body.Length);
Array.Clear(outputBytes, baseLayout.Offset + body.Length, capacity - body.Length);

var diffs = Enumerable.Range(0, baselineBytes.Length).Where(i => baselineBytes[i] != outputBytes[i]).ToList();
if (diffs.Count == 0 || diffs.Any(i => i < baseLayout.Offset || i >= baseLayout.NextOffset))
    throw new InvalidDataException("byte diff escaped Administrator.Start allocation");

var baselineMetadata = baselineBytes.AsSpan(baseLayout.MetadataOffset, baseLayout.MetadataSize).ToArray();
var outputMetadata = outputBytes.AsSpan(baseLayout.MetadataOffset, baseLayout.MetadataSize).ToArray();
var baselineMetadataSha = Sha(baselineMetadata);
var outputMetadataSha = Sha(outputMetadata);
if (baselineMetadataSha != outputMetadataSha)
    throw new InvalidDataException("CLR metadata directory changed");

Directory.CreateDirectory(Path.GetDirectoryName(output)!);
File.WriteAllBytes(output, outputBytes);
Console.WriteLine($"INPLACE_PATCH_BYTES_PASS target=0x{TargetToken:X8} start_rva=0x{baseLayout.Rva:X} start_file=0x{baseLayout.Offset:X} next_rva=0x{baseLayout.NextRva:X} capacity={capacity} new_body_len={body.Length} old_temp_local_sig=0x{oldTemporaryLocalSig:X8} reused_local_sig=0x{ReusedLocalSigToken:X8} diff_bytes={diffs.Count}");
Console.WriteLine($"METADATA_DIRECTORY_UNCHANGED_PASS offset=0x{baseLayout.MetadataOffset:X} size={baseLayout.MetadataSize} sha256={baselineMetadataSha}");
Console.WriteLine($"OUTPUT_SHA256 {Sha(outputBytes)}");

using (var fs = File.OpenRead(output))
using (var pe = new PEReader(fs))
{
    var md = pe.GetMetadataReader();
    if (md.GetTableRowCount(TableIndex.MethodDef) != 2317 || md.GetTableRowCount(TableIndex.Field) != 2802)
        throw new InvalidDataException("metadata definition count drift");
    var def = md.GetMethodDefinition(MetadataTokens.MethodDefinitionHandle((int)(TargetToken & 0x00FFFFFF)));
    var methodBody = pe.GetMethodBody(def.RelativeVirtualAddress);
    if (MetadataTokens.GetToken(methodBody.LocalSignature) != ReusedLocalSigToken || methodBody.ExceptionRegions.Length != 1)
        throw new InvalidDataException("output body header drift");
    Console.WriteLine($"OUTPUT_BODY_PASS il={methodBody.GetILBytes().Length} maxstack={methodBody.MaxStack} localsig=0x{MetadataTokens.GetToken(methodBody.LocalSignature):X8} eh={methodBody.ExceptionRegions.Length}");
}

using (var outputModule = CecilModule.ReadModule(output, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate }))
{
    var start = AllTypes(outputModule.Types).SelectMany(t => t.Methods).Single(m => Raw(m) == TargetToken);
    if (start.Body.Variables.Count != 3 || start.Body.Variables[0].VariableType.FullName != "System.Collections.Generic.List`1/Enumerator<UnityEngine.GameObject>")
        throw new InvalidDataException("reopen local signature invalid");
    if (start.Body.Instructions.Any(i => i.Operand is VariableDefinition v && v.Index != 0))
        throw new InvalidDataException("unused compatibility locals unexpectedly referenced");
    var getEnumeratorInstruction = start.Body.Instructions.Single(i => i.Operand is CecilMethodReference m && m.DeclaringType.FullName == "System.Collections.Generic.List`1<UnityEngine.GameObject>" && m.Name == "GetEnumerator");
    var getEnumerator = (CecilMethodReference)getEnumeratorInstruction.Operand;
    if (getEnumeratorInstruction.OpCode.Code != Code.Callvirt || Raw(getEnumerator) != 0x0A0000FB)
        throw new InvalidDataException("GetEnumerator baseline token/opcode drift");
    Console.WriteLine($"REOPEN_INPLACE_START_PASS instructions={start.Body.Instructions.Count} locals={start.Body.Variables.Count} handlers={start.Body.ExceptionHandlers.Count} getenumerator=0x{Raw(getEnumerator):X8}");
}

Console.WriteLine("ADMINISTRATOR_START_INPLACE_STATIC_PASS=1");
return 0;
