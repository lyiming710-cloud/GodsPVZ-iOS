0000000180317B50 push      rbp
0000000180317B52 sub       rsp, 0x20
0000000180317B56 cmp       byte ptr [rip + 0x19cd059], 0 # 0x181ce4bb6
0000000180317B5D mov       rbp, rcx
0000000180317B60 jne       0x180317bf1
0000000180317B66 lea       rcx, [rip + 0x1888eeb] # 0x181ba0a58
0000000180317B6D call      0x18024fef0
0000000180317B72 lea       rcx, [rip + 0x189395f] # 0x181bab4d8
0000000180317B79 call      0x18024fef0
0000000180317B7E lea       rcx, [rip + 0x18ab3d3] # 0x181bc2f58
0000000180317B85 call      0x18024fef0
0000000180317B8A lea       rcx, [rip + 0x1892607] # 0x181baa198
0000000180317B91 call      0x18024fef0
0000000180317B96 lea       rcx, [rip + 0x18ab363] # 0x181bc2f00
0000000180317B9D call      0x18024fef0
0000000180317BA2 lea       rcx, [rip + 0x18ab407] # 0x181bc2fb0
0000000180317BA9 call      0x18024fef0
0000000180317BAE lea       rcx, [rip + 0x189397b] # 0x181bab530
0000000180317BB5 call      0x18024fef0
0000000180317BBA lea       rcx, [rip + 0x18939c7] # 0x181bab588
0000000180317BC1 call      0x18024fef0
0000000180317BC6 lea       rcx, [rip + 0x18aa1ab] # 0x181bc1d78
0000000180317BCD call      0x18024fef0
0000000180317BD2 lea       rcx, [rip + 0x18a5a97] # 0x181bbd670
0000000180317BD9 call      0x18024fef0
0000000180317BDE lea       rcx, [rip + 0x18abf93] # 0x181bc3b78
0000000180317BE5 call      0x18024fef0
0000000180317BEA mov       byte ptr [rip + 0x19ccfc5], 1 # 0x181ce4bb6
0000000180317BF1 mov       rcx, qword ptr [rbp + 0x20]
0000000180317BF5 mov       qword ptr [rsp + 0x30], rbx
0000000180317BFA mov       qword ptr [rsp + 0x38], rsi
0000000180317BFF mov       qword ptr [rsp + 0x40], rdi
0000000180317C04 mov       qword ptr [rsp + 0x48], r14
0000000180317C09 test      rcx, rcx
0000000180317C0C je        0x180317ea5
0000000180317C12 mov       eax, dword ptr [rbp + 0x48]
0000000180317C15 cmp       dword ptr [rcx + 0x18], eax
0000000180317C18 jge       0x180317cb7
0000000180317C1E mov       rcx, qword ptr [rip + 0x1888e33] # 0x181ba0a58
0000000180317C25 call      0x180250100
0000000180317C2A xor       edx, edx
0000000180317C2C mov       rcx, rax
0000000180317C2F mov       rbx, rax
0000000180317C32 call      0x180302170
0000000180317C37 mov       rcx, qword ptr [rip + 0x18aa13a] # 0x181bc1d78
0000000180317C3E call      0x180250100
0000000180317C43 mov       rdx, qword ptr [rip + 0x18ab2b6] # 0x181bc2f00
0000000180317C4A mov       rcx, rax
0000000180317C4D mov       rdi, rax
0000000180317C50 call      0x1808053d0
0000000180317C55 test      rbx, rbx
0000000180317C58 je        0x180317ea5
0000000180317C5E lea       rcx, [rbx + 0x10]
0000000180317C62 mov       qword ptr [rbx + 0x10], rdi
0000000180317C66 mov       rdx, rdi
0000000180317C69 call      0x18024f360
0000000180317C6E mov       rcx, qword ptr [rbp + 0x20]
0000000180317C72 test      rcx, rcx
0000000180317C75 je        0x180317ea5
0000000180317C7B mov       eax, dword ptr [rbp + 0x48]
0000000180317C7E dec       eax
0000000180317C80 cmp       dword ptr [rcx + 0x18], eax
0000000180317C83 jne       0x180317c89
0000000180317C85 mov       byte ptr [rbx + 0x18], 1
0000000180317C89 mov       rcx, qword ptr [rbp + 0x20]
0000000180317C8D test      rcx, rcx
0000000180317C90 je        0x180317ea5
0000000180317C96 mov       r8, qword ptr [rip + 0x189383b] # 0x181bab4d8
0000000180317C9D mov       rdx, rbx
0000000180317CA0 call      0x180002e70
0000000180317CA5 mov       rcx, qword ptr [rbp + 0x20]
0000000180317CA9 test      rcx, rcx
0000000180317CAC je        0x180317ea5
0000000180317CB2 jmp       0x180317c12
0000000180317CB7 xor       r14d, r14d
0000000180317CBA cmp       eax, r14d
0000000180317CBD jle       0x180317e8b
0000000180317CC3 mov       rcx, qword ptr [rbp + 0x20]
0000000180317CC7 test      rcx, rcx
0000000180317CCA je        0x180317ea5
0000000180317CD0 mov       r8, qword ptr [rip + 0x18938b1] # 0x181bab588
0000000180317CD7 mov       edx, r14d
0000000180317CDA call      0x180825710
0000000180317CDF mov       rbx, rax
0000000180317CE2 test      rax, rax
0000000180317CE5 je        0x180317ea5
0000000180317CEB cmp       qword ptr [rax + 0x10], 0
0000000180317CF0 jne       0x180317d20
0000000180317CF2 mov       rcx, qword ptr [rip + 0x18aa07f] # 0x181bc1d78
0000000180317CF9 call      0x180250100
0000000180317CFE mov       rdx, qword ptr [rip + 0x18ab1fb] # 0x181bc2f00
0000000180317D05 mov       rcx, rax
0000000180317D08 mov       rdi, rax
0000000180317D0B call      0x1808053d0
0000000180317D10 lea       rcx, [rbx + 0x10]
0000000180317D14 mov       qword ptr [rbx + 0x10], rdi
0000000180317D18 mov       rdx, rdi
0000000180317D1B call      0x18024f360
0000000180317D20 cmp       byte ptr [rbx + 0x18], 0
0000000180317D24 jne       0x180317dd0
0000000180317D2A nop       word ptr [rax + rax]
0000000180317D30 mov       rax, qword ptr [rbx + 0x10]
0000000180317D34 test      rax, rax
0000000180317D37 je        0x180317ea5
0000000180317D3D cmp       dword ptr [rax + 0x18], 0xa
0000000180317D41 jge       0x180317e7e
0000000180317D47 mov       rcx, qword ptr [rip + 0x18abe2a] # 0x181bc3b78
0000000180317D4E call      0x180250100
0000000180317D53 xor       edx, edx
0000000180317D55 mov       rcx, rax
0000000180317D58 mov       rdi, rax
0000000180317D5B call      0x180302170
0000000180317D60 mov       rcx, qword ptr [rip + 0x18a5909] # 0x181bbd670
0000000180317D67 call      0x180250100
0000000180317D6C mov       rdx, qword ptr [rip + 0x1892425] # 0x181baa198
0000000180317D73 mov       rcx, rax
0000000180317D76 mov       rsi, rax
0000000180317D79 call      0x1808053d0
0000000180317D7E test      rdi, rdi
0000000180317D81 je        0x180317ea5
0000000180317D87 lea       rcx, [rdi + 0x10]
0000000180317D8B mov       qword ptr [rdi + 0x10], rsi
0000000180317D8F mov       rdx, rsi
0000000180317D92 call      0x18024f360
0000000180317D97 mov       rax, qword ptr [rbx + 0x10]
0000000180317D9B test      rax, rax
0000000180317D9E je        0x180317ea5
0000000180317DA4 cmp       dword ptr [rax + 0x18], 9
0000000180317DA8 jne       0x180317dae
0000000180317DAA mov       byte ptr [rdi + 0x18], 1
0000000180317DAE mov       rcx, qword ptr [rbx + 0x10]
0000000180317DB2 test      rcx, rcx
0000000180317DB5 je        0x180317ea5
0000000180317DBB mov       r8, qword ptr [rip + 0x18ab196] # 0x181bc2f58
0000000180317DC2 mov       rdx, rdi
0000000180317DC5 call      0x180002e70
0000000180317DCA jmp       0x180317d30
0000000180317DCF nop       
0000000180317DD0 mov       r8, qword ptr [rbx + 0x10]
0000000180317DD4 test      r8, r8
0000000180317DD7 je        0x180317ea5
0000000180317DDD mov       ecx, dword ptr [rbp + 0x4c]
0000000180317DE0 mov       eax, 0x66666667
0000000180317DE5 dec       ecx
0000000180317DE7 imul      ecx
0000000180317DE9 sar       edx, 2
0000000180317DEC mov       eax, edx
0000000180317DEE shr       eax, 0x1f
0000000180317DF1 add       edx, eax
0000000180317DF3 lea       eax, [rdx + rdx*4]
0000000180317DF6 add       eax, eax
0000000180317DF8 sub       ecx, eax
0000000180317DFA inc       ecx
0000000180317DFC cmp       dword ptr [r8 + 0x18], ecx
0000000180317E00 jge       0x180317e7e
0000000180317E02 mov       rcx, qword ptr [rip + 0x18abd6f] # 0x181bc3b78
0000000180317E09 call      0x180250100
0000000180317E0E xor       edx, edx
0000000180317E10 mov       rcx, rax
0000000180317E13 mov       rdi, rax
0000000180317E16 call      0x180302170
0000000180317E1B mov       rcx, qword ptr [rip + 0x18a584e] # 0x181bbd670
0000000180317E22 call      0x180250100
0000000180317E27 mov       rdx, qword ptr [rip + 0x189236a] # 0x181baa198
0000000180317E2E mov       rcx, rax
0000000180317E31 mov       rsi, rax
0000000180317E34 call      0x1808053d0
0000000180317E39 test      rdi, rdi
0000000180317E3C je        0x180317ea5
0000000180317E3E lea       rcx, [rdi + 0x10]
0000000180317E42 mov       qword ptr [rdi + 0x10], rsi
0000000180317E46 mov       rdx, rsi
0000000180317E49 call      0x18024f360
0000000180317E4E mov       rax, qword ptr [rbx + 0x10]
0000000180317E52 test      rax, rax
0000000180317E55 je        0x180317ea5
0000000180317E57 cmp       dword ptr [rax + 0x18], 9
0000000180317E5B jne       0x180317e61
0000000180317E5D mov       byte ptr [rdi + 0x18], 1
0000000180317E61 mov       rcx, qword ptr [rbx + 0x10]
0000000180317E65 test      rcx, rcx
0000000180317E68 je        0x180317ea5
0000000180317E6A mov       r8, qword ptr [rip + 0x18ab0e7] # 0x181bc2f58
0000000180317E71 mov       rdx, rdi
0000000180317E74 call      0x180002e70
0000000180317E79 jmp       0x180317dd0
0000000180317E7E inc       r14d
0000000180317E81 cmp       r14d, dword ptr [rbp + 0x48]
0000000180317E85 jl        0x180317cc3
0000000180317E8B mov       r14, qword ptr [rsp + 0x48]
0000000180317E90 mov       rdi, qword ptr [rsp + 0x40]
0000000180317E95 mov       rsi, qword ptr [rsp + 0x38]
0000000180317E9A mov       rbx, qword ptr [rsp + 0x30]
0000000180317E9F add       rsp, 0x20
0000000180317EA3 pop       rbp
0000000180317EA4 ret       
0000000180317EA5 call      0x180250150
0000000180317EAA int3      
0000000180317EAB int3      
0000000180317EAC int3      
0000000180317EAD int3      
0000000180317EAE int3      
0000000180317EAF int3      