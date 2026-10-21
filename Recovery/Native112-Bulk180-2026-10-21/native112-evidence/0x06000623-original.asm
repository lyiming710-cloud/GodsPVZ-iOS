000000018039D860 push      rbx
000000018039D862 sub       rsp, 0x30
000000018039D866 cmp       byte ptr [rip + 0x194763e], 0 # 0x181ce4eab
000000018039D86D mov       rbx, rcx
000000018039D870 jne       0x18039d885
000000018039D872 lea       rcx, [rip + 0x1812c87] # 0x181bb0500
000000018039D879 call      0x18024fef0
000000018039D87E mov       byte ptr [rip + 0x1947626], 1 # 0x181ce4eab
000000018039D885 mov       rax, qword ptr [rbx + 0x20]
000000018039D889 test      rax, rax
000000018039D88C je        0x18039db1c
000000018039D892 mov       rcx, qword ptr [rip + 0x1812c67] # 0x181bb0500
000000018039D899 mov       qword ptr [rsp + 0x40], rdi
000000018039D89E mov       rdi, qword ptr [rax + 0x1a0]
000000018039D8A5 cmp       dword ptr [rcx + 0xe0], 0
000000018039D8AC jne       0x18039d8b3
000000018039D8AE call      0x1802501e0
000000018039D8B3 xor       r8d, r8d
000000018039D8B6 xor       edx, edx
000000018039D8B8 mov       rcx, rdi
000000018039D8BB call      0x18131f760
000000018039D8C0 mov       rdi, qword ptr [rsp + 0x40]
000000018039D8C5 test      al, al
000000018039D8C7 jne       0x18039d9c3
000000018039D8CD mov       rax, qword ptr [rbx + 0x20]
000000018039D8D1 test      rax, rax
000000018039D8D4 je        0x18039db1c
000000018039D8DA cmp       byte ptr [rax + 0xe8], 0
000000018039D8E1 jne       0x18039da56
000000018039D8E7 mov       rcx, qword ptr [rbx + 0x30]
000000018039D8EB test      rcx, rcx
000000018039D8EE je        0x18039db1c
000000018039D8F4 mov       rax, qword ptr [rcx]
000000018039D8F7 lea       rdx, [rsp + 0x20]
000000018039D8FC movdqa    xmm0, xmmword ptr [rip + 0x120a26c] # 0x1815a7b70
000000018039D904 movdqa    xmmword ptr [rsp + 0x20], xmm0
000000018039D90A mov       r8, qword ptr [rax + 0x2b0]
000000018039D911 call      qword ptr [rax + 0x2a8]
000000018039D917 mov       rax, qword ptr [rbx + 0x20]
000000018039D91B test      rax, rax
000000018039D91E je        0x18039db1c
000000018039D924 mov       rax, qword ptr [rax + 0xe0]
000000018039D92B test      rax, rax
000000018039D92E je        0x18039db1c
000000018039D934 cmp       dword ptr [rax + 0x48], 0
000000018039D938 mov       rcx, qword ptr [rbx + 0x40]
000000018039D93C jg        0x18039d95b
000000018039D93E test      rcx, rcx
000000018039D941 je        0x18039db1c
000000018039D947 xor       edx, edx
000000018039D949 call      0x1813044d0
000000018039D94E test      rax, rax
000000018039D951 je        0x18039db1c
000000018039D957 xor       dl, dl
000000018039D959 jmp       0x18039d976
000000018039D95B test      rcx, rcx
000000018039D95E je        0x18039db1c
000000018039D964 xor       edx, edx
000000018039D966 call      0x1813044d0
000000018039D96B test      rax, rax
000000018039D96E je        0x18039db1c
000000018039D974 mov       dl, 1
000000018039D976 xor       r8d, r8d
000000018039D979 mov       rcx, rax
000000018039D97C call      0x18131bad0
000000018039D981 mov       rax, qword ptr [rbx + 0x20]
000000018039D985 test      rax, rax
000000018039D988 je        0x18039db1c
000000018039D98E mov       rax, qword ptr [rax + 0xe0]
000000018039D995 test      rax, rax
000000018039D998 je        0x18039db1c
000000018039D99E mov       ecx, dword ptr [rax + 0x28]
000000018039D9A1 cmp       ecx, -1
000000018039D9A4 jne       0x18039d9c9
000000018039D9A6 mov       rcx, qword ptr [rbx + 0x30]
000000018039D9AA test      rcx, rcx
000000018039D9AD je        0x18039db1c
000000018039D9B3 xorps     xmm1, xmm1
000000018039D9B6 xor       r8d, r8d
000000018039D9B9 add       rsp, 0x30
000000018039D9BD pop       rbx
000000018039D9BE jmp       0x1813d7750
000000018039D9C3 add       rsp, 0x30
000000018039D9C7 pop       rbx
000000018039D9C8 ret       
000000018039D9C9 mov       rax, qword ptr [rbx + 0x20]
000000018039D9CD cmp       ecx, 3
000000018039D9D0 jne       0x18039da06
000000018039D9D2 test      rax, rax
000000018039D9D5 je        0x18039db1c
000000018039D9DB cmp       byte ptr [rax + 0xe8], 0
000000018039D9E2 mov       rcx, qword ptr [rbx + 0x30]
000000018039D9E6 je        0x18039d9aa
000000018039D9E8 test      rcx, rcx
000000018039D9EB je        0x18039db1c
000000018039D9F1 movss     xmm1, dword ptr [rip + 0x120a017] # 0x1815a7a10
000000018039D9F9 xor       r8d, r8d
000000018039D9FC add       rsp, 0x30
000000018039DA00 pop       rbx
000000018039DA01 jmp       0x1813d7750
000000018039DA06 test      rax, rax
000000018039DA09 je        0x18039db1c
000000018039DA0F mov       rcx, qword ptr [rax + 0xe0]
000000018039DA16 test      rcx, rcx
000000018039DA19 je        0x18039db1c
000000018039DA1F mov       eax, dword ptr [rcx + 0x4c]
000000018039DA22 cmp       dword ptr [rcx + 0x48], eax
000000018039DA25 mov       rcx, qword ptr [rbx + 0x30]
000000018039DA29 jge       0x18039d9e8
000000018039DA2B mov       rax, qword ptr [rbx + 0x20]
000000018039DA2F mov       rdx, qword ptr [rax + 0xe0]
000000018039DA36 test      rcx, rcx
000000018039DA39 je        0x18039db1c
000000018039DA3F movss     xmm1, dword ptr [rdx + 0x40]
000000018039DA44 xor       r8d, r8d
000000018039DA47 divss     xmm1, dword ptr [rdx + 0x44]
000000018039DA4C add       rsp, 0x30
000000018039DA50 pop       rbx
000000018039DA51 jmp       0x1813d7750
000000018039DA56 mov       rcx, qword ptr [rbx + 0x40]
000000018039DA5A test      rcx, rcx
000000018039DA5D je        0x18039db1c
000000018039DA63 xor       edx, edx
000000018039DA65 call      0x1813044d0
000000018039DA6A mov       rcx, qword ptr [rbx + 0x20]
000000018039DA6E test      rcx, rcx
000000018039DA71 je        0x18039db1c
000000018039DA77 mov       rcx, qword ptr [rcx + 0xe0]
000000018039DA7E test      rcx, rcx
000000018039DA81 je        0x18039db1c
000000018039DA87 test      rax, rax
000000018039DA8A je        0x18039db1c
000000018039DA90 cmp       dword ptr [rcx + 0x48], 1
000000018039DA94 mov       rcx, rax
000000018039DA97 setg      dl
000000018039DA9A xor       r8d, r8d
000000018039DA9D call      0x18131bad0
000000018039DAA2 mov       rcx, qword ptr [rbx + 0x30]
000000018039DAA6 test      rcx, rcx
000000018039DAA9 je        0x18039db1c
000000018039DAAB mov       rax, qword ptr [rcx]
000000018039DAAE lea       rdx, [rsp + 0x20]
000000018039DAB3 movdqa    xmm0, xmmword ptr [rip + 0x120a505] # 0x1815a7fc0
000000018039DABB movdqa    xmmword ptr [rsp + 0x20], xmm0
000000018039DAC1 mov       r8, qword ptr [rax + 0x2b0]
000000018039DAC8 call      qword ptr [rax + 0x2a8]
000000018039DACE mov       rax, qword ptr [rbx + 0x20]
000000018039DAD2 test      rax, rax
000000018039DAD5 je        0x18039db1c
000000018039DAD7 mov       rax, qword ptr [rax + 0xe0]
000000018039DADE test      rax, rax
000000018039DAE1 je        0x18039db1c
000000018039DAE3 cmp       dword ptr [rax + 0x2c], 3
000000018039DAE7 je        0x18039d9c3
000000018039DAED mov       rdx, qword ptr [rbx + 0x20]
000000018039DAF1 mov       rcx, qword ptr [rbx + 0x30]
000000018039DAF5 mov       r8, qword ptr [rdx + 0xe0]
000000018039DAFC test      rcx, rcx
000000018039DAFF je        0x18039db1c
000000018039DB01 movss     xmm1, dword ptr [rdx + 0xec]
000000018039DB09 divss     xmm1, dword ptr [r8 + 0x38]
000000018039DB0F xor       r8d, r8d
000000018039DB12 add       rsp, 0x30
000000018039DB16 pop       rbx
000000018039DB17 jmp       0x1813d7750
000000018039DB1C call      0x180250150
000000018039DB21 int3      