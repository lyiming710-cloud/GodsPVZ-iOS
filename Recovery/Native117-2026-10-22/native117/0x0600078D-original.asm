00000001803C0FA0 mov       qword ptr [rsp + 8], rbx
00000001803C0FA5 push      rdi
00000001803C0FA6 sub       rsp, 0x20
00000001803C0FAA cmp       byte ptr [rip + 0x1923fc8], 0 # 0x181ce4f79
00000001803C0FB1 mov       rdi, rcx
00000001803C0FB4 jne       0x1803c0fc9
00000001803C0FB6 lea       rcx, [rip + 0x1802a03] # 0x181bc39c0
00000001803C0FBD call      0x18024fef0
00000001803C0FC2 mov       byte ptr [rip + 0x1923fb0], 1 # 0x181ce4f79
00000001803C0FC9 mov       ecx, dword ptr [rdi + 0x10]
00000001803C0FCC mov       rbx, qword ptr [rdi + 0x28]
00000001803C0FD0 test      ecx, ecx
00000001803C0FD2 je        0x1803c1007
00000001803C0FD4 sub       ecx, 1
00000001803C0FD7 je        0x1803c0ff4
00000001803C0FD9 cmp       ecx, 1
00000001803C0FDC je        0x1803c0feb
00000001803C0FDE xor       al, al
00000001803C0FE0 mov       rbx, qword ptr [rsp + 0x30]
00000001803C0FE5 add       rsp, 0x20
00000001803C0FE9 pop       rdi
00000001803C0FEA ret       
00000001803C0FEB mov       dword ptr [rdi + 0x10], 0xffffffff
00000001803C0FF2 jmp       0x1803c106b
00000001803C0FF4 mov       dword ptr [rdi + 0x10], 0xffffffff
00000001803C0FFB mov       dword ptr [rdi + 0x3c], 0
00000001803C1002 jmp       0x1803c1099
00000001803C1007 mov       rcx, qword ptr [rdi + 0x20]
00000001803C100B mov       dword ptr [rdi + 0x10], 0xffffffff
00000001803C1012 test      rcx, rcx
00000001803C1015 je        0x1803c111d
00000001803C101B mov       rax, qword ptr [rcx]
00000001803C101E xor       r8d, r8d
00000001803C1021 xor       edx, edx
00000001803C1023 mov       r9, qword ptr [rax + 0x7e0]
00000001803C102A call      qword ptr [rax + 0x7d8]
00000001803C1030 mov       rdx, qword ptr [rdi + 0x20]
00000001803C1034 test      rdx, rdx
00000001803C1037 je        0x1803c111d
00000001803C103D mov       rdx, qword ptr [rdx + 0x370]
00000001803C1044 lea       rcx, [rdi + 0x30]
00000001803C1048 mov       qword ptr [rdi + 0x30], rdx
00000001803C104C call      0x18024f360
00000001803C1051 mov       rax, qword ptr [rdi + 0x30]
00000001803C1055 test      rax, rax
00000001803C1058 je        0x1803c111d
00000001803C105E mov       eax, dword ptr [rax + 0x18]
00000001803C1061 mov       dword ptr [rdi + 0x38], eax
00000001803C1064 mov       dword ptr [rdi + 0x3c], 0
00000001803C106B test      rbx, rbx
00000001803C106E je        0x1803c111d
00000001803C1074 cmp       byte ptr [rbx + 0x28], 0
00000001803C1078 je        0x1803c1091
00000001803C107A mov       rax, qword ptr [rdi + 0x30]
00000001803C107E test      rax, rax
00000001803C1081 je        0x1803c111d
00000001803C1087 mov       eax, dword ptr [rax + 0x18]
00000001803C108A mov       dword ptr [rdi + 0x38], eax
00000001803C108D mov       byte ptr [rbx + 0x28], 0
00000001803C1091 mov       eax, dword ptr [rdi + 0x38]
00000001803C1094 cmp       dword ptr [rdi + 0x3c], eax
00000001803C1097 jg        0x1803c10d7
00000001803C1099 mov       rcx, qword ptr [rdi + 0x20]
00000001803C109D test      rcx, rcx
00000001803C10A0 je        0x1803c111d
00000001803C10A2 mov       edx, dword ptr [rdi + 0x3c]
00000001803C10A5 xor       r8d, r8d
00000001803C10A8 call      0x1811b9b50
00000001803C10AD inc       dword ptr [rdi + 0x3c]
00000001803C10B0 lea       rcx, [rdi + 0x18]
00000001803C10B4 xor       edx, edx
00000001803C10B6 mov       qword ptr [rdi + 0x18], 0
00000001803C10BE call      0x18024f360
00000001803C10C3 mov       al, 1
00000001803C10C5 mov       dword ptr [rdi + 0x10], 2
00000001803C10CC mov       rbx, qword ptr [rsp + 0x30]
00000001803C10D1 add       rsp, 0x20
00000001803C10D5 pop       rdi
00000001803C10D6 ret       
00000001803C10D7 mov       rcx, qword ptr [rip + 0x18028e2] # 0x181bc39c0
00000001803C10DE call      0x180250100
00000001803C10E3 movss     xmm1, dword ptr [rip + 0x11e6925] # 0x1815a7a10
00000001803C10EB xor       r8d, r8d
00000001803C10EE mov       rcx, rax
00000001803C10F1 mov       rbx, rax
00000001803C10F4 call      0x181333710
00000001803C10F9 lea       rcx, [rdi + 0x18]
00000001803C10FD mov       qword ptr [rdi + 0x18], rbx
00000001803C1101 mov       rdx, rbx
00000001803C1104 call      0x18024f360
00000001803C1109 mov       rbx, qword ptr [rsp + 0x30]
00000001803C110E mov       al, 1
00000001803C1110 mov       dword ptr [rdi + 0x10], 1
00000001803C1117 add       rsp, 0x20
00000001803C111B pop       rdi
00000001803C111C ret       
00000001803C111D call      0x180250150
00000001803C1122 int3      
00000001803C1123 int3      
00000001803C1124 int3      
00000001803C1125 int3      
00000001803C1126 int3      
00000001803C1127 int3      
00000001803C1128 int3      
00000001803C1129 int3      
00000001803C112A int3      
00000001803C112B int3      
00000001803C112C int3      
00000001803C112D int3      
00000001803C112E int3      
00000001803C112F int3      