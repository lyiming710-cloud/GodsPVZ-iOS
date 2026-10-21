0000000180372E10 mov       rax, rsp
0000000180372E13 mov       qword ptr [rax + 8], rbx
0000000180372E17 mov       qword ptr [rax + 0x10], rsi
0000000180372E1B push      rdi
0000000180372E1C sub       rsp, 0xa0
0000000180372E23 movaps    xmmword ptr [rax - 0x18], xmm6
0000000180372E27 movaps    xmmword ptr [rax - 0x28], xmm7
0000000180372E2B movaps    xmmword ptr [rax - 0x38], xmm8
0000000180372E30 movaps    xmmword ptr [rax - 0x48], xmm9
0000000180372E35 mov       rsi, rcx
0000000180372E38 cmp       byte ptr [rip + 0x1971f6c], 0 # 0x181ce4dab
0000000180372E3F jne       0x180372e90
0000000180372E41 lea       rcx, [rip + 0x1857a20] # 0x181bca868
0000000180372E48 call      0x18024fef0
0000000180372E4D lea       rcx, [rip + 0x1857a64] # 0x181bca8b8
0000000180372E54 call      0x18024fef0
0000000180372E59 lea       rcx, [rip + 0x1857ab0] # 0x181bca910
0000000180372E60 call      0x18024fef0
0000000180372E65 lea       rcx, [rip + 0x185087c] # 0x181bc36e8
0000000180372E6C call      0x18024fef0
0000000180372E71 lea       rcx, [rip + 0x1839d80] # 0x181bacbf8
0000000180372E78 call      0x18024fef0
0000000180372E7D lea       rcx, [rip + 0x183d67c] # 0x181bb0500
0000000180372E84 call      0x18024fef0
0000000180372E89 mov       byte ptr [rip + 0x1971f1b], 1 # 0x181ce4dab
0000000180372E90 cmp       byte ptr [rsi + 0xbf], 0
0000000180372E97 jne       0x180373045
0000000180372E9D cmp       byte ptr [rsi + 0xba], 0
0000000180372EA4 jne       0x180373045
0000000180372EAA mov       rax, qword ptr [rsi + 0x20]
0000000180372EAE test      rax, rax
0000000180372EB1 je        0x18037307a
0000000180372EB7 mov       rcx, qword ptr [rax + 0xf0]
0000000180372EBE test      rcx, rcx
0000000180372EC1 je        0x18037307a
0000000180372EC7 mov       rdx, qword ptr [rcx + 0x38]
0000000180372ECB test      rdx, rdx
0000000180372ECE je        0x18037307a
0000000180372ED4 mov       r8, qword ptr [rip + 0x185080d] # 0x181bc36e8
0000000180372EDB lea       rcx, [rsp + 0x28]
0000000180372EE0 call      0x1808197f0
0000000180372EE5 movups    xmm0, xmmword ptr [rsp + 0x28]
0000000180372EEA movups    xmmword ptr [rsp + 0x40], xmm0
0000000180372EEF movsd     xmm1, qword ptr [rsp + 0x38]
0000000180372EF5 movsd     qword ptr [rsp + 0x50], xmm1
0000000180372EFB mov       qword ptr [rsp + 0x28], 0
0000000180372F04 lea       rbx, [rsp + 0x40]
0000000180372F09 mov       qword ptr [rsp + 0x30], rbx
0000000180372F0E movdqa    xmm9, xmmword ptr [rip + 0x1234c69] # 0x1815a7b80
0000000180372F17 xorps     xmm8, xmm8
0000000180372F1B nop       dword ptr [rax + rax]
0000000180372F20 mov       rdx, qword ptr [rip + 0x1857991] # 0x181bca8b8
0000000180372F27 lea       rcx, [rsp + 0x40]
0000000180372F2C call      0x180669280
0000000180372F31 test      al, al
0000000180372F33 je        0x180373019
0000000180372F39 mov       rdi, qword ptr [rsp + 0x50]
0000000180372F3E mov       rcx, qword ptr [rip + 0x183d5bb] # 0x181bb0500
0000000180372F45 cmp       dword ptr [rcx + 0xe0], 0
0000000180372F4C jne       0x180372f53
0000000180372F4E call      0x1802501e0
0000000180372F53 xor       r8d, r8d
0000000180372F56 mov       rdx, rsi
0000000180372F59 mov       rcx, rdi
0000000180372F5C call      0x18131f760
0000000180372F61 test      al, al
0000000180372F63 jne       0x180372f20
0000000180372F65 test      rdi, rdi
0000000180372F68 je        0x18037306e
0000000180372F6E xor       edx, edx
0000000180372F70 mov       rcx, rdi
0000000180372F73 call      0x18035de80
0000000180372F78 test      al, al
0000000180372F7A je        0x180372f20
0000000180372F7C movss     xmm0, dword ptr [rdi + 0xc4]
0000000180372F84 comiss    xmm0, xmm8
0000000180372F88 ja        0x180372f20
0000000180372F8A movss     xmm6, dword ptr [rdi + 0x30]
0000000180372F8F movss     xmm7, dword ptr [rsi + 0x30]
0000000180372F94 mov       rcx, qword ptr [rip + 0x1839c5d] # 0x181bacbf8
0000000180372F9B cmp       dword ptr [rcx + 0xe0], 0
0000000180372FA2 jne       0x180372fb0
0000000180372FA4 call      0x1802501e0
0000000180372FA9 mov       rcx, qword ptr [rip + 0x1839c48] # 0x181bacbf8
0000000180372FB0 subss     xmm6, xmm7
0000000180372FB4 andps     xmm6, xmm9
0000000180372FB8 movss     xmm0, dword ptr [rsi + 0x40]
0000000180372FBD addss     xmm6, xmm6
0000000180372FC1 addss     xmm0, dword ptr [rdi + 0x40]
0000000180372FC6 comiss    xmm0, xmm6
0000000180372FC9 jbe       0x180372f20
0000000180372FCF movss     xmm6, dword ptr [rdi + 0x34]
0000000180372FD4 movss     xmm7, dword ptr [rsi + 0x34]
0000000180372FD9 cmp       dword ptr [rcx + 0xe0], 0
0000000180372FE0 jne       0x180372fe7
0000000180372FE2 call      0x1802501e0
0000000180372FE7 subss     xmm6, xmm7
0000000180372FEB andps     xmm6, xmm9
0000000180372FEF movss     xmm0, dword ptr [rsi + 0x44]
0000000180372FF4 addss     xmm6, xmm6
0000000180372FF8 addss     xmm0, dword ptr [rdi + 0x44]
0000000180372FFD comiss    xmm0, xmm6
0000000180373000 jbe       0x180372f20
0000000180373006 xor       r8d, r8d
0000000180373009 mov       rdx, rdi
000000018037300C mov       rcx, rsi
000000018037300F call      0x180373080
0000000180373014 jmp       0x180372f20
0000000180373019 mov       rdx, qword ptr [rip + 0x1857848] # 0x181bca868
0000000180373020 mov       rcx, rbx
0000000180373023 call      0x180302170
0000000180373028 jmp       0x180373045
000000018037302A mov       rdx, qword ptr [rip + 0x1857837] # 0x181bca868
0000000180373031 mov       rcx, qword ptr [rsp + 0x30]
0000000180373036 call      0x180302170
000000018037303B mov       rcx, qword ptr [rsp + 0x28]
0000000180373040 test      rcx, rcx
0000000180373043 jne       0x180373074
0000000180373045 lea       r11, [rsp + 0xa0]
000000018037304D mov       rbx, qword ptr [r11 + 0x10]
0000000180373051 mov       rsi, qword ptr [r11 + 0x18]
0000000180373055 movaps    xmm6, xmmword ptr [r11 - 0x10]
000000018037305A movaps    xmm7, xmmword ptr [r11 - 0x20]
000000018037305F movaps    xmm8, xmmword ptr [r11 - 0x30]
0000000180373064 movaps    xmm9, xmmword ptr [r11 - 0x40]
0000000180373069 mov       rsp, r11
000000018037306C pop       rdi
000000018037306D ret       
000000018037306E call      0x180250150
0000000180373073 nop       
0000000180373074 call      0x180200e60
0000000180373079 int3      
000000018037307A call      0x180250150
000000018037307F int3      