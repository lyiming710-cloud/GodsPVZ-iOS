00000001803C1960 push      rbx
00000001803C1962 push      rdi
00000001803C1963 sub       rsp, 0x28
00000001803C1967 cmp       byte ptr [rip + 0x19235f0], 0 # 0x181ce4f5e
00000001803C196E mov       rdi, rcx
00000001803C1971 jne       0x1803c19ce
00000001803C1973 lea       rcx, [rip + 0x17e2abe] # 0x181ba4438
00000001803C197A call      0x18024fef0
00000001803C197F lea       rcx, [rip + 0x17e2932] # 0x181ba42b8
00000001803C1986 call      0x18024fef0
00000001803C198B lea       rcx, [rip + 0x17eeb6e] # 0x181bb0500
00000001803C1992 call      0x18024fef0
00000001803C1997 lea       rcx, [rip + 0x17ff0e2] # 0x181bc0a80
00000001803C199E call      0x18024fef0
00000001803C19A3 lea       rcx, [rip + 0x1804096] # 0x181bc5a40
00000001803C19AA call      0x18024fef0
00000001803C19AF lea       rcx, [rip + 0x17f7132] # 0x181bb8ae8
00000001803C19B6 call      0x18024fef0
00000001803C19BB lea       rcx, [rip + 0x17f7026] # 0x181bb89e8
00000001803C19C2 call      0x18024fef0
00000001803C19C7 mov       byte ptr [rip + 0x1923590], 1 # 0x181ce4f5e
00000001803C19CE mov       ecx, dword ptr [rdi + 0x10]
00000001803C19D1 mov       rbx, qword ptr [rdi + 0x20]
00000001803C19D5 mov       qword ptr [rsp + 0x50], rsi
00000001803C19DA mov       dword ptr [rsp + 0x40], 0
00000001803C19E2 test      ecx, ecx
00000001803C19E4 je        0x1803c1a1f
00000001803C19E6 sub       ecx, 1
00000001803C19E9 je        0x1803c1a09
00000001803C19EB xor       al, al
00000001803C19ED cmp       ecx, 1
00000001803C19F0 jne       0x1803c1dd7
00000001803C19F6 mov       dword ptr [rdi + 0x10], 0xffffffff
00000001803C19FD mov       rsi, qword ptr [rsp + 0x50]
00000001803C1A02 add       rsp, 0x28
00000001803C1A06 pop       rdi
00000001803C1A07 pop       rbx
00000001803C1A08 ret       
00000001803C1A09 mov       eax, dword ptr [rdi + 0x28]
00000001803C1A0C mov       dword ptr [rdi + 0x10], 0xffffffff
00000001803C1A13 mov       dword ptr [rsp + 0x40], eax
00000001803C1A17 lea       ecx, [rax + 1]
00000001803C1A1A jmp       0x1803c1c07
00000001803C1A1F mov       dword ptr [rdi + 0x10], 0xffffffff
00000001803C1A26 test      rbx, rbx
00000001803C1A29 je        0x1803c1e0b
00000001803C1A2F cmp       dword ptr [rbx + 0x20], 0
00000001803C1A33 je        0x1803c1af1
00000001803C1A39 cmp       dword ptr [rbx + 0x20], 1
00000001803C1A3D jne       0x1803c1c05
00000001803C1A43 xor       edx, edx
00000001803C1A45 mov       rcx, rbx
00000001803C1A48 call      0x1813044d0
00000001803C1A4D test      rax, rax
00000001803C1A50 je        0x1803c1e0b
00000001803C1A56 mov       rdx, qword ptr [rip + 0x17e285b] # 0x181ba42b8
00000001803C1A5D mov       rcx, rax
00000001803C1A60 call      0x18046d280
00000001803C1A65 lea       rcx, [rbx + 0x48]
00000001803C1A69 mov       qword ptr [rbx + 0x48], rax
00000001803C1A6D mov       rdx, rax
00000001803C1A70 call      0x18024f360
00000001803C1A75 mov       rcx, qword ptr [rip + 0x17eea84] # 0x181bb0500
00000001803C1A7C mov       rsi, qword ptr [rbx + 0x38]
00000001803C1A80 cmp       dword ptr [rcx + 0xe0], 0
00000001803C1A87 jne       0x1803c1a8e
00000001803C1A89 call      0x1802501e0
00000001803C1A8E xor       r8d, r8d
00000001803C1A91 xor       edx, edx
00000001803C1A93 mov       rcx, rsi
00000001803C1A96 call      0x18131f900
00000001803C1A9B test      al, al
00000001803C1A9D je        0x1803c1ab8
00000001803C1A9F mov       rcx, qword ptr [rbx + 0x48]
00000001803C1AA3 test      rcx, rcx
00000001803C1AA6 je        0x1803c1e0b
00000001803C1AAC mov       rdx, qword ptr [rbx + 0x38]
00000001803C1AB0 xor       r8d, r8d
00000001803C1AB3 call      0x181571700
00000001803C1AB8 mov       rcx, qword ptr [rbx + 0x48]
00000001803C1ABC test      rcx, rcx
00000001803C1ABF je        0x1803c1e0b
00000001803C1AC5 xor       r8d, r8d
00000001803C1AC8 mov       edx, 0x30
00000001803C1ACD call      0x181571640
00000001803C1AD2 mov       rcx, qword ptr [rbx + 0x48]
00000001803C1AD6 test      rcx, rcx
00000001803C1AD9 je        0x1803c1e0b
00000001803C1ADF xor       r8d, r8d
00000001803C1AE2 mov       edx, 4
00000001803C1AE7 call      0x1815715e0
00000001803C1AEC jmp       0x1803c1c05
00000001803C1AF1 xor       edx, edx
00000001803C1AF3 mov       rcx, rbx
00000001803C1AF6 call      0x1813044d0
00000001803C1AFB test      rax, rax
00000001803C1AFE je        0x1803c1e0b
00000001803C1B04 mov       rdx, qword ptr [rip + 0x17e292d] # 0x181ba4438
00000001803C1B0B mov       rcx, rax
00000001803C1B0E call      0x18046d280
00000001803C1B13 lea       rcx, [rbx + 0x40]
00000001803C1B17 mov       qword ptr [rbx + 0x40], rax
00000001803C1B1B mov       rdx, rax
00000001803C1B1E call      0x18024f360
00000001803C1B23 mov       rcx, qword ptr [rip + 0x17ee9d6] # 0x181bb0500
00000001803C1B2A mov       rsi, qword ptr [rbx + 0x30]
00000001803C1B2E cmp       dword ptr [rcx + 0xe0], 0
00000001803C1B35 jne       0x1803c1b3c
00000001803C1B37 call      0x1802501e0
00000001803C1B3C xor       r8d, r8d
00000001803C1B3F xor       edx, edx
00000001803C1B41 mov       rcx, rsi
00000001803C1B44 call      0x18131f900
00000001803C1B49 test      al, al
00000001803C1B4B je        0x1803c1b66
00000001803C1B4D mov       rcx, qword ptr [rbx + 0x40]
00000001803C1B51 test      rcx, rcx
00000001803C1B54 je        0x1803c1e0b
00000001803C1B5A mov       rdx, qword ptr [rbx + 0x30]
00000001803C1B5E xor       r8d, r8d
00000001803C1B61 call      0x1811b9520
00000001803C1B66 mov       rcx, qword ptr [rbx + 0x40]
00000001803C1B6A test      rcx, rcx
00000001803C1B6D je        0x1803c1e0b
00000001803C1B73 movss     xmm1, dword ptr [rip + 0x11e6499] # 0x1815a8014
00000001803C1B7B xor       r8d, r8d
00000001803C1B7E call      0x1811b9410
00000001803C1B83 mov       rcx, qword ptr [rbx + 0x40]
00000001803C1B87 test      rcx, rcx
00000001803C1B8A je        0x1803c1e0b
00000001803C1B90 xor       r8d, r8d
00000001803C1B93 mov       edx, 0x202
00000001803C1B98 call      0x1811b8c90
00000001803C1B9D mov       rcx, qword ptr [rbx + 0x40]
00000001803C1BA1 test      rcx, rcx
00000001803C1BA4 je        0x1803c1e0b
00000001803C1BAA xor       r8d, r8d
00000001803C1BAD mov       dl, 1
00000001803C1BAF call      0x1811b9050
00000001803C1BB4 mov       rax, qword ptr [rbx + 0x40]
00000001803C1BB8 test      rax, rax
00000001803C1BBB je        0x1803c1e0b
00000001803C1BC1 mov       rdx, qword ptr [rax + 0x100]
00000001803C1BC8 test      rdx, rdx
00000001803C1BCB je        0x1803c1e0b
00000001803C1BD1 mov       rdx, qword ptr [rdx + 0x20]
00000001803C1BD5 lea       rcx, [rbx + 0x50]
00000001803C1BD9 mov       qword ptr [rbx + 0x50], rdx
00000001803C1BDD call      0x18024f360
00000001803C1BE2 mov       rdx, qword ptr [rip + 0x17fee97] # 0x181bc0a80
00000001803C1BE9 mov       rcx, qword ptr [rip + 0x1803e50] # 0x181bc5a40
00000001803C1BF0 call      0x1804a6ab0
00000001803C1BF5 lea       rcx, [rbx + 0x58]
00000001803C1BF9 mov       qword ptr [rbx + 0x58], rax
00000001803C1BFD mov       rdx, rax
00000001803C1C00 call      0x18024f360
00000001803C1C05 xor       ecx, ecx
00000001803C1C07 mov       dword ptr [rdi + 0x28], ecx
00000001803C1C0A cmp       ecx, 0xf4240
00000001803C1C10 jg        0x1803c1de3
00000001803C1C16 test      rbx, rbx
00000001803C1C19 je        0x1803c1e0b
00000001803C1C1F cmp       dword ptr [rbx + 0x20], 0
00000001803C1C23 je        0x1803c1c93
00000001803C1C25 cmp       dword ptr [rbx + 0x20], 1
00000001803C1C29 jne       0x1803c1dbb
00000001803C1C2F mov       rbx, qword ptr [rbx + 0x48]
00000001803C1C33 mov       eax, 0x10624dd3
00000001803C1C38 imul      ecx
00000001803C1C3A sar       edx, 6
00000001803C1C3D mov       eax, edx
00000001803C1C3F shr       eax, 0x1f
00000001803C1C42 add       edx, eax
00000001803C1C44 imul      eax, edx, 0x3e8
00000001803C1C4A xor       edx, edx
00000001803C1C4C sub       ecx, eax
00000001803C1C4E mov       dword ptr [rsp + 0x40], ecx
00000001803C1C52 lea       rcx, [rsp + 0x40]
00000001803C1C57 call      0x180ca3820
00000001803C1C5C mov       rcx, qword ptr [rip + 0x17f6e85] # 0x181bb8ae8
00000001803C1C63 xor       r8d, r8d
00000001803C1C66 mov       rdx, rax
00000001803C1C69 call      0x180b74270
00000001803C1C6E test      rbx, rbx
00000001803C1C71 je        0x1803c1e0b
00000001803C1C77 mov       r9, qword ptr [rbx]
00000001803C1C7A mov       rdx, rax
00000001803C1C7D mov       rcx, rbx
00000001803C1C80 mov       r8, qword ptr [r9 + 0x5f0]
00000001803C1C87 call      qword ptr [r9 + 0x5e8]
00000001803C1C8E jmp       0x1803c1dbb
00000001803C1C93 mov       rsi, qword ptr [rbx + 0x40]
00000001803C1C97 mov       eax, 0x10624dd3
00000001803C1C9C imul      ecx
00000001803C1C9E sar       edx, 6
00000001803C1CA1 mov       eax, edx
00000001803C1CA3 shr       eax, 0x1f
00000001803C1CA6 add       edx, eax
00000001803C1CA8 imul      eax, edx, 0x3e8
00000001803C1CAE xor       edx, edx
00000001803C1CB0 sub       ecx, eax
00000001803C1CB2 mov       dword ptr [rsp + 0x40], ecx
00000001803C1CB6 lea       rcx, [rsp + 0x40]
00000001803C1CBB call      0x180ca3820
00000001803C1CC0 mov       rcx, qword ptr [rip + 0x17f6d21] # 0x181bb89e8
00000001803C1CC7 xor       r8d, r8d
00000001803C1CCA mov       rdx, rax
00000001803C1CCD call      0x180b74270
00000001803C1CD2 test      rsi, rsi
00000001803C1CD5 je        0x1803c1e0b
00000001803C1CDB mov       r9, qword ptr [rsi]
00000001803C1CDE mov       rdx, rax
00000001803C1CE1 mov       rcx, rsi
00000001803C1CE4 mov       r8, qword ptr [r9 + 0x560]
00000001803C1CEB call      qword ptr [r9 + 0x558]
00000001803C1CF2 mov       ecx, dword ptr [rdi + 0x28]
00000001803C1CF5 mov       eax, 0x10624dd3
00000001803C1CFA imul      ecx
00000001803C1CFC sar       edx, 6
00000001803C1CFF mov       eax, edx
00000001803C1D01 shr       eax, 0x1f
00000001803C1D04 add       edx, eax
00000001803C1D06 imul      eax, edx, 0x3e8
00000001803C1D0C sub       ecx, eax
00000001803C1D0E cmp       ecx, 0x3e7
00000001803C1D14 jne       0x1803c1dbb
00000001803C1D1A mov       rsi, qword ptr [rbx + 0x40]
00000001803C1D1E mov       rcx, rsi
00000001803C1D21 test      rsi, rsi
00000001803C1D24 je        0x1803c1e0b
00000001803C1D2A mov       rdx, qword ptr [rsi]
00000001803C1D2D mov       qword ptr [rsp + 0x48], rbp
00000001803C1D32 mov       qword ptr [rsp + 0x20], r14
00000001803C1D37 mov       rax, qword ptr [rdx + 0x568]
00000001803C1D3E mov       rdx, qword ptr [rdx + 0x570]
00000001803C1D45 call      rax
00000001803C1D47 mov       rcx, qword ptr [rip + 0x17ee7b2] # 0x181bb0500
00000001803C1D4E mov       rbp, rax
00000001803C1D51 mov       r14, qword ptr [rbx + 0x50]
00000001803C1D55 cmp       dword ptr [rcx + 0xe0], 0
00000001803C1D5C jne       0x1803c1d63
00000001803C1D5E call      0x1802501e0
00000001803C1D63 xor       r8d, r8d
00000001803C1D66 mov       rdx, r14
00000001803C1D69 mov       rcx, rbp
00000001803C1D6C call      0x18131f760
00000001803C1D71 mov       rcx, qword ptr [rbx + 0x40]
00000001803C1D75 mov       r14, qword ptr [rsp + 0x20]
00000001803C1D7A mov       rbp, qword ptr [rsp + 0x48]
00000001803C1D7F test      al, al
00000001803C1D81 je        0x1803c1d89
00000001803C1D83 mov       rbx, qword ptr [rbx + 0x58]
00000001803C1D87 jmp       0x1803c1d8d
00000001803C1D89 mov       rbx, qword ptr [rbx + 0x50]
00000001803C1D8D test      rcx, rcx
00000001803C1D90 je        0x1803c1e0b
00000001803C1D92 mov       rax, qword ptr [rcx]
00000001803C1D95 mov       rdx, rbx
00000001803C1D98 mov       r8, qword ptr [rax + 0x580]
00000001803C1D9F call      qword ptr [rax + 0x578]
00000001803C1DA5 mov       rax, qword ptr [rsi]
00000001803C1DA8 mov       rdx, rbx
00000001803C1DAB mov       rcx, rsi
00000001803C1DAE mov       r8, qword ptr [rax + 0x580]
00000001803C1DB5 call      qword ptr [rax + 0x578]
00000001803C1DBB lea       rcx, [rdi + 0x18]
00000001803C1DBF mov       qword ptr [rdi + 0x18], 0
00000001803C1DC7 xor       edx, edx
00000001803C1DC9 call      0x18024f360
00000001803C1DCE mov       al, 1
00000001803C1DD0 mov       dword ptr [rdi + 0x10], 1
00000001803C1DD7 mov       rsi, qword ptr [rsp + 0x50]
00000001803C1DDC add       rsp, 0x28
00000001803C1DE0 pop       rdi
00000001803C1DE1 pop       rbx
00000001803C1DE2 ret       
00000001803C1DE3 lea       rcx, [rdi + 0x18]
00000001803C1DE7 mov       qword ptr [rdi + 0x18], 0
00000001803C1DEF xor       edx, edx
00000001803C1DF1 call      0x18024f360
00000001803C1DF6 mov       rsi, qword ptr [rsp + 0x50]
00000001803C1DFB mov       al, 1
00000001803C1DFD mov       dword ptr [rdi + 0x10], 2
00000001803C1E04 add       rsp, 0x28
00000001803C1E08 pop       rdi
00000001803C1E09 pop       rbx
00000001803C1E0A ret       
00000001803C1E0B call      0x180250150
00000001803C1E10 int3      
00000001803C1E11 int3      
00000001803C1E12 int3      
00000001803C1E13 int3      
00000001803C1E14 int3      
00000001803C1E15 int3      
00000001803C1E16 int3      
00000001803C1E17 int3      
00000001803C1E18 int3      
00000001803C1E19 int3      
00000001803C1E1A int3      
00000001803C1E1B int3      
00000001803C1E1C int3      
00000001803C1E1D int3      
00000001803C1E1E int3      
00000001803C1E1F int3      