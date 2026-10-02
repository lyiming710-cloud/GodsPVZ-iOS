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
using Collect=void(__attribute__((ms_abi))*)(void*,void*);
using Rate=void(__attribute__((ms_abi))*)(void*,void*,float,void*);
using Find=void*(__attribute__((ms_abi))*)(void*,int32_t,int32_t,int32_t);
int calls=0;void* seenReceiver=nullptr;void*seenValue=nullptr;uint32_t seenBits=0;bool removeResult=false;int32_t seenSun=0;void*manager=nullptr;
extern "C" bool __attribute__((ms_abi)) remove_dependency(void*p,void*v,void*){calls++;seenReceiver=p;seenValue=v;seenSun=*(int32_t*)((char*)manager+0x20);return removeResult;}
extern "C" void __attribute__((ms_abi)) setter_dependency(void*p,void*v,float rate,void*){calls++;seenReceiver=p;seenValue=v;std::memcpy(&seenBits,&rate,4);}
void dependency(uint64_t va,void*f){unsigned char b[12]={0x48,0xb8,0,0,0,0,0,0,0,0,0xff,0xe0};std::memcpy(b+2,&f,8);std::memcpy((void*)va,b,12);}
int main(int argc,char**argv){if(argc!=2)return 2;std::ifstream f(argv[1],std::ios::binary);std::vector<char>b((std::istreambuf_iterator<char>(f)),{});auto nt=at<uint32_t>(b,60);if(at<uint32_t>(b,nt)!=0x4550||at<uint16_t>(b,nt+24)!=0x20b)throw std::runtime_error("PE64 required");auto base=at<uint64_t>(b,nt+48);auto size=at<uint32_t>(b,nt+80);auto ns=at<uint16_t>(b,nt+6),opt=at<uint16_t>(b,nt+20);auto p=mmap((void*)base,size,PROT_READ|PROT_WRITE,MAP_PRIVATE|MAP_ANONYMOUS|MAP_FIXED_NOREPLACE,-1,0);if(p==MAP_FAILED||reinterpret_cast<uint64_t>(p)!=base)throw std::runtime_error("base");std::memcpy(p,b.data(),at<uint32_t>(b,nt+84));for(int i=0;i<ns;i++){auto s=nt+24+opt+40*i;auto rva=at<uint32_t>(b,s+12),len=at<uint32_t>(b,s+16),raw=at<uint32_t>(b,s+20);if(uint64_t(raw)+len>b.size()||uint64_t(rva)+len>size)throw std::runtime_error("bounds");std::memcpy((char*)p+rva,b.data()+raw,len);}
 for(uint64_t flag:{0x181ce4c52ULL,0x181ce4ff3ULL,0x181ce4c7aULL})put<uint8_t>((void*)flag,0,1);
 dependency(0x18084e290,(void*)&remove_dependency);dependency(0x1805fbe00,(void*)&setter_dependency);
 if(mprotect(p,size,PROT_READ|PROT_EXEC))throw std::runtime_error("mprotect");_mm_setcsr(8064);
 std::cerr<<"Original three caller bodies unchanged. Find additionally executes original List indexer unchanged on valid native paths; String.IsNullOrEmpty unchanged. Collect Remove and Rate dictionary setter are controlled recording dependencies; no proof of their original engines. Metadata flags/MethodInfo supplied, valid object layout and MXCSR8064. Native throw/Unity startup not executed.\n";
 std::mt19937 rng(0x3802026);alignas(16)char sun[128]={},list[64]={},project[64]={};manager=sun;put<void*>(sun,0x38,list);
 for(int k=0;k<4096;k++){int32_t initial=(int32_t)rng(),value=(int32_t)rng();removeResult=rng()&1;put<int32_t>(sun,0x20,initial);put<int32_t>(project,0x28,value);calls=0;((Collect)0x180340860)(sun,project);std::cout<<"0x06000264 "<<initial<<" "<<value<<" "<<removeResult<<" "<<calls<<" "<<seenSun<<" "<<*(int32_t*)(sun+0x20)<<" "<<(seenReceiver==list)<<" "<<(seenValue==project)<<"\n";}
 std::vector<uint32_t>edges={0,0x80000000,1,0x80000001,0x7f7fffff,0xff7fffff,0x7f800000,0xff800000,0x7fc00001,0x7fa00001,0xffc01234,0x3f800000,0xbf800000};alignas(16)char host[128]={},dict[64]={},empty[32]={},key[32]={};put<int32_t>(empty,0x10,0);put<int32_t>(key,0x10,1);put<uint16_t>(key,0x14,'x');put<void*>(host,0x50,dict);
 for(int k=0;k<8192;k++){uint32_t bits=k<(int)edges.size()*3?edges[k/3]:rng();float rate;std::memcpy(&rate,&bits,4);int kind=k<(int)edges.size()*3?k%3:rng()%3;void*value=kind==0?nullptr:kind==1?empty:key;calls=0;seenBits=0;seenReceiver=nullptr;seenValue=nullptr;((Rate)0x1803c9e10)(kind==2?host:nullptr,value,rate,nullptr);std::cout<<"0x06000890 "<<kind<<" "<<bits<<" "<<calls<<" "<<seenBits<<" "<<(calls==0||seenReceiver==dict)<<" "<<(calls==0||seenValue==key)<<"\n";}
 alignas(16)char nodes[5][32]={};void*values[]={nullptr,nodes[1],nodes[2],nodes[3],nodes[4]};auto id=[&](void*v){for(int i=0;i<5;i++)if(values[i]==v)return i;throw std::runtime_error("identity");};alignas(16)char nodeList[64]={};std::vector<char>arr(0x20+8*33);put<uint64_t>(arr.data(),0x18,33);put<void*>(nodeList,0x10,arr.data());int accepted=0;
 while(accepted<16384){int count=rng()%33;int32_t a,b,c;if(accepted<4){a=0x40000001;b=accepted;c=4;}else if(rng()&1){a=(int32_t)rng();b=(int32_t)rng();c=(int32_t)rng();}else{a=1+rng()%17;b=(int)(rng()%33)-2;c=(int)(rng()%13)-4;}int32_t index=(int32_t)(uint32_t(a)*uint32_t(c)+uint32_t(b));bool early=b>=a;if(!early&&index<0)continue;bool present=!early||bool(rng()&1);put<int32_t>(nodeList,0x18,count);std::vector<int>items;for(int i=0;i<count;i++){int v=rng()%5;items.push_back(v);put<void*>(arr.data(),0x20+8*i,values[v]);}void*out=((Find)0x18032d950)(present?nodeList:nullptr,a,b,c);std::cout<<"0x060002A0 "<<present<<" "<<count<<" "<<a<<" "<<b<<" "<<c;for(int v:items)std::cout<<" "<<v;std::cout<<" "<<id(out)<<"\n";accepted++;}
 munmap(p,size);
}
