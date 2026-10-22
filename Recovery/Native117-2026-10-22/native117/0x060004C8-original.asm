00000001803870D0 mov       qword ptr [rsp + 0x18], rbx
00000001803870D5 mov       qword ptr [rsp + 0x10], rdx
00000001803870DA mov       qword ptr [rsp + 8], rcx
00000001803870DF push      rsi
00000001803870E0 push      rdi
00000001803870E1 push      r14
00000001803870E3 sub       rsp, 0x80
00000001803870EA mov       rsi, rdx
00000001803870ED mov       rdi, rcx
00000001803870F0 cmp       byte ptr [rip + 0x195dcf4], 0 # 0x181ce4deb
00000001803870F7 jne       0x180387148
00000001803870F9 lea       rcx, [rip + 0x183de30] # 0x181bc4f30
0000000180387100 call      0x18024fef0
0000000180387105 lea       rcx, [rip + 0x183de7c] # 0x181bc4f88
000000018038710C call      0x18024fef0
0000000180387111 lea       rcx, [rip + 0x183dec8] # 0x181bc4fe0
0000000180387118 call      0x18024fef0
000000018038711D lea       rcx, [rip + 0x1828814] # 0x181baf938
0000000180387124 call      0x18024fef0
0000000180387129 lea       rcx, [rip + 0x1828910] # 0x181bafa40
0000000180387130 call      0x18024fef0
0000000180387135 lea       rcx, [rip + 0x18289b4] # 0x181bafaf0
000000018038713C call      0x18024fef0
0000000180387141 mov       byte ptr [rip + 0x195dca3], 1 # 0x181ce4deb
0000000180387148 movups    xmm0, xmmword ptr [rsi + 0x10]
000000018038714C movups    xmmword ptr [rsp + 0x30], xmm0
0000000180387151 movsd     xmm1, qword ptr [rsi + 0x20]
0000000180387156 movsd     qword ptr [rsp + 0x40], xmm1
000000018038715C mov       rdx, qword ptr [rsi]
000000018038715F mov       qword ptr [rdi], rdx
0000000180387162 mov       rcx, rdi
0000000180387165 call      0x18024f360
000000018038716A movups    xmm0, xmmword ptr [rsi + 0x10]
000000018038716E movups    xmmword ptr [rsp + 0x30], xmm0
0000000180387173 movsd     xmm1, qword ptr [rsi + 0x20]
0000000180387178 movsd     qword ptr [rsp + 0x40], xmm1
000000018038717E movq      xmm2, qword ptr [rsi + 8]
0000000180387183 movd      dword ptr [rdi + 8], xmm2
0000000180387188 mov       rcx, qword ptr [rdi + 0x10]
000000018038718C test      rcx, rcx
000000018038718F je        0x180387533
0000000180387195 inc       dword ptr [rcx + 0x1c]
0000000180387198 xor       r14d, r14d
000000018038719B mov       dword ptr [rcx + 0x18], r14d
000000018038719F movups    xmm0, xmmword ptr [rsi]
00000001803871A2 movups    xmmword ptr [rsp + 0x20], xmm0
00000001803871A7 movsd     qword ptr [rsp + 0x40], xmm1
00000001803871AD mov       rdx, qword ptr [rsi + 0x10]
00000001803871B1 test      rdx, rdx
00000001803871B4 je        0x180387533
00000001803871BA mov       r8, qword ptr [rip + 0x182892f] # 0x181bafaf0
00000001803871C1 lea       rcx, [rsp + 0x20]
00000001803871C6 call      0x18082ac10
00000001803871CB movups    xmm0, xmmword ptr [rsp + 0x20]
00000001803871D0 movups    xmmword ptr [rsp + 0x48], xmm0
00000001803871D5 movsd     xmm1, qword ptr [rsp + 0x30]
00000001803871DB movsd     qword ptr [rsp + 0x58], xmm1
00000001803871E1 mov       qword ptr [rsp + 0x20], r14
00000001803871E6 lea       rbx, [rsp + 0x48]
00000001803871EB mov       qword ptr [rsp + 0x28], rbx
00000001803871F0 mov       rdx, qword ptr [rip + 0x183dd91] # 0x181bc4f88
00000001803871F7 lea       rcx, [rsp + 0x48]
00000001803871FC call      0x180668160
0000000180387201 test      al, al
0000000180387203 je        0x180387265
0000000180387205 mov       rcx, qword ptr [rdi + 0x10]
0000000180387209 mov       edx, dword ptr [rsp + 0x58]
000000018038720D test      rcx, rcx
0000000180387210 je        0x1803874fb
0000000180387216 mov       r10, qword ptr [rip + 0x182871b] # 0x181baf938
000000018038721D inc       dword ptr [rcx + 0x1c]
0000000180387220 mov       r8, qword ptr [rcx + 0x10]
0000000180387224 movsxd    r9, dword ptr [rcx + 0x18]
0000000180387228 test      r8, r8
000000018038722B je        0x1803874f6
0000000180387231 cmp       r9d, dword ptr [r8 + 0x18]
0000000180387235 jb        0x18038724d
0000000180387237 mov       rax, qword ptr [r10 + 0x20]
000000018038723B mov       r8, qword ptr [rax + 0xc0]
0000000180387242 mov       r8, qword ptr [r8 + 0x70]
0000000180387246 call      0x180827460
000000018038724B jmp       0x1803871f0
000000018038724D lea       eax, [r9 + 1]
0000000180387251 mov       dword ptr [rcx + 0x18], eax
0000000180387254 cmp       r9d, dword ptr [r8 + 0x18]
0000000180387258 jae       0x1803874f0
000000018038725E mov       dword ptr [r8 + r9*4 + 0x20], edx
0000000180387263 jmp       0x1803871f0
0000000180387265 mov       rdx, qword ptr [rip + 0x183dcc4] # 0x181bc4f30
000000018038726C mov       rcx, rbx
000000018038726F call      0x180302170
0000000180387274 jmp       0x1803872a8
0000000180387276 mov       rdx, qword ptr [rip + 0x183dcb3] # 0x181bc4f30
000000018038727D mov       rcx, qword ptr [rsp + 0x28]
0000000180387282 call      0x180302170
0000000180387287 mov       rcx, qword ptr [rsp + 0x20]
000000018038728C test      rcx, rcx
000000018038728F jne       0x180387501
0000000180387295 xor       r14d, r14d
0000000180387298 mov       rsi, qword ptr [rsp + 0xa8]
00000001803872A0 mov       rdi, qword ptr [rsp + 0xa0]
00000001803872A8 mov       rcx, qword ptr [rdi + 0x18]
00000001803872AC test      rcx, rcx
00000001803872AF je        0x180387533
00000001803872B5 inc       dword ptr [rcx + 0x1c]
00000001803872B8 mov       dword ptr [rcx + 0x18], r14d
00000001803872BC movups    xmm0, xmmword ptr [rsi]
00000001803872BF movups    xmmword ptr [rsp + 0x20], xmm0
00000001803872C4 movsd     xmm0, qword ptr [rsi + 0x20]
00000001803872C9 movsd     qword ptr [rsp + 0x40], xmm0
00000001803872CF mov       rdx, qword ptr [rsi + 0x18]
00000001803872D3 test      rdx, rdx
00000001803872D6 je        0x180387533
00000001803872DC mov       r8, qword ptr [rip + 0x182880d] # 0x181bafaf0
00000001803872E3 lea       rcx, [rsp + 0x20]
00000001803872E8 call      0x18082ac10
00000001803872ED movups    xmm0, xmmword ptr [rsp + 0x20]
00000001803872F2 movups    xmmword ptr [rsp + 0x48], xmm0
00000001803872F7 movsd     xmm1, qword ptr [rsp + 0x30]
00000001803872FD movsd     qword ptr [rsp + 0x58], xmm1
0000000180387303 mov       qword ptr [rsp + 0x20], r14
0000000180387308 lea       rbx, [rsp + 0x48]
000000018038730D mov       qword ptr [rsp + 0x28], rbx
0000000180387312 mov       rdx, qword ptr [rip + 0x183dc6f] # 0x181bc4f88
0000000180387319 lea       rcx, [rsp + 0x48]
000000018038731E call      0x180668160
0000000180387323 test      al, al
0000000180387325 je        0x180387387
0000000180387327 mov       rcx, qword ptr [rdi + 0x18]
000000018038732B mov       edx, dword ptr [rsp + 0x58]
000000018038732F test      rcx, rcx
0000000180387332 je        0x180387511
0000000180387338 mov       r10, qword ptr [rip + 0x18285f9] # 0x181baf938
000000018038733F inc       dword ptr [rcx + 0x1c]
0000000180387342 mov       r8, qword ptr [rcx + 0x10]
0000000180387346 movsxd    r9, dword ptr [rcx + 0x18]
000000018038734A test      r8, r8
000000018038734D je        0x18038750c
0000000180387353 cmp       r9d, dword ptr [r8 + 0x18]
0000000180387357 jb        0x18038736f
0000000180387359 mov       rax, qword ptr [r10 + 0x20]
000000018038735D mov       r8, qword ptr [rax + 0xc0]
0000000180387364 mov       r8, qword ptr [r8 + 0x70]
0000000180387368 call      0x180827460
000000018038736D jmp       0x180387312
000000018038736F lea       eax, [r9 + 1]
0000000180387373 mov       dword ptr [rcx + 0x18], eax
0000000180387376 cmp       r9d, dword ptr [r8 + 0x18]
000000018038737A jae       0x180387507
0000000180387380 mov       dword ptr [r8 + r9*4 + 0x20], edx
0000000180387385 jmp       0x180387312
0000000180387387 mov       rdx, qword ptr [rip + 0x183dba2] # 0x181bc4f30
000000018038738E mov       rcx, rbx
0000000180387391 call      0x180302170
0000000180387396 jmp       0x1803873ca
0000000180387398 mov       rdx, qword ptr [rip + 0x183db91] # 0x181bc4f30
000000018038739F mov       rcx, qword ptr [rsp + 0x28]
00000001803873A4 call      0x180302170
00000001803873A9 mov       rcx, qword ptr [rsp + 0x20]
00000001803873AE test      rcx, rcx
00000001803873B1 jne       0x180387517
00000001803873B7 xor       r14d, r14d
00000001803873BA mov       rsi, qword ptr [rsp + 0xa8]
00000001803873C2 mov       rdi, qword ptr [rsp + 0xa0]
00000001803873CA mov       rcx, qword ptr [rdi + 0x20]
00000001803873CE test      rcx, rcx
00000001803873D1 je        0x180387533
00000001803873D7 inc       dword ptr [rcx + 0x1c]
00000001803873DA mov       dword ptr [rcx + 0x18], r14d
00000001803873DE movups    xmm0, xmmword ptr [rsi]
00000001803873E1 movups    xmmword ptr [rsp + 0x20], xmm0
00000001803873E6 movups    xmm1, xmmword ptr [rsi + 0x10]
00000001803873EA movups    xmmword ptr [rsp + 0x30], xmm1
00000001803873EF movsd     xmm0, qword ptr [rsi + 0x20]
00000001803873F4 movsd     qword ptr [rsp + 0x40], xmm0
00000001803873FA mov       rdx, qword ptr [rsp + 0x40]
00000001803873FF test      rdx, rdx
0000000180387402 je        0x180387533
0000000180387408 mov       r8, qword ptr [rip + 0x18286e1] # 0x181bafaf0
000000018038740F lea       rcx, [rsp + 0x20]
0000000180387414 call      0x18082ac10
0000000180387419 movups    xmm0, xmmword ptr [rsp + 0x20]
000000018038741E movups    xmmword ptr [rsp + 0x48], xmm0
0000000180387423 movsd     xmm1, qword ptr [rsp + 0x30]
0000000180387429 movsd     qword ptr [rsp + 0x58], xmm1
000000018038742F mov       qword ptr [rsp + 0x20], r14
0000000180387434 lea       rbx, [rsp + 0x48]
0000000180387439 mov       qword ptr [rsp + 0x28], rbx
000000018038743E nop       
0000000180387440 mov       rdx, qword ptr [rip + 0x183db41] # 0x181bc4f88
0000000180387447 lea       rcx, [rsp + 0x48]
000000018038744C call      0x180668160
0000000180387451 test      al, al
0000000180387453 je        0x1803874b0
0000000180387455 mov       rcx, qword ptr [rdi + 0x20]
0000000180387459 mov       edx, dword ptr [rdi + 8]
000000018038745C test      rcx, rcx
000000018038745F je        0x180387527
0000000180387465 mov       r10, qword ptr [rip + 0x18284cc] # 0x181baf938
000000018038746C inc       dword ptr [rcx + 0x1c]
000000018038746F mov       r8, qword ptr [rcx + 0x10]
0000000180387473 movsxd    r9, dword ptr [rcx + 0x18]
0000000180387477 test      r8, r8
000000018038747A je        0x180387522
0000000180387480 cmp       r9d, dword ptr [r8 + 0x18]
0000000180387484 jb        0x18038749c
0000000180387486 mov       rax, qword ptr [r10 + 0x20]
000000018038748A mov       r8, qword ptr [rax + 0xc0]
0000000180387491 mov       r8, qword ptr [r8 + 0x70]
0000000180387495 call      0x180827460
000000018038749A jmp       0x180387440
000000018038749C lea       eax, [r9 + 1]
00000001803874A0 mov       dword ptr [rcx + 0x18], eax
00000001803874A3 cmp       r9d, dword ptr [r8 + 0x18]
00000001803874A7 jae       0x18038751d
00000001803874A9 mov       dword ptr [r8 + r9*4 + 0x20], edx
00000001803874AE jmp       0x180387440
00000001803874B0 mov       rdx, qword ptr [rip + 0x183da79] # 0x181bc4f30
00000001803874B7 mov       rcx, rbx
00000001803874BA call      0x180302170
00000001803874BF jmp       0x1803874dc
00000001803874C1 mov       rdx, qword ptr [rip + 0x183da68] # 0x181bc4f30
00000001803874C8 mov       rcx, qword ptr [rsp + 0x28]
00000001803874CD call      0x180302170
00000001803874D2 mov       rcx, qword ptr [rsp + 0x20]
00000001803874D7 test      rcx, rcx
00000001803874DA jne       0x18038752d
00000001803874DC mov       rbx, qword ptr [rsp + 0xb0]
00000001803874E4 add       rsp, 0x80
00000001803874EB pop       r14
00000001803874ED pop       rdi
00000001803874EE pop       rsi
00000001803874EF ret       
00000001803874F0 call      0x180250140
00000001803874F5 nop       
00000001803874F6 call      0x180250150
00000001803874FB call      0x180250150
0000000180387500 nop       
0000000180387501 call      0x180200e60
0000000180387506 nop       
0000000180387507 call      0x180250140
000000018038750C call      0x180250150
0000000180387511 call      0x180250150
0000000180387516 nop       
0000000180387517 call      0x180200e60
000000018038751C nop       
000000018038751D call      0x180250140
0000000180387522 call      0x180250150
0000000180387527 call      0x180250150
000000018038752C nop       
000000018038752D call      0x180200e60
0000000180387532 int3      
0000000180387533 call      0x180250150
0000000180387538 int3      
0000000180387539 int3      
000000018038753A int3      
000000018038753B int3      
000000018038753C int3      
000000018038753D int3      
000000018038753E int3      
000000018038753F int3      