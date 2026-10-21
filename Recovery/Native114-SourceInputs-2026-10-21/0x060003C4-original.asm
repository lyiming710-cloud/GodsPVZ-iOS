00000001803763E0 push      rbx
00000001803763E2 sub       rsp, 0x80
00000001803763E9 cmp       byte ptr [rip + 0x196e9cd], 0 # 0x181ce4dbd
00000001803763F0 mov       rbx, rcx
00000001803763F3 jne       0x18037642c
00000001803763F5 lea       rcx, [rip + 0x182edbc] # 0x181ba51b8
00000001803763FC call      0x18024fef0
0000000180376401 lea       rcx, [rip + 0x1835c28] # 0x181bac030
0000000180376408 call      0x18024fef0
000000018037640D lea       rcx, [rip + 0x1835c74] # 0x181bac088
0000000180376414 call      0x18024fef0
0000000180376419 lea       rcx, [rip + 0x183a0e0] # 0x181bb0500
0000000180376420 call      0x18024fef0
0000000180376425 mov       byte ptr [rip + 0x196e991], 1 # 0x181ce4dbd
000000018037642C mov       qword ptr [rsp + 0x90], rsi
0000000180376434 xor       edx, edx
0000000180376436 mov       qword ptr [rsp + 0x98], rdi
000000018037643E mov       rcx, rbx
0000000180376441 movaps    xmmword ptr [rsp + 0x70], xmm6
0000000180376446 movaps    xmmword ptr [rsp + 0x60], xmm7
000000018037644B call      0x181304510
0000000180376450 test      rax, rax
0000000180376453 je        0x1803767a7
0000000180376459 xor       r8d, r8d
000000018037645C movaps    xmmword ptr [rsp + 0x50], xmm8
0000000180376462 mov       rdx, rax
0000000180376465 lea       rcx, [rsp + 0x40]
000000018037646A call      0x18132f3a0
000000018037646F cmp       byte ptr [rip + 0x196e6bf], 0 # 0x181ce4b35
0000000180376476 movsd     xmm0, qword ptr [rbx + 0x5c]
000000018037647B mov       edi, dword ptr [rbx + 0x64]
000000018037647E mov       esi, dword ptr [rax + 8]
0000000180376481 movsd     qword ptr [rsp + 0x30], xmm0
0000000180376487 movsd     xmm0, qword ptr [rax]
000000018037648B movsd     qword ptr [rsp + 0x20], xmm0
0000000180376491 jne       0x1803764a6
0000000180376493 lea       rcx, [rip + 0x183675e] # 0x181bacbf8
000000018037649A call      0x18024fef0
000000018037649F mov       byte ptr [rip + 0x196e68f], 1 # 0x181ce4b35
00000001803764A6 mov       rcx, qword ptr [rip + 0x183674b] # 0x181bacbf8
00000001803764AD movss     xmm7, dword ptr [rsp + 0x20]
00000001803764B3 movss     xmm6, dword ptr [rsp + 0x24]
00000001803764B9 subss     xmm7, dword ptr [rsp + 0x30]
00000001803764BF cmp       dword ptr [rcx + 0xe0], 0
00000001803764C6 subss     xmm6, dword ptr [rsp + 0x34]
00000001803764CC movd      xmm8, esi
00000001803764D1 movd      xmm0, edi
00000001803764D5 subss     xmm8, xmm0
00000001803764DA jne       0x1803764e1
00000001803764DC call      0x1802501e0
00000001803764E1 mulss     xmm6, xmm6
00000001803764E5 xorps     xmm0, xmm0
00000001803764E8 mulss     xmm8, xmm8
00000001803764ED mulss     xmm7, xmm7
00000001803764F1 addss     xmm6, xmm7
00000001803764F5 addss     xmm6, xmm8
00000001803764FA movaps    xmm8, xmmword ptr [rsp + 0x50]
0000000180376500 cvtps2pd  xmm1, xmm6
0000000180376503 ucomisd   xmm0, xmm1
0000000180376507 ja        0x18037650f
0000000180376509 sqrtpd    xmm0, xmm1
000000018037650D jmp       0x180376517
000000018037650F movaps    xmm0, xmm1
0000000180376512 call      0x1802cb7b4
0000000180376517 xorps     xmm6, xmm6
000000018037651A cvtsd2ss  xmm6, xmm0
000000018037651E movss     xmm0, dword ptr [rip + 0x12314ea] # 0x1815a7a10
0000000180376526 comiss    xmm0, xmm6
0000000180376529 jae       0x1803766b7
000000018037652F xor       edi, edi
0000000180376531 cmp       dword ptr [rbx + 0x24], 1
0000000180376535 mov       eax, edi
0000000180376537 mov       ecx, edi
0000000180376539 sete      cl
000000018037653C cmp       dword ptr [rbx + 0x24], eax
000000018037653F sete      al
0000000180376542 or        ecx, eax
0000000180376544 jne       0x18037655c
0000000180376546 cmp       dword ptr [rbx + 0x24], 2
000000018037654A je        0x18037655c
000000018037654C cmp       dword ptr [rbx + 0x24], 3
0000000180376550 je        0x18037655c
0000000180376552 cmp       dword ptr [rbx + 0x24], 8
0000000180376556 jne       0x180376727
000000018037655C movss     xmm7, dword ptr [rip + 0x12316f4] # 0x1815a7c58
0000000180376564 comiss    xmm7, xmm6
0000000180376567 jb        0x180376727
000000018037656D xor       edx, edx
000000018037656F mov       rcx, rbx
0000000180376572 call      0x181304510
0000000180376577 cmp       byte ptr [rip + 0x196e89c], dil # 0x181ce4e1a
000000018037657E mov       rsi, rax
0000000180376581 jne       0x180376596
0000000180376583 lea       rcx, [rip + 0x184c25e] # 0x181bc27e8
000000018037658A call      0x18024fef0
000000018037658F mov       byte ptr [rip + 0x196e884], 1 # 0x181ce4e1a
0000000180376596 movss     xmm1, dword ptr [rbx + 0x2c]
000000018037659B mov       rcx, qword ptr [rip + 0x184c246] # 0x181bc27e8
00000001803765A2 mov       rdx, qword ptr [rcx + 0xb8]
00000001803765A9 movsd     xmm0, qword ptr [rdx + 0xc]
00000001803765AE movss     xmm4, dword ptr [rdx + 0x14]
00000001803765B3 movaps    xmm2, xmm0
00000001803765B6 shufps    xmm2, xmm2, 0x55
00000001803765BA movaps    xmm3, xmm0
00000001803765BD mulss     xmm2, xmm1
00000001803765C1 mulss     xmm3, xmm1
00000001803765C5 movsd     qword ptr [rsp + 0x40], xmm0
00000001803765CB mulss     xmm4, xmm1
00000001803765CF mulss     xmm3, xmm6
00000001803765D3 mulss     xmm2, xmm6
00000001803765D7 mulss     xmm4, xmm6
00000001803765DB divss     xmm3, xmm7
00000001803765DF divss     xmm2, xmm7
00000001803765E3 divss     xmm4, xmm7
00000001803765E7 test      rsi, rsi
00000001803765EA je        0x1803767a7
00000001803765F0 unpcklps  xmm3, xmm2
00000001803765F3 lea       rdx, [rsp + 0x30]
00000001803765F8 xor       r8d, r8d
00000001803765FB movsd     qword ptr [rsp + 0x30], xmm3
0000000180376601 mov       rcx, rsi
0000000180376604 movss     dword ptr [rsp + 0x38], xmm4
000000018037660A call      0x18132fa50
000000018037660F mov       rax, qword ptr [rbx + 0x98]
0000000180376616 mov       ecx, edi
0000000180376618 test      rax, rax
000000018037661B je        0x1803767a7
0000000180376621 cmp       ecx, dword ptr [rax + 0x18]
0000000180376624 jge       0x180376727
000000018037662A mov       rcx, qword ptr [rbx + 0x98]
0000000180376631 test      rcx, rcx
0000000180376634 je        0x1803767a7
000000018037663A mov       r8, qword ptr [rip + 0x1835a47] # 0x181bac088
0000000180376641 mov       edx, edi
0000000180376643 call      0x180825710
0000000180376648 test      rax, rax
000000018037664B je        0x1803767a7
0000000180376651 mov       rdx, qword ptr [rip + 0x182eb60] # 0x181ba51b8
0000000180376658 mov       rcx, rax
000000018037665B call      0x18046dc80
0000000180376660 movaps    xmm0, xmm6
0000000180376663 divss     xmm0, xmm7
0000000180376667 test      rax, rax
000000018037666A je        0x1803767a7
0000000180376670 xor       r8d, r8d
0000000180376673 movss     dword ptr [rsp + 0x4c], xmm0
0000000180376679 lea       rdx, [rsp + 0x40]
000000018037667E mov       dword ptr [rsp + 0x40], 0x3f800000
0000000180376686 mov       rcx, rax
0000000180376689 mov       dword ptr [rsp + 0x44], 0x3f800000
0000000180376691 mov       dword ptr [rsp + 0x48], 0x3f800000
0000000180376699 call      0x1813290a0
000000018037669E mov       rax, qword ptr [rbx + 0x98]
00000001803766A5 inc       edi
00000001803766A7 mov       ecx, edi
00000001803766A9 test      rax, rax
00000001803766AC je        0x1803767a7
00000001803766B2 jmp       0x180376621
00000001803766B7 movsxd    rax, dword ptr [rbx + 0x20]
00000001803766BB cmp       eax, 8
00000001803766BE ja        0x180376727
00000001803766C0 lea       rdx, [rip - 0x3766c7] # 0x180000000
00000001803766C7 mov       ecx, dword ptr [rdx + rax*4 + 0x3767b0]
00000001803766CE add       rcx, rdx
00000001803766D1 jmp       rcx
00000001803766D3 mov       rax, qword ptr [rbx + 0x48]
00000001803766D7 test      rax, rax
00000001803766DA je        0x1803767a7
00000001803766E0 mov       rcx, qword ptr [rax + 0x120]
00000001803766E7 test      rcx, rcx
00000001803766EA je        0x1803767a7
00000001803766F0 xor       r8d, r8d
00000001803766F3 mov       rdx, rbx
00000001803766F6 call      0x180340860
00000001803766FB xor       edx, edx
00000001803766FD mov       rcx, rbx
0000000180376700 call      0x1813044d0
0000000180376705 mov       rcx, qword ptr [rip + 0x1839df4] # 0x181bb0500
000000018037670C mov       rbx, rax
000000018037670F cmp       dword ptr [rcx + 0xe0], 0
0000000180376716 jne       0x18037671d
0000000180376718 call      0x1802501e0
000000018037671D xor       edx, edx
000000018037671F mov       rcx, rbx
0000000180376722 call      0x18131e940
0000000180376727 movaps    xmm7, xmmword ptr [rsp + 0x60]
000000018037672C movaps    xmm6, xmmword ptr [rsp + 0x70]
0000000180376731 mov       rdi, qword ptr [rsp + 0x98]
0000000180376739 mov       rsi, qword ptr [rsp + 0x90]
0000000180376741 add       rsp, 0x80
0000000180376748 pop       rbx
0000000180376749 ret       
000000018037674A mov       rax, qword ptr [rbx + 0x48]
000000018037674E test      rax, rax
0000000180376751 je        0x1803767a7
0000000180376753 mov       rcx, qword ptr [rax + 0x100]
000000018037675A test      rcx, rcx
000000018037675D je        0x1803767a7
000000018037675F xor       r8d, r8d
0000000180376762 mov       rdx, rbx
0000000180376765 call      0x180322ef0
000000018037676A jmp       0x1803766fb
000000018037676C cmp       byte ptr [rip + 0x196e3c3], 0 # 0x181ce4b36
0000000180376773 jne       0x180376788
0000000180376775 lea       rcx, [rip + 0x184c06c] # 0x181bc27e8
000000018037677C call      0x18024fef0
0000000180376781 mov       byte ptr [rip + 0x196e3ae], 1 # 0x181ce4b36
0000000180376788 mov       rax, qword ptr [rip + 0x184c059] # 0x181bc27e8
000000018037678F mov       rcx, qword ptr [rax + 0xb8]
0000000180376796 movsd     xmm0, qword ptr [rcx]
000000018037679A mov       eax, dword ptr [rcx + 8]
000000018037679D movsd     qword ptr [rbx + 0x68], xmm0
00000001803767A2 mov       dword ptr [rbx + 0x70], eax
00000001803767A5 jmp       0x180376727
00000001803767A7 call      0x180250150
00000001803767AC int3      
00000001803767AD nop       dword ptr [rax]
00000001803767B0 shl       dword ptr [rsi + 0x37], cl
00000001803767B3 add       byte ptr [rdx + 0x67], cl