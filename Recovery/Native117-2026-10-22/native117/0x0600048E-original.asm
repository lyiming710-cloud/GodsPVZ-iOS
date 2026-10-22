000000018036F380 mov       rax, rsp
000000018036F383 mov       qword ptr [rax + 0x10], rbx
000000018036F387 mov       qword ptr [rax + 0x18], rsi
000000018036F38B mov       qword ptr [rax + 8], rcx
000000018036F38F push      rdi
000000018036F390 push      r12
000000018036F392 push      r13
000000018036F394 push      r14
000000018036F396 push      r15
000000018036F398 sub       rsp, 0xa0
000000018036F39F movaps    xmmword ptr [rax - 0x38], xmm6
000000018036F3A3 movaps    xmmword ptr [rax - 0x48], xmm7
000000018036F3A7 mov       r13, rcx
000000018036F3AA cmp       byte ptr [rip + 0x19759e4], 0 # 0x181ce4d95
000000018036F3B1 jne       0x18036f432
000000018036F3B3 lea       rcx, [rip + 0x185b4ae] # 0x181bca868
000000018036F3BA call      0x18024fef0
000000018036F3BF lea       rcx, [rip + 0x185b4f2] # 0x181bca8b8
000000018036F3C6 call      0x18024fef0
000000018036F3CB lea       rcx, [rip + 0x185b53e] # 0x181bca910
000000018036F3D2 call      0x18024fef0
000000018036F3D7 lea       rcx, [rip + 0x1835c7a] # 0x181ba5058
000000018036F3DE call      0x18024fef0
000000018036F3E3 lea       rcx, [rip + 0x18542fe] # 0x181bc36e8
000000018036F3EA call      0x18024fef0
000000018036F3EF lea       rcx, [rip + 0x184110a] # 0x181bb0500
000000018036F3F6 call      0x18024fef0
000000018036F3FB lea       rcx, [rip + 0x184b3ce] # 0x181bba7d0
000000018036F402 call      0x18024fef0
000000018036F407 lea       rcx, [rip + 0x1830b02] # 0x181b9ff10
000000018036F40E call      0x18024fef0
000000018036F413 lea       rcx, [rip + 0x185c9ae] # 0x181bcbdc8
000000018036F41A call      0x18024fef0
000000018036F41F lea       rcx, [rip + 0x183e3b2] # 0x181bad7d8
000000018036F426 call      0x18024fef0
000000018036F42B mov       byte ptr [rip + 0x1975963], 1 # 0x181ce4d95
000000018036F432 mov       rax, qword ptr [r13 + 0x20]
000000018036F436 test      rax, rax
000000018036F439 je        0x18036f6e9
000000018036F43F mov       rcx, qword ptr [rax + 0xf0]
000000018036F446 test      rcx, rcx
000000018036F449 je        0x18036f6e9
000000018036F44F mov       rdx, qword ptr [rcx + 0x38]
000000018036F453 test      rdx, rdx
000000018036F456 je        0x18036f6e9
000000018036F45C mov       r8, qword ptr [rip + 0x1854285] # 0x181bc36e8
000000018036F463 lea       rcx, [rsp + 0x50]
000000018036F468 call      0x1808197f0
000000018036F46D movups    xmm0, xmmword ptr [rsp + 0x50]
000000018036F472 movups    xmmword ptr [rsp + 0x68], xmm0
000000018036F477 movsd     xmm1, qword ptr [rsp + 0x60]
000000018036F47D movsd     qword ptr [rsp + 0x78], xmm1
000000018036F483 xor       r12d, r12d
000000018036F486 mov       qword ptr [rsp + 0x50], r12
000000018036F48B lea       rbx, [rsp + 0x68]
000000018036F490 mov       qword ptr [rsp + 0x58], rbx
000000018036F495 movss     xmm6, dword ptr [rip + 0x123856b] # 0x1815a7a08
000000018036F49D movss     xmm7, dword ptr [rip + 0x123856b] # 0x1815a7a10
000000018036F4A5 nop       word ptr [rax + rax]
000000018036F4B0 mov       rdx, qword ptr [rip + 0x185b401] # 0x181bca8b8
000000018036F4B7 lea       rcx, [rsp + 0x68]
000000018036F4BC call      0x180669280
000000018036F4C1 test      al, al
000000018036F4C3 je        0x18036f5bd
000000018036F4C9 mov       rsi, qword ptr [rsp + 0x78]
000000018036F4CE test      rsi, rsi
000000018036F4D1 je        0x18036f704
000000018036F4D7 xor       edx, edx
000000018036F4D9 mov       rcx, rsi
000000018036F4DC call      0x1803659e0
000000018036F4E1 test      al, al
000000018036F4E3 jne       0x18036f4b0
000000018036F4E5 mov       eax, dword ptr [rsi + 0xfc]
000000018036F4EB cmp       eax, dword ptr [r13 + 0xfc]
000000018036F4F2 jne       0x18036f4b0
000000018036F4F4 mov       rcx, qword ptr [rip + 0x184b2d5] # 0x181bba7d0
000000018036F4FB call      0x180250100
000000018036F500 mov       rdi, rax
000000018036F503 mov       qword ptr [rsp + 0x20], r12
000000018036F508 mov       r9b, 1
000000018036F50B movaps    xmm2, xmm6
000000018036F50E mov       rdx, qword ptr [rip + 0x183e2c3] # 0x181bad7d8
000000018036F515 mov       rcx, rax
000000018036F518 call      0x180325080
000000018036F51D test      rdi, rdi
000000018036F520 je        0x18036f6ff
000000018036F526 mov       rax, qword ptr [rip + 0x185c89b] # 0x181bcbdc8
000000018036F52D mov       qword ptr [rdi + 0x10], rax
000000018036F531 lea       rcx, [rdi + 0x10]
000000018036F535 mov       rdx, qword ptr [rip + 0x185c88c] # 0x181bcbdc8
000000018036F53C call      0x18024f360
000000018036F541 mov       dword ptr [rdi + 0x18], r12d
000000018036F545 mov       dword ptr [rdi + 0x1c], 0x41700000
000000018036F54C mov       r14, qword ptr [rdi + 0x48]
000000018036F550 mov       rcx, qword ptr [rip + 0x184b279] # 0x181bba7d0
000000018036F557 call      0x180250100
000000018036F55C mov       r15, rax
000000018036F55F mov       qword ptr [rsp + 0x20], r12
000000018036F564 mov       r9b, 1
000000018036F567 movaps    xmm2, xmm7
000000018036F56A mov       rdx, qword ptr [rip + 0x183099f] # 0x181b9ff10
000000018036F571 mov       rcx, rax
000000018036F574 call      0x180325080
000000018036F579 test      r14, r14
000000018036F57C je        0x18036f6fa
000000018036F582 mov       r8, qword ptr [rip + 0x1835acf] # 0x181ba5058
000000018036F589 mov       rdx, r15
000000018036F58C mov       rcx, r14
000000018036F58F call      0x180002e70
000000018036F594 test      rsi, rsi
000000018036F597 je        0x18036f6f5
000000018036F59D mov       rcx, qword ptr [rsi + 0x228]
000000018036F5A4 test      rcx, rcx
000000018036F5A7 je        0x18036f6ef
000000018036F5AD xor       r8d, r8d
000000018036F5B0 mov       rdx, rdi
000000018036F5B3 call      0x180310190
000000018036F5B8 jmp       0x18036f4b0
000000018036F5BD mov       rdx, qword ptr [rip + 0x185b2a4] # 0x181bca868
000000018036F5C4 mov       rcx, rbx
000000018036F5C7 call      0x180302170
000000018036F5CC jmp       0x18036f5f5
000000018036F5CE mov       rdx, qword ptr [rip + 0x185b293] # 0x181bca868
000000018036F5D5 mov       rcx, qword ptr [rsp + 0x58]
000000018036F5DA call      0x180302170
000000018036F5DF mov       rcx, qword ptr [rsp + 0x50]
000000018036F5E4 test      rcx, rcx
000000018036F5E7 jne       0x18036f70a
000000018036F5ED mov       r13, qword ptr [rsp + 0xd0]
000000018036F5F5 cmp       byte ptr [rip + 0x1975538], 0 # 0x181ce4b34
000000018036F5FC jne       0x18036f611
000000018036F5FE lea       rcx, [rip + 0x184228b] # 0x181bb1890
000000018036F605 call      0x18024fef0
000000018036F60A mov       byte ptr [rip + 0x1975523], 1 # 0x181ce4b34
000000018036F611 mov       rax, qword ptr [rip + 0x1842278] # 0x181bb1890
000000018036F618 mov       rcx, qword ptr [rax + 0xb8]
000000018036F61F mov       rcx, qword ptr [rcx]
000000018036F622 test      rcx, rcx
000000018036F625 je        0x18036f6e9
000000018036F62B xor       r8d, r8d
000000018036F62E mov       edx, 0xe
000000018036F633 call      0x180321f30
000000018036F638 mov       rbx, rax
000000018036F63B mov       rcx, qword ptr [rip + 0x1840ebe] # 0x181bb0500
000000018036F642 cmp       dword ptr [rcx + 0xe0], 0
000000018036F649 jne       0x18036f650
000000018036F64B call      0x1802501e0
000000018036F650 xor       edx, edx
000000018036F652 mov       rcx, rbx
000000018036F655 call      0x18131f870
000000018036F65A test      al, al
000000018036F65C je        0x18036f6c2
000000018036F65E test      rbx, rbx
000000018036F661 je        0x18036f6e9
000000018036F667 xor       edx, edx
000000018036F669 mov       rcx, rbx
000000018036F66C call      0x18131c070
000000018036F671 mov       rbx, rax
000000018036F674 mov       rcx, qword ptr [r13 + 0x1f0]
000000018036F67B test      rcx, rcx
000000018036F67E je        0x18036f6e9
000000018036F680 xor       edx, edx
000000018036F682 call      0x181304510
000000018036F687 test      rax, rax
000000018036F68A je        0x18036f6e9
000000018036F68C xor       r8d, r8d
000000018036F68F mov       rdx, rax
000000018036F692 lea       rcx, [rsp + 0x50]
000000018036F697 call      0x18132f3a0
000000018036F69C test      rbx, rbx
000000018036F69F je        0x18036f6e9
000000018036F6A1 movsd     xmm0, qword ptr [rax]
000000018036F6A5 movsd     qword ptr [rsp + 0x40], xmm0
000000018036F6AB mov       eax, dword ptr [rax + 8]
000000018036F6AE mov       dword ptr [rsp + 0x48], eax
000000018036F6B2 xor       r8d, r8d
000000018036F6B5 lea       rdx, [rsp + 0x40]
000000018036F6BA mov       rcx, rbx
000000018036F6BD call      0x18132fbb0
000000018036F6C2 lea       r11, [rsp + 0xa0]
000000018036F6CA mov       rbx, qword ptr [r11 + 0x38]
000000018036F6CE mov       rsi, qword ptr [r11 + 0x40]
000000018036F6D2 movaps    xmm6, xmmword ptr [r11 - 0x10]
000000018036F6D7 movaps    xmm7, xmmword ptr [r11 - 0x20]
000000018036F6DC mov       rsp, r11
000000018036F6DF pop       r15
000000018036F6E1 pop       r14
000000018036F6E3 pop       r13
000000018036F6E5 pop       r12
000000018036F6E7 pop       rdi
000000018036F6E8 ret       
000000018036F6E9 call      0x180250150
000000018036F6EE nop       
000000018036F6EF call      0x180250150
000000018036F6F4 nop       
000000018036F6F5 call      0x180250150
000000018036F6FA call      0x180250150
000000018036F6FF call      0x180250150
000000018036F704 call      0x180250150
000000018036F709 nop       
000000018036F70A call      0x180200e60
000000018036F70F int3      