from pathlib import Path
p = Path('Tools/Stage9Patch/Program.cs')
s = p.read_text()
s = s.replace(
'''MethodReference AnyCall(string decl,string name,int pc)=>AllRefs().First(x=>x.DeclaringType.FullName.Contains(decl)&&x.Name==name&&x.Parameters.Count==pc);''',
'''MethodReference AnyCall(string decl,string name,int pc){var q=AllRefs().Where(x=>x.DeclaringType.FullName.Contains(decl)&&x.Name==name&&x.Parameters.Count==pc).ToList();if(q.Count==0)throw new Exception($"Missing MethodRef {decl}.{name}/{pc}");return q[0];}''')
s = s.replace(
'''GenericInstanceMethod AnyGeneric(string decl,string name,string ga)=>AllRefs().OfType<GenericInstanceMethod>().First(x=>x.DeclaringType.FullName.Contains(decl)&&x.Name==name&&x.GenericArguments.Any(a=>a.Name==ga||a.FullName==ga));''',
'''GenericInstanceMethod AnyGeneric(string decl,string name,string ga){var q=AllRefs().OfType<GenericInstanceMethod>().Where(x=>x.DeclaringType.FullName.Contains(decl)&&x.Name==name&&x.GenericArguments.Any(a=>a.Name==ga||a.FullName==ga)).ToList();if(q.Count==0)throw new Exception($"Missing Generic MethodRef {decl}.{name}<{ga}>");return q[0];}''')
s = s.replace(
'''var childCount=AnyCall("UnityEngine.Transform","get_childCount",0); var getChild=AnyCall("UnityEngine.Transform","GetChild",1);''',
'''var transformType=AnyRefsType("UnityEngine.Transform"); var childCount=new MethodReference("get_childCount",module.TypeSystem.Int32,transformType){HasThis=true}; var getChild=new MethodReference("GetChild",transformType,transformType){HasThis=true}; getChild.Parameters.Add(new ParameterDefinition(module.TypeSystem.Int32));''')
s = s.replace('PatchPlantDie();', 'PatchPlantFrameLoop();\nPatchPlantDie();', 1)
p.write_text(s)
