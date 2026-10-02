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

alignas(16)char stringClass[512]={},statics[64]={},empty[32]={},clip[256]={};int barriers=0,baseCalls=0;bool receiverOK=true,initializedBeforeBase=true;
extern "C" void __attribute__((ms_abi)) barrier_dependency(void*dest,void*value){barriers++;receiverOK&=dest==clip+0x68||dest==clip+0x90;receiverOK&=value==empty;}
extern "C" void __attribute__((ms_abi)) base_dependency(void*receiver,void*){baseCalls++;receiverOK&=receiver==clip;initializedBeforeBase&=clip[0x38]==1&&*(void**)(clip+0x68)==empty&&*(void**)(clip+0x90)==empty;for(int i=0;i<4;i++)initializedBeforeBase&=*(uint32_t*)(clip+0x74+4*i)==0x3f800000;}
void dependency(uint64_t va,void*f){unsigned char bytes[12]={};bytes[0]=0x48;bytes[1]=0xb8;bytes[10]=0xff;bytes[11]=0xe0;std::memcpy(bytes+2,&f,8);std::memcpy((void*)va,bytes,12);if(std::memcmp((void*)va,bytes,12)!=0)throw std::runtime_error("trampoline bytes");}
int main(int argc,char**argv){if(argc!=2)return 2;std::ifstream f(argv[1],std::ios::binary);std::vector<char>b((std::istreambuf_iterator<char>(f)),{});auto nt=at<uint32_t>(b,60);if(at<uint32_t>(b,nt)!=0x4550||at<uint16_t>(b,nt+24)!=0x20b)throw std::runtime_error("PE64 required");auto base=at<uint64_t>(b,nt+48);auto size=at<uint32_t>(b,nt+80);auto ns=at<uint16_t>(b,nt+6),opt=at<uint16_t>(b,nt+20);auto p=mmap((void*)base,size,PROT_READ|PROT_WRITE,MAP_PRIVATE|MAP_ANONYMOUS|MAP_FIXED_NOREPLACE,-1,0);if(p==MAP_FAILED||reinterpret_cast<uint64_t>(p)!=base)throw std::runtime_error("base");std::memcpy(p,b.data(),at<uint32_t>(b,nt+84));for(int i=0;i<ns;i++){auto s=nt+24+opt+40*i;auto rva=at<uint32_t>(b,s+12),len=at<uint32_t>(b,s+16),raw=at<uint32_t>(b,s+20);if(uint64_t(raw)+len>b.size()||uint64_t(rva)+len>size)throw std::runtime_error("bounds");std::memcpy((char*)p+rva,b.data()+raw,len);}

 put<uint8_t>((void*)0x181ce4fd1,0,1);put<void*>((void*)0x181bbacf8,0,stringClass);put<void*>(stringClass,0xb8,statics);put<void*>(statics,0,empty);dependency(0x18024f360,(void*)&barrier_dependency);dependency(0x1812e22a0,(void*)&base_dependency);if(mprotect(p,size,PROT_READ|PROT_EXEC))throw std::runtime_error("mprotect");
 std::cerr<<"Unchanged original SwfClip constructor body and inline Color constant stores. String.Empty static class layout supplied; GC barrier/base MonoBehaviour ctor recording doubles. No original GC/Unity/startup engine. CLR executes actual candidate and pure Color value constructor from pinned real CoreModule with public base ctor double.\n";
 using Fn=void(__attribute__((ms_abi))*)(void*,void*);std::mt19937 rng(0x4302026);
 for(int k=0;k<4096;k++){for(int i=0;i<256;i++)clip[i]=(char)rng();auto before=std::vector<char>(clip,clip+256);barriers=baseCalls=0;receiverOK=initializedBeforeBase=true;((Fn)0x1803c8530)(clip,nullptr);bool untouched=true;for(int i=0;i<256;i++){bool changed=i==0x38||(i>=0x68&&i<0x70)||(i>=0x74&&i<0x84)||(i>=0x90&&i<0x98);if(!changed&&before[i]!=clip[i])untouched=false;}
  std::cout<<"0x0600084F "<<barriers<<" "<<baseCalls<<" "<<receiverOK<<" "<<initializedBeforeBase<<" "<<untouched<<" "<<(clip[0x38]==1)<<" "<<(*(void**)(clip+0x68)==empty)<<" "<<(*(void**)(clip+0x90)==empty);for(int i=0;i<4;i++)std::cout<<" "<<*(uint32_t*)(clip+0x74+4*i);std::cout<<"\n";
 }munmap(p,size);
}
