0000000180341C80 mov       qword ptr [rsp + 0x18], rbx
0000000180341C85 mov       dword ptr [rsp + 0x10], edx
0000000180341C89 push      rsi
0000000180341C8A push      rdi
0000000180341C8B push      r14
0000000180341C8D sub       rsp, 0x70
0000000180341C91 movaps    xmmword ptr [rsp + 0x60], xmm6
0000000180341C96 mov       rsi, rcx
0000000180341C99 cmp       byte ptr [rip + 0x19a2fb6], 0 # 0x181ce4c56
0000000180341CA0 jne       0x180341cf1
0000000180341CA2 lea       rcx, [rip + 0x186026f] # 0x181ba1f18
0000000180341CA9 call      0x18024fef0
0000000180341CAE lea       rcx, [rip + 0x187f8c3] # 0x181bc1578
0000000180341CB5 call      0x18024fef0
0000000180341CBA lea       rcx, [rip + 0x187f90f] # 0x181bc15d0
0000000180341CC1 call      0x18024fef0
0000000180341CC6 lea       rcx, [rip + 0x187f95b] # 0x181bc1628
0000000180341CCD call      0x18024fef0
0000000180341CD2 lea       rcx, [rip + 0x186307f] # 0x181ba4d58
0000000180341CD9 call      0x18024fef0
0000000180341CDE lea       rcx, [rip + 0x186e81b] # 0x181bb0500
0000000180341CE5 call      0x18024fef0
0000000180341CEA mov       byte ptr [rip + 0x19a2f65], 1 # 0x181ce4c56
0000000180341CF1 mov       rbx, qword ptr [rsi + 0x58]
0000000180341CF5 mov       rcx, qword ptr [rip + 0x186021c] # 0x181ba1f18
0000000180341CFC call      0x180250100
0000000180341D01 mov       rdi, rax
0000000180341D04 mov       dword ptr [rax + 0x10], 0x3f800000
0000000180341D0B mov       dword ptr [rax + 0x14], 0x3f800000
0000000180341D12 mov       dword ptr [rax + 0x18], 0x3f800000
0000000180341D19 xor       edx, edx
0000000180341D1B mov       rcx, rax
0000000180341D1E call      0x180302170
0000000180341D23 test      rbx, rbx
0000000180341D26 je        0x180341f29
0000000180341D2C movss     xmm0, dword ptr [rbx + 0x10]
0000000180341D31 movss     dword ptr [rdi + 0x10], xmm0
0000000180341D36 movss     xmm1, dword ptr [rbx + 0x14]
0000000180341D3B movss     dword ptr [rdi + 0x14], xmm1
0000000180341D40 movss     xmm0, dword ptr [rbx + 0x18]
0000000180341D45 movss     dword ptr [rdi + 0x18], xmm0
0000000180341D4A movss     xmm1, dword ptr [rbx + 0x1c]
0000000180341D4F movss     dword ptr [rdi + 0x1c], xmm1
0000000180341D54 movss     xmm0, dword ptr [rbx + 0x20]
0000000180341D59 movss     dword ptr [rdi + 0x20], xmm0
0000000180341D5E mov       qword ptr [rsp + 0x90], rdi
0000000180341D66 mov       rbx, qword ptr [rsi + 0x20]
0000000180341D6A mov       rcx, qword ptr [rip + 0x186e78f] # 0x181bb0500
0000000180341D71 cmp       dword ptr [rcx + 0xe0], 0
0000000180341D78 jne       0x180341d7f
0000000180341D7A call      0x1802501e0
0000000180341D7F xor       r8d, r8d
0000000180341D82 xor       edx, edx
0000000180341D84 mov       rcx, rbx
0000000180341D87 call      0x18131f760
0000000180341D8C test      al, al
0000000180341D8E jne       0x180341f10
0000000180341D94 mov       rdx, qword ptr [rsi + 0x50]
0000000180341D98 test      rdx, rdx
0000000180341D9B je        0x180341f29
0000000180341DA1 mov       r8, qword ptr [rip + 0x1862fb0] # 0x181ba4d58
0000000180341DA8 lea       rcx, [rsp + 0x28]
0000000180341DAD call      0x1808197f0
0000000180341DB2 movups    xmm0, xmmword ptr [rsp + 0x28]
0000000180341DB7 movups    xmmword ptr [rsp + 0x40], xmm0
0000000180341DBC movsd     xmm1, qword ptr [rsp + 0x38]
0000000180341DC2 movsd     qword ptr [rsp + 0x50], xmm1
0000000180341DC8 mov       qword ptr [rsp + 0x28], 0
0000000180341DD1 lea       rbx, [rsp + 0x40]
0000000180341DD6 mov       qword ptr [rsp + 0x30], rbx
0000000180341DDB nop       dword ptr [rax + rax]
0000000180341DE0 mov       rdx, qword ptr [rip + 0x187f7e9] # 0x181bc15d0
0000000180341DE7 lea       rcx, [rsp + 0x40]
0000000180341DEC call      0x180669280
0000000180341DF1 test      al, al
0000000180341DF3 je        0x180341edc
0000000180341DF9 mov       rsi, qword ptr [rsp + 0x50]
0000000180341DFE test      rsi, rsi
0000000180341E01 je        0x180341f44
0000000180341E07 cmp       byte ptr [rsi + 0x18], 0
0000000180341E0B jne       0x180341e13
0000000180341E0D cmp       byte ptr [rsi + 0x19], 0
0000000180341E11 je        0x180341de0
0000000180341E13 mov       r14, qword ptr [rsi + 0x20]
0000000180341E17 xor       edx, edx
0000000180341E19 lea       rcx, [rsp + 0x98]
0000000180341E21 call      0x180ca3820
0000000180341E26 xor       r8d, r8d
0000000180341E29 mov       rdx, rax
0000000180341E2C mov       rcx, r14
0000000180341E2F call      0x180b7cc30
0000000180341E34 test      al, al
0000000180341E36 jne       0x180341de0
0000000180341E38 test      rsi, rsi
0000000180341E3B je        0x180341f3f
0000000180341E41 mov       eax, dword ptr [rsi + 0x10]
0000000180341E44 test      eax, eax
0000000180341E46 jne       0x180341e86
0000000180341E48 test      rdi, rdi
0000000180341E4B je        0x180341f2f
0000000180341E51 movss     xmm6, dword ptr [rdi + 0x10]
0000000180341E56 xor       edx, edx
0000000180341E58 mov       rcx, rsi
0000000180341E5B call      0x180395070
0000000180341E60 addss     xmm0, xmm6
0000000180341E64 movss     dword ptr [rdi + 0x10], xmm0
0000000180341E69 movss     xmm6, dword ptr [rdi + 0x14]
0000000180341E6E xor       edx, edx
0000000180341E70 mov       rcx, rsi
0000000180341E73 call      0x180395070
0000000180341E78 addss     xmm0, xmm6
0000000180341E7C movss     dword ptr [rdi + 0x14], xmm0
0000000180341E81 jmp       0x180341de0
0000000180341E86 cmp       eax, 6
0000000180341E89 jne       0x180341eb1
0000000180341E8B test      rdi, rdi
0000000180341E8E je        0x180341f35
0000000180341E94 movss     xmm6, dword ptr [rdi + 0x1c]
0000000180341E99 xor       edx, edx
0000000180341E9B mov       rcx, rsi
0000000180341E9E call      0x180395070
0000000180341EA3 addss     xmm0, xmm6
0000000180341EA7 movss     dword ptr [rdi + 0x1c], xmm0
0000000180341EAC jmp       0x180341de0
0000000180341EB1 cmp       eax, 0x20
0000000180341EB4 jne       0x180341de0
0000000180341EBA test      rdi, rdi
0000000180341EBD je        0x180341f3a
0000000180341EBF movss     xmm6, dword ptr [rdi + 0x20]
0000000180341EC4 xor       edx, edx
0000000180341EC6 mov       rcx, rsi
0000000180341EC9 call      0x180395070
0000000180341ECE addss     xmm0, xmm6
0000000180341ED2 movss     dword ptr [rdi + 0x20], xmm0
0000000180341ED7 jmp       0x180341de0
0000000180341EDC mov       rdx, qword ptr [rip + 0x187f695] # 0x181bc1578
0000000180341EE3 mov       rcx, rbx
0000000180341EE6 call      0x180302170
0000000180341EEB jmp       0x180341f10
0000000180341EED mov       rdx, qword ptr [rip + 0x187f684] # 0x181bc1578
0000000180341EF4 mov       rcx, qword ptr [rsp + 0x30]
0000000180341EF9 call      0x180302170
0000000180341EFE mov       rcx, qword ptr [rsp + 0x28]
0000000180341F03 test      rcx, rcx
0000000180341F06 jne       0x180341f4a
0000000180341F08 mov       rdi, qword ptr [rsp + 0x90]
0000000180341F10 mov       rax, rdi
0000000180341F13 mov       rbx, qword ptr [rsp + 0xa0]
0000000180341F1B movaps    xmm6, xmmword ptr [rsp + 0x60]
0000000180341F20 add       rsp, 0x70
0000000180341F24 pop       r14
0000000180341F26 pop       rdi
0000000180341F27 pop       rsi
0000000180341F28 ret       
0000000180341F29 call      0x180250150
0000000180341F2E nop       
0000000180341F2F call      0x180250150
0000000180341F34 nop       
0000000180341F35 call      0x180250150
0000000180341F3A call      0x180250150
0000000180341F3F call      0x180250150
0000000180341F44 call      0x180250150
0000000180341F49 nop       
0000000180341F4A call      0x180200e60
0000000180341F4F int3      