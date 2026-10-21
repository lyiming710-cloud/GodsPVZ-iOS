00000001803AF1B0 mov       qword ptr [rsp + 0x10], rbx
00000001803AF1B5 mov       qword ptr [rsp + 8], rcx
00000001803AF1BA push      rsi
00000001803AF1BB push      rdi
00000001803AF1BC push      r14
00000001803AF1BE sub       rsp, 0x60
00000001803AF1C2 mov       rdi, rcx
00000001803AF1C5 cmp       byte ptr [rip + 0x1935d64], 0 # 0x181ce4f30
00000001803AF1CC jne       0x1803af269
00000001803AF1D2 lea       rcx, [rip + 0x18188a7] # 0x181bc7a80
00000001803AF1D9 call      0x18024fef0
00000001803AF1DE lea       rcx, [rip + 0x18188f3] # 0x181bc7ad8
00000001803AF1E5 call      0x18024fef0
00000001803AF1EA lea       rcx, [rip + 0x181893f] # 0x181bc7b30
00000001803AF1F1 call      0x18024fef0
00000001803AF1F6 lea       rcx, [rip + 0x17f2e1b] # 0x181ba2018
00000001803AF1FD call      0x18024fef0
00000001803AF202 lea       rcx, [rip + 0x18092a7] # 0x181bb84b0
00000001803AF209 call      0x18024fef0
00000001803AF20E lea       rcx, [rip + 0x17f5063] # 0x181ba4278
00000001803AF215 call      0x18024fef0
00000001803AF21A lea       rcx, [rip + 0x18012df] # 0x181bb0500
00000001803AF221 call      0x18024fef0
00000001803AF226 lea       rcx, [rip + 0x180724b] # 0x181bb6478
00000001803AF22D call      0x18024fef0
00000001803AF232 lea       rcx, [rip + 0x180babf] # 0x181bbacf8
00000001803AF239 call      0x18024fef0
00000001803AF23E lea       rcx, [rip + 0x17f7303] # 0x181ba6548
00000001803AF245 call      0x18024fef0
00000001803AF24A lea       rcx, [rip + 0x17f54f7] # 0x181ba4748
00000001803AF251 call      0x18024fef0
00000001803AF256 lea       rcx, [rip + 0x17f506b] # 0x181ba42c8
00000001803AF25D call      0x18024fef0
00000001803AF262 mov       byte ptr [rip + 0x1935cc7], 1 # 0x181ce4f30
00000001803AF269 mov       rax, qword ptr [rip + 0x17f2da8] # 0x181ba2018
00000001803AF270 cmp       dword ptr [rax + 0xe0], 0
00000001803AF277 jne       0x1803af288
00000001803AF279 mov       rcx, rax
00000001803AF27C call      0x1802501e0
00000001803AF281 mov       rax, qword ptr [rip + 0x17f2d90] # 0x181ba2018
00000001803AF288 mov       rax, qword ptr [rax + 0xb8]
00000001803AF28F mov       r14, qword ptr [rax]
00000001803AF292 test      r14, r14
00000001803AF295 je        0x1803af579
00000001803AF29B mov       r14, qword ptr [r14 + 0x28]
00000001803AF29F mov       qword ptr [rsp + 0x90], r14
00000001803AF2A7 test      r14, r14
00000001803AF2AA je        0x1803af4bd
00000001803AF2B0 mov       rax, qword ptr [rip + 0x180ba41] # 0x181bbacf8
00000001803AF2B7 mov       rdx, qword ptr [rax + 0xb8]
00000001803AF2BE xor       r8d, r8d
00000001803AF2C1 mov       rdx, qword ptr [rdx]
00000001803AF2C4 mov       rcx, qword ptr [rdi + 0x68]
00000001803AF2C8 call      0x180b76170
00000001803AF2CD test      al, al
00000001803AF2CF jne       0x1803af4e4
00000001803AF2D5 mov       rsi, qword ptr [r14 + 0x10]
00000001803AF2D9 mov       qword ptr [rsp + 0x98], rsi
00000001803AF2E1 mov       rax, qword ptr [r14 + 0x18]
00000001803AF2E5 test      rax, rax
00000001803AF2E8 je        0x1803af579
00000001803AF2EE mov       rdx, qword ptr [rax + 0x18]
00000001803AF2F2 test      rdx, rdx
00000001803AF2F5 je        0x1803af579
00000001803AF2FB mov       r8, qword ptr [rip + 0x18091ae] # 0x181bb84b0
00000001803AF302 lea       rcx, [rsp + 0x28]
00000001803AF307 call      0x1808197f0
00000001803AF30C movups    xmm0, xmmword ptr [rsp + 0x28]
00000001803AF311 movups    xmmword ptr [rsp + 0x40], xmm0
00000001803AF316 movsd     xmm1, qword ptr [rsp + 0x38]
00000001803AF31C movsd     qword ptr [rsp + 0x50], xmm1
00000001803AF322 mov       qword ptr [rsp + 0x28], 0
00000001803AF32B lea       rbx, [rsp + 0x40]
00000001803AF330 mov       qword ptr [rsp + 0x30], rbx
00000001803AF335 nop       word ptr [rax + rax]
00000001803AF340 mov       rdx, qword ptr [rip + 0x1818791] # 0x181bc7ad8
00000001803AF347 lea       rcx, [rsp + 0x40]
00000001803AF34C call      0x180669280
00000001803AF351 test      al, al
00000001803AF353 je        0x1803af41d
00000001803AF359 mov       rcx, qword ptr [rsp + 0x50]
00000001803AF35E cmp       rcx, rsi
00000001803AF361 je        0x1803af340
00000001803AF363 test      rcx, rcx
00000001803AF366 je        0x1803af58a
00000001803AF36C xor       r8d, r8d
00000001803AF36F mov       rdx, qword ptr [rdi + 0x68]
00000001803AF373 mov       rcx, qword ptr [rcx + 0x18]
00000001803AF377 call      0x180b76170
00000001803AF37C test      al, al
00000001803AF37E je        0x1803af340
00000001803AF380 mov       rcx, qword ptr [rdi + 0x48]
00000001803AF384 test      rcx, rcx
00000001803AF387 je        0x1803af585
00000001803AF38D xor       r9d, r9d
00000001803AF390 mov       r8b, 1
00000001803AF393 mov       rdx, qword ptr [rip + 0x17f71ae] # 0x181ba6548
00000001803AF39A call      0x1811af070
00000001803AF39F mov       rax, qword ptr [rip + 0x18070d2] # 0x181bb6478
00000001803AF3A6 cmp       dword ptr [rax + 0xe0], 0
00000001803AF3AD jne       0x1803af3be
00000001803AF3AF mov       rcx, rax
00000001803AF3B2 call      0x1802501e0
00000001803AF3B7 mov       rax, qword ptr [rip + 0x18070ba] # 0x181bb6478
00000001803AF3BE mov       rax, qword ptr [rax + 0xb8]
00000001803AF3C5 mov       rcx, qword ptr [rax + 0x108]
00000001803AF3CC test      rcx, rcx
00000001803AF3CF je        0x1803af57f
00000001803AF3D5 mov       r8, qword ptr [rip + 0x17f4e9c] # 0x181ba4278
00000001803AF3DC mov       edx, 2
00000001803AF3E1 call      0x180825710
00000001803AF3E6 mov       rdi, rax
00000001803AF3E9 mov       rcx, qword ptr [rip + 0x17f2c28] # 0x181ba2018
00000001803AF3F0 cmp       dword ptr [rcx + 0xe0], 0
00000001803AF3F7 jne       0x1803af3fe
00000001803AF3F9 call      0x1802501e0
00000001803AF3FE xor       edx, edx
00000001803AF400 mov       rcx, rdi
00000001803AF403 call      0x18031b160
00000001803AF408 nop       
00000001803AF409 mov       rdx, qword ptr [rip + 0x1818670] # 0x181bc7a80
00000001803AF410 mov       rcx, rbx
00000001803AF413 call      0x180302170
00000001803AF418 jmp       0x1803af4bd
00000001803AF41D mov       rdx, qword ptr [rip + 0x181865c] # 0x181bc7a80
00000001803AF424 mov       rcx, rbx
00000001803AF427 call      0x180302170
00000001803AF42C jmp       0x1803af465
00000001803AF42E mov       rdx, qword ptr [rip + 0x181864b] # 0x181bc7a80
00000001803AF435 mov       rcx, qword ptr [rsp + 0x30]
00000001803AF43A call      0x180302170
00000001803AF43F mov       rcx, qword ptr [rsp + 0x28]
00000001803AF444 test      rcx, rcx
00000001803AF447 jne       0x1803af590
00000001803AF44D mov       rdi, qword ptr [rsp + 0x80]
00000001803AF455 mov       r14, qword ptr [rsp + 0x90]
00000001803AF45D mov       rsi, qword ptr [rsp + 0x98]
00000001803AF465 test      rsi, rsi
00000001803AF468 je        0x1803af579
00000001803AF46E xor       r8d, r8d
00000001803AF471 mov       rdx, qword ptr [rsi + 0x18]
00000001803AF475 mov       rcx, qword ptr [rdi + 0x68]
00000001803AF479 call      0x180b76170
00000001803AF47E test      al, al
00000001803AF480 jne       0x1803af4ce
00000001803AF482 xor       r8d, r8d
00000001803AF485 mov       rdx, qword ptr [rdi + 0x68]
00000001803AF489 mov       rcx, r14
00000001803AF48C call      0x180340240
00000001803AF491 xor       edx, edx
00000001803AF493 mov       rcx, rdi
00000001803AF496 call      0x1813044d0
00000001803AF49B mov       rbx, rax
00000001803AF49E mov       rcx, qword ptr [rip + 0x180105b] # 0x181bb0500
00000001803AF4A5 cmp       dword ptr [rcx + 0xe0], 0
00000001803AF4AC jne       0x1803af4b3
00000001803AF4AE call      0x1802501e0
00000001803AF4B3 xor       edx, edx
00000001803AF4B5 mov       rcx, rbx
00000001803AF4B8 call      0x18131e940
00000001803AF4BD mov       rbx, qword ptr [rsp + 0x88]
00000001803AF4C5 add       rsp, 0x60
00000001803AF4C9 pop       r14
00000001803AF4CB pop       rdi
00000001803AF4CC pop       rsi
00000001803AF4CD ret       
00000001803AF4CE mov       rcx, qword ptr [rdi + 0x48]
00000001803AF4D2 test      rcx, rcx
00000001803AF4D5 je        0x1803af579
00000001803AF4DB mov       rdx, qword ptr [rip + 0x17f5266] # 0x181ba4748
00000001803AF4E2 jmp       0x1803af4f8
00000001803AF4E4 mov       rcx, qword ptr [rdi + 0x48]
00000001803AF4E8 test      rcx, rcx
00000001803AF4EB je        0x1803af579
00000001803AF4F1 mov       rdx, qword ptr [rip + 0x17f4dd0] # 0x181ba42c8
00000001803AF4F8 xor       r9d, r9d
00000001803AF4FB mov       r8b, 1
00000001803AF4FE call      0x1811af070
00000001803AF503 mov       rax, qword ptr [rip + 0x1806f6e] # 0x181bb6478
00000001803AF50A cmp       dword ptr [rax + 0xe0], 0
00000001803AF511 jne       0x1803af522
00000001803AF513 mov       rcx, rax
00000001803AF516 call      0x1802501e0
00000001803AF51B mov       rax, qword ptr [rip + 0x1806f56] # 0x181bb6478
00000001803AF522 mov       rax, qword ptr [rax + 0xb8]
00000001803AF529 mov       rcx, qword ptr [rax + 0x108]
00000001803AF530 test      rcx, rcx
00000001803AF533 je        0x1803af579
00000001803AF535 mov       r8, qword ptr [rip + 0x17f4d3c] # 0x181ba4278
00000001803AF53C mov       edx, 2
00000001803AF541 call      0x180825710
00000001803AF546 mov       rcx, qword ptr [rip + 0x17f2acb] # 0x181ba2018
00000001803AF54D cmp       dword ptr [rcx + 0xe0], 0
00000001803AF554 mov       rbx, rax
00000001803AF557 jne       0x1803af55e
00000001803AF559 call      0x1802501e0
00000001803AF55E xor       edx, edx
00000001803AF560 mov       rcx, rbx
00000001803AF563 call      0x18031b160
00000001803AF568 mov       rbx, qword ptr [rsp + 0x88]
00000001803AF570 add       rsp, 0x60
00000001803AF574 pop       r14
00000001803AF576 pop       rdi
00000001803AF577 pop       rsi
00000001803AF578 ret       
00000001803AF579 call      0x180250150
00000001803AF57E nop       
00000001803AF57F call      0x180250150
00000001803AF584 nop       
00000001803AF585 call      0x180250150
00000001803AF58A call      0x180250150
00000001803AF58F nop       
00000001803AF590 call      0x180200e60
00000001803AF595 int3      
00000001803AF596 int3      
00000001803AF597 int3      
00000001803AF598 int3      
00000001803AF599 int3      
00000001803AF59A int3      
00000001803AF59B int3      
00000001803AF59C int3      
00000001803AF59D int3      
00000001803AF59E int3      
00000001803AF59F int3      