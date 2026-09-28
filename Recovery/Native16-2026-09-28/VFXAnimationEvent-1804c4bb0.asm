1804c4bb0 mov qword ptr [rsp + 0x18], r8
1804c4bb5 push rbp
1804c4bb6 push r12
1804c4bb8 push r13
1804c4bba push r14
1804c4bbc push r15
1804c4bbe sub rsp, 0x50
1804c4bc2 lea rbp, [rsp + 0x30]
1804c4bc7 cmp qword ptr [r9 + 0x38], 0
1804c4bcc mov r14, r8
1804c4bcf mov qword ptr [rbp + 0x50], rbx
1804c4bd3 mov r13d, edx
1804c4bd6 mov qword ptr [rbp + 0x58], rsi
1804c4bda mov rbx, r9
1804c4bdd mov qword ptr [rbp + 0x68], rdi
1804c4be1 mov rdi, rcx
1804c4be4 movaps xmmword ptr [rbp + 0x10], xmm6
1804c4be8 jne 0x1804c4c59
1804c4bea lea rcx, [rip + 0x16e05c7]
1804c4bf1 call 0x18024fef0
1804c4bf6 lea rcx, [rip + 0x16dd41b]
1804c4bfd call 0x18024fef0
1804c4c02 lea rcx, [rip + 0x16e70b7]
1804c4c09 call 0x18024fef0
1804c4c0e lea rcx, [rip + 0x16f8f83]
1804c4c15 call 0x18024fef0
1804c4c1a lea rcx, [rip + 0x16ed3a7]
1804c4c21 call 0x18024fef0
1804c4c26 lea rcx, [rip + 0x16d6cb3]
1804c4c2d call 0x18024fef0
1804c4c32 lea rcx, [rip + 0x16fc167]
1804c4c39 call 0x18024fef0
1804c4c3e lea rcx, [rip + 0x1702dbb]
1804c4c45 call 0x18024fef0
1804c4c4a cmp qword ptr [rbx + 0x38], 0
1804c4c4f jne 0x1804c4c59
1804c4c51 mov rcx, rbx
1804c4c54 call 0x180258b00
1804c4c59 mov rax, qword ptr [rbx + 0x38]
1804c4c5d mov rcx, qword ptr [rax]
1804c4c60 mov r15d, dword ptr [rcx + 0xfc]
1804c4c67 lea rcx, [r15 + 0xf]
1804c4c6b cmp rcx, r15
1804c4c6e ja 0x1804c4c7a
1804c4c70 movabs rcx, 0xffffffffffffff0
1804c4c7a and rcx, 0xfffffffffffffff0
1804c4c7e mov rax, rcx
1804c4c81 call 0x1802c5380
1804c4c86 sub rsp, rcx
1804c4c89 mov rcx, qword ptr [rip + 0x16f8f08]
1804c4c90 lea r12, [rsp + 0x30]
1804c4c95 call 0x180250100
1804c4c9a mov rdx, qword ptr [rip + 0x16e701f]
1804c4ca1 mov rcx, rax
1804c4ca4 mov rsi, rax
1804c4ca7 call 0x1808053d0
1804c4cac test rdi, rdi
1804c4caf je 0x1804c4f6a
1804c4cb5 xor edx, edx
1804c4cb7 mov rcx, rdi
1804c4cba call 0x181304510
1804c4cbf xor r9d, r9d
1804c4cc2 mov r8, rsi
1804c4cc5 mov rdx, rax
1804c4cc8 mov rcx, rdi
1804c4ccb call 0x18030da70
1804c4cd0 cmp r13d, 0x1e
1804c4cd4 jne 0x1804c4f4c
1804c4cda mov rax, qword ptr [rbx + 0x38]
1804c4cde lea rdx, [rbp + 0x60]
1804c4ce2 mov r8, r15
1804c4ce5 mov rcx, qword ptr [rax]
1804c4ce8 mov eax, dword ptr [rcx + 0x28]
1804c4ceb mov rcx, r12
1804c4cee shr eax, 0x1f
1804c4cf1 test al, al
1804c4cf3 cmovne rdx, r14
1804c4cf7 call 0x1802f64f0
1804c4cfc mov rax, qword ptr [rbx + 0x38]
1804c4d00 mov rdx, r12
1804c4d03 mov rcx, qword ptr [rax]
1804c4d06 call 0x18024f340
1804c4d0b test rax, rax
1804c4d0e je 0x1804c4f4c
1804c4d14 mov r8, qword ptr [rax]
1804c4d17 mov rdx, qword ptr [rip + 0x16ed2aa]
1804c4d1e movzx eax, byte ptr [rdx + 0x130]
1804c4d25 cmp byte ptr [r8 + 0x130], al
1804c4d2c jb 0x1804c4f4c
1804c4d32 movzx ecx, al
1804c4d35 mov rax, qword ptr [r8 + 0xc8]
1804c4d3c cmp qword ptr [rax + rcx*8 - 8], rdx
1804c4d41 jne 0x1804c4f4c
1804c4d47 mov rax, qword ptr [rbx + 0x38]
1804c4d4b lea rdx, [rbp + 0x60]
1804c4d4f mov r8, r15
1804c4d52 mov rcx, qword ptr [rax]
1804c4d55 mov eax, dword ptr [rcx + 0x28]
1804c4d58 mov rcx, r12
1804c4d5b shr eax, 0x1f
1804c4d5e test al, al
1804c4d60 cmovne rdx, r14
1804c4d64 call 0x1802f64f0
1804c4d69 mov rax, qword ptr [rbx + 0x38]
1804c4d6d mov rdx, r12
1804c4d70 mov rcx, qword ptr [rax]
1804c4d73 call 0x18024f340
1804c4d78 mov r9, rax
1804c4d7b test rax, rax
1804c4d7e jne 0x1804c4d84
1804c4d80 xor ebx, ebx
1804c4d82 jmp 0x1804c4dbd
1804c4d84 mov r8, qword ptr [rax]
1804c4d87 mov rdx, qword ptr [rip + 0x16ed23a]
1804c4d8e movzx eax, byte ptr [rdx + 0x130]
1804c4d95 cmp byte ptr [r8 + 0x130], al
1804c4d9c jb 0x1804c4db3
1804c4d9e movzx ecx, al
1804c4da1 mov rax, qword ptr [r8 + 0xc8]
1804c4da8 cmp qword ptr [rax + rcx*8 - 8], rdx
1804c4dad jne 0x1804c4db3
1804c4daf mov al, 1
1804c4db1 jmp 0x1804c4db5
1804c4db3 xor al, al
1804c4db5 xor ebx, ebx
1804c4db7 test al, al
1804c4db9 cmovne rbx, r9
1804c4dbd mov rcx, qword ptr [rip + 0x16dd254]
1804c4dc4 cmp dword ptr [rcx + 0xe0], 0
1804c4dcb jne 0x1804c4dd2
1804c4dcd call 0x1802501e0
1804c4dd2 mov rdx, qword ptr [rip + 0x16d6b07]
1804c4dd9 xor r8d, r8d
1804c4ddc mov rcx, rsi
1804c4ddf call 0x18031b790
1804c4de4 mov rdi, rax
1804c4de7 test rbx, rbx
1804c4dea je 0x1804c4f6a
1804c4df0 mov rcx, qword ptr [rbx + 0x1e8]
1804c4df7 test rcx, rcx
1804c4dfa je 0x1804c4f6a
1804c4e00 mov rdx, qword ptr [rcx + 0x28]
1804c4e04 test rdx, rdx
1804c4e07 je 0x1804c4f6a
1804c4e0d mov r9d, dword ptr [rbx + 0x24]
1804c4e11 lea rcx, [rbp]
1804c4e15 mov r8d, dword ptr [rbx + 0x20]
1804c4e19 inc r9d
1804c4e1c mov qword ptr [rsp + 0x20], 0
1804c4e25 call 0x18030ec90
1804c4e2a movss xmm6, dword ptr [rax + 4]
1804c4e2f test rdi, rdi
1804c4e32 je 0x1804c4f6a
1804c4e38 mov rdx, qword ptr [rip + 0x16e0379]
1804c4e3f mov rcx, rdi
1804c4e42 call 0x18046dc80
1804c4e47 test rax, rax
1804c4e4a je 0x1804c4f6a
1804c4e50 mov rdx, qword ptr [rip + 0x16fbf49]
1804c4e57 xor r8d, r8d
1804c4e5a mov rcx, rax
1804c4e5d call 0x1812fb400
1804c4e62 mov rdx, qword ptr [rip + 0x16e034f]
1804c4e69 mov rcx, rdi
1804c4e6c call 0x18046dc80
1804c4e71 test rax, rax
1804c4e74 je 0x1804c4f6a
1804c4e7a addss xmm6, dword ptr [rip + 0x10e2b8e]
1804c4e82 xor r8d, r8d
1804c4e85 mov rcx, rax
1804c4e88 mulss xmm6, dword ptr [rip + 0x10e2e3c]
1804c4e90 cvttss2si edx, xmm6
1804c4e94 call 0x1812fb450
1804c4e99 mov rdx, qword ptr [rip + 0x1702b60]
1804c4ea0 xor r8d, r8d
1804c4ea3 mov rcx, rsi
1804c4ea6 call 0x18031b790
1804c4eab mov rdx, qword ptr [rbx + 0x1e8]
1804c4eb2 mov rdi, rax
1804c4eb5 test rdx, rdx
1804c4eb8 je 0x1804c4f6a
1804c4ebe mov rdx, qword ptr [rdx + 0x28]
1804c4ec2 test rdx, rdx
1804c4ec5 je 0x1804c4f6a
1804c4ecb mov r9d, dword ptr [rbx + 0x24]
1804c4ecf lea rcx, [rbp]
1804c4ed3 mov r8d, dword ptr [rbx + 0x20]
1804c4ed7 sub r9d, 2
1804c4edb mov qword ptr [rsp + 0x20], 0
1804c4ee4 call 0x18030ec90
1804c4ee9 movss xmm6, dword ptr [rax + 4]
1804c4eee test rdi, rdi
1804c4ef1 je 0x1804c4f6a
1804c4ef3 mov rdx, qword ptr [rip + 0x16e02be]
1804c4efa mov rcx, rdi
1804c4efd call 0x18046dc80
1804c4f02 test rax, rax
1804c4f05 je 0x1804c4f6a
1804c4f07 mov rdx, qword ptr [rip + 0x16fbe92]
1804c4f0e xor r8d, r8d
1804c4f11 mov rcx, rax
1804c4f14 call 0x1812fb400
1804c4f19 mov rdx, qword ptr [rip + 0x16e0298]
1804c4f20 mov rcx, rdi
1804c4f23 call 0x18046dc80
1804c4f28 test rax, rax
1804c4f2b je 0x1804c4f6a
1804c4f2d subss xmm6, dword ptr [rip + 0x10e2adb]
1804c4f35 xor r8d, r8d
1804c4f38 mov rcx, rax
1804c4f3b mulss xmm6, dword ptr [rip + 0x10e2d89]
1804c4f43 cvttss2si edx, xmm6
1804c4f47 call 0x1812fb450
1804c4f4c mov rbx, qword ptr [rbp + 0x50]
1804c4f50 mov rsi, qword ptr [rbp + 0x58]
1804c4f54 mov rdi, qword ptr [rbp + 0x68]
1804c4f58 movaps xmm6, xmmword ptr [rbp + 0x10]
1804c4f5c lea rsp, [rbp + 0x20]
1804c4f60 pop r15
1804c4f62 pop r14
1804c4f64 pop r13
1804c4f66 pop r12
1804c4f68 pop rbp
1804c4f69 ret
1804c4f6a call 0x180250150
1804c4f6f int3
