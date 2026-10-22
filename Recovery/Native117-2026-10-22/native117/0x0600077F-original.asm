00000001803C1EA0 mov       qword ptr [rsp + 0x10], rbx
00000001803C1EA5 push      rdi
00000001803C1EA6 sub       rsp, 0x20
00000001803C1EAA cmp       byte ptr [rip + 0x19230c2], 0 # 0x181ce4f73
00000001803C1EB1 mov       rdi, rcx
00000001803C1EB4 jne       0x1803c1ec9
00000001803C1EB6 lea       rcx, [rip + 0x1801b03] # 0x181bc39c0
00000001803C1EBD call      0x18024fef0
00000001803C1EC2 mov       byte ptr [rip + 0x19230aa], 1 # 0x181ce4f73
00000001803C1EC9 mov       ecx, dword ptr [rdi + 0x10]
00000001803C1ECC mov       rbx, qword ptr [rdi + 0x20]
00000001803C1ED0 mov       qword ptr [rsp + 0x30], rsi
00000001803C1ED5 test      ecx, ecx
00000001803C1ED7 je        0x1803c202d
00000001803C1EDD sub       ecx, 1
00000001803C1EE0 je        0x1803c1fb1
00000001803C1EE6 sub       ecx, 1
00000001803C1EE9 je        0x1803c1f35
00000001803C1EEB sub       ecx, 1
00000001803C1EEE je        0x1803c1f29
00000001803C1EF0 cmp       ecx, 1
00000001803C1EF3 je        0x1803c1f07
00000001803C1EF5 xor       al, al
00000001803C1EF7 mov       rsi, qword ptr [rsp + 0x30]
00000001803C1EFC mov       rbx, qword ptr [rsp + 0x38]
00000001803C1F01 add       rsp, 0x20
00000001803C1F05 pop       rdi
00000001803C1F06 ret       
00000001803C1F07 mov       dword ptr [rdi + 0x10], 0xffffffff
00000001803C1F0E mov       ecx, dword ptr [rdi + 0x28]
00000001803C1F11 mov       eax, dword ptr [rdi + 0x2c]
00000001803C1F14 inc       ecx
00000001803C1F16 cdq       
00000001803C1F17 idiv      ecx
00000001803C1F19 mov       esi, edx
00000001803C1F1B test      rbx, rbx
00000001803C1F1E je        0x1803c2149
00000001803C1F24 jmp       0x1803c2094
00000001803C1F29 mov       dword ptr [rdi + 0x10], 0xffffffff
00000001803C1F30 jmp       0x1803c20b0
00000001803C1F35 mov       dword ptr [rdi + 0x10], 0xffffffff
00000001803C1F3C test      rbx, rbx
00000001803C1F3F je        0x1803c2149
00000001803C1F45 mov       rcx, qword ptr [rbx + 0x30]
00000001803C1F49 test      rcx, rcx
00000001803C1F4C je        0x1803c2149
00000001803C1F52 mov       rax, qword ptr [rcx]
00000001803C1F55 mov       rdx, qword ptr [rbx + 0x20]
00000001803C1F59 mov       r8, qword ptr [rax + 0x560]
00000001803C1F60 call      qword ptr [rax + 0x558]
00000001803C1F66 mov       rcx, qword ptr [rip + 0x1801a53] # 0x181bc39c0
00000001803C1F6D call      0x180250100
00000001803C1F72 movss     xmm1, dword ptr [rip + 0x11e5a96] # 0x1815a7a10
00000001803C1F7A xor       r8d, r8d
00000001803C1F7D mov       rcx, rax
00000001803C1F80 mov       rbx, rax
00000001803C1F83 call      0x181333710
00000001803C1F88 lea       rcx, [rdi + 0x18]
00000001803C1F8C mov       qword ptr [rdi + 0x18], rbx
00000001803C1F90 mov       rdx, rbx
00000001803C1F93 call      0x18024f360
00000001803C1F98 mov       al, 1
00000001803C1F9A mov       dword ptr [rdi + 0x10], 3
00000001803C1FA1 mov       rsi, qword ptr [rsp + 0x30]
00000001803C1FA6 mov       rbx, qword ptr [rsp + 0x38]
00000001803C1FAB add       rsp, 0x20
00000001803C1FAF pop       rdi
00000001803C1FB0 ret       
00000001803C1FB1 mov       dword ptr [rdi + 0x10], 0xffffffff
00000001803C1FB8 test      rbx, rbx
00000001803C1FBB je        0x1803c2149
00000001803C1FC1 mov       rcx, qword ptr [rbx + 0x30]
00000001803C1FC5 test      rcx, rcx
00000001803C1FC8 je        0x1803c2149
00000001803C1FCE mov       rax, qword ptr [rcx]
00000001803C1FD1 mov       rdx, qword ptr [rbx + 0x28]
00000001803C1FD5 mov       r8, qword ptr [rax + 0x560]
00000001803C1FDC call      qword ptr [rax + 0x558]
00000001803C1FE2 mov       rcx, qword ptr [rip + 0x18019d7] # 0x181bc39c0
00000001803C1FE9 call      0x180250100
00000001803C1FEE movss     xmm1, dword ptr [rip + 0x11e5a1a] # 0x1815a7a10
00000001803C1FF6 xor       r8d, r8d
00000001803C1FF9 mov       rcx, rax
00000001803C1FFC mov       rbx, rax
00000001803C1FFF call      0x181333710
00000001803C2004 lea       rcx, [rdi + 0x18]
00000001803C2008 mov       qword ptr [rdi + 0x18], rbx
00000001803C200C mov       rdx, rbx
00000001803C200F call      0x18024f360
00000001803C2014 mov       al, 1
00000001803C2016 mov       dword ptr [rdi + 0x10], 2
00000001803C201D mov       rsi, qword ptr [rsp + 0x30]
00000001803C2022 mov       rbx, qword ptr [rsp + 0x38]
00000001803C2027 add       rsp, 0x20
00000001803C202B pop       rdi
00000001803C202C ret       
00000001803C202D mov       dword ptr [rdi + 0x10], 0xffffffff
00000001803C2034 test      rbx, rbx
00000001803C2037 je        0x1803c2149
00000001803C203D mov       rcx, qword ptr [rbx + 0x30]
00000001803C2041 test      rcx, rcx
00000001803C2044 je        0x1803c2149
00000001803C204A mov       rax, qword ptr [rcx]
00000001803C204D xor       r8d, r8d
00000001803C2050 xor       edx, edx
00000001803C2052 mov       r9, qword ptr [rax + 0x7e0]
00000001803C2059 call      qword ptr [rax + 0x7d8]
00000001803C205F mov       rax, qword ptr [rbx + 0x30]
00000001803C2063 test      rax, rax
00000001803C2066 je        0x1803c2149
00000001803C206C mov       rax, qword ptr [rax + 0x370]
00000001803C2073 test      rax, rax
00000001803C2076 je        0x1803c2149
00000001803C207C mov       eax, dword ptr [rax + 0x18]
00000001803C207F mov       dword ptr [rdi + 0x28], eax
00000001803C2082 mov       dword ptr [rdi + 0x2c], 0
00000001803C2089 lea       ecx, [rax + 1]
00000001803C208C mov       eax, dword ptr [rdi + 0x2c]
00000001803C208F cdq       
00000001803C2090 idiv      ecx
00000001803C2092 mov       esi, edx
00000001803C2094 mov       rcx, qword ptr [rbx + 0x30]
00000001803C2098 test      rcx, rcx
00000001803C209B je        0x1803c2149
00000001803C20A1 xor       r8d, r8d
00000001803C20A4 mov       edx, esi
00000001803C20A6 call      0x1811b9b50
00000001803C20AB cmp       esi, dword ptr [rdi + 0x28]
00000001803C20AE jge       0x1803c20fe
00000001803C20B0 inc       dword ptr [rdi + 0x2c]
00000001803C20B3 mov       rcx, qword ptr [rip + 0x1801906] # 0x181bc39c0
00000001803C20BA call      0x180250100
00000001803C20BF movss     xmm1, dword ptr [rip + 0x11e59dd] # 0x1815a7aa4
00000001803C20C7 xor       r8d, r8d
00000001803C20CA mov       rcx, rax
00000001803C20CD mov       rbx, rax
00000001803C20D0 call      0x181333710
00000001803C20D5 lea       rcx, [rdi + 0x18]
00000001803C20D9 mov       qword ptr [rdi + 0x18], rbx
00000001803C20DD mov       rdx, rbx
00000001803C20E0 call      0x18024f360
00000001803C20E5 mov       al, 1
00000001803C20E7 mov       dword ptr [rdi + 0x10], 4
00000001803C20EE mov       rsi, qword ptr [rsp + 0x30]
00000001803C20F3 mov       rbx, qword ptr [rsp + 0x38]
00000001803C20F8 add       rsp, 0x20
00000001803C20FC pop       rdi
00000001803C20FD ret       
00000001803C20FE mov       rcx, qword ptr [rip + 0x18018bb] # 0x181bc39c0
00000001803C2105 call      0x180250100
00000001803C210A movss     xmm1, dword ptr [rip + 0x11e58fe] # 0x1815a7a10
00000001803C2112 xor       r8d, r8d
00000001803C2115 mov       rcx, rax
00000001803C2118 mov       rbx, rax
00000001803C211B call      0x181333710
00000001803C2120 lea       rcx, [rdi + 0x18]
00000001803C2124 mov       qword ptr [rdi + 0x18], rbx
00000001803C2128 mov       rdx, rbx
00000001803C212B call      0x18024f360
00000001803C2130 mov       rsi, qword ptr [rsp + 0x30]
00000001803C2135 mov       al, 1
00000001803C2137 mov       rbx, qword ptr [rsp + 0x38]
00000001803C213C mov       dword ptr [rdi + 0x10], 1
00000001803C2143 add       rsp, 0x20
00000001803C2147 pop       rdi
00000001803C2148 ret       
00000001803C2149 call      0x180250150
00000001803C214E int3      
00000001803C214F int3      