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
    public List<MockZombie> zombieList;
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

    // Read EVERY candidate instruction, branch and properly nested/disjoint finally region.
    // Unknown types, member tokens, operand forms or EH shapes fail closed.
    private static DynamicMethod EmitAdditional(MethodDefinition method,string id,Type returnType,Type[] args)
    {
        var typeMap=new Dictionary<string,Type>{
            ["DeviceManager"]=typeof(MockDeviceManager),["DeviceManager/BoardEntryData"]=typeof(MockDeviceData),
            ["ZombieManager"]=typeof(MockZombieManager),["ZombieManager/BoardEntryData"]=typeof(MockZombieData),
            ["BoardEntry"]=typeof(MockBoardEntry),["BoardEntryType"]=typeof(int),["EnemySelecter"]=typeof(MockEnemySelecter),
            ["ZombieSelect"]=typeof(MockZombieSelect),["Wave"]=typeof(MockWave),["UnityEngine.Object"]=typeof(MockUnityObject),
            ["Board"]=typeof(MockUnityObject),["System.Int32"]=typeof(int),["System.Single"]=typeof(float),
            ["System.Boolean"]=typeof(bool),["System.String"]=typeof(string),["System.Void"]=typeof(void),
            ["Zombie"]=typeof(MockZombie),["Damage"]=typeof(MockDamage),["DamageAttribute"]=typeof(int),
            ["Projectile"]=typeof(MockProjectile),["Enemy"]=typeof(MockEnemy),["EnemyPath"]=typeof(MockEnemyPath),
            ["UnityEngine.Vector3"]=typeof(MockVector3),["BoardConfig"]=typeof(MockBoardConfig),
            ["Card_Choose"]=typeof(MockCardChoose),["SeedChooserScreen"]=typeof(MockSeedChooser),
            ["SeedChooserScreen/PlantDetail"]=typeof(MockPlantDetail)
        };
        Type MapType(TypeReference t)
        {
            if(t.FullName=="BoardEntryType"||t.FullName=="DamageAttribute")
            {
                var def=t.Resolve();
                if(!def.IsEnum || def.Fields.Single(f=>f.Name=="value__").FieldType.MetadataType!=MetadataType.Int32)
                    throw new InvalidProgramException(t.FullName+" must be int32-backed");
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
        int expectedEH=id=="0600027B"?2:id=="060006BB"?3:1;
        if(method.Body.ExceptionHandlers.Count!=expectedEH)throw new NotSupportedException(id+" wrong finally count");
        var handlers=method.Body.ExceptionHandlers.ToArray();
        foreach(var eh in handlers)
            if(eh.HandlerType!=ExceptionHandlerType.Finally || eh.TryEnd!=eh.HandlerStart ||
                eh.HandlerEnd==null || eh.HandlerEnd.Previous.OpCode!=COp.Endfinally ||
                !labels.ContainsKey(eh.TryStart)||!labels.ContainsKey(eh.HandlerStart)||!labels.ContainsKey(eh.HandlerEnd))
                throw new NotSupportedException(id+" unmodeled EH layout");
        var activeEH=new Stack<ExceptionHandler>();
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
            [0x04000203]=(typeof(MockZombieSelect),"minFlag"),[0x04000204]=(typeof(MockZombieSelect),"minWave"),
            [0x04000362]=(typeof(MockZombieManager),"zombieList"),[0x040005D2]=(typeof(MockZombie),"isDied"),
            [0x04000123]=(typeof(MockDamage),"damagePoint"),[0x04000125]=(typeof(MockDamage),"damageAttribute"),
            [0x04000129]=(typeof(MockDamage),"anti_air_able"),[0x0400012A]=(typeof(MockDamage),"anti_ground_able"),
            [0x0400012B]=(typeof(MockDamage),"anti_submarine_able"),
            [0x040001F7]=(typeof(MockEnemy),"id"),[0x040001F8]=(typeof(MockEnemy),"row"),
            [0x040001F9]=(typeof(MockEnemy),"gridX"),[0x040001FA]=(typeof(MockEnemy),"position"),
            [0x040001FB]=(typeof(MockEnemy),"wave"),[0x040001FC]=(typeof(MockEnemy),"waitingTime"),
            [0x040001FD]=(typeof(MockEnemy),"myPath"),[0x040001FE]=(typeof(MockEnemy),"entryKey"),
            [0x040003A5]=(typeof(MockEnemyPath),"gridX"),[0x040003A6]=(typeof(MockEnemyPath),"gridY"),
            [0x040003A8]=(typeof(MockEnemyPath),"waitingTime"),[0x040003AA]=(typeof(MockEnemyPath),"isEnd"),
            [0x040008B8]=(typeof(MockSeedChooser),"card_ChooseList"),[0x040008BE]=(typeof(MockSeedChooser),"plantDetail"),
            [0x04000448]=(typeof(MockCardChoose),"isChosen"),[0x04000429]=(typeof(MockCardChoose),"plantID"),
            [0x0A000013]=(typeof(string),"Empty")
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
            [0x060001A6]=("AddEnemy",typeof(MockEnemySelecter).GetMethod("AddEnemy")),
            [0x0A0000D8]=(".ctor",typeof(List<MockZombie>).GetConstructor(Type.EmptyTypes)),
            [0x0A0000DB]=("Add",typeof(List<MockZombie>).GetMethod("Add")),
            [0x06000112]=(".ctor",typeof(MockDamage).GetConstructor(Type.EmptyTypes)),
            [0x0600047F]=("TakeDamage",typeof(MockZombie).GetMethod("TakeDamage")),
            [0x0A000130]=(".ctor",typeof(List<MockEnemyPath>).GetConstructor(Type.EmptyTypes)),
            [0x0A000224]=("Clear",typeof(List<MockEnemyPath>).GetMethod("Clear")),
            [0x0A000151]=("Add",typeof(List<MockEnemyPath>).GetMethod("Add")),
            [0x060002A4]=(".ctor",typeof(MockEnemyPath).GetConstructor(new[]{typeof(int),typeof(int),typeof(float),typeof(MockBoardConfig)})),
            [0x060002F5]=("Choose_Reset",typeof(MockCardChoose).GetMethod("Choose_Reset")),
            [0x060002F4]=("Choose_Being",typeof(MockCardChoose).GetMethod("Choose_Being")),
            [0x060006C2]=("Awake",typeof(MockPlantDetail).GetMethod("Awake"))
        };
        var getEnum=new Dictionary<int,Type>{[0x0A0000E6]=typeof(MockBoardEntry),[0x0A000445]=typeof(MockZombieSelect),
            [0x0A0000D9]=typeof(MockZombie),[0x0A00022A]=typeof(MockEnemyPath),[0x0A000426]=typeof(MockCardChoose)};
        foreach(var t in new[]{typeof(MockBoardEntry),typeof(MockZombieSelect)})
        {
            bool b=t==typeof(MockBoardEntry);
            var e=typeof(MockTrackedListEnumerator<>).MakeGenericType(t);
            calls[b?0x0A0000E7:0x0A000447]=("get_Current",e.GetMethod("get_Current"));
            calls[b?0x0A0000E8:0x0A000446]=("MoveNext",e.GetMethod("MoveNext"));
            calls[b?0x0A0000A7:0x0A000138]=("Dispose",e.GetMethod("Dispose"));
        }
        foreach(var (t,current,move,dispose) in new[]{
            (typeof(MockZombie),0x0A0000DA,0x0A0000DC,0x0A00015E),
            (typeof(MockEnemyPath),0x0A00022B,0x0A00022C,0x0A000045),
            (typeof(MockCardChoose),0x0A0004B3,0x0A0004B4,0x0A0001DF)})
        {
            var e=typeof(MockTrackedListEnumerator<>).MakeGenericType(t);
            calls[current]=("get_Current",e.GetMethod("get_Current"));
            calls[move]=("MoveNext",e.GetMethod("MoveNext"));
            calls[dispose]=("Dispose",e.GetMethod("Dispose"));
        }
        var ops=typeof(ROp).GetFields(BindingFlags.Public|BindingFlags.Static)
            .Where(f=>f.FieldType==typeof(System.Reflection.Emit.OpCode))
            .Select(f=>(System.Reflection.Emit.OpCode)f.GetValue(null)).ToDictionary(o=>o.Value);
        var allowed=new HashSet<Code>{
            Code.Ldarg_0,Code.Ldarg_1,Code.Ldarg_2,Code.Ldarg_3,Code.Ldarga_S,
            Code.Ldloc_0,Code.Ldloc_1,Code.Ldloc_2,Code.Ldloc_3,Code.Ldloc_S,Code.Ldloca_S,
            Code.Stloc_0,Code.Stloc_1,Code.Stloc_2,Code.Stloc_3,Code.Stloc_S,
            Code.Ldfld,Code.Ldsfld,Code.Stfld,Code.Ldnull,Code.Ldc_I4,Code.Ldc_I4_0,Code.Ldc_I4_1,Code.Ldc_I4_2,Code.Ldc_I4_3,Code.Ldc_I4_4,
            Code.Ldc_I4_6,Code.Ldc_R4,Code.Add,Code.Sub,Code.Mul,Code.Div,Code.Rem,Code.Conv_R4,
            Code.Br,Code.Brfalse,Code.Brtrue,Code.Beq,Code.Bne_Un,Code.Bgt,Code.Ble,
            Code.Call,Code.Callvirt,Code.Newobj,Code.Leave,Code.Endfinally,Code.Ret,Code.Nop,Code.Pop
        };
        foreach(var ins in method.Body.Instructions)
        {
            if(!allowed.Contains(ins.OpCode.Code))throw new NotSupportedException(id+" unmodeled opcode "+ins);
            if(ins.Operand is Instruction target&&!labels.ContainsKey(target))
                throw new NotSupportedException(id+" branch target outside method");
            foreach(var eh in handlers.Where(e=>ins==e.TryStart).OrderByDescending(e=>e.HandlerEnd.Offset))
            {il.BeginExceptionBlock();activeEH.Push(eh);}
            foreach(var eh in handlers.Where(e=>ins==e.HandlerStart))
            {
                if(activeEH.Count==0||activeEH.Peek()!=eh)throw new NotSupportedException(id+" non-nested EH");
                il.BeginFinallyBlock();
            }
            il.MarkLabel(labels[ins]);
            var code=ins.OpCode;
            if(code==COp.Endfinally)
            {
                if(activeEH.Count==0||activeEH.Peek().HandlerEnd.Previous!=ins)throw new NotSupportedException(id+" mismatched endfinally");
                il.EndExceptionBlock();activeEH.Pop();continue;
            }
            if(code==COp.Ldfld||code==COp.Stfld||code==COp.Ldsfld)
            {
                var f=(FieldReference)ins.Operand;
                if(!fields.TryGetValue(f.MetadataToken.ToInt32(),out var map) || f.Name!=map.Name)
                    throw new NotSupportedException(id+" field binding "+f.FullName);
                var host=map.Type.GetField(map.Name);
                if(host.DeclaringType!=MapType(f.DeclaringType)||host.FieldType!=MapType(f.FieldType))
                    throw new InvalidProgramException(id+" field type mismatch "+f.FullName);
                if((code==COp.Ldsfld)!=host.IsStatic)throw new InvalidProgramException(id+" field staticness mismatch");
                il.Emit(code==COp.Ldfld?ROp.Ldfld:code==COp.Ldsfld?ROp.Ldsfld:ROp.Stfld,host);continue;
            }
            if(code==COp.Call||code==COp.Callvirt||code==COp.Newobj)
            {
                var m=(MethodReference)ins.Operand;int token=m.MetadataToken.ToInt32();
                if(token==0x0A0000B6)
                {
                    if(code!=COp.Call||m.DeclaringType.FullName!="System.Object"||m.Name!=".ctor"||
                        !m.HasThis||m.Parameters.Count!=0||m.ReturnType.MetadataType!=MetadataType.Void)
                        throw new NotSupportedException(id+" Object constructor shape");
                    il.Emit(ROp.Call,typeof(object).GetConstructor(Type.EmptyTypes));continue;
                }
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
        if(activeEH.Count!=0)throw new NotSupportedException(id+" unclosed EH");
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
