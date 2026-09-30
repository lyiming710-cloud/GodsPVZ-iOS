"""Native22 typed-repair engine (read-only proposals).

Every rule is a minimal encoding repair. A method is eligible only when its
WHOLE reachable body verifies after repair, and every single change is proven
necessary by reverting it alone and re-observing failure. Nothing is written to
any DLL here.
"""
import json,re,copy,hashlib,collections
from verify_native19_types import verify,InvalidIL

INTS={'System.Boolean','System.Char','System.Byte','System.SByte','System.Int16','System.UInt16','System.Int32','System.UInt32'}
def norm(t,types):
    if t in INTS or types.get(t,{}).get('enumType'):return 'I4'
    if t in ('System.Int64','System.UInt64'):return 'I8'
    if t in ('System.IntPtr','System.UIntPtr'):return 'I'
    if t in ('System.Single','System.Double'):return 'F'
    return t
def is_ref(t,types):
    return t=='null' or t.endswith('[]') or (t in types and not types[t].get('value') and not types[t].get('byref') and not types[t].get('unbound'))

def loc_index(op,o):
    if op.count('.')>=1 and op.split('.')[-1].isdigit():return int(op.split('.')[-1])
    return o['index'] if isinstance(o,dict) else None

def zero_const(x):
    """(kind,value) if x is an integer zero load."""
    op=x['opcode']
    if op=='ldc.i4.0':return ('i4',0)
    if op in ('ldc.i4.s','ldc.i4') and x['operand']==0:return ('i4',0)
    if op=='ldc.i8' and x['operand']==0:return ('i8',0)
    return None

def decide(issue,i,ins,d,types):
    """Return (index_to_patch, new_opcode, new_operand, reason) or None."""
    if i is None or i==0:return None
    x=ins[i];prior=ins[i-1]
    m=re.search(r' IL_([0-9A-F]+): ',issue)
    body=issue.split(' IL_',1)[1].split(': ',1)[1] if ' IL_' in issue else issue

    # R2/R3 comparison against an integer zero
    t=re.match(r'^invalid comparison (.+), (I4|I8)$',body)
    if t and x['opcode'] in ('ceq','cgt.un','cgt','clt','clt.un'):
        other=t.group(1)
        z=zero_const(prior)
        if z and x['opcode'] in ('ceq','cgt.un'):
            if other=='F':return (i-1,'ldc.r4',0.0,'float zero -> ldc.r4 0')
            n=norm(other,types)
            if is_ref(n,types):return (i-1,'ldnull',None,'reference null -> ldnull')
    t=re.match(r'^invalid comparison (I4|I8), (F)$',body)
    if t and zero_const(prior) is None:return None

    # R1 local retype from integer store
    if x['opcode'].startswith('stloc'):
        mt=re.match(r'^(I4|I8) is not assignable to System\.Object$',body)
        if mt:
            n=loc_index(x['opcode'],x['operand'])
            if n is not None and d['locals'][n]=='System.Object':
                new='System.Int32' if mt.group(1)=='I4' else 'System.Int64'
                return ('LOCAL',n,new,'integer local declared System.Object')

    # R6 managed pointer passed where the value is required
    t=re.match(r'^receiver (.+)& incompatible with (.+) \(write=(True|False), call=(True|False)\)$',body)
    if t and prior['opcode'].startswith(('ldloca','ldflda')):
        return (i-1,'ldloc' if prior['opcode'].startswith('ldloca') else 'ldfld',prior['operand'],
                'value required; address supplied')

    # R5 numeric operand width mismatch caused by a constant
    t=re.match(r'^invalid numeric operands (.+), (.+)$',body)
    if t and zero_const(prior):
        c,b=t.group(1),t.group(2)
        if {'I4','I8'}=={norm(c,types),norm(b,types)}:
            if prior['opcode']=='ldc.i4.0':return (i-1,'ldc.i8',0,' widen zero to i8')
    return None

