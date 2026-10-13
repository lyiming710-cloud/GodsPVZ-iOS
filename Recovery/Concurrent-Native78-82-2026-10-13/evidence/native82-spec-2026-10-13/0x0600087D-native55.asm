
/workspaces/GodsPVZ-native24-codespace/.validation/native55-2026-10-02/native/0x0600087D.bin:     file format binary


Disassembly of section .data:

00000001803c9170 <.data>:
   1803c9170:	48 89 5c 24 08       	mov    %rbx,0x8(%rsp)
   1803c9175:	57                   	push   %rdi
   1803c9176:	48 83 ec 20          	sub    $0x20,%rsp
   1803c917a:	80 3d 6a be 91 01 00 	cmpb   $0x0,0x191be6a(%rip)        # 0x181ce4feb
   1803c9181:	0f b6 f9             	movzbl %cl,%edi
   1803c9184:	75 4f                	jne    0x1803c91d5
   1803c9186:	48 8d 0d 4b af 7d 01 	lea    0x17daf4b(%rip),%rcx        # 0x181ba40d8
   1803c918d:	e8 5e 6d e8 ff       	call   0x18024fef0
   1803c9192:	48 8d 0d 1f 8b 7d 01 	lea    0x17d8b1f(%rip),%rcx        # 0x181ba1cb8
   1803c9199:	e8 52 6d e8 ff       	call   0x18024fef0
   1803c919e:	48 8d 0d 23 e3 7e 01 	lea    0x17ee323(%rip),%rcx        # 0x181bb74c8
   1803c91a5:	e8 46 6d e8 ff       	call   0x18024fef0
   1803c91aa:	48 8d 0d 4f 73 7e 01 	lea    0x17e734f(%rip),%rcx        # 0x181bb0500
   1803c91b1:	e8 3a 6d e8 ff       	call   0x18024fef0
   1803c91b6:	48 8d 0d 7b 2e 7f 01 	lea    0x17f2e7b(%rip),%rcx        # 0x181bbc038
   1803c91bd:	e8 2e 6d e8 ff       	call   0x18024fef0
   1803c91c2:	48 8d 0d 27 55 80 01 	lea    0x1805527(%rip),%rcx        # 0x181bce6f0
   1803c91c9:	e8 22 6d e8 ff       	call   0x18024fef0
   1803c91ce:	c6 05 16 be 91 01 01 	movb   $0x1,0x191be16(%rip)        # 0x181ce4feb
   1803c91d5:	48 8b 05 5c 2e 7f 01 	mov    0x17f2e5c(%rip),%rax        # 0x181bbc038
   1803c91dc:	48 8b 88 b8 00 00 00 	mov    0xb8(%rax),%rcx
   1803c91e3:	48 8b 19             	mov    (%rcx),%rbx
   1803c91e6:	48 8b 0d 13 73 7e 01 	mov    0x17e7313(%rip),%rcx        # 0x181bb0500
   1803c91ed:	83 b9 e0 00 00 00 00 	cmpl   $0x0,0xe0(%rcx)
   1803c91f4:	75 05                	jne    0x1803c91fb
   1803c91f6:	e8 e5 6f e8 ff       	call   0x1802501e0
   1803c91fb:	33 d2                	xor    %edx,%edx
   1803c91fd:	48 8b cb             	mov    %rbx,%rcx
   1803c9200:	e8 6b 66 f5 00       	call   0x18131f870
   1803c9205:	84 c0                	test   %al,%al
   1803c9207:	0f 85 e1 00 00 00    	jne    0x1803c92ee
   1803c920d:	48 8b 0d ec 72 7e 01 	mov    0x17e72ec(%rip),%rcx        # 0x181bb0500
   1803c9214:	83 b9 e0 00 00 00 00 	cmpl   $0x0,0xe0(%rcx)
   1803c921b:	75 05                	jne    0x1803c9222
   1803c921d:	e8 be 6f e8 ff       	call   0x1802501e0
   1803c9222:	48 8b 0d 9f e2 7e 01 	mov    0x17ee29f(%rip),%rcx        # 0x181bb74c8
   1803c9229:	e8 f2 71 0d 00       	call   0x1804a0420
   1803c922e:	48 8b 0d 03 2e 7f 01 	mov    0x17f2e03(%rip),%rcx        # 0x181bbc038
   1803c9235:	48 8b 91 b8 00 00 00 	mov    0xb8(%rcx),%rdx
   1803c923c:	48 89 02             	mov    %rax,(%rdx)
   1803c923f:	48 8b d0             	mov    %rax,%rdx
   1803c9242:	48 8b 0d ef 2d 7f 01 	mov    0x17f2def(%rip),%rcx        # 0x181bbc038
   1803c9249:	48 8b 89 b8 00 00 00 	mov    0xb8(%rcx),%rcx
   1803c9250:	e8 0b 61 e8 ff       	call   0x18024f360
   1803c9255:	40 84 ff             	test   %dil,%dil
   1803c9258:	0f 84 90 00 00 00    	je     0x1803c92ee
   1803c925e:	48 8b 05 d3 2d 7f 01 	mov    0x17f2dd3(%rip),%rax        # 0x181bbc038
   1803c9265:	48 8b 88 b8 00 00 00 	mov    0xb8(%rax),%rcx
   1803c926c:	48 8b 19             	mov    (%rcx),%rbx
   1803c926f:	48 8b 0d 8a 72 7e 01 	mov    0x17e728a(%rip),%rcx        # 0x181bb0500
   1803c9276:	83 b9 e0 00 00 00 00 	cmpl   $0x0,0xe0(%rcx)
   1803c927d:	75 05                	jne    0x1803c9284
   1803c927f:	e8 5c 6f e8 ff       	call   0x1802501e0
   1803c9284:	33 d2                	xor    %edx,%edx
   1803c9286:	48 8b cb             	mov    %rbx,%rcx
   1803c9289:	e8 e2 65 f5 00       	call   0x18131f870
   1803c928e:	84 c0                	test   %al,%al
   1803c9290:	75 5c                	jne    0x1803c92ee
   1803c9292:	48 8b 0d 1f 8a 7d 01 	mov    0x17d8a1f(%rip),%rcx        # 0x181ba1cb8
   1803c9299:	e8 62 6e e8 ff       	call   0x180250100
   1803c929e:	48 8b 15 4b 54 80 01 	mov    0x180544b(%rip),%rdx        # 0x181bce6f0
   1803c92a5:	45 33 c0             	xor    %r8d,%r8d
   1803c92a8:	48 8b c8             	mov    %rax,%rcx
   1803c92ab:	48 8b d8             	mov    %rax,%rbx
   1803c92ae:	e8 3d 2a f5 00       	call   0x18131bcf0
   1803c92b3:	48 85 db             	test   %rbx,%rbx
   1803c92b6:	74 52                	je     0x1803c930a
   1803c92b8:	48 8b 15 19 ae 7d 01 	mov    0x17dae19(%rip),%rdx        # 0x181ba40d8
   1803c92bf:	48 8b cb             	mov    %rbx,%rcx
   1803c92c2:	e8 b9 3f 0a 00       	call   0x18046d280
   1803c92c7:	48 8b 0d 6a 2d 7f 01 	mov    0x17f2d6a(%rip),%rcx        # 0x181bbc038
   1803c92ce:	48 8b 91 b8 00 00 00 	mov    0xb8(%rcx),%rdx
   1803c92d5:	48 89 02             	mov    %rax,(%rdx)
   1803c92d8:	48 8b d0             	mov    %rax,%rdx
   1803c92db:	48 8b 0d 56 2d 7f 01 	mov    0x17f2d56(%rip),%rcx        # 0x181bbc038
   1803c92e2:	48 8b 89 b8 00 00 00 	mov    0xb8(%rcx),%rcx
   1803c92e9:	e8 72 60 e8 ff       	call   0x18024f360
   1803c92ee:	48 8b 05 43 2d 7f 01 	mov    0x17f2d43(%rip),%rax        # 0x181bbc038
   1803c92f5:	48 8b 5c 24 30       	mov    0x30(%rsp),%rbx
   1803c92fa:	48 8b 80 b8 00 00 00 	mov    0xb8(%rax),%rax
   1803c9301:	48 8b 00             	mov    (%rax),%rax
   1803c9304:	48 83 c4 20          	add    $0x20,%rsp
   1803c9308:	5f                   	pop    %rdi
   1803c9309:	c3                   	ret
   1803c930a:	e8 41 6e e8 ff       	call   0x180250150
   1803c930f:	cc                   	int3
