00000001803198A0 mov       rax, rsp
00000001803198A3 mov       qword ptr [rax + 0x10], rbx
00000001803198A7 mov       qword ptr [rax + 8], rcx
00000001803198AB push      rsi
00000001803198AC push      rdi
00000001803198AD push      r13
00000001803198AF push      r14
00000001803198B1 push      r15
00000001803198B3 sub       rsp, 0xa0
00000001803198BA movaps    xmmword ptr [rax - 0x38], xmm6
00000001803198BE movaps    xmmword ptr [rax - 0x48], xmm7
00000001803198C2 mov       r13, rcx
00000001803198C5 cmp       byte ptr [rip + 0x19cb2de], 0 # 0x181ce4baa
00000001803198CC jne       0x180319969
00000001803198D2 lea       rcx, [rip + 0x188146f] # 0x181b9ad48
00000001803198D9 call      0x18024fef0
00000001803198DE lea       rcx, [rip + 0x1884ee3] # 0x181b9e7c8
00000001803198E5 call      0x18024fef0
00000001803198EA lea       rcx, [rip + 0x18b116f] # 0x181bcaa60
00000001803198F1 call      0x18024fef0
00000001803198F6 lea       rcx, [rip + 0x18b11b3] # 0x181bcaab0
00000001803198FD call      0x18024fef0
0000000180319902 lea       rcx, [rip + 0x18b11ff] # 0x181bcab08
0000000180319909 call      0x18024fef0
000000018031990E lea       rcx, [rip + 0x18908db] # 0x181baa1f0
0000000180319915 call      0x18024fef0
000000018031991A lea       rcx, [rip + 0x18aa23f] # 0x181bc3b60
0000000180319921 call      0x18024fef0
0000000180319926 lea       rcx, [rip + 0x189086b] # 0x181baa198
000000018031992D call      0x18024fef0
0000000180319932 lea       rcx, [rip + 0x18909bf] # 0x181baa2f8
0000000180319939 call      0x18024fef0
000000018031993E lea       rcx, [rip + 0x18aa2cb] # 0x181bc3c10
0000000180319945 call      0x18024fef0
000000018031994A lea       rcx, [rip + 0x18a3d1f] # 0x181bbd670
0000000180319951 call      0x18024fef0
0000000180319956 lea       rcx, [rip + 0x1886b93] # 0x181ba04f0
000000018031995D call      0x18024fef0
0000000180319962 mov       byte ptr [rip + 0x19cb241], 1 # 0x181ce4baa
0000000180319969 mov       r14, qword ptr [r13 + 0x10]
000000018031996D test      r14, r14
0000000180319970 je        0x180319d6f
0000000180319976 mov       r14d, dword ptr [r14 + 0x18]
000000018031997A mov       dword ptr [rsp + 0xe0], r14d
0000000180319982 mov       r8, qword ptr [rip + 0x18aa1d7] # 0x181bc3b60
0000000180319989 mov       rdx, qword ptr [r13 + 0x10]
000000018031998D lea       rcx, [rsp + 0x20]
0000000180319992 call      0x1808197f0
0000000180319997 movups    xmm0, xmmword ptr [rsp + 0x20]
000000018031999C movups    xmmword ptr [rsp + 0x38], xmm0
00000001803199A1 movsd     xmm1, qword ptr [rsp + 0x30]
00000001803199A7 movsd     qword ptr [rsp + 0x48], xmm1
00000001803199AD mov       qword ptr [rsp + 0x50], 0
00000001803199B6 lea       rbx, [rsp + 0x38]
00000001803199BB mov       qword ptr [rsp + 0x58], rbx
00000001803199C0 mov       rdx, qword ptr [rip + 0x18b10e9] # 0x181bcaab0
00000001803199C7 lea       rcx, [rsp + 0x38]
00000001803199CC call      0x180669280
00000001803199D1 test      al, al
00000001803199D3 je        0x1803199f6
00000001803199D5 mov       rax, qword ptr [rsp + 0x48]
00000001803199DA test      rax, rax
00000001803199DD je        0x180319d75
00000001803199E3 cmp       dword ptr [rax + 0x18], 3
00000001803199E7 jge       0x1803199c0
00000001803199E9 inc       r14d
00000001803199EC mov       dword ptr [rsp + 0xe0], r14d
00000001803199F4 jmp       0x1803199c0
00000001803199F6 mov       rdx, qword ptr [rip + 0x18b1063] # 0x181bcaa60
00000001803199FD mov       rcx, rbx
0000000180319A00 call      0x180302170
0000000180319A05 jmp       0x180319a36
0000000180319A07 mov       rdx, qword ptr [rip + 0x18b1052] # 0x181bcaa60
0000000180319A0E mov       rcx, qword ptr [rsp + 0x58]
0000000180319A13 call      0x180302170
0000000180319A18 mov       rcx, qword ptr [rsp + 0x50]
0000000180319A1D test      rcx, rcx
0000000180319A20 jne       0x180319d7b
0000000180319A26 mov       r13, qword ptr [rsp + 0xd0]
0000000180319A2E mov       r14d, dword ptr [rsp + 0xe0]
0000000180319A36 mov       rcx, qword ptr [rip + 0x18a3c33] # 0x181bbd670
0000000180319A3D call      0x180250100
0000000180319A42 mov       rdi, rax
0000000180319A45 mov       rdx, qword ptr [rip + 0x189074c] # 0x181baa198
0000000180319A4C mov       rcx, rax
0000000180319A4F call      0x1808053d0
0000000180319A54 mov       qword ptr [rsp + 0x50], rdi
0000000180319A59 mov       rdx, qword ptr [r13 + 0x10]
0000000180319A5D test      rdx, rdx
0000000180319A60 je        0x180319d6f
0000000180319A66 mov       r8, qword ptr [rip + 0x18aa0f3] # 0x181bc3b60
0000000180319A6D lea       rcx, [rsp + 0x20]
0000000180319A72 call      0x1808197f0
0000000180319A77 movups    xmm0, xmmword ptr [rsp + 0x20]
0000000180319A7C movups    xmmword ptr [rsp + 0x38], xmm0
0000000180319A81 movsd     xmm1, qword ptr [rsp + 0x30]
0000000180319A87 movsd     qword ptr [rsp + 0x48], xmm1
0000000180319A8D mov       qword ptr [rsp + 0x20], 0
0000000180319A96 lea       rbx, [rsp + 0x38]
0000000180319A9B mov       qword ptr [rsp + 0x28], rbx
0000000180319AA0 mov       rdx, qword ptr [rip + 0x18b1009] # 0x181bcaab0
0000000180319AA7 lea       rcx, [rsp + 0x38]
0000000180319AAC call      0x180669280
0000000180319AB1 test      al, al
0000000180319AB3 je        0x180319b68
0000000180319AB9 mov       r15, qword ptr [rsp + 0x48]
0000000180319ABE mov       rcx, qword ptr [rip + 0x1884d03] # 0x181b9e7c8
0000000180319AC5 call      0x180250100
0000000180319ACA mov       rsi, rax
0000000180319ACD xor       edx, edx
0000000180319ACF mov       rcx, rax
0000000180319AD2 call      0x18031a880
0000000180319AD7 test      r15, r15
0000000180319ADA je        0x180319d95
0000000180319AE0 mov       eax, dword ptr [r15 + 0x10]
0000000180319AE4 test      rsi, rsi
0000000180319AE7 je        0x180319d90
0000000180319AED mov       dword ptr [rsi + 0x10], eax
0000000180319AF0 mov       dword ptr [rsi + 0x2c], 0x4b18967f
0000000180319AF7 test      rdi, rdi
0000000180319AFA je        0x180319d8b
0000000180319B00 mov       r8, qword ptr [rip + 0x18906e9] # 0x181baa1f0
0000000180319B07 inc       dword ptr [rdi + 0x1c]
0000000180319B0A mov       rcx, qword ptr [rdi + 0x10]
0000000180319B0E movsxd    rdx, dword ptr [rdi + 0x18]
0000000180319B12 test      rcx, rcx
0000000180319B15 je        0x180319d86
0000000180319B1B cmp       edx, dword ptr [rcx + 0x18]
0000000180319B1E jb        0x180319b3f
0000000180319B20 mov       rax, qword ptr [r8 + 0x20]
0000000180319B24 mov       r8, qword ptr [rax + 0xc0]
0000000180319B2B mov       r8, qword ptr [r8 + 0x70]
0000000180319B2F mov       rdx, rsi
0000000180319B32 mov       rcx, rdi
0000000180319B35 call      0x180847ce0
0000000180319B3A jmp       0x180319aa0
0000000180319B3F lea       eax, [rdx + 1]
0000000180319B42 mov       dword ptr [rdi + 0x18], eax
0000000180319B45 cmp       edx, dword ptr [rcx + 0x18]
0000000180319B48 jae       0x180319d81
0000000180319B4E mov       qword ptr [rcx + rdx*8 + 0x20], rsi
0000000180319B53 lea       rcx, [rcx + rdx*8]
0000000180319B57 add       rcx, 0x20
0000000180319B5B mov       rdx, rsi
0000000180319B5E call      0x18024f360
0000000180319B63 jmp       0x180319aa0
0000000180319B68 mov       rdx, qword ptr [rip + 0x18b0ef1] # 0x181bcaa60
0000000180319B6F mov       rcx, rbx
0000000180319B72 call      0x180302170
0000000180319B77 jmp       0x180319bad
0000000180319B79 mov       rdx, qword ptr [rip + 0x18b0ee0] # 0x181bcaa60
0000000180319B80 mov       rcx, qword ptr [rsp + 0x28]
0000000180319B85 call      0x180302170
0000000180319B8A mov       rcx, qword ptr [rsp + 0x20]
0000000180319B8F test      rcx, rcx
0000000180319B92 jne       0x180319d9b
0000000180319B98 mov       r13, qword ptr [rsp + 0xd0]
0000000180319BA0 mov       r14d, dword ptr [rsp + 0xe0]
0000000180319BA8 mov       rdi, qword ptr [rsp + 0x50]
0000000180319BAD xorps     xmm6, xmm6
0000000180319BB0 movss     dword ptr [rsp + 0xe8], xmm6
0000000180319BB9 mov       rdx, qword ptr [r13 + 0x10]
0000000180319BBD test      rdx, rdx
0000000180319BC0 je        0x180319d6f
0000000180319BC6 mov       r8, qword ptr [rip + 0x18a9f93] # 0x181bc3b60
0000000180319BCD lea       rcx, [rsp + 0x20]
0000000180319BD2 call      0x1808197f0
0000000180319BD7 movups    xmm0, xmmword ptr [rsp + 0x20]
0000000180319BDC movups    xmmword ptr [rsp + 0x38], xmm0
0000000180319BE1 movsd     xmm1, qword ptr [rsp + 0x30]
0000000180319BE7 movsd     qword ptr [rsp + 0x48], xmm1
0000000180319BED mov       qword ptr [rsp + 0x20], 0
0000000180319BF6 lea       rbx, [rsp + 0x38]
0000000180319BFB mov       qword ptr [rsp + 0x28], rbx
0000000180319C00 mov       rdx, qword ptr [rip + 0x18b0ea9] # 0x181bcaab0
0000000180319C07 lea       rcx, [rsp + 0x38]
0000000180319C0C call      0x180669280
0000000180319C11 test      al, al
0000000180319C13 je        0x180319c33
0000000180319C15 mov       rax, qword ptr [rsp + 0x48]
0000000180319C1A test      rax, rax
0000000180319C1D je        0x180319da1
0000000180319C23 addss     xmm6, dword ptr [rax + 0x1c]
0000000180319C28 movss     dword ptr [rsp + 0xe8], xmm6
0000000180319C31 jmp       0x180319c00
0000000180319C33 mov       rdx, qword ptr [rip + 0x18b0e26] # 0x181bcaa60
0000000180319C3A mov       rcx, rbx
0000000180319C3D call      0x180302170
0000000180319C42 test      rdi, rdi
0000000180319C45 je        0x180319d6f
0000000180319C4B movsd     xmm7, qword ptr [rip + 0x128de8d] # 0x1815a7ae0
0000000180319C53 jmp       0x180319d05
0000000180319C58 mov       rdx, qword ptr [rip + 0x18b0e01] # 0x181bcaa60
0000000180319C5F mov       rcx, qword ptr [rsp + 0x28]
0000000180319C64 call      0x180302170
0000000180319C69 mov       rcx, qword ptr [rsp + 0x20]
0000000180319C6E test      rcx, rcx
0000000180319C71 jne       0x180319da7
0000000180319C77 mov       r13, qword ptr [rsp + 0xd0]
0000000180319C7F mov       r14d, dword ptr [rsp + 0xe0]
0000000180319C87 mov       rdi, qword ptr [rsp + 0x50]
0000000180319C8C movss     xmm6, dword ptr [rsp + 0xe8]
0000000180319C95 movsd     xmm7, qword ptr [rip + 0x128de43] # 0x1815a7ae0
0000000180319C9D xor       r9d, r9d
0000000180319CA0 movaps    xmm2, xmm6
0000000180319CA3 mov       rdx, qword ptr [r13 + 0x10]
0000000180319CA7 mov       rcx, r13
0000000180319CAA call      0x18031a490
0000000180319CAF mov       rsi, rax
0000000180319CB2 test      rax, rax
0000000180319CB5 je        0x180319d4a
0000000180319CBB mov       rcx, qword ptr [rip + 0x1884b06] # 0x181b9e7c8
0000000180319CC2 call      0x180250100
0000000180319CC7 mov       rbx, rax
0000000180319CCA xor       edx, edx
0000000180319CCC mov       rcx, rax
0000000180319CCF call      0x18031a880
0000000180319CD4 test      rbx, rbx
0000000180319CD7 je        0x180319d6f
0000000180319CDD mov       ecx, dword ptr [rsi + 0x10]
0000000180319CE0 mov       dword ptr [rbx + 0x10], ecx
0000000180319CE3 mov       dword ptr [rbx + 0x2c], 0x4b18967f
0000000180319CEA test      rdi, rdi
0000000180319CED je        0x180319d6f
0000000180319CF3 mov       r8, qword ptr [rip + 0x18904f6] # 0x181baa1f0
0000000180319CFA mov       rdx, rbx
0000000180319CFD mov       rcx, rdi
0000000180319D00 call      0x180002e70
0000000180319D05 movd      xmm1, dword ptr [rdi + 0x18]
0000000180319D0A cvtdq2pd  xmm1, xmm1
0000000180319D0E movd      xmm0, r14d
0000000180319D13 cvtdq2pd  xmm0, xmm0
0000000180319D17 mulsd     xmm0, xmm7
0000000180319D1B comisd    xmm0, xmm1
0000000180319D1F ja        0x180319c9d
0000000180319D25 mov       rax, rdi
0000000180319D28 lea       r11, [rsp + 0xa0]
0000000180319D30 mov       rbx, qword ptr [r11 + 0x38]
0000000180319D34 movaps    xmm6, xmmword ptr [r11 - 0x10]
0000000180319D39 movaps    xmm7, xmmword ptr [r11 - 0x20]
0000000180319D3E mov       rsp, r11
0000000180319D41 pop       r15
0000000180319D43 pop       r14
0000000180319D45 pop       r13
0000000180319D47 pop       rdi
0000000180319D48 pop       rsi
0000000180319D49 ret       
0000000180319D4A mov       rcx, qword ptr [rip + 0x1880ff7] # 0x181b9ad48
0000000180319D51 cmp       dword ptr [rcx + 0xe0], 0
0000000180319D58 jne       0x180319d5f
0000000180319D5A call      0x1802501e0
0000000180319D5F xor       edx, edx
0000000180319D61 mov       rcx, qword ptr [rip + 0x1886788] # 0x181ba04f0
0000000180319D68 call      0x1812e68f0
0000000180319D6D jmp       0x180319d25
0000000180319D6F call      0x180250150
0000000180319D74 nop       
0000000180319D75 call      0x180250150
0000000180319D7A nop       
0000000180319D7B call      0x180200e60
0000000180319D80 nop       
0000000180319D81 call      0x180250140
0000000180319D86 call      0x180250150
0000000180319D8B call      0x180250150
0000000180319D90 call      0x180250150
0000000180319D95 call      0x180250150
0000000180319D9A nop       
0000000180319D9B call      0x180200e60
0000000180319DA0 nop       
0000000180319DA1 call      0x180250150
0000000180319DA6 nop       
0000000180319DA7 call      0x180200e60
0000000180319DAC int3      
0000000180319DAD int3      
0000000180319DAE int3      
0000000180319DAF int3      