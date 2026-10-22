000000018032C490 mov       qword ptr [rsp + 0x10], rdx
000000018032C495 mov       qword ptr [rsp + 8], rcx
000000018032C49A push      rbx
000000018032C49B push      rdi
000000018032C49C sub       rsp, 0x98
000000018032C4A3 cmp       byte ptr [rip + 0x19b87c5], 0 # 0x181ce4c6f
000000018032C4AA mov       rbx, rdx
000000018032C4AD mov       rdi, rcx
000000018032C4B0 jne       0x18032c54d
000000018032C4B6 lea       rcx, [rip + 0x186e88b] # 0x181b9ad48
000000018032C4BD call      0x18024fef0
000000018032C4C2 lea       rcx, [rip + 0x187235f] # 0x181b9e828
000000018032C4C9 call      0x18024fef0
000000018032C4CE lea       rcx, [rip + 0x187df2b] # 0x181baa400
000000018032C4D5 call      0x18024fef0
000000018032C4DA lea       rcx, [rip + 0x187e027] # 0x181baa508
000000018032C4E1 call      0x18024fef0
000000018032C4E6 lea       rcx, [rip + 0x187debb] # 0x181baa3a8
000000018032C4ED call      0x18024fef0
000000018032C4F2 lea       rcx, [rip + 0x187e117] # 0x181baa610
000000018032C4F9 call      0x18024fef0
000000018032C4FE lea       rcx, [rip + 0x187e163] # 0x181baa668
000000018032C505 call      0x18024fef0
000000018032C50A lea       rcx, [rip + 0x188bcdf] # 0x181bb81f0
000000018032C511 call      0x18024fef0
000000018032C516 lea       rcx, [rip + 0x188087b] # 0x181bacd98
000000018032C51D call      0x18024fef0
000000018032C522 lea       rcx, [rip + 0x189119f] # 0x181bbd6c8
000000018032C529 call      0x18024fef0
000000018032C52E lea       rcx, [rip + 0x18806c3] # 0x181bacbf8
000000018032C535 call      0x18024fef0
000000018032C53A lea       rcx, [rip + 0x1876dc7] # 0x181ba3308
000000018032C541 call      0x18024fef0
000000018032C546 mov       byte ptr [rip + 0x19b8722], 1 # 0x181ce4c6f
000000018032C54D mov       qword ptr [rsp + 0xc0], rbp
000000018032C555 mov       qword ptr [rsp + 0x90], rsi
000000018032C55D mov       qword ptr [rsp + 0x88], r12
000000018032C565 mov       qword ptr [rsp + 0x80], r13
000000018032C56D mov       qword ptr [rsp + 0x78], r14
000000018032C572 mov       qword ptr [rsp + 0x70], r15
000000018032C577 test      rbx, rbx
000000018032C57A je        0x18032cbc5
000000018032C580 cmp       qword ptr [rbx + 0x120], 0
000000018032C588 mov       rax, qword ptr [rbx + 0x20]
000000018032C58C mov       qword ptr [rsp + 0x58], rax
000000018032C591 je        0x18032cbc1
000000018032C597 mov       rax, qword ptr [rbx + 0x120]
000000018032C59E cmp       dword ptr [rax + 0x18], 0
000000018032C5A2 je        0x18032cbc1
000000018032C5A8 xor       edx, edx
000000018032C5AA mov       rcx, rdi
000000018032C5AD call      0x18032c3b0
000000018032C5B2 cmp       byte ptr [rip + 0x19b86b4], 0 # 0x181ce4c6d
000000018032C5B9 mov       r13d, eax
000000018032C5BC jne       0x18032c5d1
000000018032C5BE lea       rcx, [rip + 0x188bbd3] # 0x181bb8198
000000018032C5C5 call      0x18024fef0
000000018032C5CA mov       byte ptr [rip + 0x19b869c], 1 # 0x181ce4c6d
000000018032C5D1 cmp       qword ptr [rdi + 0x10], 0
000000018032C5D6 jne       0x18032c5dd
000000018032C5D8 xor       r14d, r14d
000000018032C5DB jmp       0x18032c5e5
000000018032C5DD mov       rax, qword ptr [rdi + 0x10]
000000018032C5E1 mov       r14d, dword ptr [rax + 0x18]
000000018032C5E5 mov       dword ptr [rsp + 0x50], r14d
000000018032C5EA test      r13d, r13d
000000018032C5ED je        0x18032cbc1
000000018032C5F3 test      r14d, r14d
000000018032C5F6 je        0x18032cbc1
000000018032C5FC mov       eax, dword ptr [rbx + 0x2c]
000000018032C5FF mov       ebx, dword ptr [rbx + 0x28]
000000018032C602 mov       dword ptr [rsp + 0xc8], eax
000000018032C609 mov       dword ptr [rsp + 0x54], ebx
000000018032C60D test      ebx, ebx
000000018032C60F js        0x18032cbc1
000000018032C615 cmp       byte ptr [rip + 0x19b864d], 0 # 0x181ce4c69
000000018032C61C jne       0x18032c66d
000000018032C61E lea       rcx, [rip + 0x1886643] # 0x181bb2c68
000000018032C625 call      0x18024fef0
000000018032C62A lea       rcx, [rip + 0x18865df] # 0x181bb2c10
000000018032C631 call      0x18024fef0
000000018032C636 lea       rcx, [rip + 0x188bbb3] # 0x181bb81f0
000000018032C63D call      0x18024fef0
000000018032C642 lea       rcx, [rip + 0x188074f] # 0x181bacd98
000000018032C649 call      0x18024fef0
000000018032C64E lea       rcx, [rip + 0x18929c3] # 0x181bbf018
000000018032C655 call      0x18024fef0
000000018032C65A lea       rcx, [rip + 0x188370f] # 0x181bafd70
000000018032C661 call      0x18024fef0
000000018032C666 mov       byte ptr [rip + 0x19b85fc], 1 # 0x181ce4c69
000000018032C66D mov       rcx, qword ptr [rip + 0x18929a4] # 0x181bbf018
000000018032C674 call      0x180250100
000000018032C679 mov       rdx, qword ptr [rip + 0x1886590] # 0x181bb2c10
000000018032C680 mov       rcx, rax
000000018032C683 mov       rbp, rax
000000018032C686 call      0x1808053d0
000000018032C68B xor       r15d, r15d
000000018032C68E mov       r12d, 0x7fffffff
000000018032C694 test      r14d, r14d
000000018032C697 jle       0x18032c777
000000018032C69D nop       dword ptr [rax]
000000018032C6A0 mov       rcx, qword ptr [rdi + 0x10]
000000018032C6A4 test      rcx, rcx
000000018032C6A7 je        0x18032cbc5
000000018032C6AD mov       r8, qword ptr [rip + 0x188bb3c] # 0x181bb81f0
000000018032C6B4 mov       edx, r15d
000000018032C6B7 call      0x180825710
000000018032C6BC mov       r14, rax
000000018032C6BF test      r13d, r13d
000000018032C6C2 jle       0x18032c762
000000018032C6C8 nop       dword ptr [rax + rax]
000000018032C6D0 xor       ebx, ebx
000000018032C6D2 test      rax, rax
000000018032C6D5 je        0x18032cbc5
000000018032C6DB nop       dword ptr [rax + rax]
000000018032C6E0 mov       rcx, qword ptr [r14 + 0x18]
000000018032C6E4 test      rcx, rcx
000000018032C6E7 je        0x18032cbc5
000000018032C6ED mov       r8, qword ptr [rip + 0x18806a4] # 0x181bacd98
000000018032C6F4 mov       edx, ebx
000000018032C6F6 call      0x180825710
000000018032C6FB mov       rcx, qword ptr [rip + 0x188366e] # 0x181bafd70
000000018032C702 mov       rsi, rax
000000018032C705 call      0x180250100
000000018032C70A xor       edx, edx
000000018032C70C mov       rcx, rax
000000018032C70F mov       rdi, rax
000000018032C712 call      0x180302170
000000018032C717 mov       dword ptr [rdi + 0x1c], r12d
000000018032C71B mov       dword ptr [rdi + 0x10], ebx
000000018032C71E mov       dword ptr [rdi + 0x14], r15d
000000018032C722 test      rsi, rsi
000000018032C725 je        0x18032cbc5
000000018032C72B xor       edx, edx
000000018032C72D mov       rcx, rsi
000000018032C730 call      0x18032a110
000000018032C735 mov       dword ptr [rdi + 0x18], eax
000000018032C738 test      rbp, rbp
000000018032C73B je        0x18032cbc5
000000018032C741 mov       r8, qword ptr [rip + 0x1886520] # 0x181bb2c68
000000018032C748 mov       rdx, rdi
000000018032C74B mov       rcx, rbp
000000018032C74E call      0x180002e70
000000018032C753 inc       ebx
000000018032C755 cmp       ebx, r13d
000000018032C758 jl        0x18032c6e0
000000018032C75A mov       rdi, qword ptr [rsp + 0xb0]
000000018032C762 mov       r14d, dword ptr [rsp + 0x50]
000000018032C767 inc       r15d
000000018032C76A cmp       r15d, r14d
000000018032C76D jl        0x18032c6a0
000000018032C773 mov       ebx, dword ptr [rsp + 0x54]
000000018032C777 mov       rcx, qword ptr [rip + 0x1890f4a] # 0x181bbd6c8
000000018032C77E call      0x180250100
000000018032C783 mov       rdx, qword ptr [rip + 0x187dc1e] # 0x181baa3a8
000000018032C78A mov       rcx, rax
000000018032C78D mov       rsi, rax
000000018032C790 call      0x1808053d0
000000018032C795 cmp       ebx, r13d
000000018032C798 jge       0x18032c7c1
000000018032C79A mov       r9d, dword ptr [rsp + 0xc8]
000000018032C7A2 mov       r8d, ebx
000000018032C7A5 mov       edx, r13d
000000018032C7A8 mov       qword ptr [rsp + 0x20], 0
000000018032C7B1 mov       rcx, rbp
000000018032C7B4 call      0x18032d950
000000018032C7B9 mov       r15, rax
000000018032C7BC jmp       0x18032c99f
000000018032C7C1 xor       ebx, ebx
000000018032C7C3 mov       edi, 0xffffffff
000000018032C7C8 test      r14d, r14d
000000018032C7CB jle       0x18032cbc1
000000018032C7D1 neg       dword ptr [rsp + 0xc8]
000000018032C7D8 mov       r15, qword ptr [rsp + 0xb8]
000000018032C7E0 mov       rax, qword ptr [rsp + 0xb0]
000000018032C7E8 mov       rcx, qword ptr [rax + 0x10]
000000018032C7EC test      rcx, rcx
000000018032C7EF je        0x18032cbc5
000000018032C7F5 mov       r8, qword ptr [rip + 0x188b9f4] # 0x181bb81f0
000000018032C7FC mov       edx, ebx
000000018032C7FE call      0x180825710
000000018032C803 test      rax, rax
000000018032C806 je        0x18032cbc5
000000018032C80C mov       rcx, qword ptr [rax + 0x18]
000000018032C810 test      rcx, rcx
000000018032C813 je        0x18032cbc5
000000018032C819 mov       r8, qword ptr [rip + 0x1880578] # 0x181bacd98
000000018032C820 lea       edx, [r13 - 1]
000000018032C824 call      0x180825710
000000018032C829 test      rax, rax
000000018032C82C je        0x18032cbc5
000000018032C832 xor       edx, edx
000000018032C834 mov       rcx, rax
000000018032C837 call      0x18032a110
000000018032C83C cmp       eax, dword ptr [r15 + 0x11c]
000000018032C843 jg        0x18032c899
000000018032C845 mov       rcx, qword ptr [rip + 0x18803ac] # 0x181bacbf8
000000018032C84C cmp       dword ptr [rcx + 0xe0], 0
000000018032C853 jne       0x18032c861
000000018032C855 call      0x1802501e0
000000018032C85A mov       rcx, qword ptr [rip + 0x1880397] # 0x181bacbf8
000000018032C861 mov       eax, dword ptr [rsp + 0xc8]
000000018032C868 add       eax, ebx
000000018032C86A mov       edx, eax
000000018032C86C neg       edx
000000018032C86E cmovs     edx, eax
000000018032C871 cmp       edx, r12d
000000018032C874 jge       0x18032c899
000000018032C876 cmp       dword ptr [rcx + 0xe0], 0
000000018032C87D jne       0x18032c884
000000018032C87F call      0x1802501e0
000000018032C884 mov       eax, dword ptr [rsp + 0xc8]
000000018032C88B mov       edi, ebx
000000018032C88D add       eax, ebx
000000018032C88F mov       r12d, eax
000000018032C892 neg       r12d
000000018032C895 cmovs     r12d, eax
000000018032C899 inc       ebx
000000018032C89B cmp       ebx, r14d
000000018032C89E jl        0x18032c7e0
000000018032C8A4 test      edi, edi
000000018032C8A6 js        0x18032cbc1
000000018032C8AC cmp       byte ptr [rip + 0x19b83c7], 0 # 0x181ce4c7a
000000018032C8B3 lea       ebx, [r13 - 1]
000000018032C8B7 jne       0x18032c8d8
000000018032C8B9 lea       rcx, [rip + 0x1886508] # 0x181bb2dc8
000000018032C8C0 call      0x18024fef0
000000018032C8C5 lea       rcx, [rip + 0x1886554] # 0x181bb2e20
000000018032C8CC call      0x18024fef0
000000018032C8D1 mov       byte ptr [rip + 0x19b83a2], 1 # 0x181ce4c7a
000000018032C8D8 cmp       ebx, r13d
000000018032C8DB jge       0x18032cb9e
000000018032C8E1 imul      edi, r13d
000000018032C8E5 add       edi, ebx
000000018032C8E7 test      rbp, rbp
000000018032C8EA je        0x18032cbc5
000000018032C8F0 cmp       dword ptr [rbp + 0x18], edi
000000018032C8F3 jle       0x18032cb9e
000000018032C8F9 mov       r8, qword ptr [rip + 0x1886520] # 0x181bb2e20
000000018032C900 mov       edx, edi
000000018032C902 mov       rcx, rbp
000000018032C905 call      0x180825710
000000018032C90A mov       r15, rax
000000018032C90D test      rax, rax
000000018032C910 je        0x18032cb9e
000000018032C916 mov       rax, qword ptr [rsp + 0x58]
000000018032C91B test      rax, rax
000000018032C91E je        0x18032cbc5
000000018032C924 mov       rdx, qword ptr [rax + 0x28]
000000018032C928 xor       r8d, r8d
000000018032C92B mov       rcx, r15
000000018032C92E call      0x18032d9e0
000000018032C933 mov       r9, rax
000000018032C936 test      rsi, rsi
000000018032C939 je        0x18032cbc5
000000018032C93F mov       rax, qword ptr [rip + 0x187daba] # 0x181baa400
000000018032C946 inc       dword ptr [rsi + 0x1c]
000000018032C949 mov       rcx, qword ptr [rsi + 0x10]
000000018032C94D movsxd    rdx, dword ptr [rsi + 0x18]
000000018032C951 test      rcx, rcx
000000018032C954 je        0x18032cbc5
000000018032C95A cmp       edx, dword ptr [rcx + 0x18]
000000018032C95D jb        0x18032c97b
000000018032C95F mov       rax, qword ptr [rax + 0x20]
000000018032C963 mov       rdx, r9
000000018032C966 mov       rcx, rsi
000000018032C969 mov       r8, qword ptr [rax + 0xc0]
000000018032C970 mov       r8, qword ptr [r8 + 0x70]
000000018032C974 call      0x180847ce0
000000018032C979 jmp       0x18032c99f
000000018032C97B lea       eax, [rdx + 1]
000000018032C97E mov       dword ptr [rsi + 0x18], eax
000000018032C981 cmp       edx, dword ptr [rcx + 0x18]
000000018032C984 jae       0x18032cbcb
000000018032C98A mov       qword ptr [rcx + rdx*8 + 0x20], r9
000000018032C98F lea       rcx, [rcx + rdx*8]
000000018032C993 add       rcx, 0x20
000000018032C997 mov       rdx, r9
000000018032C99A call      0x18024f360
000000018032C99F mov       r12, qword ptr [rsp + 0xb8]
000000018032C9A7 mov       rcx, qword ptr [r12 + 0x120]
000000018032C9AF test      rcx, rcx
000000018032C9B2 je        0x18032cbc5
000000018032C9B8 mov       r8, qword ptr [rip + 0x187dca9] # 0x181baa668
000000018032C9BF xor       edx, edx
000000018032C9C1 call      0x180825710
000000018032C9C6 mov       rbx, rax
000000018032C9C9 test      rax, rax
000000018032C9CC je        0x18032cbc5
000000018032C9D2 cmp       byte ptr [rip + 0x19b82a1], 0 # 0x181ce4c7a
000000018032C9D9 mov       r14d, dword ptr [rax + 0x10]
000000018032C9DD mov       edi, dword ptr [rax + 0x14]
000000018032C9E0 jne       0x18032ca01
000000018032C9E2 lea       rcx, [rip + 0x18863df] # 0x181bb2dc8
000000018032C9E9 call      0x18024fef0
000000018032C9EE lea       rcx, [rip + 0x188642b] # 0x181bb2e20
000000018032C9F5 call      0x18024fef0
000000018032C9FA mov       byte ptr [rip + 0x19b8279], 1 # 0x181ce4c7a
000000018032CA01 cmp       r14d, r13d
000000018032CA04 jge       0x18032caa8
000000018032CA0A imul      edi, r13d
000000018032CA0E add       edi, r14d
000000018032CA11 test      rbp, rbp
000000018032CA14 je        0x18032cbc5
000000018032CA1A cmp       dword ptr [rbp + 0x18], edi
000000018032CA1D jle       0x18032caa8
000000018032CA23 mov       r8, qword ptr [rip + 0x18863f6] # 0x181bb2e20
000000018032CA2A mov       edx, edi
000000018032CA2C mov       rcx, rbp
000000018032CA2F call      0x180825710
000000018032CA34 test      rax, rax
000000018032CA37 je        0x18032caa8
000000018032CA39 mov       rcx, qword ptr [rsp + 0x58]
000000018032CA3E test      rcx, rcx
000000018032CA41 je        0x18032cbc5
000000018032CA47 mov       rcx, qword ptr [rcx + 0x28]
000000018032CA4B xor       edi, edi
000000018032CA4D mov       qword ptr [rsp + 0x48], rdi
000000018032CA52 mov       r9, rax
000000018032CA55 mov       qword ptr [rsp + 0x40], rcx
000000018032CA5A mov       r8, r15
000000018032CA5D mov       ecx, dword ptr [r12 + 0x11c]
000000018032CA65 mov       rdx, rsi
000000018032CA68 mov       dword ptr [rsp + 0x38], ecx
000000018032CA6C mov       ecx, dword ptr [rsp + 0x50]
000000018032CA70 mov       dword ptr [rsp + 0x30], ecx
000000018032CA74 mov       rcx, qword ptr [rsp + 0xb0]
000000018032CA7C mov       dword ptr [rsp + 0x28], r13d
000000018032CA81 mov       qword ptr [rsp + 0x20], rbp
000000018032CA86 call      0x18032a7d0
000000018032CA8B mov       r9d, dword ptr [rbx + 0x14]
000000018032CA8F mov       edx, r13d
000000018032CA92 mov       r8d, dword ptr [rbx + 0x10]
000000018032CA96 mov       rcx, rbp
000000018032CA99 mov       qword ptr [rsp + 0x20], rdi
000000018032CA9E call      0x18032d950
000000018032CAA3 jmp       0x18032cb67
000000018032CAA8 mov       rax, qword ptr [rsp + 0x58]
000000018032CAAD mov       edi, dword ptr [rbx + 0x10]
000000018032CAB0 mov       ebp, dword ptr [rbx + 0x14]
000000018032CAB3 test      rax, rax
000000018032CAB6 je        0x18032cbc5
000000018032CABC mov       rcx, qword ptr [rip + 0x1871d65] # 0x181b9e828
000000018032CAC3 mov       r14, qword ptr [rax + 0x28]
000000018032CAC7 call      0x180250100
000000018032CACC xor       edx, edx
000000018032CACE mov       rcx, rax
000000018032CAD1 mov       rbx, rax
000000018032CAD4 call      0x180302170
000000018032CAD9 xor       eax, eax
000000018032CADB mov       dword ptr [rbx + 0x10], edi
000000018032CADE mov       dword ptr [rbx + 0x14], ebp
000000018032CAE1 mov       dword ptr [rbx + 0x24], eax
000000018032CAE4 mov       byte ptr [rbx + 0x29], al
000000018032CAE7 test      r14, r14
000000018032CAEA je        0x18032cb15
000000018032CAEC mov       r9d, ebp
000000018032CAEF mov       qword ptr [rsp + 0x20], rax
000000018032CAF4 mov       r8d, edi
000000018032CAF7 lea       rcx, [rsp + 0x58]
000000018032CAFC mov       rdx, r14
000000018032CAFF call      0x18030ef10
000000018032CB04 movsd     xmm0, qword ptr [rax]
000000018032CB08 mov       ecx, dword ptr [rax + 8]
000000018032CB0B movsd     qword ptr [rbx + 0x18], xmm0
000000018032CB10 mov       dword ptr [rbx + 0x20], ecx
000000018032CB13 jmp       0x18032cb4d
000000018032CB15 cmp       byte ptr [rip + 0x19b801b], al # 0x181ce4b36
000000018032CB1B jne       0x18032cb30
000000018032CB1D lea       rcx, [rip + 0x1895cc4] # 0x181bc27e8
000000018032CB24 call      0x18024fef0
000000018032CB29 mov       byte ptr [rip + 0x19b8006], 1 # 0x181ce4b36
000000018032CB30 mov       rax, qword ptr [rip + 0x1895cb1] # 0x181bc27e8
000000018032CB37 mov       rcx, qword ptr [rax + 0xb8]
000000018032CB3E movsd     xmm0, qword ptr [rcx]
000000018032CB42 mov       eax, dword ptr [rcx + 8]
000000018032CB45 movsd     qword ptr [rbx + 0x18], xmm0
000000018032CB4A mov       dword ptr [rbx + 0x20], eax
000000018032CB4D test      rsi, rsi
000000018032CB50 je        0x18032cbc5
000000018032CB52 mov       r9, qword ptr [rip + 0x187d9af] # 0x181baa508
000000018032CB59 mov       r8, rbx
000000018032CB5C mov       edx, dword ptr [rsi + 0x18]
000000018032CB5F mov       rcx, rsi
000000018032CB62 call      0x18084d9c0
000000018032CB67 mov       rax, rsi
000000018032CB6A mov       r15, qword ptr [rsp + 0x70]
000000018032CB6F mov       r14, qword ptr [rsp + 0x78]
000000018032CB74 mov       r13, qword ptr [rsp + 0x80]
000000018032CB7C mov       r12, qword ptr [rsp + 0x88]
000000018032CB84 mov       rsi, qword ptr [rsp + 0x90]
000000018032CB8C mov       rbp, qword ptr [rsp + 0xc0]
000000018032CB94 add       rsp, 0x98
000000018032CB9B pop       rdi
000000018032CB9C pop       rbx
000000018032CB9D ret       
000000018032CB9E mov       rcx, qword ptr [rip + 0x186e1a3] # 0x181b9ad48
000000018032CBA5 cmp       dword ptr [rcx + 0xe0], 0
000000018032CBAC jne       0x18032cbb3
000000018032CBAE call      0x1802501e0
000000018032CBB3 mov       rcx, qword ptr [rip + 0x187674e] # 0x181ba3308
000000018032CBBA xor       edx, edx
000000018032CBBC call      0x1812e68f0
000000018032CBC1 xor       eax, eax
000000018032CBC3 jmp       0x18032cb6a
000000018032CBC5 call      0x180250150
000000018032CBCA int3      
000000018032CBCB call      0x180250140
000000018032CBD0 int3      
000000018032CBD1 int3      
000000018032CBD2 int3      
000000018032CBD3 int3      
000000018032CBD4 int3      
000000018032CBD5 int3      
000000018032CBD6 int3      
000000018032CBD7 int3      
000000018032CBD8 int3      
000000018032CBD9 int3      
000000018032CBDA int3      
000000018032CBDB int3      
000000018032CBDC int3      
000000018032CBDD int3      
000000018032CBDE int3      
000000018032CBDF int3      