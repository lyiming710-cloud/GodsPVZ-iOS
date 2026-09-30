using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

internal static class Program
{
    const uint TargetToken = 0x060007C9u;
    const string DonorTypeName = "TMPro.Examples.TMP_TextSelector_B_Donor";
    const int ExpectedTypes = 320, ExpectedMethods = 2317, ExpectedFields = 2802;

    static IEnumerable<TypeDefinition> Types(TypeDefinition t){yield return t;foreach(var n in t.NestedTypes)foreach(var x in Types(n))yield return x;}
    static IEnumerable<TypeDefinition> Types(ModuleDefinition m){foreach(var t in m.Types)foreach(var x in Types(t))yield return x;}
    static MethodDefinition Method(ModuleDefinition m,uint token)=>Types(m).SelectMany(t=>t.Methods).Single(x=>x.MetadataToken.ToUInt32()==token);

    static void Main(string[] args)
    {
        if(args.Length!=3) throw new ArgumentException("usage: PatcherNative13 <native12.dll> <donor.dll> <native13.dll>");
        var input=Path.GetFullPath(args[0]); var donorPath=Path.GetFullPath(args[1]); var output=Path.GetFullPath(args[2]);
        if(!File.Exists(input)) throw new FileNotFoundException(input);
        if(!File.Exists(donorPath)) throw new FileNotFoundException(donorPath);
        Directory.CreateDirectory(Path.GetDirectoryName(output)!);

        using var asm=AssemblyDefinition.ReadAssembly(input);
        using var donorAsm=AssemblyDefinition.ReadAssembly(donorPath);
        var mod=asm.MainModule; var mvid=mod.Mvid;
        CheckIdentity(mod,"input");
        var target=Method(mod,TargetToken);
        if(target.FullName!="System.Void TMPro.Examples.TMP_TextSelector_B::LateUpdate()")
            throw new InvalidOperationException("target identity mismatch: "+target.FullName);
        var donorType=Types(donorAsm.MainModule).Single(x=>x.FullName==DonorTypeName);
        var donor=donorType.Methods.Single(x=>x.Name=="LateUpdate"&&x.Parameters.Count==0&&x.ReturnType.FullName=="System.Void");
        if(!donor.HasBody) throw new InvalidOperationException("donor has no body");

        var before=Snapshot(mod);
        Transplant(mod,target,donor,donorType);
        CheckIdentity(mod,"memory"); CheckNonTargets(mod,before,"memory");
        asm.Write(output);

        using var reopened=AssemblyDefinition.ReadAssembly(output);
        if(reopened.MainModule.Mvid!=mvid) throw new InvalidOperationException($"MVID changed {mvid}->{reopened.MainModule.Mvid}");
        CheckIdentity(reopened.MainModule,"reopened"); CheckNonTargets(reopened.MainModule,before,"reopened");
        var t=Method(reopened.MainModule,TargetToken);
        Console.WriteLine($"NATIVE13_PATCH_PASS input_mvid={mvid} output_mvid={reopened.MainModule.Mvid}");
        Console.WriteLine($"TARGET token=0x{TargetToken:X8} {t.FullName} code_size={t.Body.CodeSize} il={t.Body.Instructions.Count} locals={t.Body.Variables.Count} eh={t.Body.ExceptionHandlers.Count}");
    }

    static void CheckIdentity(ModuleDefinition m,string stage)
    {
        var ts=Types(m).ToArray(); var mc=ts.Sum(t=>t.Methods.Count); var fc=ts.Sum(t=>t.Fields.Count);
        if(ts.Length!=ExpectedTypes||mc!=ExpectedMethods||fc!=ExpectedFields)
            throw new InvalidOperationException($"{stage}: counts {ts.Length}/{mc}/{fc}");
    }

