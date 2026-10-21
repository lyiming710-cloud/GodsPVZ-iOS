00000001802FCA30 mov       qword ptr [rsp + 0x18], rbx
00000001802FCA35 mov       qword ptr [rsp + 0x10], rdx
00000001802FCA3A mov       qword ptr [rsp + 8], rcx
00000001802FCA3F push      rsi
00000001802FCA40 push      rdi
00000001802FCA41 push      r14
00000001802FCA43 sub       rsp, 0x80
00000001802FCA4A mov       r14, rdx
00000001802FCA4D mov       rdi, rcx
00000001802FCA50 cmp       byte ptr [rip + 0x19e8057], 0 # 0x181ce4aae
00000001802FCA57 jne       0x1802fcb00
00000001802FCA5D lea       rcx, [rip + 0x18c5eac] # 0x181bc2910
00000001802FCA64 call      0x18024fef0
00000001802FCA69 lea       rcx, [rip + 0x18c65d0] # 0x181bc3040
00000001802FCA70 call      0x18024fef0
00000001802FCA75 lea       rcx, [rip + 0x18c5eec] # 0x181bc2968
00000001802FCA7C call      0x18024fef0
00000001802FCA81 lea       rcx, [rip + 0x18c6610] # 0x181bc3098
00000001802FCA88 call      0x18024fef0
00000001802FCA8D lea       rcx, [rip + 0x18c5f2c] # 0x181bc29c0
00000001802FCA94 call      0x18024fef0
00000001802FCA99 lea       rcx, [rip + 0x18c6650] # 0x181bc30f0
00000001802FCAA0 call      0x18024fef0
00000001802FCAA5 lea       rcx, [rip + 0x18afc4c] # 0x181bac6f8
00000001802FCAAC call      0x18024fef0
00000001802FCAB1 lea       rcx, [rip + 0x18aca80] # 0x181ba9538
00000001802FCAB8 call      0x18024fef0
00000001802FCABD lea       rcx, [rip + 0x18adc54] # 0x181baa718
00000001802FCAC4 call      0x18024fef0
00000001802FCAC9 lea       rcx, [rip + 0x18acac0] # 0x181ba9590
00000001802FCAD0 call      0x18024fef0
00000001802FCAD5 lea       rcx, [rip + 0x18adc94] # 0x181baa770
00000001802FCADC call      0x18024fef0
00000001802FCAE1 lea       rcx, [rip + 0x18b3a18] # 0x181bb0500
00000001802FCAE8 call      0x18024fef0
00000001802FCAED lea       rcx, [rip + 0x18b9984] # 0x181bb6478
00000001802FCAF4 call      0x18024fef0
00000001802FCAF9 mov       byte ptr [rip + 0x19e7fae], 1 # 0x181ce4aae
00000001802FCB00 xorps     xmm0, xmm0
00000001802FCB03 xor       eax, eax
00000001802FCB05 movups    xmmword ptr [rsp + 0x48], xmm0
00000001802FCB0A mov       qword ptr [rsp + 0x58], rax
00000001802FCB0F mov       rdx, qword ptr [rdi + 0x90]
00000001802FCB16 test      rdx, rdx
00000001802FCB19 je        0x1802fcdf1
00000001802FCB1F mov       r8, qword ptr [rip + 0x18aca6a] # 0x181ba9590
00000001802FCB26 lea       rcx, [rsp + 0x30]
00000001802FCB2B call      0x1808197f0
00000001802FCB30 movups    xmm0, xmmword ptr [rsp + 0x30]
00000001802FCB35 movups    xmmword ptr [rsp + 0x60], xmm0
00000001802FCB3A movsd     xmm1, qword ptr [rsp + 0x40]
00000001802FCB40 movsd     qword ptr [rsp + 0x70], xmm1
00000001802FCB46 mov       qword ptr [rsp + 0x30], 0
00000001802FCB4F lea       rbx, [rsp + 0x60]
00000001802FCB54 mov       qword ptr [rsp + 0x38], rbx
00000001802FCB59 nop       dword ptr [rax]
00000001802FCB60 mov       rdx, qword ptr [rip + 0x18c5e01] # 0x181bc2968
00000001802FCB67 lea       rcx, [rsp + 0x60]
00000001802FCB6C call      0x180669280
00000001802FCB71 test      al, al
00000001802FCB73 je        0x1802fcbae
00000001802FCB75 mov       rcx, qword ptr [rsp + 0x70]
00000001802FCB7A test      rcx, rcx
00000001802FCB7D je        0x1802fcdf7
00000001802FCB83 xor       edx, edx
00000001802FCB85 call      0x1813044d0
00000001802FCB8A mov       rsi, rax
00000001802FCB8D mov       rcx, qword ptr [rip + 0x18b396c] # 0x181bb0500
00000001802FCB94 cmp       dword ptr [rcx + 0xe0], 0
00000001802FCB9B jne       0x1802fcba2
00000001802FCB9D call      0x1802501e0
00000001802FCBA2 xor       edx, edx
00000001802FCBA4 mov       rcx, rsi
00000001802FCBA7 call      0x18131e940
00000001802FCBAC jmp       0x1802fcb60
00000001802FCBAE mov       rdx, qword ptr [rip + 0x18c5d5b] # 0x181bc2910
00000001802FCBB5 mov       rcx, rbx
00000001802FCBB8 call      0x180302170
00000001802FCBBD jmp       0x1802fcbee
00000001802FCBBF mov       rdx, qword ptr [rip + 0x18c5d4a] # 0x181bc2910
00000001802FCBC6 mov       rcx, qword ptr [rsp + 0x38]
00000001802FCBCB call      0x180302170
00000001802FCBD0 mov       rcx, qword ptr [rsp + 0x30]
00000001802FCBD5 test      rcx, rcx
00000001802FCBD8 jne       0x1802fcdfd
00000001802FCBDE mov       r14, qword ptr [rsp + 0xa8]
00000001802FCBE6 mov       rdi, qword ptr [rsp + 0xa0]
00000001802FCBEE mov       rcx, qword ptr [rdi + 0x90]
00000001802FCBF5 test      rcx, rcx
00000001802FCBF8 je        0x1802fcdf1
00000001802FCBFE inc       dword ptr [rcx + 0x1c]
00000001802FCC01 mov       r8d, dword ptr [rcx + 0x18]
00000001802FCC05 mov       dword ptr [rcx + 0x18], 0
00000001802FCC0C test      r8d, r8d
00000001802FCC0F jle       0x1802fcc1f
00000001802FCC11 xor       r9d, r9d
00000001802FCC14 xor       edx, edx
00000001802FCC16 mov       rcx, qword ptr [rcx + 0x10]
00000001802FCC1A call      0x180cb86f0
00000001802FCC1F mov       rdx, qword ptr [rdi + 0x98]
00000001802FCC26 test      rdx, rdx
00000001802FCC29 je        0x1802fcdf1
00000001802FCC2F mov       r8, qword ptr [rip + 0x18adb3a] # 0x181baa770
00000001802FCC36 lea       rcx, [rsp + 0x30]
00000001802FCC3B call      0x1808197f0
00000001802FCC40 movups    xmm0, xmmword ptr [rsp + 0x30]
00000001802FCC45 movups    xmmword ptr [rsp + 0x48], xmm0
00000001802FCC4A movsd     xmm1, qword ptr [rsp + 0x40]
00000001802FCC50 movsd     qword ptr [rsp + 0x58], xmm1
00000001802FCC56 mov       qword ptr [rsp + 0x30], 0
00000001802FCC5F lea       rbx, [rsp + 0x48]
00000001802FCC64 mov       qword ptr [rsp + 0x38], rbx
00000001802FCC69 nop       dword ptr [rax]
00000001802FCC70 mov       rdx, qword ptr [rip + 0x18c6421] # 0x181bc3098
00000001802FCC77 lea       rcx, [rsp + 0x48]
00000001802FCC7C call      0x180669280
00000001802FCC81 test      al, al
00000001802FCC83 je        0x1802fccbe
00000001802FCC85 mov       rcx, qword ptr [rsp + 0x58]
00000001802FCC8A test      rcx, rcx
00000001802FCC8D je        0x1802fce03
00000001802FCC93 xor       edx, edx
00000001802FCC95 call      0x1813044d0
00000001802FCC9A mov       rsi, rax
00000001802FCC9D mov       rcx, qword ptr [rip + 0x18b385c] # 0x181bb0500
00000001802FCCA4 cmp       dword ptr [rcx + 0xe0], 0
00000001802FCCAB jne       0x1802fccb2
00000001802FCCAD call      0x1802501e0
00000001802FCCB2 xor       edx, edx
00000001802FCCB4 mov       rcx, rsi
00000001802FCCB7 call      0x18131e940
00000001802FCCBC jmp       0x1802fcc70
00000001802FCCBE mov       rdx, qword ptr [rip + 0x18c637b] # 0x181bc3040
00000001802FCCC5 mov       rcx, rbx
00000001802FCCC8 call      0x180302170
00000001802FCCCD jmp       0x1802fccfe
00000001802FCCCF mov       rdx, qword ptr [rip + 0x18c636a] # 0x181bc3040
00000001802FCCD6 mov       rcx, qword ptr [rsp + 0x38]
00000001802FCCDB call      0x180302170
00000001802FCCE0 mov       rcx, qword ptr [rsp + 0x30]
00000001802FCCE5 test      rcx, rcx
00000001802FCCE8 jne       0x1802fce09
00000001802FCCEE mov       r14, qword ptr [rsp + 0xa8]
00000001802FCCF6 mov       rdi, qword ptr [rsp + 0xa0]
00000001802FCCFE mov       rcx, qword ptr [rdi + 0x98]
00000001802FCD05 test      rcx, rcx
00000001802FCD08 je        0x1802fcdf1
00000001802FCD0E inc       dword ptr [rcx + 0x1c]
00000001802FCD11 mov       r8d, dword ptr [rcx + 0x18]
00000001802FCD15 mov       dword ptr [rcx + 0x18], 0
00000001802FCD1C test      r8d, r8d
00000001802FCD1F jle       0x1802fcd2f
00000001802FCD21 xor       r9d, r9d
00000001802FCD24 xor       edx, edx
00000001802FCD26 mov       rcx, qword ptr [rcx + 0x10]
00000001802FCD2A call      0x180cb86f0
00000001802FCD2F mov       qword ptr [rdi + 0x28], r14
00000001802FCD33 lea       rcx, [rdi + 0x28]
00000001802FCD37 mov       rdx, r14
00000001802FCD3A call      0x18024f360
00000001802FCD3F test      r14, r14
00000001802FCD42 je        0x1802fcdf1
00000001802FCD48 mov       rbx, qword ptr [r14 + 0x30]
00000001802FCD4C xor       ecx, ecx
00000001802FCD4E call      0x1812e27c0
00000001802FCD53 test      rbx, rbx
00000001802FCD56 je        0x1802fcdf1
00000001802FCD5C test      rax, rax
00000001802FCD5F je        0x1802fcdf1
00000001802FCD65 xor       r8d, r8d
00000001802FCD68 movss     xmm1, dword ptr [rbx + 0x1c]
00000001802FCD6D mov       rcx, rax
00000001802FCD70 call      0x1812e2e80
00000001802FCD75 mov       ebx, dword ptr [rbx + 0x18]
00000001802FCD78 mov       rcx, qword ptr [rip + 0x18b96f9] # 0x181bb6478
00000001802FCD7F cmp       dword ptr [rcx + 0xe0], 0
00000001802FCD86 jne       0x1802fcd8d
00000001802FCD88 call      0x1802501e0
00000001802FCD8D xor       edx, edx
00000001802FCD8F mov       ecx, ebx
00000001802FCD91 call      0x18032f280
00000001802FCD96 mov       rbx, qword ptr [rdi + 0x30]
00000001802FCD9A mov       rdx, qword ptr [rip + 0x18af957] # 0x181bac6f8
00000001802FCDA1 mov       rcx, rax
00000001802FCDA4 call      0x18047ba00
00000001802FCDA9 test      rbx, rbx
00000001802FCDAC je        0x1802fcdf1
00000001802FCDAE xor       r8d, r8d
00000001802FCDB1 mov       rdx, rax
00000001802FCDB4 mov       rcx, rbx
00000001802FCDB7 call      0x1813290f0
00000001802FCDBC xor       edx, edx
00000001802FCDBE mov       rcx, rdi
00000001802FCDC1 call      0x1802fba50
00000001802FCDC6 xor       edx, edx
00000001802FCDC8 mov       rcx, rdi
00000001802FCDCB call      0x1802fbce0
00000001802FCDD0 xor       r8d, r8d
00000001802FCDD3 xor       edx, edx
00000001802FCDD5 mov       rcx, rdi
00000001802FCDD8 call      0x1802fb330
00000001802FCDDD mov       rbx, qword ptr [rsp + 0xb0]
00000001802FCDE5 add       rsp, 0x80
00000001802FCDEC pop       r14
00000001802FCDEE pop       rdi
00000001802FCDEF pop       rsi
00000001802FCDF0 ret       
00000001802FCDF1 call      0x180250150
00000001802FCDF6 nop       
00000001802FCDF7 call      0x180250150
00000001802FCDFC nop       
00000001802FCDFD call      0x180200e60
00000001802FCE02 nop       
00000001802FCE03 call      0x180250150
00000001802FCE08 nop       
00000001802FCE09 call      0x180200e60
00000001802FCE0E int3      
00000001802FCE0F int3      