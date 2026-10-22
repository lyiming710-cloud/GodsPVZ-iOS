00000001803CCF80 push      rdi
00000001803CCF82 sub       rsp, 0x20
00000001803CCF86 cmp       byte ptr [rip + 0x1918095], 0 # 0x181ce5022
00000001803CCF8D mov       rdi, rcx
00000001803CCF90 jne       0x1803ccfb1
00000001803CCF92 lea       rcx, [rip + 0x17e6e17] # 0x181bb3db0
00000001803CCF99 call      0x18024fef0
00000001803CCF9E lea       rcx, [rip + 0x17f6a1b] # 0x181bc39c0
00000001803CCFA5 call      0x18024fef0
00000001803CCFAA mov       byte ptr [rip + 0x1918071], 1 # 0x181ce5022
00000001803CCFB1 mov       ecx, dword ptr [rdi + 0x10]
00000001803CCFB4 mov       qword ptr [rsp + 0x30], rbx
00000001803CCFB9 mov       qword ptr [rsp + 0x38], rsi
00000001803CCFBE test      ecx, ecx
00000001803CCFC0 je        0x1803cd25c
00000001803CCFC6 sub       ecx, 1
00000001803CCFC9 je        0x1803cd047
00000001803CCFCB sub       ecx, 1
00000001803CCFCE je        0x1803cd042
00000001803CCFD0 sub       ecx, 1
00000001803CCFD3 je        0x1803ccff0
00000001803CCFD5 cmp       ecx, 1
00000001803CCFD8 je        0x1803cd25c
00000001803CCFDE xor       al, al
00000001803CCFE0 mov       rsi, qword ptr [rsp + 0x38]
00000001803CCFE5 mov       rbx, qword ptr [rsp + 0x30]
00000001803CCFEA add       rsp, 0x20
00000001803CCFEE pop       rdi
00000001803CCFEF ret       
00000001803CCFF0 mov       dword ptr [rdi + 0x10], 0xffffffff
00000001803CCFF7 mov       rcx, qword ptr [rip + 0x17f69c2] # 0x181bc39c0
00000001803CCFFE call      0x180250100
00000001803CD003 movss     xmm1, dword ptr [rip + 0x11daa15] # 0x1815a7a20
00000001803CD00B xor       r8d, r8d
00000001803CD00E mov       rcx, rax
00000001803CD011 mov       rbx, rax
00000001803CD014 call      0x181333710
00000001803CD019 lea       rcx, [rdi + 0x18]
00000001803CD01D mov       qword ptr [rdi + 0x18], rbx
00000001803CD021 mov       rdx, rbx
00000001803CD024 call      0x18024f360
00000001803CD029 mov       al, 1
00000001803CD02B mov       dword ptr [rdi + 0x10], 4
00000001803CD032 mov       rsi, qword ptr [rsp + 0x38]
00000001803CD037 mov       rbx, qword ptr [rsp + 0x30]
00000001803CD03C add       rsp, 0x20
00000001803CD040 pop       rdi
00000001803CD041 ret       
00000001803CD042 inc       dword ptr [rdi + 0x30]
00000001803CD045 jmp       0x1803cd04e
00000001803CD047 mov       dword ptr [rdi + 0x30], 0
00000001803CD04E cmp       dword ptr [rdi + 0x30], 3
00000001803CD052 mov       rsi, qword ptr [rdi + 0x20]
00000001803CD056 mov       dword ptr [rdi + 0x10], 0xffffffff
00000001803CD05D jge       0x1803cd1b2
00000001803CD063 cmp       qword ptr [rdi + 0x28], 0
00000001803CD068 je        0x1803cd311
00000001803CD06E cmp       byte ptr [rip + 0x1917fab], 0 # 0x181ce5020
00000001803CD075 jne       0x1803cd08a
00000001803CD077 lea       rcx, [rip + 0x17e6d32] # 0x181bb3db0
00000001803CD07E call      0x18024fef0
00000001803CD083 mov       byte ptr [rip + 0x1917f96], 1 # 0x181ce5020
00000001803CD08A test      rsi, rsi
00000001803CD08D je        0x1803cd311
00000001803CD093 mov       rax, qword ptr [rsi + 0x20]
00000001803CD097 test      rax, rax
00000001803CD09A je        0x1803cd311
00000001803CD0A0 mov       rsi, qword ptr [rax + 0x90]
00000001803CD0A7 nop       word ptr [rax + rax]
00000001803CD0B0 mov       rax, qword ptr [rip + 0x17e6cf9] # 0x181bb3db0
00000001803CD0B7 cmp       dword ptr [rax + 0xe0], 0
00000001803CD0BE jne       0x1803cd0cf
00000001803CD0C0 mov       rcx, rax
00000001803CD0C3 call      0x1802501e0
00000001803CD0C8 mov       rax, qword ptr [rip + 0x17e6ce1] # 0x181bb3db0
00000001803CD0CF mov       rax, qword ptr [rax + 0xb8]
00000001803CD0D6 mov       rdx, qword ptr [rax]
00000001803CD0D9 test      rdx, rdx
00000001803CD0DC je        0x1803cd311
00000001803CD0E2 mov       edx, dword ptr [rdx + 0x18]
00000001803CD0E5 xor       r8d, r8d
00000001803CD0E8 xor       ecx, ecx
00000001803CD0EA call      0x18130d310
00000001803CD0EF mov       rcx, qword ptr [rip + 0x17e6cba] # 0x181bb3db0
00000001803CD0F6 mov       rdx, qword ptr [rcx + 0xb8]
00000001803CD0FD mov       rcx, qword ptr [rdx]
00000001803CD100 test      rcx, rcx
00000001803CD103 je        0x1803cd311
00000001803CD109 cmp       eax, dword ptr [rcx + 0x18]
00000001803CD10C jae       0x1803cd317
00000001803CD112 cdqe      
00000001803CD114 xor       r8d, r8d
00000001803CD117 mov       rdx, rsi
00000001803CD11A mov       rbx, qword ptr [rcx + rax*8 + 0x20]
00000001803CD11F mov       rcx, rbx
00000001803CD122 call      0x180b7cc30
00000001803CD127 test      al, al
00000001803CD129 je        0x1803cd0b0
00000001803CD12B mov       rsi, qword ptr [rdi + 0x20]
00000001803CD12F test      rsi, rsi
00000001803CD132 je        0x1803cd311
00000001803CD138 xor       r8d, r8d
00000001803CD13B mov       rdx, rbx
00000001803CD13E mov       rcx, rsi
00000001803CD141 call      0x1803c68a0
00000001803CD146 cmp       byte ptr [rip + 0x1917eb7], 0 # 0x181ce5004
00000001803CD14D jne       0x1803cd162
00000001803CD14F lea       rcx, [rip + 0x17eefea] # 0x181bbc140
00000001803CD156 call      0x18024fef0
00000001803CD15B mov       byte ptr [rip + 0x1917ea2], 1 # 0x181ce5004
00000001803CD162 mov       rcx, qword ptr [rip + 0x17eefd7] # 0x181bbc140
00000001803CD169 call      0x180250100
00000001803CD16E xor       edx, edx
00000001803CD170 mov       rcx, rax
00000001803CD173 mov       rbx, rax
00000001803CD176 call      0x18030f300
00000001803CD17B xor       r8d, r8d
00000001803CD17E mov       rdx, rsi
00000001803CD181 mov       rcx, rbx
00000001803CD184 call      0x1803cba50
00000001803CD189 lea       rcx, [rdi + 0x18]
00000001803CD18D mov       qword ptr [rdi + 0x18], rbx
00000001803CD191 mov       rdx, rbx
00000001803CD194 call      0x18024f360
00000001803CD199 mov       al, 1
00000001803CD19B mov       dword ptr [rdi + 0x10], 2
00000001803CD1A2 mov       rsi, qword ptr [rsp + 0x38]
00000001803CD1A7 mov       rbx, qword ptr [rsp + 0x30]
00000001803CD1AC add       rsp, 0x20
00000001803CD1B0 pop       rdi
00000001803CD1B1 ret       
00000001803CD1B2 mov       rdx, qword ptr [rip + 0x17e6bf7] # 0x181bb3db0
00000001803CD1B9 cmp       dword ptr [rdx + 0xe0], 0
00000001803CD1C0 jne       0x1803cd1d1
00000001803CD1C2 mov       rcx, rdx
00000001803CD1C5 call      0x1802501e0
00000001803CD1CA mov       rdx, qword ptr [rip + 0x17e6bdf] # 0x181bb3db0
00000001803CD1D1 test      rsi, rsi
00000001803CD1D4 je        0x1803cd311
00000001803CD1DA mov       rdx, qword ptr [rdx + 0xb8]
00000001803CD1E1 xor       r8d, r8d
00000001803CD1E4 mov       rcx, rsi
00000001803CD1E7 mov       rdx, qword ptr [rdx + 0x10]
00000001803CD1EB call      0x1803c68a0
00000001803CD1F0 cmp       byte ptr [rip + 0x1917e0d], 0 # 0x181ce5004
00000001803CD1F7 jne       0x1803cd20c
00000001803CD1F9 lea       rcx, [rip + 0x17eef40] # 0x181bbc140
00000001803CD200 call      0x18024fef0
00000001803CD205 mov       byte ptr [rip + 0x1917df8], 1 # 0x181ce5004
00000001803CD20C mov       rcx, qword ptr [rip + 0x17eef2d] # 0x181bbc140
00000001803CD213 call      0x180250100
00000001803CD218 xor       edx, edx
00000001803CD21A mov       rcx, rax
00000001803CD21D mov       rbx, rax
00000001803CD220 call      0x18030f300
00000001803CD225 xor       r8d, r8d
00000001803CD228 mov       rdx, rsi
00000001803CD22B mov       rcx, rbx
00000001803CD22E call      0x1803cba50
00000001803CD233 lea       rcx, [rdi + 0x18]
00000001803CD237 mov       qword ptr [rdi + 0x18], rbx
00000001803CD23B mov       rdx, rbx
00000001803CD23E call      0x18024f360
00000001803CD243 mov       al, 1
00000001803CD245 mov       dword ptr [rdi + 0x10], 3
00000001803CD24C mov       rsi, qword ptr [rsp + 0x38]
00000001803CD251 mov       rbx, qword ptr [rsp + 0x30]
00000001803CD256 add       rsp, 0x20
00000001803CD25A pop       rdi
00000001803CD25B ret       
00000001803CD25C mov       rsi, qword ptr [rdi + 0x20]
00000001803CD260 mov       dword ptr [rdi + 0x10], 0xffffffff
00000001803CD267 mov       rdx, qword ptr [rip + 0x17e6b42] # 0x181bb3db0
00000001803CD26E cmp       dword ptr [rdx + 0xe0], 0
00000001803CD275 jne       0x1803cd286
00000001803CD277 mov       rcx, rdx
00000001803CD27A call      0x1802501e0
00000001803CD27F mov       rdx, qword ptr [rip + 0x17e6b2a] # 0x181bb3db0
00000001803CD286 test      rsi, rsi
00000001803CD289 je        0x1803cd311
00000001803CD28F mov       rdx, qword ptr [rdx + 0xb8]
00000001803CD296 xor       r8d, r8d
00000001803CD299 mov       rcx, rsi
00000001803CD29C mov       rdx, qword ptr [rdx + 8]
00000001803CD2A0 call      0x1803c68a0
00000001803CD2A5 cmp       byte ptr [rip + 0x1917d58], 0 # 0x181ce5004
00000001803CD2AC jne       0x1803cd2c1
00000001803CD2AE lea       rcx, [rip + 0x17eee8b] # 0x181bbc140
00000001803CD2B5 call      0x18024fef0
00000001803CD2BA mov       byte ptr [rip + 0x1917d43], 1 # 0x181ce5004
00000001803CD2C1 mov       rcx, qword ptr [rip + 0x17eee78] # 0x181bbc140
00000001803CD2C8 call      0x180250100
00000001803CD2CD xor       edx, edx
00000001803CD2CF mov       rcx, rax
00000001803CD2D2 mov       rbx, rax
00000001803CD2D5 call      0x18030f300
00000001803CD2DA xor       r8d, r8d
00000001803CD2DD mov       rdx, rsi
00000001803CD2E0 mov       rcx, rbx
00000001803CD2E3 call      0x1803cba50
00000001803CD2E8 lea       rcx, [rdi + 0x18]
00000001803CD2EC mov       qword ptr [rdi + 0x18], rbx
00000001803CD2F0 mov       rdx, rbx
00000001803CD2F3 call      0x18024f360
00000001803CD2F8 mov       rsi, qword ptr [rsp + 0x38]
00000001803CD2FD mov       al, 1
00000001803CD2FF mov       rbx, qword ptr [rsp + 0x30]
00000001803CD304 mov       dword ptr [rdi + 0x10], 1
00000001803CD30B add       rsp, 0x20
00000001803CD30F pop       rdi
00000001803CD310 ret       
00000001803CD311 call      0x180250150
00000001803CD316 int3      
00000001803CD317 call      0x180250140
00000001803CD31C int3      
00000001803CD31D int3      
00000001803CD31E int3      
00000001803CD31F int3      