000000018034E190 mov       qword ptr [rsp + 0x18], rbx
000000018034E195 push      rbp
000000018034E196 sub       rsp, 0x40
000000018034E19A cmp       byte ptr [rip + 0x1996b56], 0 # 0x181ce4cf7
000000018034E1A1 mov       rbp, rdx
000000018034E1A4 mov       rbx, rcx
000000018034E1A7 jne       0x18034e204
000000018034E1A9 lea       rcx, [rip + 0x1855da8] # 0x181ba3f58
000000018034E1B0 call      0x18024fef0
000000018034E1B5 lea       rcx, [rip + 0x1856f9c] # 0x181ba5158
000000018034E1BC call      0x18024fef0
000000018034E1C1 lea       rcx, [rip + 0x1853e50] # 0x181ba2018
000000018034E1C8 call      0x18024fef0
000000018034E1CD lea       rcx, [rip + 0x18560a4] # 0x181ba4278
000000018034E1D4 call      0x18024fef0
000000018034E1D9 lea       rcx, [rip + 0x1862320] # 0x181bb0500
000000018034E1E0 call      0x18024fef0
000000018034E1E5 lea       rcx, [rip + 0x186828c] # 0x181bb6478
000000018034E1EC call      0x18024fef0
000000018034E1F1 lea       rcx, [rip + 0x185b508] # 0x181ba9700
000000018034E1F8 call      0x18024fef0
000000018034E1FD mov       byte ptr [rip + 0x1996af3], 1 # 0x181ce4cf7
000000018034E204 mov       rcx, qword ptr [rbx + 0x1e8]
000000018034E20B mov       qword ptr [rsp + 0x50], rsi
000000018034E210 mov       qword ptr [rsp + 0x58], rdi
000000018034E215 movaps    xmmword ptr [rsp + 0x30], xmm6
000000018034E21A test      rcx, rcx
000000018034E21D je        0x18034e53a
000000018034E223 mov       r8d, dword ptr [rbx + 0x24]
000000018034E227 xor       r9d, r9d
000000018034E22A mov       edx, dword ptr [rbx + 0x20]
000000018034E22D call      0x180326fe0
000000018034E232 test      rax, rax
000000018034E235 je        0x18034e53a
000000018034E23B cmp       dword ptr [rax + 0x10], 3
000000018034E23F jne       0x18034e302
000000018034E245 mov       rax, qword ptr [rip + 0x186822c] # 0x181bb6478
000000018034E24C cmp       dword ptr [rax + 0xe0], 0
000000018034E253 jne       0x18034e264
000000018034E255 mov       rcx, rax
000000018034E258 call      0x1802501e0
000000018034E25D mov       rax, qword ptr [rip + 0x1868214] # 0x181bb6478
000000018034E264 mov       rax, qword ptr [rax + 0xb8]
000000018034E26B mov       rcx, qword ptr [rax + 0x130]
000000018034E272 test      rcx, rcx
000000018034E275 je        0x18034e53a
000000018034E27B mov       r8, qword ptr [rip + 0x1855ff6] # 0x181ba4278
000000018034E282 mov       edx, 6
000000018034E287 call      0x180825710
000000018034E28C xor       ecx, ecx
000000018034E28E mov       rbx, rax
000000018034E291 call      0x1812e27c0
000000018034E296 test      rax, rax
000000018034E299 je        0x18034e53a
000000018034E29F xor       edx, edx
000000018034E2A1 mov       rcx, rax
000000018034E2A4 call      0x181304510
000000018034E2A9 test      rax, rax
000000018034E2AC je        0x18034e53a
000000018034E2B2 xor       r8d, r8d
000000018034E2B5 lea       rcx, [rsp + 0x20]
000000018034E2BA mov       rdx, rax
000000018034E2BD call      0x18132f3a0
000000018034E2C2 mov       rcx, qword ptr [rip + 0x1853d4f] # 0x181ba2018
000000018034E2C9 movsd     xmm6, qword ptr [rax]
000000018034E2CD cmp       dword ptr [rcx + 0xe0], 0
000000018034E2D4 mov       edi, dword ptr [rax + 8]
000000018034E2D7 jne       0x18034e2de
000000018034E2D9 call      0x1802501e0
000000018034E2DE xor       ecx, ecx
000000018034E2E0 call      0x18031aea0
000000018034E2E5 xor       r9d, r9d
000000018034E2E8 movsd     qword ptr [rsp + 0x20], xmm6
000000018034E2EE movaps    xmm2, xmm0
000000018034E2F1 mov       dword ptr [rsp + 0x28], edi
000000018034E2F5 lea       rdx, [rsp + 0x20]
000000018034E2FA mov       rcx, rbx
000000018034E2FD call      0x18031b230
000000018034E302 cmp       byte ptr [rip + 0x199682b], 0 # 0x181ce4b34
000000018034E309 jne       0x18034e31e
000000018034E30B lea       rcx, [rip + 0x186357e] # 0x181bb1890
000000018034E312 call      0x18024fef0
000000018034E317 mov       byte ptr [rip + 0x1996816], 1 # 0x181ce4b34
000000018034E31E mov       rax, qword ptr [rip + 0x186356b] # 0x181bb1890
000000018034E325 mov       rcx, qword ptr [rax + 0xb8]
000000018034E32C mov       rcx, qword ptr [rcx]
000000018034E32F test      rcx, rcx
000000018034E332 je        0x18034e53a
000000018034E338 xor       r8d, r8d
000000018034E33B mov       edx, 0x16
000000018034E340 call      0x180321f30
000000018034E345 mov       rcx, qword ptr [rip + 0x18621b4] # 0x181bb0500
000000018034E34C mov       rbx, rax
000000018034E34F cmp       dword ptr [rcx + 0xe0], 0
000000018034E356 jne       0x18034e35d
000000018034E358 call      0x1802501e0
000000018034E35D xor       r8d, r8d
000000018034E360 xor       edx, edx
000000018034E362 mov       rcx, rbx
000000018034E365 call      0x18131f900
000000018034E36A test      al, al
000000018034E36C je        0x18034e520
000000018034E372 mov       rax, qword ptr [rip + 0x18680ff] # 0x181bb6478
000000018034E379 cmp       dword ptr [rax + 0xe0], 0
000000018034E380 jne       0x18034e391
000000018034E382 mov       rcx, rax
000000018034E385 call      0x1802501e0
000000018034E38A mov       rax, qword ptr [rip + 0x18680e7] # 0x181bb6478
000000018034E391 mov       rax, qword ptr [rax + 0xb8]
000000018034E398 mov       rcx, qword ptr [rax + 0x130]
000000018034E39F test      rcx, rcx
000000018034E3A2 je        0x18034e53a
000000018034E3A8 mov       r8, qword ptr [rip + 0x1855ec9] # 0x181ba4278
000000018034E3AF mov       edx, 5
000000018034E3B4 call      0x180825710
000000018034E3B9 xor       ecx, ecx
000000018034E3BB mov       rsi, rax
000000018034E3BE call      0x1812e27c0
000000018034E3C3 test      rax, rax
000000018034E3C6 je        0x18034e53a
000000018034E3CC xor       edx, edx
000000018034E3CE mov       rcx, rax
000000018034E3D1 call      0x181304510
000000018034E3D6 test      rax, rax
000000018034E3D9 je        0x18034e53a
000000018034E3DF xor       r8d, r8d
000000018034E3E2 lea       rcx, [rsp + 0x20]
000000018034E3E7 mov       rdx, rax
000000018034E3EA call      0x18132f3a0
000000018034E3EF mov       rcx, qword ptr [rip + 0x1853c22] # 0x181ba2018
000000018034E3F6 movsd     xmm6, qword ptr [rax]
000000018034E3FA cmp       dword ptr [rcx + 0xe0], 0
000000018034E401 mov       edi, dword ptr [rax + 8]
000000018034E404 jne       0x18034e40b
000000018034E406 call      0x1802501e0
000000018034E40B xor       ecx, ecx
000000018034E40D call      0x18031aea0
000000018034E412 xor       r9d, r9d
000000018034E415 movsd     qword ptr [rsp + 0x20], xmm6
000000018034E41B movaps    xmm2, xmm0
000000018034E41E mov       dword ptr [rsp + 0x28], edi
000000018034E422 lea       rdx, [rsp + 0x20]
000000018034E427 mov       rcx, rsi
000000018034E42A call      0x18031b230
000000018034E42F test      rbx, rbx
000000018034E432 je        0x18034e53a
000000018034E438 xor       edx, edx
000000018034E43A mov       rcx, rbx
000000018034E43D call      0x18131c070
000000018034E442 test      rax, rax
000000018034E445 je        0x18034e53a
000000018034E44B mov       edx, dword ptr [rbp + 8]
000000018034E44E xor       r8d, r8d
000000018034E451 movsd     xmm0, qword ptr [rbp]
000000018034E456 mov       rcx, rax
000000018034E459 mov       dword ptr [rsp + 0x28], edx
000000018034E45D lea       rdx, [rsp + 0x20]
000000018034E462 movsd     qword ptr [rsp + 0x20], xmm0
000000018034E468 call      0x18132fbb0
000000018034E46D xor       edx, edx
000000018034E46F mov       rcx, rbx
000000018034E472 call      0x18131c070
000000018034E477 cmp       byte ptr [rip + 0x19966b6], 0 # 0x181ce4b34
000000018034E47E mov       rdi, rax
000000018034E481 jne       0x18034e496
000000018034E483 lea       rcx, [rip + 0x1863406] # 0x181bb1890
000000018034E48A call      0x18024fef0
000000018034E48F mov       byte ptr [rip + 0x199669e], 1 # 0x181ce4b34
000000018034E496 mov       rcx, qword ptr [rip + 0x18633f3] # 0x181bb1890
000000018034E49D mov       rdx, qword ptr [rcx + 0xb8]
000000018034E4A4 mov       rcx, qword ptr [rdx]
000000018034E4A7 test      rcx, rcx
000000018034E4AA je        0x18034e53a
000000018034E4B0 xor       edx, edx
000000018034E4B2 call      0x181304510
000000018034E4B7 test      rdi, rdi
000000018034E4BA je        0x18034e53a
000000018034E4BC xor       r8d, r8d
000000018034E4BF mov       rdx, rax
000000018034E4C2 mov       rcx, rdi
000000018034E4C5 call      0x18132e8b0
000000018034E4CA mov       rdx, qword ptr [rip + 0x1855a87] # 0x181ba3f58
000000018034E4D1 mov       rcx, rbx
000000018034E4D4 call      0x18046d280
000000018034E4D9 mov       rdx, qword ptr [rip + 0x1856c78] # 0x181ba5158
000000018034E4E0 mov       rcx, rbx
000000018034E4E3 call      0x18046dc80
000000018034E4E8 test      rax, rax
000000018034E4EB je        0x18034e53a
000000018034E4ED mov       rdx, qword ptr [rip + 0x185b20c] # 0x181ba9700
000000018034E4F4 xor       r8d, r8d
000000018034E4F7 mov       rcx, rax
000000018034E4FA call      0x181348060
000000018034E4FF mov       rdx, qword ptr [rip + 0x1856c52] # 0x181ba5158
000000018034E506 mov       rcx, rbx
000000018034E509 call      0x18046dc80
000000018034E50E test      rax, rax
000000018034E511 je        0x18034e53a
000000018034E513 xor       r8d, r8d
000000018034E516 xor       edx, edx
000000018034E518 mov       rcx, rax
000000018034E51B call      0x1813480b0
000000018034E520 movaps    xmm6, xmmword ptr [rsp + 0x30]
000000018034E525 mov       rdi, qword ptr [rsp + 0x58]
000000018034E52A mov       rsi, qword ptr [rsp + 0x50]
000000018034E52F mov       rbx, qword ptr [rsp + 0x60]
000000018034E534 add       rsp, 0x40
000000018034E538 pop       rbp
000000018034E539 ret       
000000018034E53A call      0x180250150
000000018034E53F int3      