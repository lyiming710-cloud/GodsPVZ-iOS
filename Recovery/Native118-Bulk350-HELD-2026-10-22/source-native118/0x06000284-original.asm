000000018032B0A0 push      rbp
000000018032B0A2 push      rsi
000000018032B0A3 push      rdi
000000018032B0A4 sub       rsp, 0x90
000000018032B0AB cmp       byte ptr [rip + 0x19b9bb5], 0 # 0x181ce4c67
000000018032B0B2 mov       rdi, r9
000000018032B0B5 mov       ebp, r8d
000000018032B0B8 mov       rsi, rdx
000000018032B0BB jne       0x18032b0dc
000000018032B0BD lea       rcx, [rip + 0x187f5a4] # 0x181baa668
000000018032B0C4 call      0x18024fef0
000000018032B0C9 lea       rcx, [rip + 0x1881b28] # 0x181bacbf8
000000018032B0D0 call      0x18024fef0
000000018032B0D5 mov       byte ptr [rip + 0x19b9b8b], 1 # 0x181ce4c67
000000018032B0DC mov       qword ptr [rsp + 0xb0], rbx
000000018032B0E4 mov       qword ptr [rsp + 0xb8], r12
000000018032B0EC mov       qword ptr [rsp + 0xc8], r13
000000018032B0F4 mov       qword ptr [rsp + 0x88], r14
000000018032B0FC mov       qword ptr [rsp + 0x80], r15
000000018032B104 movaps    xmmword ptr [rsp + 0x70], xmm6
000000018032B109 movaps    xmmword ptr [rsp + 0x60], xmm7
000000018032B10E movaps    xmmword ptr [rsp + 0x50], xmm8
000000018032B114 test      ebp, ebp
000000018032B116 jg        0x18032b123
000000018032B118 test      rsi, rsi
000000018032B11B je        0x18032b8e9
000000018032B121 jmp       0x18032b141
000000018032B123 test      rsi, rsi
000000018032B126 je        0x18032b8e9
000000018032B12C mov       r8, qword ptr [rip + 0x187f535] # 0x181baa668
000000018032B133 lea       edx, [rbp - 1]
000000018032B136 mov       rcx, rsi
000000018032B139 call      0x180825710
000000018032B13E mov       rdi, rax
000000018032B141 mov       r8, qword ptr [rip + 0x187f520] # 0x181baa668
000000018032B148 mov       edx, ebp
000000018032B14A mov       rcx, rsi
000000018032B14D call      0x180825710
000000018032B152 mov       r8, qword ptr [rip + 0x187f50f] # 0x181baa668
000000018032B159 lea       edx, [rbp + 1]
000000018032B15C mov       rcx, rsi
000000018032B15F mov       rbx, rax
000000018032B162 call      0x180825710
000000018032B167 mov       rsi, rax
000000018032B16A test      rbx, rbx
000000018032B16D je        0x18032b8e9
000000018032B173 movss     xmm1, dword ptr [rbx + 0x24]
000000018032B178 xorps     xmm0, xmm0
000000018032B17B comiss    xmm1, xmm0
000000018032B17E ja        0x18032b3e4
000000018032B184 cmp       byte ptr [rbx + 0x29], 0
000000018032B188 jne       0x18032b3e4
000000018032B18E test      rdi, rdi
000000018032B191 je        0x18032b8e9
000000018032B197 mov       r12d, dword ptr [rdi + 0x14]
000000018032B19B mov       r14d, dword ptr [rdi + 0x10]
000000018032B19F mov       dword ptr [rsp + 0xc0], r12d
000000018032B1A7 test      rax, rax
000000018032B1AA je        0x18032b8e9
000000018032B1B0 mov       r8d, dword ptr [rax + 0x10]
000000018032B1B4 mov       ecx, dword ptr [rax + 0x14]
000000018032B1B7 sub       r8d, dword ptr [rbx + 0x10]
000000018032B1BB mov       r9d, dword ptr [rbx + 0x10]
000000018032B1BF mov       eax, r8d
000000018032B1C2 mov       edx, dword ptr [rbx + 0x14]
000000018032B1C5 sub       r9d, r14d
000000018032B1C8 sub       ecx, dword ptr [rbx + 0x14]
000000018032B1CB sub       edx, r12d
000000018032B1CE imul      eax, r9d
000000018032B1D2 test      eax, eax
000000018032B1D4 jne       0x18032b1e2
000000018032B1D6 test      edx, edx
000000018032B1D8 jne       0x18032b1e2
000000018032B1DA test      ecx, ecx
000000018032B1DC je        0x18032b8e2
000000018032B1E2 imul      ecx, edx
000000018032B1E5 test      ecx, ecx
000000018032B1E7 jne       0x18032b1f7
000000018032B1E9 test      r9d, r9d
000000018032B1EC jne       0x18032b1f7
000000018032B1EE test      r8d, r8d
000000018032B1F1 je        0x18032b8e2
000000018032B1F7 mov       rcx, qword ptr [rip + 0x18819fa] # 0x181bacbf8
000000018032B1FE mov       ebx, dword ptr [rsi + 0x10]
000000018032B201 sub       ebx, r14d
000000018032B204 cmp       dword ptr [rcx + 0xe0], 0
000000018032B20B jne       0x18032b212
000000018032B20D call      0x1802501e0
000000018032B212 xor       edx, edx
000000018032B214 mov       ecx, ebx
000000018032B216 call      0x180ca64e0
000000018032B21B mov       esi, dword ptr [rsi + 0x14]
000000018032B21E xor       edx, edx
000000018032B220 sub       esi, dword ptr [rdi + 0x14]
000000018032B223 mov       ecx, esi
000000018032B225 mov       dword ptr [rsp + 0x30], eax
000000018032B229 call      0x180ca64e0
000000018032B22E mov       edx, esi
000000018032B230 movd      xmm7, esi
000000018032B234 neg       edx
000000018032B236 movd      xmm0, ebx
000000018032B23A mov       ecx, ebx
000000018032B23C mov       dword ptr [rsp + 0x34], eax
000000018032B240 cmovs     edx, esi
000000018032B243 neg       ecx
000000018032B245 cvtdq2ps  xmm7, xmm7
000000018032B248 cmovs     ecx, ebx
000000018032B24B cvtdq2ps  xmm0, xmm0
000000018032B24E divss     xmm7, xmm0
000000018032B252 cmp       ecx, edx
000000018032B254 je        0x18032b78a
000000018032B25A mov       rcx, qword ptr [rip + 0x1881997] # 0x181bacbf8
000000018032B261 xor       eax, eax
000000018032B263 mov       qword ptr [rsp + 0x40], rax
000000018032B268 mov       r13d, eax
000000018032B26B cmp       dword ptr [rcx + 0xe0], eax
000000018032B271 jne       0x18032b278
000000018032B273 call      0x1802501e0
000000018032B278 movdqa    xmm8, xmmword ptr [rip + 0x127c8ff] # 0x1815a7b80
000000018032B281 movaps    xmm0, xmm7
000000018032B284 movss     xmm6, dword ptr [rip + 0x127c784] # 0x1815a7a10
000000018032B28C andps     xmm0, xmm8
000000018032B290 comiss    xmm6, xmm0
000000018032B293 mov       r15d, dword ptr [rsp + 0xd8]
000000018032B29B mov       r12d, 1
000000018032B2A1 ja        0x18032b584
000000018032B2A7 mov       rdi, qword ptr [rsp + 0xd0]
000000018032B2AF mov       ebx, esi
000000018032B2B1 mov       ebp, dword ptr [rsp + 0xc0]
000000018032B2B8 neg       ebx
000000018032B2BA divss     xmm6, xmm7
000000018032B2BE cmovs     ebx, esi
000000018032B2C1 add       ebx, ebx
000000018032B2C3 mov       dword ptr [rsp + 0x38], ebx
000000018032B2C7 movss     xmm7, dword ptr [rip + 0x127c8c5] # 0x1815a7b94
000000018032B2CF nop       
000000018032B2D0 mov       rcx, qword ptr [rip + 0x1881921] # 0x181bacbf8
000000018032B2D7 cmp       dword ptr [rcx + 0xe0], 0
000000018032B2DE jne       0x18032b2e5
000000018032B2E0 call      0x1802501e0
000000018032B2E5 cmp       r12d, ebx
000000018032B2E8 jge       0x18032b8e2
000000018032B2EE mov       rcx, qword ptr [rip + 0x1881903] # 0x181bacbf8
000000018032B2F5 cmp       dword ptr [rcx + 0xe0], 0
000000018032B2FC jne       0x18032b303
000000018032B2FE call      0x1802501e0
000000018032B303 mov       r9d, dword ptr [rsp + 0x34]
000000018032B308 movd      xmm1, r12d
000000018032B30D cvtdq2ps  xmm1, xmm1
000000018032B310 movd      xmm0, r9d
000000018032B315 cvtdq2ps  xmm0, xmm0
000000018032B318 mulss     xmm0, xmm7
000000018032B31C subss     xmm1, xmm0
000000018032B320 mulss     xmm1, xmm6
000000018032B324 andps     xmm1, xmm8
000000018032B328 mov       r8, qword ptr [rsp + 0x40]
000000018032B32D lea       eax, [r8*2 + 1]
000000018032B335 movd      xmm0, eax
000000018032B339 lea       esi, [r8 + 1]
000000018032B33D cvtdq2ps  xmm0, xmm0
000000018032B340 mov       eax, r12d
000000018032B343 cdq       
000000018032B344 comiss    xmm1, xmm0
000000018032B347 cmovb     esi, r8d
000000018032B34B sub       eax, edx
000000018032B34D sar       eax, 1
000000018032B34F cmp       eax, r13d
000000018032B352 je        0x18032b429
000000018032B358 mov       ecx, esi
000000018032B35A mov       r13d, eax
000000018032B35D imul      esi, dword ptr [rsp + 0x30]
000000018032B362 imul      eax, r9d
000000018032B366 mov       qword ptr [rsp + 0x40], rcx
000000018032B36B add       esi, r14d
000000018032B36E cmp       byte ptr [rip + 0x19b9905], 0 # 0x181ce4c7a
000000018032B375 lea       ebx, [rax + rbp]
000000018032B378 jne       0x18032b399
000000018032B37A lea       rcx, [rip + 0x1887a47] # 0x181bb2dc8
000000018032B381 call      0x18024fef0
000000018032B386 lea       rcx, [rip + 0x1887a93] # 0x181bb2e20
000000018032B38D call      0x18024fef0
000000018032B392 mov       byte ptr [rip + 0x19b98e1], 1 # 0x181ce4c7a
000000018032B399 cmp       esi, r15d
000000018032B39C jge       0x18032b8e9
000000018032B3A2 imul      ebx, r15d
000000018032B3A6 add       ebx, esi
000000018032B3A8 test      rdi, rdi
000000018032B3AB je        0x18032b8e9
000000018032B3B1 cmp       dword ptr [rdi + 0x18], ebx
000000018032B3B4 jle       0x18032b8e9
000000018032B3BA mov       r8, qword ptr [rip + 0x1887a5f] # 0x181bb2e20
000000018032B3C1 mov       edx, ebx
000000018032B3C3 mov       rcx, rdi
000000018032B3C6 call      0x180825710
000000018032B3CB test      rax, rax
000000018032B3CE je        0x18032b8e9
000000018032B3D4 mov       ecx, dword ptr [rsp + 0xe0]
000000018032B3DB cmp       dword ptr [rax + 0x18], ecx
000000018032B3DE jle       0x18032b578
000000018032B3E4 xor       al, al
000000018032B3E6 movaps    xmm8, xmmword ptr [rsp + 0x50]
000000018032B3EC movaps    xmm7, xmmword ptr [rsp + 0x60]
000000018032B3F1 movaps    xmm6, xmmword ptr [rsp + 0x70]
000000018032B3F6 mov       r15, qword ptr [rsp + 0x80]
000000018032B3FE mov       r14, qword ptr [rsp + 0x88]
000000018032B406 mov       r13, qword ptr [rsp + 0xc8]
000000018032B40E mov       r12, qword ptr [rsp + 0xb8]
000000018032B416 mov       rbx, qword ptr [rsp + 0xb0]
000000018032B41E add       rsp, 0x90
000000018032B425 pop       rdi
000000018032B426 pop       rsi
000000018032B427 pop       rbp
000000018032B428 ret       
000000018032B429 lea       eax, [r8*2 + 1]
000000018032B431 movd      xmm0, eax
000000018032B435 cvtdq2ps  xmm0, xmm0
000000018032B438 comiss    xmm0, xmm1
000000018032B43B jb        0x18032b4df
000000018032B441 lea       ebx, [r13 + 1]
000000018032B445 mov       ebp, r8d
000000018032B448 imul      ebp, dword ptr [rsp + 0x30]
000000018032B44D imul      ebx, r9d
000000018032B451 add       ebp, r14d
000000018032B454 add       ebx, dword ptr [rsp + 0xc0]
000000018032B45B cmp       byte ptr [rip + 0x19b9818], 0 # 0x181ce4c7a
000000018032B462 jne       0x18032b483
000000018032B464 lea       rcx, [rip + 0x188795d] # 0x181bb2dc8
000000018032B46B call      0x18024fef0
000000018032B470 lea       rcx, [rip + 0x18879a9] # 0x181bb2e20
000000018032B477 call      0x18024fef0
000000018032B47C mov       byte ptr [rip + 0x19b97f7], 1 # 0x181ce4c7a
000000018032B483 cmp       ebp, r15d
000000018032B486 jge       0x18032b8e9
000000018032B48C imul      ebx, r15d
000000018032B490 add       ebx, ebp
000000018032B492 test      rdi, rdi
000000018032B495 je        0x18032b8e9
000000018032B49B cmp       dword ptr [rdi + 0x18], ebx
000000018032B49E jle       0x18032b8e9
000000018032B4A4 mov       r8, qword ptr [rip + 0x1887975] # 0x181bb2e20
000000018032B4AB mov       edx, ebx
000000018032B4AD mov       rcx, rdi
000000018032B4B0 call      0x180825710
000000018032B4B5 test      rax, rax
000000018032B4B8 je        0x18032b8e9
000000018032B4BE mov       ecx, dword ptr [rsp + 0xe0]
000000018032B4C5 cmp       dword ptr [rax + 0x18], ecx
000000018032B4C8 jg        0x18032b3e4
000000018032B4CE mov       ebp, dword ptr [rsp + 0xc0]
000000018032B4D5 mov       r8, qword ptr [rsp + 0x40]
000000018032B4DA mov       r9d, dword ptr [rsp + 0x34]
000000018032B4DF cmp       esi, r8d
000000018032B4E2 je        0x18032b578
000000018032B4E8 imul      esi, dword ptr [rsp + 0x30]
000000018032B4ED mov       ebx, r13d
000000018032B4F0 imul      ebx, r9d
000000018032B4F4 add       esi, r14d
000000018032B4F7 add       ebx, dword ptr [rsp + 0xc0]
000000018032B4FE cmp       byte ptr [rip + 0x19b9775], 0 # 0x181ce4c7a
000000018032B505 jne       0x18032b526
000000018032B507 lea       rcx, [rip + 0x18878ba] # 0x181bb2dc8
000000018032B50E call      0x18024fef0
000000018032B513 lea       rcx, [rip + 0x1887906] # 0x181bb2e20
000000018032B51A call      0x18024fef0
000000018032B51F mov       byte ptr [rip + 0x19b9754], 1 # 0x181ce4c7a
000000018032B526 cmp       esi, r15d
000000018032B529 jge       0x18032b8e9
000000018032B52F imul      ebx, r15d
000000018032B533 add       ebx, esi
000000018032B535 test      rdi, rdi
000000018032B538 je        0x18032b8e9
000000018032B53E cmp       dword ptr [rdi + 0x18], ebx
000000018032B541 jle       0x18032b8e9
000000018032B547 mov       r8, qword ptr [rip + 0x18878d2] # 0x181bb2e20
000000018032B54E mov       edx, ebx
000000018032B550 mov       rcx, rdi
000000018032B553 call      0x180825710
000000018032B558 test      rax, rax
000000018032B55B je        0x18032b8e9
000000018032B561 mov       ecx, dword ptr [rsp + 0xe0]
000000018032B568 cmp       dword ptr [rax + 0x18], ecx
000000018032B56B jg        0x18032b3e4
000000018032B571 mov       ebp, dword ptr [rsp + 0xc0]
000000018032B578 mov       ebx, dword ptr [rsp + 0x38]
000000018032B57C inc       r12d
000000018032B57F jmp       0x18032b2d0
000000018032B584 mov       rbp, qword ptr [rsp + 0xd0]
000000018032B58C mov       edi, ebx
000000018032B58E movss     xmm6, dword ptr [rip + 0x127c5fe] # 0x1815a7b94
000000018032B596 neg       edi
000000018032B598 cmovs     edi, ebx
000000018032B59B add       edi, edi
000000018032B59D mov       dword ptr [rsp + 0x38], edi
000000018032B5A1 xor       esi, esi
000000018032B5A3 nop       dword ptr [rax]
000000018032B5A7 nop       word ptr [rax + rax]
000000018032B5B0 mov       rcx, qword ptr [rip + 0x1881641] # 0x181bacbf8
000000018032B5B7 cmp       dword ptr [rcx + 0xe0], 0
000000018032B5BE jne       0x18032b5c5
000000018032B5C0 call      0x1802501e0
000000018032B5C5 cmp       r12d, edi
000000018032B5C8 jge       0x18032b8e2
000000018032B5CE mov       rcx, qword ptr [rip + 0x1881623] # 0x181bacbf8
000000018032B5D5 cmp       dword ptr [rcx + 0xe0], 0
000000018032B5DC jne       0x18032b5e3
000000018032B5DE call      0x1802501e0
000000018032B5E3 mov       r8d, dword ptr [rsp + 0x34]
000000018032B5E8 movd      xmm1, r12d
000000018032B5ED cvtdq2ps  xmm1, xmm1
000000018032B5F0 movd      xmm0, r8d
000000018032B5F5 mulss     xmm1, xmm7
000000018032B5F9 cvtdq2ps  xmm0, xmm0
000000018032B5FC andps     xmm1, xmm8
000000018032B600 mulss     xmm0, xmm6
000000018032B604 addss     xmm1, xmm0
000000018032B608 lea       eax, [r13*2 + 1]
000000018032B610 movd      xmm0, eax
000000018032B614 lea       ebx, [r13 + 1]
000000018032B618 cvtdq2ps  xmm0, xmm0
000000018032B61B mov       eax, r12d
000000018032B61E cdq       
000000018032B61F comiss    xmm1, xmm0
000000018032B622 cmovb     ebx, r13d
000000018032B626 sub       eax, edx
000000018032B628 sar       eax, 1
000000018032B62A mov       edi, eax
000000018032B62C cmp       eax, esi
000000018032B62E je        0x18032b63f
000000018032B630 mov       esi, eax
000000018032B632 mov       r13d, ebx
000000018032B635 mov       qword ptr [rsp + 0x40], rsi
000000018032B63A jmp       0x18032b6f8
000000018032B63F lea       eax, [r13*2 + 1]
000000018032B647 movd      xmm0, eax
000000018032B64B cvtdq2ps  xmm0, xmm0
000000018032B64E comiss    xmm0, xmm1
000000018032B651 jb        0x18032b6ed
000000018032B657 lea       edi, [rsi + 1]
000000018032B65A mov       esi, r13d
000000018032B65D imul      edi, dword ptr [rsp + 0x30]
000000018032B662 imul      esi, r8d
000000018032B666 add       edi, r14d
000000018032B669 add       esi, dword ptr [rsp + 0xc0]
000000018032B670 cmp       byte ptr [rip + 0x19b9603], 0 # 0x181ce4c7a
000000018032B677 jne       0x18032b698
000000018032B679 lea       rcx, [rip + 0x1887748] # 0x181bb2dc8
000000018032B680 call      0x18024fef0
000000018032B685 lea       rcx, [rip + 0x1887794] # 0x181bb2e20
000000018032B68C call      0x18024fef0
000000018032B691 mov       byte ptr [rip + 0x19b95e2], 1 # 0x181ce4c7a
000000018032B698 cmp       edi, r15d
000000018032B69B jge       0x18032b8e9
000000018032B6A1 imul      esi, r15d
000000018032B6A5 add       esi, edi
000000018032B6A7 test      rbp, rbp
000000018032B6AA je        0x18032b8e9
000000018032B6B0 cmp       dword ptr [rbp + 0x18], esi
000000018032B6B3 jle       0x18032b8e9
000000018032B6B9 mov       r8, qword ptr [rip + 0x1887760] # 0x181bb2e20
000000018032B6C0 mov       edx, esi
000000018032B6C2 mov       rcx, rbp
000000018032B6C5 call      0x180825710
000000018032B6CA test      rax, rax
000000018032B6CD je        0x18032b8e9
000000018032B6D3 mov       ecx, dword ptr [rsp + 0xe0]
000000018032B6DA cmp       dword ptr [rax + 0x18], ecx
000000018032B6DD jg        0x18032b3e4
000000018032B6E3 mov       rsi, qword ptr [rsp + 0x40]
000000018032B6E8 mov       r8d, dword ptr [rsp + 0x34]
000000018032B6ED cmp       ebx, r13d
000000018032B6F0 je        0x18032b77e
000000018032B6F6 mov       edi, esi
000000018032B6F8 imul      edi, dword ptr [rsp + 0x30]
000000018032B6FD imul      ebx, r8d
000000018032B701 add       edi, r14d
000000018032B704 add       ebx, dword ptr [rsp + 0xc0]
000000018032B70B cmp       byte ptr [rip + 0x19b9568], 0 # 0x181ce4c7a
000000018032B712 jne       0x18032b733
000000018032B714 lea       rcx, [rip + 0x18876ad] # 0x181bb2dc8
000000018032B71B call      0x18024fef0
000000018032B720 lea       rcx, [rip + 0x18876f9] # 0x181bb2e20
000000018032B727 call      0x18024fef0
000000018032B72C mov       byte ptr [rip + 0x19b9547], 1 # 0x181ce4c7a
000000018032B733 cmp       edi, r15d
000000018032B736 jge       0x18032b8e9
000000018032B73C imul      ebx, r15d
000000018032B740 add       ebx, edi
000000018032B742 test      rbp, rbp
000000018032B745 je        0x18032b8e9
000000018032B74B cmp       dword ptr [rbp + 0x18], ebx
000000018032B74E jle       0x18032b8e9
000000018032B754 mov       r8, qword ptr [rip + 0x18876c5] # 0x181bb2e20
000000018032B75B mov       edx, ebx
000000018032B75D mov       rcx, rbp
000000018032B760 call      0x180825710
000000018032B765 test      rax, rax
000000018032B768 je        0x18032b8e9
000000018032B76E mov       ecx, dword ptr [rsp + 0xe0]
000000018032B775 cmp       dword ptr [rax + 0x18], ecx
000000018032B778 jg        0x18032b3e4
000000018032B77E mov       edi, dword ptr [rsp + 0x38]
000000018032B782 inc       r12d
000000018032B785 jmp       0x18032b5b0
000000018032B78A mov       edi, dword ptr [rsp + 0xd8]
000000018032B791 lea       r15d, [rax + r12]
000000018032B795 mov       ebp, dword ptr [rsp + 0xe0]
000000018032B79C xor       esi, esi
000000018032B79E mov       r13d, ebx
000000018032B7A1 neg       r13d
000000018032B7A4 cmovs     r13d, ebx
000000018032B7A8 mov       rbx, qword ptr [rsp + 0xd0]
000000018032B7B0 imul      r12d, edi
000000018032B7B4 imul      eax, edi
000000018032B7B7 mov       dword ptr [rsp + 0xc0], eax
000000018032B7BE nop       
000000018032B7C0 mov       rcx, qword ptr [rip + 0x1881431] # 0x181bacbf8
000000018032B7C7 cmp       dword ptr [rcx + 0xe0], 0
000000018032B7CE jne       0x18032b7d5
000000018032B7D0 call      0x1802501e0
000000018032B7D5 cmp       esi, r13d
000000018032B7D8 jge       0x18032b8e2
000000018032B7DE cmp       byte ptr [rip + 0x19b9495], 0 # 0x181ce4c7a
000000018032B7E5 jne       0x18032b806
000000018032B7E7 lea       rcx, [rip + 0x18875da] # 0x181bb2dc8
000000018032B7EE call      0x18024fef0
000000018032B7F3 lea       rcx, [rip + 0x1887626] # 0x181bb2e20
000000018032B7FA call      0x18024fef0
000000018032B7FF mov       byte ptr [rip + 0x19b9474], 1 # 0x181ce4c7a
000000018032B806 cmp       r14d, edi
000000018032B809 jge       0x18032b8e9
000000018032B80F lea       edx, [r12 + r14]
000000018032B813 test      rbx, rbx
000000018032B816 je        0x18032b8e9
000000018032B81C cmp       dword ptr [rbx + 0x18], edx
000000018032B81F jle       0x18032b8e9
000000018032B825 mov       r8, qword ptr [rip + 0x18875f4] # 0x181bb2e20
000000018032B82C mov       rcx, rbx
000000018032B82F call      0x180825710
000000018032B834 test      rax, rax
000000018032B837 je        0x18032b8e9
000000018032B83D cmp       dword ptr [rax + 0x18], ebp
000000018032B840 jg        0x18032b3e4
000000018032B846 cmp       byte ptr [rip + 0x19b942d], 0 # 0x181ce4c7a
000000018032B84D jne       0x18032b86e
000000018032B84F lea       rcx, [rip + 0x1887572] # 0x181bb2dc8
000000018032B856 call      0x18024fef0
000000018032B85B lea       rcx, [rip + 0x18875be] # 0x181bb2e20
000000018032B862 call      0x18024fef0
000000018032B867 mov       byte ptr [rip + 0x19b940c], 1 # 0x181ce4c7a
000000018032B86E mov       ecx, dword ptr [rsp + 0x30]
000000018032B872 lea       eax, [r14 + rcx]
000000018032B876 cmp       eax, edi
000000018032B878 jge       0x18032b8e9
000000018032B87A lea       edx, [r12 + r14]
000000018032B87E add       edx, ecx
000000018032B880 cmp       dword ptr [rbx + 0x18], edx
000000018032B883 jle       0x18032b8e9
000000018032B885 mov       r8, qword ptr [rip + 0x1887594] # 0x181bb2e20
000000018032B88C mov       rcx, rbx
000000018032B88F call      0x180825710
000000018032B894 test      rax, rax
000000018032B897 je        0x18032b8e9
000000018032B899 cmp       dword ptr [rax + 0x18], ebp
000000018032B89C jg        0x18032b3e4
000000018032B8A2 mov       r9d, r15d
000000018032B8A5 mov       qword ptr [rsp + 0x20], 0
000000018032B8AE mov       r8d, r14d
000000018032B8B1 mov       edx, edi
000000018032B8B3 mov       rcx, rbx
000000018032B8B6 call      0x18032d950
000000018032B8BB test      rax, rax
000000018032B8BE je        0x18032b8e9
000000018032B8C0 cmp       dword ptr [rax + 0x18], ebp
000000018032B8C3 jg        0x18032b3e4
000000018032B8C9 add       r14d, dword ptr [rsp + 0x30]
000000018032B8CE inc       esi
000000018032B8D0 add       r12d, dword ptr [rsp + 0xc0]
000000018032B8D8 add       r15d, dword ptr [rsp + 0x34]
000000018032B8DD jmp       0x18032b7c0
000000018032B8E2 mov       al, 1
000000018032B8E4 jmp       0x18032b3e6
000000018032B8E9 call      0x180250150
000000018032B8EE int3      
000000018032B8EF int3      