000000018030A600 mov       dword ptr [rsp + 0x10], edx
000000018030A604 push      rbx
000000018030A605 sub       rsp, 0x20
000000018030A609 cmp       byte ptr [rip + 0x19da4d8], 0 # 0x181ce4ae8
000000018030A610 mov       rbx, rcx
000000018030A613 jne       0x18030a650
000000018030A615 lea       rcx, [rip + 0x18b93e4] # 0x181bc3a00
000000018030A61C call      0x18024fef0
000000018030A621 lea       rcx, [rip + 0x18b9430] # 0x181bc3a58
000000018030A628 call      0x18024fef0
000000018030A62D lea       rcx, [rip + 0x18b06c4] # 0x181bbacf8
000000018030A634 call      0x18024fef0
000000018030A639 lea       rcx, [rip + 0x18967a8] # 0x181ba0de8
000000018030A640 call      0x18024fef0
000000018030A645 mov       edx, dword ptr [rsp + 0x38]
000000018030A649 mov       byte ptr [rip + 0x19da498], 1 # 0x181ce4ae8
000000018030A650 mov       qword ptr [rsp + 0x30], rdi
000000018030A655 test      edx, edx
000000018030A657 js        0x18030aa18
000000018030A65D mov       rax, qword ptr [rbx + 0x20]
000000018030A661 test      rax, rax
000000018030A664 je        0x18030ace1
000000018030A66A cmp       edx, dword ptr [rax + 0x18]
000000018030A66D jge       0x18030a9ff
000000018030A673 mov       rdi, qword ptr [rbx + 0x48]
000000018030A677 lea       rcx, [rsp + 0x38]
000000018030A67C mov       dword ptr [rbx + 0x38], edx
000000018030A67F xor       edx, edx
000000018030A681 call      0x180ca3820
000000018030A686 test      rdi, rdi
000000018030A689 je        0x18030ace1
000000018030A68F xor       r8d, r8d
000000018030A692 mov       rdx, rax
000000018030A695 mov       rcx, rdi
000000018030A698 call      0x181197d70
000000018030A69D mov       rcx, qword ptr [rbx + 0x20]
000000018030A6A1 test      rcx, rcx
000000018030A6A4 je        0x18030ace1
000000018030A6AA mov       r8, qword ptr [rip + 0x18b93a7] # 0x181bc3a58
000000018030A6B1 mov       edx, dword ptr [rsp + 0x38]
000000018030A6B5 call      0x180825710
000000018030A6BA lea       rcx, [rbx + 0x28]
000000018030A6BE mov       qword ptr [rbx + 0x28], rax
000000018030A6C2 mov       rdx, rax
000000018030A6C5 call      0x18024f360
000000018030A6CA mov       rdx, qword ptr [rbx + 0x28]
000000018030A6CE test      rdx, rdx
000000018030A6D1 je        0x18030ace1
000000018030A6D7 mov       rdx, qword ptr [rdx + 0x18]
000000018030A6DB lea       rcx, [rbx + 0x30]
000000018030A6DF mov       qword ptr [rbx + 0x30], rdx
000000018030A6E3 call      0x18024f360
000000018030A6E8 mov       rcx, qword ptr [rbx + 0x50]
000000018030A6EC test      rcx, rcx
000000018030A6EF je        0x18030ace1
000000018030A6F5 mov       rdx, qword ptr [rbx + 0x30]
000000018030A6F9 xor       r8d, r8d
000000018030A6FC call      0x181197d70
000000018030A701 mov       rcx, qword ptr [rbx + 0x28]
000000018030A705 mov       rdi, qword ptr [rbx + 0x58]
000000018030A709 test      rcx, rcx
000000018030A70C je        0x18030ace1
000000018030A712 add       rcx, 0x40
000000018030A716 xor       edx, edx
000000018030A718 call      0x180ca3820
000000018030A71D test      rdi, rdi
000000018030A720 je        0x18030ace1
000000018030A726 xor       r8d, r8d
000000018030A729 mov       rdx, rax
000000018030A72C mov       rcx, rdi
000000018030A72F call      0x181197d70
000000018030A734 mov       rcx, qword ptr [rbx + 0x28]
000000018030A738 mov       rdi, qword ptr [rbx + 0x60]
000000018030A73C test      rcx, rcx
000000018030A73F je        0x18030ace1
000000018030A745 add       rcx, 0x44
000000018030A749 xor       edx, edx
000000018030A74B call      0x180cc5b50
000000018030A750 test      rdi, rdi
000000018030A753 je        0x18030ace1
000000018030A759 xor       r8d, r8d
000000018030A75C mov       rdx, rax
000000018030A75F mov       rcx, rdi
000000018030A762 call      0x181197d70
000000018030A767 mov       rcx, qword ptr [rbx + 0x28]
000000018030A76B mov       rdi, qword ptr [rbx + 0x68]
000000018030A76F test      rcx, rcx
000000018030A772 je        0x18030ace1
000000018030A778 add       rcx, 0x4c
000000018030A77C xor       edx, edx
000000018030A77E call      0x180ca3820
000000018030A783 test      rdi, rdi
000000018030A786 je        0x18030ace1
000000018030A78C xor       r8d, r8d
000000018030A78F mov       rdx, rax
000000018030A792 mov       rcx, rdi
000000018030A795 call      0x181197d70
000000018030A79A mov       rcx, qword ptr [rbx + 0x28]
000000018030A79E mov       rdi, qword ptr [rbx + 0x70]
000000018030A7A2 test      rcx, rcx
000000018030A7A5 je        0x18030ace1
000000018030A7AB add       rcx, 0x30
000000018030A7AF xor       edx, edx
000000018030A7B1 call      0x180cc5b50
000000018030A7B6 test      rdi, rdi
000000018030A7B9 je        0x18030ace1
000000018030A7BF xor       r8d, r8d
000000018030A7C2 mov       rdx, rax
000000018030A7C5 mov       rcx, rdi
000000018030A7C8 call      0x181197d70
000000018030A7CD mov       rcx, qword ptr [rbx + 0x28]
000000018030A7D1 mov       rdi, qword ptr [rbx + 0x78]
000000018030A7D5 test      rcx, rcx
000000018030A7D8 je        0x18030ace1
000000018030A7DE add       rcx, 0x24
000000018030A7E2 xor       edx, edx
000000018030A7E4 call      0x180cc5b50
000000018030A7E9 test      rdi, rdi
000000018030A7EC je        0x18030ace1
000000018030A7F2 xor       r8d, r8d
000000018030A7F5 mov       rdx, rax
000000018030A7F8 mov       rcx, rdi
000000018030A7FB call      0x181197d70
000000018030A800 mov       rcx, qword ptr [rbx + 0x28]
000000018030A804 mov       rdi, qword ptr [rbx + 0x80]
000000018030A80B test      rcx, rcx
000000018030A80E je        0x18030ace1
000000018030A814 add       rcx, 0x34
000000018030A818 xor       edx, edx
000000018030A81A call      0x180cc5b50
000000018030A81F test      rdi, rdi
000000018030A822 je        0x18030ace1
000000018030A828 xor       r8d, r8d
000000018030A82B mov       rdx, rax
000000018030A82E mov       rcx, rdi
000000018030A831 call      0x181197d70
000000018030A836 mov       rcx, qword ptr [rbx + 0x28]
000000018030A83A mov       rdi, qword ptr [rbx + 0x88]
000000018030A841 test      rcx, rcx
000000018030A844 je        0x18030ace1
000000018030A84A add       rcx, 0x38
000000018030A84E xor       edx, edx
000000018030A850 call      0x180cc5b50
000000018030A855 test      rdi, rdi
000000018030A858 je        0x18030ace1
000000018030A85E xor       r8d, r8d
000000018030A861 mov       rdx, rax
000000018030A864 mov       rcx, rdi
000000018030A867 call      0x181197d70
000000018030A86C mov       rdx, qword ptr [rbx + 0x28]
000000018030A870 test      rdx, rdx
000000018030A873 je        0x18030ace1
000000018030A879 mov       rcx, qword ptr [rbx + 0xa0]
000000018030A880 test      rcx, rcx
000000018030A883 je        0x18030ace1
000000018030A889 mov       edx, dword ptr [rdx + 0x68]
000000018030A88C xor       r8d, r8d
000000018030A88F inc       edx
000000018030A891 call      0x181180950
000000018030A896 mov       rcx, qword ptr [rbx + 0x50]
000000018030A89A test      rcx, rcx
000000018030A89D je        0x18030ace1
000000018030A8A3 xor       r8d, r8d
000000018030A8A6 mov       dl, 1
000000018030A8A8 call      0x18156cc70
000000018030A8AD mov       rcx, qword ptr [rbx + 0x58]
000000018030A8B1 test      rcx, rcx
000000018030A8B4 je        0x18030ace1
000000018030A8BA xor       r8d, r8d
000000018030A8BD mov       dl, 1
000000018030A8BF call      0x18156cc70
000000018030A8C4 mov       rcx, qword ptr [rbx + 0x60]
000000018030A8C8 test      rcx, rcx
000000018030A8CB je        0x18030ace1
000000018030A8D1 xor       r8d, r8d
000000018030A8D4 mov       dl, 1
000000018030A8D6 call      0x18156cc70
000000018030A8DB mov       rcx, qword ptr [rbx + 0x68]
000000018030A8DF test      rcx, rcx
000000018030A8E2 je        0x18030ace1
000000018030A8E8 xor       r8d, r8d
000000018030A8EB mov       dl, 1
000000018030A8ED call      0x18156cc70
000000018030A8F2 mov       rcx, qword ptr [rbx + 0x70]
000000018030A8F6 test      rcx, rcx
000000018030A8F9 je        0x18030ace1
000000018030A8FF xor       r8d, r8d
000000018030A902 mov       dl, 1
000000018030A904 call      0x18156cc70
000000018030A909 mov       rcx, qword ptr [rbx + 0x78]
000000018030A90D test      rcx, rcx
000000018030A910 je        0x18030ace1
000000018030A916 xor       r8d, r8d
000000018030A919 mov       dl, 1
000000018030A91B call      0x18156cc70
000000018030A920 mov       rcx, qword ptr [rbx + 0x80]
000000018030A927 test      rcx, rcx
000000018030A92A je        0x18030ace1
000000018030A930 xor       r8d, r8d
000000018030A933 mov       dl, 1
000000018030A935 call      0x18156cc70
000000018030A93A mov       rcx, qword ptr [rbx + 0x88]
000000018030A941 test      rcx, rcx
000000018030A944 je        0x18030ace1
000000018030A94A xor       r8d, r8d
000000018030A94D mov       dl, 1
000000018030A94F call      0x18156cc70
000000018030A954 mov       rdx, qword ptr [rbx + 0x28]
000000018030A958 test      rdx, rdx
000000018030A95B je        0x18030ace1
000000018030A961 mov       rcx, qword ptr [rbx + 0x90]
000000018030A968 test      rcx, rcx
000000018030A96B je        0x18030ace1
000000018030A971 movzx     edx, byte ptr [rdx + 0x20]
000000018030A975 xor       r8d, r8d
000000018030A978 call      0x181573290
000000018030A97D mov       rdx, qword ptr [rbx + 0x28]
000000018030A981 test      rdx, rdx
000000018030A984 je        0x18030ace1
000000018030A98A mov       rcx, qword ptr [rbx + 0x98]
000000018030A991 test      rcx, rcx
000000018030A994 je        0x18030ace1
000000018030A99A movzx     edx, byte ptr [rdx + 0x21]
000000018030A99E xor       r8d, r8d
000000018030A9A1 call      0x181573290
000000018030A9A6 mov       rcx, qword ptr [rbx + 0x90]
000000018030A9AD test      rcx, rcx
000000018030A9B0 je        0x18030ace1
000000018030A9B6 xor       r8d, r8d
000000018030A9B9 mov       dl, 1
000000018030A9BB call      0x18156cc70
000000018030A9C0 mov       rcx, qword ptr [rbx + 0x98]
000000018030A9C7 test      rcx, rcx
000000018030A9CA je        0x18030ace1
000000018030A9D0 xor       r8d, r8d
000000018030A9D3 mov       dl, 1
000000018030A9D5 call      0x18156cc70
000000018030A9DA mov       rcx, qword ptr [rbx + 0xa0]
000000018030A9E1 test      rcx, rcx
000000018030A9E4 je        0x18030ace1
000000018030A9EA xor       r8d, r8d
000000018030A9ED mov       dl, 1
000000018030A9EF call      0x18156cc70
000000018030A9F4 mov       rdi, qword ptr [rsp + 0x30]
000000018030A9F9 add       rsp, 0x20
000000018030A9FD pop       rbx
000000018030A9FE ret       
000000018030A9FF mov       edx, dword ptr [rbx + 0x38]
000000018030AA02 xor       r8d, r8d
000000018030AA05 mov       rcx, rbx
000000018030AA08 call      0x18030a600
000000018030AA0D mov       rdi, qword ptr [rsp + 0x30]
000000018030AA12 add       rsp, 0x20
000000018030AA16 pop       rbx
000000018030AA17 ret       
000000018030AA18 mov       rcx, qword ptr [rbx + 0x48]
000000018030AA1C mov       dword ptr [rsp + 0x38], 0xffffffff
000000018030AA24 mov       dword ptr [rbx + 0x38], 0xffffffff
000000018030AA2B test      rcx, rcx
000000018030AA2E je        0x18030ace1
000000018030AA34 mov       rdx, qword ptr [rip + 0x18963ad] # 0x181ba0de8
000000018030AA3B xor       r8d, r8d
000000018030AA3E call      0x181197d70
000000018030AA43 lea       rcx, [rbx + 0x28]
000000018030AA47 mov       qword ptr [rbx + 0x28], 0
000000018030AA4F xor       edx, edx
000000018030AA51 call      0x18024f360
000000018030AA56 mov       rax, qword ptr [rip + 0x18b029b] # 0x181bbacf8
000000018030AA5D mov       rcx, qword ptr [rax + 0xb8]
000000018030AA64 mov       rdx, qword ptr [rcx]
000000018030AA67 lea       rcx, [rbx + 0x30]
000000018030AA6B mov       qword ptr [rbx + 0x30], rdx
000000018030AA6F call      0x18024f360
000000018030AA74 mov       rcx, qword ptr [rbx + 0x50]
000000018030AA78 test      rcx, rcx
000000018030AA7B je        0x18030ace1
000000018030AA81 mov       rax, qword ptr [rip + 0x18b0270] # 0x181bbacf8
000000018030AA88 xor       r8d, r8d
000000018030AA8B mov       rdx, qword ptr [rax + 0xb8]
000000018030AA92 mov       rdx, qword ptr [rdx]
000000018030AA95 call      0x181197d70
000000018030AA9A mov       rcx, qword ptr [rbx + 0x58]
000000018030AA9E test      rcx, rcx
000000018030AAA1 je        0x18030ace1
000000018030AAA7 mov       rax, qword ptr [rip + 0x18b024a] # 0x181bbacf8
000000018030AAAE xor       r8d, r8d
000000018030AAB1 mov       rdx, qword ptr [rax + 0xb8]
000000018030AAB8 mov       rdx, qword ptr [rdx]
000000018030AABB call      0x181197d70
000000018030AAC0 mov       rcx, qword ptr [rbx + 0x60]
000000018030AAC4 test      rcx, rcx
000000018030AAC7 je        0x18030ace1
000000018030AACD mov       rax, qword ptr [rip + 0x18b0224] # 0x181bbacf8
000000018030AAD4 xor       r8d, r8d
000000018030AAD7 mov       rdx, qword ptr [rax + 0xb8]
000000018030AADE mov       rdx, qword ptr [rdx]
000000018030AAE1 call      0x181197d70
000000018030AAE6 mov       rcx, qword ptr [rbx + 0x68]
000000018030AAEA test      rcx, rcx
000000018030AAED je        0x18030ace1
000000018030AAF3 mov       rax, qword ptr [rip + 0x18b01fe] # 0x181bbacf8
000000018030AAFA xor       r8d, r8d
000000018030AAFD mov       rdx, qword ptr [rax + 0xb8]
000000018030AB04 mov       rdx, qword ptr [rdx]
000000018030AB07 call      0x181197d70
000000018030AB0C mov       rcx, qword ptr [rbx + 0x70]
000000018030AB10 test      rcx, rcx
000000018030AB13 je        0x18030ace1
000000018030AB19 mov       rax, qword ptr [rip + 0x18b01d8] # 0x181bbacf8
000000018030AB20 xor       r8d, r8d
000000018030AB23 mov       rdx, qword ptr [rax + 0xb8]
000000018030AB2A mov       rdx, qword ptr [rdx]
000000018030AB2D call      0x181197d70
000000018030AB32 mov       rcx, qword ptr [rbx + 0x78]
000000018030AB36 test      rcx, rcx
000000018030AB39 je        0x18030ace1
000000018030AB3F mov       rax, qword ptr [rip + 0x18b01b2] # 0x181bbacf8
000000018030AB46 xor       r8d, r8d
000000018030AB49 mov       rdx, qword ptr [rax + 0xb8]
000000018030AB50 mov       rdx, qword ptr [rdx]
000000018030AB53 call      0x181197d70
000000018030AB58 mov       rcx, qword ptr [rbx + 0x80]
000000018030AB5F test      rcx, rcx
000000018030AB62 je        0x18030ace1
000000018030AB68 mov       rax, qword ptr [rip + 0x18b0189] # 0x181bbacf8
000000018030AB6F xor       r8d, r8d
000000018030AB72 mov       rdx, qword ptr [rax + 0xb8]
000000018030AB79 mov       rdx, qword ptr [rdx]
000000018030AB7C call      0x181197d70
000000018030AB81 mov       rcx, qword ptr [rbx + 0x88]
000000018030AB88 test      rcx, rcx
000000018030AB8B je        0x18030ace1
000000018030AB91 mov       rax, qword ptr [rip + 0x18b0160] # 0x181bbacf8
000000018030AB98 xor       r8d, r8d
000000018030AB9B mov       rdx, qword ptr [rax + 0xb8]
000000018030ABA2 mov       rdx, qword ptr [rdx]
000000018030ABA5 call      0x181197d70
000000018030ABAA mov       rcx, qword ptr [rbx + 0x50]
000000018030ABAE test      rcx, rcx
000000018030ABB1 je        0x18030ace1
000000018030ABB7 xor       r8d, r8d
000000018030ABBA xor       edx, edx
000000018030ABBC call      0x18156cc70
000000018030ABC1 mov       rcx, qword ptr [rbx + 0x58]
000000018030ABC5 test      rcx, rcx
000000018030ABC8 je        0x18030ace1
000000018030ABCE xor       r8d, r8d
000000018030ABD1 xor       edx, edx
000000018030ABD3 call      0x18156cc70
000000018030ABD8 mov       rcx, qword ptr [rbx + 0x60]
000000018030ABDC test      rcx, rcx
000000018030ABDF je        0x18030ace1
000000018030ABE5 xor       r8d, r8d
000000018030ABE8 xor       edx, edx
000000018030ABEA call      0x18156cc70
000000018030ABEF mov       rcx, qword ptr [rbx + 0x68]
000000018030ABF3 test      rcx, rcx
000000018030ABF6 je        0x18030ace1
000000018030ABFC xor       r8d, r8d
000000018030ABFF xor       edx, edx
000000018030AC01 call      0x18156cc70
000000018030AC06 mov       rcx, qword ptr [rbx + 0x70]
000000018030AC0A test      rcx, rcx
000000018030AC0D je        0x18030ace1
000000018030AC13 xor       r8d, r8d
000000018030AC16 xor       edx, edx
000000018030AC18 call      0x18156cc70
000000018030AC1D mov       rcx, qword ptr [rbx + 0x78]
000000018030AC21 test      rcx, rcx
000000018030AC24 je        0x18030ace1
000000018030AC2A xor       r8d, r8d
000000018030AC2D xor       edx, edx
000000018030AC2F call      0x18156cc70
000000018030AC34 mov       rcx, qword ptr [rbx + 0x80]
000000018030AC3B test      rcx, rcx
000000018030AC3E je        0x18030ace1
000000018030AC44 xor       r8d, r8d
000000018030AC47 xor       edx, edx
000000018030AC49 call      0x18156cc70
000000018030AC4E mov       rcx, qword ptr [rbx + 0x88]
000000018030AC55 test      rcx, rcx
000000018030AC58 je        0x18030ace1
000000018030AC5E xor       r8d, r8d
000000018030AC61 xor       edx, edx
000000018030AC63 call      0x18156cc70
000000018030AC68 mov       rcx, qword ptr [rbx + 0x90]
000000018030AC6F test      rcx, rcx
000000018030AC72 je        0x18030ace1
000000018030AC74 xor       r8d, r8d
000000018030AC77 xor       edx, edx
000000018030AC79 call      0x181573290
000000018030AC7E mov       rcx, qword ptr [rbx + 0x98]
000000018030AC85 test      rcx, rcx
000000018030AC88 je        0x18030ace1
000000018030AC8A xor       r8d, r8d
000000018030AC8D xor       edx, edx
000000018030AC8F call      0x181573290
000000018030AC94 mov       rcx, qword ptr [rbx + 0x90]
000000018030AC9B test      rcx, rcx
000000018030AC9E je        0x18030ace1
000000018030ACA0 xor       r8d, r8d
000000018030ACA3 xor       edx, edx
000000018030ACA5 call      0x18156cc70
000000018030ACAA mov       rcx, qword ptr [rbx + 0x98]
000000018030ACB1 test      rcx, rcx
000000018030ACB4 je        0x18030ace1
000000018030ACB6 xor       r8d, r8d
000000018030ACB9 xor       edx, edx
000000018030ACBB call      0x18156cc70
000000018030ACC0 mov       rcx, qword ptr [rbx + 0xa0]
000000018030ACC7 test      rcx, rcx
000000018030ACCA je        0x18030ace1
000000018030ACCC xor       r8d, r8d
000000018030ACCF xor       edx, edx
000000018030ACD1 call      0x18156cc70
000000018030ACD6 mov       rdi, qword ptr [rsp + 0x30]
000000018030ACDB add       rsp, 0x20
000000018030ACDF pop       rbx
000000018030ACE0 ret       
000000018030ACE1 call      0x180250150
000000018030ACE6 int3      
000000018030ACE7 int3      
000000018030ACE8 int3      
000000018030ACE9 int3      
000000018030ACEA int3      
000000018030ACEB int3      
000000018030ACEC int3      
000000018030ACED int3      
000000018030ACEE int3      
000000018030ACEF int3      