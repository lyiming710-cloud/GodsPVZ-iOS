000000018037EBF0 mov       qword ptr [rsp + 8], rbx
000000018037EBF5 push      rdi
000000018037EBF6 sub       rsp, 0x60
000000018037EBFA cmp       byte ptr [rip + 0x19661d6], 0 # 0x181ce4dd7
000000018037EC01 mov       rbx, rcx
000000018037EC04 jne       0x18037ec19
000000018037EC06 lea       rcx, [rip + 0x18318f3] # 0x181bb0500
000000018037EC0D call      0x18024fef0
000000018037EC12 mov       byte ptr [rip + 0x19661be], 1 # 0x181ce4dd7
000000018037EC19 movaps    xmmword ptr [rsp + 0x50], xmm6
000000018037EC1E xor       ecx, ecx
000000018037EC20 movaps    xmmword ptr [rsp + 0x40], xmm7
000000018037EC25 call      0x1812e27c0
000000018037EC2A xor       edx, edx
000000018037EC2C lea       rcx, [rsp + 0x30]
000000018037EC31 mov       rdi, rax
000000018037EC34 call      0x181364ca0
000000018037EC39 test      rdi, rdi
000000018037EC3C je        0x18037ee83
000000018037EC42 mov       ecx, dword ptr [rax + 8]
000000018037EC45 lea       r8, [rsp + 0x20]
000000018037EC4A movsd     xmm0, qword ptr [rax]
000000018037EC4E xor       r9d, r9d
000000018037EC51 mov       dword ptr [rsp + 0x28], ecx
000000018037EC55 mov       rdx, rdi
000000018037EC58 lea       rcx, [rsp + 0x30]
000000018037EC5D movsd     qword ptr [rsp + 0x20], xmm0
000000018037EC63 call      0x1812e1d50
000000018037EC68 movsd     xmm1, qword ptr [rax]
000000018037EC6C mov       eax, dword ptr [rax + 8]
000000018037EC6F mov       dword ptr [rsp + 0x28], eax
000000018037EC73 mov       rdi, qword ptr [rbx + 0x20]
000000018037EC77 movaps    xmm0, xmm1
000000018037EC7A movss     dword ptr [rbx + 0x70], xmm1
000000018037EC7F shufps    xmm0, xmm0, 0x55
000000018037EC83 movss     dword ptr [rbx + 0x74], xmm0
000000018037EC88 mov       rcx, qword ptr [rip + 0x1831871] # 0x181bb0500
000000018037EC8F movsd     qword ptr [rsp + 0x20], xmm1
000000018037EC95 cmp       dword ptr [rcx + 0xe0], 0
000000018037EC9C jne       0x18037eca3
000000018037EC9E call      0x1802501e0
000000018037ECA3 xor       r8d, r8d
000000018037ECA6 xor       edx, edx
000000018037ECA8 mov       rcx, rdi
000000018037ECAB call      0x18131f760
000000018037ECB0 test      al, al
000000018037ECB2 jne       0x18037ed42
000000018037ECB8 mov       rax, qword ptr [rbx + 0x20]
000000018037ECBC test      rax, rax
000000018037ECBF je        0x18037ee83
000000018037ECC5 cmp       byte ptr [rax + 0x41], 0
000000018037ECC9 je        0x18037ed42
000000018037ECCB mov       ecx, dword ptr [rbx + 0x28]
000000018037ECCE test      ecx, ecx
000000018037ECD0 je        0x18037ed57
000000018037ECD6 sub       ecx, 1
000000018037ECD9 je        0x18037ed42
000000018037ECDB cmp       ecx, 1
000000018037ECDE jne       0x18037ed42
000000018037ECE0 movss     xmm6, dword ptr [rbx + 0x70]
000000018037ECE5 movss     xmm7, dword ptr [rbx + 0x74]
000000018037ECEA cmp       dword ptr [rbx + 0x2c], 5
000000018037ECEE jne       0x18037ed00
000000018037ECF0 addss     xmm6, dword ptr [rip + 0x1228e1c] # 0x1815a7b14
000000018037ECF8 addss     xmm7, dword ptr [rip + 0x1228e14] # 0x1815a7b14
000000018037ED00 mov       rcx, qword ptr [rbx + 0x90]
000000018037ED07 test      rcx, rcx
000000018037ED0A je        0x18037ee83
000000018037ED10 xor       edx, edx
000000018037ED12 call      0x18131c070
000000018037ED17 xorps     xmm1, xmm1
000000018037ED1A test      rax, rax
000000018037ED1D je        0x18037ee83
000000018037ED23 unpcklps  xmm6, xmm7
000000018037ED26 lea       rdx, [rsp + 0x20]
000000018037ED2B xor       r8d, r8d
000000018037ED2E movsd     qword ptr [rsp + 0x20], xmm6
000000018037ED34 mov       rcx, rax
000000018037ED37 movss     dword ptr [rsp + 0x28], xmm1
000000018037ED3D call      0x18132fbb0
000000018037ED42 movaps    xmm7, xmmword ptr [rsp + 0x40]
000000018037ED47 movaps    xmm6, xmmword ptr [rsp + 0x50]
000000018037ED4C mov       rbx, qword ptr [rsp + 0x70]
000000018037ED51 add       rsp, 0x60
000000018037ED55 pop       rdi
000000018037ED56 ret       
000000018037ED57 cmp       byte ptr [rip + 0x196607a], 0 # 0x181ce4dd8
000000018037ED5E jne       0x18037ed73
000000018037ED60 lea       rcx, [rip + 0x1823bd9] # 0x181ba2940
000000018037ED67 call      0x18024fef0
000000018037ED6C mov       byte ptr [rip + 0x1966065], 1 # 0x181ce4dd8
000000018037ED73 movss     xmm6, dword ptr [rbx + 0x3c]
000000018037ED78 xor       ecx, ecx
000000018037ED7A call      0x18132c2f0
000000018037ED7F mov       rcx, qword ptr [rbx + 0x50]
000000018037ED83 addss     xmm0, xmm6
000000018037ED87 movss     dword ptr [rbx + 0x3c], xmm0
000000018037ED8C test      rcx, rcx
000000018037ED8F je        0x18037ee89
000000018037ED95 movss     xmm1, dword ptr [rbx + 0x38]
000000018037ED9A xor       r8d, r8d
000000018037ED9D subss     xmm1, xmm0
000000018037EDA1 divss     xmm1, dword ptr [rbx + 0x38]
000000018037EDA6 call      0x1813d7750
000000018037EDAB mov       rax, qword ptr [rbx + 0x50]
000000018037EDAF test      rax, rax
000000018037EDB2 je        0x18037ee89
000000018037EDB8 mov       rcx, qword ptr [rbx + 0x58]
000000018037EDBC test      rcx, rcx
000000018037EDBF je        0x18037ee89
000000018037EDC5 movss     xmm1, dword ptr [rip + 0x1228c43] # 0x1815a7a10
000000018037EDCD xor       r8d, r8d
000000018037EDD0 subss     xmm1, dword ptr [rax + 0xfc]
000000018037EDD8 call      0x1813d7750
000000018037EDDD movss     xmm0, dword ptr [rbx + 0x38]
000000018037EDE2 lea       rcx, [rsp + 0x80]
000000018037EDEA subss     xmm0, dword ptr [rbx + 0x3c]
000000018037EDEF mov       rdx, qword ptr [rip + 0x1823b4a] # 0x181ba2940
000000018037EDF6 xor       r8d, r8d
000000018037EDF9 mov       rdi, qword ptr [rbx + 0x60]
000000018037EDFD movss     dword ptr [rsp + 0x80], xmm0
000000018037EE06 call      0x180cc5a50
000000018037EE0B test      rdi, rdi
000000018037EE0E je        0x18037ee89
000000018037EE10 mov       r9, qword ptr [rdi]
000000018037EE13 mov       rdx, rax
000000018037EE16 mov       rcx, rdi
000000018037EE19 mov       r8, qword ptr [r9 + 0x560]
000000018037EE20 call      qword ptr [r9 + 0x558]
000000018037EE27 movss     xmm0, dword ptr [rbx + 0x3c]
000000018037EE2C comiss    xmm0, dword ptr [rbx + 0x38]
000000018037EE30 jb        0x18037ed42
000000018037EE36 xor       edx, edx
000000018037EE38 mov       dword ptr [rbx + 0x28], 1
000000018037EE3F mov       rcx, rbx
000000018037EE42 call      0x18037e680
000000018037EE47 mov       rcx, qword ptr [rbx + 0x48]
000000018037EE4B test      rcx, rcx
000000018037EE4E je        0x18037ee89
000000018037EE50 xor       edx, edx
000000018037EE52 call      0x18131c070
000000018037EE57 test      rax, rax
000000018037EE5A je        0x18037ee89
000000018037EE5C xor       r8d, r8d
000000018037EE5F lea       rcx, [rsp + 0x30]
000000018037EE64 mov       rdx, rax
000000018037EE67 call      0x18132f070
000000018037EE6C movsd     xmm0, qword ptr [rax]
000000018037EE70 mov       ecx, dword ptr [rax + 8]
000000018037EE73 movsd     qword ptr [rbx + 0x78], xmm0
000000018037EE78 mov       dword ptr [rbx + 0x80], ecx
000000018037EE7E jmp       0x18037ed42
000000018037EE83 call      0x180250150
000000018037EE88 int3      
000000018037EE89 call      0x180250150
000000018037EE8E int3      