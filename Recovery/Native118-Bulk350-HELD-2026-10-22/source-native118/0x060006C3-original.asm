00000001803A2610 push      rbx
00000001803A2612 sub       rsp, 0x40
00000001803A2616 cmp       byte ptr [rip + 0x19428f6], 0 # 0x181ce4f13
00000001803A261D mov       rbx, rcx
00000001803A2620 jne       0x1803a2695
00000001803A2622 lea       rcx, [rip + 0x1820747] # 0x181bc2d70
00000001803A2629 call      0x18024fef0
00000001803A262E lea       rcx, [rip + 0x1820ec3] # 0x181bc34f8
00000001803A2635 call      0x18024fef0
00000001803A263A lea       rcx, [rip + 0x1802157] # 0x181ba4798
00000001803A2641 call      0x18024fef0
00000001803A2646 lea       rcx, [rip + 0x1802f4b] # 0x181ba5598
00000001803A264D call      0x18024fef0
00000001803A2652 lea       rcx, [rip + 0x18295df] # 0x181bcbc38
00000001803A2659 call      0x18024fef0
00000001803A265E lea       rcx, [rip + 0x181c6a3] # 0x181bbed08
00000001803A2665 call      0x18024fef0
00000001803A266A lea       rcx, [rip + 0x1809a17] # 0x181bac088
00000001803A2671 call      0x18024fef0
00000001803A2676 lea       rcx, [rip + 0x1813dfb] # 0x181bb6478
00000001803A267D call      0x18024fef0
00000001803A2682 lea       rcx, [rip + 0x17f9497] # 0x181b9bb20
00000001803A2689 call      0x18024fef0
00000001803A268E mov       byte ptr [rip + 0x194287e], 1 # 0x181ce4f13
00000001803A2695 mov       rax, qword ptr [rbx + 0x10]
00000001803A2699 mov       qword ptr [rsp + 0x50], rbp
00000001803A269E mov       qword ptr [rsp + 0x58], rsi
00000001803A26A3 mov       qword ptr [rsp + 0x60], rdi
00000001803A26A8 mov       qword ptr [rsp + 0x38], r12
00000001803A26AD mov       qword ptr [rsp + 0x30], r13
00000001803A26B2 mov       qword ptr [rsp + 0x28], r14
00000001803A26B7 mov       qword ptr [rsp + 0x20], r15
00000001803A26BC test      rax, rax
00000001803A26BF je        0x1803a2e78
00000001803A26C5 mov       r13, qword ptr [rax + 0x48]
00000001803A26C9 test      r13, r13
00000001803A26CC je        0x1803a2e78
00000001803A26D2 mov       rcx, qword ptr [rbx + 0x28]
00000001803A26D6 test      rcx, rcx
00000001803A26D9 je        0x1803a2e78
00000001803A26DF mov       rdx, qword ptr [r13 + 0x10]
00000001803A26E3 xor       r9d, r9d
00000001803A26E6 mov       r8b, 1
00000001803A26E9 call      0x1811af070
00000001803A26EE mov       rcx, qword ptr [rbx + 0x30]
00000001803A26F2 test      rcx, rcx
00000001803A26F5 je        0x1803a2e78
00000001803A26FB mov       r8, qword ptr [rip + 0x1829536] # 0x181bcbc38
00000001803A2702 xor       edx, edx
00000001803A2704 call      0x180825710
00000001803A2709 test      rax, rax
00000001803A270C je        0x1803a2e78
00000001803A2712 movss     xmm1, dword ptr [r13 + 0x6c]
00000001803A2718 xor       r8d, r8d
00000001803A271B mov       rcx, rax
00000001803A271E call      0x18039c060
00000001803A2723 mov       rcx, qword ptr [rbx + 0x30]
00000001803A2727 test      rcx, rcx
00000001803A272A je        0x1803a2e78
00000001803A2730 mov       r8, qword ptr [rip + 0x1829501] # 0x181bcbc38
00000001803A2737 mov       edx, 1
00000001803A273C call      0x180825710
00000001803A2741 test      rax, rax
00000001803A2744 je        0x1803a2e78
00000001803A274A movss     xmm1, dword ptr [r13 + 0x70]
00000001803A2750 xor       r8d, r8d
00000001803A2753 mov       rcx, rax
00000001803A2756 call      0x18039c060
00000001803A275B mov       rcx, qword ptr [rbx + 0x30]
00000001803A275F test      rcx, rcx
00000001803A2762 je        0x1803a2e78
00000001803A2768 mov       r8, qword ptr [rip + 0x18294c9] # 0x181bcbc38
00000001803A276F mov       edx, 2
00000001803A2774 call      0x180825710
00000001803A2779 test      rax, rax
00000001803A277C je        0x1803a2e78
00000001803A2782 movss     xmm1, dword ptr [r13 + 0x74]
00000001803A2788 xor       r8d, r8d
00000001803A278B mov       rcx, rax
00000001803A278E call      0x18039c060
00000001803A2793 mov       rcx, qword ptr [rbx + 0x30]
00000001803A2797 test      rcx, rcx
00000001803A279A je        0x1803a2e78
00000001803A27A0 mov       r8, qword ptr [rip + 0x1829491] # 0x181bcbc38
00000001803A27A7 mov       edx, 3
00000001803A27AC call      0x180825710
00000001803A27B1 test      rax, rax
00000001803A27B4 je        0x1803a2e78
00000001803A27BA movss     xmm1, dword ptr [r13 + 0x78]
00000001803A27C0 xor       r8d, r8d
00000001803A27C3 mov       rcx, rax
00000001803A27C6 call      0x18039c060
00000001803A27CB mov       rcx, qword ptr [rbx + 0x30]
00000001803A27CF test      rcx, rcx
00000001803A27D2 je        0x1803a2e78
00000001803A27D8 mov       r8, qword ptr [rip + 0x1829459] # 0x181bcbc38
00000001803A27DF mov       edx, 4
00000001803A27E4 call      0x180825710
00000001803A27E9 test      rax, rax
00000001803A27EC je        0x1803a2e78
00000001803A27F2 movss     xmm1, dword ptr [r13 + 0x7c]
00000001803A27F8 xor       r8d, r8d
00000001803A27FB mov       rcx, rax
00000001803A27FE call      0x18039c060
00000001803A2803 mov       rcx, qword ptr [rbx + 0x30]
00000001803A2807 test      rcx, rcx
00000001803A280A je        0x1803a2e78
00000001803A2810 mov       r8, qword ptr [rip + 0x1829421] # 0x181bcbc38
00000001803A2817 mov       edx, 5
00000001803A281C call      0x180825710
00000001803A2821 mov       rcx, qword ptr [rbx + 0x10]
00000001803A2825 test      rcx, rcx
00000001803A2828 je        0x1803a2e78
00000001803A282E test      rax, rax
00000001803A2831 je        0x1803a2e78
00000001803A2837 movd      xmm1, dword ptr [rcx + 0x50]
00000001803A283C xor       r8d, r8d
00000001803A283F cvtdq2ps  xmm1, xmm1
00000001803A2842 mov       rcx, rax
00000001803A2845 call      0x18039c060
00000001803A284A mov       rcx, qword ptr [rbx + 0x30]
00000001803A284E test      rcx, rcx
00000001803A2851 je        0x1803a2e78
00000001803A2857 mov       r8, qword ptr [rip + 0x18293da] # 0x181bcbc38
00000001803A285E mov       edx, 6
00000001803A2863 call      0x180825710
00000001803A2868 test      rax, rax
00000001803A286B je        0x1803a2e78
00000001803A2871 movss     xmm1, dword ptr [r13 + 0x64]
00000001803A2877 xor       r8d, r8d
00000001803A287A mov       rcx, rax
00000001803A287D call      0x18039c060
00000001803A2882 mov       rcx, qword ptr [rbx + 0x58]
00000001803A2886 test      rcx, rcx
00000001803A2889 je        0x1803a2e78
00000001803A288F mov       r8, qword ptr [rip + 0x181c472] # 0x181bbed08
00000001803A2896 xor       edx, edx
00000001803A2898 call      0x180825710
00000001803A289D test      rax, rax
00000001803A28A0 je        0x1803a2e78
00000001803A28A6 mov       rdx, qword ptr [r13 + 0x28]
00000001803A28AA xor       r9d, r9d
00000001803A28AD mov       r8b, 1
00000001803A28B0 mov       rcx, rax
00000001803A28B3 call      0x1811af070
00000001803A28B8 xor       r8d, r8d
00000001803A28BB mov       rdx, r13
00000001803A28BE mov       rcx, rbx
00000001803A28C1 call      0x1803a3540
00000001803A28C6 mov       rdx, qword ptr [rbx + 0x10]
00000001803A28CA test      rdx, rdx
00000001803A28CD je        0x1803a2e78
00000001803A28D3 mov       rdx, qword ptr [rdx + 0x58]
00000001803A28D7 test      rdx, rdx
00000001803A28DA je        0x1803a2e78
00000001803A28E0 mov       edx, dword ptr [rdx + 0x18]
00000001803A28E3 xor       r8d, r8d
00000001803A28E6 mov       rcx, rbx
00000001803A28E9 call      0x1803a3770
00000001803A28EE xor       r14d, r14d
00000001803A28F1 mov       edi, r14d
00000001803A28F4 nop       dword ptr [rax]
00000001803A28F8 nop       dword ptr [rax + rax]
00000001803A2900 mov       rax, qword ptr [rbx + 0x10]
00000001803A2904 test      rax, rax
00000001803A2907 je        0x1803a2e78
00000001803A290D mov       rcx, qword ptr [rax + 0x48]
00000001803A2911 test      rcx, rcx
00000001803A2914 je        0x1803a2e78
00000001803A291A lea       edx, [rdi + 1]
00000001803A291D xor       r8d, r8d
00000001803A2920 call      0x180375f80
00000001803A2925 mov       rsi, rax
00000001803A2928 test      rax, rax
00000001803A292B je        0x1803a2dc4
00000001803A2931 cmp       byte ptr [rax + 0x34], r14b
00000001803A2935 je        0x1803a2c2b
00000001803A293B cmp       dword ptr [rax + 0x18], r14d
00000001803A293F je        0x1803a2c2b
00000001803A2945 mov       ecx, dword ptr [r13 + 0x48]
00000001803A2949 mov       r15d, dword ptr [rax + 0x18]
00000001803A294D lea       r12d, [rcx + rcx*2]
00000001803A2951 mov       rcx, qword ptr [rbx + 0x48]
00000001803A2955 test      rcx, rcx
00000001803A2958 je        0x1803a2e78
00000001803A295E mov       r8, qword ptr [rip + 0x1809723] # 0x181bac088
00000001803A2965 mov       edx, edi
00000001803A2967 call      0x180825710
00000001803A296C test      rax, rax
00000001803A296F je        0x1803a2e78
00000001803A2975 xor       edx, edx
00000001803A2977 mov       rcx, rax
00000001803A297A call      0x18131c070
00000001803A297F test      rax, rax
00000001803A2982 je        0x1803a2e78
00000001803A2988 xor       r8d, r8d
00000001803A298B mov       edx, 1
00000001803A2990 mov       rcx, rax
00000001803A2993 call      0x18132dbe0
00000001803A2998 test      rax, rax
00000001803A299B je        0x1803a2e78
00000001803A29A1 mov       rdx, qword ptr [rip + 0x18203c8] # 0x181bc2d70
00000001803A29A8 mov       rcx, rax
00000001803A29AB call      0x18042e9f0
00000001803A29B0 mov       rcx, qword ptr [rip + 0x1813ac1] # 0x181bb6478
00000001803A29B7 add       r15d, r12d
00000001803A29BA mov       rsi, qword ptr [rsi + 0x10]
00000001803A29BE mov       rbp, rax
00000001803A29C1 cmp       dword ptr [rcx + 0xe0], r14d
00000001803A29C8 jne       0x1803a29cf
00000001803A29CA call      0x1802501e0
00000001803A29CF xor       r8d, r8d
00000001803A29D2 mov       rdx, rsi
00000001803A29D5 mov       ecx, r15d
00000001803A29D8 call      0x1803339e0
00000001803A29DD test      rbp, rbp
00000001803A29E0 je        0x1803a2e78
00000001803A29E6 xor       r8d, r8d
00000001803A29E9 mov       rdx, rax
00000001803A29EC mov       rcx, rbp
00000001803A29EF call      0x1813d7ab0
00000001803A29F4 mov       rcx, qword ptr [rbx + 0x48]
00000001803A29F8 test      rcx, rcx
00000001803A29FB je        0x1803a2e78
00000001803A2A01 mov       r8, qword ptr [rip + 0x1809680] # 0x181bac088
00000001803A2A08 mov       edx, edi
00000001803A2A0A call      0x180825710
00000001803A2A0F test      rax, rax
00000001803A2A12 je        0x1803a2e78
00000001803A2A18 xor       edx, edx
00000001803A2A1A mov       rcx, rax
00000001803A2A1D call      0x18131c070
00000001803A2A22 test      rax, rax
00000001803A2A25 je        0x1803a2e78
00000001803A2A2B xor       r8d, r8d
00000001803A2A2E mov       edx, 1
00000001803A2A33 mov       rcx, rax
00000001803A2A36 call      0x18132dbe0
00000001803A2A3B test      rax, rax
00000001803A2A3E je        0x1803a2e78
00000001803A2A44 xor       edx, edx
00000001803A2A46 mov       rcx, rax
00000001803A2A49 call      0x1813044d0
00000001803A2A4E test      rax, rax
00000001803A2A51 je        0x1803a2e78
00000001803A2A57 xor       r8d, r8d
00000001803A2A5A mov       dl, 1
00000001803A2A5C mov       rcx, rax
00000001803A2A5F call      0x18131bad0
00000001803A2A64 mov       rcx, qword ptr [rbx + 0x48]
00000001803A2A68 test      rcx, rcx
00000001803A2A6B je        0x1803a2e78
00000001803A2A71 mov       r8, qword ptr [rip + 0x1809610] # 0x181bac088
00000001803A2A78 mov       edx, edi
00000001803A2A7A call      0x180825710
00000001803A2A7F test      rax, rax
00000001803A2A82 je        0x1803a2e78
00000001803A2A88 xor       edx, edx
00000001803A2A8A mov       rcx, rax
00000001803A2A8D call      0x18131c070
00000001803A2A92 test      rax, rax
00000001803A2A95 je        0x1803a2e78
00000001803A2A9B xor       r8d, r8d
00000001803A2A9E mov       edx, 4
00000001803A2AA3 mov       rcx, rax
00000001803A2AA6 call      0x18132dbe0
00000001803A2AAB test      rax, rax
00000001803A2AAE je        0x1803a2e78
00000001803A2AB4 xor       edx, edx
00000001803A2AB6 mov       rcx, rax
00000001803A2AB9 call      0x1813044d0
00000001803A2ABE test      rax, rax
00000001803A2AC1 je        0x1803a2e78
00000001803A2AC7 xor       r8d, r8d
00000001803A2ACA xor       edx, edx
00000001803A2ACC mov       rcx, rax
00000001803A2ACF call      0x18131bad0
00000001803A2AD4 mov       rcx, qword ptr [rbx + 0x48]
00000001803A2AD8 test      rcx, rcx
00000001803A2ADB je        0x1803a2e78
00000001803A2AE1 mov       r8, qword ptr [rip + 0x18095a0] # 0x181bac088
00000001803A2AE8 mov       edx, edi
00000001803A2AEA call      0x180825710
00000001803A2AEF test      rax, rax
00000001803A2AF2 je        0x1803a2e78
00000001803A2AF8 mov       rdx, qword ptr [rip + 0x1801c99] # 0x181ba4798
00000001803A2AFF mov       rcx, rax
00000001803A2B02 call      0x18046dc80
00000001803A2B07 test      rax, rax
00000001803A2B0A je        0x1803a2e78
00000001803A2B10 xor       r8d, r8d
00000001803A2B13 mov       dl, 1
00000001803A2B15 mov       rcx, rax
00000001803A2B18 call      0x181302cb0
00000001803A2B1D mov       rcx, qword ptr [rbx + 0x48]
00000001803A2B21 test      rcx, rcx
00000001803A2B24 je        0x1803a2e78
00000001803A2B2A mov       r8, qword ptr [rip + 0x1809557] # 0x181bac088
00000001803A2B31 mov       edx, edi
00000001803A2B33 call      0x180825710
00000001803A2B38 test      rax, rax
00000001803A2B3B je        0x1803a2e78
00000001803A2B41 xor       edx, edx
00000001803A2B43 mov       rcx, rax
00000001803A2B46 call      0x18131c070
00000001803A2B4B test      rax, rax
00000001803A2B4E je        0x1803a2e78
00000001803A2B54 xor       r8d, r8d
00000001803A2B57 mov       edx, 5
00000001803A2B5C mov       rcx, rax
00000001803A2B5F call      0x18132dbe0
00000001803A2B64 test      rax, rax
00000001803A2B67 je        0x1803a2e78
00000001803A2B6D xor       edx, edx
00000001803A2B6F mov       rcx, rax
00000001803A2B72 call      0x1813044d0
00000001803A2B77 test      rax, rax
00000001803A2B7A je        0x1803a2e78
00000001803A2B80 xor       r8d, r8d
00000001803A2B83 mov       dl, 1
00000001803A2B85 mov       rcx, rax
00000001803A2B88 call      0x18131bad0
00000001803A2B8D mov       rcx, qword ptr [rbx + 0x48]
00000001803A2B91 test      rcx, rcx
00000001803A2B94 je        0x1803a2e78
00000001803A2B9A mov       r8, qword ptr [rip + 0x18094e7] # 0x181bac088
00000001803A2BA1 mov       edx, edi
00000001803A2BA3 call      0x180825710
00000001803A2BA8 test      rax, rax
00000001803A2BAB je        0x1803a2e78
00000001803A2BB1 xor       edx, edx
00000001803A2BB3 mov       rcx, rax
00000001803A2BB6 call      0x18131c070
00000001803A2BBB test      rax, rax
00000001803A2BBE je        0x1803a2e78
00000001803A2BC4 xor       r8d, r8d
00000001803A2BC7 mov       edx, 5
00000001803A2BCC mov       rcx, rax
00000001803A2BCF call      0x18132dbe0
00000001803A2BD4 test      rax, rax
00000001803A2BD7 je        0x1803a2e78
00000001803A2BDD mov       rdx, qword ptr [rip + 0x1820914] # 0x181bc34f8
00000001803A2BE4 mov       rcx, rax
00000001803A2BE7 call      0x18042e9f0
00000001803A2BEC lea       rcx, [r13 + 0x5c]
00000001803A2BF0 xor       edx, edx
00000001803A2BF2 mov       rsi, rax
00000001803A2BF5 call      0x180ca3820
00000001803A2BFA mov       rcx, qword ptr [rip + 0x17f8f1f] # 0x181b9bb20
00000001803A2C01 xor       r8d, r8d
00000001803A2C04 mov       rdx, rax
00000001803A2C07 call      0x180b74270
00000001803A2C0C test      rsi, rsi
00000001803A2C0F je        0x1803a2e78
00000001803A2C15 xor       r9d, r9d
00000001803A2C18 mov       r8b, 1
00000001803A2C1B mov       rdx, rax
00000001803A2C1E mov       rcx, rsi
00000001803A2C21 call      0x1811af070
00000001803A2C26 jmp       0x1803a2dc4
00000001803A2C2B mov       rcx, qword ptr [rbx + 0x48]
00000001803A2C2F test      rcx, rcx
00000001803A2C32 je        0x1803a2e78
00000001803A2C38 mov       r8, qword ptr [rip + 0x1809449] # 0x181bac088
00000001803A2C3F mov       edx, edi
00000001803A2C41 call      0x180825710
00000001803A2C46 test      rax, rax
00000001803A2C49 je        0x1803a2e78
00000001803A2C4F xor       edx, edx
00000001803A2C51 mov       rcx, rax
00000001803A2C54 call      0x18131c070
00000001803A2C59 test      rax, rax
00000001803A2C5C je        0x1803a2e78
00000001803A2C62 xor       r8d, r8d
00000001803A2C65 mov       edx, 1
00000001803A2C6A mov       rcx, rax
00000001803A2C6D call      0x18132dbe0
00000001803A2C72 test      rax, rax
00000001803A2C75 je        0x1803a2e78
00000001803A2C7B xor       edx, edx
00000001803A2C7D mov       rcx, rax
00000001803A2C80 call      0x1813044d0
00000001803A2C85 test      rax, rax
00000001803A2C88 je        0x1803a2e78
00000001803A2C8E xor       r8d, r8d
00000001803A2C91 xor       edx, edx
00000001803A2C93 mov       rcx, rax
00000001803A2C96 call      0x18131bad0
00000001803A2C9B mov       rcx, qword ptr [rbx + 0x48]
00000001803A2C9F test      rcx, rcx
00000001803A2CA2 je        0x1803a2e78
00000001803A2CA8 mov       r8, qword ptr [rip + 0x18093d9] # 0x181bac088
00000001803A2CAF mov       edx, edi
00000001803A2CB1 call      0x180825710
00000001803A2CB6 test      rax, rax
00000001803A2CB9 je        0x1803a2e78
00000001803A2CBF xor       edx, edx
00000001803A2CC1 mov       rcx, rax
00000001803A2CC4 call      0x18131c070
00000001803A2CC9 test      rax, rax
00000001803A2CCC je        0x1803a2e78
00000001803A2CD2 xor       r8d, r8d
00000001803A2CD5 mov       edx, 4
00000001803A2CDA mov       rcx, rax
00000001803A2CDD call      0x18132dbe0
00000001803A2CE2 test      rax, rax
00000001803A2CE5 je        0x1803a2e78
00000001803A2CEB xor       edx, edx
00000001803A2CED mov       rcx, rax
00000001803A2CF0 call      0x1813044d0
00000001803A2CF5 test      rax, rax
00000001803A2CF8 je        0x1803a2e78
00000001803A2CFE xor       r8d, r8d
00000001803A2D01 mov       dl, 1
00000001803A2D03 mov       rcx, rax
00000001803A2D06 call      0x18131bad0
00000001803A2D0B mov       rcx, qword ptr [rbx + 0x48]
00000001803A2D0F test      rcx, rcx
00000001803A2D12 je        0x1803a2e78
00000001803A2D18 mov       r8, qword ptr [rip + 0x1809369] # 0x181bac088
00000001803A2D1F mov       edx, edi
00000001803A2D21 call      0x180825710
00000001803A2D26 test      rax, rax
00000001803A2D29 je        0x1803a2e78
00000001803A2D2F mov       rdx, qword ptr [rip + 0x1801a62] # 0x181ba4798
00000001803A2D36 mov       rcx, rax
00000001803A2D39 call      0x18046dc80
00000001803A2D3E test      rax, rax
00000001803A2D41 je        0x1803a2e78
00000001803A2D47 xor       r8d, r8d
00000001803A2D4A xor       edx, edx
00000001803A2D4C mov       rcx, rax
00000001803A2D4F call      0x181302cb0
00000001803A2D54 mov       rcx, qword ptr [rbx + 0x48]
00000001803A2D58 test      rcx, rcx
00000001803A2D5B je        0x1803a2e78
00000001803A2D61 mov       r8, qword ptr [rip + 0x1809320] # 0x181bac088
00000001803A2D68 mov       edx, edi
00000001803A2D6A call      0x180825710
00000001803A2D6F test      rax, rax
00000001803A2D72 je        0x1803a2e78
00000001803A2D78 xor       edx, edx
00000001803A2D7A mov       rcx, rax
00000001803A2D7D call      0x18131c070
00000001803A2D82 test      rax, rax
00000001803A2D85 je        0x1803a2e78
00000001803A2D8B xor       r8d, r8d
00000001803A2D8E mov       edx, 5
00000001803A2D93 mov       rcx, rax
00000001803A2D96 call      0x18132dbe0
00000001803A2D9B test      rax, rax
00000001803A2D9E je        0x1803a2e78
00000001803A2DA4 xor       edx, edx
00000001803A2DA6 mov       rcx, rax
00000001803A2DA9 call      0x1813044d0
00000001803A2DAE test      rax, rax
00000001803A2DB1 je        0x1803a2e78
00000001803A2DB7 xor       r8d, r8d
00000001803A2DBA xor       edx, edx
00000001803A2DBC mov       rcx, rax
00000001803A2DBF call      0x18131bad0
00000001803A2DC4 inc       edi
00000001803A2DC6 cmp       edi, 3
00000001803A2DC9 jl        0x1803a2900
00000001803A2DCF mov       rax, qword ptr [rbx + 0x18]
00000001803A2DD3 mov       ecx, r14d
00000001803A2DD6 test      rax, rax
00000001803A2DD9 je        0x1803a2e78
00000001803A2DDF nop       
00000001803A2DE0 cmp       ecx, dword ptr [rax + 0x18]
00000001803A2DE3 jge       0x1803a2e4f
00000001803A2DE5 mov       ecx, r14d
00000001803A2DE8 test      r14d, r14d
00000001803A2DEB je        0x1803a2e12
00000001803A2DED sub       ecx, 1
00000001803A2DF0 je        0x1803a2e41
00000001803A2DF2 cmp       ecx, 1
00000001803A2DF5 jne       0x1803a2e30
00000001803A2DF7 mov       rax, qword ptr [rbx + 0x10]
00000001803A2DFB test      rax, rax
00000001803A2DFE je        0x1803a2e78
00000001803A2E00 mov       rcx, qword ptr [rax + 0x48]
00000001803A2E04 test      rcx, rcx
00000001803A2E07 je        0x1803a2e78
00000001803A2E09 xor       edx, edx
00000001803A2E0B call      0x180376130
00000001803A2E10 jmp       0x1803a2e2b
00000001803A2E12 mov       rax, qword ptr [rbx + 0x10]
00000001803A2E16 test      rax, rax
00000001803A2E19 je        0x1803a2e78
00000001803A2E1B mov       rcx, qword ptr [rax + 0x48]
00000001803A2E1F test      rcx, rcx
00000001803A2E22 je        0x1803a2e78
00000001803A2E24 xor       edx, edx
00000001803A2E26 call      0x180375cd0
00000001803A2E2B cmp       eax, 1
00000001803A2E2E jge       0x1803a2e41
00000001803A2E30 mov       rax, qword ptr [rbx + 0x18]
00000001803A2E34 inc       r14d
00000001803A2E37 mov       ecx, r14d
00000001803A2E3A test      rax, rax
00000001803A2E3D je        0x1803a2e78
00000001803A2E3F jmp       0x1803a2de0
00000001803A2E41 xor       r8d, r8d
00000001803A2E44 mov       edx, r14d
00000001803A2E47 mov       rcx, rbx
00000001803A2E4A call      0x1803a2400
00000001803A2E4F mov       r15, qword ptr [rsp + 0x20]
00000001803A2E54 mov       r14, qword ptr [rsp + 0x28]
00000001803A2E59 mov       r13, qword ptr [rsp + 0x30]
00000001803A2E5E mov       r12, qword ptr [rsp + 0x38]
00000001803A2E63 mov       rdi, qword ptr [rsp + 0x60]
00000001803A2E68 mov       rsi, qword ptr [rsp + 0x58]
00000001803A2E6D mov       rbp, qword ptr [rsp + 0x50]
00000001803A2E72 add       rsp, 0x40
00000001803A2E76 pop       rbx
00000001803A2E77 ret       
00000001803A2E78 call      0x180250150
00000001803A2E7D int3      
00000001803A2E7E int3      
00000001803A2E7F int3      