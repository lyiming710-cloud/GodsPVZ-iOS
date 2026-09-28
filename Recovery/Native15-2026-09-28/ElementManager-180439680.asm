180439680 mov qword ptr [rsp + 0x18], rbx
180439685 push rsi
180439686 push rdi
180439687 push r14
180439689 sub rsp, 0x70
18043968d movaps xmmword ptr [rsp + 0x60], xmm6
180439692 mov rbx, rdx
180439695 mov rsi, rcx
180439698 cmp byte ptr [rip + 0x18ab98c], 0
18043969f jne 0x180439720
1804396a1 lea rcx, [rip + 0x17640e0]
1804396a8 call 0x18024fef0
1804396ad lea rcx, [rip + 0x1764b74]
1804396b4 call 0x18024fef0
1804396b9 lea rcx, [rip + 0x1764aa8]
1804396c0 call 0x18024fef0
1804396c5 lea rcx, [rip + 0x176557c]
1804396cc call 0x18024fef0
1804396d1 lea rcx, [rip + 0x176b818]
1804396d8 call 0x18024fef0
1804396dd lea rcx, [rip + 0x176bb6c]
1804396e4 call 0x18024fef0
1804396e9 lea rcx, [rip + 0x17708f0]
1804396f0 call 0x18024fef0
1804396f5 lea rcx, [rip + 0x17788cc]
1804396fc call 0x18024fef0
180439701 lea rcx, [rip + 0x17865e8]
180439708 call 0x18024fef0
18043970d lea rcx, [rip + 0x178af0c]
180439714 call 0x18024fef0
180439719 mov byte ptr [rip + 0x18ab90b], 1
180439720 xor r14d, r14d
180439723 mov qword ptr [rsp + 0x98], r14
18043972b mov edx, r14d
18043972e mov edi, r14d
180439731 test rbx, rbx
180439734 je 0x1804397c3
18043973a mov r9, qword ptr [rbx]
18043973d mov r8, qword ptr [rip + 0x178aedc]
180439744 movzx eax, byte ptr [r8 + 0x130]
18043974c cmp byte ptr [r9 + 0x130], al
180439753 jb 0x18043977c
180439755 movzx ecx, al
180439758 mov rax, qword ptr [r9 + 0xc8]
18043975f cmp qword ptr [rax + rcx*8 - 8], r8
180439764 jne 0x18043977c
180439766 movzx ecx, byte ptr [r8 + 0x130]
18043976e cmp qword ptr [rax + rcx*8 - 8], r8
180439773 sete al
180439776 test al, al
180439778 cmovne rdx, rbx
18043977c mov r10, qword ptr [rbx]
18043977f mov r8, qword ptr [rip + 0x1778842]
180439786 movzx eax, byte ptr [r8 + 0x130]
18043978e cmp byte ptr [r10 + 0x130], al
180439795 jb 0x1804397c3
180439797 movzx ecx, al
18043979a mov rax, qword ptr [r10 + 0xc8]
1804397a1 cmp qword ptr [rax + rcx*8 - 8], r8
1804397a6 jne 0x1804397c3
1804397a8 movzx ecx, byte ptr [r8 + 0x130]
1804397b0 cmp qword ptr [rax + rcx*8 - 8], r8
1804397b5 jne 0x1804397bb
1804397b7 mov al, 1
1804397b9 jmp 0x1804397bd
1804397bb xor al, al
1804397bd test al, al
1804397bf cmovne rdi, rbx
1804397c3 mov qword ptr [rsi + 0x10], rdx
1804397c7 lea rcx, [rsi + 0x10]
1804397cb call 0x18024f360
1804397d0 mov qword ptr [rsi + 0x18], rdi
1804397d4 lea rcx, [rsi + 0x18]
1804397d8 mov rdx, rdi
1804397db call 0x18024f360
1804397e0 mov rbx, qword ptr [rip + 0x1763fa1]
1804397e7 mov rcx, qword ptr [rip + 0x1786502]
1804397ee cmp dword ptr [rcx + 0xe0], r14d
1804397f5 jne 0x1804397fc
1804397f7 call 0x1802501e0
1804397fc xor edx, edx
1804397fe mov rcx, rbx
180439801 call 0x180ccee30
180439806 mov rbx, rax
180439809 mov rcx, qword ptr [rip + 0x1765438]
180439810 cmp dword ptr [rcx + 0xe0], r14d
180439817 jne 0x18043981e
180439819 call 0x1802501e0
18043981e xor edx, edx
180439820 mov rcx, rbx
180439823 call 0x180cdd3b0
180439828 test rax, rax
18043982b je 0x180439ab2
180439831 xor edx, edx
180439833 mov rcx, rax
180439836 call 0x180cba170
18043983b mov qword ptr [rsp + 0x90], rax
180439843 lea rax, [rsp + 0x90]
18043984b mov qword ptr [rsp + 0x38], rax
180439850 lea rax, [rsp + 0x98]
180439858 mov qword ptr [rsp + 0x40], rax
18043985d mov qword ptr [rsp + 0x48], r14
180439862 movups xmm6, xmmword ptr [rsp + 0x38]
180439867 movups xmmword ptr [rsp + 0x50], xmm6
18043986c nop dword ptr [rax]
180439870 mov r8, qword ptr [rsp + 0x90]
180439878 test r8, r8
18043987b je 0x180439aa6
180439881 xor ecx, ecx
180439883 mov rdx, qword ptr [rip + 0x176b9c6]
18043988a call 0x180002cd0
18043988f test al, al
180439891 je 0x180439a19
180439897 mov rbx, qword ptr [rsp + 0x90]
18043989f test rbx, rbx
1804398a2 je 0x180439aa1
1804398a8 mov r9, qword ptr [rip + 0x176b9a1]
1804398af mov r10, qword ptr [rbx]
1804398b2 movzx ecx, r14w
1804398b6 movzx edx, word ptr [r10 + 0x12e]
1804398be cmp r14w, dx
1804398c2 jae 0x1804398e8
1804398c4 mov r8, qword ptr [r10 + 0xb0]
1804398cb nop dword ptr [rax + rax]
1804398d0 movzx eax, cx
1804398d3 add rax, rax
1804398d6 cmp qword ptr [r8 + rax*8], r9
1804398da je 0x1804399cf
1804398e0 inc cx
1804398e3 cmp cx, dx
1804398e6 jb 0x1804398d0
1804398e8 mov r8d, 1
1804398ee mov rdx, r9
1804398f1 mov rcx, rbx
1804398f4 call 0x180258380
1804398f9 mov rdx, rax
1804398fc mov rax, qword ptr [rdx]
1804398ff mov rdx, qword ptr [rdx + 8]
180439903 mov rcx, rbx
180439906 call rax
180439908 mov rdi, rax
18043990b mov rcx, qword ptr [rip + 0x1764856]
180439912 call 0x180250100
180439917 mov rbx, rax
18043991a mov rcx, qword ptr [rip + 0x1764907]
180439921 test rdi, rdi
180439924 je 0x180439a9c
18043992a mov rdx, qword ptr [rdi]
18043992d mov rax, qword ptr [rcx + 0x40]
180439931 cmp qword ptr [rdx + 0x40], rax
180439935 jne 0x180439a91
18043993b mov rcx, rdi
18043993e call 0x18024f540
180439943 mov qword ptr [rsp + 0x28], r14
180439948 mov byte ptr [rsp + 0x20], 1
18043994d xor r9d, r9d
180439950 xorps xmm2, xmm2
180439953 mov edx, dword ptr [rax]
180439955 mov rcx, rbx
180439958 call 0x180316940
18043995d test rbx, rbx
180439960 je 0x180439a8c
180439966 mov qword ptr [rbx + 0x18], r14
18043996a lea rcx, [rbx + 0x18]
18043996e xor edx, edx
180439970 call 0x18024f360
180439975 mov qword ptr [rbx + 0x10], rsi
180439979 lea rcx, [rbx + 0x10]
18043997d mov rdx, rsi
180439980 call 0x18024f360
180439985 mov rcx, qword ptr [rsi + 0x20]
180439989 test rcx, rcx
18043998c je 0x180439a87
180439992 mov r9, qword ptr [rip + 0x1770647]
180439999 inc dword ptr [rcx + 0x1c]
18043999c mov rdx, qword ptr [rcx + 0x10]
1804399a0 movsxd r8, dword ptr [rcx + 0x18]
1804399a4 test rdx, rdx
1804399a7 je 0x180439a82
1804399ad cmp r8d, dword ptr [rdx + 0x18]
1804399b1 jb 0x1804399f2
1804399b3 mov rax, qword ptr [r9 + 0x20]
1804399b7 mov r8, qword ptr [rax + 0xc0]
1804399be mov r8, qword ptr [r8 + 0x70]
1804399c2 mov rdx, rbx
1804399c5 call 0x180847ce0
1804399ca jmp 0x180439870
1804399cf movzx ecx, cx
1804399d2 add rcx, rcx
1804399d5 mov ecx, dword ptr [r8 + rcx*8 + 8]
1804399da inc ecx
1804399dc movsxd rdx, ecx
1804399df shl rdx, 4
1804399e3 add rdx, 0x138
1804399ea add rdx, r10
1804399ed jmp 0x1804398fc
1804399f2 lea eax, [r8 + 1]
1804399f6 mov dword ptr [rcx + 0x18], eax
1804399f9 cmp r8d, dword ptr [rdx + 0x18]
1804399fd jae 0x180439a7c
1804399ff mov qword ptr [rdx + r8*8 + 0x20], rbx
180439a04 add rdx, 0x20
180439a08 lea rcx, [rdx + r8*8]
180439a0c mov rdx, rbx
180439a0f call 0x18024f360
180439a14 jmp 0x180439870
180439a19 movq rax, xmm6
180439a1e mov rdx, qword ptr [rip + 0x176b4cb]
180439a25 mov rcx, qword ptr [rax]
180439a28 call 0x18024f380
180439a2d mov r8, rax
180439a30 psrldq xmm6, 8
180439a35 movq rax, xmm6
180439a3a mov qword ptr [rax], r8
180439a3d test r8, r8
180439a40 je 0x180439a50
180439a42 xor ecx, ecx
180439a44 mov rdx, qword ptr [rip + 0x176b4a5]
180439a4b call 0x180002d90
180439a50 jmp 0x180439a66
180439a52 lea rcx, [rsp + 0x50]
180439a57 call 0x1802fb000
180439a5c mov rcx, qword ptr [rsp + 0x48]
180439a61 test rcx, rcx
180439a64 jne 0x180439aac
180439a66 mov rbx, qword ptr [rsp + 0xa0]
180439a6e movaps xmm6, xmmword ptr [rsp + 0x60]
180439a73 add rsp, 0x70
180439a77 pop r14
180439a79 pop rdi
180439a7a pop rsi
180439a7b ret
180439a7c call 0x180250140
180439a81 nop
180439a82 call 0x180250150
180439a87 call 0x180250150
180439a8c call 0x180250150
180439a91 mov rdx, rcx
180439a94 mov rcx, rdi
180439a97 call 0x18024f3a0
180439a9c call 0x180250150
180439aa1 call 0x180250150
180439aa6 call 0x180250150
180439aab nop
180439aac call 0x180200e60
180439ab1 int3
180439ab2 call 0x180250150
180439ab7 int3
