000000018039EE10 mov       qword ptr [rsp + 0x10], rbx
000000018039EE15 push      rdi
000000018039EE16 sub       rsp, 0x30
000000018039EE1A cmp       byte ptr [rip + 0x1946090], 0 # 0x181ce4eb1
000000018039EE21 mov       rbx, rcx
000000018039EE24 jne       0x18039ee5d
000000018039EE26 lea       rcx, [rip + 0x18031eb] # 0x181ba2018
000000018039EE2D call      0x18024fef0
000000018039EE32 lea       rcx, [rip + 0x1811027] # 0x181bafe60
000000018039EE39 call      0x18024fef0
000000018039EE3E lea       rcx, [rip + 0x1807403] # 0x181ba6248
000000018039EE45 call      0x18024fef0
000000018039EE4A lea       rcx, [rip + 0x1807f37] # 0x181ba6d88
000000018039EE51 call      0x18024fef0
000000018039EE56 mov       byte ptr [rip + 0x1946054], 1 # 0x181ce4eb1
000000018039EE5D mov       rax, qword ptr [rip + 0x18031b4] # 0x181ba2018
000000018039EE64 cmp       dword ptr [rax + 0xe0], 0
000000018039EE6B jne       0x18039ee7c
000000018039EE6D mov       rcx, rax
000000018039EE70 call      0x1802501e0
000000018039EE75 mov       rax, qword ptr [rip + 0x180319c] # 0x181ba2018
000000018039EE7C mov       rax, qword ptr [rax + 0xb8]
000000018039EE83 mov       qword ptr [rsp + 0x40], rsi
000000018039EE88 mov       rdi, qword ptr [rax]
000000018039EE8B test      rdi, rdi
000000018039EE8E je        0x18039f116
000000018039EE94 mov       rdi, qword ptr [rdi + 0x28]
000000018039EE98 test      rdi, rdi
000000018039EE9B je        0x18039f116
000000018039EEA1 mov       rdi, qword ptr [rdi + 0x10]
000000018039EEA5 test      rdi, rdi
000000018039EEA8 je        0x18039f077
000000018039EEAE cmp       dword ptr [rbx + 0x24], 1
000000018039EEB2 jne       0x18039f077
000000018039EEB8 mov       rcx, qword ptr [rbx + 0x48]
000000018039EEBC test      rcx, rcx
000000018039EEBF je        0x18039f116
000000018039EEC5 xor       edx, edx
000000018039EEC7 call      0x1813044d0
000000018039EECC mov       rcx, qword ptr [rdi + 0x38]
000000018039EED0 test      rcx, rcx
000000018039EED3 je        0x18039f116
000000018039EED9 movsxd    rdx, dword ptr [rbx + 0x20]
000000018039EEDD cmp       edx, dword ptr [rcx + 0x18]
000000018039EEE0 jae       0x18039f11c
000000018039EEE6 test      rax, rax
000000018039EEE9 je        0x18039f116
000000018039EEEF cmp       dword ptr [rcx + rdx*4 + 0x20], 0
000000018039EEF4 mov       rcx, rax
000000018039EEF7 setg      dl
000000018039EEFA xor       r8d, r8d
000000018039EEFD call      0x18131bad0
000000018039EF02 mov       rcx, qword ptr [rbx + 0x50]
000000018039EF06 test      rcx, rcx
000000018039EF09 je        0x18039f116
000000018039EF0F xor       edx, edx
000000018039EF11 call      0x1813044d0
000000018039EF16 mov       rcx, qword ptr [rbx + 0x60]
000000018039EF1A mov       rsi, rax
000000018039EF1D test      rcx, rcx
000000018039EF20 je        0x18039f116
000000018039EF26 mov       r8, qword ptr [rip + 0x1810f33] # 0x181bafe60
000000018039EF2D mov       edx, dword ptr [rbx + 0x20]
000000018039EF30 call      0x180836790
000000018039EF35 xor       r8d, r8d
000000018039EF38 mov       edx, eax
000000018039EF3A mov       rcx, rdi
000000018039EF3D call      0x18037fc70
000000018039EF42 test      rsi, rsi
000000018039EF45 je        0x18039f116
000000018039EF4B test      rax, rax
000000018039EF4E mov       rcx, rsi
000000018039EF51 setne     dl
000000018039EF54 xor       r8d, r8d
000000018039EF57 call      0x18131bad0
000000018039EF5C mov       edx, dword ptr [rbx + 0x20]
000000018039EF5F xor       r8d, r8d
000000018039EF62 mov       rsi, qword ptr [rbx + 0x30]
000000018039EF66 xor       ecx, ecx
000000018039EF68 shl       edx, 2
000000018039EF6B call      0x18030f6f0
000000018039EF70 mov       r8, qword ptr [rip + 0x18072d1] # 0x181ba6248
000000018039EF77 xor       r9d, r9d
000000018039EF7A mov       rcx, qword ptr [rip + 0x1807e07] # 0x181ba6d88
000000018039EF81 mov       rdx, rax
000000018039EF84 call      0x180b73f50
000000018039EF89 test      rsi, rsi
000000018039EF8C je        0x18039f116
000000018039EF92 xor       r9d, r9d
000000018039EF95 mov       r8b, 1
000000018039EF98 mov       rdx, rax
000000018039EF9B mov       rcx, rsi
000000018039EF9E call      0x1811af070
000000018039EFA3 mov       eax, dword ptr [rbx + 0x20]
000000018039EFA6 mov       rcx, qword ptr [rbx + 0x58]
000000018039EFAA shl       eax, 2
000000018039EFAD cmp       dword ptr [rdi + 0x20], eax
000000018039EFB0 jg        0x18039f087
000000018039EFB6 test      rcx, rcx
000000018039EFB9 je        0x18039f116
000000018039EFBF xor       edx, edx
000000018039EFC1 call      0x1813044d0
000000018039EFC6 test      rax, rax
000000018039EFC9 je        0x18039f116
000000018039EFCF xor       r8d, r8d
000000018039EFD2 mov       dl, 1
000000018039EFD4 mov       rcx, rax
000000018039EFD7 call      0x18131bad0
000000018039EFDC mov       rcx, qword ptr [rbx + 0x38]
000000018039EFE0 test      rcx, rcx
000000018039EFE3 je        0x18039f116
000000018039EFE9 xor       r8d, r8d
000000018039EFEC xor       edx, edx
000000018039EFEE call      0x18156cc70
000000018039EFF3 mov       rcx, qword ptr [rbx + 0x28]
000000018039EFF7 test      rcx, rcx
000000018039EFFA je        0x18039f116
000000018039F000 xor       edx, edx
000000018039F002 call      0x1813044d0
000000018039F007 test      rax, rax
000000018039F00A je        0x18039f116
000000018039F010 xor       r8d, r8d
000000018039F013 xor       edx, edx
000000018039F015 mov       rcx, rax
000000018039F018 call      0x18131bad0
000000018039F01D mov       rcx, qword ptr [rbx + 0x30]
000000018039F021 test      rcx, rcx
000000018039F024 je        0x18039f116
000000018039F02A xor       edx, edx
000000018039F02C call      0x1813044d0
000000018039F031 test      rax, rax
000000018039F034 je        0x18039f116
000000018039F03A xor       r8d, r8d
000000018039F03D mov       dl, 1
000000018039F03F mov       rcx, rax
000000018039F042 call      0x18131bad0
000000018039F047 mov       rcx, qword ptr [rbx + 0x40]
000000018039F04B test      rcx, rcx
000000018039F04E je        0x18039f116
000000018039F054 movdqa    xmm0, xmmword ptr [rip + 0x1208f84] # 0x1815a7fe0
000000018039F05C mov       rax, qword ptr [rcx]
000000018039F05F lea       rdx, [rsp + 0x20]
000000018039F064 movdqa    xmmword ptr [rsp + 0x20], xmm0
000000018039F06A mov       r8, qword ptr [rax + 0x2b0]
000000018039F071 call      qword ptr [rax + 0x2a8]
000000018039F077 mov       rsi, qword ptr [rsp + 0x40]
000000018039F07C mov       rbx, qword ptr [rsp + 0x48]
000000018039F081 add       rsp, 0x30
000000018039F085 pop       rdi
000000018039F086 ret       
000000018039F087 test      rcx, rcx
000000018039F08A je        0x18039f116
000000018039F090 xor       edx, edx
000000018039F092 call      0x1813044d0
000000018039F097 test      rax, rax
000000018039F09A je        0x18039f116
000000018039F09C xor       r8d, r8d
000000018039F09F xor       edx, edx
000000018039F0A1 mov       rcx, rax
000000018039F0A4 call      0x18131bad0
000000018039F0A9 mov       rcx, qword ptr [rbx + 0x38]
000000018039F0AD test      rcx, rcx
000000018039F0B0 je        0x18039f116
000000018039F0B2 xor       r8d, r8d
000000018039F0B5 mov       dl, 1
000000018039F0B7 call      0x18156cc70
000000018039F0BC mov       rcx, qword ptr [rbx + 0x28]
000000018039F0C0 test      rcx, rcx
000000018039F0C3 je        0x18039f116
000000018039F0C5 xor       edx, edx
000000018039F0C7 call      0x1813044d0
000000018039F0CC test      rax, rax
000000018039F0CF je        0x18039f116
000000018039F0D1 xor       r8d, r8d
000000018039F0D4 mov       dl, 1
000000018039F0D6 mov       rcx, rax
000000018039F0D9 call      0x18131bad0
000000018039F0DE mov       rcx, qword ptr [rbx + 0x30]
000000018039F0E2 test      rcx, rcx
000000018039F0E5 je        0x18039f116
000000018039F0E7 xor       edx, edx
000000018039F0E9 call      0x1813044d0
000000018039F0EE test      rax, rax
000000018039F0F1 je        0x18039f116
000000018039F0F3 xor       r8d, r8d
000000018039F0F6 xor       edx, edx
000000018039F0F8 mov       rcx, rax
000000018039F0FB call      0x18131bad0
000000018039F100 mov       rcx, qword ptr [rbx + 0x40]
000000018039F104 test      rcx, rcx
000000018039F107 je        0x18039f116
000000018039F109 movdqa    xmm0, xmmword ptr [rip + 0x1208a5f] # 0x1815a7b70
000000018039F111 jmp       0x18039f05c
000000018039F116 call      0x180250150
000000018039F11B int3      
000000018039F11C call      0x180250140
000000018039F121 int3      
000000018039F122 int3      
000000018039F123 int3      
000000018039F124 int3      
000000018039F125 int3      
000000018039F126 int3      
000000018039F127 int3      
000000018039F128 int3      
000000018039F129 int3      
000000018039F12A int3      
000000018039F12B int3      
000000018039F12C int3      
000000018039F12D int3      
000000018039F12E int3      
000000018039F12F int3      