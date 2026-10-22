000000018030C820 push      rbx
000000018030C822 sub       rsp, 0x40
000000018030C826 cmp       byte ptr [rip + 0x19d82c0], 0 # 0x181ce4aed
000000018030C82D mov       rbx, rcx
000000018030C830 jne       0x18030c875
000000018030C832 lea       rcx, [rip + 0x18c6ac7] # 0x181bd3300
000000018030C839 call      0x18024fef0
000000018030C83E lea       rcx, [rip + 0x18ab953] # 0x181bb8198
000000018030C845 call      0x18024fef0
000000018030C84A lea       rcx, [rip + 0x18b24b7] # 0x181bbed08
000000018030C851 call      0x18024fef0
000000018030C856 lea       rcx, [rip + 0x1894953] # 0x181ba11b0
000000018030C85D call      0x18024fef0
000000018030C862 lea       rcx, [rip + 0x18949a7] # 0x181ba1210
000000018030C869 call      0x18024fef0
000000018030C86E mov       byte ptr [rip + 0x19d8278], 1 # 0x181ce4aed
000000018030C875 mov       rcx, qword ptr [rbx + 0x90]
000000018030C87C mov       qword ptr [rsp + 0x58], rdi
000000018030C881 test      rcx, rcx
000000018030C884 je        0x18030cbb0
000000018030C88A xor       r8d, r8d
000000018030C88D mov       dl, 1
000000018030C88F call      0x18156cc70
000000018030C894 mov       rcx, qword ptr [rbx + 0x88]
000000018030C89B test      rcx, rcx
000000018030C89E je        0x18030cbb0
000000018030C8A4 cmp       dword ptr [rbx + 0x2c], 0
000000018030C8A8 setg      dl
000000018030C8AB xor       r8d, r8d
000000018030C8AE call      0x18156cc70
000000018030C8B3 mov       rdx, qword ptr [rbx + 0x20]
000000018030C8B7 test      rdx, rdx
000000018030C8BA je        0x18030cbb0
000000018030C8C0 mov       rdx, qword ptr [rdx + 0x30]
000000018030C8C4 test      rdx, rdx
000000018030C8C7 je        0x18030cbb0
000000018030C8CD mov       rcx, qword ptr [rbx + 0x38]
000000018030C8D1 test      rcx, rcx
000000018030C8D4 je        0x18030cbb0
000000018030C8DA mov       edx, dword ptr [rdx + 0x18]
000000018030C8DD xor       r8d, r8d
000000018030C8E0 call      0x181180950
000000018030C8E5 mov       rcx, qword ptr [rbx + 0x30]
000000018030C8E9 test      rcx, rcx
000000018030C8EC je        0x18030cbb0
000000018030C8F2 mov       r8, qword ptr [rip + 0x18b240f] # 0x181bbed08
000000018030C8F9 xor       edx, edx
000000018030C8FB call      0x180825710
000000018030C900 mov       rcx, qword ptr [rip + 0x18c69f9] # 0x181bd3300
000000018030C907 xor       edx, edx
000000018030C909 mov       qword ptr [rsp + 0x20], rcx
000000018030C90E mov       rdi, rax
000000018030C911 mov       ecx, dword ptr [rbx + 0x28]
000000018030C914 mov       dword ptr [rsp + 0x30], ecx
000000018030C918 lea       rcx, [rsp + 0x20]
000000018030C91D mov       qword ptr [rsp + 0x28], 0xffffffffffffffff
000000018030C926 call      0x180ce0770
000000018030C92B mov       rcx, qword ptr [rip + 0x18948de] # 0x181ba1210
000000018030C932 xor       r8d, r8d
000000018030C935 mov       rdx, rax
000000018030C938 call      0x180b74270
000000018030C93D test      rdi, rdi
000000018030C940 je        0x18030cbb0
000000018030C946 mov       r9, qword ptr [rdi]
000000018030C949 mov       rdx, rax
000000018030C94C mov       rcx, rdi
000000018030C94F mov       r8, qword ptr [r9 + 0x560]
000000018030C956 call      qword ptr [r9 + 0x558]
000000018030C95D mov       rcx, qword ptr [rbx + 0x30]
000000018030C961 test      rcx, rcx
000000018030C964 je        0x18030cbb0
000000018030C96A mov       r8, qword ptr [rip + 0x18b2397] # 0x181bbed08
000000018030C971 mov       edx, 1
000000018030C976 call      0x180825710
000000018030C97B lea       rcx, [rbx + 0x2c]
000000018030C97F xor       edx, edx
000000018030C981 mov       rdi, rax
000000018030C984 call      0x180ca3820
000000018030C989 mov       rcx, qword ptr [rip + 0x1894820] # 0x181ba11b0
000000018030C990 xor       r8d, r8d
000000018030C993 mov       rdx, rax
000000018030C996 call      0x180b74270
000000018030C99B test      rdi, rdi
000000018030C99E je        0x18030cbb0
000000018030C9A4 mov       r9, qword ptr [rdi]
000000018030C9A7 mov       rdx, rax
000000018030C9AA mov       rcx, rdi
000000018030C9AD mov       r8, qword ptr [r9 + 0x560]
000000018030C9B4 call      qword ptr [r9 + 0x558]
000000018030C9BB mov       rcx, qword ptr [rbx + 0x20]
000000018030C9BF mov       rdi, qword ptr [rbx + 0x50]
000000018030C9C3 test      rcx, rcx
000000018030C9C6 je        0x18030cbb0
000000018030C9CC mov       rcx, qword ptr [rcx + 0x30]
000000018030C9D0 test      rcx, rcx
000000018030C9D3 je        0x18030cbb0
000000018030C9D9 xor       edx, edx
000000018030C9DB call      0x18032c3b0
000000018030C9E0 xor       edx, edx
000000018030C9E2 mov       dword ptr [rsp + 0x50], eax
000000018030C9E6 lea       rcx, [rsp + 0x50]
000000018030C9EB call      0x180ca3820
000000018030C9F0 test      rdi, rdi
000000018030C9F3 je        0x18030cbb0
000000018030C9F9 xor       r8d, r8d
000000018030C9FC mov       rdx, rax
000000018030C9FF mov       rcx, rdi
000000018030CA02 call      0x18119bae0
000000018030CA07 mov       rax, qword ptr [rbx + 0x20]
000000018030CA0B mov       rdi, qword ptr [rbx + 0x58]
000000018030CA0F test      rax, rax
000000018030CA12 je        0x18030cbb0
000000018030CA18 mov       rax, qword ptr [rax + 0x30]
000000018030CA1C test      rax, rax
000000018030CA1F je        0x18030cbb0
000000018030CA25 mov       rax, qword ptr [rax + 0x10]
000000018030CA29 test      rax, rax
000000018030CA2C je        0x18030cbb0
000000018030CA32 mov       eax, dword ptr [rax + 0x18]
000000018030CA35 lea       rcx, [rsp + 0x50]
000000018030CA3A xor       edx, edx
000000018030CA3C mov       dword ptr [rsp + 0x50], eax
000000018030CA40 call      0x180ca3820
000000018030CA45 test      rdi, rdi
000000018030CA48 je        0x18030cbb0
000000018030CA4E xor       r8d, r8d
000000018030CA51 mov       rdx, rax
000000018030CA54 mov       rcx, rdi
000000018030CA57 call      0x18119bae0
000000018030CA5C mov       rcx, qword ptr [rbx + 0x20]
000000018030CA60 mov       rdi, qword ptr [rbx + 0x60]
000000018030CA64 test      rcx, rcx
000000018030CA67 je        0x18030cbb0
000000018030CA6D mov       rcx, qword ptr [rcx + 0x30]
000000018030CA71 test      rcx, rcx
000000018030CA74 je        0x18030cbb0
000000018030CA7A add       rcx, 0x1c
000000018030CA7E xor       edx, edx
000000018030CA80 call      0x180cc5b50
000000018030CA85 test      rdi, rdi
000000018030CA88 je        0x18030cbb0
000000018030CA8E xor       r8d, r8d
000000018030CA91 mov       rdx, rax
000000018030CA94 mov       rcx, rdi
000000018030CA97 call      0x18119bae0
000000018030CA9C mov       rcx, qword ptr [rbx + 0x20]
000000018030CAA0 mov       rdi, qword ptr [rbx + 0x68]
000000018030CAA4 test      rcx, rcx
000000018030CAA7 je        0x18030cbb0
000000018030CAAD add       rcx, 0x38
000000018030CAB1 xor       edx, edx
000000018030CAB3 call      0x180ca3820
000000018030CAB8 test      rdi, rdi
000000018030CABB je        0x18030cbb0
000000018030CAC1 xor       r8d, r8d
000000018030CAC4 mov       rdx, rax
000000018030CAC7 mov       rcx, rdi
000000018030CACA call      0x18119bae0
000000018030CACF mov       rcx, qword ptr [rbx + 0x20]
000000018030CAD3 mov       rdi, qword ptr [rbx + 0x70]
000000018030CAD7 test      rcx, rcx
000000018030CADA je        0x18030cbb0
000000018030CAE0 add       rcx, 0x3c
000000018030CAE4 xor       edx, edx
000000018030CAE6 call      0x180ca3820
000000018030CAEB test      rdi, rdi
000000018030CAEE je        0x18030cbb0
000000018030CAF4 xor       r8d, r8d
000000018030CAF7 mov       rdx, rax
000000018030CAFA mov       rcx, rdi
000000018030CAFD call      0x18119bae0
000000018030CB02 mov       rcx, qword ptr [rbx + 0x20]
000000018030CB06 mov       rdi, qword ptr [rbx + 0x78]
000000018030CB0A test      rcx, rcx
000000018030CB0D je        0x18030cbb0
000000018030CB13 add       rcx, 0x28
000000018030CB17 xor       edx, edx
000000018030CB19 call      0x180ca3820
000000018030CB1E test      rdi, rdi
000000018030CB21 je        0x18030cbb0
000000018030CB27 xor       r8d, r8d
000000018030CB2A mov       rdx, rax
000000018030CB2D mov       rcx, rdi
000000018030CB30 call      0x18119bae0
000000018030CB35 mov       rcx, qword ptr [rbx + 0x20]
000000018030CB39 mov       rdi, qword ptr [rbx + 0x80]
000000018030CB40 test      rcx, rcx
000000018030CB43 je        0x18030cbb0
000000018030CB45 add       rcx, 0x40
000000018030CB49 xor       edx, edx
000000018030CB4B call      0x180ca3820
000000018030CB50 test      rdi, rdi
000000018030CB53 je        0x18030cbb0
000000018030CB55 xor       r8d, r8d
000000018030CB58 mov       rdx, rax
000000018030CB5B mov       rcx, rdi
000000018030CB5E call      0x18119bae0
000000018030CB63 xor       edx, edx
000000018030CB65 mov       rcx, rbx
000000018030CB68 call      0x18030b890
000000018030CB6D xor       edx, edx
000000018030CB6F mov       rcx, rbx
000000018030CB72 call      0x18030b130
000000018030CB77 mov       rcx, qword ptr [rbx + 0x98]
000000018030CB7E test      rcx, rcx
000000018030CB81 je        0x18030cbb0
000000018030CB83 xor       r8d, r8d
000000018030CB86 xor       edx, edx
000000018030CB88 call      0x18131bad0
000000018030CB8D mov       rcx, qword ptr [rbx + 0xb8]
000000018030CB94 test      rcx, rcx
000000018030CB97 je        0x18030cbb0
000000018030CB99 mov       rdx, qword ptr [rbx + 0x20]
000000018030CB9D xor       r8d, r8d
000000018030CBA0 call      0x1802fca30
000000018030CBA5 mov       rdi, qword ptr [rsp + 0x58]
000000018030CBAA add       rsp, 0x40
000000018030CBAE pop       rbx
000000018030CBAF ret       
000000018030CBB0 call      0x180250150
000000018030CBB5 int3      
000000018030CBB6 int3      
000000018030CBB7 int3      
000000018030CBB8 int3      
000000018030CBB9 int3      
000000018030CBBA int3      
000000018030CBBB int3      
000000018030CBBC int3      
000000018030CBBD int3      
000000018030CBBE int3      
000000018030CBBF int3      