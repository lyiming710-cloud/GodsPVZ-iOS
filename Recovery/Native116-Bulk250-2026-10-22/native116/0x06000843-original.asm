00000001803C74F0 mov       qword ptr [rsp + 0x18], rbx
00000001803C74F5 push      rdi
00000001803C74F6 sub       rsp, 0x20
00000001803C74FA cmp       byte ptr [rip + 0x191daca], 0 # 0x181ce4fcb
00000001803C7501 mov       rbx, rcx
00000001803C7504 jne       0x1803c7555
00000001803C7506 lea       rcx, [rip + 0x17d383b] # 0x181b9ad48
00000001803C750D call      0x18024fef0
00000001803C7512 lea       rcx, [rip + 0x1801f67] # 0x181bc9480
00000001803C7519 call      0x18024fef0
00000001803C751E lea       rcx, [rip + 0x1801fb3] # 0x181bc94d8
00000001803C7525 call      0x18024fef0
00000001803C752A lea       rcx, [rip + 0x1804267] # 0x181bcb798
00000001803C7531 call      0x18024fef0
00000001803C7536 lea       rcx, [rip + 0x17e8fc3] # 0x181bb0500
00000001803C753D call      0x18024fef0
00000001803C7542 lea       rcx, [rip + 0x17e01f7] # 0x181ba7740
00000001803C7549 call      0x18024fef0
00000001803C754E mov       byte ptr [rip + 0x191da76], 1 # 0x181ce4fcb
00000001803C7555 lea       rcx, [rbx + 0x40]
00000001803C7559 mov       qword ptr [rbx + 0x40], 0
00000001803C7561 xor       edx, edx
00000001803C7563 call      0x18024f360
00000001803C7568 mov       rcx, qword ptr [rip + 0x17e8f91] # 0x181bb0500
00000001803C756F mov       rdi, qword ptr [rbx + 0x88]
00000001803C7576 cmp       dword ptr [rcx + 0xe0], 0
00000001803C757D jne       0x1803c7584
00000001803C757F call      0x1802501e0
00000001803C7584 mov       qword ptr [rsp + 0x30], rbp
00000001803C7589 xor       edx, edx
00000001803C758B mov       rcx, rdi
00000001803C758E mov       qword ptr [rsp + 0x38], rsi
00000001803C7593 call      0x18131f870
00000001803C7598 test      al, al
00000001803C759A je        0x1803c775d
00000001803C75A0 mov       rax, qword ptr [rbx + 0x88]
00000001803C75A7 test      rax, rax
00000001803C75AA je        0x1803c7847
00000001803C75B0 cmp       qword ptr [rax + 0x38], 0
00000001803C75B5 je        0x1803c775d
00000001803C75BB mov       rcx, qword ptr [rbx + 0x90]
00000001803C75C2 xor       edx, edx
00000001803C75C4 call      0x180b78590
00000001803C75C9 test      al, al
00000001803C75CB jne       0x1803c76f2
00000001803C75D1 mov       rax, qword ptr [rbx + 0x88]
00000001803C75D8 xor       edi, edi
00000001803C75DA test      rax, rax
00000001803C75DD je        0x1803c7847
00000001803C75E3 mov       rbp, qword ptr [rax + 0x38]
00000001803C75E7 test      rbp, rbp
00000001803C75EA je        0x1803c7847
00000001803C75F0 mov       ebp, dword ptr [rbp + 0x18]
00000001803C75F3 test      ebp, ebp
00000001803C75F5 jle       0x1803c7662
00000001803C75F7 nop       word ptr [rax + rax]
00000001803C7600 mov       rax, qword ptr [rbx + 0x88]
00000001803C7607 test      rax, rax
00000001803C760A je        0x1803c7847
00000001803C7610 mov       rcx, qword ptr [rax + 0x38]
00000001803C7614 test      rcx, rcx
00000001803C7617 je        0x1803c7847
00000001803C761D mov       r8, qword ptr [rip + 0x1801eb4] # 0x181bc94d8
00000001803C7624 mov       edx, edi
00000001803C7626 call      0x180825710
00000001803C762B mov       rsi, rax
00000001803C762E test      rax, rax
00000001803C7631 je        0x1803c764a
00000001803C7633 mov       rdx, qword ptr [rbx + 0x90]
00000001803C763A xor       r8d, r8d
00000001803C763D mov       rcx, qword ptr [rax + 0x10]
00000001803C7641 call      0x180b76170
00000001803C7646 test      al, al
00000001803C7648 jne       0x1803c7652
00000001803C764A inc       edi
00000001803C764C cmp       edi, ebp
00000001803C764E jl        0x1803c7600
00000001803C7650 jmp       0x1803c7662
00000001803C7652 lea       rcx, [rbx + 0x40]
00000001803C7656 mov       qword ptr [rbx + 0x40], rsi
00000001803C765A mov       rdx, rsi
00000001803C765D call      0x18024f360
00000001803C7662 cmp       qword ptr [rbx + 0x40], 0
00000001803C7667 jne       0x1803c775d
00000001803C766D mov       rcx, qword ptr [rip + 0x1804124] # 0x181bcb798
00000001803C7674 mov       edx, 1
00000001803C7679 call      0x18024f3e0
00000001803C767E mov       rsi, qword ptr [rbx + 0x90]
00000001803C7685 mov       rdi, rax
00000001803C7688 test      rax, rax
00000001803C768B je        0x1803c7847
00000001803C7691 test      rsi, rsi
00000001803C7694 je        0x1803c76ae
00000001803C7696 mov       rdx, qword ptr [rax]
00000001803C7699 mov       rcx, rsi
00000001803C769C mov       rdx, qword ptr [rdx + 0x40]
00000001803C76A0 call      0x18024f380
00000001803C76A5 test      rax, rax
00000001803C76A8 je        0x1803c7831
00000001803C76AE cmp       dword ptr [rdi + 0x18], 0
00000001803C76B2 jbe       0x1803c7841
00000001803C76B8 lea       rcx, [rdi + 0x20]
00000001803C76BC mov       qword ptr [rdi + 0x20], rsi
00000001803C76C0 mov       rdx, rsi
00000001803C76C3 call      0x18024f360
00000001803C76C8 mov       rcx, qword ptr [rip + 0x17d3679] # 0x181b9ad48
00000001803C76CF cmp       dword ptr [rcx + 0xe0], 0
00000001803C76D6 jne       0x1803c76dd
00000001803C76D8 call      0x1802501e0
00000001803C76DD mov       rdx, qword ptr [rip + 0x17e005c] # 0x181ba7740
00000001803C76E4 xor       r9d, r9d
00000001803C76E7 mov       r8, rdi
00000001803C76EA mov       rcx, rbx
00000001803C76ED call      0x1812e65b0
00000001803C76F2 cmp       qword ptr [rbx + 0x40], 0
00000001803C76F7 jne       0x1803c775d
00000001803C76F9 mov       rax, qword ptr [rbx + 0x88]
00000001803C7700 xor       edi, edi
00000001803C7702 test      rax, rax
00000001803C7705 je        0x1803c7847
00000001803C770B mov       rbp, qword ptr [rax + 0x38]
00000001803C770F test      rbp, rbp
00000001803C7712 je        0x1803c7847
00000001803C7718 mov       ebp, dword ptr [rbp + 0x18]
00000001803C771B test      ebp, ebp
00000001803C771D jle       0x1803c775d
00000001803C771F nop       
00000001803C7720 mov       rax, qword ptr [rbx + 0x88]
00000001803C7727 test      rax, rax
00000001803C772A je        0x1803c7847
00000001803C7730 mov       rcx, qword ptr [rax + 0x38]
00000001803C7734 test      rcx, rcx
00000001803C7737 je        0x1803c7847
00000001803C773D mov       r8, qword ptr [rip + 0x1801d94] # 0x181bc94d8
00000001803C7744 mov       edx, edi
00000001803C7746 call      0x180825710
00000001803C774B mov       rsi, rax
00000001803C774E test      rax, rax
00000001803C7751 jne       0x1803c77d8
00000001803C7757 inc       edi
00000001803C7759 cmp       edi, ebp
00000001803C775B jl        0x1803c7720
00000001803C775D movzx     ecx, byte ptr [rip + 0x191d860] # 0x181ce4fc4
00000001803C7764 mov       byte ptr [rbx + 0x38], 1
00000001803C7768 test      cl, cl
00000001803C776A jne       0x1803c7780
00000001803C776C lea       rcx, [rip + 0x1801c15] # 0x181bc9388
00000001803C7773 call      0x18024fef0
00000001803C7778 mov       cl, 1
00000001803C777A mov       byte ptr [rip + 0x191d844], cl # 0x181ce4fc4
00000001803C7780 cmp       qword ptr [rbx + 0x40], 0
00000001803C7785 je        0x1803c7814
00000001803C778B mov       rax, qword ptr [rbx + 0x40]
00000001803C778F cmp       qword ptr [rax + 0x18], 0
00000001803C7794 je        0x1803c7814
00000001803C7796 mov       rdx, qword ptr [rax + 0x18]
00000001803C779A cmp       dword ptr [rdx + 0x18], 0
00000001803C779E jle       0x1803c7814
00000001803C77A0 mov       edi, dword ptr [rbx + 0x98]
00000001803C77A6 test      cl, cl
00000001803C77A8 jne       0x1803c77bd
00000001803C77AA lea       rcx, [rip + 0x1801bd7] # 0x181bc9388
00000001803C77B1 call      0x18024fef0
00000001803C77B6 mov       byte ptr [rip + 0x191d807], 1 # 0x181ce4fc4
00000001803C77BD cmp       qword ptr [rbx + 0x40], 0
00000001803C77C2 je        0x1803c7804
00000001803C77C4 mov       rax, qword ptr [rbx + 0x40]
00000001803C77C8 cmp       qword ptr [rax + 0x18], 0
00000001803C77CD je        0x1803c7804
00000001803C77CF mov       rcx, qword ptr [rax + 0x18]
00000001803C77D3 mov       eax, dword ptr [rcx + 0x18]
00000001803C77D6 jmp       0x1803c7806
00000001803C77D8 mov       rdx, qword ptr [rax + 0x10]
00000001803C77DC lea       rcx, [rbx + 0x90]
00000001803C77E3 mov       qword ptr [rbx + 0x90], rdx
00000001803C77EA call      0x18024f360
00000001803C77EF lea       rcx, [rbx + 0x40]
00000001803C77F3 mov       qword ptr [rbx + 0x40], rsi
00000001803C77F7 mov       rdx, rsi
00000001803C77FA call      0x18024f360
00000001803C77FF jmp       0x1803c775d
00000001803C7804 xor       eax, eax
00000001803C7806 dec       eax
00000001803C7808 test      edi, edi
00000001803C780A js        0x1803c7814
00000001803C780C cmp       edi, eax
00000001803C780E jle       0x1803c7816
00000001803C7810 mov       edi, eax
00000001803C7812 jmp       0x1803c7816
00000001803C7814 xor       edi, edi
00000001803C7816 mov       rsi, qword ptr [rsp + 0x38]
00000001803C781B mov       rbp, qword ptr [rsp + 0x30]
00000001803C7820 mov       dword ptr [rbx + 0x98], edi
00000001803C7826 mov       rbx, qword ptr [rsp + 0x40]
00000001803C782B add       rsp, 0x20
00000001803C782F pop       rdi
00000001803C7830 ret       
00000001803C7831 call      0x18024fc50
00000001803C7836 mov       rcx, rax
00000001803C7839 xor       edx, edx
00000001803C783B call      0x180250110
00000001803C7840 int3      
00000001803C7841 call      0x180250140
00000001803C7846 int3      
00000001803C7847 call      0x180250150
00000001803C784C int3      
00000001803C784D int3      
00000001803C784E int3      
00000001803C784F int3      