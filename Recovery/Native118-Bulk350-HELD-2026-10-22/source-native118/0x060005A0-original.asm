0000000180391F10 push      rbx
0000000180391F12 push      rdi
0000000180391F13 sub       rsp, 0x38
0000000180391F17 cmp       byte ptr [rip + 0x1952f41], 0 # 0x181ce4e5f
0000000180391F1E mov       rbx, rcx
0000000180391F21 movsxd    rdi, edx
0000000180391F24 jne       0x180391ff1
0000000180391F2A lea       rcx, [rip + 0x18326ef] # 0x181bc4620
0000000180391F31 call      0x18024fef0
0000000180391F36 lea       rcx, [rip + 0x18130ab] # 0x181ba4fe8
0000000180391F3D call      0x18024fef0
0000000180391F42 lea       rcx, [rip + 0x181417f] # 0x181ba60c8
0000000180391F49 call      0x18024fef0
0000000180391F4E lea       rcx, [rip + 0x1811e33] # 0x181ba3d88
0000000180391F55 call      0x18024fef0
0000000180391F5A lea       rcx, [rip + 0x180e34f] # 0x181ba02b0
0000000180391F61 call      0x18024fef0
0000000180391F66 lea       rcx, [rip + 0x181523b] # 0x181ba71a8
0000000180391F6D call      0x18024fef0
0000000180391F72 lea       rcx, [rip + 0x1810077] # 0x181ba1ff0
0000000180391F79 call      0x18024fef0
0000000180391F7E lea       rcx, [rip + 0x1815043] # 0x181ba6fc8
0000000180391F85 call      0x18024fef0
0000000180391F8A lea       rcx, [rip + 0x180f3ff] # 0x181ba1390
0000000180391F91 call      0x18024fef0
0000000180391F96 lea       rcx, [rip + 0x1814f0b] # 0x181ba6ea8
0000000180391F9D call      0x18024fef0
0000000180391FA2 lea       rcx, [rip + 0x1811e9f] # 0x181ba3e48
0000000180391FA9 call      0x18024fef0
0000000180391FAE lea       rcx, [rip + 0x1815493] # 0x181ba7448
0000000180391FB5 call      0x18024fef0
0000000180391FBA lea       rcx, [rip + 0x1813bc7] # 0x181ba5b88
0000000180391FC1 call      0x18024fef0
0000000180391FC6 lea       rcx, [rip + 0x183d0bb] # 0x181bcf088
0000000180391FCD call      0x18024fef0
0000000180391FD2 lea       rcx, [rip + 0x181492f] # 0x181ba6908
0000000180391FD9 call      0x18024fef0
0000000180391FDE lea       rcx, [rip + 0x1813723] # 0x181ba5708
0000000180391FE5 call      0x18024fef0
0000000180391FEA mov       byte ptr [rip + 0x1952e6e], 1 # 0x181ce4e5f
0000000180391FF1 mov       rax, qword ptr [rbx + 0x28]
0000000180391FF5 mov       qword ptr [rsp + 0x58], rbp
0000000180391FFA mov       qword ptr [rsp + 0x30], rsi
0000000180391FFF test      rax, rax
0000000180392002 je        0x1803921f6
0000000180392008 movsxd    rcx, dword ptr [rax + 0x28]
000000018039200C lea       rbp, [rip - 0x392013] # 0x180000000
0000000180392013 cmp       ecx, 6
0000000180392016 ja        0x180392063
0000000180392018 mov       ecx, dword ptr [rbp + rcx*4 + 0x3921fc]
000000018039201F add       rcx, rbp
0000000180392022 jmp       rcx
0000000180392024 mov       rdx, qword ptr [rip + 0x1811d5d] # 0x181ba3d88
000000018039202B jmp       0x18039206a
000000018039202D mov       rdx, qword ptr [rip + 0x18148d4] # 0x181ba6908
0000000180392034 jmp       0x18039206a
0000000180392036 mov       rdx, qword ptr [rip + 0x1814f8b] # 0x181ba6fc8
000000018039203D jmp       0x18039206a
000000018039203F mov       rdx, qword ptr [rip + 0x18136c2] # 0x181ba5708
0000000180392046 jmp       0x18039206a
0000000180392048 mov       rdx, qword ptr [rip + 0x180f341] # 0x181ba1390
000000018039204F jmp       0x18039206a
0000000180392051 mov       rdx, qword ptr [rip + 0x1814e50] # 0x181ba6ea8
0000000180392058 jmp       0x18039206a
000000018039205A mov       rdx, qword ptr [rip + 0x1813b27] # 0x181ba5b88
0000000180392061 jmp       0x18039206a
0000000180392063 mov       rdx, qword ptr [rip + 0x183d01e] # 0x181bcf088
000000018039206A mov       rcx, qword ptr [rip + 0x180e23f] # 0x181ba02b0
0000000180392071 xor       r8d, r8d
0000000180392074 mov       rsi, qword ptr [rbx + 0x40]
0000000180392078 call      0x180b74270
000000018039207D test      rsi, rsi
0000000180392080 je        0x1803921f6
0000000180392086 xor       r9d, r9d
0000000180392089 mov       r8b, 1
000000018039208C mov       rdx, rax
000000018039208F mov       rcx, rsi
0000000180392092 call      0x1811af070
0000000180392097 mov       rcx, qword ptr [rip + 0x1832582] # 0x181bc4620
000000018039209E cmp       dword ptr [rcx + 0xe0], 0
00000001803920A5 jne       0x1803920ac
00000001803920A7 call      0x1802501e0
00000001803920AC xor       edx, edx
00000001803920AE mov       ecx, edi
00000001803920B0 call      0x18036d390
00000001803920B5 mov       rsi, qword ptr [rbx + 0xb8]
00000001803920BC lea       rcx, [rsp + 0x50]
00000001803920C1 xor       edx, edx
00000001803920C3 movss     dword ptr [rsp + 0x50], xmm0
00000001803920C9 call      0x180cc5b50
00000001803920CE mov       rcx, qword ptr [rip + 0x1813ff3] # 0x181ba60c8
00000001803920D5 xor       r8d, r8d
00000001803920D8 mov       rdx, rax
00000001803920DB call      0x180b74270
00000001803920E0 test      rsi, rsi
00000001803920E3 je        0x1803921f6
00000001803920E9 xor       r9d, r9d
00000001803920EC mov       r8b, 1
00000001803920EF mov       rdx, rax
00000001803920F2 mov       rcx, rsi
00000001803920F5 call      0x1811af070
00000001803920FA xor       edx, edx
00000001803920FC mov       ecx, edi
00000001803920FE call      0x18036d400
0000000180392103 mov       rsi, qword ptr [rbx + 0xc0]
000000018039210A lea       rcx, [rsp + 0x68]
000000018039210F xor       edx, edx
0000000180392111 movss     dword ptr [rsp + 0x68], xmm0
0000000180392117 call      0x180cc5b50
000000018039211C mov       rcx, qword ptr [rip + 0x1815325] # 0x181ba7448
0000000180392123 xor       r8d, r8d
0000000180392126 mov       rdx, rax
0000000180392129 call      0x180b74270
000000018039212E test      rsi, rsi
0000000180392131 je        0x1803921f6
0000000180392137 xor       r9d, r9d
000000018039213A mov       r8b, 1
000000018039213D mov       rdx, rax
0000000180392140 mov       rcx, rsi
0000000180392143 call      0x1811af070
0000000180392148 xor       edx, edx
000000018039214A mov       ecx, edi
000000018039214C call      0x18036d300
0000000180392151 mov       rsi, qword ptr [rbx + 0xc8]
0000000180392158 lea       rcx, [rsp + 0x20]
000000018039215D xor       edx, edx
000000018039215F movss     dword ptr [rsp + 0x20], xmm0
0000000180392165 call      0x180cc5b50
000000018039216A mov       rcx, qword ptr [rip + 0x1815037] # 0x181ba71a8
0000000180392171 xor       r8d, r8d
0000000180392174 mov       rdx, rax
0000000180392177 call      0x180b74270
000000018039217C test      rsi, rsi
000000018039217F je        0x1803921f6
0000000180392181 xor       r9d, r9d
0000000180392184 mov       r8b, 1
0000000180392187 mov       rdx, rax
000000018039218A mov       rcx, rsi
000000018039218D call      0x1811af070
0000000180392192 cmp       edi, 6
0000000180392195 ja        0x1803921c7
0000000180392197 mov       ecx, dword ptr [rbp + rdi*4 + 0x392218]
000000018039219E add       rcx, rbp
00000001803921A1 jmp       rcx
00000001803921A3 mov       rdx, qword ptr [rip + 0x1811bde] # 0x181ba3d88
00000001803921AA jmp       0x1803921ce
00000001803921AC mov       rdx, qword ptr [rip + 0x1811c95] # 0x181ba3e48
00000001803921B3 jmp       0x1803921ce
00000001803921B5 mov       rdx, qword ptr [rip + 0x180fe34] # 0x181ba1ff0
00000001803921BC jmp       0x1803921ce
00000001803921BE mov       rdx, qword ptr [rip + 0x1812e23] # 0x181ba4fe8
00000001803921C5 jmp       0x1803921ce
00000001803921C7 mov       rdx, qword ptr [rip + 0x183ceba] # 0x181bcf088
00000001803921CE mov       rcx, qword ptr [rbx + 0xd0]
00000001803921D5 test      rcx, rcx
00000001803921D8 je        0x1803921f6
00000001803921DA xor       r9d, r9d
00000001803921DD mov       r8b, 1
00000001803921E0 call      0x1811af070
00000001803921E5 mov       rsi, qword ptr [rsp + 0x30]
00000001803921EA mov       rbp, qword ptr [rsp + 0x58]
00000001803921EF add       rsp, 0x38
00000001803921F3 pop       rdi
00000001803921F4 pop       rbx
00000001803921F5 ret       
00000001803921F6 call      0x180250150
00000001803921FB int3      
00000001803921FC and       al, 0x20
00000001803921FE cmp       dword ptr [rax], eax
0000000180392200 sub       eax, 0x36003920
0000000180392205 and       byte ptr [rcx], bh
0000000180392207 add       byte ptr [rdi], bh
0000000180392209 and       byte ptr [rcx], bh
000000018039220B add       byte ptr [rax + 0x20], cl
000000018039220E cmp       dword ptr [rax], eax
0000000180392210 push      rcx
0000000180392211 and       byte ptr [rcx], bh
0000000180392213 add       byte ptr [rdx + 0x20], bl
0000000180392216 cmp       dword ptr [rax], eax
0000000180392218 movabs    dword ptr [0xac003921ac003921], eax
0000000180392221 and       dword ptr [rcx], edi
0000000180392223 add       byte ptr [rcx + riz + 0x21b50039], ch
000000018039222A cmp       dword ptr [rax], eax
000000018039222C lodsb     al, byte ptr [rsi]
000000018039222D and       dword ptr [rcx], edi
000000018039222F add       byte ptr [rsi - 0x33ffc6df], bh
0000000180392235 int3      
0000000180392236 int3      
0000000180392237 int3      
0000000180392238 int3      
0000000180392239 int3      
000000018039223A int3      
000000018039223B int3      
000000018039223C int3      
000000018039223D int3      
000000018039223E int3      
000000018039223F int3      