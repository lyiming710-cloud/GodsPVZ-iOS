00000001803A9C00 push      rbx
00000001803A9C02 sub       rsp, 0x30
00000001803A9C06 cmp       byte ptr [rip + 0x193b2f6], 0 # 0x181ce4f03
00000001803A9C0D mov       rbx, rcx
00000001803A9C10 jne       0x1803a9c3d
00000001803A9C12 lea       rcx, [rip + 0x180e99f] # 0x181bb85b8
00000001803A9C19 call      0x18024fef0
00000001803A9C1E lea       rcx, [rip + 0x17f6c4b] # 0x181ba0870
00000001803A9C25 call      0x18024fef0
00000001803A9C2A lea       rcx, [rip + 0x17fbef7] # 0x181ba5b28
00000001803A9C31 call      0x18024fef0
00000001803A9C36 mov       byte ptr [rip + 0x193b2c6], 1 # 0x181ce4f03
00000001803A9C3D cmp       dword ptr [rbx + 0x30], -1
00000001803A9C41 mov       qword ptr [rsp + 0x40], rdi
00000001803A9C46 je        0x1803a9dcb
00000001803A9C4C cmp       qword ptr [rbx + 0x20], 0
00000001803A9C51 je        0x1803a9d06
00000001803A9C57 mov       rax, qword ptr [rbx + 0x20]
00000001803A9C5B mov       rcx, qword ptr [rax + 0x18]
00000001803A9C5F test      rcx, rcx
00000001803A9C62 je        0x1803a9e5f
00000001803A9C68 mov       rcx, qword ptr [rcx + 0x18]
00000001803A9C6C test      rcx, rcx
00000001803A9C6F je        0x1803a9e5f
00000001803A9C75 mov       r8, qword ptr [rip + 0x180e93c] # 0x181bb85b8
00000001803A9C7C mov       edx, dword ptr [rbx + 0x30]
00000001803A9C7F call      0x180825710
00000001803A9C84 mov       rcx, qword ptr [rbx + 0x38]
00000001803A9C88 mov       rdi, rax
00000001803A9C8B test      rax, rax
00000001803A9C8E je        0x1803a9d5e
00000001803A9C94 test      rcx, rcx
00000001803A9C97 je        0x1803a9e5f
00000001803A9C9D mov       rdx, qword ptr [rax + 0x18]
00000001803A9CA1 xor       r9d, r9d
00000001803A9CA4 mov       r8b, 1
00000001803A9CA7 call      0x1811af070
00000001803A9CAC mov       rax, qword ptr [rbx + 0x20]
00000001803A9CB0 test      rax, rax
00000001803A9CB3 je        0x1803a9e5f
00000001803A9CB9 mov       rcx, qword ptr [rbx + 0x38]
00000001803A9CBD cmp       qword ptr [rax + 0x10], rdi
00000001803A9CC1 je        0x1803a9d11
00000001803A9CC3 test      rcx, rcx
00000001803A9CC6 je        0x1803a9e5f
00000001803A9CCC mov       rax, qword ptr [rcx]
00000001803A9CCF lea       rdx, [rsp + 0x20]
00000001803A9CD4 movdqa    xmm0, xmmword ptr [rip + 0x11fde94] # 0x1815a7b70
00000001803A9CDC movdqa    xmmword ptr [rsp + 0x20], xmm0
00000001803A9CE2 mov       r8, qword ptr [rax + 0x2b0]
00000001803A9CE9 call      qword ptr [rax + 0x2a8]
00000001803A9CEF mov       rcx, qword ptr [rbx + 0x40]
00000001803A9CF3 test      rcx, rcx
00000001803A9CF6 je        0x1803a9e5f
00000001803A9CFC xor       r8d, r8d
00000001803A9CFF mov       dl, 1
00000001803A9D01 call      0x18156cc70
00000001803A9D06 mov       rdi, qword ptr [rsp + 0x40]
00000001803A9D0B add       rsp, 0x30
00000001803A9D0F pop       rbx
00000001803A9D10 ret       
00000001803A9D11 test      rcx, rcx
00000001803A9D14 je        0x1803a9e5f
00000001803A9D1A mov       rax, qword ptr [rcx]
00000001803A9D1D lea       rdx, [rsp + 0x20]
00000001803A9D22 movdqa    xmm0, xmmword ptr [rip + 0x11fe1e6] # 0x1815a7f10
00000001803A9D2A movdqa    xmmword ptr [rsp + 0x20], xmm0
00000001803A9D30 mov       r8, qword ptr [rax + 0x2b0]
00000001803A9D37 call      qword ptr [rax + 0x2a8]
00000001803A9D3D mov       rcx, qword ptr [rbx + 0x40]
00000001803A9D41 test      rcx, rcx
00000001803A9D44 je        0x1803a9e5f
00000001803A9D4A xor       r8d, r8d
00000001803A9D4D xor       edx, edx
00000001803A9D4F mov       rdi, qword ptr [rsp + 0x40]
00000001803A9D54 add       rsp, 0x30
00000001803A9D58 pop       rbx
00000001803A9D59 jmp       0x18156cc70
00000001803A9D5E test      rcx, rcx
00000001803A9D61 je        0x1803a9e5f
00000001803A9D67 mov       rdx, qword ptr [rip + 0x17f6b02] # 0x181ba0870
00000001803A9D6E xor       r9d, r9d
00000001803A9D71 mov       r8b, 1
00000001803A9D74 call      0x1811af070
00000001803A9D79 mov       rcx, qword ptr [rbx + 0x40]
00000001803A9D7D test      rcx, rcx
00000001803A9D80 je        0x1803a9e5f
00000001803A9D86 xor       r8d, r8d
00000001803A9D89 xor       edx, edx
00000001803A9D8B call      0x18156cc70
00000001803A9D90 mov       rcx, qword ptr [rbx + 0x38]
00000001803A9D94 test      rcx, rcx
00000001803A9D97 je        0x1803a9e5f
00000001803A9D9D mov       rax, qword ptr [rcx]
00000001803A9DA0 lea       rdx, [rsp + 0x20]
00000001803A9DA5 movdqa    xmm0, xmmword ptr [rip + 0x11fddc3] # 0x1815a7b70
00000001803A9DAD movdqa    xmmword ptr [rsp + 0x20], xmm0
00000001803A9DB3 mov       r8, qword ptr [rax + 0x2b0]
00000001803A9DBA call      qword ptr [rax + 0x2a8]
00000001803A9DC0 mov       rdi, qword ptr [rsp + 0x40]
00000001803A9DC5 add       rsp, 0x30
00000001803A9DC9 pop       rbx
00000001803A9DCA ret       
00000001803A9DCB mov       rcx, qword ptr [rbx + 0x38]
00000001803A9DCF test      rcx, rcx
00000001803A9DD2 je        0x1803a9e5f
00000001803A9DD8 mov       rdx, qword ptr [rip + 0x17fbd49] # 0x181ba5b28
00000001803A9DDF xor       r9d, r9d
00000001803A9DE2 mov       r8b, 1
00000001803A9DE5 call      0x1811af070
00000001803A9DEA mov       rcx, qword ptr [rbx + 0x38]
00000001803A9DEE test      rcx, rcx
00000001803A9DF1 je        0x1803a9e5f
00000001803A9DF3 movss     xmm1, dword ptr [rip + 0x11fe17d] # 0x1815a7f78
00000001803A9DFB xor       r8d, r8d
00000001803A9DFE call      0x1811b9410
00000001803A9E03 mov       rcx, qword ptr [rbx + 0x40]
00000001803A9E07 test      rcx, rcx
00000001803A9E0A je        0x1803a9e5f
00000001803A9E0C xor       r8d, r8d
00000001803A9E0F mov       dl, 1
00000001803A9E11 call      0x18156cc70
00000001803A9E16 mov       rcx, qword ptr [rbx + 0x38]
00000001803A9E1A test      rcx, rcx
00000001803A9E1D je        0x1803a9e5f
00000001803A9E1F mov       rax, qword ptr [rcx]
00000001803A9E22 lea       rdx, [rsp + 0x20]
00000001803A9E27 movdqa    xmm0, xmmword ptr [rip + 0x11fdd41] # 0x1815a7b70
00000001803A9E2F movdqa    xmmword ptr [rsp + 0x20], xmm0
00000001803A9E35 mov       r8, qword ptr [rax + 0x2b0]
00000001803A9E3C call      qword ptr [rax + 0x2a8]
00000001803A9E42 mov       rcx, qword ptr [rbx + 0x48]
00000001803A9E46 test      rcx, rcx
00000001803A9E49 je        0x1803a9e5f
00000001803A9E4B xor       r8d, r8d
00000001803A9E4E xor       edx, edx
00000001803A9E50 mov       rdi, qword ptr [rsp + 0x40]
00000001803A9E55 add       rsp, 0x30
00000001803A9E59 pop       rbx
00000001803A9E5A jmp       0x18131bad0
00000001803A9E5F call      0x180250150
00000001803A9E64 int3      