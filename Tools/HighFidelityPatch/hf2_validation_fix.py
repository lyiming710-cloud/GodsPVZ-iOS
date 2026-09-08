from pathlib import Path

p = Path('Tools/HighFidelityPatch/Program.cs')
s = p.read_text(encoding='utf-8')
old = 'var setBool=AnyCall("UnityEngine.Animator","SetBool",2);'
new = '''var setBool=AllRefs().First(x=>x.DeclaringType.FullName=="UnityEngine.Animator"&&x.Name=="SetBool"&&x.Parameters.Count==2&&x.Parameters[0].ParameterType.FullName=="System.String"&&x.Parameters[1].ParameterType.FullName=="System.Boolean");'''
if old not in s:
    raise SystemExit('HF2 SetBool selection site not found')
s = s.replace(old, new, 1)
p.write_text(s, encoding='utf-8')
print('HF2 validation fix applied: Animator.SetBool(string,bool) selected explicitly')
