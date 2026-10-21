000000018034ACC0 push      rdi
000000018034ACC2 sub       rsp, 0x60
000000018034ACC6 cmp       byte ptr [rip + 0x199a010], 0 # 0x181ce4cdd
000000018034ACCD mov       rdi, rcx
000000018034ACD0 jne       0x18034ace5
000000018034ACD2 lea       rcx, [rip + 0x1865827] # 0x181bb0500
000000018034ACD9 call      0x18024fef0
000000018034ACDE mov       byte ptr [rip + 0x1999ff8], 1 # 0x181ce4cdd
000000018034ACE5 mov       rcx, qword ptr [rdi + 0x120]
000000018034ACEC mov       qword ptr [rsp + 0x70], rbx
000000018034ACF1 mov       qword ptr [rsp + 0x80], rsi
000000018034ACF9 movaps    xmmword ptr [rsp + 0x50], xmm6
000000018034ACFE movaps    xmmword ptr [rsp + 0x40], xmm7
000000018034AD03 movaps    xmmword ptr [rsp + 0x30], xmm8
000000018034AD09 test      rcx, rcx
000000018034AD0C je        0x18034afcd
000000018034AD12 xor       r8d, r8d
000000018034AD15 mov       rdx, rdi
000000018034AD18 call      0x18034cd30
000000018034AD1D mov       rsi, qword ptr [rdi + 0x118]
000000018034AD24 movss     xmm7, dword ptr [rdi + 0x88]
000000018034AD2C movss     xmm8, dword ptr [rdi + 0x8c]
000000018034AD35 test      rsi, rsi
000000018034AD38 je        0x18034afcd
000000018034AD3E cmp       byte ptr [rip + 0x1999fa1], 0 # 0x181ce4ce6
000000018034AD45 jne       0x18034ad5a
000000018034AD47 lea       rcx, [rip + 0x18657b2] # 0x181bb0500
000000018034AD4E call      0x18024fef0
000000018034AD53 mov       byte ptr [rip + 0x1999f8c], 1 # 0x181ce4ce6
000000018034AD5A mov       rcx, qword ptr [rip + 0x186579f] # 0x181bb0500
000000018034AD61 mov       rbx, qword ptr [rsi + 0x10]
000000018034AD65 cmp       dword ptr [rcx + 0xe0], 0
000000018034AD6C jne       0x18034ad73
000000018034AD6E call      0x1802501e0
000000018034AD73 xor       r8d, r8d
000000018034AD76 xor       edx, edx
000000018034AD78 mov       rcx, rbx
000000018034AD7B call      0x18131f760
000000018034AD80 xor       ebx, ebx
000000018034AD82 test      al, al
000000018034AD84 jne       0x18034ae27
000000018034AD8A mov       rcx, qword ptr [rsi + 0x10]
000000018034AD8E xorps     xmm6, xmm6
000000018034AD91 comiss    xmm6, xmm7
000000018034AD94 jae       0x18034ae14
000000018034AD96 test      rcx, rcx
000000018034AD99 je        0x18034afcd
000000018034AD9F xor       r8d, r8d
000000018034ADA2 mov       qword ptr [rsp + 0x78], rbp
000000018034ADA7 mov       dl, 1
000000018034ADA9 call      0x18131bad0
000000018034ADAE mov       rcx, qword ptr [rip + 0x186574b] # 0x181bb0500
000000018034ADB5 mov       rbp, qword ptr [rdi + 0xc8]
000000018034ADBC cmp       dword ptr [rcx + 0xe0], ebx
000000018034ADC2 jne       0x18034adc9
000000018034ADC4 call      0x1802501e0
000000018034ADC9 xor       edx, edx
000000018034ADCB mov       rcx, rbp
000000018034ADCE call      0x18131f870
000000018034ADD3 mov       rbp, qword ptr [rsp + 0x78]
000000018034ADD8 test      al, al
000000018034ADDA je        0x18034adf7
000000018034ADDC mov       rcx, qword ptr [rdi + 0xc8]
000000018034ADE3 test      rcx, rcx
000000018034ADE6 je        0x18034afcd
000000018034ADEC xor       r8d, r8d
000000018034ADEF xorps     xmm1, xmm1
000000018034ADF2 call      0x1812dbcd0
000000018034ADF7 mov       qword ptr [rsp + 0x28], rbx
000000018034ADFC mov       r9, rdi
000000018034ADFF movaps    xmm2, xmm8
000000018034AE03 mov       byte ptr [rsp + 0x20], bl
000000018034AE07 movaps    xmm1, xmm7
000000018034AE0A mov       rcx, rsi
000000018034AE0D call      0x18034d610
000000018034AE12 jmp       0x18034ae27
000000018034AE14 test      rcx, rcx
000000018034AE17 je        0x18034afcd
000000018034AE1D xor       r8d, r8d
000000018034AE20 xor       edx, edx
000000018034AE22 call      0x18131bad0
000000018034AE27 mov       rcx, qword ptr [rip + 0x18656d2] # 0x181bb0500
000000018034AE2E mov       rsi, qword ptr [rdi + 0x28]
000000018034AE32 cmp       dword ptr [rcx + 0xe0], ebx
000000018034AE38 jne       0x18034ae3f
000000018034AE3A call      0x1802501e0
000000018034AE3F xor       r8d, r8d
000000018034AE42 xor       edx, edx
000000018034AE44 mov       rcx, rsi
000000018034AE47 call      0x18131f760
000000018034AE4C test      al, al
000000018034AE4E jne       0x18034afaa
000000018034AE54 mov       rax, qword ptr [rdi + 0x108]
000000018034AE5B mov       ecx, ebx
000000018034AE5D test      rax, rax
000000018034AE60 je        0x18034afcd
000000018034AE66 cmp       ecx, dword ptr [rax + 0x18]
000000018034AE69 jge       0x18034af3b
000000018034AE6F mov       rcx, qword ptr [rdi + 0x108]
000000018034AE76 test      rcx, rcx
000000018034AE79 je        0x18034afcd
000000018034AE7F cmp       ebx, dword ptr [rcx + 0x18]
000000018034AE82 jae       0x18034afd3
000000018034AE88 movsxd    rax, ebx
000000018034AE8B mov       rsi, qword ptr [rcx + rax*8 + 0x20]
000000018034AE90 mov       rcx, qword ptr [rip + 0x1865669] # 0x181bb0500
000000018034AE97 cmp       dword ptr [rcx + 0xe0], 0
000000018034AE9E jne       0x18034aea5
000000018034AEA0 call      0x1802501e0
000000018034AEA5 xor       r8d, r8d
000000018034AEA8 xor       edx, edx
000000018034AEAA mov       rcx, rsi
000000018034AEAD call      0x18131f900
000000018034AEB2 test      al, al
000000018034AEB4 je        0x18034af22
000000018034AEB6 mov       rcx, qword ptr [rdi + 0x108]
000000018034AEBD test      rcx, rcx
000000018034AEC0 je        0x18034afcd
000000018034AEC6 cmp       ebx, dword ptr [rcx + 0x18]
000000018034AEC9 jae       0x18034afd3
000000018034AECF movsxd    rax, ebx
000000018034AED2 mov       rcx, qword ptr [rcx + rax*8 + 0x20]
000000018034AED7 test      rcx, rcx
000000018034AEDA je        0x18034afcd
000000018034AEE0 xor       edx, edx
000000018034AEE2 call      0x18131c070
000000018034AEE7 mov       rcx, qword ptr [rdi + 0x28]
000000018034AEEB mov       rsi, rax
000000018034AEEE test      rcx, rcx
000000018034AEF1 je        0x18034afcd
000000018034AEF7 mov       rcx, qword ptr [rcx + 0x78]
000000018034AEFB test      rcx, rcx
000000018034AEFE je        0x18034afcd
000000018034AF04 xor       edx, edx
000000018034AF06 call      0x18131c070
000000018034AF0B test      rsi, rsi
000000018034AF0E je        0x18034afcd
000000018034AF14 xor       r8d, r8d
000000018034AF17 mov       rdx, rax
000000018034AF1A mov       rcx, rsi
000000018034AF1D call      0x18132e8b0
000000018034AF22 mov       rax, qword ptr [rdi + 0x108]
000000018034AF29 inc       ebx
000000018034AF2B mov       ecx, ebx
000000018034AF2D test      rax, rax
000000018034AF30 je        0x18034afcd
000000018034AF36 jmp       0x18034ae66
000000018034AF3B mov       rcx, qword ptr [rip + 0x18655be] # 0x181bb0500
000000018034AF42 mov       rbx, qword ptr [rdi + 0x110]
000000018034AF49 cmp       dword ptr [rcx + 0xe0], 0
000000018034AF50 jne       0x18034af57
000000018034AF52 call      0x1802501e0
000000018034AF57 xor       r8d, r8d
000000018034AF5A xor       edx, edx
000000018034AF5C mov       rcx, rbx
000000018034AF5F call      0x18131f900
000000018034AF64 test      al, al
000000018034AF66 je        0x18034afaa
000000018034AF68 mov       rcx, qword ptr [rdi + 0x110]
000000018034AF6F test      rcx, rcx
000000018034AF72 je        0x18034afcd
000000018034AF74 xor       edx, edx
000000018034AF76 call      0x18131c070
000000018034AF7B mov       rcx, qword ptr [rdi + 0x28]
000000018034AF7F mov       rbx, rax
000000018034AF82 test      rcx, rcx
000000018034AF85 je        0x18034afcd
000000018034AF87 mov       rcx, qword ptr [rcx + 0x78]
000000018034AF8B test      rcx, rcx
000000018034AF8E je        0x18034afcd
000000018034AF90 xor       edx, edx
000000018034AF92 call      0x18131c070
000000018034AF97 test      rbx, rbx
000000018034AF9A je        0x18034afcd
000000018034AF9C xor       r8d, r8d
000000018034AF9F mov       rdx, rax
000000018034AFA2 mov       rcx, rbx
000000018034AFA5 call      0x18132e8b0
000000018034AFAA movaps    xmm8, xmmword ptr [rsp + 0x30]
000000018034AFB0 movaps    xmm7, xmmword ptr [rsp + 0x40]
000000018034AFB5 movaps    xmm6, xmmword ptr [rsp + 0x50]
000000018034AFBA mov       rsi, qword ptr [rsp + 0x80]
000000018034AFC2 mov       rbx, qword ptr [rsp + 0x70]
000000018034AFC7 add       rsp, 0x60
000000018034AFCB pop       rdi
000000018034AFCC ret       
000000018034AFCD call      0x180250150
000000018034AFD2 int3      
000000018034AFD3 call      0x180250140
000000018034AFD8 int3      
000000018034AFD9 int3      
000000018034AFDA int3      
000000018034AFDB int3      
000000018034AFDC int3      
000000018034AFDD int3      
000000018034AFDE int3      
000000018034AFDF int3      