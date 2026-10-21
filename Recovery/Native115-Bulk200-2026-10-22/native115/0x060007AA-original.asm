00000001803BCA00 push      rbp
00000001803BCA02 push      rdi
00000001803BCA03 push      r15
00000001803BCA05 sub       rsp, 0xe0
00000001803BCA0C cmp       byte ptr [rip + 0x192856f], 0 # 0x181ce4f82
00000001803BCA13 mov       rdi, rcx
00000001803BCA16 jne       0x1803bca67
00000001803BCA18 lea       rcx, [rip + 0x18066b9] # 0x181bc30d8
00000001803BCA1F call      0x18024fef0
00000001803BCA24 lea       rcx, [rip + 0x17e79ad] # 0x181ba43d8
00000001803BCA2B call      0x18024fef0
00000001803BCA30 lea       rcx, [rip + 0x17e7941] # 0x181ba4378
00000001803BCA37 call      0x18024fef0
00000001803BCA3C lea       rcx, [rip + 0x17e78d5] # 0x181ba4318
00000001803BCA43 call      0x18024fef0
00000001803BCA48 lea       rcx, [rip + 0x17e5269] # 0x181ba1cb8
00000001803BCA4F call      0x18024fef0
00000001803BCA54 lea       rcx, [rip + 0x1817f75] # 0x181bd49d0
00000001803BCA5B call      0x18024fef0
00000001803BCA60 mov       byte ptr [rip + 0x192851b], 1 # 0x181ce4f82
00000001803BCA67 xor       r15d, r15d
00000001803BCA6A mov       ebp, r15d
00000001803BCA6D cmp       dword ptr [rdi + 0x24], r15d
00000001803BCA71 jle       0x1803bcea3
00000001803BCA77 movaps    xmmword ptr [rsp + 0xc0], xmm6
00000001803BCA7F movss     xmm6, dword ptr [rip + 0x11eb3a1] # 0x1815a7e28
00000001803BCA87 movaps    xmmword ptr [rsp + 0xb0], xmm7
00000001803BCA8F movss     xmm7, dword ptr [rip + 0x11eb5a1] # 0x1815a8038
00000001803BCA97 movaps    xmmword ptr [rsp + 0x90], xmm9
00000001803BCAA0 movss     xmm9, dword ptr [rip + 0x11eaf5f] # 0x1815a7a08
00000001803BCAA9 movaps    xmmword ptr [rsp + 0x80], xmm10
00000001803BCAB2 movss     xmm10, dword ptr [rip + 0x11eb4d1] # 0x1815a7f8c
00000001803BCABB movaps    xmmword ptr [rsp + 0x70], xmm11
00000001803BCAC1 movss     xmm11, dword ptr [rip + 0x11eb54e] # 0x1815a8018
00000001803BCACA mov       qword ptr [rsp + 0x108], rbx
00000001803BCAD2 mov       qword ptr [rsp + 0xd8], rsi
00000001803BCADA mov       qword ptr [rsp + 0xd0], r14
00000001803BCAE2 movaps    xmmword ptr [rsp + 0xa0], xmm8
00000001803BCAEB nop       dword ptr [rax + rax]
00000001803BCAF0 mov       rcx, qword ptr [rip + 0x17e51c1] # 0x181ba1cb8
00000001803BCAF7 cmp       dword ptr [rdi + 0x20], r15d
00000001803BCAFB je        0x1803bccd4
00000001803BCB01 call      0x180250100
00000001803BCB06 xor       edx, edx
00000001803BCB08 mov       rcx, rax
00000001803BCB0B mov       rbx, rax
00000001803BCB0E call      0x18131be60
00000001803BCB13 test      rbx, rbx
00000001803BCB16 je        0x1803bceaf
00000001803BCB1C xor       edx, edx
00000001803BCB1E mov       rcx, rbx
00000001803BCB21 call      0x18131c070
00000001803BCB26 xor       r8d, r8d
00000001803BCB29 movaps    xmm1, xmm6
00000001803BCB2C movaps    xmm0, xmm7
00000001803BCB2F mov       rsi, rax
00000001803BCB32 call      0x18130d350
00000001803BCB37 movaps    xmm8, xmm0
00000001803BCB3B xor       r8d, r8d
00000001803BCB3E movaps    xmm0, xmm7
00000001803BCB41 movaps    xmm1, xmm6
00000001803BCB44 call      0x18130d350
00000001803BCB49 test      rsi, rsi
00000001803BCB4C je        0x1803bceaf
00000001803BCB52 movaps    xmm1, xmm8
00000001803BCB56 movss     dword ptr [rsp + 0x28], xmm0
00000001803BCB5C unpcklps  xmm1, xmm9
00000001803BCB60 lea       rdx, [rsp + 0x20]
00000001803BCB65 xor       r8d, r8d
00000001803BCB68 movsd     qword ptr [rsp + 0x20], xmm1
00000001803BCB6E mov       rcx, rsi
00000001803BCB71 call      0x18132fbb0
00000001803BCB76 mov       rdx, qword ptr [rip + 0x17e779b] # 0x181ba4318
00000001803BCB7D mov       rcx, rbx
00000001803BCB80 call      0x18046d280
00000001803BCB85 mov       rsi, rax
00000001803BCB88 test      rax, rax
00000001803BCB8B je        0x1803bceaf
00000001803BCB91 mov       rdx, qword ptr [rip + 0x1806540] # 0x181bc30d8
00000001803BCB98 mov       rcx, rax
00000001803BCB9B call      0x18042e9f0
00000001803BCBA0 mov       rcx, qword ptr [rdi + 0x28]
00000001803BCBA4 mov       r14, rax
00000001803BCBA7 test      rcx, rcx
00000001803BCBAA je        0x1803bceaf
00000001803BCBB0 xor       edx, edx
00000001803BCBB2 call      0x1813b9880
00000001803BCBB7 test      r14, r14
00000001803BCBBA je        0x1803bceaf
00000001803BCBC0 xor       r8d, r8d
00000001803BCBC3 mov       rdx, rax
00000001803BCBC6 mov       rcx, r14
00000001803BCBC9 call      0x1812fb060
00000001803BCBCE mov       rdx, qword ptr [rdi + 0x28]
00000001803BCBD2 xor       r8d, r8d
00000001803BCBD5 mov       rcx, rsi
00000001803BCBD8 call      0x1813bb9e0
00000001803BCBDD xor       r8d, r8d
00000001803BCBE0 mov       edx, 7
00000001803BCBE5 mov       rcx, rsi
00000001803BCBE8 call      0x1813bb8c0
00000001803BCBED xor       r8d, r8d
00000001803BCBF0 mov       edx, 0x60
00000001803BCBF5 mov       rcx, rsi
00000001803BCBF8 call      0x1813bb9a0
00000001803BCBFD mov       dword ptr [rsp + 0x100], 0xff00ffff
00000001803BCC08 mov       eax, dword ptr [rsp + 0x100]
00000001803BCC0F mov       ecx, eax
00000001803BCC11 shr       ecx, 8
00000001803BCC14 movzx     ecx, cl
00000001803BCC17 movd      xmm3, ecx
00000001803BCC1B mov       ecx, eax
00000001803BCC1D shr       ecx, 0x10
00000001803BCC20 movzx     ecx, cl
00000001803BCC23 shr       eax, 0x18
00000001803BCC26 cvtdq2ps  xmm3, xmm3
00000001803BCC29 movd      xmm1, ecx
00000001803BCC2D movd      xmm2, eax
00000001803BCC31 cvtdq2ps  xmm1, xmm1
00000001803BCC34 cvtdq2ps  xmm2, xmm2
00000001803BCC37 divss     xmm1, xmm10
00000001803BCC3C divss     xmm2, xmm10
00000001803BCC41 divss     xmm3, xmm10
00000001803BCC46 mov       dword ptr [rsp + 0x40], 0x3f800000
00000001803BCC4E lea       rdx, [rsp + 0x60]
00000001803BCC53 movups    xmm0, xmmword ptr [rsp + 0x40]
00000001803BCC58 xor       r8d, r8d
00000001803BCC5B mov       rcx, rsi
00000001803BCC5E shufps    xmm0, xmm0, 0xe1
00000001803BCC62 movss     xmm0, xmm3
00000001803BCC66 shufps    xmm0, xmm0, 0xc6
00000001803BCC6A movss     xmm0, xmm1
00000001803BCC6E shufps    xmm0, xmm0, 0x27
00000001803BCC72 movss     xmm0, xmm2
00000001803BCC76 shufps    xmm0, xmm0, 0x39
00000001803BCC7A movups    xmmword ptr [rsp + 0x40], xmm0
00000001803BCC7F movdqa    xmmword ptr [rsp + 0x60], xmm0
00000001803BCC85 call      0x1813bb950
00000001803BCC8A mov       rdx, qword ptr [rip + 0x1817d3f] # 0x181bd49d0
00000001803BCC91 xor       r8d, r8d
00000001803BCC94 mov       rcx, rsi
00000001803BCC97 call      0x1813bba30
00000001803BCC9C mov       rdx, qword ptr [rip + 0x17e7735] # 0x181ba43d8
00000001803BCCA3 mov       rcx, rbx
00000001803BCCA6 call      0x18046d280
00000001803BCCAB lea       rcx, [rdi + 0x30]
00000001803BCCAF mov       qword ptr [rdi + 0x30], rax
00000001803BCCB3 mov       rdx, rax
00000001803BCCB6 call      0x18024f360
00000001803BCCBB mov       rax, qword ptr [rdi + 0x30]
00000001803BCCBF test      rax, rax
00000001803BCCC2 je        0x1803bceaf
00000001803BCCC8 mov       dword ptr [rax + 0x74], 1
00000001803BCCCF jmp       0x1803bce4f
00000001803BCCD4 call      0x180250100
00000001803BCCD9 xor       edx, edx
00000001803BCCDB mov       rcx, rax
00000001803BCCDE mov       rbx, rax
00000001803BCCE1 call      0x18131be60
00000001803BCCE6 test      rbx, rbx
00000001803BCCE9 je        0x1803bceaf
00000001803BCCEF xor       edx, edx
00000001803BCCF1 mov       rcx, rbx
00000001803BCCF4 call      0x18131c070
00000001803BCCF9 xor       r8d, r8d
00000001803BCCFC movaps    xmm1, xmm6
00000001803BCCFF movaps    xmm0, xmm7
00000001803BCD02 mov       rsi, rax
00000001803BCD05 call      0x18130d350
00000001803BCD0A movaps    xmm8, xmm0
00000001803BCD0E xor       r8d, r8d
00000001803BCD11 movaps    xmm0, xmm7
00000001803BCD14 movaps    xmm1, xmm6
00000001803BCD17 call      0x18130d350
00000001803BCD1C test      rsi, rsi
00000001803BCD1F je        0x1803bceaf
00000001803BCD25 movaps    xmm1, xmm8
00000001803BCD29 movss     dword ptr [rsp + 0x38], xmm0
00000001803BCD2F unpcklps  xmm1, xmm9
00000001803BCD33 lea       rdx, [rsp + 0x30]
00000001803BCD38 xor       r8d, r8d
00000001803BCD3B movsd     qword ptr [rsp + 0x30], xmm1
00000001803BCD41 mov       rcx, rsi
00000001803BCD44 call      0x18132fbb0
00000001803BCD49 mov       rdx, qword ptr [rip + 0x17e7628] # 0x181ba4378
00000001803BCD50 mov       rcx, rbx
00000001803BCD53 call      0x18046d280
00000001803BCD58 mov       rsi, rax
00000001803BCD5B test      rax, rax
00000001803BCD5E je        0x1803bceaf
00000001803BCD64 xor       r8d, r8d
00000001803BCD67 movaps    xmm1, xmm11
00000001803BCD6B mov       rcx, rax
00000001803BCD6E call      0x1811b9410
00000001803BCD73 mov       r9, qword ptr [rsi]
00000001803BCD76 mov       rcx, rsi
00000001803BCD79 mov       rdx, qword ptr [rip + 0x1817c50] # 0x181bd49d0
00000001803BCD80 mov       r8, qword ptr [r9 + 0x560]
00000001803BCD87 call      qword ptr [r9 + 0x558]
00000001803BCD8E mov       dword ptr [rsp + 0x110], 0xff00ffff
00000001803BCD99 mov       eax, dword ptr [rsp + 0x110]
00000001803BCDA0 mov       ecx, eax
00000001803BCDA2 shr       ecx, 8
00000001803BCDA5 movzx     ecx, cl
00000001803BCDA8 movd      xmm3, ecx
00000001803BCDAC mov       ecx, eax
00000001803BCDAE shr       ecx, 0x10
00000001803BCDB1 movzx     ecx, cl
00000001803BCDB4 shr       eax, 0x18
00000001803BCDB7 cvtdq2ps  xmm3, xmm3
00000001803BCDBA movd      xmm1, ecx
00000001803BCDBE movd      xmm2, eax
00000001803BCDC2 cvtdq2ps  xmm1, xmm1
00000001803BCDC5 cvtdq2ps  xmm2, xmm2
00000001803BCDC8 divss     xmm1, xmm10
00000001803BCDCD divss     xmm2, xmm10
00000001803BCDD2 divss     xmm3, xmm10
00000001803BCDD7 mov       rax, qword ptr [rsi]
00000001803BCDDA lea       rdx, [rsp + 0x60]
00000001803BCDDF mov       dword ptr [rsp + 0x50], 0x3f800000
00000001803BCDE7 mov       rcx, rsi
00000001803BCDEA movaps    xmm0, xmmword ptr [rsp + 0x50]
00000001803BCDEF shufps    xmm0, xmm0, 0xe1
00000001803BCDF3 mov       r8, qword ptr [rax + 0x2b0]
00000001803BCDFA movss     xmm0, xmm3
00000001803BCDFE shufps    xmm0, xmm0, 0xc6
00000001803BCE02 movss     xmm0, xmm1
00000001803BCE06 shufps    xmm0, xmm0, 0x27
00000001803BCE0A movss     xmm0, xmm2
00000001803BCE0E shufps    xmm0, xmm0, 0x39
00000001803BCE12 movaps    xmmword ptr [rsp + 0x50], xmm0
00000001803BCE17 movdqa    xmmword ptr [rsp + 0x60], xmm0
00000001803BCE1D call      qword ptr [rax + 0x2a8]
00000001803BCE23 mov       rdx, qword ptr [rip + 0x17e75ae] # 0x181ba43d8
00000001803BCE2A mov       rcx, rbx
00000001803BCE2D call      0x18046d280
00000001803BCE32 lea       rcx, [rdi + 0x30]
00000001803BCE36 mov       qword ptr [rdi + 0x30], rax
00000001803BCE3A mov       rdx, rax
00000001803BCE3D call      0x18024f360
00000001803BCE42 mov       rax, qword ptr [rdi + 0x30]
00000001803BCE46 test      rax, rax
00000001803BCE49 je        0x1803bceaf
00000001803BCE4B mov       dword ptr [rax + 0x74], r15d
00000001803BCE4F inc       ebp
00000001803BCE51 cmp       ebp, dword ptr [rdi + 0x24]
00000001803BCE54 jl        0x1803bcaf0
00000001803BCE5A mov       rsi, qword ptr [rsp + 0xd8]
00000001803BCE62 mov       r14, qword ptr [rsp + 0xd0]
00000001803BCE6A movaps    xmm8, xmmword ptr [rsp + 0xa0]
00000001803BCE73 mov       rbx, qword ptr [rsp + 0x108]
00000001803BCE7B movaps    xmm6, xmmword ptr [rsp + 0xc0]
00000001803BCE83 movaps    xmm7, xmmword ptr [rsp + 0xb0]
00000001803BCE8B movaps    xmm9, xmmword ptr [rsp + 0x90]
00000001803BCE94 movaps    xmm10, xmmword ptr [rsp + 0x80]
00000001803BCE9D movaps    xmm11, xmmword ptr [rsp + 0x70]
00000001803BCEA3 add       rsp, 0xe0
00000001803BCEAA pop       r15
00000001803BCEAC pop       rdi
00000001803BCEAD pop       rbp
00000001803BCEAE ret       
00000001803BCEAF call      0x180250150
00000001803BCEB4 int3      
00000001803BCEB5 int3      
00000001803BCEB6 int3      
00000001803BCEB7 int3      
00000001803BCEB8 int3      
00000001803BCEB9 int3      
00000001803BCEBA int3      
00000001803BCEBB int3      
00000001803BCEBC int3      
00000001803BCEBD int3      
00000001803BCEBE int3      
00000001803BCEBF int3      