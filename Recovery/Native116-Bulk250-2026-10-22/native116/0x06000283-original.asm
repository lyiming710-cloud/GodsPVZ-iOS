000000018032AE50 mov       qword ptr [rsp + 8], rbx
000000018032AE55 mov       qword ptr [rsp + 0x20], rsi
000000018032AE5A mov       qword ptr [rsp + 0x18], r8
000000018032AE5F mov       qword ptr [rsp + 0x10], rdx
000000018032AE64 push      rdi
000000018032AE65 push      r14
000000018032AE67 push      r15
000000018032AE69 sub       rsp, 0x70
000000018032AE6D mov       rbx, r9
000000018032AE70 mov       rsi, r8
000000018032AE73 mov       r14, rdx
000000018032AE76 cmp       byte ptr [rip + 0x19b9de9], 0 # 0x181ce4c66
000000018032AE7D jne       0x18032aeda
000000018032AE7F lea       rcx, [rip + 0x189b12a] # 0x181bc5fb0
000000018032AE86 call      0x18024fef0
000000018032AE8B lea       rcx, [rip + 0x189b176] # 0x181bc6008
000000018032AE92 call      0x18024fef0
000000018032AE97 lea       rcx, [rip + 0x189b1c2] # 0x181bc6060
000000018032AE9E call      0x18024fef0
000000018032AEA3 lea       rcx, [rip + 0x1887dbe] # 0x181bb2c68
000000018032AEAA call      0x18024fef0
000000018032AEAF lea       rcx, [rip + 0x1887e0a] # 0x181bb2cc0
000000018032AEB6 call      0x18024fef0
000000018032AEBB lea       rcx, [rip + 0x1887e56] # 0x181bb2d18
000000018032AEC2 call      0x18024fef0
000000018032AEC7 lea       rcx, [rip + 0x1887ea2] # 0x181bb2d70
000000018032AECE call      0x18024fef0
000000018032AED3 mov       byte ptr [rip + 0x19b9d8c], 1 # 0x181ce4c66
000000018032AEDA xor       edi, edi
000000018032AEDC mov       qword ptr [rsp + 0x20], rdi
000000018032AEE1 mov       r9, rsi
000000018032AEE4 mov       r8d, dword ptr [rsp + 0xb8]
000000018032AEEC mov       edx, dword ptr [rsp + 0xb0]
000000018032AEF3 mov       rcx, rbx
000000018032AEF6 call      0x18032d4e0
000000018032AEFB test      rax, rax
000000018032AEFE je        0x18032b078
000000018032AF04 mov       r8, qword ptr [rip + 0x1887db5] # 0x181bb2cc0
000000018032AF0B mov       rdx, rax
000000018032AF0E lea       rcx, [rsp + 0x38]
000000018032AF13 call      0x1808197f0
000000018032AF18 movups    xmm0, xmmword ptr [rsp + 0x38]
000000018032AF1D movups    xmmword ptr [rsp + 0x50], xmm0
000000018032AF22 movsd     xmm1, qword ptr [rsp + 0x48]
000000018032AF28 movsd     qword ptr [rsp + 0x60], xmm1
000000018032AF2E mov       qword ptr [rsp + 0x38], rdi
000000018032AF33 lea       rbx, [rsp + 0x50]
000000018032AF38 mov       qword ptr [rsp + 0x40], rbx
000000018032AF3D mov       r15d, dword ptr [rsp + 0xc0]
000000018032AF45 nop       word ptr [rax + rax]
000000018032AF50 mov       rdx, qword ptr [rip + 0x189b0b1] # 0x181bc6008
000000018032AF57 lea       rcx, [rsp + 0x50]
000000018032AF5C call      0x180669280
000000018032AF61 test      al, al
000000018032AF63 je        0x18032afeb
000000018032AF69 mov       rdi, qword ptr [rsp + 0x60]
000000018032AF6E test      rdi, rdi
000000018032AF71 je        0x18032af50
000000018032AF73 cmp       byte ptr [rdi + 0x28], 0
000000018032AF77 jne       0x18032af50
000000018032AF79 cmp       dword ptr [rdi + 0x18], r15d
000000018032AF7D jg        0x18032af50
000000018032AF7F mov       rcx, qword ptr [r14]
000000018032AF82 test      rcx, rcx
000000018032AF85 je        0x18032b089
000000018032AF8B mov       r8, qword ptr [rip + 0x1887d86] # 0x181bb2d18
000000018032AF92 mov       rdx, rdi
000000018032AF95 call      0x18084a910
000000018032AF9A cmp       eax, -1
000000018032AF9D jne       0x18032afba
000000018032AF9F mov       rcx, qword ptr [r14]
000000018032AFA2 test      rcx, rcx
000000018032AFA5 je        0x18032b07e
000000018032AFAB mov       r8, qword ptr [rip + 0x1887cb6] # 0x181bb2c68
000000018032AFB2 mov       rdx, rdi
000000018032AFB5 call      0x180002e70
000000018032AFBA test      rsi, rsi
000000018032AFBD je        0x18032b084
000000018032AFC3 mov       ecx, dword ptr [rsi + 0x1c]
000000018032AFC6 inc       ecx
000000018032AFC8 cmp       ecx, dword ptr [rdi + 0x1c]
000000018032AFCB jge       0x18032af50
000000018032AFCD mov       dword ptr [rdi + 0x1c], ecx
000000018032AFD0 add       ecx, dword ptr [rdi + 0x20]
000000018032AFD3 mov       dword ptr [rdi + 0x24], ecx
000000018032AFD6 mov       qword ptr [rdi + 0x30], rsi
000000018032AFDA lea       rcx, [rdi + 0x30]
000000018032AFDE mov       rdx, rsi
000000018032AFE1 call      0x18024f360
000000018032AFE6 jmp       0x18032af50
000000018032AFEB mov       rdx, qword ptr [rip + 0x189afbe] # 0x181bc5fb0
000000018032AFF2 mov       rcx, rbx
000000018032AFF5 call      0x180302170
000000018032AFFA jmp       0x18032b027
000000018032AFFC mov       rdx, qword ptr [rip + 0x189afad] # 0x181bc5fb0
000000018032B003 mov       rcx, qword ptr [rsp + 0x40]
000000018032B008 call      0x180302170
000000018032B00D mov       rcx, qword ptr [rsp + 0x38]
000000018032B012 test      rcx, rcx
000000018032B015 jne       0x18032b08f
000000018032B017 mov       rsi, qword ptr [rsp + 0xa0]
000000018032B01F mov       r14, qword ptr [rsp + 0x98]
000000018032B027 mov       rcx, qword ptr [r14]
000000018032B02A test      rcx, rcx
000000018032B02D je        0x18032b078
000000018032B02F mov       r8, qword ptr [rip + 0x1887ce2] # 0x181bb2d18
000000018032B036 mov       rdx, rsi
000000018032B039 call      0x18084a910
000000018032B03E test      eax, eax
000000018032B040 js        0x18032b059
000000018032B042 mov       rcx, qword ptr [r14]
000000018032B045 test      rcx, rcx
000000018032B048 je        0x18032b078
000000018032B04A mov       r8, qword ptr [rip + 0x1887d1f] # 0x181bb2d70
000000018032B051 mov       rdx, rsi
000000018032B054 call      0x18084e290
000000018032B059 test      rsi, rsi
000000018032B05C je        0x18032b078
000000018032B05E mov       byte ptr [rsi + 0x28], 1
000000018032B062 lea       r11, [rsp + 0x70]
000000018032B067 mov       rbx, qword ptr [r11 + 0x20]
000000018032B06B mov       rsi, qword ptr [r11 + 0x38]
000000018032B06F mov       rsp, r11
000000018032B072 pop       r15
000000018032B074 pop       r14
000000018032B076 pop       rdi
000000018032B077 ret       
000000018032B078 call      0x180250150
000000018032B07D nop       
000000018032B07E call      0x180250150
000000018032B083 nop       
000000018032B084 call      0x180250150
000000018032B089 call      0x180250150
000000018032B08E nop       
000000018032B08F call      0x180200e60
000000018032B094 int3      
000000018032B095 int3      
000000018032B096 int3      
000000018032B097 int3      
000000018032B098 int3      
000000018032B099 int3      
000000018032B09A int3      
000000018032B09B int3      
000000018032B09C int3      
000000018032B09D int3      
000000018032B09E int3      
000000018032B09F int3      