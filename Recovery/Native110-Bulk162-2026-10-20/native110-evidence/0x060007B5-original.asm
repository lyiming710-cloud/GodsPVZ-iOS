00000001803B5DD0 mov       qword ptr [rsp + 8], rbx
00000001803B5DD5 mov       qword ptr [rsp + 0x10], rdi
00000001803B5DDA push      rbp
00000001803B5DDB mov       rbp, rsp
00000001803B5DDE sub       rsp, 0x60
00000001803B5DE2 mov       rbx, rcx
00000001803B5DE5 movaps    xmmword ptr [rsp + 0x50], xmm6
00000001803B5DEA mov       rcx, qword ptr [rcx + 0x38]
00000001803B5DEE mov       edi, edx
00000001803B5DF0 movaps    xmmword ptr [rsp + 0x40], xmm7
00000001803B5DF5 test      rcx, rcx
00000001803B5DF8 je        0x1803b602e
00000001803B5DFE mov       rax, qword ptr [rcx]
00000001803B5E01 lea       rdx, [rbp - 0x30]
00000001803B5E05 movdqa    xmm0, xmmword ptr [rip + 0x11f1d63] # 0x1815a7b70
00000001803B5E0D movdqa    xmmword ptr [rbp - 0x30], xmm0
00000001803B5E12 mov       r8, qword ptr [rax + 0x5e0]
00000001803B5E19 call      qword ptr [rax + 0x5d8]
00000001803B5E1F test      edi, edi
00000001803B5E21 je        0x1803b5f67
00000001803B5E27 sub       edi, 1
00000001803B5E2A je        0x1803b5f34
00000001803B5E30 sub       edi, 1
00000001803B5E33 je        0x1803b5ebe
00000001803B5E39 cmp       edi, 1
00000001803B5E3C jne       0x1803b6014
00000001803B5E42 mov       rcx, qword ptr [rbx + 0x38]
00000001803B5E46 test      rcx, rcx
00000001803B5E49 je        0x1803b602e
00000001803B5E4F xor       r8d, r8d
00000001803B5E52 mov       edx, 0x404
00000001803B5E57 call      0x1811b8c90
00000001803B5E5C mov       rcx, qword ptr [rbx + 0x38]
00000001803B5E60 test      rcx, rcx
00000001803B5E63 je        0x1803b602e
00000001803B5E69 xor       edx, edx
00000001803B5E6B call      0x1811b84f0
00000001803B5E70 movss     xmm6, dword ptr [rip + 0x11f1b98] # 0x1815a7a10
00000001803B5E78 xorps     xmm7, xmm7
00000001803B5E7B test      rax, rax
00000001803B5E7E je        0x1803b602e
00000001803B5E84 movaps    xmm0, xmm6
00000001803B5E87 xor       r8d, r8d
00000001803B5E8A unpcklps  xmm0, xmm7
00000001803B5E8D mov       rcx, rax
00000001803B5E90 movq      rdx, xmm0
00000001803B5E95 call      0x1813252d0
00000001803B5E9A mov       rdx, qword ptr [rbx + 0x48]
00000001803B5E9E mov       rdi, qword ptr [rbx + 0x40]
00000001803B5EA2 movss     xmm1, dword ptr [rip + 0x11f1dc6] # 0x1815a7c70
00000001803B5EAA test      rdx, rdx
00000001803B5EAD je        0x1803b602e
00000001803B5EB3 movaps    xmm0, xmm6
00000001803B5EB6 unpcklps  xmm0, xmm7
00000001803B5EB9 jmp       0x1803b5fd7
00000001803B5EBE mov       rcx, qword ptr [rbx + 0x38]
00000001803B5EC2 test      rcx, rcx
00000001803B5EC5 je        0x1803b602e
00000001803B5ECB xor       r8d, r8d
00000001803B5ECE mov       edx, 0x104
00000001803B5ED3 call      0x1811b8c90
00000001803B5ED8 mov       rcx, qword ptr [rbx + 0x38]
00000001803B5EDC test      rcx, rcx
00000001803B5EDF je        0x1803b602e
00000001803B5EE5 xor       edx, edx
00000001803B5EE7 call      0x1811b84f0
00000001803B5EEC movss     xmm6, dword ptr [rip + 0x11f1b1c] # 0x1815a7a10
00000001803B5EF4 test      rax, rax
00000001803B5EF7 je        0x1803b602e
00000001803B5EFD movaps    xmm0, xmm6
00000001803B5F00 xor       r8d, r8d
00000001803B5F03 unpcklps  xmm0, xmm6
00000001803B5F06 mov       rcx, rax
00000001803B5F09 movq      rdx, xmm0
00000001803B5F0E call      0x1813252d0
00000001803B5F13 mov       rdx, qword ptr [rbx + 0x48]
00000001803B5F17 mov       rdi, qword ptr [rbx + 0x40]
00000001803B5F1B movss     xmm1, dword ptr [rip + 0x11f1d4d] # 0x1815a7c70
00000001803B5F23 test      rdx, rdx
00000001803B5F26 je        0x1803b602e
00000001803B5F2C movaps    xmm0, xmm6
00000001803B5F2F jmp       0x1803b5fd4
00000001803B5F34 mov       rcx, qword ptr [rbx + 0x38]
00000001803B5F38 test      rcx, rcx
00000001803B5F3B je        0x1803b602e
00000001803B5F41 xor       r8d, r8d
00000001803B5F44 mov       edx, 0x401
00000001803B5F49 call      0x1811b8c90
00000001803B5F4E mov       rcx, qword ptr [rbx + 0x38]
00000001803B5F52 test      rcx, rcx
00000001803B5F55 je        0x1803b602e
00000001803B5F5B xor       edx, edx
00000001803B5F5D call      0x1811b84f0
00000001803B5F62 xorps     xmm6, xmm6
00000001803B5F65 jmp       0x1803b5f9d
00000001803B5F67 mov       rcx, qword ptr [rbx + 0x38]
00000001803B5F6B test      rcx, rcx
00000001803B5F6E je        0x1803b602e
00000001803B5F74 xor       r8d, r8d
00000001803B5F77 mov       edx, 0x101
00000001803B5F7C call      0x1811b8c90
00000001803B5F81 mov       rcx, qword ptr [rbx + 0x38]
00000001803B5F85 test      rcx, rcx
00000001803B5F88 je        0x1803b602e
00000001803B5F8E xor       edx, edx
00000001803B5F90 call      0x1811b84f0
00000001803B5F95 movss     xmm6, dword ptr [rip + 0x11f1a73] # 0x1815a7a10
00000001803B5F9D test      rax, rax
00000001803B5FA0 je        0x1803b602e
00000001803B5FA6 xorps     xmm0, xmm0
00000001803B5FA9 xor       r8d, r8d
00000001803B5FAC unpcklps  xmm0, xmm6
00000001803B5FAF mov       rcx, rax
00000001803B5FB2 movq      rdx, xmm0
00000001803B5FB7 call      0x1813252d0
00000001803B5FBC mov       rdx, qword ptr [rbx + 0x48]
00000001803B5FC0 mov       rdi, qword ptr [rbx + 0x40]
00000001803B5FC4 movss     xmm1, dword ptr [rip + 0x11f1ca4] # 0x1815a7c70
00000001803B5FCC test      rdx, rdx
00000001803B5FCF je        0x1803b602e
00000001803B5FD1 xorps     xmm0, xmm0
00000001803B5FD4 unpcklps  xmm0, xmm6
00000001803B5FD7 xor       r9d, r9d
00000001803B5FDA movss     dword ptr [rbp - 0x38], xmm1
00000001803B5FDF lea       r8, [rbp - 0x40]
00000001803B5FE3 movsd     qword ptr [rbp - 0x40], xmm0
00000001803B5FE8 lea       rcx, [rbp - 0x30]
00000001803B5FEC call      0x1812e2030
00000001803B5FF1 test      rdi, rdi
00000001803B5FF4 je        0x1803b602e
00000001803B5FF6 movsd     xmm0, qword ptr [rax]
00000001803B5FFA lea       rdx, [rbp - 0x40]
00000001803B5FFE mov       eax, dword ptr [rax + 8]
00000001803B6001 xor       r8d, r8d
00000001803B6004 mov       rcx, rdi
00000001803B6007 movsd     qword ptr [rbp - 0x40], xmm0
00000001803B600C mov       dword ptr [rbp - 0x38], eax
00000001803B600F call      0x18132fbb0
00000001803B6014 mov       rbx, qword ptr [rsp + 0x70]
00000001803B6019 mov       rdi, qword ptr [rsp + 0x78]
00000001803B601E movaps    xmm6, xmmword ptr [rsp + 0x50]
00000001803B6023 movaps    xmm7, xmmword ptr [rsp + 0x40]
00000001803B6028 add       rsp, 0x60
00000001803B602C pop       rbp
00000001803B602D ret       
00000001803B602E call      0x180250150
00000001803B6033 int3      