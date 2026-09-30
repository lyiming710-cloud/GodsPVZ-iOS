using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

internal static partial class Program
{
    const int ExpectedTypes = 320, ExpectedMethods = 2317, ExpectedFields = 2802;

    static readonly (string Name, int ParamCount)[] TargetMethods = new[]
    {
        ("PreviousPosition", 0),
        ("Start_PreviousPosition", 0),
        ("Update_Move", 0),
        ("Update_PreviousPosition", 0),
        ("ArmBroken", 1),
        ("Ashe", 1),
        ("CheckZombieWin", 2),
        ("CreateStartPrePath", 0),
        ("CreatParticles", 1),
        ("DestroyZombie", 0),
        ("Die", 2),
    };

    static void Main(string[] args)
    {
        if(args.Length==3&&args[2]=="export-all"){
            using var resolverAll=new LockedResolver(new[]{Path.GetDirectoryName(Path.GetFullPath(args[0])),Environment.GetEnvironmentVariable("GODSPVZ_RESOLVER")}.Where(s=>!string.IsNullOrEmpty(s)).ToArray());
            using var inputAll=AssemblyDefinition.ReadAssembly(args[0],new ReaderParameters{AssemblyResolver=resolverAll});
            ExportTargets(inputAll.MainModule,Types(inputAll.MainModule).SelectMany(t=>t.Methods).Where(m=>m.HasBody).ToArray(),args[1]);return;
        }
        if(args.Length==3&&args[2]=="correction-fixture"){
            using var fixtureAsm=AssemblyDefinition.ReadAssembly(args[0]);var fm=fixtureAsm.MainModule;var ft=Types(fm).Single(t=>t.FullName=="Zombie");
            CorrectStartPrevious(fm,ft.Methods.Single(m=>m.Name=="Start_PreviousPosition"));CorrectMove(fm,ft.Methods.Single(m=>m.Name=="Update_Move"));CorrectPreviousUpdate(fm,ft.Methods.Single(m=>m.Name=="Update_PreviousPosition"));CorrectAshe(fm,ft.Methods.Single(m=>m.Name=="Ashe"));
            foreach(var method in ft.Methods.Where(m=>new[]{"Start_PreviousPosition","Update_Move","Update_PreviousPosition","Ashe"}.Contains(m.Name)))StackCheck(method);
            var traversal=Types(fm).Single(t=>t.FullName=="Project").Methods.Single(m=>m.Name=="LoopAddAnimation");PatchProjectLoop(fm,traversal);StackCheck(traversal);
            var finding=ft.Methods.Single(m=>m.Name=="CreateStartPrePath");CorrectCreateStartPath(fm,finding);StackCheck(finding);
            fixtureAsm.Write(args[1]);return;
        }
        if (args.Length < 2 || args.Length > 3) throw new ArgumentException("usage: PatcherNative19 <native17.dll> <native19.dll> [linked]");
        bool fixture = args.Length == 3 && args[2] == "fixture";
        bool linked = args.Length == 3 && (args[2] == "linked" || fixture);
        if (args.Length == 3 && !linked) throw new ArgumentException("unknown mode");
        var input = Path.GetFullPath(args[0]); var output = Path.GetFullPath(args[1]);
        if (!File.Exists(input)) throw new FileNotFoundException(input);
        Directory.CreateDirectory(Path.GetDirectoryName(output)!);

        var resolver = new LockedResolver(new[] { Path.GetDirectoryName(input)!, Path.Combine(Directory.GetCurrentDirectory(), "Tools/Stage9Native4Recovery/resolver") }
            .Concat(fixture ? new[] { Path.GetDirectoryName(typeof(object).Assembly.Location)! } : new[] { Environment.GetEnvironmentVariable("GODSPVZ_RESOLVER") })
            .Where(x => !string.IsNullOrEmpty(x)).ToArray());

        if (fixture)
        {
            using var normalized = AssemblyDefinition.ReadAssembly(input, new ReaderParameters { AssemblyResolver = resolver });
            normalized.Write(input + ".normalized");
            input += ".normalized";
        }

        using var asm = AssemblyDefinition.ReadAssembly(input, new ReaderParameters { AssemblyResolver = resolver });
        var mod = asm.MainModule;
        var mvid = mod.Mvid;
        if (!linked) CheckIdentity(mod, "input");

        var zombieType = Types(mod).Single(x => x.FullName == "Zombie");
        var targets = FindTargets(zombieType).ToArray();
        var targetSet = new HashSet<MethodDefinition>(targets);
        var before = Snapshot(mod, targetSet);

        // Native17 seed; retain narrowly scoped corrections and reconstruct native-backed bodies.
        PatchPreviousPosition(mod, zombieType.Methods.Single(m => m.Name == "PreviousPosition" && m.Parameters.Count == 0));
        PatchArmBroken(mod, zombieType.Methods.Single(m => m.Name == "ArmBroken" && m.Parameters.Count == 1));
        PatchCheckZombieWin(mod, zombieType.Methods.Single(m => m.Name == "CheckZombieWin" && m.Parameters.Count == 2));
        PatchCreatParticles(mod, zombieType.Methods.Single(m => m.Name == "CreatParticles" && m.Parameters.Count == 1));
        PatchDestroyZombie(mod, zombieType.Methods.Single(m => m.Name == "DestroyZombie" && m.Parameters.Count == 0));
        PatchDie(mod, zombieType.Methods.Single(m => m.Name == "Die" && m.Parameters.Count == 2));

        CorrectStartPrevious(mod, targets.Single(t=>t.Name=="Start_PreviousPosition"));
        CorrectMove(mod, targets.Single(t=>t.Name=="Update_Move"));
        CorrectPreviousUpdate(mod, targets.Single(t=>t.Name=="Update_PreviousPosition"));
        CorrectAshe(mod, targets.Single(t=>t.Name=="Ashe"));
        CorrectDieDispatch(mod, targets.Single(t=>t.Name=="Die"));
        CorrectDieParticlePosition(mod, targets.Single(t=>t.Name=="Die"));
        CorrectArmorComparisons(targets.Single(t=>t.Name=="ArmBroken"),1);
        CorrectArmEnumerator(mod,targets.Single(t=>t.Name=="ArmBroken"));
        CorrectArmorComparisons(targets.Single(t=>t.Name=="Die"),2);
        CorrectCreateStartPath(mod, targets.Single(t=>t.Name=="CreateStartPrePath"));
        PatchAdditionalTargets(mod);
        foreach (var t in targets)
        {
            StackCheck(t);
            var bad = Instruction.Create(OpCodes.Pop);
            t.Body.Instructions.Insert(0, bad);
            bool rejected = false;
            try { StackCheck(t); }
            catch (InvalidOperationException) { rejected = true; }
            finally { t.Body.Instructions.RemoveAt(0); }
            if (!rejected) throw new InvalidOperationException($"stack negative control accepted for {t.Name}");
        }
        Console.WriteLine($"STACK_FLOW_PASS targets={targets.Length} negative_controls={targets.Length}");

        if (!fixture && mod.AssemblyReferences.Any(x => x.Name == "System.Private.CoreLib"))
            throw new InvalidOperationException("host framework reference leaked into Unity candidate");

        if (!linked) CheckIdentity(mod, "memory");
        CheckNonTargets(mod, targetSet, before, "memory");

        asm.Write(output);

        using var reopened = AssemblyDefinition.ReadAssembly(output, new ReaderParameters { AssemblyResolver = resolver });
        if (reopened.MainModule.Mvid != mvid) throw new InvalidOperationException($"MVID changed {mvid}->{reopened.MainModule.Mvid}");
        if (!linked) CheckIdentity(reopened.MainModule, "reopened");
        var reopenedZombie = Types(reopened.MainModule).Single(x => x.FullName == "Zombie");
        var reopenedTargets = FindTargets(reopenedZombie).ToArray();
        var reopenedTargetSet = new HashSet<MethodDefinition>(reopenedTargets);
        CheckNonTargets(reopened.MainModule, reopenedTargetSet, before, "reopened");
        foreach (var t in reopenedTargets)
        {
            StackCheck(t);
        }
        ExportTargets(reopened.MainModule,reopenedTargets,output+".targets.json");
        Console.WriteLine($"NATIVE19_PATCH_PASS linked={linked} input_mvid={mvid} output_mvid={reopened.MainModule.Mvid} targets={targets.Length}");
    }

