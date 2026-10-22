0000000180301A80 mov       qword ptr [rsp + 0x10], rdx
0000000180301A85 mov       qword ptr [rsp + 8], rcx
0000000180301A8A push      rbx
0000000180301A8B push      rsi
0000000180301A8C push      rdi
0000000180301A8D push      r14
0000000180301A8F push      r15
0000000180301A91 sub       rsp, 0x90
0000000180301A98 mov       rdi, rdx
0000000180301A9B mov       r14, rcx
0000000180301A9E cmp       byte ptr [rip + 0x19e3089], 0 # 0x181ce4b2e
0000000180301AA5 jne       0x180301ba2
0000000180301AAB lea       rcx, [rip + 0x1899296] # 0x181b9ad48
0000000180301AB2 call      0x18024fef0
0000000180301AB7 lea       rcx, [rip + 0x18bfaba] # 0x181bc1578
0000000180301ABE call      0x18024fef0
0000000180301AC3 lea       rcx, [rip + 0x18c5146] # 0x181bc6c10
0000000180301ACA call      0x18024fef0
0000000180301ACF lea       rcx, [rip + 0x18c5192] # 0x181bc6c68
0000000180301AD6 call      0x18024fef0
0000000180301ADB lea       rcx, [rip + 0x18bfaee] # 0x181bc15d0
0000000180301AE2 call      0x18024fef0
0000000180301AE7 lea       rcx, [rip + 0x18c51d2] # 0x181bc6cc0
0000000180301AEE call      0x18024fef0
0000000180301AF3 lea       rcx, [rip + 0x18bfb2e] # 0x181bc1628
0000000180301AFA call      0x18024fef0
0000000180301AFF lea       rcx, [rip + 0x18b382a] # 0x181bb5330
0000000180301B06 call      0x18024fef0
0000000180301B0B lea       rcx, [rip + 0x18a3186] # 0x181ba4c98
0000000180301B12 call      0x18024fef0
0000000180301B17 lea       rcx, [rip + 0x18b386a] # 0x181bb5388
0000000180301B1E call      0x18024fef0
0000000180301B23 lea       rcx, [rip + 0x18a322e] # 0x181ba4d58
0000000180301B2A call      0x18024fef0
0000000180301B2F lea       rcx, [rip + 0x18a3102] # 0x181ba4c38
0000000180301B36 call      0x18024fef0
0000000180301B3B lea       rcx, [rip + 0x18b3796] # 0x181bb52d8
0000000180301B42 call      0x18024fef0
0000000180301B47 lea       rcx, [rip + 0x18bdc5a] # 0x181bbf7a8
0000000180301B4E call      0x18024fef0
0000000180301B53 lea       rcx, [rip + 0x18badae] # 0x181bbc908
0000000180301B5A call      0x18024fef0
0000000180301B5F lea       rcx, [rip + 0x18b5d82] # 0x181bb78e8
0000000180301B66 call      0x18024fef0
0000000180301B6B lea       rcx, [rip + 0x18ae98e] # 0x181bb0500
0000000180301B72 call      0x18024fef0
0000000180301B77 lea       rcx, [rip + 0x18b48fa] # 0x181bb6478
0000000180301B7E call      0x18024fef0
0000000180301B83 lea       rcx, [rip + 0x18b916e] # 0x181bbacf8
0000000180301B8A call      0x18024fef0
0000000180301B8F lea       rcx, [rip + 0x18a00fa] # 0x181ba1c90
0000000180301B96 call      0x18024fef0
0000000180301B9B mov       byte ptr [rip + 0x19e2f8c], 1 # 0x181ce4b2e
0000000180301BA2 xorps     xmm0, xmm0
0000000180301BA5 xor       eax, eax
0000000180301BA7 movups    xmmword ptr [rsp + 0x38], xmm0
0000000180301BAC mov       qword ptr [rsp + 0x48], rax
0000000180301BB1 mov       rcx, qword ptr [rip + 0x18bad50] # 0x181bbc908
0000000180301BB8 call      0x180250100
0000000180301BBD mov       r15, rax
0000000180301BC0 mov       rdx, qword ptr [rip + 0x18a3071] # 0x181ba4c38
0000000180301BC7 mov       rcx, rax
0000000180301BCA call      0x1808053d0
0000000180301BCF mov       qword ptr [rsp + 0x50], r15
0000000180301BD4 test      rdi, rdi
0000000180301BD7 je        0x1803020e9
0000000180301BDD mov       rdx, qword ptr [rdi + 0x68]
0000000180301BE1 test      rdx, rdx
0000000180301BE4 je        0x1803020e9
0000000180301BEA mov       r8, qword ptr [rip + 0x18a3167] # 0x181ba4d58
0000000180301BF1 lea       rcx, [rsp + 0x20]
0000000180301BF6 call      0x1808197f0
0000000180301BFB movups    xmm0, xmmword ptr [rsp + 0x20]
0000000180301C00 movups    xmmword ptr [rsp + 0x58], xmm0
0000000180301C05 movsd     xmm1, qword ptr [rsp + 0x30]
0000000180301C0B movsd     qword ptr [rsp + 0x68], xmm1
0000000180301C11 mov       qword ptr [rsp + 0x20], 0
0000000180301C1A lea       rbx, [rsp + 0x58]
0000000180301C1F mov       qword ptr [rsp + 0x28], rbx
0000000180301C24 mov       rdx, qword ptr [rip + 0x18bf9a5] # 0x181bc15d0
0000000180301C2B lea       rcx, [rsp + 0x58]
0000000180301C30 call      0x180669280
0000000180301C35 test      al, al
0000000180301C37 je        0x180301c67
0000000180301C39 mov       rdx, qword ptr [rsp + 0x68]
0000000180301C3E test      rdx, rdx
0000000180301C41 je        0x1803020a2
0000000180301C47 cmp       dword ptr [rdx + 0x10], 0x24
0000000180301C4B jne       0x180301c24
0000000180301C4D test      r15, r15
0000000180301C50 je        0x18030209c
0000000180301C56 mov       r8, qword ptr [rip + 0x18a303b] # 0x181ba4c98
0000000180301C5D mov       rcx, r15
0000000180301C60 call      0x180002e70
0000000180301C65 jmp       0x180301c24
0000000180301C67 mov       rdx, qword ptr [rip + 0x18bf90a] # 0x181bc1578
0000000180301C6E mov       rcx, rbx
0000000180301C71 call      0x180302170
0000000180301C76 mov       rcx, qword ptr [rip + 0x18bdb2b] # 0x181bbf7a8
0000000180301C7D call      0x180250100
0000000180301C82 mov       rsi, rax
0000000180301C85 mov       rdx, qword ptr [rip + 0x18b364c] # 0x181bb52d8
0000000180301C8C mov       rcx, rax
0000000180301C8F call      0x1808053d0
0000000180301C94 mov       qword ptr [rsp + 0xd8], rsi
0000000180301C9C jmp       0x180301cf8
0000000180301C9E mov       rdx, qword ptr [rip + 0x18bf8d3] # 0x181bc1578
0000000180301CA5 mov       rcx, qword ptr [rsp + 0x28]
0000000180301CAA call      0x180302170
0000000180301CAF mov       rcx, qword ptr [rsp + 0x20]
0000000180301CB4 test      rcx, rcx
0000000180301CB7 jne       0x1803020a8
0000000180301CBD mov       rcx, qword ptr [rip + 0x18bdae4] # 0x181bbf7a8
0000000180301CC4 call      0x180250100
0000000180301CC9 mov       rsi, rax
0000000180301CCC mov       rdx, qword ptr [rip + 0x18b3605] # 0x181bb52d8
0000000180301CD3 mov       rcx, rax
0000000180301CD6 call      0x1808053d0
0000000180301CDB mov       qword ptr [rsp + 0xd8], rsi
0000000180301CE3 mov       rdi, qword ptr [rsp + 0xc8]
0000000180301CEB mov       r14, qword ptr [rsp + 0xc0]
0000000180301CF3 mov       r15, qword ptr [rsp + 0x50]
0000000180301CF8 mov       rdx, qword ptr [rdi + 0x60]
0000000180301CFC test      rdx, rdx
0000000180301CFF je        0x1803020e9
0000000180301D05 mov       r8, qword ptr [rip + 0x18b367c] # 0x181bb5388
0000000180301D0C lea       rcx, [rsp + 0x20]
0000000180301D11 call      0x1808197f0
0000000180301D16 movups    xmm0, xmmword ptr [rsp + 0x20]
0000000180301D1B movups    xmmword ptr [rsp + 0x38], xmm0
0000000180301D20 movsd     xmm1, qword ptr [rsp + 0x30]
0000000180301D26 movsd     qword ptr [rsp + 0x48], xmm1
0000000180301D2C mov       qword ptr [rsp + 0x20], 0
0000000180301D35 lea       rbx, [rsp + 0x38]
0000000180301D3A mov       qword ptr [rsp + 0x28], rbx
0000000180301D3F nop       
0000000180301D40 mov       rdx, qword ptr [rip + 0x18c4f21] # 0x181bc6c68
0000000180301D47 lea       rcx, [rsp + 0x38]
0000000180301D4C call      0x180669280
0000000180301D51 test      al, al
0000000180301D53 je        0x180301df8
0000000180301D59 mov       rdi, qword ptr [rsp + 0x48]
0000000180301D5E test      rdi, rdi
0000000180301D61 je        0x1803020bd
0000000180301D67 mov       rax, qword ptr [rip + 0x18b8f8a] # 0x181bbacf8
0000000180301D6E mov       rdx, qword ptr [rax + 0xb8]
0000000180301D75 xor       r8d, r8d
0000000180301D78 mov       rdx, qword ptr [rdx]
0000000180301D7B mov       rcx, qword ptr [rdi + 0x30]
0000000180301D7F call      0x180b7cc30
0000000180301D84 test      al, al
0000000180301D86 jne       0x180301d93
0000000180301D88 test      rsi, rsi
0000000180301D8B je        0x1803020ae
0000000180301D91 jmp       0x180301db9
0000000180301D93 test      rdi, rdi
0000000180301D96 je        0x1803020b8
0000000180301D9C xor       r8d, r8d
0000000180301D9F mov       rdx, qword ptr [rdi + 0x30]
0000000180301DA3 mov       rcx, r15
0000000180301DA6 call      0x180394560
0000000180301DAB test      rax, rax
0000000180301DAE jne       0x180301dd0
0000000180301DB0 test      rsi, rsi
0000000180301DB3 je        0x1803020b3
0000000180301DB9 mov       r8, qword ptr [rip + 0x18b3570] # 0x181bb5330
0000000180301DC0 mov       rdx, rdi
0000000180301DC3 mov       rcx, rsi
0000000180301DC6 call      0x180002e70
0000000180301DCB jmp       0x180301d40
0000000180301DD0 mov       rcx, qword ptr [rip + 0x1898f71] # 0x181b9ad48
0000000180301DD7 cmp       dword ptr [rcx + 0xe0], 0
0000000180301DDE jne       0x180301de5
0000000180301DE0 call      0x1802501e0
0000000180301DE5 xor       edx, edx
0000000180301DE7 mov       rcx, qword ptr [rip + 0x189fea2] # 0x181ba1c90
0000000180301DEE call      0x1812e68f0
0000000180301DF3 jmp       0x180301d40
0000000180301DF8 mov       rdx, qword ptr [rip + 0x18c4e11] # 0x181bc6c10
0000000180301DFF mov       rcx, rbx
0000000180301E02 call      0x180302170
0000000180301E07 jmp       0x180301e38
0000000180301E09 mov       rdx, qword ptr [rip + 0x18c4e00] # 0x181bc6c10
0000000180301E10 mov       rcx, qword ptr [rsp + 0x28]
0000000180301E15 call      0x180302170
0000000180301E1A mov       rcx, qword ptr [rsp + 0x20]
0000000180301E1F test      rcx, rcx
0000000180301E22 jne       0x1803020c3
0000000180301E28 mov       r14, qword ptr [rsp + 0xc0]
0000000180301E30 mov       rsi, qword ptr [rsp + 0xd8]
0000000180301E38 test      rsi, rsi
0000000180301E3B je        0x1803020e9
0000000180301E41 mov       r8, qword ptr [rip + 0x18b3540] # 0x181bb5388
0000000180301E48 mov       rdx, rsi
0000000180301E4B lea       rcx, [rsp + 0x20]
0000000180301E50 call      0x1808197f0
0000000180301E55 movups    xmm0, xmmword ptr [rsp + 0x20]
0000000180301E5A movups    xmmword ptr [rsp + 0x38], xmm0
0000000180301E5F movsd     xmm1, qword ptr [rsp + 0x30]
0000000180301E65 movsd     qword ptr [rsp + 0x48], xmm1
0000000180301E6B mov       qword ptr [rsp + 0x20], 0
0000000180301E74 lea       rbx, [rsp + 0x38]
0000000180301E79 mov       qword ptr [rsp + 0x28], rbx
0000000180301E7E nop       
0000000180301E80 mov       rdx, qword ptr [rip + 0x18c4de1] # 0x181bc6c68
0000000180301E87 lea       rcx, [rsp + 0x38]
0000000180301E8C call      0x180669280
0000000180301E91 test      al, al
0000000180301E93 je        0x180302061
0000000180301E99 mov       rsi, qword ptr [rsp + 0x48]
0000000180301E9E mov       rax, qword ptr [rip + 0x18b45d3] # 0x181bb6478
0000000180301EA5 cmp       dword ptr [rax + 0xe0], 0
0000000180301EAC jne       0x180301ebd
0000000180301EAE mov       rcx, rax
0000000180301EB1 call      0x1802501e0
0000000180301EB6 mov       rax, qword ptr [rip + 0x18b45bb] # 0x181bb6478
0000000180301EBD mov       rax, qword ptr [rax + 0xb8]
0000000180301EC4 mov       rdi, qword ptr [rax + 0x180]
0000000180301ECB mov       rcx, qword ptr [rip + 0x18ae62e] # 0x181bb0500
0000000180301ED2 cmp       dword ptr [rcx + 0xe0], 0
0000000180301ED9 jne       0x180301ee0
0000000180301EDB call      0x1802501e0
0000000180301EE0 mov       rdx, qword ptr [rip + 0x18b5a01] # 0x181bb78e8
0000000180301EE7 mov       rcx, rdi
0000000180301EEA call      0x1804a0690
0000000180301EEF mov       rdi, rax
0000000180301EF2 mov       rdx, qword ptr [r14 + 0x20]
0000000180301EF6 test      rax, rax
0000000180301EF9 je        0x1803020dd
0000000180301EFF mov       qword ptr [rax + 0x20], rdx
0000000180301F03 lea       rcx, [rax + 0x20]
0000000180301F07 call      0x18024f360
0000000180301F0C xor       r8d, r8d
0000000180301F0F mov       rdx, rsi
0000000180301F12 mov       rcx, rdi
0000000180301F15 call      0x1803448e0
0000000180301F1A cmp       byte ptr [rip + 0x19e2c05], 0 # 0x181ce4b26
0000000180301F21 jne       0x180301f5a
0000000180301F23 lea       rcx, [rip + 0x18a3e4e] # 0x181ba5d78
0000000180301F2A call      0x18024fef0
0000000180301F2F lea       rcx, [rip + 0x18a3fc2] # 0x181ba5ef8
0000000180301F36 call      0x18024fef0
0000000180301F3B lea       rcx, [rip + 0x18ae5be] # 0x181bb0500
0000000180301F42 call      0x18024fef0
0000000180301F47 lea       rcx, [rip + 0x18b452a] # 0x181bb6478
0000000180301F4E call      0x18024fef0
0000000180301F53 mov       byte ptr [rip + 0x19e2bcc], 1 # 0x181ce4b26
0000000180301F5A mov       byte ptr [rdi + 0x54], 1
0000000180301F5E mov       rax, qword ptr [r14 + 0x30]
0000000180301F62 test      rax, rax
0000000180301F65 je        0x1803020d8
0000000180301F6B mov       esi, dword ptr [rax + 0x18]
0000000180301F6E mov       rcx, rax
0000000180301F71 mov       r9, qword ptr [rip + 0x18a3e00] # 0x181ba5d78
0000000180301F78 inc       dword ptr [rax + 0x1c]
0000000180301F7B mov       rdx, qword ptr [rax + 0x10]
0000000180301F7F movsxd    r8, dword ptr [rax + 0x18]
0000000180301F83 test      rdx, rdx
0000000180301F86 je        0x1803020d3
0000000180301F8C cmp       r8d, dword ptr [rdx + 0x18]
0000000180301F90 jb        0x180301fab
0000000180301F92 mov       rax, qword ptr [r9 + 0x20]
0000000180301F96 mov       r8, qword ptr [rax + 0xc0]
0000000180301F9D mov       r8, qword ptr [r8 + 0x70]
0000000180301FA1 mov       rdx, rdi
0000000180301FA4 call      0x180847ce0
0000000180301FA9 jmp       0x180301fd4
0000000180301FAB lea       eax, [r8 + 1]
0000000180301FAF mov       dword ptr [rcx + 0x18], eax
0000000180301FB2 mov       rax, r8
0000000180301FB5 cmp       r8d, dword ptr [rdx + 0x18]
0000000180301FB9 jae       0x1803020c9
0000000180301FBF mov       qword ptr [rdx + r8*8 + 0x20], rdi
0000000180301FC4 add       rax, 4
0000000180301FC8 lea       rcx, [rdx + rax*8]
0000000180301FCC mov       rdx, rdi
0000000180301FCF call      0x18024f360
0000000180301FD4 mov       dword ptr [rdi + 0x58], esi
0000000180301FD7 xor       r9d, r9d
0000000180301FDA mov       r8d, esi
0000000180301FDD mov       rdx, rdi
0000000180301FE0 mov       rcx, r14
0000000180301FE3 call      0x180301990
0000000180301FE8 mov       r15, qword ptr [r14 + 0x20]
0000000180301FEC mov       rcx, qword ptr [rip + 0x18ae50d] # 0x181bb0500
0000000180301FF3 cmp       dword ptr [rcx + 0xe0], 0
0000000180301FFA jne       0x180302001
0000000180301FFC call      0x1802501e0
0000000180302001 xor       edx, edx
0000000180302003 mov       rcx, r15
0000000180302006 call      0x18131f870
000000018030200B test      al, al
000000018030200D je        0x180302039
000000018030200F mov       rax, qword ptr [r14 + 0x20]
0000000180302013 test      rax, rax
0000000180302016 je        0x1803020ce
000000018030201C cmp       byte ptr [rax + 0x41], 0
0000000180302020 je        0x180302039
0000000180302022 xor       r8d, r8d
0000000180302025 mov       dl, 1
0000000180302027 mov       rcx, rdi
000000018030202A call      0x180344e90
000000018030202F xor       edx, edx
0000000180302031 mov       rcx, rdi
0000000180302034 call      0x180343140
0000000180302039 lea       eax, [rsi + 1]
000000018030203C mov       dword ptr [r14 + 0x44], eax
0000000180302040 mov       rcx, qword ptr [rip + 0x18b4431] # 0x181bb6478
0000000180302047 cmp       dword ptr [rcx + 0xe0], 0
000000018030204E jne       0x180302055
0000000180302050 call      0x1802501e0
0000000180302055 xor       ecx, ecx
0000000180302057 call      0x1803394d0
000000018030205C jmp       0x180301e80
0000000180302061 mov       rdx, qword ptr [rip + 0x18c4ba8] # 0x181bc6c10
0000000180302068 mov       rcx, rbx
000000018030206B call      0x180302170
0000000180302070 jmp       0x18030208d
0000000180302072 mov       rdx, qword ptr [rip + 0x18c4b97] # 0x181bc6c10
0000000180302079 mov       rcx, qword ptr [rsp + 0x28]
000000018030207E call      0x180302170
0000000180302083 mov       rcx, qword ptr [rsp + 0x20]
0000000180302088 test      rcx, rcx
000000018030208B jne       0x1803020e3
000000018030208D add       rsp, 0x90
0000000180302094 pop       r15
0000000180302096 pop       r14
0000000180302098 pop       rdi
0000000180302099 pop       rsi
000000018030209A pop       rbx
000000018030209B ret       
000000018030209C call      0x180250150
00000001803020A1 nop       
00000001803020A2 call      0x180250150
00000001803020A7 nop       
00000001803020A8 call      0x180200e60
00000001803020AD nop       
00000001803020AE call      0x180250150
00000001803020B3 call      0x180250150
00000001803020B8 call      0x180250150
00000001803020BD call      0x180250150
00000001803020C2 nop       
00000001803020C3 call      0x180200e60
00000001803020C8 nop       
00000001803020C9 call      0x180250140
00000001803020CE call      0x180250150
00000001803020D3 call      0x180250150
00000001803020D8 call      0x180250150
00000001803020DD call      0x180250150
00000001803020E2 nop       
00000001803020E3 call      0x180200e60
00000001803020E8 int3      
00000001803020E9 call      0x180250150
00000001803020EE int3      
00000001803020EF int3      