00000001803C5140 push      rdi
00000001803C5142 sub       rsp, 0x50
00000001803C5146 cmp       byte ptr [rip + 0x191fecc], 0 # 0x181ce5019
00000001803C514D mov       rdi, rcx
00000001803C5150 jne       0x1803c5195
00000001803C5152 lea       rcx, [rip + 0x17d7faf] # 0x181b9d108
00000001803C5159 call      0x18024fef0
00000001803C515E lea       rcx, [rip + 0x17fc223] # 0x181bc1388
00000001803C5165 call      0x18024fef0
00000001803C516A lea       rcx, [rip + 0x17fc2c7] # 0x181bc1438
00000001803C5171 call      0x18024fef0
00000001803C5176 lea       rcx, [rip + 0x17fc36b] # 0x181bc14e8
00000001803C517D call      0x18024fef0
00000001803C5182 lea       rcx, [rip + 0x17fc467] # 0x181bc15f0
00000001803C5189 call      0x18024fef0
00000001803C518E mov       byte ptr [rip + 0x191fe84], 1 # 0x181ce5019
00000001803C5195 mov       rax, qword ptr [rip + 0x17d7f6c] # 0x181b9d108
00000001803C519C cmp       dword ptr [rax + 0xe0], 0
00000001803C51A3 jne       0x1803c51b4
00000001803C51A5 mov       rcx, rax
00000001803C51A8 call      0x1802501e0
00000001803C51AD mov       rax, qword ptr [rip + 0x17d7f54] # 0x181b9d108
00000001803C51B4 mov       rax, qword ptr [rax + 0xb8]
00000001803C51BB mov       qword ptr [rsp + 0x60], rbx
00000001803C51C0 mov       qword ptr [rsp + 0x68], rsi
00000001803C51C5 movaps    xmmword ptr [rsp + 0x40], xmm6
00000001803C51CA mov       rcx, qword ptr [rax + 0x18]
00000001803C51CE movaps    xmmword ptr [rsp + 0x30], xmm7
00000001803C51D3 test      rcx, rcx
00000001803C51D6 je        0x1803c5366
00000001803C51DC inc       dword ptr [rcx + 0x1c]
00000001803C51DF xor       ebx, ebx
00000001803C51E1 mov       dword ptr [rcx + 0x18], ebx
00000001803C51E4 mov       rax, qword ptr [rip + 0x17d7f1d] # 0x181b9d108
00000001803C51EB mov       rcx, qword ptr [rax + 0xb8]
00000001803C51F2 mov       rcx, qword ptr [rcx + 0x18]
00000001803C51F6 test      rcx, rcx
00000001803C51F9 je        0x1803c5366
00000001803C51FF mov       rdx, qword ptr [rip + 0x17fc2e2] # 0x181bc14e8
00000001803C5206 call      0x18065baa0
00000001803C520B test      rdi, rdi
00000001803C520E je        0x1803c5366
00000001803C5214 cmp       eax, dword ptr [rdi + 0x18]
00000001803C5217 jge       0x1803c525c
00000001803C5219 mov       rax, qword ptr [rip + 0x17d7ee8] # 0x181b9d108
00000001803C5220 cmp       dword ptr [rax + 0xe0], ebx
00000001803C5226 jne       0x1803c5237
00000001803C5228 mov       rcx, rax
00000001803C522B call      0x1802501e0
00000001803C5230 mov       rax, qword ptr [rip + 0x17d7ed1] # 0x181b9d108
00000001803C5237 mov       rax, qword ptr [rax + 0xb8]
00000001803C523E mov       rcx, qword ptr [rax + 0x18]
00000001803C5242 test      rcx, rcx
00000001803C5245 je        0x1803c5366
00000001803C524B mov       edx, dword ptr [rdi + 0x18]
00000001803C524E mov       r8, qword ptr [rip + 0x17fc39b] # 0x181bc15f0
00000001803C5255 add       edx, edx
00000001803C5257 call      0x180805920
00000001803C525C mov       esi, dword ptr [rdi + 0x18]
00000001803C525F test      esi, esi
00000001803C5261 jle       0x1803c534c
00000001803C5267 cmp       ebx, dword ptr [rdi + 0x18]
00000001803C526A jae       0x1803c536c
00000001803C5270 movsxd    rax, ebx
00000001803C5273 movss     xmm6, dword ptr [rdi + rax*8 + 0x20]
00000001803C5279 movss     xmm7, dword ptr [rdi + rax*8 + 0x24]
00000001803C527F mov       rax, qword ptr [rip + 0x17d7e82] # 0x181b9d108
00000001803C5286 cmp       dword ptr [rax + 0xe0], 0
00000001803C528D jne       0x1803c529e
00000001803C528F mov       rcx, rax
00000001803C5292 call      0x1802501e0
00000001803C5297 mov       rax, qword ptr [rip + 0x17d7e6a] # 0x181b9d108
00000001803C529E mov       rax, qword ptr [rax + 0xb8]
00000001803C52A5 movss     dword ptr [rax + 8], xmm6
00000001803C52AA mov       rax, qword ptr [rip + 0x17d7e57] # 0x181b9d108
00000001803C52B1 mov       rcx, qword ptr [rax + 0xb8]
00000001803C52B8 movss     dword ptr [rcx + 0xc], xmm7
00000001803C52BD mov       rcx, qword ptr [rip + 0x17d7e44] # 0x181b9d108
00000001803C52C4 mov       rax, qword ptr [rcx + 0xb8]
00000001803C52CB mov       r9, qword ptr [rax + 0x18]
00000001803C52CF test      r9, r9
00000001803C52D2 je        0x1803c5366
00000001803C52D8 mov       r10, qword ptr [rip + 0x17fc0a9] # 0x181bc1388
00000001803C52DF movsd     xmm0, qword ptr [rax + 8]
00000001803C52E4 mov       r8d, dword ptr [rax + 0x10]
00000001803C52E8 inc       dword ptr [r9 + 0x1c]
00000001803C52EC mov       rdx, qword ptr [r9 + 0x10]
00000001803C52F0 movsxd    rcx, dword ptr [r9 + 0x18]
00000001803C52F4 test      rdx, rdx
00000001803C52F7 je        0x1803c5366
00000001803C52F9 cmp       ecx, dword ptr [rdx + 0x18]
00000001803C52FC jb        0x1803c5327
00000001803C52FE mov       rax, qword ptr [r10 + 0x20]
00000001803C5302 lea       rdx, [rsp + 0x20]
00000001803C5307 mov       dword ptr [rsp + 0x28], r8d
00000001803C530C mov       rcx, r9
00000001803C530F movsd     qword ptr [rsp + 0x20], xmm0
00000001803C5315 mov       r8, qword ptr [rax + 0xc0]
00000001803C531C mov       r8, qword ptr [r8 + 0x70]
00000001803C5320 call      0x1808164b0
00000001803C5325 jmp       0x1803c5342
00000001803C5327 lea       eax, [rcx + 1]
00000001803C532A mov       dword ptr [r9 + 0x18], eax
00000001803C532E cmp       ecx, dword ptr [rdx + 0x18]
00000001803C5331 jae       0x1803c536c
00000001803C5333 lea       rcx, [rcx + rcx*2]
00000001803C5337 movsd     qword ptr [rdx + rcx*4 + 0x20], xmm0
00000001803C533D mov       dword ptr [rdx + rcx*4 + 0x28], r8d
00000001803C5342 inc       ebx
00000001803C5344 cmp       ebx, esi
00000001803C5346 jl        0x1803c5267
00000001803C534C movaps    xmm7, xmmword ptr [rsp + 0x30]
00000001803C5351 movaps    xmm6, xmmword ptr [rsp + 0x40]
00000001803C5356 mov       rsi, qword ptr [rsp + 0x68]
00000001803C535B mov       rbx, qword ptr [rsp + 0x60]
00000001803C5360 add       rsp, 0x50
00000001803C5364 pop       rdi
00000001803C5365 ret       
00000001803C5366 call      0x180250150
00000001803C536B int3      
00000001803C536C call      0x180250140
00000001803C5371 int3      