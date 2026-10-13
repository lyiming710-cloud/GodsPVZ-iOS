
/workspaces/GodsPVZ-native24-codespace/.validation/native55-2026-10-02/native/0x060003C5.bin:     file format binary


Disassembly of section .data:

0000000180377420 <.data>:
   180377420:	40 53                	rex push %rbx
   180377422:	48 83 ec 40          	sub    $0x40,%rsp
   180377426:	83 79 58 00          	cmpl   $0x0,0x58(%rcx)
   18037742a:	48 8b d9             	mov    %rcx,%rbx
   18037742d:	0f 85 84 00 00 00    	jne    0x1803774b7
   180377433:	33 d2                	xor    %edx,%edx
   180377435:	e8 d6 d0 f8 00       	call   0x181304510
   18037743a:	48 85 c0             	test   %rax,%rax
   18037743d:	0f 84 bf 01 00 00    	je     0x180377602
   180377443:	45 33 c0             	xor    %r8d,%r8d
   180377446:	48 8d 4c 24 20       	lea    0x20(%rsp),%rcx
   18037744b:	48 8b d0             	mov    %rax,%rdx
   18037744e:	e8 4d 7f fb 00       	call   0x18132f3a0
   180377453:	f3 0f 10 0d e5 05 23 	movss  0x12305e5(%rip),%xmm1        # 0x1815a7a40
   18037745a:	01 
   18037745b:	45 33 c0             	xor    %r8d,%r8d
   18037745e:	f3 0f 10 05 0a 0a 23 	movss  0x1230a0a(%rip),%xmm0        # 0x1815a7e70
   180377465:	01 
   180377466:	8b 08                	mov    (%rax),%ecx
   180377468:	89 4b 5c             	mov    %ecx,0x5c(%rbx)
   18037746b:	e8 e0 5e f9 00       	call   0x18130d350
   180377470:	f2 0f 10 4b 5c       	movsd  0x5c(%rbx),%xmm1
   180377475:	4c 8d 44 24 20       	lea    0x20(%rsp),%r8
   18037747a:	0f c6 c9 e1          	shufps $0xe1,%xmm1,%xmm1
   18037747e:	48 8d 4c 24 30       	lea    0x30(%rsp),%rcx
   180377483:	f3 0f 10 c8          	movss  %xmm0,%xmm1
   180377487:	45 33 c9             	xor    %r9d,%r9d
   18037748a:	0f c6 c9 e1          	shufps $0xe1,%xmm1,%xmm1
   18037748e:	48 8b d3             	mov    %rbx,%rdx
   180377491:	f2 0f 11 4b 5c       	movsd  %xmm1,0x5c(%rbx)
   180377496:	8b 43 64             	mov    0x64(%rbx),%eax
   180377499:	89 44 24 28          	mov    %eax,0x28(%rsp)
   18037749d:	f2 0f 11 4c 24 20    	movsd  %xmm1,0x20(%rsp)
   1803774a3:	e8 88 0a 00 00       	call   0x180377f30
   1803774a8:	f2 0f 10 00          	movsd  (%rax),%xmm0
   1803774ac:	8b 48 08             	mov    0x8(%rax),%ecx
   1803774af:	f2 0f 11 43 5c       	movsd  %xmm0,0x5c(%rbx)
   1803774b4:	89 4b 64             	mov    %ecx,0x64(%rbx)
   1803774b7:	83 7b 58 04          	cmpl   $0x4,0x58(%rbx)
   1803774bb:	0f 85 ce 00 00 00    	jne    0x18037758f
   1803774c1:	48 63 43 24          	movslq 0x24(%rbx),%rax
   1803774c5:	83 f8 08             	cmp    $0x8,%eax
   1803774c8:	0f 87 c1 00 00 00    	ja     0x18037758f
   1803774ce:	48 8d 15 2b 8b c8 ff 	lea    -0x3774d5(%rip),%rdx        # 0x180000000
   1803774d5:	8b 8c 82 08 76 37 00 	mov    0x377608(%rdx,%rax,4),%ecx
   1803774dc:	48 03 ca             	add    %rdx,%rcx
   1803774df:	ff e1                	jmp    *%rcx
   1803774e1:	48 8b 43 48          	mov    0x48(%rbx),%rax
   1803774e5:	48 85 c0             	test   %rax,%rax
   1803774e8:	0f 84 14 01 00 00    	je     0x180377602
   1803774ee:	48 8b 90 28 01 00 00 	mov    0x128(%rax),%rdx
   1803774f5:	48 85 d2             	test   %rdx,%rdx
   1803774f8:	0f 84 04 01 00 00    	je     0x180377602
   1803774fe:	45 33 c0             	xor    %r8d,%r8d
   180377501:	48 8d 4c 24 30       	lea    0x30(%rsp),%rcx
   180377506:	e8 a5 99 f8 ff       	call   0x180300eb0
   18037750b:	f2 0f 10 00          	movsd  (%rax),%xmm0
   18037750f:	8b 48 08             	mov    0x8(%rax),%ecx
   180377512:	f2 0f 11 43 5c       	movsd  %xmm0,0x5c(%rbx)
   180377517:	89 4b 64             	mov    %ecx,0x64(%rbx)
   18037751a:	f3 0f 10 43 60       	movss  0x60(%rbx),%xmm0
   18037751f:	f3 0f 58 05 bd 06 23 	addss  0x12306bd(%rip),%xmm0        # 0x1815a7be4
   180377526:	01 
   180377527:	f3 0f 11 43 60       	movss  %xmm0,0x60(%rbx)
   18037752c:	48 83 c4 40          	add    $0x40,%rsp
   180377530:	5b                   	pop    %rbx
   180377531:	c3                   	ret
   180377532:	48 8b 43 48          	mov    0x48(%rbx),%rax
   180377536:	48 85 c0             	test   %rax,%rax
   180377539:	0f 84 c3 00 00 00    	je     0x180377602
   18037753f:	48 8b 90 00 01 00 00 	mov    0x100(%rax),%rdx
   180377546:	48 85 d2             	test   %rdx,%rdx
   180377549:	0f 84 b3 00 00 00    	je     0x180377602
   18037754f:	45 33 c0             	xor    %r8d,%r8d
   180377552:	48 8d 4c 24 30       	lea    0x30(%rsp),%rcx
   180377557:	e8 04 c2 fa ff       	call   0x180323760
   18037755c:	f2 0f 10 00          	movsd  (%rax),%xmm0
   180377560:	8b 48 08             	mov    0x8(%rax),%ecx
   180377563:	f2 0f 11 43 5c       	movsd  %xmm0,0x5c(%rbx)
   180377568:	89 4b 64             	mov    %ecx,0x64(%rbx)
   18037756b:	f3 0f 10 43 60       	movss  0x60(%rbx),%xmm0
   180377570:	f3 0f 5c 05 68 06 23 	subss  0x1230668(%rip),%xmm0        # 0x1815a7be0
   180377577:	01 
   180377578:	f3 0f 11 43 60       	movss  %xmm0,0x60(%rbx)
   18037757d:	f3 0f 10 4b 5c       	movss  0x5c(%rbx),%xmm1
   180377582:	f3 0f 5c 0d 9a 08 23 	subss  0x123089a(%rip),%xmm1        # 0x1815a7e24
   180377589:	01 
   18037758a:	f3 0f 11 4b 5c       	movss  %xmm1,0x5c(%rbx)
   18037758f:	48 83 c4 40          	add    $0x40,%rsp
   180377593:	5b                   	pop    %rbx
   180377594:	c3                   	ret
   180377595:	33 c0                	xor    %eax,%eax
   180377597:	4c 8d 44 24 20       	lea    0x20(%rsp),%r8
   18037759c:	89 43 5c             	mov    %eax,0x5c(%rbx)
   18037759f:	48 8d 4c 24 30       	lea    0x30(%rsp),%rcx
   1803775a4:	48 c7 43 60 00 00 e6 	movq   $0x42e60000,0x60(%rbx)
   1803775ab:	42 
   1803775ac:	45 33 c9             	xor    %r9d,%r9d
   1803775af:	f2 0f 10 43 5c       	movsd  0x5c(%rbx),%xmm0
   1803775b4:	48 8b d3             	mov    %rbx,%rdx
   1803775b7:	f2 0f 11 44 24 20    	movsd  %xmm0,0x20(%rsp)
   1803775bd:	89 44 24 28          	mov    %eax,0x28(%rsp)
   1803775c1:	e8 6a 09 00 00       	call   0x180377f30
   1803775c6:	f2 0f 10 00          	movsd  (%rax),%xmm0
   1803775ca:	8b 48 08             	mov    0x8(%rax),%ecx
   1803775cd:	f2 0f 11 43 5c       	movsd  %xmm0,0x5c(%rbx)
   1803775d2:	89 4b 64             	mov    %ecx,0x64(%rbx)
   1803775d5:	48 83 c4 40          	add    $0x40,%rsp
   1803775d9:	5b                   	pop    %rbx
   1803775da:	c3                   	ret
   1803775db:	48 8b 43 48          	mov    0x48(%rbx),%rax
   1803775df:	48 85 c0             	test   %rax,%rax
   1803775e2:	74 1e                	je     0x180377602
   1803775e4:	48 8b 90 00 01 00 00 	mov    0x100(%rax),%rdx
   1803775eb:	48 85 d2             	test   %rdx,%rdx
   1803775ee:	74 12                	je     0x180377602
   1803775f0:	45 33 c0             	xor    %r8d,%r8d
   1803775f3:	48 8d 4c 24 30       	lea    0x30(%rsp),%rcx
   1803775f8:	e8 83 c0 fa ff       	call   0x180323680
   1803775fd:	e9 5a ff ff ff       	jmp    0x18037755c
   180377602:	e8 49 8b ed ff       	call   0x180250150
   180377607:	cc                   	int3
   180377608:	e1 74                	loope  0x18037767e
   18037760a:	37                   	(bad)
   18037760b:	00 db                	add    %bl,%bl
   18037760d:	75 37                	jne    0x180377646
   18037760f:	00 db                	add    %bl,%bl
   180377611:	75 37                	jne    0x18037764a
   180377613:	00 32                	add    %dh,(%rdx)
   180377615:	75 37                	jne    0x18037764e
   180377617:	00 95 75 37 00 95    	add    %dl,-0x6affc88b(%rbp)
   18037761d:	75 37                	jne    0x180377656
   18037761f:	00 8f 75 37 00 8f    	add    %cl,-0x70ffc88b(%rdi)
   180377625:	75 37                	jne    0x18037765e
   180377627:	00 db                	add    %bl,%bl
   180377629:	75 37                	jne    0x180377662
	...
