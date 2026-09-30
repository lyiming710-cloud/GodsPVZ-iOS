from pathlib import Path
import zipfile,hashlib
R=Path(__file__).resolve().parent
with zipfile.ZipFile(R.parent/'stage9-native3/inputs/resolver.zip') as z:
 name='attempt-4/editor-managed-resolver/UnityEngine.AnimationModule.dll'
 b=z.read(name);(R/'resolver/UnityEngine.AnimationModule.dll').write_bytes(b)
 print('UnityEngine.AnimationModule.dll',hashlib.sha256(b).hexdigest())
prior=R.parent/'stage9-native3/candidate/Assembly-CSharp-native3-four-method.dll'
b=prior.read_bytes();assert hashlib.sha256(b).hexdigest()=='72fe4526fecab9e0e6781cf84d7a5aa3ca32f57973a4be3617708ba4c01343bc'
(R/'inputs/native3-prior.dll').write_bytes(b)
