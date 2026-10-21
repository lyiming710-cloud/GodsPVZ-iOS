00000001803183E0 mov       rax, rsp
00000001803183E3 mov       qword ptr [rax + 8], rbx
00000001803183E7 mov       qword ptr [rax + 0x10], rsi
00000001803183EB mov       qword ptr [rax + 0x18], rdi
00000001803183EF mov       qword ptr [rax + 0x20], r14
00000001803183F3 push      r15
00000001803183F5 sub       rsp, 0xd0
00000001803183FC movaps    xmmword ptr [rax - 0x18], xmm6
0000000180318400 movaps    xmmword ptr [rax - 0x28], xmm7
0000000180318404 mov       rsi, rcx
0000000180318407 cmp       byte ptr [rip + 0x19cc7ad], 0 # 0x181ce4bbb
000000018031840E jne       0x180318477
0000000180318410 lea       rcx, [rip + 0x1882931] # 0x181b9ad48
0000000180318417 call      0x18024fef0
000000018031841C lea       rcx, [rip + 0x18aaa0d] # 0x181bc2e30
0000000180318423 call      0x18024fef0
0000000180318428 lea       rcx, [rip + 0x18aaa59] # 0x181bc2e88
000000018031842F call      0x18024fef0
0000000180318434 lea       rcx, [rip + 0x18aaaa5] # 0x181bc2ee0
000000018031843B call      0x18024fef0
0000000180318440 lea       rcx, [rip + 0x1891da9] # 0x181baa1f0
0000000180318447 call      0x18024fef0
000000018031844C lea       rcx, [rip + 0x1891df5] # 0x181baa248
0000000180318453 call      0x18024fef0
0000000180318458 lea       rcx, [rip + 0x18a2899] # 0x181bbacf8
000000018031845F call      0x18024fef0
0000000180318464 lea       rcx, [rip + 0x1888f85] # 0x181ba13f0
000000018031846B call      0x18024fef0
0000000180318470 mov       byte ptr [rip + 0x19cc744], 1 # 0x181ce4bbb
0000000180318477 mov       rax, qword ptr [rsi + 0x80]
000000018031847E test      rax, rax
0000000180318481 je        0x18031871d
0000000180318487 mov       rcx, qword ptr [rax + 0x28]
000000018031848B test      rcx, rcx
000000018031848E je        0x18031871d
0000000180318494 mov       rdx, qword ptr [rcx + 0x50]
0000000180318498 test      rdx, rdx
000000018031849B je        0x18031871d
00000001803184A1 mov       r8, qword ptr [rip + 0x1891da0] # 0x181baa248
00000001803184A8 lea       rcx, [rsp + 0x30]
00000001803184AD call      0x1808197f0
00000001803184B2 movups    xmm0, xmmword ptr [rsp + 0x30]
00000001803184B7 movups    xmmword ptr [rsp + 0x48], xmm0
00000001803184BC movsd     xmm1, qword ptr [rsp + 0x40]
00000001803184C2 movsd     qword ptr [rsp + 0x58], xmm1
00000001803184C8 xor       r15d, r15d
00000001803184CB mov       qword ptr [rsp + 0x30], r15
00000001803184D0 lea       rbx, [rsp + 0x48]
00000001803184D5 mov       qword ptr [rsp + 0x38], rbx
00000001803184DA movss     xmm6, dword ptr [rip + 0x128f636] # 0x1815a7b18
00000001803184E2 movss     xmm7, dword ptr [rip + 0x128f622] # 0x1815a7b0c
00000001803184EA nop       word ptr [rax + rax]
00000001803184F0 mov       rdx, qword ptr [rip + 0x18aa991] # 0x181bc2e88
00000001803184F7 lea       rcx, [rsp + 0x48]
00000001803184FC call      0x180669280
0000000180318501 test      al, al
0000000180318503 je        0x180318699
0000000180318509 mov       rdi, qword ptr [rsp + 0x58]
000000018031850E test      rdi, rdi
0000000180318511 je        0x180318711
0000000180318517 mov       rax, qword ptr [rip + 0x18a27da] # 0x181bbacf8
000000018031851E mov       rdx, qword ptr [rax + 0xb8]
0000000180318525 xor       r8d, r8d
0000000180318528 mov       rdx, qword ptr [rdx]
000000018031852B mov       rcx, qword ptr [rdi + 0x38]
000000018031852F call      0x180b7cc30
0000000180318534 test      al, al
0000000180318536 je        0x18031857e
0000000180318538 test      rdi, rdi
000000018031853B je        0x1803186ed
0000000180318541 xor       r8d, r8d
0000000180318544 mov       rdx, qword ptr [rdi + 0x38]
0000000180318548 mov       rcx, qword ptr [rsi + 0x40]
000000018031854C call      0x180394560
0000000180318551 test      rax, rax
0000000180318554 je        0x180318587
0000000180318556 mov       rcx, qword ptr [rip + 0x18827eb] # 0x181b9ad48
000000018031855D cmp       dword ptr [rcx + 0xe0], 0
0000000180318564 jne       0x18031856b
0000000180318566 call      0x1802501e0
000000018031856B xor       edx, edx
000000018031856D mov       rcx, qword ptr [rip + 0x1888e7c] # 0x181ba13f0
0000000180318574 call      0x1812e68f0
0000000180318579 jmp       0x1803184f0
000000018031857E test      rdi, rdi
0000000180318581 je        0x18031870c
0000000180318587 cmp       dword ptr [rdi + 0x28], 0
000000018031858B je        0x180318629
0000000180318591 xor       r8d, r8d
0000000180318594 mov       edx, dword ptr [rdi + 0x28]
0000000180318597 mov       rcx, rsi
000000018031859A call      0x180318120
000000018031859F mov       r14, rax
00000001803185A2 test      rax, rax
00000001803185A5 je        0x1803184f0
00000001803185AB mov       rcx, qword ptr [rsi + 0x80]
00000001803185B2 test      rcx, rcx
00000001803185B5 je        0x1803186fd
00000001803185BB mov       rdx, qword ptr [rcx + 0x28]
00000001803185BF test      rdx, rdx
00000001803185C2 je        0x1803186f8
00000001803185C8 mov       qword ptr [rsp + 0x20], r15
00000001803185CD mov       r9d, dword ptr [rdi + 0x14]
00000001803185D1 mov       r8d, dword ptr [rdi + 0x18]
00000001803185D5 lea       rcx, [rsp + 0x88]
00000001803185DD call      0x18030ec90
00000001803185E2 movsd     xmm0, qword ptr [rax]
00000001803185E6 mov       eax, dword ptr [rax + 8]
00000001803185E9 movsd     qword ptr [rsp + 0x60], xmm0
00000001803185EF addss     xmm0, xmm6
00000001803185F3 movss     xmm1, dword ptr [rsp + 0x64]
00000001803185F9 addss     xmm1, xmm7
00000001803185FD unpcklps  xmm0, xmm1
0000000180318600 movsd     qword ptr [rdi + 0x1c], xmm0
0000000180318605 mov       dword ptr [rdi + 0x24], eax
0000000180318608 mov       rcx, qword ptr [r14 + 0x10]
000000018031860C test      rcx, rcx
000000018031860F je        0x1803186f3
0000000180318615 mov       r8, qword ptr [rip + 0x1891bd4] # 0x181baa1f0
000000018031861C mov       rdx, rdi
000000018031861F call      0x180002e70
0000000180318624 jmp       0x1803184f0
0000000180318629 mov       rax, qword ptr [rsi + 0x80]
0000000180318630 test      rax, rax
0000000180318633 je        0x180318707
0000000180318639 mov       rdx, qword ptr [rax + 0x28]
000000018031863D test      rdx, rdx
0000000180318640 je        0x180318702
0000000180318646 mov       qword ptr [rsp + 0x20], r15
000000018031864B mov       r9d, dword ptr [rdi + 0x14]
000000018031864F mov       r8d, dword ptr [rdi + 0x18]
0000000180318653 lea       rcx, [rsp + 0x98]
000000018031865B call      0x18030ec90
0000000180318660 movsd     xmm0, qword ptr [rax]
0000000180318664 mov       eax, dword ptr [rax + 8]
0000000180318667 movsd     qword ptr [rsp + 0x70], xmm0
000000018031866D addss     xmm0, xmm6
0000000180318671 movss     xmm1, dword ptr [rsp + 0x74]
0000000180318677 addss     xmm1, xmm7
000000018031867B unpcklps  xmm0, xmm1
000000018031867E movsd     qword ptr [rdi + 0x1c], xmm0
0000000180318683 mov       dword ptr [rdi + 0x24], eax
0000000180318686 xor       r8d, r8d
0000000180318689 mov       rdx, rdi
000000018031868C mov       rcx, rsi
000000018031868F call      0x180317980
0000000180318694 jmp       0x1803184f0
0000000180318699 mov       rdx, qword ptr [rip + 0x18aa790] # 0x181bc2e30
00000001803186A0 mov       rcx, rbx
00000001803186A3 call      0x180302170
00000001803186A8 jmp       0x1803186c5
00000001803186AA mov       rdx, qword ptr [rip + 0x18aa77f] # 0x181bc2e30
00000001803186B1 mov       rcx, qword ptr [rsp + 0x38]
00000001803186B6 call      0x180302170
00000001803186BB mov       rcx, qword ptr [rsp + 0x30]
00000001803186C0 test      rcx, rcx
00000001803186C3 jne       0x180318717
00000001803186C5 lea       r11, [rsp + 0xd0]
00000001803186CD mov       rbx, qword ptr [r11 + 0x10]
00000001803186D1 mov       rsi, qword ptr [r11 + 0x18]
00000001803186D5 mov       rdi, qword ptr [r11 + 0x20]
00000001803186D9 mov       r14, qword ptr [r11 + 0x28]
00000001803186DD movaps    xmm6, xmmword ptr [r11 - 0x10]
00000001803186E2 movaps    xmm7, xmmword ptr [r11 - 0x20]
00000001803186E7 mov       rsp, r11
00000001803186EA pop       r15
00000001803186EC ret       
00000001803186ED call      0x180250150
00000001803186F2 nop       
00000001803186F3 call      0x180250150
00000001803186F8 call      0x180250150
00000001803186FD call      0x180250150
0000000180318702 call      0x180250150
0000000180318707 call      0x180250150
000000018031870C call      0x180250150
0000000180318711 call      0x180250150
0000000180318716 nop       
0000000180318717 call      0x180200e60
000000018031871C int3      
000000018031871D call      0x180250150
0000000180318722 int3      
0000000180318723 int3      
0000000180318724 int3      
0000000180318725 int3      
0000000180318726 int3      
0000000180318727 int3      
0000000180318728 int3      
0000000180318729 int3      
000000018031872A int3      
000000018031872B int3      
000000018031872C int3      
000000018031872D int3      
000000018031872E int3      
000000018031872F int3      