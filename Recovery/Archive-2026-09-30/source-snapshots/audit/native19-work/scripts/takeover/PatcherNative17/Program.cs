using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

internal static class Program
{
    const uint TargetToken = 0x060008EFu;
    const int ExpectedTypes = 320, ExpectedMethods = 2317, ExpectedFields = 2802;

    static void Main(string[] args)
    {
        if(args.Length < 2 || args.Length > 3) throw new ArgumentException("usage: PatcherNative17 <native16.dll> <native17.dll> [linked]");
        bool fixture=args.Length==3 && args[2]=="fixture";
        bool linked=args.Length==3 && (args[2]=="linked"||fixture);
        if(args.Length==3&&!linked)throw new ArgumentException("unknown mode");
        var input=Path.GetFullPath(args[0]); var output=Path.GetFullPath(args[1]);
        if(!File.Exists(input)) throw new FileNotFoundException(input);
        Directory.CreateDirectory(Path.GetDirectoryName(output)!);
        var resolver=new LockedResolver(new[]{Path.GetDirectoryName(input)!,Path.Combine(Directory.GetCurrentDirectory(),"Tools/Stage9Native4Recovery/resolver")}.Concat(fixture?new[]{Path.GetDirectoryName(typeof(object).Assembly.Location)!}:new[]{Environment.GetEnvironmentVariable("GODSPVZ_RESOLVER")}).Where(x=>!string.IsNullOrEmpty(x)).ToArray());
        // Roslyn orders nested fixture types differently from Cecil. Normalize
        // only the synthetic fixture once, before recording token-keyed snapshots.
        if(fixture){using var normalized=AssemblyDefinition.ReadAssembly(input,new ReaderParameters{AssemblyResolver=resolver});normalized.Write(input+".normalized");input+=".normalized";}
        using var asm=AssemblyDefinition.ReadAssembly(input,new ReaderParameters{AssemblyResolver=resolver}); var mod=asm.MainModule; var mvid=mod.Mvid;
        if(!linked) CheckIdentity(mod,"input");
        var target=FindTarget(mod,linked); var before=Snapshot(mod,target);
        var targets=Targets(mod).ToArray();
        foreach(var t in targets){var gp=t.HasGenericParameters?t.GenericParameters[0]:t.DeclaringType.GenericParameters[0];if(gp.HasConstraints||gp.Attributes!=GenericParameterAttributes.NonVariant)throw new InvalidOperationException("generic constraint changed");}
        PatchAssign(mod,target);
        foreach(var t in targets){StackCheck(t);var first=t.Body.Instructions[0];t.Body.Instructions.RemoveAt(0);bool rejected=false;try{StackCheck(t);}catch(InvalidOperationException){rejected=true;}finally{t.Body.Instructions.Insert(0,first);}if(!rejected)throw new InvalidOperationException("stack negative control accepted");}
        Console.WriteLine("STACK_FLOW_PASS targets=1 negative_controls=1");
        if(!fixture&&mod.AssemblyReferences.Any(x=>x.Name=="System.Private.CoreLib"))throw new InvalidOperationException("host framework reference leaked into Unity candidate");
        if(!linked) CheckIdentity(mod,"memory"); CheckNonTargets(mod,target,before,"memory");
        asm.Write(output);
        using var reopened=AssemblyDefinition.ReadAssembly(output);
        if(reopened.MainModule.Mvid!=mvid) throw new InvalidOperationException($"MVID changed {mvid}->{reopened.MainModule.Mvid}");
        if(!linked) CheckIdentity(reopened.MainModule,"reopened");
        var rt=FindTarget(reopened.MainModule,linked); CheckNonTargets(reopened.MainModule,rt,before,"reopened");
        foreach(var t in Targets(reopened.MainModule)){StackCheck(t);if(t.Body.ExceptionHandlers.Count!=0)throw new InvalidOperationException("unexpected EH");}
        Console.WriteLine($"NATIVE17_PATCH_PASS linked={linked} input_mvid={mvid} output_mvid={reopened.MainModule.Mvid}");
        Console.WriteLine($"TARGET token=0x{rt.MetadataToken.ToUInt32():X8} {rt.FullName} code_size={rt.Body.CodeSize} il={rt.Body.Instructions.Count} locals={rt.Body.Variables.Count} eh={rt.Body.ExceptionHandlers.Count} gp={rt.GenericParameters.Count}");
    }

