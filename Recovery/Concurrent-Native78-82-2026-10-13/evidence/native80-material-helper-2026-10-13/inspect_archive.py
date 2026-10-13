"""Read existing Xcode export tar members and renderer source excerpts; no extraction/build."""
from pathlib import Path
import sys,json,tarfile,re,hashlib
R=Path(__file__).resolve().parents[2]
sys.path.insert(0,str(R/'audit/local-preflight/python-deps'))
sys.path.insert(0,str(R/'audit/stage9-native3/python-deps'))
import zstandard
import dnfile
from dncil.cil.body import CilMethodBody
from dncil.cil.body.reader import CilMethodBodyReaderBytes
archives=['audit/project-archive-2026-09-30/GodsPVZ-Stage9-native18-unsigned-Xcode.tar.zst','audit/project-archive-2026-09-30/assets/GodsPVZ-Stage9-native18-unsigned-Xcode.tar.zst']
results=[]
for rel in archives:
 path=R/rel;out={'archive':rel,'bytes':path.stat().st_size,'archive_sha256':hashlib.sha256(path.read_bytes()).hexdigest(),'entries_read':0,'matches':[],'core_source':[],'core_dlls':[],'error':None,'last_member':None}
 try:
  with path.open('rb')as f:
   with zstandard.ZstdDecompressor().stream_reader(f)as reader:
    with tarfile.open(fileobj=reader,mode='r|')as tf:
     for m in tf:
      out['entries_read']+=1;out['last_member']={'name':m.name,'bytes':m.size}
      if any(s in m.name for s in ['UnityEngine.CoreModule','ManagedStripped','BindingsGenerator','libiPhone-lib.a','UnityFramework.framework','GraphicsScriptBindings']):out['matches'].append({'name':m.name,'bytes':m.size})
      if not m.isfile():continue
      if 'UnityEngine.CoreModule' in m.name and m.name.endswith('.cpp'):
       data=tf.extractfile(m).read();txt=data.decode('utf-8',errors='replace')
       res={'member':m.name,'bytes':len(data),'sha256':hashlib.sha256(data).hexdigest(),'renderer_is_persistent_identifier_occurrences':txt.count('Renderer_IsPersistent'),'renderer_functions':[]}
       for mat in re.finditer(r'^IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR[^\n]*Renderer_(?:IsPersistent|get_material|GetMaterial)_[^\n]*\n',txt,re.M):
        if mat.group(0).rstrip().endswith(';'):continue
        start=mat.start();br=txt.find('{',mat.end());depth=0;end=None
        if br<0:continue
        for i in range(br,len(txt)):
         if txt[i]=='{':depth+=1
         elif txt[i]=='}':
          depth-=1
          if depth==0:end=i+1;break
        if end is not None:res['renderer_functions'].append({'start_line':txt.count('\n',0,start)+1,'text':txt[start:end]})
       out['core_source'].append(res)
      if m.name.endswith('UnityEngine.CoreModule.dll'):
       data=tf.extractfile(m).read();pe=dnfile.dnPE(data=data);T=pe.net.mdtables
       res={'member':m.name,'bytes':len(data),'sha256':hashlib.sha256(data).hexdigest(),'methods':[]}
       for t in T.TypeDef.rows:
        if str(t.TypeName)!='Renderer' or str(t.TypeNamespace)!='UnityEngine':continue
        for mi in t.MethodList:
         x=mi.row
         if str(x.Name)not in ['IsPersistent','get_material','GetMaterial']:continue
         rr={'name':str(x.Name),'token':hex(0x06000000+mi.row_index),'rva':hex(x.Rva),'signature':x.Signature.value.hex(),'internal_call':x.ImplFlags.miInternalCall}
         if x.Rva:
          bb=CilMethodBody(CilMethodBodyReaderBytes(pe.get_data(x.Rva,1024)));h=bb.header_size
          rr.update({'body_bytes':pe.get_data(x.Rva,h+bb.code_size).hex(),'il':[{'offset':i.offset-h,'opcode':str(i.opcode),'operand':str(i.operand)}for i in bb.instructions]})
         res['methods'].append(rr)
       out['core_dlls'].append(res)
 except Exception as e:out['error']=repr(e)
 results.append(out)
print(json.dumps(results,indent=2))
