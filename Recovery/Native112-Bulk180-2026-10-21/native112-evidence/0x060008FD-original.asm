00000001803C4C70 push      rdi
00000001803C4C72 sub       rsp, 0x70
00000001803C4C76 cmp       byte ptr [rip + 0x192039d], 0 # 0x181ce501a
00000001803C4C7D mov       rdi, rcx
00000001803C4C80 jne       0x1803c4cc5
00000001803C4C82 lea       rcx, [rip + 0x17d847f] # 0x181b9d108
00000001803C4C89 call      0x18024fef0
00000001803C4C8E lea       rcx, [rip + 0x17fc4e3] # 0x181bc1178
00000001803C4C95 call      0x18024fef0
00000001803C4C9A lea       rcx, [rip + 0x17fc52f] # 0x181bc11d0
00000001803C4CA1 call      0x18024fef0
00000001803C4CA6 lea       rcx, [rip + 0x17fc57b] # 0x181bc1228
00000001803C4CAD call      0x18024fef0
00000001803C4CB2 lea       rcx, [rip + 0x17fc5c7] # 0x181bc1280
00000001803C4CB9 call      0x18024fef0
00000001803C4CBE mov       byte ptr [rip + 0x1920355], 1 # 0x181ce501a
00000001803C4CC5 mov       rax, qword ptr [rip + 0x17d843c] # 0x181b9d108
00000001803C4CCC cmp       dword ptr [rax + 0xe0], 0
00000001803C4CD3 jne       0x1803c4ce4
00000001803C4CD5 mov       rcx, rax
00000001803C4CD8 call      0x1802501e0
00000001803C4CDD mov       rax, qword ptr [rip + 0x17d8424] # 0x181b9d108
00000001803C4CE4 mov       rax, qword ptr [rax + 0xb8]
00000001803C4CEB mov       qword ptr [rsp + 0x80], rbx
00000001803C4CF3 mov       qword ptr [rsp + 0x88], rsi
00000001803C4CFB movaps    xmmword ptr [rsp + 0x60], xmm6
00000001803C4D00 mov       rcx, qword ptr [rax + 0x40]
00000001803C4D04 movaps    xmmword ptr [rsp + 0x50], xmm7
00000001803C4D09 movaps    xmmword ptr [rsp + 0x40], xmm8
00000001803C4D0F movaps    xmmword ptr [rsp + 0x30], xmm9
00000001803C4D15 movaps    xmmword ptr [rsp + 0x20], xmm10
00000001803C4D1B test      rcx, rcx
00000001803C4D1E je        0x1803c512c
00000001803C4D24 inc       dword ptr [rcx + 0x1c]
00000001803C4D27 xor       ebx, ebx
00000001803C4D29 mov       dword ptr [rcx + 0x18], ebx
00000001803C4D2C mov       rax, qword ptr [rip + 0x17d83d5] # 0x181b9d108
00000001803C4D33 mov       rcx, qword ptr [rax + 0xb8]
00000001803C4D3A mov       rcx, qword ptr [rcx + 0x40]
00000001803C4D3E test      rcx, rcx
00000001803C4D41 je        0x1803c512c
00000001803C4D47 mov       rdx, qword ptr [rip + 0x17fc4da] # 0x181bc1228
00000001803C4D4E call      0x18065baa0
00000001803C4D53 test      rdi, rdi
00000001803C4D56 je        0x1803c512c
00000001803C4D5C mov       ecx, dword ptr [rdi + 0x18]
00000001803C4D5F add       ecx, ecx
00000001803C4D61 cmp       eax, ecx
00000001803C4D63 jge       0x1803c4da9
00000001803C4D65 mov       rax, qword ptr [rip + 0x17d839c] # 0x181b9d108
00000001803C4D6C cmp       dword ptr [rax + 0xe0], ebx
00000001803C4D72 jne       0x1803c4d83
00000001803C4D74 mov       rcx, rax
00000001803C4D77 call      0x1802501e0
00000001803C4D7C mov       rax, qword ptr [rip + 0x17d8385] # 0x181b9d108
00000001803C4D83 mov       rax, qword ptr [rax + 0xb8]
00000001803C4D8A mov       rcx, qword ptr [rax + 0x40]
00000001803C4D8E test      rcx, rcx
00000001803C4D91 je        0x1803c512c
00000001803C4D97 mov       edx, dword ptr [rdi + 0x18]
00000001803C4D9A mov       r8, qword ptr [rip + 0x17fc4df] # 0x181bc1280
00000001803C4DA1 shl       edx, 2
00000001803C4DA4 call      0x180805920
00000001803C4DA9 mov       esi, dword ptr [rdi + 0x18]
00000001803C4DAC test      esi, esi
00000001803C4DAE jle       0x1803c50fa
00000001803C4DB4 movss     xmm10, dword ptr [rip + 0x11e3287] # 0x1815a8044
00000001803C4DBD nop       dword ptr [rax]
00000001803C4DC0 cmp       ebx, dword ptr [rdi + 0x18]
00000001803C4DC3 jae       0x1803c5132
00000001803C4DC9 xorps     xmm0, xmm0
00000001803C4DCC movsxd    rdx, ebx
00000001803C4DCF mov       ecx, dword ptr [rdi + rdx*4 + 0x20]
00000001803C4DD3 mov       eax, ecx
00000001803C4DD5 shr       eax, 0x10
00000001803C4DD8 cvtsi2sd  xmm0, rax
00000001803C4DDD movzx     eax, cx
00000001803C4DE0 lea       rcx, [rdx + 1]
00000001803C4DE4 cvtpd2ps  xmm6, xmm0
00000001803C4DE8 xorps     xmm0, xmm0
00000001803C4DEB cvtsi2sd  xmm0, rax
00000001803C4DF0 divss     xmm6, xmm10
00000001803C4DF5 cvtpd2ps  xmm7, xmm0
00000001803C4DF9 divss     xmm7, xmm10
00000001803C4DFE cmp       ecx, dword ptr [rdi + 0x18]
00000001803C4E01 jae       0x1803c5132
00000001803C4E07 mov       ecx, dword ptr [rdi + rcx*4 + 0x20]
00000001803C4E0B xorps     xmm0, xmm0
00000001803C4E0E mov       eax, ecx
00000001803C4E10 shr       eax, 0x10
00000001803C4E13 cvtsi2sd  xmm0, rax
00000001803C4E18 movzx     eax, cx
00000001803C4E1B cvtpd2ps  xmm8, xmm0
00000001803C4E20 xorps     xmm0, xmm0
00000001803C4E23 cvtsi2sd  xmm0, rax
00000001803C4E28 mov       rax, qword ptr [rip + 0x17d82d9] # 0x181b9d108
00000001803C4E2F cmp       dword ptr [rax + 0xe0], 0
00000001803C4E36 cvtpd2ps  xmm9, xmm0
00000001803C4E3B divss     xmm8, xmm10
00000001803C4E40 divss     xmm9, xmm10
00000001803C4E45 jne       0x1803c4e56
00000001803C4E47 mov       rcx, rax
00000001803C4E4A call      0x1802501e0
00000001803C4E4F mov       rax, qword ptr [rip + 0x17d82b2] # 0x181b9d108
00000001803C4E56 mov       rax, qword ptr [rax + 0xb8]
00000001803C4E5D movss     dword ptr [rax + 0x20], xmm6
00000001803C4E62 mov       rax, qword ptr [rip + 0x17d829f] # 0x181b9d108
00000001803C4E69 mov       rcx, qword ptr [rax + 0xb8]
00000001803C4E70 movss     dword ptr [rcx + 0x24], xmm7
00000001803C4E75 mov       rax, qword ptr [rip + 0x17d828c] # 0x181b9d108
00000001803C4E7C mov       rcx, qword ptr [rax + 0xb8]
00000001803C4E83 movss     dword ptr [rcx + 0x28], xmm8
00000001803C4E89 mov       rax, qword ptr [rip + 0x17d8278] # 0x181b9d108
00000001803C4E90 mov       rcx, qword ptr [rax + 0xb8]
00000001803C4E97 movss     dword ptr [rcx + 0x2c], xmm7
00000001803C4E9C mov       rax, qword ptr [rip + 0x17d8265] # 0x181b9d108
00000001803C4EA3 mov       rcx, qword ptr [rax + 0xb8]
00000001803C4EAA movss     dword ptr [rcx + 0x30], xmm8
00000001803C4EB0 mov       rax, qword ptr [rip + 0x17d8251] # 0x181b9d108
00000001803C4EB7 mov       rcx, qword ptr [rax + 0xb8]
00000001803C4EBE movss     dword ptr [rcx + 0x34], xmm9
00000001803C4EC4 mov       rax, qword ptr [rip + 0x17d823d] # 0x181b9d108
00000001803C4ECB mov       rcx, qword ptr [rax + 0xb8]
00000001803C4ED2 movss     dword ptr [rcx + 0x38], xmm6
00000001803C4ED7 mov       rax, qword ptr [rip + 0x17d822a] # 0x181b9d108
00000001803C4EDE mov       rcx, qword ptr [rax + 0xb8]
00000001803C4EE5 movss     dword ptr [rcx + 0x3c], xmm9
00000001803C4EEB mov       rcx, qword ptr [rip + 0x17d8216] # 0x181b9d108
00000001803C4EF2 mov       rax, qword ptr [rcx + 0xb8]
00000001803C4EF9 mov       r9, qword ptr [rax + 0x40]
00000001803C4EFD movss     xmm0, dword ptr [rax + 0x20]
00000001803C4F02 movss     xmm1, dword ptr [rax + 0x24]
00000001803C4F07 test      r9, r9
00000001803C4F0A je        0x1803c512c
00000001803C4F10 mov       r8, qword ptr [rip + 0x17fc261] # 0x181bc1178
00000001803C4F17 inc       dword ptr [r9 + 0x1c]
00000001803C4F1B mov       rdx, qword ptr [r9 + 0x10]
00000001803C4F1F movsxd    rcx, dword ptr [r9 + 0x18]
00000001803C4F23 test      rdx, rdx
00000001803C4F26 je        0x1803c512c
00000001803C4F2C cmp       ecx, dword ptr [rdx + 0x18]
00000001803C4F2F jb        0x1803c4f52
00000001803C4F31 mov       rax, qword ptr [r8 + 0x20]
00000001803C4F35 mov       rcx, r9
00000001803C4F38 unpcklps  xmm0, xmm1
00000001803C4F3B movq      rdx, xmm0
00000001803C4F40 mov       r8, qword ptr [rax + 0xc0]
00000001803C4F47 mov       r8, qword ptr [r8 + 0x70]
00000001803C4F4B call      0x18087a400
00000001803C4F50 jmp       0x1803c4f6e
00000001803C4F52 lea       eax, [rcx + 1]
00000001803C4F55 mov       dword ptr [r9 + 0x18], eax
00000001803C4F59 cmp       ecx, dword ptr [rdx + 0x18]
00000001803C4F5C jae       0x1803c5132
00000001803C4F62 movss     dword ptr [rdx + rcx*8 + 0x20], xmm0
00000001803C4F68 movss     dword ptr [rdx + rcx*8 + 0x24], xmm1
00000001803C4F6E mov       rcx, qword ptr [rip + 0x17d8193] # 0x181b9d108
00000001803C4F75 mov       rax, qword ptr [rcx + 0xb8]
00000001803C4F7C mov       r9, qword ptr [rax + 0x40]
00000001803C4F80 movss     xmm0, dword ptr [rax + 0x28]
00000001803C4F85 movss     xmm1, dword ptr [rax + 0x2c]
00000001803C4F8A test      r9, r9
00000001803C4F8D je        0x1803c512c
00000001803C4F93 mov       r8, qword ptr [rip + 0x17fc1de] # 0x181bc1178
00000001803C4F9A inc       dword ptr [r9 + 0x1c]
00000001803C4F9E mov       rcx, qword ptr [r9 + 0x10]
00000001803C4FA2 movsxd    rdx, dword ptr [r9 + 0x18]
00000001803C4FA6 test      rcx, rcx
00000001803C4FA9 je        0x1803c512c
00000001803C4FAF cmp       edx, dword ptr [rcx + 0x18]
00000001803C4FB2 jb        0x1803c4fd5
00000001803C4FB4 mov       rax, qword ptr [r8 + 0x20]
00000001803C4FB8 mov       rcx, r9
00000001803C4FBB unpcklps  xmm0, xmm1
00000001803C4FBE movq      rdx, xmm0
00000001803C4FC3 mov       r8, qword ptr [rax + 0xc0]
00000001803C4FCA mov       r8, qword ptr [r8 + 0x70]
00000001803C4FCE call      0x18087a400
00000001803C4FD3 jmp       0x1803c4ff1
00000001803C4FD5 lea       eax, [rdx + 1]
00000001803C4FD8 mov       dword ptr [r9 + 0x18], eax
00000001803C4FDC cmp       edx, dword ptr [rcx + 0x18]
00000001803C4FDF jae       0x1803c5132
00000001803C4FE5 movss     dword ptr [rcx + rdx*8 + 0x20], xmm0
00000001803C4FEB movss     dword ptr [rcx + rdx*8 + 0x24], xmm1
00000001803C4FF1 mov       rcx, qword ptr [rip + 0x17d8110] # 0x181b9d108
00000001803C4FF8 mov       rax, qword ptr [rcx + 0xb8]
00000001803C4FFF mov       r9, qword ptr [rax + 0x40]
00000001803C5003 movss     xmm0, dword ptr [rax + 0x30]
00000001803C5008 movss     xmm1, dword ptr [rax + 0x34]
00000001803C500D test      r9, r9
00000001803C5010 je        0x1803c512c
00000001803C5016 mov       r8, qword ptr [rip + 0x17fc15b] # 0x181bc1178
00000001803C501D inc       dword ptr [r9 + 0x1c]
00000001803C5021 mov       rcx, qword ptr [r9 + 0x10]
00000001803C5025 movsxd    rdx, dword ptr [r9 + 0x18]
00000001803C5029 test      rcx, rcx
00000001803C502C je        0x1803c512c
00000001803C5032 cmp       edx, dword ptr [rcx + 0x18]
00000001803C5035 jb        0x1803c5058
00000001803C5037 mov       rax, qword ptr [r8 + 0x20]
00000001803C503B mov       rcx, r9
00000001803C503E unpcklps  xmm0, xmm1
00000001803C5041 movq      rdx, xmm0
00000001803C5046 mov       r8, qword ptr [rax + 0xc0]
00000001803C504D mov       r8, qword ptr [r8 + 0x70]
00000001803C5051 call      0x18087a400
00000001803C5056 jmp       0x1803c5074
00000001803C5058 lea       eax, [rdx + 1]
00000001803C505B mov       dword ptr [r9 + 0x18], eax
00000001803C505F cmp       edx, dword ptr [rcx + 0x18]
00000001803C5062 jae       0x1803c5132
00000001803C5068 movss     dword ptr [rcx + rdx*8 + 0x20], xmm0
00000001803C506E movss     dword ptr [rcx + rdx*8 + 0x24], xmm1
00000001803C5074 mov       rcx, qword ptr [rip + 0x17d808d] # 0x181b9d108
00000001803C507B mov       rax, qword ptr [rcx + 0xb8]
00000001803C5082 mov       r9, qword ptr [rax + 0x40]
00000001803C5086 movss     xmm0, dword ptr [rax + 0x38]
00000001803C508B movss     xmm1, dword ptr [rax + 0x3c]
00000001803C5090 test      r9, r9
00000001803C5093 je        0x1803c512c
00000001803C5099 mov       r8, qword ptr [rip + 0x17fc0d8] # 0x181bc1178
00000001803C50A0 inc       dword ptr [r9 + 0x1c]
00000001803C50A4 mov       rcx, qword ptr [r9 + 0x10]
00000001803C50A8 movsxd    rdx, dword ptr [r9 + 0x18]
00000001803C50AC test      rcx, rcx
00000001803C50AF je        0x1803c512c
00000001803C50B1 cmp       edx, dword ptr [rcx + 0x18]
00000001803C50B4 jb        0x1803c50d7
00000001803C50B6 mov       rax, qword ptr [r8 + 0x20]
00000001803C50BA mov       rcx, r9
00000001803C50BD unpcklps  xmm0, xmm1
00000001803C50C0 movq      rdx, xmm0
00000001803C50C5 mov       r8, qword ptr [rax + 0xc0]
00000001803C50CC mov       r8, qword ptr [r8 + 0x70]
00000001803C50D0 call      0x18087a400
00000001803C50D5 jmp       0x1803c50ef
00000001803C50D7 lea       eax, [rdx + 1]
00000001803C50DA mov       dword ptr [r9 + 0x18], eax
00000001803C50DE cmp       edx, dword ptr [rcx + 0x18]
00000001803C50E1 jae       0x1803c5132
00000001803C50E3 movss     dword ptr [rcx + rdx*8 + 0x20], xmm0
00000001803C50E9 movss     dword ptr [rcx + rdx*8 + 0x24], xmm1
00000001803C50EF add       ebx, 2
00000001803C50F2 cmp       ebx, esi
00000001803C50F4 jl        0x1803c4dc0
00000001803C50FA movaps    xmm10, xmmword ptr [rsp + 0x20]
00000001803C5100 movaps    xmm9, xmmword ptr [rsp + 0x30]
00000001803C5106 movaps    xmm8, xmmword ptr [rsp + 0x40]
00000001803C510C movaps    xmm7, xmmword ptr [rsp + 0x50]
00000001803C5111 movaps    xmm6, xmmword ptr [rsp + 0x60]
00000001803C5116 mov       rsi, qword ptr [rsp + 0x88]
00000001803C511E mov       rbx, qword ptr [rsp + 0x80]
00000001803C5126 add       rsp, 0x70
00000001803C512A pop       rdi
00000001803C512B ret       
00000001803C512C call      0x180250150
00000001803C5131 int3      
00000001803C5132 call      0x180250140
00000001803C5137 int3      