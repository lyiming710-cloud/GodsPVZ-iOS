1809d52c0 mov qword ptr [rsp + 8], rbx
1809d52c5 push rdi
1809d52c6 sub rsp, 0x20
1809d52ca mov rdi, r8
1809d52cd mov rbx, rcx
1809d52d0 cmp edx, dword ptr [rcx + 0x18]
1809d52d3 jae 0x1809d535e
1809d52d9 mov rax, qword ptr [rcx + 0x10]
1809d52dd test rax, rax
1809d52e0 je 0x1809d5391
1809d52e6 movsxd rcx, dword ptr [rcx + 0x18]
1809d52ea dec rcx
1809d52ed cmp ecx, dword ptr [rax + 0x18]
1809d52f0 jae 0x1809d538b
1809d52f6 mov rdi, qword ptr [rax + rcx*8 + 0x20]
1809d52fb movsxd rcx, edx
1809d52fe cmp edx, dword ptr [rax + 0x18]
1809d5301 jae 0x1809d538b
1809d5307 mov qword ptr [rax + rcx*8 + 0x20], rdi
1809d530c mov rdx, rdi
1809d530f add rax, 0x20
1809d5313 lea rcx, [rax + rcx*8]
1809d5317 call 0x18024f360
1809d531c movsxd rax, dword ptr [rbx + 0x18]
1809d5320 mov rdx, qword ptr [rbx + 0x10]
1809d5324 lea ecx, [rax - 1]
1809d5327 mov dword ptr [rbx + 0x18], ecx
1809d532a test rdx, rdx
1809d532d je 0x1809d5391
1809d532f lea rcx, [rax - 1]
1809d5333 cmp ecx, dword ptr [rdx + 0x18]
1809d5336 jae 0x1809d538b
1809d5338 mov qword ptr [rdx + rcx*8 + 0x20], 0
1809d5341 add rdx, 0x20
1809d5345 lea rcx, [rdx + rcx*8]
1809d5349 xor edx, edx
1809d534b call 0x18024f360
1809d5350 mov rbx, qword ptr [rsp + 0x30]
1809d5355 mov rax, rdi
1809d5358 add rsp, 0x20
1809d535c pop rdi
1809d535d ret
1809d535e lea rcx, [rip + 0x11d3a5b]
1809d5365 call 0x18024ff10
1809d536a mov rcx, rax
1809d536d call 0x180250100
1809d5372 xor edx, edx
1809d5374 mov rcx, rax
1809d5377 mov rbx, rax
1809d537a call 0x180ca20a0
1809d537f mov rdx, rdi
1809d5382 mov rcx, rbx
1809d5385 call 0x180250110
1809d538a int3
1809d538b call 0x180250140
1809d5390 int3
1809d5391 call 0x180250150
1809d5396 int3
