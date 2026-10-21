000000018038F0C0 mov       rax, rsp
000000018038F0C3 mov       qword ptr [rax + 8], rcx
000000018038F0C7 push      rbx
000000018038F0C8 push      rsi
000000018038F0C9 push      rdi
000000018038F0CA push      r12
000000018038F0CC push      r13
000000018038F0CE push      r14
000000018038F0D0 push      r15
000000018038F0D2 sub       rsp, 0xa0
000000018038F0D9 movaps    xmmword ptr [rax - 0x48], xmm6
000000018038F0DD movaps    xmmword ptr [rax - 0x58], xmm7
000000018038F0E1 mov       r13, rcx
000000018038F0E4 mov       r12, rcx
000000018038F0E7 mov       qword ptr [rsp + 0xf8], rcx
000000018038F0EF cmp       byte ptr [rip + 0x1955d55], 0 # 0x181ce4e4b
000000018038F0F6 jne       0x18038f153
000000018038F0F8 lea       rcx, [rip + 0x183a2b9] # 0x181bc93b8
000000018038F0FF call      0x18024fef0
000000018038F104 lea       rcx, [rip + 0x183a305] # 0x181bc9410
000000018038F10B call      0x18024fef0
000000018038F110 lea       rcx, [rip + 0x183a349] # 0x181bc9460
000000018038F117 call      0x18024fef0
000000018038F11C lea       rcx, [rip + 0x1812ef5] # 0x181ba2018
000000018038F123 call      0x18024fef0
000000018038F128 lea       rcx, [rip + 0x182e4f1] # 0x181bbd620
000000018038F12F call      0x18024fef0
000000018038F134 lea       rcx, [rip + 0x181dabd] # 0x181bacbf8
000000018038F13B call      0x18024fef0
000000018038F140 lea       rcx, [rip + 0x1811609] # 0x181ba0750
000000018038F147 call      0x18024fef0
000000018038F14C mov       byte ptr [rip + 0x1955cf8], 1 # 0x181ce4e4b
000000018038F153 mov       r15b, 1
000000018038F156 mov       byte ptr [rsp + 0xf0], r15b
000000018038F15E mov       rdx, qword ptr [r13 + 0x168]
000000018038F165 test      rdx, rdx
000000018038F168 je        0x18038f4be
000000018038F16E mov       r8, qword ptr [rip + 0x182e4ab] # 0x181bbd620
000000018038F175 lea       rcx, [rsp + 0x40]
000000018038F17A call      0x1808197f0
000000018038F17F movups    xmm0, xmmword ptr [rsp + 0x40]
000000018038F184 movups    xmmword ptr [rsp + 0x60], xmm0
000000018038F189 movsd     xmm1, qword ptr [rsp + 0x50]
000000018038F18F movsd     qword ptr [rsp + 0x70], xmm1
000000018038F195 mov       qword ptr [rsp + 0x28], 0
000000018038F19E lea       rbx, [rsp + 0x60]
000000018038F1A3 mov       qword ptr [rsp + 0x30], rbx
000000018038F1A8 movdqa    xmm7, xmmword ptr [rip + 0x1218da0] # 0x1815a7f50
000000018038F1B0 movdqa    xmm6, xmmword ptr [rip + 0x1218d68] # 0x1815a7f20
000000018038F1B8 nop       dword ptr [rax + rax]
000000018038F1C0 mov       rdx, qword ptr [rip + 0x183a249] # 0x181bc9410
000000018038F1C7 lea       rcx, [rsp + 0x60]
000000018038F1CC call      0x180669280
000000018038F1D1 test      al, al
000000018038F1D3 je        0x18038f324
000000018038F1D9 mov       rdi, qword ptr [rsp + 0x70]
000000018038F1DE test      rdi, rdi
000000018038F1E1 je        0x18038f4f2
000000018038F1E7 mov       rcx, qword ptr [rdi + 0x58]
000000018038F1EB test      rcx, rcx
000000018038F1EE je        0x18038f4ed
000000018038F1F4 xor       edx, edx
000000018038F1F6 call      0x1813044d0
000000018038F1FB test      rax, rax
000000018038F1FE je        0x18038f4e8
000000018038F204 xor       r8d, r8d
000000018038F207 mov       dl, 1
000000018038F209 mov       rcx, rax
000000018038F20C call      0x18131bad0
000000018038F211 test      rdi, rdi
000000018038F214 je        0x18038f4e3
000000018038F21A mov       rsi, qword ptr [rdi + 0x60]
000000018038F21E lea       rcx, [rdi + 0x28]
000000018038F222 xor       edx, edx
000000018038F224 call      0x180ca3820
000000018038F229 test      rsi, rsi
000000018038F22C je        0x18038f4de
000000018038F232 xor       r9d, r9d
000000018038F235 mov       r8b, 1
000000018038F238 mov       rdx, rax
000000018038F23B mov       rcx, rsi
000000018038F23E call      0x1811af070
000000018038F243 mov       rcx, qword ptr [rdi + 0x58]
000000018038F247 test      rcx, rcx
000000018038F24A je        0x18038f4d9
000000018038F250 xor       edx, edx
000000018038F252 call      0x1813cfaa0
000000018038F257 mov       rsi, rax
000000018038F25A mov       r14d, dword ptr [rdi + 0x28]
000000018038F25E mov       rcx, qword ptr [rip + 0x181d993] # 0x181bacbf8
000000018038F265 cmp       dword ptr [rcx + 0xe0], 0
000000018038F26C jne       0x18038f273
000000018038F26E call      0x1802501e0
000000018038F273 movd      xmm0, r14d
000000018038F278 cvtdq2pd  xmm0, xmm0
000000018038F27C call      0x1802cdc30
000000018038F281 test      rsi, rsi
000000018038F284 je        0x18038f4d4
000000018038F28A cvttsd2si eax, xmm0
000000018038F28E imul      edx, eax, 0xe
000000018038F291 add       edx, 0x27
000000018038F294 movd      xmm2, edx
000000018038F298 cvtdq2ps  xmm2, xmm2
000000018038F29B xor       r9d, r9d
000000018038F29E xor       edx, edx
000000018038F2A0 mov       rcx, rsi
000000018038F2A3 call      0x181324090
000000018038F2A8 mov       rax, qword ptr [rdi + 0x20]
000000018038F2AC test      rax, rax
000000018038F2AF je        0x18038f4cf
000000018038F2B5 mov       eax, dword ptr [rax + 0x20]
000000018038F2B8 mov       rcx, qword ptr [rdi + 0x58]
000000018038F2BC cmp       eax, dword ptr [rdi + 0x28]
000000018038F2BF jge       0x18038f2f8
000000018038F2C1 test      rcx, rcx
000000018038F2C4 je        0x18038f4c4
000000018038F2CA mov       r8, qword ptr [rcx]
000000018038F2CD mov       rax, qword ptr [r8 + 0x2a8]
000000018038F2D4 movdqa    xmmword ptr [rsp + 0x40], xmm6
000000018038F2DA mov       r8, qword ptr [r8 + 0x2b0]
000000018038F2E1 lea       rdx, [rsp + 0x40]
000000018038F2E6 call      rax
000000018038F2E8 xor       r15b, r15b
000000018038F2EB mov       byte ptr [rsp + 0xf0], r15b
000000018038F2F3 jmp       0x18038f1c0
000000018038F2F8 test      rcx, rcx
000000018038F2FB je        0x18038f4ca
000000018038F301 mov       r8, qword ptr [rcx]
000000018038F304 mov       rax, qword ptr [r8 + 0x2a8]
000000018038F30B movdqa    xmmword ptr [rsp + 0x40], xmm7
000000018038F311 mov       r8, qword ptr [r8 + 0x2b0]
000000018038F318 lea       rdx, [rsp + 0x40]
000000018038F31D call      rax
000000018038F31F jmp       0x18038f1c0
000000018038F324 mov       rdx, qword ptr [rip + 0x183a08d] # 0x181bc93b8
000000018038F32B mov       rcx, rbx
000000018038F32E call      0x180302170
000000018038F333 jmp       0x18038f36d
000000018038F335 mov       rdx, qword ptr [rip + 0x183a07c] # 0x181bc93b8
000000018038F33C mov       rcx, qword ptr [rsp + 0x30]
000000018038F341 call      0x180302170
000000018038F346 mov       rcx, qword ptr [rsp + 0x28]
000000018038F34B test      rcx, rcx
000000018038F34E jne       0x18038f4f8
000000018038F354 mov       r13, qword ptr [rsp + 0xe0]
000000018038F35C movzx     r15d, byte ptr [rsp + 0xf0]
000000018038F365 mov       r12, qword ptr [rsp + 0xf8]
000000018038F36D mov       rax, qword ptr [rip + 0x1812ca4] # 0x181ba2018
000000018038F374 cmp       dword ptr [rax + 0xe0], 0
000000018038F37B jne       0x18038f38c
000000018038F37D mov       rcx, rax
000000018038F380 call      0x1802501e0
000000018038F385 mov       rax, qword ptr [rip + 0x1812c8c] # 0x181ba2018
000000018038F38C mov       rax, qword ptr [rax + 0xb8]
000000018038F393 mov       rax, qword ptr [rax]
000000018038F396 test      rax, rax
000000018038F399 je        0x18038f4be
000000018038F39F mov       rax, qword ptr [rax + 0x28]
000000018038F3A3 test      rax, rax
000000018038F3A6 je        0x18038f4be
000000018038F3AC mov       rax, qword ptr [rax + 0x10]
000000018038F3B0 test      rax, rax
000000018038F3B3 je        0x18038f4be
000000018038F3B9 mov       eax, dword ptr [rax + 0x70]
000000018038F3BC mov       dword ptr [r13 + 0x178], eax
000000018038F3C3 mov       rcx, qword ptr [r12 + 0x190]
000000018038F3CB cmp       dword ptr [r13 + 0x188], eax
000000018038F3D2 jg        0x18038f402
000000018038F3D4 test      rcx, rcx
000000018038F3D7 je        0x18038f4be
000000018038F3DD movdqa    xmm0, xmmword ptr [rip + 0x1218b2b] # 0x1815a7f10
000000018038F3E5 mov       rax, qword ptr [rcx]
000000018038F3E8 movdqa    xmmword ptr [rsp + 0x40], xmm0
000000018038F3EE mov       r8, qword ptr [rax + 0x2b0]
000000018038F3F5 lea       rdx, [rsp + 0x40]
000000018038F3FA call      qword ptr [rax + 0x2a8]
000000018038F400 jmp       0x18038f431
000000018038F402 test      rcx, rcx
000000018038F405 je        0x18038f4be
000000018038F40B movdqa    xmm0, xmmword ptr [rip + 0x1218add] # 0x1815a7ef0
000000018038F413 mov       rax, qword ptr [rcx]
000000018038F416 movdqa    xmmword ptr [rsp + 0x40], xmm0
000000018038F41C mov       r8, qword ptr [rax + 0x2b0]
000000018038F423 lea       rdx, [rsp + 0x40]
000000018038F428 call      qword ptr [rax + 0x2a8]
000000018038F42E xor       r15b, r15b
000000018038F431 mov       rbx, qword ptr [r13 + 0x180]
000000018038F438 lea       rcx, [r13 + 0x178]
000000018038F43F xor       r8d, r8d
000000018038F442 mov       rdx, qword ptr [rip + 0x1811307] # 0x181ba0750
000000018038F449 call      0x180ca3930
000000018038F44E test      rbx, rbx
000000018038F451 je        0x18038f4be
000000018038F453 xor       r9d, r9d
000000018038F456 mov       r8b, 1
000000018038F459 mov       rdx, rax
000000018038F45C mov       rcx, rbx
000000018038F45F call      0x1811af070
000000018038F464 mov       rbx, qword ptr [r13 + 0x190]
000000018038F46B lea       rcx, [r13 + 0x188]
000000018038F472 xor       r8d, r8d
000000018038F475 mov       rdx, qword ptr [rip + 0x18112d4] # 0x181ba0750
000000018038F47C call      0x180ca3930
000000018038F481 test      rbx, rbx
000000018038F484 je        0x18038f4be
000000018038F486 xor       r9d, r9d
000000018038F489 mov       r8b, 1
000000018038F48C mov       rdx, rax
000000018038F48F mov       rcx, rbx
000000018038F492 call      0x1811af070
000000018038F497 movzx     eax, r15b
000000018038F49B movaps    xmm6, xmmword ptr [rsp + 0x90]
000000018038F4A3 movaps    xmm7, xmmword ptr [rsp + 0x80]
000000018038F4AB add       rsp, 0xa0
000000018038F4B2 pop       r15
000000018038F4B4 pop       r14
000000018038F4B6 pop       r13
000000018038F4B8 pop       r12
000000018038F4BA pop       rdi
000000018038F4BB pop       rsi
000000018038F4BC pop       rbx
000000018038F4BD ret       
000000018038F4BE call      0x180250150
000000018038F4C3 nop       
000000018038F4C4 call      0x180250150
000000018038F4C9 nop       
000000018038F4CA call      0x180250150
000000018038F4CF call      0x180250150
000000018038F4D4 call      0x180250150
000000018038F4D9 call      0x180250150
000000018038F4DE call      0x180250150
000000018038F4E3 call      0x180250150
000000018038F4E8 call      0x180250150
000000018038F4ED call      0x180250150
000000018038F4F2 call      0x180250150
000000018038F4F7 nop       
000000018038F4F8 call      0x180200e60
000000018038F4FD int3      
000000018038F4FE int3      
000000018038F4FF int3      