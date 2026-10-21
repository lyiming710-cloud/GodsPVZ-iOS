00000001803125E0 mov       rax, rsp
00000001803125E3 mov       qword ptr [rax + 0x10], rbx
00000001803125E7 mov       qword ptr [rax + 0x18], rsi
00000001803125EB mov       qword ptr [rax + 8], rcx
00000001803125EF push      rdi
00000001803125F0 sub       rsp, 0xe0
00000001803125F7 movaps    xmmword ptr [rax - 0x18], xmm6
00000001803125FB movaps    xmmword ptr [rax - 0x28], xmm7
00000001803125FF movaps    xmmword ptr [rax - 0x38], xmm8
0000000180312604 mov       rdi, rcx
0000000180312607 cmp       byte ptr [rip + 0x19d2585], 0 # 0x181ce4b93
000000018031260E jne       0x180312683
0000000180312610 lea       rcx, [rip + 0x18b0e49] # 0x181bc3460
0000000180312617 call      0x18024fef0
000000018031261C lea       rcx, [rip + 0x18b00dd] # 0x181bc2700
0000000180312623 call      0x18024fef0
0000000180312628 lea       rcx, [rip + 0x18b0129] # 0x181bc2758
000000018031262F call      0x18024fef0
0000000180312634 lea       rcx, [rip + 0x18b0e7d] # 0x181bc34b8
000000018031263B call      0x18024fef0
0000000180312640 lea       rcx, [rip + 0x18b0ec9] # 0x181bc3510
0000000180312647 call      0x18024fef0
000000018031264C lea       rcx, [rip + 0x18b015d] # 0x181bc27b0
0000000180312653 call      0x18024fef0
0000000180312658 lea       rcx, [rip + 0x1892b59] # 0x181ba51b8
000000018031265F call      0x18024fef0
0000000180312664 lea       rcx, [rip + 0x1896a55] # 0x181ba90c0
000000018031266B call      0x18024fef0
0000000180312670 lea       rcx, [rip + 0x1899801] # 0x181babe78
0000000180312677 call      0x18024fef0
000000018031267C mov       byte ptr [rip + 0x19d2510], 1 # 0x181ce4b93
0000000180312683 xorps     xmm0, xmm0
0000000180312686 xor       eax, eax
0000000180312688 movups    xmmword ptr [rsp + 0x70], xmm0
000000018031268D mov       qword ptr [rsp + 0x80], rax
0000000180312695 mov       rdx, qword ptr [rdi + 0x30]
0000000180312699 test      rdx, rdx
000000018031269C je        0x1803128ba
00000001803126A2 mov       r8, qword ptr [rip + 0x1896a17] # 0x181ba90c0
00000001803126A9 lea       rcx, [rsp + 0x30]
00000001803126AE call      0x1808197f0
00000001803126B3 movups    xmm0, xmmword ptr [rsp + 0x30]
00000001803126B8 movups    xmmword ptr [rsp + 0x88], xmm0
00000001803126C0 movsd     xmm1, qword ptr [rsp + 0x40]
00000001803126C6 movsd     qword ptr [rsp + 0x98], xmm1
00000001803126CF xor       esi, esi
00000001803126D1 mov       qword ptr [rsp + 0x20], rsi
00000001803126D6 lea       rax, [rsp + 0x88]
00000001803126DE mov       qword ptr [rsp + 0x28], rax
00000001803126E3 movss     xmm8, dword ptr [rip + 0x1295324] # 0x1815a7a10
00000001803126EC movss     xmm7, dword ptr [rip + 0x1295314] # 0x1815a7a08
00000001803126F4 nop       dword ptr [rax]
00000001803126F8 nop       dword ptr [rax + rax]
0000000180312700 mov       rdx, qword ptr [rip + 0x18b0051] # 0x181bc2758
0000000180312707 lea       rcx, [rsp + 0x88]
000000018031270F call      0x180669280
0000000180312714 test      al, al
0000000180312716 je        0x180312846
000000018031271C mov       rax, qword ptr [rsp + 0x98]
0000000180312724 test      rax, rax
0000000180312727 je        0x1803128ae
000000018031272D cmp       dword ptr [rax + 0x5c], 0xc
0000000180312731 jne       0x180312700
0000000180312733 mov       rdx, qword ptr [rax + 0xd8]
000000018031273A test      rdx, rdx
000000018031273D je        0x1803128a9
0000000180312743 mov       r8, qword ptr [rip + 0x189972e] # 0x181babe78
000000018031274A lea       rcx, [rsp + 0x30]
000000018031274F call      0x1808197f0
0000000180312754 movups    xmm0, xmmword ptr [rsp + 0x30]
0000000180312759 movups    xmmword ptr [rsp + 0x70], xmm0
000000018031275E movsd     xmm1, qword ptr [rsp + 0x40]
0000000180312764 movsd     qword ptr [rsp + 0x80], xmm1
000000018031276D mov       qword ptr [rsp + 0x60], rsi
0000000180312772 lea       rbx, [rsp + 0x70]
0000000180312777 mov       qword ptr [rsp + 0x68], rbx
000000018031277C nop       dword ptr [rax]
0000000180312780 mov       rdx, qword ptr [rip + 0x18b0d31] # 0x181bc34b8
0000000180312787 lea       rcx, [rsp + 0x70]
000000018031278C call      0x180669280
0000000180312791 test      al, al
0000000180312793 je        0x180312808
0000000180312795 cmp       byte ptr [rdi + 0x40], 0
0000000180312799 jne       0x1803127a0
000000018031279B movaps    xmm6, xmm7
000000018031279E jmp       0x1803127a4
00000001803127A0 movaps    xmm6, xmm8
00000001803127A4 mov       rcx, qword ptr [rsp + 0x80]
00000001803127AC test      rcx, rcx
00000001803127AF je        0x18031289e
00000001803127B5 mov       rdx, qword ptr [rip + 0x18929fc] # 0x181ba51b8
00000001803127BC call      0x18046dc80
00000001803127C1 mov       dword ptr [rsp + 0x50], 0x3f800000
00000001803127C9 mov       dword ptr [rsp + 0x54], 0x3f800000
00000001803127D1 mov       dword ptr [rsp + 0x58], 0x3f800000
00000001803127D9 movss     dword ptr [rsp + 0x5c], xmm6
00000001803127DF test      rax, rax
00000001803127E2 je        0x180312898
00000001803127E8 movups    xmm0, xmmword ptr [rsp + 0x50]
00000001803127ED movdqa    xmmword ptr [rsp + 0x30], xmm0
00000001803127F3 xor       r8d, r8d
00000001803127F6 lea       rdx, [rsp + 0x30]
00000001803127FB mov       rcx, rax
00000001803127FE call      0x1813290a0
0000000180312803 jmp       0x180312780
0000000180312808 mov       rdx, qword ptr [rip + 0x18b0c51] # 0x181bc3460
000000018031280F mov       rcx, rbx
0000000180312812 call      0x180302170
0000000180312817 jmp       0x180312700
000000018031281C mov       rdx, qword ptr [rip + 0x18b0c3d] # 0x181bc3460
0000000180312823 mov       rcx, qword ptr [rsp + 0x68]
0000000180312828 call      0x180302170
000000018031282D mov       rcx, qword ptr [rsp + 0x60]
0000000180312832 test      rcx, rcx
0000000180312835 jne       0x1803128a4
0000000180312837 xor       esi, esi
0000000180312839 mov       rdi, qword ptr [rsp + 0xf0]
0000000180312841 jmp       0x1803126e3
0000000180312846 mov       rdx, qword ptr [rip + 0x18afeb3] # 0x181bc2700
000000018031284D mov       rcx, qword ptr [rsp + 0x28]
0000000180312852 call      0x180302170
0000000180312857 jmp       0x180312874
0000000180312859 mov       rdx, qword ptr [rip + 0x18afea0] # 0x181bc2700
0000000180312860 mov       rcx, qword ptr [rsp + 0x28]
0000000180312865 call      0x180302170
000000018031286A mov       rcx, qword ptr [rsp + 0x20]
000000018031286F test      rcx, rcx
0000000180312872 jne       0x1803128b4
0000000180312874 lea       r11, [rsp + 0xe0]
000000018031287C mov       rbx, qword ptr [r11 + 0x18]
0000000180312880 mov       rsi, qword ptr [r11 + 0x20]
0000000180312884 movaps    xmm6, xmmword ptr [r11 - 0x10]
0000000180312889 movaps    xmm7, xmmword ptr [r11 - 0x20]
000000018031288E movaps    xmm8, xmmword ptr [r11 - 0x30]
0000000180312893 mov       rsp, r11
0000000180312896 pop       rdi
0000000180312897 ret       
0000000180312898 call      0x180250150
000000018031289D nop       
000000018031289E call      0x180250150
00000001803128A3 nop       
00000001803128A4 call      0x180200e60
00000001803128A9 call      0x180250150
00000001803128AE call      0x180250150
00000001803128B3 nop       
00000001803128B4 call      0x180200e60
00000001803128B9 int3      
00000001803128BA call      0x180250150
00000001803128BF int3      