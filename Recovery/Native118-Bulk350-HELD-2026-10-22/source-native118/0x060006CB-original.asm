000000018039C060 mov       qword ptr [rsp + 0x18], rbx
000000018039C065 movss     dword ptr [rsp + 0x10], xmm1
000000018039C06B push      rdi
000000018039C06C sub       rsp, 0xd0
000000018039C073 cmp       byte ptr [rip + 0x1948e9f], 0 # 0x181ce4f19
000000018039C07A mov       rbx, rcx
000000018039C07D jne       0x18039c10b
000000018039C083 lea       rcx, [rip + 0x1810b6e] # 0x181bacbf8
000000018039C08A call      0x18024fef0
000000018039C08F lea       rcx, [rip + 0x18158da] # 0x181bb1970
000000018039C096 call      0x18024fef0
000000018039C09B lea       rcx, [rip + 0x180dfde] # 0x181baa080
000000018039C0A2 call      0x18024fef0
000000018039C0A7 lea       rcx, [rip + 0x180df7a] # 0x181baa028
000000018039C0AE call      0x18024fef0
000000018039C0B3 lea       rcx, [rip + 0x182d16e] # 0x181bc9228
000000018039C0BA call      0x18024fef0
000000018039C0BF lea       rcx, [rip + 0x181e3ca] # 0x181bba490
000000018039C0C6 call      0x18024fef0
000000018039C0CB lea       rcx, [rip + 0x1806e0e] # 0x181ba2ee0
000000018039C0D2 call      0x18024fef0
000000018039C0D7 lea       rcx, [rip + 0x1813a7a] # 0x181bafb58
000000018039C0DE call      0x18024fef0
000000018039C0E3 lea       rcx, [rip + 0x180de8e] # 0x181ba9f78
000000018039C0EA call      0x18024fef0
000000018039C0EF lea       rcx, [rip + 0x180672a] # 0x181ba2820
000000018039C0F6 call      0x18024fef0
000000018039C0FB movss     xmm1, dword ptr [rsp + 0xe8]
000000018039C104 mov       byte ptr [rip + 0x1948e0e], 1 # 0x181ce4f19
000000018039C10B mov       rdi, qword ptr [rbx + 0x10]
000000018039C10F xor       edx, edx
000000018039C111 movaps    xmmword ptr [rsp + 0xc0], xmm6
000000018039C119 mov       rcx, rdi
000000018039C11C movaps    xmmword ptr [rsp + 0xa0], xmm8
000000018039C125 movaps    xmmword ptr [rsp + 0x90], xmm9
000000018039C12E movaps    xmmword ptr [rsp + 0x80], xmm10
000000018039C137 movaps    xmmword ptr [rsp + 0x70], xmm11
000000018039C13D movaps    xmmword ptr [rsp + 0x60], xmm12
000000018039C143 movaps    xmmword ptr [rsp + 0x50], xmm13
000000018039C149 movaps    xmmword ptr [rsp + 0x40], xmm14
000000018039C14F movaps    xmmword ptr [rsp + 0x30], xmm15
000000018039C155 movss     dword ptr [rbx + 0x40], xmm1
000000018039C15A call      0x1803ccf20
000000018039C15F cmp       eax, 0x549c27b9
000000018039C164 jbe       0x18039c1d9
000000018039C166 cmp       eax, 0x70eb52e5
000000018039C16B jbe       0x18039c1a3
000000018039C16D cmp       eax, 0x7ada1982
000000018039C172 jne       0x18039c18f
000000018039C174 mov       rdx, qword ptr [rip + 0x18157f5] # 0x181bb1970
000000018039C17B xor       r8d, r8d
000000018039C17E mov       rcx, rdi
000000018039C181 call      0x180b76170
000000018039C186 test      al, al
000000018039C188 jne       0x18039c206
000000018039C18A jmp       0x18039c261
000000018039C18F cmp       eax, 0xbaabd2d5
000000018039C194 jne       0x18039c261
000000018039C19A mov       rdx, qword ptr [rip + 0x180ddd7] # 0x181ba9f78
000000018039C1A1 jmp       0x18039c1c5
000000018039C1A3 cmp       eax, 0x67029c76
000000018039C1A8 jne       0x18039c1b3
000000018039C1AA mov       rdx, qword ptr [rip + 0x18139a7] # 0x181bafb58
000000018039C1B1 jmp       0x18039c1c5
000000018039C1B3 cmp       eax, 0x70eb52e5
000000018039C1B8 jne       0x18039c261
000000018039C1BE mov       rdx, qword ptr [rip + 0x182d063] # 0x181bc9228
000000018039C1C5 xor       r8d, r8d
000000018039C1C8 mov       rcx, rdi
000000018039C1CB call      0x180b76170
000000018039C1D0 test      al, al
000000018039C1D2 jne       0x18039c22c
000000018039C1D4 jmp       0x18039c261
000000018039C1D9 cmp       eax, 0x4a0effac
000000018039C1DE jne       0x18039c1e9
000000018039C1E0 mov       rdx, qword ptr [rip + 0x181e2a9] # 0x181bba490
000000018039C1E7 jmp       0x18039c1c5
000000018039C1E9 cmp       eax, 0x529c2493
000000018039C1EE jne       0x18039c20f
000000018039C1F0 mov       rdx, qword ptr [rip + 0x180de31] # 0x181baa028
000000018039C1F7 xor       r8d, r8d
000000018039C1FA mov       rcx, rdi
000000018039C1FD call      0x180b76170
000000018039C202 test      al, al
000000018039C204 je        0x18039c261
000000018039C206 mov       rdx, qword ptr [rip + 0x1806cd3] # 0x181ba2ee0
000000018039C20D jmp       0x18039c233
000000018039C20F cmp       eax, 0x549c27b9
000000018039C214 jne       0x18039c261
000000018039C216 mov       rdx, qword ptr [rip + 0x180de63] # 0x181baa080
000000018039C21D xor       r8d, r8d
000000018039C220 mov       rcx, rdi
000000018039C223 call      0x180b76170
000000018039C228 test      al, al
000000018039C22A je        0x18039c261
000000018039C22C mov       rdx, qword ptr [rip + 0x18065ed] # 0x181ba2820
000000018039C233 mov       rdi, qword ptr [rbx + 0x38]
000000018039C237 lea       rcx, [rsp + 0xe8]
000000018039C23F xor       r8d, r8d
000000018039C242 call      0x180cc5a50
000000018039C247 test      rdi, rdi
000000018039C24A je        0x18039c58b
000000018039C250 xor       r9d, r9d
000000018039C253 mov       r8b, 1
000000018039C256 mov       rdx, rax
000000018039C259 mov       rcx, rdi
000000018039C25C call      0x1811af070
000000018039C261 movss     xmm6, dword ptr [rip + 0x120b7a7] # 0x1815a7a10
000000018039C269 xor       r8d, r8d
000000018039C26C mov       rdx, qword ptr [rip + 0x18138e5] # 0x181bafb58
000000018039C273 movaps    xmm15, xmm6
000000018039C277 mov       rcx, qword ptr [rbx + 0x10]
000000018039C27B movaps    xmm11, xmm6
000000018039C27F movss     dword ptr [rsp + 0xe0], xmm6
000000018039C288 movaps    xmm12, xmm6
000000018039C28C xorps     xmm8, xmm8
000000018039C290 xorps     xmm9, xmm9
000000018039C294 call      0x180b76170
000000018039C299 test      al, al
000000018039C29B jne       0x18039c561
000000018039C2A1 mov       rdx, qword ptr [rip + 0x180dd80] # 0x181baa028
000000018039C2A8 xor       r8d, r8d
000000018039C2AB mov       rcx, qword ptr [rbx + 0x10]
000000018039C2AF call      0x180b76170
000000018039C2B4 test      al, al
000000018039C2B6 jne       0x18039c561
000000018039C2BC mov       rdx, qword ptr [rip + 0x18156ad] # 0x181bb1970
000000018039C2C3 xor       r8d, r8d
000000018039C2C6 mov       rcx, qword ptr [rbx + 0x10]
000000018039C2CA call      0x180b76170
000000018039C2CF test      al, al
000000018039C2D1 jne       0x18039c561
000000018039C2D7 movaps    xmmword ptr [rsp + 0xb0], xmm7
000000018039C2DF movss     xmm0, dword ptr [rsp + 0xe8]
000000018039C2E8 divss     xmm0, dword ptr [rbx + 0x44]
000000018039C2ED comiss    xmm0, xmm8
000000018039C2F1 jb        0x18039c2fc
000000018039C2F3 comiss    xmm6, xmm0
000000018039C2F6 jb        0x18039c2fc
000000018039C2F8 movaps    xmm9, xmm0
000000018039C2FC comiss    xmm0, xmm6
000000018039C2FF movss     xmm1, dword ptr [rip + 0x120b71d] # 0x1815a7a24
000000018039C307 xorps     xmm7, xmm7
000000018039C30A movss     xmm3, dword ptr [rip + 0x120bc62] # 0x1815a7f74
000000018039C312 movss     xmm4, dword ptr [rip + 0x120bc4e] # 0x1815a7f68
000000018039C31A jbe       0x18039c33c
000000018039C31C comiss    xmm1, xmm0
000000018039C31F jb        0x18039c33c
000000018039C321 movaps    xmm7, xmm0
000000018039C324 movaps    xmm9, xmm6
000000018039C328 subss     xmm7, xmm6
000000018039C32C movaps    xmm11, xmm3
000000018039C330 movaps    xmm12, xmm4
000000018039C334 mulss     xmm7, dword ptr [rip + 0x120b6cc] # 0x1815a7a08
000000018039C33C comiss    xmm0, xmm1
000000018039C33F movss     xmm2, dword ptr [rip + 0x120b909] # 0x1815a7c50
000000018039C347 movaps    xmm10, xmm6
000000018039C34B movss     xmm5, dword ptr [rip + 0x120bc11] # 0x1815a7f64
000000018039C353 movss     xmm14, dword ptr [rip + 0x120b768] # 0x1815a7ac4
000000018039C35C jbe       0x18039c38f
000000018039C35E comiss    xmm2, xmm0
000000018039C361 jb        0x18039c38f
000000018039C363 movaps    xmm7, xmm0
000000018039C366 movss     dword ptr [rsp + 0xe0], xmm4
000000018039C36F subss     xmm7, xmm1
000000018039C373 movaps    xmm9, xmm6
000000018039C377 movaps    xmm15, xmm3
000000018039C37B movaps    xmm11, xmm5
000000018039C37F movaps    xmm10, xmm14
000000018039C383 movaps    xmm12, xmm6
000000018039C387 mulss     xmm7, dword ptr [rip + 0x120b721] # 0x1815a7ab0
000000018039C38F comiss    xmm0, xmm2
000000018039C392 movss     xmm1, dword ptr [rip + 0x120b84e] # 0x1815a7be8
000000018039C39A movaps    xmm13, xmm6
000000018039C39E jbe       0x18039c3dc
000000018039C3A0 comiss    xmm1, xmm0
000000018039C3A3 jbe       0x18039c3dc
000000018039C3A5 movss     xmm11, dword ptr [rip + 0x120bbc2] # 0x1815a7f70
000000018039C3AE movaps    xmm7, xmm0
000000018039C3B1 movss     xmm10, dword ptr [rip + 0x120bbb2] # 0x1815a7f6c
000000018039C3BA subss     xmm7, xmm2
000000018039C3BE movaps    xmm13, xmm14
000000018039C3C2 movaps    xmm9, xmm6
000000018039C3C6 movaps    xmm15, xmm5
000000018039C3CA movaps    xmm14, xmm6
000000018039C3CE movaps    xmm12, xmm6
000000018039C3D2 mulss     xmm7, dword ptr [rip + 0x120bb86] # 0x1815a7f60
000000018039C3DA jmp       0x18039c3e6
000000018039C3DC movss     xmm14, dword ptr [rsp + 0xe0]
000000018039C3E6 comiss    xmm0, xmm1
000000018039C3E9 jb        0x18039c403
000000018039C3EB movss     xmm10, dword ptr [rip + 0x120b618] # 0x1815a7a0c
000000018039C3F4 xorps     xmm9, xmm9
000000018039C3F8 movaps    xmm7, xmm6
000000018039C3FB movaps    xmm11, xmm6
000000018039C3FF xorps     xmm12, xmm12
000000018039C403 mov       rcx, qword ptr [rip + 0x18107ee] # 0x181bacbf8
000000018039C40A cmp       dword ptr [rcx + 0xe0], 0
000000018039C411 jne       0x18039c418
000000018039C413 call      0x1802501e0
000000018039C418 xor       r9d, r9d
000000018039C41B movaps    xmm2, xmm6
000000018039C41E xorps     xmm1, xmm1
000000018039C421 movaps    xmm0, xmm9
000000018039C425 call      0x18030e0e0
000000018039C42A movaps    xmm9, xmm0
000000018039C42E xor       r9d, r9d
000000018039C431 movaps    xmm0, xmm7
000000018039C434 movaps    xmm2, xmm6
000000018039C437 xorps     xmm1, xmm1
000000018039C43A call      0x18030e0e0
000000018039C43F mov       rcx, qword ptr [rbx + 0x28]
000000018039C443 movaps    xmm6, xmm0
000000018039C446 movaps    xmm7, xmmword ptr [rsp + 0xb0]
000000018039C44E test      rcx, rcx
000000018039C451 je        0x18039c58b
000000018039C457 xor       r8d, r8d
000000018039C45A movaps    xmm1, xmm9
000000018039C45E call      0x1813d7750
000000018039C463 mov       rcx, qword ptr [rbx + 0x30]
000000018039C467 test      rcx, rcx
000000018039C46A je        0x18039c58b
000000018039C470 xor       r8d, r8d
000000018039C473 movaps    xmm1, xmm6
000000018039C476 call      0x1813d7750
000000018039C47B mov       rcx, qword ptr [rbx + 0x28]
000000018039C47F movss     dword ptr [rsp + 0x20], xmm15
000000018039C486 movss     dword ptr [rsp + 0x24], xmm13
000000018039C48D movss     dword ptr [rsp + 0x28], xmm14
000000018039C494 mov       dword ptr [rsp + 0x2c], 0x3f800000
000000018039C49C test      rcx, rcx
000000018039C49F je        0x18039c58b
000000018039C4A5 mov       rax, qword ptr [rcx]
000000018039C4A8 lea       rdx, [rsp + 0x20]
000000018039C4AD movups    xmm0, xmmword ptr [rsp + 0x20]
000000018039C4B2 mov       r8, qword ptr [rax + 0x2b0]
000000018039C4B9 movdqa    xmmword ptr [rsp + 0x20], xmm0
000000018039C4BF call      qword ptr [rax + 0x2a8]
000000018039C4C5 mov       rcx, qword ptr [rbx + 0x30]
000000018039C4C9 movss     dword ptr [rsp + 0x20], xmm11
000000018039C4D0 movss     dword ptr [rsp + 0x24], xmm10
000000018039C4D7 movss     dword ptr [rsp + 0x28], xmm12
000000018039C4DE mov       dword ptr [rsp + 0x2c], 0x3f800000
000000018039C4E6 test      rcx, rcx
000000018039C4E9 je        0x18039c58b
000000018039C4EF mov       rax, qword ptr [rcx]
000000018039C4F2 lea       rdx, [rsp + 0x20]
000000018039C4F7 movups    xmm0, xmmword ptr [rsp + 0x20]
000000018039C4FC mov       r8, qword ptr [rax + 0x2b0]
000000018039C503 movdqa    xmmword ptr [rsp + 0x20], xmm0
000000018039C509 call      qword ptr [rax + 0x2a8]
000000018039C50F movaps    xmm15, xmmword ptr [rsp + 0x30]
000000018039C515 movaps    xmm14, xmmword ptr [rsp + 0x40]
000000018039C51B movaps    xmm13, xmmword ptr [rsp + 0x50]
000000018039C521 movaps    xmm12, xmmword ptr [rsp + 0x60]
000000018039C527 movaps    xmm11, xmmword ptr [rsp + 0x70]
000000018039C52D movaps    xmm10, xmmword ptr [rsp + 0x80]
000000018039C536 movaps    xmm9, xmmword ptr [rsp + 0x90]
000000018039C53F movaps    xmm8, xmmword ptr [rsp + 0xa0]
000000018039C548 movaps    xmm6, xmmword ptr [rsp + 0xc0]
000000018039C550 mov       rbx, qword ptr [rsp + 0xf0]
000000018039C558 add       rsp, 0xd0
000000018039C55F pop       rdi
000000018039C560 ret       
000000018039C561 mov       rcx, qword ptr [rbx + 0x28]
000000018039C565 test      rcx, rcx
000000018039C568 je        0x18039c58b
000000018039C56A xor       r8d, r8d
000000018039C56D xorps     xmm1, xmm1
000000018039C570 call      0x1813d7750
000000018039C575 mov       rcx, qword ptr [rbx + 0x30]
000000018039C579 test      rcx, rcx
000000018039C57C je        0x18039c58b
000000018039C57E xor       r8d, r8d
000000018039C581 xorps     xmm1, xmm1
000000018039C584 call      0x1813d7750
000000018039C589 jmp       0x18039c50f
000000018039C58B call      0x180250150
000000018039C590 int3      
000000018039C591 int3      
000000018039C592 int3      
000000018039C593 int3      
000000018039C594 int3      
000000018039C595 int3      
000000018039C596 int3      
000000018039C597 int3      
000000018039C598 int3      
000000018039C599 int3      
000000018039C59A int3      
000000018039C59B int3      
000000018039C59C int3      
000000018039C59D int3      
000000018039C59E int3      
000000018039C59F int3      