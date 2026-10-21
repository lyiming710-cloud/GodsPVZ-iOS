0000000180355410 push      rbx
0000000180355412 sub       rsp, 0x50
0000000180355416 cmp       byte ptr [rip + 0x198f8fc], 0 # 0x181ce4d19
000000018035541D mov       rbx, rcx
0000000180355420 jne       0x180355435
0000000180355422 lea       rcx, [rip + 0x185aba7] # 0x181baffd0
0000000180355429 call      0x18024fef0
000000018035542E mov       byte ptr [rip + 0x198f8e4], 1 # 0x181ce4d19
0000000180355435 mov       rcx, qword ptr [rbx + 0x220]
000000018035543C movaps    xmmword ptr [rsp + 0x20], xmm8
0000000180355442 test      rcx, rcx
0000000180355445 je        0x18035568a
000000018035544B mov       rdx, qword ptr [rip + 0x185ab7e] # 0x181baffd0
0000000180355452 xor       r8d, r8d
0000000180355455 mov       qword ptr [rsp + 0x60], rdi
000000018035545A movaps    xmmword ptr [rsp + 0x30], xmm7
000000018035545F movaps    xmmword ptr [rsp + 0x40], xmm6
0000000180355464 call      0x1803103a0
0000000180355469 cmp       byte ptr [rbx + 0xe8], 0
0000000180355470 mov       rdi, rax
0000000180355473 movss     xmm8, dword ptr [rip + 0x12525a4] # 0x1815a7a20
000000018035547C jne       0x180355525
0000000180355482 test      rax, rax
0000000180355485 je        0x1803554c4
0000000180355487 comiss    xmm8, dword ptr [rax + 0x60]
000000018035548C jbe       0x1803554c4
000000018035548E movss     xmm7, dword ptr [rax + 0x60]
0000000180355493 xor       ecx, ecx
0000000180355495 movss     xmm6, dword ptr [rbx + 0xc0]
000000018035549D call      0x18132c2f0
00000001803554A2 mulss     xmm0, xmm6
00000001803554A6 mulss     xmm0, dword ptr [rip + 0x1252752] # 0x1815a7c00
00000001803554AE addss     xmm0, xmm7
00000001803554B2 comiss    xmm0, xmm8
00000001803554B6 movss     dword ptr [rdi + 0x60], xmm0
00000001803554BB jbe       0x1803554c4
00000001803554BD mov       dword ptr [rdi + 0x60], 0x40000000
00000001803554C4 cmp       dword ptr [rbx + 0x88], 1
00000001803554CB jl        0x180355525
00000001803554CD movss     xmm0, dword ptr [rbx + 0x98]
00000001803554D5 comiss    xmm0, dword ptr [rbx + 0x94]
00000001803554DC jbe       0x180355525
00000001803554DE movss     xmm6, dword ptr [rbx + 0xc0]
00000001803554E6 xor       ecx, ecx
00000001803554E8 movaps    xmm7, xmm0
00000001803554EB call      0x18132c2f0
00000001803554F0 mulss     xmm7, dword ptr [rip + 0x1252704] # 0x1815a7bfc
00000001803554F8 mulss     xmm7, xmm6
00000001803554FC mulss     xmm7, xmm0
0000000180355500 addss     xmm7, dword ptr [rbx + 0x94]
0000000180355508 comiss    xmm7, dword ptr [rbx + 0x98]
000000018035550F movss     dword ptr [rbx + 0x94], xmm7
0000000180355517 jbe       0x180355525
0000000180355519 mov       eax, dword ptr [rbx + 0x98]
000000018035551F mov       dword ptr [rbx + 0x94], eax
0000000180355525 movss     xmm0, dword ptr [rbx + 0x210]
000000018035552D xorps     xmm7, xmm7
0000000180355530 comiss    xmm0, xmm7
0000000180355533 jbe       0x18035559c
0000000180355535 xor       ecx, ecx
0000000180355537 movaps    xmm6, xmm0
000000018035553A call      0x18132c2f0
000000018035553F mulss     xmm0, dword ptr [rbx + 0xc0]
0000000180355547 xor       ecx, ecx
0000000180355549 subss     xmm6, xmm0
000000018035554D movss     dword ptr [rbx + 0x210], xmm6
0000000180355555 movss     xmm6, dword ptr [rbx + 0x214]
000000018035555D call      0x18132c2f0
0000000180355562 comiss    xmm7, dword ptr [rbx + 0x210]
0000000180355569 mulss     xmm0, dword ptr [rbx + 0xc0]
0000000180355571 addss     xmm0, xmm6
0000000180355575 movss     dword ptr [rbx + 0x214], xmm0
000000018035557D jbe       0x180355589
000000018035557F mov       dword ptr [rbx + 0x210], 0
0000000180355589 comiss    xmm0, dword ptr [rip + 0x1252524] # 0x1815a7ab4
0000000180355590 jbe       0x18035559c
0000000180355592 xor       edx, edx
0000000180355594 mov       rcx, rbx
0000000180355597 call      0x1803547c0
000000018035559C cmp       byte ptr [rip + 0x198f77b], 0 # 0x181ce4d1e
00000001803555A3 movaps    xmm7, xmmword ptr [rsp + 0x30]
00000001803555A8 movaps    xmm6, xmmword ptr [rsp + 0x40]
00000001803555AD jne       0x1803555da
00000001803555AF lea       rcx, [rip + 0x185af4a] # 0x181bb0500
00000001803555B6 call      0x18024fef0
00000001803555BB lea       rcx, [rip + 0x1860966] # 0x181bb5f28
00000001803555C2 call      0x18024fef0
00000001803555C7 lea       rcx, [rip + 0x185aa02] # 0x181baffd0
00000001803555CE call      0x18024fef0
00000001803555D3 mov       byte ptr [rip + 0x198f744], 1 # 0x181ce4d1e
00000001803555DA mov       rcx, qword ptr [rip + 0x185af1f] # 0x181bb0500
00000001803555E1 mov       rdi, qword ptr [rbx + 0x168]
00000001803555E8 cmp       dword ptr [rcx + 0xe0], 0
00000001803555EF jne       0x1803555f6
00000001803555F1 call      0x1802501e0
00000001803555F6 xor       r8d, r8d
00000001803555F9 xor       edx, edx
00000001803555FB mov       rcx, rdi
00000001803555FE call      0x18131f760
0000000180355603 mov       rdi, qword ptr [rsp + 0x60]
0000000180355608 test      al, al
000000018035560A jne       0x18035565e
000000018035560C mov       rcx, qword ptr [rbx + 0x220]
0000000180355613 test      rcx, rcx
0000000180355616 je        0x18035568a
0000000180355618 mov       rdx, qword ptr [rip + 0x185a9b1] # 0x181baffd0
000000018035561F xor       r8d, r8d
0000000180355622 call      0x1803103a0
0000000180355627 test      rax, rax
000000018035562A je        0x18035565e
000000018035562C movss     xmm0, dword ptr [rax + 0x60]
0000000180355631 comiss    xmm0, xmm8
0000000180355635 mov       rcx, qword ptr [rbx + 0x168]
000000018035563C jae       0x18035567a
000000018035563E comiss    xmm0, dword ptr [rip + 0x12523cb] # 0x1815a7a10
0000000180355645 jae       0x18035566a
0000000180355647 test      rcx, rcx
000000018035564A je        0x18035568a
000000018035564C xor       r9d, r9d
000000018035564F xor       r8d, r8d
0000000180355652 mov       rdx, qword ptr [rip + 0x18608cf] # 0x181bb5f28
0000000180355659 call      0x1812dbb60
000000018035565E movaps    xmm8, xmmword ptr [rsp + 0x20]
0000000180355664 add       rsp, 0x50
0000000180355668 pop       rbx
0000000180355669 ret       
000000018035566A test      rcx, rcx
000000018035566D je        0x18035568a
000000018035566F xor       r9d, r9d
0000000180355672 mov       r8d, 1
0000000180355678 jmp       0x180355652
000000018035567A test      rcx, rcx
000000018035567D je        0x18035568a
000000018035567F xor       r9d, r9d
0000000180355682 mov       r8d, 2
0000000180355688 jmp       0x180355652
000000018035568A call      0x180250150
000000018035568F int3      