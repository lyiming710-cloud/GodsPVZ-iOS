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
uint32_t bits(float f){uint32_t x;std::memcpy(&x,&f,4);return x;}
float fl(uint32_t x){float f;std::memcpy(&f,&x,4);return f;}
using Level=bool(__attribute__((ms_abi))*)(void*,int32_t,int32_t);
using Record=bool(__attribute__((ms_abi))*)(void*,int32_t);
using Talent=int32_t(__attribute__((ms_abi))*)(void*);
using Flush=void(__attribute__((ms_abi))*)(void*,int32_t);
int main(int argc,char**argv){
 if(argc!=2)return 2;
 std::ifstream f(argv[1],std::ios::binary);std::vector<char>b((std::istreambuf_iterator<char>(f)),{});
 auto nt=at<uint32_t>(b,0x3c);if(at<uint32_t>(b,nt)!=0x4550||at<uint16_t>(b,nt+24)!=0x20b)throw std::runtime_error("PE64 required");
 auto base=at<uint64_t>(b,nt+48);auto size=at<uint32_t>(b,nt+80);auto ns=at<uint16_t>(b,nt+6),opt=at<uint16_t>(b,nt+20);
 auto p=mmap((void*)base,size,PROT_READ|PROT_WRITE,MAP_PRIVATE|MAP_ANONYMOUS|MAP_FIXED_NOREPLACE,-1,0);
 if(p==MAP_FAILED||reinterpret_cast<uint64_t>(p)!=base)throw std::runtime_error("preferred base unavailable");
 std::memcpy(p,b.data(),at<uint32_t>(b,nt+84));
 for(int i=0;i<ns;i++){auto s=nt+24+opt+40*i;auto rva=at<uint32_t>(b,s+12),len=at<uint32_t>(b,s+16),raw=at<uint32_t>(b,s+20);if(uint64_t(raw)+len>b.size()||uint64_t(rva)+len>size)throw std::runtime_error("section bounds");std::memcpy((char*)p+rva,b.data()+raw,len);}
 // Metadata initialized flags; String.Empty precondition. Native Record
 // oracle covers existing arrays only, never calls allocator/write barrier.
 alignas(16)char empty[32]={},nonempty[32]={},klass[1024]={},statics[64]={};
 put<int32_t>(nonempty,0x10,1);put<uint16_t>(nonempty,0x14,65);
 put<void*>(statics,0,empty);put<void*>(klass,0xb8,statics);put<void*>((void*)0x181bbacf8,0,klass);
 put<uint8_t>((void*)0x181ce4dee,0,1);put<uint8_t>((void*)0x181ce4de9,0,1);
 if(mprotect(p,size,PROT_READ|PROT_EXEC))throw std::runtime_error("mprotect");
 std::cerr<<"MXCSR="<<_mm_getcsr()<<" initialized metadata and String.Empty pointer; Record existing arrays only; no allocator/import/throw/startup calls\n";
 std::mt19937 rng(0x3402026);
 std::vector<int32_t> ints={INT32_MIN,INT32_MAX,INT32_MIN+1,INT32_MAX-1};for(int i=-64;i<=64;i++)ints.push_back(i);for(int i=0;i<2048;i++)ints.push_back((int32_t)rng());
 auto level=(Level)0x180380280;
 for(auto type:std::vector<int32_t>{INT32_MIN,-1,0,1,2,3,INT32_MAX})for(auto a:ints){
  alignas(16)char save[256]={},arr[128]={};put<int32_t>(save,0x20,a);put<uint64_t>(arr,0x18,8);put<void*>(save,0x38,arr);put<void*>(save,0x50,arr);
  if(type==1){for(int i=0;i<8;i++)put<int32_t>(arr,0x20+4*i,(int32_t)rng());}else{for(int i=0;i<8;i++)put<uint8_t>(arr,0x20+i,rng()&1);}
  std::vector<int32_t> indexes=type==1||type==2?std::vector<int32_t>{0,1,2,3,4,5,6,7}:std::vector<int32_t>{a,0,-1,1,INT32_MIN,INT32_MAX};
  for(auto index:indexes){std::cout<<"0x060004C5 "<<type<<" "<<a<<" "<<index;
   for(int i=0;i<8;i++)std::cout<<" "<<(type==1?at<int32_t>(std::vector<char>(arr,arr+128),0x20+i*4):int(*(uint8_t*)(arr+0x20+i)));
   std::cout<<" "<<int(level(save,type,index))<<"\n";
  }
 }
 auto record=(Record)0x1803802f0;
 for(int i=0;i<4096;i++){
  alignas(16)char save[256]={},arr[256]={};put<uint64_t>(arr,0x18,128);put<void*>(save,0x88,arr);int index=rng()%128;bool old=rng()&1;
  for(int k=0;k<128;k++)put<uint8_t>(arr,0x20+k,old);bool first=record(save,index),second=record(save,index);
  bool ok=true;for(int k=0;k<128;k++)if(*(uint8_t*)(arr+0x20+k)!=(k==index?1:old))ok=false;if(!ok)throw std::runtime_error("unexpected array write");
  std::cout<<"0x060004C6 "<<index<<" "<<old<<" "<<first<<" "<<second<<" "<<int(*(uint8_t*)(arr+0x20+index))<<"\n";
 }
 auto talent=(Talent)0x180376140;
 for(int k=0;k<8192;k++){
  int len=rng()%65;alignas(16)char plant[128]={};std::vector<char>arr(0x20+8*len);put<uint64_t>(arr.data(),0x18,len);put<void*>(plant,0x30,arr.data());std::vector<int>labels;
  for(int i=0;i<len;i++){int label=rng()%3;labels.push_back(label);put<void*>(arr.data(),0x20+8*i,label==0?nullptr:label==1?empty:nonempty);}
  std::cout<<"0x060004D1 "<<len;for(auto label:labels)std::cout<<" "<<label;std::cout<<" "<<talent(plant)<<"\n";
 }
 auto flush=(Flush)0x180318090;
 std::vector<uint32_t> fs={0,0x80000000,0x3f800000,0xbf800000,0x40000000,0xc0000000,0x7f800000,0xff800000,0x7fc12345,0xffc98765,1,0x80000001,0x7f7fffff,0xff7fffff};
 auto fc=[&](int mode,int wave,bool finish,uint32_t next,uint32_t test,uint32_t sh,uint32_t lo,uint32_t nt){
  alignas(16)char enemy[256]={};put<int32_t>(enemy,0x54,wave);put<float>(enemy,0x60,fl(next));put<float>(enemy,0x64,fl(nt));put<uint8_t>(enemy,0x6c,finish);put<float>(enemy,0x74,fl(test));put<float>(enemy,0x78,fl(sh));put<float>(enemy,0x7c,fl(lo));flush(enemy,mode);
  std::cout<<"0x060001BA "<<mode<<" "<<wave<<" "<<finish<<" "<<next<<" "<<test<<" "<<sh<<" "<<lo<<" "<<nt<<" "<<bits(*(float*)(enemy+0x60))<<" "<<bits(*(float*)(enemy+0x64))<<"\n";
  if(*(int32_t*)(enemy+0x54)!=wave||*(uint8_t*)(enemy+0x6c)!=finish||bits(*(float*)(enemy+0x74))!=test||bits(*(float*)(enemy+0x78))!=sh||bits(*(float*)(enemy+0x7c))!=lo)throw std::runtime_error("unexpected state write");
 };
 for(int mode:{INT32_MIN,-1,0,1,2,3,INT32_MAX})for(auto a:fs)for(auto c:fs)for(auto d:fs)for(int finish:{0,1})fc(mode,19,finish,a,c,d,d,a);
 for(int i=0;i<8192;i++)fc(rng()%5,(int32_t)rng(),rng()&1,rng(),rng(),rng(),rng(),rng());
 munmap(p,size);
}
