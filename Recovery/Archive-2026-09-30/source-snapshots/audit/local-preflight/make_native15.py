from pathlib import Path
r=Path(__file__).resolve().parents[1]/'native15-work'
p=r/'scripts/takeover/PatcherNative15';p.mkdir(parents=True,exist_ok=True)
s=(r/'scripts/takeover/PatcherNative14/Program.cs').read_text(encoding='utf-8')
s=s.replace('PatcherNative14','PatcherNative15').replace('NATIVE14','NATIVE15').replace('<native13.dll> <native14.dll>','<native14.dll> <native15.dll>')
s=s.replace('const uint TargetToken = 0x06000126u;', 'const uint TargetToken = 0x06000290u;')
s=s.replace('using var asm=AssemblyDefinition.ReadAssembly(input);', '''var resolver=new DefaultAssemblyResolver();
        resolver.AddSearchDirectory(Path.Combine(Directory.GetCurrentDirectory(),"Tools/Stage9Native4Recovery/resolver"));
        resolver.AddSearchDirectory(Path.GetDirectoryName(input)!);
        using var asm=AssemblyDefinition.ReadAssembly(input,new ReaderParameters{AssemblyResolver=resolver});''')
s=s.replace('Patch(mod,target);','PatchMap(mod,target); PatchBinding(mod,FindBinding(mod));')
s=s.replace('rt.Body.ExceptionHandlers.Count!=1','rt.Body.ExceptionHandlers.Count!=2')
s=s.replace('x.Name=="ElementManager"','x.Name=="Map"').replace('x.Name=="CreateNewElements"&&x.GenericParameters.Count==1&&x.Parameters.Count==1','x.Name=="RandomGet_Grid_TestPlace"&&x.GenericParameters.Count==1&&x.Parameters.Count==4').replace('CreateNewElements unexpectedly static','Map target unexpectedly static')
s=s.replace('x.HasBody&&x!=target','x.HasBody&&x!=target&&x!=FindBinding(m)')
s=s[:s.index('    static void Patch(')]
s+='''    static MethodDefinition FindBinding(ModuleDefinition m)=>Types(m).Single(x=>x.Name=="VFXAnimationEvent"&&x.Namespace=="").Methods.Single(x=>x.Name=="Binding"&&x.GenericParameters.Count==1&&x.Parameters.Count==4);
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
'''
(p/'Program.cs').write_text(s,encoding='utf-8',newline='\n')
(p/'PatcherNative15.csproj').write_text((r/'scripts/takeover/PatcherNative14/PatcherNative14.csproj').read_text(encoding='utf-8'),encoding='utf-8',newline='\n')
