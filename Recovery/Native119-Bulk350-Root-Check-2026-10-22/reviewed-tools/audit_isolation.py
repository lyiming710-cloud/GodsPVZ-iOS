"""Independent reopened-byte audit for Bulk350; does not use construction assertions as results."""
from pathlib import Path
import sys,json,hashlib,struct,importlib.util,copy
sys.dont_write_bytecode=True
V=Path(__file__).resolve().parent;R=V.parents[1]
sys.path.insert(0,str(R/'audit/stage9-native3/python-deps'))
import dnfile
spec=importlib.util.spec_from_file_location('historical_parsers',R/'audit/native111-publication-2026-10-20/repository/scripts/actions/static_gates.py')
helpers=importlib.util.module_from_spec(spec);spec.loader.exec_module(helpers)
plan=json.loads((V/'frozen-source/IMPLEMENTATION-PLAN.json').read_text('utf-8'))
meta=json.loads((V/'frozen-source/METADATA-ADDITIONS.json').read_text('utf-8'))
targets={int(x['token'],16):x for x in plan['targets']};constants={x['name']:x['token'] for x in meta['constants']}
parent=(R/'audit/native93-codex-2026-10-17/verification-2026-10-17/candidates/candidate1.dll').read_bytes()
assert hashlib.sha256(parent).hexdigest()==plan['parent_sha256']
def require(ok,why):
    if not ok:raise ValueError(why)
def enc(n):
    return bytes([n]) if n<128 else bytes([0x80|(n>>8),n&255]) if n<16384 else bytes([0xC0|(n>>24),(n>>16)&255,(n>>8)&255,n&255])
