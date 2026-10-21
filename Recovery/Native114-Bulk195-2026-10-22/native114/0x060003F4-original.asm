000000018037B6C0 mov       qword ptr [rsp + 0x10], rbx
000000018037B6C5 push      rdi
000000018037B6C6 sub       rsp, 0x60
000000018037B6CA cmp       byte ptr [rip + 0x1969702], 0 # 0x181ce4dd3
000000018037B6D1 mov       rbx, rcx
000000018037B6D4 jne       0x18037b725
000000018037B6D6 lea       rcx, [rip + 0x182887b] # 0x181ba3f58
000000018037B6DD call      0x18024fef0
000000018037B6E2 lea       rcx, [rip + 0x182692f] # 0x181ba2018
000000018037B6E9 call      0x18024fef0
000000018037B6EE lea       rcx, [rip + 0x1828b83] # 0x181ba4278
000000018037B6F5 call      0x18024fef0
000000018037B6FA lea       rcx, [rip + 0x1834dff] # 0x181bb0500
000000018037B701 call      0x18024fef0
000000018037B706 lea       rcx, [rip + 0x183ad6b] # 0x181bb6478
000000018037B70D call      0x18024fef0
000000018037B712 lea       rcx, [rip + 0x182dfe7] # 0x181ba9700
000000018037B719 call      0x18024fef0
000000018037B71E mov       byte ptr [rip + 0x19696ae], 1 # 0x181ce4dd3
000000018037B725 mov       rcx, qword ptr [rip + 0x1834dd4] # 0x181bb0500
000000018037B72C mov       rdi, qword ptr [rbx + 0x100]
000000018037B733 cmp       dword ptr [rcx + 0xe0], 0
000000018037B73A jne       0x18037b741
000000018037B73C call      0x1802501e0
000000018037B741 xor       r8d, r8d
000000018037B744 xor       edx, edx
000000018037B746 mov       rcx, rdi
000000018037B749 call      0x18131f760
000000018037B74E test      al, al
000000018037B750 jne       0x18037ba56
000000018037B756 mov       qword ptr [rsp + 0x70], rsi
000000018037B75B mov       rcx, qword ptr [rbx + 0x100]
000000018037B762 test      rcx, rcx
000000018037B765 je        0x18037ba61
000000018037B76B mov       r9d, 1
000000018037B771 mov       qword ptr [rsp + 0x20], 0
000000018037B77A mov       r8d, 0x15
000000018037B780 mov       rdx, rbx
000000018037B783 call      0x180350e40
000000018037B788 test      rax, rax
000000018037B78B je        0x18037ba61
000000018037B791 xor       edx, edx
000000018037B793 mov       rcx, rax
000000018037B796 call      0x180311890
000000018037B79B xor       edx, edx
000000018037B79D mov       rcx, rbx
000000018037B7A0 call      0x181304510
000000018037B7A5 test      rax, rax
000000018037B7A8 je        0x18037ba61
000000018037B7AE xor       r8d, r8d
000000018037B7B1 lea       rcx, [rsp + 0x30]
000000018037B7B6 mov       rdx, rax
000000018037B7B9 call      0x18132f3a0
000000018037B7BE cmp       byte ptr [rip + 0x196936f], 0 # 0x181ce4b34
000000018037B7C5 movsd     xmm0, qword ptr [rax]
000000018037B7C9 mov       esi, dword ptr [rax + 8]
000000018037B7CC movaps    xmm1, xmm0
000000018037B7CF shufps    xmm1, xmm1, 0x55
000000018037B7D3 addss     xmm1, dword ptr [rip + 0x122c32d] # 0x1815a7b08
000000018037B7DB movsd     qword ptr [rsp + 0x30], xmm0
000000018037B7E1 movss     dword ptr [rsp + 0x34], xmm1
000000018037B7E7 jne       0x18037b7fc
000000018037B7E9 lea       rcx, [rip + 0x18360a0] # 0x181bb1890
000000018037B7F0 call      0x18024fef0
000000018037B7F5 mov       byte ptr [rip + 0x1969338], 1 # 0x181ce4b34
000000018037B7FC mov       rax, qword ptr [rip + 0x183608d] # 0x181bb1890
000000018037B803 mov       rcx, qword ptr [rax + 0xb8]
000000018037B80A mov       rcx, qword ptr [rcx]
000000018037B80D test      rcx, rcx
000000018037B810 je        0x18037ba61
000000018037B816 xor       r8d, r8d
000000018037B819 mov       edx, 0x12
000000018037B81E call      0x180321f30
000000018037B823 mov       rcx, qword ptr [rip + 0x1834cd6] # 0x181bb0500
000000018037B82A mov       rdi, rax
000000018037B82D cmp       dword ptr [rcx + 0xe0], 0
000000018037B834 jne       0x18037b83b
000000018037B836 call      0x1802501e0
000000018037B83B xor       r8d, r8d
000000018037B83E xor       edx, edx
000000018037B840 mov       rcx, rdi
000000018037B843 call      0x18131f900
000000018037B848 test      al, al
000000018037B84A je        0x18037b8c5
000000018037B84C test      rdi, rdi
000000018037B84F je        0x18037ba61
000000018037B855 xor       edx, edx
000000018037B857 mov       rcx, rdi
000000018037B85A call      0x180cfb610
000000018037B85F test      rax, rax
000000018037B862 je        0x18037ba61
000000018037B868 xor       edx, edx
000000018037B86A mov       rcx, rax
000000018037B86D call      0x18131c070
000000018037B872 test      rax, rax
000000018037B875 je        0x18037ba61
000000018037B87B movsd     xmm0, qword ptr [rsp + 0x30]
000000018037B881 lea       rdx, [rsp + 0x30]
000000018037B886 xor       r8d, r8d
000000018037B889 movsd     qword ptr [rsp + 0x30], xmm0
000000018037B88F mov       rcx, rax
000000018037B892 mov       dword ptr [rsp + 0x38], esi
000000018037B896 call      0x18132fbb0
000000018037B89B mov       rdx, qword ptr [rip + 0x18286b6] # 0x181ba3f58
000000018037B8A2 mov       rcx, rdi
000000018037B8A5 call      0x18046d280
000000018037B8AA test      rax, rax
000000018037B8AD je        0x18037ba61
000000018037B8B3 mov       rdx, qword ptr [rip + 0x182de46] # 0x181ba9700
000000018037B8BA xor       r8d, r8d
000000018037B8BD mov       rcx, rax
000000018037B8C0 call      0x181348060
000000018037B8C5 mov       rax, qword ptr [rip + 0x183abac] # 0x181bb6478
000000018037B8CC cmp       dword ptr [rax + 0xe0], 0
000000018037B8D3 jne       0x18037b8e4
000000018037B8D5 mov       rcx, rax
000000018037B8D8 call      0x1802501e0
000000018037B8DD mov       rax, qword ptr [rip + 0x183ab94] # 0x181bb6478
000000018037B8E4 mov       rax, qword ptr [rax + 0xb8]
000000018037B8EB mov       rcx, qword ptr [rax + 0x130]
000000018037B8F2 test      rcx, rcx
000000018037B8F5 je        0x18037ba61
000000018037B8FB mov       r8, qword ptr [rip + 0x1828976] # 0x181ba4278
000000018037B902 mov       edx, 0x1b
000000018037B907 call      0x180825710
000000018037B90C xor       ecx, ecx
000000018037B90E mov       rdi, rax
000000018037B911 call      0x1812e27c0
000000018037B916 test      rax, rax
000000018037B919 je        0x18037ba61
000000018037B91F xor       edx, edx
000000018037B921 mov       rcx, rax
000000018037B924 call      0x181304510
000000018037B929 test      rax, rax
000000018037B92C je        0x18037ba61
000000018037B932 xor       r8d, r8d
000000018037B935 movaps    xmmword ptr [rsp + 0x50], xmm6
000000018037B93A mov       rdx, rax
000000018037B93D lea       rcx, [rsp + 0x30]
000000018037B942 call      0x18132f3a0
000000018037B947 mov       rcx, qword ptr [rip + 0x18266ca] # 0x181ba2018
000000018037B94E movsd     xmm6, qword ptr [rax]
000000018037B952 cmp       dword ptr [rcx + 0xe0], 0
000000018037B959 mov       esi, dword ptr [rax + 8]
000000018037B95C jne       0x18037b963
000000018037B95E call      0x1802501e0
000000018037B963 xor       ecx, ecx
000000018037B965 movsd     qword ptr [rsp + 0x30], xmm6
000000018037B96B mov       dword ptr [rsp + 0x38], esi
000000018037B96F call      0x18031aea0
000000018037B974 mulss     xmm0, dword ptr [rip + 0x122c298] # 0x1815a7c14
000000018037B97C lea       rdx, [rsp + 0x30]
000000018037B981 xor       r9d, r9d
000000018037B984 mov       rcx, rdi
000000018037B987 movaps    xmm2, xmm0
000000018037B98A call      0x18031b230
000000018037B98F mov       rax, qword ptr [rip + 0x183aae2] # 0x181bb6478
000000018037B996 xor       r8d, r8d
000000018037B999 mov       edx, 2
000000018037B99E mov       rcx, qword ptr [rax + 0xb8]
000000018037B9A5 mov       rdi, qword ptr [rcx + 0x130]
000000018037B9AC xor       ecx, ecx
000000018037B9AE call      0x18130d310
000000018037B9B3 movaps    xmm6, xmmword ptr [rsp + 0x50]
000000018037B9B8 test      rdi, rdi
000000018037B9BB je        0x18037ba61
000000018037B9C1 mov       r8, qword ptr [rip + 0x18288b0] # 0x181ba4278
000000018037B9C8 lea       edx, [rax + 0x11]
000000018037B9CB mov       rcx, rdi
000000018037B9CE call      0x180825710
000000018037B9D3 xor       ecx, ecx
000000018037B9D5 mov       rdi, rax
000000018037B9D8 call      0x1812e27c0
000000018037B9DD test      rax, rax
000000018037B9E0 je        0x18037ba61
000000018037B9E2 xor       edx, edx
000000018037B9E4 mov       rcx, rax
000000018037B9E7 call      0x181304510
000000018037B9EC test      rax, rax
000000018037B9EF je        0x18037ba61
000000018037B9F1 xor       r8d, r8d
000000018037B9F4 lea       rcx, [rsp + 0x40]
000000018037B9F9 mov       rdx, rax
000000018037B9FC call      0x18132f3a0
000000018037BA01 xor       ecx, ecx
000000018037BA03 movsd     xmm0, qword ptr [rax]
000000018037BA07 mov       eax, dword ptr [rax + 8]
000000018037BA0A movsd     qword ptr [rsp + 0x30], xmm0
000000018037BA10 mov       dword ptr [rsp + 0x38], eax
000000018037BA14 call      0x18031aea0
000000018037BA19 mulss     xmm0, dword ptr [rip + 0x122c3cb] # 0x1815a7dec
000000018037BA21 lea       rdx, [rsp + 0x30]
000000018037BA26 xor       r9d, r9d
000000018037BA29 mov       rcx, rdi
000000018037BA2C movaps    xmm2, xmm0
000000018037BA2F call      0x18031b230
000000018037BA34 xor       edx, edx
000000018037BA36 mov       rcx, rbx
000000018037BA39 call      0x18037b200
000000018037BA3E mov       rcx, qword ptr [rbx + 0x100]
000000018037BA45 test      rcx, rcx
000000018037BA48 je        0x18037ba61
000000018037BA4A xor       edx, edx
000000018037BA4C call      0x180354440
000000018037BA51 mov       rsi, qword ptr [rsp + 0x70]
000000018037BA56 mov       rbx, qword ptr [rsp + 0x78]
000000018037BA5B add       rsp, 0x60
000000018037BA5F pop       rdi
000000018037BA60 ret       
000000018037BA61 call      0x180250150
000000018037BA66 int3      
000000018037BA67 int3      
000000018037BA68 int3      
000000018037BA69 int3      
000000018037BA6A int3      
000000018037BA6B int3      
000000018037BA6C int3      
000000018037BA6D int3      
000000018037BA6E int3      
000000018037BA6F int3      