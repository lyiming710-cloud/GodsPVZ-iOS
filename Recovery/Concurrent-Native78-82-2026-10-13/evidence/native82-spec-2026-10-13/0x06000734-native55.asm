
/workspaces/GodsPVZ-native24-codespace/.validation/native55-2026-10-02/native/0x06000734.bin:     file format binary


Disassembly of section .data:

00000001803b7560 <.data>:
   1803b7560:	40 57                	rex push %rdi
   1803b7562:	48 83 ec 20          	sub    $0x20,%rsp
   1803b7566:	80 3d e1 d9 92 01 00 	cmpb   $0x0,0x192d9e1(%rip)        # 0x181ce4f4e
   1803b756d:	48 8b f9             	mov    %rcx,%rdi
   1803b7570:	75 43                	jne    0x1803b75b5
   1803b7572:	48 8d 0d 9f e2 7e 01 	lea    0x17ee29f(%rip),%rcx        # 0x181ba5818
   1803b7579:	e8 72 89 e9 ff       	call   0x18024fef0
   1803b757e:	48 8d 0d b3 dd 7e 01 	lea    0x17eddb3(%rip),%rcx        # 0x181ba5338
   1803b7585:	e8 66 89 e9 ff       	call   0x18024fef0
   1803b758a:	48 8d 0d 6f 8f 7f 01 	lea    0x17f8f6f(%rip),%rcx        # 0x181bb0500
   1803b7591:	e8 5a 89 e9 ff       	call   0x18024fef0
   1803b7596:	48 8d 0d 93 bb 7e 01 	lea    0x17ebb93(%rip),%rcx        # 0x181ba3130
   1803b759d:	e8 4e 89 e9 ff       	call   0x18024fef0
   1803b75a2:	48 8d 0d 47 87 80 01 	lea    0x1808747(%rip),%rcx        # 0x181bbfcf0
   1803b75a9:	e8 42 89 e9 ff       	call   0x18024fef0
   1803b75ae:	c6 05 99 d9 92 01 01 	movb   $0x1,0x192d999(%rip)        # 0x181ce4f4e
   1803b75b5:	33 d2                	xor    %edx,%edx
   1803b75b7:	48 89 5c 24 30       	mov    %rbx,0x30(%rsp)
   1803b75bc:	48 8b cf             	mov    %rdi,%rcx
   1803b75bf:	e8 0c cf f4 00       	call   0x1813044d0
   1803b75c4:	48 85 c0             	test   %rax,%rax
   1803b75c7:	0f 84 3c 01 00 00    	je     0x1803b7709
   1803b75cd:	48 8b 15 64 dd 7e 01 	mov    0x17edd64(%rip),%rdx        # 0x181ba5338
   1803b75d4:	48 8b c8             	mov    %rax,%rcx
   1803b75d7:	e8 a4 66 0b 00       	call   0x18046dc80
   1803b75dc:	48 8d 4f 48          	lea    0x48(%rdi),%rcx
   1803b75e0:	48 89 47 48          	mov    %rax,0x48(%rdi)
   1803b75e4:	48 8b d0             	mov    %rax,%rdx
   1803b75e7:	e8 74 7d e9 ff       	call   0x18024f360
   1803b75ec:	48 8b 4f 48          	mov    0x48(%rdi),%rcx
   1803b75f0:	48 85 c9             	test   %rcx,%rcx
   1803b75f3:	0f 84 10 01 00 00    	je     0x1803b7709
   1803b75f9:	33 d2                	xor    %edx,%edx
   1803b75fb:	48 89 74 24 38       	mov    %rsi,0x38(%rsp)
   1803b7600:	e8 bb 8e 94 00       	call   0x180d004c0
   1803b7605:	48 8b 0d e4 86 80 01 	mov    0x18086e4(%rip),%rcx        # 0x181bbfcf0
   1803b760c:	48 8b f0             	mov    %rax,%rsi
   1803b760f:	48 8b 1d 1a bb 7e 01 	mov    0x17ebb1a(%rip),%rbx        # 0x181ba3130
   1803b7616:	83 b9 e0 00 00 00 00 	cmpl   $0x0,0xe0(%rcx)
   1803b761d:	75 05                	jne    0x1803b7624
   1803b761f:	e8 bc 8b e9 ff       	call   0x1802501e0
   1803b7624:	33 d2                	xor    %edx,%edx
   1803b7626:	48 8b cb             	mov    %rbx,%rcx
   1803b7629:	e8 02 78 91 00       	call   0x180ccee30
   1803b762e:	45 33 c0             	xor    %r8d,%r8d
   1803b7631:	48 8b d0             	mov    %rax,%rdx
   1803b7634:	48 8b ce             	mov    %rsi,%rcx
   1803b7637:	e8 14 c1 10 00       	call   0x1804c3750
   1803b763c:	48 8b 74 24 38       	mov    0x38(%rsp),%rsi
   1803b7641:	84 c0                	test   %al,%al
   1803b7643:	75 22                	jne    0x1803b7667
   1803b7645:	33 c9                	xor    %ecx,%ecx
   1803b7647:	e8 74 b1 f2 00       	call   0x1812e27c0
   1803b764c:	48 8d 4f 50          	lea    0x50(%rdi),%rcx
   1803b7650:	48 89 47 50          	mov    %rax,0x50(%rdi)
   1803b7654:	48 8b d0             	mov    %rax,%rdx
   1803b7657:	e8 04 7d e9 ff       	call   0x18024f360
   1803b765c:	48 8b 5c 24 30       	mov    0x30(%rsp),%rbx
   1803b7661:	48 83 c4 20          	add    $0x20,%rsp
   1803b7665:	5f                   	pop    %rdi
   1803b7666:	c3                   	ret
   1803b7667:	33 d2                	xor    %edx,%edx
   1803b7669:	48 8b cf             	mov    %rdi,%rcx
   1803b766c:	e8 5f ce f4 00       	call   0x1813044d0
   1803b7671:	48 85 c0             	test   %rax,%rax
   1803b7674:	0f 84 8f 00 00 00    	je     0x1803b7709
   1803b767a:	48 8b 15 97 e1 7e 01 	mov    0x17ee197(%rip),%rdx        # 0x181ba5818
   1803b7681:	48 8b c8             	mov    %rax,%rcx
   1803b7684:	e8 37 63 0b 00       	call   0x18046d9c0
   1803b7689:	48 8d 4f 58          	lea    0x58(%rdi),%rcx
   1803b768d:	48 89 47 58          	mov    %rax,0x58(%rdi)
   1803b7691:	48 8b d0             	mov    %rax,%rdx
   1803b7694:	e8 c7 7c e9 ff       	call   0x18024f360
   1803b7699:	48 8b 0d 60 8e 7f 01 	mov    0x17f8e60(%rip),%rcx        # 0x181bb0500
   1803b76a0:	48 8b 5f 58          	mov    0x58(%rdi),%rbx
   1803b76a4:	83 b9 e0 00 00 00 00 	cmpl   $0x0,0xe0(%rcx)
   1803b76ab:	75 05                	jne    0x1803b76b2
   1803b76ad:	e8 2e 8b e9 ff       	call   0x1802501e0
   1803b76b2:	45 33 c0             	xor    %r8d,%r8d
   1803b76b5:	33 d2                	xor    %edx,%edx
   1803b76b7:	48 8b cb             	mov    %rbx,%rcx
   1803b76ba:	e8 41 82 f6 00       	call   0x18131f900
   1803b76bf:	84 c0                	test   %al,%al
   1803b76c1:	74 99                	je     0x1803b765c
   1803b76c3:	48 8b 4f 58          	mov    0x58(%rdi),%rcx
   1803b76c7:	48 85 c9             	test   %rcx,%rcx
   1803b76ca:	74 3d                	je     0x1803b7709
   1803b76cc:	33 d2                	xor    %edx,%edx
   1803b76ce:	e8 2d ee 18 01       	call   0x181546500
   1803b76d3:	85 c0                	test   %eax,%eax
   1803b76d5:	74 15                	je     0x1803b76ec
   1803b76d7:	48 8b 4f 58          	mov    0x58(%rdi),%rcx
   1803b76db:	48 85 c9             	test   %rcx,%rcx
   1803b76de:	74 29                	je     0x1803b7709
   1803b76e0:	33 d2                	xor    %edx,%edx
   1803b76e2:	e8 79 f0 18 01       	call   0x181546760
   1803b76e7:	e9 60 ff ff ff       	jmp    0x1803b764c
   1803b76ec:	48 8d 4f 50          	lea    0x50(%rdi),%rcx
   1803b76f0:	48 c7 47 50 00 00 00 	movq   $0x0,0x50(%rdi)
   1803b76f7:	00 
   1803b76f8:	33 d2                	xor    %edx,%edx
   1803b76fa:	48 8b 5c 24 30       	mov    0x30(%rsp),%rbx
   1803b76ff:	48 83 c4 20          	add    $0x20,%rsp
   1803b7703:	5f                   	pop    %rdi
   1803b7704:	e9 57 7c e9 ff       	jmp    0x18024f360
   1803b7709:	e8 42 8a e9 ff       	call   0x180250150
   1803b770e:	cc                   	int3