    sealed class LockedResolver : IAssemblyResolver
    {
        readonly string[] dirs; readonly Dictionary<string, AssemblyDefinition> cache = new();
        public LockedResolver(string[] d) { dirs = d; }
        public AssemblyDefinition Resolve(AssemblyNameReference n) => Resolve(n, new ReaderParameters());
        public AssemblyDefinition Resolve(AssemblyNameReference n, ReaderParameters p)
        {
            if (cache.TryGetValue(n.Name, out var a)) return a;
            var file = dirs.Select(d => Path.Combine(d, n.Name + ".dll")).FirstOrDefault(File.Exists);
            if (file == null) throw new AssemblyResolutionException(n);
            p.AssemblyResolver = this; a = AssemblyDefinition.ReadAssembly(file, p); cache[n.Name] = a; return a;
        }
        public void Dispose() { foreach (var a in cache.Values) a.Dispose(); }
    }

    static IEnumerable<TypeDefinition> Types(TypeDefinition t) { yield return t; foreach (var n in t.NestedTypes) foreach (var x in Types(n)) yield return x; }
    static IEnumerable<TypeDefinition> Types(ModuleDefinition m) { foreach (var t in m.Types) foreach (var x in Types(t)) yield return x; }

    static IEnumerable<MethodDefinition> FindTargets(TypeDefinition zombie)
    {
        foreach (var (name, pc) in TargetMethods)
        {
            var md = zombie.Methods.Single(x => x.Name == name && x.Parameters.Count == pc);
            if (!md.HasBody) throw new InvalidOperationException($"target {name} missing body");
            yield return md;
        }
        foreach(var md in AdditionalTargets(zombie.Module))yield return md;
    }

