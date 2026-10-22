0000000180396F10 mov       rax, rsp
0000000180396F13 mov       qword ptr [rax + 8], rcx
0000000180396F17 push      rbx
0000000180396F18 push      rsi
0000000180396F19 push      rdi
0000000180396F1A push      r14
0000000180396F1C push      r15
0000000180396F1E sub       rsp, 0x130
0000000180396F25 movaps    xmmword ptr [rax - 0x38], xmm6
0000000180396F29 movaps    xmmword ptr [rax - 0x48], xmm7
0000000180396F2D movaps    xmmword ptr [rax - 0x58], xmm8
0000000180396F32 movaps    xmmword ptr [rax - 0x68], xmm9
0000000180396F37 movaps    xmmword ptr [rax - 0x78], xmm10
0000000180396F3C movaps    xmmword ptr [rax - 0x88], xmm11
0000000180396F44 movaps    xmmword ptr [rax - 0x98], xmm12
0000000180396F4C movaps    xmmword ptr [rax - 0xa8], xmm13
0000000180396F54 movaps    xmmword ptr [rax - 0xb8], xmm14
0000000180396F5C mov       r14, rcx
0000000180396F5F cmp       byte ptr [rip + 0x194df14], 0 # 0x181ce4e7a
0000000180396F66 jne       0x180397057
0000000180396F6C lea       rcx, [rip + 0x1803dd5] # 0x181b9ad48
0000000180396F73 call      0x18024fef0
0000000180396F78 lea       rcx, [rip + 0x182beb1] # 0x181bc2e30
0000000180396F7F call      0x18024fef0
0000000180396F84 lea       rcx, [rip + 0x182a5ed] # 0x181bc1578
0000000180396F8B call      0x18024fef0
0000000180396F90 lea       rcx, [rip + 0x182a639] # 0x181bc15d0
0000000180396F97 call      0x18024fef0
0000000180396F9C lea       rcx, [rip + 0x182bee5] # 0x181bc2e88
0000000180396FA3 call      0x18024fef0
0000000180396FA8 lea       rcx, [rip + 0x182bf31] # 0x181bc2ee0
0000000180396FAF call      0x18024fef0
0000000180396FB4 lea       rcx, [rip + 0x182a66d] # 0x181bc1628
0000000180396FBB call      0x18024fef0
0000000180396FC0 lea       rcx, [rip + 0x180dcd1] # 0x181ba4c98
0000000180396FC7 call      0x18024fef0
0000000180396FCC lea       rcx, [rip + 0x1813275] # 0x181baa248
0000000180396FD3 call      0x18024fef0
0000000180396FD8 lea       rcx, [rip + 0x180dd79] # 0x181ba4d58
0000000180396FDF call      0x18024fef0
0000000180396FE4 lea       rcx, [rip + 0x180dc4d] # 0x181ba4c38
0000000180396FEB call      0x18024fef0
0000000180396FF0 lea       rcx, [rip + 0x1813301] # 0x181baa2f8
0000000180396FF7 call      0x18024fef0
0000000180396FFC lea       rcx, [rip + 0x181334d] # 0x181baa350
0000000180397003 call      0x18024fef0
0000000180397008 lea       rcx, [rip + 0x18258f9] # 0x181bbc908
000000018039700F call      0x18024fef0
0000000180397014 lea       rcx, [rip + 0x181f45d] # 0x181bb6478
000000018039701B call      0x18024fef0
0000000180397020 lea       rcx, [rip + 0x1823cd1] # 0x181bbacf8
0000000180397027 call      0x18024fef0
000000018039702C lea       rcx, [rip + 0x180dd15] # 0x181ba4d48
0000000180397033 call      0x18024fef0
0000000180397038 lea       rcx, [rip + 0x1809691] # 0x181ba06d0
000000018039703F call      0x18024fef0
0000000180397044 lea       rcx, [rip + 0x180a3a5] # 0x181ba13f0
000000018039704B call      0x18024fef0
0000000180397050 mov       byte ptr [rip + 0x194de23], 1 # 0x181ce4e7a
0000000180397057 xorps     xmm0, xmm0
000000018039705A xor       eax, eax
000000018039705C movups    xmmword ptr [rsp + 0x58], xmm0
0000000180397061 mov       qword ptr [rsp + 0x68], rax
0000000180397066 xorps     xmm1, xmm1
0000000180397069 movups    xmmword ptr [rsp + 0x70], xmm1
000000018039706E mov       qword ptr [rsp + 0x80], rax
0000000180397076 mov       rcx, qword ptr [r14 + 0x28]
000000018039707A test      rcx, rcx
000000018039707D je        0x18039754d
0000000180397083 xor       edx, edx
0000000180397085 call      0x1803198a0
000000018039708A mov       rdi, rax
000000018039708D mov       rcx, qword ptr [rip + 0x181f3e4] # 0x181bb6478
0000000180397094 cmp       dword ptr [rcx + 0xe0], 0
000000018039709B jne       0x1803970a2
000000018039709D call      0x1802501e0
00000001803970A2 xor       ecx, ecx
00000001803970A4 call      0x18033ba50
00000001803970A9 xor       r15d, r15d
00000001803970AC mov       eax, r15d
00000001803970AF mov       dword ptr [rsp + 0x170], eax
00000001803970B6 mov       ecx, r15d
00000001803970B9 test      rdi, rdi
00000001803970BC je        0x18039754d
00000001803970C2 xorps     xmm14, xmm14
00000001803970C6 movss     xmm8, dword ptr [rip + 0x1210de5] # 0x1815a7eb4
00000001803970CF movss     xmm9, dword ptr [rip + 0x1210dd0] # 0x1815a7ea8
00000001803970D8 movss     xmm10, dword ptr [rip + 0x1210a4b] # 0x1815a7b2c
00000001803970E1 movss     xmm11, dword ptr [rip + 0x1210c4a] # 0x1815a7d34
00000001803970EA movss     xmm12, dword ptr [rip + 0x1210b7d] # 0x1815a7c70
00000001803970F3 movss     xmm13, dword ptr [rip + 0x1210dc4] # 0x1815a7ec0
00000001803970FC nop       dword ptr [rax]
0000000180397100 cmp       ecx, dword ptr [rdi + 0x18]
0000000180397103 jge       0x1803971f9
0000000180397109 mov       r8, qword ptr [rip + 0x1813240] # 0x181baa350
0000000180397110 mov       edx, eax
0000000180397112 mov       rcx, rdi
0000000180397115 call      0x180825710
000000018039711A mov       rbx, rax
000000018039711D xor       edx, edx
000000018039711F lea       rcx, [rsp + 0x170]
0000000180397127 call      0x180ca3820
000000018039712C xor       r9d, r9d
000000018039712F mov       r8, qword ptr [rip + 0x180959a] # 0x181ba06d0
0000000180397136 mov       rdx, rax
0000000180397139 mov       rcx, qword ptr [rip + 0x180dc08] # 0x181ba4d48
0000000180397140 call      0x180b73f50
0000000180397145 mov       rsi, rax
0000000180397148 mov       rcx, qword ptr [rip + 0x1803bf9] # 0x181b9ad48
000000018039714F cmp       dword ptr [rcx + 0xe0], r15d
0000000180397156 jne       0x18039715d
0000000180397158 call      0x1802501e0
000000018039715D xor       edx, edx
000000018039715F mov       rcx, rsi
0000000180397162 call      0x1812e68f0
0000000180397167 xor       r8d, r8d
000000018039716A mov       edx, 6
000000018039716F mov       ecx, 1
0000000180397174 call      0x18130d310
0000000180397179 test      rbx, rbx
000000018039717C je        0x18039754d
0000000180397182 mov       dword ptr [rbx + 0x14], eax
0000000180397185 mov       dword ptr [rsp + 0x4c], r15d
000000018039718A xor       r8d, r8d
000000018039718D movaps    xmm1, xmm8
0000000180397191 movaps    xmm0, xmm9
0000000180397195 call      0x18130d350
000000018039719A movaps    xmm7, xmm0
000000018039719D movd      xmm1, dword ptr [rbx + 0x14]
00000001803971A2 cvtdq2ps  xmm1, xmm1
00000001803971A5 mulss     xmm1, xmm10
00000001803971AA movaps    xmm6, xmm11
00000001803971AE subss     xmm6, xmm1
00000001803971B2 xor       r8d, r8d
00000001803971B5 movaps    xmm1, xmm12
00000001803971B9 movaps    xmm0, xmm13
00000001803971BD call      0x18130d350
00000001803971C2 addss     xmm0, xmm6
00000001803971C6 unpcklps  xmm7, xmm0
00000001803971C9 movsd     qword ptr [rbx + 0x1c], xmm7
00000001803971CE movss     dword ptr [rbx + 0x24], xmm14
00000001803971D4 xor       r8d, r8d
00000001803971D7 mov       rdx, rbx
00000001803971DA mov       rcx, r14
00000001803971DD call      0x180396b30
00000001803971E2 mov       eax, dword ptr [rsp + 0x170]
00000001803971E9 inc       eax
00000001803971EB mov       dword ptr [rsp + 0x170], eax
00000001803971F2 mov       ecx, eax
00000001803971F4 jmp       0x180397100
00000001803971F9 mov       rcx, qword ptr [rip + 0x1825708] # 0x181bbc908
0000000180397200 call      0x180250100
0000000180397205 mov       rsi, rax
0000000180397208 mov       rdx, qword ptr [rip + 0x180da29] # 0x181ba4c38
000000018039720F mov       rcx, rax
0000000180397212 call      0x1808053d0
0000000180397217 mov       qword ptr [rsp + 0x178], rsi
000000018039721F mov       rdx, qword ptr [r14 + 0x20]
0000000180397223 test      rdx, rdx
0000000180397226 je        0x18039754d
000000018039722C mov       rdx, qword ptr [rdx + 0x68]
0000000180397230 test      rdx, rdx
0000000180397233 je        0x18039754d
0000000180397239 mov       r8, qword ptr [rip + 0x180db18] # 0x181ba4d58
0000000180397240 lea       rcx, [rsp + 0x30]
0000000180397245 call      0x1808197f0
000000018039724A movups    xmm0, xmmword ptr [rsp + 0x30]
000000018039724F movups    xmmword ptr [rsp + 0x58], xmm0
0000000180397254 movsd     xmm1, qword ptr [rsp + 0x40]
000000018039725A movsd     qword ptr [rsp + 0x68], xmm1
0000000180397260 mov       qword ptr [rsp + 0x48], r15
0000000180397265 lea       rbx, [rsp + 0x58]
000000018039726A mov       qword ptr [rsp + 0x50], rbx
000000018039726F nop       
0000000180397270 mov       rdx, qword ptr [rip + 0x182a359] # 0x181bc15d0
0000000180397277 lea       rcx, [rsp + 0x58]
000000018039727C call      0x180669280
0000000180397281 test      al, al
0000000180397283 je        0x180397324
0000000180397289 mov       rdi, qword ptr [rsp + 0x68]
000000018039728E test      rdi, rdi
0000000180397291 je        0x180397526
0000000180397297 cmp       dword ptr [rdi + 0x10], 0x16
000000018039729B jne       0x1803972c4
000000018039729D cmp       byte ptr [rdi + 0x18], 0
00000001803972A1 jne       0x180397270
00000001803972A3 cmp       byte ptr [rdi + 0x19], 0
00000001803972A7 jne       0x180397270
00000001803972A9 test      rsi, rsi
00000001803972AC je        0x180397511
00000001803972B2 mov       r8, qword ptr [rip + 0x180d9df] # 0x181ba4c98
00000001803972B9 mov       rdx, rdi
00000001803972BC mov       rcx, rsi
00000001803972BF call      0x180002e70
00000001803972C4 test      rdi, rdi
00000001803972C7 je        0x180397521
00000001803972CD cmp       dword ptr [rdi + 0x10], 0x17
00000001803972D1 jne       0x1803972fa
00000001803972D3 cmp       byte ptr [rdi + 0x18], 0
00000001803972D7 jne       0x1803972df
00000001803972D9 cmp       byte ptr [rdi + 0x19], 0
00000001803972DD je        0x180397270
00000001803972DF test      rsi, rsi
00000001803972E2 je        0x180397517
00000001803972E8 mov       r8, qword ptr [rip + 0x180d9a9] # 0x181ba4c98
00000001803972EF mov       rdx, rdi
00000001803972F2 mov       rcx, rsi
00000001803972F5 call      0x180002e70
00000001803972FA cmp       dword ptr [rdi + 0x10], 0x19
00000001803972FE jne       0x180397270
0000000180397304 test      rsi, rsi
0000000180397307 je        0x18039751c
000000018039730D mov       r8, qword ptr [rip + 0x180d984] # 0x181ba4c98
0000000180397314 mov       rdx, rdi
0000000180397317 mov       rcx, rsi
000000018039731A call      0x180002e70
000000018039731F jmp       0x180397270
0000000180397324 mov       rdx, qword ptr [rip + 0x182a24d] # 0x181bc1578
000000018039732B mov       rcx, rbx
000000018039732E call      0x180302170
0000000180397333 jmp       0x180397367
0000000180397335 mov       rdx, qword ptr [rip + 0x182a23c] # 0x181bc1578
000000018039733C mov       rcx, qword ptr [rsp + 0x50]
0000000180397341 call      0x180302170
0000000180397346 mov       rcx, qword ptr [rsp + 0x48]
000000018039734B test      rcx, rcx
000000018039734E jne       0x18039752c
0000000180397354 xor       r15d, r15d
0000000180397357 mov       r14, qword ptr [rsp + 0x160]
000000018039735F mov       rsi, qword ptr [rsp + 0x178]
0000000180397367 mov       rax, qword ptr [r14 + 0x20]
000000018039736B test      rax, rax
000000018039736E je        0x18039754d
0000000180397374 mov       rdx, qword ptr [rax + 0x50]
0000000180397378 test      rdx, rdx
000000018039737B je        0x18039754d
0000000180397381 mov       r8, qword ptr [rip + 0x1812ec0] # 0x181baa248
0000000180397388 lea       rcx, [rsp + 0x30]
000000018039738D call      0x1808197f0
0000000180397392 movups    xmm0, xmmword ptr [rsp + 0x30]
0000000180397397 movups    xmmword ptr [rsp + 0x70], xmm0
000000018039739C movsd     xmm1, qword ptr [rsp + 0x40]
00000001803973A2 movsd     qword ptr [rsp + 0x80], xmm1
00000001803973AB mov       qword ptr [rsp + 0x30], r15
00000001803973B0 lea       rbx, [rsp + 0x70]
00000001803973B5 mov       qword ptr [rsp + 0x38], rbx
00000001803973BA nop       word ptr [rax + rax]
00000001803973C0 mov       rdx, qword ptr [rip + 0x182bac1] # 0x181bc2e88
00000001803973C7 lea       rcx, [rsp + 0x70]
00000001803973CC call      0x180669280
00000001803973D1 test      al, al
00000001803973D3 je        0x1803974a2
00000001803973D9 mov       rdi, qword ptr [rsp + 0x80]
00000001803973E1 test      rdi, rdi
00000001803973E4 je        0x180397541
00000001803973EA mov       rax, qword ptr [rip + 0x1823907] # 0x181bbacf8
00000001803973F1 mov       rdx, qword ptr [rax + 0xb8]
00000001803973F8 xor       r8d, r8d
00000001803973FB mov       rdx, qword ptr [rdx]
00000001803973FE mov       rcx, qword ptr [rdi + 0x38]
0000000180397402 call      0x180b7cc30
0000000180397407 test      al, al
0000000180397409 je        0x180397450
000000018039740B test      rdi, rdi
000000018039740E je        0x180397532
0000000180397414 xor       r8d, r8d
0000000180397417 mov       rdx, qword ptr [rdi + 0x38]
000000018039741B mov       rcx, rsi
000000018039741E call      0x180394560
0000000180397423 test      rax, rax
0000000180397426 je        0x180397450
0000000180397428 mov       rcx, qword ptr [rip + 0x1803919] # 0x181b9ad48
000000018039742F cmp       dword ptr [rcx + 0xe0], 0
0000000180397436 jne       0x18039743d
0000000180397438 call      0x1802501e0
000000018039743D xor       edx, edx
000000018039743F mov       rcx, qword ptr [rip + 0x1809faa] # 0x181ba13f0
0000000180397446 call      0x1812e68f0
000000018039744B jmp       0x1803973c0
0000000180397450 mov       rdx, qword ptr [r14 + 0x20]
0000000180397454 test      rdi, rdi
0000000180397457 je        0x18039753c
000000018039745D test      rdx, rdx
0000000180397460 je        0x180397537
0000000180397466 mov       qword ptr [rsp + 0x20], r15
000000018039746B mov       r9d, dword ptr [rdi + 0x14]
000000018039746F mov       r8d, dword ptr [rdi + 0x18]
0000000180397473 lea       rcx, [rsp + 0x48]
0000000180397478 call      0x18030ef10
000000018039747D mov       rcx, rax
0000000180397480 mov       eax, dword ptr [rax + 8]
0000000180397483 movsd     xmm0, qword ptr [rcx]
0000000180397487 movsd     qword ptr [rdi + 0x1c], xmm0
000000018039748C mov       dword ptr [rdi + 0x24], eax
000000018039748F xor       r8d, r8d
0000000180397492 mov       rdx, rdi
0000000180397495 mov       rcx, r14
0000000180397498 call      0x180396b30
000000018039749D jmp       0x1803973c0
00000001803974A2 mov       rdx, qword ptr [rip + 0x182b987] # 0x181bc2e30
00000001803974A9 mov       rcx, rbx
00000001803974AC call      0x180302170
00000001803974B1 jmp       0x1803974ce
00000001803974B3 mov       rdx, qword ptr [rip + 0x182b976] # 0x181bc2e30
00000001803974BA mov       rcx, qword ptr [rsp + 0x38]
00000001803974BF call      0x180302170
00000001803974C4 mov       rcx, qword ptr [rsp + 0x30]
00000001803974C9 test      rcx, rcx
00000001803974CC jne       0x180397547
00000001803974CE lea       r11, [rsp + 0x130]
00000001803974D6 movaps    xmm6, xmmword ptr [r11 - 0x10]
00000001803974DB movaps    xmm7, xmmword ptr [r11 - 0x20]
00000001803974E0 movaps    xmm8, xmmword ptr [r11 - 0x30]
00000001803974E5 movaps    xmm9, xmmword ptr [r11 - 0x40]
00000001803974EA movaps    xmm10, xmmword ptr [r11 - 0x50]
00000001803974EF movaps    xmm11, xmmword ptr [r11 - 0x60]
00000001803974F4 movaps    xmm12, xmmword ptr [r11 - 0x70]
00000001803974F9 movaps    xmm13, xmmword ptr [r11 - 0x80]
00000001803974FE movaps    xmm14, xmmword ptr [r11 - 0x90]
0000000180397506 mov       rsp, r11
0000000180397509 pop       r15
000000018039750B pop       r14
000000018039750D pop       rdi
000000018039750E pop       rsi
000000018039750F pop       rbx
0000000180397510 ret       
0000000180397511 call      0x180250150
0000000180397516 nop       
0000000180397517 call      0x180250150
000000018039751C call      0x180250150
0000000180397521 call      0x180250150
0000000180397526 call      0x180250150
000000018039752B nop       
000000018039752C call      0x180200e60
0000000180397531 nop       
0000000180397532 call      0x180250150
0000000180397537 call      0x180250150
000000018039753C call      0x180250150
0000000180397541 call      0x180250150
0000000180397546 nop       
0000000180397547 call      0x180200e60
000000018039754C int3      
000000018039754D call      0x180250150
0000000180397552 int3      
0000000180397553 int3      
0000000180397554 int3      
0000000180397555 int3      
0000000180397556 int3      
0000000180397557 int3      
0000000180397558 int3      
0000000180397559 int3      
000000018039755A int3      
000000018039755B int3      
000000018039755C int3      
000000018039755D int3      
000000018039755E int3      
000000018039755F int3      