1809d4980 push rbp
1809d4982 push rdi
1809d4983 push r12
1809d4985 push r14
1809d4987 push r15
1809d4989 sub rsp, 0x40
1809d498d lea rbp, [rsp + 0x30]
1809d4992 mov rax, qword ptr [r8 + 0x20]
1809d4996 mov r14, rdx
1809d4999 mov qword ptr [rbp + 0x40], rbx
1809d499d mov r15, rcx
1809d49a0 mov rbx, r8
1809d49a3 mov qword ptr [rbp + 0x50], rsi
1809d49a7 mov r8, qword ptr [rax + 0xc0]
1809d49ae mov rax, qword ptr [r8 + 0x20]
1809d49b2 mov r8d, dword ptr [rax + 0xfc]
1809d49b9 lea rax, [r8 + 0xf]
1809d49bd cmp rax, r8
1809d49c0 ja 0x1809d49cc
1809d49c2 movabs rax, 0xffffffffffffff0
1809d49cc and rax, 0xfffffffffffffff0
1809d49d0 call 0x1802c5380
1809d49d5 sub rsp, rax
1809d49d8 lea rsi, [rsp + 0x30]
1809d49dd test r14, r14
1809d49e0 je 0x1809d4b6f
1809d49e6 mov rax, qword ptr [rbx + 0x20]
1809d49ea mov rcx, qword ptr [rax + 0xc0]
1809d49f1 mov rax, qword ptr [rcx + 0x30]
1809d49f5 mov rcx, r14
1809d49f8 mov rdx, rax
1809d49fb call qword ptr [rax]
1809d49fd mov rax, qword ptr [rbx + 0x20]
1809d4a01 mov rcx, qword ptr [rax + 0xc0]
1809d4a08 mov rax, qword ptr [rcx + 0x38]
1809d4a0c mov r8, qword ptr [rax]
1809d4a0f mov rax, qword ptr [rbx + 0x20]
1809d4a13 mov rcx, qword ptr [rax + 0xc0]
1809d4a1a mov rdx, qword ptr [rcx + 0x38]
1809d4a1e mov rcx, r14
1809d4a21 call r8
1809d4a24 mov rcx, qword ptr [rbx + 0x20]
1809d4a28 mov edi, eax
1809d4a2a mov rdx, qword ptr [rcx + 0xc0]
1809d4a31 mov rcx, qword ptr [rdx + 0x40]
1809d4a35 mov r8, qword ptr [rcx]
1809d4a38 mov rcx, qword ptr [rbx + 0x20]
1809d4a3c mov rdx, qword ptr [rcx + 0xc0]
1809d4a43 mov rcx, r15
1809d4a46 mov rdx, qword ptr [rdx + 0x40]
1809d4a4a call r8
1809d4a4d cmp edi, eax
1809d4a4f jge 0x1809d4a93
1809d4a51 mov rax, qword ptr [rbx + 0x20]
1809d4a55 mov rcx, qword ptr [rax + 0xc0]
1809d4a5c mov rax, qword ptr [rcx + 0x40]
1809d4a60 mov r8, qword ptr [rax]
1809d4a63 mov rax, qword ptr [rbx + 0x20]
1809d4a67 mov rcx, qword ptr [rax + 0xc0]
1809d4a6e mov rdx, qword ptr [rcx + 0x40]
1809d4a72 mov rcx, r15
1809d4a75 call r8
1809d4a78 mov rcx, qword ptr [rbx + 0x20]
1809d4a7c mov rdx, qword ptr [rcx + 0xc0]
1809d4a83 mov rcx, r14
1809d4a86 mov r9, qword ptr [rdx + 0x48]
1809d4a8a lea edx, [rax + rax]
1809d4a8d mov r8, r9
1809d4a90 call qword ptr [r9]
1809d4a93 mov rax, qword ptr [rbx + 0x20]
1809d4a97 xor edi, edi
1809d4a99 mov rcx, qword ptr [rax + 0xc0]
1809d4aa0 mov rax, qword ptr [rcx + 0x40]
1809d4aa4 mov r8, qword ptr [rax]
1809d4aa7 mov rax, qword ptr [rbx + 0x20]
1809d4aab mov rcx, qword ptr [rax + 0xc0]
1809d4ab2 mov rdx, qword ptr [rcx + 0x40]
1809d4ab6 mov rcx, r15
1809d4ab9 call r8
1809d4abc mov r12d, eax
1809d4abf test eax, eax
1809d4ac1 jle 0x1809d4b5a
1809d4ac7 nop word ptr [rax + rax]
1809d4ad0 mov rcx, qword ptr [rbx + 0x20]
1809d4ad4 lea rax, [rbp + 0x48]
1809d4ad8 lea r9, [rbp]
1809d4adc mov dword ptr [rbp + 0x48], edi
1809d4adf mov r8, r15
1809d4ae2 mov qword ptr [rbp], rax
1809d4ae6 mov qword ptr [rbp + 8], rsi
1809d4aea mov rdx, qword ptr [rcx + 0xc0]
1809d4af1 mov qword ptr [rsp + 0x20], rsi
1809d4af6 mov r10, qword ptr [rdx + 0x50]
1809d4afa mov rcx, r10
1809d4afd mov rdx, r10
1809d4b00 mov rcx, qword ptr [rcx]
1809d4b03 call qword ptr [r10 + 0x10]
1809d4b07 mov rax, qword ptr [rbx + 0x20]
1809d4b0b mov rdx, rsi
1809d4b0e mov rcx, qword ptr [rax + 0xc0]
1809d4b15 mov rax, qword ptr [rcx + 0x20]
1809d4b19 mov ecx, dword ptr [rax + 0x28]
1809d4b1c shr ecx, 0x1f
1809d4b1f test cl, cl
1809d4b21 jne 0x1809d4b26
1809d4b23 mov rdx, qword ptr [rsi]
1809d4b26 mov rax, qword ptr [rbx + 0x20]
1809d4b2a lea r9, [rbp + 0x48]
1809d4b2e mov qword ptr [rbp + 0x48], rdx
1809d4b32 mov r8, r14
1809d4b35 mov qword ptr [rsp + 0x20], rdx
1809d4b3a mov rcx, qword ptr [rax + 0xc0]
1809d4b41 mov r10, qword ptr [rcx + 0x58]
1809d4b45 mov rdx, r10
1809d4b48 mov rcx, qword ptr [r10]
1809d4b4b call qword ptr [r10 + 0x10]
1809d4b4f inc edi
1809d4b51 cmp edi, r12d
1809d4b54 jl 0x1809d4ad0
1809d4b5a mov rbx, qword ptr [rbp + 0x40]
1809d4b5e mov rsi, qword ptr [rbp + 0x50]
1809d4b62 lea rsp, [rbp + 0x10]
1809d4b66 pop r15
1809d4b68 pop r14
1809d4b6a pop r12
1809d4b6c pop rdi
1809d4b6d pop rbp
1809d4b6e ret
1809d4b6f call 0x180250150
1809d4b74 int3
