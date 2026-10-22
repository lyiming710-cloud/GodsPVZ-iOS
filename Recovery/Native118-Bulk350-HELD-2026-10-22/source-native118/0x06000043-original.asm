0000000180303CE0 push      rbx
0000000180303CE2 push      rsi
0000000180303CE3 push      rdi
0000000180303CE4 sub       rsp, 0x40
0000000180303CE8 cmp       byte ptr [rip + 0x19e0dd6], 0 # 0x181ce4ac5
0000000180303CEF mov       rdi, rdx
0000000180303CF2 movaps    xmmword ptr [rsp + 0x30], xmm6
0000000180303CF7 mov       rbx, rcx
0000000180303CFA jne       0x180303d6f
0000000180303CFC lea       rcx, [rip + 0x1897045] # 0x181b9ad48
0000000180303D03 call      0x18024fef0
0000000180303D08 lea       rcx, [rip + 0x18c6cd9] # 0x181bca9e8
0000000180303D0F call      0x18024fef0
0000000180303D14 lea       rcx, [rip + 0x18c4a85] # 0x181bc87a0
0000000180303D1B call      0x18024fef0
0000000180303D20 lea       rcx, [rip + 0x18a2e21] # 0x181ba6b48
0000000180303D27 call      0x18024fef0
0000000180303D2C lea       rcx, [rip + 0x18c8e7d] # 0x181bccbb0
0000000180303D33 call      0x18024fef0
0000000180303D38 lea       rcx, [rip + 0x18b2349] # 0x181bb6088
0000000180303D3F call      0x18024fef0
0000000180303D44 lea       rcx, [rip + 0x18bd105] # 0x181bc0e50
0000000180303D4B call      0x18024fef0
0000000180303D50 lea       rcx, [rip + 0x18c4aa1] # 0x181bc87f8
0000000180303D57 call      0x18024fef0
0000000180303D5C lea       rcx, [rip + 0x18c8cad] # 0x181bcca10
0000000180303D63 call      0x18024fef0
0000000180303D68 mov       byte ptr [rip + 0x19e0d56], 1 # 0x181ce4ac5
0000000180303D6F mov       rcx, qword ptr [rip + 0x1896fd2] # 0x181b9ad48
0000000180303D76 xor       eax, eax
0000000180303D78 mov       dword ptr [rsp + 0x24], eax
0000000180303D7C xorps     xmm6, xmm6
0000000180303D7F mov       dword ptr [rsp + 0x68], eax
0000000180303D83 mov       dword ptr [rsp + 0x20], eax
0000000180303D87 mov       dword ptr [rsp + 0x60], eax
0000000180303D8B mov       dword ptr [rsp + 0x78], eax
0000000180303D8F mov       dword ptr [rsp + 0x28], eax
0000000180303D93 cmp       dword ptr [rcx + 0xe0], eax
0000000180303D99 jne       0x180303da0
0000000180303D9B call      0x1802501e0
0000000180303DA0 xor       edx, edx
0000000180303DA2 mov       rcx, rdi
0000000180303DA5 call      0x1812e68f0
0000000180303DAA mov       rsi, qword ptr [rbx + 0xc0]
0000000180303DB1 xor       edx, edx
0000000180303DB3 mov       rcx, rsi
0000000180303DB6 call      0x1803ccf20
0000000180303DBB cmp       eax, 0x9d33518a
0000000180303DC0 jbe       0x180304062
0000000180303DC6 cmp       eax, 0xedbf91b2
0000000180303DCB jbe       0x180303f32
0000000180303DD1 cmp       eax, 0xf5a3c136
0000000180303DD6 jne       0x180303ecf
0000000180303DDC mov       rdx, qword ptr [rip + 0x18c8dcd] # 0x181bccbb0
0000000180303DE3 xor       r8d, r8d
0000000180303DE6 mov       rcx, rsi
0000000180303DE9 call      0x180b76170
0000000180303DEE test      al, al
0000000180303DF0 je        0x1803040ba
0000000180303DF6 xor       r8d, r8d
0000000180303DF9 lea       rdx, [rsp + 0x60]
0000000180303DFE mov       rcx, rdi
0000000180303E01 call      0x180ca3c50
0000000180303E06 test      al, al
0000000180303E08 je        0x180304097
0000000180303E0E mov       eax, dword ptr [rsp + 0x60]
0000000180303E12 test      eax, eax
0000000180303E14 js        0x180303e8a
0000000180303E16 mov       rcx, qword ptr [rbx + 0x20]
0000000180303E1A test      rcx, rcx
0000000180303E1D je        0x1803041b8
0000000180303E23 mov       rdx, qword ptr [rcx + 0x28]
0000000180303E27 test      rdx, rdx
0000000180303E2A je        0x1803041b8
0000000180303E30 cmp       eax, dword ptr [rdx + 0x3c]
0000000180303E33 jg        0x180303e8a
0000000180303E35 mov       rcx, qword ptr [rbx + 0x30]
0000000180303E39 test      rcx, rcx
0000000180303E3C je        0x1803041b8
0000000180303E42 mov       rdx, qword ptr [rcx + 0x38]
0000000180303E46 test      rdx, rdx
0000000180303E49 je        0x1803041b8
0000000180303E4F mov       dword ptr [rdx + 0x28], eax
0000000180303E52 mov       rcx, qword ptr [rbx + 0x30]
0000000180303E56 test      rcx, rcx
0000000180303E59 je        0x1803041b8
0000000180303E5F xor       edx, edx
0000000180303E61 call      0x180304f60
0000000180303E66 mov       rdx, qword ptr [rbx + 0x20]
0000000180303E6A test      rdx, rdx
0000000180303E6D je        0x1803041b8
0000000180303E73 mov       edx, dword ptr [rdx + 0xb8]
0000000180303E79 xor       r8d, r8d
0000000180303E7C mov       rcx, qword ptr [rbx + 0x20]
0000000180303E80 call      0x1802fb330
0000000180303E85 jmp       0x1803040ba
0000000180303E8A mov       rax, qword ptr [rbx + 0x30]
0000000180303E8E mov       rdi, qword ptr [rbx + 0x58]
0000000180303E92 test      rax, rax
0000000180303E95 je        0x1803041b8
0000000180303E9B mov       rcx, qword ptr [rax + 0x38]
0000000180303E9F test      rcx, rcx
0000000180303EA2 je        0x1803041b8
0000000180303EA8 add       rcx, 0x28
0000000180303EAC xor       edx, edx
0000000180303EAE call      0x180ca3820
0000000180303EB3 test      rdi, rdi
0000000180303EB6 je        0x1803041b8
0000000180303EBC xor       r8d, r8d
0000000180303EBF mov       rdx, rax
0000000180303EC2 mov       rcx, rdi
0000000180303EC5 call      0x181197d70
0000000180303ECA jmp       0x1803040ba
0000000180303ECF cmp       eax, 0xff6aff38
0000000180303ED4 jne       0x1803040ba
0000000180303EDA mov       rdx, qword ptr [rip + 0x18c4917] # 0x181bc87f8
0000000180303EE1 xor       r8d, r8d
0000000180303EE4 mov       rcx, rsi
0000000180303EE7 call      0x180b76170
0000000180303EEC test      al, al
0000000180303EEE je        0x1803040ba
0000000180303EF4 xor       r8d, r8d
0000000180303EF7 lea       rdx, [rsp + 0x68]
0000000180303EFC mov       rcx, rdi
0000000180303EFF call      0x180ca3c50
0000000180303F04 test      al, al
0000000180303F06 je        0x180304097
0000000180303F0C mov       rax, qword ptr [rbx + 0x30]
0000000180303F10 test      rax, rax
0000000180303F13 je        0x1803041b8
0000000180303F19 mov       rcx, qword ptr [rax + 0x38]
0000000180303F1D test      rcx, rcx
0000000180303F20 je        0x1803041b8
0000000180303F26 mov       eax, dword ptr [rsp + 0x68]
0000000180303F2A mov       dword ptr [rcx + 0x14], eax
0000000180303F2D jmp       0x1803041a3
0000000180303F32 cmp       eax, 0xd8bceb5e
0000000180303F37 jne       0x180304003
0000000180303F3D mov       rdx, qword ptr [rip + 0x18b2144] # 0x181bb6088
0000000180303F44 xor       r8d, r8d
0000000180303F47 mov       rcx, rsi
0000000180303F4A call      0x180b76170
0000000180303F4F test      al, al
0000000180303F51 je        0x1803040ba
0000000180303F57 xor       r8d, r8d
0000000180303F5A lea       rdx, [rsp + 0x78]
0000000180303F5F mov       rcx, rdi
0000000180303F62 call      0x180ca3c50
0000000180303F67 test      al, al
0000000180303F69 je        0x180304097
0000000180303F6F mov       edx, dword ptr [rsp + 0x78]
0000000180303F73 test      edx, edx
0000000180303F75 js        0x180303fc8
0000000180303F77 mov       rax, qword ptr [rbx + 0x20]
0000000180303F7B test      rax, rax
0000000180303F7E je        0x1803041b8
0000000180303F84 mov       rcx, qword ptr [rax + 0x28]
0000000180303F88 test      rcx, rcx
0000000180303F8B je        0x1803041b8
0000000180303F91 cmp       edx, dword ptr [rcx + 0x3c]
0000000180303F94 jg        0x180303fc8
0000000180303F96 test      rax, rax
0000000180303F99 je        0x1803041b8
0000000180303F9F cmp       dword ptr [rax + 0xb8], edx
0000000180303FA5 je        0x1803040ba
0000000180303FAB xor       r8d, r8d
0000000180303FAE mov       rcx, rax
0000000180303FB1 call      0x1802fb330
0000000180303FB6 xor       r8d, r8d
0000000180303FB9 xor       edx, edx
0000000180303FBB mov       rcx, rbx
0000000180303FBE call      0x1803041c0
0000000180303FC3 jmp       0x1803040ba
0000000180303FC8 mov       rcx, qword ptr [rbx + 0x20]
0000000180303FCC mov       rdi, qword ptr [rbx + 0x50]
0000000180303FD0 test      rcx, rcx
0000000180303FD3 je        0x1803041b8
0000000180303FD9 add       rcx, 0xb8
0000000180303FE0 xor       edx, edx
0000000180303FE2 call      0x180ca3820
0000000180303FE7 test      rdi, rdi
0000000180303FEA je        0x1803041b8
0000000180303FF0 xor       r8d, r8d
0000000180303FF3 mov       rdx, rax
0000000180303FF6 mov       rcx, rdi
0000000180303FF9 call      0x181197d70
0000000180303FFE jmp       0x1803040ba
0000000180304003 cmp       eax, 0xedbf91b2
0000000180304008 jne       0x1803040ba
000000018030400E mov       rdx, qword ptr [rip + 0x18c89fb] # 0x181bcca10
0000000180304015 xor       r8d, r8d
0000000180304018 mov       rcx, rsi
000000018030401B call      0x180b76170
0000000180304020 test      al, al
0000000180304022 je        0x1803040ba
0000000180304028 xor       r8d, r8d
000000018030402B lea       rdx, [rsp + 0x20]
0000000180304030 mov       rcx, rdi
0000000180304033 call      0x180ca3c50
0000000180304038 test      al, al
000000018030403A je        0x180304097
000000018030403C mov       rax, qword ptr [rbx + 0x30]
0000000180304040 test      rax, rax
0000000180304043 je        0x1803041b8
0000000180304049 mov       rcx, qword ptr [rax + 0x38]
000000018030404D test      rcx, rcx
0000000180304050 je        0x1803041b8
0000000180304056 mov       eax, dword ptr [rsp + 0x20]
000000018030405A mov       dword ptr [rcx + 0x10], eax
000000018030405D jmp       0x1803041a3
0000000180304062 cmp       eax, 0x6b00cb
0000000180304067 jne       0x1803040fc
000000018030406D mov       rdx, qword ptr [rip + 0x18c472c] # 0x181bc87a0
0000000180304074 xor       r8d, r8d
0000000180304077 mov       rcx, rsi
000000018030407A call      0x180b76170
000000018030407F test      al, al
0000000180304081 je        0x1803040ba
0000000180304083 xor       r8d, r8d
0000000180304086 lea       rdx, [rsp + 0x24]
000000018030408B mov       rcx, rdi
000000018030408E call      0x180ca3c50
0000000180304093 test      al, al
0000000180304095 jne       0x1803040d6
0000000180304097 mov       rcx, qword ptr [rip + 0x1896caa] # 0x181b9ad48
000000018030409E cmp       dword ptr [rcx + 0xe0], 0
00000001803040A5 jne       0x1803040ac
00000001803040A7 call      0x1802501e0
00000001803040AC mov       rcx, qword ptr [rip + 0x18a2a95] # 0x181ba6b48
00000001803040B3 xor       edx, edx
00000001803040B5 call      0x1812e68f0
00000001803040BA mov       rdx, qword ptr [rbx + 0x30]
00000001803040BE xor       r8d, r8d
00000001803040C1 mov       rcx, rbx
00000001803040C4 call      0x1803041c0
00000001803040C9 movaps    xmm6, xmmword ptr [rsp + 0x30]
00000001803040CE add       rsp, 0x40
00000001803040D2 pop       rdi
00000001803040D3 pop       rsi
00000001803040D4 pop       rbx
00000001803040D5 ret       
00000001803040D6 mov       rax, qword ptr [rbx + 0x30]
00000001803040DA test      rax, rax
00000001803040DD je        0x1803041b8
00000001803040E3 mov       rcx, qword ptr [rax + 0x38]
00000001803040E7 test      rcx, rcx
00000001803040EA je        0x1803041b8
00000001803040F0 mov       eax, dword ptr [rsp + 0x24]
00000001803040F4 mov       dword ptr [rcx + 0x18], eax
00000001803040F7 jmp       0x1803041a3
00000001803040FC cmp       eax, 0x64371221
0000000180304101 jne       0x18030415c
0000000180304103 mov       rdx, qword ptr [rip + 0x18c68de] # 0x181bca9e8
000000018030410A xor       r8d, r8d
000000018030410D mov       rcx, rsi
0000000180304110 call      0x180b76170
0000000180304115 test      al, al
0000000180304117 je        0x1803040ba
0000000180304119 xor       r8d, r8d
000000018030411C lea       rdx, [rsp + 0x28]
0000000180304121 mov       rcx, rdi
0000000180304124 call      0x180cc6040
0000000180304129 test      al, al
000000018030412B je        0x180304097
0000000180304131 movss     xmm0, dword ptr [rsp + 0x28]
0000000180304137 comiss    xmm0, xmm6
000000018030413A jb        0x1803040ba
0000000180304140 mov       rax, qword ptr [rbx + 0x30]
0000000180304144 test      rax, rax
0000000180304147 je        0x1803041b8
0000000180304149 mov       rcx, qword ptr [rax + 0x38]
000000018030414D test      rcx, rcx
0000000180304150 je        0x1803041b8
0000000180304152 movss     dword ptr [rcx + 0x2c], xmm0
0000000180304157 jmp       0x1803040ba
000000018030415C cmp       eax, 0x9d33518a
0000000180304161 jne       0x1803040ba
0000000180304167 mov       rdx, qword ptr [rip + 0x18bcce2] # 0x181bc0e50
000000018030416E xor       r8d, r8d
0000000180304171 mov       rcx, rsi
0000000180304174 call      0x180b76170
0000000180304179 test      al, al
000000018030417B je        0x1803040ba
0000000180304181 mov       rcx, qword ptr [rbx + 0x30]
0000000180304185 test      rcx, rcx
0000000180304188 je        0x1803041b8
000000018030418A mov       rcx, qword ptr [rcx + 0x38]
000000018030418E test      rcx, rcx
0000000180304191 je        0x1803041b8
0000000180304193 mov       qword ptr [rcx + 0x38], rdi
0000000180304197 mov       rdx, rdi
000000018030419A add       rcx, 0x38
000000018030419E call      0x18024f360
00000001803041A3 mov       rcx, qword ptr [rbx + 0x30]
00000001803041A7 test      rcx, rcx
00000001803041AA je        0x1803041b8
00000001803041AC xor       edx, edx
00000001803041AE call      0x180304f60
00000001803041B3 jmp       0x1803040ba
00000001803041B8 call      0x180250150
00000001803041BD int3      
00000001803041BE int3      
00000001803041BF int3      