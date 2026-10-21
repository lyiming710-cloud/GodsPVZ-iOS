00000001803A8780 mov       qword ptr [rsp + 0x20], rbx
00000001803A8785 mov       qword ptr [rsp + 0x18], r8
00000001803A878A mov       qword ptr [rsp + 0x10], rdx
00000001803A878F mov       qword ptr [rsp + 8], rcx
00000001803A8794 push      rsi
00000001803A8795 push      rdi
00000001803A8796 push      r12
00000001803A8798 push      r14
00000001803A879A push      r15
00000001803A879C sub       rsp, 0xe0
00000001803A87A3 mov       rdi, r8
00000001803A87A6 mov       r14, rdx
00000001803A87A9 mov       r15, rcx
00000001803A87AC cmp       byte ptr [rip + 0x193c747], 0 # 0x181ce4efa
00000001803A87B3 jne       0x1803a8850
00000001803A87B9 lea       rcx, [rip + 0x181c770] # 0x181bc4f30
00000001803A87C0 call      0x18024fef0
00000001803A87C5 lea       rcx, [rip + 0x182334c] # 0x181bcbb18
00000001803A87CC call      0x18024fef0
00000001803A87D1 lea       rcx, [rip + 0x181eb70] # 0x181bc7348
00000001803A87D8 call      0x18024fef0
00000001803A87DD lea       rcx, [rip + 0x181ebbc] # 0x181bc73a0
00000001803A87E4 call      0x18024fef0
00000001803A87E9 lea       rcx, [rip + 0x181c798] # 0x181bc4f88
00000001803A87F0 call      0x18024fef0
00000001803A87F5 lea       rcx, [rip + 0x1823374] # 0x181bcbb70
00000001803A87FC call      0x18024fef0
00000001803A8801 lea       rcx, [rip + 0x18233b8] # 0x181bcbbc0
00000001803A8808 call      0x18024fef0
00000001803A880D lea       rcx, [rip + 0x181ebe4] # 0x181bc73f8
00000001803A8814 call      0x18024fef0
00000001803A8819 lea       rcx, [rip + 0x181c7c0] # 0x181bc4fe0
00000001803A8820 call      0x18024fef0
00000001803A8825 lea       rcx, [rip + 0x180d6b4] # 0x181bb5ee0
00000001803A882C call      0x18024fef0
00000001803A8831 lea       rcx, [rip + 0x181f5d8] # 0x181bc7e10
00000001803A8838 call      0x18024fef0
00000001803A883D lea       rcx, [rip + 0x18072ac] # 0x181bafaf0
00000001803A8844 call      0x18024fef0
00000001803A8849 mov       byte ptr [rip + 0x193c6aa], 1 # 0x181ce4efa
00000001803A8850 xorps     xmm0, xmm0
00000001803A8853 xor       eax, eax
00000001803A8855 movups    xmmword ptr [rsp + 0x50], xmm0
00000001803A885A mov       qword ptr [rsp + 0x60], rax
00000001803A885F xorps     xmm1, xmm1
00000001803A8862 movups    xmmword ptr [rsp + 0x68], xmm1
00000001803A8867 mov       qword ptr [rsp + 0x78], rax
00000001803A886C mov       rdx, qword ptr [r15 + 0x40]
00000001803A8870 test      rdx, rdx
00000001803A8873 je        0x1803a8b38
00000001803A8879 mov       r8, qword ptr [rip + 0x181f590] # 0x181bc7e10
00000001803A8880 lea       rcx, [rsp + 0x38]
00000001803A8885 call      0x1808197f0
00000001803A888A movups    xmm0, xmmword ptr [rsp + 0x38]
00000001803A888F movups    xmmword ptr [rsp + 0x80], xmm0
00000001803A8897 movsd     xmm1, qword ptr [rsp + 0x48]
00000001803A889D movsd     qword ptr [rsp + 0x90], xmm1
00000001803A88A6 xor       r12d, r12d
00000001803A88A9 mov       qword ptr [rsp + 0x28], r12
00000001803A88AE lea       rbx, [rsp + 0x80]
00000001803A88B6 mov       qword ptr [rsp + 0x30], rbx
00000001803A88BB nop       dword ptr [rax + rax]
00000001803A88C0 mov       rdx, qword ptr [rip + 0x18232a9] # 0x181bcbb70
00000001803A88C7 lea       rcx, [rsp + 0x80]
00000001803A88CF call      0x180669280
00000001803A88D4 test      al, al
00000001803A88D6 je        0x1803a88f5
00000001803A88D8 mov       rcx, qword ptr [rsp + 0x90]
00000001803A88E0 test      rcx, rcx
00000001803A88E3 je        0x1803a8b10
00000001803A88E9 xor       r8d, r8d
00000001803A88EC xor       edx, edx
00000001803A88EE call      0x1803a9090
00000001803A88F3 jmp       0x1803a88c0
00000001803A88F5 mov       rdx, qword ptr [rip + 0x182321c] # 0x181bcbb18
00000001803A88FC mov       rcx, rbx
00000001803A88FF call      0x180302170
00000001803A8904 jmp       0x1803a8940
00000001803A8906 mov       rdx, qword ptr [rip + 0x182320b] # 0x181bcbb18
00000001803A890D mov       rcx, qword ptr [rsp + 0x30]
00000001803A8912 call      0x180302170
00000001803A8917 mov       rcx, qword ptr [rsp + 0x28]
00000001803A891C test      rcx, rcx
00000001803A891F jne       0x1803a8b16
00000001803A8925 xor       r12d, r12d
00000001803A8928 mov       rdi, qword ptr [rsp + 0x120]
00000001803A8930 mov       r14, qword ptr [rsp + 0x118]
00000001803A8938 mov       r15, qword ptr [rsp + 0x110]
00000001803A8940 mov       esi, r12d
00000001803A8943 mov       dword ptr [rsp + 0x20], r12d
00000001803A8948 movups    xmm0, xmmword ptr [r14]
00000001803A894C movups    xmmword ptr [rsp + 0x98], xmm0
00000001803A8954 movups    xmm1, xmmword ptr [r14 + 0x10]
00000001803A8959 movups    xmmword ptr [rsp + 0xa8], xmm1
00000001803A8961 movsd     xmm0, qword ptr [r14 + 0x20]
00000001803A8967 movsd     qword ptr [rsp + 0xb8], xmm0
00000001803A8970 mov       rdx, qword ptr [rsp + 0xb8]
00000001803A8978 test      rdx, rdx
00000001803A897B je        0x1803a8b38
00000001803A8981 mov       r8, qword ptr [rip + 0x1807168] # 0x181bafaf0
00000001803A8988 lea       rcx, [rsp + 0x38]
00000001803A898D call      0x18082ac10
00000001803A8992 movups    xmm0, xmmword ptr [rsp + 0x38]
00000001803A8997 movups    xmmword ptr [rsp + 0x50], xmm0
00000001803A899C movsd     xmm1, qword ptr [rsp + 0x48]
00000001803A89A2 movsd     qword ptr [rsp + 0x60], xmm1
00000001803A89A8 mov       qword ptr [rsp + 0x28], r12
00000001803A89AD lea       rax, [rsp + 0x50]
00000001803A89B2 mov       qword ptr [rsp + 0x30], rax
00000001803A89B7 nop       word ptr [rax + rax]
00000001803A89C0 mov       rdx, qword ptr [rip + 0x181c5c1] # 0x181bc4f88
00000001803A89C7 lea       rcx, [rsp + 0x50]
00000001803A89CC call      0x180668160
00000001803A89D1 test      al, al
00000001803A89D3 je        0x1803a8aca
00000001803A89D9 mov       r14d, dword ptr [rsp + 0x60]
00000001803A89DE mov       rdx, qword ptr [r15 + 0x28]
00000001803A89E2 test      rdx, rdx
00000001803A89E5 je        0x1803a8b2c
00000001803A89EB mov       r8, qword ptr [rip + 0x180d4ee] # 0x181bb5ee0
00000001803A89F2 lea       rcx, [rsp + 0x98]
00000001803A89FA call      0x1808197f0
00000001803A89FF movups    xmm0, xmmword ptr [rsp + 0x98]
00000001803A8A07 movups    xmmword ptr [rsp + 0x68], xmm0
00000001803A8A0C movsd     xmm1, qword ptr [rsp + 0xa8]
00000001803A8A15 movsd     qword ptr [rsp + 0x78], xmm1
00000001803A8A1B mov       qword ptr [rsp + 0x38], r12
00000001803A8A20 lea       rbx, [rsp + 0x68]
00000001803A8A25 mov       qword ptr [rsp + 0x40], rbx
00000001803A8A2A nop       word ptr [rax + rax]
00000001803A8A30 mov       rdx, qword ptr [rip + 0x181e969] # 0x181bc73a0
00000001803A8A37 lea       rcx, [rsp + 0x68]
00000001803A8A3C call      0x180669280
00000001803A8A41 test      al, al
00000001803A8A43 je        0x1803a8a91
00000001803A8A45 test      rdi, rdi
00000001803A8A48 je        0x1803a8b21
00000001803A8A4E cmp       esi, dword ptr [rdi + 0x28]
00000001803A8A51 jge       0x1803a8a7d
00000001803A8A53 mov       rdx, qword ptr [rsp + 0x78]
00000001803A8A58 test      rdx, rdx
00000001803A8A5B je        0x1803a8b1c
00000001803A8A61 cmp       r14d, dword ptr [rdx + 0x28]
00000001803A8A65 jne       0x1803a8a30
00000001803A8A67 xor       r9d, r9d
00000001803A8A6A mov       r8d, esi
00000001803A8A6D mov       rcx, r15
00000001803A8A70 call      0x1803a8b40
00000001803A8A75 inc       esi
00000001803A8A77 mov       dword ptr [rsp + 0x20], esi
00000001803A8A7B jmp       0x1803a8a7d
00000001803A8A7D mov       rdx, qword ptr [rip + 0x181e8c4] # 0x181bc7348
00000001803A8A84 mov       rcx, rbx
00000001803A8A87 call      0x180302170
00000001803A8A8C jmp       0x1803a89c0
00000001803A8A91 jmp       0x1803a8a7d
00000001803A8A93 mov       rdx, qword ptr [rip + 0x181e8ae] # 0x181bc7348
00000001803A8A9A mov       rcx, qword ptr [rsp + 0x40]
00000001803A8A9F call      0x180302170
00000001803A8AA4 mov       rcx, qword ptr [rsp + 0x38]
00000001803A8AA9 test      rcx, rcx
00000001803A8AAC jne       0x1803a8b27
00000001803A8AAE xor       r12d, r12d
00000001803A8AB1 mov       rdi, qword ptr [rsp + 0x120]
00000001803A8AB9 mov       r15, qword ptr [rsp + 0x110]
00000001803A8AC1 mov       esi, dword ptr [rsp + 0x20]
00000001803A8AC5 jmp       0x1803a89c0
00000001803A8ACA mov       rdx, qword ptr [rip + 0x181c45f] # 0x181bc4f30
00000001803A8AD1 mov       rcx, qword ptr [rsp + 0x30]
00000001803A8AD6 call      0x180302170
00000001803A8ADB jmp       0x1803a8af8
00000001803A8ADD mov       rdx, qword ptr [rip + 0x181c44c] # 0x181bc4f30
00000001803A8AE4 mov       rcx, qword ptr [rsp + 0x30]
00000001803A8AE9 call      0x180302170
00000001803A8AEE mov       rcx, qword ptr [rsp + 0x28]
00000001803A8AF3 test      rcx, rcx
00000001803A8AF6 jne       0x1803a8b32
00000001803A8AF8 mov       rbx, qword ptr [rsp + 0x128]
00000001803A8B00 add       rsp, 0xe0
00000001803A8B07 pop       r15
00000001803A8B09 pop       r14
00000001803A8B0B pop       r12
00000001803A8B0D pop       rdi
00000001803A8B0E pop       rsi
00000001803A8B0F ret       
00000001803A8B10 call      0x180250150
00000001803A8B15 nop       
00000001803A8B16 call      0x180200e60
00000001803A8B1B nop       
00000001803A8B1C call      0x180250150
00000001803A8B21 call      0x180250150
00000001803A8B26 nop       
00000001803A8B27 call      0x180200e60
00000001803A8B2C call      0x180250150
00000001803A8B31 nop       
00000001803A8B32 call      0x180200e60
00000001803A8B37 int3      
00000001803A8B38 call      0x180250150
00000001803A8B3D int3      
00000001803A8B3E int3      
00000001803A8B3F int3      