00000001803A92A0 mov       qword ptr [rsp + 0x18], rbx
00000001803A92A5 mov       dword ptr [rsp + 0x10], edx
00000001803A92A9 mov       qword ptr [rsp + 8], rcx
00000001803A92AE push      rsi
00000001803A92AF push      rdi
00000001803A92B0 push      r14
00000001803A92B2 sub       rsp, 0x70
00000001803A92B6 mov       edi, edx
00000001803A92B8 mov       rsi, rcx
00000001803A92BB cmp       byte ptr [rip + 0x193bc3d], 0 # 0x181ce4eff
00000001803A92C2 jne       0x1803a932b
00000001803A92C4 lea       rcx, [rip + 0x181c39d] # 0x181bc5668
00000001803A92CB call      0x18024fef0
00000001803A92D0 lea       rcx, [rip + 0x181c3e9] # 0x181bc56c0
00000001803A92D7 call      0x18024fef0
00000001803A92DC lea       rcx, [rip + 0x181c435] # 0x181bc5718
00000001803A92E3 call      0x18024fef0
00000001803A92E8 lea       rcx, [rip + 0x1807da1] # 0x181bb1090
00000001803A92EF call      0x18024fef0
00000001803A92F4 lea       rcx, [rip + 0x1807ded] # 0x181bb10e8
00000001803A92FB call      0x18024fef0
00000001803A9300 lea       rcx, [rip + 0x1812589] # 0x181bbb890
00000001803A9307 call      0x18024fef0
00000001803A930C lea       rcx, [rip + 0x180e945] # 0x181bb7c58
00000001803A9313 call      0x18024fef0
00000001803A9318 lea       rcx, [rip + 0x18071e1] # 0x181bb0500
00000001803A931F call      0x18024fef0
00000001803A9324 mov       byte ptr [rip + 0x193bbd4], 1 # 0x181ce4eff
00000001803A932B mov       rdx, qword ptr [rsi + 0x38]
00000001803A932F test      rdx, rdx
00000001803A9332 je        0x1803a951f
00000001803A9338 mov       r8, qword ptr [rip + 0x1807da9] # 0x181bb10e8
00000001803A933F lea       rcx, [rsp + 0x38]
00000001803A9344 call      0x1808197f0
00000001803A9349 movups    xmm0, xmmword ptr [rsp + 0x38]
00000001803A934E movups    xmmword ptr [rsp + 0x50], xmm0
00000001803A9353 movsd     xmm1, qword ptr [rsp + 0x48]
00000001803A9359 movsd     qword ptr [rsp + 0x60], xmm1
00000001803A935F mov       qword ptr [rsp + 0x38], 0
00000001803A9368 lea       rbx, [rsp + 0x50]
00000001803A936D mov       qword ptr [rsp + 0x40], rbx
00000001803A9372 mov       rdx, qword ptr [rip + 0x181c347] # 0x181bc56c0
00000001803A9379 lea       rcx, [rsp + 0x50]
00000001803A937E call      0x180669280
00000001803A9383 test      al, al
00000001803A9385 je        0x1803a93b6
00000001803A9387 mov       rcx, qword ptr [rsp + 0x60]
00000001803A938C test      rcx, rcx
00000001803A938F je        0x1803a9525
00000001803A9395 cmp       dword ptr [rcx + 0x20], edi
00000001803A9398 jne       0x1803a9372
00000001803A939A xor       edx, edx
00000001803A939C call      0x18039ee10
00000001803A93A1 nop       
00000001803A93A2 mov       rdx, qword ptr [rip + 0x181c2bf] # 0x181bc5668
00000001803A93A9 mov       rcx, rbx
00000001803A93AC call      0x180302170
00000001803A93B1 jmp       0x1803a94d8
00000001803A93B6 mov       rdx, qword ptr [rip + 0x181c2ab] # 0x181bc5668
00000001803A93BD mov       rcx, rbx
00000001803A93C0 call      0x180302170
00000001803A93C5 jmp       0x1803a93f5
00000001803A93C7 mov       rdx, qword ptr [rip + 0x181c29a] # 0x181bc5668
00000001803A93CE mov       rcx, qword ptr [rsp + 0x40]
00000001803A93D3 call      0x180302170
00000001803A93D8 mov       rcx, qword ptr [rsp + 0x38]
00000001803A93DD test      rcx, rcx
00000001803A93E0 jne       0x1803a952b
00000001803A93E6 mov       edi, dword ptr [rsp + 0x98]
00000001803A93ED mov       rsi, qword ptr [rsp + 0x90]
00000001803A93F5 mov       rbx, qword ptr [rsi + 0x30]
00000001803A93F9 mov       rcx, qword ptr [rip + 0x1807100] # 0x181bb0500
00000001803A9400 cmp       dword ptr [rcx + 0xe0], 0
00000001803A9407 jne       0x1803a940e
00000001803A9409 call      0x1802501e0
00000001803A940E mov       rdx, qword ptr [rip + 0x180e843] # 0x181bb7c58
00000001803A9415 mov       rcx, rbx
00000001803A9418 call      0x1804a0690
00000001803A941D mov       rbx, rax
00000001803A9420 test      rax, rax
00000001803A9423 je        0x1803a951f
00000001803A9429 xor       edx, edx
00000001803A942B mov       rcx, rax
00000001803A942E call      0x181304510
00000001803A9433 mov       r14, rax
00000001803A9436 mov       rcx, qword ptr [rsi + 0x28]
00000001803A943A test      rcx, rcx
00000001803A943D je        0x1803a951f
00000001803A9443 xor       edx, edx
00000001803A9445 call      0x18131c070
00000001803A944A test      r14, r14
00000001803A944D je        0x1803a951f
00000001803A9453 xor       r9d, r9d
00000001803A9456 xor       r8d, r8d
00000001803A9459 mov       rdx, rax
00000001803A945C mov       rcx, r14
00000001803A945F call      0x18132e900
00000001803A9464 mov       rcx, qword ptr [rsi + 0x40]
00000001803A9468 test      rcx, rcx
00000001803A946B je        0x1803a951f
00000001803A9471 mov       r8, qword ptr [rip + 0x1812418] # 0x181bbb890
00000001803A9478 mov       edx, edi
00000001803A947A call      0x180825710
00000001803A947F mov       qword ptr [rsp + 0x20], 0
00000001803A9488 mov       r9, rax
00000001803A948B mov       r8d, 1
00000001803A9491 mov       edx, edi
00000001803A9493 mov       rcx, rbx
00000001803A9496 call      0x18039ec50
00000001803A949B mov       rcx, qword ptr [rsi + 0x38]
00000001803A949F test      rcx, rcx
00000001803A94A2 je        0x1803a951f
00000001803A94A4 mov       r9, qword ptr [rip + 0x1807be5] # 0x181bb1090
00000001803A94AB inc       dword ptr [rcx + 0x1c]
00000001803A94AE mov       rdx, qword ptr [rcx + 0x10]
00000001803A94B2 movsxd    r8, dword ptr [rcx + 0x18]
00000001803A94B6 test      rdx, rdx
00000001803A94B9 je        0x1803a951f
00000001803A94BB cmp       r8d, dword ptr [rdx + 0x18]
00000001803A94BF jb        0x1803a94e9
00000001803A94C1 mov       rax, qword ptr [r9 + 0x20]
00000001803A94C5 mov       r8, qword ptr [rax + 0xc0]
00000001803A94CC mov       r8, qword ptr [r8 + 0x70]
00000001803A94D0 mov       rdx, rbx
00000001803A94D3 call      0x180847ce0
00000001803A94D8 mov       rbx, qword ptr [rsp + 0xa0]
00000001803A94E0 add       rsp, 0x70
00000001803A94E4 pop       r14
00000001803A94E6 pop       rdi
00000001803A94E7 pop       rsi
00000001803A94E8 ret       
00000001803A94E9 lea       eax, [r8 + 1]
00000001803A94ED mov       dword ptr [rcx + 0x18], eax
00000001803A94F0 mov       rax, r8
00000001803A94F3 cmp       r8d, dword ptr [rdx + 0x18]
00000001803A94F7 jae       0x1803a9531
00000001803A94F9 mov       qword ptr [rdx + r8*8 + 0x20], rbx
00000001803A94FE add       rax, 4
00000001803A9502 lea       rcx, [rdx + rax*8]
00000001803A9506 mov       rdx, rbx
00000001803A9509 call      0x18024f360
00000001803A950E mov       rbx, qword ptr [rsp + 0xa0]
00000001803A9516 add       rsp, 0x70
00000001803A951A pop       r14
00000001803A951C pop       rdi
00000001803A951D pop       rsi
00000001803A951E ret       
00000001803A951F call      0x180250150
00000001803A9524 nop       
00000001803A9525 call      0x180250150
00000001803A952A nop       
00000001803A952B call      0x180200e60
00000001803A9530 int3      
00000001803A9531 call      0x180250140
00000001803A9536 int3      
00000001803A9537 int3      
00000001803A9538 int3      
00000001803A9539 int3      
00000001803A953A int3      
00000001803A953B int3      
00000001803A953C int3      
00000001803A953D int3      
00000001803A953E int3      
00000001803A953F int3      