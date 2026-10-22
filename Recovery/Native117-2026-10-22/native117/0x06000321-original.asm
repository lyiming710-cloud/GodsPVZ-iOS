0000000180348290 mov       rax, rsp
0000000180348293 mov       qword ptr [rax + 0x10], rbx
0000000180348297 mov       qword ptr [rax + 8], rcx
000000018034829B push      rsi
000000018034829C push      rdi
000000018034829D push      r14
000000018034829F sub       rsp, 0xf0
00000001803482A6 movaps    xmmword ptr [rax - 0x28], xmm6
00000001803482AA movaps    xmmword ptr [rax - 0x38], xmm7
00000001803482AE movaps    xmmword ptr [rax - 0x48], xmm8
00000001803482B3 movaps    xmmword ptr [rax - 0x58], xmm9
00000001803482B8 movaps    xmmword ptr [rax - 0x68], xmm10
00000001803482BD movaps    xmmword ptr [rax - 0x78], xmm11
00000001803482C2 movaps    xmmword ptr [rax - 0x88], xmm12
00000001803482CA mov       rsi, rcx
00000001803482CD cmp       byte ptr [rip + 0x199c9f9], 0 # 0x181ce4ccd
00000001803482D4 jne       0x180348365
00000001803482DA lea       rcx, [rip + 0x187a41f] # 0x181bc2700
00000001803482E1 call      0x18024fef0
00000001803482E6 lea       rcx, [rip + 0x187e503] # 0x181bc67f0
00000001803482ED call      0x18024fef0
00000001803482F2 lea       rcx, [rip + 0x187a45f] # 0x181bc2758
00000001803482F9 call      0x18024fef0
00000001803482FE lea       rcx, [rip + 0x187e543] # 0x181bc6848
0000000180348305 call      0x18024fef0
000000018034830A lea       rcx, [rip + 0x187e58f] # 0x181bc68a0
0000000180348311 call      0x18024fef0
0000000180348316 lea       rcx, [rip + 0x187a493] # 0x181bc27b0
000000018034831D call      0x18024fef0
0000000180348322 lea       rcx, [rip + 0x1860ce7] # 0x181ba9010
0000000180348329 call      0x18024fef0
000000018034832E lea       rcx, [rip + 0x186bfdb] # 0x181bb4310
0000000180348335 call      0x18024fef0
000000018034833A lea       rcx, [rip + 0x1860d7f] # 0x181ba90c0
0000000180348341 call      0x18024fef0
0000000180348346 lea       rcx, [rip + 0x1860c6b] # 0x181ba8fb8
000000018034834D call      0x18024fef0
0000000180348352 lea       rcx, [rip + 0x1874fff] # 0x181bbd358
0000000180348359 call      0x18024fef0
000000018034835E mov       byte ptr [rip + 0x199c968], 1 # 0x181ce4ccd
0000000180348365 xorps     xmm0, xmm0
0000000180348368 xor       eax, eax
000000018034836A movups    xmmword ptr [rsp + 0x38], xmm0
000000018034836F mov       qword ptr [rsp + 0x48], rax
0000000180348374 movss     xmm10, dword ptr [rsi + 0x80]
000000018034837D mulss     xmm10, dword ptr [rip + 0x125f7ce] # 0x1815a7b54
0000000180348386 mov       rax, qword ptr [rsi + 0x28]
000000018034838A test      rax, rax
000000018034838D je        0x180348811
0000000180348393 mov       rax, qword ptr [rax + 0xe8]
000000018034839A test      rax, rax
000000018034839D je        0x180348811
00000001803483A3 mov       rdx, qword ptr [rax + 0x30]
00000001803483A7 test      rdx, rdx
00000001803483AA je        0x180348811
00000001803483B0 mov       r8, qword ptr [rip + 0x186bf59] # 0x181bb4310
00000001803483B7 lea       rcx, [rsp + 0x20]
00000001803483BC call      0x1808197f0
00000001803483C1 movups    xmm0, xmmword ptr [rsp + 0x20]
00000001803483C6 movups    xmmword ptr [rsp + 0x50], xmm0
00000001803483CB movsd     xmm1, qword ptr [rsp + 0x30]
00000001803483D1 movsd     qword ptr [rsp + 0x60], xmm1
00000001803483D7 mov       qword ptr [rsp + 0x20], 0
00000001803483E0 lea       rbx, [rsp + 0x50]
00000001803483E5 mov       qword ptr [rsp + 0x28], rbx
00000001803483EA movss     xmm12, dword ptr [rip + 0x125f8a1] # 0x1815a7c94
00000001803483F3 mov       rdx, qword ptr [rip + 0x187e44e] # 0x181bc6848
00000001803483FA lea       rcx, [rsp + 0x50]
00000001803483FF call      0x180669280
0000000180348404 test      al, al
0000000180348406 je        0x1803484cb
000000018034840C mov       rdi, qword ptr [rsp + 0x60]
0000000180348411 test      rdi, rdi
0000000180348414 je        0x1803487e3
000000018034841A movss     xmm6, dword ptr [rdi + 0x30]
000000018034841F movss     xmm7, dword ptr [rdi + 0x34]
0000000180348424 movss     xmm8, dword ptr [rsi + 0x38]
000000018034842A movss     xmm9, dword ptr [rsi + 0x3c]
0000000180348430 cmp       byte ptr [rip + 0x199c905], 0 # 0x181ce4d3c
0000000180348437 jne       0x18034844c
0000000180348439 lea       rcx, [rip + 0x18647b8] # 0x181bacbf8
0000000180348440 call      0x18024fef0
0000000180348445 mov       byte ptr [rip + 0x199c8f0], 1 # 0x181ce4d3c
000000018034844C subss     xmm6, xmm8
0000000180348451 subss     xmm7, xmm9
0000000180348456 mov       rcx, qword ptr [rip + 0x186479b] # 0x181bacbf8
000000018034845D cmp       dword ptr [rcx + 0xe0], 0
0000000180348464 jne       0x18034846b
0000000180348466 call      0x1802501e0
000000018034846B mulss     xmm7, xmm7
000000018034846F mulss     xmm6, xmm6
0000000180348473 addss     xmm6, xmm7
0000000180348477 cvtps2pd  xmm1, xmm6
000000018034847A xorps     xmm0, xmm0
000000018034847D ucomisd   xmm0, xmm1
0000000180348481 ja        0x180348489
0000000180348483 sqrtpd    xmm0, xmm1
0000000180348487 jmp       0x180348491
0000000180348489 movaps    xmm0, xmm1
000000018034848C call      0x1802cb7b4
0000000180348491 cvtsd2ss  xmm0, xmm0
0000000180348495 comiss    xmm0, xmm12
0000000180348499 ja        0x1803483f3
000000018034849F test      rdi, rdi
00000001803484A2 je        0x1803487de
00000001803484A8 mov       rcx, qword ptr [rdi + 0xf8]
00000001803484AF test      rcx, rcx
00000001803484B2 je        0x1803487d8
00000001803484B8 xor       r9d, r9d
00000001803484BB movaps    xmm2, xmm10
00000001803484BF xor       edx, edx
00000001803484C1 call      0x180314a00
00000001803484C6 jmp       0x1803483f3
00000001803484CB mov       rdx, qword ptr [rip + 0x187e31e] # 0x181bc67f0
00000001803484D2 mov       rcx, rbx
00000001803484D5 call      0x180302170
00000001803484DA jmp       0x18034850c
00000001803484DC mov       rdx, qword ptr [rip + 0x187e30d] # 0x181bc67f0
00000001803484E3 mov       rcx, qword ptr [rsp + 0x28]
00000001803484E8 call      0x180302170
00000001803484ED mov       rcx, qword ptr [rsp + 0x20]
00000001803484F2 test      rcx, rcx
00000001803484F5 jne       0x1803487e9
00000001803484FB mov       rsi, qword ptr [rsp + 0x110]
0000000180348503 movss     xmm12, dword ptr [rip + 0x125f788] # 0x1815a7c94
000000018034850C movss     xmm10, dword ptr [rsi + 0x80]
0000000180348515 mulss     xmm10, dword ptr [rip + 0x125f7a2] # 0x1815a7cc0
000000018034851E movss     dword ptr [rsp + 0x110], xmm10
0000000180348528 mov       rcx, qword ptr [rip + 0x1874e29] # 0x181bbd358
000000018034852F call      0x180250100
0000000180348534 mov       r14, rax
0000000180348537 mov       rdx, qword ptr [rip + 0x1860a7a] # 0x181ba8fb8
000000018034853E mov       rcx, rax
0000000180348541 call      0x1808053d0
0000000180348546 mov       qword ptr [rsp + 0x120], r14
000000018034854E mov       rax, qword ptr [rsi + 0x28]
0000000180348552 test      rax, rax
0000000180348555 je        0x180348811
000000018034855B mov       rax, qword ptr [rax + 0x108]
0000000180348562 test      rax, rax
0000000180348565 je        0x180348811
000000018034856B mov       rdx, qword ptr [rax + 0x30]
000000018034856F test      rdx, rdx
0000000180348572 je        0x180348811
0000000180348578 mov       r8, qword ptr [rip + 0x1860b41] # 0x181ba90c0
000000018034857F lea       rcx, [rsp + 0x20]
0000000180348584 call      0x1808197f0
0000000180348589 movups    xmm0, xmmword ptr [rsp + 0x20]
000000018034858E movups    xmmword ptr [rsp + 0x38], xmm0
0000000180348593 movsd     xmm1, qword ptr [rsp + 0x30]
0000000180348599 movsd     qword ptr [rsp + 0x48], xmm1
000000018034859F mov       qword ptr [rsp + 0x20], 0
00000001803485A8 lea       rbx, [rsp + 0x38]
00000001803485AD mov       qword ptr [rsp + 0x28], rbx
00000001803485B2 xorps     xmm11, xmm11
00000001803485B6 nop       word ptr [rax + rax]
00000001803485C0 mov       rdx, qword ptr [rip + 0x187a191] # 0x181bc2758
00000001803485C7 lea       rcx, [rsp + 0x38]
00000001803485CC call      0x180669280
00000001803485D1 test      al, al
00000001803485D3 je        0x1803486a3
00000001803485D9 mov       rdi, qword ptr [rsp + 0x48]
00000001803485DE test      rdi, rdi
00000001803485E1 je        0x1803487f9
00000001803485E7 movss     xmm6, dword ptr [rdi + 0x38]
00000001803485EC movss     xmm7, dword ptr [rdi + 0x3c]
00000001803485F1 movss     xmm8, dword ptr [rsi + 0x38]
00000001803485F7 movss     xmm9, dword ptr [rsi + 0x3c]
00000001803485FD cmp       byte ptr [rip + 0x199c738], 0 # 0x181ce4d3c
0000000180348604 jne       0x180348619
0000000180348606 lea       rcx, [rip + 0x18645eb] # 0x181bacbf8
000000018034860D call      0x18024fef0
0000000180348612 mov       byte ptr [rip + 0x199c723], 1 # 0x181ce4d3c
0000000180348619 subss     xmm6, xmm8
000000018034861E subss     xmm7, xmm9
0000000180348623 mov       rcx, qword ptr [rip + 0x18645ce] # 0x181bacbf8
000000018034862A cmp       dword ptr [rcx + 0xe0], 0
0000000180348631 jne       0x180348638
0000000180348633 call      0x1802501e0
0000000180348638 mulss     xmm7, xmm7
000000018034863C mulss     xmm6, xmm6
0000000180348640 addss     xmm6, xmm7
0000000180348644 cvtps2pd  xmm1, xmm6
0000000180348647 xorps     xmm0, xmm0
000000018034864A ucomisd   xmm0, xmm1
000000018034864E ja        0x180348656
0000000180348650 sqrtpd    xmm0, xmm1
0000000180348654 jmp       0x18034865e
0000000180348656 movaps    xmm0, xmm1
0000000180348659 call      0x1802cb7b4
000000018034865E cvtsd2ss  xmm0, xmm0
0000000180348662 comiss    xmm0, xmm12
0000000180348666 ja        0x1803485c0
000000018034866C test      rdi, rdi
000000018034866F je        0x1803487f4
0000000180348675 comiss    xmm11, dword ptr [rdi + 0x88]
000000018034867D jae       0x1803485c0
0000000180348683 test      r14, r14
0000000180348686 je        0x1803487ef
000000018034868C mov       r8, qword ptr [rip + 0x186097d] # 0x181ba9010
0000000180348693 mov       rdx, rdi
0000000180348696 mov       rcx, r14
0000000180348699 call      0x180002e70
000000018034869E jmp       0x1803485c0
00000001803486A3 mov       rdx, qword ptr [rip + 0x187a056] # 0x181bc2700
00000001803486AA mov       rcx, rbx
00000001803486AD call      0x180302170
00000001803486B2 jmp       0x1803486e5
00000001803486B4 mov       rdx, qword ptr [rip + 0x187a045] # 0x181bc2700
00000001803486BB mov       rcx, qword ptr [rsp + 0x28]
00000001803486C0 call      0x180302170
00000001803486C5 mov       rcx, qword ptr [rsp + 0x20]
00000001803486CA test      rcx, rcx
00000001803486CD jne       0x1803487ff
00000001803486D3 movss     xmm10, dword ptr [rsp + 0x110]
00000001803486DD mov       r14, qword ptr [rsp + 0x120]
00000001803486E5 test      r14, r14
00000001803486E8 je        0x180348811
00000001803486EE mov       r8, qword ptr [rip + 0x18609cb] # 0x181ba90c0
00000001803486F5 mov       rdx, r14
00000001803486F8 lea       rcx, [rsp + 0x20]
00000001803486FD call      0x1808197f0
0000000180348702 movups    xmm0, xmmword ptr [rsp + 0x20]
0000000180348707 movups    xmmword ptr [rsp + 0x38], xmm0
000000018034870C movsd     xmm1, qword ptr [rsp + 0x30]
0000000180348712 movsd     qword ptr [rsp + 0x48], xmm1
0000000180348718 mov       qword ptr [rsp + 0x20], 0
0000000180348721 lea       rbx, [rsp + 0x38]
0000000180348726 mov       qword ptr [rsp + 0x28], rbx
000000018034872B nop       dword ptr [rax + rax]
0000000180348730 mov       rdx, qword ptr [rip + 0x187a021] # 0x181bc2758
0000000180348737 lea       rcx, [rsp + 0x38]
000000018034873C call      0x180669280
0000000180348741 test      al, al
0000000180348743 je        0x180348775
0000000180348745 mov       rdi, qword ptr [rsp + 0x48]
000000018034874A test      rdi, rdi
000000018034874D je        0x180348805
0000000180348753 movss     xmm6, dword ptr [rdi + 0x88]
000000018034875B xor       ecx, ecx
000000018034875D call      0x18132c2f0
0000000180348762 mulss     xmm0, xmm10
0000000180348767 addss     xmm0, xmm6
000000018034876B movss     dword ptr [rdi + 0x88], xmm0
0000000180348773 jmp       0x180348730
0000000180348775 mov       rdx, qword ptr [rip + 0x1879f84] # 0x181bc2700
000000018034877C mov       rcx, rbx
000000018034877F call      0x180302170
0000000180348784 jmp       0x1803487a1
0000000180348786 mov       rdx, qword ptr [rip + 0x1879f73] # 0x181bc2700
000000018034878D mov       rcx, qword ptr [rsp + 0x28]
0000000180348792 call      0x180302170
0000000180348797 mov       rcx, qword ptr [rsp + 0x20]
000000018034879C test      rcx, rcx
000000018034879F jne       0x18034880b
00000001803487A1 lea       r11, [rsp + 0xf0]
00000001803487A9 mov       rbx, qword ptr [r11 + 0x28]
00000001803487AD movaps    xmm6, xmmword ptr [r11 - 0x10]
00000001803487B2 movaps    xmm7, xmmword ptr [r11 - 0x20]
00000001803487B7 movaps    xmm8, xmmword ptr [r11 - 0x30]
00000001803487BC movaps    xmm9, xmmword ptr [r11 - 0x40]
00000001803487C1 movaps    xmm10, xmmword ptr [r11 - 0x50]
00000001803487C6 movaps    xmm11, xmmword ptr [r11 - 0x60]
00000001803487CB movaps    xmm12, xmmword ptr [r11 - 0x70]
00000001803487D0 mov       rsp, r11
00000001803487D3 pop       r14
00000001803487D5 pop       rdi
00000001803487D6 pop       rsi
00000001803487D7 ret       
00000001803487D8 call      0x180250150
00000001803487DD nop       
00000001803487DE call      0x180250150
00000001803487E3 call      0x180250150
00000001803487E8 nop       
00000001803487E9 call      0x180200e60
00000001803487EE nop       
00000001803487EF call      0x180250150
00000001803487F4 call      0x180250150
00000001803487F9 call      0x180250150
00000001803487FE nop       
00000001803487FF call      0x180200e60
0000000180348804 nop       
0000000180348805 call      0x180250150
000000018034880A nop       
000000018034880B call      0x180200e60
0000000180348810 int3      
0000000180348811 call      0x180250150
0000000180348816 int3      
0000000180348817 int3      
0000000180348818 int3      
0000000180348819 int3      
000000018034881A int3      
000000018034881B int3      
000000018034881C int3      
000000018034881D int3      
000000018034881E int3      
000000018034881F int3      