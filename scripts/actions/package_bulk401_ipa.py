"""Package an Xcode-built arm64 app; validate actual archive bytes and export metadata."""
from pathlib import Path
import argparse,subprocess,plistlib,zipfile,hashlib,json,struct,os
from bulk401_export_binding import validate as validate_binding
p=argparse.ArgumentParser();p.add_argument('--app',required=True);p.add_argument('--xcode-dir',required=True);p.add_argument('--output-dir',required=True);a=p.parse_args()
app=Path(a.app).resolve();xcode=Path(a.xcode_dir).resolve();out=Path(a.output_dir).resolve()
if not app.is_dir() or app.suffix!='.app':raise ValueError('One compiled .app required')
binding=validate_binding(xcode,os.environ['GITHUB_SHA'])
out.mkdir(parents=True,exist_ok=False);payload=out/'Payload';payload.mkdir();staged=payload/'GodsPVZ.app'
subprocess.run(['ditto',str(app),str(staged)],check=True)
ipa=out/'GodsPVZ_1.0.2_bulk401_unsigned.ipa';subprocess.run(['ditto','-c','-k','--keepParent',str(payload),str(ipa)],check=True)
def sha(b):return hashlib.sha256(b).hexdigest()
def macho(data):
 if len(data)<32:raise ValueError('Truncated Mach-O')
 magic,cpu,subtype,filetype,ncmds,sizeofcmds,flags,reserved=struct.unpack_from('<8I',data)
 if magic!=0xfeedfacf or cpu!=0x100000c:raise ValueError('Require thin arm64 Mach-O')
 at=32;end=32+sizeofcmds;ios_platform=False
 if end>len(data):raise ValueError('Truncated Mach-O load commands')
 for _ in range(ncmds):
  if at+8>end:raise ValueError('Truncated load command')
  cmd,size=struct.unpack_from('<II',data,at)
  if size<8 or at+size>end:raise ValueError('Invalid load command size')
  if cmd==0x1d:
   if size!=16:raise ValueError('Malformed code-signature command')
   if struct.unpack_from('<I',data,at+12)[0]:raise ValueError('Unexpected code signature in unsigned output')
  if cmd==0x32:
   if size<24 or struct.unpack_from('<I',data,at+8)[0]!=2:raise ValueError('Require physical iOS build platform')
   ios_platform=True
  if cmd==0x25:ios_platform=True
  at+=size
 if at!=end or not ios_platform:raise ValueError('Load-command extent or iOS platform mismatch')
 return dict(architecture='arm64',platform='physical_ios',filetype=filetype,bytes=len(data),sha256=sha(data),nonempty_code_signature=False)
with zipfile.ZipFile(ipa) as z:
 if z.testzip() is not None:raise ValueError('ZIP CRC failed')
 names=z.namelist()
 if len(names)!=len(set(names)) or any(n.startswith('/') or '..' in n.split('/') for n in names):raise ValueError('Duplicate/unsafe ZIP entry')
 root='Payload/GodsPVZ.app/';info=plistlib.loads(z.read(root+'Info.plist'))
 if info.get('CFBundleIdentifier')!='com.tipsGodsStudio.godsPVZ':raise ValueError('Bundle identifier mismatch')
 executable=info.get('CFBundleExecutable','')
 if not executable or '/' in executable:raise ValueError('Unsafe bundle executable')
 binary=macho(z.read(root+executable));framework=macho(z.read(root+'Frameworks/UnityFramework.framework/UnityFramework'))
 if binary['filetype']!=2 or framework['filetype']!=6:raise ValueError('Executable/dylib Mach-O role mismatch')
 if any('_CodeSignature' in n.split('/') or n.endswith('.mobileprovision') for n in names):raise ValueError('Unexpected signing/provisioning artifacts')
 metadata=[n for n in names if n.endswith('/Managed/Metadata/global-metadata.dat')]
 if len(metadata)!=1:raise ValueError('Exactly one generated global metadata required')
 exported=(xcode/'Data/Managed/Metadata/global-metadata.dat').read_bytes();packaged=z.read(metadata[0])
 if exported!=packaged:raise ValueError('Packaged metadata differs from exact Unity export')
 report=dict(scope='UNSIGNED_IPA_STRUCTURE_ARCHITECTURE_AND_EXPORT_BINDING_ONLY',ipa_sha256=sha(ipa.read_bytes()),ipa_bytes=ipa.stat().st_size,bundle_id=info['CFBundleIdentifier'],main_binary=binary,unity_framework=framework,metadata_sha256=sha(packaged),metadata_bytes=len(packaged),zip_entries=len(names),candidate_sha256=binding['candidate_sha256'],workflow_head=binding['workflow_head'],device_execution='NOT_PERFORMED',gameplay_equivalence='NOT_PROVEN')
(out/'IPA-VALIDATION.json').write_text(json.dumps(report,indent=2)+'\n','utf-8');print(json.dumps(report))
