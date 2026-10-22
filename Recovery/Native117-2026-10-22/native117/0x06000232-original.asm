000000018033B560 mov       qword ptr [rsp + 8], rbx
000000018033B565 push      rsi
000000018033B566 push      rdi
000000018033B567 push      r14
000000018033B569 sub       rsp, 0x60
000000018033B56D cmp       byte ptr [rip + 0x19a96b6], 0 # 0x181ce4c2a
000000018033B574 jne       0x18033b64d
000000018033B57A lea       rcx, [rip + 0x18636c7] # 0x181b9ec48
000000018033B581 call      0x18024fef0
000000018033B586 lea       rcx, [rip + 0x1869963] # 0x181ba4ef0
000000018033B58D call      0x18024fef0
000000018033B592 lea       rcx, [rip + 0x1869cb7] # 0x181ba5250
000000018033B599 call      0x18024fef0
000000018033B59E lea       rcx, [rip + 0x1871153] # 0x181bac6f8
000000018033B5A5 call      0x18024fef0
000000018033B5AA lea       rcx, [rip + 0x187ec2f] # 0x181bba1e0
000000018033B5B1 call      0x18024fef0
000000018033B5B6 lea       rcx, [rip + 0x187ed2b] # 0x181bba2e8
000000018033B5BD call      0x18024fef0
000000018033B5C2 lea       rcx, [rip + 0x187edcf] # 0x181bba398
000000018033B5C9 call      0x18024fef0
000000018033B5CE lea       rcx, [rip + 0x1874f2b] # 0x181bb0500
000000018033B5D5 call      0x18024fef0
000000018033B5DA lea       rcx, [rip + 0x1876307] # 0x181bb18e8
000000018033B5E1 call      0x18024fef0
000000018033B5E6 lea       rcx, [rip + 0x187ae8b] # 0x181bb6478
000000018033B5ED call      0x18024fef0
000000018033B5F2 lea       rcx, [rip + 0x18846f7] # 0x181bbfcf0
000000018033B5F9 call      0x18024fef0
000000018033B5FE lea       rcx, [rip + 0x1868a2b] # 0x181ba4030
000000018033B605 call      0x18024fef0
000000018033B60A lea       rcx, [rip + 0x1889117] # 0x181bc4728
000000018033B611 call      0x18024fef0
000000018033B616 lea       rcx, [rip + 0x186f373] # 0x181baa990
000000018033B61D call      0x18024fef0
000000018033B622 lea       rcx, [rip + 0x188fb47] # 0x181bcb170
000000018033B629 call      0x18024fef0
000000018033B62E lea       rcx, [rip + 0x1891333] # 0x181bcc968
000000018033B635 call      0x18024fef0
000000018033B63A lea       rcx, [rip + 0x188fb4f] # 0x181bcb190
000000018033B641 call      0x18024fef0
000000018033B646 mov       byte ptr [rip + 0x19a95dd], 1 # 0x181ce4c2a
000000018033B64D xor       edi, edi
000000018033B64F mov       qword ptr [rsp + 0x98], rdi
000000018033B657 mov       rcx, qword ptr [rip + 0x187ae1a] # 0x181bb6478
000000018033B65E cmp       dword ptr [rcx + 0xe0], edi
000000018033B664 jne       0x18033b672
000000018033B666 call      0x1802501e0
000000018033B66B mov       rcx, qword ptr [rip + 0x187ae06] # 0x181bb6478
000000018033B672 mov       rax, qword ptr [rcx + 0xb8]
000000018033B679 mov       rdx, qword ptr [rax + 0xd8]
000000018033B680 test      rdx, rdx
000000018033B683 je        0x18033ba10
000000018033B689 cmp       dword ptr [rdx + 0x18], edi
000000018033B68C jg        0x18033b9f3
000000018033B692 mov       rcx, qword ptr [rip + 0x187624f] # 0x181bb18e8
000000018033B699 cmp       dword ptr [rcx + 0xe0], edi
000000018033B69F jne       0x18033b6a6
000000018033B6A1 call      0x1802501e0
000000018033B6A6 mov       qword ptr [rsp + 0x20], rdi
000000018033B6AB mov       r9, qword ptr [rip + 0x188fabe] # 0x181bcb170
000000018033B6B2 mov       r8, qword ptr [rip + 0x18912af] # 0x181bcc968
000000018033B6B9 mov       rdx, qword ptr [rip + 0x186f2d0] # 0x181baa990
000000018033B6C0 mov       rcx, qword ptr [rip + 0x188fac9] # 0x181bcb190
000000018033B6C7 call      0x180c84930
000000018033B6CC mov       r14, rax
000000018033B6CF mov       rbx, qword ptr [rip + 0x186895a] # 0x181ba4030
000000018033B6D6 mov       rcx, qword ptr [rip + 0x1884613] # 0x181bbfcf0
000000018033B6DD cmp       dword ptr [rcx + 0xe0], edi
000000018033B6E3 jne       0x18033b6ea
000000018033B6E5 call      0x1802501e0
000000018033B6EA xor       edx, edx
000000018033B6EC mov       rcx, rbx
000000018033B6EF call      0x180ccee30
000000018033B6F4 mov       rbx, rax
000000018033B6F7 mov       rcx, qword ptr [rip + 0x186354a] # 0x181b9ec48
000000018033B6FE cmp       dword ptr [rcx + 0xe0], edi
000000018033B704 jne       0x18033b70b
000000018033B706 call      0x1802501e0
000000018033B70B xor       edx, edx
000000018033B70D mov       rcx, rbx
000000018033B710 call      0x180cdd3b0
000000018033B715 test      rax, rax
000000018033B718 je        0x18033ba10
000000018033B71E xor       edx, edx
000000018033B720 mov       rcx, rax
000000018033B723 call      0x180cba170
000000018033B728 mov       qword ptr [rsp + 0x90], rax
000000018033B730 lea       rax, [rsp + 0x90]
000000018033B738 mov       qword ptr [rsp + 0x38], rax
000000018033B73D lea       rax, [rsp + 0x98]
000000018033B745 mov       qword ptr [rsp + 0x40], rax
000000018033B74A mov       qword ptr [rsp + 0x48], rdi
000000018033B74F movups    xmm0, xmmword ptr [rsp + 0x38]
000000018033B754 movups    xmmword ptr [rsp + 0x50], xmm0
000000018033B759 nop       dword ptr [rax]
000000018033B760 mov       r8, qword ptr [rsp + 0x90]
000000018033B768 test      r8, r8
000000018033B76B je        0x18033ba38
000000018033B771 xor       ecx, ecx
000000018033B773 mov       rdx, qword ptr [rip + 0x1869ad6] # 0x181ba5250
000000018033B77A call      0x180002cd0
000000018033B77F test      al, al
000000018033B781 je        0x18033b95f
000000018033B787 mov       r8, qword ptr [rsp + 0x90]
000000018033B78F test      r8, r8
000000018033B792 je        0x18033ba33
000000018033B798 mov       ecx, 1
000000018033B79D mov       rdx, qword ptr [rip + 0x1869aac] # 0x181ba5250
000000018033B7A4 call      0x180002cd0
000000018033B7A9 mov       rcx, rax
000000018033B7AC mov       r8, qword ptr [rip + 0x1888f75] # 0x181bc4728
000000018033B7B3 test      rax, rax
000000018033B7B6 je        0x18033ba2e
000000018033B7BC mov       rdx, qword ptr [rax]
000000018033B7BF mov       rax, qword ptr [r8 + 0x40]
000000018033B7C3 cmp       qword ptr [rdx + 0x40], rax
000000018033B7C7 jne       0x18033ba26
000000018033B7CD call      0x18024f540
000000018033B7D2 mov       edi, dword ptr [rax]
000000018033B7D4 mov       rbx, qword ptr [rip + 0x1868855] # 0x181ba4030
000000018033B7DB mov       rcx, qword ptr [rip + 0x188450e] # 0x181bbfcf0
000000018033B7E2 cmp       dword ptr [rcx + 0xe0], 0
000000018033B7E9 jne       0x18033b7f0
000000018033B7EB call      0x1802501e0
000000018033B7F0 xor       edx, edx
000000018033B7F2 mov       rcx, rbx
000000018033B7F5 call      0x180ccee30
000000018033B7FA mov       rsi, rax
000000018033B7FD mov       dword ptr [rsp + 0x88], edi
000000018033B804 lea       rdx, [rsp + 0x88]
000000018033B80C mov       rcx, qword ptr [rip + 0x1888f15] # 0x181bc4728
000000018033B813 call      0x18024f340
000000018033B818 mov       rbx, rax
000000018033B81B mov       rcx, qword ptr [rip + 0x1863426] # 0x181b9ec48
000000018033B822 cmp       dword ptr [rcx + 0xe0], 0
000000018033B829 jne       0x18033b830
000000018033B82B call      0x1802501e0
000000018033B830 xor       r8d, r8d
000000018033B833 mov       rdx, rbx
000000018033B836 mov       rcx, rsi
000000018033B839 call      0x180cdcd20
000000018033B83E mov       rbx, rax
000000018033B841 mov       rcx, qword ptr [rip + 0x18760a0] # 0x181bb18e8
000000018033B848 cmp       dword ptr [rcx + 0xe0], 0
000000018033B84F jne       0x18033b856
000000018033B851 call      0x1802501e0
000000018033B856 xor       r8d, r8d
000000018033B859 mov       rdx, rbx
000000018033B85C mov       rcx, r14
000000018033B85F call      0x180c84600
000000018033B864 mov       rdx, qword ptr [rip + 0x1870e8d] # 0x181bac6f8
000000018033B86B mov       rcx, rax
000000018033B86E call      0x18047ba00
000000018033B873 mov       rbx, rax
000000018033B876 mov       rcx, qword ptr [rip + 0x1874c83] # 0x181bb0500
000000018033B87D cmp       dword ptr [rcx + 0xe0], 0
000000018033B884 jne       0x18033b88b
000000018033B886 call      0x1802501e0
000000018033B88B xor       edx, edx
000000018033B88D mov       rcx, rbx
000000018033B890 call      0x18131f870
000000018033B895 test      al, al
000000018033B897 je        0x18033b760
000000018033B89D nop       dword ptr [rax]
000000018033B8A0 mov       rax, qword ptr [rip + 0x187abd1] # 0x181bb6478
000000018033B8A7 cmp       dword ptr [rax + 0xe0], 0
000000018033B8AE jne       0x18033b8bf
000000018033B8B0 mov       rcx, rax
000000018033B8B3 call      0x1802501e0
000000018033B8B8 mov       rax, qword ptr [rip + 0x187abb9] # 0x181bb6478
000000018033B8BF mov       rcx, qword ptr [rax + 0xb8]
000000018033B8C6 mov       rdx, qword ptr [rcx + 0xd8]
000000018033B8CD test      rdx, rdx
000000018033B8D0 je        0x18033ba21
000000018033B8D6 cmp       dword ptr [rdx + 0x18], edi
000000018033B8D9 jg        0x18033b91a
000000018033B8DB cmp       dword ptr [rax + 0xe0], 0
000000018033B8E2 jne       0x18033b8f3
000000018033B8E4 mov       rcx, rax
000000018033B8E7 call      0x1802501e0
000000018033B8EC mov       rax, qword ptr [rip + 0x187ab85] # 0x181bb6478
000000018033B8F3 mov       rcx, qword ptr [rax + 0xb8]
000000018033B8FA mov       rcx, qword ptr [rcx + 0xd8]
000000018033B901 test      rcx, rcx
000000018033B904 je        0x18033ba16
000000018033B90A mov       r8, qword ptr [rip + 0x187e8cf] # 0x181bba1e0
000000018033B911 xor       edx, edx
000000018033B913 call      0x180002e70
000000018033B918 jmp       0x18033b8a0
000000018033B91A cmp       dword ptr [rax + 0xe0], 0
000000018033B921 jne       0x18033b932
000000018033B923 mov       rcx, rax
000000018033B926 call      0x1802501e0
000000018033B92B mov       rax, qword ptr [rip + 0x187ab46] # 0x181bb6478
000000018033B932 mov       rcx, qword ptr [rax + 0xb8]
000000018033B939 mov       rcx, qword ptr [rcx + 0xd8]
000000018033B940 test      rcx, rcx
000000018033B943 je        0x18033ba1c
000000018033B949 mov       r9, qword ptr [rip + 0x187ea48] # 0x181bba398
000000018033B950 mov       r8, rbx
000000018033B953 mov       edx, edi
000000018033B955 call      0x180855ab0
000000018033B95A jmp       0x18033b760
000000018033B95F mov       rax, qword ptr [rsp + 0x38]
000000018033B964 mov       rdx, qword ptr [rip + 0x1869585] # 0x181ba4ef0
000000018033B96B mov       rcx, qword ptr [rax]
000000018033B96E call      0x18024f380
000000018033B973 mov       r8, rax
000000018033B976 movups    xmm0, xmmword ptr [rsp + 0x38]
000000018033B97B psrldq    xmm0, 8
000000018033B980 movq      rax, xmm0
000000018033B985 mov       qword ptr [rax], r8
000000018033B988 test      r8, r8
000000018033B98B je        0x18033b99b
000000018033B98D xor       ecx, ecx
000000018033B98F mov       rdx, qword ptr [rip + 0x186955a] # 0x181ba4ef0
000000018033B996 call      0x180002d90
000000018033B99B jmp       0x18033b9b5
000000018033B99D lea       rcx, [rsp + 0x50]
000000018033B9A2 call      0x1802fb000
000000018033B9A7 mov       rcx, qword ptr [rsp + 0x48]
000000018033B9AC test      rcx, rcx
000000018033B9AF jne       0x18033ba3e
000000018033B9B5 mov       rax, qword ptr [rip + 0x187aabc] # 0x181bb6478
000000018033B9BC cmp       dword ptr [rax + 0xe0], 0
000000018033B9C3 jne       0x18033b9d4
000000018033B9C5 mov       rcx, rax
000000018033B9C8 call      0x1802501e0
000000018033B9CD mov       rax, qword ptr [rip + 0x187aaa4] # 0x181bb6478
000000018033B9D4 mov       rax, qword ptr [rax + 0xb8]
000000018033B9DB mov       rax, qword ptr [rax + 0xd8]
000000018033B9E2 mov       rbx, qword ptr [rsp + 0x80]
000000018033B9EA add       rsp, 0x60
000000018033B9EE pop       r14
000000018033B9F0 pop       rdi
000000018033B9F1 pop       rsi
000000018033B9F2 ret       
000000018033B9F3 cmp       dword ptr [rcx + 0xe0], edi
000000018033B9F9 jne       0x18033ba07
000000018033B9FB call      0x1802501e0
000000018033BA00 mov       rcx, qword ptr [rip + 0x187aa71] # 0x181bb6478
000000018033BA07 mov       rax, qword ptr [rcx + 0xb8]
000000018033BA0E jmp       0x18033b9db
000000018033BA10 call      0x180250150
000000018033BA15 nop       
000000018033BA16 call      0x180250150
000000018033BA1B nop       
000000018033BA1C call      0x180250150
000000018033BA21 call      0x180250150
000000018033BA26 mov       rdx, r8
000000018033BA29 call      0x18024f3a0
000000018033BA2E call      0x180250150
000000018033BA33 call      0x180250150
000000018033BA38 call      0x180250150
000000018033BA3D nop       
000000018033BA3E call      0x180200e60
000000018033BA43 int3      
000000018033BA44 int3      
000000018033BA45 int3      
000000018033BA46 int3      
000000018033BA47 int3      
000000018033BA48 int3      
000000018033BA49 int3      
000000018033BA4A int3      
000000018033BA4B int3      
000000018033BA4C int3      
000000018033BA4D int3      
000000018033BA4E int3      
000000018033BA4F int3      