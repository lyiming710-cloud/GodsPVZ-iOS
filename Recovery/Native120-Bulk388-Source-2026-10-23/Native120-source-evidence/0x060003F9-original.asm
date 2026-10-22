000000018037C5E0 mov       qword ptr [rsp + 8], rbx
000000018037C5E5 push      rdi
000000018037C5E6 sub       rsp, 0x20
000000018037C5EA cmp       byte ptr [rip + 0x19687e5], 0 # 0x181ce4dd6
000000018037C5F1 mov       rbx, rcx
000000018037C5F4 jne       0x18037c621
000000018037C5F6 lea       rcx, [rip + 0x182795b] # 0x181ba3f58
000000018037C5FD call      0x18024fef0
000000018037C602 lea       rcx, [rip + 0x1833ef7] # 0x181bb0500
000000018037C609 call      0x18024fef0
000000018037C60E lea       rcx, [rip + 0x184478b] # 0x181bc0da0
000000018037C615 call      0x18024fef0
000000018037C61A mov       byte ptr [rip + 0x19687b5], 1 # 0x181ce4dd6
000000018037C621 mov       rcx, qword ptr [rip + 0x1833ed8] # 0x181bb0500
000000018037C628 mov       rdi, qword ptr [rbx + 0xf8]
000000018037C62F cmp       dword ptr [rcx + 0xe0], 0
000000018037C636 jne       0x18037c63d
000000018037C638 call      0x1802501e0
000000018037C63D xor       r8d, r8d
000000018037C640 xor       edx, edx
000000018037C642 mov       rcx, rdi
000000018037C645 call      0x18131f760
000000018037C64A test      al, al
000000018037C64C je        0x18037c682
000000018037C64E xor       edx, edx
000000018037C650 mov       rcx, rbx
000000018037C653 call      0x1813044d0
000000018037C658 test      rax, rax
000000018037C65B je        0x18037c6d3
000000018037C65D mov       rdx, qword ptr [rip + 0x18278f4] # 0x181ba3f58
000000018037C664 mov       rcx, rax
000000018037C667 call      0x18046d280
000000018037C66C lea       rcx, [rbx + 0xf8]
000000018037C673 mov       qword ptr [rbx + 0xf8], rax
000000018037C67A mov       rdx, rax
000000018037C67D call      0x18024f360
000000018037C682 mov       rcx, qword ptr [rbx + 0xf8]
000000018037C689 test      rcx, rcx
000000018037C68C je        0x18037c6d3
000000018037C68E mov       rdx, qword ptr [rip + 0x184470b] # 0x181bc0da0
000000018037C695 xor       r8d, r8d
000000018037C698 call      0x181348060
000000018037C69D mov       rcx, qword ptr [rbx + 0xf8]
000000018037C6A4 test      rcx, rcx
000000018037C6A7 je        0x18037c6d3
000000018037C6A9 movss     xmm0, dword ptr [rbx + 0x48]
000000018037C6AE mov       edx, 0xfffffff6
000000018037C6B3 mulss     xmm0, dword ptr [rip + 0x122b599] # 0x1815a7c54
000000018037C6BB xor       r8d, r8d
000000018037C6BE cvttss2si eax, xmm0
000000018037C6C2 sub       edx, eax
000000018037C6C4 mov       rbx, qword ptr [rsp + 0x30]
000000018037C6C9 add       rsp, 0x20
000000018037C6CD pop       rdi
000000018037C6CE jmp       0x1813480b0
000000018037C6D3 call      0x180250150
000000018037C6D8 int3      
000000018037C6D9 int3      
000000018037C6DA int3      
000000018037C6DB int3      
000000018037C6DC int3      
000000018037C6DD int3      
000000018037C6DE int3      
000000018037C6DF int3      