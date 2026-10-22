00000001803AC060 mov       rax, rsp
00000001803AC063 mov       qword ptr [rax + 0x18], r8
00000001803AC067 mov       qword ptr [rax + 0x10], rdx
00000001803AC06B mov       qword ptr [rax + 8], rcx
00000001803AC06F push      rbx
00000001803AC070 push      rsi
00000001803AC071 push      rdi
00000001803AC072 push      r14
00000001803AC074 push      r15
00000001803AC076 sub       rsp, 0xa0
00000001803AC07D movaps    xmmword ptr [rax - 0x38], xmm6
00000001803AC081 mov       r15, r8
00000001803AC084 mov       r14, rdx
00000001803AC087 cmp       byte ptr [rip + 0x1938e8d], 0 # 0x181ce4f1b
00000001803AC08E jne       0x1803ac12b
00000001803AC094 lea       rcx, [rip + 0x18198e5] # 0x181bc5980
00000001803AC09B call      0x18024fef0
00000001803AC0A0 lea       rcx, [rip + 0x18154d1] # 0x181bc1578
00000001803AC0A7 call      0x18024fef0
00000001803AC0AC lea       rcx, [rip + 0x1819925] # 0x181bc59d8
00000001803AC0B3 call      0x18024fef0
00000001803AC0B8 lea       rcx, [rip + 0x1815511] # 0x181bc15d0
00000001803AC0BF call      0x18024fef0
00000001803AC0C4 lea       rcx, [rip + 0x181555d] # 0x181bc1628
00000001803AC0CB call      0x18024fef0
00000001803AC0D0 lea       rcx, [rip + 0x1819959] # 0x181bc5a30
00000001803AC0D7 call      0x18024fef0
00000001803AC0DC lea       rcx, [rip + 0x17f5f35] # 0x181ba2018
00000001803AC0E3 call      0x18024fef0
00000001803AC0E8 lea       rcx, [rip + 0x1811061] # 0x181bbd150
00000001803AC0EF call      0x18024fef0
00000001803AC0F4 lea       rcx, [rip + 0x17f8c5d] # 0x181ba4d58
00000001803AC0FB call      0x18024fef0
00000001803AC100 lea       rcx, [rip + 0x1805821] # 0x181bb1928
00000001803AC107 call      0x18024fef0
00000001803AC10C lea       rcx, [rip + 0x1810fe5] # 0x181bbd0f8
00000001803AC113 call      0x18024fef0
00000001803AC118 lea       rcx, [rip + 0x1814d39] # 0x181bc0e58
00000001803AC11F call      0x18024fef0
00000001803AC124 mov       byte ptr [rip + 0x1938df0], 1 # 0x181ce4f1b
00000001803AC12B xorps     xmm0, xmm0
00000001803AC12E xor       eax, eax
00000001803AC130 movups    xmmword ptr [rsp + 0x48], xmm0
00000001803AC135 mov       qword ptr [rsp + 0x58], rax
00000001803AC13A xorps     xmm6, xmm6
00000001803AC13D movss     dword ptr [rsp + 0x38], xmm6
00000001803AC143 mov       rcx, qword ptr [rip + 0x1814d0e] # 0x181bc0e58
00000001803AC14A call      0x180250100
00000001803AC14F mov       rsi, rax
00000001803AC152 mov       rdx, qword ptr [rip + 0x1810f9f] # 0x181bbd0f8
00000001803AC159 mov       rcx, rax
00000001803AC15C call      0x1808053d0
00000001803AC161 mov       qword ptr [rsp + 0x40], rsi
00000001803AC166 test      r14, r14
00000001803AC169 je        0x1803ac420
00000001803AC16F mov       rdx, qword ptr [r14 + 0x48]
00000001803AC173 test      rdx, rdx
00000001803AC176 je        0x1803ac420
00000001803AC17C mov       r8, qword ptr [rip + 0x18057a5] # 0x181bb1928
00000001803AC183 lea       rcx, [rsp + 0x20]
00000001803AC188 call      0x1808197f0
00000001803AC18D movups    xmm0, xmmword ptr [rsp + 0x20]
00000001803AC192 movups    xmmword ptr [rsp + 0x60], xmm0
00000001803AC197 movsd     xmm1, qword ptr [rsp + 0x30]
00000001803AC19D movsd     qword ptr [rsp + 0x70], xmm1
00000001803AC1A3 mov       qword ptr [rsp + 0x20], 0
00000001803AC1AC lea       rbx, [rsp + 0x60]
00000001803AC1B1 mov       qword ptr [rsp + 0x28], rbx
00000001803AC1B6 nop       word ptr [rax + rax]
00000001803AC1C0 mov       rdx, qword ptr [rip + 0x1819811] # 0x181bc59d8
00000001803AC1C7 lea       rcx, [rsp + 0x60]
00000001803AC1CC call      0x180669280
00000001803AC1D1 test      al, al
00000001803AC1D3 je        0x1803ac2a1
00000001803AC1D9 mov       rdi, qword ptr [rsp + 0x70]
00000001803AC1DE test      rdi, rdi
00000001803AC1E1 je        0x1803ac440
00000001803AC1E7 cmp       byte ptr [rdi + 0x24], 0
00000001803AC1EB je        0x1803ac261
00000001803AC1ED mov       rdx, qword ptr [rip + 0x17f5e24] # 0x181ba2018
00000001803AC1F4 cmp       dword ptr [rdx + 0xe0], 0
00000001803AC1FB jne       0x1803ac20c
00000001803AC1FD mov       rcx, rdx
00000001803AC200 call      0x1802501e0
00000001803AC205 mov       rdx, qword ptr [rip + 0x17f5e0c] # 0x181ba2018
00000001803AC20C mov       rax, qword ptr [rdx + 0xb8]
00000001803AC213 mov       rcx, qword ptr [rax]
00000001803AC216 test      rcx, rcx
00000001803AC219 je        0x1803ac431
00000001803AC21F mov       rax, qword ptr [rcx + 0x28]
00000001803AC223 test      rax, rax
00000001803AC226 je        0x1803ac42c
00000001803AC22C mov       r10, qword ptr [rax + 0x10]
00000001803AC230 mov       rax, qword ptr [rdx + 0xb8]
00000001803AC237 mov       rcx, qword ptr [rax]
00000001803AC23A mov       r11d, dword ptr [rcx + 0x30]
00000001803AC23E test      r10, r10
00000001803AC241 je        0x1803ac426
00000001803AC247 xor       r9d, r9d
00000001803AC24A mov       r8d, dword ptr [rcx + 0x34]
00000001803AC24E mov       edx, r11d
00000001803AC251 mov       rcx, r10
00000001803AC254 call      0x180380280
00000001803AC259 test      al, al
00000001803AC25B jne       0x1803ac1c0
00000001803AC261 test      rdi, rdi
00000001803AC264 je        0x1803ac43b
00000001803AC26A xor       r8d, r8d
00000001803AC26D movaps    xmm1, xmm6
00000001803AC270 mov       rcx, rdi
00000001803AC273 call      0x18031d210
00000001803AC278 test      rax, rax
00000001803AC27B je        0x1803ac1c0
00000001803AC281 test      rsi, rsi
00000001803AC284 je        0x1803ac436
00000001803AC28A mov       r8, qword ptr [rip + 0x1810ebf] # 0x181bbd150
00000001803AC291 mov       rdx, rax
00000001803AC294 mov       rcx, rsi
00000001803AC297 call      0x180002e70
00000001803AC29C jmp       0x1803ac1c0
00000001803AC2A1 mov       rdx, qword ptr [rip + 0x18196d8] # 0x181bc5980
00000001803AC2A8 mov       rcx, rbx
00000001803AC2AB call      0x180302170
00000001803AC2B0 jmp       0x1803ac2ec
00000001803AC2B2 mov       rdx, qword ptr [rip + 0x18196c7] # 0x181bc5980
00000001803AC2B9 mov       rcx, qword ptr [rsp + 0x28]
00000001803AC2BE call      0x180302170
00000001803AC2C3 mov       rcx, qword ptr [rsp + 0x20]
00000001803AC2C8 test      rcx, rcx
00000001803AC2CB jne       0x1803ac446
00000001803AC2D1 mov       r15, qword ptr [rsp + 0xe0]
00000001803AC2D9 mov       r14, qword ptr [rsp + 0xd8]
00000001803AC2E1 movss     xmm6, dword ptr [rsp + 0x38]
00000001803AC2E7 mov       rsi, qword ptr [rsp + 0x40]
00000001803AC2EC mov       rdx, qword ptr [r14 + 0x68]
00000001803AC2F0 test      rdx, rdx
00000001803AC2F3 je        0x1803ac420
00000001803AC2F9 mov       r8, qword ptr [rip + 0x17f8a58] # 0x181ba4d58
00000001803AC300 lea       rcx, [rsp + 0x20]
00000001803AC305 call      0x1808197f0
00000001803AC30A movups    xmm0, xmmword ptr [rsp + 0x20]
00000001803AC30F movups    xmmword ptr [rsp + 0x48], xmm0
00000001803AC314 movsd     xmm1, qword ptr [rsp + 0x30]
00000001803AC31A movsd     qword ptr [rsp + 0x58], xmm1
00000001803AC320 mov       qword ptr [rsp + 0x20], 0
00000001803AC329 lea       rbx, [rsp + 0x48]
00000001803AC32E mov       qword ptr [rsp + 0x28], rbx
00000001803AC333 mov       rdx, qword ptr [rip + 0x1815296] # 0x181bc15d0
00000001803AC33A lea       rcx, [rsp + 0x48]
00000001803AC33F call      0x180669280
00000001803AC344 test      al, al
00000001803AC346 je        0x1803ac3a6
00000001803AC348 mov       rcx, qword ptr [rsp + 0x58]
00000001803AC34D test      rcx, rcx
00000001803AC350 je        0x1803ac456
00000001803AC356 cmp       byte ptr [rcx + 0x18], 0
00000001803AC35A jne       0x1803ac362
00000001803AC35C cmp       byte ptr [rcx + 0x19], 0
00000001803AC360 je        0x1803ac333
00000001803AC362 xor       r8d, r8d
00000001803AC365 mov       rdx, r14
00000001803AC368 call      0x1803948a0
00000001803AC36D test      rax, rax
00000001803AC370 je        0x1803ac451
00000001803AC376 xor       r8d, r8d
00000001803AC379 movaps    xmm1, xmm6
00000001803AC37C mov       rcx, rax
00000001803AC37F call      0x18031d210
00000001803AC384 test      rax, rax
00000001803AC387 je        0x1803ac333
00000001803AC389 test      rsi, rsi
00000001803AC38C je        0x1803ac44c
00000001803AC392 mov       r8, qword ptr [rip + 0x1810db7] # 0x181bbd150
00000001803AC399 mov       rdx, rax
00000001803AC39C mov       rcx, rsi
00000001803AC39F call      0x180002e70
00000001803AC3A4 jmp       0x1803ac333
00000001803AC3A6 mov       rdx, qword ptr [rip + 0x18151cb] # 0x181bc1578
00000001803AC3AD mov       rcx, rbx
00000001803AC3B0 call      0x180302170
00000001803AC3B5 jmp       0x1803ac3e3
00000001803AC3B7 mov       rdx, qword ptr [rip + 0x18151ba] # 0x181bc1578
00000001803AC3BE mov       rcx, qword ptr [rsp + 0x28]
00000001803AC3C3 call      0x180302170
00000001803AC3C8 mov       rcx, qword ptr [rsp + 0x20]
00000001803AC3CD test      rcx, rcx
00000001803AC3D0 jne       0x1803ac45c
00000001803AC3D6 mov       r15, qword ptr [rsp + 0xe0]
00000001803AC3DE mov       rsi, qword ptr [rsp + 0x40]
00000001803AC3E3 test      r15, r15
00000001803AC3E6 je        0x1803ac420
00000001803AC3E8 xor       r8d, r8d
00000001803AC3EB mov       rdx, rsi
00000001803AC3EE mov       rcx, r15
00000001803AC3F1 call      0x180324c60
00000001803AC3F6 xor       r8d, r8d
00000001803AC3F9 mov       rdx, rsi
00000001803AC3FC mov       rcx, qword ptr [rsp + 0xd0]
00000001803AC404 call      0x1803ac470
00000001803AC409 movaps    xmm6, xmmword ptr [rsp + 0x90]
00000001803AC411 add       rsp, 0xa0
00000001803AC418 pop       r15
00000001803AC41A pop       r14
00000001803AC41C pop       rdi
00000001803AC41D pop       rsi
00000001803AC41E pop       rbx
00000001803AC41F ret       
00000001803AC420 call      0x180250150
00000001803AC425 nop       
00000001803AC426 call      0x180250150
00000001803AC42B nop       
00000001803AC42C call      0x180250150
00000001803AC431 call      0x180250150
00000001803AC436 call      0x180250150
00000001803AC43B call      0x180250150
00000001803AC440 call      0x180250150
00000001803AC445 nop       
00000001803AC446 call      0x180200e60
00000001803AC44B nop       
00000001803AC44C call      0x180250150
00000001803AC451 call      0x180250150
00000001803AC456 call      0x180250150
00000001803AC45B nop       
00000001803AC45C call      0x180200e60
00000001803AC461 int3      
00000001803AC462 int3      
00000001803AC463 int3      
00000001803AC464 int3      
00000001803AC465 int3      
00000001803AC466 int3      
00000001803AC467 int3      
00000001803AC468 int3      
00000001803AC469 int3      
00000001803AC46A int3      
00000001803AC46B int3      
00000001803AC46C int3      
00000001803AC46D int3      
00000001803AC46E int3      
00000001803AC46F int3      