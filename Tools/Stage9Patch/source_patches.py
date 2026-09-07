from pathlib import Path
p = Path('Tools/Stage9Patch/Program.cs')
s = p.read_text()
s = s.replace(
'''MethodReference AnyCall(string decl,string name,int pc)=>AllRefs().First(x=>x.DeclaringType.FullName.Contains(decl)&&x.Name==name&&x.Parameters.Count==pc);''',
'''MethodReference AnyCall(string decl,string name,int pc){var q=AllRefs().Where(x=>x.DeclaringType.FullName.Contains(decl)&&x.Name==name&&x.Parameters.Count==pc).ToList();if(q.Count==0)throw new Exception($"Missing MethodRef {decl}.{name}/{pc}");return q[0];}''')
s = s.replace(
'''GenericInstanceMethod AnyGeneric(string decl,string name,string ga)=>AllRefs().OfType<GenericInstanceMethod>().First(x=>x.DeclaringType.FullName.Contains(decl)&&x.Name==name&&x.GenericArguments.Any(a=>a.Name==ga||a.FullName==ga));''',
'''GenericInstanceMethod AnyGeneric(string decl,string name,string ga){var q=AllRefs().OfType<GenericInstanceMethod>().Where(x=>x.DeclaringType.FullName.Contains(decl)&&x.Name==name&&x.GenericArguments.Any(a=>a.Name==ga||a.FullName==ga)).ToList();if(q.Count==0)throw new Exception($"Missing Generic MethodRef {decl}.{name}<{ga}>");return q[0];}''')
p.write_text(s)
