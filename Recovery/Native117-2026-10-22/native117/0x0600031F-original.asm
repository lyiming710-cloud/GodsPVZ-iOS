00000001803474E0 mov       rax, rsp
00000001803474E3 mov       qword ptr [rax + 0x10], rbx
00000001803474E7 mov       qword ptr [rax + 8], rcx
00000001803474EB push      rsi
00000001803474EC push      rdi
00000001803474ED push      r14
00000001803474EF sub       rsp, 0xf0
00000001803474F6 movaps    xmmword ptr [rax - 0x28], xmm6
00000001803474FA movaps    xmmword ptr [rax - 0x38], xmm7
00000001803474FE movaps    xmmword ptr [rax - 0x48], xmm8
0000000180347503 movaps    xmmword ptr [rax - 0x58], xmm9
0000000180347508 movaps    xmmword ptr [rax - 0x68], xmm10
000000018034750D movaps    xmmword ptr [rax - 0x78], xmm11
0000000180347512 movaps    xmmword ptr [rax - 0x88], xmm12
000000018034751A mov       rsi, rcx
000000018034751D cmp       byte ptr [rip + 0x199d7a7], 0 # 0x181ce4ccb
0000000180347524 jne       0x1803475b5
000000018034752A lea       rcx, [rip + 0x187b1cf] # 0x181bc2700
0000000180347531 call      0x18024fef0
0000000180347536 lea       rcx, [rip + 0x187f2b3] # 0x181bc67f0
000000018034753D call      0x18024fef0
0000000180347542 lea       rcx, [rip + 0x187b20f] # 0x181bc2758
0000000180347549 call      0x18024fef0
000000018034754E lea       rcx, [rip + 0x187f2f3] # 0x181bc6848
0000000180347555 call      0x18024fef0
000000018034755A lea       rcx, [rip + 0x187f33f] # 0x181bc68a0
0000000180347561 call      0x18024fef0
0000000180347566 lea       rcx, [rip + 0x187b243] # 0x181bc27b0
000000018034756D call      0x18024fef0
0000000180347572 lea       rcx, [rip + 0x1861a97] # 0x181ba9010
0000000180347579 call      0x18024fef0
000000018034757E lea       rcx, [rip + 0x186cd8b] # 0x181bb4310
0000000180347585 call      0x18024fef0
000000018034758A lea       rcx, [rip + 0x1861b2f] # 0x181ba90c0
0000000180347591 call      0x18024fef0
0000000180347596 lea       rcx, [rip + 0x1861a1b] # 0x181ba8fb8
000000018034759D call      0x18024fef0
00000001803475A2 lea       rcx, [rip + 0x1875daf] # 0x181bbd358
00000001803475A9 call      0x18024fef0
00000001803475AE mov       byte ptr [rip + 0x199d716], 1 # 0x181ce4ccb
00000001803475B5 xorps     xmm0, xmm0
00000001803475B8 xor       eax, eax
00000001803475BA movups    xmmword ptr [rsp + 0x38], xmm0
00000001803475BF mov       qword ptr [rsp + 0x48], rax
00000001803475C4 movss     xmm10, dword ptr [rsi + 0x80]
00000001803475CD mulss     xmm10, dword ptr [rip + 0x126057e] # 0x1815a7b54
00000001803475D6 mov       rax, qword ptr [rsi + 0x28]
00000001803475DA test      rax, rax
00000001803475DD je        0x180347a61
00000001803475E3 mov       rax, qword ptr [rax + 0xe8]
00000001803475EA test      rax, rax
00000001803475ED je        0x180347a61
00000001803475F3 mov       rdx, qword ptr [rax + 0x30]
00000001803475F7 test      rdx, rdx
00000001803475FA je        0x180347a61
0000000180347600 mov       r8, qword ptr [rip + 0x186cd09] # 0x181bb4310
0000000180347607 lea       rcx, [rsp + 0x20]
000000018034760C call      0x1808197f0
0000000180347611 movups    xmm0, xmmword ptr [rsp + 0x20]
0000000180347616 movups    xmmword ptr [rsp + 0x50], xmm0
000000018034761B movsd     xmm1, qword ptr [rsp + 0x30]
0000000180347621 movsd     qword ptr [rsp + 0x60], xmm1
0000000180347627 mov       qword ptr [rsp + 0x20], 0
0000000180347630 lea       rbx, [rsp + 0x50]
0000000180347635 mov       qword ptr [rsp + 0x28], rbx
000000018034763A movss     xmm12, dword ptr [rip + 0x1260645] # 0x1815a7c88
0000000180347643 mov       rdx, qword ptr [rip + 0x187f1fe] # 0x181bc6848
000000018034764A lea       rcx, [rsp + 0x50]
000000018034764F call      0x180669280
0000000180347654 test      al, al
0000000180347656 je        0x18034771b
000000018034765C mov       rdi, qword ptr [rsp + 0x60]
0000000180347661 test      rdi, rdi
0000000180347664 je        0x180347a33
000000018034766A movss     xmm6, dword ptr [rdi + 0x30]
000000018034766F movss     xmm7, dword ptr [rdi + 0x34]
0000000180347674 movss     xmm8, dword ptr [rsi + 0x38]
000000018034767A movss     xmm9, dword ptr [rsi + 0x3c]
0000000180347680 cmp       byte ptr [rip + 0x199d6b5], 0 # 0x181ce4d3c
0000000180347687 jne       0x18034769c
0000000180347689 lea       rcx, [rip + 0x1865568] # 0x181bacbf8
0000000180347690 call      0x18024fef0
0000000180347695 mov       byte ptr [rip + 0x199d6a0], 1 # 0x181ce4d3c
000000018034769C subss     xmm6, xmm8
00000001803476A1 subss     xmm7, xmm9
00000001803476A6 mov       rcx, qword ptr [rip + 0x186554b] # 0x181bacbf8
00000001803476AD cmp       dword ptr [rcx + 0xe0], 0
00000001803476B4 jne       0x1803476bb
00000001803476B6 call      0x1802501e0
00000001803476BB mulss     xmm7, xmm7
00000001803476BF mulss     xmm6, xmm6
00000001803476C3 addss     xmm6, xmm7
00000001803476C7 cvtps2pd  xmm1, xmm6
00000001803476CA xorps     xmm0, xmm0
00000001803476CD ucomisd   xmm0, xmm1
00000001803476D1 ja        0x1803476d9
00000001803476D3 sqrtpd    xmm0, xmm1
00000001803476D7 jmp       0x1803476e1
00000001803476D9 movaps    xmm0, xmm1
00000001803476DC call      0x1802cb7b4
00000001803476E1 cvtsd2ss  xmm0, xmm0
00000001803476E5 comiss    xmm0, xmm12
00000001803476E9 ja        0x180347643
00000001803476EF test      rdi, rdi
00000001803476F2 je        0x180347a2e
00000001803476F8 mov       rcx, qword ptr [rdi + 0xf8]
00000001803476FF test      rcx, rcx
0000000180347702 je        0x180347a28
0000000180347708 xor       r9d, r9d
000000018034770B movaps    xmm2, xmm10
000000018034770F xor       edx, edx
0000000180347711 call      0x180314a00
0000000180347716 jmp       0x180347643
000000018034771B mov       rdx, qword ptr [rip + 0x187f0ce] # 0x181bc67f0
0000000180347722 mov       rcx, rbx
0000000180347725 call      0x180302170
000000018034772A jmp       0x18034775c
000000018034772C mov       rdx, qword ptr [rip + 0x187f0bd] # 0x181bc67f0
0000000180347733 mov       rcx, qword ptr [rsp + 0x28]
0000000180347738 call      0x180302170
000000018034773D mov       rcx, qword ptr [rsp + 0x20]
0000000180347742 test      rcx, rcx
0000000180347745 jne       0x180347a39
000000018034774B mov       rsi, qword ptr [rsp + 0x110]
0000000180347753 movss     xmm12, dword ptr [rip + 0x126052c] # 0x1815a7c88
000000018034775C movss     xmm10, dword ptr [rsi + 0x80]
0000000180347765 mulss     xmm10, dword ptr [rip + 0x1260552] # 0x1815a7cc0
000000018034776E movss     dword ptr [rsp + 0x110], xmm10
0000000180347778 mov       rcx, qword ptr [rip + 0x1875bd9] # 0x181bbd358
000000018034777F call      0x180250100
0000000180347784 mov       r14, rax
0000000180347787 mov       rdx, qword ptr [rip + 0x186182a] # 0x181ba8fb8
000000018034778E mov       rcx, rax
0000000180347791 call      0x1808053d0
0000000180347796 mov       qword ptr [rsp + 0x120], r14
000000018034779E mov       rax, qword ptr [rsi + 0x28]
00000001803477A2 test      rax, rax
00000001803477A5 je        0x180347a61
00000001803477AB mov       rax, qword ptr [rax + 0x108]
00000001803477B2 test      rax, rax
00000001803477B5 je        0x180347a61
00000001803477BB mov       rdx, qword ptr [rax + 0x30]
00000001803477BF test      rdx, rdx
00000001803477C2 je        0x180347a61
00000001803477C8 mov       r8, qword ptr [rip + 0x18618f1] # 0x181ba90c0
00000001803477CF lea       rcx, [rsp + 0x20]
00000001803477D4 call      0x1808197f0
00000001803477D9 movups    xmm0, xmmword ptr [rsp + 0x20]
00000001803477DE movups    xmmword ptr [rsp + 0x38], xmm0
00000001803477E3 movsd     xmm1, qword ptr [rsp + 0x30]
00000001803477E9 movsd     qword ptr [rsp + 0x48], xmm1
00000001803477EF mov       qword ptr [rsp + 0x20], 0
00000001803477F8 lea       rbx, [rsp + 0x38]
00000001803477FD mov       qword ptr [rsp + 0x28], rbx
0000000180347802 xorps     xmm11, xmm11
0000000180347806 nop       word ptr [rax + rax]
0000000180347810 mov       rdx, qword ptr [rip + 0x187af41] # 0x181bc2758
0000000180347817 lea       rcx, [rsp + 0x38]
000000018034781C call      0x180669280
0000000180347821 test      al, al
0000000180347823 je        0x1803478f3
0000000180347829 mov       rdi, qword ptr [rsp + 0x48]
000000018034782E test      rdi, rdi
0000000180347831 je        0x180347a49
0000000180347837 movss     xmm6, dword ptr [rdi + 0x38]
000000018034783C movss     xmm7, dword ptr [rdi + 0x3c]
0000000180347841 movss     xmm8, dword ptr [rsi + 0x38]
0000000180347847 movss     xmm9, dword ptr [rsi + 0x3c]
000000018034784D cmp       byte ptr [rip + 0x199d4e8], 0 # 0x181ce4d3c
0000000180347854 jne       0x180347869
0000000180347856 lea       rcx, [rip + 0x186539b] # 0x181bacbf8
000000018034785D call      0x18024fef0
0000000180347862 mov       byte ptr [rip + 0x199d4d3], 1 # 0x181ce4d3c
0000000180347869 subss     xmm6, xmm8
000000018034786E subss     xmm7, xmm9
0000000180347873 mov       rcx, qword ptr [rip + 0x186537e] # 0x181bacbf8
000000018034787A cmp       dword ptr [rcx + 0xe0], 0
0000000180347881 jne       0x180347888
0000000180347883 call      0x1802501e0
0000000180347888 mulss     xmm7, xmm7
000000018034788C mulss     xmm6, xmm6
0000000180347890 addss     xmm6, xmm7
0000000180347894 cvtps2pd  xmm1, xmm6
0000000180347897 xorps     xmm0, xmm0
000000018034789A ucomisd   xmm0, xmm1
000000018034789E ja        0x1803478a6
00000001803478A0 sqrtpd    xmm0, xmm1
00000001803478A4 jmp       0x1803478ae
00000001803478A6 movaps    xmm0, xmm1
00000001803478A9 call      0x1802cb7b4
00000001803478AE cvtsd2ss  xmm0, xmm0
00000001803478B2 comiss    xmm0, xmm12
00000001803478B6 ja        0x180347810
00000001803478BC test      rdi, rdi
00000001803478BF je        0x180347a44
00000001803478C5 comiss    xmm11, dword ptr [rdi + 0x88]
00000001803478CD jae       0x180347810
00000001803478D3 test      r14, r14
00000001803478D6 je        0x180347a3f
00000001803478DC mov       r8, qword ptr [rip + 0x186172d] # 0x181ba9010
00000001803478E3 mov       rdx, rdi
00000001803478E6 mov       rcx, r14
00000001803478E9 call      0x180002e70
00000001803478EE jmp       0x180347810
00000001803478F3 mov       rdx, qword ptr [rip + 0x187ae06] # 0x181bc2700
00000001803478FA mov       rcx, rbx
00000001803478FD call      0x180302170
0000000180347902 jmp       0x180347935
0000000180347904 mov       rdx, qword ptr [rip + 0x187adf5] # 0x181bc2700
000000018034790B mov       rcx, qword ptr [rsp + 0x28]
0000000180347910 call      0x180302170
0000000180347915 mov       rcx, qword ptr [rsp + 0x20]
000000018034791A test      rcx, rcx
000000018034791D jne       0x180347a4f
0000000180347923 movss     xmm10, dword ptr [rsp + 0x110]
000000018034792D mov       r14, qword ptr [rsp + 0x120]
0000000180347935 test      r14, r14
0000000180347938 je        0x180347a61
000000018034793E mov       r8, qword ptr [rip + 0x186177b] # 0x181ba90c0
0000000180347945 mov       rdx, r14
0000000180347948 lea       rcx, [rsp + 0x20]
000000018034794D call      0x1808197f0
0000000180347952 movups    xmm0, xmmword ptr [rsp + 0x20]
0000000180347957 movups    xmmword ptr [rsp + 0x38], xmm0
000000018034795C movsd     xmm1, qword ptr [rsp + 0x30]
0000000180347962 movsd     qword ptr [rsp + 0x48], xmm1
0000000180347968 mov       qword ptr [rsp + 0x20], 0
0000000180347971 lea       rbx, [rsp + 0x38]
0000000180347976 mov       qword ptr [rsp + 0x28], rbx
000000018034797B nop       dword ptr [rax + rax]
0000000180347980 mov       rdx, qword ptr [rip + 0x187add1] # 0x181bc2758
0000000180347987 lea       rcx, [rsp + 0x38]
000000018034798C call      0x180669280
0000000180347991 test      al, al
0000000180347993 je        0x1803479c5
0000000180347995 mov       rdi, qword ptr [rsp + 0x48]
000000018034799A test      rdi, rdi
000000018034799D je        0x180347a55
00000001803479A3 movss     xmm6, dword ptr [rdi + 0x88]
00000001803479AB xor       ecx, ecx
00000001803479AD call      0x18132c2f0
00000001803479B2 mulss     xmm0, xmm10
00000001803479B7 addss     xmm0, xmm6
00000001803479BB movss     dword ptr [rdi + 0x88], xmm0
00000001803479C3 jmp       0x180347980
00000001803479C5 mov       rdx, qword ptr [rip + 0x187ad34] # 0x181bc2700
00000001803479CC mov       rcx, rbx
00000001803479CF call      0x180302170
00000001803479D4 jmp       0x1803479f1
00000001803479D6 mov       rdx, qword ptr [rip + 0x187ad23] # 0x181bc2700
00000001803479DD mov       rcx, qword ptr [rsp + 0x28]
00000001803479E2 call      0x180302170
00000001803479E7 mov       rcx, qword ptr [rsp + 0x20]
00000001803479EC test      rcx, rcx
00000001803479EF jne       0x180347a5b
00000001803479F1 lea       r11, [rsp + 0xf0]
00000001803479F9 mov       rbx, qword ptr [r11 + 0x28]
00000001803479FD movaps    xmm6, xmmword ptr [r11 - 0x10]
0000000180347A02 movaps    xmm7, xmmword ptr [r11 - 0x20]
0000000180347A07 movaps    xmm8, xmmword ptr [r11 - 0x30]
0000000180347A0C movaps    xmm9, xmmword ptr [r11 - 0x40]
0000000180347A11 movaps    xmm10, xmmword ptr [r11 - 0x50]
0000000180347A16 movaps    xmm11, xmmword ptr [r11 - 0x60]
0000000180347A1B movaps    xmm12, xmmword ptr [r11 - 0x70]
0000000180347A20 mov       rsp, r11
0000000180347A23 pop       r14
0000000180347A25 pop       rdi
0000000180347A26 pop       rsi
0000000180347A27 ret       
0000000180347A28 call      0x180250150
0000000180347A2D nop       
0000000180347A2E call      0x180250150
0000000180347A33 call      0x180250150
0000000180347A38 nop       
0000000180347A39 call      0x180200e60
0000000180347A3E nop       
0000000180347A3F call      0x180250150
0000000180347A44 call      0x180250150
0000000180347A49 call      0x180250150
0000000180347A4E nop       
0000000180347A4F call      0x180200e60
0000000180347A54 nop       
0000000180347A55 call      0x180250150
0000000180347A5A nop       
0000000180347A5B call      0x180200e60
0000000180347A60 int3      
0000000180347A61 call      0x180250150
0000000180347A66 int3      
0000000180347A67 int3      
0000000180347A68 int3      
0000000180347A69 int3      
0000000180347A6A int3      
0000000180347A6B int3      
0000000180347A6C int3      
0000000180347A6D int3      
0000000180347A6E int3      
0000000180347A6F int3      