from pathlib import Path
import json,struct,hashlib,collections,sys
def check(a,b,expected):
 ha=struct.unpack_from('<64I',a);hb=struct.unpack_from('<64I',b);assert ha[:2]==hb[:2]==(0xFAB11BAF,31)
 rows=[]
 for j in range(2,31):
  p,l=ha[2+2*j:4+2*j];q,m=hb[2+2*j:4+2*j];assert p+l<=len(a) and q+m<=len(b) and l==m and a[p:p+l]==b[q:q+m],('Non-literal table drift',j)
  rows.append({'header_pair_index':j,'bytes':l,'payload_equal':True,'sha256':hashlib.sha256(a[p:p+l]).hexdigest()})
 def literals(image,header):
  p,l,q,size=header[2:6];assert l%8==0 and p+l<=len(image) and q+size<=len(image);out=[]
  for i in range(l//8):
   length,ix=struct.unpack_from('<II',image,p+8*i);assert ix+length<=size;out.append(image[q+ix:q+ix+length])
  return out
 old=literals(a,ha);new=literals(b,hb);assert collections.Counter(new)<=collections.Counter(old)
 deleted=list((collections.Counter(old)-collections.Counter(new)).elements());assert collections.Counter(deleted)==collections.Counter(expected),[x.decode('utf8')for x in deleted]
 # Preserve the order of every retained literal, including duplicates.
 pending=collections.Counter(expected);keep=[]
 for value in old:
  if pending[value]:pending[value]-=1
  else:keep.append(value)
 assert keep==new,'Retained literal order changed'
 return {'non_literal_tables':rows,'old_literals':len(old),'new_literals':len(new),'removed_literals':[s.decode('utf8')for s in deleted],'retained_literal_order_equal':True,'pass':True}
a=Path(sys.argv[1]).read_bytes();b=Path(sys.argv[2]).read_bytes();expected=[s.encode('utf8')for s in json.loads(Path(sys.argv[3]).read_text())]
r=check(a,b,expected);bad=bytearray(b);pair=struct.unpack_from('<I',b,24)[0];bad[pair]^=1
try:check(a,bad,expected);raise RuntimeError('Corruption accepted')
except AssertionError:pass
r['nonliteral_corruption_control_detected']=True;print(json.dumps(r))
