180490b20 mov dword ptr [rsp + 0x20], r9d
180490b25 mov qword ptr [rsp + 0x18], r8
180490b2a mov dword ptr [rsp + 0x10], edx
180490b2e mov qword ptr [rsp + 8], rcx
180490b33 push rbp
180490b34 push rbx
180490b35 push rsi
180490b36 push rdi
180490b37 push r12
180490b39 push r13
180490b3b push r14
180490b3d push r15
180490b3f sub rsp, 0xa8
180490b46 lea rbp, [rsp + 0x20]
180490b4b mov rdi, rcx
180490b4e mov rsi, qword ptr [rbp + 0xf8]
180490b55 cmp qword ptr [rsi + 0x38], 0
180490b5a jne 0x180490c5f
180490b60 lea rcx, [rip + 0x170a1e1]
180490b67 call 0x18024fef0
180490b6c lea rcx, [rip + 0x170c395]
180490b73 call 0x18024fef0
180490b78 lea rcx, [rip + 0x1736df9]
180490b7f call 0x18024fef0
180490b84 lea rcx, [rip + 0x173429d]
180490b8b call 0x18024fef0
180490b90 lea rcx, [rip + 0x17342e9]
180490b97 call 0x18024fef0
180490b9c lea rcx, [rip + 0x1736e2d]
180490ba3 call 0x18024fef0
180490ba8 lea rcx, [rip + 0x1734329]
180490baf call 0x18024fef0
180490bb4 lea rcx, [rip + 0x1736e6d]
180490bbb call 0x18024fef0
180490bc0 lea rcx, [rip + 0x1709021]
180490bc7 call 0x18024fef0
180490bcc lea rcx, [rip + 0x170911d]
180490bd3 call 0x18024fef0
180490bd8 lea rcx, [rip + 0x1708f01]
180490bdf call 0x18024fef0
180490be4 lea rcx, [rip + 0x170920d]
180490beb call 0x18024fef0
180490bf0 lea rcx, [rip + 0x1727599]
180490bf7 call 0x18024fef0
180490bfc lea rcx, [rip + 0x171c035]
180490c03 call 0x18024fef0
180490c08 lea rcx, [rip + 0x17274d9]
180490c0f call 0x18024fef0
180490c14 lea rcx, [rip + 0x171bfc5]
180490c1b call 0x18024fef0
180490c20 lea rcx, [rip + 0x171c119]
180490c27 call 0x18024fef0
180490c2c lea rcx, [rip + 0x171c165]
180490c33 call 0x18024fef0
180490c38 lea rcx, [rip + 0x172d1c1]
180490c3f call 0x18024fef0
180490c44 lea rcx, [rip + 0x170dfcd]
180490c4b call 0x18024fef0
180490c50 cmp qword ptr [rsi + 0x38], 0
180490c55 jne 0x180490c5f
180490c57 mov rcx, rsi
180490c5a call 0x180258b00
180490c5f mov rax, qword ptr [rsi + 0x38]
180490c63 mov rcx, qword ptr [rax]
180490c66 mov eax, dword ptr [rcx + 0xfc]
180490c6c mov dword ptr [rbp + 0x18], eax
180490c6f lea rcx, [rax + 0xf]
180490c73 cmp rcx, rax
180490c76 ja 0x180490c82
180490c78 movabs rcx, 0xffffffffffffff0
180490c82 and rcx, 0xfffffffffffffff0
180490c86 mov rax, rcx
180490c89 call 0x1802c5380
180490c8e sub rsp, rcx
180490c91 lea rax, [rsp + 0x20]
180490c96 mov qword ptr [rbp + 0x20], rax
180490c9a xorps xmm0, xmm0
180490c9d xor eax, eax
180490c9f movups xmmword ptr [rbp + 0x30], xmm0
180490ca3 mov qword ptr [rbp + 0x40], rax
180490ca7 mov rcx, qword ptr [rip + 0x172d152]
180490cae call 0x180250100
180490cb3 mov r12, rax
180490cb6 mov rdx, qword ptr [rip + 0x171bf23]
180490cbd mov rcx, rax
180490cc0 call 0x1808053d0
180490cc5 mov qword ptr [rbp + 0x28], r12
180490cc9 mov rcx, qword ptr [rip + 0x172d130]
180490cd0 call 0x180250100
180490cd5 mov r15, rax
180490cd8 mov rdx, qword ptr [rip + 0x171bf01]
180490cdf mov rcx, rax
180490ce2 call 0x1808053d0
180490ce7 mov qword ptr [rbp + 0x48], r15
180490ceb mov rdx, qword ptr [rdi + 0x10]
180490cef test rdx, rdx
180490cf2 je 0x180491078
180490cf8 mov r8, qword ptr [rip + 0x17273e9]
180490cff lea rcx, [rbp]
180490d03 call 0x1808197f0
180490d08 movups xmm0, xmmword ptr [rbp]
180490d0c movups xmmword ptr [rbp + 0x50], xmm0
180490d10 movsd xmm1, qword ptr [rbp + 0x10]
180490d15 movsd qword ptr [rbp + 0x60], xmm1
180490d1a mov qword ptr [rbp], 0
180490d22 lea rbx, [rbp + 0x50]
180490d26 mov qword ptr [rbp + 8], rbx
180490d2a nop word ptr [rax + rax]
180490d30 mov rdx, qword ptr [rip + 0x1736c99]
180490d37 lea rcx, [rbp + 0x50]
180490d3b call 0x180669280
180490d40 test al, al
180490d42 je 0x180490eb2
180490d48 mov r13, qword ptr [rbp + 0x60]
180490d4c mov r14d, dword ptr [rbp + 0xe8]
180490d53 xor edx, edx
180490d55 mov rcx, rdi
180490d58 call 0x18032c3b0
180490d5d mov ecx, dword ptr [rbp + 0xf0]
180490d63 inc ecx
180490d65 cmp ecx, eax
180490d67 cmovl eax, ecx
180490d6a cmp r14d, eax
180490d6d jge 0x180490d30
180490d6f test r13, r13
180490d72 je 0x18049108e
180490d78 mov rcx, qword ptr [r13 + 0x18]
180490d7c test rcx, rcx
180490d7f je 0x180491089
180490d85 mov r8, qword ptr [rip + 0x171c00c]
180490d8c mov edx, r14d
180490d8f call 0x180825710
180490d94 mov rdi, rax
180490d97 test rax, rax
180490d9a je 0x180491084
180490da0 lea rcx, [rax + 0x1c]
180490da4 xor edx, edx
180490da6 call 0x180ca3820
180490dab mov rsi, rax
180490dae lea rcx, [rdi + 0x20]
180490db2 xor edx, edx
180490db4 call 0x180ca3820
180490db9 xor r9d, r9d
180490dbc mov r8, rax
180490dbf mov rdx, qword ptr [rip + 0x170de52]
180490dc6 mov rcx, rsi
180490dc9 call 0x180b73f50
180490dce mov rsi, rax
180490dd1 mov rcx, qword ptr [rip + 0x1709f70]
180490dd8 cmp dword ptr [rcx + 0xe0], 0
180490ddf jne 0x180490de6
180490de1 call 0x1802501e0
180490de6 xor edx, edx
180490de8 mov rcx, rsi
180490deb call 0x1812e68f0
180490df0 mov rsi, qword ptr [rbp + 0xf8]
180490df7 mov rax, qword ptr [rsi + 0x38]
180490dfb mov rcx, qword ptr [rax]
180490dfe mov eax, dword ptr [rcx + 0x28]
180490e01 shr eax, 0x1f
180490e04 lea rdx, [rbp + 0xe0]
180490e0b test al, al
180490e0d cmovne rdx, qword ptr [rbp + 0xe0]
180490e15 mov r8d, dword ptr [rbp + 0x18]
180490e19 mov rcx, qword ptr [rbp + 0x20]
180490e1d call 0x1802f64f0
180490e22 mov rax, qword ptr [rsi + 0x38]
180490e26 mov rdx, qword ptr [rbp + 0x20]
180490e2a mov rcx, qword ptr [rax]
180490e2d call 0x18024f340
180490e32 mov r10, rax
180490e35 test rax, rax
180490e38 je 0x180490ea3
180490e3a mov r9, qword ptr [rax]
180490e3d mov r8, qword ptr [rip + 0x170c0c4]
180490e44 movzx ecx, byte ptr [r8 + 0x130]
180490e4c cmp byte ptr [r9 + 0x130], cl
180490e53 jb 0x180490e6a
180490e55 movzx edx, cl
180490e58 mov rcx, qword ptr [r9 + 0xc8]
180490e5f cmp qword ptr [rcx + rdx*8 - 8], r8
180490e64 jne 0x180490e6a
180490e66 mov al, 1
180490e68 jmp 0x180490e6c
180490e6a xor al, al
180490e6c xor edx, edx
180490e6e test al, al
180490e70 cmovne rdx, r10
180490e74 test rdx, rdx
180490e77 je 0x180490ea3
180490e79 xor r8d, r8d
180490e7c mov rcx, rdi
180490e7f call 0x180329aa0
180490e84 test al, al
180490e86 je 0x180490ea3
180490e88 test r15, r15
180490e8b je 0x18049107e
180490e91 mov r8, qword ptr [rip + 0x171bda0]
180490e98 mov rdx, rdi
180490e9b mov rcx, r15
180490e9e call 0x180002e70
180490ea3 inc r14d
180490ea6 mov rdi, qword ptr [rbp + 0xd0]
180490ead jmp 0x180490d53
180490eb2 mov rdx, qword ptr [rip + 0x1736abf]
180490eb9 mov rcx, rbx
180490ebc call 0x180302170
180490ec1 jmp 0x180490ee8
180490ec3 mov rdx, qword ptr [rip + 0x1736aae]
180490eca mov rcx, qword ptr [rbp + 8]
180490ece call 0x180302170
180490ed3 mov rcx, qword ptr [rbp]
180490ed7 test rcx, rcx
180490eda jne 0x180491094
180490ee0 mov r12, qword ptr [rbp + 0x28]
180490ee4 mov r15, qword ptr [rbp + 0x48]
180490ee8 test r15, r15
180490eeb je 0x180491078
180490ef1 mov edi, dword ptr [r15 + 0x18]
180490ef5 mov r14d, dword ptr [rbp + 0xd8]
180490efc cmp r14d, edi
180490eff jg 0x180491073
180490f05 mov rcx, qword ptr [rip + 0x1727284]
180490f0c call 0x180250100
180490f11 mov rbx, rax
180490f14 mov rdx, qword ptr [rip + 0x1708bc5]
180490f1b mov rcx, rax
180490f1e call 0x1807951a0
180490f23 test rbx, rbx
180490f26 je 0x180491078
180490f2c nop dword ptr [rax]
180490f30 cmp dword ptr [rbx + 0x20], r14d
180490f34 jge 0x180490f55
180490f36 xor r8d, r8d
180490f39 mov edx, edi
180490f3b xor ecx, ecx
180490f3d call 0x18130d310
180490f42 mov r8, qword ptr [rip + 0x1708c9f]
180490f49 mov edx, eax
180490f4b mov rcx, rbx
180490f4e call 0x180788700
180490f53 jmp 0x180490f30
180490f55 mov r8, qword ptr [rip + 0x1708d94]
180490f5c mov rdx, rbx
180490f5f lea rcx, [rbp]
180490f63 call 0x18078b020
180490f68 movups xmm0, xmmword ptr [rbp]
180490f6c movups xmmword ptr [rbp + 0x30], xmm0
180490f70 movsd xmm1, qword ptr [rbp + 0x10]
180490f75 movsd qword ptr [rbp + 0x40], xmm1
180490f7a mov qword ptr [rbp], 0
180490f82 lea rbx, [rbp + 0x30]
180490f86 mov qword ptr [rbp + 8], rbx
180490f8a nop word ptr [rax + rax]
180490f90 mov rdx, qword ptr [rip + 0x1733ee9]
180490f97 lea rcx, [rbp + 0x30]
180490f9b call 0x1806676e0
180490fa0 test al, al
180490fa2 je 0x18049102e
180490fa8 mov r8, qword ptr [rip + 0x171bde9]
180490faf mov edx, dword ptr [rbp + 0x40]
180490fb2 mov rcx, r15
180490fb5 call 0x180825710
180490fba mov rdx, rax
180490fbd test r12, r12
180490fc0 je 0x1804910a4
180490fc6 mov r9, qword ptr [rip + 0x171bc6b]
180490fcd inc dword ptr [r12 + 0x1c]
180490fd2 mov rcx, qword ptr [r12 + 0x10]
180490fd7 movsxd r8, dword ptr [r12 + 0x18]
180490fdc test rcx, rcx
180490fdf je 0x18049109f
180490fe5 cmp r8d, dword ptr [rcx + 0x18]
180490fe9 jb 0x180491004
180490feb mov rax, qword ptr [r9 + 0x20]
180490fef mov r8, qword ptr [rax + 0xc0]
180490ff6 mov r8, qword ptr [r8 + 0x70]
180490ffa mov rcx, r12
180490ffd call 0x180847ce0
180491002 jmp 0x180490f90
180491004 lea eax, [r8 + 1]
180491008 mov dword ptr [r12 + 0x18], eax
18049100d cmp r8d, dword ptr [rcx + 0x18]
180491011 jae 0x18049109a
180491017 mov qword ptr [rcx + r8*8 + 0x20], rdx
18049101c lea rcx, [rcx + r8*8]
180491020 add rcx, 0x20
180491024 call 0x18024f360
180491029 jmp 0x180490f90
18049102e mov rdx, qword ptr [rip + 0x1733df3]
180491035 mov rcx, rbx
180491038 call 0x180302170
18049103d jmp 0x18049105c
18049103f mov rdx, qword ptr [rip + 0x1733de2]
180491046 mov rcx, qword ptr [rbp + 8]
18049104a call 0x180302170
18049104f mov rcx, qword ptr [rbp]
180491053 test rcx, rcx
180491056 jne 0x1804910aa
180491058 mov r12, qword ptr [rbp + 0x28]
18049105c mov rax, r12
18049105f lea rsp, [rbp + 0x88]
180491066 pop r15
180491068 pop r14
18049106a pop r13
18049106c pop r12
18049106e pop rdi
18049106f pop rsi
180491070 pop rbx
180491071 pop rbp
180491072 ret
180491073 mov rax, r15
180491076 jmp 0x18049105f
180491078 call 0x180250150
18049107d nop
18049107e call 0x180250150
180491083 nop
180491084 call 0x180250150
180491089 call 0x180250150
18049108e call 0x180250150
180491093 nop
180491094 call 0x180200e60
180491099 nop
18049109a call 0x180250140
18049109f call 0x180250150
1804910a4 call 0x180250150
1804910a9 nop
1804910aa call 0x180200e60
1804910af int3
