0000000180343640 push      rbx
0000000180343642 sub       rsp, 0x50
0000000180343646 cmp       byte ptr [rip + 0x19a1662], 0 # 0x181ce4caf
000000018034364D mov       rbx, rcx
0000000180343650 jne       0x180343665
0000000180343652 lea       rcx, [rip + 0x187f717] # 0x181bc2d70
0000000180343659 call      0x18024fef0
000000018034365E mov       byte ptr [rip + 0x19a164a], 1 # 0x181ce4caf
0000000180343665 cmp       byte ptr [rbx + 0x104], 0
000000018034366C jne       0x18034388a
0000000180343672 mov       qword ptr [rsp + 0x60], rsi
0000000180343677 mov       qword ptr [rsp + 0x68], rdi
000000018034367C mov       rcx, qword ptr [rbx + 0xf0]
0000000180343683 test      rcx, rcx
0000000180343686 je        0x180343890
000000018034368C xor       edx, edx
000000018034368E call      0x1803ab3f0
0000000180343693 mov       rcx, qword ptr [rbx + 0xf0]
000000018034369A mov       edi, eax
000000018034369C test      rcx, rcx
000000018034369F je        0x180343890
00000001803436A5 mov       rcx, qword ptr [rcx + 0x40]
00000001803436A9 test      rcx, rcx
00000001803436AC je        0x180343890
00000001803436B2 xor       edx, edx
00000001803436B4 call      0x1803a6e50
00000001803436B9 cmp       edi, eax
00000001803436BB jge       0x180343880
00000001803436C1 mov       rcx, qword ptr [rbx + 0x78]
00000001803436C5 mov       byte ptr [rbx + 0x104], 1
00000001803436CC test      rcx, rcx
00000001803436CF je        0x180343890
00000001803436D5 mov       rdx, qword ptr [rip + 0x187f694] # 0x181bc2d70
00000001803436DC call      0x18042e9f0
00000001803436E1 mov       rsi, rax
00000001803436E4 test      rax, rax
00000001803436E7 je        0x180343890
00000001803436ED mov       r8, qword ptr [rax]
00000001803436F0 lea       rcx, [rsp + 0x20]
00000001803436F5 movaps    xmmword ptr [rsp + 0x40], xmm6
00000001803436FA mov       rdx, rax
00000001803436FD movaps    xmmword ptr [rsp + 0x30], xmm7
0000000180343702 mov       r9, qword ptr [r8 + 0x298]
0000000180343709 mov       r8, qword ptr [r8 + 0x2a0]
0000000180343710 call      r9
0000000180343713 mov       r8, qword ptr [rsi]
0000000180343716 lea       rcx, [rsp + 0x20]
000000018034371B mov       rdx, rsi
000000018034371E movups    xmm7, xmmword ptr [rax]
0000000180343721 mov       rax, qword ptr [r8 + 0x298]
0000000180343728 mov       r8, qword ptr [r8 + 0x2a0]
000000018034372F call      rax
0000000180343731 mov       r8, qword ptr [rsi]
0000000180343734 lea       rcx, [rsp + 0x20]
0000000180343739 mov       rdx, rsi
000000018034373C movups    xmm6, xmmword ptr [rax]
000000018034373F mov       rax, qword ptr [r8 + 0x298]
0000000180343746 mov       r8, qword ptr [r8 + 0x2a0]
000000018034374D call      rax
000000018034374F shufps    xmm6, xmm6, 0x55
0000000180343753 lea       rdx, [rsp + 0x20]
0000000180343758 mov       rcx, rsi
000000018034375B movss     xmm1, dword ptr [rax + 8]
0000000180343760 mov       rax, qword ptr [rsi]
0000000180343763 mov       dword ptr [rsp + 0x2c], 0x3f800000
000000018034376B movaps    xmm0, xmmword ptr [rsp + 0x20]
0000000180343770 movss     xmm0, xmm7
0000000180343774 mov       r8, qword ptr [rax + 0x2b0]
000000018034377B shufps    xmm0, xmm0, 0xe1
000000018034377F movss     xmm0, xmm6
0000000180343783 shufps    xmm0, xmm0, 0xc6
0000000180343787 movss     xmm0, xmm1
000000018034378B shufps    xmm0, xmm0, 0xc9
000000018034378F movaps    xmmword ptr [rsp + 0x20], xmm0
0000000180343794 movdqa    xmmword ptr [rsp + 0x20], xmm0
000000018034379A call      qword ptr [rax + 0x2a8]
00000001803437A0 mov       rcx, qword ptr [rbx + 0x78]
00000001803437A4 movaps    xmm7, xmmword ptr [rsp + 0x30]
00000001803437A9 movaps    xmm6, xmmword ptr [rsp + 0x40]
00000001803437AE test      rcx, rcx
00000001803437B1 je        0x180343890
00000001803437B7 xor       edx, edx
00000001803437B9 call      0x181304510
00000001803437BE test      rax, rax
00000001803437C1 je        0x180343890
00000001803437C7 xor       r8d, r8d
00000001803437CA xor       edx, edx
00000001803437CC mov       rcx, rax
00000001803437CF call      0x18132dbe0
00000001803437D4 test      rax, rax
00000001803437D7 je        0x180343890
00000001803437DD xor       edx, edx
00000001803437DF mov       rcx, rax
00000001803437E2 call      0x1813044d0
00000001803437E7 test      rax, rax
00000001803437EA je        0x180343890
00000001803437F0 xor       r8d, r8d
00000001803437F3 mov       dl, 1
00000001803437F5 mov       rcx, rax
00000001803437F8 call      0x18131bad0
00000001803437FD mov       rcx, qword ptr [rbx + 0xe8]
0000000180343804 test      rcx, rcx
0000000180343807 je        0x180343890
000000018034380D xor       edx, edx
000000018034380F call      0x1813044d0
0000000180343814 test      rax, rax
0000000180343817 je        0x180343890
0000000180343819 xor       r8d, r8d
000000018034381C mov       dl, 1
000000018034381E mov       rcx, rax
0000000180343821 call      0x18131bad0
0000000180343826 lea       eax, [rdi + 1]
0000000180343829 xor       edx, edx
000000018034382B mov       rdi, qword ptr [rbx + 0xe8]
0000000180343832 lea       rcx, [rbx + 0x108]
0000000180343839 mov       dword ptr [rbx + 0x108], eax
000000018034383F call      0x180ca3820
0000000180343844 test      rdi, rdi
0000000180343847 je        0x180343890
0000000180343849 mov       r9, qword ptr [rdi]
000000018034384C mov       rdx, rax
000000018034384F mov       rcx, rdi
0000000180343852 mov       r8, qword ptr [r9 + 0x560]
0000000180343859 call      qword ptr [r9 + 0x558]
0000000180343860 mov       rcx, qword ptr [rbx + 0xf0]
0000000180343867 test      rcx, rcx
000000018034386A je        0x180343890
000000018034386C mov       rcx, qword ptr [rcx + 0x50]
0000000180343870 test      rcx, rcx
0000000180343873 je        0x180343890
0000000180343875 xor       r8d, r8d
0000000180343878 mov       rdx, rbx
000000018034387B call      0x1803a1c60
0000000180343880 mov       rsi, qword ptr [rsp + 0x60]
0000000180343885 mov       rdi, qword ptr [rsp + 0x68]
000000018034388A add       rsp, 0x50
000000018034388E pop       rbx
000000018034388F ret       
0000000180343890 call      0x180250150
0000000180343895 int3      