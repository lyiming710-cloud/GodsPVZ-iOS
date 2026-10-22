000000018034EEA0 mov       qword ptr [rsp + 0x10], rbx
000000018034EEA5 mov       qword ptr [rsp + 8], rcx
000000018034EEAA push      rsi
000000018034EEAB push      rdi
000000018034EEAC push      r12
000000018034EEAE push      r14
000000018034EEB0 push      r15
000000018034EEB2 sub       rsp, 0x80
000000018034EEB9 movaps    xmmword ptr [rsp + 0x70], xmm6
000000018034EEBE mov       rsi, rcx
000000018034EEC1 cmp       byte ptr [rip + 0x1995e34], 0 # 0x181ce4cfc
000000018034EEC8 jne       0x18034ef19
000000018034EECA lea       rcx, [rip + 0x1869237] # 0x181bb8108
000000018034EED1 call      0x18024fef0
000000018034EED6 lea       rcx, [rip + 0x1873823] # 0x181bc2700
000000018034EEDD call      0x18024fef0
000000018034EEE2 lea       rcx, [rip + 0x187386f] # 0x181bc2758
000000018034EEE9 call      0x18024fef0
000000018034EEEE lea       rcx, [rip + 0x18738bb] # 0x181bc27b0
000000018034EEF5 call      0x18024fef0
000000018034EEFA lea       rcx, [rip + 0x185a1bf] # 0x181ba90c0
000000018034EF01 call      0x18024fef0
000000018034EF06 lea       rcx, [rip + 0x18615f3] # 0x181bb0500
000000018034EF0D call      0x18024fef0
000000018034EF12 mov       byte ptr [rip + 0x1995de3], 1 # 0x181ce4cfc
000000018034EF19 mov       rcx, qword ptr [rsi + 0x70]
000000018034EF1D cmp       byte ptr [rsi + 0xe8], 0
000000018034EF24 jne       0x18034ef37
000000018034EF26 test      rcx, rcx
000000018034EF29 je        0x18034f389
000000018034EF2F xor       r12d, r12d
000000018034EF32 mov       edx, r12d
000000018034EF35 jmp       0x18034ef56
000000018034EF37 mov       rax, qword ptr [rsi + 0xe0]
000000018034EF3E test      rax, rax
000000018034EF41 je        0x18034f389
000000018034EF47 mov       edx, dword ptr [rax + 0x18]
000000018034EF4A test      rcx, rcx
000000018034EF4D je        0x18034f389
000000018034EF53 xor       r12d, r12d
000000018034EF56 xor       r8d, r8d
000000018034EF59 call      0x1803944e0
000000018034EF5E mov       r15, rax
000000018034EF61 mov       eax, dword ptr [rsi + 0x7c]
000000018034EF64 test      eax, eax
000000018034EF66 jne       0x18034f1ce
000000018034EF6C cmp       byte ptr [rsi + 0xf4], al
000000018034EF72 je        0x18034f021
000000018034EF78 mov       rax, qword ptr [rsi + 0xe0]
000000018034EF7F test      rax, rax
000000018034EF82 je        0x18034f389
000000018034EF88 cmp       dword ptr [rax + 0x18], 1
000000018034EF8C jne       0x18034f021
000000018034EF92 mov       rbx, qword ptr [rsi + 0x120]
000000018034EF99 mov       rcx, qword ptr [rip + 0x1861560] # 0x181bb0500
000000018034EFA0 cmp       dword ptr [rcx + 0xe0], 0
000000018034EFA7 jne       0x18034efae
000000018034EFA9 call      0x1802501e0
000000018034EFAE xor       r8d, r8d
000000018034EFB1 xor       edx, edx
000000018034EFB3 mov       rcx, rbx
000000018034EFB6 call      0x18131f900
000000018034EFBB test      al, al
000000018034EFBD jne       0x18034f002
000000018034EFBF mov       rbx, qword ptr [rsi + 0x128]
000000018034EFC6 mov       rcx, qword ptr [rip + 0x1861533] # 0x181bb0500
000000018034EFCD cmp       dword ptr [rcx + 0xe0], 0
000000018034EFD4 jne       0x18034efdb
000000018034EFD6 call      0x1802501e0
000000018034EFDB xor       edx, edx
000000018034EFDD mov       rcx, rbx
000000018034EFE0 call      0x18131f870
000000018034EFE5 test      al, al
000000018034EFE7 je        0x18034f002
000000018034EFE9 mov       rax, qword ptr [rsi + 0x128]
000000018034EFF0 test      rax, rax
000000018034EFF3 je        0x18034f389
000000018034EFF9 cmp       byte ptr [rax + 0x95], 0
000000018034F000 je        0x18034f004
000000018034F002 xor       eax, eax
000000018034F004 mov       rbx, qword ptr [rsp + 0xb8]
000000018034F00C movaps    xmm6, xmmword ptr [rsp + 0x70]
000000018034F011 add       rsp, 0x80
000000018034F018 pop       r15
000000018034F01A pop       r14
000000018034F01C pop       r12
000000018034F01E pop       rdi
000000018034F01F pop       rsi
000000018034F020 ret       
000000018034F021 mov       r14, r12
000000018034F024 mov       qword ptr [rsp + 0xc0], r12
000000018034F02C movss     xmm6, dword ptr [rip + 0x1258c88] # 0x1815a7cbc
000000018034F034 mov       rax, qword ptr [rsi + 0x1e8]
000000018034F03B test      rax, rax
000000018034F03E je        0x18034f389
000000018034F044 mov       rcx, qword ptr [rax + 0x108]
000000018034F04B test      rcx, rcx
000000018034F04E je        0x18034f389
000000018034F054 mov       rdx, qword ptr [rcx + 0x30]
000000018034F058 test      rdx, rdx
000000018034F05B je        0x18034f389
000000018034F061 mov       r8, qword ptr [rip + 0x185a058] # 0x181ba90c0
000000018034F068 lea       rcx, [rsp + 0x40]
000000018034F06D call      0x1808197f0
000000018034F072 movups    xmm0, xmmword ptr [rsp + 0x40]
000000018034F077 movups    xmmword ptr [rsp + 0x58], xmm0
000000018034F07C movsd     xmm1, qword ptr [rsp + 0x50]
000000018034F082 movsd     qword ptr [rsp + 0x68], xmm1
000000018034F088 mov       qword ptr [rsp + 0x40], r12
000000018034F08D lea       rbx, [rsp + 0x58]
000000018034F092 mov       qword ptr [rsp + 0x48], rbx
000000018034F097 nop       word ptr [rax + rax]
000000018034F0A0 mov       rdx, qword ptr [rip + 0x18736b1] # 0x181bc2758
000000018034F0A7 lea       rcx, [rsp + 0x58]
000000018034F0AC call      0x180669280
000000018034F0B1 test      al, al
000000018034F0B3 je        0x18034f16c
000000018034F0B9 mov       rdi, qword ptr [rsp + 0x68]
000000018034F0BE mov       qword ptr [rsp + 0x20], r12
000000018034F0C3 xor       r9d, r9d
000000018034F0C6 xor       edx, edx
000000018034F0C8 mov       r8d, 0xffffffff
000000018034F0CE mov       rcx, rsi
000000018034F0D1 call      0x180350e40
000000018034F0D6 test      rdi, rdi
000000018034F0D9 je        0x18034f399
000000018034F0DF cmp       dword ptr [rdi + 0x5c], 2
000000018034F0E3 je        0x18034f0a0
000000018034F0E5 cmp       dword ptr [rdi + 0x5c], 7
000000018034F0E9 je        0x18034f0a0
000000018034F0EB cmp       dword ptr [rdi + 0x5c], 8
000000018034F0EF je        0x18034f0a0
000000018034F0F1 cmp       dword ptr [rdi + 0x5c], 0xc
000000018034F0F5 je        0x18034f0a0
000000018034F0F7 cmp       dword ptr [rdi + 0x5c], 0xe
000000018034F0FB je        0x18034f0a0
000000018034F0FD cmp       dword ptr [rdi + 0x5c], 0xb
000000018034F101 je        0x18034f10c
000000018034F103 cmp       byte ptr [rdi + 0x95], 0
000000018034F10A jmp       0x18034f119
000000018034F10C test      rax, rax
000000018034F10F je        0x18034f394
000000018034F115 cmp       dword ptr [rax + 0x18], 3
000000018034F119 jne       0x18034f0a0
000000018034F11B mov       eax, dword ptr [rdi + 0x58]
000000018034F11E cmp       eax, dword ptr [rsi + 0xbc]
000000018034F124 je        0x18034f0a0
000000018034F12A test      r15, r15
000000018034F12D je        0x18034f38f
000000018034F133 mov       r8, qword ptr [rip + 0x1868fce] # 0x181bb8108
000000018034F13A mov       rdx, rdi
000000018034F13D mov       rcx, r15
000000018034F140 call      0x180426dd0
000000018034F145 test      al, al
000000018034F147 je        0x18034f0a0
000000018034F14D comiss    xmm6, dword ptr [rdi + 0x38]
000000018034F151 jbe       0x18034f0a0
000000018034F157 mov       r14, rdi
000000018034F15A mov       qword ptr [rsp + 0xc0], rdi
000000018034F162 movss     xmm6, dword ptr [rdi + 0x38]
000000018034F167 jmp       0x18034f0a0
000000018034F16C mov       rdx, qword ptr [rip + 0x187358d] # 0x181bc2700
000000018034F173 mov       rcx, rbx
000000018034F176 call      0x180302170
000000018034F17B jmp       0x18034f1ac
000000018034F17D mov       rdx, qword ptr [rip + 0x187357c] # 0x181bc2700
000000018034F184 mov       rcx, qword ptr [rsp + 0x48]
000000018034F189 call      0x180302170
000000018034F18E mov       rcx, qword ptr [rsp + 0x40]
000000018034F193 test      rcx, rcx
000000018034F196 jne       0x18034f39f
000000018034F19C mov       rsi, qword ptr [rsp + 0xb0]
000000018034F1A4 mov       r14, qword ptr [rsp + 0xc0]
000000018034F1AC mov       qword ptr [rsi + 0x128], r14
000000018034F1B3 lea       rcx, [rsi + 0x128]
000000018034F1BA mov       rdx, r14
000000018034F1BD call      0x18024f360
000000018034F1C2 mov       rax, qword ptr [rsi + 0x128]
000000018034F1C9 jmp       0x18034f004
000000018034F1CE cmp       eax, 1
000000018034F1D1 je        0x18034f002
000000018034F1D7 mov       r14, r12
000000018034F1DA mov       qword ptr [rsp + 0xc0], r12
000000018034F1E2 movss     xmm6, dword ptr [rip + 0x1258ad2] # 0x1815a7cbc
000000018034F1EA mov       rax, qword ptr [rsi + 0x1e8]
000000018034F1F1 test      rax, rax
000000018034F1F4 je        0x18034f389
000000018034F1FA mov       rcx, qword ptr [rax + 0x108]
000000018034F201 test      rcx, rcx
000000018034F204 je        0x18034f389
000000018034F20A mov       rdx, qword ptr [rcx + 0x30]
000000018034F20E test      rdx, rdx
000000018034F211 je        0x18034f389
000000018034F217 mov       r8, qword ptr [rip + 0x1859ea2] # 0x181ba90c0
000000018034F21E lea       rcx, [rsp + 0x40]
000000018034F223 call      0x1808197f0
000000018034F228 movups    xmm0, xmmword ptr [rsp + 0x40]
000000018034F22D movups    xmmword ptr [rsp + 0x58], xmm0
000000018034F232 movsd     xmm1, qword ptr [rsp + 0x50]
000000018034F238 movsd     qword ptr [rsp + 0x68], xmm1
000000018034F23E mov       qword ptr [rsp + 0x40], r12
000000018034F243 lea       rbx, [rsp + 0x58]
000000018034F248 mov       qword ptr [rsp + 0x48], rbx
000000018034F24D nop       dword ptr [rax]
000000018034F250 mov       rdx, qword ptr [rip + 0x1873501] # 0x181bc2758
000000018034F257 lea       rcx, [rsp + 0x58]
000000018034F25C call      0x180669280
000000018034F261 test      al, al
000000018034F263 je        0x18034f355
000000018034F269 mov       rdi, qword ptr [rsp + 0x68]
000000018034F26E mov       r8d, 0xffffffff
000000018034F274 mov       ecx, dword ptr [rsi + 0x7c]
000000018034F277 add       ecx, -4
000000018034F27A je        0x18034f29b
000000018034F27C sub       ecx, 1
000000018034F27F je        0x18034f293
000000018034F281 sub       ecx, 1
000000018034F284 je        0x18034f2a1
000000018034F286 cmp       ecx, 1
000000018034F289 jne       0x18034f2a1
000000018034F28B mov       r8d, 0x18
000000018034F291 jmp       0x18034f2a1
000000018034F293 mov       r8d, 1
000000018034F299 jmp       0x18034f2a1
000000018034F29B mov       r8d, 0xf
000000018034F2A1 mov       qword ptr [rsp + 0x20], r12
000000018034F2A6 xor       r9d, r9d
000000018034F2A9 xor       edx, edx
000000018034F2AB mov       rcx, rsi
000000018034F2AE call      0x180350e40
000000018034F2B3 test      rdi, rdi
000000018034F2B6 je        0x18034f3af
000000018034F2BC cmp       dword ptr [rdi + 0x5c], 2
000000018034F2C0 je        0x18034f250
000000018034F2C2 cmp       dword ptr [rdi + 0x5c], 7
000000018034F2C6 je        0x18034f250
000000018034F2C8 cmp       dword ptr [rdi + 0x5c], 8
000000018034F2CC je        0x18034f250
000000018034F2CE cmp       dword ptr [rdi + 0x5c], 0xc
000000018034F2D2 je        0x18034f250
000000018034F2D8 cmp       dword ptr [rdi + 0x5c], 0xe
000000018034F2DC je        0x18034f250
000000018034F2E2 cmp       dword ptr [rdi + 0x5c], 0xb
000000018034F2E6 je        0x18034f2f1
000000018034F2E8 cmp       byte ptr [rdi + 0x95], 0
000000018034F2EF jmp       0x18034f2fe
000000018034F2F1 test      rax, rax
000000018034F2F4 je        0x18034f3aa
000000018034F2FA cmp       dword ptr [rax + 0x18], 3
000000018034F2FE jne       0x18034f250
000000018034F304 mov       eax, dword ptr [rdi + 0x58]
000000018034F307 cmp       eax, dword ptr [rsi + 0xbc]
000000018034F30D je        0x18034f250
000000018034F313 test      r15, r15
000000018034F316 je        0x18034f3a5
000000018034F31C mov       r8, qword ptr [rip + 0x1868de5] # 0x181bb8108
000000018034F323 mov       rdx, rdi
000000018034F326 mov       rcx, r15
000000018034F329 call      0x180426dd0
000000018034F32E test      al, al
000000018034F330 je        0x18034f250
000000018034F336 comiss    xmm6, dword ptr [rdi + 0x38]
000000018034F33A jbe       0x18034f250
000000018034F340 mov       r14, rdi
000000018034F343 mov       qword ptr [rsp + 0xc0], rdi
000000018034F34B movss     xmm6, dword ptr [rdi + 0x38]
000000018034F350 jmp       0x18034f250
000000018034F355 mov       rdx, qword ptr [rip + 0x18733a4] # 0x181bc2700
000000018034F35C mov       rcx, rbx
000000018034F35F call      0x180302170
000000018034F364 jmp       0x18034f1ac
000000018034F369 mov       rdx, qword ptr [rip + 0x1873390] # 0x181bc2700
000000018034F370 mov       rcx, qword ptr [rsp + 0x48]
000000018034F375 call      0x180302170
000000018034F37A mov       rcx, qword ptr [rsp + 0x40]
000000018034F37F test      rcx, rcx
000000018034F382 jne       0x18034f3b5
000000018034F384 jmp       0x18034f19c
000000018034F389 call      0x180250150
000000018034F38E nop       
000000018034F38F call      0x180250150
000000018034F394 call      0x180250150
000000018034F399 call      0x180250150
000000018034F39E nop       
000000018034F39F call      0x180200e60
000000018034F3A4 nop       
000000018034F3A5 call      0x180250150
000000018034F3AA call      0x180250150
000000018034F3AF call      0x180250150
000000018034F3B4 nop       
000000018034F3B5 call      0x180200e60
000000018034F3BA int3      
000000018034F3BB int3      
000000018034F3BC int3      
000000018034F3BD int3      
000000018034F3BE int3      
000000018034F3BF int3      