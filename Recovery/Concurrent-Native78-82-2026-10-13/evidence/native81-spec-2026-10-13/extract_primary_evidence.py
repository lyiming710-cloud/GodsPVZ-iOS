"""Static-only native81 extraction; does not execute any assembly or native body."""
from pathlib import Path
import hashlib
import importlib.util
import json
import re
import struct
import sys
sys.dont_write_bytecode = True
ROOT = Path(__file__).resolve().parents[2]
OUT = Path(__file__).resolve().parent
sys.path.insert(0, str(ROOT / 'audit/stage9-native4/python-deps'))
import capstone
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
from dncil.clr.token import Token
probe_path = ROOT / 'audit/stage9-native3/repository/Tools/Stage9NativeProbe/pc_method_probe.py'
spec = importlib.util.spec_from_file_location('probe81', probe_path)
probe = importlib.util.module_from_spec(spec)
sys.modules[spec.name] = probe
spec.loader.exec_module(probe)
native_path = ROOT / 'audit/stage9-native3/inputs/GameAssembly.dll'
meta_path = ROOT / 'audit/stage9-native3/inputs/global-metadata.dat'
raw, meta = native_path.read_bytes(), meta_path.read_bytes()
pe = probe.PE(raw)
sha = lambda x: hashlib.sha256(x).hexdigest()
assert sha(raw) == '9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d'
assert sha(meta) == 'ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9'
read = lambda va,n: raw[pe.va_to_offset(va):pe.va_to_offset(va)+n]
nq = lambda va: struct.unpack('<Q',read(va,8))[0]
u = lambda offset: struct.unpack_from('<I',meta,offset)[0]
so, to, ts, mo, ms = u(24),u(160),u(164),u(48),u(52)
st = lambda i: meta[so+i:meta.index(0,so+i)].decode('utf-8')
types = []
for i in range(ts//88):
    p=to+i*88
    types.append({'index':i,'name':st(u(p)),'namespace':st(u(p+4)),'field_start':u(p+32),'method_start':u(p+36),
                  'methods':struct.unpack_from('<H',meta,p+64)[0],'fields':struct.unpack_from('<H',meta,p+68)[0]})
nested_owner={}
for i in range(len(types)):
    p=to+i*88;start=u(p+48);count=struct.unpack_from('<H',meta,p+72)[0]
    for j in range(count):nested_owner[u(u(128)+4*(start+j))]=i
def original_typename(index):
    owner=types[index]
    if index in nested_owner:return original_typename(nested_owner[index])+'/'+owner['name']
    return (owner['namespace']+'.' if owner['namespace'] else '')+owner['name']
def original_method(index):
    p=mo+index*36
    owner=types[u(p+4)]
    return {'global_method_index':index,'owner':original_typename(u(p+4)),'name':st(u(p)),
            'token':f'0x{u(p+24):08X}','flags':hex(struct.unpack_from('<H',meta,p+28)[0]),
            'parameter_count':struct.unpack_from('<H',meta,p+34)[0]}
reg_candidates=[]
pat=struct.pack('<Q',len(types))
for match in re.finditer(re.escape(pat),raw):
    p=match.start()
    if raw[p+16:p+24] != pat: continue
    offsets=struct.unpack_from('<Q',raw,p+8)[0]
    try:
        t=next(t for t in types if t['name']=='BoardConfig')
        fp=nq(offsets+8*t['index'])
        if struct.unpack('<ii',read(fp,8)) == (16,24):
            offset_pair_va=next(pe.image_base+s.va+(p-s.raw_offset) for s in pe.sections if s.raw_offset<=p<s.raw_offset+s.raw_size)
            reg_candidates.append(offset_pair_va-80)
    except (ValueError,struct.error,StopIteration): pass
assert len(reg_candidates)==1,reg_candidates
reg=reg_candidates[0]
result={'scope':'Read-only static byte / metadata / MethodBody analysis only; no candidate construction, game/runtime execution, tests, or builds.',
        'original_pe_sha256':sha(raw),'original_metadata_sha256':sha(meta),'metadata_registration_va':hex(reg),
        'generic_inst_count':nq(reg+16),'generic_inst_table':hex(nq(reg+24)),
        'method_spec_count':nq(reg+64),'method_spec_table':hex(nq(reg+72))}
result['closed_generic_method_slots']=[]
for va in (0x181BCAA60,0x181BCAAB0,0x181BCAB08,0x181BC3B60):
    encoded=nq(va); usage=(encoded>>29)&7; index=(encoded&0x1fffffff)>>1
    item={'slot':hex(va),'encoded':hex(encoded),'usage':usage,'index':index}
    if usage==6:
        assert index<nq(reg+64)
        definition,class_inst,method_inst=struct.unpack('<iii',read(nq(reg+72)+12*index,12))
        item.update(original_method(definition));item['class_inst_index']=class_inst;item['method_inst_index']=method_inst
        if class_inst>=0:
            inst=nq(nq(reg+24)+8*class_inst);argc=struct.unpack('<I',read(inst,4))[0];argv=nq(inst+8)
            args=[]
            for j in range(argc):
                type_ptr=nq(argv+j*8);data,bits=struct.unpack('<QI',read(type_ptr,12));kind=(bits>>16)&255
                arg={'type_pointer':hex(type_ptr),'type_kind':hex(kind),'data':hex(data),'bits':hex(bits)}
                if kind in (0x11,0x12) and data<len(types):arg['definition']=types[data]
                args.append(arg)
            item['class_inst_arguments']=args
    result['closed_generic_method_slots'].append(item)

source_path=ROOT/'audit/native30-continuation-2026-10-02/native55-native.json'
source=json.loads(source_path.read_text(encoding='utf-8'))
result['native55_source_sha256']=sha(source_path.read_bytes())
result['native_bodies']=[]
pdata=pe.section('.pdata')
runtime=[]
for off in range(pdata.raw_offset,pdata.raw_offset+pdata.raw_size-11,12):
    lo,hi,unwind=struct.unpack_from('<III',raw,off)
    if lo and hi:runtime.append((pe.image_base+lo,pe.image_base+hi,pe.image_base+unwind))
for row in source['methods']:
    if row['token'] not in ('0x060001AC','0x060003C5'):continue
    start,end=int(row['start'],16),int(row['end'],16)
    native=read(start,end-start)
    assert sha(native)==row['native_sha256']
    pointer=nq(probe.DEFAULT_TABLE_VA+((int(row['token'],16)&0xffffff)-1)*8)
    assert pointer==start
    rf=next(r for r in runtime if r[0]==start)
    head=read(rf[2],4);flags=head[0]>>3;codes=head[2];tail=rf[2]+4+((codes+1)//2)*4
    result['native_bodies'].append({'token':row['token'],'name':row['method'],'start':hex(start),'end':hex(end),
        'sha256':sha(native),'pointer_and_slice_match_original':True,'pdata_end':hex(rf[1]),'unwind':hex(rf[2]),
        'unwind_flags':flags,'unwind_code_count':codes,'unwind_tail_va':hex(tail),'unwind_tail_hex_128':read(tail,128).hex()})
    (OUT/(row['token']+'-native55.asm')).write_text(row['disassembly'],encoding='utf-8')
result['project_jump_table']=[{'ID':i,'target_va':hex(pe.image_base+struct.unpack('<I',read(0x180377608+i*4,4))[0])} for i in range(9)]
def compressed_unsigned(va):
    b=read(va,5);first=b[0]
    if first&1==0:return first>>1,va+1
    if first&2==0:return int.from_bytes(b[:2],'little')>>2,va+2
    if first&4==0:return int.from_bytes(b[:3],'little')>>3,va+3
    if first&8==0:return int.from_bytes(b[:4],'little')>>4,va+4
    return int.from_bytes(b[1:5],'little'),va+5
ip_map=0x181A8A76E
count,cursor=compressed_unsigned(ip_map);ip=0x18031A490;ip_rows=[]
for _ in range(count):
    delta,cursor=compressed_unsigned(cursor);encoded_state,cursor=compressed_unsigned(cursor);ip+=delta
    ip_rows.append({'ip':hex(ip),'delta':delta,'state':encoded_state-1})
try_map=0x181A8A466
try_count,cursor=compressed_unsigned(try_map);try_rows=[]
for _ in range(try_count):
    low,cursor=compressed_unsigned(cursor);high,cursor=compressed_unsigned(cursor);catch_high,cursor=compressed_unsigned(cursor)
    handlers=struct.unpack('<I',read(cursor,4))[0]+pe.image_base;cursor+=4
    try_rows.append({'try_low':low,'try_high':high,'catch_high':catch_high,'handler_array':hex(handlers)})
result['enemy_eh4_state_evidence']={'func_info_va':'0x181a8a758','func_info_header':'0x38',
    'unwind_map_va':'0x181a8a765','try_map_va':hex(try_map),'ip_state_map_va':hex(ip_map),'ip_states':ip_rows,'try_blocks':try_rows,
    'handler_array_hex_24':read(0x181a8a46e,24).hex(),
    'handler_continuation_encoded_bytes':'4904','handler_continuation_offset':274,'handler_continuation_va':'0x18031a5a2',
    'catch_type_descriptor_va':'0x181ce43b0','catch_type_descriptor_hex_80':read(0x181ce43b0,80).hex(),
    'note':'Compressed IP/state and try-block tables are decoded. Catch array raw bytes and continuation arithmetic are retained; this is not a complete symbolic C++ catch-type/handler ABI reconstruction.'}
result['constants']=[]
for va in (0x1815A7A40,0x1815A7E70,0x1815A7BE4,0x1815A7BE0,0x1815A7E24):
    b=read(va,4);result['constants'].append({'va':hex(va),'bits':f'0x{struct.unpack("<I",b)[0]:08X}','value':struct.unpack('<f',b)[0],'bytes_le':b.hex()})
result['immediate_115']={'bits':'0x42E60000','value':struct.unpack('<f',struct.pack('<I',0x42e60000))[0]}
fields=json.loads((ROOT/'audit/stage9-native3/evidence/native-fields.json').read_text())
result['original_fields']=[f for f in fields if (f['type']=='Project' and f['offset'] in (0x24,0x48,0x58,0x5c)) or (f['type']=='Board' and f['offset'] in (0x100,0x128)) or (f['type']=='ZombieSelect' and f['name']=='weight')]
dis=capstone.Cs(capstone.CS_ARCH_X86,capstone.CS_MODE_64)
result['original_native_helpers']=[]
for va in (0x1808197F0,0x18066D250,0x180669280,0x180302170,0x18130D350):
    rf=next((r for r in runtime if r[0]==va),None)
    if rf: end=rf[1];bound='exact .pdata'
    else: end=va+1;bound='one byte shared leaf stub'
    blob=read(va,end-va)
    result['original_native_helpers'].append({'start':hex(va),'end':hex(end),'boundary':bound,'sha256':sha(blob),
        'disassembly':[f'{i.address:016x}: {i.bytes.hex():<32} {i.mnemonic} {i.op_str}' for i in dis.disasm(blob,va)]})

def inspect_clr(path,targets,field_targets=None):
    clr=dnfile.dnPE(str(path));t=clr.net.mdtables
    nested={n.NestedClass.row_index:n.EnclosingClass.row_index for n in t.NestedClass.rows} if t.NestedClass else {}
    def typename(rid):
        row=t.TypeDef.rows[rid-1]
        if rid in nested:return typename(nested[rid])+'/'+str(row.TypeName)
        return (str(row.TypeNamespace)+'.' if str(row.TypeNamespace) else '')+str(row.TypeName)
    method_owner={};field_owner={}
    for rid,typedef in enumerate(t.TypeDef.rows,1):
        for ix in typedef.MethodList:method_owner[ix.row_index]=typename(rid)
        for ix in typedef.FieldList:field_owner[ix.row_index]=typename(rid)
    def tokenid(tok):
        table,rid=tok>>24,tok&0xffffff
        if table==6:
            row=t.MethodDef.rows[rid-1];return {'token':f'0x{tok:08X}','owner':method_owner[rid],'name':str(row.Name),'signature_hex':row.Signature.value.hex()}
        if table==4:
            row=t.Field.rows[rid-1];return {'token':f'0x{tok:08X}','owner':field_owner[rid],'name':str(row.Name),'signature_hex':row.Signature.value.hex()}
        if table==10:
            row=t.MemberRef.rows[rid-1];return {'token':f'0x{tok:08X}','name':str(row.Name),'signature_hex':row.Signature.value.hex()}
        if table==2:return {'token':f'0x{tok:08X}','owner':typename(rid)}
        return {'token':f'0x{tok:08X}'}
    out={'path':str(path),'sha256':sha(path.read_bytes()),'methods':[],'fields':[]}
    for rid,row in enumerate(t.MethodDef.rows,1):
        if (method_owner[rid],str(row.Name)) not in targets:continue
        item={**tokenid(0x06000000+rid),'rva':row.Rva,'public':row.Flags.mdPublic,'private':row.Flags.mdPrivate,'assembly_access':row.Flags.mdAssem,'static':row.Flags.mdStatic}
        if row.Rva:
            body=read_method_body_from_bytes(clr.get_data(row.Rva));item.update({'raw_body_hex':body.get_bytes().hex(),'body_sha256':sha(body.get_bytes()),'instructions':[]})
            for ins in body.instructions:
                operand=ins.operand
                if isinstance(operand,Token):operand=tokenid(operand.value)
                elif isinstance(operand,int) and str(ins.opcode.name).startswith(('br','beq','bne','bge','bgt','ble','blt','leave')):operand={'target_cil_offset':operand-body.header_size}
                elif operand is not None:operand=str(operand)
                item['instructions'].append({'offset':ins.offset-body.header_size,'opcode':str(ins.opcode.name),'operand':operand})
        out['methods'].append(item)
    for rid,row in enumerate(t.Field.rows,1):
        if field_targets and (field_owner[rid],str(row.Name)) in field_targets:
            out['fields'].append({**tokenid(0x04000000+rid),'public':row.Flags.fdPublic,'private':row.Flags.fdPrivate,'assembly_access':row.Flags.fdAssembly})
    return out
listowner='System.Collections.Generic.List`1';enumowner=listowner+'/Enumerator'
targets={(listowner,n) for n in ('GetEnumerator','get_Count','get_Item')}|{(enumowner,n) for n in ('.ctor','MoveNext','MoveNextRare','Dispose','get_Current')}
result['resolver_list_bodies']=inspect_clr(ROOT/'audit/stage9-native3/resolver/mscorlib.dll',targets)
result['historical_game_access_and_helpers']=inspect_clr(ROOT/'audit/stage9-native3/inputs/59bb-historical.dll',
    {('Project','PositionRevision_Camera'),('Banks','GetSunBankPosition'),('ProjectManager','GetCoinBankPosition'),('ProjectManager','GetDiamondBankPosition'),('EnemySelecter','ZombieSelectRandomGet')},
    {('Board','banks'),('Board','projectManager'),('Project','board'),('Project','ID'),('Project','movementTracks'),('Project','endPosition'),('ZombieSelect','weight')})
result['resolver_random_range']=inspect_clr(ROOT/'audit/stage9-native3/resolver/UnityEngine.CoreModule.dll',{('UnityEngine.Random','Range')})
extraction_report_path=ROOT/'audit/stage9-native3/resolver/extraction-report.json'
report=json.loads(extraction_report_path.read_text())
result['resolver_provenance']={'source_report_path':str(extraction_report_path),'source_report_sha256':sha(extraction_report_path.read_bytes()),'selected':report['selected']}
current_path=ROOT/'audit/native80-codex-2026-10-13/metadata-current.json'
current=json.loads(current_path.read_text())
result['current_parent_inventory']={'source_path':str(current_path),'source_sha256':sha(current_path.read_bytes()),
    'parent_sha256':current['input_sha256'],
    'targets':[t for t in current['targets'] if t['token'] in ('0x060001AC','0x060003C5')],
    'relevant_refs':[r for r in current['refs'] if r['token'] in ('0x0A000001','0x0A000037','0x0A000083','0x0A000138','0x0A000024','0x0A0000B7','0x0A0000B8','0x0A0000B9','0x0A0000BA')],
    'relevant_type_refs':[r for r in current['type_refs'] if r['token'] in ('0x01000013','0x01000014')],
    'relevant_field_refs':[r for r in current['external_fields'] if r['token'] in ('0x0A00001D','0x0A00008B','0x0A00009D')],
    'relevant_internals':[t for t in current['internals'] if t['name'] in ('ZombieSelect','Project','ProjectManager')],
    'evidence_limit':'Saved current-parent inventory read only; current parent DLL was not loaded independently in this subtask. Board and Banks were not included in the saved internals subset.'}
(OUT/'PRIMARY-EVIDENCE.json').write_text(json.dumps(result,ensure_ascii=False,indent=2),encoding='utf-8')
print(json.dumps({k:result[k] for k in ('scope','original_pe_sha256','original_metadata_sha256','metadata_registration_va','closed_generic_method_slots','native_bodies','project_jump_table','constants','immediate_115','original_fields')},ensure_ascii=False,indent=2))
