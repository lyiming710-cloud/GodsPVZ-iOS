"""Preserve original PC/Android properties and all pass states without export loss."""
from pathlib import Path
import hashlib
import json
import sys
import zipfile

W=Path(__file__).resolve().parent
R=W.parents[1]
sys.path.insert(0,str(W/'python-deps'))
import attrs
import UnityPy

def plain(value):
    if attrs.has(type(value)):return {k:plain(v) for k,v in attrs.asdict(value,recurse=False).items()}
    if isinstance(value,dict):return {str(k):plain(v) for k,v in value.items()}
    if isinstance(value,(list,tuple)):return [plain(v) for v in value]
    if isinstance(value,(str,int,float,bool)) or value is None:return value
    if isinstance(value,bytes):return {'bytes':len(value),'sha256':hashlib.sha256(value).hexdigest()}
    raise TypeError(type(value).__name__)

rows=[]
for origin,archive,member in [
    ('PC',R/'GodsPVZ_1.0.2.zip',None),
    ('ANDROID',R/'1.0.2Android/GodsPVZ_1.0.2_Android.apk','assets/bin/Data/data.unity3d')
]:
    with zipfile.ZipFile(archive) as z:
        if member is None:
            names=[n for n in z.namelist() if n.endswith('/resources.assets')]
            assert len(names)==1;member=names[0]
        env=UnityPy.load(z.read(member))
        for obj in env.objects:
            if obj.type.name!='Shader' or obj.path_id not in (871,886):continue
            shader=obj.read();form=shader.m_ParsedForm
            if form.m_Name not in ('Shader Graphs/Brightness','Unlit/GameUnitShader'):continue
            states=[]
            for sub in form.m_SubShaders:
                states.append({'tags':plain(sub.m_Tags),'lod':sub.m_LOD,'passes':[{'type':p.m_Type,'name':p.m_Name,'tags':plain(p.m_Tags),'state':plain(p.m_State),'vertex_program':plain(p.progVertex),'fragment_program':plain(p.progFragment)} for p in sub.m_Passes]})
            row={'origin':origin,'archive':archive.name,'member':member,'path_id':obj.path_id,'raw_sha256':hashlib.sha256(obj.get_raw_data()).hexdigest(),'shader':form.m_Name,'properties':plain(form.m_PropInfo),'fallback':form.m_FallbackName,'custom_editor':form.m_CustomEditorName,'subshaders':states}
            rows.append(row)
assert len(rows)==4
out=W/'custom-shader-evidence/ORIGINAL-PROPERTIES-AND-STATES.json'
out.write_text(json.dumps(rows,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
for row in rows:
    print(row['origin'],row['shader'])
    for sub in row['subshaders']:
        for p in sub['passes']:
            state=p['state'];print(json.dumps({'pass':p['name'],'blend':state['rtBlend0'],'zwrite':state['zWrite'],'ztest':state['zTest'],'cull':state['culling'],'stencil':state['stencilOp'],'zclip':state['zClip']},ensure_ascii=True))
