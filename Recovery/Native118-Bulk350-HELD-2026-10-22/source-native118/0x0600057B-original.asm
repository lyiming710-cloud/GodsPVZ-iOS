000000018038BDB0 mov       qword ptr [rsp + 0x10], rbx
000000018038BDB5 mov       qword ptr [rsp + 8], rcx
000000018038BDBA push      rsi
000000018038BDBB push      rdi
000000018038BDBC push      r12
000000018038BDBE push      r14
000000018038BDC0 push      r15
000000018038BDC2 sub       rsp, 0x60
000000018038BDC6 mov       rsi, rcx
000000018038BDC9 cmp       byte ptr [rip + 0x1959073], 0 # 0x181ce4e43
000000018038BDD0 jne       0x18038be6d
000000018038BDD6 lea       rcx, [rip + 0x180ef6b] # 0x181b9ad48
000000018038BDDD call      0x18024fef0
000000018038BDE2 lea       rcx, [rip + 0x183d3d7] # 0x181bc91c0
000000018038BDE9 call      0x18024fef0
000000018038BDEE lea       rcx, [rip + 0x183d423] # 0x181bc9218
000000018038BDF5 call      0x18024fef0
000000018038BDFA lea       rcx, [rip + 0x183d467] # 0x181bc9268
000000018038BE01 call      0x18024fef0
000000018038BE06 lea       rcx, [rip + 0x181620b] # 0x181ba2018
000000018038BE0D call      0x18024fef0
000000018038BE12 lea       rcx, [rip + 0x1831757] # 0x181bbd570
000000018038BE19 call      0x18024fef0
000000018038BE1E lea       rcx, [rip + 0x18317a3] # 0x181bbd5c8
000000018038BE25 call      0x18024fef0
000000018038BE2A lea       rcx, [rip + 0x1831377] # 0x181bbd1a8
000000018038BE31 call      0x18024fef0
000000018038BE36 lea       rcx, [rip + 0x183183b] # 0x181bbd678
000000018038BE3D call      0x18024fef0
000000018038BE42 lea       rcx, [rip + 0x1831887] # 0x181bbd6d0
000000018038BE49 call      0x18024fef0
000000018038BE4E lea       rcx, [rip + 0x18246ab] # 0x181bb0500
000000018038BE55 call      0x18024fef0
000000018038BE5A lea       rcx, [rip + 0x181d1f7] # 0x181ba9058
000000018038BE61 call      0x18024fef0
000000018038BE66 mov       byte ptr [rip + 0x1958fd6], 1 # 0x181ce4e43
000000018038BE6D xorps     xmm0, xmm0
000000018038BE70 xor       eax, eax
000000018038BE72 movups    xmmword ptr [rsp + 0x40], xmm0
000000018038BE77 mov       qword ptr [rsp + 0x50], rax
000000018038BE7C mov       rdi, qword ptr [rsi + 0x60]
000000018038BE80 test      rdi, rdi
000000018038BE83 je        0x18038c398
000000018038BE89 mov       edi, dword ptr [rdi + 0x44]
000000018038BE8C mov       ecx, dword ptr [rsi + 0x110]
000000018038BE92 test      ecx, ecx
000000018038BE94 je        0x18038bece
000000018038BE96 sub       ecx, 1
000000018038BE99 je        0x18038bec5
000000018038BE9B sub       ecx, 1
000000018038BE9E je        0x18038bebc
000000018038BEA0 sub       ecx, 1
000000018038BEA3 je        0x18038beb3
000000018038BEA5 cmp       ecx, 1
000000018038BEA8 jne       0x18038bece
000000018038BEAA imul      r12d, edi, 0x61a8
000000018038BEB1 jmp       0x18038bed1
000000018038BEB3 imul      r12d, edi, 0x1770
000000018038BEBA jmp       0x18038bed1
000000018038BEBC imul      r12d, edi, 0x5dc
000000018038BEC3 jmp       0x18038bed1
000000018038BEC5 imul      r12d, edi, 0x1f4
000000018038BECC jmp       0x18038bed1
000000018038BECE xor       r12d, r12d
000000018038BED1 mov       dword ptr [rsp + 0xa0], r12d
000000018038BED9 xor       ebx, ebx
000000018038BEDB xor       edx, edx
000000018038BEDD mov       rax, qword ptr [rsi + 0x168]
000000018038BEE4 test      rax, rax
000000018038BEE7 je        0x18038c398
000000018038BEED nop       dword ptr [rax]
000000018038BEF0 mov       rcx, qword ptr [rsi + 0x168]
000000018038BEF7 cmp       edx, dword ptr [rax + 0x18]
000000018038BEFA jge       0x18038bf5e
000000018038BEFC test      rcx, rcx
000000018038BEFF je        0x18038c398
000000018038BF05 mov       r8, qword ptr [rip + 0x18317c4] # 0x181bbd6d0
000000018038BF0C mov       edx, ebx
000000018038BF0E call      0x180825710
000000018038BF13 test      rax, rax
000000018038BF16 je        0x18038c398
000000018038BF1C xor       edx, edx
000000018038BF1E mov       rcx, rax
000000018038BF21 call      0x1813044d0
000000018038BF26 mov       r14, rax
000000018038BF29 mov       rcx, qword ptr [rip + 0x18245d0] # 0x181bb0500
000000018038BF30 cmp       dword ptr [rcx + 0xe0], 0
000000018038BF37 jne       0x18038bf3e
000000018038BF39 call      0x1802501e0
000000018038BF3E xor       edx, edx
000000018038BF40 mov       rcx, r14
000000018038BF43 call      0x18131e940
000000018038BF48 inc       ebx
000000018038BF4A mov       edx, ebx
000000018038BF4C mov       rax, qword ptr [rsi + 0x168]
000000018038BF53 test      rax, rax
000000018038BF56 je        0x18038c398
000000018038BF5C jmp       0x18038bef0
000000018038BF5E test      rcx, rcx
000000018038BF61 je        0x18038c398
000000018038BF67 inc       dword ptr [rcx + 0x1c]
000000018038BF6A mov       r8d, dword ptr [rcx + 0x18]
000000018038BF6E mov       dword ptr [rcx + 0x18], 0
000000018038BF75 test      r8d, r8d
000000018038BF78 jle       0x18038bf88
000000018038BF7A xor       r9d, r9d
000000018038BF7D xor       edx, edx
000000018038BF7F mov       rcx, qword ptr [rcx + 0x10]
000000018038BF83 call      0x180cb86f0
000000018038BF88 mov       rax, qword ptr [rip + 0x1816089] # 0x181ba2018
000000018038BF8F cmp       dword ptr [rax + 0xe0], 0
000000018038BF96 jne       0x18038bfa7
000000018038BF98 mov       rcx, rax
000000018038BF9B call      0x1802501e0
000000018038BFA0 mov       rax, qword ptr [rip + 0x1816071] # 0x181ba2018
000000018038BFA7 mov       rax, qword ptr [rax + 0xb8]
000000018038BFAE mov       rax, qword ptr [rax]
000000018038BFB1 test      rax, rax
000000018038BFB4 je        0x18038c398
000000018038BFBA mov       rax, qword ptr [rax + 0x28]
000000018038BFBE test      rax, rax
000000018038BFC1 je        0x18038c398
000000018038BFC7 mov       rcx, qword ptr [rax + 0x10]
000000018038BFCB test      rcx, rcx
000000018038BFCE je        0x18038c398
000000018038BFD4 xor       r8d, r8d
000000018038BFD7 mov       edx, 5
000000018038BFDC call      0x180380100
000000018038BFE1 test      rax, rax
000000018038BFE4 je        0x18038c398
000000018038BFEA xor       r8d, r8d
000000018038BFED mov       dl, 1
000000018038BFEF mov       rcx, rax
000000018038BFF2 call      0x180386d30
000000018038BFF7 mov       rbx, rax
000000018038BFFA mov       rcx, qword ptr [rsi + 0x168]
000000018038C001 test      rcx, rcx
000000018038C004 je        0x18038c398
000000018038C00A mov       r9, qword ptr [rip + 0x183155f] # 0x181bbd570
000000018038C011 inc       dword ptr [rcx + 0x1c]
000000018038C014 mov       rdx, qword ptr [rcx + 0x10]
000000018038C018 movsxd    r8, dword ptr [rcx + 0x18]
000000018038C01C test      rdx, rdx
000000018038C01F je        0x18038c398
000000018038C025 cmp       r8d, dword ptr [rdx + 0x18]
000000018038C029 jb        0x18038c044
000000018038C02B mov       rax, qword ptr [r9 + 0x20]
000000018038C02F mov       r8, qword ptr [rax + 0xc0]
000000018038C036 mov       r8, qword ptr [r8 + 0x70]
000000018038C03A mov       rdx, rbx
000000018038C03D call      0x180847ce0
000000018038C042 jmp       0x18038c06a
000000018038C044 lea       eax, [r8 + 1]
000000018038C048 mov       dword ptr [rcx + 0x18], eax
000000018038C04B cmp       r8d, dword ptr [rdx + 0x18]
000000018038C04F jae       0x18038c39e
000000018038C055 mov       qword ptr [rdx + r8*8 + 0x20], rbx
000000018038C05A add       rdx, 0x20
000000018038C05E lea       rcx, [rdx + r8*8]
000000018038C062 mov       rdx, rbx
000000018038C065 call      0x18024f360
000000018038C06A test      rbx, rbx
000000018038C06D je        0x18038c398
000000018038C073 xor       edx, edx
000000018038C075 mov       rcx, rbx
000000018038C078 call      0x1813044d0
000000018038C07D test      rax, rax
000000018038C080 je        0x18038c398
000000018038C086 xor       edx, edx
000000018038C088 mov       rcx, rax
000000018038C08B call      0x18131c070
000000018038C090 mov       r14, rax
000000018038C093 mov       rcx, qword ptr [rsi + 0x160]
000000018038C09A test      rcx, rcx
000000018038C09D je        0x18038c398
000000018038C0A3 xor       edx, edx
000000018038C0A5 call      0x18131c070
000000018038C0AA test      r14, r14
000000018038C0AD je        0x18038c398
000000018038C0B3 xor       r9d, r9d
000000018038C0B6 xor       r8d, r8d
000000018038C0B9 mov       rdx, rax
000000018038C0BC mov       rcx, r14
000000018038C0BF call      0x18132e900
000000018038C0C4 mov       ecx, dword ptr [rsi + 0x110]
000000018038C0CA test      ecx, ecx
000000018038C0CC je        0x18038c0ff
000000018038C0CE sub       ecx, 1
000000018038C0D1 je        0x18038c0f4
000000018038C0D3 sub       ecx, 1
000000018038C0D6 je        0x18038c0ef
000000018038C0D8 sub       ecx, 1
000000018038C0DB je        0x18038c0ea
000000018038C0DD cmp       ecx, 1
000000018038C0E0 jne       0x18038c0ff
000000018038C0E2 lea       eax, [rdi + rdi*2]
000000018038C0E5 shl       eax, 2
000000018038C0E8 jmp       0x18038c101
000000018038C0EA lea       eax, [rdi + rdi*4]
000000018038C0ED jmp       0x18038c101
000000018038C0EF lea       eax, [rdi + rdi]
000000018038C0F2 jmp       0x18038c101
000000018038C0F4 mov       eax, edi
000000018038C0F6 cdq       
000000018038C0F7 sub       eax, edx
000000018038C0F9 sar       eax, 1
000000018038C0FB inc       eax
000000018038C0FD jmp       0x18038c101
000000018038C0FF xor       eax, eax
000000018038C101 mov       dword ptr [rbx + 0x28], eax
000000018038C104 mov       rcx, qword ptr [rip + 0x180ec3d] # 0x181b9ad48
000000018038C10B cmp       dword ptr [rcx + 0xe0], 0
000000018038C112 jne       0x18038c119
000000018038C114 call      0x1802501e0
000000018038C119 xor       edx, edx
000000018038C11B mov       rcx, qword ptr [rip + 0x181cf36] # 0x181ba9058
000000018038C122 call      0x1812e68f0
000000018038C127 mov       rax, qword ptr [rip + 0x1815eea] # 0x181ba2018
000000018038C12E cmp       dword ptr [rax + 0xe0], 0
000000018038C135 jne       0x18038c146
000000018038C137 mov       rcx, rax
000000018038C13A call      0x1802501e0
000000018038C13F mov       rax, qword ptr [rip + 0x1815ed2] # 0x181ba2018
000000018038C146 mov       rax, qword ptr [rax + 0xb8]
000000018038C14D mov       rcx, qword ptr [rax]
000000018038C150 test      rcx, rcx
000000018038C153 je        0x18038c398
000000018038C159 mov       rax, qword ptr [rcx + 0x28]
000000018038C15D test      rax, rax
000000018038C160 je        0x18038c398
000000018038C166 mov       rcx, qword ptr [rax + 0x20]
000000018038C16A test      rcx, rcx
000000018038C16D je        0x18038c398
000000018038C173 xor       r9d, r9d
000000018038C176 mov       r8d, dword ptr [rsi + 0x110]
000000018038C17D mov       edx, dword ptr [rsi + 0x48]
000000018038C180 call      0x18032e440
000000018038C185 test      rax, rax
000000018038C188 je        0x18038c398
000000018038C18E mov       r8, qword ptr [rip + 0x1831013] # 0x181bbd1a8
000000018038C195 mov       rdx, rax
000000018038C198 lea       rcx, [rsp + 0x28]
000000018038C19D call      0x1808197f0
000000018038C1A2 movups    xmm0, xmmword ptr [rsp + 0x28]
000000018038C1A7 movups    xmmword ptr [rsp + 0x40], xmm0
000000018038C1AC movsd     xmm1, qword ptr [rsp + 0x38]
000000018038C1B2 movsd     qword ptr [rsp + 0x50], xmm1
000000018038C1B8 mov       qword ptr [rsp + 0x28], 0
000000018038C1C1 lea       rbx, [rsp + 0x40]
000000018038C1C6 mov       qword ptr [rsp + 0x30], rbx
000000018038C1CB nop       dword ptr [rax + rax]
000000018038C1D0 mov       rdx, qword ptr [rip + 0x183d041] # 0x181bc9218
000000018038C1D7 lea       rcx, [rsp + 0x40]
000000018038C1DC call      0x180669280
000000018038C1E1 test      al, al
000000018038C1E3 je        0x18038c340
000000018038C1E9 mov       r14, qword ptr [rsp + 0x50]
000000018038C1EE mov       rax, qword ptr [rip + 0x1815e23] # 0x181ba2018
000000018038C1F5 cmp       dword ptr [rax + 0xe0], 0
000000018038C1FC jne       0x18038c20d
000000018038C1FE mov       rcx, rax
000000018038C201 call      0x1802501e0
000000018038C206 mov       rax, qword ptr [rip + 0x1815e0b] # 0x181ba2018
000000018038C20D mov       rax, qword ptr [rax + 0xb8]
000000018038C214 mov       rax, qword ptr [rax]
000000018038C217 test      rax, rax
000000018038C21A je        0x18038c3dc
000000018038C220 mov       rax, qword ptr [rax + 0x28]
000000018038C224 test      rax, rax
000000018038C227 je        0x18038c3d7
000000018038C22D mov       rcx, qword ptr [rax + 0x10]
000000018038C231 test      r14, r14
000000018038C234 je        0x18038c3d2
000000018038C23A test      rcx, rcx
000000018038C23D je        0x18038c3cd
000000018038C243 xor       r8d, r8d
000000018038C246 mov       edx, dword ptr [r14 + 0x18]
000000018038C24A call      0x180380100
000000018038C24F test      rax, rax
000000018038C252 je        0x18038c3c8
000000018038C258 xor       r8d, r8d
000000018038C25B mov       dl, 1
000000018038C25D mov       rcx, rax
000000018038C260 call      0x180386d30
000000018038C265 mov       rdi, rax
000000018038C268 mov       r9, qword ptr [rsi + 0x168]
000000018038C26F test      r9, r9
000000018038C272 je        0x18038c3c3
000000018038C278 mov       rax, qword ptr [rip + 0x18312f1] # 0x181bbd570
000000018038C27F inc       dword ptr [r9 + 0x1c]
000000018038C283 mov       rcx, qword ptr [r9 + 0x10]
000000018038C287 movsxd    rdx, dword ptr [r9 + 0x18]
000000018038C28B test      rcx, rcx
000000018038C28E je        0x18038c3be
000000018038C294 cmp       edx, dword ptr [rcx + 0x18]
000000018038C297 jb        0x18038c2b5
000000018038C299 mov       rax, qword ptr [rax + 0x20]
000000018038C29D mov       r8, qword ptr [rax + 0xc0]
000000018038C2A4 mov       r8, qword ptr [r8 + 0x70]
000000018038C2A8 mov       rdx, rdi
000000018038C2AB mov       rcx, r9
000000018038C2AE call      0x180847ce0
000000018038C2B3 jmp       0x18038c2da
000000018038C2B5 lea       eax, [rdx + 1]
000000018038C2B8 mov       dword ptr [r9 + 0x18], eax
000000018038C2BC cmp       edx, dword ptr [rcx + 0x18]
000000018038C2BF jae       0x18038c3a4
000000018038C2C5 mov       qword ptr [rcx + rdx*8 + 0x20], rdi
000000018038C2CA lea       rcx, [rcx + rdx*8]
000000018038C2CE add       rcx, 0x20
000000018038C2D2 mov       rdx, rdi
000000018038C2D5 call      0x18024f360
000000018038C2DA test      rdi, rdi
000000018038C2DD je        0x18038c3b9
000000018038C2E3 xor       edx, edx
000000018038C2E5 mov       rcx, rdi
000000018038C2E8 call      0x1813044d0
000000018038C2ED test      rax, rax
000000018038C2F0 je        0x18038c3b4
000000018038C2F6 xor       edx, edx
000000018038C2F8 mov       rcx, rax
000000018038C2FB call      0x18131c070
000000018038C300 mov       r15, rax
000000018038C303 mov       rcx, qword ptr [rsi + 0x160]
000000018038C30A test      rcx, rcx
000000018038C30D je        0x18038c3af
000000018038C313 xor       edx, edx
000000018038C315 call      0x18131c070
000000018038C31A test      r15, r15
000000018038C31D je        0x18038c3aa
000000018038C323 xor       r9d, r9d
000000018038C326 xor       r8d, r8d
000000018038C329 mov       rdx, rax
000000018038C32C mov       rcx, r15
000000018038C32F call      0x18132e900
000000018038C334 mov       eax, dword ptr [r14 + 0x20]
000000018038C338 mov       dword ptr [rdi + 0x28], eax
000000018038C33B jmp       0x18038c1d0
000000018038C340 mov       rdx, qword ptr [rip + 0x183ce79] # 0x181bc91c0
000000018038C347 mov       rcx, rbx
000000018038C34A call      0x180302170
000000018038C34F jmp       0x18038c37c
000000018038C351 mov       rdx, qword ptr [rip + 0x183ce68] # 0x181bc91c0
000000018038C358 mov       rcx, qword ptr [rsp + 0x30]
000000018038C35D call      0x180302170
000000018038C362 mov       rcx, qword ptr [rsp + 0x28]
000000018038C367 test      rcx, rcx
000000018038C36A jne       0x18038c3e2
000000018038C36C mov       rsi, qword ptr [rsp + 0x90]
000000018038C374 mov       r12d, dword ptr [rsp + 0xa0]
000000018038C37C mov       dword ptr [rsi + 0x188], r12d
000000018038C383 mov       rbx, qword ptr [rsp + 0x98]
000000018038C38B add       rsp, 0x60
000000018038C38F pop       r15
000000018038C391 pop       r14
000000018038C393 pop       r12
000000018038C395 pop       rdi
000000018038C396 pop       rsi
000000018038C397 ret       
000000018038C398 call      0x180250150
000000018038C39D int3      
000000018038C39E call      0x180250140
000000018038C3A3 nop       
000000018038C3A4 call      0x180250140
000000018038C3A9 nop       
000000018038C3AA call      0x180250150
000000018038C3AF call      0x180250150
000000018038C3B4 call      0x180250150
000000018038C3B9 call      0x180250150
000000018038C3BE call      0x180250150
000000018038C3C3 call      0x180250150
000000018038C3C8 call      0x180250150
000000018038C3CD call      0x180250150
000000018038C3D2 call      0x180250150
000000018038C3D7 call      0x180250150
000000018038C3DC call      0x180250150
000000018038C3E1 nop       
000000018038C3E2 call      0x180200e60
000000018038C3E7 int3      
000000018038C3E8 int3      
000000018038C3E9 int3      
000000018038C3EA int3      
000000018038C3EB int3      
000000018038C3EC int3      
000000018038C3ED int3      
000000018038C3EE int3      
000000018038C3EF int3      