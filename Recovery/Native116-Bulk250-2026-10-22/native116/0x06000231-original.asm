000000018033AAD0 push      rbx
000000018033AAD2 push      rsi
000000018033AAD3 push      rdi
000000018033AAD4 sub       rsp, 0x50
000000018033AAD8 cmp       byte ptr [rip + 0x19aa14a], 0 # 0x181ce4c29
000000018033AADF jne       0x18033ab60
000000018033AAE1 lea       rcx, [rip + 0x1864160] # 0x181b9ec48
000000018033AAE8 call      0x18024fef0
000000018033AAED lea       rcx, [rip + 0x186a3fc] # 0x181ba4ef0
000000018033AAF4 call      0x18024fef0
000000018033AAF9 lea       rcx, [rip + 0x186a750] # 0x181ba5250
000000018033AB00 call      0x18024fef0
000000018033AB05 lea       rcx, [rip + 0x1888e44] # 0x181bc3950
000000018033AB0C call      0x18024fef0
000000018033AB11 lea       rcx, [rip + 0x1888de0] # 0x181bc38f8
000000018033AB18 call      0x18024fef0
000000018033AB1D lea       rcx, [rip + 0x188740c] # 0x181bc1f30
000000018033AB24 call      0x18024fef0
000000018033AB29 lea       rcx, [rip + 0x187b948] # 0x181bb6478
000000018033AB30 call      0x18024fef0
000000018033AB35 lea       rcx, [rip + 0x18851b4] # 0x181bbfcf0
000000018033AB3C call      0x18024fef0
000000018033AB41 lea       rcx, [rip + 0x18694e8] # 0x181ba4030
000000018033AB48 call      0x18024fef0
000000018033AB4D lea       rcx, [rip + 0x1889bd4] # 0x181bc4728
000000018033AB54 call      0x18024fef0
000000018033AB59 mov       byte ptr [rip + 0x19aa0c9], 1 # 0x181ce4c29
000000018033AB60 xor       esi, esi
000000018033AB62 mov       qword ptr [rsp + 0x80], rsi
000000018033AB6A mov       rcx, qword ptr [rip + 0x18873bf] # 0x181bc1f30
000000018033AB71 call      0x180250100
000000018033AB76 mov       rdi, rax
000000018033AB79 mov       rdx, qword ptr [rip + 0x1888d78] # 0x181bc38f8
000000018033AB80 mov       rcx, rax
000000018033AB83 call      0x1808053d0
000000018033AB88 mov       qword ptr [rsp + 0x88], rdi
000000018033AB90 mov       rbx, qword ptr [rip + 0x1869499] # 0x181ba4030
000000018033AB97 mov       rcx, qword ptr [rip + 0x1885152] # 0x181bbfcf0
000000018033AB9E cmp       dword ptr [rcx + 0xe0], esi
000000018033ABA4 jne       0x18033abab
000000018033ABA6 call      0x1802501e0
000000018033ABAB xor       edx, edx
000000018033ABAD mov       rcx, rbx
000000018033ABB0 call      0x180ccee30
000000018033ABB5 mov       rbx, rax
000000018033ABB8 mov       rcx, qword ptr [rip + 0x1864089] # 0x181b9ec48
000000018033ABBF cmp       dword ptr [rcx + 0xe0], esi
000000018033ABC5 jne       0x18033abcc
000000018033ABC7 call      0x1802501e0
000000018033ABCC xor       edx, edx
000000018033ABCE mov       rcx, rbx
000000018033ABD1 call      0x180cdd3b0
000000018033ABD6 test      rax, rax
000000018033ABD9 je        0x18033adfa
000000018033ABDF xor       edx, edx
000000018033ABE1 mov       rcx, rax
000000018033ABE4 call      0x180cba170
000000018033ABE9 mov       qword ptr [rsp + 0x78], rax
000000018033ABEE lea       rax, [rsp + 0x78]
000000018033ABF3 mov       qword ptr [rsp + 0x28], rax
000000018033ABF8 lea       rax, [rsp + 0x80]
000000018033AC00 mov       qword ptr [rsp + 0x30], rax
000000018033AC05 mov       qword ptr [rsp + 0x38], rsi
000000018033AC0A movups    xmm0, xmmword ptr [rsp + 0x28]
000000018033AC0F movups    xmmword ptr [rsp + 0x40], xmm0
000000018033AC14 nop       dword ptr [rax]
000000018033AC18 nop       dword ptr [rax + rax]
000000018033AC20 mov       r8, qword ptr [rsp + 0x78]
000000018033AC25 test      r8, r8
000000018033AC28 je        0x18033ae22
000000018033AC2E xor       ecx, ecx
000000018033AC30 mov       rdx, qword ptr [rip + 0x186a619] # 0x181ba5250
000000018033AC37 call      0x180002cd0
000000018033AC3C test      al, al
000000018033AC3E je        0x18033ad95
000000018033AC44 mov       rbx, qword ptr [rsp + 0x78]
000000018033AC49 test      rbx, rbx
000000018033AC4C je        0x18033ae1d
000000018033AC52 mov       r9, qword ptr [rip + 0x186a5f7] # 0x181ba5250
000000018033AC59 mov       r10, qword ptr [rbx]
000000018033AC5C movzx     ecx, si
000000018033AC5F movzx     edx, word ptr [r10 + 0x12e]
000000018033AC67 cmp       si, dx
000000018033AC6A jae       0x18033ac98
000000018033AC6C mov       r8, qword ptr [r10 + 0xb0]
000000018033AC73 nop       dword ptr [rax]
000000018033AC77 nop       word ptr [rax + rax]
000000018033AC80 movzx     eax, cx
000000018033AC83 add       rax, rax
000000018033AC86 cmp       qword ptr [r8 + rax*8], r9
000000018033AC8A je        0x18033ad4a
000000018033AC90 inc       cx
000000018033AC93 cmp       cx, dx
000000018033AC96 jb        0x18033ac80
000000018033AC98 mov       r8d, 1
000000018033AC9E mov       rdx, r9
000000018033ACA1 mov       rcx, rbx
000000018033ACA4 call      0x180258380
000000018033ACA9 mov       rdx, rax
000000018033ACAC mov       rax, qword ptr [rdx]
000000018033ACAF mov       rdx, qword ptr [rdx + 8]
000000018033ACB3 mov       rcx, rbx
000000018033ACB6 call      rax
000000018033ACB8 mov       rcx, rax
000000018033ACBB mov       r8, qword ptr [rip + 0x1889a66] # 0x181bc4728
000000018033ACC2 test      rax, rax
000000018033ACC5 je        0x18033ae18
000000018033ACCB mov       rdx, qword ptr [rax]
000000018033ACCE mov       rax, qword ptr [r8 + 0x40]
000000018033ACD2 cmp       qword ptr [rdx + 0x40], rax
000000018033ACD6 jne       0x18033ae10
000000018033ACDC call      0x18024f540
000000018033ACE1 mov       ebx, dword ptr [rax]
000000018033ACE3 mov       rcx, qword ptr [rip + 0x187b78e] # 0x181bb6478
000000018033ACEA cmp       dword ptr [rcx + 0xe0], 0
000000018033ACF1 jne       0x18033acf8
000000018033ACF3 call      0x1802501e0
000000018033ACF8 xor       edx, edx
000000018033ACFA mov       ecx, ebx
000000018033ACFC call      0x18033ae30
000000018033AD01 mov       rdx, rax
000000018033AD04 test      rdi, rdi
000000018033AD07 je        0x18033ae0b
000000018033AD0D mov       r9, qword ptr [rip + 0x1888c3c] # 0x181bc3950
000000018033AD14 inc       dword ptr [rdi + 0x1c]
000000018033AD17 mov       rcx, qword ptr [rdi + 0x10]
000000018033AD1B movsxd    r8, dword ptr [rdi + 0x18]
000000018033AD1F test      rcx, rcx
000000018033AD22 je        0x18033ae06
000000018033AD28 cmp       r8d, dword ptr [rcx + 0x18]
000000018033AD2C jb        0x18033ad6d
000000018033AD2E mov       rax, qword ptr [r9 + 0x20]
000000018033AD32 mov       r8, qword ptr [rax + 0xc0]
000000018033AD39 mov       r8, qword ptr [r8 + 0x70]
000000018033AD3D mov       rcx, rdi
000000018033AD40 call      0x180847ce0
000000018033AD45 jmp       0x18033ac20
000000018033AD4A movzx     ecx, cx
000000018033AD4D add       rcx, rcx
000000018033AD50 mov       ecx, dword ptr [r8 + rcx*8 + 8]
000000018033AD55 inc       ecx
000000018033AD57 movsxd    rdx, ecx
000000018033AD5A shl       rdx, 4
000000018033AD5E add       rdx, 0x138
000000018033AD65 add       rdx, r10
000000018033AD68 jmp       0x18033acac
000000018033AD6D lea       eax, [r8 + 1]
000000018033AD71 mov       dword ptr [rdi + 0x18], eax
000000018033AD74 cmp       r8d, dword ptr [rcx + 0x18]
000000018033AD78 jae       0x18033ae00
000000018033AD7E mov       qword ptr [rcx + r8*8 + 0x20], rdx
000000018033AD83 lea       rcx, [rcx + r8*8]
000000018033AD87 add       rcx, 0x20
000000018033AD8B call      0x18024f360
000000018033AD90 jmp       0x18033ac20
000000018033AD95 mov       rax, qword ptr [rsp + 0x28]
000000018033AD9A mov       rdx, qword ptr [rip + 0x186a14f] # 0x181ba4ef0
000000018033ADA1 mov       rcx, qword ptr [rax]
000000018033ADA4 call      0x18024f380
000000018033ADA9 mov       r8, rax
000000018033ADAC movups    xmm0, xmmword ptr [rsp + 0x28]
000000018033ADB1 psrldq    xmm0, 8
000000018033ADB6 movq      rax, xmm0
000000018033ADBB mov       qword ptr [rax], r8
000000018033ADBE test      r8, r8
000000018033ADC1 je        0x18033add1
000000018033ADC3 xor       ecx, ecx
000000018033ADC5 mov       rdx, qword ptr [rip + 0x186a124] # 0x181ba4ef0
000000018033ADCC call      0x180002d90
000000018033ADD1 jmp       0x18033adef
000000018033ADD3 lea       rcx, [rsp + 0x40]
000000018033ADD8 call      0x1802fb000
000000018033ADDD mov       rcx, qword ptr [rsp + 0x38]
000000018033ADE2 test      rcx, rcx
000000018033ADE5 jne       0x18033ae28
000000018033ADE7 mov       rdi, qword ptr [rsp + 0x88]
000000018033ADEF mov       rax, rdi
000000018033ADF2 add       rsp, 0x50
000000018033ADF6 pop       rdi
000000018033ADF7 pop       rsi
000000018033ADF8 pop       rbx
000000018033ADF9 ret       
000000018033ADFA call      0x180250150
000000018033ADFF nop       
000000018033AE00 call      0x180250140
000000018033AE05 nop       
000000018033AE06 call      0x180250150
000000018033AE0B call      0x180250150
000000018033AE10 mov       rdx, r8
000000018033AE13 call      0x18024f3a0
000000018033AE18 call      0x180250150
000000018033AE1D call      0x180250150
000000018033AE22 call      0x180250150
000000018033AE27 nop       
000000018033AE28 call      0x180200e60
000000018033AE2D int3      
000000018033AE2E int3      
000000018033AE2F int3      