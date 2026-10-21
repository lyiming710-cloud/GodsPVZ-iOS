000000018034C3E0 mov       qword ptr [rsp + 0x10], rbx
000000018034C3E5 mov       qword ptr [rsp + 8], rcx
000000018034C3EA push      rdi
000000018034C3EB sub       rsp, 0x70
000000018034C3EF mov       rdi, rcx
000000018034C3F2 cmp       byte ptr [rip + 0x19988c6], 0 # 0x181ce4cbf
000000018034C3F9 jne       0x18034c456
000000018034C3FB lea       rcx, [rip + 0x1876406] # 0x181bc2808
000000018034C402 call      0x18024fef0
000000018034C407 lea       rcx, [rip + 0x1876452] # 0x181bc2860
000000018034C40E call      0x18024fef0
000000018034C413 lea       rcx, [rip + 0x187649e] # 0x181bc28b8
000000018034C41A call      0x18024fef0
000000018034C41F lea       rcx, [rip + 0x185ceaa] # 0x181ba92d0
000000018034C426 call      0x18024fef0
000000018034C42B lea       rcx, [rip + 0x185cef6] # 0x181ba9328
000000018034C432 call      0x18024fef0
000000018034C437 lea       rcx, [rip + 0x185cf42] # 0x181ba9380
000000018034C43E call      0x18024fef0
000000018034C443 lea       rcx, [rip + 0x185cf8e] # 0x181ba93d8
000000018034C44A call      0x18024fef0
000000018034C44F mov       byte ptr [rip + 0x1998869], 1 # 0x181ce4cbf
000000018034C456 mov       rdx, qword ptr [rdi + 0xb0]
000000018034C45D test      rdx, rdx
000000018034C460 je        0x18034c737
000000018034C466 mov       r8, qword ptr [rip + 0x185cf13] # 0x181ba9380
000000018034C46D lea       rcx, [rsp + 0x38]
000000018034C472 call      0x1808197f0
000000018034C477 movups    xmm0, xmmword ptr [rsp + 0x38]
000000018034C47C movups    xmmword ptr [rsp + 0x50], xmm0
000000018034C481 movsd     xmm1, qword ptr [rsp + 0x48]
000000018034C487 movsd     qword ptr [rsp + 0x60], xmm1
000000018034C48D mov       qword ptr [rsp + 0x38], 0
000000018034C496 lea       rbx, [rsp + 0x50]
000000018034C49B mov       qword ptr [rsp + 0x40], rbx
000000018034C4A0 mov       rdx, qword ptr [rip + 0x18763b9] # 0x181bc2860
000000018034C4A7 lea       rcx, [rsp + 0x50]
000000018034C4AC call      0x180669280
000000018034C4B1 test      al, al
000000018034C4B3 je        0x18034c529
000000018034C4B5 mov       rcx, qword ptr [rdi + 0xa8]
000000018034C4BC mov       rdx, qword ptr [rsp + 0x60]
000000018034C4C1 test      rcx, rcx
000000018034C4C4 je        0x18034c748
000000018034C4CA mov       r10, qword ptr [rip + 0x185cdff] # 0x181ba92d0
000000018034C4D1 inc       dword ptr [rcx + 0x1c]
000000018034C4D4 mov       r8, qword ptr [rcx + 0x10]
000000018034C4D8 movsxd    r9, dword ptr [rcx + 0x18]
000000018034C4DC test      r8, r8
000000018034C4DF je        0x18034c743
000000018034C4E5 cmp       r9d, dword ptr [r8 + 0x18]
000000018034C4E9 jb        0x18034c501
000000018034C4EB mov       rax, qword ptr [r10 + 0x20]
000000018034C4EF mov       r8, qword ptr [rax + 0xc0]
000000018034C4F6 mov       r8, qword ptr [r8 + 0x70]
000000018034C4FA call      0x180847ce0
000000018034C4FF jmp       0x18034c4a0
000000018034C501 lea       eax, [r9 + 1]
000000018034C505 mov       dword ptr [rcx + 0x18], eax
000000018034C508 cmp       r9d, dword ptr [r8 + 0x18]
000000018034C50C jae       0x18034c73d
000000018034C512 mov       qword ptr [r8 + r9*8 + 0x20], rdx
000000018034C517 add       r8, 0x20
000000018034C51B lea       rcx, [r8 + r9*8]
000000018034C51F call      0x18024f360
000000018034C524 jmp       0x18034c4a0
000000018034C529 mov       rdx, qword ptr [rip + 0x18762d8] # 0x181bc2808
000000018034C530 mov       rcx, rbx
000000018034C533 call      0x180302170
000000018034C538 jmp       0x18034c561
000000018034C53A mov       rdx, qword ptr [rip + 0x18762c7] # 0x181bc2808
000000018034C541 mov       rcx, qword ptr [rsp + 0x40]
000000018034C546 call      0x180302170
000000018034C54B mov       rcx, qword ptr [rsp + 0x38]
000000018034C550 test      rcx, rcx
000000018034C553 jne       0x18034c74e
000000018034C559 mov       rdi, qword ptr [rsp + 0x80]
000000018034C561 mov       rcx, qword ptr [rdi + 0xb0]
000000018034C568 test      rcx, rcx
000000018034C56B je        0x18034c737
000000018034C571 inc       dword ptr [rcx + 0x1c]
000000018034C574 mov       r8d, dword ptr [rcx + 0x18]
000000018034C578 mov       dword ptr [rcx + 0x18], 0
000000018034C57F test      r8d, r8d
000000018034C582 jle       0x18034c592
000000018034C584 xor       r9d, r9d
000000018034C587 xor       edx, edx
000000018034C589 mov       rcx, qword ptr [rcx + 0x10]
000000018034C58D call      0x180cb86f0
000000018034C592 mov       rdx, qword ptr [rdi + 0xa8]
000000018034C599 test      rdx, rdx
000000018034C59C je        0x18034c737
000000018034C5A2 mov       r8, qword ptr [rip + 0x185cdd7] # 0x181ba9380
000000018034C5A9 lea       rcx, [rsp + 0x38]
000000018034C5AE call      0x1808197f0
000000018034C5B3 movups    xmm0, xmmword ptr [rsp + 0x38]
000000018034C5B8 movups    xmmword ptr [rsp + 0x50], xmm0
000000018034C5BD movsd     xmm1, qword ptr [rsp + 0x48]
000000018034C5C3 movsd     qword ptr [rsp + 0x60], xmm1
000000018034C5C9 mov       qword ptr [rsp + 0x38], 0
000000018034C5D2 lea       rbx, [rsp + 0x50]
000000018034C5D7 mov       qword ptr [rsp + 0x40], rbx
000000018034C5DC nop       dword ptr [rax]
000000018034C5E0 mov       rdx, qword ptr [rip + 0x1876279] # 0x181bc2860
000000018034C5E7 lea       rcx, [rsp + 0x50]
000000018034C5EC call      0x180669280
000000018034C5F1 test      al, al
000000018034C5F3 je        0x18034c60c
000000018034C5F5 mov       rcx, qword ptr [rsp + 0x60]
000000018034C5FA test      rcx, rcx
000000018034C5FD je        0x18034c754
000000018034C603 xor       edx, edx
000000018034C605 call      0x180312140
000000018034C60A jmp       0x18034c5e0
000000018034C60C mov       rdx, qword ptr [rip + 0x18761f5] # 0x181bc2808
000000018034C613 mov       rcx, rbx
000000018034C616 call      0x180302170
000000018034C61B jmp       0x18034c644
000000018034C61D mov       rdx, qword ptr [rip + 0x18761e4] # 0x181bc2808
000000018034C624 mov       rcx, qword ptr [rsp + 0x40]
000000018034C629 call      0x180302170
000000018034C62E mov       rcx, qword ptr [rsp + 0x38]
000000018034C633 test      rcx, rcx
000000018034C636 jne       0x18034c75a
000000018034C63C mov       rdi, qword ptr [rsp + 0x80]
000000018034C644 mov       rdx, qword ptr [rdi + 0xb8]
000000018034C64B test      rdx, rdx
000000018034C64E je        0x18034c737
000000018034C654 mov       r8, qword ptr [rip + 0x185cd25] # 0x181ba9380
000000018034C65B lea       rcx, [rsp + 0x38]
000000018034C660 call      0x1808197f0
000000018034C665 movups    xmm0, xmmword ptr [rsp + 0x38]
000000018034C66A movups    xmmword ptr [rsp + 0x50], xmm0
000000018034C66F movsd     xmm1, qword ptr [rsp + 0x48]
000000018034C675 movsd     qword ptr [rsp + 0x60], xmm1
000000018034C67B mov       qword ptr [rsp + 0x38], 0
000000018034C684 lea       rbx, [rsp + 0x50]
000000018034C689 mov       qword ptr [rsp + 0x40], rbx
000000018034C68E nop       
000000018034C690 mov       rdx, qword ptr [rip + 0x18761c9] # 0x181bc2860
000000018034C697 lea       rcx, [rsp + 0x50]
000000018034C69C call      0x180669280
000000018034C6A1 test      al, al
000000018034C6A3 je        0x18034c6c8
000000018034C6A5 mov       rcx, qword ptr [rdi + 0xa8]
000000018034C6AC test      rcx, rcx
000000018034C6AF je        0x18034c760
000000018034C6B5 mov       r8, qword ptr [rip + 0x185cd1c] # 0x181ba93d8
000000018034C6BC mov       rdx, qword ptr [rsp + 0x60]
000000018034C6C1 call      0x18084e290
000000018034C6C6 jmp       0x18034c690
000000018034C6C8 mov       rdx, qword ptr [rip + 0x1876139] # 0x181bc2808
000000018034C6CF mov       rcx, rbx
000000018034C6D2 call      0x180302170
000000018034C6D7 jmp       0x18034c6fc
000000018034C6D9 mov       rdx, qword ptr [rip + 0x1876128] # 0x181bc2808
000000018034C6E0 mov       rcx, qword ptr [rsp + 0x40]
000000018034C6E5 call      0x180302170
000000018034C6EA mov       rcx, qword ptr [rsp + 0x38]
000000018034C6EF test      rcx, rcx
000000018034C6F2 jne       0x18034c766
000000018034C6F4 mov       rdi, qword ptr [rsp + 0x80]
000000018034C6FC mov       rcx, qword ptr [rdi + 0xb8]
000000018034C703 test      rcx, rcx
000000018034C706 je        0x18034c737
000000018034C708 inc       dword ptr [rcx + 0x1c]
000000018034C70B mov       r8d, dword ptr [rcx + 0x18]
000000018034C70F mov       dword ptr [rcx + 0x18], 0
000000018034C716 test      r8d, r8d
000000018034C719 jle       0x18034c729
000000018034C71B xor       r9d, r9d
000000018034C71E xor       edx, edx
000000018034C720 mov       rcx, qword ptr [rcx + 0x10]
000000018034C724 call      0x180cb86f0
000000018034C729 mov       rbx, qword ptr [rsp + 0x88]
000000018034C731 add       rsp, 0x70
000000018034C735 pop       rdi
000000018034C736 ret       
000000018034C737 call      0x180250150
000000018034C73C nop       
000000018034C73D call      0x180250140
000000018034C742 nop       
000000018034C743 call      0x180250150
000000018034C748 call      0x180250150
000000018034C74D nop       
000000018034C74E call      0x180200e60
000000018034C753 nop       
000000018034C754 call      0x180250150
000000018034C759 nop       
000000018034C75A call      0x180200e60
000000018034C75F nop       
000000018034C760 call      0x180250150
000000018034C765 nop       
000000018034C766 call      0x180200e60
000000018034C76B int3      
000000018034C76C int3      
000000018034C76D int3      
000000018034C76E int3      
000000018034C76F int3      