00000001803A3770 mov       qword ptr [rsp + 8], rbx
00000001803A3775 mov       qword ptr [rsp + 0x10], rsi
00000001803A377A mov       qword ptr [rsp + 0x18], rdi
00000001803A377F push      r14
00000001803A3781 sub       rsp, 0x60
00000001803A3785 mov       esi, edx
00000001803A3787 mov       rdi, rcx
00000001803A378A cmp       byte ptr [rip + 0x1941784], 0 # 0x181ce4f15
00000001803A3791 jne       0x1803a37e2
00000001803A3793 lea       rcx, [rip + 0x1825f16] # 0x181bc96b0
00000001803A379A call      0x18024fef0
00000001803A379F lea       rcx, [rip + 0x1825f5a] # 0x181bc9700
00000001803A37A6 call      0x18024fef0
00000001803A37AB lea       rcx, [rip + 0x1825fa6] # 0x181bc9758
00000001803A37B2 call      0x18024fef0
00000001803A37B7 lea       rcx, [rip + 0x181b4f2] # 0x181bbecb0
00000001803A37BE call      0x18024fef0
00000001803A37C3 lea       rcx, [rip + 0x18088be] # 0x181bac088
00000001803A37CA call      0x18024fef0
00000001803A37CF lea       rcx, [rip + 0x180cd2a] # 0x181bb0500
00000001803A37D6 call      0x18024fef0
00000001803A37DB mov       byte ptr [rip + 0x1941733], 1 # 0x181ce4f15
00000001803A37E2 xorps     xmm0, xmm0
00000001803A37E5 xor       eax, eax
00000001803A37E7 movups    xmmword ptr [rsp + 0x40], xmm0
00000001803A37EC mov       qword ptr [rsp + 0x50], rax
00000001803A37F1 mov       rcx, qword ptr [rdi + 0x48]
00000001803A37F5 test      rcx, rcx
00000001803A37F8 je        0x1803a3a54
00000001803A37FE lea       edx, [rsi - 1]
00000001803A3801 mov       r8, qword ptr [rip + 0x1808880] # 0x181bac088
00000001803A3808 call      0x180825710
00000001803A380D test      rax, rax
00000001803A3810 je        0x1803a3a54
00000001803A3816 xor       edx, edx
00000001803A3818 mov       rcx, rax
00000001803A381B call      0x18131c070
00000001803A3820 test      rax, rax
00000001803A3823 je        0x1803a3a54
00000001803A3829 xor       r8d, r8d
00000001803A382C mov       edx, 2
00000001803A3831 mov       rcx, rax
00000001803A3834 call      0x18132dbe0
00000001803A3839 test      rax, rax
00000001803A383C je        0x1803a3a54
00000001803A3842 xor       edx, edx
00000001803A3844 mov       rcx, rax
00000001803A3847 call      0x1813044d0
00000001803A384C test      rax, rax
00000001803A384F je        0x1803a3a54
00000001803A3855 xor       r8d, r8d
00000001803A3858 xor       edx, edx
00000001803A385A mov       rcx, rax
00000001803A385D call      0x18131bad0
00000001803A3862 mov       rbx, qword ptr [rdi + 0x10]
00000001803A3866 mov       rcx, qword ptr [rip + 0x180cc93] # 0x181bb0500
00000001803A386D cmp       dword ptr [rcx + 0xe0], 0
00000001803A3874 jne       0x1803a387b
00000001803A3876 call      0x1802501e0
00000001803A387B xor       r8d, r8d
00000001803A387E xor       edx, edx
00000001803A3880 mov       rcx, rbx
00000001803A3883 call      0x18131f760
00000001803A3888 test      al, al
00000001803A388A jne       0x1803a3a29
00000001803A3890 mov       rax, qword ptr [rdi + 0x10]
00000001803A3894 test      rax, rax
00000001803A3897 je        0x1803a3a54
00000001803A389D mov       rcx, qword ptr [rax + 0x48]
00000001803A38A1 test      rcx, rcx
00000001803A38A4 je        0x1803a3a54
00000001803A38AA xor       r8d, r8d
00000001803A38AD mov       edx, esi
00000001803A38AF call      0x180375f80
00000001803A38B4 mov       r14, rax
00000001803A38B7 test      rax, rax
00000001803A38BA je        0x1803a3a29
00000001803A38C0 cmp       dword ptr [rax + 0x18], 0
00000001803A38C4 je        0x1803a3a29
00000001803A38CA cmp       byte ptr [rax + 0x34], 0
00000001803A38CE je        0x1803a3a29
00000001803A38D4 xor       ebx, ebx
00000001803A38D6 dec       esi
00000001803A38D8 nop       dword ptr [rax + rax]
00000001803A38E0 mov       rcx, qword ptr [rdi + 0x48]
00000001803A38E4 test      rcx, rcx
00000001803A38E7 je        0x1803a3a54
00000001803A38ED mov       r8, qword ptr [rip + 0x1808794] # 0x181bac088
00000001803A38F4 mov       edx, ebx
00000001803A38F6 call      0x180825710
00000001803A38FB test      rax, rax
00000001803A38FE je        0x1803a3a54
00000001803A3904 xor       edx, edx
00000001803A3906 mov       rcx, rax
00000001803A3909 call      0x18131c070
00000001803A390E test      rax, rax
00000001803A3911 je        0x1803a3a54
00000001803A3917 xor       r8d, r8d
00000001803A391A mov       edx, 2
00000001803A391F mov       rcx, rax
00000001803A3922 call      0x18132dbe0
00000001803A3927 test      rax, rax
00000001803A392A je        0x1803a3a54
00000001803A3930 xor       edx, edx
00000001803A3932 mov       rcx, rax
00000001803A3935 call      0x1813044d0
00000001803A393A test      rax, rax
00000001803A393D je        0x1803a3a54
00000001803A3943 cmp       ebx, esi
00000001803A3945 setne     dl
00000001803A3948 xor       r8d, r8d
00000001803A394B mov       rcx, rax
00000001803A394E call      0x18131bad0
00000001803A3953 inc       ebx
00000001803A3955 cmp       ebx, 3
00000001803A3958 jl        0x1803a38e0
00000001803A395A mov       rax, qword ptr [rdi + 0x10]
00000001803A395E test      rax, rax
00000001803A3961 je        0x1803a3a54
00000001803A3967 cmp       qword ptr [rax + 0x48], 0
00000001803A396C je        0x1803a397c
00000001803A396E xor       r8d, r8d
00000001803A3971 mov       rdx, r14
00000001803A3974 mov       rcx, rdi
00000001803A3977 call      0x1803a2e80
00000001803A397C mov       rdx, qword ptr [rdi + 0x38]
00000001803A3980 test      rdx, rdx
00000001803A3983 je        0x1803a3a54
00000001803A3989 mov       r8, qword ptr [rip + 0x181b320] # 0x181bbecb0
00000001803A3990 lea       rcx, [rsp + 0x28]
00000001803A3995 call      0x1808197f0
00000001803A399A movups    xmm0, xmmword ptr [rsp + 0x28]
00000001803A399F movups    xmmword ptr [rsp + 0x40], xmm0
00000001803A39A4 movsd     xmm1, qword ptr [rsp + 0x38]
00000001803A39AA movsd     qword ptr [rsp + 0x50], xmm1
00000001803A39B0 mov       qword ptr [rsp + 0x28], 0
00000001803A39B9 lea       rbx, [rsp + 0x40]
00000001803A39BE mov       qword ptr [rsp + 0x30], rbx
00000001803A39C3 mov       rdx, qword ptr [rip + 0x1825d36] # 0x181bc9700
00000001803A39CA lea       rcx, [rsp + 0x40]
00000001803A39CF call      0x180669280
00000001803A39D4 test      al, al
00000001803A39D6 je        0x1803a39fd
00000001803A39D8 mov       rcx, qword ptr [rsp + 0x50]
00000001803A39DD test      rcx, rcx
00000001803A39E0 je        0x1803a3a48
00000001803A39E2 xor       edx, edx
00000001803A39E4 call      0x1813044d0
00000001803A39E9 test      rax, rax
00000001803A39EC je        0x1803a3a42
00000001803A39EE xor       r8d, r8d
00000001803A39F1 mov       dl, 1
00000001803A39F3 mov       rcx, rax
00000001803A39F6 call      0x18131bad0
00000001803A39FB jmp       0x1803a39c3
00000001803A39FD mov       rdx, qword ptr [rip + 0x1825cac] # 0x181bc96b0
00000001803A3A04 mov       rcx, rbx
00000001803A3A07 call      0x180302170
00000001803A3A0C jmp       0x1803a3a29
00000001803A3A0E mov       rdx, qword ptr [rip + 0x1825c9b] # 0x181bc96b0
00000001803A3A15 mov       rcx, qword ptr [rsp + 0x30]
00000001803A3A1A call      0x180302170
00000001803A3A1F mov       rcx, qword ptr [rsp + 0x28]
00000001803A3A24 test      rcx, rcx
00000001803A3A27 jne       0x1803a3a4e
00000001803A3A29 mov       rbx, qword ptr [rsp + 0x70]
00000001803A3A2E mov       rsi, qword ptr [rsp + 0x78]
00000001803A3A33 mov       rdi, qword ptr [rsp + 0x80]
00000001803A3A3B add       rsp, 0x60
00000001803A3A3F pop       r14
00000001803A3A41 ret       
00000001803A3A42 call      0x180250150
00000001803A3A47 nop       
00000001803A3A48 call      0x180250150
00000001803A3A4D nop       
00000001803A3A4E call      0x180200e60
00000001803A3A53 int3      
00000001803A3A54 call      0x180250150
00000001803A3A59 int3      
00000001803A3A5A int3      
00000001803A3A5B int3      
00000001803A3A5C int3      
00000001803A3A5D int3      
00000001803A3A5E int3      
00000001803A3A5F int3      