def run(candidate,settings=meta):
    require(len(candidate)>=len(parent),'truncated image')
    pp=helpers.parse_pe_and_cli(parent);pc=helpers.parse_pe_and_cli(candidate)
    allowed=set()
    for off in [pp['sections'][-1]['header_offset']+8,pp['sections'][-1]['header_offset']+16,pp['opt_off']+8,pp['opt_off']+56,pp['opt_off']+64,pp['cli_off']+8,pp['cli_off']+12]:allowed.update(range(off,off+4))
    require(all(a==b or i in allowed for i,(a,b) in enumerate(zip(parent,candidate))),'unauthorized original-prefix byte modification')
    mp=helpers.parse_metadata_root(parent,pp['meta_off'],pp['meta_size']);mc=helpers.parse_metadata_root(candidate,pc['meta_off'],pc['meta_size'])
    for key in ['heap_sizes','valid_mask','sorted_mask']:require(mp[key]==mc[key],'metadata header '+key)
    require(mp['streams'].keys()==mc['streams'].keys(),'metadata stream set changed')
    for name in mp['streams']:
        if name=='#~':continue
        a=mp['streams'][name]['bytes'];b=mc['streams'][name]['bytes']
        require(b[:len(a)]==a,'original heap prefix changed: '+name)
        if name=='#GUID':require(a==b,'GUID heap changed')
    dp=dnfile.dnPE(data=parent);dc=dnfile.dnPE(data=candidate)
    expected_counts={10:plan['memberref_count_after'],17:plan['standalone_count_after'],27:plan['typespec_count_after'],43:plan['methodspec_count_after']}
    widths=[];rvas=0
    for a,b in zip(dp.net.mdtables.tables_list,dc.net.mdtables.tables_list):
        require(a.number==b.number and a.row_size==b.row_size,'table row layout changed')
        require(b.num_rows==expected_counts.get(a.number,a.num_rows),'table row count changed: '+a.name)
        widths.append(dict(table=a.name,width_before=a.row_size,width_after=b.row_size))
        for i,ar in enumerate(a.rows):
            br=b.rows[i];ap=ar.struct.get_file_offset();bp=br.struct.get_file_offset();ab=parent[ap:ap+a.row_size];bb=candidate[bp:bp+b.row_size]
            if a.number==6 and 0x06000001+i in targets:
                require(ab[4:]==bb[4:],'target MethodDef identity changed');require(ab[:4]!=bb[:4],'target RVA not relocated');rvas+=1
            else:require(ab==bb,'old table row modified: '+a.name+':'+str(i+1))
    for i,x in enumerate(settings['member_definitions'],1101):
        row=dc.net.mdtables.MemberRef.rows[i-1]
        require(x['token']==0x0A000000+i,'member declaration RID')
        require(struct.unpack_from('<H',candidate,row.struct.get_file_offset())[0]==x['class_code'],'appended MemberRef declaring owner')
        sig=next((y['signature'] for y in settings['emitted_field_signatures'] if y['token']==x['token']),x['signature'])
        require(str(row.Name)==(x['new_name'] or x['template_name']) and row.Signature.value.hex()==sig,'appended MemberRef name/signature')
    for i,x in enumerate(settings['local_blobs'],1766):require(dc.net.mdtables.StandAloneSig.rows[i-1].Signature.value.hex()==x,'appended local signature')
    for i,x in enumerate(settings['type_blobs'],174):require(dc.net.mdtables.TypeSpec.rows[i-1].Signature.value.hex()==x,'appended TypeSpec')
    ms=dc.net.mdtables.MethodSpec.rows
    require(ms[185].Method.row_index==126 and ms[185].Method.table.name=='MemberRef' and ms[185].Instantiation.value.hex()=='0a011180dc','new Array.IndexOf instantiation')
    require(ms[186].Method.row_index==484 and ms[186].Method.table.name=='MemberRef' and ms[186].Instantiation.value.hex()=='0a01128210','new Zombie circle instantiation')
    for i,x in enumerate(settings['added_methodspecs'],188):
        row=ms[i-1];require(row.Method.table.name=='MethodDef' and row.Method.row_index==(x['definition']&0xFFFFFF) and row.Instantiation.value.hex()==x['instantiation'],'appended game MethodSpec binding')
    # Derive expected appended payload, including UV array element signatures,
    # rather than trust the stale aggregate payload-length counters in old plans.
    blobs=[bytes.fromhex(x) for x in settings['local_blobs']]+[bytes.fromhex(x['signature']) for x in settings['emitted_field_signatures']]+[bytes.fromhex(x) for x in settings['type_blobs']]+[bytes.fromhex('0a011180dc'),bytes.fromhex('0a01128210')]+[bytes.fromhex(x['instantiation']) for x in settings['added_methodspecs']]
    suffix=b''.join(enc(len(x))+x for x in blobs)
    require(mc['streams']['#Blob']['bytes']==mp['streams']['#Blob']['bytes']+suffix,'exact appended #Blob payload')
    require(len(mc['streams']['#Blob']['bytes'])<65536,'short #Blob heap bound')
    names=b''.join(x['new_name'].encode()+b'\0' for x in settings['member_definitions'] if x['new_name'])
    require(mc['streams']['#Strings']['bytes']==mp['streams']['#Strings']['bytes']+names,'exact appended #Strings payload')
    us=mp['streams']['#US']['bytes'];entries=[]
    for x in settings['user_strings']:
        require(x['token']==0x70000000+len(us),'#US token offset')
        text=x['value'];payload=text.encode('utf-16-le');flag=int(any(ord(c)>255 or 1<=ord(c)<=8 or 14<=ord(c)<=31 or ord(c) in (0x27,0x2D,0x7F) for c in text));encoded=enc(len(payload)+1)+payload+bytes([flag])
        require(bytes.fromhex(x['encoded_bytes'])==encoded,'independent UTF16/terminal encoding')
        us+=encoded;entries.append(x['token'])
    require(mc['streams']['#US']['bytes']==us,'exact #US literal bytes')
    unchanged=0;zero_rva=0;eh=0;records=[]
    for i,(a,b) in enumerate(zip(dp.net.mdtables.MethodDef.rows,dc.net.mdtables.MethodDef.rows),1):
        tok=0x06000000+i
        if not a.Rva:
            require(not b.Rva,'unexpected body on abstract/external method');require(tok not in targets,'target has no body');unchanged+=1;zero_rva+=1;continue
        ao=dp.get_offset_from_rva(a.Rva);bo=dc.get_offset_from_rva(b.Rva);ab=helpers.parse_raw_method_body_span(parent,ao);bb=helpers.parse_raw_method_body_span(candidate,bo)
        if tok not in targets:
            require(parent[ao:ao+ab[0]]==candidate[bo:bo+bb[0]],'non-target raw body/header/EH changed');unchanged+=1;continue
        t=targets[tok];expected=t.get('local_constant');expected=constants[expected] if expected else int(t['new_local_sig_token'],16) if t.get('new_local_sig_token') else ab[4]
        require(bb[4]==expected,f'target {tok:#x} local token changed: {bb[4]:#x} vs {expected:#x}; {t}')
        require(bo>=len(parent) and bo%4==0,'target body append/alignment')
        require(struct.unpack_from('<H',candidate,bo)[0]&0x10,'target InitLocals removed')
        require(parent[ao:ao+ab[0]]==candidate[ao:ao+ab[0]],'old target slot modified')
        code=candidate[bo+bb[1]:bo+bb[1]+bb[2]]
        require(code!=parent[ao+ab[1]:ao+ab[1]+ab[2]],'target did not change')
        for clause in bb[8]:
            _,_,flags,ts,tl,hs,hl,ct=clause
            require(flags==2 and ct==0 and tl>0 and hl>0 and ts+tl==hs and hs+hl<=len(code) and code[hs+hl-1]==0xDC,'finally region extent/kind/endfinally')
        eh+=bb[7];records.append(dict(token=hex(tok),raw_body_sha256=hashlib.sha256(candidate[bo:bo+bb[0]]).hexdigest(),finally_regions=bb[7]))
    require(len(records)==350 and unchanged==1967 and eh==158,f'body scope or total EH count: targets={len(records)} unchanged={unchanged} EH={eh}')
    return dict(status='PASS_SCOPED_REOPENED_BYTE_AND_METADATA_ISOLATION',target_bodies=350,unchanged_non_target_methods=unchanged,unchanged_non_target_bodies=unchanged-zero_rva,unchanged_non_target_zero_rva=zero_rva,finally_regions=eh,appended_memberrefs=180,appended_local_signatures=293,appended_typespecs=5,appended_methodspecs=8,user_strings=len(entries),blob_bytes=len(mc['streams']['#Blob']['bytes']),blob_headroom_to_65536=65536-len(mc['streams']['#Blob']['bytes']),table_widths=widths,methods=records)
