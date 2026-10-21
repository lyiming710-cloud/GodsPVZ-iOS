0000000180338560 push      rbx
0000000180338562 push      rsi
0000000180338563 push      rdi
0000000180338564 sub       rsp, 0x50
0000000180338568 cmp       byte ptr [rip + 0x19ac6af], 0 # 0x181ce4c1e
000000018033856F jne       0x1803385f0
0000000180338571 lea       rcx, [rip + 0x1864f10] # 0x181b9d488
0000000180338578 call      0x18024fef0
000000018033857D lea       rcx, [rip + 0x1864b04] # 0x181b9d088
0000000180338584 call      0x18024fef0
0000000180338589 lea       rcx, [rip + 0x18666b8] # 0x181b9ec48
0000000180338590 call      0x18024fef0
0000000180338595 lea       rcx, [rip + 0x186c954] # 0x181ba4ef0
000000018033859C call      0x18024fef0
00000001803385A1 lea       rcx, [rip + 0x186cca8] # 0x181ba5250
00000001803385A8 call      0x18024fef0
00000001803385AD lea       rcx, [rip + 0x1870ed4] # 0x181ba9488
00000001803385B4 call      0x18024fef0
00000001803385B9 lea       rcx, [rip + 0x1870e70] # 0x181ba9430
00000001803385C0 call      0x18024fef0
00000001803385C5 lea       rcx, [rip + 0x1884e3c] # 0x181bbd408
00000001803385CC call      0x18024fef0
00000001803385D1 lea       rcx, [rip + 0x187dea0] # 0x181bb6478
00000001803385D8 call      0x18024fef0
00000001803385DD lea       rcx, [rip + 0x188770c] # 0x181bbfcf0
00000001803385E4 call      0x18024fef0
00000001803385E9 mov       byte ptr [rip + 0x19ac62e], 1 # 0x181ce4c1e
00000001803385F0 xor       esi, esi
00000001803385F2 mov       qword ptr [rsp + 0x80], rsi
00000001803385FA mov       rcx, qword ptr [rip + 0x1884e07] # 0x181bbd408
0000000180338601 call      0x180250100
0000000180338606 mov       rdi, rax
0000000180338609 mov       rdx, qword ptr [rip + 0x1870e20] # 0x181ba9430
0000000180338610 mov       rcx, rax
0000000180338613 call      0x1808053d0
0000000180338618 mov       qword ptr [rsp + 0x88], rdi
0000000180338620 mov       rbx, qword ptr [rip + 0x1864e61] # 0x181b9d488
0000000180338627 mov       rcx, qword ptr [rip + 0x18876c2] # 0x181bbfcf0
000000018033862E cmp       dword ptr [rcx + 0xe0], esi
0000000180338634 jne       0x18033863b
0000000180338636 call      0x1802501e0
000000018033863B xor       edx, edx
000000018033863D mov       rcx, rbx
0000000180338640 call      0x180ccee30
0000000180338645 mov       rbx, rax
0000000180338648 mov       rcx, qword ptr [rip + 0x18665f9] # 0x181b9ec48
000000018033864F cmp       dword ptr [rcx + 0xe0], esi
0000000180338655 jne       0x18033865c
0000000180338657 call      0x1802501e0
000000018033865C xor       edx, edx
000000018033865E mov       rcx, rbx
0000000180338661 call      0x180cdd3b0
0000000180338666 test      rax, rax
0000000180338669 je        0x18033888a
000000018033866F xor       edx, edx
0000000180338671 mov       rcx, rax
0000000180338674 call      0x180cba170
0000000180338679 mov       qword ptr [rsp + 0x78], rax
000000018033867E lea       rax, [rsp + 0x78]
0000000180338683 mov       qword ptr [rsp + 0x28], rax
0000000180338688 lea       rax, [rsp + 0x80]
0000000180338690 mov       qword ptr [rsp + 0x30], rax
0000000180338695 mov       qword ptr [rsp + 0x38], rsi
000000018033869A movups    xmm0, xmmword ptr [rsp + 0x28]
000000018033869F movups    xmmword ptr [rsp + 0x40], xmm0
00000001803386A4 nop       dword ptr [rax]
00000001803386A8 nop       dword ptr [rax + rax]
00000001803386B0 mov       r8, qword ptr [rsp + 0x78]
00000001803386B5 test      r8, r8
00000001803386B8 je        0x1803388b2
00000001803386BE xor       ecx, ecx
00000001803386C0 mov       rdx, qword ptr [rip + 0x186cb89] # 0x181ba5250
00000001803386C7 call      0x180002cd0
00000001803386CC test      al, al
00000001803386CE je        0x180338825
00000001803386D4 mov       rbx, qword ptr [rsp + 0x78]
00000001803386D9 test      rbx, rbx
00000001803386DC je        0x1803388ad
00000001803386E2 mov       r9, qword ptr [rip + 0x186cb67] # 0x181ba5250
00000001803386E9 mov       r10, qword ptr [rbx]
00000001803386EC movzx     ecx, si
00000001803386EF movzx     edx, word ptr [r10 + 0x12e]
00000001803386F7 cmp       si, dx
00000001803386FA jae       0x180338728
00000001803386FC mov       r8, qword ptr [r10 + 0xb0]
0000000180338703 nop       dword ptr [rax]
0000000180338707 nop       word ptr [rax + rax]
0000000180338710 movzx     eax, cx
0000000180338713 add       rax, rax
0000000180338716 cmp       qword ptr [r8 + rax*8], r9
000000018033871A je        0x1803387da
0000000180338720 inc       cx
0000000180338723 cmp       cx, dx
0000000180338726 jb        0x180338710
0000000180338728 mov       r8d, 1
000000018033872E mov       rdx, r9
0000000180338731 mov       rcx, rbx
0000000180338734 call      0x180258380
0000000180338739 mov       rdx, rax
000000018033873C mov       rax, qword ptr [rdx]
000000018033873F mov       rdx, qword ptr [rdx + 8]
0000000180338743 mov       rcx, rbx
0000000180338746 call      rax
0000000180338748 mov       rcx, rax
000000018033874B mov       r8, qword ptr [rip + 0x1864936] # 0x181b9d088
0000000180338752 test      rax, rax
0000000180338755 je        0x1803388a8
000000018033875B mov       rdx, qword ptr [rax]
000000018033875E mov       rax, qword ptr [r8 + 0x40]
0000000180338762 cmp       qword ptr [rdx + 0x40], rax
0000000180338766 jne       0x1803388a0
000000018033876C call      0x18024f540
0000000180338771 mov       ebx, dword ptr [rax]
0000000180338773 mov       rcx, qword ptr [rip + 0x187dcfe] # 0x181bb6478
000000018033877A cmp       dword ptr [rcx + 0xe0], 0
0000000180338781 jne       0x180338788
0000000180338783 call      0x1802501e0
0000000180338788 xor       edx, edx
000000018033878A mov       ecx, ebx
000000018033878C call      0x1803388c0
0000000180338791 mov       rdx, rax
0000000180338794 test      rdi, rdi
0000000180338797 je        0x18033889b
000000018033879D mov       r9, qword ptr [rip + 0x1870ce4] # 0x181ba9488
00000001803387A4 inc       dword ptr [rdi + 0x1c]
00000001803387A7 mov       rcx, qword ptr [rdi + 0x10]
00000001803387AB movsxd    r8, dword ptr [rdi + 0x18]
00000001803387AF test      rcx, rcx
00000001803387B2 je        0x180338896
00000001803387B8 cmp       r8d, dword ptr [rcx + 0x18]
00000001803387BC jb        0x1803387fd
00000001803387BE mov       rax, qword ptr [r9 + 0x20]
00000001803387C2 mov       r8, qword ptr [rax + 0xc0]
00000001803387C9 mov       r8, qword ptr [r8 + 0x70]
00000001803387CD mov       rcx, rdi
00000001803387D0 call      0x180847ce0
00000001803387D5 jmp       0x1803386b0
00000001803387DA movzx     ecx, cx
00000001803387DD add       rcx, rcx
00000001803387E0 mov       ecx, dword ptr [r8 + rcx*8 + 8]
00000001803387E5 inc       ecx
00000001803387E7 movsxd    rdx, ecx
00000001803387EA shl       rdx, 4
00000001803387EE add       rdx, 0x138
00000001803387F5 add       rdx, r10
00000001803387F8 jmp       0x18033873c
00000001803387FD lea       eax, [r8 + 1]
0000000180338801 mov       dword ptr [rdi + 0x18], eax
0000000180338804 cmp       r8d, dword ptr [rcx + 0x18]
0000000180338808 jae       0x180338890
000000018033880E mov       qword ptr [rcx + r8*8 + 0x20], rdx
0000000180338813 lea       rcx, [rcx + r8*8]
0000000180338817 add       rcx, 0x20
000000018033881B call      0x18024f360
0000000180338820 jmp       0x1803386b0
0000000180338825 mov       rax, qword ptr [rsp + 0x28]
000000018033882A mov       rdx, qword ptr [rip + 0x186c6bf] # 0x181ba4ef0
0000000180338831 mov       rcx, qword ptr [rax]
0000000180338834 call      0x18024f380
0000000180338839 mov       r8, rax
000000018033883C movups    xmm0, xmmword ptr [rsp + 0x28]
0000000180338841 psrldq    xmm0, 8
0000000180338846 movq      rax, xmm0
000000018033884B mov       qword ptr [rax], r8
000000018033884E test      r8, r8
0000000180338851 je        0x180338861
0000000180338853 xor       ecx, ecx
0000000180338855 mov       rdx, qword ptr [rip + 0x186c694] # 0x181ba4ef0
000000018033885C call      0x180002d90
0000000180338861 jmp       0x18033887f
0000000180338863 lea       rcx, [rsp + 0x40]
0000000180338868 call      0x1802fb000
000000018033886D mov       rcx, qword ptr [rsp + 0x38]
0000000180338872 test      rcx, rcx
0000000180338875 jne       0x1803388b8
0000000180338877 mov       rdi, qword ptr [rsp + 0x88]
000000018033887F mov       rax, rdi
0000000180338882 add       rsp, 0x50
0000000180338886 pop       rdi
0000000180338887 pop       rsi
0000000180338888 pop       rbx
0000000180338889 ret       
000000018033888A call      0x180250150
000000018033888F nop       
0000000180338890 call      0x180250140
0000000180338895 nop       
0000000180338896 call      0x180250150
000000018033889B call      0x180250150
00000001803388A0 mov       rdx, r8
00000001803388A3 call      0x18024f3a0
00000001803388A8 call      0x180250150
00000001803388AD call      0x180250150
00000001803388B2 call      0x180250150
00000001803388B7 nop       
00000001803388B8 call      0x180200e60
00000001803388BD int3      
00000001803388BE int3      
00000001803388BF int3      