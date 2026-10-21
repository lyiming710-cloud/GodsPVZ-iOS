000000018039E790 mov       qword ptr [rsp + 0x10], rbx
000000018039E795 push      rdi
000000018039E796 sub       rsp, 0x20
000000018039E79A cmp       byte ptr [rip + 0x1946715], 0 # 0x181ce4eb6
000000018039E7A1 mov       rdi, rdx
000000018039E7A4 mov       rbx, rcx
000000018039E7A7 jne       0x18039e7ec
000000018039E7A9 lea       rcx, [rip + 0x181d088] # 0x181bbb838
000000018039E7B0 call      0x18024fef0
000000018039E7B5 lea       rcx, [rip + 0x181d0d4] # 0x181bbb890
000000018039E7BC call      0x18024fef0
000000018039E7C1 lea       rcx, [rip + 0x181bb78] # 0x181bba340
000000018039E7C8 call      0x18024fef0
000000018039E7CD lea       rcx, [rip + 0x1817ca4] # 0x181bb6478
000000018039E7D4 call      0x18024fef0
000000018039E7D9 lea       rcx, [rip + 0x18308a8] # 0x181bcf088
000000018039E7E0 call      0x18024fef0
000000018039E7E5 mov       byte ptr [rip + 0x19466ca], 1 # 0x181ce4eb6
000000018039E7EC mov       ecx, dword ptr [rbx + 0x68]
000000018039E7EF mov       edx, 0xf423f
000000018039E7F4 sub       ecx, 1
000000018039E7F7 je        0x18039e80f
000000018039E7F9 sub       ecx, 1
000000018039E7FC je        0x18039e80f
000000018039E7FE sub       ecx, 1
000000018039E801 je        0x18039e808
000000018039E803 cmp       ecx, 1
000000018039E806 jne       0x18039e814
000000018039E808 mov       edx, 0x29
000000018039E80D jmp       0x18039e814
000000018039E80F mov       edx, 0x15
000000018039E814 mov       qword ptr [rsp + 0x30], rsi
000000018039E819 test      rdi, rdi
000000018039E81C je        0x18039ebbd
000000018039E822 mov       rcx, qword ptr [rbx + 0x28]
000000018039E826 cmp       dword ptr [rdi + 0x20], edx
000000018039E829 jge       0x18039e9b4
000000018039E82F test      rcx, rcx
000000018039E832 je        0x18039ebbd
000000018039E838 xor       edx, edx
000000018039E83A call      0x1813044d0
000000018039E83F test      rax, rax
000000018039E842 je        0x18039ebbd
000000018039E848 xor       r8d, r8d
000000018039E84B mov       dl, 1
000000018039E84D mov       rcx, rax
000000018039E850 call      0x18131bad0
000000018039E855 mov       rcx, qword ptr [rbx + 0x28]
000000018039E859 test      rcx, rcx
000000018039E85C je        0x18039ebbd
000000018039E862 mov       rdx, qword ptr [rip + 0x183081f] # 0x181bcf088
000000018039E869 xor       r9d, r9d
000000018039E86C mov       r8b, 1
000000018039E86F call      0x1811af070
000000018039E874 mov       rcx, qword ptr [rbx + 0x50]
000000018039E878 test      rcx, rcx
000000018039E87B je        0x18039ebbd
000000018039E881 xor       edx, edx
000000018039E883 call      0x1813044d0
000000018039E888 test      rax, rax
000000018039E88B je        0x18039ebbd
000000018039E891 xor       r8d, r8d
000000018039E894 mov       dl, 1
000000018039E896 mov       rcx, rax
000000018039E899 call      0x18131bad0
000000018039E89E mov       rcx, qword ptr [rbx + 0x30]
000000018039E8A2 test      rcx, rcx
000000018039E8A5 je        0x18039ebbd
000000018039E8AB xor       r8d, r8d
000000018039E8AE xor       edx, edx
000000018039E8B0 call      0x18156cc70
000000018039E8B5 mov       rax, qword ptr [rip + 0x1817bbc] # 0x181bb6478
000000018039E8BC mov       rdi, qword ptr [rbx + 0x38]
000000018039E8C0 cmp       dword ptr [rax + 0xe0], 0
000000018039E8C7 jne       0x18039e8d8
000000018039E8C9 mov       rcx, rax
000000018039E8CC call      0x1802501e0
000000018039E8D1 mov       rax, qword ptr [rip + 0x1817ba0] # 0x181bb6478
000000018039E8D8 mov       rax, qword ptr [rax + 0xb8]
000000018039E8DF mov       rcx, qword ptr [rax + 0xa8]
000000018039E8E6 test      rcx, rcx
000000018039E8E9 je        0x18039ebbd
000000018039E8EF mov       r8, qword ptr [rip + 0x181ba4a] # 0x181bba340
000000018039E8F6 xor       edx, edx
000000018039E8F8 call      0x180825710
000000018039E8FD test      rdi, rdi
000000018039E900 je        0x18039ebbd
000000018039E906 xor       r8d, r8d
000000018039E909 mov       rdx, rax
000000018039E90C mov       rcx, rdi
000000018039E90F call      0x1813d7ab0
000000018039E914 mov       rax, qword ptr [rip + 0x1817b5d] # 0x181bb6478
000000018039E91B mov       rdi, qword ptr [rbx + 0x40]
000000018039E91F mov       rcx, qword ptr [rax + 0xb8]
000000018039E926 mov       rcx, qword ptr [rcx + 0xb0]
000000018039E92D test      rcx, rcx
000000018039E930 je        0x18039ebbd
000000018039E936 mov       r8, qword ptr [rip + 0x181ba03] # 0x181bba340
000000018039E93D xor       edx, edx
000000018039E93F call      0x180825710
000000018039E944 test      rdi, rdi
000000018039E947 je        0x18039ebbd
000000018039E94D xor       r8d, r8d
000000018039E950 mov       rdx, rax
000000018039E953 mov       rcx, rdi
000000018039E956 call      0x1813d7ab0
000000018039E95B mov       rax, qword ptr [rip + 0x1817b16] # 0x181bb6478
000000018039E962 mov       rbx, qword ptr [rbx + 0x48]
000000018039E966 mov       rcx, qword ptr [rax + 0xb8]
000000018039E96D mov       rcx, qword ptr [rcx + 0xb8]
000000018039E974 test      rcx, rcx
000000018039E977 je        0x18039ebbd
000000018039E97D mov       r8, qword ptr [rip + 0x181b9bc] # 0x181bba340
000000018039E984 xor       edx, edx
000000018039E986 call      0x180825710
000000018039E98B test      rbx, rbx
000000018039E98E je        0x18039ebbd
000000018039E994 xor       r8d, r8d
000000018039E997 mov       rdx, rax
000000018039E99A mov       rcx, rbx
000000018039E99D call      0x1813d7ab0
000000018039E9A2 mov       al, 1
000000018039E9A4 mov       rsi, qword ptr [rsp + 0x30]
000000018039E9A9 mov       rbx, qword ptr [rsp + 0x38]
000000018039E9AE add       rsp, 0x20
000000018039E9B2 pop       rdi
000000018039E9B3 ret       
000000018039E9B4 test      rcx, rcx
000000018039E9B7 je        0x18039ebbd
000000018039E9BD xor       edx, edx
000000018039E9BF call      0x1813044d0
000000018039E9C4 test      rax, rax
000000018039E9C7 je        0x18039ebbd
000000018039E9CD xor       r8d, r8d
000000018039E9D0 mov       dl, 1
000000018039E9D2 mov       rcx, rax
000000018039E9D5 call      0x18131bad0
000000018039E9DA mov       rcx, qword ptr [rbx + 0x20]
000000018039E9DE mov       rdx, qword ptr [rip + 0x18306a3] # 0x181bcf088
000000018039E9E5 test      rcx, rcx
000000018039E9E8 je        0x18039ebbd
000000018039E9EE mov       rcx, qword ptr [rcx + 0x20]
000000018039E9F2 test      rcx, rcx
000000018039E9F5 je        0x18039ebbd
000000018039E9FB mov       eax, dword ptr [rbx + 0x68]
000000018039E9FE cmp       dword ptr [rcx + 0x18], eax
000000018039EA01 jle       0x18039ea1d
000000018039EA03 mov       rax, qword ptr [rbx + 0x20]
000000018039EA07 mov       r8, qword ptr [rip + 0x181ce82] # 0x181bbb890
000000018039EA0E mov       edx, dword ptr [rbx + 0x68]
000000018039EA11 mov       rcx, qword ptr [rax + 0x20]
000000018039EA15 call      0x180825710
000000018039EA1A mov       rdx, rax
000000018039EA1D mov       rcx, qword ptr [rbx + 0x28]
000000018039EA21 test      rcx, rcx
000000018039EA24 je        0x18039ebbd
000000018039EA2A xor       r9d, r9d
000000018039EA2D mov       r8b, 1
000000018039EA30 call      0x1811af070
000000018039EA35 mov       rcx, qword ptr [rbx + 0x50]
000000018039EA39 test      rcx, rcx
000000018039EA3C je        0x18039ebbd
000000018039EA42 xor       edx, edx
000000018039EA44 call      0x1813044d0
000000018039EA49 test      rax, rax
000000018039EA4C je        0x18039ebbd
000000018039EA52 xor       r8d, r8d
000000018039EA55 xor       edx, edx
000000018039EA57 mov       rcx, rax
000000018039EA5A call      0x18131bad0
000000018039EA5F mov       rcx, qword ptr [rbx + 0x30]
000000018039EA63 test      rcx, rcx
000000018039EA66 je        0x18039ebbd
000000018039EA6C xor       r8d, r8d
000000018039EA6F mov       dl, 1
000000018039EA71 call      0x18156cc70
000000018039EA76 cmp       byte ptr [rip + 0x1946436], 0 # 0x181ce4eb3
000000018039EA7D mov       edi, dword ptr [rbx + 0x68]
000000018039EA80 jne       0x18039eaa1
000000018039EA82 lea       rcx, [rip + 0x181b8b7] # 0x181bba340
000000018039EA89 call      0x18024fef0
000000018039EA8E lea       rcx, [rip + 0x18179e3] # 0x181bb6478
000000018039EA95 call      0x18024fef0
000000018039EA9A mov       byte ptr [rip + 0x1946412], 1 # 0x181ce4eb3
000000018039EAA1 mov       rsi, qword ptr [rbx + 0x38]
000000018039EAA5 mov       dword ptr [rbx + 0x68], edi
000000018039EAA8 mov       rax, qword ptr [rip + 0x18179c9] # 0x181bb6478
000000018039EAAF cmp       dword ptr [rax + 0xe0], 0
000000018039EAB6 jne       0x18039eac7
000000018039EAB8 mov       rcx, rax
000000018039EABB call      0x1802501e0
000000018039EAC0 mov       rax, qword ptr [rip + 0x18179b1] # 0x181bb6478
000000018039EAC7 mov       rax, qword ptr [rax + 0xb8]
000000018039EACE mov       rcx, qword ptr [rax + 0xa8]
000000018039EAD5 test      rcx, rcx
000000018039EAD8 je        0x18039ebbd
000000018039EADE mov       r8, qword ptr [rip + 0x181b85b] # 0x181bba340
000000018039EAE5 mov       edx, edi
000000018039EAE7 call      0x180825710
000000018039EAEC test      rsi, rsi
000000018039EAEF je        0x18039ebbd
000000018039EAF5 xor       r8d, r8d
000000018039EAF8 mov       rdx, rax
000000018039EAFB mov       rcx, rsi
000000018039EAFE call      0x1813d7ab0
000000018039EB03 mov       rax, qword ptr [rip + 0x181796e] # 0x181bb6478
000000018039EB0A mov       rsi, qword ptr [rbx + 0x40]
000000018039EB0E mov       rcx, qword ptr [rax + 0xb8]
000000018039EB15 mov       rcx, qword ptr [rcx + 0xb0]
000000018039EB1C test      rcx, rcx
000000018039EB1F je        0x18039ebbd
000000018039EB25 mov       r8, qword ptr [rip + 0x181b814] # 0x181bba340
000000018039EB2C mov       edx, edi
000000018039EB2E call      0x180825710
000000018039EB33 test      rsi, rsi
000000018039EB36 je        0x18039ebbd
000000018039EB3C xor       r8d, r8d
000000018039EB3F mov       rdx, rax
000000018039EB42 mov       rcx, rsi
000000018039EB45 call      0x1813d7ab0
000000018039EB4A mov       rax, qword ptr [rip + 0x1817927] # 0x181bb6478
000000018039EB51 mov       rsi, qword ptr [rbx + 0x48]
000000018039EB55 mov       rcx, qword ptr [rax + 0xb8]
000000018039EB5C mov       rcx, qword ptr [rcx + 0xb8]
000000018039EB63 test      rcx, rcx
000000018039EB66 je        0x18039ebbd
000000018039EB68 mov       r8, qword ptr [rip + 0x181b7d1] # 0x181bba340
000000018039EB6F mov       edx, edi
000000018039EB71 call      0x180825710
000000018039EB76 test      rsi, rsi
000000018039EB79 je        0x18039ebbd
000000018039EB7B xor       r8d, r8d
000000018039EB7E mov       rdx, rax
000000018039EB81 mov       rcx, rsi
000000018039EB84 call      0x1813d7ab0
000000018039EB89 mov       rcx, qword ptr [rbx + 0x58]
000000018039EB8D test      rcx, rcx
000000018039EB90 je        0x18039ebbd
000000018039EB92 xor       edx, edx
000000018039EB94 call      0x1813044d0
000000018039EB99 test      rax, rax
000000018039EB9C je        0x18039ebbd
000000018039EB9E xor       r8d, r8d
000000018039EBA1 xor       edx, edx
000000018039EBA3 mov       rcx, rax
000000018039EBA6 call      0x18131bad0
000000018039EBAB mov       rsi, qword ptr [rsp + 0x30]
000000018039EBB0 xor       al, al
000000018039EBB2 mov       rbx, qword ptr [rsp + 0x38]
000000018039EBB7 add       rsp, 0x20
000000018039EBBB pop       rdi
000000018039EBBC ret       
000000018039EBBD call      0x180250150
000000018039EBC2 int3      
000000018039EBC3 int3      
000000018039EBC4 int3      
000000018039EBC5 int3      
000000018039EBC6 int3      
000000018039EBC7 int3      
000000018039EBC8 int3      
000000018039EBC9 int3      
000000018039EBCA int3      
000000018039EBCB int3      
000000018039EBCC int3      
000000018039EBCD int3      
000000018039EBCE int3      
000000018039EBCF int3      