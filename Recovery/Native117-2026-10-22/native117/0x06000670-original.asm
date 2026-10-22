00000001803A5A50 push      rbx
00000001803A5A52 sub       rsp, 0x20
00000001803A5A56 cmp       byte ptr [rip + 0x193f482], 0 # 0x181ce4edf
00000001803A5A5D mov       rbx, rcx
00000001803A5A60 jne       0x1803a5b5d
00000001803A5A66 lea       rcx, [rip + 0x181528b] # 0x181bbacf8
00000001803A5A6D call      0x18024fef0
00000001803A5A72 lea       rcx, [rip + 0x17fb317] # 0x181ba0d90
00000001803A5A79 call      0x18024fef0
00000001803A5A7E lea       rcx, [rip + 0x17fb1eb] # 0x181ba0c70
00000001803A5A85 call      0x18024fef0
00000001803A5A8A lea       rcx, [rip + 0x17fb3bf] # 0x181ba0e50
00000001803A5A91 call      0x18024fef0
00000001803A5A96 lea       rcx, [rip + 0x17fdbcb] # 0x181ba3668
00000001803A5A9D call      0x18024fef0
00000001803A5AA2 lea       rcx, [rip + 0x17fb827] # 0x181ba12d0
00000001803A5AA9 call      0x18024fef0
00000001803A5AAE lea       rcx, [rip + 0x1801933] # 0x181ba73e8
00000001803A5AB5 call      0x18024fef0
00000001803A5ABA lea       rcx, [rip + 0x17ff6a7] # 0x181ba5168
00000001803A5AC1 call      0x18024fef0
00000001803A5AC6 lea       rcx, [rip + 0x17fad83] # 0x181ba0850
00000001803A5ACD call      0x18024fef0
00000001803A5AD2 lea       rcx, [rip + 0x17fab37] # 0x181ba0610
00000001803A5AD9 call      0x18024fef0
00000001803A5ADE lea       rcx, [rip + 0x17ff323] # 0x181ba4e08
00000001803A5AE5 call      0x18024fef0
00000001803A5AEA lea       rcx, [rip + 0x1800e77] # 0x181ba6968
00000001803A5AF1 call      0x18024fef0
00000001803A5AF6 lea       rcx, [rip + 0x17ff78b] # 0x181ba5288
00000001803A5AFD call      0x18024fef0
00000001803A5B02 lea       rcx, [rip + 0x17fb2e7] # 0x181ba0df0
00000001803A5B09 call      0x18024fef0
00000001803A5B0E lea       rcx, [rip + 0x17fc1db] # 0x181ba1cf0
00000001803A5B15 call      0x18024fef0
00000001803A5B1A lea       rcx, [rip + 0x18019e7] # 0x181ba7508
00000001803A5B21 call      0x18024fef0
00000001803A5B26 lea       rcx, [rip + 0x17fe07b] # 0x181ba3ba8
00000001803A5B2D call      0x18024fef0
00000001803A5B32 lea       rcx, [rip + 0x182954f] # 0x181bcf088
00000001803A5B39 call      0x18024fef0
00000001803A5B3E lea       rcx, [rip + 0x17fad6b] # 0x181ba08b0
00000001803A5B45 call      0x18024fef0
00000001803A5B4A lea       rcx, [rip + 0x17ff317] # 0x181ba4e68
00000001803A5B51 call      0x18024fef0
00000001803A5B56 mov       byte ptr [rip + 0x193f382], 1 # 0x181ce4edf
00000001803A5B5D cmp       dword ptr [rbx + 0x20], 1
00000001803A5B61 mov       rax, qword ptr [rip + 0x1815190] # 0x181bbacf8
00000001803A5B68 mov       rcx, qword ptr [rax + 0xb8]
00000001803A5B6F mov       rax, qword ptr [rcx]
00000001803A5B72 jne       0x1803a5bbd
00000001803A5B74 mov       ecx, dword ptr [rbx + 0x60]
00000001803A5B77 test      ecx, ecx
00000001803A5B79 je        0x1803a5ba0
00000001803A5B7B sub       ecx, 1
00000001803A5B7E je        0x1803a5b97
00000001803A5B80 cmp       ecx, 1
00000001803A5B83 je        0x1803a5b8e
00000001803A5B85 mov       rdx, qword ptr [rip + 0x18294fc] # 0x181bcf088
00000001803A5B8C jmp       0x1803a5ba7
00000001803A5B8E mov       rdx, qword ptr [rip + 0x1801973] # 0x181ba7508
00000001803A5B95 jmp       0x1803a5ba7
00000001803A5B97 mov       rdx, qword ptr [rip + 0x17fe00a] # 0x181ba3ba8
00000001803A5B9E jmp       0x1803a5ba7
00000001803A5BA0 mov       rdx, qword ptr [rip + 0x17fb729] # 0x181ba12d0
00000001803A5BA7 mov       r8, qword ptr [rip + 0x17fad02] # 0x181ba08b0
00000001803A5BAE xor       r9d, r9d
00000001803A5BB1 mov       rcx, qword ptr [rip + 0x17ff6d0] # 0x181ba5288
00000001803A5BB8 call      0x180b73f50
00000001803A5BBD cmp       dword ptr [rbx + 0x20], 3
00000001803A5BC1 jne       0x1803a5c97
00000001803A5BC7 movsxd    rax, dword ptr [rbx + 0x60]
00000001803A5BCB cmp       eax, 0xe
00000001803A5BCE ja        0x1803a5c90
00000001803A5BD4 lea       rdx, [rip - 0x3a5bdb] # 0x180000000
00000001803A5BDB mov       ecx, dword ptr [rdx + rax*4 + 0x3a5ca0]
00000001803A5BE2 add       rcx, rdx
00000001803A5BE5 jmp       rcx
00000001803A5BE7 mov       rax, qword ptr [rip + 0x1800d7a] # 0x181ba6968
00000001803A5BEE add       rsp, 0x20
00000001803A5BF2 pop       rbx
00000001803A5BF3 ret       
00000001803A5BF4 mov       rax, qword ptr [rip + 0x17faa15] # 0x181ba0610
00000001803A5BFB add       rsp, 0x20
00000001803A5BFF pop       rbx
00000001803A5C00 ret       
00000001803A5C01 mov       rax, qword ptr [rip + 0x17fda60] # 0x181ba3668
00000001803A5C08 add       rsp, 0x20
00000001803A5C0C pop       rbx
00000001803A5C0D ret       
00000001803A5C0E mov       rax, qword ptr [rip + 0x18017d3] # 0x181ba73e8
00000001803A5C15 add       rsp, 0x20
00000001803A5C19 pop       rbx
00000001803A5C1A ret       
00000001803A5C1B mov       rax, qword ptr [rip + 0x17fc0ce] # 0x181ba1cf0
00000001803A5C22 add       rsp, 0x20
00000001803A5C26 pop       rbx
00000001803A5C27 ret       
00000001803A5C28 mov       rax, qword ptr [rip + 0x17ff239] # 0x181ba4e68
00000001803A5C2F add       rsp, 0x20
00000001803A5C33 pop       rbx
00000001803A5C34 ret       
00000001803A5C35 mov       rax, qword ptr [rip + 0x17fb154] # 0x181ba0d90
00000001803A5C3C add       rsp, 0x20
00000001803A5C40 pop       rbx
00000001803A5C41 ret       
00000001803A5C42 mov       rax, qword ptr [rip + 0x17ff51f] # 0x181ba5168
00000001803A5C49 add       rsp, 0x20
00000001803A5C4D pop       rbx
00000001803A5C4E ret       
00000001803A5C4F mov       rax, qword ptr [rip + 0x17fabfa] # 0x181ba0850
00000001803A5C56 add       rsp, 0x20
00000001803A5C5A pop       rbx
00000001803A5C5B ret       
00000001803A5C5C mov       rax, qword ptr [rip + 0x17fb1ed] # 0x181ba0e50
00000001803A5C63 add       rsp, 0x20
00000001803A5C67 pop       rbx
00000001803A5C68 ret       
00000001803A5C69 mov       rax, qword ptr [rip + 0x17ff198] # 0x181ba4e08
00000001803A5C70 add       rsp, 0x20
00000001803A5C74 pop       rbx
00000001803A5C75 ret       
00000001803A5C76 mov       rax, qword ptr [rip + 0x17fb173] # 0x181ba0df0
00000001803A5C7D add       rsp, 0x20
00000001803A5C81 pop       rbx
00000001803A5C82 ret       
00000001803A5C83 mov       rax, qword ptr [rip + 0x17fafe6] # 0x181ba0c70
00000001803A5C8A add       rsp, 0x20
00000001803A5C8E pop       rbx
00000001803A5C8F ret       
00000001803A5C90 mov       rax, qword ptr [rip + 0x18293f1] # 0x181bcf088
00000001803A5C97 add       rsp, 0x20
00000001803A5C9B pop       rbx
00000001803A5C9C ret       
00000001803A5C9D nop       dword ptr [rax]
00000001803A5CA0 nop       
00000001803A5CA1 pop       rsp
00000001803A5CA2 cmp       al, byte ptr [rax]
00000001803A5CA4 nop       
00000001803A5CA5 pop       rsp
00000001803A5CA6 cmp       al, byte ptr [rax]
00000001803A5CA8 out       0x5b, eax
00000001803A5CAA cmp       al, byte ptr [rax]
00000001803A5CAC hlt       
00000001803A5CAD pop       rbx
00000001803A5CAE cmp       al, byte ptr [rax]
00000001803A5CB0 add       dword ptr [rdx + rdi], ebx