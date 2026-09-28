1804c4920 mov qword ptr [rsp + 0x10], rdx
1804c4925 push rbp
1804c4926 push rdi
1804c4927 push r12
1804c4929 push r14
1804c492b push r15
1804c492d sub rsp, 0x30
1804c4931 lea rbp, [rsp + 0x20]
1804c4936 mov qword ptr [rbp + 0x40], rbx
1804c493a movzx r15d, r8b
1804c493e mov rbx, qword ptr [rbp + 0x68]
1804c4942 mov rdi, rcx
1804c4945 mov qword ptr [rbp + 0x50], rsi
1804c4949 mov rsi, rdx
1804c494c movaps xmmword ptr [rbp], xmm6
1804c4950 movaps xmm6, xmm3
1804c4953 cmp qword ptr [rbx + 0x38], 0
1804c4958 jne 0x1804c4981
1804c495a lea rcx, [rip + 0x16e740f]
1804c4961 call 0x18024fef0
1804c4966 lea rcx, [rip + 0x16ed65b]
1804c496d call 0x18024fef0
1804c4972 cmp qword ptr [rbx + 0x38], 0
1804c4977 jne 0x1804c4981
1804c4979 mov rcx, rbx
1804c497c call 0x180258b00
1804c4981 mov rax, qword ptr [rbx + 0x38]
1804c4985 mov rcx, qword ptr [rax]
1804c4988 mov r14d, dword ptr [rcx + 0xfc]
1804c498f lea rcx, [r14 + 0xf]
1804c4993 cmp rcx, r14
1804c4996 ja 0x1804c49a2
1804c4998 movabs rcx, 0xffffffffffffff0
1804c49a2 and rcx, 0xfffffffffffffff0
1804c49a6 mov rax, rcx
1804c49a9 call 0x1802c5380
1804c49ae mov rdx, qword ptr [rbp + 0x60]
1804c49b2 sub rsp, rcx
1804c49b5 lea rcx, [rdi + 0x48]
1804c49b9 mov qword ptr [rdi + 0x48], rdx
1804c49bd lea r12, [rsp + 0x20]
1804c49c2 call 0x18024f360
1804c49c7 movss dword ptr [rdi + 0x44], xmm6
1804c49cc test r15b, r15b
1804c49cf je 0x1804c4a85
1804c49d5 mov rax, qword ptr [rbx + 0x38]
1804c49d9 lea rdx, [rbp + 0x48]
1804c49dd mov r8, r14
1804c49e0 mov rcx, qword ptr [rax]
1804c49e3 mov eax, dword ptr [rcx + 0x28]
1804c49e6 mov rcx, r12
1804c49e9 shr eax, 0x1f
1804c49ec test al, al
1804c49ee cmovne rdx, rsi
1804c49f2 call 0x1802f64f0
1804c49f7 mov rax, qword ptr [rbx + 0x38]
1804c49fb mov rdx, r12
1804c49fe mov rcx, qword ptr [rax]
1804c4a01 call 0x18024f340
1804c4a06 mov r10, rax
1804c4a09 test rax, rax
1804c4a0c je 0x1804c4a85
1804c4a0e mov r8, qword ptr [rip + 0x16ed5b3]
1804c4a15 mov r9, qword ptr [rax]
1804c4a18 movzx ecx, byte ptr [r8 + 0x130]
1804c4a20 cmp byte ptr [r9 + 0x130], cl
1804c4a27 jb 0x1804c4a3e
1804c4a29 movzx edx, cl
1804c4a2c mov rcx, qword ptr [r9 + 0xc8]
1804c4a33 cmp qword ptr [rcx + rdx*8 - 8], r8
1804c4a38 jne 0x1804c4a3e
1804c4a3a mov al, 1
1804c4a3c jmp 0x1804c4a40
1804c4a3e xor al, al
1804c4a40 xor ebx, ebx
1804c4a42 test al, al
1804c4a44 cmovne rbx, r10
1804c4a48 test rbx, rbx
1804c4a4b je 0x1804c4a85
1804c4a4d lea rcx, [rdi + 0x28]
1804c4a51 mov qword ptr [rdi + 0x28], rbx
1804c4a55 mov rdx, rbx
1804c4a58 call 0x18024f360
1804c4a5d mov rbx, qword ptr [rbx + 0x1d0]
1804c4a64 xor edx, edx
1804c4a66 mov rcx, rdi
1804c4a69 call 0x1813044d0
1804c4a6e test rbx, rbx
1804c4a71 je 0x1804c4a9e
1804c4a73 mov r8, qword ptr [rip + 0x16e72f6]
1804c4a7a mov rdx, rax
1804c4a7d mov rcx, rbx
1804c4a80 call 0x180002e70
1804c4a85 mov rbx, qword ptr [rbp + 0x40]
1804c4a89 mov rsi, qword ptr [rbp + 0x50]
1804c4a8d movaps xmm6, xmmword ptr [rbp]
1804c4a91 lea rsp, [rbp + 0x10]
1804c4a95 pop r15
1804c4a97 pop r14
1804c4a99 pop r12
1804c4a9b pop rdi
1804c4a9c pop rbp
1804c4a9d ret
1804c4a9e call 0x180250150
1804c4aa3 int3
