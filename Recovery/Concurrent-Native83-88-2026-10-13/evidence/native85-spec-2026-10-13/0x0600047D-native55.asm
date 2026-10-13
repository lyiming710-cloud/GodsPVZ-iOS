
/workspaces/GodsPVZ-native24-codespace/.validation/native55-2026-10-02/native/0x0600047D.bin:     file format binary


Disassembly of section .data:

0000000180368f60 <.data>:
   180368f60:	48 89 5c 24 18       	mov    %rbx,0x18(%rsp)
   180368f65:	48 89 74 24 20       	mov    %rsi,0x20(%rsp)
   180368f6a:	57                   	push   %rdi
   180368f6b:	48 81 ec 90 00 00 00 	sub    $0x90,%rsp
   180368f72:	80 3d 10 be 97 01 00 	cmpb   $0x0,0x197be10(%rip)        # 0x181ce4d89
   180368f79:	48 8b d9             	mov    %rcx,%rbx
   180368f7c:	75 37                	jne    0x180368fb5
   180368f7e:	48 8d 0d 93 90 83 01 	lea    0x1839093(%rip),%rcx        # 0x181ba2018
   180368f85:	e8 66 6f ee ff       	call   0x18024fef0
   180368f8a:	48 8d 0d 8f 28 85 01 	lea    0x185288f(%rip),%rcx        # 0x181bbb820
   180368f91:	e8 5a 6f ee ff       	call   0x18024fef0
   180368f96:	48 8d 0d eb 60 86 01 	lea    0x18660eb(%rip),%rcx        # 0x181bcf088
   180368f9d:	e8 4e 6f ee ff       	call   0x18024fef0
   180368fa2:	48 8d 0d 37 0e 84 01 	lea    0x1840e37(%rip),%rcx        # 0x181ba9de0
   180368fa9:	e8 42 6f ee ff       	call   0x18024fef0
   180368fae:	c6 05 d4 bd 97 01 01 	movb   $0x1,0x197bdd4(%rip)        # 0x181ce4d89
   180368fb5:	8b 43 60             	mov    0x60(%rbx),%eax
   180368fb8:	33 ff                	xor    %edi,%edi
   180368fba:	48 8b 35 c7 60 86 01 	mov    0x18660c7(%rip),%rsi        # 0x181bcf088
   180368fc1:	83 c0 fa             	add    $0xfffffffa,%eax
   180368fc4:	83 f8 10             	cmp    $0x10,%eax
   180368fc7:	77 30                	ja     0x180368ff9
   180368fc9:	48 8d 15 30 70 c9 ff 	lea    -0x368fd0(%rip),%rdx        # 0x180000000
   180368fd0:	48 98                	cltq
   180368fd2:	8b 8c 82 8c 91 36 00 	mov    0x36918c(%rdx,%rax,4),%ecx
   180368fd9:	48 03 ca             	add    %rdx,%rcx
   180368fdc:	ff e1                	jmp    *%rcx
   180368fde:	33 d2                	xor    %edx,%edx
   180368fe0:	48 8b cb             	mov    %rbx,%rcx
   180368fe3:	e8 98 92 00 00       	call   0x180372280
   180368fe8:	e9 82 01 00 00       	jmp    0x18036916f
   180368fed:	bf 01 00 00 00       	mov    $0x1,%edi
   180368ff2:	48 8b 35 e7 0d 84 01 	mov    0x1840de7(%rip),%rsi        # 0x181ba9de0
   180368ff9:	48 8b 43 20          	mov    0x20(%rbx),%rax
   180368ffd:	48 89 ac 24 a0 00 00 	mov    %rbp,0xa0(%rsp)
   180369004:	00 
   180369005:	0f 29 b4 24 80 00 00 	movaps %xmm6,0x80(%rsp)
   18036900c:	00 
   18036900d:	0f 29 7c 24 70       	movaps %xmm7,0x70(%rsp)
   180369012:	48 85 c0             	test   %rax,%rax
   180369015:	0f 84 69 01 00 00    	je     0x180369184
   18036901b:	48 8b 88 f8 00 00 00 	mov    0xf8(%rax),%rcx
   180369022:	48 85 c9             	test   %rcx,%rcx
   180369025:	0f 84 59 01 00 00    	je     0x180369184
   18036902b:	45 33 c0             	xor    %r8d,%r8d
   18036902e:	4c 89 b4 24 a8 00 00 	mov    %r14,0xa8(%rsp)
   180369035:	00 
   180369036:	8b d7                	mov    %edi,%edx
   180369038:	e8 13 b1 fb ff       	call   0x180324150
   18036903d:	48 8b 0d d4 8f 83 01 	mov    0x1838fd4(%rip),%rcx        # 0x181ba2018
   180369044:	48 8b e8             	mov    %rax,%rbp
   180369047:	4c 8b b3 b0 01 00 00 	mov    0x1b0(%rbx),%r14
   18036904e:	83 b9 e0 00 00 00 00 	cmpl   $0x0,0xe0(%rcx)
   180369055:	75 05                	jne    0x18036905c
   180369057:	e8 84 71 ee ff       	call   0x1802501e0
   18036905c:	45 33 c9             	xor    %r9d,%r9d
   18036905f:	48 8d 4c 24 60       	lea    0x60(%rsp),%rcx
   180369064:	4c 8b c6             	mov    %rsi,%r8
   180369067:	49 8b d6             	mov    %r14,%rdx
   18036906a:	e8 11 26 fb ff       	call   0x18031b680
   18036906f:	45 33 c9             	xor    %r9d,%r9d
   180369072:	48 c7 44 24 20 00 00 	movq   $0x0,0x20(%rsp)
   180369079:	00 00 
   18036907b:	4c 8b c5             	mov    %rbp,%r8
   18036907e:	8b d7                	mov    %edi,%edx
   180369080:	48 8b cb             	mov    %rbx,%rcx
   180369083:	f2 0f 10 38          	movsd  (%rax),%xmm7
   180369087:	8b 70 08             	mov    0x8(%rax),%esi
   18036908a:	e8 71 7c ff ff       	call   0x180360d00
   18036908f:	80 3d a0 ba 97 01 00 	cmpb   $0x0,0x197baa0(%rip)        # 0x181ce4b36
   180369096:	48 8b f8             	mov    %rax,%rdi
   180369099:	4c 8b b4 24 a8 00 00 	mov    0xa8(%rsp),%r14
   1803690a0:	00 
   1803690a1:	0f 57 f6             	xorps  %xmm6,%xmm6
   1803690a4:	75 13                	jne    0x1803690b9
   1803690a6:	48 8d 0d 3b 97 85 01 	lea    0x185973b(%rip),%rcx        # 0x181bc27e8
   1803690ad:	e8 3e 6e ee ff       	call   0x18024fef0
   1803690b2:	c6 05 7d ba 97 01 01 	movb   $0x1,0x197ba7d(%rip)        # 0x181ce4b36
   1803690b9:	48 8b 0d 28 97 85 01 	mov    0x1859728(%rip),%rcx        # 0x181bc27e8
   1803690c0:	48 8b 81 b8 00 00 00 	mov    0xb8(%rcx),%rax
   1803690c7:	48 85 ed             	test   %rbp,%rbp
   1803690ca:	0f 84 b4 00 00 00    	je     0x180369184
   1803690d0:	f2 0f 10 00          	movsd  (%rax),%xmm0
   1803690d4:	4c 8d 4c 24 40       	lea    0x40(%rsp),%r9
   1803690d9:	8b 40 08             	mov    0x8(%rax),%eax
   1803690dc:	4c 8d 44 24 50       	lea    0x50(%rsp),%r8
   1803690e1:	f2 0f 11 44 24 40    	movsd  %xmm0,0x40(%rsp)
   1803690e7:	48 8d 54 24 60       	lea    0x60(%rsp),%rdx
   1803690ec:	f3 0f 10 05 74 ec 23 	movss  0x123ec74(%rip),%xmm0        # 0x1815a7d68
   1803690f3:	01 
   1803690f4:	48 8b cd             	mov    %rbp,%rcx
   1803690f7:	0f 14 c6             	unpcklps %xmm6,%xmm0
   1803690fa:	f2 0f 11 44 24 50    	movsd  %xmm0,0x50(%rsp)
   180369100:	f3 0f 10 43 38       	movss  0x38(%rbx),%xmm0
   180369105:	f3 0f 58 05 33 e9 23 	addss  0x123e933(%rip),%xmm0        # 0x1815a7a40
   18036910c:	01 
   18036910d:	89 44 24 48          	mov    %eax,0x48(%rsp)
   180369111:	48 8b 05 08 27 85 01 	mov    0x1852708(%rip),%rax        # 0x181bbb820
   180369118:	48 89 44 24 38       	mov    %rax,0x38(%rsp)
   18036911d:	48 89 5c 24 30       	mov    %rbx,0x30(%rsp)
   180369122:	c7 44 24 28 00 00 00 	movl   $0x0,0x28(%rsp)
   180369129:	00 
   18036912a:	f3 0f 11 44 24 20    	movss  %xmm0,0x20(%rsp)
   180369130:	f3 0f 11 74 24 58    	movss  %xmm6,0x58(%rsp)
   180369136:	f2 0f 11 7c 24 60    	movsd  %xmm7,0x60(%rsp)
   18036913c:	89 74 24 68          	mov    %esi,0x68(%rsp)
   180369140:	e8 ab 94 13 00       	call   0x1804a25f0
   180369145:	44 8b 83 fc 00 00 00 	mov    0xfc(%rbx),%r8d
   18036914c:	45 33 c9             	xor    %r9d,%r9d
   18036914f:	48 8b d7             	mov    %rdi,%rdx
   180369152:	48 8b cd             	mov    %rbp,%rcx
   180369155:	e8 96 2f 01 00       	call   0x18037c0f0
   18036915a:	0f 28 b4 24 80 00 00 	movaps 0x80(%rsp),%xmm6
   180369161:	00 
   180369162:	48 8b ac 24 a0 00 00 	mov    0xa0(%rsp),%rbp
   180369169:	00 
   18036916a:	0f 28 7c 24 70       	movaps 0x70(%rsp),%xmm7
   18036916f:	4c 8d 9c 24 90 00 00 	lea    0x90(%rsp),%r11
   180369176:	00 
   180369177:	49 8b 5b 20          	mov    0x20(%r11),%rbx
   18036917b:	49 8b 73 28          	mov    0x28(%r11),%rsi
   18036917f:	49 8b e3             	mov    %r11,%rsp
   180369182:	5f                   	pop    %rdi
   180369183:	c3                   	ret
   180369184:	e8 c7 6f ee ff       	call   0x180250150
   180369189:	cc                   	int3
   18036918a:	66 90                	xchg   %ax,%ax
   18036918c:	f2 8f                	repnz (bad)
   18036918e:	36 00 ed             	ss add %ch,%ch
   180369191:	8f                   	(bad)
   180369192:	36 00 f2             	ss add %dh,%dl
   180369195:	8f                   	(bad)
   180369196:	36 00 f9             	ss add %bh,%cl
   180369199:	8f                   	(bad)
   18036919a:	36 00 f9             	ss add %bh,%cl
   18036919d:	8f                   	(bad)
   18036919e:	36 00 f9             	ss add %bh,%cl
   1803691a1:	8f                   	(bad)
   1803691a2:	36 00 f9             	ss add %bh,%cl
   1803691a5:	8f                   	(bad)
   1803691a6:	36 00 f9             	ss add %bh,%cl
   1803691a9:	8f                   	(bad)
   1803691aa:	36 00 f9             	ss add %bh,%cl
   1803691ad:	8f                   	(bad)
   1803691ae:	36 00 f9             	ss add %bh,%cl
   1803691b1:	8f                   	(bad)
   1803691b2:	36 00 f9             	ss add %bh,%cl
   1803691b5:	8f                   	(bad)
   1803691b6:	36 00 de             	ss add %bl,%dh
   1803691b9:	8f                   	(bad)
   1803691ba:	36 00 f9             	ss add %bh,%cl
   1803691bd:	8f                   	(bad)
   1803691be:	36 00 f2             	ss add %dh,%dl
   1803691c1:	8f                   	(bad)
   1803691c2:	36 00 ed             	ss add %ch,%ch
   1803691c5:	8f                   	(bad)
   1803691c6:	36 00 f2             	ss add %dh,%dl
   1803691c9:	8f                   	(bad)
   1803691ca:	36 00 f9             	ss add %bh,%cl
   1803691cd:	8f                   	(bad)
   1803691ce:	36                   	ss
	...
