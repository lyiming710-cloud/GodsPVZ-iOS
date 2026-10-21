00000001803AAB80 mov       qword ptr [rsp + 0x10], rdx
00000001803AAB85 mov       qword ptr [rsp + 8], rcx
00000001803AAB8A push      rbx
00000001803AAB8B push      rsi
00000001803AAB8C push      rdi
00000001803AAB8D push      r12
00000001803AAB8F push      r13
00000001803AAB91 push      r14
00000001803AAB93 push      r15
00000001803AAB95 sub       rsp, 0x90
00000001803AAB9C mov       rsi, rdx
00000001803AAB9F mov       r13, rcx
00000001803AABA2 cmp       byte ptr [rip + 0x193a35f], 0 # 0x181ce4f08
00000001803AABA9 jne       0x1803aabfa
00000001803AABAB lea       rcx, [rip + 0x1816ff6] # 0x181bc1ba8
00000001803AABB2 call      0x18024fef0
00000001803AABB7 lea       rcx, [rip + 0x1817042] # 0x181bc1c00
00000001803AABBE call      0x18024fef0
00000001803AABC3 lea       rcx, [rip + 0x181708e] # 0x181bc1c58
00000001803AABCA call      0x18024fef0
00000001803AABCF lea       rcx, [rip + 0x17fb562] # 0x181ba6138
00000001803AABD6 call      0x18024fef0
00000001803AABDB lea       rcx, [rip + 0x1805226] # 0x181bafe08
00000001803AABE2 call      0x18024fef0
00000001803AABE7 lea       rcx, [rip + 0x1805272] # 0x181bafe60
00000001803AABEE call      0x18024fef0
00000001803AABF3 mov       byte ptr [rip + 0x193a30e], 1 # 0x181ce4f08
00000001803AABFA mov       rdx, qword ptr [r13 + 0x20]
00000001803AABFE test      rdx, rdx
00000001803AAC01 je        0x1803aaeae
00000001803AAC07 mov       r8, qword ptr [rip + 0x17fb52a] # 0x181ba6138
00000001803AAC0E lea       rcx, [rsp + 0x20]
00000001803AAC13 call      0x1808197f0
00000001803AAC18 movups    xmm0, xmmword ptr [rsp + 0x20]
00000001803AAC1D movups    xmmword ptr [rsp + 0x38], xmm0
00000001803AAC22 movsd     xmm1, qword ptr [rsp + 0x30]
00000001803AAC28 movsd     qword ptr [rsp + 0x48], xmm1
00000001803AAC2E mov       qword ptr [rsp + 0x20], 0
00000001803AAC37 lea       rbx, [rsp + 0x38]
00000001803AAC3C mov       qword ptr [rsp + 0x28], rbx
00000001803AAC41 mov       rdx, qword ptr [rip + 0x1816fb8] # 0x181bc1c00
00000001803AAC48 lea       rcx, [rsp + 0x38]
00000001803AAC4D call      0x180669280
00000001803AAC52 test      al, al
00000001803AAC54 je        0x1803aac6d
00000001803AAC56 mov       rcx, qword ptr [rsp + 0x48]
00000001803AAC5B test      rcx, rcx
00000001803AAC5E je        0x1803aae82
00000001803AAC64 xor       edx, edx
00000001803AAC66 call      0x1803438a0
00000001803AAC6B jmp       0x1803aac41
00000001803AAC6D mov       rdx, qword ptr [rip + 0x1816f34] # 0x181bc1ba8
00000001803AAC74 mov       rcx, rbx
00000001803AAC77 call      0x180302170
00000001803AAC7C jmp       0x1803aacad
00000001803AAC7E mov       rdx, qword ptr [rip + 0x1816f23] # 0x181bc1ba8
00000001803AAC85 mov       rcx, qword ptr [rsp + 0x28]
00000001803AAC8A call      0x180302170
00000001803AAC8F mov       rcx, qword ptr [rsp + 0x20]
00000001803AAC94 test      rcx, rcx
00000001803AAC97 jne       0x1803aae88
00000001803AAC9D mov       rsi, qword ptr [rsp + 0xd8]
00000001803AACA5 mov       r13, qword ptr [rsp + 0xd0]
00000001803AACAD xor       r14d, r14d
00000001803AACB0 mov       dword ptr [rsp + 0xe8], r14d
00000001803AACB8 nop       dword ptr [rax + rax]
00000001803AACC0 movups    xmm0, xmmword ptr [rsi]
00000001803AACC3 movups    xmmword ptr [rsp + 0x50], xmm0
00000001803AACC8 movsd     xmm0, qword ptr [rsi + 0x20]
00000001803AACCD movsd     qword ptr [rsp + 0x70], xmm0
00000001803AACD3 mov       rax, qword ptr [rsi + 0x10]
00000001803AACD7 test      rax, rax
00000001803AACDA je        0x1803aaeae
00000001803AACE0 cmp       r14d, dword ptr [rax + 0x18]
00000001803AACE4 jge       0x1803aae6f
00000001803AACEA movups    xmm0, xmmword ptr [rsi]
00000001803AACED movups    xmmword ptr [rsp + 0x50], xmm0
00000001803AACF2 movsd     xmm0, qword ptr [rsi + 0x20]
00000001803AACF7 movsd     qword ptr [rsp + 0x70], xmm0
00000001803AACFD mov       rcx, rax
00000001803AAD00 test      rax, rax
00000001803AAD03 je        0x1803aaeae
00000001803AAD09 mov       r8, qword ptr [rip + 0x1805150] # 0x181bafe60
00000001803AAD10 mov       edx, r14d
00000001803AAD13 call      0x180836790
00000001803AAD18 mov       r12d, eax
00000001803AAD1B mov       rdx, qword ptr [r13 + 0x20]
00000001803AAD1F test      rdx, rdx
00000001803AAD22 je        0x1803aaeae
00000001803AAD28 mov       r8, qword ptr [rip + 0x17fb409] # 0x181ba6138
00000001803AAD2F lea       rcx, [rsp + 0x50]
00000001803AAD34 call      0x1808197f0
00000001803AAD39 movups    xmm0, xmmword ptr [rsp + 0x50]
00000001803AAD3E movups    xmmword ptr [rsp + 0x38], xmm0
00000001803AAD43 movsd     xmm1, qword ptr [rsp + 0x60]
00000001803AAD49 movsd     qword ptr [rsp + 0x48], xmm1
00000001803AAD4F mov       qword ptr [rsp + 0x20], 0
00000001803AAD58 lea       rbx, [rsp + 0x38]
00000001803AAD5D mov       qword ptr [rsp + 0x28], rbx
00000001803AAD62 mov       rdx, qword ptr [rip + 0x1816e97] # 0x181bc1c00
00000001803AAD69 lea       rcx, [rsp + 0x38]
00000001803AAD6E call      0x180669280
00000001803AAD73 test      al, al
00000001803AAD75 je        0x1803aae23
00000001803AAD7B mov       rdi, qword ptr [rsp + 0x48]
00000001803AAD80 test      rdi, rdi
00000001803AAD83 je        0x1803aaea2
00000001803AAD89 cmp       dword ptr [rdi + 0x24], r12d
00000001803AAD8D jne       0x1803aad62
00000001803AAD8F xor       edx, edx
00000001803AAD91 mov       rcx, rdi
00000001803AAD94 call      0x180343640
00000001803AAD99 movups    xmm0, xmmword ptr [rsi]
00000001803AAD9C movups    xmmword ptr [rsp + 0x50], xmm0
00000001803AADA1 movsd     xmm0, qword ptr [rsi + 0x20]
00000001803AADA6 movsd     qword ptr [rsp + 0x70], xmm0
00000001803AADAC mov       rax, qword ptr [rsi + 0x18]
00000001803AADB0 test      rax, rax
00000001803AADB3 je        0x1803aae9d
00000001803AADB9 cmp       dword ptr [rax + 0x18], r14d
00000001803AADBD jle       0x1803aad62
00000001803AADBF test      rdi, rdi
00000001803AADC2 je        0x1803aae98
00000001803AADC8 mov       r15, qword ptr [rdi + 0x48]
00000001803AADCC movups    xmm0, xmmword ptr [rsi]
00000001803AADCF movups    xmmword ptr [rsp + 0x50], xmm0
00000001803AADD4 movsd     xmm0, qword ptr [rsi + 0x20]
00000001803AADD9 movsd     qword ptr [rsp + 0x70], xmm0
00000001803AADDF mov       rcx, rax
00000001803AADE2 test      rax, rax
00000001803AADE5 je        0x1803aae93
00000001803AADEB mov       r8, qword ptr [rip + 0x180506e] # 0x181bafe60
00000001803AADF2 mov       edx, r14d
00000001803AADF5 call      0x180836790
00000001803AADFA test      r15, r15
00000001803AADFD je        0x1803aae8e
00000001803AAE03 xor       r8d, r8d
00000001803AAE06 mov       edx, eax
00000001803AAE08 mov       rcx, r15
00000001803AAE0B call      0x180375f80
00000001803AAE10 xor       r8d, r8d
00000001803AAE13 mov       rdx, rax
00000001803AAE16 mov       rcx, rdi
00000001803AAE19 call      0x180344520
00000001803AAE1E jmp       0x1803aad62
00000001803AAE23 mov       rdx, qword ptr [rip + 0x1816d7e] # 0x181bc1ba8
00000001803AAE2A mov       rcx, rbx
00000001803AAE2D call      0x180302170
00000001803AAE32 jmp       0x1803aae67
00000001803AAE34 mov       rdx, qword ptr [rip + 0x1816d6d] # 0x181bc1ba8
00000001803AAE3B mov       rcx, qword ptr [rsp + 0x28]
00000001803AAE40 call      0x180302170
00000001803AAE45 mov       rcx, qword ptr [rsp + 0x20]
00000001803AAE4A test      rcx, rcx
00000001803AAE4D jne       0x1803aaea8
00000001803AAE4F mov       rsi, qword ptr [rsp + 0xd8]
00000001803AAE57 mov       r13, qword ptr [rsp + 0xd0]
00000001803AAE5F mov       r14d, dword ptr [rsp + 0xe8]
00000001803AAE67 inc       r14d
00000001803AAE6A jmp       0x1803aacb0
00000001803AAE6F add       rsp, 0x90
00000001803AAE76 pop       r15
00000001803AAE78 pop       r14
00000001803AAE7A pop       r13
00000001803AAE7C pop       r12
00000001803AAE7E pop       rdi
00000001803AAE7F pop       rsi
00000001803AAE80 pop       rbx
00000001803AAE81 ret       
00000001803AAE82 call      0x180250150
00000001803AAE87 nop       
00000001803AAE88 call      0x180200e60
00000001803AAE8D nop       
00000001803AAE8E call      0x180250150
00000001803AAE93 call      0x180250150
00000001803AAE98 call      0x180250150
00000001803AAE9D call      0x180250150
00000001803AAEA2 call      0x180250150
00000001803AAEA7 nop       
00000001803AAEA8 call      0x180200e60
00000001803AAEAD int3      
00000001803AAEAE call      0x180250150
00000001803AAEB3 int3      
00000001803AAEB4 int3      
00000001803AAEB5 int3      
00000001803AAEB6 int3      
00000001803AAEB7 int3      
00000001803AAEB8 int3      
00000001803AAEB9 int3      
00000001803AAEBA int3      
00000001803AAEBB int3      
00000001803AAEBC int3      
00000001803AAEBD int3      
00000001803AAEBE int3      
00000001803AAEBF int3      