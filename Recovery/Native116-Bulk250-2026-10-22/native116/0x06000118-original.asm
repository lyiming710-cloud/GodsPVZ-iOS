000000018031C960 push      rsi
000000018031C962 sub       rsp, 0x90
000000018031C969 cmp       byte ptr [rip + 0x19c81de], 0 # 0x181ce4b4e
000000018031C970 mov       rsi, rcx
000000018031C973 jne       0x18031c9dc
000000018031C975 lea       rcx, [rip + 0x187e3cc] # 0x181b9ad48
000000018031C97C call      0x18024fef0
000000018031C981 lea       rcx, [rip + 0x1885690] # 0x181ba2018
000000018031C988 call      0x18024fef0
000000018031C98D lea       rcx, [rip + 0x1886c3c] # 0x181ba35d0
000000018031C994 call      0x18024fef0
000000018031C999 lea       rcx, [rip + 0x18878d8] # 0x181ba4278
000000018031C9A0 call      0x18024fef0
000000018031C9A5 lea       rcx, [rip + 0x1899acc] # 0x181bb6478
000000018031C9AC call      0x18024fef0
000000018031C9B1 lea       rcx, [rip + 0x18a5e30] # 0x181bc27e8
000000018031C9B8 call      0x18024fef0
000000018031C9BD lea       rcx, [rip + 0x1882254] # 0x181b9ec18
000000018031C9C4 call      0x18024fef0
000000018031C9C9 lea       rcx, [rip + 0x1884180] # 0x181ba0b50
000000018031C9D0 call      0x18024fef0
000000018031C9D5 mov       byte ptr [rip + 0x19c8172], 1 # 0x181ce4b4e
000000018031C9DC mov       rax, qword ptr [rsi + 0x20]
000000018031C9E0 mov       qword ptr [rsp + 0xa0], rbx
000000018031C9E8 mov       qword ptr [rsp + 0xa8], rbp
000000018031C9F0 mov       qword ptr [rsp + 0xb0], rdi
000000018031C9F8 movaps    xmmword ptr [rsp + 0x80], xmm6
000000018031CA00 movaps    xmmword ptr [rsp + 0x70], xmm7
000000018031CA05 movaps    xmmword ptr [rsp + 0x60], xmm8
000000018031CA0B test      rax, rax
000000018031CA0E je        0x18031cc91
000000018031CA14 mov       rcx, qword ptr [rax + 0x20]
000000018031CA18 test      rcx, rcx
000000018031CA1B je        0x18031cc91
000000018031CA21 mov       rdx, qword ptr [rcx + 0x28]
000000018031CA25 test      rdx, rdx
000000018031CA28 je        0x18031cc91
000000018031CA2E mov       r9d, dword ptr [rsi + 0x2c]
000000018031CA32 lea       rcx, [rsp + 0x50]
000000018031CA37 mov       r8d, dword ptr [rsi + 0x28]
000000018031CA3B xor       ebp, ebp
000000018031CA3D mov       qword ptr [rsp + 0x20], rbp
000000018031CA42 call      0x18030ec90
000000018031CA47 lea       rcx, [rsi + 0x28]
000000018031CA4B xor       edx, edx
000000018031CA4D movsd     xmm7, qword ptr [rax]
000000018031CA51 movss     xmm6, dword ptr [rax + 8]
000000018031CA56 movsd     qword ptr [rsp + 0x40], xmm7
000000018031CA5C movss     xmm8, dword ptr [rsp + 0x44]
000000018031CA63 addss     xmm8, dword ptr [rip + 0x128b0a0] # 0x1815a7b0c
000000018031CA6C addss     xmm7, dword ptr [rip + 0x128b0a4] # 0x1815a7b18
000000018031CA74 call      0x180ca3820
000000018031CA79 lea       rcx, [rsi + 0x2c]
000000018031CA7D xor       edx, edx
000000018031CA7F mov       rbx, rax
000000018031CA82 call      0x180ca3820
000000018031CA87 mov       r8, qword ptr [rip + 0x188218a] # 0x181b9ec18
000000018031CA8E mov       r9, rax
000000018031CA91 mov       rcx, qword ptr [rip + 0x18840b8] # 0x181ba0b50
000000018031CA98 mov       rdx, rbx
000000018031CA9B mov       qword ptr [rsp + 0x20], rbp
000000018031CAA0 call      0x180b73950
000000018031CAA5 mov       rcx, qword ptr [rip + 0x187e29c] # 0x181b9ad48
000000018031CAAC mov       rbx, rax
000000018031CAAF cmp       dword ptr [rcx + 0xe0], ebp
000000018031CAB5 jne       0x18031cabc
000000018031CAB7 call      0x1802501e0
000000018031CABC xor       edx, edx
000000018031CABE mov       rcx, rbx
000000018031CAC1 call      0x1812e68f0
000000018031CAC6 mov       rcx, qword ptr [rip + 0x18a5d1b] # 0x181bc27e8
000000018031CACD lea       rdx, [rsp + 0x30]
000000018031CAD2 movaps    xmm0, xmm7
000000018031CAD5 movss     dword ptr [rsp + 0x38], xmm6
000000018031CADB unpcklps  xmm0, xmm8
000000018031CADF movsd     qword ptr [rsp + 0x30], xmm0
000000018031CAE5 call      0x18024f340
000000018031CAEA xor       edx, edx
000000018031CAEC mov       rcx, rax
000000018031CAEF call      0x1812e68f0
000000018031CAF4 mov       rcx, qword ptr [rsi + 0x20]
000000018031CAF8 test      rcx, rcx
000000018031CAFB je        0x18031cc91
000000018031CB01 movaps    xmm0, xmm7
000000018031CB04 movss     dword ptr [rsp + 0x48], xmm6
000000018031CB0A unpcklps  xmm0, xmm8
000000018031CB0E lea       rdx, [rsp + 0x40]
000000018031CB13 mov       r9b, 1
000000018031CB16 movsd     qword ptr [rsp + 0x40], xmm0
000000018031CB1C movaps    xmm2, xmm6
000000018031CB1F mov       qword ptr [rsp + 0x20], rbp
000000018031CB24 call      0x180369f40
000000018031CB29 mov       rcx, qword ptr [rip + 0x1886aa0] # 0x181ba35d0
000000018031CB30 mov       rbx, qword ptr [rsi + 0x20]
000000018031CB34 call      0x180250100
000000018031CB39 xor       edx, edx
000000018031CB3B mov       rcx, rax
000000018031CB3E mov       rdi, rax
000000018031CB41 call      0x180310bb0
000000018031CB46 lea       rcx, [rdi + 0x60]
000000018031CB4A mov       byte ptr [rdi + 0x58], bpl
000000018031CB4E mov       rdx, rbx
000000018031CB51 mov       qword ptr [rdi + 0x60], rbx
000000018031CB55 call      0x18024f360
000000018031CB5A mov       dword ptr [rdi + 0x1c], 0x3f000000
000000018031CB61 mov       dword ptr [rdi + 0x18], ebp
000000018031CB64 mov       byte ptr [rdi + 0x20], bpl
000000018031CB68 mov       rcx, qword ptr [rsi + 0x20]
000000018031CB6C test      rcx, rcx
000000018031CB6F je        0x18031cc91
000000018031CB75 mov       rcx, qword ptr [rcx + 0x228]
000000018031CB7C test      rcx, rcx
000000018031CB7F je        0x18031cc91
000000018031CB85 xor       r8d, r8d
000000018031CB88 mov       rdx, rdi
000000018031CB8B call      0x180310190
000000018031CB90 mov       rax, qword ptr [rip + 0x18998e1] # 0x181bb6478
000000018031CB97 cmp       dword ptr [rax + 0xe0], ebp
000000018031CB9D jne       0x18031cbae
000000018031CB9F mov       rcx, rax
000000018031CBA2 call      0x1802501e0
000000018031CBA7 mov       rax, qword ptr [rip + 0x18998ca] # 0x181bb6478
000000018031CBAE mov       rax, qword ptr [rax + 0xb8]
000000018031CBB5 mov       rcx, qword ptr [rax + 0x120]
000000018031CBBC test      rcx, rcx
000000018031CBBF je        0x18031cc91
000000018031CBC5 mov       r8, qword ptr [rip + 0x18876ac] # 0x181ba4278
000000018031CBCC mov       edx, 6
000000018031CBD1 call      0x180825710
000000018031CBD6 xor       ecx, ecx
000000018031CBD8 mov       rbx, rax
000000018031CBDB call      0x1812e27c0
000000018031CBE0 test      rax, rax
000000018031CBE3 je        0x18031cc91
000000018031CBE9 xor       edx, edx
000000018031CBEB mov       rcx, rax
000000018031CBEE call      0x181304510
000000018031CBF3 test      rax, rax
000000018031CBF6 je        0x18031cc91
000000018031CBFC xor       r8d, r8d
000000018031CBFF lea       rcx, [rsp + 0x50]
000000018031CC04 mov       rdx, rax
000000018031CC07 call      0x18132f3a0
000000018031CC0C mov       rcx, qword ptr [rip + 0x1885405] # 0x181ba2018
000000018031CC13 movsd     xmm6, qword ptr [rax]
000000018031CC17 mov       edi, dword ptr [rax + 8]
000000018031CC1A cmp       dword ptr [rcx + 0xe0], ebp
000000018031CC20 jne       0x18031cc27
000000018031CC22 call      0x1802501e0
000000018031CC27 xor       ecx, ecx
000000018031CC29 movsd     qword ptr [rsp + 0x40], xmm6
000000018031CC2F mov       dword ptr [rsp + 0x48], edi
000000018031CC33 call      0x18031aea0
000000018031CC38 mulss     xmm0, dword ptr [rip + 0x128ae88] # 0x1815a7ac8
000000018031CC40 lea       rdx, [rsp + 0x40]
000000018031CC45 mov       rcx, rbx
000000018031CC48 mov       qword ptr [rsp + 0x20], rbp
000000018031CC4D movss     xmm3, dword ptr [rip + 0x128ae77] # 0x1815a7acc
000000018031CC55 movaps    xmm2, xmm0
000000018031CC58 call      0x18031b350
000000018031CC5D movaps    xmm8, xmmword ptr [rsp + 0x60]
000000018031CC63 movaps    xmm7, xmmword ptr [rsp + 0x70]
000000018031CC68 movaps    xmm6, xmmword ptr [rsp + 0x80]
000000018031CC70 mov       rdi, qword ptr [rsp + 0xb0]
000000018031CC78 mov       rbp, qword ptr [rsp + 0xa8]
000000018031CC80 mov       rbx, qword ptr [rsp + 0xa0]
000000018031CC88 add       rsp, 0x90
000000018031CC8F pop       rsi
000000018031CC90 ret       
000000018031CC91 call      0x180250150
000000018031CC96 int3      
000000018031CC97 int3      
000000018031CC98 int3      
000000018031CC99 int3      
000000018031CC9A int3      
000000018031CC9B int3      
000000018031CC9C int3      
000000018031CC9D int3      
000000018031CC9E int3      
000000018031CC9F int3      