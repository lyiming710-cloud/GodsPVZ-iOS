000000018037EF20 mov       qword ptr [rsp + 0x10], rbx
000000018037EF25 mov       qword ptr [rsp + 0x18], rsi
000000018037EF2A mov       qword ptr [rsp + 0x20], rdi
000000018037EF2F mov       qword ptr [rsp + 8], rcx
000000018037EF34 push      r14
000000018037EF36 sub       rsp, 0x60
000000018037EF3A mov       rdi, rcx
000000018037EF3D cmp       byte ptr [rip + 0x1965e9f], 0 # 0x181ce4de3
000000018037EF44 jne       0x18037efad
000000018037EF46 lea       rcx, [rip + 0x1847ab3] # 0x181bc6a00
000000018037EF4D call      0x18024fef0
000000018037EF52 lea       rcx, [rip + 0x1847aff] # 0x181bc6a58
000000018037EF59 call      0x18024fef0
000000018037EF5E lea       rcx, [rip + 0x1847b4b] # 0x181bc6ab0
000000018037EF65 call      0x18024fef0
000000018037EF6A lea       rcx, [rip + 0x18230a7] # 0x181ba2018
000000018037EF71 call      0x18024fef0
000000018037EF76 lea       rcx, [rip + 0x1835c23] # 0x181bb4ba0
000000018037EF7D call      0x18024fef0
000000018037EF82 lea       rcx, [rip + 0x183e2cf] # 0x181bbd258
000000018037EF89 call      0x18024fef0
000000018037EF8E lea       rcx, [rip + 0x183e31b] # 0x181bbd2b0
000000018037EF95 call      0x18024fef0
000000018037EF9A lea       rcx, [rip + 0x183e367] # 0x181bbd308
000000018037EFA1 call      0x18024fef0
000000018037EFA6 mov       byte ptr [rip + 0x1965e36], 1 # 0x181ce4de3
000000018037EFAD mov       rdx, qword ptr [rdi + 0x60]
000000018037EFB1 test      rdx, rdx
000000018037EFB4 je        0x18037f194
000000018037EFBA mov       r8, qword ptr [rip + 0x1835bdf] # 0x181bb4ba0
000000018037EFC1 lea       rcx, [rsp + 0x28]
000000018037EFC6 call      0x1808197f0
000000018037EFCB movups    xmm0, xmmword ptr [rsp + 0x28]
000000018037EFD0 movups    xmmword ptr [rsp + 0x40], xmm0
000000018037EFD5 movsd     xmm1, qword ptr [rsp + 0x38]
000000018037EFDB movsd     qword ptr [rsp + 0x50], xmm1
000000018037EFE1 mov       qword ptr [rsp + 0x28], 0
000000018037EFEA lea       rbx, [rsp + 0x40]
000000018037EFEF mov       qword ptr [rsp + 0x30], rbx
000000018037EFF4 mov       rdx, qword ptr [rip + 0x1847a5d] # 0x181bc6a58
000000018037EFFB lea       rcx, [rsp + 0x40]
000000018037F000 call      0x180669280
000000018037F005 test      al, al
000000018037F007 je        0x18037f05a
000000018037F009 mov       rsi, qword ptr [rsp + 0x50]
000000018037F00E mov       rax, qword ptr [rip + 0x1823003] # 0x181ba2018
000000018037F015 cmp       dword ptr [rax + 0xe0], 0
000000018037F01C jne       0x18037f02d
000000018037F01E mov       rcx, rax
000000018037F021 call      0x1802501e0
000000018037F026 mov       rax, qword ptr [rip + 0x1822feb] # 0x181ba2018
000000018037F02D mov       rax, qword ptr [rax + 0xb8]
000000018037F034 mov       rcx, qword ptr [rax]
000000018037F037 test      rcx, rcx
000000018037F03A je        0x18037f1a0
000000018037F040 mov       rcx, qword ptr [rcx + 0x28]
000000018037F044 test      rcx, rcx
000000018037F047 je        0x18037f19a
000000018037F04D xor       r8d, r8d
000000018037F050 mov       rdx, rsi
000000018037F053 call      0x180340020
000000018037F058 jmp       0x18037eff4
000000018037F05A mov       rdx, qword ptr [rip + 0x184799f] # 0x181bc6a00
000000018037F061 mov       rcx, rbx
000000018037F064 call      0x180302170
000000018037F069 jmp       0x18037f090
000000018037F06B mov       rdx, qword ptr [rip + 0x184798e] # 0x181bc6a00
000000018037F072 mov       rcx, qword ptr [rsp + 0x30]
000000018037F077 call      0x180302170
000000018037F07C mov       rcx, qword ptr [rsp + 0x28]
000000018037F081 test      rcx, rcx
000000018037F084 jne       0x18037f1a6
000000018037F08A mov       rdi, qword ptr [rsp + 0x70]
000000018037F08F nop       
000000018037F090 xor       r14b, r14b
000000018037F093 xor       ebx, ebx
000000018037F095 nop       word ptr [rax + rax]
000000018037F0A0 mov       rax, qword ptr [rdi + 0x78]
000000018037F0A4 test      rax, rax
000000018037F0A7 je        0x18037f194
000000018037F0AD mov       eax, dword ptr [rax + 0x18]
000000018037F0B0 dec       eax
000000018037F0B2 cmp       ebx, eax
000000018037F0B4 jge       0x18037f174
000000018037F0BA mov       r8, qword ptr [rip + 0x183e1ef] # 0x181bbd2b0
000000018037F0C1 mov       edx, ebx
000000018037F0C3 mov       rcx, qword ptr [rdi + 0x78]
000000018037F0C7 call      0x180825710
000000018037F0CC test      rax, rax
000000018037F0CF je        0x18037f194
000000018037F0D5 mov       esi, dword ptr [rax + 0x18]
000000018037F0D8 mov       rcx, qword ptr [rdi + 0x78]
000000018037F0DC test      rcx, rcx
000000018037F0DF je        0x18037f194
000000018037F0E5 lea       edx, [rbx + 1]
000000018037F0E8 mov       r8, qword ptr [rip + 0x183e1c1] # 0x181bbd2b0
000000018037F0EF call      0x180825710
000000018037F0F4 test      rax, rax
000000018037F0F7 je        0x18037f194
000000018037F0FD cmp       esi, dword ptr [rax + 0x18]
000000018037F100 jle       0x18037f16d
000000018037F102 mov       rcx, qword ptr [rdi + 0x78]
000000018037F106 test      rcx, rcx
000000018037F109 je        0x18037f194
000000018037F10F mov       r8, qword ptr [rip + 0x183e19a] # 0x181bbd2b0
000000018037F116 mov       edx, ebx
000000018037F118 call      0x180825710
000000018037F11D mov       rsi, rax
000000018037F120 mov       r14, qword ptr [rdi + 0x78]
000000018037F124 mov       rcx, r14
000000018037F127 test      r14, r14
000000018037F12A je        0x18037f194
000000018037F12C lea       edx, [rbx + 1]
000000018037F12F mov       r8, qword ptr [rip + 0x183e17a] # 0x181bbd2b0
000000018037F136 call      0x180825710
000000018037F13B mov       r9, qword ptr [rip + 0x183e1c6] # 0x181bbd308
000000018037F142 mov       r8, rax
000000018037F145 mov       edx, ebx
000000018037F147 mov       rcx, r14
000000018037F14A call      0x180855ab0
000000018037F14F mov       rcx, qword ptr [rdi + 0x78]
000000018037F153 test      rcx, rcx
000000018037F156 je        0x18037f194
000000018037F158 lea       edx, [rbx + 1]
000000018037F15B mov       r9, qword ptr [rip + 0x183e1a6] # 0x181bbd308
000000018037F162 mov       r8, rsi
000000018037F165 call      0x180855ab0
000000018037F16A mov       r14b, 1
000000018037F16D inc       ebx
000000018037F16F jmp       0x18037f0a0
000000018037F174 test      r14b, r14b
000000018037F177 jne       0x18037f090
000000018037F17D lea       r11, [rsp + 0x60]
000000018037F182 mov       rbx, qword ptr [r11 + 0x18]
000000018037F186 mov       rsi, qword ptr [r11 + 0x20]
000000018037F18A mov       rdi, qword ptr [r11 + 0x28]
000000018037F18E mov       rsp, r11
000000018037F191 pop       r14
000000018037F193 ret       
000000018037F194 call      0x180250150
000000018037F199 nop       
000000018037F19A call      0x180250150
000000018037F19F nop       
000000018037F1A0 call      0x180250150
000000018037F1A5 nop       
000000018037F1A6 call      0x180200e60
000000018037F1AB int3      
000000018037F1AC int3      
000000018037F1AD int3      
000000018037F1AE int3      
000000018037F1AF int3      