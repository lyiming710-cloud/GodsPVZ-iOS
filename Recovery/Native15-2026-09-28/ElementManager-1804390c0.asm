1804390c0 mov qword ptr [rsp + 0x10], rdx
1804390c5 push rbp
1804390c6 push rsi
1804390c7 push rdi
1804390c8 push r12
1804390ca push r13
1804390cc push r14
1804390ce push r15
1804390d0 sub rsp, 0x70
1804390d4 lea rbp, [rsp + 0x30]
1804390d9 mov qword ptr [rbp + 0x98], rbx
1804390e0 movaps xmmword ptr [rbp + 0x30], xmm6
1804390e4 mov rbx, r8
1804390e7 mov r15, rdx
1804390ea mov rsi, rcx
1804390ed cmp qword ptr [r8 + 0x38], 0
1804390f2 jne 0x18043917f
1804390f8 lea rcx, [rip + 0x1764689]
1804390ff call 0x18024fef0
180439104 lea rcx, [rip + 0x176511d]
18043910b call 0x18024fef0
180439110 lea rcx, [rip + 0x1765051]
180439117 call 0x18024fef0
18043911c lea rcx, [rip + 0x1765b25]
180439123 call 0x18024fef0
180439128 lea rcx, [rip + 0x176bdc1]
18043912f call 0x18024fef0
180439134 lea rcx, [rip + 0x176c115]
18043913b call 0x18024fef0
180439140 lea rcx, [rip + 0x1770e99]
180439147 call 0x18024fef0
18043914c lea rcx, [rip + 0x1778e75]
180439153 call 0x18024fef0
180439158 lea rcx, [rip + 0x1786b91]
18043915f call 0x18024fef0
180439164 lea rcx, [rip + 0x178b4b5]
18043916b call 0x18024fef0
180439170 cmp qword ptr [rbx + 0x38], 0
180439175 jne 0x18043917f
180439177 mov rcx, rbx
18043917a call 0x180258b00
18043917f mov rax, qword ptr [rbx + 0x38]
180439183 mov rcx, qword ptr [rax]
180439186 mov r12d, dword ptr [rcx + 0xfc]
18043918d mov r8d, r12d
180439190 lea rax, [r12 + 0xf]
180439195 cmp rax, r12
180439198 ja 0x1804391a4
18043919a movabs rax, 0xffffffffffffff0
1804391a4 and rax, 0xfffffffffffffff0
1804391a8 call 0x1802c5380
1804391ad sub rsp, rax
1804391b0 lea r13, [rsp + 0x30]
1804391b5 mov qword ptr [rbp + 0x90], 0
1804391c0 xor r14d, r14d
1804391c3 xor edi, edi
1804391c5 mov rax, qword ptr [rbx + 0x38]
1804391c9 mov rcx, qword ptr [rax]
1804391cc mov eax, dword ptr [rcx + 0x28]
1804391cf shr eax, 0x1f
1804391d2 lea rdx, [rbp + 0x88]
1804391d9 test al, al
1804391db cmovne rdx, r15
1804391df mov rcx, r13
1804391e2 call 0x1802f64f0
1804391e7 mov rax, qword ptr [rbx + 0x38]
1804391eb mov rdx, r13
1804391ee mov rcx, qword ptr [rax]
1804391f1 call 0x18024f340
1804391f6 test rax, rax
1804391f9 je 0x1804392a1
1804391ff mov r8, qword ptr [rax]
180439202 mov rdx, qword ptr [rip + 0x178b417]
180439209 movzx eax, byte ptr [rdx + 0x130]
180439210 cmp byte ptr [r8 + 0x130], al
180439217 jb 0x1804392a1
18043921d movzx ecx, al
180439220 mov rax, qword ptr [r8 + 0xc8]
180439227 cmp qword ptr [rax + rcx*8 - 8], rdx
18043922c jne 0x1804392a1
18043922e mov rax, qword ptr [rbx + 0x38]
180439232 mov rcx, qword ptr [rax]
180439235 mov eax, dword ptr [rcx + 0x28]
180439238 shr eax, 0x1f
18043923b lea rdx, [rbp + 0x88]
180439242 test al, al
180439244 cmovne rdx, r15
180439248 mov r8, r12
18043924b mov rcx, r13
18043924e call 0x1802f64f0
180439253 mov rax, qword ptr [rbx + 0x38]
180439257 mov rdx, r13
18043925a mov rcx, qword ptr [rax]
18043925d call 0x18024f340
180439262 mov r9, rax
180439265 test rax, rax
180439268 je 0x1804392a1
18043926a mov r8, qword ptr [rax]
18043926d mov rdx, qword ptr [rip + 0x178b3ac]
180439274 movzx eax, byte ptr [rdx + 0x130]
18043927b cmp byte ptr [r8 + 0x130], al
180439282 jb 0x180439299
180439284 movzx ecx, al
180439287 mov rax, qword ptr [r8 + 0xc8]
18043928e cmp qword ptr [rax + rcx*8 - 8], rdx
180439293 jne 0x180439299
180439295 mov al, 1
180439297 jmp 0x18043929b
180439299 xor al, al
18043929b test al, al
18043929d cmovne r14, r9
1804392a1 mov rax, qword ptr [rbx + 0x38]
1804392a5 mov rcx, qword ptr [rax]
1804392a8 mov eax, dword ptr [rcx + 0x28]
1804392ab shr eax, 0x1f
1804392ae lea rdx, [rbp + 0x88]
1804392b5 test al, al
1804392b7 cmovne rdx, r15
1804392bb mov r8, r12
1804392be mov rcx, r13
1804392c1 call 0x1802f64f0
1804392c6 mov rax, qword ptr [rbx + 0x38]
1804392ca mov rdx, r13
1804392cd mov rcx, qword ptr [rax]
1804392d0 call 0x18024f340
1804392d5 test rax, rax
1804392d8 je 0x180439380
1804392de mov r8, qword ptr [rax]
1804392e1 mov rdx, qword ptr [rip + 0x1778ce0]
1804392e8 movzx eax, byte ptr [rdx + 0x130]
1804392ef cmp byte ptr [r8 + 0x130], al
1804392f6 jb 0x180439380
1804392fc movzx ecx, al
1804392ff mov rax, qword ptr [r8 + 0xc8]
180439306 cmp qword ptr [rax + rcx*8 - 8], rdx
18043930b jne 0x180439380
18043930d mov rax, qword ptr [rbx + 0x38]
180439311 mov rcx, qword ptr [rax]
180439314 mov eax, dword ptr [rcx + 0x28]
180439317 shr eax, 0x1f
18043931a lea rdx, [rbp + 0x88]
180439321 test al, al
180439323 cmovne rdx, r15
180439327 mov r8, r12
18043932a mov rcx, r13
18043932d call 0x1802f64f0
180439332 mov rax, qword ptr [rbx + 0x38]
180439336 mov rdx, r13
180439339 mov rcx, qword ptr [rax]
18043933c call 0x18024f340
180439341 mov r9, rax
180439344 test rax, rax
180439347 je 0x180439380
180439349 mov r8, qword ptr [rax]
18043934c mov rdx, qword ptr [rip + 0x1778c75]
180439353 movzx eax, byte ptr [rdx + 0x130]
18043935a cmp byte ptr [r8 + 0x130], al
180439361 jb 0x180439378
180439363 movzx ecx, al
180439366 mov rax, qword ptr [r8 + 0xc8]
18043936d cmp qword ptr [rax + rcx*8 - 8], rdx
180439372 jne 0x180439378
180439374 mov al, 1
180439376 jmp 0x18043937a
180439378 xor al, al
18043937a test al, al
18043937c cmovne rdi, r9
180439380 mov qword ptr [rsi + 0x10], r14
180439384 lea rcx, [rsi + 0x10]
180439388 mov rdx, r14
18043938b call 0x18024f360
180439390 mov qword ptr [rsi + 0x18], rdi
180439394 lea rcx, [rsi + 0x18]
180439398 mov rdx, rdi
18043939b call 0x18024f360
1804393a0 mov rbx, qword ptr [rip + 0x17643e1]
1804393a7 mov rcx, qword ptr [rip + 0x1786942]
1804393ae cmp dword ptr [rcx + 0xe0], 0
1804393b5 jne 0x1804393bc
1804393b7 call 0x1802501e0
1804393bc xor edx, edx
1804393be mov rcx, rbx
1804393c1 call 0x180ccee30
1804393c6 mov rbx, rax
1804393c9 mov rcx, qword ptr [rip + 0x1765878]
1804393d0 cmp dword ptr [rcx + 0xe0], 0
1804393d7 jne 0x1804393de
1804393d9 call 0x1802501e0
1804393de xor edx, edx
1804393e0 mov rcx, rbx
1804393e3 call 0x180cdd3b0
1804393e8 test rax, rax
1804393eb je 0x180439679
1804393f1 xor edx, edx
1804393f3 mov rcx, rax
1804393f6 call 0x180cba170
1804393fb mov qword ptr [rbp + 0x80], rax
180439402 lea rax, [rbp + 0x80]
180439409 mov qword ptr [rbp + 8], rax
18043940d lea rax, [rbp + 0x90]
180439414 mov qword ptr [rbp + 0x10], rax
180439418 xor r14d, r14d
18043941b mov qword ptr [rbp + 0x18], r14
18043941f movups xmm6, xmmword ptr [rbp + 8]
180439423 movups xmmword ptr [rbp + 0x20], xmm6
180439427 nop word ptr [rax + rax]
180439430 mov r8, qword ptr [rbp + 0x80]
180439437 test r8, r8
18043943a je 0x18043966d
180439440 xor ecx, ecx
180439442 mov rdx, qword ptr [rip + 0x176be07]
180439449 call 0x180002cd0
18043944e test al, al
180439450 je 0x1804395dd
180439456 mov rbx, qword ptr [rbp + 0x80]
18043945d test rbx, rbx
180439460 je 0x180439668
180439466 mov r9, qword ptr [rip + 0x176bde3]
18043946d mov r10, qword ptr [rbx]
180439470 movzx ecx, r14w
180439474 movzx edx, word ptr [r10 + 0x12e]
18043947c cmp r14w, dx
180439480 jae 0x1804394a8
180439482 mov r8, qword ptr [r10 + 0xb0]
180439489 nop dword ptr [rax]
180439490 movzx eax, cx
180439493 add rax, rax
180439496 cmp qword ptr [r8 + rax*8], r9
18043949a je 0x18043958f
1804394a0 inc cx
1804394a3 cmp cx, dx
1804394a6 jb 0x180439490
1804394a8 mov r8d, 1
1804394ae mov rdx, r9
1804394b1 mov rcx, rbx
1804394b4 call 0x180258380
1804394b9 mov rdx, rax
1804394bc mov rax, qword ptr [rdx]
1804394bf mov rdx, qword ptr [rdx + 8]
1804394c3 mov rcx, rbx
1804394c6 call rax
1804394c8 mov rdi, rax
1804394cb mov rcx, qword ptr [rip + 0x1764c96]
1804394d2 call 0x180250100
1804394d7 mov rbx, rax
1804394da mov rcx, qword ptr [rip + 0x1764d47]
1804394e1 test rdi, rdi
1804394e4 je 0x180439663
1804394ea mov rdx, qword ptr [rdi]
1804394ed mov rax, qword ptr [rcx + 0x40]
1804394f1 cmp qword ptr [rdx + 0x40], rax
1804394f5 jne 0x180439658
1804394fb mov rcx, rdi
1804394fe call 0x18024f540
180439503 mov qword ptr [rsp + 0x28], r14
180439508 mov byte ptr [rsp + 0x20], 1
18043950d xor r9d, r9d
180439510 xorps xmm2, xmm2
180439513 mov edx, dword ptr [rax]
180439515 mov rcx, rbx
180439518 call 0x180316940
18043951d test rbx, rbx
180439520 je 0x180439653
180439526 mov qword ptr [rbx + 0x18], r14
18043952a lea rcx, [rbx + 0x18]
18043952e xor edx, edx
180439530 call 0x18024f360
180439535 mov qword ptr [rbx + 0x10], rsi
180439539 lea rcx, [rbx + 0x10]
18043953d mov rdx, rsi
180439540 call 0x18024f360
180439545 mov rcx, qword ptr [rsi + 0x20]
180439549 test rcx, rcx
18043954c je 0x18043964e
180439552 mov r9, qword ptr [rip + 0x1770a87]
180439559 inc dword ptr [rcx + 0x1c]
18043955c mov rdx, qword ptr [rcx + 0x10]
180439560 movsxd r8, dword ptr [rcx + 0x18]
180439564 test rdx, rdx
180439567 je 0x180439649
18043956d cmp r8d, dword ptr [rdx + 0x18]
180439571 jb 0x1804395b2
180439573 mov rax, qword ptr [r9 + 0x20]
180439577 mov r8, qword ptr [rax + 0xc0]
18043957e mov r8, qword ptr [r8 + 0x70]
180439582 mov rdx, rbx
180439585 call 0x180847ce0
18043958a jmp 0x180439430
18043958f movzx ecx, cx
180439592 add rcx, rcx
180439595 mov ecx, dword ptr [r8 + rcx*8 + 8]
18043959a inc ecx
18043959c movsxd rdx, ecx
18043959f shl rdx, 4
1804395a3 add rdx, 0x138
1804395aa add rdx, r10
1804395ad jmp 0x1804394bc
1804395b2 lea eax, [r8 + 1]
1804395b6 mov dword ptr [rcx + 0x18], eax
1804395b9 cmp r8d, dword ptr [rdx + 0x18]
1804395bd jae 0x180439643
1804395c3 mov qword ptr [rdx + r8*8 + 0x20], rbx
1804395c8 add rdx, 0x20
1804395cc lea rcx, [rdx + r8*8]
1804395d0 mov rdx, rbx
1804395d3 call 0x18024f360
1804395d8 jmp 0x180439430
1804395dd movq rax, xmm6
1804395e2 mov rdx, qword ptr [rip + 0x176b907]
1804395e9 mov rcx, qword ptr [rax]
1804395ec call 0x18024f380
1804395f1 mov r8, rax
1804395f4 psrldq xmm6, 8
1804395f9 movq rax, xmm6
1804395fe mov qword ptr [rax], r8
180439601 test r8, r8
180439604 je 0x180439614
180439606 xor ecx, ecx
180439608 mov rdx, qword ptr [rip + 0x176b8e1]
18043960f call 0x180002d90
180439614 jmp 0x180439628
180439616 lea rcx, [rbp + 0x20]
18043961a call 0x1802fb000
18043961f mov rcx, qword ptr [rbp + 0x18]
180439623 test rcx, rcx
180439626 jne 0x180439673
180439628 mov rbx, qword ptr [rbp + 0x98]
18043962f movaps xmm6, xmmword ptr [rbp + 0x30]
180439633 lea rsp, [rbp + 0x40]
180439637 pop r15
180439639 pop r14
18043963b pop r13
18043963d pop r12
18043963f pop rdi
180439640 pop rsi
180439641 pop rbp
180439642 ret
180439643 call 0x180250140
180439648 nop
180439649 call 0x180250150
18043964e call 0x180250150
180439653 call 0x180250150
180439658 mov rdx, rcx
18043965b mov rcx, rdi
18043965e call 0x18024f3a0
180439663 call 0x180250150
180439668 call 0x180250150
18043966d call 0x180250150
180439672 nop
180439673 call 0x180200e60
180439678 int3
180439679 call 0x180250150
18043967e int3
