0000000180354010 mov       qword ptr [rsp + 0x10], rbx
0000000180354015 mov       qword ptr [rsp + 0x20], rsi
000000018035401A mov       qword ptr [rsp + 8], rcx
000000018035401F push      rdi
0000000180354020 sub       rsp, 0x70
0000000180354024 movaps    xmmword ptr [rsp + 0x60], xmm6
0000000180354029 mov       rsi, rcx
000000018035402C cmp       byte ptr [rip + 0x1990ce2], 0 # 0x181ce4d15
0000000180354033 jne       0x180354084
0000000180354035 lea       rcx, [rip + 0x187682c] # 0x181bca868
000000018035403C call      0x18024fef0
0000000180354041 lea       rcx, [rip + 0x1876870] # 0x181bca8b8
0000000180354048 call      0x18024fef0
000000018035404D lea       rcx, [rip + 0x18768bc] # 0x181bca910
0000000180354054 call      0x18024fef0
0000000180354059 lea       rcx, [rip + 0x186f688] # 0x181bc36e8
0000000180354060 call      0x18024fef0
0000000180354065 lea       rcx, [rip + 0x185c494] # 0x181bb0500
000000018035406C call      0x18024fef0
0000000180354071 lea       rcx, [rip + 0x1872928] # 0x181bc69a0
0000000180354078 call      0x18024fef0
000000018035407D mov       byte ptr [rip + 0x1990c91], 1 # 0x181ce4d15
0000000180354084 movss     xmm1, dword ptr [rsi + 0xd4]
000000018035408C xorps     xmm0, xmm0
000000018035408F comiss    xmm1, xmm0
0000000180354092 jbe       0x1803540fe
0000000180354094 cmp       dword ptr [rsi + 0x90], 1
000000018035409B jne       0x1803540fe
000000018035409D movss     xmm6, dword ptr [rsi + 0xd0]
00000001803540A5 xor       ecx, ecx
00000001803540A7 call      0x18132c2f0
00000001803540AC movaps    xmm1, xmm0
00000001803540AF addss     xmm1, xmm6
00000001803540B3 movss     dword ptr [rsi + 0xd0], xmm1
00000001803540BB addss     xmm0, xmm6
00000001803540BF comiss    xmm0, dword ptr [rsi + 0xd4]
00000001803540C6 jb        0x1803540fe
00000001803540C8 mov       rcx, qword ptr [rsi + 0x168]
00000001803540CF test      rcx, rcx
00000001803540D2 je        0x18035440d
00000001803540D8 xor       r9d, r9d
00000001803540DB mov       r8b, 1
00000001803540DE mov       rdx, qword ptr [rip + 0x18728bb] # 0x181bc69a0
00000001803540E5 call      0x1812dba00
00000001803540EA mov       dword ptr [rsi + 0xd0], 0
00000001803540F4 mov       dword ptr [rsi + 0x90], 2
00000001803540FE cmp       dword ptr [rsi + 0x90], 0
0000000180354105 jne       0x1803543f6
000000018035410B mov       qword ptr [rsp + 0x90], 0
0000000180354117 mov       rax, qword ptr [rsi + 0x1e8]
000000018035411E test      rax, rax
0000000180354121 je        0x18035440d
0000000180354127 mov       rcx, qword ptr [rax + 0xf0]
000000018035412E test      rcx, rcx
0000000180354131 je        0x18035440d
0000000180354137 mov       rdx, qword ptr [rcx + 0x38]
000000018035413B test      rdx, rdx
000000018035413E je        0x18035440d
0000000180354144 mov       r8, qword ptr [rip + 0x186f59d] # 0x181bc36e8
000000018035414B lea       rcx, [rsp + 0x30]
0000000180354150 call      0x1808197f0
0000000180354155 movups    xmm0, xmmword ptr [rsp + 0x30]
000000018035415A movups    xmmword ptr [rsp + 0x48], xmm0
000000018035415F movsd     xmm1, qword ptr [rsp + 0x40]
0000000180354165 movsd     qword ptr [rsp + 0x58], xmm1
000000018035416B mov       qword ptr [rsp + 0x30], 0
0000000180354174 lea       rbx, [rsp + 0x48]
0000000180354179 mov       qword ptr [rsp + 0x38], rbx
000000018035417E movss     xmm6, dword ptr [rip + 0x1253882] # 0x1815a7a08
0000000180354186 nop       word ptr [rax + rax]
0000000180354190 mov       rdx, qword ptr [rip + 0x1876721] # 0x181bca8b8
0000000180354197 lea       rcx, [rsp + 0x48]
000000018035419C call      0x180669280
00000001803541A1 test      al, al
00000001803541A3 je        0x180354293
00000001803541A9 mov       rdi, qword ptr [rsp + 0x58]
00000001803541AE mov       rcx, qword ptr [rip + 0x185c34b] # 0x181bb0500
00000001803541B5 cmp       dword ptr [rcx + 0xe0], 0
00000001803541BC jne       0x1803541c3
00000001803541BE call      0x1802501e0
00000001803541C3 xor       r8d, r8d
00000001803541C6 xor       edx, edx
00000001803541C8 mov       rcx, rdi
00000001803541CB call      0x18131f900
00000001803541D0 test      al, al
00000001803541D2 je        0x180354190
00000001803541D4 test      rdi, rdi
00000001803541D7 je        0x180354413
00000001803541DD xor       edx, edx
00000001803541DF mov       rcx, rdi
00000001803541E2 call      0x1803659e0
00000001803541E7 test      al, al
00000001803541E9 jne       0x180354190
00000001803541EB cmp       dword ptr [rdi + 0x60], 0xd
00000001803541EF je        0x1803541ff
00000001803541F1 xor       edx, edx
00000001803541F3 mov       rcx, rdi
00000001803541F6 call      0x18035de80
00000001803541FB test      al, al
00000001803541FD je        0x180354190
00000001803541FF movss     xmm2, dword ptr [rsi + 0x30]
0000000180354204 movss     xmm0, dword ptr [rsi + 0x40]
0000000180354209 movss     xmm1, dword ptr [rsi + 0x44]
000000018035420E mulss     xmm1, xmm6
0000000180354212 addss     xmm1, dword ptr [rsi + 0x34]
0000000180354217 mulss     xmm0, xmm6
000000018035421B subss     xmm2, xmm0
000000018035421F movss     xmm4, dword ptr [rdi + 0x30]
0000000180354224 movss     xmm0, dword ptr [rdi + 0x40]
0000000180354229 movss     xmm3, dword ptr [rdi + 0x44]
000000018035422E mulss     xmm3, xmm6
0000000180354232 addss     xmm3, dword ptr [rdi + 0x34]
0000000180354237 mulss     xmm0, xmm6
000000018035423B subss     xmm4, xmm0
000000018035423F movaps    xmm0, xmm4
0000000180354242 addss     xmm0, dword ptr [rdi + 0x40]
0000000180354247 comiss    xmm0, xmm2
000000018035424A jbe       0x180354270
000000018035424C addss     xmm2, dword ptr [rsi + 0x40]
0000000180354251 comiss    xmm2, xmm4
0000000180354254 jbe       0x180354270
0000000180354256 movaps    xmm0, xmm3
0000000180354259 addss     xmm0, dword ptr [rdi + 0x44]
000000018035425E comiss    xmm0, xmm1
0000000180354261 jbe       0x180354270
0000000180354263 addss     xmm1, dword ptr [rsi + 0x44]
0000000180354268 comiss    xmm1, xmm3
000000018035426B seta      al
000000018035426E jmp       0x180354272
0000000180354270 xor       eax, eax
0000000180354272 test      al, al
0000000180354274 je        0x180354190
000000018035427A mov       qword ptr [rsp + 0x90], rdi
0000000180354282 mov       rdx, qword ptr [rip + 0x18765df] # 0x181bca868
0000000180354289 mov       rcx, rbx
000000018035428C call      0x180302170
0000000180354291 jmp       0x1803542cb
0000000180354293 mov       rdx, qword ptr [rip + 0x18765ce] # 0x181bca868
000000018035429A mov       rcx, rbx
000000018035429D call      0x180302170
00000001803542A2 jmp       0x1803542cb
00000001803542A4 mov       rdx, qword ptr [rip + 0x18765bd] # 0x181bca868
00000001803542AB mov       rcx, qword ptr [rsp + 0x38]
00000001803542B0 call      0x180302170
00000001803542B5 mov       rcx, qword ptr [rsp + 0x30]
00000001803542BA test      rcx, rcx
00000001803542BD jne       0x180354419
00000001803542C3 mov       rsi, qword ptr [rsp + 0x80]
00000001803542CB mov       rdx, qword ptr [rsi + 0x130]
00000001803542D2 test      rdx, rdx
00000001803542D5 je        0x18035440d
00000001803542DB mov       r8, qword ptr [rip + 0x186f406] # 0x181bc36e8
00000001803542E2 lea       rcx, [rsp + 0x30]
00000001803542E7 call      0x1808197f0
00000001803542EC movups    xmm0, xmmword ptr [rsp + 0x30]
00000001803542F1 movups    xmmword ptr [rsp + 0x48], xmm0
00000001803542F6 movsd     xmm1, qword ptr [rsp + 0x40]
00000001803542FC movsd     qword ptr [rsp + 0x58], xmm1
0000000180354302 mov       qword ptr [rsp + 0x30], 0
000000018035430B lea       rbx, [rsp + 0x48]
0000000180354310 mov       qword ptr [rsp + 0x38], rbx
0000000180354315 nop       word ptr [rax + rax]
0000000180354320 mov       rdx, qword ptr [rip + 0x1876591] # 0x181bca8b8
0000000180354327 lea       rcx, [rsp + 0x48]
000000018035432C call      0x180669280
0000000180354331 test      al, al
0000000180354333 je        0x180354390
0000000180354335 mov       rdi, qword ptr [rsp + 0x58]
000000018035433A mov       rcx, qword ptr [rip + 0x185c1bf] # 0x181bb0500
0000000180354341 cmp       dword ptr [rcx + 0xe0], 0
0000000180354348 jne       0x18035434f
000000018035434A call      0x1802501e0
000000018035434F xor       r8d, r8d
0000000180354352 xor       edx, edx
0000000180354354 mov       rcx, rdi
0000000180354357 call      0x18131f900
000000018035435C test      al, al
000000018035435E je        0x180354320
0000000180354360 test      rdi, rdi
0000000180354363 je        0x18035441f
0000000180354369 xor       edx, edx
000000018035436B mov       rcx, rdi
000000018035436E call      0x1803659e0
0000000180354373 test      al, al
0000000180354375 jne       0x180354320
0000000180354377 mov       qword ptr [rsp + 0x90], rdi
000000018035437F mov       rdx, qword ptr [rip + 0x18764e2] # 0x181bca868
0000000180354386 mov       rcx, rbx
0000000180354389 call      0x180302170
000000018035438E jmp       0x1803543c4
0000000180354390 mov       rdx, qword ptr [rip + 0x18764d1] # 0x181bca868
0000000180354397 mov       rcx, rbx
000000018035439A call      0x180302170
000000018035439F jmp       0x1803543c4
00000001803543A1 mov       rdx, qword ptr [rip + 0x18764c0] # 0x181bca868
00000001803543A8 mov       rcx, qword ptr [rsp + 0x38]
00000001803543AD call      0x180302170
00000001803543B2 mov       rcx, qword ptr [rsp + 0x30]
00000001803543B7 test      rcx, rcx
00000001803543BA jne       0x180354425
00000001803543BC mov       rsi, qword ptr [rsp + 0x80]
00000001803543C4 mov       rcx, qword ptr [rip + 0x185c135] # 0x181bb0500
00000001803543CB cmp       dword ptr [rcx + 0xe0], 0
00000001803543D2 jne       0x1803543d9
00000001803543D4 call      0x1802501e0
00000001803543D9 xor       edx, edx
00000001803543DB mov       rcx, qword ptr [rsp + 0x90]
00000001803543E3 call      0x18131f870
00000001803543E8 test      al, al
00000001803543EA je        0x1803543f6
00000001803543EC xor       edx, edx
00000001803543EE mov       rcx, rsi
00000001803543F1 call      0x180353a90
00000001803543F6 lea       r11, [rsp + 0x70]
00000001803543FB mov       rbx, qword ptr [r11 + 0x18]
00000001803543FF mov       rsi, qword ptr [r11 + 0x28]
0000000180354403 movaps    xmm6, xmmword ptr [rsp + 0x60]
0000000180354408 mov       rsp, r11
000000018035440B pop       rdi
000000018035440C ret       
000000018035440D call      0x180250150
0000000180354412 nop       
0000000180354413 call      0x180250150
0000000180354418 nop       
0000000180354419 call      0x180200e60
000000018035441E nop       
000000018035441F call      0x180250150
0000000180354424 nop       
0000000180354425 call      0x180200e60
000000018035442A int3      
000000018035442B int3      
000000018035442C int3      
000000018035442D int3      
000000018035442E int3      
000000018035442F int3      