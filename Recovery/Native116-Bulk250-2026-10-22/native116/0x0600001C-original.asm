00000001802FC210 mov       qword ptr [rsp + 0x18], rsi
00000001802FC215 push      rdi
00000001802FC216 sub       rsp, 0x20
00000001802FC21A cmp       byte ptr [rip + 0x19e8894], 0 # 0x181ce4ab5
00000001802FC221 mov       rdi, rcx
00000001802FC224 jne       0x1802fc269
00000001802FC226 lea       rcx, [rip + 0x18a259b] # 0x181b9e7c8
00000001802FC22D call      0x18024fef0
00000001802FC232 lea       rcx, [rip + 0x18ae487] # 0x181baa6c0
00000001802FC239 call      0x18024fef0
00000001802FC23E lea       rcx, [rip + 0x18adfab] # 0x181baa1f0
00000001802FC245 call      0x18024fef0
00000001802FC24A lea       rcx, [rip + 0x18bb84f] # 0x181bb7aa0
00000001802FC251 call      0x18024fef0
00000001802FC256 lea       rcx, [rip + 0x18b42a3] # 0x181bb0500
00000001802FC25D call      0x18024fef0
00000001802FC262 mov       byte ptr [rip + 0x19e884c], 1 # 0x181ce4ab5
00000001802FC269 mov       rcx, qword ptr [rip + 0x18a2558] # 0x181b9e7c8
00000001802FC270 mov       qword ptr [rsp + 0x30], rbx
00000001802FC275 mov       qword ptr [rsp + 0x38], rbp
00000001802FC27A call      0x180250100
00000001802FC27F xor       edx, edx
00000001802FC281 mov       rcx, rax
00000001802FC284 mov       rsi, rax
00000001802FC287 call      0x18031a880
00000001802FC28C mov       rcx, qword ptr [rdi + 0x28]
00000001802FC290 test      rcx, rcx
00000001802FC293 je        0x1802fc483
00000001802FC299 mov       rcx, qword ptr [rcx + 0x30]
00000001802FC29D test      rcx, rcx
00000001802FC2A0 je        0x1802fc483
00000001802FC2A6 xor       edx, edx
00000001802FC2A8 call      0x18032c3b0
00000001802FC2AD test      rsi, rsi
00000001802FC2B0 je        0x1802fc483
00000001802FC2B6 mov       dword ptr [rsi + 0x18], eax
00000001802FC2B9 mov       rax, qword ptr [rdi + 0x28]
00000001802FC2BD test      rax, rax
00000001802FC2C0 je        0x1802fc483
00000001802FC2C6 mov       rcx, qword ptr [rax + 0x30]
00000001802FC2CA test      rcx, rcx
00000001802FC2CD je        0x1802fc483
00000001802FC2D3 xor       edx, edx
00000001802FC2D5 call      0x18032c440
00000001802FC2DA cdq       
00000001802FC2DB sub       eax, edx
00000001802FC2DD sar       eax, 1
00000001802FC2DF mov       dword ptr [rsi + 0x14], eax
00000001802FC2E2 mov       eax, dword ptr [rdi + 0xb8]
00000001802FC2E8 mov       dword ptr [rsi + 0x28], eax
00000001802FC2EB mov       rcx, qword ptr [rip + 0x18b420e] # 0x181bb0500
00000001802FC2F2 mov       rbx, qword ptr [rdi + 0xa8]
00000001802FC2F9 cmp       dword ptr [rcx + 0xe0], 0
00000001802FC300 jne       0x1802fc307
00000001802FC302 call      0x1802501e0
00000001802FC307 mov       rdx, qword ptr [rip + 0x18bb792] # 0x181bb7aa0
00000001802FC30E mov       rcx, rbx
00000001802FC311 call      0x1804a0690
00000001802FC316 mov       rbx, rax
00000001802FC319 test      rax, rax
00000001802FC31C je        0x1802fc483
00000001802FC322 xor       edx, edx
00000001802FC324 mov       rcx, rax
00000001802FC327 call      0x181304510
00000001802FC32C mov       rcx, qword ptr [rdi + 0x70]
00000001802FC330 mov       rbp, rax
00000001802FC333 test      rcx, rcx
00000001802FC336 je        0x1802fc483
00000001802FC33C xor       edx, edx
00000001802FC33E call      0x18131c070
00000001802FC343 test      rbp, rbp
00000001802FC346 je        0x1802fc483
00000001802FC34C xor       r9d, r9d
00000001802FC34F xor       r8d, r8d
00000001802FC352 mov       rdx, rax
00000001802FC355 mov       rcx, rbp
00000001802FC358 call      0x18132e900
00000001802FC35D lea       rcx, [rbx + 0x30]
00000001802FC361 mov       qword ptr [rbx + 0x30], rdi
00000001802FC365 mov       rdx, rdi
00000001802FC368 call      0x18024f360
00000001802FC36D xor       r8d, r8d
00000001802FC370 mov       rdx, rsi
00000001802FC373 mov       rcx, rbx
00000001802FC376 call      0x180304cb0
00000001802FC37B mov       rcx, qword ptr [rdi + 0x98]
00000001802FC382 test      rcx, rcx
00000001802FC385 je        0x1802fc483
00000001802FC38B mov       r9, qword ptr [rip + 0x18ae32e] # 0x181baa6c0
00000001802FC392 inc       dword ptr [rcx + 0x1c]
00000001802FC395 mov       rdx, qword ptr [rcx + 0x10]
00000001802FC399 movsxd    r8, dword ptr [rcx + 0x18]
00000001802FC39D test      rdx, rdx
00000001802FC3A0 je        0x1802fc483
00000001802FC3A6 cmp       r8d, dword ptr [rdx + 0x18]
00000001802FC3AA jb        0x1802fc3c5
00000001802FC3AC mov       rax, qword ptr [r9 + 0x20]
00000001802FC3B0 mov       rdx, rbx
00000001802FC3B3 mov       r8, qword ptr [rax + 0xc0]
00000001802FC3BA mov       r8, qword ptr [r8 + 0x70]
00000001802FC3BE call      0x180847ce0
00000001802FC3C3 jmp       0x1802fc3eb
00000001802FC3C5 lea       eax, [r8 + 1]
00000001802FC3C9 mov       dword ptr [rcx + 0x18], eax
00000001802FC3CC cmp       r8d, dword ptr [rdx + 0x18]
00000001802FC3D0 jae       0x1802fc489
00000001802FC3D6 mov       qword ptr [rdx + r8*8 + 0x20], rbx
00000001802FC3DB add       rdx, 0x20
00000001802FC3DF lea       rcx, [rdx + r8*8]
00000001802FC3E3 mov       rdx, rbx
00000001802FC3E6 call      0x18024f360
00000001802FC3EB mov       rcx, qword ptr [rdi + 0x80]
00000001802FC3F2 test      rcx, rcx
00000001802FC3F5 je        0x1802fc483
00000001802FC3FB xor       r8d, r8d
00000001802FC3FE mov       rdx, rbx
00000001802FC401 call      0x1803041c0
00000001802FC406 mov       rcx, qword ptr [rdi + 0x28]
00000001802FC40A test      rcx, rcx
00000001802FC40D je        0x1802fc483
00000001802FC40F mov       rcx, qword ptr [rcx + 0x50]
00000001802FC413 mov       rdx, qword ptr [rbx + 0x38]
00000001802FC417 test      rcx, rcx
00000001802FC41A je        0x1802fc483
00000001802FC41C mov       r10, qword ptr [rip + 0x18addcd] # 0x181baa1f0
00000001802FC423 inc       dword ptr [rcx + 0x1c]
00000001802FC426 mov       r8, qword ptr [rcx + 0x10]
00000001802FC42A movsxd    r9, dword ptr [rcx + 0x18]
00000001802FC42E test      r8, r8
00000001802FC431 je        0x1802fc483
00000001802FC433 cmp       r9d, dword ptr [r8 + 0x18]
00000001802FC437 jb        0x1802fc462
00000001802FC439 mov       rax, qword ptr [r10 + 0x20]
00000001802FC43D mov       r8, qword ptr [rax + 0xc0]
00000001802FC444 mov       r8, qword ptr [r8 + 0x70]
00000001802FC448 call      0x180847ce0
00000001802FC44D mov       rbp, qword ptr [rsp + 0x38]
00000001802FC452 mov       rbx, qword ptr [rsp + 0x30]
00000001802FC457 mov       rsi, qword ptr [rsp + 0x40]
00000001802FC45C add       rsp, 0x20
00000001802FC460 pop       rdi
00000001802FC461 ret       
00000001802FC462 lea       eax, [r9 + 1]
00000001802FC466 mov       dword ptr [rcx + 0x18], eax
00000001802FC469 cmp       r9d, dword ptr [r8 + 0x18]
00000001802FC46D jae       0x1802fc489
00000001802FC46F mov       qword ptr [r8 + r9*8 + 0x20], rdx
00000001802FC474 add       r8, 0x20
00000001802FC478 lea       rcx, [r8 + r9*8]
00000001802FC47C call      0x18024f360
00000001802FC481 jmp       0x1802fc44d
00000001802FC483 call      0x180250150
00000001802FC488 int3      
00000001802FC489 call      0x180250140
00000001802FC48E int3      
00000001802FC48F int3      