    static IEnumerable<TypeDefinition> Types(TypeDefinition t){yield return t;foreach(var n in t.NestedTypes)foreach(var x in Types(n))yield return x;}
    static void StackCheck(MethodDefinition m){
        var seen=new Dictionary<Instruction,int>();var queue=new Queue<(Instruction,int)>();queue.Enqueue((m.Body.Instructions[0],0));
        foreach(var e in m.Body.ExceptionHandlers)queue.Enqueue((e.HandlerStart,e.HandlerType==ExceptionHandlerType.Catch?1:0));
        while(queue.Count>0){var (i,depth)=queue.Dequeue();if(i==null)throw new InvalidOperationException("fallthrough past body");if(seen.TryGetValue(i,out var old)){if(old!=depth)throw new InvalidOperationException("stack merge mismatch");continue;}seen[i]=depth;
            int pop=i.OpCode.StackBehaviourPop==StackBehaviour.Pop0?0:i.OpCode.StackBehaviourPop.ToString().Split('_').Length;
            int push=i.OpCode.StackBehaviourPush==StackBehaviour.Push0?0:i.OpCode.StackBehaviourPush.ToString().Split('_').Length;
            if(i.Operand is MethodReference mr){pop=mr.Parameters.Count+(i.OpCode==OpCodes.Newobj?0:mr.HasThis?1:0);push=i.OpCode==OpCodes.Newobj||mr.ReturnType.MetadataType!=MetadataType.Void?1:0;}
            if(i.OpCode==OpCodes.Ret){pop=m.ReturnType.MetadataType==MetadataType.Void?0:1;push=0;}
            if(i.OpCode==OpCodes.Leave||i.OpCode==OpCodes.Leave_S){pop=depth;push=0;}
            if(depth<pop)throw new InvalidOperationException("stack underflow "+m.FullName+" "+i);int next=depth-pop+push;
            if(i.OpCode==OpCodes.Ret||i.OpCode==OpCodes.Endfinally){if(next!=0)throw new InvalidOperationException("nonempty terminal stack");continue;}
            if(i.OpCode==OpCodes.Leave||i.OpCode==OpCodes.Leave_S){if(depth!=0)throw new InvalidOperationException("nonempty leave stack");next=0;}
            if(i.Operand is Instruction target)queue.Enqueue((target,next));
            if(i.Operand is Instruction[] targets)foreach(var target2 in targets)queue.Enqueue((target2,next));
            if(i.OpCode.FlowControl!=FlowControl.Branch&&i.OpCode.FlowControl!=FlowControl.Throw)queue.Enqueue((i.Next,next));
        }
    }
    sealed class LockedResolver:IAssemblyResolver {
        readonly string[] dirs;readonly Dictionary<string,AssemblyDefinition> cache=new();
        public LockedResolver(string[] d){dirs=d;}
        public AssemblyDefinition Resolve(AssemblyNameReference n)=>Resolve(n,new ReaderParameters());
        public AssemblyDefinition Resolve(AssemblyNameReference n,ReaderParameters p){
            if(cache.TryGetValue(n.Name,out var a))return a;
            var file=dirs.Select(d=>Path.Combine(d,n.Name+".dll")).FirstOrDefault(File.Exists);
            if(file==null)throw new AssemblyResolutionException(n);
            p.AssemblyResolver=this;a=AssemblyDefinition.ReadAssembly(file,p);cache[n.Name]=a;return a;
        }
        public void Dispose(){foreach(var a in cache.Values)a.Dispose();}
    }
    static IEnumerable<TypeDefinition> Types(ModuleDefinition m){foreach(var t in m.Types)foreach(var x in Types(t))yield return x;}
    static MethodDefinition FindTarget(ModuleDefinition m,bool linked)
    {
        var t=Types(m).Single(x=>x.FullName=="FTRuntime.Internal.SwfList`1");
        var md=t.Methods.Single(x=>x.Name=="AssignTo"&&x.Parameters.Count==1&&x.Parameters[0].ParameterType.FullName=="System.Collections.Generic.List`1<T>");
        if(!linked && md.MetadataToken.ToUInt32()!=TargetToken)throw new InvalidOperationException($"target token mismatch 0x{md.MetadataToken.ToUInt32():X8}");
        if(!md.HasBody)throw new InvalidOperationException("target body missing");
        if(md.IsStatic)throw new InvalidOperationException("Sorting target unexpectedly static");
        return md;
    }
    static void CheckIdentity(ModuleDefinition m,string stage){var ts=Types(m).ToArray();int mc=ts.Sum(t=>t.Methods.Count),fc=ts.Sum(t=>t.Fields.Count);if(ts.Length!=ExpectedTypes||mc!=ExpectedMethods||fc!=ExpectedFields)throw new InvalidOperationException($"{stage}: counts {ts.Length}/{mc}/{fc}");}
    static Dictionary<uint,string> Snapshot(ModuleDefinition m,MethodDefinition target)=>Types(m).SelectMany(t=>t.Methods).Where(x=>x.HasBody&&!Targets(m).Contains(x)).ToDictionary(x=>x.MetadataToken.ToUInt32(),Fingerprint);
    static void CheckNonTargets(ModuleDefinition m,MethodDefinition target,Dictionary<uint,string> before,string stage){
        var now=Snapshot(m,target);if(now.Count!=before.Count)throw new InvalidOperationException($"{stage}: non-target count {before.Count}->{now.Count}");
        foreach(var kv in before)if(!now.TryGetValue(kv.Key,out var v)||v!=kv.Value){
            int at=0;while(v!=null&&at<Math.Min(v.Length,kv.Value.Length)&&v[at]==kv.Value[at])at++;
            throw new InvalidOperationException($"{stage}: non-target changed 0x{kv.Key:X8} at={at} before={kv.Value.Substring(Math.Max(0,at-25),Math.Min(120,kv.Value.Length-Math.Max(0,at-25)))} after={v?.Substring(Math.Max(0,at-25),Math.Min(120,v.Length-Math.Max(0,at-25)))}");
        }
        Console.WriteLine($"NON_TARGET_ISOLATION_PASS stage={stage} methods={before.Count}");
    }
    static string StableOp(Instruction i){var n=i.OpCode.Code.ToString();if((i.Operand is Instruction||i.Operand is Instruction[])&&n.EndsWith("_S",StringComparison.Ordinal))return n[..^2];return n;}
    static int Ix(MethodDefinition m,Instruction i)=>i==null?-1:m.Body.Instructions.IndexOf(i);
    static string Fingerprint(MethodDefinition m)
    {
        var b=new StringBuilder().Append(m.Body.InitLocals).Append('|').Append(m.Body.MaxStackSize).Append('|');
        foreach(var v in m.Body.Variables)b.Append("V:").Append(v.VariableType.FullName).Append(';');
        foreach(var i in m.Body.Instructions){b.Append(StableOp(i)).Append(':');switch(i.Operand){case null:break;case Instruction x:b.Append('@').Append(Ix(m,x));break;case Instruction[] xs:foreach(var x in xs)b.Append('@').Append(Ix(m,x)).Append(',');break;case VariableDefinition v:b.Append('V').Append(v.Index).Append(':').Append(v.VariableType.FullName);break;case ParameterDefinition p:b.Append('P').Append(p.Index).Append(':').Append(p.ParameterType.FullName);break;case MemberReference mr:b.Append('M').Append(mr.FullName).Append('@').Append(mr.DeclaringType?.Scope?.Name);break;default:b.Append(i.Operand);break;}b.Append(';');}
        foreach(var e in m.Body.ExceptionHandlers)b.Append("EH:").Append(e.HandlerType).Append(':').Append(Ix(m,e.TryStart)).Append(':').Append(Ix(m,e.TryEnd)).Append(':').Append(Ix(m,e.HandlerStart)).Append(':').Append(Ix(m,e.HandlerEnd)).Append(':').Append(Ix(m,e.FilterStart)).Append(':').Append(e.CatchType?.FullName).Append(';');
        return b.ToString();
    }
    static MethodReference MR(ModuleDefinition m,string decl,string name,int argc,string ret=null)
    {
        var q=m.GetMemberReferences().OfType<MethodReference>().Concat(Types(m).SelectMany(t=>t.Methods)).Where(x=>x.DeclaringType.FullName==decl&&x.Name==name&&x.Parameters.Count==argc);
        if(ret!=null)q=q.Where(x=>x.ReturnType.FullName==ret);
        return q.First();
    }

