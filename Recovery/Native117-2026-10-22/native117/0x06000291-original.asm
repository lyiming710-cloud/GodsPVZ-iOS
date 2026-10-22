000000018032CEE0 mov       dword ptr [rsp + 0x18], r8d
000000018032CEE5 mov       dword ptr [rsp + 0x10], edx
000000018032CEE9 mov       qword ptr [rsp + 8], rcx
000000018032CEEE push      rbx
000000018032CEEF push      rsi
000000018032CEF0 push      rdi
000000018032CEF1 push      r12
000000018032CEF3 push      r13
000000018032CEF5 push      r14
000000018032CEF7 push      r15
000000018032CEF9 sub       rsp, 0x70
000000018032CEFD mov       r13, rcx
000000018032CF00 cmp       byte ptr [rip + 0x19b7d6a], 0 # 0x181ce4c71
000000018032CF07 jne       0x18032cfc8
000000018032CF0D lea       rcx, [rip + 0x186de34] # 0x181b9ad48
000000018032CF14 call      0x18024fef0
000000018032CF19 lea       rcx, [rip + 0x189aa58] # 0x181bc7978
000000018032CF20 call      0x18024fef0
000000018032CF25 lea       rcx, [rip + 0x189aaa4] # 0x181bc79d0
000000018032CF2C call      0x18024fef0
000000018032CF31 lea       rcx, [rip + 0x189aaf0] # 0x181bc7a28
000000018032CF38 call      0x18024fef0
000000018032CF3D lea       rcx, [rip + 0x1875e4c] # 0x181ba2d90
000000018032CF44 call      0x18024fef0
000000018032CF49 lea       rcx, [rip + 0x187fce8] # 0x181bacc38
000000018032CF50 call      0x18024fef0
000000018032CF55 lea       rcx, [rip + 0x188b134] # 0x181bb8090
000000018032CF5C call      0x18024fef0
000000018032CF61 lea       rcx, [rip + 0x188b180] # 0x181bb80e8
000000018032CF68 call      0x18024fef0
000000018032CF6D lea       rcx, [rip + 0x188b1cc] # 0x181bb8140
000000018032CF74 call      0x18024fef0
000000018032CF79 lea       rcx, [rip + 0x187fd68] # 0x181bacce8
000000018032CF80 call      0x18024fef0
000000018032CF85 lea       rcx, [rip + 0x188b20c] # 0x181bb8198
000000018032CF8C call      0x18024fef0
000000018032CF91 lea       rcx, [rip + 0x187fda8] # 0x181bacd40
000000018032CF98 call      0x18024fef0
000000018032CF9D lea       rcx, [rip + 0x1889a54] # 0x181bb69f8
000000018032CFA4 call      0x18024fef0
000000018032CFA9 lea       rcx, [rip + 0x189f228] # 0x181bcc1d8
000000018032CFB0 call      0x18024fef0
000000018032CFB5 lea       rcx, [rip + 0x189e9ec] # 0x181bcb9a8
000000018032CFBC call      0x18024fef0
000000018032CFC1 mov       byte ptr [rip + 0x19b7ca9], 1 # 0x181ce4c71
000000018032CFC8 xor       edx, edx
000000018032CFCA lea       rcx, [rsp + 0xb8]
000000018032CFD2 call      0x180ca3820
000000018032CFD7 mov       rbx, rax
000000018032CFDA xor       edx, edx
000000018032CFDC lea       rcx, [rsp + 0xc0]
000000018032CFE4 call      0x180ca3820
000000018032CFE9 xor       edi, edi
000000018032CFEB mov       qword ptr [rsp + 0x20], rdi
000000018032CFF0 mov       r9, rax
000000018032CFF3 mov       r8, qword ptr [rip + 0x189f1de] # 0x181bcc1d8
000000018032CFFA mov       rdx, rbx
000000018032CFFD mov       rcx, qword ptr [rip + 0x189e9a4] # 0x181bcb9a8
000000018032D004 call      0x180b73950
000000018032D009 mov       rbx, rax
000000018032D00C mov       rcx, qword ptr [rip + 0x186dd35] # 0x181b9ad48
000000018032D013 cmp       dword ptr [rcx + 0xe0], edi
000000018032D019 jne       0x18032d020
000000018032D01B call      0x1802501e0
000000018032D020 xor       edx, edx
000000018032D022 mov       rcx, rbx
000000018032D025 call      0x1812e68f0
000000018032D02A mov       rax, qword ptr [r13 + 0x10]
000000018032D02E test      rax, rax
000000018032D031 je        0x18032d25b
000000018032D037 mov       edx, dword ptr [rsp + 0xc0]
000000018032D03E cmp       dword ptr [rax + 0x18], edx
000000018032D041 jle       0x18032d059
000000018032D043 mov       r8d, dword ptr [rax + 0x18]
000000018032D047 sub       r8d, edx
000000018032D04A mov       r9, qword ptr [rip + 0x188b0ef] # 0x181bb8140
000000018032D051 mov       rcx, rax
000000018032D054 call      0x180803380
000000018032D059 mov       rdx, qword ptr [r13 + 0x10]
000000018032D05D test      rdx, rdx
000000018032D060 je        0x18032d25b
000000018032D066 mov       r8, qword ptr [rip + 0x188b07b] # 0x181bb80e8
000000018032D06D lea       rcx, [rsp + 0x38]
000000018032D072 call      0x1808197f0
000000018032D077 movups    xmm0, xmmword ptr [rsp + 0x38]
000000018032D07C movups    xmmword ptr [rsp + 0x50], xmm0
000000018032D081 movsd     xmm1, qword ptr [rsp + 0x48]
000000018032D087 movsd     qword ptr [rsp + 0x60], xmm1
000000018032D08D mov       qword ptr [rsp + 0x38], rdi
000000018032D092 lea       rbx, [rsp + 0x50]
000000018032D097 mov       qword ptr [rsp + 0x40], rbx
000000018032D09C nop       dword ptr [rax]
000000018032D0A0 mov       rdx, qword ptr [rip + 0x189a929] # 0x181bc79d0
000000018032D0A7 lea       rcx, [rsp + 0x50]
000000018032D0AC call      0x180669280
000000018032D0B1 test      al, al
000000018032D0B3 je        0x18032d161
000000018032D0B9 mov       rdi, qword ptr [rsp + 0x60]
000000018032D0BE test      rdi, rdi
000000018032D0C1 je        0x18032d271
000000018032D0C7 mov       rax, qword ptr [rdi + 0x18]
000000018032D0CB test      rax, rax
000000018032D0CE je        0x18032d26c
000000018032D0D4 mov       ecx, dword ptr [rsp + 0xb8]
000000018032D0DB cmp       dword ptr [rax + 0x18], ecx
000000018032D0DE jle       0x18032d100
000000018032D0E0 mov       r8d, dword ptr [rax + 0x18]
000000018032D0E4 sub       r8d, ecx
000000018032D0E7 mov       r9, qword ptr [rip + 0x187fbfa] # 0x181bacce8
000000018032D0EE mov       edx, ecx
000000018032D0F0 mov       rcx, rax
000000018032D0F3 call      0x180803380
000000018032D0F8 mov       ecx, dword ptr [rsp + 0xb8]
000000018032D0FF nop       
000000018032D100 test      rdi, rdi
000000018032D103 je        0x18032d267
000000018032D109 mov       rax, qword ptr [rdi + 0x18]
000000018032D10D test      rax, rax
000000018032D110 je        0x18032d261
000000018032D116 cmp       dword ptr [rax + 0x18], ecx
000000018032D119 jge       0x18032d0a0
000000018032D11B mov       r14, rax
000000018032D11E mov       r15d, dword ptr [rax + 0x18]
000000018032D122 mov       r12d, dword ptr [rdi + 0x10]
000000018032D126 mov       rcx, qword ptr [rip + 0x1875c63] # 0x181ba2d90
000000018032D12D call      0x180250100
000000018032D132 mov       rsi, rax
000000018032D135 xor       r9d, r9d
000000018032D138 mov       r8d, r12d
000000018032D13B mov       edx, r15d
000000018032D13E mov       rcx, rax
000000018032D141 call      0x18032a750
000000018032D146 mov       r8, qword ptr [rip + 0x187faeb] # 0x181bacc38
000000018032D14D mov       rdx, rsi
000000018032D150 mov       rcx, r14
000000018032D153 call      0x180002e70
000000018032D158 mov       ecx, dword ptr [rsp + 0xb8]
000000018032D15F jmp       0x18032d109
000000018032D161 mov       rdx, qword ptr [rip + 0x189a810] # 0x181bc7978
000000018032D168 mov       rcx, rbx
000000018032D16B call      0x180302170
000000018032D170 jmp       0x18032d232
000000018032D175 mov       rdx, qword ptr [rip + 0x189a7fc] # 0x181bc7978
000000018032D17C mov       rcx, qword ptr [rsp + 0x40]
000000018032D181 call      0x180302170
000000018032D186 mov       rcx, qword ptr [rsp + 0x38]
000000018032D18B test      rcx, rcx
000000018032D18E jne       0x18032d277
000000018032D194 mov       r13, qword ptr [rsp + 0xb0]
000000018032D19C mov       rdi, qword ptr [r13 + 0x10]
000000018032D1A0 mov       r14d, dword ptr [rsp + 0xb8]
000000018032D1A8 test      rdi, rdi
000000018032D1AB je        0x18032d25b
000000018032D1B1 mov       ebx, dword ptr [rdi + 0x18]
000000018032D1B4 mov       rcx, qword ptr [rip + 0x188983d] # 0x181bb69f8
000000018032D1BB call      0x180250100
000000018032D1C0 mov       rsi, rax
000000018032D1C3 xor       r9d, r9d
000000018032D1C6 mov       r8d, ebx
000000018032D1C9 mov       edx, r14d
000000018032D1CC mov       rcx, rax
000000018032D1CF call      0x18033deb0
000000018032D1D4 mov       rax, qword ptr [rip + 0x188aeb5] # 0x181bb8090
000000018032D1DB inc       dword ptr [rdi + 0x1c]
000000018032D1DE mov       rdx, qword ptr [rdi + 0x10]
000000018032D1E2 movsxd    r8, dword ptr [rdi + 0x18]
000000018032D1E6 test      rdx, rdx
000000018032D1E9 je        0x18032d25b
000000018032D1EB cmp       r8d, dword ptr [rdx + 0x18]
000000018032D1EF jb        0x18032d20d
000000018032D1F1 mov       rax, qword ptr [rax + 0x20]
000000018032D1F5 mov       r8, qword ptr [rax + 0xc0]
000000018032D1FC mov       r8, qword ptr [r8 + 0x70]
000000018032D200 mov       rdx, rsi
000000018032D203 mov       rcx, rdi
000000018032D206 call      0x180847ce0
000000018032D20B jmp       0x18032d232
000000018032D20D lea       eax, [r8 + 1]
000000018032D211 mov       dword ptr [rdi + 0x18], eax
000000018032D214 mov       rax, r8
000000018032D217 cmp       r8d, dword ptr [rdx + 0x18]
000000018032D21B jae       0x18032d27d
000000018032D21D mov       qword ptr [rdx + r8*8 + 0x20], rsi
000000018032D222 add       rax, 4
000000018032D226 lea       rcx, [rdx + rax*8]
000000018032D22A mov       rdx, rsi
000000018032D22D call      0x18024f360
000000018032D232 mov       rcx, qword ptr [r13 + 0x10]
000000018032D236 test      rcx, rcx
000000018032D239 je        0x18032d25b
000000018032D23B mov       eax, dword ptr [rsp + 0xc0]
000000018032D242 cmp       dword ptr [rcx + 0x18], eax
000000018032D245 jl        0x18032d19c
000000018032D24B add       rsp, 0x70
000000018032D24F pop       r15
000000018032D251 pop       r14
000000018032D253 pop       r13
000000018032D255 pop       r12
000000018032D257 pop       rdi
000000018032D258 pop       rsi
000000018032D259 pop       rbx
000000018032D25A ret       
000000018032D25B call      0x180250150
000000018032D260 nop       
000000018032D261 call      0x180250150
000000018032D266 nop       
000000018032D267 call      0x180250150
000000018032D26C call      0x180250150
000000018032D271 call      0x180250150
000000018032D276 nop       
000000018032D277 call      0x180200e60
000000018032D27C int3      
000000018032D27D call      0x180250140
000000018032D282 int3      
000000018032D283 int3      
000000018032D284 int3      
000000018032D285 int3      
000000018032D286 int3      
000000018032D287 int3      
000000018032D288 int3      
000000018032D289 int3      
000000018032D28A int3      
000000018032D28B int3      
000000018032D28C int3      
000000018032D28D int3      
000000018032D28E int3      
000000018032D28F int3      