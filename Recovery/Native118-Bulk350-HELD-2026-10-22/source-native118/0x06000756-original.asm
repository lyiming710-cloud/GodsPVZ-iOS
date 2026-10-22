00000001803B2AE0 push      rbx
00000001803B2AE2 push      rsi
00000001803B2AE3 push      r14
00000001803B2AE5 sub       rsp, 0xf0
00000001803B2AEC cmp       byte ptr [rip + 0x193246d], 0 # 0x181ce4f60
00000001803B2AF3 mov       rbx, rcx
00000001803B2AF6 jne       0x1803b2b5f
00000001803B2AF8 lea       rcx, [rip + 0x18105d9] # 0x181bc30d8
00000001803B2AFF call      0x18024fef0
00000001803B2B04 lea       rcx, [rip + 0x17f186d] # 0x181ba4378
00000001803B2B0B call      0x18024fef0
00000001803B2B10 lea       rcx, [rip + 0x17f1801] # 0x181ba4318
00000001803B2B17 call      0x18024fef0
00000001803B2B1C lea       rcx, [rip + 0x17ef195] # 0x181ba1cb8
00000001803B2B23 call      0x18024fef0
00000001803B2B28 lea       rcx, [rip + 0x1809b91] # 0x181bbc6c0
00000001803B2B2F call      0x18024fef0
00000001803B2B34 lea       rcx, [rip + 0x1805985] # 0x181bb84c0
00000001803B2B3B call      0x18024fef0
00000001803B2B40 lea       rcx, [rip + 0x17f64b9] # 0x181ba9000
00000001803B2B47 call      0x18024fef0
00000001803B2B4C lea       rcx, [rip + 0x1805a75] # 0x181bb85c8
00000001803B2B53 call      0x18024fef0
00000001803B2B58 mov       byte ptr [rip + 0x1932401], 1 # 0x181ce4f60
00000001803B2B5F mov       ecx, dword ptr [rbx + 0x24]
00000001803B2B62 xor       esi, esi
00000001803B2B64 mov       qword ptr [rsp + 0x118], rbp
00000001803B2B6C mov       r14d, esi
00000001803B2B6F mov       qword ptr [rsp + 0x128], rdi
00000001803B2B77 movaps    xmmword ptr [rsp + 0xe0], xmm6
00000001803B2B7F movaps    xmmword ptr [rsp + 0xd0], xmm7
00000001803B2B87 movaps    xmmword ptr [rsp + 0xc0], xmm8
00000001803B2B90 movaps    xmmword ptr [rsp + 0xb0], xmm9
00000001803B2B99 movaps    xmmword ptr [rsp + 0xa0], xmm10
00000001803B2BA2 test      ecx, ecx
00000001803B2BA4 je        0x1803b2cfb
00000001803B2BAA sub       ecx, 1
00000001803B2BAD je        0x1803b2c71
00000001803B2BB3 sub       ecx, 1
00000001803B2BB6 je        0x1803b2be4
00000001803B2BB8 cmp       ecx, 1
00000001803B2BBB jne       0x1803b2d51
00000001803B2BC1 mov       rcx, qword ptr [rip + 0x1809af8] # 0x181bbc6c0
00000001803B2BC8 mov       rdi, qword ptr [rbx + 0x28]
00000001803B2BCC cmp       dword ptr [rcx + 0xe0], esi
00000001803B2BD2 jne       0x1803b2bd9
00000001803B2BD4 call      0x1802501e0
00000001803B2BD9 mov       r9d, 0x1015
00000001803B2BDF jmp       0x1803b2d19
00000001803B2BE4 mov       rcx, qword ptr [rip + 0x1809ad5] # 0x181bbc6c0
00000001803B2BEB mov       rdi, qword ptr [rbx + 0x28]
00000001803B2BEF cmp       dword ptr [rcx + 0xe0], esi
00000001803B2BF5 jne       0x1803b2bfc
00000001803B2BF7 call      0x1802501e0
00000001803B2BFC mov       qword ptr [rsp + 0x40], rsi
00000001803B2C01 mov       edx, 0x5a
00000001803B2C06 mov       byte ptr [rsp + 0x38], 1
00000001803B2C0B mov       r9d, 0x1045
00000001803B2C11 mov       dword ptr [rsp + 0x30], 1
00000001803B2C19 mov       r8d, 9
00000001803B2C1F mov       dword ptr [rsp + 0x28], 0x100
00000001803B2C27 mov       rcx, rdi
00000001803B2C2A mov       dword ptr [rsp + 0x20], 0x100
00000001803B2C32 call      0x181182650
00000001803B2C37 mov       r14, rax
00000001803B2C3A test      rax, rax
00000001803B2C3D je        0x1803b30e6
00000001803B2C43 mov       rcx, qword ptr [rip + 0x1805876] # 0x181bb84c0
00000001803B2C4A xor       edx, edx
00000001803B2C4C mov       rdi, qword ptr [rax + 0x20]
00000001803B2C50 call      0x1812fba10
00000001803B2C55 test      rdi, rdi
00000001803B2C58 je        0x1803b30e6
00000001803B2C5E xor       r8d, r8d
00000001803B2C61 mov       rdx, rax
00000001803B2C64 mov       rcx, rdi
00000001803B2C67 call      0x1812f0980
00000001803B2C6C jmp       0x1803b2d51
00000001803B2C71 mov       rcx, qword ptr [rip + 0x1809a48] # 0x181bbc6c0
00000001803B2C78 mov       rdi, qword ptr [rbx + 0x28]
00000001803B2C7C cmp       dword ptr [rcx + 0xe0], esi
00000001803B2C82 jne       0x1803b2c89
00000001803B2C84 call      0x1802501e0
00000001803B2C89 mov       qword ptr [rsp + 0x40], rsi
00000001803B2C8E mov       edx, 0x5a
00000001803B2C93 mov       byte ptr [rsp + 0x38], 1
00000001803B2C98 mov       r9d, 0x1045
00000001803B2C9E mov       dword ptr [rsp + 0x30], 1
00000001803B2CA6 mov       r8d, 9
00000001803B2CAC mov       dword ptr [rsp + 0x28], 0x100
00000001803B2CB4 mov       rcx, rdi
00000001803B2CB7 mov       dword ptr [rsp + 0x20], 0x100
00000001803B2CBF call      0x181182650
00000001803B2CC4 mov       r14, rax
00000001803B2CC7 test      rax, rax
00000001803B2CCA je        0x1803b30e6
00000001803B2CD0 mov       rcx, qword ptr [rip + 0x18058f1] # 0x181bb85c8
00000001803B2CD7 xor       edx, edx
00000001803B2CD9 mov       rdi, qword ptr [rax + 0x20]
00000001803B2CDD call      0x1812fba10
00000001803B2CE2 test      rdi, rdi
00000001803B2CE5 je        0x1803b30e6
00000001803B2CEB xor       r8d, r8d
00000001803B2CEE mov       rdx, rax
00000001803B2CF1 mov       rcx, rdi
00000001803B2CF4 call      0x1812f0980
00000001803B2CF9 jmp       0x1803b2d51
00000001803B2CFB mov       rcx, qword ptr [rip + 0x18099be] # 0x181bbc6c0
00000001803B2D02 mov       rdi, qword ptr [rbx + 0x28]
00000001803B2D06 cmp       dword ptr [rcx + 0xe0], esi
00000001803B2D0C jne       0x1803b2d13
00000001803B2D0E call      0x1802501e0
00000001803B2D13 mov       r9d, 0x1045
00000001803B2D19 mov       qword ptr [rsp + 0x40], rsi
00000001803B2D1E mov       r8d, 9
00000001803B2D24 mov       byte ptr [rsp + 0x38], 1
00000001803B2D29 mov       edx, 0x5a
00000001803B2D2E mov       dword ptr [rsp + 0x30], 1
00000001803B2D36 mov       rcx, rdi
00000001803B2D39 mov       dword ptr [rsp + 0x28], 0x100
00000001803B2D41 mov       dword ptr [rsp + 0x20], 0x100
00000001803B2D49 call      0x181182650
00000001803B2D4E mov       r14, rax
00000001803B2D51 cmp       dword ptr [rbx + 0x20], esi
00000001803B2D54 jle       0x1803b309f
00000001803B2D5A movss     xmm7, dword ptr [rip + 0x11f4d66] # 0x1815a7ac8
00000001803B2D62 xorps     xmm6, xmm6
00000001803B2D65 movss     xmm10, dword ptr [rip + 0x11f52ae] # 0x1815a801c
00000001803B2D6E movss     xmm8, dword ptr [rip + 0x11f5215] # 0x1815a7f8c
00000001803B2D77 movss     xmm9, dword ptr [rip + 0x11f52a0] # 0x1815a8020
00000001803B2D80 mov       eax, dword ptr [rbx + 0x24]
00000001803B2D83 cmp       eax, 3
00000001803B2D86 ja        0x1803b2eff
00000001803B2D8C mov       rcx, qword ptr [rip + 0x17eef25] # 0x181ba1cb8
00000001803B2D93 call      0x180250100
00000001803B2D98 xor       edx, edx
00000001803B2D9A mov       rcx, rax
00000001803B2D9D mov       rdi, rax
00000001803B2DA0 call      0x18131be60
00000001803B2DA5 test      rdi, rdi
00000001803B2DA8 je        0x1803b30e6
00000001803B2DAE xor       edx, edx
00000001803B2DB0 mov       rcx, rdi
00000001803B2DB3 call      0x18131c070
00000001803B2DB8 test      rax, rax
00000001803B2DBB je        0x1803b30e6
00000001803B2DC1 movaps    xmm0, xmm6
00000001803B2DC4 movss     dword ptr [rsp + 0x58], xmm6
00000001803B2DCA unpcklps  xmm0, xmm7
00000001803B2DCD lea       rdx, [rsp + 0x50]
00000001803B2DD2 xor       r8d, r8d
00000001803B2DD5 movsd     qword ptr [rsp + 0x50], xmm0
00000001803B2DDB mov       rcx, rax
00000001803B2DDE call      0x18132fbb0
00000001803B2DE3 mov       rdx, qword ptr [rip + 0x17f158e] # 0x181ba4378
00000001803B2DEA mov       rcx, rdi
00000001803B2DED call      0x18046d280
00000001803B2DF2 mov       rdi, rax
00000001803B2DF5 test      rax, rax
00000001803B2DF8 je        0x1803b30e6
00000001803B2DFE xor       r8d, r8d
00000001803B2E01 mov       rdx, r14
00000001803B2E04 mov       rcx, rax
00000001803B2E07 call      0x1811b9520
00000001803B2E0C xor       r8d, r8d
00000001803B2E0F movaps    xmm1, xmm10
00000001803B2E13 mov       rcx, rdi
00000001803B2E16 call      0x1811b9410
00000001803B2E1B mov       r9, qword ptr [rdi]
00000001803B2E1E mov       rcx, rdi
00000001803B2E21 mov       rdx, qword ptr [rip + 0x17f61d8] # 0x181ba9000
00000001803B2E28 mov       r8, qword ptr [r9 + 0x560]
00000001803B2E2F call      qword ptr [r9 + 0x558]
00000001803B2E36 xor       r8d, r8d
00000001803B2E39 mov       edx, 0x202
00000001803B2E3E mov       rcx, rdi
00000001803B2E41 call      0x1811b8c90
00000001803B2E46 mov       dword ptr [rsp + 0x110], 0xff00ffff
00000001803B2E51 mov       eax, dword ptr [rsp + 0x110]
00000001803B2E58 mov       ecx, eax
00000001803B2E5A shr       ecx, 8
00000001803B2E5D movzx     ecx, cl
00000001803B2E60 movd      xmm3, ecx
00000001803B2E64 mov       ecx, eax
00000001803B2E66 shr       ecx, 0x10
00000001803B2E69 movzx     ecx, cl
00000001803B2E6C shr       eax, 0x18
00000001803B2E6F cvtdq2ps  xmm3, xmm3
00000001803B2E72 movd      xmm1, ecx
00000001803B2E76 movd      xmm2, eax
00000001803B2E7A cvtdq2ps  xmm1, xmm1
00000001803B2E7D cvtdq2ps  xmm2, xmm2
00000001803B2E80 divss     xmm1, xmm8
00000001803B2E85 divss     xmm2, xmm8
00000001803B2E8A divss     xmm3, xmm8
00000001803B2E8F mov       rax, qword ptr [rdi]
00000001803B2E92 lea       rdx, [rsp + 0x90]
00000001803B2E9A mov       dword ptr [rsp + 0x70], 0x3f800000
00000001803B2EA2 mov       rcx, rdi
00000001803B2EA5 movaps    xmm0, xmmword ptr [rsp + 0x70]
00000001803B2EAA shufps    xmm0, xmm0, 0xe1
00000001803B2EAE mov       r8, qword ptr [rax + 0x2b0]
00000001803B2EB5 movss     xmm0, xmm3
00000001803B2EB9 shufps    xmm0, xmm0, 0xc6
00000001803B2EBD movss     xmm0, xmm1
00000001803B2EC1 shufps    xmm0, xmm0, 0x27
00000001803B2EC5 movss     xmm0, xmm2
00000001803B2EC9 shufps    xmm0, xmm0, 0x39
00000001803B2ECD movaps    xmmword ptr [rsp + 0x70], xmm0
00000001803B2ED2 movdqa    xmmword ptr [rsp + 0x90], xmm0
00000001803B2EDB call      qword ptr [rax + 0x2a8]
00000001803B2EE1 cmp       dword ptr [rbx + 0x24], 3
00000001803B2EE5 jne       0x1803b3094
00000001803B2EEB xor       r8d, r8d
00000001803B2EEE movaps    xmm1, xmm9
00000001803B2EF2 mov       rcx, rdi
00000001803B2EF5 call      0x1811b9410
00000001803B2EFA jmp       0x1803b3094
00000001803B2EFF cmp       eax, 4
00000001803B2F02 jne       0x1803b3094
00000001803B2F08 mov       rcx, qword ptr [rip + 0x17eeda9] # 0x181ba1cb8
00000001803B2F0F call      0x180250100
00000001803B2F14 xor       edx, edx
00000001803B2F16 mov       rcx, rax
00000001803B2F19 mov       rdi, rax
00000001803B2F1C call      0x18131be60
00000001803B2F21 test      rdi, rdi
00000001803B2F24 je        0x1803b30e6
00000001803B2F2A xor       edx, edx
00000001803B2F2C mov       rcx, rdi
00000001803B2F2F call      0x18131c070
00000001803B2F34 test      rax, rax
00000001803B2F37 je        0x1803b30e6
00000001803B2F3D movaps    xmm0, xmm6
00000001803B2F40 movss     dword ptr [rsp + 0x68], xmm6
00000001803B2F46 unpcklps  xmm0, xmm7
00000001803B2F49 lea       rdx, [rsp + 0x60]
00000001803B2F4E xor       r8d, r8d
00000001803B2F51 movsd     qword ptr [rsp + 0x60], xmm0
00000001803B2F57 mov       rcx, rax
00000001803B2F5A call      0x18132fbb0
00000001803B2F5F mov       rdx, qword ptr [rip + 0x17f13b2] # 0x181ba4318
00000001803B2F66 mov       rcx, rdi
00000001803B2F69 call      0x18046d280
00000001803B2F6E mov       rdi, rax
00000001803B2F71 test      rax, rax
00000001803B2F74 je        0x1803b30e6
00000001803B2F7A mov       rdx, qword ptr [rip + 0x1810157] # 0x181bc30d8
00000001803B2F81 mov       rcx, rax
00000001803B2F84 call      0x18042e9f0
00000001803B2F89 mov       rcx, qword ptr [rbx + 0x28]
00000001803B2F8D mov       rbp, rax
00000001803B2F90 test      rcx, rcx
00000001803B2F93 je        0x1803b30e6
00000001803B2F99 xor       edx, edx
00000001803B2F9B call      0x1813b9880
00000001803B2FA0 test      rbp, rbp
00000001803B2FA3 je        0x1803b30e6
00000001803B2FA9 xor       r8d, r8d
00000001803B2FAC mov       rdx, rax
00000001803B2FAF mov       rcx, rbp
00000001803B2FB2 call      0x1812fb060
00000001803B2FB7 mov       rdx, qword ptr [rbx + 0x28]
00000001803B2FBB xor       r8d, r8d
00000001803B2FBE mov       rcx, rdi
00000001803B2FC1 call      0x1813bb9e0
00000001803B2FC6 xor       r8d, r8d
00000001803B2FC9 mov       edx, 4
00000001803B2FCE mov       rcx, rdi
00000001803B2FD1 call      0x1813bb8c0
00000001803B2FD6 xor       r8d, r8d
00000001803B2FD9 mov       edx, 0x82
00000001803B2FDE mov       rcx, rdi
00000001803B2FE1 call      0x1813bb9a0
00000001803B2FE6 mov       dword ptr [rsp + 0x120], 0xff00ffff
00000001803B2FF1 mov       eax, dword ptr [rsp + 0x120]
00000001803B2FF8 mov       ecx, eax
00000001803B2FFA shr       ecx, 8
00000001803B2FFD movzx     ecx, cl
00000001803B3000 movd      xmm3, ecx
00000001803B3004 mov       ecx, eax
00000001803B3006 shr       ecx, 0x10
00000001803B3009 movzx     ecx, cl
00000001803B300C shr       eax, 0x18
00000001803B300F cvtdq2ps  xmm3, xmm3
00000001803B3012 movd      xmm1, ecx
00000001803B3016 movd      xmm2, eax
00000001803B301A cvtdq2ps  xmm1, xmm1
00000001803B301D cvtdq2ps  xmm2, xmm2
00000001803B3020 divss     xmm1, xmm8
00000001803B3025 divss     xmm2, xmm8
00000001803B302A divss     xmm3, xmm8
00000001803B302F mov       dword ptr [rsp + 0x80], 0x3f800000
00000001803B303A lea       rdx, [rsp + 0x90]
00000001803B3042 movups    xmm0, xmmword ptr [rsp + 0x80]
00000001803B304A xor       r8d, r8d
00000001803B304D mov       rcx, rdi
00000001803B3050 shufps    xmm0, xmm0, 0xe1
00000001803B3054 movss     xmm0, xmm3
00000001803B3058 shufps    xmm0, xmm0, 0xc6
00000001803B305C movss     xmm0, xmm1
00000001803B3060 shufps    xmm0, xmm0, 0x27
00000001803B3064 movss     xmm0, xmm2
00000001803B3068 shufps    xmm0, xmm0, 0x39
00000001803B306C movups    xmmword ptr [rsp + 0x80], xmm0
00000001803B3074 movdqa    xmmword ptr [rsp + 0x90], xmm0
00000001803B307D call      0x1813bb950
00000001803B3082 mov       rdx, qword ptr [rip + 0x17f5f77] # 0x181ba9000
00000001803B3089 xor       r8d, r8d
00000001803B308C mov       rcx, rdi
00000001803B308F call      0x1813bba30
00000001803B3094 inc       esi
00000001803B3096 cmp       esi, dword ptr [rbx + 0x20]
00000001803B3099 jl        0x1803b2d80
00000001803B309F movaps    xmm10, xmmword ptr [rsp + 0xa0]
00000001803B30A8 movaps    xmm9, xmmword ptr [rsp + 0xb0]
00000001803B30B1 movaps    xmm8, xmmword ptr [rsp + 0xc0]
00000001803B30BA movaps    xmm7, xmmword ptr [rsp + 0xd0]
00000001803B30C2 movaps    xmm6, xmmword ptr [rsp + 0xe0]
00000001803B30CA mov       rdi, qword ptr [rsp + 0x128]
00000001803B30D2 mov       rbp, qword ptr [rsp + 0x118]
00000001803B30DA add       rsp, 0xf0
00000001803B30E1 pop       r14
00000001803B30E3 pop       rsi
00000001803B30E4 pop       rbx
00000001803B30E5 ret       
00000001803B30E6 call      0x180250150
00000001803B30EB int3      
00000001803B30EC int3      
00000001803B30ED int3      
00000001803B30EE int3      
00000001803B30EF int3      