    static void CheckIdentity(ModuleDefinition m, string stage)
    {
        var ts = Types(m).ToArray();
        int mc = ts.Sum(t => t.Methods.Count), fc = ts.Sum(t => t.Fields.Count);
        if (ts.Length != ExpectedTypes || mc != ExpectedMethods || fc != ExpectedFields)
            throw new InvalidOperationException($"{stage}: counts {ts.Length}/{mc}/{fc}");
    }

    static Dictionary<uint, string> Snapshot(ModuleDefinition m, HashSet<MethodDefinition> targets) =>
        Types(m).SelectMany(t => t.Methods).Where(x => x.HasBody && !targets.Contains(x)).ToDictionary(x => x.MetadataToken.ToUInt32(), Fingerprint);

    static void CheckNonTargets(ModuleDefinition m, HashSet<MethodDefinition> targets, Dictionary<uint, string> before, string stage)
    {
        var now = Snapshot(m, targets);
        if (now.Count != before.Count) throw new InvalidOperationException($"{stage}: non-target count {before.Count}->{now.Count}");
        foreach (var kv in before)
        {
            if (!now.TryGetValue(kv.Key, out var v) || v != kv.Value)
            {
                int at = 0; while (v != null && at < Math.Min(v.Length, kv.Value.Length) && v[at] == kv.Value[at]) at++;
                throw new InvalidOperationException($"{stage}: non-target changed 0x{kv.Key:X8} at={at} before={kv.Value.Substring(Math.Max(0, at - 25), Math.Min(120, kv.Value.Length - Math.Max(0, at - 25)))} after={v?.Substring(Math.Max(0, at - 25), Math.Min(120, v.Length - Math.Max(0, at - 25)))}");
            }
        }
        Console.WriteLine($"NON_TARGET_ISOLATION_PASS stage={stage} methods={before.Count}");
    }

    static string StableOp(Instruction i)
    {
        var n = i.OpCode.Code.ToString();
        if ((i.Operand is Instruction || i.Operand is Instruction[]) && n.EndsWith("_S", StringComparison.Ordinal)) return n[..^2];
        return n;
    }
    static int Ix(MethodDefinition m, Instruction i) => i == null ? -1 : m.Body.Instructions.IndexOf(i);

