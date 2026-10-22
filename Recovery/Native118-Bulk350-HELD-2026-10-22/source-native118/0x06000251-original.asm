000000018032E720 push      rbx
000000018032E722 sub       rsp, 0x20
000000018032E726 cmp       byte ptr [rip + 0x19b6516], 0 # 0x181ce4c43
000000018032E72D mov       rbx, rcx
000000018032E730 jne       0x18032e821
000000018032E736 lea       rcx, [rip + 0x188661b] # 0x181bb4d58
000000018032E73D call      0x18024fef0
000000018032E742 lea       rcx, [rip + 0x1885dd7] # 0x181bb4520
000000018032E749 call      0x18024fef0
000000018032E74E lea       rcx, [rip + 0x188639b] # 0x181bb4af0
000000018032E755 call      0x18024fef0
000000018032E75A lea       rcx, [rip + 0x188617f] # 0x181bb48e0
000000018032E761 call      0x18024fef0
000000018032E766 lea       rcx, [rip + 0x1886013] # 0x181bb4780
000000018032E76D call      0x18024fef0
000000018032E772 lea       rcx, [rip + 0x1885fb7] # 0x181bb4730
000000018032E779 call      0x18024fef0
000000018032E77E lea       rcx, [rip + 0x1886103] # 0x181bb4888
000000018032E785 call      0x18024fef0
000000018032E78A lea       rcx, [rip + 0x1886047] # 0x181bb47d8
000000018032E791 call      0x18024fef0
000000018032E796 lea       rcx, [rip + 0x1885ddb] # 0x181bb4578
000000018032E79D call      0x18024fef0
000000018032E7A2 lea       rcx, [rip + 0x18864a7] # 0x181bb4c50
000000018032E7A9 call      0x18024fef0
000000018032E7AE lea       rcx, [rip + 0x18865fb] # 0x181bb4db0
000000018032E7B5 call      0x18024fef0
000000018032E7BA lea       rcx, [rip + 0x1886177] # 0x181bb4938
000000018032E7C1 call      0x18024fef0
000000018032E7C6 lea       rcx, [rip + 0x1890dcb] # 0x181bbf598
000000018032E7CD call      0x18024fef0
000000018032E7D2 lea       rcx, [rip + 0x1890d67] # 0x181bbf540
000000018032E7D9 call      0x18024fef0
000000018032E7DE lea       rcx, [rip + 0x188383b] # 0x181bb2020
000000018032E7E5 call      0x18024fef0
000000018032E7EA lea       rcx, [rip + 0x1883887] # 0x181bb2078
000000018032E7F1 call      0x18024fef0
000000018032E7F6 lea       rcx, [rip + 0x18838d3] # 0x181bb20d0
000000018032E7FD call      0x18024fef0
000000018032E802 lea       rcx, [rip + 0x188391f] # 0x181bb2128
000000018032E809 call      0x18024fef0
000000018032E80E lea       rcx, [rip + 0x188396b] # 0x181bb2180
000000018032E815 call      0x18024fef0
000000018032E81A mov       byte ptr [rip + 0x19b6422], 1 # 0x181ce4c43
000000018032E821 mov       rax, qword ptr [rbx + 0x18]
000000018032E825 mov       qword ptr [rsp + 0x30], rbp
000000018032E82A mov       qword ptr [rsp + 0x38], rsi
000000018032E82F mov       qword ptr [rsp + 0x40], rdi
000000018032E834 mov       qword ptr [rsp + 0x48], r14
000000018032E839 test      rax, rax
000000018032E83C je        0x18032eeea
000000018032E842 cmp       dword ptr [rax + 0x18], 0x80
000000018032E849 jge       0x18032e8ed
000000018032E84F mov       rsi, qword ptr [rbx + 0x18]
000000018032E853 test      rsi, rsi
000000018032E856 je        0x18032eeea
000000018032E85C mov       rcx, qword ptr [rip + 0x18838c5] # 0x181bb2128
000000018032E863 mov       edi, dword ptr [rsi + 0x18]
000000018032E866 call      0x180250100
000000018032E86B xor       r8d, r8d
000000018032E86E mov       edx, edi
000000018032E870 mov       rcx, rax
000000018032E873 mov       rbp, rax
000000018032E876 call      0x180376280
000000018032E87B mov       rax, qword ptr [rip + 0x188626e] # 0x181bb4af0
000000018032E882 inc       dword ptr [rsi + 0x1c]
000000018032E885 mov       rcx, qword ptr [rsi + 0x10]
000000018032E889 movsxd    rdx, dword ptr [rsi + 0x18]
000000018032E88D test      rcx, rcx
000000018032E890 je        0x18032eeea
000000018032E896 cmp       edx, dword ptr [rcx + 0x18]
000000018032E899 jb        0x18032e8b7
000000018032E89B mov       rax, qword ptr [rax + 0x20]
000000018032E89F mov       rdx, rbp
000000018032E8A2 mov       rcx, rsi
000000018032E8A5 mov       r8, qword ptr [rax + 0xc0]
000000018032E8AC mov       r8, qword ptr [r8 + 0x70]
000000018032E8B0 call      0x180847ce0
000000018032E8B5 jmp       0x18032e8db
000000018032E8B7 lea       eax, [rdx + 1]
000000018032E8BA mov       dword ptr [rsi + 0x18], eax
000000018032E8BD cmp       edx, dword ptr [rcx + 0x18]
000000018032E8C0 jae       0x18032eef0
000000018032E8C6 mov       qword ptr [rcx + rdx*8 + 0x20], rbp
000000018032E8CB lea       rcx, [rcx + rdx*8]
000000018032E8CF add       rcx, 0x20
000000018032E8D3 mov       rdx, rbp
000000018032E8D6 call      0x18024f360
000000018032E8DB mov       rax, qword ptr [rbx + 0x18]
000000018032E8DF test      rax, rax
000000018032E8E2 je        0x18032eeea
000000018032E8E8 jmp       0x18032e842
000000018032E8ED mov       rax, qword ptr [rbx + 0x20]
000000018032E8F1 test      rax, rax
000000018032E8F4 je        0x18032eeea
000000018032E8FA nop       word ptr [rax + rax]
000000018032E900 cmp       dword ptr [rax + 0x18], 0x80
000000018032E907 jge       0x18032eb05
000000018032E90D mov       rsi, qword ptr [rbx + 0x20]
000000018032E911 test      rsi, rsi
000000018032E914 je        0x18032eeea
000000018032E91A mov       rcx, qword ptr [rip + 0x18836ff] # 0x181bb2020
000000018032E921 mov       ebp, dword ptr [rsi + 0x18]
000000018032E924 call      0x180250100
000000018032E929 cmp       byte ptr [rip + 0x19b6317], 0 # 0x181ce4c47
000000018032E930 mov       rdi, rax
000000018032E933 jne       0x18032e954
000000018032E935 lea       rcx, [rip + 0x189c6cc] # 0x181bcb008
000000018032E93C call      0x18024fef0
000000018032E941 lea       rcx, [rip + 0x189da70] # 0x181bcc3b8
000000018032E948 call      0x18024fef0
000000018032E94D mov       byte ptr [rip + 0x19b62f3], 1 # 0x181ce4c47
000000018032E954 mov       rcx, qword ptr [rip + 0x189c6ad] # 0x181bcb008
000000018032E95B mov       edx, 5
000000018032E960 call      0x18024f3e0
000000018032E965 lea       rcx, [rdi + 0x28]
000000018032E969 mov       qword ptr [rdi + 0x28], rax
000000018032E96D mov       rdx, rax
000000018032E970 call      0x18024f360
000000018032E975 mov       rcx, qword ptr [rip + 0x189da3c] # 0x181bcc3b8
000000018032E97C mov       edx, 5
000000018032E981 call      0x18024f3e0
000000018032E986 lea       rcx, [rdi + 0x30]
000000018032E98A mov       qword ptr [rdi + 0x30], rax
000000018032E98E mov       rdx, rax
000000018032E991 call      0x18024f360
000000018032E996 mov       rcx, qword ptr [rip + 0x189da1b] # 0x181bcc3b8
000000018032E99D mov       edx, 0xc8
000000018032E9A2 call      0x18024f3e0
000000018032E9A7 lea       rcx, [rdi + 0x38]
000000018032E9AB mov       qword ptr [rdi + 0x38], rax
000000018032E9AF mov       rdx, rax
000000018032E9B2 call      0x18024f360
000000018032E9B7 mov       rcx, qword ptr [rip + 0x189d9fa] # 0x181bcc3b8
000000018032E9BE mov       edx, 0xc8
000000018032E9C3 call      0x18024f3e0
000000018032E9C8 lea       rcx, [rdi + 0x48]
000000018032E9CC mov       qword ptr [rdi + 0x48], rax
000000018032E9D0 mov       rdx, rax
000000018032E9D3 call      0x18024f360
000000018032E9D8 mov       rcx, qword ptr [rip + 0x189d9d9] # 0x181bcc3b8
000000018032E9DF mov       edx, 5
000000018032E9E4 call      0x18024f3e0
000000018032E9E9 lea       rcx, [rdi + 0x58]
000000018032E9ED mov       qword ptr [rdi + 0x58], rax
000000018032E9F1 mov       rdx, rax
000000018032E9F4 call      0x18024f360
000000018032E9F9 mov       rcx, qword ptr [rip + 0x189d9b8] # 0x181bcc3b8
000000018032EA00 mov       edx, 0xc8
000000018032EA05 call      0x18024f3e0
000000018032EA0A lea       rcx, [rdi + 0x60]
000000018032EA0E mov       qword ptr [rdi + 0x60], rax
000000018032EA12 mov       rdx, rax
000000018032EA15 call      0x18024f360
000000018032EA1A mov       rcx, qword ptr [rip + 0x189d997] # 0x181bcc3b8
000000018032EA21 mov       edx, 5
000000018032EA26 call      0x18024f3e0
000000018032EA2B lea       rcx, [rdi + 0x70]
000000018032EA2F mov       qword ptr [rdi + 0x70], rax
000000018032EA33 mov       rdx, rax
000000018032EA36 call      0x18024f360
000000018032EA3B mov       rcx, qword ptr [rip + 0x189d976] # 0x181bcc3b8
000000018032EA42 mov       edx, 5
000000018032EA47 call      0x18024f3e0
000000018032EA4C lea       rcx, [rdi + 0x78]
000000018032EA50 mov       qword ptr [rdi + 0x78], rax
000000018032EA54 mov       rdx, rax
000000018032EA57 call      0x18024f360
000000018032EA5C mov       rcx, qword ptr [rip + 0x189d955] # 0x181bcc3b8
000000018032EA63 mov       edx, 5
000000018032EA68 call      0x18024f3e0
000000018032EA6D lea       rcx, [rdi + 0x80]
000000018032EA74 mov       qword ptr [rdi + 0x80], rax
000000018032EA7B mov       rdx, rax
000000018032EA7E call      0x18024f360
000000018032EA83 xor       edx, edx
000000018032EA85 mov       rcx, rdi
000000018032EA88 call      0x180302170
000000018032EA8D mov       dword ptr [rdi + 0x20], ebp
000000018032EA90 mov       rcx, qword ptr [rip + 0x1885a89] # 0x181bb4520
000000018032EA97 inc       dword ptr [rsi + 0x1c]
000000018032EA9A mov       rdx, qword ptr [rsi + 0x10]
000000018032EA9E movsxd    r8, dword ptr [rsi + 0x18]
000000018032EAA2 test      rdx, rdx
000000018032EAA5 je        0x18032eeea
000000018032EAAB cmp       r8d, dword ptr [rdx + 0x18]
000000018032EAAF jb        0x18032eacd
000000018032EAB1 mov       rax, qword ptr [rcx + 0x20]
000000018032EAB5 mov       rdx, rdi
000000018032EAB8 mov       rcx, rsi
000000018032EABB mov       r8, qword ptr [rax + 0xc0]
000000018032EAC2 mov       r8, qword ptr [r8 + 0x70]
000000018032EAC6 call      0x180847ce0
000000018032EACB jmp       0x18032eaf3
000000018032EACD lea       eax, [r8 + 1]
000000018032EAD1 mov       dword ptr [rsi + 0x18], eax
000000018032EAD4 cmp       r8d, dword ptr [rdx + 0x18]
000000018032EAD8 jae       0x18032eef0
000000018032EADE mov       qword ptr [rdx + r8*8 + 0x20], rdi
000000018032EAE3 add       rdx, 0x20
000000018032EAE7 lea       rcx, [rdx + r8*8]
000000018032EAEB mov       rdx, rdi
000000018032EAEE call      0x18024f360
000000018032EAF3 mov       rax, qword ptr [rbx + 0x20]
000000018032EAF7 test      rax, rax
000000018032EAFA jne       0x18032e900
000000018032EB00 jmp       0x18032eeea
000000018032EB05 cmp       qword ptr [rbx + 0x28], 0
000000018032EB0A jne       0x18032eb3a
000000018032EB0C mov       rcx, qword ptr [rip + 0x1890a2d] # 0x181bbf540
000000018032EB13 call      0x180250100
000000018032EB18 mov       rdx, qword ptr [rip + 0x1885c11] # 0x181bb4730
000000018032EB1F mov       rcx, rax
000000018032EB22 mov       rdi, rax
000000018032EB25 call      0x1808053d0
000000018032EB2A lea       rcx, [rbx + 0x28]
000000018032EB2E mov       qword ptr [rbx + 0x28], rdi
000000018032EB32 mov       rdx, rdi
000000018032EB35 call      0x18024f360
000000018032EB3A mov       rax, qword ptr [rbx + 0x28]
000000018032EB3E test      rax, rax
000000018032EB41 je        0x18032eeea
000000018032EB47 nop       word ptr [rax + rax]
000000018032EB50 cmp       dword ptr [rax + 0x18], 0xc3
000000018032EB57 jge       0x18032ec0b
000000018032EB5D mov       rdi, qword ptr [rbx + 0x28]
000000018032EB61 test      rdi, rdi
000000018032EB64 je        0x18032eeea
000000018032EB6A mov       rcx, qword ptr [rip + 0x1883507] # 0x181bb2078
000000018032EB71 mov       edi, dword ptr [rdi + 0x18]
000000018032EB74 call      0x180250100
000000018032EB79 lea       edx, [rdi + 1]
000000018032EB7C xor       r8d, r8d
000000018032EB7F mov       rcx, rax
000000018032EB82 mov       rsi, rax
000000018032EB85 call      0x18032de10
000000018032EB8A mov       r9, qword ptr [rbx + 0x28]
000000018032EB8E test      r9, r9
000000018032EB91 je        0x18032eeea
000000018032EB97 mov       rax, qword ptr [rip + 0x1885be2] # 0x181bb4780
000000018032EB9E inc       dword ptr [r9 + 0x1c]
000000018032EBA2 mov       rcx, qword ptr [r9 + 0x10]
000000018032EBA6 movsxd    rdx, dword ptr [r9 + 0x18]
000000018032EBAA test      rcx, rcx
000000018032EBAD je        0x18032eeea
000000018032EBB3 cmp       edx, dword ptr [rcx + 0x18]
000000018032EBB6 jb        0x18032ebd4
000000018032EBB8 mov       rax, qword ptr [rax + 0x20]
000000018032EBBC mov       rdx, rsi
000000018032EBBF mov       rcx, r9
000000018032EBC2 mov       r8, qword ptr [rax + 0xc0]
000000018032EBC9 mov       r8, qword ptr [r8 + 0x70]
000000018032EBCD call      0x180847ce0
000000018032EBD2 jmp       0x18032ebf9
000000018032EBD4 lea       eax, [rdx + 1]
000000018032EBD7 mov       dword ptr [r9 + 0x18], eax
000000018032EBDB cmp       edx, dword ptr [rcx + 0x18]
000000018032EBDE jae       0x18032eef0
000000018032EBE4 mov       qword ptr [rcx + rdx*8 + 0x20], rsi
000000018032EBE9 lea       rcx, [rcx + rdx*8]
000000018032EBED add       rcx, 0x20
000000018032EBF1 mov       rdx, rsi
000000018032EBF4 call      0x18024f360
000000018032EBF9 mov       rax, qword ptr [rbx + 0x28]
000000018032EBFD test      rax, rax
000000018032EC00 jne       0x18032eb50
000000018032EC06 jmp       0x18032eeea
000000018032EC0B cmp       qword ptr [rbx + 0x30], 0
000000018032EC10 jne       0x18032ec40
000000018032EC12 mov       rcx, qword ptr [rip + 0x189097f] # 0x181bbf598
000000018032EC19 call      0x180250100
000000018032EC1E mov       rdx, qword ptr [rip + 0x1885c63] # 0x181bb4888
000000018032EC25 mov       rcx, rax
000000018032EC28 mov       rdi, rax
000000018032EC2B call      0x1808053d0
000000018032EC30 lea       rcx, [rbx + 0x30]
000000018032EC34 mov       qword ptr [rbx + 0x30], rdi
000000018032EC38 mov       rdx, rdi
000000018032EC3B call      0x18024f360
000000018032EC40 mov       rax, qword ptr [rbx + 0x30]
000000018032EC44 test      rax, rax
000000018032EC47 je        0x18032eeea
000000018032EC4D nop       dword ptr [rax]
000000018032EC50 cmp       dword ptr [rax + 0x18], 0x80
000000018032EC57 jge       0x18032ede0
000000018032EC5D mov       rax, qword ptr [rbx + 0x30]
000000018032EC61 test      rax, rax
000000018032EC64 je        0x18032eeea
000000018032EC6A mov       rcx, qword ptr [rip + 0x188345f] # 0x181bb20d0
000000018032EC71 mov       rbp, rax
000000018032EC74 mov       r14d, dword ptr [rax + 0x18]
000000018032EC78 call      0x180250100
000000018032EC7D cmp       byte ptr [rip + 0x19b5fc5], 0 # 0x181ce4c49
000000018032EC84 mov       rsi, rax
000000018032EC87 jne       0x18032eca8
000000018032EC89 lea       rcx, [rip + 0x188e468] # 0x181bbd0f8
000000018032EC90 call      0x18024fef0
000000018032EC95 lea       rcx, [rip + 0x18921bc] # 0x181bc0e58
000000018032EC9C call      0x18024fef0
000000018032ECA1 mov       byte ptr [rip + 0x19b5fa1], 1 # 0x181ce4c49
000000018032ECA8 mov       rcx, qword ptr [rip + 0x18921a9] # 0x181bc0e58
000000018032ECAF call      0x180250100
000000018032ECB4 mov       rdx, qword ptr [rip + 0x188e43d] # 0x181bbd0f8
000000018032ECBB mov       rcx, rax
000000018032ECBE mov       rdi, rax
000000018032ECC1 call      0x1808053d0
000000018032ECC6 lea       rcx, [rsi + 0x18]
000000018032ECCA mov       qword ptr [rsi + 0x18], rdi
000000018032ECCE mov       rdx, rdi
000000018032ECD1 call      0x18024f360
000000018032ECD6 mov       rcx, qword ptr [rip + 0x189217b] # 0x181bc0e58
000000018032ECDD call      0x180250100
000000018032ECE2 mov       rdx, qword ptr [rip + 0x188e40f] # 0x181bbd0f8
000000018032ECE9 mov       rcx, rax
000000018032ECEC mov       rdi, rax
000000018032ECEF call      0x1808053d0
000000018032ECF4 lea       rcx, [rsi + 0x20]
000000018032ECF8 mov       qword ptr [rsi + 0x20], rdi
000000018032ECFC mov       rdx, rdi
000000018032ECFF call      0x18024f360
000000018032ED04 mov       rcx, qword ptr [rip + 0x189214d] # 0x181bc0e58
000000018032ED0B call      0x180250100
000000018032ED10 mov       rdx, qword ptr [rip + 0x188e3e1] # 0x181bbd0f8
000000018032ED17 mov       rcx, rax
000000018032ED1A mov       rdi, rax
000000018032ED1D call      0x1808053d0
000000018032ED22 lea       rcx, [rsi + 0x28]
000000018032ED26 mov       qword ptr [rsi + 0x28], rdi
000000018032ED2A mov       rdx, rdi
000000018032ED2D call      0x18024f360
000000018032ED32 mov       rcx, qword ptr [rip + 0x189211f] # 0x181bc0e58
000000018032ED39 call      0x180250100
000000018032ED3E mov       rdx, qword ptr [rip + 0x188e3b3] # 0x181bbd0f8
000000018032ED45 mov       rcx, rax
000000018032ED48 mov       rdi, rax
000000018032ED4B call      0x1808053d0
000000018032ED50 lea       rcx, [rsi + 0x30]
000000018032ED54 mov       qword ptr [rsi + 0x30], rdi
000000018032ED58 mov       rdx, rdi
000000018032ED5B call      0x18024f360
000000018032ED60 xor       edx, edx
000000018032ED62 mov       rcx, rsi
000000018032ED65 call      0x180302170
000000018032ED6A mov       dword ptr [rsi + 0x10], r14d
000000018032ED6E mov       r8, qword ptr [rip + 0x1885b6b] # 0x181bb48e0
000000018032ED75 inc       dword ptr [rbp + 0x1c]
000000018032ED78 mov       rdx, qword ptr [rbp + 0x10]
000000018032ED7C movsxd    rcx, dword ptr [rbp + 0x18]
000000018032ED80 test      rdx, rdx
000000018032ED83 je        0x18032eeea
000000018032ED89 cmp       ecx, dword ptr [rdx + 0x18]
000000018032ED8C jb        0x18032edaa
000000018032ED8E mov       rax, qword ptr [r8 + 0x20]
000000018032ED92 mov       rdx, rsi
000000018032ED95 mov       rcx, rbp
000000018032ED98 mov       r8, qword ptr [rax + 0xc0]
000000018032ED9F mov       r8, qword ptr [r8 + 0x70]
000000018032EDA3 call      0x180847ce0
000000018032EDA8 jmp       0x18032edce
000000018032EDAA lea       eax, [rcx + 1]
000000018032EDAD mov       dword ptr [rbp + 0x18], eax
000000018032EDB0 cmp       ecx, dword ptr [rdx + 0x18]
000000018032EDB3 jae       0x18032eef0
000000018032EDB9 mov       qword ptr [rdx + rcx*8 + 0x20], rsi
000000018032EDBE add       rdx, 0x20
000000018032EDC2 lea       rcx, [rdx + rcx*8]
000000018032EDC6 mov       rdx, rsi
000000018032EDC9 call      0x18024f360
000000018032EDCE mov       rax, qword ptr [rbx + 0x30]
000000018032EDD2 test      rax, rax
000000018032EDD5 jne       0x18032ec50
000000018032EDDB jmp       0x18032eeea
000000018032EDE0 mov       rax, qword ptr [rbx + 0x38]
000000018032EDE4 test      rax, rax
000000018032EDE7 je        0x18032eeea
000000018032EDED nop       dword ptr [rax]
000000018032EDF0 cmp       dword ptr [rax + 0x18], 0x80
000000018032EDF7 jge       0x18032eed0
000000018032EDFD mov       rax, qword ptr [rbx + 0x38]
000000018032EE01 test      rax, rax
000000018032EE04 je        0x18032eeea
000000018032EE0A mov       rcx, qword ptr [rip + 0x188336f] # 0x181bb2180
000000018032EE11 mov       rsi, rax
000000018032EE14 mov       ebp, dword ptr [rax + 0x18]
000000018032EE17 call      0x180250100
000000018032EE1C cmp       byte ptr [rip + 0x19b5e27], 0 # 0x181ce4c4a
000000018032EE23 mov       rdi, rax
000000018032EE26 jne       0x18032ee3b
000000018032EE28 lea       rcx, [rip + 0x189ee29] # 0x181bcdc58
000000018032EE2F call      0x18024fef0
000000018032EE34 mov       byte ptr [rip + 0x19b5e0f], 1 # 0x181ce4c4a
000000018032EE3B mov       rcx, qword ptr [rip + 0x189ee16] # 0x181bcdc58
000000018032EE42 mov       edx, 8
000000018032EE47 call      0x18024f3e0
000000018032EE4C lea       rcx, [rdi + 0x18]
000000018032EE50 mov       qword ptr [rdi + 0x18], rax
000000018032EE54 mov       rdx, rax
000000018032EE57 call      0x18024f360
000000018032EE5C xor       edx, edx
000000018032EE5E mov       rcx, rdi
000000018032EE61 call      0x180302170
000000018032EE66 mov       dword ptr [rdi + 0x10], ebp
000000018032EE69 mov       r8, qword ptr [rip + 0x1885ee8] # 0x181bb4d58
000000018032EE70 inc       dword ptr [rsi + 0x1c]
000000018032EE73 mov       rcx, qword ptr [rsi + 0x10]
000000018032EE77 movsxd    rdx, dword ptr [rsi + 0x18]
000000018032EE7B test      rcx, rcx
000000018032EE7E je        0x18032eeea
000000018032EE80 cmp       edx, dword ptr [rcx + 0x18]
000000018032EE83 jb        0x18032eea1
000000018032EE85 mov       rax, qword ptr [r8 + 0x20]
000000018032EE89 mov       rdx, rdi
000000018032EE8C mov       rcx, rsi
000000018032EE8F mov       r8, qword ptr [rax + 0xc0]
000000018032EE96 mov       r8, qword ptr [r8 + 0x70]
000000018032EE9A call      0x180847ce0
000000018032EE9F jmp       0x18032eec1
000000018032EEA1 lea       eax, [rdx + 1]
000000018032EEA4 mov       dword ptr [rsi + 0x18], eax
000000018032EEA7 cmp       edx, dword ptr [rcx + 0x18]
000000018032EEAA jae       0x18032eef0
000000018032EEAC mov       qword ptr [rcx + rdx*8 + 0x20], rdi
000000018032EEB1 lea       rcx, [rcx + rdx*8]
000000018032EEB5 add       rcx, 0x20
000000018032EEB9 mov       rdx, rdi
000000018032EEBC call      0x18024f360
000000018032EEC1 mov       rax, qword ptr [rbx + 0x38]
000000018032EEC5 test      rax, rax
000000018032EEC8 jne       0x18032edf0
000000018032EECE jmp       0x18032eeea
000000018032EED0 mov       r14, qword ptr [rsp + 0x48]
000000018032EED5 mov       rdi, qword ptr [rsp + 0x40]
000000018032EEDA mov       rsi, qword ptr [rsp + 0x38]
000000018032EEDF mov       rbp, qword ptr [rsp + 0x30]
000000018032EEE4 add       rsp, 0x20
000000018032EEE8 pop       rbx
000000018032EEE9 ret       
000000018032EEEA call      0x180250150
000000018032EEEF int3      
000000018032EEF0 call      0x180250140
000000018032EEF5 int3      
000000018032EEF6 int3      
000000018032EEF7 int3      
000000018032EEF8 int3      
000000018032EEF9 int3      
000000018032EEFA int3      
000000018032EEFB int3      
000000018032EEFC int3      
000000018032EEFD int3      
000000018032EEFE int3      
000000018032EEFF int3      