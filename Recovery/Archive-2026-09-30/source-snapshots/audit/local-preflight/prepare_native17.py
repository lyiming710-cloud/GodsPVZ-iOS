from pathlib import Path
import json
A=Path(__file__).resolve().parents[1];R=A/'native15-work'
s=(R/'scripts/codespaces/validate_native16.py').read_text()
for a,b in [('native16','native17'),('Native16','Native17'),('NATIVE16','NATIVE17'),('native15','native16')]:s=s.replace(a,b)
s=s.replace("g=previous['g'];work", "g=previous['g'];work")
s=s.replace("3416340aa16234f853f0d50e13c34b4ef382d1233c351ef3b3fcd918abed5d97","51a749bde92ec75e53dbeed5d35d083ba3616d30f4e4fd756af48a0bd3aed1a5").replace("542370a6b40adb7df92fa3bfa7384f867a035b29f3685152ac9a3bc809f64259","7576209f5c31c673434f95975d5e57cd8140f2b16f5608ddb8a521e6e721eadc")
s=s.replace("targets={'System.Void FTRuntime.Internal.SwfAssocList`1::Remove(T)','System.Void VFXAnimationEvent::SetSorting(ParticleState,T)'}", "targets={'System.Void FTRuntime.Internal.SwfList`1::AssignTo(System.Collections.Generic.List`1<T>)'}")
s='\n'.join(l for l in s.splitlines() if "assert not any('SwfList`1::UnorderedRemoveAt'" not in l)+'\n'
(R/'scripts/codespaces/validate_native17.py').write_text(s,newline='\n')
E=R/'Recovery/Native17-2026-09-28';E.mkdir(parents=True,exist_ok=True)
rows=[r for r in json.loads((A/'native15-evidence/mapping.json').read_text()) if r['token']=='0x60008ef']
(E/'mapping.json').write_text(json.dumps(rows,indent=2)+'\n',newline='\n')
for r in rows:
 n=r['type']+'-'+r['va'][2:]+'.asm';(E/n).write_text('\n'.join(l.rstrip() for l in (A/'native15-evidence'/n).read_text().splitlines())+'\n',newline='\n')
