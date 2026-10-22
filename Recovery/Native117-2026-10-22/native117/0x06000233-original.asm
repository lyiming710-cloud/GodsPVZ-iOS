000000018033B070 mov       qword ptr [rsp + 8], rbx
000000018033B075 push      rsi
000000018033B076 push      rdi
000000018033B077 push      r14
000000018033B079 sub       rsp, 0x60
000000018033B07D cmp       byte ptr [rip + 0x19a9ba7], 0 # 0x181ce4c2b
000000018033B084 jne       0x18033b15d
000000018033B08A lea       rcx, [rip + 0x1863bb7] # 0x181b9ec48
000000018033B091 call      0x18024fef0
000000018033B096 lea       rcx, [rip + 0x1869e53] # 0x181ba4ef0
000000018033B09D call      0x18024fef0
000000018033B0A2 lea       rcx, [rip + 0x186a1a7] # 0x181ba5250
000000018033B0A9 call      0x18024fef0
000000018033B0AE lea       rcx, [rip + 0x1871643] # 0x181bac6f8
000000018033B0B5 call      0x18024fef0
000000018033B0BA lea       rcx, [rip + 0x187f11f] # 0x181bba1e0
000000018033B0C1 call      0x18024fef0
000000018033B0C6 lea       rcx, [rip + 0x187f21b] # 0x181bba2e8
000000018033B0CD call      0x18024fef0
000000018033B0D2 lea       rcx, [rip + 0x187f2bf] # 0x181bba398
000000018033B0D9 call      0x18024fef0
000000018033B0DE lea       rcx, [rip + 0x187541b] # 0x181bb0500
000000018033B0E5 call      0x18024fef0
000000018033B0EA lea       rcx, [rip + 0x18767f7] # 0x181bb18e8
000000018033B0F1 call      0x18024fef0
000000018033B0F6 lea       rcx, [rip + 0x187b37b] # 0x181bb6478
000000018033B0FD call      0x18024fef0
000000018033B102 lea       rcx, [rip + 0x1884be7] # 0x181bbfcf0
000000018033B109 call      0x18024fef0
000000018033B10E lea       rcx, [rip + 0x1868f1b] # 0x181ba4030
000000018033B115 call      0x18024fef0
000000018033B11A lea       rcx, [rip + 0x1889607] # 0x181bc4728
000000018033B121 call      0x18024fef0
000000018033B126 lea       rcx, [rip + 0x186f863] # 0x181baa990
000000018033B12D call      0x18024fef0
000000018033B132 lea       rcx, [rip + 0x189182f] # 0x181bcc968
000000018033B139 call      0x18024fef0
000000018033B13E lea       rcx, [rip + 0x187a653] # 0x181bb5798
000000018033B145 call      0x18024fef0
000000018033B14A lea       rcx, [rip + 0x189003f] # 0x181bcb190
000000018033B151 call      0x18024fef0
000000018033B156 mov       byte ptr [rip + 0x19a9ace], 1 # 0x181ce4c2b
000000018033B15D xor       edi, edi
000000018033B15F mov       qword ptr [rsp + 0x98], rdi
000000018033B167 mov       rcx, qword ptr [rip + 0x187b30a] # 0x181bb6478
000000018033B16E cmp       dword ptr [rcx + 0xe0], edi
000000018033B174 jne       0x18033b182
000000018033B176 call      0x1802501e0
000000018033B17B mov       rcx, qword ptr [rip + 0x187b2f6] # 0x181bb6478
000000018033B182 mov       rax, qword ptr [rcx + 0xb8]
000000018033B189 mov       rdx, qword ptr [rax + 0xe0]
000000018033B190 test      rdx, rdx
000000018033B193 je        0x18033b520
000000018033B199 cmp       dword ptr [rdx + 0x18], edi
000000018033B19C jg        0x18033b503
000000018033B1A2 mov       rcx, qword ptr [rip + 0x187673f] # 0x181bb18e8
000000018033B1A9 cmp       dword ptr [rcx + 0xe0], edi
000000018033B1AF jne       0x18033b1b6
000000018033B1B1 call      0x1802501e0
000000018033B1B6 mov       qword ptr [rsp + 0x20], rdi
000000018033B1BB mov       r9, qword ptr [rip + 0x187a5d6] # 0x181bb5798
000000018033B1C2 mov       r8, qword ptr [rip + 0x189179f] # 0x181bcc968
000000018033B1C9 mov       rdx, qword ptr [rip + 0x186f7c0] # 0x181baa990
000000018033B1D0 mov       rcx, qword ptr [rip + 0x188ffb9] # 0x181bcb190
000000018033B1D7 call      0x180c84930
000000018033B1DC mov       r14, rax
000000018033B1DF mov       rbx, qword ptr [rip + 0x1868e4a] # 0x181ba4030
000000018033B1E6 mov       rcx, qword ptr [rip + 0x1884b03] # 0x181bbfcf0
000000018033B1ED cmp       dword ptr [rcx + 0xe0], edi
000000018033B1F3 jne       0x18033b1fa
000000018033B1F5 call      0x1802501e0
000000018033B1FA xor       edx, edx
000000018033B1FC mov       rcx, rbx
000000018033B1FF call      0x180ccee30
000000018033B204 mov       rbx, rax
000000018033B207 mov       rcx, qword ptr [rip + 0x1863a3a] # 0x181b9ec48
000000018033B20E cmp       dword ptr [rcx + 0xe0], edi
000000018033B214 jne       0x18033b21b
000000018033B216 call      0x1802501e0
000000018033B21B xor       edx, edx
000000018033B21D mov       rcx, rbx
000000018033B220 call      0x180cdd3b0
000000018033B225 test      rax, rax
000000018033B228 je        0x18033b520
000000018033B22E xor       edx, edx
000000018033B230 mov       rcx, rax
000000018033B233 call      0x180cba170
000000018033B238 mov       qword ptr [rsp + 0x90], rax
000000018033B240 lea       rax, [rsp + 0x90]
000000018033B248 mov       qword ptr [rsp + 0x38], rax
000000018033B24D lea       rax, [rsp + 0x98]
000000018033B255 mov       qword ptr [rsp + 0x40], rax
000000018033B25A mov       qword ptr [rsp + 0x48], rdi
000000018033B25F movups    xmm0, xmmword ptr [rsp + 0x38]
000000018033B264 movups    xmmword ptr [rsp + 0x50], xmm0
000000018033B269 nop       dword ptr [rax]
000000018033B270 mov       r8, qword ptr [rsp + 0x90]
000000018033B278 test      r8, r8
000000018033B27B je        0x18033b548
000000018033B281 xor       ecx, ecx
000000018033B283 mov       rdx, qword ptr [rip + 0x1869fc6] # 0x181ba5250
000000018033B28A call      0x180002cd0
000000018033B28F test      al, al
000000018033B291 je        0x18033b46f
000000018033B297 mov       r8, qword ptr [rsp + 0x90]
000000018033B29F test      r8, r8
000000018033B2A2 je        0x18033b543
000000018033B2A8 mov       ecx, 1
000000018033B2AD mov       rdx, qword ptr [rip + 0x1869f9c] # 0x181ba5250
000000018033B2B4 call      0x180002cd0
000000018033B2B9 mov       rcx, rax
000000018033B2BC mov       r8, qword ptr [rip + 0x1889465] # 0x181bc4728
000000018033B2C3 test      rax, rax
000000018033B2C6 je        0x18033b53e
000000018033B2CC mov       rdx, qword ptr [rax]
000000018033B2CF mov       rax, qword ptr [r8 + 0x40]
000000018033B2D3 cmp       qword ptr [rdx + 0x40], rax
000000018033B2D7 jne       0x18033b536
000000018033B2DD call      0x18024f540
000000018033B2E2 mov       edi, dword ptr [rax]
000000018033B2E4 mov       rbx, qword ptr [rip + 0x1868d45] # 0x181ba4030
000000018033B2EB mov       rcx, qword ptr [rip + 0x18849fe] # 0x181bbfcf0
000000018033B2F2 cmp       dword ptr [rcx + 0xe0], 0
000000018033B2F9 jne       0x18033b300
000000018033B2FB call      0x1802501e0
000000018033B300 xor       edx, edx
000000018033B302 mov       rcx, rbx
000000018033B305 call      0x180ccee30
000000018033B30A mov       rsi, rax
000000018033B30D mov       dword ptr [rsp + 0x88], edi
000000018033B314 lea       rdx, [rsp + 0x88]
000000018033B31C mov       rcx, qword ptr [rip + 0x1889405] # 0x181bc4728
000000018033B323 call      0x18024f340
000000018033B328 mov       rbx, rax
000000018033B32B mov       rcx, qword ptr [rip + 0x1863916] # 0x181b9ec48
000000018033B332 cmp       dword ptr [rcx + 0xe0], 0
000000018033B339 jne       0x18033b340
000000018033B33B call      0x1802501e0
000000018033B340 xor       r8d, r8d
000000018033B343 mov       rdx, rbx
000000018033B346 mov       rcx, rsi
000000018033B349 call      0x180cdcd20
000000018033B34E mov       rbx, rax
000000018033B351 mov       rcx, qword ptr [rip + 0x1876590] # 0x181bb18e8
000000018033B358 cmp       dword ptr [rcx + 0xe0], 0
000000018033B35F jne       0x18033b366
000000018033B361 call      0x1802501e0
000000018033B366 xor       r8d, r8d
000000018033B369 mov       rdx, rbx
000000018033B36C mov       rcx, r14
000000018033B36F call      0x180c84600
000000018033B374 mov       rdx, qword ptr [rip + 0x187137d] # 0x181bac6f8
000000018033B37B mov       rcx, rax
000000018033B37E call      0x18047ba00
000000018033B383 mov       rbx, rax
000000018033B386 mov       rcx, qword ptr [rip + 0x1875173] # 0x181bb0500
000000018033B38D cmp       dword ptr [rcx + 0xe0], 0
000000018033B394 jne       0x18033b39b
000000018033B396 call      0x1802501e0
000000018033B39B xor       edx, edx
000000018033B39D mov       rcx, rbx
000000018033B3A0 call      0x18131f870
000000018033B3A5 test      al, al
000000018033B3A7 je        0x18033b270
000000018033B3AD nop       dword ptr [rax]
000000018033B3B0 mov       rax, qword ptr [rip + 0x187b0c1] # 0x181bb6478
000000018033B3B7 cmp       dword ptr [rax + 0xe0], 0
000000018033B3BE jne       0x18033b3cf
000000018033B3C0 mov       rcx, rax
000000018033B3C3 call      0x1802501e0
000000018033B3C8 mov       rax, qword ptr [rip + 0x187b0a9] # 0x181bb6478
000000018033B3CF mov       rcx, qword ptr [rax + 0xb8]
000000018033B3D6 mov       rdx, qword ptr [rcx + 0xe0]
000000018033B3DD test      rdx, rdx
000000018033B3E0 je        0x18033b531
000000018033B3E6 cmp       dword ptr [rdx + 0x18], edi
000000018033B3E9 jg        0x18033b42a
000000018033B3EB cmp       dword ptr [rax + 0xe0], 0
000000018033B3F2 jne       0x18033b403
000000018033B3F4 mov       rcx, rax
000000018033B3F7 call      0x1802501e0
000000018033B3FC mov       rax, qword ptr [rip + 0x187b075] # 0x181bb6478
000000018033B403 mov       rcx, qword ptr [rax + 0xb8]
000000018033B40A mov       rcx, qword ptr [rcx + 0xe0]
000000018033B411 test      rcx, rcx
000000018033B414 je        0x18033b526
000000018033B41A mov       r8, qword ptr [rip + 0x187edbf] # 0x181bba1e0
000000018033B421 xor       edx, edx
000000018033B423 call      0x180002e70
000000018033B428 jmp       0x18033b3b0
000000018033B42A cmp       dword ptr [rax + 0xe0], 0
000000018033B431 jne       0x18033b442
000000018033B433 mov       rcx, rax
000000018033B436 call      0x1802501e0
000000018033B43B mov       rax, qword ptr [rip + 0x187b036] # 0x181bb6478
000000018033B442 mov       rcx, qword ptr [rax + 0xb8]
000000018033B449 mov       rcx, qword ptr [rcx + 0xe0]
000000018033B450 test      rcx, rcx
000000018033B453 je        0x18033b52c
000000018033B459 mov       r9, qword ptr [rip + 0x187ef38] # 0x181bba398
000000018033B460 mov       r8, rbx
000000018033B463 mov       edx, edi
000000018033B465 call      0x180855ab0
000000018033B46A jmp       0x18033b270
000000018033B46F mov       rax, qword ptr [rsp + 0x38]
000000018033B474 mov       rdx, qword ptr [rip + 0x1869a75] # 0x181ba4ef0
000000018033B47B mov       rcx, qword ptr [rax]
000000018033B47E call      0x18024f380
000000018033B483 mov       r8, rax
000000018033B486 movups    xmm0, xmmword ptr [rsp + 0x38]
000000018033B48B psrldq    xmm0, 8
000000018033B490 movq      rax, xmm0
000000018033B495 mov       qword ptr [rax], r8
000000018033B498 test      r8, r8
000000018033B49B je        0x18033b4ab
000000018033B49D xor       ecx, ecx
000000018033B49F mov       rdx, qword ptr [rip + 0x1869a4a] # 0x181ba4ef0
000000018033B4A6 call      0x180002d90
000000018033B4AB jmp       0x18033b4c5
000000018033B4AD lea       rcx, [rsp + 0x50]
000000018033B4B2 call      0x1802fb000
000000018033B4B7 mov       rcx, qword ptr [rsp + 0x48]
000000018033B4BC test      rcx, rcx
000000018033B4BF jne       0x18033b54e
000000018033B4C5 mov       rax, qword ptr [rip + 0x187afac] # 0x181bb6478
000000018033B4CC cmp       dword ptr [rax + 0xe0], 0
000000018033B4D3 jne       0x18033b4e4
000000018033B4D5 mov       rcx, rax
000000018033B4D8 call      0x1802501e0
000000018033B4DD mov       rax, qword ptr [rip + 0x187af94] # 0x181bb6478
000000018033B4E4 mov       rax, qword ptr [rax + 0xb8]
000000018033B4EB mov       rax, qword ptr [rax + 0xe0]
000000018033B4F2 mov       rbx, qword ptr [rsp + 0x80]
000000018033B4FA add       rsp, 0x60
000000018033B4FE pop       r14
000000018033B500 pop       rdi
000000018033B501 pop       rsi
000000018033B502 ret       
000000018033B503 cmp       dword ptr [rcx + 0xe0], edi
000000018033B509 jne       0x18033b517
000000018033B50B call      0x1802501e0
000000018033B510 mov       rcx, qword ptr [rip + 0x187af61] # 0x181bb6478
000000018033B517 mov       rax, qword ptr [rcx + 0xb8]
000000018033B51E jmp       0x18033b4eb
000000018033B520 call      0x180250150
000000018033B525 nop       
000000018033B526 call      0x180250150
000000018033B52B nop       
000000018033B52C call      0x180250150
000000018033B531 call      0x180250150
000000018033B536 mov       rdx, r8
000000018033B539 call      0x18024f3a0
000000018033B53E call      0x180250150
000000018033B543 call      0x180250150
000000018033B548 call      0x180250150
000000018033B54D nop       
000000018033B54E call      0x180200e60
000000018033B553 int3      
000000018033B554 int3      
000000018033B555 int3      
000000018033B556 int3      
000000018033B557 int3      
000000018033B558 int3      
000000018033B559 int3      
000000018033B55A int3      
000000018033B55B int3      
000000018033B55C int3      
000000018033B55D int3      
000000018033B55E int3      
000000018033B55F int3      