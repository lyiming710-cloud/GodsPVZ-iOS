0000000180367540 mov       rax, rsp
0000000180367543 mov       qword ptr [rax + 8], rcx
0000000180367547 push      rbx
0000000180367548 push      rsi
0000000180367549 push      rdi
000000018036754A sub       rsp, 0xa0
0000000180367551 movaps    xmmword ptr [rax - 0x28], xmm6
0000000180367555 movaps    xmmword ptr [rax - 0x38], xmm7
0000000180367559 movaps    xmmword ptr [rax - 0x48], xmm8
000000018036755E mov       rdi, rcx
0000000180367561 cmp       byte ptr [rip + 0x197d81c], 0 # 0x181ce4d84
0000000180367568 jne       0x1803675a1
000000018036756A lea       rcx, [rip + 0x183d97f] # 0x181ba4ef0
0000000180367571 call      0x18024fef0
0000000180367576 lea       rcx, [rip + 0x183dcd3] # 0x181ba5250
000000018036757D call      0x18024fef0
0000000180367582 lea       rcx, [rip + 0x18580df] # 0x181bbf668
0000000180367589 call      0x18024fef0
000000018036758E lea       rcx, [rip + 0x184aefb] # 0x181bb2490
0000000180367595 call      0x18024fef0
000000018036759A mov       byte ptr [rip + 0x197d7e3], 1 # 0x181ce4d84
00000001803675A1 xor       esi, esi
00000001803675A3 mov       qword ptr [rsp + 0x20], rsi
00000001803675A8 cmp       byte ptr [rip + 0x197d587], sil # 0x181ce4b36
00000001803675AF jne       0x1803675c4
00000001803675B1 lea       rcx, [rip + 0x185b230] # 0x181bc27e8
00000001803675B8 call      0x18024fef0
00000001803675BD mov       byte ptr [rip + 0x197d572], 1 # 0x181ce4b36
00000001803675C4 mov       rax, qword ptr [rip + 0x185b21d] # 0x181bc27e8
00000001803675CB mov       rcx, qword ptr [rax + 0xb8]
00000001803675D2 movsd     xmm0, qword ptr [rcx]
00000001803675D6 movss     dword ptr [rsp + 0x30], xmm0
00000001803675DC movaps    xmm6, xmm0
00000001803675DF shufps    xmm6, xmm6, 0x55
00000001803675E3 movsd     qword ptr [rsp + 0x40], xmm0
00000001803675E9 movss     dword ptr [rsp + 0x34], xmm6
00000001803675EF movss     xmm0, dword ptr [rcx + 8]
00000001803675F4 movss     dword ptr [rsp + 0x38], xmm0
00000001803675FA cmp       byte ptr [rdi + 0xb8], sil
0000000180367601 jne       0x1803678cb
0000000180367607 cmp       byte ptr [rdi + 0xbc], sil
000000018036760E jne       0x1803678cb
0000000180367614 movd      xmm0, dword ptr [rdi + 0x2c]
0000000180367619 cvtdq2ps  xmm0, xmm0
000000018036761C mulss     xmm0, dword ptr [rip + 0x1240508] # 0x1815a7b2c
0000000180367624 movss     xmm7, dword ptr [rip + 0x1240708] # 0x1815a7d34
000000018036762C subss     xmm7, xmm0
0000000180367630 movss     dword ptr [rsp + 0xd0], xmm7
0000000180367639 xor       edx, edx
000000018036763B mov       rcx, rdi
000000018036763E call      0x181304510
0000000180367643 test      rax, rax
0000000180367646 je        0x1803678ec
000000018036764C xor       edx, edx
000000018036764E mov       rcx, rax
0000000180367651 call      0x18132dc20
0000000180367656 mov       qword ptr [rsp + 0xd8], rax
000000018036765E lea       rax, [rsp + 0xd8]
0000000180367666 mov       qword ptr [rsp + 0x40], rax
000000018036766B lea       rax, [rsp + 0x20]
0000000180367670 mov       qword ptr [rsp + 0x48], rax
0000000180367675 mov       qword ptr [rsp + 0x50], rsi
000000018036767A movups    xmm8, xmmword ptr [rsp + 0x40]
0000000180367680 movups    xmmword ptr [rsp + 0x58], xmm8
0000000180367686 nop       word ptr [rax + rax]
0000000180367690 mov       r8, qword ptr [rsp + 0xd8]
0000000180367698 test      r8, r8
000000018036769B je        0x180367905
00000001803676A1 xor       ecx, ecx
00000001803676A3 mov       rdx, qword ptr [rip + 0x183dba6] # 0x181ba5250
00000001803676AA call      0x180002cd0
00000001803676AF test      al, al
00000001803676B1 je        0x1803677e3
00000001803676B7 mov       rbx, qword ptr [rsp + 0xd8]
00000001803676BF test      rbx, rbx
00000001803676C2 je        0x180367900
00000001803676C8 mov       r9, qword ptr [rip + 0x183db81] # 0x181ba5250
00000001803676CF mov       r10, qword ptr [rbx]
00000001803676D2 movzx     ecx, si
00000001803676D5 movzx     edx, word ptr [r10 + 0x12e]
00000001803676DD cmp       si, dx
00000001803676E0 jae       0x180367704
00000001803676E2 mov       r8, qword ptr [r10 + 0xb0]
00000001803676E9 nop       dword ptr [rax]
00000001803676F0 movzx     eax, cx
00000001803676F3 add       rax, rax
00000001803676F6 cmp       qword ptr [r8 + rax*8], r9
00000001803676FA je        0x180367739
00000001803676FC inc       cx
00000001803676FF cmp       cx, dx
0000000180367702 jb        0x1803676f0
0000000180367704 mov       r8d, 1
000000018036770A mov       rdx, r9
000000018036770D mov       rcx, rbx
0000000180367710 call      0x180258380
0000000180367715 mov       rdx, rax
0000000180367718 mov       rax, qword ptr [rdx]
000000018036771B mov       rdx, qword ptr [rdx + 8]
000000018036771F mov       rcx, rbx
0000000180367722 call      rax
0000000180367724 mov       rbx, rax
0000000180367727 mov       rdx, qword ptr [rip + 0x1857f3a] # 0x181bbf668
000000018036772E test      rax, rax
0000000180367731 je        0x1803678f2
0000000180367737 jmp       0x180367759
0000000180367739 movzx     ecx, cx
000000018036773C add       rcx, rcx
000000018036773F mov       ecx, dword ptr [r8 + rcx*8 + 8]
0000000180367744 inc       ecx
0000000180367746 movsxd    rdx, ecx
0000000180367749 shl       rdx, 4
000000018036774D add       rdx, 0x138
0000000180367754 add       rdx, r10
0000000180367757 jmp       0x180367718
0000000180367759 mov       r8, qword ptr [rax]
000000018036775C movzx     eax, byte ptr [rdx + 0x130]
0000000180367763 cmp       byte ptr [r8 + 0x130], al
000000018036776A jb        0x1803678f8
0000000180367770 movzx     ecx, al
0000000180367773 mov       rax, qword ptr [r8 + 0xc8]
000000018036777A cmp       qword ptr [rax + rcx*8 - 8], rdx
000000018036777F jne       0x1803678f8
0000000180367785 xor       edx, edx
0000000180367787 mov       rcx, rbx
000000018036778A call      0x18131f6f0
000000018036778F xor       r8d, r8d
0000000180367792 mov       rdx, qword ptr [rip + 0x184acf7] # 0x181bb2490
0000000180367799 mov       rcx, rax
000000018036779C call      0x180b76170
00000001803677A1 test      al, al
00000001803677A3 je        0x180367690
00000001803677A9 xor       r8d, r8d
00000001803677AC mov       rdx, rbx
00000001803677AF lea       rcx, [rsp + 0x40]
00000001803677B4 call      0x18132f3a0
00000001803677B9 movsd     xmm1, qword ptr [rax]
00000001803677BD mov       eax, dword ptr [rax + 8]
00000001803677C0 mov       dword ptr [rsp + 0x38], eax
00000001803677C4 movss     dword ptr [rdi + 0x30], xmm1
00000001803677C9 movaps    xmm0, xmm1
00000001803677CC shufps    xmm0, xmm0, 0x55
00000001803677D0 movsd     qword ptr [rsp + 0x30], xmm1
00000001803677D6 movaps    xmm6, xmm0
00000001803677D9 movss     dword ptr [rdi + 0x34], xmm0
00000001803677DE jmp       0x180367690
00000001803677E3 movq      rax, xmm8
00000001803677E8 mov       rdx, qword ptr [rip + 0x183d701] # 0x181ba4ef0
00000001803677EF mov       rcx, qword ptr [rax]
00000001803677F2 call      0x18024f380
00000001803677F7 mov       r8, rax
00000001803677FA psrldq    xmm8, 8
0000000180367800 movq      rax, xmm8
0000000180367805 mov       qword ptr [rax], r8
0000000180367808 test      r8, r8
000000018036780B je        0x18036781b
000000018036780D xor       ecx, ecx
000000018036780F mov       rdx, qword ptr [rip + 0x183d6da] # 0x181ba4ef0
0000000180367816 call      0x180002d90
000000018036781B jmp       0x18036784c
000000018036781D lea       rcx, [rsp + 0x58]
0000000180367822 call      0x1802fb000
0000000180367827 mov       rcx, qword ptr [rsp + 0x50]
000000018036782C test      rcx, rcx
000000018036782F jne       0x18036790b
0000000180367835 mov       rdi, qword ptr [rsp + 0xc0]
000000018036783D movss     xmm6, dword ptr [rsp + 0x34]
0000000180367843 movss     xmm7, dword ptr [rsp + 0xd0]
000000018036784C comiss    xmm7, xmm6
000000018036784F ja        0x180367876
0000000180367851 comiss    xmm6, xmm7
0000000180367854 jbe       0x18036789a
0000000180367856 xor       ecx, ecx
0000000180367858 call      0x18132c2f0
000000018036785D mulss     xmm0, dword ptr [rip + 0x12404b7] # 0x1815a7d1c
0000000180367865 subss     xmm6, xmm0
0000000180367869 movss     dword ptr [rsp + 0x34], xmm6
000000018036786F comiss    xmm7, xmm6
0000000180367872 ja        0x180367894
0000000180367874 jmp       0x18036789a
0000000180367876 xor       ecx, ecx
0000000180367878 call      0x18132c2f0
000000018036787D mulss     xmm0, dword ptr [rip + 0x1240497] # 0x1815a7d1c
0000000180367885 addss     xmm0, xmm6
0000000180367889 movss     dword ptr [rsp + 0x34], xmm0
000000018036788F comiss    xmm0, xmm7
0000000180367892 jbe       0x18036789a
0000000180367894 movss     dword ptr [rsp + 0x34], xmm7
000000018036789A xor       edx, edx
000000018036789C mov       rcx, rdi
000000018036789F call      0x1813044d0
00000001803678A4 movsd     xmm0, qword ptr [rsp + 0x30]
00000001803678AA movsd     qword ptr [rsp + 0x40], xmm0
00000001803678B0 mov       ecx, dword ptr [rsp + 0x38]
00000001803678B4 mov       dword ptr [rsp + 0x48], ecx
00000001803678B8 xor       r9d, r9d
00000001803678BB lea       r8, [rsp + 0x40]
00000001803678C0 mov       rdx, rax
00000001803678C3 mov       rcx, rdi
00000001803678C6 call      0x180367f40
00000001803678CB movaps    xmm6, xmmword ptr [rsp + 0x90]
00000001803678D3 movaps    xmm7, xmmword ptr [rsp + 0x80]
00000001803678DB movaps    xmm8, xmmword ptr [rsp + 0x70]
00000001803678E1 add       rsp, 0xa0
00000001803678E8 pop       rdi
00000001803678E9 pop       rsi
00000001803678EA pop       rbx
00000001803678EB ret       
00000001803678EC call      0x180250150
00000001803678F1 nop       
00000001803678F2 call      0x180250150
00000001803678F7 nop       
00000001803678F8 mov       rcx, rbx
00000001803678FB call      0x18024f3a0
0000000180367900 call      0x180250150
0000000180367905 call      0x180250150
000000018036790A nop       
000000018036790B call      0x180200e60
0000000180367910 int3      