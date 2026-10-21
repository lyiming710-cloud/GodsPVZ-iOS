000000018031A930 mov       qword ptr [rsp + 8], rbx
000000018031A935 mov       qword ptr [rsp + 0x10], rsi
000000018031A93A mov       qword ptr [rsp + 0x18], rdi
000000018031A93F mov       qword ptr [rsp + 0x20], r12
000000018031A944 push      r13
000000018031A946 push      r14
000000018031A948 push      r15
000000018031A94A sub       rsp, 0x80
000000018031A951 movaps    xmmword ptr [rsp + 0x70], xmm6
000000018031A956 mov       rdi, rdx
000000018031A959 mov       r14, rcx
000000018031A95C cmp       byte ptr [rip + 0x19ca243], 0 # 0x181ce4ba6
000000018031A963 jne       0x18031a9e4
000000018031A965 lea       rcx, [rip + 0x1883ebc] # 0x181b9e828
000000018031A96C call      0x18024fef0
000000018031A971 lea       rcx, [rip + 0x18a85c0] # 0x181bc2f38
000000018031A978 call      0x18024fef0
000000018031A97D lea       rcx, [rip + 0x18a860c] # 0x181bc2f90
000000018031A984 call      0x18024fef0
000000018031A989 lea       rcx, [rip + 0x18a8658] # 0x181bc2fe8
000000018031A990 call      0x18024fef0
000000018031A995 lea       rcx, [rip + 0x188fa64] # 0x181baa400
000000018031A99C call      0x18024fef0
000000018031A9A1 lea       rcx, [rip + 0x188fab0] # 0x181baa458
000000018031A9A8 call      0x18024fef0
000000018031A9AD lea       rcx, [rip + 0x188fafc] # 0x181baa4b0
000000018031A9B4 call      0x18024fef0
000000018031A9B9 lea       rcx, [rip + 0x188f9e8] # 0x181baa3a8
000000018031A9C0 call      0x18024fef0
000000018031A9C5 lea       rcx, [rip + 0x18a2cfc] # 0x181bbd6c8
000000018031A9CC call      0x18024fef0
000000018031A9D1 lea       rcx, [rip + 0x18a0320] # 0x181bbacf8
000000018031A9D8 call      0x18024fef0
000000018031A9DD mov       byte ptr [rip + 0x19ca1c2], 1 # 0x181ce4ba6
000000018031A9E4 mov       rax, qword ptr [rip + 0x18a030d] # 0x181bbacf8
000000018031A9EB mov       rcx, qword ptr [rax + 0xb8]
000000018031A9F2 mov       rdx, qword ptr [rcx]
000000018031A9F5 mov       qword ptr [r14 + 0x38], rdx
000000018031A9F9 lea       rcx, [r14 + 0x38]
000000018031A9FD call      0x18024f360
000000018031AA02 xor       edx, edx
000000018031AA04 mov       rcx, r14
000000018031AA07 call      0x180302170
000000018031AA0C mov       rcx, qword ptr [rip + 0x18a2cb5] # 0x181bbd6c8
000000018031AA13 call      0x180250100
000000018031AA18 mov       rbx, rax
000000018031AA1B mov       rdx, qword ptr [rip + 0x188f986] # 0x181baa3a8
000000018031AA22 mov       rcx, rax
000000018031AA25 call      0x1808053d0
000000018031AA2A mov       qword ptr [r14 + 0x30], rbx
000000018031AA2E lea       rcx, [r14 + 0x30]
000000018031AA32 mov       rdx, rbx
000000018031AA35 call      0x18024f360
000000018031AA3A test      rdi, rdi
000000018031AA3D je        0x18031ac14
000000018031AA43 mov       eax, dword ptr [rdi + 0x10]
000000018031AA46 mov       dword ptr [r14 + 0x10], eax
000000018031AA4A mov       eax, dword ptr [rdi + 0x14]
000000018031AA4D mov       dword ptr [r14 + 0x14], eax
000000018031AA51 mov       eax, dword ptr [rdi + 0x18]
000000018031AA54 mov       dword ptr [r14 + 0x18], eax
000000018031AA58 mov       eax, dword ptr [rdi + 0x24]
000000018031AA5B movsd     xmm0, qword ptr [rdi + 0x1c]
000000018031AA60 movsd     qword ptr [r14 + 0x1c], xmm0
000000018031AA66 mov       dword ptr [r14 + 0x24], eax
000000018031AA6A mov       eax, dword ptr [rdi + 0x28]
000000018031AA6D mov       dword ptr [r14 + 0x28], eax
000000018031AA71 movss     xmm0, dword ptr [rdi + 0x2c]
000000018031AA76 movss     dword ptr [r14 + 0x2c], xmm0
000000018031AA7C mov       rdx, qword ptr [rdi + 0x38]
000000018031AA80 mov       qword ptr [r14 + 0x38], rdx
000000018031AA84 lea       rcx, [r14 + 0x38]
000000018031AA88 call      0x18024f360
000000018031AA8D mov       rcx, qword ptr [r14 + 0x30]
000000018031AA91 test      rcx, rcx
000000018031AA94 je        0x18031ac61
000000018031AA9A inc       dword ptr [rcx + 0x1c]
000000018031AA9D mov       r8d, dword ptr [rcx + 0x18]
000000018031AAA1 xor       r13d, r13d
000000018031AAA4 mov       dword ptr [rcx + 0x18], r13d
000000018031AAA8 test      r8d, r8d
000000018031AAAB jle       0x18031aabb
000000018031AAAD xor       r9d, r9d
000000018031AAB0 xor       edx, edx
000000018031AAB2 mov       rcx, qword ptr [rcx + 0x10]
000000018031AAB6 call      0x180cb86f0
000000018031AABB mov       rdx, qword ptr [rdi + 0x30]
000000018031AABF test      rdx, rdx
000000018031AAC2 je        0x18031ac61
000000018031AAC8 mov       r8, qword ptr [rip + 0x188f9e1] # 0x181baa4b0
000000018031AACF lea       rcx, [rsp + 0x38]
000000018031AAD4 call      0x1808197f0
000000018031AAD9 movups    xmm0, xmmword ptr [rsp + 0x38]
000000018031AADE movups    xmmword ptr [rsp + 0x50], xmm0
000000018031AAE3 movsd     xmm1, qword ptr [rsp + 0x48]
000000018031AAE9 movsd     qword ptr [rsp + 0x60], xmm1
000000018031AAEF mov       qword ptr [rsp + 0x38], r13
000000018031AAF4 lea       rbx, [rsp + 0x50]
000000018031AAF9 mov       qword ptr [rsp + 0x40], rbx
000000018031AAFE nop       
000000018031AB00 mov       rdx, qword ptr [rip + 0x18a8489] # 0x181bc2f90
000000018031AB07 lea       rcx, [rsp + 0x50]
000000018031AB0C call      0x180669280
000000018031AB11 test      al, al
000000018031AB13 je        0x18031abe8
000000018031AB19 mov       rdi, qword ptr [rsp + 0x60]
000000018031AB1E test      rdi, rdi
000000018031AB21 je        0x18031ac55
000000018031AB27 mov       r15d, dword ptr [rdi + 0x10]
000000018031AB2B mov       r12d, dword ptr [rdi + 0x14]
000000018031AB2F movss     xmm6, dword ptr [rdi + 0x24]
000000018031AB34 mov       rcx, qword ptr [rip + 0x1883ced] # 0x181b9e828
000000018031AB3B call      0x180250100
000000018031AB40 mov       rsi, rax
000000018031AB43 mov       qword ptr [rsp + 0x28], r13
000000018031AB48 mov       qword ptr [rsp + 0x20], r13
000000018031AB4D movaps    xmm3, xmm6
000000018031AB50 mov       r8d, r12d
000000018031AB53 mov       edx, r15d
000000018031AB56 mov       rcx, rax
000000018031AB59 call      0x1803299e0
000000018031AB5E test      rdi, rdi
000000018031AB61 je        0x18031ac50
000000018031AB67 movzx     eax, byte ptr [rdi + 0x29]
000000018031AB6B test      rsi, rsi
000000018031AB6E je        0x18031ac4b
000000018031AB74 mov       byte ptr [rsi + 0x29], al
000000018031AB77 mov       rcx, qword ptr [r14 + 0x30]
000000018031AB7B test      rcx, rcx
000000018031AB7E je        0x18031ac46
000000018031AB84 mov       r9, qword ptr [rip + 0x188f875] # 0x181baa400
000000018031AB8B inc       dword ptr [rcx + 0x1c]
000000018031AB8E mov       rdx, qword ptr [rcx + 0x10]
000000018031AB92 movsxd    r8, dword ptr [rcx + 0x18]
000000018031AB96 test      rdx, rdx
000000018031AB99 je        0x18031ac41
000000018031AB9F cmp       r8d, dword ptr [rdx + 0x18]
000000018031ABA3 jb        0x18031abc1
000000018031ABA5 mov       rax, qword ptr [r9 + 0x20]
000000018031ABA9 mov       r8, qword ptr [rax + 0xc0]
000000018031ABB0 mov       r8, qword ptr [r8 + 0x70]
000000018031ABB4 mov       rdx, rsi
000000018031ABB7 call      0x180847ce0
000000018031ABBC jmp       0x18031ab00
000000018031ABC1 lea       eax, [r8 + 1]
000000018031ABC5 mov       dword ptr [rcx + 0x18], eax
000000018031ABC8 cmp       r8d, dword ptr [rdx + 0x18]
000000018031ABCC jae       0x18031ac3b
000000018031ABCE mov       qword ptr [rdx + r8*8 + 0x20], rsi
000000018031ABD3 add       rdx, 0x20
000000018031ABD7 lea       rcx, [rdx + r8*8]
000000018031ABDB mov       rdx, rsi
000000018031ABDE call      0x18024f360
000000018031ABE3 jmp       0x18031ab00
000000018031ABE8 mov       rdx, qword ptr [rip + 0x18a8349] # 0x181bc2f38
000000018031ABEF mov       rcx, rbx
000000018031ABF2 call      0x180302170
000000018031ABF7 jmp       0x18031ac14
000000018031ABF9 mov       rdx, qword ptr [rip + 0x18a8338] # 0x181bc2f38
000000018031AC00 mov       rcx, qword ptr [rsp + 0x40]
000000018031AC05 call      0x180302170
000000018031AC0A mov       rcx, qword ptr [rsp + 0x38]
000000018031AC0F test      rcx, rcx
000000018031AC12 jne       0x18031ac5b
000000018031AC14 lea       r11, [rsp + 0x80]
000000018031AC1C mov       rbx, qword ptr [r11 + 0x20]
000000018031AC20 mov       rsi, qword ptr [r11 + 0x28]
000000018031AC24 mov       rdi, qword ptr [r11 + 0x30]
000000018031AC28 mov       r12, qword ptr [r11 + 0x38]
000000018031AC2C movaps    xmm6, xmmword ptr [rsp + 0x70]
000000018031AC31 mov       rsp, r11
000000018031AC34 pop       r15
000000018031AC36 pop       r14
000000018031AC38 pop       r13
000000018031AC3A ret       
000000018031AC3B call      0x180250140
000000018031AC40 nop       
000000018031AC41 call      0x180250150
000000018031AC46 call      0x180250150
000000018031AC4B call      0x180250150
000000018031AC50 call      0x180250150
000000018031AC55 call      0x180250150
000000018031AC5A nop       
000000018031AC5B call      0x180200e60
000000018031AC60 int3      
000000018031AC61 call      0x180250150
000000018031AC66 int3      
000000018031AC67 int3      
000000018031AC68 int3      
000000018031AC69 int3      
000000018031AC6A int3      
000000018031AC6B int3      
000000018031AC6C int3      
000000018031AC6D int3      
000000018031AC6E int3      
000000018031AC6F int3      