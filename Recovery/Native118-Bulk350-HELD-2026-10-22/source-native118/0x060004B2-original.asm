0000000180371B10 mov       qword ptr [rsp + 8], rcx
0000000180371B15 push      rbx
0000000180371B16 push      rsi
0000000180371B17 push      rdi
0000000180371B18 push      r12
0000000180371B1A push      r13
0000000180371B1C push      r14
0000000180371B1E push      r15
0000000180371B20 sub       rsp, 0xc0
0000000180371B27 mov       rsi, rcx
0000000180371B2A cmp       byte ptr [rip + 0x1973281], 0 # 0x181ce4db2
0000000180371B31 jne       0x180371bda
0000000180371B37 lea       rcx, [rip + 0x1850bc2] # 0x181bc2700
0000000180371B3E call      0x18024fef0
0000000180371B43 lea       rcx, [rip + 0x1858d1e] # 0x181bca868
0000000180371B4A call      0x18024fef0
0000000180371B4F lea       rcx, [rip + 0x1854c9a] # 0x181bc67f0
0000000180371B56 call      0x18024fef0
0000000180371B5B lea       rcx, [rip + 0x1850bf6] # 0x181bc2758
0000000180371B62 call      0x18024fef0
0000000180371B67 lea       rcx, [rip + 0x1854cda] # 0x181bc6848
0000000180371B6E call      0x18024fef0
0000000180371B73 lea       rcx, [rip + 0x1858d3e] # 0x181bca8b8
0000000180371B7A call      0x18024fef0
0000000180371B7F lea       rcx, [rip + 0x1858d8a] # 0x181bca910
0000000180371B86 call      0x18024fef0
0000000180371B8B lea       rcx, [rip + 0x1854d0e] # 0x181bc68a0
0000000180371B92 call      0x18024fef0
0000000180371B97 lea       rcx, [rip + 0x1850c12] # 0x181bc27b0
0000000180371B9E call      0x18024fef0
0000000180371BA3 lea       rcx, [rip + 0x1851b3e] # 0x181bc36e8
0000000180371BAA call      0x18024fef0
0000000180371BAF lea       rcx, [rip + 0x184275a] # 0x181bb4310
0000000180371BB6 call      0x18024fef0
0000000180371BBB lea       rcx, [rip + 0x18374fe] # 0x181ba90c0
0000000180371BC2 call      0x18024fef0
0000000180371BC7 lea       rcx, [rip + 0x183e932] # 0x181bb0500
0000000180371BCE call      0x18024fef0
0000000180371BD3 mov       byte ptr [rip + 0x19731d8], 1 # 0x181ce4db2
0000000180371BDA xorps     xmm0, xmm0
0000000180371BDD xor       eax, eax
0000000180371BDF movups    xmmword ptr [rsp + 0x60], xmm0
0000000180371BE4 mov       qword ptr [rsp + 0x70], rax
0000000180371BE9 xorps     xmm1, xmm1
0000000180371BEC movups    xmmword ptr [rsp + 0x78], xmm1
0000000180371BF1 mov       qword ptr [rsp + 0x88], rax
0000000180371BF9 cmp       byte ptr [rsi + 0xd0], al
0000000180371BFF je        0x1803721ce
0000000180371C05 cmp       byte ptr [rsi + 0xd1], al
0000000180371C0B jne       0x1803720a9
0000000180371C11 mov       rax, qword ptr [rsi + 0x20]
0000000180371C15 test      rax, rax
0000000180371C18 je        0x180372227
0000000180371C1E mov       rcx, qword ptr [rax + 0xe8]
0000000180371C25 xor       r14d, r14d
0000000180371C28 mov       r15d, r14d
0000000180371C2B mov       qword ptr [rsp + 0x118], r14
0000000180371C33 mov       r12d, r14d
0000000180371C36 mov       qword ptr [rsp + 0x110], r14
0000000180371C3E mov       r13d, r14d
0000000180371C41 test      rcx, rcx
0000000180371C44 je        0x180372227
0000000180371C4A mov       rdx, qword ptr [rcx + 0x30]
0000000180371C4E test      rdx, rdx
0000000180371C51 je        0x180372227
0000000180371C57 mov       r8, qword ptr [rip + 0x18426b2] # 0x181bb4310
0000000180371C5E lea       rcx, [rsp + 0x30]
0000000180371C63 call      0x1808197f0
0000000180371C68 movups    xmm0, xmmword ptr [rsp + 0x30]
0000000180371C6D movups    xmmword ptr [rsp + 0x90], xmm0
0000000180371C75 movsd     xmm1, qword ptr [rsp + 0x40]
0000000180371C7B movsd     qword ptr [rsp + 0xa0], xmm1
0000000180371C84 mov       qword ptr [rsp + 0x30], r14
0000000180371C89 lea       rbx, [rsp + 0x90]
0000000180371C91 mov       qword ptr [rsp + 0x38], rbx
0000000180371C96 nop       word ptr [rax + rax]
0000000180371CA0 mov       rdx, qword ptr [rip + 0x1854ba1] # 0x181bc6848
0000000180371CA7 lea       rcx, [rsp + 0x90]
0000000180371CAF call      0x180669280
0000000180371CB4 test      al, al
0000000180371CB6 je        0x180371cfb
0000000180371CB8 mov       rdi, qword ptr [rsp + 0xa0]
0000000180371CC0 test      rdi, rdi
0000000180371CC3 je        0x180372233
0000000180371CC9 xor       edx, edx
0000000180371CCB mov       rcx, rdi
0000000180371CCE call      0x18034e170
0000000180371CD3 test      al, al
0000000180371CD5 je        0x180371ca0
0000000180371CD7 test      rdi, rdi
0000000180371CDA je        0x18037222d
0000000180371CE0 mov       eax, dword ptr [rsi + 0xfc]
0000000180371CE6 cmp       dword ptr [rdi + 0xbc], eax
0000000180371CEC je        0x180371ca0
0000000180371CEE mov       r15, rdi
0000000180371CF1 mov       qword ptr [rsp + 0x118], rdi
0000000180371CF9 jmp       0x180371ca0
0000000180371CFB mov       rdx, qword ptr [rip + 0x1854aee] # 0x181bc67f0
0000000180371D02 mov       rcx, rbx
0000000180371D05 call      0x180302170
0000000180371D0A jmp       0x180371d44
0000000180371D0C mov       rdx, qword ptr [rip + 0x1854add] # 0x181bc67f0
0000000180371D13 mov       rcx, qword ptr [rsp + 0x38]
0000000180371D18 call      0x180302170
0000000180371D1D mov       rcx, qword ptr [rsp + 0x30]
0000000180371D22 test      rcx, rcx
0000000180371D25 jne       0x180372239
0000000180371D2B xor       r14d, r14d
0000000180371D2E mov       rsi, qword ptr [rsp + 0x100]
0000000180371D36 mov       r15, qword ptr [rsp + 0x118]
0000000180371D3E mov       r12d, r14d
0000000180371D41 mov       r13d, r14d
0000000180371D44 mov       qword ptr [rsi + 0x170], r15
0000000180371D4B lea       rcx, [rsi + 0x170]
0000000180371D52 mov       rdx, r15
0000000180371D55 call      0x18024f360
0000000180371D5A mov       rcx, qword ptr [rip + 0x183e79f] # 0x181bb0500
0000000180371D61 cmp       dword ptr [rcx + 0xe0], 0
0000000180371D68 jne       0x180371d6f
0000000180371D6A call      0x1802501e0
0000000180371D6F xor       r8d, r8d
0000000180371D72 xor       edx, edx
0000000180371D74 mov       rcx, r15
0000000180371D77 call      0x18131f760
0000000180371D7C test      al, al
0000000180371D7E je        0x180372168
0000000180371D84 mov       rax, qword ptr [rsi + 0x20]
0000000180371D88 test      rax, rax
0000000180371D8B je        0x180372227
0000000180371D91 mov       rcx, qword ptr [rax + 0x108]
0000000180371D98 test      rcx, rcx
0000000180371D9B je        0x180372227
0000000180371DA1 mov       rdx, qword ptr [rcx + 0x30]
0000000180371DA5 test      rdx, rdx
0000000180371DA8 je        0x180372227
0000000180371DAE mov       r8, qword ptr [rip + 0x183730b] # 0x181ba90c0
0000000180371DB5 lea       rcx, [rsp + 0x30]
0000000180371DBA call      0x1808197f0
0000000180371DBF movups    xmm0, xmmword ptr [rsp + 0x30]
0000000180371DC4 movups    xmmword ptr [rsp + 0x60], xmm0
0000000180371DC9 movsd     xmm1, qword ptr [rsp + 0x40]
0000000180371DCF movsd     qword ptr [rsp + 0x70], xmm1
0000000180371DD5 mov       qword ptr [rsp + 0x30], r14
0000000180371DDA lea       rbx, [rsp + 0x60]
0000000180371DDF mov       qword ptr [rsp + 0x38], rbx
0000000180371DE4 mov       rdx, qword ptr [rip + 0x185096d] # 0x181bc2758
0000000180371DEB lea       rcx, [rsp + 0x60]
0000000180371DF0 call      0x180669280
0000000180371DF5 test      al, al
0000000180371DF7 je        0x180371e49
0000000180371DF9 mov       rdi, qword ptr [rsp + 0x70]
0000000180371DFE mov       qword ptr [rsp + 0x20], r14
0000000180371E03 xor       r9d, r9d
0000000180371E06 xor       r8d, r8d
0000000180371E09 mov       edx, 0x1a
0000000180371E0E mov       rcx, rsi
0000000180371E11 call      0x180360d00
0000000180371E16 test      rdi, rdi
0000000180371E19 je        0x18037223f
0000000180371E1F xor       r8d, r8d
0000000180371E22 mov       rdx, rax
0000000180371E25 mov       rcx, rdi
0000000180371E28 call      0x180346b00
0000000180371E2D test      al, al
0000000180371E2F je        0x180371de4
0000000180371E31 mov       eax, dword ptr [rsi + 0xfc]
0000000180371E37 cmp       dword ptr [rdi + 0x58], eax
0000000180371E3A je        0x180371de4
0000000180371E3C mov       r12, rdi
0000000180371E3F mov       qword ptr [rsp + 0x110], rdi
0000000180371E47 jmp       0x180371de4
0000000180371E49 mov       rdx, qword ptr [rip + 0x18508b0] # 0x181bc2700
0000000180371E50 mov       rcx, rbx
0000000180371E53 call      0x180302170
0000000180371E58 mov       qword ptr [rsi + 0x180], r12
0000000180371E5F lea       rcx, [rsi + 0x180]
0000000180371E66 mov       rdx, r12
0000000180371E69 call      0x18024f360
0000000180371E6E mov       rcx, qword ptr [rip + 0x183e68b] # 0x181bb0500
0000000180371E75 cmp       dword ptr [rcx + 0xe0], 0
0000000180371E7C jne       0x180371e83
0000000180371E7E call      0x1802501e0
0000000180371E83 xor       r8d, r8d
0000000180371E86 xor       edx, edx
0000000180371E88 mov       rcx, r15
0000000180371E8B call      0x18131f760
0000000180371E90 test      al, al
0000000180371E92 je        0x180372108
0000000180371E98 mov       rcx, qword ptr [rip + 0x183e661] # 0x181bb0500
0000000180371E9F cmp       dword ptr [rcx + 0xe0], 0
0000000180371EA6 jne       0x180371ead
0000000180371EA8 call      0x1802501e0
0000000180371EAD xor       r8d, r8d
0000000180371EB0 xor       edx, edx
0000000180371EB2 mov       rcx, r12
0000000180371EB5 call      0x18131f760
0000000180371EBA test      al, al
0000000180371EBC je        0x180372108
0000000180371EC2 mov       rax, qword ptr [rsi + 0x20]
0000000180371EC6 test      rax, rax
0000000180371EC9 je        0x180372227
0000000180371ECF mov       rcx, qword ptr [rax + 0xf0]
0000000180371ED6 test      rcx, rcx
0000000180371ED9 je        0x180372227
0000000180371EDF mov       rdx, qword ptr [rcx + 0x38]
0000000180371EE3 test      rdx, rdx
0000000180371EE6 je        0x180372227
0000000180371EEC mov       r8, qword ptr [rip + 0x18517f5] # 0x181bc36e8
0000000180371EF3 lea       rcx, [rsp + 0x30]
0000000180371EF8 call      0x1808197f0
0000000180371EFD movups    xmm0, xmmword ptr [rsp + 0x30]
0000000180371F02 movups    xmmword ptr [rsp + 0x78], xmm0
0000000180371F07 movsd     xmm1, qword ptr [rsp + 0x40]
0000000180371F0D movsd     qword ptr [rsp + 0x88], xmm1
0000000180371F16 mov       qword ptr [rsp + 0x30], r14
0000000180371F1B lea       rbx, [rsp + 0x78]
0000000180371F20 mov       qword ptr [rsp + 0x38], rbx
0000000180371F25 nop       word ptr [rax + rax]
0000000180371F30 mov       rdx, qword ptr [rip + 0x1858981] # 0x181bca8b8
0000000180371F37 lea       rcx, [rsp + 0x78]
0000000180371F3C call      0x180669280
0000000180371F41 test      al, al
0000000180371F43 je        0x180371fdd
0000000180371F49 mov       rdi, qword ptr [rsp + 0x88]
0000000180371F51 test      rdi, rdi
0000000180371F54 je        0x18037224f
0000000180371F5A xor       edx, edx
0000000180371F5C mov       rcx, rdi
0000000180371F5F call      0x1813044d0
0000000180371F64 test      rax, rax
0000000180371F67 je        0x18037224a
0000000180371F6D xor       edx, edx
0000000180371F6F mov       rcx, rax
0000000180371F72 call      0x18131bf10
0000000180371F77 test      al, al
0000000180371F79 je        0x180371f30
0000000180371F7B cmp       byte ptr [rdi + 0xbe], 0
0000000180371F82 jne       0x180371f30
0000000180371F84 cmp       byte ptr [rdi + 0xb8], 0
0000000180371F8B jne       0x180371f30
0000000180371F8D cmp       byte ptr [rdi + 0xb9], 0
0000000180371F94 jne       0x180371f30
0000000180371F96 cmp       dword ptr [rdi + 0x60], 3
0000000180371F9A je        0x180371fa8
0000000180371F9C cmp       dword ptr [rdi + 0x60], 0x17
0000000180371FA0 je        0x180371fa8
0000000180371FA2 cmp       dword ptr [rdi + 0x60], 0xd
0000000180371FA6 jmp       0x180371fb4
0000000180371FA8 cmp       byte ptr [rdi + 0xce], 0
0000000180371FAF sete      al
0000000180371FB2 test      al, al
0000000180371FB4 je        0x180371f30
0000000180371FBA test      rdi, rdi
0000000180371FBD je        0x180372245
0000000180371FC3 mov       eax, dword ptr [rsi + 0xfc]
0000000180371FC9 cmp       dword ptr [rdi + 0xfc], eax
0000000180371FCF je        0x180371f30
0000000180371FD5 mov       r13, rdi
0000000180371FD8 jmp       0x180371f30
0000000180371FDD mov       rdx, qword ptr [rip + 0x1858884] # 0x181bca868
0000000180371FE4 mov       rcx, rbx
0000000180371FE7 call      0x180302170
0000000180371FEC mov       qword ptr [rsi + 0x178], r13
0000000180371FF3 lea       rcx, [rsi + 0x178]
0000000180371FFA mov       rdx, r13
0000000180371FFD call      0x18024f360
0000000180372002 mov       rcx, qword ptr [rip + 0x183e4f7] # 0x181bb0500
0000000180372009 cmp       dword ptr [rcx + 0xe0], 0
0000000180372010 jne       0x180372017
0000000180372012 call      0x1802501e0
0000000180372017 xor       r8d, r8d
000000018037201A xor       edx, edx
000000018037201C mov       rcx, r15
000000018037201F call      0x18131f760
0000000180372024 test      al, al
0000000180372026 je        0x1803720bc
000000018037202C mov       rcx, qword ptr [rip + 0x183e4cd] # 0x181bb0500
0000000180372033 cmp       dword ptr [rcx + 0xe0], 0
000000018037203A jne       0x180372041
000000018037203C call      0x1802501e0
0000000180372041 xor       r8d, r8d
0000000180372044 xor       edx, edx
0000000180372046 mov       rcx, r12
0000000180372049 call      0x18131f760
000000018037204E test      al, al
0000000180372050 je        0x1803720bc
0000000180372052 mov       rcx, qword ptr [rip + 0x183e4a7] # 0x181bb0500
0000000180372059 cmp       dword ptr [rcx + 0xe0], 0
0000000180372060 jne       0x180372067
0000000180372062 call      0x1802501e0
0000000180372067 xor       r8d, r8d
000000018037206A xor       edx, edx
000000018037206C mov       rcx, r13
000000018037206F call      0x18131f760
0000000180372074 test      al, al
0000000180372076 je        0x1803720bc
0000000180372078 mov       rax, qword ptr [rsi + 0x1f8]
000000018037207F test      rax, rax
0000000180372082 je        0x180372227
0000000180372088 cmp       dword ptr [rax + 0x18], 0
000000018037208C jbe       0x180372261
0000000180372092 mov       rcx, qword ptr [rax + 0x20]
0000000180372096 test      rcx, rcx
0000000180372099 je        0x180372227
000000018037209F xor       r8d, r8d
00000001803720A2 xor       edx, edx
00000001803720A4 call      0x18131bad0
00000001803720A9 add       rsp, 0xc0
00000001803720B0 pop       r15
00000001803720B2 pop       r14
00000001803720B4 pop       r13
00000001803720B6 pop       r12
00000001803720B8 pop       rdi
00000001803720B9 pop       rsi
00000001803720BA pop       rbx
00000001803720BB ret       
00000001803720BC test      r13, r13
00000001803720BF je        0x180372227
00000001803720C5 movss     xmm1, dword ptr [r13 + 0x34]
00000001803720CB movss     xmm0, dword ptr [r13 + 0x30]
00000001803720D1 jmp       0x1803721a7
00000001803720D6 mov       rdx, qword ptr [rip + 0x185878b] # 0x181bca868
00000001803720DD mov       rcx, qword ptr [rsp + 0x38]
00000001803720E2 call      0x180302170
00000001803720E7 mov       rcx, qword ptr [rsp + 0x30]
00000001803720EC test      rcx, rcx
00000001803720EF jne       0x180372255
00000001803720F5 xor       r14d, r14d
00000001803720F8 mov       rsi, qword ptr [rsp + 0x100]
0000000180372100 mov       r12, qword ptr [rsp + 0x110]
0000000180372108 mov       qword ptr [rsi + 0x178], r14
000000018037210F lea       rcx, [rsi + 0x178]
0000000180372116 xor       edx, edx
0000000180372118 call      0x18024f360
000000018037211D test      r12, r12
0000000180372120 je        0x180372227
0000000180372126 movss     xmm1, dword ptr [r12 + 0x3c]
000000018037212D movss     xmm0, dword ptr [r12 + 0x38]
0000000180372134 jmp       0x1803721a7
0000000180372136 mov       rdx, qword ptr [rip + 0x18505c3] # 0x181bc2700
000000018037213D mov       rcx, qword ptr [rsp + 0x38]
0000000180372142 call      0x180302170
0000000180372147 mov       rcx, qword ptr [rsp + 0x30]
000000018037214C test      rcx, rcx
000000018037214F jne       0x18037225b
0000000180372155 xor       r14d, r14d
0000000180372158 mov       rsi, qword ptr [rsp + 0x100]
0000000180372160 mov       r15, qword ptr [rsp + 0x118]
0000000180372168 mov       qword ptr [rsi + 0x180], r14
000000018037216F lea       rcx, [rsi + 0x180]
0000000180372176 xor       edx, edx
0000000180372178 call      0x18024f360
000000018037217D mov       qword ptr [rsi + 0x178], r14
0000000180372184 lea       rcx, [rsi + 0x178]
000000018037218B xor       edx, edx
000000018037218D call      0x18024f360
0000000180372192 test      r15, r15
0000000180372195 je        0x180372227
000000018037219B movss     xmm1, dword ptr [r15 + 0x34]
00000001803721A1 movss     xmm0, dword ptr [r15 + 0x30]
00000001803721A7 xorps     xmm2, xmm2
00000001803721AA unpcklps  xmm0, xmm1
00000001803721AD movsd     qword ptr [rsp + 0x50], xmm0
00000001803721B3 movss     dword ptr [rsp + 0x58], xmm2
00000001803721B9 xor       r8d, r8d
00000001803721BC lea       rdx, [rsp + 0x50]
00000001803721C1 mov       rcx, rsi
00000001803721C4 call      0x180372720
00000001803721C9 jmp       0x1803720a9
00000001803721CE xor       r14d, r14d
00000001803721D1 mov       qword ptr [rsi + 0x170], r14
00000001803721D8 lea       rcx, [rsi + 0x170]
00000001803721DF xor       edx, edx
00000001803721E1 call      0x18024f360
00000001803721E6 mov       qword ptr [rsi + 0x180], r14
00000001803721ED lea       rcx, [rsi + 0x180]
00000001803721F4 xor       edx, edx
00000001803721F6 call      0x18024f360
00000001803721FB mov       qword ptr [rsi + 0x178], r14
0000000180372202 lea       rcx, [rsi + 0x178]
0000000180372209 xor       edx, edx
000000018037220B call      0x18024f360
0000000180372210 mov       rax, qword ptr [rsi + 0x1f8]
0000000180372217 test      rax, rax
000000018037221A je        0x180372227
000000018037221C cmp       dword ptr [rax + 0x18], r14d
0000000180372220 jbe       0x180372261
0000000180372222 jmp       0x180372092
0000000180372227 call      0x180250150
000000018037222C nop       
000000018037222D call      0x180250150
0000000180372232 nop       
0000000180372233 call      0x180250150
0000000180372238 nop       
0000000180372239 call      0x180200e60
000000018037223E nop       
000000018037223F call      0x180250150
0000000180372244 nop       
0000000180372245 call      0x180250150
000000018037224A call      0x180250150
000000018037224F call      0x180250150
0000000180372254 nop       
0000000180372255 call      0x180200e60
000000018037225A int3      
000000018037225B call      0x180200e60
0000000180372260 int3      
0000000180372261 call      0x180250140
0000000180372266 int3      
0000000180372267 int3      
0000000180372268 int3      
0000000180372269 int3      
000000018037226A int3      
000000018037226B int3      
000000018037226C int3      
000000018037226D int3      
000000018037226E int3      
000000018037226F int3      