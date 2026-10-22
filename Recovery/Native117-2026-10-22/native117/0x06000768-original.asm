00000001803BCEC0 mov       qword ptr [rsp + 0x10], rbx
00000001803BCEC5 push      rdi
00000001803BCEC6 sub       rsp, 0x40
00000001803BCECA cmp       byte ptr [rip + 0x1928098], 0 # 0x181ce4f69
00000001803BCED1 mov       rdi, rcx
00000001803BCED4 jne       0x1803bcef5
00000001803BCED6 lea       rcx, [rip + 0x17fbfdb] # 0x181bb8eb8
00000001803BCEDD call      0x18024fef0
00000001803BCEE2 lea       rcx, [rip + 0x1806a7f] # 0x181bc3968
00000001803BCEE9 call      0x18024fef0
00000001803BCEEE mov       byte ptr [rip + 0x1928074], 1 # 0x181ce4f69
00000001803BCEF5 mov       eax, dword ptr [rdi + 0x10]
00000001803BCEF8 mov       rbx, qword ptr [rdi + 0x20]
00000001803BCEFC mov       qword ptr [rsp + 0x50], rsi
00000001803BCF01 movaps    xmmword ptr [rsp + 0x30], xmm6
00000001803BCF06 test      eax, eax
00000001803BCF08 jne       0x1803bcf34
00000001803BCF0A movss     xmm1, dword ptr [rip + 0x11eaafe] # 0x1815a7a10
00000001803BCF12 xor       r8d, r8d
00000001803BCF15 xorps     xmm0, xmm0
00000001803BCF18 mov       dword ptr [rdi + 0x10], 0xffffffff
00000001803BCF1F call      0x18130d350
00000001803BCF24 test      rbx, rbx
00000001803BCF27 je        0x1803bd037
00000001803BCF2D movss     dword ptr [rbx + 0x38], xmm0
00000001803BCF32 jmp       0x1803bcf4d
00000001803BCF34 cmp       eax, 1
00000001803BCF37 jne       0x1803bd020
00000001803BCF3D mov       dword ptr [rdi + 0x10], 0xffffffff
00000001803BCF44 test      rbx, rbx
00000001803BCF47 je        0x1803bd037
00000001803BCF4D mov       rcx, qword ptr [rbx + 0x30]
00000001803BCF51 test      rcx, rcx
00000001803BCF54 je        0x1803bd037
00000001803BCF5A movss     xmm1, dword ptr [rbx + 0x38]
00000001803BCF5F xor       r8d, r8d
00000001803BCF62 call      0x1812de000
00000001803BCF67 mov       rdx, qword ptr [rip + 0x17fbf4a] # 0x181bb8eb8
00000001803BCF6E movaps    xmm6, xmm0
00000001803BCF71 mov       rsi, qword ptr [rbx + 0x28]
00000001803BCF75 cmp       dword ptr [rdx + 0xe0], 0
00000001803BCF7C jne       0x1803bcf8d
00000001803BCF7E mov       rcx, rdx
00000001803BCF81 call      0x1802501e0
00000001803BCF86 mov       rdx, qword ptr [rip + 0x17fbf2b] # 0x181bb8eb8
00000001803BCF8D test      rsi, rsi
00000001803BCF90 je        0x1803bd037
00000001803BCF96 mov       rdx, qword ptr [rdx + 0xb8]
00000001803BCF9D xor       r9d, r9d
00000001803BCFA0 movaps    xmm2, xmm6
00000001803BCFA3 movaps    xmmword ptr [rsp + 0x20], xmm7
00000001803BCFA8 mov       rcx, rsi
00000001803BCFAB mov       edx, dword ptr [rdx + 0x7c]
00000001803BCFAE call      0x1812efc20
00000001803BCFB3 movss     xmm7, dword ptr [rbx + 0x38]
00000001803BCFB8 xor       ecx, ecx
00000001803BCFBA call      0x18132c2f0
00000001803BCFBF movss     xmm1, dword ptr [rip + 0x11eaaed] # 0x1815a7ab4
00000001803BCFC7 movaps    xmm6, xmm0
00000001803BCFCA movss     xmm0, dword ptr [rip + 0x11eaada] # 0x1815a7aac
00000001803BCFD2 xor       r8d, r8d
00000001803BCFD5 call      0x18130d350
00000001803BCFDA mulss     xmm0, xmm6
00000001803BCFDE addss     xmm0, xmm7
00000001803BCFE2 movss     dword ptr [rbx + 0x38], xmm0
00000001803BCFE7 mov       rcx, qword ptr [rip + 0x180697a] # 0x181bc3968
00000001803BCFEE call      0x180250100
00000001803BCFF3 xor       edx, edx
00000001803BCFF5 mov       rcx, rax
00000001803BCFF8 mov       rbx, rax
00000001803BCFFB call      0x18030f300
00000001803BD000 lea       rcx, [rdi + 0x18]
00000001803BD004 mov       qword ptr [rdi + 0x18], rbx
00000001803BD008 mov       rdx, rbx
00000001803BD00B call      0x18024f360
00000001803BD010 movaps    xmm7, xmmword ptr [rsp + 0x20]
00000001803BD015 mov       al, 1
00000001803BD017 mov       dword ptr [rdi + 0x10], 1
00000001803BD01E jmp       0x1803bd022
00000001803BD020 xor       al, al
00000001803BD022 movaps    xmm6, xmmword ptr [rsp + 0x30]
00000001803BD027 mov       rsi, qword ptr [rsp + 0x50]
00000001803BD02C mov       rbx, qword ptr [rsp + 0x58]
00000001803BD031 add       rsp, 0x40
00000001803BD035 pop       rdi
00000001803BD036 ret       
00000001803BD037 call      0x180250150
00000001803BD03C int3      
00000001803BD03D int3      
00000001803BD03E int3      
00000001803BD03F int3      