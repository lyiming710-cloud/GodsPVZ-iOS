from pathlib import Path
import argparse,json,hashlib,subprocess,shutil
def sha(path):return hashlib.sha256(Path(path).read_bytes()).hexdigest()
def main():
 parser=argparse.ArgumentParser();parser.add_argument('restored_archive',type=Path);parser.add_argument('new_output',type=Path);args=parser.parse_args();root=args.restored_archive.resolve();out=args.new_output.resolve();assert not out.exists(),'New output directory required'
 manifest=json.loads((root/'MANIFEST.json').read_text())
 for name,row in manifest['files'].items():
  p=(root/name).resolve();assert p.is_relative_to(root) and p.stat().st_size==row['bytes'] and sha(p)==row['sha256'],name
 candidate=root/'inputs/native27.dll';assert sha(candidate)=='ae6ac972e0615666091b7359774f0c0ec70d01d8e5d05d0942324b7d13b34920';out.mkdir(parents=True);src=out/'build-src';src.mkdir()
 for name in ['Program.cs','Native27Verifier.csproj']:shutil.copyfile(root/'tools'/name,src/name)
 def run(cmd,name,expected=0):
  with (out/(name+'.log')).open('w') as log:rc=subprocess.run(cmd,stdout=log,stderr=subprocess.STDOUT).returncode
  assert rc==expected,(name,rc,(out/(name+'.log')).read_text()[-4000:])
 run(['dotnet','build',str(src/'Native27Verifier.csproj'),'-c','Release','-o',str(out/'bin'),'-p:MonoCecilPath='+str(root/'inputs/legacy-fixture/Mono.Cecil.dll')],'build')
 for mode,expected in [('normal',0),('fixture_fault_emit',2),('fixture_fault_invoke',2)]:
  result=out/(mode+'.json');run(['dotnet',str(out/'bin/Native27Verifier.dll'),str(candidate),str(result),str(root/'inputs/legacy-fixture/RuntimeFixture.dll'),mode],mode,expected);d=json.loads(result.read_text());assert d['positive_pass'] and d['positive_cases']==174
  if mode=='normal':assert d['gate_pass'] and d['tool_errors']==0 and len(d['mutants'])==72 and d['legacy_counterexamples_reproduced'] and sum(m['Status']=='CLR_REJECTED' for m in d['mutants'])==2
  else:assert not d['gate_pass'] and d['tool_errors']==1 and sum(m['Status']=='TOOL_ERROR' and not m['Detected'] for m in d['mutants'])==1
 assert sha(candidate)=='ae6ac972e0615666091b7359774f0c0ec70d01d8e5d05d0942324b7d13b34920'
 print(json.dumps({'archive_members_verified':len(manifest['files']),'positive_cases':174,'negative_variants':72,'expected_tool_faults_rejected':2,'candidate_unchanged':True,'scope':'Rebuilt hardened fixture and reproduced controls; no IL2CPP/Clang/Unity/game test'}))
if __name__=='__main__':main()
