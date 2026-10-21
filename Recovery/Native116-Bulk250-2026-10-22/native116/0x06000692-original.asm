00000001803A8340 mov       rax, rsp
00000001803A8343 mov       qword ptr [rax + 0x10], rbx
00000001803A8347 mov       qword ptr [rax + 0x18], rsi
00000001803A834B mov       qword ptr [rax + 8], rcx
00000001803A834F push      rdi
00000001803A8350 push      r14
00000001803A8352 push      r15
00000001803A8354 sub       rsp, 0xb0
00000001803A835B movaps    xmmword ptr [rax - 0x28], xmm6
00000001803A835F movaps    xmmword ptr [rax - 0x38], xmm7
00000001803A8363 movaps    xmmword ptr [rax - 0x48], xmm8
00000001803A8368 movaps    xmmword ptr [rax - 0x58], xmm9
00000001803A836D mov       r14, rdx
00000001803A8370 mov       rsi, rcx
00000001803A8373 cmp       byte ptr [rip + 0x193cb7f], 0 # 0x181ce4ef9
00000001803A837A jne       0x1803a83ef
00000001803A837C lea       rcx, [rip + 0x181eebd] # 0x181bc7240
00000001803A8383 call      0x18024fef0
00000001803A8388 lea       rcx, [rip + 0x181ef09] # 0x181bc7298
00000001803A838F call      0x18024fef0
00000001803A8394 lea       rcx, [rip + 0x181ef55] # 0x181bc72f0
00000001803A839B call      0x18024fef0
00000001803A83A0 lea       rcx, [rip + 0x17fcb71] # 0x181ba4f18
00000001803A83A7 call      0x18024fef0
00000001803A83AC lea       rcx, [rip + 0x180dad5] # 0x181bb5e88
00000001803A83B3 call      0x18024fef0
00000001803A83B8 lea       rcx, [rip + 0x180da19] # 0x181bb5dd8
00000001803A83BF call      0x18024fef0
00000001803A83C4 lea       rcx, [rip + 0x181faf5] # 0x181bc7ec0
00000001803A83CB call      0x18024fef0
00000001803A83D0 lea       rcx, [rip + 0x180f7d1] # 0x181bb7ba8
00000001803A83D7 call      0x18024fef0
00000001803A83DC lea       rcx, [rip + 0x180811d] # 0x181bb0500
00000001803A83E3 call      0x18024fef0
00000001803A83E8 mov       byte ptr [rip + 0x193cb0a], 1 # 0x181ce4ef9
00000001803A83EF xorps     xmm0, xmm0
00000001803A83F2 xor       eax, eax
00000001803A83F4 movups    xmmword ptr [rsp + 0x58], xmm0
00000001803A83F9 mov       qword ptr [rsp + 0x68], rax
00000001803A83FE nop       
00000001803A8400 xor       r15d, r15d
00000001803A8403 mov       edi, r15d
00000001803A8406 test      r14, r14
00000001803A8409 je        0x1803a8704
00000001803A840F mov       rcx, qword ptr [rsi + 0x40]
00000001803A8413 cmp       edi, dword ptr [r14 + 0x28]
00000001803A8417 jl        0x1803a844d
00000001803A8419 test      rcx, rcx
00000001803A841C je        0x1803a8704
00000001803A8422 mov       r8, qword ptr [rip + 0x181fa97] # 0x181bc7ec0
00000001803A8429 mov       edx, edi
00000001803A842B call      0x180825710
00000001803A8430 mov       rbx, rax
00000001803A8433 test      rax, rax
00000001803A8436 je        0x1803a8704
00000001803A843C mov       rcx, qword ptr [rax + 0x10]
00000001803A8440 test      rcx, rcx
00000001803A8443 je        0x1803a8704
00000001803A8449 xor       dl, dl
00000001803A844B jmp       0x1803a847f
00000001803A844D test      rcx, rcx
00000001803A8450 je        0x1803a8704
00000001803A8456 mov       r8, qword ptr [rip + 0x181fa63] # 0x181bc7ec0
00000001803A845D mov       edx, edi
00000001803A845F call      0x180825710
00000001803A8464 mov       rbx, rax
00000001803A8467 test      rax, rax
00000001803A846A je        0x1803a8704
00000001803A8470 mov       rcx, qword ptr [rax + 0x10]
00000001803A8474 test      rcx, rcx
00000001803A8477 je        0x1803a8704
00000001803A847D mov       dl, 1
00000001803A847F xor       r8d, r8d
00000001803A8482 call      0x18131bad0
00000001803A8487 xor       r8d, r8d
00000001803A848A xor       edx, edx
00000001803A848C mov       rcx, rbx
00000001803A848F call      0x1803a9090
00000001803A8494 inc       edi
00000001803A8496 cmp       edi, 3
00000001803A8499 jl        0x1803a840f
00000001803A849F mov       rdx, qword ptr [r14 + 0x68]
00000001803A84A3 test      rdx, rdx
00000001803A84A6 je        0x1803a8704
00000001803A84AC mov       r8, qword ptr [rip + 0x180d925] # 0x181bb5dd8
00000001803A84B3 lea       rcx, [rsp + 0x40]
00000001803A84B8 call      0x1808197f0
00000001803A84BD movups    xmm0, xmmword ptr [rsp + 0x40]
00000001803A84C2 movups    xmmword ptr [rsp + 0x58], xmm0
00000001803A84C7 movsd     xmm1, qword ptr [rsp + 0x50]
00000001803A84CD movsd     qword ptr [rsp + 0x68], xmm1
00000001803A84D3 mov       qword ptr [rsp + 0x40], r15
00000001803A84D8 lea       rbx, [rsp + 0x58]
00000001803A84DD mov       qword ptr [rsp + 0x48], rbx
00000001803A84E2 movss     xmm8, dword ptr [rip + 0x11ffa99] # 0x1815a7f84
00000001803A84EB movss     xmm9, dword ptr [rip + 0x11ff84c] # 0x1815a7d40
00000001803A84F4 xorps     xmm7, xmm7
00000001803A84F7 nop       word ptr [rax + rax]
00000001803A8500 mov       rdx, qword ptr [rip + 0x181ed91] # 0x181bc7298
00000001803A8507 lea       rcx, [rsp + 0x58]
00000001803A850C call      0x180669280
00000001803A8511 test      al, al
00000001803A8513 je        0x1803a8690
00000001803A8519 mov       r14, qword ptr [rsp + 0x68]
00000001803A851E mov       rdi, qword ptr [rsi + 0x38]
00000001803A8522 mov       rcx, qword ptr [rip + 0x1807fd7] # 0x181bb0500
00000001803A8529 cmp       dword ptr [rcx + 0xe0], 0
00000001803A8530 jne       0x1803a8537
00000001803A8532 call      0x1802501e0
00000001803A8537 mov       rdx, qword ptr [rip + 0x180f66a] # 0x181bb7ba8
00000001803A853E mov       rcx, rdi
00000001803A8541 call      0x1804a0690
00000001803A8546 test      rax, rax
00000001803A8549 je        0x1803a872e
00000001803A854F mov       rdx, qword ptr [rip + 0x17fc9c2] # 0x181ba4f18
00000001803A8556 mov       rcx, rax
00000001803A8559 call      0x18046dc80
00000001803A855E mov       rdi, rax
00000001803A8561 mov       rdx, qword ptr [rsi + 0x20]
00000001803A8565 test      rax, rax
00000001803A8568 je        0x1803a8729
00000001803A856E mov       qword ptr [rax + 0x38], rdx
00000001803A8572 lea       rcx, [rax + 0x38]
00000001803A8576 call      0x18024f360
00000001803A857B mov       qword ptr [rdi + 0x40], rsi
00000001803A857F lea       rcx, [rdi + 0x40]
00000001803A8583 mov       rdx, rsi
00000001803A8586 call      0x18024f360
00000001803A858B xor       r8d, r8d
00000001803A858E mov       rdx, r14
00000001803A8591 mov       rcx, rdi
00000001803A8594 call      0x18037e0b0
00000001803A8599 mov       rcx, qword ptr [rsi + 0x28]
00000001803A859D test      rcx, rcx
00000001803A85A0 je        0x1803a8724
00000001803A85A6 mov       r9, qword ptr [rip + 0x180d8db] # 0x181bb5e88
00000001803A85AD inc       dword ptr [rcx + 0x1c]
00000001803A85B0 mov       rdx, qword ptr [rcx + 0x10]
00000001803A85B4 movsxd    r8, dword ptr [rcx + 0x18]
00000001803A85B8 test      rdx, rdx
00000001803A85BB je        0x1803a871f
00000001803A85C1 cmp       r8d, dword ptr [rdx + 0x18]
00000001803A85C5 jb        0x1803a85e0
00000001803A85C7 mov       rax, qword ptr [r9 + 0x20]
00000001803A85CB mov       r8, qword ptr [rax + 0xc0]
00000001803A85D2 mov       r8, qword ptr [r8 + 0x70]
00000001803A85D6 mov       rdx, rdi
00000001803A85D9 call      0x180847ce0
00000001803A85DE jmp       0x1803a8606
00000001803A85E0 lea       eax, [r8 + 1]
00000001803A85E4 mov       dword ptr [rcx + 0x18], eax
00000001803A85E7 cmp       r8d, dword ptr [rdx + 0x18]
00000001803A85EB jae       0x1803a870a
00000001803A85F1 mov       qword ptr [rdx + r8*8 + 0x20], rdi
00000001803A85F6 add       rdx, 0x20
00000001803A85FA lea       rcx, [rdx + r8*8]
00000001803A85FE mov       rdx, rdi
00000001803A8601 call      0x18024f360
00000001803A8606 xor       edx, edx
00000001803A8608 mov       rcx, rdi
00000001803A860B call      0x181304510
00000001803A8610 mov       r14, rax
00000001803A8613 mov       rcx, qword ptr [rsi + 0x30]
00000001803A8617 test      rcx, rcx
00000001803A861A je        0x1803a871a
00000001803A8620 xor       edx, edx
00000001803A8622 call      0x18131c070
00000001803A8627 test      r14, r14
00000001803A862A je        0x1803a8715
00000001803A8630 xor       r9d, r9d
00000001803A8633 xor       r8d, r8d
00000001803A8636 mov       rdx, rax
00000001803A8639 mov       rcx, r14
00000001803A863C call      0x18132e900
00000001803A8641 movd      xmm6, r15d
00000001803A8646 cvtdq2ps  xmm6, xmm6
00000001803A8649 mulss     xmm6, xmm8
00000001803A864E subss     xmm6, xmm9
00000001803A8653 xor       edx, edx
00000001803A8655 mov       rcx, rdi
00000001803A8658 call      0x181304510
00000001803A865D test      rax, rax
00000001803A8660 je        0x1803a8710
00000001803A8666 movaps    xmm0, xmm6
00000001803A8669 unpcklps  xmm0, xmm7
00000001803A866C movsd     qword ptr [rsp + 0x30], xmm0
00000001803A8672 movss     dword ptr [rsp + 0x38], xmm7
00000001803A8678 xor       r8d, r8d
00000001803A867B lea       rdx, [rsp + 0x30]
00000001803A8680 mov       rcx, rax
00000001803A8683 call      0x18132f910
00000001803A8688 inc       r15d
00000001803A868B jmp       0x1803a8500
00000001803A8690 mov       rdx, qword ptr [rip + 0x181eba9] # 0x181bc7240
00000001803A8697 mov       rcx, rbx
00000001803A869A call      0x180302170
00000001803A869F jmp       0x1803a86c4
00000001803A86A1 mov       rdx, qword ptr [rip + 0x181eb98] # 0x181bc7240
00000001803A86A8 mov       rcx, qword ptr [rsp + 0x48]
00000001803A86AD call      0x180302170
00000001803A86B2 mov       rcx, qword ptr [rsp + 0x40]
00000001803A86B7 test      rcx, rcx
00000001803A86BA jne       0x1803a8734
00000001803A86BC mov       rsi, qword ptr [rsp + 0xd0]
00000001803A86C4 mov       rcx, qword ptr [rsi + 0x30]
00000001803A86C8 test      rcx, rcx
00000001803A86CB je        0x1803a8704
00000001803A86CD xor       r8d, r8d
00000001803A86D0 xor       edx, edx
00000001803A86D2 call      0x18131bad0
00000001803A86D7 lea       r11, [rsp + 0xb0]
00000001803A86DF mov       rbx, qword ptr [r11 + 0x28]
00000001803A86E3 mov       rsi, qword ptr [r11 + 0x30]
00000001803A86E7 movaps    xmm6, xmmword ptr [r11 - 0x10]
00000001803A86EC movaps    xmm7, xmmword ptr [r11 - 0x20]
00000001803A86F1 movaps    xmm8, xmmword ptr [r11 - 0x30]
00000001803A86F6 movaps    xmm9, xmmword ptr [r11 - 0x40]
00000001803A86FB mov       rsp, r11
00000001803A86FE pop       r15
00000001803A8700 pop       r14
00000001803A8702 pop       rdi
00000001803A8703 ret       
00000001803A8704 call      0x180250150
00000001803A8709 nop       
00000001803A870A call      0x180250140
00000001803A870F nop       
00000001803A8710 call      0x180250150
00000001803A8715 call      0x180250150
00000001803A871A call      0x180250150
00000001803A871F call      0x180250150
00000001803A8724 call      0x180250150
00000001803A8729 call      0x180250150
00000001803A872E call      0x180250150
00000001803A8733 nop       
00000001803A8734 call      0x180200e60
00000001803A8739 int3      
00000001803A873A int3      
00000001803A873B int3      
00000001803A873C int3      
00000001803A873D int3      
00000001803A873E int3      
00000001803A873F int3      