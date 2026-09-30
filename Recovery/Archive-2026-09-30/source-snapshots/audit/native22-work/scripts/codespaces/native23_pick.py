import json, collections, re, os
ROOT='/workspaces/GodsPVZ-native19/.validation/native22'
bm=json.load(open(ROOT+'/out/body-map.json'))

def fam(m):
    if 'comparison between pointer and integer' in m: return 'A-ptr-vs-int'
    if 'assigning to' in m: return 'B-assign-type'
    if 'no viable overloaded' in m: return 'B-assign-type'
    if 'no matching function for call to' in m: return 'C-call-signature'
    if 'invalid operands to binary expression' in m: return 'D-invalid-operands'
    if 'C-style cast' in m: return 'E-cast-not-allowed'
    if 'cast from pointer to smaller type' in m: return 'F-cast-ptr-smaller'
    if 'member reference' in m or 'no member named' in m: return 'G-member-access'
    return 'H-other'

CORE=('Plant','Zombie','Board','Device','Projectile','MouseManager','Grid','Map','Bullet','Card','Sun','LawnMower','Coin','GridManager','PlantCard','SunFlower','Peashooter')
rows=[]
for tok,v in bm.items():
    if not v['errors']: continue
    sym=v['symbol']
    m=re.match(r'^([A-Za-z0-9_]+?)(_m[0-9A-F]{8,})?$', sym)
    cls=sym.split('_')[0]
    if cls not in CORE: continue
    fs=collections.Counter(fam(e['msg']) for e in v['errors'])
    pure = set(fs) <= {'A-ptr-vs-int','B-assign-type'}
    rows.append((len(v['errors']), tok, sym[:60], v['file'], pure, ','.join(sorted(fs))))
rows.sort()
print('core-class methods with errors: %d' % len(rows))
print()
print('--- PURE A/B only, ascending size (first 25) ---')
n=0
for cnt,tok,sym,f,pure,fams in rows:
    if not pure: continue
    n+=1
    if n<=25: print('  %-8s n=%-3d %-14s %-58s %s' % (tok,cnt,f,sym,fams))
print('  ... total pure in core classes: %d' % sum(1 for r in rows if r[4]))
print()
print('--- STRUCTURAL (cannot be repaired mechanically) in core classes (first 25) ---')
n=0
for cnt,tok,sym,f,pure,fams in rows:
    if pure: continue
    n+=1
    if n<=25: print('  %-8s n=%-3d %-14s %-58s %s' % (tok,cnt,f,sym,fams))
print('  ... total structural in core classes: %d' % sum(1 for r in rows if not r[4]))
print()
print('=== overall: file distribution of the 608 ===')
c=collections.Counter(v['file'] for v in bm.values() if v['errors'])
for k,v in c.most_common(): print('   %4d %s' % (v,k))
print()
print('=== existing proposals coverage ===')
p=ROOT+'/out/native22-proposals.json'
if os.path.exists(p):
    pr=json.load(open(p))
    if isinstance(pr,dict):
        print('type dict, keys:', list(pr)[:8])
        toks=set(pr.get('tokens',[]) or list(pr))
    else:
        print('type list, len', len(pr)); toks=set()
        for x in pr:
            t=x.get('token') or x.get('tokens')
            if isinstance(t,str): toks.add(t.upper())
    err=set(t.upper() for t,v in bm.items() if v['errors'])
    print('proposal tokens: %d   overlap with the 608: %d' % (len(toks), len(toks & err)))
