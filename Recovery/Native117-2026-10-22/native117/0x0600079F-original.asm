00000001803C0400 mov       qword ptr [rsp + 0x20], rsi
00000001803C0405 push      rbp
00000001803C0406 push      rdi
00000001803C0407 push      r14
00000001803C0409 mov       rbp, rsp
00000001803C040C sub       rsp, 0x80
00000001803C0413 cmp       byte ptr [rip + 0x1924b66], 0 # 0x181ce4f80
00000001803C041A mov       rdi, rcx
00000001803C041D movaps    xmmword ptr [rsp + 0x40], xmm9
00000001803C0423 jne       0x1803c0438
00000001803C0425 lea       rcx, [rip + 0x17fdb4c] # 0x181bbdf78
00000001803C042C call      0x18024fef0
00000001803C0431 mov       byte ptr [rip + 0x1924b48], 1 # 0x181ce4f80
00000001803C0438 mov       ecx, dword ptr [rdi + 0x10]
00000001803C043B xor       r14d, r14d
00000001803C043E mov       rsi, qword ptr [rdi + 0x20]
00000001803C0442 movss     xmm9, dword ptr [rip + 0x11e75d9] # 0x1815a7a24
00000001803C044B mov       qword ptr [rsp + 0xa8], rbx
00000001803C0453 movaps    xmmword ptr [rsp + 0x70], xmm6
00000001803C0458 movaps    xmmword ptr [rsp + 0x60], xmm7
00000001803C045D movaps    xmmword ptr [rsp + 0x50], xmm8
00000001803C0463 mov       dword ptr [rbp + 0x30], r14d
00000001803C0467 test      ecx, ecx
00000001803C0469 je        0x1803c04d9
00000001803C046B sub       ecx, 1
00000001803C046E je        0x1803c04cd
00000001803C0470 cmp       ecx, 1
00000001803C0473 jne       0x1803c04c6
00000001803C0475 mov       dword ptr [rdi + 0x10], 0xffffffff
00000001803C047C test      rsi, rsi
00000001803C047F je        0x1803c0967
00000001803C0485 mov       rcx, qword ptr [rsi + 0x48]
00000001803C0489 test      rcx, rcx
00000001803C048C je        0x1803c0967
00000001803C0492 movsd     xmm0, qword ptr [rdi + 0x34]
00000001803C0497 lea       rdx, [rbp - 0x60]
00000001803C049B mov       eax, dword ptr [rdi + 0x3c]
00000001803C049E xor       r8d, r8d
00000001803C04A1 movsd     qword ptr [rbp - 0x60], xmm0
00000001803C04A6 mov       dword ptr [rbp - 0x58], eax
00000001803C04A9 call      0x18132fbb0
00000001803C04AE xor       edx, edx
00000001803C04B0 mov       rcx, rsi
00000001803C04B3 call      0x1803bbcf0
00000001803C04B8 xor       r8d, r8d
00000001803C04BB mov       rdx, rax
00000001803C04BE mov       rcx, rsi
00000001803C04C1 call      0x18131dbe0
00000001803C04C6 xor       al, al
00000001803C04C8 jmp       0x1803c08d2
00000001803C04CD mov       dword ptr [rdi + 0x10], 0xffffffff
00000001803C04D4 jmp       0x1803c0582
00000001803C04D9 movss     xmm1, dword ptr [rip + 0x11e7627] # 0x1815a7b08
00000001803C04E1 xor       r8d, r8d
00000001803C04E4 movss     xmm0, dword ptr [rip + 0x11e76f4] # 0x1815a7be0
00000001803C04EC mov       dword ptr [rdi + 0x10], 0xffffffff
00000001803C04F3 mov       dword ptr [rdi + 0x28], 0x40000000
00000001803C04FA call      0x18130d350
00000001803C04FF movss     dword ptr [rdi + 0x2c], xmm0
00000001803C0504 movss     dword ptr [rdi + 0x30], xmm0
00000001803C0509 test      rsi, rsi
00000001803C050C je        0x1803c0967
00000001803C0512 mov       rdx, qword ptr [rsi + 0x48]
00000001803C0516 test      rdx, rdx
00000001803C0519 je        0x1803c0967
00000001803C051F xor       r8d, r8d
00000001803C0522 lea       rcx, [rbp - 0x50]
00000001803C0526 call      0x18132f3a0
00000001803C052B movsd     xmm0, qword ptr [rax]
00000001803C052F mov       ecx, dword ptr [rax + 8]
00000001803C0532 movsd     qword ptr [rdi + 0x34], xmm0
00000001803C0537 mov       dword ptr [rdi + 0x3c], ecx
00000001803C053A mov       rdx, qword ptr [rsi + 0x38]
00000001803C053E test      rdx, rdx
00000001803C0541 je        0x1803c0967
00000001803C0547 xor       r8d, r8d
00000001803C054A lea       rcx, [rbp - 0x50]
00000001803C054E call      0x1813bb830
00000001803C0553 xor       edx, edx
00000001803C0555 lea       rcx, [rbp - 0x50]
00000001803C0559 movups    xmm0, xmmword ptr [rax]
00000001803C055C movaps    xmmword ptr [rbp - 0x50], xmm0
00000001803C0560 call      0x18039bf00
00000001803C0565 movaps    xmm0, xmm9
00000001803C0569 mov       dword ptr [rdi + 0x40], eax
00000001803C056C divss     xmm0, dword ptr [rdi + 0x2c]
00000001803C0571 mov       dword ptr [rdi + 0x44], 0x437f0000
00000001803C0578 mulss     xmm0, dword ptr [rdi + 0x28]
00000001803C057D movss     dword ptr [rdi + 0x48], xmm0
00000001803C0582 movss     xmm0, dword ptr [rdi + 0x30]
00000001803C0587 xorps     xmm8, xmm8
00000001803C058B comiss    xmm0, xmm8
00000001803C058F jbe       0x1803c0904
00000001803C0595 xor       ecx, ecx
00000001803C0597 movaps    xmm6, xmm0
00000001803C059A call      0x18132c2f0
00000001803C059F divss     xmm0, dword ptr [rdi + 0x28]
00000001803C05A4 movss     xmm7, dword ptr [rip + 0x11e79e0] # 0x1815a7f8c
00000001803C05AC mulss     xmm0, dword ptr [rdi + 0x2c]
00000001803C05B1 subss     xmm6, xmm0
00000001803C05B5 comiss    xmm9, xmm6
00000001803C05B9 movss     dword ptr [rdi + 0x30], xmm6
00000001803C05BE jb        0x1803c05f1
00000001803C05C0 movss     xmm6, dword ptr [rdi + 0x44]
00000001803C05C5 xor       ecx, ecx
00000001803C05C7 call      0x18132c2f0
00000001803C05CC divss     xmm0, dword ptr [rdi + 0x48]
00000001803C05D1 mulss     xmm0, xmm7
00000001803C05D5 subss     xmm6, xmm0
00000001803C05D9 comiss    xmm8, xmm6
00000001803C05DD ja        0x1803c05e9
00000001803C05DF comiss    xmm6, xmm7
00000001803C05E2 jbe       0x1803c05ec
00000001803C05E4 movaps    xmm6, xmm7
00000001803C05E7 jmp       0x1803c05ec
00000001803C05E9 xorps     xmm6, xmm6
00000001803C05EC movss     dword ptr [rdi + 0x44], xmm6
00000001803C05F1 cvttss2si eax, dword ptr [rdi + 0x30]
00000001803C05F6 mov       dword ptr [rbp + 0x30], eax
00000001803C05F9 test      rsi, rsi
00000001803C05FC je        0x1803c0967
00000001803C0602 mov       rbx, qword ptr [rsi + 0x38]
00000001803C0606 lea       rcx, [rbp + 0x30]
00000001803C060A xor       edx, edx
00000001803C060C call      0x180ca3820
00000001803C0611 test      rbx, rbx
00000001803C0614 je        0x1803c0967
00000001803C061A xor       r8d, r8d
00000001803C061D mov       rdx, rax
00000001803C0620 mov       rcx, rbx
00000001803C0623 call      0x1813bba30
00000001803C0628 movzx     ecx, byte ptr [rdi + 0x40]
00000001803C062C movss     xmm0, dword ptr [rdi + 0x44]
00000001803C0631 movzx     edx, byte ptr [rdi + 0x41]
00000001803C0635 movzx     r8d, byte ptr [rdi + 0x42]
00000001803C063A mov       r9, qword ptr [rsi + 0x38]
00000001803C063E movd      xmm3, ecx
00000001803C0642 cvttss2si eax, xmm0
00000001803C0646 mov       dword ptr [rbp + 0x20], r14d
00000001803C064A mov       byte ptr [rbp + 0x20], cl
00000001803C064D mov       byte ptr [rbp + 0x21], dl
00000001803C0650 mov       byte ptr [rbp + 0x22], r8b
00000001803C0654 cvtdq2ps  xmm3, xmm3
00000001803C0657 mov       byte ptr [rbp + 0x23], al
00000001803C065A mov       eax, dword ptr [rbp + 0x20]
00000001803C065D mov       ecx, eax
00000001803C065F shr       ecx, 8
00000001803C0662 movzx     ecx, cl
00000001803C0665 divss     xmm3, xmm7
00000001803C0669 movd      xmm2, ecx
00000001803C066D mov       ecx, eax
00000001803C066F shr       ecx, 0x10
00000001803C0672 movzx     ecx, cl
00000001803C0675 shr       eax, 0x18
00000001803C0678 cvtdq2ps  xmm2, xmm2
00000001803C067B movd      xmm1, ecx
00000001803C067F movd      xmm0, eax
00000001803C0683 cvtdq2ps  xmm1, xmm1
00000001803C0686 cvtdq2ps  xmm0, xmm0
00000001803C0689 divss     xmm1, xmm7
00000001803C068D divss     xmm0, xmm7
00000001803C0691 divss     xmm2, xmm7
00000001803C0695 movss     dword ptr [rbp - 0x50], xmm3
00000001803C069A movss     dword ptr [rbp - 0x4c], xmm2
00000001803C069F movss     dword ptr [rbp - 0x48], xmm1
00000001803C06A4 movss     dword ptr [rbp - 0x44], xmm0
00000001803C06A9 test      r9, r9
00000001803C06AC je        0x1803c0967
00000001803C06B2 movups    xmm0, xmmword ptr [rbp - 0x50]
00000001803C06B6 xor       r8d, r8d
00000001803C06B9 lea       rdx, [rbp - 0x50]
00000001803C06BD mov       rcx, r9
00000001803C06C0 movdqa    xmmword ptr [rbp - 0x50], xmm0
00000001803C06C5 call      0x1813bb950
00000001803C06CA mov       r14, qword ptr [rsi + 0x48]
00000001803C06CE test      r14, r14
00000001803C06D1 je        0x1803c0967
00000001803C06D7 xor       r8d, r8d
00000001803C06DA lea       rcx, [rbp - 0x50]
00000001803C06DE mov       rdx, r14
00000001803C06E1 call      0x18132f3a0
00000001803C06E6 movss     xmm6, dword ptr [rdi + 0x2c]
00000001803C06EB xor       ecx, ecx
00000001803C06ED movsd     xmm7, qword ptr [rax]
00000001803C06F1 mov       ebx, dword ptr [rax + 8]
00000001803C06F4 call      0x18132c2f0
00000001803C06F9 movaps    xmm4, xmm0
00000001803C06FC movd      xmm2, ebx
00000001803C0700 mulss     xmm4, xmm6
00000001803C0704 addss     xmm2, xmm8
00000001803C0709 movaps    xmm1, xmm7
00000001803C070C movsd     qword ptr [rbp - 0x50], xmm7
00000001803C0711 movaps    xmm3, xmm7
00000001803C0714 shufps    xmm1, xmm1, 0x55
00000001803C0718 addss     xmm4, xmm1
00000001803C071C addss     xmm3, xmm8
00000001803C0721 unpcklps  xmm3, xmm4
00000001803C0724 lea       rdx, [rbp - 0x60]
00000001803C0728 xor       r8d, r8d
00000001803C072B movsd     qword ptr [rbp - 0x60], xmm3
00000001803C0730 mov       rcx, r14
00000001803C0733 movss     dword ptr [rbp - 0x58], xmm2
00000001803C0738 call      0x18132fbb0
00000001803C073D mov       rdx, qword ptr [rsi + 0x50]
00000001803C0741 movsd     xmm6, qword ptr [rsi + 0x58]
00000001803C0746 mov       ebx, dword ptr [rsi + 0x60]
00000001803C0749 test      rdx, rdx
00000001803C074C je        0x1803c0967
00000001803C0752 xor       r8d, r8d
00000001803C0755 lea       rcx, [rbp - 0x50]
00000001803C0759 call      0x18132f3a0
00000001803C075E xor       r9d, r9d
00000001803C0761 lea       rdx, [rbp - 0x60]
00000001803C0765 mov       r8d, 0x3e8
00000001803C076B lea       rcx, [rbp - 0x50]
00000001803C076F movsd     xmm0, qword ptr [rax]
00000001803C0773 mov       eax, dword ptr [rax + 8]
00000001803C0776 movsd     qword ptr [rbp - 0x60], xmm0
00000001803C077B mov       dword ptr [rbp - 0x58], eax
00000001803C077E movsd     qword ptr [rbp - 0x50], xmm6
00000001803C0783 mov       dword ptr [rbp - 0x48], ebx
00000001803C0786 call      0x1811657a0
00000001803C078B test      al, al
00000001803C078D je        0x1803c07d6
00000001803C078F mov       rdx, qword ptr [rsi + 0x50]
00000001803C0793 movups    xmm6, xmmword ptr [rsi + 0x64]
00000001803C0797 test      rdx, rdx
00000001803C079A je        0x1803c0967
00000001803C07A0 xor       r8d, r8d
00000001803C07A3 lea       rcx, [rbp - 0x50]
00000001803C07A7 call      0x18132f510
00000001803C07AC xor       r9d, r9d
00000001803C07AF movdqa    xmmword ptr [rbp - 0x60], xmm6
00000001803C07B4 mov       r8d, 0x3e8
00000001803C07BA lea       rdx, [rbp - 0x50]
00000001803C07BE lea       rcx, [rbp - 0x60]
00000001803C07C2 movups    xmm0, xmmword ptr [rax]
00000001803C07C5 movaps    xmmword ptr [rbp - 0x50], xmm0
00000001803C07C9 call      0x1811656d0
00000001803C07CE test      al, al
00000001803C07D0 jne       0x1803c0893
00000001803C07D6 mov       rdx, qword ptr [rsi + 0x50]
00000001803C07DA test      rdx, rdx
00000001803C07DD je        0x1803c0967
00000001803C07E3 xor       r8d, r8d
00000001803C07E6 lea       rcx, [rbp - 0x50]
00000001803C07EA call      0x18132f3a0
00000001803C07EF mov       rdx, qword ptr [rsi + 0x50]
00000001803C07F3 movsd     xmm0, qword ptr [rax]
00000001803C07F7 mov       ecx, dword ptr [rax + 8]
00000001803C07FA movsd     qword ptr [rsi + 0x58], xmm0
00000001803C07FF mov       dword ptr [rsi + 0x60], ecx
00000001803C0802 test      rdx, rdx
00000001803C0805 je        0x1803c0967
00000001803C080B xor       r8d, r8d
00000001803C080E lea       rcx, [rbp - 0x50]
00000001803C0812 call      0x18132f510
00000001803C0817 mov       rcx, qword ptr [rsi + 0x48]
00000001803C081B movups    xmm0, xmmword ptr [rax]
00000001803C081E movups    xmmword ptr [rsi + 0x64], xmm0
00000001803C0822 test      rcx, rcx
00000001803C0825 je        0x1803c0967
00000001803C082B xor       r8d, r8d
00000001803C082E movdqa    xmmword ptr [rbp - 0x50], xmm0
00000001803C0833 lea       rdx, [rbp - 0x50]
00000001803C0837 call      0x18132fc50
00000001803C083C mov       rdx, qword ptr [rsi + 0x40]
00000001803C0840 test      rdx, rdx
00000001803C0843 je        0x1803c0967
00000001803C0849 xor       r8d, r8d
00000001803C084C lea       rcx, [rbp - 0x50]
00000001803C0850 call      0x18132f3a0
00000001803C0855 movsd     xmm0, qword ptr [rsi + 0x58]
00000001803C085A movsd     xmm1, qword ptr [rax]
00000001803C085E movss     xmm2, dword ptr [rax + 8]
00000001803C0863 subss     xmm1, xmm0
00000001803C0867 subss     xmm2, dword ptr [rsi + 0x60]
00000001803C086C mov       rcx, qword ptr [rsi + 0x40]
00000001803C0870 test      rcx, rcx
00000001803C0873 je        0x1803c0967
00000001803C0879 unpcklps  xmm1, xmm8
00000001803C087D lea       rdx, [rbp - 0x50]
00000001803C0881 xor       r8d, r8d
00000001803C0884 movsd     qword ptr [rbp - 0x50], xmm1
00000001803C0889 movss     dword ptr [rbp - 0x48], xmm2
00000001803C088E call      0x18132f770
00000001803C0893 mov       rax, qword ptr [rip + 0x17fd6de] # 0x181bbdf78
00000001803C089A cmp       dword ptr [rax + 0xe0], 0
00000001803C08A1 jne       0x1803c08b2
00000001803C08A3 mov       rcx, rax
00000001803C08A6 call      0x1802501e0
00000001803C08AB mov       rax, qword ptr [rip + 0x17fd6c6] # 0x181bbdf78
00000001803C08B2 mov       rax, qword ptr [rax + 0xb8]
00000001803C08B9 lea       rcx, [rdi + 0x18]
00000001803C08BD mov       rdx, qword ptr [rax]
00000001803C08C0 mov       qword ptr [rdi + 0x18], rdx
00000001803C08C4 call      0x18024f360
00000001803C08C9 mov       dword ptr [rdi + 0x10], 1
00000001803C08D0 mov       al, 1
00000001803C08D2 movaps    xmm8, xmmword ptr [rsp + 0x50]
00000001803C08D8 movaps    xmm7, xmmword ptr [rsp + 0x60]
00000001803C08DD movaps    xmm6, xmmword ptr [rsp + 0x70]
00000001803C08E2 mov       rbx, qword ptr [rsp + 0xa8]
00000001803C08EA mov       rsi, qword ptr [rsp + 0xb8]
00000001803C08F2 movaps    xmm9, xmmword ptr [rsp + 0x40]
00000001803C08F8 add       rsp, 0x80
00000001803C08FF pop       r14
00000001803C0901 pop       rdi
00000001803C0902 pop       rbp
00000001803C0903 ret       
00000001803C0904 mov       rax, qword ptr [rip + 0x17fd66d] # 0x181bbdf78
00000001803C090B cmp       dword ptr [rax + 0xe0], r14d
00000001803C0912 jne       0x1803c0923
00000001803C0914 mov       rcx, rax
00000001803C0917 call      0x1802501e0
00000001803C091C mov       rax, qword ptr [rip + 0x17fd655] # 0x181bbdf78
00000001803C0923 mov       rax, qword ptr [rax + 0xb8]
00000001803C092A xor       r8d, r8d
00000001803C092D mov       edx, 0x14
00000001803C0932 xor       ecx, ecx
00000001803C0934 mov       rbx, qword ptr [rax + 8]
00000001803C0938 call      0x18130d310
00000001803C093D test      rbx, rbx
00000001803C0940 je        0x1803c0967
00000001803C0942 cmp       eax, dword ptr [rbx + 0x18]
00000001803C0945 jae       0x1803c096d
00000001803C0947 cdqe      
00000001803C0949 lea       rcx, [rdi + 0x18]
00000001803C094D mov       rdx, qword ptr [rbx + rax*8 + 0x20]
00000001803C0952 mov       qword ptr [rdi + 0x18], rdx
00000001803C0956 call      0x18024f360
00000001803C095B mov       dword ptr [rdi + 0x10], 2
00000001803C0962 jmp       0x1803c08d0
00000001803C0967 call      0x180250150
00000001803C096C int3      
00000001803C096D call      0x180250140
00000001803C0972 int3      
00000001803C0973 int3      
00000001803C0974 int3      
00000001803C0975 int3      
00000001803C0976 int3      
00000001803C0977 int3      
00000001803C0978 int3      
00000001803C0979 int3      
00000001803C097A int3      
00000001803C097B int3      
00000001803C097C int3      
00000001803C097D int3      
00000001803C097E int3      
00000001803C097F int3      