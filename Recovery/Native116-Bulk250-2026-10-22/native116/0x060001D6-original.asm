0000000180320090 mov       qword ptr [rsp + 0x20], rbx
0000000180320095 push      rsi
0000000180320096 sub       rsp, 0x80
000000018032009D cmp       byte ptr [rip + 0x19c4b2e], 0 # 0x181ce4bd2
00000001803200A4 mov       rbx, rcx
00000001803200A7 jne       0x1803200c8
00000001803200A9 lea       rcx, [rip + 0x188f4c0] # 0x181baf570
00000001803200B0 call      0x18024fef0
00000001803200B5 lea       rcx, [rip + 0x1890444] # 0x181bb0500
00000001803200BC call      0x18024fef0
00000001803200C1 mov       byte ptr [rip + 0x19c4b0a], 1 # 0x181ce4bd2
00000001803200C8 mov       rsi, qword ptr [rbx + 0x28]
00000001803200CC mov       qword ptr [rsp + 0x90], rbp
00000001803200D4 mov       qword ptr [rsp + 0x98], rdi
00000001803200DC movaps    xmmword ptr [rsp + 0x70], xmm6
00000001803200E1 test      rsi, rsi
00000001803200E4 je        0x180320437
00000001803200EA mov       rsi, qword ptr [rsi + 0xc8]
00000001803200F1 test      rsi, rsi
00000001803200F4 je        0x180320437
00000001803200FA mov       rbp, qword ptr [rsi + 0x28]
00000001803200FE xor       edx, edx
0000000180320100 mov       rcx, rbx
0000000180320103 call      0x18031ea60
0000000180320108 mov       rcx, qword ptr [rip + 0x18903f1] # 0x181bb0500
000000018032010F mov       rdi, rax
0000000180320112 cmp       dword ptr [rcx + 0xe0], 0
0000000180320119 jne       0x180320120
000000018032011B call      0x1802501e0
0000000180320120 xor       r8d, r8d
0000000180320123 xor       edx, edx
0000000180320125 mov       rcx, rdi
0000000180320128 call      0x18131f760
000000018032012D test      al, al
000000018032012F jne       0x180320182
0000000180320131 mov       rax, qword ptr [rbx + 0x28]
0000000180320135 test      rax, rax
0000000180320138 je        0x180320437
000000018032013E cmp       byte ptr [rax + 0xd0], 0
0000000180320145 jne       0x1803201ab
0000000180320147 xor       r9d, r9d
000000018032014A mov       r8b, 1
000000018032014D mov       rdx, rdi
0000000180320150 mov       rcx, rax
0000000180320153 call      0x180328190
0000000180320158 mov       rcx, qword ptr [rbx + 0x28]
000000018032015C test      rcx, rcx
000000018032015F je        0x180320437
0000000180320165 mov       rcx, qword ptr [rcx + 0xc0]
000000018032016C test      rcx, rcx
000000018032016F je        0x180320437
0000000180320175 xor       r8d, r8d
0000000180320178 mov       edx, 3
000000018032017D call      0x180314840
0000000180320182 mov       rax, rdi
0000000180320185 movaps    xmm6, xmmword ptr [rsp + 0x70]
000000018032018A mov       rdi, qword ptr [rsp + 0x98]
0000000180320192 mov       rbp, qword ptr [rsp + 0x90]
000000018032019A mov       rbx, qword ptr [rsp + 0xa8]
00000001803201A2 add       rsp, 0x80
00000001803201A9 pop       rsi
00000001803201AA ret       
00000001803201AB mov       rcx, qword ptr [rip + 0x189034e] # 0x181bb0500
00000001803201B2 cmp       dword ptr [rcx + 0xe0], 0
00000001803201B9 jne       0x1803201c0
00000001803201BB call      0x1802501e0
00000001803201C0 xor       r8d, r8d
00000001803201C3 mov       rdx, rdi
00000001803201C6 mov       rcx, rbp
00000001803201C9 call      0x18131f760
00000001803201CE test      al, al
00000001803201D0 jne       0x18032041a
00000001803201D6 mov       rcx, qword ptr [rsi + 0x50]
00000001803201DA movsd     xmm0, qword ptr [rbx + 0x5c]
00000001803201DF mov       ebp, dword ptr [rbx + 0x64]
00000001803201E2 movsd     qword ptr [rsp + 0x20], xmm0
00000001803201E8 test      rcx, rcx
00000001803201EB je        0x180320437
00000001803201F1 xor       edx, edx
00000001803201F3 call      0x181304510
00000001803201F8 test      rax, rax
00000001803201FB je        0x180320437
0000000180320201 mov       qword ptr [rsp + 0xa0], r14
0000000180320209 lea       rcx, [rsp + 0x40]
000000018032020E movaps    xmmword ptr [rsp + 0x60], xmm7
0000000180320213 xor       r8d, r8d
0000000180320216 mov       rdx, rax
0000000180320219 movaps    xmmword ptr [rsp + 0x50], xmm8
000000018032021F call      0x18132f3a0
0000000180320224 cmp       byte ptr [rip + 0x19c490a], 0 # 0x181ce4b35
000000018032022B movsd     xmm0, qword ptr [rax]
000000018032022F mov       r14d, dword ptr [rax + 8]
0000000180320233 movsd     qword ptr [rsp + 0x30], xmm0
0000000180320239 jne       0x18032024e
000000018032023B lea       rcx, [rip + 0x188c9b6] # 0x181bacbf8
0000000180320242 call      0x18024fef0
0000000180320247 mov       byte ptr [rip + 0x19c48e7], 1 # 0x181ce4b35
000000018032024E mov       rcx, qword ptr [rip + 0x188c9a3] # 0x181bacbf8
0000000180320255 movss     xmm7, dword ptr [rsp + 0x20]
000000018032025B movss     xmm6, dword ptr [rsp + 0x24]
0000000180320261 subss     xmm7, dword ptr [rsp + 0x30]
0000000180320267 cmp       dword ptr [rcx + 0xe0], 0
000000018032026E subss     xmm6, dword ptr [rsp + 0x34]
0000000180320274 movd      xmm0, r14d
0000000180320279 mov       r14, qword ptr [rsp + 0xa0]
0000000180320281 movd      xmm8, ebp
0000000180320286 subss     xmm8, xmm0
000000018032028B jne       0x180320292
000000018032028D call      0x1802501e0
0000000180320292 mulss     xmm6, xmm6
0000000180320296 xorps     xmm0, xmm0
0000000180320299 mulss     xmm7, xmm7
000000018032029D mulss     xmm8, xmm8
00000001803202A2 addss     xmm6, xmm7
00000001803202A6 movaps    xmm7, xmmword ptr [rsp + 0x60]
00000001803202AB addss     xmm6, xmm8
00000001803202B0 movaps    xmm8, xmmword ptr [rsp + 0x50]
00000001803202B6 cvtps2pd  xmm1, xmm6
00000001803202B9 ucomisd   xmm0, xmm1
00000001803202BD ja        0x1803202c5
00000001803202BF sqrtpd    xmm0, xmm1
00000001803202C3 jmp       0x1803202cd
00000001803202C5 movaps    xmm0, xmm1
00000001803202C8 call      0x1802cb7b4
00000001803202CD xorps     xmm1, xmm1
00000001803202D0 cvtsd2ss  xmm1, xmm0
00000001803202D4 movss     xmm0, dword ptr [rip + 0x1287858] # 0x1815a7b34
00000001803202DC comiss    xmm0, xmm1
00000001803202DF ja        0x180320413
00000001803202E5 mov       rcx, qword ptr [rsi + 0x58]
00000001803202E9 movsd     xmm6, qword ptr [rbx + 0x5c]
00000001803202EE mov       ebp, dword ptr [rbx + 0x64]
00000001803202F1 test      rcx, rcx
00000001803202F4 je        0x180320437
00000001803202FA mov       r8, qword ptr [rip + 0x188f26f] # 0x181baf570
0000000180320301 xor       edx, edx
0000000180320303 call      0x180825710
0000000180320308 test      rax, rax
000000018032030B je        0x180320437
0000000180320311 xor       edx, edx
0000000180320313 mov       rcx, rax
0000000180320316 call      0x181304510
000000018032031B test      rax, rax
000000018032031E je        0x180320437
0000000180320324 xor       r8d, r8d
0000000180320327 lea       rcx, [rsp + 0x40]
000000018032032C mov       rdx, rax
000000018032032F call      0x18132f3a0
0000000180320334 xor       r8d, r8d
0000000180320337 movsd     qword ptr [rsp + 0x20], xmm6
000000018032033D lea       rdx, [rsp + 0x30]
0000000180320342 mov       dword ptr [rsp + 0x28], ebp
0000000180320346 lea       rcx, [rsp + 0x20]
000000018032034B movsd     xmm0, qword ptr [rax]
000000018032034F mov       eax, dword ptr [rax + 8]
0000000180320352 movsd     qword ptr [rsp + 0x30], xmm0
0000000180320358 mov       dword ptr [rsp + 0x38], eax
000000018032035C call      0x1802fb1b0
0000000180320361 movss     xmm1, dword ptr [rip + 0x12877b3] # 0x1815a7b1c
0000000180320369 comiss    xmm1, xmm0
000000018032036C ja        0x180320413
0000000180320372 mov       rcx, qword ptr [rsi + 0x48]
0000000180320376 movsd     xmm6, qword ptr [rbx + 0x5c]
000000018032037B mov       ebp, dword ptr [rbx + 0x64]
000000018032037E test      rcx, rcx
0000000180320381 je        0x180320437
0000000180320387 xor       edx, edx
0000000180320389 call      0x181304510
000000018032038E test      rax, rax
0000000180320391 je        0x180320437
0000000180320397 xor       r8d, r8d
000000018032039A lea       rcx, [rsp + 0x40]
000000018032039F mov       rdx, rax
00000001803203A2 call      0x18132f3a0
00000001803203A7 xor       r8d, r8d
00000001803203AA movsd     qword ptr [rsp + 0x20], xmm6
00000001803203B0 lea       rdx, [rsp + 0x30]
00000001803203B5 mov       dword ptr [rsp + 0x28], ebp
00000001803203B9 lea       rcx, [rsp + 0x20]
00000001803203BE movsd     xmm0, qword ptr [rax]
00000001803203C2 mov       eax, dword ptr [rax + 8]
00000001803203C5 movsd     qword ptr [rsp + 0x30], xmm0
00000001803203CB mov       dword ptr [rsp + 0x38], eax
00000001803203CF call      0x1802fb1b0
00000001803203D4 movss     xmm1, dword ptr [rip + 0x1287738] # 0x1815a7b14
00000001803203DC comiss    xmm1, xmm0
00000001803203DF ja        0x180320413
00000001803203E1 mov       rcx, qword ptr [rbx + 0x28]
00000001803203E5 test      rcx, rcx
00000001803203E8 je        0x180320437
00000001803203EA xor       r9d, r9d
00000001803203ED xor       r8d, r8d
00000001803203F0 mov       dl, 1
00000001803203F2 call      0x1803265e0
00000001803203F7 mov       rcx, qword ptr [rbx + 0x28]
00000001803203FB test      rcx, rcx
00000001803203FE je        0x180320437
0000000180320400 xor       r9d, r9d
0000000180320403 xor       r8d, r8d
0000000180320406 mov       rdx, rdi
0000000180320409 call      0x180328190
000000018032040E jmp       0x180320182
0000000180320413 xor       eax, eax
0000000180320415 jmp       0x180320185
000000018032041A mov       rcx, qword ptr [rbx + 0x28]
000000018032041E test      rcx, rcx
0000000180320421 je        0x180320437
0000000180320423 mov       r8b, 1
0000000180320426 xor       r9d, r9d
0000000180320429 movzx     edx, r8b
000000018032042D call      0x1803265e0
0000000180320432 jmp       0x180320182
0000000180320437 call      0x180250150
000000018032043C int3      
000000018032043D int3      
000000018032043E int3      
000000018032043F int3      