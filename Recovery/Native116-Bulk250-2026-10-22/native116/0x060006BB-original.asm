00000001803ABB00 mov       qword ptr [rsp + 0x18], rbx
00000001803ABB05 mov       qword ptr [rsp + 0x20], rsi
00000001803ABB0A mov       qword ptr [rsp + 0x10], rdx
00000001803ABB0F mov       qword ptr [rsp + 8], rcx
00000001803ABB14 push      rdi
00000001803ABB15 push      r14
00000001803ABB17 push      r15
00000001803ABB19 sub       rsp, 0xb0
00000001803ABB20 mov       rdi, rdx
00000001803ABB23 mov       r14, rcx
00000001803ABB26 cmp       byte ptr [rip + 0x19393df], 0 # 0x181ce4f0c
00000001803ABB2D jne       0x1803abb66
00000001803ABB2F lea       rcx, [rip + 0x1816072] # 0x181bc1ba8
00000001803ABB36 call      0x18024fef0
00000001803ABB3B lea       rcx, [rip + 0x18160be] # 0x181bc1c00
00000001803ABB42 call      0x18024fef0
00000001803ABB47 lea       rcx, [rip + 0x181610a] # 0x181bc1c58
00000001803ABB4E call      0x18024fef0
00000001803ABB53 lea       rcx, [rip + 0x17fa5de] # 0x181ba6138
00000001803ABB5A call      0x18024fef0
00000001803ABB5F mov       byte ptr [rip + 0x19393a6], 1 # 0x181ce4f0c
00000001803ABB66 xorps     xmm0, xmm0
00000001803ABB69 xor       eax, eax
00000001803ABB6B movups    xmmword ptr [rsp + 0x60], xmm0
00000001803ABB70 mov       qword ptr [rsp + 0x70], rax
00000001803ABB75 mov       rdx, qword ptr [r14 + 0x20]
00000001803ABB79 test      rdx, rdx
00000001803ABB7C je        0x1803abe0b
00000001803ABB82 mov       r8, qword ptr [rip + 0x17fa5af] # 0x181ba6138
00000001803ABB89 lea       rcx, [rsp + 0x30]
00000001803ABB8E call      0x1808197f0
00000001803ABB93 movups    xmm0, xmmword ptr [rsp + 0x30]
00000001803ABB98 movups    xmmword ptr [rsp + 0x48], xmm0
00000001803ABB9D movsd     xmm1, qword ptr [rsp + 0x40]
00000001803ABBA3 movsd     qword ptr [rsp + 0x58], xmm1
00000001803ABBA9 xor       r15d, r15d
00000001803ABBAC mov       qword ptr [rsp + 0x20], r15
00000001803ABBB1 lea       rbx, [rsp + 0x48]
00000001803ABBB6 mov       qword ptr [rsp + 0x28], rbx
00000001803ABBBB nop       dword ptr [rax + rax]
00000001803ABBC0 mov       rdx, qword ptr [rip + 0x1816039] # 0x181bc1c00
00000001803ABBC7 lea       rcx, [rsp + 0x48]
00000001803ABBCC call      0x180669280
00000001803ABBD1 test      al, al
00000001803ABBD3 je        0x1803abbf5
00000001803ABBD5 mov       rcx, qword ptr [rsp + 0x58]
00000001803ABBDA test      rcx, rcx
00000001803ABBDD je        0x1803abdde
00000001803ABBE3 cmp       byte ptr [rcx + 0x104], 0
00000001803ABBEA je        0x1803abbc0
00000001803ABBEC xor       edx, edx
00000001803ABBEE call      0x1803438a0
00000001803ABBF3 jmp       0x1803abbc0
00000001803ABBF5 mov       rdx, qword ptr [rip + 0x1815fac] # 0x181bc1ba8
00000001803ABBFC mov       rcx, rbx
00000001803ABBFF call      0x180302170
00000001803ABC04 jmp       0x1803abc38
00000001803ABC06 mov       rdx, qword ptr [rip + 0x1815f9b] # 0x181bc1ba8
00000001803ABC0D mov       rcx, qword ptr [rsp + 0x28]
00000001803ABC12 call      0x180302170
00000001803ABC17 mov       rcx, qword ptr [rsp + 0x20]
00000001803ABC1C test      rcx, rcx
00000001803ABC1F jne       0x1803abde4
00000001803ABC25 xor       r15d, r15d
00000001803ABC28 mov       rdi, qword ptr [rsp + 0xd8]
00000001803ABC30 mov       r14, qword ptr [rsp + 0xd0]
00000001803ABC38 mov       rcx, qword ptr [r14 + 0x50]
00000001803ABC3C test      rcx, rcx
00000001803ABC3F je        0x1803abe0b
00000001803ABC45 xor       r8d, r8d
00000001803ABC48 xor       edx, edx
00000001803ABC4A call      0x1803a1c60
00000001803ABC4F test      rdi, rdi
00000001803ABC52 je        0x1803abe0b
00000001803ABC58 mov       r8, qword ptr [rip + 0x17fa4d9] # 0x181ba6138
00000001803ABC5F mov       rdx, rdi
00000001803ABC62 lea       rcx, [rsp + 0x30]
00000001803ABC67 call      0x1808197f0
00000001803ABC6C movups    xmm0, xmmword ptr [rsp + 0x30]
00000001803ABC71 movups    xmmword ptr [rsp + 0x48], xmm0
00000001803ABC76 movsd     xmm1, qword ptr [rsp + 0x40]
00000001803ABC7C movsd     qword ptr [rsp + 0x58], xmm1
00000001803ABC82 mov       qword ptr [rsp + 0x20], r15
00000001803ABC87 lea       rax, [rsp + 0x48]
00000001803ABC8C mov       qword ptr [rsp + 0x28], rax
00000001803ABC91 mov       rdx, qword ptr [rip + 0x1815f68] # 0x181bc1c00
00000001803ABC98 lea       rcx, [rsp + 0x48]
00000001803ABC9D call      0x180669280
00000001803ABCA2 test      al, al
00000001803ABCA4 je        0x1803abd97
00000001803ABCAA mov       rsi, qword ptr [rsp + 0x58]
00000001803ABCAF mov       rdx, qword ptr [r14 + 0x20]
00000001803ABCB3 test      rdx, rdx
00000001803ABCB6 je        0x1803abdff
00000001803ABCBC mov       r8, qword ptr [rip + 0x17fa475] # 0x181ba6138
00000001803ABCC3 lea       rcx, [rsp + 0x90]
00000001803ABCCB call      0x1808197f0
00000001803ABCD0 movups    xmm0, xmmword ptr [rsp + 0x90]
00000001803ABCD8 movups    xmmword ptr [rsp + 0x60], xmm0
00000001803ABCDD movsd     xmm1, qword ptr [rsp + 0xa0]
00000001803ABCE6 movsd     qword ptr [rsp + 0x70], xmm1
00000001803ABCEC mov       qword ptr [rsp + 0x30], r15
00000001803ABCF1 lea       rbx, [rsp + 0x60]
00000001803ABCF6 mov       qword ptr [rsp + 0x38], rbx
00000001803ABCFB nop       dword ptr [rax + rax]
00000001803ABD00 mov       rdx, qword ptr [rip + 0x1815ef9] # 0x181bc1c00
00000001803ABD07 lea       rcx, [rsp + 0x60]
00000001803ABD0C call      0x180669280
00000001803ABD11 test      al, al
00000001803ABD13 je        0x1803abd58
00000001803ABD15 mov       rdi, qword ptr [rsp + 0x70]
00000001803ABD1A test      rdi, rdi
00000001803ABD1D je        0x1803abdf4
00000001803ABD23 mov       eax, dword ptr [rdi + 0x24]
00000001803ABD26 test      rsi, rsi
00000001803ABD29 je        0x1803abdef
00000001803ABD2F cmp       eax, dword ptr [rsi + 0x24]
00000001803ABD32 jne       0x1803abd00
00000001803ABD34 xor       edx, edx
00000001803ABD36 mov       rcx, rdi
00000001803ABD39 call      0x180343640
00000001803ABD3E mov       rcx, qword ptr [r14 + 0x50]
00000001803ABD42 test      rcx, rcx
00000001803ABD45 je        0x1803abdea
00000001803ABD4B xor       r8d, r8d
00000001803ABD4E mov       rdx, rdi
00000001803ABD51 call      0x1803a1c60
00000001803ABD56 jmp       0x1803abd00
00000001803ABD58 mov       rdx, qword ptr [rip + 0x1815e49] # 0x181bc1ba8
00000001803ABD5F mov       rcx, rbx
00000001803ABD62 call      0x180302170
00000001803ABD67 jmp       0x1803abc91
00000001803ABD6C mov       rdx, qword ptr [rip + 0x1815e35] # 0x181bc1ba8
00000001803ABD73 mov       rcx, qword ptr [rsp + 0x38]
00000001803ABD78 call      0x180302170
00000001803ABD7D mov       rcx, qword ptr [rsp + 0x30]
00000001803ABD82 test      rcx, rcx
00000001803ABD85 jne       0x1803abdfa
00000001803ABD87 xor       r15d, r15d
00000001803ABD8A mov       r14, qword ptr [rsp + 0xd0]
00000001803ABD92 jmp       0x1803abc91
00000001803ABD97 mov       rdx, qword ptr [rip + 0x1815e0a] # 0x181bc1ba8
00000001803ABD9E mov       rcx, qword ptr [rsp + 0x28]
00000001803ABDA3 call      0x180302170
00000001803ABDA8 jmp       0x1803abdc5
00000001803ABDAA mov       rdx, qword ptr [rip + 0x1815df7] # 0x181bc1ba8
00000001803ABDB1 mov       rcx, qword ptr [rsp + 0x28]
00000001803ABDB6 call      0x180302170
00000001803ABDBB mov       rcx, qword ptr [rsp + 0x20]
00000001803ABDC0 test      rcx, rcx
00000001803ABDC3 jne       0x1803abe05
00000001803ABDC5 lea       r11, [rsp + 0xb0]
00000001803ABDCD mov       rbx, qword ptr [r11 + 0x30]
00000001803ABDD1 mov       rsi, qword ptr [r11 + 0x38]
00000001803ABDD5 mov       rsp, r11
00000001803ABDD8 pop       r15
00000001803ABDDA pop       r14
00000001803ABDDC pop       rdi
00000001803ABDDD ret       
00000001803ABDDE call      0x180250150
00000001803ABDE3 nop       
00000001803ABDE4 call      0x180200e60
00000001803ABDE9 nop       
00000001803ABDEA call      0x180250150
00000001803ABDEF call      0x180250150
00000001803ABDF4 call      0x180250150
00000001803ABDF9 nop       
00000001803ABDFA call      0x180200e60
00000001803ABDFF call      0x180250150
00000001803ABE04 nop       
00000001803ABE05 call      0x180200e60
00000001803ABE0A int3      
00000001803ABE0B call      0x180250150
00000001803ABE10 int3      
00000001803ABE11 int3      
00000001803ABE12 int3      
00000001803ABE13 int3      
00000001803ABE14 int3      
00000001803ABE15 int3      
00000001803ABE16 int3      
00000001803ABE17 int3      
00000001803ABE18 int3      
00000001803ABE19 int3      
00000001803ABE1A int3      
00000001803ABE1B int3      
00000001803ABE1C int3      
00000001803ABE1D int3      
00000001803ABE1E int3      
00000001803ABE1F int3      