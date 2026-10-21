0000000180320AF0 mov       qword ptr [rsp + 0x18], rbx
0000000180320AF5 push      rdi
0000000180320AF6 sub       rsp, 0x40
0000000180320AFA cmp       byte ptr [rip + 0x19c40d4], 0 # 0x181ce4bd5
0000000180320B01 mov       rbx, rcx
0000000180320B04 jne       0x180320b19
0000000180320B06 lea       rcx, [rip + 0x188f9f3] # 0x181bb0500
0000000180320B0D call      0x18024fef0
0000000180320B12 mov       byte ptr [rip + 0x19c40bc], 1 # 0x181ce4bd5
0000000180320B19 mov       rcx, qword ptr [rip + 0x188f9e0] # 0x181bb0500
0000000180320B20 mov       rdi, qword ptr [rbx + 0xa8]
0000000180320B27 cmp       dword ptr [rcx + 0xe0], 0
0000000180320B2E jne       0x180320b35
0000000180320B30 call      0x1802501e0
0000000180320B35 mov       qword ptr [rsp + 0x50], rbp
0000000180320B3A xor       r8d, r8d
0000000180320B3D xor       edx, edx
0000000180320B3F mov       qword ptr [rsp + 0x58], rsi
0000000180320B44 mov       rcx, rdi
0000000180320B47 call      0x18131f760
0000000180320B4C test      al, al
0000000180320B4E jne       0x180320b65
0000000180320B50 mov       rax, qword ptr [rbx + 0xa8]
0000000180320B57 test      rax, rax
0000000180320B5A je        0x180320e31
0000000180320B60 mov       edx, dword ptr [rax + 0x7c]
0000000180320B63 jmp       0x180320b6a
0000000180320B65 mov       edx, 0xffffffff
0000000180320B6A xor       r8d, r8d
0000000180320B6D mov       rcx, rbx
0000000180320B70 call      0x18031ee10
0000000180320B75 mov       rcx, qword ptr [rbx + 0xa8]
0000000180320B7C mov       rsi, rax
0000000180320B7F test      rcx, rcx
0000000180320B82 je        0x180320e31
0000000180320B88 xor       r9d, r9d
0000000180320B8B xor       edx, edx
0000000180320B8D mov       r8d, 0xffffffff
0000000180320B93 call      0x180350e20
0000000180320B98 xor       r9d, r9d
0000000180320B9B mov       r8, rax
0000000180320B9E mov       dl, 1
0000000180320BA0 mov       rcx, rbx
0000000180320BA3 call      0x18031e300
0000000180320BA8 mov       rdi, qword ptr [rbx + 0xa8]
0000000180320BAF mov       rbp, rax
0000000180320BB2 test      rdi, rdi
0000000180320BB5 je        0x180320e31
0000000180320BBB mov       rcx, qword ptr [rbx + 0xa8]
0000000180320BC2 mov       rdi, qword ptr [rdi + 0xe0]
0000000180320BC9 mov       edx, dword ptr [rcx + 0x7c]
0000000180320BCC test      edx, edx
0000000180320BCE jne       0x180320ce2
0000000180320BD4 test      rdi, rdi
0000000180320BD7 je        0x180320e31
0000000180320BDD cmp       dword ptr [rdi + 0x18], 1
0000000180320BE1 jne       0x180320dc1
0000000180320BE7 mov       rcx, qword ptr [rip + 0x188f912] # 0x181bb0500
0000000180320BEE cmp       dword ptr [rcx + 0xe0], edx
0000000180320BF4 jne       0x180320bfb
0000000180320BF6 call      0x1802501e0
0000000180320BFB xor       edx, edx
0000000180320BFD mov       rcx, rsi
0000000180320C00 call      0x18131f870
0000000180320C05 test      al, al
0000000180320C07 je        0x180320c5d
0000000180320C09 mov       rcx, qword ptr [rbx + 0xa8]
0000000180320C10 test      rcx, rcx
0000000180320C13 je        0x180320e31
0000000180320C19 mov       qword ptr [rcx + 0x120], rsi
0000000180320C20 mov       rdx, rsi
0000000180320C23 add       rcx, 0x120
0000000180320C2A call      0x18024f360
0000000180320C2F mov       rcx, qword ptr [rbx + 0xa8]
0000000180320C36 test      rcx, rcx
0000000180320C39 je        0x180320e31
0000000180320C3F mov       qword ptr [rcx + 0x128], 0
0000000180320C4A xor       edx, edx
0000000180320C4C add       rcx, 0x128
0000000180320C53 call      0x18024f360
0000000180320C58 jmp       0x180320dae
0000000180320C5D cmp       dword ptr [rdi + 0x18], 1
0000000180320C61 jne       0x180320dc1
0000000180320C67 mov       rcx, qword ptr [rip + 0x188f892] # 0x181bb0500
0000000180320C6E cmp       dword ptr [rcx + 0xe0], 0
0000000180320C75 jne       0x180320c7c
0000000180320C77 call      0x1802501e0
0000000180320C7C xor       edx, edx
0000000180320C7E mov       rcx, rbp
0000000180320C81 call      0x18131f870
0000000180320C86 test      al, al
0000000180320C88 je        0x180320dc1
0000000180320C8E mov       rcx, qword ptr [rbx + 0xa8]
0000000180320C95 test      rcx, rcx
0000000180320C98 je        0x180320e31
0000000180320C9E mov       qword ptr [rcx + 0x120], 0
0000000180320CA9 xor       edx, edx
0000000180320CAB add       rcx, 0x120
0000000180320CB2 call      0x18024f360
0000000180320CB7 mov       rcx, qword ptr [rbx + 0xa8]
0000000180320CBE test      rcx, rcx
0000000180320CC1 je        0x180320e31
0000000180320CC7 mov       qword ptr [rcx + 0x128], rbp
0000000180320CCE mov       rdx, rbp
0000000180320CD1 add       rcx, 0x128
0000000180320CD8 call      0x18024f360
0000000180320CDD jmp       0x180320dae
0000000180320CE2 cmp       edx, 4
0000000180320CE5 jne       0x180320d78
0000000180320CEB test      rdi, rdi
0000000180320CEE je        0x180320e31
0000000180320CF4 cmp       dword ptr [rdi + 0x18], 1
0000000180320CF8 jne       0x180320dc1
0000000180320CFE test      rcx, rcx
0000000180320D01 je        0x180320e31
0000000180320D07 mov       rcx, qword ptr [rcx + 0x70]
0000000180320D0B test      rcx, rcx
0000000180320D0E je        0x180320e31
0000000180320D14 xor       r8d, r8d
0000000180320D17 mov       edx, 1
0000000180320D1C call      0x1803944e0
0000000180320D21 mov       rdi, qword ptr [rbx + 0xa8]
0000000180320D28 test      rax, rax
0000000180320D2B je        0x180320e31
0000000180320D31 mov       ecx, dword ptr [rbx + 0x64]
0000000180320D34 lea       r8, [rsp + 0x20]
0000000180320D39 movsd     xmm0, qword ptr [rbx + 0x5c]
0000000180320D3E xor       r9d, r9d
0000000180320D41 mov       dword ptr [rsp + 0x28], ecx
0000000180320D45 mov       rdx, rax
0000000180320D48 lea       rcx, [rsp + 0x30]
0000000180320D4D movsd     qword ptr [rsp + 0x20], xmm0
0000000180320D53 call      0x1802ffa20
0000000180320D58 mov       ecx, dword ptr [rax + 8]
0000000180320D5B test      rdi, rdi
0000000180320D5E je        0x180320e31
0000000180320D64 movsd     xmm0, qword ptr [rax]
0000000180320D68 movsd     qword ptr [rdi + 0x138], xmm0
0000000180320D70 mov       dword ptr [rdi + 0x140], ecx
0000000180320D76 jmp       0x180320dae
0000000180320D78 cmp       edx, 0x31
0000000180320D7B jne       0x180320dc1
0000000180320D7D test      rdi, rdi
0000000180320D80 je        0x180320e31
0000000180320D86 cmp       dword ptr [rdi + 0x18], 1
0000000180320D8A jne       0x180320dc1
0000000180320D8C mov       rax, rcx
0000000180320D8F mov       ecx, dword ptr [rbx + 0x64]
0000000180320D92 test      rax, rax
0000000180320D95 je        0x180320e31
0000000180320D9B movsd     xmm0, qword ptr [rbx + 0x5c]
0000000180320DA0 movsd     qword ptr [rax + 0x138], xmm0
0000000180320DA8 mov       dword ptr [rax + 0x140], ecx
0000000180320DAE mov       rcx, qword ptr [rbx + 0xa8]
0000000180320DB5 test      rcx, rcx
0000000180320DB8 je        0x180320e31
0000000180320DBA xor       edx, edx
0000000180320DBC call      0x18035b150
0000000180320DC1 mov       rax, qword ptr [rbx + 0xa8]
0000000180320DC8 test      rax, rax
0000000180320DCB je        0x180320e31
0000000180320DCD mov       rcx, qword ptr [rax + 0x70]
0000000180320DD1 test      rcx, rcx
0000000180320DD4 je        0x180320e31
0000000180320DD6 xor       edx, edx
0000000180320DD8 call      0x180394350
0000000180320DDD mov       rcx, qword ptr [rip + 0x188f71c] # 0x181bb0500
0000000180320DE4 mov       rdi, qword ptr [rbx + 0x90]
0000000180320DEB cmp       dword ptr [rcx + 0xe0], 0
0000000180320DF2 jne       0x180320df9
0000000180320DF4 call      0x1802501e0
0000000180320DF9 xor       edx, edx
0000000180320DFB mov       rcx, rdi
0000000180320DFE call      0x18131e940
0000000180320E03 xor       edx, edx
0000000180320E05 mov       rcx, rbx
0000000180320E08 call      0x180320fe0
0000000180320E0D mov       rcx, qword ptr [rbx + 0x28]
0000000180320E11 test      rcx, rcx
0000000180320E14 je        0x180320e31
0000000180320E16 xor       edx, edx
0000000180320E18 mov       rsi, qword ptr [rsp + 0x58]
0000000180320E1D mov       rbp, qword ptr [rsp + 0x50]
0000000180320E22 mov       rbx, qword ptr [rsp + 0x60]
0000000180320E27 add       rsp, 0x40
0000000180320E2B pop       rdi
0000000180320E2C jmp       0x180326190
0000000180320E31 call      0x180250150
0000000180320E36 int3      
0000000180320E37 int3      
0000000180320E38 int3      
0000000180320E39 int3      
0000000180320E3A int3      
0000000180320E3B int3      
0000000180320E3C int3      
0000000180320E3D int3      
0000000180320E3E int3      
0000000180320E3F int3      