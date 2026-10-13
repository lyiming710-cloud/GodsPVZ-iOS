
/workspaces/GodsPVZ-native24-codespace/.validation/native55-2026-10-02/native/0x060002CE.bin:     file format binary


Disassembly of section .data:

0000000180327700 <.data>:
   180327700:	40 53                	rex push %rbx
   180327702:	48 83 ec 40          	sub    $0x40,%rsp
   180327706:	80 3d 8e d5 9b 01 00 	cmpb   $0x0,0x19bd58e(%rip)        # 0x181ce4c9b
   18032770d:	48 8b d9             	mov    %rcx,%rbx
   180327710:	75 2b                	jne    0x18032773d
   180327712:	48 8d 0d ff a8 87 01 	lea    0x187a8ff(%rip),%rcx        # 0x181ba2018
   180327719:	e8 d2 87 f2 ff       	call   0x18024fef0
   18032771e:	48 8d 0d 53 cb 87 01 	lea    0x187cb53(%rip),%rcx        # 0x181ba4278
   180327725:	e8 c6 87 f2 ff       	call   0x18024fef0
   18032772a:	48 8d 0d 47 ed 88 01 	lea    0x188ed47(%rip),%rcx        # 0x181bb6478
   180327731:	e8 ba 87 f2 ff       	call   0x18024fef0
   180327736:	c6 05 5e d5 9b 01 01 	movb   $0x1,0x19bd55e(%rip)        # 0x181ce4c9b
   18032773d:	80 7b 38 00          	cmpb   $0x0,0x38(%rbx)
   180327741:	48 89 7c 24 58       	mov    %rdi,0x58(%rsp)
   180327746:	74 3d                	je     0x180327785
   180327748:	80 3d e3 d3 9b 01 00 	cmpb   $0x0,0x19bd3e3(%rip)        # 0x181ce4b32
   18032774f:	75 13                	jne    0x180327764
   180327751:	48 8d 0d e8 a2 8a 01 	lea    0x18aa2e8(%rip),%rcx        # 0x181bd1a40
   180327758:	e8 93 87 f2 ff       	call   0x18024fef0
   18032775d:	c6 05 ce d3 9b 01 01 	movb   $0x1,0x19bd3ce(%rip)        # 0x181ce4b32
   180327764:	48 8b 05 d5 a2 8a 01 	mov    0x18aa2d5(%rip),%rax        # 0x181bd1a40
   18032776b:	48 8b 88 b8 00 00 00 	mov    0xb8(%rax),%rcx
   180327772:	48 8b 09             	mov    (%rcx),%rcx
   180327775:	48 85 c9             	test   %rcx,%rcx
   180327778:	0f 84 93 01 00 00    	je     0x180327911
   18032777e:	33 d2                	xor    %edx,%edx
   180327780:	e8 7b 7d fe ff       	call   0x18030f500
   180327785:	80 bb e0 00 00 00 00 	cmpb   $0x0,0xe0(%rbx)
   18032778c:	0f 85 74 01 00 00    	jne    0x180327906
   180327792:	80 7b 48 00          	cmpb   $0x0,0x48(%rbx)
   180327796:	0f 85 6a 01 00 00    	jne    0x180327906
   18032779c:	80 7b 42 00          	cmpb   $0x0,0x42(%rbx)
   1803277a0:	c6 83 e0 00 00 00 01 	movb   $0x1,0xe0(%rbx)
   1803277a7:	0f 85 4f 01 00 00    	jne    0x1803278fc
   1803277ad:	48 8b 05 c4 ec 88 01 	mov    0x188ecc4(%rip),%rax        # 0x181bb6478
   1803277b4:	83 b8 e0 00 00 00 00 	cmpl   $0x0,0xe0(%rax)
   1803277bb:	75 0f                	jne    0x1803277cc
   1803277bd:	48 8b c8             	mov    %rax,%rcx
   1803277c0:	e8 1b 8a f2 ff       	call   0x1802501e0
   1803277c5:	48 8b 05 ac ec 88 01 	mov    0x188ecac(%rip),%rax        # 0x181bb6478
   1803277cc:	48 8b 80 b8 00 00 00 	mov    0xb8(%rax),%rax
   1803277d3:	48 8b 88 00 01 00 00 	mov    0x100(%rax),%rcx
   1803277da:	48 85 c9             	test   %rcx,%rcx
   1803277dd:	0f 84 2e 01 00 00    	je     0x180327911
   1803277e3:	4c 8b 05 8e ca 87 01 	mov    0x187ca8e(%rip),%r8        # 0x181ba4278
   1803277ea:	ba 0b 00 00 00       	mov    $0xb,%edx
   1803277ef:	e8 1c df 4f 00       	call   0x180825710
   1803277f4:	33 c9                	xor    %ecx,%ecx
   1803277f6:	48 8b f8             	mov    %rax,%rdi
   1803277f9:	e8 c2 af fb 00       	call   0x1812e27c0
   1803277fe:	48 85 c0             	test   %rax,%rax
   180327801:	0f 84 0a 01 00 00    	je     0x180327911
   180327807:	33 d2                	xor    %edx,%edx
   180327809:	48 8b c8             	mov    %rax,%rcx
   18032780c:	e8 ff cc fd 00       	call   0x181304510
   180327811:	48 85 c0             	test   %rax,%rax
   180327814:	0f 84 f7 00 00 00    	je     0x180327911
   18032781a:	48 89 74 24 50       	mov    %rsi,0x50(%rsp)
   18032781f:	48 8d 4c 24 20       	lea    0x20(%rsp),%rcx
   180327824:	45 33 c0             	xor    %r8d,%r8d
   180327827:	0f 29 74 24 30       	movaps %xmm6,0x30(%rsp)
   18032782c:	48 8b d0             	mov    %rax,%rdx
   18032782f:	e8 6c 7b 00 01       	call   0x18132f3a0
   180327834:	48 8b 0d dd a7 87 01 	mov    0x187a7dd(%rip),%rcx        # 0x181ba2018
   18032783b:	f2 0f 10 30          	movsd  (%rax),%xmm6
   18032783f:	83 b9 e0 00 00 00 00 	cmpl   $0x0,0xe0(%rcx)
   180327846:	8b 70 08             	mov    0x8(%rax),%esi
   180327849:	75 05                	jne    0x180327850
   18032784b:	e8 90 89 f2 ff       	call   0x1802501e0
   180327850:	33 c9                	xor    %ecx,%ecx
   180327852:	e8 49 36 ff ff       	call   0x18031aea0
   180327857:	45 33 c9             	xor    %r9d,%r9d
   18032785a:	f2 0f 11 74 24 20    	movsd  %xmm6,0x20(%rsp)
   180327860:	0f 28 d0             	movaps %xmm0,%xmm2
   180327863:	89 74 24 28          	mov    %esi,0x28(%rsp)
   180327867:	48 8d 54 24 20       	lea    0x20(%rsp),%rdx
   18032786c:	48 8b cf             	mov    %rdi,%rcx
   18032786f:	e8 bc 39 ff ff       	call   0x18031b230
   180327874:	48 8b 8b 90 00 00 00 	mov    0x90(%rbx),%rcx
   18032787b:	0f 28 74 24 30       	movaps 0x30(%rsp),%xmm6
   180327880:	48 8b 74 24 50       	mov    0x50(%rsp),%rsi
   180327885:	48 85 c9             	test   %rcx,%rcx
   180327888:	0f 84 83 00 00 00    	je     0x180327911
   18032788e:	45 33 c0             	xor    %r8d,%r8d
   180327891:	b2 01                	mov    $0x1,%dl
   180327893:	e8 38 42 ff 00       	call   0x18131bad0
   180327898:	80 3d f3 d3 9b 01 00 	cmpb   $0x0,0x19bd3f3(%rip)        # 0x181ce4c92
   18032789f:	75 1f                	jne    0x1803278c0
   1803278a1:	48 8d 0d a0 34 87 01 	lea    0x18734a0(%rip),%rcx        # 0x181b9ad48
   1803278a8:	e8 43 86 f2 ff       	call   0x18024fef0
   1803278ad:	48 8d 0d 84 f6 89 01 	lea    0x189f684(%rip),%rcx        # 0x181bc6f38
   1803278b4:	e8 37 86 f2 ff       	call   0x18024fef0
   1803278b9:	c6 05 d2 d3 9b 01 01 	movb   $0x1,0x19bd3d2(%rip)        # 0x181ce4c92
   1803278c0:	48 8b 0d 81 34 87 01 	mov    0x1873481(%rip),%rcx        # 0x181b9ad48
   1803278c7:	83 b9 e0 00 00 00 00 	cmpl   $0x0,0xe0(%rcx)
   1803278ce:	75 05                	jne    0x1803278d5
   1803278d0:	e8 0b 89 f2 ff       	call   0x1802501e0
   1803278d5:	48 8b 0d 5c f6 89 01 	mov    0x189f65c(%rip),%rcx        # 0x181bc6f38
   1803278dc:	33 d2                	xor    %edx,%edx
   1803278de:	e8 0d f0 fb 00       	call   0x1812e68f0
   1803278e3:	33 d2                	xor    %edx,%edx
   1803278e5:	c6 43 42 01          	movb   $0x1,0x42(%rbx)
   1803278e9:	0f 57 c0             	xorps  %xmm0,%xmm0
   1803278ec:	e8 0f 4c 00 01       	call   0x18132c500
   1803278f1:	48 8b 7c 24 58       	mov    0x58(%rsp),%rdi
   1803278f6:	48 83 c4 40          	add    $0x40,%rsp
   1803278fa:	5b                   	pop    %rbx
   1803278fb:	c3                   	ret
   1803278fc:	33 d2                	xor    %edx,%edx
   1803278fe:	48 8b cb             	mov    %rbx,%rcx
   180327901:	e8 fa ed ff ff       	call   0x180326700
   180327906:	48 8b 7c 24 58       	mov    0x58(%rsp),%rdi
   18032790b:	48 83 c4 40          	add    $0x40,%rsp
   18032790f:	5b                   	pop    %rbx
   180327910:	c3                   	ret
   180327911:	e8 3a 88 f2 ff       	call   0x180250150
   180327916:	cc                   	int3
