000000018032A7D0 mov       qword ptr [rsp + 0x20], r9
000000018032A7D5 mov       qword ptr [rsp + 0x18], r8
000000018032A7DA mov       qword ptr [rsp + 0x10], rdx
000000018032A7DF mov       qword ptr [rsp + 8], rcx
000000018032A7E4 push      rbx
000000018032A7E5 push      rsi
000000018032A7E6 push      rdi
000000018032A7E7 push      r12
000000018032A7E9 push      r13
000000018032A7EB push      r14
000000018032A7ED push      r15
000000018032A7EF sub       rsp, 0x90
000000018032A7F6 mov       r14, r9
000000018032A7F9 mov       rsi, r8
000000018032A7FC mov       r15, rdx
000000018032A7FF mov       qword ptr [rsp + 0x40], rdx
000000018032A804 cmp       byte ptr [rip + 0x19ba45a], 0 # 0x181ce4c65
000000018032A80B jne       0x18032a8a8
000000018032A811 lea       rcx, [rip + 0x189b798] # 0x181bc5fb0
000000018032A818 call      0x18024fef0
000000018032A81D lea       rcx, [rip + 0x189b7e4] # 0x181bc6008
000000018032A824 call      0x18024fef0
000000018032A829 lea       rcx, [rip + 0x189b830] # 0x181bc6060
000000018032A830 call      0x18024fef0
000000018032A835 lea       rcx, [rip + 0x187fc1c] # 0x181baa458
000000018032A83C call      0x18024fef0
000000018032A841 lea       rcx, [rip + 0x1888478] # 0x181bb2cc0
000000018032A848 call      0x18024fef0
000000018032A84D lea       rcx, [rip + 0x187fcb4] # 0x181baa508
000000018032A854 call      0x18024fef0
000000018032A859 lea       rcx, [rip + 0x187fd58] # 0x181baa5b8
000000018032A860 call      0x18024fef0
000000018032A865 lea       rcx, [rip + 0x18883a4] # 0x181bb2c10
000000018032A86C call      0x18024fef0
000000018032A871 lea       rcx, [rip + 0x187fd98] # 0x181baa610
000000018032A878 call      0x18024fef0
000000018032A87D lea       rcx, [rip + 0x1888544] # 0x181bb2dc8
000000018032A884 call      0x18024fef0
000000018032A889 lea       rcx, [rip + 0x1894788] # 0x181bbf018
000000018032A890 call      0x18024fef0
000000018032A895 lea       rcx, [rip + 0x188235c] # 0x181bacbf8
000000018032A89C call      0x18024fef0
000000018032A8A1 mov       byte ptr [rip + 0x19ba3bd], 1 # 0x181ce4c65
000000018032A8A8 cmp       rsi, r14
000000018032A8AB je        0x18032adee
000000018032A8B1 test      rsi, rsi
000000018032A8B4 je        0x18032adee
000000018032A8BA mov       rax, qword ptr [rsp + 0xf0]
000000018032A8C2 test      rax, rax
000000018032A8C5 je        0x18032ae3c
000000018032A8CB mov       r8, qword ptr [rip + 0x18883ee] # 0x181bb2cc0
000000018032A8D2 mov       rdx, rax
000000018032A8D5 lea       rcx, [rsp + 0x58]
000000018032A8DA call      0x1808197f0
000000018032A8DF movups    xmm0, xmmword ptr [rsp + 0x58]
000000018032A8E4 movups    xmmword ptr [rsp + 0x70], xmm0
000000018032A8E9 movsd     xmm1, qword ptr [rsp + 0x68]
000000018032A8EF movsd     qword ptr [rsp + 0x80], xmm1
000000018032A8F8 xor       r13d, r13d
000000018032A8FB mov       qword ptr [rsp + 0x58], r13
000000018032A900 lea       rbx, [rsp + 0x70]
000000018032A905 mov       qword ptr [rsp + 0x60], rbx
000000018032A90A nop       word ptr [rax + rax]
000000018032A910 mov       rdx, qword ptr [rip + 0x189b6f1] # 0x181bc6008
000000018032A917 lea       rcx, [rsp + 0x70]
000000018032A91C call      0x180669280
000000018032A921 test      al, al
000000018032A923 je        0x18032a9ac
000000018032A929 mov       rdi, qword ptr [rsp + 0x80]
000000018032A931 test      rdi, rdi
000000018032A934 je        0x18032ae30
000000018032A93A mov       dword ptr [rdi + 0x1c], 0x7fffffff
000000018032A941 test      r14, r14
000000018032A944 je        0x18032ae2b
000000018032A94A mov       esi, dword ptr [r14 + 0x10]
000000018032A94E test      rdi, rdi
000000018032A951 je        0x18032ae25
000000018032A957 mov       r12d, dword ptr [rdi + 0x10]
000000018032A95B mov       rcx, qword ptr [rip + 0x1882296] # 0x181bacbf8
000000018032A962 cmp       dword ptr [rcx + 0xe0], 0
000000018032A969 jne       0x18032a970
000000018032A96B call      0x1802501e0
000000018032A970 sub       esi, r12d
000000018032A973 mov       edx, esi
000000018032A975 neg       edx
000000018032A977 cmovs     edx, esi
000000018032A97A mov       ecx, dword ptr [r14 + 0x14]
000000018032A97E sub       ecx, dword ptr [rdi + 0x14]
000000018032A981 mov       eax, ecx
000000018032A983 neg       eax
000000018032A985 cmovs     eax, ecx
000000018032A988 add       eax, edx
000000018032A98A mov       dword ptr [rdi + 0x20], eax
000000018032A98D mov       dword ptr [rdi + 0x24], 0x7fffffff
000000018032A994 mov       qword ptr [rdi + 0x30], r13
000000018032A998 lea       rcx, [rdi + 0x30]
000000018032A99C xor       edx, edx
000000018032A99E call      0x18024f360
000000018032A9A3 mov       byte ptr [rdi + 0x28], 0
000000018032A9A7 jmp       0x18032a910
000000018032A9AC mov       rdx, qword ptr [rip + 0x189b5fd] # 0x181bc5fb0
000000018032A9B3 mov       rcx, rbx
000000018032A9B6 call      0x180302170
000000018032A9BB jmp       0x18032a9ef
000000018032A9BD mov       rdx, qword ptr [rip + 0x189b5ec] # 0x181bc5fb0
000000018032A9C4 mov       rcx, qword ptr [rsp + 0x60]
000000018032A9C9 call      0x180302170
000000018032A9CE mov       rcx, qword ptr [rsp + 0x58]
000000018032A9D3 test      rcx, rcx
000000018032A9D6 jne       0x18032ae36
000000018032A9DC xor       r13d, r13d
000000018032A9DF mov       r14, qword ptr [rsp + 0xe8]
000000018032A9E7 mov       r15, qword ptr [rsp + 0xd8]
000000018032A9EF mov       rdi, qword ptr [rsp + 0xe0]
000000018032A9F7 mov       dword ptr [rdi + 0x1c], r13d
000000018032A9FB mov       rcx, qword ptr [rip + 0x1894616] # 0x181bbf018
000000018032AA02 call      0x180250100
000000018032AA07 mov       rbx, rax
000000018032AA0A mov       rdx, qword ptr [rip + 0x18881ff] # 0x181bb2c10
000000018032AA11 mov       rcx, rax
000000018032AA14 call      0x1808053d0
000000018032AA19 mov       qword ptr [rsp + 0xe8], rbx
000000018032AA21 mov       qword ptr [rsp + 0x38], r13
000000018032AA26 mov       eax, dword ptr [rsp + 0x108]
000000018032AA2D mov       dword ptr [rsp + 0x30], eax
000000018032AA31 mov       eax, dword ptr [rsp + 0x100]
000000018032AA38 mov       dword ptr [rsp + 0x28], eax
000000018032AA3C mov       eax, dword ptr [rsp + 0xf8]
000000018032AA43 mov       dword ptr [rsp + 0x20], eax
000000018032AA47 mov       r9, qword ptr [rsp + 0xf0]
000000018032AA4F mov       r8, rdi
000000018032AA52 lea       rdx, [rsp + 0xe8]
000000018032AA5A mov       rcx, qword ptr [rsp + 0xd0]
000000018032AA62 call      0x18032ae50
000000018032AA67 mov       rdi, qword ptr [rsp + 0xe8]
000000018032AA6F test      rdi, rdi
000000018032AA72 je        0x18032ae3c
000000018032AA78 nop       dword ptr [rax + rax]
000000018032AA80 cmp       dword ptr [rdi + 0x18], 0
000000018032AA84 je        0x18032ab84
000000018032AA8A cmp       byte ptr [rip + 0x19ba1eb], 0 # 0x181ce4c7c
000000018032AA91 jne       0x18032aab2
000000018032AA93 lea       rcx, [rip + 0x188832e] # 0x181bb2dc8
000000018032AA9A call      0x18024fef0
000000018032AA9F lea       rcx, [rip + 0x188837a] # 0x181bb2e20
000000018032AAA6 call      0x18024fef0
000000018032AAAB mov       byte ptr [rip + 0x19ba1ca], 1 # 0x181ce4c7c
000000018032AAB2 cmp       dword ptr [rdi + 0x18], 0
000000018032AAB6 jle       0x18032ab25
000000018032AAB8 mov       esi, 0x7fffffff
000000018032AABD mov       r12d, esi
000000018032AAC0 mov       ebx, r13d
000000018032AAC3 cmp       ebx, dword ptr [rdi + 0x18]
000000018032AAC6 jge       0x18032ab0b
000000018032AAC8 mov       r8, qword ptr [rip + 0x1888351] # 0x181bb2e20
000000018032AACF mov       edx, ebx
000000018032AAD1 mov       rcx, rdi
000000018032AAD4 call      0x180825710
000000018032AAD9 test      rax, rax
000000018032AADC je        0x18032ae3c
000000018032AAE2 cmp       dword ptr [rax + 0x24], esi
000000018032AAE5 jge       0x18032ab07
000000018032AAE7 mov       r12d, ebx
000000018032AAEA mov       r8, qword ptr [rip + 0x188832f] # 0x181bb2e20
000000018032AAF1 mov       edx, ebx
000000018032AAF3 mov       rcx, rdi
000000018032AAF6 call      0x180825710
000000018032AAFB test      rax, rax
000000018032AAFE je        0x18032ae3c
000000018032AB04 mov       esi, dword ptr [rax + 0x24]
000000018032AB07 inc       ebx
000000018032AB09 jmp       0x18032aac3
000000018032AB0B cmp       r12d, dword ptr [rdi + 0x18]
000000018032AB0F jge       0x18032ab25
000000018032AB11 mov       r8, qword ptr [rip + 0x1888308] # 0x181bb2e20
000000018032AB18 mov       edx, r12d
000000018032AB1B mov       rcx, rdi
000000018032AB1E call      0x180825710
000000018032AB23 jmp       0x18032ab28
000000018032AB25 mov       rax, r13
000000018032AB28 mov       qword ptr [rsp + 0x38], r13
000000018032AB2D mov       ecx, dword ptr [rsp + 0x108]
000000018032AB34 mov       dword ptr [rsp + 0x30], ecx
000000018032AB38 mov       ecx, dword ptr [rsp + 0x100]
000000018032AB3F mov       dword ptr [rsp + 0x28], ecx
000000018032AB43 mov       ecx, dword ptr [rsp + 0xf8]
000000018032AB4A mov       dword ptr [rsp + 0x20], ecx
000000018032AB4E mov       r9, qword ptr [rsp + 0xf0]
000000018032AB56 mov       r8, rax
000000018032AB59 lea       rdx, [rsp + 0xe8]
000000018032AB61 mov       rcx, qword ptr [rsp + 0xd0]
000000018032AB69 call      0x18032ae50
000000018032AB6E mov       rdi, qword ptr [rsp + 0xe8]
000000018032AB76 test      rdi, rdi
000000018032AB79 je        0x18032ae3c
000000018032AB7F jmp       0x18032aa80
000000018032AB84 test      r14, r14
000000018032AB87 je        0x18032ae3c
000000018032AB8D cmp       qword ptr [rsp + 0x40], 0
000000018032AB93 je        0x18032ae3c
000000018032AB99 cmp       qword ptr [r14 + 0x30], 0
000000018032AB9E je        0x18032adcd
000000018032ABA4 mov       r12d, dword ptr [r15 + 0x18]
000000018032ABA8 nop       dword ptr [rax + rax]
000000018032ABB0 test      r14, r14
000000018032ABB3 je        0x18032ae3c
000000018032ABB9 cmp       byte ptr [rip + 0x19ba0b9], 0 # 0x181ce4c79
000000018032ABC0 jne       0x18032abd5
000000018032ABC2 lea       rcx, [rip + 0x1873c5f] # 0x181b9e828
000000018032ABC9 call      0x18024fef0
000000018032ABCE mov       byte ptr [rip + 0x19ba0a4], 1 # 0x181ce4c79
000000018032ABD5 mov       edi, dword ptr [r14 + 0x10]
000000018032ABD9 mov       esi, dword ptr [r14 + 0x14]
000000018032ABDD mov       rcx, qword ptr [rip + 0x1873c44] # 0x181b9e828
000000018032ABE4 call      0x180250100
000000018032ABE9 mov       rbx, rax
000000018032ABEC xor       edx, edx
000000018032ABEE mov       rcx, rax
000000018032ABF1 call      0x180302170
000000018032ABF6 mov       dword ptr [rbx + 0x10], edi
000000018032ABF9 mov       dword ptr [rbx + 0x14], esi
000000018032ABFC mov       dword ptr [rbx + 0x24], r13d
000000018032AC00 mov       byte ptr [rbx + 0x29], 0
000000018032AC04 mov       rax, qword ptr [rsp + 0x110]
000000018032AC0C test      rax, rax
000000018032AC0F je        0x18032ac3a
000000018032AC11 mov       qword ptr [rsp + 0x20], r13
000000018032AC16 mov       r9d, esi
000000018032AC19 mov       r8d, edi
000000018032AC1C mov       rdx, rax
000000018032AC1F lea       rcx, [rsp + 0x40]
000000018032AC24 call      0x18030ef10
000000018032AC29 mov       ecx, dword ptr [rax + 8]
000000018032AC2C movsd     xmm0, qword ptr [rax]
000000018032AC30 movsd     qword ptr [rbx + 0x18], xmm0
000000018032AC35 mov       dword ptr [rbx + 0x20], ecx
000000018032AC38 jmp       0x18032ac73
000000018032AC3A cmp       byte ptr [rip + 0x19b9ef5], 0 # 0x181ce4b36
000000018032AC41 jne       0x18032ac56
000000018032AC43 lea       rcx, [rip + 0x1897b9e] # 0x181bc27e8
000000018032AC4A call      0x18024fef0
000000018032AC4F mov       byte ptr [rip + 0x19b9ee0], 1 # 0x181ce4b36
000000018032AC56 mov       rax, qword ptr [rip + 0x1897b8b] # 0x181bc27e8
000000018032AC5D mov       rcx, qword ptr [rax + 0xb8]
000000018032AC64 mov       eax, dword ptr [rcx + 8]
000000018032AC67 movsd     xmm0, qword ptr [rcx]
000000018032AC6B movsd     qword ptr [rbx + 0x18], xmm0
000000018032AC70 mov       dword ptr [rbx + 0x20], eax
000000018032AC73 mov       r9, qword ptr [rip + 0x187f88e] # 0x181baa508
000000018032AC7A mov       r8, rbx
000000018032AC7D mov       edx, r12d
000000018032AC80 mov       rcx, r15
000000018032AC83 call      0x18084d9c0
000000018032AC88 mov       r14, qword ptr [r14 + 0x30]
000000018032AC8C mov       rsi, qword ptr [rsp + 0xe0]
000000018032AC94 cmp       r14, rsi
000000018032AC97 jne       0x18032abb0
000000018032AC9D cmp       byte ptr [rip + 0x19b9fd5], 0 # 0x181ce4c79
000000018032ACA4 jne       0x18032acb9
000000018032ACA6 lea       rcx, [rip + 0x1873b7b] # 0x181b9e828
000000018032ACAD call      0x18024fef0
000000018032ACB2 mov       byte ptr [rip + 0x19b9fc0], 1 # 0x181ce4c79
000000018032ACB9 mov       ebx, dword ptr [rsi + 0x10]
000000018032ACBC mov       esi, dword ptr [rsi + 0x14]
000000018032ACBF mov       rcx, qword ptr [rip + 0x1873b62] # 0x181b9e828
000000018032ACC6 call      0x180250100
000000018032ACCB mov       rdi, rax
000000018032ACCE xor       edx, edx
000000018032ACD0 mov       rcx, rax
000000018032ACD3 call      0x180302170
000000018032ACD8 mov       dword ptr [rdi + 0x10], ebx
000000018032ACDB mov       dword ptr [rdi + 0x14], esi
000000018032ACDE mov       dword ptr [rdi + 0x24], r13d
000000018032ACE2 mov       byte ptr [rdi + 0x29], 0
000000018032ACE6 mov       rdx, qword ptr [rsp + 0x110]
000000018032ACEE test      rdx, rdx
000000018032ACF1 je        0x18032ad19
000000018032ACF3 mov       qword ptr [rsp + 0x20], r13
000000018032ACF8 mov       r9d, esi
000000018032ACFB mov       r8d, ebx
000000018032ACFE lea       rcx, [rsp + 0x40]
000000018032AD03 call      0x18030ef10
000000018032AD08 mov       ecx, dword ptr [rax + 8]
000000018032AD0B movsd     xmm0, qword ptr [rax]
000000018032AD0F movsd     qword ptr [rdi + 0x18], xmm0
000000018032AD14 mov       dword ptr [rdi + 0x20], ecx
000000018032AD17 jmp       0x18032ad52
000000018032AD19 cmp       byte ptr [rip + 0x19b9e16], 0 # 0x181ce4b36
000000018032AD20 jne       0x18032ad35
000000018032AD22 lea       rcx, [rip + 0x1897abf] # 0x181bc27e8
000000018032AD29 call      0x18024fef0
000000018032AD2E mov       byte ptr [rip + 0x19b9e01], 1 # 0x181ce4b36
000000018032AD35 mov       rax, qword ptr [rip + 0x1897aac] # 0x181bc27e8
000000018032AD3C mov       rcx, qword ptr [rax + 0xb8]
000000018032AD43 mov       eax, dword ptr [rcx + 8]
000000018032AD46 movsd     xmm0, qword ptr [rcx]
000000018032AD4A movsd     qword ptr [rdi + 0x18], xmm0
000000018032AD4F mov       dword ptr [rdi + 0x20], eax
000000018032AD52 mov       ebx, dword ptr [r15 + 0x18]
000000018032AD56 sub       ebx, 2
000000018032AD59 cmp       ebx, r12d
000000018032AD5C jl        0x18032adba
000000018032AD5E nop       
000000018032AD60 mov       qword ptr [rsp + 0x38], r13
000000018032AD65 mov       eax, dword ptr [rsp + 0x108]
000000018032AD6C mov       dword ptr [rsp + 0x30], eax
000000018032AD70 mov       eax, dword ptr [rsp + 0xf8]
000000018032AD77 mov       dword ptr [rsp + 0x28], eax
000000018032AD7B mov       rax, qword ptr [rsp + 0xf0]
000000018032AD83 mov       qword ptr [rsp + 0x20], rax
000000018032AD88 mov       r9, rdi
000000018032AD8B mov       r8d, ebx
000000018032AD8E mov       rdx, r15
000000018032AD91 mov       rcx, qword ptr [rsp + 0xd0]
000000018032AD99 call      0x18032b0a0
000000018032AD9E test      al, al
000000018032ADA0 je        0x18032adb3
000000018032ADA2 mov       r8, qword ptr [rip + 0x187f80f] # 0x181baa5b8
000000018032ADA9 mov       edx, ebx
000000018032ADAB mov       rcx, r15
000000018032ADAE call      0x18084e090
000000018032ADB3 dec       ebx
000000018032ADB5 cmp       ebx, r12d
000000018032ADB8 jge       0x18032ad60
000000018032ADBA add       rsp, 0x90
000000018032ADC1 pop       r15
000000018032ADC3 pop       r14
000000018032ADC5 pop       r13
000000018032ADC7 pop       r12
000000018032ADC9 pop       rdi
000000018032ADCA pop       rsi
000000018032ADCB pop       rbx
000000018032ADCC ret       
000000018032ADCD inc       dword ptr [r15 + 0x1c]
000000018032ADD1 mov       r8d, dword ptr [r15 + 0x18]
000000018032ADD5 mov       dword ptr [r15 + 0x18], r13d
000000018032ADD9 test      r8d, r8d
000000018032ADDC jle       0x18032adba
000000018032ADDE xor       r9d, r9d
000000018032ADE1 xor       edx, edx
000000018032ADE3 mov       rcx, qword ptr [r15 + 0x10]
000000018032ADE7 call      0x180cb86f0
000000018032ADEC jmp       0x18032adba
000000018032ADEE test      r15, r15
000000018032ADF1 je        0x18032ae3c
000000018032ADF3 mov       ebx, dword ptr [r15 + 0x18]
000000018032ADF7 test      r14, r14
000000018032ADFA je        0x18032ae3c
000000018032ADFC xor       r8d, r8d
000000018032ADFF mov       rdx, qword ptr [rsp + 0x110]
000000018032AE07 mov       rcx, r14
000000018032AE0A call      0x18032d9e0
000000018032AE0F mov       r9, qword ptr [rip + 0x187f6f2] # 0x181baa508
000000018032AE16 mov       r8, rax
000000018032AE19 mov       edx, ebx
000000018032AE1B mov       rcx, r15
000000018032AE1E call      0x18084d9c0
000000018032AE23 jmp       0x18032adba
000000018032AE25 call      0x180250150
000000018032AE2A nop       
000000018032AE2B call      0x180250150
000000018032AE30 call      0x180250150
000000018032AE35 nop       
000000018032AE36 call      0x180200e60
000000018032AE3B int3      
000000018032AE3C call      0x180250150
000000018032AE41 int3      
000000018032AE42 int3      
000000018032AE43 int3      
000000018032AE44 int3      
000000018032AE45 int3      
000000018032AE46 int3      
000000018032AE47 int3      
000000018032AE48 int3      
000000018032AE49 int3      
000000018032AE4A int3      
000000018032AE4B int3      
000000018032AE4C int3      
000000018032AE4D int3      
000000018032AE4E int3      
000000018032AE4F int3      