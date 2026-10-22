from pathlib import Path
import json,shutil,importlib.util,re,subprocess,sys
v=Path(__file__).resolve().parent;r=v/'revision-r3';r.mkdir()
for p in (v/'revision-r1').iterdir():
    if p.is_file():shutil.copyfile(p,r/p.name)
changes=[]
def edit(file,method,old,new,reason):
    p=r/file;s=p.read_text('utf-8');at=s.index(' '+method+'(');end=s.find('\n    private static ',at+1);end=len(s) if end<0 else end
    body=s[at:end];assert old in body,(method,old);body=body.replace(old,new);p.write_text(s[:at]+body+s[end:],'utf-8');changes.append(dict(file=file,emitter=method,old=old,new=new,reason=reason))
n118='N118.MethodBodies.cs';n117='N117.MethodBodies.cs'
for method,tok in [('N118_BoardEnemyPreviews','0x06000234'),('N118_SetPreCards','0x06000229')]:edit(n118,method,f'.Call({tok});',f'.Call({tok}).Op(0x26);','Preserve call and discard unused returned resource list')
edit(n118,'N118_PlaceDevice','.Call(0x06000672);','.Call(0x06000672).Op(0x26);','Native ignores popup return value')
edit(n118,'N118_DropArmor2','.Call(0x0600046A).Branch','.Call(0x0600046A).Op(0x26).Branch','Native ignores path-finding boolean in ladder branch')
for method in ['N118_SphShot','N118_Skill149']:edit(n118,method,'.Call(0x0600013D)','.Call(0x0600013D).Op(0x26)','Native ignores returned AudioSource')
edit(n118,'N118_SaveNameAudio','.Call(0x0600013B);','.Call(0x0600013B).Op(0x26);','Discard unused audio before leave/ret')
edit(n117,'N117_WindowTip','.Call(0x0600013B);','.Call(0x0600013B).Op(0x26);','Discard unused audio in RenamePlayer helper')
edit(n117,'N117_MousePlant','.Virtual(0x0600019C).Branch','.Virtual(0x0600019C).Op(0x26).Branch','Discard TriggerCheck boolean ignored in original mouse route')
edit(n117,'N117_DevicePreviews','b.Label("afterEntries").Arg(1).Field(0x0400019E)','b.Label("afterEntries").Arg(0).Field(0x04000781).Field(0x0400019E)','Native loads BoardPreview.boardConfig at +0x20')
edit(n117,'N117_DevicePreviews','b.Label("appendPre").Arg(0).Field(0x040001D7).Load(4).Virtual(N117_PreDeviceAdd);','b.Label("appendPre").Arg(0).Load(4).Call(0x060005D2);','Original 0x180396A51/A7E calls CreateDevicePreview_Device')
edit(n118,'N118_FlowerWait','b.New(0x060008C6).Store(5);b.Load(5).Load(1).Call(0x060008C9);','b.Load(1).New(0x060008C6).Store(5);','Original ctor 0x1803CBC80 itself tail-calls Subscribe exactly once')
sp=importlib.util.spec_from_file_location('checker',v.parents[1]/'audit/offline-test-handoff-2026-10-14/payload/tools/frozen/verify_types.py');m=importlib.util.module_from_spec(sp);sp.loader.exec_module(m)
d=json.loads((v/'r1-targets.json').read_text('utf-8'));p=r/'Program.cs';s=p.read_text('utf-8');peaks=[]
for tok in ['0x06000058','0x060003C1','0x06000585']:
    a=next(x for x in d['methods'] if x['token']==tok);old=a['maxStack'];a['maxStack']=65535;peak=m.verify(a,d['types'])['max_stack'];assert peak>old
    pattern=r'(\(Token:'+tok+r', Code:.*?, Max:)'+str(old)+r'(, Locals:)';s,n=re.subn(pattern,lambda z:z[1]+str(peak)+z[2],s);assert n==1
    peaks.append(dict(token=tok,previous_declared=old,actual_computed_peak=peak))
p.write_text(s,'utf-8')
(v/'13-R3-METHOD-CORRECTIONS.json').write_text(json.dumps(dict(state='SOURCE_CORRECTIONS_PENDING_RERUN',changes=changes,max_stack=peaks,partial_revision_r2='NOT_FINAL; earlier edit script aborted and is retained only as construction evidence'),indent=2)+'\n','utf-8')
print('edits',len(changes),'peaks',peaks)
