0000000180348F10 mov       qword ptr [rsp + 0x10], rbx
0000000180348F15 mov       qword ptr [rsp + 0x18], rsi
0000000180348F1A push      rdi
0000000180348F1B sub       rsp, 0x30
0000000180348F1F cmp       byte ptr [rip + 0x199bdab], 0 # 0x181ce4cd1
0000000180348F26 mov       rdi, rdx
0000000180348F29 mov       rsi, rcx
0000000180348F2C jne       0x180348f41
0000000180348F2E lea       rcx, [rip + 0x18519f3] # 0x181b9a928
0000000180348F35 call      0x18024fef0
0000000180348F3A mov       byte ptr [rip + 0x199bd90], 1 # 0x181ce4cd1
0000000180348F41 mov       rcx, qword ptr [rip + 0x18519e0] # 0x181b9a928
0000000180348F48 mov       qword ptr [rsp + 0x40], rbp
0000000180348F4D movaps    xmmword ptr [rsp + 0x20], xmm6
0000000180348F52 call      0x180250100
0000000180348F57 xor       edx, edx
0000000180348F59 mov       rcx, rax
0000000180348F5C mov       rbx, rax
0000000180348F5F call      0x180311cd0
0000000180348F64 test      rbx, rbx
0000000180348F67 je        0x1803491e2
0000000180348F6D cmp       byte ptr [rip + 0x199bd60], 0 # 0x181ce4cd4
0000000180348F74 mov       ecx, dword ptr [rsi + 0x58]
0000000180348F77 mov       dword ptr [rbx + 0x14], ecx
0000000180348F7A jne       0x180348f8f
0000000180348F7C lea       rcx, [rip + 0x186757d] # 0x181bb0500
0000000180348F83 call      0x18024fef0
0000000180348F88 mov       byte ptr [rip + 0x199bd45], 1 # 0x181ce4cd4
0000000180348F8F mov       eax, dword ptr [rsi + 0x5c]
0000000180348F92 xor       ebp, ebp
0000000180348F94 cmp       eax, 2
0000000180348F97 jne       0x180348fa2
0000000180348F99 mov       dword ptr [rbx + 0x18], 4
0000000180348FA0 jmp       0x18034900a
0000000180348FA2 cmp       eax, 6
0000000180348FA5 je        0x180348ffb
0000000180348FA7 cmp       eax, 0xe
0000000180348FAA jne       0x18034900a
0000000180348FAC mov       rcx, qword ptr [rip + 0x186754d] # 0x181bb0500
0000000180348FB3 cmp       dword ptr [rcx + 0xe0], ebp
0000000180348FB9 jne       0x180348fc0
0000000180348FBB call      0x1802501e0
0000000180348FC0 xor       r8d, r8d
0000000180348FC3 xor       edx, edx
0000000180348FC5 mov       rcx, rdi
0000000180348FC8 call      0x18131f900
0000000180348FCD test      al, al
0000000180348FCF je        0x18034900a
0000000180348FD1 test      rdi, rdi
0000000180348FD4 je        0x1803491e2
0000000180348FDA cmp       dword ptr [rdi + 0x24], 0x1f
0000000180348FDE jne       0x180348ff2
0000000180348FE0 mov       dword ptr [rbx + 0x18], 3
0000000180348FE7 mov       byte ptr [rbx + 0x1f], 1
0000000180348FEB mov       byte ptr [rbx + 0x21], 1
0000000180348FEF mov       dword ptr [rbx + 0x14], ebp
0000000180348FF2 cmp       dword ptr [rdi + 0x24], 0x20
0000000180348FF6 jne       0x18034900a
0000000180348FF8 mov       dword ptr [rbx + 0x14], ebp
0000000180348FFB mov       byte ptr [rbx + 0x21], 1
0000000180348FFF mov       byte ptr [rbx + 0x1f], 1
0000000180349003 mov       dword ptr [rbx + 0x18], 3
000000018034900A cmp       byte ptr [rip + 0x199bcc2], bpl # 0x181ce4cd3
0000000180349011 jne       0x18034903e
0000000180349013 lea       rcx, [rip + 0x186ef36] # 0x181bb7f50
000000018034901A call      0x18024fef0
000000018034901F lea       rcx, [rip + 0x186efda] # 0x181bb8000
0000000180349026 call      0x18024fef0
000000018034902B lea       rcx, [rip + 0x18674ce] # 0x181bb0500
0000000180349032 call      0x18024fef0
0000000180349037 mov       byte ptr [rip + 0x199bc95], 1 # 0x181ce4cd3
000000018034903E mov       eax, dword ptr [rsi + 0x5c]
0000000180349041 cmp       eax, 2
0000000180349044 jne       0x180349065
0000000180349046 mov       rcx, qword ptr [rsi + 0x128]
000000018034904D test      rcx, rcx
0000000180349050 je        0x1803491e2
0000000180349056 xor       r8d, r8d
0000000180349059 xor       edx, edx
000000018034905B call      0x1803944e0
0000000180349060 jmp       0x180349119
0000000180349065 cmp       eax, 6
0000000180349068 jne       0x18034908b
000000018034906A xor       edx, edx
000000018034906C mov       rcx, rsi
000000018034906F call      0x181304510
0000000180349074 mov       r9, qword ptr [rip + 0x186eed5] # 0x181bb7f50
000000018034907B mov       rcx, rsi
000000018034907E movss     xmm2, dword ptr [rip + 0x125ec06] # 0x1815a7c8c
0000000180349086 jmp       0x180349111
000000018034908B cmp       eax, 0xe
000000018034908E jne       0x18034911c
0000000180349094 mov       rcx, qword ptr [rip + 0x1867465] # 0x181bb0500
000000018034909B cmp       dword ptr [rcx + 0xe0], ebp
00000001803490A1 jne       0x1803490a8
00000001803490A3 call      0x1802501e0
00000001803490A8 xor       r8d, r8d
00000001803490AB xor       edx, edx
00000001803490AD mov       rcx, rdi
00000001803490B0 call      0x18131f900
00000001803490B5 test      al, al
00000001803490B7 je        0x18034911c
00000001803490B9 test      rdi, rdi
00000001803490BC je        0x1803491e2
00000001803490C2 cmp       dword ptr [rdi + 0x24], 0x1f
00000001803490C6 jne       0x1803490ef
00000001803490C8 xor       edx, edx
00000001803490CA mov       rcx, rdi
00000001803490CD call      0x181304510
00000001803490D2 mov       r9, qword ptr [rip + 0x186ef27] # 0x181bb8000
00000001803490D9 mov       rdx, rax
00000001803490DC movss     xmm2, dword ptr [rip + 0x125ebb4] # 0x1815a7c98
00000001803490E4 mov       rcx, rdi
00000001803490E7 call      0x180426380
00000001803490EC mov       rbp, rax
00000001803490EF cmp       dword ptr [rdi + 0x24], 0x20
00000001803490F3 jne       0x18034911c
00000001803490F5 xor       edx, edx
00000001803490F7 mov       rcx, rdi
00000001803490FA call      0x181304510
00000001803490FF mov       r9, qword ptr [rip + 0x186eefa] # 0x181bb8000
0000000180349106 mov       rcx, rdi
0000000180349109 movss     xmm2, dword ptr [rip + 0x125eb7f] # 0x1815a7c90
0000000180349111 mov       rdx, rax
0000000180349114 call      0x180426380
0000000180349119 mov       rbp, rax
000000018034911C lea       rcx, [rbx + 0x28]
0000000180349120 mov       qword ptr [rbx + 0x28], rbp
0000000180349124 mov       rdx, rbp
0000000180349127 call      0x18024f360
000000018034912C cmp       byte ptr [rip + 0x199bb9f], 0 # 0x181ce4cd2
0000000180349133 jne       0x180349148
0000000180349135 lea       rcx, [rip + 0x18673c4] # 0x181bb0500
000000018034913C call      0x18024fef0
0000000180349141 mov       byte ptr [rip + 0x199bb8a], 1 # 0x181ce4cd2
0000000180349148 cmp       dword ptr [rsi + 0x5c], 0xe
000000018034914C movss     xmm6, dword ptr [rsi + 0x80]
0000000180349154 jne       0x18034919f
0000000180349156 mov       rcx, qword ptr [rip + 0x18673a3] # 0x181bb0500
000000018034915D cmp       dword ptr [rcx + 0xe0], 0
0000000180349164 jne       0x18034916b
0000000180349166 call      0x1802501e0
000000018034916B xor       r8d, r8d
000000018034916E xor       edx, edx
0000000180349170 mov       rcx, rdi
0000000180349173 call      0x18131f900
0000000180349178 test      al, al
000000018034917A je        0x18034919f
000000018034917C test      rdi, rdi
000000018034917F je        0x1803491e2
0000000180349181 cmp       dword ptr [rdi + 0x24], 0x1f
0000000180349185 jne       0x180349191
0000000180349187 movss     xmm6, dword ptr [rip + 0x125eb29] # 0x1815a7cb8
000000018034918F jmp       0x18034919f
0000000180349191 cmp       dword ptr [rdi + 0x24], 0x20
0000000180349195 jne       0x18034919f
0000000180349197 movss     xmm6, dword ptr [rip + 0x125eb11] # 0x1815a7cb0
000000018034919F lea       rcx, [rbx + 0x60]
00000001803491A3 movss     dword ptr [rbx + 0x10], xmm6
00000001803491A8 mov       rdx, rsi
00000001803491AB mov       qword ptr [rbx + 0x60], rsi
00000001803491AF call      0x18024f360
00000001803491B4 mov       rdx, qword ptr [rsi + 0x28]
00000001803491B8 lea       rcx, [rbx + 0x68]
00000001803491BC mov       qword ptr [rbx + 0x68], rdx
00000001803491C0 call      0x18024f360
00000001803491C5 movaps    xmm6, xmmword ptr [rsp + 0x20]
00000001803491CA mov       rax, rbx
00000001803491CD mov       rbx, qword ptr [rsp + 0x48]
00000001803491D2 mov       rbp, qword ptr [rsp + 0x40]
00000001803491D7 mov       rsi, qword ptr [rsp + 0x50]
00000001803491DC add       rsp, 0x30
00000001803491E0 pop       rdi
00000001803491E1 ret       
00000001803491E2 call      0x180250150
00000001803491E7 int3      
00000001803491E8 int3      
00000001803491E9 int3      
00000001803491EA int3      
00000001803491EB int3      
00000001803491EC int3      
00000001803491ED int3      
00000001803491EE int3      
00000001803491EF int3      