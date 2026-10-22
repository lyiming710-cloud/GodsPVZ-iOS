00000001803C1170 mov       qword ptr [rsp + 8], rbx
00000001803C1175 push      rdi
00000001803C1176 sub       rsp, 0x20
00000001803C117A cmp       byte ptr [rip + 0x1923df9], 0 # 0x181ce4f7a
00000001803C1181 mov       rdi, rcx
00000001803C1184 jne       0x1803c1199
00000001803C1186 lea       rcx, [rip + 0x1802833] # 0x181bc39c0
00000001803C118D call      0x18024fef0
00000001803C1192 mov       byte ptr [rip + 0x1923de1], 1 # 0x181ce4f7a
00000001803C1199 mov       ecx, dword ptr [rdi + 0x10]
00000001803C119C xor       ebx, ebx
00000001803C119E test      ecx, ecx
00000001803C11A0 je        0x1803c11ce
00000001803C11A2 sub       ecx, 1
00000001803C11A5 je        0x1803c11c2
00000001803C11A7 cmp       ecx, 1
00000001803C11AA je        0x1803c11b9
00000001803C11AC xor       al, al
00000001803C11AE mov       rbx, qword ptr [rsp + 0x30]
00000001803C11B3 add       rsp, 0x20
00000001803C11B7 pop       rdi
00000001803C11B8 ret       
00000001803C11B9 mov       dword ptr [rdi + 0x10], 0xffffffff
00000001803C11C0 jmp       0x1803c1241
00000001803C11C2 mov       dword ptr [rdi + 0x10], 0xffffffff
00000001803C11C9 jmp       0x1803c12ca
00000001803C11CE mov       rcx, qword ptr [rdi + 0x20]
00000001803C11D2 mov       dword ptr [rdi + 0x10], 0xffffffff
00000001803C11D9 test      rcx, rcx
00000001803C11DC je        0x1803c1359
00000001803C11E2 mov       rax, qword ptr [rcx]
00000001803C11E5 xor       r8d, r8d
00000001803C11E8 xor       edx, edx
00000001803C11EA mov       r9, qword ptr [rax + 0x7e0]
00000001803C11F1 call      qword ptr [rax + 0x7d8]
00000001803C11F7 mov       rax, qword ptr [rdi + 0x20]
00000001803C11FB test      rax, rax
00000001803C11FE je        0x1803c1359
00000001803C1204 mov       rax, qword ptr [rax + 0x370]
00000001803C120B test      rax, rax
00000001803C120E je        0x1803c1359
00000001803C1214 mov       eax, dword ptr [rax + 0x24]
00000001803C1217 mov       dword ptr [rdi + 0x28], eax
00000001803C121A mov       rax, qword ptr [rdi + 0x20]
00000001803C121E test      rax, rax
00000001803C1221 je        0x1803c1359
00000001803C1227 mov       rax, qword ptr [rax + 0x370]
00000001803C122E test      rax, rax
00000001803C1231 je        0x1803c1359
00000001803C1237 mov       eax, dword ptr [rax + 0x18]
00000001803C123A mov       dword ptr [rdi + 0x2c], eax
00000001803C123D mov       qword ptr [rdi + 0x30], rbx
00000001803C1241 mov       ecx, dword ptr [rdi + 0x28]
00000001803C1244 mov       eax, dword ptr [rdi + 0x30]
00000001803C1247 inc       ecx
00000001803C1249 cdq       
00000001803C124A idiv      ecx
00000001803C124C test      edx, edx
00000001803C124E je        0x1803c12a7
00000001803C1250 cmp       edx, dword ptr [rdi + 0x28]
00000001803C1253 jl        0x1803c125f
00000001803C1255 jne       0x1803c12aa
00000001803C1257 mov       eax, dword ptr [rdi + 0x2c]
00000001803C125A mov       dword ptr [rdi + 0x34], eax
00000001803C125D jmp       0x1803c12aa
00000001803C125F mov       rax, qword ptr [rdi + 0x20]
00000001803C1263 test      rax, rax
00000001803C1266 je        0x1803c1359
00000001803C126C mov       rcx, qword ptr [rax + 0x370]
00000001803C1273 test      rcx, rcx
00000001803C1276 je        0x1803c1359
00000001803C127C mov       rcx, qword ptr [rcx + 0x40]
00000001803C1280 test      rcx, rcx
00000001803C1283 je        0x1803c1359
00000001803C1289 movsxd    rax, edx
00000001803C128C dec       rax
00000001803C128F cmp       eax, dword ptr [rcx + 0x18]
00000001803C1292 jae       0x1803c135f
00000001803C1298 lea       rax, [rax + rax*2]
00000001803C129C mov       ecx, dword ptr [rcx + rax*8 + 0x2c]
00000001803C12A0 inc       ecx
00000001803C12A2 mov       dword ptr [rdi + 0x34], ecx
00000001803C12A5 jmp       0x1803c12aa
00000001803C12A7 mov       dword ptr [rdi + 0x34], ebx
00000001803C12AA mov       rcx, qword ptr [rdi + 0x20]
00000001803C12AE test      rcx, rcx
00000001803C12B1 je        0x1803c1359
00000001803C12B7 mov       edx, dword ptr [rdi + 0x34]
00000001803C12BA xor       r8d, r8d
00000001803C12BD call      0x1811b9b50
00000001803C12C2 mov       eax, dword ptr [rdi + 0x2c]
00000001803C12C5 cmp       dword ptr [rdi + 0x34], eax
00000001803C12C8 jge       0x1803c1313
00000001803C12CA inc       dword ptr [rdi + 0x30]
00000001803C12CD mov       rcx, qword ptr [rip + 0x18026ec] # 0x181bc39c0
00000001803C12D4 call      0x180250100
00000001803C12D9 movss     xmm1, dword ptr [rip + 0x11e6927] # 0x1815a7c08
00000001803C12E1 xor       r8d, r8d
00000001803C12E4 mov       rcx, rax
00000001803C12E7 mov       rbx, rax
00000001803C12EA call      0x181333710
00000001803C12EF lea       rcx, [rdi + 0x18]
00000001803C12F3 mov       qword ptr [rdi + 0x18], rbx
00000001803C12F7 mov       rdx, rbx
00000001803C12FA call      0x18024f360
00000001803C12FF mov       al, 1
00000001803C1301 mov       dword ptr [rdi + 0x10], 2
00000001803C1308 mov       rbx, qword ptr [rsp + 0x30]
00000001803C130D add       rsp, 0x20
00000001803C1311 pop       rdi
00000001803C1312 ret       
00000001803C1313 mov       rcx, qword ptr [rip + 0x18026a6] # 0x181bc39c0
00000001803C131A call      0x180250100
00000001803C131F movss     xmm1, dword ptr [rip + 0x11e66e9] # 0x1815a7a10
00000001803C1327 xor       r8d, r8d
00000001803C132A mov       rcx, rax
00000001803C132D mov       rbx, rax
00000001803C1330 call      0x181333710
00000001803C1335 lea       rcx, [rdi + 0x18]
00000001803C1339 mov       qword ptr [rdi + 0x18], rbx
00000001803C133D mov       rdx, rbx
00000001803C1340 call      0x18024f360
00000001803C1345 mov       rbx, qword ptr [rsp + 0x30]
00000001803C134A mov       al, 1
00000001803C134C mov       dword ptr [rdi + 0x10], 1
00000001803C1353 add       rsp, 0x20
00000001803C1357 pop       rdi
00000001803C1358 ret       
00000001803C1359 call      0x180250150
00000001803C135E int3      
00000001803C135F call      0x180250140
00000001803C1364 int3      
00000001803C1365 int3      
00000001803C1366 int3      
00000001803C1367 int3      
00000001803C1368 int3      
00000001803C1369 int3      
00000001803C136A int3      
00000001803C136B int3      
00000001803C136C int3      
00000001803C136D int3      
00000001803C136E int3      
00000001803C136F int3      