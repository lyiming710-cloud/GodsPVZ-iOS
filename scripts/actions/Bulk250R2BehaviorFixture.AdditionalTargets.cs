using System.Reflection;
using System.Reflection.Emit;
using Mono.Cecil;
using Mono.Cecil.Cil;
using COp = Mono.Cecil.Cil.OpCodes;
using ROp = System.Reflection.Emit.OpCodes;

namespace GodsPVZ.Actions.Verification;

// Explicit controlled dependencies. Their original candidate/native method bodies are
// NOT executed. No assertion here establishes Unity fake-null, RNG, ctor or AddEnemy equivalence.
public class MockUnityObject { public bool destroyed; }
public class MockDeviceData { public float magnification_Atk,magnification_Hp,magnification_Def; }
public class MockZombieData { public float magnification_Atk,magnification_Hp,magnification_Def,toughness,elementalResistance; }
public class MockZombieManager
{
    public MockUnityObject board;
    public MockZombieData boardEntryData;
    public List<MockBoardEntry> effectBoardEntries;
}
public class MockZombieSelect { public int point,minFlag,minWave; public float weight; public string label; }
public class MockWave { public List<MockZombieSelect> added=new(); }
public class MockEnemySelecter
{
    public int enemyPoint_Base;
    public List<MockZombieSelect> zombieSelects=new();
    public Func<List<MockZombieSelect>,float,MockZombieSelect> choose;
    public Action<MockWave,MockZombieSelect> add;
    public List<string> calls=new();
    public MockZombieSelect ZombieSelectRandomGet(List<MockZombieSelect> pool,float total)
    {
        calls.Add("choose:"+string.Join(",",pool.Select(p=>p.label))+":"+BitConverter.SingleToInt32Bits(total));
        return choose==null?pool[0]:choose(pool,total);
    }
    public void AddEnemy(MockWave wave,MockZombieSelect value)
    {
        calls.Add("add:"+value.label);
        if(add!=null)add(wave,value);else wave.added.Add(value);
    }
}

public static partial class CandidateCilTranslator
{
    public static bool ControlledUnityInequality(MockUnityObject a,MockUnityObject b)=>
        (a==null||a.destroyed)!=(b==null||b.destroyed) ||
        (a!=null&&!a.destroyed&&b!=null&&!b.destroyed&&!ReferenceEquals(a,b));

