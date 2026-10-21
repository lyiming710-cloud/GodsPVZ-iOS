"""Read metadata/CIL as authorship evidence only; never invoke loaded methods."""
from pathlib import Path
import hashlib,json,sys
ROOT=Path(__file__).resolve().parents[2]
sys.dont_write_bytecode=True;sys.path.insert(0,str(ROOT/'audit/stage9-native3/python-deps'))
import dnfile
from dncil.cil.body import CilMethodBody
from dncil.cil.body.reader import CilMethodBodyReaderBytes
sources=[ROOT/'audit/local-preflight/exact-editor-tools/resolver/UnityEngine.CoreModule.dll',
         ROOT/'audit/offline-test-handoff-2026-10-14/payload/references/managed-support/Unity.TextMeshPro.dll']
result={'scope':'READ_ONLY_REFERENCE_METADATA_AND_CIL_NO_METHOD_EXECUTION','project_tests_executed':False,
        'production_dependency_profile_qualified':False,'references':[]}
for path in sources:
    raw=path.read_bytes();pe=dnfile.dnPE(data=raw);md=pe.net.mdtables
    rec={'path':path.relative_to(ROOT).as_posix(),'bytes':len(raw),'sha256':hashlib.sha256(raw).hexdigest(),'methods':[],'fields':[]}
    for t in md.TypeDef.rows:
        name=str(t.TypeNamespace)+'.'+str(t.TypeName)
        for h in t.MethodList:
            m=h.row;n=str(m.Name)
            wanted=(str(t.TypeName)=='Quaternion' and n=='Euler' and m.Signature.value.hex().endswith('0c0c0c')) or (str(t.TypeName)=='Renderer' and n=='set_sharedMaterial') or (str(t.TypeName)=='TMP_Text' and n=='get_textInfo')
            if wanted:
                body=CilMethodBody(CilMethodBodyReaderBytes(pe.get_data(m.Rva,2048)))
                instructions=[]
                for ins in body.instructions:
                    value=ins.operand;operand=None if value is None else str(value)
                    if hasattr(value,'value'):
                        token=value.value;operand={'token':f'0x{token:08X}'}
                        if token>>24==6:operand['method_name']=str(md.MethodDef.rows[(token&0xFFFFFF)-1].Name)
                        elif token>>24==4:operand['field_name']=str(md.Field.rows[(token&0xFFFFFF)-1].Name)
                    instructions.append({'offset':ins.offset-body.header_size,'opcode':str(ins.opcode),'operand':operand})
                rec['methods'].append({'owner':name,'name':n,'signature':m.Signature.value.hex(),'public':bool(m.Flags.mdPublic),
                    'rva':m.Rva,'body_sha256':hashlib.sha256(body.raw_bytes).hexdigest(),'instructions':instructions})
        if str(t.TypeName)=='TMP_MeshInfo':
            for h in t.FieldList:
                f=h.row
                if str(f.Name) in ['vertices','uvs0','uvs2','colors32']:
                    rec['fields'].append({'owner':name,'name':str(f.Name),'signature':f.Signature.value.hex(),'public':bool(f.Flags.fdPublic)})
    result['references'].append(rec)
parent=dnfile.dnPE(str(ROOT/'audit/native93-codex-2026-10-17/verification-2026-10-17/candidates/candidate1.dll')).net.mdtables
m=parent.MethodDef.rows[0x90C-1]
result['hash_helper']={'token':'0x0600090C','name':str(m.Name),'signature':m.Signature.value.hex(),'assembly_visibility':bool(m.Flags.mdAssem),
                       'behavior_execution_or_acceptance':False}
maps=json.loads((ROOT/'audit/offline-test-handoff-2026-10-14/payload/inputs/maps/native-fields.json').read_text('utf-8-sig'))
result['native_tmp_fields']=[f for f in maps if f['type'] in ['TMP_CharacterInfo','TMP_MeshInfo','TMP_TextInfo'] and f['name'] in ['materialReferenceIndex','vertexIndex','vertices','colors32','uvs0','uvs2','characterCount','characterInfo','meshInfo']]
result['value_type_offset_note']='Native field map includes the boxed 16-byte object header for these value types; actual struct/array element field offsets subtract16. Array data starts at32. No raw pointer offsets are emitted in the new CIL.'
(Path(__file__).resolve().parent/'REFERENCE-BINDINGS.json').write_text(json.dumps(result,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
print(json.dumps({'scope':result['scope'],'references':len(result['references']),'project_tests_executed':False}))
