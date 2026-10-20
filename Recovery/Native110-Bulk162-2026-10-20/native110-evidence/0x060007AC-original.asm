00000001803BB230 push      rbx
00000001803BB232 sub       rsp, 0x30
00000001803BB236 cmp       byte ptr [rip + 0x1929d46], 0 # 0x181ce4f83
00000001803BB23D mov       rbx, rcx
00000001803BB240 jne       0x1803bb2b5
00000001803BB242 lea       rcx, [rip + 0x17e912f] # 0x181ba4378
00000001803BB249 call      0x18024fef0
00000001803BB24E lea       rcx, [rip + 0x17ea1a3] # 0x181ba53f8
00000001803BB255 call      0x18024fef0
00000001803BB25A lea       rcx, [rip + 0x17e6a57] # 0x181ba1cb8
00000001803BB261 call      0x18024fef0
00000001803BB266 lea       rcx, [rip + 0x1805813] # 0x181bc0a80
00000001803BB26D call      0x18024fef0
00000001803BB272 lea       rcx, [rip + 0x180590f] # 0x181bc0b88
00000001803BB279 call      0x18024fef0
00000001803BB27E lea       rcx, [rip + 0x17f75b3] # 0x181bb2838
00000001803BB285 call      0x18024fef0
00000001803BB28A lea       rcx, [rip + 0x180a85f] # 0x181bc5af0
00000001803BB291 call      0x18024fef0
00000001803BB296 lea       rcx, [rip + 0x180b03b] # 0x181bc62d8
00000001803BB29D call      0x18024fef0
00000001803BB2A2 lea       rcx, [rip + 0x180a73f] # 0x181bc59e8
00000001803BB2A9 call      0x18024fef0
00000001803BB2AE mov       byte ptr [rip + 0x1929cce], 1 # 0x181ce4f83
00000001803BB2B5 xor       edx, edx
00000001803BB2B7 mov       rcx, rbx
00000001803BB2BA call      0x181302c30
00000001803BB2BF test      al, al
00000001803BB2C1 je        0x1803bb4b5
00000001803BB2C7 mov       qword ptr [rsp + 0x48], rdi
00000001803BB2CC mov       qword ptr [rsp + 0x40], rsi
00000001803BB2D1 xor       ecx, ecx
00000001803BB2D3 call      0x1812e27c0
00000001803BB2D8 lea       rcx, [rbx + 0x40]
00000001803BB2DC mov       qword ptr [rbx + 0x40], rax
00000001803BB2E0 mov       rdx, rax
00000001803BB2E3 call      0x18024f360
00000001803BB2E8 mov       rcx, qword ptr [rip + 0x17e69c9] # 0x181ba1cb8
00000001803BB2EF call      0x180250100
00000001803BB2F4 mov       rdx, qword ptr [rip + 0x180afdd] # 0x181bc62d8
00000001803BB2FB xor       r8d, r8d
00000001803BB2FE mov       rcx, rax
00000001803BB301 mov       rdi, rax
00000001803BB304 call      0x18131bcf0
00000001803BB309 test      rdi, rdi
00000001803BB30C je        0x1803bb4bb
00000001803BB312 xor       edx, edx
00000001803BB314 mov       rcx, rdi
00000001803BB317 call      0x18131c070
00000001803BB31C lea       rcx, [rbx + 0x38]
00000001803BB320 mov       qword ptr [rbx + 0x38], rax
00000001803BB324 mov       rdx, rax
00000001803BB327 call      0x18024f360
00000001803BB32C mov       rcx, qword ptr [rbx + 0x40]
00000001803BB330 mov       rsi, qword ptr [rbx + 0x38]
00000001803BB334 test      rcx, rcx
00000001803BB337 je        0x1803bb4bb
00000001803BB33D xor       edx, edx
00000001803BB33F call      0x181304510
00000001803BB344 test      rsi, rsi
00000001803BB347 je        0x1803bb4bb
00000001803BB34D xor       r8d, r8d
00000001803BB350 mov       rdx, rax
00000001803BB353 mov       rcx, rsi
00000001803BB356 call      0x18132faa0
00000001803BB35B cmp       byte ptr [rip + 0x1929c4d], 0 # 0x181ce4faf
00000001803BB362 mov       rsi, qword ptr [rbx + 0x38]
00000001803BB366 jne       0x1803bb37b
00000001803BB368 lea       rcx, [rip + 0x17f8af1] # 0x181bb3e60
00000001803BB36F call      0x18024fef0
00000001803BB374 mov       byte ptr [rip + 0x1929c34], 1 # 0x181ce4faf
00000001803BB37B mov       rax, qword ptr [rip + 0x17f8ade] # 0x181bb3e60
00000001803BB382 mov       rcx, qword ptr [rax + 0xb8]
00000001803BB389 test      rsi, rsi
00000001803BB38C je        0x1803bb4bb
00000001803BB392 movups    xmm0, xmmword ptr [rcx]
00000001803BB395 xor       r8d, r8d
00000001803BB398 lea       rdx, [rsp + 0x20]
00000001803BB39D mov       rcx, rsi
00000001803BB3A0 movaps    xmmword ptr [rsp + 0x20], xmm0
00000001803BB3A5 call      0x18132f9b0
00000001803BB3AA mov       rdx, qword ptr [rip + 0x17e8fc7] # 0x181ba4378
00000001803BB3B1 mov       rcx, rdi
00000001803BB3B4 call      0x18046d280
00000001803BB3B9 lea       rcx, [rbx + 0x28]
00000001803BB3BD mov       qword ptr [rbx + 0x28], rax
00000001803BB3C1 mov       rdx, rax
00000001803BB3C4 call      0x18024f360
00000001803BB3C9 mov       rdx, qword ptr [rip + 0x18057b8] # 0x181bc0b88
00000001803BB3D0 mov       rcx, qword ptr [rip + 0x180a611] # 0x181bc59e8
00000001803BB3D7 mov       rsi, qword ptr [rbx + 0x28]
00000001803BB3DB call      0x1804a6ab0
00000001803BB3E0 test      rsi, rsi
00000001803BB3E3 je        0x1803bb4bb
00000001803BB3E9 xor       r8d, r8d
00000001803BB3EC mov       rdx, rax
00000001803BB3EF mov       rcx, rsi
00000001803BB3F2 call      0x1811b9520
00000001803BB3F7 mov       rdx, qword ptr [rip + 0x1805682] # 0x181bc0a80
00000001803BB3FE mov       rcx, qword ptr [rip + 0x180a6eb] # 0x181bc5af0
00000001803BB405 mov       rsi, qword ptr [rbx + 0x28]
00000001803BB409 call      0x1804a6ab0
00000001803BB40E test      rsi, rsi
00000001803BB411 je        0x1803bb4bb
00000001803BB417 mov       r9, qword ptr [rsi]
00000001803BB41A mov       rdx, rax
00000001803BB41D mov       rcx, rsi
00000001803BB420 mov       r8, qword ptr [r9 + 0x580]
00000001803BB427 call      qword ptr [r9 + 0x578]
00000001803BB42E mov       rcx, qword ptr [rbx + 0x28]
00000001803BB432 test      rcx, rcx
00000001803BB435 je        0x1803bb4bb
00000001803BB43B movss     xmm1, dword ptr [rip + 0x11ec5e9] # 0x1815a7a2c
00000001803BB443 xor       r8d, r8d
00000001803BB446 call      0x1811b9410
00000001803BB44B mov       rcx, qword ptr [rbx + 0x28]
00000001803BB44F test      rcx, rcx
00000001803BB452 je        0x1803bb4bb
00000001803BB454 xor       r8d, r8d
00000001803BB457 mov       dl, 1
00000001803BB459 call      0x1811b96e0
00000001803BB45E mov       rdx, qword ptr [rip + 0x17e9f93] # 0x181ba53f8
00000001803BB465 mov       rcx, rdi
00000001803BB468 call      0x18046dc80
00000001803BB46D lea       rcx, [rbx + 0x30]
00000001803BB471 mov       qword ptr [rbx + 0x30], rax
00000001803BB475 mov       rdx, rax
00000001803BB478 call      0x18024f360
00000001803BB47D mov       edx, dword ptr [rbx + 0x20]
00000001803BB480 xor       r8d, r8d
00000001803BB483 mov       rcx, rbx
00000001803BB486 call      0x1803bb4d0
00000001803BB48B mov       rcx, qword ptr [rbx + 0x28]
00000001803BB48F test      rcx, rcx
00000001803BB492 je        0x1803bb4bb
00000001803BB494 mov       rax, qword ptr [rcx]
00000001803BB497 mov       rdx, qword ptr [rip + 0x17f739a] # 0x181bb2838
00000001803BB49E mov       r8, qword ptr [rax + 0x560]
00000001803BB4A5 call      qword ptr [rax + 0x558]
00000001803BB4AB mov       rsi, qword ptr [rsp + 0x40]
00000001803BB4B0 mov       rdi, qword ptr [rsp + 0x48]
00000001803BB4B5 add       rsp, 0x30
00000001803BB4B9 pop       rbx
00000001803BB4BA ret       
00000001803BB4BB call      0x180250150
00000001803BB4C0 int3      