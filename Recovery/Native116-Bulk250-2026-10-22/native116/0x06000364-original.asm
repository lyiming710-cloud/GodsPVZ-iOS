000000018034F3C0 mov       qword ptr [rsp + 0x18], rbx
000000018034F3C5 mov       qword ptr [rsp + 0x20], rsi
000000018034F3CA mov       dword ptr [rsp + 0x10], edx
000000018034F3CE push      rdi
000000018034F3CF push      r12
000000018034F3D1 push      r13
000000018034F3D3 push      r14
000000018034F3D5 push      r15
000000018034F3D7 sub       rsp, 0x80
000000018034F3DE movaps    xmmword ptr [rsp + 0x70], xmm6
000000018034F3E3 mov       edi, edx
000000018034F3E5 mov       r15, rcx
000000018034F3E8 cmp       byte ptr [rip + 0x199590e], 0 # 0x181ce4cfd
000000018034F3EF jne       0x18034f480
000000018034F3F5 lea       rcx, [rip + 0x1868d0c] # 0x181bb8108
000000018034F3FC call      0x18024fef0
000000018034F401 lea       rcx, [rip + 0x18732f8] # 0x181bc2700
000000018034F408 call      0x18024fef0
000000018034F40D lea       rcx, [rip + 0x1873344] # 0x181bc2758
000000018034F414 call      0x18024fef0
000000018034F419 lea       rcx, [rip + 0x1873390] # 0x181bc27b0
000000018034F420 call      0x18024fef0
000000018034F425 lea       rcx, [rip + 0x1859be4] # 0x181ba9010
000000018034F42C call      0x18024fef0
000000018034F431 lea       rcx, [rip + 0x1859c88] # 0x181ba90c0
000000018034F438 call      0x18024fef0
000000018034F43D lea       rcx, [rip + 0x1859b74] # 0x181ba8fb8
000000018034F444 call      0x18024fef0
000000018034F449 lea       rcx, [rip + 0x1859d20] # 0x181ba9170
000000018034F450 call      0x18024fef0
000000018034F455 lea       rcx, [rip + 0x1859d6c] # 0x181ba91c8
000000018034F45C call      0x18024fef0
000000018034F461 lea       rcx, [rip + 0x1859db8] # 0x181ba9220
000000018034F468 call      0x18024fef0
000000018034F46D lea       rcx, [rip + 0x186dee4] # 0x181bbd358
000000018034F474 call      0x18024fef0
000000018034F479 mov       byte ptr [rip + 0x199587d], 1 # 0x181ce4cfd
000000018034F480 mov       rcx, qword ptr [r15 + 0x70]
000000018034F484 cmp       byte ptr [r15 + 0xe8], 0
000000018034F48C jne       0x18034f49f
000000018034F48E test      rcx, rcx
000000018034F491 je        0x18034f723
000000018034F497 xor       r12d, r12d
000000018034F49A mov       edx, r12d
000000018034F49D jmp       0x18034f4be
000000018034F49F mov       rax, qword ptr [r15 + 0xe0]
000000018034F4A6 test      rax, rax
000000018034F4A9 je        0x18034f723
000000018034F4AF mov       edx, dword ptr [rax + 0x18]
000000018034F4B2 test      rcx, rcx
000000018034F4B5 je        0x18034f723
000000018034F4BB xor       r12d, r12d
000000018034F4BE xor       r8d, r8d
000000018034F4C1 call      0x1803944e0
000000018034F4C6 mov       r13, rax
000000018034F4C9 mov       rcx, qword ptr [rip + 0x186de88] # 0x181bbd358
000000018034F4D0 call      0x180250100
000000018034F4D5 mov       r14, rax
000000018034F4D8 mov       rdx, qword ptr [rip + 0x1859ad9] # 0x181ba8fb8
000000018034F4DF mov       rcx, rax
000000018034F4E2 call      0x1808053d0
000000018034F4E7 mov       qword ptr [rsp + 0xb0], r14
000000018034F4EF mov       rax, qword ptr [r15 + 0x1e8]
000000018034F4F6 test      rax, rax
000000018034F4F9 je        0x18034f723
000000018034F4FF mov       rax, qword ptr [rax + 0x108]
000000018034F506 test      rax, rax
000000018034F509 je        0x18034f723
000000018034F50F mov       rdx, qword ptr [rax + 0x30]
000000018034F513 test      rdx, rdx
000000018034F516 je        0x18034f723
000000018034F51C mov       r8, qword ptr [rip + 0x1859b9d] # 0x181ba90c0
000000018034F523 lea       rcx, [rsp + 0x38]
000000018034F528 call      0x1808197f0
000000018034F52D movups    xmm0, xmmword ptr [rsp + 0x38]
000000018034F532 movups    xmmword ptr [rsp + 0x50], xmm0
000000018034F537 movsd     xmm1, qword ptr [rsp + 0x48]
000000018034F53D movsd     qword ptr [rsp + 0x60], xmm1
000000018034F543 mov       qword ptr [rsp + 0x38], r12
000000018034F548 lea       rbx, [rsp + 0x50]
000000018034F54D mov       qword ptr [rsp + 0x40], rbx
000000018034F552 jmp       0x18034f567
000000018034F554 nop       dword ptr [rax]
000000018034F558 nop       dword ptr [rax + rax]
000000018034F560 mov       edi, dword ptr [rsp + 0xb8]
000000018034F567 mov       rdx, qword ptr [rip + 0x18731ea] # 0x181bc2758
000000018034F56E lea       rcx, [rsp + 0x50]
000000018034F573 call      0x180669280
000000018034F578 test      al, al
000000018034F57A je        0x18034f6ca
000000018034F580 mov       rsi, qword ptr [rsp + 0x60]
000000018034F585 mov       r8d, 0xffffffff
000000018034F58B mov       ecx, dword ptr [r15 + 0x7c]
000000018034F58F add       ecx, -4
000000018034F592 je        0x18034f5b3
000000018034F594 sub       ecx, 1
000000018034F597 je        0x18034f5ab
000000018034F599 sub       ecx, 1
000000018034F59C je        0x18034f5b9
000000018034F59E cmp       ecx, 1
000000018034F5A1 jne       0x18034f5b9
000000018034F5A3 mov       r8d, 0x18
000000018034F5A9 jmp       0x18034f5b9
000000018034F5AB mov       r8d, 1
000000018034F5B1 jmp       0x18034f5b9
000000018034F5B3 mov       r8d, 0xf
000000018034F5B9 mov       qword ptr [rsp + 0x20], r12
000000018034F5BE xor       r9d, r9d
000000018034F5C1 xor       edx, edx
000000018034F5C3 mov       rcx, r15
000000018034F5C6 call      0x180350e40
000000018034F5CB test      rsi, rsi
000000018034F5CE je        0x18034f73e
000000018034F5D4 cmp       dword ptr [rsi + 0x5c], 2
000000018034F5D8 je        0x18034f567
000000018034F5DA cmp       dword ptr [rsi + 0x5c], 7
000000018034F5DE je        0x18034f567
000000018034F5E0 cmp       dword ptr [rsi + 0x5c], 8
000000018034F5E4 je        0x18034f567
000000018034F5E6 cmp       dword ptr [rsi + 0x5c], 0xc
000000018034F5EA je        0x18034f567
000000018034F5F0 cmp       dword ptr [rsi + 0x5c], 0xe
000000018034F5F4 je        0x18034f567
000000018034F5FA cmp       dword ptr [rsi + 0x5c], 0xb
000000018034F5FE je        0x18034f609
000000018034F600 cmp       byte ptr [rsi + 0x95], 0
000000018034F607 jmp       0x18034f616
000000018034F609 test      rax, rax
000000018034F60C je        0x18034f739
000000018034F612 cmp       dword ptr [rax + 0x18], 3
000000018034F616 jne       0x18034f567
000000018034F61C mov       eax, dword ptr [rsi + 0x58]
000000018034F61F cmp       eax, dword ptr [r15 + 0xbc]
000000018034F626 je        0x18034f567
000000018034F62C test      r13, r13
000000018034F62F je        0x18034f734
000000018034F635 mov       r8, qword ptr [rip + 0x1868acc] # 0x181bb8108
000000018034F63C mov       rdx, rsi
000000018034F63F mov       rcx, r13
000000018034F642 call      0x180426dd0
000000018034F647 test      al, al
000000018034F649 je        0x18034f567
000000018034F64F test      r14, r14
000000018034F652 je        0x18034f72f
000000018034F658 cmp       dword ptr [r14 + 0x18], edi
000000018034F65C jl        0x18034f6b3
000000018034F65E nop       
000000018034F660 mov       edi, r12d
000000018034F663 cmp       edi, dword ptr [r14 + 0x18]
000000018034F667 jge       0x18034f560
000000018034F66D movss     xmm6, dword ptr [rsi + 0x38]
000000018034F672 mov       r8, qword ptr [rip + 0x1859b4f] # 0x181ba91c8
000000018034F679 mov       edx, edi
000000018034F67B mov       rcx, r14
000000018034F67E call      0x180825710
000000018034F683 test      rax, rax
000000018034F686 je        0x18034f729
000000018034F68C movss     xmm0, dword ptr [rax + 0x38]
000000018034F691 comiss    xmm0, xmm6
000000018034F694 ja        0x18034f69a
000000018034F696 inc       edi
000000018034F698 jmp       0x18034f663
000000018034F69A mov       r9, qword ptr [rip + 0x1859b7f] # 0x181ba9220
000000018034F6A1 mov       r8, rsi
000000018034F6A4 mov       edx, edi
000000018034F6A6 mov       rcx, r14
000000018034F6A9 call      0x180855ab0
000000018034F6AE jmp       0x18034f560
000000018034F6B3 mov       r8, qword ptr [rip + 0x1859956] # 0x181ba9010
000000018034F6BA mov       rdx, rsi
000000018034F6BD mov       rcx, r14
000000018034F6C0 call      0x180002e70
000000018034F6C5 jmp       0x18034f567
000000018034F6CA mov       rdx, qword ptr [rip + 0x187302f] # 0x181bc2700
000000018034F6D1 mov       rcx, rbx
000000018034F6D4 call      0x180302170
000000018034F6D9 jmp       0x18034f6fe
000000018034F6DB mov       rdx, qword ptr [rip + 0x187301e] # 0x181bc2700
000000018034F6E2 mov       rcx, qword ptr [rsp + 0x40]
000000018034F6E7 call      0x180302170
000000018034F6EC mov       rcx, qword ptr [rsp + 0x38]
000000018034F6F1 test      rcx, rcx
000000018034F6F4 jne       0x18034f744
000000018034F6F6 mov       r14, qword ptr [rsp + 0xb0]
000000018034F6FE mov       rax, r14
000000018034F701 lea       r11, [rsp + 0x80]
000000018034F709 mov       rbx, qword ptr [r11 + 0x40]
000000018034F70D mov       rsi, qword ptr [r11 + 0x48]
000000018034F711 movaps    xmm6, xmmword ptr [rsp + 0x70]
000000018034F716 mov       rsp, r11
000000018034F719 pop       r15
000000018034F71B pop       r14
000000018034F71D pop       r13
000000018034F71F pop       r12
000000018034F721 pop       rdi
000000018034F722 ret       
000000018034F723 call      0x180250150
000000018034F728 nop       
000000018034F729 call      0x180250150
000000018034F72E nop       
000000018034F72F call      0x180250150
000000018034F734 call      0x180250150
000000018034F739 call      0x180250150
000000018034F73E call      0x180250150
000000018034F743 nop       
000000018034F744 call      0x180200e60
000000018034F749 int3      
000000018034F74A int3      
000000018034F74B int3      
000000018034F74C int3      
000000018034F74D int3      
000000018034F74E int3      
000000018034F74F int3      