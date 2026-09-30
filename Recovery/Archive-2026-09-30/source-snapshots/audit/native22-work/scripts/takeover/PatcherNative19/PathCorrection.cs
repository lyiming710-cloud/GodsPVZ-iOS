using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
internal static partial class Program
{
    // PC 0x18035E080: inspect every home grid; retain the shortest successful
    // path endpoint. Both inlined List.Clear operations capture old _size first.
    static void CorrectCreateStartPath(ModuleDefinition mod,MethodDefinition m)
    {
        var zombie=m.DeclaringType;FieldDefinition Z(string n)=>zombie.Fields.Single(f=>f.Name==n);
        var board=Z("board");var map=FR(mod,"Board","map");var config=FR(mod,"Board","boardConfig");
        var rows=FR(mod,"Map","rows");var grids=FR(mod,"Row","grids");var home=FR(mod,"Grid","isHome");
        var prePath=Z("prePath");var path=Z("path");
        var ctor=MR(mod,"EnemyPath",".ctor",4);var distance=MR(mod,"EnemyPath","DistanceStatistics",3);
        var add=mod.GetMemberReferences().OfType<MethodReference>().Concat(Types(mod).SelectMany(t=>t.Methods)).First(x=>x.Name=="Add"&&x.DeclaringType.FullName==prePath.FieldType.FullName);
        MethodReference ListMethod(TypeReference type,string name,TypeReference ret,bool index=false){var mr=new MethodReference(name,ret,type){HasThis=true};if(index)mr.Parameters.Add(new ParameterDefinition(mod.TypeSystem.Int32));return mr;}
        var clear=ListMethod(prePath.FieldType,"Clear",mod.TypeSystem.Void);
        var rowCount=ListMethod(rows.FieldType,"get_Count",mod.TypeSystem.Int32);var gridCount=ListMethod(grids.FieldType,"get_Count",mod.TypeSystem.Int32);
        var generic=add.Parameters[0].ParameterType;
        var rowItem=ListMethod(rows.FieldType,"get_Item",generic,true);var gridItem=ListMethod(grids.FieldType,"get_Item",generic,true);
        Reset(m);var il=m.Body.GetILProcessor();var x=Local(m,mod.TypeSystem.Int32);var y=Local(m,mod.TypeSystem.Int32);var bestDistance=Local(m,mod.TypeSystem.Int32);var candidateDistance=Local(m,mod.TypeSystem.Int32);var best=Local(m,ctor.DeclaringType);var candidate=Local(m,ctor.DeclaringType);var row=Local(m,grids.DeclaringType);
        var ret=Instruction.Create(OpCodes.Ret);var rowTest=Instruction.Create(OpCodes.Ldloc,y);var rowBody=Instruction.Create(OpCodes.Ldarg_0);var gridTest=Instruction.Create(OpCodes.Ldloc,x);var gridBody=Instruction.Create(OpCodes.Ldloc,row);var nextGrid=Instruction.Create(OpCodes.Ldloc,x);var nextRow=Instruction.Create(OpCodes.Ldloc,y);var clearCandidate=Instruction.Create(OpCodes.Ldarg_0);var noBest=Instruction.Create(OpCodes.Ldarg_0);
        void Rows(){il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,board);il.Emit(OpCodes.Ldfld,map);il.Emit(OpCodes.Ldfld,rows);}
        void Clear(){il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,prePath);il.Emit(OpCodes.Callvirt,clear);}
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,board);il.Emit(OpCodes.Ldnull);il.Emit(OpCodes.Call,MR(mod,"UnityEngine.Object","op_Equality",2));il.Emit(OpCodes.Brtrue,ret);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,Z("ID"));il.Emit(OpCodes.Ldc_I4,13);il.Emit(OpCodes.Beq,ret);
        Clear();il.Emit(OpCodes.Ldc_I4,int.MaxValue);il.Emit(OpCodes.Stloc,bestDistance);il.Emit(OpCodes.Ldnull);il.Emit(OpCodes.Stloc,best);il.Emit(OpCodes.Ldc_I4_0);il.Emit(OpCodes.Stloc,y);il.Emit(OpCodes.Br,rowTest);
        il.Append(rowBody);il.Emit(OpCodes.Ldfld,board);il.Emit(OpCodes.Ldfld,map);il.Emit(OpCodes.Ldfld,rows);il.Emit(OpCodes.Ldloc,y);il.Emit(OpCodes.Callvirt,rowItem);il.Emit(OpCodes.Stloc,row);il.Emit(OpCodes.Ldc_I4_0);il.Emit(OpCodes.Stloc,x);il.Emit(OpCodes.Br,gridTest);
        il.Append(gridBody);il.Emit(OpCodes.Ldfld,grids);il.Emit(OpCodes.Ldloc,x);il.Emit(OpCodes.Callvirt,gridItem);il.Emit(OpCodes.Ldfld,home);il.Emit(OpCodes.Brfalse,nextGrid);
        il.Emit(OpCodes.Ldloc,x);il.Emit(OpCodes.Ldloc,y);il.Emit(OpCodes.Ldc_R4,9999f);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,board);il.Emit(OpCodes.Ldfld,config);il.Emit(OpCodes.Newobj,ctor);il.Emit(OpCodes.Stloc,candidate);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,prePath);il.Emit(OpCodes.Ldloc,candidate);il.Emit(OpCodes.Callvirt,add);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Call,zombie.Methods.Single(t=>t.Name=="Path_Finding"));il.Emit(OpCodes.Brfalse,clearCandidate);
        il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,path);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,Z("gridX"));il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,Z("gridY"));il.Emit(OpCodes.Call,distance);il.Emit(OpCodes.Stloc,candidateDistance);
        il.Emit(OpCodes.Ldloc,candidateDistance);il.Emit(OpCodes.Ldloc,bestDistance);il.Emit(OpCodes.Bge,clearCandidate);il.Emit(OpCodes.Ldloc,candidateDistance);il.Emit(OpCodes.Stloc,bestDistance);il.Emit(OpCodes.Ldloc,candidate);il.Emit(OpCodes.Stloc,best);
        il.Append(clearCandidate);il.Emit(OpCodes.Ldfld,prePath);il.Emit(OpCodes.Callvirt,clear);
        il.Append(nextGrid);il.Emit(OpCodes.Ldc_I4_1);il.Emit(OpCodes.Add);il.Emit(OpCodes.Stloc,x);
        il.Append(gridTest);il.Emit(OpCodes.Ldloc,row);il.Emit(OpCodes.Ldfld,grids);il.Emit(OpCodes.Callvirt,gridCount);il.Emit(OpCodes.Blt,gridBody);
        il.Append(nextRow);il.Emit(OpCodes.Ldc_I4_1);il.Emit(OpCodes.Add);il.Emit(OpCodes.Stloc,y);
        il.Append(rowTest);Rows();il.Emit(OpCodes.Callvirt,rowCount);il.Emit(OpCodes.Blt,rowBody);
        il.Emit(OpCodes.Ldloc,best);il.Emit(OpCodes.Brfalse,noBest);il.Emit(OpCodes.Ldarg_0);il.Emit(OpCodes.Ldfld,prePath);il.Emit(OpCodes.Ldloc,best);il.Emit(OpCodes.Callvirt,add);il.Emit(OpCodes.Br,ret);
        il.Append(noBest);il.Emit(OpCodes.Ldnull);il.Emit(OpCodes.Stfld,path);il.Append(ret);
    }
}
