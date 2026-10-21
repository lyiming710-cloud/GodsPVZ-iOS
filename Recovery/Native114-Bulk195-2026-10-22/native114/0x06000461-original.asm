0000000180364DC0 mov       qword ptr [rsp + 0x10], rbx
0000000180364DC5 push      rdi
0000000180364DC6 sub       rsp, 0x20
0000000180364DCA cmp       byte ptr [rip + 0x197ffa3], 0 # 0x181ce4d74
0000000180364DD1 movzx     edi, dl
0000000180364DD4 mov       rbx, rcx
0000000180364DD7 jne       0x180364e34
0000000180364DD9 lea       rcx, [rip + 0x18403d8] # 0x181ba51b8
0000000180364DE0 call      0x18024fef0
0000000180364DE5 lea       rcx, [rip + 0x183d22c] # 0x181ba2018
0000000180364DEC call      0x18024fef0
0000000180364DF1 lea       rcx, [rip + 0x184b708] # 0x181bb0500
0000000180364DF8 call      0x18024fef0
0000000180364DFD lea       rcx, [rip + 0x1851674] # 0x181bb6478
0000000180364E04 call      0x18024fef0
0000000180364E09 lea       rcx, [rip + 0x18702c0] # 0x181bd50d0
0000000180364E10 call      0x18024fef0
0000000180364E15 lea       rcx, [rip + 0x1868c04] # 0x181bcda20
0000000180364E1C call      0x18024fef0
0000000180364E21 lea       rcx, [rip + 0x1867f30] # 0x181bccd58
0000000180364E28 call      0x18024fef0
0000000180364E2D mov       byte ptr [rip + 0x197ff40], 1 # 0x181ce4d74
0000000180364E34 cmp       dword ptr [rbx + 0x9c], 0
0000000180364E3B je        0x180364e62
0000000180364E3D xorps     xmm0, xmm0
0000000180364E40 comiss    xmm0, dword ptr [rbx + 0xa0]
0000000180364E47 jb        0x180364e62
0000000180364E49 xor       r8d, r8d
0000000180364E4C movzx     edx, dil
0000000180364E50 mov       rcx, rbx
0000000180364E53 mov       rbx, qword ptr [rsp + 0x38]
0000000180364E58 add       rsp, 0x20
0000000180364E5C pop       rdi
0000000180364E5D jmp       0x18035f560
0000000180364E62 mov       ecx, dword ptr [rbx + 0x9c]
0000000180364E68 mov       qword ptr [rsp + 0x30], rsi
0000000180364E6D sub       ecx, 1
0000000180364E70 je        0x180365139
0000000180364E76 sub       ecx, 1
0000000180364E79 je        0x18036528b
0000000180364E7F sub       ecx, 1
0000000180364E82 je        0x180364fe5
0000000180364E88 cmp       ecx, 1
0000000180364E8B jne       0x18036528b
0000000180364E91 mov       rcx, qword ptr [rip + 0x183d180] # 0x181ba2018
0000000180364E98 mov       rdi, qword ptr [rbx + 0x1b0]
0000000180364E9F cmp       dword ptr [rcx + 0xe0], 0
0000000180364EA6 jne       0x180364ead
0000000180364EA8 call      0x1802501e0
0000000180364EAD mov       rdx, qword ptr [rip + 0x187021c] # 0x181bd50d0
0000000180364EB4 xor       r8d, r8d
0000000180364EB7 mov       rcx, rdi
0000000180364EBA call      0x18031b790
0000000180364EBF mov       rcx, qword ptr [rip + 0x184b63a] # 0x181bb0500
0000000180364EC6 xor       edi, edi
0000000180364EC8 mov       rsi, rax
0000000180364ECB cmp       dword ptr [rcx + 0xe0], edi
0000000180364ED1 jne       0x180364ed8
0000000180364ED3 call      0x1802501e0
0000000180364ED8 xor       edx, edx
0000000180364EDA mov       rcx, rsi
0000000180364EDD call      0x18131f870
0000000180364EE2 test      al, al
0000000180364EE4 je        0x180364f01
0000000180364EE6 test      rsi, rsi
0000000180364EE9 je        0x18036529b
0000000180364EEF mov       rdx, qword ptr [rip + 0x18402c2] # 0x181ba51b8
0000000180364EF6 mov       rcx, rsi
0000000180364EF9 call      0x18046dc80
0000000180364EFE mov       rdi, rax
0000000180364F01 cmp       dword ptr [rbx + 0xb0], 2
0000000180364F08 movss     xmm1, dword ptr [rip + 0x1242b14] # 0x1815a7a24
0000000180364F10 jge       0x180364f71
0000000180364F12 movss     xmm0, dword ptr [rbx + 0xa4]
0000000180364F1A divss     xmm0, xmm1
0000000180364F1E comiss    xmm0, dword ptr [rbx + 0xa0]
0000000180364F25 jbe       0x180364f71
0000000180364F27 mov       rax, qword ptr [rip + 0x185154a] # 0x181bb6478
0000000180364F2E cmp       dword ptr [rax + 0xe0], 0
0000000180364F35 jne       0x180364f46
0000000180364F37 mov       rcx, rax
0000000180364F3A call      0x1802501e0
0000000180364F3F mov       rax, qword ptr [rip + 0x1851532] # 0x181bb6478
0000000180364F46 mov       rcx, qword ptr [rip + 0x183d0cb] # 0x181ba2018
0000000180364F4D mov       rax, qword ptr [rax + 0xb8]
0000000180364F54 cmp       dword ptr [rcx + 0xe0], 0
0000000180364F5B mov       rbx, qword ptr [rax + 0x18]
0000000180364F5F jne       0x180364f66
0000000180364F61 call      0x1802501e0
0000000180364F66 mov       r8d, 0x23
0000000180364F6C jmp       0x18036527d
0000000180364F71 cmp       dword ptr [rbx + 0xb0], 1
0000000180364F78 jge       0x18036528b
0000000180364F7E movss     xmm0, dword ptr [rbx + 0xa4]
0000000180364F86 addss     xmm0, xmm0
0000000180364F8A divss     xmm0, xmm1
0000000180364F8E comiss    xmm0, dword ptr [rbx + 0xa0]
0000000180364F95 jbe       0x18036528b
0000000180364F9B mov       rax, qword ptr [rip + 0x18514d6] # 0x181bb6478
0000000180364FA2 cmp       dword ptr [rax + 0xe0], 0
0000000180364FA9 jne       0x180364fba
0000000180364FAB mov       rcx, rax
0000000180364FAE call      0x1802501e0
0000000180364FB3 mov       rax, qword ptr [rip + 0x18514be] # 0x181bb6478
0000000180364FBA mov       rcx, qword ptr [rip + 0x183d057] # 0x181ba2018
0000000180364FC1 mov       rax, qword ptr [rax + 0xb8]
0000000180364FC8 cmp       dword ptr [rcx + 0xe0], 0
0000000180364FCF mov       rbx, qword ptr [rax + 0x18]
0000000180364FD3 jne       0x180364fda
0000000180364FD5 call      0x1802501e0
0000000180364FDA mov       r8d, 0x22
0000000180364FE0 jmp       0x18036527d
0000000180364FE5 mov       rcx, qword ptr [rip + 0x183d02c] # 0x181ba2018
0000000180364FEC mov       rdi, qword ptr [rbx + 0x1b0]
0000000180364FF3 cmp       dword ptr [rcx + 0xe0], 0
0000000180364FFA jne       0x180365001
0000000180364FFC call      0x1802501e0
0000000180365001 mov       rdx, qword ptr [rip + 0x1867d50] # 0x181bccd58
0000000180365008 xor       r8d, r8d
000000018036500B mov       rcx, rdi
000000018036500E call      0x18031b790
0000000180365013 mov       rcx, qword ptr [rip + 0x184b4e6] # 0x181bb0500
000000018036501A xor       edi, edi
000000018036501C mov       rsi, rax
000000018036501F cmp       dword ptr [rcx + 0xe0], edi
0000000180365025 jne       0x18036502c
0000000180365027 call      0x1802501e0
000000018036502C xor       edx, edx
000000018036502E mov       rcx, rsi
0000000180365031 call      0x18131f870
0000000180365036 test      al, al
0000000180365038 je        0x180365055
000000018036503A test      rsi, rsi
000000018036503D je        0x18036529b
0000000180365043 mov       rdx, qword ptr [rip + 0x184016e] # 0x181ba51b8
000000018036504A mov       rcx, rsi
000000018036504D call      0x18046dc80
0000000180365052 mov       rdi, rax
0000000180365055 cmp       dword ptr [rbx + 0xb0], 2
000000018036505C movss     xmm1, dword ptr [rip + 0x12429c0] # 0x1815a7a24
0000000180365064 jge       0x1803650c5
0000000180365066 movss     xmm0, dword ptr [rbx + 0xa4]
000000018036506E divss     xmm0, xmm1
0000000180365072 comiss    xmm0, dword ptr [rbx + 0xa0]
0000000180365079 jbe       0x1803650c5
000000018036507B mov       rax, qword ptr [rip + 0x18513f6] # 0x181bb6478
0000000180365082 cmp       dword ptr [rax + 0xe0], 0
0000000180365089 jne       0x18036509a
000000018036508B mov       rcx, rax
000000018036508E call      0x1802501e0
0000000180365093 mov       rax, qword ptr [rip + 0x18513de] # 0x181bb6478
000000018036509A mov       rcx, qword ptr [rip + 0x183cf77] # 0x181ba2018
00000001803650A1 mov       rax, qword ptr [rax + 0xb8]
00000001803650A8 cmp       dword ptr [rcx + 0xe0], 0
00000001803650AF mov       rbx, qword ptr [rax + 0x18]
00000001803650B3 jne       0x1803650ba
00000001803650B5 call      0x1802501e0
00000001803650BA mov       r8d, 0x1f
00000001803650C0 jmp       0x18036527d
00000001803650C5 cmp       dword ptr [rbx + 0xb0], 1
00000001803650CC jge       0x18036528b
00000001803650D2 movss     xmm0, dword ptr [rbx + 0xa4]
00000001803650DA addss     xmm0, xmm0
00000001803650DE divss     xmm0, xmm1
00000001803650E2 comiss    xmm0, dword ptr [rbx + 0xa0]
00000001803650E9 jbe       0x18036528b
00000001803650EF mov       rax, qword ptr [rip + 0x1851382] # 0x181bb6478
00000001803650F6 cmp       dword ptr [rax + 0xe0], 0
00000001803650FD jne       0x18036510e
00000001803650FF mov       rcx, rax
0000000180365102 call      0x1802501e0
0000000180365107 mov       rax, qword ptr [rip + 0x185136a] # 0x181bb6478
000000018036510E mov       rcx, qword ptr [rip + 0x183cf03] # 0x181ba2018
0000000180365115 mov       rax, qword ptr [rax + 0xb8]
000000018036511C cmp       dword ptr [rcx + 0xe0], 0
0000000180365123 mov       rbx, qword ptr [rax + 0x18]
0000000180365127 jne       0x18036512e
0000000180365129 call      0x1802501e0
000000018036512E mov       r8d, 0x1e
0000000180365134 jmp       0x18036527d
0000000180365139 mov       rcx, qword ptr [rip + 0x183ced8] # 0x181ba2018
0000000180365140 mov       rdi, qword ptr [rbx + 0x1b0]
0000000180365147 cmp       dword ptr [rcx + 0xe0], 0
000000018036514E jne       0x180365155
0000000180365150 call      0x1802501e0
0000000180365155 mov       rdx, qword ptr [rip + 0x18688c4] # 0x181bcda20
000000018036515C xor       r8d, r8d
000000018036515F mov       rcx, rdi
0000000180365162 call      0x18031b790
0000000180365167 mov       rcx, qword ptr [rip + 0x184b392] # 0x181bb0500
000000018036516E xor       edi, edi
0000000180365170 mov       rsi, rax
0000000180365173 cmp       dword ptr [rcx + 0xe0], edi
0000000180365179 jne       0x180365180
000000018036517B call      0x1802501e0
0000000180365180 xor       edx, edx
0000000180365182 mov       rcx, rsi
0000000180365185 call      0x18131f870
000000018036518A test      al, al
000000018036518C je        0x1803651a9
000000018036518E test      rsi, rsi
0000000180365191 je        0x18036529b
0000000180365197 mov       rdx, qword ptr [rip + 0x184001a] # 0x181ba51b8
000000018036519E mov       rcx, rsi
00000001803651A1 call      0x18046dc80
00000001803651A6 mov       rdi, rax
00000001803651A9 cmp       dword ptr [rbx + 0xb0], 2
00000001803651B0 movss     xmm1, dword ptr [rip + 0x124286c] # 0x1815a7a24
00000001803651B8 jge       0x180365216
00000001803651BA movss     xmm0, dword ptr [rbx + 0xa4]
00000001803651C2 divss     xmm0, xmm1
00000001803651C6 comiss    xmm0, dword ptr [rbx + 0xa0]
00000001803651CD jbe       0x180365216
00000001803651CF mov       rax, qword ptr [rip + 0x18512a2] # 0x181bb6478
00000001803651D6 cmp       dword ptr [rax + 0xe0], 0
00000001803651DD jne       0x1803651ee
00000001803651DF mov       rcx, rax
00000001803651E2 call      0x1802501e0
00000001803651E7 mov       rax, qword ptr [rip + 0x185128a] # 0x181bb6478
00000001803651EE mov       rcx, qword ptr [rip + 0x183ce23] # 0x181ba2018
00000001803651F5 mov       rax, qword ptr [rax + 0xb8]
00000001803651FC cmp       dword ptr [rcx + 0xe0], 0
0000000180365203 mov       rbx, qword ptr [rax + 0x18]
0000000180365207 jne       0x18036520e
0000000180365209 call      0x1802501e0
000000018036520E mov       r8d, 0xb
0000000180365214 jmp       0x18036527d
0000000180365216 cmp       dword ptr [rbx + 0xb0], 1
000000018036521D jge       0x18036528b
000000018036521F movss     xmm0, dword ptr [rbx + 0xa4]
0000000180365227 addss     xmm0, xmm0
000000018036522B divss     xmm0, xmm1
000000018036522F comiss    xmm0, dword ptr [rbx + 0xa0]
0000000180365236 jbe       0x18036528b
0000000180365238 mov       rax, qword ptr [rip + 0x1851239] # 0x181bb6478
000000018036523F cmp       dword ptr [rax + 0xe0], 0
0000000180365246 jne       0x180365257
0000000180365248 mov       rcx, rax
000000018036524B call      0x1802501e0
0000000180365250 mov       rax, qword ptr [rip + 0x1851221] # 0x181bb6478
0000000180365257 mov       rcx, qword ptr [rip + 0x183cdba] # 0x181ba2018
000000018036525E mov       rax, qword ptr [rax + 0xb8]
0000000180365265 cmp       dword ptr [rcx + 0xe0], 0
000000018036526C mov       rbx, qword ptr [rax + 0x18]
0000000180365270 jne       0x180365277
0000000180365272 call      0x1802501e0
0000000180365277 mov       r8d, 0xa
000000018036527D xor       r9d, r9d
0000000180365280 mov       rdx, rbx
0000000180365283 mov       rcx, rdi
0000000180365286 call      0x18031c2b0
000000018036528B mov       rsi, qword ptr [rsp + 0x30]
0000000180365290 mov       rbx, qword ptr [rsp + 0x38]
0000000180365295 add       rsp, 0x20
0000000180365299 pop       rdi
000000018036529A ret       
000000018036529B call      0x180250150
00000001803652A0 int3      
00000001803652A1 int3      
00000001803652A2 int3      
00000001803652A3 int3      
00000001803652A4 int3      
00000001803652A5 int3      
00000001803652A6 int3      
00000001803652A7 int3      
00000001803652A8 int3      
00000001803652A9 int3      
00000001803652AA int3      
00000001803652AB int3      
00000001803652AC int3      
00000001803652AD int3      
00000001803652AE int3      
00000001803652AF int3      