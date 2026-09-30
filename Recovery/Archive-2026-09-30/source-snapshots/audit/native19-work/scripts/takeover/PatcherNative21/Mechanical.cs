using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.Json;
using Mono.Cecil;
using Mono.Cecil.Cil;

internal static class Native21Entry
{
    public static void Main(string[] args) => Program.RunIntegerLocals(args);
}

internal static partial class Program
{
    public static void RunIntegerLocals(string[] args)
    {
        if(args.Length!=4)throw new ArgumentException("input output manifest linked|unlinked");
        string expected=args[3] switch {
            "linked"=>"f561d48a353f1747b77260f9db69acabc1bb4860f46d4ffc447d81764c55efa9",
            "unlinked"=>"7d2b3661f3e4aa3ba27b986c233d65c920e4c439851eae1788ae58323cc1d910",
            _=>throw new ArgumentException("unknown kind")};
        string Hash(string path)=>Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path))).ToLowerInvariant();
        if(Hash(args[0])!=expected)throw new Exception("Native20 input hash mismatch");
        if(Hash(args[2])!="0e74832cb1f63d68acf182beff071e6bb4f58b466e05583fd9e64ad9f7c07cc5")throw new Exception("proposal hash drift");
        using var manifest=JsonDocument.Parse(File.ReadAllText(args[2]));
        var proposals=manifest.RootElement.GetProperty("eligible").EnumerateArray().ToArray();
        if(proposals.Length!=38||proposals.Sum(p=>p.GetProperty("changes").GetArrayLength())!=9||proposals.Sum(p=>p.GetProperty("locals").GetArrayLength())!=90)throw new Exception("proposal count drift");
        using var resolver=new LockedResolver(new[]{Path.GetDirectoryName(Path.GetFullPath(args[0])),Environment.GetEnvironmentVariable("GODSPVZ_RESOLVER")}.Where(s=>!string.IsNullOrEmpty(s)).ToArray());
        using var asm=AssemblyDefinition.ReadAssembly(args[0],new ReaderParameters{AssemblyResolver=resolver});
        var mod=asm.MainModule;CheckIdentity(mod,"input");var mvid=mod.Mvid;
        var targets=proposals.Select(p=>(MethodDefinition)mod.LookupToken(Convert.ToInt32(p.GetProperty("token").GetString()[2..],16))).ToArray();
        if(targets.Distinct().Count()!=38)throw new Exception("duplicate target");
        var before=Snapshot(mod,targets.ToHashSet());var expectedBodies=new Dictionary<uint,string>();
        ExportTargets(mod,targets,args[1]+".before.json");
        var maps=new List<object>();
        for(int n=0;n<targets.Length;n++){
            var method=targets[n];var proposal=proposals[n];
            if(method.FullName!=proposal.GetProperty("method").GetString())throw new Exception("method identity drift");
            foreach(var local in proposal.GetProperty("locals").EnumerateArray()){
                var variable=method.Body.Variables[local.GetProperty("index").GetInt32()];
                if(variable.VariableType.FullName!="System.Object"||local.GetProperty("before").GetString()!="System.Object")throw new Exception("local input type drift");
                variable.VariableType=local.GetProperty("after").GetString() switch {
                    "System.Int32"=>mod.TypeSystem.Int32,
                    "System.Int64"=>mod.TypeSystem.Int64,
                    _=>throw new Exception("unsupported inferred local type")};
            }
            foreach(var change in proposal.GetProperty("changes").EnumerateArray()){
                var prev=change.GetProperty("before");var next=change.GetProperty("after");
                int offset=change.GetProperty("offset").GetInt32();
                var ins=method.Body.Instructions.Single(i=>i.Offset==offset);
                if(ins.OpCode.Name!=prev.GetProperty("opcode").GetString())throw new Exception("opcode drift");
                string replacement=next.GetProperty("opcode").GetString();
                if(replacement=="ldnull"||replacement=="ldc.r4"){
                    if(!(ins.OpCode==OpCodes.Ldc_I4_0||((ins.OpCode==OpCodes.Ldc_I4||ins.OpCode==OpCodes.Ldc_I4_S)&&Convert.ToInt32(ins.Operand)==0)))throw new Exception("expected integer zero");
                    if(ins.Next==null||!new[]{Code.Ceq,Code.Cgt,Code.Cgt_Un,Code.Clt,Code.Clt_Un}.Contains(ins.Next.OpCode.Code))throw new Exception("expected comparison");
                    if(replacement=="ldnull"&&!new[]{Code.Ceq,Code.Cgt_Un}.Contains(ins.Next.OpCode.Code))throw new Exception("invalid reference comparison");
                    ins.OpCode=replacement=="ldnull"?OpCodes.Ldnull:OpCodes.Ldc_R4;
                    ins.Operand=replacement=="ldnull"?null:(object)0f;
                }else{
                    string type;
                    if(replacement=="ldloc"&&ins.Operand is VariableDefinition v&&(ins.OpCode==OpCodes.Ldloca||ins.OpCode==OpCodes.Ldloca_S)){
                        if(v.Index!=prev.GetProperty("operand").GetProperty("index").GetInt32())throw new Exception("local index drift");
                        type=v.VariableType.FullName;ins.OpCode=OpCodes.Ldloc;
                    }else if(replacement=="ldfld"&&ins.Operand is FieldReference f&&ins.OpCode==OpCodes.Ldflda){
                        if(f.FullName!=prev.GetProperty("operand").GetProperty("identity").GetString())throw new Exception("field identity drift");
                        type=f.FieldType.FullName;ins.OpCode=OpCodes.Ldfld;
                    }else throw new Exception("unsupported proposal");
                    if(!new[]{"UnityEngine.Vector2","UnityEngine.Vector3","UnityEngine.Vector4","UnityEngine.Quaternion","UnityEngine.Color"}.Contains(type))throw new Exception("unsupported value type");
                    if(ins.Next?.Operand is not MethodReference call||call.Parameters.Count==0||call.Parameters.Last().ParameterType.FullName!=type)throw new Exception("by-value signature drift");
                }
                maps.Add(new{token=proposal.GetProperty("token").GetString(),index=method.Body.Instructions.IndexOf(ins),original_offset=offset,before=prev.Clone(),after=next.Clone()});
            }
            // Enlarged constants and ldloc operands can overflow short branches.
            // Keep the same Instruction objects so all branch and EH identities survive.
            var longBranches=typeof(OpCodes).GetFields().Where(f=>f.FieldType==typeof(OpCode)).Select(f=>(OpCode)f.GetValue(null)).Where(o=>o.OperandType==OperandType.InlineBrTarget).ToDictionary(o=>o.Name);
            foreach(var ins in method.Body.Instructions.Where(i=>i.OpCode.OperandType==OperandType.ShortInlineBrTarget))ins.OpCode=longBranches[ins.OpCode.Name[..^2]];
            expectedBodies[method.MetadataToken.ToUInt32()]=Fingerprint(method);
        }
        CheckNonTargets(mod,targets.ToHashSet(),before,"memory");CheckIdentity(mod,"memory");
        asm.Write(args[1]);
        using var reopened=AssemblyDefinition.ReadAssembly(args[1],new ReaderParameters{AssemblyResolver=resolver});
        var rm=reopened.MainModule;CheckIdentity(rm,"reopened");if(rm.Mvid!=mvid)throw new Exception("MVID changed");
        var rt=targets.Select(m=>(MethodDefinition)rm.LookupToken(m.MetadataToken)).ToArray();
        CheckNonTargets(rm,rt.ToHashSet(),before,"reopened");
        foreach(var method in rt)if(Fingerprint(method)!=expectedBodies[method.MetadataToken.ToUInt32()])throw new Exception("target changed unexpectedly after write: "+method.FullName);
        ExportTargets(rm,rt,args[1]+".targets.json");
        File.WriteAllText(args[1]+".edits.json",JsonSerializer.Serialize(maps,new JsonSerializerOptions{WriteIndented=true}));
        Console.WriteLine($"NATIVE21_PATCH_PASS targets={rt.Length} edits={maps.Count} normalized_non_targets={before.Count} sha256={Hash(args[1])}");
    }
}
