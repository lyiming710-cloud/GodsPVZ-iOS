00000001802FBCE0 mov       qword ptr [rsp + 8], rbx
00000001802FBCE5 mov       qword ptr [rsp + 0x10], rsi
00000001802FBCEA mov       qword ptr [rsp + 0x18], rdi
00000001802FBCEF mov       qword ptr [rsp + 0x20], r14
00000001802FBCF4 push      r15
00000001802FBCF6 sub       rsp, 0x60
00000001802FBCFA mov       rsi, rcx
00000001802FBCFD cmp       byte ptr [rip + 0x19e8dad], 0 # 0x181ce4ab1
00000001802FBD04 jne       0x1802fbd6d
00000001802FBD06 lea       rcx, [rip + 0x18c7123] # 0x181bc2e30
00000001802FBD0D call      0x18024fef0
00000001802FBD12 lea       rcx, [rip + 0x18c716f] # 0x181bc2e88
00000001802FBD19 call      0x18024fef0
00000001802FBD1E lea       rcx, [rip + 0x18c71bb] # 0x181bc2ee0
00000001802FBD25 call      0x18024fef0
00000001802FBD2A lea       rcx, [rip + 0x18ae98f] # 0x181baa6c0
00000001802FBD31 call      0x18024fef0
00000001802FBD36 lea       rcx, [rip + 0x18ae50b] # 0x181baa248
00000001802FBD3D call      0x18024fef0
00000001802FBD42 lea       rcx, [rip + 0x18bbd57] # 0x181bb7aa0
00000001802FBD49 call      0x18024fef0
00000001802FBD4E lea       rcx, [rip + 0x18b47ab] # 0x181bb0500
00000001802FBD55 call      0x18024fef0
00000001802FBD5A lea       rcx, [rip + 0x18ba717] # 0x181bb6478
00000001802FBD61 call      0x18024fef0
00000001802FBD66 mov       byte ptr [rip + 0x19e8d44], 1 # 0x181ce4ab1
00000001802FBD6D mov       rcx, qword ptr [rip + 0x18ba704] # 0x181bb6478
00000001802FBD74 cmp       dword ptr [rcx + 0xe0], 0
00000001802FBD7B jne       0x1802fbd82
00000001802FBD7D call      0x1802501e0
00000001802FBD82 xor       ecx, ecx
00000001802FBD84 call      0x18033b560
00000001802FBD89 mov       rax, qword ptr [rsi + 0x28]
00000001802FBD8D test      rax, rax
00000001802FBD90 je        0x1802fbf6f
00000001802FBD96 mov       rdx, qword ptr [rax + 0x50]
00000001802FBD9A test      rdx, rdx
00000001802FBD9D je        0x1802fbf6f
00000001802FBDA3 mov       r8, qword ptr [rip + 0x18ae49e] # 0x181baa248
00000001802FBDAA lea       rcx, [rsp + 0x28]
00000001802FBDAF call      0x1808197f0
00000001802FBDB4 movups    xmm0, xmmword ptr [rsp + 0x28]
00000001802FBDB9 movups    xmmword ptr [rsp + 0x40], xmm0
00000001802FBDBE movsd     xmm1, qword ptr [rsp + 0x38]
00000001802FBDC4 movsd     qword ptr [rsp + 0x50], xmm1
00000001802FBDCA mov       qword ptr [rsp + 0x28], 0
00000001802FBDD3 lea       rbx, [rsp + 0x40]
00000001802FBDD8 mov       qword ptr [rsp + 0x30], rbx
00000001802FBDDD nop       dword ptr [rax]
00000001802FBDE0 mov       rdx, qword ptr [rip + 0x18c70a1] # 0x181bc2e88
00000001802FBDE7 lea       rcx, [rsp + 0x40]
00000001802FBDEC call      0x180669280
00000001802FBDF1 test      al, al
00000001802FBDF3 je        0x1802fbf02
00000001802FBDF9 mov       r15, qword ptr [rsp + 0x50]
00000001802FBDFE mov       rdi, qword ptr [rsi + 0xa8]
00000001802FBE05 mov       rcx, qword ptr [rip + 0x18b46f4] # 0x181bb0500
00000001802FBE0C cmp       dword ptr [rcx + 0xe0], 0
00000001802FBE13 jne       0x1802fbe1a
00000001802FBE15 call      0x1802501e0
00000001802FBE1A mov       rdx, qword ptr [rip + 0x18bbc7f] # 0x181bb7aa0
00000001802FBE21 mov       rcx, rdi
00000001802FBE24 call      0x1804a0690
00000001802FBE29 mov       rdi, rax
00000001802FBE2C test      rax, rax
00000001802FBE2F je        0x1802fbf63
00000001802FBE35 xor       edx, edx
00000001802FBE37 mov       rcx, rax
00000001802FBE3A call      0x181304510
00000001802FBE3F mov       r14, rax
00000001802FBE42 mov       rcx, qword ptr [rsi + 0x70]
00000001802FBE46 test      rcx, rcx
00000001802FBE49 je        0x1802fbf5e
00000001802FBE4F xor       edx, edx
00000001802FBE51 call      0x18131c070
00000001802FBE56 test      r14, r14
00000001802FBE59 je        0x1802fbf59
00000001802FBE5F xor       r9d, r9d
00000001802FBE62 xor       r8d, r8d
00000001802FBE65 mov       rdx, rax
00000001802FBE68 mov       rcx, r14
00000001802FBE6B call      0x18132e900
00000001802FBE70 mov       qword ptr [rdi + 0x30], rsi
00000001802FBE74 lea       rcx, [rdi + 0x30]
00000001802FBE78 mov       rdx, rsi
00000001802FBE7B call      0x18024f360
00000001802FBE80 xor       r8d, r8d
00000001802FBE83 mov       rdx, r15
00000001802FBE86 mov       rcx, rdi
00000001802FBE89 call      0x180304cb0
00000001802FBE8E mov       rcx, qword ptr [rsi + 0x98]
00000001802FBE95 test      rcx, rcx
00000001802FBE98 je        0x1802fbf54
00000001802FBE9E mov       r9, qword ptr [rip + 0x18ae81b] # 0x181baa6c0
00000001802FBEA5 inc       dword ptr [rcx + 0x1c]
00000001802FBEA8 mov       rdx, qword ptr [rcx + 0x10]
00000001802FBEAC movsxd    r8, dword ptr [rcx + 0x18]
00000001802FBEB0 test      rdx, rdx
00000001802FBEB3 je        0x1802fbf4f
00000001802FBEB9 cmp       r8d, dword ptr [rdx + 0x18]
00000001802FBEBD jb        0x1802fbedb
00000001802FBEBF mov       rax, qword ptr [r9 + 0x20]
00000001802FBEC3 mov       r8, qword ptr [rax + 0xc0]
00000001802FBECA mov       r8, qword ptr [r8 + 0x70]
00000001802FBECE mov       rdx, rdi
00000001802FBED1 call      0x180847ce0
00000001802FBED6 jmp       0x1802fbde0
00000001802FBEDB lea       eax, [r8 + 1]
00000001802FBEDF mov       dword ptr [rcx + 0x18], eax
00000001802FBEE2 cmp       r8d, dword ptr [rdx + 0x18]
00000001802FBEE6 jae       0x1802fbf49
00000001802FBEE8 mov       qword ptr [rdx + r8*8 + 0x20], rdi
00000001802FBEED add       rdx, 0x20
00000001802FBEF1 lea       rcx, [rdx + r8*8]
00000001802FBEF5 mov       rdx, rdi
00000001802FBEF8 call      0x18024f360
00000001802FBEFD jmp       0x1802fbde0
00000001802FBF02 mov       rdx, qword ptr [rip + 0x18c6f27] # 0x181bc2e30
00000001802FBF09 mov       rcx, rbx
00000001802FBF0C call      0x180302170
00000001802FBF11 jmp       0x1802fbf2e
00000001802FBF13 mov       rdx, qword ptr [rip + 0x18c6f16] # 0x181bc2e30
00000001802FBF1A mov       rcx, qword ptr [rsp + 0x30]
00000001802FBF1F call      0x180302170
00000001802FBF24 mov       rcx, qword ptr [rsp + 0x28]
00000001802FBF29 test      rcx, rcx
00000001802FBF2C jne       0x1802fbf69
00000001802FBF2E lea       r11, [rsp + 0x60]
00000001802FBF33 mov       rbx, qword ptr [r11 + 0x10]
00000001802FBF37 mov       rsi, qword ptr [r11 + 0x18]
00000001802FBF3B mov       rdi, qword ptr [r11 + 0x20]
00000001802FBF3F mov       r14, qword ptr [r11 + 0x28]
00000001802FBF43 mov       rsp, r11
00000001802FBF46 pop       r15
00000001802FBF48 ret       
00000001802FBF49 call      0x180250140
00000001802FBF4E nop       
00000001802FBF4F call      0x180250150
00000001802FBF54 call      0x180250150
00000001802FBF59 call      0x180250150
00000001802FBF5E call      0x180250150
00000001802FBF63 call      0x180250150
00000001802FBF68 nop       
00000001802FBF69 call      0x180200e60
00000001802FBF6E int3      
00000001802FBF6F call      0x180250150
00000001802FBF74 int3      
00000001802FBF75 int3      
00000001802FBF76 int3      
00000001802FBF77 int3      
00000001802FBF78 int3      
00000001802FBF79 int3      
00000001802FBF7A int3      
00000001802FBF7B int3      
00000001802FBF7C int3      
00000001802FBF7D int3      
00000001802FBF7E int3      
00000001802FBF7F int3      