    // Read EVERY candidate instruction, branch and single finally region.
    // Unknown types, member tokens, operand forms or EH shapes fail closed.
    private static DynamicMethod EmitAdditional(MethodDefinition method,string id,Type returnType,Type[] args)
    {
        var typeMap=new Dictionary<string,Type>{
            ["DeviceManager"]=typeof(MockDeviceManager),["DeviceManager/BoardEntryData"]=typeof(MockDeviceData),
            ["ZombieManager"]=typeof(MockZombieManager),["ZombieManager/BoardEntryData"]=typeof(MockZombieData),
            ["BoardEntry"]=typeof(MockBoardEntry),["BoardEntryType"]=typeof(int),["EnemySelecter"]=typeof(MockEnemySelecter),
            ["ZombieSelect"]=typeof(MockZombieSelect),["Wave"]=typeof(MockWave),["UnityEngine.Object"]=typeof(MockUnityObject),
            ["Board"]=typeof(MockUnityObject),["System.Int32"]=typeof(int),["System.Single"]=typeof(float),
            ["System.Boolean"]=typeof(bool),["System.String"]=typeof(string),["System.Void"]=typeof(void)
        };
        Type MapType(TypeReference t)
        {
            if(t.FullName=="BoardEntryType")
            {
                var def=t.Resolve();
                if(!def.IsEnum || def.Fields.Single(f=>f.Name=="value__").FieldType.MetadataType!=MetadataType.Int32)
                    throw new InvalidProgramException("BoardEntryType must be int32-backed");
            }
            if(typeMap.TryGetValue(t.FullName,out var mapped))return mapped;
            if(t is GenericInstanceType g && g.GenericArguments.Count==1)
            {
                var element=MapType(g.GenericArguments[0]);
                if(g.ElementType.FullName=="System.Collections.Generic.List\u00601")return typeof(List<>).MakeGenericType(element);
                if(g.ElementType.FullName=="System.Collections.Generic.List\u00601/Enumerator")return typeof(MockTrackedListEnumerator<>).MakeGenericType(element);
            }
            throw new NotSupportedException(id+" unmapped candidate type "+t.FullName);
        }
        Type CallType(TypeReference t,MethodReference owner)
        {
            if(t is GenericParameter p && p.Type==GenericParameterType.Type && owner.DeclaringType is GenericInstanceType g)
                return MapType(g.GenericArguments[p.Position]);
            if(t is GenericInstanceType instance && instance.GenericArguments.Count==1)
            {
                var arg=CallType(instance.GenericArguments[0],owner);
                if(instance.ElementType.FullName=="System.Collections.Generic.List\u00601")
                    return typeof(List<>).MakeGenericType(arg);
                if(instance.ElementType.FullName=="System.Collections.Generic.List\u00601/Enumerator")
                    return typeof(MockTrackedListEnumerator<>).MakeGenericType(arg);
            }
            return MapType(t);
        }
        void CheckCall(MethodReference source,MethodBase host)
        {
            var expected=source.Parameters.Select(p=>CallType(p.ParameterType,source)).ToArray();
            if(source.HasThis==host.IsStatic || !expected.SequenceEqual(host.GetParameters().Select(p=>p.ParameterType)) ||
                MapType(source.DeclaringType)!=host.DeclaringType ||
                (host is MethodInfo mi && CallType(source.ReturnType,source)!=mi.ReturnType))
                throw new InvalidProgramException(id+" method signature binding mismatch "+source.FullName);
        }
        if(!method.HasThis || args[0]!=MapType(method.DeclaringType) ||
            returnType!=MapType(method.ReturnType) || method.Parameters.Count+1!=args.Length ||
            method.Parameters.Where((p,i)=>MapType(p.ParameterType)!=args[i+1]).Any())
            throw new InvalidProgramException(id+" candidate signature mismatch");
        var dm=new DynamicMethod(id+"_"+method.Name,returnType,args,typeof(Program).Module,true);
        dm.InitLocals=method.Body.InitLocals;
        var il=dm.GetILGenerator();
        var locals=method.Body.Variables.Select(v=>il.DeclareLocal(MapType(v.VariableType))).ToArray();
        var labels=method.Body.Instructions.ToDictionary(i=>i,i=>il.DefineLabel());
        if(method.Body.ExceptionHandlers.Count!=1)throw new NotSupportedException(id+" requires exactly one finally");
        var eh=method.Body.ExceptionHandlers.Single();
        if(eh.HandlerType!=ExceptionHandlerType.Finally || eh.TryEnd!=eh.HandlerStart ||
            eh.HandlerEnd==null || eh.HandlerEnd.Previous.OpCode!=COp.Endfinally)
            throw new NotSupportedException(id+" unmodeled EH layout");
        var fields=new Dictionary<int,(Type Type,string Name)>{
            [0x040001DA]=(typeof(MockDeviceManager),"boardEntryData"),[0x040001D4]=(typeof(MockDeviceManager),"board"),
            [0x040001D9]=(typeof(MockDeviceManager),"effectBoardEntries"),
            [0x040001DB]=(typeof(MockDeviceData),"magnification_Atk"),[0x040001DC]=(typeof(MockDeviceData),"magnification_Hp"),
            [0x040001DD]=(typeof(MockDeviceData),"magnification_Def"),
            [0x04000369]=(typeof(MockZombieManager),"boardEntryData"),[0x0400035F]=(typeof(MockZombieManager),"board"),
            [0x04000368]=(typeof(MockZombieManager),"effectBoardEntries"),
            [0x0400036A]=(typeof(MockZombieData),"magnification_Atk"),[0x0400036B]=(typeof(MockZombieData),"magnification_Hp"),
            [0x0400036C]=(typeof(MockZombieData),"magnification_Def"),[0x0400036D]=(typeof(MockZombieData),"toughness"),
            [0x0400036E]=(typeof(MockZombieData),"elementalResistance"),
            [0x040007FC]=(typeof(MockBoardEntry),"must"),[0x040007FD]=(typeof(MockBoardEntry),"select"),
            [0x040007FE]=(typeof(MockBoardEntry),"key"),[0x040007FA]=(typeof(MockBoardEntry),"boardEntryType"),
            [0x04000208]=(typeof(MockEnemySelecter),"enemyPoint_Base"),[0x04000207]=(typeof(MockEnemySelecter),"zombieSelects"),
            [0x04000201]=(typeof(MockZombieSelect),"point"),[0x04000202]=(typeof(MockZombieSelect),"weight"),
            [0x04000203]=(typeof(MockZombieSelect),"minFlag"),[0x04000204]=(typeof(MockZombieSelect),"minWave")
        };
        var calls=new Dictionary<int,(string Name,MethodBase Host)>{
            [0x06000191]=(".ctor",typeof(MockDeviceData).GetConstructor(Type.EmptyTypes)),
            [0x0600027F]=(".ctor",typeof(MockZombieData).GetConstructor(Type.EmptyTypes)),
            [0x06000607]=("GetValue",typeof(MockBoardEntry).GetMethod("GetValue")),
            [0x0A00000C]=("op_Inequality",typeof(CandidateCilTranslator).GetMethod(nameof(ControlledUnityInequality))),
            [0x0A00002A]=("ToString",typeof(int).GetMethod("ToString",Type.EmptyTypes)),
            [0x0A00003A]=("op_Equality",typeof(string).GetMethod("op_Equality")),
            [0x0A000139]=(".ctor",typeof(List<MockZombieSelect>).GetConstructor(Type.EmptyTypes)),
            [0x0A00013A]=("Add",typeof(List<MockZombieSelect>).GetMethod("Add")),
            [0x0A000146]=("get_Count",typeof(List<MockZombieSelect>).GetMethod("get_Count")),
            [0x060001AC]=("ZombieSelectRandomGet",typeof(MockEnemySelecter).GetMethod("ZombieSelectRandomGet")),
            [0x060001A6]=("AddEnemy",typeof(MockEnemySelecter).GetMethod("AddEnemy"))
        };
        var getEnum=new Dictionary<int,Type>{[0x0A0000E6]=typeof(MockBoardEntry),[0x0A000445]=typeof(MockZombieSelect)};
        foreach(var t in new[]{typeof(MockBoardEntry),typeof(MockZombieSelect)})
        {
            bool b=t==typeof(MockBoardEntry);
            var e=typeof(MockTrackedListEnumerator<>).MakeGenericType(t);
            calls[b?0x0A0000E7:0x0A000447]=("get_Current",e.GetMethod("get_Current"));
            calls[b?0x0A0000E8:0x0A000446]=("MoveNext",e.GetMethod("MoveNext"));
            calls[b?0x0A0000A7:0x0A000138]=("Dispose",e.GetMethod("Dispose"));
        }
        var ops=typeof(ROp).GetFields(BindingFlags.Public|BindingFlags.Static)
            .Where(f=>f.FieldType==typeof(System.Reflection.Emit.OpCode))
            .Select(f=>(System.Reflection.Emit.OpCode)f.GetValue(null)).ToDictionary(o=>o.Value);
        var allowed=new HashSet<Code>{
            Code.Ldarg_0,Code.Ldarg_1,Code.Ldarg_2,Code.Ldarg_3,Code.Ldarga_S,
            Code.Ldloc_0,Code.Ldloc_1,Code.Ldloc_2,Code.Ldloc_3,Code.Ldloc_S,Code.Ldloca_S,
            Code.Stloc_0,Code.Stloc_1,Code.Stloc_2,Code.Stloc_3,Code.Stloc_S,
            Code.Ldfld,Code.Stfld,Code.Ldnull,Code.Ldc_I4,Code.Ldc_I4_0,Code.Ldc_I4_2,Code.Ldc_I4_3,
            Code.Ldc_I4_6,Code.Ldc_R4,Code.Add,Code.Sub,Code.Mul,Code.Div,Code.Rem,Code.Conv_R4,
            Code.Br,Code.Brfalse,Code.Brtrue,Code.Beq,Code.Bne_Un,Code.Bgt,Code.Ble,
            Code.Call,Code.Callvirt,Code.Newobj,Code.Leave,Code.Endfinally,Code.Ret,Code.Nop,Code.Pop
        };
        foreach(var ins in method.Body.Instructions)
        {
            if(!allowed.Contains(ins.OpCode.Code))throw new NotSupportedException(id+" unmodeled opcode "+ins);
            if(ins==eh.TryStart)il.BeginExceptionBlock();
            if(ins==eh.HandlerStart)il.BeginFinallyBlock();
            il.MarkLabel(labels[ins]);
            var code=ins.OpCode;
            if(code==COp.Endfinally){il.EndExceptionBlock();continue;}
            if(code==COp.Ldfld||code==COp.Stfld)
            {
                var f=(FieldReference)ins.Operand;
                if(!fields.TryGetValue(f.MetadataToken.ToInt32(),out var map) || f.Name!=map.Name)
                    throw new NotSupportedException(id+" field binding "+f.FullName);
                var host=map.Type.GetField(map.Name);
                if(host.DeclaringType!=MapType(f.DeclaringType)||host.FieldType!=MapType(f.FieldType))
                    throw new InvalidProgramException(id+" field type mismatch "+f.FullName);
                il.Emit(code==COp.Ldfld?ROp.Ldfld:ROp.Stfld,host);continue;
            }
            if(code==COp.Call||code==COp.Callvirt||code==COp.Newobj)
            {
                var m=(MethodReference)ins.Operand;int token=m.MetadataToken.ToInt32();
                if(getEnum.TryGetValue(token,out var elem))
                {
                    if(m.Name!="GetEnumerator"||code!=COp.Callvirt||m.Parameters.Count!=0||!m.HasThis||
                        MapType(m.DeclaringType)!=typeof(List<>).MakeGenericType(elem)||
                        CallType(m.ReturnType,m)!=typeof(MockTrackedListEnumerator<>).MakeGenericType(elem))
                        throw new InvalidProgramException(id+" enumerator call shape");
                    il.Emit(ROp.Newobj,typeof(MockTrackedListEnumerator<>).MakeGenericType(elem)
                        .GetConstructor(new[]{typeof(List<>).MakeGenericType(elem)}));continue;
                }
                if(!calls.TryGetValue(token,out var binding)||binding.Name!=m.Name)
                    throw new NotSupportedException(id+" call binding "+m.FullName);
                // Unity op_Inequality is deliberately a static controlled adapter;
                // all other calls retain mapped declaring type, parameters and return.
                if(token==0x0A00000C)
                {
                    if(m.HasThis||m.DeclaringType.FullName!="UnityEngine.Object"||
                        m.ReturnType.MetadataType!=MetadataType.Boolean||
                        m.Parameters.Count!=2||m.Parameters.Any(p=>p.ParameterType.FullName!="UnityEngine.Object"))
                        throw new InvalidProgramException(id+" Unity inequality signature mismatch");
                }
                else CheckCall(m,binding.Host);
                if(binding.Host is ConstructorInfo ctor)
                {if(code!=COp.Newobj)throw new InvalidProgramException(id+" constructor opcode");il.Emit(ROp.Newobj,ctor);}
                else il.Emit(code==COp.Callvirt?ROp.Callvirt:ROp.Call,(MethodInfo)binding.Host);
                continue;
            }
            var op=ops[code.Value];
            switch(code.OperandType)
            {
                case Mono.Cecil.Cil.OperandType.InlineNone: il.Emit(op);break;
                case Mono.Cecil.Cil.OperandType.InlineI: il.Emit(op,(int)ins.Operand);break;
                case Mono.Cecil.Cil.OperandType.ShortInlineI: il.Emit(op,(sbyte)ins.Operand);break;
                case Mono.Cecil.Cil.OperandType.ShortInlineR: il.Emit(op,(float)ins.Operand);break;
                case Mono.Cecil.Cil.OperandType.InlineBrTarget:
                    il.Emit(op,labels[(Instruction)ins.Operand]);break;
                case Mono.Cecil.Cil.OperandType.ShortInlineBrTarget:
                    il.Emit(ops.Values.Single(o=>o.Name==op.Name[..^2]),labels[(Instruction)ins.Operand]);break;
                case Mono.Cecil.Cil.OperandType.InlineVar:
                case Mono.Cecil.Cil.OperandType.ShortInlineVar:
                    il.Emit(op,locals[((VariableDefinition)ins.Operand).Index]);break;
                case Mono.Cecil.Cil.OperandType.InlineArg:
                case Mono.Cecil.Cil.OperandType.ShortInlineArg:
                    il.Emit(op,(short)(((ParameterDefinition)ins.Operand).Index+1));break;
                default:throw new NotSupportedException(id+" operand/opcode "+ins);
            }
        }
        return dm;
    }
    public static Func<MockDeviceManager,int,MockDeviceData> CompileDeviceData187(MethodDefinition m)=>
        (Func<MockDeviceManager,int,MockDeviceData>)EmitAdditional(m,"06000187",typeof(MockDeviceData),
            new[]{typeof(MockDeviceManager),typeof(int)}).CreateDelegate(typeof(Func<MockDeviceManager,int,MockDeviceData>));
    public static Func<MockZombieManager,int,MockZombieData> CompileZombieData26F(MethodDefinition m)=>
        (Func<MockZombieManager,int,MockZombieData>)EmitAdditional(m,"0600026F",typeof(MockZombieData),
            new[]{typeof(MockZombieManager),typeof(int)}).CreateDelegate(typeof(Func<MockZombieManager,int,MockZombieData>));
    public static Action<MockEnemySelecter,int,int,MockWave> CompileFillWave1A8(MethodDefinition m)=>
        (Action<MockEnemySelecter,int,int,MockWave>)EmitAdditional(m,"060001A8",typeof(void),
            new[]{typeof(MockEnemySelecter),typeof(int),typeof(int),typeof(MockWave)})
            .CreateDelegate(typeof(Action<MockEnemySelecter,int,int,MockWave>));
}
