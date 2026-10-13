
/workspaces/GodsPVZ-native24-codespace/.validation/native55-2026-10-02/native/0x0600075F.bin:     file format binary


Disassembly of section .data:

00000001803b4d80 <.data>:
   1803b4d80:	40 53                	rex push %rbx
   1803b4d82:	48 83 ec 40          	sub    $0x40,%rsp
   1803b4d86:	80 3d d9 01 93 01 00 	cmpb   $0x0,0x19301d9(%rip)        # 0x181ce4f66
   1803b4d8d:	48 8b d9             	mov    %rcx,%rbx
   1803b4d90:	75 1f                	jne    0x1803b4db1
   1803b4d92:	48 8d 0d 2f e1 80 01 	lea    0x180e12f(%rip),%rcx        # 0x181bc2ec8
   1803b4d99:	e8 52 b1 e9 ff       	call   0x18024fef0
   1803b4d9e:	48 8d 0d 5b b7 7f 01 	lea    0x17fb75b(%rip),%rcx        # 0x181bb0500
   1803b4da5:	e8 46 b1 e9 ff       	call   0x18024fef0
   1803b4daa:	c6 05 b5 01 93 01 01 	movb   $0x1,0x19301b5(%rip)        # 0x181ce4f66
   1803b4db1:	33 d2                	xor    %edx,%edx
   1803b4db3:	48 89 7c 24 50       	mov    %rdi,0x50(%rsp)
   1803b4db8:	48 8b cb             	mov    %rbx,%rcx
   1803b4dbb:	e8 50 f7 f4 00       	call   0x181304510
   1803b4dc0:	48 8d 4b 28          	lea    0x28(%rbx),%rcx
   1803b4dc4:	48 89 43 28          	mov    %rax,0x28(%rbx)
   1803b4dc8:	48 8b d0             	mov    %rax,%rdx
   1803b4dcb:	e8 90 a5 e9 ff       	call   0x18024f360
   1803b4dd0:	48 8b 53 28          	mov    0x28(%rbx),%rdx
   1803b4dd4:	48 85 d2             	test   %rdx,%rdx
   1803b4dd7:	0f 84 27 01 00 00    	je     0x1803b4f04
   1803b4ddd:	45 33 c0             	xor    %r8d,%r8d
   1803b4de0:	48 8d 4c 24 30       	lea    0x30(%rsp),%rcx
   1803b4de5:	e8 26 a7 f7 00       	call   0x18132f510
   1803b4dea:	45 33 c0             	xor    %r8d,%r8d
   1803b4ded:	48 8d 54 24 30       	lea    0x30(%rsp),%rdx
   1803b4df2:	48 8d 4c 24 20       	lea    0x20(%rsp),%rcx
   1803b4df7:	0f 10 00             	movups (%rax),%xmm0
   1803b4dfa:	0f 29 44 24 30       	movaps %xmm0,0x30(%rsp)
   1803b4dff:	e8 ec 79 f5 00       	call   0x18130c7f0
   1803b4e04:	f3 0f 10 0d 10 30 1f 	movss  0x11f3010(%rip),%xmm1        # 0x1815a7e1c
   1803b4e0b:	01 
   1803b4e0c:	f2 0f 10 00          	movsd  (%rax),%xmm0
   1803b4e10:	f3 0f 10 50 08       	movss  0x8(%rax),%xmm2
   1803b4e15:	0f 28 d8             	movaps %xmm0,%xmm3
   1803b4e18:	0f c6 db 55          	shufps $0x55,%xmm3,%xmm3
   1803b4e1c:	0f 28 e0             	movaps %xmm0,%xmm4
   1803b4e1f:	f3 0f 59 d9          	mulss  %xmm1,%xmm3
   1803b4e23:	f3 0f 59 e1          	mulss  %xmm1,%xmm4
   1803b4e27:	f2 0f 11 44 24 30    	movsd  %xmm0,0x30(%rsp)
   1803b4e2d:	f3 0f 59 d1          	mulss  %xmm1,%xmm2
   1803b4e31:	0f 14 e3             	unpcklps %xmm3,%xmm4
   1803b4e34:	48 8d 54 24 20       	lea    0x20(%rsp),%rdx
   1803b4e39:	45 33 c0             	xor    %r8d,%r8d
   1803b4e3c:	f2 0f 11 64 24 20    	movsd  %xmm4,0x20(%rsp)
   1803b4e42:	48 8d 4c 24 30       	lea    0x30(%rsp),%rcx
   1803b4e47:	f3 0f 11 54 24 28    	movss  %xmm2,0x28(%rsp)
   1803b4e4d:	e8 be 78 f5 00       	call   0x18130c710
   1803b4e52:	f2 0f 10 00          	movsd  (%rax),%xmm0
   1803b4e56:	8b 48 08             	mov    0x8(%rax),%ecx
   1803b4e59:	48 8b 53 28          	mov    0x28(%rbx),%rdx
   1803b4e5d:	f2 0f 11 43 40       	movsd  %xmm0,0x40(%rbx)
   1803b4e62:	89 4b 48             	mov    %ecx,0x48(%rbx)
   1803b4e65:	48 85 d2             	test   %rdx,%rdx
   1803b4e68:	0f 84 96 00 00 00    	je     0x1803b4f04
   1803b4e6e:	45 33 c0             	xor    %r8d,%r8d
   1803b4e71:	48 8d 4c 24 30       	lea    0x30(%rsp),%rcx
   1803b4e76:	e8 25 a5 f7 00       	call   0x18132f3a0
   1803b4e7b:	8b 48 08             	mov    0x8(%rax),%ecx
   1803b4e7e:	f2 0f 10 00          	movsd  (%rax),%xmm0
   1803b4e82:	f2 0f 11 43 4c       	movsd  %xmm0,0x4c(%rbx)
   1803b4e87:	89 4b 54             	mov    %ecx,0x54(%rbx)
   1803b4e8a:	48 8b cb             	mov    %rbx,%rcx
   1803b4e8d:	48 8b 15 34 e0 80 01 	mov    0x180e034(%rip),%rdx        # 0x181bc2ec8
   1803b4e94:	e8 57 9b 07 00       	call   0x18042e9f0
   1803b4e99:	48 8b 0d 60 b6 7f 01 	mov    0x17fb660(%rip),%rcx        # 0x181bb0500
   1803b4ea0:	48 8b f8             	mov    %rax,%rdi
   1803b4ea3:	83 b9 e0 00 00 00 00 	cmpl   $0x0,0xe0(%rcx)
   1803b4eaa:	75 05                	jne    0x1803b4eb1
   1803b4eac:	e8 2f b3 e9 ff       	call   0x1802501e0
   1803b4eb1:	45 33 c0             	xor    %r8d,%r8d
   1803b4eb4:	33 d2                	xor    %edx,%edx
   1803b4eb6:	48 8b cf             	mov    %rdi,%rcx
   1803b4eb9:	e8 42 aa f6 00       	call   0x18131f900
   1803b4ebe:	84 c0                	test   %al,%al
   1803b4ec0:	74 1a                	je     0x1803b4edc
   1803b4ec2:	48 85 ff             	test   %rdi,%rdi
   1803b4ec5:	74 3d                	je     0x1803b4f04
   1803b4ec7:	45 33 c0             	xor    %r8d,%r8d
   1803b4eca:	48 8d 4c 24 30       	lea    0x30(%rsp),%rcx
   1803b4ecf:	48 8b d7             	mov    %rdi,%rdx
   1803b4ed2:	e8 09 8b f3 00       	call   0x1812ed9e0
   1803b4ed7:	0f 10 00             	movups (%rax),%xmm0
   1803b4eda:	eb 08                	jmp    0x1803b4ee4
   1803b4edc:	66 0f 6f 05 fc 2f 1f 	movdqa 0x11f2ffc(%rip),%xmm0        # 0x1815a7ee0
   1803b4ee3:	01 
   1803b4ee4:	33 d2                	xor    %edx,%edx
   1803b4ee6:	66 0f 7f 44 24 30    	movdqa %xmm0,0x30(%rsp)
   1803b4eec:	48 8d 4c 24 30       	lea    0x30(%rsp),%rcx
   1803b4ef1:	e8 0a 70 fe ff       	call   0x18039bf00
   1803b4ef6:	48 8b 7c 24 50       	mov    0x50(%rsp),%rdi
   1803b4efb:	89 43 58             	mov    %eax,0x58(%rbx)
   1803b4efe:	48 83 c4 40          	add    $0x40,%rsp
   1803b4f02:	5b                   	pop    %rbx
   1803b4f03:	c3                   	ret
   1803b4f04:	e8 47 b2 e9 ff       	call   0x180250150
   1803b4f09:	cc                   	int3
