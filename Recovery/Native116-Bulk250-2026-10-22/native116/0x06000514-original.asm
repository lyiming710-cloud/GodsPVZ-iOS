0000000180381350 mov       qword ptr [rsp + 8], rbx
0000000180381355 mov       qword ptr [rsp + 0x10], rsi
000000018038135A push      rdi
000000018038135B sub       rsp, 0xa0
0000000180381362 movaps    xmmword ptr [rsp + 0x90], xmm6
000000018038136A mov       rbx, rcx
000000018038136D cmp       byte ptr [rip + 0x1963a93], 0 # 0x181ce4e07
0000000180381374 jne       0x1803813d1
0000000180381376 lea       rcx, [rip + 0x1836d33] # 0x181bb80b0
000000018038137D call      0x18024fef0
0000000180381382 lea       rcx, [rip + 0x18494df] # 0x181bca868
0000000180381389 call      0x18024fef0
000000018038138E lea       rcx, [rip + 0x1849523] # 0x181bca8b8
0000000180381395 call      0x18024fef0
000000018038139A lea       rcx, [rip + 0x184956f] # 0x181bca910
00000001803813A1 call      0x18024fef0
00000001803813A6 lea       rcx, [rip + 0x1823b0b] # 0x181ba4eb8
00000001803813AD call      0x18024fef0
00000001803813B2 lea       rcx, [rip + 0x184232f] # 0x181bc36e8
00000001803813B9 call      0x18024fef0
00000001803813BE lea       rcx, [rip + 0x182f13b] # 0x181bb0500
00000001803813C5 call      0x18024fef0
00000001803813CA mov       byte ptr [rip + 0x1963a36], 1 # 0x181ce4e07
00000001803813D1 test      rbx, rbx
00000001803813D4 je        0x180381661
00000001803813DA mov       byte ptr [rbx + 0xb6], 1
00000001803813E1 mov       byte ptr [rbx + 0xf4], 1
00000001803813E8 xor       edx, edx
00000001803813EA mov       rcx, rbx
00000001803813ED call      0x180355d50
00000001803813F2 cmp       byte ptr [rip + 0x196373b], 0 # 0x181ce4b34
00000001803813F9 jne       0x18038140e
00000001803813FB lea       rcx, [rip + 0x183048e] # 0x181bb1890
0000000180381402 call      0x18024fef0
0000000180381407 mov       byte ptr [rip + 0x1963726], 1 # 0x181ce4b34
000000018038140E mov       rax, qword ptr [rip + 0x183047b] # 0x181bb1890
0000000180381415 mov       rcx, qword ptr [rax + 0xb8]
000000018038141C mov       rcx, qword ptr [rcx]
000000018038141F test      rcx, rcx
0000000180381422 je        0x180381661
0000000180381428 xor       r8d, r8d
000000018038142B mov       edx, 0x24
0000000180381430 call      0x180321f30
0000000180381435 mov       rdi, rax
0000000180381438 mov       rcx, qword ptr [rip + 0x182f0c1] # 0x181bb0500
000000018038143F cmp       dword ptr [rcx + 0xe0], 0
0000000180381446 jne       0x18038144d
0000000180381448 call      0x1802501e0
000000018038144D xor       r8d, r8d
0000000180381450 xor       edx, edx
0000000180381452 mov       rcx, rdi
0000000180381455 call      0x18131f760
000000018038145A test      al, al
000000018038145C jne       0x18038163b
0000000180381462 test      rdi, rdi
0000000180381465 je        0x180381661
000000018038146B xor       edx, edx
000000018038146D mov       rcx, rdi
0000000180381470 call      0x18131c070
0000000180381475 test      rax, rax
0000000180381478 je        0x180381661
000000018038147E movsd     xmm0, qword ptr [rbx + 0x138]
0000000180381486 movsd     qword ptr [rsp + 0x40], xmm0
000000018038148C mov       ecx, dword ptr [rbx + 0x140]
0000000180381492 mov       dword ptr [rsp + 0x48], ecx
0000000180381496 xor       r8d, r8d
0000000180381499 lea       rdx, [rsp + 0x40]
000000018038149E mov       rcx, rax
00000001803814A1 call      0x18132fbb0
00000001803814A6 mov       rdx, qword ptr [rip + 0x1823a0b] # 0x181ba4eb8
00000001803814AD mov       rcx, rdi
00000001803814B0 call      0x18046dc80
00000001803814B5 test      rax, rax
00000001803814B8 je        0x180381661
00000001803814BE mov       qword ptr [rax + 0x20], rbx
00000001803814C2 lea       rcx, [rax + 0x20]
00000001803814C6 mov       rdx, rbx
00000001803814C9 call      0x18024f360
00000001803814CE xor       edx, edx
00000001803814D0 mov       rcx, rbx
00000001803814D3 call      0x181304510
00000001803814D8 mov       rdi, rax
00000001803814DB movsd     xmm0, qword ptr [rbx + 0x138]
00000001803814E3 movsd     qword ptr [rsp + 0x40], xmm0
00000001803814E9 mov       esi, dword ptr [rbx + 0x140]
00000001803814EF xor       edx, edx
00000001803814F1 mov       rcx, rbx
00000001803814F4 call      0x181304510
00000001803814F9 test      rax, rax
00000001803814FC je        0x180381661
0000000180381502 xor       r8d, r8d
0000000180381505 mov       rdx, rax
0000000180381508 lea       rcx, [rsp + 0x50]
000000018038150D call      0x18132f3a0
0000000180381512 movsd     xmm2, qword ptr [rax]
0000000180381516 movd      xmm4, esi
000000018038151A subss     xmm4, dword ptr [rax + 8]
000000018038151F movss     xmm3, dword ptr [rsp + 0x44]
0000000180381525 movaps    xmm0, xmm2
0000000180381528 shufps    xmm0, xmm0, 0x55
000000018038152C subss     xmm3, xmm0
0000000180381530 movss     xmm1, dword ptr [rsp + 0x40]
0000000180381536 movsd     qword ptr [rsp + 0x50], xmm2
000000018038153C subss     xmm1, xmm2
0000000180381540 mov       rax, qword ptr [rip + 0x1836b69] # 0x181bb80b0
0000000180381547 unpcklps  xmm1, xmm3
000000018038154A movsd     qword ptr [rsp + 0x40], xmm1
0000000180381550 movss     dword ptr [rsp + 0x48], xmm4
0000000180381556 mov       qword ptr [rsp + 0x20], rax
000000018038155B lea       r9, [rsp + 0x40]
0000000180381560 movss     xmm2, dword ptr [rip + 0x1226724] # 0x1815a7c8c
0000000180381568 mov       rdx, rdi
000000018038156B mov       rcx, rbx
000000018038156E call      0x180426070
0000000180381573 xor       r9d, r9d
0000000180381576 mov       r8, rax
0000000180381579 mov       edx, 0x3e7
000000018038157E mov       rcx, rbx
0000000180381581 call      0x18034fbe0
0000000180381586 test      rax, rax
0000000180381589 je        0x180381661
000000018038158F mov       r8, qword ptr [rip + 0x1842152] # 0x181bc36e8
0000000180381596 mov       rdx, rax
0000000180381599 lea       rcx, [rsp + 0x50]
000000018038159E call      0x1808197f0
00000001803815A3 movups    xmm0, xmmword ptr [rsp + 0x50]
00000001803815A8 movups    xmmword ptr [rsp + 0x70], xmm0
00000001803815AD movsd     xmm1, qword ptr [rsp + 0x60]
00000001803815B3 movsd     qword ptr [rsp + 0x80], xmm1
00000001803815BC mov       qword ptr [rsp + 0x50], 0
00000001803815C5 lea       rbx, [rsp + 0x70]
00000001803815CA mov       qword ptr [rsp + 0x58], rbx
00000001803815CF movss     xmm6, dword ptr [rip + 0x122644d] # 0x1815a7a24
00000001803815D7 nop       word ptr [rax + rax]
00000001803815E0 mov       rdx, qword ptr [rip + 0x18492d1] # 0x181bca8b8
00000001803815E7 lea       rcx, [rsp + 0x70]
00000001803815EC call      0x180669280
00000001803815F1 test      al, al
00000001803815F3 je        0x18038160f
00000001803815F5 mov       rcx, qword ptr [rsp + 0x80]
00000001803815FD test      rcx, rcx
0000000180381600 je        0x180381655
0000000180381602 xor       r8d, r8d
0000000180381605 movaps    xmm1, xmm6
0000000180381608 call      0x180369a70
000000018038160D jmp       0x1803815e0
000000018038160F mov       rdx, qword ptr [rip + 0x1849252] # 0x181bca868
0000000180381616 mov       rcx, rbx
0000000180381619 call      0x180302170
000000018038161E jmp       0x18038163b
0000000180381620 mov       rdx, qword ptr [rip + 0x1849241] # 0x181bca868
0000000180381627 mov       rcx, qword ptr [rsp + 0x58]
000000018038162C call      0x180302170
0000000180381631 mov       rcx, qword ptr [rsp + 0x50]
0000000180381636 test      rcx, rcx
0000000180381639 jne       0x18038165b
000000018038163B lea       r11, [rsp + 0xa0]
0000000180381643 mov       rbx, qword ptr [r11 + 0x10]
0000000180381647 mov       rsi, qword ptr [r11 + 0x18]
000000018038164B movaps    xmm6, xmmword ptr [r11 - 0x10]
0000000180381650 mov       rsp, r11
0000000180381653 pop       rdi
0000000180381654 ret       
0000000180381655 call      0x180250150
000000018038165A nop       
000000018038165B call      0x180200e60
0000000180381660 int3      
0000000180381661 call      0x180250150
0000000180381666 int3      
0000000180381667 int3      
0000000180381668 int3      
0000000180381669 int3      
000000018038166A int3      
000000018038166B int3      
000000018038166C int3      
000000018038166D int3      
000000018038166E int3      
000000018038166F int3      