
/workspaces/GodsPVZ-native24-codespace/.validation/native55-2026-10-02/native/0x06000365.bin:     file format binary


Disassembly of section .data:

000000018034ff40 <.data>:
   18034ff40:	40 53                	rex push %rbx
   18034ff42:	48 83 ec 40          	sub    $0x40,%rsp
   18034ff46:	83 79 7c 02          	cmpl   $0x2,0x7c(%rcx)
   18034ff4a:	48 8b d9             	mov    %rcx,%rbx
   18034ff4d:	0f 85 53 01 00 00    	jne    0x1803500a6
   18034ff53:	80 3d da 4b 99 01 00 	cmpb   $0x0,0x1994bda(%rip)        # 0x181ce4b34
   18034ff5a:	75 13                	jne    0x18034ff6f
   18034ff5c:	48 8d 0d 2d 19 86 01 	lea    0x186192d(%rip),%rcx        # 0x181bb1890
   18034ff63:	e8 88 ff ef ff       	call   0x18024fef0
   18034ff68:	c6 05 c5 4b 99 01 01 	movb   $0x1,0x1994bc5(%rip)        # 0x181ce4b34
   18034ff6f:	48 8b 05 1a 19 86 01 	mov    0x186191a(%rip),%rax        # 0x181bb1890
   18034ff76:	48 89 7c 24 50       	mov    %rdi,0x50(%rsp)
   18034ff7b:	48 8b 90 b8 00 00 00 	mov    0xb8(%rax),%rdx
   18034ff82:	48 8b 0a             	mov    (%rdx),%rcx
   18034ff85:	48 85 c9             	test   %rcx,%rcx
   18034ff88:	0f 84 1e 01 00 00    	je     0x1803500ac
   18034ff8e:	45 33 c0             	xor    %r8d,%r8d
   18034ff91:	ba 02 00 00 00       	mov    $0x2,%edx
   18034ff96:	e8 95 1f fd ff       	call   0x180321f30
   18034ff9b:	48 8b f8             	mov    %rax,%rdi
   18034ff9e:	48 85 c0             	test   %rax,%rax
   18034ffa1:	0f 84 05 01 00 00    	je     0x1803500ac
   18034ffa7:	33 d2                	xor    %edx,%edx
   18034ffa9:	48 8b c8             	mov    %rax,%rcx
   18034ffac:	e8 bf c0 fc 00       	call   0x18131c070
   18034ffb1:	f3 0f 10 4b 38       	movss  0x38(%rbx),%xmm1
   18034ffb6:	0f 57 d2             	xorps  %xmm2,%xmm2
   18034ffb9:	f3 0f 58 4b 34       	addss  0x34(%rbx),%xmm1
   18034ffbe:	48 85 c0             	test   %rax,%rax
   18034ffc1:	0f 84 e5 00 00 00    	je     0x1803500ac
   18034ffc7:	f3 0f 10 43 30       	movss  0x30(%rbx),%xmm0
   18034ffcc:	48 8d 54 24 30       	lea    0x30(%rsp),%rdx
   18034ffd1:	0f 14 c1             	unpcklps %xmm1,%xmm0
   18034ffd4:	45 33 c0             	xor    %r8d,%r8d
   18034ffd7:	48 8b c8             	mov    %rax,%rcx
   18034ffda:	f2 0f 11 44 24 30    	movsd  %xmm0,0x30(%rsp)
   18034ffe0:	f3 0f 11 54 24 38    	movss  %xmm2,0x38(%rsp)
   18034ffe6:	e8 c5 fb fd 00       	call   0x18132fbb0
   18034ffeb:	33 d2                	xor    %edx,%edx
   18034ffed:	48 8b cf             	mov    %rdi,%rcx
   18034fff0:	e8 7b c0 fc 00       	call   0x18131c070
   18034fff5:	f3 0f 10 0d cb 7a 25 	movss  0x1257acb(%rip),%xmm1        # 0x1815a7ac8
   18034fffc:	01 
   18034fffd:	48 85 c0             	test   %rax,%rax
   180350000:	0f 84 a6 00 00 00    	je     0x1803500ac
   180350006:	0f 28 c1             	movaps %xmm1,%xmm0
   180350009:	f3 0f 11 4c 24 38    	movss  %xmm1,0x38(%rsp)
   18035000f:	0f 14 c1             	unpcklps %xmm1,%xmm0
   180350012:	48 8d 54 24 30       	lea    0x30(%rsp),%rdx
   180350017:	45 33 c0             	xor    %r8d,%r8d
   18035001a:	f2 0f 11 44 24 30    	movsd  %xmm0,0x30(%rsp)
   180350020:	48 8b c8             	mov    %rax,%rcx
   180350023:	e8 28 fa fd 00       	call   0x18132fa50
   180350028:	48 8b 8b e8 01 00 00 	mov    0x1e8(%rbx),%rcx
   18035002f:	48 85 c9             	test   %rcx,%rcx
   180350032:	74 78                	je     0x1803500ac
   180350034:	f3 0f 10 15 c8 7a 25 	movss  0x1257ac8(%rip),%xmm2        # 0x1815a7b04
   18035003b:	01 
   18035003c:	45 33 c9             	xor    %r9d,%r9d
   18035003f:	f3 0f 10 0d c5 7b 25 	movss  0x1257bc5(%rip),%xmm1        # 0x1815a7c0c
   180350046:	01 
   180350047:	e8 94 76 fd ff       	call   0x1803276e0
   18035004c:	45 33 c9             	xor    %r9d,%r9d
   18035004f:	48 c7 44 24 20 00 00 	movq   $0x0,0x20(%rsp)
   180350056:	00 00 
   180350058:	33 d2                	xor    %edx,%edx
   18035005a:	41 b8 ff ff ff ff    	mov    $0xffffffff,%r8d
   180350060:	48 8b cb             	mov    %rbx,%rcx
   180350063:	e8 d8 0d 00 00       	call   0x180350e40
   180350068:	48 85 c0             	test   %rax,%rax
   18035006b:	74 3f                	je     0x1803500ac
   18035006d:	33 d2                	xor    %edx,%edx
   18035006f:	48 8b c8             	mov    %rax,%rcx
   180350072:	e8 19 18 fc ff       	call   0x180311890
   180350077:	48 8b 83 e0 00 00 00 	mov    0xe0(%rbx),%rax
   18035007e:	48 85 c0             	test   %rax,%rax
   180350081:	74 29                	je     0x1803500ac
   180350083:	83 78 18 01          	cmpl   $0x1,0x18(%rax)
   180350087:	75 0e                	jne    0x180350097
   180350089:	45 33 c0             	xor    %r8d,%r8d
   18035008c:	48 8b d3             	mov    %rbx,%rdx
   18035008f:	48 8b c8             	mov    %rax,%rcx
   180350092:	e8 b9 64 03 00       	call   0x180386550
   180350097:	33 d2                	xor    %edx,%edx
   180350099:	48 8b cb             	mov    %rbx,%rcx
   18035009c:	e8 bf 60 00 00       	call   0x180356160
   1803500a1:	48 8b 7c 24 50       	mov    0x50(%rsp),%rdi
   1803500a6:	48 83 c4 40          	add    $0x40,%rsp
   1803500aa:	5b                   	pop    %rbx
   1803500ab:	c3                   	ret
   1803500ac:	e8 9f 00 f0 ff       	call   0x180250150
   1803500b1:	cc                   	int3
