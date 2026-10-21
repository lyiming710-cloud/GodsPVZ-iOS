000000018034AFE0 push      rdi
000000018034AFE2 sub       rsp, 0x20
000000018034AFE6 cmp       byte ptr [rip + 0x1999ccc], 0 # 0x181ce4cb9
000000018034AFED mov       rdi, rcx
000000018034AFF0 jne       0x18034b059
000000018034AFF2 lea       rcx, [rip + 0x185701f] # 0x181ba2018
000000018034AFF9 call      0x18024fef0
000000018034AFFE lea       rcx, [rip + 0x188155b] # 0x181bcc560
000000018034B005 call      0x18024fef0
000000018034B00A lea       rcx, [rip + 0x18786c7] # 0x181bc36d8
000000018034B011 call      0x18024fef0
000000018034B016 lea       rcx, [rip + 0x1878663] # 0x181bc3680
000000018034B01D call      0x18024fef0
000000018034B022 lea       rcx, [rip + 0x1881adf] # 0x181bccb08
000000018034B029 call      0x18024fef0
000000018034B02E lea       rcx, [rip + 0x185ff33] # 0x181baaf68
000000018034B035 call      0x18024fef0
000000018034B03A lea       rcx, [rip + 0x185fe77] # 0x181baaeb8
000000018034B041 call      0x18024fef0
000000018034B046 lea       rcx, [rip + 0x1881b13] # 0x181bccb60
000000018034B04D call      0x18024fef0
000000018034B052 mov       byte ptr [rip + 0x1999c60], 1 # 0x181ce4cb9
000000018034B059 cmp       dword ptr [rdi + 0x5c], 3
000000018034B05D jne       0x18034b267
000000018034B063 cmp       qword ptr [rdi + 0x60], 0
000000018034B068 je        0x18034b267
000000018034B06E mov       rcx, qword ptr [rip + 0x18814eb] # 0x181bcc560
000000018034B075 mov       edx, 6
000000018034B07A mov       qword ptr [rsp + 0x30], rbx
000000018034B07F mov       qword ptr [rsp + 0x38], rbp
000000018034B084 mov       qword ptr [rsp + 0x40], rsi
000000018034B089 call      0x18024f3e0
000000018034B08E mov       rbx, rax
000000018034B091 test      rax, rax
000000018034B094 je        0x18034b2e5
000000018034B09A cmp       dword ptr [rax + 0x18], 0
000000018034B09E mov       rdx, qword ptr [rip + 0x185fe13] # 0x181baaeb8
000000018034B0A5 jbe       0x18034b2eb
000000018034B0AB lea       rcx, [rax + 0x20]
000000018034B0AF mov       qword ptr [rax + 0x20], rdx
000000018034B0B3 call      0x18024f360
000000018034B0B8 cmp       dword ptr [rbx + 0x18], 1
000000018034B0BC mov       rdx, qword ptr [rip + 0x185fea5] # 0x181baaf68
000000018034B0C3 jbe       0x18034b2eb
000000018034B0C9 lea       rcx, [rbx + 0x28]
000000018034B0CD mov       qword ptr [rbx + 0x28], rdx
000000018034B0D1 call      0x18024f360
000000018034B0D6 cmp       dword ptr [rbx + 0x18], 2
000000018034B0DA mov       rdx, qword ptr [rip + 0x1881a27] # 0x181bccb08
000000018034B0E1 jbe       0x18034b2eb
000000018034B0E7 lea       rcx, [rbx + 0x30]
000000018034B0EB mov       qword ptr [rbx + 0x30], rdx
000000018034B0EF call      0x18024f360
000000018034B0F4 cmp       dword ptr [rbx + 0x18], 3
000000018034B0F8 mov       rdx, qword ptr [rip + 0x1881a61] # 0x181bccb60
000000018034B0FF jbe       0x18034b2eb
000000018034B105 lea       rcx, [rbx + 0x38]
000000018034B109 mov       qword ptr [rbx + 0x38], rdx
000000018034B10D call      0x18024f360
000000018034B112 cmp       dword ptr [rbx + 0x18], 4
000000018034B116 mov       rdx, qword ptr [rip + 0x1878563] # 0x181bc3680
000000018034B11D jbe       0x18034b2eb
000000018034B123 lea       rcx, [rbx + 0x40]
000000018034B127 mov       qword ptr [rbx + 0x40], rdx
000000018034B12B call      0x18024f360
000000018034B130 cmp       dword ptr [rbx + 0x18], 5
000000018034B134 mov       rdx, qword ptr [rip + 0x187859d] # 0x181bc36d8
000000018034B13B jbe       0x18034b2eb
000000018034B141 lea       rcx, [rbx + 0x48]
000000018034B145 mov       qword ptr [rbx + 0x48], rdx
000000018034B149 call      0x18024f360
000000018034B14E mov       rax, qword ptr [rdi + 0x60]
000000018034B152 test      rax, rax
000000018034B155 je        0x18034b2e5
000000018034B15B mov       eax, dword ptr [rax + 0x10]
000000018034B15E dec       eax
000000018034B160 cmp       eax, 7
000000018034B163 ja        0x18034b26d
000000018034B169 lea       rdx, [rip - 0x34b170] # 0x180000000
000000018034B170 cdqe      
000000018034B172 mov       ecx, dword ptr [rdx + rax*4 + 0x34b2f4]
000000018034B179 add       rcx, rdx
000000018034B17C jmp       rcx
000000018034B17E cmp       dword ptr [rbx + 0x18], 0
000000018034B182 mov       rsi, qword ptr [rdi + 0xd8]
000000018034B189 jbe       0x18034b2eb
000000018034B18F mov       rcx, qword ptr [rip + 0x1856e82] # 0x181ba2018
000000018034B196 mov       rbp, qword ptr [rbx + 0x20]
000000018034B19A cmp       dword ptr [rcx + 0xe0], 0
000000018034B1A1 jne       0x18034b1a8
000000018034B1A3 call      0x1802501e0
000000018034B1A8 xor       r8d, r8d
000000018034B1AB mov       rdx, rbp
000000018034B1AE mov       rcx, rsi
000000018034B1B1 call      0x18031bac0
000000018034B1B6 cmp       dword ptr [rbx + 0x18], 1
000000018034B1BA jbe       0x18034b2eb
000000018034B1C0 mov       rdx, qword ptr [rbx + 0x28]
000000018034B1C4 jmp       0x18034b2ab
000000018034B1C9 cmp       dword ptr [rbx + 0x18], 0
000000018034B1CD mov       rsi, qword ptr [rdi + 0xd8]
000000018034B1D4 jbe       0x18034b2eb
000000018034B1DA mov       rcx, qword ptr [rip + 0x1856e37] # 0x181ba2018
000000018034B1E1 mov       rbp, qword ptr [rbx + 0x20]
000000018034B1E5 cmp       dword ptr [rcx + 0xe0], 0
000000018034B1EC jne       0x18034b1f3
000000018034B1EE call      0x1802501e0
000000018034B1F3 xor       r8d, r8d
000000018034B1F6 mov       rdx, rbp
000000018034B1F9 mov       rcx, rsi
000000018034B1FC call      0x18031bac0
000000018034B201 cmp       dword ptr [rbx + 0x18], 1
000000018034B205 jbe       0x18034b2eb
000000018034B20B mov       rdx, qword ptr [rbx + 0x28]
000000018034B20F xor       r8d, r8d
000000018034B212 mov       rcx, qword ptr [rdi + 0xd8]
000000018034B219 call      0x18031bac0
000000018034B21E cmp       dword ptr [rbx + 0x18], 2
000000018034B222 jbe       0x18034b2eb
000000018034B228 mov       rdx, qword ptr [rbx + 0x30]
000000018034B22C xor       r8d, r8d
000000018034B22F mov       rcx, qword ptr [rdi + 0xd8]
000000018034B236 call      0x18031bac0
000000018034B23B cmp       dword ptr [rbx + 0x18], 3
000000018034B23F jbe       0x18034b2eb
000000018034B245 mov       rdx, qword ptr [rbx + 0x38]
000000018034B249 xor       r8d, r8d
000000018034B24C mov       rcx, qword ptr [rdi + 0xd8]
000000018034B253 call      0x18031bac0
000000018034B258 mov       rbp, qword ptr [rsp + 0x38]
000000018034B25D mov       rsi, qword ptr [rsp + 0x40]
000000018034B262 mov       rbx, qword ptr [rsp + 0x30]
000000018034B267 add       rsp, 0x20
000000018034B26B pop       rdi
000000018034B26C ret       
000000018034B26D cmp       dword ptr [rbx + 0x18], 2
000000018034B271 mov       rsi, qword ptr [rdi + 0xd8]
000000018034B278 jbe       0x18034b2eb
000000018034B27A mov       rcx, qword ptr [rip + 0x1856d97] # 0x181ba2018
000000018034B281 mov       rbp, qword ptr [rbx + 0x30]
000000018034B285 cmp       dword ptr [rcx + 0xe0], 0
000000018034B28C jne       0x18034b293
000000018034B28E call      0x1802501e0
000000018034B293 xor       r8d, r8d
000000018034B296 mov       rdx, rbp
000000018034B299 mov       rcx, rsi
000000018034B29C call      0x18031bac0
000000018034B2A1 cmp       dword ptr [rbx + 0x18], 3
000000018034B2A5 jbe       0x18034b2eb
000000018034B2A7 mov       rdx, qword ptr [rbx + 0x38]
000000018034B2AB mov       rcx, qword ptr [rdi + 0xd8]
000000018034B2B2 xor       r8d, r8d
000000018034B2B5 call      0x18031bac0
000000018034B2BA cmp       dword ptr [rbx + 0x18], 4
000000018034B2BE jbe       0x18034b2eb
000000018034B2C0 mov       rdx, qword ptr [rbx + 0x40]
000000018034B2C4 xor       r8d, r8d
000000018034B2C7 mov       rcx, qword ptr [rdi + 0xd8]
000000018034B2CE call      0x18031bac0
000000018034B2D3 cmp       dword ptr [rbx + 0x18], 5
000000018034B2D7 jbe       0x18034b2eb
000000018034B2D9 mov       rdx, qword ptr [rbx + 0x48]
000000018034B2DD xor       r8d, r8d
000000018034B2E0 jmp       0x18034b24c
000000018034B2E5 call      0x180250150
000000018034B2EA int3      
000000018034B2EB call      0x180250140
000000018034B2F0 int3      
000000018034B2F1 nop       dword ptr [rax]
000000018034B2F4 pop       rax
000000018034B2F5 mov       dl, 0x34
000000018034B2F7 add       byte ptr [rsi - 0x4f], bh
000000018034B2FA xor       al, 0
000000018034B2FC pop       rax
000000018034B2FD mov       dl, 0x34
000000018034B2FF add       byte ptr [rax - 0x4e], bl
000000018034B302 xor       al, 0
000000018034B304 insd      dword ptr [rdi], dx
000000018034B305 mov       dl, 0x34
000000018034B307 add       byte ptr [rbp - 0x4e], ch
000000018034B30A xor       al, 0
000000018034B30C insd      dword ptr [rdi], dx
000000018034B30D mov       dl, 0x34
000000018034B30F add       cl, cl
000000018034B311 mov       cl, 0x34