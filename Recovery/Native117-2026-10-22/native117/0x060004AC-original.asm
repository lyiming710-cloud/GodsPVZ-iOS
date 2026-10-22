0000000180373BB0 push      rbx
0000000180373BB2 push      rsi
0000000180373BB3 push      rdi
0000000180373BB4 push      r12
0000000180373BB6 push      r13
0000000180373BB8 push      r14
0000000180373BBA push      r15
0000000180373BBC sub       rsp, 0x80
0000000180373BC3 movaps    xmmword ptr [rsp + 0x70], xmm6
0000000180373BC8 mov       r14, rcx
0000000180373BCB cmp       byte ptr [rip + 0x19711dd], 0 # 0x181ce4daf
0000000180373BD2 jne       0x180373c6f
0000000180373BD8 lea       rcx, [rip + 0x1827169] # 0x181b9ad48
0000000180373BDF call      0x18024fef0
0000000180373BE4 lea       rcx, [rip + 0x182ac3d] # 0x181b9e828
0000000180373BEB call      0x18024fef0
0000000180373BF0 lea       rcx, [rip + 0x184eb09] # 0x181bc2700
0000000180373BF7 call      0x18024fef0
0000000180373BFC lea       rcx, [rip + 0x184eb55] # 0x181bc2758
0000000180373C03 call      0x18024fef0
0000000180373C08 lea       rcx, [rip + 0x184eba1] # 0x181bc27b0
0000000180373C0F call      0x18024fef0
0000000180373C14 lea       rcx, [rip + 0x18354a5] # 0x181ba90c0
0000000180373C1B call      0x18024fef0
0000000180373C20 lea       rcx, [rip + 0x18368e1] # 0x181baa508
0000000180373C27 call      0x18024fef0
0000000180373C2C lea       rcx, [rip + 0x1836985] # 0x181baa5b8
0000000180373C33 call      0x18024fef0
0000000180373C38 lea       rcx, [rip + 0x1858921] # 0x181bcc560
0000000180373C3F call      0x18024fef0
0000000180373C44 lea       rcx, [rip + 0x182afcd] # 0x181b9ec18
0000000180373C4B call      0x18024fef0
0000000180373C50 lea       rcx, [rip + 0x182ac61] # 0x181b9e8b8
0000000180373C57 call      0x18024fef0
0000000180373C5C lea       rcx, [rip + 0x182ca0d] # 0x181ba0670
0000000180373C63 call      0x18024fef0
0000000180373C68 mov       byte ptr [rip + 0x1971140], 1 # 0x181ce4daf
0000000180373C6F xor       ebx, ebx
0000000180373C71 mov       dword ptr [rsp + 0xc0], ebx
0000000180373C78 mov       qword ptr [rsp + 0xd8], rbx
0000000180373C80 mov       eax, 0x7fffffff
0000000180373C85 mov       dword ptr [rsp + 0xd0], eax
0000000180373C8C mov       rax, qword ptr [r14 + 0x20]
0000000180373C90 test      rax, rax
0000000180373C93 je        0x180373f66
0000000180373C99 mov       rcx, qword ptr [rax + 0x108]
0000000180373CA0 test      rcx, rcx
0000000180373CA3 je        0x180373f66
0000000180373CA9 mov       rdx, qword ptr [rcx + 0x30]
0000000180373CAD test      rdx, rdx
0000000180373CB0 je        0x180373f66
0000000180373CB6 mov       r8, qword ptr [rip + 0x1835403] # 0x181ba90c0
0000000180373CBD lea       rcx, [rsp + 0x38]
0000000180373CC2 call      0x1808197f0
0000000180373CC7 movups    xmm0, xmmword ptr [rsp + 0x38]
0000000180373CCC movups    xmmword ptr [rsp + 0x50], xmm0
0000000180373CD1 movsd     xmm1, qword ptr [rsp + 0x48]
0000000180373CD7 movsd     qword ptr [rsp + 0x60], xmm1
0000000180373CDD mov       qword ptr [rsp + 0x38], rbx
0000000180373CE2 lea       rbx, [rsp + 0x50]
0000000180373CE7 mov       qword ptr [rsp + 0x40], rbx
0000000180373CEC movss     xmm6, dword ptr [rip + 0x1233fc8] # 0x1815a7cbc
0000000180373CF4 mov       rdx, qword ptr [rip + 0x184ea5d] # 0x181bc2758
0000000180373CFB lea       rcx, [rsp + 0x50]
0000000180373D00 call      0x180669280
0000000180373D05 test      al, al
0000000180373D07 je        0x180373f1a
0000000180373D0D mov       rsi, qword ptr [rsp + 0x60]
0000000180373D12 test      rsi, rsi
0000000180373D15 je        0x180373fa4
0000000180373D1B cmp       dword ptr [rsi + 0x5c], 5
0000000180373D1F jne       0x180373cf4
0000000180373D21 mov       r15d, dword ptr [rsi + 0x30]
0000000180373D25 mov       r12d, dword ptr [rsi + 0x34]
0000000180373D29 mov       rax, qword ptr [r14 + 0x20]
0000000180373D2D test      rax, rax
0000000180373D30 je        0x180373f9f
0000000180373D36 mov       r13, qword ptr [rax + 0x28]
0000000180373D3A mov       rcx, qword ptr [rip + 0x182aae7] # 0x181b9e828
0000000180373D41 call      0x180250100
0000000180373D46 mov       rdi, rax
0000000180373D49 mov       qword ptr [rsp + 0x28], 0
0000000180373D52 mov       qword ptr [rsp + 0x20], r13
0000000180373D57 movaps    xmm3, xmm6
0000000180373D5A mov       r8d, r12d
0000000180373D5D mov       edx, r15d
0000000180373D60 mov       rcx, rax
0000000180373D63 call      0x1803299e0
0000000180373D68 mov       rcx, qword ptr [r14 + 0x120]
0000000180373D6F test      rcx, rcx
0000000180373D72 je        0x180373f9a
0000000180373D78 mov       r9, qword ptr [rip + 0x1836789] # 0x181baa508
0000000180373D7F mov       r8, rdi
0000000180373D82 xor       edx, edx
0000000180373D84 call      0x18084d9c0
0000000180373D89 xor       edx, edx
0000000180373D8B mov       rcx, r14
0000000180373D8E call      0x180365ea0
0000000180373D93 test      al, al
0000000180373D95 je        0x180373ef7
0000000180373D9B xor       r9d, r9d
0000000180373D9E mov       r8d, dword ptr [r14 + 0x2c]
0000000180373DA2 mov       edx, dword ptr [r14 + 0x28]
0000000180373DA6 mov       rcx, qword ptr [r14 + 0x128]
0000000180373DAD call      0x180329830
0000000180373DB2 mov       dword ptr [rsp + 0xc0], eax
0000000180373DB9 mov       edx, 6
0000000180373DBE mov       rcx, qword ptr [rip + 0x185879b] # 0x181bcc560
0000000180373DC5 call      0x18024f3e0
0000000180373DCA mov       rdi, rax
0000000180373DCD test      rax, rax
0000000180373DD0 je        0x180373f90
0000000180373DD6 mov       rdx, qword ptr [rip + 0x182c893] # 0x181ba0670
0000000180373DDD cmp       dword ptr [rax + 0x18], 0
0000000180373DE1 jbe       0x180373f6c
0000000180373DE7 mov       qword ptr [rax + 0x20], rdx
0000000180373DEB lea       rcx, [rax + 0x20]
0000000180373DEF call      0x18024f360
0000000180373DF4 test      rsi, rsi
0000000180373DF7 je        0x180373f8b
0000000180373DFD lea       rcx, [rsi + 0x30]
0000000180373E01 xor       edx, edx
0000000180373E03 call      0x180ca3820
0000000180373E08 cmp       dword ptr [rdi + 0x18], 1
0000000180373E0C jbe       0x180373f72
0000000180373E12 mov       qword ptr [rdi + 0x28], rax
0000000180373E16 lea       rcx, [rdi + 0x28]
0000000180373E1A mov       rdx, rax
0000000180373E1D call      0x18024f360
0000000180373E22 mov       rdx, qword ptr [rip + 0x182adef] # 0x181b9ec18
0000000180373E29 cmp       dword ptr [rdi + 0x18], 2
0000000180373E2D jbe       0x180373f77
0000000180373E33 mov       qword ptr [rdi + 0x30], rdx
0000000180373E37 lea       rcx, [rdi + 0x30]
0000000180373E3B call      0x18024f360
0000000180373E40 lea       rcx, [rsi + 0x34]
0000000180373E44 xor       edx, edx
0000000180373E46 call      0x180ca3820
0000000180373E4B cmp       dword ptr [rdi + 0x18], 3
0000000180373E4F jbe       0x180373f7c
0000000180373E55 mov       qword ptr [rdi + 0x38], rax
0000000180373E59 lea       rcx, [rdi + 0x38]
0000000180373E5D mov       rdx, rax
0000000180373E60 call      0x18024f360
0000000180373E65 mov       rdx, qword ptr [rip + 0x182aa4c] # 0x181b9e8b8
0000000180373E6C cmp       dword ptr [rdi + 0x18], 4
0000000180373E70 jbe       0x180373f81
0000000180373E76 mov       qword ptr [rdi + 0x40], rdx
0000000180373E7A lea       rcx, [rdi + 0x40]
0000000180373E7E call      0x18024f360
0000000180373E83 xor       edx, edx
0000000180373E85 lea       rcx, [rsp + 0xc0]
0000000180373E8D call      0x180ca3820
0000000180373E92 cmp       dword ptr [rdi + 0x18], 5
0000000180373E96 jbe       0x180373f86
0000000180373E9C mov       qword ptr [rdi + 0x48], rax
0000000180373EA0 lea       rcx, [rdi + 0x48]
0000000180373EA4 mov       rdx, rax
0000000180373EA7 call      0x18024f360
0000000180373EAC xor       edx, edx
0000000180373EAE mov       rcx, rdi
0000000180373EB1 call      0x180b73be0
0000000180373EB6 mov       rdi, rax
0000000180373EB9 mov       rcx, qword ptr [rip + 0x1826e88] # 0x181b9ad48
0000000180373EC0 cmp       dword ptr [rcx + 0xe0], 0
0000000180373EC7 jne       0x180373ece
0000000180373EC9 call      0x1802501e0
0000000180373ECE xor       edx, edx
0000000180373ED0 mov       rcx, rdi
0000000180373ED3 call      0x1812e68f0
0000000180373ED8 mov       eax, dword ptr [rsp + 0xc0]
0000000180373EDF cmp       eax, dword ptr [rsp + 0xd0]
0000000180373EE6 jge       0x180373ef7
0000000180373EE8 mov       dword ptr [rsp + 0xd0], eax
0000000180373EEF mov       qword ptr [rsp + 0xd8], rsi
0000000180373EF7 mov       rcx, qword ptr [r14 + 0x120]
0000000180373EFE test      rcx, rcx
0000000180373F01 je        0x180373f95
0000000180373F07 mov       r8, qword ptr [rip + 0x18366aa] # 0x181baa5b8
0000000180373F0E xor       edx, edx
0000000180373F10 call      0x18084e090
0000000180373F15 jmp       0x180373cf4
0000000180373F1A mov       rdx, qword ptr [rip + 0x184e7df] # 0x181bc2700
0000000180373F21 mov       rcx, rbx
0000000180373F24 call      0x180302170
0000000180373F29 jmp       0x180373f46
0000000180373F2B mov       rdx, qword ptr [rip + 0x184e7ce] # 0x181bc2700
0000000180373F32 mov       rcx, qword ptr [rsp + 0x40]
0000000180373F37 call      0x180302170
0000000180373F3C mov       rcx, qword ptr [rsp + 0x38]
0000000180373F41 test      rcx, rcx
0000000180373F44 jne       0x180373faa
0000000180373F46 mov       rax, qword ptr [rsp + 0xd8]
0000000180373F4E movaps    xmm6, xmmword ptr [rsp + 0x70]
0000000180373F53 add       rsp, 0x80
0000000180373F5A pop       r15
0000000180373F5C pop       r14
0000000180373F5E pop       r13
0000000180373F60 pop       r12
0000000180373F62 pop       rdi
0000000180373F63 pop       rsi
0000000180373F64 pop       rbx
0000000180373F65 ret       
0000000180373F66 call      0x180250150
0000000180373F6B nop       
0000000180373F6C call      0x180250140
0000000180373F71 nop       
0000000180373F72 call      0x180250140
0000000180373F77 call      0x180250140
0000000180373F7C call      0x180250140
0000000180373F81 call      0x180250140
0000000180373F86 call      0x180250140
0000000180373F8B call      0x180250150
0000000180373F90 call      0x180250150
0000000180373F95 call      0x180250150
0000000180373F9A call      0x180250150
0000000180373F9F call      0x180250150
0000000180373FA4 call      0x180250150
0000000180373FA9 nop       
0000000180373FAA call      0x180200e60
0000000180373FAF int3      