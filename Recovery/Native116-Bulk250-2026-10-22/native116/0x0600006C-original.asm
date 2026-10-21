00000001803076E0 push      rbx
00000001803076E2 push      rdi
00000001803076E3 push      r14
00000001803076E5 sub       rsp, 0x90
00000001803076EC cmp       byte ptr [rip + 0x19dd3e6], 0 # 0x181ce4ad9
00000001803076F3 mov       rbx, rdx
00000001803076F6 mov       rdi, rcx
00000001803076F9 jne       0x180307762
00000001803076FB lea       rcx, [rip + 0x18c3906] # 0x181bcb008
0000000180307702 call      0x18024fef0
0000000180307707 lea       rcx, [rip + 0x18a5542] # 0x181bacc50
000000018030770E call      0x18024fef0
0000000180307713 lea       rcx, [rip + 0x18a54de] # 0x181bacbf8
000000018030771A call      0x18024fef0
000000018030771F lea       rcx, [rip + 0x18c110a] # 0x181bc8830
0000000180307726 call      0x18024fef0
000000018030772B lea       rcx, [rip + 0x189da96] # 0x181ba51c8
0000000180307732 call      0x18024fef0
0000000180307737 lea       rcx, [rip + 0x18b61ea] # 0x181bbd928
000000018030773E call      0x18024fef0
0000000180307743 lea       rcx, [rip + 0x18a608e] # 0x181bad7d8
000000018030774A call      0x18024fef0
000000018030774F lea       rcx, [rip + 0x18c2be2] # 0x181bca338
0000000180307756 call      0x18024fef0
000000018030775B mov       byte ptr [rip + 0x19dd377], 1 # 0x181ce4ad9
0000000180307762 mov       rcx, qword ptr [rip + 0x18c389f] # 0x181bcb008
0000000180307769 mov       edx, 5
000000018030776E mov       qword ptr [rsp + 0xb0], rbp
0000000180307776 mov       qword ptr [rsp + 0xc0], rsi
000000018030777E mov       qword ptr [rsp + 0x88], r12
0000000180307786 mov       qword ptr [rsp + 0x80], r15
000000018030778E movaps    xmmword ptr [rsp + 0x70], xmm6
0000000180307793 movaps    xmmword ptr [rsp + 0x60], xmm7
0000000180307798 movaps    xmmword ptr [rsp + 0x50], xmm8
000000018030779E movaps    xmmword ptr [rsp + 0x40], xmm9
00000001803077A4 movaps    xmmword ptr [rsp + 0x30], xmm10
00000001803077AA movaps    xmmword ptr [rsp + 0x20], xmm11
00000001803077B0 call      0x18024f3e0
00000001803077B5 mov       rdx, qword ptr [rip + 0x18c1074] # 0x181bc8830
00000001803077BC xor       r8d, r8d
00000001803077BF mov       rcx, rax
00000001803077C2 mov       r14, rax
00000001803077C5 call      0x180c0afd0
00000001803077CA mov       rdx, qword ptr [rip + 0x18c2b67] # 0x181bca338
00000001803077D1 xor       r8d, r8d
00000001803077D4 mov       rcx, rbx
00000001803077D7 call      0x180b76170
00000001803077DC test      al, al
00000001803077DE je        0x180307810
00000001803077E0 mov       rbx, qword ptr [rdi + 0x48]
00000001803077E4 movss     xmm6, dword ptr [rdi + 0x60]
00000001803077E9 movss     xmm7, dword ptr [rdi + 0x64]
00000001803077EE test      rbx, rbx
00000001803077F1 je        0x180307ac9
00000001803077F7 mov       rax, qword ptr [rdi + 0x48]
00000001803077FB mov       rbx, qword ptr [rbx + 0x38]
00000001803077FF movss     dword ptr [rax + 0x40], xmm7
0000000180307804 mov       r12, qword ptr [rdi + 0xb8]
000000018030780B jmp       0x180307898
0000000180307810 mov       rdx, qword ptr [rip + 0x18a5fc1] # 0x181bad7d8
0000000180307817 xor       r8d, r8d
000000018030781A mov       rcx, rbx
000000018030781D call      0x180b76170
0000000180307822 test      al, al
0000000180307824 je        0x180307853
0000000180307826 mov       rbx, qword ptr [rdi + 0x48]
000000018030782A movss     xmm6, dword ptr [rdi + 0x68]
000000018030782F movss     xmm7, dword ptr [rdi + 0x6c]
0000000180307834 test      rbx, rbx
0000000180307837 je        0x180307ac9
000000018030783D mov       rax, qword ptr [rdi + 0x48]
0000000180307841 mov       rbx, qword ptr [rbx + 0x48]
0000000180307845 movss     dword ptr [rax + 0x50], xmm7
000000018030784A mov       r12, qword ptr [rdi + 0xc0]
0000000180307851 jmp       0x180307898
0000000180307853 mov       rdx, qword ptr [rip + 0x18b60ce] # 0x181bbd928
000000018030785A xor       r8d, r8d
000000018030785D mov       rcx, rbx
0000000180307860 call      0x180b76170
0000000180307865 test      al, al
0000000180307867 je        0x180307a7b
000000018030786D mov       rbx, qword ptr [rdi + 0x48]
0000000180307871 movss     xmm6, dword ptr [rdi + 0x70]
0000000180307876 movss     xmm7, dword ptr [rdi + 0x74]
000000018030787B test      rbx, rbx
000000018030787E je        0x180307ac9
0000000180307884 mov       rax, qword ptr [rdi + 0x48]
0000000180307888 mov       rbx, qword ptr [rbx + 0x60]
000000018030788C movss     dword ptr [rax + 0x68], xmm7
0000000180307891 mov       r12, qword ptr [rdi + 0xc8]
0000000180307898 xor       r15d, r15d
000000018030789B mov       esi, r15d
000000018030789E mov       edi, r15d
00000001803078A1 mov       ebp, r15d
00000001803078A4 test      r14, r14
00000001803078A7 je        0x180307ac9
00000001803078AD nop       dword ptr [rax]
00000001803078B0 cmp       edi, dword ptr [r14 + 0x18]
00000001803078B4 jae       0x180307acf
00000001803078BA movsxd    rax, edi
00000001803078BD cmp       ebp, dword ptr [r14 + rax*4 + 0x20]
00000001803078C2 jge       0x180307918
00000001803078C4 mov       rcx, qword ptr [rip + 0x18a532d] # 0x181bacbf8
00000001803078CB cmp       dword ptr [rcx + 0xe0], r15d
00000001803078D2 jne       0x1803078d9
00000001803078D4 call      0x1802501e0
00000001803078D9 test      rbx, rbx
00000001803078DC je        0x180307ac9
00000001803078E2 cmp       esi, dword ptr [rbx + 0x18]
00000001803078E5 jae       0x180307acf
00000001803078EB movd      xmm0, ebp
00000001803078EF cvtdq2ps  xmm0, xmm0
00000001803078F2 divss     xmm0, xmm7
00000001803078F6 cvtps2pd  xmm0, xmm0
00000001803078F9 call      0x1802cb340
00000001803078FE xorps     xmm1, xmm1
0000000180307901 movsxd    rax, esi
0000000180307904 cvtsd2ss  xmm1, xmm0
0000000180307908 inc       esi
000000018030790A inc       ebp
000000018030790C mulss     xmm1, xmm6
0000000180307910 movss     dword ptr [rbx + rax*4 + 0x20], xmm1
0000000180307916 jmp       0x1803078b0
0000000180307918 test      rbx, rbx
000000018030791B je        0x180307ac9
0000000180307921 movsxd    rax, esi
0000000180307924 dec       rax
0000000180307927 cmp       eax, dword ptr [rbx + 0x18]
000000018030792A jae       0x180307acf
0000000180307930 movss     xmm6, dword ptr [rbx + rax*4 + 0x20]
0000000180307936 inc       edi
0000000180307938 cmp       edi, 5
000000018030793B jl        0x1803078a1
0000000180307941 movsd     xmm10, qword ptr [rip + 0x12a00ce] # 0x1815a7a18
000000018030794A xorps     xmm9, xmm9
000000018030794E movss     xmm7, dword ptr [rip + 0x12a00ba] # 0x1815a7a10
0000000180307956 movss     xmm8, dword ptr [rip + 0x12a00a9] # 0x1815a7a08
000000018030795F movsd     xmm11, qword ptr [rip + 0x12a00f0] # 0x1815a7a58
0000000180307968 mov       eax, r15d
000000018030796B nop       dword ptr [rax + rax]
0000000180307970 cmp       eax, dword ptr [rbx + 0x18]
0000000180307973 jge       0x180307a38
0000000180307979 cmp       r15d, dword ptr [rbx + 0x18]
000000018030797D jae       0x180307acf
0000000180307983 mov       rcx, qword ptr [rip + 0x18a52c6] # 0x181bacc50
000000018030798A movsxd    rax, r15d
000000018030798D cmp       dword ptr [rcx + 0xe0], 0
0000000180307994 movss     xmm6, dword ptr [rbx + rax*4 + 0x20]
000000018030799A jne       0x1803079a1
000000018030799C call      0x1802501e0
00000001803079A1 cvtps2pd  xmm0, xmm6
00000001803079A4 lea       rdx, [rsp + 0xb8]
00000001803079AC call      0x1802cb1d8
00000001803079B1 comiss    xmm6, xmm9
00000001803079B5 jb        0x1803079eb
00000001803079B7 ucomisd   xmm0, xmm10
00000001803079BC jp        0x1803079dc
00000001803079BE jne       0x1803079dc
00000001803079C0 movsd     xmm0, qword ptr [rsp + 0xb8]
00000001803079C9 cvttsd2si rax, xmm0
00000001803079CE cvtpd2ps  xmm0, xmm0
00000001803079D2 test      al, 1
00000001803079D4 je        0x180307a1d
00000001803079D6 addss     xmm0, xmm7
00000001803079DA jmp       0x180307a1d
00000001803079DC addss     xmm6, xmm8
00000001803079E1 movaps    xmm0, xmm6
00000001803079E4 call      0x1802cb738
00000001803079E9 jmp       0x180307a1d
00000001803079EB ucomisd   xmm0, xmm11
00000001803079F0 jp        0x180307a10
00000001803079F2 jne       0x180307a10
00000001803079F4 movsd     xmm0, qword ptr [rsp + 0xb8]
00000001803079FD cvttsd2si rax, xmm0
0000000180307A02 cvtpd2ps  xmm0, xmm0
0000000180307A06 test      al, 1
0000000180307A08 je        0x180307a1d
0000000180307A0A subss     xmm0, xmm7
0000000180307A0E jmp       0x180307a1d
0000000180307A10 subss     xmm6, xmm8
0000000180307A15 movaps    xmm0, xmm6
0000000180307A18 call      0x1802cb2b8
0000000180307A1D cmp       r15d, dword ptr [rbx + 0x18]
0000000180307A21 jae       0x180307acf
0000000180307A27 movsxd    rax, r15d
0000000180307A2A inc       r15d
0000000180307A2D movss     dword ptr [rbx + rax*4 + 0x20], xmm0
0000000180307A33 jmp       0x180307968
0000000180307A38 cmp       dword ptr [rbx + 0x18], 0xc7
0000000180307A3F jbe       0x180307acf
0000000180307A45 lea       rcx, [rbx + 0x33c]
0000000180307A4C xor       edx, edx
0000000180307A4E call      0x180cc5b50
0000000180307A53 mov       rcx, qword ptr [rip + 0x189d76e] # 0x181ba51c8
0000000180307A5A xor       r8d, r8d
0000000180307A5D mov       rdx, rax
0000000180307A60 call      0x180b74270
0000000180307A65 test      r12, r12
0000000180307A68 je        0x180307ac9
0000000180307A6A xor       r9d, r9d
0000000180307A6D mov       r8b, 1
0000000180307A70 mov       rdx, rax
0000000180307A73 mov       rcx, r12
0000000180307A76 call      0x1811af070
0000000180307A7B movaps    xmm11, xmmword ptr [rsp + 0x20]
0000000180307A81 movaps    xmm10, xmmword ptr [rsp + 0x30]
0000000180307A87 movaps    xmm9, xmmword ptr [rsp + 0x40]
0000000180307A8D movaps    xmm8, xmmword ptr [rsp + 0x50]
0000000180307A93 movaps    xmm7, xmmword ptr [rsp + 0x60]
0000000180307A98 movaps    xmm6, xmmword ptr [rsp + 0x70]
0000000180307A9D mov       r15, qword ptr [rsp + 0x80]
0000000180307AA5 mov       r12, qword ptr [rsp + 0x88]
0000000180307AAD mov       rsi, qword ptr [rsp + 0xc0]
0000000180307AB5 mov       rbp, qword ptr [rsp + 0xb0]
0000000180307ABD add       rsp, 0x90
0000000180307AC4 pop       r14
0000000180307AC6 pop       rdi
0000000180307AC7 pop       rbx
0000000180307AC8 ret       
0000000180307AC9 call      0x180250150
0000000180307ACE int3      
0000000180307ACF call      0x180250140
0000000180307AD4 int3      
0000000180307AD5 int3      
0000000180307AD6 int3      
0000000180307AD7 int3      
0000000180307AD8 int3      
0000000180307AD9 int3      
0000000180307ADA int3      
0000000180307ADB int3      
0000000180307ADC int3      
0000000180307ADD int3      
0000000180307ADE int3      
0000000180307ADF int3      