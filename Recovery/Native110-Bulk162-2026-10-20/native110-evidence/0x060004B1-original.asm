0000000180372720 mov       qword ptr [rsp + 8], rbx
0000000180372725 push      rdi
0000000180372726 sub       rsp, 0xa0
000000018037272D cmp       byte ptr [rip + 0x197267d], 0 # 0x181ce4db1
0000000180372734 mov       rdi, rdx
0000000180372737 mov       rbx, rcx
000000018037273A jne       0x180372773
000000018037273C lea       rcx, [rip + 0x185062d] # 0x181bc2d70
0000000180372743 call      0x18024fef0
0000000180372748 lea       rcx, [rip + 0x1831fe9] # 0x181ba4738
000000018037274F call      0x18024fef0
0000000180372754 lea       rcx, [rip + 0x183dda5] # 0x181bb0500
000000018037275B call      0x18024fef0
0000000180372760 lea       rcx, [rip + 0x183fee1] # 0x181bb2648
0000000180372767 call      0x18024fef0
000000018037276C mov       byte ptr [rip + 0x197263e], 1 # 0x181ce4db1
0000000180372773 xor       eax, eax
0000000180372775 movaps    xmmword ptr [rsp + 0x90], xmm6
000000018037277D xorps     xmm0, xmm0
0000000180372780 mov       dword ptr [rsp + 0x60], eax
0000000180372784 mov       rax, qword ptr [rbx + 0x1f8]
000000018037278B movups    xmmword ptr [rsp + 0x40], xmm0
0000000180372790 movups    xmmword ptr [rsp + 0x50], xmm0
0000000180372795 test      rax, rax
0000000180372798 je        0x180372a22
000000018037279E cmp       dword ptr [rax + 0x18], 0
00000001803727A2 jbe       0x180372a28
00000001803727A8 mov       rcx, qword ptr [rax + 0x20]
00000001803727AC test      rcx, rcx
00000001803727AF je        0x180372a22
00000001803727B5 movzx     edx, byte ptr [rbx + 0xd0]
00000001803727BC xor       r8d, r8d
00000001803727BF call      0x18131bad0
00000001803727C4 mov       rax, qword ptr [rbx + 0x1f8]
00000001803727CB test      rax, rax
00000001803727CE je        0x180372a22
00000001803727D4 cmp       dword ptr [rax + 0x18], 0
00000001803727D8 jbe       0x180372a28
00000001803727DE mov       rcx, qword ptr [rax + 0x20]
00000001803727E2 test      rcx, rcx
00000001803727E5 je        0x180372a22
00000001803727EB xor       edx, edx
00000001803727ED call      0x18131c070
00000001803727F2 test      rax, rax
00000001803727F5 je        0x180372a22
00000001803727FB mov       ecx, dword ptr [rdi + 8]
00000001803727FE lea       rdx, [rsp + 0x30]
0000000180372803 movsd     xmm0, qword ptr [rdi]
0000000180372807 xor       r8d, r8d
000000018037280A mov       dword ptr [rsp + 0x38], ecx
000000018037280E mov       rcx, rax
0000000180372811 movsd     qword ptr [rsp + 0x30], xmm0
0000000180372817 call      0x18132fbb0
000000018037281C mov       rcx, qword ptr [rip + 0x183dcdd] # 0x181bb0500
0000000180372823 mov       rdi, qword ptr [rbx + 0x158]
000000018037282A cmp       dword ptr [rcx + 0xe0], 0
0000000180372831 jne       0x180372838
0000000180372833 call      0x1802501e0
0000000180372838 xor       r8d, r8d
000000018037283B xor       edx, edx
000000018037283D mov       rcx, rdi
0000000180372840 call      0x18131f900
0000000180372845 test      al, al
0000000180372847 je        0x1803729c5
000000018037284D mov       rcx, qword ptr [rbx + 0x158]
0000000180372854 test      rcx, rcx
0000000180372857 je        0x180372a22
000000018037285D xor       r8d, r8d
0000000180372860 xor       edx, edx
0000000180372862 call      0x1812db740
0000000180372867 test      rax, rax
000000018037286A je        0x180372a22
0000000180372870 cmp       qword ptr [rax + 0x18], 0
0000000180372875 je        0x1803729c5
000000018037287B mov       rdx, qword ptr [rbx + 0x158]
0000000180372882 test      rdx, rdx
0000000180372885 je        0x180372a22
000000018037288B xor       r9d, r9d
000000018037288E lea       rcx, [rsp + 0x68]
0000000180372893 xor       r8d, r8d
0000000180372896 call      0x1812db780
000000018037289B xor       edx, edx
000000018037289D lea       rcx, [rsp + 0x40]
00000001803728A2 movups    xmm0, xmmword ptr [rax]
00000001803728A5 movups    xmmword ptr [rsp + 0x40], xmm0
00000001803728AA movups    xmm1, xmmword ptr [rax + 0x10]
00000001803728AE movups    xmmword ptr [rsp + 0x50], xmm1
00000001803728B3 mov       eax, dword ptr [rax + 0x20]
00000001803728B6 mov       dword ptr [rsp + 0x60], eax
00000001803728BA call      0x1802fb140
00000001803728BF mov       rcx, qword ptr [rbx + 0x1f8]
00000001803728C6 movaps    xmm6, xmm0
00000001803728C9 test      rcx, rcx
00000001803728CC je        0x180372a22
00000001803728D2 cmp       dword ptr [rcx + 0x18], 0
00000001803728D6 jbe       0x180372a28
00000001803728DC mov       rcx, qword ptr [rcx + 0x20]
00000001803728E0 test      rcx, rcx
00000001803728E3 je        0x180372a22
00000001803728E9 xor       edx, edx
00000001803728EB call      0x18131c070
00000001803728F0 test      rax, rax
00000001803728F3 je        0x180372a22
00000001803728F9 xor       r8d, r8d
00000001803728FC mov       edx, 1
0000000180372901 mov       rcx, rax
0000000180372904 call      0x18132dbe0
0000000180372909 test      rax, rax
000000018037290C je        0x180372a22
0000000180372912 mov       rdx, qword ptr [rip + 0x1850457] # 0x181bc2d70
0000000180372919 mov       rcx, rax
000000018037291C call      0x18042e9f0
0000000180372921 mov       rdi, rax
0000000180372924 test      rax, rax
0000000180372927 je        0x180372a22
000000018037292D movss     xmm1, dword ptr [rip + 0x12350db] # 0x1815a7a10
0000000180372935 movaps    xmm0, xmm6
0000000180372938 call      0x1802cd5e8
000000018037293D movaps    xmm1, xmm0
0000000180372940 xor       r8d, r8d
0000000180372943 mov       rcx, rdi
0000000180372946 call      0x1813d7750
000000018037294B mov       rax, qword ptr [rbx + 0x1f8]
0000000180372952 test      rax, rax
0000000180372955 je        0x180372a22
000000018037295B cmp       dword ptr [rax + 0x18], 0
000000018037295F jbe       0x180372a28
0000000180372965 mov       rcx, qword ptr [rax + 0x20]
0000000180372969 test      rcx, rcx
000000018037296C je        0x180372a22
0000000180372972 mov       rdx, qword ptr [rip + 0x1831dbf] # 0x181ba4738
0000000180372979 call      0x18046dc80
000000018037297E mov       rcx, qword ptr [rip + 0x183db7b] # 0x181bb0500
0000000180372985 mov       rdi, rax
0000000180372988 cmp       dword ptr [rcx + 0xe0], 0
000000018037298F jne       0x180372996
0000000180372991 call      0x1802501e0
0000000180372996 xor       r8d, r8d
0000000180372999 xor       edx, edx
000000018037299B mov       rcx, rdi
000000018037299E call      0x18131f900
00000001803729A3 test      al, al
00000001803729A5 je        0x1803729c5
00000001803729A7 test      rdi, rdi
00000001803729AA je        0x180372a22
00000001803729AC movaps    xmm3, xmm6
00000001803729AF mov       qword ptr [rsp + 0x20], 0
00000001803729B8 xor       r8d, r8d
00000001803729BB xor       edx, edx
00000001803729BD mov       rcx, rdi
00000001803729C0 call      0x1812db860
00000001803729C5 cmp       byte ptr [rbx + 0xd0], 0
00000001803729CC je        0x180372a09
00000001803729CE cmp       byte ptr [rbx + 0xcf], 0
00000001803729D5 je        0x180372a09
00000001803729D7 cmp       byte ptr [rbx + 0xd1], 0
00000001803729DE jne       0x180372a09
00000001803729E0 mov       rcx, qword ptr [rbx + 0x158]
00000001803729E7 test      rcx, rcx
00000001803729EA je        0x180372a22
00000001803729EC mov       rdx, qword ptr [rip + 0x183fc55] # 0x181bb2648
00000001803729F3 xor       r8d, r8d
00000001803729F6 call      0x1812dbc00
00000001803729FB mov       byte ptr [rbx + 0xcf], 0
0000000180372A02 mov       byte ptr [rbx + 0xd1], 1
0000000180372A09 movaps    xmm6, xmmword ptr [rsp + 0x90]
0000000180372A11 mov       rbx, qword ptr [rsp + 0xb0]
0000000180372A19 add       rsp, 0xa0
0000000180372A20 pop       rdi
0000000180372A21 ret       
0000000180372A22 call      0x180250150
0000000180372A27 int3      
0000000180372A28 call      0x180250140
0000000180372A2D int3      