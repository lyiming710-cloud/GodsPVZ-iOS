0000000180387F80 mov       rax, rsp
0000000180387F83 mov       qword ptr [rax + 8], rbx
0000000180387F87 mov       qword ptr [rax + 0x10], rsi
0000000180387F8B mov       qword ptr [rax + 0x18], rdi
0000000180387F8F mov       qword ptr [rax + 0x20], r12
0000000180387F93 push      r13
0000000180387F95 push      r14
0000000180387F97 push      r15
0000000180387F99 sub       rsp, 0xe0
0000000180387FA0 movaps    xmmword ptr [rax - 0x28], xmm6
0000000180387FA4 movaps    xmmword ptr [rax - 0x38], xmm7
0000000180387FA8 movaps    xmmword ptr [rax - 0x48], xmm8
0000000180387FAD movaps    xmmword ptr [rax - 0x58], xmm9
0000000180387FB2 movaps    xmmword ptr [rax - 0x68], xmm10
0000000180387FB7 movaps    xmmword ptr [rax - 0x78], xmm11
0000000180387FBC movaps    xmmword ptr [rsp + 0x70], xmm12
0000000180387FC2 mov       rsi, rcx
0000000180387FC5 cmp       byte ptr [rip + 0x195ce57], 0 # 0x181ce4e23
0000000180387FCC jne       0x18038805d
0000000180387FD2 lea       rcx, [rip + 0x1812d6f] # 0x181b9ad48
0000000180387FD9 call      0x18024fef0
0000000180387FDE lea       rcx, [rip + 0x183ea1b] # 0x181bc6a00
0000000180387FE5 call      0x18024fef0
0000000180387FEA lea       rcx, [rip + 0x183ea67] # 0x181bc6a58
0000000180387FF1 call      0x18024fef0
0000000180387FF6 lea       rcx, [rip + 0x183eab3] # 0x181bc6ab0
0000000180387FFD call      0x18024fef0
0000000180388002 lea       rcx, [rip + 0x181e06f] # 0x181ba6078
0000000180388009 call      0x18024fef0
000000018038800E lea       rcx, [rip + 0x182cb8b] # 0x181bb4ba0
0000000180388015 call      0x18024fef0
000000018038801A lea       rcx, [rip + 0x181e177] # 0x181ba6198
0000000180388021 call      0x18024fef0
0000000180388026 lea       rcx, [rip + 0x182f913] # 0x181bb7940
000000018038802D call      0x18024fef0
0000000180388032 lea       rcx, [rip + 0x18284c7] # 0x181bb0500
0000000180388039 call      0x18024fef0
000000018038803E lea       rcx, [rip + 0x182e433] # 0x181bb6478
0000000180388045 call      0x18024fef0
000000018038804A lea       rcx, [rip + 0x181a0bf] # 0x181ba2110
0000000180388051 call      0x18024fef0
0000000180388056 mov       byte ptr [rip + 0x195cdc6], 1 # 0x181ce4e23
000000018038805D xorps     xmm0, xmm0
0000000180388060 xor       eax, eax
0000000180388062 movups    xmmword ptr [rsp + 0x58], xmm0
0000000180388067 mov       qword ptr [rsp + 0x68], rax
000000018038806C mov       rcx, qword ptr [rip + 0x182e405] # 0x181bb6478
0000000180388073 cmp       dword ptr [rcx + 0xe0], eax
0000000180388079 jne       0x180388080
000000018038807B call      0x1802501e0
0000000180388080 cmp       byte ptr [rip + 0x195ce18], 0 # 0x181ce4e9f
0000000180388087 jne       0x18038809c
0000000180388089 lea       rcx, [rip + 0x182e3e8] # 0x181bb6478
0000000180388090 call      0x18024fef0
0000000180388095 mov       byte ptr [rip + 0x195ce03], 1 # 0x181ce4e9f
000000018038809C mov       rcx, qword ptr [rip + 0x182e3d5] # 0x181bb6478
00000001803880A3 cmp       dword ptr [rcx + 0xe0], 0
00000001803880AA jne       0x1803880b1
00000001803880AC call      0x1802501e0
00000001803880B1 mov       rax, qword ptr [rip + 0x182e3c0] # 0x181bb6478
00000001803880B8 mov       rcx, qword ptr [rax + 0xb8]
00000001803880BF mov       r12, qword ptr [rcx + 0x188]
00000001803880C6 mov       rcx, qword ptr [rip + 0x1828433] # 0x181bb0500
00000001803880CD cmp       dword ptr [rcx + 0xe0], 0
00000001803880D4 jne       0x1803880db
00000001803880D6 call      0x1802501e0
00000001803880DB xor       edx, edx
00000001803880DD mov       rcx, r12
00000001803880E0 call      0x18131f870
00000001803880E5 test      al, al
00000001803880E7 je        0x180388331
00000001803880ED mov       rax, qword ptr [rsi + 0x28]
00000001803880F1 test      rax, rax
00000001803880F4 je        0x1803883c9
00000001803880FA mov       rdx, qword ptr [rax + 0x60]
00000001803880FE test      rdx, rdx
0000000180388101 je        0x1803883c9
0000000180388107 mov       r8, qword ptr [rip + 0x182ca92] # 0x181bb4ba0
000000018038810E lea       rcx, [rsp + 0x40]
0000000180388113 call      0x1808197f0
0000000180388118 movups    xmm0, xmmword ptr [rsp + 0x40]
000000018038811D movups    xmmword ptr [rsp + 0x58], xmm0
0000000180388122 movsd     xmm1, qword ptr [rsp + 0x50]
0000000180388128 movsd     qword ptr [rsp + 0x68], xmm1
000000018038812E mov       qword ptr [rsp + 0x40], 0
0000000180388137 lea       rbx, [rsp + 0x58]
000000018038813C mov       qword ptr [rsp + 0x48], rbx
0000000180388141 mov       r13d, 2
0000000180388147 movss     xmm9, dword ptr [rip + 0x121fbdc] # 0x1815a7d2c
0000000180388150 movss     xmm10, dword ptr [rip + 0x121fd53] # 0x1815a7eac
0000000180388159 movss     xmm11, dword ptr [rip + 0x121fcd6] # 0x1815a7e38
0000000180388162 movss     xmm12, dword ptr [rip + 0x121fd31] # 0x1815a7e9c
000000018038816B xorps     xmm8, xmm8
000000018038816F nop       
0000000180388170 mov       rdx, qword ptr [rip + 0x183e8e1] # 0x181bc6a58
0000000180388177 lea       rcx, [rsp + 0x58]
000000018038817C call      0x180669280
0000000180388181 test      al, al
0000000180388183 je        0x180388301
0000000180388189 mov       r15, qword ptr [rsp + 0x68]
000000018038818E mov       rcx, qword ptr [rip + 0x182836b] # 0x181bb0500
0000000180388195 cmp       dword ptr [rcx + 0xe0], 0
000000018038819C jne       0x1803881a3
000000018038819E call      0x1802501e0
00000001803881A3 mov       rdx, qword ptr [rip + 0x182f796] # 0x181bb7940
00000001803881AA mov       rcx, r12
00000001803881AD call      0x1804a0690
00000001803881B2 mov       rdi, rax
00000001803881B5 test      rax, rax
00000001803881B8 je        0x1803883bd
00000001803881BE mov       qword ptr [rax + 0xf8], rsi
00000001803881C5 lea       rcx, [rax + 0xf8]
00000001803881CC mov       rdx, rsi
00000001803881CF call      0x18024f360
00000001803881D4 xor       edx, edx
00000001803881D6 mov       rcx, rdi
00000001803881D9 call      0x181304510
00000001803881DE mov       r14, rax
00000001803881E1 mov       rcx, qword ptr [rsi + 0x68]
00000001803881E5 test      rcx, rcx
00000001803881E8 je        0x1803883b8
00000001803881EE xor       edx, edx
00000001803881F0 call      0x18131c070
00000001803881F5 test      r14, r14
00000001803881F8 je        0x1803883b3
00000001803881FE xor       r9d, r9d
0000000180388201 xor       r8d, r8d
0000000180388204 mov       rdx, rax
0000000180388207 mov       rcx, r14
000000018038820A call      0x18132e900
000000018038820F mov       rax, qword ptr [rsi + 0x70]
0000000180388213 test      rax, rax
0000000180388216 je        0x1803883ae
000000018038821C mov       eax, dword ptr [rax + 0x18]
000000018038821F cdq       
0000000180388220 idiv      r13d
0000000180388223 movd      xmm6, eax
0000000180388227 cvtdq2ps  xmm6, xmm6
000000018038822A mulss     xmm6, xmm9
000000018038822F subss     xmm6, xmm10
0000000180388234 movd      xmm0, edx
0000000180388238 cvtdq2ps  xmm0, xmm0
000000018038823B mulss     xmm0, xmm11
0000000180388240 movaps    xmm7, xmm12
0000000180388244 subss     xmm7, xmm0
0000000180388248 xor       edx, edx
000000018038824A mov       rcx, rdi
000000018038824D call      0x181304510
0000000180388252 test      rax, rax
0000000180388255 je        0x1803883a9
000000018038825B movaps    xmm0, xmm6
000000018038825E unpcklps  xmm0, xmm7
0000000180388261 movsd     qword ptr [rsp + 0x30], xmm0
0000000180388267 movss     dword ptr [rsp + 0x38], xmm8
000000018038826E xor       r8d, r8d
0000000180388271 lea       rdx, [rsp + 0x30]
0000000180388276 mov       rcx, rax
0000000180388279 call      0x18132f910
000000018038827E xor       r8d, r8d
0000000180388281 mov       rdx, r15
0000000180388284 mov       rcx, rdi
0000000180388287 call      0x180343be0
000000018038828C mov       rcx, qword ptr [rsi + 0x70]
0000000180388290 test      rcx, rcx
0000000180388293 je        0x1803883a4
0000000180388299 mov       r9, qword ptr [rip + 0x181ddd8] # 0x181ba6078
00000001803882A0 inc       dword ptr [rcx + 0x1c]
00000001803882A3 mov       rdx, qword ptr [rcx + 0x10]
00000001803882A7 movsxd    r8, dword ptr [rcx + 0x18]
00000001803882AB test      rdx, rdx
00000001803882AE je        0x18038839f
00000001803882B4 cmp       r8d, dword ptr [rdx + 0x18]
00000001803882B8 jb        0x1803882d6
00000001803882BA mov       rax, qword ptr [r9 + 0x20]
00000001803882BE mov       r8, qword ptr [rax + 0xc0]
00000001803882C5 mov       r8, qword ptr [r8 + 0x70]
00000001803882C9 mov       rdx, rdi
00000001803882CC call      0x180847ce0
00000001803882D1 jmp       0x180388170
00000001803882D6 lea       eax, [r8 + 1]
00000001803882DA mov       dword ptr [rcx + 0x18], eax
00000001803882DD cmp       r8d, dword ptr [rdx + 0x18]
00000001803882E1 jae       0x180388399
00000001803882E7 mov       qword ptr [rdx + r8*8 + 0x20], rdi
00000001803882EC add       rdx, 0x20
00000001803882F0 lea       rcx, [rdx + r8*8]
00000001803882F4 mov       rdx, rdi
00000001803882F7 call      0x18024f360
00000001803882FC jmp       0x180388170
0000000180388301 mov       rdx, qword ptr [rip + 0x183e6f8] # 0x181bc6a00
0000000180388308 mov       rcx, rbx
000000018038830B call      0x180302170
0000000180388310 jmp       0x180388354
0000000180388312 mov       rdx, qword ptr [rip + 0x183e6e7] # 0x181bc6a00
0000000180388319 mov       rcx, qword ptr [rsp + 0x48]
000000018038831E call      0x180302170
0000000180388323 mov       rcx, qword ptr [rsp + 0x40]
0000000180388328 test      rcx, rcx
000000018038832B jne       0x1803883c3
0000000180388331 mov       rcx, qword ptr [rip + 0x1812a10] # 0x181b9ad48
0000000180388338 cmp       dword ptr [rcx + 0xe0], 0
000000018038833F jne       0x180388346
0000000180388341 call      0x1802501e0
0000000180388346 xor       edx, edx
0000000180388348 mov       rcx, qword ptr [rip + 0x1819dc1] # 0x181ba2110
000000018038834F call      0x1812e68f0
0000000180388354 lea       r11, [rsp + 0xe0]
000000018038835C mov       rbx, qword ptr [r11 + 0x20]
0000000180388360 mov       rsi, qword ptr [r11 + 0x28]
0000000180388364 mov       rdi, qword ptr [r11 + 0x30]
0000000180388368 mov       r12, qword ptr [r11 + 0x38]
000000018038836C movaps    xmm6, xmmword ptr [r11 - 0x10]
0000000180388371 movaps    xmm7, xmmword ptr [r11 - 0x20]
0000000180388376 movaps    xmm8, xmmword ptr [r11 - 0x30]
000000018038837B movaps    xmm9, xmmword ptr [r11 - 0x40]
0000000180388380 movaps    xmm10, xmmword ptr [r11 - 0x50]
0000000180388385 movaps    xmm11, xmmword ptr [r11 - 0x60]
000000018038838A movaps    xmm12, xmmword ptr [r11 - 0x70]
000000018038838F mov       rsp, r11
0000000180388392 pop       r15
0000000180388394 pop       r14
0000000180388396 pop       r13
0000000180388398 ret       
0000000180388399 call      0x180250140
000000018038839E nop       
000000018038839F call      0x180250150
00000001803883A4 call      0x180250150
00000001803883A9 call      0x180250150
00000001803883AE call      0x180250150
00000001803883B3 call      0x180250150
00000001803883B8 call      0x180250150
00000001803883BD call      0x180250150
00000001803883C2 nop       
00000001803883C3 call      0x180200e60
00000001803883C8 int3      
00000001803883C9 call      0x180250150
00000001803883CE int3      
00000001803883CF int3      