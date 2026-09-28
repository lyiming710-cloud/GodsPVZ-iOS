1804c4ab0 mov qword ptr [rsp + 8], rbx
1804c4ab5 mov qword ptr [rsp + 0x10], rsi
1804c4aba push rdi
1804c4abb sub rsp, 0x30
1804c4abf cmp byte ptr [rip + 0x182059e], 0
1804c4ac6 movzx ebx, r8b
1804c4aca movaps xmmword ptr [rsp + 0x20], xmm6
1804c4acf mov rsi, rdx
1804c4ad2 movaps xmm6, xmm3
1804c4ad5 mov rdi, rcx
1804c4ad8 jne 0x1804c4af9
1804c4ada lea rcx, [rip + 0x16e728f]
1804c4ae1 call 0x18024fef0
1804c4ae6 lea rcx, [rip + 0x16ed4db]
1804c4aed call 0x18024fef0
1804c4af2 mov byte ptr [rip + 0x182056b], 1
1804c4af9 mov rdx, qword ptr [rsp + 0x60]
1804c4afe lea rcx, [rdi + 0x48]
1804c4b02 mov qword ptr [rdi + 0x48], rdx
1804c4b06 call 0x18024f360
1804c4b0b movss dword ptr [rdi + 0x44], xmm6
1804c4b10 test bl, bl
1804c4b12 je 0x1804c4b8f
1804c4b14 test rsi, rsi
1804c4b17 je 0x1804c4b8f
1804c4b19 mov rdx, qword ptr [rip + 0x16ed4a8]
1804c4b20 mov r8, qword ptr [rsi]
1804c4b23 movzx eax, byte ptr [rdx + 0x130]
1804c4b2a cmp byte ptr [r8 + 0x130], al
1804c4b31 jb 0x1804c4b48
1804c4b33 movzx ecx, al
1804c4b36 mov rax, qword ptr [r8 + 0xc8]
1804c4b3d cmp qword ptr [rax + rcx*8 - 8], rdx
1804c4b42 jne 0x1804c4b48
1804c4b44 mov al, 1
1804c4b46 jmp 0x1804c4b4a
1804c4b48 xor al, al
1804c4b4a xor ebx, ebx
1804c4b4c test al, al
1804c4b4e cmovne rbx, rsi
1804c4b52 test rbx, rbx
1804c4b55 je 0x1804c4b8f
1804c4b57 lea rcx, [rdi + 0x28]
1804c4b5b mov qword ptr [rdi + 0x28], rbx
1804c4b5f mov rdx, rbx
1804c4b62 call 0x18024f360
1804c4b67 mov rbx, qword ptr [rbx + 0x1d0]
1804c4b6e xor edx, edx
1804c4b70 mov rcx, rdi
1804c4b73 call 0x1813044d0
1804c4b78 test rbx, rbx
1804c4b7b je 0x1804c4ba4
1804c4b7d mov r8, qword ptr [rip + 0x16e71ec]
1804c4b84 mov rdx, rax
1804c4b87 mov rcx, rbx
1804c4b8a call 0x180002e70
1804c4b8f mov rbx, qword ptr [rsp + 0x40]
1804c4b94 mov rsi, qword ptr [rsp + 0x48]
1804c4b99 movaps xmm6, xmmword ptr [rsp + 0x20]
1804c4b9e add rsp, 0x30
1804c4ba2 pop rdi
1804c4ba3 ret
1804c4ba4 call 0x180250150
1804c4ba9 int3
