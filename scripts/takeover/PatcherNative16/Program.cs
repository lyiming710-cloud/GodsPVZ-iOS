using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

internal static class Program
{
    const uint TargetToken = 0x060000C6u;
    const int ExpectedTypes = 320, ExpectedMethods = 2317, ExpectedFields = 2802;

    static void Main(string[] args)
    {
        if(args.Length < 2 || args.Length > 3) throw new ArgumentException("usage: PatcherNative16 <native15.dll> <native16.dll> [linked]");
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
        if(!linked && (FindRemove(mod).MetadataToken.ToUInt32()!=0x060008D9 || FindUnordered(mod).MetadataToken.ToUInt32()!=0x060008E9))throw new InvalidOperationException("dependency token mismatch");
        foreach(var t in targets){var gp=t.HasGenericParameters?t.GenericParameters[0]:t.DeclaringType.GenericParameters[0];if(gp.HasConstraints||gp.Attributes!=GenericParameterAttributes.NonVariant)throw new InvalidOperationException("generic constraint changed");}
        PatchSorting(mod,target);PatchRemove(mod,FindRemove(mod));PatchUnordered(mod,FindUnordered(mod));
        foreach(var t in targets){StackCheck(t);var first=t.Body.Instructions[0];t.Body.Instructions.RemoveAt(0);bool rejected=false;try{StackCheck(t);}catch(InvalidOperationException){rejected=true;}finally{t.Body.Instructions.Insert(0,first);}if(!rejected)throw new InvalidOperationException("stack negative control accepted");}
        Console.WriteLine("STACK_FLOW_PASS targets=3 negative_controls=3");
        if(!fixture&&mod.AssemblyReferences.Any(x=>x.Name=="System.Private.CoreLib"))throw new InvalidOperationException("host framework reference leaked into Unity candidate");
        if(!linked) CheckIdentity(mod,"memory"); CheckNonTargets(mod,target,before,"memory");
        asm.Write(output);
        using var reopened=AssemblyDefinition.ReadAssembly(output);
        if(reopened.MainModule.Mvid!=mvid) throw new InvalidOperationException($"MVID changed {mvid}->{reopened.MainModule.Mvid}");
        if(!linked) CheckIdentity(reopened.MainModule,"reopened");
        var rt=FindTarget(reopened.MainModule,linked); CheckNonTargets(reopened.MainModule,rt,before,"reopened");
        foreach(var t in Targets(reopened.MainModule)){StackCheck(t);if(t.Body.ExceptionHandlers.Count!=0)throw new InvalidOperationException("unexpected EH");}
        Console.WriteLine($"NATIVE16_PATCH_PASS linked={linked} input_mvid={mvid} output_mvid={reopened.MainModule.Mvid}");
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
        var t=Types(m).Single(x=>x.Name=="VFXAnimationEvent"&&x.Namespace=="");
        var md=t.Methods.Single(x=>x.Name=="SetSorting"&&x.GenericParameters.Count==1&&x.Parameters.Count==2);
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

    static MethodDefinition FindRemove(ModuleDefinition m)=>Types(m).Single(x=>x.FullName=="FTRuntime.Internal.SwfAssocList`1").Methods.Single(x=>x.Name=="Remove"&&x.Parameters.Count==1);
    static MethodDefinition FindUnordered(ModuleDefinition m)=>Types(m).Single(x=>x.FullName=="FTRuntime.Internal.SwfList`1").Methods.Single(x=>x.Name=="UnorderedRemoveAt"&&x.Parameters.Count==1);
    static IEnumerable<MethodDefinition> Targets(ModuleDefinition m)=>new[]{FindTarget(m,true),FindRemove(m),FindUnordered(m)};
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
    static void PatchUnordered(ModuleDefinition mod,MethodDefinition m){
        var data=GF(m,"_data");var size=GF(m,"_size");var gp=m.DeclaringType.GenericParameters[0];
        var exc=MR(mod,"System.IndexOutOfRangeException",".ctor",0);
        Reset(m);var moved=Local(m,gp);var zero=Local(m,gp);var last=Local(m,mod.TypeSystem.Int32);
        var il=m.Body.GetILProcessor();var valid=il.Create(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,size);il.Emit(OpCodes.Blt_Un,valid);
        il.Emit(OpCodes.Newobj,exc);il.Emit(OpCodes.Throw);
        il.Append(valid);il.Emit(OpCodes.Ldfld,data);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,size);il.Emit(OpCodes.Ldc_I4_1);il.Emit(OpCodes.Sub);il.Emit(OpCodes.Ldelem_Any,gp);il.Emit(OpCodes.Stloc,moved);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,data);il.Emit(OpCodes.Ldarg_1);il.Emit(OpCodes.Ldloc,moved);il.Emit(OpCodes.Stelem_Any,gp);
        // Decrement occurs before clearing the old last slot, as in both native bodies.
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,data);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,size);il.Emit(OpCodes.Ldc_I4_1);il.Emit(OpCodes.Sub);il.Emit(OpCodes.Stloc,last);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldloc,last);il.Emit(OpCodes.Stfld,size);
        il.Emit(OpCodes.Ldloc,last);il.Emit(OpCodes.Ldloca,zero);il.Emit(OpCodes.Initobj,gp);il.Emit(OpCodes.Ldloc,zero);il.Emit(OpCodes.Stelem_Any,gp);
        il.Emit(OpCodes.Ldloc,moved);il.Emit(OpCodes.Ret);
    }
    static void PatchRemove(ModuleDefinition mod,MethodDefinition m){
        var dict=GF(m,"_dict");var list=GF(m,"_list");var comp=GF(m,"_comp");
        var find=Host(mod,dict.FieldType,"TryGetValue",2);var remove=Host(mod,dict.FieldType,"Remove",1);var set=Host(mod,dict.FieldType,"set_Item",2);
        var equals=Host(mod,comp.FieldType,"Equals",2);var unordered=Host(mod,list.FieldType,"UnorderedRemoveAt",1);
        Reset(m);var index=Local(m,mod.TypeSystem.Int32);var moved=Local(m,m.DeclaringType.GenericParameters[0]);var il=m.Body.GetILProcessor();var done=il.Create(OpCodes.Ret);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,dict);il.Emit(OpCodes.Ldarg_1);il.Emit(OpCodes.Ldloca,index);il.Emit(OpCodes.Callvirt,find);il.Emit(OpCodes.Brfalse,done);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,dict);il.Emit(OpCodes.Ldarg_1);il.Emit(OpCodes.Callvirt,remove);il.Emit(OpCodes.Pop);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,list);il.Emit(OpCodes.Ldloc,index);il.Emit(OpCodes.Callvirt,unordered);il.Emit(OpCodes.Stloc,moved);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,comp);il.Emit(OpCodes.Ldloc,moved);il.Emit(OpCodes.Ldarg_1);il.Emit(OpCodes.Callvirt,equals);il.Emit(OpCodes.Brtrue,done);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,dict);il.Emit(OpCodes.Ldloc,moved);il.Emit(OpCodes.Ldloc,index);il.Emit(OpCodes.Callvirt,set);il.Append(done);
    }
    static void PatchSorting(ModuleDefinition mod,MethodDefinition m){
        var plant=TD(mod,"Plant");var loop=m.DeclaringType.Methods.Single(x=>x.Name=="LoopAddAnimation"&&x.Parameters.Count==2);
        var list=loop.Parameters[1].ParameterType;var ctor=Host(mod,list,".ctor",0);
        var lookup=TD(mod,"GlobalStaticVars").Methods.Single(x=>x.Name=="GetAnimationSprite_Name"&&x.Parameters.Count==2);
        var position=TD(mod,"BoardConfig").Methods.Single(x=>x.Name=="GetGridPosition"&&x.Parameters.Count==2);
        var component=m.Body.Instructions.Select(x=>x.Operand).OfType<GenericInstanceMethod>().First(x=>x.Name=="GetComponent"&&x.GenericArguments[0].FullName=="UnityEngine.SpriteRenderer");
        var transform=MR(mod,"UnityEngine.Component","get_transform",0);
        var layer=MR(mod,"UnityEngine.Renderer","set_sortingLayerName",1);var order=MR(mod,"UnityEngine.Renderer","set_sortingOrder",1);
        var y=mod.ImportReference(position.ReturnType.Resolve().Fields.Single(x=>x.Name=="y"));
        Reset(m);var animations=Local(m,list);var p=Local(m,plant);var obj=Local(m,lookup.ReturnType);var pos=Local(m,position.ReturnType);
        var il=m.Body.GetILProcessor();var done=il.Create(OpCodes.Ret);
        il.Emit(OpCodes.Newobj,ctor);il.Emit(OpCodes.Stloc,animations);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Call,transform);il.Emit(OpCodes.Ldloc,animations);il.Emit(OpCodes.Call,loop);
        il.Emit(OpCodes.Ldarg_1);il.Emit(OpCodes.Ldc_I4,30);il.Emit(OpCodes.Bne_Un,done);
        il.Emit(OpCodes.Ldarg_2);il.Emit(OpCodes.Box,m.GenericParameters[0]);il.Emit(OpCodes.Isinst,plant);il.Emit(OpCodes.Stloc,p);il.Emit(OpCodes.Ldloc,p);il.Emit(OpCodes.Brfalse,done);
        foreach(var upper in new[]{false,true}){
            il.Emit(OpCodes.Ldloc,animations);il.Emit(OpCodes.Ldstr,upper?"Upper":"Lower");il.Emit(OpCodes.Call,lookup);il.Emit(OpCodes.Stloc,obj);
            il.Emit(OpCodes.Ldloc,p);il.Emit(OpCodes.Ldfld,FD(plant,"board"));il.Emit(OpCodes.Ldfld,FD(TD(mod,"Board"),"boardConfig"));
            il.Emit(OpCodes.Ldloc,p);il.Emit(OpCodes.Ldfld,FD(plant,"GridX"));il.Emit(OpCodes.Ldloc,p);il.Emit(OpCodes.Ldfld,FD(plant,"GridY"));il.Emit(OpCodes.Ldc_I4,upper?2:1);il.Emit(upper?OpCodes.Sub:OpCodes.Add);il.Emit(OpCodes.Callvirt,position);il.Emit(OpCodes.Stloc,pos);
            il.Emit(OpCodes.Ldloc,obj);il.Emit(OpCodes.Callvirt,component);il.Emit(OpCodes.Ldstr,"Entity");il.Emit(OpCodes.Callvirt,layer);
            il.Emit(OpCodes.Ldloc,obj);il.Emit(OpCodes.Callvirt,component);il.Emit(OpCodes.Ldloca,pos);il.Emit(OpCodes.Ldfld,y);il.Emit(OpCodes.Ldc_R4,1f);il.Emit(upper?OpCodes.Sub:OpCodes.Add);il.Emit(OpCodes.Ldc_R4,-10f);il.Emit(OpCodes.Mul);il.Emit(OpCodes.Conv_I4);il.Emit(OpCodes.Callvirt,order);
        }
        il.Append(done);
    }

}
