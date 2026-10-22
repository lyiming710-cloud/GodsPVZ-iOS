00000001812EFF60 mov       qword ptr [rsp + 0x10], rbx
00000001812EFF65 mov       qword ptr [rsp + 0x18], rbp
00000001812EFF6A push      rsi
00000001812EFF6B sub       rsp, 0x20
00000001812EFF6F mov       rbx, r8
00000001812EFF72 mov       esi, edx
00000001812EFF74 mov       rbp, rcx
00000001812EFF77 test      r8, r8
00000001812EFF7A je        0x1812effcd
00000001812EFF7C cmp       dword ptr [r8 + 0x18], 0
00000001812EFF81 je        0x1812effd3
00000001812EFF83 mov       qword ptr [rsp + 0x30], rdi
00000001812EFF88 mov       rax, qword ptr [rip + 0x9fa9f9] # 0x181cea988
00000001812EFF8F mov       edi, dword ptr [r8 + 0x18]
00000001812EFF93 test      rax, rax
00000001812EFF96 jne       0x1812effab
00000001812EFF98 lea       rcx, [rip + 0x32dc71] # 0x18161dc10
00000001812EFF9F call      0x1802501b0
00000001812EFFA4 mov       qword ptr [rip + 0x9fa9dd], rax # 0x181cea988
00000001812EFFAB mov       r9d, edi
00000001812EFFAE mov       r8, rbx
00000001812EFFB1 mov       edx, esi
00000001812EFFB3 mov       rcx, rbp
00000001812EFFB6 mov       rdi, qword ptr [rsp + 0x30]
00000001812EFFBB mov       rbx, qword ptr [rsp + 0x38]
00000001812EFFC0 mov       rbp, qword ptr [rsp + 0x40]
00000001812EFFC5 add       rsp, 0x20
00000001812EFFC9 pop       rsi
00000001812EFFCA jmp       rax
00000001812EFFCD call      0x180250150
00000001812EFFD2 int3      
00000001812EFFD3 lea       rcx, [rip + 0x8df446] # 0x181bcf420
00000001812EFFDA call      0x18024ff10
00000001812EFFDF mov       rcx, rax
00000001812EFFE2 call      0x180250100
00000001812EFFE7 lea       rcx, [rip + 0x8dc8d2] # 0x181bcc8c0
00000001812EFFEE mov       rbx, rax
00000001812EFFF1 call      0x18024ff10
00000001812EFFF6 mov       rdx, rax
00000001812EFFF9 xor       r8d, r8d
00000001812EFFFC mov       rcx, rbx
00000001812EFFFF call      0x180b80360
00000001812F0004 lea       rcx, [rip + 0x8bfbd5] # 0x181bafbe0
00000001812F000B call      0x18024ff10
00000001812F0010 mov       rdx, rax
00000001812F0013 mov       rcx, rbx
00000001812F0016 call      0x180250110
00000001812F001B int3      
00000001812F001C int3      
00000001812F001D int3      
00000001812F001E int3      
00000001812F001F int3      