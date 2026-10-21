00000001803C4480 push      rdi
00000001803C4482 sub       rsp, 0x40
00000001803C4486 cmp       byte ptr [rip + 0x1920b8f], 0 # 0x181ce501c
00000001803C448D mov       rdi, rcx
00000001803C4490 jne       0x1803c44d5
00000001803C4492 lea       rcx, [rip + 0x17d8c6f] # 0x181b9d108
00000001803C4499 call      0x18024fef0
00000001803C449E lea       rcx, [rip + 0x17e24d3] # 0x181ba6978
00000001803C44A5 call      0x18024fef0
00000001803C44AA lea       rcx, [rip + 0x17e2587] # 0x181ba6a38
00000001803C44B1 call      0x18024fef0
00000001803C44B6 lea       rcx, [rip + 0x17e263b] # 0x181ba6af8
00000001803C44BD call      0x18024fef0
00000001803C44C2 lea       rcx, [rip + 0x17e274f] # 0x181ba6c18
00000001803C44C9 call      0x18024fef0
00000001803C44CE mov       byte ptr [rip + 0x1920b47], 1 # 0x181ce501c
00000001803C44D5 mov       rax, qword ptr [rip + 0x17d8c2c] # 0x181b9d108
00000001803C44DC cmp       dword ptr [rax + 0xe0], 0
00000001803C44E3 jne       0x1803c44f4
00000001803C44E5 mov       rcx, rax
00000001803C44E8 call      0x1802501e0
00000001803C44ED mov       rax, qword ptr [rip + 0x17d8c14] # 0x181b9d108
00000001803C44F4 mov       rax, qword ptr [rax + 0xb8]
00000001803C44FB mov       qword ptr [rsp + 0x50], rbx
00000001803C4500 mov       qword ptr [rsp + 0x58], rbp
00000001803C4505 mov       qword ptr [rsp + 0x60], rsi
00000001803C450A mov       rcx, qword ptr [rax + 0x70]
00000001803C450E mov       qword ptr [rsp + 0x68], r14
00000001803C4513 movaps    xmmword ptr [rsp + 0x30], xmm6
00000001803C4518 test      rcx, rcx
00000001803C451B je        0x1803c487b
00000001803C4521 inc       dword ptr [rcx + 0x1c]
00000001803C4524 xor       ebx, ebx
00000001803C4526 mov       dword ptr [rcx + 0x18], ebx
00000001803C4529 mov       rax, qword ptr [rip + 0x17d8bd8] # 0x181b9d108
00000001803C4530 mov       rcx, qword ptr [rax + 0xb8]
00000001803C4537 mov       rcx, qword ptr [rcx + 0x70]
00000001803C453B test      rcx, rcx
00000001803C453E je        0x1803c487b
00000001803C4544 mov       rdx, qword ptr [rip + 0x17e25ad] # 0x181ba6af8
00000001803C454B call      0x18065baa0
00000001803C4550 test      rdi, rdi
00000001803C4553 je        0x1803c487b
00000001803C4559 mov       ecx, dword ptr [rdi + 0x18]
00000001803C455C add       ecx, ecx
00000001803C455E cmp       eax, ecx
00000001803C4560 jge       0x1803c45a6
00000001803C4562 mov       rax, qword ptr [rip + 0x17d8b9f] # 0x181b9d108
00000001803C4569 cmp       dword ptr [rax + 0xe0], ebx
00000001803C456F jne       0x1803c4580
00000001803C4571 mov       rcx, rax
00000001803C4574 call      0x1802501e0
00000001803C4579 mov       rax, qword ptr [rip + 0x17d8b88] # 0x181b9d108
00000001803C4580 mov       rax, qword ptr [rax + 0xb8]
00000001803C4587 mov       rcx, qword ptr [rax + 0x70]
00000001803C458B test      rcx, rcx
00000001803C458E je        0x1803c487b
00000001803C4594 mov       edx, dword ptr [rdi + 0x18]
00000001803C4597 mov       r8, qword ptr [rip + 0x17e267a] # 0x181ba6c18
00000001803C459E shl       edx, 2
00000001803C45A1 call      0x180805920
00000001803C45A6 mov       r14d, dword ptr [rdi + 0x18]
00000001803C45AA test      r14d, r14d
00000001803C45AD jle       0x1803c485c
00000001803C45B3 movss     xmm6, dword ptr [rip + 0x11e3a81] # 0x1815a803c
00000001803C45BB nop       dword ptr [rax + rax]
00000001803C45C0 cmp       ebx, dword ptr [rdi + 0x18]
00000001803C45C3 jae       0x1803c4881
00000001803C45C9 movsxd    rax, ebx
00000001803C45CC mov       ebp, dword ptr [rdi + rax*4 + 0x20]
00000001803C45D0 movsxd    rax, ebx
00000001803C45D3 inc       rax
00000001803C45D6 cmp       eax, dword ptr [rdi + 0x18]
00000001803C45D9 jae       0x1803c4881
00000001803C45DF mov       r9, qword ptr [rip + 0x17d8b22] # 0x181b9d108
00000001803C45E6 mov       esi, dword ptr [rdi + rax*4 + 0x20]
00000001803C45EA cmp       dword ptr [r9 + 0xe0], 0
00000001803C45F2 jne       0x1803c4603
00000001803C45F4 mov       rcx, r9
00000001803C45F7 call      0x1802501e0
00000001803C45FC mov       r9, qword ptr [rip + 0x17d8b05] # 0x181b9d108
00000001803C4603 mov       rcx, qword ptr [r9 + 0xb8]
00000001803C460A mov       eax, ebp
00000001803C460C shr       eax, 0x10
00000001803C460F cwde      
00000001803C4610 movd      xmm0, eax
00000001803C4614 cvtdq2ps  xmm0, xmm0
00000001803C4617 movsx     eax, bp
00000001803C461A mulss     xmm0, xmm6
00000001803C461E movss     dword ptr [rcx + 0x60], xmm0
00000001803C4623 movd      xmm0, eax
00000001803C4627 mov       eax, esi
00000001803C4629 cvtdq2ps  xmm0, xmm0
00000001803C462C shr       eax, 0x10
00000001803C462F cwde      
00000001803C4630 mulss     xmm0, xmm6
00000001803C4634 movss     dword ptr [rcx + 0x64], xmm0
00000001803C4639 movd      xmm0, eax
00000001803C463D movsx     eax, si
00000001803C4640 cvtdq2ps  xmm0, xmm0
00000001803C4643 movd      xmm1, eax
00000001803C4647 cvtdq2ps  xmm1, xmm1
00000001803C464A mulss     xmm0, xmm6
00000001803C464E mulss     xmm1, xmm6
00000001803C4652 movss     dword ptr [rcx + 0x68], xmm0
00000001803C4657 movss     dword ptr [rcx + 0x6c], xmm1
00000001803C465C mov       rcx, qword ptr [rip + 0x17d8aa5] # 0x181b9d108
00000001803C4663 mov       rax, qword ptr [rcx + 0xb8]
00000001803C466A mov       r9, qword ptr [rax + 0x70]
00000001803C466E test      r9, r9
00000001803C4671 je        0x1803c487b
00000001803C4677 mov       r8, qword ptr [rip + 0x17e22fa] # 0x181ba6978
00000001803C467E movups    xmm0, xmmword ptr [rax + 0x60]
00000001803C4682 inc       dword ptr [r9 + 0x1c]
00000001803C4686 mov       rdx, qword ptr [r9 + 0x10]
00000001803C468A movsxd    rcx, dword ptr [r9 + 0x18]
00000001803C468E test      rdx, rdx
00000001803C4691 je        0x1803c487b
00000001803C4697 cmp       ecx, dword ptr [rdx + 0x18]
00000001803C469A jb        0x1803c46c0
00000001803C469C mov       rax, qword ptr [r8 + 0x20]
00000001803C46A0 lea       rdx, [rsp + 0x20]
00000001803C46A5 mov       rcx, r9
00000001803C46A8 movdqa    xmmword ptr [rsp + 0x20], xmm0
00000001803C46AE mov       r8, qword ptr [rax + 0xc0]
00000001803C46B5 mov       r8, qword ptr [r8 + 0x70]
00000001803C46B9 call      0x180807540
00000001803C46BE jmp       0x1803c46db
00000001803C46C0 lea       eax, [rcx + 1]
00000001803C46C3 mov       dword ptr [r9 + 0x18], eax
00000001803C46C7 cmp       ecx, dword ptr [rdx + 0x18]
00000001803C46CA jae       0x1803c4881
00000001803C46D0 lea       rax, [rcx + 2]
00000001803C46D4 add       rax, rax
00000001803C46D7 movups    xmmword ptr [rdx + rax*8], xmm0
00000001803C46DB mov       rcx, qword ptr [rip + 0x17d8a26] # 0x181b9d108
00000001803C46E2 mov       rax, qword ptr [rcx + 0xb8]
00000001803C46E9 mov       r9, qword ptr [rax + 0x70]
00000001803C46ED test      r9, r9
00000001803C46F0 je        0x1803c487b
00000001803C46F6 mov       r8, qword ptr [rip + 0x17e227b] # 0x181ba6978
00000001803C46FD movups    xmm0, xmmword ptr [rax + 0x60]
00000001803C4701 inc       dword ptr [r9 + 0x1c]
00000001803C4705 mov       rdx, qword ptr [r9 + 0x10]
00000001803C4709 movsxd    rcx, dword ptr [r9 + 0x18]
00000001803C470D test      rdx, rdx
00000001803C4710 je        0x1803c487b
00000001803C4716 cmp       ecx, dword ptr [rdx + 0x18]
00000001803C4719 jb        0x1803c473f
00000001803C471B mov       rax, qword ptr [r8 + 0x20]
00000001803C471F lea       rdx, [rsp + 0x20]
00000001803C4724 mov       rcx, r9
00000001803C4727 movdqa    xmmword ptr [rsp + 0x20], xmm0
00000001803C472D mov       r8, qword ptr [rax + 0xc0]
00000001803C4734 mov       r8, qword ptr [r8 + 0x70]
00000001803C4738 call      0x180807540
00000001803C473D jmp       0x1803c475a
00000001803C473F lea       eax, [rcx + 1]
00000001803C4742 mov       dword ptr [r9 + 0x18], eax
00000001803C4746 cmp       ecx, dword ptr [rdx + 0x18]
00000001803C4749 jae       0x1803c4881
00000001803C474F lea       rax, [rcx + 2]
00000001803C4753 add       rax, rax
00000001803C4756 movups    xmmword ptr [rdx + rax*8], xmm0
00000001803C475A mov       rcx, qword ptr [rip + 0x17d89a7] # 0x181b9d108
00000001803C4761 mov       rax, qword ptr [rcx + 0xb8]
00000001803C4768 mov       r9, qword ptr [rax + 0x70]
00000001803C476C test      r9, r9
00000001803C476F je        0x1803c487b
00000001803C4775 mov       r8, qword ptr [rip + 0x17e21fc] # 0x181ba6978
00000001803C477C movups    xmm0, xmmword ptr [rax + 0x60]
00000001803C4780 inc       dword ptr [r9 + 0x1c]
00000001803C4784 mov       rdx, qword ptr [r9 + 0x10]
00000001803C4788 movsxd    rcx, dword ptr [r9 + 0x18]
00000001803C478C test      rdx, rdx
00000001803C478F je        0x1803c487b
00000001803C4795 cmp       ecx, dword ptr [rdx + 0x18]
00000001803C4798 jb        0x1803c47be
00000001803C479A mov       rax, qword ptr [r8 + 0x20]
00000001803C479E lea       rdx, [rsp + 0x20]
00000001803C47A3 mov       rcx, r9
00000001803C47A6 movdqa    xmmword ptr [rsp + 0x20], xmm0
00000001803C47AC mov       r8, qword ptr [rax + 0xc0]
00000001803C47B3 mov       r8, qword ptr [r8 + 0x70]
00000001803C47B7 call      0x180807540
00000001803C47BC jmp       0x1803c47d9
00000001803C47BE lea       eax, [rcx + 1]
00000001803C47C1 mov       dword ptr [r9 + 0x18], eax
00000001803C47C5 cmp       ecx, dword ptr [rdx + 0x18]
00000001803C47C8 jae       0x1803c4881
00000001803C47CE lea       rax, [rcx + 2]
00000001803C47D2 add       rax, rax
00000001803C47D5 movups    xmmword ptr [rdx + rax*8], xmm0
00000001803C47D9 mov       rcx, qword ptr [rip + 0x17d8928] # 0x181b9d108
00000001803C47E0 mov       rax, qword ptr [rcx + 0xb8]
00000001803C47E7 mov       r9, qword ptr [rax + 0x70]
00000001803C47EB test      r9, r9
00000001803C47EE je        0x1803c487b
00000001803C47F4 mov       r8, qword ptr [rip + 0x17e217d] # 0x181ba6978
00000001803C47FB movups    xmm0, xmmword ptr [rax + 0x60]
00000001803C47FF inc       dword ptr [r9 + 0x1c]
00000001803C4803 mov       rdx, qword ptr [r9 + 0x10]
00000001803C4807 movsxd    rcx, dword ptr [r9 + 0x18]
00000001803C480B test      rdx, rdx
00000001803C480E je        0x1803c487b
00000001803C4810 cmp       ecx, dword ptr [rdx + 0x18]
00000001803C4813 jb        0x1803c4839
00000001803C4815 mov       rax, qword ptr [r8 + 0x20]
00000001803C4819 lea       rdx, [rsp + 0x20]
00000001803C481E mov       rcx, r9
00000001803C4821 movdqa    xmmword ptr [rsp + 0x20], xmm0
00000001803C4827 mov       r8, qword ptr [rax + 0xc0]
00000001803C482E mov       r8, qword ptr [r8 + 0x70]
00000001803C4832 call      0x180807540
00000001803C4837 jmp       0x1803c4850
00000001803C4839 lea       eax, [rcx + 1]
00000001803C483C mov       dword ptr [r9 + 0x18], eax
00000001803C4840 cmp       ecx, dword ptr [rdx + 0x18]
00000001803C4843 jae       0x1803c4881
00000001803C4845 lea       rax, [rcx + 2]
00000001803C4849 add       rax, rax
00000001803C484C movups    xmmword ptr [rdx + rax*8], xmm0
00000001803C4850 add       ebx, 2
00000001803C4853 cmp       ebx, r14d
00000001803C4856 jl        0x1803c45c0
00000001803C485C movaps    xmm6, xmmword ptr [rsp + 0x30]
00000001803C4861 mov       r14, qword ptr [rsp + 0x68]
00000001803C4866 mov       rsi, qword ptr [rsp + 0x60]
00000001803C486B mov       rbp, qword ptr [rsp + 0x58]
00000001803C4870 mov       rbx, qword ptr [rsp + 0x50]
00000001803C4875 add       rsp, 0x40
00000001803C4879 pop       rdi
00000001803C487A ret       
00000001803C487B call      0x180250150
00000001803C4880 int3      
00000001803C4881 call      0x180250140
00000001803C4886 int3      