    static Dictionary<uint,string> Snapshot(ModuleDefinition m)=>Types(m).SelectMany(t=>t.Methods)
        .Where(x=>x.HasBody&&x.MetadataToken.ToUInt32()!=TargetToken)
        .ToDictionary(x=>x.MetadataToken.ToUInt32(),Fingerprint);

    static void CheckNonTargets(ModuleDefinition m,Dictionary<uint,string> before,string stage)
    {
        var now=Snapshot(m); if(now.Count!=before.Count) throw new InvalidOperationException($"{stage}: non-target count changed");
        foreach(var kv in before) if(!now.TryGetValue(kv.Key,out var v)||v!=kv.Value)
            throw new InvalidOperationException($"{stage}: non-target changed 0x{kv.Key:X8}");
        Console.WriteLine($"NON_TARGET_ISOLATION_PASS stage={stage} methods={before.Count}");
    }

    static string StableOp(Instruction i){var n=i.OpCode.Code.ToString();if(i.Operand is Instruction||i.Operand is Instruction[])return n.EndsWith("_S",StringComparison.Ordinal)?n[..^2]:n;return n;}
    static int Idx(MethodDefinition m,Instruction i)=>i==null?-1:m.Body.Instructions.IndexOf(i);
    static string Fingerprint(MethodDefinition m)
    {
        var b=new StringBuilder().Append(m.Body.InitLocals).Append('|').Append(m.Body.MaxStackSize).Append('|');
        foreach(var v in m.Body.Variables)b.Append("V:").Append(v.VariableType.FullName).Append(';');
        foreach(var i in m.Body.Instructions){b.Append(StableOp(i)).Append(':');switch(i.Operand){case null:break;case Instruction x:b.Append('@').Append(Idx(m,x));break;case Instruction[] xs:foreach(var x in xs)b.Append('@').Append(Idx(m,x)).Append(',');break;case VariableDefinition v:b.Append('V').Append(v.Index).Append(':').Append(v.VariableType.FullName);break;case ParameterDefinition p:b.Append('P').Append(p.Index).Append(':').Append(p.ParameterType.FullName);break;case TypeReference tr:b.Append('T').Append(tr.FullName).Append('@').Append(tr.Scope?.Name);break;case MemberReference mr:b.Append('M').Append(mr.FullName).Append('@').Append(mr.DeclaringType?.Scope?.Name);break;default:b.Append(i.Operand);break;}b.Append(';');}
        foreach(var e in m.Body.ExceptionHandlers)b.Append("EH:").Append(e.HandlerType).Append(':').Append(Idx(m,e.TryStart)).Append(':').Append(Idx(m,e.TryEnd)).Append(':').Append(Idx(m,e.HandlerStart)).Append(':').Append(Idx(m,e.HandlerEnd)).Append(':').Append(Idx(m,e.FilterStart)).Append(':').Append(e.CatchType?.FullName).Append(';');
        return b.ToString();
    }

    static FieldReference MapField(ModuleDefinition mod, MethodDefinition target, TypeDefinition donorType, FieldReference f)
    {
        if(f.DeclaringType.FullName==donorType.FullName)
            return target.DeclaringType.Fields.Single(x=>x.Name==f.Name);
        return mod.ImportReference(f);
    }

    static MethodReference MapMethod(ModuleDefinition mod, MethodDefinition target, TypeDefinition donorType, MethodReference m)
    {
        if(m.DeclaringType.FullName==donorType.FullName)
        {
            var hits=target.DeclaringType.Methods.Where(x=>x.Name==m.Name&&x.Parameters.Count==m.Parameters.Count).ToArray();
            if(hits.Length!=1) throw new InvalidOperationException($"self method map ambiguous: {m.FullName} hits={hits.Length}");
            return hits[0];
        }
        return mod.ImportReference(m);
    }

