0000000180305B00 mov       qword ptr [rsp + 0x18], rbx
0000000180305B05 mov       dword ptr [rsp + 0x10], edx
0000000180305B09 push      rbp
0000000180305B0A push      rsi
0000000180305B0B push      rdi
0000000180305B0C sub       rsp, 0x20
0000000180305B10 cmp       byte ptr [rip + 0x19defbb], 0 # 0x181ce4ad2
0000000180305B17 mov       rbx, rcx
0000000180305B1A jne       0x180305b53
0000000180305B1C lea       rcx, [rip + 0x18ae37d] # 0x181bb3ea0
0000000180305B23 call      0x18024fef0
0000000180305B28 lea       rcx, [rip + 0x18ae3c9] # 0x181bb3ef8
0000000180305B2F call      0x18024fef0
0000000180305B34 lea       rcx, [rip + 0x18aa9c5] # 0x181bb0500
0000000180305B3B call      0x18024fef0
0000000180305B40 lea       rcx, [rip + 0x18b51b1] # 0x181bbacf8
0000000180305B47 call      0x18024fef0
0000000180305B4C mov       byte ptr [rip + 0x19def7f], 1 # 0x181ce4ad2
0000000180305B53 mov       rcx, qword ptr [rip + 0x18aa9a6] # 0x181bb0500
0000000180305B5A xor       edi, edi
0000000180305B5C mov       rsi, qword ptr [rbx + 0x20]
0000000180305B60 mov       ebp, edi
0000000180305B62 cmp       dword ptr [rcx + 0xe0], edi
0000000180305B68 jne       0x180305b6f
0000000180305B6A call      0x1802501e0
0000000180305B6F xor       r8d, r8d
0000000180305B72 xor       edx, edx
0000000180305B74 mov       rcx, rsi
0000000180305B77 call      0x18131f760
0000000180305B7C test      al, al
0000000180305B7E jne       0x180305ceb
0000000180305B84 mov       ecx, dword ptr [rsp + 0x48]
0000000180305B88 test      ecx, ecx
0000000180305B8A jns       0x180305b95
0000000180305B8C mov       ecx, 0xffffffff
0000000180305B91 mov       dword ptr [rsp + 0x48], ecx
0000000180305B95 mov       rax, qword ptr [rbx + 0x20]
0000000180305B99 test      rax, rax
0000000180305B9C je        0x180306042
0000000180305BA2 mov       rdx, qword ptr [rax + 0x50]
0000000180305BA6 test      rdx, rdx
0000000180305BA9 je        0x180306042
0000000180305BAF cmp       ecx, dword ptr [rdx + 0x18]
0000000180305BB2 jl        0x180305bbd
0000000180305BB4 mov       ecx, dword ptr [rdx + 0x18]
0000000180305BB7 dec       ecx
0000000180305BB9 mov       dword ptr [rsp + 0x48], ecx
0000000180305BBD test      ecx, ecx
0000000180305BBF js        0x180305bef
0000000180305BC1 test      rax, rax
0000000180305BC4 je        0x180306042
0000000180305BCA mov       r9, qword ptr [rax + 0x50]
0000000180305BCE test      r9, r9
0000000180305BD1 je        0x180306042
0000000180305BD7 mov       r8, qword ptr [rip + 0x18ae31a] # 0x181bb3ef8
0000000180305BDE mov       edx, ecx
0000000180305BE0 mov       rcx, r9
0000000180305BE3 call      0x180825710
0000000180305BE8 mov       ecx, dword ptr [rsp + 0x48]
0000000180305BEC mov       rbp, rax
0000000180305BEF mov       rdx, qword ptr [rbx + 0x20]
0000000180305BF3 test      rdx, rdx
0000000180305BF6 je        0x180306042
0000000180305BFC mov       rax, qword ptr [rdx + 0x50]
0000000180305C00 test      rax, rax
0000000180305C03 je        0x180306042
0000000180305C09 mov       r9, qword ptr [rbx + 0x80]
0000000180305C10 test      r9, r9
0000000180305C13 je        0x180306042
0000000180305C19 mov       eax, dword ptr [rax + 0x18]
0000000180305C1C dec       eax
0000000180305C1E cmp       ecx, eax
0000000180305C20 mov       rcx, r9
0000000180305C23 setl      dl
0000000180305C26 xor       r8d, r8d
0000000180305C29 call      0x18156cc70
0000000180305C2E mov       rcx, qword ptr [rbx + 0x88]
0000000180305C35 test      rcx, rcx
0000000180305C38 je        0x180306042
0000000180305C3E cmp       dword ptr [rsp + 0x48], edi
0000000180305C42 setge     dl
0000000180305C45 xor       r8d, r8d
0000000180305C48 call      0x18156cc70
0000000180305C4D mov       rax, qword ptr [rbx + 0x20]
0000000180305C51 mov       ecx, edi
0000000180305C53 test      rax, rax
0000000180305C56 je        0x180306042
0000000180305C5C nop       dword ptr [rax]
0000000180305C60 mov       rax, qword ptr [rax + 0x50]
0000000180305C64 test      rax, rax
0000000180305C67 je        0x180306042
0000000180305C6D cmp       ecx, dword ptr [rax + 0x18]
0000000180305C70 jge       0x180305d28
0000000180305C76 mov       rax, qword ptr [rbx + 0x20]
0000000180305C7A test      rax, rax
0000000180305C7D je        0x180306042
0000000180305C83 mov       rcx, qword ptr [rax + 0x50]
0000000180305C87 test      rcx, rcx
0000000180305C8A je        0x180306042
0000000180305C90 mov       r8, qword ptr [rip + 0x18ae261] # 0x181bb3ef8
0000000180305C97 mov       edx, edi
0000000180305C99 call      0x180825710
0000000180305C9E test      rax, rax
0000000180305CA1 je        0x180306042
0000000180305CA7 mov       rsi, qword ptr [rax + 0x30]
0000000180305CAB lea       rcx, [rsp + 0x40]
0000000180305CB0 xor       edx, edx
0000000180305CB2 mov       dword ptr [rsp + 0x40], edi
0000000180305CB6 call      0x180ca3820
0000000180305CBB test      rsi, rsi
0000000180305CBE je        0x180306048
0000000180305CC4 xor       r9d, r9d
0000000180305CC7 mov       r8b, 1
0000000180305CCA mov       rdx, rax
0000000180305CCD mov       rcx, rsi
0000000180305CD0 call      0x1811af070
0000000180305CD5 mov       rax, qword ptr [rbx + 0x20]
0000000180305CD9 inc       edi
0000000180305CDB mov       ecx, edi
0000000180305CDD test      rax, rax
0000000180305CE0 je        0x180306042
0000000180305CE6 jmp       0x180305c60
0000000180305CEB mov       ecx, 0xffffffff
0000000180305CF0 mov       dword ptr [rsp + 0x48], ecx
0000000180305CF4 mov       rcx, qword ptr [rbx + 0x80]
0000000180305CFB test      rcx, rcx
0000000180305CFE je        0x180306042
0000000180305D04 xor       r8d, r8d
0000000180305D07 xor       edx, edx
0000000180305D09 call      0x18156cc70
0000000180305D0E mov       rcx, qword ptr [rbx + 0x88]
0000000180305D15 test      rcx, rcx
0000000180305D18 je        0x180306042
0000000180305D1E xor       r8d, r8d
0000000180305D21 xor       edx, edx
0000000180305D23 call      0x18156cc70
0000000180305D28 mov       eax, dword ptr [rsp + 0x48]
0000000180305D2C mov       rdi, qword ptr [rbx + 0x28]
0000000180305D30 mov       dword ptr [rbx + 0x38], eax
0000000180305D33 mov       rcx, qword ptr [rip + 0x18aa7c6] # 0x181bb0500
0000000180305D3A cmp       dword ptr [rcx + 0xe0], 0
0000000180305D41 jne       0x180305d48
0000000180305D43 call      0x1802501e0
0000000180305D48 xor       r8d, r8d
0000000180305D4B xor       edx, edx
0000000180305D4D mov       rcx, rdi
0000000180305D50 call      0x18131f900
0000000180305D55 test      al, al
0000000180305D57 je        0x180305d90
0000000180305D59 mov       rax, qword ptr [rbx + 0x28]
0000000180305D5D test      rax, rax
0000000180305D60 je        0x180306042
0000000180305D66 mov       rcx, qword ptr [rax + 0x28]
0000000180305D6A test      rcx, rcx
0000000180305D6D je        0x180306042
0000000180305D73 xor       edx, edx
0000000180305D75 call      0x1813044d0
0000000180305D7A test      rax, rax
0000000180305D7D je        0x180306042
0000000180305D83 xor       r8d, r8d
0000000180305D86 xor       edx, edx
0000000180305D88 mov       rcx, rax
0000000180305D8B call      0x18131bad0
0000000180305D90 lea       rcx, [rbx + 0x28]
0000000180305D94 mov       qword ptr [rbx + 0x28], rbp
0000000180305D98 mov       rdx, rbp
0000000180305D9B call      0x18024f360
0000000180305DA0 mov       rdi, qword ptr [rbx + 0x40]
0000000180305DA4 lea       rcx, [rsp + 0x48]
0000000180305DA9 xor       edx, edx
0000000180305DAB call      0x180ca3820
0000000180305DB0 test      rdi, rdi
0000000180305DB3 je        0x180306042
0000000180305DB9 xor       r8d, r8d
0000000180305DBC mov       rdx, rax
0000000180305DBF mov       rcx, rdi
0000000180305DC2 call      0x181197d70
0000000180305DC7 mov       rcx, qword ptr [rip + 0x18aa732] # 0x181bb0500
0000000180305DCE mov       rdi, qword ptr [rbx + 0x28]
0000000180305DD2 cmp       dword ptr [rcx + 0xe0], 0
0000000180305DD9 jne       0x180305de0
0000000180305DDB call      0x1802501e0
0000000180305DE0 xor       r8d, r8d
0000000180305DE3 xor       edx, edx
0000000180305DE5 mov       rcx, rdi
0000000180305DE8 call      0x18131f760
0000000180305DED test      al, al
0000000180305DEF jne       0x180305f51
0000000180305DF5 mov       rdi, qword ptr [rbx + 0x28]
0000000180305DF9 test      rdi, rdi
0000000180305DFC je        0x180306042
0000000180305E02 mov       rdi, qword ptr [rdi + 0x40]
0000000180305E06 mov       rsi, qword ptr [rbx + 0x48]
0000000180305E0A test      rdi, rdi
0000000180305E0D je        0x180306042
0000000180305E13 lea       rcx, [rdi + 0x10]
0000000180305E17 xor       edx, edx
0000000180305E19 call      0x180ca3820
0000000180305E1E test      rsi, rsi
0000000180305E21 je        0x180306042
0000000180305E27 xor       r8d, r8d
0000000180305E2A mov       rdx, rax
0000000180305E2D mov       rcx, rsi
0000000180305E30 call      0x181197d70
0000000180305E35 mov       rsi, qword ptr [rbx + 0x50]
0000000180305E39 lea       rcx, [rdi + 0x14]
0000000180305E3D xor       edx, edx
0000000180305E3F call      0x180ca3820
0000000180305E44 test      rsi, rsi
0000000180305E47 je        0x180306042
0000000180305E4D xor       r8d, r8d
0000000180305E50 mov       rdx, rax
0000000180305E53 mov       rcx, rsi
0000000180305E56 call      0x181197d70
0000000180305E5B mov       rsi, qword ptr [rbx + 0x58]
0000000180305E5F lea       rcx, [rdi + 0x24]
0000000180305E63 xor       edx, edx
0000000180305E65 call      0x180cc5b50
0000000180305E6A test      rsi, rsi
0000000180305E6D je        0x180306042
0000000180305E73 xor       r8d, r8d
0000000180305E76 mov       rdx, rax
0000000180305E79 mov       rcx, rsi
0000000180305E7C call      0x181197d70
0000000180305E81 mov       rcx, qword ptr [rbx + 0x60]
0000000180305E85 test      rcx, rcx
0000000180305E88 je        0x180306042
0000000180305E8E movzx     edx, byte ptr [rdi + 0x29]
0000000180305E92 xor       r8d, r8d
0000000180305E95 call      0x181573290
0000000180305E9A mov       rcx, qword ptr [rbx + 0x48]
0000000180305E9E test      rcx, rcx
0000000180305EA1 je        0x180306042
0000000180305EA7 xor       r8d, r8d
0000000180305EAA mov       dl, 1
0000000180305EAC call      0x18156cc70
0000000180305EB1 mov       rcx, qword ptr [rbx + 0x50]
0000000180305EB5 test      rcx, rcx
0000000180305EB8 je        0x180306042
0000000180305EBE xor       r8d, r8d
0000000180305EC1 mov       dl, 1
0000000180305EC3 call      0x18156cc70
0000000180305EC8 mov       rcx, qword ptr [rbx + 0x58]
0000000180305ECC test      rcx, rcx
0000000180305ECF je        0x180306042
0000000180305ED5 xor       r8d, r8d
0000000180305ED8 mov       dl, 1
0000000180305EDA call      0x18156cc70
0000000180305EDF mov       rcx, qword ptr [rbx + 0x60]
0000000180305EE3 test      rcx, rcx
0000000180305EE6 je        0x180306042
0000000180305EEC xor       r8d, r8d
0000000180305EEF mov       dl, 1
0000000180305EF1 call      0x18156cc70
0000000180305EF6 mov       rcx, qword ptr [rbx + 0x78]
0000000180305EFA test      rcx, rcx
0000000180305EFD je        0x180306042
0000000180305F03 xor       r8d, r8d
0000000180305F06 mov       dl, 1
0000000180305F08 call      0x18156cc70
0000000180305F0D mov       rcx, qword ptr [rbx + 0x28]
0000000180305F11 test      rcx, rcx
0000000180305F14 je        0x180306042
0000000180305F1A mov       rcx, qword ptr [rcx + 0x28]
0000000180305F1E test      rcx, rcx
0000000180305F21 je        0x180306042
0000000180305F27 xor       edx, edx
0000000180305F29 call      0x1813044d0
0000000180305F2E test      rax, rax
0000000180305F31 je        0x180306042
0000000180305F37 xor       r8d, r8d
0000000180305F3A mov       dl, 1
0000000180305F3C mov       rcx, rax
0000000180305F3F call      0x18131bad0
0000000180305F44 mov       rbx, qword ptr [rsp + 0x50]
0000000180305F49 add       rsp, 0x20
0000000180305F4D pop       rdi
0000000180305F4E pop       rsi
0000000180305F4F pop       rbp
0000000180305F50 ret       
0000000180305F51 mov       rcx, qword ptr [rbx + 0x48]
0000000180305F55 test      rcx, rcx
0000000180305F58 je        0x180306042
0000000180305F5E mov       rax, qword ptr [rip + 0x18b4d93] # 0x181bbacf8
0000000180305F65 xor       r8d, r8d
0000000180305F68 mov       rdx, qword ptr [rax + 0xb8]
0000000180305F6F mov       rdx, qword ptr [rdx]
0000000180305F72 call      0x181197d70
0000000180305F77 mov       rcx, qword ptr [rbx + 0x50]
0000000180305F7B test      rcx, rcx
0000000180305F7E je        0x180306042
0000000180305F84 mov       rax, qword ptr [rip + 0x18b4d6d] # 0x181bbacf8
0000000180305F8B xor       r8d, r8d
0000000180305F8E mov       rdx, qword ptr [rax + 0xb8]
0000000180305F95 mov       rdx, qword ptr [rdx]
0000000180305F98 call      0x181197d70
0000000180305F9D mov       rcx, qword ptr [rbx + 0x58]
0000000180305FA1 test      rcx, rcx
0000000180305FA4 je        0x180306042
0000000180305FAA mov       rax, qword ptr [rip + 0x18b4d47] # 0x181bbacf8
0000000180305FB1 xor       r8d, r8d
0000000180305FB4 mov       rdx, qword ptr [rax + 0xb8]
0000000180305FBB mov       rdx, qword ptr [rdx]
0000000180305FBE call      0x181197d70
0000000180305FC3 mov       rcx, qword ptr [rbx + 0x60]
0000000180305FC7 test      rcx, rcx
0000000180305FCA je        0x180306042
0000000180305FCC xor       r8d, r8d
0000000180305FCF xor       edx, edx
0000000180305FD1 call      0x181573290
0000000180305FD6 mov       rcx, qword ptr [rbx + 0x48]
0000000180305FDA test      rcx, rcx
0000000180305FDD je        0x180306042
0000000180305FDF xor       r8d, r8d
0000000180305FE2 xor       edx, edx
0000000180305FE4 call      0x18156cc70
0000000180305FE9 mov       rcx, qword ptr [rbx + 0x50]
0000000180305FED test      rcx, rcx
0000000180305FF0 je        0x180306042
0000000180305FF2 xor       r8d, r8d
0000000180305FF5 xor       edx, edx
0000000180305FF7 call      0x18156cc70
0000000180305FFC mov       rcx, qword ptr [rbx + 0x58]
0000000180306000 test      rcx, rcx
0000000180306003 je        0x180306042
0000000180306005 xor       r8d, r8d
0000000180306008 xor       edx, edx
000000018030600A call      0x18156cc70
000000018030600F mov       rcx, qword ptr [rbx + 0x60]
0000000180306013 test      rcx, rcx
0000000180306016 je        0x180306042
0000000180306018 xor       r8d, r8d
000000018030601B xor       edx, edx
000000018030601D call      0x18156cc70
0000000180306022 mov       rcx, qword ptr [rbx + 0x78]
0000000180306026 test      rcx, rcx
0000000180306029 je        0x180306042
000000018030602B xor       r8d, r8d
000000018030602E xor       edx, edx
0000000180306030 call      0x18156cc70
0000000180306035 mov       rbx, qword ptr [rsp + 0x50]
000000018030603A add       rsp, 0x20
000000018030603E pop       rdi
000000018030603F pop       rsi
0000000180306040 pop       rbp
0000000180306041 ret       
0000000180306042 call      0x180250150
0000000180306047 int3      
0000000180306048 call      0x180250150
000000018030604D int3      
000000018030604E int3      
000000018030604F int3      