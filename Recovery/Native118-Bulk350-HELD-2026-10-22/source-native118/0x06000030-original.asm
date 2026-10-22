0000000180302A40 push      rbx
0000000180302A42 push      rdi
0000000180302A43 sub       rsp, 0x28
0000000180302A47 cmp       byte ptr [rip + 0x19e206d], 0 # 0x181ce4abb
0000000180302A4E mov       rdi, rdx
0000000180302A51 mov       rbx, rcx
0000000180302A54 jne       0x180302a75
0000000180302A56 lea       rcx, [rip + 0x18adaa3] # 0x181bb0500
0000000180302A5D call      0x18024fef0
0000000180302A62 lea       rcx, [rip + 0x18b828f] # 0x181bbacf8
0000000180302A69 call      0x18024fef0
0000000180302A6E mov       byte ptr [rip + 0x19e2046], 1 # 0x181ce4abb
0000000180302A75 lea       rcx, [rbx + 0x28]
0000000180302A79 mov       qword ptr [rbx + 0x28], rdi
0000000180302A7D mov       rdx, rdi
0000000180302A80 call      0x18024f360
0000000180302A85 mov       rcx, qword ptr [rip + 0x18ada74] # 0x181bb0500
0000000180302A8C mov       rdi, qword ptr [rbx + 0x28]
0000000180302A90 cmp       dword ptr [rcx + 0xe0], 0
0000000180302A97 jne       0x180302a9e
0000000180302A99 call      0x1802501e0
0000000180302A9E mov       qword ptr [rsp + 0x48], rbp
0000000180302AA3 xor       r8d, r8d
0000000180302AA6 mov       qword ptr [rsp + 0x50], rsi
0000000180302AAB xor       edx, edx
0000000180302AAD mov       rcx, rdi
0000000180302AB0 mov       qword ptr [rsp + 0x20], r14
0000000180302AB5 call      0x18131f760
0000000180302ABA test      al, al
0000000180302ABC jne       0x180302fb9
0000000180302AC2 mov       rdi, qword ptr [rbx + 0x28]
0000000180302AC6 test      rdi, rdi
0000000180302AC9 je        0x18030327e
0000000180302ACF mov       rdi, qword ptr [rdi + 0x30]
0000000180302AD3 test      rdi, rdi
0000000180302AD6 je        0x18030327e
0000000180302ADC mov       rsi, qword ptr [rdi + 0x20]
0000000180302AE0 test      rsi, rsi
0000000180302AE3 je        0x18030327e
0000000180302AE9 mov       rbp, qword ptr [rsi + 0x28]
0000000180302AED lea       rcx, [rsp + 0x40]
0000000180302AF2 mov       eax, dword ptr [rsi + 0x1c]
0000000180302AF5 xor       edx, edx
0000000180302AF7 mov       r14, qword ptr [rbx + 0x40]
0000000180302AFB mov       dword ptr [rsp + 0x40], eax
0000000180302AFF call      0x180ca3820
0000000180302B04 test      r14, r14
0000000180302B07 je        0x18030327e
0000000180302B0D xor       r8d, r8d
0000000180302B10 mov       rdx, rax
0000000180302B13 mov       rcx, r14
0000000180302B16 call      0x181197d70
0000000180302B1B mov       r14, qword ptr [rbx + 0x30]
0000000180302B1F lea       rcx, [rdi + 0x10]
0000000180302B23 xor       edx, edx
0000000180302B25 call      0x180ca3820
0000000180302B2A test      r14, r14
0000000180302B2D je        0x18030327e
0000000180302B33 xor       r8d, r8d
0000000180302B36 mov       rdx, rax
0000000180302B39 mov       rcx, r14
0000000180302B3C call      0x181197d70
0000000180302B41 mov       r14, qword ptr [rbx + 0x38]
0000000180302B45 lea       rcx, [rdi + 0x14]
0000000180302B49 xor       edx, edx
0000000180302B4B call      0x180ca3820
0000000180302B50 test      r14, r14
0000000180302B53 je        0x18030327e
0000000180302B59 xor       r8d, r8d
0000000180302B5C mov       rdx, rax
0000000180302B5F mov       rcx, r14
0000000180302B62 call      0x181197d70
0000000180302B67 mov       rcx, qword ptr [rbx + 0x68]
0000000180302B6B test      rcx, rcx
0000000180302B6E je        0x18030327e
0000000180302B74 mov       rdx, qword ptr [rdi + 0x18]
0000000180302B78 xor       r8d, r8d
0000000180302B7B call      0x181197d70
0000000180302B80 mov       rcx, qword ptr [rbx + 0x80]
0000000180302B87 test      rcx, rcx
0000000180302B8A je        0x18030327e
0000000180302B90 mov       edx, dword ptr [rsi + 0x18]
0000000180302B93 xor       r8d, r8d
0000000180302B96 call      0x181180950
0000000180302B9B mov       rcx, qword ptr [rbx + 0x40]
0000000180302B9F test      rcx, rcx
0000000180302BA2 je        0x18030327e
0000000180302BA8 xor       r8d, r8d
0000000180302BAB mov       dl, 1
0000000180302BAD call      0x18156cc70
0000000180302BB2 mov       rcx, qword ptr [rbx + 0x30]
0000000180302BB6 test      rcx, rcx
0000000180302BB9 je        0x18030327e
0000000180302BBF xor       r8d, r8d
0000000180302BC2 mov       dl, 1
0000000180302BC4 call      0x18156cc70
0000000180302BC9 mov       rcx, qword ptr [rbx + 0x38]
0000000180302BCD test      rcx, rcx
0000000180302BD0 je        0x18030327e
0000000180302BD6 xor       r8d, r8d
0000000180302BD9 mov       dl, 1
0000000180302BDB call      0x18156cc70
0000000180302BE0 mov       rcx, qword ptr [rbx + 0x68]
0000000180302BE4 test      rcx, rcx
0000000180302BE7 je        0x18030327e
0000000180302BED xor       r8d, r8d
0000000180302BF0 mov       dl, 1
0000000180302BF2 call      0x18156cc70
0000000180302BF7 mov       rcx, qword ptr [rbx + 0x80]
0000000180302BFE test      rcx, rcx
0000000180302C01 je        0x18030327e
0000000180302C07 xor       r8d, r8d
0000000180302C0A mov       dl, 1
0000000180302C0C call      0x18156cc70
0000000180302C11 mov       rcx, qword ptr [rbx + 0x80]
0000000180302C18 test      rcx, rcx
0000000180302C1B je        0x18030327e
0000000180302C21 xor       r8d, r8d
0000000180302C24 mov       dl, 1
0000000180302C26 call      0x18156cc70
0000000180302C2B cmp       dword ptr [rsp + 0x40], 8
0000000180302C30 mov       rcx, qword ptr [rbx + 0x48]
0000000180302C34 je        0x180302ca6
0000000180302C36 test      rcx, rcx
0000000180302C39 je        0x18030327e
0000000180302C3F xor       r8d, r8d
0000000180302C42 xor       edx, edx
0000000180302C44 call      0x18156cc70
0000000180302C49 mov       rcx, qword ptr [rbx + 0x50]
0000000180302C4D test      rcx, rcx
0000000180302C50 je        0x18030327e
0000000180302C56 xor       r8d, r8d
0000000180302C59 xor       edx, edx
0000000180302C5B call      0x18156cc70
0000000180302C60 mov       rcx, qword ptr [rbx + 0x48]
0000000180302C64 test      rcx, rcx
0000000180302C67 je        0x18030327e
0000000180302C6D mov       rax, qword ptr [rip + 0x18b8084] # 0x181bbacf8
0000000180302C74 xor       r8d, r8d
0000000180302C77 mov       rdx, qword ptr [rax + 0xb8]
0000000180302C7E mov       rdx, qword ptr [rdx]
0000000180302C81 call      0x181197d70
0000000180302C86 mov       rdi, qword ptr [rbx + 0x50]
0000000180302C8A test      rdi, rdi
0000000180302C8D je        0x18030327e
0000000180302C93 mov       rax, qword ptr [rip + 0x18b805e] # 0x181bbacf8
0000000180302C9A mov       rcx, qword ptr [rax + 0xb8]
0000000180302CA1 mov       rax, qword ptr [rcx]
0000000180302CA4 jmp       0x180302d0e
0000000180302CA6 test      rcx, rcx
0000000180302CA9 je        0x18030327e
0000000180302CAF xor       r8d, r8d
0000000180302CB2 mov       dl, 1
0000000180302CB4 call      0x18156cc70
0000000180302CB9 mov       rcx, qword ptr [rbx + 0x50]
0000000180302CBD test      rcx, rcx
0000000180302CC0 je        0x18030327e
0000000180302CC6 xor       r8d, r8d
0000000180302CC9 mov       dl, 1
0000000180302CCB call      0x18156cc70
0000000180302CD0 mov       rdi, qword ptr [rbx + 0x48]
0000000180302CD4 lea       rcx, [rsi + 0x20]
0000000180302CD8 xor       edx, edx
0000000180302CDA call      0x180ca3820
0000000180302CDF test      rdi, rdi
0000000180302CE2 je        0x18030327e
0000000180302CE8 xor       r8d, r8d
0000000180302CEB mov       rdx, rax
0000000180302CEE mov       rcx, rdi
0000000180302CF1 call      0x181197d70
0000000180302CF6 mov       rdi, qword ptr [rbx + 0x50]
0000000180302CFA lea       rcx, [rsi + 0x24]
0000000180302CFE xor       edx, edx
0000000180302D00 call      0x180ca3820
0000000180302D05 test      rdi, rdi
0000000180302D08 je        0x18030327e
0000000180302D0E xor       r8d, r8d
0000000180302D11 mov       rdx, rax
0000000180302D14 mov       rcx, rdi
0000000180302D17 call      0x181197d70
0000000180302D1C cmp       dword ptr [rsp + 0x40], 3
0000000180302D21 mov       rcx, qword ptr [rbx + 0x70]
0000000180302D25 je        0x180302e2e
0000000180302D2B test      rcx, rcx
0000000180302D2E je        0x18030327e
0000000180302D34 xor       r8d, r8d
0000000180302D37 xor       edx, edx
0000000180302D39 call      0x18156cc70
0000000180302D3E mov       rcx, qword ptr [rbx + 0x70]
0000000180302D42 test      rcx, rcx
0000000180302D45 je        0x18030327e
0000000180302D4B xor       r8d, r8d
0000000180302D4E xor       edx, edx
0000000180302D50 call      0x181573290
0000000180302D55 mov       rcx, qword ptr [rbx + 0x78]
0000000180302D59 test      rcx, rcx
0000000180302D5C je        0x18030327e
0000000180302D62 xor       r8d, r8d
0000000180302D65 xor       edx, edx
0000000180302D67 call      0x18156cc70
0000000180302D6C mov       rcx, qword ptr [rbx + 0x78]
0000000180302D70 test      rcx, rcx
0000000180302D73 je        0x18030327e
0000000180302D79 xor       r8d, r8d
0000000180302D7C xor       edx, edx
0000000180302D7E call      0x181180950
0000000180302D83 mov       rcx, qword ptr [rbx + 0x58]
0000000180302D87 test      rcx, rcx
0000000180302D8A je        0x18030327e
0000000180302D90 xor       r8d, r8d
0000000180302D93 xor       edx, edx
0000000180302D95 call      0x18156cc70
0000000180302D9A mov       rcx, qword ptr [rbx + 0x58]
0000000180302D9E test      rcx, rcx
0000000180302DA1 je        0x18030327e
0000000180302DA7 mov       rax, qword ptr [rip + 0x18b7f4a] # 0x181bbacf8
0000000180302DAE xor       r8d, r8d
0000000180302DB1 mov       rdx, qword ptr [rax + 0xb8]
0000000180302DB8 mov       rdx, qword ptr [rdx]
0000000180302DBB call      0x181197d70
0000000180302DC0 mov       rcx, qword ptr [rbx + 0x60]
0000000180302DC4 test      rcx, rcx
0000000180302DC7 je        0x18030327e
0000000180302DCD xor       r8d, r8d
0000000180302DD0 xor       edx, edx
0000000180302DD2 call      0x18156cc70
0000000180302DD7 mov       rcx, qword ptr [rbx + 0x60]
0000000180302DDB test      rcx, rcx
0000000180302DDE je        0x18030327e
0000000180302DE4 mov       rax, qword ptr [rip + 0x18b7f0d] # 0x181bbacf8
0000000180302DEB xor       r8d, r8d
0000000180302DEE mov       rdx, qword ptr [rax + 0xb8]
0000000180302DF5 mov       rdx, qword ptr [rdx]
0000000180302DF8 call      0x181197d70
0000000180302DFD mov       rcx, qword ptr [rbx + 0x88]
0000000180302E04 test      rcx, rcx
0000000180302E07 je        0x18030327e
0000000180302E0D xor       r8d, r8d
0000000180302E10 xor       edx, edx
0000000180302E12 call      0x18156cc70
0000000180302E17 mov       rcx, qword ptr [rbx + 0x88]
0000000180302E1E test      rcx, rcx
0000000180302E21 je        0x18030327e
0000000180302E27 xor       edx, edx
0000000180302E29 jmp       0x180302f67
0000000180302E2E test      rcx, rcx
0000000180302E31 je        0x18030327e
0000000180302E37 xor       r8d, r8d
0000000180302E3A mov       dl, 1
0000000180302E3C call      0x18156cc70
0000000180302E41 mov       rcx, qword ptr [rbx + 0x70]
0000000180302E45 test      rcx, rcx
0000000180302E48 je        0x18030327e
0000000180302E4E movss     xmm1, dword ptr [rsi + 0x30]
0000000180302E53 xorps     xmm0, xmm0
0000000180302E56 comiss    xmm1, xmm0
0000000180302E59 seta      dl
0000000180302E5C xor       r8d, r8d
0000000180302E5F call      0x181573290
0000000180302E64 mov       rcx, qword ptr [rbx + 0x78]
0000000180302E68 test      rcx, rcx
0000000180302E6B je        0x18030327e
0000000180302E71 xor       r8d, r8d
0000000180302E74 mov       dl, 1
0000000180302E76 call      0x18156cc70
0000000180302E7B test      rbp, rbp
0000000180302E7E je        0x18030327e
0000000180302E84 mov       rcx, qword ptr [rbx + 0x78]
0000000180302E88 test      rcx, rcx
0000000180302E8B je        0x18030327e
0000000180302E91 mov       edx, dword ptr [rbp + 0x10]
0000000180302E94 xor       r8d, r8d
0000000180302E97 call      0x181180950
0000000180302E9C mov       rcx, qword ptr [rbx + 0x58]
0000000180302EA0 test      rcx, rcx
0000000180302EA3 je        0x18030327e
0000000180302EA9 xor       r8d, r8d
0000000180302EAC mov       dl, 1
0000000180302EAE call      0x18156cc70
0000000180302EB3 mov       rdi, qword ptr [rbx + 0x58]
0000000180302EB7 lea       rcx, [rbp + 0x14]
0000000180302EBB xor       edx, edx
0000000180302EBD call      0x180ca3820
0000000180302EC2 test      rdi, rdi
0000000180302EC5 je        0x18030327e
0000000180302ECB xor       r8d, r8d
0000000180302ECE mov       rdx, rax
0000000180302ED1 mov       rcx, rdi
0000000180302ED4 call      0x181197d70
0000000180302ED9 cmp       dword ptr [rbp + 0x10], 8
0000000180302EDD mov       rcx, qword ptr [rbx + 0x60]
0000000180302EE1 jne       0x180302dc4
0000000180302EE7 test      rcx, rcx
0000000180302EEA je        0x18030327e
0000000180302EF0 xor       r8d, r8d
0000000180302EF3 mov       dl, 1
0000000180302EF5 call      0x18156cc70
0000000180302EFA mov       rcx, qword ptr [rbp + 0x18]
0000000180302EFE mov       rdi, qword ptr [rbx + 0x60]
0000000180302F02 test      rcx, rcx
0000000180302F05 je        0x18030327e
0000000180302F0B add       rcx, 0x1c
0000000180302F0F xor       edx, edx
0000000180302F11 call      0x180ca3820
0000000180302F16 test      rdi, rdi
0000000180302F19 je        0x18030327e
0000000180302F1F xor       r8d, r8d
0000000180302F22 mov       rdx, rax
0000000180302F25 mov       rcx, rdi
0000000180302F28 call      0x181197d70
0000000180302F2D mov       rcx, qword ptr [rbx + 0x88]
0000000180302F34 test      rcx, rcx
0000000180302F37 je        0x18030327e
0000000180302F3D xor       r8d, r8d
0000000180302F40 mov       dl, 1
0000000180302F42 call      0x18156cc70
0000000180302F47 mov       rdx, qword ptr [rbp + 0x18]
0000000180302F4B mov       rcx, qword ptr [rbx + 0x88]
0000000180302F52 test      rdx, rdx
0000000180302F55 je        0x18030327e
0000000180302F5B test      rcx, rcx
0000000180302F5E je        0x18030327e
0000000180302F64 mov       edx, dword ptr [rdx + 0x18]
0000000180302F67 xor       r8d, r8d
0000000180302F6A call      0x181180950
0000000180302F6F mov       rcx, qword ptr [rbx + 0x90]
0000000180302F76 test      rcx, rcx
0000000180302F79 je        0x18030327e
0000000180302F7F xor       r8d, r8d
0000000180302F82 mov       dl, 1
0000000180302F84 call      0x18156cc70
0000000180302F89 mov       rcx, qword ptr [rbx + 0x98]
0000000180302F90 test      rcx, rcx
0000000180302F93 je        0x18030327e
0000000180302F99 xor       r8d, r8d
0000000180302F9C mov       dl, 1
0000000180302F9E call      0x18156cc70
0000000180302FA3 mov       r14, qword ptr [rsp + 0x20]
0000000180302FA8 mov       rsi, qword ptr [rsp + 0x50]
0000000180302FAD mov       rbp, qword ptr [rsp + 0x48]
0000000180302FB2 add       rsp, 0x28
0000000180302FB6 pop       rdi
0000000180302FB7 pop       rbx
0000000180302FB8 ret       
0000000180302FB9 mov       rcx, qword ptr [rbx + 0x40]
0000000180302FBD test      rcx, rcx
0000000180302FC0 je        0x18030327e
0000000180302FC6 mov       rax, qword ptr [rip + 0x18b7d2b] # 0x181bbacf8
0000000180302FCD xor       r8d, r8d
0000000180302FD0 mov       rdx, qword ptr [rax + 0xb8]
0000000180302FD7 mov       rdx, qword ptr [rdx]
0000000180302FDA call      0x181197d70
0000000180302FDF mov       rcx, qword ptr [rbx + 0x30]
0000000180302FE3 test      rcx, rcx
0000000180302FE6 je        0x18030327e
0000000180302FEC mov       rax, qword ptr [rip + 0x18b7d05] # 0x181bbacf8
0000000180302FF3 xor       r8d, r8d
0000000180302FF6 mov       rdx, qword ptr [rax + 0xb8]
0000000180302FFD mov       rdx, qword ptr [rdx]
0000000180303000 call      0x181197d70
0000000180303005 mov       rcx, qword ptr [rbx + 0x38]
0000000180303009 test      rcx, rcx
000000018030300C je        0x18030327e
0000000180303012 mov       rax, qword ptr [rip + 0x18b7cdf] # 0x181bbacf8
0000000180303019 xor       r8d, r8d
000000018030301C mov       rdx, qword ptr [rax + 0xb8]
0000000180303023 mov       rdx, qword ptr [rdx]
0000000180303026 call      0x181197d70
000000018030302B mov       rcx, qword ptr [rbx + 0x48]
000000018030302F test      rcx, rcx
0000000180303032 je        0x18030327e
0000000180303038 mov       rax, qword ptr [rip + 0x18b7cb9] # 0x181bbacf8
000000018030303F xor       r8d, r8d
0000000180303042 mov       rdx, qword ptr [rax + 0xb8]
0000000180303049 mov       rdx, qword ptr [rdx]
000000018030304C call      0x181197d70
0000000180303051 mov       rcx, qword ptr [rbx + 0x50]
0000000180303055 test      rcx, rcx
0000000180303058 je        0x18030327e
000000018030305E mov       rax, qword ptr [rip + 0x18b7c93] # 0x181bbacf8
0000000180303065 xor       r8d, r8d
0000000180303068 mov       rdx, qword ptr [rax + 0xb8]
000000018030306F mov       rdx, qword ptr [rdx]
0000000180303072 call      0x181197d70
0000000180303077 mov       rcx, qword ptr [rbx + 0x58]
000000018030307B test      rcx, rcx
000000018030307E je        0x18030327e
0000000180303084 mov       rax, qword ptr [rip + 0x18b7c6d] # 0x181bbacf8
000000018030308B xor       r8d, r8d
000000018030308E mov       rdx, qword ptr [rax + 0xb8]
0000000180303095 mov       rdx, qword ptr [rdx]
0000000180303098 call      0x181197d70
000000018030309D mov       rcx, qword ptr [rbx + 0x60]
00000001803030A1 test      rcx, rcx
00000001803030A4 je        0x18030327e
00000001803030AA mov       rax, qword ptr [rip + 0x18b7c47] # 0x181bbacf8
00000001803030B1 xor       r8d, r8d
00000001803030B4 mov       rdx, qword ptr [rax + 0xb8]
00000001803030BB mov       rdx, qword ptr [rdx]
00000001803030BE call      0x181197d70
00000001803030C3 mov       rcx, qword ptr [rbx + 0x68]
00000001803030C7 test      rcx, rcx
00000001803030CA je        0x18030327e
00000001803030D0 mov       rax, qword ptr [rip + 0x18b7c21] # 0x181bbacf8
00000001803030D7 xor       r8d, r8d
00000001803030DA mov       rdx, qword ptr [rax + 0xb8]
00000001803030E1 mov       rdx, qword ptr [rdx]
00000001803030E4 call      0x181197d70
00000001803030E9 mov       rcx, qword ptr [rbx + 0x80]
00000001803030F0 test      rcx, rcx
00000001803030F3 je        0x18030327e
00000001803030F9 xor       r8d, r8d
00000001803030FC xor       edx, edx
00000001803030FE call      0x181180950
0000000180303103 mov       rcx, qword ptr [rbx + 0x78]
0000000180303107 test      rcx, rcx
000000018030310A je        0x18030327e
0000000180303110 xor       r8d, r8d
0000000180303113 xor       edx, edx
0000000180303115 call      0x181180950
000000018030311A mov       rcx, qword ptr [rbx + 0x70]
000000018030311E test      rcx, rcx
0000000180303121 je        0x18030327e
0000000180303127 xor       r8d, r8d
000000018030312A xor       edx, edx
000000018030312C call      0x181573290
0000000180303131 mov       rcx, qword ptr [rbx + 0x40]
0000000180303135 test      rcx, rcx
0000000180303138 je        0x18030327e
000000018030313E xor       r8d, r8d
0000000180303141 xor       edx, edx
0000000180303143 call      0x18156cc70
0000000180303148 mov       rcx, qword ptr [rbx + 0x30]
000000018030314C test      rcx, rcx
000000018030314F je        0x18030327e
0000000180303155 xor       r8d, r8d
0000000180303158 xor       edx, edx
000000018030315A call      0x18156cc70
000000018030315F mov       rcx, qword ptr [rbx + 0x38]
0000000180303163 test      rcx, rcx
0000000180303166 je        0x18030327e
000000018030316C xor       r8d, r8d
000000018030316F xor       edx, edx
0000000180303171 call      0x18156cc70
0000000180303176 mov       rcx, qword ptr [rbx + 0x68]
000000018030317A test      rcx, rcx
000000018030317D je        0x18030327e
0000000180303183 xor       r8d, r8d
0000000180303186 xor       edx, edx
0000000180303188 call      0x18156cc70
000000018030318D mov       rcx, qword ptr [rbx + 0x80]
0000000180303194 test      rcx, rcx
0000000180303197 je        0x18030327e
000000018030319D xor       r8d, r8d
00000001803031A0 xor       edx, edx
00000001803031A2 call      0x18156cc70
00000001803031A7 mov       rcx, qword ptr [rbx + 0x78]
00000001803031AB test      rcx, rcx
00000001803031AE je        0x18030327e
00000001803031B4 xor       r8d, r8d
00000001803031B7 xor       edx, edx
00000001803031B9 call      0x18156cc70
00000001803031BE mov       rcx, qword ptr [rbx + 0x48]
00000001803031C2 test      rcx, rcx
00000001803031C5 je        0x18030327e
00000001803031CB xor       r8d, r8d
00000001803031CE xor       edx, edx
00000001803031D0 call      0x18156cc70
00000001803031D5 mov       rcx, qword ptr [rbx + 0x50]
00000001803031D9 test      rcx, rcx
00000001803031DC je        0x18030327e
00000001803031E2 xor       r8d, r8d
00000001803031E5 xor       edx, edx
00000001803031E7 call      0x18156cc70
00000001803031EC mov       rcx, qword ptr [rbx + 0x90]
00000001803031F3 test      rcx, rcx
00000001803031F6 je        0x18030327e
00000001803031FC xor       r8d, r8d
00000001803031FF xor       edx, edx
0000000180303201 call      0x18156cc70
0000000180303206 mov       rcx, qword ptr [rbx + 0x98]
000000018030320D test      rcx, rcx
0000000180303210 je        0x18030327e
0000000180303212 xor       r8d, r8d
0000000180303215 xor       edx, edx
0000000180303217 call      0x18156cc70
000000018030321C mov       rcx, qword ptr [rbx + 0x70]
0000000180303220 test      rcx, rcx
0000000180303223 je        0x18030327e
0000000180303225 xor       r8d, r8d
0000000180303228 xor       edx, edx
000000018030322A call      0x18156cc70
000000018030322F mov       rcx, qword ptr [rbx + 0x78]
0000000180303233 test      rcx, rcx
0000000180303236 je        0x18030327e
0000000180303238 xor       r8d, r8d
000000018030323B xor       edx, edx
000000018030323D call      0x18156cc70
0000000180303242 mov       rcx, qword ptr [rbx + 0x58]
0000000180303246 test      rcx, rcx
0000000180303249 je        0x18030327e
000000018030324B xor       r8d, r8d
000000018030324E xor       edx, edx
0000000180303250 call      0x18156cc70
0000000180303255 mov       rcx, qword ptr [rbx + 0x60]
0000000180303259 test      rcx, rcx
000000018030325C je        0x18030327e
000000018030325E xor       r8d, r8d
0000000180303261 xor       edx, edx
0000000180303263 call      0x18156cc70
0000000180303268 mov       rcx, qword ptr [rbx + 0x88]
000000018030326F test      rcx, rcx
0000000180303272 je        0x18030327e
0000000180303274 xor       r8d, r8d
0000000180303277 xor       edx, edx
0000000180303279 jmp       0x180302f9e
000000018030327E call      0x180250150
0000000180303283 int3      
0000000180303284 int3      
0000000180303285 int3      
0000000180303286 int3      
0000000180303287 int3      
0000000180303288 int3      
0000000180303289 int3      
000000018030328A int3      
000000018030328B int3      
000000018030328C int3      
000000018030328D int3      
000000018030328E int3      
000000018030328F int3      