    static string Fingerprint(MethodDefinition m)
    {
        var b = new StringBuilder().Append(m.Body.InitLocals).Append('|').Append(m.Body.MaxStackSize).Append('|');
        foreach (var v in m.Body.Variables) b.Append("V:").Append(v.VariableType.FullName).Append(';');
        foreach (var i in m.Body.Instructions)
        {
            b.Append(StableOp(i)).Append(':');
            switch (i.Operand)
            {
                case null: break;
                case Instruction x: b.Append('@').Append(Ix(m, x)); break;
                case Instruction[] xs: foreach (var x in xs) b.Append('@').Append(Ix(m, x)).Append(','); break;
                case VariableDefinition v: b.Append('V').Append(v.Index).Append(':').Append(v.VariableType.FullName); break;
                case ParameterDefinition p: b.Append('P').Append(p.Index).Append(':').Append(p.ParameterType.FullName); break;
                case MemberReference mr: b.Append('M').Append(mr.FullName).Append('@').Append(mr.DeclaringType?.Scope?.Name); break;
                default: b.Append(i.Operand); break;
            }
            b.Append(';');
        }
        foreach (var e in m.Body.ExceptionHandlers)
            b.Append("EH:").Append(e.HandlerType).Append(':').Append(Ix(m, e.TryStart)).Append(':').Append(Ix(m, e.TryEnd)).Append(':').Append(Ix(m, e.HandlerStart)).Append(':').Append(Ix(m, e.HandlerEnd)).Append(':').Append(Ix(m, e.FilterStart)).Append(':').Append(e.CatchType?.FullName).Append(';');
        return b.ToString();
    }

    static MethodReference MR(ModuleDefinition m, string decl, string name, int argc, string ret = null)
    {
        var q = m.GetMemberReferences().OfType<MethodReference>().Concat(Types(m).SelectMany(t => t.Methods)).Where(x => x.DeclaringType.FullName == decl && x.Name == name && x.Parameters.Count == argc);
        if (ret != null) q = q.Where(x => x.ReturnType.FullName == ret);
        return q.First();
    }

    static FieldReference FR(ModuleDefinition m, string decl, string name)
    {
        var q = m.GetMemberReferences().OfType<FieldReference>().Concat(Types(m).SelectMany(t => t.Fields)).Where(x => x.DeclaringType.FullName == decl && x.Name == name);
        return q.First();
    }

    static void Reset(MethodDefinition m) { m.Body = new MethodBody(m) { InitLocals = true, MaxStackSize = 8 }; }
    static VariableDefinition Local(MethodDefinition m, TypeReference t) { var v = new VariableDefinition(t); m.Body.Variables.Add(v); return v; }

    static void StackCheck(MethodDefinition m)
    {
        if (m.Body.Instructions.Count == 0) throw new InvalidOperationException("empty body");
        var seen = new Dictionary<Instruction, int>();
        var queue = new Queue<(Instruction, int)>();
        queue.Enqueue((m.Body.Instructions[0], 0));
        foreach (var e in m.Body.ExceptionHandlers) queue.Enqueue((e.HandlerStart, e.HandlerType == ExceptionHandlerType.Catch ? 1 : 0));
        while (queue.Count > 0)
        {
            var (i, depth) = queue.Dequeue();
            if (i == null) throw new InvalidOperationException("fallthrough past body");
            if (seen.TryGetValue(i, out var old))
            {
                if (old != depth) throw new InvalidOperationException("stack merge mismatch in " + m.FullName);
                continue;
            }
            seen[i] = depth;
            int pop = i.OpCode.StackBehaviourPop == StackBehaviour.Pop0 ? 0 : i.OpCode.StackBehaviourPop.ToString().Split('_').Length;
            int push = i.OpCode.StackBehaviourPush == StackBehaviour.Push0 ? 0 : i.OpCode.StackBehaviourPush.ToString().Split('_').Length;
            if (i.Operand is MethodReference mr)
            {
                pop = mr.Parameters.Count + (i.OpCode == OpCodes.Newobj ? 0 : mr.HasThis ? 1 : 0);
                push = i.OpCode == OpCodes.Newobj || mr.ReturnType.MetadataType != MetadataType.Void ? 1 : 0;
            }
            if (i.OpCode == OpCodes.Ret) { pop = m.ReturnType.MetadataType == MetadataType.Void ? 0 : 1; push = 0; }
            if (i.OpCode == OpCodes.Leave || i.OpCode == OpCodes.Leave_S) { pop = depth; push = 0; }
            if (depth < pop) throw new InvalidOperationException("stack underflow " + m.FullName + " " + i);
            int next = depth - pop + push;
            if (i.OpCode == OpCodes.Ret || i.OpCode == OpCodes.Endfinally)
            {
                if (next != 0) throw new InvalidOperationException("nonempty terminal stack in " + m.FullName);
                continue;
            }
            if (i.OpCode == OpCodes.Leave || i.OpCode == OpCodes.Leave_S)
            {
                if (depth != 0) throw new InvalidOperationException("nonempty leave stack");
                next = 0;
            }
            if (i.Operand is Instruction target) queue.Enqueue((target, next));
            else if (i.Operand is Instruction[] targets) { foreach (var tgt in targets) queue.Enqueue((tgt, next)); }
            if (i.OpCode != OpCodes.Br && i.OpCode != OpCodes.Br_S && i.OpCode != OpCodes.Leave && i.OpCode != OpCodes.Leave_S && i.OpCode != OpCodes.Throw && i.OpCode != OpCodes.Rethrow)
            {
                int ix = m.Body.Instructions.IndexOf(i);
                if (ix + 1 < m.Body.Instructions.Count) queue.Enqueue((m.Body.Instructions[ix + 1], next));
            }
        }
    }