out=[]
eligible=[];blocked=[]
data=json.load(open('out/all-methods.json'))
types=data['types'];methods=data['methods']
for pos,original in enumerate(methods):
    if pos%200==0:print('...',pos,flush=True)
    d=copy.deepcopy(original)
    if any(x['opcode']=='ldstr' and isinstance(x['operand'],str) and x['operand'].startswith(
        ('Unmanaged memory load','Indirect jump','Method not found','Not implemented instruction')) for x in d['instructions']):
        blocked.append({'token':d['token'],'method':d['name'],'reason':'native reconstruction hint present'});continue
    changes=[];locals_changed=[];residual=None
    for attempt in range(120):
        try:
            checked=verify(d,types);residual=None;break
        except InvalidIL as e:
            issue=str(e);residual=issue
            mm=re.search(r' IL_([0-9A-F]+):',issue)
            if not mm:break
            off=int(mm.group(1),16)
            idx=next((n for n,z in enumerate(d['instructions']) if z['offset']==off),None)
            act=decide(issue,idx,d['instructions'],d,types)
            if act is None:break
            if act[0]=='LOCAL':
                _,n,new,reason=act
                if d['locals'][n]!=new:
                    locals_changed.append({'index':n,'before':d['locals'][n],'after':new,'reason':reason})
                    d['locals'][n]=new
                else:break
            else:
                j,no,noval,reason=act
                old={'opcode':d['instructions'][j]['opcode'],'operand':copy.deepcopy(d['instructions'][j]['operand'])}
                if j in [c[0] for c in changes]:break
                changes.append((j,copy.deepcopy(old),{'opcode':no,'operand':noval},reason))
                d['instructions'][j]['opcode']=no;d['instructions'][j]['operand']=noval
        except Exception as e:
            residual='analysis error '+type(e).__name__+str(e)[:80];break
    if residual is not None or (not changes and not locals_changed):
        if residual is not None:blocked.append({'token':d['token'],'method':d['name'],'reason':residual})
        continue
    # negative controls: each change must be individually necessary
    bad=False
    for ed in locals_changed:
        probe=copy.deepcopy(d);probe['locals'][ed['index']]=ed['before']
        try:verify(probe,types);bad=True;break
        except InvalidIL:pass
    if bad:blocked.append({'token':d['token'],'method':d['name'],'reason':'local negative control accepted'});continue
    for j,old,new,reason in changes:
        probe=copy.deepcopy(d);probe['instructions'][j]['opcode']=old['opcode'];probe['instructions'][j]['operand']=old['operand']
        try:verify(probe,types);bad=True;break
        except InvalidIL:pass
    if bad:blocked.append({'token':d['token'],'method':d['name'],'reason':'instruction negative control accepted'});continue
    eligible.append({'token':d['token'],'method':d['name'],
        'locals':[{k:v for k,v in e.items() if k!='reason'} for e in locals_changed],
        'local_reasons':[e.get('reason') for e in locals_changed],
        'changes':[{'index':j,'before':old,'after':new,'reason':reason} for j,old,new,reason in changes]})
res={'status':'READ_ONLY_PROPOSALS','source_sha256':hashlib.sha256(open('out/all-methods.json','rb').read()).hexdigest(),
 'eligible_methods':len(eligible),'locals':sum(len(m['locals']) for m in eligible),
 'instruction_edits':sum(len(m['changes']) for m in eligible),'eligible':eligible,'blocked':blocked}
json.dump(res,open('out/native22-proposals.json','w'),indent=2)
print(json.dumps({k:v for k,v in res.items() if k not in ('eligible','blocked')},indent=2))
c=collections.Counter()
for b in blocked:
    r=re.sub(r'IL_[0-9A-F]+','IL_',b['reason']);r=r.split(' IL_',1)[1] if ' IL_' in r else r
    r=re.sub(r'\b[A-Za-z_][\w.`<>]*\b','T',r)
    c[r[:80]]+=1
print('blocked total:',len(blocked))
for k,v in c.most_common(15):print(f'{v:5d}  {k}')