if __name__=='__main__':
    rev=sys.argv[1];b=(V/(rev+'-candidate.dll')).read_bytes();report=run(b);report['candidate_sha256']=hashlib.sha256(b).hexdigest();controls=[]
    # Negative controls perturb actual candidate bytes or independently pinned expectations.
    dn=dnfile.dnPE(data=b);non=next(x for i,x in enumerate(dn.net.mdtables.MethodDef.rows,1) if x.Rva and 0x06000000+i not in targets)
    cases=[('old_non_target_byte',dn.get_offset_from_rva(non.Rva)+1),('new_member_owner',dn.net.mdtables.MemberRef.rows[1100].struct.get_file_offset()),('new_local_signature_index',dn.net.mdtables.StandAloneSig.rows[1765].struct.get_file_offset()),('new_typespec_index',dn.net.mdtables.TypeSpec.rows[173].struct.get_file_offset()),('new_methodspec_binding',dn.net.mdtables.MethodSpec.rows[185].struct.get_file_offset()),('target_initlocals',dn.get_offset_from_rva(dn.net.mdtables.MethodDef.rows[(next(iter(targets))&0xFFFFFF)-1].Rva))]
    # Explicit first target indexing avoids masking precedence mistakes.
    cases[-1]=('target_initlocals',dn.get_offset_from_rva(dn.net.mdtables.MethodDef.rows[(next(iter(targets))&0xFFFFFF)-1].Rva))
    for name,at in cases:
        mutant=bytearray(b);mutant[at]^=0x10 if name=='target_initlocals' else 1
        try:run(bytes(mutant));rejected=False;why='NOT_REJECTED'
        except Exception as e:rejected=True;why=str(e)
        controls.append(dict(name=name,rejected=rejected,reason=why))
    for name in ['wrong_literal_text','wrong_literal_flag','wrong_literal_offset','wrong_local_blob']:
        bad=copy.deepcopy(meta)
        if name=='wrong_literal_text':bad['user_strings'][-1]['value']+='X'
        elif name=='wrong_literal_flag':bad['user_strings'][-1]['encoded_bytes']=bad['user_strings'][-1]['encoded_bytes'][:-2]+'ff'
        elif name=='wrong_literal_offset':bad['user_strings'][-1]['token']+=1
        else:bad['local_blobs'][-1]+='08'
        try:run(b,bad);rejected=False;why='NOT_REJECTED'
        except Exception as e:rejected=True;why=str(e)
        controls.append(dict(name=name,rejected=rejected,reason=why))
    report['negative_controls']=controls;report['negative_controls_all_rejected']=all(x['rejected'] for x in controls)
    (V/(rev+'-ISOLATION.json')).write_text(json.dumps(report,indent=2)+'\n','utf-8')
    print(json.dumps({k:v for k,v in report.items() if k not in ['table_widths','methods','negative_controls']}));print(controls)
    sys.exit(not report['negative_controls_all_rejected'])
