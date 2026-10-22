000000018031DE90 mov       qword ptr [rsp + 0x10], rdx
000000018031DE95 mov       qword ptr [rsp + 8], rcx
000000018031DE9A push      rbx
000000018031DE9B push      rsi
000000018031DE9C push      rdi
000000018031DE9D push      r14
000000018031DE9F push      r15
000000018031DEA1 sub       rsp, 0x60
000000018031DEA5 mov       rsi, rdx
000000018031DEA8 mov       rdi, rcx
000000018031DEAB cmp       byte ptr [rip + 0x19c6d25], 0 # 0x181ce4bd7
000000018031DEB2 jne       0x18031df1b
000000018031DEB4 lea       rcx, [rip + 0x1887035] # 0x181ba4ef0
000000018031DEBB call      0x18024fef0
000000018031DEC0 lea       rcx, [rip + 0x1887389] # 0x181ba5250
000000018031DEC7 call      0x18024fef0
000000018031DECC lea       rcx, [rip + 0x1899b1d] # 0x181bb79f0
000000018031DED3 call      0x18024fef0
000000018031DED8 lea       rcx, [rip + 0x1892621] # 0x181bb0500
000000018031DEDF call      0x18024fef0
000000018031DEE4 lea       rcx, [rip + 0x18a177d] # 0x181bbf668
000000018031DEEB call      0x18024fef0
000000018031DEF0 lea       rcx, [rip + 0x1888831] # 0x181ba6728
000000018031DEF7 call      0x18024fef0
000000018031DEFC lea       rcx, [rip + 0x189458d] # 0x181bb2490
000000018031DF03 call      0x18024fef0
000000018031DF08 lea       rcx, [rip + 0x18887b9] # 0x181ba66c8
000000018031DF0F call      0x18024fef0
000000018031DF14 mov       byte ptr [rip + 0x19c6cbc], 1 # 0x181ce4bd7
000000018031DF1B xor       r15d, r15d
000000018031DF1E mov       qword ptr [rsp + 0x20], r15
000000018031DF23 test      rsi, rsi
000000018031DF26 je        0x18031e2d2
000000018031DF2C mov       rax, qword ptr [rdi + 0x28]
000000018031DF30 test      rax, rax
000000018031DF33 je        0x18031e2d2
000000018031DF39 mov       rcx, qword ptr [rax + 0x108]
000000018031DF40 mov       rax, qword ptr [rsi + 0x40]
000000018031DF44 test      rax, rax
000000018031DF47 je        0x18031e2d2
000000018031DF4D test      rcx, rcx
000000018031DF50 je        0x18031e2d2
000000018031DF56 xor       r8d, r8d
000000018031DF59 mov       edx, dword ptr [rax + 0x1c]
000000018031DF5C call      0x180312c00
000000018031DF61 mov       r14, rax
000000018031DF64 mov       qword ptr [rsp + 0x28], rax
000000018031DF69 mov       rcx, qword ptr [rip + 0x1892590] # 0x181bb0500
000000018031DF70 cmp       dword ptr [rcx + 0xe0], r15d
000000018031DF77 jne       0x18031df7e
000000018031DF79 call      0x1802501e0
000000018031DF7E xor       r8d, r8d
000000018031DF81 xor       edx, edx
000000018031DF83 mov       rcx, r14
000000018031DF86 call      0x18131f760
000000018031DF8B test      al, al
000000018031DF8D jne       0x18031e2b6
000000018031DF93 mov       rcx, qword ptr [rip + 0x1892566] # 0x181bb0500
000000018031DF9A cmp       dword ptr [rcx + 0xe0], r15d
000000018031DFA1 jne       0x18031dfa8
000000018031DFA3 call      0x1802501e0
000000018031DFA8 mov       rdx, qword ptr [rip + 0x1899a41] # 0x181bb79f0
000000018031DFAF mov       rcx, r14
000000018031DFB2 call      0x1804a0690
000000018031DFB7 mov       qword ptr [rdi + 0x80], rax
000000018031DFBE lea       rcx, [rdi + 0x80]
000000018031DFC5 mov       rdx, rax
000000018031DFC8 call      0x18024f360
000000018031DFCD mov       rcx, qword ptr [rdi + 0x80]
000000018031DFD4 test      rcx, rcx
000000018031DFD7 je        0x18031e2d2
000000018031DFDD xor       edx, edx
000000018031DFDF call      0x181304510
000000018031DFE4 test      rax, rax
000000018031DFE7 je        0x18031e2d2
000000018031DFED xor       edx, edx
000000018031DFEF mov       rcx, rax
000000018031DFF2 call      0x18132dc20
000000018031DFF7 mov       qword ptr [rsp + 0xa8], rax
000000018031DFFF lea       rax, [rsp + 0xa8]
000000018031E007 mov       qword ptr [rsp + 0x38], rax
000000018031E00C lea       rax, [rsp + 0x20]
000000018031E011 mov       qword ptr [rsp + 0x40], rax
000000018031E016 mov       qword ptr [rsp + 0x48], r15
000000018031E01B movups    xmm0, xmmword ptr [rsp + 0x38]
000000018031E020 movups    xmmword ptr [rsp + 0x50], xmm0
000000018031E025 mov       r8, qword ptr [rsp + 0xa8]
000000018031E02D test      r8, r8
000000018031E030 je        0x18031e2ee
000000018031E036 xor       ecx, ecx
000000018031E038 mov       rdx, qword ptr [rip + 0x1887211] # 0x181ba5250
000000018031E03F call      0x180002cd0
000000018031E044 test      al, al
000000018031E046 je        0x18031e136
000000018031E04C mov       rbx, qword ptr [rsp + 0xa8]
000000018031E054 test      rbx, rbx
000000018031E057 je        0x18031e2e9
000000018031E05D mov       r9, qword ptr [rip + 0x18871ec] # 0x181ba5250
000000018031E064 mov       r10, qword ptr [rbx]
000000018031E067 movzx     ecx, r15w
000000018031E06B movzx     edx, word ptr [r10 + 0x12e]
000000018031E073 cmp       r15w, dx
000000018031E077 jae       0x18031e094
000000018031E079 mov       r8, qword ptr [r10 + 0xb0]
000000018031E080 movzx     eax, cx
000000018031E083 add       rax, rax
000000018031E086 cmp       qword ptr [r8 + rax*8], r9
000000018031E08A je        0x18031e0c9
000000018031E08C inc       cx
000000018031E08F cmp       cx, dx
000000018031E092 jb        0x18031e080
000000018031E094 mov       r8d, 1
000000018031E09A mov       rdx, r9
000000018031E09D mov       rcx, rbx
000000018031E0A0 call      0x180258380
000000018031E0A5 mov       rdx, rax
000000018031E0A8 mov       rax, qword ptr [rdx]
000000018031E0AB mov       rdx, qword ptr [rdx + 8]
000000018031E0AF mov       rcx, rbx
000000018031E0B2 call      rax
000000018031E0B4 mov       r8, rax
000000018031E0B7 mov       rcx, qword ptr [rip + 0x18a15aa] # 0x181bbf668
000000018031E0BE test      rax, rax
000000018031E0C1 je        0x18031e2d8
000000018031E0C7 jmp       0x18031e0e9
000000018031E0C9 movzx     ecx, cx
000000018031E0CC add       rcx, rcx
000000018031E0CF mov       ecx, dword ptr [r8 + rcx*8 + 8]
000000018031E0D4 inc       ecx
000000018031E0D6 movsxd    rdx, ecx
000000018031E0D9 shl       rdx, 4
000000018031E0DD add       rdx, 0x138
000000018031E0E4 add       rdx, r10
000000018031E0E7 jmp       0x18031e0a8
000000018031E0E9 mov       r9, qword ptr [rax]
000000018031E0EC movzx     eax, byte ptr [rcx + 0x130]
000000018031E0F3 cmp       byte ptr [r9 + 0x130], al
000000018031E0FA jb        0x18031e2de
000000018031E100 movzx     edx, al
000000018031E103 mov       rax, qword ptr [r9 + 0xc8]
000000018031E10A cmp       qword ptr [rax + rdx*8 - 8], rcx
000000018031E10F jne       0x18031e2de
000000018031E115 xor       edx, edx
000000018031E117 mov       rcx, r8
000000018031E11A call      0x18131f6f0
000000018031E11F xor       r8d, r8d
000000018031E122 mov       rdx, qword ptr [rip + 0x1894367] # 0x181bb2490
000000018031E129 mov       rcx, rax
000000018031E12C call      0x180b76170
000000018031E131 jmp       0x18031e025
000000018031E136 mov       rax, qword ptr [rsp + 0x38]
000000018031E13B mov       rdx, qword ptr [rip + 0x1886dae] # 0x181ba4ef0
000000018031E142 mov       rcx, qword ptr [rax]
000000018031E145 call      0x18024f380
000000018031E14A mov       r8, rax
000000018031E14D movups    xmm0, xmmword ptr [rsp + 0x38]
000000018031E152 psrldq    xmm0, 8
000000018031E157 movq      rax, xmm0
000000018031E15C mov       qword ptr [rax], r8
000000018031E15F test      r8, r8
000000018031E162 je        0x18031e172
000000018031E164 xor       ecx, ecx
000000018031E166 mov       rdx, qword ptr [rip + 0x1886d83] # 0x181ba4ef0
000000018031E16D call      0x180002d90
000000018031E172 jmp       0x18031e1a1
000000018031E174 lea       rcx, [rsp + 0x50]
000000018031E179 call      0x1802fb000
000000018031E17E mov       rcx, qword ptr [rsp + 0x48]
000000018031E183 test      rcx, rcx
000000018031E186 jne       0x18031e2f4
000000018031E18C mov       rsi, qword ptr [rsp + 0x98]
000000018031E194 mov       rdi, qword ptr [rsp + 0x90]
000000018031E19C mov       r14, qword ptr [rsp + 0x28]
000000018031E1A1 mov       rcx, qword ptr [rdi + 0x80]
000000018031E1A8 mov       rdx, qword ptr [rdi + 0x28]
000000018031E1AC test      rcx, rcx
000000018031E1AF je        0x18031e2d2
000000018031E1B5 mov       qword ptr [rcx + 0x28], rdx
000000018031E1B9 add       rcx, 0x28
000000018031E1BD call      0x18024f360
000000018031E1C2 mov       rcx, qword ptr [rdi + 0x80]
000000018031E1C9 test      rcx, rcx
000000018031E1CC je        0x18031e2d2
000000018031E1D2 xor       r9d, r9d
000000018031E1D5 mov       r8, rdi
000000018031E1D8 mov       rdx, rsi
000000018031E1DB call      0x18034a450
000000018031E1E0 mov       rcx, qword ptr [rdi + 0x80]
000000018031E1E7 test      rcx, rcx
000000018031E1EA je        0x18031e2d2
000000018031E1F0 xor       edx, edx
000000018031E1F2 call      0x180349940
000000018031E1F7 mov       dword ptr [rdi + 0x40], 1
000000018031E1FE mov       rax, qword ptr [rdi + 0x80]
000000018031E205 test      rax, rax
000000018031E208 je        0x18031e2d2
000000018031E20E mov       eax, dword ptr [rax + 0x54]
000000018031E211 mov       dword ptr [rdi + 0x9c], eax
000000018031E217 mov       qword ptr [rdi + 0x68], rsi
000000018031E21B lea       rcx, [rdi + 0x68]
000000018031E21F mov       rdx, rsi
000000018031E222 call      0x18024f360
000000018031E227 mov       rcx, qword ptr [rip + 0x18922d2] # 0x181bb0500
000000018031E22E cmp       dword ptr [rcx + 0xe0], 0
000000018031E235 jne       0x18031e23c
000000018031E237 call      0x1802501e0
000000018031E23C mov       rdx, qword ptr [rip + 0x18997ad] # 0x181bb79f0
000000018031E243 mov       rcx, r14
000000018031E246 call      0x1804a0690
000000018031E24B mov       rbx, rax
000000018031E24E mov       qword ptr [rdi + 0x88], rax
000000018031E255 lea       rcx, [rdi + 0x88]
000000018031E25C mov       rdx, rax
000000018031E25F call      0x18024f360
000000018031E264 xor       r8d, r8d
000000018031E267 xor       edx, edx
000000018031E269 mov       rcx, rbx
000000018031E26C call      0x18131f760
000000018031E271 test      al, al
000000018031E273 jne       0x18031e29a
000000018031E275 mov       rcx, qword ptr [rdi + 0x88]
000000018031E27C test      rcx, rcx
000000018031E27F je        0x18031e2d2
000000018031E281 xor       r8d, r8d
000000018031E284 mov       rdx, rdi
000000018031E287 call      0x18034a620
000000018031E28C mov       al, 1
000000018031E28E add       rsp, 0x60
000000018031E292 pop       r15
000000018031E294 pop       r14
000000018031E296 pop       rdi
000000018031E297 pop       rsi
000000018031E298 pop       rbx
000000018031E299 ret       
000000018031E29A xor       edx, edx
000000018031E29C mov       rcx, qword ptr [rip + 0x1888485] # 0x181ba6728
000000018031E2A3 call      0x18131e440
000000018031E2A8 mov       al, 1
000000018031E2AA add       rsp, 0x60
000000018031E2AE pop       r15
000000018031E2B0 pop       r14
000000018031E2B2 pop       rdi
000000018031E2B3 pop       rsi
000000018031E2B4 pop       rbx
000000018031E2B5 ret       
000000018031E2B6 xor       edx, edx
000000018031E2B8 mov       rcx, qword ptr [rip + 0x1888409] # 0x181ba66c8
000000018031E2BF call      0x18131e440
000000018031E2C4 xor       al, al
000000018031E2C6 add       rsp, 0x60
000000018031E2CA pop       r15
000000018031E2CC pop       r14
000000018031E2CE pop       rdi
000000018031E2CF pop       rsi
000000018031E2D0 pop       rbx
000000018031E2D1 ret       
000000018031E2D2 call      0x180250150
000000018031E2D7 nop       
000000018031E2D8 call      0x180250150
000000018031E2DD nop       
000000018031E2DE mov       rdx, rcx
000000018031E2E1 mov       rcx, r8
000000018031E2E4 call      0x18024f3a0
000000018031E2E9 call      0x180250150
000000018031E2EE call      0x180250150
000000018031E2F3 nop       
000000018031E2F4 call      0x180200e60
000000018031E2F9 int3      
000000018031E2FA int3      
000000018031E2FB int3      
000000018031E2FC int3      
000000018031E2FD int3      
000000018031E2FE int3      
000000018031E2FF int3      