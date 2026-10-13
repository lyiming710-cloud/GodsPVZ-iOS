
/workspaces/GodsPVZ-native24-codespace/.validation/native55-2026-10-02/native/0x060006FE.bin:     file format binary


Disassembly of section .data:

00000001803b0320 <.data>:
   1803b0320:	40 53                	rex push %rbx
   1803b0322:	48 83 ec 20          	sub    $0x20,%rsp
   1803b0326:	48 63 41 20          	movslq 0x20(%rcx),%rax
   1803b032a:	48 8b d9             	mov    %rcx,%rbx
   1803b032d:	83 f8 05             	cmp    $0x5,%eax
   1803b0330:	77 27                	ja     0x1803b0359
   1803b0332:	48 8d 0d c7 fc c4 ff 	lea    -0x3b0339(%rip),%rcx        # 0x180000000
   1803b0339:	48 89 7c 24 30       	mov    %rdi,0x30(%rsp)
   1803b033e:	8b 94 81 6c 05 3b 00 	mov    0x3b056c(%rcx,%rax,4),%edx
   1803b0345:	48 03 d1             	add    %rcx,%rdx
   1803b0348:	ff e2                	jmp    *%rdx
   1803b034a:	33 d2                	xor    %edx,%edx
   1803b034c:	48 8b cb             	mov    %rbx,%rcx
   1803b034f:	e8 5c f6 ff ff       	call   0x1803af9b0
   1803b0354:	48 8b 7c 24 30       	mov    0x30(%rsp),%rdi
   1803b0359:	48 83 c4 20          	add    $0x20,%rsp
   1803b035d:	5b                   	pop    %rbx
   1803b035e:	c3                   	ret
   1803b035f:	33 d2                	xor    %edx,%edx
   1803b0361:	48 8b cb             	mov    %rbx,%rcx
   1803b0364:	48 8b 7c 24 30       	mov    0x30(%rsp),%rdi
   1803b0369:	48 83 c4 20          	add    $0x20,%rsp
   1803b036d:	5b                   	pop    %rbx
   1803b036e:	e9 9d f5 ff ff       	jmp    0x1803af910
   1803b0373:	80 3d be 4b 93 01 00 	cmpb   $0x0,0x1934bbe(%rip)        # 0x181ce4f38
   1803b037a:	75 2b                	jne    0x1803b03a7
   1803b037c:	48 8d 0d 95 83 80 01 	lea    0x1808395(%rip),%rcx        # 0x181bb8718
   1803b0383:	e8 68 fb e9 ff       	call   0x18024fef0
   1803b0388:	48 8d 0d e1 83 80 01 	lea    0x18083e1(%rip),%rcx        # 0x181bb8770
   1803b038f:	e8 5c fb e9 ff       	call   0x18024fef0
   1803b0394:	48 8d 0d 65 01 80 01 	lea    0x1800165(%rip),%rcx        # 0x181bb0500
   1803b039b:	e8 50 fb e9 ff       	call   0x18024fef0
   1803b03a0:	c6 05 91 4b 93 01 01 	movb   $0x1,0x1934b91(%rip)        # 0x181ce4f38
   1803b03a7:	83 7b 68 00          	cmpl   $0x0,0x68(%rbx)
   1803b03ab:	7c a7                	jl     0x1803b0354
   1803b03ad:	80 3d 7d 47 93 01 00 	cmpb   $0x0,0x193477d(%rip)        # 0x181ce4b31
   1803b03b4:	75 13                	jne    0x1803b03c9
   1803b03b6:	48 8d 0d b3 c1 7f 01 	lea    0x17fc1b3(%rip),%rcx        # 0x181bac570
   1803b03bd:	e8 2e fb e9 ff       	call   0x18024fef0
   1803b03c2:	c6 05 68 47 93 01 01 	movb   $0x1,0x1934768(%rip)        # 0x181ce4b31
   1803b03c9:	48 8b 05 a0 c1 7f 01 	mov    0x17fc1a0(%rip),%rax        # 0x181bac570
   1803b03d0:	48 8b 88 b8 00 00 00 	mov    0xb8(%rax),%rcx
   1803b03d7:	48 8b 39             	mov    (%rcx),%rdi
   1803b03da:	48 85 ff             	test   %rdi,%rdi
   1803b03dd:	0f 84 82 01 00 00    	je     0x1803b0565
   1803b03e3:	48 8b 0d 16 01 80 01 	mov    0x1800116(%rip),%rcx        # 0x181bb0500
   1803b03ea:	48 8b 7f 58          	mov    0x58(%rdi),%rdi
   1803b03ee:	83 b9 e0 00 00 00 00 	cmpl   $0x0,0xe0(%rcx)
   1803b03f5:	75 05                	jne    0x1803b03fc
   1803b03f7:	e8 e4 fd e9 ff       	call   0x1802501e0
   1803b03fc:	45 33 c0             	xor    %r8d,%r8d
   1803b03ff:	33 d2                	xor    %edx,%edx
   1803b0401:	48 8b cf             	mov    %rdi,%rcx
   1803b0404:	e8 57 f3 f6 00       	call   0x18131f760
   1803b0409:	84 c0                	test   %al,%al
   1803b040b:	0f 85 43 ff ff ff    	jne    0x1803b0354
   1803b0411:	48 85 ff             	test   %rdi,%rdi
   1803b0414:	0f 84 4b 01 00 00    	je     0x1803b0565
   1803b041a:	48 8b 4f 50          	mov    0x50(%rdi),%rcx
   1803b041e:	48 85 c9             	test   %rcx,%rcx
   1803b0421:	0f 84 3e 01 00 00    	je     0x1803b0565
   1803b0427:	8b 43 68             	mov    0x68(%rbx),%eax
   1803b042a:	39 41 18             	cmp    %eax,0x18(%rcx)
   1803b042d:	0f 8e 21 ff ff ff    	jle    0x1803b0354
   1803b0433:	4c 8b 05 36 83 80 01 	mov    0x1808336(%rip),%r8        # 0x181bb8770
   1803b043a:	8b d0                	mov    %eax,%edx
   1803b043c:	e8 cf 52 47 00       	call   0x180825710
   1803b0441:	48 8b 0d b8 00 80 01 	mov    0x18000b8(%rip),%rcx        # 0x181bb0500
   1803b0448:	48 8b f8             	mov    %rax,%rdi
   1803b044b:	83 b9 e0 00 00 00 00 	cmpl   $0x0,0xe0(%rcx)
   1803b0452:	75 05                	jne    0x1803b0459
   1803b0454:	e8 87 fd e9 ff       	call   0x1802501e0
   1803b0459:	33 d2                	xor    %edx,%edx
   1803b045b:	48 8b cf             	mov    %rdi,%rcx
   1803b045e:	e8 0d f4 f6 00       	call   0x18131f870
   1803b0463:	84 c0                	test   %al,%al
   1803b0465:	0f 84 c4 00 00 00    	je     0x1803b052f
   1803b046b:	48 85 ff             	test   %rdi,%rdi
   1803b046e:	0f 84 f1 00 00 00    	je     0x1803b0565
   1803b0474:	48 83 7f 20 00       	cmpq   $0x0,0x20(%rdi)
   1803b0479:	0f 84 b0 00 00 00    	je     0x1803b052f
   1803b047f:	8b 57 30             	mov    0x30(%rdi),%edx
   1803b0482:	45 33 c0             	xor    %r8d,%r8d
   1803b0485:	48 8b 4f 20          	mov    0x20(%rdi),%rcx
   1803b0489:	e8 c2 e1 f8 ff       	call   0x18033e650
   1803b048e:	80 3d 9c 46 93 01 00 	cmpb   $0x0,0x193469c(%rip)        # 0x181ce4b31
   1803b0495:	75 13                	jne    0x1803b04aa
   1803b0497:	48 8d 0d d2 c0 7f 01 	lea    0x17fc0d2(%rip),%rcx        # 0x181bac570
   1803b049e:	e8 4d fa e9 ff       	call   0x18024fef0
   1803b04a3:	c6 05 87 46 93 01 01 	movb   $0x1,0x1934687(%rip)        # 0x181ce4b31
   1803b04aa:	48 8b 05 bf c0 7f 01 	mov    0x17fc0bf(%rip),%rax        # 0x181bac570
   1803b04b1:	48 8b 88 b8 00 00 00 	mov    0xb8(%rax),%rcx
   1803b04b8:	48 8b 09             	mov    (%rcx),%rcx
   1803b04bb:	48 85 c9             	test   %rcx,%rcx
   1803b04be:	0f 84 a1 00 00 00    	je     0x1803b0565
   1803b04c4:	33 d2                	xor    %edx,%edx
   1803b04c6:	e8 e5 f6 fe ff       	call   0x18039fbb0
   1803b04cb:	eb 62                	jmp    0x1803b052f
   1803b04cd:	80 3d 61 4a 93 01 00 	cmpb   $0x0,0x1934a61(%rip)        # 0x181ce4f35
   1803b04d4:	75 13                	jne    0x1803b04e9
   1803b04d6:	48 8d 0d 23 00 80 01 	lea    0x1800023(%rip),%rcx        # 0x181bb0500
   1803b04dd:	e8 0e fa e9 ff       	call   0x18024fef0
   1803b04e2:	c6 05 4c 4a 93 01 01 	movb   $0x1,0x1934a4c(%rip)        # 0x181ce4f35
   1803b04e9:	80 3d 41 46 93 01 00 	cmpb   $0x0,0x1934641(%rip)        # 0x181ce4b31
   1803b04f0:	75 13                	jne    0x1803b0505
   1803b04f2:	48 8d 0d 77 c0 7f 01 	lea    0x17fc077(%rip),%rcx        # 0x181bac570
   1803b04f9:	e8 f2 f9 e9 ff       	call   0x18024fef0
   1803b04fe:	c6 05 2c 46 93 01 01 	movb   $0x1,0x193462c(%rip)        # 0x181ce4b31
   1803b0505:	48 8b 05 64 c0 7f 01 	mov    0x17fc064(%rip),%rax        # 0x181bac570
   1803b050c:	48 8b 88 b8 00 00 00 	mov    0xb8(%rax),%rcx
   1803b0513:	48 8b 01             	mov    (%rcx),%rax
   1803b0516:	48 85 c0             	test   %rax,%rax
   1803b0519:	74 4a                	je     0x1803b0565
   1803b051b:	48 8b 48 50          	mov    0x50(%rax),%rcx
   1803b051f:	48 85 c9             	test   %rcx,%rcx
   1803b0522:	0f 84 2c fe ff ff    	je     0x1803b0354
   1803b0528:	33 d2                	xor    %edx,%edx
   1803b052a:	e8 41 dd f8 ff       	call   0x18033e270
   1803b052f:	33 d2                	xor    %edx,%edx
   1803b0531:	48 8b cb             	mov    %rbx,%rcx
   1803b0534:	e8 97 3f f5 00       	call   0x1813044d0
   1803b0539:	48 8b 0d c0 ff 7f 01 	mov    0x17fffc0(%rip),%rcx        # 0x181bb0500
   1803b0540:	48 8b d8             	mov    %rax,%rbx
   1803b0543:	83 b9 e0 00 00 00 00 	cmpl   $0x0,0xe0(%rcx)
   1803b054a:	75 05                	jne    0x1803b0551
   1803b054c:	e8 8f fc e9 ff       	call   0x1802501e0
   1803b0551:	33 d2                	xor    %edx,%edx
   1803b0553:	48 8b cb             	mov    %rbx,%rcx
   1803b0556:	48 8b 7c 24 30       	mov    0x30(%rsp),%rdi
   1803b055b:	48 83 c4 20          	add    $0x20,%rsp
   1803b055f:	5b                   	pop    %rbx
   1803b0560:	e9 db e3 f6 00       	jmp    0x18131e940
   1803b0565:	e8 e6 fb e9 ff       	call   0x180250150
   1803b056a:	cc                   	int3
   1803b056b:	90                   	nop
   1803b056c:	54                   	push   %rsp
   1803b056d:	03 3b                	add    (%rbx),%edi
   1803b056f:	00 4a 03             	add    %cl,0x3(%rdx)
   1803b0572:	3b 00                	cmp    (%rax),%eax
   1803b0574:	5f                   	pop    %rdi
   1803b0575:	03 3b                	add    (%rbx),%edi
   1803b0577:	00 5f 03             	add    %bl,0x3(%rdi)
   1803b057a:	3b 00                	cmp    (%rax),%eax
   1803b057c:	73 03                	jae    0x1803b0581
   1803b057e:	3b 00                	cmp    (%rax),%eax
   1803b0580:	cd 04                	int    $0x4
   1803b0582:	3b 00                	cmp    (%rax),%eax
