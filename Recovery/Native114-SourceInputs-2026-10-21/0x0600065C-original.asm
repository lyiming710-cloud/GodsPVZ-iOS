00000001803A1600 mov       qword ptr [rsp + 0x20], rsi
00000001803A1605 movss     dword ptr [rsp + 0x10], xmm1
00000001803A160B push      rdi
00000001803A160C sub       rsp, 0xd0
00000001803A1613 cmp       byte ptr [rip + 0x19438b8], 0 # 0x181ce4ed2
00000001803A161A mov       rdi, rcx
00000001803A161D mov       qword ptr [rsp + 0xf0], rbx
00000001803A1625 jne       0x1803a16fb
00000001803A162B lea       rcx, [rip + 0x180b5c6] # 0x181bacbf8
00000001803A1632 call      0x18024fef0
00000001803A1637 lea       rcx, [rip + 0x18196ba] # 0x181bbacf8
00000001803A163E call      0x18024fef0
00000001803A1643 lea       rcx, [rip + 0x1810326] # 0x181bb1970
00000001803A164A call      0x18024fef0
00000001803A164F lea       rcx, [rip + 0x1808a2a] # 0x181baa080
00000001803A1656 call      0x18024fef0
00000001803A165B lea       rcx, [rip + 0x18089c6] # 0x181baa028
00000001803A1662 call      0x18024fef0
00000001803A1667 lea       rcx, [rip + 0x1827bba] # 0x181bc9228
00000001803A166E call      0x18024fef0
00000001803A1673 lea       rcx, [rip + 0x1801f2e] # 0x181ba35a8
00000001803A167A call      0x18024fef0
00000001803A167F lea       rcx, [rip + 0x1818e0a] # 0x181bba490
00000001803A1686 call      0x18024fef0
00000001803A168B lea       rcx, [rip + 0x18042b6] # 0x181ba5948
00000001803A1692 call      0x18024fef0
00000001803A1697 lea       rcx, [rip + 0x180e4ba] # 0x181bafb58
00000001803A169E call      0x18024fef0
00000001803A16A3 lea       rcx, [rip + 0x18023de] # 0x181ba3a88
00000001803A16AA call      0x18024fef0
00000001803A16AF lea       rcx, [rip + 0x18088c2] # 0x181ba9f78
00000001803A16B6 call      0x18024fef0
00000001803A16BB lea       rcx, [rip + 0x1803c26] # 0x181ba52e8
00000001803A16C2 call      0x18024fef0
00000001803A16C7 lea       rcx, [rip + 0x1805c5a] # 0x181ba7328
00000001803A16CE call      0x18024fef0
00000001803A16D3 lea       rcx, [rip + 0x180246e] # 0x181ba3b48
00000001803A16DA call      0x18024fef0
00000001803A16DF lea       rcx, [rip + 0x1805b82] # 0x181ba7268
00000001803A16E6 call      0x18024fef0
00000001803A16EB movss     xmm1, dword ptr [rsp + 0xe8]
00000001803A16F4 mov       byte ptr [rip + 0x19437d7], 1 # 0x181ce4ed2
00000001803A16FB movss     dword ptr [rdi + 0x50], xmm1
00000001803A1700 xor       edx, edx
00000001803A1702 mov       rax, qword ptr [rip + 0x18195ef] # 0x181bbacf8
00000001803A1709 mov       rcx, qword ptr [rax + 0xb8]
00000001803A1710 mov       rbx, qword ptr [rcx]
00000001803A1713 mov       rcx, rdi
00000001803A1716 call      0x18131f6f0
00000001803A171B xor       edx, edx
00000001803A171D mov       rcx, rax
00000001803A1720 mov       rsi, rax
00000001803A1723 call      0x1803ccf20
00000001803A1728 cmp       eax, 0x549c27b9
00000001803A172D jbe       0x1803a17ef
00000001803A1733 cmp       eax, 0x70eb52e5
00000001803A1738 jbe       0x1803a1798
00000001803A173A cmp       eax, 0x7ada1982
00000001803A173F jne       0x1803a1767
00000001803A1741 mov       rdx, qword ptr [rip + 0x1810228] # 0x181bb1970
00000001803A1748 xor       r8d, r8d
00000001803A174B mov       rcx, rsi
00000001803A174E call      0x180b76170
00000001803A1753 test      al, al
00000001803A1755 je        0x1803a185e
00000001803A175B mov       rbx, qword ptr [rip + 0x18041e6] # 0x181ba5948
00000001803A1762 jmp       0x1803a185e
00000001803A1767 cmp       eax, 0xbaabd2d5
00000001803A176C jne       0x1803a185e
00000001803A1772 mov       rdx, qword ptr [rip + 0x18087ff] # 0x181ba9f78
00000001803A1779 xor       r8d, r8d
00000001803A177C mov       rcx, rsi
00000001803A177F call      0x180b76170
00000001803A1784 test      al, al
00000001803A1786 je        0x1803a185e
00000001803A178C mov       rbx, qword ptr [rip + 0x1801e15] # 0x181ba35a8
00000001803A1793 jmp       0x1803a185e
00000001803A1798 cmp       eax, 0x67029c76
00000001803A179D jne       0x1803a17c5
00000001803A179F mov       rdx, qword ptr [rip + 0x180e3b2] # 0x181bafb58
00000001803A17A6 xor       r8d, r8d
00000001803A17A9 mov       rcx, rsi
00000001803A17AC call      0x180b76170
00000001803A17B1 test      al, al
00000001803A17B3 je        0x1803a185e
00000001803A17B9 mov       rbx, qword ptr [rip + 0x1805b68] # 0x181ba7328
00000001803A17C0 jmp       0x1803a185e
00000001803A17C5 cmp       eax, 0x70eb52e5
00000001803A17CA jne       0x1803a185e
00000001803A17D0 mov       rdx, qword ptr [rip + 0x1827a51] # 0x181bc9228
00000001803A17D7 xor       r8d, r8d
00000001803A17DA mov       rcx, rsi
00000001803A17DD call      0x180b76170
00000001803A17E2 test      al, al
00000001803A17E4 je        0x1803a185e
00000001803A17E6 mov       rbx, qword ptr [rip + 0x1803afb] # 0x181ba52e8
00000001803A17ED jmp       0x1803a185e
00000001803A17EF cmp       eax, 0x4a0effac
00000001803A17F4 jne       0x1803a1815
00000001803A17F6 mov       rdx, qword ptr [rip + 0x1818c93] # 0x181bba490
00000001803A17FD xor       r8d, r8d
00000001803A1800 mov       rcx, rsi
00000001803A1803 call      0x180b76170
00000001803A1808 test      al, al
00000001803A180A je        0x1803a185e
00000001803A180C mov       rbx, qword ptr [rip + 0x1805a55] # 0x181ba7268
00000001803A1813 jmp       0x1803a185e
00000001803A1815 cmp       eax, 0x529c2493
00000001803A181A jne       0x1803a183b
00000001803A181C mov       rdx, qword ptr [rip + 0x1808805] # 0x181baa028
00000001803A1823 xor       r8d, r8d
00000001803A1826 mov       rcx, rsi
00000001803A1829 call      0x180b76170
00000001803A182E test      al, al
00000001803A1830 je        0x1803a185e
00000001803A1832 mov       rbx, qword ptr [rip + 0x180230f] # 0x181ba3b48
00000001803A1839 jmp       0x1803a185e
00000001803A183B cmp       eax, 0x549c27b9
00000001803A1840 jne       0x1803a185e
00000001803A1842 mov       rdx, qword ptr [rip + 0x1808837] # 0x181baa080
00000001803A1849 xor       r8d, r8d
00000001803A184C mov       rcx, rsi
00000001803A184F call      0x180b76170
00000001803A1854 test      al, al
00000001803A1856 cmovne    rbx, qword ptr [rip + 0x180222a] # 0x181ba3a88
00000001803A185E mov       rsi, qword ptr [rdi + 0x40]
00000001803A1862 lea       rcx, [rsp + 0xe8]
00000001803A186A movaps    xmmword ptr [rsp + 0xc0], xmm6
00000001803A1872 xor       r8d, r8d
00000001803A1875 movaps    xmmword ptr [rsp + 0xa0], xmm8
00000001803A187E mov       rdx, rbx
00000001803A1881 movaps    xmmword ptr [rsp + 0x90], xmm9
00000001803A188A movaps    xmmword ptr [rsp + 0x80], xmm10
00000001803A1893 movaps    xmmword ptr [rsp + 0x70], xmm11
00000001803A1899 movaps    xmmword ptr [rsp + 0x60], xmm12
00000001803A189F movaps    xmmword ptr [rsp + 0x50], xmm13
00000001803A18A5 movaps    xmmword ptr [rsp + 0x40], xmm14
00000001803A18AB movaps    xmmword ptr [rsp + 0x30], xmm15
00000001803A18B1 call      0x180cc5a50
00000001803A18B6 mov       rbx, qword ptr [rsp + 0xf0]
00000001803A18BE test      rsi, rsi
00000001803A18C1 je        0x1803a1c1d
00000001803A18C7 xor       r9d, r9d
00000001803A18CA mov       r8b, 1
00000001803A18CD mov       rdx, rax
00000001803A18D0 mov       rcx, rsi
00000001803A18D3 call      0x1811af070
00000001803A18D8 movss     xmm6, dword ptr [rip + 0x1206130] # 0x1815a7a10
00000001803A18E0 xor       edx, edx
00000001803A18E2 mov       rcx, rdi
00000001803A18E5 movss     dword ptr [rsp + 0xe0], xmm6
00000001803A18EE movaps    xmm15, xmm6
00000001803A18F2 movaps    xmm12, xmm6
00000001803A18F6 movaps    xmm13, xmm6
00000001803A18FA xorps     xmm8, xmm8
00000001803A18FE xorps     xmm10, xmm10
00000001803A1902 call      0x18131f6f0
00000001803A1907 mov       rdx, qword ptr [rip + 0x180e24a] # 0x181bafb58
00000001803A190E xor       r8d, r8d
00000001803A1911 mov       rcx, rax
00000001803A1914 call      0x180b76170
00000001803A1919 test      al, al
00000001803A191B jne       0x1803a1bf3
00000001803A1921 xor       edx, edx
00000001803A1923 mov       rcx, rdi
00000001803A1926 call      0x18131f6f0
00000001803A192B mov       rdx, qword ptr [rip + 0x18086f6] # 0x181baa028
00000001803A1932 xor       r8d, r8d
00000001803A1935 mov       rcx, rax
00000001803A1938 call      0x180b76170
00000001803A193D test      al, al
00000001803A193F jne       0x1803a1bf3
00000001803A1945 xor       edx, edx
00000001803A1947 mov       rcx, rdi
00000001803A194A call      0x18131f6f0
00000001803A194F mov       rdx, qword ptr [rip + 0x181001a] # 0x181bb1970
00000001803A1956 xor       r8d, r8d
00000001803A1959 mov       rcx, rax
00000001803A195C call      0x180b76170
00000001803A1961 test      al, al
00000001803A1963 jne       0x1803a1bf3
00000001803A1969 movaps    xmmword ptr [rsp + 0xb0], xmm7
00000001803A1971 movss     xmm0, dword ptr [rsp + 0xe8]
00000001803A197A divss     xmm0, dword ptr [rdi + 0x54]
00000001803A197F comiss    xmm0, xmm8
00000001803A1983 jb        0x1803a198e
00000001803A1985 comiss    xmm6, xmm0
00000001803A1988 jb        0x1803a198e
00000001803A198A movaps    xmm10, xmm0
00000001803A198E comiss    xmm0, xmm6
00000001803A1991 movss     xmm1, dword ptr [rip + 0x120608b] # 0x1815a7a24
00000001803A1999 xorps     xmm7, xmm7
00000001803A199C movss     xmm3, dword ptr [rip + 0x12065d0] # 0x1815a7f74
00000001803A19A4 movss     xmm4, dword ptr [rip + 0x12065bc] # 0x1815a7f68
00000001803A19AC jbe       0x1803a19ce
00000001803A19AE comiss    xmm1, xmm0
00000001803A19B1 jb        0x1803a19ce
00000001803A19B3 movaps    xmm7, xmm0
00000001803A19B6 movaps    xmm10, xmm6
00000001803A19BA subss     xmm7, xmm6
00000001803A19BE movaps    xmm12, xmm3
00000001803A19C2 movaps    xmm13, xmm4
00000001803A19C6 mulss     xmm7, dword ptr [rip + 0x120603a] # 0x1815a7a08
00000001803A19CE comiss    xmm0, xmm1
00000001803A19D1 movss     xmm2, dword ptr [rip + 0x1206277] # 0x1815a7c50
00000001803A19D9 movaps    xmm9, xmm6
00000001803A19DD movss     xmm5, dword ptr [rip + 0x120657f] # 0x1815a7f64
00000001803A19E5 movss     xmm14, dword ptr [rip + 0x12060d6] # 0x1815a7ac4
00000001803A19EE jbe       0x1803a1a21
00000001803A19F0 comiss    xmm2, xmm0
00000001803A19F3 jb        0x1803a1a21
00000001803A19F5 movaps    xmm7, xmm0
00000001803A19F8 movss     dword ptr [rsp + 0xe0], xmm4
00000001803A1A01 subss     xmm7, xmm1
00000001803A1A05 movaps    xmm10, xmm6
00000001803A1A09 movaps    xmm15, xmm3
00000001803A1A0D movaps    xmm12, xmm5
00000001803A1A11 movaps    xmm9, xmm14
00000001803A1A15 movaps    xmm13, xmm6
00000001803A1A19 mulss     xmm7, dword ptr [rip + 0x120608f] # 0x1815a7ab0
00000001803A1A21 comiss    xmm0, xmm2
00000001803A1A24 movss     xmm1, dword ptr [rip + 0x12061bc] # 0x1815a7be8
00000001803A1A2C movaps    xmm11, xmm6
00000001803A1A30 jbe       0x1803a1a6e
00000001803A1A32 comiss    xmm1, xmm0
00000001803A1A35 jbe       0x1803a1a6e
00000001803A1A37 movss     xmm12, dword ptr [rip + 0x1206530] # 0x1815a7f70
00000001803A1A40 movaps    xmm7, xmm0
00000001803A1A43 movss     xmm9, dword ptr [rip + 0x1206520] # 0x1815a7f6c
00000001803A1A4C subss     xmm7, xmm2
00000001803A1A50 movaps    xmm11, xmm14
00000001803A1A54 movaps    xmm10, xmm6
00000001803A1A58 movaps    xmm15, xmm5
00000001803A1A5C movaps    xmm14, xmm6
00000001803A1A60 movaps    xmm13, xmm6
00000001803A1A64 mulss     xmm7, dword ptr [rip + 0x12064f4] # 0x1815a7f60
00000001803A1A6C jmp       0x1803a1a78
00000001803A1A6E movss     xmm14, dword ptr [rsp + 0xe0]
00000001803A1A78 comiss    xmm0, xmm1
00000001803A1A7B jb        0x1803a1a95
00000001803A1A7D movss     xmm9, dword ptr [rip + 0x1205f86] # 0x1815a7a0c
00000001803A1A86 xorps     xmm10, xmm10
00000001803A1A8A movaps    xmm7, xmm6
00000001803A1A8D movaps    xmm12, xmm6
00000001803A1A91 xorps     xmm13, xmm13
00000001803A1A95 mov       rcx, qword ptr [rip + 0x180b15c] # 0x181bacbf8
00000001803A1A9C cmp       dword ptr [rcx + 0xe0], 0
00000001803A1AA3 jne       0x1803a1aaa
00000001803A1AA5 call      0x1802501e0
00000001803A1AAA xor       r9d, r9d
00000001803A1AAD movaps    xmm2, xmm6
00000001803A1AB0 xorps     xmm1, xmm1
00000001803A1AB3 movaps    xmm0, xmm10
00000001803A1AB7 call      0x18030e0e0
00000001803A1ABC movaps    xmm10, xmm0
00000001803A1AC0 xor       r9d, r9d
00000001803A1AC3 movaps    xmm0, xmm7
00000001803A1AC6 movaps    xmm2, xmm6
00000001803A1AC9 xorps     xmm1, xmm1
00000001803A1ACC call      0x18030e0e0
00000001803A1AD1 mov       rcx, qword ptr [rdi + 0x28]
00000001803A1AD5 movaps    xmm6, xmm0
00000001803A1AD8 movaps    xmm7, xmmword ptr [rsp + 0xb0]
00000001803A1AE0 test      rcx, rcx
00000001803A1AE3 je        0x1803a1c1d
00000001803A1AE9 xor       r8d, r8d
00000001803A1AEC movaps    xmm1, xmm10
00000001803A1AF0 call      0x1813d7750
00000001803A1AF5 mov       rcx, qword ptr [rdi + 0x30]
00000001803A1AF9 test      rcx, rcx
00000001803A1AFC je        0x1803a1c1d
00000001803A1B02 xor       r8d, r8d
00000001803A1B05 movaps    xmm1, xmm6
00000001803A1B08 call      0x1813d7750
00000001803A1B0D mov       rcx, qword ptr [rdi + 0x28]
00000001803A1B11 movss     dword ptr [rsp + 0x20], xmm15
00000001803A1B18 movss     dword ptr [rsp + 0x24], xmm11
00000001803A1B1F movss     dword ptr [rsp + 0x28], xmm14
00000001803A1B26 mov       dword ptr [rsp + 0x2c], 0x3f800000
00000001803A1B2E test      rcx, rcx
00000001803A1B31 je        0x1803a1c1d
00000001803A1B37 mov       rax, qword ptr [rcx]
00000001803A1B3A lea       rdx, [rsp + 0x20]
00000001803A1B3F movups    xmm0, xmmword ptr [rsp + 0x20]
00000001803A1B44 mov       r8, qword ptr [rax + 0x2b0]
00000001803A1B4B movdqa    xmmword ptr [rsp + 0x20], xmm0
00000001803A1B51 call      qword ptr [rax + 0x2a8]
00000001803A1B57 mov       rcx, qword ptr [rdi + 0x30]
00000001803A1B5B movss     dword ptr [rsp + 0x20], xmm12
00000001803A1B62 movss     dword ptr [rsp + 0x24], xmm9
00000001803A1B69 movss     dword ptr [rsp + 0x28], xmm13
00000001803A1B70 mov       dword ptr [rsp + 0x2c], 0x3f800000
00000001803A1B78 test      rcx, rcx
00000001803A1B7B je        0x1803a1c1d
00000001803A1B81 mov       rax, qword ptr [rcx]
00000001803A1B84 lea       rdx, [rsp + 0x20]
00000001803A1B89 movups    xmm0, xmmword ptr [rsp + 0x20]
00000001803A1B8E mov       r8, qword ptr [rax + 0x2b0]
00000001803A1B95 movdqa    xmmword ptr [rsp + 0x20], xmm0
00000001803A1B9B call      qword ptr [rax + 0x2a8]
00000001803A1BA1 movaps    xmm15, xmmword ptr [rsp + 0x30]
00000001803A1BA7 movaps    xmm14, xmmword ptr [rsp + 0x40]
00000001803A1BAD movaps    xmm13, xmmword ptr [rsp + 0x50]
00000001803A1BB3 movaps    xmm12, xmmword ptr [rsp + 0x60]
00000001803A1BB9 movaps    xmm11, xmmword ptr [rsp + 0x70]
00000001803A1BBF movaps    xmm10, xmmword ptr [rsp + 0x80]
00000001803A1BC8 movaps    xmm9, xmmword ptr [rsp + 0x90]
00000001803A1BD1 movaps    xmm8, xmmword ptr [rsp + 0xa0]
00000001803A1BDA movaps    xmm6, xmmword ptr [rsp + 0xc0]
00000001803A1BE2 mov       rsi, qword ptr [rsp + 0xf8]
00000001803A1BEA add       rsp, 0xd0
00000001803A1BF1 pop       rdi
00000001803A1BF2 ret       
00000001803A1BF3 mov       rcx, qword ptr [rdi + 0x28]
00000001803A1BF7 test      rcx, rcx
00000001803A1BFA je        0x1803a1c1d
00000001803A1BFC xor       r8d, r8d
00000001803A1BFF xorps     xmm1, xmm1
00000001803A1C02 call      0x1813d7750
00000001803A1C07 mov       rcx, qword ptr [rdi + 0x30]
00000001803A1C0B test      rcx, rcx
00000001803A1C0E je        0x1803a1c1d
00000001803A1C10 xor       r8d, r8d
00000001803A1C13 xorps     xmm1, xmm1
00000001803A1C16 call      0x1813d7750
00000001803A1C1B jmp       0x1803a1ba1
00000001803A1C1D call      0x180250150
00000001803A1C22 int3      
00000001803A1C23 int3      
00000001803A1C24 int3      
00000001803A1C25 int3      
00000001803A1C26 int3      
00000001803A1C27 int3      
00000001803A1C28 int3      
00000001803A1C29 int3      
00000001803A1C2A int3      
00000001803A1C2B int3      
00000001803A1C2C int3      
00000001803A1C2D int3      
00000001803A1C2E int3      
00000001803A1C2F int3      