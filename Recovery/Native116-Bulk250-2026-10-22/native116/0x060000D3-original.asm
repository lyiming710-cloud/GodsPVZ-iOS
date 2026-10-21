00000001802FF140 mov       rax, rsp
00000001802FF143 mov       qword ptr [rax + 0x10], rbx
00000001802FF147 push      rsi
00000001802FF148 push      rdi
00000001802FF149 push      r14
00000001802FF14B sub       rsp, 0x90
00000001802FF152 movaps    xmmword ptr [rax - 0x28], xmm6
00000001802FF156 movaps    xmmword ptr [rax - 0x38], xmm7
00000001802FF15A movaps    xmmword ptr [rax - 0x48], xmm8
00000001802FF15F movzx     esi, dl
00000001802FF162 mov       rbx, rcx
00000001802FF165 cmp       byte ptr [rip + 0x19e59b3], 0 # 0x181ce4b1f
00000001802FF16C jne       0x1802ff1b1
00000001802FF16E lea       rcx, [rip + 0x18c40c3] # 0x181bc3238
00000001802FF175 call      0x18024fef0
00000001802FF17A lea       rcx, [rip + 0x18a5d6f] # 0x181ba4ef0
00000001802FF181 call      0x18024fef0
00000001802FF186 lea       rcx, [rip + 0x18a60c3] # 0x181ba5250
00000001802FF18D call      0x18024fef0
00000001802FF192 lea       rcx, [rip + 0x18b1367] # 0x181bb0500
00000001802FF199 call      0x18024fef0
00000001802FF19E lea       rcx, [rip + 0x18c04c3] # 0x181bbf668
00000001802FF1A5 call      0x18024fef0
00000001802FF1AA mov       byte ptr [rip + 0x19e596e], 1 # 0x181ce4b1f
00000001802FF1B1 xor       r14d, r14d
00000001802FF1B4 mov       qword ptr [rsp + 0xc8], r14
00000001802FF1BC mov       rdi, qword ptr [rbx + 0x10]
00000001802FF1C0 mov       rcx, qword ptr [rip + 0x18b1339] # 0x181bb0500
00000001802FF1C7 cmp       dword ptr [rcx + 0xe0], r14d
00000001802FF1CE jne       0x1802ff1d5
00000001802FF1D0 call      0x1802501e0
00000001802FF1D5 xor       r8d, r8d
00000001802FF1D8 xor       edx, edx
00000001802FF1DA mov       rcx, rdi
00000001802FF1DD call      0x18131f900
00000001802FF1E2 test      al, al
00000001802FF1E4 je        0x1802ff437
00000001802FF1EA mov       rcx, qword ptr [rbx + 0x10]
00000001802FF1EE test      rcx, rcx
00000001802FF1F1 je        0x1802ff486
00000001802FF1F7 xor       edx, edx
00000001802FF1F9 call      0x1813044d0
00000001802FF1FE test      rax, rax
00000001802FF201 je        0x1802ff486
00000001802FF207 xor       r8d, r8d
00000001802FF20A mov       dl, 1
00000001802FF20C mov       rcx, rax
00000001802FF20F call      0x18131bad0
00000001802FF214 mov       rcx, qword ptr [rbx + 0x10]
00000001802FF218 test      rcx, rcx
00000001802FF21B je        0x1802ff486
00000001802FF221 xor       edx, edx
00000001802FF223 call      0x18132dc20
00000001802FF228 mov       qword ptr [rsp + 0xb0], rax
00000001802FF230 lea       rax, [rsp + 0xb0]
00000001802FF238 mov       qword ptr [rsp + 0x30], rax
00000001802FF23D lea       rax, [rsp + 0xc8]
00000001802FF245 mov       qword ptr [rsp + 0x38], rax
00000001802FF24A mov       qword ptr [rsp + 0x40], r14
00000001802FF24F movups    xmm6, xmmword ptr [rsp + 0x30]
00000001802FF254 movups    xmmword ptr [rsp + 0x48], xmm6
00000001802FF259 movdqa    xmm7, xmmword ptr [rip + 0x12a881f] # 0x1815a7a80
00000001802FF261 movdqa    xmm8, xmmword ptr [rip + 0x12a8826] # 0x1815a7a90
00000001802FF26A nop       word ptr [rax + rax]
00000001802FF270 mov       r8, qword ptr [rsp + 0xb0]
00000001802FF278 test      r8, r8
00000001802FF27B je        0x1802ff47a
00000001802FF281 xor       ecx, ecx
00000001802FF283 mov       rdx, qword ptr [rip + 0x18a5fc6] # 0x181ba5250
00000001802FF28A call      0x180002cd0
00000001802FF28F test      al, al
00000001802FF291 je        0x1802ff3ea
00000001802FF297 mov       rbx, qword ptr [rsp + 0xb0]
00000001802FF29F test      rbx, rbx
00000001802FF2A2 je        0x1802ff475
00000001802FF2A8 mov       r9, qword ptr [rip + 0x18a5fa1] # 0x181ba5250
00000001802FF2AF mov       r10, qword ptr [rbx]
00000001802FF2B2 movzx     ecx, r14w
00000001802FF2B6 movzx     edx, word ptr [r10 + 0x12e]
00000001802FF2BE cmp       r14w, dx
00000001802FF2C2 jae       0x1802ff2e4
00000001802FF2C4 mov       r8, qword ptr [r10 + 0xb0]
00000001802FF2CB nop       dword ptr [rax + rax]
00000001802FF2D0 movzx     eax, cx
00000001802FF2D3 add       rax, rax
00000001802FF2D6 cmp       qword ptr [r8 + rax*8], r9
00000001802FF2DA je        0x1802ff319
00000001802FF2DC inc       cx
00000001802FF2DF cmp       cx, dx
00000001802FF2E2 jb        0x1802ff2d0
00000001802FF2E4 mov       r8d, 1
00000001802FF2EA mov       rdx, r9
00000001802FF2ED mov       rcx, rbx
00000001802FF2F0 call      0x180258380
00000001802FF2F5 mov       rdx, rax
00000001802FF2F8 mov       rax, qword ptr [rdx]
00000001802FF2FB mov       rdx, qword ptr [rdx + 8]
00000001802FF2FF mov       rcx, rbx
00000001802FF302 call      rax
00000001802FF304 mov       r8, rax
00000001802FF307 mov       rcx, qword ptr [rip + 0x18c035a] # 0x181bbf668
00000001802FF30E test      rax, rax
00000001802FF311 je        0x1802ff45a
00000001802FF317 jmp       0x1802ff339
00000001802FF319 movzx     ecx, cx
00000001802FF31C add       rcx, rcx
00000001802FF31F mov       ecx, dword ptr [r8 + rcx*8 + 8]
00000001802FF324 inc       ecx
00000001802FF326 movsxd    rdx, ecx
00000001802FF329 shl       rdx, 4
00000001802FF32D add       rdx, 0x138
00000001802FF334 add       rdx, r10
00000001802FF337 jmp       0x1802ff2f8
00000001802FF339 mov       r9, qword ptr [rax]
00000001802FF33C movzx     eax, byte ptr [rcx + 0x130]
00000001802FF343 cmp       byte ptr [r9 + 0x130], al
00000001802FF34A jb        0x1802ff46a
00000001802FF350 movzx     edx, al
00000001802FF353 mov       rax, qword ptr [r9 + 0xc8]
00000001802FF35A cmp       qword ptr [rax + rdx*8 - 8], rcx
00000001802FF35F jne       0x1802ff46a
00000001802FF365 mov       rdx, qword ptr [rip + 0x18c3ecc] # 0x181bc3238
00000001802FF36C mov       rcx, r8
00000001802FF36F call      0x18042e9f0
00000001802FF374 mov       rbx, rax
00000001802FF377 mov       rcx, qword ptr [rip + 0x18b1182] # 0x181bb0500
00000001802FF37E cmp       dword ptr [rcx + 0xe0], 0
00000001802FF385 jne       0x1802ff38c
00000001802FF387 call      0x1802501e0
00000001802FF38C xor       edx, edx
00000001802FF38E mov       rcx, rbx
00000001802FF391 call      0x18131f870
00000001802FF396 test      al, al
00000001802FF398 je        0x1802ff270
00000001802FF39E test      sil, sil
00000001802FF3A1 jne       0x1802ff3c7
00000001802FF3A3 test      rbx, rbx
00000001802FF3A6 je        0x1802ff460
00000001802FF3AC movaps    xmmword ptr [rsp + 0x30], xmm8
00000001802FF3B2 xor       r8d, r8d
00000001802FF3B5 lea       rdx, [rsp + 0x30]
00000001802FF3BA mov       rcx, rbx
00000001802FF3BD call      0x1813290a0
00000001802FF3C2 jmp       0x1802ff270
00000001802FF3C7 test      rbx, rbx
00000001802FF3CA je        0x1802ff465
00000001802FF3D0 movaps    xmmword ptr [rsp + 0x30], xmm7
00000001802FF3D5 xor       r8d, r8d
00000001802FF3D8 lea       rdx, [rsp + 0x30]
00000001802FF3DD mov       rcx, rbx
00000001802FF3E0 call      0x1813290a0
00000001802FF3E5 jmp       0x1802ff270
00000001802FF3EA movq      rax, xmm6
00000001802FF3EF mov       rdx, qword ptr [rip + 0x18a5afa] # 0x181ba4ef0
00000001802FF3F6 mov       rcx, qword ptr [rax]
00000001802FF3F9 call      0x18024f380
00000001802FF3FE mov       r8, rax
00000001802FF401 psrldq    xmm6, 8
00000001802FF406 movq      rax, xmm6
00000001802FF40B mov       qword ptr [rax], r8
00000001802FF40E test      r8, r8
00000001802FF411 je        0x1802ff421
00000001802FF413 xor       ecx, ecx
00000001802FF415 mov       rdx, qword ptr [rip + 0x18a5ad4] # 0x181ba4ef0
00000001802FF41C call      0x180002d90
00000001802FF421 jmp       0x1802ff437
00000001802FF423 lea       rcx, [rsp + 0x48]
00000001802FF428 call      0x1802fb000
00000001802FF42D mov       rcx, qword ptr [rsp + 0x40]
00000001802FF432 test      rcx, rcx
00000001802FF435 jne       0x1802ff480
00000001802FF437 lea       r11, [rsp + 0x90]
00000001802FF43F mov       rbx, qword ptr [r11 + 0x28]
00000001802FF443 movaps    xmm6, xmmword ptr [r11 - 0x10]
00000001802FF448 movaps    xmm7, xmmword ptr [rsp + 0x70]
00000001802FF44D movaps    xmm8, xmmword ptr [r11 - 0x30]
00000001802FF452 mov       rsp, r11
00000001802FF455 pop       r14
00000001802FF457 pop       rdi
00000001802FF458 pop       rsi
00000001802FF459 ret       
00000001802FF45A call      0x180250150
00000001802FF45F nop       
00000001802FF460 call      0x180250150
00000001802FF465 call      0x180250150
00000001802FF46A mov       rdx, rcx
00000001802FF46D mov       rcx, r8
00000001802FF470 call      0x18024f3a0
00000001802FF475 call      0x180250150
00000001802FF47A call      0x180250150
00000001802FF47F nop       
00000001802FF480 call      0x180200e60
00000001802FF485 int3      
00000001802FF486 call      0x180250150
00000001802FF48B int3      
00000001802FF48C int3      
00000001802FF48D int3      
00000001802FF48E int3      
00000001802FF48F int3      