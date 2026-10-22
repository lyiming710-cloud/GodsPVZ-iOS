0000000180324C60 mov       qword ptr [rsp + 0x18], rbx
0000000180324C65 mov       qword ptr [rsp + 0x20], rsi
0000000180324C6A mov       qword ptr [rsp + 0x10], rdx
0000000180324C6F mov       qword ptr [rsp + 8], rcx
0000000180324C74 push      rdi
0000000180324C75 sub       rsp, 0x80
0000000180324C7C mov       rdi, rdx
0000000180324C7F mov       rsi, rcx
0000000180324C82 cmp       byte ptr [rip + 0x19bfef4], 0 # 0x181ce4b7d
0000000180324C89 jne       0x180324cfe
0000000180324C8B lea       rcx, [rip + 0x18a452e] # 0x181bc91c0
0000000180324C92 call      0x18024fef0
0000000180324C97 lea       rcx, [rip + 0x18a1d62] # 0x181bc6a00
0000000180324C9E call      0x18024fef0
0000000180324CA3 lea       rcx, [rip + 0x18a456e] # 0x181bc9218
0000000180324CAA call      0x18024fef0
0000000180324CAF lea       rcx, [rip + 0x18a1da2] # 0x181bc6a58
0000000180324CB6 call      0x18024fef0
0000000180324CBB lea       rcx, [rip + 0x18a1dee] # 0x181bc6ab0
0000000180324CC2 call      0x18024fef0
0000000180324CC7 lea       rcx, [rip + 0x18a459a] # 0x181bc9268
0000000180324CCE call      0x18024fef0
0000000180324CD3 lea       rcx, [rip + 0x1898476] # 0x181bbd150
0000000180324CDA call      0x18024fef0
0000000180324CDF lea       rcx, [rip + 0x188feba] # 0x181bb4ba0
0000000180324CE6 call      0x18024fef0
0000000180324CEB lea       rcx, [rip + 0x18984b6] # 0x181bbd1a8
0000000180324CF2 call      0x18024fef0
0000000180324CF7 mov       byte ptr [rip + 0x19bfe7f], 1 # 0x181ce4b7d
0000000180324CFE xorps     xmm0, xmm0
0000000180324D01 xor       eax, eax
0000000180324D03 movups    xmmword ptr [rsp + 0x48], xmm0
0000000180324D08 mov       qword ptr [rsp + 0x58], rax
0000000180324D0D mov       rdx, qword ptr [rsi + 0x10]
0000000180324D11 test      rdx, rdx
0000000180324D14 je        0x180324f69
0000000180324D1A mov       r8, qword ptr [rip + 0x1898487] # 0x181bbd1a8
0000000180324D21 lea       rcx, [rsp + 0x30]
0000000180324D26 call      0x1808197f0
0000000180324D2B movups    xmm0, xmmword ptr [rsp + 0x30]
0000000180324D30 movups    xmmword ptr [rsp + 0x60], xmm0
0000000180324D35 movsd     xmm1, qword ptr [rsp + 0x40]
0000000180324D3B movsd     qword ptr [rsp + 0x70], xmm1
0000000180324D41 mov       qword ptr [rsp + 0x30], 0
0000000180324D4A lea       rbx, [rsp + 0x60]
0000000180324D4F mov       qword ptr [rsp + 0x38], rbx
0000000180324D54 mov       rdx, qword ptr [rip + 0x18a44bd] # 0x181bc9218
0000000180324D5B lea       rcx, [rsp + 0x60]
0000000180324D60 call      0x180669280
0000000180324D65 test      al, al
0000000180324D67 je        0x180324dde
0000000180324D69 test      rdi, rdi
0000000180324D6C je        0x180324f47
0000000180324D72 mov       r8, qword ptr [rip + 0x18983d7] # 0x181bbd150
0000000180324D79 inc       dword ptr [rdi + 0x1c]
0000000180324D7C mov       rcx, qword ptr [rdi + 0x10]
0000000180324D80 movsxd    rdx, dword ptr [rdi + 0x18]
0000000180324D84 test      rcx, rcx
0000000180324D87 je        0x180324f42
0000000180324D8D cmp       edx, dword ptr [rcx + 0x18]
0000000180324D90 jb        0x180324db0
0000000180324D92 mov       rax, qword ptr [r8 + 0x20]
0000000180324D96 mov       r8, qword ptr [rax + 0xc0]
0000000180324D9D mov       r8, qword ptr [r8 + 0x70]
0000000180324DA1 mov       rdx, qword ptr [rsp + 0x70]
0000000180324DA6 mov       rcx, rdi
0000000180324DA9 call      0x180847ce0
0000000180324DAE jmp       0x180324d54
0000000180324DB0 lea       eax, [rdx + 1]
0000000180324DB3 mov       dword ptr [rdi + 0x18], eax
0000000180324DB6 mov       rax, qword ptr [rsp + 0x70]
0000000180324DBB cmp       edx, dword ptr [rcx + 0x18]
0000000180324DBE jae       0x180324f3c
0000000180324DC4 mov       qword ptr [rcx + rdx*8 + 0x20], rax
0000000180324DC9 lea       rcx, [rcx + rdx*8]
0000000180324DCD add       rcx, 0x20
0000000180324DD1 mov       rdx, rax
0000000180324DD4 call      0x18024f360
0000000180324DD9 jmp       0x180324d54
0000000180324DDE mov       rdx, qword ptr [rip + 0x18a43db] # 0x181bc91c0
0000000180324DE5 mov       rcx, rbx
0000000180324DE8 call      0x180302170
0000000180324DED jmp       0x180324e1e
0000000180324DEF mov       rdx, qword ptr [rip + 0x18a43ca] # 0x181bc91c0
0000000180324DF6 mov       rcx, qword ptr [rsp + 0x38]
0000000180324DFB call      0x180302170
0000000180324E00 mov       rcx, qword ptr [rsp + 0x30]
0000000180324E05 test      rcx, rcx
0000000180324E08 jne       0x180324f4d
0000000180324E0E mov       rdi, qword ptr [rsp + 0x98]
0000000180324E16 mov       rsi, qword ptr [rsp + 0x90]
0000000180324E1E mov       rdx, qword ptr [rsi + 0x18]
0000000180324E22 test      rdx, rdx
0000000180324E25 je        0x180324f69
0000000180324E2B mov       r8, qword ptr [rip + 0x188fd6e] # 0x181bb4ba0
0000000180324E32 lea       rcx, [rsp + 0x30]
0000000180324E37 call      0x1808197f0
0000000180324E3C movups    xmm0, xmmword ptr [rsp + 0x30]
0000000180324E41 movups    xmmword ptr [rsp + 0x48], xmm0
0000000180324E46 movsd     xmm1, qword ptr [rsp + 0x40]
0000000180324E4C movsd     qword ptr [rsp + 0x58], xmm1
0000000180324E52 mov       qword ptr [rsp + 0x30], 0
0000000180324E5B lea       rbx, [rsp + 0x48]
0000000180324E60 mov       qword ptr [rsp + 0x38], rbx
0000000180324E65 nop       word ptr [rax + rax]
0000000180324E70 mov       rdx, qword ptr [rip + 0x18a1be1] # 0x181bc6a58
0000000180324E77 lea       rcx, [rsp + 0x48]
0000000180324E7C call      0x180669280
0000000180324E81 test      al, al
0000000180324E83 je        0x180324efb
0000000180324E85 xor       edx, edx
0000000180324E87 mov       rcx, qword ptr [rsp + 0x58]
0000000180324E8C call      0x180387010
0000000180324E91 mov       rdx, rax
0000000180324E94 test      rdi, rdi
0000000180324E97 je        0x180324f5d
0000000180324E9D mov       r9, qword ptr [rip + 0x18982ac] # 0x181bbd150
0000000180324EA4 inc       dword ptr [rdi + 0x1c]
0000000180324EA7 mov       rcx, qword ptr [rdi + 0x10]
0000000180324EAB movsxd    r8, dword ptr [rdi + 0x18]
0000000180324EAF test      rcx, rcx
0000000180324EB2 je        0x180324f58
0000000180324EB8 cmp       r8d, dword ptr [rcx + 0x18]
0000000180324EBC jb        0x180324ed7
0000000180324EBE mov       rax, qword ptr [r9 + 0x20]
0000000180324EC2 mov       r8, qword ptr [rax + 0xc0]
0000000180324EC9 mov       r8, qword ptr [r8 + 0x70]
0000000180324ECD mov       rcx, rdi
0000000180324ED0 call      0x180847ce0
0000000180324ED5 jmp       0x180324e70
0000000180324ED7 lea       eax, [r8 + 1]
0000000180324EDB mov       dword ptr [rdi + 0x18], eax
0000000180324EDE cmp       r8d, dword ptr [rcx + 0x18]
0000000180324EE2 jae       0x180324f53
0000000180324EE4 mov       qword ptr [rcx + r8*8 + 0x20], rdx
0000000180324EE9 lea       rcx, [rcx + r8*8]
0000000180324EED add       rcx, 0x20
0000000180324EF1 call      0x18024f360
0000000180324EF6 jmp       0x180324e70
0000000180324EFB mov       rdx, qword ptr [rip + 0x18a1afe] # 0x181bc6a00
0000000180324F02 mov       rcx, rbx
0000000180324F05 call      0x180302170
0000000180324F0A jmp       0x180324f27
0000000180324F0C mov       rdx, qword ptr [rip + 0x18a1aed] # 0x181bc6a00
0000000180324F13 mov       rcx, qword ptr [rsp + 0x38]
0000000180324F18 call      0x180302170
0000000180324F1D mov       rcx, qword ptr [rsp + 0x30]
0000000180324F22 test      rcx, rcx
0000000180324F25 jne       0x180324f63
0000000180324F27 lea       r11, [rsp + 0x80]
0000000180324F2F mov       rbx, qword ptr [r11 + 0x20]
0000000180324F33 mov       rsi, qword ptr [r11 + 0x28]
0000000180324F37 mov       rsp, r11
0000000180324F3A pop       rdi
0000000180324F3B ret       
0000000180324F3C call      0x180250140
0000000180324F41 nop       
0000000180324F42 call      0x180250150
0000000180324F47 call      0x180250150
0000000180324F4C nop       
0000000180324F4D call      0x180200e60
0000000180324F52 nop       
0000000180324F53 call      0x180250140
0000000180324F58 call      0x180250150
0000000180324F5D call      0x180250150
0000000180324F62 nop       
0000000180324F63 call      0x180200e60
0000000180324F68 int3      
0000000180324F69 call      0x180250150
0000000180324F6E int3      
0000000180324F6F int3      