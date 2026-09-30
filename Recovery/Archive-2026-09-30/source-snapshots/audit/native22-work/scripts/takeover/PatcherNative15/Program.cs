using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

internal static class Program
{
    const uint TargetToken = 0x06000290u;
    const int ExpectedTypes = 320, ExpectedMethods = 2317, ExpectedFields = 2802;

    static void Main(string[] args)
    {
        if(args.Length < 2 || args.Length > 3) throw new ArgumentException("usage: PatcherNative15 <native14.dll> <native15.dll> [linked]");
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
        var binding=FindBinding(mod);
        if(!linked&&binding.MetadataToken.ToUInt32()!=0x060000C4)throw new InvalidOperationException("Binding token mismatch");
        foreach(var t in new[]{target,binding})if(t.GenericParameters[0].HasConstraints||t.GenericParameters[0].Attributes!=GenericParameterAttributes.NonVariant)throw new InvalidOperationException("generic constraint changed");
        PatchMap(mod,target); PatchBinding(mod,FindBinding(mod));
        foreach(var t in new[]{target,binding}){StackCheck(t);var first=t.Body.Instructions[0];t.Body.Instructions.RemoveAt(0);bool rejected=false;try{StackCheck(t);}catch(InvalidOperationException){rejected=true;}finally{t.Body.Instructions.Insert(0,first);}if(!rejected)throw new InvalidOperationException("stack negative control accepted");}
        Console.WriteLine("STACK_FLOW_PASS targets=2 negative_controls=2");
        if(!fixture&&mod.AssemblyReferences.Any(x=>x.Name=="System.Private.CoreLib"))throw new InvalidOperationException("host framework reference leaked into Unity candidate");
        if(!linked) CheckIdentity(mod,"memory"); CheckNonTargets(mod,target,before,"memory");
        asm.Write(output);
        using var reopened=AssemblyDefinition.ReadAssembly(output);
        if(reopened.MainModule.Mvid!=mvid) throw new InvalidOperationException($"MVID changed {mvid}->{reopened.MainModule.Mvid}");
        if(!linked) CheckIdentity(reopened.MainModule,"reopened");
        var rt=FindTarget(reopened.MainModule,linked); CheckNonTargets(reopened.MainModule,rt,before,"reopened");
        StackCheck(rt);StackCheck(FindBinding(reopened.MainModule));
        if(rt.Body.ExceptionHandlers.Count!=2 || rt.Body.ExceptionHandlers.Any(x=>x.HandlerType!=ExceptionHandlerType.Finally)) throw new InvalidOperationException("finally cleanup missing after reopen");
        Console.WriteLine($"NATIVE15_PATCH_PASS linked={linked} input_mvid={mvid} output_mvid={reopened.MainModule.Mvid}");
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
        var t=Types(m).Single(x=>x.Name=="Map"&&x.Namespace=="");
        var md=t.Methods.Single(x=>x.Name=="RandomGet_Grid_TestPlace"&&x.GenericParameters.Count==1&&x.Parameters.Count==4);
        if(!linked && md.MetadataToken.ToUInt32()!=TargetToken)throw new InvalidOperationException($"target token mismatch 0x{md.MetadataToken.ToUInt32():X8}");
        if(!md.HasBody)throw new InvalidOperationException("target body missing");
        if(md.IsStatic)throw new InvalidOperationException("Map target unexpectedly static");
        return md;
    }
    static void CheckIdentity(ModuleDefinition m,string stage){var ts=Types(m).ToArray();int mc=ts.Sum(t=>t.Methods.Count),fc=ts.Sum(t=>t.Fields.Count);if(ts.Length!=ExpectedTypes||mc!=ExpectedMethods||fc!=ExpectedFields)throw new InvalidOperationException($"{stage}: counts {ts.Length}/{mc}/{fc}");}
    static Dictionary<uint,string> Snapshot(ModuleDefinition m,MethodDefinition target)=>Types(m).SelectMany(t=>t.Methods).Where(x=>x.HasBody&&x!=target&&x!=FindBinding(m)).ToDictionary(x=>x.MetadataToken.ToUInt32(),Fingerprint);
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

