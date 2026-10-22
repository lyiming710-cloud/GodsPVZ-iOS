00000001803547C0 mov       qword ptr [rsp + 0x20], rbx
00000001803547C5 push      rdi
00000001803547C6 sub       rsp, 0x100
00000001803547CD cmp       byte ptr [rip + 0x1990548], 0 # 0x181ce4d1c
00000001803547D4 mov       rbx, rcx
00000001803547D7 jne       0x1803547f8
00000001803547D9 lea       rcx, [rip + 0x1858418] # 0x181bacbf8
00000001803547E0 call      0x18024fef0
00000001803547E5 lea       rcx, [rip + 0x1867194] # 0x181bbb980
00000001803547EC call      0x18024fef0
00000001803547F1 mov       byte ptr [rip + 0x1990524], 1 # 0x181ce4d1c
00000001803547F8 mov       rdi, qword ptr [rbx + 0x1e8]
00000001803547FF mov       qword ptr [rsp + 0x110], rbp
0000000180354807 mov       qword ptr [rsp + 0x118], rsi
000000018035480F mov       qword ptr [rsp + 0x120], r14
0000000180354817 movaps    xmmword ptr [rsp + 0xf0], xmm6
000000018035481F movaps    xmmword ptr [rsp + 0xe0], xmm7
0000000180354827 movaps    xmmword ptr [rsp + 0xd0], xmm8
0000000180354830 movaps    xmmword ptr [rsp + 0xc0], xmm9
0000000180354839 movaps    xmmword ptr [rsp + 0xb0], xmm10
0000000180354842 movaps    xmmword ptr [rsp + 0xa0], xmm11
000000018035484B movaps    xmmword ptr [rsp + 0x90], xmm12
0000000180354854 test      rdi, rdi
0000000180354857 je        0x180354b8f
000000018035485D mov       rax, qword ptr [rbx + 0x1e8]
0000000180354864 mov       rdi, qword ptr [rdi + 0x30]
0000000180354868 mov       rdx, qword ptr [rax + 0x28]
000000018035486C test      rdx, rdx
000000018035486F je        0x180354b8f
0000000180354875 xor       r14d, r14d
0000000180354878 lea       rcx, [rsp + 0x80]
0000000180354880 xor       r9d, r9d
0000000180354883 mov       qword ptr [rsp + 0x20], r14
0000000180354888 xor       r8d, r8d
000000018035488B call      0x18030ec30
0000000180354890 movsd     xmm0, qword ptr [rax]
0000000180354894 mov       eax, dword ptr [rax + 8]
0000000180354897 mov       dword ptr [rsp + 0x68], eax
000000018035489B mov       rax, qword ptr [rbx + 0x1e8]
00000001803548A2 movsd     qword ptr [rsp + 0x60], xmm0
00000001803548A8 test      rax, rax
00000001803548AB je        0x180354b8f
00000001803548B1 mov       rsi, qword ptr [rax + 0x28]
00000001803548B5 test      rdi, rdi
00000001803548B8 je        0x180354b8f
00000001803548BE xor       edx, edx
00000001803548C0 mov       rcx, rdi
00000001803548C3 call      0x18032c3b0
00000001803548C8 xor       edx, edx
00000001803548CA mov       rcx, rdi
00000001803548CD mov       ebp, eax
00000001803548CF call      0x18032c440
00000001803548D4 test      rsi, rsi
00000001803548D7 je        0x180354b8f
00000001803548DD lea       r9d, [rax - 1]
00000001803548E1 mov       qword ptr [rsp + 0x20], r14
00000001803548E6 lea       r8d, [rbp - 1]
00000001803548EA mov       rdx, rsi
00000001803548ED lea       rcx, [rsp + 0x80]
00000001803548F5 call      0x18030ec30
00000001803548FA movss     xmm0, dword ptr [rsp + 0x60]
0000000180354900 xor       r8d, r8d
0000000180354903 movsd     xmm6, qword ptr [rax]
0000000180354907 mov       eax, dword ptr [rax + 8]
000000018035490A movaps    xmm1, xmm6
000000018035490D mov       dword ptr [rsp + 0x88], eax
0000000180354914 call      0x18130d350
0000000180354919 movss     xmm1, dword ptr [rsp + 0x64]
000000018035491F movaps    xmm12, xmm0
0000000180354923 movaps    xmm0, xmm6
0000000180354926 movsd     qword ptr [rsp + 0x80], xmm6
000000018035492F shufps    xmm0, xmm0, 0x55
0000000180354933 xor       r8d, r8d
0000000180354936 call      0x18130d350
000000018035493B mov       rcx, qword ptr [rbx + 0x1e8]
0000000180354942 movaps    xmm10, xmm0
0000000180354946 test      rcx, rcx
0000000180354949 je        0x180354b8f
000000018035494F mov       rcx, qword ptr [rcx + 0xf8]
0000000180354956 test      rcx, rcx
0000000180354959 je        0x180354b8f
000000018035495F xor       r8d, r8d
0000000180354962 mov       edx, 0x1e
0000000180354967 call      0x180324150
000000018035496C movss     xmm11, dword ptr [rip + 0x125333b] # 0x1815a7cb0
0000000180354975 xor       r9d, r9d
0000000180354978 mov       r8d, 0x1e
000000018035497E mov       qword ptr [rsp + 0x20], r14
0000000180354983 mov       rdx, rax
0000000180354986 addss     xmm10, xmm11
000000018035498B mov       rcx, rbx
000000018035498E mov       rdi, rax
0000000180354991 xorps     xmm8, xmm8
0000000180354995 call      0x180350e40
000000018035499A xor       r8d, r8d
000000018035499D mov       edx, 5
00000001803549A2 xor       ecx, ecx
00000001803549A4 mov       rbp, rax
00000001803549A7 call      0x18130d310
00000001803549AC mov       esi, eax
00000001803549AE mov       edx, eax
00000001803549B0 test      eax, eax
00000001803549B2 je        0x1803549f0
00000001803549B4 sub       edx, 1
00000001803549B7 je        0x1803549e6
00000001803549B9 sub       edx, 1
00000001803549BC je        0x1803549dc
00000001803549BE sub       edx, 1
00000001803549C1 je        0x1803549d2
00000001803549C3 cmp       edx, 1
00000001803549C6 jne       0x1803549f0
00000001803549C8 movss     xmm6, dword ptr [rip + 0x1253278] # 0x1815a7c48
00000001803549D0 jmp       0x1803549f3
00000001803549D2 movss     xmm6, dword ptr [rip + 0x1253266] # 0x1815a7c40
00000001803549DA jmp       0x1803549f3
00000001803549DC movss     xmm6, dword ptr [rip + 0x12532e0] # 0x1815a7cc4
00000001803549E4 jmp       0x1803549f3
00000001803549E6 movss     xmm6, dword ptr [rip + 0x12532da] # 0x1815a7cc8
00000001803549EE jmp       0x1803549f3
00000001803549F0 xorps     xmm6, xmm6
00000001803549F3 mov       rcx, qword ptr [rip + 0x18581fe] # 0x181bacbf8
00000001803549FA cvtps2pd  xmm7, xmm6
00000001803549FD mulsd     xmm7, qword ptr [rip + 0x125321b] # 0x1815a7c20
0000000180354A05 cmp       dword ptr [rcx + 0xe0], r14d
0000000180354A0C jne       0x180354a13
0000000180354A0E call      0x1802501e0
0000000180354A13 movaps    xmm0, xmm7
0000000180354A16 call      0x1802cc980
0000000180354A1B xorps     xmm9, xmm9
0000000180354A1F cvtsd2ss  xmm9, xmm0
0000000180354A24 movaps    xmm0, xmm7
0000000180354A27 mulss     xmm9, dword ptr [rip + 0x1253284] # 0x1815a7cb4
0000000180354A30 call      0x1802cbe50
0000000180354A35 cmp       byte ptr [rip + 0x19900fa], r14b # 0x181ce4b36
0000000180354A3C xorps     xmm7, xmm7
0000000180354A3F cvtsd2ss  xmm7, xmm0
0000000180354A43 mulss     xmm7, dword ptr [rip + 0x125328d] # 0x1815a7cd8
0000000180354A4B jne       0x180354a60
0000000180354A4D lea       rcx, [rip + 0x186dd94] # 0x181bc27e8
0000000180354A54 call      0x18024fef0
0000000180354A59 mov       byte ptr [rip + 0x19900d6], 1 # 0x181ce4b36
0000000180354A60 mov       rax, qword ptr [rip + 0x186dd81] # 0x181bc27e8
0000000180354A67 mov       rax, qword ptr [rax + 0xb8]
0000000180354A6E test      rdi, rdi
0000000180354A71 je        0x180354b8f
0000000180354A77 movsd     xmm0, qword ptr [rax]
0000000180354A7B lea       r9, [rsp + 0x60]
0000000180354A80 mov       eax, dword ptr [rax + 8]
0000000180354A83 lea       r8, [rsp + 0x70]
0000000180354A88 andps     xmm6, xmmword ptr [rip + 0x12530f1] # 0x1815a7b80
0000000180354A8F lea       rdx, [rsp + 0x80]
0000000180354A97 mov       dword ptr [rsp + 0x68], eax
0000000180354A9B mov       rcx, rdi
0000000180354A9E mov       rax, qword ptr [rip + 0x1866edb] # 0x181bbb980
0000000180354AA5 mov       qword ptr [rsp + 0x58], rax
0000000180354AAA mov       qword ptr [rsp + 0x50], rbx
0000000180354AAF movss     dword ptr [rsp + 0x48], xmm8
0000000180354AB6 movss     dword ptr [rsp + 0x40], xmm8
0000000180354ABD movss     dword ptr [rsp + 0x38], xmm6
0000000180354AC3 mov       dword ptr [rsp + 0x30], r14d
0000000180354AC8 unpcklps  xmm9, xmm8
0000000180354ACC unpcklps  xmm12, xmm10
0000000180354AD0 movss     dword ptr [rsp + 0x28], xmm7
0000000180354AD6 movss     dword ptr [rsp + 0x20], xmm11
0000000180354ADD movss     dword ptr [rsp + 0x78], xmm8
0000000180354AE4 movss     dword ptr [rsp + 0x88], xmm8
0000000180354AEE movsd     qword ptr [rsp + 0x60], xmm0
0000000180354AF4 movsd     qword ptr [rsp + 0x70], xmm9
0000000180354AFB movsd     qword ptr [rsp + 0x80], xmm12
0000000180354B05 call      0x1804a2700
0000000180354B0A mov       r8d, dword ptr [rbx + 0xbc]
0000000180354B11 xor       r9d, r9d
0000000180354B14 mov       rdx, rbp
0000000180354B17 mov       rcx, rdi
0000000180354B1A call      0x18037c0f0
0000000180354B1F movaps    xmm12, xmmword ptr [rsp + 0x90]
0000000180354B28 movaps    xmm11, xmmword ptr [rsp + 0xa0]
0000000180354B31 movaps    xmm10, xmmword ptr [rsp + 0xb0]
0000000180354B3A movaps    xmm9, xmmword ptr [rsp + 0xc0]
0000000180354B43 movaps    xmm8, xmmword ptr [rsp + 0xd0]
0000000180354B4C movaps    xmm7, xmmword ptr [rsp + 0xe0]
0000000180354B54 movaps    xmm6, xmmword ptr [rsp + 0xf0]
0000000180354B5C mov       rbp, qword ptr [rsp + 0x110]
0000000180354B64 mov       dword ptr [rdi + 0x74], esi
0000000180354B67 mov       rsi, qword ptr [rsp + 0x118]
0000000180354B6F mov       dword ptr [rbx + 0x214], r14d
0000000180354B76 mov       r14, qword ptr [rsp + 0x120]
0000000180354B7E mov       rbx, qword ptr [rsp + 0x128]
0000000180354B86 add       rsp, 0x100
0000000180354B8D pop       rdi
0000000180354B8E ret       
0000000180354B8F call      0x180250150
0000000180354B94 int3      
0000000180354B95 int3      
0000000180354B96 int3      
0000000180354B97 int3      
0000000180354B98 int3      
0000000180354B99 int3      
0000000180354B9A int3      
0000000180354B9B int3      
0000000180354B9C int3      
0000000180354B9D int3      
0000000180354B9E int3      
0000000180354B9F int3      