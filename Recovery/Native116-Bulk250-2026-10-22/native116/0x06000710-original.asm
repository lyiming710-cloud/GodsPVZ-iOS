00000001803B1650 mov       qword ptr [rsp + 0x20], rbx
00000001803B1655 push      rsi
00000001803B1656 sub       rsp, 0x20
00000001803B165A cmp       byte ptr [rip + 0x19338de], 0 # 0x181ce4f3f
00000001803B1661 mov       rsi, rcx
00000001803B1664 jne       0x1803b16b5
00000001803B1666 lea       rcx, [rip + 0x17fe2cb] # 0x181baf938
00000001803B166D call      0x18024fef0
00000001803B1672 lea       rcx, [rip + 0x17fe527] # 0x181bafba0
00000001803B1679 call      0x18024fef0
00000001803B167E lea       rcx, [rip + 0x17fe5cb] # 0x181bafc50
00000001803B1685 call      0x18024fef0
00000001803B168A lea       rcx, [rip + 0x17fe777] # 0x181bafe08
00000001803B1691 call      0x18024fef0
00000001803B1696 lea       rcx, [rip + 0x17fe7c3] # 0x181bafe60
00000001803B169D call      0x18024fef0
00000001803B16A2 lea       rcx, [rip + 0x17fee57] # 0x181bb0500
00000001803B16A9 call      0x18024fef0
00000001803B16AE mov       byte ptr [rip + 0x193388a], 1 # 0x181ce4f3f
00000001803B16B5 cmp       byte ptr [rsi + 0x40], 0
00000001803B16B9 mov       rbx, qword ptr [rsi + 0x28]
00000001803B16BD sete      al
00000001803B16C0 mov       byte ptr [rsi + 0x40], al
00000001803B16C3 mov       rcx, qword ptr [rip + 0x17fee36] # 0x181bb0500
00000001803B16CA cmp       dword ptr [rcx + 0xe0], 0
00000001803B16D1 jne       0x1803b16d8
00000001803B16D3 call      0x1802501e0
00000001803B16D8 mov       qword ptr [rsp + 0x30], rbp
00000001803B16DD xor       r8d, r8d
00000001803B16E0 mov       qword ptr [rsp + 0x38], rdi
00000001803B16E5 xor       edx, edx
00000001803B16E7 mov       rcx, rbx
00000001803B16EA mov       qword ptr [rsp + 0x40], r14
00000001803B16EF call      0x18131f900
00000001803B16F4 test      al, al
00000001803B16F6 je        0x1803b1850
00000001803B16FC mov       rax, qword ptr [rsi + 0x20]
00000001803B1700 test      rax, rax
00000001803B1703 je        0x1803b1931
00000001803B1709 mov       rbx, qword ptr [rax + 0x20]
00000001803B170D test      rbx, rbx
00000001803B1710 je        0x1803b1931
00000001803B1716 mov       rbx, qword ptr [rbx + 0x20]
00000001803B171A xor       edi, edi
00000001803B171C mov       r14d, 0xffffffff
00000001803B1722 mov       ebp, edi
00000001803B1724 mov       eax, edi
00000001803B1726 test      rbx, rbx
00000001803B1729 je        0x1803b1931
00000001803B172F nop       
00000001803B1730 cmp       eax, dword ptr [rbx + 0x18]
00000001803B1733 jge       0x1803b1765
00000001803B1735 mov       r8, qword ptr [rip + 0x17fe724] # 0x181bafe60
00000001803B173C mov       edx, ebp
00000001803B173E mov       rcx, rbx
00000001803B1741 call      0x180836790
00000001803B1746 mov       rdx, qword ptr [rsi + 0x28]
00000001803B174A test      rdx, rdx
00000001803B174D je        0x1803b1931
00000001803B1753 cmp       eax, dword ptr [rdx + 0x60]
00000001803B1756 mov       ecx, ebp
00000001803B1758 cmovne    ecx, r14d
00000001803B175C inc       ebp
00000001803B175E mov       eax, ebp
00000001803B1760 mov       r14d, ecx
00000001803B1763 jmp       0x1803b1730
00000001803B1765 cmp       byte ptr [rsi + 0x40], dil
00000001803B1769 je        0x1803b1910
00000001803B176F cmp       r14d, -1
00000001803B1773 jne       0x1803b1906
00000001803B1779 cmp       dword ptr [rbx + 0x18], edi
00000001803B177C je        0x1803b18a9
00000001803B1782 mov       eax, edi
00000001803B1784 cmp       eax, dword ptr [rbx + 0x18]
00000001803B1787 jge       0x1803b1850
00000001803B178D test      edi, edi
00000001803B178F jne       0x1803b17b4
00000001803B1791 mov       r8, qword ptr [rip + 0x17fe6c8] # 0x181bafe60
00000001803B1798 xor       edx, edx
00000001803B179A mov       rcx, rbx
00000001803B179D call      0x180836790
00000001803B17A2 mov       rcx, qword ptr [rsi + 0x28]
00000001803B17A6 test      rcx, rcx
00000001803B17A9 je        0x1803b1931
00000001803B17AF cmp       eax, dword ptr [rcx + 0x60]
00000001803B17B2 jg        0x1803b1832
00000001803B17B4 mov       eax, dword ptr [rbx + 0x18]
00000001803B17B7 dec       eax
00000001803B17B9 cmp       edi, eax
00000001803B17BB jne       0x1803b17e4
00000001803B17BD mov       r8, qword ptr [rip + 0x17fe69c] # 0x181bafe60
00000001803B17C4 mov       edx, edi
00000001803B17C6 mov       rcx, rbx
00000001803B17C9 call      0x180836790
00000001803B17CE mov       rcx, qword ptr [rsi + 0x28]
00000001803B17D2 test      rcx, rcx
00000001803B17D5 je        0x1803b1931
00000001803B17DB cmp       eax, dword ptr [rcx + 0x60]
00000001803B17DE jle       0x1803b1873
00000001803B17E4 mov       r8, qword ptr [rip + 0x17fe675] # 0x181bafe60
00000001803B17EB mov       edx, edi
00000001803B17ED mov       rcx, rbx
00000001803B17F0 call      0x180836790
00000001803B17F5 mov       rcx, qword ptr [rsi + 0x28]
00000001803B17F9 test      rcx, rcx
00000001803B17FC je        0x1803b1931
00000001803B1802 cmp       eax, dword ptr [rcx + 0x60]
00000001803B1805 jg        0x1803b182b
00000001803B1807 mov       r8, qword ptr [rip + 0x17fe652] # 0x181bafe60
00000001803B180E lea       edx, [rdi + 1]
00000001803B1811 mov       rcx, rbx
00000001803B1814 call      0x180836790
00000001803B1819 mov       rcx, qword ptr [rsi + 0x28]
00000001803B181D test      rcx, rcx
00000001803B1820 je        0x1803b1931
00000001803B1826 cmp       eax, dword ptr [rcx + 0x60]
00000001803B1829 jge       0x1803b1890
00000001803B182B inc       edi
00000001803B182D jmp       0x1803b1782
00000001803B1832 test      rcx, rcx
00000001803B1835 je        0x1803b1931
00000001803B183B mov       r9, qword ptr [rip + 0x17fe35e] # 0x181bafba0
00000001803B1842 xor       edx, edx
00000001803B1844 mov       r8d, dword ptr [rcx + 0x60]
00000001803B1848 mov       rcx, rbx
00000001803B184B call      0x18082dea0
00000001803B1850 xor       edx, edx
00000001803B1852 mov       rcx, rsi
00000001803B1855 mov       r14, qword ptr [rsp + 0x40]
00000001803B185A mov       rdi, qword ptr [rsp + 0x38]
00000001803B185F mov       rbp, qword ptr [rsp + 0x30]
00000001803B1864 mov       rbx, qword ptr [rsp + 0x48]
00000001803B1869 add       rsp, 0x20
00000001803B186D pop       rsi
00000001803B186E jmp       0x1803b1c50
00000001803B1873 test      rcx, rcx
00000001803B1876 je        0x1803b1931
00000001803B187C mov       edx, dword ptr [rcx + 0x60]
00000001803B187F mov       rcx, rbx
00000001803B1882 mov       r8, qword ptr [rip + 0x17fe0af] # 0x181baf938
00000001803B1889 call      0x180003400
00000001803B188E jmp       0x1803b1850
00000001803B1890 test      rcx, rcx
00000001803B1893 je        0x1803b1931
00000001803B1899 mov       r9, qword ptr [rip + 0x17fe300] # 0x181bafba0
00000001803B18A0 lea       edx, [rdi + 1]
00000001803B18A3 mov       r8d, dword ptr [rcx + 0x60]
00000001803B18A7 jmp       0x1803b1848
00000001803B18A9 mov       rax, qword ptr [rsi + 0x28]
00000001803B18AD test      rax, rax
00000001803B18B0 je        0x1803b1931
00000001803B18B2 mov       r9d, dword ptr [rax + 0x60]
00000001803B18B6 mov       r8, qword ptr [rip + 0x17fe07b] # 0x181baf938
00000001803B18BD inc       dword ptr [rbx + 0x1c]
00000001803B18C0 mov       rdx, qword ptr [rbx + 0x10]
00000001803B18C4 movsxd    rcx, dword ptr [rbx + 0x18]
00000001803B18C8 test      rdx, rdx
00000001803B18CB je        0x1803b1931
00000001803B18CD cmp       ecx, dword ptr [rdx + 0x18]
00000001803B18D0 jb        0x1803b18f1
00000001803B18D2 mov       rax, qword ptr [r8 + 0x20]
00000001803B18D6 mov       edx, r9d
00000001803B18D9 mov       rcx, rbx
00000001803B18DC mov       r8, qword ptr [rax + 0xc0]
00000001803B18E3 mov       r8, qword ptr [r8 + 0x70]
00000001803B18E7 call      0x180827460
00000001803B18EC jmp       0x1803b1850
00000001803B18F1 lea       eax, [rcx + 1]
00000001803B18F4 mov       dword ptr [rbx + 0x18], eax
00000001803B18F7 cmp       ecx, dword ptr [rdx + 0x18]
00000001803B18FA jae       0x1803b1937
00000001803B18FC mov       dword ptr [rdx + rcx*4 + 0x20], r9d
00000001803B1901 jmp       0x1803b1850
00000001803B1906 cmp       byte ptr [rsi + 0x40], dil
00000001803B190A jne       0x1803b1850
00000001803B1910 cmp       r14d, -1
00000001803B1914 je        0x1803b1850
00000001803B191A mov       r8, qword ptr [rip + 0x17fe32f] # 0x181bafc50
00000001803B1921 mov       edx, r14d
00000001803B1924 mov       rcx, rbx
00000001803B1927 call      0x18080d270
00000001803B192C jmp       0x1803b1850
00000001803B1931 call      0x180250150
00000001803B1936 int3      
00000001803B1937 call      0x180250140
00000001803B193C int3      
00000001803B193D int3      
00000001803B193E int3      
00000001803B193F int3      