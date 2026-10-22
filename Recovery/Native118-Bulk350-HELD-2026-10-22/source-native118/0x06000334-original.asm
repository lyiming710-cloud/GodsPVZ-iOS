000000018034A620 mov       rax, rsp
000000018034A623 mov       qword ptr [rax + 0x18], rbx
000000018034A627 mov       qword ptr [rax + 0x20], rsi
000000018034A62B mov       qword ptr [rax + 0x10], rdx
000000018034A62F mov       qword ptr [rax + 8], rcx
000000018034A633 push      rdi
000000018034A634 push      r14
000000018034A636 push      r15
000000018034A638 sub       rsp, 0x130
000000018034A63F movaps    xmmword ptr [rax - 0x28], xmm6
000000018034A643 movaps    xmmword ptr [rax - 0x38], xmm7
000000018034A647 movaps    xmmword ptr [rax - 0x48], xmm8
000000018034A64C movaps    xmmword ptr [rax - 0x58], xmm9
000000018034A651 movaps    xmmword ptr [rax - 0x68], xmm10
000000018034A656 mov       r15, rdx
000000018034A659 mov       rdi, rcx
000000018034A65C cmp       byte ptr [rip + 0x199a679], 0 # 0x181ce4cdc
000000018034A663 jne       0x18034a6f4
000000018034A669 lea       rcx, [rip + 0x1889838] # 0x181bd3ea8
000000018034A670 call      0x18024fef0
000000018034A675 lea       rcx, [rip + 0x18506cc] # 0x181b9ad48
000000018034A67C call      0x18024fef0
000000018034A681 lea       rcx, [rip + 0x1878dd8] # 0x181bc3460
000000018034A688 call      0x18024fef0
000000018034A68D lea       rcx, [rip + 0x1878e24] # 0x181bc34b8
000000018034A694 call      0x18024fef0
000000018034A699 lea       rcx, [rip + 0x1878e70] # 0x181bc3510
000000018034A6A0 call      0x18024fef0
000000018034A6A5 lea       rcx, [rip + 0x18598ac] # 0x181ba3f58
000000018034A6AC call      0x18024fef0
000000018034A6B1 lea       rcx, [rip + 0x185ab00] # 0x181ba51b8
000000018034A6B8 call      0x18024fef0
000000018034A6BD lea       rcx, [rip + 0x18617b4] # 0x181babe78
000000018034A6C4 call      0x18024fef0
000000018034A6C9 lea       rcx, [rip + 0x1865e30] # 0x181bb0500
000000018034A6D0 call      0x18024fef0
000000018034A6D5 lea       rcx, [rip + 0x185c52c] # 0x181ba6c08
000000018034A6DC call      0x18024fef0
000000018034A6E1 lea       rcx, [rip + 0x1877c28] # 0x181bc2310
000000018034A6E8 call      0x18024fef0
000000018034A6ED mov       byte ptr [rip + 0x199a5e8], 1 # 0x181ce4cdc
000000018034A6F4 xorps     xmm0, xmm0
000000018034A6F7 xor       eax, eax
000000018034A6F9 movups    xmmword ptr [rsp + 0x78], xmm0
000000018034A6FE movups    xmmword ptr [rsp + 0x88], xmm0
000000018034A706 mov       dword ptr [rsp + 0x98], eax
000000018034A70D test      r15, r15
000000018034A710 je        0x18034ab27
000000018034A716 mov       rdx, qword ptr [r15 + 0x28]
000000018034A71A mov       qword ptr [rdi + 0x28], rdx
000000018034A71E lea       rcx, [rdi + 0x28]
000000018034A722 call      0x18024f360
000000018034A727 mov       rcx, qword ptr [rip + 0x185061a] # 0x181b9ad48
000000018034A72E cmp       dword ptr [rcx + 0xe0], 0
000000018034A735 jne       0x18034a73c
000000018034A737 call      0x1802501e0
000000018034A73C xor       edx, edx
000000018034A73E mov       rcx, qword ptr [rip + 0x185c4c3] # 0x181ba6c08
000000018034A745 call      0x1812e68f0
000000018034A74A mov       rdx, qword ptr [rdi + 0xd8]
000000018034A751 test      rdx, rdx
000000018034A754 je        0x18034ab27
000000018034A75A mov       r8, qword ptr [rip + 0x1861717] # 0x181babe78
000000018034A761 lea       rcx, [rsp + 0xa0]
000000018034A769 call      0x1808197f0
000000018034A76E movups    xmm0, xmmword ptr [rsp + 0xa0]
000000018034A776 movups    xmmword ptr [rsp + 0x60], xmm0
000000018034A77B movsd     xmm1, qword ptr [rsp + 0xb0]
000000018034A784 movsd     qword ptr [rsp + 0x70], xmm1
000000018034A78A mov       qword ptr [rsp + 0x50], 0
000000018034A793 lea       rbx, [rsp + 0x60]
000000018034A798 mov       qword ptr [rsp + 0x58], rbx
000000018034A79D movss     xmm10, dword ptr [rip + 0x125d25e] # 0x1815a7a04
000000018034A7A6 nop       word ptr [rax + rax]
000000018034A7B0 mov       rdx, qword ptr [rip + 0x1878d01] # 0x181bc34b8
000000018034A7B7 lea       rcx, [rsp + 0x60]
000000018034A7BC call      0x180669280
000000018034A7C1 test      al, al
000000018034A7C3 je        0x18034a8c8
000000018034A7C9 mov       rsi, qword ptr [rsp + 0x70]
000000018034A7CE test      rsi, rsi
000000018034A7D1 je        0x18034ab38
000000018034A7D7 mov       rdx, qword ptr [rip + 0x185a9da] # 0x181ba51b8
000000018034A7DE mov       rcx, rsi
000000018034A7E1 call      0x18046dc80
000000018034A7E6 test      rax, rax
000000018034A7E9 je        0x18034ab33
000000018034A7EF xor       r8d, r8d
000000018034A7F2 mov       rdx, rax
000000018034A7F5 lea       rcx, [rsp + 0xa0]
000000018034A7FD call      0x181328fc0
000000018034A802 movups    xmm6, xmmword ptr [rax]
000000018034A805 movaps    xmm7, xmm6
000000018034A808 shufps    xmm7, xmm6, 0xff
000000018034A80C mulss     xmm7, xmm10
000000018034A811 movss     dword ptr [rsp + 0x30], xmm6
000000018034A817 movaps    xmm8, xmm6
000000018034A81B shufps    xmm8, xmm6, 0x55
000000018034A820 movss     dword ptr [rsp + 0x34], xmm8
000000018034A827 movaps    xmm9, xmm6
000000018034A82B shufps    xmm9, xmm6, 0xaa
000000018034A830 movss     dword ptr [rsp + 0x38], xmm9
000000018034A837 movss     dword ptr [rsp + 0x3c], xmm7
000000018034A83D lea       rdx, [rsp + 0x30]
000000018034A842 mov       rcx, qword ptr [rip + 0x188965f] # 0x181bd3ea8
000000018034A849 call      0x18024f340
000000018034A84E mov       r14, rax
000000018034A851 mov       rcx, qword ptr [rip + 0x18504f0] # 0x181b9ad48
000000018034A858 cmp       dword ptr [rcx + 0xe0], 0
000000018034A85F jne       0x18034a866
000000018034A861 call      0x1802501e0
000000018034A866 xor       edx, edx
000000018034A868 mov       rcx, r14
000000018034A86B call      0x1812e68f0
000000018034A870 mov       rdx, qword ptr [rip + 0x185a941] # 0x181ba51b8
000000018034A877 mov       rcx, rsi
000000018034A87A call      0x18046dc80
000000018034A87F movss     dword ptr [rsp + 0x40], xmm6
000000018034A885 movss     dword ptr [rsp + 0x44], xmm8
000000018034A88C movss     dword ptr [rsp + 0x48], xmm9
000000018034A893 movss     dword ptr [rsp + 0x4c], xmm7
000000018034A899 test      rax, rax
000000018034A89C je        0x18034ab2d
000000018034A8A2 movups    xmm0, xmmword ptr [rsp + 0x40]
000000018034A8A7 movdqa    xmmword ptr [rsp + 0xd0], xmm0
000000018034A8B0 xor       r8d, r8d
000000018034A8B3 lea       rdx, [rsp + 0xd0]
000000018034A8BB mov       rcx, rax
000000018034A8BE call      0x1813290a0
000000018034A8C3 jmp       0x18034a7b0
000000018034A8C8 mov       rdx, qword ptr [rip + 0x1878b91] # 0x181bc3460
000000018034A8CF mov       rcx, rbx
000000018034A8D2 call      0x180302170
000000018034A8D7 jmp       0x18034a908
000000018034A8D9 mov       rdx, qword ptr [rip + 0x1878b80] # 0x181bc3460
000000018034A8E0 mov       rcx, qword ptr [rsp + 0x58]
000000018034A8E5 call      0x180302170
000000018034A8EA mov       rcx, qword ptr [rsp + 0x50]
000000018034A8EF test      rcx, rcx
000000018034A8F2 jne       0x18034ab3e
000000018034A8F8 mov       r15, qword ptr [rsp + 0x158]
000000018034A900 mov       rdi, qword ptr [rsp + 0x150]
000000018034A908 mov       byte ptr [rdi + 0x94], 0
000000018034A90F mov       rbx, qword ptr [rdi + 0xe0]
000000018034A916 mov       rcx, qword ptr [rip + 0x1865be3] # 0x181bb0500
000000018034A91D cmp       dword ptr [rcx + 0xe0], 0
000000018034A924 jne       0x18034a92b
000000018034A926 call      0x1802501e0
000000018034A92B xor       r8d, r8d
000000018034A92E xor       edx, edx
000000018034A930 mov       rcx, rbx
000000018034A933 call      0x18131f760
000000018034A938 test      al, al
000000018034A93A je        0x18034a96e
000000018034A93C mov       rcx, qword ptr [rdi + 0xc0]
000000018034A943 test      rcx, rcx
000000018034A946 je        0x18034ab27
000000018034A94C mov       rdx, qword ptr [rip + 0x1859605] # 0x181ba3f58
000000018034A953 call      0x18046d280
000000018034A958 mov       qword ptr [rdi + 0xe0], rax
000000018034A95F lea       rcx, [rdi + 0xe0]
000000018034A966 mov       rdx, rax
000000018034A969 call      0x18024f360
000000018034A96E mov       rcx, qword ptr [rdi + 0xe0]
000000018034A975 test      rcx, rcx
000000018034A978 je        0x18034ab27
000000018034A97E xor       r8d, r8d
000000018034A981 mov       rdx, qword ptr [rip + 0x1877988] # 0x181bc2310
000000018034A988 call      0x181348060
000000018034A98D mov       rcx, qword ptr [rdi + 0xe0]
000000018034A994 test      rcx, rcx
000000018034A997 je        0x18034ab27
000000018034A99D xor       r8d, r8d
000000018034A9A0 mov       edx, 0x2328
000000018034A9A5 call      0x1813480b0
000000018034A9AA xor       edx, edx
000000018034A9AC mov       rcx, rdi
000000018034A9AF call      0x181304510
000000018034A9B4 mov       rbx, rax
000000018034A9B7 xor       edx, edx
000000018034A9B9 mov       rcx, r15
000000018034A9BC call      0x181304510
000000018034A9C1 test      rbx, rbx
000000018034A9C4 je        0x18034ab27
000000018034A9CA xor       r9d, r9d
000000018034A9CD xor       r8d, r8d
000000018034A9D0 mov       rdx, rax
000000018034A9D3 mov       rcx, rbx
000000018034A9D6 call      0x18132e900
000000018034A9DB mov       rcx, qword ptr [rdi + 0xd0]
000000018034A9E2 test      rcx, rcx
000000018034A9E5 je        0x18034ab27
000000018034A9EB xor       r8d, r8d
000000018034A9EE xor       edx, edx
000000018034A9F0 call      0x18131bad0
000000018034A9F5 mov       rbx, qword ptr [rdi + 0x110]
000000018034A9FC mov       rcx, qword ptr [rip + 0x1865afd] # 0x181bb0500
000000018034AA03 cmp       dword ptr [rcx + 0xe0], 0
000000018034AA0A jne       0x18034aa11
000000018034AA0C call      0x1802501e0
000000018034AA11 xor       edx, edx
000000018034AA13 mov       rcx, rbx
000000018034AA16 call      0x18131f870
000000018034AA1B test      al, al
000000018034AA1D je        0x18034aa39
000000018034AA1F mov       rcx, qword ptr [rdi + 0x110]
000000018034AA26 test      rcx, rcx
000000018034AA29 je        0x18034ab27
000000018034AA2F xor       r8d, r8d
000000018034AA32 xor       edx, edx
000000018034AA34 call      0x18131bad0
000000018034AA39 mov       rbx, qword ptr [rdi + 0xc8]
000000018034AA40 mov       rcx, qword ptr [rip + 0x1865ab9] # 0x181bb0500
000000018034AA47 cmp       dword ptr [rcx + 0xe0], 0
000000018034AA4E jne       0x18034aa55
000000018034AA50 call      0x1802501e0
000000018034AA55 xor       edx, edx
000000018034AA57 mov       rcx, rbx
000000018034AA5A call      0x18131f870
000000018034AA5F test      al, al
000000018034AA61 je        0x18034aaf5
000000018034AA67 mov       rdx, qword ptr [rdi + 0xc8]
000000018034AA6E test      rdx, rdx
000000018034AA71 je        0x18034ab27
000000018034AA77 xor       r9d, r9d
000000018034AA7A xor       r8d, r8d
000000018034AA7D lea       rcx, [rsp + 0xa0]
000000018034AA85 call      0x1812db780
000000018034AA8A movups    xmm0, xmmword ptr [rax]
000000018034AA8D movups    xmmword ptr [rsp + 0x78], xmm0
000000018034AA92 movups    xmm1, xmmword ptr [rax + 0x10]
000000018034AA96 movups    xmmword ptr [rsp + 0x88], xmm1
000000018034AA9E mov       eax, dword ptr [rax + 0x20]
000000018034AAA1 mov       dword ptr [rsp + 0x98], eax
000000018034AAA8 mov       rbx, qword ptr [rdi + 0xc8]
000000018034AAAF xor       edx, edx
000000018034AAB1 lea       rcx, [rsp + 0x78]
000000018034AAB6 call      0x1808e3a80
000000018034AABB test      rbx, rbx
000000018034AABE je        0x18034ab27
000000018034AAC0 mov       qword ptr [rsp + 0x20], 0
000000018034AAC9 movss     xmm3, dword ptr [rip + 0x125d127] # 0x1815a7bf8
000000018034AAD1 xor       r8d, r8d
000000018034AAD4 mov       edx, eax
000000018034AAD6 mov       rcx, rbx
000000018034AAD9 call      0x1812db860
000000018034AADE mov       rcx, qword ptr [rdi + 0xc8]
000000018034AAE5 test      rcx, rcx
000000018034AAE8 je        0x18034ab27
000000018034AAEA xor       r8d, r8d
000000018034AAED xorps     xmm1, xmm1
000000018034AAF0 call      0x1812dbcd0
000000018034AAF5 lea       r11, [rsp + 0x130]
000000018034AAFD mov       rbx, qword ptr [r11 + 0x30]
000000018034AB01 mov       rsi, qword ptr [r11 + 0x38]
000000018034AB05 movaps    xmm6, xmmword ptr [r11 - 0x10]
000000018034AB0A movaps    xmm7, xmmword ptr [r11 - 0x20]
000000018034AB0F movaps    xmm8, xmmword ptr [r11 - 0x30]
000000018034AB14 movaps    xmm9, xmmword ptr [r11 - 0x40]
000000018034AB19 movaps    xmm10, xmmword ptr [r11 - 0x50]
000000018034AB1E mov       rsp, r11
000000018034AB21 pop       r15
000000018034AB23 pop       r14
000000018034AB25 pop       rdi
000000018034AB26 ret       
000000018034AB27 call      0x180250150
000000018034AB2C nop       
000000018034AB2D call      0x180250150
000000018034AB32 nop       
000000018034AB33 call      0x180250150
000000018034AB38 call      0x180250150
000000018034AB3D nop       
000000018034AB3E call      0x180200e60
000000018034AB43 int3      
000000018034AB44 int3      
000000018034AB45 int3      
000000018034AB46 int3      
000000018034AB47 int3      
000000018034AB48 int3      
000000018034AB49 int3      
000000018034AB4A int3      
000000018034AB4B int3      
000000018034AB4C int3      
000000018034AB4D int3      
000000018034AB4E int3      
000000018034AB4F int3      