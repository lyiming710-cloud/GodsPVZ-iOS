0000000180300700 mov       rax, rsp
0000000180300703 mov       qword ptr [rax + 0x10], rbx
0000000180300707 mov       qword ptr [rax + 8], rcx
000000018030070B push      rdi
000000018030070C push      r14
000000018030070E push      r15
0000000180300710 sub       rsp, 0xe0
0000000180300717 movaps    xmmword ptr [rax - 0x28], xmm6
000000018030071B movaps    xmmword ptr [rax - 0x38], xmm7
000000018030071F movaps    xmmword ptr [rax - 0x48], xmm8
0000000180300724 movaps    xmmword ptr [rax - 0x58], xmm9
0000000180300729 movaps    xmmword ptr [rax - 0x68], xmm10
000000018030072E mov       r14, rcx
0000000180300731 cmp       byte ptr [rip + 0x19e43f0], 0 # 0x181ce4b28
0000000180300738 jne       0x180300805
000000018030073E lea       rcx, [rip + 0x189a603] # 0x181b9ad48
0000000180300745 call      0x18024fef0
000000018030074A lea       rcx, [rip + 0x189da77] # 0x181b9e1c8
0000000180300751 call      0x18024fef0
0000000180300756 lea       rcx, [rip + 0x18c1343] # 0x181bc1aa0
000000018030075D call      0x18024fef0
0000000180300762 lea       rcx, [rip + 0x18c0e0f] # 0x181bc1578
0000000180300769 call      0x18024fef0
000000018030076E lea       rcx, [rip + 0x18c1383] # 0x181bc1af8
0000000180300775 call      0x18024fef0
000000018030077A lea       rcx, [rip + 0x18c0e4f] # 0x181bc15d0
0000000180300781 call      0x18024fef0
0000000180300786 lea       rcx, [rip + 0x18c0e9b] # 0x181bc1628
000000018030078D call      0x18024fef0
0000000180300792 lea       rcx, [rip + 0x18c13b7] # 0x181bc1b50
0000000180300799 call      0x18024fef0
000000018030079E lea       rcx, [rip + 0x18a44f3] # 0x181ba4c98
00000001803007A5 call      0x18024fef0
00000001803007AA lea       rcx, [rip + 0x18a4547] # 0x181ba4cf8
00000001803007B1 call      0x18024fef0
00000001803007B6 lea       rcx, [rip + 0x18a459b] # 0x181ba4d58
00000001803007BD call      0x18024fef0
00000001803007C2 lea       rcx, [rip + 0x18a560f] # 0x181ba5dd8
00000001803007C9 call      0x18024fef0
00000001803007CE lea       rcx, [rip + 0x18a983b] # 0x181baa010
00000001803007D5 call      0x18024fef0
00000001803007DA lea       rcx, [rip + 0x18a97d7] # 0x181ba9fb8
00000001803007E1 call      0x18024fef0
00000001803007E6 lea       rcx, [rip + 0x18c6263] # 0x181bc6a50
00000001803007ED call      0x18024fef0
00000001803007F2 lea       rcx, [rip + 0x18c62af] # 0x181bc6aa8
00000001803007F9 call      0x18024fef0
00000001803007FE mov       byte ptr [rip + 0x19e4323], 1 # 0x181ce4b28
0000000180300805 xorps     xmm0, xmm0
0000000180300808 xor       eax, eax
000000018030080A movups    xmmword ptr [rsp + 0x50], xmm0
000000018030080F mov       qword ptr [rsp + 0x60], rax
0000000180300814 mov       rax, qword ptr [rip + 0x189d9ad] # 0x181b9e1c8
000000018030081B cmp       dword ptr [rax + 0xe0], 0
0000000180300822 jne       0x180300833
0000000180300824 mov       rcx, rax
0000000180300827 call      0x1802501e0
000000018030082C mov       rax, qword ptr [rip + 0x189d995] # 0x181b9e1c8
0000000180300833 mov       rax, qword ptr [rax + 0xb8]
000000018030083A mov       rcx, qword ptr [rax]
000000018030083D test      rcx, rcx
0000000180300840 je        0x180300d51
0000000180300846 inc       dword ptr [rcx + 0x1c]
0000000180300849 mov       r8d, dword ptr [rcx + 0x18]
000000018030084D mov       dword ptr [rcx + 0x18], 0
0000000180300854 test      r8d, r8d
0000000180300857 jle       0x180300867
0000000180300859 xor       r9d, r9d
000000018030085C xor       edx, edx
000000018030085E mov       rcx, qword ptr [rcx + 0x10]
0000000180300862 call      0x180cb86f0
0000000180300867 movss     xmm6, dword ptr [rip + 0x12a71a1] # 0x1815a7a10
000000018030086F movss     dword ptr [rsp + 0x28], xmm6
0000000180300875 xorps     xmm7, xmm7
0000000180300878 movss     dword ptr [rsp + 0x2c], xmm7
000000018030087E mov       dword ptr [rsp + 0x110], 0x3f800000
0000000180300889 mov       dword ptr [rsp + 0x118], 0
0000000180300894 movaps    xmm8, xmm6
0000000180300898 movss     dword ptr [rsp + 0x30], xmm6
000000018030089E xorps     xmm9, xmm9
00000001803008A2 movss     dword ptr [rsp + 0x34], xmm9
00000001803008A9 mov       dword ptr [rsp + 0x20], 0x3f800000
00000001803008B1 mov       dword ptr [rsp + 0x24], 0
00000001803008B9 mov       rax, qword ptr [r14 + 0x20]
00000001803008BD test      rax, rax
00000001803008C0 je        0x180300d51
00000001803008C6 mov       rax, qword ptr [rax + 0x28]
00000001803008CA test      rax, rax
00000001803008CD je        0x180300d51
00000001803008D3 mov       rdx, qword ptr [rax + 0x68]
00000001803008D7 test      rdx, rdx
00000001803008DA je        0x180300d51
00000001803008E0 mov       r8, qword ptr [rip + 0x18a4471] # 0x181ba4d58
00000001803008E7 lea       rcx, [rsp + 0x38]
00000001803008EC call      0x1808197f0
00000001803008F1 movups    xmm0, xmmword ptr [rsp + 0x38]
00000001803008F6 movups    xmmword ptr [rsp + 0x68], xmm0
00000001803008FB movsd     xmm1, qword ptr [rsp + 0x48]
0000000180300901 movsd     qword ptr [rsp + 0x78], xmm1
0000000180300907 mov       qword ptr [rsp + 0x38], 0
0000000180300910 lea       rbx, [rsp + 0x68]
0000000180300915 mov       qword ptr [rsp + 0x40], rbx
000000018030091A lea       r15, [rip - 0x300921] # 0x180000000
0000000180300921 mov       rdx, qword ptr [rip + 0x18c0ca8] # 0x181bc15d0
0000000180300928 lea       rcx, [rsp + 0x68]
000000018030092D call      0x180669280
0000000180300932 test      al, al
0000000180300934 je        0x180300ab3
000000018030093A mov       rdi, qword ptr [rsp + 0x78]
000000018030093F test      rdi, rdi
0000000180300942 je        0x180300d2f
0000000180300948 cmp       byte ptr [rdi + 0x18], 0
000000018030094C jne       0x180300954
000000018030094E cmp       byte ptr [rdi + 0x19], 0
0000000180300952 je        0x180300921
0000000180300954 mov       edx, dword ptr [rdi + 0x10]
0000000180300957 lea       eax, [rdx - 7]
000000018030095A cmp       eax, 9
000000018030095D ja        0x180300a64
0000000180300963 cdqe      
0000000180300965 mov       ecx, dword ptr [r15 + rax*4 + 0x300d58]
000000018030096D add       rcx, r15
0000000180300970 jmp       rcx
0000000180300972 xor       edx, edx
0000000180300974 mov       rcx, rdi
0000000180300977 call      0x180395070
000000018030097C addss     xmm6, xmm0
0000000180300980 movss     dword ptr [rsp + 0x28], xmm6
0000000180300986 jmp       0x180300921
0000000180300988 xor       edx, edx
000000018030098A mov       rcx, rdi
000000018030098D call      0x180395070
0000000180300992 addss     xmm7, xmm0
0000000180300996 movss     dword ptr [rsp + 0x2c], xmm7
000000018030099C jmp       0x180300921
000000018030099E movss     xmm10, dword ptr [rsp + 0x110]
00000001803009A8 xor       edx, edx
00000001803009AA mov       rcx, rdi
00000001803009AD call      0x180395070
00000001803009B2 addss     xmm0, xmm10
00000001803009B7 movss     dword ptr [rsp + 0x110], xmm0
00000001803009C0 jmp       0x180300921
00000001803009C5 movss     xmm10, dword ptr [rsp + 0x118]
00000001803009CF xor       edx, edx
00000001803009D1 mov       rcx, rdi
00000001803009D4 call      0x180395070
00000001803009D9 addss     xmm0, xmm10
00000001803009DE movss     dword ptr [rsp + 0x118], xmm0
00000001803009E7 jmp       0x180300921
00000001803009EC xor       edx, edx
00000001803009EE mov       rcx, rdi
00000001803009F1 call      0x180395070
00000001803009F6 addss     xmm8, xmm0
00000001803009FB movss     dword ptr [rsp + 0x30], xmm8
0000000180300A02 jmp       0x180300921
0000000180300A07 xor       edx, edx
0000000180300A09 mov       rcx, rdi
0000000180300A0C call      0x180395070
0000000180300A11 addss     xmm9, xmm0
0000000180300A16 movss     dword ptr [rsp + 0x34], xmm9
0000000180300A1D jmp       0x180300921
0000000180300A22 movss     xmm10, dword ptr [rsp + 0x20]
0000000180300A29 xor       edx, edx
0000000180300A2B mov       rcx, rdi
0000000180300A2E call      0x180395070
0000000180300A33 addss     xmm0, xmm10
0000000180300A38 movss     dword ptr [rsp + 0x20], xmm0
0000000180300A3E jmp       0x180300921
0000000180300A43 movss     xmm10, dword ptr [rsp + 0x24]
0000000180300A4A xor       edx, edx
0000000180300A4C mov       rcx, rdi
0000000180300A4F call      0x180395070
0000000180300A54 addss     xmm0, xmm10
0000000180300A59 movss     dword ptr [rsp + 0x24], xmm0
0000000180300A5F jmp       0x180300921
0000000180300A64 cmp       edx, 0x1a
0000000180300A67 jne       0x180300921
0000000180300A6D mov       rax, qword ptr [rip + 0x189d754] # 0x181b9e1c8
0000000180300A74 cmp       dword ptr [rax + 0xe0], 0
0000000180300A7B jne       0x180300a8c
0000000180300A7D mov       rcx, rax
0000000180300A80 call      0x1802501e0
0000000180300A85 mov       rax, qword ptr [rip + 0x189d73c] # 0x181b9e1c8
0000000180300A8C mov       rax, qword ptr [rax + 0xb8]
0000000180300A93 mov       rcx, qword ptr [rax]
0000000180300A96 test      rcx, rcx
0000000180300A99 je        0x180300d29
0000000180300A9F mov       r8, qword ptr [rip + 0x18a41f2] # 0x181ba4c98
0000000180300AA6 mov       rdx, rdi
0000000180300AA9 call      0x180002e70
0000000180300AAE jmp       0x180300921
0000000180300AB3 mov       rdx, qword ptr [rip + 0x18c0abe] # 0x181bc1578
0000000180300ABA mov       rcx, rbx
0000000180300ABD call      0x180302170
0000000180300AC2 jmp       0x180300b05
0000000180300AC4 mov       rdx, qword ptr [rip + 0x18c0aad] # 0x181bc1578
0000000180300ACB mov       rcx, qword ptr [rsp + 0x40]
0000000180300AD0 call      0x180302170
0000000180300AD5 mov       rcx, qword ptr [rsp + 0x38]
0000000180300ADA test      rcx, rcx
0000000180300ADD jne       0x180300d35
0000000180300AE3 mov       r14, qword ptr [rsp + 0x100]
0000000180300AEB movss     xmm6, dword ptr [rsp + 0x28]
0000000180300AF1 movss     xmm7, dword ptr [rsp + 0x2c]
0000000180300AF7 movss     xmm8, dword ptr [rsp + 0x30]
0000000180300AFE movss     xmm9, dword ptr [rsp + 0x34]
0000000180300B05 xor       edx, edx
0000000180300B07 lea       rcx, [rsp + 0x110]
0000000180300B0F call      0x180cc5b50
0000000180300B14 xor       r8d, r8d
0000000180300B17 mov       rdx, rax
0000000180300B1A mov       rcx, qword ptr [rip + 0x18a94ef] # 0x181baa010
0000000180300B21 call      0x180b74270
0000000180300B26 mov       rbx, rax
0000000180300B29 mov       rcx, qword ptr [rip + 0x189a218] # 0x181b9ad48
0000000180300B30 cmp       dword ptr [rcx + 0xe0], 0
0000000180300B37 jne       0x180300b3e
0000000180300B39 call      0x1802501e0
0000000180300B3E xor       edx, edx
0000000180300B40 mov       rcx, rbx
0000000180300B43 call      0x1812e68f0
0000000180300B48 xor       edx, edx
0000000180300B4A lea       rcx, [rsp + 0x118]
0000000180300B52 call      0x180cc5b50
0000000180300B57 xor       r8d, r8d
0000000180300B5A mov       rdx, rax
0000000180300B5D mov       rcx, qword ptr [rip + 0x18a9454] # 0x181ba9fb8
0000000180300B64 call      0x180b74270
0000000180300B69 xor       edx, edx
0000000180300B6B mov       rcx, rax
0000000180300B6E call      0x1812e68f0
0000000180300B73 xor       edx, edx
0000000180300B75 lea       rcx, [rsp + 0x20]
0000000180300B7A call      0x180cc5b50
0000000180300B7F xor       r8d, r8d
0000000180300B82 mov       rdx, rax
0000000180300B85 mov       rcx, qword ptr [rip + 0x18c5f1c] # 0x181bc6aa8
0000000180300B8C call      0x180b74270
0000000180300B91 xor       edx, edx
0000000180300B93 mov       rcx, rax
0000000180300B96 call      0x1812e68f0
0000000180300B9B xor       edx, edx
0000000180300B9D lea       rcx, [rsp + 0x24]
0000000180300BA2 call      0x180cc5b50
0000000180300BA7 xor       r8d, r8d
0000000180300BAA mov       rdx, rax
0000000180300BAD mov       rcx, qword ptr [rip + 0x18c5e9c] # 0x181bc6a50
0000000180300BB4 call      0x180b74270
0000000180300BB9 xor       edx, edx
0000000180300BBB mov       rcx, rax
0000000180300BBE call      0x1812e68f0
0000000180300BC3 mov       rdx, qword ptr [r14 + 0x30]
0000000180300BC7 test      rdx, rdx
0000000180300BCA je        0x180300d51
0000000180300BD0 mov       r8, qword ptr [rip + 0x18a5201] # 0x181ba5dd8
0000000180300BD7 lea       rcx, [rsp + 0x38]
0000000180300BDC call      0x1808197f0
0000000180300BE1 movups    xmm0, xmmword ptr [rsp + 0x38]
0000000180300BE6 movups    xmmword ptr [rsp + 0x50], xmm0
0000000180300BEB movsd     xmm1, qword ptr [rsp + 0x48]
0000000180300BF1 movsd     qword ptr [rsp + 0x60], xmm1
0000000180300BF7 mov       qword ptr [rsp + 0x38], 0
0000000180300C00 lea       rbx, [rsp + 0x50]
0000000180300C05 mov       qword ptr [rsp + 0x40], rbx
0000000180300C0A nop       word ptr [rax + rax]
0000000180300C10 mov       rdx, qword ptr [rip + 0x18c0ee1] # 0x181bc1af8
0000000180300C17 lea       rcx, [rsp + 0x50]
0000000180300C1C call      0x180669280
0000000180300C21 test      al, al
0000000180300C23 je        0x180300ccf
0000000180300C29 mov       rdx, qword ptr [rsp + 0x60]
0000000180300C2E test      rdx, rdx
0000000180300C31 je        0x180300d45
0000000180300C37 cmp       dword ptr [rdx + 0x28], -1
0000000180300C3B je        0x180300c7a
0000000180300C3D movss     xmm0, dword ptr [rdx + 0x48]
0000000180300C42 mulss     xmm0, xmm6
0000000180300C46 movss     dword ptr [rdx + 0x48], xmm0
0000000180300C4B test      rdx, rdx
0000000180300C4E je        0x180300d3b
0000000180300C54 addss     xmm0, xmm7
0000000180300C58 movss     dword ptr [rdx + 0x48], xmm0
0000000180300C5D movd      xmm0, dword ptr [rdx + 0x5c]
0000000180300C62 cvtdq2ps  xmm0, xmm0
0000000180300C65 mulss     xmm0, xmm8
0000000180300C6A cvttss2si ecx, xmm0
0000000180300C6E cvttss2si eax, xmm9
0000000180300C73 add       eax, ecx
0000000180300C75 mov       dword ptr [rdx + 0x5c], eax
0000000180300C78 jmp       0x180300c83
0000000180300C7A test      rdx, rdx
0000000180300C7D je        0x180300d40
0000000180300C83 cmp       dword ptr [rdx + 0x2c], -1
0000000180300C87 je        0x180300c10
0000000180300C89 movss     xmm0, dword ptr [rdx + 0x48]
0000000180300C8E mulss     xmm0, dword ptr [rsp + 0x110]
0000000180300C97 movss     dword ptr [rdx + 0x48], xmm0
0000000180300C9C addss     xmm0, dword ptr [rsp + 0x118]
0000000180300CA5 movss     dword ptr [rdx + 0x48], xmm0
0000000180300CAA movd      xmm0, dword ptr [rdx + 0x5c]
0000000180300CAF cvtdq2ps  xmm0, xmm0
0000000180300CB2 mulss     xmm0, dword ptr [rsp + 0x20]
0000000180300CB8 cvttss2si ecx, xmm0
0000000180300CBC mov       dword ptr [rdx + 0x5c], ecx
0000000180300CBF cvttss2si eax, dword ptr [rsp + 0x24]
0000000180300CC5 add       eax, ecx
0000000180300CC7 mov       dword ptr [rdx + 0x5c], eax
0000000180300CCA jmp       0x180300c10
0000000180300CCF mov       rdx, qword ptr [rip + 0x18c0dca] # 0x181bc1aa0
0000000180300CD6 mov       rcx, rbx
0000000180300CD9 call      0x180302170
0000000180300CDE jmp       0x180300cfb
0000000180300CE0 mov       rdx, qword ptr [rip + 0x18c0db9] # 0x181bc1aa0
0000000180300CE7 mov       rcx, qword ptr [rsp + 0x40]
0000000180300CEC call      0x180302170
0000000180300CF1 mov       rcx, qword ptr [rsp + 0x38]
0000000180300CF6 test      rcx, rcx
0000000180300CF9 jne       0x180300d4b
0000000180300CFB lea       r11, [rsp + 0xe0]
0000000180300D03 mov       rbx, qword ptr [r11 + 0x28]
0000000180300D07 movaps    xmm6, xmmword ptr [r11 - 0x10]
0000000180300D0C movaps    xmm7, xmmword ptr [r11 - 0x20]
0000000180300D11 movaps    xmm8, xmmword ptr [r11 - 0x30]
0000000180300D16 movaps    xmm9, xmmword ptr [r11 - 0x40]
0000000180300D1B movaps    xmm10, xmmword ptr [r11 - 0x50]
0000000180300D20 mov       rsp, r11
0000000180300D23 pop       r15
0000000180300D25 pop       r14
0000000180300D27 pop       rdi
0000000180300D28 ret       
0000000180300D29 call      0x180250150
0000000180300D2E nop       
0000000180300D2F call      0x180250150
0000000180300D34 nop       
0000000180300D35 call      0x180200e60
0000000180300D3A nop       
0000000180300D3B call      0x180250150
0000000180300D40 call      0x180250150
0000000180300D45 call      0x180250150
0000000180300D4A nop       
0000000180300D4B call      0x180200e60
0000000180300D50 int3      
0000000180300D51 call      0x180250150
0000000180300D56 int3      
0000000180300D57 nop       
0000000180300D58 jb        0x180300d63
0000000180300D5A xor       byte ptr [rax], al
0000000180300D5C mov       byte ptr [rcx], cl
0000000180300D5E xor       byte ptr [rax], al
0000000180300D60 sahf      
0000000180300D61 or        dword ptr [rax], esi
0000000180300D63 add       ch, al
0000000180300D65 or        dword ptr [rax], esi
0000000180300D67 add       byte ptr [rcx], ah
0000000180300D69 or        dword ptr [rax], esi
0000000180300D6B add       byte ptr [rcx], ah
0000000180300D6D or        dword ptr [rax], esi
0000000180300D6F add       ah, ch
0000000180300D71 or        dword ptr [rax], esi
0000000180300D73 add       byte ptr [rdi], al
0000000180300D75 or        dh, byte ptr [rax]
0000000180300D77 add       byte ptr [rdx], ah
0000000180300D79 or        dh, byte ptr [rax]
0000000180300D7B add       byte ptr [rbx + 0xa], al
0000000180300D7E xor       byte ptr [rax], al