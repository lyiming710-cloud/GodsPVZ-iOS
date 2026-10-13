
/workspaces/GodsPVZ-native24-codespace/.validation/native55-2026-10-02/native/0x06000386.bin:     file format binary


Disassembly of section .data:

0000000180353a90 <.data>:
   180353a90:	40 53                	rex push %rbx
   180353a92:	48 83 ec 70          	sub    $0x70,%rsp
   180353a96:	80 3d 77 12 99 01 00 	cmpb   $0x0,0x1991277(%rip)        # 0x181ce4d14
   180353a9d:	48 8b d9             	mov    %rcx,%rbx
   180353aa0:	75 2b                	jne    0x180353acd
   180353aa2:	48 8d 0d 6f e5 84 01 	lea    0x184e56f(%rip),%rcx        # 0x181ba2018
   180353aa9:	e8 42 c4 ef ff       	call   0x18024fef0
   180353aae:	48 8d 0d c3 07 85 01 	lea    0x18507c3(%rip),%rcx        # 0x181ba4278
   180353ab5:	e8 36 c4 ef ff       	call   0x18024fef0
   180353aba:	48 8d 0d b7 29 86 01 	lea    0x18629b7(%rip),%rcx        # 0x181bb6478
   180353ac1:	e8 2a c4 ef ff       	call   0x18024fef0
   180353ac6:	c6 05 47 12 99 01 01 	movb   $0x1,0x1991247(%rip)        # 0x181ce4d14
   180353acd:	48 89 b4 24 80 00 00 	mov    %rsi,0x80(%rsp)
   180353ad4:	00 
   180353ad5:	45 33 c9             	xor    %r9d,%r9d
   180353ad8:	33 d2                	xor    %edx,%edx
   180353ada:	48 89 bc 24 88 00 00 	mov    %rdi,0x88(%rsp)
   180353ae1:	00 
   180353ae2:	41 b8 ff ff ff ff    	mov    $0xffffffff,%r8d
   180353ae8:	48 c7 44 24 20 00 00 	movq   $0x0,0x20(%rsp)
   180353aef:	00 00 
   180353af1:	48 8b cb             	mov    %rbx,%rcx
   180353af4:	e8 47 d3 ff ff       	call   0x180350e40
   180353af9:	48 85 c0             	test   %rax,%rax
   180353afc:	0f 84 29 02 00 00    	je     0x180353d2b
   180353b02:	33 d2                	xor    %edx,%edx
   180353b04:	48 8b c8             	mov    %rax,%rcx
   180353b07:	e8 84 dd fb ff       	call   0x180311890
   180353b0c:	48 8b 8b e8 01 00 00 	mov    0x1e8(%rbx),%rcx
   180353b13:	48 85 c9             	test   %rcx,%rcx
   180353b16:	0f 84 0f 02 00 00    	je     0x180353d2b
   180353b1c:	f3 0f 10 15 bc 40 25 	movss  0x12540bc(%rip),%xmm2        # 0x1815a7be0
   180353b23:	01 
   180353b24:	45 33 c9             	xor    %r9d,%r9d
   180353b27:	f3 0f 10 0d d5 40 25 	movss  0x12540d5(%rip),%xmm1        # 0x1815a7c04
   180353b2e:	01 
   180353b2f:	e8 ac 3b fd ff       	call   0x1803276e0
   180353b34:	80 3d f9 0f 99 01 00 	cmpb   $0x0,0x1990ff9(%rip)        # 0x181ce4b34
   180353b3b:	75 13                	jne    0x180353b50
   180353b3d:	48 8d 0d 4c dd 85 01 	lea    0x185dd4c(%rip),%rcx        # 0x181bb1890
   180353b44:	e8 a7 c3 ef ff       	call   0x18024fef0
   180353b49:	c6 05 e4 0f 99 01 01 	movb   $0x1,0x1990fe4(%rip)        # 0x181ce4b34
   180353b50:	48 8b 05 39 dd 85 01 	mov    0x185dd39(%rip),%rax        # 0x181bb1890
   180353b57:	48 8b 88 b8 00 00 00 	mov    0xb8(%rax),%rcx
   180353b5e:	48 8b 09             	mov    (%rcx),%rcx
   180353b61:	48 85 c9             	test   %rcx,%rcx
   180353b64:	0f 84 c1 01 00 00    	je     0x180353d2b
   180353b6a:	45 33 c0             	xor    %r8d,%r8d
   180353b6d:	ba 17 00 00 00       	mov    $0x17,%edx
   180353b72:	e8 b9 e3 fc ff       	call   0x180321f30
   180353b77:	48 8b f8             	mov    %rax,%rdi
   180353b7a:	48 85 c0             	test   %rax,%rax
   180353b7d:	0f 84 a8 01 00 00    	je     0x180353d2b
   180353b83:	33 d2                	xor    %edx,%edx
   180353b85:	48 8b c8             	mov    %rax,%rcx
   180353b88:	e8 e3 84 fc 00       	call   0x18131c070
   180353b8d:	33 d2                	xor    %edx,%edx
   180353b8f:	48 8b cb             	mov    %rbx,%rcx
   180353b92:	48 8b f0             	mov    %rax,%rsi
   180353b95:	e8 76 09 fb 00       	call   0x181304510
   180353b9a:	48 85 c0             	test   %rax,%rax
   180353b9d:	0f 84 88 01 00 00    	je     0x180353d2b
   180353ba3:	45 33 c0             	xor    %r8d,%r8d
   180353ba6:	48 8d 4c 24 40       	lea    0x40(%rsp),%rcx
   180353bab:	48 8b d0             	mov    %rax,%rdx
   180353bae:	e8 ed b7 fd 00       	call   0x18132f3a0
   180353bb3:	48 85 f6             	test   %rsi,%rsi
   180353bb6:	0f 84 6f 01 00 00    	je     0x180353d2b
   180353bbc:	f2 0f 10 00          	movsd  (%rax),%xmm0
   180353bc0:	48 8d 54 24 30       	lea    0x30(%rsp),%rdx
   180353bc5:	8b 40 08             	mov    0x8(%rax),%eax
   180353bc8:	45 33 c0             	xor    %r8d,%r8d
   180353bcb:	48 8b ce             	mov    %rsi,%rcx
   180353bce:	f2 0f 11 44 24 30    	movsd  %xmm0,0x30(%rsp)
   180353bd4:	89 44 24 38          	mov    %eax,0x38(%rsp)
   180353bd8:	e8 d3 bf fd 00       	call   0x18132fbb0
   180353bdd:	33 d2                	xor    %edx,%edx
   180353bdf:	48 8b cf             	mov    %rdi,%rcx
   180353be2:	e8 89 84 fc 00       	call   0x18131c070
   180353be7:	f3 0f 10 0d d9 3e 25 	movss  0x1253ed9(%rip),%xmm1        # 0x1815a7ac8
   180353bee:	01 
   180353bef:	48 85 c0             	test   %rax,%rax
   180353bf2:	0f 84 33 01 00 00    	je     0x180353d2b
   180353bf8:	0f 28 c1             	movaps %xmm1,%xmm0
   180353bfb:	f3 0f 11 4c 24 38    	movss  %xmm1,0x38(%rsp)
   180353c01:	0f 14 c1             	unpcklps %xmm1,%xmm0
   180353c04:	48 8d 54 24 30       	lea    0x30(%rsp),%rdx
   180353c09:	45 33 c0             	xor    %r8d,%r8d
   180353c0c:	f2 0f 11 44 24 30    	movsd  %xmm0,0x30(%rsp)
   180353c12:	48 8b c8             	mov    %rax,%rcx
   180353c15:	e8 36 be fd 00       	call   0x18132fa50
   180353c1a:	48 8b 05 57 28 86 01 	mov    0x1862857(%rip),%rax        # 0x181bb6478
   180353c21:	83 b8 e0 00 00 00 00 	cmpl   $0x0,0xe0(%rax)
   180353c28:	75 0f                	jne    0x180353c39
   180353c2a:	48 8b c8             	mov    %rax,%rcx
   180353c2d:	e8 ae c5 ef ff       	call   0x1802501e0
   180353c32:	48 8b 05 3f 28 86 01 	mov    0x186283f(%rip),%rax        # 0x181bb6478
   180353c39:	48 8b 80 b8 00 00 00 	mov    0xb8(%rax),%rax
   180353c40:	48 8b 88 30 01 00 00 	mov    0x130(%rax),%rcx
   180353c47:	48 85 c9             	test   %rcx,%rcx
   180353c4a:	0f 84 db 00 00 00    	je     0x180353d2b
   180353c50:	4c 8b 05 21 06 85 01 	mov    0x1850621(%rip),%r8        # 0x181ba4278
   180353c57:	33 d2                	xor    %edx,%edx
   180353c59:	e8 b2 1a 4d 00       	call   0x180825710
   180353c5e:	33 c9                	xor    %ecx,%ecx
   180353c60:	48 8b f8             	mov    %rax,%rdi
   180353c63:	e8 58 eb f8 00       	call   0x1812e27c0
   180353c68:	48 85 c0             	test   %rax,%rax
   180353c6b:	0f 84 ba 00 00 00    	je     0x180353d2b
   180353c71:	33 d2                	xor    %edx,%edx
   180353c73:	48 8b c8             	mov    %rax,%rcx
   180353c76:	e8 95 08 fb 00       	call   0x181304510
   180353c7b:	48 85 c0             	test   %rax,%rax
   180353c7e:	0f 84 a7 00 00 00    	je     0x180353d2b
   180353c84:	0f 29 74 24 60       	movaps %xmm6,0x60(%rsp)
   180353c89:	48 8d 4c 24 40       	lea    0x40(%rsp),%rcx
   180353c8e:	45 33 c0             	xor    %r8d,%r8d
   180353c91:	0f 29 7c 24 50       	movaps %xmm7,0x50(%rsp)
   180353c96:	48 8b d0             	mov    %rax,%rdx
   180353c99:	e8 02 b7 fd 00       	call   0x18132f3a0
   180353c9e:	48 8b 0d 73 e3 84 01 	mov    0x184e373(%rip),%rcx        # 0x181ba2018
   180353ca5:	f2 0f 10 38          	movsd  (%rax),%xmm7
   180353ca9:	83 b9 e0 00 00 00 00 	cmpl   $0x0,0xe0(%rcx)
   180353cb0:	8b 70 08             	mov    0x8(%rax),%esi
   180353cb3:	75 05                	jne    0x180353cba
   180353cb5:	e8 26 c5 ef ff       	call   0x1802501e0
   180353cba:	33 c9                	xor    %ecx,%ecx
   180353cbc:	e8 df 71 fc ff       	call   0x18031aea0
   180353cc1:	f3 0f 10 0d 4b 3d 25 	movss  0x1253d4b(%rip),%xmm1        # 0x1815a7a14
   180353cc8:	01 
   180353cc9:	0f 28 f0             	movaps %xmm0,%xmm6
   180353ccc:	f3 0f 10 05 f0 3d 25 	movss  0x1253df0(%rip),%xmm0        # 0x1815a7ac4
   180353cd3:	01 
   180353cd4:	45 33 c0             	xor    %r8d,%r8d
   180353cd7:	e8 74 96 fb 00       	call   0x18130d350
   180353cdc:	0f 28 d8             	movaps %xmm0,%xmm3
   180353cdf:	f2 0f 11 7c 24 30    	movsd  %xmm7,0x30(%rsp)
   180353ce5:	0f 28 d6             	movaps %xmm6,%xmm2
   180353ce8:	89 74 24 38          	mov    %esi,0x38(%rsp)
   180353cec:	48 8d 54 24 30       	lea    0x30(%rsp),%rdx
   180353cf1:	48 c7 44 24 20 00 00 	movq   $0x0,0x20(%rsp)
   180353cf8:	00 00 
   180353cfa:	48 8b cf             	mov    %rdi,%rcx
   180353cfd:	e8 4e 76 fc ff       	call   0x18031b350
   180353d02:	33 d2                	xor    %edx,%edx
   180353d04:	48 8b cb             	mov    %rbx,%rcx
   180353d07:	0f 28 7c 24 50       	movaps 0x50(%rsp),%xmm7
   180353d0c:	0f 28 74 24 60       	movaps 0x60(%rsp),%xmm6
   180353d11:	48 8b bc 24 88 00 00 	mov    0x88(%rsp),%rdi
   180353d18:	00 
   180353d19:	48 8b b4 24 80 00 00 	mov    0x80(%rsp),%rsi
   180353d20:	00 
   180353d21:	48 83 c4 70          	add    $0x70,%rsp
   180353d25:	5b                   	pop    %rbx
   180353d26:	e9 35 24 00 00       	jmp    0x180356160
   180353d2b:	e8 20 c4 ef ff       	call   0x180250150
   180353d30:	cc                   	int3
