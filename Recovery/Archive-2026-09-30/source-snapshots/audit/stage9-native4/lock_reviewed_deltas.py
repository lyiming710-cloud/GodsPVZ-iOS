from pathlib import Path
R=Path(__file__).resolve().parent
old=(R.parent/'stage9-native3/specification/expected-reference-use-deltas.tsv').read_text().splitlines()
new=(R/'evidence/reference-use-deltas.tsv').read_text().splitlines()
assert new[:len(old)]==old,'prior four-method deltas changed'
expected=[
'[Method:0x0267]\t1\t!0 System.Collections.Generic.List`1/Enumerator<BoardEntry>::get_Current()',
'[Method:0x0267]\t1\tSystem.Boolean System.Collections.Generic.List`1/Enumerator<BoardEntry>::MoveNext()',
'[Method:0x0267]\t-1\tSystem.Boolean System.Collections.Generic.List`1/Enumerator<System.Object>::MoveNext()',
'[Method:0x0267]\t1\tSystem.Collections.Generic.List`1/Enumerator<!0> System.Collections.Generic.List`1<BoardEntry>::GetEnumerator()',
'[Method:0x0267]\t1\tSystem.Void System.Collections.Generic.List`1/Enumerator<BoardEntry>::Dispose()',
'[Method:0x0267]\t-2\tSystem.Void System.NullReferenceException::.ctor()',
'[Method:0x039f]\t1\tSystem.Void UnityEngine.Animator::SetTrigger(System.String)']
assert new[len(old):]==expected,'new method deltas differ from native-backed review'
(R/'specification/expected-reference-use-deltas.tsv').write_text('\n'.join(new)+'\n')
print('native3 use deltas preserved; seven new delta rows reviewed and locked')
