00000001803AEB70 mov       qword ptr [rsp + 0x10], rbx
00000001803AEB75 mov       qword ptr [rsp + 0x20], rsi
00000001803AEB7A mov       qword ptr [rsp + 8], rcx
00000001803AEB7F push      rdi
00000001803AEB80 sub       rsp, 0x60
00000001803AEB84 mov       rdi, rcx
00000001803AEB87 cmp       byte ptr [rip + 0x19363a1], 0 # 0x181ce4f2f
00000001803AEB8E jne       0x1803aec1f
00000001803AEB94 lea       rcx, [rip + 0x1818ee5] # 0x181bc7a80
00000001803AEB9B call      0x18024fef0
00000001803AEBA0 lea       rcx, [rip + 0x1818f31] # 0x181bc7ad8
00000001803AEBA7 call      0x18024fef0
00000001803AEBAC lea       rcx, [rip + 0x1818f7d] # 0x181bc7b30
00000001803AEBB3 call      0x18024fef0
00000001803AEBB8 lea       rcx, [rip + 0x17f3459] # 0x181ba2018
00000001803AEBBF call      0x18024fef0
00000001803AEBC4 lea       rcx, [rip + 0x18098e5] # 0x181bb84b0
00000001803AEBCB call      0x18024fef0
00000001803AEBD0 lea       rcx, [rip + 0x17f56a1] # 0x181ba4278
00000001803AEBD7 call      0x18024fef0
00000001803AEBDC lea       rcx, [rip + 0x180191d] # 0x181bb0500
00000001803AEBE3 call      0x18024fef0
00000001803AEBE8 lea       rcx, [rip + 0x1807889] # 0x181bb6478
00000001803AEBEF call      0x18024fef0
00000001803AEBF4 lea       rcx, [rip + 0x180c0fd] # 0x181bbacf8
00000001803AEBFB call      0x18024fef0
00000001803AEC00 lea       rcx, [rip + 0x17f7941] # 0x181ba6548
00000001803AEC07 call      0x18024fef0
00000001803AEC0C lea       rcx, [rip + 0x17f56b5] # 0x181ba42c8
00000001803AEC13 call      0x18024fef0
00000001803AEC18 mov       byte ptr [rip + 0x1936310], 1 # 0x181ce4f2f
00000001803AEC1F mov       rax, qword ptr [rip + 0x17f33f2] # 0x181ba2018
00000001803AEC26 cmp       dword ptr [rax + 0xe0], 0
00000001803AEC2D jne       0x1803aec3e
00000001803AEC2F mov       rcx, rax
00000001803AEC32 call      0x1802501e0
00000001803AEC37 mov       rax, qword ptr [rip + 0x17f33da] # 0x181ba2018
00000001803AEC3E mov       rax, qword ptr [rax + 0xb8]
00000001803AEC45 mov       rsi, qword ptr [rax]
00000001803AEC48 test      rsi, rsi
00000001803AEC4B je        0x1803aef67
00000001803AEC51 mov       rsi, qword ptr [rsi + 0x28]
00000001803AEC55 mov       qword ptr [rsp + 0x80], rsi
00000001803AEC5D test      rsi, rsi
00000001803AEC60 je        0x1803aeebd
00000001803AEC66 mov       rax, qword ptr [rip + 0x180c08b] # 0x181bbacf8
00000001803AEC6D mov       rdx, qword ptr [rax + 0xb8]
00000001803AEC74 xor       r8d, r8d
00000001803AEC77 mov       rdx, qword ptr [rdx]
00000001803AEC7A mov       rcx, qword ptr [rdi + 0x68]
00000001803AEC7E call      0x180b76170
00000001803AEC83 test      al, al
00000001803AEC85 jne       0x1803aeed0
00000001803AEC8B mov       rax, qword ptr [rsi + 0x18]
00000001803AEC8F test      rax, rax
00000001803AEC92 je        0x1803aef67
00000001803AEC98 mov       rdx, qword ptr [rax + 0x18]
00000001803AEC9C test      rdx, rdx
00000001803AEC9F je        0x1803aef67
00000001803AECA5 mov       r8, qword ptr [rip + 0x1809804] # 0x181bb84b0
00000001803AECAC lea       rcx, [rsp + 0x28]
00000001803AECB1 call      0x1808197f0
00000001803AECB6 movups    xmm0, xmmword ptr [rsp + 0x28]
00000001803AECBB movups    xmmword ptr [rsp + 0x40], xmm0
00000001803AECC0 movsd     xmm1, qword ptr [rsp + 0x38]
00000001803AECC6 movsd     qword ptr [rsp + 0x50], xmm1
00000001803AECCC mov       qword ptr [rsp + 0x28], 0
00000001803AECD5 lea       rbx, [rsp + 0x40]
00000001803AECDA mov       qword ptr [rsp + 0x30], rbx
00000001803AECDF nop       
00000001803AECE0 mov       rdx, qword ptr [rip + 0x1818df1] # 0x181bc7ad8
00000001803AECE7 lea       rcx, [rsp + 0x40]
00000001803AECEC call      0x180669280
00000001803AECF1 test      al, al
00000001803AECF3 je        0x1803aedb8
00000001803AECF9 mov       rcx, qword ptr [rsp + 0x50]
00000001803AECFE test      rcx, rcx
00000001803AED01 je        0x1803aef78
00000001803AED07 xor       r8d, r8d
00000001803AED0A mov       rdx, qword ptr [rdi + 0x68]
00000001803AED0E mov       rcx, qword ptr [rcx + 0x18]
00000001803AED12 call      0x180b76170
00000001803AED17 test      al, al
00000001803AED19 je        0x1803aece0
00000001803AED1B mov       rcx, qword ptr [rdi + 0x48]
00000001803AED1F test      rcx, rcx
00000001803AED22 je        0x1803aef73
00000001803AED28 xor       r9d, r9d
00000001803AED2B mov       r8b, 1
00000001803AED2E mov       rdx, qword ptr [rip + 0x17f7813] # 0x181ba6548
00000001803AED35 call      0x1811af070
00000001803AED3A mov       rax, qword ptr [rip + 0x1807737] # 0x181bb6478
00000001803AED41 cmp       dword ptr [rax + 0xe0], 0
00000001803AED48 jne       0x1803aed59
00000001803AED4A mov       rcx, rax
00000001803AED4D call      0x1802501e0
00000001803AED52 mov       rax, qword ptr [rip + 0x180771f] # 0x181bb6478
00000001803AED59 mov       rax, qword ptr [rax + 0xb8]
00000001803AED60 mov       rcx, qword ptr [rax + 0x108]
00000001803AED67 test      rcx, rcx
00000001803AED6A je        0x1803aef6d
00000001803AED70 mov       r8, qword ptr [rip + 0x17f5501] # 0x181ba4278
00000001803AED77 mov       edx, 2
00000001803AED7C call      0x180825710
00000001803AED81 mov       rdi, rax
00000001803AED84 mov       rcx, qword ptr [rip + 0x17f328d] # 0x181ba2018
00000001803AED8B cmp       dword ptr [rcx + 0xe0], 0
00000001803AED92 jne       0x1803aed99
00000001803AED94 call      0x1802501e0
00000001803AED99 xor       edx, edx
00000001803AED9B mov       rcx, rdi
00000001803AED9E call      0x18031b160
00000001803AEDA3 nop       
00000001803AEDA4 mov       rdx, qword ptr [rip + 0x1818cd5] # 0x181bc7a80
00000001803AEDAB mov       rcx, rbx
00000001803AEDAE call      0x180302170
00000001803AEDB3 jmp       0x1803aeebd
00000001803AEDB8 mov       rdx, qword ptr [rip + 0x1818cc1] # 0x181bc7a80
00000001803AEDBF mov       rcx, rbx
00000001803AEDC2 call      0x180302170
00000001803AEDC7 jmp       0x1803aedf5
00000001803AEDC9 mov       rdx, qword ptr [rip + 0x1818cb0] # 0x181bc7a80
00000001803AEDD0 mov       rcx, qword ptr [rsp + 0x30]
00000001803AEDD5 call      0x180302170
00000001803AEDDA mov       rcx, qword ptr [rsp + 0x28]
00000001803AEDDF test      rcx, rcx
00000001803AEDE2 jne       0x1803aef7e
00000001803AEDE8 mov       rdi, qword ptr [rsp + 0x70]
00000001803AEDED mov       rsi, qword ptr [rsp + 0x80]
00000001803AEDF5 xor       r8d, r8d
00000001803AEDF8 mov       rdx, qword ptr [rdi + 0x68]
00000001803AEDFC mov       rcx, rsi
00000001803AEDFF call      0x18033e4b0
00000001803AEE04 cmp       byte ptr [rip + 0x1935d26], 0 # 0x181ce4b31
00000001803AEE0B jne       0x1803aee20
00000001803AEE0D lea       rcx, [rip + 0x17fd75c] # 0x181bac570
00000001803AEE14 call      0x18024fef0
00000001803AEE19 mov       byte ptr [rip + 0x1935d11], 1 # 0x181ce4b31
00000001803AEE20 mov       rax, qword ptr [rip + 0x17fd749] # 0x181bac570
00000001803AEE27 mov       rcx, qword ptr [rax + 0xb8]
00000001803AEE2E mov       rax, qword ptr [rcx]
00000001803AEE31 test      rax, rax
00000001803AEE34 je        0x1803aef67
00000001803AEE3A mov       rax, qword ptr [rax + 0x58]
00000001803AEE3E test      rax, rax
00000001803AEE41 je        0x1803aef67
00000001803AEE47 mov       rcx, qword ptr [rax + 0x28]
00000001803AEE4B test      rcx, rcx
00000001803AEE4E je        0x1803aef67
00000001803AEE54 xor       r8d, r8d
00000001803AEE57 xor       edx, edx
00000001803AEE59 call      0x18131bad0
00000001803AEE5E cmp       dword ptr [rdi + 0x20], 2
00000001803AEE62 jne       0x1803aee91
00000001803AEE64 xor       ecx, ecx
00000001803AEE66 call      0x1802fb0c0
00000001803AEE6B test      rax, rax
00000001803AEE6E je        0x1803aef67
00000001803AEE74 mov       rcx, qword ptr [rax + 0x70]
00000001803AEE78 test      rcx, rcx
00000001803AEE7B je        0x1803aef67
00000001803AEE81 xor       r8d, r8d
00000001803AEE84 movss     xmm1, dword ptr [rip + 0x11f8b84] # 0x1815a7a10
00000001803AEE8C call      0x1812dbcd0
00000001803AEE91 xor       edx, edx
00000001803AEE93 mov       rcx, rdi
00000001803AEE96 call      0x1813044d0
00000001803AEE9B mov       rbx, rax
00000001803AEE9E mov       rcx, qword ptr [rip + 0x180165b] # 0x181bb0500
00000001803AEEA5 cmp       dword ptr [rcx + 0xe0], 0
00000001803AEEAC jne       0x1803aeeb3
00000001803AEEAE call      0x1802501e0
00000001803AEEB3 xor       edx, edx
00000001803AEEB5 mov       rcx, rbx
00000001803AEEB8 call      0x18131e940
00000001803AEEBD mov       rbx, qword ptr [rsp + 0x78]
00000001803AEEC2 mov       rsi, qword ptr [rsp + 0x88]
00000001803AEECA add       rsp, 0x60
00000001803AEECE pop       rdi
00000001803AEECF ret       
00000001803AEED0 mov       rcx, qword ptr [rdi + 0x48]
00000001803AEED4 test      rcx, rcx
00000001803AEED7 je        0x1803aef67
00000001803AEEDD xor       r9d, r9d
00000001803AEEE0 mov       r8b, 1
00000001803AEEE3 mov       rdx, qword ptr [rip + 0x17f53de] # 0x181ba42c8
00000001803AEEEA call      0x1811af070
00000001803AEEEF mov       rax, qword ptr [rip + 0x1807582] # 0x181bb6478
00000001803AEEF6 cmp       dword ptr [rax + 0xe0], 0
00000001803AEEFD jne       0x1803aef0e
00000001803AEEFF mov       rcx, rax
00000001803AEF02 call      0x1802501e0
00000001803AEF07 mov       rax, qword ptr [rip + 0x180756a] # 0x181bb6478
00000001803AEF0E mov       rax, qword ptr [rax + 0xb8]
00000001803AEF15 mov       rcx, qword ptr [rax + 0x108]
00000001803AEF1C test      rcx, rcx
00000001803AEF1F je        0x1803aef67
00000001803AEF21 mov       r8, qword ptr [rip + 0x17f5350] # 0x181ba4278
00000001803AEF28 mov       edx, 2
00000001803AEF2D call      0x180825710
00000001803AEF32 mov       rbx, rax
00000001803AEF35 mov       rcx, qword ptr [rip + 0x17f30dc] # 0x181ba2018
00000001803AEF3C cmp       dword ptr [rcx + 0xe0], 0
00000001803AEF43 jne       0x1803aef4a
00000001803AEF45 call      0x1802501e0
00000001803AEF4A xor       edx, edx
00000001803AEF4C mov       rcx, rbx
00000001803AEF4F call      0x18031b160
00000001803AEF54 mov       rbx, qword ptr [rsp + 0x78]
00000001803AEF59 mov       rsi, qword ptr [rsp + 0x88]
00000001803AEF61 add       rsp, 0x60
00000001803AEF65 pop       rdi
00000001803AEF66 ret       
00000001803AEF67 call      0x180250150
00000001803AEF6C nop       
00000001803AEF6D call      0x180250150
00000001803AEF72 nop       
00000001803AEF73 call      0x180250150
00000001803AEF78 call      0x180250150
00000001803AEF7D nop       
00000001803AEF7E call      0x180200e60
00000001803AEF83 int3      
00000001803AEF84 int3      
00000001803AEF85 int3      
00000001803AEF86 int3      
00000001803AEF87 int3      
00000001803AEF88 int3      
00000001803AEF89 int3      
00000001803AEF8A int3      
00000001803AEF8B int3      
00000001803AEF8C int3      
00000001803AEF8D int3      
00000001803AEF8E int3      
00000001803AEF8F int3      