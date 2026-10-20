0000000180306710 mov       qword ptr [rsp + 0x10], rbx
0000000180306715 mov       qword ptr [rsp + 0x18], rbp
000000018030671A push      rdi
000000018030671B sub       rsp, 0x70
000000018030671F cmp       byte ptr [rip + 0x19de3ea], 0 # 0x181ce4b10
0000000180306726 mov       rdi, rcx
0000000180306729 movsxd    rbp, edx
000000018030672C jne       0x180306759
000000018030672E lea       rcx, [rip + 0x189e783] # 0x181ba4eb8
0000000180306735 call      0x18024fef0
000000018030673A lea       rcx, [rip + 0x18a9dbf] # 0x181bb0500
0000000180306741 call      0x18024fef0
0000000180306746 lea       rcx, [rip + 0x18c693b] # 0x181bcd088
000000018030674D call      0x18024fef0
0000000180306752 mov       byte ptr [rip + 0x19de3b7], 1 # 0x181ce4b10
0000000180306759 mov       rcx, qword ptr [rip + 0x18a9da0] # 0x181bb0500
0000000180306760 mov       rbx, qword ptr [rdi + 0x20]
0000000180306764 cmp       dword ptr [rcx + 0xe0], 0
000000018030676B jne       0x180306772
000000018030676D call      0x1802501e0
0000000180306772 xor       r8d, r8d
0000000180306775 xor       edx, edx
0000000180306777 mov       rcx, rbx
000000018030677A call      0x18131f760
000000018030677F test      al, al
0000000180306781 jne       0x180306a5a
0000000180306787 mov       qword ptr [rsp + 0x80], rsi
000000018030678F movaps    xmmword ptr [rsp + 0x60], xmm6
0000000180306794 movaps    xmmword ptr [rsp + 0x50], xmm7
0000000180306799 movaps    xmmword ptr [rsp + 0x40], xmm8
000000018030679F mov       rax, qword ptr [rdi + 0x20]
00000001803067A3 test      rax, rax
00000001803067A6 je        0x180306a6c
00000001803067AC cmp       dword ptr [rax + 0x88], 0
00000001803067B3 jle       0x180306a42
00000001803067B9 cmp       dword ptr [rax + 0x100], 2
00000001803067C0 jl        0x180306a42
00000001803067C6 mov       rcx, qword ptr [rip + 0x18c68bb] # 0x181bcd088
00000001803067CD mov       edx, 6
00000001803067D2 call      0x18024f3e0
00000001803067D7 movss     xmm3, dword ptr [rip + 0x12a1289] # 0x1815a7a68
00000001803067DF mov       rbx, rax
00000001803067E2 movss     xmm1, dword ptr [rip + 0x12a1256] # 0x1815a7a40
00000001803067EA xorps     xmm2, xmm2
00000001803067ED test      rax, rax
00000001803067F0 je        0x180306a6c
00000001803067F6 cmp       dword ptr [rax + 0x18], 0
00000001803067FA jbe       0x180306a72
0000000180306800 movss     xmm4, dword ptr [rip + 0x12a125c] # 0x1815a7a64
0000000180306808 movaps    xmm0, xmm3
000000018030680B movss     xmm5, dword ptr [rip + 0x12a1229] # 0x1815a7a3c
0000000180306813 unpcklps  xmm0, xmm1
0000000180306816 movsd     qword ptr [rax + 0x20], xmm0
000000018030681B movss     dword ptr [rax + 0x28], xmm2
0000000180306820 cmp       dword ptr [rax + 0x18], 1
0000000180306824 jbe       0x180306a72
000000018030682A movss     xmm6, dword ptr [rip + 0x12a11fe] # 0x1815a7a30
0000000180306832 movaps    xmm0, xmm4
0000000180306835 unpcklps  xmm0, xmm5
0000000180306838 movsd     qword ptr [rax + 0x2c], xmm0
000000018030683D movss     dword ptr [rax + 0x34], xmm2
0000000180306842 cmp       dword ptr [rax + 0x18], 2
0000000180306846 jbe       0x180306a72
000000018030684C movss     xmm1, dword ptr [rip + 0x12a11d8] # 0x1815a7a2c
0000000180306854 movaps    xmm0, xmm4
0000000180306857 unpcklps  xmm0, xmm6
000000018030685A movsd     qword ptr [rax + 0x38], xmm0
000000018030685F movss     dword ptr [rax + 0x40], xmm2
0000000180306864 cmp       dword ptr [rax + 0x18], 3
0000000180306868 jbe       0x180306a72
000000018030686E movaps    xmm0, xmm3
0000000180306871 movss     xmm3, dword ptr [rip + 0x12a11f3] # 0x1815a7a6c
0000000180306879 unpcklps  xmm0, xmm1
000000018030687C movsd     qword ptr [rax + 0x44], xmm0
0000000180306881 movss     dword ptr [rax + 0x4c], xmm2
0000000180306886 cmp       dword ptr [rax + 0x18], 4
000000018030688A jbe       0x180306a72
0000000180306890 movaps    xmm0, xmm3
0000000180306893 unpcklps  xmm0, xmm6
0000000180306896 movsd     qword ptr [rax + 0x50], xmm0
000000018030689B movss     dword ptr [rax + 0x58], xmm2
00000001803068A0 cmp       dword ptr [rax + 0x18], 5
00000001803068A4 jbe       0x180306a72
00000001803068AA movaps    xmm0, xmm3
00000001803068AD unpcklps  xmm0, xmm5
00000001803068B0 movsd     qword ptr [rax + 0x5c], xmm0
00000001803068B5 movss     dword ptr [rax + 0x64], xmm2
00000001803068BA test      ebp, 0xfffffffd
00000001803068C0 je        0x1803068c7
00000001803068C2 cmp       ebp, 4
00000001803068C5 jne       0x1803068e1
00000001803068C7 mov       rax, qword ptr [rdi + 0x20]
00000001803068CB test      rax, rax
00000001803068CE je        0x180306a6c
00000001803068D4 cmp       dword ptr [rax + 0x100], 3
00000001803068DB jl        0x180306a42
00000001803068E1 cmp       byte ptr [rip + 0x19de24c], 0 # 0x181ce4b34
00000001803068E8 jne       0x1803068fd
00000001803068EA lea       rcx, [rip + 0x18aaf9f] # 0x181bb1890
00000001803068F1 call      0x18024fef0
00000001803068F6 mov       byte ptr [rip + 0x19de237], 1 # 0x181ce4b34
00000001803068FD mov       rax, qword ptr [rip + 0x18aaf8c] # 0x181bb1890
0000000180306904 mov       rcx, qword ptr [rax + 0xb8]
000000018030690B mov       rcx, qword ptr [rcx]
000000018030690E test      rcx, rcx
0000000180306911 je        0x180306a6c
0000000180306917 xor       r8d, r8d
000000018030691A mov       edx, 0xb
000000018030691F call      0x180321f30
0000000180306924 mov       rcx, qword ptr [rdi + 0x20]
0000000180306928 mov       rsi, rax
000000018030692B test      rcx, rcx
000000018030692E je        0x180306a6c
0000000180306934 xor       edx, edx
0000000180306936 call      0x181304510
000000018030693B test      rax, rax
000000018030693E je        0x180306a6c
0000000180306944 xor       r8d, r8d
0000000180306947 lea       rcx, [rsp + 0x20]
000000018030694C mov       rdx, rax
000000018030694F call      0x18132f3a0
0000000180306954 cmp       ebp, dword ptr [rbx + 0x18]
0000000180306957 jae       0x180306a72
000000018030695D movsd     xmm1, qword ptr [rax]
0000000180306961 lea       rdx, [rbp*2]
0000000180306969 add       rdx, rbp
000000018030696C movaps    xmm0, xmm1
000000018030696F shufps    xmm0, xmm0, 0x55
0000000180306973 movsd     xmm7, qword ptr [rbx + rdx*4 + 0x20]
0000000180306979 movss     xmm6, dword ptr [rbx + rdx*4 + 0x24]
000000018030697F addss     xmm7, xmm1
0000000180306983 movss     xmm8, dword ptr [rbx + rdx*4 + 0x28]
000000018030698A addss     xmm6, xmm0
000000018030698E addss     xmm8, dword ptr [rax + 8]
0000000180306994 mov       rcx, qword ptr [rip + 0x18a9b65] # 0x181bb0500
000000018030699B cmp       dword ptr [rcx + 0xe0], 0
00000001803069A2 jne       0x1803069a9
00000001803069A4 call      0x1802501e0
00000001803069A9 xor       edx, edx
00000001803069AB mov       rcx, rsi
00000001803069AE call      0x18131f870
00000001803069B3 test      al, al
00000001803069B5 je        0x180306a42
00000001803069BB test      rsi, rsi
00000001803069BE je        0x180306a6c
00000001803069C4 xor       edx, edx
00000001803069C6 mov       rcx, rsi
00000001803069C9 call      0x18131c070
00000001803069CE test      rax, rax
00000001803069D1 je        0x180306a6c
00000001803069D7 unpcklps  xmm7, xmm6
00000001803069DA lea       rdx, [rsp + 0x20]
00000001803069DF xor       r8d, r8d
00000001803069E2 movsd     qword ptr [rsp + 0x20], xmm7
00000001803069E8 mov       rcx, rax
00000001803069EB movss     dword ptr [rsp + 0x28], xmm8
00000001803069F2 call      0x18132fbb0
00000001803069F7 mov       rdx, qword ptr [rip + 0x189e4ba] # 0x181ba4eb8
00000001803069FE mov       rcx, rsi
0000000180306A01 call      0x18046dc80
0000000180306A06 mov       rcx, qword ptr [rip + 0x18a9af3] # 0x181bb0500
0000000180306A0D mov       rbx, rax
0000000180306A10 cmp       dword ptr [rcx + 0xe0], 0
0000000180306A17 jne       0x180306a1e
0000000180306A19 call      0x1802501e0
0000000180306A1E xor       edx, edx
0000000180306A20 mov       rcx, rbx
0000000180306A23 call      0x18131f870
0000000180306A28 test      al, al
0000000180306A2A je        0x180306a42
0000000180306A2C mov       rdx, qword ptr [rdi + 0x20]
0000000180306A30 test      rbx, rbx
0000000180306A33 je        0x180306a6c
0000000180306A35 lea       rcx, [rbx + 0x20]
0000000180306A39 mov       qword ptr [rbx + 0x20], rdx
0000000180306A3D call      0x18024f360
0000000180306A42 movaps    xmm7, xmmword ptr [rsp + 0x50]
0000000180306A47 movaps    xmm6, xmmword ptr [rsp + 0x60]
0000000180306A4C mov       rsi, qword ptr [rsp + 0x80]
0000000180306A54 movaps    xmm8, xmmword ptr [rsp + 0x40]
0000000180306A5A lea       r11, [rsp + 0x70]
0000000180306A5F mov       rbx, qword ptr [r11 + 0x18]
0000000180306A63 mov       rbp, qword ptr [r11 + 0x20]
0000000180306A67 mov       rsp, r11
0000000180306A6A pop       rdi
0000000180306A6B ret       
0000000180306A6C call      0x180250150
0000000180306A71 int3      
0000000180306A72 call      0x180250140
0000000180306A77 int3      