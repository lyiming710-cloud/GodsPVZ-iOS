1809d3850 mov qword ptr [rsp + 0x10], rbx
1809d3855 mov qword ptr [rsp + 0x18], rbp
1809d385a mov qword ptr [rsp + 0x20], rsi
1809d385f push rdi
1809d3860 push r14
1809d3862 push r15
1809d3864 sub rsp, 0x20
1809d3868 xor ebx, ebx
1809d386a mov rsi, rcx
1809d386d mov rcx, qword ptr [rcx + 0x18]
1809d3871 mov r14, r8
1809d3874 mov dword ptr [rsp + 0x40], ebx
1809d3878 mov r15, rdx
1809d387b test rcx, rcx
1809d387e je 0x1809d3a51
1809d3884 mov rax, qword ptr [r8 + 0x20]
1809d3888 lea r8, [rsp + 0x40]
1809d388d mov r9, qword ptr [rax + 0xc0]
1809d3894 mov r9, qword ptr [r9 + 0x90]
1809d389b call 0x1805e5f60
1809d38a0 test al, al
1809d38a2 je 0x1809d3a1e
1809d38a8 mov rcx, qword ptr [rsi + 0x18]
1809d38ac test rcx, rcx
1809d38af je 0x1809d3a51
1809d38b5 mov rax, qword ptr [r14 + 0x20]
1809d38b9 mov rdx, r15
1809d38bc mov r8, qword ptr [rax + 0xc0]
1809d38c3 mov r8, qword ptr [r8 + 0x98]
1809d38ca call 0x1805de960
1809d38cf mov rdi, qword ptr [rsi + 0x10]
1809d38d3 test rdi, rdi
1809d38d6 je 0x1809d3a51
1809d38dc mov rax, qword ptr [r14 + 0x20]
1809d38e0 movsxd rdx, dword ptr [rsp + 0x40]
1809d38e5 mov rcx, qword ptr [rax + 0xc0]
1809d38ec mov rbp, qword ptr [rcx + 0xa0]
1809d38f3 cmp edx, dword ptr [rdi + 0x18]
1809d38f6 jae 0x1809d3a5d
1809d38fc mov rax, qword ptr [rdi + 0x10]
1809d3900 test rax, rax
1809d3903 je 0x1809d3a51
1809d3909 movsxd rcx, dword ptr [rdi + 0x18]
1809d390d dec rcx
1809d3910 cmp ecx, dword ptr [rax + 0x18]
1809d3913 jae 0x1809d3a57
1809d3919 mov rbp, qword ptr [rax + rcx*8 + 0x20]
1809d391e mov rcx, rdx
1809d3921 cmp edx, dword ptr [rax + 0x18]
1809d3924 jae 0x1809d3a57
1809d392a add rcx, 4
1809d392e mov qword ptr [rax + rdx*8 + 0x20], rbp
1809d3933 mov rdx, rbp
1809d3936 lea rcx, [rax + rcx*8]
1809d393a call 0x18024f360
1809d393f movsxd rdx, dword ptr [rdi + 0x18]
1809d3943 mov rcx, qword ptr [rdi + 0x10]
1809d3947 lea eax, [rdx - 1]
1809d394a mov dword ptr [rdi + 0x18], eax
1809d394d test rcx, rcx
1809d3950 je 0x1809d3a51
1809d3956 lea rax, [rdx - 1]
1809d395a cmp eax, dword ptr [rcx + 0x18]
1809d395d jae 0x1809d3a57
1809d3963 mov qword ptr [rcx + rax*8 + 0x20], rbx
1809d3968 xor edx, edx
1809d396a lea rcx, [rcx + rax*8]
1809d396e add rcx, 0x20
1809d3972 call 0x18024f360
1809d3977 mov rdi, qword ptr [rsi + 0x20]
1809d397b test rdi, rdi
1809d397e je 0x1809d3a51
1809d3984 mov rax, qword ptr [r14 + 0x20]
1809d3988 mov rcx, qword ptr [rax + 0xc0]
1809d398f mov rdx, qword ptr [rcx + 0x40]
1809d3993 test byte ptr [rdx + 0x135], 1
1809d399a jne 0x1809d39a7
1809d399c mov rcx, rdx
1809d399f call 0x180258a80
1809d39a4 mov rdx, rax
1809d39a7 mov r9, qword ptr [rdi]
1809d39aa movzx ecx, word ptr [r9 + 0x12e]
1809d39b2 cmp bx, cx
1809d39b5 jae 0x1809d39d4
1809d39b7 mov r8, qword ptr [r9 + 0xb0]
1809d39be nop
1809d39c0 movzx eax, bx
1809d39c3 add rax, rax
1809d39c6 cmp qword ptr [r8 + rax*8], rdx
1809d39ca je 0x1809d3a37
1809d39cc inc bx
1809d39cf cmp bx, cx
1809d39d2 jb 0x1809d39c0
1809d39d4 xor r8d, r8d
1809d39d7 mov rcx, rdi
1809d39da call 0x180258380
1809d39df mov r10, qword ptr [rax]
1809d39e2 mov r8, r15
1809d39e5 mov r9, qword ptr [rax + 8]
1809d39e9 mov rdx, rbp
1809d39ec mov rcx, rdi
1809d39ef call r10
1809d39f2 test al, al
1809d39f4 jne 0x1809d3a1e
1809d39f6 mov rcx, qword ptr [rsi + 0x18]
1809d39fa test rcx, rcx
1809d39fd je 0x1809d3a51
1809d39ff mov rax, qword ptr [r14 + 0x20]
1809d3a03 mov rdx, rbp
1809d3a06 mov r8d, dword ptr [rsp + 0x40]
1809d3a0b mov r9, qword ptr [rax + 0xc0]
1809d3a12 mov r9, qword ptr [r9 + 0xb0]
1809d3a19 call 0x1805eaa60
1809d3a1e mov rbx, qword ptr [rsp + 0x48]
1809d3a23 mov rbp, qword ptr [rsp + 0x50]
1809d3a28 mov rsi, qword ptr [rsp + 0x58]
1809d3a2d add rsp, 0x20
1809d3a31 pop r15
1809d3a33 pop r14
1809d3a35 pop rdi
1809d3a36 ret
1809d3a37 movzx ecx, bx
1809d3a3a add rcx, rcx
1809d3a3d movsxd rax, dword ptr [r8 + rcx*8 + 8]
1809d3a42 shl rax, 4
1809d3a46 add rax, 0x138
1809d3a4c add rax, r9
1809d3a4f jmp 0x1809d39df
1809d3a51 call 0x180250150
1809d3a56 int3
1809d3a57 call 0x180250140
1809d3a5c int3
1809d3a5d lea rcx, [rip + 0x11d535c]
1809d3a64 call 0x18024ff10
1809d3a69 mov rcx, rax
1809d3a6c call 0x180250100
1809d3a71 xor edx, edx
1809d3a73 mov rcx, rax
1809d3a76 mov rbx, rax
1809d3a79 call 0x180ca20a0
1809d3a7e mov rdx, rbp
1809d3a81 mov rcx, rbx
1809d3a84 call 0x180250110
1809d3a89 int3
