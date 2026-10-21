0000000180313070 push      rsi
0000000180313072 push      r12
0000000180313074 push      r13
0000000180313076 push      r14
0000000180313078 sub       rsp, 0x28
000000018031307C cmp       byte ptr [rip + 0x19d1b18], 0 # 0x181ce4b9b
0000000180313083 mov       r12d, r8d
0000000180313086 mov       r13d, edx
0000000180313089 mov       r14, rcx
000000018031308C jne       0x1803130dd
000000018031308E lea       rcx, [rip + 0x1889ed3] # 0x181b9cf68
0000000180313095 call      0x18024fef0
000000018031309A lea       rcx, [rip + 0x1889f27] # 0x181b9cfc8
00000001803130A1 call      0x18024fef0
00000001803130A6 lea       rcx, [rip + 0x18a238b] # 0x181bb5438
00000001803130AD call      0x18024fef0
00000001803130B2 lea       rcx, [rip + 0x18a019f] # 0x181bb3258
00000001803130B9 call      0x18024fef0
00000001803130BE lea       rcx, [rip + 0x18a7c33] # 0x181bbacf8
00000001803130C5 call      0x18024fef0
00000001803130CA lea       rcx, [rip + 0x1891797] # 0x181ba4868
00000001803130D1 call      0x18024fef0
00000001803130D6 mov       byte ptr [rip + 0x19d1abe], 1 # 0x181ce4b9b
00000001803130DD mov       rcx, qword ptr [rip + 0x1889ee4] # 0x181b9cfc8
00000001803130E4 mov       qword ptr [rsp + 0x50], rbx
00000001803130E9 mov       qword ptr [rsp + 0x58], rbp
00000001803130EE mov       qword ptr [rsp + 0x60], rdi
00000001803130F3 call      0x180250100
00000001803130F8 xor       r8d, r8d
00000001803130FB mov       edx, 3
0000000180313100 mov       rcx, rax
0000000180313103 mov       rsi, rax
0000000180313106 call      0x180345e10
000000018031310B test      rsi, rsi
000000018031310E je        0x180313318
0000000180313114 mov       rcx, qword ptr [rip + 0x189174d] # 0x181ba4868
000000018031311B mov       qword ptr [rsi + 0x10], rcx
000000018031311F lea       rcx, [rsi + 0x10]
0000000180313123 mov       rdx, qword ptr [rip + 0x189173e] # 0x181ba4868
000000018031312A mov       qword ptr [rsp + 0x20], r15
000000018031312F call      0x18024f360
0000000180313134 mov       dword ptr [rsi + 0x30], 0x45bb8000
000000018031313B mov       edi, 2
0000000180313140 mov       dword ptr [rsi + 0x18], edi
0000000180313143 mov       dword ptr [rsi + 0x34], 0x45bb8000
000000018031314A mov       rax, qword ptr [rip + 0x18a7ba7] # 0x181bbacf8
0000000180313151 mov       rcx, qword ptr [rip + 0x18a0100] # 0x181bb3258
0000000180313158 mov       rdx, qword ptr [rax + 0xb8]
000000018031315F mov       r15, qword ptr [rdx]
0000000180313162 call      0x180250100
0000000180313167 cmp       byte ptr [rip + 0x19d1a1f], 0 # 0x181ce4b8d
000000018031316E mov       rbp, rax
0000000180313171 jne       0x180313192
0000000180313173 lea       rcx, [rip + 0x1889e4e] # 0x181b9cfc8
000000018031317A call      0x18024fef0
000000018031317F lea       rcx, [rip + 0x18a7b72] # 0x181bbacf8
0000000180313186 call      0x18024fef0
000000018031318B mov       byte ptr [rip + 0x19d19fb], 1 # 0x181ce4b8d
0000000180313192 mov       rcx, qword ptr [rip + 0x18a7b5f] # 0x181bbacf8
0000000180313199 mov       rdx, qword ptr [rcx + 0xb8]
00000001803131A0 lea       rcx, [rbp + 0x18]
00000001803131A4 mov       rdx, qword ptr [rdx]
00000001803131A7 mov       qword ptr [rbp + 0x18], rdx
00000001803131AB call      0x18024f360
00000001803131B0 mov       rcx, qword ptr [rip + 0x1889e11] # 0x181b9cfc8
00000001803131B7 call      0x180250100
00000001803131BC xor       r8d, r8d
00000001803131BF mov       edx, 0xffffffff
00000001803131C4 mov       rcx, rax
00000001803131C7 mov       rbx, rax
00000001803131CA call      0x180345e10
00000001803131CF lea       rcx, [rbp + 0x20]
00000001803131D3 mov       qword ptr [rbp + 0x20], rbx
00000001803131D7 mov       rdx, rbx
00000001803131DA call      0x18024f360
00000001803131DF xor       edx, edx
00000001803131E1 mov       rcx, rbp
00000001803131E4 call      0x180302170
00000001803131E9 lea       rcx, [rbp + 0x20]
00000001803131ED mov       dword ptr [rbp + 0x10], r13d
00000001803131F1 mov       rdx, rsi
00000001803131F4 mov       dword ptr [rbp + 0x14], r12d
00000001803131F8 mov       qword ptr [rbp + 0x20], rsi
00000001803131FC call      0x18024f360
0000000180313201 lea       rcx, [rbp + 0x18]
0000000180313205 mov       qword ptr [rbp + 0x18], r15
0000000180313209 mov       rdx, r15
000000018031320C call      0x18024f360
0000000180313211 mov       rcx, qword ptr [rip + 0x1889d50] # 0x181b9cf68
0000000180313218 call      0x180250100
000000018031321D xor       edx, edx
000000018031321F mov       rcx, rax
0000000180313222 mov       rbx, rax
0000000180313225 call      0x18030f300
000000018031322A mov       r15, qword ptr [rsp + 0x20]
000000018031322F test      rbx, rbx
0000000180313232 je        0x180313318
0000000180313238 xor       r8d, r8d
000000018031323B mov       dword ptr [rbx + 0x10], edi
000000018031323E mov       edx, 5
0000000180313243 xor       ecx, ecx
0000000180313245 call      0x18130d310
000000018031324A test      eax, eax
000000018031324C je        0x180313277
000000018031324E sub       eax, 1
0000000180313251 je        0x180313279
0000000180313253 sub       eax, 1
0000000180313256 je        0x180313270
0000000180313258 sub       eax, 1
000000018031325B je        0x180313269
000000018031325D cmp       eax, 1
0000000180313260 jne       0x180313277
0000000180313262 mov       edi, 7
0000000180313267 jmp       0x180313279
0000000180313269 mov       edi, 0xe
000000018031326E jmp       0x180313279
0000000180313270 mov       edi, 4
0000000180313275 jmp       0x180313279
0000000180313277 xor       edi, edi
0000000180313279 mov       dword ptr [rbx + 0x14], edi
000000018031327C lea       rcx, [rsi + 0x28]
0000000180313280 mov       rdx, rbx
0000000180313283 mov       qword ptr [rsi + 0x28], rbx
0000000180313287 call      0x18024f360
000000018031328C lea       rcx, [rbp + 0x20]
0000000180313290 mov       qword ptr [rbp + 0x20], rsi
0000000180313294 mov       rdx, rsi
0000000180313297 call      0x18024f360
000000018031329C mov       rcx, qword ptr [r14 + 0x38]
00000001803132A0 test      rcx, rcx
00000001803132A3 je        0x180313318
00000001803132A5 mov       r9, qword ptr [rip + 0x18a218c] # 0x181bb5438
00000001803132AC inc       dword ptr [rcx + 0x1c]
00000001803132AF mov       rdx, qword ptr [rcx + 0x10]
00000001803132B3 movsxd    r8, dword ptr [rcx + 0x18]
00000001803132B7 test      rdx, rdx
00000001803132BA je        0x180313318
00000001803132BC cmp       r8d, dword ptr [rdx + 0x18]
00000001803132C0 jb        0x1803132f4
00000001803132C2 mov       rax, qword ptr [r9 + 0x20]
00000001803132C6 mov       rdx, rbp
00000001803132C9 mov       r8, qword ptr [rax + 0xc0]
00000001803132D0 mov       r8, qword ptr [r8 + 0x70]
00000001803132D4 call      0x180847ce0
00000001803132D9 mov       rdi, qword ptr [rsp + 0x60]
00000001803132DE mov       rbp, qword ptr [rsp + 0x58]
00000001803132E3 mov       rbx, qword ptr [rsp + 0x50]
00000001803132E8 add       rsp, 0x28
00000001803132EC pop       r14
00000001803132EE pop       r13
00000001803132F0 pop       r12
00000001803132F2 pop       rsi
00000001803132F3 ret       
00000001803132F4 lea       eax, [r8 + 1]
00000001803132F8 mov       dword ptr [rcx + 0x18], eax
00000001803132FB cmp       r8d, dword ptr [rdx + 0x18]
00000001803132FF jae       0x18031331e
0000000180313301 mov       qword ptr [rdx + r8*8 + 0x20], rbp
0000000180313306 add       rdx, 0x20
000000018031330A lea       rcx, [rdx + r8*8]
000000018031330E mov       rdx, rbp
0000000180313311 call      0x18024f360
0000000180313316 jmp       0x1803132d9
0000000180313318 call      0x180250150
000000018031331D int3      
000000018031331E call      0x180250140
0000000180313323 int3      
0000000180313324 int3      
0000000180313325 int3      
0000000180313326 int3      
0000000180313327 int3      
0000000180313328 int3      
0000000180313329 int3      
000000018031332A int3      
000000018031332B int3      
000000018031332C int3      
000000018031332D int3      
000000018031332E int3      
000000018031332F int3      