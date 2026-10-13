
/workspaces/GodsPVZ-native24-codespace/.validation/native55-2026-10-02/native/0x060006DF.bin:     file format binary


Disassembly of section .data:

00000001803ad1b0 <.data>:
   1803ad1b0:	48 89 5c 24 08       	mov    %rbx,0x8(%rsp)
   1803ad1b5:	48 89 74 24 10       	mov    %rsi,0x10(%rsp)
   1803ad1ba:	57                   	push   %rdi
   1803ad1bb:	48 81 ec 90 00 00 00 	sub    $0x90,%rsp
   1803ad1c2:	80 3d 5a 7d 93 01 00 	cmpb   $0x0,0x1937d5a(%rip)        # 0x181ce4f23
   1803ad1c9:	48 8b fa             	mov    %rdx,%rdi
   1803ad1cc:	48 8b d9             	mov    %rcx,%rbx
   1803ad1cf:	75 2b                	jne    0x1803ad1fc
   1803ad1d1:	48 8d 0d 70 db 7e 01 	lea    0x17edb70(%rip),%rcx        # 0x181b9ad48
   1803ad1d8:	e8 13 2d ea ff       	call   0x18024fef0
   1803ad1dd:	48 8d 0d bc fb 80 01 	lea    0x180fbbc(%rip),%rcx        # 0x181bbcda0
   1803ad1e4:	e8 07 2d ea ff       	call   0x18024fef0
   1803ad1e9:	48 8d 0d 38 f4 80 01 	lea    0x180f438(%rip),%rcx        # 0x181bbc628
   1803ad1f0:	e8 fb 2c ea ff       	call   0x18024fef0
   1803ad1f5:	c6 05 27 7d 93 01 01 	movb   $0x1,0x1937d27(%rip)        # 0x181ce4f23
   1803ad1fc:	48 8b 73 20          	mov    0x20(%rbx),%rsi
   1803ad200:	33 c0                	xor    %eax,%eax
   1803ad202:	48 89 44 24 50       	mov    %rax,0x50(%rsp)
   1803ad207:	0f 57 c0             	xorps  %xmm0,%xmm0
   1803ad20a:	0f 11 44 24 30       	movups %xmm0,0x30(%rsp)
   1803ad20f:	0f 11 44 24 40       	movups %xmm0,0x40(%rsp)
   1803ad214:	48 85 ff             	test   %rdi,%rdi
   1803ad217:	0f 84 85 01 00 00    	je     0x1803ad3a2
   1803ad21d:	0f 29 b4 24 80 00 00 	movaps %xmm6,0x80(%rsp)
   1803ad224:	00 
   1803ad225:	f3 0f 10 b7 04 01 00 	movss  0x104(%rdi),%xmm6
   1803ad22c:	00 
   1803ad22d:	0f 29 7c 24 70       	movaps %xmm7,0x70(%rsp)
   1803ad232:	f3 0f 10 bf 08 01 00 	movss  0x108(%rdi),%xmm7
   1803ad239:	00 
   1803ad23a:	44 0f 29 44 24 60    	movaps %xmm8,0x60(%rsp)
   1803ad240:	33 c9                	xor    %ecx,%ecx
   1803ad242:	45 0f 57 c0          	xorps  %xmm8,%xmm8
   1803ad246:	e8 75 55 f3 00       	call   0x1812e27c0
   1803ad24b:	48 8b 0d 4e fb 80 01 	mov    0x180fb4e(%rip),%rcx        # 0x181bbcda0
   1803ad252:	48 8b f8             	mov    %rax,%rdi
   1803ad255:	83 b9 e0 00 00 00 00 	cmpl   $0x0,0xe0(%rcx)
   1803ad25c:	75 05                	jne    0x1803ad263
   1803ad25e:	e8 7d 2f ea ff       	call   0x1802501e0
   1803ad263:	0f 14 f7             	unpcklps %xmm7,%xmm6
   1803ad266:	48 8d 54 24 20       	lea    0x20(%rsp),%rdx
   1803ad26b:	45 33 c9             	xor    %r9d,%r9d
   1803ad26e:	f2 0f 11 74 24 20    	movsd  %xmm6,0x20(%rsp)
   1803ad274:	4c 8b c7             	mov    %rdi,%r8
   1803ad277:	f3 44 0f 11 44 24 28 	movss  %xmm8,0x28(%rsp)
   1803ad27e:	48 8b ce             	mov    %rsi,%rcx
   1803ad281:	e8 aa f4 e1 00       	call   0x1811cc730
   1803ad286:	44 0f 28 44 24 60    	movaps 0x60(%rsp),%xmm8
   1803ad28c:	0f 28 7c 24 70       	movaps 0x70(%rsp),%xmm7
   1803ad291:	0f 28 b4 24 80 00 00 	movaps 0x80(%rsp),%xmm6
   1803ad298:	00 
   1803ad299:	89 43 28             	mov    %eax,0x28(%rbx)
   1803ad29c:	83 f8 ff             	cmp    $0xffffffff,%eax
   1803ad29f:	0f 84 e8 00 00 00    	je     0x1803ad38d
   1803ad2a5:	48 8b 4b 20          	mov    0x20(%rbx),%rcx
   1803ad2a9:	48 85 c9             	test   %rcx,%rcx
   1803ad2ac:	0f 84 f0 00 00 00    	je     0x1803ad3a2
   1803ad2b2:	48 8b 91 70 03 00 00 	mov    0x370(%rcx),%rdx
   1803ad2b9:	48 85 d2             	test   %rdx,%rdx
   1803ad2bc:	0f 84 e0 00 00 00    	je     0x1803ad3a2
   1803ad2c2:	48 8b 52 48          	mov    0x48(%rdx),%rdx
   1803ad2c6:	48 85 d2             	test   %rdx,%rdx
   1803ad2c9:	0f 84 d3 00 00 00    	je     0x1803ad3a2
   1803ad2cf:	3b 42 18             	cmp    0x18(%rdx),%eax
   1803ad2d2:	0f 83 d0 00 00 00    	jae    0x1803ad3a8
   1803ad2d8:	48 98                	cltq
   1803ad2da:	48 8d 0c 80          	lea    (%rax,%rax,4),%rcx
   1803ad2de:	0f 10 44 ca 20       	movups 0x20(%rdx,%rcx,8),%xmm0
   1803ad2e3:	0f 11 44 24 30       	movups %xmm0,0x30(%rsp)
   1803ad2e8:	0f 10 4c ca 30       	movups 0x30(%rdx,%rcx,8),%xmm1
   1803ad2ed:	0f 11 4c 24 40       	movups %xmm1,0x40(%rsp)
   1803ad2f2:	f2 0f 10 44 ca 40    	movsd  0x40(%rdx,%rcx,8),%xmm0
   1803ad2f8:	33 d2                	xor    %edx,%edx
   1803ad2fa:	48 8d 4c 24 30       	lea    0x30(%rsp),%rcx
   1803ad2ff:	f2 0f 11 44 24 50    	movsd  %xmm0,0x50(%rsp)
   1803ad305:	e8 86 e9 de 00       	call   0x18119bc90
   1803ad30a:	48 8b 0d 37 da 7e 01 	mov    0x17eda37(%rip),%rcx        # 0x181b9ad48
   1803ad311:	48 8b d8             	mov    %rax,%rbx
   1803ad314:	83 b9 e0 00 00 00 00 	cmpl   $0x0,0xe0(%rcx)
   1803ad31b:	75 05                	jne    0x1803ad322
   1803ad31d:	e8 be 2e ea ff       	call   0x1802501e0
   1803ad322:	33 d2                	xor    %edx,%edx
   1803ad324:	48 8b cb             	mov    %rbx,%rcx
   1803ad327:	e8 c4 95 f3 00       	call   0x1812e68f0
   1803ad32c:	48 8b 15 f5 f2 80 01 	mov    0x180f2f5(%rip),%rdx        # 0x181bbc628
   1803ad333:	45 33 c0             	xor    %r8d,%r8d
   1803ad336:	48 8b cb             	mov    %rbx,%rcx
   1803ad339:	e8 32 8e 7c 00       	call   0x180b76170
   1803ad33e:	84 c0                	test   %al,%al
   1803ad340:	74 4b                	je     0x1803ad38d
   1803ad342:	80 3d db 7b 93 01 00 	cmpb   $0x0,0x1937bdb(%rip)        # 0x181ce4f24
   1803ad349:	75 1f                	jne    0x1803ad36a
   1803ad34b:	48 8d 0d be 1e 82 01 	lea    0x1821ebe(%rip),%rcx        # 0x181bcf210
   1803ad352:	e8 99 2b ea ff       	call   0x18024fef0
   1803ad357:	48 8d 0d 92 b3 80 01 	lea    0x180b392(%rip),%rcx        # 0x181bb86f0
   1803ad35e:	e8 8d 2b ea ff       	call   0x18024fef0
   1803ad363:	c6 05 ba 7b 93 01 01 	movb   $0x1,0x1937bba(%rip)        # 0x181ce4f24
   1803ad36a:	48 8b 0d 9f 1e 82 01 	mov    0x1821e9f(%rip),%rcx        # 0x181bcf210
   1803ad371:	83 b9 e0 00 00 00 00 	cmpl   $0x0,0xe0(%rcx)
   1803ad378:	75 05                	jne    0x1803ad37f
   1803ad37a:	e8 61 2e ea ff       	call   0x1802501e0
   1803ad37f:	48 8b 0d 6a b3 80 01 	mov    0x180b36a(%rip),%rcx        # 0x181bb86f0
   1803ad386:	33 d2                	xor    %edx,%edx
   1803ad388:	e8 33 1d f3 00       	call   0x1812df0c0
   1803ad38d:	4c 8d 9c 24 90 00 00 	lea    0x90(%rsp),%r11
   1803ad394:	00 
   1803ad395:	49 8b 5b 10          	mov    0x10(%r11),%rbx
   1803ad399:	49 8b 73 18          	mov    0x18(%r11),%rsi
   1803ad39d:	49 8b e3             	mov    %r11,%rsp
   1803ad3a0:	5f                   	pop    %rdi
   1803ad3a1:	c3                   	ret
   1803ad3a2:	e8 a9 2d ea ff       	call   0x180250150
   1803ad3a7:	cc                   	int3
   1803ad3a8:	e8 93 2d ea ff       	call   0x180250140
   1803ad3ad:	cc                   	int3
