"""Read native/metadata and archived CIL as source data; never execute targets."""
from pathlib import Path
import sys, json, hashlib, struct, re, zipfile
sys.dont_write_bytecode = True
R = Path(__file__).resolve().parents[2]
prefix=(R/'audit/native80-spec-2026-10-13/inspect_original.py').read_text().split('cells=[]',1)[0]
exec(prefix)
import dnfile
from dncil.cil.body import CilMethodBody
from dncil.cil.body.reader import CilMethodBodyReaderBytes
from capstone import CS_OP_MEM
ds=Cs(CS_ARCH_X86,CS_MODE_64);ds.detail=True
def body(va):
    e=next(e.struct for e in pe.DIRECTORY_ENTRY_EXCEPTION if e.struct.BeginAddress+base==va)
    end=e.EndAddress+base; data=native(va,end-va); uw=e.UnwindData+base
    out={'start':hex(va),'end':hex(end),'size':len(data),'sha256':hashlib.sha256(data).hexdigest(),
        'unwind':hex(uw),'unwind_flags':native(uw,1)[0]>>3,'bytes':data.hex(),'instructions':[]}
    for i in ds.disasm(data,va):
        row={'address':hex(i.address),'bytes':i.bytes.hex(),'mnemonic':i.mnemonic,'operands':i.op_str}
        for op in i.operands:
            if op.type==CS_OP_MEM and i.reg_name(op.mem.base)=='rip':row['rip_target']=hex(i.address+i.size+op.mem.disp)
        out['instructions'].append(row)
    return out
def leafbody(va,size):
    data=native(va,size)
    return {'start':hex(va),'end':hex(va+size),'size':size,'sha256':hashlib.sha256(data).hexdigest(),
        'bytes':data.hex(),'unwind_flags':None,'instructions':[{'address':hex(i.address),'bytes':i.bytes.hex(),
            'mnemonic':i.mnemonic,'operands':i.op_str}for i in ds.disasm(data,va)]}
def cell(va):
    v=nq(va);k=(v>>29)&7;idx=(v>>1)&0xfffffff
    out={'cell':hex(va),'bytes':native(va,8).hex(),'encoded':hex(v),'kind':k,'index':idx}
    if k==6:out['method_spec']=ms(idx)
    elif k in (1,2):out['type']=typ(idx)
    elif k==5:
        count,start=struct.unpack_from('<II',b,u(b,8)+idx*8)
        data=b[u(b,16)+start:u(b,16)+start+count]
        out['literal']={'count':count,'bytes':data.hex(),'text':data.decode('utf-8')}
    return out

target_bodies={hex(v):body(v) for v in [0x180381670,0x180382D80,0x1804C4AB0,0x18046DC80,0x1804A25F0,0x180322500]}
target_bodies['0x18034de90']=leafbody(0x18034DE90,8)
callvas={int(x['operands'],16) for bod in target_bodies.values() for x in bod['instructions']
    if x['mnemonic']=='call' and x['operands'].startswith('0x')}
module_tables={'Assembly-CSharp.dll':0x181B82D60,'UnityEngine.CoreModule.dll':0x181CC06B0}
method_records=[]
for rec in maps:
    if int(rec['va'],16) not in callvas and not(rec['image']=='Assembly-CSharp.dll'and int(rec['token'],16)in [0x06000517,0x06000510,0x060003B9,0x060001E5]):continue
    for t in types:
        if t['name']!=rec['type']:continue
        for j in range(t['methods']):
            m=method(t['methodStart']+j)
            if m['token']==rec['token']:
                rid=int(m['token'],16)&0xffffff;ptrcell=module_tables[rec['image']]+8*(rid-1)
                method_records.append(dict(m,map=rec,independent_pointer_cell=hex(ptrcell),independent_pointer=hex(nq(ptrcell))))

gp_table=0x181760110
gp_references=[]
for match in re.finditer(re.escape(struct.pack('<Q',gp_table)),raw):
    v=pe.get_rva_from_offset(match.start())+base
    gp_references.append({'pointer_field':hex(v),'count_field':hex(v-8),'count':nq(v-8),'table':hex(nq(v))})
