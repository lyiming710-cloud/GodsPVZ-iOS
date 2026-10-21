00000001803BA0B0 cmp       edx, -1
00000001803BA0B3 je        0x1803bab70
00000001803BA0B9 push      rdi
00000001803BA0BA sub       rsp, 0x20
00000001803BA0BE mov       rax, qword ptr [rcx + 0x38]
00000001803BA0C2 mov       rdi, rcx
00000001803BA0C5 mov       qword ptr [rsp + 0x30], rbx
00000001803BA0CA test      rax, rax
00000001803BA0CD je        0x1803bab71
00000001803BA0D3 mov       rax, qword ptr [rax + 0x370]
00000001803BA0DA test      rax, rax
00000001803BA0DD je        0x1803bab71
00000001803BA0E3 mov       eax, dword ptr [rax + 0x18]
00000001803BA0E6 dec       eax
00000001803BA0E8 cmp       edx, eax
00000001803BA0EA jg        0x1803bab66
00000001803BA0F0 mov       rax, qword ptr [rcx + 0x38]
00000001803BA0F4 mov       rcx, qword ptr [rax + 0x370]
00000001803BA0FB mov       r8, qword ptr [rcx + 0x38]
00000001803BA0FF test      r8, r8
00000001803BA102 je        0x1803bab71
00000001803BA108 cmp       edx, dword ptr [r8 + 0x18]
00000001803BA10C jae       0x1803bab77
00000001803BA112 movsxd    rax, edx
00000001803BA115 imul      rcx, rax, 0x178
00000001803BA11C mov       rax, qword ptr [rdi + 0x38]
00000001803BA120 mov       r9, qword ptr [rax + 0x370]
00000001803BA127 movsxd    r8, dword ptr [rcx + r8 + 0x58]
00000001803BA12C test      r9, r9
00000001803BA12F je        0x1803bab71
00000001803BA135 mov       r9, qword ptr [r9 + 0x38]
00000001803BA139 test      r9, r9
00000001803BA13C je        0x1803bab71
00000001803BA142 cmp       edx, dword ptr [r9 + 0x18]
00000001803BA146 jae       0x1803bab77
00000001803BA14C mov       rbx, qword ptr [rdi + 0xa0]
00000001803BA153 movsxd    rax, edx
00000001803BA156 imul      rcx, rax, 0x178
00000001803BA15D movsxd    rdx, dword ptr [rcx + r9 + 0x6c]
00000001803BA162 test      rbx, rbx
00000001803BA165 je        0x1803bab71
00000001803BA16B cmp       r8d, dword ptr [rbx + 0x18]
00000001803BA16F jae       0x1803bab77
00000001803BA175 mov       rax, qword ptr [rdi + 0x38]
00000001803BA179 lea       rcx, [r8 + r8*4]
00000001803BA17D add       rcx, rcx
00000001803BA180 mov       r11, qword ptr [rax + 0x370]
00000001803BA187 mov       rbx, qword ptr [rbx + rcx*8 + 0x30]
00000001803BA18C test      r11, r11
00000001803BA18F je        0x1803bab71
00000001803BA195 mov       r11, qword ptr [r11 + 0x60]
00000001803BA199 test      r11, r11
00000001803BA19C je        0x1803bab71
00000001803BA1A2 cmp       r8d, dword ptr [r11 + 0x18]
00000001803BA1A6 jae       0x1803bab77
00000001803BA1AC lea       rcx, [r8 + r8*4]
00000001803BA1B0 add       rcx, rcx
00000001803BA1B3 mov       r11, qword ptr [r11 + rcx*8 + 0x30]
00000001803BA1B8 test      rbx, rbx
00000001803BA1BB je        0x1803bab71
00000001803BA1C1 cmp       edx, dword ptr [rbx + 0x18]
00000001803BA1C4 jae       0x1803bab77
00000001803BA1CA lea       rcx, [rdx + rdx*2]
00000001803BA1CE lea       r9, [rbx + rcx*4]
00000001803BA1D2 test      r11, r11
00000001803BA1D5 je        0x1803bab71
00000001803BA1DB cmp       edx, dword ptr [r11 + 0x18]
00000001803BA1DF jae       0x1803bab77
00000001803BA1E5 movsd     xmm0, qword ptr [r9 + 0x20]
00000001803BA1EB lea       rcx, [rdx + rdx*2]
00000001803BA1EF movsd     qword ptr [r11 + rcx*4 + 0x20], xmm0
00000001803BA1F6 mov       eax, dword ptr [r9 + 0x28]
00000001803BA1FA mov       dword ptr [r11 + rcx*4 + 0x28], eax
00000001803BA1FF lea       rax, [rdx + 1]
00000001803BA203 cmp       eax, dword ptr [rbx + 0x18]
00000001803BA206 jae       0x1803bab77
00000001803BA20C lea       rax, [rax + rax*2]
00000001803BA210 lea       r9, [rbx + rax*4]
00000001803BA214 lea       rax, [rdx + 1]
00000001803BA218 cmp       eax, dword ptr [r11 + 0x18]
00000001803BA21C jae       0x1803bab77
00000001803BA222 movsd     xmm0, qword ptr [r9 + 0x20]
00000001803BA228 lea       rax, [rax + rax*2]
00000001803BA22C lea       rcx, [r11 + rax*4]
00000001803BA230 movsd     qword ptr [rcx + 0x20], xmm0
00000001803BA235 mov       eax, dword ptr [r9 + 0x28]
00000001803BA239 mov       dword ptr [rcx + 0x28], eax
00000001803BA23C lea       rax, [rdx + 2]
00000001803BA240 cmp       eax, dword ptr [rbx + 0x18]
00000001803BA243 jae       0x1803bab77
00000001803BA249 lea       rax, [rax + rax*2]
00000001803BA24D lea       r9, [rbx + rax*4]
00000001803BA251 lea       rax, [rdx + 2]
00000001803BA255 cmp       eax, dword ptr [r11 + 0x18]
00000001803BA259 jae       0x1803bab77
00000001803BA25F movsd     xmm0, qword ptr [r9 + 0x20]
00000001803BA265 lea       rax, [rax + rax*2]
00000001803BA269 lea       rcx, [r11 + rax*4]
00000001803BA26D movsd     qword ptr [rcx + 0x20], xmm0
00000001803BA272 mov       eax, dword ptr [r9 + 0x28]
00000001803BA276 mov       dword ptr [rcx + 0x28], eax
00000001803BA279 lea       rax, [rdx + 3]
00000001803BA27D cmp       eax, dword ptr [rbx + 0x18]
00000001803BA280 jae       0x1803bab77
00000001803BA286 lea       rax, [rax + rax*2]
00000001803BA28A lea       r9, [rbx + rax*4]
00000001803BA28E lea       rax, [rdx + 3]
00000001803BA292 cmp       eax, dword ptr [r11 + 0x18]
00000001803BA296 jae       0x1803bab77
00000001803BA29C movsd     xmm0, qword ptr [r9 + 0x20]
00000001803BA2A2 lea       rax, [rax + rax*2]
00000001803BA2A6 lea       rcx, [r11 + rax*4]
00000001803BA2AA movsd     qword ptr [rcx + 0x20], xmm0
00000001803BA2AF mov       eax, dword ptr [r9 + 0x28]
00000001803BA2B3 mov       dword ptr [rcx + 0x28], eax
00000001803BA2B6 mov       r10, qword ptr [rdi + 0x38]
00000001803BA2BA test      r10, r10
00000001803BA2BD je        0x1803bab71
00000001803BA2C3 mov       r10, qword ptr [r10 + 0x370]
00000001803BA2CA test      r10, r10
00000001803BA2CD je        0x1803bab71
00000001803BA2D3 mov       r10, qword ptr [r10 + 0x60]
00000001803BA2D7 test      r10, r10
00000001803BA2DA je        0x1803bab71
00000001803BA2E0 cmp       r8d, dword ptr [r10 + 0x18]
00000001803BA2E4 jae       0x1803bab77
00000001803BA2EA mov       r9, qword ptr [rdi + 0xa0]
00000001803BA2F1 lea       rcx, [r8 + r8*4]
00000001803BA2F5 add       rcx, rcx
00000001803BA2F8 mov       r10, qword ptr [r10 + rcx*8 + 0x58]
00000001803BA2FD test      r9, r9
00000001803BA300 je        0x1803bab71
00000001803BA306 cmp       r8d, dword ptr [r9 + 0x18]
00000001803BA30A jae       0x1803bab77
00000001803BA310 lea       rcx, [r8 + r8*4]
00000001803BA314 add       rcx, rcx
00000001803BA317 mov       r9, qword ptr [r9 + rcx*8 + 0x58]
00000001803BA31C test      r9, r9
00000001803BA31F je        0x1803bab71
00000001803BA325 cmp       edx, dword ptr [r9 + 0x18]
00000001803BA329 jae       0x1803bab77
00000001803BA32F test      r10, r10
00000001803BA332 je        0x1803bab71
00000001803BA338 mov       eax, dword ptr [r9 + rdx*4 + 0x20]
00000001803BA33D cmp       edx, dword ptr [r10 + 0x18]
00000001803BA341 jae       0x1803bab77
00000001803BA347 mov       dword ptr [r10 + rdx*4 + 0x20], eax
00000001803BA34C lea       rax, [rdx + 1]
00000001803BA350 cmp       eax, dword ptr [r9 + 0x18]
00000001803BA354 jae       0x1803bab77
00000001803BA35A mov       eax, dword ptr [r9 + rax*4 + 0x20]
00000001803BA35F lea       rcx, [rdx + 1]
00000001803BA363 cmp       ecx, dword ptr [r10 + 0x18]
00000001803BA367 jae       0x1803bab77
00000001803BA36D mov       dword ptr [r10 + rcx*4 + 0x20], eax
00000001803BA372 lea       rax, [rdx + 2]
00000001803BA376 cmp       eax, dword ptr [r9 + 0x18]
00000001803BA37A jae       0x1803bab77
00000001803BA380 mov       eax, dword ptr [r9 + rax*4 + 0x20]
00000001803BA385 lea       rcx, [rdx + 2]
00000001803BA389 cmp       ecx, dword ptr [r10 + 0x18]
00000001803BA38D jae       0x1803bab77
00000001803BA393 mov       dword ptr [r10 + rcx*4 + 0x20], eax
00000001803BA398 lea       rax, [rdx + 3]
00000001803BA39C cmp       eax, dword ptr [r9 + 0x18]
00000001803BA3A0 jae       0x1803bab77
00000001803BA3A6 mov       eax, dword ptr [r9 + rax*4 + 0x20]
00000001803BA3AB lea       rcx, [rdx + 3]
00000001803BA3AF cmp       ecx, dword ptr [r10 + 0x18]
00000001803BA3B3 jae       0x1803bab77
00000001803BA3B9 mov       dword ptr [r10 + rcx*4 + 0x20], eax
00000001803BA3BE mov       r10, qword ptr [rdi + 0xa0]
00000001803BA3C5 test      r10, r10
00000001803BA3C8 je        0x1803bab71
00000001803BA3CE cmp       r8d, dword ptr [r10 + 0x18]
00000001803BA3D2 jae       0x1803bab77
00000001803BA3D8 mov       r9, qword ptr [rdi + 0x38]
00000001803BA3DC lea       rcx, [r8 + r8*4]
00000001803BA3E0 add       rcx, rcx
00000001803BA3E3 mov       r10, qword ptr [r10 + rcx*8 + 0x48]
00000001803BA3E8 test      r9, r9
00000001803BA3EB je        0x1803bab71
00000001803BA3F1 mov       r9, qword ptr [r9 + 0x370]
00000001803BA3F8 test      r9, r9
00000001803BA3FB je        0x1803bab71
00000001803BA401 mov       r9, qword ptr [r9 + 0x60]
00000001803BA405 test      r9, r9
00000001803BA408 je        0x1803bab71
00000001803BA40E cmp       r8d, dword ptr [r9 + 0x18]
00000001803BA412 jae       0x1803bab77
00000001803BA418 lea       rcx, [r8 + r8*4]
00000001803BA41C add       rcx, rcx
00000001803BA41F mov       r9, qword ptr [r9 + rcx*8 + 0x48]
00000001803BA424 test      r10, r10
00000001803BA427 je        0x1803bab71
00000001803BA42D cmp       edx, dword ptr [r10 + 0x18]
00000001803BA431 jae       0x1803bab77
00000001803BA437 movss     xmm0, dword ptr [r10 + rdx*8 + 0x20]
00000001803BA43E movss     xmm1, dword ptr [r10 + rdx*8 + 0x24]
00000001803BA445 test      r9, r9
00000001803BA448 je        0x1803bab71
00000001803BA44E cmp       edx, dword ptr [r9 + 0x18]
00000001803BA452 jae       0x1803bab77
00000001803BA458 lea       rax, [rdx + 1]
00000001803BA45C movss     dword ptr [r9 + rdx*8 + 0x20], xmm0
00000001803BA463 movss     dword ptr [r9 + rdx*8 + 0x24], xmm1
00000001803BA46A cmp       eax, dword ptr [r10 + 0x18]
00000001803BA46E jae       0x1803bab77
00000001803BA474 movss     xmm0, dword ptr [r10 + rax*8 + 0x20]
00000001803BA47B movss     xmm1, dword ptr [r10 + rax*8 + 0x24]
00000001803BA482 lea       rax, [rdx + 1]
00000001803BA486 cmp       eax, dword ptr [r9 + 0x18]
00000001803BA48A jae       0x1803bab77
00000001803BA490 movss     dword ptr [r9 + rax*8 + 0x20], xmm0
00000001803BA497 movss     dword ptr [r9 + rax*8 + 0x24], xmm1
00000001803BA49E lea       rax, [rdx + 2]
00000001803BA4A2 cmp       eax, dword ptr [r10 + 0x18]
00000001803BA4A6 jae       0x1803bab77
00000001803BA4AC movss     xmm0, dword ptr [r10 + rax*8 + 0x20]
00000001803BA4B3 movss     xmm1, dword ptr [r10 + rax*8 + 0x24]
00000001803BA4BA lea       rax, [rdx + 2]
00000001803BA4BE cmp       eax, dword ptr [r9 + 0x18]
00000001803BA4C2 jae       0x1803bab77
00000001803BA4C8 movss     dword ptr [r9 + rax*8 + 0x20], xmm0
00000001803BA4CF movss     dword ptr [r9 + rax*8 + 0x24], xmm1
00000001803BA4D6 lea       rax, [rdx + 3]
00000001803BA4DA cmp       eax, dword ptr [r10 + 0x18]
00000001803BA4DE jae       0x1803bab77
00000001803BA4E4 movss     xmm0, dword ptr [r10 + rax*8 + 0x20]
00000001803BA4EB movss     xmm1, dword ptr [r10 + rax*8 + 0x24]
00000001803BA4F2 lea       rax, [rdx + 3]
00000001803BA4F6 cmp       eax, dword ptr [r9 + 0x18]
00000001803BA4FA jae       0x1803bab77
00000001803BA500 movss     dword ptr [r9 + rax*8 + 0x20], xmm0
00000001803BA507 movss     dword ptr [r9 + rax*8 + 0x24], xmm1
00000001803BA50E mov       rcx, qword ptr [rdi + 0xa0]
00000001803BA515 test      rcx, rcx
00000001803BA518 je        0x1803bab71
00000001803BA51E cmp       r8d, dword ptr [rcx + 0x18]
00000001803BA522 jae       0x1803bab77
00000001803BA528 mov       r9, qword ptr [rdi + 0x38]
00000001803BA52C lea       rax, [r8 + 1]
00000001803BA530 lea       rax, [rax + rax*4]
00000001803BA534 add       rax, rax
00000001803BA537 mov       rcx, qword ptr [rcx + rax*8]
00000001803BA53B test      r9, r9
00000001803BA53E je        0x1803bab71
00000001803BA544 mov       r9, qword ptr [r9 + 0x370]
00000001803BA54B test      r9, r9
00000001803BA54E je        0x1803bab71
00000001803BA554 mov       r9, qword ptr [r9 + 0x60]
00000001803BA558 test      r9, r9
00000001803BA55B je        0x1803bab71
00000001803BA561 cmp       r8d, dword ptr [r9 + 0x18]
00000001803BA565 jae       0x1803bab77
00000001803BA56B lea       rax, [r8 + 1]
00000001803BA56F lea       rax, [rax + rax*4]
00000001803BA573 add       rax, rax
00000001803BA576 mov       r9, qword ptr [r9 + rax*8]
00000001803BA57A test      rcx, rcx
00000001803BA57D je        0x1803bab71
00000001803BA583 cmp       edx, dword ptr [rcx + 0x18]
00000001803BA586 jae       0x1803bab77
00000001803BA58C movss     xmm0, dword ptr [rcx + rdx*8 + 0x20]
00000001803BA592 movss     xmm1, dword ptr [rcx + rdx*8 + 0x24]
00000001803BA598 test      r9, r9
00000001803BA59B je        0x1803bab71
00000001803BA5A1 cmp       edx, dword ptr [r9 + 0x18]
00000001803BA5A5 jae       0x1803bab77
00000001803BA5AB lea       rax, [rdx + 1]
00000001803BA5AF movss     dword ptr [r9 + rdx*8 + 0x20], xmm0
00000001803BA5B6 movss     dword ptr [r9 + rdx*8 + 0x24], xmm1
00000001803BA5BD cmp       eax, dword ptr [rcx + 0x18]
00000001803BA5C0 jae       0x1803bab77
00000001803BA5C6 movss     xmm0, dword ptr [rcx + rax*8 + 0x20]
00000001803BA5CC movss     xmm1, dword ptr [rcx + rax*8 + 0x24]
00000001803BA5D2 lea       rax, [rdx + 1]
00000001803BA5D6 cmp       eax, dword ptr [r9 + 0x18]
00000001803BA5DA jae       0x1803bab77
00000001803BA5E0 movss     dword ptr [r9 + rax*8 + 0x20], xmm0
00000001803BA5E7 movss     dword ptr [r9 + rax*8 + 0x24], xmm1
00000001803BA5EE lea       rax, [rdx + 2]
00000001803BA5F2 cmp       eax, dword ptr [rcx + 0x18]
00000001803BA5F5 jae       0x1803bab77
00000001803BA5FB movss     xmm0, dword ptr [rcx + rax*8 + 0x20]
00000001803BA601 movss     xmm1, dword ptr [rcx + rax*8 + 0x24]
00000001803BA607 lea       rax, [rdx + 2]
00000001803BA60B cmp       eax, dword ptr [r9 + 0x18]
00000001803BA60F jae       0x1803bab77
00000001803BA615 movss     dword ptr [r9 + rax*8 + 0x20], xmm0
00000001803BA61C movss     dword ptr [r9 + rax*8 + 0x24], xmm1
00000001803BA623 lea       rax, [rdx + 3]
00000001803BA627 cmp       eax, dword ptr [rcx + 0x18]
00000001803BA62A jae       0x1803bab77
00000001803BA630 movss     xmm0, dword ptr [rcx + rax*8 + 0x20]
00000001803BA636 movss     xmm1, dword ptr [rcx + rax*8 + 0x24]
00000001803BA63C lea       rax, [rdx + 3]
00000001803BA640 cmp       eax, dword ptr [r9 + 0x18]
00000001803BA644 jae       0x1803bab77
00000001803BA64A movss     dword ptr [r9 + rax*8 + 0x20], xmm0
00000001803BA651 movss     dword ptr [r9 + rax*8 + 0x24], xmm1
00000001803BA658 mov       eax, dword ptr [rbx + 0x18]
00000001803BA65B cdq       
00000001803BA65C and       edx, 3
00000001803BA65F add       eax, edx
00000001803BA661 sar       eax, 2
00000001803BA664 lea       r9d, [rax*4]
00000001803BA66C lea       edx, [r9 - 4]
00000001803BA670 cmp       edx, dword ptr [rbx + 0x18]
00000001803BA673 jae       0x1803bab77
00000001803BA679 movsxd    rax, r9d
00000001803BA67C lea       rcx, [rax + rax*2]
00000001803BA680 lea       r10, [rbx + rcx*4]
00000001803BA684 cmp       edx, dword ptr [r11 + 0x18]
00000001803BA688 jae       0x1803bab77
00000001803BA68E movsd     xmm0, qword ptr [r10 - 0x10]
00000001803BA694 movsxd    rax, r9d
00000001803BA697 lea       rcx, [rax + rax*2]
00000001803BA69B movsd     qword ptr [r11 + rcx*4 - 0x10], xmm0
00000001803BA6A2 mov       eax, dword ptr [r10 - 8]
00000001803BA6A6 mov       dword ptr [r11 + rcx*4 - 8], eax
00000001803BA6AB movsxd    rax, r9d
00000001803BA6AE add       rax, -3
00000001803BA6B2 cmp       eax, dword ptr [rbx + 0x18]
00000001803BA6B5 jae       0x1803bab77
00000001803BA6BB lea       rax, [rax + rax*2]
00000001803BA6BF lea       r10, [rbx + rax*4]
00000001803BA6C3 movsxd    rax, r9d
00000001803BA6C6 add       rax, -3
00000001803BA6CA cmp       eax, dword ptr [r11 + 0x18]
00000001803BA6CE jae       0x1803bab77
00000001803BA6D4 movsd     xmm0, qword ptr [r10 + 0x20]
00000001803BA6DA lea       rax, [rax + rax*2]
00000001803BA6DE lea       rcx, [r11 + rax*4]
00000001803BA6E2 movsd     qword ptr [rcx + 0x20], xmm0
00000001803BA6E7 mov       eax, dword ptr [r10 + 0x28]
00000001803BA6EB mov       dword ptr [rcx + 0x28], eax
00000001803BA6EE movsxd    rax, r9d
00000001803BA6F1 add       rax, -2
00000001803BA6F5 cmp       eax, dword ptr [rbx + 0x18]
00000001803BA6F8 jae       0x1803bab77
00000001803BA6FE lea       rax, [rax + rax*2]
00000001803BA702 lea       r10, [rbx + rax*4]
00000001803BA706 movsxd    rax, r9d
00000001803BA709 add       rax, -2
00000001803BA70D cmp       eax, dword ptr [r11 + 0x18]
00000001803BA711 jae       0x1803bab77
00000001803BA717 movsd     xmm0, qword ptr [r10 + 0x20]
00000001803BA71D lea       rax, [rax + rax*2]
00000001803BA721 lea       rcx, [r11 + rax*4]
00000001803BA725 movsd     qword ptr [rcx + 0x20], xmm0
00000001803BA72A mov       eax, dword ptr [r10 + 0x28]
00000001803BA72E mov       dword ptr [rcx + 0x28], eax
00000001803BA731 movsxd    rax, r9d
00000001803BA734 dec       rax
00000001803BA737 cmp       eax, dword ptr [rbx + 0x18]
00000001803BA73A jae       0x1803bab77
00000001803BA740 lea       rax, [rax + rax*2]
00000001803BA744 lea       r10, [rbx + rax*4]
00000001803BA748 movsxd    rax, r9d
00000001803BA74B dec       rax
00000001803BA74E cmp       eax, dword ptr [r11 + 0x18]
00000001803BA752 jae       0x1803bab77
00000001803BA758 movsd     xmm0, qword ptr [r10 + 0x20]
00000001803BA75E lea       rax, [rax + rax*2]
00000001803BA762 lea       rcx, [r11 + rax*4]
00000001803BA766 movsd     qword ptr [rcx + 0x20], xmm0
00000001803BA76B mov       eax, dword ptr [r10 + 0x28]
00000001803BA76F mov       dword ptr [rcx + 0x28], eax
00000001803BA772 mov       r11, qword ptr [rdi + 0xa0]
00000001803BA779 test      r11, r11
00000001803BA77C je        0x1803bab71
00000001803BA782 cmp       r8d, dword ptr [r11 + 0x18]
00000001803BA786 jae       0x1803bab77
00000001803BA78C mov       r10, qword ptr [rdi + 0x38]
00000001803BA790 lea       rcx, [r8 + r8*4]
00000001803BA794 add       rcx, rcx
00000001803BA797 mov       r11, qword ptr [r11 + rcx*8 + 0x58]
00000001803BA79C test      r10, r10
00000001803BA79F je        0x1803bab71
00000001803BA7A5 mov       r10, qword ptr [r10 + 0x370]
00000001803BA7AC test      r10, r10
00000001803BA7AF je        0x1803bab71
00000001803BA7B5 mov       r10, qword ptr [r10 + 0x60]
00000001803BA7B9 test      r10, r10
00000001803BA7BC je        0x1803bab71
00000001803BA7C2 cmp       r8d, dword ptr [r10 + 0x18]
00000001803BA7C6 jae       0x1803bab77
00000001803BA7CC lea       rcx, [r8 + r8*4]
00000001803BA7D0 add       rcx, rcx
00000001803BA7D3 mov       r10, qword ptr [r10 + rcx*8 + 0x58]
00000001803BA7D8 test      r11, r11
00000001803BA7DB je        0x1803bab71
00000001803BA7E1 cmp       edx, dword ptr [r11 + 0x18]
00000001803BA7E5 jae       0x1803bab77
00000001803BA7EB test      r10, r10
00000001803BA7EE je        0x1803bab71
00000001803BA7F4 movsxd    rax, r9d
00000001803BA7F7 mov       eax, dword ptr [r11 + rax*4 + 0x10]
00000001803BA7FC cmp       edx, dword ptr [r10 + 0x18]
00000001803BA800 jae       0x1803bab77
00000001803BA806 movsxd    rcx, r9d
00000001803BA809 mov       dword ptr [r10 + rcx*4 + 0x10], eax
00000001803BA80E movsxd    rax, r9d
00000001803BA811 add       rax, -3
00000001803BA815 cmp       eax, dword ptr [r11 + 0x18]
00000001803BA819 jae       0x1803bab77
00000001803BA81F mov       eax, dword ptr [r11 + rax*4 + 0x20]
00000001803BA824 movsxd    rcx, r9d
00000001803BA827 add       rcx, -3
00000001803BA82B cmp       ecx, dword ptr [r10 + 0x18]
00000001803BA82F jae       0x1803bab77
00000001803BA835 mov       dword ptr [r10 + rcx*4 + 0x20], eax
00000001803BA83A movsxd    rax, r9d
00000001803BA83D add       rax, -2
00000001803BA841 cmp       eax, dword ptr [r11 + 0x18]
00000001803BA845 jae       0x1803bab77
00000001803BA84B mov       eax, dword ptr [r11 + rax*4 + 0x20]
00000001803BA850 movsxd    rcx, r9d
00000001803BA853 add       rcx, -2
00000001803BA857 cmp       ecx, dword ptr [r10 + 0x18]
00000001803BA85B jae       0x1803bab77
00000001803BA861 mov       dword ptr [r10 + rcx*4 + 0x20], eax
00000001803BA866 movsxd    rax, edx
00000001803BA869 add       rax, 3
00000001803BA86D cmp       eax, dword ptr [r11 + 0x18]
00000001803BA871 jae       0x1803bab77
00000001803BA877 mov       eax, dword ptr [r11 + rax*4 + 0x20]
00000001803BA87C movsxd    rcx, edx
00000001803BA87F add       rcx, 3
00000001803BA883 cmp       ecx, dword ptr [r10 + 0x18]
00000001803BA887 jae       0x1803bab77
00000001803BA88D mov       dword ptr [r10 + rcx*4 + 0x20], eax
00000001803BA892 mov       r11, qword ptr [rdi + 0xa0]
00000001803BA899 test      r11, r11
00000001803BA89C je        0x1803bab71
00000001803BA8A2 cmp       r8d, dword ptr [r11 + 0x18]
00000001803BA8A6 jae       0x1803bab77
00000001803BA8AC mov       r10, qword ptr [rdi + 0x38]
00000001803BA8B0 lea       rcx, [r8 + r8*4]
00000001803BA8B4 add       rcx, rcx
00000001803BA8B7 mov       r11, qword ptr [r11 + rcx*8 + 0x48]
00000001803BA8BC test      r10, r10
00000001803BA8BF je        0x1803bab71
00000001803BA8C5 mov       r10, qword ptr [r10 + 0x370]
00000001803BA8CC test      r10, r10
00000001803BA8CF je        0x1803bab71
00000001803BA8D5 mov       r10, qword ptr [r10 + 0x60]
00000001803BA8D9 test      r10, r10
00000001803BA8DC je        0x1803bab71
00000001803BA8E2 cmp       r8d, dword ptr [r10 + 0x18]
00000001803BA8E6 jae       0x1803bab77
00000001803BA8EC lea       rcx, [r8 + r8*4]
00000001803BA8F0 add       rcx, rcx
00000001803BA8F3 mov       r10, qword ptr [r10 + rcx*8 + 0x48]
00000001803BA8F8 test      r11, r11
00000001803BA8FB je        0x1803bab71
00000001803BA901 cmp       edx, dword ptr [r11 + 0x18]
00000001803BA905 jae       0x1803bab77
00000001803BA90B movsxd    rax, r9d
00000001803BA90E movss     xmm0, dword ptr [r11 + rax*8]
00000001803BA914 movss     xmm1, dword ptr [r11 + rax*8 + 4]
00000001803BA91B test      r10, r10
00000001803BA91E je        0x1803bab71
00000001803BA924 cmp       edx, dword ptr [r10 + 0x18]
00000001803BA928 jae       0x1803bab77
00000001803BA92E movsxd    rax, r9d
00000001803BA931 movss     dword ptr [r10 + rax*8], xmm0
00000001803BA937 movss     dword ptr [r10 + rax*8 + 4], xmm1
00000001803BA93E movsxd    rax, edx
00000001803BA941 inc       rax
00000001803BA944 cmp       eax, dword ptr [r11 + 0x18]
00000001803BA948 jae       0x1803bab77
00000001803BA94E movss     xmm0, dword ptr [r11 + rax*8 + 0x20]
00000001803BA955 movss     xmm1, dword ptr [r11 + rax*8 + 0x24]
00000001803BA95C movsxd    rax, edx
00000001803BA95F inc       rax
00000001803BA962 cmp       eax, dword ptr [r10 + 0x18]
00000001803BA966 jae       0x1803bab77
00000001803BA96C movss     dword ptr [r10 + rax*8 + 0x20], xmm0
00000001803BA973 movss     dword ptr [r10 + rax*8 + 0x24], xmm1
00000001803BA97A movsxd    rax, edx
00000001803BA97D add       rax, 2
00000001803BA981 cmp       eax, dword ptr [r11 + 0x18]
00000001803BA985 jae       0x1803bab77
00000001803BA98B movss     xmm0, dword ptr [r11 + rax*8 + 0x20]
00000001803BA992 movss     xmm1, dword ptr [r11 + rax*8 + 0x24]
00000001803BA999 movsxd    rax, edx
00000001803BA99C add       rax, 2
00000001803BA9A0 cmp       eax, dword ptr [r10 + 0x18]
00000001803BA9A4 jae       0x1803bab77
00000001803BA9AA movss     dword ptr [r10 + rax*8 + 0x20], xmm0
00000001803BA9B1 movss     dword ptr [r10 + rax*8 + 0x24], xmm1
00000001803BA9B8 movsxd    rax, edx
00000001803BA9BB add       rax, 3
00000001803BA9BF cmp       eax, dword ptr [r11 + 0x18]
00000001803BA9C3 jae       0x1803bab77
00000001803BA9C9 movss     xmm0, dword ptr [r11 + rax*8 + 0x20]
00000001803BA9D0 movss     xmm1, dword ptr [r11 + rax*8 + 0x24]
00000001803BA9D7 movsxd    rax, edx
00000001803BA9DA add       rax, 3
00000001803BA9DE cmp       eax, dword ptr [r10 + 0x18]
00000001803BA9E2 jae       0x1803bab77
00000001803BA9E8 movss     dword ptr [r10 + rax*8 + 0x20], xmm0
00000001803BA9EF movss     dword ptr [r10 + rax*8 + 0x24], xmm1
00000001803BA9F6 mov       r10, qword ptr [rdi + 0xa0]
00000001803BA9FD test      r10, r10
00000001803BAA00 je        0x1803bab71
00000001803BAA06 cmp       r8d, dword ptr [r10 + 0x18]
00000001803BAA0A jae       0x1803bab77
00000001803BAA10 mov       rcx, qword ptr [rdi + 0x38]
00000001803BAA14 lea       rax, [r8 + 1]
00000001803BAA18 lea       rax, [rax + rax*4]
00000001803BAA1C add       rax, rax
00000001803BAA1F mov       r10, qword ptr [r10 + rax*8]
00000001803BAA23 test      rcx, rcx
00000001803BAA26 je        0x1803bab71
00000001803BAA2C mov       rcx, qword ptr [rcx + 0x370]
00000001803BAA33 test      rcx, rcx
00000001803BAA36 je        0x1803bab71
00000001803BAA3C mov       rcx, qword ptr [rcx + 0x60]
00000001803BAA40 test      rcx, rcx
00000001803BAA43 je        0x1803bab71
00000001803BAA49 cmp       r8d, dword ptr [rcx + 0x18]
00000001803BAA4D jae       0x1803bab77
00000001803BAA53 lea       rax, [r8 + 1]
00000001803BAA57 lea       rax, [rax + rax*4]
00000001803BAA5B add       rax, rax
00000001803BAA5E mov       rcx, qword ptr [rcx + rax*8]
00000001803BAA62 test      r10, r10
00000001803BAA65 je        0x1803bab71
00000001803BAA6B cmp       edx, dword ptr [r10 + 0x18]
00000001803BAA6F jae       0x1803bab77
00000001803BAA75 movsxd    rax, r9d
00000001803BAA78 movss     xmm0, dword ptr [r10 + rax*8]
00000001803BAA7E movss     xmm1, dword ptr [r10 + rax*8 + 4]
00000001803BAA85 test      rcx, rcx
00000001803BAA88 je        0x1803bab71
00000001803BAA8E cmp       edx, dword ptr [rcx + 0x18]
00000001803BAA91 jae       0x1803bab77
00000001803BAA97 movsxd    rax, r9d
00000001803BAA9A movss     dword ptr [rcx + rax*8], xmm0
00000001803BAA9F movss     dword ptr [rcx + rax*8 + 4], xmm1
00000001803BAAA5 movsxd    rax, edx
00000001803BAAA8 inc       rax
00000001803BAAAB cmp       eax, dword ptr [r10 + 0x18]
00000001803BAAAF jae       0x1803bab77
00000001803BAAB5 movss     xmm0, dword ptr [r10 + rax*8 + 0x20]
00000001803BAABC movss     xmm1, dword ptr [r10 + rax*8 + 0x24]
00000001803BAAC3 movsxd    rax, edx
00000001803BAAC6 inc       rax
00000001803BAAC9 cmp       eax, dword ptr [rcx + 0x18]
00000001803BAACC jae       0x1803bab77
00000001803BAAD2 movss     dword ptr [rcx + rax*8 + 0x20], xmm0
00000001803BAAD8 movss     dword ptr [rcx + rax*8 + 0x24], xmm1
00000001803BAADE movsxd    rax, edx
00000001803BAAE1 add       rax, 2
00000001803BAAE5 cmp       eax, dword ptr [r10 + 0x18]
00000001803BAAE9 jae       0x1803bab77
00000001803BAAEF movss     xmm0, dword ptr [r10 + rax*8 + 0x20]
00000001803BAAF6 movss     xmm1, dword ptr [r10 + rax*8 + 0x24]
00000001803BAAFD movsxd    rax, edx
00000001803BAB00 add       rax, 2
00000001803BAB04 cmp       eax, dword ptr [rcx + 0x18]
00000001803BAB07 jae       0x1803bab77
00000001803BAB09 movss     dword ptr [rcx + rax*8 + 0x20], xmm0
00000001803BAB0F movss     dword ptr [rcx + rax*8 + 0x24], xmm1
00000001803BAB15 movsxd    rax, edx
00000001803BAB18 add       rax, 3
00000001803BAB1C cmp       eax, dword ptr [r10 + 0x18]
00000001803BAB20 jae       0x1803bab77
00000001803BAB22 movss     xmm0, dword ptr [r10 + rax*8 + 0x20]
00000001803BAB29 movss     xmm1, dword ptr [r10 + rax*8 + 0x24]
00000001803BAB30 movsxd    rax, edx
00000001803BAB33 add       rax, 3
00000001803BAB37 cmp       eax, dword ptr [rcx + 0x18]
00000001803BAB3A jae       0x1803bab77
00000001803BAB3C movss     dword ptr [rcx + rax*8 + 0x20], xmm0
00000001803BAB42 movss     dword ptr [rcx + rax*8 + 0x24], xmm1
00000001803BAB48 mov       rcx, qword ptr [rdi + 0x38]
00000001803BAB4C test      rcx, rcx
00000001803BAB4F je        0x1803bab71
00000001803BAB51 mov       rax, qword ptr [rcx]
00000001803BAB54 mov       edx, 0xff
00000001803BAB59 mov       r8, qword ptr [rax + 0x800]
00000001803BAB60 call      qword ptr [rax + 0x7f8]
00000001803BAB66 mov       rbx, qword ptr [rsp + 0x30]
00000001803BAB6B add       rsp, 0x20
00000001803BAB6F pop       rdi
00000001803BAB70 ret       
00000001803BAB71 call      0x180250150
00000001803BAB76 int3      
00000001803BAB77 call      0x180250140
00000001803BAB7C int3      
00000001803BAB7D int3      
00000001803BAB7E int3      
00000001803BAB7F int3      