0000000180396480 mov       qword ptr [rsp + 0x18], rsi
0000000180396485 push      rdi
0000000180396486 sub       rsp, 0x40
000000018039648A cmp       byte ptr [rip + 0x194e9e8], 0 # 0x181ce4e79
0000000180396491 mov       rdi, rdx
0000000180396494 mov       rsi, rcx
0000000180396497 jne       0x1803964c4
0000000180396499 lea       rcx, [rip + 0x1812b70] # 0x181ba9010
00000001803964A0 call      0x18024fef0
00000001803964A5 lea       rcx, [rip + 0x1821544] # 0x181bb79f0
00000001803964AC call      0x18024fef0
00000001803964B1 lea       rcx, [rip + 0x181a048] # 0x181bb0500
00000001803964B8 call      0x18024fef0
00000001803964BD mov       byte ptr [rip + 0x194e9b5], 1 # 0x181ce4e79
00000001803964C4 mov       qword ptr [rsp + 0x50], rbx
00000001803964C9 mov       qword ptr [rsp + 0x58], rbp
00000001803964CE test      rdi, rdi
00000001803964D1 je        0x18039670d
00000001803964D7 mov       rax, qword ptr [rdi + 0x20]
00000001803964DB test      rax, rax
00000001803964DE je        0x18039670d
00000001803964E4 cmp       byte ptr [rip + 0x194e994], 0 # 0x181ce4e7f
00000001803964EB mov       ebx, dword ptr [rax + 0x1c]
00000001803964EE jne       0x18039651b
00000001803964F0 lea       rcx, [rip + 0x1812c79] # 0x181ba9170
00000001803964F7 call      0x18024fef0
00000001803964FC lea       rcx, [rip + 0x1812cc5] # 0x181ba91c8
0000000180396503 call      0x18024fef0
0000000180396508 lea       rcx, [rip + 0x1819ff1] # 0x181bb0500
000000018039650F call      0x18024fef0
0000000180396514 mov       byte ptr [rip + 0x194e964], 1 # 0x181ce4e7f
000000018039651B mov       rax, qword ptr [rsi + 0x50]
000000018039651F test      rax, rax
0000000180396522 je        0x18039670d
0000000180396528 xor       ebp, ebp
000000018039652A cmp       dword ptr [rax + 0x18], ebx
000000018039652D jl        0x180396568
000000018039652F mov       r8, qword ptr [rip + 0x1812c92] # 0x181ba91c8
0000000180396536 mov       edx, ebx
0000000180396538 mov       rcx, rax
000000018039653B call      0x180825710
0000000180396540 mov       rcx, qword ptr [rip + 0x1819fb9] # 0x181bb0500
0000000180396547 mov       rbx, rax
000000018039654A cmp       dword ptr [rcx + 0xe0], ebp
0000000180396550 jne       0x180396557
0000000180396552 call      0x1802501e0
0000000180396557 xor       r8d, r8d
000000018039655A xor       edx, edx
000000018039655C mov       rcx, rbx
000000018039655F call      0x18131f900
0000000180396564 test      al, al
0000000180396566 jne       0x18039656b
0000000180396568 mov       rbx, rbp
000000018039656B mov       rcx, qword ptr [rip + 0x1819f8e] # 0x181bb0500
0000000180396572 cmp       dword ptr [rcx + 0xe0], ebp
0000000180396578 jne       0x18039657f
000000018039657A call      0x1802501e0
000000018039657F xor       edx, edx
0000000180396581 mov       rcx, rbx
0000000180396584 call      0x18131f870
0000000180396589 test      al, al
000000018039658B je        0x1803966d4
0000000180396591 mov       rcx, qword ptr [rip + 0x1819f68] # 0x181bb0500
0000000180396598 cmp       dword ptr [rcx + 0xe0], ebp
000000018039659E jne       0x1803965a5
00000001803965A0 call      0x1802501e0
00000001803965A5 mov       rdx, qword ptr [rip + 0x1821444] # 0x181bb79f0
00000001803965AC mov       rcx, rbx
00000001803965AF call      0x1804a0690
00000001803965B4 mov       rbx, rax
00000001803965B7 test      rax, rax
00000001803965BA je        0x18039670d
00000001803965C0 lea       rcx, [rax + 0x28]
00000001803965C4 mov       qword ptr [rax + 0x28], rbp
00000001803965C8 xor       edx, edx
00000001803965CA call      0x18024f360
00000001803965CF mov       rdx, qword ptr [rdi + 0x20]
00000001803965D3 xor       r8d, r8d
00000001803965D6 mov       rcx, rbx
00000001803965D9 call      0x1803491f0
00000001803965DE mov       rdx, qword ptr [rsi + 0x20]
00000001803965E2 test      rdx, rdx
00000001803965E5 je        0x18039670d
00000001803965EB mov       r9d, dword ptr [rdi + 0x14]
00000001803965EF lea       rcx, [rsp + 0x30]
00000001803965F4 mov       r8d, dword ptr [rdi + 0x10]
00000001803965F8 mov       qword ptr [rsp + 0x20], rbp
00000001803965FD call      0x18030eeb0
0000000180396602 xor       r8d, r8d
0000000180396605 lea       rdx, [rsp + 0x30]
000000018039660A movsd     xmm1, qword ptr [rax]
000000018039660E mov       ecx, dword ptr [rax + 8]
0000000180396611 movaps    xmm0, xmm1
0000000180396614 subss     xmm0, dword ptr [rip + 0x121187c] # 0x1815a7e98
000000018039661C mov       dword ptr [rsp + 0x38], ecx
0000000180396620 mov       rcx, rbx
0000000180396623 movss     xmm1, xmm0
0000000180396627 movsd     qword ptr [rsp + 0x30], xmm1
000000018039662D movsd     qword ptr [rsp + 0x30], xmm1
0000000180396633 call      0x18034ab50
0000000180396638 xor       edx, edx
000000018039663A mov       rcx, rbx
000000018039663D call      0x181304510
0000000180396642 xor       edx, edx
0000000180396644 mov       rcx, rsi
0000000180396647 mov       rbp, rax
000000018039664A call      0x181304510
000000018039664F test      rbp, rbp
0000000180396652 je        0x18039670d
0000000180396658 xor       r8d, r8d
000000018039665B mov       rdx, rax
000000018039665E mov       rcx, rbp
0000000180396661 call      0x18132e8b0
0000000180396666 mov       eax, dword ptr [rdi + 0x10]
0000000180396669 xor       edx, edx
000000018039666B mov       dword ptr [rbx + 0x30], eax
000000018039666E mov       rcx, rbx
0000000180396671 mov       eax, dword ptr [rdi + 0x14]
0000000180396674 mov       dword ptr [rbx + 0x34], eax
0000000180396677 call      0x18034a300
000000018039667C xor       edx, edx
000000018039667E mov       rcx, rbx
0000000180396681 call      0x18034acc0
0000000180396686 xor       edx, edx
0000000180396688 mov       rcx, rbx
000000018039668B call      0x180349ff0
0000000180396690 mov       byte ptr [rbx + 0x94], 0
0000000180396697 mov       rcx, qword ptr [rsi + 0x48]
000000018039669B test      rcx, rcx
000000018039669E je        0x18039670d
00000001803966A0 mov       r9, qword ptr [rip + 0x1812969] # 0x181ba9010
00000001803966A7 inc       dword ptr [rcx + 0x1c]
00000001803966AA mov       rdx, qword ptr [rcx + 0x10]
00000001803966AE movsxd    r8, dword ptr [rcx + 0x18]
00000001803966B2 test      rdx, rdx
00000001803966B5 je        0x18039670d
00000001803966B7 cmp       r8d, dword ptr [rdx + 0x18]
00000001803966BB jb        0x1803966e9
00000001803966BD mov       rax, qword ptr [r9 + 0x20]
00000001803966C1 mov       rdx, rbx
00000001803966C4 mov       r8, qword ptr [rax + 0xc0]
00000001803966CB mov       r8, qword ptr [r8 + 0x70]
00000001803966CF call      0x180847ce0
00000001803966D4 mov       rbp, qword ptr [rsp + 0x58]
00000001803966D9 mov       rbx, qword ptr [rsp + 0x50]
00000001803966DE mov       rsi, qword ptr [rsp + 0x60]
00000001803966E3 add       rsp, 0x40
00000001803966E7 pop       rdi
00000001803966E8 ret       
00000001803966E9 lea       eax, [r8 + 1]
00000001803966ED mov       dword ptr [rcx + 0x18], eax
00000001803966F0 cmp       r8d, dword ptr [rdx + 0x18]
00000001803966F4 jae       0x180396713
00000001803966F6 mov       qword ptr [rdx + r8*8 + 0x20], rbx
00000001803966FB add       rdx, 0x20
00000001803966FF lea       rcx, [rdx + r8*8]
0000000180396703 mov       rdx, rbx
0000000180396706 call      0x18024f360
000000018039670B jmp       0x1803966d4
000000018039670D call      0x180250150
0000000180396712 int3      
0000000180396713 call      0x180250140
0000000180396718 int3      