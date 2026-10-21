00000001803AC470 mov       qword ptr [rsp + 8], rbx
00000001803AC475 mov       qword ptr [rsp + 0x10], rsi
00000001803AC47A mov       qword ptr [rsp + 0x18], rdi
00000001803AC47F push      r14
00000001803AC481 sub       rsp, 0x80
00000001803AC488 movaps    xmmword ptr [rsp + 0x70], xmm6
00000001803AC48D mov       rbx, rdx
00000001803AC490 mov       r14, rcx
00000001803AC493 cmp       byte ptr [rip + 0x1938a82], 0 # 0x181ce4f1c
00000001803AC49A jne       0x1803ac4df
00000001803AC49C lea       rcx, [rip + 0x181cd1d] # 0x181bc91c0
00000001803AC4A3 call      0x18024fef0
00000001803AC4A8 lea       rcx, [rip + 0x181cd69] # 0x181bc9218
00000001803AC4AF call      0x18024fef0
00000001803AC4B4 lea       rcx, [rip + 0x181cdad] # 0x181bc9268
00000001803AC4BB call      0x18024fef0
00000001803AC4C0 lea       rcx, [rip + 0x18110a9] # 0x181bbd570
00000001803AC4C7 call      0x18024fef0
00000001803AC4CC lea       rcx, [rip + 0x1810cd5] # 0x181bbd1a8
00000001803AC4D3 call      0x18024fef0
00000001803AC4D8 mov       byte ptr [rip + 0x1938a3d], 1 # 0x181ce4f1c
00000001803AC4DF test      rbx, rbx
00000001803AC4E2 je        0x1803ac6f9
00000001803AC4E8 mov       r8, qword ptr [rip + 0x1810cb9] # 0x181bbd1a8
00000001803AC4EF mov       rdx, rbx
00000001803AC4F2 lea       rcx, [rsp + 0x40]
00000001803AC4F7 call      0x1808197f0
00000001803AC4FC movups    xmm0, xmmword ptr [rsp + 0x40]
00000001803AC501 movups    xmmword ptr [rsp + 0x58], xmm0
00000001803AC506 movsd     xmm1, qword ptr [rsp + 0x50]
00000001803AC50C movsd     qword ptr [rsp + 0x68], xmm1
00000001803AC512 mov       qword ptr [rsp + 0x40], 0
00000001803AC51B lea       rbx, [rsp + 0x58]
00000001803AC520 mov       qword ptr [rsp + 0x48], rbx
00000001803AC525 movss     xmm6, dword ptr [rip + 0x11fb4e3] # 0x1815a7a10
00000001803AC52D nop       dword ptr [rax]
00000001803AC530 mov       rdx, qword ptr [rip + 0x181cce1] # 0x181bc9218
00000001803AC537 lea       rcx, [rsp + 0x58]
00000001803AC53C call      0x180669280
00000001803AC541 test      al, al
00000001803AC543 je        0x1803ac674
00000001803AC549 mov       rcx, qword ptr [rsp + 0x68]
00000001803AC54E test      rcx, rcx
00000001803AC551 je        0x1803ac6ed
00000001803AC557 xor       r8d, r8d
00000001803AC55A mov       dl, 1
00000001803AC55C call      0x180386d30
00000001803AC561 mov       rdi, rax
00000001803AC564 test      rax, rax
00000001803AC567 je        0x1803ac6e8
00000001803AC56D xor       edx, edx
00000001803AC56F mov       rcx, rax
00000001803AC572 call      0x1813044d0
00000001803AC577 test      rax, rax
00000001803AC57A je        0x1803ac6e3
00000001803AC580 xor       edx, edx
00000001803AC582 mov       rcx, rax
00000001803AC585 call      0x18131c070
00000001803AC58A test      rax, rax
00000001803AC58D je        0x1803ac6de
00000001803AC593 movaps    xmm0, xmm6
00000001803AC596 unpcklps  xmm0, xmm6
00000001803AC599 movsd     qword ptr [rsp + 0x30], xmm0
00000001803AC59F movss     dword ptr [rsp + 0x38], xmm6
00000001803AC5A5 xor       r8d, r8d
00000001803AC5A8 lea       rdx, [rsp + 0x30]
00000001803AC5AD mov       rcx, rax
00000001803AC5B0 call      0x18132fa50
00000001803AC5B5 xor       edx, edx
00000001803AC5B7 mov       rcx, rdi
00000001803AC5BA call      0x1813044d0
00000001803AC5BF test      rax, rax
00000001803AC5C2 je        0x1803ac6d9
00000001803AC5C8 xor       edx, edx
00000001803AC5CA mov       rcx, rax
00000001803AC5CD call      0x18131c070
00000001803AC5D2 mov       rsi, rax
00000001803AC5D5 mov       rcx, qword ptr [r14 + 0x28]
00000001803AC5D9 test      rcx, rcx
00000001803AC5DC je        0x1803ac6d4
00000001803AC5E2 xor       edx, edx
00000001803AC5E4 call      0x18131c070
00000001803AC5E9 test      rsi, rsi
00000001803AC5EC je        0x1803ac6cf
00000001803AC5F2 xor       r9d, r9d
00000001803AC5F5 xor       r8d, r8d
00000001803AC5F8 mov       rdx, rax
00000001803AC5FB mov       rcx, rsi
00000001803AC5FE call      0x18132e900
00000001803AC603 mov       rcx, qword ptr [r14 + 0x20]
00000001803AC607 test      rcx, rcx
00000001803AC60A je        0x1803ac6ca
00000001803AC610 mov       r9, qword ptr [rip + 0x1810f59] # 0x181bbd570
00000001803AC617 inc       dword ptr [rcx + 0x1c]
00000001803AC61A mov       rdx, qword ptr [rcx + 0x10]
00000001803AC61E movsxd    r8, dword ptr [rcx + 0x18]
00000001803AC622 test      rdx, rdx
00000001803AC625 je        0x1803ac6c5
00000001803AC62B cmp       r8d, dword ptr [rdx + 0x18]
00000001803AC62F jb        0x1803ac64d
00000001803AC631 mov       rax, qword ptr [r9 + 0x20]
00000001803AC635 mov       r8, qword ptr [rax + 0xc0]
00000001803AC63C mov       r8, qword ptr [r8 + 0x70]
00000001803AC640 mov       rdx, rdi
00000001803AC643 call      0x180847ce0
00000001803AC648 jmp       0x1803ac530
00000001803AC64D lea       eax, [r8 + 1]
00000001803AC651 mov       dword ptr [rcx + 0x18], eax
00000001803AC654 cmp       r8d, dword ptr [rdx + 0x18]
00000001803AC658 jae       0x1803ac6bf
00000001803AC65A mov       qword ptr [rdx + r8*8 + 0x20], rdi
00000001803AC65F add       rdx, 0x20
00000001803AC663 lea       rcx, [rdx + r8*8]
00000001803AC667 mov       rdx, rdi
00000001803AC66A call      0x18024f360
00000001803AC66F jmp       0x1803ac530
00000001803AC674 mov       rdx, qword ptr [rip + 0x181cb45] # 0x181bc91c0
00000001803AC67B mov       rcx, rbx
00000001803AC67E call      0x180302170
00000001803AC683 jmp       0x1803ac6a0
00000001803AC685 mov       rdx, qword ptr [rip + 0x181cb34] # 0x181bc91c0
00000001803AC68C mov       rcx, qword ptr [rsp + 0x48]
00000001803AC691 call      0x180302170
00000001803AC696 mov       rcx, qword ptr [rsp + 0x40]
00000001803AC69B test      rcx, rcx
00000001803AC69E jne       0x1803ac6f3
00000001803AC6A0 lea       r11, [rsp + 0x80]
00000001803AC6A8 mov       rbx, qword ptr [r11 + 0x10]
00000001803AC6AC mov       rsi, qword ptr [r11 + 0x18]
00000001803AC6B0 mov       rdi, qword ptr [r11 + 0x20]
00000001803AC6B4 movaps    xmm6, xmmword ptr [rsp + 0x70]
00000001803AC6B9 mov       rsp, r11
00000001803AC6BC pop       r14
00000001803AC6BE ret       
00000001803AC6BF call      0x180250140
00000001803AC6C4 nop       
00000001803AC6C5 call      0x180250150
00000001803AC6CA call      0x180250150
00000001803AC6CF call      0x180250150
00000001803AC6D4 call      0x180250150
00000001803AC6D9 call      0x180250150
00000001803AC6DE call      0x180250150
00000001803AC6E3 call      0x180250150
00000001803AC6E8 call      0x180250150
00000001803AC6ED call      0x180250150
00000001803AC6F2 nop       
00000001803AC6F3 call      0x180200e60
00000001803AC6F8 int3      
00000001803AC6F9 call      0x180250150
00000001803AC6FE int3      
00000001803AC6FF int3      