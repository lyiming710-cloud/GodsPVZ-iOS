0000000180304CB0 mov       qword ptr [rsp + 8], rbx
0000000180304CB5 mov       qword ptr [rsp + 0x10], rsi
0000000180304CBA mov       qword ptr [rsp + 0x18], rdi
0000000180304CBF push      r14
0000000180304CC1 sub       rsp, 0x60
0000000180304CC5 mov       rbx, rdx
0000000180304CC8 mov       rsi, rcx
0000000180304CCB cmp       byte ptr [rip + 0x19dfdf6], 0 # 0x181ce4ac8
0000000180304CD2 jne       0x180304d2f
0000000180304CD4 lea       rcx, [rip + 0x18be25d] # 0x181bc2f38
0000000180304CDB call      0x18024fef0
0000000180304CE0 lea       rcx, [rip + 0x18be2a9] # 0x181bc2f90
0000000180304CE7 call      0x18024fef0
0000000180304CEC lea       rcx, [rip + 0x18be2f5] # 0x181bc2fe8
0000000180304CF3 call      0x18024fef0
0000000180304CF8 lea       rcx, [rip + 0x18af041] # 0x181bb3d40
0000000180304CFF call      0x18024fef0
0000000180304D04 lea       rcx, [rip + 0x18a57a5] # 0x181baa4b0
0000000180304D0B call      0x18024fef0
0000000180304D10 lea       rcx, [rip + 0x18b30a1] # 0x181bb7db8
0000000180304D17 call      0x18024fef0
0000000180304D1C lea       rcx, [rip + 0x18ab7dd] # 0x181bb0500
0000000180304D23 call      0x18024fef0
0000000180304D28 mov       byte ptr [rip + 0x19dfd99], 1 # 0x181ce4ac8
0000000180304D2F mov       qword ptr [rsi + 0x38], rbx
0000000180304D33 lea       rcx, [rsi + 0x38]
0000000180304D37 mov       rdx, rbx
0000000180304D3A call      0x18024f360
0000000180304D3F xor       edx, edx
0000000180304D41 mov       rcx, rsi
0000000180304D44 call      0x180304f60
0000000180304D49 test      rbx, rbx
0000000180304D4C je        0x180304f57
0000000180304D52 mov       rdx, qword ptr [rbx + 0x30]
0000000180304D56 test      rdx, rdx
0000000180304D59 je        0x180304f57
0000000180304D5F mov       r8, qword ptr [rip + 0x18a574a] # 0x181baa4b0
0000000180304D66 lea       rcx, [rsp + 0x28]
0000000180304D6B call      0x1808197f0
0000000180304D70 movups    xmm0, xmmword ptr [rsp + 0x28]
0000000180304D75 movups    xmmword ptr [rsp + 0x40], xmm0
0000000180304D7A movsd     xmm1, qword ptr [rsp + 0x38]
0000000180304D80 movsd     qword ptr [rsp + 0x50], xmm1
0000000180304D86 mov       qword ptr [rsp + 0x28], 0
0000000180304D8F lea       rbx, [rsp + 0x40]
0000000180304D94 mov       qword ptr [rsp + 0x30], rbx
0000000180304D99 nop       dword ptr [rax]
0000000180304DA0 mov       rdx, qword ptr [rip + 0x18be1e9] # 0x181bc2f90
0000000180304DA7 lea       rcx, [rsp + 0x40]
0000000180304DAC call      0x180669280
0000000180304DB1 test      al, al
0000000180304DB3 je        0x180304ee7
0000000180304DB9 mov       r14, qword ptr [rsp + 0x50]
0000000180304DBE mov       rax, qword ptr [rsi + 0x30]
0000000180304DC2 test      rax, rax
0000000180304DC5 je        0x180304f4b
0000000180304DCB mov       rdi, qword ptr [rax + 0xb0]
0000000180304DD2 mov       rcx, qword ptr [rip + 0x18ab727] # 0x181bb0500
0000000180304DD9 cmp       dword ptr [rcx + 0xe0], 0
0000000180304DE0 jne       0x180304de7
0000000180304DE2 call      0x1802501e0
0000000180304DE7 mov       rdx, qword ptr [rip + 0x18b2fca] # 0x181bb7db8
0000000180304DEE mov       rcx, rdi
0000000180304DF1 call      0x1804a0690
0000000180304DF6 mov       rdi, rax
0000000180304DF9 test      rax, rax
0000000180304DFC je        0x180304f46
0000000180304E02 mov       qword ptr [rax + 0x38], rsi
0000000180304E06 lea       rcx, [rax + 0x38]
0000000180304E0A mov       rdx, rsi
0000000180304E0D call      0x18024f360
0000000180304E12 mov       qword ptr [rdi + 0x40], r14
0000000180304E16 lea       rcx, [rdi + 0x40]
0000000180304E1A mov       rdx, r14
0000000180304E1D call      0x18024f360
0000000180304E22 xor       edx, edx
0000000180304E24 mov       rcx, rdi
0000000180304E27 call      0x180306260
0000000180304E2C xor       edx, edx
0000000180304E2E mov       rcx, rdi
0000000180304E31 call      0x181304510
0000000180304E36 mov       r14, rax
0000000180304E39 xor       edx, edx
0000000180304E3B mov       rcx, rsi
0000000180304E3E call      0x181304510
0000000180304E43 test      r14, r14
0000000180304E46 je        0x180304f41
0000000180304E4C xor       r9d, r9d
0000000180304E4F mov       r8b, 1
0000000180304E52 mov       rdx, rax
0000000180304E55 mov       rcx, r14
0000000180304E58 call      0x18132e900
0000000180304E5D mov       rcx, qword ptr [rsi + 0x50]
0000000180304E61 test      rcx, rcx
0000000180304E64 je        0x180304f3c
0000000180304E6A mov       r9, qword ptr [rip + 0x18aeecf] # 0x181bb3d40
0000000180304E71 inc       dword ptr [rcx + 0x1c]
0000000180304E74 mov       rdx, qword ptr [rcx + 0x10]
0000000180304E78 movsxd    r8, dword ptr [rcx + 0x18]
0000000180304E7C test      rdx, rdx
0000000180304E7F je        0x180304f37
0000000180304E85 cmp       r8d, dword ptr [rdx + 0x18]
0000000180304E89 jb        0x180304ea4
0000000180304E8B mov       rax, qword ptr [r9 + 0x20]
0000000180304E8F mov       r8, qword ptr [rax + 0xc0]
0000000180304E96 mov       r8, qword ptr [r8 + 0x70]
0000000180304E9A mov       rdx, rdi
0000000180304E9D call      0x180847ce0
0000000180304EA2 jmp       0x180304ec6
0000000180304EA4 lea       eax, [r8 + 1]
0000000180304EA8 mov       dword ptr [rcx + 0x18], eax
0000000180304EAB cmp       r8d, dword ptr [rdx + 0x18]
0000000180304EAF jae       0x180304f2c
0000000180304EB1 mov       qword ptr [rdx + r8*8 + 0x20], rdi
0000000180304EB6 add       rdx, 0x20
0000000180304EBA lea       rcx, [rdx + r8*8]
0000000180304EBE mov       rdx, rdi
0000000180304EC1 call      0x18024f360
0000000180304EC6 xor       edx, edx
0000000180304EC8 mov       rcx, rdi
0000000180304ECB call      0x1813044d0
0000000180304ED0 test      rax, rax
0000000180304ED3 je        0x180304f32
0000000180304ED5 xor       r8d, r8d
0000000180304ED8 xor       edx, edx
0000000180304EDA mov       rcx, rax
0000000180304EDD call      0x18131bad0
0000000180304EE2 jmp       0x180304da0
0000000180304EE7 mov       rdx, qword ptr [rip + 0x18be04a] # 0x181bc2f38
0000000180304EEE mov       rcx, rbx
0000000180304EF1 call      0x180302170
0000000180304EF6 jmp       0x180304f13
0000000180304EF8 mov       rdx, qword ptr [rip + 0x18be039] # 0x181bc2f38
0000000180304EFF mov       rcx, qword ptr [rsp + 0x30]
0000000180304F04 call      0x180302170
0000000180304F09 mov       rcx, qword ptr [rsp + 0x28]
0000000180304F0E test      rcx, rcx
0000000180304F11 jne       0x180304f51
0000000180304F13 mov       rbx, qword ptr [rsp + 0x70]
0000000180304F18 mov       rsi, qword ptr [rsp + 0x78]
0000000180304F1D mov       rdi, qword ptr [rsp + 0x80]
0000000180304F25 add       rsp, 0x60
0000000180304F29 pop       r14
0000000180304F2B ret       
0000000180304F2C call      0x180250140
0000000180304F31 nop       
0000000180304F32 call      0x180250150
0000000180304F37 call      0x180250150
0000000180304F3C call      0x180250150
0000000180304F41 call      0x180250150
0000000180304F46 call      0x180250150
0000000180304F4B call      0x180250150
0000000180304F50 nop       
0000000180304F51 call      0x180200e60
0000000180304F56 int3      
0000000180304F57 call      0x180250150
0000000180304F5C int3      
0000000180304F5D int3      
0000000180304F5E int3      
0000000180304F5F int3      