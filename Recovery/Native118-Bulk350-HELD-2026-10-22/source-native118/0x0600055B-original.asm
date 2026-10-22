00000001803893B0 mov       qword ptr [rsp + 8], rbx
00000001803893B5 mov       qword ptr [rsp + 0x10], rsi
00000001803893BA push      rdi
00000001803893BB sub       rsp, 0x20
00000001803893BF cmp       byte ptr [rip + 0x195ba68], 0 # 0x181ce4e2e
00000001803893C6 mov       rdi, rdx
00000001803893C9 mov       rbx, rcx
00000001803893CC jne       0x18038945d
00000001803893D2 lea       rcx, [rip + 0x1818c3f] # 0x181ba2018
00000001803893D9 call      0x18024fef0
00000001803893DE lea       rcx, [rip + 0x181fde3] # 0x181ba91c8
00000001803893E5 call      0x18024fef0
00000001803893EA lea       rcx, [rip + 0x1830f4f] # 0x181bba340
00000001803893F1 call      0x18024fef0
00000001803893F6 lea       rcx, [rip + 0x182e5f3] # 0x181bb79f0
00000001803893FD call      0x18024fef0
0000000180389402 lea       rcx, [rip + 0x18270f7] # 0x181bb0500
0000000180389409 call      0x18024fef0
000000018038940E lea       rcx, [rip + 0x182d063] # 0x181bb6478
0000000180389415 call      0x18024fef0
000000018038941A lea       rcx, [rip + 0x18318d7] # 0x181bbacf8
0000000180389421 call      0x18024fef0
0000000180389426 lea       rcx, [rip + 0x181645b] # 0x181b9f888
000000018038942D call      0x18024fef0
0000000180389432 lea       rcx, [rip + 0x181fa0f] # 0x181ba8e48
0000000180389439 call      0x18024fef0
000000018038943E lea       rcx, [rip + 0x183f343] # 0x181bc8788
0000000180389445 call      0x18024fef0
000000018038944A lea       rcx, [rip + 0x181faa7] # 0x181ba8ef8
0000000180389451 call      0x18024fef0
0000000180389456 mov       byte ptr [rip + 0x195b9d1], 1 # 0x181ce4e2e
000000018038945D mov       rcx, qword ptr [rip + 0x182709c] # 0x181bb0500
0000000180389464 mov       rsi, qword ptr [rbx + 0x70]
0000000180389468 cmp       dword ptr [rcx + 0xe0], 0
000000018038946F jne       0x180389476
0000000180389471 call      0x1802501e0
0000000180389476 xor       r8d, r8d
0000000180389479 xor       edx, edx
000000018038947B mov       rcx, rsi
000000018038947E call      0x18131f900
0000000180389483 test      al, al
0000000180389485 je        0x1803894bd
0000000180389487 mov       rcx, qword ptr [rbx + 0x70]
000000018038948B test      rcx, rcx
000000018038948E je        0x180389a97
0000000180389494 xor       edx, edx
0000000180389496 call      0x1813044d0
000000018038949B mov       rcx, qword ptr [rip + 0x182705e] # 0x181bb0500
00000001803894A2 mov       rsi, rax
00000001803894A5 cmp       dword ptr [rcx + 0xe0], 0
00000001803894AC jne       0x1803894b3
00000001803894AE call      0x1802501e0
00000001803894B3 xor       edx, edx
00000001803894B5 mov       rcx, rsi
00000001803894B8 call      0x18131e940
00000001803894BD lea       rcx, [rbx + 0x28]
00000001803894C1 mov       qword ptr [rbx + 0x28], rdi
00000001803894C5 mov       rdx, rdi
00000001803894C8 call      0x18024f360
00000001803894CD test      rdi, rdi
00000001803894D0 je        0x180389942
00000001803894D6 mov       eax, dword ptr [rdi + 0x18]
00000001803894D9 mov       dword ptr [rbx + 0x20], eax
00000001803894DC mov       rax, qword ptr [rip + 0x1818b35] # 0x181ba2018
00000001803894E3 cmp       dword ptr [rax + 0xe0], 0
00000001803894EA jne       0x1803894fb
00000001803894EC mov       rcx, rax
00000001803894EF call      0x1802501e0
00000001803894F4 mov       rax, qword ptr [rip + 0x1818b1d] # 0x181ba2018
00000001803894FB mov       rax, qword ptr [rax + 0xb8]
0000000180389502 mov       rax, qword ptr [rax]
0000000180389505 test      rax, rax
0000000180389508 je        0x180389a97
000000018038950E mov       rax, qword ptr [rax + 0x28]
0000000180389512 test      rax, rax
0000000180389515 je        0x180389a97
000000018038951B mov       rdx, qword ptr [rax + 0x10]
000000018038951F test      rdx, rdx
0000000180389522 je        0x180389a97
0000000180389528 mov       rdx, qword ptr [rdx + 0x88]
000000018038952F test      rdx, rdx
0000000180389532 je        0x180389a97
0000000180389538 movsxd    rax, dword ptr [rdi + 0x18]
000000018038953C cmp       eax, dword ptr [rdx + 0x18]
000000018038953F jae       0x180389a9d
0000000180389545 cmp       byte ptr [rax + rdx + 0x20], 0
000000018038954A mov       rcx, qword ptr [rbx + 0x30]
000000018038954E je        0x1803897b4
0000000180389554 test      rcx, rcx
0000000180389557 je        0x180389a97
000000018038955D mov       rdx, qword ptr [rdi + 0x10]
0000000180389561 xor       r9d, r9d
0000000180389564 mov       r8b, 1
0000000180389567 call      0x1811af070
000000018038956C mov       rsi, qword ptr [rbx + 0x38]
0000000180389570 lea       rcx, [rdi + 0x1c]
0000000180389574 xor       edx, edx
0000000180389576 call      0x180cc5b50
000000018038957B test      rsi, rsi
000000018038957E je        0x180389a97
0000000180389584 xor       r9d, r9d
0000000180389587 mov       r8b, 1
000000018038958A mov       rdx, rax
000000018038958D mov       rcx, rsi
0000000180389590 call      0x1811af070
0000000180389595 mov       rsi, qword ptr [rbx + 0x40]
0000000180389599 lea       rcx, [rdi + 0x20]
000000018038959D xor       edx, edx
000000018038959F call      0x180cc5b50
00000001803895A4 test      rsi, rsi
00000001803895A7 je        0x180389a97
00000001803895AD xor       r9d, r9d
00000001803895B0 mov       r8b, 1
00000001803895B3 mov       rdx, rax
00000001803895B6 mov       rcx, rsi
00000001803895B9 call      0x1811af070
00000001803895BE mov       rsi, qword ptr [rbx + 0x48]
00000001803895C2 lea       rcx, [rdi + 0x24]
00000001803895C6 xor       edx, edx
00000001803895C8 call      0x180cc5b50
00000001803895CD test      rsi, rsi
00000001803895D0 je        0x180389a97
00000001803895D6 xor       r9d, r9d
00000001803895D9 mov       r8b, 1
00000001803895DC mov       rdx, rax
00000001803895DF mov       rcx, rsi
00000001803895E2 call      0x1811af070
00000001803895E7 mov       rsi, qword ptr [rbx + 0x50]
00000001803895EB lea       rcx, [rdi + 0x40]
00000001803895EF xor       edx, edx
00000001803895F1 call      0x180cc5b50
00000001803895F6 test      rsi, rsi
00000001803895F9 je        0x180389a97
00000001803895FF xor       r9d, r9d
0000000180389602 mov       r8b, 1
0000000180389605 mov       rdx, rax
0000000180389608 mov       rcx, rsi
000000018038960B call      0x1811af070
0000000180389610 mov       rsi, qword ptr [rbx + 0x58]
0000000180389614 lea       rcx, [rdi + 0x44]
0000000180389618 xor       edx, edx
000000018038961A call      0x180cc5b50
000000018038961F mov       rdx, qword ptr [rip + 0x183f162] # 0x181bc8788
0000000180389626 xor       r8d, r8d
0000000180389629 mov       rcx, rax
000000018038962C call      0x180b74270
0000000180389631 test      rsi, rsi
0000000180389634 je        0x180389a97
000000018038963A xor       r9d, r9d
000000018038963D mov       r8b, 1
0000000180389640 mov       rdx, rax
0000000180389643 mov       rcx, rsi
0000000180389646 call      0x1811af070
000000018038964B mov       rcx, qword ptr [rbx + 0x60]
000000018038964F test      rcx, rcx
0000000180389652 je        0x180389a97
0000000180389658 mov       rdx, qword ptr [rdi + 0x30]
000000018038965C xor       r9d, r9d
000000018038965F mov       r8b, 1
0000000180389662 call      0x1811af070
0000000180389667 mov       rcx, qword ptr [rbx + 0x78]
000000018038966B test      rcx, rcx
000000018038966E je        0x180389a97
0000000180389674 xor       edx, edx
0000000180389676 call      0x1813044d0
000000018038967B test      rax, rax
000000018038967E je        0x180389a97
0000000180389684 xor       r8d, r8d
0000000180389687 xor       edx, edx
0000000180389689 mov       rcx, rax
000000018038968C call      0x18131bad0
0000000180389691 mov       rcx, qword ptr [rbx + 0x80]
0000000180389698 test      rcx, rcx
000000018038969B je        0x180389a97
00000001803896A1 xor       edx, edx
00000001803896A3 call      0x1813044d0
00000001803896A8 test      rax, rax
00000001803896AB je        0x180389a97
00000001803896B1 xor       r8d, r8d
00000001803896B4 xor       edx, edx
00000001803896B6 mov       rcx, rax
00000001803896B9 call      0x18131bad0
00000001803896BE mov       rcx, qword ptr [rbx + 0x88]
00000001803896C5 test      rcx, rcx
00000001803896C8 je        0x180389a97
00000001803896CE mov       r8, qword ptr [rip + 0x181faf3] # 0x181ba91c8
00000001803896D5 mov       edx, dword ptr [rbx + 0x20]
00000001803896D8 call      0x180825710
00000001803896DD mov       rcx, qword ptr [rip + 0x1826e1c] # 0x181bb0500
00000001803896E4 mov       rdi, rax
00000001803896E7 cmp       dword ptr [rcx + 0xe0], 0
00000001803896EE jne       0x1803896f5
00000001803896F0 call      0x1802501e0
00000001803896F5 xor       r8d, r8d
00000001803896F8 xor       edx, edx
00000001803896FA mov       rcx, rdi
00000001803896FD call      0x18131f900
0000000180389702 test      al, al
0000000180389704 je        0x180389932
000000018038970A mov       rcx, qword ptr [rbx + 0x88]
0000000180389711 test      rcx, rcx
0000000180389714 je        0x180389a97
000000018038971A mov       r8, qword ptr [rip + 0x181faa7] # 0x181ba91c8
0000000180389721 mov       edx, dword ptr [rbx + 0x20]
0000000180389724 call      0x180825710
0000000180389729 mov       rcx, qword ptr [rip + 0x1826dd0] # 0x181bb0500
0000000180389730 mov       rdi, rax
0000000180389733 cmp       dword ptr [rcx + 0xe0], 0
000000018038973A jne       0x180389741
000000018038973C call      0x1802501e0
0000000180389741 mov       rdx, qword ptr [rip + 0x182e2a8] # 0x181bb79f0
0000000180389748 mov       rcx, rdi
000000018038974B call      0x1804a0690
0000000180389750 lea       rcx, [rbx + 0x70]
0000000180389754 mov       qword ptr [rbx + 0x70], rax
0000000180389758 mov       rdx, rax
000000018038975B call      0x18024f360
0000000180389760 mov       rcx, qword ptr [rbx + 0x70]
0000000180389764 test      rcx, rcx
0000000180389767 je        0x180389a97
000000018038976D xor       edx, edx
000000018038976F call      0x181304510
0000000180389774 mov       rcx, qword ptr [rbx + 0x68]
0000000180389778 mov       rdi, rax
000000018038977B test      rcx, rcx
000000018038977E je        0x180389a97
0000000180389784 xor       edx, edx
0000000180389786 call      0x18131c070
000000018038978B test      rdi, rdi
000000018038978E je        0x180389a97
0000000180389794 xor       r9d, r9d
0000000180389797 xor       r8d, r8d
000000018038979A mov       rdx, rax
000000018038979D mov       rcx, rdi
00000001803897A0 mov       rbx, qword ptr [rsp + 0x30]
00000001803897A5 mov       rsi, qword ptr [rsp + 0x38]
00000001803897AA add       rsp, 0x20
00000001803897AE pop       rdi
00000001803897AF jmp       0x18132e900
00000001803897B4 test      rcx, rcx
00000001803897B7 je        0x180389a97
00000001803897BD mov       rdx, qword ptr [rip + 0x181f734] # 0x181ba8ef8
00000001803897C4 xor       r9d, r9d
00000001803897C7 mov       r8b, 1
00000001803897CA call      0x1811af070
00000001803897CF mov       rcx, qword ptr [rbx + 0x38]
00000001803897D3 test      rcx, rcx
00000001803897D6 je        0x180389a97
00000001803897DC mov       rdx, qword ptr [rip + 0x181f665] # 0x181ba8e48
00000001803897E3 xor       r9d, r9d
00000001803897E6 mov       r8b, 1
00000001803897E9 call      0x1811af070
00000001803897EE mov       rcx, qword ptr [rbx + 0x40]
00000001803897F2 test      rcx, rcx
00000001803897F5 je        0x180389a97
00000001803897FB mov       rdx, qword ptr [rip + 0x181f646] # 0x181ba8e48
0000000180389802 xor       r9d, r9d
0000000180389805 mov       r8b, 1
0000000180389808 call      0x1811af070
000000018038980D mov       rcx, qword ptr [rbx + 0x48]
0000000180389811 test      rcx, rcx
0000000180389814 je        0x180389a97
000000018038981A mov       rdx, qword ptr [rip + 0x181f627] # 0x181ba8e48
0000000180389821 xor       r9d, r9d
0000000180389824 mov       r8b, 1
0000000180389827 call      0x1811af070
000000018038982C mov       rcx, qword ptr [rbx + 0x50]
0000000180389830 test      rcx, rcx
0000000180389833 je        0x180389a97
0000000180389839 mov       rdx, qword ptr [rip + 0x181f608] # 0x181ba8e48
0000000180389840 xor       r9d, r9d
0000000180389843 mov       r8b, 1
0000000180389846 call      0x1811af070
000000018038984B mov       rcx, qword ptr [rbx + 0x58]
000000018038984F test      rcx, rcx
0000000180389852 je        0x180389a97
0000000180389858 mov       rdx, qword ptr [rip + 0x181f5e9] # 0x181ba8e48
000000018038985F xor       r9d, r9d
0000000180389862 mov       r8b, 1
0000000180389865 call      0x1811af070
000000018038986A mov       rcx, qword ptr [rbx + 0x60]
000000018038986E test      rcx, rcx
0000000180389871 je        0x180389a97
0000000180389877 mov       rdx, qword ptr [rip + 0x181f5ca] # 0x181ba8e48
000000018038987E xor       r9d, r9d
0000000180389881 mov       r8b, 1
0000000180389884 call      0x1811af070
0000000180389889 mov       rcx, qword ptr [rip + 0x182cbe8] # 0x181bb6478
0000000180389890 mov       rdi, qword ptr [rbx + 0x78]
0000000180389894 cmp       dword ptr [rcx + 0xe0], 0
000000018038989B jne       0x1803898a2
000000018038989D call      0x1802501e0
00000001803898A2 xor       ecx, ecx
00000001803898A4 call      0x180338b00
00000001803898A9 test      rax, rax
00000001803898AC je        0x180389a97
00000001803898B2 mov       r8, qword ptr [rip + 0x1830a87] # 0x181bba340
00000001803898B9 mov       rcx, rax
00000001803898BC mov       edx, dword ptr [rbx + 0x20]
00000001803898BF call      0x180825710
00000001803898C4 test      rdi, rdi
00000001803898C7 je        0x180389a97
00000001803898CD xor       r8d, r8d
00000001803898D0 mov       rdx, rax
00000001803898D3 mov       rcx, rdi
00000001803898D6 call      0x1813d7ab0
00000001803898DB mov       rcx, qword ptr [rbx + 0x78]
00000001803898DF test      rcx, rcx
00000001803898E2 je        0x180389a97
00000001803898E8 xor       edx, edx
00000001803898EA call      0x1813044d0
00000001803898EF test      rax, rax
00000001803898F2 je        0x180389a97
00000001803898F8 xor       r8d, r8d
00000001803898FB mov       dl, 1
00000001803898FD mov       rcx, rax
0000000180389900 call      0x18131bad0
0000000180389905 mov       rcx, qword ptr [rbx + 0x80]
000000018038990C test      rcx, rcx
000000018038990F je        0x180389a97
0000000180389915 xor       edx, edx
0000000180389917 call      0x1813044d0
000000018038991C test      rax, rax
000000018038991F je        0x180389a97
0000000180389925 xor       r8d, r8d
0000000180389928 mov       dl, 1
000000018038992A mov       rcx, rax
000000018038992D call      0x18131bad0
0000000180389932 mov       rbx, qword ptr [rsp + 0x30]
0000000180389937 mov       rsi, qword ptr [rsp + 0x38]
000000018038993C add       rsp, 0x20
0000000180389940 pop       rdi
0000000180389941 ret       
0000000180389942 mov       rcx, qword ptr [rbx + 0x30]
0000000180389946 test      rcx, rcx
0000000180389949 je        0x180389a97
000000018038994F mov       rdx, qword ptr [rip + 0x1815f32] # 0x181b9f888
0000000180389956 xor       r9d, r9d
0000000180389959 mov       r8b, 1
000000018038995C call      0x1811af070
0000000180389961 mov       rcx, qword ptr [rbx + 0x38]
0000000180389965 test      rcx, rcx
0000000180389968 je        0x180389a97
000000018038996E mov       rax, qword ptr [rip + 0x1831383] # 0x181bbacf8
0000000180389975 xor       r9d, r9d
0000000180389978 mov       r8b, 1
000000018038997B mov       rdx, qword ptr [rax + 0xb8]
0000000180389982 mov       rdx, qword ptr [rdx]
0000000180389985 call      0x1811af070
000000018038998A mov       rcx, qword ptr [rbx + 0x40]
000000018038998E test      rcx, rcx
0000000180389991 je        0x180389a97
0000000180389997 mov       rax, qword ptr [rip + 0x183135a] # 0x181bbacf8
000000018038999E xor       r9d, r9d
00000001803899A1 mov       r8b, 1
00000001803899A4 mov       rdx, qword ptr [rax + 0xb8]
00000001803899AB mov       rdx, qword ptr [rdx]
00000001803899AE call      0x1811af070
00000001803899B3 mov       rcx, qword ptr [rbx + 0x48]
00000001803899B7 test      rcx, rcx
00000001803899BA je        0x180389a97
00000001803899C0 mov       rax, qword ptr [rip + 0x1831331] # 0x181bbacf8
00000001803899C7 xor       r9d, r9d
00000001803899CA mov       r8b, 1
00000001803899CD mov       rdx, qword ptr [rax + 0xb8]
00000001803899D4 mov       rdx, qword ptr [rdx]
00000001803899D7 call      0x1811af070
00000001803899DC mov       rcx, qword ptr [rbx + 0x50]
00000001803899E0 test      rcx, rcx
00000001803899E3 je        0x180389a97
00000001803899E9 mov       rax, qword ptr [rip + 0x1831308] # 0x181bbacf8
00000001803899F0 xor       r9d, r9d
00000001803899F3 mov       r8b, 1
00000001803899F6 mov       rdx, qword ptr [rax + 0xb8]
00000001803899FD mov       rdx, qword ptr [rdx]
0000000180389A00 call      0x1811af070
0000000180389A05 mov       rcx, qword ptr [rbx + 0x58]
0000000180389A09 test      rcx, rcx
0000000180389A0C je        0x180389a97
0000000180389A12 mov       rax, qword ptr [rip + 0x18312df] # 0x181bbacf8
0000000180389A19 xor       r9d, r9d
0000000180389A1C mov       r8b, 1
0000000180389A1F mov       rdx, qword ptr [rax + 0xb8]
0000000180389A26 mov       rdx, qword ptr [rdx]
0000000180389A29 call      0x1811af070
0000000180389A2E mov       rcx, qword ptr [rbx + 0x60]
0000000180389A32 test      rcx, rcx
0000000180389A35 je        0x180389a97
0000000180389A37 mov       rax, qword ptr [rip + 0x18312ba] # 0x181bbacf8
0000000180389A3E xor       r9d, r9d
0000000180389A41 mov       r8b, 1
0000000180389A44 mov       rdx, qword ptr [rax + 0xb8]
0000000180389A4B mov       rdx, qword ptr [rdx]
0000000180389A4E call      0x1811af070
0000000180389A53 mov       rcx, qword ptr [rbx + 0x78]
0000000180389A57 test      rcx, rcx
0000000180389A5A je        0x180389a97
0000000180389A5C xor       edx, edx
0000000180389A5E call      0x1813044d0
0000000180389A63 test      rax, rax
0000000180389A66 je        0x180389a97
0000000180389A68 xor       r8d, r8d
0000000180389A6B xor       edx, edx
0000000180389A6D mov       rcx, rax
0000000180389A70 call      0x18131bad0
0000000180389A75 mov       rcx, qword ptr [rbx + 0x80]
0000000180389A7C test      rcx, rcx
0000000180389A7F je        0x180389a97
0000000180389A81 xor       edx, edx
0000000180389A83 call      0x1813044d0
0000000180389A88 test      rax, rax
0000000180389A8B je        0x180389a97
0000000180389A8D xor       r8d, r8d
0000000180389A90 xor       edx, edx
0000000180389A92 jmp       0x18038992a
0000000180389A97 call      0x180250150
0000000180389A9C int3      
0000000180389A9D call      0x180250140
0000000180389AA2 int3      
0000000180389AA3 int3      
0000000180389AA4 int3      
0000000180389AA5 int3      
0000000180389AA6 int3      
0000000180389AA7 int3      
0000000180389AA8 int3      
0000000180389AA9 int3      
0000000180389AAA int3      
0000000180389AAB int3      
0000000180389AAC int3      
0000000180389AAD int3      
0000000180389AAE int3      
0000000180389AAF int3      