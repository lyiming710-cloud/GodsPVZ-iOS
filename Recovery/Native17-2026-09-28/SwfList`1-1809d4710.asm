1809d4710 mov qword ptr [rsp + 8], rbx
1809d4715 mov qword ptr [rsp + 0x10], rbp
1809d471a mov qword ptr [rsp + 0x18], rsi
1809d471f push rdi
1809d4720 push r14
1809d4722 push r15
1809d4724 sub rsp, 0x20
1809d4728 mov r14, r8
1809d472b mov rdi, rdx
1809d472e mov rsi, rcx
1809d4731 test rdx, rdx
1809d4734 je 0x1809d4857
1809d473a mov r8d, dword ptr [rdx + 0x18]
1809d473e xor ebx, ebx
1809d4740 inc dword ptr [rdx + 0x1c]
1809d4743 mov dword ptr [rdx + 0x18], ebx
1809d4746 test r8d, r8d
1809d4749 jle 0x1809d4759
1809d474b mov rcx, qword ptr [rdi + 0x10]
1809d474f xor r9d, r9d
1809d4752 xor edx, edx
1809d4754 call 0x180cb86f0
1809d4759 mov rax, qword ptr [r14 + 0x20]
1809d475d mov rcx, qword ptr [rax + 0xc0]
1809d4764 mov rdx, qword ptr [rcx + 0x38]
1809d4768 mov rcx, rdi
1809d476b call 0x18065baa0
1809d4770 cmp eax, dword ptr [rsi + 0x18]
1809d4773 jge 0x1809d4791
1809d4775 mov rax, qword ptr [r14 + 0x20]
1809d4779 mov rcx, rdi
1809d477c mov edx, dword ptr [rsi + 0x18]
1809d477f add edx, edx
1809d4781 mov r8, qword ptr [rax + 0xc0]
1809d4788 mov r8, qword ptr [r8 + 0x48]
1809d478c call 0x180805920
1809d4791 mov r15d, dword ptr [rsi + 0x18]
1809d4795 test r15d, r15d
1809d4798 jle 0x1809d483e
1809d479e nop
1809d47a0 mov rax, qword ptr [r14 + 0x20]
1809d47a4 mov rcx, qword ptr [rax + 0xc0]
1809d47ab mov rbp, qword ptr [rcx + 0x50]
1809d47af cmp ebx, dword ptr [rsi + 0x18]
1809d47b2 jae 0x1809d4863
1809d47b8 mov rcx, qword ptr [rsi + 0x10]
1809d47bc test rcx, rcx
1809d47bf je 0x1809d4857
1809d47c5 cmp ebx, dword ptr [rcx + 0x18]
1809d47c8 jae 0x1809d485d
1809d47ce movsxd r8, dword ptr [rdi + 0x18]
1809d47d2 movsxd rax, ebx
1809d47d5 mov rdx, qword ptr [rcx + rax*8 + 0x20]
1809d47da mov rax, qword ptr [r14 + 0x20]
1809d47de mov rcx, qword ptr [rax + 0xc0]
1809d47e5 mov rax, qword ptr [rcx + 0x58]
1809d47e9 inc dword ptr [rdi + 0x1c]
1809d47ec mov rcx, qword ptr [rdi + 0x10]
1809d47f0 test rcx, rcx
1809d47f3 je 0x1809d4857
1809d47f5 cmp r8d, dword ptr [rcx + 0x18]
1809d47f9 jb 0x1809d4814
1809d47fb mov rax, qword ptr [rax + 0x20]
1809d47ff mov rcx, rdi
1809d4802 mov r8, qword ptr [rax + 0xc0]
1809d4809 mov r8, qword ptr [r8 + 0x70]
1809d480d call 0x180847ce0
1809d4812 jmp 0x1809d4833
1809d4814 lea eax, [r8 + 1]
1809d4818 mov dword ptr [rdi + 0x18], eax
1809d481b cmp r8d, dword ptr [rcx + 0x18]
1809d481f jae 0x1809d485d
1809d4821 mov qword ptr [rcx + r8*8 + 0x20], rdx
1809d4826 lea rcx, [rcx + r8*8]
1809d482a add rcx, 0x20
1809d482e call 0x18024f360
1809d4833 inc ebx
1809d4835 cmp ebx, r15d
1809d4838 jl 0x1809d47a0
1809d483e mov rbx, qword ptr [rsp + 0x40]
1809d4843 mov rbp, qword ptr [rsp + 0x48]
1809d4848 mov rsi, qword ptr [rsp + 0x50]
1809d484d add rsp, 0x20
1809d4851 pop r15
1809d4853 pop r14
1809d4855 pop rdi
1809d4856 ret
1809d4857 call 0x180250150
1809d485c int3
1809d485d call 0x180250140
1809d4862 int3
1809d4863 lea rcx, [rip + 0x11d4556]
1809d486a call 0x18024ff10
1809d486f mov rcx, rax
1809d4872 call 0x180250100
1809d4877 xor edx, edx
1809d4879 mov rcx, rax
1809d487c mov rbx, rax
1809d487f call 0x180ca20a0
1809d4884 mov rdx, rbp
1809d4887 mov rcx, rbx
1809d488a call 0x180250110
1809d488f int3
