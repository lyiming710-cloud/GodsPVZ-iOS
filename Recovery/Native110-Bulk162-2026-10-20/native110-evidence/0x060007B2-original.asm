00000001803B5B60 push      rbx
00000001803B5B62 sub       rsp, 0x30
00000001803B5B66 cmp       byte ptr [rip + 0x192f419], 0 # 0x181ce4f86
00000001803B5B6D mov       rbx, rcx
00000001803B5B70 jne       0x1803b5bd9
00000001803B5B72 lea       rcx, [rip + 0x1819697] # 0x181bcf210
00000001803B5B79 call      0x18024fef0
00000001803B5B7E lea       rcx, [rip + 0x17ee7f3] # 0x181ba4378
00000001803B5B85 call      0x18024fef0
00000001803B5B8A lea       rcx, [rip + 0x17ec127] # 0x181ba1cb8
00000001803B5B91 call      0x18024fef0
00000001803B5B96 lea       rcx, [rip + 0x180aee3] # 0x181bc0a80
00000001803B5B9D call      0x18024fef0
00000001803B5BA2 lea       rcx, [rip + 0x180afdf] # 0x181bc0b88
00000001803B5BA9 call      0x18024fef0
00000001803B5BAE lea       rcx, [rip + 0x180ff3b] # 0x181bc5af0
00000001803B5BB5 call      0x18024fef0
00000001803B5BBA lea       rcx, [rip + 0x1810717] # 0x181bc62d8
00000001803B5BC1 call      0x18024fef0
00000001803B5BC6 lea       rcx, [rip + 0x180fe1b] # 0x181bc59e8
00000001803B5BCD call      0x18024fef0
00000001803B5BD2 mov       byte ptr [rip + 0x192f3ad], 1 # 0x181ce4f86
00000001803B5BD9 xor       edx, edx
00000001803B5BDB mov       rcx, rbx
00000001803B5BDE call      0x181302c30
00000001803B5BE3 test      al, al
00000001803B5BE5 je        0x1803b5db9
00000001803B5BEB mov       qword ptr [rsp + 0x48], rdi
00000001803B5BF0 xor       ecx, ecx
00000001803B5BF2 call      0x1812e27c0
00000001803B5BF7 lea       rcx, [rbx + 0x48]
00000001803B5BFB mov       qword ptr [rbx + 0x48], rax
00000001803B5BFF mov       rdx, rax
00000001803B5C02 call      0x18024f360
00000001803B5C07 mov       rcx, qword ptr [rip + 0x1819602] # 0x181bcf210
00000001803B5C0E cmp       dword ptr [rcx + 0xe0], 0
00000001803B5C15 jne       0x1803b5c1c
00000001803B5C17 call      0x1802501e0
00000001803B5C1C xor       edx, edx
00000001803B5C1E mov       qword ptr [rsp + 0x40], rsi
00000001803B5C23 mov       ecx, 0x270f
00000001803B5C28 call      0x1812df480
00000001803B5C2D mov       rcx, qword ptr [rip + 0x17ec084] # 0x181ba1cb8
00000001803B5C34 call      0x180250100
00000001803B5C39 mov       rdx, qword ptr [rip + 0x1810698] # 0x181bc62d8
00000001803B5C40 xor       r8d, r8d
00000001803B5C43 mov       rcx, rax
00000001803B5C46 mov       rdi, rax
00000001803B5C49 call      0x18131bcf0
00000001803B5C4E test      rdi, rdi
00000001803B5C51 je        0x1803b5dbf
00000001803B5C57 mov       rdx, qword ptr [rip + 0x17ee71a] # 0x181ba4378
00000001803B5C5E mov       rcx, rdi
00000001803B5C61 call      0x18046d280
00000001803B5C66 lea       rcx, [rbx + 0x38]
00000001803B5C6A mov       qword ptr [rbx + 0x38], rax
00000001803B5C6E mov       rdx, rax
00000001803B5C71 call      0x18024f360
00000001803B5C76 mov       rdx, qword ptr [rip + 0x180af0b] # 0x181bc0b88
00000001803B5C7D mov       rcx, qword ptr [rip + 0x180fd64] # 0x181bc59e8
00000001803B5C84 mov       rsi, qword ptr [rbx + 0x38]
00000001803B5C88 call      0x1804a6ab0
00000001803B5C8D test      rsi, rsi
00000001803B5C90 je        0x1803b5dbf
00000001803B5C96 xor       r8d, r8d
00000001803B5C99 mov       rdx, rax
00000001803B5C9C mov       rcx, rsi
00000001803B5C9F call      0x1811b9520
00000001803B5CA4 mov       rdx, qword ptr [rip + 0x180add5] # 0x181bc0a80
00000001803B5CAB mov       rcx, qword ptr [rip + 0x180fe3e] # 0x181bc5af0
00000001803B5CB2 mov       rsi, qword ptr [rbx + 0x38]
00000001803B5CB6 call      0x1804a6ab0
00000001803B5CBB test      rsi, rsi
00000001803B5CBE je        0x1803b5dbf
00000001803B5CC4 mov       r9, qword ptr [rsi]
00000001803B5CC7 mov       rdx, rax
00000001803B5CCA mov       rcx, rsi
00000001803B5CCD mov       r8, qword ptr [r9 + 0x580]
00000001803B5CD4 call      qword ptr [r9 + 0x578]
00000001803B5CDB xor       edx, edx
00000001803B5CDD mov       rcx, rdi
00000001803B5CE0 call      0x18131c070
00000001803B5CE5 lea       rcx, [rbx + 0x40]
00000001803B5CE9 mov       qword ptr [rbx + 0x40], rax
00000001803B5CED mov       rdx, rax
00000001803B5CF0 call      0x18024f360
00000001803B5CF5 mov       rcx, qword ptr [rbx + 0x48]
00000001803B5CF9 mov       rdi, qword ptr [rbx + 0x40]
00000001803B5CFD test      rcx, rcx
00000001803B5D00 je        0x1803b5dbf
00000001803B5D06 xor       edx, edx
00000001803B5D08 call      0x181304510
00000001803B5D0D test      rdi, rdi
00000001803B5D10 je        0x1803b5dbf
00000001803B5D16 xor       r8d, r8d
00000001803B5D19 mov       rdx, rax
00000001803B5D1C mov       rcx, rdi
00000001803B5D1F call      0x18132e8b0
00000001803B5D24 cmp       byte ptr [rip + 0x192f284], 0 # 0x181ce4faf
00000001803B5D2B mov       rdi, qword ptr [rbx + 0x40]
00000001803B5D2F jne       0x1803b5d44
00000001803B5D31 lea       rcx, [rip + 0x17fe128] # 0x181bb3e60
00000001803B5D38 call      0x18024fef0
00000001803B5D3D mov       byte ptr [rip + 0x192f26b], 1 # 0x181ce4faf
00000001803B5D44 mov       rax, qword ptr [rip + 0x17fe115] # 0x181bb3e60
00000001803B5D4B mov       rcx, qword ptr [rax + 0xb8]
00000001803B5D52 test      rdi, rdi
00000001803B5D55 je        0x1803b5dbf
00000001803B5D57 movups    xmm0, xmmword ptr [rcx]
00000001803B5D5A xor       r8d, r8d
00000001803B5D5D lea       rdx, [rsp + 0x20]
00000001803B5D62 mov       rcx, rdi
00000001803B5D65 movaps    xmmword ptr [rsp + 0x20], xmm0
00000001803B5D6A call      0x18132f9b0
00000001803B5D6F mov       rcx, qword ptr [rbx + 0x38]
00000001803B5D73 test      rcx, rcx
00000001803B5D76 je        0x1803b5dbf
00000001803B5D78 xor       r8d, r8d
00000001803B5D7B xor       edx, edx
00000001803B5D7D call      0x1811b9000
00000001803B5D82 mov       rcx, qword ptr [rbx + 0x38]
00000001803B5D86 test      rcx, rcx
00000001803B5D89 je        0x1803b5dbf
00000001803B5D8B movss     xmm1, dword ptr [rip + 0x11f1e5d] # 0x1815a7bf0
00000001803B5D93 xor       r8d, r8d
00000001803B5D96 call      0x1811b9410
00000001803B5D9B mov       edx, dword ptr [rbx + 0x2c]
00000001803B5D9E xor       r8d, r8d
00000001803B5DA1 mov       rcx, rbx
00000001803B5DA4 call      0x1803b5dd0
00000001803B5DA9 mov       eax, dword ptr [rbx + 0x2c]
00000001803B5DAC mov       rsi, qword ptr [rsp + 0x40]
00000001803B5DB1 mov       rdi, qword ptr [rsp + 0x48]
00000001803B5DB6 mov       dword ptr [rbx + 0x50], eax
00000001803B5DB9 add       rsp, 0x30
00000001803B5DBD pop       rbx
00000001803B5DBE ret       
00000001803B5DBF call      0x180250150
00000001803B5DC4 int3      