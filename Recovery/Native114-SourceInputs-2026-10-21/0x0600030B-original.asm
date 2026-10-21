000000018034BFD0 push      rbx
000000018034BFD2 sub       rsp, 0x60
000000018034BFD6 cmp       byte ptr [rip + 0x1998ce1], 0 # 0x181ce4cbe
000000018034BFDD mov       rbx, rcx
000000018034BFE0 jne       0x18034bff5
000000018034BFE2 lea       rcx, [rip + 0x1864517] # 0x181bb0500
000000018034BFE9 call      0x18024fef0
000000018034BFEE mov       byte ptr [rip + 0x1998cc9], 1 # 0x181ce4cbe
000000018034BFF5 cmp       byte ptr [rbx + 0x94], 0
000000018034BFFC je        0x18034c3c5
000000018034C002 mov       qword ptr [rsp + 0x70], rdi
000000018034C007 mov       rcx, qword ptr [rip + 0x18644f2] # 0x181bb0500
000000018034C00E mov       rdi, qword ptr [rbx + 0x28]
000000018034C012 cmp       dword ptr [rcx + 0xe0], 0
000000018034C019 jne       0x18034c020
000000018034C01B call      0x1802501e0
000000018034C020 xor       r8d, r8d
000000018034C023 xor       edx, edx
000000018034C025 mov       rcx, rdi
000000018034C028 call      0x18131f760
000000018034C02D test      al, al
000000018034C02F jne       0x18034c3c0
000000018034C035 movaps    xmmword ptr [rsp + 0x50], xmm6
000000018034C03A movaps    xmmword ptr [rsp + 0x40], xmm7
000000018034C03F mov       rax, qword ptr [rbx + 0x28]
000000018034C043 test      rax, rax
000000018034C046 je        0x18034c3d7
000000018034C04C cmp       byte ptr [rax + 0x41], 0
000000018034C050 je        0x18034c3b6
000000018034C056 cmp       dword ptr [rbx + 0x5c], 2
000000018034C05A jne       0x18034c0d3
000000018034C05C cmp       byte ptr [rip + 0x1998c67], 0 # 0x181ce4cca
000000018034C063 jne       0x18034c084
000000018034C065 lea       rcx, [rip + 0x1864494] # 0x181bb0500
000000018034C06C call      0x18024fef0
000000018034C071 lea       rcx, [rip + 0x1861b28] # 0x181badba0
000000018034C078 call      0x18024fef0
000000018034C07D mov       byte ptr [rip + 0x1998c46], 1 # 0x181ce4cca
000000018034C084 xor       edx, edx
000000018034C086 mov       rcx, rbx
000000018034C089 call      0x1803489a0
000000018034C08E mov       rcx, qword ptr [rip + 0x186446b] # 0x181bb0500
000000018034C095 mov       rdi, rax
000000018034C098 cmp       dword ptr [rcx + 0xe0], 0
000000018034C09F jne       0x18034c0a6
000000018034C0A1 call      0x1802501e0
000000018034C0A6 xor       edx, edx
000000018034C0A8 mov       rcx, rdi
000000018034C0AB call      0x18131f870
000000018034C0B0 test      al, al
000000018034C0B2 je        0x18034c0d3
000000018034C0B4 mov       rcx, qword ptr [rbx + 0xc8]
000000018034C0BB test      rcx, rcx
000000018034C0BE je        0x18034c3d7
000000018034C0C4 mov       rdx, qword ptr [rip + 0x1861ad5] # 0x181badba0
000000018034C0CB xor       r8d, r8d
000000018034C0CE call      0x1812dbc00
000000018034C0D3 cmp       dword ptr [rbx + 0x5c], 9
000000018034C0D7 xorps     xmm7, xmm7
000000018034C0DA jne       0x18034c1ec
000000018034C0E0 cmp       byte ptr [rip + 0x1998bde], 0 # 0x181ce4cc5
000000018034C0E7 jne       0x18034c0fc
000000018034C0E9 lea       rcx, [rip + 0x1860b08] # 0x181bacbf8
000000018034C0F0 call      0x18024fef0
000000018034C0F5 mov       byte ptr [rip + 0x1998bc9], 1 # 0x181ce4cc5
000000018034C0FC movss     xmm0, dword ptr [rbx + 0x74]
000000018034C101 comiss    xmm0, xmm7
000000018034C104 jbe       0x18034c1ec
000000018034C10A xor       ecx, ecx
000000018034C10C movaps    xmmword ptr [rsp + 0x30], xmm8
000000018034C112 movaps    xmm6, xmm0
000000018034C115 call      0x18132c2f0
000000018034C11A mulss     xmm0, dword ptr [rip + 0x125bb32] # 0x1815a7c54
000000018034C122 xor       edx, edx
000000018034C124 mov       rcx, rbx
000000018034C127 subss     xmm6, xmm0
000000018034C12B movss     dword ptr [rbx + 0x74], xmm6
000000018034C130 call      0x1803474e0
000000018034C135 comiss    xmm7, dword ptr [rbx + 0x74]
000000018034C139 jb        0x18034c14c
000000018034C13B xor       edx, edx
000000018034C13D mov       dword ptr [rbx + 0x74], 0
000000018034C144 mov       rcx, rbx
000000018034C147 call      0x1803461e0
000000018034C14C mov       rcx, qword ptr [rip + 0x1860aa5] # 0x181bacbf8
000000018034C153 movss     xmm6, dword ptr [rbx + 0x74]
000000018034C158 movss     xmm8, dword ptr [rbx + 0x78]
000000018034C15E cmp       dword ptr [rcx + 0xe0], 0
000000018034C165 jne       0x18034c16c
000000018034C167 call      0x1802501e0
000000018034C16C movsd     xmm1, qword ptr [rip + 0x125b8a4] # 0x1815a7a18
000000018034C174 xor       r8d, r8d
000000018034C177 divss     xmm6, xmm8
000000018034C17C cvtps2pd  xmm0, xmm6
000000018034C17F call      0x180ca60d0
000000018034C184 xorps     xmm6, xmm6
000000018034C187 cvtsd2ss  xmm6, xmm0
000000018034C18B mov       rcx, qword ptr [rbx + 0xc0]
000000018034C192 movaps    xmm8, xmmword ptr [rsp + 0x30]
000000018034C198 test      rcx, rcx
000000018034C19B je        0x18034c3d7
000000018034C1A1 xor       edx, edx
000000018034C1A3 call      0x18131c070
000000018034C1A8 test      rax, rax
000000018034C1AB je        0x18034c3d7
000000018034C1B1 xor       r8d, r8d
000000018034C1B4 mov       edx, 1
000000018034C1B9 mov       rcx, rax
000000018034C1BC call      0x18132dbe0
000000018034C1C1 test      rax, rax
000000018034C1C4 je        0x18034c3d7
000000018034C1CA movaps    xmm0, xmm6
000000018034C1CD movss     dword ptr [rsp + 0x28], xmm7
000000018034C1D3 unpcklps  xmm0, xmm6
000000018034C1D6 lea       rdx, [rsp + 0x20]
000000018034C1DB xor       r8d, r8d
000000018034C1DE movsd     qword ptr [rsp + 0x20], xmm0
000000018034C1E4 mov       rcx, rax
000000018034C1E7 call      0x18132fa50
000000018034C1EC cmp       dword ptr [rbx + 0x5c], 0xa
000000018034C1F0 jne       0x18034c1fc
000000018034C1F2 xor       edx, edx
000000018034C1F4 mov       rcx, rbx
000000018034C1F7 call      0x180348290
000000018034C1FC cmp       dword ptr [rbx + 0x5c], 0xd
000000018034C200 jne       0x18034c311
000000018034C206 cmp       byte ptr [rip + 0x1998ab9], 0 # 0x181ce4cc6
000000018034C20D jne       0x18034c222
000000018034C20F lea       rcx, [rip + 0x18642ea] # 0x181bb0500
000000018034C216 call      0x18024fef0
000000018034C21B mov       byte ptr [rip + 0x1998aa4], 1 # 0x181ce4cc6
000000018034C222 cmp       byte ptr [rbx + 0x95], 0
000000018034C229 jne       0x18034c311
000000018034C22F mov       rcx, qword ptr [rip + 0x18642ca] # 0x181bb0500
000000018034C236 mov       rdi, qword ptr [rbx + 0xe8]
000000018034C23D cmp       dword ptr [rcx + 0xe0], 0
000000018034C244 jne       0x18034c24b
000000018034C246 call      0x1802501e0
000000018034C24B xor       r8d, r8d
000000018034C24E xor       edx, edx
000000018034C250 mov       rcx, rdi
000000018034C253 call      0x18131f760
000000018034C258 test      al, al
000000018034C25A jne       0x18034c307
000000018034C260 mov       rcx, qword ptr [rbx + 0xe8]
000000018034C267 test      rcx, rcx
000000018034C26A je        0x18034c3d7
000000018034C270 xor       edx, edx
000000018034C272 call      0x1803659e0
000000018034C277 test      al, al
000000018034C279 jne       0x18034c307
000000018034C27F movss     xmm0, dword ptr [rbx + 0x9c]
000000018034C287 comiss    xmm0, xmm7
000000018034C28A jbe       0x18034c2bd
000000018034C28C xor       ecx, ecx
000000018034C28E movaps    xmm6, xmm0
000000018034C291 call      0x18132c2f0
000000018034C296 subss     xmm6, xmm0
000000018034C29A comiss    xmm7, xmm6
000000018034C29D movss     dword ptr [rbx + 0x9c], xmm6
000000018034C2A5 jb        0x18034c2bd
000000018034C2A7 mov       eax, dword ptr [rbx + 0xa0]
000000018034C2AD xor       edx, edx
000000018034C2AF mov       rcx, rbx
000000018034C2B2 mov       dword ptr [rbx + 0x9c], eax
000000018034C2B8 call      0x180347f00
000000018034C2BD mov       rax, qword ptr [rbx + 0xe8]
000000018034C2C4 test      rax, rax
000000018034C2C7 je        0x18034c3d7
000000018034C2CD cmp       dword ptr [rax + 0x80], 3
000000018034C2D4 jge       0x18034c311
000000018034C2D6 movss     xmm0, dword ptr [rbx + 0x74]
000000018034C2DB comiss    xmm0, xmm7
000000018034C2DE jbe       0x18034c311
000000018034C2E0 xor       ecx, ecx
000000018034C2E2 movaps    xmm6, xmm0
000000018034C2E5 call      0x18132c2f0
000000018034C2EA mulss     xmm0, dword ptr [rip + 0x125b97a] # 0x1815a7c6c
000000018034C2F2 subss     xmm6, xmm0
000000018034C2F6 comiss    xmm7, xmm6
000000018034C2F9 movss     dword ptr [rbx + 0x74], xmm6
000000018034C2FE jb        0x18034c311
000000018034C300 mov       dword ptr [rbx + 0x74], 0
000000018034C307 xor       edx, edx
000000018034C309 mov       rcx, rbx
000000018034C30C call      0x1803461e0
000000018034C311 cmp       dword ptr [rbx + 0x5c], 0xe
000000018034C315 jne       0x18034c3b6
000000018034C31B movss     xmm0, dword ptr [rbx + 0x98]
000000018034C323 comiss    xmm0, dword ptr [rip + 0x125b6fa] # 0x1815a7a24
000000018034C32A ja        0x18034c3cb
000000018034C330 cmp       byte ptr [rbx + 0xa4], 0
000000018034C337 movss     xmm6, dword ptr [rip + 0x125b6d1] # 0x1815a7a10
000000018034C33F je        0x18034c357
000000018034C341 comiss    xmm0, xmm6
000000018034C344 jbe       0x18034c357
000000018034C346 xor       edx, edx
000000018034C348 mov       rcx, rbx
000000018034C34B call      0x180346d00
000000018034C350 mov       byte ptr [rbx + 0xa4], 0
000000018034C357 movss     xmm0, dword ptr [rbx + 0x98]
000000018034C35F comiss    xmm0, xmm6
000000018034C362 jbe       0x18034c3b6
000000018034C364 movss     xmm0, dword ptr [rbx + 0x9c]
000000018034C36C comiss    xmm0, xmm7
000000018034C36F jbe       0x18034c3b6
000000018034C371 xor       ecx, ecx
000000018034C373 movaps    xmm6, xmm0
000000018034C376 call      0x18132c2f0
000000018034C37B subss     xmm6, xmm0
000000018034C37F comiss    xmm7, xmm6
000000018034C382 movss     dword ptr [rbx + 0x9c], xmm6
000000018034C38A jb        0x18034c3b6
000000018034C38C xor       edx, edx
000000018034C38E mov       rcx, rbx
000000018034C391 call      0x180347020
000000018034C396 xor       edx, edx
000000018034C398 mov       rcx, rbx
000000018034C39B call      0x180347020
000000018034C3A0 xor       edx, edx
000000018034C3A2 mov       rcx, rbx
000000018034C3A5 call      0x180347020
000000018034C3AA mov       eax, dword ptr [rbx + 0xa0]
000000018034C3B0 mov       dword ptr [rbx + 0x9c], eax
000000018034C3B6 movaps    xmm6, xmmword ptr [rsp + 0x50]
000000018034C3BB movaps    xmm7, xmmword ptr [rsp + 0x40]
000000018034C3C0 mov       rdi, qword ptr [rsp + 0x70]
000000018034C3C5 add       rsp, 0x60
000000018034C3C9 pop       rbx
000000018034C3CA ret       
000000018034C3CB xor       edx, edx
000000018034C3CD mov       rcx, rbx
000000018034C3D0 call      0x1803461e0
000000018034C3D5 jmp       0x18034c3b6
000000018034C3D7 call      0x180250150
000000018034C3DC int3      
000000018034C3DD int3      
000000018034C3DE int3      
000000018034C3DF int3      