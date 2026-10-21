00000001803C4890 mov       qword ptr [rsp + 0x10], rbx
00000001803C4895 push      rsi
00000001803C4896 sub       rsp, 0x20
00000001803C489A cmp       byte ptr [rip + 0x1920777], 0 # 0x181ce5018
00000001803C48A1 mov       esi, edx
00000001803C48A3 mov       ebx, ecx
00000001803C48A5 jne       0x1803c48ea
00000001803C48A7 lea       rcx, [rip + 0x17d885a] # 0x181b9d108
00000001803C48AE call      0x18024fef0
00000001803C48B3 lea       rcx, [rip + 0x17eb07e] # 0x181baf938
00000001803C48BA call      0x18024fef0
00000001803C48BF lea       rcx, [rip + 0x17eb17a] # 0x181bafa40
00000001803C48C6 call      0x18024fef0
00000001803C48CB lea       rcx, [rip + 0x17eb4de] # 0x181bafdb0
00000001803C48D2 call      0x18024fef0
00000001803C48D7 lea       rcx, [rip + 0x17eb5da] # 0x181bafeb8
00000001803C48DE call      0x18024fef0
00000001803C48E3 mov       byte ptr [rip + 0x192072e], 1 # 0x181ce5018
00000001803C48EA mov       rax, qword ptr [rip + 0x17d8817] # 0x181b9d108
00000001803C48F1 cmp       dword ptr [rax + 0xe0], 0
00000001803C48F8 jne       0x1803c4909
00000001803C48FA mov       rcx, rax
00000001803C48FD call      0x1802501e0
00000001803C4902 mov       rax, qword ptr [rip + 0x17d87ff] # 0x181b9d108
00000001803C4909 mov       rax, qword ptr [rax + 0xb8]
00000001803C4910 mov       qword ptr [rsp + 0x30], rdi
00000001803C4915 mov       rcx, qword ptr [rax]
00000001803C4918 test      rcx, rcx
00000001803C491B je        0x1803c4c60
00000001803C4921 inc       dword ptr [rcx + 0x1c]
00000001803C4924 xor       edi, edi
00000001803C4926 mov       dword ptr [rcx + 0x18], edi
00000001803C4929 mov       rax, qword ptr [rip + 0x17d87d8] # 0x181b9d108
00000001803C4930 mov       rcx, qword ptr [rax + 0xb8]
00000001803C4937 mov       rcx, qword ptr [rcx]
00000001803C493A test      rcx, rcx
00000001803C493D je        0x1803c4c60
00000001803C4943 mov       rdx, qword ptr [rip + 0x17eb466] # 0x181bafdb0
00000001803C494A call      0x18065baa0
00000001803C494F cmp       eax, esi
00000001803C4951 jge       0x1803c4993
00000001803C4953 mov       rax, qword ptr [rip + 0x17d87ae] # 0x181b9d108
00000001803C495A cmp       dword ptr [rax + 0xe0], edi
00000001803C4960 jne       0x1803c4971
00000001803C4962 mov       rcx, rax
00000001803C4965 call      0x1802501e0
00000001803C496A mov       rax, qword ptr [rip + 0x17d8797] # 0x181b9d108
00000001803C4971 mov       rax, qword ptr [rax + 0xb8]
00000001803C4978 mov       rcx, qword ptr [rax]
00000001803C497B test      rcx, rcx
00000001803C497E je        0x1803c4c60
00000001803C4984 mov       r8, qword ptr [rip + 0x17eb52d] # 0x181bafeb8
00000001803C498B lea       edx, [rsi + rsi]
00000001803C498E call      0x180805920
00000001803C4993 test      esi, esi
00000001803C4995 jle       0x1803c4c50
00000001803C499B add       ebx, 2
00000001803C499E nop       
00000001803C49A0 mov       rax, qword ptr [rip + 0x17d8761] # 0x181b9d108
00000001803C49A7 cmp       dword ptr [rax + 0xe0], 0
00000001803C49AE jne       0x1803c49bf
00000001803C49B0 mov       rcx, rax
00000001803C49B3 call      0x1802501e0
00000001803C49B8 mov       rax, qword ptr [rip + 0x17d8749] # 0x181b9d108
00000001803C49BF mov       rax, qword ptr [rax + 0xb8]
00000001803C49C6 mov       rcx, qword ptr [rax]
00000001803C49C9 test      rcx, rcx
00000001803C49CC je        0x1803c4c60
00000001803C49D2 mov       r9, qword ptr [rip + 0x17eaf5f] # 0x181baf938
00000001803C49D9 inc       dword ptr [rcx + 0x1c]
00000001803C49DC mov       r8, qword ptr [rcx + 0x10]
00000001803C49E0 movsxd    rdx, dword ptr [rcx + 0x18]
00000001803C49E4 test      r8, r8
00000001803C49E7 je        0x1803c4c60
00000001803C49ED cmp       edx, dword ptr [r8 + 0x18]
00000001803C49F1 jb        0x1803c4a0b
00000001803C49F3 mov       rax, qword ptr [r9 + 0x20]
00000001803C49F7 mov       edx, ebx
00000001803C49F9 mov       r8, qword ptr [rax + 0xc0]
00000001803C4A00 mov       r8, qword ptr [r8 + 0x70]
00000001803C4A04 call      0x180827460
00000001803C4A09 jmp       0x1803c4a20
00000001803C4A0B lea       eax, [rdx + 1]
00000001803C4A0E mov       dword ptr [rcx + 0x18], eax
00000001803C4A11 cmp       edx, dword ptr [r8 + 0x18]
00000001803C4A15 jae       0x1803c4c66
00000001803C4A1B mov       dword ptr [r8 + rdx*4 + 0x20], ebx
00000001803C4A20 mov       rax, qword ptr [rip + 0x17d86e1] # 0x181b9d108
00000001803C4A27 mov       rcx, qword ptr [rax + 0xb8]
00000001803C4A2E mov       r9, qword ptr [rcx]
00000001803C4A31 test      r9, r9
00000001803C4A34 je        0x1803c4c60
00000001803C4A3A mov       rcx, qword ptr [rip + 0x17eaef7] # 0x181baf938
00000001803C4A41 inc       dword ptr [r9 + 0x1c]
00000001803C4A45 mov       r8, qword ptr [r9 + 0x10]
00000001803C4A49 movsxd    rdx, dword ptr [r9 + 0x18]
00000001803C4A4D test      r8, r8
00000001803C4A50 je        0x1803c4c60
00000001803C4A56 cmp       edx, dword ptr [r8 + 0x18]
00000001803C4A5A jb        0x1803c4a78
00000001803C4A5C mov       rax, qword ptr [rcx + 0x20]
00000001803C4A60 lea       edx, [rbx - 1]
00000001803C4A63 mov       rcx, r9
00000001803C4A66 mov       r8, qword ptr [rax + 0xc0]
00000001803C4A6D mov       r8, qword ptr [r8 + 0x70]
00000001803C4A71 call      0x180827460
00000001803C4A76 jmp       0x1803c4a91
00000001803C4A78 lea       eax, [rdx + 1]
00000001803C4A7B mov       dword ptr [r9 + 0x18], eax
00000001803C4A7F cmp       edx, dword ptr [r8 + 0x18]
00000001803C4A83 jae       0x1803c4c66
00000001803C4A89 lea       ecx, [rbx - 1]
00000001803C4A8C mov       dword ptr [r8 + rdx*4 + 0x20], ecx
00000001803C4A91 mov       rax, qword ptr [rip + 0x17d8670] # 0x181b9d108
00000001803C4A98 mov       rcx, qword ptr [rax + 0xb8]
00000001803C4A9F mov       r9, qword ptr [rcx]
00000001803C4AA2 test      r9, r9
00000001803C4AA5 je        0x1803c4c60
00000001803C4AAB mov       rcx, qword ptr [rip + 0x17eae86] # 0x181baf938
00000001803C4AB2 inc       dword ptr [r9 + 0x1c]
00000001803C4AB6 mov       r8, qword ptr [r9 + 0x10]
00000001803C4ABA movsxd    rdx, dword ptr [r9 + 0x18]
00000001803C4ABE test      r8, r8
00000001803C4AC1 je        0x1803c4c60
00000001803C4AC7 cmp       edx, dword ptr [r8 + 0x18]
00000001803C4ACB jb        0x1803c4ae9
00000001803C4ACD mov       rax, qword ptr [rcx + 0x20]
00000001803C4AD1 lea       edx, [rbx - 2]
00000001803C4AD4 mov       rcx, r9
00000001803C4AD7 mov       r8, qword ptr [rax + 0xc0]
00000001803C4ADE mov       r8, qword ptr [r8 + 0x70]
00000001803C4AE2 call      0x180827460
00000001803C4AE7 jmp       0x1803c4b02
00000001803C4AE9 lea       eax, [rdx + 1]
00000001803C4AEC mov       dword ptr [r9 + 0x18], eax
00000001803C4AF0 cmp       edx, dword ptr [r8 + 0x18]
00000001803C4AF4 jae       0x1803c4c66
00000001803C4AFA lea       ecx, [rbx - 2]
00000001803C4AFD mov       dword ptr [r8 + rdx*4 + 0x20], ecx
00000001803C4B02 mov       rax, qword ptr [rip + 0x17d85ff] # 0x181b9d108
00000001803C4B09 mov       rcx, qword ptr [rax + 0xb8]
00000001803C4B10 mov       r9, qword ptr [rcx]
00000001803C4B13 test      r9, r9
00000001803C4B16 je        0x1803c4c60
00000001803C4B1C mov       rcx, qword ptr [rip + 0x17eae15] # 0x181baf938
00000001803C4B23 inc       dword ptr [r9 + 0x1c]
00000001803C4B27 mov       r8, qword ptr [r9 + 0x10]
00000001803C4B2B movsxd    rdx, dword ptr [r9 + 0x18]
00000001803C4B2F test      r8, r8
00000001803C4B32 je        0x1803c4c60
00000001803C4B38 cmp       edx, dword ptr [r8 + 0x18]
00000001803C4B3C jb        0x1803c4b5a
00000001803C4B3E mov       rax, qword ptr [rcx + 0x20]
00000001803C4B42 lea       edx, [rbx - 2]
00000001803C4B45 mov       rcx, r9
00000001803C4B48 mov       r8, qword ptr [rax + 0xc0]
00000001803C4B4F mov       r8, qword ptr [r8 + 0x70]
00000001803C4B53 call      0x180827460
00000001803C4B58 jmp       0x1803c4b73
00000001803C4B5A lea       eax, [rdx + 1]
00000001803C4B5D mov       dword ptr [r9 + 0x18], eax
00000001803C4B61 cmp       edx, dword ptr [r8 + 0x18]
00000001803C4B65 jae       0x1803c4c66
00000001803C4B6B lea       ecx, [rbx - 2]
00000001803C4B6E mov       dword ptr [r8 + rdx*4 + 0x20], ecx
00000001803C4B73 mov       rax, qword ptr [rip + 0x17d858e] # 0x181b9d108
00000001803C4B7A mov       rcx, qword ptr [rax + 0xb8]
00000001803C4B81 mov       r9, qword ptr [rcx]
00000001803C4B84 test      r9, r9
00000001803C4B87 je        0x1803c4c60
00000001803C4B8D mov       rcx, qword ptr [rip + 0x17eada4] # 0x181baf938
00000001803C4B94 inc       dword ptr [r9 + 0x1c]
00000001803C4B98 mov       r8, qword ptr [r9 + 0x10]
00000001803C4B9C movsxd    rdx, dword ptr [r9 + 0x18]
00000001803C4BA0 test      r8, r8
00000001803C4BA3 je        0x1803c4c60
00000001803C4BA9 cmp       edx, dword ptr [r8 + 0x18]
00000001803C4BAD jb        0x1803c4bcb
00000001803C4BAF mov       rax, qword ptr [rcx + 0x20]
00000001803C4BB3 lea       edx, [rbx + 1]
00000001803C4BB6 mov       rcx, r9
00000001803C4BB9 mov       r8, qword ptr [rax + 0xc0]
00000001803C4BC0 mov       r8, qword ptr [r8 + 0x70]
00000001803C4BC4 call      0x180827460
00000001803C4BC9 jmp       0x1803c4be4
00000001803C4BCB lea       eax, [rdx + 1]
00000001803C4BCE mov       dword ptr [r9 + 0x18], eax
00000001803C4BD2 cmp       edx, dword ptr [r8 + 0x18]
00000001803C4BD6 jae       0x1803c4c66
00000001803C4BDC lea       ecx, [rbx + 1]
00000001803C4BDF mov       dword ptr [r8 + rdx*4 + 0x20], ecx
00000001803C4BE4 mov       rax, qword ptr [rip + 0x17d851d] # 0x181b9d108
00000001803C4BEB mov       rcx, qword ptr [rax + 0xb8]
00000001803C4BF2 mov       r9, qword ptr [rcx]
00000001803C4BF5 test      r9, r9
00000001803C4BF8 je        0x1803c4c60
00000001803C4BFA mov       r8, qword ptr [rip + 0x17ead37] # 0x181baf938
00000001803C4C01 inc       dword ptr [r9 + 0x1c]
00000001803C4C05 mov       rdx, qword ptr [r9 + 0x10]
00000001803C4C09 movsxd    rcx, dword ptr [r9 + 0x18]
00000001803C4C0D test      rdx, rdx
00000001803C4C10 je        0x1803c4c60
00000001803C4C12 cmp       ecx, dword ptr [rdx + 0x18]
00000001803C4C15 jb        0x1803c4c32
00000001803C4C17 mov       rax, qword ptr [r8 + 0x20]
00000001803C4C1B mov       edx, ebx
00000001803C4C1D mov       rcx, r9
00000001803C4C20 mov       r8, qword ptr [rax + 0xc0]
00000001803C4C27 mov       r8, qword ptr [r8 + 0x70]
00000001803C4C2B call      0x180827460
00000001803C4C30 jmp       0x1803c4c42
00000001803C4C32 lea       eax, [rcx + 1]
00000001803C4C35 mov       dword ptr [r9 + 0x18], eax
00000001803C4C39 cmp       ecx, dword ptr [rdx + 0x18]
00000001803C4C3C jae       0x1803c4c66
00000001803C4C3E mov       dword ptr [rdx + rcx*4 + 0x20], ebx
00000001803C4C42 add       ebx, 4
00000001803C4C45 add       edi, 6
00000001803C4C48 cmp       edi, esi
00000001803C4C4A jl        0x1803c49a0
00000001803C4C50 mov       rdi, qword ptr [rsp + 0x30]
00000001803C4C55 mov       rbx, qword ptr [rsp + 0x38]
00000001803C4C5A add       rsp, 0x20
00000001803C4C5E pop       rsi
00000001803C4C5F ret       
00000001803C4C60 call      0x180250150
00000001803C4C65 int3      
00000001803C4C66 call      0x180250140
00000001803C4C6B int3      