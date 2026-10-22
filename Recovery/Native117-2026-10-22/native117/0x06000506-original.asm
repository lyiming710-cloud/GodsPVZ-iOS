0000000180380F90 push      rbx
0000000180380F92 sub       rsp, 0x60
0000000180380F96 cmp       byte ptr [rip + 0x1963e63], 0 # 0x181ce4e00
0000000180380F9D mov       rbx, rcx
0000000180380FA0 jne       0x180381009
0000000180380FA2 lea       rcx, [rip + 0x18245cf] # 0x181ba5578
0000000180380FA9 call      0x18024fef0
0000000180380FAE lea       rcx, [rip + 0x18240a3] # 0x181ba5058
0000000180380FB5 call      0x18024fef0
0000000180380FBA lea       rcx, [rip + 0x183980f] # 0x181bba7d0
0000000180380FC1 call      0x18024fef0
0000000180380FC6 lea       rcx, [rip + 0x181d2ab] # 0x181b9e278
0000000180380FCD call      0x18024fef0
0000000180380FD2 lea       rcx, [rip + 0x181d2ff] # 0x181b9e2d8
0000000180380FD9 call      0x18024fef0
0000000180380FDE lea       rcx, [rip + 0x183b44b] # 0x181bbc430
0000000180380FE5 call      0x18024fef0
0000000180380FEA lea       rcx, [rip + 0x1829527] # 0x181baa518
0000000180380FF1 call      0x18024fef0
0000000180380FF6 lea       rcx, [rip + 0x182ba1b] # 0x181baca18
0000000180380FFD call      0x18024fef0
0000000180381002 mov       byte ptr [rip + 0x1963df7], 1 # 0x181ce4e00
0000000180381009 mov       qword ptr [rsp + 0x70], rbp
000000018038100E mov       qword ptr [rsp + 0x78], rsi
0000000180381013 mov       qword ptr [rsp + 0x80], rdi
000000018038101B mov       qword ptr [rsp + 0x88], r14
0000000180381023 movaps    xmmword ptr [rsp + 0x50], xmm6
0000000180381028 movaps    xmmword ptr [rsp + 0x40], xmm7
000000018038102D test      rbx, rbx
0000000180381030 je        0x180381336
0000000180381036 mov       rax, qword ptr [rbx + 0xe0]
000000018038103D test      rax, rax
0000000180381040 je        0x180381336
0000000180381046 mov       rax, qword ptr [rax + 0x50]
000000018038104A test      rax, rax
000000018038104D je        0x180381336
0000000180381053 cmp       dword ptr [rax + 0x18], 0
0000000180381057 jbe       0x18038133c
000000018038105D mov       rcx, qword ptr [rip + 0x183976c] # 0x181bba7d0
0000000180381064 movss     xmm6, dword ptr [rax + 0x20]
0000000180381069 call      0x180250100
000000018038106E movss     xmm7, dword ptr [rip + 0x12269ae] # 0x1815a7a24
0000000180381076 xor       r14d, r14d
0000000180381079 mov       rdx, qword ptr [rip + 0x182b998] # 0x181baca18
0000000180381080 xor       r9d, r9d
0000000180381083 mulss     xmm6, xmm7
0000000180381087 mov       rcx, rax
000000018038108A mov       rsi, rax
000000018038108D mov       qword ptr [rsp + 0x20], r14
0000000180381092 movaps    xmm2, xmm6
0000000180381095 call      0x180325080
000000018038109A test      rsi, rsi
000000018038109D je        0x180381336
00000001803810A3 lea       rcx, [rsi + 0x28]
00000001803810A7 mov       dword ptr [rsi + 0x18], 2
00000001803810AE mov       rdx, rbx
00000001803810B1 mov       byte ptr [rsi + 0x20], 1
00000001803810B5 mov       qword ptr [rsi + 0x28], rbx
00000001803810B9 call      0x18024f360
00000001803810BE mov       rax, qword ptr [rbx + 0xe0]
00000001803810C5 mov       rbp, qword ptr [rsi + 0x48]
00000001803810C9 test      rax, rax
00000001803810CC je        0x180381336
00000001803810D2 mov       rax, qword ptr [rax + 0x50]
00000001803810D6 test      rax, rax
00000001803810D9 je        0x180381336
00000001803810DF cmp       dword ptr [rax + 0x18], 1
00000001803810E3 jbe       0x18038133c
00000001803810E9 mov       rcx, qword ptr [rip + 0x18396e0] # 0x181bba7d0
00000001803810F0 movss     xmm6, dword ptr [rax + 0x24]
00000001803810F5 call      0x180250100
00000001803810FA mov       rdx, qword ptr [rip + 0x183b32f] # 0x181bbc430
0000000180381101 mov       r9b, 1
0000000180381104 mulss     xmm6, xmm7
0000000180381108 mov       rcx, rax
000000018038110B mov       rdi, rax
000000018038110E mov       qword ptr [rsp + 0x20], r14
0000000180381113 movaps    xmm2, xmm6
0000000180381116 call      0x180325080
000000018038111B test      rdi, rdi
000000018038111E je        0x180381336
0000000180381124 lea       rcx, [rdi + 0x28]
0000000180381128 mov       byte ptr [rdi + 0x20], 1
000000018038112C mov       rdx, rbx
000000018038112F mov       qword ptr [rdi + 0x28], rbx
0000000180381133 call      0x18024f360
0000000180381138 test      rbp, rbp
000000018038113B je        0x180381336
0000000180381141 mov       r8, qword ptr [rip + 0x1823f10] # 0x181ba5058
0000000180381148 inc       dword ptr [rbp + 0x1c]
000000018038114B mov       rcx, qword ptr [rbp + 0x10]
000000018038114F movsxd    rdx, dword ptr [rbp + 0x18]
0000000180381153 test      rcx, rcx
0000000180381156 je        0x180381336
000000018038115C cmp       edx, dword ptr [rcx + 0x18]
000000018038115F jb        0x18038117d
0000000180381161 mov       rax, qword ptr [r8 + 0x20]
0000000180381165 mov       rdx, rdi
0000000180381168 mov       rcx, rbp
000000018038116B mov       r8, qword ptr [rax + 0xc0]
0000000180381172 mov       r8, qword ptr [r8 + 0x70]
0000000180381176 call      0x180847ce0
000000018038117B jmp       0x1803811a1
000000018038117D lea       eax, [rdx + 1]
0000000180381180 mov       dword ptr [rbp + 0x18], eax
0000000180381183 cmp       edx, dword ptr [rcx + 0x18]
0000000180381186 jae       0x18038133c
000000018038118C mov       qword ptr [rcx + rdx*8 + 0x20], rdi
0000000180381191 lea       rcx, [rcx + rdx*8]
0000000180381195 add       rcx, 0x20
0000000180381199 mov       rdx, rdi
000000018038119C call      0x18024f360
00000001803811A1 mov       rcx, qword ptr [rbx + 0x220]
00000001803811A8 test      rcx, rcx
00000001803811AB je        0x180381336
00000001803811B1 xor       r8d, r8d
00000001803811B4 mov       rdx, rsi
00000001803811B7 call      0x180310190
00000001803811BC mov       rax, qword ptr [rbx + 0x1e8]
00000001803811C3 test      rax, rax
00000001803811C6 je        0x180381336
00000001803811CC cmp       byte ptr [rip + 0x1963961], r14b # 0x181ce4b34
00000001803811D3 mov       rsi, qword ptr [rax + 0x28]
00000001803811D7 jne       0x1803811ec
00000001803811D9 lea       rcx, [rip + 0x18306b0] # 0x181bb1890
00000001803811E0 call      0x18024fef0
00000001803811E5 mov       byte ptr [rip + 0x1963948], 1 # 0x181ce4b34
00000001803811EC mov       rax, qword ptr [rip + 0x183069d] # 0x181bb1890
00000001803811F3 mov       rcx, qword ptr [rax + 0xb8]
00000001803811FA mov       rcx, qword ptr [rcx]
00000001803811FD test      rcx, rcx
0000000180381200 je        0x180381336
0000000180381206 xor       r8d, r8d
0000000180381209 mov       edx, 0x1e
000000018038120E call      0x180321f30
0000000180381213 mov       rdi, rax
0000000180381216 test      rax, rax
0000000180381219 je        0x180381336
000000018038121F mov       rdx, qword ptr [rip + 0x1824352] # 0x181ba5578
0000000180381226 mov       rcx, rax
0000000180381229 call      0x18046dc80
000000018038122E test      rax, rax
0000000180381231 je        0x180381336
0000000180381237 mov       rcx, qword ptr [rip + 0x181d03a] # 0x181b9e278
000000018038123E xorps     xmm3, xmm3
0000000180381241 mov       qword ptr [rsp + 0x28], rcx
0000000180381246 mov       r8b, 1
0000000180381249 mov       rcx, qword ptr [rip + 0x18292c8] # 0x181baa518
0000000180381250 mov       rdx, rbx
0000000180381253 mov       qword ptr [rsp + 0x20], rcx
0000000180381258 xorps     xmm6, xmm6
000000018038125B mov       rcx, rax
000000018038125E call      0x1804c4ab0
0000000180381263 mov       rdx, qword ptr [rip + 0x182430e] # 0x181ba5578
000000018038126A mov       rcx, rdi
000000018038126D call      0x18046dc80
0000000180381272 test      rax, rax
0000000180381275 je        0x180381336
000000018038127B mov       r9, qword ptr [rip + 0x181d056] # 0x181b9e2d8
0000000180381282 mov       r8, rbx
0000000180381285 mov       edx, 0x1e
000000018038128A mov       rcx, rax
000000018038128D call      0x1804c4f70
0000000180381292 xor       edx, edx
0000000180381294 mov       rcx, rdi
0000000180381297 call      0x18131c070
000000018038129C mov       rdi, rax
000000018038129F test      rsi, rsi
00000001803812A2 je        0x180381336
00000001803812A8 mov       r8d, dword ptr [rbx + 0x20]
00000001803812AC lea       rcx, [rsp + 0x30]
00000001803812B1 mov       r9d, dword ptr [rbx + 0x24]
00000001803812B5 inc       r8d
00000001803812B8 mov       rdx, rsi
00000001803812BB mov       qword ptr [rsp + 0x20], r14
00000001803812C0 call      0x18030ec90
00000001803812C5 mov       r9d, dword ptr [rbx + 0x24]
00000001803812C9 lea       rcx, [rsp + 0x30]
00000001803812CE mov       r8d, dword ptr [rbx + 0x20]
00000001803812D2 mov       rdx, rsi
00000001803812D5 mov       qword ptr [rsp + 0x20], r14
00000001803812DA movss     xmm7, dword ptr [rax]
00000001803812DE call      0x18030ec30
00000001803812E3 movss     xmm1, dword ptr [rax + 4]
00000001803812E8 test      rdi, rdi
00000001803812EB je        0x180381336
00000001803812ED unpcklps  xmm7, xmm1
00000001803812F0 lea       rdx, [rsp + 0x30]
00000001803812F5 xor       r8d, r8d
00000001803812F8 movsd     qword ptr [rsp + 0x30], xmm7
00000001803812FE mov       rcx, rdi
0000000180381301 movss     dword ptr [rsp + 0x38], xmm6
0000000180381307 call      0x18132fbb0
000000018038130C movaps    xmm7, xmmword ptr [rsp + 0x40]
0000000180381311 movaps    xmm6, xmmword ptr [rsp + 0x50]
0000000180381316 mov       r14, qword ptr [rsp + 0x88]
000000018038131E mov       rdi, qword ptr [rsp + 0x80]
0000000180381326 mov       rsi, qword ptr [rsp + 0x78]
000000018038132B mov       rbp, qword ptr [rsp + 0x70]
0000000180381330 add       rsp, 0x60
0000000180381334 pop       rbx
0000000180381335 ret       
0000000180381336 call      0x180250150
000000018038133B int3      
000000018038133C call      0x180250140
0000000180381341 int3      
0000000180381342 int3      
0000000180381343 int3      
0000000180381344 int3      
0000000180381345 int3      
0000000180381346 int3      
0000000180381347 int3      
0000000180381348 int3      
0000000180381349 int3      
000000018038134A int3      
000000018038134B int3      
000000018038134C int3      
000000018038134D int3      
000000018038134E int3      
000000018038134F int3      