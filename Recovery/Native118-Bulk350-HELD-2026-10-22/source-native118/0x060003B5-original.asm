000000018035AAB0 mov       rax, rsp
000000018035AAB3 mov       qword ptr [rax + 0x20], rbx
000000018035AAB7 mov       dword ptr [rax + 0x18], r8d
000000018035AABB mov       dword ptr [rax + 0x10], edx
000000018035AABE mov       qword ptr [rax + 8], rcx
000000018035AAC2 push      rsi
000000018035AAC3 push      rdi
000000018035AAC4 push      r13
000000018035AAC6 push      r14
000000018035AAC8 push      r15
000000018035AACA sub       rsp, 0x100
000000018035AAD1 movaps    xmmword ptr [rax - 0x38], xmm6
000000018035AAD5 movaps    xmmword ptr [rax - 0x48], xmm7
000000018035AAD9 movaps    xmmword ptr [rax - 0x58], xmm8
000000018035AADE mov       r14d, r8d
000000018035AAE1 mov       r15d, edx
000000018035AAE4 mov       rsi, rcx
000000018035AAE7 cmp       byte ptr [rip + 0x198a24a], 0 # 0x181ce4d38
000000018035AAEE jne       0x18035ab33
000000018035AAF0 lea       rcx, [rip + 0x1868969] # 0x181bc3460
000000018035AAF7 call      0x18024fef0
000000018035AAFC lea       rcx, [rip + 0x18689b5] # 0x181bc34b8
000000018035AB03 call      0x18024fef0
000000018035AB08 lea       rcx, [rip + 0x1868a01] # 0x181bc3510
000000018035AB0F call      0x18024fef0
000000018035AB14 lea       rcx, [rip + 0x185135d] # 0x181babe78
000000018035AB1B call      0x18024fef0
000000018035AB20 lea       rcx, [rip + 0x18559d9] # 0x181bb0500
000000018035AB27 call      0x18024fef0
000000018035AB2C mov       byte ptr [rip + 0x198a205], 1 # 0x181ce4d38
000000018035AB33 xor       edx, edx
000000018035AB35 mov       rcx, rsi
000000018035AB38 call      0x181304510
000000018035AB3D test      rax, rax
000000018035AB40 je        0x18035b123
000000018035AB46 xor       r8d, r8d
000000018035AB49 mov       rdx, rax
000000018035AB4C lea       rcx, [rsp + 0x80]
000000018035AB54 call      0x18132f3a0
000000018035AB59 movsd     xmm0, qword ptr [rax]
000000018035AB5D movsd     qword ptr [rsp + 0x30], xmm0
000000018035AB63 mov       ebx, dword ptr [rax + 8]
000000018035AB66 mov       rax, qword ptr [rsi + 0x1e8]
000000018035AB6D test      rax, rax
000000018035AB70 je        0x18035b123
000000018035AB76 mov       rdx, qword ptr [rax + 0x28]
000000018035AB7A test      rdx, rdx
000000018035AB7D je        0x18035b123
000000018035AB83 mov       qword ptr [rsp + 0x20], 0
000000018035AB8C mov       r9d, r14d
000000018035AB8F mov       r8d, r15d
000000018035AB92 lea       rcx, [rsp + 0x40]
000000018035AB97 call      0x18030eeb0
000000018035AB9C movsd     xmm7, qword ptr [rax]
000000018035ABA0 mov       r13d, dword ptr [rax + 8]
000000018035ABA4 mov       dword ptr [rsp + 0x98], r13d
000000018035ABAC movsd     qword ptr [rsp + 0x80], xmm7
000000018035ABB5 mov       dword ptr [rsp + 0x88], r13d
000000018035ABBD xor       r8d, r8d
000000018035ABC0 lea       rdx, [rsp + 0x80]
000000018035ABC8 mov       rcx, rsi
000000018035ABCB call      0x1803592c0
000000018035ABD0 movd      xmm8, r13d
000000018035ABD5 movd      xmm0, ebx
000000018035ABD9 subss     xmm8, xmm0
000000018035ABDE movaps    xmm6, xmm7
000000018035ABE1 shufps    xmm6, xmm6, 0x55
000000018035ABE5 subss     xmm6, dword ptr [rsp + 0x34]
000000018035ABEB movsd     qword ptr [rsp + 0x90], xmm7
000000018035ABF4 subss     xmm7, dword ptr [rsp + 0x30]
000000018035ABFA movss     dword ptr [rsp + 0x68], xmm7
000000018035AC00 movss     dword ptr [rsp + 0x6c], xmm6
000000018035AC06 movss     dword ptr [rsp + 0x70], xmm8
000000018035AC0D mov       rcx, qword ptr [rsi + 0x1a0]
000000018035AC14 test      rcx, rcx
000000018035AC17 je        0x18035b123
000000018035AC1D xor       edx, edx
000000018035AC1F call      0x18131c070
000000018035AC24 mov       rbx, rax
000000018035AC27 test      rax, rax
000000018035AC2A je        0x18035b123
000000018035AC30 xor       r8d, r8d
000000018035AC33 mov       rdx, rax
000000018035AC36 lea       rcx, [rsp + 0x40]
000000018035AC3B call      0x18132f3a0
000000018035AC40 movsd     xmm1, qword ptr [rax]
000000018035AC44 movss     xmm3, dword ptr [rax + 8]
000000018035AC49 addss     xmm3, xmm8
000000018035AC4E movaps    xmm2, xmm1
000000018035AC51 shufps    xmm2, xmm2, 0x55
000000018035AC55 addss     xmm2, xmm6
000000018035AC59 movaps    xmm0, xmm7
000000018035AC5C movsd     qword ptr [rsp + 0x40], xmm1
000000018035AC62 addss     xmm0, xmm1
000000018035AC66 unpcklps  xmm0, xmm2
000000018035AC69 movsd     qword ptr [rsp + 0x30], xmm0
000000018035AC6F movss     dword ptr [rsp + 0x38], xmm3
000000018035AC75 xor       r8d, r8d
000000018035AC78 lea       rdx, [rsp + 0x30]
000000018035AC7D mov       rcx, rbx
000000018035AC80 call      0x18132fbb0
000000018035AC85 mov       rcx, qword ptr [rsi + 0x1b0]
000000018035AC8C test      rcx, rcx
000000018035AC8F je        0x18035b123
000000018035AC95 xor       edx, edx
000000018035AC97 call      0x18131c070
000000018035AC9C mov       rbx, rax
000000018035AC9F test      rax, rax
000000018035ACA2 je        0x18035b123
000000018035ACA8 xor       r8d, r8d
000000018035ACAB mov       rdx, rax
000000018035ACAE lea       rcx, [rsp + 0x40]
000000018035ACB3 call      0x18132f3a0
000000018035ACB8 movsd     xmm1, qword ptr [rax]
000000018035ACBC movss     xmm3, dword ptr [rax + 8]
000000018035ACC1 addss     xmm3, xmm8
000000018035ACC6 movaps    xmm2, xmm1
000000018035ACC9 shufps    xmm2, xmm2, 0x55
000000018035ACCD addss     xmm2, xmm6
000000018035ACD1 movaps    xmm0, xmm7
000000018035ACD4 movsd     qword ptr [rsp + 0x40], xmm1
000000018035ACDA addss     xmm0, xmm1
000000018035ACDE unpcklps  xmm0, xmm2
000000018035ACE1 movsd     qword ptr [rsp + 0x30], xmm0
000000018035ACE7 movss     dword ptr [rsp + 0x38], xmm3
000000018035ACED xor       r8d, r8d
000000018035ACF0 lea       rdx, [rsp + 0x30]
000000018035ACF5 mov       rcx, rbx
000000018035ACF8 call      0x18132fbb0
000000018035ACFD mov       rcx, qword ptr [rsi + 0x1c0]
000000018035AD04 test      rcx, rcx
000000018035AD07 je        0x18035b123
000000018035AD0D xor       edx, edx
000000018035AD0F call      0x18131c070
000000018035AD14 mov       rbx, rax
000000018035AD17 test      rax, rax
000000018035AD1A je        0x18035b123
000000018035AD20 xor       r8d, r8d
000000018035AD23 mov       rdx, rax
000000018035AD26 lea       rcx, [rsp + 0x40]
000000018035AD2B call      0x18132f3a0
000000018035AD30 movsd     xmm1, qword ptr [rax]
000000018035AD34 movss     xmm3, dword ptr [rax + 8]
000000018035AD39 addss     xmm3, xmm8
000000018035AD3E movaps    xmm2, xmm1
000000018035AD41 shufps    xmm2, xmm2, 0x55
000000018035AD45 addss     xmm2, xmm6
000000018035AD49 movaps    xmm0, xmm7
000000018035AD4C movsd     qword ptr [rsp + 0x40], xmm1
000000018035AD52 addss     xmm0, xmm1
000000018035AD56 unpcklps  xmm0, xmm2
000000018035AD59 movsd     qword ptr [rsp + 0x30], xmm0
000000018035AD5F movss     dword ptr [rsp + 0x38], xmm3
000000018035AD65 xor       r8d, r8d
000000018035AD68 lea       rdx, [rsp + 0x30]
000000018035AD6D mov       rcx, rbx
000000018035AD70 call      0x18132fbb0
000000018035AD75 mov       rdx, qword ptr [rsi + 0x1c8]
000000018035AD7C test      rdx, rdx
000000018035AD7F je        0x18035b123
000000018035AD85 mov       r8, qword ptr [rip + 0x18510ec] # 0x181babe78
000000018035AD8C lea       rcx, [rsp + 0x50]
000000018035AD91 call      0x1808197f0
000000018035AD96 movups    xmm0, xmmword ptr [rsp + 0x50]
000000018035AD9B movups    xmmword ptr [rsp + 0xa0], xmm0
000000018035ADA3 movsd     xmm1, qword ptr [rsp + 0x60]
000000018035ADA9 movsd     qword ptr [rsp + 0xb0], xmm1
000000018035ADB2 mov       qword ptr [rsp + 0x50], 0
000000018035ADBB lea       rbx, [rsp + 0xa0]
000000018035ADC3 mov       qword ptr [rsp + 0x58], rbx
000000018035ADC8 nop       dword ptr [rax + rax]
000000018035ADD0 mov       rdx, qword ptr [rip + 0x18686e1] # 0x181bc34b8
000000018035ADD7 lea       rcx, [rsp + 0xa0]
000000018035ADDF call      0x180669280
000000018035ADE4 test      al, al
000000018035ADE6 je        0x18035ae8c
000000018035ADEC mov       rdi, qword ptr [rsp + 0xb0]
000000018035ADF4 mov       rcx, qword ptr [rip + 0x1855705] # 0x181bb0500
000000018035ADFB cmp       dword ptr [rcx + 0xe0], 0
000000018035AE02 jne       0x18035ae09
000000018035AE04 call      0x1802501e0
000000018035AE09 xor       edx, edx
000000018035AE0B mov       rcx, rdi
000000018035AE0E call      0x18131f870
000000018035AE13 test      al, al
000000018035AE15 je        0x18035add0
000000018035AE17 test      rdi, rdi
000000018035AE1A je        0x18035b12f
000000018035AE20 xor       edx, edx
000000018035AE22 mov       rcx, rdi
000000018035AE25 call      0x18131c070
000000018035AE2A mov       rdi, rax
000000018035AE2D test      rax, rax
000000018035AE30 je        0x18035b129
000000018035AE36 xor       r8d, r8d
000000018035AE39 mov       rdx, rax
000000018035AE3C lea       rcx, [rsp + 0x40]
000000018035AE41 call      0x18132f3a0
000000018035AE46 movsd     xmm2, qword ptr [rax]
000000018035AE4A movaps    xmm4, xmm8
000000018035AE4E addss     xmm4, dword ptr [rax + 8]
000000018035AE53 movaps    xmm3, xmm6
000000018035AE56 movaps    xmm0, xmm2
000000018035AE59 shufps    xmm0, xmm0, 0x55
000000018035AE5D addss     xmm3, xmm0
000000018035AE61 movaps    xmm1, xmm7
000000018035AE64 addss     xmm1, xmm2
000000018035AE68 unpcklps  xmm1, xmm3
000000018035AE6B movsd     qword ptr [rsp + 0x30], xmm1
000000018035AE71 movss     dword ptr [rsp + 0x38], xmm4
000000018035AE77 xor       r8d, r8d
000000018035AE7A lea       rdx, [rsp + 0x30]
000000018035AE7F mov       rcx, rdi
000000018035AE82 call      0x18132fbb0
000000018035AE87 jmp       0x18035add0
000000018035AE8C mov       rdx, qword ptr [rip + 0x18685cd] # 0x181bc3460
000000018035AE93 mov       rcx, rbx
000000018035AE96 call      0x180302170
000000018035AE9B jmp       0x18035aeef
000000018035AE9D mov       rdx, qword ptr [rip + 0x18685bc] # 0x181bc3460
000000018035AEA4 mov       rcx, qword ptr [rsp + 0x58]
000000018035AEA9 call      0x180302170
000000018035AEAE mov       rcx, qword ptr [rsp + 0x50]
000000018035AEB3 test      rcx, rcx
000000018035AEB6 jne       0x18035b135
000000018035AEBC mov       r14d, dword ptr [rsp + 0x140]
000000018035AEC4 mov       r15d, dword ptr [rsp + 0x138]
000000018035AECC mov       rsi, qword ptr [rsp + 0x130]
000000018035AED4 mov       r13d, dword ptr [rsp + 0x98]
000000018035AEDC movss     xmm7, dword ptr [rsp + 0x68]
000000018035AEE2 movss     xmm6, dword ptr [rsp + 0x6c]
000000018035AEE8 movss     xmm8, dword ptr [rsp + 0x70]
000000018035AEEF mov       rdx, qword ptr [rsi + 0x1d0]
000000018035AEF6 test      rdx, rdx
000000018035AEF9 je        0x18035b123
000000018035AEFF mov       r8, qword ptr [rip + 0x1850f72] # 0x181babe78
000000018035AF06 lea       rcx, [rsp + 0x50]
000000018035AF0B call      0x1808197f0
000000018035AF10 movups    xmm0, xmmword ptr [rsp + 0x50]
000000018035AF15 movups    xmmword ptr [rsp + 0xa0], xmm0
000000018035AF1D movsd     xmm1, qword ptr [rsp + 0x60]
000000018035AF23 movsd     qword ptr [rsp + 0xb0], xmm1
000000018035AF2C mov       qword ptr [rsp + 0x50], 0
000000018035AF35 lea       rbx, [rsp + 0xa0]
000000018035AF3D mov       qword ptr [rsp + 0x58], rbx
000000018035AF42 nop       dword ptr [rax]
000000018035AF46 nop       word ptr [rax + rax]
000000018035AF50 mov       rdx, qword ptr [rip + 0x1868561] # 0x181bc34b8
000000018035AF57 lea       rcx, [rsp + 0xa0]
000000018035AF5F call      0x180669280
000000018035AF64 test      al, al
000000018035AF66 je        0x18035b00c
000000018035AF6C mov       rdi, qword ptr [rsp + 0xb0]
000000018035AF74 mov       rcx, qword ptr [rip + 0x1855585] # 0x181bb0500
000000018035AF7B cmp       dword ptr [rcx + 0xe0], 0
000000018035AF82 jne       0x18035af89
000000018035AF84 call      0x1802501e0
000000018035AF89 xor       edx, edx
000000018035AF8B mov       rcx, rdi
000000018035AF8E call      0x18131f870
000000018035AF93 test      al, al
000000018035AF95 je        0x18035af50
000000018035AF97 test      rdi, rdi
000000018035AF9A je        0x18035b140
000000018035AFA0 xor       edx, edx
000000018035AFA2 mov       rcx, rdi
000000018035AFA5 call      0x18131c070
000000018035AFAA mov       rdi, rax
000000018035AFAD test      rax, rax
000000018035AFB0 je        0x18035b13b
000000018035AFB6 xor       r8d, r8d
000000018035AFB9 mov       rdx, rax
000000018035AFBC lea       rcx, [rsp + 0x40]
000000018035AFC1 call      0x18132f3a0
000000018035AFC6 movsd     xmm2, qword ptr [rax]
000000018035AFCA movaps    xmm4, xmm8
000000018035AFCE addss     xmm4, dword ptr [rax + 8]
000000018035AFD3 movaps    xmm3, xmm6
000000018035AFD6 movaps    xmm0, xmm2
000000018035AFD9 shufps    xmm0, xmm0, 0x55
000000018035AFDD addss     xmm3, xmm0
000000018035AFE1 movaps    xmm1, xmm7
000000018035AFE4 addss     xmm1, xmm2
000000018035AFE8 unpcklps  xmm1, xmm3
000000018035AFEB movsd     qword ptr [rsp + 0x30], xmm1
000000018035AFF1 movss     dword ptr [rsp + 0x38], xmm4
000000018035AFF7 xor       r8d, r8d
000000018035AFFA lea       rdx, [rsp + 0x30]
000000018035AFFF mov       rcx, rdi
000000018035B002 call      0x18132fbb0
000000018035B007 jmp       0x18035af50
000000018035B00C mov       rdx, qword ptr [rip + 0x186844d] # 0x181bc3460
000000018035B013 mov       rcx, rbx
000000018035B016 call      0x180302170
000000018035B01B jmp       0x18035b05c
000000018035B01D mov       rdx, qword ptr [rip + 0x186843c] # 0x181bc3460
000000018035B024 mov       rcx, qword ptr [rsp + 0x58]
000000018035B029 call      0x180302170
000000018035B02E mov       rcx, qword ptr [rsp + 0x50]
000000018035B033 test      rcx, rcx
000000018035B036 jne       0x18035b146
000000018035B03C mov       r14d, dword ptr [rsp + 0x140]
000000018035B044 mov       r15d, dword ptr [rsp + 0x138]
000000018035B04C mov       rsi, qword ptr [rsp + 0x130]
000000018035B054 mov       r13d, dword ptr [rsp + 0x98]
000000018035B05C mov       rcx, qword ptr [rsi + 0x1e8]
000000018035B063 test      rcx, rcx
000000018035B066 je        0x18035b123
000000018035B06C xor       r9d, r9d
000000018035B06F mov       r8d, dword ptr [rsi + 0x24]
000000018035B073 mov       edx, dword ptr [rsi + 0x20]
000000018035B076 call      0x180326fe0
000000018035B07B test      rax, rax
000000018035B07E je        0x18035b123
000000018035B084 xor       r8d, r8d
000000018035B087 mov       rdx, rsi
000000018035B08A mov       rcx, rax
000000018035B08D call      0x18032a610
000000018035B092 mov       dword ptr [rsi + 0x20], r15d
000000018035B096 mov       dword ptr [rsi + 0x24], r14d
000000018035B09A xor       edx, edx
000000018035B09C mov       rcx, rsi
000000018035B09F call      0x1803585b0
000000018035B0A4 mov       byte ptr [rsi + 0xb0], 1
000000018035B0AB movsd     xmm0, qword ptr [rsp + 0x90]
000000018035B0B4 movsd     qword ptr [rsp + 0x30], xmm0
000000018035B0BA mov       dword ptr [rsp + 0x38], r13d
000000018035B0BF xor       r8d, r8d
000000018035B0C2 lea       rdx, [rsp + 0x30]
000000018035B0C7 mov       rcx, rsi
000000018035B0CA call      0x18034e190
000000018035B0CF mov       rcx, qword ptr [rsi + 0x1e8]
000000018035B0D6 test      rcx, rcx
000000018035B0D9 je        0x18035b123
000000018035B0DB xor       r9d, r9d
000000018035B0DE mov       r8d, r14d
000000018035B0E1 mov       edx, r15d
000000018035B0E4 call      0x180326fe0
000000018035B0E9 test      rax, rax
000000018035B0EC je        0x18035b123
000000018035B0EE xor       r8d, r8d
000000018035B0F1 mov       rdx, rsi
000000018035B0F4 mov       rcx, rax
000000018035B0F7 call      0x18032a3e0
000000018035B0FC lea       r11, [rsp + 0x100]
000000018035B104 mov       rbx, qword ptr [r11 + 0x48]
000000018035B108 movaps    xmm6, xmmword ptr [r11 - 0x10]
000000018035B10D movaps    xmm7, xmmword ptr [r11 - 0x20]
000000018035B112 movaps    xmm8, xmmword ptr [r11 - 0x30]
000000018035B117 mov       rsp, r11
000000018035B11A pop       r15
000000018035B11C pop       r14
000000018035B11E pop       r13
000000018035B120 pop       rdi
000000018035B121 pop       rsi
000000018035B122 ret       
000000018035B123 call      0x180250150
000000018035B128 nop       
000000018035B129 call      0x180250150
000000018035B12E nop       
000000018035B12F call      0x180250150
000000018035B134 nop       
000000018035B135 call      0x180200e60
000000018035B13A nop       
000000018035B13B call      0x180250150
000000018035B140 call      0x180250150
000000018035B145 nop       
000000018035B146 call      0x180200e60
000000018035B14B int3      
000000018035B14C int3      
000000018035B14D int3      
000000018035B14E int3      
000000018035B14F int3      