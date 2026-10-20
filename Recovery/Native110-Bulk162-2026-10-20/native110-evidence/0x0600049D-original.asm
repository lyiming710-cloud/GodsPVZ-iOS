0000000180370D80 push      rbx
0000000180370D82 sub       rsp, 0x60
0000000180370D86 cmp       byte ptr [rip + 0x1974014], 0 # 0x181ce4da1
0000000180370D8D mov       rbx, rcx
0000000180370D90 jne       0x180370dc9
0000000180370D92 lea       rcx, [rip + 0x183127f] # 0x181ba2018
0000000180370D99 call      0x18024fef0
0000000180370D9E lea       rcx, [rip + 0x18334d3] # 0x181ba4278
0000000180370DA5 call      0x18024fef0
0000000180370DAA lea       rcx, [rip + 0x183f74f] # 0x181bb0500
0000000180370DB1 call      0x18024fef0
0000000180370DB6 lea       rcx, [rip + 0x18456bb] # 0x181bb6478
0000000180370DBD call      0x18024fef0
0000000180370DC2 mov       byte ptr [rip + 0x1973fd8], 1 # 0x181ce4da1
0000000180370DC9 xor       r9d, r9d
0000000180370DCC mov       qword ptr [rsp + 0x78], rdi
0000000180370DD1 xor       r8d, r8d
0000000180370DD4 mov       qword ptr [rsp + 0x20], 0
0000000180370DDD mov       edx, 0xffffffff
0000000180370DE2 mov       rcx, rbx
0000000180370DE5 call      0x180360d00
0000000180370DEA test      rax, rax
0000000180370DED je        0x18037104c
0000000180370DF3 xor       edx, edx
0000000180370DF5 mov       rcx, rax
0000000180370DF8 call      0x180311890
0000000180370DFD xor       edx, edx
0000000180370DFF mov       rcx, rbx
0000000180370E02 call      0x18035e470
0000000180370E07 mov       rax, qword ptr [rip + 0x184566a] # 0x181bb6478
0000000180370E0E cmp       dword ptr [rax + 0xe0], 0
0000000180370E15 jne       0x180370e26
0000000180370E17 mov       rcx, rax
0000000180370E1A call      0x1802501e0
0000000180370E1F mov       rax, qword ptr [rip + 0x1845652] # 0x181bb6478
0000000180370E26 mov       rax, qword ptr [rax + 0xb8]
0000000180370E2D mov       rcx, qword ptr [rax + 0x130]
0000000180370E34 test      rcx, rcx
0000000180370E37 je        0x18037104c
0000000180370E3D mov       r8, qword ptr [rip + 0x1833434] # 0x181ba4278
0000000180370E44 mov       edx, 1
0000000180370E49 call      0x180825710
0000000180370E4E mov       rcx, qword ptr [rip + 0x183f6ab] # 0x181bb0500
0000000180370E55 mov       rdi, rax
0000000180370E58 cmp       dword ptr [rcx + 0xe0], 0
0000000180370E5F jne       0x180370e66
0000000180370E61 call      0x1802501e0
0000000180370E66 xor       edx, edx
0000000180370E68 mov       rcx, rdi
0000000180370E6B call      0x18131f870
0000000180370E70 test      al, al
0000000180370E72 je        0x180370f2d
0000000180370E78 xor       ecx, ecx
0000000180370E7A call      0x1812e27c0
0000000180370E7F test      rax, rax
0000000180370E82 je        0x18037104c
0000000180370E88 xor       edx, edx
0000000180370E8A mov       rcx, rax
0000000180370E8D call      0x181304510
0000000180370E92 test      rax, rax
0000000180370E95 je        0x18037104c
0000000180370E9B mov       qword ptr [rsp + 0x70], rsi
0000000180370EA0 lea       rcx, [rsp + 0x30]
0000000180370EA5 movaps    xmmword ptr [rsp + 0x50], xmm6
0000000180370EAA xor       r8d, r8d
0000000180370EAD mov       rdx, rax
0000000180370EB0 movaps    xmmword ptr [rsp + 0x40], xmm7
0000000180370EB5 call      0x18132f3a0
0000000180370EBA mov       rcx, qword ptr [rip + 0x1831157] # 0x181ba2018
0000000180370EC1 movsd     xmm7, qword ptr [rax]
0000000180370EC5 cmp       dword ptr [rcx + 0xe0], 0
0000000180370ECC mov       esi, dword ptr [rax + 8]
0000000180370ECF jne       0x180370ed6
0000000180370ED1 call      0x1802501e0
0000000180370ED6 xor       ecx, ecx
0000000180370ED8 call      0x18031aea0
0000000180370EDD movss     xmm1, dword ptr [rip + 0x1236b2f] # 0x1815a7a14
0000000180370EE5 movaps    xmm6, xmm0
0000000180370EE8 movss     xmm0, dword ptr [rip + 0x1236bd4] # 0x1815a7ac4
0000000180370EF0 xor       r8d, r8d
0000000180370EF3 call      0x18130d350
0000000180370EF8 movaps    xmm3, xmm0
0000000180370EFB movsd     qword ptr [rsp + 0x30], xmm7
0000000180370F01 movaps    xmm2, xmm6
0000000180370F04 mov       dword ptr [rsp + 0x38], esi
0000000180370F08 lea       rdx, [rsp + 0x30]
0000000180370F0D mov       qword ptr [rsp + 0x20], 0
0000000180370F16 mov       rcx, rdi
0000000180370F19 call      0x18031b350
0000000180370F1E movaps    xmm7, xmmword ptr [rsp + 0x40]
0000000180370F23 movaps    xmm6, xmmword ptr [rsp + 0x50]
0000000180370F28 mov       rsi, qword ptr [rsp + 0x70]
0000000180370F2D cmp       byte ptr [rip + 0x1973c00], 0 # 0x181ce4b34
0000000180370F34 jne       0x180370f49
0000000180370F36 lea       rcx, [rip + 0x1840953] # 0x181bb1890
0000000180370F3D call      0x18024fef0
0000000180370F42 mov       byte ptr [rip + 0x1973beb], 1 # 0x181ce4b34
0000000180370F49 mov       rax, qword ptr [rip + 0x1840940] # 0x181bb1890
0000000180370F50 mov       rcx, qword ptr [rax + 0xb8]
0000000180370F57 mov       rcx, qword ptr [rcx]
0000000180370F5A test      rcx, rcx
0000000180370F5D je        0x18037104c
0000000180370F63 xor       r8d, r8d
0000000180370F66 mov       edx, 2
0000000180370F6B call      0x180321f30
0000000180370F70 mov       rcx, qword ptr [rip + 0x183f589] # 0x181bb0500
0000000180370F77 mov       rdi, rax
0000000180370F7A cmp       dword ptr [rcx + 0xe0], 0
0000000180370F81 jne       0x180370f88
0000000180370F83 call      0x1802501e0
0000000180370F88 xor       edx, edx
0000000180370F8A mov       rcx, rdi
0000000180370F8D call      0x18131f870
0000000180370F92 test      al, al
0000000180370F94 je        0x180371041
0000000180370F9A test      rdi, rdi
0000000180370F9D je        0x18037104c
0000000180370FA3 xor       edx, edx
0000000180370FA5 mov       rcx, rdi
0000000180370FA8 call      0x18131c070
0000000180370FAD movss     xmm1, dword ptr [rbx + 0x38]
0000000180370FB2 xorps     xmm2, xmm2
0000000180370FB5 addss     xmm1, dword ptr [rbx + 0x34]
0000000180370FBA test      rax, rax
0000000180370FBD je        0x18037104c
0000000180370FC3 movss     xmm0, dword ptr [rbx + 0x30]
0000000180370FC8 lea       rdx, [rsp + 0x30]
0000000180370FCD unpcklps  xmm0, xmm1
0000000180370FD0 xor       r8d, r8d
0000000180370FD3 mov       rcx, rax
0000000180370FD6 movsd     qword ptr [rsp + 0x30], xmm0
0000000180370FDC movss     dword ptr [rsp + 0x38], xmm2
0000000180370FE2 call      0x18132fbb0
0000000180370FE7 xor       edx, edx
0000000180370FE9 mov       rcx, rdi
0000000180370FEC call      0x18131c070
0000000180370FF1 movss     xmm1, dword ptr [rip + 0x1236acf] # 0x1815a7ac8
0000000180370FF9 test      rax, rax
0000000180370FFC je        0x18037104c
0000000180370FFE movaps    xmm0, xmm1
0000000180371001 movss     dword ptr [rsp + 0x38], xmm1
0000000180371007 unpcklps  xmm0, xmm1
000000018037100A lea       rdx, [rsp + 0x30]
000000018037100F xor       r8d, r8d
0000000180371012 movsd     qword ptr [rsp + 0x30], xmm0
0000000180371018 mov       rcx, rax
000000018037101B call      0x18132fa50
0000000180371020 mov       rcx, qword ptr [rbx + 0x20]
0000000180371024 test      rcx, rcx
0000000180371027 je        0x18037104c
0000000180371029 movss     xmm2, dword ptr [rip + 0x1236ad3] # 0x1815a7b04
0000000180371031 xor       r9d, r9d
0000000180371034 movss     xmm1, dword ptr [rip + 0x1236bd0] # 0x1815a7c0c
000000018037103C call      0x1803276e0
0000000180371041 mov       rdi, qword ptr [rsp + 0x78]
0000000180371046 add       rsp, 0x60
000000018037104A pop       rbx
000000018037104B ret       
000000018037104C call      0x180250150
0000000180371051 int3      