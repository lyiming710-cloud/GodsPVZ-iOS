"""Read original PE/metadata and archived parent CIL. No assembly execution/testing."""
from pathlib import Path
import json, struct, re, hashlib, zipfile, sys
R = Path(__file__).resolve().parents[2]
prefix = (R/'audit/native80-spec-2026-10-13/inspect_original.py').read_text().split('cells=[]', 1)[0]
exec(prefix)
import dnfile
from dncil.cil.body import CilMethodBody
from dncil.cil.body.reader import CilMethodBodyReaderBytes
from capstone import CS_OP_MEM
D = Cs(CS_ARCH_X86, CS_MODE_64)
D.detail = True

def disassemble(va, size):
    result = []
    for i in D.disasm(native(va, size), va):
        row = {'address': hex(i.address), 'bytes': i.bytes.hex(), 'mnemonic': i.mnemonic, 'operands': i.op_str}
        for op in i.operands:
            if op.type == CS_OP_MEM and i.reg_name(op.mem.base) == 'rip':
                row['rip_target'] = hex(i.address + i.size + op.mem.disp)
        result.append(row)
    return result

def nativebody(va, size=None):
    pdata = []
    for entry in pe.DIRECTORY_ENTRY_EXCEPTION:
        e = entry.struct
        if e.BeginAddress + base <= va < e.EndAddress + base:
            uv = e.UnwindData + base
            pdata.append({'begin': hex(e.BeginAddress + base), 'end': hex(e.EndAddress + base),
                'unwind_address': hex(uv), 'unwind_header': native(uv, 4).hex(), 'unwind_flags': native(uv, 1)[0] >> 3})
            if size is None:
                size = e.EndAddress + base - va
    if size is None:
        raise ValueError('Explicit length required for leaf source fragment')
    data = native(va, size)
    return {'start': hex(va), 'end': hex(va+size), 'size': size, 'bytes': data.hex(),
        'sha256': hashlib.sha256(data).hexdigest(), 'pdata': pdata, 'instructions': disassemble(va, size)}

def module(image):
    matches = []
    for m in re.finditer(re.escape(image.encode()+b'\0'), raw):
        nameva = pe.get_rva_from_offset(m.start()) + base
        for ref in re.finditer(re.escape(struct.pack('<Q', nameva)), raw):
            a = ref.start()
            count, ptr = q(raw, a+8), q(raw, a+16)
            if 100 < count < 100000 and base < ptr < base + pe.OPTIONAL_HEADER.SizeOfImage:
                matches.append({'image': image, 'name_address': hex(nameva), 'module_address': hex(pe.get_rva_from_offset(a)+base),
                    'method_count': count, 'pointer_table': hex(ptr)})
    if len(matches) != 1:
        raise ValueError((image, matches))
    return matches[0]

mods = {image: module(image) for image in ['mscorlib.dll', 'Assembly-CSharp.dll']}
object_type = next(t for t in types if t['namespace']=='System' and t['name']=='Object')
object_ctor = next(method(object_type['methodStart']+i) for i in range(object_type['methods'])
    if method(object_type['methodStart']+i)['name']=='.ctor')
rid = int(object_ctor['token'], 16) & 0xffffff
cell = int(mods['mscorlib.dll']['pointer_table'], 16) + 8*(rid-1)
object_mapping = {'type': object_type, 'method': object_ctor, 'module': mods['mscorlib.dll'],
    'pointer_cell': hex(cell), 'pointer_cell_bytes': native(cell, 8).hex(), 'native': hex(nq(cell))}
