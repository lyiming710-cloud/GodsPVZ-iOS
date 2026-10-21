00000001803438A0 mov       rax, rsp
00000001803438A3 mov       qword ptr [rax + 0x10], rbx
00000001803438A7 mov       qword ptr [rax + 0x18], rsi
00000001803438AB mov       qword ptr [rax + 8], rcx
00000001803438AF push      rdi
00000001803438B0 sub       rsp, 0x90
00000001803438B7 movaps    xmmword ptr [rax - 0x18], xmm6
00000001803438BB movaps    xmmword ptr [rax - 0x28], xmm7
00000001803438BF mov       rsi, rcx
00000001803438C2 cmp       byte ptr [rip + 0x19a13e7], 0 # 0x181ce4cb0
00000001803438C9 jne       0x18034390e
00000001803438CB lea       rcx, [rip + 0x187f49e] # 0x181bc2d70
00000001803438D2 call      0x18024fef0
00000001803438D7 lea       rcx, [rip + 0x187e2ca] # 0x181bc1ba8
00000001803438DE call      0x18024fef0
00000001803438E3 lea       rcx, [rip + 0x187e316] # 0x181bc1c00
00000001803438EA call      0x18024fef0
00000001803438EF lea       rcx, [rip + 0x187e362] # 0x181bc1c58
00000001803438F6 call      0x18024fef0
00000001803438FB lea       rcx, [rip + 0x1862836] # 0x181ba6138
0000000180343902 call      0x18024fef0
0000000180343907 mov       byte ptr [rip + 0x19a13a2], 1 # 0x181ce4cb0
000000018034390E mov       byte ptr [rsi + 0x104], 0
0000000180343915 mov       rcx, qword ptr [rsi + 0x78]
0000000180343919 test      rcx, rcx
000000018034391C je        0x180343bc2
0000000180343922 mov       rdx, qword ptr [rip + 0x187f447] # 0x181bc2d70
0000000180343929 call      0x18042e9f0
000000018034392E mov       rbx, rax
0000000180343931 test      rax, rax
0000000180343934 je        0x180343bc2
000000018034393A mov       r8, qword ptr [rax]
000000018034393D mov       r9, qword ptr [r8 + 0x298]
0000000180343944 mov       r8, qword ptr [r8 + 0x2a0]
000000018034394B mov       rdx, rax
000000018034394E lea       rcx, [rsp + 0x30]
0000000180343953 call      r9
0000000180343956 movups    xmm7, xmmword ptr [rax]
0000000180343959 mov       r8, qword ptr [rbx]
000000018034395C mov       rax, qword ptr [r8 + 0x298]
0000000180343963 mov       r8, qword ptr [r8 + 0x2a0]
000000018034396A mov       rdx, rbx
000000018034396D lea       rcx, [rsp + 0x30]
0000000180343972 call      rax
0000000180343974 movups    xmm6, xmmword ptr [rax]
0000000180343977 mov       r8, qword ptr [rbx]
000000018034397A mov       rax, qword ptr [r8 + 0x298]
0000000180343981 mov       r8, qword ptr [r8 + 0x2a0]
0000000180343988 mov       rdx, rbx
000000018034398B lea       rcx, [rsp + 0x30]
0000000180343990 call      rax
0000000180343992 shufps    xmm6, xmm6, 0x55
0000000180343996 movss     xmm1, dword ptr [rax + 8]
000000018034399B mov       dword ptr [rsp + 0x3c], 0x3b03126f
00000001803439A3 mov       r8, qword ptr [rbx]
00000001803439A6 mov       rax, qword ptr [r8 + 0x2a8]
00000001803439AD movaps    xmm0, xmmword ptr [rsp + 0x30]
00000001803439B2 movss     xmm0, xmm7
00000001803439B6 shufps    xmm0, xmm0, 0xe1
00000001803439BA movss     xmm0, xmm6
00000001803439BE shufps    xmm0, xmm0, 0xc6
00000001803439C2 movss     xmm0, xmm1
00000001803439C6 shufps    xmm0, xmm0, 0xc9
00000001803439CA movaps    xmmword ptr [rsp + 0x30], xmm0
00000001803439CF movdqa    xmmword ptr [rsp + 0x30], xmm0
00000001803439D5 mov       r8, qword ptr [r8 + 0x2b0]
00000001803439DC lea       rdx, [rsp + 0x30]
00000001803439E1 mov       rcx, rbx
00000001803439E4 call      rax
00000001803439E6 mov       rcx, qword ptr [rsi + 0x78]
00000001803439EA test      rcx, rcx
00000001803439ED je        0x180343bc2
00000001803439F3 xor       edx, edx
00000001803439F5 call      0x181304510
00000001803439FA test      rax, rax
00000001803439FD je        0x180343bc2
0000000180343A03 xor       r8d, r8d
0000000180343A06 xor       edx, edx
0000000180343A08 mov       rcx, rax
0000000180343A0B call      0x18132dbe0
0000000180343A10 test      rax, rax
0000000180343A13 je        0x180343bc2
0000000180343A19 xor       edx, edx
0000000180343A1B mov       rcx, rax
0000000180343A1E call      0x1813044d0
0000000180343A23 test      rax, rax
0000000180343A26 je        0x180343bc2
0000000180343A2C xor       r8d, r8d
0000000180343A2F xor       edx, edx
0000000180343A31 mov       rcx, rax
0000000180343A34 call      0x18131bad0
0000000180343A39 mov       rcx, qword ptr [rsi + 0xe8]
0000000180343A40 test      rcx, rcx
0000000180343A43 je        0x180343bc2
0000000180343A49 xor       edx, edx
0000000180343A4B call      0x1813044d0
0000000180343A50 test      rax, rax
0000000180343A53 je        0x180343bc2
0000000180343A59 xor       r8d, r8d
0000000180343A5C xor       edx, edx
0000000180343A5E mov       rcx, rax
0000000180343A61 call      0x18131bad0
0000000180343A66 mov       rax, qword ptr [rsi + 0xf0]
0000000180343A6D test      rax, rax
0000000180343A70 je        0x180343bc2
0000000180343A76 mov       rdx, qword ptr [rax + 0x20]
0000000180343A7A test      rdx, rdx
0000000180343A7D je        0x180343bc2
0000000180343A83 mov       r8, qword ptr [rip + 0x18626ae] # 0x181ba6138
0000000180343A8A lea       rcx, [rsp + 0x30]
0000000180343A8F call      0x1808197f0
0000000180343A94 movups    xmm0, xmmword ptr [rsp + 0x30]
0000000180343A99 movups    xmmword ptr [rsp + 0x50], xmm0
0000000180343A9E movsd     xmm1, qword ptr [rsp + 0x40]
0000000180343AA4 movsd     qword ptr [rsp + 0x60], xmm1
0000000180343AAA mov       qword ptr [rsp + 0x30], 0
0000000180343AB3 lea       rbx, [rsp + 0x50]
0000000180343AB8 mov       qword ptr [rsp + 0x38], rbx
0000000180343ABD nop       dword ptr [rax]
0000000180343AC0 mov       rdx, qword ptr [rip + 0x187e139] # 0x181bc1c00
0000000180343AC7 lea       rcx, [rsp + 0x50]
0000000180343ACC call      0x180669280
0000000180343AD1 test      al, al
0000000180343AD3 je        0x180343b46
0000000180343AD5 mov       rcx, qword ptr [rsp + 0x60]
0000000180343ADA test      rcx, rcx
0000000180343ADD je        0x180343bd3
0000000180343AE3 cmp       byte ptr [rcx + 0x104], 0
0000000180343AEA je        0x180343ac0
0000000180343AEC mov       eax, dword ptr [rcx + 0x108]
0000000180343AF2 cmp       eax, dword ptr [rsi + 0x108]
0000000180343AF8 jle       0x180343ac0
0000000180343AFA dec       dword ptr [rcx + 0x108]
0000000180343B00 test      rcx, rcx
0000000180343B03 je        0x180343bce
0000000180343B09 mov       rdi, qword ptr [rcx + 0xe8]
0000000180343B10 add       rcx, 0x108
0000000180343B17 xor       edx, edx
0000000180343B19 call      0x180ca3820
0000000180343B1E test      rdi, rdi
0000000180343B21 je        0x180343bc8
0000000180343B27 mov       r8, qword ptr [rdi]
0000000180343B2A mov       r9, qword ptr [r8 + 0x558]
0000000180343B31 mov       r8, qword ptr [r8 + 0x560]
0000000180343B38 mov       rdx, rax
0000000180343B3B mov       rcx, rdi
0000000180343B3E call      r9
0000000180343B41 jmp       0x180343ac0
0000000180343B46 mov       rdx, qword ptr [rip + 0x187e05b] # 0x181bc1ba8
0000000180343B4D mov       rcx, rbx
0000000180343B50 call      0x180302170
0000000180343B55 jmp       0x180343b7a
0000000180343B57 mov       rdx, qword ptr [rip + 0x187e04a] # 0x181bc1ba8
0000000180343B5E mov       rcx, qword ptr [rsp + 0x38]
0000000180343B63 call      0x180302170
0000000180343B68 mov       rcx, qword ptr [rsp + 0x30]
0000000180343B6D test      rcx, rcx
0000000180343B70 jne       0x180343bd9
0000000180343B72 mov       rsi, qword ptr [rsp + 0xa0]
0000000180343B7A mov       dword ptr [rsi + 0x108], 0
0000000180343B84 mov       rax, qword ptr [rsi + 0xf0]
0000000180343B8B test      rax, rax
0000000180343B8E je        0x180343bc2
0000000180343B90 mov       rcx, qword ptr [rax + 0x50]
0000000180343B94 test      rcx, rcx
0000000180343B97 je        0x180343bc2
0000000180343B99 xor       r8d, r8d
0000000180343B9C xor       edx, edx
0000000180343B9E call      0x1803a1c60
0000000180343BA3 lea       r11, [rsp + 0x90]
0000000180343BAB mov       rbx, qword ptr [r11 + 0x18]
0000000180343BAF mov       rsi, qword ptr [r11 + 0x20]
0000000180343BB3 movaps    xmm6, xmmword ptr [r11 - 0x10]
0000000180343BB8 movaps    xmm7, xmmword ptr [rsp + 0x70]
0000000180343BBD mov       rsp, r11
0000000180343BC0 pop       rdi
0000000180343BC1 ret       
0000000180343BC2 call      0x180250150
0000000180343BC7 nop       
0000000180343BC8 call      0x180250150
0000000180343BCD nop       
0000000180343BCE call      0x180250150
0000000180343BD3 call      0x180250150
0000000180343BD8 nop       
0000000180343BD9 call      0x180200e60
0000000180343BDE int3      
0000000180343BDF int3      