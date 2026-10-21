000000018039ABB0 mov       qword ptr [rsp + 0x10], rbx
000000018039ABB5 push      rsi
000000018039ABB6 push      rdi
000000018039ABB7 push      r14
000000018039ABB9 sub       rsp, 0x50
000000018039ABBD mov       rsi, rcx
000000018039ABC0 cmp       byte ptr [rip + 0x194a2c3], 0 # 0x181ce4e8a
000000018039ABC7 jne       0x18039ac18
000000018039ABC9 lea       rcx, [rip + 0x18281a0] # 0x181bc2d70
000000018039ABD0 call      0x18024fef0
000000018039ABD5 lea       rcx, [rip + 0x180a314] # 0x181ba4ef0
000000018039ABDC call      0x18024fef0
000000018039ABE1 lea       rcx, [rip + 0x180a668] # 0x181ba5250
000000018039ABE8 call      0x18024fef0
000000018039ABED lea       rcx, [rip + 0x18147c4] # 0x181baf3b8
000000018039ABF4 call      0x18024fef0
000000018039ABF9 lea       rcx, [rip + 0x1815900] # 0x181bb0500
000000018039AC00 call      0x18024fef0
000000018039AC05 lea       rcx, [rip + 0x1824a5c] # 0x181bbf668
000000018039AC0C call      0x18024fef0
000000018039AC11 mov       byte ptr [rip + 0x194a272], 1 # 0x181ce4e8a
000000018039AC18 xor       r14d, r14d
000000018039AC1B mov       qword ptr [rsp + 0x80], r14
000000018039AC23 mov       rcx, qword ptr [rsi + 0x28]
000000018039AC27 test      rcx, rcx
000000018039AC2A je        0x18039ae44
000000018039AC30 xor       edx, edx
000000018039AC32 call      0x18132dc20
000000018039AC37 mov       qword ptr [rsp + 0x70], rax
000000018039AC3C lea       rax, [rsp + 0x70]
000000018039AC41 mov       qword ptr [rsp + 0x28], rax
000000018039AC46 lea       rax, [rsp + 0x80]
000000018039AC4E mov       qword ptr [rsp + 0x30], rax
000000018039AC53 mov       qword ptr [rsp + 0x38], r14
000000018039AC58 movups    xmm0, xmmword ptr [rsp + 0x28]
000000018039AC5D movups    xmmword ptr [rsp + 0x40], xmm0
000000018039AC62 mov       r8, qword ptr [rsp + 0x70]
000000018039AC67 test      r8, r8
000000018039AC6A je        0x18039ae38
000000018039AC70 xor       ecx, ecx
000000018039AC72 mov       rdx, qword ptr [rip + 0x180a5d7] # 0x181ba5250
000000018039AC79 call      0x180002cd0
000000018039AC7E test      al, al
000000018039AC80 je        0x18039adc0
000000018039AC86 mov       rbx, qword ptr [rsp + 0x70]
000000018039AC8B test      rbx, rbx
000000018039AC8E je        0x18039ae33
000000018039AC94 mov       r9, qword ptr [rip + 0x180a5b5] # 0x181ba5250
000000018039AC9B mov       r10, qword ptr [rbx]
000000018039AC9E movzx     ecx, r14w
000000018039ACA2 movzx     edx, word ptr [r10 + 0x12e]
000000018039ACAA cmp       r14w, dx
000000018039ACAE jae       0x18039acd4
000000018039ACB0 mov       r8, qword ptr [r10 + 0xb0]
000000018039ACB7 nop       word ptr [rax + rax]
000000018039ACC0 movzx     eax, cx
000000018039ACC3 add       rax, rax
000000018039ACC6 cmp       qword ptr [r8 + rax*8], r9
000000018039ACCA je        0x18039ad09
000000018039ACCC inc       cx
000000018039ACCF cmp       cx, dx
000000018039ACD2 jb        0x18039acc0
000000018039ACD4 mov       r8d, 1
000000018039ACDA mov       rdx, r9
000000018039ACDD mov       rcx, rbx
000000018039ACE0 call      0x180258380
000000018039ACE5 mov       rdx, rax
000000018039ACE8 mov       rax, qword ptr [rdx]
000000018039ACEB mov       rdx, qword ptr [rdx + 8]
000000018039ACEF mov       rcx, rbx
000000018039ACF2 call      rax
000000018039ACF4 mov       rbx, rax
000000018039ACF7 mov       rdx, qword ptr [rip + 0x182496a] # 0x181bbf668
000000018039ACFE test      rax, rax
000000018039AD01 je        0x18039ae20
000000018039AD07 jmp       0x18039ad29
000000018039AD09 movzx     ecx, cx
000000018039AD0C add       rcx, rcx
000000018039AD0F mov       ecx, dword ptr [r8 + rcx*8 + 8]
000000018039AD14 inc       ecx
000000018039AD16 movsxd    rdx, ecx
000000018039AD19 shl       rdx, 4
000000018039AD1D add       rdx, 0x138
000000018039AD24 add       rdx, r10
000000018039AD27 jmp       0x18039ace8
000000018039AD29 mov       r8, qword ptr [rax]
000000018039AD2C movzx     eax, byte ptr [rdx + 0x130]
000000018039AD33 cmp       byte ptr [r8 + 0x130], al
000000018039AD3A jb        0x18039ae2b
000000018039AD40 movzx     ecx, al
000000018039AD43 mov       rax, qword ptr [r8 + 0xc8]
000000018039AD4A cmp       qword ptr [rax + rcx*8 - 8], rdx
000000018039AD4F jne       0x18039ae2b
000000018039AD55 mov       rdx, qword ptr [rip + 0x1828014] # 0x181bc2d70
000000018039AD5C mov       rcx, rbx
000000018039AD5F call      0x18042e9f0
000000018039AD64 mov       rdi, rax
000000018039AD67 mov       rcx, qword ptr [rip + 0x1815792] # 0x181bb0500
000000018039AD6E cmp       dword ptr [rcx + 0xe0], 0
000000018039AD75 jne       0x18039ad7c
000000018039AD77 call      0x1802501e0
000000018039AD7C xor       r8d, r8d
000000018039AD7F xor       edx, edx
000000018039AD81 mov       rcx, rdi
000000018039AD84 call      0x18131f900
000000018039AD89 test      al, al
000000018039AD8B je        0x18039ac62
000000018039AD91 mov       rdi, qword ptr [rsi + 0x30]
000000018039AD95 mov       rdx, qword ptr [rip + 0x1827fd4] # 0x181bc2d70
000000018039AD9C mov       rcx, rbx
000000018039AD9F call      0x18042e9f0
000000018039ADA4 test      rdi, rdi
000000018039ADA7 je        0x18039ae26
000000018039ADA9 mov       r8, qword ptr [rip + 0x1814608] # 0x181baf3b8
000000018039ADB0 mov       rdx, rax
000000018039ADB3 mov       rcx, rdi
000000018039ADB6 call      0x180002e70
000000018039ADBB jmp       0x18039ac62
000000018039ADC0 mov       rax, qword ptr [rsp + 0x28]
000000018039ADC5 mov       rdx, qword ptr [rip + 0x180a124] # 0x181ba4ef0
000000018039ADCC mov       rcx, qword ptr [rax]
000000018039ADCF call      0x18024f380
000000018039ADD4 mov       r8, rax
000000018039ADD7 movups    xmm0, xmmword ptr [rsp + 0x28]
000000018039ADDC psrldq    xmm0, 8
000000018039ADE1 movq      rax, xmm0
000000018039ADE6 mov       qword ptr [rax], r8
000000018039ADE9 test      r8, r8
000000018039ADEC je        0x18039adfc
000000018039ADEE xor       ecx, ecx
000000018039ADF0 mov       rdx, qword ptr [rip + 0x180a0f9] # 0x181ba4ef0
000000018039ADF7 call      0x180002d90
000000018039ADFC jmp       0x18039ae12
000000018039ADFE lea       rcx, [rsp + 0x40]
000000018039AE03 call      0x1802fb000
000000018039AE08 mov       rcx, qword ptr [rsp + 0x38]
000000018039AE0D test      rcx, rcx
000000018039AE10 jne       0x18039ae3e
000000018039AE12 mov       rbx, qword ptr [rsp + 0x78]
000000018039AE17 add       rsp, 0x50
000000018039AE1B pop       r14
000000018039AE1D pop       rdi
000000018039AE1E pop       rsi
000000018039AE1F ret       
000000018039AE20 call      0x180250150
000000018039AE25 nop       
000000018039AE26 call      0x180250150
000000018039AE2B mov       rcx, rbx
000000018039AE2E call      0x18024f3a0
000000018039AE33 call      0x180250150
000000018039AE38 call      0x180250150
000000018039AE3D nop       
000000018039AE3E call      0x180200e60
000000018039AE43 int3      
000000018039AE44 call      0x180250150
000000018039AE49 int3      