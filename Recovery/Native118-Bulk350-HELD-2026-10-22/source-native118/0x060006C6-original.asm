00000001803A2E80 push      rbp
00000001803A2E82 push      rdi
00000001803A2E83 push      r14
00000001803A2E85 mov       rbp, rsp
00000001803A2E88 sub       rsp, 0x70
00000001803A2E8C cmp       byte ptr [rip + 0x1942083], 0 # 0x181ce4f16
00000001803A2E93 mov       rdi, rdx
00000001803A2E96 mov       r14, rcx
00000001803A2E99 jne       0x1803a2ed2
00000001803A2E9B lea       rcx, [rip + 0x181be66] # 0x181bbed08
00000001803A2EA2 call      0x18024fef0
00000001803A2EA7 lea       rcx, [rip + 0x18288ea] # 0x181bcb798
00000001803A2EAE call      0x18024fef0
00000001803A2EB3 lea       rcx, [rip + 0x181662e] # 0x181bb94e8
00000001803A2EBA call      0x18024fef0
00000001803A2EBF lea       rcx, [rip + 0x17f977a] # 0x181b9c640
00000001803A2EC6 call      0x18024fef0
00000001803A2ECB mov       byte ptr [rip + 0x1942044], 1 # 0x181ce4f16
00000001803A2ED2 mov       rcx, qword ptr [r14 + 0x10]
00000001803A2ED6 mov       qword ptr [rsp + 0xa0], rbx
00000001803A2EDE mov       qword ptr [rsp + 0x68], rsi
00000001803A2EE3 mov       qword ptr [rsp + 0x60], r12
00000001803A2EE8 mov       qword ptr [rsp + 0x58], r13
00000001803A2EED mov       qword ptr [rsp + 0x50], r15
00000001803A2EF2 test      rcx, rcx
00000001803A2EF5 je        0x1803a3485
00000001803A2EFB xor       r8d, r8d
00000001803A2EFE mov       rdx, rdi
00000001803A2F01 call      0x180344520
00000001803A2F06 mov       rcx, qword ptr [r14 + 0x38]
00000001803A2F0A test      rcx, rcx
00000001803A2F0D je        0x1803a3485
00000001803A2F13 mov       r8, qword ptr [rip + 0x181bdee] # 0x181bbed08
00000001803A2F1A xor       edx, edx
00000001803A2F1C call      0x180825710
00000001803A2F21 test      rdi, rdi
00000001803A2F24 je        0x1803a3485
00000001803A2F2A test      rax, rax
00000001803A2F2D je        0x1803a3485
00000001803A2F33 mov       rdx, qword ptr [rdi + 0x10]
00000001803A2F37 xor       r9d, r9d
00000001803A2F3A mov       r8b, 1
00000001803A2F3D mov       rcx, rax
00000001803A2F40 call      0x1811af070
00000001803A2F45 movdqa    xmm0, xmmword ptr [rip + 0x1204fe3] # 0x1815a7f30
00000001803A2F4D lea       rcx, [rbp - 0x30]
00000001803A2F51 xor       edx, edx
00000001803A2F53 movaps    xmmword ptr [rbp - 0x30], xmm0
00000001803A2F57 call      0x181303320
00000001803A2F5C mov       rcx, qword ptr [r14 + 0x38]
00000001803A2F60 mov       r15, rax
00000001803A2F63 test      rcx, rcx
00000001803A2F66 je        0x1803a3485
00000001803A2F6C mov       r8, qword ptr [rip + 0x181bd95] # 0x181bbed08
00000001803A2F73 mov       edx, 1
00000001803A2F78 call      0x180825710
00000001803A2F7D mov       rcx, qword ptr [r14 + 0x10]
00000001803A2F81 mov       r12, rax
00000001803A2F84 test      rcx, rcx
00000001803A2F87 je        0x1803a3485
00000001803A2F8D mov       rcx, qword ptr [rcx + 0x58]
00000001803A2F91 test      rcx, rcx
00000001803A2F94 je        0x1803a3485
00000001803A2F9A mov       r13, qword ptr [rcx + 0x20]
00000001803A2F9E mov       edx, 0xa
00000001803A2FA3 mov       rcx, qword ptr [rip + 0x18287ee] # 0x181bcb798
00000001803A2FAA call      0x18024f3e0
00000001803A2FAF mov       rcx, qword ptr [rdi + 0x50]
00000001803A2FB3 mov       rbx, rax
00000001803A2FB6 test      rcx, rcx
00000001803A2FB9 je        0x1803a3485
00000001803A2FBF cmp       dword ptr [rcx + 0x18], 0
00000001803A2FC3 jbe       0x1803a352b
00000001803A2FC9 movss     xmm0, dword ptr [rcx + 0x20]
00000001803A2FCE lea       rdx, [rbp + 0x20]
00000001803A2FD2 mov       rcx, qword ptr [rip + 0x181650f] # 0x181bb94e8
00000001803A2FD9 movss     dword ptr [rbp + 0x20], xmm0
00000001803A2FDE call      0x18024f340
00000001803A2FE3 mov       rsi, rax
00000001803A2FE6 test      rbx, rbx
00000001803A2FE9 je        0x1803a3485
00000001803A2FEF test      rax, rax
00000001803A2FF2 je        0x1803a300c
00000001803A2FF4 mov       rdx, qword ptr [rbx]
00000001803A2FF7 mov       rcx, rax
00000001803A2FFA mov       rdx, qword ptr [rdx + 0x40]
00000001803A2FFE call      0x18024f380
00000001803A3003 test      rax, rax
00000001803A3006 je        0x1803a348b
00000001803A300C cmp       dword ptr [rbx + 0x18], 0
00000001803A3010 jbe       0x1803a352b
00000001803A3016 lea       rcx, [rbx + 0x20]
00000001803A301A mov       qword ptr [rbx + 0x20], rsi
00000001803A301E mov       rdx, rsi
00000001803A3021 call      0x18024f360
00000001803A3026 mov       rax, qword ptr [rdi + 0x50]
00000001803A302A test      rax, rax
00000001803A302D je        0x1803a3485
00000001803A3033 cmp       dword ptr [rax + 0x18], 1
00000001803A3037 jbe       0x1803a352b
00000001803A303D movss     xmm0, dword ptr [rax + 0x24]
00000001803A3042 lea       rdx, [rbp + 0x28]
00000001803A3046 mov       rcx, qword ptr [rip + 0x181649b] # 0x181bb94e8
00000001803A304D movss     dword ptr [rbp + 0x28], xmm0
00000001803A3052 call      0x18024f340
00000001803A3057 mov       rsi, rax
00000001803A305A test      rax, rax
00000001803A305D je        0x1803a3077
00000001803A305F mov       rdx, qword ptr [rbx]
00000001803A3062 mov       rcx, rax
00000001803A3065 mov       rdx, qword ptr [rdx + 0x40]
00000001803A3069 call      0x18024f380
00000001803A306E test      rax, rax
00000001803A3071 je        0x1803a349b
00000001803A3077 cmp       dword ptr [rbx + 0x18], 1
00000001803A307B jbe       0x1803a352b
00000001803A3081 lea       rcx, [rbx + 0x28]
00000001803A3085 mov       qword ptr [rbx + 0x28], rsi
00000001803A3089 mov       rdx, rsi
00000001803A308C call      0x18024f360
00000001803A3091 mov       rax, qword ptr [rdi + 0x50]
00000001803A3095 test      rax, rax
00000001803A3098 je        0x1803a3485
00000001803A309E cmp       dword ptr [rax + 0x18], 2
00000001803A30A2 jbe       0x1803a352b
00000001803A30A8 movss     xmm0, dword ptr [rax + 0x28]
00000001803A30AD lea       rdx, [rbp + 0x38]
00000001803A30B1 mov       rcx, qword ptr [rip + 0x1816430] # 0x181bb94e8
00000001803A30B8 movss     dword ptr [rbp + 0x38], xmm0
00000001803A30BD call      0x18024f340
00000001803A30C2 mov       rsi, rax
00000001803A30C5 test      rax, rax
00000001803A30C8 je        0x1803a30e2
00000001803A30CA mov       rdx, qword ptr [rbx]
00000001803A30CD mov       rcx, rax
00000001803A30D0 mov       rdx, qword ptr [rdx + 0x40]
00000001803A30D4 call      0x18024f380
00000001803A30D9 test      rax, rax
00000001803A30DC je        0x1803a34ab
00000001803A30E2 cmp       dword ptr [rbx + 0x18], 2
00000001803A30E6 jbe       0x1803a352b
00000001803A30EC lea       rcx, [rbx + 0x30]
00000001803A30F0 mov       qword ptr [rbx + 0x30], rsi
00000001803A30F4 mov       rdx, rsi
00000001803A30F7 call      0x18024f360
00000001803A30FC mov       rax, qword ptr [rdi + 0x50]
00000001803A3100 test      rax, rax
00000001803A3103 je        0x1803a3485
00000001803A3109 cmp       dword ptr [rax + 0x18], 3
00000001803A310D jbe       0x1803a352b
00000001803A3113 movss     xmm0, dword ptr [rax + 0x2c]
00000001803A3118 lea       rdx, [rbp - 0x50]
00000001803A311C mov       rcx, qword ptr [rip + 0x18163c5] # 0x181bb94e8
00000001803A3123 movss     dword ptr [rbp - 0x50], xmm0
00000001803A3128 call      0x18024f340
00000001803A312D mov       rsi, rax
00000001803A3130 test      rax, rax
00000001803A3133 je        0x1803a314d
00000001803A3135 mov       rdx, qword ptr [rbx]
00000001803A3138 mov       rcx, rax
00000001803A313B mov       rdx, qword ptr [rdx + 0x40]
00000001803A313F call      0x18024f380
00000001803A3144 test      rax, rax
00000001803A3147 je        0x1803a34bb
00000001803A314D cmp       dword ptr [rbx + 0x18], 3
00000001803A3151 jbe       0x1803a352b
00000001803A3157 lea       rcx, [rbx + 0x38]
00000001803A315B mov       qword ptr [rbx + 0x38], rsi
00000001803A315F mov       rdx, rsi
00000001803A3162 call      0x18024f360
00000001803A3167 mov       rax, qword ptr [rdi + 0x50]
00000001803A316B test      rax, rax
00000001803A316E je        0x1803a3485
00000001803A3174 cmp       dword ptr [rax + 0x18], 4
00000001803A3178 jbe       0x1803a352b
00000001803A317E movss     xmm0, dword ptr [rax + 0x30]
00000001803A3183 lea       rdx, [rbp - 0x4c]
00000001803A3187 mov       rcx, qword ptr [rip + 0x181635a] # 0x181bb94e8
00000001803A318E movss     dword ptr [rbp - 0x4c], xmm0
00000001803A3193 call      0x18024f340
00000001803A3198 mov       rsi, rax
00000001803A319B test      rax, rax
00000001803A319E je        0x1803a31b8
00000001803A31A0 mov       rdx, qword ptr [rbx]
00000001803A31A3 mov       rcx, rax
00000001803A31A6 mov       rdx, qword ptr [rdx + 0x40]
00000001803A31AA call      0x18024f380
00000001803A31AF test      rax, rax
00000001803A31B2 je        0x1803a34cb
00000001803A31B8 cmp       dword ptr [rbx + 0x18], 4
00000001803A31BC jbe       0x1803a352b
00000001803A31C2 lea       rcx, [rbx + 0x40]
00000001803A31C6 mov       qword ptr [rbx + 0x40], rsi
00000001803A31CA mov       rdx, rsi
00000001803A31CD call      0x18024f360
00000001803A31D2 mov       rax, qword ptr [rdi + 0x50]
00000001803A31D6 test      rax, rax
00000001803A31D9 je        0x1803a3485
00000001803A31DF cmp       dword ptr [rax + 0x18], 5
00000001803A31E3 jbe       0x1803a352b
00000001803A31E9 movss     xmm0, dword ptr [rax + 0x34]
00000001803A31EE lea       rdx, [rbp - 0x48]
00000001803A31F2 mov       rcx, qword ptr [rip + 0x18162ef] # 0x181bb94e8
00000001803A31F9 movss     dword ptr [rbp - 0x48], xmm0
00000001803A31FE call      0x18024f340
00000001803A3203 mov       rsi, rax
00000001803A3206 test      rax, rax
00000001803A3209 je        0x1803a3223
00000001803A320B mov       rdx, qword ptr [rbx]
00000001803A320E mov       rcx, rax
00000001803A3211 mov       rdx, qword ptr [rdx + 0x40]
00000001803A3215 call      0x18024f380
00000001803A321A test      rax, rax
00000001803A321D je        0x1803a34db
00000001803A3223 cmp       dword ptr [rbx + 0x18], 5
00000001803A3227 jbe       0x1803a352b
00000001803A322D lea       rcx, [rbx + 0x48]
00000001803A3231 mov       qword ptr [rbx + 0x48], rsi
00000001803A3235 mov       rdx, rsi
00000001803A3238 call      0x18024f360
00000001803A323D mov       rax, qword ptr [rdi + 0x50]
00000001803A3241 test      rax, rax
00000001803A3244 je        0x1803a3485
00000001803A324A cmp       dword ptr [rax + 0x18], 6
00000001803A324E jbe       0x1803a352b
00000001803A3254 movss     xmm0, dword ptr [rax + 0x38]
00000001803A3259 lea       rdx, [rbp - 0x44]
00000001803A325D mov       rcx, qword ptr [rip + 0x1816284] # 0x181bb94e8
00000001803A3264 movss     dword ptr [rbp - 0x44], xmm0
00000001803A3269 call      0x18024f340
00000001803A326E mov       rsi, rax
00000001803A3271 test      rax, rax
00000001803A3274 je        0x1803a328e
00000001803A3276 mov       rdx, qword ptr [rbx]
00000001803A3279 mov       rcx, rax
00000001803A327C mov       rdx, qword ptr [rdx + 0x40]
00000001803A3280 call      0x18024f380
00000001803A3285 test      rax, rax
00000001803A3288 je        0x1803a34eb
00000001803A328E cmp       dword ptr [rbx + 0x18], 6
00000001803A3292 jbe       0x1803a352b
00000001803A3298 lea       rcx, [rbx + 0x50]
00000001803A329C mov       qword ptr [rbx + 0x50], rsi
00000001803A32A0 mov       rdx, rsi
00000001803A32A3 call      0x18024f360
00000001803A32A8 mov       rax, qword ptr [rdi + 0x50]
00000001803A32AC test      rax, rax
00000001803A32AF je        0x1803a3485
00000001803A32B5 cmp       dword ptr [rax + 0x18], 7
00000001803A32B9 jbe       0x1803a352b
00000001803A32BF movss     xmm0, dword ptr [rax + 0x3c]
00000001803A32C4 lea       rdx, [rbp - 0x40]
00000001803A32C8 mov       rcx, qword ptr [rip + 0x1816219] # 0x181bb94e8
00000001803A32CF movss     dword ptr [rbp - 0x40], xmm0
00000001803A32D4 call      0x18024f340
00000001803A32D9 mov       rsi, rax
00000001803A32DC test      rax, rax
00000001803A32DF je        0x1803a32f9
00000001803A32E1 mov       rdx, qword ptr [rbx]
00000001803A32E4 mov       rcx, rax
00000001803A32E7 mov       rdx, qword ptr [rdx + 0x40]
00000001803A32EB call      0x18024f380
00000001803A32F0 test      rax, rax
00000001803A32F3 je        0x1803a34fb
00000001803A32F9 cmp       dword ptr [rbx + 0x18], 7
00000001803A32FD jbe       0x1803a352b
00000001803A3303 lea       rcx, [rbx + 0x58]
00000001803A3307 mov       qword ptr [rbx + 0x58], rsi
00000001803A330B mov       rdx, rsi
00000001803A330E call      0x18024f360
00000001803A3313 mov       rax, qword ptr [rdi + 0x50]
00000001803A3317 test      rax, rax
00000001803A331A je        0x1803a3485
00000001803A3320 cmp       dword ptr [rax + 0x18], 8
00000001803A3324 jbe       0x1803a352b
00000001803A332A movss     xmm0, dword ptr [rax + 0x40]
00000001803A332F lea       rdx, [rbp - 0x3c]
00000001803A3333 mov       rcx, qword ptr [rip + 0x18161ae] # 0x181bb94e8
00000001803A333A movss     dword ptr [rbp - 0x3c], xmm0
00000001803A333F call      0x18024f340
00000001803A3344 mov       rsi, rax
00000001803A3347 test      rax, rax
00000001803A334A je        0x1803a3364
00000001803A334C mov       rdx, qword ptr [rbx]
00000001803A334F mov       rcx, rax
00000001803A3352 mov       rdx, qword ptr [rdx + 0x40]
00000001803A3356 call      0x18024f380
00000001803A335B test      rax, rax
00000001803A335E je        0x1803a350b
00000001803A3364 cmp       dword ptr [rbx + 0x18], 8
00000001803A3368 jbe       0x1803a352b
00000001803A336E lea       rcx, [rbx + 0x60]
00000001803A3372 mov       qword ptr [rbx + 0x60], rsi
00000001803A3376 mov       rdx, rsi
00000001803A3379 call      0x18024f360
00000001803A337E test      r15, r15
00000001803A3381 je        0x1803a339b
00000001803A3383 mov       rdx, qword ptr [rbx]
00000001803A3386 mov       rcx, r15
00000001803A3389 mov       rdx, qword ptr [rdx + 0x40]
00000001803A338D call      0x18024f380
00000001803A3392 test      rax, rax
00000001803A3395 je        0x1803a351b
00000001803A339B cmp       dword ptr [rbx + 0x18], 9
00000001803A339F jbe       0x1803a352b
00000001803A33A5 lea       rcx, [rbx + 0x68]
00000001803A33A9 mov       qword ptr [rbx + 0x68], r15
00000001803A33AD mov       rdx, r15
00000001803A33B0 call      0x18024f360
00000001803A33B5 xor       r8d, r8d
00000001803A33B8 mov       rdx, rbx
00000001803A33BB mov       rcx, r13
00000001803A33BE call      0x180b76d10
00000001803A33C3 test      r12, r12
00000001803A33C6 je        0x1803a3485
00000001803A33CC xor       r9d, r9d
00000001803A33CF mov       r8b, 1
00000001803A33D2 mov       rdx, rax
00000001803A33D5 mov       rcx, r12
00000001803A33D8 call      0x1811af070
00000001803A33DD mov       rcx, qword ptr [r14 + 0x38]
00000001803A33E1 test      rcx, rcx
00000001803A33E4 je        0x1803a3485
00000001803A33EA mov       r8, qword ptr [rip + 0x181b917] # 0x181bbed08
00000001803A33F1 mov       edx, 2
00000001803A33F6 call      0x180825710
00000001803A33FB movss     xmm0, dword ptr [rdi + 0x40]
00000001803A3400 lea       rdx, [rbp - 0x38]
00000001803A3404 mov       rcx, qword ptr [rip + 0x18160dd] # 0x181bb94e8
00000001803A340B mov       rsi, rax
00000001803A340E movss     dword ptr [rbp - 0x38], xmm0
00000001803A3413 call      0x18024f340
00000001803A3418 movss     xmm0, dword ptr [rdi + 0x44]
00000001803A341D lea       rdx, [rbp - 0x34]
00000001803A3421 mov       rcx, qword ptr [rip + 0x18160c0] # 0x181bb94e8
00000001803A3428 mov       rbx, rax
00000001803A342B movss     dword ptr [rbp - 0x34], xmm0
00000001803A3430 call      0x18024f340
00000001803A3435 mov       rcx, qword ptr [rip + 0x17f9204] # 0x181b9c640
00000001803A343C xor       r9d, r9d
00000001803A343F mov       r8, rax
00000001803A3442 mov       rdx, rbx
00000001803A3445 call      0x180b772a0
00000001803A344A test      rsi, rsi
00000001803A344D je        0x1803a3485
00000001803A344F xor       r9d, r9d
00000001803A3452 mov       r8b, 1
00000001803A3455 mov       rdx, rax
00000001803A3458 mov       rcx, rsi
00000001803A345B call      0x1811af070
00000001803A3460 mov       r15, qword ptr [rsp + 0x50]
00000001803A3465 mov       r13, qword ptr [rsp + 0x58]
00000001803A346A mov       r12, qword ptr [rsp + 0x60]
00000001803A346F mov       rsi, qword ptr [rsp + 0x68]
00000001803A3474 mov       rbx, qword ptr [rsp + 0xa0]
00000001803A347C add       rsp, 0x70
00000001803A3480 pop       r14
00000001803A3482 pop       rdi
00000001803A3483 pop       rbp
00000001803A3484 ret       
00000001803A3485 call      0x180250150
00000001803A348A int3      
00000001803A348B call      0x18024fc50
00000001803A3490 mov       rcx, rax
00000001803A3493 xor       edx, edx
00000001803A3495 call      0x180250110
00000001803A349A int3      
00000001803A349B call      0x18024fc50
00000001803A34A0 mov       rcx, rax
00000001803A34A3 xor       edx, edx
00000001803A34A5 call      0x180250110
00000001803A34AA int3      
00000001803A34AB call      0x18024fc50
00000001803A34B0 mov       rcx, rax
00000001803A34B3 xor       edx, edx
00000001803A34B5 call      0x180250110
00000001803A34BA int3      
00000001803A34BB call      0x18024fc50
00000001803A34C0 mov       rcx, rax
00000001803A34C3 xor       edx, edx
00000001803A34C5 call      0x180250110
00000001803A34CA int3      
00000001803A34CB call      0x18024fc50
00000001803A34D0 mov       rcx, rax
00000001803A34D3 xor       edx, edx
00000001803A34D5 call      0x180250110
00000001803A34DA int3      
00000001803A34DB call      0x18024fc50
00000001803A34E0 mov       rcx, rax
00000001803A34E3 xor       edx, edx
00000001803A34E5 call      0x180250110
00000001803A34EA int3      
00000001803A34EB call      0x18024fc50
00000001803A34F0 mov       rcx, rax
00000001803A34F3 xor       edx, edx
00000001803A34F5 call      0x180250110
00000001803A34FA int3      
00000001803A34FB call      0x18024fc50
00000001803A3500 mov       rcx, rax
00000001803A3503 xor       edx, edx
00000001803A3505 call      0x180250110
00000001803A350A int3      
00000001803A350B call      0x18024fc50
00000001803A3510 mov       rcx, rax
00000001803A3513 xor       edx, edx
00000001803A3515 call      0x180250110
00000001803A351A int3      
00000001803A351B call      0x18024fc50
00000001803A3520 mov       rcx, rax
00000001803A3523 xor       edx, edx
00000001803A3525 call      0x180250110
00000001803A352A int3      
00000001803A352B call      0x180250140
00000001803A3530 int3      
00000001803A3531 int3      
00000001803A3532 int3      
00000001803A3533 int3      
00000001803A3534 int3      
00000001803A3535 int3      
00000001803A3536 int3      
00000001803A3537 int3      
00000001803A3538 int3      
00000001803A3539 int3      
00000001803A353A int3      
00000001803A353B int3      
00000001803A353C int3      
00000001803A353D int3      
00000001803A353E int3      
00000001803A353F int3      