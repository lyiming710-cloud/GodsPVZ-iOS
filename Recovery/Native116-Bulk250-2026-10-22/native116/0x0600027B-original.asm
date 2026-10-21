0000000180341F50 mov       qword ptr [rsp + 0x10], rbx
0000000180341F55 mov       qword ptr [rsp + 0x18], rsi
0000000180341F5A mov       qword ptr [rsp + 0x20], rdi
0000000180341F5F push      r14
0000000180341F61 sub       rsp, 0x60
0000000180341F65 mov       rbx, rcx
0000000180341F68 cmp       byte ptr [rip + 0x19a2cf1], 0 # 0x181ce4c60
0000000180341F6F jne       0x180341fd8
0000000180341F71 lea       rcx, [rip + 0x18589b0] # 0x181b9a928
0000000180341F78 call      0x18024fef0
0000000180341F7D lea       rcx, [rip + 0x18888e4] # 0x181bca868
0000000180341F84 call      0x18024fef0
0000000180341F89 lea       rcx, [rip + 0x1888928] # 0x181bca8b8
0000000180341F90 call      0x18024fef0
0000000180341F95 lea       rcx, [rip + 0x1888974] # 0x181bca910
0000000180341F9C call      0x18024fef0
0000000180341FA1 lea       rcx, [rip + 0x1881638] # 0x181bc35e0
0000000180341FA8 call      0x18024fef0
0000000180341FAD lea       rcx, [rip + 0x1881734] # 0x181bc36e8
0000000180341FB4 call      0x18024fef0
0000000180341FB9 lea       rcx, [rip + 0x18815c8] # 0x181bc3588
0000000180341FC0 call      0x18024fef0
0000000180341FC5 lea       rcx, [rip + 0x187ff0c] # 0x181bc1ed8
0000000180341FCC call      0x18024fef0
0000000180341FD1 mov       byte ptr [rip + 0x19a2c88], 1 # 0x181ce4c60
0000000180341FD8 mov       rcx, qword ptr [rip + 0x187fef9] # 0x181bc1ed8
0000000180341FDF call      0x180250100
0000000180341FE4 mov       rdi, rax
0000000180341FE7 mov       rdx, qword ptr [rip + 0x188159a] # 0x181bc3588
0000000180341FEE mov       rcx, rax
0000000180341FF1 call      0x1808053d0
0000000180341FF6 mov       qword ptr [rsp + 0x70], rdi
0000000180341FFB mov       rdx, qword ptr [rbx + 0x38]
0000000180341FFF test      rdx, rdx
0000000180342002 je        0x1803421d4
0000000180342008 mov       r8, qword ptr [rip + 0x18816d9] # 0x181bc36e8
000000018034200F lea       rcx, [rsp + 0x30]
0000000180342014 call      0x1808197f0
0000000180342019 movups    xmm0, xmmword ptr [rsp + 0x30]
000000018034201E movups    xmmword ptr [rsp + 0x48], xmm0
0000000180342023 movsd     xmm1, qword ptr [rsp + 0x40]
0000000180342029 movsd     qword ptr [rsp + 0x58], xmm1
000000018034202F xor       r14d, r14d
0000000180342032 mov       qword ptr [rsp + 0x30], r14
0000000180342037 lea       rbx, [rsp + 0x48]
000000018034203C mov       qword ptr [rsp + 0x38], rbx
0000000180342041 mov       rdx, qword ptr [rip + 0x1888870] # 0x181bca8b8
0000000180342048 lea       rcx, [rsp + 0x48]
000000018034204D call      0x180669280
0000000180342052 test      al, al
0000000180342054 je        0x180342087
0000000180342056 mov       rdx, qword ptr [rsp + 0x58]
000000018034205B test      rdx, rdx
000000018034205E je        0x1803421b7
0000000180342064 cmp       byte ptr [rdx + 0xb8], 0
000000018034206B jne       0x180342041
000000018034206D test      rdi, rdi
0000000180342070 je        0x1803421b1
0000000180342076 mov       r8, qword ptr [rip + 0x1881563] # 0x181bc35e0
000000018034207D mov       rcx, rdi
0000000180342080 call      0x180002e70
0000000180342085 jmp       0x180342041
0000000180342087 mov       rdx, qword ptr [rip + 0x18887da] # 0x181bca868
000000018034208E mov       rcx, rbx
0000000180342091 call      0x180302170
0000000180342096 jmp       0x1803420bf
0000000180342098 mov       rdx, qword ptr [rip + 0x18887c9] # 0x181bca868
000000018034209F mov       rcx, qword ptr [rsp + 0x38]
00000001803420A4 call      0x180302170
00000001803420A9 mov       rcx, qword ptr [rsp + 0x30]
00000001803420AE test      rcx, rcx
00000001803420B1 jne       0x1803421bd
00000001803420B7 xor       r14d, r14d
00000001803420BA mov       rdi, qword ptr [rsp + 0x70]
00000001803420BF test      rdi, rdi
00000001803420C2 je        0x1803421d4
00000001803420C8 mov       r8, qword ptr [rip + 0x1881619] # 0x181bc36e8
00000001803420CF mov       rdx, rdi
00000001803420D2 lea       rcx, [rsp + 0x30]
00000001803420D7 call      0x1808197f0
00000001803420DC movups    xmm0, xmmword ptr [rsp + 0x30]
00000001803420E1 movups    xmmword ptr [rsp + 0x48], xmm0
00000001803420E6 movsd     xmm1, qword ptr [rsp + 0x40]
00000001803420EC movsd     qword ptr [rsp + 0x58], xmm1
00000001803420F2 mov       qword ptr [rsp + 0x30], r14
00000001803420F7 lea       rbx, [rsp + 0x48]
00000001803420FC mov       qword ptr [rsp + 0x38], rbx
0000000180342101 mov       rdx, qword ptr [rip + 0x18887b0] # 0x181bca8b8
0000000180342108 lea       rcx, [rsp + 0x48]
000000018034210D call      0x180669280
0000000180342112 test      al, al
0000000180342114 je        0x18034216e
0000000180342116 mov       rsi, qword ptr [rsp + 0x58]
000000018034211B mov       rcx, qword ptr [rip + 0x1858806] # 0x181b9a928
0000000180342122 call      0x180250100
0000000180342127 mov       rdi, rax
000000018034212A xor       edx, edx
000000018034212C mov       rcx, rax
000000018034212F call      0x180311cd0
0000000180342134 test      rdi, rdi
0000000180342137 je        0x1803421c8
000000018034213D mov       qword ptr [rdi + 0x10], 0x4cbebc20
0000000180342145 mov       dword ptr [rdi + 0x18], 4
000000018034214C mov       word ptr [rdi + 0x1f], 0x101
0000000180342152 mov       byte ptr [rdi + 0x21], 1
0000000180342156 test      rsi, rsi
0000000180342159 je        0x1803421c3
000000018034215B xor       r9d, r9d
000000018034215E xor       r8d, r8d
0000000180342161 mov       rdx, rdi
0000000180342164 mov       rcx, rsi
0000000180342167 call      0x180369aa0
000000018034216C jmp       0x180342101
000000018034216E mov       rdx, qword ptr [rip + 0x18886f3] # 0x181bca868
0000000180342175 mov       rcx, rbx
0000000180342178 call      0x180302170
000000018034217D jmp       0x18034219a
000000018034217F mov       rdx, qword ptr [rip + 0x18886e2] # 0x181bca868
0000000180342186 mov       rcx, qword ptr [rsp + 0x38]
000000018034218B call      0x180302170
0000000180342190 mov       rcx, qword ptr [rsp + 0x30]
0000000180342195 test      rcx, rcx
0000000180342198 jne       0x1803421ce
000000018034219A lea       r11, [rsp + 0x60]
000000018034219F mov       rbx, qword ptr [r11 + 0x18]
00000001803421A3 mov       rsi, qword ptr [r11 + 0x20]
00000001803421A7 mov       rdi, qword ptr [r11 + 0x28]
00000001803421AB mov       rsp, r11
00000001803421AE pop       r14
00000001803421B0 ret       
00000001803421B1 call      0x180250150
00000001803421B6 nop       
00000001803421B7 call      0x180250150
00000001803421BC nop       
00000001803421BD call      0x180200e60
00000001803421C2 nop       
00000001803421C3 call      0x180250150
00000001803421C8 call      0x180250150
00000001803421CD nop       
00000001803421CE call      0x180200e60
00000001803421D3 int3      
00000001803421D4 call      0x180250150
00000001803421D9 int3      
00000001803421DA int3      
00000001803421DB int3      
00000001803421DC int3      
00000001803421DD int3      
00000001803421DE int3      
00000001803421DF int3      