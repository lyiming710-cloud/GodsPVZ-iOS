00000001803A0470 mov       qword ptr [rsp + 0x18], rbx
00000001803A0475 push      rdi
00000001803A0476 sub       rsp, 0x20
00000001803A047A cmp       byte ptr [rip + 0x1944a43], 0 # 0x181ce4ec4
00000001803A0481 mov       rbx, rcx
00000001803A0484 jne       0x1803a04d5
00000001803A0486 lea       rcx, [rip + 0x17fa8bb] # 0x181b9ad48
00000001803A048D call      0x18024fef0
00000001803A0492 lea       rcx, [rip + 0x1801b7f] # 0x181ba2018
00000001803A0499 call      0x18024fef0
00000001803A049E lea       rcx, [rip + 0x1817eeb] # 0x181bb8390
00000001803A04A5 call      0x18024fef0
00000001803A04AA lea       rcx, [rip + 0x181004f] # 0x181bb0500
00000001803A04B1 call      0x18024fef0
00000001803A04B6 lea       rcx, [rip + 0x1815fbb] # 0x181bb6478
00000001803A04BD call      0x18024fef0
00000001803A04C2 lea       rcx, [rip + 0x1800567] # 0x181ba0a30
00000001803A04C9 call      0x18024fef0
00000001803A04CE mov       byte ptr [rip + 0x19449ef], 1 # 0x181ce4ec4
00000001803A04D5 mov       rcx, qword ptr [rip + 0x1810024] # 0x181bb0500
00000001803A04DC mov       rdi, qword ptr [rbx + 0x48]
00000001803A04E0 cmp       dword ptr [rcx + 0xe0], 0
00000001803A04E7 jne       0x1803a04ee
00000001803A04E9 call      0x1802501e0
00000001803A04EE mov       qword ptr [rsp + 0x30], rbp
00000001803A04F3 xor       edx, edx
00000001803A04F5 mov       rcx, rdi
00000001803A04F8 mov       qword ptr [rsp + 0x38], rsi
00000001803A04FD call      0x18131f870
00000001803A0502 test      al, al
00000001803A0504 jne       0x1803a05c8
00000001803A050A mov       rax, qword ptr [rip + 0x1815f67] # 0x181bb6478
00000001803A0511 cmp       dword ptr [rax + 0xe0], 0
00000001803A0518 jne       0x1803a0529
00000001803A051A mov       rcx, rax
00000001803A051D call      0x1802501e0
00000001803A0522 mov       rax, qword ptr [rip + 0x1815f4f] # 0x181bb6478
00000001803A0529 mov       rcx, qword ptr [rip + 0x180ffd0] # 0x181bb0500
00000001803A0530 mov       rax, qword ptr [rax + 0xb8]
00000001803A0537 cmp       dword ptr [rcx + 0xe0], 0
00000001803A053E mov       rdi, qword ptr [rax + 0x1e0]
00000001803A0545 jne       0x1803a054c
00000001803A0547 call      0x1802501e0
00000001803A054C xor       edx, edx
00000001803A054E mov       rcx, rdi
00000001803A0551 call      0x18131f870
00000001803A0556 test      al, al
00000001803A0558 jne       0x1803a0592
00000001803A055A mov       rcx, qword ptr [rip + 0x17fa7e7] # 0x181b9ad48
00000001803A0561 cmp       dword ptr [rcx + 0xe0], 0
00000001803A0568 jne       0x1803a056f
00000001803A056A call      0x1802501e0
00000001803A056F mov       rcx, qword ptr [rip + 0x18004ba] # 0x181ba0a30
00000001803A0576 xor       edx, edx
00000001803A0578 call      0x1812e68f0
00000001803A057D mov       rsi, qword ptr [rsp + 0x38]
00000001803A0582 mov       rbp, qword ptr [rsp + 0x30]
00000001803A0587 mov       rbx, qword ptr [rsp + 0x40]
00000001803A058C add       rsp, 0x20
00000001803A0590 pop       rdi
00000001803A0591 ret       
00000001803A0592 mov       rcx, qword ptr [rip + 0x180ff67] # 0x181bb0500
00000001803A0599 cmp       dword ptr [rcx + 0xe0], 0
00000001803A05A0 jne       0x1803a05a7
00000001803A05A2 call      0x1802501e0
00000001803A05A7 mov       rdx, qword ptr [rip + 0x1817de2] # 0x181bb8390
00000001803A05AE mov       rcx, rdi
00000001803A05B1 call      0x1804a0690
00000001803A05B6 lea       rcx, [rbx + 0x48]
00000001803A05BA mov       qword ptr [rbx + 0x48], rax
00000001803A05BE mov       rdx, rax
00000001803A05C1 call      0x18024f360
00000001803A05C6 jmp       0x1803a05d8
00000001803A05C8 xor       r8d, r8d
00000001803A05CB mov       edx, 1
00000001803A05D0 mov       rcx, rbx
00000001803A05D3 call      0x18039fee0
00000001803A05D8 mov       rcx, qword ptr [rbx + 0x48]
00000001803A05DC test      rcx, rcx
00000001803A05DF je        0x1803a074e
00000001803A05E5 xor       edx, edx
00000001803A05E7 call      0x1813044d0
00000001803A05EC test      rax, rax
00000001803A05EF je        0x1803a074e
00000001803A05F5 xor       r8d, r8d
00000001803A05F8 mov       dl, 1
00000001803A05FA mov       rcx, rax
00000001803A05FD call      0x18131bad0
00000001803A0602 mov       rcx, qword ptr [rbx + 0x48]
00000001803A0606 test      rcx, rcx
00000001803A0609 je        0x1803a074e
00000001803A060F mov       qword ptr [rcx + 0x28], rbx
00000001803A0613 mov       rdx, rbx
00000001803A0616 add       rcx, 0x28
00000001803A061A call      0x18024f360
00000001803A061F mov       rax, qword ptr [rip + 0x18019f2] # 0x181ba2018
00000001803A0626 mov       rdi, qword ptr [rbx + 0x48]
00000001803A062A cmp       dword ptr [rax + 0xe0], 0
00000001803A0631 jne       0x1803a0642
00000001803A0633 mov       rcx, rax
00000001803A0636 call      0x1802501e0
00000001803A063B mov       rax, qword ptr [rip + 0x18019d6] # 0x181ba2018
00000001803A0642 mov       rax, qword ptr [rax + 0xb8]
00000001803A0649 mov       rax, qword ptr [rax]
00000001803A064C test      rax, rax
00000001803A064F je        0x1803a074e
00000001803A0655 mov       rax, qword ptr [rax + 0x28]
00000001803A0659 test      rax, rax
00000001803A065C je        0x1803a074e
00000001803A0662 mov       rbp, qword ptr [rax + 0x10]
00000001803A0666 test      rdi, rdi
00000001803A0669 je        0x1803a074e
00000001803A066F cmp       byte ptr [rip + 0x19448af], 0 # 0x181ce4f25
00000001803A0676 jne       0x1803a068b
00000001803A0678 lea       rcx, [rip + 0x18000d1] # 0x181ba0750
00000001803A067F call      0x18024fef0
00000001803A0684 mov       byte ptr [rip + 0x194489a], 1 # 0x181ce4f25
00000001803A068B xor       edx, edx
00000001803A068D mov       rcx, rdi
00000001803A0690 call      0x1803ae060
00000001803A0695 mov       rsi, qword ptr [rdi + 0x40]
00000001803A0699 test      rsi, rsi
00000001803A069C je        0x1803a074e
00000001803A06A2 cmp       byte ptr [rip + 0x1944880], 0 # 0x181ce4f29
00000001803A06A9 jne       0x1803a06be
00000001803A06AB lea       rcx, [rip + 0x1815dc6] # 0x181bb6478
00000001803A06B2 call      0x18024fef0
00000001803A06B7 mov       byte ptr [rip + 0x194486b], 1 # 0x181ce4f29
00000001803A06BE lea       rcx, [rsi + 0x20]
00000001803A06C2 mov       qword ptr [rsi + 0x20], 0
00000001803A06CA xor       edx, edx
00000001803A06CC call      0x18024f360
00000001803A06D1 lea       rcx, [rsi + 0x28]
00000001803A06D5 xor       edx, edx
00000001803A06D7 mov       qword ptr [rcx], 0
00000001803A06DE call      0x18024f360
00000001803A06E3 xor       edx, edx
00000001803A06E5 mov       rcx, rsi
00000001803A06E8 call      0x1803ae670
00000001803A06ED lea       rcx, [rdi + 0x20]
00000001803A06F1 mov       qword ptr [rdi + 0x20], rbp
00000001803A06F5 mov       rdx, rbp
00000001803A06F8 call      0x18024f360
00000001803A06FD mov       rsi, qword ptr [rdi + 0x48]
00000001803A0701 test      rbp, rbp
00000001803A0704 je        0x1803a074e
00000001803A0706 mov       rdx, qword ptr [rip + 0x1800043] # 0x181ba0750
00000001803A070D lea       rcx, [rbp + 0x70]
00000001803A0711 xor       r8d, r8d
00000001803A0714 call      0x180ca3930
00000001803A0719 test      rsi, rsi
00000001803A071C je        0x1803a074e
00000001803A071E xor       r9d, r9d
00000001803A0721 mov       r8b, 1
00000001803A0724 mov       rdx, rax
00000001803A0727 mov       rcx, rsi
00000001803A072A call      0x1811af070
00000001803A072F xor       edx, edx
00000001803A0731 mov       rcx, rdi
00000001803A0734 call      0x1803ae1f0
00000001803A0739 xor       r8d, r8d
00000001803A073C mov       edx, 1
00000001803A0741 mov       rcx, rbx
00000001803A0744 call      0x18039fee0
00000001803A0749 jmp       0x1803a057d
00000001803A074E call      0x180250150
00000001803A0753 int3      
00000001803A0754 int3      
00000001803A0755 int3      
00000001803A0756 int3      
00000001803A0757 int3      
00000001803A0758 int3      
00000001803A0759 int3      
00000001803A075A int3      
00000001803A075B int3      
00000001803A075C int3      
00000001803A075D int3      
00000001803A075E int3      
00000001803A075F int3      