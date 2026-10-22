00000001803787D0 mov       qword ptr [rsp + 0x10], rbx
00000001803787D5 push      rdi
00000001803787D6 sub       rsp, 0x80
00000001803787DD cmp       byte ptr [rip + 0x196c5d7], 0 # 0x181ce4dbb
00000001803787E4 mov       rbx, rcx
00000001803787E7 movaps    xmmword ptr [rsp + 0x70], xmm6
00000001803787EC movaps    xmmword ptr [rsp + 0x50], xmm8
00000001803787F2 movaps    xmmword ptr [rsp + 0x40], xmm9
00000001803787F8 jne       0x180378849
00000001803787FA lea       rcx, [rip + 0x182c9b7] # 0x181ba51b8
0000000180378801 call      0x18024fef0
0000000180378806 lea       rcx, [rip + 0x183d04b] # 0x181bb5858
000000018037880D call      0x18024fef0
0000000180378812 lea       rcx, [rip + 0x1833817] # 0x181bac030
0000000180378819 call      0x18024fef0
000000018037881E lea       rcx, [rip + 0x1833863] # 0x181bac088
0000000180378825 call      0x18024fef0
000000018037882A lea       rcx, [rip + 0x18343c7] # 0x181bacbf8
0000000180378831 call      0x18024fef0
0000000180378836 lea       rcx, [rip + 0x1837cc3] # 0x181bb0500
000000018037883D call      0x18024fef0
0000000180378842 mov       byte ptr [rip + 0x196c572], 1 # 0x181ce4dbb
0000000180378849 movss     xmm6, dword ptr [rbx + 0x34]
000000018037884E xor       ecx, ecx
0000000180378850 mov       qword ptr [rsp + 0x90], rsi
0000000180378858 movaps    xmmword ptr [rsp + 0x60], xmm7
000000018037885D call      0x18132c2f0
0000000180378862 addss     xmm0, xmm6
0000000180378866 xor       edx, edx
0000000180378868 mov       rcx, rbx
000000018037886B movss     dword ptr [rbx + 0x34], xmm0
0000000180378870 call      0x1803779a0
0000000180378875 movss     xmm8, dword ptr [rip + 0x122f192] # 0x1815a7a10
000000018037887E xor       edi, edi
0000000180378880 xorps     xmm9, xmm9
0000000180378884 cmp       dword ptr [rbx + 0x20], edi
0000000180378887 jne       0x1803788da
0000000180378889 xor       edx, edx
000000018037888B mov       rcx, rbx
000000018037888E call      0x181304510
0000000180378893 xor       ecx, ecx
0000000180378895 mov       rsi, rax
0000000180378898 call      0x18132c2f0
000000018037889D test      rsi, rsi
00000001803788A0 je        0x180378cc5
00000001803788A6 mulss     xmm0, dword ptr [rip + 0x122f5b2] # 0x1815a7e60
00000001803788AE lea       rdx, [rsp + 0x30]
00000001803788B3 xor       r9d, r9d
00000001803788B6 mov       rcx, rsi
00000001803788B9 xorps     xmm1, xmm1
00000001803788BC movss     dword ptr [rsp + 0x38], xmm8
00000001803788C3 unpcklps  xmm1, xmm9
00000001803788C7 movaps    xmm2, xmm0
00000001803788CA movsd     qword ptr [rsp + 0x30], xmm1
00000001803788D0 mov       qword ptr [rsp + 0x20], rdi
00000001803788D5 call      0x18132e150
00000001803788DA cmp       dword ptr [rbx + 0x58], 5
00000001803788DE jne       0x180378a12
00000001803788E4 movss     xmm6, dword ptr [rbx + 0x38]
00000001803788E9 xor       ecx, ecx
00000001803788EB call      0x18132c2f0
00000001803788F0 cmp       dword ptr [rbx + 0x24], 4
00000001803788F4 addss     xmm0, xmm6
00000001803788F8 movss     dword ptr [rbx + 0x38], xmm0
00000001803788FD je        0x180378909
00000001803788FF cmp       dword ptr [rbx + 0x24], 5
0000000180378903 jne       0x180378a12
0000000180378909 cmp       byte ptr [rip + 0x196c4ae], dil # 0x181ce4dbe
0000000180378910 jne       0x18037893d
0000000180378912 lea       rcx, [rip + 0x182c89f] # 0x181ba51b8
0000000180378919 call      0x18024fef0
000000018037891E lea       rcx, [rip + 0x1833763] # 0x181bac088
0000000180378925 call      0x18024fef0
000000018037892A lea       rcx, [rip + 0x18342c7] # 0x181bacbf8
0000000180378931 call      0x18024fef0
0000000180378936 mov       byte ptr [rip + 0x196c481], 1 # 0x181ce4dbe
000000018037893D movss     xmm6, dword ptr [rbx + 0x40]
0000000180378942 xor       ecx, ecx
0000000180378944 call      0x18132c2f0
0000000180378949 addss     xmm0, xmm6
000000018037894D comiss    xmm0, dword ptr [rip + 0x122f2bc] # 0x1815a7c10
0000000180378954 movss     dword ptr [rbx + 0x40], xmm0
0000000180378959 jbe       0x18037895e
000000018037895B mov       dword ptr [rbx + 0x40], edi
000000018037895E mov       rcx, qword ptr [rip + 0x1834293] # 0x181bacbf8
0000000180378965 movss     xmm6, dword ptr [rbx + 0x40]
000000018037896A cmp       dword ptr [rcx + 0xe0], edi
0000000180378970 jne       0x180378977
0000000180378972 call      0x1802501e0
0000000180378977 subss     xmm6, dword ptr [rip + 0x122f389] # 0x1815a7d08
000000018037897F mov       rcx, qword ptr [rbx + 0x98]
0000000180378986 movaps    xmm7, xmm8
000000018037898A andps     xmm6, xmmword ptr [rip + 0x122f1ef] # 0x1815a7b80
0000000180378991 divss     xmm6, dword ptr [rip + 0x122f36f] # 0x1815a7d08
0000000180378999 mulss     xmm6, dword ptr [rip + 0x122f447] # 0x1815a7de8
00000001803789A1 subss     xmm7, xmm6
00000001803789A5 test      rcx, rcx
00000001803789A8 je        0x180378cc5
00000001803789AE mov       r8, qword ptr [rip + 0x18336d3] # 0x181bac088
00000001803789B5 xor       edx, edx
00000001803789B7 call      0x180825710
00000001803789BC test      rax, rax
00000001803789BF je        0x180378cc5
00000001803789C5 mov       rdx, qword ptr [rip + 0x182c7ec] # 0x181ba51b8
00000001803789CC mov       rcx, rax
00000001803789CF call      0x18046dc80
00000001803789D4 movss     dword ptr [rsp + 0x30], xmm7
00000001803789DA movss     dword ptr [rsp + 0x34], xmm7
00000001803789E0 movss     dword ptr [rsp + 0x38], xmm7
00000001803789E6 mov       dword ptr [rsp + 0x3c], 0x3f800000
00000001803789EE test      rax, rax
00000001803789F1 je        0x180378cc5
00000001803789F7 movups    xmm0, xmmword ptr [rsp + 0x30]
00000001803789FC xor       r8d, r8d
00000001803789FF lea       rdx, [rsp + 0x30]
0000000180378A04 mov       rcx, rax
0000000180378A07 movdqa    xmmword ptr [rsp + 0x30], xmm0
0000000180378A0D call      0x1813290a0
0000000180378A12 cmp       dword ptr [rbx + 0x58], 1
0000000180378A16 jne       0x180378af3
0000000180378A1C movss     xmm7, dword ptr [rip + 0x122efe4] # 0x1815a7a08
0000000180378A24 comiss    xmm7, dword ptr [rbx + 0x34]
0000000180378A28 jbe       0x180378af3
0000000180378A2E mov       rcx, qword ptr [rip + 0x18341c3] # 0x181bacbf8
0000000180378A35 movss     xmm6, dword ptr [rbx + 0x34]
0000000180378A3A cmp       dword ptr [rcx + 0xe0], edi
0000000180378A40 jne       0x180378a47
0000000180378A42 call      0x1802501e0
0000000180378A47 addss     xmm6, xmm7
0000000180378A4B xor       r8d, r8d
0000000180378A4E movaps    xmm1, xmm8
0000000180378A52 movaps    xmm0, xmm6
0000000180378A55 call      0x180ca60a0
0000000180378A5A xor       edx, edx
0000000180378A5C mov       rcx, rbx
0000000180378A5F movaps    xmm6, xmm0
0000000180378A62 call      0x181304510
0000000180378A67 cmp       byte ptr [rip + 0x196c3ac], dil # 0x181ce4e1a
0000000180378A6E mov       rsi, rax
0000000180378A71 jne       0x180378a86
0000000180378A73 lea       rcx, [rip + 0x1849d6e] # 0x181bc27e8
0000000180378A7A call      0x18024fef0
0000000180378A7F mov       byte ptr [rip + 0x196c394], 1 # 0x181ce4e1a
0000000180378A86 movss     xmm1, dword ptr [rbx + 0x2c]
0000000180378A8B mov       rcx, qword ptr [rip + 0x1849d56] # 0x181bc27e8
0000000180378A92 mov       rdx, qword ptr [rcx + 0xb8]
0000000180378A99 movsd     xmm0, qword ptr [rdx + 0xc]
0000000180378A9E movss     xmm4, dword ptr [rdx + 0x14]
0000000180378AA3 movaps    xmm2, xmm0
0000000180378AA6 shufps    xmm2, xmm2, 0x55
0000000180378AAA movaps    xmm3, xmm0
0000000180378AAD mulss     xmm2, xmm1
0000000180378AB1 mulss     xmm3, xmm1
0000000180378AB5 movsd     qword ptr [rsp + 0x30], xmm0
0000000180378ABB mulss     xmm4, xmm1
0000000180378ABF mulss     xmm3, xmm6
0000000180378AC3 mulss     xmm2, xmm6
0000000180378AC7 mulss     xmm4, xmm6
0000000180378ACB test      rsi, rsi
0000000180378ACE je        0x180378cc5
0000000180378AD4 unpcklps  xmm3, xmm2
0000000180378AD7 lea       rdx, [rsp + 0x30]
0000000180378ADC xor       r8d, r8d
0000000180378ADF movsd     qword ptr [rsp + 0x30], xmm3
0000000180378AE5 mov       rcx, rsi
0000000180378AE8 movss     dword ptr [rsp + 0x38], xmm4
0000000180378AEE call      0x18132fa50
0000000180378AF3 cmp       dword ptr [rbx + 0x58], 4
0000000180378AF7 jne       0x180378b03
0000000180378AF9 xor       edx, edx
0000000180378AFB mov       rcx, rbx
0000000180378AFE call      0x1803763e0
0000000180378B03 movss     xmm0, dword ptr [rbx + 0x3c]
0000000180378B08 comiss    xmm0, xmm9
0000000180378B0C jbe       0x180378c85
0000000180378B12 xor       ecx, ecx
0000000180378B14 movaps    xmm6, xmm0
0000000180378B17 call      0x18132c2f0
0000000180378B1C subss     xmm6, xmm0
0000000180378B20 movss     xmm0, dword ptr [rip + 0x122ef88] # 0x1815a7ab0
0000000180378B28 comiss    xmm0, xmm6
0000000180378B2B movss     dword ptr [rbx + 0x3c], xmm6
0000000180378B30 jb        0x180378be8
0000000180378B36 mov       rax, qword ptr [rbx + 0x98]
0000000180378B3D mov       ecx, edi
0000000180378B3F test      rax, rax
0000000180378B42 je        0x180378cc5
0000000180378B48 movss     xmm6, dword ptr [rip + 0x122efb0] # 0x1815a7b00
0000000180378B50 cmp       ecx, dword ptr [rax + 0x18]
0000000180378B53 jge       0x180378be8
0000000180378B59 mov       rcx, qword ptr [rbx + 0x98]
0000000180378B60 test      rcx, rcx
0000000180378B63 je        0x180378cc5
0000000180378B69 mov       r8, qword ptr [rip + 0x1833518] # 0x181bac088
0000000180378B70 mov       edx, edi
0000000180378B72 call      0x180825710
0000000180378B77 test      rax, rax
0000000180378B7A je        0x180378cc5
0000000180378B80 mov       rdx, qword ptr [rip + 0x182c631] # 0x181ba51b8
0000000180378B87 mov       rcx, rax
0000000180378B8A call      0x18046dc80
0000000180378B8F movss     xmm0, dword ptr [rbx + 0x3c]
0000000180378B94 mulss     xmm0, xmm6
0000000180378B98 test      rax, rax
0000000180378B9B je        0x180378cc5
0000000180378BA1 xor       r8d, r8d
0000000180378BA4 movss     dword ptr [rsp + 0x3c], xmm0
0000000180378BAA lea       rdx, [rsp + 0x30]
0000000180378BAF mov       dword ptr [rsp + 0x30], 0x3f800000
0000000180378BB7 mov       rcx, rax
0000000180378BBA mov       dword ptr [rsp + 0x34], 0x3f800000
0000000180378BC2 mov       dword ptr [rsp + 0x38], 0x3f800000
0000000180378BCA call      0x1813290a0
0000000180378BCF mov       rax, qword ptr [rbx + 0x98]
0000000180378BD6 inc       edi
0000000180378BD8 mov       ecx, edi
0000000180378BDA test      rax, rax
0000000180378BDD je        0x180378cc5
0000000180378BE3 jmp       0x180378b50
0000000180378BE8 comiss    xmm9, dword ptr [rbx + 0x3c]
0000000180378BED jb        0x180378c85
0000000180378BF3 mov       rcx, qword ptr [rip + 0x1837906] # 0x181bb0500
0000000180378BFA mov       rdi, qword ptr [rbx + 0x48]
0000000180378BFE cmp       dword ptr [rcx + 0xe0], 0
0000000180378C05 jne       0x180378c0c
0000000180378C07 call      0x1802501e0
0000000180378C0C xor       edx, edx
0000000180378C0E mov       rcx, rdi
0000000180378C11 call      0x18131f870
0000000180378C16 test      al, al
0000000180378C18 je        0x180378c59
0000000180378C1A cmp       dword ptr [rbx + 0x20], 0
0000000180378C1E mov       rax, qword ptr [rbx + 0x48]
0000000180378C22 je        0x180378cb4
0000000180378C28 test      rax, rax
0000000180378C2B je        0x180378cc5
0000000180378C31 mov       rcx, qword ptr [rax + 0x100]
0000000180378C38 test      rcx, rcx
0000000180378C3B je        0x180378cc5
0000000180378C41 mov       rcx, qword ptr [rcx + 0x38]
0000000180378C45 test      rcx, rcx
0000000180378C48 je        0x180378cc5
0000000180378C4A mov       r8, qword ptr [rip + 0x183cc07] # 0x181bb5858
0000000180378C51 mov       rdx, rbx
0000000180378C54 call      0x18084e290
0000000180378C59 xor       edx, edx
0000000180378C5B mov       rcx, rbx
0000000180378C5E call      0x1813044d0
0000000180378C63 mov       rcx, qword ptr [rip + 0x1837896] # 0x181bb0500
0000000180378C6A mov       rbx, rax
0000000180378C6D cmp       dword ptr [rcx + 0xe0], 0
0000000180378C74 jne       0x180378c7b
0000000180378C76 call      0x1802501e0
0000000180378C7B xor       edx, edx
0000000180378C7D mov       rcx, rbx
0000000180378C80 call      0x18131e940
0000000180378C85 movaps    xmm7, xmmword ptr [rsp + 0x60]
0000000180378C8A mov       rsi, qword ptr [rsp + 0x90]
0000000180378C92 mov       rbx, qword ptr [rsp + 0x98]
0000000180378C9A movaps    xmm6, xmmword ptr [rsp + 0x70]
0000000180378C9F movaps    xmm8, xmmword ptr [rsp + 0x50]
0000000180378CA5 movaps    xmm9, xmmword ptr [rsp + 0x40]
0000000180378CAB add       rsp, 0x80
0000000180378CB2 pop       rdi
0000000180378CB3 ret       
0000000180378CB4 test      rax, rax
0000000180378CB7 je        0x180378cc5
0000000180378CB9 mov       rcx, qword ptr [rax + 0x120]
0000000180378CC0 jmp       0x180378c38
0000000180378CC5 call      0x180250150
0000000180378CCA int3      
0000000180378CCB int3      
0000000180378CCC int3      
0000000180378CCD int3      
0000000180378CCE int3      
0000000180378CCF int3      