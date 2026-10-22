00000001803C09C0 mov       qword ptr [rsp + 0x20], rsi
00000001803C09C5 push      rbp
00000001803C09C6 push      rdi
00000001803C09C7 push      r14
00000001803C09C9 mov       rbp, rsp
00000001803C09CC sub       rsp, 0x80
00000001803C09D3 cmp       byte ptr [rip + 0x19245a7], 0 # 0x181ce4f81
00000001803C09DA mov       rdi, rcx
00000001803C09DD movaps    xmmword ptr [rsp + 0x40], xmm9
00000001803C09E3 jne       0x1803c09f8
00000001803C09E5 lea       rcx, [rip + 0x17fd58c] # 0x181bbdf78
00000001803C09EC call      0x18024fef0
00000001803C09F1 mov       byte ptr [rip + 0x1924589], 1 # 0x181ce4f81
00000001803C09F8 mov       ecx, dword ptr [rdi + 0x10]
00000001803C09FB xor       r14d, r14d
00000001803C09FE mov       rsi, qword ptr [rdi + 0x20]
00000001803C0A02 movss     xmm9, dword ptr [rip + 0x11e7019] # 0x1815a7a24
00000001803C0A0B mov       qword ptr [rsp + 0xa8], rbx
00000001803C0A13 movaps    xmmword ptr [rsp + 0x70], xmm6
00000001803C0A18 movaps    xmmword ptr [rsp + 0x60], xmm7
00000001803C0A1D movaps    xmmword ptr [rsp + 0x50], xmm8
00000001803C0A23 mov       dword ptr [rbp + 0x30], r14d
00000001803C0A27 test      ecx, ecx
00000001803C0A29 je        0x1803c0a99
00000001803C0A2B sub       ecx, 1
00000001803C0A2E je        0x1803c0a8d
00000001803C0A30 cmp       ecx, 1
00000001803C0A33 jne       0x1803c0a86
00000001803C0A35 mov       dword ptr [rdi + 0x10], 0xffffffff
00000001803C0A3C test      rsi, rsi
00000001803C0A3F je        0x1803c0f4b
00000001803C0A45 mov       rcx, qword ptr [rsi + 0x48]
00000001803C0A49 test      rcx, rcx
00000001803C0A4C je        0x1803c0f4b
00000001803C0A52 movsd     xmm0, qword ptr [rdi + 0x34]
00000001803C0A57 lea       rdx, [rbp - 0x60]
00000001803C0A5B mov       eax, dword ptr [rdi + 0x3c]
00000001803C0A5E xor       r8d, r8d
00000001803C0A61 movsd     qword ptr [rbp - 0x60], xmm0
00000001803C0A66 mov       dword ptr [rbp - 0x58], eax
00000001803C0A69 call      0x18132fbb0
00000001803C0A6E xor       edx, edx
00000001803C0A70 mov       rcx, rsi
00000001803C0A73 call      0x1803bbd60
00000001803C0A78 xor       r8d, r8d
00000001803C0A7B mov       rdx, rax
00000001803C0A7E mov       rcx, rsi
00000001803C0A81 call      0x18131dbe0
00000001803C0A86 xor       al, al
00000001803C0A88 jmp       0x1803c0eb6
00000001803C0A8D mov       dword ptr [rdi + 0x10], 0xffffffff
00000001803C0A94 jmp       0x1803c0b4d
00000001803C0A99 movss     xmm1, dword ptr [rip + 0x11e7067] # 0x1815a7b08
00000001803C0AA1 xor       r8d, r8d
00000001803C0AA4 movss     xmm0, dword ptr [rip + 0x11e7134] # 0x1815a7be0
00000001803C0AAC mov       dword ptr [rdi + 0x10], 0xffffffff
00000001803C0AB3 mov       dword ptr [rdi + 0x28], 0x40000000
00000001803C0ABA call      0x18130d350
00000001803C0ABF movss     dword ptr [rdi + 0x2c], xmm0
00000001803C0AC4 movss     dword ptr [rdi + 0x30], xmm0
00000001803C0AC9 test      rsi, rsi
00000001803C0ACC je        0x1803c0f4b
00000001803C0AD2 mov       rdx, qword ptr [rsi + 0x48]
00000001803C0AD6 test      rdx, rdx
00000001803C0AD9 je        0x1803c0f4b
00000001803C0ADF xor       r8d, r8d
00000001803C0AE2 lea       rcx, [rbp - 0x50]
00000001803C0AE6 call      0x18132f3a0
00000001803C0AEB movsd     xmm0, qword ptr [rax]
00000001803C0AEF mov       ecx, dword ptr [rax + 8]
00000001803C0AF2 movsd     qword ptr [rdi + 0x34], xmm0
00000001803C0AF7 mov       dword ptr [rdi + 0x3c], ecx
00000001803C0AFA mov       rdx, qword ptr [rsi + 0x30]
00000001803C0AFE test      rdx, rdx
00000001803C0B01 je        0x1803c0f4b
00000001803C0B07 mov       r8, qword ptr [rdx]
00000001803C0B0A lea       rcx, [rbp - 0x50]
00000001803C0B0E mov       rax, qword ptr [r8 + 0x298]
00000001803C0B15 mov       r8, qword ptr [r8 + 0x2a0]
00000001803C0B1C call      rax
00000001803C0B1E xor       edx, edx
00000001803C0B20 lea       rcx, [rbp - 0x50]
00000001803C0B24 movups    xmm0, xmmword ptr [rax]
00000001803C0B27 movaps    xmmword ptr [rbp - 0x50], xmm0
00000001803C0B2B call      0x18039bf00
00000001803C0B30 movaps    xmm0, xmm9
00000001803C0B34 mov       dword ptr [rdi + 0x40], eax
00000001803C0B37 divss     xmm0, dword ptr [rdi + 0x2c]
00000001803C0B3C mov       dword ptr [rdi + 0x44], 0x437f0000
00000001803C0B43 mulss     xmm0, dword ptr [rdi + 0x28]
00000001803C0B48 movss     dword ptr [rdi + 0x48], xmm0
00000001803C0B4D movss     xmm0, dword ptr [rdi + 0x30]
00000001803C0B52 xorps     xmm8, xmm8
00000001803C0B56 comiss    xmm0, xmm8
00000001803C0B5A jbe       0x1803c0ee8
00000001803C0B60 xor       ecx, ecx
00000001803C0B62 movaps    xmm6, xmm0
00000001803C0B65 call      0x18132c2f0
00000001803C0B6A divss     xmm0, dword ptr [rdi + 0x28]
00000001803C0B6F movss     xmm7, dword ptr [rip + 0x11e7415] # 0x1815a7f8c
00000001803C0B77 mulss     xmm0, dword ptr [rdi + 0x2c]
00000001803C0B7C subss     xmm6, xmm0
00000001803C0B80 comiss    xmm9, xmm6
00000001803C0B84 movss     dword ptr [rdi + 0x30], xmm6
00000001803C0B89 jb        0x1803c0bbc
00000001803C0B8B movss     xmm6, dword ptr [rdi + 0x44]
00000001803C0B90 xor       ecx, ecx
00000001803C0B92 call      0x18132c2f0
00000001803C0B97 divss     xmm0, dword ptr [rdi + 0x48]
00000001803C0B9C mulss     xmm0, xmm7
00000001803C0BA0 subss     xmm6, xmm0
00000001803C0BA4 comiss    xmm8, xmm6
00000001803C0BA8 ja        0x1803c0bb4
00000001803C0BAA comiss    xmm6, xmm7
00000001803C0BAD jbe       0x1803c0bb7
00000001803C0BAF movaps    xmm6, xmm7
00000001803C0BB2 jmp       0x1803c0bb7
00000001803C0BB4 xorps     xmm6, xmm6
00000001803C0BB7 movss     dword ptr [rdi + 0x44], xmm6
00000001803C0BBC cvttss2si eax, dword ptr [rdi + 0x30]
00000001803C0BC1 mov       dword ptr [rbp + 0x30], eax
00000001803C0BC4 test      rsi, rsi
00000001803C0BC7 je        0x1803c0f4b
00000001803C0BCD mov       rbx, qword ptr [rsi + 0x30]
00000001803C0BD1 lea       rcx, [rbp + 0x30]
00000001803C0BD5 xor       edx, edx
00000001803C0BD7 call      0x180ca3820
00000001803C0BDC test      rbx, rbx
00000001803C0BDF je        0x1803c0f4b
00000001803C0BE5 mov       r9, qword ptr [rbx]
00000001803C0BE8 mov       rdx, rax
00000001803C0BEB mov       rcx, rbx
00000001803C0BEE mov       r8, qword ptr [r9 + 0x560]
00000001803C0BF5 call      qword ptr [r9 + 0x558]
00000001803C0BFC movzx     ecx, byte ptr [rdi + 0x40]
00000001803C0C00 movss     xmm0, dword ptr [rdi + 0x44]
00000001803C0C05 movzx     edx, byte ptr [rdi + 0x41]
00000001803C0C09 movzx     r8d, byte ptr [rdi + 0x42]
00000001803C0C0E mov       r9, qword ptr [rsi + 0x30]
00000001803C0C12 cvttss2si eax, xmm0
00000001803C0C16 mov       dword ptr [rbp + 0x20], r14d
00000001803C0C1A mov       byte ptr [rbp + 0x20], cl
00000001803C0C1D mov       byte ptr [rbp + 0x21], dl
00000001803C0C20 mov       byte ptr [rbp + 0x22], r8b
00000001803C0C24 movd      xmm0, ecx
00000001803C0C28 mov       byte ptr [rbp + 0x23], al
00000001803C0C2B mov       eax, dword ptr [rbp + 0x20]
00000001803C0C2E mov       ecx, eax
00000001803C0C30 shr       ecx, 8
00000001803C0C33 movzx     ecx, cl
00000001803C0C36 cvtdq2ps  xmm0, xmm0
00000001803C0C39 movd      xmm1, ecx
00000001803C0C3D mov       ecx, eax
00000001803C0C3F shr       ecx, 0x10
00000001803C0C42 movzx     ecx, cl
00000001803C0C45 shr       eax, 0x18
00000001803C0C48 cvtdq2ps  xmm1, xmm1
00000001803C0C4B movd      xmm2, ecx
00000001803C0C4F movd      xmm3, eax
00000001803C0C53 cvtdq2ps  xmm2, xmm2
00000001803C0C56 cvtdq2ps  xmm3, xmm3
00000001803C0C59 divss     xmm2, xmm7
00000001803C0C5D divss     xmm3, xmm7
00000001803C0C61 divss     xmm0, xmm7
00000001803C0C65 divss     xmm1, xmm7
00000001803C0C69 test      r9, r9
00000001803C0C6C je        0x1803c0f4b
00000001803C0C72 mov       rax, qword ptr [r9]
00000001803C0C75 lea       rdx, [rbp - 0x50]
00000001803C0C79 shufps    xmm0, xmm0, 0xe1
00000001803C0C7D mov       rcx, r9
00000001803C0C80 movss     xmm0, xmm1
00000001803C0C84 shufps    xmm0, xmm0, 0xc6
00000001803C0C88 mov       r8, qword ptr [rax + 0x2b0]
00000001803C0C8F movss     xmm0, xmm2
00000001803C0C93 shufps    xmm0, xmm0, 0x27
00000001803C0C97 movss     xmm0, xmm3
00000001803C0C9B shufps    xmm0, xmm0, 0x39
00000001803C0C9F movaps    xmmword ptr [rbp - 0x50], xmm0
00000001803C0CA3 movdqa    xmmword ptr [rbp - 0x50], xmm0
00000001803C0CA8 call      qword ptr [rax + 0x2a8]
00000001803C0CAE mov       r14, qword ptr [rsi + 0x48]
00000001803C0CB2 test      r14, r14
00000001803C0CB5 je        0x1803c0f4b
00000001803C0CBB xor       r8d, r8d
00000001803C0CBE lea       rcx, [rbp - 0x50]
00000001803C0CC2 mov       rdx, r14
00000001803C0CC5 call      0x18132f3a0
00000001803C0CCA movss     xmm6, dword ptr [rdi + 0x2c]
00000001803C0CCF xor       ecx, ecx
00000001803C0CD1 movsd     xmm7, qword ptr [rax]
00000001803C0CD5 mov       ebx, dword ptr [rax + 8]
00000001803C0CD8 call      0x18132c2f0
00000001803C0CDD movaps    xmm4, xmm0
00000001803C0CE0 movd      xmm2, ebx
00000001803C0CE4 mulss     xmm4, xmm6
00000001803C0CE8 addss     xmm2, xmm8
00000001803C0CED movaps    xmm1, xmm7
00000001803C0CF0 movsd     qword ptr [rbp - 0x50], xmm7
00000001803C0CF5 movaps    xmm3, xmm7
00000001803C0CF8 shufps    xmm1, xmm1, 0x55
00000001803C0CFC addss     xmm4, xmm1
00000001803C0D00 addss     xmm3, xmm8
00000001803C0D05 unpcklps  xmm3, xmm4
00000001803C0D08 lea       rdx, [rbp - 0x60]
00000001803C0D0C xor       r8d, r8d
00000001803C0D0F movsd     qword ptr [rbp - 0x60], xmm3
00000001803C0D14 mov       rcx, r14
00000001803C0D17 movss     dword ptr [rbp - 0x58], xmm2
00000001803C0D1C call      0x18132fbb0
00000001803C0D21 mov       rdx, qword ptr [rsi + 0x50]
00000001803C0D25 movsd     xmm6, qword ptr [rsi + 0x58]
00000001803C0D2A mov       ebx, dword ptr [rsi + 0x60]
00000001803C0D2D test      rdx, rdx
00000001803C0D30 je        0x1803c0f4b
00000001803C0D36 xor       r8d, r8d
00000001803C0D39 lea       rcx, [rbp - 0x50]
00000001803C0D3D call      0x18132f3a0
00000001803C0D42 xor       r9d, r9d
00000001803C0D45 lea       rdx, [rbp - 0x60]
00000001803C0D49 mov       r8d, 0x3e8
00000001803C0D4F lea       rcx, [rbp - 0x50]
00000001803C0D53 movsd     xmm0, qword ptr [rax]
00000001803C0D57 mov       eax, dword ptr [rax + 8]
00000001803C0D5A movsd     qword ptr [rbp - 0x60], xmm0
00000001803C0D5F mov       dword ptr [rbp - 0x58], eax
00000001803C0D62 movsd     qword ptr [rbp - 0x50], xmm6
00000001803C0D67 mov       dword ptr [rbp - 0x48], ebx
00000001803C0D6A call      0x1811657a0
00000001803C0D6F test      al, al
00000001803C0D71 je        0x1803c0dba
00000001803C0D73 mov       rdx, qword ptr [rsi + 0x50]
00000001803C0D77 movups    xmm6, xmmword ptr [rsi + 0x64]
00000001803C0D7B test      rdx, rdx
00000001803C0D7E je        0x1803c0f4b
00000001803C0D84 xor       r8d, r8d
00000001803C0D87 lea       rcx, [rbp - 0x50]
00000001803C0D8B call      0x18132f510
00000001803C0D90 xor       r9d, r9d
00000001803C0D93 movdqa    xmmword ptr [rbp - 0x60], xmm6
00000001803C0D98 mov       r8d, 0x3e8
00000001803C0D9E lea       rdx, [rbp - 0x50]
00000001803C0DA2 lea       rcx, [rbp - 0x60]
00000001803C0DA6 movups    xmm0, xmmword ptr [rax]
00000001803C0DA9 movaps    xmmword ptr [rbp - 0x50], xmm0
00000001803C0DAD call      0x1811656d0
00000001803C0DB2 test      al, al
00000001803C0DB4 jne       0x1803c0e77
00000001803C0DBA mov       rdx, qword ptr [rsi + 0x50]
00000001803C0DBE test      rdx, rdx
00000001803C0DC1 je        0x1803c0f4b
00000001803C0DC7 xor       r8d, r8d
00000001803C0DCA lea       rcx, [rbp - 0x50]
00000001803C0DCE call      0x18132f3a0
00000001803C0DD3 mov       rdx, qword ptr [rsi + 0x50]
00000001803C0DD7 movsd     xmm0, qword ptr [rax]
00000001803C0DDB mov       ecx, dword ptr [rax + 8]
00000001803C0DDE movsd     qword ptr [rsi + 0x58], xmm0
00000001803C0DE3 mov       dword ptr [rsi + 0x60], ecx
00000001803C0DE6 test      rdx, rdx
00000001803C0DE9 je        0x1803c0f4b
00000001803C0DEF xor       r8d, r8d
00000001803C0DF2 lea       rcx, [rbp - 0x50]
00000001803C0DF6 call      0x18132f510
00000001803C0DFB mov       rcx, qword ptr [rsi + 0x48]
00000001803C0DFF movups    xmm0, xmmword ptr [rax]
00000001803C0E02 movups    xmmword ptr [rsi + 0x64], xmm0
00000001803C0E06 test      rcx, rcx
00000001803C0E09 je        0x1803c0f4b
00000001803C0E0F xor       r8d, r8d
00000001803C0E12 movdqa    xmmword ptr [rbp - 0x50], xmm0
00000001803C0E17 lea       rdx, [rbp - 0x50]
00000001803C0E1B call      0x18132fc50
00000001803C0E20 mov       rdx, qword ptr [rsi + 0x40]
00000001803C0E24 test      rdx, rdx
00000001803C0E27 je        0x1803c0f4b
00000001803C0E2D xor       r8d, r8d
00000001803C0E30 lea       rcx, [rbp - 0x50]
00000001803C0E34 call      0x18132f3a0
00000001803C0E39 movsd     xmm0, qword ptr [rsi + 0x58]
00000001803C0E3E movsd     xmm1, qword ptr [rax]
00000001803C0E42 movss     xmm2, dword ptr [rax + 8]
00000001803C0E47 subss     xmm1, xmm0
00000001803C0E4B subss     xmm2, dword ptr [rsi + 0x60]
00000001803C0E50 mov       rcx, qword ptr [rsi + 0x40]
00000001803C0E54 test      rcx, rcx
00000001803C0E57 je        0x1803c0f4b
00000001803C0E5D unpcklps  xmm1, xmm8
00000001803C0E61 lea       rdx, [rbp - 0x50]
00000001803C0E65 xor       r8d, r8d
00000001803C0E68 movsd     qword ptr [rbp - 0x50], xmm1
00000001803C0E6D movss     dword ptr [rbp - 0x48], xmm2
00000001803C0E72 call      0x18132f770
00000001803C0E77 mov       rax, qword ptr [rip + 0x17fd0fa] # 0x181bbdf78
00000001803C0E7E cmp       dword ptr [rax + 0xe0], 0
00000001803C0E85 jne       0x1803c0e96
00000001803C0E87 mov       rcx, rax
00000001803C0E8A call      0x1802501e0
00000001803C0E8F mov       rax, qword ptr [rip + 0x17fd0e2] # 0x181bbdf78
00000001803C0E96 mov       rax, qword ptr [rax + 0xb8]
00000001803C0E9D lea       rcx, [rdi + 0x18]
00000001803C0EA1 mov       rdx, qword ptr [rax]
00000001803C0EA4 mov       qword ptr [rdi + 0x18], rdx
00000001803C0EA8 call      0x18024f360
00000001803C0EAD mov       dword ptr [rdi + 0x10], 1
00000001803C0EB4 mov       al, 1
00000001803C0EB6 movaps    xmm8, xmmword ptr [rsp + 0x50]
00000001803C0EBC movaps    xmm7, xmmword ptr [rsp + 0x60]
00000001803C0EC1 movaps    xmm6, xmmword ptr [rsp + 0x70]
00000001803C0EC6 mov       rbx, qword ptr [rsp + 0xa8]
00000001803C0ECE mov       rsi, qword ptr [rsp + 0xb8]
00000001803C0ED6 movaps    xmm9, xmmword ptr [rsp + 0x40]
00000001803C0EDC add       rsp, 0x80
00000001803C0EE3 pop       r14
00000001803C0EE5 pop       rdi
00000001803C0EE6 pop       rbp
00000001803C0EE7 ret       
00000001803C0EE8 mov       rax, qword ptr [rip + 0x17fd089] # 0x181bbdf78
00000001803C0EEF cmp       dword ptr [rax + 0xe0], r14d
00000001803C0EF6 jne       0x1803c0f07
00000001803C0EF8 mov       rcx, rax
00000001803C0EFB call      0x1802501e0
00000001803C0F00 mov       rax, qword ptr [rip + 0x17fd071] # 0x181bbdf78
00000001803C0F07 mov       rax, qword ptr [rax + 0xb8]
00000001803C0F0E xor       r8d, r8d
00000001803C0F11 mov       edx, 0x13
00000001803C0F16 xor       ecx, ecx
00000001803C0F18 mov       rbx, qword ptr [rax + 8]
00000001803C0F1C call      0x18130d310
00000001803C0F21 test      rbx, rbx
00000001803C0F24 je        0x1803c0f4b
00000001803C0F26 cmp       eax, dword ptr [rbx + 0x18]
00000001803C0F29 jae       0x1803c0f51
00000001803C0F2B cdqe      
00000001803C0F2D lea       rcx, [rdi + 0x18]
00000001803C0F31 mov       rdx, qword ptr [rbx + rax*8 + 0x20]
00000001803C0F36 mov       qword ptr [rdi + 0x18], rdx
00000001803C0F3A call      0x18024f360
00000001803C0F3F mov       dword ptr [rdi + 0x10], 2
00000001803C0F46 jmp       0x1803c0eb4
00000001803C0F4B call      0x180250150
00000001803C0F50 int3      
00000001803C0F51 call      0x180250140
00000001803C0F56 int3      
00000001803C0F57 int3      
00000001803C0F58 int3      
00000001803C0F59 int3      
00000001803C0F5A int3      
00000001803C0F5B int3      
00000001803C0F5C int3      
00000001803C0F5D int3      
00000001803C0F5E int3      
00000001803C0F5F int3      