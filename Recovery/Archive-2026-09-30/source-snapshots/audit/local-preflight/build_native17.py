from pathlib import Path
R=Path(__file__).resolve().parents[1]/'native15-work';p=R/'scripts/takeover/PatcherNative17';p.mkdir(exist_ok=True)
s=(R/'scripts/takeover/PatcherNative16/Program.cs').read_text().replace('PatcherNative16 <native15.dll> <native16.dll>','PatcherNative17 <native16.dll> <native17.dll>').replace('NATIVE16','NATIVE17').replace('0x060000C6u','0x060008EFu')
s='\n'.join(l for l in s.splitlines() if 'dependency token mismatch' not in l)+'\n'
s=s.replace('PatchSorting(mod,target);PatchRemove(mod,FindRemove(mod));PatchUnordered(mod,FindUnordered(mod));','PatchAssign(mod,target);').replace('targets=3 negative_controls=3','targets=1 negative_controls=1')
s=s.replace('x.Name=="VFXAnimationEvent"&&x.Namespace==""','x.FullName=="FTRuntime.Internal.SwfList`1"').replace('x.Name=="SetSorting"&&x.GenericParameters.Count==1&&x.Parameters.Count==2','x.Name=="AssignTo"&&x.Parameters.Count==1&&x.Parameters[0].ParameterType.FullName=="System.Collections.Generic.List`1<T>"')
a=s.index('    static MethodDefinition FindRemove');b=s.index('    static TypeDefinition TD',a)
s=s[:a]+'    static IEnumerable<MethodDefinition> Targets(ModuleDefinition m)=>new[]{FindTarget(m,true)};\n'+s[b:]
a=s.index('    static void PatchUnordered')
s=s[:a]+'''    static void PatchAssign(ModuleDefinition mod,MethodDefinition m){
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
'''
(p/'Program.cs').write_text(s,newline='\n');(p/'PatcherNative17.csproj').write_text((R/'scripts/takeover/PatcherNative16/PatcherNative16.csproj').read_text(),newline='\n')
# Inspector can select overloaded targets by their first parameter's full type.
p=R/'scripts/codespaces/InspectMethod/Program.cs';s=p.read_text().replace('if(args.Length!=4)','if(args.Length<4||args.Length>5)').replace('x.Parameters.Count==int.Parse(args[3]));','x.Parameters.Count==int.Parse(args[3])&&(args.Length==4||x.Parameters[0].ParameterType.FullName==args[4]));');p.write_text(s,newline='\n')
