000000018032BF30 mov       qword ptr [rsp + 0x10], rbx
000000018032BF35 mov       qword ptr [rsp + 0x18], rsi
000000018032BF3A push      rdi
000000018032BF3B sub       rsp, 0xa0
000000018032BF42 mov       rbx, rcx
000000018032BF45 cmp       byte ptr [rip + 0x19b8d1e], 0 # 0x181ce4c6a
000000018032BF4C jne       0x18032bfdd
000000018032BF52 lea       rcx, [rip + 0x189ba1f] # 0x181bc7978
000000018032BF59 call      0x18024fef0
000000018032BF5E lea       rcx, [rip + 0x1897813] # 0x181bc3778
000000018032BF65 call      0x18024fef0
000000018032BF6A lea       rcx, [rip + 0x189785f] # 0x181bc37d0
000000018032BF71 call      0x18024fef0
000000018032BF76 lea       rcx, [rip + 0x189ba53] # 0x181bc79d0
000000018032BF7D call      0x18024fef0
000000018032BF82 lea       rcx, [rip + 0x189ba9f] # 0x181bc7a28
000000018032BF89 call      0x18024fef0
000000018032BF8E lea       rcx, [rip + 0x1897893] # 0x181bc3828
000000018032BF95 call      0x18024fef0
000000018032BF9A lea       rcx, [rip + 0x1880c97] # 0x181bacc38
000000018032BFA1 call      0x18024fef0
000000018032BFA6 lea       rcx, [rip + 0x188c13b] # 0x181bb80e8
000000018032BFAD call      0x18024fef0
000000018032BFB2 lea       rcx, [rip + 0x1880cd7] # 0x181bacc90
000000018032BFB9 call      0x18024fef0
000000018032BFBE lea       rcx, [rip + 0x1880c1b] # 0x181bacbe0
000000018032BFC5 call      0x18024fef0
000000018032BFCA lea       rcx, [rip + 0x1891e2f] # 0x181bbde00
000000018032BFD1 call      0x18024fef0
000000018032BFD6 mov       byte ptr [rip + 0x19b8c8d], 1 # 0x181ce4c6a
000000018032BFDD xorps     xmm0, xmm0
000000018032BFE0 xor       eax, eax
000000018032BFE2 movups    xmmword ptr [rsp + 0x48], xmm0
000000018032BFE7 mov       qword ptr [rsp + 0x58], rax
000000018032BFEC mov       rcx, qword ptr [rip + 0x1891e0d] # 0x181bbde00
000000018032BFF3 call      0x180250100
000000018032BFF8 mov       rdi, rax
000000018032BFFB mov       rdx, qword ptr [rip + 0x1880bde] # 0x181bacbe0
000000018032C002 mov       rcx, rax
000000018032C005 call      0x1808053d0
000000018032C00A mov       qword ptr [rsp + 0xb0], rdi
000000018032C012 mov       rdx, qword ptr [rbx + 0x10]
000000018032C016 test      rdx, rdx
000000018032C019 je        0x18032c193
000000018032C01F mov       r8, qword ptr [rip + 0x188c0c2] # 0x181bb80e8
000000018032C026 lea       rcx, [rsp + 0x30]
000000018032C02B call      0x1808197f0
000000018032C030 movups    xmm0, xmmword ptr [rsp + 0x30]
000000018032C035 movups    xmmword ptr [rsp + 0x60], xmm0
000000018032C03A movsd     xmm1, qword ptr [rsp + 0x40]
000000018032C040 movsd     qword ptr [rsp + 0x70], xmm1
000000018032C046 xor       esi, esi
000000018032C048 mov       qword ptr [rsp + 0x20], rsi
000000018032C04D lea       rax, [rsp + 0x60]
000000018032C052 mov       qword ptr [rsp + 0x28], rax
000000018032C057 nop       word ptr [rax + rax]
000000018032C060 mov       rdx, qword ptr [rip + 0x189b969] # 0x181bc79d0
000000018032C067 lea       rcx, [rsp + 0x60]
000000018032C06C call      0x180669280
000000018032C071 test      al, al
000000018032C073 je        0x18032c145
000000018032C079 mov       rax, qword ptr [rsp + 0x70]
000000018032C07E test      rax, rax
000000018032C081 je        0x18032c1a9
000000018032C087 mov       rdx, qword ptr [rax + 0x18]
000000018032C08B test      rdx, rdx
000000018032C08E je        0x18032c1a4
000000018032C094 mov       r8, qword ptr [rip + 0x1880bf5] # 0x181bacc90
000000018032C09B lea       rcx, [rsp + 0x88]
000000018032C0A3 call      0x1808197f0
000000018032C0A8 movups    xmm0, xmmword ptr [rsp + 0x88]
000000018032C0B0 movups    xmmword ptr [rsp + 0x48], xmm0
000000018032C0B5 movsd     xmm1, qword ptr [rsp + 0x98]
000000018032C0BE movsd     qword ptr [rsp + 0x58], xmm1
000000018032C0C4 mov       qword ptr [rsp + 0x30], rsi
000000018032C0C9 lea       rbx, [rsp + 0x48]
000000018032C0CE mov       qword ptr [rsp + 0x38], rbx
000000018032C0D3 mov       rdx, qword ptr [rip + 0x18976f6] # 0x181bc37d0
000000018032C0DA lea       rcx, [rsp + 0x48]
000000018032C0DF call      0x180669280
000000018032C0E4 test      al, al
000000018032C0E6 je        0x18032c107
000000018032C0E8 test      rdi, rdi
000000018032C0EB je        0x18032c199
000000018032C0F1 mov       r8, qword ptr [rip + 0x1880b40] # 0x181bacc38
000000018032C0F8 mov       rdx, qword ptr [rsp + 0x58]
000000018032C0FD mov       rcx, rdi
000000018032C100 call      0x180002e70
000000018032C105 jmp       0x18032c0d3
000000018032C107 mov       rdx, qword ptr [rip + 0x189766a] # 0x181bc3778
000000018032C10E mov       rcx, rbx
000000018032C111 call      0x180302170
000000018032C116 jmp       0x18032c060
000000018032C11B mov       rdx, qword ptr [rip + 0x1897656] # 0x181bc3778
000000018032C122 mov       rcx, qword ptr [rsp + 0x38]
000000018032C127 call      0x180302170
000000018032C12C mov       rcx, qword ptr [rsp + 0x30]
000000018032C131 test      rcx, rcx
000000018032C134 jne       0x18032c19f
000000018032C136 xor       esi, esi
000000018032C138 mov       rdi, qword ptr [rsp + 0xb0]
000000018032C140 jmp       0x18032c060
000000018032C145 mov       rdx, qword ptr [rip + 0x189b82c] # 0x181bc7978
000000018032C14C mov       rcx, qword ptr [rsp + 0x28]
000000018032C151 call      0x180302170
000000018032C156 jmp       0x18032c17b
000000018032C158 mov       rdx, qword ptr [rip + 0x189b819] # 0x181bc7978
000000018032C15F mov       rcx, qword ptr [rsp + 0x28]
000000018032C164 call      0x180302170
000000018032C169 mov       rcx, qword ptr [rsp + 0x20]
000000018032C16E test      rcx, rcx
000000018032C171 jne       0x18032c1af
000000018032C173 mov       rdi, qword ptr [rsp + 0xb0]
000000018032C17B mov       rax, rdi
000000018032C17E lea       r11, [rsp + 0xa0]
000000018032C186 mov       rbx, qword ptr [r11 + 0x18]
000000018032C18A mov       rsi, qword ptr [r11 + 0x20]
000000018032C18E mov       rsp, r11
000000018032C191 pop       rdi
000000018032C192 ret       
000000018032C193 call      0x180250150
000000018032C198 nop       
000000018032C199 call      0x180250150
000000018032C19E nop       
000000018032C19F call      0x180200e60
000000018032C1A4 call      0x180250150
000000018032C1A9 call      0x180250150
000000018032C1AE nop       
000000018032C1AF call      0x180200e60
000000018032C1B4 int3      
000000018032C1B5 int3      
000000018032C1B6 int3      
000000018032C1B7 int3      
000000018032C1B8 int3      
000000018032C1B9 int3      
000000018032C1BA int3      
000000018032C1BB int3      
000000018032C1BC int3      
000000018032C1BD int3      
000000018032C1BE int3      
000000018032C1BF int3      