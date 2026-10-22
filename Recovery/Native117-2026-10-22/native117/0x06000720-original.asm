00000001803C2150 mov       qword ptr [rsp + 0x18], rbx
00000001803C2155 push      rdi
00000001803C2156 sub       rsp, 0xf0
00000001803C215D cmp       byte ptr [rip + 0x1922de5], 0 # 0x181ce4f49
00000001803C2164 mov       rbx, rcx
00000001803C2167 jne       0x1803c217c
00000001803C2169 lea       rcx, [rip + 0x1812170] # 0x181bd42e0
00000001803C2170 call      0x18024fef0
00000001803C2175 mov       byte ptr [rip + 0x1922dcd], 1 # 0x181ce4f49
00000001803C217C mov       eax, dword ptr [rbx + 0x10]
00000001803C217F mov       rdi, qword ptr [rbx + 0x20]
00000001803C2183 test      eax, eax
00000001803C2185 jne       0x1803c21a3
00000001803C2187 xorps     xmm0, xmm0
00000001803C218A mov       dword ptr [rbx + 0x10], 0xffffffff
00000001803C2191 movups    xmmword ptr [rbx + 0x28], xmm0
00000001803C2195 movups    xmmword ptr [rbx + 0x38], xmm0
00000001803C2199 movups    xmmword ptr [rbx + 0x48], xmm0
00000001803C219D movups    xmmword ptr [rbx + 0x58], xmm0
00000001803C21A1 jmp       0x1803c21b3
00000001803C21A3 cmp       eax, 1
00000001803C21A6 jne       0x1803c23b7
00000001803C21AC mov       dword ptr [rbx + 0x10], 0xffffffff
00000001803C21B3 cmp       byte ptr [rip + 0x192297c], 0 # 0x181ce4b36
00000001803C21BA mov       qword ptr [rsp + 0x100], rbp
00000001803C21C2 mov       qword ptr [rsp + 0x108], rsi
00000001803C21CA movaps    xmmword ptr [rsp + 0xb0], xmm9
00000001803C21D3 movaps    xmmword ptr [rsp + 0xa0], xmm10
00000001803C21DC jne       0x1803c21f1
00000001803C21DE lea       rcx, [rip + 0x1800603] # 0x181bc27e8
00000001803C21E5 call      0x18024fef0
00000001803C21EA mov       byte ptr [rip + 0x1922945], 1 # 0x181ce4b36
00000001803C21F1 mov       rax, qword ptr [rip + 0x18005f0] # 0x181bc27e8
00000001803C21F8 mov       rcx, qword ptr [rax + 0xb8]
00000001803C21FF movsd     xmm9, qword ptr [rcx]
00000001803C2204 mov       ebp, dword ptr [rcx + 8]
00000001803C2207 xor       ecx, ecx
00000001803C2209 call      0x18132c470
00000001803C220E movaps    xmm10, xmm0
00000001803C2212 test      rdi, rdi
00000001803C2215 je        0x1803c23ca
00000001803C221B movaps    xmmword ptr [rsp + 0xe0], xmm6
00000001803C2223 xor       ecx, ecx
00000001803C2225 movaps    xmmword ptr [rsp + 0xd0], xmm7
00000001803C222D movaps    xmmword ptr [rsp + 0xc0], xmm8
00000001803C2236 movss     xmm8, dword ptr [rdi + 0x20]
00000001803C223C call      0x18132c470
00000001803C2241 movss     xmm7, dword ptr [rdi + 0x24]
00000001803C2246 xor       ecx, ecx
00000001803C2248 movaps    xmm6, xmm0
00000001803C224B call      0x18132c470
00000001803C2250 movss     xmm1, dword ptr [rip + 0x11e5b8c] # 0x1815a7de4
00000001803C2258 movaps    xmm2, xmm0
00000001803C225B mulss     xmm2, dword ptr [rdi + 0x28]
00000001803C2260 mulss     xmm8, xmm10
00000001803C2265 mulss     xmm7, xmm6
00000001803C2269 mulss     xmm2, xmm1
00000001803C226D mulss     xmm8, xmm1
00000001803C2272 mulss     xmm7, xmm1
00000001803C2276 unpcklps  xmm8, xmm7
00000001803C227A lea       rdx, [rsp + 0x30]
00000001803C227F xor       r8d, r8d
00000001803C2282 movsd     qword ptr [rsp + 0x30], xmm8
00000001803C2289 lea       rcx, [rsp + 0x50]
00000001803C228E movss     dword ptr [rsp + 0x38], xmm2
00000001803C2294 call      0x18130c6c0
00000001803C2299 cmp       byte ptr [rip + 0x1922b7a], 0 # 0x181ce4e1a
00000001803C22A0 movups    xmm6, xmmword ptr [rax]
00000001803C22A3 movaps    xmm8, xmmword ptr [rsp + 0xc0]
00000001803C22AC movaps    xmm7, xmmword ptr [rsp + 0xd0]
00000001803C22B4 jne       0x1803c22c9
00000001803C22B6 lea       rcx, [rip + 0x180052b] # 0x181bc27e8
00000001803C22BD call      0x18024fef0
00000001803C22C2 mov       byte ptr [rip + 0x1922b51], 1 # 0x181ce4e1a
00000001803C22C9 mov       rax, qword ptr [rip + 0x1800518] # 0x181bc27e8
00000001803C22D0 lea       r9, [rsp + 0x30]
00000001803C22D5 mov       dword ptr [rsp + 0x48], ebp
00000001803C22D9 lea       r8, [rsp + 0x50]
00000001803C22DE xor       ebp, ebp
00000001803C22E0 movdqa    xmmword ptr [rsp + 0x50], xmm6
00000001803C22E6 lea       rdx, [rsp + 0x40]
00000001803C22EB movsd     qword ptr [rsp + 0x40], xmm9
00000001803C22F2 mov       rax, qword ptr [rax + 0xb8]
00000001803C22F9 lea       rcx, [rbx + 0x28]
00000001803C22FD mov       qword ptr [rsp + 0x20], rbp
00000001803C2302 movsd     xmm0, qword ptr [rax + 0xc]
00000001803C2307 mov       eax, dword ptr [rax + 0x14]
00000001803C230A movsd     qword ptr [rsp + 0x30], xmm0
00000001803C2310 mov       dword ptr [rsp + 0x38], eax
00000001803C2314 call      0x181309d80
00000001803C2319 mov       rcx, qword ptr [rdi + 0x38]
00000001803C231D movaps    xmm6, xmmword ptr [rsp + 0xe0]
00000001803C2325 test      rcx, rcx
00000001803C2328 je        0x1803c23ca
00000001803C232E movups    xmm0, xmmword ptr [rbx + 0x28]
00000001803C2332 mov       rdx, qword ptr [rip + 0x1811fa7] # 0x181bd42e0
00000001803C2339 xor       r9d, r9d
00000001803C233C movups    xmm1, xmmword ptr [rbx + 0x38]
00000001803C2340 lea       r8, [rsp + 0x60]
00000001803C2345 movaps    xmmword ptr [rsp + 0x60], xmm0
00000001803C234A movups    xmm0, xmmword ptr [rbx + 0x48]
00000001803C234E movaps    xmmword ptr [rsp + 0x70], xmm1
00000001803C2353 movups    xmm1, xmmword ptr [rbx + 0x58]
00000001803C2357 movaps    xmmword ptr [rsp + 0x80], xmm0
00000001803C235F movaps    xmmword ptr [rsp + 0x90], xmm1
00000001803C2367 call      0x1812f00c0
00000001803C236C lea       rcx, [rbx + 0x18]
00000001803C2370 mov       qword ptr [rbx + 0x18], rbp
00000001803C2374 xor       edx, edx
00000001803C2376 call      0x18024f360
00000001803C237B movaps    xmm9, xmmword ptr [rsp + 0xb0]
00000001803C2384 mov       al, 1
00000001803C2386 mov       rsi, qword ptr [rsp + 0x108]
00000001803C238E mov       rbp, qword ptr [rsp + 0x100]
00000001803C2396 movaps    xmm10, xmmword ptr [rsp + 0xa0]
00000001803C239F mov       dword ptr [rbx + 0x10], 1
00000001803C23A6 mov       rbx, qword ptr [rsp + 0x110]
00000001803C23AE add       rsp, 0xf0
00000001803C23B5 pop       rdi
00000001803C23B6 ret       
00000001803C23B7 mov       rbx, qword ptr [rsp + 0x110]
00000001803C23BF xor       al, al
00000001803C23C1 add       rsp, 0xf0
00000001803C23C8 pop       rdi
00000001803C23C9 ret       
00000001803C23CA call      0x180250150
00000001803C23CF int3      