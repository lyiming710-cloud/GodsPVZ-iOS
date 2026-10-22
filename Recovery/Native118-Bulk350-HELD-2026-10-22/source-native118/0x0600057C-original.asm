000000018038C3F0 mov       qword ptr [rsp + 0x10], rbx
000000018038C3F5 mov       qword ptr [rsp + 0x18], rsi
000000018038C3FA mov       qword ptr [rsp + 0x20], rdi
000000018038C3FF mov       qword ptr [rsp + 8], rcx
000000018038C404 push      r13
000000018038C406 push      r14
000000018038C408 push      r15
000000018038C40A sub       rsp, 0x60
000000018038C40E mov       r15, rcx
000000018038C411 cmp       byte ptr [rip + 0x1958a2c], 0 # 0x181ce4e44
000000018038C418 jne       0x18038c499
000000018038C41A lea       rcx, [rip + 0x183cd9f] # 0x181bc91c0
000000018038C421 call      0x18024fef0
000000018038C426 lea       rcx, [rip + 0x183cdeb] # 0x181bc9218
000000018038C42D call      0x18024fef0
000000018038C432 lea       rcx, [rip + 0x183ce2f] # 0x181bc9268
000000018038C439 call      0x18024fef0
000000018038C43E lea       rcx, [rip + 0x1815bd3] # 0x181ba2018
000000018038C445 call      0x18024fef0
000000018038C44A lea       rcx, [rip + 0x183111f] # 0x181bbd570
000000018038C451 call      0x18024fef0
000000018038C456 lea       rcx, [rip + 0x183116b] # 0x181bbd5c8
000000018038C45D call      0x18024fef0
000000018038C462 lea       rcx, [rip + 0x1830d3f] # 0x181bbd1a8
000000018038C469 call      0x18024fef0
000000018038C46E lea       rcx, [rip + 0x1831203] # 0x181bbd678
000000018038C475 call      0x18024fef0
000000018038C47A lea       rcx, [rip + 0x183124f] # 0x181bbd6d0
000000018038C481 call      0x18024fef0
000000018038C486 lea       rcx, [rip + 0x1824073] # 0x181bb0500
000000018038C48D call      0x18024fef0
000000018038C492 mov       byte ptr [rip + 0x19589ab], 1 # 0x181ce4e44
000000018038C499 xorps     xmm0, xmm0
000000018038C49C xor       eax, eax
000000018038C49E movups    xmmword ptr [rsp + 0x40], xmm0
000000018038C4A3 mov       qword ptr [rsp + 0x50], rax
000000018038C4A8 mov       rsi, qword ptr [r15 + 0x60]
000000018038C4AC test      rsi, rsi
000000018038C4AF je        0x18038c9ab
000000018038C4B5 mov       esi, dword ptr [rsi + 0x44]
000000018038C4B8 xor       ebx, ebx
000000018038C4BA xor       edx, edx
000000018038C4BC mov       rax, qword ptr [r15 + 0x168]
000000018038C4C3 test      rax, rax
000000018038C4C6 je        0x18038c9ab
000000018038C4CC nop       dword ptr [rax]
000000018038C4D0 mov       rcx, qword ptr [r15 + 0x168]
000000018038C4D7 cmp       edx, dword ptr [rax + 0x18]
000000018038C4DA jge       0x18038c53e
000000018038C4DC test      rcx, rcx
000000018038C4DF je        0x18038c9ab
000000018038C4E5 mov       r8, qword ptr [rip + 0x18311e4] # 0x181bbd6d0
000000018038C4EC mov       edx, ebx
000000018038C4EE call      0x180825710
000000018038C4F3 test      rax, rax
000000018038C4F6 je        0x18038c9ab
000000018038C4FC xor       edx, edx
000000018038C4FE mov       rcx, rax
000000018038C501 call      0x1813044d0
000000018038C506 mov       rdi, rax
000000018038C509 mov       rcx, qword ptr [rip + 0x1823ff0] # 0x181bb0500
000000018038C510 cmp       dword ptr [rcx + 0xe0], 0
000000018038C517 jne       0x18038c51e
000000018038C519 call      0x1802501e0
000000018038C51E xor       edx, edx
000000018038C520 mov       rcx, rdi
000000018038C523 call      0x18131e940
000000018038C528 inc       ebx
000000018038C52A mov       edx, ebx
000000018038C52C mov       rax, qword ptr [r15 + 0x168]
000000018038C533 test      rax, rax
000000018038C536 je        0x18038c9ab
000000018038C53C jmp       0x18038c4d0
000000018038C53E test      rcx, rcx
000000018038C541 je        0x18038c9ab
000000018038C547 inc       dword ptr [rcx + 0x1c]
000000018038C54A mov       r8d, dword ptr [rcx + 0x18]
000000018038C54E mov       dword ptr [rcx + 0x18], 0
000000018038C555 test      r8d, r8d
000000018038C558 jle       0x18038c568
000000018038C55A xor       r9d, r9d
000000018038C55D xor       edx, edx
000000018038C55F mov       rcx, qword ptr [rcx + 0x10]
000000018038C563 call      0x180cb86f0
000000018038C568 mov       rax, qword ptr [r15 + 0x60]
000000018038C56C test      rax, rax
000000018038C56F je        0x18038c9ab
000000018038C575 movsxd    r13, dword ptr [rax + 0x5c]
000000018038C579 lea       eax, [r13 - 1]
000000018038C57D lea       rdi, [rip - 0x38c584] # 0x180000000
000000018038C584 cmp       eax, 7
000000018038C587 ja        0x18038c5ac
000000018038C589 cdqe      
000000018038C58B mov       eax, dword ptr [rdi + rax*4 + 0x38c9fc]
000000018038C592 add       rax, rdi
000000018038C595 jmp       rax
000000018038C597 mov       ebx, 0x1c
000000018038C59C jmp       0x18038c5ae
000000018038C59E mov       ebx, 0x1d
000000018038C5A3 jmp       0x18038c5ae
000000018038C5A5 mov       ebx, 0x1e
000000018038C5AA jmp       0x18038c5ae
000000018038C5AC xor       ebx, ebx
000000018038C5AE mov       rax, qword ptr [rip + 0x1815a63] # 0x181ba2018
000000018038C5B5 cmp       dword ptr [rax + 0xe0], 0
000000018038C5BC jne       0x18038c5cd
000000018038C5BE mov       rcx, rax
000000018038C5C1 call      0x1802501e0
000000018038C5C6 mov       rax, qword ptr [rip + 0x1815a4b] # 0x181ba2018
000000018038C5CD mov       rax, qword ptr [rax + 0xb8]
000000018038C5D4 mov       rax, qword ptr [rax]
000000018038C5D7 test      rax, rax
000000018038C5DA je        0x18038c9ab
000000018038C5E0 mov       rax, qword ptr [rax + 0x28]
000000018038C5E4 test      rax, rax
000000018038C5E7 je        0x18038c9ab
000000018038C5ED mov       rcx, qword ptr [rax + 0x10]
000000018038C5F1 test      rcx, rcx
000000018038C5F4 je        0x18038c9ab
000000018038C5FA xor       r8d, r8d
000000018038C5FD mov       edx, ebx
000000018038C5FF call      0x180380100
000000018038C604 test      rax, rax
000000018038C607 je        0x18038c9ab
000000018038C60D xor       r8d, r8d
000000018038C610 mov       dl, 1
000000018038C612 mov       rcx, rax
000000018038C615 call      0x180386d30
000000018038C61A mov       rbx, rax
000000018038C61D mov       rcx, qword ptr [r15 + 0x168]
000000018038C624 test      rcx, rcx
000000018038C627 je        0x18038c9ab
000000018038C62D mov       r9, qword ptr [rip + 0x1830f3c] # 0x181bbd570
000000018038C634 inc       dword ptr [rcx + 0x1c]
000000018038C637 mov       rdx, qword ptr [rcx + 0x10]
000000018038C63B movsxd    r8, dword ptr [rcx + 0x18]
000000018038C63F test      rdx, rdx
000000018038C642 je        0x18038c9ab
000000018038C648 cmp       r8d, dword ptr [rdx + 0x18]
000000018038C64C jb        0x18038c667
000000018038C64E mov       rax, qword ptr [r9 + 0x20]
000000018038C652 mov       r8, qword ptr [rax + 0xc0]
000000018038C659 mov       r8, qword ptr [r8 + 0x70]
000000018038C65D mov       rdx, rbx
000000018038C660 call      0x180847ce0
000000018038C665 jmp       0x18038c68d
000000018038C667 lea       eax, [r8 + 1]
000000018038C66B mov       dword ptr [rcx + 0x18], eax
000000018038C66E cmp       r8d, dword ptr [rdx + 0x18]
000000018038C672 jae       0x18038c9b1
000000018038C678 mov       qword ptr [rdx + r8*8 + 0x20], rbx
000000018038C67D add       rdx, 0x20
000000018038C681 lea       rcx, [rdx + r8*8]
000000018038C685 mov       rdx, rbx
000000018038C688 call      0x18024f360
000000018038C68D test      rbx, rbx
000000018038C690 je        0x18038c9ab
000000018038C696 xor       edx, edx
000000018038C698 mov       rcx, rbx
000000018038C69B call      0x1813044d0
000000018038C6A0 test      rax, rax
000000018038C6A3 je        0x18038c9ab
000000018038C6A9 xor       edx, edx
000000018038C6AB mov       rcx, rax
000000018038C6AE call      0x18131c070
000000018038C6B3 mov       r14, rax
000000018038C6B6 mov       rcx, qword ptr [r15 + 0x160]
000000018038C6BD test      rcx, rcx
000000018038C6C0 je        0x18038c9ab
000000018038C6C6 xor       edx, edx
000000018038C6C8 call      0x18131c070
000000018038C6CD test      r14, r14
000000018038C6D0 je        0x18038c9ab
000000018038C6D6 xor       r9d, r9d
000000018038C6D9 xor       r8d, r8d
000000018038C6DC mov       rdx, rax
000000018038C6DF mov       rcx, r14
000000018038C6E2 call      0x18132e900
000000018038C6E7 cmp       r13d, 8
000000018038C6EB ja        0x18038c73d
000000018038C6ED mov       ecx, dword ptr [rdi + r13*4 + 0x38ca1c]
000000018038C6F5 add       rcx, rdi
000000018038C6F8 jmp       rcx
000000018038C6FA mov       eax, esi
000000018038C6FC cdq       
000000018038C6FD sub       eax, edx
000000018038C6FF sar       eax, 1
000000018038C701 inc       eax
000000018038C703 jmp       0x18038c73f
000000018038C705 lea       eax, [rsi + 1]
000000018038C708 jmp       0x18038c73f
000000018038C70A lea       eax, [rsi + rsi*2]
000000018038C70D cdq       
000000018038C70E sub       eax, edx
000000018038C710 sar       eax, 1
000000018038C712 add       eax, 2
000000018038C715 jmp       0x18038c73f
000000018038C717 lea       eax, [rsi*2 + 3]
000000018038C71E jmp       0x18038c73f
000000018038C720 lea       eax, [rsi*2 + 1]
000000018038C727 jmp       0x18038c73f
000000018038C729 lea       eax, [rsi + 1]
000000018038C72C lea       eax, [rsi + rax*2]
000000018038C72F jmp       0x18038c73f
000000018038C731 lea       eax, [rsi*4]
000000018038C738 add       eax, 3
000000018038C73B jmp       0x18038c73f
000000018038C73D xor       eax, eax
000000018038C73F mov       dword ptr [rbx + 0x28], eax
000000018038C742 mov       rax, qword ptr [rip + 0x18158cf] # 0x181ba2018
000000018038C749 cmp       dword ptr [rax + 0xe0], 0
000000018038C750 jne       0x18038c761
000000018038C752 mov       rcx, rax
000000018038C755 call      0x1802501e0
000000018038C75A mov       rax, qword ptr [rip + 0x18158b7] # 0x181ba2018
000000018038C761 mov       rax, qword ptr [rax + 0xb8]
000000018038C768 mov       rax, qword ptr [rax]
000000018038C76B test      rax, rax
000000018038C76E je        0x18038c9ab
000000018038C774 mov       rax, qword ptr [rax + 0x28]
000000018038C778 test      rax, rax
000000018038C77B je        0x18038c9ab
000000018038C781 mov       rcx, qword ptr [rax + 0x20]
000000018038C785 test      rcx, rcx
000000018038C788 je        0x18038c9ab
000000018038C78E xor       r9d, r9d
000000018038C791 mov       r8d, r13d
000000018038C794 mov       edx, dword ptr [r15 + 0x48]
000000018038C798 call      0x18032e5e0
000000018038C79D test      rax, rax
000000018038C7A0 je        0x18038c9ab
000000018038C7A6 mov       r8, qword ptr [rip + 0x18309fb] # 0x181bbd1a8
000000018038C7AD mov       rdx, rax
000000018038C7B0 lea       rcx, [rsp + 0x28]
000000018038C7B5 call      0x1808197f0
000000018038C7BA movups    xmm0, xmmword ptr [rsp + 0x28]
000000018038C7BF movups    xmmword ptr [rsp + 0x40], xmm0
000000018038C7C4 movsd     xmm1, qword ptr [rsp + 0x38]
000000018038C7CA movsd     qword ptr [rsp + 0x50], xmm1
000000018038C7D0 mov       qword ptr [rsp + 0x28], 0
000000018038C7D9 lea       rbx, [rsp + 0x40]
000000018038C7DE mov       qword ptr [rsp + 0x30], rbx
000000018038C7E3 mov       rdx, qword ptr [rip + 0x183ca2e] # 0x181bc9218
000000018038C7EA lea       rcx, [rsp + 0x40]
000000018038C7EF call      0x180669280
000000018038C7F4 test      al, al
000000018038C7F6 je        0x18038c951
000000018038C7FC mov       rsi, qword ptr [rsp + 0x50]
000000018038C801 mov       rax, qword ptr [rip + 0x1815810] # 0x181ba2018
000000018038C808 cmp       dword ptr [rax + 0xe0], 0
000000018038C80F jne       0x18038c820
000000018038C811 mov       rcx, rax
000000018038C814 call      0x1802501e0
000000018038C819 mov       rax, qword ptr [rip + 0x18157f8] # 0x181ba2018
000000018038C820 mov       rax, qword ptr [rax + 0xb8]
000000018038C827 mov       rax, qword ptr [rax]
000000018038C82A test      rax, rax
000000018038C82D je        0x18038c9ef
000000018038C833 mov       rax, qword ptr [rax + 0x28]
000000018038C837 test      rax, rax
000000018038C83A je        0x18038c9ea
000000018038C840 mov       rcx, qword ptr [rax + 0x10]
000000018038C844 test      rsi, rsi
000000018038C847 je        0x18038c9e5
000000018038C84D test      rcx, rcx
000000018038C850 je        0x18038c9e0
000000018038C856 xor       r8d, r8d
000000018038C859 mov       edx, dword ptr [rsi + 0x18]
000000018038C85C call      0x180380100
000000018038C861 test      rax, rax
000000018038C864 je        0x18038c9db
000000018038C86A xor       r8d, r8d
000000018038C86D mov       dl, 1
000000018038C86F mov       rcx, rax
000000018038C872 call      0x180386d30
000000018038C877 mov       rdi, rax
000000018038C87A mov       r9, qword ptr [r15 + 0x168]
000000018038C881 test      r9, r9
000000018038C884 je        0x18038c9d6
000000018038C88A mov       rax, qword ptr [rip + 0x1830cdf] # 0x181bbd570
000000018038C891 inc       dword ptr [r9 + 0x1c]
000000018038C895 mov       rcx, qword ptr [r9 + 0x10]
000000018038C899 movsxd    rdx, dword ptr [r9 + 0x18]
000000018038C89D test      rcx, rcx
000000018038C8A0 je        0x18038c9d1
000000018038C8A6 cmp       edx, dword ptr [rcx + 0x18]
000000018038C8A9 jb        0x18038c8c7
000000018038C8AB mov       rax, qword ptr [rax + 0x20]
000000018038C8AF mov       r8, qword ptr [rax + 0xc0]
000000018038C8B6 mov       r8, qword ptr [r8 + 0x70]
000000018038C8BA mov       rdx, rdi
000000018038C8BD mov       rcx, r9
000000018038C8C0 call      0x180847ce0
000000018038C8C5 jmp       0x18038c8ec
000000018038C8C7 lea       eax, [rdx + 1]
000000018038C8CA mov       dword ptr [r9 + 0x18], eax
000000018038C8CE cmp       edx, dword ptr [rcx + 0x18]
000000018038C8D1 jae       0x18038c9b7
000000018038C8D7 mov       qword ptr [rcx + rdx*8 + 0x20], rdi
000000018038C8DC lea       rcx, [rcx + rdx*8]
000000018038C8E0 add       rcx, 0x20
000000018038C8E4 mov       rdx, rdi
000000018038C8E7 call      0x18024f360
000000018038C8EC test      rdi, rdi
000000018038C8EF je        0x18038c9cc
000000018038C8F5 xor       edx, edx
000000018038C8F7 mov       rcx, rdi
000000018038C8FA call      0x1813044d0
000000018038C8FF test      rax, rax
000000018038C902 je        0x18038c9c7
000000018038C908 xor       edx, edx
000000018038C90A mov       rcx, rax
000000018038C90D call      0x18131c070
000000018038C912 mov       r14, rax
000000018038C915 mov       rcx, qword ptr [r15 + 0x160]
000000018038C91C test      rcx, rcx
000000018038C91F je        0x18038c9c2
000000018038C925 xor       edx, edx
000000018038C927 call      0x18131c070
000000018038C92C test      r14, r14
000000018038C92F je        0x18038c9bd
000000018038C935 xor       r9d, r9d
000000018038C938 xor       r8d, r8d
000000018038C93B mov       rdx, rax
000000018038C93E mov       rcx, r14
000000018038C941 call      0x18132e900
000000018038C946 mov       eax, dword ptr [rsi + 0x20]
000000018038C949 mov       dword ptr [rdi + 0x28], eax
000000018038C94C jmp       0x18038c7e3
000000018038C951 mov       rdx, qword ptr [rip + 0x183c868] # 0x181bc91c0
000000018038C958 mov       rcx, rbx
000000018038C95B call      0x180302170
000000018038C960 jmp       0x18038c985
000000018038C962 mov       rdx, qword ptr [rip + 0x183c857] # 0x181bc91c0
000000018038C969 mov       rcx, qword ptr [rsp + 0x30]
000000018038C96E call      0x180302170
000000018038C973 mov       rcx, qword ptr [rsp + 0x28]
000000018038C978 test      rcx, rcx
000000018038C97B jne       0x18038c9f5
000000018038C97D mov       r15, qword ptr [rsp + 0x80]
000000018038C985 mov       dword ptr [r15 + 0x188], 0
000000018038C990 lea       r11, [rsp + 0x60]
000000018038C995 mov       rbx, qword ptr [r11 + 0x28]
000000018038C999 mov       rsi, qword ptr [r11 + 0x30]
000000018038C99D mov       rdi, qword ptr [r11 + 0x38]
000000018038C9A1 mov       rsp, r11
000000018038C9A4 pop       r15
000000018038C9A6 pop       r14
000000018038C9A8 pop       r13
000000018038C9AA ret       
000000018038C9AB call      0x180250150
000000018038C9B0 int3      
000000018038C9B1 call      0x180250140
000000018038C9B6 nop       
000000018038C9B7 call      0x180250140
000000018038C9BC nop       
000000018038C9BD call      0x180250150
000000018038C9C2 call      0x180250150
000000018038C9C7 call      0x180250150
000000018038C9CC call      0x180250150
000000018038C9D1 call      0x180250150
000000018038C9D6 call      0x180250150
000000018038C9DB call      0x180250150
000000018038C9E0 call      0x180250150
000000018038C9E5 call      0x180250150
000000018038C9EA call      0x180250150
000000018038C9EF call      0x180250150
000000018038C9F4 nop       
000000018038C9F5 call      0x180200e60
000000018038C9FA int3      
000000018038C9FB nop       
000000018038C9FC xchg      edi, eax