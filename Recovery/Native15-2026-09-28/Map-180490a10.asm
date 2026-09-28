180490a10 mov qword ptr [rsp + 0x18], r8
180490a15 push rbp
180490a16 push rsi
180490a17 push rdi
180490a18 push r14
180490a1a push r15
180490a1c sub rsp, 0x60
180490a20 lea rbp, [rsp + 0x30]
180490a25 cmp qword ptr [r9 + 0x38], 0
180490a2a mov rsi, r8
180490a2d mov qword ptr [rbp + 0x60], rbx
180490a31 mov r15d, edx
180490a34 mov rbx, r9
180490a37 mov r14, rcx
180490a3a jne 0x180490a44
180490a3c mov rcx, rbx
180490a3f call 0x180258b00
180490a44 mov rax, qword ptr [rbx + 0x38]
180490a48 mov r10, qword ptr [rax]
180490a4b mov r8d, dword ptr [r10 + 0xfc]
180490a52 lea rax, [r8 + 0xf]
180490a56 cmp rax, r8
180490a59 ja 0x180490a65
180490a5b movabs rax, 0xffffffffffffff0
180490a65 and rax, 0xfffffffffffffff0
180490a69 call 0x1802c5380
180490a6e sub rsp, rax
180490a71 lea rdx, [rbp + 0x70]
180490a75 mov rax, qword ptr [rbx + 0x38]
180490a79 lea rdi, [rsp + 0x30]
180490a7e mov rcx, qword ptr [rax]
180490a81 mov eax, dword ptr [rcx + 0x28]
180490a84 mov rcx, rdi
180490a87 shr eax, 0x1f
180490a8a test al, al
180490a8c cmovne rdx, rsi
180490a90 call 0x1802f64f0
180490a95 xor edx, edx
180490a97 mov rcx, r14
180490a9a call 0x18032c3b0
180490a9f mov rcx, qword ptr [rbx + 0x38]
180490aa3 lea r8d, [rax - 1]
180490aa7 mov rdx, qword ptr [rcx]
180490aaa mov ecx, dword ptr [rdx + 0x28]
180490aad shr ecx, 0x1f
180490ab0 test cl, cl
180490ab2 jne 0x180490ab7
180490ab4 mov rdi, qword ptr [rdi]
180490ab7 mov rax, qword ptr [rbx + 0x38]
180490abb lea r9, [rbp + 8]
180490abf mov dword ptr [rbp + 0x78], r8d
180490ac3 mov r8, r14
180490ac6 mov dword ptr [rbp + 0x70], 0
180490acd mov dword ptr [rbp + 0x68], r15d
180490ad1 mov r10, qword ptr [rax + 8]
180490ad5 lea rax, [rbp + 0x68]
180490ad9 mov qword ptr [rbp + 8], rax
180490add mov rdx, r10
180490ae0 lea rax, [rbp + 0x70]
180490ae4 mov qword ptr [rbp + 0x10], rdi
180490ae8 mov qword ptr [rbp + 0x18], rax
180490aec lea rax, [rbp + 0x78]
180490af0 mov qword ptr [rbp + 0x20], rax
180490af4 mov rax, qword ptr [rbx + 0x38]
180490af8 mov rcx, qword ptr [rax + 8]
180490afc lea rax, [rbp]
180490b00 mov qword ptr [rsp + 0x20], rax
180490b05 mov rcx, qword ptr [rcx]
180490b08 call qword ptr [r10 + 0x10]
180490b0c mov rax, qword ptr [rbp]
180490b10 mov rbx, qword ptr [rbp + 0x60]
180490b14 lea rsp, [rbp + 0x30]
180490b18 pop r15
180490b1a pop r14
180490b1c pop rdi
180490b1d pop rsi
180490b1e pop rbp
180490b1f ret
