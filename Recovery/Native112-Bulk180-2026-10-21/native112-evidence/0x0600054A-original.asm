0000000180387A80 push      rbx
0000000180387A82 sub       rsp, 0x20
0000000180387A86 cmp       byte ptr [rip + 0x195d399], 0 # 0x181ce4e26
0000000180387A8D mov       rbx, rcx
0000000180387A90 jne       0x180387ab1
0000000180387A92 lea       rcx, [rip + 0x181e6ff] # 0x181ba6198
0000000180387A99 call      0x18024fef0
0000000180387A9E lea       rcx, [rip + 0x1828a5b] # 0x181bb0500
0000000180387AA5 call      0x18024fef0
0000000180387AAA mov       byte ptr [rip + 0x195d375], 1 # 0x181ce4e26
0000000180387AB1 mov       rcx, qword ptr [rbx + 0x50]
0000000180387AB5 mov       qword ptr [rsp + 0x30], rdi
0000000180387ABA test      rcx, rcx
0000000180387ABD je        0x180387d2d
0000000180387AC3 mov       rax, qword ptr [rcx + 0x60]
0000000180387AC7 test      rax, rax
0000000180387ACA je        0x180387d2d
0000000180387AD0 mov       eax, dword ptr [rax + 0x58]
0000000180387AD3 cmp       dword ptr [rcx + 0xa8], eax
0000000180387AD9 jne       0x180387d0a
0000000180387ADF mov       rax, qword ptr [rcx + 0x60]
0000000180387AE3 mov       r8, rcx
0000000180387AE6 mov       eax, dword ptr [rax + 0x54]
0000000180387AE9 cmp       dword ptr [rcx + 0x110], eax
0000000180387AEF jne       0x180387cf9
0000000180387AF5 mov       rcx, qword ptr [rcx + 0x60]
0000000180387AF9 mov       eax, dword ptr [rcx + 0x5c]
0000000180387AFC cmp       dword ptr [r8 + 0xac], eax
0000000180387B03 jne       0x180387ce0
0000000180387B09 mov       rcx, qword ptr [rip + 0x18289f0] # 0x181bb0500
0000000180387B10 mov       rdi, qword ptr [rbx + 0x88]
0000000180387B17 cmp       dword ptr [rcx + 0xe0], 0
0000000180387B1E jne       0x180387b25
0000000180387B20 call      0x1802501e0
0000000180387B25 xor       edx, edx
0000000180387B27 mov       rcx, rdi
0000000180387B2A call      0x18131f870
0000000180387B2F test      al, al
0000000180387B31 je        0x180387b50
0000000180387B33 mov       rcx, qword ptr [rbx + 0x88]
0000000180387B3A test      rcx, rcx
0000000180387B3D je        0x180387d2d
0000000180387B43 xor       r8d, r8d
0000000180387B46 mov       edx, 1
0000000180387B4B call      0x18039fee0
0000000180387B50 mov       rcx, qword ptr [rip + 0x18289a9] # 0x181bb0500
0000000180387B57 mov       rdi, qword ptr [rbx + 0x90]
0000000180387B5E cmp       dword ptr [rcx + 0xe0], 0
0000000180387B65 jne       0x180387b6c
0000000180387B67 call      0x1802501e0
0000000180387B6C xor       edx, edx
0000000180387B6E mov       rcx, rdi
0000000180387B71 call      0x18131f870
0000000180387B76 test      al, al
0000000180387B78 je        0x180387c4b
0000000180387B7E cmp       dword ptr [rbx + 0x98], 1
0000000180387B85 jne       0x180387c4b
0000000180387B8B mov       rdi, qword ptr [rbx + 0x90]
0000000180387B92 test      rdi, rdi
0000000180387B95 je        0x180387d2d
0000000180387B9B mov       rdi, qword ptr [rdi + 0x60]
0000000180387B9F test      rdi, rdi
0000000180387BA2 je        0x180387d2d
0000000180387BA8 mov       rax, qword ptr [rdi + 0x50]
0000000180387BAC test      rax, rax
0000000180387BAF je        0x180387d2d
0000000180387BB5 mov       rcx, qword ptr [rax + 0x10]
0000000180387BB9 test      rcx, rcx
0000000180387BBC je        0x180387d2d
0000000180387BC2 mov       rdx, qword ptr [rbx + 0x28]
0000000180387BC6 xor       r8d, r8d
0000000180387BC9 call      0x180344320
0000000180387BCE mov       rax, qword ptr [rdi + 0x50]
0000000180387BD2 test      rax, rax
0000000180387BD5 je        0x180387d2d
0000000180387BDB mov       rdx, qword ptr [rax + 0x10]
0000000180387BDF test      rdx, rdx
0000000180387BE2 je        0x180387d2d
0000000180387BE8 mov       rcx, qword ptr [rdi + 0x40]
0000000180387BEC test      rcx, rcx
0000000180387BEF je        0x180387d2d
0000000180387BF5 mov       edx, dword ptr [rdx + 0x24]
0000000180387BF8 xor       r8d, r8d
0000000180387BFB call      0x1803a7150
0000000180387C00 xor       edx, edx
0000000180387C02 mov       rcx, rbx
0000000180387C05 call      0x1813044d0
0000000180387C0A test      rax, rax
0000000180387C0D je        0x180387d2d
0000000180387C13 xor       r8d, r8d
0000000180387C16 xor       edx, edx
0000000180387C18 mov       rcx, rax
0000000180387C1B call      0x18131bad0
0000000180387C20 xor       edx, edx
0000000180387C22 mov       rcx, rdi
0000000180387C25 call      0x1813044d0
0000000180387C2A test      rax, rax
0000000180387C2D je        0x180387d2d
0000000180387C33 xor       r8d, r8d
0000000180387C36 mov       dl, 1
0000000180387C38 mov       rcx, rax
0000000180387C3B call      0x18131bad0
0000000180387C40 mov       rdi, qword ptr [rsp + 0x30]
0000000180387C45 add       rsp, 0x20
0000000180387C49 pop       rbx
0000000180387C4A ret       
0000000180387C4B mov       rcx, qword ptr [rbx + 0x38]
0000000180387C4F test      rcx, rcx
0000000180387C52 je        0x180387d2d
0000000180387C58 xor       r8d, r8d
0000000180387C5B mov       dl, 1
0000000180387C5D call      0x18131bad0
0000000180387C62 mov       rcx, qword ptr [rbx + 0x50]
0000000180387C66 test      rcx, rcx
0000000180387C69 je        0x180387d2d
0000000180387C6F xor       edx, edx
0000000180387C71 call      0x1813044d0
0000000180387C76 test      rax, rax
0000000180387C79 je        0x180387d2d
0000000180387C7F xor       r8d, r8d
0000000180387C82 xor       edx, edx
0000000180387C84 mov       rcx, rax
0000000180387C87 call      0x18131bad0
0000000180387C8C mov       rcx, qword ptr [rbx + 0x80]
0000000180387C93 test      rcx, rcx
0000000180387C96 je        0x180387d2d
0000000180387C9C xor       edx, edx
0000000180387C9E call      0x1813044d0
0000000180387CA3 test      rax, rax
0000000180387CA6 je        0x180387d2d
0000000180387CAC xor       r8d, r8d
0000000180387CAF mov       dl, 1
0000000180387CB1 mov       rcx, rax
0000000180387CB4 call      0x18131bad0
0000000180387CB9 mov       rax, qword ptr [rbx + 0x70]
0000000180387CBD test      rax, rax
0000000180387CC0 je        0x180387d2d
0000000180387CC2 cmp       dword ptr [rax + 0x18], 0
0000000180387CC6 jg        0x180387c40
0000000180387CCC xor       edx, edx
0000000180387CCE mov       rcx, rbx
0000000180387CD1 mov       rdi, qword ptr [rsp + 0x30]
0000000180387CD6 add       rsp, 0x20
0000000180387CDA pop       rbx
0000000180387CDB jmp       0x180387f80
0000000180387CE0 mov       rcx, r8
0000000180387CE3 test      r8, r8
0000000180387CE6 je        0x180387d2d
0000000180387CE8 xor       edx, edx
0000000180387CEA mov       rdi, qword ptr [rsp + 0x30]
0000000180387CEF add       rsp, 0x20
0000000180387CF3 pop       rbx
0000000180387CF4 jmp       0x18038b6b0
0000000180387CF9 xor       edx, edx
0000000180387CFB mov       rdi, qword ptr [rsp + 0x30]
0000000180387D00 add       rsp, 0x20
0000000180387D04 pop       rbx
0000000180387D05 jmp       0x18038b4f0
0000000180387D0A test      rcx, rcx
0000000180387D0D je        0x180387d2d
0000000180387D0F mov       rdx, qword ptr [rcx + 0x60]
0000000180387D13 test      rdx, rdx
0000000180387D16 je        0x180387d2d
0000000180387D18 mov       edx, dword ptr [rdx + 0x58]
0000000180387D1B xor       r8d, r8d
0000000180387D1E mov       rdi, qword ptr [rsp + 0x30]
0000000180387D23 add       rsp, 0x20
0000000180387D27 pop       rbx
0000000180387D28 jmp       0x18038dcc0
0000000180387D2D call      0x180250150
0000000180387D32 int3      