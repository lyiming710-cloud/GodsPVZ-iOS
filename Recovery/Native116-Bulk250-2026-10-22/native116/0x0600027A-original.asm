0000000180341990 mov       qword ptr [rsp + 0x10], rbx
0000000180341995 push      rdi
0000000180341996 sub       rsp, 0x30
000000018034199A cmp       byte ptr [rip + 0x19a32be], 0 # 0x181ce4c5f
00000001803419A1 mov       ebx, edx
00000001803419A3 mov       rdi, rcx
00000001803419A6 jne       0x1803419eb
00000001803419A8 lea       rcx, [rip + 0x1859399] # 0x181b9ad48
00000001803419AF call      0x18024fef0
00000001803419B4 lea       rcx, [rip + 0x1881e8d] # 0x181bc3848
00000001803419BB call      0x18024fef0
00000001803419C0 lea       rcx, [rip + 0x1876b29] # 0x181bb84f0
00000001803419C7 call      0x18024fef0
00000001803419CC lea       rcx, [rip + 0x186eb2d] # 0x181bb0500
00000001803419D3 call      0x18024fef0
00000001803419D8 lea       rcx, [rip + 0x185eb71] # 0x181ba0550
00000001803419DF call      0x18024fef0
00000001803419E4 mov       byte ptr [rip + 0x19a3274], 1 # 0x181ce4c5f
00000001803419EB mov       rcx, qword ptr [rdi + 0x30]
00000001803419EF mov       qword ptr [rsp + 0x40], rsi
00000001803419F4 test      rcx, rcx
00000001803419F7 je        0x180341c5b
00000001803419FD mov       r8, qword ptr [rip + 0x1881e44] # 0x181bc3848
0000000180341A04 mov       edx, ebx
0000000180341A06 call      0x180825710
0000000180341A0B mov       rcx, qword ptr [rip + 0x186eaee] # 0x181bb0500
0000000180341A12 mov       rsi, rax
0000000180341A15 cmp       dword ptr [rcx + 0xe0], 0
0000000180341A1C jne       0x180341a23
0000000180341A1E call      0x1802501e0
0000000180341A23 xor       r8d, r8d
0000000180341A26 xor       edx, edx
0000000180341A28 mov       rcx, rsi
0000000180341A2B call      0x18131f760
0000000180341A30 test      al, al
0000000180341A32 jne       0x180341c26
0000000180341A38 mov       rcx, qword ptr [rdi + 0x30]
0000000180341A3C test      rcx, rcx
0000000180341A3F je        0x180341c5b
0000000180341A45 mov       r8, qword ptr [rip + 0x1881dfc] # 0x181bc3848
0000000180341A4C mov       edx, ebx
0000000180341A4E call      0x180825710
0000000180341A53 mov       rcx, qword ptr [rip + 0x186eaa6] # 0x181bb0500
0000000180341A5A mov       rbx, rax
0000000180341A5D cmp       dword ptr [rcx + 0xe0], 0
0000000180341A64 jne       0x180341a6b
0000000180341A66 call      0x1802501e0
0000000180341A6B mov       rdx, qword ptr [rip + 0x1876a7e] # 0x181bb84f0
0000000180341A72 mov       rcx, rbx
0000000180341A75 call      0x1804a0690
0000000180341A7A mov       rbx, rax
0000000180341A7D test      rax, rax
0000000180341A80 je        0x180341c5b
0000000180341A86 mov       byte ptr [rax + 0xbd], 1
0000000180341A8D lea       rcx, [rax + 0x20]
0000000180341A91 mov       rdx, qword ptr [rdi + 0x20]
0000000180341A95 mov       qword ptr [rax + 0x20], rdx
0000000180341A99 call      0x18024f360
0000000180341A9E xor       edx, edx
0000000180341AA0 mov       rcx, rbx
0000000180341AA3 call      0x1813044d0
0000000180341AA8 test      rax, rax
0000000180341AAB je        0x180341c5b
0000000180341AB1 xor       edx, edx
0000000180341AB3 mov       rcx, rax
0000000180341AB6 call      0x18131c070
0000000180341ABB xorps     xmm1, xmm1
0000000180341ABE test      rax, rax
0000000180341AC1 je        0x180341c5b
0000000180341AC7 xorps     xmm0, xmm0
0000000180341ACA movss     dword ptr [rsp + 0x28], xmm1
0000000180341AD0 unpcklps  xmm0, xmm1
0000000180341AD3 lea       rdx, [rsp + 0x20]
0000000180341AD8 xor       r8d, r8d
0000000180341ADB movsd     qword ptr [rsp + 0x20], xmm0
0000000180341AE1 mov       rcx, rax
0000000180341AE4 call      0x18132fbb0
0000000180341AE9 xor       edx, edx
0000000180341AEB mov       rcx, rbx
0000000180341AEE call      0x1813044d0
0000000180341AF3 test      rax, rax
0000000180341AF6 je        0x180341c5b
0000000180341AFC xor       edx, edx
0000000180341AFE mov       rcx, rax
0000000180341B01 call      0x18131c070
0000000180341B06 xor       edx, edx
0000000180341B08 mov       rcx, rdi
0000000180341B0B mov       rsi, rax
0000000180341B0E call      0x181304510
0000000180341B13 test      rsi, rsi
0000000180341B16 je        0x180341c5b
0000000180341B1C xor       r9d, r9d
0000000180341B1F mov       r8b, 1
0000000180341B22 mov       rdx, rax
0000000180341B25 mov       rcx, rsi
0000000180341B28 call      0x18132e900
0000000180341B2D cmp       byte ptr [rip + 0x19a3126], 0 # 0x181ce4c5a
0000000180341B34 mov       dword ptr [rbx + 0x114], 0x3c23d70a
0000000180341B3E jne       0x180341b53
0000000180341B40 lea       rcx, [rip + 0x1881a99] # 0x181bc35e0
0000000180341B47 call      0x18024fef0
0000000180341B4C mov       byte ptr [rip + 0x19a3107], 1 # 0x181ce4c5a
0000000180341B53 mov       rcx, qword ptr [rdi + 0x38]
0000000180341B57 test      rcx, rcx
0000000180341B5A je        0x180341c5b
0000000180341B60 mov       r9, qword ptr [rip + 0x1881a79] # 0x181bc35e0
0000000180341B67 inc       dword ptr [rcx + 0x1c]
0000000180341B6A mov       rdx, qword ptr [rcx + 0x10]
0000000180341B6E movsxd    r8, dword ptr [rcx + 0x18]
0000000180341B72 test      rdx, rdx
0000000180341B75 je        0x180341c5b
0000000180341B7B cmp       r8d, dword ptr [rdx + 0x18]
0000000180341B7F jb        0x180341b9a
0000000180341B81 mov       rax, qword ptr [r9 + 0x20]
0000000180341B85 mov       rdx, rbx
0000000180341B88 mov       r8, qword ptr [rax + 0xc0]
0000000180341B8F mov       r8, qword ptr [r8 + 0x70]
0000000180341B93 call      0x180847ce0
0000000180341B98 jmp       0x180341bc0
0000000180341B9A lea       eax, [r8 + 1]
0000000180341B9E mov       dword ptr [rcx + 0x18], eax
0000000180341BA1 cmp       r8d, dword ptr [rdx + 0x18]
0000000180341BA5 jae       0x180341c61
0000000180341BAB mov       qword ptr [rdx + r8*8 + 0x20], rbx
0000000180341BB0 add       rdx, 0x20
0000000180341BB4 lea       rcx, [rdx + r8*8]
0000000180341BB8 mov       rdx, rbx
0000000180341BBB call      0x18024f360
0000000180341BC0 mov       rdx, qword ptr [rbx + 0x1f0]
0000000180341BC7 test      rdx, rdx
0000000180341BCA je        0x180341c5b
0000000180341BD0 xor       r8d, r8d
0000000180341BD3 lea       rcx, [rsp + 0x20]
0000000180341BD8 call      0x18132f3a0
0000000180341BDD mov       rdx, qword ptr [rbx + 0x1f0]
0000000180341BE4 movss     xmm0, dword ptr [rax]
0000000180341BE8 movss     dword ptr [rbx + 0x30], xmm0
0000000180341BED test      rdx, rdx
0000000180341BF0 je        0x180341c5b
0000000180341BF2 xor       r8d, r8d
0000000180341BF5 lea       rcx, [rsp + 0x20]
0000000180341BFA call      0x18132f3a0
0000000180341BFF xor       edx, edx
0000000180341C01 mov       rcx, rbx
0000000180341C04 movss     xmm0, dword ptr [rax + 4]
0000000180341C09 movss     dword ptr [rbx + 0x34], xmm0
0000000180341C0E call      0x180367440
0000000180341C13 mov       rax, rbx
0000000180341C16 mov       rsi, qword ptr [rsp + 0x40]
0000000180341C1B mov       rbx, qword ptr [rsp + 0x48]
0000000180341C20 add       rsp, 0x30
0000000180341C24 pop       rdi
0000000180341C25 ret       
0000000180341C26 mov       rcx, qword ptr [rip + 0x185911b] # 0x181b9ad48
0000000180341C2D cmp       dword ptr [rcx + 0xe0], 0
0000000180341C34 jne       0x180341c3b
0000000180341C36 call      0x1802501e0
0000000180341C3B mov       rcx, qword ptr [rip + 0x185e90e] # 0x181ba0550
0000000180341C42 xor       edx, edx
0000000180341C44 call      0x1812e68f0
0000000180341C49 mov       rsi, qword ptr [rsp + 0x40]
0000000180341C4E xor       eax, eax
0000000180341C50 mov       rbx, qword ptr [rsp + 0x48]
0000000180341C55 add       rsp, 0x30
0000000180341C59 pop       rdi
0000000180341C5A ret       
0000000180341C5B call      0x180250150
0000000180341C60 int3      
0000000180341C61 call      0x180250140
0000000180341C66 int3      
0000000180341C67 int3      
0000000180341C68 int3      
0000000180341C69 int3      
0000000180341C6A int3      
0000000180341C6B int3      
0000000180341C6C int3      
0000000180341C6D int3      
0000000180341C6E int3      
0000000180341C6F int3      