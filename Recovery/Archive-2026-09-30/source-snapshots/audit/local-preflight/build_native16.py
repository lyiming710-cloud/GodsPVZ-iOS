from pathlib import Path
R=Path(__file__).resolve().parents[1]/'native15-work'
p=R/'scripts/takeover/PatcherNative16';p.mkdir(exist_ok=True)
s=(R/'scripts/takeover/PatcherNative15/Program.cs').read_text()
s=s.replace('PatcherNative15 <native14.dll> <native15.dll>','PatcherNative16 <native15.dll> <native16.dll>').replace('NATIVE15','NATIVE16')
s=s.replace('const uint TargetToken = 0x06000290u;', 'const uint TargetToken = 0x060000C6u;')
s=s.replace('var binding=FindBinding(mod);','var targets=Targets(mod).ToArray();')
a=s.index('        if(!linked&&binding.');b=s.index('        if(!fixture&&mod.',a)
s=s[:a]+'''        if(!linked && (FindRemove(mod).MetadataToken.ToUInt32()!=0x060008D9 || FindUnordered(mod).MetadataToken.ToUInt32()!=0x060008E9))throw new InvalidOperationException("dependency token mismatch");
        foreach(var t in targets){var gp=t.HasGenericParameters?t.GenericParameters[0]:t.DeclaringType.GenericParameters[0];if(gp.HasConstraints||gp.Attributes!=GenericParameterAttributes.NonVariant)throw new InvalidOperationException("generic constraint changed");}
        PatchSorting(mod,target);PatchRemove(mod,FindRemove(mod));PatchUnordered(mod,FindUnordered(mod));
        foreach(var t in targets){StackCheck(t);var first=t.Body.Instructions[0];t.Body.Instructions.RemoveAt(0);bool rejected=false;try{StackCheck(t);}catch(InvalidOperationException){rejected=true;}finally{t.Body.Instructions.Insert(0,first);}if(!rejected)throw new InvalidOperationException("stack negative control accepted");}
        Console.WriteLine("STACK_FLOW_PASS targets=3 negative_controls=3");
'''+s[b:]
a=s.index('        StackCheck(rt);');b=s.index('        Console.WriteLine($"NATIVE16',a)
s=s[:a]+'''        foreach(var t in Targets(reopened.MainModule)){StackCheck(t);if(t.Body.ExceptionHandlers.Count!=0)throw new InvalidOperationException("unexpected EH");}
'''+s[b:]
s=s.replace('x.Name=="Map"','x.Name=="VFXAnimationEvent"').replace('x.Name=="RandomGet_Grid_TestPlace"&&x.GenericParameters.Count==1&&x.Parameters.Count==4','x.Name=="SetSorting"&&x.GenericParameters.Count==1&&x.Parameters.Count==2').replace('Map target','Sorting target')
s=s.replace('x!=target&&x!=FindBinding(m)','!Targets(m).Contains(x)')
a=s.index('    static MethodDefinition FindBinding');b=s.index('    static TypeDefinition TD',a)
s=s[:a]+'''    static MethodDefinition FindRemove(ModuleDefinition m)=>Types(m).Single(x=>x.FullName=="FTRuntime.Internal.SwfAssocList`1").Methods.Single(x=>x.Name=="Remove"&&x.Parameters.Count==1);
    static MethodDefinition FindUnordered(ModuleDefinition m)=>Types(m).Single(x=>x.FullName=="FTRuntime.Internal.SwfList`1").Methods.Single(x=>x.Name=="UnorderedRemoveAt"&&x.Parameters.Count==1);
    static IEnumerable<MethodDefinition> Targets(ModuleDefinition m)=>new[]{FindTarget(m,true),FindRemove(m),FindUnordered(m)};
'''+s[b:]
a=s.index('    static GenericInstanceType Enumerator')
s=s[:a]+(Path(__file__).parent/'native16_emitters.txt').read_text()+'\n}\n'
(p/'Program.cs').write_text(s,newline='\n')
(p/'PatcherNative16.csproj').write_text((R/'scripts/takeover/PatcherNative15/PatcherNative15.csproj').read_text(),newline='\n')
