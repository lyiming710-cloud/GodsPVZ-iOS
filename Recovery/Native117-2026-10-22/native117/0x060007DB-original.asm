00000001803C00B0 push      rsi
00000001803C00B2 push      rdi
00000001803C00B3 sub       rsp, 0x38
00000001803C00B7 cmp       byte ptr [rip + 0x1924ede], 0 # 0x181ce4f9c
00000001803C00BE mov       rdi, rcx
00000001803C00C1 jne       0x1803c00d6
00000001803C00C3 lea       rcx, [rip + 0x18038f6] # 0x181bc39c0
00000001803C00CA call      0x18024fef0
00000001803C00CF mov       byte ptr [rip + 0x1924ec6], 1 # 0x181ce4f9c
00000001803C00D6 mov       ecx, dword ptr [rdi + 0x10]
00000001803C00D9 mov       rsi, qword ptr [rdi + 0x20]
00000001803C00DD mov       qword ptr [rsp + 0x58], rbx
00000001803C00E2 mov       qword ptr [rsp + 0x60], rbp
00000001803C00E7 mov       qword ptr [rsp + 0x30], r14
00000001803C00EC test      ecx, ecx
00000001803C00EE je        0x1803c010d
00000001803C00F0 sub       ecx, 1
00000001803C00F3 je        0x1803c0101
00000001803C00F5 cmp       ecx, 1
00000001803C00F8 je        0x1803c0101
00000001803C00FA xor       al, al
00000001803C00FC jmp       0x1803c035c
00000001803C0101 mov       dword ptr [rdi + 0x10], 0xffffffff
00000001803C0108 jmp       0x1803c01a0
00000001803C010D mov       dword ptr [rdi + 0x10], 0xffffffff
00000001803C0114 test      rsi, rsi
00000001803C0117 je        0x1803c03ad
00000001803C011D mov       rcx, qword ptr [rsi + 0x20]
00000001803C0121 test      rcx, rcx
00000001803C0124 je        0x1803c03ad
00000001803C012A mov       rax, qword ptr [rcx]
00000001803C012D xor       r8d, r8d
00000001803C0130 xor       edx, edx
00000001803C0132 mov       r9, qword ptr [rax + 0x7e0]
00000001803C0139 call      qword ptr [rax + 0x7d8]
00000001803C013F mov       rdx, qword ptr [rsi + 0x20]
00000001803C0143 test      rdx, rdx
00000001803C0146 je        0x1803c03ad
00000001803C014C mov       rdx, qword ptr [rdx + 0x370]
00000001803C0153 lea       rcx, [rdi + 0x28]
00000001803C0157 mov       qword ptr [rdi + 0x28], rdx
00000001803C015B call      0x18024f360
00000001803C0160 mov       dword ptr [rdi + 0x30], 0
00000001803C0167 mov       rdx, qword ptr [rsi + 0x20]
00000001803C016B test      rdx, rdx
00000001803C016E je        0x1803c03ad
00000001803C0174 mov       r8, qword ptr [rdx]
00000001803C0177 lea       rcx, [rsp + 0x20]
00000001803C017C mov       rax, qword ptr [r8 + 0x298]
00000001803C0183 mov       r8, qword ptr [r8 + 0x2a0]
00000001803C018A call      rax
00000001803C018C xor       edx, edx
00000001803C018E lea       rcx, [rsp + 0x20]
00000001803C0193 movups    xmm0, xmmword ptr [rax]
00000001803C0196 movaps    xmmword ptr [rsp + 0x20], xmm0
00000001803C019B call      0x18039bf00
00000001803C01A0 mov       rax, qword ptr [rdi + 0x28]
00000001803C01A4 test      rax, rax
00000001803C01A7 je        0x1803c03ad
00000001803C01AD mov       r14d, dword ptr [rax + 0x18]
00000001803C01B1 test      r14d, r14d
00000001803C01B4 je        0x1803c0372
00000001803C01BA mov       rdx, qword ptr [rax + 0x38]
00000001803C01BE test      rdx, rdx
00000001803C01C1 je        0x1803c03ad
00000001803C01C7 movsxd    rcx, dword ptr [rdi + 0x30]
00000001803C01CB cmp       ecx, dword ptr [rdx + 0x18]
00000001803C01CE jae       0x1803c03b3
00000001803C01D4 mov       rbx, qword ptr [rax + 0x60]
00000001803C01D8 test      rbx, rbx
00000001803C01DB je        0x1803c03ad
00000001803C01E1 imul      rcx, rcx, 0x178
00000001803C01E8 movsxd    rax, dword ptr [rcx + rdx + 0x58]
00000001803C01ED cmp       eax, dword ptr [rbx + 0x18]
00000001803C01F0 jae       0x1803c03b3
00000001803C01F6 lea       rcx, [rax + rax*4]
00000001803C01FA mov       rax, qword ptr [rdi + 0x28]
00000001803C01FE add       rcx, rcx
00000001803C0201 mov       rbp, qword ptr [rax + 0x38]
00000001803C0205 mov       rbx, qword ptr [rbx + rcx*8 + 0x58]
00000001803C020A test      rbp, rbp
00000001803C020D je        0x1803c03ad
00000001803C0213 movsxd    rax, dword ptr [rdi + 0x30]
00000001803C0217 cmp       eax, dword ptr [rbp + 0x18]
00000001803C021A jae       0x1803c03b3
00000001803C0220 imul      rcx, rax, 0x178
00000001803C0227 mov       rax, qword ptr [rdi + 0x28]
00000001803C022B mov       rdx, qword ptr [rax + 0x38]
00000001803C022F movsxd    rbp, dword ptr [rcx + rbp + 0x6c]
00000001803C0234 test      rdx, rdx
00000001803C0237 je        0x1803c03ad
00000001803C023D movsxd    rax, dword ptr [rdi + 0x30]
00000001803C0241 cmp       eax, dword ptr [rdx + 0x18]
00000001803C0244 jae       0x1803c03b3
00000001803C024A imul      rcx, rax, 0x178
00000001803C0251 cmp       byte ptr [rcx + rdx + 0x194], 0
00000001803C0259 je        0x1803c0315
00000001803C025F xor       r8d, r8d
00000001803C0262 mov       edx, 0xff
00000001803C0267 xor       ecx, ecx
00000001803C0269 call      0x18130d310
00000001803C026E xor       r8d, r8d
00000001803C0271 mov       byte ptr [rsp + 0x50], al
00000001803C0275 mov       edx, 0xff
00000001803C027A xor       ecx, ecx
00000001803C027C call      0x18130d310
00000001803C0281 xor       r8d, r8d
00000001803C0284 mov       byte ptr [rsp + 0x51], al
00000001803C0288 mov       edx, 0xff
00000001803C028D xor       ecx, ecx
00000001803C028F call      0x18130d310
00000001803C0294 mov       byte ptr [rsp + 0x52], al
00000001803C0298 mov       byte ptr [rsp + 0x53], 0xff
00000001803C029D test      rbx, rbx
00000001803C02A0 je        0x1803c03ad
00000001803C02A6 cmp       ebp, dword ptr [rbx + 0x18]
00000001803C02A9 jae       0x1803c03b3
00000001803C02AF mov       eax, dword ptr [rsp + 0x50]
00000001803C02B3 lea       rcx, [rbp + 1]
00000001803C02B7 mov       dword ptr [rbx + rbp*4 + 0x20], eax
00000001803C02BB cmp       ecx, dword ptr [rbx + 0x18]
00000001803C02BE jae       0x1803c03b3
00000001803C02C4 mov       dword ptr [rbx + rcx*4 + 0x20], eax
00000001803C02C8 lea       rcx, [rbp + 2]
00000001803C02CC cmp       ecx, dword ptr [rbx + 0x18]
00000001803C02CF jae       0x1803c03b3
00000001803C02D5 mov       dword ptr [rbx + rcx*4 + 0x20], eax
00000001803C02D9 lea       rcx, [rbp + 3]
00000001803C02DD cmp       ecx, dword ptr [rbx + 0x18]
00000001803C02E0 jae       0x1803c03b3
00000001803C02E6 mov       dword ptr [rbx + rcx*4 + 0x20], eax
00000001803C02EA test      rsi, rsi
00000001803C02ED je        0x1803c03ad
00000001803C02F3 mov       rcx, qword ptr [rsi + 0x20]
00000001803C02F7 test      rcx, rcx
00000001803C02FA je        0x1803c03ad
00000001803C0300 mov       rax, qword ptr [rcx]
00000001803C0303 mov       edx, 0x10
00000001803C0308 mov       r8, qword ptr [rax + 0x800]
00000001803C030F call      qword ptr [rax + 0x7f8]
00000001803C0315 mov       eax, dword ptr [rdi + 0x30]
00000001803C0318 inc       eax
00000001803C031A cdq       
00000001803C031B idiv      r14d
00000001803C031E mov       dword ptr [rdi + 0x30], edx
00000001803C0321 mov       rcx, qword ptr [rip + 0x1803698] # 0x181bc39c0
00000001803C0328 call      0x180250100
00000001803C032D movss     xmm1, dword ptr [rip + 0x11e776f] # 0x1815a7aa4
00000001803C0335 xor       r8d, r8d
00000001803C0338 mov       rcx, rax
00000001803C033B mov       rbx, rax
00000001803C033E call      0x181333710
00000001803C0343 lea       rcx, [rdi + 0x18]
00000001803C0347 mov       qword ptr [rdi + 0x18], rbx
00000001803C034B mov       rdx, rbx
00000001803C034E call      0x18024f360
00000001803C0353 mov       dword ptr [rdi + 0x10], 2
00000001803C035A mov       al, 1
00000001803C035C mov       r14, qword ptr [rsp + 0x30]
00000001803C0361 mov       rbp, qword ptr [rsp + 0x60]
00000001803C0366 mov       rbx, qword ptr [rsp + 0x58]
00000001803C036B add       rsp, 0x38
00000001803C036F pop       rdi
00000001803C0370 pop       rsi
00000001803C0371 ret       
00000001803C0372 mov       rcx, qword ptr [rip + 0x1803647] # 0x181bc39c0
00000001803C0379 call      0x180250100
00000001803C037E movss     xmm1, dword ptr [rip + 0x11e772a] # 0x1815a7ab0
00000001803C0386 xor       r8d, r8d
00000001803C0389 mov       rcx, rax
00000001803C038C mov       rbx, rax
00000001803C038F call      0x181333710
00000001803C0394 lea       rcx, [rdi + 0x18]
00000001803C0398 mov       qword ptr [rdi + 0x18], rbx
00000001803C039C mov       rdx, rbx
00000001803C039F call      0x18024f360
00000001803C03A4 mov       dword ptr [rdi + 0x10], 1
00000001803C03AB jmp       0x1803c035a
00000001803C03AD call      0x180250150
00000001803C03B2 int3      
00000001803C03B3 call      0x180250140
00000001803C03B8 int3      
00000001803C03B9 int3      
00000001803C03BA int3      
00000001803C03BB int3      
00000001803C03BC int3      
00000001803C03BD int3      
00000001803C03BE int3      
00000001803C03BF int3      