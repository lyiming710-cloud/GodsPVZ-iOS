00000001803B2250 push      rbx
00000001803B2252 push      r14
00000001803B2254 push      r15
00000001803B2256 sub       rsp, 0x150
00000001803B225D cmp       byte ptr [rip + 0x1932cfb], 0 # 0x181ce4f5f
00000001803B2264 mov       rbx, rcx
00000001803B2267 jne       0x1803b22e8
00000001803B2269 lea       rcx, [rip + 0x1810e68] # 0x181bc30d8
00000001803B2270 call      0x18024fef0
00000001803B2275 lea       rcx, [rip + 0x17f179c] # 0x181ba3a18
00000001803B227C call      0x18024fef0
00000001803B2281 lea       rcx, [rip + 0x17f2150] # 0x181ba43d8
00000001803B2288 call      0x18024fef0
00000001803B228D lea       rcx, [rip + 0x17f21a4] # 0x181ba4438
00000001803B2294 call      0x18024fef0
00000001803B2299 lea       rcx, [rip + 0x17f20d8] # 0x181ba4378
00000001803B22A0 call      0x18024fef0
00000001803B22A5 lea       rcx, [rip + 0x17f206c] # 0x181ba4318
00000001803B22AC call      0x18024fef0
00000001803B22B1 lea       rcx, [rip + 0x17efa00] # 0x181ba1cb8
00000001803B22B8 call      0x18024fef0
00000001803B22BD lea       rcx, [rip + 0x180e70c] # 0x181bc09d0
00000001803B22C4 call      0x18024fef0
00000001803B22C9 lea       rcx, [rip + 0x1822700] # 0x181bd49d0
00000001803B22D0 call      0x18024fef0
00000001803B22D5 lea       rcx, [rip + 0x181386c] # 0x181bc5b48
00000001803B22DC call      0x18024fef0
00000001803B22E1 mov       byte ptr [rip + 0x1932c77], 1 # 0x181ce4f5f
00000001803B22E8 xor       r15d, r15d
00000001803B22EB mov       r14d, r15d
00000001803B22EE cmp       dword ptr [rbx + 0x24], r15d
00000001803B22F2 jle       0x1803b2ab8
00000001803B22F8 movaps    xmmword ptr [rsp + 0x130], xmm6
00000001803B2300 movss     xmm6, dword ptr [rip + 0x11f5b20] # 0x1815a7e28
00000001803B2308 movaps    xmmword ptr [rsp + 0x120], xmm7
00000001803B2310 movss     xmm7, dword ptr [rip + 0x11f5d20] # 0x1815a8038
00000001803B2318 movaps    xmmword ptr [rsp + 0x100], xmm9
00000001803B2321 movss     xmm9, dword ptr [rip + 0x11f5c62] # 0x1815a7f8c
00000001803B232A movaps    xmmword ptr [rsp + 0xf0], xmm10
00000001803B2333 movss     xmm10, dword ptr [rip + 0x11f58cc] # 0x1815a7c08
00000001803B233C movaps    xmmword ptr [rsp + 0xe0], xmm11
00000001803B2345 movss     xmm11, dword ptr [rip + 0x11f5762] # 0x1815a7ab0
00000001803B234E movaps    xmmword ptr [rsp + 0xd0], xmm12
00000001803B2357 movss     xmm12, dword ptr [rip + 0x11f5880] # 0x1815a7be0
00000001803B2360 movaps    xmmword ptr [rsp + 0xc0], xmm13
00000001803B2369 movss     xmm13, dword ptr [rip + 0x11f5ca6] # 0x1815a8018
00000001803B2372 movaps    xmmword ptr [rsp + 0xb0], xmm14
00000001803B237B movss     xmm14, dword ptr [rip + 0x11f5684] # 0x1815a7a08
00000001803B2384 movaps    xmmword ptr [rsp + 0xa0], xmm15
00000001803B238D xorps     xmm15, xmm15
00000001803B2391 mov       qword ptr [rsp + 0x178], rbp
00000001803B2399 mov       qword ptr [rsp + 0x148], rsi
00000001803B23A1 mov       qword ptr [rsp + 0x140], rdi
00000001803B23A9 movaps    xmmword ptr [rsp + 0x110], xmm8
00000001803B23B2 cmp       dword ptr [rbx + 0x20], r15d
00000001803B23B6 je        0x1803b2826
00000001803B23BC cmp       dword ptr [rbx + 0x20], 1
00000001803B23C0 je        0x1803b262b
00000001803B23C6 cmp       dword ptr [rbx + 0x20], 2
00000001803B23CA jne       0x1803b2a3b
00000001803B23D0 mov       rcx, qword ptr [rip + 0x17ef8e1] # 0x181ba1cb8
00000001803B23D7 call      0x180250100
00000001803B23DC xor       edx, edx
00000001803B23DE mov       rcx, rax
00000001803B23E1 mov       rdi, rax
00000001803B23E4 call      0x18131be60
00000001803B23E9 test      rdi, rdi
00000001803B23EC je        0x1803b2ac5
00000001803B23F2 mov       rdx, qword ptr [rip + 0x17f161f] # 0x181ba3a18
00000001803B23F9 mov       rcx, rdi
00000001803B23FC call      0x18046d280
00000001803B2401 xor       ecx, ecx
00000001803B2403 mov       rsi, rax
00000001803B2406 call      0x1812e27c0
00000001803B240B test      rsi, rsi
00000001803B240E je        0x1803b2ac5
00000001803B2414 xor       r8d, r8d
00000001803B2417 mov       rdx, rax
00000001803B241A mov       rcx, rsi
00000001803B241D call      0x181546c40
00000001803B2422 xor       edx, edx
00000001803B2424 mov       rcx, rdi
00000001803B2427 call      0x18131c070
00000001803B242C test      rax, rax
00000001803B242F je        0x1803b2ac5
00000001803B2435 movaps    xmm0, xmm10
00000001803B2439 movss     dword ptr [rsp + 0x28], xmm10
00000001803B2440 unpcklps  xmm0, xmm10
00000001803B2444 lea       rdx, [rsp + 0x20]
00000001803B2449 xor       r8d, r8d
00000001803B244C movsd     qword ptr [rsp + 0x20], xmm0
00000001803B2452 mov       rcx, rax
00000001803B2455 call      0x18132fa50
00000001803B245A xor       edx, edx
00000001803B245C mov       rcx, rdi
00000001803B245F call      0x18131c070
00000001803B2464 xor       r8d, r8d
00000001803B2467 movaps    xmm1, xmm6
00000001803B246A movaps    xmm0, xmm7
00000001803B246D mov       rsi, rax
00000001803B2470 call      0x18130d350
00000001803B2475 movaps    xmm8, xmm0
00000001803B2479 xor       r8d, r8d
00000001803B247C movaps    xmm0, xmm7
00000001803B247F movaps    xmm1, xmm6
00000001803B2482 call      0x18130d350
00000001803B2487 test      rsi, rsi
00000001803B248A je        0x1803b2ac5
00000001803B2490 movaps    xmm1, xmm8
00000001803B2494 movss     dword ptr [rsp + 0x38], xmm0
00000001803B249A unpcklps  xmm1, xmm12
00000001803B249E lea       rdx, [rsp + 0x30]
00000001803B24A3 xor       r8d, r8d
00000001803B24A6 movsd     qword ptr [rsp + 0x30], xmm1
00000001803B24AC mov       rcx, rsi
00000001803B24AF call      0x18132fbb0
00000001803B24B4 mov       rcx, qword ptr [rip + 0x17ef7fd] # 0x181ba1cb8
00000001803B24BB call      0x180250100
00000001803B24C0 xor       edx, edx
00000001803B24C2 mov       rcx, rax
00000001803B24C5 mov       rsi, rax
00000001803B24C8 call      0x18131be60
00000001803B24CD test      rsi, rsi
00000001803B24D0 je        0x1803b2ac5
00000001803B24D6 mov       rdx, qword ptr [rip + 0x17f1f5b] # 0x181ba4438
00000001803B24DD mov       rcx, rsi
00000001803B24E0 call      0x18046d280
00000001803B24E5 mov       rsi, rax
00000001803B24E8 test      rax, rax
00000001803B24EB je        0x1803b2ac5
00000001803B24F1 xor       edx, edx
00000001803B24F3 mov       rcx, rax
00000001803B24F6 call      0x1811b84f0
00000001803B24FB xor       edx, edx
00000001803B24FD mov       rcx, rdi
00000001803B2500 mov       rbp, rax
00000001803B2503 call      0x18131c070
00000001803B2508 test      rbp, rbp
00000001803B250B je        0x1803b2ac5
00000001803B2511 xor       r9d, r9d
00000001803B2514 xor       r8d, r8d
00000001803B2517 mov       rdx, rax
00000001803B251A mov       rcx, rbp
00000001803B251D call      0x18132e900
00000001803B2522 mov       dword ptr [rsp + 0x170], 0xff00ffff
00000001803B252D mov       eax, dword ptr [rsp + 0x170]
00000001803B2534 mov       ecx, eax
00000001803B2536 shr       ecx, 8
00000001803B2539 movzx     ecx, cl
00000001803B253C movd      xmm3, ecx
00000001803B2540 mov       ecx, eax
00000001803B2542 shr       ecx, 0x10
00000001803B2545 movzx     ecx, cl
00000001803B2548 shr       eax, 0x18
00000001803B254B cvtdq2ps  xmm3, xmm3
00000001803B254E movd      xmm1, ecx
00000001803B2552 movd      xmm2, eax
00000001803B2556 cvtdq2ps  xmm1, xmm1
00000001803B2559 cvtdq2ps  xmm2, xmm2
00000001803B255C divss     xmm1, xmm9
00000001803B2561 divss     xmm2, xmm9
00000001803B2566 divss     xmm3, xmm9
00000001803B256B mov       rax, qword ptr [rsi]
00000001803B256E lea       rdx, [rsp + 0x90]
00000001803B2576 mov       dword ptr [rsp + 0x60], 0x3f800000
00000001803B257E mov       rcx, rsi
00000001803B2581 movaps    xmm0, xmmword ptr [rsp + 0x60]
00000001803B2586 shufps    xmm0, xmm0, 0xe1
00000001803B258A mov       r8, qword ptr [rax + 0x2b0]
00000001803B2591 movss     xmm0, xmm3
00000001803B2595 shufps    xmm0, xmm0, 0xc6
00000001803B2599 movss     xmm0, xmm1
00000001803B259D shufps    xmm0, xmm0, 0x27
00000001803B25A1 movss     xmm0, xmm2
00000001803B25A5 shufps    xmm0, xmm0, 0x39
00000001803B25A9 movaps    xmmword ptr [rsp + 0x60], xmm0
00000001803B25AE movdqa    xmmword ptr [rsp + 0x90], xmm0
00000001803B25B7 call      qword ptr [rax + 0x2a8]
00000001803B25BD xor       r8d, r8d
00000001803B25C0 mov       edx, 0x402
00000001803B25C5 mov       rcx, rsi
00000001803B25C8 call      0x1811b8c90
00000001803B25CD xor       r8d, r8d
00000001803B25D0 movaps    xmm1, xmm13
00000001803B25D4 mov       rcx, rsi
00000001803B25D7 call      0x1811b9410
00000001803B25DC mov       rax, qword ptr [rsi]
00000001803B25DF mov       rcx, rsi
00000001803B25E2 mov       rdx, qword ptr [rip + 0x18223e7] # 0x181bd49d0
00000001803B25E9 mov       r8, qword ptr [rax + 0x560]
00000001803B25F0 call      qword ptr [rax + 0x558]
00000001803B25F6 mov       rdx, qword ptr [rip + 0x17f1ddb] # 0x181ba43d8
00000001803B25FD mov       rcx, rdi
00000001803B2600 call      0x18046d280
00000001803B2605 lea       rcx, [rbx + 0x30]
00000001803B2609 mov       qword ptr [rbx + 0x30], rax
00000001803B260D mov       rdx, rax
00000001803B2610 call      0x18024f360
00000001803B2615 mov       rax, qword ptr [rbx + 0x30]
00000001803B2619 test      rax, rax
00000001803B261C je        0x1803b2ac5
00000001803B2622 mov       dword ptr [rax + 0x74], r15d
00000001803B2626 jmp       0x1803b2a3b
00000001803B262B mov       rcx, qword ptr [rip + 0x17ef686] # 0x181ba1cb8
00000001803B2632 call      0x180250100
00000001803B2637 xor       edx, edx
00000001803B2639 mov       rcx, rax
00000001803B263C mov       rsi, rax
00000001803B263F call      0x18131be60
00000001803B2644 test      rsi, rsi
00000001803B2647 je        0x1803b2ac5
00000001803B264D xor       edx, edx
00000001803B264F mov       rcx, rsi
00000001803B2652 call      0x18131c070
00000001803B2657 xor       r8d, r8d
00000001803B265A movaps    xmm1, xmm6
00000001803B265D movaps    xmm0, xmm7
00000001803B2660 mov       rdi, rax
00000001803B2663 call      0x18130d350
00000001803B2668 movaps    xmm8, xmm0
00000001803B266C xor       r8d, r8d
00000001803B266F movaps    xmm0, xmm7
00000001803B2672 movaps    xmm1, xmm6
00000001803B2675 call      0x18130d350
00000001803B267A test      rdi, rdi
00000001803B267D je        0x1803b2ac5
00000001803B2683 movaps    xmm1, xmm8
00000001803B2687 movss     dword ptr [rsp + 0x48], xmm0
00000001803B268D unpcklps  xmm1, xmm11
00000001803B2691 lea       rdx, [rsp + 0x40]
00000001803B2696 xor       r8d, r8d
00000001803B2699 movsd     qword ptr [rsp + 0x40], xmm1
00000001803B269F mov       rcx, rdi
00000001803B26A2 call      0x18132fbb0
00000001803B26A7 mov       rdx, qword ptr [rip + 0x17f1c6a] # 0x181ba4318
00000001803B26AE mov       rcx, rsi
00000001803B26B1 call      0x18046d280
00000001803B26B6 mov       rdx, qword ptr [rip + 0x180e313] # 0x181bc09d0
00000001803B26BD mov       rdi, rax
00000001803B26C0 mov       rcx, qword ptr [rip + 0x1813481] # 0x181bc5b48
00000001803B26C7 call      0x1804a6ab0
00000001803B26CC test      rdi, rdi
00000001803B26CF je        0x1803b2ac5
00000001803B26D5 xor       r8d, r8d
00000001803B26D8 mov       rdx, rax
00000001803B26DB mov       rcx, rdi
00000001803B26DE call      0x1813bb9e0
00000001803B26E3 mov       rdx, qword ptr [rip + 0x18109ee] # 0x181bc30d8
00000001803B26EA mov       rcx, rdi
00000001803B26ED call      0x18042e9f0
00000001803B26F2 xor       edx, edx
00000001803B26F4 mov       rcx, rdi
00000001803B26F7 mov       rbp, rax
00000001803B26FA call      0x1813bb880
00000001803B26FF test      rax, rax
00000001803B2702 je        0x1803b2ac5
00000001803B2708 xor       edx, edx
00000001803B270A mov       rcx, rax
00000001803B270D call      0x1813b9880
00000001803B2712 test      rbp, rbp
00000001803B2715 je        0x1803b2ac5
00000001803B271B xor       r8d, r8d
00000001803B271E mov       rdx, rax
00000001803B2721 mov       rcx, rbp
00000001803B2724 call      0x1812fb060
00000001803B2729 xor       r8d, r8d
00000001803B272C mov       edx, 7
00000001803B2731 mov       rcx, rdi
00000001803B2734 call      0x1813bb8c0
00000001803B2739 xor       r8d, r8d
00000001803B273C mov       edx, 0x60
00000001803B2741 mov       rcx, rdi
00000001803B2744 call      0x1813bb9a0
00000001803B2749 mov       dword ptr [rsp + 0x180], 0xff00ffff
00000001803B2754 mov       eax, dword ptr [rsp + 0x180]
00000001803B275B mov       ecx, eax
00000001803B275D shr       ecx, 8
00000001803B2760 movzx     ecx, cl
00000001803B2763 movd      xmm3, ecx
00000001803B2767 mov       ecx, eax
00000001803B2769 shr       ecx, 0x10
00000001803B276C movzx     ecx, cl
00000001803B276F shr       eax, 0x18
00000001803B2772 cvtdq2ps  xmm3, xmm3
00000001803B2775 movd      xmm1, ecx
00000001803B2779 movd      xmm2, eax
00000001803B277D cvtdq2ps  xmm1, xmm1
00000001803B2780 cvtdq2ps  xmm2, xmm2
00000001803B2783 divss     xmm1, xmm9
00000001803B2788 divss     xmm2, xmm9
00000001803B278D divss     xmm3, xmm9
00000001803B2792 mov       dword ptr [rsp + 0x70], 0x3f800000
00000001803B279A lea       rdx, [rsp + 0x90]
00000001803B27A2 movups    xmm0, xmmword ptr [rsp + 0x70]
00000001803B27A7 xor       r8d, r8d
00000001803B27AA mov       rcx, rdi
00000001803B27AD shufps    xmm0, xmm0, 0xe1
00000001803B27B1 movss     xmm0, xmm3
00000001803B27B5 shufps    xmm0, xmm0, 0xc6
00000001803B27B9 movss     xmm0, xmm1
00000001803B27BD shufps    xmm0, xmm0, 0x27
00000001803B27C1 movss     xmm0, xmm2
00000001803B27C5 shufps    xmm0, xmm0, 0x39
00000001803B27C9 movups    xmmword ptr [rsp + 0x70], xmm0
00000001803B27CE movdqa    xmmword ptr [rsp + 0x90], xmm0
00000001803B27D7 call      0x1813bb950
00000001803B27DC mov       rdx, qword ptr [rip + 0x18221ed] # 0x181bd49d0
00000001803B27E3 xor       r8d, r8d
00000001803B27E6 mov       rcx, rdi
00000001803B27E9 call      0x1813bba30
00000001803B27EE mov       rdx, qword ptr [rip + 0x17f1be3] # 0x181ba43d8
00000001803B27F5 mov       rcx, rsi
00000001803B27F8 call      0x18046d280
00000001803B27FD lea       rcx, [rbx + 0x30]
00000001803B2801 mov       qword ptr [rbx + 0x30], rax
00000001803B2805 mov       rdx, rax
00000001803B2808 call      0x18024f360
00000001803B280D mov       rax, qword ptr [rbx + 0x30]
00000001803B2811 test      rax, rax
00000001803B2814 je        0x1803b2ac5
00000001803B281A mov       dword ptr [rax + 0x74], 1
00000001803B2821 jmp       0x1803b2a3b
00000001803B2826 mov       rcx, qword ptr [rip + 0x17ef48b] # 0x181ba1cb8
00000001803B282D call      0x180250100
00000001803B2832 xor       edx, edx
00000001803B2834 mov       rcx, rax
00000001803B2837 mov       rsi, rax
00000001803B283A call      0x18131be60
00000001803B283F test      rsi, rsi
00000001803B2842 je        0x1803b2ac5
00000001803B2848 xor       edx, edx
00000001803B284A mov       rcx, rsi
00000001803B284D call      0x18131c070
00000001803B2852 xor       r8d, r8d
00000001803B2855 movaps    xmm1, xmm6
00000001803B2858 movaps    xmm0, xmm7
00000001803B285B mov       rdi, rax
00000001803B285E call      0x18130d350
00000001803B2863 movaps    xmm8, xmm0
00000001803B2867 xor       r8d, r8d
00000001803B286A movaps    xmm0, xmm7
00000001803B286D movaps    xmm1, xmm6
00000001803B2870 call      0x18130d350
00000001803B2875 test      rdi, rdi
00000001803B2878 je        0x1803b2ac5
00000001803B287E movaps    xmm1, xmm8
00000001803B2882 movss     dword ptr [rsp + 0x58], xmm0
00000001803B2888 unpcklps  xmm1, xmm11
00000001803B288C lea       rdx, [rsp + 0x50]
00000001803B2891 xor       r8d, r8d
00000001803B2894 movsd     qword ptr [rsp + 0x50], xmm1
00000001803B289A mov       rcx, rdi
00000001803B289D call      0x18132fbb0
00000001803B28A2 mov       rdx, qword ptr [rip + 0x17f1acf] # 0x181ba4378
00000001803B28A9 mov       rcx, rsi
00000001803B28AC call      0x18046d280
00000001803B28B1 mov       rdi, rax
00000001803B28B4 test      rax, rax
00000001803B28B7 je        0x1803b2ac5
00000001803B28BD mov       r9, qword ptr [rax]
00000001803B28C0 mov       dl, 1
00000001803B28C2 mov       rcx, rax
00000001803B28C5 mov       r8, qword ptr [r9 + 0x600]
00000001803B28CC call      qword ptr [r9 + 0x5f8]
00000001803B28D3 xor       edx, edx
00000001803B28D5 mov       rcx, rdi
00000001803B28D8 call      0x1811b84f0
00000001803B28DD test      rax, rax
00000001803B28E0 je        0x1803b2ac5
00000001803B28E6 movaps    xmm0, xmm14
00000001803B28EA xor       r8d, r8d
00000001803B28ED unpcklps  xmm0, xmm15
00000001803B28F1 mov       rcx, rax
00000001803B28F4 movq      rdx, xmm0
00000001803B28F9 call      0x1813252d0
00000001803B28FE xor       r8d, r8d
00000001803B2901 mov       edx, 0x402
00000001803B2906 mov       rcx, rdi
00000001803B2909 call      0x1811b8c90
00000001803B290E xor       r8d, r8d
00000001803B2911 movaps    xmm1, xmm13
00000001803B2915 mov       rcx, rdi
00000001803B2918 call      0x1811b9410
00000001803B291D xor       r8d, r8d
00000001803B2920 xor       edx, edx
00000001803B2922 mov       rcx, rdi
00000001803B2925 call      0x1811b8f80
00000001803B292A mov       dword ptr [rsp + 0x188], 0xff00ffff
00000001803B2935 mov       eax, dword ptr [rsp + 0x188]
00000001803B293C mov       ecx, eax
00000001803B293E shr       ecx, 8
00000001803B2941 movzx     ecx, cl
00000001803B2944 movd      xmm3, ecx
00000001803B2948 mov       ecx, eax
00000001803B294A shr       ecx, 0x10
00000001803B294D movzx     ecx, cl
00000001803B2950 shr       eax, 0x18
00000001803B2953 cvtdq2ps  xmm3, xmm3
00000001803B2956 movd      xmm1, ecx
00000001803B295A movd      xmm2, eax
00000001803B295E cvtdq2ps  xmm1, xmm1
00000001803B2961 cvtdq2ps  xmm2, xmm2
00000001803B2964 divss     xmm1, xmm9
00000001803B2969 divss     xmm2, xmm9
00000001803B296E divss     xmm3, xmm9
00000001803B2973 mov       rax, qword ptr [rdi]
00000001803B2976 lea       rdx, [rsp + 0x90]
00000001803B297E mov       dword ptr [rsp + 0x80], 0x3f800000
00000001803B2989 mov       rcx, rdi
00000001803B298C movaps    xmm0, xmmword ptr [rsp + 0x80]
00000001803B2994 shufps    xmm0, xmm0, 0xe1
00000001803B2998 mov       r8, qword ptr [rax + 0x2b0]
00000001803B299F movss     xmm0, xmm3
00000001803B29A3 shufps    xmm0, xmm0, 0xc6
00000001803B29A7 movss     xmm0, xmm1
00000001803B29AB shufps    xmm0, xmm0, 0x27
00000001803B29AF movss     xmm0, xmm2
00000001803B29B3 shufps    xmm0, xmm0, 0x39
00000001803B29B7 movaps    xmmword ptr [rsp + 0x80], xmm0
00000001803B29BF movdqa    xmmword ptr [rsp + 0x90], xmm0
00000001803B29C8 call      qword ptr [rax + 0x2a8]
00000001803B29CE mov       rax, qword ptr [rdi]
00000001803B29D1 mov       rcx, rdi
00000001803B29D4 mov       rdx, qword ptr [rip + 0x1821ff5] # 0x181bd49d0
00000001803B29DB mov       r8, qword ptr [rax + 0x560]
00000001803B29E2 call      qword ptr [rax + 0x558]
00000001803B29E8 movzx     edx, byte ptr [rbx + 0x28]
00000001803B29EC xor       r8d, r8d
00000001803B29EF mov       rcx, rdi
00000001803B29F2 call      0x1811b9780
00000001803B29F7 mov       rdx, qword ptr [rip + 0x17f19da] # 0x181ba43d8
00000001803B29FE mov       rcx, rsi
00000001803B2A01 call      0x18046d280
00000001803B2A06 lea       rcx, [rbx + 0x30]
00000001803B2A0A mov       qword ptr [rbx + 0x30], rax
00000001803B2A0E mov       rdx, rax
00000001803B2A11 call      0x18024f360
00000001803B2A16 mov       rax, qword ptr [rbx + 0x30]
00000001803B2A1A test      rax, rax
00000001803B2A1D je        0x1803b2ac5
00000001803B2A23 mov       dword ptr [rax + 0x74], r15d
00000001803B2A27 mov       rcx, qword ptr [rbx + 0x30]
00000001803B2A2B test      rcx, rcx
00000001803B2A2E je        0x1803b2ac5
00000001803B2A34 movzx     eax, byte ptr [rbx + 0x28]
00000001803B2A38 mov       byte ptr [rcx + 0x78], al
00000001803B2A3B inc       r14d
00000001803B2A3E cmp       r14d, dword ptr [rbx + 0x24]
00000001803B2A42 jl        0x1803b23b2
00000001803B2A48 mov       rsi, qword ptr [rsp + 0x148]
00000001803B2A50 mov       rdi, qword ptr [rsp + 0x140]
00000001803B2A58 movaps    xmm8, xmmword ptr [rsp + 0x110]
00000001803B2A61 mov       rbp, qword ptr [rsp + 0x178]
00000001803B2A69 movaps    xmm10, xmmword ptr [rsp + 0xf0]
00000001803B2A72 movaps    xmm9, xmmword ptr [rsp + 0x100]
00000001803B2A7B movaps    xmm11, xmmword ptr [rsp + 0xe0]
00000001803B2A84 movaps    xmm7, xmmword ptr [rsp + 0x120]
00000001803B2A8C movaps    xmm6, xmmword ptr [rsp + 0x130]
00000001803B2A94 movaps    xmm12, xmmword ptr [rsp + 0xd0]
00000001803B2A9D movaps    xmm13, xmmword ptr [rsp + 0xc0]
00000001803B2AA6 movaps    xmm14, xmmword ptr [rsp + 0xb0]
00000001803B2AAF movaps    xmm15, xmmword ptr [rsp + 0xa0]
00000001803B2AB8 add       rsp, 0x150
00000001803B2ABF pop       r15
00000001803B2AC1 pop       r14
00000001803B2AC3 pop       rbx
00000001803B2AC4 ret       
00000001803B2AC5 call      0x180250150
00000001803B2ACA int3      
00000001803B2ACB int3      
00000001803B2ACC int3      
00000001803B2ACD int3      
00000001803B2ACE int3      
00000001803B2ACF int3      