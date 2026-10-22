000000018030B130 push      rbx
000000018030B132 push      r13
000000018030B134 push      r15
000000018030B136 sub       rsp, 0x30
000000018030B13A cmp       byte ptr [rip + 0x19d99ae], 0 # 0x181ce4aef
000000018030B141 mov       rbx, rcx
000000018030B144 jne       0x18030b241
000000018030B14A lea       rcx, [rip + 0x188fbf7] # 0x181b9ad48
000000018030B151 call      0x18024fef0
000000018030B156 lea       rcx, [rip + 0x18b8b63] # 0x181bc3cc0
000000018030B15D call      0x18024fef0
000000018030B162 lea       rcx, [rip + 0x18b2ef7] # 0x181bbe060
000000018030B169 call      0x18024fef0
000000018030B16E lea       rcx, [rip + 0x18b8ba3] # 0x181bc3d18
000000018030B175 call      0x18024fef0
000000018030B17A lea       rcx, [rip + 0x18b2f37] # 0x181bbe0b8
000000018030B181 call      0x18024fef0
000000018030B186 lea       rcx, [rip + 0x18ad00b] # 0x181bb8198
000000018030B18D call      0x18024fef0
000000018030B192 lea       rcx, [rip + 0x18b2f77] # 0x181bbe110
000000018030B199 call      0x18024fef0
000000018030B19E lea       rcx, [rip + 0x18b864b] # 0x181bc37f0
000000018030B1A5 call      0x18024fef0
000000018030B1AA lea       rcx, [rip + 0x18b8bbf] # 0x181bc3d70
000000018030B1B1 call      0x18024fef0
000000018030B1B6 lea       rcx, [rip + 0x18a4c4b] # 0x181bafe08
000000018030B1BD call      0x18024fef0
000000018030B1C2 lea       rcx, [rip + 0x18b8bff] # 0x181bc3dc8
000000018030B1C9 call      0x18024fef0
000000018030B1CE lea       rcx, [rip + 0x18b2f93] # 0x181bbe168
000000018030B1D5 call      0x18024fef0
000000018030B1DA lea       rcx, [rip + 0x18b8667] # 0x181bc3848
000000018030B1E1 call      0x18024fef0
000000018030B1E6 lea       rcx, [rip + 0x18ad003] # 0x181bb81f0
000000018030B1ED call      0x18024fef0
000000018030B1F2 lea       rcx, [rip + 0x18a4c67] # 0x181bafe60
000000018030B1F9 call      0x18024fef0
000000018030B1FE lea       rcx, [rip + 0x18ad083] # 0x181bb8288
000000018030B205 call      0x18024fef0
000000018030B20A lea       rcx, [rip + 0x18ad337] # 0x181bb8548
000000018030B211 call      0x18024fef0
000000018030B216 lea       rcx, [rip + 0x18a52e3] # 0x181bb0500
000000018030B21D call      0x18024fef0
000000018030B222 lea       rcx, [rip + 0x18ab24f] # 0x181bb6478
000000018030B229 call      0x18024fef0
000000018030B22E lea       rcx, [rip + 0x189621b] # 0x181ba1450
000000018030B235 call      0x18024fef0
000000018030B23A mov       byte ptr [rip + 0x19d98ae], 1 # 0x181ce4aef
000000018030B241 mov       rcx, qword ptr [rip + 0x18ab230] # 0x181bb6478
000000018030B248 xor       r15d, r15d
000000018030B24B mov       dword ptr [rsp + 0x60], r15d
000000018030B250 cmp       dword ptr [rcx + 0xe0], r15d
000000018030B257 jne       0x18030b25e
000000018030B259 call      0x1802501e0
000000018030B25E cmp       byte ptr [rip + 0x19d98ce], r15b # 0x181ce4b33
000000018030B265 jne       0x18030b27a
000000018030B267 lea       rcx, [rip + 0x18ab20a] # 0x181bb6478
000000018030B26E call      0x18024fef0
000000018030B273 mov       byte ptr [rip + 0x19d98b9], 1 # 0x181ce4b33
000000018030B27A mov       rcx, qword ptr [rip + 0x18ab1f7] # 0x181bb6478
000000018030B281 cmp       dword ptr [rcx + 0xe0], r15d
000000018030B288 jne       0x18030b28f
000000018030B28A call      0x1802501e0
000000018030B28F mov       rax, qword ptr [rip + 0x18ab1e2] # 0x181bb6478
000000018030B296 mov       rcx, qword ptr [rax + 0xb8]
000000018030B29D mov       r13, qword ptr [rcx + 0x198]
000000018030B2A4 mov       rcx, qword ptr [rip + 0x18a5255] # 0x181bb0500
000000018030B2AB cmp       dword ptr [rcx + 0xe0], r15d
000000018030B2B2 jne       0x18030b2b9
000000018030B2B4 call      0x1802501e0
000000018030B2B9 xor       edx, edx
000000018030B2BB mov       rcx, r13
000000018030B2BE call      0x18131f870
000000018030B2C3 test      al, al
000000018030B2C5 je        0x18030b835
000000018030B2CB mov       qword ptr [rsp + 0x68], rdi
000000018030B2D0 mov       qword ptr [rsp + 0x50], rbp
000000018030B2D5 mov       qword ptr [rsp + 0x58], rsi
000000018030B2DA mov       qword ptr [rsp + 0x28], r12
000000018030B2DF mov       qword ptr [rsp + 0x20], r14
000000018030B2E4 mov       rdi, qword ptr [rbx + 0xa0]
000000018030B2EB test      rdi, rdi
000000018030B2EE je        0x18030b885
000000018030B2F4 mov       edi, dword ptr [rdi + 0x18]
000000018030B2F7 sub       edi, 1
000000018030B2FA js        0x18030b358
000000018030B2FC nop       dword ptr [rax]
000000018030B300 mov       rcx, qword ptr [rbx + 0xa0]
000000018030B307 test      rcx, rcx
000000018030B30A je        0x18030b885
000000018030B310 mov       r8, qword ptr [rip + 0x18b8ab1] # 0x181bc3dc8
000000018030B317 mov       edx, edi
000000018030B319 call      0x180825710
000000018030B31E test      rax, rax
000000018030B321 je        0x18030b885
000000018030B327 xor       edx, edx
000000018030B329 mov       rcx, rax
000000018030B32C call      0x1813044d0
000000018030B331 mov       rcx, qword ptr [rip + 0x18a51c8] # 0x181bb0500
000000018030B338 mov       rsi, rax
000000018030B33B cmp       dword ptr [rcx + 0xe0], r15d
000000018030B342 jne       0x18030b349
000000018030B344 call      0x1802501e0
000000018030B349 xor       edx, edx
000000018030B34B mov       rcx, rsi
000000018030B34E call      0x18131e940
000000018030B353 sub       edi, 1
000000018030B356 jns       0x18030b300
000000018030B358 mov       rcx, qword ptr [rbx + 0xa0]
000000018030B35F test      rcx, rcx
000000018030B362 je        0x18030b885
000000018030B368 mov       r8d, dword ptr [rcx + 0x18]
000000018030B36C inc       dword ptr [rcx + 0x1c]
000000018030B36F mov       dword ptr [rcx + 0x18], r15d
000000018030B373 test      r8d, r8d
000000018030B376 jle       0x18030b386
000000018030B378 mov       rcx, qword ptr [rcx + 0x10]
000000018030B37C xor       r9d, r9d
000000018030B37F xor       edx, edx
000000018030B381 call      0x180cb86f0
000000018030B386 mov       rcx, qword ptr [rip + 0x18ab0eb] # 0x181bb6478
000000018030B38D cmp       dword ptr [rcx + 0xe0], r15d
000000018030B394 jne       0x18030b39b
000000018030B396 call      0x1802501e0
000000018030B39B xor       ecx, ecx
000000018030B39D call      0x18033ba50
000000018030B3A2 mov       r12, rax
000000018030B3A5 mov       ebp, r15d
000000018030B3A8 mov       ecx, r15d
000000018030B3AB test      rax, rax
000000018030B3AE je        0x18030b885
000000018030B3B4 cmp       ecx, dword ptr [r12 + 0x18]
000000018030B3B9 jge       0x18030b4d2
000000018030B3BF mov       rcx, qword ptr [rip + 0x18a513a] # 0x181bb0500
000000018030B3C6 xor       sil, sil
000000018030B3C9 cmp       dword ptr [rcx + 0xe0], r15d
000000018030B3D0 jne       0x18030b3d7
000000018030B3D2 call      0x1802501e0
000000018030B3D7 mov       rdx, qword ptr [rip + 0x18ad16a] # 0x181bb8548
000000018030B3DE mov       rcx, r13
000000018030B3E1 call      0x1804a0690
000000018030B3E6 mov       r14, rax
000000018030B3E9 mov       edi, r15d
000000018030B3EC nop       dword ptr [rax]
000000018030B3F0 mov       rcx, qword ptr [rbx + 0x20]
000000018030B3F4 test      rcx, rcx
000000018030B3F7 je        0x18030b885
000000018030B3FD mov       rdx, qword ptr [rcx + 0x20]
000000018030B401 test      rdx, rdx
000000018030B404 je        0x18030b885
000000018030B40A cmp       edi, dword ptr [rdx + 0x18]
000000018030B40D jge       0x18030b433
000000018030B40F mov       r8, qword ptr [rip + 0x18a4a4a] # 0x181bafe60
000000018030B416 mov       edx, edi
000000018030B418 mov       rcx, qword ptr [rcx + 0x20]
000000018030B41C call      0x180836790
000000018030B421 cmp       eax, ebp
000000018030B423 movzx     esi, sil
000000018030B427 mov       eax, 1
000000018030B42C cmove     esi, eax
000000018030B42F inc       edi
000000018030B431 jmp       0x18030b3f0
000000018030B433 test      r14, r14
000000018030B436 je        0x18030b885
000000018030B43C lea       rcx, [r14 + 0x20]
000000018030B440 mov       qword ptr [r14 + 0x20], rbx
000000018030B444 mov       rdx, rbx
000000018030B447 call      0x18024f360
000000018030B44C mov       r8, qword ptr [rip + 0x18b83f5] # 0x181bc3848
000000018030B453 mov       edx, ebp
000000018030B455 mov       rcx, r12
000000018030B458 call      0x180825710
000000018030B45D xor       r9d, r9d
000000018030B460 movzx     r8d, sil
000000018030B464 mov       rdx, rax
000000018030B467 mov       rcx, r14
000000018030B46A call      0x1803b1940
000000018030B46F mov       rcx, qword ptr [rbx + 0xa0]
000000018030B476 test      rcx, rcx
000000018030B479 je        0x18030b885
000000018030B47F mov       r8, qword ptr [rip + 0x18b883a] # 0x181bc3cc0
000000018030B486 mov       rdx, r14
000000018030B489 call      0x180002e70
000000018030B48E xor       edx, edx
000000018030B490 mov       rcx, r14
000000018030B493 call      0x181304510
000000018030B498 mov       rcx, qword ptr [rbx + 0xa8]
000000018030B49F mov       rdi, rax
000000018030B4A2 test      rcx, rcx
000000018030B4A5 je        0x18030b885
000000018030B4AB xor       edx, edx
000000018030B4AD call      0x18131c070
000000018030B4B2 test      rdi, rdi
000000018030B4B5 je        0x18030b885
000000018030B4BB xor       r8d, r8d
000000018030B4BE mov       rdx, rax
000000018030B4C1 mov       rcx, rdi
000000018030B4C4 call      0x18132e8b0
000000018030B4C9 inc       ebp
000000018030B4CB mov       ecx, ebp
000000018030B4CD jmp       0x18030b3b4
000000018030B4D2 mov       rcx, qword ptr [rbx + 0xb0]
000000018030B4D9 test      rcx, rcx
000000018030B4DC je        0x18030b885
000000018030B4E2 mov       rax, qword ptr [rbx + 0x20]
000000018030B4E6 test      rax, rax
000000018030B4E9 je        0x18030b885
000000018030B4EF mov       rdx, qword ptr [rax + 0x30]
000000018030B4F3 test      rdx, rdx
000000018030B4F6 je        0x18030b885
000000018030B4FC mov       rax, qword ptr [rdx + 0x10]
000000018030B500 test      rax, rax
000000018030B503 je        0x18030b885
000000018030B509 mov       eax, dword ptr [rax + 0x18]
000000018030B50C cmp       dword ptr [rcx + 0x18], eax
000000018030B50F jle       0x18030b5da
000000018030B515 mov       rdi, qword ptr [rbx + 0xb0]
000000018030B51C test      rdi, rdi
000000018030B51F je        0x18030b885
000000018030B525 mov       edi, dword ptr [rdi + 0x18]
000000018030B528 mov       rcx, qword ptr [rbx + 0xb0]
000000018030B52F sub       edi, 1
000000018030B532 mov       r8, qword ptr [rip + 0x18b2c2f] # 0x181bbe168
000000018030B539 je        0x18030b5aa
000000018030B53B mov       edx, edi
000000018030B53D call      0x180825710
000000018030B542 test      rax, rax
000000018030B545 je        0x18030b885
000000018030B54B xor       edx, edx
000000018030B54D mov       rcx, rax
000000018030B550 call      0x1813044d0
000000018030B555 mov       rcx, qword ptr [rip + 0x18a4fa4] # 0x181bb0500
000000018030B55C mov       rsi, rax
000000018030B55F cmp       dword ptr [rcx + 0xe0], r15d
000000018030B566 jne       0x18030b56d
000000018030B568 call      0x1802501e0
000000018030B56D xor       edx, edx
000000018030B56F mov       rcx, rsi
000000018030B572 call      0x18131e940
000000018030B577 mov       rcx, qword ptr [rbx + 0xb0]
000000018030B57E test      rcx, rcx
000000018030B581 je        0x18030b885
000000018030B587 mov       r8, qword ptr [rip + 0x18b2b2a] # 0x181bbe0b8
000000018030B58E mov       edx, edi
000000018030B590 call      0x18084e090
000000018030B595 mov       rcx, qword ptr [rbx + 0xb0]
000000018030B59C test      rcx, rcx
000000018030B59F jne       0x18030b4e2
000000018030B5A5 jmp       0x18030b885
000000018030B5AA xor       edx, edx
000000018030B5AC call      0x180825710
000000018030B5B1 test      rax, rax
000000018030B5B4 je        0x18030b885
000000018030B5BA xor       edx, edx
000000018030B5BC mov       rcx, rax
000000018030B5BF call      0x1813044d0
000000018030B5C4 test      rax, rax
000000018030B5C7 je        0x18030b885
000000018030B5CD xor       r8d, r8d
000000018030B5D0 xor       edx, edx
000000018030B5D2 mov       rcx, rax
000000018030B5D5 call      0x18131bad0
000000018030B5DA mov       rcx, qword ptr [rbx + 0x20]
000000018030B5DE test      rcx, rcx
000000018030B5E1 je        0x18030b885
000000018030B5E7 mov       eax, dword ptr [rsp + 0x60]
000000018030B5EB nop       dword ptr [rax + rax]
000000018030B5F0 mov       rcx, qword ptr [rcx + 0x30]
000000018030B5F4 test      rcx, rcx
000000018030B5F7 je        0x18030b885
000000018030B5FD mov       rcx, qword ptr [rcx + 0x10]
000000018030B601 test      rcx, rcx
000000018030B604 je        0x18030b885
000000018030B60A cmp       r15d, dword ptr [rcx + 0x18]
000000018030B60E jge       0x18030b862
000000018030B614 mov       rcx, qword ptr [rbx + 0xb0]
000000018030B61B test      rcx, rcx
000000018030B61E je        0x18030b885
000000018030B624 cmp       eax, dword ptr [rcx + 0x18]
000000018030B627 jl        0x18030b70c
000000018030B62D mov       r8, qword ptr [rip + 0x18b2b34] # 0x181bbe168
000000018030B634 xor       edx, edx
000000018030B636 mov       rsi, rcx
000000018030B639 call      0x180825710
000000018030B63E mov       rcx, qword ptr [rip + 0x18a4ebb] # 0x181bb0500
000000018030B645 mov       rdi, rax
000000018030B648 cmp       dword ptr [rcx + 0xe0], 0
000000018030B64F jne       0x18030b656
000000018030B651 call      0x1802501e0
000000018030B656 mov       rdx, qword ptr [rip + 0x18acc2b] # 0x181bb8288
000000018030B65D mov       rcx, rdi
000000018030B660 call      0x1804a0690
000000018030B665 mov       r8, qword ptr [rip + 0x18b29f4] # 0x181bbe060
000000018030B66C mov       rdx, rax
000000018030B66F mov       rcx, rsi
000000018030B672 call      0x180002e70
000000018030B677 mov       rcx, qword ptr [rbx + 0xb0]
000000018030B67E test      rcx, rcx
000000018030B681 je        0x18030b885
000000018030B687 mov       r8, qword ptr [rip + 0x18b2ada] # 0x181bbe168
000000018030B68E mov       edx, dword ptr [rsp + 0x60]
000000018030B692 call      0x180825710
000000018030B697 test      rax, rax
000000018030B69A je        0x18030b885
000000018030B6A0 xor       edx, edx
000000018030B6A2 mov       rcx, rax
000000018030B6A5 call      0x181304510
000000018030B6AA mov       rcx, qword ptr [rbx + 0xb0]
000000018030B6B1 mov       rdi, rax
000000018030B6B4 test      rcx, rcx
000000018030B6B7 je        0x18030b885
000000018030B6BD mov       r8, qword ptr [rip + 0x18b2aa4] # 0x181bbe168
000000018030B6C4 xor       edx, edx
000000018030B6C6 call      0x180825710
000000018030B6CB test      rax, rax
000000018030B6CE je        0x18030b885
000000018030B6D4 xor       edx, edx
000000018030B6D6 mov       rcx, rax
000000018030B6D9 call      0x181304510
000000018030B6DE test      rax, rax
000000018030B6E1 je        0x18030b885
000000018030B6E7 xor       edx, edx
000000018030B6E9 mov       rcx, rax
000000018030B6EC call      0x18132dc90
000000018030B6F1 test      rdi, rdi
000000018030B6F4 je        0x18030b885
000000018030B6FA xor       r8d, r8d
000000018030B6FD mov       rdx, rax
000000018030B700 mov       rcx, rdi
000000018030B703 call      0x18132e8b0
000000018030B708 mov       eax, dword ptr [rsp + 0x60]
000000018030B70C mov       rcx, qword ptr [rbx + 0xb0]
000000018030B713 test      rcx, rcx
000000018030B716 je        0x18030b885
000000018030B71C mov       r8, qword ptr [rip + 0x18b2a45] # 0x181bbe168
000000018030B723 mov       edx, eax
000000018030B725 call      0x180825710
000000018030B72A test      rax, rax
000000018030B72D je        0x18030b885
000000018030B733 xor       edx, edx
000000018030B735 mov       rcx, rax
000000018030B738 call      0x1813044d0
000000018030B73D test      rax, rax
000000018030B740 je        0x18030b885
000000018030B746 xor       r8d, r8d
000000018030B749 mov       dl, 1
000000018030B74B mov       rcx, rax
000000018030B74E call      0x18131bad0
000000018030B753 mov       rcx, qword ptr [rbx + 0xb0]
000000018030B75A test      rcx, rcx
000000018030B75D je        0x18030b885
000000018030B763 mov       r8, qword ptr [rip + 0x18b29fe] # 0x181bbe168
000000018030B76A mov       edx, dword ptr [rsp + 0x60]
000000018030B76E call      0x180825710
000000018030B773 mov       rcx, qword ptr [rbx + 0x20]
000000018030B777 mov       rdi, rax
000000018030B77A test      rcx, rcx
000000018030B77D je        0x18030b885
000000018030B783 mov       rcx, qword ptr [rcx + 0x30]
000000018030B787 test      rcx, rcx
000000018030B78A je        0x18030b885
000000018030B790 mov       rcx, qword ptr [rcx + 0x10]
000000018030B794 test      rcx, rcx
000000018030B797 je        0x18030b885
000000018030B79D mov       r8, qword ptr [rip + 0x18aca4c] # 0x181bb81f0
000000018030B7A4 mov       edx, dword ptr [rsp + 0x60]
000000018030B7A8 call      0x180825710
000000018030B7AD test      rax, rax
000000018030B7B0 je        0x18030b885
000000018030B7B6 lea       rcx, [rax + 0x20]
000000018030B7BA xor       edx, edx
000000018030B7BC call      0x180ca3820
000000018030B7C1 test      rdi, rdi
000000018030B7C4 je        0x18030b885
000000018030B7CA xor       r8d, r8d
000000018030B7CD mov       rdx, rax
000000018030B7D0 mov       rcx, rdi
000000018030B7D3 call      0x18119bae0
000000018030B7D8 mov       rcx, qword ptr [rbx + 0xb0]
000000018030B7DF test      rcx, rcx
000000018030B7E2 je        0x18030b885
000000018030B7E8 mov       r8, qword ptr [rip + 0x18b2979] # 0x181bbe168
000000018030B7EF mov       edx, dword ptr [rsp + 0x60]
000000018030B7F3 call      0x180825710
000000018030B7F8 xor       edx, edx
000000018030B7FA lea       rcx, [rsp + 0x60]
000000018030B7FF mov       rdi, rax
000000018030B802 call      0x180ca3820
000000018030B807 test      rdi, rdi
000000018030B80A je        0x18030b885
000000018030B80C xor       r8d, r8d
000000018030B80F mov       rdx, rax
000000018030B812 mov       rcx, rdi
000000018030B815 call      0x18131fa50
000000018030B81A mov       eax, dword ptr [rsp + 0x60]
000000018030B81E mov       rcx, qword ptr [rbx + 0x20]
000000018030B822 inc       eax
000000018030B824 mov       dword ptr [rsp + 0x60], eax
000000018030B828 mov       r15d, eax
000000018030B82B test      rcx, rcx
000000018030B82E je        0x18030b885
000000018030B830 jmp       0x18030b5f0
000000018030B835 mov       rcx, qword ptr [rip + 0x188f50c] # 0x181b9ad48
000000018030B83C cmp       dword ptr [rcx + 0xe0], r15d
000000018030B843 jne       0x18030b84a
000000018030B845 call      0x1802501e0
000000018030B84A mov       rcx, qword ptr [rip + 0x1895bff] # 0x181ba1450
000000018030B851 xor       edx, edx
000000018030B853 call      0x1812e68f0
000000018030B858 add       rsp, 0x30
000000018030B85C pop       r15
000000018030B85E pop       r13
000000018030B860 pop       rbx
000000018030B861 ret       
000000018030B862 mov       r12, qword ptr [rsp + 0x28]
000000018030B867 mov       rbp, qword ptr [rsp + 0x50]
000000018030B86C mov       rsi, qword ptr [rsp + 0x58]
000000018030B871 mov       r14, qword ptr [rsp + 0x20]
000000018030B876 mov       rdi, qword ptr [rsp + 0x68]
000000018030B87B add       rsp, 0x30
000000018030B87F pop       r15
000000018030B881 pop       r13
000000018030B883 pop       rbx
000000018030B884 ret       
000000018030B885 call      0x180250150
000000018030B88A int3      
000000018030B88B int3      
000000018030B88C int3      
000000018030B88D int3      
000000018030B88E int3      
000000018030B88F int3      