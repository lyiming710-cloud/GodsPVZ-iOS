00000001803128D0 mov       qword ptr [rsp + 0x18], rbx
00000001803128D5 mov       dword ptr [rsp + 0x10], edx
00000001803128D9 push      rsi
00000001803128DA push      rdi
00000001803128DB push      r14
00000001803128DD sub       rsp, 0x70
00000001803128E1 movaps    xmmword ptr [rsp + 0x60], xmm6
00000001803128E6 mov       rbx, rcx
00000001803128E9 cmp       byte ptr [rip + 0x19d22a6], 0 # 0x181ce4b96
00000001803128F0 jne       0x180312941
00000001803128F2 lea       rcx, [rip + 0x18b8a77] # 0x181bcb370
00000001803128F9 call      0x18024fef0
00000001803128FE lea       rcx, [rip + 0x18aec73] # 0x181bc1578
0000000180312905 call      0x18024fef0
000000018031290A lea       rcx, [rip + 0x18aecbf] # 0x181bc15d0
0000000180312911 call      0x18024fef0
0000000180312916 lea       rcx, [rip + 0x18aed0b] # 0x181bc1628
000000018031291D call      0x18024fef0
0000000180312922 lea       rcx, [rip + 0x189242f] # 0x181ba4d58
0000000180312929 call      0x18024fef0
000000018031292E lea       rcx, [rip + 0x189dbcb] # 0x181bb0500
0000000180312935 call      0x18024fef0
000000018031293A mov       byte ptr [rip + 0x19d2255], 1 # 0x181ce4b96
0000000180312941 mov       rsi, qword ptr [rbx + 0x50]
0000000180312945 mov       rcx, qword ptr [rip + 0x18b8a24] # 0x181bcb370
000000018031294C call      0x180250100
0000000180312951 mov       rdi, rax
0000000180312954 mov       dword ptr [rax + 0x10], 0x3f800000
000000018031295B mov       dword ptr [rax + 0x14], 0x3f800000
0000000180312962 mov       dword ptr [rax + 0x18], 0x3f800000
0000000180312969 xor       edx, edx
000000018031296B mov       rcx, rax
000000018031296E call      0x180302170
0000000180312973 test      rsi, rsi
0000000180312976 je        0x180312b4e
000000018031297C movss     xmm0, dword ptr [rsi + 0x10]
0000000180312981 movss     dword ptr [rdi + 0x10], xmm0
0000000180312986 movss     xmm1, dword ptr [rsi + 0x14]
000000018031298B movss     dword ptr [rdi + 0x14], xmm1
0000000180312990 movss     xmm0, dword ptr [rsi + 0x18]
0000000180312995 movss     dword ptr [rdi + 0x18], xmm0
000000018031299A mov       qword ptr [rsp + 0x90], rdi
00000001803129A2 mov       rsi, qword ptr [rbx + 0x20]
00000001803129A6 mov       rcx, qword ptr [rip + 0x189db53] # 0x181bb0500
00000001803129AD cmp       dword ptr [rcx + 0xe0], 0
00000001803129B4 jne       0x1803129bb
00000001803129B6 call      0x1802501e0
00000001803129BB xor       r8d, r8d
00000001803129BE xor       edx, edx
00000001803129C0 mov       rcx, rsi
00000001803129C3 call      0x18131f760
00000001803129C8 test      al, al
00000001803129CA jne       0x180312b35
00000001803129D0 mov       rdx, qword ptr [rbx + 0x48]
00000001803129D4 test      rdx, rdx
00000001803129D7 je        0x180312b4e
00000001803129DD mov       r8, qword ptr [rip + 0x1892374] # 0x181ba4d58
00000001803129E4 lea       rcx, [rsp + 0x28]
00000001803129E9 call      0x1808197f0
00000001803129EE movups    xmm0, xmmword ptr [rsp + 0x28]
00000001803129F3 movups    xmmword ptr [rsp + 0x40], xmm0
00000001803129F8 movsd     xmm1, qword ptr [rsp + 0x38]
00000001803129FE movsd     qword ptr [rsp + 0x50], xmm1
0000000180312A04 mov       qword ptr [rsp + 0x28], 0
0000000180312A0D lea       rbx, [rsp + 0x40]
0000000180312A12 mov       qword ptr [rsp + 0x30], rbx
0000000180312A17 nop       word ptr [rax + rax]
0000000180312A20 mov       rdx, qword ptr [rip + 0x18aeba9] # 0x181bc15d0
0000000180312A27 lea       rcx, [rsp + 0x40]
0000000180312A2C call      0x180669280
0000000180312A31 test      al, al
0000000180312A33 je        0x180312b01
0000000180312A39 mov       rsi, qword ptr [rsp + 0x50]
0000000180312A3E test      rsi, rsi
0000000180312A41 je        0x180312b69
0000000180312A47 cmp       byte ptr [rsi + 0x18], 0
0000000180312A4B jne       0x180312a53
0000000180312A4D cmp       byte ptr [rsi + 0x19], 0
0000000180312A51 je        0x180312a20
0000000180312A53 mov       r14, qword ptr [rsi + 0x20]
0000000180312A57 xor       edx, edx
0000000180312A59 lea       rcx, [rsp + 0x98]
0000000180312A61 call      0x180ca3820
0000000180312A66 xor       r8d, r8d
0000000180312A69 mov       rdx, rax
0000000180312A6C mov       rcx, r14
0000000180312A6F call      0x180b7cc30
0000000180312A74 test      al, al
0000000180312A76 jne       0x180312a20
0000000180312A78 test      rsi, rsi
0000000180312A7B je        0x180312b64
0000000180312A81 mov       ecx, dword ptr [rsi + 0x10]
0000000180312A84 add       ecx, -0x21
0000000180312A87 je        0x180312adf
0000000180312A89 sub       ecx, 1
0000000180312A8C je        0x180312ab9
0000000180312A8E cmp       ecx, 1
0000000180312A91 jne       0x180312a20
0000000180312A93 test      rdi, rdi
0000000180312A96 je        0x180312b54
0000000180312A9C movss     xmm6, dword ptr [rdi + 0x10]
0000000180312AA1 xor       edx, edx
0000000180312AA3 mov       rcx, rsi
0000000180312AA6 call      0x180395070
0000000180312AAB addss     xmm0, xmm6
0000000180312AAF movss     dword ptr [rdi + 0x10], xmm0
0000000180312AB4 jmp       0x180312a20
0000000180312AB9 test      rdi, rdi
0000000180312ABC je        0x180312b5a
0000000180312AC2 movss     xmm6, dword ptr [rdi + 0x18]
0000000180312AC7 xor       edx, edx
0000000180312AC9 mov       rcx, rsi
0000000180312ACC call      0x180395070
0000000180312AD1 addss     xmm0, xmm6
0000000180312AD5 movss     dword ptr [rdi + 0x18], xmm0
0000000180312ADA jmp       0x180312a20
0000000180312ADF test      rdi, rdi
0000000180312AE2 je        0x180312b5f
0000000180312AE4 movss     xmm6, dword ptr [rdi + 0x14]
0000000180312AE9 xor       edx, edx
0000000180312AEB mov       rcx, rsi
0000000180312AEE call      0x180395070
0000000180312AF3 addss     xmm0, xmm6
0000000180312AF7 movss     dword ptr [rdi + 0x14], xmm0
0000000180312AFC jmp       0x180312a20
0000000180312B01 mov       rdx, qword ptr [rip + 0x18aea70] # 0x181bc1578
0000000180312B08 mov       rcx, rbx
0000000180312B0B call      0x180302170
0000000180312B10 jmp       0x180312b35
0000000180312B12 mov       rdx, qword ptr [rip + 0x18aea5f] # 0x181bc1578
0000000180312B19 mov       rcx, qword ptr [rsp + 0x30]
0000000180312B1E call      0x180302170
0000000180312B23 mov       rcx, qword ptr [rsp + 0x28]
0000000180312B28 test      rcx, rcx
0000000180312B2B jne       0x180312b6f
0000000180312B2D mov       rdi, qword ptr [rsp + 0x90]
0000000180312B35 mov       rax, rdi
0000000180312B38 mov       rbx, qword ptr [rsp + 0xa0]
0000000180312B40 movaps    xmm6, xmmword ptr [rsp + 0x60]
0000000180312B45 add       rsp, 0x70
0000000180312B49 pop       r14
0000000180312B4B pop       rdi
0000000180312B4C pop       rsi
0000000180312B4D ret       
0000000180312B4E call      0x180250150
0000000180312B53 nop       
0000000180312B54 call      0x180250150
0000000180312B59 nop       
0000000180312B5A call      0x180250150
0000000180312B5F call      0x180250150
0000000180312B64 call      0x180250150
0000000180312B69 call      0x180250150
0000000180312B6E nop       
0000000180312B6F call      0x180200e60
0000000180312B74 int3      
0000000180312B75 int3      
0000000180312B76 int3      
0000000180312B77 int3      
0000000180312B78 int3      
0000000180312B79 int3      
0000000180312B7A int3      
0000000180312B7B int3      
0000000180312B7C int3      
0000000180312B7D int3      
0000000180312B7E int3      
0000000180312B7F int3      