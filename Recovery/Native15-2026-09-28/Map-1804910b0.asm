1804910b0 mov dword ptr [rsp + 0x20], r9d
1804910b5 mov dword ptr [rsp + 0x10], edx
1804910b9 mov qword ptr [rsp + 8], rcx
1804910be push rbx
1804910bf push rsi
1804910c0 push rdi
1804910c1 push r12
1804910c3 push r13
1804910c5 push r14
1804910c7 push r15
1804910c9 sub rsp, 0x90
1804910d0 mov esi, r9d
1804910d3 mov r15, r8
1804910d6 mov rdi, rcx
1804910d9 cmp byte ptr [rip + 0x1853f70], 0
1804910e0 jne 0x1804911dd
1804910e6 lea rcx, [rip + 0x1709c5b]
1804910ed call 0x18024fef0
1804910f2 lea rcx, [rip + 0x170be0f]
1804910f9 call 0x18024fef0
1804910fe lea rcx, [rip + 0x1736873]
180491105 call 0x18024fef0
18049110a lea rcx, [rip + 0x1733d17]
180491111 call 0x18024fef0
180491116 lea rcx, [rip + 0x1733d63]
18049111d call 0x18024fef0
180491122 lea rcx, [rip + 0x17368a7]
180491129 call 0x18024fef0
18049112e lea rcx, [rip + 0x1733da3]
180491135 call 0x18024fef0
18049113a lea rcx, [rip + 0x17368e7]
180491141 call 0x18024fef0
180491146 lea rcx, [rip + 0x1708a9b]
18049114d call 0x18024fef0
180491152 lea rcx, [rip + 0x1708b97]
180491159 call 0x18024fef0
18049115e lea rcx, [rip + 0x170897b]
180491165 call 0x18024fef0
18049116a lea rcx, [rip + 0x1708c87]
180491171 call 0x18024fef0
180491176 lea rcx, [rip + 0x1727013]
18049117d call 0x18024fef0
180491182 lea rcx, [rip + 0x171baaf]
180491189 call 0x18024fef0
18049118e lea rcx, [rip + 0x1726f53]
180491195 call 0x18024fef0
18049119a lea rcx, [rip + 0x171ba3f]
1804911a1 call 0x18024fef0
1804911a6 lea rcx, [rip + 0x171bb93]
1804911ad call 0x18024fef0
1804911b2 lea rcx, [rip + 0x171bbdf]
1804911b9 call 0x18024fef0
1804911be lea rcx, [rip + 0x172cc3b]
1804911c5 call 0x18024fef0
1804911ca lea rcx, [rip + 0x170da47]
1804911d1 call 0x18024fef0
1804911d6 mov byte ptr [rip + 0x1853e73], 1
1804911dd xorps xmm0, xmm0
1804911e0 xor eax, eax
1804911e2 movups xmmword ptr [rsp + 0x40], xmm0
1804911e7 mov qword ptr [rsp + 0x50], rax
1804911ec mov rcx, qword ptr [rip + 0x172cc0d]
1804911f3 call 0x180250100
1804911f8 mov rbx, rax
1804911fb mov qword ptr [rsp + 0x58], rax
180491200 mov rdx, qword ptr [rip + 0x171b9d9]
180491207 mov rcx, rax
18049120a call 0x1808053d0
18049120f mov qword ptr [rsp + 0x38], rbx
180491214 mov rcx, qword ptr [rip + 0x172cbe5]
18049121b call 0x180250100
180491220 mov r12, rax
180491223 mov rdx, qword ptr [rip + 0x171b9b6]
18049122a mov rcx, rax
18049122d call 0x1808053d0
180491232 mov qword ptr [rsp + 0x60], r12
180491237 mov rdx, qword ptr [rdi + 0x10]
18049123b test rdx, rdx
18049123e je 0x180491598
180491244 mov r8, qword ptr [rip + 0x1726e9d]
18049124b lea rcx, [rsp + 0x20]
180491250 call 0x1808197f0
180491255 movups xmm0, xmmword ptr [rsp + 0x20]
18049125a movups xmmword ptr [rsp + 0x68], xmm0
18049125f movsd xmm1, qword ptr [rsp + 0x30]
180491265 movsd qword ptr [rsp + 0x78], xmm1
18049126b mov qword ptr [rsp + 0x20], 0
180491274 lea rbx, [rsp + 0x68]
180491279 mov qword ptr [rsp + 0x28], rbx
18049127e nop
180491280 mov rdx, qword ptr [rip + 0x1736749]
180491287 lea rcx, [rsp + 0x68]
18049128c call 0x180669280
180491291 test al, al
180491293 je 0x1804913ca
180491299 mov r13, qword ptr [rsp + 0x78]
18049129e nop
1804912a0 xor edx, edx
1804912a2 mov rcx, rdi
1804912a5 call 0x18032c3b0
1804912aa mov ecx, eax
1804912ac mov eax, dword ptr [rsp + 0xf0]
1804912b3 inc eax
1804912b5 cmp eax, ecx
1804912b7 cmovl ecx, eax
1804912ba cmp esi, ecx
1804912bc jge 0x1804913be
1804912c2 test r13, r13
1804912c5 je 0x1804915ae
1804912cb mov rcx, qword ptr [r13 + 0x18]
1804912cf test rcx, rcx
1804912d2 je 0x1804915a9
1804912d8 mov r8, qword ptr [rip + 0x171bab9]
1804912df mov edx, esi
1804912e1 call 0x180825710
1804912e6 mov rdi, rax
1804912e9 test rax, rax
1804912ec je 0x1804915a4
1804912f2 lea rcx, [rax + 0x1c]
1804912f6 xor edx, edx
1804912f8 call 0x180ca3820
1804912fd mov r14, rax
180491300 lea rcx, [rdi + 0x20]
180491304 xor edx, edx
180491306 call 0x180ca3820
18049130b xor r9d, r9d
18049130e mov r8, rax
180491311 mov rdx, qword ptr [rip + 0x170d900]
180491318 mov rcx, r14
18049131b call 0x180b73f50
180491320 mov r14, rax
180491323 mov rcx, qword ptr [rip + 0x1709a1e]
18049132a cmp dword ptr [rcx + 0xe0], 0
180491331 jne 0x180491338
180491333 call 0x1802501e0
180491338 xor edx, edx
18049133a mov rcx, r14
18049133d call 0x1812e68f0
180491342 test r15, r15
180491345 je 0x1804913af
180491347 mov r8, qword ptr [r15]
18049134a mov rdx, qword ptr [rip + 0x170bbb7]
180491351 movzx eax, byte ptr [rdx + 0x130]
180491358 cmp byte ptr [r8 + 0x130], al
18049135f jb 0x180491376
180491361 movzx ecx, al
180491364 mov rax, qword ptr [r8 + 0xc8]
18049136b cmp qword ptr [rax + rcx*8 - 8], rdx
180491370 jne 0x180491376
180491372 mov al, 1
180491374 jmp 0x180491378
180491376 xor al, al
180491378 xor edx, edx
18049137a test al, al
18049137c cmovne rdx, r15
180491380 test rdx, rdx
180491383 je 0x1804913af
180491385 xor r8d, r8d
180491388 mov rcx, rdi
18049138b call 0x180329aa0
180491390 test al, al
180491392 je 0x1804913af
180491394 test r12, r12
180491397 je 0x18049159e
18049139d mov r8, qword ptr [rip + 0x171b894]
1804913a4 mov rdx, rdi
1804913a7 mov rcx, r12
1804913aa call 0x180002e70
1804913af inc esi
1804913b1 mov rdi, qword ptr [rsp + 0xd0]
1804913b9 jmp 0x1804912a0
1804913be mov esi, dword ptr [rsp + 0xe8]
1804913c5 jmp 0x180491280
1804913ca mov rdx, qword ptr [rip + 0x17365a7]
1804913d1 mov rcx, rbx
1804913d4 call 0x180302170
1804913d9 mov rsi, qword ptr [rsp + 0x58]
1804913de jmp 0x180491409
1804913e0 mov rdx, qword ptr [rip + 0x1736591]
1804913e7 mov rcx, qword ptr [rsp + 0x28]
1804913ec call 0x180302170
1804913f1 mov rcx, qword ptr [rsp + 0x20]
1804913f6 test rcx, rcx
1804913f9 jne 0x1804915b4
1804913ff mov rsi, qword ptr [rsp + 0x38]
180491404 mov r12, qword ptr [rsp + 0x60]
180491409 test r12, r12
18049140c je 0x180491598
180491412 mov edi, dword ptr [r12 + 0x18]
180491417 mov r14d, dword ptr [rsp + 0xd8]
18049141f cmp r14d, edi
180491422 jg 0x180491593
180491428 mov rcx, qword ptr [rip + 0x1726d61]
18049142f call 0x180250100
180491434 mov rbx, rax
180491437 mov rdx, qword ptr [rip + 0x17086a2]
18049143e mov rcx, rax
180491441 call 0x1807951a0
180491446 test rbx, rbx
180491449 je 0x180491598
18049144f nop
180491450 cmp dword ptr [rbx + 0x20], r14d
180491454 jge 0x180491475
180491456 xor r8d, r8d
180491459 mov edx, edi
18049145b xor ecx, ecx
18049145d call 0x18130d310
180491462 mov r8, qword ptr [rip + 0x170877f]
180491469 mov edx, eax
18049146b mov rcx, rbx
18049146e call 0x180788700
180491473 jmp 0x180491450
180491475 mov r8, qword ptr [rip + 0x1708874]
18049147c mov rdx, rbx
18049147f lea rcx, [rsp + 0x20]
180491484 call 0x18078b020
180491489 movups xmm0, xmmword ptr [rsp + 0x20]
18049148e movups xmmword ptr [rsp + 0x40], xmm0
180491493 movsd xmm1, qword ptr [rsp + 0x30]
180491499 movsd qword ptr [rsp + 0x50], xmm1
18049149f mov qword ptr [rsp + 0x20], 0
1804914a8 lea rbx, [rsp + 0x40]
1804914ad mov qword ptr [rsp + 0x28], rbx
1804914b2 mov rdx, qword ptr [rip + 0x17339c7]
1804914b9 lea rcx, [rsp + 0x40]
1804914be call 0x1806676e0
1804914c3 test al, al
1804914c5 je 0x18049154c
1804914cb mov r8, qword ptr [rip + 0x171b8c6]
1804914d2 mov edx, dword ptr [rsp + 0x50]
1804914d6 mov rcx, r12
1804914d9 call 0x180825710
1804914de mov rdx, rax
1804914e1 test rsi, rsi
1804914e4 je 0x1804915c4
1804914ea mov r9, qword ptr [rip + 0x171b747]
1804914f1 inc dword ptr [rsi + 0x1c]
1804914f4 mov rcx, qword ptr [rsi + 0x10]
1804914f8 movsxd r8, dword ptr [rsi + 0x18]
1804914fc test rcx, rcx
1804914ff je 0x1804915bf
180491505 cmp r8d, dword ptr [rcx + 0x18]
180491509 jb 0x180491524
18049150b mov rax, qword ptr [r9 + 0x20]
18049150f mov r8, qword ptr [rax + 0xc0]
180491516 mov r8, qword ptr [r8 + 0x70]
18049151a mov rcx, rsi
18049151d call 0x180847ce0
180491522 jmp 0x1804914b2
180491524 lea eax, [r8 + 1]
180491528 mov dword ptr [rsi + 0x18], eax
18049152b cmp r8d, dword ptr [rcx + 0x18]
18049152f jae 0x1804915ba
180491535 mov qword ptr [rcx + r8*8 + 0x20], rdx
18049153a lea rcx, [rcx + r8*8]
18049153e add rcx, 0x20
180491542 call 0x18024f360
180491547 jmp 0x1804914b2
18049154c mov rdx, qword ptr [rip + 0x17338d5]
180491553 mov rcx, rbx
180491556 call 0x180302170
18049155b jmp 0x18049157d
18049155d mov rdx, qword ptr [rip + 0x17338c4]
180491564 mov rcx, qword ptr [rsp + 0x28]
180491569 call 0x180302170
18049156e mov rcx, qword ptr [rsp + 0x20]
180491573 test rcx, rcx
180491576 jne 0x1804915ca
180491578 mov rsi, qword ptr [rsp + 0x38]
18049157d mov rax, rsi
180491580 add rsp, 0x90
180491587 pop r15
180491589 pop r14
18049158b pop r13
18049158d pop r12
18049158f pop rdi
180491590 pop rsi
180491591 pop rbx
180491592 ret
180491593 mov rax, r12
180491596 jmp 0x180491580
180491598 call 0x180250150
18049159d nop
18049159e call 0x180250150
1804915a3 nop
1804915a4 call 0x180250150
1804915a9 call 0x180250150
1804915ae call 0x180250150
1804915b3 nop
1804915b4 call 0x180200e60
1804915b9 nop
1804915ba call 0x180250140
1804915bf call 0x180250150
1804915c4 call 0x180250150
1804915c9 nop
1804915ca call 0x180200e60
1804915cf int3
