00000001803CE450 mov       qword ptr [rsp + 0x10], rsi
00000001803CE455 mov       qword ptr [rsp + 0x18], rdi
00000001803CE45A push      rbp
00000001803CE45B lea       rbp, [rsp - 0x57]
00000001803CE460 sub       rsp, 0xc0
00000001803CE467 cmp       byte ptr [rip + 0x1916b4c], 0 # 0x181ce4fba
00000001803CE46E mov       rsi, rcx
00000001803CE471 jne       0x1803ce492
00000001803CE473 lea       rcx, [rip + 0x18007ee] # 0x181bcec68
00000001803CE47A call      0x18024fef0
00000001803CE47F lea       rcx, [rip + 0x17fcdd2] # 0x181bcb258
00000001803CE486 call      0x18024fef0
00000001803CE48B mov       byte ptr [rip + 0x1916b28], 1 # 0x181ce4fba
00000001803CE492 mov       rcx, qword ptr [rip + 0x17fcdbf] # 0x181bcb258
00000001803CE499 mov       edx, 5
00000001803CE49E call      0x18024f3e0
00000001803CE4A3 mov       rdi, rax
00000001803CE4A6 lea       rcx, [rbp - 0x49]
00000001803CE4AA xor       eax, eax
00000001803CE4AC xorps     xmm0, xmm0
00000001803CE4AF xor       r9d, r9d
00000001803CE4B2 mov       qword ptr [rbp - 0x39], rax
00000001803CE4B6 xorps     xmm2, xmm2
00000001803CE4B9 mov       dword ptr [rbp - 0x31], eax
00000001803CE4BC xorps     xmm1, xmm1
00000001803CE4BF movups    xmmword ptr [rbp - 0x49], xmm0
00000001803CE4C3 call      0x1812ed750
00000001803CE4C8 test      rdi, rdi
00000001803CE4CB je        0x1803ce6a1
00000001803CE4D1 cmp       dword ptr [rdi + 0x18], 0
00000001803CE4D5 jbe       0x1803ce6a7
00000001803CE4DB movups    xmm0, xmmword ptr [rbp - 0x49]
00000001803CE4DF lea       rcx, [rbp - 0x29]
00000001803CE4E3 xor       r9d, r9d
00000001803CE4E6 movss     xmm2, dword ptr [rip + 0x11d9532] # 0x1815a7a20
00000001803CE4EE movups    xmmword ptr [rdi + 0x20], xmm0
00000001803CE4F2 movsd     xmm1, qword ptr [rbp - 0x39]
00000001803CE4F7 xorps     xmm0, xmm0
00000001803CE4FA movsd     qword ptr [rdi + 0x30], xmm1
00000001803CE4FF mov       eax, dword ptr [rbp - 0x31]
00000001803CE502 movss     xmm1, dword ptr [rip + 0x11d95a6] # 0x1815a7ab0
00000001803CE50A mov       dword ptr [rdi + 0x38], eax
00000001803CE50D xor       eax, eax
00000001803CE50F mov       qword ptr [rbp - 0x19], rax
00000001803CE513 mov       dword ptr [rbp - 0x11], eax
00000001803CE516 movups    xmmword ptr [rbp - 0x29], xmm0
00000001803CE51A call      0x1812ed750
00000001803CE51F cmp       dword ptr [rdi + 0x18], 1
00000001803CE523 jbe       0x1803ce6a7
00000001803CE529 movups    xmm0, xmmword ptr [rbp - 0x29]
00000001803CE52D xor       r9d, r9d
00000001803CE530 lea       rcx, [rbp - 9]
00000001803CE534 xorps     xmm2, xmm2
00000001803CE537 movups    xmmword ptr [rdi + 0x3c], xmm0
00000001803CE53B movsd     xmm1, qword ptr [rbp - 0x19]
00000001803CE540 xorps     xmm0, xmm0
00000001803CE543 movsd     qword ptr [rdi + 0x4c], xmm1
00000001803CE548 mov       eax, dword ptr [rbp - 0x11]
00000001803CE54B movss     xmm1, dword ptr [rip + 0x11d94b5] # 0x1815a7a08
00000001803CE553 mov       dword ptr [rdi + 0x54], eax
00000001803CE556 xor       eax, eax
00000001803CE558 mov       qword ptr [rbp + 7], rax
00000001803CE55C mov       dword ptr [rbp + 0xf], eax
00000001803CE55F movups    xmmword ptr [rbp - 9], xmm0
00000001803CE563 call      0x1812ed750
00000001803CE568 cmp       dword ptr [rdi + 0x18], 2
00000001803CE56C jbe       0x1803ce6a7
00000001803CE572 movups    xmm0, xmmword ptr [rbp - 9]
00000001803CE576 lea       rcx, [rbp + 0x17]
00000001803CE57A xor       r9d, r9d
00000001803CE57D movss     xmm2, dword ptr [rip + 0x11d949b] # 0x1815a7a20
00000001803CE585 movups    xmmword ptr [rdi + 0x58], xmm0
00000001803CE589 movsd     xmm1, qword ptr [rbp + 7]
00000001803CE58E xorps     xmm0, xmm0
00000001803CE591 movsd     qword ptr [rdi + 0x68], xmm1
00000001803CE596 mov       eax, dword ptr [rbp + 0xf]
00000001803CE599 movss     xmm1, dword ptr [rip + 0x11d9673] # 0x1815a7c14
00000001803CE5A1 mov       dword ptr [rdi + 0x70], eax
00000001803CE5A4 xor       eax, eax
00000001803CE5A6 mov       qword ptr [rbp + 0x27], rax
00000001803CE5AA mov       dword ptr [rbp + 0x2f], eax
00000001803CE5AD movups    xmmword ptr [rbp + 0x17], xmm0
00000001803CE5B1 call      0x1812ed750
00000001803CE5B6 cmp       dword ptr [rdi + 0x18], 3
00000001803CE5BA jbe       0x1803ce6a7
00000001803CE5C0 movups    xmm0, xmmword ptr [rbp + 0x17]
00000001803CE5C4 xor       r9d, r9d
00000001803CE5C7 lea       rcx, [rbp + 0x37]
00000001803CE5CB xorps     xmm2, xmm2
00000001803CE5CE movups    xmmword ptr [rdi + 0x74], xmm0
00000001803CE5D2 movsd     xmm1, qword ptr [rbp + 0x27]
00000001803CE5D7 xorps     xmm0, xmm0
00000001803CE5DA movsd     qword ptr [rdi + 0x84], xmm1
00000001803CE5E2 mov       eax, dword ptr [rbp + 0x2f]
00000001803CE5E5 movss     xmm1, dword ptr [rip + 0x11d9423] # 0x1815a7a10
00000001803CE5ED mov       dword ptr [rdi + 0x8c], eax
00000001803CE5F3 xor       eax, eax
00000001803CE5F5 mov       qword ptr [rbp + 0x47], rax
00000001803CE5F9 mov       dword ptr [rbp + 0x4f], eax
00000001803CE5FC movups    xmmword ptr [rbp + 0x37], xmm0
00000001803CE600 call      0x1812ed750
00000001803CE605 cmp       dword ptr [rdi + 0x18], 4
00000001803CE609 jbe       0x1803ce6a7
00000001803CE60F movups    xmm0, xmmword ptr [rbp + 0x37]
00000001803CE613 mov       qword ptr [rsp + 0xd0], rbx
00000001803CE61B movups    xmmword ptr [rdi + 0x90], xmm0
00000001803CE622 movsd     xmm1, qword ptr [rbp + 0x47]
00000001803CE627 movsd     qword ptr [rdi + 0xa0], xmm1
00000001803CE62F mov       eax, dword ptr [rbp + 0x4f]
00000001803CE632 mov       dword ptr [rdi + 0xa8], eax
00000001803CE638 mov       rcx, qword ptr [rip + 0x1800629] # 0x181bcec68
00000001803CE63F call      0x180250100
00000001803CE644 xor       r8d, r8d
00000001803CE647 mov       rdx, rdi
00000001803CE64A mov       rcx, rax
00000001803CE64D mov       rbx, rax
00000001803CE650 call      0x1812de700
00000001803CE655 lea       rcx, [rsi + 0x28]
00000001803CE659 mov       qword ptr [rsi + 0x28], rbx
00000001803CE65D mov       rdx, rbx
00000001803CE660 call      0x18024f360
00000001803CE665 xor       edx, edx
00000001803CE667 mov       dword ptr [rsi + 0x30], 0x3f800000
00000001803CE66E mov       rcx, rsi
00000001803CE671 mov       dword ptr [rsi + 0x34], 0x3f800000
00000001803CE678 mov       dword ptr [rsi + 0x38], 0x3f800000
00000001803CE67F call      0x1812e22a0
00000001803CE684 mov       rbx, qword ptr [rsp + 0xd0]
00000001803CE68C lea       r11, [rsp + 0xc0]
00000001803CE694 mov       rsi, qword ptr [r11 + 0x18]
00000001803CE698 mov       rdi, qword ptr [r11 + 0x20]
00000001803CE69C mov       rsp, r11
00000001803CE69F pop       rbp
00000001803CE6A0 ret       
00000001803CE6A1 call      0x180250150
00000001803CE6A6 int3      
00000001803CE6A7 call      0x180250140
00000001803CE6AC int3      