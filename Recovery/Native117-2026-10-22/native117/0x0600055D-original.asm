0000000180389B30 mov       qword ptr [rsp + 8], rcx
0000000180389B35 push      rbx
0000000180389B36 push      rsi
0000000180389B37 push      rdi
0000000180389B38 push      r12
0000000180389B3A push      r13
0000000180389B3C push      r14
0000000180389B3E push      r15
0000000180389B40 sub       rsp, 0x50
0000000180389B44 mov       r14, rcx
0000000180389B47 cmp       byte ptr [rip + 0x195b2e2], 0 # 0x181ce4e30
0000000180389B4E jne       0x180389bf7
0000000180389B54 lea       rcx, [rip + 0x181392d] # 0x181b9d488
0000000180389B5B call      0x18024fef0
0000000180389B60 lea       rcx, [rip + 0x1813521] # 0x181b9d088
0000000180389B67 call      0x18024fef0
0000000180389B6C lea       rcx, [rip + 0x18150d5] # 0x181b9ec48
0000000180389B73 call      0x18024fef0
0000000180389B78 lea       rcx, [rip + 0x1818499] # 0x181ba2018
0000000180389B7F call      0x18024fef0
0000000180389B84 lea       rcx, [rip + 0x181b365] # 0x181ba4ef0
0000000180389B8B call      0x18024fef0
0000000180389B90 lea       rcx, [rip + 0x181b6b9] # 0x181ba5250
0000000180389B97 call      0x18024fef0
0000000180389B9C lea       rcx, [rip + 0x1819cb5] # 0x181ba3858
0000000180389BA3 call      0x18024fef0
0000000180389BA8 lea       rcx, [rip + 0x1830739] # 0x181bba2e8
0000000180389BAF call      0x18024fef0
0000000180389BB4 lea       rcx, [rip + 0x1830785] # 0x181bba340
0000000180389BBB call      0x18024fef0
0000000180389BC0 lea       rcx, [rip + 0x182db69] # 0x181bb7730
0000000180389BC7 call      0x18024fef0
0000000180389BCC lea       rcx, [rip + 0x182692d] # 0x181bb0500
0000000180389BD3 call      0x18024fef0
0000000180389BD8 lea       rcx, [rip + 0x182c899] # 0x181bb6478
0000000180389BDF call      0x18024fef0
0000000180389BE4 lea       rcx, [rip + 0x1836105] # 0x181bbfcf0
0000000180389BEB call      0x18024fef0
0000000180389BF0 mov       byte ptr [rip + 0x195b239], 1 # 0x181ce4e30
0000000180389BF7 xor       r13d, r13d
0000000180389BFA mov       qword ptr [rsp + 0xa8], r13
0000000180389C02 mov       rcx, qword ptr [rip + 0x182c86f] # 0x181bb6478
0000000180389C09 cmp       dword ptr [rcx + 0xe0], r13d
0000000180389C10 jne       0x180389c1e
0000000180389C12 call      0x1802501e0
0000000180389C17 mov       rcx, qword ptr [rip + 0x182c85a] # 0x181bb6478
0000000180389C1E mov       rax, qword ptr [rcx + 0xb8]
0000000180389C25 mov       r12, qword ptr [rax + 0x1f0]
0000000180389C2C mov       rax, qword ptr [rip + 0x18183e5] # 0x181ba2018
0000000180389C33 cmp       dword ptr [rax + 0xe0], r13d
0000000180389C3A jne       0x180389c52
0000000180389C3C mov       rcx, rax
0000000180389C3F call      0x1802501e0
0000000180389C44 mov       rax, qword ptr [rip + 0x18183cd] # 0x181ba2018
0000000180389C4B mov       rcx, qword ptr [rip + 0x182c826] # 0x181bb6478
0000000180389C52 mov       rax, qword ptr [rax + 0xb8]
0000000180389C59 mov       rax, qword ptr [rax]
0000000180389C5C test      rax, rax
0000000180389C5F je        0x180389fb8
0000000180389C65 mov       rax, qword ptr [rax + 0x28]
0000000180389C69 test      rax, rax
0000000180389C6C je        0x180389fb8
0000000180389C72 mov       r15, qword ptr [rax + 0x10]
0000000180389C76 test      r15, r15
0000000180389C79 je        0x180389fa8
0000000180389C7F cmp       dword ptr [rcx + 0xe0], r13d
0000000180389C86 jne       0x180389c8d
0000000180389C88 call      0x1802501e0
0000000180389C8D xor       ecx, ecx
0000000180389C8F call      0x180338b00
0000000180389C94 mov       qword ptr [r14 + 0x28], rax
0000000180389C98 lea       rcx, [r14 + 0x28]
0000000180389C9C mov       rdx, rax
0000000180389C9F call      0x18024f360
0000000180389CA4 mov       rcx, qword ptr [rip + 0x1826855] # 0x181bb0500
0000000180389CAB cmp       dword ptr [rcx + 0xe0], r13d
0000000180389CB2 jne       0x180389cb9
0000000180389CB4 call      0x1802501e0
0000000180389CB9 xor       r8d, r8d
0000000180389CBC xor       edx, edx
0000000180389CBE mov       rcx, r12
0000000180389CC1 call      0x18131f760
0000000180389CC6 test      al, al
0000000180389CC8 jne       0x180389fa8
0000000180389CCE mov       rbx, qword ptr [rip + 0x18137b3] # 0x181b9d488
0000000180389CD5 mov       rcx, qword ptr [rip + 0x1836014] # 0x181bbfcf0
0000000180389CDC cmp       dword ptr [rcx + 0xe0], r13d
0000000180389CE3 jne       0x180389cea
0000000180389CE5 call      0x1802501e0
0000000180389CEA xor       edx, edx
0000000180389CEC mov       rcx, rbx
0000000180389CEF call      0x180ccee30
0000000180389CF4 mov       rbx, rax
0000000180389CF7 mov       rcx, qword ptr [rip + 0x1814f4a] # 0x181b9ec48
0000000180389CFE cmp       dword ptr [rcx + 0xe0], r13d
0000000180389D05 jne       0x180389d0c
0000000180389D07 call      0x1802501e0
0000000180389D0C xor       edx, edx
0000000180389D0E mov       rcx, rbx
0000000180389D11 call      0x180cdd3b0
0000000180389D16 test      rax, rax
0000000180389D19 je        0x180389fb8
0000000180389D1F xor       edx, edx
0000000180389D21 mov       rcx, rax
0000000180389D24 call      0x180cba170
0000000180389D29 mov       qword ptr [rsp + 0xa0], rax
0000000180389D31 lea       rax, [rsp + 0xa0]
0000000180389D39 mov       qword ptr [rsp + 0x28], rax
0000000180389D3E lea       rax, [rsp + 0xa8]
0000000180389D46 mov       qword ptr [rsp + 0x30], rax
0000000180389D4B mov       qword ptr [rsp + 0x38], r13
0000000180389D50 movups    xmm0, xmmword ptr [rsp + 0x28]
0000000180389D55 movups    xmmword ptr [rsp + 0x40], xmm0
0000000180389D5A nop       word ptr [rax + rax]
0000000180389D60 mov       r8, qword ptr [rsp + 0xa0]
0000000180389D68 test      r8, r8
0000000180389D6B je        0x180389ff4
0000000180389D71 xor       ecx, ecx
0000000180389D73 mov       rdx, qword ptr [rip + 0x181b4d6] # 0x181ba5250
0000000180389D7A call      0x180002cd0
0000000180389D7F test      al, al
0000000180389D81 je        0x180389f44
0000000180389D87 mov       rbx, qword ptr [rsp + 0xa0]
0000000180389D8F test      rbx, rbx
0000000180389D92 je        0x180389fef
0000000180389D98 mov       r9, qword ptr [rip + 0x181b4b1] # 0x181ba5250
0000000180389D9F mov       r10, qword ptr [rbx]
0000000180389DA2 movzx     ecx, r13w
0000000180389DA6 movzx     edx, word ptr [r10 + 0x12e]
0000000180389DAE cmp       r13w, dx
0000000180389DB2 jae       0x180389dd8
0000000180389DB4 mov       r8, qword ptr [r10 + 0xb0]
0000000180389DBB nop       dword ptr [rax + rax]
0000000180389DC0 movzx     eax, cx
0000000180389DC3 add       rax, rax
0000000180389DC6 cmp       qword ptr [r8 + rax*8], r9
0000000180389DCA je        0x180389e9b
0000000180389DD0 inc       cx
0000000180389DD3 cmp       cx, dx
0000000180389DD6 jb        0x180389dc0
0000000180389DD8 mov       r8d, 1
0000000180389DDE mov       rdx, r9
0000000180389DE1 mov       rcx, rbx
0000000180389DE4 call      0x180258380
0000000180389DE9 mov       rdx, rax
0000000180389DEC mov       rax, qword ptr [rdx]
0000000180389DEF mov       rdx, qword ptr [rdx + 8]
0000000180389DF3 mov       rcx, rbx
0000000180389DF6 call      rax
0000000180389DF8 mov       rcx, rax
0000000180389DFB mov       r8, qword ptr [rip + 0x1813286] # 0x181b9d088
0000000180389E02 test      rax, rax
0000000180389E05 je        0x180389fea
0000000180389E0B mov       rdx, qword ptr [rax]
0000000180389E0E mov       rax, qword ptr [r8 + 0x40]
0000000180389E12 cmp       qword ptr [rdx + 0x40], rax
0000000180389E16 jne       0x180389fe2
0000000180389E1C call      0x18024f540
0000000180389E21 mov       edi, dword ptr [rax]
0000000180389E23 test      edi, edi
0000000180389E25 js        0x180389d60
0000000180389E2B mov       rcx, qword ptr [rip + 0x18266ce] # 0x181bb0500
0000000180389E32 cmp       dword ptr [rcx + 0xe0], 0
0000000180389E39 jne       0x180389e40
0000000180389E3B call      0x1802501e0
0000000180389E40 mov       rdx, qword ptr [rip + 0x182d8e9] # 0x181bb7730
0000000180389E47 mov       rcx, r12
0000000180389E4A call      0x1804a0690
0000000180389E4F mov       rbx, rax
0000000180389E52 mov       rax, qword ptr [r14 + 0x28]
0000000180389E56 test      rax, rax
0000000180389E59 je        0x180389fdd
0000000180389E5F cmp       dword ptr [rax + 0x18], edi
0000000180389E62 jle       0x180389ebe
0000000180389E64 test      rbx, rbx
0000000180389E67 je        0x180389fc4
0000000180389E6D mov       rsi, qword ptr [rbx + 0x38]
0000000180389E71 mov       r8, qword ptr [rip + 0x18304c8] # 0x181bba340
0000000180389E78 mov       edx, edi
0000000180389E7A mov       rcx, rax
0000000180389E7D call      0x180825710
0000000180389E82 test      rsi, rsi
0000000180389E85 je        0x180389fbe
0000000180389E8B xor       r8d, r8d
0000000180389E8E mov       rdx, rax
0000000180389E91 mov       rcx, rsi
0000000180389E94 call      0x1813d7ab0
0000000180389E99 jmp       0x180389ec7
0000000180389E9B movzx     ecx, cx
0000000180389E9E add       rcx, rcx
0000000180389EA1 mov       ecx, dword ptr [r8 + rcx*8 + 8]
0000000180389EA6 inc       ecx
0000000180389EA8 movsxd    rdx, ecx
0000000180389EAB shl       rdx, 4
0000000180389EAF add       rdx, 0x138
0000000180389EB6 add       rdx, r10
0000000180389EB9 jmp       0x180389dec
0000000180389EBE test      rbx, rbx
0000000180389EC1 je        0x180389fd8
0000000180389EC7 xor       edx, edx
0000000180389EC9 mov       rcx, rbx
0000000180389ECC call      0x181304510
0000000180389ED1 mov       rsi, rax
0000000180389ED4 mov       rcx, qword ptr [r14 + 0x30]
0000000180389ED8 test      rcx, rcx
0000000180389EDB je        0x180389fd3
0000000180389EE1 xor       edx, edx
0000000180389EE3 call      0x18131c070
0000000180389EE8 test      rsi, rsi
0000000180389EEB je        0x180389fce
0000000180389EF1 xor       r9d, r9d
0000000180389EF4 xor       r8d, r8d
0000000180389EF7 mov       rdx, rax
0000000180389EFA mov       rcx, rsi
0000000180389EFD call      0x18132e900
0000000180389F02 mov       qword ptr [rbx + 0x20], r14
0000000180389F06 lea       rcx, [rbx + 0x20]
0000000180389F0A mov       rdx, r14
0000000180389F0D call      0x18024f360
0000000180389F12 xor       r9d, r9d
0000000180389F15 mov       r8d, edi
0000000180389F18 mov       rdx, r15
0000000180389F1B mov       rcx, rbx
0000000180389F1E call      0x18038a7e0
0000000180389F23 mov       rcx, qword ptr [r14 + 0x20]
0000000180389F27 test      rcx, rcx
0000000180389F2A je        0x180389fc9
0000000180389F30 mov       r8, qword ptr [rip + 0x1819921] # 0x181ba3858
0000000180389F37 mov       rdx, rbx
0000000180389F3A call      0x180002e70
0000000180389F3F jmp       0x180389d60
0000000180389F44 mov       rax, qword ptr [rsp + 0x28]
0000000180389F49 mov       rdx, qword ptr [rip + 0x181afa0] # 0x181ba4ef0
0000000180389F50 mov       rcx, qword ptr [rax]
0000000180389F53 call      0x18024f380
0000000180389F58 mov       r8, rax
0000000180389F5B movups    xmm0, xmmword ptr [rsp + 0x28]
0000000180389F60 psrldq    xmm0, 8
0000000180389F65 movq      rax, xmm0
0000000180389F6A mov       qword ptr [rax], r8
0000000180389F6D test      r8, r8
0000000180389F70 je        0x180389f80
0000000180389F72 xor       ecx, ecx
0000000180389F74 mov       rdx, qword ptr [rip + 0x181af75] # 0x181ba4ef0
0000000180389F7B call      0x180002d90
0000000180389F80 jmp       0x180389f9e
0000000180389F82 lea       rcx, [rsp + 0x40]
0000000180389F87 call      0x1802fb000
0000000180389F8C mov       rcx, qword ptr [rsp + 0x38]
0000000180389F91 test      rcx, rcx
0000000180389F94 jne       0x180389ffa
0000000180389F96 mov       r14, qword ptr [rsp + 0x90]
0000000180389F9E xor       edx, edx
0000000180389FA0 mov       rcx, r14
0000000180389FA3 call      0x18038a170
0000000180389FA8 add       rsp, 0x50
0000000180389FAC pop       r15
0000000180389FAE pop       r14
0000000180389FB0 pop       r13
0000000180389FB2 pop       r12
0000000180389FB4 pop       rdi
0000000180389FB5 pop       rsi
0000000180389FB6 pop       rbx
0000000180389FB7 ret       
0000000180389FB8 call      0x180250150
0000000180389FBD nop       
0000000180389FBE call      0x180250150
0000000180389FC3 nop       
0000000180389FC4 call      0x180250150
0000000180389FC9 call      0x180250150
0000000180389FCE call      0x180250150
0000000180389FD3 call      0x180250150
0000000180389FD8 call      0x180250150
0000000180389FDD call      0x180250150
0000000180389FE2 mov       rdx, r8
0000000180389FE5 call      0x18024f3a0
0000000180389FEA call      0x180250150
0000000180389FEF call      0x180250150
0000000180389FF4 call      0x180250150
0000000180389FF9 nop       
0000000180389FFA call      0x180200e60
0000000180389FFF int3      