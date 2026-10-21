00000001803B3FE0 mov       qword ptr [rsp + 0x18], rbx
00000001803B3FE5 mov       qword ptr [rsp + 0x20], rsi
00000001803B3FEA push      rbp
00000001803B3FEB lea       rbp, [rsp - 0x57]
00000001803B3FF0 sub       rsp, 0xc0
00000001803B3FF7 cmp       byte ptr [rip + 0x1930f66], 0 # 0x181ce4f64
00000001803B3FFE mov       rsi, rcx
00000001803B4001 jne       0x1803b4016
00000001803B4003 lea       rcx, [rip + 0x17fc4f6] # 0x181bb0500
00000001803B400A call      0x18024fef0
00000001803B400F mov       byte ptr [rip + 0x1930f4e], 1 # 0x181ce4f64
00000001803B4016 xor       edx, edx
00000001803B4018 mov       rcx, rsi
00000001803B401B call      0x1803b3640
00000001803B4020 mov       rcx, qword ptr [rip + 0x17fc4d9] # 0x181bb0500
00000001803B4027 mov       rbx, qword ptr [rsi + 0x30]
00000001803B402B cmp       dword ptr [rcx + 0xe0], 0
00000001803B4032 jne       0x1803b4039
00000001803B4034 call      0x1802501e0
00000001803B4039 xor       r8d, r8d
00000001803B403C xor       edx, edx
00000001803B403E mov       rcx, rbx
00000001803B4041 call      0x18131f900
00000001803B4046 test      al, al
00000001803B4048 je        0x1803b4379
00000001803B404E cmp       dword ptr [rsi + 0x54], 1
00000001803B4052 mov       qword ptr [rsp + 0xd0], rdi
00000001803B405A mov       qword ptr [rsp + 0xd8], r14
00000001803B4062 movaps    xmmword ptr [rsp + 0xb0], xmm6
00000001803B406A movaps    xmmword ptr [rsp + 0xa0], xmm7
00000001803B4072 je        0x1803b4170
00000001803B4078 cmp       dword ptr [rsi + 0x54], 0
00000001803B407C jne       0x1803b423e
00000001803B4082 mov       rdx, qword ptr [rsi + 0x30]
00000001803B4086 test      rdx, rdx
00000001803B4089 je        0x1803b4488
00000001803B408F xor       r8d, r8d
00000001803B4092 lea       rcx, [rbp - 0x29]
00000001803B4096 call      0x18132f3a0
00000001803B409B movss     xmm1, dword ptr [rsi + 0x44]
00000001803B40A0 xorps     xmm6, xmm6
00000001803B40A3 movss     xmm2, dword ptr [rsi + 0x50]
00000001803B40A8 mov       rbx, qword ptr [rsi + 0x30]
00000001803B40AC movsd     xmm0, qword ptr [rax]
00000001803B40B0 mov       edi, dword ptr [rax + 8]
00000001803B40B3 mulss     xmm1, dword ptr [rip + 0x11f3d29] # 0x1815a7de4
00000001803B40BB mulss     xmm2, dword ptr [rip + 0x11f3d21] # 0x1815a7de4
00000001803B40C3 movsd     qword ptr [rbp - 0x19], xmm0
00000001803B40C8 unpcklps  xmm1, xmm2
00000001803B40CB lea       rdx, [rbp - 0x29]
00000001803B40CF xor       r8d, r8d
00000001803B40D2 movsd     qword ptr [rbp - 0x29], xmm1
00000001803B40D7 lea       rcx, [rbp + 7]
00000001803B40DB movss     dword ptr [rbp - 0x21], xmm6
00000001803B40E0 call      0x18130c6c0
00000001803B40E5 movss     xmm1, dword ptr [rsi + 0x38]
00000001803B40EA lea       r8, [rbp - 0x29]
00000001803B40EE xorps     xmm1, xmmword ptr [rip + 0x11f3bfb] # 0x1815a7cf0
00000001803B40F5 lea       rdx, [rbp + 7]
00000001803B40F9 xorps     xmm0, xmm0
00000001803B40FC movss     dword ptr [rbp - 0x21], xmm1
00000001803B4101 unpcklps  xmm0, xmm6
00000001803B4104 lea       rcx, [rbp - 9]
00000001803B4108 movsd     qword ptr [rbp - 0x29], xmm0
00000001803B410D xor       r9d, r9d
00000001803B4110 movups    xmm0, xmmword ptr [rax]
00000001803B4113 movaps    xmmword ptr [rbp + 7], xmm0
00000001803B4117 call      0x18130d120
00000001803B411C test      rbx, rbx
00000001803B411F je        0x1803b4488
00000001803B4125 movsd     xmm0, qword ptr [rax]
00000001803B4129 lea       r8, [rbp - 0x29]
00000001803B412D mov       eax, dword ptr [rax + 8]
00000001803B4130 lea       rcx, [rbp - 9]
00000001803B4134 xor       r9d, r9d
00000001803B4137 movsd     qword ptr [rbp - 0x29], xmm0
00000001803B413C mov       rdx, rbx
00000001803B413F mov       dword ptr [rbp - 0x21], eax
00000001803B4142 call      0x18132e9c0
00000001803B4147 movss     xmm4, dword ptr [rbp - 0x19]
00000001803B414C movss     xmm3, dword ptr [rbp - 0x15]
00000001803B4151 movd      xmm2, edi
00000001803B4155 movsd     xmm1, qword ptr [rax]
00000001803B4159 movaps    xmm0, xmm1
00000001803B415C addss     xmm4, xmm1
00000001803B4160 shufps    xmm0, xmm0, 0x55
00000001803B4164 addss     xmm3, xmm0
00000001803B4168 unpcklps  xmm4, xmm3
00000001803B416B jmp       0x1803b422a
00000001803B4170 mov       rdx, qword ptr [rsi + 0x30]
00000001803B4174 test      rdx, rdx
00000001803B4177 je        0x1803b4488
00000001803B417D xor       r8d, r8d
00000001803B4180 lea       rcx, [rbp - 9]
00000001803B4184 call      0x18132f3a0
00000001803B4189 movss     xmm1, dword ptr [rsi + 0x44]
00000001803B418E xorps     xmm6, xmm6
00000001803B4191 movss     xmm2, dword ptr [rsi + 0x50]
00000001803B4196 mulss     xmm1, dword ptr [rip + 0x11f3c46] # 0x1815a7de4
00000001803B419E mov       ebx, dword ptr [rax + 8]
00000001803B41A1 movsd     xmm7, qword ptr [rax]
00000001803B41A5 mulss     xmm2, dword ptr [rip + 0x11f3c37] # 0x1815a7de4
00000001803B41AD unpcklps  xmm1, xmm2
00000001803B41B0 lea       rdx, [rbp - 0x19]
00000001803B41B4 xor       r8d, r8d
00000001803B41B7 movsd     qword ptr [rbp - 0x19], xmm1
00000001803B41BC lea       rcx, [rbp + 7]
00000001803B41C0 movss     dword ptr [rbp - 0x11], xmm6
00000001803B41C5 call      0x18130c6c0
00000001803B41CA movss     xmm1, dword ptr [rsi + 0x38]
00000001803B41CF lea       r8, [rbp - 0x19]
00000001803B41D3 xorps     xmm1, xmmword ptr [rip + 0x11f3b16] # 0x1815a7cf0
00000001803B41DA lea       rdx, [rbp + 7]
00000001803B41DE xorps     xmm0, xmm0
00000001803B41E1 movss     dword ptr [rbp - 0x11], xmm1
00000001803B41E6 unpcklps  xmm0, xmm6
00000001803B41E9 lea       rcx, [rbp - 9]
00000001803B41ED movsd     qword ptr [rbp - 0x19], xmm0
00000001803B41F2 xor       r9d, r9d
00000001803B41F5 movups    xmm0, xmmword ptr [rax]
00000001803B41F8 movaps    xmmword ptr [rbp + 7], xmm0
00000001803B41FC call      0x18130d120
00000001803B4201 movaps    xmm3, xmm7
00000001803B4204 movsd     qword ptr [rbp - 0x19], xmm7
00000001803B4209 movaps    xmm4, xmm7
00000001803B420C shufps    xmm3, xmm3, 0x55
00000001803B4210 movd      xmm2, ebx
00000001803B4214 movsd     xmm1, qword ptr [rax]
00000001803B4218 movaps    xmm0, xmm1
00000001803B421B addss     xmm4, xmm1
00000001803B421F shufps    xmm0, xmm0, 0x55
00000001803B4223 addss     xmm3, xmm0
00000001803B4227 unpcklps  xmm4, xmm3
00000001803B422A addss     xmm2, dword ptr [rax + 8]
00000001803B422F movsd     qword ptr [rsi + 0x74], xmm4
00000001803B4234 movsd     qword ptr [rbp - 0x19], xmm1
00000001803B4239 movss     dword ptr [rsi + 0x7c], xmm2
00000001803B423E cmp       byte ptr [rsi + 0x58], 0
00000001803B4242 mov       r14, qword ptr [rsi + 0x20]
00000001803B4246 jne       0x1803b425e
00000001803B4248 test      r14, r14
00000001803B424B je        0x1803b4488
00000001803B4251 movsd     xmm0, qword ptr [rsi + 0x74]
00000001803B4256 mov       eax, dword ptr [rsi + 0x7c]
00000001803B4259 jmp       0x1803b4320
00000001803B425E mov       rdx, r14
00000001803B4261 test      r14, r14
00000001803B4264 je        0x1803b4488
00000001803B426A movaps    xmmword ptr [rsp + 0x90], xmm8
00000001803B4273 lea       rcx, [rbp - 9]
00000001803B4277 xor       r8d, r8d
00000001803B427A movaps    xmmword ptr [rsp + 0x80], xmm9
00000001803B4283 call      0x18132f3a0
00000001803B4288 movss     xmm6, dword ptr [rsi + 0x5c]
00000001803B428D xor       ecx, ecx
00000001803B428F movsd     xmm8, qword ptr [rsi + 0x74]
00000001803B4295 mov       edi, dword ptr [rsi + 0x7c]
00000001803B4298 movsd     xmm7, qword ptr [rax]
00000001803B429C mov       ebx, dword ptr [rax + 8]
00000001803B429F call      0x18132c320
00000001803B42A4 movaps    xmm9, xmm0
00000001803B42A8 xor       ecx, ecx
00000001803B42AA mulss     xmm9, xmm6
00000001803B42AF call      0x18132c2f0
00000001803B42B4 mov       qword ptr [rsp + 0x38], 0
00000001803B42BD lea       r9, [rsi + 0x68]
00000001803B42C1 movss     dword ptr [rsp + 0x30], xmm0
00000001803B42C7 lea       r8, [rbp - 0x19]
00000001803B42CB movss     xmm0, dword ptr [rip + 0x11f3d51] # 0x1815a8024
00000001803B42D3 lea       rdx, [rbp - 0x29]
00000001803B42D7 movss     dword ptr [rsp + 0x28], xmm0
00000001803B42DD lea       rcx, [rbp - 9]
00000001803B42E1 movss     dword ptr [rsp + 0x20], xmm9
00000001803B42E8 movsd     qword ptr [rbp - 0x19], xmm8
00000001803B42EE mov       dword ptr [rbp - 0x11], edi
00000001803B42F1 movsd     qword ptr [rbp - 0x29], xmm7
00000001803B42F6 mov       dword ptr [rbp - 0x21], ebx
00000001803B42F9 call      0x181312780
00000001803B42FE movaps    xmm9, xmmword ptr [rsp + 0x80]
00000001803B4307 movaps    xmm8, xmmword ptr [rsp + 0x90]
00000001803B4310 test      r14, r14
00000001803B4313 je        0x1803b4488
00000001803B4319 movsd     xmm0, qword ptr [rax]
00000001803B431D mov       eax, dword ptr [rax + 8]
00000001803B4320 xor       r8d, r8d
00000001803B4323 movsd     qword ptr [rbp - 0x19], xmm0
00000001803B4328 lea       rdx, [rbp - 0x19]
00000001803B432C mov       dword ptr [rbp - 0x11], eax
00000001803B432F mov       rcx, r14
00000001803B4332 call      0x18132fbb0
00000001803B4337 cmp       byte ptr [rsi + 0x59], 0
00000001803B433B mov       rbx, qword ptr [rsi + 0x20]
00000001803B433F jne       0x1803b438e
00000001803B4341 test      rbx, rbx
00000001803B4344 je        0x1803b4488
00000001803B434A mov       rdx, qword ptr [rsi + 0x30]
00000001803B434E xor       r8d, r8d
00000001803B4351 mov       rcx, rbx
00000001803B4354 call      0x18132dea0
00000001803B4359 movaps    xmm6, xmmword ptr [rsp + 0xb0]
00000001803B4361 mov       r14, qword ptr [rsp + 0xd8]
00000001803B4369 mov       rdi, qword ptr [rsp + 0xd0]
00000001803B4371 movaps    xmm7, xmmword ptr [rsp + 0xa0]
00000001803B4379 lea       r11, [rsp + 0xc0]
00000001803B4381 mov       rbx, qword ptr [r11 + 0x20]
00000001803B4385 mov       rsi, qword ptr [r11 + 0x28]
00000001803B4389 mov       rsp, r11
00000001803B438C pop       rbp
00000001803B438D ret       
00000001803B438E mov       rdx, rbx
00000001803B4391 test      rbx, rbx
00000001803B4394 je        0x1803b4488
00000001803B439A xor       r8d, r8d
00000001803B439D lea       rcx, [rbp + 7]
00000001803B43A1 call      0x18132f510
00000001803B43A6 mov       rdx, qword ptr [rsi + 0x30]
00000001803B43AA movups    xmm7, xmmword ptr [rax]
00000001803B43AD test      rdx, rdx
00000001803B43B0 je        0x1803b4488
00000001803B43B6 xor       r8d, r8d
00000001803B43B9 lea       rcx, [rbp - 9]
00000001803B43BD call      0x18132f3a0
00000001803B43C2 mov       rdx, qword ptr [rsi + 0x20]
00000001803B43C6 movsd     xmm0, qword ptr [rax]
00000001803B43CA mov       edi, dword ptr [rax + 8]
00000001803B43CD movsd     qword ptr [rbp - 0x19], xmm0
00000001803B43D2 test      rdx, rdx
00000001803B43D5 je        0x1803b4488
00000001803B43DB xor       r8d, r8d
00000001803B43DE lea       rcx, [rbp - 9]
00000001803B43E2 call      0x18132f3a0
00000001803B43E7 movss     xmm3, dword ptr [rbp - 0x19]
00000001803B43EC movss     xmm2, dword ptr [rbp - 0x15]
00000001803B43F1 movsd     xmm1, qword ptr [rax]
00000001803B43F5 movaps    xmm0, xmm1
00000001803B43F8 movsd     qword ptr [rbp - 0x19], xmm1
00000001803B43FD subss     xmm3, xmm1
00000001803B4401 shufps    xmm0, xmm0, 0x55
00000001803B4405 movd      xmm1, edi
00000001803B4409 subss     xmm2, xmm0
00000001803B440D subss     xmm1, dword ptr [rax + 8]
00000001803B4412 unpcklps  xmm3, xmm2
00000001803B4415 lea       rdx, [rbp - 0x19]
00000001803B4419 xor       r8d, r8d
00000001803B441C movsd     qword ptr [rbp - 0x19], xmm3
00000001803B4421 lea       rcx, [rbp + 7]
00000001803B4425 movss     dword ptr [rbp - 0x11], xmm1
00000001803B442A call      0x18130ca30
00000001803B442F movss     xmm6, dword ptr [rsi + 0x60]
00000001803B4434 xor       ecx, ecx
00000001803B4436 movdqa    xmmword ptr [rbp - 9], xmm7
00000001803B443B movups    xmm0, xmmword ptr [rax]
00000001803B443E movaps    xmmword ptr [rbp + 7], xmm0
00000001803B4442 call      0x18132c2f0
00000001803B4447 mulss     xmm0, xmm6
00000001803B444B lea       r8, [rbp + 7]
00000001803B444F lea       rdx, [rbp - 9]
00000001803B4453 mov       qword ptr [rsp + 0x20], 0
00000001803B445C lea       rcx, [rbp - 0x19]
00000001803B4460 movaps    xmm3, xmm0
00000001803B4463 call      0x18130c960
00000001803B4468 test      rbx, rbx
00000001803B446B je        0x1803b4488
00000001803B446D movups    xmm0, xmmword ptr [rax]
00000001803B4470 xor       r8d, r8d
00000001803B4473 lea       rdx, [rbp + 7]
00000001803B4477 mov       rcx, rbx
00000001803B447A movaps    xmmword ptr [rbp + 7], xmm0
00000001803B447E call      0x18132fc50
00000001803B4483 jmp       0x1803b4359
00000001803B4488 call      0x180250150
00000001803B448D int3      
00000001803B448E int3      
00000001803B448F int3      