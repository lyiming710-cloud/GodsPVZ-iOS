1804c4f70 mov qword ptr [rsp + 8], rbx
1804c4f75 mov qword ptr [rsp + 0x10], rbp
1804c4f7a mov qword ptr [rsp + 0x18], rsi
1804c4f7f push rdi
1804c4f80 sub rsp, 0x50
1804c4f84 cmp byte ptr [rip + 0x18200da], 0
1804c4f8b mov rdi, r8
1804c4f8e mov ebp, edx
1804c4f90 mov rbx, rcx
1804c4f93 jne 0x1804c4ffc
1804c4f95 lea rcx, [rip + 0x16e021c]
1804c4f9c call 0x18024fef0
1804c4fa1 lea rcx, [rip + 0x16dd070]
1804c4fa8 call 0x18024fef0
1804c4fad lea rcx, [rip + 0x16e6d0c]
1804c4fb4 call 0x18024fef0
1804c4fb9 lea rcx, [rip + 0x16f8bd8]
1804c4fc0 call 0x18024fef0
1804c4fc5 lea rcx, [rip + 0x16ecffc]
1804c4fcc call 0x18024fef0
1804c4fd1 lea rcx, [rip + 0x16d6908]
1804c4fd8 call 0x18024fef0
1804c4fdd lea rcx, [rip + 0x16fbdbc]
1804c4fe4 call 0x18024fef0
1804c4fe9 lea rcx, [rip + 0x1702a10]
1804c4ff0 call 0x18024fef0
1804c4ff5 mov byte ptr [rip + 0x1820069], 1
1804c4ffc mov rcx, qword ptr [rip + 0x16f8b95]
1804c5003 movaps xmmword ptr [rsp + 0x40], xmm6
1804c5008 call 0x180250100
1804c500d mov rdx, qword ptr [rip + 0x16e6cac]
1804c5014 mov rcx, rax
1804c5017 mov rsi, rax
1804c501a call 0x1808053d0
1804c501f test rbx, rbx
1804c5022 je 0x1804c5243
1804c5028 xor edx, edx
1804c502a mov rcx, rbx
1804c502d call 0x181304510
1804c5032 xor r9d, r9d
1804c5035 mov r8, rsi
1804c5038 mov rdx, rax
1804c503b mov rcx, rbx
1804c503e call 0x18030da70
1804c5043 cmp ebp, 0x1e
1804c5046 jne 0x1804c5229
1804c504c test rdi, rdi
1804c504f je 0x1804c5229
1804c5055 mov rdx, qword ptr [rip + 0x16ecf6c]
1804c505c mov r8, qword ptr [rdi]
1804c505f movzx eax, byte ptr [rdx + 0x130]
1804c5066 cmp byte ptr [r8 + 0x130], al
1804c506d jb 0x1804c5229
1804c5073 movzx ecx, al
1804c5076 mov rax, qword ptr [r8 + 0xc8]
1804c507d cmp qword ptr [rax + rcx*8 - 8], rdx
1804c5082 jne 0x1804c5229
1804c5088 movzx ecx, byte ptr [rdx + 0x130]
1804c508f cmp qword ptr [rax + rcx*8 - 8], rdx
1804c5094 mov rcx, qword ptr [rip + 0x16dcf7d]
1804c509b sete al
1804c509e xor ebp, ebp
1804c50a0 test al, al
1804c50a2 mov ebx, ebp
1804c50a4 cmovne rbx, rdi
1804c50a8 cmp dword ptr [rcx + 0xe0], ebp
1804c50ae jne 0x1804c50b5
1804c50b0 call 0x1802501e0
1804c50b5 mov rdx, qword ptr [rip + 0x16d6824]
1804c50bc xor r8d, r8d
1804c50bf mov rcx, rsi
1804c50c2 call 0x18031b790
1804c50c7 mov rdi, rax
1804c50ca test rbx, rbx
1804c50cd je 0x1804c5243
1804c50d3 mov rcx, qword ptr [rbx + 0x1e8]
1804c50da test rcx, rcx
1804c50dd je 0x1804c5243
1804c50e3 mov rdx, qword ptr [rcx + 0x28]
1804c50e7 test rdx, rdx
1804c50ea je 0x1804c5243
1804c50f0 mov r9d, dword ptr [rbx + 0x24]
1804c50f4 lea rcx, [rsp + 0x30]
1804c50f9 mov r8d, dword ptr [rbx + 0x20]
1804c50fd inc r9d
1804c5100 mov qword ptr [rsp + 0x20], rbp
1804c5105 call 0x18030ec90
1804c510a movss xmm6, dword ptr [rax + 4]
1804c510f test rdi, rdi
1804c5112 je 0x1804c5243
1804c5118 mov rdx, qword ptr [rip + 0x16e0099]
1804c511f mov rcx, rdi
1804c5122 call 0x18046dc80
1804c5127 test rax, rax
1804c512a je 0x1804c5243
1804c5130 mov rdx, qword ptr [rip + 0x16fbc69]
1804c5137 xor r8d, r8d
1804c513a mov rcx, rax
1804c513d call 0x1812fb400
1804c5142 mov rdx, qword ptr [rip + 0x16e006f]
1804c5149 mov rcx, rdi
1804c514c call 0x18046dc80
1804c5151 test rax, rax
1804c5154 je 0x1804c5243
1804c515a addss xmm6, dword ptr [rip + 0x10e28ae]
1804c5162 xor r8d, r8d
1804c5165 mov rcx, rax
1804c5168 mulss xmm6, dword ptr [rip + 0x10e2b5c]
1804c5170 cvttss2si edx, xmm6
1804c5174 call 0x1812fb450
1804c5179 mov rdx, qword ptr [rip + 0x1702880]
1804c5180 xor r8d, r8d
1804c5183 mov rcx, rsi
1804c5186 call 0x18031b790
1804c518b mov rdx, qword ptr [rbx + 0x1e8]
1804c5192 mov rdi, rax
1804c5195 test rdx, rdx
1804c5198 je 0x1804c5243
1804c519e mov rdx, qword ptr [rdx + 0x28]
1804c51a2 test rdx, rdx
1804c51a5 je 0x1804c5243
1804c51ab mov r9d, dword ptr [rbx + 0x24]
1804c51af lea rcx, [rsp + 0x30]
1804c51b4 mov r8d, dword ptr [rbx + 0x20]
1804c51b8 sub r9d, 2
1804c51bc mov qword ptr [rsp + 0x20], rbp
1804c51c1 call 0x18030ec90
1804c51c6 movss xmm6, dword ptr [rax + 4]
1804c51cb test rdi, rdi
1804c51ce je 0x1804c5243
1804c51d0 mov rdx, qword ptr [rip + 0x16dffe1]
1804c51d7 mov rcx, rdi
1804c51da call 0x18046dc80
1804c51df test rax, rax
1804c51e2 je 0x1804c5243
1804c51e4 mov rdx, qword ptr [rip + 0x16fbbb5]
1804c51eb xor r8d, r8d
1804c51ee mov rcx, rax
1804c51f1 call 0x1812fb400
1804c51f6 mov rdx, qword ptr [rip + 0x16dffbb]
1804c51fd mov rcx, rdi
1804c5200 call 0x18046dc80
1804c5205 test rax, rax
1804c5208 je 0x1804c5243
1804c520a subss xmm6, dword ptr [rip + 0x10e27fe]
1804c5212 xor r8d, r8d
1804c5215 mov rcx, rax
1804c5218 mulss xmm6, dword ptr [rip + 0x10e2aac]
1804c5220 cvttss2si edx, xmm6
1804c5224 call 0x1812fb450
1804c5229 movaps xmm6, xmmword ptr [rsp + 0x40]
1804c522e mov rbx, qword ptr [rsp + 0x60]
1804c5233 mov rbp, qword ptr [rsp + 0x68]
1804c5238 mov rsi, qword ptr [rsp + 0x70]
1804c523d add rsp, 0x50
1804c5241 pop rdi
1804c5242 ret
1804c5243 call 0x180250150
1804c5248 int3
