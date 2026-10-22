00000001803C13B0 push      rsi
00000001803C13B2 push      rdi
00000001803C13B3 sub       rsp, 0x28
00000001803C13B7 cmp       byte ptr [rip + 0x1923b9e], 0 # 0x181ce4f5c
00000001803C13BE mov       rsi, rcx
00000001803C13C1 jne       0x1803c145e
00000001803C13C7 lea       rcx, [rip + 0x1801d0a] # 0x181bc30d8
00000001803C13CE call      0x18024fef0
00000001803C13D3 lea       rcx, [rip + 0x17dca6e] # 0x181b9de48
00000001803C13DA call      0x18024fef0
00000001803C13DF lea       rcx, [rip + 0x17dfaf2] # 0x181ba0ed8
00000001803C13E6 call      0x18024fef0
00000001803C13EB lea       rcx, [rip + 0x17e2f86] # 0x181ba4378
00000001803C13F2 call      0x18024fef0
00000001803C13F7 lea       rcx, [rip + 0x17e2f1a] # 0x181ba4318
00000001803C13FE call      0x18024fef0
00000001803C1403 lea       rcx, [rip + 0x17ef0f6] # 0x181bb0500
00000001803C140A call      0x18024fef0
00000001803C140F lea       rcx, [rip + 0x17ff66a] # 0x181bc0a80
00000001803C1416 call      0x18024fef0
00000001803C141B lea       rcx, [rip + 0x17fe8ce] # 0x181bbfcf0
00000001803C1422 call      0x18024fef0
00000001803C1427 lea       rcx, [rip + 0x17f76ba] # 0x181bb8ae8
00000001803C142E call      0x18024fef0
00000001803C1433 lea       rcx, [rip + 0x180470e] # 0x181bc5b48
00000001803C143A call      0x18024fef0
00000001803C143F lea       rcx, [rip + 0x17f764a] # 0x181bb8a90
00000001803C1446 call      0x18024fef0
00000001803C144B lea       rcx, [rip + 0x1804646] # 0x181bc5a98
00000001803C1452 call      0x18024fef0
00000001803C1457 mov       byte ptr [rip + 0x1923afe], 1 # 0x181ce4f5c
00000001803C145E mov       ecx, dword ptr [rsi + 0x10]
00000001803C1461 mov       rdi, qword ptr [rsi + 0x20]
00000001803C1465 mov       qword ptr [rsp + 0x48], rbx
00000001803C146A mov       qword ptr [rsp + 0x50], rbp
00000001803C146F mov       qword ptr [rsp + 0x20], r14
00000001803C1474 mov       dword ptr [rsp + 0x40], 0
00000001803C147C test      ecx, ecx
00000001803C147E je        0x1803c14b2
00000001803C1480 sub       ecx, 1
00000001803C1483 je        0x1803c149c
00000001803C1485 xor       al, al
00000001803C1487 cmp       ecx, 1
00000001803C148A jne       0x1803c191f
00000001803C1490 mov       dword ptr [rsi + 0x10], 0xffffffff
00000001803C1497 jmp       0x1803c191f
00000001803C149C mov       eax, dword ptr [rsi + 0x28]
00000001803C149F mov       dword ptr [rsi + 0x10], 0xffffffff
00000001803C14A6 mov       dword ptr [rsp + 0x40], eax
00000001803C14AA lea       ecx, [rax + 1]
00000001803C14AD jmp       0x1803c1788
00000001803C14B2 mov       dword ptr [rsi + 0x10], 0xffffffff
00000001803C14B9 test      rdi, rdi
00000001803C14BC je        0x1803c1951
00000001803C14C2 cmp       dword ptr [rdi + 0x20], 0
00000001803C14C6 je        0x1803c163c
00000001803C14CC cmp       dword ptr [rdi + 0x20], 1
00000001803C14D0 jne       0x1803c1786
00000001803C14D6 xor       edx, edx
00000001803C14D8 mov       rcx, rdi
00000001803C14DB call      0x1813044d0
00000001803C14E0 test      rax, rax
00000001803C14E3 je        0x1803c1951
00000001803C14E9 mov       rdx, qword ptr [rip + 0x17e2e28] # 0x181ba4318
00000001803C14F0 mov       rcx, rax
00000001803C14F3 call      0x18046d280
00000001803C14F8 lea       rcx, [rdi + 0x48]
00000001803C14FC mov       qword ptr [rdi + 0x48], rax
00000001803C1500 mov       rdx, rax
00000001803C1503 call      0x18024f360
00000001803C1508 mov       rcx, qword ptr [rip + 0x17eeff1] # 0x181bb0500
00000001803C150F mov       rbx, qword ptr [rdi + 0x30]
00000001803C1513 cmp       dword ptr [rcx + 0xe0], 0
00000001803C151A jne       0x1803c1521
00000001803C151C call      0x1802501e0
00000001803C1521 xor       r8d, r8d
00000001803C1524 xor       edx, edx
00000001803C1526 mov       rcx, rbx
00000001803C1529 call      0x18131f900
00000001803C152E mov       rbp, qword ptr [rdi + 0x48]
00000001803C1532 test      al, al
00000001803C1534 jne       0x1803c1591
00000001803C1536 mov       rcx, qword ptr [rip + 0x17fe7b3] # 0x181bbfcf0
00000001803C153D mov       rbx, qword ptr [rip + 0x17dc904] # 0x181b9de48
00000001803C1544 cmp       dword ptr [rcx + 0xe0], 0
00000001803C154B jne       0x1803c1552
00000001803C154D call      0x1802501e0
00000001803C1552 xor       edx, edx
00000001803C1554 mov       rcx, rbx
00000001803C1557 call      0x180ccee30
00000001803C155C mov       rcx, qword ptr [rip + 0x18045e5] # 0x181bc5b48
00000001803C1563 xor       r8d, r8d
00000001803C1566 mov       rdx, rax
00000001803C1569 call      0x18130e2b0
00000001803C156E mov       rcx, rax
00000001803C1571 test      rbp, rbp
00000001803C1574 je        0x1803c1951
00000001803C157A xor       edx, edx
00000001803C157C test      rax, rax
00000001803C157F je        0x1803c159e
00000001803C1581 mov       rax, qword ptr [rip + 0x17df950] # 0x181ba0ed8
00000001803C1588 cmp       qword ptr [rcx], rax
00000001803C158B cmove     rdx, rcx
00000001803C158F jmp       0x1803c159e
00000001803C1591 test      rbp, rbp
00000001803C1594 je        0x1803c1951
00000001803C159A mov       rdx, qword ptr [rdi + 0x30]
00000001803C159E xor       r8d, r8d
00000001803C15A1 mov       rcx, rbp
00000001803C15A4 call      0x1813bb9e0
00000001803C15A9 mov       rcx, qword ptr [rdi + 0x48]
00000001803C15AD test      rcx, rcx
00000001803C15B0 je        0x1803c1951
00000001803C15B6 mov       rdx, qword ptr [rip + 0x1801b1b] # 0x181bc30d8
00000001803C15BD call      0x18042e9f0
00000001803C15C2 mov       rcx, qword ptr [rdi + 0x48]
00000001803C15C6 mov       rbx, rax
00000001803C15C9 test      rcx, rcx
00000001803C15CC je        0x1803c1951
00000001803C15D2 xor       edx, edx
00000001803C15D4 call      0x1813bb880
00000001803C15D9 test      rax, rax
00000001803C15DC je        0x1803c1951
00000001803C15E2 xor       edx, edx
00000001803C15E4 mov       rcx, rax
00000001803C15E7 call      0x1813b9880
00000001803C15EC test      rbx, rbx
00000001803C15EF je        0x1803c1951
00000001803C15F5 xor       r8d, r8d
00000001803C15F8 mov       rdx, rax
00000001803C15FB mov       rcx, rbx
00000001803C15FE call      0x1812fb060
00000001803C1603 mov       rcx, qword ptr [rdi + 0x48]
00000001803C1607 test      rcx, rcx
00000001803C160A je        0x1803c1951
00000001803C1610 xor       r8d, r8d
00000001803C1613 mov       edx, 0x30
00000001803C1618 call      0x1813bb9a0
00000001803C161D mov       rcx, qword ptr [rdi + 0x48]
00000001803C1621 test      rcx, rcx
00000001803C1624 je        0x1803c1951
00000001803C162A xor       r8d, r8d
00000001803C162D mov       edx, 4
00000001803C1632 call      0x1813bb8c0
00000001803C1637 jmp       0x1803c1786
00000001803C163C xor       edx, edx
00000001803C163E mov       rcx, rdi
00000001803C1641 call      0x1813044d0
00000001803C1646 test      rax, rax
00000001803C1649 je        0x1803c1951
00000001803C164F mov       rdx, qword ptr [rip + 0x17e2d22] # 0x181ba4378
00000001803C1656 mov       rcx, rax
00000001803C1659 call      0x18046d280
00000001803C165E lea       rcx, [rdi + 0x38]
00000001803C1662 mov       qword ptr [rdi + 0x38], rax
00000001803C1666 mov       rdx, rax
00000001803C1669 call      0x18024f360
00000001803C166E mov       rcx, qword ptr [rdi + 0x38]
00000001803C1672 test      rcx, rcx
00000001803C1675 je        0x1803c1951
00000001803C167B mov       rax, qword ptr [rcx]
00000001803C167E mov       dl, 1
00000001803C1680 mov       r8, qword ptr [rax + 0x600]
00000001803C1687 call      qword ptr [rax + 0x5f8]
00000001803C168D mov       rcx, qword ptr [rip + 0x17eee6c] # 0x181bb0500
00000001803C1694 mov       rbx, qword ptr [rdi + 0x28]
00000001803C1698 cmp       dword ptr [rcx + 0xe0], 0
00000001803C169F jne       0x1803c16a6
00000001803C16A1 call      0x1802501e0
00000001803C16A6 xor       r8d, r8d
00000001803C16A9 xor       edx, edx
00000001803C16AB mov       rcx, rbx
00000001803C16AE call      0x18131f900
00000001803C16B3 test      al, al
00000001803C16B5 je        0x1803c16d0
00000001803C16B7 mov       rcx, qword ptr [rdi + 0x38]
00000001803C16BB test      rcx, rcx
00000001803C16BE je        0x1803c1951
00000001803C16C4 mov       rdx, qword ptr [rdi + 0x28]
00000001803C16C8 xor       r8d, r8d
00000001803C16CB call      0x1811b9520
00000001803C16D0 mov       rcx, qword ptr [rdi + 0x38]
00000001803C16D4 test      rcx, rcx
00000001803C16D7 je        0x1803c1951
00000001803C16DD movss     xmm1, dword ptr [rip + 0x11e692f] # 0x1815a8014
00000001803C16E5 xor       r8d, r8d
00000001803C16E8 call      0x1811b9410
00000001803C16ED mov       rcx, qword ptr [rdi + 0x38]
00000001803C16F1 test      rcx, rcx
00000001803C16F4 je        0x1803c1951
00000001803C16FA xor       r8d, r8d
00000001803C16FD mov       edx, 0x202
00000001803C1702 call      0x1811b8c90
00000001803C1707 mov       rcx, qword ptr [rdi + 0x38]
00000001803C170B test      rcx, rcx
00000001803C170E je        0x1803c1951
00000001803C1714 xor       r8d, r8d
00000001803C1717 mov       dl, 1
00000001803C1719 call      0x1811b9050
00000001803C171E mov       rcx, qword ptr [rdi + 0x38]
00000001803C1722 test      rcx, rcx
00000001803C1725 je        0x1803c1951
00000001803C172B xor       r8d, r8d
00000001803C172E xor       edx, edx
00000001803C1730 call      0x1811b9000
00000001803C1735 mov       rax, qword ptr [rdi + 0x38]
00000001803C1739 test      rax, rax
00000001803C173C je        0x1803c1951
00000001803C1742 mov       rdx, qword ptr [rax + 0x100]
00000001803C1749 test      rdx, rdx
00000001803C174C je        0x1803c1951
00000001803C1752 mov       rdx, qword ptr [rdx + 0x20]
00000001803C1756 lea       rcx, [rdi + 0x50]
00000001803C175A mov       qword ptr [rdi + 0x50], rdx
00000001803C175E call      0x18024f360
00000001803C1763 mov       rdx, qword ptr [rip + 0x17ff316] # 0x181bc0a80
00000001803C176A mov       rcx, qword ptr [rip + 0x1804327] # 0x181bc5a98
00000001803C1771 call      0x1804a6ab0
00000001803C1776 lea       rcx, [rdi + 0x58]
00000001803C177A mov       qword ptr [rdi + 0x58], rax
00000001803C177E mov       rdx, rax
00000001803C1781 call      0x18024f360
00000001803C1786 xor       ecx, ecx
00000001803C1788 mov       dword ptr [rsi + 0x28], ecx
00000001803C178B cmp       ecx, 0xf4240
00000001803C1791 jg        0x1803c1935
00000001803C1797 test      rdi, rdi
00000001803C179A je        0x1803c1951
00000001803C17A0 cmp       dword ptr [rdi + 0x20], 0
00000001803C17A4 je        0x1803c180b
00000001803C17A6 cmp       dword ptr [rdi + 0x20], 1
00000001803C17AA jne       0x1803c1903
00000001803C17B0 mov       rbx, qword ptr [rdi + 0x48]
00000001803C17B4 mov       eax, 0x10624dd3
00000001803C17B9 imul      ecx
00000001803C17BB sar       edx, 6
00000001803C17BE mov       eax, edx
00000001803C17C0 shr       eax, 0x1f
00000001803C17C3 add       edx, eax
00000001803C17C5 imul      eax, edx, 0x3e8
00000001803C17CB xor       edx, edx
00000001803C17CD sub       ecx, eax
00000001803C17CF mov       dword ptr [rsp + 0x40], ecx
00000001803C17D3 lea       rcx, [rsp + 0x40]
00000001803C17D8 call      0x180ca3820
00000001803C17DD mov       rcx, qword ptr [rip + 0x17f7304] # 0x181bb8ae8
00000001803C17E4 xor       r8d, r8d
00000001803C17E7 mov       rdx, rax
00000001803C17EA call      0x180b74270
00000001803C17EF test      rbx, rbx
00000001803C17F2 je        0x1803c1951
00000001803C17F8 xor       r8d, r8d
00000001803C17FB mov       rdx, rax
00000001803C17FE mov       rcx, rbx
00000001803C1801 call      0x1813bba30
00000001803C1806 jmp       0x1803c1903
00000001803C180B mov       rcx, qword ptr [rdi + 0x38]
00000001803C180F test      rcx, rcx
00000001803C1812 je        0x1803c1951
00000001803C1818 mov       r8d, dword ptr [rsi + 0x28]
00000001803C181C mov       eax, 0x10624dd3
00000001803C1821 imul      r8d
00000001803C1824 xor       r9d, r9d
00000001803C1827 sar       edx, 6
00000001803C182A mov       eax, edx
00000001803C182C shr       eax, 0x1f
00000001803C182F add       edx, eax
00000001803C1831 imul      eax, edx, 0x3e8
00000001803C1837 mov       rdx, qword ptr [rip + 0x17f7252] # 0x181bb8a90
00000001803C183E sub       r8d, eax
00000001803C1841 movd      xmm2, r8d
00000001803C1846 cvtdq2ps  xmm2, xmm2
00000001803C1849 call      0x1811af370
00000001803C184E mov       ecx, dword ptr [rsi + 0x28]
00000001803C1851 mov       eax, 0x10624dd3
00000001803C1856 imul      ecx
00000001803C1858 sar       edx, 6
00000001803C185B mov       eax, edx
00000001803C185D shr       eax, 0x1f
00000001803C1860 add       edx, eax
00000001803C1862 imul      eax, edx, 0x3e8
00000001803C1868 sub       ecx, eax
00000001803C186A cmp       ecx, 0x3e7
00000001803C1870 jne       0x1803c1903
00000001803C1876 mov       r14, qword ptr [rdi + 0x38]
00000001803C187A mov       rcx, r14
00000001803C187D test      r14, r14
00000001803C1880 je        0x1803c1951
00000001803C1886 mov       rdx, qword ptr [r14]
00000001803C1889 mov       rax, qword ptr [rdx + 0x568]
00000001803C1890 mov       rdx, qword ptr [rdx + 0x570]
00000001803C1897 call      rax
00000001803C1899 mov       rcx, qword ptr [rip + 0x17eec60] # 0x181bb0500
00000001803C18A0 mov       rbx, rax
00000001803C18A3 mov       rbp, qword ptr [rdi + 0x50]
00000001803C18A7 cmp       dword ptr [rcx + 0xe0], 0
00000001803C18AE jne       0x1803c18b5
00000001803C18B0 call      0x1802501e0
00000001803C18B5 xor       r8d, r8d
00000001803C18B8 mov       rdx, rbp
00000001803C18BB mov       rcx, rbx
00000001803C18BE call      0x18131f760
00000001803C18C3 mov       rcx, qword ptr [rdi + 0x38]
00000001803C18C7 test      al, al
00000001803C18C9 je        0x1803c18d1
00000001803C18CB mov       rbx, qword ptr [rdi + 0x58]
00000001803C18CF jmp       0x1803c18d5
00000001803C18D1 mov       rbx, qword ptr [rdi + 0x50]
00000001803C18D5 test      rcx, rcx
00000001803C18D8 je        0x1803c1951
00000001803C18DA mov       rax, qword ptr [rcx]
00000001803C18DD mov       rdx, rbx
00000001803C18E0 mov       r8, qword ptr [rax + 0x580]
00000001803C18E7 call      qword ptr [rax + 0x578]
00000001803C18ED mov       rax, qword ptr [r14]
00000001803C18F0 mov       rdx, rbx
00000001803C18F3 mov       rcx, r14
00000001803C18F6 mov       r8, qword ptr [rax + 0x580]
00000001803C18FD call      qword ptr [rax + 0x578]
00000001803C1903 lea       rcx, [rsi + 0x18]
00000001803C1907 mov       qword ptr [rsi + 0x18], 0
00000001803C190F xor       edx, edx
00000001803C1911 call      0x18024f360
00000001803C1916 mov       dword ptr [rsi + 0x10], 1
00000001803C191D mov       al, 1
00000001803C191F mov       r14, qword ptr [rsp + 0x20]
00000001803C1924 mov       rbp, qword ptr [rsp + 0x50]
00000001803C1929 mov       rbx, qword ptr [rsp + 0x48]
00000001803C192E add       rsp, 0x28
00000001803C1932 pop       rdi
00000001803C1933 pop       rsi
00000001803C1934 ret       
00000001803C1935 lea       rcx, [rsi + 0x18]
00000001803C1939 mov       qword ptr [rsi + 0x18], 0
00000001803C1941 xor       edx, edx
00000001803C1943 call      0x18024f360
00000001803C1948 mov       dword ptr [rsi + 0x10], 2
00000001803C194F jmp       0x1803c191d
00000001803C1951 call      0x180250150
00000001803C1956 int3      
00000001803C1957 int3      
00000001803C1958 int3      
00000001803C1959 int3      
00000001803C195A int3      
00000001803C195B int3      
00000001803C195C int3      
00000001803C195D int3      
00000001803C195E int3      
00000001803C195F int3      