000000018034FBE0 mov       qword ptr [rsp + 0x10], rbx
000000018034FBE5 mov       qword ptr [rsp + 0x18], rsi
000000018034FBEA mov       qword ptr [rsp + 8], rcx
000000018034FBEF push      rdi
000000018034FBF0 push      r12
000000018034FBF2 push      r13
000000018034FBF4 push      r14
000000018034FBF6 push      r15
000000018034FBF8 sub       rsp, 0x70
000000018034FBFC movaps    xmmword ptr [rsp + 0x60], xmm6
000000018034FC01 mov       r13, r8
000000018034FC04 mov       r12d, edx
000000018034FC07 mov       rdi, rcx
000000018034FC0A cmp       byte ptr [rip + 0x19950ea], 0 # 0x181ce4cfb
000000018034FC11 jne       0x18034fca2
000000018034FC17 lea       rcx, [rip + 0x18685f2] # 0x181bb8210
000000018034FC1E call      0x18024fef0
000000018034FC23 lea       rcx, [rip + 0x187ac3e] # 0x181bca868
000000018034FC2A call      0x18024fef0
000000018034FC2F lea       rcx, [rip + 0x187ac82] # 0x181bca8b8
000000018034FC36 call      0x18024fef0
000000018034FC3B lea       rcx, [rip + 0x187acce] # 0x181bca910
000000018034FC42 call      0x18024fef0
000000018034FC47 lea       rcx, [rip + 0x1873992] # 0x181bc35e0
000000018034FC4E call      0x18024fef0
000000018034FC53 lea       rcx, [rip + 0x1873a8e] # 0x181bc36e8
000000018034FC5A call      0x18024fef0
000000018034FC5F lea       rcx, [rip + 0x1873922] # 0x181bc3588
000000018034FC66 call      0x18024fef0
000000018034FC6B lea       rcx, [rip + 0x1873b7e] # 0x181bc37f0
000000018034FC72 call      0x18024fef0
000000018034FC77 lea       rcx, [rip + 0x1873bca] # 0x181bc3848
000000018034FC7E call      0x18024fef0
000000018034FC83 lea       rcx, [rip + 0x1873c16] # 0x181bc38a0
000000018034FC8A call      0x18024fef0
000000018034FC8F lea       rcx, [rip + 0x1872242] # 0x181bc1ed8
000000018034FC96 call      0x18024fef0
000000018034FC9B mov       byte ptr [rip + 0x1995059], 1 # 0x181ce4cfb
000000018034FCA2 mov       rcx, qword ptr [rip + 0x187222f] # 0x181bc1ed8
000000018034FCA9 call      0x180250100
000000018034FCAE mov       r14, rax
000000018034FCB1 mov       rdx, qword ptr [rip + 0x18738d0] # 0x181bc3588
000000018034FCB8 mov       rcx, rax
000000018034FCBB call      0x1808053d0
000000018034FCC0 mov       qword ptr [rsp + 0x20], r14
000000018034FCC5 mov       rax, qword ptr [rdi + 0x1e8]
000000018034FCCC test      rax, rax
000000018034FCCF je        0x18034fe96
000000018034FCD5 mov       rax, qword ptr [rax + 0xf0]
000000018034FCDC test      rax, rax
000000018034FCDF je        0x18034fe96
000000018034FCE5 mov       rdx, qword ptr [rax + 0x38]
000000018034FCE9 test      rdx, rdx
000000018034FCEC je        0x18034fe96
000000018034FCF2 mov       r8, qword ptr [rip + 0x18739ef] # 0x181bc36e8
000000018034FCF9 lea       rcx, [rsp + 0x30]
000000018034FCFE call      0x1808197f0
000000018034FD03 movups    xmm0, xmmword ptr [rsp + 0x30]
000000018034FD08 movups    xmmword ptr [rsp + 0x48], xmm0
000000018034FD0D movsd     xmm1, qword ptr [rsp + 0x40]
000000018034FD13 movsd     qword ptr [rsp + 0x58], xmm1
000000018034FD19 mov       qword ptr [rsp + 0x30], 0
000000018034FD22 lea       rbx, [rsp + 0x48]
000000018034FD27 mov       qword ptr [rsp + 0x38], rbx
000000018034FD2C jmp       0x18034fd38
000000018034FD2E nop       
000000018034FD30 mov       rdi, qword ptr [rsp + 0xa0]
000000018034FD38 mov       rdx, qword ptr [rip + 0x187ab79] # 0x181bca8b8
000000018034FD3F lea       rcx, [rsp + 0x48]
000000018034FD44 call      0x180669280
000000018034FD49 test      al, al
000000018034FD4B je        0x18034fe43
000000018034FD51 mov       rsi, qword ptr [rsp + 0x58]
000000018034FD56 test      rsi, rsi
000000018034FD59 je        0x18034feb6
000000018034FD5F xor       edx, edx
000000018034FD61 mov       rcx, rsi
000000018034FD64 call      0x18035de80
000000018034FD69 test      al, al
000000018034FD6B je        0x18034fd38
000000018034FD6D test      rsi, rsi
000000018034FD70 je        0x18034feb1
000000018034FD76 xor       edx, edx
000000018034FD78 mov       rcx, rsi
000000018034FD7B call      0x1803659e0
000000018034FD80 test      al, al
000000018034FD82 jne       0x18034fd38
000000018034FD84 mov       eax, dword ptr [rsi + 0xfc]
000000018034FD8A cmp       eax, dword ptr [rdi + 0xbc]
000000018034FD90 je        0x18034fd38
000000018034FD92 test      r13, r13
000000018034FD95 je        0x18034feac
000000018034FD9B mov       r8, qword ptr [rip + 0x186846e] # 0x181bb8210
000000018034FDA2 mov       rdx, rsi
000000018034FDA5 mov       rcx, r13
000000018034FDA8 call      0x180426dd0
000000018034FDAD test      al, al
000000018034FDAF je        0x18034fd38
000000018034FDB1 test      r14, r14
000000018034FDB4 je        0x18034fea7
000000018034FDBA cmp       dword ptr [r14 + 0x18], r12d
000000018034FDBE jl        0x18034fe2c
000000018034FDC0 xor       edi, edi
000000018034FDC2 cmp       edi, r12d
000000018034FDC5 jge       0x18034fd30
000000018034FDCB test      rsi, rsi
000000018034FDCE je        0x18034fea2
000000018034FDD4 movss     xmm6, dword ptr [rsi + 0x30]
000000018034FDD9 mov       r8, qword ptr [rip + 0x1873a68] # 0x181bc3848
000000018034FDE0 mov       edx, edi
000000018034FDE2 mov       rcx, r14
000000018034FDE5 call      0x180825710
000000018034FDEA test      rax, rax
000000018034FDED je        0x18034fe9c
000000018034FDF3 movss     xmm0, dword ptr [rax + 0x30]
000000018034FDF8 comiss    xmm0, xmm6
000000018034FDFB jbe       0x18034fe28
000000018034FDFD mov       r8, qword ptr [rip + 0x1873a44] # 0x181bc3848
000000018034FE04 mov       edx, edi
000000018034FE06 mov       rcx, r14
000000018034FE09 call      0x180825710
000000018034FE0E mov       r15, rax
000000018034FE11 mov       r9, qword ptr [rip + 0x1873a88] # 0x181bc38a0
000000018034FE18 mov       r8, rsi
000000018034FE1B mov       edx, edi
000000018034FE1D mov       rcx, r14
000000018034FE20 call      0x180855ab0
000000018034FE25 mov       rsi, r15
000000018034FE28 inc       edi
000000018034FE2A jmp       0x18034fdc2
000000018034FE2C mov       r8, qword ptr [rip + 0x18737ad] # 0x181bc35e0
000000018034FE33 mov       rdx, rsi
000000018034FE36 mov       rcx, r14
000000018034FE39 call      0x180002e70
000000018034FE3E jmp       0x18034fd38
000000018034FE43 mov       rdx, qword ptr [rip + 0x187aa1e] # 0x181bca868
000000018034FE4A mov       rcx, rbx
000000018034FE4D call      0x180302170
000000018034FE52 jmp       0x18034fe74
000000018034FE54 mov       rdx, qword ptr [rip + 0x187aa0d] # 0x181bca868
000000018034FE5B mov       rcx, qword ptr [rsp + 0x38]
000000018034FE60 call      0x180302170
000000018034FE65 mov       rcx, qword ptr [rsp + 0x30]
000000018034FE6A test      rcx, rcx
000000018034FE6D jne       0x18034febc
000000018034FE6F mov       r14, qword ptr [rsp + 0x20]
000000018034FE74 mov       rax, r14
000000018034FE77 lea       r11, [rsp + 0x70]
000000018034FE7C mov       rbx, qword ptr [r11 + 0x38]
000000018034FE80 mov       rsi, qword ptr [r11 + 0x40]
000000018034FE84 movaps    xmm6, xmmword ptr [rsp + 0x60]
000000018034FE89 mov       rsp, r11
000000018034FE8C pop       r15
000000018034FE8E pop       r14
000000018034FE90 pop       r13
000000018034FE92 pop       r12
000000018034FE94 pop       rdi
000000018034FE95 ret       
000000018034FE96 call      0x180250150
000000018034FE9B nop       
000000018034FE9C call      0x180250150
000000018034FEA1 nop       
000000018034FEA2 call      0x180250150
000000018034FEA7 call      0x180250150
000000018034FEAC call      0x180250150
000000018034FEB1 call      0x180250150
000000018034FEB6 call      0x180250150
000000018034FEBB nop       
000000018034FEBC call      0x180200e60
000000018034FEC1 int3      
000000018034FEC2 int3      
000000018034FEC3 int3      
000000018034FEC4 int3      
000000018034FEC5 int3      
000000018034FEC6 int3      
000000018034FEC7 int3      
000000018034FEC8 int3      
000000018034FEC9 int3      
000000018034FECA int3      
000000018034FECB int3      
000000018034FECC int3      
000000018034FECD int3      
000000018034FECE int3      
000000018034FECF int3      