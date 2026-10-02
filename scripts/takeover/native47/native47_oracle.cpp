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

alignas(16)char owner[256],save[256],altSave[256],label1[64],label2[64],formatSkip[32],rendered[32];
int mode,alt,trace,formatted,textId;bool addressOK,formatOK,boolOK;
extern "C" void* __attribute__((ms_abi)) render(void*r,void*f,void*){trace=3;addressOK&=r==owner+0x28;formatOK&=f==formatSkip;formatted=*(int*)r;if(mode==1){*(int*)r=alt;put<void*>(owner,0x30,label2);}if(mode==3)put<void*>(owner,0x20,altSave);return rendered;}
extern "C" void __attribute__((ms_abi)) set_text(void*r,void*t,bool v,void*){trace=trace*10+4;textId=r==label1?1:r==label2?2:0;boolOK&=v&&t==rendered;if(mode==2){put<void*>(owner,0x20,altSave);put<int>(owner,0x28,alt);}if(mode==3)put<int>(owner,0x28,alt);}
void dependency(uint64_t va,void*f){unsigned char bytes[12]={};bytes[0]=0x48;bytes[1]=0xb8;bytes[10]=0xff;bytes[11]=0xe0;std::memcpy(bytes+2,&f,8);std::memcpy((void*)va,bytes,12);if(std::memcmp((void*)va,bytes,12)!=0)throw std::runtime_error("trampoline bytes");}
int main(int argc,char**argv){if(argc!=2)return 2;std::ifstream f(argv[1],std::ios::binary);std::vector<char>b((std::istreambuf_iterator<char>(f)),{});auto nt=at<uint32_t>(b,60);if(at<uint32_t>(b,nt)!=0x4550||at<uint16_t>(b,nt+24)!=0x20b)throw std::runtime_error("PE64 required");auto base=at<uint64_t>(b,nt+48);auto size=at<uint32_t>(b,nt+80);auto ns=at<uint16_t>(b,nt+6),opt=at<uint16_t>(b,nt+20);auto p=mmap((void*)base,size,PROT_READ|PROT_WRITE,MAP_PRIVATE|MAP_ANONYMOUS|MAP_FIXED_NOREPLACE,-1,0);if(p==MAP_FAILED||reinterpret_cast<uint64_t>(p)!=base)throw std::runtime_error("base");std::memcpy(p,b.data(),at<uint32_t>(b,nt+84));for(int i=0;i<ns;i++){auto s=nt+24+opt+40*i;auto rva=at<uint32_t>(b,s+12),len=at<uint32_t>(b,s+16),raw=at<uint32_t>(b,s+20);if(uint64_t(raw)+len>b.size()||uint64_t(rva)+len>size)throw std::runtime_error("bounds");std::memcpy((char*)p+rva,b.data()+raw,len);}

 put<uint8_t>((void*)0x181ce4b03,0,1);put<void*>((void*)0x181ba2fa8,0,formatSkip);
 dependency(0x180ca3930,(void*)&render);dependency(0x1811af070,(void*)&set_text);if(mprotect(p,size,PROT_READ|PROT_EXEC))throw std::runtime_error("mprotect");
 std::cerr<<"Unchanged original SkipLevel caller. Supplied objects and verified format slot identity; formatter/text dependencies recorded with deliberate ref/receiver/Save/level mutations. Signed wrap, guard, capture and post-callback reload observed. No native formatter/UI/throw engine or iOS execution.\n";
 using Fn=void(__attribute__((ms_abi))*)(void*,int,void*);std::mt19937 rng(0x4702026);int edges[][2]={{0,0},{1,-1},{-1,1},{INT32_MAX,1},{INT32_MIN,-1},{0,1},{1,0},{-2,3},{INT32_MAX,0},{INT32_MIN,INT32_MAX}};
 for(int k=0;k<8192;k++){
  mode=k%4;alt=(int)rng();int level=k<10?edges[k][0]:(int)rng(),delta=k<10?edges[k][1]:(int)rng();bool nullSave=k>=10&&k%17==0;std::memset(owner,0x55,sizeof owner);std::memset(save,0x55,sizeof save);std::memset(altSave,0x55,sizeof altSave);put<void*>(owner,0x20,nullSave?nullptr:save);put<int>(owner,0x28,level);put<void*>(owner,0x30,label1);put<int>(save,0x20,777);put<int>(altSave,0x20,999);
  trace=formatted=textId=0;addressOK=formatOK=boolOK=true;((Fn)0x18030d780)(owner,delta,nullptr);
  void*finalSave=*(void**)(owner+0x20);std::cout<<"0x060000A5 "<<(uint32_t)level<<" "<<(uint32_t)delta<<" "<<nullSave<<" "<<mode<<" "<<(uint32_t)alt<<" "<<(uint32_t)*(int*)(owner+0x28)<<" "<<(uint32_t)*(int*)(save+0x20)<<" "<<(uint32_t)*(int*)(altSave+0x20)<<" "<<(finalSave==save?1:finalSave==altSave?2:0)<<" "<<textId<<" "<<(*(void**)(owner+0x30)==label2?2:1)<<" "<<(uint32_t)formatted<<" "<<trace<<" "<<boolOK<<" "<<formatOK<<" "<<addressOK<<"\n";
 }
 munmap(p,size);
}
