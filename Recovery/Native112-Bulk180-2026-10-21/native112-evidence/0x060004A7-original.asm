0000000180372B60 push      rbx
0000000180372B62 sub       rsp, 0x40
0000000180372B66 cmp       byte ptr [rip + 0x197223d], 0 # 0x181ce4daa
0000000180372B6D mov       rbx, rcx
0000000180372B70 jne       0x180372bb5
0000000180372B72 lea       rcx, [rip + 0x182f49f] # 0x181ba2018
0000000180372B79 call      0x18024fef0
0000000180372B7E lea       rcx, [rip + 0x18316f3] # 0x181ba4278
0000000180372B85 call      0x18024fef0
0000000180372B8A lea       rcx, [rip + 0x18438e7] # 0x181bb6478
0000000180372B91 call      0x18024fef0
0000000180372B96 lea       rcx, [rip + 0x1847c33] # 0x181bba7d0
0000000180372B9D call      0x18024fef0
0000000180372BA2 lea       rcx, [rip + 0x184090f] # 0x181bb34b8
0000000180372BA9 call      0x18024fef0
0000000180372BAE mov       byte ptr [rip + 0x19721f5], 1 # 0x181ce4daa
0000000180372BB5 cmp       byte ptr [rbx + 0xbf], 0
0000000180372BBC jne       0x180372dfa
0000000180372BC2 mov       rcx, qword ptr [rbx + 0x228]
0000000180372BC9 mov       qword ptr [rsp + 0x58], rdi
0000000180372BCE test      rcx, rcx
0000000180372BD1 je        0x180372e00
0000000180372BD7 mov       rdx, qword ptr [rip + 0x18408da] # 0x181bb34b8
0000000180372BDE xor       r8d, r8d
0000000180372BE1 call      0x180310250
0000000180372BE6 test      rax, rax
0000000180372BE9 je        0x180372c1e
0000000180372BEB mov       r8, qword ptr [rip + 0x1847bde] # 0x181bba7d0
0000000180372BF2 mov       r9, qword ptr [rax]
0000000180372BF5 movzx     ecx, byte ptr [r8 + 0x130]
0000000180372BFD cmp       byte ptr [r9 + 0x130], cl
0000000180372C04 jb        0x180372c1e
0000000180372C06 movzx     edx, cl
0000000180372C09 mov       rcx, qword ptr [r9 + 0xc8]
0000000180372C10 cmp       qword ptr [rcx + rdx*8 - 8], r8
0000000180372C15 jne       0x180372c1e
0000000180372C17 mov       dword ptr [rax + 0x60], 0xbf800000
0000000180372C1E cmp       byte ptr [rip + 0x1971f11], 0 # 0x181ce4b36
0000000180372C25 jne       0x180372c3a
0000000180372C27 lea       rcx, [rip + 0x184fbba] # 0x181bc27e8
0000000180372C2E call      0x18024fef0
0000000180372C33 mov       byte ptr [rip + 0x1971efc], 1 # 0x181ce4b36
0000000180372C3A mov       rax, qword ptr [rip + 0x184fba7] # 0x181bc27e8
0000000180372C41 mov       rcx, qword ptr [rax + 0xb8]
0000000180372C48 mov       eax, dword ptr [rcx + 8]
0000000180372C4B movsd     xmm0, qword ptr [rcx]
0000000180372C4F movsd     qword ptr [rbx + 0x13c], xmm0
0000000180372C57 mov       dword ptr [rbx + 0x144], eax
0000000180372C5D mov       rax, qword ptr [rip + 0x1843814] # 0x181bb6478
0000000180372C64 cmp       dword ptr [rax + 0xe0], 0
0000000180372C6B jne       0x180372c7c
0000000180372C6D mov       rcx, rax
0000000180372C70 call      0x1802501e0
0000000180372C75 mov       rax, qword ptr [rip + 0x18437fc] # 0x181bb6478
0000000180372C7C mov       rax, qword ptr [rax + 0xb8]
0000000180372C83 mov       rcx, qword ptr [rax + 0x138]
0000000180372C8A test      rcx, rcx
0000000180372C8D je        0x180372e00
0000000180372C93 mov       r8, qword ptr [rip + 0x18315de] # 0x181ba4278
0000000180372C9A mov       edx, 0xc
0000000180372C9F call      0x180825710
0000000180372CA4 xor       ecx, ecx
0000000180372CA6 mov       rdi, rax
0000000180372CA9 call      0x1812e27c0
0000000180372CAE test      rax, rax
0000000180372CB1 je        0x180372e00
0000000180372CB7 xor       edx, edx
0000000180372CB9 mov       rcx, rax
0000000180372CBC call      0x181304510
0000000180372CC1 test      rax, rax
0000000180372CC4 je        0x180372e00
0000000180372CCA mov       qword ptr [rsp + 0x50], rsi
0000000180372CCF lea       rcx, [rsp + 0x20]
0000000180372CD4 xor       r8d, r8d
0000000180372CD7 movaps    xmmword ptr [rsp + 0x30], xmm6
0000000180372CDC mov       rdx, rax
0000000180372CDF call      0x18132f3a0
0000000180372CE4 mov       rcx, qword ptr [rip + 0x182f32d] # 0x181ba2018
0000000180372CEB movsd     xmm6, qword ptr [rax]
0000000180372CEF cmp       dword ptr [rcx + 0xe0], 0
0000000180372CF6 mov       esi, dword ptr [rax + 8]
0000000180372CF9 jne       0x180372d00
0000000180372CFB call      0x1802501e0
0000000180372D00 xor       ecx, ecx
0000000180372D02 call      0x18031aea0
0000000180372D07 xor       r9d, r9d
0000000180372D0A movsd     qword ptr [rsp + 0x20], xmm6
0000000180372D10 movaps    xmm2, xmm0
0000000180372D13 mov       dword ptr [rsp + 0x28], esi
0000000180372D17 lea       rdx, [rsp + 0x20]
0000000180372D1C mov       rcx, rdi
0000000180372D1F call      0x18031b230
0000000180372D24 cmp       byte ptr [rip + 0x197207e], 0 # 0x181ce4da9
0000000180372D2B movaps    xmm6, xmmword ptr [rsp + 0x30]
0000000180372D30 mov       rsi, qword ptr [rsp + 0x50]
0000000180372D35 jne       0x180372d56
0000000180372D37 lea       rcx, [rip + 0x183d7c2] # 0x181bb0500
0000000180372D3E call      0x18024fef0
0000000180372D43 lea       rcx, [rip + 0x18551fe] # 0x181bc7f48
0000000180372D4A call      0x18024fef0
0000000180372D4F mov       byte ptr [rip + 0x1972053], 1 # 0x181ce4da9
0000000180372D56 movss     xmm1, dword ptr [rbx + 0xc0]
0000000180372D5E subss     xmm1, dword ptr [rip + 0x1234ffa] # 0x1815a7d60
0000000180372D66 movss     dword ptr [rbx + 0xc0], xmm1
0000000180372D6E xorps     xmm0, xmm0
0000000180372D71 comiss    xmm0, xmm1
0000000180372D74 jb        0x180372db8
0000000180372D76 mov       rcx, qword ptr [rbx + 0x158]
0000000180372D7D mov       dword ptr [rbx + 0xc0], 0
0000000180372D87 mov       byte ptr [rbx + 0xbf], 1
0000000180372D8E test      rcx, rcx
0000000180372D91 je        0x180372e00
0000000180372D93 mov       rdx, qword ptr [rip + 0x18551ae] # 0x181bc7f48
0000000180372D9A xor       r9d, r9d
0000000180372D9D mov       r8b, 1
0000000180372DA0 call      0x1812dba00
0000000180372DA5 movss     xmm1, dword ptr [rip + 0x1234e4f] # 0x1815a7bfc
0000000180372DAD xor       r8d, r8d
0000000180372DB0 mov       rcx, rbx
0000000180372DB3 call      0x18036a5e0
0000000180372DB8 mov       rcx, qword ptr [rip + 0x183d741] # 0x181bb0500
0000000180372DBF mov       rdi, qword ptr [rbx + 0x208]
0000000180372DC6 cmp       dword ptr [rcx + 0xe0], 0
0000000180372DCD jne       0x180372dd4
0000000180372DCF call      0x1802501e0
0000000180372DD4 xor       edx, edx
0000000180372DD6 mov       rcx, rdi
0000000180372DD9 call      0x18131f870
0000000180372DDE test      al, al
0000000180372DE0 je        0x180372df5
0000000180372DE2 mov       rcx, qword ptr [rbx + 0x208]
0000000180372DE9 test      rcx, rcx
0000000180372DEC je        0x180372e00
0000000180372DEE xor       edx, edx
0000000180372DF0 call      0x18039dba0
0000000180372DF5 mov       rdi, qword ptr [rsp + 0x58]
0000000180372DFA add       rsp, 0x40
0000000180372DFE pop       rbx
0000000180372DFF ret       
0000000180372E00 call      0x180250150
0000000180372E05 int3      