canonical=[]
for j in range(nq(reg+32)):
    p=nq(reg+40)+16*j;si,mi,iv,ad=struct.unpack('<iiii',native(p,16))
    md,ci,gi=struct.unpack('<iii',native(specTable+12*si,12))
    if md in [41961,36969,42770]:
        canonical.append({'row':j,'record':hex(p),'bytes':native(p,16).hex(),'method_spec':ms(si),
            'method_pointer_index':mi,'pointer_cell':hex(gp_table+8*mi),'pointer':hex(nq(gp_table+8*mi)),
            'invoker_index':iv,'adjustor_index':ad})

archive=R/'audit/native82-codex-2026-10-13/godspvz-native78-82-pending-2026-10-13.zip'
member='candidates/native82/candidate1.dll'
with zipfile.ZipFile(archive)as z:parentbytes=z.read(member)
parent=dnfile.dnPE(data=parentbytes);T=parent.net.mdtables
inventory=json.loads((R/'audit/native83-codex-2026-10-13/metadata-current.json').read_text())
refnames={int(x['token'],16):x['name'] for x in inventory['refs']}
def cil(assembly,token,names=None):
    tables=assembly.net.mdtables
    owners={mr.row_index:t for t in tables.TypeDef.rows for mr in t.MethodList}
    fowners={fr.row_index:t for t in tables.TypeDef.rows for fr in t.FieldList}
    def fullname(t):return (str(t.TypeNamespace)+'.' if str(t.TypeNamespace)else '')+str(t.TypeName)
    def identity(tok):
        if names and tok in names:return names[tok]
        tab,ri=tok>>24,tok&0xffffff
        if tab==6:return fullname(owners[ri])+'::'+str(tables.MethodDef.rows[ri-1].Name)
        if tab==4:return fullname(fowners[ri])+'::'+str(tables.Field.rows[ri-1].Name)
        if tab==10:return str(tables.MemberRef.rows[ri-1].Name)+' / sig '+tables.MemberRef.rows[ri-1].Signature.value.hex()
        if tab==0x2B:
            row=tables.MethodSpec.rows[ri-1]
            base=(6 if row.Method.table.name=='MethodDef'else 10)<<24|row.Method.row_index
            return identity(base)+' / inst '+row.Instantiation.value.hex()
        if tab==0x70:return str(assembly.net.user_strings.get(ri).value)
        return hex(tok)
    m=tables.MethodDef.rows[(token&0xffffff)-1]
    bb=CilMethodBody(CilMethodBodyReaderBytes(assembly.get_data(m.Rva,8192)))
    out={'token':hex(token),'name':identity(token),'signature':m.Signature.value.hex(),'rva':hex(m.Rva),
        'public':bool(m.Flags.mdPublic),'static':bool(m.Flags.mdStatic),'code_size':bb.code_size,
        'max_stack':bb.max_stack,'local_sig':str(bb.local_var_sig_tok),'eh_count':len(bb.exception_handlers),
        'body_bytes':assembly.get_data(m.Rva,bb.header_size+bb.code_size).hex(),'instructions':[]}
    if bb.local_var_sig_tok:out['local_sig_bytes']=tables.StandAloneSig.rows[(bb.local_var_sig_tok.value&0xffffff)-1].Signature.value.hex()
    for i in bb.instructions:
        operand=i.operand;v=None if operand is None else str(operand);op=str(i.opcode)
        if hasattr(operand,'value'):v={'token':hex(operand.value),'identity':identity(operand.value)}
        elif isinstance(operand,int)and(op.startswith('br')or op.startswith('leave')):v=operand-bb.header_size
        out['instructions'].append({'offset':i.offset-bb.header_size,'opcode':op,'operand':v})
    return out
specs=[]
for rid,row in enumerate(T.MethodSpec.rows,1):
    if (row.Method.table.name=='MethodDef' and row.Method.row_index in [0xC4,0x3ED])or(row.Method.table.name=='MemberRef'and row.Method.row_index==0x79):
        specs.append({'token':hex(0x2B000000|rid),'method_table':row.Method.table.name,'method_rid':row.Method.row_index,
            'instantiation':row.Instantiation.value.hex(),'blob_index':row.struct.Instantiation_BlobIndex})
