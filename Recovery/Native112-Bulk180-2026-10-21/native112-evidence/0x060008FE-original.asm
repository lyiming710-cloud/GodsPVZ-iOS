00000001803C4070 push      rdi
00000001803C4072 sub       rsp, 0x40
00000001803C4076 cmp       byte ptr [rip + 0x1920f9e], 0 # 0x181ce501b
00000001803C407D mov       rdi, rcx
00000001803C4080 jne       0x1803c40c5
00000001803C4082 lea       rcx, [rip + 0x17d907f] # 0x181b9d108
00000001803C4089 call      0x18024fef0
00000001803C408E lea       rcx, [rip + 0x17fd873] # 0x181bc1908
00000001803C4095 call      0x18024fef0
00000001803C409A lea       rcx, [rip + 0x17fd917] # 0x181bc19b8
00000001803C40A1 call      0x18024fef0
00000001803C40A6 lea       rcx, [rip + 0x17fd9bb] # 0x181bc1a68
00000001803C40AD call      0x18024fef0
00000001803C40B2 lea       rcx, [rip + 0x17fdab7] # 0x181bc1b70
00000001803C40B9 call      0x18024fef0
00000001803C40BE mov       byte ptr [rip + 0x1920f56], 1 # 0x181ce501b
00000001803C40C5 mov       rax, qword ptr [rip + 0x17d903c] # 0x181b9d108
00000001803C40CC cmp       dword ptr [rax + 0xe0], 0
00000001803C40D3 jne       0x1803c40e4
00000001803C40D5 mov       rcx, rax
00000001803C40D8 call      0x1802501e0
00000001803C40DD mov       rax, qword ptr [rip + 0x17d9024] # 0x181b9d108
00000001803C40E4 mov       rax, qword ptr [rax + 0xb8]
00000001803C40EB mov       qword ptr [rsp + 0x50], rbx
00000001803C40F0 mov       qword ptr [rsp + 0x58], rbp
00000001803C40F5 mov       qword ptr [rsp + 0x60], rsi
00000001803C40FA mov       rcx, qword ptr [rax + 0x58]
00000001803C40FE mov       qword ptr [rsp + 0x68], r14
00000001803C4103 movaps    xmmword ptr [rsp + 0x30], xmm6
00000001803C4108 test      rcx, rcx
00000001803C410B je        0x1803c446b
00000001803C4111 inc       dword ptr [rcx + 0x1c]
00000001803C4114 xor       ebx, ebx
00000001803C4116 mov       dword ptr [rcx + 0x18], ebx
00000001803C4119 mov       rax, qword ptr [rip + 0x17d8fe8] # 0x181b9d108
00000001803C4120 mov       rcx, qword ptr [rax + 0xb8]
00000001803C4127 mov       rcx, qword ptr [rcx + 0x58]
00000001803C412B test      rcx, rcx
00000001803C412E je        0x1803c446b
00000001803C4134 mov       rdx, qword ptr [rip + 0x17fd92d] # 0x181bc1a68
00000001803C413B call      0x18065baa0
00000001803C4140 test      rdi, rdi
00000001803C4143 je        0x1803c446b
00000001803C4149 mov       ecx, dword ptr [rdi + 0x18]
00000001803C414C add       ecx, ecx
00000001803C414E cmp       eax, ecx
00000001803C4150 jge       0x1803c4196
00000001803C4152 mov       rax, qword ptr [rip + 0x17d8faf] # 0x181b9d108
00000001803C4159 cmp       dword ptr [rax + 0xe0], ebx
00000001803C415F jne       0x1803c4170
00000001803C4161 mov       rcx, rax
00000001803C4164 call      0x1802501e0
00000001803C4169 mov       rax, qword ptr [rip + 0x17d8f98] # 0x181b9d108
00000001803C4170 mov       rax, qword ptr [rax + 0xb8]
00000001803C4177 mov       rcx, qword ptr [rax + 0x58]
00000001803C417B test      rcx, rcx
00000001803C417E je        0x1803c446b
00000001803C4184 mov       edx, dword ptr [rdi + 0x18]
00000001803C4187 mov       r8, qword ptr [rip + 0x17fd9e2] # 0x181bc1b70
00000001803C418E shl       edx, 2
00000001803C4191 call      0x180805920
00000001803C4196 mov       r14d, dword ptr [rdi + 0x18]
00000001803C419A test      r14d, r14d
00000001803C419D jle       0x1803c444c
00000001803C41A3 movss     xmm6, dword ptr [rip + 0x11e3e91] # 0x1815a803c
00000001803C41AB nop       dword ptr [rax + rax]
00000001803C41B0 cmp       ebx, dword ptr [rdi + 0x18]
00000001803C41B3 jae       0x1803c4471
00000001803C41B9 movsxd    rax, ebx
00000001803C41BC mov       ebp, dword ptr [rdi + rax*4 + 0x20]
00000001803C41C0 movsxd    rax, ebx
00000001803C41C3 inc       rax
00000001803C41C6 cmp       eax, dword ptr [rdi + 0x18]
00000001803C41C9 jae       0x1803c4471
00000001803C41CF mov       r9, qword ptr [rip + 0x17d8f32] # 0x181b9d108
00000001803C41D6 mov       esi, dword ptr [rdi + rax*4 + 0x20]
00000001803C41DA cmp       dword ptr [r9 + 0xe0], 0
00000001803C41E2 jne       0x1803c41f3
00000001803C41E4 mov       rcx, r9
00000001803C41E7 call      0x1802501e0
00000001803C41EC mov       r9, qword ptr [rip + 0x17d8f15] # 0x181b9d108
00000001803C41F3 mov       rcx, qword ptr [r9 + 0xb8]
00000001803C41FA mov       eax, ebp
00000001803C41FC shr       eax, 0x10
00000001803C41FF cwde      
00000001803C4200 movd      xmm0, eax
00000001803C4204 cvtdq2ps  xmm0, xmm0
00000001803C4207 movsx     eax, bp
00000001803C420A mulss     xmm0, xmm6
00000001803C420E movss     dword ptr [rcx + 0x48], xmm0
00000001803C4213 movd      xmm0, eax
00000001803C4217 mov       eax, esi
00000001803C4219 cvtdq2ps  xmm0, xmm0
00000001803C421C shr       eax, 0x10
00000001803C421F cwde      
00000001803C4220 mulss     xmm0, xmm6
00000001803C4224 movss     dword ptr [rcx + 0x4c], xmm0
00000001803C4229 movd      xmm0, eax
00000001803C422D movsx     eax, si
00000001803C4230 cvtdq2ps  xmm0, xmm0
00000001803C4233 movd      xmm1, eax
00000001803C4237 cvtdq2ps  xmm1, xmm1
00000001803C423A mulss     xmm0, xmm6
00000001803C423E mulss     xmm1, xmm6
00000001803C4242 movss     dword ptr [rcx + 0x50], xmm0
00000001803C4247 movss     dword ptr [rcx + 0x54], xmm1
00000001803C424C mov       rcx, qword ptr [rip + 0x17d8eb5] # 0x181b9d108
00000001803C4253 mov       rax, qword ptr [rcx + 0xb8]
00000001803C425A mov       r9, qword ptr [rax + 0x58]
00000001803C425E test      r9, r9
00000001803C4261 je        0x1803c446b
00000001803C4267 mov       r8, qword ptr [rip + 0x17fd69a] # 0x181bc1908
00000001803C426E movups    xmm0, xmmword ptr [rax + 0x48]
00000001803C4272 inc       dword ptr [r9 + 0x1c]
00000001803C4276 mov       rdx, qword ptr [r9 + 0x10]
00000001803C427A movsxd    rcx, dword ptr [r9 + 0x18]
00000001803C427E test      rdx, rdx
00000001803C4281 je        0x1803c446b
00000001803C4287 cmp       ecx, dword ptr [rdx + 0x18]
00000001803C428A jb        0x1803c42b0
00000001803C428C mov       rax, qword ptr [r8 + 0x20]
00000001803C4290 lea       rdx, [rsp + 0x20]
00000001803C4295 mov       rcx, r9
00000001803C4298 movdqa    xmmword ptr [rsp + 0x20], xmm0
00000001803C429E mov       r8, qword ptr [rax + 0xc0]
00000001803C42A5 mov       r8, qword ptr [r8 + 0x70]
00000001803C42A9 call      0x180807540
00000001803C42AE jmp       0x1803c42cb
00000001803C42B0 lea       eax, [rcx + 1]
00000001803C42B3 mov       dword ptr [r9 + 0x18], eax
00000001803C42B7 cmp       ecx, dword ptr [rdx + 0x18]
00000001803C42BA jae       0x1803c4471
00000001803C42C0 lea       rax, [rcx + 2]
00000001803C42C4 add       rax, rax
00000001803C42C7 movups    xmmword ptr [rdx + rax*8], xmm0
00000001803C42CB mov       rcx, qword ptr [rip + 0x17d8e36] # 0x181b9d108
00000001803C42D2 mov       rax, qword ptr [rcx + 0xb8]
00000001803C42D9 mov       r9, qword ptr [rax + 0x58]
00000001803C42DD test      r9, r9
00000001803C42E0 je        0x1803c446b
00000001803C42E6 mov       r8, qword ptr [rip + 0x17fd61b] # 0x181bc1908
00000001803C42ED movups    xmm0, xmmword ptr [rax + 0x48]
00000001803C42F1 inc       dword ptr [r9 + 0x1c]
00000001803C42F5 mov       rdx, qword ptr [r9 + 0x10]
00000001803C42F9 movsxd    rcx, dword ptr [r9 + 0x18]
00000001803C42FD test      rdx, rdx
00000001803C4300 je        0x1803c446b
00000001803C4306 cmp       ecx, dword ptr [rdx + 0x18]
00000001803C4309 jb        0x1803c432f
00000001803C430B mov       rax, qword ptr [r8 + 0x20]
00000001803C430F lea       rdx, [rsp + 0x20]
00000001803C4314 mov       rcx, r9
00000001803C4317 movdqa    xmmword ptr [rsp + 0x20], xmm0
00000001803C431D mov       r8, qword ptr [rax + 0xc0]
00000001803C4324 mov       r8, qword ptr [r8 + 0x70]
00000001803C4328 call      0x180807540
00000001803C432D jmp       0x1803c434a
00000001803C432F lea       eax, [rcx + 1]
00000001803C4332 mov       dword ptr [r9 + 0x18], eax
00000001803C4336 cmp       ecx, dword ptr [rdx + 0x18]
00000001803C4339 jae       0x1803c4471
00000001803C433F lea       rax, [rcx + 2]
00000001803C4343 add       rax, rax
00000001803C4346 movups    xmmword ptr [rdx + rax*8], xmm0
00000001803C434A mov       rcx, qword ptr [rip + 0x17d8db7] # 0x181b9d108
00000001803C4351 mov       rax, qword ptr [rcx + 0xb8]
00000001803C4358 mov       r9, qword ptr [rax + 0x58]
00000001803C435C test      r9, r9
00000001803C435F je        0x1803c446b
00000001803C4365 mov       r8, qword ptr [rip + 0x17fd59c] # 0x181bc1908
00000001803C436C movups    xmm0, xmmword ptr [rax + 0x48]
00000001803C4370 inc       dword ptr [r9 + 0x1c]
00000001803C4374 mov       rdx, qword ptr [r9 + 0x10]
00000001803C4378 movsxd    rcx, dword ptr [r9 + 0x18]
00000001803C437C test      rdx, rdx
00000001803C437F je        0x1803c446b
00000001803C4385 cmp       ecx, dword ptr [rdx + 0x18]
00000001803C4388 jb        0x1803c43ae
00000001803C438A mov       rax, qword ptr [r8 + 0x20]
00000001803C438E lea       rdx, [rsp + 0x20]
00000001803C4393 mov       rcx, r9
00000001803C4396 movdqa    xmmword ptr [rsp + 0x20], xmm0
00000001803C439C mov       r8, qword ptr [rax + 0xc0]
00000001803C43A3 mov       r8, qword ptr [r8 + 0x70]
00000001803C43A7 call      0x180807540
00000001803C43AC jmp       0x1803c43c9
00000001803C43AE lea       eax, [rcx + 1]
00000001803C43B1 mov       dword ptr [r9 + 0x18], eax
00000001803C43B5 cmp       ecx, dword ptr [rdx + 0x18]
00000001803C43B8 jae       0x1803c4471
00000001803C43BE lea       rax, [rcx + 2]
00000001803C43C2 add       rax, rax
00000001803C43C5 movups    xmmword ptr [rdx + rax*8], xmm0
00000001803C43C9 mov       rcx, qword ptr [rip + 0x17d8d38] # 0x181b9d108
00000001803C43D0 mov       rax, qword ptr [rcx + 0xb8]
00000001803C43D7 mov       r9, qword ptr [rax + 0x58]
00000001803C43DB test      r9, r9
00000001803C43DE je        0x1803c446b
00000001803C43E4 mov       r8, qword ptr [rip + 0x17fd51d] # 0x181bc1908
00000001803C43EB movups    xmm0, xmmword ptr [rax + 0x48]
00000001803C43EF inc       dword ptr [r9 + 0x1c]
00000001803C43F3 mov       rdx, qword ptr [r9 + 0x10]
00000001803C43F7 movsxd    rcx, dword ptr [r9 + 0x18]
00000001803C43FB test      rdx, rdx
00000001803C43FE je        0x1803c446b
00000001803C4400 cmp       ecx, dword ptr [rdx + 0x18]
00000001803C4403 jb        0x1803c4429
00000001803C4405 mov       rax, qword ptr [r8 + 0x20]
00000001803C4409 lea       rdx, [rsp + 0x20]
00000001803C440E mov       rcx, r9
00000001803C4411 movdqa    xmmword ptr [rsp + 0x20], xmm0
00000001803C4417 mov       r8, qword ptr [rax + 0xc0]
00000001803C441E mov       r8, qword ptr [r8 + 0x70]
00000001803C4422 call      0x180807540
00000001803C4427 jmp       0x1803c4440
00000001803C4429 lea       eax, [rcx + 1]
00000001803C442C mov       dword ptr [r9 + 0x18], eax
00000001803C4430 cmp       ecx, dword ptr [rdx + 0x18]
00000001803C4433 jae       0x1803c4471
00000001803C4435 lea       rax, [rcx + 2]
00000001803C4439 add       rax, rax
00000001803C443C movups    xmmword ptr [rdx + rax*8], xmm0
00000001803C4440 add       ebx, 2
00000001803C4443 cmp       ebx, r14d
00000001803C4446 jl        0x1803c41b0
00000001803C444C movaps    xmm6, xmmword ptr [rsp + 0x30]
00000001803C4451 mov       r14, qword ptr [rsp + 0x68]
00000001803C4456 mov       rsi, qword ptr [rsp + 0x60]
00000001803C445B mov       rbp, qword ptr [rsp + 0x58]
00000001803C4460 mov       rbx, qword ptr [rsp + 0x50]
00000001803C4465 add       rsp, 0x40
00000001803C4469 pop       rdi
00000001803C446A ret       
00000001803C446B call      0x180250150
00000001803C4470 int3      
00000001803C4471 call      0x180250140
00000001803C4476 int3      