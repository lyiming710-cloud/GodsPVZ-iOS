00000001803B58D0 push      rbx
00000001803B58D2 sub       rsp, 0x20
00000001803B58D6 cmp       byte ptr [rip + 0x192f6a7], 0 # 0x181ce4f84
00000001803B58DD mov       rbx, rcx
00000001803B58E0 jne       0x1803b5955
00000001803B58E2 lea       rcx, [rip + 0x180dc0f] # 0x181bc34f8
00000001803B58E9 call      0x18024fef0
00000001803B58EE lea       rcx, [rip + 0x180dbab] # 0x181bc34a0
00000001803B58F5 call      0x18024fef0
00000001803B58FA lea       rcx, [rip + 0x17eeb37] # 0x181ba4438
00000001803B5901 call      0x18024fef0
00000001803B5906 lea       rcx, [rip + 0x17eea6b] # 0x181ba4378
00000001803B590D call      0x18024fef0
00000001803B5912 lea       rcx, [rip + 0x180b167] # 0x181bc0a80
00000001803B5919 call      0x18024fef0
00000001803B591E lea       rcx, [rip + 0x180b263] # 0x181bc0b88
00000001803B5925 call      0x18024fef0
00000001803B592A lea       rcx, [rip + 0x17f377f] # 0x181ba90b0
00000001803B5931 call      0x18024fef0
00000001803B5936 lea       rcx, [rip + 0x1810053] # 0x181bc5990
00000001803B593D call      0x18024fef0
00000001803B5942 lea       rcx, [rip + 0x180ffef] # 0x181bc5938
00000001803B5949 call      0x18024fef0
00000001803B594E mov       byte ptr [rip + 0x192f62f], 1 # 0x181ce4f84
00000001803B5955 cmp       dword ptr [rbx + 0x20], 0
00000001803B5959 mov       rcx, rbx
00000001803B595C mov       qword ptr [rsp + 0x38], rdi
00000001803B5961 je        0x1803b5990
00000001803B5963 mov       rdx, qword ptr [rip + 0x180db8e] # 0x181bc34f8
00000001803B596A call      0x18042e9f0
00000001803B596F test      rax, rax
00000001803B5972 jne       0x1803b59c3
00000001803B5974 xor       edx, edx
00000001803B5976 mov       rcx, rbx
00000001803B5979 call      0x1813044d0
00000001803B597E test      rax, rax
00000001803B5981 je        0x1803b5ad7
00000001803B5987 mov       rdx, qword ptr [rip + 0x17eeaaa] # 0x181ba4438
00000001803B598E jmp       0x1803b59bb
00000001803B5990 mov       rdx, qword ptr [rip + 0x180db09] # 0x181bc34a0
00000001803B5997 call      0x18042e9f0
00000001803B599C test      rax, rax
00000001803B599F jne       0x1803b59c3
00000001803B59A1 xor       edx, edx
00000001803B59A3 mov       rcx, rbx
00000001803B59A6 call      0x1813044d0
00000001803B59AB test      rax, rax
00000001803B59AE je        0x1803b5ad7
00000001803B59B4 mov       rdx, qword ptr [rip + 0x17ee9bd] # 0x181ba4378
00000001803B59BB mov       rcx, rax
00000001803B59BE call      0x18046d280
00000001803B59C3 mov       rcx, rax
00000001803B59C6 lea       rcx, [rbx + 0x28]
00000001803B59CA mov       qword ptr [rbx + 0x28], rax
00000001803B59CE mov       rdx, rax
00000001803B59D1 call      0x18024f360
00000001803B59D6 mov       rdx, qword ptr [rip + 0x180b1ab] # 0x181bc0b88
00000001803B59DD mov       rcx, qword ptr [rip + 0x180ff54] # 0x181bc5938
00000001803B59E4 mov       rdi, qword ptr [rbx + 0x28]
00000001803B59E8 call      0x1804a6ab0
00000001803B59ED test      rdi, rdi
00000001803B59F0 je        0x1803b5ad7
00000001803B59F6 xor       r8d, r8d
00000001803B59F9 mov       rdx, rax
00000001803B59FC mov       rcx, rdi
00000001803B59FF call      0x1811b9520
00000001803B5A04 mov       rdx, qword ptr [rip + 0x180b075] # 0x181bc0a80
00000001803B5A0B mov       rcx, qword ptr [rip + 0x180ff7e] # 0x181bc5990
00000001803B5A12 mov       rdi, qword ptr [rbx + 0x28]
00000001803B5A16 call      0x1804a6ab0
00000001803B5A1B test      rdi, rdi
00000001803B5A1E je        0x1803b5ad7
00000001803B5A24 mov       r9, qword ptr [rdi]
00000001803B5A27 mov       rdx, rax
00000001803B5A2A mov       rcx, rdi
00000001803B5A2D mov       r8, qword ptr [r9 + 0x580]
00000001803B5A34 call      qword ptr [r9 + 0x578]
00000001803B5A3B mov       rcx, qword ptr [rbx + 0x28]
00000001803B5A3F test      rcx, rcx
00000001803B5A42 je        0x1803b5ad7
00000001803B5A48 movss     xmm1, dword ptr [rip + 0x11f1ff0] # 0x1815a7a40
00000001803B5A50 xor       r8d, r8d
00000001803B5A53 call      0x1811b9410
00000001803B5A58 mov       rcx, qword ptr [rbx + 0x28]
00000001803B5A5C test      rcx, rcx
00000001803B5A5F je        0x1803b5ad7
00000001803B5A61 mov       rax, qword ptr [rcx]
00000001803B5A64 mov       rdx, qword ptr [rip + 0x17f3645] # 0x181ba90b0
00000001803B5A6B mov       r8, qword ptr [rax + 0x560]
00000001803B5A72 call      qword ptr [rax + 0x558]
00000001803B5A78 mov       rcx, qword ptr [rbx + 0x28]
00000001803B5A7C test      rcx, rcx
00000001803B5A7F je        0x1803b5ad7
00000001803B5A81 movss     xmm1, dword ptr [rip + 0x11f259b] # 0x1815a8024
00000001803B5A89 xor       r9d, r9d
00000001803B5A8C movaps    xmm2, xmm1
00000001803B5A8F call      0x1811a4990
00000001803B5A94 mov       rcx, qword ptr [rbx + 0x28]
00000001803B5A98 mov       qword ptr [rsp + 0x30], rax
00000001803B5A9D test      rcx, rcx
00000001803B5AA0 je        0x1803b5ad7
00000001803B5AA2 xor       edx, edx
00000001803B5AA4 call      0x1811b84f0
00000001803B5AA9 movss     xmm1, dword ptr [rsp + 0x34]
00000001803B5AAF test      rax, rax
00000001803B5AB2 je        0x1803b5ad7
00000001803B5AB4 movss     xmm0, dword ptr [rsp + 0x30]
00000001803B5ABA xor       r8d, r8d
00000001803B5ABD unpcklps  xmm0, xmm1
00000001803B5AC0 mov       rcx, rax
00000001803B5AC3 movq      rdx, xmm0
00000001803B5AC8 mov       rdi, qword ptr [rsp + 0x38]
00000001803B5ACD add       rsp, 0x20
00000001803B5AD1 pop       rbx
00000001803B5AD2 jmp       0x181325360
00000001803B5AD7 call      0x180250150
00000001803B5ADC int3      