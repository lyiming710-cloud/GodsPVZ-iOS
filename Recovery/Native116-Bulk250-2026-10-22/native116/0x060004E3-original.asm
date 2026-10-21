0000000180386210 mov       qword ptr [rsp + 0x18], rbx
0000000180386215 push      rbp
0000000180386216 push      rsi
0000000180386217 push      rdi
0000000180386218 push      r12
000000018038621A push      r15
000000018038621C sub       rsp, 0x40
0000000180386220 cmp       byte ptr [rip + 0x195ebd1], 0 # 0x181ce4df8
0000000180386227 mov       r15, r9
000000018038622A mov       r12, r8
000000018038622D mov       rbx, rdx
0000000180386230 mov       rbp, rcx
0000000180386233 jne       0x180386278
0000000180386235 lea       rcx, [rip + 0x1814b0c] # 0x181b9ad48
000000018038623C call      0x18024fef0
0000000180386241 lea       rcx, [rip + 0x182c320] # 0x181bb2568
0000000180386248 call      0x18024fef0
000000018038624D lea       rcx, [rip + 0x182a2ac] # 0x181bb0500
0000000180386254 call      0x18024fef0
0000000180386259 lea       rcx, [rip + 0x1833650] # 0x181bb98b0
0000000180386260 call      0x18024fef0
0000000180386265 lea       rcx, [rip + 0x181d1bc] # 0x181ba3428
000000018038626C call      0x18024fef0
0000000180386271 mov       byte ptr [rip + 0x195eb80], 1 # 0x181ce4df8
0000000180386278 mov       rcx, qword ptr [rip + 0x1814ac9] # 0x181b9ad48
000000018038627F xor       esi, esi
0000000180386281 mov       qword ptr [rsp + 0x78], rsi
0000000180386286 cmp       dword ptr [rcx + 0xe0], esi
000000018038628C jne       0x180386293
000000018038628E call      0x1802501e0
0000000180386293 mov       rcx, qword ptr [rip + 0x181d18e] # 0x181ba3428
000000018038629A xor       edx, edx
000000018038629C call      0x1812e68f0
00000001803862A1 mov       rcx, qword ptr [rip + 0x182a258] # 0x181bb0500
00000001803862A8 mov       edi, esi
00000001803862AA cmp       dword ptr [rcx + 0xe0], esi
00000001803862B0 jne       0x1803862b7
00000001803862B2 call      0x1802501e0
00000001803862B7 xor       edx, edx
00000001803862B9 mov       qword ptr [rsp + 0x70], r14
00000001803862BE mov       rcx, rbx
00000001803862C1 call      0x18131f870
00000001803862C6 test      al, al
00000001803862C8 je        0x1803862dc
00000001803862CA test      rbx, rbx
00000001803862CD je        0x180386544
00000001803862D3 mov       eax, dword ptr [rbx + 0x7c]
00000001803862D6 lea       edi, [rax + rax*2]
00000001803862D9 add       edi, dword ptr [rbp + 0x18]
00000001803862DC mov       rax, qword ptr [rip + 0x18335cd] # 0x181bb98b0
00000001803862E3 cmp       dword ptr [rax + 0xe0], esi
00000001803862E9 jne       0x1803862fa
00000001803862EB mov       rcx, rax
00000001803862EE call      0x1802501e0
00000001803862F3 mov       rax, qword ptr [rip + 0x18335b6] # 0x181bb98b0
00000001803862FA mov       rax, qword ptr [rax + 0xb8]
0000000180386301 mov       rcx, qword ptr [rax + 0x10]
0000000180386305 test      rcx, rcx
0000000180386308 je        0x180386544
000000018038630E mov       r9, qword ptr [rip + 0x182c253] # 0x181bb2568
0000000180386315 lea       r8, [rsp + 0x78]
000000018038631A mov       edx, edi
000000018038631C call      0x1805adc80
0000000180386321 mov       r10, qword ptr [rsp + 0x78]
0000000180386326 mov       r14, qword ptr [rsp + 0x90]
000000018038632E test      r10, r10
0000000180386331 je        0x180386352
0000000180386333 mov       rax, qword ptr [r10 + 0x28]
0000000180386337 mov       r9, r15
000000018038633A mov       rcx, qword ptr [r10 + 0x40]
000000018038633E mov       r8, r12
0000000180386341 mov       qword ptr [rsp + 0x28], rax
0000000180386346 mov       rdx, rbx
0000000180386349 mov       qword ptr [rsp + 0x20], r14
000000018038634E call      qword ptr [r10 + 0x18]
0000000180386352 mov       rcx, qword ptr [rip + 0x182a1a7] # 0x181bb0500
0000000180386359 cmp       dword ptr [rcx + 0xe0], esi
000000018038635F jne       0x180386366
0000000180386361 call      0x1802501e0
0000000180386366 xor       edx, edx
0000000180386368 mov       rcx, rbx
000000018038636B call      0x18131f870
0000000180386370 test      al, al
0000000180386372 je        0x18038652b
0000000180386378 cmp       dword ptr [rbp + 0x2c], esi
000000018038637B jne       0x18038652b
0000000180386381 test      rbx, rbx
0000000180386384 je        0x180386544
000000018038638A movss     xmm1, dword ptr [rbx + 0xec]
0000000180386392 xorps     xmm0, xmm0
0000000180386395 subss     xmm1, dword ptr [rip + 0x1221673] # 0x1815a7a10
000000018038639D comiss    xmm0, xmm1
00000001803863A0 movss     dword ptr [rbx + 0xec], xmm1
00000001803863A8 jb        0x18038652b
00000001803863AE cmp       byte ptr [rip + 0x195ea44], sil # 0x181ce4df9
00000001803863B5 jne       0x180386406
00000001803863B7 lea       rcx, [rip + 0x181498a] # 0x181b9ad48
00000001803863BE call      0x18024fef0
00000001803863C3 lea       rcx, [rip + 0x182c096] # 0x181bb2460
00000001803863CA call      0x18024fef0
00000001803863CF lea       rcx, [rip + 0x182a12a] # 0x181bb0500
00000001803863D6 call      0x18024fef0
00000001803863DB lea       rcx, [rip + 0x18334ce] # 0x181bb98b0
00000001803863E2 call      0x18024fef0
00000001803863E7 lea       rcx, [rip + 0x18332b2] # 0x181bb96a0
00000001803863EE call      0x18024fef0
00000001803863F3 lea       rcx, [rip + 0x181d08e] # 0x181ba3488
00000001803863FA call      0x18024fef0
00000001803863FF mov       byte ptr [rip + 0x195e9f3], 1 # 0x181ce4df9
0000000180386406 mov       rcx, qword ptr [rip + 0x181493b] # 0x181b9ad48
000000018038640D mov       qword ptr [rsp + 0x30], rsi
0000000180386412 cmp       dword ptr [rcx + 0xe0], esi
0000000180386418 jne       0x18038641f
000000018038641A call      0x1802501e0
000000018038641F mov       rcx, qword ptr [rip + 0x181d062] # 0x181ba3488
0000000180386426 xor       edx, edx
0000000180386428 call      0x1812e68f0
000000018038642D mov       rcx, qword ptr [rip + 0x182a0cc] # 0x181bb0500
0000000180386434 mov       edi, esi
0000000180386436 cmp       dword ptr [rcx + 0xe0], esi
000000018038643C jne       0x180386443
000000018038643E call      0x1802501e0
0000000180386443 xor       edx, edx
0000000180386445 mov       rcx, rbx
0000000180386448 call      0x18131f870
000000018038644D test      al, al
000000018038644F je        0x1803864c0
0000000180386451 mov       eax, dword ptr [rbx + 0x7c]
0000000180386454 lea       edi, [rax + rax*2]
0000000180386457 add       edi, dword ptr [rbp + 0x18]
000000018038645A mov       byte ptr [rbx + 0xe8], sil
0000000180386461 mov       dword ptr [rbx + 0xec], esi
0000000180386467 mov       rdx, qword ptr [rip + 0x1833232] # 0x181bb96a0
000000018038646E mov       rsi, qword ptr [rbx + 0x168]
0000000180386475 cmp       dword ptr [rdx + 0xe0], 0
000000018038647C jne       0x18038648d
000000018038647E mov       rcx, rdx
0000000180386481 call      0x1802501e0
0000000180386486 mov       rdx, qword ptr [rip + 0x1833213] # 0x181bb96a0
000000018038648D test      rsi, rsi
0000000180386490 je        0x18038654a
0000000180386496 mov       rdx, qword ptr [rdx + 0xb8]
000000018038649D xor       r9d, r9d
00000001803864A0 xor       r8d, r8d
00000001803864A3 mov       rcx, rsi
00000001803864A6 mov       edx, dword ptr [rdx]
00000001803864A8 call      0x1812db9a0
00000001803864AD dec       dword ptr [rbp + 0x48]
00000001803864B0 xor       r8d, r8d
00000001803864B3 mov       edx, 0xffffffff
00000001803864B8 mov       rcx, rbx
00000001803864BB call      0x180359e70
00000001803864C0 mov       rax, qword ptr [rip + 0x18333e9] # 0x181bb98b0
00000001803864C7 cmp       dword ptr [rax + 0xe0], 0
00000001803864CE jne       0x1803864df
00000001803864D0 mov       rcx, rax
00000001803864D3 call      0x1802501e0
00000001803864D8 mov       rax, qword ptr [rip + 0x18333d1] # 0x181bb98b0
00000001803864DF mov       rax, qword ptr [rax + 0xb8]
00000001803864E6 mov       rcx, qword ptr [rax + 0x18]
00000001803864EA test      rcx, rcx
00000001803864ED je        0x18038654a
00000001803864EF mov       r9, qword ptr [rip + 0x182bf6a] # 0x181bb2460
00000001803864F6 lea       r8, [rsp + 0x30]
00000001803864FB mov       edx, edi
00000001803864FD call      0x1805adc80
0000000180386502 mov       r10, qword ptr [rsp + 0x30]
0000000180386507 test      r10, r10
000000018038650A je        0x18038652b
000000018038650C mov       rax, qword ptr [r10 + 0x28]
0000000180386510 mov       r9, r15
0000000180386513 mov       rcx, qword ptr [r10 + 0x40]
0000000180386517 mov       r8, r12
000000018038651A mov       qword ptr [rsp + 0x28], rax
000000018038651F mov       rdx, rbx
0000000180386522 mov       qword ptr [rsp + 0x20], r14
0000000180386527 call      qword ptr [r10 + 0x18]
000000018038652B mov       r14, qword ptr [rsp + 0x70]
0000000180386530 mov       rbx, qword ptr [rsp + 0x80]
0000000180386538 add       rsp, 0x40
000000018038653C pop       r15
000000018038653E pop       r12
0000000180386540 pop       rdi
0000000180386541 pop       rsi
0000000180386542 pop       rbp
0000000180386543 ret       
0000000180386544 call      0x180250150
0000000180386549 int3      
000000018038654A call      0x180250150
000000018038654F int3      