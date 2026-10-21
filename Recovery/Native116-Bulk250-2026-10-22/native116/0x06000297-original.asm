0000000180329AA0 mov       qword ptr [rsp + 0x18], rbx
0000000180329AA5 push      rsi
0000000180329AA6 sub       rsp, 0x20
0000000180329AAA cmp       byte ptr [rip + 0x19bb1c5], 0 # 0x181ce4c76
0000000180329AB1 mov       rsi, rdx
0000000180329AB4 mov       rbx, rcx
0000000180329AB7 jne       0x180329ad8
0000000180329AB9 lea       rcx, [rip + 0x1886a40] # 0x181bb0500
0000000180329AC0 call      0x18024fef0
0000000180329AC5 lea       rcx, [rip + 0x188678c] # 0x181bb0258
0000000180329ACC call      0x18024fef0
0000000180329AD1 mov       byte ptr [rip + 0x19bb19e], 1 # 0x181ce4c76
0000000180329AD8 mov       eax, dword ptr [rbx + 0x10]
0000000180329ADB mov       qword ptr [rsp + 0x38], rdi
0000000180329AE0 cmp       eax, 3
0000000180329AE3 jne       0x180329bca
0000000180329AE9 test      rsi, rsi
0000000180329AEC je        0x180329d3c
0000000180329AF2 xor       edx, edx
0000000180329AF4 mov       rcx, rsi
0000000180329AF7 call      0x1802ffcb0
0000000180329AFC mov       rdi, qword ptr [rbx + 0x28]
0000000180329B00 mov       rcx, qword ptr [rip + 0x18869f9] # 0x181bb0500
0000000180329B07 test      al, al
0000000180329B09 jne       0x180329b7f
0000000180329B0B cmp       dword ptr [rcx + 0xe0], 0
0000000180329B12 jne       0x180329b19
0000000180329B14 call      0x1802501e0
0000000180329B19 xor       r8d, r8d
0000000180329B1C xor       edx, edx
0000000180329B1E mov       rcx, rdi
0000000180329B21 call      0x18131f900
0000000180329B26 test      al, al
0000000180329B28 jne       0x180329b6a
0000000180329B2A mov       rcx, qword ptr [rip + 0x18869cf] # 0x181bb0500
0000000180329B31 mov       rdi, qword ptr [rbx + 0x48]
0000000180329B35 cmp       dword ptr [rcx + 0xe0], 0
0000000180329B3C jne       0x180329b43
0000000180329B3E call      0x1802501e0
0000000180329B43 xor       r8d, r8d
0000000180329B46 xor       edx, edx
0000000180329B48 mov       rcx, rdi
0000000180329B4B call      0x18131f900
0000000180329B50 test      al, al
0000000180329B52 jne       0x180329bd8
0000000180329B58 xor       al, al
0000000180329B5A mov       rdi, qword ptr [rsp + 0x38]
0000000180329B5F mov       rbx, qword ptr [rsp + 0x40]
0000000180329B64 add       rsp, 0x20
0000000180329B68 pop       rsi
0000000180329B69 ret       
0000000180329B6A mov       rax, qword ptr [rbx + 0x28]
0000000180329B6E test      rax, rax
0000000180329B71 je        0x180329d3c
0000000180329B77 cmp       dword ptr [rax + 0x78], 0x10
0000000180329B7B jne       0x180329b58
0000000180329B7D jmp       0x180329bd8
0000000180329B7F cmp       dword ptr [rcx + 0xe0], 0
0000000180329B86 jne       0x180329b8d
0000000180329B88 call      0x1802501e0
0000000180329B8D xor       r8d, r8d
0000000180329B90 xor       edx, edx
0000000180329B92 mov       rcx, rdi
0000000180329B95 call      0x18131f900
0000000180329B9A test      al, al
0000000180329B9C jne       0x180329b58
0000000180329B9E mov       rcx, qword ptr [rip + 0x188695b] # 0x181bb0500
0000000180329BA5 mov       rdi, qword ptr [rbx + 0x48]
0000000180329BA9 cmp       dword ptr [rcx + 0xe0], 0
0000000180329BB0 jne       0x180329bb7
0000000180329BB2 call      0x1802501e0
0000000180329BB7 xor       r8d, r8d
0000000180329BBA xor       edx, edx
0000000180329BBC mov       rcx, rdi
0000000180329BBF call      0x18131f900
0000000180329BC4 test      al, al
0000000180329BC6 jne       0x180329b58
0000000180329BC8 jmp       0x180329bd8
0000000180329BCA cmp       eax, 5
0000000180329BCD je        0x180329b58
0000000180329BCF test      rsi, rsi
0000000180329BD2 je        0x180329d3c
0000000180329BD8 mov       ecx, dword ptr [rsi + 0x70]
0000000180329BDB xor       edi, edi
0000000180329BDD mov       qword ptr [rsp + 0x30], rbp
0000000180329BE2 test      ecx, ecx
0000000180329BE4 je        0x180329c0b
0000000180329BE6 sub       ecx, 1
0000000180329BE9 je        0x180329c05
0000000180329BEB sub       ecx, 1
0000000180329BEE je        0x180329bff
0000000180329BF0 cmp       ecx, 1
0000000180329BF3 je        0x180329bf9
0000000180329BF5 mov       ebp, edi
0000000180329BF7 jmp       0x180329c0f
0000000180329BF9 mov       rbp, qword ptr [rbx + 0x40]
0000000180329BFD jmp       0x180329c0f
0000000180329BFF mov       rbp, qword ptr [rbx + 0x30]
0000000180329C03 jmp       0x180329c0f
0000000180329C05 mov       rbp, qword ptr [rbx + 0x38]
0000000180329C09 jmp       0x180329c0f
0000000180329C0B mov       rbp, qword ptr [rbx + 0x28]
0000000180329C0F mov       ecx, dword ptr [rsi + 0x70]
0000000180329C12 test      ecx, ecx
0000000180329C14 je        0x180329c42
0000000180329C16 sub       ecx, 1
0000000180329C19 je        0x180329c3c
0000000180329C1B sub       ecx, 1
0000000180329C1E je        0x180329c36
0000000180329C20 sub       ecx, 1
0000000180329C23 je        0x180329c30
0000000180329C25 cmp       ecx, 1
0000000180329C28 jne       0x180329c46
0000000180329C2A mov       rdi, qword ptr [rbx + 0x58]
0000000180329C2E jmp       0x180329c46
0000000180329C30 mov       rdi, qword ptr [rbx + 0x68]
0000000180329C34 jmp       0x180329c46
0000000180329C36 mov       rdi, qword ptr [rbx + 0x50]
0000000180329C3A jmp       0x180329c46
0000000180329C3C mov       rdi, qword ptr [rbx + 0x60]
0000000180329C40 jmp       0x180329c46
0000000180329C42 mov       rdi, qword ptr [rbx + 0x48]
0000000180329C46 mov       rcx, qword ptr [rip + 0x18868b3] # 0x181bb0500
0000000180329C4D cmp       dword ptr [rcx + 0xe0], 0
0000000180329C54 jne       0x180329c5b
0000000180329C56 call      0x1802501e0
0000000180329C5B xor       r8d, r8d
0000000180329C5E xor       edx, edx
0000000180329C60 mov       rcx, rbp
0000000180329C63 call      0x18131f900
0000000180329C68 mov       rbp, qword ptr [rsp + 0x30]
0000000180329C6D test      al, al
0000000180329C6F jne       0x180329b58
0000000180329C75 mov       rcx, qword ptr [rip + 0x1886884] # 0x181bb0500
0000000180329C7C cmp       dword ptr [rcx + 0xe0], 0
0000000180329C83 jne       0x180329c8a
0000000180329C85 call      0x1802501e0
0000000180329C8A xor       r8d, r8d
0000000180329C8D xor       edx, edx
0000000180329C8F mov       rcx, rdi
0000000180329C92 call      0x18131f900
0000000180329C97 test      al, al
0000000180329C99 jne       0x180329b58
0000000180329C9F mov       rcx, qword ptr [rip + 0x188685a] # 0x181bb0500
0000000180329CA6 mov       rdi, qword ptr [rbx + 0x48]
0000000180329CAA cmp       dword ptr [rcx + 0xe0], 0
0000000180329CB1 jne       0x180329cb8
0000000180329CB3 call      0x1802501e0
0000000180329CB8 xor       edx, edx
0000000180329CBA mov       rcx, rdi
0000000180329CBD call      0x18131f870
0000000180329CC2 test      al, al
0000000180329CC4 je        0x180329ce8
0000000180329CC6 test      rdi, rdi
0000000180329CC9 je        0x180329d3c
0000000180329CCB mov       r8, qword ptr [rip + 0x1886586] # 0x181bb0258
0000000180329CD2 xor       r9d, r9d
0000000180329CD5 mov       edx, dword ptr [rsi + 0x70]
0000000180329CD8 mov       rcx, rdi
0000000180329CDB call      0x180346b60
0000000180329CE0 test      al, al
0000000180329CE2 je        0x180329b58
0000000180329CE8 mov       rcx, qword ptr [rip + 0x1886811] # 0x181bb0500
0000000180329CEF mov       rbx, qword ptr [rbx + 0x28]
0000000180329CF3 cmp       dword ptr [rcx + 0xe0], 0
0000000180329CFA jne       0x180329d01
0000000180329CFC call      0x1802501e0
0000000180329D01 xor       edx, edx
0000000180329D03 mov       rcx, rbx
0000000180329D06 call      0x18131f870
0000000180329D0B test      al, al
0000000180329D0D je        0x180329d2a
0000000180329D0F test      rbx, rbx
0000000180329D12 je        0x180329d3c
0000000180329D14 mov       edx, dword ptr [rsi + 0x70]
0000000180329D17 xor       r8d, r8d
0000000180329D1A mov       rcx, rbx
0000000180329D1D call      0x18034e180
0000000180329D22 test      al, al
0000000180329D24 je        0x180329b58
0000000180329D2A mov       rdi, qword ptr [rsp + 0x38]
0000000180329D2F mov       al, 1
0000000180329D31 mov       rbx, qword ptr [rsp + 0x40]
0000000180329D36 add       rsp, 0x20
0000000180329D3A pop       rsi
0000000180329D3B ret       
0000000180329D3C call      0x180250150
0000000180329D41 int3      
0000000180329D42 int3      
0000000180329D43 int3      
0000000180329D44 int3      
0000000180329D45 int3      
0000000180329D46 int3      
0000000180329D47 int3      
0000000180329D48 int3      
0000000180329D49 int3      
0000000180329D4A int3      
0000000180329D4B int3      
0000000180329D4C int3      
0000000180329D4D int3      
0000000180329D4E int3      
0000000180329D4F int3      