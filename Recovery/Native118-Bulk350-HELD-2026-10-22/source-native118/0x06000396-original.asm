0000000180355880 mov       qword ptr [rsp + 0x10], rbx
0000000180355885 mov       qword ptr [rsp + 0x18], rsi
000000018035588A mov       qword ptr [rsp + 0x20], rdi
000000018035588F mov       qword ptr [rsp + 8], rcx
0000000180355894 push      r14
0000000180355896 sub       rsp, 0x60
000000018035589A mov       rdi, rcx
000000018035589D cmp       byte ptr [rip + 0x198f47d], 0 # 0x181ce4d21
00000001803558A4 jne       0x1803558f5
00000001803558A6 lea       rcx, [rip + 0x1874fbb] # 0x181bca868
00000001803558AD call      0x18024fef0
00000001803558B2 lea       rcx, [rip + 0x1874fff] # 0x181bca8b8
00000001803558B9 call      0x18024fef0
00000001803558BE lea       rcx, [rip + 0x187504b] # 0x181bca910
00000001803558C5 call      0x18024fef0
00000001803558CA lea       rcx, [rip + 0x186de17] # 0x181bc36e8
00000001803558D1 call      0x18024fef0
00000001803558D6 lea       rcx, [rip + 0x186df13] # 0x181bc37f0
00000001803558DD call      0x18024fef0
00000001803558E2 lea       rcx, [rip + 0x185ac17] # 0x181bb0500
00000001803558E9 call      0x18024fef0
00000001803558EE mov       byte ptr [rip + 0x198f42c], 1 # 0x181ce4d21
00000001803558F5 xorps     xmm0, xmm0
00000001803558F8 xor       eax, eax
00000001803558FA movups    xmmword ptr [rsp + 0x40], xmm0
00000001803558FF mov       qword ptr [rsp + 0x50], rax
0000000180355904 xor       r14b, r14b
0000000180355907 movsxd    rdx, dword ptr [rdi + 0x7c]
000000018035590B cmp       edx, 7
000000018035590E ja        0x180355c6d
0000000180355914 lea       r8, [rip - 0x35591b] # 0x180000000
000000018035591B mov       ecx, dword ptr [r8 + rdx*4 + 0x355d28]
0000000180355923 add       rcx, r8
0000000180355926 jmp       rcx
0000000180355928 cmp       dword ptr [rdi + 0x88], 1
000000018035592F jl        0x1803559ce
0000000180355935 mov       rbx, qword ptr [rdi + 0x120]
000000018035593C mov       rcx, qword ptr [rip + 0x185abbd] # 0x181bb0500
0000000180355943 cmp       dword ptr [rcx + 0xe0], eax
0000000180355949 jne       0x180355950
000000018035594B call      0x1802501e0
0000000180355950 xor       edx, edx
0000000180355952 mov       rcx, rbx
0000000180355955 call      0x18131f870
000000018035595A test      al, al
000000018035595C je        0x18035598b
000000018035595E mov       rcx, qword ptr [rdi + 0x120]
0000000180355965 test      rcx, rcx
0000000180355968 je        0x180355d13
000000018035596E xor       edx, edx
0000000180355970 call      0x18035de80
0000000180355975 test      al, al
0000000180355977 je        0x18035598b
0000000180355979 mov       r14b, 1
000000018035597C xor       edx, edx
000000018035597E mov       rcx, rdi
0000000180355981 call      0x180355d50
0000000180355986 jmp       0x180355bfc
000000018035598B mov       rbx, qword ptr [rdi + 0x128]
0000000180355992 mov       rcx, qword ptr [rip + 0x185ab67] # 0x181bb0500
0000000180355999 cmp       dword ptr [rcx + 0xe0], 0
00000001803559A0 jne       0x1803559a7
00000001803559A2 call      0x1802501e0
00000001803559A7 xor       edx, edx
00000001803559A9 mov       rcx, rbx
00000001803559AC call      0x18131f870
00000001803559B1 test      al, al
00000001803559B3 je        0x1803559ce
00000001803559B5 mov       rax, qword ptr [rdi + 0x128]
00000001803559BC test      rax, rax
00000001803559BF je        0x180355d13
00000001803559C5 cmp       byte ptr [rax + 0x95], r14b
00000001803559CC je        0x180355979
00000001803559CE xor       edx, edx
00000001803559D0 mov       rcx, rdi
00000001803559D3 call      0x18034f750
00000001803559D8 mov       qword ptr [rdi + 0x120], rax
00000001803559DF mov       rdx, rax
00000001803559E2 lea       rcx, [rdi + 0x120]
00000001803559E9 call      0x18024f360
00000001803559EE mov       rcx, qword ptr [rip + 0x185ab0b] # 0x181bb0500
00000001803559F5 mov       rsi, qword ptr [rdi + 0x120]
00000001803559FC cmp       dword ptr [rcx + 0xe0], 0
0000000180355A03 jne       0x180355a0a
0000000180355A05 call      0x1802501e0
0000000180355A0A xor       r8d, r8d
0000000180355A0D xor       edx, edx
0000000180355A0F mov       rcx, rsi
0000000180355A12 call      0x18131f900
0000000180355A17 xor       edx, edx
0000000180355A19 test      al, al
0000000180355A1B jne       0x180355a83
0000000180355A1D mov       rcx, rdi
0000000180355A20 call      0x18034eea0
0000000180355A25 mov       qword ptr [rdi + 0x128], rax
0000000180355A2C mov       rdx, rax
0000000180355A2F lea       rcx, [rdi + 0x128]
0000000180355A36 call      0x18024f360
0000000180355A3B mov       rcx, qword ptr [rip + 0x185aabe] # 0x181bb0500
0000000180355A42 cmp       dword ptr [rcx + 0xe0], 0
0000000180355A49 mov       rbx, qword ptr [rdi + 0x128]
0000000180355A50 jne       0x180355a57
0000000180355A52 call      0x1802501e0
0000000180355A57 xor       r8d, r8d
0000000180355A5A xor       edx, edx
0000000180355A5C mov       rcx, rbx
0000000180355A5F call      0x18131f900
0000000180355A64 test      al, al
0000000180355A66 je        0x180355bfc
0000000180355A6C mov       qword ptr [rdi + 0x120], 0
0000000180355A77 lea       rcx, [rdi + 0x120]
0000000180355A7E jmp       0x180355cfa
0000000180355A83 mov       qword ptr [rdi + 0x128], 0
0000000180355A8E lea       rcx, [rdi + 0x128]
0000000180355A95 jmp       0x180355cfc
0000000180355A9A mov       rax, qword ptr [rdi + 0x130]
0000000180355AA1 test      rax, rax
0000000180355AA4 je        0x180355d13
0000000180355AAA cmp       dword ptr [rax + 0x18], 0
0000000180355AAE jle       0x180355baa
0000000180355AB4 mov       r8, qword ptr [rip + 0x186dc2d] # 0x181bc36e8
0000000180355ABB mov       rdx, rax
0000000180355ABE lea       rcx, [rsp + 0x28]
0000000180355AC3 call      0x1808197f0
0000000180355AC8 movups    xmm0, xmmword ptr [rsp + 0x28]
0000000180355ACD movups    xmmword ptr [rsp + 0x40], xmm0
0000000180355AD2 movsd     xmm1, qword ptr [rsp + 0x38]
0000000180355AD8 movsd     qword ptr [rsp + 0x50], xmm1
0000000180355ADE mov       qword ptr [rsp + 0x28], 0
0000000180355AE7 lea       rbx, [rsp + 0x40]
0000000180355AEC mov       qword ptr [rsp + 0x30], rbx
0000000180355AF1 mov       rdx, qword ptr [rip + 0x1874dc0] # 0x181bca8b8
0000000180355AF8 lea       rcx, [rsp + 0x40]
0000000180355AFD call      0x180669280
0000000180355B02 test      al, al
0000000180355B04 je        0x180355b75
0000000180355B06 mov       rsi, qword ptr [rsp + 0x50]
0000000180355B0B mov       rcx, qword ptr [rip + 0x185a9ee] # 0x181bb0500
0000000180355B12 cmp       dword ptr [rcx + 0xe0], 0
0000000180355B19 jne       0x180355b20
0000000180355B1B call      0x1802501e0
0000000180355B20 xor       r8d, r8d
0000000180355B23 xor       edx, edx
0000000180355B25 mov       rcx, rsi
0000000180355B28 call      0x18131f760
0000000180355B2D test      al, al
0000000180355B2F jne       0x180355af1
0000000180355B31 test      rsi, rsi
0000000180355B34 je        0x180355d19
0000000180355B3A xor       edx, edx
0000000180355B3C mov       rcx, rsi
0000000180355B3F call      0x18035de80
0000000180355B44 test      al, al
0000000180355B46 je        0x180355af1
0000000180355B48 mov       qword ptr [rdi + 0x120], rsi
0000000180355B4F lea       rcx, [rdi + 0x120]
0000000180355B56 mov       rdx, rsi
0000000180355B59 call      0x18024f360
0000000180355B5E mov       r14b, 1
0000000180355B61 mov       rdx, qword ptr [rip + 0x1874d00] # 0x181bca868
0000000180355B68 mov       rcx, rbx
0000000180355B6B call      0x180302170
0000000180355B70 jmp       0x180355d04
0000000180355B75 mov       rdx, qword ptr [rip + 0x1874cec] # 0x181bca868
0000000180355B7C mov       rcx, rbx
0000000180355B7F call      0x180302170
0000000180355B84 jmp       0x180355bfc
0000000180355B86 mov       rdx, qword ptr [rip + 0x1874cdb] # 0x181bca868
0000000180355B8D mov       rcx, qword ptr [rsp + 0x30]
0000000180355B92 call      0x180302170
0000000180355B97 mov       rcx, qword ptr [rsp + 0x28]
0000000180355B9C test      rcx, rcx
0000000180355B9F jne       0x180355d1f
0000000180355BA5 mov       rdi, qword ptr [rsp + 0x70]
0000000180355BAA xor       edx, edx
0000000180355BAC mov       rcx, rdi
0000000180355BAF call      0x18034f750
0000000180355BB4 mov       qword ptr [rdi + 0x120], rax
0000000180355BBB lea       rcx, [rdi + 0x120]
0000000180355BC2 mov       rdx, rax
0000000180355BC5 call      0x18024f360
0000000180355BCA mov       rbx, qword ptr [rdi + 0x120]
0000000180355BD1 mov       rcx, qword ptr [rip + 0x185a928] # 0x181bb0500
0000000180355BD8 cmp       dword ptr [rcx + 0xe0], 0
0000000180355BDF jne       0x180355be6
0000000180355BE1 call      0x1802501e0
0000000180355BE6 xor       edx, edx
0000000180355BE8 mov       rcx, rbx
0000000180355BEB call      0x18131f870
0000000180355BF0 movzx     r14d, al
0000000180355BF4 test      al, al
0000000180355BF6 jne       0x180355d04
0000000180355BFC movzx     eax, r14b
0000000180355C00 lea       r11, [rsp + 0x60]
0000000180355C05 mov       rbx, qword ptr [r11 + 0x18]
0000000180355C09 mov       rsi, qword ptr [r11 + 0x20]
0000000180355C0D mov       rdi, qword ptr [r11 + 0x28]
0000000180355C11 mov       rsp, r11
0000000180355C14 pop       r14
0000000180355C16 ret       
0000000180355C17 cmp       byte ptr [rdi + 0xe8], al
0000000180355C1D je        0x180355c39
0000000180355C1F mov       rax, qword ptr [rdi + 0xe0]
0000000180355C26 test      rax, rax
0000000180355C29 je        0x180355d13
0000000180355C2F cmp       dword ptr [rax + 0x18], 2
0000000180355C33 je        0x180355979
0000000180355C39 mov       rax, qword ptr [rdi + 0xe0]
0000000180355C40 test      rax, rax
0000000180355C43 je        0x180355d13
0000000180355C49 cmp       dword ptr [rax + 0x18], 2
0000000180355C4D je        0x1803559ce
0000000180355C53 xor       al, al
0000000180355C55 jmp       0x180355c00
0000000180355C57 cmp       byte ptr [rdi + 0x205], al
0000000180355C5D jne       0x180355c00
0000000180355C5F jmp       0x1803559ce
0000000180355C64 cmp       dword ptr [rdi + 0x90], 1
0000000180355C6B jmp       0x180355c5d
0000000180355C6D cmp       edx, 0x31
0000000180355C70 jne       0x180355bfc
0000000180355C72 cmp       byte ptr [rdi + 0xe8], al
0000000180355C78 je        0x180355bfc
0000000180355C7A mov       rax, qword ptr [rdi + 0xe0]
0000000180355C81 test      rax, rax
0000000180355C84 je        0x180355d13
0000000180355C8A cmp       dword ptr [rax + 0x18], 1
0000000180355C8E je        0x180355bfc
0000000180355C94 xor       edx, edx
0000000180355C96 mov       rcx, rdi
0000000180355C99 call      0x18034f750
0000000180355C9E mov       qword ptr [rdi + 0x120], rax
0000000180355CA5 lea       rcx, [rdi + 0x120]
0000000180355CAC mov       rdx, rax
0000000180355CAF call      0x18024f360
0000000180355CB4 mov       rsi, qword ptr [rdi + 0x120]
0000000180355CBB mov       rcx, qword ptr [rip + 0x185a83e] # 0x181bb0500
0000000180355CC2 cmp       dword ptr [rcx + 0xe0], 0
0000000180355CC9 jne       0x180355cd0
0000000180355CCB call      0x1802501e0
0000000180355CD0 xor       r8d, r8d
0000000180355CD3 xor       edx, edx
0000000180355CD5 mov       rcx, rsi
0000000180355CD8 call      0x18131f900
0000000180355CDD test      al, al
0000000180355CDF jne       0x180355ce8
0000000180355CE1 xor       edx, edx
0000000180355CE3 jmp       0x180355a1d
0000000180355CE8 mov       qword ptr [rdi + 0x128], 0
0000000180355CF3 lea       rcx, [rdi + 0x128]
0000000180355CFA xor       edx, edx
0000000180355CFC mov       r14b, 1
0000000180355CFF call      0x18024f360
0000000180355D04 xor       edx, edx
0000000180355D06 mov       rcx, rdi
0000000180355D09 call      0x180355d50
0000000180355D0E jmp       0x180355bfc
0000000180355D13 call      0x180250150
0000000180355D18 nop       
0000000180355D19 call      0x180250150
0000000180355D1E nop       
0000000180355D1F call      0x180200e60
0000000180355D24 int3      
0000000180355D25 nop       dword ptr [rax]
0000000180355D28 sub       byte ptr [rcx + 0x35], bl
0000000180355D2B add       ah, bh
0000000180355D2D pop       rbx
0000000180355D2E xor       eax, 0x355bfc00
0000000180355D33 add       byte ptr [rdx + 0x1700355a], bl
0000000180355D39 pop       rsp
0000000180355D3A xor       eax, 0x355c5700
0000000180355D3F add       byte ptr [rdi + 0x5c], dl
0000000180355D42 xor       eax, 0x355c6400
0000000180355D47 add       ah, cl
0000000180355D49 int3      
0000000180355D4A int3      
0000000180355D4B int3      
0000000180355D4C int3      
0000000180355D4D int3      
0000000180355D4E int3      
0000000180355D4F int3      