"""Package only a real arm64 Xcode product, and verify the resulting IPA ZIP."""
from pathlib import Path
import hashlib,json,os,plistlib,shutil,subprocess,sys,zipfile

out=Path(sys.argv[1]).resolve();e=out/'evidence';e.mkdir(parents=True,exist_ok=True)
products=out/'DerivedData/Build/Products/Release-iphoneos'
want=os.environ['EXPECTED_BUNDLE_ID'];minimum=os.environ['EXPECTED_MIN_IOS']
hits=[]
for app in products.glob('*.app'):
    with (app/'Info.plist').open('rb') as f:info=plistlib.load(f)
    if info.get('CFBundleIdentifier')==want:hits.append((app,info))
assert len(hits)==1,[(p.name,i.get('CFBundleIdentifier')) for p,i in hits]
app,info=hits[0]
assert info.get('MinimumOSVersion')==minimum,info.get('MinimumOSVersion')
binary=app/info['CFBundleExecutable'];assert binary.is_file()
machos=[binary]
frameworks=app/'Frameworks'
if frameworks.exists():
    for framework in frameworks.glob('*.framework'):
        with (framework/'Info.plist').open('rb') as f:fi=plistlib.load(f)
        machos.append(framework/fi['CFBundleExecutable'])
assert any(p.name=='UnityFramework' for p in machos),'UnityFramework missing'
audit=[]
for p in machos:
    desc=subprocess.check_output(['file',str(p)],text=True).strip()
    assert 'Mach-O' in desc,desc
    arch=subprocess.check_output(['lipo','-archs',str(p)],text=True).strip()
    assert 'arm64' in arch.split(),arch
    load=subprocess.check_output(['otool','-l',str(p)],text=True)
    assert 'LC_BUILD_VERSION' in load or 'LC_VERSION_MIN_IPHONEOS' in load
    audit.append({'path':str(p.relative_to(app)),'file':desc,'architectures':arch})
    (e/(p.name+'-load-commands.txt')).write_text(load)
metadata=list(app.rglob('global-metadata.dat'));assert len(metadata)==1,metadata
source=out/'work/xcode/Data/Managed/Metadata/global-metadata.dat'
def sha(p):
    h=hashlib.sha256()
    with p.open('rb') as f:
        for chunk in iter(lambda:f.read(1024*1024),b''):h.update(chunk)
    return h.hexdigest()
assert sha(metadata[0])==sha(source),'packaged metadata differs from verified export'
root=out/'ipa-root';payload=root/'Payload';payload.mkdir(parents=True,exist_ok=False)
subprocess.run(['ditto',str(app),str(payload/app.name)],check=True)
delivery=out/'delivery';delivery.mkdir(exist_ok=True)
ipa=delivery/'GodsPVZ-Stage9-native19-unsigned.ipa'
assert not ipa.exists(),'refusing to append to a stale IPA'
subprocess.run(['/usr/bin/zip','-qry',str(ipa),'Payload'],cwd=root,check=True)
with zipfile.ZipFile(ipa) as z:
    assert z.testzip() is None,'IPA CRC failure'
    prefix='Payload/'+app.name+'/'
    assert all(n.startswith('Payload/') for n in z.namelist())
    packed=plistlib.loads(z.read(prefix+'Info.plist'))
    assert packed['CFBundleIdentifier']==want
    assert z.getinfo(prefix+info['CFBundleExecutable']).file_size==binary.stat().st_size
    assert any(n.endswith('/global-metadata.dat') for n in z.namelist())
digest=sha(ipa)
(delivery/(ipa.name+'.sha256')).write_text(digest+'  '+ipa.name+'\n')
result={'status':'UNSIGNED_IPA_PACKAGE_PASS','xcode_compile':'PASS','signing':'DISABLED',
        'device_runtime_validation':'NOT_PERFORMED','production_promotion':False,
        'export_run_id':os.environ['EXPORT_RUN_ID'],'export_head_sha':os.environ['EXPORT_HEAD_SHA'],
        'build_run_id':os.environ['GITHUB_RUN_ID'],'build_head_sha':os.environ['GITHUB_SHA'],
        'bundle_id':want,'minimum_ios':packed['MinimumOSVersion'],'ipa':ipa.name,
        'size_bytes':ipa.stat().st_size,'sha256':digest,'metadata_sha256':sha(metadata[0]),'mach_o':audit}
text=json.dumps(result,indent=2)+'\n'
(e/'RESULT.json').write_text(text);(delivery/'RESULT.json').write_text(text)
print(text)
