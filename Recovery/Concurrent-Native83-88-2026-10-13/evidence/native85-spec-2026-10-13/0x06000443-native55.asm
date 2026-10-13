
/workspaces/GodsPVZ-native24-codespace/.validation/native55-2026-10-02/native/0x06000443.bin:     file format binary


Disassembly of section .data:

0000000180360d00 <.data>:
   180360d00:	48 89 5c 24 10       	mov    %rbx,0x10(%rsp)
   180360d05:	48 89 6c 24 18       	mov    %rbp,0x18(%rsp)
   180360d0a:	56                   	push   %rsi
   180360d0b:	57                   	push   %rdi
   180360d0c:	41 56                	push   %r14
   180360d0e:	48 83 ec 40          	sub    $0x40,%rsp
   180360d12:	80 3d 43 40 98 01 00 	cmpb   $0x0,0x1984043(%rip)        # 0x181ce4d5c
   180360d19:	41 8b f9             	mov    %r9d,%edi
   180360d1c:	4d 8b f0             	mov    %r8,%r14
   180360d1f:	8b da                	mov    %edx,%ebx
   180360d21:	48 8b f1             	mov    %rcx,%rsi
   180360d24:	75 13                	jne    0x180360d39
   180360d26:	48 8d 0d fb 9b 83 01 	lea    0x1839bfb(%rip),%rcx        # 0x181b9a928
   180360d2d:	e8 be f1 ee ff       	call   0x18024fef0
   180360d32:	c6 05 23 40 98 01 01 	movb   $0x1,0x1984023(%rip)        # 0x181ce4d5c
   180360d39:	48 8b 0d e8 9b 83 01 	mov    0x1839be8(%rip),%rcx        # 0x181b9a928
   180360d40:	0f 29 74 24 30       	movaps %xmm6,0x30(%rsp)
   180360d45:	e8 b6 f3 ee ff       	call   0x180250100
   180360d4a:	33 d2                	xor    %edx,%edx
   180360d4c:	48 8b c8             	mov    %rax,%rcx
   180360d4f:	48 8b e8             	mov    %rax,%rbp
   180360d52:	e8 79 0f fb ff       	call   0x180311cd0
   180360d57:	48 85 ed             	test   %rbp,%rbp
   180360d5a:	0f 84 9d 01 00 00    	je     0x180360efd
   180360d60:	8b 8e fc 00 00 00    	mov    0xfc(%rsi),%ecx
   180360d66:	48 8d 54 24 60       	lea    0x60(%rsp),%rdx
   180360d6b:	89 4d 14             	mov    %ecx,0x14(%rbp)
   180360d6e:	44 8b cf             	mov    %edi,%r9d
   180360d71:	48 89 6c 24 60       	mov    %rbp,0x60(%rsp)
   180360d76:	44 8b c3             	mov    %ebx,%r8d
   180360d79:	33 ed                	xor    %ebp,%ebp
   180360d7b:	48 8b ce             	mov    %rsi,%rcx
   180360d7e:	48 89 6c 24 20       	mov    %rbp,0x20(%rsp)
   180360d83:	e8 78 fd ff ff       	call   0x180360b00
   180360d88:	4d 8b ce             	mov    %r14,%r9
   180360d8b:	48 89 6c 24 28       	mov    %rbp,0x28(%rsp)
   180360d90:	44 8b c3             	mov    %ebx,%r8d
   180360d93:	89 7c 24 20          	mov    %edi,0x20(%rsp)
   180360d97:	48 8d 54 24 60       	lea    0x60(%rsp),%rdx
   180360d9c:	48 8b ce             	mov    %rsi,%rcx
   180360d9f:	e8 3c fb ff ff       	call   0x1803608e0
   180360da4:	33 d2                	xor    %edx,%edx
   180360da6:	48 8b ce             	mov    %rsi,%rcx
   180360da9:	e8 d2 f4 ff ff       	call   0x180360280
   180360dae:	8b 46 60             	mov    0x60(%rsi),%eax
   180360db1:	0f 28 f0             	movaps %xmm0,%xmm6
   180360db4:	83 c0 fd             	add    $0xfffffffd,%eax
   180360db7:	83 f8 13             	cmp    $0x13,%eax
   180360dba:	0f 87 85 00 00 00    	ja     0x180360e45
   180360dc0:	48 8d 15 39 f2 c9 ff 	lea    -0x360dc7(%rip),%rdx        # 0x180000000
   180360dc7:	48 98                	cltq
   180360dc9:	8b 8c 82 04 0f 36 00 	mov    0x360f04(%rdx,%rax,4),%ecx
   180360dd0:	48 03 ca             	add    %rdx,%rcx
   180360dd3:	ff e1                	jmp    *%rcx
   180360dd5:	85 db                	test   %ebx,%ebx
   180360dd7:	78 6c                	js     0x180360e45
   180360dd9:	f3 0f 59 35 03 6e 24 	mulss  0x1246e03(%rip),%xmm6        # 0x1815a7be4
   180360de0:	01 
   180360de1:	eb 62                	jmp    0x180360e45
   180360de3:	f3 0f 59 35 fd 6d 24 	mulss  0x1246dfd(%rip),%xmm6        # 0x1815a7be8
   180360dea:	01 
   180360deb:	eb 58                	jmp    0x180360e45
   180360ded:	33 d2                	xor    %edx,%edx
   180360def:	48 8b ce             	mov    %rsi,%rcx
   180360df2:	e8 89 05 00 00       	call   0x180361380
   180360df7:	f3 0f 59 c6          	mulss  %xmm6,%xmm0
   180360dfb:	0f 28 f0             	movaps %xmm0,%xmm6
   180360dfe:	f3 0f 58 f6          	addss  %xmm6,%xmm6
   180360e02:	eb 41                	jmp    0x180360e45
   180360e04:	85 db                	test   %ebx,%ebx
   180360e06:	79 0a                	jns    0x180360e12
   180360e08:	f3 0f 59 35 00 6f 24 	mulss  0x1246f00(%rip),%xmm6        # 0x1815a7d10
   180360e0f:	01 
   180360e10:	eb 33                	jmp    0x180360e45
   180360e12:	f3 0f 59 35 2a 6e 24 	mulss  0x1246e2a(%rip),%xmm6        # 0x1815a7c44
   180360e19:	01 
   180360e1a:	eb 29                	jmp    0x180360e45
   180360e1c:	85 db                	test   %ebx,%ebx
   180360e1e:	78 25                	js     0x180360e45
   180360e20:	f3 0f 59 35 b8 6d 24 	mulss  0x1246db8(%rip),%xmm6        # 0x1815a7be0
   180360e27:	01 
   180360e28:	eb 1b                	jmp    0x180360e45
   180360e2a:	85 ff                	test   %edi,%edi
   180360e2c:	75 0a                	jne    0x180360e38
   180360e2e:	f3 0f 59 35 d2 6b 24 	mulss  0x1246bd2(%rip),%xmm6        # 0x1815a7a08
   180360e35:	01 
   180360e36:	eb 0d                	jmp    0x180360e45
   180360e38:	83 ff 01             	cmp    $0x1,%edi
   180360e3b:	75 08                	jne    0x180360e45
   180360e3d:	f3 0f 59 35 bf 6c 24 	mulss  0x1246cbf(%rip),%xmm6        # 0x1815a7b04
   180360e44:	01 
   180360e45:	48 8b 7c 24 60       	mov    0x60(%rsp),%rdi
   180360e4a:	48 85 ff             	test   %rdi,%rdi
   180360e4d:	0f 84 aa 00 00 00    	je     0x180360efd
   180360e53:	40 38 2d 03 3f 98 01 	cmp    %bpl,0x1983f03(%rip)        # 0x181ce4d5d
   180360e5a:	f3 0f 11 77 10       	movss  %xmm6,0x10(%rdi)
   180360e5f:	75 13                	jne    0x180360e74
   180360e61:	48 8d 0d 00 d3 83 01 	lea    0x183d300(%rip),%rcx        # 0x181b9e168
   180360e68:	e8 83 f0 ee ff       	call   0x18024fef0
   180360e6d:	c6 05 e9 3e 98 01 01 	movb   $0x1,0x1983ee9(%rip)        # 0x181ce4d5d
   180360e74:	8b 46 60             	mov    0x60(%rsi),%eax
   180360e77:	83 f8 07             	cmp    $0x7,%eax
   180360e7a:	74 05                	je     0x180360e81
   180360e7c:	83 f8 14             	cmp    $0x14,%eax
   180360e7f:	75 40                	jne    0x180360ec1
   180360e81:	85 db                	test   %ebx,%ebx
   180360e83:	78 3c                	js     0x180360ec1
   180360e85:	48 8b 0d dc d2 83 01 	mov    0x183d2dc(%rip),%rcx        # 0x181b9e168
   180360e8c:	e8 6f f2 ee ff       	call   0x180250100
   180360e91:	f3 0f 10 15 a7 6c 24 	movss  0x1246ca7(%rip),%xmm2        # 0x1815a7b40
   180360e98:	01 
   180360e99:	4c 8b cf             	mov    %rdi,%r9
   180360e9c:	33 d2                	xor    %edx,%edx
   180360e9e:	48 89 6c 24 28       	mov    %rbp,0x28(%rsp)
   180360ea3:	48 8b c8             	mov    %rax,%rcx
   180360ea6:	c6 44 24 20 01       	movb   $0x1,0x20(%rsp)
   180360eab:	48 8b d8             	mov    %rax,%rbx
   180360eae:	e8 8d 5a fb ff       	call   0x180316940
   180360eb3:	45 33 c0             	xor    %r8d,%r8d
   180360eb6:	48 8b d3             	mov    %rbx,%rdx
   180360eb9:	48 8b cf             	mov    %rdi,%rcx
   180360ebc:	e8 ff fe fa ff       	call   0x180310dc0
   180360ec1:	48 8d 4f 58          	lea    0x58(%rdi),%rcx
   180360ec5:	48 89 77 58          	mov    %rsi,0x58(%rdi)
   180360ec9:	48 8b d6             	mov    %rsi,%rdx
   180360ecc:	e8 8f e4 ee ff       	call   0x18024f360
   180360ed1:	48 8b 56 20          	mov    0x20(%rsi),%rdx
   180360ed5:	48 8d 4f 68          	lea    0x68(%rdi),%rcx
   180360ed9:	48 89 57 68          	mov    %rdx,0x68(%rdi)
   180360edd:	e8 7e e4 ee ff       	call   0x18024f360
   180360ee2:	0f 28 74 24 30       	movaps 0x30(%rsp),%xmm6
   180360ee7:	48 8b c7             	mov    %rdi,%rax
   180360eea:	48 8b 5c 24 68       	mov    0x68(%rsp),%rbx
   180360eef:	48 8b 6c 24 70       	mov    0x70(%rsp),%rbp
   180360ef4:	48 83 c4 40          	add    $0x40,%rsp
   180360ef8:	41 5e                	pop    %r14
   180360efa:	5f                   	pop    %rdi
   180360efb:	5e                   	pop    %rsi
   180360efc:	c3                   	ret
   180360efd:	e8 4e f2 ee ff       	call   0x180250150
   180360f02:	cc                   	int3
   180360f03:	90                   	nop
   180360f04:	d5 0d 36             	{rex2 0xd} (bad)
   180360f07:	00 45 0e             	add    %al,0xe(%rbp)
   180360f0a:	36 00 45 0e          	ss add %al,0xe(%rbp)
   180360f0e:	36 00 45 0e          	ss add %al,0xe(%rbp)
   180360f12:	36 00 45 0e          	ss add %al,0xe(%rbp)
   180360f16:	36 00 45 0e          	ss add %al,0xe(%rbp)
   180360f1a:	36 00 45 0e          	ss add %al,0xe(%rbp)
   180360f1e:	36 00 45 0e          	ss add %al,0xe(%rbp)
   180360f22:	36 00 e3             	ss add %ah,%bl
   180360f25:	0d 36 00 45 0e       	or     $0xe450036,%eax
   180360f2a:	36 00 ed             	ss add %ch,%ch
   180360f2d:	0d 36 00 45 0e       	or     $0xe450036,%eax
   180360f32:	36 00 45 0e          	ss add %al,0xe(%rbp)
   180360f36:	36 00 04 0e          	ss add %al,(%rsi,%rcx,1)
   180360f3a:	36 00 1c 0e          	ss add %bl,(%rsi,%rcx,1)
   180360f3e:	36 00 2a             	ss add %ch,(%rdx)
   180360f41:	0e                   	(bad)
   180360f42:	36 00 45 0e          	ss add %al,0xe(%rbp)
   180360f46:	36 00 45 0e          	ss add %al,0xe(%rbp)
   180360f4a:	36 00 45 0e          	ss add %al,0xe(%rbp)
   180360f4e:	36 00 45 0e          	ss add %al,0xe(%rbp)
   180360f52:	36                   	ss
	...
