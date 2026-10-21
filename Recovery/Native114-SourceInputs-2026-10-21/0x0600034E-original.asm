000000018035B4B0 push      rbx
000000018035B4B2 sub       rsp, 0x90
000000018035B4B9 cmp       byte ptr [rip + 0x1989830], 0 # 0x181ce4cf0
000000018035B4C0 mov       rbx, rcx
000000018035B4C3 movaps    xmmword ptr [rsp + 0x60], xmm8
000000018035B4C9 jne       0x18035b4de
000000018035B4CB lea       rcx, [rip + 0x1846b46] # 0x181ba2018
000000018035B4D2 call      0x18024fef0
000000018035B4D7 mov       byte ptr [rip + 0x1989812], 1 # 0x181ce4cf0
000000018035B4DE movss     xmm0, dword ptr [rbx + 0xc4]
000000018035B4E6 xorps     xmm8, xmm8
000000018035B4EA comiss    xmm0, xmm8
000000018035B4EE jbe       0x18035b5c5
000000018035B4F4 cmp       byte ptr [rbx + 0xb5], 0
000000018035B4FB je        0x18035b5c5
000000018035B501 mov       rax, qword ptr [rbx + 0x1e8]
000000018035B508 mov       qword ptr [rsp + 0xa0], rdi
000000018035B510 movaps    xmmword ptr [rsp + 0x80], xmm6
000000018035B518 movaps    xmmword ptr [rsp + 0x70], xmm7
000000018035B51D movaps    xmmword ptr [rsp + 0x50], xmm9
000000018035B523 test      rax, rax
000000018035B526 je        0x18035b80b
000000018035B52C cmp       byte ptr [rax + 0x40], 0
000000018035B530 je        0x18035b53f
000000018035B532 mov       eax, dword ptr [rbx + 0x7c]
000000018035B535 cmp       eax, 1
000000018035B538 je        0x18035b5aa
000000018035B53A cmp       eax, 9
000000018035B53D je        0x18035b5aa
000000018035B53F movss     xmm0, dword ptr [rbx + 0xc8]
000000018035B547 comiss    xmm0, xmm8
000000018035B54B jbe       0x18035b5aa
000000018035B54D mov       eax, dword ptr [rbx + 0x7c]
000000018035B550 movaps    xmm9, xmm0
000000018035B554 cmp       eax, 1
000000018035B557 je        0x18035b5df
000000018035B55D cmp       eax, 9
000000018035B560 je        0x18035b5df
000000018035B562 xor       ecx, ecx
000000018035B564 movaps    xmm7, xmm9
000000018035B568 call      0x18132c2f0
000000018035B56D xor       edx, edx
000000018035B56F mov       rcx, rbx
000000018035B572 movaps    xmm6, xmm0
000000018035B575 call      0x180350260
000000018035B57A mulss     xmm0, xmm6
000000018035B57E subss     xmm7, xmm0
000000018035B582 comiss    xmm8, xmm7
000000018035B586 movss     dword ptr [rbx + 0xc8], xmm7
000000018035B58E jb        0x18035b5aa
000000018035B590 xor       edx, edx
000000018035B592 mov       rcx, rbx
000000018035B595 call      0x180355880
000000018035B59A test      al, al
000000018035B59C je        0x18035b5d4
000000018035B59E mov       eax, dword ptr [rbx + 0xc4]
000000018035B5A4 mov       dword ptr [rbx + 0xc8], eax
000000018035B5AA movaps    xmm7, xmmword ptr [rsp + 0x70]
000000018035B5AF movaps    xmm6, xmmword ptr [rsp + 0x80]
000000018035B5B7 mov       rdi, qword ptr [rsp + 0xa0]
000000018035B5BF movaps    xmm9, xmmword ptr [rsp + 0x50]
000000018035B5C5 movaps    xmm8, xmmword ptr [rsp + 0x60]
000000018035B5CB add       rsp, 0x90
000000018035B5D2 pop       rbx
000000018035B5D3 ret       
000000018035B5D4 movss     dword ptr [rbx + 0xc8], xmm9
000000018035B5DD jmp       0x18035b5aa
000000018035B5DF mov       rax, qword ptr [rbx + 0x1e8]
000000018035B5E6 test      rax, rax
000000018035B5E9 je        0x18035b80b
000000018035B5EF cmp       byte ptr [rax + 0x40], 0
000000018035B5F3 jne       0x18035b5aa
000000018035B5F5 xor       ecx, ecx
000000018035B5F7 movaps    xmm7, xmm9
000000018035B5FB call      0x18132c2f0
000000018035B600 xor       edx, edx
000000018035B602 mov       rcx, rbx
000000018035B605 movaps    xmm6, xmm0
000000018035B608 call      0x180350260
000000018035B60D mulss     xmm0, xmm6
000000018035B611 subss     xmm7, xmm0
000000018035B615 movss     xmm0, dword ptr [rip + 0x124c3f3] # 0x1815a7a10
000000018035B61D comiss    xmm0, xmm7
000000018035B620 movss     dword ptr [rbx + 0xc8], xmm7
000000018035B628 jb        0x18035b645
000000018035B62A movss     xmm0, dword ptr [rip + 0x124c3f2] # 0x1815a7a24
000000018035B632 movaps    xmm1, xmm7
000000018035B635 addss     xmm1, xmm7
000000018035B639 subss     xmm0, xmm1
000000018035B63D movss     dword ptr [rbx + 0x1f8], xmm0
000000018035B645 comiss    xmm8, xmm7
000000018035B649 jb        0x18035b5aa
000000018035B64F cmp       byte ptr [rip + 0x19896ce], 0 # 0x181ce4d24
000000018035B656 jne       0x18035b683
000000018035B658 lea       rcx, [rip + 0x18469b9] # 0x181ba2018
000000018035B65F call      0x18024fef0
000000018035B664 lea       rcx, [rip + 0x18600ad] # 0x181bbb718
000000018035B66B call      0x18024fef0
000000018035B670 lea       rcx, [rip + 0x185a7c9] # 0x181bb5e40
000000018035B677 call      0x18024fef0
000000018035B67C mov       byte ptr [rip + 0x19896a1], 1 # 0x181ce4d24
000000018035B683 cmp       dword ptr [rbx + 0x7c], 1
000000018035B687 jne       0x18035b75a
000000018035B68D mov       rcx, qword ptr [rip + 0x1846984] # 0x181ba2018
000000018035B694 mov       rdi, qword ptr [rbx + 0x148]
000000018035B69B cmp       dword ptr [rcx + 0xe0], 0
000000018035B6A2 jne       0x18035b6a9
000000018035B6A4 call      0x1802501e0
000000018035B6A9 mov       r8, qword ptr [rip + 0x185a790] # 0x181bb5e40
000000018035B6B0 lea       rcx, [rsp + 0x40]
000000018035B6B5 xor       r9d, r9d
000000018035B6B8 mov       rdx, rdi
000000018035B6BB call      0x18031b680
000000018035B6C0 mov       rcx, qword ptr [rbx + 0x1e8]
000000018035B6C7 test      rcx, rcx
000000018035B6CA je        0x18035b80b
000000018035B6D0 mov       rcx, qword ptr [rcx + 0x100]
000000018035B6D7 test      rcx, rcx
000000018035B6DA je        0x18035b80b
000000018035B6E0 movsd     xmm0, qword ptr [rax]
000000018035B6E4 lea       r9, [rsp + 0x30]
000000018035B6E9 mov       eax, dword ptr [rax + 8]
000000018035B6EC xor       r8d, r8d
000000018035B6EF mov       edx, 1
000000018035B6F4 movsd     qword ptr [rsp + 0x30], xmm0
000000018035B6FA mov       dword ptr [rsp + 0x38], eax
000000018035B6FE mov       qword ptr [rsp + 0x20], 0
000000018035B707 call      0x180323050
000000018035B70C mov       rdi, rax
000000018035B70F test      rax, rax
000000018035B712 je        0x18035b80b
000000018035B718 cvttss2si edx, dword ptr [rbx + 0x9c]
000000018035B720 xor       r8d, r8d
000000018035B723 mov       rcx, rax
000000018035B726 call      0x180378750
000000018035B72B mov       r8, qword ptr [rip + 0x185ffe6] # 0x181bbb718
000000018035B732 mov       rdx, rbx
000000018035B735 mov       rcx, rdi
000000018035B738 call      0x1804a1aa0
000000018035B73D xor       edx, edx
000000018035B73F mov       rcx, rdi
000000018035B742 call      0x1803782b0
000000018035B747 cmp       dword ptr [rbx + 0x88], 1
000000018035B74E jl        0x18035b75a
000000018035B750 xor       edx, edx
000000018035B752 mov       rcx, rbx
000000018035B755 call      0x180355690
000000018035B75A mov       rax, qword ptr [rbx + 0x1d8]
000000018035B761 test      rax, rax
000000018035B764 je        0x18035b80b
000000018035B76A cmp       dword ptr [rax + 0x18], 0
000000018035B76E jbe       0x18035b811
000000018035B774 mov       rcx, qword ptr [rip + 0x184689d] # 0x181ba2018
000000018035B77B mov       rdi, qword ptr [rax + 0x20]
000000018035B77F cmp       dword ptr [rcx + 0xe0], 0
000000018035B786 jne       0x18035b78d
000000018035B788 call      0x1802501e0
000000018035B78D xor       ecx, ecx
000000018035B78F call      0x18031aea0
000000018035B794 test      rdi, rdi
000000018035B797 je        0x18035b80b
000000018035B799 xor       r8d, r8d
000000018035B79C movaps    xmm1, xmm0
000000018035B79F mov       rcx, rdi
000000018035B7A2 call      0x1812dc4b0
000000018035B7A7 mov       rax, qword ptr [rbx + 0x1d8]
000000018035B7AE test      rax, rax
000000018035B7B1 je        0x18035b80b
000000018035B7B3 cmp       dword ptr [rax + 0x18], 0
000000018035B7B7 jbe       0x18035b811
000000018035B7B9 mov       rcx, qword ptr [rax + 0x20]
000000018035B7BD test      rcx, rcx
000000018035B7C0 je        0x18035b80b
000000018035B7C2 xor       edx, edx
000000018035B7C4 call      0x1812dc300
000000018035B7C9 cmp       byte ptr [rbx + 0xe8], 0
000000018035B7D0 mov       eax, dword ptr [rbx + 0xc4]
000000018035B7D6 mov       dword ptr [rbx + 0xc8], eax
000000018035B7DC je        0x18035b5aa
000000018035B7E2 mov       rax, qword ptr [rbx + 0xe0]
000000018035B7E9 test      rax, rax
000000018035B7EC je        0x18035b80b
000000018035B7EE cmp       dword ptr [rax + 0x18], 1
000000018035B7F2 jne       0x18035b5aa
000000018035B7F8 xor       r8d, r8d
000000018035B7FB mov       rdx, rbx
000000018035B7FE mov       rcx, rax
000000018035B801 call      0x180386550
000000018035B806 jmp       0x18035b5aa
000000018035B80B call      0x180250150
000000018035B810 int3      
000000018035B811 call      0x180250140
000000018035B816 int3      
000000018035B817 int3      
000000018035B818 int3      
000000018035B819 int3      
000000018035B81A int3      
000000018035B81B int3      
000000018035B81C int3      
000000018035B81D int3      
000000018035B81E int3      
000000018035B81F int3      