    static MethodDefinition FindBinding(ModuleDefinition m)=>Types(m).Single(x=>x.Name=="VFXAnimationEvent"&&x.Namespace=="").Methods.Single(x=>x.Name=="Binding"&&x.GenericParameters.Count==1&&x.Parameters.Count==4);
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
    static GenericInstanceType Enumerator(ModuleDefinition mod,GenericInstanceType host){
        var n=host.Resolve().NestedTypes.Single(x=>x.Name=="Enumerator");
        var t=new GenericInstanceType(mod.ImportReference(n));t.GenericArguments.Add(host.GenericArguments[0]);return t;
    }
    static void PatchBinding(ModuleDefinition mod,MethodDefinition m){
        var plant=TD(mod,"Plant");var particles=FD(plant,"particleSystems");
        var add=Host(mod,particles.FieldType,"Add",1);
        var getObject=MR(mod,"UnityEngine.Component","get_gameObject",0);
        Reset(m);var v=Local(m,plant);var il=m.Body.GetILProcessor();var end=il.Create(OpCodes.Ret);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldarg,m.Parameters[3]);il.Emit(OpCodes.Stfld,FD(m.DeclaringType,"type"));
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldarg_3);il.Emit(OpCodes.Stfld,FD(m.DeclaringType,"duration"));
        il.Emit(OpCodes.Ldarg_2);il.Emit(OpCodes.Brfalse,end);
        il.Emit(OpCodes.Ldarg_1);il.Emit(OpCodes.Box,m.GenericParameters[0]);il.Emit(OpCodes.Isinst,plant);il.Emit(OpCodes.Stloc,v);
        il.Emit(OpCodes.Ldloc,v);il.Emit(OpCodes.Brfalse,end);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldloc,v);il.Emit(OpCodes.Stfld,FD(m.DeclaringType,"plant"));
        il.Emit(OpCodes.Ldloc,v);il.Emit(OpCodes.Ldfld,particles);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Call,getObject);il.Emit(OpCodes.Callvirt,add);il.Append(end);
    }
    static void PatchMap(ModuleDefinition mod,MethodDefinition m){
        var grid=TD(mod,"Grid");var row=TD(mod,"Row");var device=TD(mod,"Device");
        var list=(GenericInstanceType)m.ReturnType;var rows=FD(m.DeclaringType,"rows");var rowList=(GenericInstanceType)rows.FieldType;
        var set=mod.GetMemberReferences().OfType<MethodReference>().Select(x=>x.DeclaringType).OfType<GenericInstanceType>().First(x=>x.FullName=="System.Collections.Generic.HashSet`1<System.Int32>");
        var re=Enumerator(mod,rowList);var se=Enumerator(mod,set);
        var lc=Host(mod,list,".ctor",0);var la=Host(mod,list,"Add",1);var li=Host(mod,list,"get_Item",1);var lcount=Host(mod,list,"get_Count",0);
        var rget=Host(mod,rowList,"GetEnumerator",0);var rm=Host(mod,re,"MoveNext",0);var rc=Host(mod,re,"get_Current",0);var rd=Host(mod,re,"Dispose",0);
        var sc=Host(mod,set,".ctor",0);var sa=Host(mod,set,"Add",1);var sn=Host(mod,set,"get_Count",0);var sg=Host(mod,set,"GetEnumerator",0);var sm=Host(mod,se,"MoveNext",0);var sr=Host(mod,se,"get_Current",0);var sd=Host(mod,se,"Dispose",0);
        var random=MR(mod,"UnityEngine.Random","Range",2,"System.Int32");var log=MR(mod,"UnityEngine.Debug","Log",1);
        var tostring=MR(mod,"System.Int32","ToString",0,"System.String");var concat=MR(mod,"System.String","Concat",3,"System.String");
        var can=grid.Methods.Single(x=>x.Name=="CanPlacing"&&x.Parameters.Count==1&&x.Parameters[0].ParameterType.FullName=="Device");
        var width=m.DeclaringType.Methods.Single(x=>x.Name=="GetMapX"&&x.Parameters.Count==0);
        Reset(m);var result=Local(m,list);var valid=Local(m,list);var ren=Local(m,re);var rv=Local(m,row);var ix=Local(m,mod.TypeSystem.Int32);var bound=Local(m,mod.TypeSystem.Int32);var gv=Local(m,grid);var dv=Local(m,device);var count=Local(m,mod.TypeSystem.Int32);var hs=Local(m,set);var sen=Local(m,se);
        var il=m.Body.GetILProcessor();var rowNext=il.Create(OpCodes.Ldloca,ren);var rowBody=il.Create(OpCodes.Ldloca,ren);var inner=il.Create(OpCodes.Ldarg_0);var minDone=il.Create(OpCodes.Ldloc,ix);var inc=il.Create(OpCodes.Ldloc,ix);var rf=il.Create(OpCodes.Ldloca,ren);var afterRows=il.Create(OpCodes.Ldloc,valid);var sample=il.Create(OpCodes.Ldloc,hs);var sampleDone=il.Create(OpCodes.Ldloc,hs);var setNext=il.Create(OpCodes.Ldloca,sen);var setBody=il.Create(OpCodes.Ldloc,result);var sf=il.Create(OpCodes.Ldloca,sen);var done=il.Create(OpCodes.Ldloc,result);var all=il.Create(OpCodes.Ldloc,valid);
        il.Emit(OpCodes.Newobj,lc);il.Emit(OpCodes.Stloc,result);il.Emit(OpCodes.Newobj,lc);il.Emit(OpCodes.Stloc,valid);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,rows);il.Emit(OpCodes.Callvirt,rget);il.Emit(OpCodes.Stloc,ren);
        il.Append(rowNext);il.Emit(OpCodes.Call,rm);il.Emit(OpCodes.Brtrue,rowBody);il.Emit(OpCodes.Leave,afterRows);
        il.Append(rowBody);il.Emit(OpCodes.Call,rc);il.Emit(OpCodes.Stloc,rv);il.Emit(OpCodes.Ldarg_3);il.Emit(OpCodes.Stloc,ix);
        il.Append(inner);il.Emit(OpCodes.Call,width);il.Emit(OpCodes.Stloc,bound);
        il.Emit(OpCodes.Ldarg,m.Parameters[3]);il.Emit(OpCodes.Ldc_I4_1);il.Emit(OpCodes.Add);il.Emit(OpCodes.Ldloc,bound);il.Emit(OpCodes.Bge,minDone);
        il.Emit(OpCodes.Ldarg,m.Parameters[3]);il.Emit(OpCodes.Ldc_I4_1);il.Emit(OpCodes.Add);il.Emit(OpCodes.Stloc,bound);
        il.Append(minDone);il.Emit(OpCodes.Ldloc,bound);il.Emit(OpCodes.Bge,rowNext);
        il.Emit(OpCodes.Ldloc,rv);il.Emit(OpCodes.Ldfld,FD(row,"grids"));il.Emit(OpCodes.Ldloc,ix);il.Emit(OpCodes.Callvirt,li);il.Emit(OpCodes.Stloc,gv);
        il.Emit(OpCodes.Ldloc,gv);il.Emit(OpCodes.Ldflda,FD(grid,"gridX"));il.Emit(OpCodes.Call,tostring);il.Emit(OpCodes.Ldstr,",");il.Emit(OpCodes.Ldloc,gv);il.Emit(OpCodes.Ldflda,FD(grid,"gridY"));il.Emit(OpCodes.Call,tostring);il.Emit(OpCodes.Call,concat);il.Emit(OpCodes.Call,log);
        il.Emit(OpCodes.Ldarg_2);il.Emit(OpCodes.Box,m.GenericParameters[0]);il.Emit(OpCodes.Isinst,device);il.Emit(OpCodes.Stloc,dv);il.Emit(OpCodes.Ldloc,dv);il.Emit(OpCodes.Brfalse,inc);
        il.Emit(OpCodes.Ldloc,gv);il.Emit(OpCodes.Ldloc,dv);il.Emit(OpCodes.Callvirt,can);il.Emit(OpCodes.Brfalse,inc);
        il.Emit(OpCodes.Ldloc,valid);il.Emit(OpCodes.Ldloc,gv);il.Emit(OpCodes.Callvirt,la);
        il.Append(inc);il.Emit(OpCodes.Ldc_I4_1);il.Emit(OpCodes.Add);il.Emit(OpCodes.Stloc,ix);il.Emit(OpCodes.Br,inner);
        il.Append(rf);il.Emit(OpCodes.Call,rd);il.Emit(OpCodes.Endfinally);
        il.Append(afterRows);il.Emit(OpCodes.Callvirt,lcount);il.Emit(OpCodes.Stloc,count);il.Emit(OpCodes.Ldarg_1);il.Emit(OpCodes.Ldloc,count);il.Emit(OpCodes.Bgt,all);
        il.Emit(OpCodes.Newobj,sc);il.Emit(OpCodes.Stloc,hs);
        il.Append(sample);il.Emit(OpCodes.Callvirt,sn);il.Emit(OpCodes.Ldarg_1);il.Emit(OpCodes.Bge,sampleDone);
        il.Emit(OpCodes.Ldloc,hs);il.Emit(OpCodes.Ldc_I4_0);il.Emit(OpCodes.Ldloc,count);il.Emit(OpCodes.Call,random);il.Emit(OpCodes.Callvirt,sa);il.Emit(OpCodes.Pop);il.Emit(OpCodes.Br,sample);
        il.Append(sampleDone);il.Emit(OpCodes.Callvirt,sg);il.Emit(OpCodes.Stloc,sen);
        il.Append(setNext);il.Emit(OpCodes.Call,sm);il.Emit(OpCodes.Brtrue,setBody);il.Emit(OpCodes.Leave,done);
        il.Append(setBody);il.Emit(OpCodes.Ldloc,valid);il.Emit(OpCodes.Ldloca,sen);il.Emit(OpCodes.Call,sr);il.Emit(OpCodes.Callvirt,li);il.Emit(OpCodes.Callvirt,la);il.Emit(OpCodes.Br,setNext);
        il.Append(sf);il.Emit(OpCodes.Call,sd);il.Emit(OpCodes.Endfinally);
        il.Append(done);il.Emit(OpCodes.Ret);il.Append(all);il.Emit(OpCodes.Ret);
        m.Body.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally){TryStart=rowNext,TryEnd=rf,HandlerStart=rf,HandlerEnd=afterRows});
        m.Body.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally){TryStart=setNext,TryEnd=sf,HandlerStart=sf,HandlerEnd=done});
    }
}
