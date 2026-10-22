000000018039B7D0 push      rbx
000000018039B7D2 push      rsi
000000018039B7D3 sub       rsp, 0x58
000000018039B7D7 cmp       byte ptr [rip + 0x19496b7], 0 # 0x181ce4e95
000000018039B7DE mov       rsi, rcx
000000018039B7E1 jne       0x18039b84a
000000018039B7E3 lea       rcx, [rip + 0x1809eae] # 0x181ba5698
000000018039B7EA call      0x18024fef0
000000018039B7EF lea       rcx, [rip + 0x1809f02] # 0x181ba56f8
000000018039B7F6 call      0x18024fef0
000000018039B7FB lea       rcx, [rip + 0x180aab6] # 0x181ba62b8
000000018039B802 call      0x18024fef0
000000018039B807 lea       rcx, [rip + 0x180ab0a] # 0x181ba6318
000000018039B80E call      0x18024fef0
000000018039B813 lea       rcx, [rip + 0x181ce8e] # 0x181bb86a8
000000018039B81A call      0x18024fef0
000000018039B81F lea       rcx, [rip + 0x1814cda] # 0x181bb0500
000000018039B826 call      0x18024fef0
000000018039B82B lea       rcx, [rip + 0x181906e] # 0x181bb48a0
000000018039B832 call      0x18024fef0
000000018039B837 lea       rcx, [rip + 0x1806b6a] # 0x181ba23a8
000000018039B83E call      0x18024fef0
000000018039B843 mov       byte ptr [rip + 0x194964b], 1 # 0x181ce4e95
000000018039B84A mov       rax, qword ptr [rsi + 0x20]
000000018039B84E xor       ebx, ebx
000000018039B850 mov       qword ptr [rsp + 0x78], rbp
000000018039B855 xor       ecx, ecx
000000018039B857 mov       qword ptr [rsp + 0x50], rdi
000000018039B85C mov       qword ptr [rsp + 0x48], r14
000000018039B861 mov       qword ptr [rsp + 0x40], r15
000000018039B866 movaps    xmmword ptr [rsp + 0x30], xmm6
000000018039B86B mov       dword ptr [rsp + 0x70], 0
000000018039B873 test      rax, rax
000000018039B876 je        0x18039bb7b
000000018039B87C xorps     xmm6, xmm6
000000018039B87F nop       
000000018039B880 mov       rax, qword ptr [rax + 0x18]
000000018039B884 test      rax, rax
000000018039B887 je        0x18039bb7b
000000018039B88D cmp       ecx, dword ptr [rax + 0x18]
000000018039B890 jge       0x18039ba40
000000018039B896 mov       rcx, qword ptr [rsi + 0x28]
000000018039B89A mov       eax, 0x66666667
000000018039B89F mov       r15, qword ptr [rsi + 0x30]
000000018039B8A3 mov       r14d, ebx
000000018039B8A6 imul      ebx
000000018039B8A8 sar       edx, 1
000000018039B8AA mov       eax, edx
000000018039B8AC shr       eax, 0x1f
000000018039B8AF add       edx, eax
000000018039B8B1 lea       eax, [rdx + rdx*4]
000000018039B8B4 sub       r14d, eax
000000018039B8B7 mov       eax, 0x66666667
000000018039B8BC imul      ebx
000000018039B8BE mov       ebp, edx
000000018039B8C0 sar       ebp, 1
000000018039B8C2 mov       eax, ebp
000000018039B8C4 shr       eax, 0x1f
000000018039B8C7 add       ebp, eax
000000018039B8C9 test      rcx, rcx
000000018039B8CC je        0x18039bb7b
000000018039B8D2 xor       edx, edx
000000018039B8D4 call      0x18131c070
000000018039B8D9 mov       rcx, qword ptr [rip + 0x1814c20] # 0x181bb0500
000000018039B8E0 mov       rdi, rax
000000018039B8E3 cmp       dword ptr [rcx + 0xe0], 0
000000018039B8EA jne       0x18039b8f1
000000018039B8EC call      0x1802501e0
000000018039B8F1 mov       r8, qword ptr [rip + 0x181cdb0] # 0x181bb86a8
000000018039B8F8 mov       rdx, rdi
000000018039B8FB mov       rcx, r15
000000018039B8FE call      0x1804a05a0
000000018039B903 mov       rdi, rax
000000018039B906 test      rax, rax
000000018039B909 je        0x18039bb7b
000000018039B90F xor       r8d, r8d
000000018039B912 mov       dl, 1
000000018039B914 mov       rcx, rax
000000018039B917 call      0x18131bad0
000000018039B91C xor       edx, edx
000000018039B91E mov       rcx, rdi
000000018039B921 call      0x18131c070
000000018039B926 mov       rcx, rax
000000018039B929 lea       eax, [r14 + r14*4]
000000018039B92D shl       eax, 5
000000018039B930 movd      xmm0, eax
000000018039B934 imul      eax, ebp, 0xffffff74
000000018039B93A cvtdq2ps  xmm0, xmm0
000000018039B93D movd      xmm1, eax
000000018039B941 cvtdq2ps  xmm1, xmm1
000000018039B944 test      rcx, rcx
000000018039B947 je        0x18039bb7b
000000018039B94D unpcklps  xmm0, xmm1
000000018039B950 lea       rdx, [rsp + 0x20]
000000018039B955 xor       r8d, r8d
000000018039B958 movsd     qword ptr [rsp + 0x20], xmm0
000000018039B95E movss     dword ptr [rsp + 0x28], xmm6
000000018039B964 call      0x18132f910
000000018039B969 mov       rdx, qword ptr [rip + 0x1809d28] # 0x181ba5698
000000018039B970 mov       rcx, rdi
000000018039B973 call      0x18046d5e0
000000018039B978 mov       rcx, qword ptr [rsi + 0x20]
000000018039B97C mov       rbp, rax
000000018039B97F test      rcx, rcx
000000018039B982 je        0x18039bb7b
000000018039B988 mov       rcx, qword ptr [rcx + 0x18]
000000018039B98C test      rcx, rcx
000000018039B98F je        0x18039bb7b
000000018039B995 mov       r8, qword ptr [rip + 0x180a97c] # 0x181ba6318
000000018039B99C mov       edx, ebx
000000018039B99E call      0x180825710
000000018039B9A3 test      rax, rax
000000018039B9A6 je        0x18039bb7b
000000018039B9AC test      rbp, rbp
000000018039B9AF je        0x18039bb7b
000000018039B9B5 mov       rdx, qword ptr [rax + 0x18]
000000018039B9B9 xor       r8d, r8d
000000018039B9BC mov       rcx, rbp
000000018039B9BF call      0x1813d7ab0
000000018039B9C4 mov       rdx, qword ptr [rip + 0x1809d2d] # 0x181ba56f8
000000018039B9CB mov       rcx, rdi
000000018039B9CE call      0x18046d5e0
000000018039B9D3 mov       rcx, qword ptr [rsi + 0x20]
000000018039B9D7 mov       rdi, rax
000000018039B9DA test      rcx, rcx
000000018039B9DD je        0x18039bb7b
000000018039B9E3 mov       rcx, qword ptr [rcx + 0x18]
000000018039B9E7 test      rcx, rcx
000000018039B9EA je        0x18039bb7b
000000018039B9F0 mov       r8, qword ptr [rip + 0x180a921] # 0x181ba6318
000000018039B9F7 mov       edx, ebx
000000018039B9F9 call      0x180825710
000000018039B9FE mov       rdx, rax
000000018039BA01 test      rax, rax
000000018039BA04 je        0x18039bb7b
000000018039BA0A test      rdi, rdi
000000018039BA0D je        0x18039bb7b
000000018039BA13 mov       rax, qword ptr [rdi]
000000018039BA16 mov       rcx, rdi
000000018039BA19 mov       rdx, qword ptr [rdx + 0x10]
000000018039BA1D mov       r8, qword ptr [rax + 0x560]
000000018039BA24 call      qword ptr [rax + 0x558]
000000018039BA2A mov       rax, qword ptr [rsi + 0x20]
000000018039BA2E inc       ebx
000000018039BA30 mov       ecx, ebx
000000018039BA32 test      rax, rax
000000018039BA35 je        0x18039bb7b
000000018039BA3B jmp       0x18039b880
000000018039BA40 mov       rcx, qword ptr [rsi + 0x20]
000000018039BA44 test      rcx, rcx
000000018039BA47 je        0x18039bb7b
000000018039BA4D mov       rcx, qword ptr [rcx + 0x18]
000000018039BA51 test      rcx, rcx
000000018039BA54 je        0x18039bb7b
000000018039BA5A mov       ecx, dword ptr [rcx + 0x18]
000000018039BA5D mov       eax, 0x66666667
000000018039BA62 add       ecx, 4
000000018039BA65 imul      ecx
000000018039BA67 mov       rcx, qword ptr [rsi + 0x28]
000000018039BA6B sar       edx, 1
000000018039BA6D mov       eax, edx
000000018039BA6F shr       eax, 0x1f
000000018039BA72 add       edx, eax
000000018039BA74 imul      edi, edx, 0x8c
000000018039BA7A test      rcx, rcx
000000018039BA7D je        0x18039bb7b
000000018039BA83 xor       edx, edx
000000018039BA85 call      0x18131c070
000000018039BA8A mov       rdx, qword ptr [rip + 0x1818e0f] # 0x181bb48a0
000000018039BA91 test      rax, rax
000000018039BA94 je        0x18039bb7b
000000018039BA9A xor       ebx, ebx
000000018039BA9C cmp       qword ptr [rax], rdx
000000018039BA9F cmove     rbx, rax
000000018039BAA3 test      rbx, rbx
000000018039BAA6 je        0x18039bb72
000000018039BAAC xor       edx, edx
000000018039BAAE mov       rcx, rbx
000000018039BAB1 call      0x181324b10
000000018039BAB6 movd      xmm1, edi
000000018039BABA xor       r8d, r8d
000000018039BABD cvtdq2ps  xmm1, xmm1
000000018039BAC0 mov       qword ptr [rsp + 0x80], rax
000000018039BAC8 mov       rcx, rbx
000000018039BACB movss     xmm0, dword ptr [rsp + 0x80]
000000018039BAD4 unpcklps  xmm0, xmm1
000000018039BAD7 movq      rdx, xmm0
000000018039BADC call      0x181325360
000000018039BAE1 mov       rbx, qword ptr [rsi + 0x38]
000000018039BAE5 lea       rcx, [rsp + 0x70]
000000018039BAEA xor       edx, edx
000000018039BAEC mov       dword ptr [rsp + 0x70], 0
000000018039BAF4 call      0x180ca3820
000000018039BAF9 mov       rcx, qword ptr [rsi + 0x20]
000000018039BAFD mov       rdi, rax
000000018039BB00 test      rcx, rcx
000000018039BB03 je        0x18039bb7b
000000018039BB05 mov       rcx, qword ptr [rcx + 0x18]
000000018039BB09 test      rcx, rcx
000000018039BB0C je        0x18039bb7b
000000018039BB0E mov       ecx, dword ptr [rcx + 0x18]
000000018039BB11 xor       edx, edx
000000018039BB13 mov       dword ptr [rsp + 0x70], ecx
000000018039BB17 lea       rcx, [rsp + 0x70]
000000018039BB1C call      0x180ca3820
000000018039BB21 mov       rdx, qword ptr [rip + 0x1806880] # 0x181ba23a8
000000018039BB28 xor       r9d, r9d
000000018039BB2B mov       r8, rax
000000018039BB2E mov       rcx, rdi
000000018039BB31 call      0x180b73f50
000000018039BB36 test      rbx, rbx
000000018039BB39 je        0x18039bb7b
000000018039BB3B mov       r9, qword ptr [rbx]
000000018039BB3E mov       rdx, rax
000000018039BB41 mov       rcx, rbx
000000018039BB44 mov       r8, qword ptr [r9 + 0x560]
000000018039BB4B call      qword ptr [r9 + 0x558]
000000018039BB52 movaps    xmm6, xmmword ptr [rsp + 0x30]
000000018039BB57 mov       r15, qword ptr [rsp + 0x40]
000000018039BB5C mov       r14, qword ptr [rsp + 0x48]
000000018039BB61 mov       rdi, qword ptr [rsp + 0x50]
000000018039BB66 mov       rbp, qword ptr [rsp + 0x78]
000000018039BB6B add       rsp, 0x58
000000018039BB6F pop       rsi
000000018039BB70 pop       rbx
000000018039BB71 ret       
000000018039BB72 mov       rcx, rax
000000018039BB75 call      0x18024f3a0
000000018039BB7A int3      
000000018039BB7B call      0x180250150
000000018039BB80 int3      
000000018039BB81 int3      
000000018039BB82 int3      
000000018039BB83 int3      
000000018039BB84 int3      
000000018039BB85 int3      
000000018039BB86 int3      
000000018039BB87 int3      
000000018039BB88 int3      
000000018039BB89 int3      
000000018039BB8A int3      
000000018039BB8B int3      
000000018039BB8C int3      
000000018039BB8D int3      
000000018039BB8E int3      
000000018039BB8F int3      