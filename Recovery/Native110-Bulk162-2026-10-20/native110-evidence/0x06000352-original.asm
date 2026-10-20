000000018035BBF0 mov       rax, rsp
000000018035BBF3 mov       qword ptr [rax + 8], rbx
000000018035BBF7 push      rdi
000000018035BBF8 sub       rsp, 0xd0
000000018035BBFF movaps    xmmword ptr [rax - 0x18], xmm6
000000018035BC03 movaps    xmmword ptr [rax - 0x28], xmm7
000000018035BC07 movaps    xmmword ptr [rax - 0x38], xmm8
000000018035BC0C movaps    xmmword ptr [rax - 0x48], xmm9
000000018035BC11 mov       rbx, rcx
000000018035BC14 cmp       byte ptr [rip + 0x19890d8], 0 # 0x181ce4cf3
000000018035BC1B jne       0x18035bc6c
000000018035BC1D lea       rcx, [rip + 0x186783c] # 0x181bc3460
000000018035BC24 call      0x18024fef0
000000018035BC29 lea       rcx, [rip + 0x1867888] # 0x181bc34b8
000000018035BC30 call      0x18024fef0
000000018035BC35 lea       rcx, [rip + 0x18678d4] # 0x181bc3510
000000018035BC3C call      0x18024fef0
000000018035BC41 lea       rcx, [rip + 0x1849570] # 0x181ba51b8
000000018035BC48 call      0x18024fef0
000000018035BC4D lea       rcx, [rip + 0x1850224] # 0x181babe78
000000018035BC54 call      0x18024fef0
000000018035BC59 lea       rcx, [rip + 0x18548a0] # 0x181bb0500
000000018035BC60 call      0x18024fef0
000000018035BC65 mov       byte ptr [rip + 0x1989087], 1 # 0x181ce4cf3
000000018035BC6C movss     xmm6, dword ptr [rip + 0x124bd9c] # 0x1815a7a10
000000018035BC74 movaps    xmm8, xmm6
000000018035BC78 movaps    xmm9, xmm6
000000018035BC7C mov       rax, qword ptr [rbx + 0x1e8]
000000018035BC83 test      rax, rax
000000018035BC86 je        0x18035be7e
000000018035BC8C mov       rcx, qword ptr [rax + 0x110]
000000018035BC93 test      rcx, rcx
000000018035BC96 je        0x18035be7e
000000018035BC9C mov       rdi, qword ptr [rcx + 0xa0]
000000018035BCA3 mov       rcx, qword ptr [rip + 0x1854856] # 0x181bb0500
000000018035BCAA cmp       dword ptr [rcx + 0xe0], 0
000000018035BCB1 jne       0x18035bcb8
000000018035BCB3 call      0x1802501e0
000000018035BCB8 xor       r8d, r8d
000000018035BCBB mov       rdx, rbx
000000018035BCBE mov       rcx, rdi
000000018035BCC1 call      0x18131f760
000000018035BCC6 test      al, al
000000018035BCC8 je        0x18035bcda
000000018035BCCA movss     xmm6, dword ptr [rip + 0x124bd32] # 0x1815a7a04
000000018035BCD2 movaps    xmm8, xmm6
000000018035BCD6 movaps    xmm9, xmm6
000000018035BCDA mov       rdx, qword ptr [rbx + 0xf8]
000000018035BCE1 test      rdx, rdx
000000018035BCE4 je        0x18035be7e
000000018035BCEA xor       r8d, r8d
000000018035BCED lea       rcx, [rsp + 0x50]
000000018035BCF2 call      0x180314f20
000000018035BCF7 movups    xmm1, xmmword ptr [rax]
000000018035BCFA movaps    xmm0, xmm1
000000018035BCFD shufps    xmm0, xmm1, 0xaa
000000018035BD01 mulss     xmm9, xmm0
000000018035BD06 movaps    xmm0, xmm1
000000018035BD09 shufps    xmm0, xmm1, 0x55
000000018035BD0D mulss     xmm8, xmm0
000000018035BD12 mulss     xmm6, xmm1
000000018035BD16 mov       rdx, qword ptr [rbx + 0x148]
000000018035BD1D test      rdx, rdx
000000018035BD20 je        0x18035be7e
000000018035BD26 mov       r8, qword ptr [rip + 0x185014b] # 0x181babe78
000000018035BD2D lea       rcx, [rsp + 0x60]
000000018035BD32 call      0x1808197f0
000000018035BD37 movups    xmm0, xmmword ptr [rsp + 0x60]
000000018035BD3C movups    xmmword ptr [rsp + 0x78], xmm0
000000018035BD41 movsd     xmm1, qword ptr [rsp + 0x70]
000000018035BD47 movsd     qword ptr [rsp + 0x88], xmm1
000000018035BD50 mov       qword ptr [rsp + 0x38], 0
000000018035BD59 lea       rbx, [rsp + 0x78]
000000018035BD5E mov       qword ptr [rsp + 0x40], rbx
000000018035BD63 mov       rdx, qword ptr [rip + 0x186774e] # 0x181bc34b8
000000018035BD6A lea       rcx, [rsp + 0x78]
000000018035BD6F call      0x180669280
000000018035BD74 test      al, al
000000018035BD76 je        0x18035be11
000000018035BD7C mov       rdi, qword ptr [rsp + 0x88]
000000018035BD84 test      rdi, rdi
000000018035BD87 je        0x18035be72
000000018035BD8D mov       rdx, qword ptr [rip + 0x1849424] # 0x181ba51b8
000000018035BD94 mov       rcx, rdi
000000018035BD97 call      0x18046dc80
000000018035BD9C test      rax, rax
000000018035BD9F je        0x18035be6d
000000018035BDA5 xor       r8d, r8d
000000018035BDA8 mov       rdx, rax
000000018035BDAB lea       rcx, [rsp + 0x60]
000000018035BDB0 call      0x181328fc0
000000018035BDB5 movss     xmm7, dword ptr [rax + 0xc]
000000018035BDBA test      rdi, rdi
000000018035BDBD je        0x18035be68
000000018035BDC3 mov       rdx, qword ptr [rip + 0x18493ee] # 0x181ba51b8
000000018035BDCA mov       rcx, rdi
000000018035BDCD call      0x18046dc80
000000018035BDD2 movss     dword ptr [rsp + 0x28], xmm6
000000018035BDD8 movss     dword ptr [rsp + 0x2c], xmm8
000000018035BDDF movss     dword ptr [rsp + 0x30], xmm9
000000018035BDE6 movss     dword ptr [rsp + 0x34], xmm7
000000018035BDEC test      rax, rax
000000018035BDEF je        0x18035be62
000000018035BDF1 movups    xmm0, xmmword ptr [rsp + 0x28]
000000018035BDF6 movdqa    xmmword ptr [rsp + 0x50], xmm0
000000018035BDFC xor       r8d, r8d
000000018035BDFF lea       rdx, [rsp + 0x50]
000000018035BE04 mov       rcx, rax
000000018035BE07 call      0x1813290a0
000000018035BE0C jmp       0x18035bd63
000000018035BE11 mov       rdx, qword ptr [rip + 0x1867648] # 0x181bc3460
000000018035BE18 mov       rcx, rbx
000000018035BE1B call      0x180302170
000000018035BE20 jmp       0x18035be3d
000000018035BE22 mov       rdx, qword ptr [rip + 0x1867637] # 0x181bc3460
000000018035BE29 mov       rcx, qword ptr [rsp + 0x40]
000000018035BE2E call      0x180302170
000000018035BE33 mov       rcx, qword ptr [rsp + 0x38]
000000018035BE38 test      rcx, rcx
000000018035BE3B jne       0x18035be78
000000018035BE3D lea       r11, [rsp + 0xd0]
000000018035BE45 mov       rbx, qword ptr [r11 + 0x10]
000000018035BE49 movaps    xmm6, xmmword ptr [r11 - 0x10]
000000018035BE4E movaps    xmm7, xmmword ptr [r11 - 0x20]
000000018035BE53 movaps    xmm8, xmmword ptr [r11 - 0x30]
000000018035BE58 movaps    xmm9, xmmword ptr [r11 - 0x40]
000000018035BE5D mov       rsp, r11
000000018035BE60 pop       rdi
000000018035BE61 ret       
000000018035BE62 call      0x180250150
000000018035BE67 nop       
000000018035BE68 call      0x180250150
000000018035BE6D call      0x180250150
000000018035BE72 call      0x180250150
000000018035BE77 nop       
000000018035BE78 call      0x180200e60
000000018035BE7D int3      
000000018035BE7E call      0x180250150
000000018035BE83 int3      