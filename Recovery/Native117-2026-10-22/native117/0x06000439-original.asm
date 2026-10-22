000000018035F040 mov       qword ptr [rsp + 0x10], rbx
000000018035F045 mov       qword ptr [rsp + 0x18], rsi
000000018035F04A mov       qword ptr [rsp + 8], rcx
000000018035F04F push      rdi
000000018035F050 push      r14
000000018035F052 push      r15
000000018035F054 sub       rsp, 0x70
000000018035F058 movzx     ebx, dl
000000018035F05B mov       rdi, rcx
000000018035F05E cmp       byte ptr [rip + 0x1985cef], 0 # 0x181ce4d54
000000018035F065 jne       0x18035f13e
000000018035F06B lea       rcx, [rip + 0x1869b66] # 0x181bc8bd8
000000018035F072 call      0x18024fef0
000000018035F077 lea       rcx, [rip + 0x1869bb2] # 0x181bc8c30
000000018035F07E call      0x18024fef0
000000018035F083 lea       rcx, [rip + 0x1869bf6] # 0x181bc8c80
000000018035F08A call      0x18024fef0
000000018035F08F lea       rcx, [rip + 0x1842f82] # 0x181ba2018
000000018035F096 call      0x18024fef0
000000018035F09B lea       rcx, [rip + 0x185c47e] # 0x181bbb520
000000018035F0A2 call      0x18024fef0
000000018035F0A7 lea       rcx, [rip + 0x185c5d2] # 0x181bbb680
000000018035F0AE call      0x18024fef0
000000018035F0B3 lea       rcx, [rip + 0x185c35e] # 0x181bbb418
000000018035F0BA call      0x18024fef0
000000018035F0BF lea       rcx, [rip + 0x18618c2] # 0x181bc0988
000000018035F0C6 call      0x18024fef0
000000018035F0CB lea       rcx, [rip + 0x1875ef6] # 0x181bd4fc8
000000018035F0D2 call      0x18024fef0
000000018035F0D7 lea       rcx, [rip + 0x186e20a] # 0x181bcd2e8
000000018035F0DE call      0x18024fef0
000000018035F0E3 lea       rcx, [rip + 0x186cdde] # 0x181bcbec8
000000018035F0EA call      0x18024fef0
000000018035F0EF lea       rcx, [rip + 0x187613a] # 0x181bd5230
000000018035F0F6 call      0x18024fef0
000000018035F0FB lea       rcx, [rip + 0x1846ca6] # 0x181ba5da8
000000018035F102 call      0x18024fef0
000000018035F107 lea       rcx, [rip + 0x1875e62] # 0x181bd4f70
000000018035F10E call      0x18024fef0
000000018035F113 lea       rcx, [rip + 0x1846cee] # 0x181ba5e08
000000018035F11A call      0x18024fef0
000000018035F11F lea       rcx, [rip + 0x186cdf2] # 0x181bcbf18
000000018035F126 call      0x18024fef0
000000018035F12B lea       rcx, [rip + 0x1846d36] # 0x181ba5e68
000000018035F132 call      0x18024fef0
000000018035F137 mov       byte ptr [rip + 0x1985c16], 1 # 0x181ce4d54
000000018035F13E test      bl, bl
000000018035F140 jne       0x18035f2c5
000000018035F146 mov       eax, dword ptr [rdi + 0x84]
000000018035F14C dec       eax
000000018035F14E cmp       eax, 5
000000018035F151 ja        0x18035f2c5
000000018035F157 cdqe      
000000018035F159 lea       rsi, [rip - 0x35f160] # 0x180000000
000000018035F160 mov       eax, dword ptr [rsi + rax*4 + 0x35f530]
000000018035F167 add       rax, rsi
000000018035F16A jmp       rax
000000018035F16C xor       ecx, ecx
000000018035F16E call      0x1802fb100
000000018035F173 test      rax, rax
000000018035F176 je        0x18035f521
000000018035F17C xor       r8d, r8d
000000018035F17F mov       edx, 3
000000018035F184 mov       rcx, rax
000000018035F187 call      0x180321f30
000000018035F18C mov       rbx, rax
000000018035F18F mov       r14, qword ptr [rdi + 0x1b0]
000000018035F196 mov       rcx, qword ptr [rip + 0x1842e7b] # 0x181ba2018
000000018035F19D cmp       dword ptr [rcx + 0xe0], 0
000000018035F1A4 jne       0x18035f1ab
000000018035F1A6 call      0x1802501e0
000000018035F1AB mov       r8, qword ptr [rip + 0x1846cb6] # 0x181ba5e68
000000018035F1B2 xor       r9d, r9d
000000018035F1B5 mov       rdx, r14
000000018035F1B8 lea       rcx, [rsp + 0x30]
000000018035F1BD call      0x18031b680
000000018035F1C2 movsd     xmm0, qword ptr [rax]
000000018035F1C6 movsd     qword ptr [rsp + 0xa8], xmm0
000000018035F1CF mov       r14d, dword ptr [rax + 8]
000000018035F1D3 test      rbx, rbx
000000018035F1D6 je        0x18035f521
000000018035F1DC xor       edx, edx
000000018035F1DE mov       rcx, rbx
000000018035F1E1 call      0x18131c070
000000018035F1E6 test      rax, rax
000000018035F1E9 je        0x18035f521
000000018035F1EF movsd     xmm0, qword ptr [rsp + 0xa8]
000000018035F1F8 movsd     qword ptr [rsp + 0x30], xmm0
000000018035F1FE mov       dword ptr [rsp + 0x38], r14d
000000018035F203 xor       r8d, r8d
000000018035F206 lea       rdx, [rsp + 0x30]
000000018035F20B mov       rcx, rax
000000018035F20E call      0x18132fbb0
000000018035F213 jmp       0x18035f2cc
000000018035F218 xor       ecx, ecx
000000018035F21A call      0x1802fb100
000000018035F21F test      rax, rax
000000018035F222 je        0x18035f521
000000018035F228 mov       edx, 1
000000018035F22D xor       r8d, r8d
000000018035F230 mov       rcx, rax
000000018035F233 call      0x180321f30
000000018035F238 mov       rbx, rax
000000018035F23B mov       r14, qword ptr [rdi + 0x1b0]
000000018035F242 mov       rcx, qword ptr [rip + 0x1842dcf] # 0x181ba2018
000000018035F249 cmp       dword ptr [rcx + 0xe0], 0
000000018035F250 jne       0x18035f257
000000018035F252 call      0x1802501e0
000000018035F257 mov       r8, qword ptr [rip + 0x1846baa] # 0x181ba5e08
000000018035F25E jmp       0x18035f1b2
000000018035F263 xor       ecx, ecx
000000018035F265 call      0x1802fb100
000000018035F26A test      rax, rax
000000018035F26D je        0x18035f521
000000018035F273 mov       edx, 8
000000018035F278 jmp       0x18035f22d
000000018035F27A xor       ecx, ecx
000000018035F27C call      0x1802fb100
000000018035F281 test      rax, rax
000000018035F284 je        0x18035f521
000000018035F28A xor       r8d, r8d
000000018035F28D mov       edx, 0x1b
000000018035F292 mov       rcx, rax
000000018035F295 call      0x180321f30
000000018035F29A mov       rbx, rax
000000018035F29D mov       r14, qword ptr [rdi + 0x1b0]
000000018035F2A4 mov       rcx, qword ptr [rip + 0x1842d6d] # 0x181ba2018
000000018035F2AB cmp       dword ptr [rcx + 0xe0], 0
000000018035F2B2 jne       0x18035f2b9
000000018035F2B4 call      0x1802501e0
000000018035F2B9 mov       r8, qword ptr [rip + 0x1875cb0] # 0x181bd4f70
000000018035F2C0 jmp       0x18035f1b2
000000018035F2C5 lea       rsi, [rip - 0x35f2cc] # 0x180000000
000000018035F2CC mov       rcx, qword ptr [rip + 0x18616b5] # 0x181bc0988
000000018035F2D3 call      0x180250100
000000018035F2D8 mov       rbx, rax
000000018035F2DB mov       rdx, qword ptr [rip + 0x185c136] # 0x181bbb418
000000018035F2E2 mov       rcx, rax
000000018035F2E5 call      0x1808053d0
000000018035F2EA mov       ecx, dword ptr [rdi + 0x84]
000000018035F2F0 dec       ecx
000000018035F2F2 cmp       ecx, 5
000000018035F2F5 ja        0x18035f3f1
000000018035F2FB movsxd    rcx, ecx
000000018035F2FE mov       edx, dword ptr [rsi + rcx*4 + 0x35f548]
000000018035F305 add       rdx, rsi
000000018035F308 jmp       rdx
000000018035F30A test      rbx, rbx
000000018035F30D je        0x18035f521
000000018035F313 mov       r8, qword ptr [rip + 0x185c206] # 0x181bbb520
000000018035F31A mov       rdx, qword ptr [rip + 0x1846b47] # 0x181ba5e68
000000018035F321 mov       rcx, rbx
000000018035F324 call      0x180002e70
000000018035F329 jmp       0x18035f3fa
000000018035F32E test      rbx, rbx
000000018035F331 je        0x18035f521
000000018035F337 mov       r8, qword ptr [rip + 0x185c1e2] # 0x181bbb520
000000018035F33E mov       rdx, qword ptr [rip + 0x1846a63] # 0x181ba5da8
000000018035F345 mov       rcx, rbx
000000018035F348 call      0x180002e70
000000018035F34D jmp       0x18035f3fa
000000018035F352 test      rbx, rbx
000000018035F355 je        0x18035f521
000000018035F35B mov       r8, qword ptr [rip + 0x185c1be] # 0x181bbb520
000000018035F362 mov       rdx, qword ptr [rip + 0x186cb5f] # 0x181bcbec8
000000018035F369 mov       rcx, rbx
000000018035F36C call      0x180002e70
000000018035F371 mov       r8, qword ptr [rip + 0x185c1a8] # 0x181bbb520
000000018035F378 mov       rdx, qword ptr [rip + 0x186cb99] # 0x181bcbf18
000000018035F37F mov       rcx, rbx
000000018035F382 call      0x180002e70
000000018035F387 jmp       0x18035f3fa
000000018035F389 test      rbx, rbx
000000018035F38C je        0x18035f521
000000018035F392 mov       r8, qword ptr [rip + 0x185c187] # 0x181bbb520
000000018035F399 mov       rdx, qword ptr [rip + 0x1846a68] # 0x181ba5e08
000000018035F3A0 mov       rcx, rbx
000000018035F3A3 call      0x180002e70
000000018035F3A8 jmp       0x18035f3fa
000000018035F3AA test      rbx, rbx
000000018035F3AD je        0x18035f521
000000018035F3B3 mov       r8, qword ptr [rip + 0x185c166] # 0x181bbb520
000000018035F3BA mov       rdx, qword ptr [rip + 0x1875e6f] # 0x181bd5230
000000018035F3C1 mov       rcx, rbx
000000018035F3C4 call      0x180002e70
000000018035F3C9 mov       r8, qword ptr [rip + 0x185c150] # 0x181bbb520
000000018035F3D0 mov       rdx, qword ptr [rip + 0x1875bf1] # 0x181bd4fc8
000000018035F3D7 mov       rcx, rbx
000000018035F3DA call      0x180002e70
000000018035F3DF cmp       dword ptr [rdi + 0x60], 0x10
000000018035F3E3 jne       0x18035f3fa
000000018035F3E5 mov       dword ptr [rdi + 0xe0], 0x3f800000
000000018035F3EF jmp       0x18035f3fa
000000018035F3F1 test      rbx, rbx
000000018035F3F4 je        0x18035f521
000000018035F3FA mov       r8, qword ptr [rip + 0x185c27f] # 0x181bbb680
000000018035F401 mov       rdx, rbx
000000018035F404 lea       rcx, [rsp + 0x30]
000000018035F409 call      0x1808197f0
000000018035F40E movups    xmm0, xmmword ptr [rsp + 0x30]
000000018035F413 movups    xmmword ptr [rsp + 0x50], xmm0
000000018035F418 movsd     xmm1, qword ptr [rsp + 0x40]
000000018035F41E movsd     qword ptr [rsp + 0x60], xmm1
000000018035F424 xor       r15d, r15d
000000018035F427 mov       qword ptr [rsp + 0x30], r15
000000018035F42C lea       rbx, [rsp + 0x50]
000000018035F431 mov       qword ptr [rsp + 0x38], rbx
000000018035F436 nop       word ptr [rax + rax]
000000018035F440 mov       rdx, qword ptr [rip + 0x18697e9] # 0x181bc8c30
000000018035F447 lea       rcx, [rsp + 0x50]
000000018035F44C call      0x180669280
000000018035F451 test      al, al
000000018035F453 je        0x18035f486
000000018035F455 mov       rsi, qword ptr [rdi + 0x1b0]
000000018035F45C mov       r14, qword ptr [rsp + 0x60]
000000018035F461 mov       rcx, qword ptr [rip + 0x1842bb0] # 0x181ba2018
000000018035F468 cmp       dword ptr [rcx + 0xe0], 0
000000018035F46F jne       0x18035f476
000000018035F471 call      0x1802501e0
000000018035F476 xor       r8d, r8d
000000018035F479 mov       rdx, r14
000000018035F47C mov       rcx, rsi
000000018035F47F call      0x18031bac0
000000018035F484 jmp       0x18035f440
000000018035F486 mov       rdx, qword ptr [rip + 0x186974b] # 0x181bc8bd8
000000018035F48D mov       rcx, rbx
000000018035F490 call      0x180302170
000000018035F495 jmp       0x18035f4bd
000000018035F497 mov       rdx, qword ptr [rip + 0x186973a] # 0x181bc8bd8
000000018035F49E mov       rcx, qword ptr [rsp + 0x38]
000000018035F4A3 call      0x180302170
000000018035F4A8 mov       rcx, qword ptr [rsp + 0x30]
000000018035F4AD test      rcx, rcx
000000018035F4B0 jne       0x18035f527
000000018035F4B2 xor       r15d, r15d
000000018035F4B5 mov       rdi, qword ptr [rsp + 0x90]
000000018035F4BD mov       rbx, qword ptr [rdi + 0x1b0]
000000018035F4C4 mov       rcx, qword ptr [rip + 0x1842b4d] # 0x181ba2018
000000018035F4CB cmp       dword ptr [rcx + 0xe0], 0
000000018035F4D2 jne       0x18035f4d9
000000018035F4D4 call      0x1802501e0
000000018035F4D9 xor       r8d, r8d
000000018035F4DC mov       rdx, qword ptr [rip + 0x186de05] # 0x181bcd2e8
000000018035F4E3 mov       rcx, rbx
000000018035F4E6 call      0x18031adf0
000000018035F4EB mov       qword ptr [rdi + 0x88], 0
000000018035F4F6 mov       dword ptr [rdi + 0x90], r15d
000000018035F4FD mov       dword ptr [rdi + 0x84], r15d
000000018035F504 mov       dword ptr [rdi + 0x98], r15d
000000018035F50B lea       r11, [rsp + 0x70]
000000018035F510 mov       rbx, qword ptr [r11 + 0x28]
000000018035F514 mov       rsi, qword ptr [r11 + 0x30]
000000018035F518 mov       rsp, r11
000000018035F51B pop       r15
000000018035F51D pop       r14
000000018035F51F pop       rdi
000000018035F520 ret       
000000018035F521 call      0x180250150
000000018035F526 int3      
000000018035F527 call      0x180200e60
000000018035F52C int3      
000000018035F52D nop       dword ptr [rax]
000000018035F530 insb      byte ptr [rdi], dx
000000018035F531 int1      
000000018035F532 xor       eax, 0x35f21800
000000018035F537 add       ah, cl
000000018035F539 xor       eax, 0x35f2cc00
000000018035F53F add       byte ptr [rbx - 0xe], ah
000000018035F542 xor       eax, 0x35f27a00
000000018035F547 add       byte ptr [rdx], cl
000000018035F549 xor       eax, 0x35f38900
000000018035F54F add       byte ptr [rsi], ch
000000018035F551 xor       eax, 0x35f35200
000000018035F557 add       byte ptr [rcx - 0x55ffca0d], cl