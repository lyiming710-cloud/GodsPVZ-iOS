
/workspaces/GodsPVZ-native24-codespace/.validation/native55-2026-10-02/native/0x06000199.bin:     file format binary


Disassembly of section .data:

00000001803142e0 <.data>:
   1803142e0:	48 89 5c 24 08       	mov    %rbx,0x8(%rsp)
   1803142e5:	57                   	push   %rdi
   1803142e6:	48 83 ec 60          	sub    $0x60,%rsp
   1803142ea:	80 3d b0 08 9d 01 00 	cmpb   $0x0,0x19d08b0(%rip)        # 0x181ce4ba1
   1803142f1:	0f 29 7c 24 40       	movaps %xmm7,0x40(%rsp)
   1803142f6:	48 63 fa             	movslq %edx,%rdi
   1803142f9:	75 37                	jne    0x180314332
   1803142fb:	48 8d 0d 16 dd 88 01 	lea    0x188dd16(%rip),%rcx        # 0x181ba2018
   180314302:	e8 e9 bb f3 ff       	call   0x18024fef0
   180314307:	48 8d 0d 6a ff 88 01 	lea    0x188ff6a(%rip),%rcx        # 0x181ba4278
   18031430e:	e8 dd bb f3 ff       	call   0x18024fef0
   180314313:	48 8d 0d e6 c1 89 01 	lea    0x189c1e6(%rip),%rcx        # 0x181bb0500
   18031431a:	e8 d1 bb f3 ff       	call   0x18024fef0
   18031431f:	48 8d 0d 52 21 8a 01 	lea    0x18a2152(%rip),%rcx        # 0x181bb6478
   180314326:	e8 c5 bb f3 ff       	call   0x18024fef0
   18031432b:	c6 05 6f 08 9d 01 01 	movb   $0x1,0x19d086f(%rip)        # 0x181ce4ba1
   180314332:	48 8b 0d df dc 88 01 	mov    0x188dcdf(%rip),%rcx        # 0x181ba2018
   180314339:	33 db                	xor    %ebx,%ebx
   18031433b:	39 99 e0 00 00 00    	cmp    %ebx,0xe0(%rcx)
   180314341:	75 05                	jne    0x180314348
   180314343:	e8 98 be f3 ff       	call   0x1802501e0
   180314348:	33 c9                	xor    %ecx,%ecx
   18031434a:	e8 51 6b 00 00       	call   0x18031aea0
   18031434f:	0f 28 f8             	movaps %xmm0,%xmm7
   180314352:	83 ff 06             	cmp    $0x6,%edi
   180314355:	0f 87 1e 01 00 00    	ja     0x180314479
   18031435b:	48 8d 15 9e bc ce ff 	lea    -0x314362(%rip),%rdx        # 0x180000000
   180314362:	8b 8c ba 3c 45 31 00 	mov    0x31453c(%rdx,%rdi,4),%ecx
   180314369:	48 03 ca             	add    %rdx,%rcx
   18031436c:	ff e1                	jmp    *%rcx
   18031436e:	48 8b 05 03 21 8a 01 	mov    0x18a2103(%rip),%rax        # 0x181bb6478
   180314375:	39 98 e0 00 00 00    	cmp    %ebx,0xe0(%rax)
   18031437b:	75 0f                	jne    0x18031438c
   18031437d:	48 8b c8             	mov    %rax,%rcx
   180314380:	e8 5b be f3 ff       	call   0x1802501e0
   180314385:	48 8b 05 ec 20 8a 01 	mov    0x18a20ec(%rip),%rax        # 0x181bb6478
   18031438c:	48 8b 80 b8 00 00 00 	mov    0xb8(%rax),%rax
   180314393:	48 8b 88 40 01 00 00 	mov    0x140(%rax),%rcx
   18031439a:	48 85 c9             	test   %rcx,%rcx
   18031439d:	0f 84 90 01 00 00    	je     0x180314533
   1803143a3:	33 d2                	xor    %edx,%edx
   1803143a5:	e9 c0 00 00 00       	jmp    0x18031446a
   1803143aa:	48 8b 05 c7 20 8a 01 	mov    0x18a20c7(%rip),%rax        # 0x181bb6478
   1803143b1:	39 98 e0 00 00 00    	cmp    %ebx,0xe0(%rax)
   1803143b7:	75 0f                	jne    0x1803143c8
   1803143b9:	48 8b c8             	mov    %rax,%rcx
   1803143bc:	e8 1f be f3 ff       	call   0x1802501e0
   1803143c1:	48 8b 05 b0 20 8a 01 	mov    0x18a20b0(%rip),%rax        # 0x181bb6478
   1803143c8:	b9 01 00 00 00       	mov    $0x1,%ecx
   1803143cd:	eb 72                	jmp    0x180314441
   1803143cf:	48 8b 05 a2 20 8a 01 	mov    0x18a20a2(%rip),%rax        # 0x181bb6478
   1803143d6:	39 98 e0 00 00 00    	cmp    %ebx,0xe0(%rax)
   1803143dc:	75 0f                	jne    0x1803143ed
   1803143de:	48 8b c8             	mov    %rax,%rcx
   1803143e1:	e8 fa bd f3 ff       	call   0x1802501e0
   1803143e6:	48 8b 05 8b 20 8a 01 	mov    0x18a208b(%rip),%rax        # 0x181bb6478
   1803143ed:	b9 04 00 00 00       	mov    $0x4,%ecx
   1803143f2:	eb 4d                	jmp    0x180314441
   1803143f4:	48 8b 05 7d 20 8a 01 	mov    0x18a207d(%rip),%rax        # 0x181bb6478
   1803143fb:	39 98 e0 00 00 00    	cmp    %ebx,0xe0(%rax)
   180314401:	75 0f                	jne    0x180314412
   180314403:	48 8b c8             	mov    %rax,%rcx
   180314406:	e8 d5 bd f3 ff       	call   0x1802501e0
   18031440b:	48 8b 05 66 20 8a 01 	mov    0x18a2066(%rip),%rax        # 0x181bb6478
   180314412:	ba 02 00 00 00       	mov    $0x2,%edx
   180314417:	b9 07 00 00 00       	mov    $0x7,%ecx
   18031441c:	eb 28                	jmp    0x180314446
   18031441e:	48 8b 05 53 20 8a 01 	mov    0x18a2053(%rip),%rax        # 0x181bb6478
   180314425:	39 98 e0 00 00 00    	cmp    %ebx,0xe0(%rax)
   18031442b:	75 0f                	jne    0x18031443c
   18031442d:	48 8b c8             	mov    %rax,%rcx
   180314430:	e8 ab bd f3 ff       	call   0x1802501e0
   180314435:	48 8b 05 3c 20 8a 01 	mov    0x18a203c(%rip),%rax        # 0x181bb6478
   18031443c:	b9 09 00 00 00       	mov    $0x9,%ecx
   180314441:	ba 03 00 00 00       	mov    $0x3,%edx
   180314446:	48 8b 80 b8 00 00 00 	mov    0xb8(%rax),%rax
   18031444d:	45 33 c0             	xor    %r8d,%r8d
   180314450:	48 8b 98 40 01 00 00 	mov    0x140(%rax),%rbx
   180314457:	e8 b4 8e ff 00       	call   0x18130d310
   18031445c:	48 85 db             	test   %rbx,%rbx
   18031445f:	0f 84 ce 00 00 00    	je     0x180314533
   180314465:	48 8b cb             	mov    %rbx,%rcx
   180314468:	8b d0                	mov    %eax,%edx
   18031446a:	4c 8b 05 07 fe 88 01 	mov    0x188fe07(%rip),%r8        # 0x181ba4278
   180314471:	e8 9a 12 51 00       	call   0x180825710
   180314476:	48 8b d8             	mov    %rax,%rbx
   180314479:	48 8b 0d 80 c0 89 01 	mov    0x189c080(%rip),%rcx        # 0x181bb0500
   180314480:	83 b9 e0 00 00 00 00 	cmpl   $0x0,0xe0(%rcx)
   180314487:	75 05                	jne    0x18031448e
   180314489:	e8 52 bd f3 ff       	call   0x1802501e0
   18031448e:	45 33 c0             	xor    %r8d,%r8d
   180314491:	33 d2                	xor    %edx,%edx
   180314493:	48 8b cb             	mov    %rbx,%rcx
   180314496:	e8 65 b4 00 01       	call   0x18131f900
   18031449b:	84 c0                	test   %al,%al
   18031449d:	0f 84 80 00 00 00    	je     0x180314523
   1803144a3:	33 c9                	xor    %ecx,%ecx
   1803144a5:	e8 16 e3 fc 00       	call   0x1812e27c0
   1803144aa:	48 85 c0             	test   %rax,%rax
   1803144ad:	0f 84 80 00 00 00    	je     0x180314533
   1803144b3:	33 d2                	xor    %edx,%edx
   1803144b5:	48 8b c8             	mov    %rax,%rcx
   1803144b8:	e8 53 00 ff 00       	call   0x181304510
   1803144bd:	48 85 c0             	test   %rax,%rax
   1803144c0:	74 71                	je     0x180314533
   1803144c2:	45 33 c0             	xor    %r8d,%r8d
   1803144c5:	0f 29 74 24 50       	movaps %xmm6,0x50(%rsp)
   1803144ca:	48 8b d0             	mov    %rax,%rdx
   1803144cd:	48 8d 4c 24 30       	lea    0x30(%rsp),%rcx
   1803144d2:	e8 c9 ae 01 01       	call   0x18132f3a0
   1803144d7:	48 8b 0d 3a db 88 01 	mov    0x188db3a(%rip),%rcx        # 0x181ba2018
   1803144de:	f2 0f 10 30          	movsd  (%rax),%xmm6
   1803144e2:	83 b9 e0 00 00 00 00 	cmpl   $0x0,0xe0(%rcx)
   1803144e9:	8b 78 08             	mov    0x8(%rax),%edi
   1803144ec:	75 05                	jne    0x1803144f3
   1803144ee:	e8 ed bc f3 ff       	call   0x1802501e0
   1803144f3:	f3 0f 10 1d 15 35 29 	movss  0x1293515(%rip),%xmm3        # 0x1815a7a10
   1803144fa:	01 
   1803144fb:	48 8d 54 24 30       	lea    0x30(%rsp),%rdx
   180314500:	0f 28 d7             	movaps %xmm7,%xmm2
   180314503:	f2 0f 11 74 24 30    	movsd  %xmm6,0x30(%rsp)
   180314509:	48 8b cb             	mov    %rbx,%rcx
   18031450c:	89 7c 24 38          	mov    %edi,0x38(%rsp)
   180314510:	48 c7 44 24 20 00 00 	movq   $0x0,0x20(%rsp)
   180314517:	00 00 
   180314519:	e8 32 6e 00 00       	call   0x18031b350
   18031451e:	0f 28 74 24 50       	movaps 0x50(%rsp),%xmm6
   180314523:	48 8b 5c 24 70       	mov    0x70(%rsp),%rbx
   180314528:	0f 28 7c 24 40       	movaps 0x40(%rsp),%xmm7
   18031452d:	48 83 c4 60          	add    $0x60,%rsp
   180314531:	5f                   	pop    %rdi
   180314532:	c3                   	ret
   180314533:	e8 18 bc f3 ff       	call   0x180250150
   180314538:	cc                   	int3
   180314539:	0f 1f 00             	nopl   (%rax)
   18031453c:	79 44                	jns    0x180314582
   18031453e:	31 00                	xor    %eax,(%rax)
   180314540:	79 44                	jns    0x180314586
   180314542:	31 00                	xor    %eax,(%rax)
   180314544:	6e                   	outsb  %ds:(%rsi),(%dx)
   180314545:	43 31 00             	rex.XB xor %eax,(%r8)
   180314548:	aa                   	stos   %al,%es:(%rdi)
   180314549:	43 31 00             	rex.XB xor %eax,(%r8)
   18031454c:	cf                   	iret
   18031454d:	43 31 00             	rex.XB xor %eax,(%r8)
   180314550:	f4                   	hlt
   180314551:	43 31 00             	rex.XB xor %eax,(%r8)
   180314554:	1e                   	(bad)
   180314555:	44 31 00             	xor    %r8d,(%rax)