    // 1. PreviousPosition: return this.previousPosition;
    static void PatchPreviousPosition(ModuleDefinition mod, MethodDefinition m)
    {
        Reset(m);
        var fld = m.DeclaringType.Fields.Single(f => f.Name == "previousPosition");
        var il = m.Body.GetILProcessor();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldfld, fld);
        il.Emit(OpCodes.Ret);
    }

    // 5. ArmBroken: surgical fix for 2 invalid ldloca calls + remove trailing warning
    static void PatchArmBroken(ModuleDefinition mod, MethodDefinition m)
    {
        var instructions = m.Body.Instructions;
        var v40 = m.Body.Variables.First(v => v.Index == 40 && v.VariableType.FullName == "UnityEngine.Vector3");
        var v16 = m.Body.Variables.First(v => v.Index == 16 && v.VariableType.FullName == "UnityEngine.Vector3");

        for (int i = 0; i < instructions.Count; i++)
        {
            var inst = instructions[i];
            if (inst.OpCode == OpCodes.Call && inst.Operand is MethodReference mr)
            {
                if (mr.Name == "CreateAudioAtPoint" && i >= 2 && instructions[i - 2].OpCode == OpCodes.Ldloca)
                {
                    instructions[i - 2] = Instruction.Create(OpCodes.Ldloc, v40);
                }
                else if (mr.Name == "set_position" && i >= 1 && instructions[i - 1].OpCode == OpCodes.Ldloca)
                {
                    instructions[i - 1] = Instruction.Create(OpCodes.Ldloc, v16);
                }
            }
        }

        RemoveTrailingStackWarning(m);
    }

    // 7. CheckZombieWin: surgical fix for ldc.i4.0 / ldc.i4 0 ceq on Grid pointer + remove trailing warning
    static void PatchCheckZombieWin(ModuleDefinition mod, MethodDefinition m)
    {
        var instructions = m.Body.Instructions;
        for (int i = 0; i < instructions.Count; i++)
        {
            var inst = instructions[i];
            if (inst.OpCode == OpCodes.Call && inst.Operand is MethodReference mr && mr.Name == "GetGrid")
            {
                for (int j = i + 1; j < Math.Min(instructions.Count, i + 6); j++)
                {
                    var cand = instructions[j];
                    if (cand.OpCode == OpCodes.Ldc_I4_0 || (cand.OpCode == OpCodes.Ldc_I4 && Convert.ToInt32(cand.Operand) == 0))
                    {
                        instructions[j] = Instruction.Create(OpCodes.Ldnull);
                        break;
                    }
                }
            }
        }
        RemoveTrailingStackWarning(m);
    }

    // 9. CreatParticles: surgical fix for ldloca V_6 passed to Transform.set_position
    static void PatchCreatParticles(ModuleDefinition mod, MethodDefinition m)
    {
        var instructions = m.Body.Instructions;
        var v3 = m.Body.Variables.First(v => v.Index == 3 && v.VariableType.FullName == "UnityEngine.Vector3");
        for (int i = 0; i < instructions.Count; i++)
        {
            var inst = instructions[i];
            if (inst.OpCode == OpCodes.Call && inst.Operand is MethodReference mr && mr.Name == "set_position")
            {
                if (i >= 1 && (instructions[i - 1].OpCode == OpCodes.Ldloca || instructions[i - 1].OpCode == OpCodes.Ldloc))
                {
                    instructions[i - 1] = Instruction.Create(OpCodes.Ldloc, v3);
                }
            }
        }
        RemoveTrailingStackWarning(m);
    }

    // 10. DestroyZombie: fix loop variables V_10, V_17 to Int32; List<Zombie>.Remove; ldelem.ref
    static void PatchDestroyZombie(ModuleDefinition mod, MethodDefinition m)
    {
        var v10 = m.Body.Variables.First(v => v.Index == 10);
        v10.VariableType = mod.TypeSystem.Int32;
        var v17 = m.Body.Variables.First(v => v.Index == 17);
        v17.VariableType = mod.TypeSystem.Int32;

        var instructions = m.Body.Instructions;
        for (int i = 0; i < instructions.Count; i++)
        {
            var inst = instructions[i];
            if (inst.OpCode == OpCodes.Call && inst.Operand is MethodReference mr && mr.Name == "Remove")
            {
                if (mr.DeclaringType is GenericInstanceType git && git.ElementType.Name.StartsWith("List"))
                {
                    var listZombie = new GenericInstanceType(git.ElementType);
                    listZombie.GenericArguments.Add(m.DeclaringType);
                    var removeMethod = new MethodReference(mr.Name, mr.ReturnType, listZombie)
                    {
                        HasThis = mr.HasThis,
                        ExplicitThis = mr.ExplicitThis,
                        CallingConvention = mr.CallingConvention
                    };
                    foreach (var p in mr.Parameters)
                    {
                        removeMethod.Parameters.Add(new ParameterDefinition(p.Name, p.Attributes, p.ParameterType));
                    }
                    inst.Operand = removeMethod;
                }
            }
            if (inst.OpCode == OpCodes.Ldelem_Any)
            {
                instructions[i] = Instruction.Create(OpCodes.Ldelem_Ref);
            }
        }
        RemoveTrailingStackWarning(m);
    }

    // Restore numeric locals and the two position arguments before native CFG recovery.
    static void PatchDie(ModuleDefinition mod, MethodDefinition m)
    {
        foreach(var index in new[]{15,18})m.Body.Variables[index].VariableType=mod.TypeSystem.Boolean;
        foreach(var index in new[]{33,36})m.Body.Variables[index].VariableType=mod.TypeSystem.Int32;
        int calls=0;var il=m.Body.GetILProcessor();
        foreach(var setter in m.Body.Instructions.ToArray().Where(i=>i.Operand is MethodReference mr&&mr.Name=="set_position")){
            var value=setter.Previous;
            if(value.OpCode!=OpCodes.Ldloca||!(value.Operand is VariableDefinition v)||v.Index!=60||!(value.Previous.Operand is VariableDefinition receiver))throw new Exception("Die position input drift");
            if(receiver.Index==57){value.OpCode=OpCodes.Ldloc;value.Operand=m.Body.Variables[9];}
            else if(receiver.Index==12){
                value.OpCode=OpCodes.Ldarg_0;value.Operand=null;
                il.InsertBefore(setter,Instruction.Create(OpCodes.Call,MR(mod,"UnityEngine.Component","get_transform",0)));
                il.InsertBefore(setter,Instruction.Create(OpCodes.Call,MR(mod,"UnityEngine.Transform","get_position",0)));
            }else throw new Exception("unexpected Die position receiver");
            calls++;
        }
        if(calls!=2)throw new Exception("Die expected exactly two position calls");
        RemoveTrailingStackWarning(m);
    }

    static void RemoveTrailingStackWarning(MethodDefinition m)
    {
        var instructions = m.Body.Instructions;
        for (int i = instructions.Count - 1; i >= 0; i--)
        {
            var inst = instructions[i];
            if (inst.OpCode == OpCodes.Ldstr && inst.Operand is string s && s.StartsWith("Warning: Method ends with non empty stack"))
            {
                if (i + 1 < instructions.Count && instructions[i + 1].OpCode == OpCodes.Pop)
                {
                    instructions.RemoveAt(i + 1);
                }
                instructions.RemoveAt(i);
                break;
            }
        }
    }
}
