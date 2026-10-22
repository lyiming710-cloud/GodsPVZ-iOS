0000000180357C50 push      rbp
0000000180357C52 push      rbx
0000000180357C53 push      r12
0000000180357C55 lea       rbp, [rsp - 0x47]
0000000180357C5A sub       rsp, 0xc0
0000000180357C61 cmp       byte ptr [rip + 0x198d0cc], 0 # 0x181ce4d34
0000000180357C68 mov       rbx, rcx
0000000180357C6B jne       0x180357cb0
0000000180357C6D lea       rcx, [rip + 0x184a3a4] # 0x181ba2018
0000000180357C74 call      0x18024fef0
0000000180357C79 lea       rcx, [rip + 0x1858880] # 0x181bb0500
0000000180357C80 call      0x18024fef0
0000000180357C85 lea       rcx, [rip + 0x1863b3c] # 0x181bbb7c8
0000000180357C8C call      0x18024fef0
0000000180357C91 lea       rcx, [rip + 0x1852098] # 0x181ba9d30
0000000180357C98 call      0x18024fef0
0000000180357C9D lea       rcx, [rip + 0x1852034] # 0x181ba9cd8
0000000180357CA4 call      0x18024fef0
0000000180357CA9 mov       byte ptr [rip + 0x198d084], 1 # 0x181ce4d34
0000000180357CB0 mov       qword ptr [rsp + 0xe0], rsi
0000000180357CB8 xor       r12d, r12d
0000000180357CBB mov       qword ptr [rsp + 0xe8], rdi
0000000180357CC3 mov       qword ptr [rsp + 0xf0], r13
0000000180357CCB mov       qword ptr [rsp + 0xb8], r14
0000000180357CD3 mov       qword ptr [rsp + 0xb0], r15
0000000180357CDB movaps    xmmword ptr [rsp + 0xa0], xmm6
0000000180357CE3 movaps    xmmword ptr [rsp + 0x90], xmm7
0000000180357CEB movaps    xmmword ptr [rsp + 0x80], xmm8
0000000180357CF4 movaps    xmmword ptr [rsp + 0x70], xmm9
0000000180357CFA cmp       byte ptr [rbx + 0xe8], r12b
0000000180357D01 jne       0x180358036
0000000180357D07 mov       rcx, qword ptr [rip + 0x184a30a] # 0x181ba2018
0000000180357D0E mov       rdi, qword ptr [rbx + 0x148]
0000000180357D15 cmp       dword ptr [rcx + 0xe0], r12d
0000000180357D1C jne       0x180357d23
0000000180357D1E call      0x1802501e0
0000000180357D23 mov       r8, qword ptr [rip + 0x1851fae] # 0x181ba9cd8
0000000180357D2A lea       rcx, [rbp - 0x19]
0000000180357D2E xor       r9d, r9d
0000000180357D31 mov       rdx, rdi
0000000180357D34 call      0x18031b680
0000000180357D39 mov       r8, qword ptr [rip + 0x1851ff0] # 0x181ba9d30
0000000180357D40 lea       rcx, [rbp - 0x19]
0000000180357D44 mov       rdx, qword ptr [rbx + 0x148]
0000000180357D4B xor       r9d, r9d
0000000180357D4E movsd     xmm7, qword ptr [rax]
0000000180357D52 mov       r15d, dword ptr [rax + 8]
0000000180357D56 call      0x18031b680
0000000180357D5B mov       rcx, qword ptr [rbx + 0x1e8]
0000000180357D62 movsd     xmm9, qword ptr [rax]
0000000180357D67 mov       r13d, dword ptr [rax + 8]
0000000180357D6B test      rcx, rcx
0000000180357D6E je        0x1803580a7
0000000180357D74 mov       rcx, qword ptr [rcx + 0xf8]
0000000180357D7B test      rcx, rcx
0000000180357D7E je        0x1803580a7
0000000180357D84 xor       r8d, r8d
0000000180357D87 mov       edx, 0x18
0000000180357D8C call      0x180324150
0000000180357D91 mov       rcx, qword ptr [rbx + 0x1e8]
0000000180357D98 mov       rsi, rax
0000000180357D9B test      rcx, rcx
0000000180357D9E je        0x1803580a7
0000000180357DA4 mov       rcx, qword ptr [rcx + 0xf8]
0000000180357DAB test      rcx, rcx
0000000180357DAE je        0x1803580a7
0000000180357DB4 xor       r8d, r8d
0000000180357DB7 mov       edx, 0x18
0000000180357DBC call      0x180324150
0000000180357DC1 xor       r9d, r9d
0000000180357DC4 mov       qword ptr [rsp + 0x20], r12
0000000180357DC9 mov       r8d, 0x18
0000000180357DCF mov       rdx, rsi
0000000180357DD2 mov       rcx, rbx
0000000180357DD5 mov       r14, rax
0000000180357DD8 call      0x180350e40
0000000180357DDD xor       r9d, r9d
0000000180357DE0 mov       qword ptr [rsp + 0x20], r12
0000000180357DE5 mov       r8d, 0x18
0000000180357DEB mov       rdx, r14
0000000180357DEE mov       rcx, rbx
0000000180357DF1 mov       rdi, rax
0000000180357DF4 call      0x180350e40
0000000180357DF9 cmp       byte ptr [rip + 0x198cd36], 0 # 0x181ce4b36
0000000180357E00 mov       r12, rax
0000000180357E03 movss     xmm6, dword ptr [rip + 0x124fea1] # 0x1815a7cac
0000000180357E0B xorps     xmm8, xmm8
0000000180357E0F jne       0x180357e24
0000000180357E11 lea       rcx, [rip + 0x186a9d0] # 0x181bc27e8
0000000180357E18 call      0x18024fef0
0000000180357E1D mov       byte ptr [rip + 0x198cd12], 1 # 0x181ce4b36
0000000180357E24 mov       rax, qword ptr [rip + 0x186a9bd] # 0x181bc27e8
0000000180357E2B mov       rax, qword ptr [rax + 0xb8]
0000000180357E32 test      rsi, rsi
0000000180357E35 je        0x1803580a7
0000000180357E3B movsd     xmm0, qword ptr [rax]
0000000180357E3F lea       r9, [rbp - 0x39]
0000000180357E43 mov       eax, dword ptr [rax + 8]
0000000180357E46 lea       r8, [rbp - 0x29]
0000000180357E4A movsd     qword ptr [rbp - 0x39], xmm0
0000000180357E4F lea       rdx, [rbp - 0x19]
0000000180357E53 mov       dword ptr [rbp - 0x31], eax
0000000180357E56 movaps    xmm0, xmm6
0000000180357E59 mov       rax, qword ptr [rip + 0x1863968] # 0x181bbb7c8
0000000180357E60 mov       rcx, rsi
0000000180357E63 mov       qword ptr [rsp + 0x38], rax
0000000180357E68 mov       dword ptr [rbp - 0x11], r15d
0000000180357E6C xor       r15d, r15d
0000000180357E6F movsd     qword ptr [rbp - 0x19], xmm7
0000000180357E74 movss     xmm7, dword ptr [rip + 0x124fdec] # 0x1815a7c68
0000000180357E7C mov       qword ptr [rsp + 0x30], rbx
0000000180357E81 unpcklps  xmm0, xmm8
0000000180357E85 mov       dword ptr [rsp + 0x28], r15d
0000000180357E8A movss     dword ptr [rsp + 0x20], xmm7
0000000180357E90 movss     dword ptr [rbp - 0x21], xmm8
0000000180357E96 movsd     qword ptr [rbp - 0x29], xmm0
0000000180357E9B call      0x1804a25f0
0000000180357EA0 mov       r8d, dword ptr [rbx + 0xbc]
0000000180357EA7 xor       r9d, r9d
0000000180357EAA mov       rdx, rdi
0000000180357EAD mov       rcx, rsi
0000000180357EB0 call      0x18037c0f0
0000000180357EB5 cmp       byte ptr [rip + 0x198cc7a], r15b # 0x181ce4b36
0000000180357EBC jne       0x180357ed1
0000000180357EBE lea       rcx, [rip + 0x186a923] # 0x181bc27e8
0000000180357EC5 call      0x18024fef0
0000000180357ECA mov       byte ptr [rip + 0x198cc65], 1 # 0x181ce4b36
0000000180357ED1 mov       rax, qword ptr [rip + 0x186a910] # 0x181bc27e8
0000000180357ED8 mov       rax, qword ptr [rax + 0xb8]
0000000180357EDF test      r14, r14
0000000180357EE2 je        0x1803580a7
0000000180357EE8 movsd     xmm0, qword ptr [rax]
0000000180357EEC lea       r9, [rbp - 0x19]
0000000180357EF0 mov       eax, dword ptr [rax + 8]
0000000180357EF3 lea       r8, [rbp - 0x29]
0000000180357EF7 movsd     qword ptr [rbp - 0x19], xmm0
0000000180357EFC lea       rdx, [rbp - 0x39]
0000000180357F00 mov       dword ptr [rbp - 0x11], eax
0000000180357F03 movaps    xmm0, xmm6
0000000180357F06 mov       rax, qword ptr [rip + 0x18638bb] # 0x181bbb7c8
0000000180357F0D mov       rcx, r14
0000000180357F10 mov       qword ptr [rsp + 0x38], rax
0000000180357F15 mov       qword ptr [rsp + 0x30], rbx
0000000180357F1A unpcklps  xmm0, xmm8
0000000180357F1E mov       dword ptr [rsp + 0x28], r15d
0000000180357F23 movss     dword ptr [rsp + 0x20], xmm7
0000000180357F29 movss     dword ptr [rbp - 0x21], xmm8
0000000180357F2F movsd     qword ptr [rbp - 0x29], xmm0
0000000180357F34 movsd     qword ptr [rbp - 0x39], xmm9
0000000180357F3A mov       dword ptr [rbp - 0x31], r13d
0000000180357F3E call      0x1804a25f0
0000000180357F43 mov       r8d, dword ptr [rbx + 0xbc]
0000000180357F4A xor       r9d, r9d
0000000180357F4D mov       rdx, r12
0000000180357F50 mov       rcx, r14
0000000180357F53 call      0x18037c0f0
0000000180357F58 xor       edx, edx
0000000180357F5A mov       rcx, rbx
0000000180357F5D call      0x18034f750
0000000180357F62 mov       rcx, qword ptr [rip + 0x1858597] # 0x181bb0500
0000000180357F69 mov       rdi, rax
0000000180357F6C cmp       dword ptr [rcx + 0xe0], r15d
0000000180357F73 jne       0x180357f7a
0000000180357F75 call      0x1802501e0
0000000180357F7A xor       edx, edx
0000000180357F7C mov       rcx, rdi
0000000180357F7F call      0x18131f870
0000000180357F84 test      al, al
0000000180357F86 jne       0x180357fd4
0000000180357F88 xor       edx, edx
0000000180357F8A mov       rcx, rbx
0000000180357F8D call      0x18034eea0
0000000180357F92 mov       rcx, qword ptr [rip + 0x1858567] # 0x181bb0500
0000000180357F99 mov       rdi, rax
0000000180357F9C cmp       dword ptr [rcx + 0xe0], r15d
0000000180357FA3 jne       0x180357faa
0000000180357FA5 call      0x1802501e0
0000000180357FAA xor       r8d, r8d
0000000180357FAD xor       edx, edx
0000000180357FAF mov       rcx, rdi
0000000180357FB2 call      0x18131f760
0000000180357FB7 test      al, al
0000000180357FB9 jne       0x180358054
0000000180357FBF test      rdi, rdi
0000000180357FC2 je        0x1803580a7
0000000180357FC8 movss     xmm6, dword ptr [rdi + 0x38]
0000000180357FCD movss     xmm7, dword ptr [rdi + 0x3c]
0000000180357FD2 jmp       0x180357fe7
0000000180357FD4 test      rdi, rdi
0000000180357FD7 je        0x1803580a7
0000000180357FDD movss     xmm6, dword ptr [rdi + 0x30]
0000000180357FE2 movss     xmm7, dword ptr [rdi + 0x34]
0000000180357FE7 subss     xmm6, dword ptr [rsi + 0x44]
0000000180357FEC subss     xmm7, dword ptr [rsi + 0x48]
0000000180357FF1 xor       r8d, r8d
0000000180357FF4 movss     dword ptr [rbp - 0x11], xmm8
0000000180357FFA lea       rdx, [rbp - 0x19]
0000000180357FFE mov       rcx, rsi
0000000180358001 movaps    xmm0, xmm6
0000000180358004 unpcklps  xmm0, xmm7
0000000180358007 movsd     qword ptr [rbp - 0x19], xmm0
000000018035800C call      0x180378d50
0000000180358011 movaps    xmm0, xmm6
0000000180358014 movss     dword ptr [rbp - 0x11], xmm8
000000018035801A unpcklps  xmm0, xmm7
000000018035801D lea       rdx, [rbp - 0x19]
0000000180358021 xor       r8d, r8d
0000000180358024 movsd     qword ptr [rbp - 0x19], xmm0
0000000180358029 mov       rcx, r14
000000018035802C call      0x180378d50
0000000180358031 xor       r12d, r12d
0000000180358034 jmp       0x18035804d
0000000180358036 mov       rcx, qword ptr [rbx + 0xe0]
000000018035803D test      rcx, rcx
0000000180358040 je        0x1803580a7
0000000180358042 xor       r8d, r8d
0000000180358045 mov       rdx, rbx
0000000180358048 call      0x180386550
000000018035804D mov       dword ptr [rbx + 0x90], r12d
0000000180358054 movaps    xmm9, xmmword ptr [rsp + 0x70]
000000018035805A movaps    xmm8, xmmword ptr [rsp + 0x80]
0000000180358063 movaps    xmm7, xmmword ptr [rsp + 0x90]
000000018035806B movaps    xmm6, xmmword ptr [rsp + 0xa0]
0000000180358073 mov       r15, qword ptr [rsp + 0xb0]
000000018035807B mov       r14, qword ptr [rsp + 0xb8]
0000000180358083 mov       r13, qword ptr [rsp + 0xf0]
000000018035808B mov       rdi, qword ptr [rsp + 0xe8]
0000000180358093 mov       rsi, qword ptr [rsp + 0xe0]
000000018035809B add       rsp, 0xc0
00000001803580A2 pop       r12
00000001803580A4 pop       rbx
00000001803580A5 pop       rbp
00000001803580A6 ret       
00000001803580A7 call      0x180250150
00000001803580AC int3      
00000001803580AD int3      
00000001803580AE int3      
00000001803580AF int3      