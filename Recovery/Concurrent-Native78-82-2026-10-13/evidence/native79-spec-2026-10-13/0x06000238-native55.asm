
/workspaces/GodsPVZ-native24-codespace/.validation/native55-2026-10-02/native/0x06000238.bin:     file format binary


Disassembly of section .data:

000000018033e270 <.data>:
   18033e270:	48 89 5c 24 08       	mov    %rbx,0x8(%rsp)
   18033e275:	57                   	push   %rdi
   18033e276:	48 83 ec 20          	sub    $0x20,%rsp
   18033e27a:	80 3d ae 69 9a 01 00 	cmpb   $0x0,0x19a69ae(%rip)        # 0x181ce4c2f
   18033e281:	48 8b f9             	mov    %rcx,%rdi
   18033e284:	75 2b                	jne    0x18033e2b1
   18033e286:	48 8d 0d cb a1 87 01 	lea    0x187a1cb(%rip),%rcx        # 0x181bb8458
   18033e28d:	e8 5e 1c f1 ff       	call   0x18024fef0
   18033e292:	48 8d 0d 27 96 87 01 	lea    0x1879627(%rip),%rcx        # 0x181bb78c0
   18033e299:	e8 52 1c f1 ff       	call   0x18024fef0
   18033e29e:	48 8d 0d 23 36 86 01 	lea    0x1863623(%rip),%rcx        # 0x181ba18c8
   18033e2a5:	e8 46 1c f1 ff       	call   0x18024fef0
   18033e2aa:	c6 05 7e 69 9a 01 01 	movb   $0x1,0x19a697e(%rip)        # 0x181ce4c2f
   18033e2b1:	48 8b 0d 08 96 87 01 	mov    0x1879608(%rip),%rcx        # 0x181bb78c0
   18033e2b8:	e8 43 1e f1 ff       	call   0x180250100
   18033e2bd:	33 d2                	xor    %edx,%edx
   18033e2bf:	48 8b c8             	mov    %rax,%rcx
   18033e2c2:	48 8b d8             	mov    %rax,%rbx
   18033e2c5:	e8 c6 20 04 00       	call   0x180380390
   18033e2ca:	45 33 c0             	xor    %r8d,%r8d
   18033e2cd:	48 8b d3             	mov    %rbx,%rdx
   18033e2d0:	48 8b cf             	mov    %rdi,%rcx
   18033e2d3:	e8 08 0e 00 00       	call   0x18033f0e0
   18033e2d8:	48 85 db             	test   %rbx,%rbx
   18033e2db:	0f 84 02 01 00 00    	je     0x18033e3e3
   18033e2e1:	48 8b 57 10          	mov    0x10(%rdi),%rdx
   18033e2e5:	45 33 c0             	xor    %r8d,%r8d
   18033e2e8:	48 8b cb             	mov    %rbx,%rcx
   18033e2eb:	e8 c0 0e 04 00       	call   0x18037f1b0
   18033e2f0:	48 8b 4f 10          	mov    0x10(%rdi),%rcx
   18033e2f4:	48 85 c9             	test   %rcx,%rcx
   18033e2f7:	0f 84 e6 00 00 00    	je     0x18033e3e3
   18033e2fd:	48 8b 15 c4 35 86 01 	mov    0x18635c4(%rip),%rdx        # 0x181ba18c8
   18033e304:	45 33 c0             	xor    %r8d,%r8d
   18033e307:	48 8b 49 18          	mov    0x18(%rcx),%rcx
   18033e30b:	e8 60 5f 83 00       	call   0x180b74270
   18033e310:	48 8d 4b 18          	lea    0x18(%rbx),%rcx
   18033e314:	48 89 43 18          	mov    %rax,0x18(%rbx)
   18033e318:	48 8b d0             	mov    %rax,%rdx
   18033e31b:	e8 40 10 f1 ff       	call   0x18024f360
   18033e320:	48 8b 4f 18          	mov    0x18(%rdi),%rcx
   18033e324:	48 85 c9             	test   %rcx,%rcx
   18033e327:	0f 84 b6 00 00 00    	je     0x18033e3e3
   18033e32d:	48 8b 49 18          	mov    0x18(%rcx),%rcx
   18033e331:	48 85 c9             	test   %rcx,%rcx
   18033e334:	0f 84 a9 00 00 00    	je     0x18033e3e3
   18033e33a:	4c 8b 0d 17 a1 87 01 	mov    0x187a117(%rip),%r9        # 0x181bb8458
   18033e341:	ff 41 1c             	incl   0x1c(%rcx)
   18033e344:	48 8b 51 10          	mov    0x10(%rcx),%rdx
   18033e348:	4c 63 41 18          	movslq 0x18(%rcx),%r8
   18033e34c:	48 85 d2             	test   %rdx,%rdx
   18033e34f:	0f 84 8e 00 00 00    	je     0x18033e3e3
   18033e355:	44 3b 42 18          	cmp    0x18(%rdx),%r8d
   18033e359:	72 19                	jb     0x18033e374
   18033e35b:	49 8b 41 20          	mov    0x20(%r9),%rax
   18033e35f:	48 8b d3             	mov    %rbx,%rdx
   18033e362:	4c 8b 80 c0 00 00 00 	mov    0xc0(%rax),%r8
   18033e369:	4d 8b 40 70          	mov    0x70(%r8),%r8
   18033e36d:	e8 6e 99 50 00       	call   0x180847ce0
   18033e372:	eb 22                	jmp    0x18033e396
   18033e374:	41 8d 40 01          	lea    0x1(%r8),%eax
   18033e378:	89 41 18             	mov    %eax,0x18(%rcx)
   18033e37b:	44 3b 42 18          	cmp    0x18(%rdx),%r8d
   18033e37f:	73 68                	jae    0x18033e3e9
   18033e381:	4a 89 5c c2 20       	mov    %rbx,0x20(%rdx,%r8,8)
   18033e386:	48 83 c2 20          	add    $0x20,%rdx
   18033e38a:	4a 8d 0c c2          	lea    (%rdx,%r8,8),%rcx
   18033e38e:	48 8b d3             	mov    %rbx,%rdx
   18033e391:	e8 ca 0f f1 ff       	call   0x18024f360
   18033e396:	33 d2                	xor    %edx,%edx
   18033e398:	48 8b cf             	mov    %rdi,%rcx
   18033e39b:	e8 50 1f 00 00       	call   0x1803402f0
   18033e3a0:	80 3d 8a 67 9a 01 00 	cmpb   $0x0,0x19a678a(%rip)        # 0x181ce4b31
   18033e3a7:	75 13                	jne    0x18033e3bc
   18033e3a9:	48 8d 0d c0 e1 86 01 	lea    0x186e1c0(%rip),%rcx        # 0x181bac570
   18033e3b0:	e8 3b 1b f1 ff       	call   0x18024fef0
   18033e3b5:	c6 05 75 67 9a 01 01 	movb   $0x1,0x19a6775(%rip)        # 0x181ce4b31
   18033e3bc:	48 8b 05 ad e1 86 01 	mov    0x186e1ad(%rip),%rax        # 0x181bac570
   18033e3c3:	48 8b 88 b8 00 00 00 	mov    0xb8(%rax),%rcx
   18033e3ca:	48 8b 09             	mov    (%rcx),%rcx
   18033e3cd:	48 85 c9             	test   %rcx,%rcx
   18033e3d0:	74 11                	je     0x18033e3e3
   18033e3d2:	33 d2                	xor    %edx,%edx
   18033e3d4:	48 8b 5c 24 30       	mov    0x30(%rsp),%rbx
   18033e3d9:	48 83 c4 20          	add    $0x20,%rsp
   18033e3dd:	5f                   	pop    %rdi
   18033e3de:	e9 cd 17 06 00       	jmp    0x18039fbb0
   18033e3e3:	e8 68 1d f1 ff       	call   0x180250150
   18033e3e8:	cc                   	int3
   18033e3e9:	e8 52 1d f1 ff       	call   0x180250140
   18033e3ee:	cc                   	int3
