#include <sys/mman.h>
#include <xmmintrin.h>
#include <cstdint>
#include <cstring>
#include <fstream>
#include <iostream>
#include <vector>
#include <random>
#include <stdexcept>
template<class T>T at(const std::vector<char>&b,size_t p){T x;std::memcpy(&x,b.data()+p,sizeof x);return x;}
template<class T>void put(void*p,size_t off,T x){std::memcpy(static_cast<char*>(p)+off,&x,sizeof x);}

alignas(16)char owner[256],save[256],oldSave[256],info1[128],info2[128],label1[64],label2[64],formatSkip[32],formatMoney[32],rendered[32];
int token,mode,alt,trace,formatted,textId,infoId,gcCalls;bool addressOK,formatOK,boolOK,gcOK,partialOK;
void event(int x){trace=trace*10+x;}
extern "C" void __attribute__((ms_abi)) gc(void*r,void*v){gcCalls++;gcOK&=*(void**)r==v;}
extern "C" void __attribute__((ms_abi)) clear(void*r,void*){event(1);partialOK&=r==owner&&*(void**)(owner+0x20)==oldSave;if(mode==1)put<void*>(owner,0x40,info2);}
extern "C" void __attribute__((ms_abi)) set_info(void*r,void*){event(2);infoId=r==info1?1:r==info2?2:0;partialOK&=infoId!=0&&*(void**)((char*)r+0x20)==nullptr&&*(void**)((char*)r+0x28)==nullptr&&*(void**)(owner+0x20)==oldSave;if(mode==2){put<void*>(owner,0x40,info2);put<void*>(owner,0x48,label2);}}
extern "C" void* __attribute__((ms_abi)) render(void*r,void*f,void*){event(3);addressOK&=r==(token==0x060000a4?owner+0x28:save+0x70);formatOK&=f==(token==0x060000a4?formatSkip:formatMoney);partialOK&=*(void**)(owner+0x20)==save;formatted=*(int*)r;if(mode==3){*(int*)r=alt;put<void*>(owner,token==0x060000a4?0x30:0x48,label2);put<void*>(owner,0x20,oldSave);}return rendered;}
extern "C" void __attribute__((ms_abi)) set_text(void*r,void*t,bool v,void*){event(4);textId=r==label1?1:r==label2?2:0;boolOK&=v&&t==rendered;}
extern "C" void __attribute__((ms_abi)) create(void*r,void*){event(5);partialOK&=r==owner;}
void dependency(uint64_t va,void*f){unsigned char bytes[12]={};bytes[0]=0x48;bytes[1]=0xb8;bytes[10]=0xff;bytes[11]=0xe0;std::memcpy(bytes+2,&f,8);std::memcpy((void*)va,bytes,12);if(std::memcmp((void*)va,bytes,12)!=0)throw std::runtime_error("trampoline bytes");}
int main(int argc,char**argv){if(argc!=2)return 2;std::ifstream f(argv[1],std::ios::binary);std::vector<char>b((std::istreambuf_iterator<char>(f)),{});auto nt=at<uint32_t>(b,60);if(at<uint32_t>(b,nt)!=0x4550||at<uint16_t>(b,nt+24)!=0x20b)throw std::runtime_error("PE64 required");auto base=at<uint64_t>(b,nt+48);auto size=at<uint32_t>(b,nt+80);auto ns=at<uint16_t>(b,nt+6),opt=at<uint16_t>(b,nt+20);auto p=mmap((void*)base,size,PROT_READ|PROT_WRITE,MAP_PRIVATE|MAP_ANONYMOUS|MAP_FIXED_NOREPLACE,-1,0);if(p==MAP_FAILED||reinterpret_cast<uint64_t>(p)!=base)throw std::runtime_error("base");std::memcpy(p,b.data(),at<uint32_t>(b,nt+84));for(int i=0;i<ns;i++){auto s=nt+24+opt+40*i;auto rva=at<uint32_t>(b,s+12),len=at<uint32_t>(b,s+16),raw=at<uint32_t>(b,s+20);if(uint64_t(raw)+len>b.size()||uint64_t(rva)+len>size)throw std::runtime_error("bounds");std::memcpy((char*)p+rva,b.data()+raw,len);}

 put<uint8_t>((void*)0x181ce4b02,0,1);put<uint8_t>((void*)0x181ce4f25,0,1);put<uint8_t>((void*)0x181ce4f29,0,1);put<void*>((void*)0x181ba2fa8,0,formatSkip);put<void*>((void*)0x181ba0750,0,formatMoney);
 dependency(0x18024f360,(void*)&gc);dependency(0x1803ae060,(void*)&clear);dependency(0x1803ae670,(void*)&set_info);dependency(0x180ca3930,(void*)&render);dependency(0x1811af070,(void*)&set_text);dependency(0x1803ae1f0,(void*)&create);if(mprotect(p,size,PROT_READ|PROT_EXEC))throw std::runtime_error("mprotect");
 std::cerr<<"Two unchanged original formatted-int UI callers; recorded clear/info/formatter/text/create/GC dependencies, supplied objects and format-string identities. Pointer aliases, captured receivers and intermediate effects recorded. No original native formatting/UI/helper/GC/throw implementation or iOS engine execution.\n";
 using Fn=void(__attribute__((ms_abi))*)(void*,void*,void*);std::mt19937 rng(0x4602026);
 for(int t:{0x060000a4,0x060006e8})for(int k=0;k<4096;k++){
  token=t;mode=k%5;alt=(int)rng();int adv=(int)rng(),coins=(int)rng(),level=(int)rng(),challenge=k%4==0?0:(int)rng();std::memset(owner,0x55,sizeof owner);std::memset(save,0x55,sizeof save);std::memset(info1,0x55,sizeof info1);std::memset(info2,0x55,sizeof info2);
  put<void*>(owner,0x20,oldSave);put<int>(owner,0x28,level);put<int>(owner,0x38,challenge);put<void*>(owner,0x30,label1);put<void*>(owner,0x48,label1);put<void*>(owner,0x40,info1);put<int>(save,0x20,adv);put<int>(save,0x70,coins);
  trace=0;formatted=textId=infoId=gcCalls=0;addressOK=formatOK=boolOK=gcOK=partialOK=true;
  ((Fn)(t==0x060000a4?0x18030d810:0x1803ae420))(owner,save,nullptr);
  bool inf1=*(void**)(info1+0x20)==nullptr&&*(void**)(info1+0x28)==nullptr,inf2=*(void**)(info2+0x20)==nullptr&&*(void**)(info2+0x28)==nullptr;
  std::cout<<(t==0x060000a4?"0x060000A4":"0x060006E8")<<" "<<(uint32_t)adv<<" "<<(uint32_t)coins<<" "<<(uint32_t)level<<" "<<(uint32_t)challenge<<" "<<mode<<" "<<(uint32_t)alt<<" "<<(uint32_t)formatted<<" "<<(uint32_t)*(int*)(t==0x060000a4?owner+0x28:save+0x70)<<" "<<textId<<" "<<(*(void**)(owner+(t==0x060000a4?0x30:0x48))==label2?2:1)<<" "<<(*(void**)(owner+0x20)==save?1:0)<<" "<<trace<<" "<<formatOK<<" "<<addressOK<<" "<<boolOK<<" "<<infoId<<" "<<inf1<<" "<<inf2<<" "<<gcOK<<" "<<gcCalls<<" "<<partialOK<<"\n";
 }
 munmap(p,size);
}