generic_params=[]
for rid,row in enumerate(T.GenericParam.rows,1):
    if row.Owner.table.name=='MethodDef'and row.Owner.row_index==0xC4:
        generic_params.append({'token':hex(0x2A000000|rid),'name':str(row.Name),'number':row.Number,'flags':str(row.Flags),
            'constraints':[str(c.Constraint) for c in T.GenericParamConstraint.rows if c.Owner.row_index==rid]})
fullpath=R/'audit/stage9-native3/resolver/UnityEngine.CoreModule.dll';core=dnfile.dnPE(str(fullpath));coremethods=[]
for t in core.net.mdtables.TypeDef.rows:
    if str(t.TypeName)=='Vector3':
        for mr in t.MethodList:
            if str(mr.row.Name)=='get_zero' or(str(mr.row.Name)=='.ctor'and mr.row.Signature.value.hex()=='2003010c0c0c'):
                coremethods.append(cil(core,0x06000000|mr.row_index))
needfields={'Plant':['blockable','board','skill','animationSprites','camp','particleSystems'],
    'Board':['projectileManager'],'ParticlesManager':['<instance>k__BackingField'],
    'Skill':['value_float'],'Damage':['damagePoint','penetrationVigour'],'Element':['point'],
    'Projectile':['camp','damage'],'Vector3':['zeroVector','x','y','z'],'VFXAnimationEvent':None}
selectedfields=[dict(f,type=typ(f['type_index']),declaring_type=f['type'])for f in fields
    if f['type']in needfields and(needfields[f['type']]is None or f['name']in needfields[f['type']])]
currentfields=[]
for ri,t in enumerate(T.TypeDef.rows,1):
    for fr in t.FieldList:
        if str(t.TypeName) in needfields and(needfields[str(t.TypeName)]is None or str(fr.row.Name)in needfields[str(t.TypeName)]):
            currentfields.append({'token':hex(0x04000000|fr.row_index),'owner':str(t.TypeName),'name':str(fr.row.Name),
                'signature':fr.row.Signature.value.hex(),'public':bool(fr.row.Flags.fdPublic),'private':bool(fr.row.Flags.fdPrivate),'static':bool(fr.row.Flags.fdStatic)})
refs=[dict(x,signature=T.MemberRef.rows[(int(x['token'],16)&0xffffff)-1].Signature.value.hex())for x in inventory['refs']
    if any(s in x['name']for s in ['Vector3::.ctor','get_zero','GetComponent','get_transform','get_position','set_position'])]
out={'scope':'Only native PE/metadata and managed CIL source parsing; no tests/runtime/build/candidate.',
    'source_hashes':{str(p.relative_to(R)):hashlib.sha256(p.read_bytes()).hexdigest()for p in [R/'audit/stage9-native3/inputs/GameAssembly.dll',R/'audit/stage9-native3/inputs/global-metadata.dat']},
    'registration':{'address':hex(reg),'types':hex(typeTable),'generic_inst':hex(instTable),'method_specs':hex(specTable),
        'generic_method_functions_count':nq(reg+32),'generic_method_functions_table':hex(nq(reg+40))},
    'generic_pointer_table_references':gp_references,'generic_canonical_rows':canonical,'native_bodies':target_bodies,
    'usage_cells':[cell(v)for v in [0x181BA5578,0x181B9E278,0x181BAA518,0x181BB1890,0x181BA2018,0x181BBB7C8,0x181BA9DE0,0x181BC27E8]],
    'methods':method_records,'fields':selectedfields,'constants':[{'va':hex(v),'bytes':native(v,4).hex(),'bits':hex(u(native(v,4))),'float32':struct.unpack('<f',native(v,4))[0]}for v in [0x1815A7E54,0x1815A7C68]],
    'parent':{'archive':str(archive.relative_to(R)),'member':member,'sha256':hashlib.sha256(parentbytes).hexdigest(),
        'MethodSpec_row_count':len(T.MethodSpec.rows),
        'methods':[cil(parent,t,refnames)for t in [0x06000517,0x06000510,0x060000C4,0x060001E5,0x060003B9]],'selected_specs':specs,
        'Binding_generic_params':generic_params,'selected_fields':currentfields,'selected_member_refs':refs},
    'full_core':{'path':str(fullpath.relative_to(R)),'sha256':hashlib.sha256(fullpath.read_bytes()).hexdigest(),
        'size':fullpath.stat().st_size,'getters':coremethods}}
print(json.dumps(out,ensure_ascii=False,indent=2))
