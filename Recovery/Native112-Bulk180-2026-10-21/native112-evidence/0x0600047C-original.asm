0000000180367F40 mov       rax, rsp
0000000180367F43 mov       qword ptr [rax + 8], rbx
0000000180367F47 mov       qword ptr [rax + 0x18], rsi
0000000180367F4B mov       qword ptr [rax + 0x20], rdi
0000000180367F4F push      r14
0000000180367F51 sub       rsp, 0xd0
0000000180367F58 movaps    xmmword ptr [rax - 0x18], xmm6
0000000180367F5C movaps    xmmword ptr [rax - 0x28], xmm7
0000000180367F60 movaps    xmmword ptr [rax - 0x38], xmm8
0000000180367F65 movaps    xmmword ptr [rax - 0x48], xmm9
0000000180367F6A mov       rsi, r8
0000000180367F6D mov       rdi, rdx
0000000180367F70 cmp       byte ptr [rip + 0x197ce11], 0 # 0x181ce4d88
0000000180367F77 jne       0x180367fb0
0000000180367F79 lea       rcx, [rip + 0x183cf70] # 0x181ba4ef0
0000000180367F80 call      0x18024fef0
0000000180367F85 lea       rcx, [rip + 0x183d2c4] # 0x181ba5250
0000000180367F8C call      0x18024fef0
0000000180367F91 lea       rcx, [rip + 0x18576d0] # 0x181bbf668
0000000180367F98 call      0x18024fef0
0000000180367F9D lea       rcx, [rip + 0x184a4ec] # 0x181bb2490
0000000180367FA4 call      0x18024fef0
0000000180367FA9 mov       byte ptr [rip + 0x197cdd8], 1 # 0x181ce4d88
0000000180367FB0 xor       r14d, r14d
0000000180367FB3 mov       qword ptr [rsp + 0x20], r14
0000000180367FB8 test      rdi, rdi
0000000180367FBB je        0x18036826d
0000000180367FC1 xor       edx, edx
0000000180367FC3 mov       rcx, rdi
0000000180367FC6 call      0x18131c070
0000000180367FCB test      rax, rax
0000000180367FCE je        0x18036826d
0000000180367FD4 xor       edx, edx
0000000180367FD6 mov       rcx, rax
0000000180367FD9 call      0x18132dc20
0000000180367FDE mov       qword ptr [rsp + 0xe8], rax
0000000180367FE6 lea       rax, [rsp + 0xe8]
0000000180367FEE mov       qword ptr [rsp + 0x50], rax
0000000180367FF3 lea       rax, [rsp + 0x20]
0000000180367FF8 mov       qword ptr [rsp + 0x58], rax
0000000180367FFD mov       qword ptr [rsp + 0x60], r14
0000000180368002 movups    xmm9, xmmword ptr [rsp + 0x50]
0000000180368008 movups    xmmword ptr [rsp + 0x68], xmm9
000000018036800E nop       
0000000180368010 mov       r8, qword ptr [rsp + 0xe8]
0000000180368018 test      r8, r8
000000018036801B je        0x180368261
0000000180368021 xor       ecx, ecx
0000000180368023 mov       rdx, qword ptr [rip + 0x183d226] # 0x181ba5250
000000018036802A call      0x180002cd0
000000018036802F test      al, al
0000000180368031 je        0x1803681cd
0000000180368037 mov       rbx, qword ptr [rsp + 0xe8]
000000018036803F test      rbx, rbx
0000000180368042 je        0x18036825c
0000000180368048 mov       r9, qword ptr [rip + 0x183d201] # 0x181ba5250
000000018036804F mov       r10, qword ptr [rbx]
0000000180368052 movzx     ecx, r14w
0000000180368056 movzx     edx, word ptr [r10 + 0x12e]
000000018036805E cmp       r14w, dx
0000000180368062 jae       0x180368084
0000000180368064 mov       r8, qword ptr [r10 + 0xb0]
000000018036806B nop       dword ptr [rax + rax]
0000000180368070 movzx     eax, cx
0000000180368073 add       rax, rax
0000000180368076 cmp       qword ptr [r8 + rax*8], r9
000000018036807A je        0x1803680b9
000000018036807C inc       cx
000000018036807F cmp       cx, dx
0000000180368082 jb        0x180368070
0000000180368084 mov       r8d, 1
000000018036808A mov       rdx, r9
000000018036808D mov       rcx, rbx
0000000180368090 call      0x180258380
0000000180368095 mov       rdx, rax
0000000180368098 mov       rax, qword ptr [rdx]
000000018036809B mov       rdx, qword ptr [rdx + 8]
000000018036809F mov       rcx, rbx
00000001803680A2 call      rax
00000001803680A4 mov       rbx, rax
00000001803680A7 mov       rdx, qword ptr [rip + 0x18575ba] # 0x181bbf668
00000001803680AE test      rax, rax
00000001803680B1 je        0x180368249
00000001803680B7 jmp       0x1803680d9
00000001803680B9 movzx     ecx, cx
00000001803680BC add       rcx, rcx
00000001803680BF mov       ecx, dword ptr [r8 + rcx*8 + 8]
00000001803680C4 inc       ecx
00000001803680C6 movsxd    rdx, ecx
00000001803680C9 shl       rdx, 4
00000001803680CD add       rdx, 0x138
00000001803680D4 add       rdx, r10
00000001803680D7 jmp       0x180368098
00000001803680D9 mov       r8, qword ptr [rax]
00000001803680DC movzx     eax, byte ptr [rdx + 0x130]
00000001803680E3 cmp       byte ptr [r8 + 0x130], al
00000001803680EA jb        0x180368254
00000001803680F0 movzx     ecx, al
00000001803680F3 mov       rax, qword ptr [r8 + 0xc8]
00000001803680FA cmp       qword ptr [rax + rcx*8 - 8], rdx
00000001803680FF jne       0x180368254
0000000180368105 xor       edx, edx
0000000180368107 mov       rcx, rbx
000000018036810A call      0x18131f6f0
000000018036810F xor       r8d, r8d
0000000180368112 mov       rdx, qword ptr [rip + 0x184a377] # 0x181bb2490
0000000180368119 mov       rcx, rax
000000018036811C call      0x180b76170
0000000180368121 test      al, al
0000000180368123 je        0x180368010
0000000180368129 xor       r8d, r8d
000000018036812C mov       rdx, rbx
000000018036812F lea       rcx, [rsp + 0x80]
0000000180368137 call      0x18132f3a0
000000018036813C movsd     xmm1, qword ptr [rax]
0000000180368140 movsd     xmm6, qword ptr [rsi]
0000000180368144 movss     xmm8, dword ptr [rsi + 8]
000000018036814A subss     xmm8, dword ptr [rax + 8]
0000000180368150 movaps    xmm7, xmm6
0000000180368153 shufps    xmm7, xmm7, 0x55
0000000180368157 movaps    xmm0, xmm1
000000018036815A shufps    xmm0, xmm0, 0x55
000000018036815E subss     xmm7, xmm0
0000000180368162 subss     xmm6, xmm1
0000000180368166 xor       edx, edx
0000000180368168 mov       rcx, rdi
000000018036816B call      0x18131c070
0000000180368170 mov       rbx, rax
0000000180368173 test      rax, rax
0000000180368176 je        0x18036824f
000000018036817C xor       r8d, r8d
000000018036817F mov       rdx, rax
0000000180368182 lea       rcx, [rsp + 0x50]
0000000180368187 call      0x18132f3a0
000000018036818C movsd     xmm0, qword ptr [rax]
0000000180368190 movss     xmm2, dword ptr [rax + 8]
0000000180368195 addss     xmm2, xmm8
000000018036819A movaps    xmm1, xmm0
000000018036819D shufps    xmm1, xmm1, 0x55
00000001803681A1 addss     xmm1, xmm7
00000001803681A5 addss     xmm6, xmm0
00000001803681A9 unpcklps  xmm6, xmm1
00000001803681AC movsd     qword ptr [rsp + 0x30], xmm6
00000001803681B2 movss     dword ptr [rsp + 0x38], xmm2
00000001803681B8 xor       r8d, r8d
00000001803681BB lea       rdx, [rsp + 0x30]
00000001803681C0 mov       rcx, rbx
00000001803681C3 call      0x18132fbb0
00000001803681C8 jmp       0x180368010
00000001803681CD movq      rax, xmm9
00000001803681D2 mov       rdx, qword ptr [rip + 0x183cd17] # 0x181ba4ef0
00000001803681D9 mov       rcx, qword ptr [rax]
00000001803681DC call      0x18024f380
00000001803681E1 mov       r8, rax
00000001803681E4 psrldq    xmm9, 8
00000001803681EA movq      rax, xmm9
00000001803681EF mov       qword ptr [rax], r8
00000001803681F2 test      r8, r8
00000001803681F5 je        0x180368205
00000001803681F7 xor       ecx, ecx
00000001803681F9 mov       rdx, qword ptr [rip + 0x183ccf0] # 0x181ba4ef0
0000000180368200 call      0x180002d90
0000000180368205 jmp       0x18036821b
0000000180368207 lea       rcx, [rsp + 0x68]
000000018036820C call      0x1802fb000
0000000180368211 mov       rcx, qword ptr [rsp + 0x60]
0000000180368216 test      rcx, rcx
0000000180368219 jne       0x180368267
000000018036821B lea       r11, [rsp + 0xd0]
0000000180368223 mov       rbx, qword ptr [r11 + 0x10]
0000000180368227 mov       rsi, qword ptr [r11 + 0x20]
000000018036822B mov       rdi, qword ptr [r11 + 0x28]
000000018036822F movaps    xmm6, xmmword ptr [r11 - 0x10]
0000000180368234 movaps    xmm7, xmmword ptr [r11 - 0x20]
0000000180368239 movaps    xmm8, xmmword ptr [r11 - 0x30]
000000018036823E movaps    xmm9, xmmword ptr [r11 - 0x40]
0000000180368243 mov       rsp, r11
0000000180368246 pop       r14
0000000180368248 ret       
0000000180368249 call      0x180250150
000000018036824E nop       
000000018036824F call      0x180250150
0000000180368254 mov       rcx, rbx
0000000180368257 call      0x18024f3a0
000000018036825C call      0x180250150
0000000180368261 call      0x180250150
0000000180368266 nop       
0000000180368267 call      0x180200e60
000000018036826C int3      
000000018036826D call      0x180250150
0000000180368272 int3      