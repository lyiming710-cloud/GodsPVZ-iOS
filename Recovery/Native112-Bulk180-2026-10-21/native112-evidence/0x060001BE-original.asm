0000000180318730 push      rsi
0000000180318732 push      r13
0000000180318734 sub       rsp, 0x58
0000000180318738 cmp       byte ptr [rip + 0x19cc47d], 0 # 0x181ce4bbc
000000018031873F mov       rsi, rdx
0000000180318742 mov       r13, rcx
0000000180318745 jne       0x18031878a
0000000180318747 lea       rcx, [rip + 0x18860da] # 0x181b9e828
000000018031874E call      0x18024fef0
0000000180318753 lea       rcx, [rip + 0x1891ca6] # 0x181baa400
000000018031875A call      0x18024fef0
000000018031875F lea       rcx, [rip + 0x189fa8a] # 0x181bb81f0
0000000180318766 call      0x18024fef0
000000018031876B lea       rcx, [rip + 0x1894626] # 0x181bacd98
0000000180318772 call      0x18024fef0
0000000180318777 lea       rcx, [rip + 0x189447a] # 0x181bacbf8
000000018031877E call      0x18024fef0
0000000180318783 mov       byte ptr [rip + 0x19cc432], 1 # 0x181ce4bbc
000000018031878A mov       rax, qword ptr [r13 + 0x80]
0000000180318791 mov       qword ptr [rsp + 0x78], rbx
0000000180318796 mov       qword ptr [rsp + 0x50], rbp
000000018031879B mov       qword ptr [rsp + 0x48], rdi
00000001803187A0 mov       qword ptr [rsp + 0x40], r12
00000001803187A5 mov       qword ptr [rsp + 0x38], r14
00000001803187AA mov       qword ptr [rsp + 0x30], r15
00000001803187AF test      rax, rax
00000001803187B2 je        0x180318995
00000001803187B8 mov       rcx, qword ptr [rax + 0x30]
00000001803187BC test      rcx, rcx
00000001803187BF je        0x180318995
00000001803187C5 xor       edx, edx
00000001803187C7 call      0x18032c3b0
00000001803187CC mov       rcx, qword ptr [r13 + 0x80]
00000001803187D3 mov       r15d, eax
00000001803187D6 test      rcx, rcx
00000001803187D9 je        0x180318995
00000001803187DF mov       rcx, qword ptr [rcx + 0x30]
00000001803187E3 test      rcx, rcx
00000001803187E6 je        0x180318995
00000001803187EC xor       edx, edx
00000001803187EE call      0x18032c440
00000001803187F3 mov       r12d, 0xffffffff
00000001803187F9 mov       dword ptr [rsp + 0x88], eax
0000000180318800 xor       edi, edi
0000000180318802 mov       dword ptr [rsp + 0x70], r12d
0000000180318807 mov       ebp, r12d
000000018031880A mov       r14d, 0x7fffffff
0000000180318810 test      eax, eax
0000000180318812 jle       0x180318908
0000000180318818 nop       dword ptr [rax + rax]
0000000180318820 xor       ebx, ebx
0000000180318822 test      r15d, r15d
0000000180318825 jle       0x1803188fe
000000018031882B nop       dword ptr [rax + rax]
0000000180318830 mov       rax, qword ptr [r13 + 0x80]
0000000180318837 test      rax, rax
000000018031883A je        0x180318995
0000000180318840 mov       rcx, qword ptr [rax + 0x30]
0000000180318844 test      rcx, rcx
0000000180318847 je        0x180318995
000000018031884D mov       rcx, qword ptr [rcx + 0x10]
0000000180318851 test      rcx, rcx
0000000180318854 je        0x180318995
000000018031885A mov       r8, qword ptr [rip + 0x189f98f] # 0x181bb81f0
0000000180318861 mov       edx, edi
0000000180318863 call      0x180825710
0000000180318868 test      rax, rax
000000018031886B je        0x180318995
0000000180318871 mov       rcx, qword ptr [rax + 0x18]
0000000180318875 test      rcx, rcx
0000000180318878 je        0x180318995
000000018031887E mov       r8, qword ptr [rip + 0x1894513] # 0x181bacd98
0000000180318885 mov       edx, ebx
0000000180318887 call      0x180825710
000000018031888C test      rax, rax
000000018031888F je        0x180318995
0000000180318895 cmp       byte ptr [rax + 0x14], 0
0000000180318899 je        0x1803188ec
000000018031889B test      rsi, rsi
000000018031889E je        0x180318995
00000001803188A4 mov       rcx, qword ptr [rip + 0x189434d] # 0x181bacbf8
00000001803188AB mov       ebp, dword ptr [rsi + 0x18]
00000001803188AE cmp       dword ptr [rcx + 0xe0], 0
00000001803188B5 jne       0x1803188bc
00000001803188B7 call      0x1802501e0
00000001803188BC mov       eax, edi
00000001803188BE mov       ecx, ebx
00000001803188C0 sub       eax, dword ptr [rsi + 0x14]
00000001803188C3 mov       edx, eax
00000001803188C5 neg       edx
00000001803188C7 cmovs     edx, eax
00000001803188CA sub       ecx, ebp
00000001803188CC mov       eax, ecx
00000001803188CE neg       eax
00000001803188D0 cmovs     eax, ecx
00000001803188D3 add       edx, eax
00000001803188D5 cmp       edx, r14d
00000001803188D8 jge       0x1803188e8
00000001803188DA mov       r12d, ebx
00000001803188DD mov       dword ptr [rsp + 0x70], edi
00000001803188E1 mov       ebp, edi
00000001803188E3 mov       r14d, edx
00000001803188E6 jmp       0x1803188ec
00000001803188E8 mov       ebp, dword ptr [rsp + 0x70]
00000001803188EC inc       ebx
00000001803188EE cmp       ebx, r15d
00000001803188F1 jl        0x180318830
00000001803188F7 mov       eax, dword ptr [rsp + 0x88]
00000001803188FE inc       edi
0000000180318900 cmp       edi, eax
0000000180318902 jl        0x180318820
0000000180318908 mov       rbx, qword ptr [r13 + 0x80]
000000018031890F test      rbx, rbx
0000000180318912 je        0x180318995
0000000180318918 mov       rcx, qword ptr [rip + 0x1885f09] # 0x181b9e828
000000018031891F mov       rbx, qword ptr [rbx + 0x28]
0000000180318923 call      0x180250100
0000000180318928 xorps     xmm3, xmm3
000000018031892B mov       qword ptr [rsp + 0x28], 0
0000000180318934 mov       r8d, ebp
0000000180318937 mov       qword ptr [rsp + 0x20], rbx
000000018031893C mov       edx, r12d
000000018031893F mov       rcx, rax
0000000180318942 mov       rdi, rax
0000000180318945 call      0x1803299e0
000000018031894A test      rdi, rdi
000000018031894D je        0x180318995
000000018031894F mov       byte ptr [rdi + 0x29], 1
0000000180318953 test      rsi, rsi
0000000180318956 je        0x180318995
0000000180318958 mov       rcx, qword ptr [rsi + 0x30]
000000018031895C test      rcx, rcx
000000018031895F je        0x180318995
0000000180318961 mov       r8, qword ptr [rip + 0x1891a98] # 0x181baa400
0000000180318968 mov       rdx, rdi
000000018031896B mov       r15, qword ptr [rsp + 0x30]
0000000180318970 mov       r14, qword ptr [rsp + 0x38]
0000000180318975 mov       r12, qword ptr [rsp + 0x40]
000000018031897A mov       rdi, qword ptr [rsp + 0x48]
000000018031897F mov       rbp, qword ptr [rsp + 0x50]
0000000180318984 mov       rbx, qword ptr [rsp + 0x78]
0000000180318989 add       rsp, 0x58
000000018031898D pop       r13
000000018031898F pop       rsi
0000000180318990 jmp       0x180002e70
0000000180318995 call      0x180250150
000000018031899A int3      