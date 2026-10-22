00000001802FF490 push      rbp
00000001802FF492 push      rbx
00000001802FF493 push      r12
00000001802FF495 push      r13
00000001802FF497 push      r14
00000001802FF499 mov       rbp, rsp
00000001802FF49C sub       rsp, 0x80
00000001802FF4A3 cmp       byte ptr [rip + 0x19e5679], 0 # 0x181ce4b23
00000001802FF4AA mov       r13, r8
00000001802FF4AD mov       r14, rdx
00000001802FF4B0 mov       r12, rcx
00000001802FF4B3 jne       0x1802ff534
00000001802FF4B5 lea       rcx, [rip + 0x18ba6f4] # 0x181bb9bb0
00000001802FF4BC call      0x18024fef0
00000001802FF4C1 lea       rcx, [rip + 0x18c1ec0] # 0x181bc1388
00000001802FF4C8 call      0x18024fef0
00000001802FF4CD lea       rcx, [rip + 0x18ba684] # 0x181bb9b58
00000001802FF4D4 call      0x18024fef0
00000001802FF4D9 lea       rcx, [rip + 0x18c1df8] # 0x181bc12d8
00000001802FF4E0 call      0x18024fef0
00000001802FF4E5 lea       rcx, [rip + 0x18ba774] # 0x181bb9c60
00000001802FF4EC call      0x18024fef0
00000001802FF4F1 lea       rcx, [rip + 0x18c20a0] # 0x181bc1598
00000001802FF4F8 call      0x18024fef0
00000001802FF4FD lea       rcx, [rip + 0x18ba7b4] # 0x181bb9cb8
00000001802FF504 call      0x18024fef0
00000001802FF509 lea       rcx, [rip + 0x18c1058] # 0x181bc0568
00000001802FF510 call      0x18024fef0
00000001802FF515 lea       rcx, [rip + 0x18c24ec] # 0x181bc1a08
00000001802FF51C call      0x18024fef0
00000001802FF521 lea       rcx, [rip + 0x18b0fd8] # 0x181bb0500
00000001802FF528 call      0x18024fef0
00000001802FF52D mov       byte ptr [rip + 0x19e55ef], 1 # 0x181ce4b23
00000001802FF534 mov       rcx, qword ptr [rip + 0x18b0fc5] # 0x181bb0500
00000001802FF53B mov       rbx, qword ptr [r14 + 0x10]
00000001802FF53F cmp       dword ptr [rcx + 0xe0], 0
00000001802FF546 jne       0x1802ff54d
00000001802FF548 call      0x1802501e0
00000001802FF54D xor       r8d, r8d
00000001802FF550 xor       edx, edx
00000001802FF552 mov       rcx, rbx
00000001802FF555 call      0x18131f760
00000001802FF55A test      al, al
00000001802FF55C jne       0x1802ff9d6
00000001802FF562 mov       qword ptr [rsp + 0xb0], rsi
00000001802FF56A mov       qword ptr [rsp + 0xb8], rdi
00000001802FF572 mov       qword ptr [rsp + 0xc0], r15
00000001802FF57A movaps    xmmword ptr [rsp + 0x70], xmm6
00000001802FF57F movaps    xmmword ptr [rsp + 0x60], xmm7
00000001802FF584 movaps    xmmword ptr [rsp + 0x50], xmm8
00000001802FF58A movaps    xmmword ptr [rsp + 0x40], xmm9
00000001802FF590 mov       rdx, qword ptr [r14 + 0x10]
00000001802FF594 test      rdx, rdx
00000001802FF597 je        0x1802ff9ee
00000001802FF59D xor       r8d, r8d
00000001802FF5A0 lea       rcx, [rbp - 0x50]
00000001802FF5A4 call      0x18132f3a0
00000001802FF5A9 mov       rcx, qword ptr [rip + 0x18c0fb8] # 0x181bc0568
00000001802FF5B0 movsd     xmm0, qword ptr [rax]
00000001802FF5B4 mov       eax, dword ptr [rax + 8]
00000001802FF5B7 movsd     qword ptr [rbp - 0x60], xmm0
00000001802FF5BC mov       dword ptr [rbp - 0x58], eax
00000001802FF5BF call      0x180250100
00000001802FF5C4 mov       rdx, qword ptr [rip + 0x18ba58d] # 0x181bb9b58
00000001802FF5CB mov       rcx, rax
00000001802FF5CE mov       rbx, rax
00000001802FF5D1 call      0x1808053d0
00000001802FF5D6 mov       rcx, qword ptr [rip + 0x18c242b] # 0x181bc1a08
00000001802FF5DD call      0x180250100
00000001802FF5E2 mov       rdx, qword ptr [rip + 0x18c1cef] # 0x181bc12d8
00000001802FF5E9 mov       rcx, rax
00000001802FF5EC mov       rdi, rax
00000001802FF5EF call      0x1808053d0
00000001802FF5F4 mov       rax, qword ptr [r14 + 0x48]
00000001802FF5F8 xor       r15d, r15d
00000001802FF5FB mov       esi, r15d
00000001802FF5FE mov       ecx, r15d
00000001802FF601 test      rax, rax
00000001802FF604 je        0x1802ff9ee
00000001802FF60A movss     xmm6, dword ptr [rbp - 0x58]
00000001802FF60F movss     xmm7, dword ptr [rbp - 0x5c]
00000001802FF614 movss     xmm8, dword ptr [rbp - 0x60]
00000001802FF61A nop       word ptr [rax + rax]
00000001802FF620 cmp       ecx, dword ptr [rax + 0x18]
00000001802FF623 jge       0x1802ff6ed
00000001802FF629 mov       rdx, qword ptr [r14 + 0x50]
00000001802FF62D test      rdx, rdx
00000001802FF630 je        0x1802ff9ee
00000001802FF636 mov       r9, qword ptr [rip + 0x18c1f5b] # 0x181bc1598
00000001802FF63D lea       rcx, [rbp - 0x60]
00000001802FF641 mov       r8d, esi
00000001802FF644 call      0x1808257b0
00000001802FF649 movaps    xmm0, xmm8
00000001802FF64D movaps    xmm1, xmm7
00000001802FF650 addss     xmm0, dword ptr [rbp - 0x60]
00000001802FF655 addss     xmm1, dword ptr [rbp - 0x5c]
00000001802FF65A movaps    xmm2, xmm6
00000001802FF65D addss     xmm2, dword ptr [rbp - 0x58]
00000001802FF662 test      rdi, rdi
00000001802FF665 je        0x1802ff9ee
00000001802FF66B mov       r8, qword ptr [rip + 0x18c1d16] # 0x181bc1388
00000001802FF672 inc       dword ptr [rdi + 0x1c]
00000001802FF675 mov       rdx, qword ptr [rdi + 0x10]
00000001802FF679 movsxd    rcx, dword ptr [rdi + 0x18]
00000001802FF67D test      rdx, rdx
00000001802FF680 je        0x1802ff9ee
00000001802FF686 cmp       ecx, dword ptr [rdx + 0x18]
00000001802FF689 jb        0x1802ff6b5
00000001802FF68B mov       rax, qword ptr [r8 + 0x20]
00000001802FF68F lea       rdx, [rbp - 0x50]
00000001802FF693 unpcklps  xmm0, xmm1
00000001802FF696 mov       rcx, rdi
00000001802FF699 movss     dword ptr [rbp - 0x48], xmm2
00000001802FF69E movsd     qword ptr [rbp - 0x50], xmm0
00000001802FF6A3 mov       r8, qword ptr [rax + 0xc0]
00000001802FF6AA mov       r8, qword ptr [r8 + 0x70]
00000001802FF6AE call      0x1808164b0
00000001802FF6B3 jmp       0x1802ff6d7
00000001802FF6B5 lea       eax, [rcx + 1]
00000001802FF6B8 mov       dword ptr [rdi + 0x18], eax
00000001802FF6BB cmp       ecx, dword ptr [rdx + 0x18]
00000001802FF6BE jae       0x1802ff9f4
00000001802FF6C4 lea       rcx, [rcx + rcx*2]
00000001802FF6C8 unpcklps  xmm0, xmm1
00000001802FF6CB movsd     qword ptr [rdx + rcx*4 + 0x20], xmm0
00000001802FF6D1 movss     dword ptr [rdx + rcx*4 + 0x28], xmm2
00000001802FF6D7 mov       rax, qword ptr [r14 + 0x48]
00000001802FF6DB inc       esi
00000001802FF6DD mov       ecx, esi
00000001802FF6DF test      rax, rax
00000001802FF6E2 je        0x1802ff9ee
00000001802FF6E8 jmp       0x1802ff620
00000001802FF6ED mov       rax, qword ptr [r14 + 0x48]
00000001802FF6F1 mov       esi, r15d
00000001802FF6F4 mov       ecx, r15d
00000001802FF6F7 test      rax, rax
00000001802FF6FA je        0x1802ff9ee
00000001802FF700 cmp       ecx, dword ptr [rax + 0x18]
00000001802FF703 jge       0x1802ff860
00000001802FF709 test      rdi, rdi
00000001802FF70C je        0x1802ff9ee
00000001802FF712 mov       r9, qword ptr [rip + 0x18c1e7f] # 0x181bc1598
00000001802FF719 lea       rcx, [rbp - 0x60]
00000001802FF71D mov       r8d, esi
00000001802FF720 mov       rdx, rdi
00000001802FF723 call      0x1808257b0
00000001802FF728 cmp       byte ptr [rip + 0x19e5406], r15b # 0x181ce4b35
00000001802FF72F movsd     xmm0, qword ptr [r13]
00000001802FF735 movsd     qword ptr [rbp - 0x50], xmm0
00000001802FF73A jne       0x1802ff74f
00000001802FF73C lea       rcx, [rip + 0x18ad4b5] # 0x181bacbf8
00000001802FF743 call      0x18024fef0
00000001802FF748 mov       byte ptr [rip + 0x19e53e6], 1 # 0x181ce4b35
00000001802FF74F mov       rcx, qword ptr [rip + 0x18ad4a2] # 0x181bacbf8
00000001802FF756 movss     xmm7, dword ptr [rbp - 0x50]
00000001802FF75B movss     xmm6, dword ptr [rbp - 0x4c]
00000001802FF760 movss     xmm8, dword ptr [r13 + 8]
00000001802FF766 subss     xmm7, dword ptr [rbp - 0x60]
00000001802FF76B subss     xmm6, dword ptr [rbp - 0x5c]
00000001802FF770 subss     xmm8, dword ptr [rbp - 0x58]
00000001802FF776 cmp       dword ptr [rcx + 0xe0], r15d
00000001802FF77D jne       0x1802ff784
00000001802FF77F call      0x1802501e0
00000001802FF784 mulss     xmm6, xmm6
00000001802FF788 xorps     xmm0, xmm0
00000001802FF78B mulss     xmm7, xmm7
00000001802FF78F mulss     xmm8, xmm8
00000001802FF794 addss     xmm6, xmm7
00000001802FF798 addss     xmm6, xmm8
00000001802FF79D cvtps2pd  xmm1, xmm6
00000001802FF7A0 ucomisd   xmm0, xmm1
00000001802FF7A4 ja        0x1802ff7ac
00000001802FF7A6 sqrtpd    xmm0, xmm1
00000001802FF7AA jmp       0x1802ff7b4
00000001802FF7AC movaps    xmm0, xmm1
00000001802FF7AF call      0x1802cb7b4
00000001802FF7B4 xorps     xmm1, xmm1
00000001802FF7B7 cvtsd2ss  xmm1, xmm0
00000001802FF7BB test      rbx, rbx
00000001802FF7BE je        0x1802ff9ee
00000001802FF7C4 mov       r8, qword ptr [rip + 0x18ba3e5] # 0x181bb9bb0
00000001802FF7CB inc       dword ptr [rbx + 0x1c]
00000001802FF7CE mov       rdx, qword ptr [rbx + 0x10]
00000001802FF7D2 movsxd    rcx, dword ptr [rbx + 0x18]
00000001802FF7D6 test      rdx, rdx
00000001802FF7D9 je        0x1802ff9ee
00000001802FF7DF cmp       ecx, dword ptr [rdx + 0x18]
00000001802FF7E2 jb        0x1802ff7fd
00000001802FF7E4 mov       rax, qword ptr [r8 + 0x20]
00000001802FF7E8 mov       rcx, rbx
00000001802FF7EB mov       r8, qword ptr [rax + 0xc0]
00000001802FF7F2 mov       r8, qword ptr [r8 + 0x70]
00000001802FF7F6 call      0x18086ad70
00000001802FF7FB jmp       0x1802ff812
00000001802FF7FD lea       eax, [rcx + 1]
00000001802FF800 mov       dword ptr [rbx + 0x18], eax
00000001802FF803 cmp       ecx, dword ptr [rdx + 0x18]
00000001802FF806 jae       0x1802ff9f4
00000001802FF80C movss     dword ptr [rdx + rcx*4 + 0x20], xmm1
00000001802FF812 mov       r8, qword ptr [rip + 0x18ba49f] # 0x181bb9cb8
00000001802FF819 mov       edx, esi
00000001802FF81B mov       rcx, rbx
00000001802FF81E call      0x180878880
00000001802FF823 mov       rcx, qword ptr [r14 + 0x48]
00000001802FF827 movaps    xmm6, xmm0
00000001802FF82A test      rcx, rcx
00000001802FF82D je        0x1802ff9ee
00000001802FF833 mov       r8, qword ptr [rip + 0x18ba47e] # 0x181bb9cb8
00000001802FF83A mov       edx, esi
00000001802FF83C call      0x180878880
00000001802FF841 comiss    xmm0, xmm6
00000001802FF844 jae       0x1802ff9bf
00000001802FF84A mov       rax, qword ptr [r14 + 0x48]
00000001802FF84E inc       esi
00000001802FF850 mov       ecx, esi
00000001802FF852 test      rax, rax
00000001802FF855 jne       0x1802ff700
00000001802FF85B jmp       0x1802ff9ee
00000001802FF860 test      rbx, rbx
00000001802FF863 je        0x1802ff9ee
00000001802FF869 cmp       dword ptr [rbx + 0x18], r15d
00000001802FF86D je        0x1802ff9bf
00000001802FF873 mov       esi, 1
00000001802FF878 mov       eax, esi
00000001802FF87A nop       word ptr [rax + rax]
00000001802FF880 cmp       eax, dword ptr [rbx + 0x18]
00000001802FF883 jge       0x1802ff8b6
00000001802FF885 mov       r8, qword ptr [rip + 0x18ba42c] # 0x181bb9cb8
00000001802FF88C mov       edx, esi
00000001802FF88E mov       rcx, rbx
00000001802FF891 call      0x180878880
00000001802FF896 mov       r8, qword ptr [rip + 0x18ba41b] # 0x181bb9cb8
00000001802FF89D mov       edx, r15d
00000001802FF8A0 mov       rcx, rbx
00000001802FF8A3 movaps    xmm6, xmm0
00000001802FF8A6 call      0x180878880
00000001802FF8AB comiss    xmm0, xmm6
00000001802FF8AE cmova     r15d, esi
00000001802FF8B2 inc       esi
00000001802FF8B4 jmp       0x1802ff878
00000001802FF8B6 test      rdi, rdi
00000001802FF8B9 je        0x1802ff9ee
00000001802FF8BF mov       r9, qword ptr [rip + 0x18c1cd2] # 0x181bc1598
00000001802FF8C6 lea       rcx, [rbp - 0x60]
00000001802FF8CA mov       r8d, r15d
00000001802FF8CD mov       rdx, rdi
00000001802FF8D0 call      0x1808257b0
00000001802FF8D5 movsd     xmm8, qword ptr [r13]
00000001802FF8DB movss     xmm7, dword ptr [r13 + 4]
00000001802FF8E1 movss     xmm9, dword ptr [r13 + 8]
00000001802FF8E7 subss     xmm8, dword ptr [rbp - 0x60]
00000001802FF8ED subss     xmm7, dword ptr [rbp - 0x5c]
00000001802FF8F2 subss     xmm9, dword ptr [rbp - 0x58]
00000001802FF8F8 mov       r9, qword ptr [rip + 0x18c1c99] # 0x181bc1598
00000001802FF8FF lea       rcx, [rbp - 0x60]
00000001802FF903 mov       r8d, r15d
00000001802FF906 mov       rdx, rdi
00000001802FF909 call      0x1808257b0
00000001802FF90E mov       rcx, qword ptr [r14 + 0x48]
00000001802FF912 test      rcx, rcx
00000001802FF915 je        0x1802ff9ee
00000001802FF91B mov       r8, qword ptr [rip + 0x18ba396] # 0x181bb9cb8
00000001802FF922 mov       edx, r15d
00000001802FF925 call      0x180878880
00000001802FF92A mov       r8, qword ptr [rip + 0x18ba387] # 0x181bb9cb8
00000001802FF931 mov       edx, r15d
00000001802FF934 mov       rcx, rbx
00000001802FF937 movaps    xmm6, xmm0
00000001802FF93A call      0x180878880
00000001802FF93F divss     xmm6, xmm0
00000001802FF943 mulss     xmm8, xmm6
00000001802FF948 mulss     xmm7, xmm6
00000001802FF94C mulss     xmm9, xmm6
00000001802FF951 movss     xmm0, dword ptr [rbp - 0x60]
00000001802FF956 movss     xmm1, dword ptr [rbp - 0x5c]
00000001802FF95B addss     xmm0, xmm8
00000001802FF960 movss     xmm2, dword ptr [rbp - 0x58]
00000001802FF965 addss     xmm1, xmm7
00000001802FF969 addss     xmm2, xmm9
00000001802FF96E unpcklps  xmm0, xmm1
00000001802FF971 movsd     qword ptr [r12], xmm0
00000001802FF977 movss     dword ptr [r12 + 8], xmm2
00000001802FF97E movaps    xmm8, xmmword ptr [rsp + 0x50]
00000001802FF984 movaps    xmm7, xmmword ptr [rsp + 0x60]
00000001802FF989 movaps    xmm6, xmmword ptr [rsp + 0x70]
00000001802FF98E mov       r15, qword ptr [rsp + 0xc0]
00000001802FF996 mov       rdi, qword ptr [rsp + 0xb8]
00000001802FF99E mov       rsi, qword ptr [rsp + 0xb0]
00000001802FF9A6 movaps    xmm9, xmmword ptr [rsp + 0x40]
00000001802FF9AC mov       rax, r12
00000001802FF9AF add       rsp, 0x80
00000001802FF9B6 pop       r14
00000001802FF9B8 pop       r13
00000001802FF9BA pop       r12
00000001802FF9BC pop       rbx
00000001802FF9BD pop       rbp
00000001802FF9BE ret       
00000001802FF9BF movsd     xmm0, qword ptr [r13]
00000001802FF9C5 mov       eax, dword ptr [r13 + 8]
00000001802FF9C9 movsd     qword ptr [r12], xmm0
00000001802FF9CF mov       dword ptr [r12 + 8], eax
00000001802FF9D4 jmp       0x1802ff97e
00000001802FF9D6 xor       eax, eax
00000001802FF9D8 mov       qword ptr [rbp - 0x50], rax
00000001802FF9DC movsd     xmm0, qword ptr [rbp - 0x50]
00000001802FF9E1 movsd     qword ptr [r12], xmm0
00000001802FF9E7 mov       dword ptr [r12 + 8], eax
00000001802FF9EC jmp       0x1802ff9ac
00000001802FF9EE call      0x180250150
00000001802FF9F3 int3      
00000001802FF9F4 call      0x180250140
00000001802FF9F9 int3      
00000001802FF9FA int3      
00000001802FF9FB int3      
00000001802FF9FC int3      
00000001802FF9FD int3      
00000001802FF9FE int3      
00000001802FF9FF int3      