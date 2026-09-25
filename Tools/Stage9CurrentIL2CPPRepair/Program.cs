using System.Security.Cryptography;
using Mono.Cecil;
using Mono.Cecil.Cil;

static class Program
{
    const string ExpectedInputSha = "59bb8e0224787f369653bfdb91fadff0a2d09316e33060d04497b4cab5638c33";
    const uint MouseToken = 0x060001DE;
    const uint DeviceToken = 0x0600017F;

    static IEnumerable<TypeDefinition> All(IEnumerable<TypeDefinition> roots)
    {
        foreach (var t in roots) { yield return t; foreach (var n in All(t.NestedTypes)) yield return n; }
    }
    static string Sha(string p) => Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(p))).ToLowerInvariant();
    static MethodDefinition M(ModuleDefinition m, uint tok) => All(m.Types).SelectMany(t=>t.Methods).Single(x=>x.MetadataToken.ToUInt32()==tok);
    static FieldDefinition F(ModuleDefinition m, uint tok) => All(m.Types).SelectMany(t=>t.Fields).Single(x=>x.MetadataToken.ToUInt32()==tok);
    static MethodReference MR(ModuleDefinition m, uint tok) => m.GetMemberReferences().OfType<MethodReference>().Single(x=>x.MetadataToken.ToUInt32()==tok);
    static FieldReference FR(ModuleDefinition m, uint tok) => m.GetMemberReferences().OfType<FieldReference>().Single(x=>x.MetadataToken.ToUInt32()==tok);

    static MethodReference MoveNext(ModuleDefinition m, TypeReference enumType) => new("MoveNext", m.TypeSystem.Boolean, enumType) { HasThis=true };

    static void Emit(ILProcessor il, OpCode op) => il.Append(Instruction.Create(op));
    static void Emit(ILProcessor il, OpCode op, Instruction x) => il.Append(Instruction.Create(op,x));
    static void Emit(ILProcessor il, OpCode op, VariableDefinition x) => il.Append(Instruction.Create(op,x));
    static void Emit(ILProcessor il, OpCode op, MethodReference x) => il.Append(Instruction.Create(op,x));
    static void Emit(ILProcessor il, OpCode op, FieldReference x) => il.Append(Instruction.Create(op,x));
    static void Emit(ILProcessor il, OpCode op, float x) => il.Append(Instruction.Create(op,x));
    static void Emit(ILProcessor il, OpCode op, int x) => il.Append(Instruction.Create(op,x));

    static void RepairMouse(ModuleDefinition m)
    {
        var md=M(m,MouseToken);
        if (md.FullName!="Plant MouseManager::GetPlantUnderMouse()") throw new Exception("mouse identity mismatch: "+md.FullName);

        var board=F(m,0x04000230); var gridY=F(m,0x04000235); var mousePos=F(m,0x0400023A);
        var plantManager=F(m,0x040003D4); var plants=F(m,0x040002B1);
        var pGridY=F(m,0x040004A1); var fX=F(m,0x040004A4); var fY=F(m,0x040004A5);
        var fW=F(m,0x040004A8); var fD=F(m,0x040004A9); var fH=F(m,0x040004AA);
        var objNe=MR(m,0x0A000006); var getEnum=MR(m,0x0A0000D0); var getCurrent=MR(m,0x0A0000D1); var dispose=MR(m,0x0A000156);
        var enumType=getEnum.ReturnType;
        var moveNext=MoveNext(m,enumType);
        var vectorX=new FieldReference("x",m.TypeSystem.Single,mousePos.FieldType);
        var vectorY=new FieldReference("y",m.TypeSystem.Single,mousePos.FieldType);
        var plantType=md.ReturnType;

        var body=new MethodBody(md){InitLocals=true,MaxStackSize=8}; md.Body=body;
        var result=new VariableDefinition(plantType); var en=new VariableDefinition(enumType); var plant=new VariableDefinition(plantType);
        body.Variables.Add(result); body.Variables.Add(en); body.Variables.Add(plant);
        var il=body.GetILProcessor();

        Emit(il,OpCodes.Ldnull); Emit(il,OpCodes.Stloc,result);
        Emit(il,OpCodes.Ldarg_0); Emit(il,OpCodes.Ldfld,board); Emit(il,OpCodes.Ldfld,plantManager); Emit(il,OpCodes.Ldfld,plants);
        Emit(il,OpCodes.Callvirt,getEnum); Emit(il,OpCodes.Stloc,en);

        var bodyStart=Instruction.Create(OpCodes.Ldloca,en);
        var test=Instruction.Create(OpCodes.Ldloca,en);
        var finallyStart=Instruction.Create(OpCodes.Ldloca,en);
        var afterFinally=Instruction.Create(OpCodes.Ldloc,result);
        var tryStart=Instruction.Create(OpCodes.Br,test); il.Append(tryStart);

        il.Append(bodyStart); Emit(il,OpCodes.Call,getCurrent); Emit(il,OpCodes.Stloc,plant);
        Emit(il,OpCodes.Ldloc,plant); Emit(il,OpCodes.Ldnull); Emit(il,OpCodes.Call,objNe); Emit(il,OpCodes.Brfalse,test);
        Emit(il,OpCodes.Ldloc,plant); Emit(il,OpCodes.Ldfld,pGridY); Emit(il,OpCodes.Ldarg_0); Emit(il,OpCodes.Ldfld,gridY); Emit(il,OpCodes.Bne_Un,test);

        void MouseComponent(FieldReference component){ Emit(il,OpCodes.Ldarg_0); Emit(il,OpCodes.Ldflda,mousePos); Emit(il,OpCodes.Ldfld,component); }
        MouseComponent(vectorX); Emit(il,OpCodes.Ldloc,plant); Emit(il,OpCodes.Ldfld,fX); Emit(il,OpCodes.Ldloc,plant); Emit(il,OpCodes.Ldfld,fW); Emit(il,OpCodes.Ldc_R4,0.5f); Emit(il,OpCodes.Mul); Emit(il,OpCodes.Sub); Emit(il,OpCodes.Blt,test);
        MouseComponent(vectorX); Emit(il,OpCodes.Ldloc,plant); Emit(il,OpCodes.Ldfld,fX); Emit(il,OpCodes.Ldloc,plant); Emit(il,OpCodes.Ldfld,fW); Emit(il,OpCodes.Ldc_R4,0.5f); Emit(il,OpCodes.Mul); Emit(il,OpCodes.Add); Emit(il,OpCodes.Bgt,test);
        MouseComponent(vectorY); Emit(il,OpCodes.Ldloc,plant); Emit(il,OpCodes.Ldfld,fY); Emit(il,OpCodes.Ldloc,plant); Emit(il,OpCodes.Ldfld,fD); Emit(il,OpCodes.Ldc_R4,0.5f); Emit(il,OpCodes.Mul); Emit(il,OpCodes.Sub); Emit(il,OpCodes.Blt,test);
        MouseComponent(vectorY); Emit(il,OpCodes.Ldloc,plant); Emit(il,OpCodes.Ldfld,fY); Emit(il,OpCodes.Ldloc,plant); Emit(il,OpCodes.Ldfld,fH); Emit(il,OpCodes.Add); Emit(il,OpCodes.Bgt,test);
        Emit(il,OpCodes.Ldloc,plant); Emit(il,OpCodes.Stloc,result);

        il.Append(test); Emit(il,OpCodes.Call,moveNext); Emit(il,OpCodes.Brtrue,bodyStart); Emit(il,OpCodes.Leave,afterFinally);
        il.Append(finallyStart); Emit(il,OpCodes.Call,dispose); Emit(il,OpCodes.Endfinally);
        il.Append(afterFinally); Emit(il,OpCodes.Ret);
        body.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally){TryStart=tryStart,TryEnd=finallyStart,HandlerStart=finallyStart,HandlerEnd=afterFinally});
        Console.WriteLine("REPAIRED token=0x060001DE native=0x18031EA60 semantics=foreach(board.plantManager.plants), GridY, fX/fY/fW/fD/fH, mouseWorldPosition.x/y");
    }

    static void RepairDevice(ModuleDefinition m)
    {
        var md=M(m,DeviceToken);
        if (md.FullName!="System.Void DeviceManager::Start_BoardEntry()") throw new Exception("device identity mismatch: "+md.FullName);
        var board=F(m,0x040001D4); var effects=F(m,0x040001D9); var data=F(m,0x040001DA);
        var boardConfig=F(m,0x040003B4); var boardEntries=F(m,0x040001A0);
        var typ=F(m,0x040007FA); var must=F(m,0x040007FC); var select=F(m,0x040007FD); var key=F(m,0x040007FE);
        var atk=F(m,0x040001DB); var hp=F(m,0x040001DC); var def=F(m,0x040001DD);
        var objNe=MR(m,0x0A000006); var strNe=MR(m,0x0A0000AD); var empty=FR(m,0x0A00000F);
        var getEnum=MR(m,0x0A0000E4); var getCurrent=MR(m,0x0A0000E5); var add=MR(m,0x0A0000A3); var dispose=MR(m,0x0A0000A4);
        var getValue=M(m,0x06000607); var enumType=getEnum.ReturnType; var moveNext=MoveNext(m,enumType);
        var entryType=getCurrent.ReturnType;

        var body=new MethodBody(md){InitLocals=true,MaxStackSize=8}; md.Body=body;
        var en=new VariableDefinition(enumType); var entry=new VariableDefinition(entryType); body.Variables.Add(en); body.Variables.Add(entry);
        var il=body.GetILProcessor();
        var ret=Instruction.Create(OpCodes.Ret);

        Emit(il,OpCodes.Ldarg_0); Emit(il,OpCodes.Ldfld,board); Emit(il,OpCodes.Ldnull); Emit(il,OpCodes.Call,objNe); Emit(il,OpCodes.Brfalse,ret);
        Emit(il,OpCodes.Ldarg_0); Emit(il,OpCodes.Ldfld,board); Emit(il,OpCodes.Ldfld,boardConfig); Emit(il,OpCodes.Ldfld,boardEntries); Emit(il,OpCodes.Callvirt,getEnum); Emit(il,OpCodes.Stloc,en);

        var bodyStart=Instruction.Create(OpCodes.Ldloca,en); var process=Instruction.Create(OpCodes.Ldarg_0);
        var test=Instruction.Create(OpCodes.Ldloca,en); var finallyStart=Instruction.Create(OpCodes.Ldloca,en); var afterFinally=Instruction.Create(OpCodes.Br,ret);
        var caseHp=Instruction.Create(OpCodes.Ldarg_0); var caseDef=Instruction.Create(OpCodes.Ldarg_0); var caseAtk=Instruction.Create(OpCodes.Ldarg_0);
        var tryStart=Instruction.Create(OpCodes.Br,test); il.Append(tryStart);

        il.Append(bodyStart); Emit(il,OpCodes.Call,getCurrent); Emit(il,OpCodes.Stloc,entry);
        Emit(il,OpCodes.Ldloc,entry); Emit(il,OpCodes.Ldfld,must); Emit(il,OpCodes.Brtrue,process);
        Emit(il,OpCodes.Ldloc,entry); Emit(il,OpCodes.Ldfld,select); Emit(il,OpCodes.Brfalse,test);
        il.Append(process); Emit(il,OpCodes.Ldfld,effects); Emit(il,OpCodes.Ldloc,entry); Emit(il,OpCodes.Callvirt,add);
        Emit(il,OpCodes.Ldloc,entry); Emit(il,OpCodes.Ldfld,key); Emit(il,OpCodes.Ldsfld,empty); Emit(il,OpCodes.Call,strNe); Emit(il,OpCodes.Brtrue,test);
        Emit(il,OpCodes.Ldloc,entry); Emit(il,OpCodes.Ldfld,typ); Emit(il,OpCodes.Ldc_I4,33); Emit(il,OpCodes.Beq,caseHp);
        Emit(il,OpCodes.Ldloc,entry); Emit(il,OpCodes.Ldfld,typ); Emit(il,OpCodes.Ldc_I4,34); Emit(il,OpCodes.Beq,caseDef);
        Emit(il,OpCodes.Ldloc,entry); Emit(il,OpCodes.Ldfld,typ); Emit(il,OpCodes.Ldc_I4,35); Emit(il,OpCodes.Beq,caseAtk); Emit(il,OpCodes.Br,test);

        void Accumulate(Instruction start, FieldReference field){ il.Append(start); Emit(il,OpCodes.Ldfld,data); Emit(il,OpCodes.Dup); Emit(il,OpCodes.Ldfld,field); Emit(il,OpCodes.Ldloc,entry); Emit(il,OpCodes.Call,getValue); Emit(il,OpCodes.Add); Emit(il,OpCodes.Stfld,field); Emit(il,OpCodes.Br,test); }
        Accumulate(caseHp,hp); Accumulate(caseDef,def); Accumulate(caseAtk,atk);

        il.Append(test); Emit(il,OpCodes.Call,moveNext); Emit(il,OpCodes.Brtrue,bodyStart); Emit(il,OpCodes.Leave,afterFinally);
        il.Append(finallyStart); Emit(il,OpCodes.Call,dispose); Emit(il,OpCodes.Endfinally);
        il.Append(afterFinally); il.Append(ret);
        body.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally){TryStart=tryStart,TryEnd=finallyStart,HandlerStart=finallyStart,HandlerEnd=afterFinally});
        Console.WriteLine("REPAIRED token=0x0600017F native=0x180313B90 semantics=foreach(board.boardConfig.boardEntries), must/select, key, boardEntryType 33=Hp 34=Def 35=Atk");
    }

    public static int Main(string[] args)
    {
        if(args.Length!=2){Console.Error.WriteLine("usage: <input> <output>");return 2;}
        var input=Path.GetFullPath(args[0]); var output=Path.GetFullPath(args[1]);
        var sha=Sha(input); Console.WriteLine($"INPUT sha256={sha}"); if(sha!=ExpectedInputSha){Console.Error.WriteLine("INPUT_HASH_MISMATCH");return 3;}
        using(var asm=AssemblyDefinition.ReadAssembly(input,new ReaderParameters{InMemory=true,ReadSymbols=false}))
        {
            if(asm.MainModule.Kind!=ModuleKind.Dll) throw new Exception("ModuleKind must remain Dll");
            if(All(asm.MainModule.Types).Sum(t=>t.Methods.Count)!=2317) throw new Exception("MethodDef invariant");
            RepairMouse(asm.MainModule); RepairDevice(asm.MainModule);
            Directory.CreateDirectory(Path.GetDirectoryName(output)!); asm.Write(output);
        }
        using(var check=AssemblyDefinition.ReadAssembly(output,new ReaderParameters{InMemory=true,ReadSymbols=false}))
        {
            if(check.MainModule.Kind!=ModuleKind.Dll) throw new Exception("output ModuleKind");
            if(All(check.MainModule.Types).Sum(t=>t.Methods.Count)!=2317) throw new Exception("output MethodDef invariant");
            if(!M(check.MainModule,MouseToken).HasBody || !M(check.MainModule,DeviceToken).HasBody) throw new Exception("target body missing");
        }
        Console.WriteLine($"OUTPUT sha256={Sha(output)}"); Console.WriteLine("METHODDEF_INVARIANT=2317"); Console.WriteLine("WRITE_SCOPE=ONLY_0x060001DE_0x0600017F"); Console.WriteLine("STAGE9_CURRENT_IL2CPP_REPAIR_OK"); return 0;
    }
}
