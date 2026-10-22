000000018030B890 push      rbx
000000018030B892 sub       rsp, 0x30
000000018030B896 cmp       byte ptr [rip + 0x19d9253], 0 # 0x181ce4af0
000000018030B89D mov       rbx, rcx
000000018030B8A0 jne       0x18030b931
000000018030B8A6 lea       rcx, [rip + 0x189942b] # 0x181ba4cd8
000000018030B8AD call      0x18024fef0
000000018030B8B2 lea       rcx, [rip + 0x18a15e7] # 0x181bacea0
000000018030B8B9 call      0x18024fef0
000000018030B8BE lea       rcx, [rip + 0x18a1633] # 0x181bacef8
000000018030B8C5 call      0x18024fef0
000000018030B8CA lea       rcx, [rip + 0x18a167f] # 0x181bacf50
000000018030B8D1 call      0x18024fef0
000000018030B8D6 lea       rcx, [rip + 0x18ac8bb] # 0x181bb8198
000000018030B8DD call      0x18024fef0
000000018030B8E2 lea       rcx, [rip + 0x18a16bf] # 0x181bacfa8
000000018030B8E9 call      0x18024fef0
000000018030B8EE lea       rcx, [rip + 0x18ac8fb] # 0x181bb81f0
000000018030B8F5 call      0x18024fef0
000000018030B8FA lea       rcx, [rip + 0x18a1497] # 0x181bacd98
000000018030B901 call      0x18024fef0
000000018030B906 lea       rcx, [rip + 0x18ac2f3] # 0x181bb7c00
000000018030B90D call      0x18024fef0
000000018030B912 lea       rcx, [rip + 0x18a4be7] # 0x181bb0500
000000018030B919 call      0x18024fef0
000000018030B91E lea       rcx, [rip + 0x18aab53] # 0x181bb6478
000000018030B925 call      0x18024fef0
000000018030B92A mov       byte ptr [rip + 0x19d91bf], 1 # 0x181ce4af0
000000018030B931 mov       rax, qword ptr [rbx + 0x20]
000000018030B935 mov       qword ptr [rsp + 0x40], rbp
000000018030B93A mov       qword ptr [rsp + 0x48], rsi
000000018030B93F mov       qword ptr [rsp + 0x50], rdi
000000018030B944 mov       qword ptr [rsp + 0x58], r13
000000018030B949 mov       qword ptr [rsp + 0x28], r14
000000018030B94E mov       qword ptr [rsp + 0x20], r15
000000018030B953 test      rax, rax
000000018030B956 je        0x18030bca0
000000018030B95C mov       r13, qword ptr [rax + 0x30]
000000018030B960 test      r13, r13
000000018030B963 je        0x18030bca0
000000018030B969 xor       edx, edx
000000018030B96B mov       rcx, r13
000000018030B96E call      0x18032c3b0
000000018030B973 mov       r15d, eax
000000018030B976 mov       rax, qword ptr [r13 + 0x10]
000000018030B97A test      rax, rax
000000018030B97D je        0x18030bca0
000000018030B983 mov       rcx, qword ptr [rbx + 0x48]
000000018030B987 mov       r14d, dword ptr [rax + 0x18]
000000018030B98B test      rcx, rcx
000000018030B98E je        0x18030bca0
000000018030B994 mov       eax, r14d
000000018030B997 imul      eax, r15d
000000018030B99B cmp       dword ptr [rcx + 0x18], eax
000000018030B99E jle       0x18030ba4a
000000018030B9A4 mov       edi, dword ptr [rcx + 0x18]
000000018030B9A7 mov       ebp, r14d
000000018030B9AA imul      ebp, r15d
000000018030B9AE dec       edi
000000018030B9B0 cmp       edi, ebp
000000018030B9B2 jl        0x18030ba4a
000000018030B9B8 nop       dword ptr [rax + rax]
000000018030B9C0 mov       rcx, qword ptr [rbx + 0x48]
000000018030B9C4 test      rcx, rcx
000000018030B9C7 je        0x18030bca0
000000018030B9CD mov       r8, qword ptr [rip + 0x18a15d4] # 0x181bacfa8
000000018030B9D4 mov       edx, edi
000000018030B9D6 call      0x180825710
000000018030B9DB test      rax, rax
000000018030B9DE je        0x18030bca0
000000018030B9E4 xor       edx, edx
000000018030B9E6 mov       rcx, rax
000000018030B9E9 call      0x1813044d0
000000018030B9EE mov       rcx, qword ptr [rip + 0x18a4b0b] # 0x181bb0500
000000018030B9F5 mov       rsi, rax
000000018030B9F8 cmp       dword ptr [rcx + 0xe0], 0
000000018030B9FF jne       0x18030ba06
000000018030BA01 call      0x1802501e0
000000018030BA06 xor       edx, edx
000000018030BA08 mov       rcx, rsi
000000018030BA0B call      0x18131e940
000000018030BA10 mov       rsi, qword ptr [rbx + 0x48]
000000018030BA14 mov       rcx, rsi
000000018030BA17 test      rsi, rsi
000000018030BA1A je        0x18030bca0
000000018030BA20 mov       r8, qword ptr [rip + 0x18a1581] # 0x181bacfa8
000000018030BA27 mov       edx, edi
000000018030BA29 call      0x180825710
000000018030BA2E mov       r8, qword ptr [rip + 0x18a14c3] # 0x181bacef8
000000018030BA35 mov       rdx, rax
000000018030BA38 mov       rcx, rsi
000000018030BA3B call      0x18084e290
000000018030BA40 dec       edi
000000018030BA42 cmp       edi, ebp
000000018030BA44 jge       0x18030b9c0
000000018030BA4A mov       rax, qword ptr [rbx + 0x48]
000000018030BA4E test      rax, rax
000000018030BA51 je        0x18030bca0
000000018030BA57 imul      r14d, r15d
000000018030BA5B nop       dword ptr [rax + rax]
000000018030BA60 cmp       dword ptr [rax + 0x18], r14d
000000018030BA64 jge       0x18030bb93
000000018030BA6A mov       rax, qword ptr [rip + 0x18aaa07] # 0x181bb6478
000000018030BA71 cmp       dword ptr [rax + 0xe0], 0
000000018030BA78 jne       0x18030ba89
000000018030BA7A mov       rcx, rax
000000018030BA7D call      0x1802501e0
000000018030BA82 mov       rax, qword ptr [rip + 0x18aa9ef] # 0x181bb6478
000000018030BA89 mov       rcx, qword ptr [rip + 0x18a4a70] # 0x181bb0500
000000018030BA90 mov       rax, qword ptr [rax + 0xb8]
000000018030BA97 cmp       dword ptr [rcx + 0xe0], 0
000000018030BA9E mov       rdi, qword ptr [rax + 0x1f8]
000000018030BAA5 jne       0x18030baac
000000018030BAA7 call      0x1802501e0
000000018030BAAC mov       rdx, qword ptr [rip + 0x18ac14d] # 0x181bb7c00
000000018030BAB3 mov       rcx, rdi
000000018030BAB6 call      0x1804a0690
000000018030BABB mov       rdi, rax
000000018030BABE test      rax, rax
000000018030BAC1 je        0x18030bca0
000000018030BAC7 lea       rcx, [rax + 0x38]
000000018030BACB mov       qword ptr [rax + 0x38], rbx
000000018030BACF mov       rdx, rbx
000000018030BAD2 call      0x18024f360
000000018030BAD7 mov       r9, qword ptr [rbx + 0x48]
000000018030BADB test      r9, r9
000000018030BADE je        0x18030bca0
000000018030BAE4 mov       rax, qword ptr [rip + 0x18a13b5] # 0x181bacea0
000000018030BAEB inc       dword ptr [r9 + 0x1c]
000000018030BAEF mov       rcx, qword ptr [r9 + 0x10]
000000018030BAF3 movsxd    rdx, dword ptr [r9 + 0x18]
000000018030BAF7 test      rcx, rcx
000000018030BAFA je        0x18030bca0
000000018030BB00 cmp       edx, dword ptr [rcx + 0x18]
000000018030BB03 jb        0x18030bb21
000000018030BB05 mov       rax, qword ptr [rax + 0x20]
000000018030BB09 mov       rdx, rdi
000000018030BB0C mov       rcx, r9
000000018030BB0F mov       r8, qword ptr [rax + 0xc0]
000000018030BB16 mov       r8, qword ptr [r8 + 0x70]
000000018030BB1A call      0x180847ce0
000000018030BB1F jmp       0x18030bb46
000000018030BB21 lea       eax, [rdx + 1]
000000018030BB24 mov       dword ptr [r9 + 0x18], eax
000000018030BB28 cmp       edx, dword ptr [rcx + 0x18]
000000018030BB2B jae       0x18030bca6
000000018030BB31 mov       qword ptr [rcx + rdx*8 + 0x20], rdi
000000018030BB36 lea       rcx, [rcx + rdx*8]
000000018030BB3A add       rcx, 0x20
000000018030BB3E mov       rdx, rdi
000000018030BB41 call      0x18024f360
000000018030BB46 xor       edx, edx
000000018030BB48 mov       rcx, rdi
000000018030BB4B call      0x181304510
000000018030BB50 mov       rcx, qword ptr [rbx + 0x40]
000000018030BB54 mov       rdi, rax
000000018030BB57 test      rcx, rcx
000000018030BB5A je        0x18030bca0
000000018030BB60 xor       edx, edx
000000018030BB62 call      0x18131c070
000000018030BB67 test      rdi, rdi
000000018030BB6A je        0x18030bca0
000000018030BB70 xor       r9d, r9d
000000018030BB73 xor       r8d, r8d
000000018030BB76 mov       rdx, rax
000000018030BB79 mov       rcx, rdi
000000018030BB7C call      0x18132e900
000000018030BB81 mov       rax, qword ptr [rbx + 0x48]
000000018030BB85 test      rax, rax
000000018030BB88 je        0x18030bca0
000000018030BB8E jmp       0x18030ba60
000000018030BB93 mov       rax, qword ptr [rbx + 0x48]
000000018030BB97 xor       esi, esi
000000018030BB99 mov       ecx, esi
000000018030BB9B test      rax, rax
000000018030BB9E je        0x18030bca0
000000018030BBA4 cmp       ecx, dword ptr [rax + 0x18]
000000018030BBA7 jge       0x18030bc55
000000018030BBAD mov       rcx, qword ptr [rbx + 0x48]
000000018030BBB1 test      rcx, rcx
000000018030BBB4 je        0x18030bca0
000000018030BBBA mov       r8, qword ptr [rip + 0x18a13e7] # 0x181bacfa8
000000018030BBC1 mov       edx, esi
000000018030BBC3 call      0x180825710
000000018030BBC8 mov       rdi, rax
000000018030BBCB test      rax, rax
000000018030BBCE je        0x18030bca0
000000018030BBD4 mov       eax, esi
000000018030BBD6 cdq       
000000018030BBD7 idiv      r15d
000000018030BBDA mov       dword ptr [rdi + 0x20], edx
000000018030BBDD mov       dword ptr [rdi + 0x24], eax
000000018030BBE0 mov       rcx, qword ptr [r13 + 0x10]
000000018030BBE4 test      rcx, rcx
000000018030BBE7 je        0x18030bca0
000000018030BBED mov       r8, qword ptr [rip + 0x18ac5fc] # 0x181bb81f0
000000018030BBF4 mov       edx, eax
000000018030BBF6 call      0x180825710
000000018030BBFB test      rax, rax
000000018030BBFE je        0x18030bca0
000000018030BC04 mov       rcx, qword ptr [rax + 0x18]
000000018030BC08 test      rcx, rcx
000000018030BC0B je        0x18030bca0
000000018030BC11 mov       r8, qword ptr [rip + 0x18a1180] # 0x181bacd98
000000018030BC18 mov       edx, dword ptr [rdi + 0x20]
000000018030BC1B call      0x180825710
000000018030BC20 test      rax, rax
000000018030BC23 je        0x18030bca0
000000018030BC25 mov       ecx, dword ptr [rax + 0x10]
000000018030BC28 xor       edx, edx
000000018030BC2A mov       dword ptr [rdi + 0x28], ecx
000000018030BC2D mov       ecx, dword ptr [rax + 0x18]
000000018030BC30 mov       dword ptr [rdi + 0x2c], ecx
000000018030BC33 mov       rcx, rdi
000000018030BC36 movzx     eax, byte ptr [rax + 0x14]
000000018030BC3A mov       byte ptr [rdi + 0x30], al
000000018030BC3D call      0x18039d5b0
000000018030BC42 mov       rax, qword ptr [rbx + 0x48]
000000018030BC46 inc       esi
000000018030BC48 mov       ecx, esi
000000018030BC4A test      rax, rax
000000018030BC4D jne       0x18030bba4
000000018030BC53 jmp       0x18030bca0
000000018030BC55 mov       rcx, qword ptr [rbx + 0x40]
000000018030BC59 test      rcx, rcx
000000018030BC5C je        0x18030bca0
000000018030BC5E mov       rdx, qword ptr [rip + 0x1899073] # 0x181ba4cd8
000000018030BC65 call      0x18046dc80
000000018030BC6A test      rax, rax
000000018030BC6D je        0x18030bca0
000000018030BC6F xor       r8d, r8d
000000018030BC72 mov       edx, r15d
000000018030BC75 mov       rcx, rax
000000018030BC78 mov       r15, qword ptr [rsp + 0x20]
000000018030BC7D mov       r14, qword ptr [rsp + 0x28]
000000018030BC82 mov       r13, qword ptr [rsp + 0x58]
000000018030BC87 mov       rdi, qword ptr [rsp + 0x50]
000000018030BC8C mov       rsi, qword ptr [rsp + 0x48]
000000018030BC91 mov       rbp, qword ptr [rsp + 0x40]
000000018030BC96 add       rsp, 0x30
000000018030BC9A pop       rbx
000000018030BC9B jmp       0x18154a7e0
000000018030BCA0 call      0x180250150
000000018030BCA5 int3      
000000018030BCA6 call      0x180250140
000000018030BCAB int3      
000000018030BCAC int3      
000000018030BCAD int3      
000000018030BCAE int3      
000000018030BCAF int3      