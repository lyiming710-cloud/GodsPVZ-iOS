00000001802FBA50 mov       qword ptr [rsp + 8], rbx
00000001802FBA55 mov       qword ptr [rsp + 0x10], rsi
00000001802FBA5A mov       qword ptr [rsp + 0x18], rdi
00000001802FBA5F mov       qword ptr [rsp + 0x20], r14
00000001802FBA64 push      r15
00000001802FBA66 sub       rsp, 0x60
00000001802FBA6A mov       rsi, rcx
00000001802FBA6D cmp       byte ptr [rip + 0x19e903c], 0 # 0x181ce4ab0
00000001802FBA74 jne       0x1802fbad1
00000001802FBA76 lea       rcx, [rip + 0x18cb29b] # 0x181bc6d18
00000001802FBA7D call      0x18024fef0
00000001802FBA82 lea       rcx, [rip + 0x18cb2e7] # 0x181bc6d70
00000001802FBA89 call      0x18024fef0
00000001802FBA8E lea       rcx, [rip + 0x18cb333] # 0x181bc6dc8
00000001802FBA95 call      0x18024fef0
00000001802FBA9A lea       rcx, [rip + 0x18ada3f] # 0x181ba94e0
00000001802FBAA1 call      0x18024fef0
00000001802FBAA6 lea       rcx, [rip + 0x18b99e3] # 0x181bb5490
00000001802FBAAD call      0x18024fef0
00000001802FBAB2 lea       rcx, [rip + 0x18bbf8f] # 0x181bb7a48
00000001802FBAB9 call      0x18024fef0
00000001802FBABE lea       rcx, [rip + 0x18b4a3b] # 0x181bb0500
00000001802FBAC5 call      0x18024fef0
00000001802FBACA mov       byte ptr [rip + 0x19e8fdf], 1 # 0x181ce4ab0
00000001802FBAD1 mov       rax, qword ptr [rsi + 0x28]
00000001802FBAD5 test      rax, rax
00000001802FBAD8 je        0x1802fbccb
00000001802FBADE mov       rdx, qword ptr [rax + 0x58]
00000001802FBAE2 test      rdx, rdx
00000001802FBAE5 je        0x1802fbccb
00000001802FBAEB mov       r8, qword ptr [rip + 0x18b999e] # 0x181bb5490
00000001802FBAF2 lea       rcx, [rsp + 0x28]
00000001802FBAF7 call      0x1808197f0
00000001802FBAFC movups    xmm0, xmmword ptr [rsp + 0x28]
00000001802FBB01 movups    xmmword ptr [rsp + 0x40], xmm0
00000001802FBB06 movsd     xmm1, qword ptr [rsp + 0x38]
00000001802FBB0C movsd     qword ptr [rsp + 0x50], xmm1
00000001802FBB12 mov       qword ptr [rsp + 0x28], 0
00000001802FBB1B lea       rbx, [rsp + 0x40]
00000001802FBB20 mov       qword ptr [rsp + 0x30], rbx
00000001802FBB25 nop       word ptr [rax + rax]
00000001802FBB30 mov       rdx, qword ptr [rip + 0x18cb239] # 0x181bc6d70
00000001802FBB37 lea       rcx, [rsp + 0x40]
00000001802FBB3C call      0x180669280
00000001802FBB41 test      al, al
00000001802FBB43 je        0x1802fbc5e
00000001802FBB49 mov       r14, qword ptr [rsp + 0x50]
00000001802FBB4E mov       rdi, qword ptr [rsi + 0xa0]
00000001802FBB55 mov       rcx, qword ptr [rip + 0x18b49a4] # 0x181bb0500
00000001802FBB5C cmp       dword ptr [rcx + 0xe0], 0
00000001802FBB63 jne       0x1802fbb6a
00000001802FBB65 call      0x1802501e0
00000001802FBB6A mov       rdx, qword ptr [rip + 0x18bbed7] # 0x181bb7a48
00000001802FBB71 mov       rcx, rdi
00000001802FBB74 call      0x1804a0690
00000001802FBB79 mov       rdi, rax
00000001802FBB7C test      rax, rax
00000001802FBB7F je        0x1802fbcbf
00000001802FBB85 xor       edx, edx
00000001802FBB87 mov       rcx, rax
00000001802FBB8A call      0x181304510
00000001802FBB8F mov       r15, rax
00000001802FBB92 mov       rcx, qword ptr [rsi + 0x68]
00000001802FBB96 test      rcx, rcx
00000001802FBB99 je        0x1802fbcba
00000001802FBB9F xor       edx, edx
00000001802FBBA1 call      0x18131c070
00000001802FBBA6 test      r15, r15
00000001802FBBA9 je        0x1802fbcb5
00000001802FBBAF xor       r9d, r9d
00000001802FBBB2 xor       r8d, r8d
00000001802FBBB5 mov       rdx, rax
00000001802FBBB8 mov       rcx, r15
00000001802FBBBB call      0x18132e900
00000001802FBBC0 mov       qword ptr [rdi + 0x28], rsi
00000001802FBBC4 lea       rcx, [rdi + 0x28]
00000001802FBBC8 mov       rdx, rsi
00000001802FBBCB call      0x18024f360
00000001802FBBD0 mov       qword ptr [rdi + 0x30], r14
00000001802FBBD4 lea       rcx, [rdi + 0x30]
00000001802FBBD8 mov       rdx, r14
00000001802FBBDB call      0x18024f360
00000001802FBBE0 xor       edx, edx
00000001802FBBE2 mov       rcx, rdi
00000001802FBBE5 call      0x180303880
00000001802FBBEA mov       rcx, qword ptr [rsi + 0x90]
00000001802FBBF1 test      rcx, rcx
00000001802FBBF4 je        0x1802fbcb0
00000001802FBBFA mov       r9, qword ptr [rip + 0x18ad8df] # 0x181ba94e0
00000001802FBC01 inc       dword ptr [rcx + 0x1c]
00000001802FBC04 mov       rdx, qword ptr [rcx + 0x10]
00000001802FBC08 movsxd    r8, dword ptr [rcx + 0x18]
00000001802FBC0C test      rdx, rdx
00000001802FBC0F je        0x1802fbcab
00000001802FBC15 cmp       r8d, dword ptr [rdx + 0x18]
00000001802FBC19 jb        0x1802fbc37
00000001802FBC1B mov       rax, qword ptr [r9 + 0x20]
00000001802FBC1F mov       r8, qword ptr [rax + 0xc0]
00000001802FBC26 mov       r8, qword ptr [r8 + 0x70]
00000001802FBC2A mov       rdx, rdi
00000001802FBC2D call      0x180847ce0
00000001802FBC32 jmp       0x1802fbb30
00000001802FBC37 lea       eax, [r8 + 1]
00000001802FBC3B mov       dword ptr [rcx + 0x18], eax
00000001802FBC3E cmp       r8d, dword ptr [rdx + 0x18]
00000001802FBC42 jae       0x1802fbca5
00000001802FBC44 mov       qword ptr [rdx + r8*8 + 0x20], rdi
00000001802FBC49 add       rdx, 0x20
00000001802FBC4D lea       rcx, [rdx + r8*8]
00000001802FBC51 mov       rdx, rdi
00000001802FBC54 call      0x18024f360
00000001802FBC59 jmp       0x1802fbb30
00000001802FBC5E mov       rdx, qword ptr [rip + 0x18cb0b3] # 0x181bc6d18
00000001802FBC65 mov       rcx, rbx
00000001802FBC68 call      0x180302170
00000001802FBC6D jmp       0x1802fbc8a
00000001802FBC6F mov       rdx, qword ptr [rip + 0x18cb0a2] # 0x181bc6d18
00000001802FBC76 mov       rcx, qword ptr [rsp + 0x30]
00000001802FBC7B call      0x180302170
00000001802FBC80 mov       rcx, qword ptr [rsp + 0x28]
00000001802FBC85 test      rcx, rcx
00000001802FBC88 jne       0x1802fbcc5
00000001802FBC8A lea       r11, [rsp + 0x60]
00000001802FBC8F mov       rbx, qword ptr [r11 + 0x10]
00000001802FBC93 mov       rsi, qword ptr [r11 + 0x18]
00000001802FBC97 mov       rdi, qword ptr [r11 + 0x20]
00000001802FBC9B mov       r14, qword ptr [r11 + 0x28]
00000001802FBC9F mov       rsp, r11
00000001802FBCA2 pop       r15
00000001802FBCA4 ret       
00000001802FBCA5 call      0x180250140
00000001802FBCAA nop       
00000001802FBCAB call      0x180250150
00000001802FBCB0 call      0x180250150
00000001802FBCB5 call      0x180250150
00000001802FBCBA call      0x180250150
00000001802FBCBF call      0x180250150
00000001802FBCC4 nop       
00000001802FBCC5 call      0x180200e60
00000001802FBCCA int3      
00000001802FBCCB call      0x180250150
00000001802FBCD0 int3      
00000001802FBCD1 int3      
00000001802FBCD2 int3      
00000001802FBCD3 int3      
00000001802FBCD4 int3      
00000001802FBCD5 int3      
00000001802FBCD6 int3      
00000001802FBCD7 int3      
00000001802FBCD8 int3      
00000001802FBCD9 int3      
00000001802FBCDA int3      
00000001802FBCDB int3      
00000001802FBCDC int3      
00000001802FBCDD int3      
00000001802FBCDE int3      
00000001802FBCDF int3      