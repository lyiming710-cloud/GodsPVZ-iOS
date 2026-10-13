
/workspaces/GodsPVZ-native24-codespace/.validation/native55-2026-10-02/native/0x060001AC.bin:     file format binary


Disassembly of section .data:

000000018031a490 <.data>:
   18031a490:	48 89 5c 24 08       	mov    %rbx,0x8(%rsp)
   18031a495:	57                   	push   %rdi
   18031a496:	48 83 ec 70          	sub    $0x70,%rsp
   18031a49a:	0f 29 74 24 60       	movaps %xmm6,0x60(%rsp)
   18031a49f:	0f 28 f2             	movaps %xmm2,%xmm6
   18031a4a2:	48 8b da             	mov    %rdx,%rbx
   18031a4a5:	80 3d 01 a7 9c 01 00 	cmpb   $0x0,0x19ca701(%rip)        # 0x181ce4bad
   18031a4ac:	75 37                	jne    0x18031a4e5
   18031a4ae:	48 8d 0d ab 05 8b 01 	lea    0x18b05ab(%rip),%rcx        # 0x181bcaa60
   18031a4b5:	e8 36 5a f3 ff       	call   0x18024fef0
   18031a4ba:	48 8d 0d ef 05 8b 01 	lea    0x18b05ef(%rip),%rcx        # 0x181bcaab0
   18031a4c1:	e8 2a 5a f3 ff       	call   0x18024fef0
   18031a4c6:	48 8d 0d 3b 06 8b 01 	lea    0x18b063b(%rip),%rcx        # 0x181bcab08
   18031a4cd:	e8 1e 5a f3 ff       	call   0x18024fef0
   18031a4d2:	48 8d 0d 87 96 8a 01 	lea    0x18a9687(%rip),%rcx        # 0x181bc3b60
   18031a4d9:	e8 12 5a f3 ff       	call   0x18024fef0
   18031a4de:	c6 05 c8 a6 9c 01 01 	movb   $0x1,0x19ca6c8(%rip)        # 0x181ce4bad
   18031a4e5:	45 33 c0             	xor    %r8d,%r8d
   18031a4e8:	0f 28 ce             	movaps %xmm6,%xmm1
   18031a4eb:	0f 57 c0             	xorps  %xmm0,%xmm0
   18031a4ee:	e8 5d 2e ff 00       	call   0x18130d350
   18031a4f3:	0f 28 f0             	movaps %xmm0,%xmm6
   18031a4f6:	48 85 db             	test   %rbx,%rbx
   18031a4f9:	0f 84 d3 00 00 00    	je     0x18031a5d2
   18031a4ff:	4c 8b 05 5a 96 8a 01 	mov    0x18a965a(%rip),%r8        # 0x181bc3b60
   18031a506:	48 8b d3             	mov    %rbx,%rdx
   18031a509:	48 8d 4c 24 28       	lea    0x28(%rsp),%rcx
   18031a50e:	e8 dd f2 4f 00       	call   0x1808197f0
   18031a513:	0f 10 4c 24 28       	movups 0x28(%rsp),%xmm1
   18031a518:	0f 11 4c 24 40       	movups %xmm1,0x40(%rsp)
   18031a51d:	f2 0f 10 54 24 38    	movsd  0x38(%rsp),%xmm2
   18031a523:	f2 0f 11 54 24 50    	movsd  %xmm2,0x50(%rsp)
   18031a529:	48 c7 44 24 28 00 00 	movq   $0x0,0x28(%rsp)
   18031a530:	00 00 
   18031a532:	48 8d 5c 24 40       	lea    0x40(%rsp),%rbx
   18031a537:	48 89 5c 24 30       	mov    %rbx,0x30(%rsp)
   18031a53c:	0f 1f 40 00          	nopl   0x0(%rax)
   18031a540:	48 8b 15 69 05 8b 01 	mov    0x18b0569(%rip),%rdx        # 0x181bcaab0
   18031a547:	48 8d 4c 24 40       	lea    0x40(%rsp),%rcx
   18031a54c:	e8 2f ed 34 00       	call   0x180669280
   18031a551:	84 c0                	test   %al,%al
   18031a553:	74 3c                	je     0x18031a591
   18031a555:	48 8b 7c 24 50       	mov    0x50(%rsp),%rdi
   18031a55a:	48 85 ff             	test   %rdi,%rdi
   18031a55d:	74 79                	je     0x18031a5d8
   18031a55f:	0f 2f 77 1c          	comiss 0x1c(%rdi),%xmm6
   18031a563:	77 25                	ja     0x18031a58a
   18031a565:	48 8b 15 f4 04 8b 01 	mov    0x18b04f4(%rip),%rdx        # 0x181bcaa60
   18031a56c:	48 8b cb             	mov    %rbx,%rcx
   18031a56f:	e8 fc 7b fe ff       	call   0x180302170
   18031a574:	48 8b c7             	mov    %rdi,%rax
   18031a577:	48 8b 9c 24 80 00 00 	mov    0x80(%rsp),%rbx
   18031a57e:	00 
   18031a57f:	0f 28 74 24 60       	movaps 0x60(%rsp),%xmm6
   18031a584:	48 83 c4 70          	add    $0x70,%rsp
   18031a588:	5f                   	pop    %rdi
   18031a589:	c3                   	ret
   18031a58a:	f3 0f 5c 77 1c       	subss  0x1c(%rdi),%xmm6
   18031a58f:	eb af                	jmp    0x18031a540
   18031a591:	48 8b 15 c8 04 8b 01 	mov    0x18b04c8(%rip),%rdx        # 0x181bcaa60
   18031a598:	48 8b cb             	mov    %rbx,%rcx
   18031a59b:	e8 d0 7b fe ff       	call   0x180302170
   18031a5a0:	eb 1b                	jmp    0x18031a5bd
   18031a5a2:	48 8b 15 b7 04 8b 01 	mov    0x18b04b7(%rip),%rdx        # 0x181bcaa60
   18031a5a9:	48 8b 4c 24 30       	mov    0x30(%rsp),%rcx
   18031a5ae:	e8 bd 7b fe ff       	call   0x180302170
   18031a5b3:	48 8b 4c 24 28       	mov    0x28(%rsp),%rcx
   18031a5b8:	48 85 c9             	test   %rcx,%rcx
   18031a5bb:	75 21                	jne    0x18031a5de
   18031a5bd:	33 c0                	xor    %eax,%eax
   18031a5bf:	48 8b 9c 24 80 00 00 	mov    0x80(%rsp),%rbx
   18031a5c6:	00 
   18031a5c7:	0f 28 74 24 60       	movaps 0x60(%rsp),%xmm6
   18031a5cc:	48 83 c4 70          	add    $0x70,%rsp
   18031a5d0:	5f                   	pop    %rdi
   18031a5d1:	c3                   	ret
   18031a5d2:	e8 79 5b f3 ff       	call   0x180250150
   18031a5d7:	90                   	nop
   18031a5d8:	e8 73 5b f3 ff       	call   0x180250150
   18031a5dd:	90                   	nop
   18031a5de:	e8 7d 68 ee ff       	call   0x180200e60
   18031a5e3:	cc                   	int3
