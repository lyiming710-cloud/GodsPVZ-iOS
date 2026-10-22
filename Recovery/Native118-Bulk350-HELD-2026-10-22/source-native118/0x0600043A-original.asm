000000018035F560 mov       qword ptr [rsp + 0x18], rbx
000000018035F565 push      rbp
000000018035F566 sub       rsp, 0x50
000000018035F56A cmp       byte ptr [rip + 0x19857e4], 0 # 0x181ce4d55
000000018035F571 movzx     ebp, dl
000000018035F574 mov       rbx, rcx
000000018035F577 jne       0x18035f668
000000018035F57D lea       rcx, [rip + 0x1842a94] # 0x181ba2018
000000018035F584 call      0x18024fef0
000000018035F589 lea       rcx, [rip + 0x186de00] # 0x181bcd390
000000018035F590 call      0x18024fef0
000000018035F595 lea       rcx, [rip + 0x186df9c] # 0x181bcd538
000000018035F59C call      0x18024fef0
000000018035F5A1 lea       rcx, [rip + 0x186e130] # 0x181bcd6d8
000000018035F5A8 call      0x18024fef0
000000018035F5AD lea       rcx, [rip + 0x186e17c] # 0x181bcd730
000000018035F5B4 call      0x18024fef0
000000018035F5B9 lea       rcx, [rip + 0x186ded0] # 0x181bcd490
000000018035F5C0 call      0x18024fef0
000000018035F5C5 lea       rcx, [rip + 0x1875c0c] # 0x181bd51d8
000000018035F5CC call      0x18024fef0
000000018035F5D1 lea       rcx, [rip + 0x186e200] # 0x181bcd7d8
000000018035F5D8 call      0x18024fef0
000000018035F5DD lea       rcx, [rip + 0x1875aec] # 0x181bd50d0
000000018035F5E4 call      0x18024fef0
000000018035F5E9 lea       rcx, [rip + 0x186def0] # 0x181bcd4e0
000000018035F5F0 call      0x18024fef0
000000018035F5F5 lea       rcx, [rip + 0x186e034] # 0x181bcd630
000000018035F5FC call      0x18024fef0
000000018035F601 lea       rcx, [rip + 0x186dde0] # 0x181bcd3e8
000000018035F608 call      0x18024fef0
000000018035F60D lea       rcx, [rip + 0x1847154] # 0x181ba6768
000000018035F614 call      0x18024fef0
000000018035F619 lea       rcx, [rip + 0x186de18] # 0x181bcd438
000000018035F620 call      0x18024fef0
000000018035F625 lea       rcx, [rip + 0x186e3f4] # 0x181bcda20
000000018035F62C call      0x18024fef0
000000018035F631 lea       rcx, [rip + 0x186fa50] # 0x181bcf088
000000018035F638 call      0x18024fef0
000000018035F63D lea       rcx, [rip + 0x186d714] # 0x181bccd58
000000018035F644 call      0x18024fef0
000000018035F649 lea       rcx, [rip + 0x1875818] # 0x181bd4e68
000000018035F650 call      0x18024fef0
000000018035F655 lea       rcx, [rip + 0x186df2c] # 0x181bcd588
000000018035F65C call      0x18024fef0
000000018035F661 mov       byte ptr [rip + 0x19856ed], 1 # 0x181ce4d55
000000018035F668 mov       qword ptr [rsp + 0x60], rsi
000000018035F66D mov       qword ptr [rsp + 0x68], rdi
000000018035F672 movaps    xmmword ptr [rsp + 0x40], xmm6
000000018035F677 test      bpl, bpl
000000018035F67A jne       0x18035f8a3
000000018035F680 mov       ecx, dword ptr [rbx + 0x9c]
000000018035F686 sub       ecx, 1
000000018035F689 je        0x18035f7ed
000000018035F68F sub       ecx, 1
000000018035F692 je        0x18035f782
000000018035F698 sub       ecx, 1
000000018035F69B je        0x18035f714
000000018035F69D cmp       ecx, 1
000000018035F6A0 jne       0x18035f8a3
000000018035F6A6 cmp       byte ptr [rip + 0x1985487], bpl # 0x181ce4b34
000000018035F6AD jne       0x18035f6c2
000000018035F6AF lea       rcx, [rip + 0x18521da] # 0x181bb1890
000000018035F6B6 call      0x18024fef0
000000018035F6BB mov       byte ptr [rip + 0x1985472], 1 # 0x181ce4b34
000000018035F6C2 mov       rax, qword ptr [rip + 0x18521c7] # 0x181bb1890
000000018035F6C9 mov       rcx, qword ptr [rax + 0xb8]
000000018035F6D0 mov       rcx, qword ptr [rcx]
000000018035F6D3 test      rcx, rcx
000000018035F6D6 je        0x18035fc11
000000018035F6DC xor       r8d, r8d
000000018035F6DF mov       edx, 0x11
000000018035F6E4 call      0x180321f30
000000018035F6E9 mov       rcx, qword ptr [rip + 0x1842928] # 0x181ba2018
000000018035F6F0 mov       rdi, rax
000000018035F6F3 mov       rsi, qword ptr [rbx + 0x1b0]
000000018035F6FA cmp       dword ptr [rcx + 0xe0], 0
000000018035F701 jne       0x18035f708
000000018035F703 call      0x1802501e0
000000018035F708 mov       r8, qword ptr [rip + 0x18759c1] # 0x181bd50d0
000000018035F70F jmp       0x18035f856
000000018035F714 cmp       byte ptr [rip + 0x1985419], 0 # 0x181ce4b34
000000018035F71B jne       0x18035f730
000000018035F71D lea       rcx, [rip + 0x185216c] # 0x181bb1890
000000018035F724 call      0x18024fef0
000000018035F729 mov       byte ptr [rip + 0x1985404], 1 # 0x181ce4b34
000000018035F730 mov       rax, qword ptr [rip + 0x1852159] # 0x181bb1890
000000018035F737 mov       rcx, qword ptr [rax + 0xb8]
000000018035F73E mov       rcx, qword ptr [rcx]
000000018035F741 test      rcx, rcx
000000018035F744 je        0x18035fc11
000000018035F74A xor       r8d, r8d
000000018035F74D mov       edx, 9
000000018035F752 call      0x180321f30
000000018035F757 mov       rcx, qword ptr [rip + 0x18428ba] # 0x181ba2018
000000018035F75E mov       rdi, rax
000000018035F761 mov       rsi, qword ptr [rbx + 0x1b0]
000000018035F768 cmp       dword ptr [rcx + 0xe0], 0
000000018035F76F jne       0x18035f776
000000018035F771 call      0x1802501e0
000000018035F776 mov       r8, qword ptr [rip + 0x186d5db] # 0x181bccd58
000000018035F77D jmp       0x18035f856
000000018035F782 cmp       byte ptr [rip + 0x19853ab], 0 # 0x181ce4b34
000000018035F789 jne       0x18035f79e
000000018035F78B lea       rcx, [rip + 0x18520fe] # 0x181bb1890
000000018035F792 call      0x18024fef0
000000018035F797 mov       byte ptr [rip + 0x1985396], 1 # 0x181ce4b34
000000018035F79E mov       rax, qword ptr [rip + 0x18520eb] # 0x181bb1890
000000018035F7A5 mov       rcx, qword ptr [rax + 0xb8]
000000018035F7AC mov       rcx, qword ptr [rcx]
000000018035F7AF test      rcx, rcx
000000018035F7B2 je        0x18035fc11
000000018035F7B8 xor       r8d, r8d
000000018035F7BB mov       edx, 0x14
000000018035F7C0 call      0x180321f30
000000018035F7C5 mov       rcx, qword ptr [rip + 0x184284c] # 0x181ba2018
000000018035F7CC mov       rdi, rax
000000018035F7CF mov       rsi, qword ptr [rbx + 0x1b0]
000000018035F7D6 cmp       dword ptr [rcx + 0xe0], 0
000000018035F7DD jne       0x18035f7e4
000000018035F7DF call      0x1802501e0
000000018035F7E4 mov       r8, qword ptr [rip + 0x186f89d] # 0x181bcf088
000000018035F7EB jmp       0x18035f856
000000018035F7ED cmp       byte ptr [rip + 0x1985340], 0 # 0x181ce4b34
000000018035F7F4 jne       0x18035f809
000000018035F7F6 lea       rcx, [rip + 0x1852093] # 0x181bb1890
000000018035F7FD call      0x18024fef0
000000018035F802 mov       byte ptr [rip + 0x198532b], 1 # 0x181ce4b34
000000018035F809 mov       rax, qword ptr [rip + 0x1852080] # 0x181bb1890
000000018035F810 mov       rcx, qword ptr [rax + 0xb8]
000000018035F817 mov       rcx, qword ptr [rcx]
000000018035F81A test      rcx, rcx
000000018035F81D je        0x18035fc11
000000018035F823 xor       r8d, r8d
000000018035F826 mov       edx, 0x1c
000000018035F82B call      0x180321f30
000000018035F830 mov       rcx, qword ptr [rip + 0x18427e1] # 0x181ba2018
000000018035F837 mov       rdi, rax
000000018035F83A mov       rsi, qword ptr [rbx + 0x1b0]
000000018035F841 cmp       dword ptr [rcx + 0xe0], 0
000000018035F848 jne       0x18035f84f
000000018035F84A call      0x1802501e0
000000018035F84F mov       r8, qword ptr [rip + 0x186e1ca] # 0x181bcda20
000000018035F856 xor       r9d, r9d
000000018035F859 lea       rcx, [rsp + 0x30]
000000018035F85E mov       rdx, rsi
000000018035F861 call      0x18031b680
000000018035F866 mov       esi, dword ptr [rax + 8]
000000018035F869 movsd     xmm6, qword ptr [rax]
000000018035F86D test      rdi, rdi
000000018035F870 je        0x18035fc11
000000018035F876 xor       edx, edx
000000018035F878 mov       rcx, rdi
000000018035F87B call      0x18131c070
000000018035F880 test      rax, rax
000000018035F883 je        0x18035fc11
000000018035F889 xor       r8d, r8d
000000018035F88C movsd     qword ptr [rsp + 0x30], xmm6
000000018035F892 lea       rdx, [rsp + 0x30]
000000018035F897 mov       dword ptr [rsp + 0x38], esi
000000018035F89B mov       rcx, rax
000000018035F89E call      0x18132fbb0
000000018035F8A3 mov       ecx, dword ptr [rbx + 0x9c]
000000018035F8A9 mov       rdi, qword ptr [rip + 0x186f7d8] # 0x181bcf088
000000018035F8B0 sub       ecx, 1
000000018035F8B3 je        0x18035fa69
000000018035F8B9 sub       ecx, 1
000000018035F8BC je        0x18035fb7b
000000018035F8C2 sub       ecx, 1
000000018035F8C5 je        0x18035f97f
000000018035F8CB cmp       ecx, 1
000000018035F8CE jne       0x18035fb7b
000000018035F8D4 mov       rcx, qword ptr [rip + 0x184273d] # 0x181ba2018
000000018035F8DB mov       rsi, qword ptr [rbx + 0x1b0]
000000018035F8E2 cmp       dword ptr [rcx + 0xe0], 0
000000018035F8E9 jne       0x18035f8f0
000000018035F8EB call      0x1802501e0
000000018035F8F0 mov       rdx, qword ptr [rip + 0x18757d9] # 0x181bd50d0
000000018035F8F7 xor       r8d, r8d
000000018035F8FA mov       rcx, rsi
000000018035F8FD call      0x18031bac0
000000018035F902 mov       rdx, qword ptr [rip + 0x18758cf] # 0x181bd51d8
000000018035F909 xor       r8d, r8d
000000018035F90C mov       rcx, qword ptr [rbx + 0x1b0]
000000018035F913 call      0x18031bac0
000000018035F918 mov       rdx, qword ptr [rip + 0x186da71] # 0x181bcd390
000000018035F91F xor       r8d, r8d
000000018035F922 mov       rcx, qword ptr [rbx + 0x1b0]
000000018035F929 call      0x18031adf0
000000018035F92E mov       rdx, qword ptr [rip + 0x186dc53] # 0x181bcd588
000000018035F935 xor       r8d, r8d
000000018035F938 mov       rcx, qword ptr [rbx + 0x1b0]
000000018035F93F call      0x18031adf0
000000018035F944 mov       rcx, qword ptr [rbx + 0x158]
000000018035F94B test      rcx, rcx
000000018035F94E je        0x18035fc11
000000018035F954 mov       rdx, qword ptr [rip + 0x187550d] # 0x181bd4e68
000000018035F95B xor       r9d, r9d
000000018035F95E xor       r8d, r8d
000000018035F961 call      0x1812dba00
000000018035F966 sub       dword ptr [rbx + 0x11c], 0x3e8
000000018035F970 xor       edx, edx
000000018035F972 mov       rcx, rbx
000000018035F975 call      0x180365ea0
000000018035F97A jmp       0x18035fb7b
000000018035F97F mov       rcx, qword ptr [rip + 0x1842692] # 0x181ba2018
000000018035F986 mov       rdi, qword ptr [rbx + 0x1b0]
000000018035F98D cmp       dword ptr [rcx + 0xe0], 0
000000018035F994 jne       0x18035f99b
000000018035F996 call      0x1802501e0
000000018035F99B mov       rdx, qword ptr [rip + 0x186dd8e] # 0x181bcd730
000000018035F9A2 xor       r8d, r8d
000000018035F9A5 mov       rcx, rdi
000000018035F9A8 call      0x18031bac0
000000018035F9AD mov       rdx, qword ptr [rip + 0x186db84] # 0x181bcd538
000000018035F9B4 xor       r8d, r8d
000000018035F9B7 mov       rcx, qword ptr [rbx + 0x1b0]
000000018035F9BE call      0x18031bac0
000000018035F9C3 mov       rdx, qword ptr [rip + 0x186dac6] # 0x181bcd490
000000018035F9CA xor       r8d, r8d
000000018035F9CD mov       rcx, qword ptr [rbx + 0x1b0]
000000018035F9D4 call      0x18031bac0
000000018035F9D9 mov       rdx, qword ptr [rip + 0x186dcf8] # 0x181bcd6d8
000000018035F9E0 xor       r8d, r8d
000000018035F9E3 mov       rcx, qword ptr [rbx + 0x1b0]
000000018035F9EA call      0x18031adf0
000000018035F9EF mov       rdx, qword ptr [rip + 0x186dde2] # 0x181bcd7d8
000000018035F9F6 xor       r8d, r8d
000000018035F9F9 mov       rcx, qword ptr [rbx + 0x1b0]
000000018035FA00 call      0x18031adf0
000000018035FA05 mov       rdx, qword ptr [rip + 0x186dc24] # 0x181bcd630
000000018035FA0C xor       r8d, r8d
000000018035FA0F mov       rcx, qword ptr [rbx + 0x1b0]
000000018035FA16 call      0x18031adf0
000000018035FA1B mov       rdx, qword ptr [rip + 0x186dabe] # 0x181bcd4e0
000000018035FA22 xor       r8d, r8d
000000018035FA25 mov       rcx, qword ptr [rbx + 0x1b0]
000000018035FA2C call      0x18031adf0
000000018035FA31 mov       rdx, qword ptr [rip + 0x186da00] # 0x181bcd438
000000018035FA38 xor       r8d, r8d
000000018035FA3B mov       rcx, qword ptr [rbx + 0x1b0]
000000018035FA42 call      0x18031adf0
000000018035FA47 mov       rdx, qword ptr [rip + 0x186d99a] # 0x181bcd3e8
000000018035FA4E xor       r8d, r8d
000000018035FA51 mov       rcx, qword ptr [rbx + 0x1b0]
000000018035FA58 call      0x18031adf0
000000018035FA5D mov       rdi, qword ptr [rip + 0x186d2f4] # 0x181bccd58
000000018035FA64 jmp       0x18035fb7b
000000018035FA69 mov       rcx, qword ptr [rip + 0x18425a8] # 0x181ba2018
000000018035FA70 mov       rdi, qword ptr [rbx + 0x1b0]
000000018035FA77 cmp       dword ptr [rcx + 0xe0], 0
000000018035FA7E jne       0x18035fa85
000000018035FA80 call      0x1802501e0
000000018035FA85 mov       rdx, qword ptr [rip + 0x186dca4] # 0x181bcd730
000000018035FA8C xor       r8d, r8d
000000018035FA8F mov       rcx, rdi
000000018035FA92 call      0x18031bac0
000000018035FA97 mov       rdx, qword ptr [rip + 0x186dc3a] # 0x181bcd6d8
000000018035FA9E xor       r8d, r8d
000000018035FAA1 mov       rcx, qword ptr [rbx + 0x1b0]
000000018035FAA8 call      0x18031adf0
000000018035FAAD mov       rdx, qword ptr [rip + 0x186dd24] # 0x181bcd7d8
000000018035FAB4 xor       r8d, r8d
000000018035FAB7 mov       rcx, qword ptr [rbx + 0x1b0]
000000018035FABE call      0x18031adf0
000000018035FAC3 mov       rdx, qword ptr [rip + 0x186db66] # 0x181bcd630
000000018035FACA xor       r8d, r8d
000000018035FACD mov       rcx, qword ptr [rbx + 0x1b0]
000000018035FAD4 call      0x18031adf0
000000018035FAD9 mov       eax, dword ptr [rbx + 0x60]
000000018035FADC cmp       eax, 1
000000018035FADF je        0x18035fb74
000000018035FAE5 cmp       eax, 0xa
000000018035FAE8 je        0x18035fb74
000000018035FAEE mov       rcx, qword ptr [rip + 0x1842523] # 0x181ba2018
000000018035FAF5 mov       rdi, qword ptr [rbx + 0x1b0]
000000018035FAFC cmp       dword ptr [rcx + 0xe0], 0
000000018035FB03 jne       0x18035fb0a
000000018035FB05 call      0x1802501e0
000000018035FB0A mov       rdx, qword ptr [rip + 0x186da27] # 0x181bcd538
000000018035FB11 xor       r8d, r8d
000000018035FB14 mov       rcx, rdi
000000018035FB17 call      0x18031bac0
000000018035FB1C mov       rdx, qword ptr [rip + 0x186d96d] # 0x181bcd490
000000018035FB23 xor       r8d, r8d
000000018035FB26 mov       rcx, qword ptr [rbx + 0x1b0]
000000018035FB2D call      0x18031bac0
000000018035FB32 mov       rdx, qword ptr [rip + 0x186d9a7] # 0x181bcd4e0
000000018035FB39 xor       r8d, r8d
000000018035FB3C mov       rcx, qword ptr [rbx + 0x1b0]
000000018035FB43 call      0x18031adf0
000000018035FB48 mov       rdx, qword ptr [rip + 0x186d8e9] # 0x181bcd438
000000018035FB4F xor       r8d, r8d
000000018035FB52 mov       rcx, qword ptr [rbx + 0x1b0]
000000018035FB59 call      0x18031adf0
000000018035FB5E mov       rdx, qword ptr [rip + 0x186d883] # 0x181bcd3e8
000000018035FB65 xor       r8d, r8d
000000018035FB68 mov       rcx, qword ptr [rbx + 0x1b0]
000000018035FB6F call      0x18031adf0
000000018035FB74 mov       rdi, qword ptr [rip + 0x186dea5] # 0x181bcda20
000000018035FB7B mov       rcx, qword ptr [rip + 0x1842496] # 0x181ba2018
000000018035FB82 mov       rsi, qword ptr [rbx + 0x1b0]
000000018035FB89 cmp       dword ptr [rcx + 0xe0], 0
000000018035FB90 jne       0x18035fb97
000000018035FB92 call      0x1802501e0
000000018035FB97 xor       r8d, r8d
000000018035FB9A mov       rdx, rdi
000000018035FB9D mov       rcx, rsi
000000018035FBA0 call      0x18031bac0
000000018035FBA5 movss     xmm2, dword ptr [rip + 0x1247e63] # 0x1815a7a10
000000018035FBAD xor       eax, eax
000000018035FBAF mov       qword ptr [rbx + 0xa0], rax
000000018035FBB6 xorps     xmm1, xmm1
000000018035FBB9 mov       dword ptr [rbx + 0xa8], eax
000000018035FBBF mov       rcx, rbx
000000018035FBC2 mov       dword ptr [rbx + 0x9c], eax
000000018035FBC8 mov       dword ptr [rbx + 0xb0], eax
000000018035FBCE mov       r9, qword ptr [rip + 0x1846b93] # 0x181ba6768
000000018035FBD5 mov       qword ptr [rsp + 0x20], rax
000000018035FBDA call      0x180360110
000000018035FBDF cmp       byte ptr [rbx + 0xb6], 0
000000018035FBE6 je        0x18035fbf7
000000018035FBE8 xor       r8d, r8d
000000018035FBEB movzx     edx, bpl
000000018035FBEF mov       rcx, rbx
000000018035FBF2 call      0x18035d0a0
000000018035FBF7 movaps    xmm6, xmmword ptr [rsp + 0x40]
000000018035FBFC mov       rdi, qword ptr [rsp + 0x68]
000000018035FC01 mov       rsi, qword ptr [rsp + 0x60]
000000018035FC06 mov       rbx, qword ptr [rsp + 0x70]
000000018035FC0B add       rsp, 0x50
000000018035FC0F pop       rbp
000000018035FC10 ret       
000000018035FC11 call      0x180250150
000000018035FC16 int3      
000000018035FC17 int3      
000000018035FC18 int3      
000000018035FC19 int3      
000000018035FC1A int3      
000000018035FC1B int3      
000000018035FC1C int3      
000000018035FC1D int3      
000000018035FC1E int3      
000000018035FC1F int3      