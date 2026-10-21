0000000180319DB0 mov       rax, rsp
0000000180319DB3 mov       qword ptr [rax + 0x20], r9
0000000180319DB7 mov       dword ptr [rax + 0x18], r8d
0000000180319DBB mov       dword ptr [rax + 0x10], edx
0000000180319DBE mov       qword ptr [rax + 8], rcx
0000000180319DC2 push      rbx
0000000180319DC3 push      rsi
0000000180319DC4 push      rdi
0000000180319DC5 push      r12
0000000180319DC7 push      r13
0000000180319DC9 push      r14
0000000180319DCB push      r15
0000000180319DCD sub       rsp, 0xa0
0000000180319DD4 movaps    xmmword ptr [rax - 0x48], xmm6
0000000180319DD8 movaps    xmmword ptr [rax - 0x58], xmm7
0000000180319DDC mov       r15d, r8d
0000000180319DDF mov       r12d, edx
0000000180319DE2 mov       r13, rcx
0000000180319DE5 cmp       byte ptr [rip + 0x19cadbf], 0 # 0x181ce4bab
0000000180319DEC jne       0x180319e55
0000000180319DEE lea       rcx, [rip + 0x18b0c6b] # 0x181bcaa60
0000000180319DF5 call      0x18024fef0
0000000180319DFA lea       rcx, [rip + 0x18b0caf] # 0x181bcaab0
0000000180319E01 call      0x18024fef0
0000000180319E06 lea       rcx, [rip + 0x18b0cfb] # 0x181bcab08
0000000180319E0D call      0x18024fef0
0000000180319E12 lea       rcx, [rip + 0x18a9cef] # 0x181bc3b08
0000000180319E19 call      0x18024fef0
0000000180319E1E lea       rcx, [rip + 0x18a9d3b] # 0x181bc3b60
0000000180319E25 call      0x18024fef0
0000000180319E2A lea       rcx, [rip + 0x18a9c7f] # 0x181bc3ab0
0000000180319E31 call      0x18024fef0
0000000180319E36 lea       rcx, [rip + 0x18a9dd3] # 0x181bc3c10
0000000180319E3D call      0x18024fef0
0000000180319E42 lea       rcx, [rip + 0x18a813f] # 0x181bc1f88
0000000180319E49 call      0x18024fef0
0000000180319E4E mov       byte ptr [rip + 0x19cad56], 1 # 0x181ce4bab
0000000180319E55 xorps     xmm0, xmm0
0000000180319E58 xor       eax, eax
0000000180319E5A movups    xmmword ptr [rsp + 0x38], xmm0
0000000180319E5F mov       qword ptr [rsp + 0x48], rax
0000000180319E64 mov       r8d, dword ptr [r13 + 0x18]
0000000180319E68 imul      r8d, r12d
0000000180319E6C add       r8d, dword ptr [r13 + 0x18]
0000000180319E70 mov       ecx, dword ptr [r13 + 0x18]
0000000180319E74 imul      ecx, r15d
0000000180319E78 mov       eax, 0x55555556
0000000180319E7D imul      ecx
0000000180319E7F mov       r14d, edx
0000000180319E82 shr       r14d, 0x1f
0000000180319E86 add       r14d, edx
0000000180319E89 add       r14d, r8d
0000000180319E8C mov       eax, 0x66666667
0000000180319E91 imul      r15d
0000000180319E94 sar       edx, 2
0000000180319E97 mov       eax, edx
0000000180319E99 shr       eax, 0x1f
0000000180319E9C add       edx, eax
0000000180319E9E lea       ecx, [rdx + rdx*4]
0000000180319EA1 add       ecx, ecx
0000000180319EA3 mov       eax, r15d
0000000180319EA6 sub       eax, ecx
0000000180319EA8 cmp       eax, 9
0000000180319EAB jne       0x180319eb9
0000000180319EAD mov       ecx, dword ptr [r13 + 0x18]
0000000180319EB1 lea       eax, [r14 + rcx]
0000000180319EB5 lea       r14d, [rax + rcx*2]
0000000180319EB9 xorps     xmm7, xmm7
0000000180319EBC mov       dword ptr [rsp + 0x24], r14d
0000000180319EC1 test      r14d, r14d
0000000180319EC4 jle       0x18031a044
0000000180319ECA mov       rcx, qword ptr [rip + 0x18a80b7] # 0x181bc1f88
0000000180319ED1 call      0x180250100
0000000180319ED6 mov       rsi, rax
0000000180319ED9 mov       rdx, qword ptr [rip + 0x18a9bd0] # 0x181bc3ab0
0000000180319EE0 mov       rcx, rax
0000000180319EE3 call      0x1808053d0
0000000180319EE8 mov       qword ptr [rsp + 0x50], rsi
0000000180319EED movaps    xmm6, xmm7
0000000180319EF0 movss     dword ptr [rsp + 0x20], xmm7
0000000180319EF6 mov       rdx, qword ptr [r13 + 0x10]
0000000180319EFA test      rdx, rdx
0000000180319EFD je        0x18031a07e
0000000180319F03 mov       r8, qword ptr [rip + 0x18a9c56] # 0x181bc3b60
0000000180319F0A lea       rcx, [rsp + 0x60]
0000000180319F0F call      0x1808197f0
0000000180319F14 movups    xmm0, xmmword ptr [rsp + 0x60]
0000000180319F19 movups    xmmword ptr [rsp + 0x38], xmm0
0000000180319F1E movsd     xmm1, qword ptr [rsp + 0x70]
0000000180319F24 movsd     qword ptr [rsp + 0x48], xmm1
0000000180319F2A mov       qword ptr [rsp + 0x28], 0
0000000180319F33 lea       rbx, [rsp + 0x38]
0000000180319F38 mov       qword ptr [rsp + 0x30], rbx
0000000180319F3D nop       dword ptr [rax]
0000000180319F40 mov       rdx, qword ptr [rip + 0x18b0b69] # 0x181bcaab0
0000000180319F47 lea       rcx, [rsp + 0x38]
0000000180319F4C call      0x180669280
0000000180319F51 test      al, al
0000000180319F53 je        0x180319fa6
0000000180319F55 mov       rdi, qword ptr [rsp + 0x48]
0000000180319F5A test      rdi, rdi
0000000180319F5D je        0x18031a072
0000000180319F63 cmp       dword ptr [rdi + 0x18], r14d
0000000180319F67 jg        0x180319f40
0000000180319F69 cmp       dword ptr [rdi + 0x24], r15d
0000000180319F6D jg        0x180319f40
0000000180319F6F cmp       dword ptr [rdi + 0x20], r12d
0000000180319F73 jg        0x180319f40
0000000180319F75 test      rsi, rsi
0000000180319F78 je        0x18031a06d
0000000180319F7E mov       r8, qword ptr [rip + 0x18a9b83] # 0x181bc3b08
0000000180319F85 mov       rdx, rdi
0000000180319F88 mov       rcx, rsi
0000000180319F8B call      0x180002e70
0000000180319F90 test      rdi, rdi
0000000180319F93 je        0x18031a067
0000000180319F99 addss     xmm6, dword ptr [rdi + 0x1c]
0000000180319F9E movss     dword ptr [rsp + 0x20], xmm6
0000000180319FA4 jmp       0x180319f40
0000000180319FA6 mov       rdx, qword ptr [rip + 0x18b0ab3] # 0x181bcaa60
0000000180319FAD mov       rcx, rbx
0000000180319FB0 call      0x180302170
0000000180319FB5 jmp       0x18031a001
0000000180319FB7 mov       rdx, qword ptr [rip + 0x18b0aa2] # 0x181bcaa60
0000000180319FBE mov       rcx, qword ptr [rsp + 0x30]
0000000180319FC3 call      0x180302170
0000000180319FC8 mov       rcx, qword ptr [rsp + 0x28]
0000000180319FCD test      rcx, rcx
0000000180319FD0 jne       0x18031a078
0000000180319FD6 mov       r15d, dword ptr [rsp + 0xf0]
0000000180319FDE mov       r12d, dword ptr [rsp + 0xe8]
0000000180319FE6 mov       r13, qword ptr [rsp + 0xe0]
0000000180319FEE mov       r14d, dword ptr [rsp + 0x24]
0000000180319FF3 mov       rsi, qword ptr [rsp + 0x50]
0000000180319FF8 movss     xmm6, dword ptr [rsp + 0x20]
0000000180319FFE xorps     xmm7, xmm7
000000018031A001 test      rsi, rsi
000000018031A004 je        0x18031a07e
000000018031A006 cmp       dword ptr [rsi + 0x18], 0
000000018031A00A jle       0x18031a044
000000018031A00C xor       r9d, r9d
000000018031A00F movaps    xmm2, xmm6
000000018031A012 mov       rdx, rsi
000000018031A015 mov       rcx, r13
000000018031A018 call      0x18031a490
000000018031A01D mov       rbx, rax
000000018031A020 test      rax, rax
000000018031A023 je        0x18031a044
000000018031A025 xor       r9d, r9d
000000018031A028 mov       r8, rax
000000018031A02B mov       rdx, qword ptr [rsp + 0xf8]
000000018031A033 mov       rcx, r13
000000018031A036 call      0x180319720
000000018031A03B sub       r14d, dword ptr [rbx + 0x18]
000000018031A03F jmp       0x180319ebc
000000018031A044 movaps    xmm6, xmmword ptr [rsp + 0x90]
000000018031A04C movaps    xmm7, xmmword ptr [rsp + 0x80]
000000018031A054 add       rsp, 0xa0
000000018031A05B pop       r15
000000018031A05D pop       r14
000000018031A05F pop       r13
000000018031A061 pop       r12
000000018031A063 pop       rdi
000000018031A064 pop       rsi
000000018031A065 pop       rbx
000000018031A066 ret       
000000018031A067 call      0x180250150
000000018031A06C nop       
000000018031A06D call      0x180250150
000000018031A072 call      0x180250150
000000018031A077 nop       
000000018031A078 call      0x180200e60
000000018031A07D int3      
000000018031A07E call      0x180250150
000000018031A083 int3      
000000018031A084 int3      
000000018031A085 int3      
000000018031A086 int3      
000000018031A087 int3      
000000018031A088 int3      
000000018031A089 int3      
000000018031A08A int3      
000000018031A08B int3      
000000018031A08C int3      
000000018031A08D int3      
000000018031A08E int3      
000000018031A08F int3      