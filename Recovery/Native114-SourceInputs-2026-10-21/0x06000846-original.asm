00000001803C8110 mov       qword ptr [rsp + 0x20], rbx
00000001803C8115 push      rdi
00000001803C8116 sub       rsp, 0x30
00000001803C811A cmp       byte ptr [rip + 0x191ceac], 0 # 0x181ce4fcd
00000001803C8121 mov       rdi, rcx
00000001803C8124 jne       0x1803c8145
00000001803C8126 lea       rcx, [rip + 0x17e496b] # 0x181baca98
00000001803C812D call      0x18024fef0
00000001803C8132 lea       rcx, [rip + 0x17e83c7] # 0x181bb0500
00000001803C8139 call      0x18024fef0
00000001803C813E mov       byte ptr [rip + 0x191ce88], 1 # 0x181ce4fcd
00000001803C8145 mov       rcx, qword ptr [rip + 0x17e83b4] # 0x181bb0500
00000001803C814C mov       rbx, qword ptr [rdi + 0x28]
00000001803C8150 cmp       dword ptr [rcx + 0xe0], 0
00000001803C8157 jne       0x1803c815e
00000001803C8159 call      0x1802501e0
00000001803C815E xor       edx, edx
00000001803C8160 mov       rcx, rbx
00000001803C8163 call      0x18131f870
00000001803C8168 test      al, al
00000001803C816A je        0x1803c8514
00000001803C8170 cmp       qword ptr [rdi + 0x48], 0
00000001803C8175 jne       0x1803c81a0
00000001803C8177 mov       rcx, qword ptr [rip + 0x17e491a] # 0x181baca98
00000001803C817E call      0x180250100
00000001803C8183 xor       edx, edx
00000001803C8185 mov       rcx, rax
00000001803C8188 mov       rbx, rax
00000001803C818B call      0x1812eefd0
00000001803C8190 lea       rcx, [rdi + 0x48]
00000001803C8194 mov       qword ptr [rdi + 0x48], rbx
00000001803C8198 mov       rdx, rbx
00000001803C819B call      0x18024f360
00000001803C81A0 mov       rcx, qword ptr [rdi + 0x28]
00000001803C81A4 mov       qword ptr [rsp + 0x40], rbp
00000001803C81A9 mov       qword ptr [rsp + 0x48], rsi
00000001803C81AE mov       qword ptr [rsp + 0x50], r14
00000001803C81B3 test      rcx, rcx
00000001803C81B6 je        0x1803c851f
00000001803C81BC mov       rdx, qword ptr [rdi + 0x48]
00000001803C81C0 xor       r8d, r8d
00000001803C81C3 call      0x1812faeb0
00000001803C81C8 cmp       byte ptr [rip + 0x191ce43], 0 # 0x181ce5012
00000001803C81CF mov       rbx, qword ptr [rdi + 0x48]
00000001803C81D3 jne       0x1803c81e8
00000001803C81D5 lea       rcx, [rip + 0x17d4f8c] # 0x181b9d168
00000001803C81DC call      0x18024fef0
00000001803C81E1 mov       byte ptr [rip + 0x191ce2a], 1 # 0x181ce5012
00000001803C81E8 xor       ecx, ecx
00000001803C81EA call      0x1803c5d10
00000001803C81EF test      rbx, rbx
00000001803C81F2 je        0x1803c851f
00000001803C81F8 mov       rax, qword ptr [rip + 0x17d4f69] # 0x181b9d168
00000001803C81FF lea       r8, [rsp + 0x20]
00000001803C8204 movups    xmm0, xmmword ptr [rdi + 0x74]
00000001803C8208 xor       r9d, r9d
00000001803C820B mov       rcx, rbx
00000001803C820E mov       rdx, qword ptr [rax + 0xb8]
00000001803C8215 movaps    xmmword ptr [rsp + 0x20], xmm0
00000001803C821A mov       edx, dword ptr [rdx]
00000001803C821C call      0x1812ee1d0
00000001803C8221 mov       rcx, qword ptr [rip + 0x17e82d8] # 0x181bb0500
00000001803C8228 mov       rbx, qword ptr [rdi + 0x88]
00000001803C822F cmp       dword ptr [rcx + 0xe0], 0
00000001803C8236 jne       0x1803c823d
00000001803C8238 call      0x1802501e0
00000001803C823D xor       edx, edx
00000001803C823F mov       rcx, rbx
00000001803C8242 call      0x18131f870
00000001803C8247 xor       ebp, ebp
00000001803C8249 test      al, al
00000001803C824B je        0x1803c8263
00000001803C824D mov       rbx, qword ptr [rdi + 0x88]
00000001803C8254 test      rbx, rbx
00000001803C8257 je        0x1803c851f
00000001803C825D mov       rbx, qword ptr [rbx + 0x20]
00000001803C8261 jmp       0x1803c8266
00000001803C8263 mov       rbx, rbp
00000001803C8266 mov       rcx, qword ptr [rip + 0x17e8293] # 0x181bb0500
00000001803C826D cmp       dword ptr [rcx + 0xe0], ebp
00000001803C8273 jne       0x1803c827a
00000001803C8275 call      0x1802501e0
00000001803C827A xor       edx, edx
00000001803C827C mov       rcx, rbx
00000001803C827F call      0x18131f870
00000001803C8284 test      al, al
00000001803C8286 je        0x1803c82cc
00000001803C8288 test      rbx, rbx
00000001803C828B je        0x1803c851f
00000001803C8291 xor       edx, edx
00000001803C8293 mov       rcx, rbx
00000001803C8296 call      0x18132a270
00000001803C829B mov       rcx, qword ptr [rip + 0x17e825e] # 0x181bb0500
00000001803C82A2 mov       rsi, rax
00000001803C82A5 cmp       dword ptr [rcx + 0xe0], ebp
00000001803C82AB jne       0x1803c82b2
00000001803C82AD call      0x1802501e0
00000001803C82B2 xor       edx, edx
00000001803C82B4 mov       rcx, rsi
00000001803C82B7 call      0x18131f870
00000001803C82BC test      al, al
00000001803C82BE je        0x1803c82cc
00000001803C82C0 xor       edx, edx
00000001803C82C2 mov       rcx, rbx
00000001803C82C5 call      0x18132a270
00000001803C82CA jmp       0x1803c82d3
00000001803C82CC xor       ecx, ecx
00000001803C82CE call      0x1812fe5a0
00000001803C82D3 mov       rsi, rax
00000001803C82D6 mov       rcx, qword ptr [rip + 0x17e8223] # 0x181bb0500
00000001803C82DD cmp       dword ptr [rcx + 0xe0], ebp
00000001803C82E3 jne       0x1803c82ea
00000001803C82E5 call      0x1802501e0
00000001803C82EA xor       edx, edx
00000001803C82EC mov       rcx, rbx
00000001803C82EF call      0x18131f870
00000001803C82F4 test      al, al
00000001803C82F6 je        0x1803c830e
00000001803C82F8 test      rbx, rbx
00000001803C82FB je        0x1803c851f
00000001803C8301 xor       edx, edx
00000001803C8303 mov       rcx, rbx
00000001803C8306 call      0x181329f20
00000001803C830B mov       rbp, rax
00000001803C830E cmp       byte ptr [rip + 0x191ccfe], 0 # 0x181ce5013
00000001803C8315 mov       rbx, qword ptr [rdi + 0x48]
00000001803C8319 jne       0x1803c832e
00000001803C831B lea       rcx, [rip + 0x17d4e46] # 0x181b9d168
00000001803C8322 call      0x18024fef0
00000001803C8327 mov       byte ptr [rip + 0x191cce5], 1 # 0x181ce5013
00000001803C832E xor       ecx, ecx
00000001803C8330 call      0x1803c5d10
00000001803C8335 mov       rax, qword ptr [rip + 0x17d4e2c] # 0x181b9d168
00000001803C833C mov       rcx, qword ptr [rax + 0xb8]
00000001803C8343 mov       r14d, dword ptr [rcx + 4]
00000001803C8347 mov       rcx, qword ptr [rip + 0x17e81b2] # 0x181bb0500
00000001803C834E cmp       dword ptr [rcx + 0xe0], 0
00000001803C8355 jne       0x1803c835c
00000001803C8357 call      0x1802501e0
00000001803C835C xor       edx, edx
00000001803C835E mov       rcx, rsi
00000001803C8361 call      0x18131f870
00000001803C8366 test      al, al
00000001803C8368 jne       0x1803c8374
00000001803C836A xor       ecx, ecx
00000001803C836C call      0x1812fe5a0
00000001803C8371 mov       rsi, rax
00000001803C8374 test      rbx, rbx
00000001803C8377 je        0x1803c851f
00000001803C837D xor       r9d, r9d
00000001803C8380 mov       r8, rsi
00000001803C8383 mov       edx, r14d
00000001803C8386 mov       rcx, rbx
00000001803C8389 call      0x1812eeb90
00000001803C838E mov       rcx, qword ptr [rip + 0x17e816b] # 0x181bb0500
00000001803C8395 cmp       dword ptr [rcx + 0xe0], 0
00000001803C839C jne       0x1803c83a3
00000001803C839E call      0x1802501e0
00000001803C83A3 xor       edx, edx
00000001803C83A5 mov       rcx, rbp
00000001803C83A8 call      0x18131f870
00000001803C83AD mov       rbx, qword ptr [rdi + 0x48]
00000001803C83B1 test      al, al
00000001803C83B3 jne       0x1803c8455
00000001803C83B9 cmp       byte ptr [rip + 0x191cc55], al # 0x181ce5014
00000001803C83BF jne       0x1803c83d4
00000001803C83C1 lea       rcx, [rip + 0x17d4da0] # 0x181b9d168
00000001803C83C8 call      0x18024fef0
00000001803C83CD mov       byte ptr [rip + 0x191cc40], 1 # 0x181ce5014
00000001803C83D4 xor       ecx, ecx
00000001803C83D6 call      0x1803c5d10
00000001803C83DB mov       rax, qword ptr [rip + 0x17d4d86] # 0x181b9d168
00000001803C83E2 mov       rcx, qword ptr [rax + 0xb8]
00000001803C83E9 mov       esi, dword ptr [rcx + 8]
00000001803C83EC xor       ecx, ecx
00000001803C83EE call      0x1812fe5a0
00000001803C83F3 test      rbx, rbx
00000001803C83F6 je        0x1803c851f
00000001803C83FC xor       r9d, r9d
00000001803C83FF mov       r8, rax
00000001803C8402 mov       edx, esi
00000001803C8404 mov       rcx, rbx
00000001803C8407 call      0x1812eeb90
00000001803C840C cmp       byte ptr [rip + 0x191cc02], 0 # 0x181ce5015
00000001803C8413 mov       rbx, qword ptr [rdi + 0x48]
00000001803C8417 jne       0x1803c842c
00000001803C8419 lea       rcx, [rip + 0x17d4d48] # 0x181b9d168
00000001803C8420 call      0x18024fef0
00000001803C8425 mov       byte ptr [rip + 0x191cbe9], 1 # 0x181ce5015
00000001803C842C xor       ecx, ecx
00000001803C842E call      0x1803c5d10
00000001803C8433 test      rbx, rbx
00000001803C8436 je        0x1803c851f
00000001803C843C mov       rax, qword ptr [rip + 0x17d4d25] # 0x181b9d168
00000001803C8443 xorps     xmm2, xmm2
00000001803C8446 mov       rcx, qword ptr [rax + 0xb8]
00000001803C844D mov       edx, dword ptr [rcx + 0xc]
00000001803C8450 jmp       0x1803c84e5
00000001803C8455 cmp       byte ptr [rip + 0x191cbb8], 0 # 0x181ce5014
00000001803C845C jne       0x1803c8471
00000001803C845E lea       rcx, [rip + 0x17d4d03] # 0x181b9d168
00000001803C8465 call      0x18024fef0
00000001803C846A mov       byte ptr [rip + 0x191cba3], 1 # 0x181ce5014
00000001803C8471 xor       ecx, ecx
00000001803C8473 call      0x1803c5d10
00000001803C8478 test      rbx, rbx
00000001803C847B je        0x1803c851f
00000001803C8481 mov       rax, qword ptr [rip + 0x17d4ce0] # 0x181b9d168
00000001803C8488 xor       r9d, r9d
00000001803C848B mov       r8, rbp
00000001803C848E mov       rcx, rbx
00000001803C8491 mov       rdx, qword ptr [rax + 0xb8]
00000001803C8498 mov       edx, dword ptr [rdx + 8]
00000001803C849B call      0x1812eeb90
00000001803C84A0 cmp       byte ptr [rip + 0x191cb6e], 0 # 0x181ce5015
00000001803C84A7 mov       rbx, qword ptr [rdi + 0x48]
00000001803C84AB jne       0x1803c84c0
00000001803C84AD lea       rcx, [rip + 0x17d4cb4] # 0x181b9d168
00000001803C84B4 call      0x18024fef0
00000001803C84B9 mov       byte ptr [rip + 0x191cb55], 1 # 0x181ce5015
00000001803C84C0 xor       ecx, ecx
00000001803C84C2 call      0x1803c5d10
00000001803C84C7 test      rbx, rbx
00000001803C84CA je        0x1803c851f
00000001803C84CC mov       rax, qword ptr [rip + 0x17d4c95] # 0x181b9d168
00000001803C84D3 movss     xmm2, dword ptr [rip + 0x11df535] # 0x1815a7a10
00000001803C84DB mov       rdx, qword ptr [rax + 0xb8]
00000001803C84E2 mov       edx, dword ptr [rdx + 0xc]
00000001803C84E5 xor       r9d, r9d
00000001803C84E8 mov       rcx, rbx
00000001803C84EB call      0x1812ee4d0
00000001803C84F0 mov       rcx, qword ptr [rdi + 0x28]
00000001803C84F4 test      rcx, rcx
00000001803C84F7 je        0x1803c851f
00000001803C84F9 mov       rdx, qword ptr [rdi + 0x48]
00000001803C84FD xor       r8d, r8d
00000001803C8500 call      0x1812faf40
00000001803C8505 mov       rsi, qword ptr [rsp + 0x48]
00000001803C850A mov       rbp, qword ptr [rsp + 0x40]
00000001803C850F mov       r14, qword ptr [rsp + 0x50]
00000001803C8514 mov       rbx, qword ptr [rsp + 0x58]
00000001803C8519 add       rsp, 0x30
00000001803C851D pop       rdi
00000001803C851E ret       
00000001803C851F call      0x180250150
00000001803C8524 int3      
00000001803C8525 int3      
00000001803C8526 int3      
00000001803C8527 int3      
00000001803C8528 int3      
00000001803C8529 int3      
00000001803C852A int3      
00000001803C852B int3      
00000001803C852C int3      
00000001803C852D int3      
00000001803C852E int3      
00000001803C852F int3      