    static IEnumerable<MethodDefinition> Targets(ModuleDefinition m)=>new[]{FindTarget(m,true)};
    static TypeDefinition TD(ModuleDefinition m,string n)=>Types(m).Single(x=>x.Name==n&&x.Namespace=="");
    static FieldDefinition FD(TypeDefinition t,string n)=>t.Fields.Single(x=>x.Name==n);
    static void Reset(MethodDefinition m){m.Body=new MethodBody(m){InitLocals=true,MaxStackSize=8};}
    static VariableDefinition Local(MethodDefinition m,TypeReference t){var v=new VariableDefinition(t);m.Body.Variables.Add(v);return v;}
    static MethodReference Host(ModuleDefinition mod,TypeReference host,string name,int argc){
        var d=host.Resolve().Methods.Single(x=>x.Name==name&&x.Parameters.Count==argc);
        var imported=mod.ImportReference(d);
        var r=new MethodReference(name,imported.ReturnType,host){HasThis=d.HasThis,ExplicitThis=d.ExplicitThis,CallingConvention=d.CallingConvention};
        foreach(var a in imported.Parameters)r.Parameters.Add(new ParameterDefinition(a.ParameterType));
        return r;
    }
    static FieldReference GF(MethodDefinition m,string name){
        var self=new GenericInstanceType(m.DeclaringType);self.GenericArguments.Add(m.DeclaringType.GenericParameters[0]);
        var f=FD(m.DeclaringType,name);return new FieldReference(f.Name,f.FieldType,self);
    }
    static void PatchAssign(ModuleDefinition mod,MethodDefinition m){
        var list=m.Parameters[0].ParameterType;var clear=Host(mod,list,"Clear",0);var capacity=Host(mod,list,"get_Capacity",0);var setCapacity=Host(mod,list,"set_Capacity",1);var add=Host(mod,list,"Add",1);
        var data=GF(m,"_data");var size=GF(m,"_size");var gp=m.DeclaringType.GenericParameters[0];var exc=MR(mod,"System.IndexOutOfRangeException",".ctor",0);
        Reset(m);var count=Local(m,mod.TypeSystem.Int32);var index=Local(m,mod.TypeSystem.Int32);var il=m.Body.GetILProcessor();
        var ready=il.Create(OpCodes.Ldarg_0);var test=il.Create(OpCodes.Ldloc,index);var body=il.Create(OpCodes.Ldloc,index);var read=il.Create(OpCodes.Ldarg_1);
        il.Emit(OpCodes.Ldarg_1);il.Emit(OpCodes.Callvirt,clear);
        il.Emit(OpCodes.Ldarg_1);il.Emit(OpCodes.Callvirt,capacity);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,size);il.Emit(OpCodes.Bge,ready);
        il.Emit(OpCodes.Ldarg_1);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,size);il.Emit(OpCodes.Ldc_I4_2);il.Emit(OpCodes.Mul);il.Emit(OpCodes.Callvirt,setCapacity);
        il.Append(ready);il.Emit(OpCodes.Ldfld,size);il.Emit(OpCodes.Stloc,count);il.Emit(OpCodes.Ldc_I4_0);il.Emit(OpCodes.Stloc,index);il.Emit(OpCodes.Br,test);
        // Native inlines the indexer's unsigned size guard. Preserve it here
        // rather than calling the still-damaged managed indexer.
        il.Append(body);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,size);il.Emit(OpCodes.Blt_Un,read);il.Emit(OpCodes.Newobj,exc);il.Emit(OpCodes.Throw);
        il.Append(read);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,data);il.Emit(OpCodes.Ldloc,index);il.Emit(OpCodes.Ldelem_Any,gp);il.Emit(OpCodes.Callvirt,add);
        il.Emit(OpCodes.Ldloc,index);il.Emit(OpCodes.Ldc_I4_1);il.Emit(OpCodes.Add);il.Emit(OpCodes.Stloc,index);
        il.Append(test);il.Emit(OpCodes.Ldloc,count);il.Emit(OpCodes.Blt,body);il.Emit(OpCodes.Ret);
    }
}
