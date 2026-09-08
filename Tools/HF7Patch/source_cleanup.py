from pathlib import Path

p = Path('Tools/HF7Patch/Program.cs')
s = p.read_text(encoding='utf-8')
old = '''    var abs = Ref("System.Math", "Abs", "System.Single");
    var sqrt = Ref("System.Math", "Sqrt", "System.Double");
    var nreCtor = Ref("System.NullReferenceException", ".ctor");'''
new = '''    var core = module.TypeSystem.CoreLibrary;
    var mathType = new TypeReference("System", "Math", module, core);
    var abs = new MethodReference("Abs", module.TypeSystem.Single, mathType) { HasThis = false };
    abs.Parameters.Add(new ParameterDefinition(module.TypeSystem.Single));
    var sqrt = new MethodReference("Sqrt", module.TypeSystem.Double, mathType) { HasThis = false };
    sqrt.Parameters.Add(new ParameterDefinition(module.TypeSystem.Double));
    var nreType = new TypeReference("System", "NullReferenceException", module, core);
    var nreCtor = new MethodReference(".ctor", module.TypeSystem.Void, nreType) { HasThis = true };'''
if old not in s:
    raise SystemExit('HF7 BCL reference block not found')
p.write_text(s.replace(old, new, 1), encoding='utf-8')
print('HF7 BCL references normalized')
