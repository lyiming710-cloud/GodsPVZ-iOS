000000018038D680 mov       qword ptr [rsp + 0x10], rbx
000000018038D685 mov       qword ptr [rsp + 0x18], rsi
000000018038D68A mov       qword ptr [rsp + 8], rcx
000000018038D68F push      rdi
000000018038D690 sub       rsp, 0x70
000000018038D694 mov       esi, edx
000000018038D696 mov       rdi, rcx
000000018038D699 cmp       byte ptr [rip + 0x19577a5], 0 # 0x181ce4e45
000000018038D6A0 jne       0x18038d73d
000000018038D6A6 lea       rcx, [rip + 0x1836d6b] # 0x181bc4418
000000018038D6AD call      0x18024fef0
000000018038D6B2 lea       rcx, [rip + 0x180d68f] # 0x181b9ad48
000000018038D6B9 call      0x18024fef0
000000018038D6BE lea       rcx, [rip + 0x1835d9b] # 0x181bc3460
000000018038D6C5 call      0x18024fef0
000000018038D6CA lea       rcx, [rip + 0x1835de7] # 0x181bc34b8
000000018038D6D1 call      0x18024fef0
000000018038D6D6 lea       rcx, [rip + 0x1835e33] # 0x181bc3510
000000018038D6DD call      0x18024fef0
000000018038D6E2 lea       rcx, [rip + 0x181eda7] # 0x181bac490
000000018038D6E9 call      0x18024fef0
000000018038D6EE lea       rcx, [rip + 0x181e783] # 0x181babe78
000000018038D6F5 call      0x18024fef0
000000018038D6FA lea       rcx, [rip + 0x182a70f] # 0x181bb7e10
000000018038D701 call      0x18024fef0
000000018038D706 lea       rcx, [rip + 0x1822df3] # 0x181bb0500
000000018038D70D call      0x18024fef0
000000018038D712 lea       rcx, [rip + 0x1828d5f] # 0x181bb6478
000000018038D719 call      0x18024fef0
000000018038D71E lea       rcx, [rip + 0x1834beb] # 0x181bc2310
000000018038D725 call      0x18024fef0
000000018038D72A lea       rcx, [rip + 0x181437f] # 0x181ba1ab0
000000018038D731 call      0x18024fef0
000000018038D736 mov       byte ptr [rip + 0x1957708], 1 # 0x181ce4e45
000000018038D73D mov       rbx, qword ptr [rdi + 0x28]
000000018038D741 mov       rcx, qword ptr [rip + 0x1822db8] # 0x181bb0500
000000018038D748 cmp       dword ptr [rcx + 0xe0], 0
000000018038D74F jne       0x18038d756
000000018038D751 call      0x1802501e0
000000018038D756 xor       edx, edx
000000018038D758 mov       rcx, rbx
000000018038D75B call      0x18131f870
000000018038D760 test      al, al
000000018038D762 je        0x18038d79a
000000018038D764 mov       rcx, qword ptr [rdi + 0x28]
000000018038D768 test      rcx, rcx
000000018038D76B je        0x18038da90
000000018038D771 xor       edx, edx
000000018038D773 call      0x1813044d0
000000018038D778 mov       rbx, rax
000000018038D77B mov       rcx, qword ptr [rip + 0x1822d7e] # 0x181bb0500
000000018038D782 cmp       dword ptr [rcx + 0xe0], 0
000000018038D789 jne       0x18038d790
000000018038D78B call      0x1802501e0
000000018038D790 xor       edx, edx
000000018038D792 mov       rcx, rbx
000000018038D795 call      0x18131e940
000000018038D79A mov       rcx, qword ptr [rip + 0x1828cd7] # 0x181bb6478
000000018038D7A1 cmp       dword ptr [rcx + 0xe0], 0
000000018038D7A8 jne       0x18038d7af
000000018038D7AA call      0x1802501e0
000000018038D7AF xor       edx, edx
000000018038D7B1 mov       ecx, esi
000000018038D7B3 call      0x18032f7b0
000000018038D7B8 mov       rdx, qword ptr [rip + 0x181ecd1] # 0x181bac490
000000018038D7BF mov       rcx, rax
000000018038D7C2 call      0x18047ba00
000000018038D7C7 mov       rbx, rax
000000018038D7CA mov       rcx, qword ptr [rip + 0x1822d2f] # 0x181bb0500
000000018038D7D1 cmp       dword ptr [rcx + 0xe0], 0
000000018038D7D8 jne       0x18038d7df
000000018038D7DA call      0x1802501e0
000000018038D7DF xor       edx, edx
000000018038D7E1 mov       rcx, rbx
000000018038D7E4 call      0x18131f870
000000018038D7E9 test      al, al
000000018038D7EB jne       0x18038d822
000000018038D7ED mov       rcx, qword ptr [rip + 0x180d554] # 0x181b9ad48
000000018038D7F4 cmp       dword ptr [rcx + 0xe0], 0
000000018038D7FB jne       0x18038d802
000000018038D7FD call      0x1802501e0
000000018038D802 xor       edx, edx
000000018038D804 mov       rcx, qword ptr [rip + 0x18142a5] # 0x181ba1ab0
000000018038D80B call      0x1812e68f0
000000018038D810 lea       r11, [rsp + 0x70]
000000018038D815 mov       rbx, qword ptr [r11 + 0x18]
000000018038D819 mov       rsi, qword ptr [r11 + 0x20]
000000018038D81D mov       rsp, r11
000000018038D820 pop       rdi
000000018038D821 ret       
000000018038D822 mov       rcx, qword ptr [rip + 0x1822cd7] # 0x181bb0500
000000018038D829 cmp       dword ptr [rcx + 0xe0], 0
000000018038D830 jne       0x18038d837
000000018038D832 call      0x1802501e0
000000018038D837 mov       rdx, qword ptr [rip + 0x182a5d2] # 0x181bb7e10
000000018038D83E mov       rcx, rbx
000000018038D841 call      0x1804a0690
000000018038D846 mov       qword ptr [rdi + 0x28], rax
000000018038D84A lea       rcx, [rdi + 0x28]
000000018038D84E mov       rdx, rax
000000018038D851 call      0x18024f360
000000018038D856 mov       rcx, qword ptr [rdi + 0x28]
000000018038D85A test      rcx, rcx
000000018038D85D je        0x18038da90
000000018038D863 xor       edx, edx
000000018038D865 call      0x181304510
000000018038D86A mov       rbx, rax
000000018038D86D mov       rcx, qword ptr [rdi + 0x30]
000000018038D871 test      rcx, rcx
000000018038D874 je        0x18038da90
000000018038D87A xor       edx, edx
000000018038D87C call      0x18131c070
000000018038D881 test      rbx, rbx
000000018038D884 je        0x18038da90
000000018038D88A xor       r8d, r8d
000000018038D88D mov       rdx, rax
000000018038D890 mov       rcx, rbx
000000018038D893 call      0x18132e8b0
000000018038D898 mov       rcx, qword ptr [rdi + 0x28]
000000018038D89C test      rcx, rcx
000000018038D89F je        0x18038da90
000000018038D8A5 xor       edx, edx
000000018038D8A7 call      0x181304510
000000018038D8AC xorps     xmm1, xmm1
000000018038D8AF test      rax, rax
000000018038D8B2 je        0x18038da90
000000018038D8B8 xorps     xmm0, xmm0
000000018038D8BB unpcklps  xmm0, xmm1
000000018038D8BE movsd     qword ptr [rsp + 0x30], xmm0
000000018038D8C4 movss     dword ptr [rsp + 0x38], xmm1
000000018038D8CA xor       r8d, r8d
000000018038D8CD lea       rdx, [rsp + 0x30]
000000018038D8D2 mov       rcx, rax
000000018038D8D5 call      0x18132f910
000000018038D8DA mov       rdx, qword ptr [rip + 0x1836b37] # 0x181bc4418
000000018038D8E1 mov       rcx, qword ptr [rdi + 0x28]
000000018038D8E5 call      0x18042da10
000000018038D8EA mov       rbx, rax
000000018038D8ED test      rax, rax
000000018038D8F0 je        0x18038da90
000000018038D8F6 xor       r8d, r8d
000000018038D8F9 mov       rdx, qword ptr [rip + 0x1834a10] # 0x181bc2310
000000018038D900 mov       rcx, rax
000000018038D903 call      0x181348060
000000018038D908 xor       r8d, r8d
000000018038D90B mov       edx, 0x64
000000018038D910 mov       rcx, rbx
000000018038D913 call      0x1813480b0
000000018038D918 mov       rcx, qword ptr [rdi + 0x28]
000000018038D91C test      rcx, rcx
000000018038D91F je        0x18038da90
000000018038D925 mov       rcx, qword ptr [rcx + 0x70]
000000018038D929 test      rcx, rcx
000000018038D92C je        0x18038d935
000000018038D92E xor       edx, edx
000000018038D930 call      0x180394350
000000018038D935 mov       rax, qword ptr [rdi + 0x28]
000000018038D939 test      rax, rax
000000018038D93C je        0x18038da90
000000018038D942 mov       rcx, qword ptr [rax + 0x1a0]
000000018038D949 test      rcx, rcx
000000018038D94C je        0x18038da90
000000018038D952 xor       r8d, r8d
000000018038D955 xor       edx, edx
000000018038D957 call      0x18131bad0
000000018038D95C mov       rcx, qword ptr [rdi + 0x28]
000000018038D960 test      rcx, rcx
000000018038D963 je        0x18038da90
000000018038D969 mov       rcx, qword ptr [rcx + 0x1b0]
000000018038D970 test      rcx, rcx
000000018038D973 je        0x18038da90
000000018038D979 xor       r8d, r8d
000000018038D97C xor       edx, edx
000000018038D97E call      0x18131bad0
000000018038D983 mov       rdx, qword ptr [rdi + 0x28]
000000018038D987 test      rdx, rdx
000000018038D98A je        0x18038da90
000000018038D990 mov       rdx, qword ptr [rdx + 0x1c8]
000000018038D997 test      rdx, rdx
000000018038D99A je        0x18038da90
000000018038D9A0 mov       r8, qword ptr [rip + 0x181e4d1] # 0x181babe78
000000018038D9A7 lea       rcx, [rsp + 0x30]
000000018038D9AC call      0x1808197f0
000000018038D9B1 movups    xmm0, xmmword ptr [rsp + 0x30]
000000018038D9B6 movups    xmmword ptr [rsp + 0x50], xmm0
000000018038D9BB movsd     xmm1, qword ptr [rsp + 0x40]
000000018038D9C1 movsd     qword ptr [rsp + 0x60], xmm1
000000018038D9C7 mov       qword ptr [rsp + 0x30], 0
000000018038D9D0 lea       rbx, [rsp + 0x50]
000000018038D9D5 mov       qword ptr [rsp + 0x38], rbx
000000018038D9DA nop       word ptr [rax + rax]
000000018038D9E0 mov       rdx, qword ptr [rip + 0x1835ad1] # 0x181bc34b8
000000018038D9E7 lea       rcx, [rsp + 0x50]
000000018038D9EC call      0x180669280
000000018038D9F1 test      al, al
000000018038D9F3 je        0x18038da0b
000000018038D9F5 mov       rcx, qword ptr [rsp + 0x60]
000000018038D9FA test      rcx, rcx
000000018038D9FD je        0x18038d9e0
000000018038D9FF xor       r8d, r8d
000000018038DA02 xor       edx, edx
000000018038DA04 call      0x18131bad0
000000018038DA09 jmp       0x18038d9e0
000000018038DA0B mov       rdx, qword ptr [rip + 0x1835a4e] # 0x181bc3460
000000018038DA12 mov       rcx, rbx
000000018038DA15 call      0x180302170
000000018038DA1A jmp       0x18038da3f
000000018038DA1C mov       rdx, qword ptr [rip + 0x1835a3d] # 0x181bc3460
000000018038DA23 mov       rcx, qword ptr [rsp + 0x38]
000000018038DA28 call      0x180302170
000000018038DA2D mov       rcx, qword ptr [rsp + 0x30]
000000018038DA32 test      rcx, rcx
000000018038DA35 jne       0x18038da96
000000018038DA37 mov       rdi, qword ptr [rsp + 0x80]
000000018038DA3F mov       rcx, qword ptr [rdi + 0x28]
000000018038DA43 test      rcx, rcx
000000018038DA46 je        0x18038da90
000000018038DA48 xor       edx, edx
000000018038DA4A call      0x181304510
000000018038DA4F movss     xmm1, dword ptr [rip + 0x121a169] # 0x1815a7bc0
000000018038DA57 test      rax, rax
000000018038DA5A je        0x18038da90
000000018038DA5C movaps    xmm0, xmm1
000000018038DA5F unpcklps  xmm0, xmm1
000000018038DA62 movsd     qword ptr [rsp + 0x30], xmm0
000000018038DA68 movss     dword ptr [rsp + 0x38], xmm1
000000018038DA6E xor       r8d, r8d
000000018038DA71 lea       rdx, [rsp + 0x30]
000000018038DA76 mov       rcx, rax
000000018038DA79 call      0x18132fa50
000000018038DA7E lea       r11, [rsp + 0x70]
000000018038DA83 mov       rbx, qword ptr [r11 + 0x18]
000000018038DA87 mov       rsi, qword ptr [r11 + 0x20]
000000018038DA8B mov       rsp, r11
000000018038DA8E pop       rdi
000000018038DA8F ret       
000000018038DA90 call      0x180250150
000000018038DA95 int3      
000000018038DA96 call      0x180200e60
000000018038DA9B int3      
000000018038DA9C int3      
000000018038DA9D int3      
000000018038DA9E int3      
000000018038DA9F int3      