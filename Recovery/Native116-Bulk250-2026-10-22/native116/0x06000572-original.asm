000000018038AB10 mov       qword ptr [rsp + 0x10], rbx
000000018038AB15 mov       qword ptr [rsp + 8], rcx
000000018038AB1A push      rdi
000000018038AB1B sub       rsp, 0x60
000000018038AB1F mov       rdi, rcx
000000018038AB22 cmp       byte ptr [rip + 0x195a312], 0 # 0x181ce4e3b
000000018038AB29 jne       0x18038ab6e
000000018038AB2B lea       rcx, [rip + 0x183e886] # 0x181bc93b8
000000018038AB32 call      0x18024fef0
000000018038AB37 lea       rcx, [rip + 0x183e8d2] # 0x181bc9410
000000018038AB3E call      0x18024fef0
000000018038AB43 lea       rcx, [rip + 0x183e916] # 0x181bc9460
000000018038AB4A call      0x18024fef0
000000018038AB4F lea       rcx, [rip + 0x18174c2] # 0x181ba2018
000000018038AB56 call      0x18024fef0
000000018038AB5B lea       rcx, [rip + 0x1832abe] # 0x181bbd620
000000018038AB62 call      0x18024fef0
000000018038AB67 mov       byte ptr [rip + 0x195a2cd], 1 # 0x181ce4e3b
000000018038AB6E mov       rdx, qword ptr [rdi + 0x168]
000000018038AB75 test      rdx, rdx
000000018038AB78 je        0x18038adf5
000000018038AB7E mov       r8, qword ptr [rip + 0x1832a9b] # 0x181bbd620
000000018038AB85 lea       rcx, [rsp + 0x28]
000000018038AB8A call      0x1808197f0
000000018038AB8F movups    xmm0, xmmword ptr [rsp + 0x28]
000000018038AB94 movups    xmmword ptr [rsp + 0x40], xmm0
000000018038AB99 movsd     xmm1, qword ptr [rsp + 0x38]
000000018038AB9F movsd     qword ptr [rsp + 0x50], xmm1
000000018038ABA5 mov       qword ptr [rsp + 0x28], 0
000000018038ABAE lea       rbx, [rsp + 0x40]
000000018038ABB3 mov       qword ptr [rsp + 0x30], rbx
000000018038ABB8 nop       dword ptr [rax + rax]
000000018038ABC0 mov       rdx, qword ptr [rip + 0x183e849] # 0x181bc9410
000000018038ABC7 lea       rcx, [rsp + 0x40]
000000018038ABCC call      0x180669280
000000018038ABD1 test      al, al
000000018038ABD3 je        0x18038abfb
000000018038ABD5 mov       rcx, qword ptr [rsp + 0x50]
000000018038ABDA test      rcx, rcx
000000018038ABDD je        0x18038ae01
000000018038ABE3 mov       rdx, qword ptr [rcx + 0x20]
000000018038ABE7 test      rdx, rdx
000000018038ABEA je        0x18038adfb
000000018038ABF0 mov       eax, dword ptr [rdx + 0x20]
000000018038ABF3 sub       eax, dword ptr [rcx + 0x28]
000000018038ABF6 mov       dword ptr [rdx + 0x20], eax
000000018038ABF9 jmp       0x18038abc0
000000018038ABFB mov       rdx, qword ptr [rip + 0x183e7b6] # 0x181bc93b8
000000018038AC02 mov       rcx, rbx
000000018038AC05 call      0x180302170
000000018038AC0A jmp       0x18038ac30
000000018038AC0C mov       rdx, qword ptr [rip + 0x183e7a5] # 0x181bc93b8
000000018038AC13 mov       rcx, qword ptr [rsp + 0x30]
000000018038AC18 call      0x180302170
000000018038AC1D mov       rcx, qword ptr [rsp + 0x28]
000000018038AC22 test      rcx, rcx
000000018038AC25 jne       0x18038ae07
000000018038AC2B mov       rdi, qword ptr [rsp + 0x70]
000000018038AC30 mov       rax, qword ptr [rip + 0x18173e1] # 0x181ba2018
000000018038AC37 cmp       dword ptr [rax + 0xe0], 0
000000018038AC3E jne       0x18038ac4f
000000018038AC40 mov       rcx, rax
000000018038AC43 call      0x1802501e0
000000018038AC48 mov       rax, qword ptr [rip + 0x18173c9] # 0x181ba2018
000000018038AC4F mov       rax, qword ptr [rax + 0xb8]
000000018038AC56 mov       rcx, qword ptr [rax]
000000018038AC59 test      rcx, rcx
000000018038AC5C je        0x18038adf5
000000018038AC62 mov       rcx, qword ptr [rcx + 0x28]
000000018038AC66 test      rcx, rcx
000000018038AC69 je        0x18038adf5
000000018038AC6F mov       rcx, qword ptr [rcx + 0x10]
000000018038AC73 test      rcx, rcx
000000018038AC76 je        0x18038adf5
000000018038AC7C mov       eax, dword ptr [rcx + 0x70]
000000018038AC7F sub       eax, dword ptr [rdi + 0x188]
000000018038AC85 mov       dword ptr [rcx + 0x70], eax
000000018038AC88 mov       rcx, qword ptr [rdi + 0x60]
000000018038AC8C test      rcx, rcx
000000018038AC8F je        0x18038adf5
000000018038AC95 mov       eax, dword ptr [rdi + 0xa8]
000000018038AC9B mov       dword ptr [rcx + 0x58], eax
000000018038AC9E mov       rcx, qword ptr [rdi + 0x60]
000000018038ACA2 mov       rax, qword ptr [rdi + 0xe0]
000000018038ACA9 test      rax, rax
000000018038ACAC je        0x18038adf5
000000018038ACB2 test      rcx, rcx
000000018038ACB5 je        0x18038adf5
000000018038ACBB mov       eax, dword ptr [rax + 0x6c]
000000018038ACBE mov       dword ptr [rcx + 0x6c], eax
000000018038ACC1 mov       rcx, qword ptr [rdi + 0x60]
000000018038ACC5 test      rcx, rcx
000000018038ACC8 je        0x18038adf5
000000018038ACCE mov       eax, dword ptr [rcx + 0x6c]
000000018038ACD1 mov       dword ptr [rcx + 0x68], eax
000000018038ACD4 mov       rcx, qword ptr [rdi + 0x60]
000000018038ACD8 mov       rax, qword ptr [rdi + 0xe0]
000000018038ACDF test      rax, rax
000000018038ACE2 je        0x18038adf5
000000018038ACE8 test      rcx, rcx
000000018038ACEB je        0x18038adf5
000000018038ACF1 mov       eax, dword ptr [rax + 0x70]
000000018038ACF4 mov       dword ptr [rcx + 0x70], eax
000000018038ACF7 mov       rcx, qword ptr [rdi + 0x60]
000000018038ACFB mov       rax, qword ptr [rdi + 0xe0]
000000018038AD02 test      rax, rax
000000018038AD05 je        0x18038adf5
000000018038AD0B test      rcx, rcx
000000018038AD0E je        0x18038adf5
000000018038AD14 mov       eax, dword ptr [rax + 0x74]
000000018038AD17 mov       dword ptr [rcx + 0x74], eax
000000018038AD1A mov       rcx, qword ptr [rdi + 0x60]
000000018038AD1E mov       rax, qword ptr [rdi + 0xe0]
000000018038AD25 test      rax, rax
000000018038AD28 je        0x18038adf5
000000018038AD2E test      rcx, rcx
000000018038AD31 je        0x18038adf5
000000018038AD37 mov       eax, dword ptr [rax + 0x78]
000000018038AD3A mov       dword ptr [rcx + 0x78], eax
000000018038AD3D mov       rcx, qword ptr [rdi + 0x60]
000000018038AD41 mov       rax, qword ptr [rdi + 0xe0]
000000018038AD48 test      rax, rax
000000018038AD4B je        0x18038adf5
000000018038AD51 test      rcx, rcx
000000018038AD54 je        0x18038adf5
000000018038AD5A mov       eax, dword ptr [rax + 0x7c]
000000018038AD5D mov       dword ptr [rcx + 0x7c], eax
000000018038AD60 mov       rcx, qword ptr [rdi + 0x60]
000000018038AD64 mov       rax, qword ptr [rdi + 0xe0]
000000018038AD6B test      rax, rax
000000018038AD6E je        0x18038adf5
000000018038AD74 test      rcx, rcx
000000018038AD77 je        0x18038adf5
000000018038AD79 mov       eax, dword ptr [rax + 0x60]
000000018038AD7C mov       dword ptr [rcx + 0x60], eax
000000018038AD7F mov       rcx, qword ptr [rdi + 0x60]
000000018038AD83 mov       rax, qword ptr [rdi + 0xe0]
000000018038AD8A test      rax, rax
000000018038AD8D je        0x18038adf5
000000018038AD8F test      rcx, rcx
000000018038AD92 je        0x18038adf5
000000018038AD94 mov       eax, dword ptr [rax + 0x64]
000000018038AD97 mov       dword ptr [rcx + 0x64], eax
000000018038AD9A xor       edx, edx
000000018038AD9C mov       rcx, rdi
000000018038AD9F call      0x18038cbc0
000000018038ADA4 mov       rdx, qword ptr [rdi + 0x60]
000000018038ADA8 test      rdx, rdx
000000018038ADAB je        0x18038adf5
000000018038ADAD xor       r8d, r8d
000000018038ADB0 mov       edx, dword ptr [rdx + 0x58]
000000018038ADB3 mov       rcx, rdi
000000018038ADB6 call      0x18038dcc0
000000018038ADBB mov       rdx, qword ptr [rdi + 0x60]
000000018038ADBF test      rdx, rdx
000000018038ADC2 je        0x18038adf5
000000018038ADC4 mov       ecx, dword ptr [rdx + 0x54]
000000018038ADC7 add       ecx, 2
000000018038ADCA lea       eax, [rcx + rcx*4]
000000018038ADCD add       eax, eax
000000018038ADCF cmp       dword ptr [rdx + 0x58], eax
000000018038ADD2 jl        0x18038adea
000000018038ADD4 mov       rcx, qword ptr [rdi + 0xd0]
000000018038ADDB test      rcx, rcx
000000018038ADDE je        0x18038adf5
000000018038ADE0 xor       r8d, r8d
000000018038ADE3 xor       edx, edx
000000018038ADE5 call      0x18156cc70
000000018038ADEA mov       rbx, qword ptr [rsp + 0x78]
000000018038ADEF add       rsp, 0x60
000000018038ADF3 pop       rdi
000000018038ADF4 ret       
000000018038ADF5 call      0x180250150
000000018038ADFA nop       
000000018038ADFB call      0x180250150
000000018038AE00 nop       
000000018038AE01 call      0x180250150
000000018038AE06 nop       
000000018038AE07 call      0x180200e60
000000018038AE0C int3      
000000018038AE0D int3      
000000018038AE0E int3      
000000018038AE0F int3      