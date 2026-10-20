000000018036F710 mov       qword ptr [rsp + 8], rbx
000000018036F715 mov       qword ptr [rsp + 0x10], rsi
000000018036F71A mov       qword ptr [rsp + 0x18], rdi
000000018036F71F push      r12
000000018036F721 push      r14
000000018036F723 push      r15
000000018036F725 sub       rsp, 0x70
000000018036F729 mov       rsi, rcx
000000018036F72C cmp       byte ptr [rip + 0x1975663], 0 # 0x181ce4d96
000000018036F733 jne       0x18036f79c
000000018036F735 lea       rcx, [rip + 0x182d88c] # 0x181b9cfc8
000000018036F73C call      0x18024fef0
000000018036F741 lea       rcx, [rip + 0x1854030] # 0x181bc3778
000000018036F748 call      0x18024fef0
000000018036F74D lea       rcx, [rip + 0x185407c] # 0x181bc37d0
000000018036F754 call      0x18024fef0
000000018036F759 lea       rcx, [rip + 0x18540c8] # 0x181bc3828
000000018036F760 call      0x18024fef0
000000018036F765 lea       rcx, [rip + 0x183d524] # 0x181bacc90
000000018036F76C call      0x18024fef0
000000018036F771 lea       rcx, [rip + 0x1839a50] # 0x181ba91c8
000000018036F778 call      0x18024fef0
000000018036F77D lea       rcx, [rip + 0x183fedc] # 0x181baf660
000000018036F784 call      0x18024fef0
000000018036F789 lea       rcx, [rip + 0x1840d70] # 0x181bb0500
000000018036F790 call      0x18024fef0
000000018036F795 mov       byte ptr [rip + 0x19755fa], 1 # 0x181ce4d96
000000018036F79C mov       rax, qword ptr [rsi + 0x20]
000000018036F7A0 test      rax, rax
000000018036F7A3 je        0x18036f98b
000000018036F7A9 mov       rdi, qword ptr [rax + 0x30]
000000018036F7AD mov       r15, qword ptr [rax + 0x108]
000000018036F7B4 test      r15, r15
000000018036F7B7 je        0x18036f98b
000000018036F7BD mov       rcx, qword ptr [r15 + 0x28]
000000018036F7C1 test      rcx, rcx
000000018036F7C4 je        0x18036f98b
000000018036F7CA mov       r8, qword ptr [rip + 0x18399f7] # 0x181ba91c8
000000018036F7D1 mov       edx, 0xd
000000018036F7D6 call      0x180825710
000000018036F7DB mov       r14, rax
000000018036F7DE xor       r12d, r12d
000000018036F7E1 mov       ebx, r12d
000000018036F7E4 cmp       dword ptr [rsi + 0x80], 2
000000018036F7EB setl      bl
000000018036F7EE test      rdi, rdi
000000018036F7F1 je        0x18036f98b
000000018036F7F7 xor       edx, edx
000000018036F7F9 mov       rcx, rdi
000000018036F7FC call      0x18032c3b0
000000018036F801 lea       r8d, [rax - 2]
000000018036F805 mov       rax, qword ptr [rip + 0x183fe54] # 0x181baf660
000000018036F80C mov       qword ptr [rsp + 0x28], rax
000000018036F811 mov       dword ptr [rsp + 0x20], r8d
000000018036F816 mov       r9d, 4
000000018036F81C mov       r8, r14
000000018036F81F lea       edx, [rbx + 2]
000000018036F822 mov       rcx, rdi
000000018036F825 call      0x1804910b0
000000018036F82A test      rax, rax
000000018036F82D je        0x18036f98b
000000018036F833 mov       r8, qword ptr [rip + 0x183d456] # 0x181bacc90
000000018036F83A mov       rdx, rax
000000018036F83D lea       rcx, [rsp + 0x38]
000000018036F842 call      0x1808197f0
000000018036F847 movups    xmm0, xmmword ptr [rsp + 0x38]
000000018036F84C movups    xmmword ptr [rsp + 0x50], xmm0
000000018036F851 movsd     xmm1, qword ptr [rsp + 0x48]
000000018036F857 movsd     qword ptr [rsp + 0x60], xmm1
000000018036F85D mov       qword ptr [rsp + 0x38], r12
000000018036F862 lea       rbx, [rsp + 0x50]
000000018036F867 mov       qword ptr [rsp + 0x40], rbx
000000018036F86C nop       dword ptr [rax]
000000018036F870 mov       rdx, qword ptr [rip + 0x1853f59] # 0x181bc37d0
000000018036F877 lea       rcx, [rsp + 0x50]
000000018036F87C call      0x180669280
000000018036F881 test      al, al
000000018036F883 je        0x18036f92d
000000018036F889 mov       r14, qword ptr [rsp + 0x60]
000000018036F88E mov       rcx, qword ptr [rip + 0x182d733] # 0x181b9cfc8
000000018036F895 call      0x180250100
000000018036F89A mov       rdi, rax
000000018036F89D xor       r8d, r8d
000000018036F8A0 mov       edx, 0xd
000000018036F8A5 mov       rcx, rax
000000018036F8A8 call      0x180345e10
000000018036F8AD mov       eax, dword ptr [rsi + 0xfc]
000000018036F8B3 test      rdi, rdi
000000018036F8B6 je        0x18036f97f
000000018036F8BC mov       dword ptr [rdi + 0x18], eax
000000018036F8BF test      r14, r14
000000018036F8C2 je        0x18036f97a
000000018036F8C8 mov       qword ptr [rsp + 0x20], r12
000000018036F8CD mov       r9d, dword ptr [r14 + 0x20]
000000018036F8D1 mov       r8d, dword ptr [r14 + 0x1c]
000000018036F8D5 mov       rdx, rdi
000000018036F8D8 mov       rcx, r15
000000018036F8DB call      0x180312e90
000000018036F8E0 mov       rdi, rax
000000018036F8E3 mov       rcx, qword ptr [rip + 0x1840c16] # 0x181bb0500
000000018036F8EA cmp       dword ptr [rcx + 0xe0], 0
000000018036F8F1 jne       0x18036f8f8
000000018036F8F3 call      0x1802501e0
000000018036F8F8 xor       r8d, r8d
000000018036F8FB xor       edx, edx
000000018036F8FD mov       rcx, rdi
000000018036F900 call      0x18131f900
000000018036F905 test      al, al
000000018036F907 je        0x18036f870
000000018036F90D test      rdi, rdi
000000018036F910 je        0x18036f974
000000018036F912 mov       qword ptr [rdi + 0xe8], rsi
000000018036F919 lea       rcx, [rdi + 0xe8]
000000018036F920 mov       rdx, rsi
000000018036F923 call      0x18024f360
000000018036F928 jmp       0x18036f870
000000018036F92D mov       rdx, qword ptr [rip + 0x1853e44] # 0x181bc3778
000000018036F934 mov       rcx, rbx
000000018036F937 call      0x180302170
000000018036F93C jmp       0x18036f959
000000018036F93E mov       rdx, qword ptr [rip + 0x1853e33] # 0x181bc3778
000000018036F945 mov       rcx, qword ptr [rsp + 0x40]
000000018036F94A call      0x180302170
000000018036F94F mov       rcx, qword ptr [rsp + 0x38]
000000018036F954 test      rcx, rcx
000000018036F957 jne       0x18036f985
000000018036F959 lea       r11, [rsp + 0x70]
000000018036F95E mov       rbx, qword ptr [r11 + 0x20]
000000018036F962 mov       rsi, qword ptr [r11 + 0x28]
000000018036F966 mov       rdi, qword ptr [r11 + 0x30]
000000018036F96A mov       rsp, r11
000000018036F96D pop       r15
000000018036F96F pop       r14
000000018036F971 pop       r12
000000018036F973 ret       
000000018036F974 call      0x180250150
000000018036F979 nop       
000000018036F97A call      0x180250150
000000018036F97F call      0x180250150
000000018036F984 nop       
000000018036F985 call      0x180200e60
000000018036F98A int3      
000000018036F98B call      0x180250150
000000018036F990 int3      