000000018032D4E0 push      rbx
000000018032D4E2 push      rsi
000000018032D4E3 push      r12
000000018032D4E5 push      r13
000000018032D4E7 push      r14
000000018032D4E9 sub       rsp, 0x20
000000018032D4ED cmp       byte ptr [rip + 0x19b7787], 0 # 0x181ce4c7b
000000018032D4F4 mov       rsi, r9
000000018032D4F7 mov       r13d, r8d
000000018032D4FA mov       r14d, edx
000000018032D4FD mov       r12, rcx
000000018032D500 jne       0x18032d551
000000018032D502 lea       rcx, [rip + 0x186d83f] # 0x181b9ad48
000000018032D509 call      0x18024fef0
000000018032D50E lea       rcx, [rip + 0x1885753] # 0x181bb2c68
000000018032D515 call      0x18024fef0
000000018032D51A lea       rcx, [rip + 0x18856ef] # 0x181bb2c10
000000018032D521 call      0x18024fef0
000000018032D526 lea       rcx, [rip + 0x188589b] # 0x181bb2dc8
000000018032D52D call      0x18024fef0
000000018032D532 lea       rcx, [rip + 0x1891adf] # 0x181bbf018
000000018032D539 call      0x18024fef0
000000018032D53E lea       rcx, [rip + 0x1875223] # 0x181ba2768
000000018032D545 call      0x18024fef0
000000018032D54A mov       byte ptr [rip + 0x19b772a], 1 # 0x181ce4c7b
000000018032D551 mov       rcx, qword ptr [rip + 0x1891ac0] # 0x181bbf018
000000018032D558 mov       qword ptr [rsp + 0x50], rbp
000000018032D55D mov       qword ptr [rsp + 0x58], rdi
000000018032D562 mov       qword ptr [rsp + 0x60], r15
000000018032D567 call      0x180250100
000000018032D56C mov       rdx, qword ptr [rip + 0x188569d] # 0x181bb2c10
000000018032D573 mov       rcx, rax
000000018032D576 mov       rbx, rax
000000018032D579 call      0x1808053d0
000000018032D57E test      r12, r12
000000018032D581 je        0x18032d935
000000018032D587 mov       ecx, r14d
000000018032D58A imul      ecx, r13d
000000018032D58E cmp       dword ptr [r12 + 0x18], ecx
000000018032D593 je        0x18032d5b8
000000018032D595 mov       rcx, qword ptr [rip + 0x186d7ac] # 0x181b9ad48
000000018032D59C cmp       dword ptr [rcx + 0xe0], 0
000000018032D5A3 jne       0x18032d5aa
000000018032D5A5 call      0x1802501e0
000000018032D5AA mov       rcx, qword ptr [rip + 0x18751b7] # 0x181ba2768
000000018032D5B1 xor       edx, edx
000000018032D5B3 call      0x1812e68f0
000000018032D5B8 test      rsi, rsi
000000018032D5BB je        0x18032d935
000000018032D5C1 xor       edi, edi
000000018032D5C3 cmp       dword ptr [rsi + 0x14], edi
000000018032D5C6 jle       0x18032d63f
000000018032D5C8 mov       ebp, dword ptr [rsi + 0x14]
000000018032D5CB mov       r15d, dword ptr [rsi + 0x10]
000000018032D5CF dec       ebp
000000018032D5D1 cmp       byte ptr [rip + 0x19b76a2], dil # 0x181ce4c7a
000000018032D5D8 jne       0x18032d5f9
000000018032D5DA lea       rcx, [rip + 0x18857e7] # 0x181bb2dc8
000000018032D5E1 call      0x18024fef0
000000018032D5E6 lea       rcx, [rip + 0x1885833] # 0x181bb2e20
000000018032D5ED call      0x18024fef0
000000018032D5F2 mov       byte ptr [rip + 0x19b7681], 1 # 0x181ce4c7a
000000018032D5F9 cmp       r15d, r14d
000000018032D5FC jge       0x18032d61f
000000018032D5FE imul      ebp, r14d
000000018032D602 add       ebp, r15d
000000018032D605 cmp       dword ptr [r12 + 0x18], ebp
000000018032D60A jle       0x18032d61f
000000018032D60C mov       r8, qword ptr [rip + 0x188580d] # 0x181bb2e20
000000018032D613 mov       edx, ebp
000000018032D615 mov       rcx, r12
000000018032D618 call      0x180825710
000000018032D61D jmp       0x18032d622
000000018032D61F mov       rax, rdi
000000018032D622 test      rbx, rbx
000000018032D625 je        0x18032d935
000000018032D62B mov       r8, qword ptr [rip + 0x1885636] # 0x181bb2c68
000000018032D632 mov       rdx, rax
000000018032D635 mov       rcx, rbx
000000018032D638 call      0x180002e70
000000018032D63D jmp       0x18032d6a6
000000018032D63F test      rbx, rbx
000000018032D642 je        0x18032d935
000000018032D648 mov       r8, qword ptr [rip + 0x1885619] # 0x181bb2c68
000000018032D64F inc       dword ptr [rbx + 0x1c]
000000018032D652 mov       rcx, qword ptr [rbx + 0x10]
000000018032D656 movsxd    rdx, dword ptr [rbx + 0x18]
000000018032D65A test      rcx, rcx
000000018032D65D je        0x18032d935
000000018032D663 cmp       edx, dword ptr [rcx + 0x18]
000000018032D666 jb        0x18032d683
000000018032D668 mov       rax, qword ptr [r8 + 0x20]
000000018032D66C xor       edx, edx
000000018032D66E mov       rcx, rbx
000000018032D671 mov       r8, qword ptr [rax + 0xc0]
000000018032D678 mov       r8, qword ptr [r8 + 0x70]
000000018032D67C call      0x180847ce0
000000018032D681 jmp       0x18032d6a6
000000018032D683 lea       eax, [rdx + 1]
000000018032D686 mov       dword ptr [rbx + 0x18], eax
000000018032D689 cmp       edx, dword ptr [rcx + 0x18]
000000018032D68C jae       0x18032d93b
000000018032D692 mov       qword ptr [rcx + rdx*8 + 0x20], rdi
000000018032D697 lea       rcx, [rcx + rdx*8]
000000018032D69B add       rcx, 0x20
000000018032D69F xor       edx, edx
000000018032D6A1 call      0x18024f360
000000018032D6A6 lea       eax, [r14 - 1]
000000018032D6AA cmp       dword ptr [rsi + 0x10], eax
000000018032D6AD jge       0x18032d71e
000000018032D6AF mov       r15d, dword ptr [rsi + 0x10]
000000018032D6B3 mov       ebp, dword ptr [rsi + 0x14]
000000018032D6B6 inc       r15d
000000018032D6B9 cmp       byte ptr [rip + 0x19b75ba], dil # 0x181ce4c7a
000000018032D6C0 jne       0x18032d6e1
000000018032D6C2 lea       rcx, [rip + 0x18856ff] # 0x181bb2dc8
000000018032D6C9 call      0x18024fef0
000000018032D6CE lea       rcx, [rip + 0x188574b] # 0x181bb2e20
000000018032D6D5 call      0x18024fef0
000000018032D6DA mov       byte ptr [rip + 0x19b7599], 1 # 0x181ce4c7a
000000018032D6E1 cmp       r15d, r14d
000000018032D6E4 jge       0x18032d707
000000018032D6E6 imul      ebp, r14d
000000018032D6EA add       ebp, r15d
000000018032D6ED cmp       dword ptr [r12 + 0x18], ebp
000000018032D6F2 jle       0x18032d707
000000018032D6F4 mov       r8, qword ptr [rip + 0x1885725] # 0x181bb2e20
000000018032D6FB mov       edx, ebp
000000018032D6FD mov       rcx, r12
000000018032D700 call      0x180825710
000000018032D705 jmp       0x18032d70a
000000018032D707 mov       rax, rdi
000000018032D70A mov       r8, qword ptr [rip + 0x1885557] # 0x181bb2c68
000000018032D711 mov       rdx, rax
000000018032D714 mov       rcx, rbx
000000018032D717 call      0x180002e70
000000018032D71C jmp       0x18032d77c
000000018032D71E mov       r8, qword ptr [rip + 0x1885543] # 0x181bb2c68
000000018032D725 inc       dword ptr [rbx + 0x1c]
000000018032D728 mov       rcx, qword ptr [rbx + 0x10]
000000018032D72C movsxd    rdx, dword ptr [rbx + 0x18]
000000018032D730 test      rcx, rcx
000000018032D733 je        0x18032d935
000000018032D739 cmp       edx, dword ptr [rcx + 0x18]
000000018032D73C jb        0x18032d759
000000018032D73E mov       rax, qword ptr [r8 + 0x20]
000000018032D742 xor       edx, edx
000000018032D744 mov       rcx, rbx
000000018032D747 mov       r8, qword ptr [rax + 0xc0]
000000018032D74E mov       r8, qword ptr [r8 + 0x70]
000000018032D752 call      0x180847ce0
000000018032D757 jmp       0x18032d77c
000000018032D759 lea       eax, [rdx + 1]
000000018032D75C mov       dword ptr [rbx + 0x18], eax
000000018032D75F cmp       edx, dword ptr [rcx + 0x18]
000000018032D762 jae       0x18032d93b
000000018032D768 mov       qword ptr [rcx + rdx*8 + 0x20], rdi
000000018032D76D lea       rcx, [rcx + rdx*8]
000000018032D771 add       rcx, 0x20
000000018032D775 xor       edx, edx
000000018032D777 call      0x18024f360
000000018032D77C lea       eax, [r13 - 1]
000000018032D780 cmp       dword ptr [rsi + 0x14], eax
000000018032D783 jge       0x18032d7f3
000000018032D785 mov       ebp, dword ptr [rsi + 0x14]
000000018032D788 mov       r15d, dword ptr [rsi + 0x10]
000000018032D78C inc       ebp
000000018032D78E cmp       byte ptr [rip + 0x19b74e5], dil # 0x181ce4c7a
000000018032D795 jne       0x18032d7b6
000000018032D797 lea       rcx, [rip + 0x188562a] # 0x181bb2dc8
000000018032D79E call      0x18024fef0
000000018032D7A3 lea       rcx, [rip + 0x1885676] # 0x181bb2e20
000000018032D7AA call      0x18024fef0
000000018032D7AF mov       byte ptr [rip + 0x19b74c4], 1 # 0x181ce4c7a
000000018032D7B6 cmp       r15d, r14d
000000018032D7B9 jge       0x18032d7dc
000000018032D7BB imul      ebp, r14d
000000018032D7BF add       ebp, r15d
000000018032D7C2 cmp       dword ptr [r12 + 0x18], ebp
000000018032D7C7 jle       0x18032d7dc
000000018032D7C9 mov       r8, qword ptr [rip + 0x1885650] # 0x181bb2e20
000000018032D7D0 mov       edx, ebp
000000018032D7D2 mov       rcx, r12
000000018032D7D5 call      0x180825710
000000018032D7DA jmp       0x18032d7df
000000018032D7DC mov       rax, rdi
000000018032D7DF mov       r8, qword ptr [rip + 0x1885482] # 0x181bb2c68
000000018032D7E6 mov       rdx, rax
000000018032D7E9 mov       rcx, rbx
000000018032D7EC call      0x180002e70
000000018032D7F1 jmp       0x18032d851
000000018032D7F3 mov       r8, qword ptr [rip + 0x188546e] # 0x181bb2c68
000000018032D7FA inc       dword ptr [rbx + 0x1c]
000000018032D7FD mov       rcx, qword ptr [rbx + 0x10]
000000018032D801 movsxd    rdx, dword ptr [rbx + 0x18]
000000018032D805 test      rcx, rcx
000000018032D808 je        0x18032d935
000000018032D80E cmp       edx, dword ptr [rcx + 0x18]
000000018032D811 jb        0x18032d82e
000000018032D813 mov       rax, qword ptr [r8 + 0x20]
000000018032D817 xor       edx, edx
000000018032D819 mov       rcx, rbx
000000018032D81C mov       r8, qword ptr [rax + 0xc0]
000000018032D823 mov       r8, qword ptr [r8 + 0x70]
000000018032D827 call      0x180847ce0
000000018032D82C jmp       0x18032d851
000000018032D82E lea       eax, [rdx + 1]
000000018032D831 mov       dword ptr [rbx + 0x18], eax
000000018032D834 cmp       edx, dword ptr [rcx + 0x18]
000000018032D837 jae       0x18032d93b
000000018032D83D mov       qword ptr [rcx + rdx*8 + 0x20], rdi
000000018032D842 lea       rcx, [rcx + rdx*8]
000000018032D846 add       rcx, 0x20
000000018032D84A xor       edx, edx
000000018032D84C call      0x18024f360
000000018032D851 cmp       dword ptr [rsi + 0x10], edi
000000018032D854 jle       0x18032d8c0
000000018032D856 mov       ebp, dword ptr [rsi + 0x14]
000000018032D859 mov       esi, dword ptr [rsi + 0x10]
000000018032D85C dec       esi
000000018032D85E cmp       byte ptr [rip + 0x19b7415], dil # 0x181ce4c7a
000000018032D865 jne       0x18032d886
000000018032D867 lea       rcx, [rip + 0x188555a] # 0x181bb2dc8
000000018032D86E call      0x18024fef0
000000018032D873 lea       rcx, [rip + 0x18855a6] # 0x181bb2e20
000000018032D87A call      0x18024fef0
000000018032D87F mov       byte ptr [rip + 0x19b73f4], 1 # 0x181ce4c7a
000000018032D886 cmp       esi, r14d
000000018032D889 jge       0x18032d8ac
000000018032D88B imul      ebp, r14d
000000018032D88F add       ebp, esi
000000018032D891 cmp       dword ptr [r12 + 0x18], ebp
000000018032D896 jle       0x18032d8ac
000000018032D898 mov       r8, qword ptr [rip + 0x1885581] # 0x181bb2e20
000000018032D89F mov       edx, ebp
000000018032D8A1 mov       rcx, r12
000000018032D8A4 call      0x180825710
000000018032D8A9 mov       rdi, rax
000000018032D8AC mov       r8, qword ptr [rip + 0x18853b5] # 0x181bb2c68
000000018032D8B3 mov       rdx, rdi
000000018032D8B6 mov       rcx, rbx
000000018032D8B9 call      0x180002e70
000000018032D8BE jmp       0x18032d916
000000018032D8C0 mov       r8, qword ptr [rip + 0x18853a1] # 0x181bb2c68
000000018032D8C7 inc       dword ptr [rbx + 0x1c]
000000018032D8CA mov       rcx, qword ptr [rbx + 0x10]
000000018032D8CE movsxd    rdx, dword ptr [rbx + 0x18]
000000018032D8D2 test      rcx, rcx
000000018032D8D5 je        0x18032d935
000000018032D8D7 cmp       edx, dword ptr [rcx + 0x18]
000000018032D8DA jb        0x18032d8f7
000000018032D8DC mov       rax, qword ptr [r8 + 0x20]
000000018032D8E0 xor       edx, edx
000000018032D8E2 mov       rcx, rbx
000000018032D8E5 mov       r8, qword ptr [rax + 0xc0]
000000018032D8EC mov       r8, qword ptr [r8 + 0x70]
000000018032D8F0 call      0x180847ce0
000000018032D8F5 jmp       0x18032d916
000000018032D8F7 lea       eax, [rdx + 1]
000000018032D8FA mov       dword ptr [rbx + 0x18], eax
000000018032D8FD cmp       edx, dword ptr [rcx + 0x18]
000000018032D900 jae       0x18032d93b
000000018032D902 mov       qword ptr [rcx + rdx*8 + 0x20], rdi
000000018032D907 lea       rcx, [rcx + rdx*8]
000000018032D90B add       rcx, 0x20
000000018032D90F xor       edx, edx
000000018032D911 call      0x18024f360
000000018032D916 mov       r15, qword ptr [rsp + 0x60]
000000018032D91B mov       rax, rbx
000000018032D91E mov       rdi, qword ptr [rsp + 0x58]
000000018032D923 mov       rbp, qword ptr [rsp + 0x50]
000000018032D928 add       rsp, 0x20
000000018032D92C pop       r14
000000018032D92E pop       r13
000000018032D930 pop       r12
000000018032D932 pop       rsi
000000018032D933 pop       rbx
000000018032D934 ret       
000000018032D935 call      0x180250150
000000018032D93A int3      
000000018032D93B call      0x180250140
000000018032D940 int3      
000000018032D941 int3      
000000018032D942 int3      
000000018032D943 int3      
000000018032D944 int3      
000000018032D945 int3      
000000018032D946 int3      
000000018032D947 int3      
000000018032D948 int3      
000000018032D949 int3      
000000018032D94A int3      
000000018032D94B int3      
000000018032D94C int3      
000000018032D94D int3      
000000018032D94E int3      
000000018032D94F int3      