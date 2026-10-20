000000018036FDF0 mov       qword ptr [rsp + 8], rbx
000000018036FDF5 mov       qword ptr [rsp + 0x10], rsi
000000018036FDFA mov       qword ptr [rsp + 0x18], rdi
000000018036FDFF push      r12
000000018036FE01 push      r14
000000018036FE03 push      r15
000000018036FE05 sub       rsp, 0x70
000000018036FE09 mov       r14, rcx
000000018036FE0C cmp       byte ptr [rip + 0x1974f88], 0 # 0x181ce4d9b
000000018036FE13 jne       0x18036fe7c
000000018036FE15 lea       rcx, [rip + 0x182d1ac] # 0x181b9cfc8
000000018036FE1C call      0x18024fef0
000000018036FE21 lea       rcx, [rip + 0x1853950] # 0x181bc3778
000000018036FE28 call      0x18024fef0
000000018036FE2D lea       rcx, [rip + 0x185399c] # 0x181bc37d0
000000018036FE34 call      0x18024fef0
000000018036FE39 lea       rcx, [rip + 0x18539e8] # 0x181bc3828
000000018036FE40 call      0x18024fef0
000000018036FE45 lea       rcx, [rip + 0x183ce44] # 0x181bacc90
000000018036FE4C call      0x18024fef0
000000018036FE51 lea       rcx, [rip + 0x1839370] # 0x181ba91c8
000000018036FE58 call      0x18024fef0
000000018036FE5D lea       rcx, [rip + 0x183f7fc] # 0x181baf660
000000018036FE64 call      0x18024fef0
000000018036FE69 lea       rcx, [rip + 0x1840690] # 0x181bb0500
000000018036FE70 call      0x18024fef0
000000018036FE75 mov       byte ptr [rip + 0x1974f1f], 1 # 0x181ce4d9b
000000018036FE7C mov       rax, qword ptr [r14 + 0x20]
000000018036FE80 test      rax, rax
000000018036FE83 je        0x18037007e
000000018036FE89 mov       rdi, qword ptr [rax + 0x30]
000000018036FE8D mov       r15, qword ptr [rax + 0x108]
000000018036FE94 test      r15, r15
000000018036FE97 je        0x18037007e
000000018036FE9D mov       rcx, qword ptr [r15 + 0x28]
000000018036FEA1 test      rcx, rcx
000000018036FEA4 je        0x18037007e
000000018036FEAA mov       r8, qword ptr [rip + 0x1839317] # 0x181ba91c8
000000018036FEB1 mov       edx, 0xd
000000018036FEB6 call      0x180825710
000000018036FEBB mov       rsi, rax
000000018036FEBE test      rdi, rdi
000000018036FEC1 je        0x18037007e
000000018036FEC7 xor       edx, edx
000000018036FEC9 mov       rcx, rdi
000000018036FECC call      0x18032c3b0
000000018036FED1 mov       ebx, eax
000000018036FED3 xor       edx, edx
000000018036FED5 mov       rcx, rdi
000000018036FED8 call      0x18032c3b0
000000018036FEDD dec       eax
000000018036FEDF lea       r9d, [rbx - 1]
000000018036FEE3 mov       rdx, qword ptr [rip + 0x183f776] # 0x181baf660
000000018036FEEA mov       qword ptr [rsp + 0x28], rdx
000000018036FEEF mov       dword ptr [rsp + 0x20], eax
000000018036FEF3 mov       r8, rsi
000000018036FEF6 mov       edx, 0x3e7
000000018036FEFB mov       rcx, rdi
000000018036FEFE call      0x1804910b0
000000018036FF03 test      rax, rax
000000018036FF06 je        0x18037007e
000000018036FF0C mov       r8, qword ptr [rip + 0x183cd7d] # 0x181bacc90
000000018036FF13 mov       rdx, rax
000000018036FF16 lea       rcx, [rsp + 0x38]
000000018036FF1B call      0x1808197f0
000000018036FF20 movups    xmm0, xmmword ptr [rsp + 0x38]
000000018036FF25 movups    xmmword ptr [rsp + 0x50], xmm0
000000018036FF2A movsd     xmm1, qword ptr [rsp + 0x48]
000000018036FF30 movsd     qword ptr [rsp + 0x60], xmm1
000000018036FF36 xor       r12d, r12d
000000018036FF39 mov       qword ptr [rsp + 0x38], r12
000000018036FF3E lea       rbx, [rsp + 0x50]
000000018036FF43 mov       qword ptr [rsp + 0x40], rbx
000000018036FF48 nop       dword ptr [rax + rax]
000000018036FF50 mov       rdx, qword ptr [rip + 0x1853879] # 0x181bc37d0
000000018036FF57 lea       rcx, [rsp + 0x50]
000000018036FF5C call      0x180669280
000000018036FF61 test      al, al
000000018036FF63 je        0x18037001b
000000018036FF69 mov       rsi, qword ptr [rsp + 0x60]
000000018036FF6E mov       rcx, qword ptr [rip + 0x182d053] # 0x181b9cfc8
000000018036FF75 call      0x180250100
000000018036FF7A mov       rdi, rax
000000018036FF7D xor       r8d, r8d
000000018036FF80 mov       edx, 0xd
000000018036FF85 mov       rcx, rax
000000018036FF88 call      0x180345e10
000000018036FF8D mov       eax, dword ptr [r14 + 0xfc]
000000018036FF94 test      rdi, rdi
000000018036FF97 je        0x180370072
000000018036FF9D mov       dword ptr [rdi + 0x18], eax
000000018036FFA0 test      rsi, rsi
000000018036FFA3 je        0x18037006d
000000018036FFA9 mov       qword ptr [rsp + 0x20], r12
000000018036FFAE mov       r9d, dword ptr [rsi + 0x20]
000000018036FFB2 mov       r8d, dword ptr [rsi + 0x1c]
000000018036FFB6 mov       rdx, rdi
000000018036FFB9 mov       rcx, r15
000000018036FFBC call      0x180312e90
000000018036FFC1 mov       rdi, rax
000000018036FFC4 mov       rcx, qword ptr [rip + 0x1840535] # 0x181bb0500
000000018036FFCB cmp       dword ptr [rcx + 0xe0], 0
000000018036FFD2 jne       0x18036ffd9
000000018036FFD4 call      0x1802501e0
000000018036FFD9 xor       r8d, r8d
000000018036FFDC xor       edx, edx
000000018036FFDE mov       rcx, rdi
000000018036FFE1 call      0x18131f900
000000018036FFE6 test      al, al
000000018036FFE8 je        0x180370007
000000018036FFEA test      rdi, rdi
000000018036FFED je        0x180370062
000000018036FFEF mov       qword ptr [rdi + 0xe8], r14
000000018036FFF6 lea       rcx, [rdi + 0xe8]
000000018036FFFD mov       rdx, r14
0000000180370000 call      0x18024f360
0000000180370005 jmp       0x18037000c
0000000180370007 test      rdi, rdi
000000018037000A je        0x180370068
000000018037000C xor       edx, edx
000000018037000E mov       rcx, rdi
0000000180370011 call      0x180347db0
0000000180370016 jmp       0x18036ff50
000000018037001B mov       rdx, qword ptr [rip + 0x1853756] # 0x181bc3778
0000000180370022 mov       rcx, rbx
0000000180370025 call      0x180302170
000000018037002A jmp       0x180370047
000000018037002C mov       rdx, qword ptr [rip + 0x1853745] # 0x181bc3778
0000000180370033 mov       rcx, qword ptr [rsp + 0x40]
0000000180370038 call      0x180302170
000000018037003D mov       rcx, qword ptr [rsp + 0x38]
0000000180370042 test      rcx, rcx
0000000180370045 jne       0x180370078
0000000180370047 lea       r11, [rsp + 0x70]
000000018037004C mov       rbx, qword ptr [r11 + 0x20]
0000000180370050 mov       rsi, qword ptr [r11 + 0x28]
0000000180370054 mov       rdi, qword ptr [r11 + 0x30]
0000000180370058 mov       rsp, r11
000000018037005B pop       r15
000000018037005D pop       r14
000000018037005F pop       r12
0000000180370061 ret       
0000000180370062 call      0x180250150
0000000180370067 nop       
0000000180370068 call      0x180250150
000000018037006D call      0x180250150
0000000180370072 call      0x180250150
0000000180370077 nop       
0000000180370078 call      0x180200e60
000000018037007D int3      
000000018037007E call      0x180250150
0000000180370083 int3      