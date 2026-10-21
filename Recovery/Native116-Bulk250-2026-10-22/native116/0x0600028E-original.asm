000000018032CC20 mov       qword ptr [rsp + 8], rbx
000000018032CC25 mov       qword ptr [rsp + 0x10], rsi
000000018032CC2A mov       qword ptr [rsp + 0x18], rdi
000000018032CC2F mov       qword ptr [rsp + 0x20], r14
000000018032CC34 push      r15
000000018032CC36 sub       rsp, 0x60
000000018032CC3A mov       ebx, r9d
000000018032CC3D mov       r14d, r8d
000000018032CC40 mov       esi, edx
000000018032CC42 mov       r15, rcx
000000018032CC45 cmp       byte ptr [rip + 0x19b8024], 0 # 0x181ce4c70
000000018032CC4C jne       0x18032ccf5
000000018032CC52 lea       rcx, [rip + 0x18981cf] # 0x181bc4e28
000000018032CC59 call      0x18024fef0
000000018032CC5E lea       rcx, [rip + 0x189821b] # 0x181bc4e80
000000018032CC65 call      0x18024fef0
000000018032CC6A lea       rcx, [rip + 0x1898267] # 0x181bc4ed8
000000018032CC71 call      0x18024fef0
000000018032CC76 lea       rcx, [rip + 0x186cf6b] # 0x181b99be8
000000018032CC7D call      0x18024fef0
000000018032CC82 lea       rcx, [rip + 0x186d067] # 0x181b99cf0
000000018032CC89 call      0x18024fef0
000000018032CC8E lea       rcx, [rip + 0x186ce4b] # 0x181b99ae0
000000018032CC95 call      0x18024fef0
000000018032CC9A lea       rcx, [rip + 0x186d157] # 0x181b99df8
000000018032CCA1 call      0x18024fef0
000000018032CCA6 lea       rcx, [rip + 0x188b4e3] # 0x181bb8190
000000018032CCAD call      0x18024fef0
000000018032CCB2 lea       rcx, [rip + 0x187ff7f] # 0x181bacc38
000000018032CCB9 call      0x18024fef0
000000018032CCBE lea       rcx, [rip + 0x187ff1b] # 0x181bacbe0
000000018032CCC5 call      0x18024fef0
000000018032CCCA lea       rcx, [rip + 0x188006f] # 0x181bacd40
000000018032CCD1 call      0x18024fef0
000000018032CCD6 lea       rcx, [rip + 0x18800bb] # 0x181bacd98
000000018032CCDD call      0x18024fef0
000000018032CCE2 lea       rcx, [rip + 0x1891117] # 0x181bbde00
000000018032CCE9 call      0x18024fef0
000000018032CCEE mov       byte ptr [rip + 0x19b7f7b], 1 # 0x181ce4c70
000000018032CCF5 xorps     xmm0, xmm0
000000018032CCF8 xor       eax, eax
000000018032CCFA movups    xmmword ptr [rsp + 0x48], xmm0
000000018032CCFF mov       qword ptr [rsp + 0x58], rax
000000018032CD04 mov       rcx, qword ptr [rip + 0x18910f5] # 0x181bbde00
000000018032CD0B call      0x180250100
000000018032CD10 mov       rdi, rax
000000018032CD13 mov       rdx, qword ptr [rip + 0x187fec6] # 0x181bacbe0
000000018032CD1A mov       rcx, rax
000000018032CD1D call      0x1808053d0
000000018032CD22 mov       qword ptr [rsp + 0x20], rdi
000000018032CD27 xor       r9d, r9d
000000018032CD2A mov       r8d, ebx
000000018032CD2D mov       edx, r14d
000000018032CD30 mov       rcx, r15
000000018032CD33 call      0x18032c1c0
000000018032CD38 mov       r15, rax
000000018032CD3B test      rax, rax
000000018032CD3E je        0x18032cec1
000000018032CD44 mov       r14d, dword ptr [rax + 0x18]
000000018032CD48 cmp       esi, r14d
000000018032CD4B jg        0x18032cea6
000000018032CD51 mov       rcx, qword ptr [rip + 0x188b438] # 0x181bb8190
000000018032CD58 call      0x180250100
000000018032CD5D mov       rbx, rax
000000018032CD60 mov       rdx, qword ptr [rip + 0x186cd79] # 0x181b99ae0
000000018032CD67 mov       rcx, rax
000000018032CD6A call      0x1807951a0
000000018032CD6F test      rbx, rbx
000000018032CD72 je        0x18032cec1
000000018032CD78 cmp       dword ptr [rbx + 0x20], esi
000000018032CD7B jge       0x18032cd9d
000000018032CD7D xor       r8d, r8d
000000018032CD80 mov       edx, r14d
000000018032CD83 xor       ecx, ecx
000000018032CD85 call      0x18130d310
000000018032CD8A mov       r8, qword ptr [rip + 0x186ce57] # 0x181b99be8
000000018032CD91 mov       edx, eax
000000018032CD93 mov       rcx, rbx
000000018032CD96 call      0x180788700
000000018032CD9B jmp       0x18032cd78
000000018032CD9D mov       r8, qword ptr [rip + 0x186cf4c] # 0x181b99cf0
000000018032CDA4 mov       rdx, rbx
000000018032CDA7 lea       rcx, [rsp + 0x30]
000000018032CDAC call      0x18078b020
000000018032CDB1 movups    xmm0, xmmword ptr [rsp + 0x30]
000000018032CDB6 movups    xmmword ptr [rsp + 0x48], xmm0
000000018032CDBB movsd     xmm1, qword ptr [rsp + 0x40]
000000018032CDC1 movsd     qword ptr [rsp + 0x58], xmm1
000000018032CDC7 mov       qword ptr [rsp + 0x30], 0
000000018032CDD0 lea       rbx, [rsp + 0x48]
000000018032CDD5 mov       qword ptr [rsp + 0x38], rbx
000000018032CDDA nop       word ptr [rax + rax]
000000018032CDE0 mov       rdx, qword ptr [rip + 0x1898099] # 0x181bc4e80
000000018032CDE7 lea       rcx, [rsp + 0x48]
000000018032CDEC call      0x1806676e0
000000018032CDF1 test      al, al
000000018032CDF3 je        0x18032ce72
000000018032CDF5 mov       r8, qword ptr [rip + 0x187ff9c] # 0x181bacd98
000000018032CDFC mov       edx, dword ptr [rsp + 0x58]
000000018032CE00 mov       rcx, r15
000000018032CE03 call      0x180825710
000000018032CE08 mov       rdx, rax
000000018032CE0B test      rdi, rdi
000000018032CE0E je        0x18032ced2
000000018032CE14 mov       r9, qword ptr [rip + 0x187fe1d] # 0x181bacc38
000000018032CE1B inc       dword ptr [rdi + 0x1c]
000000018032CE1E mov       rcx, qword ptr [rdi + 0x10]
000000018032CE22 movsxd    r8, dword ptr [rdi + 0x18]
000000018032CE26 test      rcx, rcx
000000018032CE29 je        0x18032cecd
000000018032CE2F cmp       r8d, dword ptr [rcx + 0x18]
000000018032CE33 jb        0x18032ce4e
000000018032CE35 mov       rax, qword ptr [r9 + 0x20]
000000018032CE39 mov       r8, qword ptr [rax + 0xc0]
000000018032CE40 mov       r8, qword ptr [r8 + 0x70]
000000018032CE44 mov       rcx, rdi
000000018032CE47 call      0x180847ce0
000000018032CE4C jmp       0x18032cde0
000000018032CE4E lea       eax, [r8 + 1]
000000018032CE52 mov       dword ptr [rdi + 0x18], eax
000000018032CE55 cmp       r8d, dword ptr [rcx + 0x18]
000000018032CE59 jae       0x18032cec7
000000018032CE5B mov       qword ptr [rcx + r8*8 + 0x20], rdx
000000018032CE60 lea       rcx, [rcx + r8*8]
000000018032CE64 add       rcx, 0x20
000000018032CE68 call      0x18024f360
000000018032CE6D jmp       0x18032cde0
000000018032CE72 mov       rdx, qword ptr [rip + 0x1897faf] # 0x181bc4e28
000000018032CE79 mov       rcx, rbx
000000018032CE7C call      0x180302170
000000018032CE81 jmp       0x18032cea3
000000018032CE83 mov       rdx, qword ptr [rip + 0x1897f9e] # 0x181bc4e28
000000018032CE8A mov       rcx, qword ptr [rsp + 0x38]
000000018032CE8F call      0x180302170
000000018032CE94 mov       rcx, qword ptr [rsp + 0x30]
000000018032CE99 test      rcx, rcx
000000018032CE9C jne       0x18032ced8
000000018032CE9E mov       rdi, qword ptr [rsp + 0x20]
000000018032CEA3 mov       rax, rdi
000000018032CEA6 lea       r11, [rsp + 0x60]
000000018032CEAB mov       rbx, qword ptr [r11 + 0x10]
000000018032CEAF mov       rsi, qword ptr [r11 + 0x18]
000000018032CEB3 mov       rdi, qword ptr [r11 + 0x20]
000000018032CEB7 mov       r14, qword ptr [r11 + 0x28]
000000018032CEBB mov       rsp, r11
000000018032CEBE pop       r15
000000018032CEC0 ret       
000000018032CEC1 call      0x180250150
000000018032CEC6 nop       
000000018032CEC7 call      0x180250140
000000018032CECC nop       
000000018032CECD call      0x180250150
000000018032CED2 call      0x180250150
000000018032CED7 nop       
000000018032CED8 call      0x180200e60
000000018032CEDD int3      
000000018032CEDE int3      
000000018032CEDF int3      