    static CallSite MapCallSite(ModuleDefinition mod, CallSite cs)
    {
        var mapped=new CallSite(mod.ImportReference(cs.ReturnType))
        {
            CallingConvention=cs.CallingConvention,
            HasThis=cs.HasThis,
            ExplicitThis=cs.ExplicitThis
        };
        foreach(var p in cs.Parameters)
            mapped.Parameters.Add(new ParameterDefinition(p.Name,p.Attributes,mod.ImportReference(p.ParameterType)));
        return mapped;
    }

    static Instruction CloneSkeleton(ModuleDefinition mod, MethodDefinition target, TypeDefinition donorType,
        Instruction src, Dictionary<VariableDefinition,VariableDefinition> vars, Dictionary<ParameterDefinition,ParameterDefinition> pars,
        Instruction placeholder)
    {
        var op=src.OpCode; var o=src.Operand;
        if(o==null) return Instruction.Create(op);
        if(o is Instruction) return Instruction.Create(op,placeholder);
        if(o is Instruction[]) return Instruction.Create(op,new[]{placeholder});
        if(o is VariableDefinition vd) return Instruction.Create(op,vars[vd]);
        if(o is ParameterDefinition pd) return Instruction.Create(op,pars[pd]);
        if(o is FieldReference fr) return Instruction.Create(op,MapField(mod,target,donorType,fr));
        if(o is MethodReference mr) return Instruction.Create(op,MapMethod(mod,target,donorType,mr));
        if(o is TypeReference tr) return Instruction.Create(op,mod.ImportReference(tr));
        if(o is CallSite cs) return Instruction.Create(op,MapCallSite(mod,cs));
        if(o is string s) return Instruction.Create(op,s);
        if(o is sbyte sb) return Instruction.Create(op,sb);
        if(o is byte by) return Instruction.Create(op,(sbyte)by);
        if(o is int iv) return Instruction.Create(op,iv);
        if(o is long lv) return Instruction.Create(op,lv);
        if(o is float fv) return Instruction.Create(op,fv);
        if(o is double dv) return Instruction.Create(op,dv);
        throw new NotSupportedException($"unsupported donor operand {o.GetType().FullName}: {o}");
    }

    static void Transplant(ModuleDefinition mod, MethodDefinition target, MethodDefinition donor, TypeDefinition donorType)
    {
        var nb=new MethodBody(target){InitLocals=donor.Body.InitLocals,MaxStackSize=donor.Body.MaxStackSize};
        target.Body=nb;
        var vars=new Dictionary<VariableDefinition,VariableDefinition>();
        foreach(var v in donor.Body.Variables){var nv=new VariableDefinition(mod.ImportReference(v.VariableType));nb.Variables.Add(nv);vars[v]=nv;}
        var pars=new Dictionary<ParameterDefinition,ParameterDefinition>();
        for(int i=0;i<donor.Parameters.Count;i++) pars[donor.Parameters[i]]=target.Parameters[i];
        var il=nb.GetILProcessor();
        var map=new Dictionary<Instruction,Instruction>();
        var placeholder=Instruction.Create(OpCodes.Nop);
        foreach(var s in donor.Body.Instructions){var d=CloneSkeleton(mod,target,donorType,s,vars,pars,placeholder);map[s]=d;il.Append(d);}
        foreach(var s in donor.Body.Instructions)
        {
            var d=map[s];
            if(s.Operand is Instruction bi)d.Operand=map[bi];
            else if(s.Operand is Instruction[] sw)d.Operand=sw.Select(x=>map[x]).ToArray();
        }
        foreach(var e in donor.Body.ExceptionHandlers)
        {
            var ne=new ExceptionHandler(e.HandlerType){TryStart=map[e.TryStart],TryEnd=e.TryEnd==null?null:map[e.TryEnd],HandlerStart=map[e.HandlerStart],HandlerEnd=e.HandlerEnd==null?null:map[e.HandlerEnd],FilterStart=e.FilterStart==null?null:map[e.FilterStart],CatchType=e.CatchType==null?null:mod.ImportReference(e.CatchType)};
            nb.ExceptionHandlers.Add(ne);
        }
    }
}
