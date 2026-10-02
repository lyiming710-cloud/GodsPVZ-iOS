from pathlib import Path
import subprocess,json,hashlib
w=Path('/workspaces/GodsPVZ-native24-codespace');n=w/'.validation/native53-2026-10-02';d=n/'mutant-writer';d.mkdir(exist_ok=True);(d/'Writer.csproj').write_bytes((w/'scripts/takeover/native53/Patcher/Patcher.csproj').read_bytes())
(d/'Program.cs').write_text('''using Mono.Cecil;using Mono.Cecil.Cil;using System.Reflection.PortableExecutable;using System.Reflection.Metadata;using System.Reflection.Metadata.Ecma335;using MD=Mono.Cecil.MethodDefinition;byte[]b=File.ReadAllBytes(args[0]);using var a=Mono.Cecil.AssemblyDefinition.ReadAssembly(args[0]);using var p=new PEReader(new MemoryStream(b));int off(int r){var s=p.PEHeaders.SectionHeaders.Single(s=>r>=s.VirtualAddress&&r<s.VirtualAddress+s.SizeOfRawData);return s.PointerToRawData+r-s.VirtualAddress;}var m=(MD)a.MainModule.LookupToken(0x060001A9);int h=off(m.RVA),root=off(p.PEHeaders.CorHeader!.MetadataDirectory.RelativeVirtualAddress);switch(args[2]){
case "wrong-offset":var f=m.Body.Instructions.First(i=>i.OpCode==OpCodes.Ldc_R4);BitConverter.GetBytes(58f).CopyTo(b,h+12+f.Offset+1);break;
case "wrong-priority":m=(MD)a.MainModule.LookupToken(0x06000188);var bottom=m.DeclaringType.Module.GetTypes().Single(t=>t.Name=="Grid").Fields.Single(f=>f.Name=="device_bottom");foreach(var i in m.Body.Instructions.Where(i=>i.Operand is FieldReference f&&f.Name=="device_top"))BitConverter.GetBytes(bottom.MetadataToken.ToInt32()).CopyTo(b,off(m.RVA)+12+i.Offset+1);break;
case "wrong-bound":m=(MD)a.MainModule.LookupToken(0x0600083D);var bound=m.Body.Instructions.Single(i=>i.OpCode==OpCodes.Bge_S);b[off(m.RVA)+12+bound.Offset]=0x30;break;
case "flip-loop":m=(MD)a.MainModule.LookupToken(0x06000903);var j=m.Body.Instructions.Single(i=>i.OpCode==OpCodes.Brfalse_S);b[off(m.RVA)+12+j.Offset]=0x2d;break;
case "old-heap":b[root+p.GetMetadataReader().GetHeapMetadataOffset(HeapIndex.String)+2]^=1;break;
case "locals":m=(MD)a.MainModule.LookupToken(0x06000903);BitConverter.GetBytes(0x110006C7).CopyTo(b,off(m.RVA)+8);break;
case "section-flags":int sh=p.PEHeaders.PEHeaderStartOffset+p.PEHeaders.CoffHeader.SizeOfOptionalHeader+(p.PEHeaders.CoffHeader.NumberOfSections-1)*40;b[sh+36]^=1;break;
case "body-padding":b[h+12+m.Body.CodeSize]=1;break;
case "max-stack":BitConverter.GetBytes((ushort)9).CopyTo(b,h+2);break;
case "non-target":m=(MD)a.MainModule.LookupToken(0x06000091);b[off(m.RVA)+12]^=1;break;
default:throw new Exception();}File.WriteAllBytes(args[1],b);''')
def run(args,name,expected=0):
 p=subprocess.run(args,capture_output=True,text=True,timeout=90);(n/(name+'.log')).write_text(p.stdout+p.stderr);assert p.returncode==expected,(name,p.returncode,p.stdout+p.stderr)
cecil=w/'.validation/native25-2026-10-01/native24-restored/cecil-support/Mono.Cecil.dll';parent=w/'.validation/native52-2026-10-02/candidate1.dll'
run(['dotnet','build',str(d/'Writer.csproj'),'-o',str(d/'built'),'-p:MonoCecilPath='+str(cecil)],'actual-mutator-build');rows=[];scope=[]
semantic=['wrong-offset','wrong-priority','wrong-bound','flip-loop']
for mode in semantic:
 mutant=n/(mode+'-semantic-negative.dll');out=n/(mode+'-actual-negative.json');run(['dotnet',str(d/'built/Writer.dll'),str(n/'candidate1.dll'),str(mutant),mode],mode+'-mutate');run(['dotnet',str(n/'built/RuntimeFixture/RuntimeFixture.dll'),str(mutant),str(n/'NATIVE-ORACLE.tsv'),str(out),'positive-only'],mode+'-actual-negative',2);r=json.loads(out.read_text());assert r['positive_failures']>0 and r['tool_errors']==0;rows.append({'mode':mode,'positive_failures':r['positive_failures'],'tool_errors':0,'harness_watchdogs':r['positive_harness_watchdog_triggers']})
(n/'ACTUAL-DLL-NEGATIVES.json').write_text(json.dumps(rows,indent=2))
for mode in semantic+['old-heap','locals','section-flags','body-padding','max-stack','non-target']:
 mutant=n/(mode+'-scope-negative.dll');out=n/(mode+'-must-not-pass.json');run(['dotnet',str(d/'built/Writer.dll'),str(n/'candidate1.dll'),str(mutant),mode],mode+'-scope-mutate');report=json.loads((n/'patch1.json').read_text());report['candidate_sha256']=hashlib.sha256(mutant.read_bytes()).hexdigest()
 if mode=='max-stack':report['methods'][0]['new_max']=9
 if mode=='locals':report['methods'][3]['local_sig']=0x110006C7
 for row in report['methods']:row['code_sha256']=hashlib.sha256(mutant.read_bytes()[row['header']+12:row['header']+12+row['new_code_size']]).hexdigest()
 forged=n/(mode+'-forged-report.json');forged.write_text(json.dumps(report,indent=2));run(['dotnet',str(n/'built/Audit/Audit.dll'),str(parent),str(mutant),str(forged),str(out)],mode+'-reject',2);assert not out.exists();scope.append({'mode':mode,'detected':True,'forged_candidate_code_report':True})
(n/'BYTE-AUDIT-NEGATIVES.json').write_text(json.dumps(scope,indent=2));print(json.dumps({'actual_DLL':rows,'byte_scope':scope}))
