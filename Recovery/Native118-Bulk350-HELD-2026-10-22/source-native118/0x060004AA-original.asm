00000001803732F0 mov       rax, rsp
00000001803732F3 mov       qword ptr [rax + 0x20], rbx
00000001803732F7 mov       dword ptr [rax + 0x18], r8d
00000001803732FB mov       dword ptr [rax + 0x10], edx
00000001803732FE mov       qword ptr [rax + 8], rcx
0000000180373302 push      rsi
0000000180373303 push      rdi
0000000180373304 push      r12
0000000180373306 push      r14
0000000180373308 push      r15
000000018037330A sub       rsp, 0xc0
0000000180373311 movaps    xmmword ptr [rax - 0x38], xmm6
0000000180373315 movaps    xmmword ptr [rax - 0x48], xmm7
0000000180373319 movaps    xmmword ptr [rax - 0x58], xmm8
000000018037331E movaps    xmmword ptr [rax - 0x68], xmm9
0000000180373323 mov       esi, r8d
0000000180373326 mov       r14d, edx
0000000180373329 mov       rdi, rcx
000000018037332C cmp       byte ptr [rip + 0x1971a7a], 0 # 0x181ce4dad
0000000180373333 jne       0x18037339c
0000000180373335 lea       rcx, [rip + 0x184fbfc] # 0x181bc2f38
000000018037333C call      0x18024fef0
0000000180373341 lea       rcx, [rip + 0x184fc48] # 0x181bc2f90
0000000180373348 call      0x18024fef0
000000018037334D lea       rcx, [rip + 0x184fc94] # 0x181bc2fe8
0000000180373354 call      0x18024fef0
0000000180373359 lea       rcx, [rip + 0x182ecb8] # 0x181ba2018
0000000180373360 call      0x18024fef0
0000000180373365 lea       rcx, [rip + 0x1837144] # 0x181baa4b0
000000018037336C call      0x18024fef0
0000000180373371 lea       rcx, [rip + 0x1830f00] # 0x181ba4278
0000000180373378 call      0x18024fef0
000000018037337D lea       rcx, [rip + 0x183d17c] # 0x181bb0500
0000000180373384 call      0x18024fef0
0000000180373389 lea       rcx, [rip + 0x18430e8] # 0x181bb6478
0000000180373390 call      0x18024fef0
0000000180373395 mov       byte ptr [rip + 0x1971a11], 1 # 0x181ce4dad
000000018037339C xorps     xmm0, xmm0
000000018037339F xor       eax, eax
00000001803733A1 movups    xmmword ptr [rsp + 0x60], xmm0
00000001803733A6 mov       qword ptr [rsp + 0x70], rax
00000001803733AB cmp       qword ptr [rdi + 0x128], rax
00000001803733B2 je        0x18037347d
00000001803733B8 mov       r8, qword ptr [rip + 0x18370f1] # 0x181baa4b0
00000001803733BF mov       rdx, qword ptr [rdi + 0x128]
00000001803733C6 lea       rcx, [rsp + 0x40]
00000001803733CB call      0x1808197f0
00000001803733D0 movups    xmm0, xmmword ptr [rsp + 0x40]
00000001803733D5 movups    xmmword ptr [rsp + 0x60], xmm0
00000001803733DA movsd     xmm1, qword ptr [rsp + 0x50]
00000001803733E0 movsd     qword ptr [rsp + 0x70], xmm1
00000001803733E6 xor       r15d, r15d
00000001803733E9 mov       qword ptr [rsp + 0x40], r15
00000001803733EE lea       rbx, [rsp + 0x60]
00000001803733F3 mov       qword ptr [rsp + 0x48], rbx
00000001803733F8 nop       dword ptr [rax + rax]
0000000180373400 mov       rdx, qword ptr [rip + 0x184fb89] # 0x181bc2f90
0000000180373407 lea       rcx, [rsp + 0x60]
000000018037340C call      0x180669280
0000000180373411 test      al, al
0000000180373413 je        0x180373436
0000000180373415 mov       rax, qword ptr [rsp + 0x70]
000000018037341A test      rax, rax
000000018037341D je        0x180373400
000000018037341F cmp       byte ptr [rax + 0x28], 0
0000000180373423 jne       0x180373400
0000000180373425 cmp       r14d, dword ptr [rax + 0x10]
0000000180373429 jne       0x180373400
000000018037342B cmp       esi, dword ptr [rax + 0x14]
000000018037342E jne       0x180373400
0000000180373430 mov       byte ptr [rax + 0x28], 1
0000000180373434 jmp       0x180373400
0000000180373436 mov       rdx, qword ptr [rip + 0x184fafb] # 0x181bc2f38
000000018037343D mov       rcx, rbx
0000000180373440 call      0x180302170
0000000180373445 jmp       0x180373480
0000000180373447 mov       rdx, qword ptr [rip + 0x184faea] # 0x181bc2f38
000000018037344E mov       rcx, qword ptr [rsp + 0x48]
0000000180373453 call      0x180302170
0000000180373458 mov       rcx, qword ptr [rsp + 0x40]
000000018037345D test      rcx, rcx
0000000180373460 jne       0x180373a92
0000000180373466 mov       esi, dword ptr [rsp + 0x100]
000000018037346D mov       r14d, dword ptr [rsp + 0xf8]
0000000180373475 mov       rdi, qword ptr [rsp + 0xf0]
000000018037347D xor       r15d, r15d
0000000180373480 mov       rcx, qword ptr [rdi + 0x20]
0000000180373484 test      rcx, rcx
0000000180373487 je        0x180373a8c
000000018037348D xor       r9d, r9d
0000000180373490 mov       r8d, esi
0000000180373493 mov       edx, r14d
0000000180373496 call      0x180326fe0
000000018037349B mov       rbx, rax
000000018037349E test      rax, rax
00000001803734A1 je        0x180373a02
00000001803734A7 cmp       byte ptr [rax + 0x14], 0
00000001803734AB je        0x1803734c1
00000001803734AD mov       rcx, qword ptr [rdi + 0x20]
00000001803734B1 test      rcx, rcx
00000001803734B4 je        0x180373a8c
00000001803734BA xor       edx, edx
00000001803734BC call      0x180326910
00000001803734C1 mov       qword ptr [rsp + 0x20], r15
00000001803734C6 xor       r9d, r9d
00000001803734C9 xor       r8d, r8d
00000001803734CC mov       edx, 0xffffffff
00000001803734D1 mov       rcx, rdi
00000001803734D4 call      0x180360d00
00000001803734D9 mov       r12, rax
00000001803734DC mov       rsi, qword ptr [rbx + 0x60]
00000001803734E0 mov       rcx, qword ptr [rip + 0x183d019] # 0x181bb0500
00000001803734E7 cmp       dword ptr [rcx + 0xe0], 0
00000001803734EE jne       0x1803734f5
00000001803734F0 call      0x1802501e0
00000001803734F5 xor       edx, edx
00000001803734F7 mov       rcx, rsi
00000001803734FA call      0x18131f870
00000001803734FF mov       rcx, qword ptr [rip + 0x183cffa] # 0x181bb0500
0000000180373506 test      al, al
0000000180373508 jne       0x1803735a9
000000018037350E mov       rsi, qword ptr [rbx + 0x50]
0000000180373512 cmp       dword ptr [rcx + 0xe0], 0
0000000180373519 jne       0x180373520
000000018037351B call      0x1802501e0
0000000180373520 xor       edx, edx
0000000180373522 mov       rcx, rsi
0000000180373525 call      0x18131f870
000000018037352A test      al, al
000000018037352C je        0x180373590
000000018037352E mov       rsi, qword ptr [rbx + 0x50]
0000000180373532 mov       r14, qword ptr [rdi + 0x180]
0000000180373539 mov       rcx, qword ptr [rip + 0x183cfc0] # 0x181bb0500
0000000180373540 cmp       dword ptr [rcx + 0xe0], 0
0000000180373547 jne       0x18037354e
0000000180373549 call      0x1802501e0
000000018037354E xor       r8d, r8d
0000000180373551 mov       rdx, r14
0000000180373554 mov       rcx, rsi
0000000180373557 call      0x18131f760
000000018037355C test      al, al
000000018037355E je        0x18037358a
0000000180373560 mov       qword ptr [rdi + 0x180], r15
0000000180373567 lea       rcx, [rdi + 0x180]
000000018037356E xor       edx, edx
0000000180373570 call      0x18024f360
0000000180373575 mov       qword ptr [rdi + 0x128], r15
000000018037357C lea       rcx, [rdi + 0x128]
0000000180373583 xor       edx, edx
0000000180373585 call      0x18024f360
000000018037358A mov       rcx, qword ptr [rbx + 0x50]
000000018037358E jmp       0x180373602
0000000180373590 movss     xmm7, dword ptr [rip + 0x123467c] # 0x1815a7c14
0000000180373598 xorps     xmm8, xmm8
000000018037359C movss     xmm6, dword ptr [rip + 0x1234658] # 0x1815a7bfc
00000001803735A4 jmp       0x1803737b5
00000001803735A9 mov       rsi, qword ptr [rbx + 0x60]
00000001803735AD mov       r14, qword ptr [rdi + 0x180]
00000001803735B4 cmp       dword ptr [rcx + 0xe0], 0
00000001803735BB jne       0x1803735c2
00000001803735BD call      0x1802501e0
00000001803735C2 xor       r8d, r8d
00000001803735C5 mov       rdx, r14
00000001803735C8 mov       rcx, rsi
00000001803735CB call      0x18131f760
00000001803735D0 test      al, al
00000001803735D2 je        0x1803735fe
00000001803735D4 mov       qword ptr [rdi + 0x180], r15
00000001803735DB lea       rcx, [rdi + 0x180]
00000001803735E2 xor       edx, edx
00000001803735E4 call      0x18024f360
00000001803735E9 mov       qword ptr [rdi + 0x128], r15
00000001803735F0 lea       rcx, [rdi + 0x128]
00000001803735F7 xor       edx, edx
00000001803735F9 call      0x18024f360
00000001803735FE mov       rcx, qword ptr [rbx + 0x60]
0000000180373602 test      rcx, rcx
0000000180373605 je        0x180373a8c
000000018037360B xor       r9d, r9d
000000018037360E xor       r8d, r8d
0000000180373611 mov       rdx, r12
0000000180373614 call      0x18034b8e0
0000000180373619 mov       rax, qword ptr [rip + 0x1842e58] # 0x181bb6478
0000000180373620 cmp       dword ptr [rax + 0xe0], 0
0000000180373627 jne       0x180373638
0000000180373629 mov       rcx, rax
000000018037362C call      0x1802501e0
0000000180373631 mov       rax, qword ptr [rip + 0x1842e40] # 0x181bb6478
0000000180373638 mov       rax, qword ptr [rax + 0xb8]
000000018037363F mov       rcx, qword ptr [rax + 0x138]
0000000180373646 test      rcx, rcx
0000000180373649 je        0x180373a8c
000000018037364F mov       r8, qword ptr [rip + 0x1830c22] # 0x181ba4278
0000000180373656 mov       edx, 0xc
000000018037365B call      0x180825710
0000000180373660 mov       r14, rax
0000000180373663 xor       ecx, ecx
0000000180373665 call      0x1812e27c0
000000018037366A test      rax, rax
000000018037366D je        0x180373a8c
0000000180373673 xor       edx, edx
0000000180373675 mov       rcx, rax
0000000180373678 call      0x181304510
000000018037367D test      rax, rax
0000000180373680 je        0x180373a8c
0000000180373686 xor       r8d, r8d
0000000180373689 mov       rdx, rax
000000018037368C lea       rcx, [rsp + 0x40]
0000000180373691 call      0x18132f3a0
0000000180373696 mov       rcx, qword ptr [rip + 0x182e97b] # 0x181ba2018
000000018037369D cmp       dword ptr [rcx + 0xe0], 0
00000001803736A4 mov       esi, dword ptr [rax + 8]
00000001803736A7 movsd     xmm6, qword ptr [rax]
00000001803736AB jne       0x1803736b2
00000001803736AD call      0x1802501e0
00000001803736B2 xor       ecx, ecx
00000001803736B4 call      0x18031aea0
00000001803736B9 movsd     qword ptr [rsp + 0x40], xmm6
00000001803736BF mov       dword ptr [rsp + 0x48], esi
00000001803736C3 xor       r9d, r9d
00000001803736C6 movaps    xmm2, xmm0
00000001803736C9 lea       rdx, [rsp + 0x40]
00000001803736CE mov       rcx, r14
00000001803736D1 call      0x18031b230
00000001803736D6 cmp       byte ptr [rip + 0x19716cc], 0 # 0x181ce4da9
00000001803736DD jne       0x1803736fe
00000001803736DF lea       rcx, [rip + 0x183ce1a] # 0x181bb0500
00000001803736E6 call      0x18024fef0
00000001803736EB lea       rcx, [rip + 0x1854856] # 0x181bc7f48
00000001803736F2 call      0x18024fef0
00000001803736F7 mov       byte ptr [rip + 0x19716ab], 1 # 0x181ce4da9
00000001803736FE movss     xmm0, dword ptr [rdi + 0xc0]
0000000180373706 movss     xmm7, dword ptr [rip + 0x1234506] # 0x1815a7c14
000000018037370E subss     xmm0, xmm7
0000000180373712 movss     dword ptr [rdi + 0xc0], xmm0
000000018037371A xorps     xmm8, xmm8
000000018037371E comiss    xmm8, xmm0
0000000180373722 jb        0x18037376c
0000000180373724 mov       dword ptr [rdi + 0xc0], r15d
000000018037372B mov       byte ptr [rdi + 0xbf], 1
0000000180373732 mov       rcx, qword ptr [rdi + 0x158]
0000000180373739 test      rcx, rcx
000000018037373C je        0x180373a8c
0000000180373742 xor       r9d, r9d
0000000180373745 mov       r8b, 1
0000000180373748 mov       rdx, qword ptr [rip + 0x18547f9] # 0x181bc7f48
000000018037374F call      0x1812dba00
0000000180373754 xor       r8d, r8d
0000000180373757 movss     xmm6, dword ptr [rip + 0x123449d] # 0x1815a7bfc
000000018037375F movaps    xmm1, xmm6
0000000180373762 mov       rcx, rdi
0000000180373765 call      0x18036a5e0
000000018037376A jmp       0x180373774
000000018037376C movss     xmm6, dword ptr [rip + 0x1234488] # 0x1815a7bfc
0000000180373774 mov       rcx, qword ptr [rip + 0x183cd85] # 0x181bb0500
000000018037377B cmp       dword ptr [rcx + 0xe0], 0
0000000180373782 mov       rsi, qword ptr [rdi + 0x208]
0000000180373789 jne       0x180373790
000000018037378B call      0x1802501e0
0000000180373790 xor       edx, edx
0000000180373792 mov       rcx, rsi
0000000180373795 call      0x18131f870
000000018037379A test      al, al
000000018037379C je        0x1803737b5
000000018037379E mov       rcx, qword ptr [rdi + 0x208]
00000001803737A5 test      rcx, rcx
00000001803737A8 je        0x180373a8c
00000001803737AE xor       edx, edx
00000001803737B0 call      0x18039dba0
00000001803737B5 mov       rsi, qword ptr [rbx + 0x38]
00000001803737B9 mov       rcx, qword ptr [rip + 0x183cd40] # 0x181bb0500
00000001803737C0 cmp       dword ptr [rcx + 0xe0], 0
00000001803737C7 jne       0x1803737ce
00000001803737C9 call      0x1802501e0
00000001803737CE xor       edx, edx
00000001803737D0 mov       rcx, rsi
00000001803737D3 call      0x18131f870
00000001803737D8 mov       rcx, qword ptr [rip + 0x183cd21] # 0x181bb0500
00000001803737DF test      al, al
00000001803737E1 jne       0x180373a2e
00000001803737E7 mov       rsi, qword ptr [rbx + 0x30]
00000001803737EB cmp       dword ptr [rcx + 0xe0], 0
00000001803737F2 jne       0x1803737f9
00000001803737F4 call      0x1802501e0
00000001803737F9 xor       edx, edx
00000001803737FB mov       rcx, rsi
00000001803737FE call      0x18131f870
0000000180373803 test      al, al
0000000180373805 je        0x180373a02
000000018037380B mov       rsi, qword ptr [rbx + 0x30]
000000018037380F mov       r14, qword ptr [rdi + 0x170]
0000000180373816 mov       rcx, qword ptr [rip + 0x183cce3] # 0x181bb0500
000000018037381D cmp       dword ptr [rcx + 0xe0], 0
0000000180373824 jne       0x18037382b
0000000180373826 call      0x1802501e0
000000018037382B xor       r8d, r8d
000000018037382E mov       rdx, r14
0000000180373831 mov       rcx, rsi
0000000180373834 call      0x18131f760
0000000180373839 test      al, al
000000018037383B je        0x180373867
000000018037383D mov       qword ptr [rdi + 0x180], r15
0000000180373844 lea       rcx, [rdi + 0x180]
000000018037384B xor       edx, edx
000000018037384D call      0x18024f360
0000000180373852 mov       qword ptr [rdi + 0x128], r15
0000000180373859 lea       rcx, [rdi + 0x128]
0000000180373860 xor       edx, edx
0000000180373862 call      0x18024f360
0000000180373867 mov       rcx, qword ptr [rbx + 0x30]
000000018037386B test      rcx, rcx
000000018037386E je        0x180373a8c
0000000180373874 xor       r9d, r9d
0000000180373877 xor       r8d, r8d
000000018037387A mov       rdx, r12
000000018037387D call      0x18035a7a0
0000000180373882 mov       rax, qword ptr [rip + 0x1842bef] # 0x181bb6478
0000000180373889 cmp       dword ptr [rax + 0xe0], 0
0000000180373890 jne       0x1803738a1
0000000180373892 mov       rcx, rax
0000000180373895 call      0x1802501e0
000000018037389A mov       rax, qword ptr [rip + 0x1842bd7] # 0x181bb6478
00000001803738A1 mov       rax, qword ptr [rax + 0xb8]
00000001803738A8 mov       rcx, qword ptr [rax + 0x138]
00000001803738AF test      rcx, rcx
00000001803738B2 je        0x180373a8c
00000001803738B8 mov       r8, qword ptr [rip + 0x18309b9] # 0x181ba4278
00000001803738BF mov       edx, 0xc
00000001803738C4 call      0x180825710
00000001803738C9 mov       rsi, rax
00000001803738CC xor       ecx, ecx
00000001803738CE call      0x1812e27c0
00000001803738D3 test      rax, rax
00000001803738D6 je        0x180373a8c
00000001803738DC xor       edx, edx
00000001803738DE mov       rcx, rax
00000001803738E1 call      0x181304510
00000001803738E6 test      rax, rax
00000001803738E9 je        0x180373a8c
00000001803738EF xor       r8d, r8d
00000001803738F2 mov       rdx, rax
00000001803738F5 lea       rcx, [rsp + 0x40]
00000001803738FA call      0x18132f3a0
00000001803738FF mov       rcx, qword ptr [rip + 0x182e712] # 0x181ba2018
0000000180373906 cmp       dword ptr [rcx + 0xe0], 0
000000018037390D mov       ebx, dword ptr [rax + 8]
0000000180373910 movsd     xmm9, qword ptr [rax]
0000000180373915 jne       0x18037391c
0000000180373917 call      0x1802501e0
000000018037391C xor       ecx, ecx
000000018037391E call      0x18031aea0
0000000180373923 movsd     qword ptr [rsp + 0x40], xmm9
000000018037392A mov       dword ptr [rsp + 0x48], ebx
000000018037392E xor       r9d, r9d
0000000180373931 movaps    xmm2, xmm0
0000000180373934 lea       rdx, [rsp + 0x40]
0000000180373939 mov       rcx, rsi
000000018037393C call      0x18031b230
0000000180373941 cmp       byte ptr [rip + 0x1971461], 0 # 0x181ce4da9
0000000180373948 jne       0x180373969
000000018037394A lea       rcx, [rip + 0x183cbaf] # 0x181bb0500
0000000180373951 call      0x18024fef0
0000000180373956 lea       rcx, [rip + 0x18545eb] # 0x181bc7f48
000000018037395D call      0x18024fef0
0000000180373962 mov       byte ptr [rip + 0x1971440], 1 # 0x181ce4da9
0000000180373969 movss     xmm0, dword ptr [rdi + 0xc0]
0000000180373971 subss     xmm0, xmm7
0000000180373975 comiss    xmm8, xmm0
0000000180373979 movss     dword ptr [rdi + 0xc0], xmm0
0000000180373981 jb        0x1803739c1
0000000180373983 mov       rcx, qword ptr [rdi + 0x158]
000000018037398A test      rcx, rcx
000000018037398D mov       byte ptr [rdi + 0xbf], 1
0000000180373994 mov       dword ptr [rdi + 0xc0], r15d
000000018037399B je        0x180373a8c
00000001803739A1 xor       r9d, r9d
00000001803739A4 mov       r8b, 1
00000001803739A7 mov       rdx, qword ptr [rip + 0x185459a] # 0x181bc7f48
00000001803739AE call      0x1812dba00
00000001803739B3 xor       r8d, r8d
00000001803739B6 movaps    xmm1, xmm6
00000001803739B9 mov       rcx, rdi
00000001803739BC call      0x18036a5e0
00000001803739C1 mov       rcx, qword ptr [rip + 0x183cb38] # 0x181bb0500
00000001803739C8 cmp       dword ptr [rcx + 0xe0], 0
00000001803739CF mov       rbx, qword ptr [rdi + 0x208]
00000001803739D6 jne       0x1803739dd
00000001803739D8 call      0x1802501e0
00000001803739DD xor       edx, edx
00000001803739DF mov       rcx, rbx
00000001803739E2 call      0x18131f870
00000001803739E7 test      al, al
00000001803739E9 je        0x180373a02
00000001803739EB mov       rcx, qword ptr [rdi + 0x208]
00000001803739F2 test      rcx, rcx
00000001803739F5 je        0x180373a8c
00000001803739FB xor       edx, edx
00000001803739FD call      0x18039dba0
0000000180373A02 lea       r11, [rsp + 0xc0]
0000000180373A0A mov       rbx, qword ptr [r11 + 0x48]
0000000180373A0E movaps    xmm6, xmmword ptr [r11 - 0x10]
0000000180373A13 movaps    xmm7, xmmword ptr [r11 - 0x20]
0000000180373A18 movaps    xmm8, xmmword ptr [r11 - 0x30]
0000000180373A1D movaps    xmm9, xmmword ptr [r11 - 0x40]
0000000180373A22 mov       rsp, r11
0000000180373A25 pop       r15
0000000180373A27 pop       r14
0000000180373A29 pop       r12
0000000180373A2B pop       rdi
0000000180373A2C pop       rsi
0000000180373A2D ret       
0000000180373A2E mov       rsi, qword ptr [rbx + 0x38]
0000000180373A32 mov       r14, qword ptr [rdi + 0x170]
0000000180373A39 cmp       dword ptr [rcx + 0xe0], 0
0000000180373A40 jne       0x180373a47
0000000180373A42 call      0x1802501e0
0000000180373A47 xor       r8d, r8d
0000000180373A4A mov       rdx, r14
0000000180373A4D mov       rcx, rsi
0000000180373A50 call      0x18131f760
0000000180373A55 test      al, al
0000000180373A57 je        0x180373a83
0000000180373A59 mov       qword ptr [rdi + 0x180], r15
0000000180373A60 lea       rcx, [rdi + 0x180]
0000000180373A67 xor       edx, edx
0000000180373A69 call      0x18024f360
0000000180373A6E mov       qword ptr [rdi + 0x128], r15
0000000180373A75 lea       rcx, [rdi + 0x128]
0000000180373A7C xor       edx, edx
0000000180373A7E call      0x18024f360
0000000180373A83 mov       rcx, qword ptr [rbx + 0x38]
0000000180373A87 jmp       0x18037386b
0000000180373A8C call      0x180250150
0000000180373A91 int3      
0000000180373A92 call      0x180200e60
0000000180373A97 int3      
0000000180373A98 int3      
0000000180373A99 int3      
0000000180373A9A int3      
0000000180373A9B int3      
0000000180373A9C int3      
0000000180373A9D int3      
0000000180373A9E int3      
0000000180373A9F int3      