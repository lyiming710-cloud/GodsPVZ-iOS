0000000180308880 mov       qword ptr [rsp + 8], rbx
0000000180308885 mov       dword ptr [rsp + 0x10], edx
0000000180308889 push      rdi
000000018030888A sub       rsp, 0x20
000000018030888E cmp       byte ptr [rip + 0x19dc24c], 0 # 0x181ce4ae1
0000000180308895 mov       rbx, rcx
0000000180308898 jne       0x1803088b1
000000018030889A lea       rcx, [rip + 0x18b2457] # 0x181bbacf8
00000001803088A1 call      0x18024fef0
00000001803088A6 mov       edx, dword ptr [rsp + 0x38]
00000001803088AA mov       byte ptr [rip + 0x19dc230], 1 # 0x181ce4ae1
00000001803088B1 mov       rdi, qword ptr [rbx + 0xd8]
00000001803088B8 lea       rcx, [rsp + 0x38]
00000001803088BD mov       dword ptr [rbx + 0xd0], edx
00000001803088C3 xor       edx, edx
00000001803088C5 call      0x180ca3820
00000001803088CA test      rdi, rdi
00000001803088CD je        0x180308bbb
00000001803088D3 xor       r8d, r8d
00000001803088D6 mov       rdx, rax
00000001803088D9 mov       rcx, rdi
00000001803088DC call      0x181197d70
00000001803088E1 movsxd    r8, dword ptr [rsp + 0x38]
00000001803088E6 cmp       r8d, 4
00000001803088EA ja        0x180308ab8
00000001803088F0 cmp       qword ptr [rbx + 0x48], 0
00000001803088F5 je        0x180308ab8
00000001803088FB mov       rax, qword ptr [rbx + 0x48]
00000001803088FF mov       rdi, qword ptr [rbx + 0xe0]
0000000180308906 mov       rdx, qword ptr [rax + 0x28]
000000018030890A test      rdx, rdx
000000018030890D je        0x180308bbb
0000000180308913 mov       rax, r8
0000000180308916 cmp       r8d, dword ptr [rdx + 0x18]
000000018030891A jae       0x180308bc1
0000000180308920 add       rax, 8
0000000180308924 lea       rcx, [rdx + rax*4]
0000000180308928 xor       edx, edx
000000018030892A call      0x180ca3820
000000018030892F test      rdi, rdi
0000000180308932 je        0x180308bbb
0000000180308938 xor       r8d, r8d
000000018030893B mov       rdx, rax
000000018030893E mov       rcx, rdi
0000000180308941 call      0x181197d70
0000000180308946 mov       rax, qword ptr [rbx + 0x48]
000000018030894A mov       rdi, qword ptr [rbx + 0xe8]
0000000180308951 test      rax, rax
0000000180308954 je        0x180308bbb
000000018030895A mov       rax, qword ptr [rax + 0x30]
000000018030895E test      rax, rax
0000000180308961 je        0x180308bbb
0000000180308967 movsxd    rdx, dword ptr [rsp + 0x38]
000000018030896C cmp       edx, dword ptr [rax + 0x18]
000000018030896F jae       0x180308bc1
0000000180308975 add       rdx, 8
0000000180308979 lea       rcx, [rax + rdx*4]
000000018030897D xor       edx, edx
000000018030897F call      0x180cc5b50
0000000180308984 test      rdi, rdi
0000000180308987 je        0x180308bbb
000000018030898D xor       r8d, r8d
0000000180308990 mov       rdx, rax
0000000180308993 mov       rcx, rdi
0000000180308996 call      0x181197d70
000000018030899B mov       rax, qword ptr [rbx + 0x48]
000000018030899F mov       rdi, qword ptr [rbx + 0xf0]
00000001803089A6 test      rax, rax
00000001803089A9 je        0x180308bbb
00000001803089AF mov       rax, qword ptr [rax + 0x70]
00000001803089B3 test      rax, rax
00000001803089B6 je        0x180308bbb
00000001803089BC movsxd    rdx, dword ptr [rsp + 0x38]
00000001803089C1 cmp       edx, dword ptr [rax + 0x18]
00000001803089C4 jae       0x180308bc1
00000001803089CA add       rdx, 8
00000001803089CE lea       rcx, [rax + rdx*4]
00000001803089D2 xor       edx, edx
00000001803089D4 call      0x180cc5b50
00000001803089D9 test      rdi, rdi
00000001803089DC je        0x180308bbb
00000001803089E2 xor       r8d, r8d
00000001803089E5 mov       rdx, rax
00000001803089E8 mov       rcx, rdi
00000001803089EB call      0x181197d70
00000001803089F0 mov       rax, qword ptr [rbx + 0x48]
00000001803089F4 mov       rdi, qword ptr [rbx + 0xf8]
00000001803089FB test      rax, rax
00000001803089FE je        0x180308bbb
0000000180308A04 mov       rax, qword ptr [rax + 0x58]
0000000180308A08 test      rax, rax
0000000180308A0B je        0x180308bbb
0000000180308A11 movsxd    rdx, dword ptr [rsp + 0x38]
0000000180308A16 cmp       edx, dword ptr [rax + 0x18]
0000000180308A19 jae       0x180308bc1
0000000180308A1F add       rdx, 8
0000000180308A23 lea       rcx, [rax + rdx*4]
0000000180308A27 xor       edx, edx
0000000180308A29 call      0x180cc5b50
0000000180308A2E test      rdi, rdi
0000000180308A31 je        0x180308bbb
0000000180308A37 xor       r8d, r8d
0000000180308A3A mov       rdx, rax
0000000180308A3D mov       rcx, rdi
0000000180308A40 call      0x181197d70
0000000180308A45 mov       rcx, qword ptr [rbx + 0xe0]
0000000180308A4C test      rcx, rcx
0000000180308A4F je        0x180308bbb
0000000180308A55 xor       r8d, r8d
0000000180308A58 mov       dl, 1
0000000180308A5A call      0x18156cc70
0000000180308A5F mov       rcx, qword ptr [rbx + 0xe8]
0000000180308A66 test      rcx, rcx
0000000180308A69 je        0x180308bbb
0000000180308A6F xor       r8d, r8d
0000000180308A72 mov       dl, 1
0000000180308A74 call      0x18156cc70
0000000180308A79 mov       rcx, qword ptr [rbx + 0xf0]
0000000180308A80 test      rcx, rcx
0000000180308A83 je        0x180308bbb
0000000180308A89 xor       r8d, r8d
0000000180308A8C mov       dl, 1
0000000180308A8E call      0x18156cc70
0000000180308A93 mov       rcx, qword ptr [rbx + 0xf8]
0000000180308A9A test      rcx, rcx
0000000180308A9D je        0x180308bbb
0000000180308AA3 xor       r8d, r8d
0000000180308AA6 mov       dl, 1
0000000180308AA8 call      0x18156cc70
0000000180308AAD mov       rbx, qword ptr [rsp + 0x30]
0000000180308AB2 add       rsp, 0x20
0000000180308AB6 pop       rdi
0000000180308AB7 ret       
0000000180308AB8 mov       rcx, qword ptr [rbx + 0xe0]
0000000180308ABF test      rcx, rcx
0000000180308AC2 je        0x180308bbb
0000000180308AC8 mov       rax, qword ptr [rip + 0x18b2229] # 0x181bbacf8
0000000180308ACF xor       r8d, r8d
0000000180308AD2 mov       rdx, qword ptr [rax + 0xb8]
0000000180308AD9 mov       rdx, qword ptr [rdx]
0000000180308ADC call      0x181197d70
0000000180308AE1 mov       rcx, qword ptr [rbx + 0xe8]
0000000180308AE8 test      rcx, rcx
0000000180308AEB je        0x180308bbb
0000000180308AF1 mov       rax, qword ptr [rip + 0x18b2200] # 0x181bbacf8
0000000180308AF8 xor       r8d, r8d
0000000180308AFB mov       rdx, qword ptr [rax + 0xb8]
0000000180308B02 mov       rdx, qword ptr [rdx]
0000000180308B05 call      0x181197d70
0000000180308B0A mov       rcx, qword ptr [rbx + 0xf0]
0000000180308B11 test      rcx, rcx
0000000180308B14 je        0x180308bbb
0000000180308B1A mov       rax, qword ptr [rip + 0x18b21d7] # 0x181bbacf8
0000000180308B21 xor       r8d, r8d
0000000180308B24 mov       rdx, qword ptr [rax + 0xb8]
0000000180308B2B mov       rdx, qword ptr [rdx]
0000000180308B2E call      0x181197d70
0000000180308B33 mov       rcx, qword ptr [rbx + 0xf8]
0000000180308B3A test      rcx, rcx
0000000180308B3D je        0x180308bbb
0000000180308B3F mov       rax, qword ptr [rip + 0x18b21b2] # 0x181bbacf8
0000000180308B46 xor       r8d, r8d
0000000180308B49 mov       rdx, qword ptr [rax + 0xb8]
0000000180308B50 mov       rdx, qword ptr [rdx]
0000000180308B53 call      0x181197d70
0000000180308B58 mov       rcx, qword ptr [rbx + 0xe0]
0000000180308B5F test      rcx, rcx
0000000180308B62 je        0x180308bbb
0000000180308B64 xor       r8d, r8d
0000000180308B67 xor       edx, edx
0000000180308B69 call      0x18156cc70
0000000180308B6E mov       rcx, qword ptr [rbx + 0xe8]
0000000180308B75 test      rcx, rcx
0000000180308B78 je        0x180308bbb
0000000180308B7A xor       r8d, r8d
0000000180308B7D xor       edx, edx
0000000180308B7F call      0x18156cc70
0000000180308B84 mov       rcx, qword ptr [rbx + 0xf0]
0000000180308B8B test      rcx, rcx
0000000180308B8E je        0x180308bbb
0000000180308B90 xor       r8d, r8d
0000000180308B93 xor       edx, edx
0000000180308B95 call      0x18156cc70
0000000180308B9A mov       rcx, qword ptr [rbx + 0xf8]
0000000180308BA1 test      rcx, rcx
0000000180308BA4 je        0x180308bbb
0000000180308BA6 xor       r8d, r8d
0000000180308BA9 xor       edx, edx
0000000180308BAB call      0x18156cc70
0000000180308BB0 mov       rbx, qword ptr [rsp + 0x30]
0000000180308BB5 add       rsp, 0x20
0000000180308BB9 pop       rdi
0000000180308BBA ret       
0000000180308BBB call      0x180250150
0000000180308BC0 int3      
0000000180308BC1 call      0x180250140
0000000180308BC6 int3      
0000000180308BC7 int3      
0000000180308BC8 int3      
0000000180308BC9 int3      
0000000180308BCA int3      
0000000180308BCB int3      
0000000180308BCC int3      
0000000180308BCD int3      
0000000180308BCE int3      
0000000180308BCF int3      