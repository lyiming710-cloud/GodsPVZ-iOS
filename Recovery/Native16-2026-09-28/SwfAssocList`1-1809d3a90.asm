1809d3a90 mov qword ptr [rsp + 0x10], rdx
1809d3a95 mov qword ptr [rsp + 8], rcx
1809d3a9a push rbp
1809d3a9b push rbx
1809d3a9c push rsi
1809d3a9d push rdi
1809d3a9e push r12
1809d3aa0 push r13
1809d3aa2 push r14
1809d3aa4 push r15
1809d3aa6 sub rsp, 0x68
1809d3aaa lea rbp, [rsp + 0x30]
1809d3aaf mov rax, qword ptr [r8 + 0x20]
1809d3ab3 mov rsi, r8
1809d3ab6 mov r15, rdx
1809d3ab9 movabs rcx, 0xffffffffffffff0
1809d3ac3 mov r9, qword ptr [rax + 0xc0]
1809d3aca mov rax, qword ptr [r9 + 0x60]
1809d3ace mov eax, dword ptr [rax + 0xfc]
1809d3ad4 mov ebx, eax
1809d3ad6 mov dword ptr [rbp + 0x98], eax
1809d3adc add rax, 0xf
1809d3ae0 cmp rax, rbx
1809d3ae3 ja 0x1809d3ae8
1809d3ae5 mov rax, rcx
1809d3ae8 and rax, 0xfffffffffffffff0
1809d3aec call 0x1802c5380
1809d3af1 sub rsp, rax
1809d3af4 lea rax, [rbx + 0xf]
1809d3af8 lea r14, [rsp + 0x30]
1809d3afd cmp rax, rbx
1809d3b00 ja 0x1809d3b05
1809d3b02 mov rax, rcx
1809d3b05 and rax, 0xfffffffffffffff0
1809d3b09 call 0x1802c5380
1809d3b0e sub rsp, rax
1809d3b11 lea rax, [rsp + 0x30]
1809d3b16 mov qword ptr [rbp + 0x10], rax
1809d3b1a lea rax, [rbx + 0xf]
1809d3b1e cmp rax, rbx
1809d3b21 ja 0x1809d3b26
1809d3b23 mov rax, rcx
1809d3b26 and rax, 0xfffffffffffffff0
1809d3b2a call 0x1802c5380
1809d3b2f sub rsp, rax
1809d3b32 lea rax, [rbx + 0xf]
1809d3b36 lea r13, [rsp + 0x30]
1809d3b3b cmp rax, rbx
1809d3b3e ja 0x1809d3b43
1809d3b40 mov rax, rcx
1809d3b43 and rax, 0xfffffffffffffff0
1809d3b47 call 0x1802c5380
1809d3b4c sub rsp, rax
1809d3b4f xor edi, edi
1809d3b51 lea rax, [rbx + 0xf]
1809d3b55 mov dword ptr [rbp], edi
1809d3b58 lea r12, [rsp + 0x30]
1809d3b5d cmp rax, rbx
1809d3b60 ja 0x1809d3b65
1809d3b62 mov rax, rcx
1809d3b65 and rax, 0xfffffffffffffff0
1809d3b69 call 0x1802c5380
1809d3b6e sub rsp, rax
1809d3b71 mov r8, rbx
1809d3b74 xor edx, edx
1809d3b76 lea rax, [rsp + 0x30]
1809d3b7b mov rcx, rax
1809d3b7e mov qword ptr [rbp + 8], rax
1809d3b82 call 0x1802f6130
1809d3b87 mov rax, qword ptr [rbp + 0x80]
1809d3b8e lea rdx, [rbp + 0x88]
1809d3b95 mov r8, rbx
1809d3b98 mov rax, qword ptr [rax + 0x18]
1809d3b9c mov qword ptr [rbp + 0x90], rax
1809d3ba3 mov rax, qword ptr [rsi + 0x20]
1809d3ba7 mov rcx, qword ptr [rax + 0xc0]
1809d3bae mov rax, qword ptr [rcx + 0x60]
1809d3bb2 mov ecx, dword ptr [rax + 0x28]
1809d3bb5 shr ecx, 0x1f
1809d3bb8 test cl, cl
1809d3bba mov rcx, r14
1809d3bbd cmovne rdx, r15
1809d3bc1 call 0x1802f64f0
1809d3bc6 mov r8, qword ptr [rbp + 0x90]
1809d3bcd test r8, r8
1809d3bd0 je 0x1809d3f44
1809d3bd6 mov rax, qword ptr [rsi + 0x20]
1809d3bda mov rdx, r14
1809d3bdd mov rcx, qword ptr [rax + 0xc0]
1809d3be4 mov rax, qword ptr [rcx + 0x60]
1809d3be8 mov ecx, dword ptr [rax + 0x28]
1809d3beb shr ecx, 0x1f
1809d3bee test cl, cl
1809d3bf0 jne 0x1809d3bf5
1809d3bf2 mov rdx, qword ptr [r14]
1809d3bf5 mov rax, qword ptr [rsi + 0x20]
1809d3bf9 lea r9, [rbp + 0x18]
1809d3bfd mov qword ptr [rbp + 0x18], rdx
1809d3c01 mov rcx, qword ptr [rax + 0xc0]
1809d3c08 lea rax, [rbp]
1809d3c0c mov qword ptr [rbp + 0x20], rax
1809d3c10 mov rax, qword ptr [rsi + 0x20]
1809d3c14 mov r10, qword ptr [rcx + 0x90]
1809d3c1b mov rdx, r10
1809d3c1e mov rcx, qword ptr [rax + 0xc0]
1809d3c25 lea rax, [rbp + 0x90]
1809d3c2c mov qword ptr [rsp + 0x20], rax
1809d3c31 mov rcx, qword ptr [rcx + 0x90]
1809d3c38 mov rcx, qword ptr [rcx]
1809d3c3b call qword ptr [r10 + 0x10]
1809d3c3f cmp byte ptr [rbp + 0x90], dil
1809d3c46 je 0x1809d3f16
1809d3c4c mov rbx, qword ptr [rbp + 0x80]
1809d3c53 lea rdx, [rbp + 0x88]
1809d3c5a mov r8d, dword ptr [rbp + 0x98]
1809d3c61 mov rax, qword ptr [rbx + 0x18]
1809d3c65 mov qword ptr [rbp + 0x90], rax
1809d3c6c mov rax, qword ptr [rsi + 0x20]
1809d3c70 mov rcx, qword ptr [rax + 0xc0]
1809d3c77 mov rax, qword ptr [rcx + 0x60]
1809d3c7b mov ecx, dword ptr [rax + 0x28]
1809d3c7e shr ecx, 0x1f
1809d3c81 test cl, cl
1809d3c83 mov rcx, r14
1809d3c86 cmovne rdx, r15
1809d3c8a call 0x1802f64f0
1809d3c8f mov r8, qword ptr [rbp + 0x90]
1809d3c96 test r8, r8
1809d3c99 je 0x1809d3f44
1809d3c9f mov rax, qword ptr [rsi + 0x20]
1809d3ca3 mov rdx, r14
1809d3ca6 mov rcx, qword ptr [rax + 0xc0]
1809d3cad mov rax, qword ptr [rcx + 0x60]
1809d3cb1 mov ecx, dword ptr [rax + 0x28]
1809d3cb4 shr ecx, 0x1f
1809d3cb7 test cl, cl
1809d3cb9 jne 0x1809d3cbe
1809d3cbb mov rdx, qword ptr [r14]
1809d3cbe mov rax, qword ptr [rsi + 0x20]
1809d3cc2 lea r9, [rbp + 0x18]
1809d3cc6 mov qword ptr [rbp + 0x18], rdx
1809d3cca mov rcx, qword ptr [rax + 0xc0]
1809d3cd1 lea rax, [rbp + 0x90]
1809d3cd8 mov qword ptr [rsp + 0x20], rax
1809d3cdd mov r10, qword ptr [rcx + 0x98]
1809d3ce4 mov rdx, r10
1809d3ce7 mov rcx, qword ptr [r10]
1809d3cea call qword ptr [r10 + 0x10]
1809d3cee mov r8, qword ptr [rbx + 0x10]
1809d3cf2 test r8, r8
1809d3cf5 je 0x1809d3f44
1809d3cfb mov rax, qword ptr [rsi + 0x20]
1809d3cff lea r9, [rbp + 0x18]
1809d3d03 mov rdx, qword ptr [rbp + 0x10]
1809d3d07 mov qword ptr [rbp + 0x20], rdx
1809d3d0b mov qword ptr [rsp + 0x20], rdx
1809d3d10 mov rcx, qword ptr [rax + 0xc0]
1809d3d17 mov eax, dword ptr [rbp]
1809d3d1a mov dword ptr [rbp + 0x90], eax
1809d3d20 lea rax, [rbp + 0x90]
1809d3d27 mov qword ptr [rbp + 0x18], rax
1809d3d2b mov r10, qword ptr [rcx + 0xa0]
1809d3d32 mov rax, qword ptr [rsi + 0x20]
1809d3d36 mov rdx, r10
1809d3d39 mov rcx, qword ptr [rax + 0xc0]
1809d3d40 mov rcx, qword ptr [rcx + 0xa0]
1809d3d47 mov rcx, qword ptr [rcx]
1809d3d4a call qword ptr [r10 + 0x10]
1809d3d4e mov ebx, dword ptr [rbp + 0x98]
1809d3d54 mov rdx, qword ptr [rbp + 0x10]
1809d3d58 mov r8d, ebx
1809d3d5b mov rcx, qword ptr [rbp + 8]
1809d3d5f call 0x1802f64f0
1809d3d64 mov rax, qword ptr [rbp + 0x80]
1809d3d6b mov r8d, ebx
1809d3d6e mov rdx, qword ptr [rbp + 8]
1809d3d72 mov rcx, r13
1809d3d75 mov rax, qword ptr [rax + 0x20]
1809d3d79 mov qword ptr [rbp + 0x90], rax
1809d3d80 call 0x1802f64f0
1809d3d85 mov rax, qword ptr [rsi + 0x20]
1809d3d89 lea rdx, [rbp + 0x88]
1809d3d90 mov r8d, ebx
1809d3d93 mov rcx, qword ptr [rax + 0xc0]
1809d3d9a mov rax, qword ptr [rcx + 0x60]
1809d3d9e mov ecx, dword ptr [rax + 0x28]
1809d3da1 shr ecx, 0x1f
1809d3da4 test cl, cl
1809d3da6 mov rcx, r12
1809d3da9 cmovne rdx, r15
1809d3dad call 0x1802f64f0
1809d3db2 mov rbx, qword ptr [rbp + 0x90]
1809d3db9 test rbx, rbx
1809d3dbc je 0x1809d3f44
1809d3dc2 mov rax, qword ptr [rsi + 0x20]
1809d3dc6 mov rcx, qword ptr [rax + 0xc0]
1809d3dcd mov rax, qword ptr [rcx + 0x60]
1809d3dd1 mov ecx, dword ptr [rax + 0x28]
1809d3dd4 shr ecx, 0x1f
1809d3dd7 test cl, cl
1809d3dd9 jne 0x1809d3de3
1809d3ddb mov r12, qword ptr [r12]
1809d3ddf mov r13, qword ptr [r13]
1809d3de3 mov rax, qword ptr [rsi + 0x20]
1809d3de7 mov rcx, qword ptr [rax + 0xc0]
1809d3dee mov rdx, qword ptr [rcx + 0x40]
1809d3df2 test byte ptr [rdx + 0x135], 1
1809d3df9 jne 0x1809d3e06
1809d3dfb mov rcx, rdx
1809d3dfe call 0x180258a80
1809d3e03 mov rdx, rax
1809d3e06 mov r9, qword ptr [rbx]
1809d3e09 movzx ecx, word ptr [r9 + 0x12e]
1809d3e11 cmp di, cx
1809d3e14 jae 0x1809d3e38
1809d3e16 mov r8, qword ptr [r9 + 0xb0]
1809d3e1d nop dword ptr [rax]
1809d3e20 movzx eax, di
1809d3e23 add rax, rax
1809d3e26 cmp qword ptr [r8 + rax*8], rdx
1809d3e2a je 0x1809d3f27
1809d3e30 inc di
1809d3e33 cmp di, cx
1809d3e36 jb 0x1809d3e20
1809d3e38 xor r8d, r8d
1809d3e3b mov rcx, rbx
1809d3e3e call 0x180258380
1809d3e43 mov qword ptr [rbp + 0x18], r13
1809d3e47 lea rdx, [rbp + 0x88]
1809d3e4e mov qword ptr [rbp + 0x20], r12
1809d3e52 lea r9, [rbp + 0x18]
1809d3e56 mov r10, qword ptr [rax + 8]
1809d3e5a mov r8, rbx
1809d3e5d mov qword ptr [rsp + 0x20], rdx
1809d3e62 mov rdx, r10
1809d3e65 mov rcx, qword ptr [r10 + 8]
1809d3e69 call qword ptr [r10 + 0x10]
1809d3e6d cmp byte ptr [rbp + 0x88], 0
1809d3e74 jne 0x1809d3f16
1809d3e7a mov rax, qword ptr [rbp + 0x80]
1809d3e81 mov rcx, r14
1809d3e84 mov r8d, dword ptr [rbp + 0x98]
1809d3e8b mov rdx, qword ptr [rbp + 8]
1809d3e8f mov rbx, qword ptr [rax + 0x18]
1809d3e93 call 0x1802f64f0
1809d3e98 test rbx, rbx
1809d3e9b je 0x1809d3f44
1809d3ea1 mov rax, qword ptr [rsi + 0x20]
1809d3ea5 mov rcx, qword ptr [rax + 0xc0]
1809d3eac mov rax, qword ptr [rcx + 0x60]
1809d3eb0 mov ecx, dword ptr [rax + 0x28]
1809d3eb3 shr ecx, 0x1f
1809d3eb6 test cl, cl
1809d3eb8 jne 0x1809d3ebd
1809d3eba mov r14, qword ptr [r14]
1809d3ebd mov rax, qword ptr [rsi + 0x20]
1809d3ec1 lea r9, [rbp + 0x18]
1809d3ec5 mov r8, rbx
1809d3ec8 mov qword ptr [rbp + 0x18], r14
1809d3ecc mov rcx, qword ptr [rax + 0xc0]
1809d3ed3 mov eax, dword ptr [rbp]
1809d3ed6 mov dword ptr [rbp + 0x80], eax
1809d3edc lea rax, [rbp + 0x80]
1809d3ee3 mov qword ptr [rbp + 0x20], rax
1809d3ee7 mov r10, qword ptr [rcx + 0xb0]
1809d3eee mov rax, qword ptr [rsi + 0x20]
1809d3ef2 mov rdx, r10
1809d3ef5 mov rcx, qword ptr [rax + 0xc0]
1809d3efc lea rax, [rbp + 0x80]
1809d3f03 mov qword ptr [rsp + 0x20], rax
1809d3f08 mov rcx, qword ptr [rcx + 0xb0]
1809d3f0f mov rcx, qword ptr [rcx]
1809d3f12 call qword ptr [r10 + 0x10]
1809d3f16 lea rsp, [rbp + 0x38]
1809d3f1a pop r15
1809d3f1c pop r14
1809d3f1e pop r13
1809d3f20 pop r12
1809d3f22 pop rdi
1809d3f23 pop rsi
1809d3f24 pop rbx
1809d3f25 pop rbp
1809d3f26 ret
1809d3f27 movzx ecx, di
1809d3f2a add rcx, rcx
1809d3f2d movsxd rax, dword ptr [r8 + rcx*8 + 8]
1809d3f32 shl rax, 4
1809d3f36 add rax, 0x138
1809d3f3c add rax, r9
1809d3f3f jmp 0x1809d3e43
1809d3f44 call 0x180250150
1809d3f49 int3
