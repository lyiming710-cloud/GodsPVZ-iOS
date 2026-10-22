00000001803527D0 mov       rax, rsp
00000001803527D3 mov       qword ptr [rax + 8], rbx
00000001803527D7 mov       qword ptr [rax + 0x10], rsi
00000001803527DB mov       qword ptr [rax + 0x18], rdi
00000001803527DF mov       qword ptr [rax + 0x20], r12
00000001803527E3 push      r13
00000001803527E5 push      r14
00000001803527E7 push      r15
00000001803527E9 sub       rsp, 0x90
00000001803527F0 movaps    xmmword ptr [rax - 0x28], xmm6
00000001803527F4 mov       rsi, rcx
00000001803527F7 cmp       byte ptr [rip + 0x1992514], 0 # 0x181ce4d12
00000001803527FE jne       0x18035284f
0000000180352800 lea       rcx, [rip + 0x184c021] # 0x181b9e828
0000000180352807 call      0x18024fef0
000000018035280C lea       rcx, [rip + 0x1878055] # 0x181bca868
0000000180352813 call      0x18024fef0
0000000180352818 lea       rcx, [rip + 0x1878099] # 0x181bca8b8
000000018035281F call      0x18024fef0
0000000180352824 lea       rcx, [rip + 0x18780e5] # 0x181bca910
000000018035282B call      0x18024fef0
0000000180352830 lea       rcx, [rip + 0x1870eb1] # 0x181bc36e8
0000000180352837 call      0x18024fef0
000000018035283C lea       rcx, [rip + 0x185a3b5] # 0x181bacbf8
0000000180352843 call      0x18024fef0
0000000180352848 mov       byte ptr [rip + 0x19924c3], 1 # 0x181ce4d12
000000018035284F cmp       byte ptr [rip + 0x19922de], 0 # 0x181ce4b34
0000000180352856 jne       0x18035286b
0000000180352858 lea       rcx, [rip + 0x185f031] # 0x181bb1890
000000018035285F call      0x18024fef0
0000000180352864 mov       byte ptr [rip + 0x19922c9], 1 # 0x181ce4b34
000000018035286B mov       rax, qword ptr [rip + 0x185f01e] # 0x181bb1890
0000000180352872 mov       rcx, qword ptr [rax + 0xb8]
0000000180352879 mov       rcx, qword ptr [rcx]
000000018035287C test      rcx, rcx
000000018035287F je        0x180352b04
0000000180352885 xor       r8d, r8d
0000000180352888 mov       edx, 0x10
000000018035288D call      0x180321f30
0000000180352892 test      rax, rax
0000000180352895 je        0x180352b04
000000018035289B xor       edx, edx
000000018035289D mov       rcx, rax
00000001803528A0 call      0x18131c070
00000001803528A5 mov       rbx, rax
00000001803528A8 mov       rcx, qword ptr [rsi + 0x1e8]
00000001803528AF test      rcx, rcx
00000001803528B2 je        0x180352b04
00000001803528B8 mov       rdx, qword ptr [rcx + 0x28]
00000001803528BC mov       r8d, dword ptr [rsi + 0x20]
00000001803528C0 test      rdx, rdx
00000001803528C3 je        0x180352b04
00000001803528C9 inc       r8d
00000001803528CC xor       edi, edi
00000001803528CE mov       qword ptr [rsp + 0x20], rdi
00000001803528D3 mov       r9d, dword ptr [rsi + 0x24]
00000001803528D7 lea       rcx, [rsp + 0x50]
00000001803528DC call      0x18030ec30
00000001803528E1 test      rbx, rbx
00000001803528E4 je        0x180352b04
00000001803528EA movsd     xmm0, qword ptr [rax]
00000001803528EE movsd     qword ptr [rsp + 0x40], xmm0
00000001803528F4 mov       eax, dword ptr [rax + 8]
00000001803528F7 mov       dword ptr [rsp + 0x48], eax
00000001803528FB xor       r8d, r8d
00000001803528FE lea       rdx, [rsp + 0x40]
0000000180352903 mov       rcx, rbx
0000000180352906 call      0x18132fbb0
000000018035290B mov       rax, qword ptr [rsi + 0x1e8]
0000000180352912 test      rax, rax
0000000180352915 je        0x180352b04
000000018035291B mov       rax, qword ptr [rax + 0xf0]
0000000180352922 test      rax, rax
0000000180352925 je        0x180352b04
000000018035292B mov       rdx, qword ptr [rax + 0x38]
000000018035292F test      rdx, rdx
0000000180352932 je        0x180352b04
0000000180352938 mov       r8, qword ptr [rip + 0x1870da9] # 0x181bc36e8
000000018035293F lea       rcx, [rsp + 0x50]
0000000180352944 call      0x1808197f0
0000000180352949 movups    xmm0, xmmword ptr [rsp + 0x50]
000000018035294E movups    xmmword ptr [rsp + 0x68], xmm0
0000000180352953 movsd     xmm1, qword ptr [rsp + 0x60]
0000000180352959 movsd     qword ptr [rsp + 0x78], xmm1
000000018035295F mov       qword ptr [rsp + 0x50], rdi
0000000180352964 lea       rbx, [rsp + 0x68]
0000000180352969 mov       qword ptr [rsp + 0x58], rbx
000000018035296E nop       
0000000180352970 mov       rdx, qword ptr [rip + 0x1877f41] # 0x181bca8b8
0000000180352977 lea       rcx, [rsp + 0x68]
000000018035297C call      0x180669280
0000000180352981 test      al, al
0000000180352983 je        0x180352a95
0000000180352989 mov       rdi, qword ptr [rsp + 0x78]
000000018035298E test      rdi, rdi
0000000180352991 je        0x180352af8
0000000180352997 xor       edx, edx
0000000180352999 mov       rcx, rdi
000000018035299C call      0x18035de80
00000001803529A1 test      al, al
00000001803529A3 je        0x180352970
00000001803529A5 test      rdi, rdi
00000001803529A8 je        0x180352af3
00000001803529AE xor       edx, edx
00000001803529B0 mov       rcx, rdi
00000001803529B3 call      0x180364280
00000001803529B8 test      al, al
00000001803529BA jne       0x180352970
00000001803529BC mov       eax, dword ptr [rdi + 0x28]
00000001803529BF cmp       eax, dword ptr [rsi + 0x20]
00000001803529C2 jl        0x180352970
00000001803529C4 sub       eax, dword ptr [rsi + 0x20]
00000001803529C7 cmp       eax, 2
00000001803529CA jg        0x180352970
00000001803529CC mov       r14d, dword ptr [rdi + 0x2c]
00000001803529D0 mov       r15d, dword ptr [rsi + 0x24]
00000001803529D4 mov       rcx, qword ptr [rip + 0x185a21d] # 0x181bacbf8
00000001803529DB cmp       dword ptr [rcx + 0xe0], 0
00000001803529E2 jne       0x1803529e9
00000001803529E4 call      0x1802501e0
00000001803529E9 sub       r14d, r15d
00000001803529EC mov       eax, r14d
00000001803529EF neg       eax
00000001803529F1 cmovs     eax, r14d
00000001803529F5 cmp       eax, 1
00000001803529F8 jg        0x180352970
00000001803529FE mov       r15d, dword ptr [rsi + 0x20]
0000000180352A02 mov       r12d, dword ptr [rsi + 0x24]
0000000180352A06 movss     xmm6, dword ptr [rdi + 0x114]
0000000180352A0E mov       rax, qword ptr [rsi + 0x1e8]
0000000180352A15 test      rax, rax
0000000180352A18 je        0x180352aee
0000000180352A1E mov       r13, qword ptr [rax + 0x28]
0000000180352A22 mov       rcx, qword ptr [rip + 0x184bdff] # 0x181b9e828
0000000180352A29 call      0x180250100
0000000180352A2E mov       r14, rax
0000000180352A31 mov       qword ptr [rsp + 0x28], 0
0000000180352A3A mov       qword ptr [rsp + 0x20], r13
0000000180352A3F movaps    xmm3, xmm6
0000000180352A42 mov       r8d, r12d
0000000180352A45 mov       edx, r15d
0000000180352A48 mov       rcx, rax
0000000180352A4B call      0x1803299e0
0000000180352A50 test      r14, r14
0000000180352A53 je        0x180352ae8
0000000180352A59 mov       byte ptr [r14 + 0x2a], 1
0000000180352A5E mov       qword ptr [r14 + 0x38], rsi
0000000180352A62 lea       rcx, [r14 + 0x38]
0000000180352A66 mov       rdx, rsi
0000000180352A69 call      0x18024f360
0000000180352A6E mov       dword ptr [rdi + 0x114], 0x3c23d70a
0000000180352A78 xor       r8d, r8d
0000000180352A7B mov       rdx, r14
0000000180352A7E mov       rcx, rdi
0000000180352A81 call      0x180365880
0000000180352A86 xor       edx, edx
0000000180352A88 mov       rcx, rdi
0000000180352A8B call      0x180365ea0
0000000180352A90 jmp       0x180352970
0000000180352A95 mov       rdx, qword ptr [rip + 0x1877dcc] # 0x181bca868
0000000180352A9C mov       rcx, rbx
0000000180352A9F call      0x180302170
0000000180352AA4 jmp       0x180352ac1
0000000180352AA6 mov       rdx, qword ptr [rip + 0x1877dbb] # 0x181bca868
0000000180352AAD mov       rcx, qword ptr [rsp + 0x58]
0000000180352AB2 call      0x180302170
0000000180352AB7 mov       rcx, qword ptr [rsp + 0x50]
0000000180352ABC test      rcx, rcx
0000000180352ABF jne       0x180352afe
0000000180352AC1 lea       r11, [rsp + 0x90]
0000000180352AC9 mov       rbx, qword ptr [r11 + 0x20]
0000000180352ACD mov       rsi, qword ptr [r11 + 0x28]
0000000180352AD1 mov       rdi, qword ptr [r11 + 0x30]
0000000180352AD5 mov       r12, qword ptr [r11 + 0x38]
0000000180352AD9 movaps    xmm6, xmmword ptr [r11 - 0x10]
0000000180352ADE mov       rsp, r11
0000000180352AE1 pop       r15
0000000180352AE3 pop       r14
0000000180352AE5 pop       r13
0000000180352AE7 ret       
0000000180352AE8 call      0x180250150
0000000180352AED nop       
0000000180352AEE call      0x180250150
0000000180352AF3 call      0x180250150
0000000180352AF8 call      0x180250150
0000000180352AFD nop       
0000000180352AFE call      0x180200e60
0000000180352B03 int3      
0000000180352B04 call      0x180250150
0000000180352B09 int3      
0000000180352B0A int3      
0000000180352B0B int3      
0000000180352B0C int3      
0000000180352B0D int3      
0000000180352B0E int3      
0000000180352B0F int3      