000000018035BEE0 push      rbx
000000018035BEE2 sub       rsp, 0x50
000000018035BEE6 cmp       byte ptr [rip + 0x1988e07], 0 # 0x181ce4cf4
000000018035BEED mov       rbx, rcx
000000018035BEF0 jne       0x18035bf05
000000018035BEF2 lea       rcx, [rip + 0x186179f] # 0x181bbd698
000000018035BEF9 call      0x18024fef0
000000018035BEFE mov       byte ptr [rip + 0x1988def], 1 # 0x181ce4cf4
000000018035BF05 cmp       byte ptr [rip + 0x1988de5], 0 # 0x181ce4cf1
000000018035BF0C jne       0x18035bf45
000000018035BF0E lea       rcx, [rip + 0x1867883] # 0x181bc3798
000000018035BF15 call      0x18024fef0
000000018035BF1A lea       rcx, [rip + 0x18678cf] # 0x181bc37f0
000000018035BF21 call      0x18024fef0
000000018035BF26 lea       rcx, [rip + 0x186791b] # 0x181bc3848
000000018035BF2D call      0x18024fef0
000000018035BF32 lea       rcx, [rip + 0x18545c7] # 0x181bb0500
000000018035BF39 call      0x18024fef0
000000018035BF3E mov       byte ptr [rip + 0x1988dac], 1 # 0x181ce4cf1
000000018035BF45 mov       rax, qword ptr [rbx + 0x130]
000000018035BF4C mov       qword ptr [rsp + 0x60], rsi
000000018035BF51 mov       qword ptr [rsp + 0x68], rdi
000000018035BF56 movaps    xmmword ptr [rsp + 0x40], xmm6
000000018035BF5B movaps    xmmword ptr [rsp + 0x30], xmm7
000000018035BF60 test      rax, rax
000000018035BF63 je        0x18035c232
000000018035BF69 cmp       dword ptr [rax + 0x18], 0
000000018035BF6D jle       0x18035c025
000000018035BF73 mov       esi, dword ptr [rax + 0x18]
000000018035BF76 sub       esi, 1
000000018035BF79 js        0x18035c025
000000018035BF7F nop       
000000018035BF80 mov       rax, qword ptr [rbx + 0x130]
000000018035BF87 test      rax, rax
000000018035BF8A je        0x18035c232
000000018035BF90 cmp       esi, dword ptr [rax + 0x18]
000000018035BF93 jge       0x18035c025
000000018035BF99 mov       r8, qword ptr [rip + 0x18678a8] # 0x181bc3848
000000018035BFA0 mov       edx, esi
000000018035BFA2 mov       rcx, rax
000000018035BFA5 call      0x180825710
000000018035BFAA mov       rcx, qword ptr [rip + 0x185454f] # 0x181bb0500
000000018035BFB1 mov       rdi, rax
000000018035BFB4 cmp       dword ptr [rcx + 0xe0], 0
000000018035BFBB jne       0x18035bfc2
000000018035BFBD call      0x1802501e0
000000018035BFC2 xor       r8d, r8d
000000018035BFC5 xor       edx, edx
000000018035BFC7 mov       rcx, rdi
000000018035BFCA call      0x18131f760
000000018035BFCF test      al, al
000000018035BFD1 jne       0x18035bffd
000000018035BFD3 test      rdi, rdi
000000018035BFD6 je        0x18035c232
000000018035BFDC xor       edx, edx
000000018035BFDE mov       rcx, rdi
000000018035BFE1 call      0x1803659e0
000000018035BFE6 test      al, al
000000018035BFE8 je        0x18035c01c
000000018035BFEA xor       r9d, r9d
000000018035BFED xor       r8d, r8d
000000018035BFF0 mov       rdx, rdi
000000018035BFF3 mov       rcx, rbx
000000018035BFF6 call      0x18034df60
000000018035BFFB jmp       0x18035c01c
000000018035BFFD mov       rcx, qword ptr [rbx + 0x130]
000000018035C004 test      rcx, rcx
000000018035C007 je        0x18035c232
000000018035C00D mov       r8, qword ptr [rip + 0x1867784] # 0x181bc3798
000000018035C014 mov       rdx, rdi
000000018035C017 call      0x18084e290
000000018035C01C sub       esi, 1
000000018035C01F jns       0x18035bf80
000000018035C025 cmp       dword ptr [rbx + 0x7c], 0
000000018035C029 jne       0x18035c035
000000018035C02B xor       edx, edx
000000018035C02D mov       rcx, rbx
000000018035C030 call      0x180353650
000000018035C035 cmp       dword ptr [rbx + 0x7c], 3
000000018035C039 jne       0x18035c045
000000018035C03B xor       edx, edx
000000018035C03D mov       rcx, rbx
000000018035C040 call      0x180352b10
000000018035C045 cmp       dword ptr [rbx + 0x7c], 4
000000018035C049 jne       0x18035c055
000000018035C04B xor       edx, edx
000000018035C04D mov       rcx, rbx
000000018035C050 call      0x180354010
000000018035C055 xor       esi, esi
000000018035C057 cmp       dword ptr [rbx + 0x7c], 5
000000018035C05B jne       0x18035c117
000000018035C061 cmp       dword ptr [rbx + 0x88], esi
000000018035C067 jle       0x18035c117
000000018035C06D cmp       dword ptr [rbx + 0x100], 1
000000018035C074 jle       0x18035c117
000000018035C07A cmp       byte ptr [rip + 0x1988c97], sil # 0x181ce4d18
000000018035C081 jne       0x18035c096
000000018035C083 lea       rcx, [rip + 0x1855f3e] # 0x181bb1fc8
000000018035C08A call      0x18024fef0
000000018035C08F mov       byte ptr [rip + 0x1988c82], 1 # 0x181ce4d18
000000018035C096 cmp       dword ptr [rbx + 0x100], 1
000000018035C09D jle       0x18035c117
000000018035C09F movss     xmm7, dword ptr [rbx + 0xd8]
000000018035C0A7 xor       ecx, ecx
000000018035C0A9 movss     xmm6, dword ptr [rbx + 0xc0]
000000018035C0B1 call      0x18132c2f0
000000018035C0B6 mulss     xmm0, xmm6
000000018035C0BA addss     xmm0, xmm7
000000018035C0BE comiss    xmm0, dword ptr [rbx + 0xdc]
000000018035C0C5 movss     dword ptr [rbx + 0xd8], xmm0
000000018035C0CD jb        0x18035c117
000000018035C0CF mov       rdx, qword ptr [rip + 0x1855ef2] # 0x181bb1fc8
000000018035C0D6 mov       rdi, qword ptr [rbx + 0x168]
000000018035C0DD cmp       dword ptr [rdx + 0xe0], esi
000000018035C0E3 jne       0x18035c0f4
000000018035C0E5 mov       rcx, rdx
000000018035C0E8 call      0x1802501e0
000000018035C0ED mov       rdx, qword ptr [rip + 0x1855ed4] # 0x181bb1fc8
000000018035C0F4 test      rdi, rdi
000000018035C0F7 je        0x18035c232
000000018035C0FD mov       rdx, qword ptr [rdx + 0xb8]
000000018035C104 xor       r8d, r8d
000000018035C107 mov       rcx, rdi
000000018035C10A mov       edx, dword ptr [rdx]
000000018035C10C call      0x1812dbbc0
000000018035C111 mov       dword ptr [rbx + 0xd8], esi
000000018035C117 cmp       dword ptr [rbx + 0x7c], 0x31
000000018035C11B jne       0x18035c127
000000018035C11D xor       edx, edx
000000018035C11F mov       rcx, rbx
000000018035C122 call      0x180355410
000000018035C127 xor       edx, edx
000000018035C129 mov       rcx, rbx
000000018035C12C call      0x18035c340
000000018035C131 xor       edx, edx
000000018035C133 mov       rcx, rbx
000000018035C136 call      0x18035b4b0
000000018035C13B mov       rcx, qword ptr [rbx + 0x220]
000000018035C142 test      rcx, rcx
000000018035C145 je        0x18035c232
000000018035C14B mov       r8, qword ptr [rip + 0x1861546] # 0x181bbd698
000000018035C152 mov       rdx, rbx
000000018035C155 call      0x180429970
000000018035C15A cmp       byte ptr [rip + 0x1988b94], sil # 0x181ce4cf5
000000018035C161 jne       0x18035c176
000000018035C163 lea       rcx, [rip + 0x1841ffe] # 0x181b9e168
000000018035C16A call      0x18024fef0
000000018035C16F mov       byte ptr [rip + 0x1988b7f], 1 # 0x181ce4cf5
000000018035C176 mov       rax, qword ptr [rbx + 0x1e8]
000000018035C17D test      rax, rax
000000018035C180 je        0x18035c232
000000018035C186 cmp       qword ptr [rax + 0x28], rsi
000000018035C18A je        0x18035c218
000000018035C190 mov       rax, qword ptr [rax + 0x30]
000000018035C194 test      rax, rax
000000018035C197 je        0x18035c218
000000018035C199 mov       eax, dword ptr [rax + 0x18]
000000018035C19C xorps     xmm7, xmm7
000000018035C19F cmp       eax, 1
000000018035C1A2 jne       0x18035c1ae
000000018035C1A4 movss     xmm7, dword ptr [rip + 0x124baac] # 0x1815a7c58
000000018035C1AC jmp       0x18035c1ca
000000018035C1AE cmp       eax, 8
000000018035C1B1 jne       0x18035c1bd
000000018035C1B3 movss     xmm7, dword ptr [rip + 0x124bab9] # 0x1815a7c74
000000018035C1BB jmp       0x18035c1ca
000000018035C1BD cmp       eax, 0xa
000000018035C1C0 jne       0x18035c1ca
000000018035C1C2 movss     xmm7, dword ptr [rip + 0x124baae] # 0x1815a7c78
000000018035C1CA xor       ecx, ecx
000000018035C1CC call      0x18132c2f0
000000018035C1D1 mov       rcx, qword ptr [rip + 0x1841f90] # 0x181b9e168
000000018035C1D8 movaps    xmm6, xmm0
000000018035C1DB call      0x180250100
000000018035C1E0 mulss     xmm6, xmm7
000000018035C1E4 xor       r9d, r9d
000000018035C1E7 mov       qword ptr [rsp + 0x28], rsi
000000018035C1EC xor       edx, edx
000000018035C1EE mov       rcx, rax
000000018035C1F1 mov       byte ptr [rsp + 0x20], 1
000000018035C1F6 mov       rdi, rax
000000018035C1F9 movaps    xmm2, xmm6
000000018035C1FC call      0x180316940
000000018035C201 mov       rcx, qword ptr [rbx + 0xf8]
000000018035C208 test      rcx, rcx
000000018035C20B je        0x18035c232
000000018035C20D xor       r8d, r8d
000000018035C210 mov       rdx, rdi
000000018035C213 call      0x180315390
000000018035C218 movaps    xmm7, xmmword ptr [rsp + 0x30]
000000018035C21D movaps    xmm6, xmmword ptr [rsp + 0x40]
000000018035C222 mov       rdi, qword ptr [rsp + 0x68]
000000018035C227 mov       rsi, qword ptr [rsp + 0x60]
000000018035C22C add       rsp, 0x50
000000018035C230 pop       rbx
000000018035C231 ret       
000000018035C232 call      0x180250150
000000018035C237 int3      
000000018035C238 int3      
000000018035C239 int3      
000000018035C23A int3      
000000018035C23B int3      
000000018035C23C int3      
000000018035C23D int3      
000000018035C23E int3      
000000018035C23F int3      