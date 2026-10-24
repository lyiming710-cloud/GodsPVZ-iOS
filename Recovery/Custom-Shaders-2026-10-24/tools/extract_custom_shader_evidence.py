from pathlib import Path
import sys,zipfile,json,hashlib,gc
work=Path(__file__).resolve().parent
sys.path.insert(0,str(work/'python-deps'))
import UnityPy
root=work.parent.parent;out=work/'custom-shader-evidence';out.mkdir(exist_ok=True)
rows=[]
for origin,file in [('PC',root/'GodsPVZ_1.0.2.zip'),('ANDROID',root/'1.0.2Android/GodsPVZ_1.0.2_Android.apk')]:
    with zipfile.ZipFile(file) as z:
        names=[n for n in z.namelist() if n.endswith(('resources.assets','sharedassets0.assets'))] if origin=='PC' else ['assets/bin/Data/data.unity3d']
        for name in names:
            print('READ',origin,name,flush=True)
            data=z.read(name);env=UnityPy.load(data)
            for obj in env.objects:
                if obj.type.name!='Shader':continue
                shader=obj.read()
                if shader.m_ParsedForm.m_Name not in ('Shader Graphs/Brightness','Unlit/GameUnitShader'):continue
                shader_name=shader.m_ParsedForm.m_Name
                prefix=origin+'-'+shader_name.replace('/','_').replace(' ','_')
                raw=obj.get_raw_data();(out/(prefix+'.bin')).write_bytes(raw)
                row=dict(origin=origin,member=name,shader=shader_name,path_id=obj.path_id,
                    raw_sha256=hashlib.sha256(raw).hexdigest(),raw_bytes=len(raw),platforms=list(shader.platforms))
                try:
                    text=shader.export();(out/(prefix+'.export.txt')).write_text(text,'utf-8')
                    row.update(export_bytes=len(text.encode()),export_sha256=hashlib.sha256(text.encode()).hexdigest(),export_scope='DISASSEMBLY_AND_GLSL_EVIDENCE_NOT_DROP_IN_SHADER_SOURCE')
                except Exception as error:row['export_error']=str(error)
                rows.append(row);print(json.dumps(row),flush=True)
            del data,env;gc.collect()
(out/'MANIFEST.json').write_text(json.dumps(rows,indent=2)+'\n','utf-8')
