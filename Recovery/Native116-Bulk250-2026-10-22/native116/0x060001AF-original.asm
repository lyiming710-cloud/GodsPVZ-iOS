00000001803189A0 mov       qword ptr [rsp + 0x10], rbx
00000001803189A5 mov       qword ptr [rsp + 0x18], rsi
00000001803189AA mov       qword ptr [rsp + 0x20], rdi
00000001803189AF mov       qword ptr [rsp + 8], rcx
00000001803189B4 push      r12
00000001803189B6 push      r14
00000001803189B8 push      r15
00000001803189BA sub       rsp, 0x70
00000001803189BE mov       rdi, rcx
00000001803189C1 cmp       byte ptr [rip + 0x19cc1e8], 0 # 0x181ce4bb0
00000001803189C8 jne       0x180318a0d
00000001803189CA lea       rcx, [rip + 0x18a8ba7] # 0x181bc1578
00000001803189D1 call      0x18024fef0
00000001803189D6 lea       rcx, [rip + 0x18a8bf3] # 0x181bc15d0
00000001803189DD call      0x18024fef0
00000001803189E2 lea       rcx, [rip + 0x18a8c3f] # 0x181bc1628
00000001803189E9 call      0x18024fef0
00000001803189EE lea       rcx, [rip + 0x188c2a3] # 0x181ba4c98
00000001803189F5 call      0x18024fef0
00000001803189FA lea       rcx, [rip + 0x188c357] # 0x181ba4d58
0000000180318A01 call      0x18024fef0
0000000180318A06 mov       byte ptr [rip + 0x19cc1a3], 1 # 0x181ce4bb0
0000000180318A0D mov       rax, qword ptr [rdi + 0x80]
0000000180318A14 test      rax, rax
0000000180318A17 je        0x180318c7f
0000000180318A1D mov       rcx, qword ptr [rax + 0x28]
0000000180318A21 test      rcx, rcx
0000000180318A24 je        0x180318c7f
0000000180318A2A mov       rdx, qword ptr [rcx + 0x68]
0000000180318A2E test      rdx, rdx
0000000180318A31 je        0x180318c7f
0000000180318A37 mov       r8, qword ptr [rip + 0x188c31a] # 0x181ba4d58
0000000180318A3E lea       rcx, [rsp + 0x38]
0000000180318A43 call      0x1808197f0
0000000180318A48 movups    xmm0, xmmword ptr [rsp + 0x38]
0000000180318A4D movups    xmmword ptr [rsp + 0x50], xmm0
0000000180318A52 movsd     xmm1, qword ptr [rsp + 0x48]
0000000180318A58 movsd     qword ptr [rsp + 0x60], xmm1
0000000180318A5E xor       r12d, r12d
0000000180318A61 mov       qword ptr [rsp + 0x38], r12
0000000180318A66 lea       rbx, [rsp + 0x50]
0000000180318A6B mov       qword ptr [rsp + 0x40], rbx
0000000180318A70 mov       rdx, qword ptr [rip + 0x18a8b59] # 0x181bc15d0
0000000180318A77 lea       rcx, [rsp + 0x50]
0000000180318A7C call      0x180669280
0000000180318A81 test      al, al
0000000180318A83 je        0x180318b26
0000000180318A89 mov       rdx, qword ptr [rsp + 0x60]
0000000180318A8E test      rdx, rdx
0000000180318A91 je        0x180318c95
0000000180318A97 mov       ecx, dword ptr [rdx + 0x10]
0000000180318A9A cmp       ecx, 1
0000000180318A9D je        0x180318af8
0000000180318A9F add       ecx, -0x16
0000000180318AA2 je        0x180318ace
0000000180318AA4 sub       ecx, 1
0000000180318AA7 je        0x180318af8
0000000180318AA9 sub       ecx, 1
0000000180318AAC je        0x180318a70
0000000180318AAE cmp       ecx, 1
0000000180318AB1 jne       0x180318a70
0000000180318AB3 mov       rcx, qword ptr [rdi + 0x40]
0000000180318AB7 test      rcx, rcx
0000000180318ABA je        0x180318c85
0000000180318AC0 mov       r8, qword ptr [rip + 0x188c1d1] # 0x181ba4c98
0000000180318AC7 call      0x180002e70
0000000180318ACC jmp       0x180318a70
0000000180318ACE cmp       byte ptr [rdx + 0x18], 0
0000000180318AD2 jne       0x180318a70
0000000180318AD4 cmp       byte ptr [rdx + 0x19], 0
0000000180318AD8 jne       0x180318a70
0000000180318ADA mov       rcx, qword ptr [rdi + 0x40]
0000000180318ADE test      rcx, rcx
0000000180318AE1 je        0x180318c8b
0000000180318AE7 mov       r8, qword ptr [rip + 0x188c1aa] # 0x181ba4c98
0000000180318AEE call      0x180002e70
0000000180318AF3 jmp       0x180318a70
0000000180318AF8 cmp       byte ptr [rdx + 0x18], 0
0000000180318AFC jne       0x180318b08
0000000180318AFE cmp       byte ptr [rdx + 0x19], 0
0000000180318B02 je        0x180318a70
0000000180318B08 mov       rcx, qword ptr [rdi + 0x40]
0000000180318B0C test      rcx, rcx
0000000180318B0F je        0x180318c90
0000000180318B15 mov       r8, qword ptr [rip + 0x188c17c] # 0x181ba4c98
0000000180318B1C call      0x180002e70
0000000180318B21 jmp       0x180318a70
0000000180318B26 mov       rdx, qword ptr [rip + 0x18a8a4b] # 0x181bc1578
0000000180318B2D mov       rcx, rbx
0000000180318B30 call      0x180302170
0000000180318B35 jmp       0x180318b61
0000000180318B37 mov       rdx, qword ptr [rip + 0x18a8a3a] # 0x181bc1578
0000000180318B3E mov       rcx, qword ptr [rsp + 0x40]
0000000180318B43 call      0x180302170
0000000180318B48 mov       rcx, qword ptr [rsp + 0x38]
0000000180318B4D test      rcx, rcx
0000000180318B50 jne       0x180318c9b
0000000180318B56 xor       r12d, r12d
0000000180318B59 mov       rdi, qword ptr [rsp + 0x90]
0000000180318B61 xor       edx, edx
0000000180318B63 mov       rcx, rdi
0000000180318B66 call      0x180316ab0
0000000180318B6B mov       r15, rax
0000000180318B6E xor       edx, edx
0000000180318B70 mov       rcx, rdi
0000000180318B73 call      0x180317b50
0000000180318B78 cmp       byte ptr [rip + 0x19cc038], 0 # 0x181ce4bb7
0000000180318B7F jne       0x180318b94
0000000180318B81 lea       rcx, [rip + 0x1892a00] # 0x181bab588
0000000180318B88 call      0x18024fef0
0000000180318B8D mov       byte ptr [rip + 0x19cc023], 1 # 0x181ce4bb7
0000000180318B94 mov       r14d, r12d
0000000180318B97 cmp       dword ptr [rdi + 0x48], 0
0000000180318B9B jle       0x180318c4c
0000000180318BA1 mov       rcx, qword ptr [rdi + 0x20]
0000000180318BA5 test      rcx, rcx
0000000180318BA8 je        0x180318c7f
0000000180318BAE mov       r8, qword ptr [rip + 0x18929d3] # 0x181bab588
0000000180318BB5 mov       edx, r14d
0000000180318BB8 call      0x180825710
0000000180318BBD mov       rsi, rax
0000000180318BC0 cmp       byte ptr [rip + 0x19cbff1], 0 # 0x181ce4bb8
0000000180318BC7 jne       0x180318bf0
0000000180318BC9 lea       rcx, [rip + 0x18aa3e0] # 0x181bc2fb0
0000000180318BD0 call      0x18024fef0
0000000180318BD5 lea       rcx, [rip + 0x18aa42c] # 0x181bc3008
0000000180318BDC call      0x18024fef0
0000000180318BE1 mov       byte ptr [rip + 0x19cbfd0], 1 # 0x181ce4bb8
0000000180318BE8 nop       dword ptr [rax + rax]
0000000180318BF0 mov       ebx, r12d
0000000180318BF3 test      rsi, rsi
0000000180318BF6 je        0x180318c7f
0000000180318BFC mov       rax, qword ptr [rsi + 0x10]
0000000180318C00 test      rax, rax
0000000180318C03 je        0x180318c7f
0000000180318C05 cmp       ebx, dword ptr [rax + 0x18]
0000000180318C08 jge       0x180318c3f
0000000180318C0A mov       r8, qword ptr [rip + 0x18aa3f7] # 0x181bc3008
0000000180318C11 mov       edx, ebx
0000000180318C13 mov       rcx, rax
0000000180318C16 call      0x180825710
0000000180318C1B test      rax, rax
0000000180318C1E je        0x180318c7f
0000000180318C20 test      r15, r15
0000000180318C23 je        0x180318c7f
0000000180318C25 mov       qword ptr [rsp + 0x20], r12
0000000180318C2A mov       r9, rax
0000000180318C2D mov       r8d, ebx
0000000180318C30 mov       edx, r14d
0000000180318C33 mov       rcx, r15
0000000180318C36 call      0x180319db0
0000000180318C3B inc       ebx
0000000180318C3D jmp       0x180318bfc
0000000180318C3F inc       r14d
0000000180318C42 cmp       r14d, dword ptr [rdi + 0x48]
0000000180318C46 jl        0x180318ba1
0000000180318C4C mov       qword ptr [rdi + 0x50], 0
0000000180318C54 mov       eax, dword ptr [rdi + 0x70]
0000000180318C57 mov       dword ptr [rdi + 0x60], eax
0000000180318C5A xor       edx, edx
0000000180318C5C mov       rcx, rdi
0000000180318C5F call      0x1803183e0
0000000180318C64 lea       r11, [rsp + 0x70]
0000000180318C69 mov       rbx, qword ptr [r11 + 0x28]
0000000180318C6D mov       rsi, qword ptr [r11 + 0x30]
0000000180318C71 mov       rdi, qword ptr [r11 + 0x38]
0000000180318C75 mov       rsp, r11
0000000180318C78 pop       r15
0000000180318C7A pop       r14
0000000180318C7C pop       r12
0000000180318C7E ret       
0000000180318C7F call      0x180250150
0000000180318C84 nop       
0000000180318C85 call      0x180250150
0000000180318C8A nop       
0000000180318C8B call      0x180250150
0000000180318C90 call      0x180250150
0000000180318C95 call      0x180250150
0000000180318C9A nop       
0000000180318C9B call      0x180200e60
0000000180318CA0 int3      
0000000180318CA1 int3      
0000000180318CA2 int3      
0000000180318CA3 int3      
0000000180318CA4 int3      
0000000180318CA5 int3      
0000000180318CA6 int3      
0000000180318CA7 int3      
0000000180318CA8 int3      
0000000180318CA9 int3      
0000000180318CAA int3      
0000000180318CAB int3      
0000000180318CAC int3      
0000000180318CAD int3      
0000000180318CAE int3      
0000000180318CAF int3      