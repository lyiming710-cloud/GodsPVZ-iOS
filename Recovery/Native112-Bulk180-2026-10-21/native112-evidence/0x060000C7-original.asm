000000018030DA70 mov       qword ptr [rsp + 8], rbx
000000018030DA75 mov       qword ptr [rsp + 0x18], rsi
000000018030DA7A push      rdi
000000018030DA7B push      r14
000000018030DA7D push      r15
000000018030DA7F sub       rsp, 0x60
000000018030DA83 mov       rsi, r8
000000018030DA86 mov       rbx, rdx
000000018030DA89 mov       r14, rcx
000000018030DA8C cmp       byte ptr [rip + 0x19d7085], 0 # 0x181ce4b18
000000018030DA93 jne       0x18030dae4
000000018030DA95 lea       rcx, [rip + 0x18b579c] # 0x181bc3238
000000018030DA9C call      0x18024fef0
000000018030DAA1 lea       rcx, [rip + 0x1897448] # 0x181ba4ef0
000000018030DAA8 call      0x18024fef0
000000018030DAAD lea       rcx, [rip + 0x189779c] # 0x181ba5250
000000018030DAB4 call      0x18024fef0
000000018030DAB9 lea       rcx, [rip + 0x189e2b0] # 0x181babd70
000000018030DAC0 call      0x18024fef0
000000018030DAC5 lea       rcx, [rip + 0x18a2a34] # 0x181bb0500
000000018030DACC call      0x18024fef0
000000018030DAD1 lea       rcx, [rip + 0x18b1b90] # 0x181bbf668
000000018030DAD8 call      0x18024fef0
000000018030DADD mov       byte ptr [rip + 0x19d7034], 1 # 0x181ce4b18
000000018030DAE4 xor       r15d, r15d
000000018030DAE7 mov       qword ptr [rsp + 0x20], r15
000000018030DAEC test      rbx, rbx
000000018030DAEF je        0x18030dd71
000000018030DAF5 mov       rdx, qword ptr [rip + 0x18b573c] # 0x181bc3238
000000018030DAFC mov       rcx, rbx
000000018030DAFF call      0x18042e9f0
000000018030DB04 mov       rdi, rax
000000018030DB07 mov       rcx, qword ptr [rip + 0x18a29f2] # 0x181bb0500
000000018030DB0E cmp       dword ptr [rcx + 0xe0], r15d
000000018030DB15 jne       0x18030db1c
000000018030DB17 call      0x1802501e0
000000018030DB1C xor       edx, edx
000000018030DB1E mov       rcx, rdi
000000018030DB21 call      0x18131f870
000000018030DB26 test      al, al
000000018030DB28 je        0x18030dba0
000000018030DB2A xor       edx, edx
000000018030DB2C mov       rcx, rbx
000000018030DB2F call      0x1813044d0
000000018030DB34 mov       r9, rax
000000018030DB37 test      rsi, rsi
000000018030DB3A je        0x18030dd71
000000018030DB40 mov       rax, qword ptr [rip + 0x189e229] # 0x181babd70
000000018030DB47 inc       dword ptr [rsi + 0x1c]
000000018030DB4A mov       rcx, qword ptr [rsi + 0x10]
000000018030DB4E movsxd    rdx, dword ptr [rsi + 0x18]
000000018030DB52 test      rcx, rcx
000000018030DB55 je        0x18030dd71
000000018030DB5B cmp       edx, dword ptr [rcx + 0x18]
000000018030DB5E jb        0x18030db7c
000000018030DB60 mov       rax, qword ptr [rax + 0x20]
000000018030DB64 mov       r8, qword ptr [rax + 0xc0]
000000018030DB6B mov       r8, qword ptr [r8 + 0x70]
000000018030DB6F mov       rdx, r9
000000018030DB72 mov       rcx, rsi
000000018030DB75 call      0x180847ce0
000000018030DB7A jmp       0x18030dba0
000000018030DB7C lea       eax, [rdx + 1]
000000018030DB7F mov       dword ptr [rsi + 0x18], eax
000000018030DB82 cmp       edx, dword ptr [rcx + 0x18]
000000018030DB85 jae       0x18030dd52
000000018030DB8B mov       qword ptr [rcx + rdx*8 + 0x20], r9
000000018030DB90 lea       rcx, [rcx + rdx*8]
000000018030DB94 add       rcx, 0x20
000000018030DB98 mov       rdx, r9
000000018030DB9B call      0x18024f360
000000018030DBA0 xor       edx, edx
000000018030DBA2 mov       rcx, rbx
000000018030DBA5 call      0x18132dc20
000000018030DBAA mov       qword ptr [rsp + 0x88], rax
000000018030DBB2 lea       rax, [rsp + 0x88]
000000018030DBBA mov       qword ptr [rsp + 0x30], rax
000000018030DBBF lea       rax, [rsp + 0x20]
000000018030DBC4 mov       qword ptr [rsp + 0x38], rax
000000018030DBC9 mov       qword ptr [rsp + 0x40], r15
000000018030DBCE movups    xmm0, xmmword ptr [rsp + 0x30]
000000018030DBD3 movups    xmmword ptr [rsp + 0x48], xmm0
000000018030DBD8 nop       dword ptr [rax + rax]
000000018030DBE0 mov       r8, qword ptr [rsp + 0x88]
000000018030DBE8 test      r8, r8
000000018030DBEB je        0x18030dd65
000000018030DBF1 xor       ecx, ecx
000000018030DBF3 mov       rdx, qword ptr [rip + 0x1897656] # 0x181ba5250
000000018030DBFA call      0x180002cd0
000000018030DBFF test      al, al
000000018030DC01 je        0x18030dcea
000000018030DC07 mov       rbx, qword ptr [rsp + 0x88]
000000018030DC0F test      rbx, rbx
000000018030DC12 je        0x18030dd60
000000018030DC18 mov       r9, qword ptr [rip + 0x1897631] # 0x181ba5250
000000018030DC1F mov       r10, qword ptr [rbx]
000000018030DC22 movzx     ecx, r15w
000000018030DC26 movzx     edx, word ptr [r10 + 0x12e]
000000018030DC2E cmp       r15w, dx
000000018030DC32 jae       0x18030dc54
000000018030DC34 mov       r8, qword ptr [r10 + 0xb0]
000000018030DC3B nop       dword ptr [rax + rax]
000000018030DC40 movzx     eax, cx
000000018030DC43 add       rax, rax
000000018030DC46 cmp       qword ptr [r8 + rax*8], r9
000000018030DC4A je        0x18030dc88
000000018030DC4C inc       cx
000000018030DC4F cmp       cx, dx
000000018030DC52 jb        0x18030dc40
000000018030DC54 mov       r8d, 1
000000018030DC5A mov       rdx, r9
000000018030DC5D mov       rcx, rbx
000000018030DC60 call      0x180258380
000000018030DC65 mov       rdx, rax
000000018030DC68 mov       rax, qword ptr [rdx]
000000018030DC6B mov       rdx, qword ptr [rdx + 8]
000000018030DC6F mov       rcx, rbx
000000018030DC72 call      rax
000000018030DC74 mov       r10, rax
000000018030DC77 mov       rdx, qword ptr [rip + 0x18b19ea] # 0x181bbf668
000000018030DC7E test      rax, rax
000000018030DC81 jne       0x18030dca8
000000018030DC83 mov       r10, r15
000000018030DC86 jmp       0x18030dcd4
000000018030DC88 movzx     ecx, cx
000000018030DC8B add       rcx, rcx
000000018030DC8E mov       ecx, dword ptr [r8 + rcx*8 + 8]
000000018030DC93 inc       ecx
000000018030DC95 movsxd    rdx, ecx
000000018030DC98 shl       rdx, 4
000000018030DC9C add       rdx, 0x138
000000018030DCA3 add       rdx, r10
000000018030DCA6 jmp       0x18030dc68
000000018030DCA8 mov       r8, qword ptr [rax]
000000018030DCAB movzx     eax, byte ptr [rdx + 0x130]
000000018030DCB2 cmp       byte ptr [r8 + 0x130], al
000000018030DCB9 jb        0x18030dd58
000000018030DCBF movzx     ecx, al
000000018030DCC2 mov       rax, qword ptr [r8 + 0xc8]
000000018030DCC9 cmp       qword ptr [rax + rcx*8 - 8], rdx
000000018030DCCE jne       0x18030dd58
000000018030DCD4 xor       r9d, r9d
000000018030DCD7 mov       r8, rsi
000000018030DCDA mov       rdx, r10
000000018030DCDD mov       rcx, r14
000000018030DCE0 call      0x18030da70
000000018030DCE5 jmp       0x18030dbe0
000000018030DCEA mov       rax, qword ptr [rsp + 0x30]
000000018030DCEF mov       rdx, qword ptr [rip + 0x18971fa] # 0x181ba4ef0
000000018030DCF6 mov       rcx, qword ptr [rax]
000000018030DCF9 call      0x18024f380
000000018030DCFE mov       r8, rax
000000018030DD01 movups    xmm0, xmmword ptr [rsp + 0x30]
000000018030DD06 psrldq    xmm0, 8
000000018030DD0B movq      rax, xmm0
000000018030DD10 mov       qword ptr [rax], r8
000000018030DD13 test      r8, r8
000000018030DD16 je        0x18030dd26
000000018030DD18 xor       ecx, ecx
000000018030DD1A mov       rdx, qword ptr [rip + 0x18971cf] # 0x181ba4ef0
000000018030DD21 call      0x180002d90
000000018030DD26 jmp       0x18030dd3c
000000018030DD28 lea       rcx, [rsp + 0x48]
000000018030DD2D call      0x1802fb000
000000018030DD32 mov       rcx, qword ptr [rsp + 0x40]
000000018030DD37 test      rcx, rcx
000000018030DD3A jne       0x18030dd6b
000000018030DD3C lea       r11, [rsp + 0x60]
000000018030DD41 mov       rbx, qword ptr [r11 + 0x20]
000000018030DD45 mov       rsi, qword ptr [r11 + 0x30]
000000018030DD49 mov       rsp, r11
000000018030DD4C pop       r15
000000018030DD4E pop       r14
000000018030DD50 pop       rdi
000000018030DD51 ret       
000000018030DD52 call      0x180250140
000000018030DD57 nop       
000000018030DD58 mov       rcx, r10
000000018030DD5B call      0x18024f3a0
000000018030DD60 call      0x180250150
000000018030DD65 call      0x180250150
000000018030DD6A nop       
000000018030DD6B call      0x180200e60
000000018030DD70 int3      
000000018030DD71 call      0x180250150
000000018030DD76 int3      