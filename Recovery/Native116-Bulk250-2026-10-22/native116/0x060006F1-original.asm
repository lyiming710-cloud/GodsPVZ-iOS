00000001803AE670 mov       rax, rsp
00000001803AE673 mov       qword ptr [rax + 0x10], rbx
00000001803AE677 mov       qword ptr [rax + 8], rcx
00000001803AE67B push      rdi
00000001803AE67C sub       rsp, 0xa0
00000001803AE683 movaps    xmmword ptr [rax - 0x18], xmm6
00000001803AE687 movaps    xmmword ptr [rax - 0x28], xmm7
00000001803AE68B mov       rdi, rcx
00000001803AE68E cmp       byte ptr [rip + 0x1936895], 0 # 0x181ce4f2a
00000001803AE695 jne       0x1803ae6fe
00000001803AE697 lea       rcx, [rip + 0x1816682] # 0x181bc4d20
00000001803AE69E call      0x18024fef0
00000001803AE6A3 lea       rcx, [rip + 0x18166ce] # 0x181bc4d78
00000001803AE6AA call      0x18024fef0
00000001803AE6AF lea       rcx, [rip + 0x181671a] # 0x181bc4dd0
00000001803AE6B6 call      0x18024fef0
00000001803AE6BB lea       rcx, [rip + 0x1800d4e] # 0x181baf410
00000001803AE6C2 call      0x18024fef0
00000001803AE6C7 lea       rcx, [rip + 0x1800e4a] # 0x181baf518
00000001803AE6CE call      0x18024fef0
00000001803AE6D3 lea       rcx, [rip + 0x1800e96] # 0x181baf570
00000001803AE6DA call      0x18024fef0
00000001803AE6DF lea       rcx, [rip + 0x1807d92] # 0x181bb6478
00000001803AE6E6 call      0x18024fef0
00000001803AE6EB lea       rcx, [rip + 0x180c606] # 0x181bbacf8
00000001803AE6F2 call      0x18024fef0
00000001803AE6F7 mov       byte ptr [rip + 0x193682c], 1 # 0x181ce4f2a
00000001803AE6FE mov       rcx, qword ptr [rdi + 0x30]
00000001803AE702 cmp       qword ptr [rdi + 0x28], 0
00000001803AE707 je        0x1803ae8ef
00000001803AE70D mov       rdx, qword ptr [rdi + 0x28]
00000001803AE711 test      rcx, rcx
00000001803AE714 je        0x1803aea7e
00000001803AE71A xor       r9d, r9d
00000001803AE71D mov       r8b, 1
00000001803AE720 mov       rdx, qword ptr [rdx + 0x10]
00000001803AE724 call      0x1811af070
00000001803AE729 mov       rcx, qword ptr [rdi + 0x38]
00000001803AE72D mov       rdx, qword ptr [rdi + 0x28]
00000001803AE731 test      rdx, rdx
00000001803AE734 je        0x1803aea7e
00000001803AE73A test      rcx, rcx
00000001803AE73D je        0x1803aea7e
00000001803AE743 xor       r9d, r9d
00000001803AE746 mov       r8b, 1
00000001803AE749 mov       rdx, qword ptr [rdx + 0x30]
00000001803AE74D call      0x1811af070
00000001803AE752 mov       rcx, qword ptr [rdi + 0x40]
00000001803AE756 mov       rdx, qword ptr [rdi + 0x28]
00000001803AE75A test      rdx, rdx
00000001803AE75D je        0x1803aea7e
00000001803AE763 test      rcx, rcx
00000001803AE766 je        0x1803aea7e
00000001803AE76C xor       r9d, r9d
00000001803AE76F mov       r8b, 1
00000001803AE772 mov       rdx, qword ptr [rdx + 0x28]
00000001803AE776 call      0x1811af070
00000001803AE77B mov       rcx, qword ptr [rdi + 0x48]
00000001803AE77F mov       rdx, qword ptr [rdi + 0x28]
00000001803AE783 test      rdx, rdx
00000001803AE786 je        0x1803aea7e
00000001803AE78C test      rcx, rcx
00000001803AE78F je        0x1803aea7e
00000001803AE795 xor       r9d, r9d
00000001803AE798 mov       r8b, 1
00000001803AE79B mov       rdx, qword ptr [rdx + 0x38]
00000001803AE79F call      0x1811af070
00000001803AE7A4 xor       ebx, ebx
00000001803AE7A6 mov       ecx, ebx
00000001803AE7A8 mov       rax, qword ptr [rdi + 0x50]
00000001803AE7AC test      rax, rax
00000001803AE7AF je        0x1803aea7e
00000001803AE7B5 movdqa    xmm6, xmmword ptr [rip + 0x11f9713] # 0x1815a7ed0
00000001803AE7BD movdqa    xmm7, xmmword ptr [rip + 0x11f93ab] # 0x1815a7b70
00000001803AE7C5 cmp       ecx, dword ptr [rax + 0x18]
00000001803AE7C8 jge       0x1803ae861
00000001803AE7CE mov       rax, qword ptr [rdi + 0x28]
00000001803AE7D2 test      rax, rax
00000001803AE7D5 je        0x1803aea7e
00000001803AE7DB mov       rcx, qword ptr [rdi + 0x50]
00000001803AE7DF cmp       ebx, dword ptr [rax + 0x1c]
00000001803AE7E2 jl        0x1803ae80c
00000001803AE7E4 test      rcx, rcx
00000001803AE7E7 je        0x1803aea7e
00000001803AE7ED mov       r8, qword ptr [rip + 0x1800d7c] # 0x181baf570
00000001803AE7F4 mov       edx, ebx
00000001803AE7F6 call      0x180825710
00000001803AE7FB test      rax, rax
00000001803AE7FE je        0x1803aea7e
00000001803AE804 movdqa    xmmword ptr [rsp + 0x30], xmm6
00000001803AE80A jmp       0x1803ae832
00000001803AE80C test      rcx, rcx
00000001803AE80F je        0x1803aea7e
00000001803AE815 mov       r8, qword ptr [rip + 0x1800d54] # 0x181baf570
00000001803AE81C mov       edx, ebx
00000001803AE81E call      0x180825710
00000001803AE823 test      rax, rax
00000001803AE826 je        0x1803aea7e
00000001803AE82C movdqa    xmmword ptr [rsp + 0x30], xmm7
00000001803AE832 mov       r9, qword ptr [rax]
00000001803AE835 mov       r8, qword ptr [r9 + 0x2b0]
00000001803AE83C lea       rdx, [rsp + 0x30]
00000001803AE841 mov       rcx, rax
00000001803AE844 call      qword ptr [r9 + 0x2a8]
00000001803AE84B inc       ebx
00000001803AE84D mov       ecx, ebx
00000001803AE84F mov       rax, qword ptr [rdi + 0x50]
00000001803AE853 test      rax, rax
00000001803AE856 je        0x1803aea7e
00000001803AE85C jmp       0x1803ae7c5
00000001803AE861 mov       rcx, qword ptr [rdi + 0x58]
00000001803AE865 test      rcx, rcx
00000001803AE868 je        0x1803aea7e
00000001803AE86E xor       edx, edx
00000001803AE870 call      0x1813044d0
00000001803AE875 test      rax, rax
00000001803AE878 je        0x1803aea7e
00000001803AE87E xor       r8d, r8d
00000001803AE881 mov       dl, 1
00000001803AE883 mov       rcx, rax
00000001803AE886 call      0x18131bad0
00000001803AE88B mov       rbx, qword ptr [rdi + 0x58]
00000001803AE88F mov       rax, qword ptr [rdi + 0x28]
00000001803AE893 test      rax, rax
00000001803AE896 je        0x1803aea7e
00000001803AE89C mov       edi, dword ptr [rax + 0x18]
00000001803AE89F mov       rcx, qword ptr [rip + 0x1807bd2] # 0x181bb6478
00000001803AE8A6 cmp       dword ptr [rcx + 0xe0], 0
00000001803AE8AD jne       0x1803ae8b4
00000001803AE8AF call      0x1802501e0
00000001803AE8B4 xor       edx, edx
00000001803AE8B6 mov       ecx, edi
00000001803AE8B8 call      0x18033a9e0
00000001803AE8BD test      rbx, rbx
00000001803AE8C0 je        0x1803aea7e
00000001803AE8C6 xor       r8d, r8d
00000001803AE8C9 mov       rdx, rax
00000001803AE8CC mov       rcx, rbx
00000001803AE8CF call      0x1813d7ab0
00000001803AE8D4 lea       r11, [rsp + 0xa0]
00000001803AE8DC mov       rbx, qword ptr [r11 + 0x18]
00000001803AE8E0 movaps    xmm6, xmmword ptr [r11 - 0x10]
00000001803AE8E5 movaps    xmm7, xmmword ptr [r11 - 0x20]
00000001803AE8EA mov       rsp, r11
00000001803AE8ED pop       rdi
00000001803AE8EE ret       
00000001803AE8EF mov       rax, qword ptr [rip + 0x180c402] # 0x181bbacf8
00000001803AE8F6 mov       rdx, qword ptr [rax + 0xb8]
00000001803AE8FD test      rcx, rcx
00000001803AE900 je        0x1803aea7e
00000001803AE906 xor       r9d, r9d
00000001803AE909 mov       r8b, 1
00000001803AE90C mov       rdx, qword ptr [rdx]
00000001803AE90F call      0x1811af070
00000001803AE914 mov       rcx, qword ptr [rdi + 0x38]
00000001803AE918 mov       rax, qword ptr [rip + 0x180c3d9] # 0x181bbacf8
00000001803AE91F mov       rdx, qword ptr [rax + 0xb8]
00000001803AE926 test      rcx, rcx
00000001803AE929 je        0x1803aea7e
00000001803AE92F xor       r9d, r9d
00000001803AE932 mov       r8b, 1
00000001803AE935 mov       rdx, qword ptr [rdx]
00000001803AE938 call      0x1811af070
00000001803AE93D mov       rcx, qword ptr [rdi + 0x40]
00000001803AE941 mov       rax, qword ptr [rip + 0x180c3b0] # 0x181bbacf8
00000001803AE948 mov       rdx, qword ptr [rax + 0xb8]
00000001803AE94F test      rcx, rcx
00000001803AE952 je        0x1803aea7e
00000001803AE958 xor       r9d, r9d
00000001803AE95B mov       r8b, 1
00000001803AE95E mov       rdx, qword ptr [rdx]
00000001803AE961 call      0x1811af070
00000001803AE966 mov       rcx, qword ptr [rdi + 0x48]
00000001803AE96A mov       rax, qword ptr [rip + 0x180c387] # 0x181bbacf8
00000001803AE971 mov       rdx, qword ptr [rax + 0xb8]
00000001803AE978 test      rcx, rcx
00000001803AE97B je        0x1803aea7e
00000001803AE981 xor       r9d, r9d
00000001803AE984 mov       r8b, 1
00000001803AE987 mov       rdx, qword ptr [rdx]
00000001803AE98A call      0x1811af070
00000001803AE98F mov       rdx, qword ptr [rdi + 0x50]
00000001803AE993 test      rdx, rdx
00000001803AE996 je        0x1803aea7e
00000001803AE99C mov       r8, qword ptr [rip + 0x1800a6d] # 0x181baf410
00000001803AE9A3 lea       rcx, [rsp + 0x40]
00000001803AE9A8 call      0x1808197f0
00000001803AE9AD movups    xmm0, xmmword ptr [rsp + 0x40]
00000001803AE9B2 movups    xmmword ptr [rsp + 0x60], xmm0
00000001803AE9B7 movsd     xmm1, qword ptr [rsp + 0x50]
00000001803AE9BD movsd     qword ptr [rsp + 0x70], xmm1
00000001803AE9C3 xor       ebx, ebx
00000001803AE9C5 mov       qword ptr [rsp + 0x30], rbx
00000001803AE9CA lea       rbx, [rsp + 0x60]
00000001803AE9CF mov       qword ptr [rsp + 0x38], rbx
00000001803AE9D4 movdqa    xmm6, xmmword ptr [rip + 0x11f94f4] # 0x1815a7ed0
00000001803AE9DC nop       dword ptr [rax]
00000001803AE9E0 mov       rdx, qword ptr [rip + 0x1816391] # 0x181bc4d78
00000001803AE9E7 lea       rcx, [rsp + 0x60]
00000001803AE9EC call      0x180669280
00000001803AE9F1 test      al, al
00000001803AE9F3 je        0x1803aea23
00000001803AE9F5 mov       rcx, qword ptr [rsp + 0x70]
00000001803AE9FA test      rcx, rcx
00000001803AE9FD je        0x1803aea84
00000001803AEA03 mov       r8, qword ptr [rcx]
00000001803AEA06 mov       rax, qword ptr [r8 + 0x2a8]
00000001803AEA0D movdqa    xmmword ptr [rsp + 0x40], xmm6
00000001803AEA13 mov       r8, qword ptr [r8 + 0x2b0]
00000001803AEA1A lea       rdx, [rsp + 0x40]
00000001803AEA1F call      rax
00000001803AEA21 jmp       0x1803ae9e0
00000001803AEA23 mov       rdx, qword ptr [rip + 0x18162f6] # 0x181bc4d20
00000001803AEA2A mov       rcx, rbx
00000001803AEA2D call      0x180302170
00000001803AEA32 jmp       0x1803aea57
00000001803AEA34 mov       rdx, qword ptr [rip + 0x18162e5] # 0x181bc4d20
00000001803AEA3B mov       rcx, qword ptr [rsp + 0x38]
00000001803AEA40 call      0x180302170
00000001803AEA45 mov       rcx, qword ptr [rsp + 0x30]
00000001803AEA4A test      rcx, rcx
00000001803AEA4D jne       0x1803aea8a
00000001803AEA4F mov       rdi, qword ptr [rsp + 0xb0]
00000001803AEA57 mov       rcx, qword ptr [rdi + 0x58]
00000001803AEA5B test      rcx, rcx
00000001803AEA5E je        0x1803aea7e
00000001803AEA60 xor       edx, edx
00000001803AEA62 call      0x1813044d0
00000001803AEA67 test      rax, rax
00000001803AEA6A je        0x1803aea7e
00000001803AEA6C xor       r8d, r8d
00000001803AEA6F xor       edx, edx
00000001803AEA71 mov       rcx, rax
00000001803AEA74 call      0x18131bad0
00000001803AEA79 jmp       0x1803ae8d4
00000001803AEA7E call      0x180250150
00000001803AEA83 nop       
00000001803AEA84 call      0x180250150
00000001803AEA89 nop       
00000001803AEA8A call      0x180200e60
00000001803AEA8F int3      