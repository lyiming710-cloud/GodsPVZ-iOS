00000001803B3100 mov       rax, rsp
00000001803B3103 mov       qword ptr [rax + 0x20], rbx
00000001803B3107 push      rdi
00000001803B3108 sub       rsp, 0x100
00000001803B310F cmp       byte ptr [rip + 0x1931e4b], 0 # 0x181ce4f61
00000001803B3116 mov       rdi, rcx
00000001803B3119 movaps    xmmword ptr [rax - 0x38], xmm8
00000001803B311E movaps    xmmword ptr [rax - 0x48], xmm9
00000001803B3123 movaps    xmmword ptr [rax - 0x78], xmm12
00000001803B3128 jne       0x1803b316d
00000001803B312A lea       rcx, [rip + 0x17f1247] # 0x181ba4378
00000001803B3131 call      0x18024fef0
00000001803B3136 lea       rcx, [rip + 0x17eeb7b] # 0x181ba1cb8
00000001803B313D call      0x18024fef0
00000001803B3142 lea       rcx, [rip + 0x181f007] # 0x181bd2150
00000001803B3149 call      0x18024fef0
00000001803B314E lea       rcx, [rip + 0x1805103] # 0x181bb8258
00000001803B3155 call      0x18024fef0
00000001803B315A lea       rcx, [rip + 0x1820ed7] # 0x181bd4038
00000001803B3161 call      0x18024fef0
00000001803B3166 mov       byte ptr [rip + 0x1931df4], 1 # 0x181ce4f61
00000001803B316D mov       qword ptr [rsp + 0x118], rsi
00000001803B3175 xor       edx, edx
00000001803B3177 movaps    xmmword ptr [rsp + 0xf0], xmm6
00000001803B317F mov       rcx, rdi
00000001803B3182 movaps    xmmword ptr [rsp + 0xe0], xmm7
00000001803B318A movaps    xmmword ptr [rsp + 0xb0], xmm10
00000001803B3193 movaps    xmmword ptr [rsp + 0xa0], xmm11
00000001803B319C movaps    xmmword ptr [rsp + 0x80], xmm13
00000001803B31A5 movaps    xmmword ptr [rsp + 0x70], xmm14
00000001803B31AB movaps    xmmword ptr [rsp + 0x60], xmm15
00000001803B31B1 call      0x181304510
00000001803B31B6 lea       rcx, [rdi + 0x30]
00000001803B31BA mov       qword ptr [rdi + 0x30], rax
00000001803B31BE mov       rdx, rax
00000001803B31C1 call      0x18024f360
00000001803B31C6 xor       ecx, ecx
00000001803B31C8 xorps     xmm12, xmm12
00000001803B31CC xorps     xmm9, xmm9
00000001803B31D0 call      0x1812e27c0
00000001803B31D5 xor       ecx, ecx
00000001803B31D7 mov       rbx, rax
00000001803B31DA call      0x1812fb930
00000001803B31DF cdq       
00000001803B31E0 sub       eax, edx
00000001803B31E2 sar       eax, 1
00000001803B31E4 movd      xmm8, eax
00000001803B31E9 cvtdq2ps  xmm8, xmm8
00000001803B31ED test      rbx, rbx
00000001803B31F0 je        0x1803b3539
00000001803B31F6 xor       r8d, r8d
00000001803B31F9 movaps    xmm1, xmm8
00000001803B31FD mov       rcx, rbx
00000001803B3200 call      0x1812e2e80
00000001803B3205 xor       ecx, ecx
00000001803B3207 call      0x1812fb960
00000001803B320C xor       ecx, ecx
00000001803B320E movd      xmm11, eax
00000001803B3213 cvtdq2ps  xmm11, xmm11
00000001803B3217 call      0x1812fb930
00000001803B321C movd      xmm0, eax
00000001803B3220 mov       eax, dword ptr [rdi + 0x24]
00000001803B3223 cvtdq2ps  xmm0, xmm0
00000001803B3226 mov       dword ptr [rsp + 0x110], eax
00000001803B322D divss     xmm11, xmm0
00000001803B3232 cmp       eax, dword ptr [rdi + 0x28]
00000001803B3235 jg        0x1803b34da
00000001803B323B movss     xmm13, dword ptr [rip + 0x11f4aac] # 0x1815a7cf0
00000001803B3244 movss     xmm10, dword ptr [rip + 0x11f4db3] # 0x1815a8000
00000001803B324D movss     xmm15, dword ptr [rip + 0x11f47b2] # 0x1815a7a08
00000001803B3256 movss     xmm14, dword ptr [rip + 0x11f4d2d] # 0x1815a7f8c
00000001803B325F nop       
00000001803B3260 cmp       dword ptr [rdi + 0x20], 0
00000001803B3264 jne       0x1803b34c7
00000001803B326A xor       edx, edx
00000001803B326C lea       rcx, [rsp + 0x110]
00000001803B3274 call      0x180ca3820
00000001803B3279 mov       r8, qword ptr [rip + 0x181eed0] # 0x181bd2150
00000001803B3280 xor       r9d, r9d
00000001803B3283 mov       rcx, qword ptr [rip + 0x1804fce] # 0x181bb8258
00000001803B328A mov       rdx, rax
00000001803B328D call      0x180b73f50
00000001803B3292 mov       rcx, qword ptr [rip + 0x17eea1f] # 0x181ba1cb8
00000001803B3299 mov       rbx, rax
00000001803B329C call      0x180250100
00000001803B32A1 xor       r8d, r8d
00000001803B32A4 mov       rdx, rbx
00000001803B32A7 mov       rcx, rax
00000001803B32AA mov       rsi, rax
00000001803B32AD call      0x18131bcf0
00000001803B32B2 movaps    xmm0, xmm8
00000001803B32B6 addss     xmm0, xmm8
00000001803B32BB comiss    xmm9, xmm0
00000001803B32BF ja        0x1803b34da
00000001803B32C5 test      rsi, rsi
00000001803B32C8 je        0x1803b3539
00000001803B32CE xor       edx, edx
00000001803B32D0 mov       rcx, rsi
00000001803B32D3 call      0x18131c070
00000001803B32D8 mov       rdx, qword ptr [rdi + 0x30]
00000001803B32DC mov       rbx, rax
00000001803B32DF test      rdx, rdx
00000001803B32E2 je        0x1803b3539
00000001803B32E8 movaps    xmm7, xmm8
00000001803B32EC lea       rcx, [rsp + 0x30]
00000001803B32F1 xorps     xmm7, xmm13
00000001803B32F5 movaps    xmm6, xmm8
00000001803B32F9 mulss     xmm7, xmm11
00000001803B32FE xor       r8d, r8d
00000001803B3301 mulss     xmm6, xmm10
00000001803B3306 mulss     xmm7, xmm10
00000001803B330B subss     xmm6, xmm9
00000001803B3310 call      0x18132f3a0
00000001803B3315 movsd     xmm0, qword ptr [rax]
00000001803B3319 movss     xmm3, dword ptr [rax + 8]
00000001803B331E movaps    xmm1, xmm0
00000001803B3321 shufps    xmm1, xmm1, 0x55
00000001803B3325 addss     xmm3, xmm12
00000001803B332A movaps    xmm2, xmm0
00000001803B332D movsd     qword ptr [rsp + 0x40], xmm0
00000001803B3333 addss     xmm1, xmm6
00000001803B3337 addss     xmm2, xmm7
00000001803B333B test      rbx, rbx
00000001803B333E je        0x1803b3539
00000001803B3344 unpcklps  xmm2, xmm1
00000001803B3347 lea       rdx, [rsp + 0x20]
00000001803B334C xor       r8d, r8d
00000001803B334F movsd     qword ptr [rsp + 0x20], xmm2
00000001803B3355 mov       rcx, rbx
00000001803B3358 movss     dword ptr [rsp + 0x28], xmm3
00000001803B335E call      0x18132fbb0
00000001803B3363 mov       rdx, qword ptr [rip + 0x17f100e] # 0x181ba4378
00000001803B336A mov       rcx, rsi
00000001803B336D call      0x18046d280
00000001803B3372 mov       rbx, rax
00000001803B3375 test      rax, rax
00000001803B3378 je        0x1803b3539
00000001803B337E xor       edx, edx
00000001803B3380 mov       rcx, rax
00000001803B3383 call      0x1811b84f0
00000001803B3388 test      rax, rax
00000001803B338B je        0x1803b3539
00000001803B3391 movaps    xmm0, xmm12
00000001803B3395 xor       r8d, r8d
00000001803B3398 unpcklps  xmm0, xmm15
00000001803B339C mov       rcx, rax
00000001803B339F movq      rdx, xmm0
00000001803B33A4 call      0x1813252d0
00000001803B33A9 xor       r8d, r8d
00000001803B33AC xor       edx, edx
00000001803B33AE mov       rcx, rbx
00000001803B33B1 call      0x1811b9000
00000001803B33B6 xor       r8d, r8d
00000001803B33B9 mov       dl, 1
00000001803B33BB mov       rcx, rbx
00000001803B33BE call      0x1811b9050
00000001803B33C3 xor       r8d, r8d
00000001803B33C6 mov       dl, 1
00000001803B33C8 mov       rcx, rbx
00000001803B33CB call      0x1811b96b0
00000001803B33D0 movd      xmm1, dword ptr [rsp + 0x110]
00000001803B33D9 xor       r8d, r8d
00000001803B33DC cvtdq2ps  xmm1, xmm1
00000001803B33DF mov       rcx, rbx
00000001803B33E2 call      0x1811b9410
00000001803B33E7 xor       edx, edx
00000001803B33E9 lea       rcx, [rsp + 0x110]
00000001803B33F1 call      0x180ca3820
00000001803B33F6 mov       rdx, qword ptr [rip + 0x1820c3b] # 0x181bd4038
00000001803B33FD xor       r8d, r8d
00000001803B3400 mov       rcx, rax
00000001803B3403 call      0x180b74270
00000001803B3408 mov       r9, qword ptr [rbx]
00000001803B340B mov       rdx, rax
00000001803B340E mov       rcx, rbx
00000001803B3411 mov       r8, qword ptr [r9 + 0x560]
00000001803B3418 call      qword ptr [r9 + 0x558]
00000001803B341F mov       dword ptr [rsp + 0x120], 0xffffffff
00000001803B342A mov       eax, dword ptr [rsp + 0x120]
00000001803B3431 mov       ecx, eax
00000001803B3433 shr       ecx, 8
00000001803B3436 movzx     ecx, cl
00000001803B3439 movd      xmm3, ecx
00000001803B343D mov       ecx, eax
00000001803B343F shr       ecx, 0x10
00000001803B3442 movzx     ecx, cl
00000001803B3445 shr       eax, 0x18
00000001803B3448 cvtdq2ps  xmm3, xmm3
00000001803B344B movd      xmm1, ecx
00000001803B344F movd      xmm2, eax
00000001803B3453 cvtdq2ps  xmm1, xmm1
00000001803B3456 cvtdq2ps  xmm2, xmm2
00000001803B3459 divss     xmm1, xmm14
00000001803B345E divss     xmm2, xmm14
00000001803B3463 divss     xmm3, xmm14
00000001803B3468 mov       rax, qword ptr [rbx]
00000001803B346B lea       rdx, [rsp + 0x40]
00000001803B3470 mov       dword ptr [rsp + 0x50], 0x3f800000
00000001803B3478 mov       rcx, rbx
00000001803B347B movaps    xmm0, xmmword ptr [rsp + 0x50]
00000001803B3480 shufps    xmm0, xmm0, 0xe1
00000001803B3484 mov       r8, qword ptr [rax + 0x2b0]
00000001803B348B movss     xmm0, xmm3
00000001803B348F shufps    xmm0, xmm0, 0xc6
00000001803B3493 movss     xmm0, xmm1
00000001803B3497 shufps    xmm0, xmm0, 0x27
00000001803B349B movss     xmm0, xmm2
00000001803B349F shufps    xmm0, xmm0, 0x39
00000001803B34A3 movaps    xmmword ptr [rsp + 0x50], xmm0
00000001803B34A8 movdqa    xmmword ptr [rsp + 0x40], xmm0
00000001803B34AE call      qword ptr [rax + 0x2a8]
00000001803B34B4 mov       eax, dword ptr [rsp + 0x110]
00000001803B34BB movd      xmm0, eax
00000001803B34BF cvtdq2ps  xmm0, xmm0
00000001803B34C2 addss     xmm9, xmm0
00000001803B34C7 add       eax, dword ptr [rdi + 0x2c]
00000001803B34CA mov       dword ptr [rsp + 0x110], eax
00000001803B34D1 cmp       eax, dword ptr [rdi + 0x28]
00000001803B34D4 jle       0x1803b3260
00000001803B34DA movaps    xmm15, xmmword ptr [rsp + 0x60]
00000001803B34E0 lea       r11, [rsp + 0x100]
00000001803B34E8 mov       rbx, qword ptr [r11 + 0x28]
00000001803B34EC movaps    xmm8, xmmword ptr [r11 - 0x30]
00000001803B34F1 movaps    xmm9, xmmword ptr [r11 - 0x40]
00000001803B34F6 movaps    xmm12, xmmword ptr [r11 - 0x70]
00000001803B34FB movaps    xmm14, xmmword ptr [rsp + 0x70]
00000001803B3501 movaps    xmm13, xmmword ptr [rsp + 0x80]
00000001803B350A movaps    xmm11, xmmword ptr [rsp + 0xa0]
00000001803B3513 movaps    xmm10, xmmword ptr [rsp + 0xb0]
00000001803B351C movaps    xmm7, xmmword ptr [rsp + 0xe0]
00000001803B3524 movaps    xmm6, xmmword ptr [rsp + 0xf0]
00000001803B352C mov       rsi, qword ptr [rsp + 0x118]
00000001803B3534 mov       rsp, r11
00000001803B3537 pop       rdi
00000001803B3538 ret       
00000001803B3539 call      0x180250150
00000001803B353E int3      
00000001803B353F int3      