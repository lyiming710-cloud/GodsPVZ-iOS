00000001803499F0 mov       qword ptr [rsp + 0x10], rbx
00000001803499F5 mov       qword ptr [rsp + 0x18], rbp
00000001803499FA push      rdi
00000001803499FB sub       rsp, 0x80
0000000180349A02 cmp       byte ptr [rip + 0x199b2ce], 0 # 0x181ce4cd7
0000000180349A09 mov       ebx, r8d
0000000180349A0C mov       ebp, edx
0000000180349A0E mov       rdi, rcx
0000000180349A11 jne       0x180349a32
0000000180349A13 lea       rcx, [rip + 0x18585fe] # 0x181ba2018
0000000180349A1A call      0x18024fef0
0000000180349A1F lea       rcx, [rip + 0x1866ada] # 0x181bb0500
0000000180349A26 call      0x18024fef0
0000000180349A2B mov       byte ptr [rip + 0x199b2a5], 1 # 0x181ce4cd7
0000000180349A32 mov       rax, qword ptr [rip + 0x18585df] # 0x181ba2018
0000000180349A39 cmp       dword ptr [rax + 0xe0], 0
0000000180349A40 jne       0x180349a51
0000000180349A42 mov       rcx, rax
0000000180349A45 call      0x1802501e0
0000000180349A4A mov       rax, qword ptr [rip + 0x18585c7] # 0x181ba2018
0000000180349A51 mov       rax, qword ptr [rax + 0xb8]
0000000180349A58 mov       qword ptr [rsp + 0x90], rsi
0000000180349A60 movaps    xmmword ptr [rsp + 0x70], xmm6
0000000180349A65 movaps    xmmword ptr [rsp + 0x60], xmm7
0000000180349A6A mov       rax, qword ptr [rax]
0000000180349A6D movaps    xmmword ptr [rsp + 0x50], xmm8
0000000180349A73 test      rax, rax
0000000180349A76 je        0x180349fe2
0000000180349A7C mov       rax, qword ptr [rax + 0x28]
0000000180349A80 test      rax, rax
0000000180349A83 je        0x180349fe2
0000000180349A89 mov       rcx, qword ptr [rax + 0x10]
0000000180349A8D test      rcx, rcx
0000000180349A90 je        0x180349fe2
0000000180349A96 mov       edx, dword ptr [rdi + 0x5c]
0000000180349A99 xor       r8d, r8d
0000000180349A9C call      0x1803802f0
0000000180349AA1 test      al, al
0000000180349AA3 jne       0x180349ab9
0000000180349AA5 mov       r8, qword ptr [rdi + 0x28]
0000000180349AA9 xor       r9d, r9d
0000000180349AAC mov       edx, dword ptr [rdi + 0x5c]
0000000180349AAF mov       ecx, 3
0000000180349AB4 call      0x1803a5f80
0000000180349AB9 mov       rcx, qword ptr [rdi + 0x28]
0000000180349ABD mov       dword ptr [rdi + 0x30], ebp
0000000180349AC0 mov       dword ptr [rdi + 0x34], ebx
0000000180349AC3 test      rcx, rcx
0000000180349AC6 je        0x180349fe2
0000000180349ACC xor       r9d, r9d
0000000180349ACF mov       r8d, ebx
0000000180349AD2 mov       edx, ebp
0000000180349AD4 call      0x180326fe0
0000000180349AD9 mov       rdx, qword ptr [rdi + 0x28]
0000000180349ADD mov       rsi, rax
0000000180349AE0 test      rdx, rdx
0000000180349AE3 je        0x180349fe2
0000000180349AE9 mov       rdx, qword ptr [rdx + 0x28]
0000000180349AED test      rdx, rdx
0000000180349AF0 je        0x180349fe2
0000000180349AF6 mov       r9d, ebx
0000000180349AF9 mov       qword ptr [rsp + 0x20], 0
0000000180349B02 mov       r8d, ebp
0000000180349B05 lea       rcx, [rsp + 0x40]
0000000180349B0A call      0x18030ebd0
0000000180349B0F cmp       dword ptr [rdi + 0x5c], 0xb
0000000180349B13 movsd     xmm0, qword ptr [rax]
0000000180349B17 mov       ebp, dword ptr [rax + 8]
0000000180349B1A movsd     qword ptr [rsp + 0x30], xmm0
0000000180349B20 jne       0x180349be2
0000000180349B26 test      rsi, rsi
0000000180349B29 je        0x180349fe2
0000000180349B2F xor       r8d, r8d
0000000180349B32 mov       edx, 2
0000000180349B37 mov       rcx, rsi
0000000180349B3A call      0x18032a0a0
0000000180349B3F mov       rcx, qword ptr [rip + 0x18669ba] # 0x181bb0500
0000000180349B46 mov       rbx, rax
0000000180349B49 cmp       dword ptr [rcx + 0xe0], 0
0000000180349B50 jne       0x180349b57
0000000180349B52 call      0x1802501e0
0000000180349B57 xor       r8d, r8d
0000000180349B5A xor       edx, edx
0000000180349B5C mov       rcx, rbx
0000000180349B5F call      0x18131f900
0000000180349B64 test      al, al
0000000180349B66 je        0x180349bcc
0000000180349B68 test      rbx, rbx
0000000180349B6B je        0x180349fe2
0000000180349B71 mov       eax, dword ptr [rbx + 0x5c]
0000000180349B74 movss     xmm6, dword ptr [rsp + 0x30]
0000000180349B7A cmp       eax, 6
0000000180349B7D jne       0x180349b97
0000000180349B7F movss     xmm7, dword ptr [rsp + 0x34]
0000000180349B85 subss     xmm7, dword ptr [rip + 0x125e053] # 0x1815a7be0
0000000180349B8D addss     xmm6, dword ptr [rip + 0x125e0cf] # 0x1815a7c64
0000000180349B95 jmp       0x180349bee
0000000180349B97 cmp       eax, 0xa
0000000180349B9A jne       0x180349bb4
0000000180349B9C movss     xmm7, dword ptr [rsp + 0x34]
0000000180349BA2 subss     xmm7, dword ptr [rip + 0x125e036] # 0x1815a7be0
0000000180349BAA addss     xmm6, dword ptr [rip + 0x125e0aa] # 0x1815a7c5c
0000000180349BB2 jmp       0x180349bee
0000000180349BB4 movss     xmm7, dword ptr [rsp + 0x34]
0000000180349BBA subss     xmm7, dword ptr [rip + 0x125e01e] # 0x1815a7be0
0000000180349BC2 addss     xmm6, dword ptr [rip + 0x125df42] # 0x1815a7b0c
0000000180349BCA jmp       0x180349bee
0000000180349BCC movss     xmm6, dword ptr [rsp + 0x30]
0000000180349BD2 movss     xmm7, dword ptr [rsp + 0x34]
0000000180349BD8 subss     xmm7, dword ptr [rip + 0x125e000] # 0x1815a7be0
0000000180349BE0 jmp       0x180349bee
0000000180349BE2 movss     xmm7, dword ptr [rsp + 0x34]
0000000180349BE8 movss     xmm6, dword ptr [rsp + 0x30]
0000000180349BEE mov       rcx, qword ptr [rdi + 0xd0]
0000000180349BF5 test      rcx, rcx
0000000180349BF8 je        0x180349fe2
0000000180349BFE xor       edx, edx
0000000180349C00 call      0x18131c070
0000000180349C05 test      rax, rax
0000000180349C08 je        0x180349fe2
0000000180349C0E xor       r8d, r8d
0000000180349C11 lea       rcx, [rsp + 0x40]
0000000180349C16 mov       rdx, rax
0000000180349C19 call      0x18132f3a0
0000000180349C1E movd      xmm8, ebp
0000000180349C23 movsd     xmm1, qword ptr [rax]
0000000180349C27 subss     xmm8, dword ptr [rax + 8]
0000000180349C2D movaps    xmm0, xmm1
0000000180349C30 movsd     qword ptr [rsp + 0x30], xmm1
0000000180349C36 shufps    xmm0, xmm0, 0x55
0000000180349C3A subss     xmm6, xmm1
0000000180349C3E subss     xmm7, xmm0
0000000180349C42 xor       edx, edx
0000000180349C44 mov       rcx, rdi
0000000180349C47 call      0x181304510
0000000180349C4C mov       rbx, rax
0000000180349C4F test      rax, rax
0000000180349C52 je        0x180349fe2
0000000180349C58 xor       r8d, r8d
0000000180349C5B lea       rcx, [rsp + 0x40]
0000000180349C60 mov       rdx, rax
0000000180349C63 call      0x18132f3a0
0000000180349C68 movsd     xmm1, qword ptr [rax]
0000000180349C6C addss     xmm8, dword ptr [rax + 8]
0000000180349C72 movaps    xmm0, xmm1
0000000180349C75 movsd     qword ptr [rsp + 0x30], xmm1
0000000180349C7B shufps    xmm0, xmm0, 0x55
0000000180349C7F addss     xmm6, xmm1
0000000180349C83 addss     xmm7, xmm0
0000000180349C87 unpcklps  xmm6, xmm7
0000000180349C8A lea       rdx, [rsp + 0x30]
0000000180349C8F xor       r8d, r8d
0000000180349C92 movsd     qword ptr [rsp + 0x30], xmm6
0000000180349C98 mov       rcx, rbx
0000000180349C9B movss     dword ptr [rsp + 0x38], xmm8
0000000180349CA2 call      0x18132fbb0
0000000180349CA7 mov       rcx, qword ptr [rdi + 0xd0]
0000000180349CAE test      rcx, rcx
0000000180349CB1 je        0x180349fe2
0000000180349CB7 xor       edx, edx
0000000180349CB9 call      0x18131c070
0000000180349CBE test      rax, rax
0000000180349CC1 je        0x180349fe2
0000000180349CC7 xor       r8d, r8d
0000000180349CCA lea       rcx, [rsp + 0x40]
0000000180349CCF mov       rdx, rax
0000000180349CD2 call      0x18132f3a0
0000000180349CD7 mov       ecx, dword ptr [rax]
0000000180349CD9 mov       dword ptr [rdi + 0x38], ecx
0000000180349CDC mov       rcx, qword ptr [rdi + 0xd0]
0000000180349CE3 test      rcx, rcx
0000000180349CE6 je        0x180349fe2
0000000180349CEC xor       edx, edx
0000000180349CEE call      0x18131c070
0000000180349CF3 test      rax, rax
0000000180349CF6 je        0x180349fe2
0000000180349CFC xor       r8d, r8d
0000000180349CFF lea       rcx, [rsp + 0x40]
0000000180349D04 mov       rdx, rax
0000000180349D07 call      0x18132f3a0
0000000180349D0C xor       edx, edx
0000000180349D0E mov       ecx, dword ptr [rax + 4]
0000000180349D11 mov       dword ptr [rdi + 0x3c], ecx
0000000180349D14 mov       rcx, rdi
0000000180349D17 call      0x181304510
0000000180349D1C mov       rcx, qword ptr [rdi + 0x28]
0000000180349D20 mov       rbx, rax
0000000180349D23 test      rcx, rcx
0000000180349D26 je        0x180349fe2
0000000180349D2C mov       rcx, qword ptr [rcx + 0x108]
0000000180349D33 test      rcx, rcx
0000000180349D36 je        0x180349fe2
0000000180349D3C xor       edx, edx
0000000180349D3E call      0x181304510
0000000180349D43 test      rbx, rbx
0000000180349D46 je        0x180349fe2
0000000180349D4C xor       r8d, r8d
0000000180349D4F mov       rdx, rax
0000000180349D52 mov       rcx, rbx
0000000180349D55 call      0x18132e8b0
0000000180349D5A cmp       byte ptr [rip + 0x199af79], 0 # 0x181ce4cda
0000000180349D61 jne       0x180349d8e
0000000180349D63 lea       rcx, [rip + 0x185a1ee] # 0x181ba3f58
0000000180349D6A call      0x18024fef0
0000000180349D6F lea       rcx, [rip + 0x186678a] # 0x181bb0500
0000000180349D76 call      0x18024fef0
0000000180349D7B lea       rcx, [rip + 0x187701e] # 0x181bc0da0
0000000180349D82 call      0x18024fef0
0000000180349D87 mov       byte ptr [rip + 0x199af4c], 1 # 0x181ce4cda
0000000180349D8E mov       rcx, qword ptr [rip + 0x186676b] # 0x181bb0500
0000000180349D95 mov       rbx, qword ptr [rdi + 0xe0]
0000000180349D9C cmp       dword ptr [rcx + 0xe0], 0
0000000180349DA3 jne       0x180349daa
0000000180349DA5 call      0x1802501e0
0000000180349DAA xor       r8d, r8d
0000000180349DAD xor       edx, edx
0000000180349DAF mov       rcx, rbx
0000000180349DB2 call      0x18131f760
0000000180349DB7 test      al, al
0000000180349DB9 je        0x180349df3
0000000180349DBB xor       edx, edx
0000000180349DBD mov       rcx, rdi
0000000180349DC0 call      0x1813044d0
0000000180349DC5 test      rax, rax
0000000180349DC8 je        0x180349fe2
0000000180349DCE mov       rdx, qword ptr [rip + 0x185a183] # 0x181ba3f58
0000000180349DD5 mov       rcx, rax
0000000180349DD8 call      0x18046d280
0000000180349DDD lea       rcx, [rdi + 0xe0]
0000000180349DE4 mov       qword ptr [rdi + 0xe0], rax
0000000180349DEB mov       rdx, rax
0000000180349DEE call      0x18024f360
0000000180349DF3 mov       rcx, qword ptr [rdi + 0xe0]
0000000180349DFA test      rcx, rcx
0000000180349DFD je        0x180349fe2
0000000180349E03 mov       rdx, qword ptr [rip + 0x1876f96] # 0x181bc0da0
0000000180349E0A xor       r8d, r8d
0000000180349E0D call      0x181348060
0000000180349E12 movss     xmm0, dword ptr [rdi + 0x3c]
0000000180349E17 mulss     xmm0, dword ptr [rip + 0x125dead] # 0x1815a7ccc
0000000180349E1F mov       ecx, dword ptr [rdi + 0x70]
0000000180349E22 cvttss2si edx, xmm0
0000000180349E26 test      ecx, ecx
0000000180349E28 je        0x180349e55
0000000180349E2A sub       ecx, 1
0000000180349E2D je        0x180349e3e
0000000180349E2F sub       ecx, 1
0000000180349E32 je        0x180349e70
0000000180349E34 cmp       ecx, 1
0000000180349E37 jne       0x180349e70
0000000180349E39 add       edx, 0xa
0000000180349E3C jmp       0x180349e70
0000000180349E3E movss     xmm0, dword ptr [rdi + 0x4c]
0000000180349E43 mulss     xmm0, dword ptr [rip + 0x125de09] # 0x1815a7c54
0000000180349E4B mulss     xmm0, dword ptr [rip + 0x125dbb5] # 0x1815a7a08
0000000180349E53 jmp       0x180349e6a
0000000180349E55 movss     xmm0, dword ptr [rdi + 0x4c]
0000000180349E5A mulss     xmm0, dword ptr [rip + 0x125ddf2] # 0x1815a7c54
0000000180349E62 mulss     xmm0, dword ptr [rip + 0x125dcde] # 0x1815a7b48
0000000180349E6A cvttss2si eax, xmm0
0000000180349E6E add       edx, eax
0000000180349E70 mov       rcx, qword ptr [rdi + 0xe0]
0000000180349E77 test      rcx, rcx
0000000180349E7A je        0x180349fe2
0000000180349E80 xor       r8d, r8d
0000000180349E83 call      0x1813480b0
0000000180349E88 xor       edx, edx
0000000180349E8A mov       rcx, rdi
0000000180349E8D call      0x18034acc0
0000000180349E92 cmp       byte ptr [rip + 0x199ae3f], 0 # 0x181ce4cd8
0000000180349E99 jne       0x180349eba
0000000180349E9B lea       rcx, [rip + 0x186665e] # 0x181bb0500
0000000180349EA2 call      0x18024fef0
0000000180349EA7 lea       rcx, [rip + 0x1880f42] # 0x181bcadf0
0000000180349EAE call      0x18024fef0
0000000180349EB3 mov       byte ptr [rip + 0x199ae1e], 1 # 0x181ce4cd8
0000000180349EBA mov       rcx, qword ptr [rdi + 0xd0]
0000000180349EC1 test      rcx, rcx
0000000180349EC4 je        0x180349fe2
0000000180349ECA cmp       dword ptr [rdi + 0x5c], 4
0000000180349ECE setne     dl
0000000180349ED1 xor       r8d, r8d
0000000180349ED4 call      0x18131bad0
0000000180349ED9 mov       rcx, qword ptr [rip + 0x1866620] # 0x181bb0500
0000000180349EE0 mov       rbx, qword ptr [rdi + 0xc8]
0000000180349EE7 cmp       dword ptr [rcx + 0xe0], 0
0000000180349EEE jne       0x180349ef5
0000000180349EF0 call      0x1802501e0
0000000180349EF5 xor       edx, edx
0000000180349EF7 mov       rcx, rbx
0000000180349EFA call      0x18131f870
0000000180349EFF test      al, al
0000000180349F01 je        0x180349f2a
0000000180349F03 mov       rcx, qword ptr [rdi + 0xc8]
0000000180349F0A test      rcx, rcx
0000000180349F0D je        0x180349fe2
0000000180349F13 movss     xmm2, dword ptr [rip + 0x125daf5] # 0x1815a7a10
0000000180349F1B xor       r9d, r9d
0000000180349F1E mov       rdx, qword ptr [rip + 0x1880ecb] # 0x181bcadf0
0000000180349F25 call      0x1812dbab0
0000000180349F2A mov       rax, qword ptr [rdi + 0x28]
0000000180349F2E mov       byte ptr [rdi + 0x94], 1
0000000180349F35 test      rax, rax
0000000180349F38 je        0x180349fe2
0000000180349F3E mov       rcx, qword ptr [rax + 0x108]
0000000180349F45 test      rcx, rcx
0000000180349F48 je        0x180349fe2
0000000180349F4E xor       r8d, r8d
0000000180349F51 mov       rdx, rdi
0000000180349F54 call      0x1803123a0
0000000180349F59 test      rsi, rsi
0000000180349F5C je        0x180349fe2
0000000180349F62 xor       edx, edx
0000000180349F64 mov       rcx, rsi
0000000180349F67 call      0x18032a110
0000000180349F6C xor       r8d, r8d
0000000180349F6F mov       rdx, rdi
0000000180349F72 mov       rcx, rsi
0000000180349F75 movd      xmm6, eax
0000000180349F79 call      0x18032a350
0000000180349F7E xor       edx, edx
0000000180349F80 mov       rcx, rsi
0000000180349F83 cvtdq2ps  xmm6, xmm6
0000000180349F86 call      0x18032a110
0000000180349F8B movd      xmm0, eax
0000000180349F8F cvtdq2ps  xmm0, xmm0
0000000180349F92 ucomiss   xmm6, xmm0
0000000180349F95 jp        0x180349f99
0000000180349F97 je        0x180349fb5
0000000180349F99 mov       rax, qword ptr [rdi + 0x28]
0000000180349F9D test      rax, rax
0000000180349FA0 je        0x180349fe2
0000000180349FA2 mov       rcx, qword ptr [rax + 0xf0]
0000000180349FA9 test      rcx, rcx
0000000180349FAC je        0x180349fe2
0000000180349FAE xor       edx, edx
0000000180349FB0 call      0x1803415a0
0000000180349FB5 movaps    xmm8, xmmword ptr [rsp + 0x50]
0000000180349FBB lea       r11, [rsp + 0x80]
0000000180349FC3 mov       rbx, qword ptr [r11 + 0x18]
0000000180349FC7 mov       rbp, qword ptr [r11 + 0x20]
0000000180349FCB movaps    xmm7, xmmword ptr [rsp + 0x60]
0000000180349FD0 movaps    xmm6, xmmword ptr [rsp + 0x70]
0000000180349FD5 mov       rsi, qword ptr [rsp + 0x90]
0000000180349FDD mov       rsp, r11
0000000180349FE0 pop       rdi
0000000180349FE1 ret       
0000000180349FE2 call      0x180250150
0000000180349FE7 int3      
0000000180349FE8 int3      
0000000180349FE9 int3      
0000000180349FEA int3      
0000000180349FEB int3      
0000000180349FEC int3      
0000000180349FED int3      
0000000180349FEE int3      
0000000180349FEF int3      