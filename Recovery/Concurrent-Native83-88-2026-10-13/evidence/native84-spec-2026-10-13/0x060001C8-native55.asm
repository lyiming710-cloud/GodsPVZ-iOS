
/workspaces/GodsPVZ-native24-codespace/.validation/native55-2026-10-02/native/0x060001C8.bin:     file format binary


Disassembly of section .data:

000000018031d7e0 <.data>:
   18031d7e0:	40 53                	rex push %rbx
   18031d7e2:	48 83 ec 20          	sub    $0x20,%rsp
   18031d7e6:	80 3d d9 73 9c 01 00 	cmpb   $0x0,0x19c73d9(%rip)        # 0x181ce4bc6
   18031d7ed:	48 8b d9             	mov    %rcx,%rbx
   18031d7f0:	75 13                	jne    0x18031d805
   18031d7f2:	48 8d 0d 07 2d 89 01 	lea    0x1892d07(%rip),%rcx        # 0x181bb0500
   18031d7f9:	e8 f2 26 f3 ff       	call   0x18024fef0
   18031d7fe:	c6 05 c1 73 9c 01 01 	movb   $0x1,0x19c73c1(%rip)        # 0x181ce4bc6
   18031d805:	48 63 43 40          	movslq 0x40(%rbx),%rax
   18031d809:	83 f8 06             	cmp    $0x6,%eax
   18031d80c:	0f 87 b3 01 00 00    	ja     0x18031d9c5
   18031d812:	48 8d 15 e7 27 ce ff 	lea    -0x31d819(%rip),%rdx        # 0x180000000
   18031d819:	48 89 7c 24 30       	mov    %rdi,0x30(%rsp)
   18031d81e:	8b 8c 82 d4 d9 31 00 	mov    0x31d9d4(%rdx,%rax,4),%ecx
   18031d825:	48 03 ca             	add    %rdx,%rcx
   18031d828:	ff e1                	jmp    *%rcx
   18031d82a:	48 8b 43 28          	mov    0x28(%rbx),%rax
   18031d82e:	48 85 c0             	test   %rax,%rax
   18031d831:	0f 84 94 01 00 00    	je     0x18031d9cb
   18031d837:	80 b8 d0 00 00 00 00 	cmpb   $0x0,0xd0(%rax)
   18031d83e:	0f 84 7c 01 00 00    	je     0x18031d9c0
   18031d844:	41 b0 01             	mov    $0x1,%r8b
   18031d847:	45 33 c9             	xor    %r9d,%r9d
   18031d84a:	41 0f b6 d0          	movzbl %r8b,%edx
   18031d84e:	48 8b c8             	mov    %rax,%rcx
   18031d851:	48 8b 7c 24 30       	mov    0x30(%rsp),%rdi
   18031d856:	48 83 c4 20          	add    $0x20,%rsp
   18031d85a:	5b                   	pop    %rbx
   18031d85b:	e9 80 8d 00 00       	jmp    0x1803265e0
   18031d860:	33 d2                	xor    %edx,%edx
   18031d862:	48 8b cb             	mov    %rbx,%rcx
   18031d865:	48 8b 7c 24 30       	mov    0x30(%rsp),%rdi
   18031d86a:	48 83 c4 20          	add    $0x20,%rsp
   18031d86e:	5b                   	pop    %rbx
   18031d86f:	e9 8c fc ff ff       	jmp    0x18031d500
   18031d874:	48 8b 0d 85 2c 89 01 	mov    0x1892c85(%rip),%rcx        # 0x181bb0500
   18031d87b:	48 8b bb 90 00 00 00 	mov    0x90(%rbx),%rdi
   18031d882:	83 b9 e0 00 00 00 00 	cmpl   $0x0,0xe0(%rcx)
   18031d889:	75 05                	jne    0x18031d890
   18031d88b:	e8 50 29 f3 ff       	call   0x1802501e0
   18031d890:	33 d2                	xor    %edx,%edx
   18031d892:	48 8b cf             	mov    %rdi,%rcx
   18031d895:	e8 d6 1f 00 01       	call   0x18131f870
   18031d89a:	84 c0                	test   %al,%al
   18031d89c:	74 5e                	je     0x18031d8fc
   18031d89e:	48 8b 83 a8 00 00 00 	mov    0xa8(%rbx),%rax
   18031d8a5:	48 85 c0             	test   %rax,%rax
   18031d8a8:	0f 84 1d 01 00 00    	je     0x18031d9cb
   18031d8ae:	48 8b 48 70          	mov    0x70(%rax),%rcx
   18031d8b2:	48 85 c9             	test   %rcx,%rcx
   18031d8b5:	0f 84 10 01 00 00    	je     0x18031d9cb
   18031d8bb:	33 d2                	xor    %edx,%edx
   18031d8bd:	e8 8e 6a 07 00       	call   0x180394350
   18031d8c2:	48 8b 0d 37 2c 89 01 	mov    0x1892c37(%rip),%rcx        # 0x181bb0500
   18031d8c9:	48 8b bb 90 00 00 00 	mov    0x90(%rbx),%rdi
   18031d8d0:	83 b9 e0 00 00 00 00 	cmpl   $0x0,0xe0(%rcx)
   18031d8d7:	75 05                	jne    0x18031d8de
   18031d8d9:	e8 02 29 f3 ff       	call   0x1802501e0
   18031d8de:	33 d2                	xor    %edx,%edx
   18031d8e0:	48 8b cf             	mov    %rdi,%rcx
   18031d8e3:	e8 58 10 00 01       	call   0x18131e940
   18031d8e8:	48 8b 4b 28          	mov    0x28(%rbx),%rcx
   18031d8ec:	48 85 c9             	test   %rcx,%rcx
   18031d8ef:	0f 84 d6 00 00 00    	je     0x18031d9cb
   18031d8f5:	33 d2                	xor    %edx,%edx
   18031d8f7:	e8 94 88 00 00       	call   0x180326190
   18031d8fc:	48 8b 43 28          	mov    0x28(%rbx),%rax
   18031d900:	48 85 c0             	test   %rax,%rax
   18031d903:	0f 84 c2 00 00 00    	je     0x18031d9cb
   18031d909:	80 b8 d0 00 00 00 00 	cmpb   $0x0,0xd0(%rax)
   18031d910:	0f 84 a0 00 00 00    	je     0x18031d9b6
   18031d916:	41 b0 01             	mov    $0x1,%r8b
   18031d919:	45 33 c9             	xor    %r9d,%r9d
   18031d91c:	41 0f b6 d0          	movzbl %r8b,%edx
   18031d920:	48 8b c8             	mov    %rax,%rcx
   18031d923:	e8 b8 8c 00 00       	call   0x1803265e0
   18031d928:	e9 89 00 00 00       	jmp    0x18031d9b6
   18031d92d:	48 8b 0d cc 2b 89 01 	mov    0x1892bcc(%rip),%rcx        # 0x181bb0500
   18031d934:	48 8b bb b0 00 00 00 	mov    0xb0(%rbx),%rdi
   18031d93b:	83 b9 e0 00 00 00 00 	cmpl   $0x0,0xe0(%rcx)
   18031d942:	75 05                	jne    0x18031d949
   18031d944:	e8 97 28 f3 ff       	call   0x1802501e0
   18031d949:	45 33 c0             	xor    %r8d,%r8d
   18031d94c:	33 d2                	xor    %edx,%edx
   18031d94e:	48 8b cf             	mov    %rdi,%rcx
   18031d951:	e8 aa 1f 00 01       	call   0x18131f900
   18031d956:	84 c0                	test   %al,%al
   18031d958:	74 5c                	je     0x18031d9b6
   18031d95a:	48 8b 8b b0 00 00 00 	mov    0xb0(%rbx),%rcx
   18031d961:	48 85 c9             	test   %rcx,%rcx
   18031d964:	74 65                	je     0x18031d9cb
   18031d966:	33 d2                	xor    %edx,%edx
   18031d968:	e8 b3 09 06 00       	call   0x18037e320
   18031d96d:	eb 47                	jmp    0x18031d9b6
   18031d96f:	48 8b 0d 8a 2b 89 01 	mov    0x1892b8a(%rip),%rcx        # 0x181bb0500
   18031d976:	48 8b bb b0 00 00 00 	mov    0xb0(%rbx),%rdi
   18031d97d:	83 b9 e0 00 00 00 00 	cmpl   $0x0,0xe0(%rcx)
   18031d984:	75 05                	jne    0x18031d98b
   18031d986:	e8 55 28 f3 ff       	call   0x1802501e0
   18031d98b:	33 d2                	xor    %edx,%edx
   18031d98d:	48 8b cf             	mov    %rdi,%rcx
   18031d990:	e8 db 1e 00 01       	call   0x18131f870
   18031d995:	84 c0                	test   %al,%al
   18031d997:	74 13                	je     0x18031d9ac
   18031d999:	48 8b 8b b0 00 00 00 	mov    0xb0(%rbx),%rcx
   18031d9a0:	48 85 c9             	test   %rcx,%rcx
   18031d9a3:	74 26                	je     0x18031d9cb
   18031d9a5:	33 d2                	xor    %edx,%edx
   18031d9a7:	e8 74 09 06 00       	call   0x18037e320
   18031d9ac:	33 d2                	xor    %edx,%edx
   18031d9ae:	48 8b cb             	mov    %rbx,%rcx
   18031d9b1:	e8 3a 00 00 00       	call   0x18031d9f0
   18031d9b6:	33 d2                	xor    %edx,%edx
   18031d9b8:	48 8b cb             	mov    %rbx,%rcx
   18031d9bb:	e8 20 36 00 00       	call   0x180320fe0
   18031d9c0:	48 8b 7c 24 30       	mov    0x30(%rsp),%rdi
   18031d9c5:	48 83 c4 20          	add    $0x20,%rsp
   18031d9c9:	5b                   	pop    %rbx
   18031d9ca:	c3                   	ret
   18031d9cb:	e8 80 27 f3 ff       	call   0x180250150
   18031d9d0:	cc                   	int3
   18031d9d1:	0f 1f 00             	nopl   (%rax)
   18031d9d4:	2a d8                	sub    %al,%bl
   18031d9d6:	31 00                	xor    %eax,(%rax)
   18031d9d8:	60                   	(bad)
   18031d9d9:	d8 31                	fdivs  (%rcx)
   18031d9db:	00 c0                	add    %al,%al
   18031d9dd:	d9 31                	fnstenv (%rcx)
   18031d9df:	00 c0                	add    %al,%al
   18031d9e1:	d9 31                	fnstenv (%rcx)
   18031d9e3:	00 74 d8 31          	add    %dh,0x31(%rax,%rbx,8)
   18031d9e7:	00 2d d9 31 00 6f    	add    %ch,0x6f0031d9(%rip)        # 0x1ef320bc6
   18031d9ed:	d9 31                	fnstenv (%rcx)
	...