aliases = []
io, isz = u(b,168), u(b,172)
for im in range(isz//40):
    p = io+40*im
    if st(u(b,p)) not in mods:
        continue
    image = st(u(b,p)); start, count = u(b,p+8), u(b,p+12)
    table = int(mods[image]['pointer_table'],16)
    for t in types[start:start+count]:
        for i in range(t['methods']):
            m = method(t['methodStart']+i)
            c = table + 8*((int(m['token'],16)&0xffffff)-1)
            if nq(c)==0x180302170:
                aliases.append({'image': image, 'owner': m['owner'], 'name': m['name'], 'token': m['token'],
                    'method_index': m['index'], 'parameter_count': len(m['parameters']), 'pointer_cell': hex(c)})

cells = []
for va in [0x181BB6568,0x181BB9B58,0x181BC12D8,0x181BBFB18,0x181BC0568,0x181BC1A08,0x181BA4FF8,0x181BBCA10]:
    value = nq(va); kind = (value>>29)&7; index=(value>>1)&0xfffffff
    item={'cell':hex(va),'raw_bytes':native(va,8).hex(),'encoded':hex(value),'kind':kind,'source_index':index}
    if kind==6:
        item['method_spec']=ms(index)
    elif kind in (1,2):
        item['type']=typ(index)
    cells.append(item)

inventory_path=R/'audit/native83-codex-2026-10-13/metadata-current.json'
inventory=json.loads(inventory_path.read_text())
archive=R/'audit/native82-codex-2026-10-13/godspvz-native78-82-pending-2026-10-13.zip'
member='candidates/native82/candidate1.dll'
with zipfile.ZipFile(archive) as z:
    parent_bytes=z.read(member)
parent=dnfile.dnPE(data=parent_bytes)
refnames={int(x['token'],16):x['name'] for x in inventory['refs']}

def cilbody(assembly, token, names=None):
    T=assembly.net.mdtables
    owners={m.row_index:t for t in T.TypeDef.rows for m in t.MethodList}
    fowners={f.row_index:t for t in T.TypeDef.rows for f in t.FieldList}
    def fullname(t):
        ns=str(t.TypeNamespace)
        return (ns+'.' if ns else '')+str(t.TypeName)
    def identity(tok):
        if names and tok in names: return names[tok]
        table, rid=tok>>24, tok&0xffffff
        if table==6:
            row=T.MethodDef.rows[rid-1]
            return fullname(owners[rid])+'::'+str(row.Name)+' / sig '+row.Signature.value.hex()
        if table==4:
            row=T.Field.rows[rid-1]
            return fullname(fowners[rid])+'::'+str(row.Name)+' / sig '+row.Signature.value.hex()
        if table==10:
            row=T.MemberRef.rows[rid-1]
            return str(row.Name)+' / sig '+row.Signature.value.hex()+' / class '+row.Class.table.name+':'+str(row.Class.row_index)
        if table==0x70: return str(assembly.net.user_strings.get(rid).value)
        return hex(tok)
    m=T.MethodDef.rows[(token&0xffffff)-1]
    body=CilMethodBody(CilMethodBodyReaderBytes(assembly.get_data(m.Rva,4096)))
    result={'token':hex(token),'identity':identity(token),'rva':hex(m.Rva),'signature':m.Signature.value.hex(),
        'code_size':body.code_size,'max_stack':body.max_stack,'local_sig':str(body.local_var_sig_tok),
        'eh_count':len(body.exception_handlers),'body_bytes':assembly.get_data(m.Rva,body.header_size+body.code_size).hex(),'instructions':[]}
    if body.local_var_sig_tok:
        localtoken=body.local_var_sig_tok.value
        result['local_sig_bytes']=T.StandAloneSig.rows[(localtoken&0xffffff)-1].Signature.value.hex()
    for i in body.instructions:
        operand=i.operand
        value=None if operand is None else str(operand)
        if hasattr(operand,'value'):
            value={'token':hex(operand.value),'identity':identity(operand.value)}
        elif isinstance(operand,int) and (str(i.opcode).startswith('br') or str(i.opcode).startswith('leave')):
            value=operand-body.header_size
        result['instructions'].append({'offset':i.offset-body.header_size,'opcode':str(i.opcode),'operand':value})
    return result

current_refs=[]
for token in [0x0A000094,0x0A000095,0x0A000096,0x0A0000B6,0x0A0000BB]:
    row=parent.net.mdtables.MemberRef.rows[(token&0xffffff)-1]
    cls=row.Class
    c={'table':cls.table.name,'row':cls.row_index}
    if cls.table.name=='TypeSpec': c['signature']=cls.row.Signature.value.hex()
    if cls.table.name=='TypeRef':
        c.update({'name':str(cls.row.TypeNamespace)+'.'+str(cls.row.TypeName),
            'scope_table':cls.row.ResolutionScope.table.name,'scope_row':cls.row.ResolutionScope.row_index})
        if cls.row.ResolutionScope.table.name=='AssemblyRef':c['assembly']=str(cls.row.ResolutionScope.row.Name)
    current_refs.append({'token':hex(token),'name':refnames[token],'signature':row.Signature.value.hex(),'class':c})

full_path=R/'audit/stage9-native3/resolver/mscorlib.dll'
full_bytes=full_path.read_bytes(); full=dnfile.dnPE(data=full_bytes)
full_helpers=[]
for t in full.net.mdtables.TypeDef.rows:
    if (str(t.TypeNamespace),str(t.TypeName)) not in [('System','Object'),('System.Collections.Generic','List`1')]:
        continue
    for mr in t.MethodList:
        if str(mr.row.Name)=='.ctor' and mr.row.Signature.value.hex()=='200001':
            full_helpers.append(cilbody(full,0x06000000|mr.row_index))

out={'scope':'Read-only PE, original metadata and archived CIL source inspection. No assembly runtime, tests, builds or candidates.',
    'source_hashes':{str(p.relative_to(R)):hashlib.sha256(p.read_bytes()).hexdigest() for p in [
        R/'audit/stage9-native3/inputs/GameAssembly.dll',R/'audit/stage9-native3/inputs/global-metadata.dat']},
    'registration':{'address':hex(reg),'types':hex(typeTable),'generic_insts':hex(instTable),'method_specs':hex(specTable)},
    'modules':mods,'object_ctor_mapping':object_mapping,'shared_302170_aliases':aliases,
    'native_bodies':{'AttackRange_ctor':nativebody(0x1803002A0),'shared_List_ctor':nativebody(0x1808053D0),
        'Object_ctor':nativebody(0x180302170,3),'Buff_ctor':nativebody(0x180310BB0),'Hide_ctor':nativebody(0x18031C900)},
    'typed_usage_cells':cells,
    'original_fields':[dict(f,decoded_type=typ(f['type_index'])) for f in fields if f['type'] in ['AttackRange','Buff','Hide']],
    'parent':{'archive':str(archive.relative_to(R)),'member':member,'sha256':hashlib.sha256(parent_bytes).hexdigest(),
        'inventory_sha256':inventory['input_sha256'],'member_refs':current_refs,
        'constructors':[cilbody(parent,t,refnames) for t in [0x060000E0,0x060000F5,0x060000FD]]},
    'full_mscorlib':{'path':str(full_path.relative_to(R)),'size':len(full_bytes),'sha256':hashlib.sha256(full_bytes).hexdigest(),
        'constructors':full_helpers}}
print(json.dumps(out,indent=2,ensure_ascii=False))
