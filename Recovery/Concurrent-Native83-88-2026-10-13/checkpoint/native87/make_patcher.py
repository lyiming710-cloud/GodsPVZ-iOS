from pathlib import Path
import base64

p = Path('audit/native87-codex-2026-10-13')
p.mkdir(exist_ok=True)
source = Path('audit/native84-codex-2026-10-13/Patcher84.cs').read_text().replace('RepairNative84','RepairNative87')
source = source.replace('d1727135f7fcb7141a62ad5bbe0e2342e6d25efb97d25b59ae223bb26fcd9975', '20ec9187fb3edc51d80cd486a70f6e5e91d7a1958ce4f4db456a2bee7124fa11')
source = source.replace('Pinned untested Native83 parent','Pinned untested Native86 parent')
a=source.index('  var m=M(0x060001C8);')
b=source.index('  foreach(var(token,(code,max))in targets)',a)
source=source[:a]+r'''  var m=M(0x06000199);R(m.Body.Variables[1].VariableType.FullName=="System.Single"&&m.Body.Variables[3].VariableType.FullName=="System.Object"&&m.Body.Variables[11].VariableType.FullName=="System.Object"&&m.Body.Variables[7].VariableType.FullName=="UnityEngine.Camera"&&m.Body.Variables[9].VariableType.FullName=="UnityEngine.Transform"&&m.Body.Variables[10].VariableType.FullName=="UnityEngine.Vector3","Original dialogue captures with proven typed sources");
  var audioType=module.GetTypeReferences().Single(t=>t.FullName=="UnityEngine.AudioClip");R(audioType.MetadataToken.ToInt32()==0x0100002D,"Exact clip TypeRef");
  var clipList=module.LookupToken(0x1B00001A) as Mono.Cecil.TypeReference;R(clipList?.FullName=="System.Collections.Generic.List`1<UnityEngine.AudioClip>","Exact closed List<AudioClip>");
  var il=new IL().Ref(0x28,M(0x06000138)).S(1).O(0x14).S(3)
   .Arg(1).Switch("check-clip","check-clip","index-zero","range-one","range-four","range-seven","range-nine").Branch(0x38,"check-clip")
   .Label("index-zero").Ref(0x7E,F("ResourceManager","dialogueClips")).S(11).Int(0).S(0).Branch(0x38,"list-check")
   .Label("range-one").Ref(0x7E,F("ResourceManager","dialogueClips")).S(11).Int(1).Int(3).Branch(0x38,"random")
   .Label("range-four").Ref(0x7E,F("ResourceManager","dialogueClips")).S(11).Int(4).Int(3).Branch(0x38,"random")
   .Label("range-seven").Ref(0x7E,F("ResourceManager","dialogueClips")).S(11).Int(7).Int(2).Branch(0x38,"random")
   .Label("range-nine").Ref(0x7E,F("ResourceManager","dialogueClips")).S(11).Int(9).Int(3)
   .Label("random").Ref(0x28,0x0A00012B).S(0)
   .Label("list-check").L(11).Ref(0x74,clipList!).O(0x25).Branch(0x3A,"list-present").O(0x26).Branch(0x38,"fail")
   .Label("list-present").L(0).O(0x69).Ref(0x6F,0x0A000084).S(3)
   .Label("check-clip").L(3).Ref(0x74,audioType).O(0x14).Ref(0x28,0x0A00000C).Branch(0x39,"return")
   .Ref(0x28,0x0A000004).S(7).L(7).Branch(0x39,"fail")
   .L(7).Ref(0x6F,0x0A000001).S(9).L(9).Branch(0x39,"fail")
   .L(9).Ref(0x6F,0x0A000037).S(10)
   .L(3).Ref(0x74,audioType).L(10).L(1).Float(1f).Ref(0x28,M(0x0600013E)).O(0x26)
   .Label("return").O(0x2A).Label("fail").O(0x14,0x7A);
  Set(0x06000199,il,4);
  m=M(0x06000386);R(m.Body.Variables[1].VariableType.FullName=="Damage"&&m.Body.Variables[3].VariableType.FullName=="UnityEngine.GameObject"&&m.Body.Variables[4].VariableType.FullName=="UnityEngine.Transform"&&m.Body.Variables[5].VariableType.FullName=="UnityEngine.Vector3"&&m.Body.Variables[6].VariableType.FullName=="UnityEngine.Transform"&&m.Body.Variables[7].VariableType.FullName=="UnityEngine.Camera"&&m.Body.Variables[8].VariableType.FullName=="UnityEngine.Transform"&&m.Body.Variables[11].VariableType.FullName=="UnityEngine.AudioClip"&&m.Body.Variables[12].VariableType.FullName=="UnityEngine.Vector3"&&m.Body.Variables[13].VariableType.FullName=="System.Single"&&m.Body.Variables[16].VariableType.FullName=="System.Single","Exact typed PotatoBoom captures");
  il=new IL().Arg(0).O(0x14).Int(-1).Int(0).Ref(0x28,M(0x0600036D)).S(1).L(1).Branch(0x39,"fail")
   .L(1).Ref(0x6F,M(0x0600010C)).O(0x26)
   .Arg(0).Ref(0x7B,F("Plant","board")).O(0x25).Branch(0x3A,"board-present").O(0x26).Branch(0x38,"fail")
   .Label("board-present").Float(0.08f).Float(5f).Ref(0x6F,M(0x060002CD))
   .Ref(0x28,M(0x060001E5)).O(0x25).Branch(0x3A,"manager-present").O(0x26).Branch(0x38,"fail")
   .Label("manager-present").Int(23).Ref(0x6F,M(0x060001EA)).S(3).L(3).Branch(0x39,"fail")
   .L(3).Ref(0x6F,0x0A00002E).S(4)
   .Arg(0).Ref(0x6F,0x0A000001).S(6).L(6).Branch(0x39,"fail")
   .L(6).Ref(0x6F,0x0A000037).S(5).L(4).Branch(0x39,"fail")
   .L(4).L(5).Ref(0x6F,0x0A000038)
   .L(3).Ref(0x6F,0x0A00002E).O(0x25).Branch(0x3A,"scale-target-present").O(0x26).Branch(0x38,"fail")
   .Label("scale-target-present").Float(1.2f).Float(1.2f).Float(1.2f).Ref(0x73,0x0A00009C).Ref(0x6F,0x0A0000E5)
   .Ref(0x7E,F("ResourceManager","particleClips")).O(0x25).Branch(0x3A,"list-present").O(0x26).Branch(0x38,"fail")
   .Label("list-present").Int(0).Ref(0x6F,0x0A000084).S(11)
   .Ref(0x28,0x0A000004).S(7).L(7).Branch(0x39,"fail")
   .L(7).Ref(0x6F,0x0A000001).S(8).L(8).Branch(0x39,"fail")
   .L(8).Ref(0x6F,0x0A000037).S(12)
   .Ref(0x28,M(0x06000138)).S(13)
   .Float(0.9f).Float(1.1f).Ref(0x28,0x0A000083).S(16)
   .L(11).L(12).L(13).L(16).Ref(0x28,M(0x0600013E)).O(0x26)
   .Arg(0).Ref(0x28,M(0x06000397)).O(0x2A).Label("fail").O(0x14,0x7A);
  Set(0x06000386,il,4);
'''+source[b:]
a=source.index(',scope="')+1;b=source.index('"},new JsonSerializerOptions',a)
source=source[:a]+'scope="Two complete original-native-backed callers; PlayAudio captures AudioVolume first, original seven-entry enum dispatch, captures dialogue list before exact Random pairs and checks it afterwards, retained clip Unity inequality on every path, typed position and pitch1; PotatoBoom discards AreaDamage bool, rereads board, exact Shake order, legal pure Particles getter, delayed first-effect null failure after plant position, second transform getter on same particle, captured clip without null skip and ordered Camera-volume-Random-audio-PlantDie; full bodies only target RVA relocations if oversize, all signatures/metadata rows otherwise intact; dependency M0600013E known clip-length/timeScale ordering gap remains explicitly unqualified pending separate repair; no tests executed; parent chain pending'+source[b:]
(p/'Patcher87.cs').write_text(source)
remote='''from pathlib import Path
import subprocess,base64,zipfile,json,hashlib
w=Path('/workspaces/GodsPVZ-native24-codespace');n=Path('/tmp/godspvz-native87-codex-2026-10-13');n.mkdir(exist_ok=True)
s=n/'Patcher';s.mkdir(exist_ok=True)
(s/'Program.cs').write_bytes(base64.b64decode(PATCHER_B64));(s/'Patcher.csproj').write_bytes((w/'scripts/takeover/native59/Patcher/Patcher.csproj').read_bytes())
c=w/'.validation/native25-2026-10-01/native24-restored/cecil-support/Mono.Cecil.dll'
r=subprocess.run(['dotnet','build',str(s/'Patcher.csproj'),'-c','Release','-o',str(n/'bin'),'-p:MonoCecilPath='+str(c)],capture_output=True,text=True);(n/'build.log').write_text(r.stdout+r.stderr)
if r.returncode:raise RuntimeError(r.stdout+r.stderr)
r=subprocess.run(['dotnet',str(n/'bin/Patcher.dll'),'/tmp/godspvz-native86-codex-2026-10-13/candidate1.dll',str(n/'candidate1.dll'),str(n/'generation1.json')],capture_output=True,text=True);(n/'generation.log').write_text(r.stdout+r.stderr)
if r.returncode:raise RuntimeError(r.stdout+r.stderr)
dest=w/'.validation/native83-86-2026-10-13-checkpoints/native87-generated.zip'
if dest.exists():raise RuntimeError('Fresh persistent checkpoint required')
with zipfile.ZipFile(dest,'w',zipfile.ZIP_DEFLATED,compresslevel=6)as z:
 for f in n.rglob('*'):
  if f.is_file() and 'obj' not in f.parts:z.write(f,'build/native87/'+f.relative_to(n).as_posix())
print(json.dumps({'checkpoint_path':str(dest),'checkpoint_bytes':dest.stat().st_size,'checkpoint_sha256':hashlib.sha256(dest.read_bytes()).hexdigest()}))
print((n/'generation1.json').read_text())
'''.replace('PATCHER_B64',repr(base64.b64encode(source.encode()).decode()))
(p/'generate_remote.py').write_text(remote)
