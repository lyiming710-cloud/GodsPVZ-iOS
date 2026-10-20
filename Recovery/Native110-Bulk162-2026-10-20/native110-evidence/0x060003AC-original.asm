0000000180357880 mov       rax, rsp
0000000180357883 mov       qword ptr [rax + 8], rbx
0000000180357887 mov       qword ptr [rax + 0x10], rsi
000000018035788B push      rdi
000000018035788C sub       rsp, 0xd0
0000000180357893 cmp       byte ptr [rip + 0x198d499], 0 # 0x181ce4d33
000000018035789A mov       rbx, rcx
000000018035789D movaps    xmmword ptr [rax - 0x28], xmm7
00000001803578A1 movaps    xmmword ptr [rax - 0x58], xmm10
00000001803578A6 jne       0x1803578f7
00000001803578A8 lea       rcx, [rip + 0x184a769] # 0x181ba2018
00000001803578AF call      0x18024fef0
00000001803578B4 lea       rcx, [rip + 0x18585a5] # 0x181bafe60
00000001803578BB call      0x18024fef0
00000001803578C0 lea       rcx, [rip + 0x1858649] # 0x181baff10
00000001803578C7 call      0x18024fef0
00000001803578CC lea       rcx, [rip + 0x1858c2d] # 0x181bb0500
00000001803578D3 call      0x18024fef0
00000001803578D8 lea       rcx, [rip + 0x1863f99] # 0x181bbb878
00000001803578DF call      0x18024fef0
00000001803578E4 lea       rcx, [rip + 0x18533bd] # 0x181baaca8
00000001803578EB call      0x18024fef0
00000001803578F0 mov       byte ptr [rip + 0x198d43c], 1 # 0x181ce4d33
00000001803578F7 mov       rcx, qword ptr [rip + 0x184a71a] # 0x181ba2018
00000001803578FE mov       rdi, qword ptr [rbx + 0x148]
0000000180357905 cmp       dword ptr [rcx + 0xe0], 0
000000018035790C jne       0x180357913
000000018035790E call      0x1802501e0
0000000180357913 mov       r8, qword ptr [rip + 0x185338e] # 0x181baaca8
000000018035791A lea       rcx, [rsp + 0x70]
000000018035791F xor       r9d, r9d
0000000180357922 mov       rdx, rdi
0000000180357925 call      0x18031b680
000000018035792A mov       rcx, qword ptr [rip + 0x1858bcf] # 0x181bb0500
0000000180357931 mov       rdi, qword ptr [rbx + 0x120]
0000000180357938 movsd     xmm10, qword ptr [rax]
000000018035793D cmp       dword ptr [rcx + 0xe0], 0
0000000180357944 mov       esi, dword ptr [rax + 8]
0000000180357947 movsd     qword ptr [rsp + 0x50], xmm10
000000018035794E jne       0x180357955
0000000180357950 call      0x1802501e0
0000000180357955 movaps    xmmword ptr [rsp + 0xc0], xmm6
000000018035795D xor       r8d, r8d
0000000180357960 movaps    xmmword ptr [rsp + 0xa0], xmm8
0000000180357969 xor       edx, edx
000000018035796B mov       rcx, rdi
000000018035796E movaps    xmmword ptr [rsp + 0x90], xmm9
0000000180357977 call      0x18131f900
000000018035797C movss     xmm7, dword ptr [rip + 0x1250294] # 0x1815a7c18
0000000180357984 test      al, al
0000000180357986 jne       0x1803579e7
0000000180357988 mov       rcx, qword ptr [rip + 0x1858b71] # 0x181bb0500
000000018035798F mov       rdi, qword ptr [rbx + 0x128]
0000000180357996 cmp       dword ptr [rcx + 0xe0], 0
000000018035799D jne       0x1803579a4
000000018035799F call      0x1802501e0
00000001803579A4 xor       r8d, r8d
00000001803579A7 xor       edx, edx
00000001803579A9 mov       rcx, rdi
00000001803579AC call      0x18131f900
00000001803579B1 test      al, al
00000001803579B3 je        0x180357a1c
00000001803579B5 mov       rcx, qword ptr [rbx + 0x128]
00000001803579BC test      rcx, rcx
00000001803579BF je        0x180357c40
00000001803579C5 xor       edx, edx
00000001803579C7 call      0x181304510
00000001803579CC test      rax, rax
00000001803579CF je        0x180357c40
00000001803579D5 xor       r8d, r8d
00000001803579D8 lea       rcx, [rsp + 0x70]
00000001803579DD mov       rdx, rax
00000001803579E0 call      0x18132f3a0
00000001803579E5 jmp       0x180357a07
00000001803579E7 mov       rdx, qword ptr [rbx + 0x120]
00000001803579EE test      rdx, rdx
00000001803579F1 je        0x180357c40
00000001803579F7 xor       r9d, r9d
00000001803579FA lea       rcx, [rsp + 0x70]
00000001803579FF movaps    xmm2, xmm7
0000000180357A02 call      0x1803617f0
0000000180357A07 movsd     xmm0, qword ptr [rax]
0000000180357A0B mov       ecx, dword ptr [rax + 8]
0000000180357A0E movsd     qword ptr [rbx + 0x138], xmm0
0000000180357A16 mov       dword ptr [rbx + 0x140], ecx
0000000180357A1C movsd     xmm0, qword ptr [rbx + 0x138]
0000000180357A24 movss     xmm9, dword ptr [rbx + 0x140]
0000000180357A2D movaps    xmm6, xmm0
0000000180357A30 movaps    xmm8, xmm0
0000000180357A34 movsd     qword ptr [rsp + 0x70], xmm0
0000000180357A3A subss     xmm8, dword ptr [rsp + 0x50]
0000000180357A41 shufps    xmm6, xmm6, 0x55
0000000180357A45 subss     xmm6, dword ptr [rsp + 0x54]
0000000180357A4B movd      xmm0, esi
0000000180357A4F subss     xmm9, xmm0
0000000180357A54 divss     xmm8, xmm7
0000000180357A59 divss     xmm6, xmm7
0000000180357A5D divss     xmm9, xmm7
0000000180357A62 mov       rcx, qword ptr [rbx + 0x1e8]
0000000180357A69 test      rcx, rcx
0000000180357A6C je        0x180357c40
0000000180357A72 mov       rcx, qword ptr [rcx + 0xf8]
0000000180357A79 test      rcx, rcx
0000000180357A7C je        0x180357c40
0000000180357A82 xor       r8d, r8d
0000000180357A85 mov       edx, 0xf
0000000180357A8A call      0x180324150
0000000180357A8F cmp       byte ptr [rip + 0x198d0a0], 0 # 0x181ce4b36
0000000180357A96 mov       rdi, rax
0000000180357A99 jne       0x180357aae
0000000180357A9B lea       rcx, [rip + 0x186ad46] # 0x181bc27e8
0000000180357AA2 call      0x18024fef0
0000000180357AA7 mov       byte ptr [rip + 0x198d088], 1 # 0x181ce4b36
0000000180357AAE mov       rcx, qword ptr [rip + 0x186ad33] # 0x181bc27e8
0000000180357AB5 mov       rax, qword ptr [rcx + 0xb8]
0000000180357ABC test      rdi, rdi
0000000180357ABF je        0x180357c40
0000000180357AC5 movsd     xmm0, qword ptr [rax]
0000000180357AC9 lea       r9, [rsp + 0x50]
0000000180357ACE mov       eax, dword ptr [rax + 8]
0000000180357AD1 lea       r8, [rsp + 0x60]
0000000180357AD6 movss     xmm1, dword ptr [rip + 0x1250036] # 0x1815a7b14
0000000180357ADE lea       rdx, [rsp + 0x70]
0000000180357AE3 mov       dword ptr [rsp + 0x58], eax
0000000180357AE7 mov       rcx, rdi
0000000180357AEA mov       rax, qword ptr [rip + 0x1863d87] # 0x181bbb878
0000000180357AF1 mov       qword ptr [rsp + 0x40], rax
0000000180357AF6 mov       qword ptr [rsp + 0x38], rbx
0000000180357AFB movsd     qword ptr [rsp + 0x50], xmm0
0000000180357B01 movss     xmm0, dword ptr [rip + 0x125019b] # 0x1815a7ca4
0000000180357B09 mov       dword ptr [rsp + 0x30], 1
0000000180357B11 unpcklps  xmm8, xmm6
0000000180357B15 movss     dword ptr [rsp + 0x28], xmm0
0000000180357B1B movss     dword ptr [rsp + 0x20], xmm1
0000000180357B21 movss     dword ptr [rsp + 0x68], xmm9
0000000180357B28 movsd     qword ptr [rsp + 0x60], xmm8
0000000180357B2F movsd     qword ptr [rsp + 0x70], xmm10
0000000180357B36 mov       dword ptr [rsp + 0x78], esi
0000000180357B3A call      0x1804a24e0
0000000180357B3F xor       r9d, r9d
0000000180357B42 mov       qword ptr [rsp + 0x20], 0
0000000180357B4B mov       r8d, 0xf
0000000180357B51 mov       rdx, rdi
0000000180357B54 mov       rcx, rbx
0000000180357B57 call      0x180350e40
0000000180357B5C cmp       byte ptr [rbx + 0xf4], 0
0000000180357B63 mov       rsi, rax
0000000180357B66 je        0x180357b93
0000000180357B68 mov       rcx, qword ptr [rbx + 0xe0]
0000000180357B6F test      rcx, rcx
0000000180357B72 je        0x180357c40
0000000180357B78 cmp       dword ptr [rcx + 0x18], 1
0000000180357B7C jne       0x180357b93
0000000180357B7E xor       r9d, r9d
0000000180357B81 mov       r8, rax
0000000180357B84 mov       rdx, rbx
0000000180357B87 call      0x1803865a0
0000000180357B8C mov       byte ptr [rbx + 0xf4], 0
0000000180357B93 mov       r8d, dword ptr [rbx + 0xbc]
0000000180357B9A xor       r9d, r9d
0000000180357B9D mov       rdx, rsi
0000000180357BA0 mov       rcx, rdi
0000000180357BA3 call      0x18037c0f0
0000000180357BA8 mov       rcx, qword ptr [rip + 0x184a469] # 0x181ba2018
0000000180357BAF mov       rdi, qword ptr [rbx + 0x148]
0000000180357BB6 cmp       dword ptr [rcx + 0xe0], 0
0000000180357BBD jne       0x180357bc4
0000000180357BBF call      0x1802501e0
0000000180357BC4 mov       rdx, qword ptr [rip + 0x18530dd] # 0x181baaca8
0000000180357BCB xor       r8d, r8d
0000000180357BCE mov       rcx, rdi
0000000180357BD1 call      0x18031bac0
0000000180357BD6 mov       rbx, qword ptr [rbx + 0x218]
0000000180357BDD test      rbx, rbx
0000000180357BE0 je        0x180357c40
0000000180357BE2 mov       r8, qword ptr [rip + 0x1858277] # 0x181bafe60
0000000180357BE9 xor       edx, edx
0000000180357BEB mov       rcx, rbx
0000000180357BEE call      0x180836790
0000000180357BF3 mov       r9, qword ptr [rip + 0x1858316] # 0x181baff10
0000000180357BFA xor       edx, edx
0000000180357BFC mov       rcx, rbx
0000000180357BFF lea       r8d, [rax - 1]
0000000180357C03 movaps    xmm9, xmmword ptr [rsp + 0x90]
0000000180357C0C lea       r11, [rsp + 0xd0]
0000000180357C14 mov       rbx, qword ptr [r11 + 0x10]
0000000180357C18 mov       rsi, qword ptr [r11 + 0x18]
0000000180357C1C movaps    xmm7, xmmword ptr [r11 - 0x20]
0000000180357C21 movaps    xmm10, xmmword ptr [r11 - 0x50]
0000000180357C26 movaps    xmm8, xmmword ptr [rsp + 0xa0]
0000000180357C2F movaps    xmm6, xmmword ptr [rsp + 0xc0]
0000000180357C37 mov       rsp, r11
0000000180357C3A pop       rdi
0000000180357C3B jmp       0x180836a10
0000000180357C40 call      0x180250150
0000000180357C45 int3      