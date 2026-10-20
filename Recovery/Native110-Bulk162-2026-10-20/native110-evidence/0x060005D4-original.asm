0000000180396B30 mov       qword ptr [rsp + 0x18], rbp
0000000180396B35 push      rdi
0000000180396B36 sub       rsp, 0x30
0000000180396B3A cmp       byte ptr [rip + 0x194e33a], 0 # 0x181ce4e7b
0000000180396B41 mov       rdi, rdx
0000000180396B44 mov       rbp, rcx
0000000180396B47 jne       0x180396bbc
0000000180396B49 lea       rcx, [rip + 0x18041f8] # 0x181b9ad48
0000000180396B50 call      0x18024fef0
0000000180396B55 lea       rcx, [rip + 0x180b4bc] # 0x181ba2018
0000000180396B5C call      0x18024fef0
0000000180396B61 lea       rcx, [rip + 0x182ca78] # 0x181bc35e0
0000000180396B68 call      0x18024fef0
0000000180396B6D lea       rcx, [rip + 0x182ccd4] # 0x181bc3848
0000000180396B74 call      0x18024fef0
0000000180396B79 lea       rcx, [rip + 0x1821970] # 0x181bb84f0
0000000180396B80 call      0x18024fef0
0000000180396B85 lea       rcx, [rip + 0x1819974] # 0x181bb0500
0000000180396B8C call      0x18024fef0
0000000180396B91 lea       rcx, [rip + 0x181f8e0] # 0x181bb6478
0000000180396B98 call      0x18024fef0
0000000180396B9D lea       rcx, [rip + 0x180e864] # 0x181ba5408
0000000180396BA4 call      0x18024fef0
0000000180396BA9 lea       rcx, [rip + 0x18098e0] # 0x181ba0490
0000000180396BB0 call      0x18024fef0
0000000180396BB5 mov       byte ptr [rip + 0x194e2bf], 1 # 0x181ce4e7b
0000000180396BBC mov       rax, qword ptr [rip + 0x181f8b5] # 0x181bb6478
0000000180396BC3 cmp       dword ptr [rax + 0xe0], 0
0000000180396BCA jne       0x180396bdb
0000000180396BCC mov       rcx, rax
0000000180396BCF call      0x1802501e0
0000000180396BD4 mov       rax, qword ptr [rip + 0x181f89d] # 0x181bb6478
0000000180396BDB mov       rax, qword ptr [rax + 0xb8]
0000000180396BE2 mov       qword ptr [rsp + 0x40], rbx
0000000180396BE7 mov       qword ptr [rsp + 0x48], rsi
0000000180396BEC mov       rcx, qword ptr [rax + 0x160]
0000000180396BF3 test      rdi, rdi
0000000180396BF6 je        0x180396ef8
0000000180396BFC test      rcx, rcx
0000000180396BFF je        0x180396ef8
0000000180396C05 mov       r8, qword ptr [rip + 0x182cc3c] # 0x181bc3848
0000000180396C0C mov       edx, dword ptr [rdi + 0x10]
0000000180396C0F call      0x180825710
0000000180396C14 mov       rcx, qword ptr [rip + 0x18198e5] # 0x181bb0500
0000000180396C1B mov       rbx, rax
0000000180396C1E cmp       dword ptr [rcx + 0xe0], 0
0000000180396C25 jne       0x180396c2c
0000000180396C27 call      0x1802501e0
0000000180396C2C xor       r8d, r8d
0000000180396C2F xor       edx, edx
0000000180396C31 mov       rcx, rbx
0000000180396C34 call      0x18131f760
0000000180396C39 test      al, al
0000000180396C3B jne       0x180396eb0
0000000180396C41 mov       rax, qword ptr [rip + 0x181f830] # 0x181bb6478
0000000180396C48 cmp       dword ptr [rax + 0xe0], 0
0000000180396C4F jne       0x180396c60
0000000180396C51 mov       rcx, rax
0000000180396C54 call      0x1802501e0
0000000180396C59 mov       rax, qword ptr [rip + 0x181f818] # 0x181bb6478
0000000180396C60 mov       rax, qword ptr [rax + 0xb8]
0000000180396C67 mov       rcx, qword ptr [rax + 0x160]
0000000180396C6E test      rcx, rcx
0000000180396C71 je        0x180396ef8
0000000180396C77 mov       r8, qword ptr [rip + 0x182cbca] # 0x181bc3848
0000000180396C7E mov       edx, dword ptr [rdi + 0x10]
0000000180396C81 call      0x180825710
0000000180396C86 mov       rcx, qword ptr [rip + 0x1819873] # 0x181bb0500
0000000180396C8D mov       rbx, rax
0000000180396C90 cmp       dword ptr [rcx + 0xe0], 0
0000000180396C97 jne       0x180396c9e
0000000180396C99 call      0x1802501e0
0000000180396C9E mov       rdx, qword ptr [rip + 0x182184b] # 0x181bb84f0
0000000180396CA5 mov       rcx, rbx
0000000180396CA8 call      0x1804a0690
0000000180396CAD movsd     xmm0, qword ptr [rdi + 0x1c]
0000000180396CB2 mov       rbx, rax
0000000180396CB5 mov       esi, dword ptr [rdi + 0x24]
0000000180396CB8 movsd     qword ptr [rsp + 0x20], xmm0
0000000180396CBE subss     xmm0, dword ptr [rip + 0x12111d2] # 0x1815a7e98
0000000180396CC6 movss     dword ptr [rsp + 0x20], xmm0
0000000180396CCC test      rax, rax
0000000180396CCF je        0x180396ef8
0000000180396CD5 xor       edx, edx
0000000180396CD7 mov       rcx, rax
0000000180396CDA call      0x181304510
0000000180396CDF test      rax, rax
0000000180396CE2 je        0x180396ef8
0000000180396CE8 movsd     xmm0, qword ptr [rsp + 0x20]
0000000180396CEE lea       rdx, [rsp + 0x20]
0000000180396CF3 xor       r8d, r8d
0000000180396CF6 movsd     qword ptr [rsp + 0x20], xmm0
0000000180396CFC mov       rcx, rax
0000000180396CFF mov       dword ptr [rsp + 0x28], esi
0000000180396D03 call      0x18132fbb0
0000000180396D08 xor       edx, edx
0000000180396D0A mov       rcx, rbx
0000000180396D0D call      0x1813044d0
0000000180396D12 test      rax, rax
0000000180396D15 je        0x180396ef8
0000000180396D1B xor       edx, edx
0000000180396D1D mov       rcx, rax
0000000180396D20 call      0x18131c070
0000000180396D25 test      rax, rax
0000000180396D28 je        0x180396ef8
0000000180396D2E mov       rdx, qword ptr [rbp + 0x68]
0000000180396D32 xor       r9d, r9d
0000000180396D35 xor       r8d, r8d
0000000180396D38 mov       rcx, rax
0000000180396D3B call      0x18132e900
0000000180396D40 mov       eax, dword ptr [rdi + 0x14]
0000000180396D43 lea       rcx, [rbx + 0x120]
0000000180396D4A mov       dword ptr [rbx + 0x2c], eax
0000000180396D4D xor       edx, edx
0000000180396D4F mov       eax, dword ptr [rdi + 0x2c]
0000000180396D52 mov       dword ptr [rbx + 0x114], eax
0000000180396D58 mov       qword ptr [rbx + 0x120], 0
0000000180396D63 call      0x18024f360
0000000180396D68 mov       eax, dword ptr [rdi + 0x28]
0000000180396D6B mov       dword ptr [rbx + 0x64], eax
0000000180396D6E mov       rcx, qword ptr [rbp + 0x40]
0000000180396D72 test      rcx, rcx
0000000180396D75 je        0x180396ef8
0000000180396D7B mov       r9, qword ptr [rip + 0x182c85e] # 0x181bc35e0
0000000180396D82 inc       dword ptr [rcx + 0x1c]
0000000180396D85 mov       rdx, qword ptr [rcx + 0x10]
0000000180396D89 movsxd    r8, dword ptr [rcx + 0x18]
0000000180396D8D test      rdx, rdx
0000000180396D90 je        0x180396ef8
0000000180396D96 cmp       r8d, dword ptr [rdx + 0x18]
0000000180396D9A jb        0x180396db5
0000000180396D9C mov       rax, qword ptr [r9 + 0x20]
0000000180396DA0 mov       rdx, rbx
0000000180396DA3 mov       r8, qword ptr [rax + 0xc0]
0000000180396DAA mov       r8, qword ptr [r8 + 0x70]
0000000180396DAE call      0x180847ce0
0000000180396DB3 jmp       0x180396ddb
0000000180396DB5 lea       eax, [r8 + 1]
0000000180396DB9 mov       dword ptr [rcx + 0x18], eax
0000000180396DBC cmp       r8d, dword ptr [rdx + 0x18]
0000000180396DC0 jae       0x180396efe
0000000180396DC6 mov       qword ptr [rdx + r8*8 + 0x20], rbx
0000000180396DCB add       rdx, 0x20
0000000180396DCF lea       rcx, [rdx + r8*8]
0000000180396DD3 mov       rdx, rbx
0000000180396DD6 call      0x18024f360
0000000180396DDB mov       rdx, qword ptr [rbx + 0x1f0]
0000000180396DE2 test      rdx, rdx
0000000180396DE5 je        0x180396ef8
0000000180396DEB xor       r8d, r8d
0000000180396DEE lea       rcx, [rsp + 0x20]
0000000180396DF3 call      0x18132f3a0
0000000180396DF8 mov       rdx, qword ptr [rbx + 0x1f0]
0000000180396DFF movss     xmm0, dword ptr [rax]
0000000180396E03 movss     dword ptr [rbx + 0x30], xmm0
0000000180396E08 test      rdx, rdx
0000000180396E0B je        0x180396ef8
0000000180396E11 xor       r8d, r8d
0000000180396E14 lea       rcx, [rsp + 0x20]
0000000180396E19 call      0x18132f3a0
0000000180396E1E xor       edx, edx
0000000180396E20 mov       rcx, rbx
0000000180396E23 movss     xmm0, dword ptr [rax + 4]
0000000180396E28 movss     dword ptr [rbx + 0x34], xmm0
0000000180396E2D call      0x180367440
0000000180396E32 mov       byte ptr [rbx + 0xbd], 0
0000000180396E39 mov       rax, qword ptr [rip + 0x180b1d8] # 0x181ba2018
0000000180396E40 cmp       dword ptr [rax + 0xe0], 0
0000000180396E47 jne       0x180396e58
0000000180396E49 mov       rcx, rax
0000000180396E4C call      0x1802501e0
0000000180396E51 mov       rax, qword ptr [rip + 0x180b1c0] # 0x181ba2018
0000000180396E58 mov       rax, qword ptr [rax + 0xb8]
0000000180396E5F mov       rax, qword ptr [rax]
0000000180396E62 test      rax, rax
0000000180396E65 je        0x180396ef8
0000000180396E6B mov       rax, qword ptr [rax + 0x28]
0000000180396E6F test      rax, rax
0000000180396E72 je        0x180396ef8
0000000180396E78 mov       rcx, qword ptr [rax + 0x10]
0000000180396E7C test      rcx, rcx
0000000180396E7F je        0x180396ef8
0000000180396E81 mov       rcx, qword ptr [rcx + 0x80]
0000000180396E88 test      rcx, rcx
0000000180396E8B je        0x180396ef8
0000000180396E8D movsxd    rax, dword ptr [rdi + 0x10]
0000000180396E91 cmp       eax, dword ptr [rcx + 0x18]
0000000180396E94 jae       0x180396efe
0000000180396E96 mov       byte ptr [rax + rcx + 0x20], 1
0000000180396E9B mov       rsi, qword ptr [rsp + 0x48]
0000000180396EA0 mov       rbx, qword ptr [rsp + 0x40]
0000000180396EA5 mov       rbp, qword ptr [rsp + 0x50]
0000000180396EAA add       rsp, 0x30
0000000180396EAE pop       rdi
0000000180396EAF ret       
0000000180396EB0 lea       rcx, [rdi + 0x10]
0000000180396EB4 xor       edx, edx
0000000180396EB6 call      0x180ca3820
0000000180396EBB mov       r8, qword ptr [rip + 0x180e546] # 0x181ba5408
0000000180396EC2 xor       r9d, r9d
0000000180396EC5 mov       rcx, qword ptr [rip + 0x18095c4] # 0x181ba0490
0000000180396ECC mov       rdx, rax
0000000180396ECF call      0x180b73f50
0000000180396ED4 mov       rcx, qword ptr [rip + 0x1803e6d] # 0x181b9ad48
0000000180396EDB mov       rbx, rax
0000000180396EDE cmp       dword ptr [rcx + 0xe0], 0
0000000180396EE5 jne       0x180396eec
0000000180396EE7 call      0x1802501e0
0000000180396EEC xor       edx, edx
0000000180396EEE mov       rcx, rbx
0000000180396EF1 call      0x1812e68f0
0000000180396EF6 jmp       0x180396e9b
0000000180396EF8 call      0x180250150
0000000180396EFD int3      
0000000180396EFE call      0x180250140
0000000180396F03 int3      