#!/usr/bin/env python3
"""Map GodsPVZ PC IL2CPP native method pointers to ORIGINAL MethodDefs.

Uses original global-metadata.dat token RIDs plus Assembly-CSharp.dll's
Il2CppCodeGenModule in GameAssembly.dll. Do not substitute Cpp2IL-rewritten RIDs.
"""
from __future__ import annotations
import argparse, struct, sys
from pathlib import Path

MAGIC=0xFAB11BAF
PAIR_BASE=8
STRINGS=2
METHODS=5
TYPEDEFS=19
IMAGES=20
METHOD_SIZE=36
TYPE_SIZE=88
IMAGE_SIZE=40

u16=lambda b,o: struct.unpack_from("<H",b,o)[0]
u32=lambda b,o: struct.unpack_from("<I",b,o)[0]
i32=lambda b,o: struct.unpack_from("<i",b,o)[0]
u64=lambda b,o: struct.unpack_from("<Q",b,o)[0]

def pair(b,n):
    o=PAIR_BASE+n*8
    return u32(b,o),u32(b,o+4)

def cstr(b,o):
    e=b.find(b"\0",o)
    if e<0: raise ValueError(f"unterminated string @0x{o:X}")
    return b[o:e].decode("utf-8","replace")

def metadata_methods(meta,module):
    if u32(meta,0)!=MAGIC: raise ValueError("bad metadata magic")
    if u32(meta,4)!=31: raise ValueError(f"expected metadata v31, got v{u32(meta,4)}")
    so,_=pair(meta,STRINGS); mo,mc=pair(meta,METHODS); to,tc=pair(meta,TYPEDEFS); io,ic=pair(meta,IMAGES)
    if mc%METHOD_SIZE or tc%TYPE_SIZE or ic%IMAGE_SIZE: raise ValueError("unexpected v31 table sizes")
    first_type=type_count=None
    for off in range(io,io+ic,IMAGE_SIZE):
        if cstr(meta,so+i32(meta,off))==module:
            first_type=u32(meta,off+8); type_count=u32(meta,off+12); break
    if first_type is None: raise ValueError(f"{module} image not found")
    out=[]
    for ti in range(first_type,first_type+type_count):
        off=to+ti*TYPE_SIZE
        name=cstr(meta,so+i32(meta,off)); ns=cstr(meta,so+i32(meta,off+4))
        typ=f"{ns}.{name}" if ns else name
        first=i32(meta,off+36); count=u16(meta,off+64)
        for mi in range(first,first+count):
            m=mo+mi*METHOD_SIZE
            token=u32(meta,m+24); rid=token&0xFFFFFF
            out.append((rid,token,typ,cstr(meta,so+i32(meta,m)),u16(meta,m+34)))
    out.sort()
    if len({x[0] for x in out})!=len(out): raise ValueError("duplicate MethodDef RID")
    return out

class PE:
    def __init__(self,b):
        if b[:2]!=b"MZ": raise ValueError("not PE")
        pe=u32(b,0x3c)
        if b[pe:pe+4]!=b"PE\0\0": raise ValueError("bad PE signature")
        n=u16(b,pe+6); osz=u16(b,pe+20); opt=pe+24
        if u16(b,opt)!=0x20B: raise ValueError("expected PE32+")
        self.base=u64(b,opt+24); self.sections=[]
        st=opt+osz
        for i in range(n):
            o=st+i*40
            vs,va,rs,rp=struct.unpack_from("<IIII",b,o+8)
            self.sections.append((va,vs,rp,rs))
    def off_to_va(self,off):
        for va,vs,rp,rs in self.sections:
            if rp<=off<rp+rs: return self.base+va+(off-rp)
        return None
    def va_to_off(self,addr):
        rva=addr-self.base
        for va,vs,rp,rs in self.sections:
            if va<=rva<va+max(vs,rs):
                d=rva-va
                if d<rs: return rp+d
        return None

def find_codegen(peb,pe,module,expected):
    needle=module.encode()+b"\0"; pos=0
    while True:
        pos=peb.find(needle,pos)
        if pos<0: break
        name_va=pe.off_to_va(pos); pos+=1
        if name_va is None: continue
        ref=struct.pack("<Q",name_va); r=0
        while True:
            r=peb.find(ref,r)
            if r<0: break
            if r+24<=len(peb):
                count=u64(peb,r+8); table=u64(peb,r+16); toff=pe.va_to_off(table)
                if count==expected and toff is not None and toff+count*8<=len(peb):
                    return name_va,count,table,toff
            r+=1
    raise ValueError(f"Il2CppCodeGenModule for {module} ({expected} pointers) not found")

def main():
    ap=argparse.ArgumentParser()
    ap.add_argument("gameassembly",type=Path)
    ap.add_argument("metadata",type=Path)
    ap.add_argument("--module",default="Assembly-CSharp.dll")
    ap.add_argument("--lookup",help="optional native address, e.g. 0x18035C4D0")
    a=ap.parse_args()
    peb=a.gameassembly.read_bytes(); meta=a.metadata.read_bytes()
    methods=metadata_methods(meta,a.module); pe=PE(peb)
    name_va,count,table_va,toff=find_codegen(peb,pe,a.module,len(methods))
    rows=[]
    for rid,token,typ,name,pc in methods:
        addr=u64(peb,toff+(rid-1)*8)
        rows.append((rid-1,rid,token,addr,typ,name,pc))
    print(f"# module={a.module} methods={len(rows)} methodPointerCount={count} moduleNameVA=0x{name_va:X} tableVA=0x{table_va:X}",file=sys.stderr)
    if a.lookup:
        target=int(a.lookup,0)
        rows=[r for r in rows if r[3]==target]
    print("native_index\trid\ttoken\tnative_address\tdeclaring_type\tmethod\tparam_count")
    for idx,rid,token,addr,typ,name,pc in rows:
        print(f"{idx}\t{rid}\t0x{token:08X}\t0x{addr:016X}\t{typ}\t{name}\t{pc}")
    return 0 if rows else 3

if __name__=="__main__":
    raise SystemExit(main())
