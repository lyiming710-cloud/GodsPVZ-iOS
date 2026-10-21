000000018034E540 mov       qword ptr [rsp + 0x10], rbx
000000018034E545 mov       qword ptr [rsp + 0x18], rsi
000000018034E54A mov       qword ptr [rsp + 8], rcx
000000018034E54F push      rdi
000000018034E550 sub       rsp, 0x60
000000018034E554 mov       rdi, rcx
000000018034E557 cmp       byte ptr [rip + 0x199679a], 0 # 0x181ce4cf8
000000018034E55E jne       0x18034e5af
000000018034E560 lea       rcx, [rip + 0x1874ef9] # 0x181bc3460
000000018034E567 call      0x18024fef0
000000018034E56C lea       rcx, [rip + 0x1874f45] # 0x181bc34b8
000000018034E573 call      0x18024fef0
000000018034E578 lea       rcx, [rip + 0x1874f91] # 0x181bc3510
000000018034E57F call      0x18024fef0
000000018034E584 lea       rcx, [rip + 0x185d8ed] # 0x181babe78
000000018034E58B call      0x18024fef0
000000018034E590 lea       rcx, [rip + 0x1865dd1] # 0x181bb4368
000000018034E597 call      0x18024fef0
000000018034E59C lea       rcx, [rip + 0x1861f5d] # 0x181bb0500
000000018034E5A3 call      0x18024fef0
000000018034E5A8 mov       byte ptr [rip + 0x1996749], 1 # 0x181ce4cf8
000000018034E5AF mov       rbx, qword ptr [rdi + 0x1a0]
000000018034E5B6 mov       rcx, qword ptr [rip + 0x1861f43] # 0x181bb0500
000000018034E5BD cmp       dword ptr [rcx + 0xe0], 0
000000018034E5C4 jne       0x18034e5cb
000000018034E5C6 call      0x1802501e0
000000018034E5CB xor       edx, edx
000000018034E5CD mov       rcx, rbx
000000018034E5D0 call      0x18131f870
000000018034E5D5 test      al, al
000000018034E5D7 je        0x18034e5ff
000000018034E5D9 mov       rbx, qword ptr [rdi + 0x1a0]
000000018034E5E0 mov       rcx, qword ptr [rip + 0x1861f19] # 0x181bb0500
000000018034E5E7 cmp       dword ptr [rcx + 0xe0], 0
000000018034E5EE jne       0x18034e5f5
000000018034E5F0 call      0x1802501e0
000000018034E5F5 xor       edx, edx
000000018034E5F7 mov       rcx, rbx
000000018034E5FA call      0x18131e940
000000018034E5FF mov       rbx, qword ptr [rdi + 0x1b0]
000000018034E606 mov       rcx, qword ptr [rip + 0x1861ef3] # 0x181bb0500
000000018034E60D cmp       dword ptr [rcx + 0xe0], 0
000000018034E614 jne       0x18034e61b
000000018034E616 call      0x1802501e0
000000018034E61B xor       edx, edx
000000018034E61D mov       rcx, rbx
000000018034E620 call      0x18131f870
000000018034E625 test      al, al
000000018034E627 je        0x18034e64f
000000018034E629 mov       rbx, qword ptr [rdi + 0x1b0]
000000018034E630 mov       rcx, qword ptr [rip + 0x1861ec9] # 0x181bb0500
000000018034E637 cmp       dword ptr [rcx + 0xe0], 0
000000018034E63E jne       0x18034e645
000000018034E640 call      0x1802501e0
000000018034E645 xor       edx, edx
000000018034E647 mov       rcx, rbx
000000018034E64A call      0x18131e940
000000018034E64F mov       rbx, qword ptr [rdi + 0x1c0]
000000018034E656 mov       rcx, qword ptr [rip + 0x1861ea3] # 0x181bb0500
000000018034E65D cmp       dword ptr [rcx + 0xe0], 0
000000018034E664 jne       0x18034e66b
000000018034E666 call      0x1802501e0
000000018034E66B xor       edx, edx
000000018034E66D mov       rcx, rbx
000000018034E670 call      0x18131f870
000000018034E675 test      al, al
000000018034E677 je        0x18034e69f
000000018034E679 mov       rbx, qword ptr [rdi + 0x1c0]
000000018034E680 mov       rcx, qword ptr [rip + 0x1861e79] # 0x181bb0500
000000018034E687 cmp       dword ptr [rcx + 0xe0], 0
000000018034E68E jne       0x18034e695
000000018034E690 call      0x1802501e0
000000018034E695 xor       edx, edx
000000018034E697 mov       rcx, rbx
000000018034E69A call      0x18131e940
000000018034E69F mov       rdx, qword ptr [rdi + 0x1c8]
000000018034E6A6 test      rdx, rdx
000000018034E6A9 je        0x18034e7e1
000000018034E6AF mov       r8, qword ptr [rip + 0x185d7c2] # 0x181babe78
000000018034E6B6 lea       rcx, [rsp + 0x28]
000000018034E6BB call      0x1808197f0
000000018034E6C0 movups    xmm0, xmmword ptr [rsp + 0x28]
000000018034E6C5 movups    xmmword ptr [rsp + 0x40], xmm0
000000018034E6CA movsd     xmm1, qword ptr [rsp + 0x38]
000000018034E6D0 movsd     qword ptr [rsp + 0x50], xmm1
000000018034E6D6 mov       qword ptr [rsp + 0x28], 0
000000018034E6DF lea       rbx, [rsp + 0x40]
000000018034E6E4 mov       qword ptr [rsp + 0x30], rbx
000000018034E6E9 nop       dword ptr [rax]
000000018034E6F0 mov       rdx, qword ptr [rip + 0x1874dc1] # 0x181bc34b8
000000018034E6F7 lea       rcx, [rsp + 0x40]
000000018034E6FC call      0x180669280
000000018034E701 test      al, al
000000018034E703 je        0x18034e72b
000000018034E705 mov       rsi, qword ptr [rsp + 0x50]
000000018034E70A mov       rcx, qword ptr [rip + 0x1861def] # 0x181bb0500
000000018034E711 cmp       dword ptr [rcx + 0xe0], 0
000000018034E718 jne       0x18034e71f
000000018034E71A call      0x1802501e0
000000018034E71F xor       edx, edx
000000018034E721 mov       rcx, rsi
000000018034E724 call      0x18131e940
000000018034E729 jmp       0x18034e6f0
000000018034E72B mov       rdx, qword ptr [rip + 0x1874d2e] # 0x181bc3460
000000018034E732 mov       rcx, rbx
000000018034E735 call      0x180302170
000000018034E73A jmp       0x18034e760
000000018034E73C mov       rdx, qword ptr [rip + 0x1874d1d] # 0x181bc3460
000000018034E743 mov       rcx, qword ptr [rsp + 0x30]
000000018034E748 call      0x180302170
000000018034E74D mov       rcx, qword ptr [rsp + 0x28]
000000018034E752 test      rcx, rcx
000000018034E755 jne       0x18034e7e7
000000018034E75B mov       rdi, qword ptr [rsp + 0x70]
000000018034E760 xor       edx, edx
000000018034E762 mov       rcx, rdi
000000018034E765 call      0x1813044d0
000000018034E76A mov       rbx, rax
000000018034E76D mov       rcx, qword ptr [rip + 0x1861d8c] # 0x181bb0500
000000018034E774 cmp       dword ptr [rcx + 0xe0], 0
000000018034E77B jne       0x18034e782
000000018034E77D call      0x1802501e0
000000018034E782 xor       edx, edx
000000018034E784 mov       rcx, rbx
000000018034E787 call      0x18131e940
000000018034E78C xor       edx, edx
000000018034E78E mov       rcx, qword ptr [rdi + 0x1e8]
000000018034E795 call      0x18131f870
000000018034E79A test      al, al
000000018034E79C je        0x18034e7ce
000000018034E79E mov       rax, qword ptr [rdi + 0x1e8]
000000018034E7A5 test      rax, rax
000000018034E7A8 je        0x18034e7e1
000000018034E7AA mov       rcx, qword ptr [rax + 0xe8]
000000018034E7B1 test      rcx, rcx
000000018034E7B4 je        0x18034e7e1
000000018034E7B6 mov       rcx, qword ptr [rcx + 0x30]
000000018034E7BA test      rcx, rcx
000000018034E7BD je        0x18034e7e1
000000018034E7BF mov       r8, qword ptr [rip + 0x1865ba2] # 0x181bb4368
000000018034E7C6 mov       rdx, rdi
000000018034E7C9 call      0x18084e290
000000018034E7CE mov       rbx, qword ptr [rsp + 0x78]
000000018034E7D3 mov       rsi, qword ptr [rsp + 0x80]
000000018034E7DB add       rsp, 0x60
000000018034E7DF pop       rdi
000000018034E7E0 ret       
000000018034E7E1 call      0x180250150
000000018034E7E6 int3      
000000018034E7E7 call      0x180200e60
000000018034E7EC int3      
000000018034E7ED int3      
000000018034E7EE int3      
000000018034E7EF int3      