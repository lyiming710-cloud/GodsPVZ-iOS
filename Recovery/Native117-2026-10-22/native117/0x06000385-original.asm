0000000180353650 mov       qword ptr [rsp + 8], rbx
0000000180353655 push      rdi
0000000180353656 sub       rsp, 0x60
000000018035365A cmp       byte ptr [rip + 0x19916b2], 0 # 0x181ce4d13
0000000180353661 mov       rbx, rcx
0000000180353664 jne       0x180353685
0000000180353666 lea       rcx, [rip + 0x1858a1b] # 0x181bac088
000000018035366D call      0x18024fef0
0000000180353672 lea       rcx, [rip + 0x185ce87] # 0x181bb0500
0000000180353679 call      0x18024fef0
000000018035367E mov       byte ptr [rip + 0x199168e], 1 # 0x181ce4d13
0000000180353685 xor       edx, edx
0000000180353687 mov       rcx, rbx
000000018035368A call      0x18034f750
000000018035368F lea       rcx, [rbx + 0x120]
0000000180353696 mov       qword ptr [rbx + 0x120], rax
000000018035369D mov       rdx, rax
00000001803536A0 mov       rdi, rax
00000001803536A3 call      0x18024f360
00000001803536A8 mov       rcx, qword ptr [rip + 0x185ce51] # 0x181bb0500
00000001803536AF cmp       dword ptr [rcx + 0xe0], 0
00000001803536B6 jne       0x1803536bd
00000001803536B8 call      0x1802501e0
00000001803536BD movaps    xmmword ptr [rsp + 0x50], xmm6
00000001803536C2 xor       r8d, r8d
00000001803536C5 xor       edx, edx
00000001803536C7 movaps    xmmword ptr [rsp + 0x40], xmm7
00000001803536CC mov       rcx, rdi
00000001803536CF call      0x18131f900
00000001803536D4 test      al, al
00000001803536D6 jne       0x1803538d3
00000001803536DC xor       edx, edx
00000001803536DE mov       rcx, rbx
00000001803536E1 call      0x18034eea0
00000001803536E6 lea       rcx, [rbx + 0x128]
00000001803536ED mov       qword ptr [rbx + 0x128], rax
00000001803536F4 mov       rdx, rax
00000001803536F7 mov       rdi, rax
00000001803536FA call      0x18024f360
00000001803536FF mov       rcx, qword ptr [rip + 0x185cdfa] # 0x181bb0500
0000000180353706 cmp       dword ptr [rcx + 0xe0], 0
000000018035370D jne       0x180353714
000000018035370F call      0x1802501e0
0000000180353714 xor       r8d, r8d
0000000180353717 xor       edx, edx
0000000180353719 mov       rcx, rdi
000000018035371C call      0x18131f900
0000000180353721 mov       rcx, qword ptr [rbx + 0x1c8]
0000000180353728 test      al, al
000000018035372A jne       0x1803537bc
0000000180353730 test      rcx, rcx
0000000180353733 je        0x180353a8a
0000000180353739 mov       r8, qword ptr [rip + 0x1858948] # 0x181bac088
0000000180353740 mov       edx, 1
0000000180353745 call      0x180825710
000000018035374A mov       rcx, qword ptr [rip + 0x185cdaf] # 0x181bb0500
0000000180353751 mov       rdi, rax
0000000180353754 cmp       dword ptr [rcx + 0xe0], 0
000000018035375B jne       0x180353762
000000018035375D call      0x1802501e0
0000000180353762 xor       edx, edx
0000000180353764 mov       rcx, rdi
0000000180353767 call      0x18131f870
000000018035376C test      al, al
000000018035376E je        0x1803537a7
0000000180353770 mov       rcx, qword ptr [rbx + 0x1c8]
0000000180353777 test      rcx, rcx
000000018035377A je        0x180353a8a
0000000180353780 mov       r8, qword ptr [rip + 0x1858901] # 0x181bac088
0000000180353787 mov       edx, 1
000000018035378C call      0x180825710
0000000180353791 test      rax, rax
0000000180353794 je        0x180353a8a
000000018035379A xor       r8d, r8d
000000018035379D xor       edx, edx
000000018035379F mov       rcx, rax
00000001803537A2 call      0x18131bad0
00000001803537A7 movaps    xmm7, xmmword ptr [rsp + 0x40]
00000001803537AC movaps    xmm6, xmmword ptr [rsp + 0x50]
00000001803537B1 mov       rbx, qword ptr [rsp + 0x70]
00000001803537B6 add       rsp, 0x60
00000001803537BA pop       rdi
00000001803537BB ret       
00000001803537BC test      rcx, rcx
00000001803537BF je        0x180353a8a
00000001803537C5 mov       r8, qword ptr [rip + 0x18588bc] # 0x181bac088
00000001803537CC mov       edx, 1
00000001803537D1 call      0x180825710
00000001803537D6 mov       rcx, qword ptr [rip + 0x185cd23] # 0x181bb0500
00000001803537DD mov       rdi, rax
00000001803537E0 cmp       dword ptr [rcx + 0xe0], 0
00000001803537E7 jne       0x1803537ee
00000001803537E9 call      0x1802501e0
00000001803537EE xor       edx, edx
00000001803537F0 mov       rcx, rdi
00000001803537F3 call      0x18131f870
00000001803537F8 test      al, al
00000001803537FA je        0x1803537a7
00000001803537FC mov       rcx, qword ptr [rbx + 0x1c8]
0000000180353803 test      rcx, rcx
0000000180353806 je        0x180353a8a
000000018035380C mov       r8, qword ptr [rip + 0x1858875] # 0x181bac088
0000000180353813 mov       edx, 1
0000000180353818 call      0x180825710
000000018035381D test      rax, rax
0000000180353820 je        0x180353a8a
0000000180353826 xor       r8d, r8d
0000000180353829 mov       dl, 1
000000018035382B mov       rcx, rax
000000018035382E call      0x18131bad0
0000000180353833 mov       rcx, qword ptr [rbx + 0x128]
000000018035383A test      rcx, rcx
000000018035383D je        0x180353a8a
0000000180353843 xor       edx, edx
0000000180353845 call      0x181304510
000000018035384A test      rax, rax
000000018035384D je        0x180353a8a
0000000180353853 xor       r8d, r8d
0000000180353856 lea       rcx, [rsp + 0x30]
000000018035385B mov       rdx, rax
000000018035385E call      0x18132f3a0
0000000180353863 mov       rcx, qword ptr [rbx + 0x1c8]
000000018035386A movsd     xmm0, qword ptr [rax]
000000018035386E mov       edi, dword ptr [rax + 8]
0000000180353871 movaps    xmm1, xmm0
0000000180353874 shufps    xmm1, xmm1, 0x55
0000000180353878 movsd     qword ptr [rsp + 0x30], xmm0
000000018035387E addss     xmm1, dword ptr [rip + 0x12543da] # 0x1815a7c60
0000000180353886 movss     dword ptr [rsp + 0x34], xmm1
000000018035388C test      rcx, rcx
000000018035388F je        0x180353a8a
0000000180353895 mov       r8, qword ptr [rip + 0x18587ec] # 0x181bac088
000000018035389C mov       edx, 1
00000001803538A1 call      0x180825710
00000001803538A6 test      rax, rax
00000001803538A9 je        0x180353a8a
00000001803538AF xor       edx, edx
00000001803538B1 mov       rcx, rax
00000001803538B4 call      0x18131c070
00000001803538B9 test      rax, rax
00000001803538BC je        0x180353a8a
00000001803538C2 movsd     xmm0, qword ptr [rsp + 0x30]
00000001803538C8 movsd     qword ptr [rsp + 0x30], xmm0
00000001803538CE jmp       0x1803539f5
00000001803538D3 mov       rcx, qword ptr [rbx + 0x1c8]
00000001803538DA test      rcx, rcx
00000001803538DD je        0x180353a8a
00000001803538E3 mov       r8, qword ptr [rip + 0x185879e] # 0x181bac088
00000001803538EA mov       edx, 1
00000001803538EF call      0x180825710
00000001803538F4 mov       rcx, qword ptr [rip + 0x185cc05] # 0x181bb0500
00000001803538FB mov       rdi, rax
00000001803538FE cmp       dword ptr [rcx + 0xe0], 0
0000000180353905 jne       0x18035390c
0000000180353907 call      0x1802501e0
000000018035390C xor       edx, edx
000000018035390E mov       rcx, rdi
0000000180353911 call      0x18131f870
0000000180353916 test      al, al
0000000180353918 je        0x1803537a7
000000018035391E mov       rcx, qword ptr [rbx + 0x1c8]
0000000180353925 test      rcx, rcx
0000000180353928 je        0x180353a8a
000000018035392E mov       r8, qword ptr [rip + 0x1858753] # 0x181bac088
0000000180353935 mov       edx, 1
000000018035393A call      0x180825710
000000018035393F test      rax, rax
0000000180353942 je        0x180353a8a
0000000180353948 xor       r8d, r8d
000000018035394B mov       dl, 1
000000018035394D mov       rcx, rax
0000000180353950 call      0x18131bad0
0000000180353955 mov       rcx, qword ptr [rbx + 0x120]
000000018035395C test      rcx, rcx
000000018035395F je        0x180353a8a
0000000180353965 xor       edx, edx
0000000180353967 call      0x181304510
000000018035396C test      rax, rax
000000018035396F je        0x180353a8a
0000000180353975 xor       r8d, r8d
0000000180353978 lea       rcx, [rsp + 0x30]
000000018035397D mov       rdx, rax
0000000180353980 call      0x18132f3a0
0000000180353985 mov       rcx, qword ptr [rbx + 0x1c8]
000000018035398C movsd     xmm0, qword ptr [rax]
0000000180353990 mov       edi, dword ptr [rax + 8]
0000000180353993 movaps    xmm6, xmm0
0000000180353996 movsd     qword ptr [rsp + 0x30], xmm0
000000018035399C movss     xmm7, dword ptr [rsp + 0x30]
00000001803539A2 shufps    xmm6, xmm6, 0x55
00000001803539A6 addss     xmm6, dword ptr [rip + 0x12542be] # 0x1815a7c6c
00000001803539AE subss     xmm7, dword ptr [rip + 0x125429e] # 0x1815a7c54
00000001803539B6 test      rcx, rcx
00000001803539B9 je        0x180353a8a
00000001803539BF mov       r8, qword ptr [rip + 0x18586c2] # 0x181bac088
00000001803539C6 mov       edx, 1
00000001803539CB call      0x180825710
00000001803539D0 test      rax, rax
00000001803539D3 je        0x180353a8a
00000001803539D9 xor       edx, edx
00000001803539DB mov       rcx, rax
00000001803539DE call      0x18131c070
00000001803539E3 test      rax, rax
00000001803539E6 je        0x180353a8a
00000001803539EC unpcklps  xmm7, xmm6
00000001803539EF movsd     qword ptr [rsp + 0x30], xmm7
00000001803539F5 xor       r8d, r8d
00000001803539F8 mov       dword ptr [rsp + 0x38], edi
00000001803539FC lea       rdx, [rsp + 0x30]
0000000180353A01 mov       rcx, rax
0000000180353A04 call      0x18132fbb0
0000000180353A09 mov       rcx, qword ptr [rbx + 0x1c8]
0000000180353A10 test      rcx, rcx
0000000180353A13 je        0x180353a8a
0000000180353A15 mov       r8, qword ptr [rip + 0x185866c] # 0x181bac088
0000000180353A1C mov       edx, 1
0000000180353A21 call      0x180825710
0000000180353A26 test      rax, rax
0000000180353A29 je        0x180353a8a
0000000180353A2B xor       edx, edx
0000000180353A2D mov       rcx, rax
0000000180353A30 call      0x18131c070
0000000180353A35 movss     xmm7, dword ptr [rip + 0x1253fd3] # 0x1815a7a10
0000000180353A3D xor       ecx, ecx
0000000180353A3F mov       rbx, rax
0000000180353A42 xorps     xmm6, xmm6
0000000180353A45 call      0x18132c2f0
0000000180353A4A test      rbx, rbx
0000000180353A4D je        0x180353a8a
0000000180353A4F mulss     xmm0, dword ptr [rip + 0x12540c5] # 0x1815a7b1c
0000000180353A57 lea       rdx, [rsp + 0x30]
0000000180353A5C xor       r9d, r9d
0000000180353A5F mov       rcx, rbx
0000000180353A62 xorps     xmm1, xmm1
0000000180353A65 movss     dword ptr [rsp + 0x38], xmm7
0000000180353A6B unpcklps  xmm1, xmm6
0000000180353A6E movaps    xmm2, xmm0
0000000180353A71 movsd     qword ptr [rsp + 0x30], xmm1
0000000180353A77 mov       qword ptr [rsp + 0x20], 0
0000000180353A80 call      0x18132e150
0000000180353A85 jmp       0x1803537a7
0000000180353A8A call      0x180250150
0000000180353A8F int3      