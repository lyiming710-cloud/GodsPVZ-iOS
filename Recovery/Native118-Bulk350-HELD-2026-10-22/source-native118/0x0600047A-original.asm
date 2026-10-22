0000000180367920 mov       qword ptr [rsp + 0x10], rbx
0000000180367925 mov       qword ptr [rsp + 0x18], rsi
000000018036792A mov       qword ptr [rsp + 0x20], rdi
000000018036792F mov       qword ptr [rsp + 8], rcx
0000000180367934 push      r14
0000000180367936 sub       rsp, 0xa0
000000018036793D mov       rsi, rcx
0000000180367940 cmp       byte ptr [rip + 0x197d43f], 0 # 0x181ce4d86
0000000180367947 jne       0x1803679bc
0000000180367949 lea       rcx, [rip + 0x185b2d8] # 0x181bc2c28
0000000180367950 call      0x18024fef0
0000000180367955 lea       rcx, [rip + 0x185b3d4] # 0x181bc2d30
000000018036795C call      0x18024fef0
0000000180367961 lea       rcx, [rip + 0x185b318] # 0x181bc2c80
0000000180367968 call      0x18024fef0
000000018036796D lea       rcx, [rip + 0x185b414] # 0x181bc2d88
0000000180367974 call      0x18024fef0
0000000180367979 lea       rcx, [rip + 0x185b358] # 0x181bc2cd8
0000000180367980 call      0x18024fef0
0000000180367985 lea       rcx, [rip + 0x185b44c] # 0x181bc2dd8
000000018036798C call      0x18024fef0
0000000180367991 lea       rcx, [rip + 0x18427a8] # 0x181baa140
0000000180367998 call      0x18024fef0
000000018036799D lea       rcx, [rip + 0x18426ec] # 0x181baa090
00000001803679A4 call      0x18024fef0
00000001803679A9 lea       rcx, [rip + 0x1848b50] # 0x181bb0500
00000001803679B0 call      0x18024fef0
00000001803679B5 mov       byte ptr [rip + 0x197d3ca], 1 # 0x181ce4d86
00000001803679BC xorps     xmm0, xmm0
00000001803679BF xor       eax, eax
00000001803679C1 movups    xmmword ptr [rsp + 0x48], xmm0
00000001803679C6 mov       qword ptr [rsp + 0x58], rax
00000001803679CB xorps     xmm1, xmm1
00000001803679CE movups    xmmword ptr [rsp + 0x60], xmm1
00000001803679D3 mov       qword ptr [rsp + 0x70], rax
00000001803679D8 mov       rbx, qword ptr [rsi + 0x20]
00000001803679DC mov       rcx, qword ptr [rip + 0x1848b1d] # 0x181bb0500
00000001803679E3 cmp       dword ptr [rcx + 0xe0], eax
00000001803679E9 jne       0x1803679f0
00000001803679EB call      0x1802501e0
00000001803679F0 xor       r8d, r8d
00000001803679F3 xor       edx, edx
00000001803679F5 mov       rcx, rbx
00000001803679F8 call      0x18131f760
00000001803679FD test      al, al
00000001803679FF jne       0x180367de0
0000000180367A05 xor       r14d, r14d
0000000180367A08 mov       ebx, r14d
0000000180367A0B mov       ecx, r14d
0000000180367A0E mov       rax, qword ptr [rsi + 0x1f8]
0000000180367A15 test      rax, rax
0000000180367A18 je        0x180367dfa
0000000180367A1E nop       
0000000180367A20 cmp       ecx, dword ptr [rax + 0x18]
0000000180367A23 jge       0x180367af5
0000000180367A29 mov       rcx, qword ptr [rsi + 0x1f8]
0000000180367A30 test      rcx, rcx
0000000180367A33 je        0x180367dfa
0000000180367A39 cmp       ebx, dword ptr [rcx + 0x18]
0000000180367A3C jae       0x180367e00
0000000180367A42 movsxd    rax, ebx
0000000180367A45 mov       rdi, qword ptr [rcx + rax*8 + 0x20]
0000000180367A4A mov       rcx, qword ptr [rip + 0x1848aaf] # 0x181bb0500
0000000180367A51 cmp       dword ptr [rcx + 0xe0], r14d
0000000180367A58 jne       0x180367a5f
0000000180367A5A call      0x1802501e0
0000000180367A5F xor       r8d, r8d
0000000180367A62 xor       edx, edx
0000000180367A64 mov       rcx, rdi
0000000180367A67 call      0x18131f900
0000000180367A6C test      al, al
0000000180367A6E je        0x180367adc
0000000180367A70 mov       rcx, qword ptr [rsi + 0x1f8]
0000000180367A77 test      rcx, rcx
0000000180367A7A je        0x180367dfa
0000000180367A80 cmp       ebx, dword ptr [rcx + 0x18]
0000000180367A83 jae       0x180367e00
0000000180367A89 movsxd    rax, ebx
0000000180367A8C mov       rcx, qword ptr [rcx + rax*8 + 0x20]
0000000180367A91 test      rcx, rcx
0000000180367A94 je        0x180367dfa
0000000180367A9A xor       edx, edx
0000000180367A9C call      0x18131c070
0000000180367AA1 mov       rdi, rax
0000000180367AA4 mov       rcx, qword ptr [rsi + 0x20]
0000000180367AA8 test      rcx, rcx
0000000180367AAB je        0x180367dfa
0000000180367AB1 mov       rcx, qword ptr [rcx + 0x78]
0000000180367AB5 test      rcx, rcx
0000000180367AB8 je        0x180367dfa
0000000180367ABE xor       edx, edx
0000000180367AC0 call      0x18131c070
0000000180367AC5 test      rdi, rdi
0000000180367AC8 je        0x180367dfa
0000000180367ACE xor       r8d, r8d
0000000180367AD1 mov       rdx, rax
0000000180367AD4 mov       rcx, rdi
0000000180367AD7 call      0x18132e8b0
0000000180367ADC inc       ebx
0000000180367ADE mov       ecx, ebx
0000000180367AE0 mov       rax, qword ptr [rsi + 0x1f8]
0000000180367AE7 test      rax, rax
0000000180367AEA je        0x180367dfa
0000000180367AF0 jmp       0x180367a20
0000000180367AF5 mov       rbx, qword ptr [rsi + 0x200]
0000000180367AFC mov       rcx, qword ptr [rip + 0x18489fd] # 0x181bb0500
0000000180367B03 cmp       dword ptr [rcx + 0xe0], r14d
0000000180367B0A jne       0x180367b11
0000000180367B0C call      0x1802501e0
0000000180367B11 xor       r8d, r8d
0000000180367B14 xor       edx, edx
0000000180367B16 mov       rcx, rbx
0000000180367B19 call      0x18131f900
0000000180367B1E test      al, al
0000000180367B20 je        0x180367b74
0000000180367B22 mov       rcx, qword ptr [rsi + 0x200]
0000000180367B29 test      rcx, rcx
0000000180367B2C je        0x180367dfa
0000000180367B32 xor       edx, edx
0000000180367B34 call      0x18131c070
0000000180367B39 mov       rbx, rax
0000000180367B3C mov       rcx, qword ptr [rsi + 0x20]
0000000180367B40 test      rcx, rcx
0000000180367B43 je        0x180367dfa
0000000180367B49 mov       rcx, qword ptr [rcx + 0x78]
0000000180367B4D test      rcx, rcx
0000000180367B50 je        0x180367dfa
0000000180367B56 xor       edx, edx
0000000180367B58 call      0x18131c070
0000000180367B5D test      rbx, rbx
0000000180367B60 je        0x180367dfa
0000000180367B66 xor       r8d, r8d
0000000180367B69 mov       rdx, rax
0000000180367B6C mov       rcx, rbx
0000000180367B6F call      0x18132e8b0
0000000180367B74 mov       rbx, qword ptr [rsi + 0x210]
0000000180367B7B mov       rcx, qword ptr [rip + 0x184897e] # 0x181bb0500
0000000180367B82 cmp       dword ptr [rcx + 0xe0], r14d
0000000180367B89 jne       0x180367b90
0000000180367B8B call      0x1802501e0
0000000180367B90 xor       edx, edx
0000000180367B92 mov       rcx, rbx
0000000180367B95 call      0x18131f870
0000000180367B9A test      al, al
0000000180367B9C je        0x180367bf0
0000000180367B9E mov       rcx, qword ptr [rsi + 0x210]
0000000180367BA5 test      rcx, rcx
0000000180367BA8 je        0x180367dfa
0000000180367BAE xor       edx, edx
0000000180367BB0 call      0x18131c070
0000000180367BB5 mov       rbx, rax
0000000180367BB8 mov       rcx, qword ptr [rsi + 0x20]
0000000180367BBC test      rcx, rcx
0000000180367BBF je        0x180367dfa
0000000180367BC5 mov       rcx, qword ptr [rcx + 0x78]
0000000180367BC9 test      rcx, rcx
0000000180367BCC je        0x180367dfa
0000000180367BD2 xor       edx, edx
0000000180367BD4 call      0x18131c070
0000000180367BD9 test      rbx, rbx
0000000180367BDC je        0x180367dfa
0000000180367BE2 xor       r8d, r8d
0000000180367BE5 mov       rdx, rax
0000000180367BE8 mov       rcx, rbx
0000000180367BEB call      0x18132e8b0
0000000180367BF0 mov       rbx, qword ptr [rsi + 0x220]
0000000180367BF7 mov       rcx, qword ptr [rip + 0x1848902] # 0x181bb0500
0000000180367BFE cmp       dword ptr [rcx + 0xe0], r14d
0000000180367C05 jne       0x180367c0c
0000000180367C07 call      0x1802501e0
0000000180367C0C xor       edx, edx
0000000180367C0E mov       rcx, rbx
0000000180367C11 call      0x18131f870
0000000180367C16 test      al, al
0000000180367C18 je        0x180367c6c
0000000180367C1A mov       rcx, qword ptr [rsi + 0x220]
0000000180367C21 test      rcx, rcx
0000000180367C24 je        0x180367dfa
0000000180367C2A xor       edx, edx
0000000180367C2C call      0x18131c070
0000000180367C31 mov       rbx, rax
0000000180367C34 mov       rcx, qword ptr [rsi + 0x20]
0000000180367C38 test      rcx, rcx
0000000180367C3B je        0x180367dfa
0000000180367C41 mov       rcx, qword ptr [rcx + 0x78]
0000000180367C45 test      rcx, rcx
0000000180367C48 je        0x180367dfa
0000000180367C4E xor       edx, edx
0000000180367C50 call      0x18131c070
0000000180367C55 test      rbx, rbx
0000000180367C58 je        0x180367dfa
0000000180367C5E xor       r8d, r8d
0000000180367C61 mov       rdx, rax
0000000180367C64 mov       rcx, rbx
0000000180367C67 call      0x18132e8b0
0000000180367C6C mov       rax, qword ptr [rsi + 0x108]
0000000180367C73 test      rax, rax
0000000180367C76 je        0x180367dfa
0000000180367C7C mov       rdx, qword ptr [rax + 0x20]
0000000180367C80 test      rdx, rdx
0000000180367C83 je        0x180367dfa
0000000180367C89 mov       r8, qword ptr [rip + 0x1842400] # 0x181baa090
0000000180367C90 lea       rcx, [rsp + 0x30]
0000000180367C95 call      0x1808197f0
0000000180367C9A movups    xmm0, xmmword ptr [rsp + 0x30]
0000000180367C9F movups    xmmword ptr [rsp + 0x48], xmm0
0000000180367CA4 movsd     xmm1, qword ptr [rsp + 0x40]
0000000180367CAA movsd     qword ptr [rsp + 0x58], xmm1
0000000180367CB0 mov       qword ptr [rsp + 0x20], r14
0000000180367CB5 lea       rax, [rsp + 0x48]
0000000180367CBA mov       qword ptr [rsp + 0x28], rax
0000000180367CBF nop       
0000000180367CC0 mov       rdx, qword ptr [rip + 0x185afb9] # 0x181bc2c80
0000000180367CC7 lea       rcx, [rsp + 0x48]
0000000180367CCC call      0x180669280
0000000180367CD1 test      al, al
0000000180367CD3 je        0x180367db2
0000000180367CD9 mov       rdi, qword ptr [rsp + 0x58]
0000000180367CDE mov       rdx, qword ptr [rsi + 0x218]
0000000180367CE5 test      rdx, rdx
0000000180367CE8 je        0x180367e17
0000000180367CEE mov       r8, qword ptr [rip + 0x184244b] # 0x181baa140
0000000180367CF5 lea       rcx, [rsp + 0x88]
0000000180367CFD call      0x1808197f0
0000000180367D02 movups    xmm0, xmmword ptr [rsp + 0x88]
0000000180367D0A movups    xmmword ptr [rsp + 0x60], xmm0
0000000180367D0F movsd     xmm1, qword ptr [rsp + 0x98]
0000000180367D18 movsd     qword ptr [rsp + 0x70], xmm1
0000000180367D1E mov       qword ptr [rsp + 0x30], r14
0000000180367D23 lea       rbx, [rsp + 0x60]
0000000180367D28 mov       qword ptr [rsp + 0x38], rbx
0000000180367D2D nop       dword ptr [rax]
0000000180367D30 mov       rdx, qword ptr [rip + 0x185b051] # 0x181bc2d88
0000000180367D37 lea       rcx, [rsp + 0x60]
0000000180367D3C call      0x180669280
0000000180367D41 test      al, al
0000000180367D43 je        0x180367d73
0000000180367D45 test      rdi, rdi
0000000180367D48 je        0x180367e0c
0000000180367D4E mov       eax, dword ptr [rdi + 0x20]
0000000180367D51 mov       rdx, qword ptr [rsp + 0x70]
0000000180367D56 test      rdx, rdx
0000000180367D59 je        0x180367e06
0000000180367D5F cmp       eax, dword ptr [rdx + 0x20]
0000000180367D62 jne       0x180367d30
0000000180367D64 mov       qword ptr [rdi + 0x28], rdx
0000000180367D68 lea       rcx, [rdi + 0x28]
0000000180367D6C call      0x18024f360
0000000180367D71 jmp       0x180367d30
0000000180367D73 mov       rdx, qword ptr [rip + 0x185afb6] # 0x181bc2d30
0000000180367D7A mov       rcx, rbx
0000000180367D7D call      0x180302170
0000000180367D82 jmp       0x180367cc0
0000000180367D87 mov       rdx, qword ptr [rip + 0x185afa2] # 0x181bc2d30
0000000180367D8E mov       rcx, qword ptr [rsp + 0x38]
0000000180367D93 call      0x180302170
0000000180367D98 mov       rcx, qword ptr [rsp + 0x30]
0000000180367D9D test      rcx, rcx
0000000180367DA0 jne       0x180367e12
0000000180367DA2 xor       r14d, r14d
0000000180367DA5 mov       rsi, qword ptr [rsp + 0xb0]
0000000180367DAD jmp       0x180367cc0
0000000180367DB2 mov       rdx, qword ptr [rip + 0x185ae6f] # 0x181bc2c28
0000000180367DB9 mov       rcx, qword ptr [rsp + 0x28]
0000000180367DBE call      0x180302170
0000000180367DC3 jmp       0x180367de0
0000000180367DC5 mov       rdx, qword ptr [rip + 0x185ae5c] # 0x181bc2c28
0000000180367DCC mov       rcx, qword ptr [rsp + 0x28]
0000000180367DD1 call      0x180302170
0000000180367DD6 mov       rcx, qword ptr [rsp + 0x20]
0000000180367DDB test      rcx, rcx
0000000180367DDE jne       0x180367e1d
0000000180367DE0 lea       r11, [rsp + 0xa0]
0000000180367DE8 mov       rbx, qword ptr [r11 + 0x18]
0000000180367DEC mov       rsi, qword ptr [r11 + 0x20]
0000000180367DF0 mov       rdi, qword ptr [r11 + 0x28]
0000000180367DF4 mov       rsp, r11
0000000180367DF7 pop       r14
0000000180367DF9 ret       
0000000180367DFA call      0x180250150
0000000180367DFF int3      
0000000180367E00 call      0x180250140
0000000180367E05 nop       
0000000180367E06 call      0x180250150
0000000180367E0B nop       
0000000180367E0C call      0x180250150
0000000180367E11 nop       
0000000180367E12 call      0x180200e60
0000000180367E17 call      0x180250150
0000000180367E1C nop       
0000000180367E1D call      0x180200e60
0000000180367E22 int3      
0000000180367E23 int3      
0000000180367E24 int3      
0000000180367E25 int3      
0000000180367E26 int3      
0000000180367E27 int3      
0000000180367E28 int3      
0000000180367E29 int3      
0000000180367E2A int3      
0000000180367E2B int3      
0000000180367E2C int3      
0000000180367E2D int3      
0000000180367E2E int3      
0000000180367E2F int3      