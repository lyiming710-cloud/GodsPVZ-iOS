1809d53a0 mov qword ptr [rsp + 0x18], r8
1809d53a5 push rbp
1809d53a6 push rsi
1809d53a7 push rdi
1809d53a8 push r12
1809d53aa push r13
1809d53ac push r14
1809d53ae push r15
1809d53b0 sub rsp, 0x30
1809d53b4 lea rbp, [rsp + 0x20]
1809d53b9 mov rax, qword ptr [r9 + 0x20]
1809d53bd mov r15, r9
1809d53c0 mov qword ptr [rbp + 0x58], rbx
1809d53c4 mov r14, rcx
1809d53c7 movsxd rsi, edx
1809d53ca movabs r12, 0xffffffffffffff0
1809d53d4 mov r10, qword ptr [rax + 0xc0]
1809d53db mov rax, qword ptr [r10 + 0x20]
1809d53df mov ebx, dword ptr [rax + 0xfc]
1809d53e5 lea r10, [rbx + 0xf]
1809d53e9 cmp r10, rbx
1809d53ec ja 0x1809d53f1
1809d53ee mov r10, r12
1809d53f1 and r10, 0xfffffffffffffff0
1809d53f5 mov rax, r10
1809d53f8 call 0x1802c5380
1809d53fd sub rsp, rax
1809d5400 lea rcx, [rbx + 0xf]
1809d5404 lea rax, [rsp + 0x20]
1809d5409 mov qword ptr [rbp + 0x68], rax
1809d540d cmp rcx, rbx
1809d5410 ja 0x1809d5415
1809d5412 mov rcx, r12
1809d5415 and rcx, 0xfffffffffffffff0
1809d5419 mov rax, rcx
1809d541c call 0x1802c5380
1809d5421 sub rsp, rcx
1809d5424 lea rcx, [rbx + 0xf]
1809d5428 lea rax, [rsp + 0x20]
1809d542d mov qword ptr [rbp + 0x50], rax
1809d5431 cmp rcx, rbx
1809d5434 ja 0x1809d5439
1809d5436 mov rcx, r12
1809d5439 and rcx, 0xfffffffffffffff0
1809d543d mov rax, rcx
1809d5440 call 0x1802c5380
1809d5445 sub rsp, rcx
1809d5448 lea rcx, [rbx + 0xf]
1809d544c lea r13, [rsp + 0x20]
1809d5451 cmp rcx, rbx
1809d5454 ja 0x1809d5459
1809d5456 mov rcx, r12
1809d5459 and rcx, 0xfffffffffffffff0
1809d545d mov rax, rcx
1809d5460 call 0x1802c5380
1809d5465 sub rsp, rcx
1809d5468 lea rcx, [rbx + 0xf]
1809d546c lea rax, [rsp + 0x20]
1809d5471 mov qword ptr [rbp + 8], rax
1809d5475 cmp rcx, rbx
1809d5478 ja 0x1809d547d
1809d547a mov rcx, r12
1809d547d and rcx, 0xfffffffffffffff0
1809d5481 mov rax, rcx
1809d5484 call 0x1802c5380
1809d5489 sub rsp, rcx
1809d548c mov r8, rbx
1809d548f xor edx, edx
1809d5491 lea rdi, [rsp + 0x20]
1809d5496 mov rcx, rdi
1809d5499 mov qword ptr [rbp], rdi
1809d549d call 0x1802f6130
1809d54a2 lea rcx, [rbx + 0xf]
1809d54a6 cmp rcx, rbx
1809d54a9 ja 0x1809d54ae
1809d54ab mov rcx, r12
1809d54ae and rcx, 0xfffffffffffffff0
1809d54b2 mov rax, rcx
1809d54b5 call 0x1802c5380
1809d54ba sub rsp, rcx
1809d54bd mov r8, rbx
1809d54c0 xor edx, edx
1809d54c2 lea r12, [rsp + 0x20]
1809d54c7 mov rcx, r12
1809d54ca call 0x1802f6130
1809d54cf cmp esi, dword ptr [r14 + 0x18]
1809d54d3 jae 0x1809d5694
1809d54d9 mov r9, qword ptr [r14 + 0x10]
1809d54dd test r9, r9
1809d54e0 je 0x1809d56c7
1809d54e6 mov r10d, dword ptr [r14 + 0x18]
1809d54ea dec r10d
1809d54ed cmp r10d, dword ptr [r9 + 0x18]
1809d54f1 jae 0x1809d56c1
1809d54f7 mov rax, qword ptr [r9]
1809d54fa mov r8, rbx
1809d54fd mov rcx, qword ptr [rbp + 0x68]
1809d5501 add r9, 0x20
1809d5505 mov edx, dword ptr [rax + 0x104]
1809d550b movsxd rax, r10d
1809d550e imul rdx, rax
1809d5512 add rdx, r9
1809d5515 call 0x1802f64f0
1809d551a mov rdx, qword ptr [rbp + 0x68]
1809d551e mov r8, rbx
1809d5521 mov rcx, rdi
1809d5524 call 0x1802f64f0
1809d5529 mov rdx, qword ptr [rbp]
1809d552d mov r8, rbx
1809d5530 mov rcx, qword ptr [rbp + 0x50]
1809d5534 mov rdi, qword ptr [r14 + 0x10]
1809d5538 call 0x1802f64f0
1809d553d test rdi, rdi
1809d5540 je 0x1809d56c7
1809d5546 cmp esi, dword ptr [rdi + 0x18]
1809d5549 jae 0x1809d56c1
1809d554f mov rax, qword ptr [rdi]
1809d5552 mov r8, rbx
1809d5555 mov rdx, qword ptr [rbp + 0x50]
1809d5559 mov ecx, dword ptr [rax + 0x104]
1809d555f imul rcx, rsi
1809d5563 add rcx, 0x20
1809d5567 add rcx, rdi
1809d556a call 0x1802f64f0
1809d556f cmp esi, dword ptr [rdi + 0x18]
1809d5572 jae 0x1809d56c1
1809d5578 mov rax, qword ptr [rdi]
1809d557b mov ecx, dword ptr [rax + 0x104]
1809d5581 mov rax, qword ptr [r15 + 0x20]
1809d5585 imul rcx, rsi
1809d5589 add rdi, rcx
1809d558c mov rcx, qword ptr [rax + 0xc0]
1809d5593 mov rax, qword ptr [rcx + 0x20]
1809d5597 test byte ptr [rax + 0x135], 1
1809d559e jne 0x1809d55a8
1809d55a0 mov rcx, rax
1809d55a3 call 0x180258a80
1809d55a8 mov r8, qword ptr [rbp + 0x50]
1809d55ac lea rdx, [rdi + 0x20]
1809d55b0 mov rcx, rax
1809d55b3 call 0x18024f370
1809d55b8 mov esi, dword ptr [r14 + 0x18]
1809d55bc mov r8, rbx
1809d55bf mov rdi, qword ptr [r14 + 0x10]
1809d55c3 dec esi
1809d55c5 xor edx, edx
1809d55c7 mov dword ptr [r14 + 0x18], esi
1809d55cb mov rcx, r12
1809d55ce call 0x1802f6130
1809d55d3 mov r8, rbx
1809d55d6 mov rdx, r12
1809d55d9 mov rcx, r13
1809d55dc call 0x1802f64f0
1809d55e1 test rdi, rdi
1809d55e4 je 0x1809d56c7
1809d55ea cmp esi, dword ptr [rdi + 0x18]
1809d55ed jae 0x1809d56c1
1809d55f3 mov rax, qword ptr [rdi]
1809d55f6 mov r8, rbx
1809d55f9 mov rdx, r13
1809d55fc mov ecx, dword ptr [rax + 0x104]
1809d5602 movsxd rax, esi
1809d5605 imul rcx, rax
1809d5609 add rcx, 0x20
1809d560d add rcx, rdi
1809d5610 call 0x1802f64f0
1809d5615 cmp esi, dword ptr [rdi + 0x18]
1809d5618 jae 0x1809d56c1
1809d561e mov rax, qword ptr [rdi]
1809d5621 mov ecx, dword ptr [rax + 0x104]
1809d5627 movsxd rax, esi
1809d562a imul rcx, rax
1809d562e mov rax, qword ptr [r15 + 0x20]
1809d5632 add rdi, rcx
1809d5635 mov rcx, qword ptr [rax + 0xc0]
1809d563c mov rax, qword ptr [rcx + 0x20]
1809d5640 test byte ptr [rax + 0x135], 1
1809d5647 jne 0x1809d5651
1809d5649 mov rcx, rax
1809d564c call 0x180258a80
1809d5651 mov r8, r13
1809d5654 lea rdx, [rdi + 0x20]
1809d5658 mov rcx, rax
1809d565b call 0x18024f370
1809d5660 mov rdx, qword ptr [rbp]
1809d5664 mov r8, rbx
1809d5667 mov rcx, qword ptr [rbp + 8]
1809d566b call 0x1802f64f0
1809d5670 mov rdx, qword ptr [rbp + 8]
1809d5674 mov r8, rbx
1809d5677 mov rcx, qword ptr [rbp + 0x60]
1809d567b call 0x1802f64f0
1809d5680 mov rbx, qword ptr [rbp + 0x58]
1809d5684 lea rsp, [rbp + 0x10]
1809d5688 pop r15
1809d568a pop r14
1809d568c pop r13
1809d568e pop r12
1809d5690 pop rdi
1809d5691 pop rsi
1809d5692 pop rbp
1809d5693 ret
1809d5694 lea rcx, [rip + 0x11d3725]
1809d569b call 0x18024ff10
1809d56a0 mov rcx, rax
1809d56a3 call 0x180250100
1809d56a8 xor edx, edx
1809d56aa mov rcx, rax
1809d56ad mov rbx, rax
1809d56b0 call 0x180ca20a0
1809d56b5 mov rdx, r15
1809d56b8 mov rcx, rbx
1809d56bb call 0x180250110
1809d56c0 int3
1809d56c1 call 0x180250140
1809d56c6 int3
1809d56c7 call 0x180250150
1809d56cc int3
