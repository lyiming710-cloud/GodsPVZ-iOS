00000001803A8D30 mov       qword ptr [rsp + 8], rbx
00000001803A8D35 mov       qword ptr [rsp + 0x10], rsi
00000001803A8D3A mov       qword ptr [rsp + 0x18], rdi
00000001803A8D3F push      r14
00000001803A8D41 sub       rsp, 0x60
00000001803A8D45 mov       ebx, edx
00000001803A8D47 mov       rsi, rcx
00000001803A8D4A cmp       byte ptr [rip + 0x193c1aa], 0 # 0x181ce4efb
00000001803A8D51 jne       0x1803a8dc6
00000001803A8D53 lea       rcx, [rip + 0x181e5ee] # 0x181bc7348
00000001803A8D5A call      0x18024fef0
00000001803A8D5F lea       rcx, [rip + 0x181e63a] # 0x181bc73a0
00000001803A8D66 call      0x18024fef0
00000001803A8D6B lea       rcx, [rip + 0x181e686] # 0x181bc73f8
00000001803A8D72 call      0x18024fef0
00000001803A8D77 lea       rcx, [rip + 0x17f929a] # 0x181ba2018
00000001803A8D7E call      0x18024fef0
00000001803A8D83 lea       rcx, [rip + 0x180d156] # 0x181bb5ee0
00000001803A8D8A call      0x18024fef0
00000001803A8D8F lea       rcx, [rip + 0x181f12a] # 0x181bc7ec0
00000001803A8D96 call      0x18024fef0
00000001803A8D9B lea       rcx, [rip + 0x17fb4d6] # 0x181ba4278
00000001803A8DA2 call      0x18024fef0
00000001803A8DA7 lea       rcx, [rip + 0x1807752] # 0x181bb0500
00000001803A8DAE call      0x18024fef0
00000001803A8DB3 lea       rcx, [rip + 0x180d6be] # 0x181bb6478
00000001803A8DBA call      0x18024fef0
00000001803A8DBF mov       byte ptr [rip + 0x193c135], 1 # 0x181ce4efb
00000001803A8DC6 mov       dword ptr [rsi + 0x48], ebx
00000001803A8DC9 mov       rcx, qword ptr [rsi + 0x30]
00000001803A8DCD test      rcx, rcx
00000001803A8DD0 je        0x1803a8fb9
00000001803A8DD6 xor       r8d, r8d
00000001803A8DD9 mov       dl, 1
00000001803A8DDB call      0x18131bad0
00000001803A8DE0 mov       rax, qword ptr [rip + 0x180d691] # 0x181bb6478
00000001803A8DE7 cmp       dword ptr [rax + 0xe0], 0
00000001803A8DEE jne       0x1803a8dff
00000001803A8DF0 mov       rcx, rax
00000001803A8DF3 call      0x1802501e0
00000001803A8DF8 mov       rax, qword ptr [rip + 0x180d679] # 0x181bb6478
00000001803A8DFF mov       rax, qword ptr [rax + 0xb8]
00000001803A8E06 mov       rcx, qword ptr [rax + 0x138]
00000001803A8E0D test      rcx, rcx
00000001803A8E10 je        0x1803a8fb9
00000001803A8E16 mov       r8, qword ptr [rip + 0x17fb45b] # 0x181ba4278
00000001803A8E1D mov       edx, 9
00000001803A8E22 call      0x180825710
00000001803A8E27 mov       rbx, rax
00000001803A8E2A mov       rcx, qword ptr [rip + 0x17f91e7] # 0x181ba2018
00000001803A8E31 cmp       dword ptr [rcx + 0xe0], 0
00000001803A8E38 jne       0x1803a8e3f
00000001803A8E3A call      0x1802501e0
00000001803A8E3F xor       edx, edx
00000001803A8E41 mov       rcx, rbx
00000001803A8E44 call      0x18031b160
00000001803A8E49 mov       rdx, qword ptr [rsi + 0x28]
00000001803A8E4D test      rdx, rdx
00000001803A8E50 je        0x1803a8fb9
00000001803A8E56 mov       r8, qword ptr [rip + 0x180d083] # 0x181bb5ee0
00000001803A8E5D lea       rcx, [rsp + 0x28]
00000001803A8E62 call      0x1808197f0
00000001803A8E67 movups    xmm0, xmmword ptr [rsp + 0x28]
00000001803A8E6C movups    xmmword ptr [rsp + 0x40], xmm0
00000001803A8E71 movsd     xmm1, qword ptr [rsp + 0x38]
00000001803A8E77 movsd     qword ptr [rsp + 0x50], xmm1
00000001803A8E7D mov       qword ptr [rsp + 0x28], 0
00000001803A8E86 lea       rbx, [rsp + 0x40]
00000001803A8E8B mov       qword ptr [rsp + 0x30], rbx
00000001803A8E90 mov       rdx, qword ptr [rip + 0x181e509] # 0x181bc73a0
00000001803A8E97 lea       rcx, [rsp + 0x40]
00000001803A8E9C call      0x180669280
00000001803A8EA1 test      al, al
00000001803A8EA3 je        0x1803a8f4e
00000001803A8EA9 mov       rdi, qword ptr [rsp + 0x50]
00000001803A8EAE mov       rcx, qword ptr [rsi + 0x40]
00000001803A8EB2 test      rcx, rcx
00000001803A8EB5 je        0x1803a8fad
00000001803A8EBB mov       r8, qword ptr [rip + 0x181effe] # 0x181bc7ec0
00000001803A8EC2 mov       edx, dword ptr [rsi + 0x48]
00000001803A8EC5 call      0x180825710
00000001803A8ECA test      rax, rax
00000001803A8ECD je        0x1803a8fa8
00000001803A8ED3 mov       r14, qword ptr [rax + 0x38]
00000001803A8ED7 mov       rcx, qword ptr [rip + 0x1807622] # 0x181bb0500
00000001803A8EDE cmp       dword ptr [rcx + 0xe0], 0
00000001803A8EE5 jne       0x1803a8eec
00000001803A8EE7 call      0x1802501e0
00000001803A8EEC xor       r8d, r8d
00000001803A8EEF mov       rdx, r14
00000001803A8EF2 mov       rcx, rdi
00000001803A8EF5 call      0x18131f760
00000001803A8EFA test      al, al
00000001803A8EFC jne       0x1803a8f28
00000001803A8EFE test      rdi, rdi
00000001803A8F01 je        0x1803a8f99
00000001803A8F07 xor       edx, edx
00000001803A8F09 mov       rcx, rdi
00000001803A8F0C call      0x1813044d0
00000001803A8F11 test      rax, rax
00000001803A8F14 je        0x1803a8f93
00000001803A8F16 xor       r8d, r8d
00000001803A8F19 mov       dl, 1
00000001803A8F1B mov       rcx, rax
00000001803A8F1E call      0x18131bad0
00000001803A8F23 jmp       0x1803a8e90
00000001803A8F28 test      rdi, rdi
00000001803A8F2B je        0x1803a8fa3
00000001803A8F2D xor       edx, edx
00000001803A8F2F mov       rcx, rdi
00000001803A8F32 call      0x1813044d0
00000001803A8F37 test      rax, rax
00000001803A8F3A je        0x1803a8f9e
00000001803A8F3C xor       r8d, r8d
00000001803A8F3F xor       edx, edx
00000001803A8F41 mov       rcx, rax
00000001803A8F44 call      0x18131bad0
00000001803A8F49 jmp       0x1803a8e90
00000001803A8F4E mov       rdx, qword ptr [rip + 0x181e3f3] # 0x181bc7348
00000001803A8F55 mov       rcx, rbx
00000001803A8F58 call      0x180302170
00000001803A8F5D jmp       0x1803a8f7a
00000001803A8F5F mov       rdx, qword ptr [rip + 0x181e3e2] # 0x181bc7348
00000001803A8F66 mov       rcx, qword ptr [rsp + 0x30]
00000001803A8F6B call      0x180302170
00000001803A8F70 mov       rcx, qword ptr [rsp + 0x28]
00000001803A8F75 test      rcx, rcx
00000001803A8F78 jne       0x1803a8fb3
00000001803A8F7A mov       rbx, qword ptr [rsp + 0x70]
00000001803A8F7F mov       rsi, qword ptr [rsp + 0x78]
00000001803A8F84 mov       rdi, qword ptr [rsp + 0x80]
00000001803A8F8C add       rsp, 0x60
00000001803A8F90 pop       r14
00000001803A8F92 ret       
00000001803A8F93 call      0x180250150
00000001803A8F98 nop       
00000001803A8F99 call      0x180250150
00000001803A8F9E call      0x180250150
00000001803A8FA3 call      0x180250150
00000001803A8FA8 call      0x180250150
00000001803A8FAD call      0x180250150
00000001803A8FB2 nop       
00000001803A8FB3 call      0x180200e60
00000001803A8FB8 int3      
00000001803A8FB9 call      0x180250150
00000001803A8FBE int3      
00000001803A8FBF int3      