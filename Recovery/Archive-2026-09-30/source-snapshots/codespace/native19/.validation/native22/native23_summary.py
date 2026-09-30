import json, collections, os
ROOT='/workspaces/GodsPVZ-native19/.validation/native22'
bm=json.load(open(ROOT+'/out/body-map.json'))

def fam(m):
    if 'comparison between pointer and integer' in m: return 'A-ptr-vs-int'
    if 'assigning to' in m: return 'B-assign-type'
    if 'no matching function for call to' in m: return 'C-call-signature'
    if 'invalid operands to binary expression' in m: return 'D-invalid-operands'
    if 'C-style cast' in m: return 'E-cast-not-allowed'
    if 'no viable overloaded' in m: return 'B-assign-type'
    if 'cast from pointer to smaller type' in m: return 'F-cast-ptr-smaller'
    if 'member reference' in m or 'no member named' in m: return 'G-member-access'
    return 'H-other'

tot_lines=0
bytok={}
for tok,v in bm.items():
    if not v['errors']: continue
    fs=[fam(e['msg']) for e in v['errors']]
    tot_lines+=len(v['errors'])
    bytok[tok]=(v,collections.Counter(fs))

print('tokens with errors: %d   total error lines inside those bodies: %d' % (len(bytok), tot_lines))
print()
print('=== purity classification of the 608 ===')
pure=0; mixed=0; structural=set()
for tok,(v,c) in bytok.items():
    keys=set(c)
    if keys <= {'A-ptr-vs-int','B-assign-type','E-cast-not-allowed','F-cast-ptr-smaller'}:
        pure+=1
    else:
        mixed+=1
        structural.add(tok)
print('pure-encoding families only : %d' % pure)
print('contains structural families: %d' % mixed)
print()
print('=== family counts among structural ones ===')
c2=collections.Counter()
for tok in structural:
    for f,c in bytok[tok][1].items(): c2[f]+=c
for k,v in c2.most_common(12): print('  %6d %s' % (v,k))
print()
print('=== top 25 methods by error count (symbol, file, count, families) ===')
rank=sorted(bytok.items(), key=lambda kv:-len(kv[1][0]['errors']))[:25]
for tok,(v,c) in rank:
    print('  %-8s %-70s %-22s n=%-3d %s' % (tok, v['symbol'][:70], v['file'], len(v['errors']), ','.join(sorted(set(fam(e['msg']) for e in v['errors'])))))
print()
print('=== how many error lines are OUTSIDE any mapped body? ===')
import re
LOGDIR='/workspaces/GodsPVZ-native19/.validation/native22-full2'
ERR_RE=re.compile(r'^([^:]+\.(?:cpp|c)):(\d+):(\d+): error: (.*)$')
inside=0; outside=0; otherfile=collections.Counter()
bodies=collections.defaultdict(list)
for tok,v in bm.items(): bodies[v['file']].append((v['start'],v['end']))
for lg in sorted(os.listdir(LOGDIR)):
    if not lg.endswith('.log'): continue
    base=lg[:-4]
    rs=bodies.get(base,[])
    for line in open(os.path.join(LOGDIR,lg),errors='replace'):
        m=ERR_RE.match(line)
        if not m: continue
        if os.path.basename(m.group(1))!=base: continue
        ln=int(m.group(2))
        if any(s<=ln<=e for (s,e) in rs): inside+=1
        else:
            outside+=1; otherfile[base]+=1
print('inside mapped body: %d   outside: %d' % (inside, outside))
print('top files with unmapped errors:')
for k,v in otherfile.most_common(12): print('   %6d %s' % (v,k))
