000000018038AFD0 mov       qword ptr [rsp + 0x10], rbx
000000018038AFD5 mov       qword ptr [rsp + 0x18], rsi
000000018038AFDA mov       qword ptr [rsp + 8], rcx
000000018038AFDF push      rdi
000000018038AFE0 sub       rsp, 0x80
000000018038AFE7 mov       rdi, rcx
000000018038AFEA cmp       byte ptr [rip + 0x1959e4c], 0 # 0x181ce4e3d
000000018038AFF1 jne       0x18038b042
000000018038AFF3 lea       rcx, [rip + 0x1843786] # 0x181bce780
000000018038AFFA call      0x18024fef0
000000018038AFFF lea       rcx, [rip + 0x183e3b2] # 0x181bc93b8
000000018038B006 call      0x18024fef0
000000018038B00B lea       rcx, [rip + 0x183e3fe] # 0x181bc9410
000000018038B012 call      0x18024fef0
000000018038B017 lea       rcx, [rip + 0x183e442] # 0x181bc9460
000000018038B01E call      0x18024fef0
000000018038B023 lea       rcx, [rip + 0x1816fee] # 0x181ba2018
000000018038B02A call      0x18024fef0
000000018038B02F lea       rcx, [rip + 0x18325ea] # 0x181bbd620
000000018038B036 call      0x18024fef0
000000018038B03B mov       byte ptr [rip + 0x1959dfb], 1 # 0x181ce4e3d
000000018038B042 mov       rdx, qword ptr [rdi + 0x168]
000000018038B049 test      rdx, rdx
000000018038B04C je        0x18038b31e
000000018038B052 mov       r8, qword ptr [rip + 0x18325c7] # 0x181bbd620
000000018038B059 lea       rcx, [rsp + 0x40]
000000018038B05E call      0x1808197f0
000000018038B063 movups    xmm0, xmmword ptr [rsp + 0x40]
000000018038B068 movups    xmmword ptr [rsp + 0x60], xmm0
000000018038B06D movsd     xmm1, qword ptr [rsp + 0x50]
000000018038B073 movsd     qword ptr [rsp + 0x70], xmm1
000000018038B079 mov       qword ptr [rsp + 0x40], 0
000000018038B082 lea       rbx, [rsp + 0x60]
000000018038B087 mov       qword ptr [rsp + 0x48], rbx
000000018038B08C nop       dword ptr [rax]
000000018038B090 mov       rdx, qword ptr [rip + 0x183e379] # 0x181bc9410
000000018038B097 lea       rcx, [rsp + 0x60]
000000018038B09C call      0x180669280
000000018038B0A1 test      al, al
000000018038B0A3 je        0x18038b0cb
000000018038B0A5 mov       rcx, qword ptr [rsp + 0x70]
000000018038B0AA test      rcx, rcx
000000018038B0AD je        0x18038b32a
000000018038B0B3 mov       rdx, qword ptr [rcx + 0x20]
000000018038B0B7 test      rdx, rdx
000000018038B0BA je        0x18038b324
000000018038B0C0 mov       eax, dword ptr [rdx + 0x20]
000000018038B0C3 sub       eax, dword ptr [rcx + 0x28]
000000018038B0C6 mov       dword ptr [rdx + 0x20], eax
000000018038B0C9 jmp       0x18038b090
000000018038B0CB mov       rdx, qword ptr [rip + 0x183e2e6] # 0x181bc93b8
000000018038B0D2 mov       rcx, rbx
000000018038B0D5 call      0x180302170
000000018038B0DA jmp       0x18038b103
000000018038B0DC mov       rdx, qword ptr [rip + 0x183e2d5] # 0x181bc93b8
000000018038B0E3 mov       rcx, qword ptr [rsp + 0x48]
000000018038B0E8 call      0x180302170
000000018038B0ED mov       rcx, qword ptr [rsp + 0x40]
000000018038B0F2 test      rcx, rcx
000000018038B0F5 jne       0x18038b330
000000018038B0FB mov       rdi, qword ptr [rsp + 0x90]
000000018038B103 mov       rax, qword ptr [rip + 0x1816f0e] # 0x181ba2018
000000018038B10A cmp       dword ptr [rax + 0xe0], 0
000000018038B111 jne       0x18038b122
000000018038B113 mov       rcx, rax
000000018038B116 call      0x1802501e0
000000018038B11B mov       rax, qword ptr [rip + 0x1816ef6] # 0x181ba2018
000000018038B122 mov       rax, qword ptr [rax + 0xb8]
000000018038B129 mov       rcx, qword ptr [rax]
000000018038B12C test      rcx, rcx
000000018038B12F je        0x18038b31e
000000018038B135 mov       rcx, qword ptr [rcx + 0x28]
000000018038B139 test      rcx, rcx
000000018038B13C je        0x18038b31e
000000018038B142 mov       rcx, qword ptr [rcx + 0x10]
000000018038B146 test      rcx, rcx
000000018038B149 je        0x18038b31e
000000018038B14F mov       eax, dword ptr [rcx + 0x70]
000000018038B152 sub       eax, dword ptr [rdi + 0x188]
000000018038B158 mov       dword ptr [rcx + 0x70], eax
000000018038B15B mov       rbx, qword ptr [rdi + 0x20]
000000018038B15F mov       rdx, qword ptr [rip + 0x184361a] # 0x181bce780
000000018038B166 cmp       dword ptr [rdx + 0xe0], 0
000000018038B16D jne       0x18038b17e
000000018038B16F mov       rcx, rdx
000000018038B172 call      0x1802501e0
000000018038B177 mov       rdx, qword ptr [rip + 0x1843602] # 0x181bce780
000000018038B17E test      rbx, rbx
000000018038B181 je        0x18038b31e
000000018038B187 mov       rdx, qword ptr [rdx + 0xb8]
000000018038B18E xor       r9d, r9d
000000018038B191 xor       r8d, r8d
000000018038B194 mov       edx, dword ptr [rdx + 4]
000000018038B197 mov       rcx, rbx
000000018038B19A call      0x1812db9a0
000000018038B19F mov       rbx, qword ptr [rdi + 0x80]
000000018038B1A6 test      rbx, rbx
000000018038B1A9 je        0x18038b31e
000000018038B1AF cmp       byte ptr [rip + 0x1959c9e], 0 # 0x181ce4e54
000000018038B1B6 jne       0x18038b1cb
000000018038B1B8 lea       rcx, [rip + 0x1816e59] # 0x181ba2018
000000018038B1BF call      0x18024fef0
000000018038B1C4 mov       byte ptr [rip + 0x1959c89], 1 # 0x181ce4e54
000000018038B1CB mov       rcx, qword ptr [rbx + 0x30]
000000018038B1CF test      rcx, rcx
000000018038B1D2 je        0x18038b31e
000000018038B1D8 xor       r8d, r8d
000000018038B1DB mov       edx, dword ptr [rbx + 0x94]
000000018038B1E1 call      0x180375f80
000000018038B1E6 mov       rsi, rax
000000018038B1E9 mov       rdx, qword ptr [rbx + 0x30]
000000018038B1ED test      rdx, rdx
000000018038B1F0 je        0x18038b31e
000000018038B1F6 inc       dword ptr [rdx + 0x5c]
000000018038B1F9 mov       rax, qword ptr [rip + 0x1816e18] # 0x181ba2018
000000018038B200 cmp       dword ptr [rax + 0xe0], 0
000000018038B207 jne       0x18038b218
000000018038B209 mov       rcx, rax
000000018038B20C call      0x1802501e0
000000018038B211 mov       rax, qword ptr [rip + 0x1816e00] # 0x181ba2018
000000018038B218 mov       rax, qword ptr [rax + 0xb8]
000000018038B21F mov       rcx, qword ptr [rax]
000000018038B222 test      rcx, rcx
000000018038B225 je        0x18038b31e
000000018038B22B mov       rcx, qword ptr [rcx + 0x28]
000000018038B22F mov       r8, qword ptr [rbx + 0x30]
000000018038B233 test      r8, r8
000000018038B236 je        0x18038b31e
000000018038B23C test      rcx, rcx
000000018038B23F je        0x18038b31e
000000018038B245 xor       r9d, r9d
000000018038B248 mov       r8d, dword ptr [r8 + 0x5c]
000000018038B24C mov       rdx, qword ptr [rbx + 0x30]
000000018038B250 call      0x18033e790
000000018038B255 movdqa    xmm0, xmmword ptr [rip + 0x121ccd3] # 0x1815a7f30
000000018038B25D movaps    xmmword ptr [rsp + 0x40], xmm0
000000018038B262 xor       r9d, r9d
000000018038B265 lea       r8, [rsp + 0x40]
000000018038B26A mov       rdx, rsi
000000018038B26D mov       rcx, rbx
000000018038B270 call      0x180390420
000000018038B275 mov       rcx, qword ptr [rbx + 0x80]
000000018038B27C mov       rdx, qword ptr [rbx + 0x30]
000000018038B280 test      rdx, rdx
000000018038B283 je        0x18038b31e
000000018038B289 test      rcx, rcx
000000018038B28C je        0x18038b31e
000000018038B292 mov       qword ptr [rsp + 0x28], 0
000000018038B29B mov       byte ptr [rsp + 0x20], 1
000000018038B2A0 mov       r9d, dword ptr [rdx + 0x44]
000000018038B2A4 mov       r8d, dword ptr [rdx + 0x54]
000000018038B2A8 mov       edx, dword ptr [rdx + 0x5c]
000000018038B2AB call      0x18039af40
000000018038B2B0 mov       rcx, qword ptr [rdi + 0xd8]
000000018038B2B7 test      rcx, rcx
000000018038B2BA je        0x18038b31e
000000018038B2BC xor       r8d, r8d
000000018038B2BF mov       dl, 1
000000018038B2C1 call      0x18131bad0
000000018038B2C6 mov       rbx, qword ptr [rdi + 0x148]
000000018038B2CD mov       rcx, qword ptr [rdi + 0x60]
000000018038B2D1 test      rcx, rcx
000000018038B2D4 je        0x18038b31e
000000018038B2D6 xor       edx, edx
000000018038B2D8 call      0x1803761e0
000000018038B2DD test      rbx, rbx
000000018038B2E0 je        0x18038b31e
000000018038B2E2 xor       al, 1
000000018038B2E4 xor       r8d, r8d
000000018038B2E7 movzx     edx, al
000000018038B2EA mov       rcx, rbx
000000018038B2ED call      0x18156cc70
000000018038B2F2 mov       rdx, qword ptr [rdi + 0x60]
000000018038B2F6 test      rdx, rdx
000000018038B2F9 je        0x18038b31e
000000018038B2FB xor       r8d, r8d
000000018038B2FE mov       edx, dword ptr [rdx + 0x5c]
000000018038B301 mov       rcx, rdi
000000018038B304 call      0x18038ef10
000000018038B309 lea       r11, [rsp + 0x80]
000000018038B311 mov       rbx, qword ptr [r11 + 0x18]
000000018038B315 mov       rsi, qword ptr [r11 + 0x20]
000000018038B319 mov       rsp, r11
000000018038B31C pop       rdi
000000018038B31D ret       
000000018038B31E call      0x180250150
000000018038B323 nop       
000000018038B324 call      0x180250150
000000018038B329 nop       
000000018038B32A call      0x180250150
000000018038B32F nop       
000000018038B330 call      0x180200e60
000000018038B335 int3      
000000018038B336 int3      
000000018038B337 int3      
000000018038B338 int3      
000000018038B339 int3      
000000018038B33A int3      
000000018038B33B int3      
000000018038B33C int3      
000000018038B33D int3      
000000018038B33E int3      
000000018038B33F int3      