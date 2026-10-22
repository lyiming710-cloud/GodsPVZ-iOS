00000001803BBDD0 push      rdi
00000001803BBDD2 sub       rsp, 0x40
00000001803BBDD6 cmp       byte ptr [rip + 0x192919f], 0 # 0x181ce4f7c
00000001803BBDDD mov       rdi, rcx
00000001803BBDE0 jne       0x1803bbe31
00000001803BBDE2 lea       rcx, [rip + 0x18072ef] # 0x181bc30d8
00000001803BBDE9 call      0x18024fef0
00000001803BBDEE lea       rcx, [rip + 0x17e8583] # 0x181ba4378
00000001803BBDF5 call      0x18024fef0
00000001803BBDFA lea       rcx, [rip + 0x17e8517] # 0x181ba4318
00000001803BBE01 call      0x18024fef0
00000001803BBE06 lea       rcx, [rip + 0x1804bc3] # 0x181bc09d0
00000001803BBE0D call      0x18024fef0
00000001803BBE12 lea       rcx, [rip + 0x17feedf] # 0x181bbacf8
00000001803BBE19 call      0x18024fef0
00000001803BBE1E lea       rcx, [rip + 0x1809d23] # 0x181bc5b48
00000001803BBE25 call      0x18024fef0
00000001803BBE2A mov       byte ptr [rip + 0x192914b], 1 # 0x181ce4f7c
00000001803BBE31 cmp       dword ptr [rdi + 0x74], 0
00000001803BBE35 mov       qword ptr [rsp + 0x58], rbx
00000001803BBE3A mov       qword ptr [rsp + 0x60], rsi
00000001803BBE3F je        0x1803bc0c8
00000001803BBE45 cmp       dword ptr [rdi + 0x74], 1
00000001803BBE49 jne       0x1803bc0b8
00000001803BBE4F mov       rcx, qword ptr [rdi + 0x28]
00000001803BBE53 test      rcx, rcx
00000001803BBE56 je        0x1803bc38d
00000001803BBE5C xor       edx, edx
00000001803BBE5E call      0x18131c070
00000001803BBE63 lea       rcx, [rdi + 0x48]
00000001803BBE67 mov       qword ptr [rdi + 0x48], rax
00000001803BBE6B mov       rdx, rax
00000001803BBE6E call      0x18024f360
00000001803BBE73 mov       rdx, qword ptr [rdi + 0x40]
00000001803BBE77 mov       rbx, qword ptr [rdi + 0x48]
00000001803BBE7B test      rdx, rdx
00000001803BBE7E je        0x1803bc38d
00000001803BBE84 xor       r8d, r8d
00000001803BBE87 lea       rcx, [rsp + 0x20]
00000001803BBE8C call      0x18132f3a0
00000001803BBE91 xorps     xmm1, xmm1
00000001803BBE94 movsd     xmm0, qword ptr [rax]
00000001803BBE98 movss     xmm4, dword ptr [rax + 8]
00000001803BBE9D movaps    xmm2, xmm0
00000001803BBEA0 shufps    xmm2, xmm2, 0x55
00000001803BBEA4 addss     xmm4, xmm1
00000001803BBEA8 addss     xmm2, dword ptr [rip + 0x11ebd38] # 0x1815a7be8
00000001803BBEB0 movaps    xmm3, xmm0
00000001803BBEB3 movsd     qword ptr [rsp + 0x30], xmm0
00000001803BBEB9 addss     xmm3, xmm1
00000001803BBEBD test      rbx, rbx
00000001803BBEC0 je        0x1803bc38d
00000001803BBEC6 unpcklps  xmm3, xmm2
00000001803BBEC9 lea       rdx, [rsp + 0x20]
00000001803BBECE xor       r8d, r8d
00000001803BBED1 movsd     qword ptr [rsp + 0x20], xmm3
00000001803BBED7 mov       rcx, rbx
00000001803BBEDA movss     dword ptr [rsp + 0x28], xmm4
00000001803BBEE0 call      0x18132fbb0
00000001803BBEE5 mov       rcx, qword ptr [rdi + 0x28]
00000001803BBEE9 test      rcx, rcx
00000001803BBEEC je        0x1803bc38d
00000001803BBEF2 mov       rdx, qword ptr [rip + 0x17e841f] # 0x181ba4318
00000001803BBEF9 call      0x18046d280
00000001803BBEFE lea       rcx, [rdi + 0x38]
00000001803BBF02 mov       qword ptr [rdi + 0x38], rax
00000001803BBF06 mov       rdx, rax
00000001803BBF09 call      0x18024f360
00000001803BBF0E mov       rdx, qword ptr [rip + 0x1804abb] # 0x181bc09d0
00000001803BBF15 mov       rcx, qword ptr [rip + 0x1809c2c] # 0x181bc5b48
00000001803BBF1C mov       rbx, qword ptr [rdi + 0x38]
00000001803BBF20 call      0x1804a6ab0
00000001803BBF25 test      rbx, rbx
00000001803BBF28 je        0x1803bc38d
00000001803BBF2E xor       r8d, r8d
00000001803BBF31 mov       rdx, rax
00000001803BBF34 mov       rcx, rbx
00000001803BBF37 call      0x1813bb9e0
00000001803BBF3C mov       rcx, qword ptr [rdi + 0x38]
00000001803BBF40 test      rcx, rcx
00000001803BBF43 je        0x1803bc38d
00000001803BBF49 mov       rdx, qword ptr [rip + 0x1807188] # 0x181bc30d8
00000001803BBF50 call      0x18042e9f0
00000001803BBF55 mov       rcx, qword ptr [rdi + 0x38]
00000001803BBF59 mov       rbx, rax
00000001803BBF5C test      rcx, rcx
00000001803BBF5F je        0x1803bc38d
00000001803BBF65 xor       edx, edx
00000001803BBF67 call      0x1813bb880
00000001803BBF6C test      rax, rax
00000001803BBF6F je        0x1803bc38d
00000001803BBF75 xor       edx, edx
00000001803BBF77 mov       rcx, rax
00000001803BBF7A call      0x1813b9880
00000001803BBF7F test      rbx, rbx
00000001803BBF82 je        0x1803bc38d
00000001803BBF88 xor       r8d, r8d
00000001803BBF8B mov       rdx, rax
00000001803BBF8E mov       rcx, rbx
00000001803BBF91 call      0x1812fb060
00000001803BBF96 mov       rsi, qword ptr [rdi + 0x38]
00000001803BBF9A xor       r8d, r8d
00000001803BBF9D mov       edx, 0xff
00000001803BBFA2 xor       ecx, ecx
00000001803BBFA4 call      0x18130d310
00000001803BBFA9 mov       ebx, eax
00000001803BBFAB mov       dword ptr [rsp + 0x50], 0
00000001803BBFB3 xor       r8d, r8d
00000001803BBFB6 mov       byte ptr [rsp + 0x50], bl
00000001803BBFBA mov       edx, 0xff
00000001803BBFBF xor       ecx, ecx
00000001803BBFC1 call      0x18130d310
00000001803BBFC6 xor       r8d, r8d
00000001803BBFC9 mov       byte ptr [rsp + 0x51], al
00000001803BBFCD mov       edx, 0xff
00000001803BBFD2 xor       ecx, ecx
00000001803BBFD4 call      0x18130d310
00000001803BBFD9 movss     xmm3, dword ptr [rip + 0x11ebfab] # 0x1815a7f8c
00000001803BBFE1 mov       byte ptr [rsp + 0x52], al
00000001803BBFE5 movzx     eax, bl
00000001803BBFE8 mov       byte ptr [rsp + 0x53], 0xff
00000001803BBFED movd      xmm4, eax
00000001803BBFF1 mov       eax, dword ptr [rsp + 0x50]
00000001803BBFF5 mov       ecx, eax
00000001803BBFF7 shr       ecx, 8
00000001803BBFFA movzx     ecx, cl
00000001803BBFFD cvtdq2ps  xmm4, xmm4
00000001803BC000 movd      xmm2, ecx
00000001803BC004 mov       ecx, eax
00000001803BC006 shr       ecx, 0x10
00000001803BC009 movzx     ecx, cl
00000001803BC00C shr       eax, 0x18
00000001803BC00F cvtdq2ps  xmm2, xmm2
00000001803BC012 movd      xmm1, ecx
00000001803BC016 movd      xmm0, eax
00000001803BC01A cvtdq2ps  xmm1, xmm1
00000001803BC01D cvtdq2ps  xmm0, xmm0
00000001803BC020 divss     xmm1, xmm3
00000001803BC024 divss     xmm0, xmm3
00000001803BC028 divss     xmm4, xmm3
00000001803BC02C divss     xmm2, xmm3
00000001803BC030 movss     dword ptr [rsp + 0x30], xmm4
00000001803BC036 movss     dword ptr [rsp + 0x34], xmm2
00000001803BC03C movss     dword ptr [rsp + 0x38], xmm1
00000001803BC042 movss     dword ptr [rsp + 0x3c], xmm0
00000001803BC048 test      rsi, rsi
00000001803BC04B je        0x1803bc38d
00000001803BC051 movups    xmm0, xmmword ptr [rsp + 0x30]
00000001803BC056 xor       r8d, r8d
00000001803BC059 lea       rdx, [rsp + 0x30]
00000001803BC05E mov       rcx, rsi
00000001803BC061 movdqa    xmmword ptr [rsp + 0x30], xmm0
00000001803BC067 call      0x1813bb950
00000001803BC06C mov       rcx, qword ptr [rdi + 0x38]
00000001803BC070 test      rcx, rcx
00000001803BC073 je        0x1803bc38d
00000001803BC079 xor       r8d, r8d
00000001803BC07C mov       edx, 7
00000001803BC081 call      0x1813bb8c0
00000001803BC086 mov       rcx, qword ptr [rdi + 0x38]
00000001803BC08A test      rcx, rcx
00000001803BC08D je        0x1803bc38d
00000001803BC093 xor       r8d, r8d
00000001803BC096 mov       edx, 0x18
00000001803BC09B call      0x1813bb9a0
00000001803BC0A0 xor       edx, edx
00000001803BC0A2 mov       rcx, rdi
00000001803BC0A5 call      0x1803bbcf0
00000001803BC0AA xor       r8d, r8d
00000001803BC0AD mov       rdx, rax
00000001803BC0B0 mov       rcx, rdi
00000001803BC0B3 call      0x18131dbe0
00000001803BC0B8 mov       rsi, qword ptr [rsp + 0x60]
00000001803BC0BD mov       rbx, qword ptr [rsp + 0x58]
00000001803BC0C2 add       rsp, 0x40
00000001803BC0C6 pop       rdi
00000001803BC0C7 ret       
00000001803BC0C8 mov       rcx, qword ptr [rdi + 0x28]
00000001803BC0CC test      rcx, rcx
00000001803BC0CF je        0x1803bc38d
00000001803BC0D5 mov       rdx, qword ptr [rip + 0x17e829c] # 0x181ba4378
00000001803BC0DC call      0x18046d280
00000001803BC0E1 lea       rcx, [rdi + 0x30]
00000001803BC0E5 mov       qword ptr [rdi + 0x30], rax
00000001803BC0E9 mov       rdx, rax
00000001803BC0EC call      0x18024f360
00000001803BC0F1 mov       rcx, qword ptr [rdi + 0x30]
00000001803BC0F5 test      rcx, rcx
00000001803BC0F8 je        0x1803bc38d
00000001803BC0FE xor       edx, edx
00000001803BC100 call      0x1811b84f0
00000001803BC105 movss     xmm1, dword ptr [rip + 0x11eb917] # 0x1815a7a24
00000001803BC10D test      rax, rax
00000001803BC110 je        0x1803bc38d
00000001803BC116 movaps    xmm0, xmm1
00000001803BC119 xor       r8d, r8d
00000001803BC11C unpcklps  xmm0, xmm1
00000001803BC11F mov       rcx, rax
00000001803BC122 movq      rdx, xmm0
00000001803BC127 call      0x181325360
00000001803BC12C mov       rcx, qword ptr [rdi + 0x28]
00000001803BC130 test      rcx, rcx
00000001803BC133 je        0x1803bc38d
00000001803BC139 xor       edx, edx
00000001803BC13B call      0x18131c070
00000001803BC140 lea       rcx, [rdi + 0x48]
00000001803BC144 mov       qword ptr [rdi + 0x48], rax
00000001803BC148 mov       rdx, rax
00000001803BC14B call      0x18024f360
00000001803BC150 mov       rdx, qword ptr [rdi + 0x40]
00000001803BC154 mov       rbx, qword ptr [rdi + 0x48]
00000001803BC158 test      rdx, rdx
00000001803BC15B je        0x1803bc38d
00000001803BC161 xor       r8d, r8d
00000001803BC164 lea       rcx, [rsp + 0x30]
00000001803BC169 call      0x18132f3a0
00000001803BC16E xorps     xmm1, xmm1
00000001803BC171 movsd     xmm0, qword ptr [rax]
00000001803BC175 movss     xmm4, dword ptr [rax + 8]
00000001803BC17A movaps    xmm2, xmm0
00000001803BC17D shufps    xmm2, xmm2, 0x55
00000001803BC181 addss     xmm4, xmm1
00000001803BC185 addss     xmm2, dword ptr [rip + 0x11eba5b] # 0x1815a7be8
00000001803BC18D movaps    xmm3, xmm0
00000001803BC190 movsd     qword ptr [rsp + 0x20], xmm0
00000001803BC196 addss     xmm3, xmm1
00000001803BC19A test      rbx, rbx
00000001803BC19D je        0x1803bc38d
00000001803BC1A3 unpcklps  xmm3, xmm2
00000001803BC1A6 lea       rdx, [rsp + 0x20]
00000001803BC1AB xor       r8d, r8d
00000001803BC1AE movsd     qword ptr [rsp + 0x20], xmm3
00000001803BC1B4 mov       rcx, rbx
00000001803BC1B7 movss     dword ptr [rsp + 0x28], xmm4
00000001803BC1BD call      0x18132fbb0
00000001803BC1C2 mov       rcx, qword ptr [rdi + 0x30]
00000001803BC1C6 test      rcx, rcx
00000001803BC1C9 je        0x1803bc38d
00000001803BC1CF xor       r8d, r8d
00000001803BC1D2 mov       edx, 0x202
00000001803BC1D7 call      0x1811b8c90
00000001803BC1DC mov       rsi, qword ptr [rdi + 0x30]
00000001803BC1E0 xor       r8d, r8d
00000001803BC1E3 mov       edx, 0xff
00000001803BC1E8 xor       ecx, ecx
00000001803BC1EA call      0x18130d310
00000001803BC1EF mov       ebx, eax
00000001803BC1F1 mov       dword ptr [rsp + 0x50], 0
00000001803BC1F9 xor       r8d, r8d
00000001803BC1FC mov       byte ptr [rsp + 0x50], bl
00000001803BC200 mov       edx, 0xff
00000001803BC205 xor       ecx, ecx
00000001803BC207 call      0x18130d310
00000001803BC20C xor       r8d, r8d
00000001803BC20F mov       byte ptr [rsp + 0x51], al
00000001803BC213 mov       edx, 0xff
00000001803BC218 xor       ecx, ecx
00000001803BC21A call      0x18130d310
00000001803BC21F movss     xmm0, dword ptr [rip + 0x11ebd65] # 0x1815a7f8c
00000001803BC227 mov       byte ptr [rsp + 0x52], al
00000001803BC22B movzx     eax, bl
00000001803BC22E mov       byte ptr [rsp + 0x53], 0xff
00000001803BC233 movd      xmm1, eax
00000001803BC237 mov       eax, dword ptr [rsp + 0x50]
00000001803BC23B mov       ecx, eax
00000001803BC23D shr       ecx, 8
00000001803BC240 movzx     ecx, cl
00000001803BC243 cvtdq2ps  xmm1, xmm1
00000001803BC246 movd      xmm2, ecx
00000001803BC24A mov       ecx, eax
00000001803BC24C shr       ecx, 0x10
00000001803BC24F movzx     ecx, cl
00000001803BC252 shr       eax, 0x18
00000001803BC255 cvtdq2ps  xmm2, xmm2
00000001803BC258 movd      xmm3, ecx
00000001803BC25C movd      xmm4, eax
00000001803BC260 cvtdq2ps  xmm3, xmm3
00000001803BC263 cvtdq2ps  xmm4, xmm4
00000001803BC266 divss     xmm3, xmm0
00000001803BC26A divss     xmm4, xmm0
00000001803BC26E divss     xmm1, xmm0
00000001803BC272 divss     xmm2, xmm0
00000001803BC276 test      rsi, rsi
00000001803BC279 je        0x1803bc38d
00000001803BC27F mov       rax, qword ptr [rsi]
00000001803BC282 lea       rdx, [rsp + 0x30]
00000001803BC287 shufps    xmm1, xmm1, 0xe1
00000001803BC28B mov       rcx, rsi
00000001803BC28E movss     xmm1, xmm2
00000001803BC292 shufps    xmm1, xmm1, 0xc6
00000001803BC296 mov       r8, qword ptr [rax + 0x2b0]
00000001803BC29D movss     xmm1, xmm3
00000001803BC2A1 shufps    xmm1, xmm1, 0x27
00000001803BC2A5 movss     xmm1, xmm4
00000001803BC2A9 shufps    xmm1, xmm1, 0x39
00000001803BC2AD movaps    xmmword ptr [rsp + 0x30], xmm1
00000001803BC2B2 movdqa    xmmword ptr [rsp + 0x30], xmm1
00000001803BC2B8 call      qword ptr [rax + 0x2a8]
00000001803BC2BE mov       rcx, qword ptr [rdi + 0x30]
00000001803BC2C2 test      rcx, rcx
00000001803BC2C5 je        0x1803bc38d
00000001803BC2CB movss     xmm1, dword ptr [rip + 0x11eb91d] # 0x1815a7bf0
00000001803BC2D3 xor       r8d, r8d
00000001803BC2D6 call      0x1811b9410
00000001803BC2DB mov       rcx, qword ptr [rdi + 0x30]
00000001803BC2DF test      rcx, rcx
00000001803BC2E2 je        0x1803bc38d
00000001803BC2E8 xor       r8d, r8d
00000001803BC2EB xor       edx, edx
00000001803BC2ED call      0x1811b8f80
00000001803BC2F2 mov       rcx, qword ptr [rdi + 0x30]
00000001803BC2F6 test      rcx, rcx
00000001803BC2F9 je        0x1803bc38d
00000001803BC2FF mov       rax, qword ptr [rip + 0x17fe9f2] # 0x181bbacf8
00000001803BC306 mov       r9, qword ptr [rcx]
00000001803BC309 mov       rdx, qword ptr [rax + 0xb8]
00000001803BC310 mov       r8, qword ptr [r9 + 0x560]
00000001803BC317 mov       rdx, qword ptr [rdx]
00000001803BC31A call      qword ptr [r9 + 0x558]
00000001803BC321 mov       rcx, qword ptr [rdi + 0x30]
00000001803BC325 test      rcx, rcx
00000001803BC328 je        0x1803bc38d
00000001803BC32A movzx     edx, byte ptr [rdi + 0x78]
00000001803BC32E xor       r8d, r8d
00000001803BC331 call      0x1811b9780
00000001803BC336 cmp       byte ptr [rip + 0x1928c40], 0 # 0x181ce4f7d
00000001803BC33D jne       0x1803bc352
00000001803BC33F lea       rcx, [rip + 0x17e2442] # 0x181b9e788
00000001803BC346 call      0x18024fef0
00000001803BC34B mov       byte ptr [rip + 0x1928c2b], 1 # 0x181ce4f7d
00000001803BC352 mov       rcx, qword ptr [rip + 0x17e242f] # 0x181b9e788
00000001803BC359 call      0x180250100
00000001803BC35E xor       edx, edx
00000001803BC360 mov       rcx, rax
00000001803BC363 mov       rbx, rax
00000001803BC366 call      0x180302170
00000001803BC36B lea       rcx, [rbx + 0x20]
00000001803BC36F mov       dword ptr [rbx + 0x10], 0
00000001803BC376 mov       rdx, rdi
00000001803BC379 mov       qword ptr [rbx + 0x20], rdi
00000001803BC37D call      0x18024f360
00000001803BC382 xor       r8d, r8d
00000001803BC385 mov       rdx, rbx
00000001803BC388 jmp       0x1803bc0b0
00000001803BC38D call      0x180250150
00000001803BC392 int3      
00000001803BC393 int3      
00000001803BC394 int3      
00000001803BC395 int3      
00000001803BC396 int3      
00000001803BC397 int3      
00000001803BC398 int3      
00000001803BC399 int3      
00000001803BC39A int3      
00000001803BC39B int3      
00000001803BC39C int3      
00000001803BC39D int3      
00000001803BC39E int3      
00000001803BC39F int3      