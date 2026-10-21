000000018030CE00 mov       qword ptr [rsp + 0x18], rbx
000000018030CE05 mov       qword ptr [rsp + 0x20], rsi
000000018030CE0A push      rdi
000000018030CE0B sub       rsp, 0x20
000000018030CE0F cmp       byte ptr [rip + 0x19d7ce6], 0 # 0x181ce4afc
000000018030CE16 mov       rbx, rdx
000000018030CE19 mov       rdi, rcx
000000018030CE1C jne       0x18030ce91
000000018030CE1E lea       rcx, [rip + 0x188df23] # 0x181b9ad48
000000018030CE25 call      0x18024fef0
000000018030CE2A lea       rcx, [rip + 0x18b26cf] # 0x181bbf500
000000018030CE31 call      0x18024fef0
000000018030CE36 lea       rcx, [rip + 0x18a3e23] # 0x181bb0c60
000000018030CE3D call      0x18024fef0
000000018030CE42 lea       rcx, [rip + 0x189f097] # 0x181babee0
000000018030CE49 call      0x18024fef0
000000018030CE4E lea       rcx, [rip + 0x1899cf3] # 0x181ba6b48
000000018030CE55 call      0x18024fef0
000000018030CE5A lea       rcx, [rip + 0x18b98ef] # 0x181bc6750
000000018030CE61 call      0x18024fef0
000000018030CE66 lea       rcx, [rip + 0x18b032b] # 0x181bbd198
000000018030CE6D call      0x18024fef0
000000018030CE72 lea       rcx, [rip + 0x18a938f] # 0x181bb6208
000000018030CE79 call      0x18024fef0
000000018030CE7E lea       rcx, [rip + 0x18a927b] # 0x181bb6100
000000018030CE85 call      0x18024fef0
000000018030CE8A mov       byte ptr [rip + 0x19d7c6b], 1 # 0x181ce4afc
000000018030CE91 mov       rcx, qword ptr [rip + 0x188deb0] # 0x181b9ad48
000000018030CE98 xor       eax, eax
000000018030CE9A mov       dword ptr [rsp + 0x38], eax
000000018030CE9E mov       dword ptr [rsp + 0x30], eax
000000018030CEA2 cmp       dword ptr [rcx + 0xe0], eax
000000018030CEA8 jne       0x18030ceaf
000000018030CEAA call      0x1802501e0
000000018030CEAF xor       edx, edx
000000018030CEB1 mov       rcx, rbx
000000018030CEB4 call      0x1812e68f0
000000018030CEB9 mov       rsi, qword ptr [rdi + 0x20]
000000018030CEBD xor       edx, edx
000000018030CEBF mov       rcx, rsi
000000018030CEC2 call      0x1803ccf20
000000018030CEC7 cmp       eax, 0x66de7a09
000000018030CECC jbe       0x18030d02e
000000018030CED2 cmp       eax, 0xcb7fbec6
000000018030CED7 jbe       0x18030cf57
000000018030CED9 cmp       eax, 0xd07cc970
000000018030CEDE jne       0x18030cf23
000000018030CEE0 mov       rdx, qword ptr [rip + 0x18a3d79] # 0x181bb0c60
000000018030CEE7 xor       r8d, r8d
000000018030CEEA mov       rcx, rsi
000000018030CEED call      0x180b76170
000000018030CEF2 test      al, al
000000018030CEF4 je        0x18030cf13
000000018030CEF6 mov       rcx, qword ptr [rdi + 0x38]
000000018030CEFA test      rcx, rcx
000000018030CEFD je        0x18030d12b
000000018030CF03 mov       qword ptr [rcx + 0x20], rbx
000000018030CF07 add       rcx, 0x20
000000018030CF0B mov       rdx, rbx
000000018030CF0E call      0x18024f360
000000018030CF13 mov       rbx, qword ptr [rsp + 0x40]
000000018030CF18 mov       rsi, qword ptr [rsp + 0x48]
000000018030CF1D add       rsp, 0x20
000000018030CF21 pop       rdi
000000018030CF22 ret       
000000018030CF23 cmp       eax, 0xd22573dd
000000018030CF28 jne       0x18030cf13
000000018030CF2A mov       rdx, qword ptr [rip + 0x18b981f] # 0x181bc6750
000000018030CF31 xor       r8d, r8d
000000018030CF34 mov       rcx, rsi
000000018030CF37 call      0x180b76170
000000018030CF3C test      al, al
000000018030CF3E je        0x18030cf13
000000018030CF40 mov       rcx, qword ptr [rdi + 0x38]
000000018030CF44 test      rcx, rcx
000000018030CF47 je        0x18030d12b
000000018030CF4D mov       qword ptr [rcx + 0x28], rbx
000000018030CF51 add       rcx, 0x28
000000018030CF55 jmp       0x18030cf0b
000000018030CF57 cmp       eax, 0x6c5a3a15
000000018030CF5C jne       0x18030cf9e
000000018030CF5E mov       rdx, qword ptr [rip + 0x18a92a3] # 0x181bb6208
000000018030CF65 xor       r8d, r8d
000000018030CF68 mov       rcx, rsi
000000018030CF6B call      0x180b76170
000000018030CF70 test      al, al
000000018030CF72 je        0x18030cf13
000000018030CF74 lea       rcx, [rdi + 0x30]
000000018030CF78 mov       qword ptr [rdi + 0x30], rbx
000000018030CF7C mov       rdx, rbx
000000018030CF7F call      0x18024f360
000000018030CF84 mov       rcx, qword ptr [rdi + 0x38]
000000018030CF88 test      rcx, rcx
000000018030CF8B je        0x18030d12b
000000018030CF91 mov       qword ptr [rcx + 0x10], rbx
000000018030CF95 add       rcx, 0x10
000000018030CF99 jmp       0x18030cf0b
000000018030CF9E cmp       eax, 0xcb7fbec6
000000018030CFA3 jne       0x18030cf13
000000018030CFA9 mov       rdx, qword ptr [rip + 0x189ef30] # 0x181babee0
000000018030CFB0 xor       r8d, r8d
000000018030CFB3 mov       rcx, rsi
000000018030CFB6 call      0x180b76170
000000018030CFBB test      al, al
000000018030CFBD je        0x18030cf13
000000018030CFC3 xor       r8d, r8d
000000018030CFC6 lea       rdx, [rsp + 0x30]
000000018030CFCB mov       rcx, rbx
000000018030CFCE call      0x180ca3c50
000000018030CFD3 test      al, al
000000018030CFD5 jne       0x18030d00a
000000018030CFD7 mov       rcx, qword ptr [rip + 0x188dd6a] # 0x181b9ad48
000000018030CFDE cmp       dword ptr [rcx + 0xe0], 0
000000018030CFE5 jne       0x18030cfec
000000018030CFE7 call      0x1802501e0
000000018030CFEC mov       rcx, qword ptr [rip + 0x1899b55] # 0x181ba6b48
000000018030CFF3 xor       edx, edx
000000018030CFF5 call      0x1812e68f0
000000018030CFFA mov       rbx, qword ptr [rsp + 0x40]
000000018030CFFF mov       rsi, qword ptr [rsp + 0x48]
000000018030D004 add       rsp, 0x20
000000018030D008 pop       rdi
000000018030D009 ret       
000000018030D00A mov       rcx, qword ptr [rdi + 0x38]
000000018030D00E test      rcx, rcx
000000018030D011 je        0x18030d12b
000000018030D017 mov       eax, dword ptr [rsp + 0x30]
000000018030D01B mov       dword ptr [rcx + 0x1c], eax
000000018030D01E mov       rbx, qword ptr [rsp + 0x40]
000000018030D023 mov       rsi, qword ptr [rsp + 0x48]
000000018030D028 add       rsp, 0x20
000000018030D02C pop       rdi
000000018030D02D ret       
000000018030D02E cmp       eax, 0x1b2a6feb
000000018030D033 jne       0x18030d0b9
000000018030D039 mov       rdx, qword ptr [rip + 0x18a90c0] # 0x181bb6100
000000018030D040 xor       r8d, r8d
000000018030D043 mov       rcx, rsi
000000018030D046 call      0x180b76170
000000018030D04B test      al, al
000000018030D04D je        0x18030cf13
000000018030D053 xor       r8d, r8d
000000018030D056 lea       rdx, [rsp + 0x38]
000000018030D05B mov       rcx, rbx
000000018030D05E call      0x180ca3c50
000000018030D063 test      al, al
000000018030D065 jne       0x18030d09a
000000018030D067 mov       rcx, qword ptr [rip + 0x188dcda] # 0x181b9ad48
000000018030D06E cmp       dword ptr [rcx + 0xe0], 0
000000018030D075 jne       0x18030d07c
000000018030D077 call      0x1802501e0
000000018030D07C mov       rcx, qword ptr [rip + 0x1899ac5] # 0x181ba6b48
000000018030D083 xor       edx, edx
000000018030D085 call      0x1812e68f0
000000018030D08A mov       rbx, qword ptr [rsp + 0x40]
000000018030D08F mov       rsi, qword ptr [rsp + 0x48]
000000018030D094 add       rsp, 0x20
000000018030D098 pop       rdi
000000018030D099 ret       
000000018030D09A mov       edx, dword ptr [rsp + 0x38]
000000018030D09E xor       r8d, r8d
000000018030D0A1 mov       rcx, rdi
000000018030D0A4 call      0x18030d140
000000018030D0A9 mov       rbx, qword ptr [rsp + 0x40]
000000018030D0AE mov       rsi, qword ptr [rsp + 0x48]
000000018030D0B3 add       rsp, 0x20
000000018030D0B7 pop       rdi
000000018030D0B8 ret       
000000018030D0B9 cmp       eax, 0x1d97162a
000000018030D0BE jne       0x18030d0f0
000000018030D0C0 mov       rdx, qword ptr [rip + 0x18b2439] # 0x181bbf500
000000018030D0C7 xor       r8d, r8d
000000018030D0CA mov       rcx, rsi
000000018030D0CD call      0x180b76170
000000018030D0D2 test      al, al
000000018030D0D4 je        0x18030cf13
000000018030D0DA mov       rcx, qword ptr [rdi + 0x38]
000000018030D0DE test      rcx, rcx
000000018030D0E1 je        0x18030d12b
000000018030D0E3 mov       qword ptr [rcx + 0x38], rbx
000000018030D0E7 add       rcx, 0x38
000000018030D0EB jmp       0x18030cf0b
000000018030D0F0 cmp       eax, 0x66de7a09
000000018030D0F5 jne       0x18030cf13
000000018030D0FB mov       rdx, qword ptr [rip + 0x18b0096] # 0x181bbd198
000000018030D102 xor       r8d, r8d
000000018030D105 mov       rcx, rsi
000000018030D108 call      0x180b76170
000000018030D10D test      al, al
000000018030D10F je        0x18030cf13
000000018030D115 mov       rcx, qword ptr [rdi + 0x38]
000000018030D119 test      rcx, rcx
000000018030D11C je        0x18030d12b
000000018030D11E mov       qword ptr [rcx + 0x30], rbx
000000018030D122 add       rcx, 0x30
000000018030D126 jmp       0x18030cf0b
000000018030D12B call      0x180250150
000000018030D130 int3      
000000018030D131 int3      
000000018030D132 int3      
000000018030D133 int3      
000000018030D134 int3      
000000018030D135 int3      
000000018030D136 int3      
000000018030D137 int3      
000000018030D138 int3      
000000018030D139 int3      
000000018030D13A int3      
000000018030D13B int3      
000000018030D13C int3      
000000018030D13D int3      
000000018030D13E int3      
000000018030D13F int3      