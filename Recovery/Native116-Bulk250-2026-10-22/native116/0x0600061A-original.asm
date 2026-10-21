000000018039CBA0 mov       qword ptr [rsp + 0x18], rsi
000000018039CBA5 mov       qword ptr [rsp + 0x20], rdi
000000018039CBAA push      r14
000000018039CBAC sub       rsp, 0x80
000000018039CBB3 cmp       byte ptr [rip + 0x19482ea], 0 # 0x181ce4ea4
000000018039CBBA mov       esi, edx
000000018039CBBC movaps    xmmword ptr [rsp + 0x30], xmm10
000000018039CBC2 mov       r14, rcx
000000018039CBC5 jne       0x18039cc0a
000000018039CBC7 lea       rcx, [rip + 0x180f1a2] # 0x181babd70
000000018039CBCE call      0x18024fef0
000000018039CBD3 lea       rcx, [rip + 0x180f4ae] # 0x181bac088
000000018039CBDA call      0x18024fef0
000000018039CBDF lea       rcx, [rip + 0x181afc2] # 0x181bb7ba8
000000018039CBE6 call      0x18024fef0
000000018039CBEB lea       rcx, [rip + 0x181390e] # 0x181bb0500
000000018039CBF2 call      0x18024fef0
000000018039CBF7 lea       rcx, [rip + 0x181987a] # 0x181bb6478
000000018039CBFE call      0x18024fef0
000000018039CC03 mov       byte ptr [rip + 0x194829a], 1 # 0x181ce4ea4
000000018039CC0A movss     xmm10, dword ptr [rip + 0x120b37d] # 0x1815a7f90
000000018039CC13 xor       eax, eax
000000018039CC15 mov       qword ptr [rsp + 0x90], rbx
000000018039CC1D xor       edi, edi
000000018039CC1F mov       qword ptr [rsp + 0x98], rbp
000000018039CC27 movaps    xmmword ptr [rsp + 0x70], xmm6
000000018039CC2C movaps    xmmword ptr [rsp + 0x60], xmm7
000000018039CC31 movd      xmm0, esi
000000018039CC35 movaps    xmmword ptr [rsp + 0x50], xmm8
000000018039CC3B movaps    xmmword ptr [rsp + 0x40], xmm9
000000018039CC41 mov       qword ptr [rsp + 0x20], rax
000000018039CC46 mov       dword ptr [rsp + 0x28], eax
000000018039CC4A cvtdq2ps  xmm0, xmm0
000000018039CC4D divss     xmm10, xmm0
000000018039CC52 test      esi, esi
000000018039CC54 jle       0x18039cd2b
000000018039CC5A nop       word ptr [rax + rax]
000000018039CC60 mov       rax, qword ptr [rip + 0x1819811] # 0x181bb6478
000000018039CC67 mov       rbx, qword ptr [r14 + 0x50]
000000018039CC6B cmp       dword ptr [rax + 0xe0], 0
000000018039CC72 jne       0x18039cc83
000000018039CC74 mov       rcx, rax
000000018039CC77 call      0x1802501e0
000000018039CC7C mov       rax, qword ptr [rip + 0x18197f5] # 0x181bb6478
000000018039CC83 mov       rcx, qword ptr [rip + 0x1813876] # 0x181bb0500
000000018039CC8A mov       rax, qword ptr [rax + 0xb8]
000000018039CC91 cmp       dword ptr [rcx + 0xe0], 0
000000018039CC98 mov       rbp, qword ptr [rax + 0x218]
000000018039CC9F jne       0x18039cca6
000000018039CCA1 call      0x1802501e0
000000018039CCA6 mov       rdx, qword ptr [rip + 0x181aefb] # 0x181bb7ba8
000000018039CCAD mov       rcx, rbp
000000018039CCB0 call      0x1804a0690
000000018039CCB5 mov       r9, rax
000000018039CCB8 test      rbx, rbx
000000018039CCBB je        0x18039ce5e
000000018039CCC1 mov       rax, qword ptr [rip + 0x180f0a8] # 0x181babd70
000000018039CCC8 inc       dword ptr [rbx + 0x1c]
000000018039CCCB mov       rcx, qword ptr [rbx + 0x10]
000000018039CCCF movsxd    rdx, dword ptr [rbx + 0x18]
000000018039CCD3 test      rcx, rcx
000000018039CCD6 je        0x18039ce5e
000000018039CCDC cmp       edx, dword ptr [rcx + 0x18]
000000018039CCDF jb        0x18039ccfd
000000018039CCE1 mov       rax, qword ptr [rax + 0x20]
000000018039CCE5 mov       rdx, r9
000000018039CCE8 mov       rcx, rbx
000000018039CCEB mov       r8, qword ptr [rax + 0xc0]
000000018039CCF2 mov       r8, qword ptr [r8 + 0x70]
000000018039CCF6 call      0x180847ce0
000000018039CCFB jmp       0x18039cd21
000000018039CCFD lea       eax, [rdx + 1]
000000018039CD00 mov       dword ptr [rbx + 0x18], eax
000000018039CD03 cmp       edx, dword ptr [rcx + 0x18]
000000018039CD06 jae       0x18039ce64
000000018039CD0C mov       qword ptr [rcx + rdx*8 + 0x20], r9
000000018039CD11 lea       rcx, [rcx + rdx*8]
000000018039CD15 add       rcx, 0x20
000000018039CD19 mov       rdx, r9
000000018039CD1C call      0x18024f360
000000018039CD21 inc       edi
000000018039CD23 cmp       edi, esi
000000018039CD25 jl        0x18039cc60
000000018039CD2B lea       ebx, [rsi - 1]
000000018039CD2E test      ebx, ebx
000000018039CD30 js        0x18039ce1d
000000018039CD36 movss     xmm8, dword ptr [rip + 0x120aea1] # 0x1815a7be0
000000018039CD3F sub       esi, ebx
000000018039CD41 movss     xmm7, dword ptr [rip + 0x120b23f] # 0x1815a7f88
000000018039CD49 dec       esi
000000018039CD4B xorps     xmm9, xmm9
000000018039CD4F nop       
000000018039CD50 mov       rcx, qword ptr [r14 + 0x50]
000000018039CD54 test      rcx, rcx
000000018039CD57 je        0x18039ce5e
000000018039CD5D mov       r8, qword ptr [rip + 0x180f324] # 0x181bac088
000000018039CD64 mov       edx, ebx
000000018039CD66 call      0x180825710
000000018039CD6B test      rax, rax
000000018039CD6E je        0x18039ce5e
000000018039CD74 xor       edx, edx
000000018039CD76 mov       rcx, rax
000000018039CD79 call      0x18131c070
000000018039CD7E mov       rcx, qword ptr [r14 + 0x48]
000000018039CD82 mov       rdi, rax
000000018039CD85 test      rcx, rcx
000000018039CD88 je        0x18039ce5e
000000018039CD8E xor       edx, edx
000000018039CD90 call      0x18131c070
000000018039CD95 test      rdi, rdi
000000018039CD98 je        0x18039ce5e
000000018039CD9E xor       r9d, r9d
000000018039CDA1 xor       r8d, r8d
000000018039CDA4 mov       rdx, rax
000000018039CDA7 mov       rcx, rdi
000000018039CDAA call      0x18132e900
000000018039CDAF mov       rcx, qword ptr [r14 + 0x50]
000000018039CDB3 movd      xmm6, esi
000000018039CDB7 cvtdq2ps  xmm6, xmm6
000000018039CDBA mulss     xmm6, xmm10
000000018039CDBF subss     xmm6, xmm7
000000018039CDC3 test      rcx, rcx
000000018039CDC6 je        0x18039ce5e
000000018039CDCC mov       r8, qword ptr [rip + 0x180f2b5] # 0x181bac088
000000018039CDD3 mov       edx, ebx
000000018039CDD5 call      0x180825710
000000018039CDDA test      rax, rax
000000018039CDDD je        0x18039ce5e
000000018039CDDF xor       edx, edx
000000018039CDE1 mov       rcx, rax
000000018039CDE4 call      0x18131c070
000000018039CDE9 test      rax, rax
000000018039CDEC je        0x18039ce5e
000000018039CDEE movaps    xmm0, xmm6
000000018039CDF1 movss     dword ptr [rsp + 0x28], xmm9
000000018039CDF8 unpcklps  xmm0, xmm8
000000018039CDFC lea       rdx, [rsp + 0x20]
000000018039CE01 xor       r8d, r8d
000000018039CE04 movsd     qword ptr [rsp + 0x20], xmm0
000000018039CE0A mov       rcx, rax
000000018039CE0D call      0x18132f910
000000018039CE12 inc       esi
000000018039CE14 sub       ebx, 1
000000018039CE17 jns       0x18039cd50
000000018039CE1D movaps    xmm9, xmmword ptr [rsp + 0x40]
000000018039CE23 lea       r11, [rsp + 0x80]
000000018039CE2B mov       rsi, qword ptr [r11 + 0x20]
000000018039CE2F mov       rdi, qword ptr [r11 + 0x28]
000000018039CE33 movaps    xmm10, xmmword ptr [r11 - 0x50]
000000018039CE38 movaps    xmm8, xmmword ptr [rsp + 0x50]
000000018039CE3E movaps    xmm7, xmmword ptr [rsp + 0x60]
000000018039CE43 movaps    xmm6, xmmword ptr [rsp + 0x70]
000000018039CE48 mov       rbp, qword ptr [rsp + 0x98]
000000018039CE50 mov       rbx, qword ptr [rsp + 0x90]
000000018039CE58 mov       rsp, r11
000000018039CE5B pop       r14
000000018039CE5D ret       
000000018039CE5E call      0x180250150
000000018039CE63 int3      
000000018039CE64 call      0x180250140
000000018039CE69 int3      
000000018039CE6A int3      
000000018039CE6B int3      
000000018039CE6C int3      
000000018039CE6D int3      
000000018039CE6E int3      
000000018039CE6F int3      