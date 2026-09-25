using System.Security.Cryptography;
using Mono.Cecil;
using Mono.Cecil.Cil;

static class Program
{
    const string ExpectedInputSha = "18e44a5a50f1da09cf2f1fc0aace0c18d5cf981606bedaacc879b96e8d2f18bd";
    const uint ZombieMouseToken = 0x060001E0;
    const uint EnemySelecterToken = 0x060001B2;

    static IEnumerable<TypeDefinition> All(IEnumerable<TypeDefinition> roots)
    {
        foreach (var t in roots) { yield return t; foreach (var n in All(t.NestedTypes)) yield return n; }
    }
    static string Sha(string p) => Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(p))).ToLowerInvariant();
    static MethodDefinition M(ModuleDefinition m, uint tok) => All(m.Types).SelectMany(t=>t.Methods).Single(x=>x.MetadataToken.ToUInt32()==tok);
    static FieldDefinition F(ModuleDefinition m, uint tok) => All(m.Types).SelectMany(t=>t.Fields).Single(x=>x.MetadataToken.ToUInt32()==tok);
    static MethodReference MR(ModuleDefinition m, uint tok) => m.GetMemberReferences().OfType<MethodReference>().Single(x=>x.MetadataToken.ToUInt32()==tok);
    static FieldReference FR(ModuleDefinition m, uint tok) => m.GetMemberReferences().OfType<FieldReference>().Single(x=>x.MetadataToken.ToUInt32()==tok);

    static TypeReference GenericArg0(TypeReference t)
    {
        if (t is not GenericInstanceType gi || gi.GenericArguments.Count < 1)
            throw new Exception("expected closed generic type: " + t.FullName);
        return gi.GenericArguments[0];
    }

    static TypeReference ClosedEnumerator(MethodReference getEnumerator, TypeReference concreteElement)
    {
        if (getEnumerator.ReturnType is not GenericInstanceType gi)
            throw new Exception("GetEnumerator return is not GenericInstanceType: " + getEnumerator.ReturnType.FullName);
        var closed = new GenericInstanceType(gi.ElementType);
        closed.GenericArguments.Add(concreteElement);
        return closed;
    }

    static MethodReference MoveNext(ModuleDefinition m, TypeReference closedEnumType) =>
        new("MoveNext", m.TypeSystem.Boolean, closedEnumType) { HasThis=true };

    static void Emit(ILProcessor il, OpCode op) => il.Append(Instruction.Create(op));
    static void Emit(ILProcessor il, OpCode op, Instruction x) => il.Append(Instruction.Create(op,x));
    static void Emit(ILProcessor il, OpCode op, VariableDefinition x) => il.Append(Instruction.Create(op,x));
    static void Emit(ILProcessor il, OpCode op, MethodReference x) => il.Append(Instruction.Create(op,x));
    static void Emit(ILProcessor il, OpCode op, FieldReference x) => il.Append(Instruction.Create(op,x));
    static void Emit(ILProcessor il, OpCode op, float x) => il.Append(Instruction.Create(op,x));
    static void Emit(ILProcessor il, OpCode op, int x) => il.Append(Instruction.Create(op,x));

    static void RepairZombieMouse(ModuleDefinition m)
    {
        var md = M(m, ZombieMouseToken);
        if (md.FullName != "Zombie MouseManager::GetZombieUnderMouse(System.Int32)")
            throw new Exception("zombie mouse identity mismatch: " + md.FullName);

        var board = F(m, 0x04000230);
        var mousePos = F(m, 0x0400023A);
        var zombieManager = F(m, 0x0400035F);
        var zombieList = F(m, 0x04000362);

        var fX = F(m, 0x040005AD);
        var fY = F(m, 0x040005AE);
        var fW = F(m, 0x040005B1);
        var fD = F(m, 0x040005B2);
        var fH = F(m, 0x040005B3);

        var canAttacked = M(m, 0x06000431);
        var objNe = MR(m, 0x0A000006);
        var getEnum = MR(m, 0x0A0000D2);
        var getCurrent = MR(m, 0x0A0000D3);
        var dispose = MR(m, 0x0A000155);

        var zombieType = md.ReturnType;
        var listElement = GenericArg0(zombieList.FieldType);
        if (listElement.FullName != zombieType.FullName)
            throw new Exception("zombie list element mismatch: " + listElement.FullName + " != " + zombieType.FullName);

        var enumType = ClosedEnumerator(getEnum, zombieType);
        var moveNext = MoveNext(m, enumType);
        var vectorX = new FieldReference("x", m.TypeSystem.Single, mousePos.FieldType);
        var vectorY = new FieldReference("y", m.TypeSystem.Single, mousePos.FieldType);

        var body = new MethodBody(md) { InitLocals = true, MaxStackSize = 8 };
        md.Body = body;

        var result = new VariableDefinition(zombieType);
        var en = new VariableDefinition(enumType);
        var z = new VariableDefinition(zombieType);
        body.Variables.Add(result);
        body.Variables.Add(en);
        body.Variables.Add(z);

        var il = body.GetILProcessor();

        Emit(il, OpCodes.Ldnull);
        Emit(il, OpCodes.Stloc, result);

        var ret = Instruction.Create(OpCodes.Ret);
        var afterFinally = Instruction.Create(OpCodes.Ldloc, result);

        // if (this.board == null) return null;
        Emit(il, OpCodes.Ldarg_0);
        Emit(il, OpCodes.Ldfld, board);
        Emit(il, OpCodes.Ldnull);
        Emit(il, OpCodes.Call, objNe);
        Emit(il, OpCodes.Brfalse, afterFinally);

        // if (this.board.zombieManager == null) return null;
        Emit(il, OpCodes.Ldarg_0);
        Emit(il, OpCodes.Ldfld, board);
        Emit(il, OpCodes.Ldfld, zombieManager);
        Emit(il, OpCodes.Ldnull);
        Emit(il, OpCodes.Call, objNe);
        Emit(il, OpCodes.Brfalse, afterFinally);

        // en = this.board.zombieManager.zombieList.GetEnumerator();
        Emit(il, OpCodes.Ldarg_0);
        Emit(il, OpCodes.Ldfld, board);
        Emit(il, OpCodes.Ldfld, zombieManager);
        Emit(il, OpCodes.Ldfld, zombieList);
        Emit(il, OpCodes.Callvirt, getEnum);
        Emit(il, OpCodes.Stloc, en);

        var bodyStart = Instruction.Create(OpCodes.Ldloca, en);
        var test = Instruction.Create(OpCodes.Ldloca, en);
        var finallyStart = Instruction.Create(OpCodes.Ldloca, en);
        var tryStart = Instruction.Create(OpCodes.Br, test);
        il.Append(tryStart);

        il.Append(bodyStart);
        Emit(il, OpCodes.Call, getCurrent);
        Emit(il, OpCodes.Stloc, z);

        Emit(il, OpCodes.Ldloc, z);
        Emit(il, OpCodes.Ldnull);
        Emit(il, OpCodes.Call, objNe);
        Emit(il, OpCodes.Brfalse, test);

        // if (plantID == 0 && !z.CanAttacked()) continue;
        var skipCanAttack = Instruction.Create(OpCodes.Nop);
        Emit(il, OpCodes.Ldarg_1);
        Emit(il, OpCodes.Ldc_I4_0);
        Emit(il, OpCodes.Bne_Un, skipCanAttack);
        Emit(il, OpCodes.Ldloc, z);
        Emit(il, OpCodes.Callvirt, canAttacked);
        Emit(il, OpCodes.Brfalse, test);
        il.Append(skipCanAttack);

        void MouseComp(FieldReference c)
        {
            Emit(il, OpCodes.Ldarg_0);
            Emit(il, OpCodes.Ldflda, mousePos);
            Emit(il, OpCodes.Ldfld, c);
        }

        MouseComp(vectorX);
        Emit(il, OpCodes.Ldloc, z);
        Emit(il, OpCodes.Ldfld, fX);
        Emit(il, OpCodes.Ldloc, z);
        Emit(il, OpCodes.Ldfld, fW);
        Emit(il, OpCodes.Ldc_R4, 0.5f);
        Emit(il, OpCodes.Mul);
        Emit(il, OpCodes.Sub);
        Emit(il, OpCodes.Blt, test);

        MouseComp(vectorX);
        Emit(il, OpCodes.Ldloc, z);
        Emit(il, OpCodes.Ldfld, fX);
        Emit(il, OpCodes.Ldloc, z);
        Emit(il, OpCodes.Ldfld, fW);
        Emit(il, OpCodes.Ldc_R4, 0.5f);
        Emit(il, OpCodes.Mul);
        Emit(il, OpCodes.Add);
        Emit(il, OpCodes.Bgt, test);

        MouseComp(vectorY);
        Emit(il, OpCodes.Ldloc, z);
        Emit(il, OpCodes.Ldfld, fY);
        Emit(il, OpCodes.Ldloc, z);
        Emit(il, OpCodes.Ldfld, fD);
        Emit(il, OpCodes.Ldc_R4, 0.5f);
        Emit(il, OpCodes.Mul);
        Emit(il, OpCodes.Sub);
        Emit(il, OpCodes.Blt, test);

        MouseComp(vectorY);
        Emit(il, OpCodes.Ldloc, z);
        Emit(il, OpCodes.Ldfld, fY);
        Emit(il, OpCodes.Ldloc, z);
        Emit(il, OpCodes.Ldfld, fH);
        Emit(il, OpCodes.Add);
        Emit(il, OpCodes.Bgt, test);

        Emit(il, OpCodes.Ldloc, z);
        Emit(il, OpCodes.Stloc, result);

        il.Append(test);
        Emit(il, OpCodes.Call, moveNext);
        Emit(il, OpCodes.Brtrue, bodyStart);
        Emit(il, OpCodes.Leave, afterFinally);

        il.Append(finallyStart);
        Emit(il, OpCodes.Call, dispose);
        Emit(il, OpCodes.Endfinally);

        il.Append(afterFinally);
        Emit(il, OpCodes.Ret);

        body.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally)
        {
            TryStart = tryStart,
            TryEnd = finallyStart,
            HandlerStart = finallyStart,
            HandlerEnd = afterFinally
        });

        Console.WriteLine("REPAIRED token=0x060001E0 native=0x18031EE10 enum=" + enumType.FullName + " semantics=foreach(board.zombieManager.zombieList), plantID, CanAttacked(), fX/fY/fW/fD/fH, mouseWorldPosition.x/y");
    }

    static void RepairEnemySelecter(ModuleDefinition m)
    {
        var md = M(m, EnemySelecterToken);
        if (md.FullName != "EnemySelecter EnemyManager::CreateEnemySelecter()")
            throw new Exception("enemy selecter identity mismatch: " + md.FullName);

        var board = F(m, 0x0400021E);
        var sBoard = F(m, 0x04000209);
        var sEnemyManager = F(m, 0x0400020A);
        var zombieSelects = F(m, 0x04000207);

        var ctorSelecter = M(m, 0x060001AE);
        var loadAllInfo = M(m, 0x06000231);
        var ctorZombieSelect = M(m, 0x060001A5);

        var addZombieSelect = MR(m, 0x0A000146);
        var getEnumInfo = MR(m, 0x0A000141);
        var getCurrentInfo = MR(m, 0x0A000142);
        var moveNextInfo = MR(m, 0x0A000143);
        var disposeInfo = MR(m, 0x0A00013E);

        var selecterType = md.ReturnType;
        var infoListType = loadAllInfo.ReturnType;
        var infoType = GenericArg0(infoListType);
        var infoEnumType = ClosedEnumerator(getEnumInfo, infoType);

        var body = new MethodBody(md) { InitLocals = true, MaxStackSize = 8 };
        md.Body = body;

        var selecter = new VariableDefinition(selecterType);
        var infoList = new VariableDefinition(infoListType);
        var en = new VariableDefinition(infoEnumType);
        var info = new VariableDefinition(infoType);
        var zs = new VariableDefinition(ctorZombieSelect.DeclaringType);

        body.Variables.Add(selecter);
        body.Variables.Add(infoList);
        body.Variables.Add(en);
        body.Variables.Add(info);
        body.Variables.Add(zs);

        var il = body.GetILProcessor();

        // selecter = new EnemySelecter();
        Emit(il, OpCodes.Newobj, ctorSelecter);
        Emit(il, OpCodes.Stloc, selecter);

        // infoList = ResourceManager.Load_zombieInfo_all();
        Emit(il, OpCodes.Call, loadAllInfo);
        Emit(il, OpCodes.Stloc, infoList);

        // en = infoList.GetEnumerator();
        Emit(il, OpCodes.Ldloc, infoList);
        Emit(il, OpCodes.Callvirt, getEnumInfo);
        Emit(il, OpCodes.Stloc, en);

        var bodyStart = Instruction.Create(OpCodes.Ldloca, en);
        var test = Instruction.Create(OpCodes.Ldloca, en);
        var finallyStart = Instruction.Create(OpCodes.Ldloca, en);
        var afterFinally = Instruction.Create(OpCodes.Ldloc, selecter);
        var tryStart = Instruction.Create(OpCodes.Br, test);
        il.Append(tryStart);

        il.Append(bodyStart);
        Emit(il, OpCodes.Call, getCurrentInfo);
        Emit(il, OpCodes.Stloc, info);

        // zs = new ZombieSelect(info);
        Emit(il, OpCodes.Ldloc, info);
        Emit(il, OpCodes.Newobj, ctorZombieSelect);
        Emit(il, OpCodes.Stloc, zs);

        // selecter.zombieSelects.Add(zs);
        Emit(il, OpCodes.Ldloc, selecter);
        Emit(il, OpCodes.Ldfld, zombieSelects);
        Emit(il, OpCodes.Ldloc, zs);
        Emit(il, OpCodes.Callvirt, addZombieSelect);

        il.Append(test);
        Emit(il, OpCodes.Call, moveNextInfo);
        Emit(il, OpCodes.Brtrue, bodyStart);
        Emit(il, OpCodes.Leave, afterFinally);

        il.Append(finallyStart);
        Emit(il, OpCodes.Call, disposeInfo);
        Emit(il, OpCodes.Endfinally);

        il.Append(afterFinally);

        // selecter.board = this.board;
        Emit(il, OpCodes.Ldarg_0);
        Emit(il, OpCodes.Ldfld, board);
        Emit(il, OpCodes.Stfld, sBoard);

        // selecter.enemyManager = this;
        Emit(il, OpCodes.Ldloc, selecter);
        Emit(il, OpCodes.Ldarg_0);
        Emit(il, OpCodes.Stfld, sEnemyManager);

        Emit(il, OpCodes.Ldloc, selecter);
        Emit(il, OpCodes.Ret);

        body.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally)
        {
            TryStart = tryStart,
            TryEnd = finallyStart,
            HandlerStart = finallyStart,
            HandlerEnd = afterFinally
        });

        Console.WriteLine("REPAIRED token=0x060001B2 native=0x180316AB0 enum=" + infoEnumType.FullName + " semantics=new EnemySelecter(), foreach(ZombieInfo), zombieSelects.Add, board/enemyManager link");
    }

    public static int Main(string[] args)
    {
        if (args.Length != 2) { Console.Error.WriteLine("usage: <input> <output>"); return 2; }
        var input = Path.GetFullPath(args[0]); var output = Path.GetFullPath(args[1]);
        var sha = Sha(input); Console.WriteLine("INPUT sha256=" + sha);
        if (sha != ExpectedInputSha) { Console.Error.WriteLine("INPUT_HASH_MISMATCH"); return 3; }

        using (var asm = AssemblyDefinition.ReadAssembly(input, new ReaderParameters { InMemory = true, ReadSymbols = false }))
        {
            if (asm.MainModule.Kind != ModuleKind.Dll) throw new Exception("ModuleKind must remain Dll");
            if (All(asm.MainModule.Types).Sum(t => t.Methods.Count) != 2317) throw new Exception("MethodDef invariant");

            RepairZombieMouse(asm.MainModule);
            RepairEnemySelecter(asm.MainModule);

            Directory.CreateDirectory(Path.GetDirectoryName(output)!);
            asm.Write(output);
        }

        using (var check = AssemblyDefinition.ReadAssembly(output, new ReaderParameters { InMemory = true, ReadSymbols = false }))
        {
            if (check.MainModule.Kind != ModuleKind.Dll) throw new Exception("output ModuleKind");
            if (All(check.MainModule.Types).Sum(t => t.Methods.Count) != 2317) throw new Exception("output MethodDef invariant");
            if (!M(check.MainModule, ZombieMouseToken).HasBody || !M(check.MainModule, EnemySelecterToken).HasBody)
                throw new Exception("target body missing");
            var zm = M(check.MainModule, ZombieMouseToken);
            var es = M(check.MainModule, EnemySelecterToken);
            if (zm.Body.Variables.Any(v => v.VariableType.FullName.Contains("<!0>", StringComparison.Ordinal) || v.VariableType.FullName == "!0"))
                throw new Exception("zombie mouse ownerless generic local remains");
            if (es.Body.Variables.Any(v => v.VariableType.FullName.Contains("<!0>", StringComparison.Ordinal) || v.VariableType.FullName == "!0"))
                throw new Exception("enemy selecter ownerless generic local remains");
        }

        Console.WriteLine("OUTPUT sha256=" + Sha(output));
        Console.WriteLine("METHODDEF_INVARIANT=2317");
        Console.WriteLine("WRITE_SCOPE=ONLY_0x060001E0_0x060001B2");
        Console.WriteLine("CLOSED_FOREACH_ENUMERATORS=Zombie,ZombieInfo");
        Console.WriteLine("STAGE9_CURRENT_IL2CPP_REPAIR_OK");
        return 0;
    }
}