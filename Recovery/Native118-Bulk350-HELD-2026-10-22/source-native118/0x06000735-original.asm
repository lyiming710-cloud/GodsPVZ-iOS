00000001803B7710 push      rbp
00000001803B7712 push      rbx
00000001803B7713 lea       rbp, [rsp - 0x4f]
00000001803B7718 sub       rsp, 0xf8
00000001803B771F cmp       byte ptr [rip + 0x192d829], 0 # 0x181ce4f4f
00000001803B7726 mov       rbx, rcx
00000001803B7729 jne       0x1803b774a
00000001803B772B lea       rcx, [rip + 0x181203e] # 0x181bc9770
00000001803B7732 call      0x18024fef0
00000001803B7737 lea       rcx, [rip + 0x1805662] # 0x181bbcda0
00000001803B773E call      0x18024fef0
00000001803B7743 mov       byte ptr [rip + 0x192d805], 1 # 0x181ce4f4f
00000001803B774A mov       rcx, qword ptr [rbx + 0x48]
00000001803B774E xor       eax, eax
00000001803B7750 mov       qword ptr [rsp + 0x118], rsi
00000001803B7758 xorps     xmm1, xmm1
00000001803B775B mov       qword ptr [rsp + 0x120], rdi
00000001803B7763 xorps     xmm0, xmm0
00000001803B7766 mov       qword ptr [rsp + 0x128], r14
00000001803B776E mov       qword ptr [rsp + 0xf0], r15
00000001803B7776 mov       qword ptr [rbp - 0x59], rax
00000001803B777A mov       qword ptr [rbp - 0x31], rax
00000001803B777E movups    xmmword ptr [rbp - 0x69], xmm0
00000001803B7782 movups    xmmword ptr [rbp - 0x51], xmm1
00000001803B7786 movups    xmmword ptr [rbp - 0x41], xmm1
00000001803B778A test      rcx, rcx
00000001803B778D je        0x1803b7dbf
00000001803B7793 xor       edx, edx
00000001803B7795 call      0x1811b84f0
00000001803B779A xor       edx, edx
00000001803B779C lea       rcx, [rbp - 0x79]
00000001803B77A0 mov       rdi, rax
00000001803B77A3 call      0x181364ca0
00000001803B77A8 mov       rcx, qword ptr [rip + 0x18055f1] # 0x181bbcda0
00000001803B77AF mov       r14, qword ptr [rbx + 0x50]
00000001803B77B3 movsd     xmm0, qword ptr [rax]
00000001803B77B7 cmp       dword ptr [rcx + 0xe0], 0
00000001803B77BE mov       esi, dword ptr [rax + 8]
00000001803B77C1 movsd     qword ptr [rbp + 0x67], xmm0
00000001803B77C6 jne       0x1803b77d2
00000001803B77C8 call      0x1802501e0
00000001803B77CD movsd     xmm0, qword ptr [rbp + 0x67]
00000001803B77D2 xor       r9d, r9d
00000001803B77D5 movsd     qword ptr [rbp - 0x79], xmm0
00000001803B77DA mov       r8, r14
00000001803B77DD mov       dword ptr [rbp - 0x71], esi
00000001803B77E0 lea       rdx, [rbp - 0x79]
00000001803B77E4 mov       rcx, rdi
00000001803B77E7 call      0x1811d0420
00000001803B77EC test      al, al
00000001803B77EE jne       0x1803b782a
00000001803B77F0 mov       qword ptr [rbx + 0x60], 0xffffffffffffffff
00000001803B77F8 mov       qword ptr [rbx + 0x68], 0xffffffffffffffff
00000001803B7800 mov       r15, qword ptr [rsp + 0xf0]
00000001803B7808 mov       r14, qword ptr [rsp + 0x128]
00000001803B7810 mov       rdi, qword ptr [rsp + 0x120]
00000001803B7818 mov       rsi, qword ptr [rsp + 0x118]
00000001803B7820 add       rsp, 0xf8
00000001803B7827 pop       rbx
00000001803B7828 pop       rbp
00000001803B7829 ret       
00000001803B782A mov       rsi, qword ptr [rbx + 0x48]
00000001803B782E lea       rcx, [rbp - 0x79]
00000001803B7832 xor       edx, edx
00000001803B7834 call      0x181364ca0
00000001803B7839 mov       rcx, qword ptr [rip + 0x1805560] # 0x181bbcda0
00000001803B7840 mov       r14, qword ptr [rbx + 0x50]
00000001803B7844 movsd     xmm0, qword ptr [rax]
00000001803B7848 cmp       dword ptr [rcx + 0xe0], 0
00000001803B784F mov       edi, dword ptr [rax + 8]
00000001803B7852 movsd     qword ptr [rbp + 0x67], xmm0
00000001803B7857 jne       0x1803b7863
00000001803B7859 call      0x1802501e0
00000001803B785E movsd     xmm0, qword ptr [rbp + 0x67]
00000001803B7863 mov       dword ptr [rbp - 0x71], edi
00000001803B7866 lea       rdx, [rbp - 0x79]
00000001803B786A xor       edi, edi
00000001803B786C movsd     qword ptr [rbp - 0x79], xmm0
00000001803B7871 mov       r9b, 1
00000001803B7874 mov       qword ptr [rsp + 0x20], rdi
00000001803B7879 mov       r8, r14
00000001803B787C mov       rcx, rsi
00000001803B787F call      0x1811cc220
00000001803B7884 movsxd    rsi, eax
00000001803B7887 cmp       esi, -1
00000001803B788A je        0x1803b79a4
00000001803B7890 cmp       esi, dword ptr [rbx + 0x64]
00000001803B7893 je        0x1803b79a4
00000001803B7899 mov       rax, qword ptr [rbx + 0x48]
00000001803B789D mov       dword ptr [rbx + 0x64], esi
00000001803B78A0 test      rax, rax
00000001803B78A3 je        0x1803b7dbf
00000001803B78A9 mov       rdx, qword ptr [rax + 0x370]
00000001803B78B0 test      rdx, rdx
00000001803B78B3 je        0x1803b7dbf
00000001803B78B9 mov       rdx, qword ptr [rdx + 0x38]
00000001803B78BD test      rdx, rdx
00000001803B78C0 je        0x1803b7dbf
00000001803B78C6 cmp       esi, dword ptr [rdx + 0x18]
00000001803B78C9 jae       0x1803b7dc5
00000001803B78CF imul      rcx, rsi, 0x178
00000001803B78D6 mov       eax, dword ptr [rcx + rdx + 0x2c]
00000001803B78DA test      eax, eax
00000001803B78DC je        0x1803b792b
00000001803B78DE cmp       eax, 1
00000001803B78E1 jne       0x1803b79a4
00000001803B78E7 mov       rax, qword ptr [rbx + 0x48]
00000001803B78EB mov       rcx, qword ptr [rax + 0x370]
00000001803B78F2 mov       rdx, qword ptr [rcx + 0x38]
00000001803B78F6 imul      rcx, rsi, 0x178
00000001803B78FD cmp       byte ptr [rip + 0x192d64d], dil # 0x181ce4f51
00000001803B7904 movzx     r14d, word ptr [rcx + rdx + 0x20]
00000001803B790A jne       0x1803b791f
00000001803B790C lea       rcx, [rip + 0x17f4fd5] # 0x181bac8e8
00000001803B7913 call      0x18024fef0
00000001803B7918 mov       byte ptr [rip + 0x192d632], 1 # 0x181ce4f51
00000001803B791F cmp       qword ptr [rbx + 0x28], rdi
00000001803B7923 je        0x1803b79a4
00000001803B7925 mov       rcx, qword ptr [rbx + 0x28]
00000001803B7929 jmp       0x1803b7991
00000001803B792B mov       rax, qword ptr [rbx + 0x48]
00000001803B792F test      rax, rax
00000001803B7932 je        0x1803b7dbf
00000001803B7938 mov       rdx, qword ptr [rax + 0x370]
00000001803B793F test      rdx, rdx
00000001803B7942 je        0x1803b7dbf
00000001803B7948 mov       rdx, qword ptr [rdx + 0x38]
00000001803B794C test      rdx, rdx
00000001803B794F je        0x1803b7dbf
00000001803B7955 cmp       esi, dword ptr [rdx + 0x18]
00000001803B7958 jae       0x1803b7dc5
00000001803B795E imul      rcx, rsi, 0x178
00000001803B7965 cmp       byte ptr [rip + 0x192d5e4], dil # 0x181ce4f50
00000001803B796C movzx     r14d, word ptr [rcx + rdx + 0x20]
00000001803B7972 jne       0x1803b7987
00000001803B7974 lea       rcx, [rip + 0x17f4f6d] # 0x181bac8e8
00000001803B797B call      0x18024fef0
00000001803B7980 mov       byte ptr [rip + 0x192d5c9], 1 # 0x181ce4f50
00000001803B7987 cmp       qword ptr [rbx + 0x20], rdi
00000001803B798B je        0x1803b79a4
00000001803B798D mov       rcx, qword ptr [rbx + 0x20]
00000001803B7991 mov       r9, qword ptr [rip + 0x17f4f50] # 0x181bac8e8
00000001803B7998 mov       r8d, esi
00000001803B799B movzx     edx, r14w
00000001803B799F call      0x180a65040
00000001803B79A4 mov       rsi, qword ptr [rbx + 0x48]
00000001803B79A8 lea       rcx, [rbp - 0x79]
00000001803B79AC xor       edx, edx
00000001803B79AE call      0x181364ca0
00000001803B79B3 mov       rcx, qword ptr [rip + 0x18053e6] # 0x181bbcda0
00000001803B79BA mov       r15, qword ptr [rbx + 0x50]
00000001803B79BE movsd     xmm0, qword ptr [rax]
00000001803B79C2 mov       r14d, dword ptr [rax + 8]
00000001803B79C6 movsd     qword ptr [rbp + 0x67], xmm0
00000001803B79CB cmp       dword ptr [rcx + 0xe0], edi
00000001803B79D1 jne       0x1803b79dd
00000001803B79D3 call      0x1802501e0
00000001803B79D8 movsd     xmm0, qword ptr [rbp + 0x67]
00000001803B79DD xor       r9d, r9d
00000001803B79E0 movsd     qword ptr [rbp - 0x79], xmm0
00000001803B79E5 mov       r8, r15
00000001803B79E8 mov       dword ptr [rbp - 0x71], r14d
00000001803B79EC lea       rdx, [rbp - 0x79]
00000001803B79F0 mov       rcx, rsi
00000001803B79F3 call      0x1811cce00
00000001803B79F8 cmp       eax, -1
00000001803B79FB je        0x1803b7ab0
00000001803B7A01 cmp       eax, dword ptr [rbx + 0x68]
00000001803B7A04 je        0x1803b7ab0
00000001803B7A0A mov       rcx, qword ptr [rbx + 0x48]
00000001803B7A0E mov       dword ptr [rbx + 0x68], eax
00000001803B7A11 test      rcx, rcx
00000001803B7A14 je        0x1803b7dbf
00000001803B7A1A mov       rdx, qword ptr [rcx + 0x370]
00000001803B7A21 test      rdx, rdx
00000001803B7A24 je        0x1803b7dbf
00000001803B7A2A mov       rdx, qword ptr [rdx + 0x40]
00000001803B7A2E test      rdx, rdx
00000001803B7A31 je        0x1803b7dbf
00000001803B7A37 cmp       eax, dword ptr [rdx + 0x18]
00000001803B7A3A jae       0x1803b7dc5
00000001803B7A40 cdqe      
00000001803B7A42 lea       rcx, [rax + rax*2]
00000001803B7A46 movups    xmm0, xmmword ptr [rdx + rcx*8 + 0x20]
00000001803B7A4B movups    xmmword ptr [rbp - 0x69], xmm0
00000001803B7A4F movsd     xmm1, qword ptr [rdx + rcx*8 + 0x30]
00000001803B7A55 xor       edx, edx
00000001803B7A57 lea       rcx, [rbp - 0x69]
00000001803B7A5B movsd     qword ptr [rbp - 0x59], xmm1
00000001803B7A60 call      0x18119c2e0
00000001803B7A65 cmp       byte ptr [rip + 0x192d4e6], dil # 0x181ce4f52
00000001803B7A6C mov       rsi, rax
00000001803B7A6F mov       r14d, dword ptr [rbp - 0x61]
00000001803B7A73 mov       r15d, dword ptr [rbp - 0x59]
00000001803B7A77 jne       0x1803b7a8c
00000001803B7A79 lea       rcx, [rip + 0x17f4fc8] # 0x181baca48
00000001803B7A80 call      0x18024fef0
00000001803B7A85 mov       byte ptr [rip + 0x192d4c6], 1 # 0x181ce4f52
00000001803B7A8C cmp       qword ptr [rbx + 0x30], rdi
00000001803B7A90 je        0x1803b7ab0
00000001803B7A92 mov       rax, qword ptr [rip + 0x17f4faf] # 0x181baca48
00000001803B7A99 mov       r9d, r15d
00000001803B7A9C mov       rcx, qword ptr [rbx + 0x30]
00000001803B7AA0 mov       r8d, r14d
00000001803B7AA3 mov       rdx, rsi
00000001803B7AA6 mov       qword ptr [rsp + 0x20], rax
00000001803B7AAB call      0x180a66460
00000001803B7AB0 mov       rsi, qword ptr [rbx + 0x48]
00000001803B7AB4 lea       rcx, [rbp - 0x79]
00000001803B7AB8 xor       edx, edx
00000001803B7ABA call      0x181364ca0
00000001803B7ABF mov       rcx, qword ptr [rip + 0x18052da] # 0x181bbcda0
00000001803B7AC6 mov       r15, qword ptr [rbx + 0x50]
00000001803B7ACA movsd     xmm0, qword ptr [rax]
00000001803B7ACE mov       r14d, dword ptr [rax + 8]
00000001803B7AD2 movsd     qword ptr [rbp + 0x67], xmm0
00000001803B7AD7 cmp       dword ptr [rcx + 0xe0], edi
00000001803B7ADD jne       0x1803b7ae9
00000001803B7ADF call      0x1802501e0
00000001803B7AE4 movsd     xmm0, qword ptr [rbp + 0x67]
00000001803B7AE9 xor       r9d, r9d
00000001803B7AEC movsd     qword ptr [rbp - 0x79], xmm0
00000001803B7AF1 mov       r8, r15
00000001803B7AF4 mov       dword ptr [rbp - 0x71], r14d
00000001803B7AF8 lea       rdx, [rbp - 0x79]
00000001803B7AFC mov       rcx, rsi
00000001803B7AFF call      0x1811cc540
00000001803B7B04 cmp       eax, -1
00000001803B7B07 je        0x1803b7c9a
00000001803B7B0D cmp       eax, dword ptr [rbx + 0x6c]
00000001803B7B10 je        0x1803b7c9a
00000001803B7B16 mov       rcx, qword ptr [rbx + 0x48]
00000001803B7B1A mov       dword ptr [rbx + 0x6c], eax
00000001803B7B1D test      rcx, rcx
00000001803B7B20 je        0x1803b7dbf
00000001803B7B26 mov       rdx, qword ptr [rcx + 0x370]
00000001803B7B2D test      rdx, rdx
00000001803B7B30 je        0x1803b7dbf
00000001803B7B36 mov       rdx, qword ptr [rdx + 0x50]
00000001803B7B3A test      rdx, rdx
00000001803B7B3D je        0x1803b7dbf
00000001803B7B43 cmp       eax, dword ptr [rdx + 0x18]
00000001803B7B46 jae       0x1803b7dc5
00000001803B7B4C cdqe      
00000001803B7B4E imul      rcx, rax, 0x5c
00000001803B7B52 movups    xmm0, xmmword ptr [rcx + rdx + 0x20]
00000001803B7B57 movups    xmmword ptr [rbp - 0x79], xmm0
00000001803B7B5B movups    xmm0, xmmword ptr [rcx + rdx + 0x30]
00000001803B7B60 mov       rdx, qword ptr [rcx + rdx + 0x20]
00000001803B7B65 mov       rcx, qword ptr [rip + 0x1811c04] # 0x181bc9770
00000001803B7B6C shr       rdx, 0x20
00000001803B7B70 movups    xmmword ptr [rbp - 0x29], xmm0
00000001803B7B74 call      0x18024f3e0
00000001803B7B79 mov       r14, qword ptr [rbp - 0x79]
00000001803B7B7D mov       r9, rax
00000001803B7B80 mov       rsi, qword ptr [rbp - 0x29]
00000001803B7B84 mov       rcx, r14
00000001803B7B87 shr       rcx, 0x20
00000001803B7B8B test      ecx, ecx
00000001803B7B8D jle       0x1803b7c41
00000001803B7B93 mov       r8, r14
00000001803B7B96 shr       r8, 0x20
00000001803B7B9A nop       word ptr [rax + rax]
00000001803B7BA0 mov       rax, qword ptr [rbx + 0x48]
00000001803B7BA4 test      rax, rax
00000001803B7BA7 je        0x1803b7dbf
00000001803B7BAD mov       rax, qword ptr [rax + 0x370]
00000001803B7BB4 test      rax, rax
00000001803B7BB7 je        0x1803b7dbf
00000001803B7BBD mov       rax, qword ptr [rax + 0x38]
00000001803B7BC1 test      rax, rax
00000001803B7BC4 je        0x1803b7dbf
00000001803B7BCA cmp       edi, dword ptr [rax + 0x18]
00000001803B7BCD jge       0x1803b7c41
00000001803B7BCF mov       rax, qword ptr [rbx + 0x48]
00000001803B7BD3 test      rax, rax
00000001803B7BD6 je        0x1803b7dbf
00000001803B7BDC mov       rdx, qword ptr [rax + 0x370]
00000001803B7BE3 test      rdx, rdx
00000001803B7BE6 je        0x1803b7dbf
00000001803B7BEC mov       rdx, qword ptr [rdx + 0x38]
00000001803B7BF0 test      rdx, rdx
00000001803B7BF3 je        0x1803b7dbf
00000001803B7BF9 mov       rcx, rsi
00000001803B7BFC shr       rcx, 0x20
00000001803B7C00 add       ecx, edi
00000001803B7C02 cmp       ecx, dword ptr [rdx + 0x18]
00000001803B7C05 jae       0x1803b7dc5
00000001803B7C0B test      r9, r9
00000001803B7C0E je        0x1803b7dbf
00000001803B7C14 cmp       edi, dword ptr [r9 + 0x18]
00000001803B7C18 jae       0x1803b7dc5
00000001803B7C1E movsxd    rax, ecx
00000001803B7C21 imul      rax, rax, 0x178
00000001803B7C28 movsxd    rcx, edi
00000001803B7C2B inc       edi
00000001803B7C2D movzx     eax, word ptr [rax + rdx + 0x20]
00000001803B7C32 mov       word ptr [r9 + rcx*2 + 0x20], ax
00000001803B7C38 cmp       edi, r8d
00000001803B7C3B jl        0x1803b7ba0
00000001803B7C41 xor       r8d, r8d
00000001803B7C44 mov       rdx, r9
00000001803B7C47 xor       ecx, ecx
00000001803B7C49 call      0x180b74e90
00000001803B7C4E shr       rsi, 0x20
00000001803B7C52 mov       rdi, rax
00000001803B7C55 shr       r14, 0x20
00000001803B7C59 cmp       byte ptr [rip + 0x192d2f3], 0 # 0x181ce4f53
00000001803B7C60 jne       0x1803b7c75
00000001803B7C62 lea       rcx, [rip + 0x17f4ddf] # 0x181baca48
00000001803B7C69 call      0x18024fef0
00000001803B7C6E mov       byte ptr [rip + 0x192d2de], 1 # 0x181ce4f53
00000001803B7C75 cmp       qword ptr [rbx + 0x38], 0
00000001803B7C7A je        0x1803b7c9a
00000001803B7C7C mov       rax, qword ptr [rip + 0x17f4dc5] # 0x181baca48
00000001803B7C83 mov       r9d, r14d
00000001803B7C86 mov       rcx, qword ptr [rbx + 0x38]
00000001803B7C8A mov       r8d, esi
00000001803B7C8D mov       rdx, rdi
00000001803B7C90 mov       qword ptr [rsp + 0x20], rax
00000001803B7C95 call      0x180a66460
00000001803B7C9A mov       rdi, qword ptr [rbx + 0x48]
00000001803B7C9E lea       rcx, [rbp - 0x79]
00000001803B7CA2 xor       edx, edx
00000001803B7CA4 call      0x181364ca0
00000001803B7CA9 mov       rcx, qword ptr [rip + 0x18050f0] # 0x181bbcda0
00000001803B7CB0 mov       r14, qword ptr [rbx + 0x50]
00000001803B7CB4 movsd     xmm0, qword ptr [rax]
00000001803B7CB8 cmp       dword ptr [rcx + 0xe0], 0
00000001803B7CBF mov       esi, dword ptr [rax + 8]
00000001803B7CC2 movsd     qword ptr [rbp + 0x67], xmm0
00000001803B7CC7 jne       0x1803b7cd3
00000001803B7CC9 call      0x1802501e0
00000001803B7CCE movsd     xmm0, qword ptr [rbp + 0x67]
00000001803B7CD3 xor       r9d, r9d
00000001803B7CD6 movsd     qword ptr [rbp - 0x79], xmm0
00000001803B7CDB mov       r8, r14
00000001803B7CDE mov       dword ptr [rbp - 0x71], esi
00000001803B7CE1 lea       rdx, [rbp - 0x79]
00000001803B7CE5 mov       rcx, rdi
00000001803B7CE8 call      0x1811cc730
00000001803B7CED movsxd    rdi, eax
00000001803B7CF0 cmp       edi, -1
00000001803B7CF3 je        0x1803b7800
00000001803B7CF9 cmp       edi, dword ptr [rbx + 0x60]
00000001803B7CFC je        0x1803b7800
00000001803B7D02 mov       rax, qword ptr [rbx + 0x48]
00000001803B7D06 mov       dword ptr [rbx + 0x60], edi
00000001803B7D09 test      rax, rax
00000001803B7D0C je        0x1803b7dbf
00000001803B7D12 mov       rdx, qword ptr [rax + 0x370]
00000001803B7D19 test      rdx, rdx
00000001803B7D1C je        0x1803b7dbf
00000001803B7D22 mov       rdx, qword ptr [rdx + 0x48]
00000001803B7D26 test      rdx, rdx
00000001803B7D29 je        0x1803b7dbf
00000001803B7D2F cmp       edi, dword ptr [rdx + 0x18]
00000001803B7D32 jae       0x1803b7dc5
00000001803B7D38 lea       rcx, [rdi + rdi*4]
00000001803B7D3C movups    xmm0, xmmword ptr [rdx + rcx*8 + 0x20]
00000001803B7D41 movups    xmmword ptr [rbp - 0x51], xmm0
00000001803B7D45 movups    xmm1, xmmword ptr [rdx + rcx*8 + 0x30]
00000001803B7D4A movups    xmmword ptr [rbp - 0x41], xmm1
00000001803B7D4E movsd     xmm0, qword ptr [rdx + rcx*8 + 0x40]
00000001803B7D54 xor       edx, edx
00000001803B7D56 lea       rcx, [rbp - 0x51]
00000001803B7D5A movsd     qword ptr [rbp - 0x31], xmm0
00000001803B7D5F call      0x18119bc90
00000001803B7D64 xor       edx, edx
00000001803B7D66 lea       rcx, [rbp - 0x51]
00000001803B7D6A mov       rsi, rax
00000001803B7D6D call      0x18119bd30
00000001803B7D72 cmp       byte ptr [rip + 0x192d1db], 0 # 0x181ce4f54
00000001803B7D79 mov       r14, rax
00000001803B7D7C jne       0x1803b7d91
00000001803B7D7E lea       rcx, [rip + 0x17f4e23] # 0x181bacba8
00000001803B7D85 call      0x18024fef0
00000001803B7D8A mov       byte ptr [rip + 0x192d1c3], 1 # 0x181ce4f54
00000001803B7D91 cmp       qword ptr [rbx + 0x40], 0
00000001803B7D96 je        0x1803b7800
00000001803B7D9C mov       rdx, qword ptr [rip + 0x17f4e05] # 0x181bacba8
00000001803B7DA3 mov       r9d, edi
00000001803B7DA6 mov       rcx, qword ptr [rbx + 0x40]
00000001803B7DAA mov       r8, r14
00000001803B7DAD mov       qword ptr [rsp + 0x20], rdx
00000001803B7DB2 mov       rdx, rsi
00000001803B7DB5 call      0x180a660f0
00000001803B7DBA jmp       0x1803b7800
00000001803B7DBF call      0x180250150
00000001803B7DC4 int3      
00000001803B7DC5 call      0x180250140
00000001803B7DCA int3      
00000001803B7DCB int3      
00000001803B7DCC int3      
00000001803B7DCD int3      
00000001803B7DCE int3      
00000001803B7DCF int3      