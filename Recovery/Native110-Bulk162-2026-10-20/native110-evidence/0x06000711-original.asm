00000001803B1940 mov       qword ptr [rsp + 0x10], rbx
00000001803B1945 mov       qword ptr [rsp + 0x18], rbp
00000001803B194A push      rsi
00000001803B194B sub       rsp, 0x30
00000001803B194F cmp       byte ptr [rip + 0x19335ea], 0 # 0x181ce4f40
00000001803B1956 movzx     ebp, r8b
00000001803B195A mov       rsi, rdx
00000001803B195D mov       rbx, rcx
00000001803B1960 jne       0x1803b19a5
00000001803B1962 lea       rcx, [rip + 0x1812aaf] # 0x181bc4418
00000001803B1969 call      0x18024fef0
00000001803B196E lea       rcx, [rip + 0x1806b7b] # 0x181bb84f0
00000001803B1975 call      0x18024fef0
00000001803B197A lea       rcx, [rip + 0x17feb7f] # 0x181bb0500
00000001803B1981 call      0x18024fef0
00000001803B1986 lea       rcx, [rip + 0x1804aeb] # 0x181bb6478
00000001803B198D call      0x18024fef0
00000001803B1992 lea       rcx, [rip + 0x18160bf] # 0x181bc7a58
00000001803B1999 call      0x18024fef0
00000001803B199E mov       byte ptr [rip + 0x193359b], 1 # 0x181ce4f40
00000001803B19A5 mov       rcx, qword ptr [rip + 0x17feb54] # 0x181bb0500
00000001803B19AC cmp       dword ptr [rcx + 0xe0], 0
00000001803B19B3 jne       0x1803b19ba
00000001803B19B5 call      0x1802501e0
00000001803B19BA xor       r8d, r8d
00000001803B19BD mov       qword ptr [rsp + 0x40], rdi
00000001803B19C2 xor       edx, edx
00000001803B19C4 mov       rcx, rsi
00000001803B19C7 call      0x18131f760
00000001803B19CC test      al, al
00000001803B19CE jne       0x1803b1be2
00000001803B19D4 mov       rcx, qword ptr [rip + 0x17feb25] # 0x181bb0500
00000001803B19DB cmp       dword ptr [rcx + 0xe0], 0
00000001803B19E2 jne       0x1803b19e9
00000001803B19E4 call      0x1802501e0
00000001803B19E9 mov       rdx, qword ptr [rip + 0x1806b00] # 0x181bb84f0
00000001803B19F0 mov       rcx, rsi
00000001803B19F3 call      0x1804a0690
00000001803B19F8 lea       rcx, [rbx + 0x28]
00000001803B19FC mov       qword ptr [rbx + 0x28], rax
00000001803B1A00 mov       rdx, rax
00000001803B1A03 call      0x18024f360
00000001803B1A08 mov       rax, qword ptr [rbx + 0x28]
00000001803B1A0C test      rax, rax
00000001803B1A0F je        0x1803b1c43
00000001803B1A15 mov       byte ptr [rax + 0xbd], 0
00000001803B1A1C mov       rcx, qword ptr [rbx + 0x28]
00000001803B1A20 test      rcx, rcx
00000001803B1A23 je        0x1803b1c43
00000001803B1A29 mov       rcx, qword ptr [rcx + 0x160]
00000001803B1A30 xor       edx, edx
00000001803B1A32 call      0x18131f870
00000001803B1A37 test      al, al
00000001803B1A39 jne       0x1803b1a6d
00000001803B1A3B mov       rdi, qword ptr [rbx + 0x28]
00000001803B1A3F mov       rdx, qword ptr [rip + 0x18129d2] # 0x181bc4418
00000001803B1A46 mov       rcx, rdi
00000001803B1A49 call      0x18042da10
00000001803B1A4E test      rdi, rdi
00000001803B1A51 je        0x1803b1c43
00000001803B1A57 lea       rcx, [rdi + 0x160]
00000001803B1A5E mov       qword ptr [rdi + 0x160], rax
00000001803B1A65 mov       rdx, rax
00000001803B1A68 call      0x18024f360
00000001803B1A6D mov       rax, qword ptr [rbx + 0x28]
00000001803B1A71 test      rax, rax
00000001803B1A74 je        0x1803b1c43
00000001803B1A7A mov       rcx, qword ptr [rax + 0x160]
00000001803B1A81 test      rcx, rcx
00000001803B1A84 je        0x1803b1c43
00000001803B1A8A mov       rdx, qword ptr [rip + 0x1815fc7] # 0x181bc7a58
00000001803B1A91 xor       r8d, r8d
00000001803B1A94 call      0x181348060
00000001803B1A99 mov       rcx, qword ptr [rbx + 0x28]
00000001803B1A9D test      rcx, rcx
00000001803B1AA0 je        0x1803b1c43
00000001803B1AA6 mov       rcx, qword ptr [rcx + 0x160]
00000001803B1AAD test      rcx, rcx
00000001803B1AB0 je        0x1803b1c43
00000001803B1AB6 xor       r8d, r8d
00000001803B1AB9 mov       edx, 0x2710
00000001803B1ABE call      0x1813480b0
00000001803B1AC3 mov       rcx, qword ptr [rbx + 0x28]
00000001803B1AC7 test      rcx, rcx
00000001803B1ACA je        0x1803b1c43
00000001803B1AD0 xor       edx, edx
00000001803B1AD2 call      0x181304510
00000001803B1AD7 xor       edx, edx
00000001803B1AD9 mov       rcx, rbx
00000001803B1ADC mov       rdi, rax
00000001803B1ADF call      0x181304510
00000001803B1AE4 test      rdi, rdi
00000001803B1AE7 je        0x1803b1c43
00000001803B1AED xor       r8d, r8d
00000001803B1AF0 mov       rdx, rax
00000001803B1AF3 mov       rcx, rdi
00000001803B1AF6 call      0x18132e8b0
00000001803B1AFB mov       rcx, qword ptr [rbx + 0x28]
00000001803B1AFF test      rcx, rcx
00000001803B1B02 je        0x1803b1c43
00000001803B1B08 xor       edx, edx
00000001803B1B0A call      0x181304510
00000001803B1B0F cmp       byte ptr [rip + 0x1933020], 0 # 0x181ce4b36
00000001803B1B16 mov       rdi, rax
00000001803B1B19 jne       0x1803b1b2e
00000001803B1B1B lea       rcx, [rip + 0x1810cc6] # 0x181bc27e8
00000001803B1B22 call      0x18024fef0
00000001803B1B27 mov       byte ptr [rip + 0x1933008], 1 # 0x181ce4b36
00000001803B1B2E mov       rcx, qword ptr [rip + 0x1810cb3] # 0x181bc27e8
00000001803B1B35 mov       rax, qword ptr [rcx + 0xb8]
00000001803B1B3C test      rdi, rdi
00000001803B1B3F je        0x1803b1c43
00000001803B1B45 movsd     xmm0, qword ptr [rax]
00000001803B1B49 lea       rdx, [rsp + 0x20]
00000001803B1B4E mov       eax, dword ptr [rax + 8]
00000001803B1B51 xor       r8d, r8d
00000001803B1B54 mov       rcx, rdi
00000001803B1B57 movsd     qword ptr [rsp + 0x20], xmm0
00000001803B1B5D mov       dword ptr [rsp + 0x28], eax
00000001803B1B61 call      0x18132f910
00000001803B1B66 mov       byte ptr [rbx + 0x40], bpl
00000001803B1B6A test      rsi, rsi
00000001803B1B6D je        0x1803b1c43
00000001803B1B73 mov       rcx, qword ptr [rip + 0x18048fe] # 0x181bb6478
00000001803B1B7A mov       edi, dword ptr [rsi + 0x60]
00000001803B1B7D cmp       dword ptr [rcx + 0xe0], 0
00000001803B1B84 jne       0x1803b1b8b
00000001803B1B86 call      0x1802501e0
00000001803B1B8B xor       edx, edx
00000001803B1B8D mov       ecx, edi
00000001803B1B8F call      0x18033ae30
00000001803B1B94 test      rax, rax
00000001803B1B97 je        0x1803b1c25
00000001803B1B9D cmp       byte ptr [rax + 0x20], 0
00000001803B1BA1 mov       edi, 1
00000001803B1BA6 jne       0x1803b1bac
00000001803B1BA8 movzx     edi, byte ptr [rax + 0x21]
00000001803B1BAC mov       rcx, qword ptr [rbx + 0x30]
00000001803B1BB0 test      rcx, rcx
00000001803B1BB3 je        0x1803b1c43
00000001803B1BB9 test      edi, edi
00000001803B1BBB sete      dl
00000001803B1BBE xor       r8d, r8d
00000001803B1BC1 call      0x18156cc70
00000001803B1BC6 mov       rcx, qword ptr [rbx + 0x38]
00000001803B1BCA test      rcx, rcx
00000001803B1BCD je        0x1803b1c43
00000001803B1BCF xor       edx, edx
00000001803B1BD1 call      0x1813044d0
00000001803B1BD6 test      rax, rax
00000001803B1BD9 je        0x1803b1c43
00000001803B1BDB test      edi, edi
00000001803B1BDD setne     dl
00000001803B1BE0 jmp       0x1803b1c1a
00000001803B1BE2 xor       edx, edx
00000001803B1BE4 mov       byte ptr [rbx + 0x40], 0
00000001803B1BE8 mov       rcx, rbx
00000001803B1BEB call      0x1803b1c50
00000001803B1BF0 mov       rcx, qword ptr [rbx + 0x30]
00000001803B1BF4 test      rcx, rcx
00000001803B1BF7 je        0x1803b1c43
00000001803B1BF9 xor       r8d, r8d
00000001803B1BFC xor       edx, edx
00000001803B1BFE call      0x18156cc70
00000001803B1C03 mov       rcx, qword ptr [rbx + 0x38]
00000001803B1C07 test      rcx, rcx
00000001803B1C0A je        0x1803b1c43
00000001803B1C0C xor       edx, edx
00000001803B1C0E call      0x1813044d0
00000001803B1C13 test      rax, rax
00000001803B1C16 je        0x1803b1c43
00000001803B1C18 mov       dl, 1
00000001803B1C1A xor       r8d, r8d
00000001803B1C1D mov       rcx, rax
00000001803B1C20 call      0x18131bad0
00000001803B1C25 xor       edx, edx
00000001803B1C27 mov       rcx, rbx
00000001803B1C2A mov       rdi, qword ptr [rsp + 0x40]
00000001803B1C2F mov       rbx, qword ptr [rsp + 0x48]
00000001803B1C34 mov       rbp, qword ptr [rsp + 0x50]
00000001803B1C39 add       rsp, 0x30
00000001803B1C3D pop       rsi
00000001803B1C3E jmp       0x1803b1c50
00000001803B1C43 call      0x180250150
00000001803B1C48 int3      