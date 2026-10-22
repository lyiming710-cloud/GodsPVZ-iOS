0000000180372280 push      rbx
0000000180372282 sub       rsp, 0xb0
0000000180372289 cmp       byte ptr [rip + 0x1972b20], 0 # 0x181ce4db0
0000000180372290 mov       rbx, rcx
0000000180372293 jne       0x1803722f0
0000000180372295 lea       rcx, [rip + 0x182fd7c] # 0x181ba2018
000000018037229C call      0x18024fef0
00000001803722A1 lea       rcx, [rip + 0x1831fd0] # 0x181ba4278
00000001803722A8 call      0x18024fef0
00000001803722AD lea       rcx, [rip + 0x183e24c] # 0x181bb0500
00000001803722B4 call      0x18024fef0
00000001803722B9 lea       rcx, [rip + 0x1849718] # 0x181bbb9d8
00000001803722C0 call      0x18024fef0
00000001803722C5 lea       rcx, [rip + 0x18441ac] # 0x181bb6478
00000001803722CC call      0x18024fef0
00000001803722D1 lea       rcx, [rip + 0x18592c0] # 0x181bcb598
00000001803722D8 call      0x18024fef0
00000001803722DD lea       rcx, [rip + 0x183a6a4] # 0x181bac988
00000001803722E4 call      0x18024fef0
00000001803722E9 mov       byte ptr [rip + 0x1972ac0], 1 # 0x181ce4db0
00000001803722F0 mov       rax, qword ptr [rbx + 0x20]
00000001803722F4 mov       qword ptr [rsp + 0xc0], rbp
00000001803722FC mov       qword ptr [rsp + 0xc8], rsi
0000000180372304 mov       qword ptr [rsp + 0xd0], rdi
000000018037230C mov       qword ptr [rsp + 0xd8], r14
0000000180372314 movaps    xmmword ptr [rsp + 0xa0], xmm6
000000018037231C movaps    xmmword ptr [rsp + 0x90], xmm7
0000000180372324 test      rax, rax
0000000180372327 je        0x18037270e
000000018037232D mov       rcx, qword ptr [rax + 0xf8]
0000000180372334 test      rcx, rcx
0000000180372337 je        0x18037270e
000000018037233D xor       r8d, r8d
0000000180372340 mov       edx, 0x1a
0000000180372345 call      0x180324150
000000018037234A mov       rcx, qword ptr [rip + 0x182fcc7] # 0x181ba2018
0000000180372351 mov       rdi, rax
0000000180372354 mov       rsi, qword ptr [rbx + 0x1b0]
000000018037235B cmp       dword ptr [rcx + 0xe0], 0
0000000180372362 jne       0x180372369
0000000180372364 call      0x1802501e0
0000000180372369 mov       r8, qword ptr [rip + 0x1859228] # 0x181bcb598
0000000180372370 lea       rcx, [rsp + 0x80]
0000000180372378 xor       r9d, r9d
000000018037237B mov       rdx, rsi
000000018037237E call      0x18031b680
0000000180372383 xor       r14d, r14d
0000000180372386 xor       r9d, r9d
0000000180372389 mov       r8, rdi
000000018037238C mov       qword ptr [rsp + 0x20], r14
0000000180372391 mov       edx, 0x1a
0000000180372396 mov       rcx, rbx
0000000180372399 movsd     xmm7, qword ptr [rax]
000000018037239D mov       esi, dword ptr [rax + 8]
00000001803723A0 call      0x180360d00
00000001803723A5 cmp       byte ptr [rip + 0x197278a], r14b # 0x181ce4b36
00000001803723AC mov       rbp, rax
00000001803723AF xorps     xmm6, xmm6
00000001803723B2 jne       0x1803723c7
00000001803723B4 lea       rcx, [rip + 0x185042d] # 0x181bc27e8
00000001803723BB call      0x18024fef0
00000001803723C0 mov       byte ptr [rip + 0x197276f], 1 # 0x181ce4b36
00000001803723C7 mov       rcx, qword ptr [rip + 0x185041a] # 0x181bc27e8
00000001803723CE mov       rax, qword ptr [rcx + 0xb8]
00000001803723D5 test      rdi, rdi
00000001803723D8 je        0x18037270e
00000001803723DE movsd     xmm0, qword ptr [rax]
00000001803723E2 lea       r9, [rsp + 0x60]
00000001803723E7 mov       eax, dword ptr [rax + 8]
00000001803723EA lea       r8, [rsp + 0x70]
00000001803723EF movss     xmm1, dword ptr [rip + 0x1235959] # 0x1815a7d50
00000001803723F7 lea       rdx, [rsp + 0x80]
00000001803723FF movsd     qword ptr [rsp + 0x60], xmm0
0000000180372405 mov       rcx, rdi
0000000180372408 movss     xmm0, dword ptr [rip + 0x1235960] # 0x1815a7d70
0000000180372410 unpcklps  xmm0, xmm6
0000000180372413 movsd     qword ptr [rsp + 0x70], xmm0
0000000180372419 movss     xmm0, dword ptr [rip + 0x12358f7] # 0x1815a7d18
0000000180372421 mov       dword ptr [rsp + 0x68], eax
0000000180372425 mov       rax, qword ptr [rip + 0x18495ac] # 0x181bbb9d8
000000018037242C mov       qword ptr [rsp + 0x58], rax
0000000180372431 mov       qword ptr [rsp + 0x50], rbx
0000000180372436 movss     dword ptr [rsp + 0x48], xmm6
000000018037243C movss     dword ptr [rsp + 0x40], xmm6
0000000180372442 movss     dword ptr [rsp + 0x38], xmm0
0000000180372448 movss     xmm0, dword ptr [rip + 0x12358f0] # 0x1815a7d40
0000000180372450 mov       dword ptr [rsp + 0x30], 5
0000000180372458 movss     dword ptr [rsp + 0x28], xmm1
000000018037245E movss     dword ptr [rsp + 0x20], xmm0
0000000180372464 movss     dword ptr [rsp + 0x78], xmm6
000000018037246A movsd     qword ptr [rsp + 0x80], xmm7
0000000180372473 mov       dword ptr [rsp + 0x88], esi
000000018037247A call      0x1804a2700
000000018037247F mov       rcx, qword ptr [rip + 0x183e07a] # 0x181bb0500
0000000180372486 mov       rsi, qword ptr [rbx + 0x170]
000000018037248D cmp       dword ptr [rcx + 0xe0], r14d
0000000180372494 jne       0x18037249b
0000000180372496 call      0x1802501e0
000000018037249B xor       r8d, r8d
000000018037249E xor       edx, edx
00000001803724A0 mov       rcx, rsi
00000001803724A3 call      0x18131f900
00000001803724A8 test      al, al
00000001803724AA je        0x1803724e5
00000001803724AC mov       rax, qword ptr [rbx + 0x170]
00000001803724B3 test      rax, rax
00000001803724B6 je        0x18037270e
00000001803724BC mov       eax, dword ptr [rax + 0x30]
00000001803724BF mov       dword ptr [rdi + 0xb8], eax
00000001803724C5 mov       rax, qword ptr [rbx + 0x170]
00000001803724CC test      rax, rax
00000001803724CF je        0x18037270e
00000001803724D5 mov       eax, dword ptr [rax + 0x34]
00000001803724D8 mov       dword ptr [rdi + 0xbc], eax
00000001803724DE mov       dword ptr [rdi + 0xc0], r14d
00000001803724E5 mov       rcx, qword ptr [rip + 0x183e014] # 0x181bb0500
00000001803724EC mov       rsi, qword ptr [rbx + 0x180]
00000001803724F3 cmp       dword ptr [rcx + 0xe0], r14d
00000001803724FA jne       0x180372501
00000001803724FC call      0x1802501e0
0000000180372501 xor       r8d, r8d
0000000180372504 xor       edx, edx
0000000180372506 mov       rcx, rsi
0000000180372509 call      0x18131f900
000000018037250E test      al, al
0000000180372510 je        0x18037254b
0000000180372512 mov       rax, qword ptr [rbx + 0x180]
0000000180372519 test      rax, rax
000000018037251C je        0x18037270e
0000000180372522 mov       eax, dword ptr [rax + 0x38]
0000000180372525 mov       dword ptr [rdi + 0xb8], eax
000000018037252B mov       rax, qword ptr [rbx + 0x180]
0000000180372532 test      rax, rax
0000000180372535 je        0x18037270e
000000018037253B mov       eax, dword ptr [rax + 0x3c]
000000018037253E mov       dword ptr [rdi + 0xbc], eax
0000000180372544 mov       dword ptr [rdi + 0xc0], r14d
000000018037254B mov       rcx, qword ptr [rip + 0x183dfae] # 0x181bb0500
0000000180372552 mov       rsi, qword ptr [rbx + 0x178]
0000000180372559 cmp       dword ptr [rcx + 0xe0], r14d
0000000180372560 jne       0x180372567
0000000180372562 call      0x1802501e0
0000000180372567 xor       r8d, r8d
000000018037256A xor       edx, edx
000000018037256C mov       rcx, rsi
000000018037256F call      0x18131f900
0000000180372574 test      al, al
0000000180372576 je        0x1803725b1
0000000180372578 mov       rax, qword ptr [rbx + 0x178]
000000018037257F test      rax, rax
0000000180372582 je        0x18037270e
0000000180372588 mov       eax, dword ptr [rax + 0x30]
000000018037258B mov       dword ptr [rdi + 0xb8], eax
0000000180372591 mov       rax, qword ptr [rbx + 0x178]
0000000180372598 test      rax, rax
000000018037259B je        0x18037270e
00000001803725A1 mov       eax, dword ptr [rax + 0x34]
00000001803725A4 mov       dword ptr [rdi + 0xbc], eax
00000001803725AA mov       dword ptr [rdi + 0xc0], r14d
00000001803725B1 mov       r8d, dword ptr [rbx + 0xfc]
00000001803725B8 xor       r9d, r9d
00000001803725BB mov       rdx, rbp
00000001803725BE mov       rcx, rdi
00000001803725C1 call      0x18037c0f0
00000001803725C6 mov       eax, dword ptr [rbx + 0xd4]
00000001803725CC dec       eax
00000001803725CE mov       byte ptr [rbx + 0xd1], r14b
00000001803725D5 mov       dword ptr [rbx + 0xd4], eax
00000001803725DB test      eax, eax
00000001803725DD jg        0x180372608
00000001803725DF mov       rcx, qword ptr [rbx + 0x158]
00000001803725E6 test      rcx, rcx
00000001803725E9 je        0x18037270e
00000001803725EF mov       rdx, qword ptr [rip + 0x183a392] # 0x181bac988
00000001803725F6 xor       r9d, r9d
00000001803725F9 xor       r8d, r8d
00000001803725FC call      0x1812dba00
0000000180372601 mov       byte ptr [rbx + 0xd0], r14b
0000000180372608 mov       rax, qword ptr [rip + 0x1843e69] # 0x181bb6478
000000018037260F cmp       dword ptr [rax + 0xe0], r14d
0000000180372616 jne       0x180372627
0000000180372618 mov       rcx, rax
000000018037261B call      0x1802501e0
0000000180372620 mov       rax, qword ptr [rip + 0x1843e51] # 0x181bb6478
0000000180372627 mov       rax, qword ptr [rax + 0xb8]
000000018037262E mov       rcx, qword ptr [rax + 0x120]
0000000180372635 test      rcx, rcx
0000000180372638 je        0x18037270e
000000018037263E mov       r8, qword ptr [rip + 0x1831c33] # 0x181ba4278
0000000180372645 mov       edx, 8
000000018037264A call      0x180825710
000000018037264F xor       ecx, ecx
0000000180372651 mov       rbx, rax
0000000180372654 call      0x1812e27c0
0000000180372659 test      rax, rax
000000018037265C je        0x18037270e
0000000180372662 xor       edx, edx
0000000180372664 mov       rcx, rax
0000000180372667 call      0x181304510
000000018037266C test      rax, rax
000000018037266F je        0x18037270e
0000000180372675 xor       r8d, r8d
0000000180372678 lea       rcx, [rsp + 0x80]
0000000180372680 mov       rdx, rax
0000000180372683 call      0x18132f3a0
0000000180372688 mov       rcx, qword ptr [rip + 0x182f989] # 0x181ba2018
000000018037268F movsd     xmm6, qword ptr [rax]
0000000180372693 mov       edi, dword ptr [rax + 8]
0000000180372696 cmp       dword ptr [rcx + 0xe0], r14d
000000018037269D jne       0x1803726a4
000000018037269F call      0x1802501e0
00000001803726A4 xor       ecx, ecx
00000001803726A6 movsd     qword ptr [rsp + 0x80], xmm6
00000001803726AF mov       dword ptr [rsp + 0x88], edi
00000001803726B6 call      0x18031aea0
00000001803726BB addss     xmm0, xmm0
00000001803726BF xor       r9d, r9d
00000001803726C2 lea       rdx, [rsp + 0x80]
00000001803726CA mov       rcx, rbx
00000001803726CD movaps    xmm2, xmm0
00000001803726D0 call      0x18031b230
00000001803726D5 movaps    xmm7, xmmword ptr [rsp + 0x90]
00000001803726DD movaps    xmm6, xmmword ptr [rsp + 0xa0]
00000001803726E5 mov       r14, qword ptr [rsp + 0xd8]
00000001803726ED mov       rdi, qword ptr [rsp + 0xd0]
00000001803726F5 mov       rsi, qword ptr [rsp + 0xc8]
00000001803726FD mov       rbp, qword ptr [rsp + 0xc0]
0000000180372705 add       rsp, 0xb0
000000018037270C pop       rbx
000000018037270D ret       
000000018037270E call      0x180250150
0000000180372713 int3      
0000000180372714 int3      
0000000180372715 int3      
0000000180372716 int3      
0000000180372717 int3      
0000000180372718 int3      
0000000180372719 int3      
000000018037271A int3      
000000018037271B int3      
000000018037271C int3      
000000018037271D int3      
000000018037271E int3      
000000018037271F int3      