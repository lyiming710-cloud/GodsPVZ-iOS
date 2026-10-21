0000000180386D30 push      rbx
0000000180386D32 push      rsi
0000000180386D33 push      r14
0000000180386D35 sub       rsp, 0x30
0000000180386D39 cmp       byte ptr [rip + 0x195e0b2], 0 # 0x181ce4df2
0000000180386D40 movzx     r14d, dl
0000000180386D44 mov       rsi, rcx
0000000180386D47 jne       0x180386d80
0000000180386D49 lea       rcx, [rip + 0x1825ea8] # 0x181bacbf8
0000000180386D50 call      0x18024fef0
0000000180386D55 lea       rcx, [rip + 0x18314d4] # 0x181bb8230
0000000180386D5C call      0x18024fef0
0000000180386D61 lea       rcx, [rip + 0x1829798] # 0x181bb0500
0000000180386D68 call      0x18024fef0
0000000180386D6D lea       rcx, [rip + 0x182f704] # 0x181bb6478
0000000180386D74 call      0x18024fef0
0000000180386D79 mov       byte ptr [rip + 0x195e072], 1 # 0x181ce4df2
0000000180386D80 mov       rax, qword ptr [rip + 0x182f6f1] # 0x181bb6478
0000000180386D87 cmp       dword ptr [rax + 0xe0], 0
0000000180386D8E jne       0x180386d9f
0000000180386D90 mov       rcx, rax
0000000180386D93 call      0x1802501e0
0000000180386D98 mov       rax, qword ptr [rip + 0x182f6d9] # 0x181bb6478
0000000180386D9F mov       rcx, qword ptr [rip + 0x182975a] # 0x181bb0500
0000000180386DA6 mov       rax, qword ptr [rax + 0xb8]
0000000180386DAD cmp       dword ptr [rcx + 0xe0], 0
0000000180386DB4 mov       rbx, qword ptr [rax + 0x190]
0000000180386DBB jne       0x180386dc2
0000000180386DBD call      0x1802501e0
0000000180386DC2 xor       r8d, r8d
0000000180386DC5 xor       edx, edx
0000000180386DC7 mov       rcx, rbx
0000000180386DCA call      0x18131f760
0000000180386DCF test      al, al
0000000180386DD1 jne       0x180386ff7
0000000180386DD7 mov       rax, qword ptr [rip + 0x182f69a] # 0x181bb6478
0000000180386DDE cmp       dword ptr [rax + 0xe0], 0
0000000180386DE5 jne       0x180386df6
0000000180386DE7 mov       rcx, rax
0000000180386DEA call      0x1802501e0
0000000180386DEF mov       rax, qword ptr [rip + 0x182f682] # 0x181bb6478
0000000180386DF6 mov       rcx, qword ptr [rip + 0x1829703] # 0x181bb0500
0000000180386DFD mov       rax, qword ptr [rax + 0xb8]
0000000180386E04 cmp       dword ptr [rcx + 0xe0], 0
0000000180386E0B mov       rbx, qword ptr [rax + 0x190]
0000000180386E12 jne       0x180386e19
0000000180386E14 call      0x1802501e0
0000000180386E19 mov       rdx, qword ptr [rip + 0x1831410] # 0x181bb8230
0000000180386E20 mov       rcx, rbx
0000000180386E23 mov       qword ptr [rsp + 0x50], rbp
0000000180386E28 mov       qword ptr [rsp + 0x58], rdi
0000000180386E2D call      0x1804a0690
0000000180386E32 mov       rbx, rax
0000000180386E35 test      rax, rax
0000000180386E38 je        0x180387002
0000000180386E3E xor       r8d, r8d
0000000180386E41 mov       rdx, rsi
0000000180386E44 mov       rcx, rax
0000000180386E47 call      0x180386a40
0000000180386E4C mov       rcx, qword ptr [rbx + 0x20]
0000000180386E50 test      rcx, rcx
0000000180386E53 je        0x180387002
0000000180386E59 mov       ecx, dword ptr [rcx + 0x20]
0000000180386E5C xor       edx, edx
0000000180386E5E mov       dword ptr [rsp + 0x68], ecx
0000000180386E62 lea       rcx, [rsp + 0x68]
0000000180386E67 mov       rdi, qword ptr [rbx + 0x50]
0000000180386E6B call      0x180ca3820
0000000180386E70 test      rdi, rdi
0000000180386E73 je        0x180387002
0000000180386E79 mov       r9, qword ptr [rdi]
0000000180386E7C mov       rdx, rax
0000000180386E7F mov       rcx, rdi
0000000180386E82 mov       r8, qword ptr [r9 + 0x560]
0000000180386E89 call      qword ptr [r9 + 0x558]
0000000180386E90 mov       rcx, qword ptr [rbx + 0x48]
0000000180386E94 test      rcx, rcx
0000000180386E97 je        0x180387002
0000000180386E9D xor       edx, edx
0000000180386E9F call      0x1813cfaa0
0000000180386EA4 mov       rcx, qword ptr [rip + 0x1825d4d] # 0x181bacbf8
0000000180386EAB mov       rdi, rax
0000000180386EAE mov       ebp, dword ptr [rsp + 0x68]
0000000180386EB2 cmp       dword ptr [rcx + 0xe0], 0
0000000180386EB9 jne       0x180386ec0
0000000180386EBB call      0x1802501e0
0000000180386EC0 test      rdi, rdi
0000000180386EC3 je        0x180387002
0000000180386EC9 movd      xmm0, ebp
0000000180386ECD cvtdq2pd  xmm0, xmm0
0000000180386ED1 call      0x1802cdc30
0000000180386ED6 cvttsd2si eax, xmm0
0000000180386EDA xor       r9d, r9d
0000000180386EDD xor       edx, edx
0000000180386EDF imul      ecx, eax, 0xe
0000000180386EE2 add       ecx, 0x27
0000000180386EE5 movd      xmm2, ecx
0000000180386EE9 mov       rcx, rdi
0000000180386EEC cvtdq2ps  xmm2, xmm2
0000000180386EEF call      0x181324090
0000000180386EF4 cmp       dword ptr [rsp + 0x68], 0
0000000180386EF9 jne       0x180386f8f
0000000180386EFF mov       rcx, qword ptr [rbx + 0x30]
0000000180386F03 movdqa    xmm0, xmmword ptr [rip + 0x1220f75] # 0x1815a7e80
0000000180386F0B test      rcx, rcx
0000000180386F0E je        0x180387002
0000000180386F14 mov       rax, qword ptr [rcx]
0000000180386F17 lea       rdx, [rsp + 0x20]
0000000180386F1C movdqa    xmmword ptr [rsp + 0x20], xmm0
0000000180386F22 mov       r8, qword ptr [rax + 0x2b0]
0000000180386F29 call      qword ptr [rax + 0x2a8]
0000000180386F2F mov       rcx, qword ptr [rbx + 0x38]
0000000180386F33 movdqa    xmm0, xmmword ptr [rip + 0x1220f45] # 0x1815a7e80
0000000180386F3B test      rcx, rcx
0000000180386F3E je        0x180387002
0000000180386F44 mov       rax, qword ptr [rcx]
0000000180386F47 lea       rdx, [rsp + 0x20]
0000000180386F4C movdqa    xmmword ptr [rsp + 0x20], xmm0
0000000180386F52 mov       r8, qword ptr [rax + 0x2b0]
0000000180386F59 call      qword ptr [rax + 0x2a8]
0000000180386F5F mov       rcx, qword ptr [rbx + 0x40]
0000000180386F63 movdqa    xmm0, xmmword ptr [rip + 0x1220f15] # 0x1815a7e80
0000000180386F6B test      rcx, rcx
0000000180386F6E je        0x180387002
0000000180386F74 mov       rax, qword ptr [rcx]
0000000180386F77 lea       rdx, [rsp + 0x20]
0000000180386F7C movdqa    xmmword ptr [rsp + 0x20], xmm0
0000000180386F82 mov       r8, qword ptr [rax + 0x2b0]
0000000180386F89 call      qword ptr [rax + 0x2a8]
0000000180386F8F mov       rcx, qword ptr [rbx + 0x48]
0000000180386F93 test      rcx, rcx
0000000180386F96 je        0x180387002
0000000180386F98 xor       edx, edx
0000000180386F9A call      0x1813044d0
0000000180386F9F test      rax, rax
0000000180386FA2 je        0x180387002
0000000180386FA4 xor       r8d, r8d
0000000180386FA7 movzx     edx, r14b
0000000180386FAB mov       rcx, rax
0000000180386FAE call      0x18131bad0
0000000180386FB3 cmp       dword ptr [rsi + 0x18], 0x32
0000000180386FB7 je        0x180386fbf
0000000180386FB9 cmp       dword ptr [rsi + 0x18], 0x33
0000000180386FBD jne       0x180386fe1
0000000180386FBF mov       rcx, qword ptr [rbx + 0x48]
0000000180386FC3 test      rcx, rcx
0000000180386FC6 je        0x180387002
0000000180386FC8 xor       edx, edx
0000000180386FCA call      0x1813044d0
0000000180386FCF test      rax, rax
0000000180386FD2 je        0x180387002
0000000180386FD4 xor       r8d, r8d
0000000180386FD7 xor       edx, edx
0000000180386FD9 mov       rcx, rax
0000000180386FDC call      0x18131bad0
0000000180386FE1 mov       rbp, qword ptr [rsp + 0x50]
0000000180386FE6 mov       rax, rbx
0000000180386FE9 mov       rdi, qword ptr [rsp + 0x58]
0000000180386FEE add       rsp, 0x30
0000000180386FF2 pop       r14
0000000180386FF4 pop       rsi
0000000180386FF5 pop       rbx
0000000180386FF6 ret       
0000000180386FF7 xor       eax, eax
0000000180386FF9 add       rsp, 0x30
0000000180386FFD pop       r14
0000000180386FFF pop       rsi
0000000180387000 pop       rbx
0000000180387001 ret       
0000000180387002 call      0x180250150
0000000180387007 int3      
0000000180387008 int3      
0000000180387009 int3      
000000018038700A int3      
000000018038700B int3      
000000018038700C int3      
000000018038700D int3      
000000018038700E int3      
000000018038700F int3      