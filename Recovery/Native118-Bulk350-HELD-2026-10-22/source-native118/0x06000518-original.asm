0000000180381D50 mov       rax, rsp
0000000180381D53 mov       qword ptr [rax + 0x10], rbx
0000000180381D57 mov       qword ptr [rax + 0x18], rsi
0000000180381D5B mov       qword ptr [rax + 8], rcx
0000000180381D5F push      rdi
0000000180381D60 push      r12
0000000180381D62 push      r13
0000000180381D64 push      r14
0000000180381D66 push      r15
0000000180381D68 sub       rsp, 0xd0
0000000180381D6F movaps    xmmword ptr [rax - 0x38], xmm6
0000000180381D73 mov       r14, rcx
0000000180381D76 cmp       byte ptr [rip + 0x196308d], 0 # 0x181ce4e0a
0000000180381D7D jne       0x180381e26
0000000180381D83 lea       rcx, [rip + 0x1840976] # 0x181bc2700
0000000180381D8A call      0x18024fef0
0000000180381D8F lea       rcx, [rip + 0x1848ad2] # 0x181bca868
0000000180381D96 call      0x18024fef0
0000000180381D9B lea       rcx, [rip + 0x18409b6] # 0x181bc2758
0000000180381DA2 call      0x18024fef0
0000000180381DA7 lea       rcx, [rip + 0x1848b0a] # 0x181bca8b8
0000000180381DAE call      0x18024fef0
0000000180381DB3 lea       rcx, [rip + 0x1848b56] # 0x181bca910
0000000180381DBA call      0x18024fef0
0000000180381DBF lea       rcx, [rip + 0x18409ea] # 0x181bc27b0
0000000180381DC6 call      0x18024fef0
0000000180381DCB lea       rcx, [rip + 0x1820246] # 0x181ba2018
0000000180381DD2 call      0x18024fef0
0000000180381DD7 lea       rcx, [rip + 0x184190a] # 0x181bc36e8
0000000180381DDE call      0x18024fef0
0000000180381DE3 lea       rcx, [rip + 0x18272d6] # 0x181ba90c0
0000000180381DEA call      0x18024fef0
0000000180381DEF lea       rcx, [rip + 0x18419fa] # 0x181bc37f0
0000000180381DF6 call      0x18024fef0
0000000180381DFB lea       rcx, [rip + 0x182736e] # 0x181ba9170
0000000180381E02 call      0x18024fef0
0000000180381E07 lea       rcx, [rip + 0x182246a] # 0x181ba4278
0000000180381E0E call      0x18024fef0
0000000180381E13 lea       rcx, [rip + 0x183465e] # 0x181bb6478
0000000180381E1A call      0x18024fef0
0000000180381E1F mov       byte ptr [rip + 0x1962fe4], 1 # 0x181ce4e0a
0000000180381E26 xorps     xmm0, xmm0
0000000180381E29 xor       eax, eax
0000000180381E2B movups    xmmword ptr [rsp + 0x60], xmm0
0000000180381E30 mov       qword ptr [rsp + 0x70], rax
0000000180381E35 xorps     xmm1, xmm1
0000000180381E38 movups    xmmword ptr [rsp + 0x78], xmm1
0000000180381E3D mov       qword ptr [rsp + 0x88], rax
0000000180381E45 xor       bl, bl
0000000180381E47 mov       r12d, 4
0000000180381E4D mov       r13d, r12d
0000000180381E50 test      r14, r14
0000000180381E53 je        0x180382364
0000000180381E59 xor       r8d, r8d
0000000180381E5C mov       edx, r12d
0000000180381E5F mov       rcx, r14
0000000180381E62 call      0x18034fed0
0000000180381E67 mov       r15, rax
0000000180381E6A mov       qword ptr [rsp + 0x90], rax
0000000180381E72 test      rax, rax
0000000180381E75 je        0x180382364
0000000180381E7B cmp       dword ptr [rax + 0x18], 0
0000000180381E7F jle       0x180382070
0000000180381E85 mov       r8, qword ptr [rip + 0x184185c] # 0x181bc36e8
0000000180381E8C mov       rdx, rax
0000000180381E8F lea       rcx, [rsp + 0x30]
0000000180381E94 call      0x1808197f0
0000000180381E99 movups    xmm0, xmmword ptr [rsp + 0x30]
0000000180381E9E movups    xmmword ptr [rsp + 0x60], xmm0
0000000180381EA3 movsd     xmm1, qword ptr [rsp + 0x40]
0000000180381EA9 movsd     qword ptr [rsp + 0x70], xmm1
0000000180381EAF mov       qword ptr [rsp + 0x30], 0
0000000180381EB8 lea       rbx, [rsp + 0x60]
0000000180381EBD mov       qword ptr [rsp + 0x38], rbx
0000000180381EC2 movss     xmm6, dword ptr [rip + 0x1225d9e] # 0x1815a7c68
0000000180381ECA nop       word ptr [rax + rax]
0000000180381ED0 mov       rdx, qword ptr [rip + 0x18489e1] # 0x181bca8b8
0000000180381ED7 lea       rcx, [rsp + 0x60]
0000000180381EDC call      0x180669280
0000000180381EE1 test      al, al
0000000180381EE3 je        0x180382025
0000000180381EE9 mov       rdi, qword ptr [rsp + 0x70]
0000000180381EEE xor       r9d, r9d
0000000180381EF1 xor       edx, edx
0000000180381EF3 mov       r8d, 0xffffffff
0000000180381EF9 mov       rcx, r14
0000000180381EFC call      0x180350e20
0000000180381F01 test      rax, rax
0000000180381F04 je        0x180382393
0000000180381F0A mov       rcx, qword ptr [r14 + 0xe0]
0000000180381F11 test      rcx, rcx
0000000180381F14 je        0x18038238e
0000000180381F1A mov       rcx, qword ptr [rcx + 0x50]
0000000180381F1E test      rcx, rcx
0000000180381F21 je        0x180382389
0000000180381F27 cmp       dword ptr [rcx + 0x18], 0
0000000180381F2B jbe       0x18038236a
0000000180381F31 movss     xmm0, dword ptr [rcx + 0x20]
0000000180381F36 mulss     xmm0, dword ptr [rax + 0x10]
0000000180381F3B movss     dword ptr [rax + 0x10], xmm0
0000000180381F40 test      rdi, rdi
0000000180381F43 je        0x180382384
0000000180381F49 xor       r9d, r9d
0000000180381F4C xor       r8d, r8d
0000000180381F4F mov       rdx, rax
0000000180381F52 mov       rcx, rdi
0000000180381F55 call      0x180369aa0
0000000180381F5A cmp       byte ptr [rip + 0x1962bd3], 0 # 0x181ce4b34
0000000180381F61 jne       0x180381f76
0000000180381F63 lea       rcx, [rip + 0x182f926] # 0x181bb1890
0000000180381F6A call      0x18024fef0
0000000180381F6F mov       byte ptr [rip + 0x1962bbe], 1 # 0x181ce4b34
0000000180381F76 mov       rax, qword ptr [rip + 0x182f913] # 0x181bb1890
0000000180381F7D mov       rdx, qword ptr [rax + 0xb8]
0000000180381F84 mov       rcx, qword ptr [rdx]
0000000180381F87 test      rcx, rcx
0000000180381F8A je        0x18038237f
0000000180381F90 xor       r8d, r8d
0000000180381F93 mov       edx, 0x22
0000000180381F98 call      0x180321f30
0000000180381F9D mov       rsi, rax
0000000180381FA0 xor       edx, edx
0000000180381FA2 mov       rcx, rdi
0000000180381FA5 call      0x181304510
0000000180381FAA test      rax, rax
0000000180381FAD je        0x18038237a
0000000180381FB3 xor       r8d, r8d
0000000180381FB6 mov       rdx, rax
0000000180381FB9 lea       rcx, [rsp + 0x98]
0000000180381FC1 call      0x18132f3a0
0000000180381FC6 movsd     xmm1, qword ptr [rax]
0000000180381FCA mov       edi, dword ptr [rax + 8]
0000000180381FCD movaps    xmm0, xmm1
0000000180381FD0 shufps    xmm0, xmm0, 0x55
0000000180381FD4 movsd     qword ptr [rsp + 0x50], xmm1
0000000180381FDA addss     xmm0, xmm6
0000000180381FDE movss     dword ptr [rsp + 0x54], xmm0
0000000180381FE4 test      rsi, rsi
0000000180381FE7 je        0x180382375
0000000180381FED xor       edx, edx
0000000180381FEF mov       rcx, rsi
0000000180381FF2 call      0x18131c070
0000000180381FF7 test      rax, rax
0000000180381FFA je        0x180382370
0000000180382000 movsd     xmm0, qword ptr [rsp + 0x50]
0000000180382006 movsd     qword ptr [rsp + 0x20], xmm0
000000018038200C mov       dword ptr [rsp + 0x28], edi
0000000180382010 xor       r8d, r8d
0000000180382013 lea       rdx, [rsp + 0x20]
0000000180382018 mov       rcx, rax
000000018038201B call      0x18132fbb0
0000000180382020 jmp       0x180381ed0
0000000180382025 mov       rdx, qword ptr [rip + 0x184883c] # 0x181bca868
000000018038202C mov       rcx, rbx
000000018038202F call      0x180302170
0000000180382034 jmp       0x18038206e
0000000180382036 mov       rdx, qword ptr [rip + 0x184882b] # 0x181bca868
000000018038203D mov       rcx, qword ptr [rsp + 0x38]
0000000180382042 call      0x180302170
0000000180382047 mov       rcx, qword ptr [rsp + 0x30]
000000018038204C test      rcx, rcx
000000018038204F jne       0x180382399
0000000180382055 mov       r12d, 4
000000018038205B mov       r14, qword ptr [rsp + 0x100]
0000000180382063 mov       r13d, r12d
0000000180382066 mov       r15, qword ptr [rsp + 0x90]
000000018038206E mov       bl, 1
0000000180382070 cmp       dword ptr [r15 + 0x18], r13d
0000000180382074 jge       0x18038227a
000000018038207A sub       r12d, dword ptr [r15 + 0x18]
000000018038207E xor       r8d, r8d
0000000180382081 mov       edx, r12d
0000000180382084 mov       rcx, r14
0000000180382087 call      0x18034f3c0
000000018038208C test      rax, rax
000000018038208F je        0x180382364
0000000180382095 cmp       dword ptr [rax + 0x18], 0
0000000180382099 jle       0x18038227a
000000018038209F mov       r8, qword ptr [rip + 0x182701a] # 0x181ba90c0
00000001803820A6 mov       rdx, rax
00000001803820A9 lea       rcx, [rsp + 0x30]
00000001803820AE call      0x1808197f0
00000001803820B3 movups    xmm0, xmmword ptr [rsp + 0x30]
00000001803820B8 movups    xmmword ptr [rsp + 0x78], xmm0
00000001803820BD movsd     xmm1, qword ptr [rsp + 0x40]
00000001803820C3 movsd     qword ptr [rsp + 0x88], xmm1
00000001803820CC mov       qword ptr [rsp + 0x30], 0
00000001803820D5 lea       rbx, [rsp + 0x78]
00000001803820DA mov       qword ptr [rsp + 0x38], rbx
00000001803820DF movss     xmm6, dword ptr [rip + 0x1225a2d] # 0x1815a7b14
00000001803820E7 nop       word ptr [rax + rax]
00000001803820F0 mov       rdx, qword ptr [rip + 0x1840661] # 0x181bc2758
00000001803820F7 lea       rcx, [rsp + 0x78]
00000001803820FC call      0x180669280
0000000180382101 test      al, al
0000000180382103 je        0x180382248
0000000180382109 mov       rdi, qword ptr [rsp + 0x88]
0000000180382111 xor       r9d, r9d
0000000180382114 xor       edx, edx
0000000180382116 mov       r8d, 0xffffffff
000000018038211C mov       rcx, r14
000000018038211F call      0x180350e20
0000000180382124 test      rax, rax
0000000180382127 je        0x1803823c7
000000018038212D mov       rcx, qword ptr [r14 + 0xe0]
0000000180382134 test      rcx, rcx
0000000180382137 je        0x1803823c2
000000018038213D mov       rcx, qword ptr [rcx + 0x50]
0000000180382141 test      rcx, rcx
0000000180382144 je        0x1803823bd
000000018038214A cmp       dword ptr [rcx + 0x18], 0
000000018038214E jbe       0x18038239f
0000000180382154 movss     xmm0, dword ptr [rcx + 0x20]
0000000180382159 mulss     xmm0, dword ptr [rax + 0x10]
000000018038215E movss     dword ptr [rax + 0x10], xmm0
0000000180382163 test      rdi, rdi
0000000180382166 je        0x1803823b8
000000018038216C xor       r9d, r9d
000000018038216F xor       r8d, r8d
0000000180382172 mov       rdx, rax
0000000180382175 mov       rcx, rdi
0000000180382178 call      0x18034b8e0
000000018038217D cmp       byte ptr [rip + 0x19629b0], 0 # 0x181ce4b34
0000000180382184 jne       0x180382199
0000000180382186 lea       rcx, [rip + 0x182f703] # 0x181bb1890
000000018038218D call      0x18024fef0
0000000180382192 mov       byte ptr [rip + 0x196299b], 1 # 0x181ce4b34
0000000180382199 mov       rax, qword ptr [rip + 0x182f6f0] # 0x181bb1890
00000001803821A0 mov       rdx, qword ptr [rax + 0xb8]
00000001803821A7 mov       rcx, qword ptr [rdx]
00000001803821AA test      rcx, rcx
00000001803821AD je        0x1803823b3
00000001803821B3 xor       r8d, r8d
00000001803821B6 mov       edx, 0x22
00000001803821BB call      0x180321f30
00000001803821C0 mov       rsi, rax
00000001803821C3 xor       edx, edx
00000001803821C5 mov       rcx, rdi
00000001803821C8 call      0x181304510
00000001803821CD test      rax, rax
00000001803821D0 je        0x1803823ae
00000001803821D6 xor       r8d, r8d
00000001803821D9 mov       rdx, rax
00000001803821DC lea       rcx, [rsp + 0x98]
00000001803821E4 call      0x18132f3a0
00000001803821E9 movsd     xmm1, qword ptr [rax]
00000001803821ED mov       edi, dword ptr [rax + 8]
00000001803821F0 movaps    xmm0, xmm1
00000001803821F3 shufps    xmm0, xmm0, 0x55
00000001803821F7 movsd     qword ptr [rsp + 0x20], xmm1
00000001803821FD addss     xmm0, xmm6
0000000180382201 movss     dword ptr [rsp + 0x24], xmm0
0000000180382207 test      rsi, rsi
000000018038220A je        0x1803823a9
0000000180382210 xor       edx, edx
0000000180382212 mov       rcx, rsi
0000000180382215 call      0x18131c070
000000018038221A test      rax, rax
000000018038221D je        0x1803823a4
0000000180382223 movsd     xmm0, qword ptr [rsp + 0x20]
0000000180382229 movsd     qword ptr [rsp + 0x50], xmm0
000000018038222F mov       dword ptr [rsp + 0x58], edi
0000000180382233 xor       r8d, r8d
0000000180382236 lea       rdx, [rsp + 0x50]
000000018038223B mov       rcx, rax
000000018038223E call      0x18132fbb0
0000000180382243 jmp       0x1803820f0
0000000180382248 mov       rdx, qword ptr [rip + 0x18404b1] # 0x181bc2700
000000018038224F mov       rcx, rbx
0000000180382252 call      0x180302170
0000000180382257 jmp       0x180382282
0000000180382259 mov       rdx, qword ptr [rip + 0x18404a0] # 0x181bc2700
0000000180382260 mov       rcx, qword ptr [rsp + 0x38]
0000000180382265 call      0x180302170
000000018038226A mov       rcx, qword ptr [rsp + 0x30]
000000018038226F test      rcx, rcx
0000000180382272 jne       0x1803823cd
0000000180382278 jmp       0x180382282
000000018038227A test      bl, bl
000000018038227C je        0x180382342
0000000180382282 mov       rax, qword ptr [rip + 0x18341ef] # 0x181bb6478
0000000180382289 cmp       dword ptr [rax + 0xe0], 0
0000000180382290 jne       0x1803822a1
0000000180382292 mov       rcx, rax
0000000180382295 call      0x1802501e0
000000018038229A mov       rax, qword ptr [rip + 0x18341d7] # 0x181bb6478
00000001803822A1 mov       rax, qword ptr [rax + 0xb8]
00000001803822A8 mov       rcx, qword ptr [rax + 0x130]
00000001803822AF test      rcx, rcx
00000001803822B2 je        0x180382364
00000001803822B8 mov       r8, qword ptr [rip + 0x1821fb9] # 0x181ba4278
00000001803822BF mov       edx, 0x18
00000001803822C4 call      0x180825710
00000001803822C9 mov       rbx, rax
00000001803822CC xor       ecx, ecx
00000001803822CE call      0x1812e27c0
00000001803822D3 test      rax, rax
00000001803822D6 je        0x180382364
00000001803822DC xor       edx, edx
00000001803822DE mov       rcx, rax
00000001803822E1 call      0x181304510
00000001803822E6 test      rax, rax
00000001803822E9 je        0x180382364
00000001803822EB xor       r8d, r8d
00000001803822EE mov       rdx, rax
00000001803822F1 lea       rcx, [rsp + 0x98]
00000001803822F9 call      0x18132f3a0
00000001803822FE movsd     xmm6, qword ptr [rax]
0000000180382302 mov       edi, dword ptr [rax + 8]
0000000180382305 mov       rcx, qword ptr [rip + 0x181fd0c] # 0x181ba2018
000000018038230C cmp       dword ptr [rcx + 0xe0], 0
0000000180382313 jne       0x18038231a
0000000180382315 call      0x1802501e0
000000018038231A movsd     qword ptr [rsp + 0x20], xmm6
0000000180382320 mov       dword ptr [rsp + 0x28], edi
0000000180382324 xor       ecx, ecx
0000000180382326 call      0x18031aea0
000000018038232B addss     xmm0, xmm0
000000018038232F xor       r9d, r9d
0000000180382332 movaps    xmm2, xmm0
0000000180382335 lea       rdx, [rsp + 0x20]
000000018038233A mov       rcx, rbx
000000018038233D call      0x18031b230
0000000180382342 lea       r11, [rsp + 0xd0]
000000018038234A mov       rbx, qword ptr [r11 + 0x38]
000000018038234E mov       rsi, qword ptr [r11 + 0x40]
0000000180382352 movaps    xmm6, xmmword ptr [r11 - 0x10]
0000000180382357 mov       rsp, r11
000000018038235A pop       r15
000000018038235C pop       r14
000000018038235E pop       r13
0000000180382360 pop       r12
0000000180382362 pop       rdi
0000000180382363 ret       
0000000180382364 call      0x180250150
0000000180382369 nop       
000000018038236A call      0x180250140
000000018038236F nop       
0000000180382370 call      0x180250150
0000000180382375 call      0x180250150
000000018038237A call      0x180250150
000000018038237F call      0x180250150
0000000180382384 call      0x180250150
0000000180382389 call      0x180250150
000000018038238E call      0x180250150
0000000180382393 call      0x180250150
0000000180382398 nop       
0000000180382399 call      0x180200e60
000000018038239E nop       
000000018038239F call      0x180250140
00000001803823A4 call      0x180250150
00000001803823A9 call      0x180250150
00000001803823AE call      0x180250150
00000001803823B3 call      0x180250150
00000001803823B8 call      0x180250150
00000001803823BD call      0x180250150
00000001803823C2 call      0x180250150
00000001803823C7 call      0x180250150
00000001803823CC nop       
00000001803823CD call      0x180200e60
00000001803823D2 int3      
00000001803823D3 int3      
00000001803823D4 int3      
00000001803823D5 int3      
00000001803823D6 int3      
00000001803823D7 int3      
00000001803823D8 int3      
00000001803823D9 int3      
00000001803823DA int3      
00000001803823DB int3      
00000001803823DC int3      
00000001803823DD int3      
00000001803823DE int3      
00000001803823DF int3      