000000018034F750 mov       qword ptr [rsp + 0x10], rbx
000000018034F755 mov       qword ptr [rsp + 0x20], rsi
000000018034F75A mov       qword ptr [rsp + 8], rcx
000000018034F75F push      rdi
000000018034F760 push      r14
000000018034F762 push      r15
000000018034F764 sub       rsp, 0x70
000000018034F768 movaps    xmmword ptr [rsp + 0x60], xmm6
000000018034F76D mov       rsi, rcx
000000018034F770 cmp       byte ptr [rip + 0x1995583], 0 # 0x181ce4cfa
000000018034F777 jne       0x18034f7c8
000000018034F779 lea       rcx, [rip + 0x1868a90] # 0x181bb8210
000000018034F780 call      0x18024fef0
000000018034F785 lea       rcx, [rip + 0x187b0dc] # 0x181bca868
000000018034F78C call      0x18024fef0
000000018034F791 lea       rcx, [rip + 0x187b120] # 0x181bca8b8
000000018034F798 call      0x18024fef0
000000018034F79D lea       rcx, [rip + 0x187b16c] # 0x181bca910
000000018034F7A4 call      0x18024fef0
000000018034F7A9 lea       rcx, [rip + 0x1873f38] # 0x181bc36e8
000000018034F7B0 call      0x18024fef0
000000018034F7B5 lea       rcx, [rip + 0x1860d44] # 0x181bb0500
000000018034F7BC call      0x18024fef0
000000018034F7C1 mov       byte ptr [rip + 0x1995532], 1 # 0x181ce4cfa
000000018034F7C8 mov       rcx, qword ptr [rsi + 0x70]
000000018034F7CC cmp       byte ptr [rsi + 0xe8], 0
000000018034F7D3 jne       0x18034f7e6
000000018034F7D5 test      rcx, rcx
000000018034F7D8 je        0x18034fba4
000000018034F7DE xor       r14d, r14d
000000018034F7E1 mov       edx, r14d
000000018034F7E4 jmp       0x18034f805
000000018034F7E6 mov       rax, qword ptr [rsi + 0xe0]
000000018034F7ED test      rax, rax
000000018034F7F0 je        0x18034fba4
000000018034F7F6 mov       edx, dword ptr [rax + 0x18]
000000018034F7F9 test      rcx, rcx
000000018034F7FC je        0x18034fba4
000000018034F802 xor       r14d, r14d
000000018034F805 xor       r8d, r8d
000000018034F808 call      0x1803944e0
000000018034F80D mov       r15, rax
000000018034F810 mov       eax, dword ptr [rsi + 0x7c]
000000018034F813 test      eax, eax
000000018034F815 jne       0x18034fa5b
000000018034F81B cmp       byte ptr [rsi + 0xf4], al
000000018034F821 je        0x18034f8d9
000000018034F827 mov       rax, qword ptr [rsi + 0xe0]
000000018034F82E test      rax, rax
000000018034F831 je        0x18034fba4
000000018034F837 cmp       dword ptr [rax + 0x18], 1
000000018034F83B jne       0x18034f8d9
000000018034F841 mov       rbx, qword ptr [rsi + 0x128]
000000018034F848 mov       rcx, qword ptr [rip + 0x1860cb1] # 0x181bb0500
000000018034F84F cmp       dword ptr [rcx + 0xe0], 0
000000018034F856 jne       0x18034f85d
000000018034F858 call      0x1802501e0
000000018034F85D xor       r8d, r8d
000000018034F860 xor       edx, edx
000000018034F862 mov       rcx, rbx
000000018034F865 call      0x18131f900
000000018034F86A test      al, al
000000018034F86C jne       0x18034f8d5
000000018034F86E mov       rbx, qword ptr [rsi + 0x120]
000000018034F875 mov       rcx, qword ptr [rip + 0x1860c84] # 0x181bb0500
000000018034F87C cmp       dword ptr [rcx + 0xe0], 0
000000018034F883 jne       0x18034f88a
000000018034F885 call      0x1802501e0
000000018034F88A xor       edx, edx
000000018034F88C mov       rcx, rbx
000000018034F88F call      0x18131f870
000000018034F894 test      al, al
000000018034F896 je        0x18034f8d9
000000018034F898 mov       rcx, qword ptr [rsi + 0x120]
000000018034F89F test      rcx, rcx
000000018034F8A2 je        0x18034fba4
000000018034F8A8 xor       edx, edx
000000018034F8AA call      0x18035de80
000000018034F8AF test      al, al
000000018034F8B1 je        0x18034f8d9
000000018034F8B3 mov       rax, qword ptr [rsi + 0x120]
000000018034F8BA lea       r11, [rsp + 0x70]
000000018034F8BF mov       rbx, qword ptr [r11 + 0x28]
000000018034F8C3 mov       rsi, qword ptr [r11 + 0x38]
000000018034F8C7 movaps    xmm6, xmmword ptr [rsp + 0x60]
000000018034F8CC mov       rsp, r11
000000018034F8CF pop       r15
000000018034F8D1 pop       r14
000000018034F8D3 pop       rdi
000000018034F8D4 ret       
000000018034F8D5 xor       eax, eax
000000018034F8D7 jmp       0x18034f8ba
000000018034F8D9 mov       qword ptr [rsp + 0xa0], r14
000000018034F8E1 movss     xmm6, dword ptr [rip + 0x12583d3] # 0x1815a7cbc
000000018034F8E9 mov       rcx, qword ptr [rsi + 0x70]
000000018034F8ED test      rcx, rcx
000000018034F8F0 je        0x18034fba4
000000018034F8F6 xor       r8d, r8d
000000018034F8F9 xor       edx, edx
000000018034F8FB call      0x1803944e0
000000018034F900 mov       r15, rax
000000018034F903 mov       rcx, qword ptr [rsi + 0x1e8]
000000018034F90A test      rcx, rcx
000000018034F90D je        0x18034fba4
000000018034F913 mov       rcx, qword ptr [rcx + 0xf0]
000000018034F91A test      rcx, rcx
000000018034F91D je        0x18034fba4
000000018034F923 mov       rdx, qword ptr [rcx + 0x38]
000000018034F927 test      rdx, rdx
000000018034F92A je        0x18034fba4
000000018034F930 mov       r8, qword ptr [rip + 0x1873db1] # 0x181bc36e8
000000018034F937 lea       rcx, [rsp + 0x30]
000000018034F93C call      0x1808197f0
000000018034F941 movups    xmm0, xmmword ptr [rsp + 0x30]
000000018034F946 movups    xmmword ptr [rsp + 0x48], xmm0
000000018034F94B movsd     xmm1, qword ptr [rsp + 0x40]
000000018034F951 movsd     qword ptr [rsp + 0x58], xmm1
000000018034F957 mov       qword ptr [rsp + 0x30], r14
000000018034F95C lea       rbx, [rsp + 0x48]
000000018034F961 mov       qword ptr [rsp + 0x38], rbx
000000018034F966 nop       word ptr [rax + rax]
000000018034F970 mov       rdx, qword ptr [rip + 0x187af41] # 0x181bca8b8
000000018034F977 lea       rcx, [rsp + 0x48]
000000018034F97C call      0x180669280
000000018034F981 test      al, al
000000018034F983 je        0x18034fa00
000000018034F985 mov       rdi, qword ptr [rsp + 0x58]
000000018034F98A test      rdi, rdi
000000018034F98D je        0x18034fbb4
000000018034F993 xor       edx, edx
000000018034F995 mov       rcx, rdi
000000018034F998 call      0x18035de80
000000018034F99D test      al, al
000000018034F99F je        0x18034f970
000000018034F9A1 test      rdi, rdi
000000018034F9A4 je        0x18034fbaf
000000018034F9AA xor       edx, edx
000000018034F9AC mov       rcx, rdi
000000018034F9AF call      0x1803659e0
000000018034F9B4 test      al, al
000000018034F9B6 jne       0x18034f970
000000018034F9B8 mov       eax, dword ptr [rdi + 0xfc]
000000018034F9BE cmp       eax, dword ptr [rsi + 0xbc]
000000018034F9C4 je        0x18034f970
000000018034F9C6 test      r15, r15
000000018034F9C9 je        0x18034fbaa
000000018034F9CF mov       r8, qword ptr [rip + 0x186883a] # 0x181bb8210
000000018034F9D6 mov       rdx, rdi
000000018034F9D9 mov       rcx, r15
000000018034F9DC call      0x180426dd0
000000018034F9E1 test      al, al
000000018034F9E3 je        0x18034f970
000000018034F9E5 comiss    xmm6, dword ptr [rdi + 0x30]
000000018034F9E9 jbe       0x18034f970
000000018034F9EB mov       r14, rdi
000000018034F9EE mov       qword ptr [rsp + 0xa0], rdi
000000018034F9F6 movss     xmm6, dword ptr [rdi + 0x30]
000000018034F9FB jmp       0x18034f970
000000018034FA00 mov       rdx, qword ptr [rip + 0x187ae61] # 0x181bca868
000000018034FA07 mov       rcx, rbx
000000018034FA0A call      0x180302170
000000018034FA0F jmp       0x18034fa40
000000018034FA11 mov       rdx, qword ptr [rip + 0x187ae50] # 0x181bca868
000000018034FA18 mov       rcx, qword ptr [rsp + 0x38]
000000018034FA1D call      0x180302170
000000018034FA22 mov       rcx, qword ptr [rsp + 0x30]
000000018034FA27 test      rcx, rcx
000000018034FA2A jne       0x18034fbba
000000018034FA30 mov       rsi, qword ptr [rsp + 0x90]
000000018034FA38 mov       r14, qword ptr [rsp + 0xa0]
000000018034FA40 mov       qword ptr [rsi + 0x120], r14
000000018034FA47 lea       rcx, [rsi + 0x120]
000000018034FA4E mov       rdx, r14
000000018034FA51 call      0x18024f360
000000018034FA56 jmp       0x18034f8b3
000000018034FA5B cmp       eax, 1
000000018034FA5E je        0x18034f8d5
000000018034FA64 mov       qword ptr [rsp + 0xa0], r14
000000018034FA6C movss     xmm6, dword ptr [rip + 0x1258248] # 0x1815a7cbc
000000018034FA74 mov       rax, qword ptr [rsi + 0x1e8]
000000018034FA7B test      rax, rax
000000018034FA7E je        0x18034fba4
000000018034FA84 mov       rcx, qword ptr [rax + 0xf0]
000000018034FA8B test      rcx, rcx
000000018034FA8E je        0x18034fba4
000000018034FA94 mov       rdx, qword ptr [rcx + 0x38]
000000018034FA98 test      rdx, rdx
000000018034FA9B je        0x18034fba4
000000018034FAA1 mov       r8, qword ptr [rip + 0x1873c40] # 0x181bc36e8
000000018034FAA8 lea       rcx, [rsp + 0x30]
000000018034FAAD call      0x1808197f0
000000018034FAB2 movups    xmm0, xmmword ptr [rsp + 0x30]
000000018034FAB7 movups    xmmword ptr [rsp + 0x48], xmm0
000000018034FABC movsd     xmm1, qword ptr [rsp + 0x40]
000000018034FAC2 movsd     qword ptr [rsp + 0x58], xmm1
000000018034FAC8 mov       qword ptr [rsp + 0x30], r14
000000018034FACD lea       rbx, [rsp + 0x48]
000000018034FAD2 mov       qword ptr [rsp + 0x38], rbx
000000018034FAD7 nop       word ptr [rax + rax]
000000018034FAE0 mov       rdx, qword ptr [rip + 0x187add1] # 0x181bca8b8
000000018034FAE7 lea       rcx, [rsp + 0x48]
000000018034FAEC call      0x180669280
000000018034FAF1 test      al, al
000000018034FAF3 je        0x18034fb70
000000018034FAF5 mov       rdi, qword ptr [rsp + 0x58]
000000018034FAFA test      rdi, rdi
000000018034FAFD je        0x18034fbca
000000018034FB03 xor       edx, edx
000000018034FB05 mov       rcx, rdi
000000018034FB08 call      0x18035de80
000000018034FB0D test      al, al
000000018034FB0F je        0x18034fae0
000000018034FB11 test      rdi, rdi
000000018034FB14 je        0x18034fbc5
000000018034FB1A xor       edx, edx
000000018034FB1C mov       rcx, rdi
000000018034FB1F call      0x1803659e0
000000018034FB24 test      al, al
000000018034FB26 jne       0x18034fae0
000000018034FB28 mov       eax, dword ptr [rdi + 0xfc]
000000018034FB2E cmp       eax, dword ptr [rsi + 0xbc]
000000018034FB34 je        0x18034fae0
000000018034FB36 test      r15, r15
000000018034FB39 je        0x18034fbc0
000000018034FB3F mov       r8, qword ptr [rip + 0x18686ca] # 0x181bb8210
000000018034FB46 mov       rdx, rdi
000000018034FB49 mov       rcx, r15
000000018034FB4C call      0x180426dd0
000000018034FB51 test      al, al
000000018034FB53 je        0x18034fae0
000000018034FB55 comiss    xmm6, dword ptr [rdi + 0x30]
000000018034FB59 jbe       0x18034fae0
000000018034FB5B mov       r14, rdi
000000018034FB5E mov       qword ptr [rsp + 0xa0], rdi
000000018034FB66 movss     xmm6, dword ptr [rdi + 0x30]
000000018034FB6B jmp       0x18034fae0
000000018034FB70 mov       rdx, qword ptr [rip + 0x187acf1] # 0x181bca868
000000018034FB77 mov       rcx, rbx
000000018034FB7A call      0x180302170
000000018034FB7F jmp       0x18034fa40
000000018034FB84 mov       rdx, qword ptr [rip + 0x187acdd] # 0x181bca868
000000018034FB8B mov       rcx, qword ptr [rsp + 0x38]
000000018034FB90 call      0x180302170
000000018034FB95 mov       rcx, qword ptr [rsp + 0x30]
000000018034FB9A test      rcx, rcx
000000018034FB9D jne       0x18034fbd0
000000018034FB9F jmp       0x18034fa30
000000018034FBA4 call      0x180250150
000000018034FBA9 nop       
000000018034FBAA call      0x180250150
000000018034FBAF call      0x180250150
000000018034FBB4 call      0x180250150
000000018034FBB9 nop       
000000018034FBBA call      0x180200e60
000000018034FBBF nop       
000000018034FBC0 call      0x180250150
000000018034FBC5 call      0x180250150
000000018034FBCA call      0x180250150
000000018034FBCF nop       
000000018034FBD0 call      0x180200e60
000000018034FBD5 int3      
000000018034FBD6 int3      
000000018034FBD7 int3      
000000018034FBD8 int3      
000000018034FBD9 int3      
000000018034FBDA int3      
000000018034FBDB int3      
000000018034FBDC int3      
000000018034FBDD int3      
000000018034FBDE int3      
000000018034FBDF int3      