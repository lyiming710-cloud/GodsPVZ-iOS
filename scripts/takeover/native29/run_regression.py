from pathlib import Path
import subprocess,json,hashlib,base64
w=Path('/workspaces/GodsPVZ-native24-codespace');n=w/'.validation/native29-2026-10-01';prev=w/'.validation/native25-2026-10-01';candidate=n/'trial2.dll';parent=w/'.validation/native28-2026-10-01/native28-final1.dll'
def run(args,name,expected=0):
 p=subprocess.run(args,capture_output=True,text=True);(n/(name+'.log')).write_text(p.stdout+p.stderr);assert p.returncode==expected,(name,p.stdout+p.stderr);print(name,p.stdout[-400:],flush=True)
run(['dotnet',str(n/'patcher-bin/Repair.dll'),str(parent),str(n/'trial3.dll'),str(n/'trial3.json')],'deterministic-repeat');assert candidate.read_bytes()==(n/'trial3.dll').read_bytes()
for name,inp,out in [('wrong-parent',candidate,n/'must-not-exist.dll'),('existing-output',parent,candidate)]:run(['dotnet',str(n/'patcher-bin/Repair.dll'),str(inp),str(out),str(n/(name+'.json'))],name,3)
assert not (n/'must-not-exist.dll').exists()
rows=[]
for name,exe in [('NATIVE28',w/'.validation/native28-2026-10-01/fixture-bin/RuntimeFixture.dll'),('NATIVE27-VERIFIER',w/'.validation/native27-verifier-2026-10-01/fixture-bin/Native27Verifier.dll'),('NATIVE26',w/'.validation/native26-2026-10-01/fixture-bin/RuntimeFixture.dll'),('NATIVE25',prev/'fixture-bin/RuntimeFixture.dll'),('NATIVE24',prev/'previous-fixture-bin/Runtime.dll')]:
 run(['dotnet',str(exe),str(candidate),str(n/(name+'.json'))]+([str(w/'.validation/native27-2026-10-01/fixture-bin/RuntimeFixture.dll')] if name=='NATIVE27-VERIFIER' else []),name);rows.append({'suite':name,'report':name+'.json','sha256':hashlib.sha256((n/(name+'.json')).read_bytes()).hexdigest()})
(n/'REGRESSION.json').write_text(json.dumps({'pass':True,'suites':rows,'candidate_sha256':hashlib.sha256(candidate.read_bytes()).hexdigest()},indent=2))
src=n/'negative-audit';src.mkdir(exist_ok=True);(src/'Mutate.csproj').write_text('<Project Sdk="Microsoft.NET.Sdk"><PropertyGroup><OutputType>Exe</OutputType><TargetFramework>net10.0</TargetFramework><ImplicitUsings>enable</ImplicitUsings></PropertyGroup></Project>')
(src/'Program.cs').write_text('''using System.Reflection.Metadata;using System.Reflection.Metadata.Ecma335;using System.Reflection.PortableExecutable;
byte[] b=File.ReadAllBytes(args[0]);using var pe=new PEReader(new MemoryStream(b));var md=pe.GetMetadataReader();int offset(int rva){var s=pe.PEHeaders.SectionHeaders.Single(s=>rva>=s.VirtualAddress&&rva<s.VirtualAddress+s.SizeOfRawData);return s.PointerToRawData+rva-s.VirtualAddress;}int root=offset(pe.PEHeaders.CorHeader!.MetadataDirectory.RelativeVirtualAddress);int x;if(args[2]=="non-target"){var h=md.MethodDefinitions.First(h=>md.GetMethodDefinition(h).RelativeVirtualAddress>0);var m=md.GetMethodDefinition(h);x=offset(m.RelativeVirtualAddress);x+=(b[x]&3)==2?1:(BitConverter.ToUInt16(b,x)>>12)*4;b[x]^=1;}else if(args[2]=="old-row"){x=root+md.GetTableMetadataOffset(TableIndex.Field)+2;b[x]^=1;}else if(args[2]=="closed-ref"){x=root+md.GetTableMetadataOffset(TableIndex.MemberRef)+(md.GetTableRowCount(TableIndex.MemberRef)-3)*md.GetTableRowSize(TableIndex.MemberRef);BitConverter.GetBytes((ushort)(0x96*8+4)).CopyTo(b,x);}else throw new Exception();File.WriteAllBytes(args[1],b);Console.WriteLine(args[2]+" "+x);''')
run(['dotnet','build',str(src/'Mutate.csproj'),'-c','Release','-o',str(n/'negative-audit-bin')],'negative-audit-build')
neg=[]
for mode in ['non-target','old-row','closed-ref']:
 out=n/(mode+'-negative.dll');run(['dotnet',str(n/'negative-audit-bin/Mutate.dll'),str(candidate),str(out),mode],mode+'-mutate')
 run(['dotnet',str(n/'audit-bin/Audit.dll'),str(parent),str(out),str(n/'trial2.json'),str(n/(mode+'-must-not-pass.json'))],mode+'-audit-reject',2);assert not (n/(mode+'-must-not-pass.json')).exists();neg.append({'mode':mode,'audit_exit':2,'detected':True})
(n/'AUDIT-NEGATIVE.json').write_text(json.dumps(neg,indent=2));print('REGRESSION_AND_ISOLATION_NEGATIVES_PASS')
