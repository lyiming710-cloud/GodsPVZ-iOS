000000018038B880 push      rsi
000000018038B882 sub       rsp, 0x60
000000018038B886 cmp       byte ptr [rip + 0x19595b5], 0 # 0x181ce4e42
000000018038B88D mov       rsi, rcx
000000018038B890 jne       0x18038b8ed
000000018038B892 lea       rcx, [rip + 0x181677f] # 0x181ba2018
000000018038B899 call      0x18024fef0
000000018038B89E lea       rcx, [rip + 0x1831ccb] # 0x181bbd570
000000018038B8A5 call      0x18024fef0
000000018038B8AA lea       rcx, [rip + 0x1831d17] # 0x181bbd5c8
000000018038B8B1 call      0x18024fef0
000000018038B8B6 lea       rcx, [rip + 0x1831dbb] # 0x181bbd678
000000018038B8BD call      0x18024fef0
000000018038B8C2 lea       rcx, [rip + 0x1828f67] # 0x181bb4830
000000018038B8C9 call      0x18024fef0
000000018038B8CE lea       rcx, [rip + 0x1831dfb] # 0x181bbd6d0
000000018038B8D5 call      0x18024fef0
000000018038B8DA lea       rcx, [rip + 0x1824c1f] # 0x181bb0500
000000018038B8E1 call      0x18024fef0
000000018038B8E6 mov       byte ptr [rip + 0x1959555], 1 # 0x181ce4e42
000000018038B8ED mov       rax, qword ptr [rsi + 0x60]
000000018038B8F1 mov       qword ptr [rsp + 0x70], rbx
000000018038B8F6 mov       qword ptr [rsp + 0x78], rbp
000000018038B8FB mov       qword ptr [rsp + 0x80], rdi
000000018038B903 mov       qword ptr [rsp + 0x88], r12
000000018038B90B mov       qword ptr [rsp + 0x58], r14
000000018038B910 mov       qword ptr [rsp + 0x50], r15
000000018038B915 movaps    xmmword ptr [rsp + 0x40], xmm6
000000018038B91A movaps    xmmword ptr [rsp + 0x30], xmm7
000000018038B91F test      rax, rax
000000018038B922 je        0x18038bda0
000000018038B928 mov       ecx, dword ptr [rax + 0x58]
000000018038B92B xor       r12d, r12d
000000018038B92E mov       ebp, dword ptr [rax + 0x54]
000000018038B931 xor       r15d, r15d
000000018038B934 movsxd    rdi, dword ptr [rax + 0x44]
000000018038B938 cmp       dword ptr [rsi + 0xa8], ecx
000000018038B93E jle       0x18038ba94
000000018038B944 mov       r14d, ecx
000000018038B947 mov       ecx, ebp
000000018038B949 test      ebp, ebp
000000018038B94B je        0x18038b97d
000000018038B94D sub       ecx, 1
000000018038B950 je        0x18038b976
000000018038B952 sub       ecx, 1
000000018038B955 je        0x18038b96f
000000018038B957 sub       ecx, 1
000000018038B95A je        0x18038b968
000000018038B95C cmp       ecx, 1
000000018038B95F jne       0x18038b97d
000000018038B961 mov       eax, 0x87
000000018038B966 jmp       0x18038b982
000000018038B968 mov       eax, 0x56
000000018038B96D jmp       0x18038b982
000000018038B96F mov       eax, 0x2f
000000018038B974 jmp       0x18038b982
000000018038B976 mov       eax, 0x12
000000018038B97B jmp       0x18038b982
000000018038B97D mov       eax, 0xffffffff
000000018038B982 lea       ebx, [r14 + rax]
000000018038B986 mov       rax, qword ptr [rip + 0x181668b] # 0x181ba2018
000000018038B98D cmp       dword ptr [rax + 0xe0], 0
000000018038B994 jne       0x18038b9a5
000000018038B996 mov       rcx, rax
000000018038B999 call      0x1802501e0
000000018038B99E mov       rax, qword ptr [rip + 0x1816673] # 0x181ba2018
000000018038B9A5 mov       rax, qword ptr [rax + 0xb8]
000000018038B9AC mov       rax, qword ptr [rax]
000000018038B9AF test      rax, rax
000000018038B9B2 je        0x18038bda0
000000018038B9B8 mov       rax, qword ptr [rax + 0x28]
000000018038B9BC test      rax, rax
000000018038B9BF je        0x18038bda0
000000018038B9C5 mov       rcx, qword ptr [rax + 0x20]
000000018038B9C9 test      rcx, rcx
000000018038B9CC je        0x18038bda0
000000018038B9D2 mov       rcx, qword ptr [rcx + 0x28]
000000018038B9D6 test      rcx, rcx
000000018038B9D9 je        0x18038bda0
000000018038B9DF mov       r8, qword ptr [rip + 0x1828e4a] # 0x181bb4830
000000018038B9E6 mov       edx, ebx
000000018038B9E8 call      0x180825710
000000018038B9ED test      rax, rax
000000018038B9F0 je        0x18038bda0
000000018038B9F6 mov       rcx, qword ptr [rax + 0x10]
000000018038B9FA test      rcx, rcx
000000018038B9FD je        0x18038bda0
000000018038BA03 cmp       edi, dword ptr [rcx + 0x18]
000000018038BA06 jae       0x18038bda6
000000018038BA0C mov       rax, qword ptr [rip + 0x1816605] # 0x181ba2018
000000018038BA13 add       r12d, dword ptr [rcx + rdi*4 + 0x20]
000000018038BA18 mov       rcx, qword ptr [rax + 0xb8]
000000018038BA1F mov       rax, qword ptr [rcx]
000000018038BA22 test      rax, rax
000000018038BA25 je        0x18038bda0
000000018038BA2B mov       rax, qword ptr [rax + 0x28]
000000018038BA2F test      rax, rax
000000018038BA32 je        0x18038bda0
000000018038BA38 mov       rcx, qword ptr [rax + 0x20]
000000018038BA3C test      rcx, rcx
000000018038BA3F je        0x18038bda0
000000018038BA45 mov       rcx, qword ptr [rcx + 0x28]
000000018038BA49 test      rcx, rcx
000000018038BA4C je        0x18038bda0
000000018038BA52 mov       r8, qword ptr [rip + 0x1828dd7] # 0x181bb4830
000000018038BA59 mov       edx, ebx
000000018038BA5B call      0x180825710
000000018038BA60 test      rax, rax
000000018038BA63 je        0x18038bda0
000000018038BA69 mov       rcx, qword ptr [rax + 0x18]
000000018038BA6D test      rcx, rcx
000000018038BA70 je        0x18038bda0
000000018038BA76 cmp       edi, dword ptr [rcx + 0x18]
000000018038BA79 jae       0x18038bda6
000000018038BA7F add       r15d, dword ptr [rcx + rdi*4 + 0x20]
000000018038BA84 inc       r14d
000000018038BA87 cmp       r14d, dword ptr [rsi + 0xa8]
000000018038BA8E jl        0x18038b947
000000018038BA94 mov       rax, qword ptr [rsi + 0x168]
000000018038BA9B xor       ebx, ebx
000000018038BA9D xor       edx, edx
000000018038BA9F test      rax, rax
000000018038BAA2 je        0x18038bda0
000000018038BAA8 nop       dword ptr [rax + rax]
000000018038BAB0 mov       rcx, qword ptr [rsi + 0x168]
000000018038BAB7 cmp       edx, dword ptr [rax + 0x18]
000000018038BABA jge       0x18038bb1e
000000018038BABC test      rcx, rcx
000000018038BABF je        0x18038bda0
000000018038BAC5 mov       r8, qword ptr [rip + 0x1831c04] # 0x181bbd6d0
000000018038BACC mov       edx, ebx
000000018038BACE call      0x180825710
000000018038BAD3 test      rax, rax
000000018038BAD6 je        0x18038bda0
000000018038BADC xor       edx, edx
000000018038BADE mov       rcx, rax
000000018038BAE1 call      0x1813044d0
000000018038BAE6 mov       rcx, qword ptr [rip + 0x1824a13] # 0x181bb0500
000000018038BAED mov       rdi, rax
000000018038BAF0 cmp       dword ptr [rcx + 0xe0], 0
000000018038BAF7 jne       0x18038bafe
000000018038BAF9 call      0x1802501e0
000000018038BAFE xor       edx, edx
000000018038BB00 mov       rcx, rdi
000000018038BB03 call      0x18131e940
000000018038BB08 mov       rax, qword ptr [rsi + 0x168]
000000018038BB0F inc       ebx
000000018038BB11 mov       edx, ebx
000000018038BB13 test      rax, rax
000000018038BB16 je        0x18038bda0
000000018038BB1C jmp       0x18038bab0
000000018038BB1E test      rcx, rcx
000000018038BB21 je        0x18038bda0
000000018038BB27 mov       r8d, dword ptr [rcx + 0x18]
000000018038BB2B inc       dword ptr [rcx + 0x1c]
000000018038BB2E mov       dword ptr [rcx + 0x18], 0
000000018038BB35 test      r8d, r8d
000000018038BB38 jle       0x18038bb48
000000018038BB3A mov       rcx, qword ptr [rcx + 0x10]
000000018038BB3E xor       r9d, r9d
000000018038BB41 xor       edx, edx
000000018038BB43 call      0x180cb86f0
000000018038BB48 mov       rcx, qword ptr [rip + 0x18164c9] # 0x181ba2018
000000018038BB4F cmp       dword ptr [rcx + 0xe0], 0
000000018038BB56 jne       0x18038bb64
000000018038BB58 call      0x1802501e0
000000018038BB5D mov       rcx, qword ptr [rip + 0x18164b4] # 0x181ba2018
000000018038BB64 mov       rcx, qword ptr [rcx + 0xb8]
000000018038BB6B mov       rcx, qword ptr [rcx]
000000018038BB6E test      rcx, rcx
000000018038BB71 je        0x18038bda0
000000018038BB77 mov       rcx, qword ptr [rcx + 0x28]
000000018038BB7B test      rcx, rcx
000000018038BB7E je        0x18038bda0
000000018038BB84 mov       rcx, qword ptr [rcx + 0x10]
000000018038BB88 test      rcx, rcx
000000018038BB8B je        0x18038bda0
000000018038BB91 mov       eax, ebp
000000018038BB93 xor       r8d, r8d
000000018038BB96 cdq       
000000018038BB97 sub       eax, edx
000000018038BB99 sar       eax, 1
000000018038BB9B mov       edx, eax
000000018038BB9D call      0x180380100
000000018038BBA2 mov       rbx, qword ptr [rsi + 0x168]
000000018038BBA9 test      rax, rax
000000018038BBAC je        0x18038bda0
000000018038BBB2 xor       r8d, r8d
000000018038BBB5 mov       dl, 1
000000018038BBB7 mov       rcx, rax
000000018038BBBA call      0x180386d30
000000018038BBBF mov       r9, rax
000000018038BBC2 test      rbx, rbx
000000018038BBC5 je        0x18038bda0
000000018038BBCB mov       rax, qword ptr [rip + 0x183199e] # 0x181bbd570
000000018038BBD2 inc       dword ptr [rbx + 0x1c]
000000018038BBD5 mov       rcx, qword ptr [rbx + 0x10]
000000018038BBD9 movsxd    rdx, dword ptr [rbx + 0x18]
000000018038BBDD test      rcx, rcx
000000018038BBE0 je        0x18038bda0
000000018038BBE6 cmp       edx, dword ptr [rcx + 0x18]
000000018038BBE9 jb        0x18038bc07
000000018038BBEB mov       rax, qword ptr [rax + 0x20]
000000018038BBEF mov       rdx, r9
000000018038BBF2 mov       rcx, rbx
000000018038BBF5 mov       r8, qword ptr [rax + 0xc0]
000000018038BBFC mov       r8, qword ptr [r8 + 0x70]
000000018038BC00 call      0x180847ce0
000000018038BC05 jmp       0x18038bc2b
000000018038BC07 lea       eax, [rdx + 1]
000000018038BC0A mov       dword ptr [rbx + 0x18], eax
000000018038BC0D cmp       edx, dword ptr [rcx + 0x18]
000000018038BC10 jae       0x18038bda6
000000018038BC16 mov       qword ptr [rcx + rdx*8 + 0x20], r9
000000018038BC1B lea       rcx, [rcx + rdx*8]
000000018038BC1F add       rcx, 0x20
000000018038BC23 mov       rdx, r9
000000018038BC26 call      0x18024f360
000000018038BC2B mov       rbx, qword ptr [rsi + 0x168]
000000018038BC32 test      rbx, rbx
000000018038BC35 je        0x18038bda0
000000018038BC3B mov       ebx, dword ptr [rbx + 0x18]
000000018038BC3E mov       r8, qword ptr [rip + 0x1831a8b] # 0x181bbd6d0
000000018038BC45 dec       ebx
000000018038BC47 mov       rcx, qword ptr [rsi + 0x168]
000000018038BC4E mov       edx, ebx
000000018038BC50 call      0x180825710
000000018038BC55 test      rax, rax
000000018038BC58 je        0x18038bda0
000000018038BC5E xor       edx, edx
000000018038BC60 mov       rcx, rax
000000018038BC63 call      0x1813044d0
000000018038BC68 test      rax, rax
000000018038BC6B je        0x18038bda0
000000018038BC71 xor       edx, edx
000000018038BC73 mov       rcx, rax
000000018038BC76 call      0x18131c070
000000018038BC7B mov       rcx, qword ptr [rsi + 0x160]
000000018038BC82 mov       rdi, rax
000000018038BC85 test      rcx, rcx
000000018038BC88 je        0x18038bda0
000000018038BC8E xor       edx, edx
000000018038BC90 call      0x18131c070
000000018038BC95 test      rdi, rdi
000000018038BC98 je        0x18038bda0
000000018038BC9E xor       r9d, r9d
000000018038BCA1 xor       r8d, r8d
000000018038BCA4 mov       rdx, rax
000000018038BCA7 mov       rcx, rdi
000000018038BCAA call      0x18132e900
000000018038BCAF mov       rcx, qword ptr [rsi + 0x168]
000000018038BCB6 xor       edi, edi
000000018038BCB8 movss     xmm6, dword ptr [rip + 0x121c1e0] # 0x1815a7ea0
000000018038BCC0 movss     xmm7, dword ptr [rip + 0x121c04c] # 0x1815a7d14
000000018038BCC8 movd      xmm0, ebx
000000018038BCCC mov       qword ptr [rsp + 0x20], rdi
000000018038BCD1 cvtdq2ps  xmm0, xmm0
000000018038BCD4 mulss     xmm0, dword ptr [rip + 0x121bd6c] # 0x1815a7a48
000000018038BCDC subss     xmm6, xmm0
000000018038BCE0 test      rcx, rcx
000000018038BCE3 je        0x18038bda0
000000018038BCE9 mov       r8, qword ptr [rip + 0x18319e0] # 0x181bbd6d0
000000018038BCF0 mov       edx, ebx
000000018038BCF2 call      0x180825710
000000018038BCF7 test      rax, rax
000000018038BCFA je        0x18038bda0
000000018038BD00 xor       edx, edx
000000018038BD02 mov       rcx, rax
000000018038BD05 call      0x1813044d0
000000018038BD0A test      rax, rax
000000018038BD0D je        0x18038bda0
000000018038BD13 xor       edx, edx
000000018038BD15 mov       rcx, rax
000000018038BD18 call      0x18131c070
000000018038BD1D test      rax, rax
000000018038BD20 je        0x18038bda0
000000018038BD22 movaps    xmm0, xmm6
000000018038BD25 mov       dword ptr [rsp + 0x28], edi
000000018038BD29 unpcklps  xmm0, xmm7
000000018038BD2C lea       rdx, [rsp + 0x20]
000000018038BD31 xor       r8d, r8d
000000018038BD34 movsd     qword ptr [rsp + 0x20], xmm0
000000018038BD3A mov       rcx, rax
000000018038BD3D call      0x18132f910
000000018038BD42 mov       rcx, qword ptr [rsi + 0x168]
000000018038BD49 test      rcx, rcx
000000018038BD4C je        0x18038bda0
000000018038BD4E mov       r8, qword ptr [rip + 0x183197b] # 0x181bbd6d0
000000018038BD55 mov       edx, ebx
000000018038BD57 call      0x180825710
000000018038BD5C test      rax, rax
000000018038BD5F je        0x18038bda0
000000018038BD61 movaps    xmm7, xmmword ptr [rsp + 0x30]
000000018038BD66 movaps    xmm6, xmmword ptr [rsp + 0x40]
000000018038BD6B mov       r14, qword ptr [rsp + 0x58]
000000018038BD70 mov       rdi, qword ptr [rsp + 0x80]
000000018038BD78 mov       rbp, qword ptr [rsp + 0x78]
000000018038BD7D mov       rbx, qword ptr [rsp + 0x70]
000000018038BD82 mov       dword ptr [rax + 0x28], r15d
000000018038BD86 mov       r15, qword ptr [rsp + 0x50]
000000018038BD8B mov       dword ptr [rsi + 0x188], r12d
000000018038BD92 mov       r12, qword ptr [rsp + 0x88]
000000018038BD9A add       rsp, 0x60
000000018038BD9E pop       rsi
000000018038BD9F ret       
000000018038BDA0 call      0x180250150
000000018038BDA5 int3      
000000018038BDA6 call      0x180250140
000000018038BDAB int3      
000000018038BDAC int3      
000000018038BDAD int3      
000000018038BDAE int3      
000000018038BDAF int3      