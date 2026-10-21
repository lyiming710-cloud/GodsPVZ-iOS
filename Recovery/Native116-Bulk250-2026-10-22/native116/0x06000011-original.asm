00000001802FD1E0 mov       qword ptr [rsp + 8], rbx
00000001802FD1E5 mov       qword ptr [rsp + 0x10], rsi
00000001802FD1EA mov       qword ptr [rsp + 0x18], rdi
00000001802FD1EF mov       qword ptr [rsp + 0x20], r14
00000001802FD1F4 push      r15
00000001802FD1F6 sub       rsp, 0x70
00000001802FD1FA mov       rsi, rcx
00000001802FD1FD cmp       byte ptr [rip + 0x19e78a9], 0 # 0x181ce4aad
00000001802FD204 jne       0x1802fd23d
00000001802FD206 lea       rcx, [rip + 0x18c5703] # 0x181bc2910
00000001802FD20D call      0x18024fef0
00000001802FD212 lea       rcx, [rip + 0x18c574f] # 0x181bc2968
00000001802FD219 call      0x18024fef0
00000001802FD21E lea       rcx, [rip + 0x18c579b] # 0x181bc29c0
00000001802FD225 call      0x18024fef0
00000001802FD22A lea       rcx, [rip + 0x18ac35f] # 0x181ba9590
00000001802FD231 call      0x18024fef0
00000001802FD236 mov       byte ptr [rip + 0x19e7870], 1 # 0x181ce4aad
00000001802FD23D xor       ecx, ecx
00000001802FD23F call      0x1812e27c0
00000001802FD244 mov       rbx, rax
00000001802FD247 xor       edx, edx
00000001802FD249 lea       rcx, [rsp + 0x40]
00000001802FD24E call      0x181364ca0
00000001802FD253 test      rbx, rbx
00000001802FD256 je        0x1802fd40d
00000001802FD25C movsd     xmm0, qword ptr [rax]
00000001802FD260 movsd     qword ptr [rsp + 0x30], xmm0
00000001802FD266 mov       ecx, dword ptr [rax + 8]
00000001802FD269 mov       dword ptr [rsp + 0x38], ecx
00000001802FD26D xor       r9d, r9d
00000001802FD270 lea       r8, [rsp + 0x30]
00000001802FD275 mov       rdx, rbx
00000001802FD278 lea       rcx, [rsp + 0x40]
00000001802FD27D call      0x1812e1d50
00000001802FD282 movsd     xmm0, qword ptr [rax]
00000001802FD286 movsd     qword ptr [rsp + 0x30], xmm0
00000001802FD28C mov       rcx, qword ptr [rsi + 0x28]
00000001802FD290 test      rcx, rcx
00000001802FD293 je        0x1802fd40d
00000001802FD299 xor       r9d, r9d
00000001802FD29C movss     xmm2, dword ptr [rsp + 0x34]
00000001802FD2A2 movss     xmm1, dword ptr [rsp + 0x30]
00000001802FD2A8 call      0x18030ed50
00000001802FD2AD mov       r15d, eax
00000001802FD2B0 mov       rcx, qword ptr [rsi + 0x28]
00000001802FD2B4 test      rcx, rcx
00000001802FD2B7 je        0x1802fd40d
00000001802FD2BD xor       r9d, r9d
00000001802FD2C0 movss     xmm2, dword ptr [rsp + 0x34]
00000001802FD2C6 movss     xmm1, dword ptr [rsp + 0x30]
00000001802FD2CC call      0x18030ee00
00000001802FD2D1 mov       r14d, eax
00000001802FD2D4 mov       rax, qword ptr [rsi + 0x50]
00000001802FD2D8 test      rax, rax
00000001802FD2DB je        0x1802fd40d
00000001802FD2E1 cmp       byte ptr [rax + 0x120], 0
00000001802FD2E8 je        0x1802fd3db
00000001802FD2EE mov       rdx, qword ptr [rsi + 0x90]
00000001802FD2F5 test      rdx, rdx
00000001802FD2F8 je        0x1802fd40d
00000001802FD2FE mov       r8, qword ptr [rip + 0x18ac28b] # 0x181ba9590
00000001802FD305 lea       rcx, [rsp + 0x40]
00000001802FD30A call      0x1808197f0
00000001802FD30F movups    xmm0, xmmword ptr [rsp + 0x40]
00000001802FD314 movups    xmmword ptr [rsp + 0x58], xmm0
00000001802FD319 movsd     xmm1, qword ptr [rsp + 0x50]
00000001802FD31F movsd     qword ptr [rsp + 0x68], xmm1
00000001802FD325 mov       qword ptr [rsp + 0x40], 0
00000001802FD32E lea       rbx, [rsp + 0x58]
00000001802FD333 mov       qword ptr [rsp + 0x48], rbx
00000001802FD338 nop       dword ptr [rax + rax]
00000001802FD340 mov       rdx, qword ptr [rip + 0x18c5621] # 0x181bc2968
00000001802FD347 lea       rcx, [rsp + 0x58]
00000001802FD34C call      0x180669280
00000001802FD351 test      al, al
00000001802FD353 je        0x1802fd3af
00000001802FD355 mov       rdi, qword ptr [rsp + 0x68]
00000001802FD35A test      rdi, rdi
00000001802FD35D je        0x1802fd401
00000001802FD363 mov       rax, qword ptr [rdi + 0x30]
00000001802FD367 test      rax, rax
00000001802FD36A je        0x1802fd3fc
00000001802FD370 cmp       dword ptr [rax + 0x10], r15d
00000001802FD374 jne       0x1802fd340
00000001802FD376 cmp       dword ptr [rax + 0x14], r14d
00000001802FD37A jne       0x1802fd340
00000001802FD37C xor       edx, edx
00000001802FD37E xor       ecx, ecx
00000001802FD380 call      0x1813649f0
00000001802FD385 test      al, al
00000001802FD387 je        0x1802fd340
00000001802FD389 mov       rcx, qword ptr [rsi + 0x78]
00000001802FD38D test      rcx, rcx
00000001802FD390 je        0x1802fd3f6
00000001802FD392 xor       r8d, r8d
00000001802FD395 mov       rdx, rdi
00000001802FD398 call      0x180302a40
00000001802FD39D nop       
00000001802FD39E mov       rdx, qword ptr [rip + 0x18c556b] # 0x181bc2910
00000001802FD3A5 mov       rcx, rbx
00000001802FD3A8 call      0x180302170
00000001802FD3AD jmp       0x1802fd3db
00000001802FD3AF mov       rdx, qword ptr [rip + 0x18c555a] # 0x181bc2910
00000001802FD3B6 mov       rcx, rbx
00000001802FD3B9 call      0x180302170
00000001802FD3BE jmp       0x1802fd3db
00000001802FD3C0 mov       rdx, qword ptr [rip + 0x18c5549] # 0x181bc2910
00000001802FD3C7 mov       rcx, qword ptr [rsp + 0x48]
00000001802FD3CC call      0x180302170
00000001802FD3D1 mov       rcx, qword ptr [rsp + 0x40]
00000001802FD3D6 test      rcx, rcx
00000001802FD3D9 jne       0x1802fd407
00000001802FD3DB lea       r11, [rsp + 0x70]
00000001802FD3E0 mov       rbx, qword ptr [r11 + 0x10]
00000001802FD3E4 mov       rsi, qword ptr [r11 + 0x18]
00000001802FD3E8 mov       rdi, qword ptr [r11 + 0x20]
00000001802FD3EC mov       r14, qword ptr [r11 + 0x28]
00000001802FD3F0 mov       rsp, r11
00000001802FD3F3 pop       r15
00000001802FD3F5 ret       
00000001802FD3F6 call      0x180250150
00000001802FD3FB nop       
00000001802FD3FC call      0x180250150
00000001802FD401 call      0x180250150
00000001802FD406 nop       
00000001802FD407 call      0x180200e60
00000001802FD40C int3      
00000001802FD40D call      0x180250150
00000001802FD412 int3      
00000001802FD413 int3      
00000001802FD414 int3      
00000001802FD415 int3      
00000001802FD416 int3      
00000001802FD417 int3      
00000001802FD418 int3      
00000001802FD419 int3      
00000001802FD41A int3      
00000001802FD41B int3      
00000001802FD41C int3      
00000001802FD41D int3      
00000001802FD41E int3      
00000001802FD41F int3      