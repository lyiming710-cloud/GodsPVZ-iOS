000000018036F9A0 mov       rax, rsp
000000018036F9A3 mov       qword ptr [rax + 0x10], rbx
000000018036F9A7 mov       qword ptr [rax + 0x18], rsi
000000018036F9AB mov       qword ptr [rax + 8], rcx
000000018036F9AF push      rdi
000000018036F9B0 push      r14
000000018036F9B2 push      r15
000000018036F9B4 sub       rsp, 0xb0
000000018036F9BB movaps    xmmword ptr [rax - 0x28], xmm6
000000018036F9BF movaps    xmmword ptr [rax - 0x38], xmm7
000000018036F9C3 movaps    xmmword ptr [rax - 0x48], xmm8
000000018036F9C8 mov       r14, rcx
000000018036F9CB cmp       byte ptr [rip + 0x19753c8], 0 # 0x181ce4d9a
000000018036F9D2 jne       0x18036fa47
000000018036F9D4 lea       rcx, [rip + 0x185ae8d] # 0x181bca868
000000018036F9DB call      0x18024fef0
000000018036F9E0 lea       rcx, [rip + 0x185aed1] # 0x181bca8b8
000000018036F9E7 call      0x18024fef0
000000018036F9EC lea       rcx, [rip + 0x185af1d] # 0x181bca910
000000018036F9F3 call      0x18024fef0
000000018036F9F8 lea       rcx, [rip + 0x1853ce9] # 0x181bc36e8
000000018036F9FF call      0x18024fef0
000000018036FA04 lea       rcx, [rip + 0x1840af5] # 0x181bb0500
000000018036FA0B call      0x18024fef0
000000018036FA10 lea       rcx, [rip + 0x184adb9] # 0x181bba7d0
000000018036FA17 call      0x18024fef0
000000018036FA1C lea       rcx, [rip + 0x184ca0d] # 0x181bbc430
000000018036FA23 call      0x18024fef0
000000018036FA28 lea       rcx, [rip + 0x185c349] # 0x181bcbd78
000000018036FA2F call      0x18024fef0
000000018036FA34 lea       rcx, [rip + 0x185c3e5] # 0x181bcbe20
000000018036FA3B call      0x18024fef0
000000018036FA40 mov       byte ptr [rip + 0x1975353], 1 # 0x181ce4d9a
000000018036FA47 mov       rax, qword ptr [r14 + 0x20]
000000018036FA4B test      rax, rax
000000018036FA4E je        0x18036fdb3
000000018036FA54 mov       rcx, qword ptr [rax + 0xf0]
000000018036FA5B test      rcx, rcx
000000018036FA5E je        0x18036fdb3
000000018036FA64 mov       rdx, qword ptr [rcx + 0x38]
000000018036FA68 test      rdx, rdx
000000018036FA6B je        0x18036fdb3
000000018036FA71 mov       r8, qword ptr [rip + 0x1853c70] # 0x181bc36e8
000000018036FA78 lea       rcx, [rsp + 0x50]
000000018036FA7D call      0x1808197f0
000000018036FA82 movups    xmm0, xmmword ptr [rsp + 0x50]
000000018036FA87 movups    xmmword ptr [rsp + 0x68], xmm0
000000018036FA8C movsd     xmm1, qword ptr [rsp + 0x60]
000000018036FA92 movsd     qword ptr [rsp + 0x78], xmm1
000000018036FA98 xor       r15d, r15d
000000018036FA9B mov       qword ptr [rsp + 0x50], r15
000000018036FAA0 lea       rbx, [rsp + 0x68]
000000018036FAA5 mov       qword ptr [rsp + 0x58], rbx
000000018036FAAA movss     xmm6, dword ptr [rip + 0x1237f6e] # 0x1815a7a20
000000018036FAB2 movss     xmm7, dword ptr [rip + 0x12381c6] # 0x1815a7c80
000000018036FABA movss     xmm8, dword ptr [rip + 0x12381a9] # 0x1815a7c6c
000000018036FAC3 mov       rdx, qword ptr [rip + 0x185adee] # 0x181bca8b8
000000018036FACA lea       rcx, [rsp + 0x68]
000000018036FACF call      0x180669280
000000018036FAD4 test      al, al
000000018036FAD6 je        0x18036fc86
000000018036FADC mov       rdi, qword ptr [rsp + 0x78]
000000018036FAE1 test      rdi, rdi
000000018036FAE4 je        0x18036fddd
000000018036FAEA xor       edx, edx
000000018036FAEC mov       rcx, rdi
000000018036FAEF call      0x1803659e0
000000018036FAF4 test      al, al
000000018036FAF6 jne       0x18036fac3
000000018036FAF8 mov       eax, dword ptr [rdi + 0xfc]
000000018036FAFE cmp       eax, dword ptr [r14 + 0xfc]
000000018036FB05 jne       0x18036fac3
000000018036FB07 mov       rcx, qword ptr [rdi + 0x228]
000000018036FB0E test      rcx, rcx
000000018036FB11 je        0x18036fdd8
000000018036FB17 xor       r8d, r8d
000000018036FB1A mov       rdx, qword ptr [rip + 0x185c2ff] # 0x181bcbe20
000000018036FB21 call      0x180310250
000000018036FB26 test      rax, rax
000000018036FB29 jne       0x18036fac3
000000018036FB2B mov       rcx, qword ptr [rip + 0x18409ce] # 0x181bb0500
000000018036FB32 cmp       dword ptr [rcx + 0xe0], eax
000000018036FB38 jne       0x18036fb3f
000000018036FB3A call      0x1802501e0
000000018036FB3F xor       r8d, r8d
000000018036FB42 mov       rdx, r14
000000018036FB45 mov       rcx, rdi
000000018036FB48 call      0x18131f760
000000018036FB4D test      al, al
000000018036FB4F je        0x18036fbcb
000000018036FB51 mov       rcx, qword ptr [rip + 0x184ac78] # 0x181bba7d0
000000018036FB58 call      0x180250100
000000018036FB5D mov       rsi, rax
000000018036FB60 mov       qword ptr [rsp + 0x20], r15
000000018036FB65 mov       r9b, 1
000000018036FB68 movaps    xmm2, xmm6
000000018036FB6B mov       rdx, qword ptr [rip + 0x184c8be] # 0x181bbc430
000000018036FB72 mov       rcx, rax
000000018036FB75 call      0x180325080
000000018036FB7A test      rsi, rsi
000000018036FB7D je        0x18036fdc4
000000018036FB83 mov       rax, qword ptr [rip + 0x185c1ee] # 0x181bcbd78
000000018036FB8A mov       qword ptr [rsi + 0x10], rax
000000018036FB8E lea       rcx, [rsi + 0x10]
000000018036FB92 mov       rdx, qword ptr [rip + 0x185c1df] # 0x181bcbd78
000000018036FB99 call      0x18024f360
000000018036FB9E mov       dword ptr [rsi + 0x18], 1
000000018036FBA5 test      rdi, rdi
000000018036FBA8 je        0x18036fdbf
000000018036FBAE mov       rcx, qword ptr [rdi + 0x228]
000000018036FBB5 test      rcx, rcx
000000018036FBB8 je        0x18036fdb9
000000018036FBBE xor       r8d, r8d
000000018036FBC1 mov       rdx, rsi
000000018036FBC4 call      0x180310190
000000018036FBC9 jmp       0x18036fbd4
000000018036FBCB test      rdi, rdi
000000018036FBCE je        0x18036fdd3
000000018036FBD4 cmp       byte ptr [rdi + 0xb5], 0
000000018036FBDB jne       0x18036fc11
000000018036FBDD cmp       byte ptr [rdi + 0xb4], 0
000000018036FBE4 jne       0x18036fc11
000000018036FBE6 mov       rcx, qword ptr [rip + 0x184abe3] # 0x181bba7d0
000000018036FBED call      0x180250100
000000018036FBF2 mov       rsi, rax
000000018036FBF5 mov       qword ptr [rsp + 0x20], r15
000000018036FBFA xor       r9d, r9d
000000018036FBFD movaps    xmm2, xmm7
000000018036FC00 mov       rdx, qword ptr [rip + 0x184c829] # 0x181bbc430
000000018036FC07 mov       rcx, rax
000000018036FC0A call      0x180325080
000000018036FC0F jmp       0x18036fc3b
000000018036FC11 mov       rcx, qword ptr [rip + 0x184abb8] # 0x181bba7d0
000000018036FC18 call      0x180250100
000000018036FC1D mov       rsi, rax
000000018036FC20 mov       qword ptr [rsp + 0x20], r15
000000018036FC25 xor       r9d, r9d
000000018036FC28 movaps    xmm2, xmm8
000000018036FC2C mov       rdx, qword ptr [rip + 0x184c7fd] # 0x181bbc430
000000018036FC33 mov       rcx, rax
000000018036FC36 call      0x180325080
000000018036FC3B test      rsi, rsi
000000018036FC3E je        0x18036fdce
000000018036FC44 mov       rax, qword ptr [rip + 0x185c1d5] # 0x181bcbe20
000000018036FC4B mov       qword ptr [rsi + 0x10], rax
000000018036FC4F lea       rcx, [rsi + 0x10]
000000018036FC53 mov       rdx, qword ptr [rip + 0x185c1c6] # 0x181bcbe20
000000018036FC5A call      0x18024f360
000000018036FC5F mov       dword ptr [rsi + 0x18], 1
000000018036FC66 mov       rcx, qword ptr [rdi + 0x228]
000000018036FC6D test      rcx, rcx
000000018036FC70 je        0x18036fdc9
000000018036FC76 xor       r8d, r8d
000000018036FC79 mov       rdx, rsi
000000018036FC7C call      0x180310190
000000018036FC81 jmp       0x18036fac3
000000018036FC86 mov       rdx, qword ptr [rip + 0x185abdb] # 0x181bca868
000000018036FC8D mov       rcx, rbx
000000018036FC90 call      0x180302170
000000018036FC95 jmp       0x18036fcbe
000000018036FC97 mov       rdx, qword ptr [rip + 0x185abca] # 0x181bca868
000000018036FC9E mov       rcx, qword ptr [rsp + 0x58]
000000018036FCA3 call      0x180302170
000000018036FCA8 mov       rcx, qword ptr [rsp + 0x50]
000000018036FCAD test      rcx, rcx
000000018036FCB0 jne       0x18036fde3
000000018036FCB6 mov       r14, qword ptr [rsp + 0xd0]
000000018036FCBE cmp       byte ptr [rip + 0x1974e6f], 0 # 0x181ce4b34
000000018036FCC5 jne       0x18036fcda
000000018036FCC7 lea       rcx, [rip + 0x1841bc2] # 0x181bb1890
000000018036FCCE call      0x18024fef0
000000018036FCD3 mov       byte ptr [rip + 0x1974e5a], 1 # 0x181ce4b34
000000018036FCDA mov       rax, qword ptr [rip + 0x1841baf] # 0x181bb1890
000000018036FCE1 mov       rcx, qword ptr [rax + 0xb8]
000000018036FCE8 mov       rcx, qword ptr [rcx]
000000018036FCEB test      rcx, rcx
000000018036FCEE je        0x18036fdb3
000000018036FCF4 xor       r8d, r8d
000000018036FCF7 mov       edx, 0xf
000000018036FCFC call      0x180321f30
000000018036FD01 mov       rbx, rax
000000018036FD04 mov       rcx, qword ptr [rip + 0x18407f5] # 0x181bb0500
000000018036FD0B cmp       dword ptr [rcx + 0xe0], 0
000000018036FD12 jne       0x18036fd19
000000018036FD14 call      0x1802501e0
000000018036FD19 xor       edx, edx
000000018036FD1B mov       rcx, rbx
000000018036FD1E call      0x18131f870
000000018036FD23 test      al, al
000000018036FD25 je        0x18036fd8b
000000018036FD27 test      rbx, rbx
000000018036FD2A je        0x18036fdb3
000000018036FD30 xor       edx, edx
000000018036FD32 mov       rcx, rbx
000000018036FD35 call      0x18131c070
000000018036FD3A mov       rbx, rax
000000018036FD3D mov       rcx, qword ptr [r14 + 0x1f0]
000000018036FD44 test      rcx, rcx
000000018036FD47 je        0x18036fdb3
000000018036FD49 xor       edx, edx
000000018036FD4B call      0x181304510
000000018036FD50 test      rax, rax
000000018036FD53 je        0x18036fdb3
000000018036FD55 xor       r8d, r8d
000000018036FD58 mov       rdx, rax
000000018036FD5B lea       rcx, [rsp + 0x50]
000000018036FD60 call      0x18132f3a0
000000018036FD65 test      rbx, rbx
000000018036FD68 je        0x18036fdb3
000000018036FD6A movsd     xmm0, qword ptr [rax]
000000018036FD6E movsd     qword ptr [rsp + 0x40], xmm0
000000018036FD74 mov       eax, dword ptr [rax + 8]
000000018036FD77 mov       dword ptr [rsp + 0x48], eax
000000018036FD7B xor       r8d, r8d
000000018036FD7E lea       rdx, [rsp + 0x40]
000000018036FD83 mov       rcx, rbx
000000018036FD86 call      0x18132fbb0
000000018036FD8B lea       r11, [rsp + 0xb0]
000000018036FD93 mov       rbx, qword ptr [r11 + 0x28]
000000018036FD97 mov       rsi, qword ptr [r11 + 0x30]
000000018036FD9B movaps    xmm6, xmmword ptr [r11 - 0x10]
000000018036FDA0 movaps    xmm7, xmmword ptr [r11 - 0x20]
000000018036FDA5 movaps    xmm8, xmmword ptr [r11 - 0x30]
000000018036FDAA mov       rsp, r11
000000018036FDAD pop       r15
000000018036FDAF pop       r14
000000018036FDB1 pop       rdi
000000018036FDB2 ret       
000000018036FDB3 call      0x180250150
000000018036FDB8 nop       
000000018036FDB9 call      0x180250150
000000018036FDBE nop       
000000018036FDBF call      0x180250150
000000018036FDC4 call      0x180250150
000000018036FDC9 call      0x180250150
000000018036FDCE call      0x180250150
000000018036FDD3 call      0x180250150
000000018036FDD8 call      0x180250150
000000018036FDDD call      0x180250150
000000018036FDE2 nop       
000000018036FDE3 call      0x180200e60
000000018036FDE8 int3      
000000018036FDE9 int3      
000000018036FDEA int3      
000000018036FDEB int3      
000000018036FDEC int3      
000000018036FDED int3      
000000018036FDEE int3      
000000018036FDEF int3      