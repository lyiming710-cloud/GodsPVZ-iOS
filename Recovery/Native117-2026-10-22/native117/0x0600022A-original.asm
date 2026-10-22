0000000180339A90 push      rbx
0000000180339A92 push      rsi
0000000180339A93 push      rdi
0000000180339A94 push      r14
0000000180339A96 push      r15
0000000180339A98 sub       rsp, 0x50
0000000180339A9C cmp       byte ptr [rip + 0x19ab17f], 0 # 0x181ce4c22
0000000180339AA3 jne       0x180339b88
0000000180339AA9 lea       rcx, [rip + 0x1861298] # 0x181b9ad48
0000000180339AB0 call      0x18024fef0
0000000180339AB5 lea       rcx, [rip + 0x186518c] # 0x181b9ec48
0000000180339ABC call      0x18024fef0
0000000180339AC1 lea       rcx, [rip + 0x186b428] # 0x181ba4ef0
0000000180339AC8 call      0x18024fef0
0000000180339ACD lea       rcx, [rip + 0x186b77c] # 0x181ba5250
0000000180339AD4 call      0x18024fef0
0000000180339AD9 lea       rcx, [rip + 0x1872a08] # 0x181bac4e8
0000000180339AE0 call      0x18024fef0
0000000180339AE5 lea       rcx, [rip + 0x187af54] # 0x181bb4a40
0000000180339AEC call      0x18024fef0
0000000180339AF1 lea       rcx, [rip + 0x1876a08] # 0x181bb0500
0000000180339AF8 call      0x18024fef0
0000000180339AFD lea       rcx, [rip + 0x1877de4] # 0x181bb18e8
0000000180339B04 call      0x18024fef0
0000000180339B09 lea       rcx, [rip + 0x1867a08] # 0x181ba1518
0000000180339B10 call      0x18024fef0
0000000180339B15 lea       rcx, [rip + 0x18786bc] # 0x181bb21d8
0000000180339B1C call      0x18024fef0
0000000180339B21 lea       rcx, [rip + 0x187c950] # 0x181bb6478
0000000180339B28 call      0x18024fef0
0000000180339B2D lea       rcx, [rip + 0x18861bc] # 0x181bbfcf0
0000000180339B34 call      0x18024fef0
0000000180339B39 lea       rcx, [rip + 0x1870980] # 0x181baa4c0
0000000180339B40 call      0x18024fef0
0000000180339B45 lea       rcx, [rip + 0x188cda4] # 0x181bc68f0
0000000180339B4C call      0x18024fef0
0000000180339B51 lea       rcx, [rip + 0x1868c70] # 0x181ba27c8
0000000180339B58 call      0x18024fef0
0000000180339B5D lea       rcx, [rip + 0x186b9c4] # 0x181ba5528
0000000180339B64 call      0x18024fef0
0000000180339B69 lea       rcx, [rip + 0x186ba18] # 0x181ba5588
0000000180339B70 call      0x18024fef0
0000000180339B75 lea       rcx, [rip + 0x1866a34] # 0x181ba05b0
0000000180339B7C call      0x18024fef0
0000000180339B81 mov       byte ptr [rip + 0x19ab09a], 1 # 0x181ce4c22
0000000180339B88 xor       r15d, r15d
0000000180339B8B mov       qword ptr [rsp + 0x98], r15
0000000180339B93 mov       rcx, qword ptr [rip + 0x1877d4e] # 0x181bb18e8
0000000180339B9A cmp       dword ptr [rcx + 0xe0], r15d
0000000180339BA1 jne       0x180339ba8
0000000180339BA3 call      0x1802501e0
0000000180339BA8 xor       r8d, r8d
0000000180339BAB mov       rdx, qword ptr [rip + 0x187090e] # 0x181baa4c0
0000000180339BB2 mov       rcx, qword ptr [rip + 0x188cd37] # 0x181bc68f0
0000000180339BB9 call      0x180c84600
0000000180339BBE mov       r14, rax
0000000180339BC1 mov       rbx, qword ptr [rip + 0x1867950] # 0x181ba1518
0000000180339BC8 mov       rcx, qword ptr [rip + 0x1886121] # 0x181bbfcf0
0000000180339BCF cmp       dword ptr [rcx + 0xe0], r15d
0000000180339BD6 jne       0x180339bdd
0000000180339BD8 call      0x1802501e0
0000000180339BDD xor       edx, edx
0000000180339BDF mov       rcx, rbx
0000000180339BE2 call      0x180ccee30
0000000180339BE7 mov       rbx, rax
0000000180339BEA mov       rcx, qword ptr [rip + 0x1865057] # 0x181b9ec48
0000000180339BF1 cmp       dword ptr [rcx + 0xe0], r15d
0000000180339BF8 jne       0x180339bff
0000000180339BFA call      0x1802501e0
0000000180339BFF xor       edx, edx
0000000180339C01 mov       rcx, rbx
0000000180339C04 call      0x180cdd3b0
0000000180339C09 test      rax, rax
0000000180339C0C je        0x180339fa0
0000000180339C12 xor       edx, edx
0000000180339C14 mov       rcx, rax
0000000180339C17 call      0x180cba170
0000000180339C1C mov       qword ptr [rsp + 0x90], rax
0000000180339C24 lea       rax, [rsp + 0x90]
0000000180339C2C mov       qword ptr [rsp + 0x28], rax
0000000180339C31 lea       rax, [rsp + 0x98]
0000000180339C39 mov       qword ptr [rsp + 0x30], rax
0000000180339C3E mov       qword ptr [rsp + 0x38], r15
0000000180339C43 movups    xmm0, xmmword ptr [rsp + 0x28]
0000000180339C48 movups    xmmword ptr [rsp + 0x40], xmm0
0000000180339C4D nop       dword ptr [rax]
0000000180339C50 mov       r8, qword ptr [rsp + 0x90]
0000000180339C58 test      r8, r8
0000000180339C5B je        0x180339fc8
0000000180339C61 xor       ecx, ecx
0000000180339C63 mov       rdx, qword ptr [rip + 0x186b5e6] # 0x181ba5250
0000000180339C6A call      0x180002cd0
0000000180339C6F test      al, al
0000000180339C71 je        0x180339f15
0000000180339C77 mov       rbx, qword ptr [rsp + 0x90]
0000000180339C7F test      rbx, rbx
0000000180339C82 je        0x180339fc3
0000000180339C88 mov       r9, qword ptr [rip + 0x186b5c1] # 0x181ba5250
0000000180339C8F mov       r10, qword ptr [rbx]
0000000180339C92 movzx     ecx, r15w
0000000180339C96 movzx     edx, word ptr [r10 + 0x12e]
0000000180339C9E cmp       r15w, dx
0000000180339CA2 jae       0x180339cc8
0000000180339CA4 mov       r8, qword ptr [r10 + 0xb0]
0000000180339CAB nop       dword ptr [rax + rax]
0000000180339CB0 movzx     eax, cx
0000000180339CB3 add       rax, rax
0000000180339CB6 cmp       qword ptr [r8 + rax*8], r9
0000000180339CBA je        0x180339e25
0000000180339CC0 inc       cx
0000000180339CC3 cmp       cx, dx
0000000180339CC6 jb        0x180339cb0
0000000180339CC8 mov       r8d, 1
0000000180339CCE mov       rdx, r9
0000000180339CD1 mov       rcx, rbx
0000000180339CD4 call      0x180258380
0000000180339CD9 mov       rdx, rax
0000000180339CDC mov       rax, qword ptr [rdx]
0000000180339CDF mov       rdx, qword ptr [rdx + 8]
0000000180339CE3 mov       rcx, rbx
0000000180339CE6 call      rax
0000000180339CE8 mov       rcx, rax
0000000180339CEB mov       r8, qword ptr [rip + 0x18784e6] # 0x181bb21d8
0000000180339CF2 test      rax, rax
0000000180339CF5 je        0x180339fbe
0000000180339CFB mov       rdx, qword ptr [rax]
0000000180339CFE mov       rax, qword ptr [r8 + 0x40]
0000000180339D02 cmp       qword ptr [rdx + 0x40], rax
0000000180339D06 jne       0x180339fb6
0000000180339D0C call      0x18024f540
0000000180339D11 mov       edi, dword ptr [rax]
0000000180339D13 mov       rbx, qword ptr [rip + 0x18677fe] # 0x181ba1518
0000000180339D1A mov       rcx, qword ptr [rip + 0x1885fcf] # 0x181bbfcf0
0000000180339D21 cmp       dword ptr [rcx + 0xe0], 0
0000000180339D28 jne       0x180339d2f
0000000180339D2A call      0x1802501e0
0000000180339D2F xor       edx, edx
0000000180339D31 mov       rcx, rbx
0000000180339D34 call      0x180ccee30
0000000180339D39 mov       rsi, rax
0000000180339D3C mov       dword ptr [rsp + 0x88], edi
0000000180339D43 lea       rdx, [rsp + 0x88]
0000000180339D4B mov       rcx, qword ptr [rip + 0x1878486] # 0x181bb21d8
0000000180339D52 call      0x18024f340
0000000180339D57 mov       rbx, rax
0000000180339D5A mov       rcx, qword ptr [rip + 0x1864ee7] # 0x181b9ec48
0000000180339D61 cmp       dword ptr [rcx + 0xe0], 0
0000000180339D68 jne       0x180339d6f
0000000180339D6A call      0x1802501e0
0000000180339D6F xor       r8d, r8d
0000000180339D72 mov       rdx, rbx
0000000180339D75 mov       rcx, rsi
0000000180339D78 call      0x180cdcd20
0000000180339D7D mov       rdi, rax
0000000180339D80 mov       rcx, qword ptr [rip + 0x1877b61] # 0x181bb18e8
0000000180339D87 cmp       dword ptr [rcx + 0xe0], 0
0000000180339D8E jne       0x180339d95
0000000180339D90 call      0x1802501e0
0000000180339D95 xor       r8d, r8d
0000000180339D98 mov       rdx, rdi
0000000180339D9B mov       rcx, r14
0000000180339D9E call      0x180c84600
0000000180339DA3 mov       rdx, qword ptr [rip + 0x187273e] # 0x181bac4e8
0000000180339DAA mov       rcx, rax
0000000180339DAD call      0x18047ba00
0000000180339DB2 mov       rbx, rax
0000000180339DB5 mov       rax, qword ptr [rip + 0x187c6bc] # 0x181bb6478
0000000180339DBC cmp       dword ptr [rax + 0xe0], 0
0000000180339DC3 jne       0x180339dd4
0000000180339DC5 mov       rcx, rax
0000000180339DC8 call      0x1802501e0
0000000180339DCD mov       rax, qword ptr [rip + 0x187c6a4] # 0x181bb6478
0000000180339DD4 mov       rax, qword ptr [rax + 0xb8]
0000000180339DDB mov       rcx, qword ptr [rax + 0x158]
0000000180339DE2 test      rcx, rcx
0000000180339DE5 je        0x180339fb1
0000000180339DEB mov       r9, qword ptr [rip + 0x187ac4e] # 0x181bb4a40
0000000180339DF2 inc       dword ptr [rcx + 0x1c]
0000000180339DF5 mov       rdx, qword ptr [rcx + 0x10]
0000000180339DF9 movsxd    r8, dword ptr [rcx + 0x18]
0000000180339DFD test      rdx, rdx
0000000180339E00 je        0x180339fac
0000000180339E06 cmp       r8d, dword ptr [rdx + 0x18]
0000000180339E0A jb        0x180339e48
0000000180339E0C mov       rax, qword ptr [r9 + 0x20]
0000000180339E10 mov       r8, qword ptr [rax + 0xc0]
0000000180339E17 mov       r8, qword ptr [r8 + 0x70]
0000000180339E1B mov       rdx, rbx
0000000180339E1E call      0x180847ce0
0000000180339E23 jmp       0x180339e71
0000000180339E25 movzx     ecx, cx
0000000180339E28 add       rcx, rcx
0000000180339E2B mov       ecx, dword ptr [r8 + rcx*8 + 8]
0000000180339E30 inc       ecx
0000000180339E32 movsxd    rdx, ecx
0000000180339E35 shl       rdx, 4
0000000180339E39 add       rdx, 0x138
0000000180339E40 add       rdx, r10
0000000180339E43 jmp       0x180339cdc
0000000180339E48 lea       eax, [r8 + 1]
0000000180339E4C mov       dword ptr [rcx + 0x18], eax
0000000180339E4F mov       rax, r8
0000000180339E52 cmp       r8d, dword ptr [rdx + 0x18]
0000000180339E56 jae       0x180339fa6
0000000180339E5C mov       qword ptr [rdx + r8*8 + 0x20], rbx
0000000180339E61 add       rax, 4
0000000180339E65 lea       rcx, [rdx + rax*8]
0000000180339E69 mov       rdx, rbx
0000000180339E6C call      0x18024f360
0000000180339E71 mov       rcx, qword ptr [rip + 0x1876688] # 0x181bb0500
0000000180339E78 cmp       dword ptr [rcx + 0xe0], 0
0000000180339E7F jne       0x180339e86
0000000180339E81 call      0x1802501e0
0000000180339E86 xor       edx, edx
0000000180339E88 mov       rcx, rbx
0000000180339E8B call      0x18131f870
0000000180339E90 test      al, al
0000000180339E92 jne       0x180339ed5
0000000180339E94 mov       rcx, qword ptr [rip + 0x1877a4d] # 0x181bb18e8
0000000180339E9B cmp       dword ptr [rcx + 0xe0], 0
0000000180339EA2 jne       0x180339ea9
0000000180339EA4 call      0x1802501e0
0000000180339EA9 xor       r8d, r8d
0000000180339EAC mov       rdx, rdi
0000000180339EAF mov       rcx, r14
0000000180339EB2 call      0x180c84600
0000000180339EB7 xor       r9d, r9d
0000000180339EBA mov       r8, qword ptr [rip + 0x186b667] # 0x181ba5528
0000000180339EC1 mov       rdx, rax
0000000180339EC4 mov       rcx, qword ptr [rip + 0x18666e5] # 0x181ba05b0
0000000180339ECB call      0x180b73f50
0000000180339ED0 mov       rbx, rax
0000000180339ED3 jmp       0x180339ef1
0000000180339ED5 xor       r9d, r9d
0000000180339ED8 mov       r8, qword ptr [rip + 0x186b6a9] # 0x181ba5588
0000000180339EDF mov       rdx, rdi
0000000180339EE2 mov       rcx, qword ptr [rip + 0x18688df] # 0x181ba27c8
0000000180339EE9 call      0x180b73f50
0000000180339EEE mov       rbx, rax
0000000180339EF1 mov       rcx, qword ptr [rip + 0x1860e50] # 0x181b9ad48
0000000180339EF8 cmp       dword ptr [rcx + 0xe0], 0
0000000180339EFF jne       0x180339f06
0000000180339F01 call      0x1802501e0
0000000180339F06 xor       edx, edx
0000000180339F08 mov       rcx, rbx
0000000180339F0B call      0x1812e68f0
0000000180339F10 jmp       0x180339c50
0000000180339F15 mov       rax, qword ptr [rsp + 0x28]
0000000180339F1A mov       rdx, qword ptr [rip + 0x186afcf] # 0x181ba4ef0
0000000180339F21 mov       rcx, qword ptr [rax]
0000000180339F24 call      0x18024f380
0000000180339F29 mov       r8, rax
0000000180339F2C movups    xmm0, xmmword ptr [rsp + 0x28]
0000000180339F31 psrldq    xmm0, 8
0000000180339F36 movq      rax, xmm0
0000000180339F3B mov       qword ptr [rax], r8
0000000180339F3E test      r8, r8
0000000180339F41 je        0x180339f51
0000000180339F43 xor       ecx, ecx
0000000180339F45 mov       rdx, qword ptr [rip + 0x186afa4] # 0x181ba4ef0
0000000180339F4C call      0x180002d90
0000000180339F51 jmp       0x180339f67
0000000180339F53 lea       rcx, [rsp + 0x40]
0000000180339F58 call      0x1802fb000
0000000180339F5D mov       rcx, qword ptr [rsp + 0x38]
0000000180339F62 test      rcx, rcx
0000000180339F65 jne       0x180339fce
0000000180339F67 mov       rax, qword ptr [rip + 0x187c50a] # 0x181bb6478
0000000180339F6E cmp       dword ptr [rax + 0xe0], 0
0000000180339F75 jne       0x180339f86
0000000180339F77 mov       rcx, rax
0000000180339F7A call      0x1802501e0
0000000180339F7F mov       rax, qword ptr [rip + 0x187c4f2] # 0x181bb6478
0000000180339F86 mov       rax, qword ptr [rax + 0xb8]
0000000180339F8D mov       rax, qword ptr [rax + 0x158]
0000000180339F94 add       rsp, 0x50
0000000180339F98 pop       r15
0000000180339F9A pop       r14
0000000180339F9C pop       rdi
0000000180339F9D pop       rsi
0000000180339F9E pop       rbx
0000000180339F9F ret       
0000000180339FA0 call      0x180250150
0000000180339FA5 nop       
0000000180339FA6 call      0x180250140
0000000180339FAB nop       
0000000180339FAC call      0x180250150
0000000180339FB1 call      0x180250150
0000000180339FB6 mov       rdx, r8
0000000180339FB9 call      0x18024f3a0
0000000180339FBE call      0x180250150
0000000180339FC3 call      0x180250150
0000000180339FC8 call      0x180250150
0000000180339FCD nop       
0000000180339FCE call      0x180200e60
0000000180339FD3 int3      
0000000180339FD4 int3      
0000000180339FD5 int3      
0000000180339FD6 int3      
0000000180339FD7 int3      
0000000180339FD8 int3      
0000000180339FD9 int3      
0000000180339FDA int3      
0000000180339FDB int3      
0000000180339FDC int3      
0000000180339FDD int3      
0000000180339FDE int3      
0000000180339FDF int3      