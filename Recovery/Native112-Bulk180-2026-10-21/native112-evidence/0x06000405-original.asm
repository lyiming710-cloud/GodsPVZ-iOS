000000018037E7E0 push      rbx
000000018037E7E2 sub       rsp, 0x40
000000018037E7E6 cmp       byte ptr [rip + 0x19665ee], 0 # 0x181ce4ddb
000000018037E7ED mov       rbx, rcx
000000018037E7F0 jne       0x18037e81d
000000018037E7F2 lea       rcx, [rip + 0x182381f] # 0x181ba2018
000000018037E7F9 call      0x18024fef0
000000018037E7FE lea       rcx, [rip + 0x18393a3] # 0x181bb7ba8
000000018037E805 call      0x18024fef0
000000018037E80A lea       rcx, [rip + 0x1831cef] # 0x181bb0500
000000018037E811 call      0x18024fef0
000000018037E816 mov       byte ptr [rip + 0x19665be], 1 # 0x181ce4ddb
000000018037E81D mov       rax, qword ptr [rbx + 0x20]
000000018037E821 mov       qword ptr [rsp + 0x58], rdi
000000018037E826 test      rax, rax
000000018037E829 je        0x18037ea36
000000018037E82F mov       rcx, qword ptr [rax + 0x110]
000000018037E836 test      rcx, rcx
000000018037E839 je        0x18037ea36
000000018037E83F cmp       dword ptr [rcx + 0x40], 0
000000018037E843 jne       0x18037ea2b
000000018037E849 cmp       byte ptr [rax + 0x42], 0
000000018037E84D jne       0x18037ea2b
000000018037E853 mov       ecx, dword ptr [rbx + 0x28]
000000018037E856 test      ecx, ecx
000000018037E858 je        0x18037ea2b
000000018037E85E sub       ecx, 1
000000018037E861 jne       0x18037ea2b
000000018037E867 cmp       byte ptr [rax + 0xd0], cl
000000018037E86D je        0x18037e881
000000018037E86F mov       r8b, 1
000000018037E872 xor       r9d, r9d
000000018037E875 movzx     edx, r8b
000000018037E879 mov       rcx, rax
000000018037E87C call      0x1803265e0
000000018037E881 mov       rax, qword ptr [rbx + 0x20]
000000018037E885 test      rax, rax
000000018037E888 je        0x18037ea36
000000018037E88E mov       rcx, qword ptr [rax + 0x110]
000000018037E895 test      rcx, rcx
000000018037E898 je        0x18037ea36
000000018037E89E mov       eax, dword ptr [rbx + 0x2c]
000000018037E8A1 mov       dword ptr [rcx + 0x40], eax
000000018037E8A4 mov       rcx, qword ptr [rbx + 0x20]
000000018037E8A8 test      rcx, rcx
000000018037E8AB je        0x18037ea36
000000018037E8B1 mov       rcx, qword ptr [rcx + 0x110]
000000018037E8B8 test      rcx, rcx
000000018037E8BB je        0x18037ea36
000000018037E8C1 mov       qword ptr [rcx + 0xb0], rbx
000000018037E8C8 mov       rdx, rbx
000000018037E8CB add       rcx, 0xb0
000000018037E8D2 call      0x18024f360
000000018037E8D7 mov       rdi, qword ptr [rbx + 0x98]
000000018037E8DE xor       ecx, ecx
000000018037E8E0 mov       dword ptr [rbx + 0x28], 2
000000018037E8E7 call      0x1812e27c0
000000018037E8EC test      rax, rax
000000018037E8EF je        0x18037ea36
000000018037E8F5 xor       edx, edx
000000018037E8F7 mov       rcx, rax
000000018037E8FA call      0x181304510
000000018037E8FF test      rax, rax
000000018037E902 je        0x18037ea36
000000018037E908 mov       qword ptr [rsp + 0x50], rsi
000000018037E90D lea       rcx, [rsp + 0x20]
000000018037E912 xor       r8d, r8d
000000018037E915 movaps    xmmword ptr [rsp + 0x30], xmm6
000000018037E91A mov       rdx, rax
000000018037E91D call      0x18132f3a0
000000018037E922 mov       rcx, qword ptr [rip + 0x18236ef] # 0x181ba2018
000000018037E929 movsd     xmm6, qword ptr [rax]
000000018037E92D cmp       dword ptr [rcx + 0xe0], 0
000000018037E934 mov       esi, dword ptr [rax + 8]
000000018037E937 jne       0x18037e93e
000000018037E939 call      0x1802501e0
000000018037E93E xor       ecx, ecx
000000018037E940 call      0x18031aea0
000000018037E945 xor       r9d, r9d
000000018037E948 movsd     qword ptr [rsp + 0x20], xmm6
000000018037E94E movaps    xmm2, xmm0
000000018037E951 mov       dword ptr [rsp + 0x28], esi
000000018037E955 lea       rdx, [rsp + 0x20]
000000018037E95A mov       rcx, rdi
000000018037E95D call      0x18031b230
000000018037E962 mov       rcx, qword ptr [rbx + 0x88]
000000018037E969 movaps    xmm6, xmmword ptr [rsp + 0x30]
000000018037E96E mov       rsi, qword ptr [rsp + 0x50]
000000018037E973 test      rcx, rcx
000000018037E976 je        0x18037ea36
000000018037E97C xor       edx, edx
000000018037E97E call      0x1813044d0
000000018037E983 mov       rcx, qword ptr [rip + 0x1831b76] # 0x181bb0500
000000018037E98A mov       rdi, rax
000000018037E98D cmp       dword ptr [rcx + 0xe0], 0
000000018037E994 jne       0x18037e99b
000000018037E996 call      0x1802501e0
000000018037E99B mov       rdx, qword ptr [rip + 0x1839206] # 0x181bb7ba8
000000018037E9A2 mov       rcx, rdi
000000018037E9A5 call      0x1804a0690
000000018037E9AA lea       rcx, [rbx + 0x90]
000000018037E9B1 mov       qword ptr [rbx + 0x90], rax
000000018037E9B8 mov       rdx, rax
000000018037E9BB call      0x18024f360
000000018037E9C0 mov       rcx, qword ptr [rbx + 0x90]
000000018037E9C7 test      rcx, rcx
000000018037E9CA je        0x18037ea36
000000018037E9CC xor       edx, edx
000000018037E9CE call      0x18131c070
000000018037E9D3 mov       rcx, qword ptr [rbx + 0x20]
000000018037E9D7 mov       rdi, rax
000000018037E9DA test      rcx, rcx
000000018037E9DD je        0x18037ea36
000000018037E9DF mov       rcx, qword ptr [rcx + 0x80]
000000018037E9E6 test      rcx, rcx
000000018037E9E9 je        0x18037ea36
000000018037E9EB xor       edx, edx
000000018037E9ED call      0x18131c070
000000018037E9F2 test      rdi, rdi
000000018037E9F5 je        0x18037ea36
000000018037E9F7 xor       r8d, r8d
000000018037E9FA mov       rdx, rax
000000018037E9FD mov       rcx, rdi
000000018037EA00 call      0x18132e8b0
000000018037EA05 xor       edx, edx
000000018037EA07 mov       rcx, rbx
000000018037EA0A call      0x18037e680
000000018037EA0F mov       rcx, qword ptr [rbx + 0x20]
000000018037EA13 test      rcx, rcx
000000018037EA16 je        0x18037ea36
000000018037EA18 mov       rcx, qword ptr [rcx + 0x110]
000000018037EA1F test      rcx, rcx
000000018037EA22 je        0x18037ea36
000000018037EA24 xor       edx, edx
000000018037EA26 call      0x1803213e0
000000018037EA2B mov       rdi, qword ptr [rsp + 0x58]
000000018037EA30 add       rsp, 0x40
000000018037EA34 pop       rbx
000000018037EA35 ret       
000000018037EA36 call      0x180250150
000000018037EA3B int3      