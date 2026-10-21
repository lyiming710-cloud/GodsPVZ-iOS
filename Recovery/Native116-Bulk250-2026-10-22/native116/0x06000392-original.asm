0000000180354BA0 mov       qword ptr [rsp + 8], rcx
0000000180354BA5 push      rbx
0000000180354BA6 push      rsi
0000000180354BA7 push      rdi
0000000180354BA8 push      r12
0000000180354BAA push      r13
0000000180354BAC push      r14
0000000180354BAE push      r15
0000000180354BB0 sub       rsp, 0x80
0000000180354BB7 movaps    xmmword ptr [rsp + 0x70], xmm6
0000000180354BBC movaps    xmmword ptr [rsp + 0x60], xmm7
0000000180354BC1 mov       r13, rcx
0000000180354BC4 cmp       byte ptr [rip + 0x1990152], 0 # 0x181ce4d1d
0000000180354BCB jne       0x180354c40
0000000180354BCD lea       rcx, [rip + 0x187245c] # 0x181bc7030
0000000180354BD4 call      0x18024fef0
0000000180354BD9 lea       rcx, [rip + 0x18724a8] # 0x181bc7088
0000000180354BE0 call      0x18024fef0
0000000180354BE5 lea       rcx, [rip + 0x18724f4] # 0x181bc70e0
0000000180354BEC call      0x18024fef0
0000000180354BF1 lea       rcx, [rip + 0x1860e18] # 0x181bb5a10
0000000180354BF8 call      0x18024fef0
0000000180354BFD lea       rcx, [rip + 0x1860e64] # 0x181bb5a68
0000000180354C04 call      0x18024fef0
0000000180354C09 lea       rcx, [rip + 0x1860da8] # 0x181bb59b8
0000000180354C10 call      0x18024fef0
0000000180354C15 lea       rcx, [rip + 0x1860ea4] # 0x181bb5ac0
0000000180354C1C call      0x18024fef0
0000000180354C21 lea       rcx, [rip + 0x186ace0] # 0x181bbf908
0000000180354C28 call      0x18024fef0
0000000180354C2D lea       rcx, [rip + 0x185b8cc] # 0x181bb0500
0000000180354C34 call      0x18024fef0
0000000180354C39 mov       byte ptr [rip + 0x19900dd], 1 # 0x181ce4d1d
0000000180354C40 cmp       dword ptr [r13 + 0x88], 4
0000000180354C48 jl        0x180354ea4
0000000180354C4E mov       rcx, qword ptr [rip + 0x186acb3] # 0x181bbf908
0000000180354C55 call      0x180250100
0000000180354C5A mov       r14, rax
0000000180354C5D mov       rdx, qword ptr [rip + 0x1860d54] # 0x181bb59b8
0000000180354C64 mov       rcx, rax
0000000180354C67 call      0x1808053d0
0000000180354C6C mov       qword ptr [rsp + 0xd8], r14
0000000180354C74 xor       esi, esi
0000000180354C76 mov       r15d, esi
0000000180354C79 mov       dword ptr [rsp + 0xd0], esi
0000000180354C80 mov       rax, qword ptr [r13 + 0x1e8]
0000000180354C87 test      rax, rax
0000000180354C8A je        0x180354ec1
0000000180354C90 mov       rax, qword ptr [rax + 0xf8]
0000000180354C97 test      rax, rax
0000000180354C9A je        0x180354ec1
0000000180354CA0 mov       rdx, qword ptr [rax + 0x28]
0000000180354CA4 test      rdx, rdx
0000000180354CA7 je        0x180354ec1
0000000180354CAD mov       r8, qword ptr [rip + 0x1860db4] # 0x181bb5a68
0000000180354CB4 lea       rcx, [rsp + 0x28]
0000000180354CB9 call      0x1808197f0
0000000180354CBE movups    xmm0, xmmword ptr [rsp + 0x28]
0000000180354CC3 movups    xmmword ptr [rsp + 0x40], xmm0
0000000180354CC8 movsd     xmm1, qword ptr [rsp + 0x38]
0000000180354CCE movsd     qword ptr [rsp + 0x50], xmm1
0000000180354CD4 mov       qword ptr [rsp + 0x28], rsi
0000000180354CD9 lea       rbx, [rsp + 0x40]
0000000180354CDE mov       qword ptr [rsp + 0x30], rbx
0000000180354CE3 movss     xmm6, dword ptr [rip + 0x1252d25] # 0x1815a7a10
0000000180354CEB nop       dword ptr [rax + rax]
0000000180354CF0 mov       rdx, qword ptr [rip + 0x1872391] # 0x181bc7088
0000000180354CF7 lea       rcx, [rsp + 0x40]
0000000180354CFC call      0x180669280
0000000180354D01 test      al, al
0000000180354D03 je        0x180354daa
0000000180354D09 mov       rdi, qword ptr [rsp + 0x50]
0000000180354D0E test      rdi, rdi
0000000180354D11 je        0x180354ed7
0000000180354D17 xor       edx, edx
0000000180354D19 mov       rcx, rdi
0000000180354D1C call      0x1813044d0
0000000180354D21 test      rax, rax
0000000180354D24 je        0x180354ed2
0000000180354D2A xor       edx, edx
0000000180354D2C mov       rcx, rax
0000000180354D2F call      0x18131bf10
0000000180354D34 test      al, al
0000000180354D36 je        0x180354cf0
0000000180354D38 test      rdi, rdi
0000000180354D3B je        0x180354ecd
0000000180354D41 cmp       dword ptr [rdi + 0x24], 0x15
0000000180354D45 jne       0x180354cf0
0000000180354D47 comiss    xmm6, dword ptr [rdi + 0x3c]
0000000180354D4B ja        0x180354cf0
0000000180354D4D mov       r12, qword ptr [rdi + 0x100]
0000000180354D54 mov       rcx, qword ptr [rip + 0x185b7a5] # 0x181bb0500
0000000180354D5B cmp       dword ptr [rcx + 0xe0], 0
0000000180354D62 jne       0x180354d69
0000000180354D64 call      0x1802501e0
0000000180354D69 xor       r8d, r8d
0000000180354D6C mov       rdx, r13
0000000180354D6F mov       rcx, r12
0000000180354D72 call      0x18131f900
0000000180354D77 test      al, al
0000000180354D79 jne       0x180354cf0
0000000180354D7F inc       r15d
0000000180354D82 mov       dword ptr [rsp + 0xd0], r15d
0000000180354D8A test      r14, r14
0000000180354D8D je        0x180354ec7
0000000180354D93 mov       r8, qword ptr [rip + 0x1860c76] # 0x181bb5a10
0000000180354D9A mov       rdx, rdi
0000000180354D9D mov       rcx, r14
0000000180354DA0 call      0x180002e70
0000000180354DA5 jmp       0x180354cf0
0000000180354DAA mov       rdx, qword ptr [rip + 0x187227f] # 0x181bc7030
0000000180354DB1 mov       rcx, rbx
0000000180354DB4 call      0x180302170
0000000180354DB9 jmp       0x180354df4
0000000180354DBB mov       rdx, qword ptr [rip + 0x187226e] # 0x181bc7030
0000000180354DC2 mov       rcx, qword ptr [rsp + 0x30]
0000000180354DC7 call      0x180302170
0000000180354DCC mov       rcx, qword ptr [rsp + 0x28]
0000000180354DD1 test      rcx, rcx
0000000180354DD4 jne       0x180354edd
0000000180354DDA xor       esi, esi
0000000180354DDC mov       r13, qword ptr [rsp + 0xc0]
0000000180354DE4 mov       r14, qword ptr [rsp + 0xd8]
0000000180354DEC mov       r15d, dword ptr [rsp + 0xd0]
0000000180354DF4 cmp       r15d, 7
0000000180354DF8 jl        0x180354ea4
0000000180354DFE lea       eax, [r15 - 7]
0000000180354E02 movd      xmm6, eax
0000000180354E06 cvtdq2ps  xmm6, xmm6
0000000180354E09 mulss     xmm6, dword ptr [rip + 0x1252bf7] # 0x1815a7a08
0000000180354E11 addss     xmm6, dword ptr [rip + 0x1252dc7] # 0x1815a7be0
0000000180354E19 movd      xmm0, r15d
0000000180354E1E cvtdq2ps  xmm0, xmm0
0000000180354E21 mulss     xmm0, dword ptr [rip + 0x1252bdf] # 0x1815a7a08
0000000180354E29 addss     xmm0, dword ptr [rip + 0x1252c9b] # 0x1815a7acc
0000000180354E31 movss     dword ptr [r13 + 0x210], xmm0
0000000180354E3A movss     xmm7, dword ptr [rip + 0x1252bde] # 0x1815a7a20
0000000180354E42 cmp       esi, 7
0000000180354E45 jge       0x180354e78
0000000180354E47 test      r14, r14
0000000180354E4A je        0x180354ec1
0000000180354E4C mov       r8, qword ptr [rip + 0x1860c6d] # 0x181bb5ac0
0000000180354E53 mov       edx, esi
0000000180354E55 mov       rcx, r14
0000000180354E58 call      0x180825710
0000000180354E5D test      rax, rax
0000000180354E60 je        0x180354ec1
0000000180354E62 movaps    xmm2, xmm6
0000000180354E65 addss     xmm2, xmm7
0000000180354E69 xor       r9d, r9d
0000000180354E6C mov       edx, esi
0000000180354E6E mov       rcx, rax
0000000180354E71 call      0x18037ba70
0000000180354E76 jmp       0x180354e9d
0000000180354E78 test      r14, r14
0000000180354E7B je        0x180354ec1
0000000180354E7D mov       r8, qword ptr [rip + 0x1860c3c] # 0x181bb5ac0
0000000180354E84 mov       edx, esi
0000000180354E86 mov       rcx, r14
0000000180354E89 call      0x180825710
0000000180354E8E test      rax, rax
0000000180354E91 je        0x180354ec1
0000000180354E93 xor       edx, edx
0000000180354E95 mov       rcx, rax
0000000180354E98 call      0x18037b6c0
0000000180354E9D inc       esi
0000000180354E9F cmp       esi, r15d
0000000180354EA2 jl        0x180354e42
0000000180354EA4 movaps    xmm6, xmmword ptr [rsp + 0x70]
0000000180354EA9 movaps    xmm7, xmmword ptr [rsp + 0x60]
0000000180354EAE add       rsp, 0x80
0000000180354EB5 pop       r15
0000000180354EB7 pop       r14
0000000180354EB9 pop       r13
0000000180354EBB pop       r12
0000000180354EBD pop       rdi
0000000180354EBE pop       rsi
0000000180354EBF pop       rbx
0000000180354EC0 ret       
0000000180354EC1 call      0x180250150
0000000180354EC6 nop       
0000000180354EC7 call      0x180250150
0000000180354ECC nop       
0000000180354ECD call      0x180250150
0000000180354ED2 call      0x180250150
0000000180354ED7 call      0x180250150
0000000180354EDC nop       
0000000180354EDD call      0x180200e60
0000000180354EE2 int3      
0000000180354EE3 int3      
0000000180354EE4 int3      
0000000180354EE5 int3      
0000000180354EE6 int3      
0000000180354EE7 int3      
0000000180354EE8 int3      
0000000180354EE9 int3      
0000000180354EEA int3      
0000000180354EEB int3      
0000000180354EEC int3      
0000000180354EED int3      
0000000180354EEE int3      
0000000180354EEF int3      