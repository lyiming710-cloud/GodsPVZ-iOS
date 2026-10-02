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
using Count=int32_t(__attribute__((ms_abi))*)(void*);
using Position=void*(__attribute__((ms_abi))*)(void*,void*,int32_t,int32_t);
int main(int argc,char**argv){
 if(argc!=2)return 2;
 std::ifstream f(argv[1],std::ios::binary);std::vector<char>b((std::istreambuf_iterator<char>(f)),{});
 auto nt=at<uint32_t>(b,0x3c);if(at<uint32_t>(b,nt)!=0x4550||at<uint16_t>(b,nt+24)!=0x20b)throw std::runtime_error("PE64 required");
 auto base=at<uint64_t>(b,nt+48);auto size=at<uint32_t>(b,nt+80);auto ns=at<uint16_t>(b,nt+6),opt=at<uint16_t>(b,nt+20);
 auto p=mmap((void*)base,size,PROT_READ|PROT_WRITE,MAP_PRIVATE|MAP_ANONYMOUS|MAP_FIXED_NOREPLACE,-1,0);
 if(p==MAP_FAILED||reinterpret_cast<uint64_t>(p)!=base)throw std::runtime_error("preferred base unavailable");
 std::memcpy(p,b.data(),at<uint32_t>(b,nt+84));
 for(int i=0;i<ns;i++){auto s=nt+24+opt+40*i;auto rva=at<uint32_t>(b,s+12),len=at<uint32_t>(b,s+16),raw=at<uint32_t>(b,s+20);if(uint64_t(raw)+len>b.size()||uint64_t(rva)+len>size)throw std::runtime_error("section bounds");std::memcpy((char*)p+rva,b.data()+raw,len);}
 // Map metadata references marked initialized, method bodies unchanged.
 put<uint8_t>((void*)0x181ce4c6c,0,1);put<uint8_t>((void*)0x181ce4c6d,0,1);
 if(mprotect(p,size,PROT_READ|PROT_EXEC))throw std::runtime_error("mprotect");
 std::cerr<<"MXCSR="<<_mm_getcsr()<<" Map metadata initialized flags; native generic List.get_Item body, valid layouts; no throw/import/startup calls\n";
 std::mt19937 rng(0x3302026);
 auto grid=(Position)0x18030ec90,center=(Position)0x18030ec30;
 auto getx=(Count)0x18032c3b0,gety=(Count)0x18032c440;
 auto run=[&](int mode,int width,int height,int32_t x,int32_t y){
  alignas(16)char config[128]={},map[128]={},row[64]={},rows[64]={},grids[64]={};
  std::vector<char> ra(0x20+8*(height+1)),ga(0x20+8*(width+1));
  put<void*>(config,0x30,map);
  if(mode!=0){put<void*>(map,0x10,rows);put<void*>(rows,0x10,ra.data());put<int32_t>(rows,0x18,height);put<uint64_t>(ra.data(),0x18,height);for(int i=0;i<height;i++)put<void*>(ra.data(),0x20+8*i,row);}
  put<void*>(row,0x18,grids);put<void*>(grids,0x10,ga.data());put<int32_t>(grids,0x18,width);put<uint64_t>(ga.data(),0x18,width);
  std::cout<<"0x06000289 "<<mode<<" "<<width<<" "<<height<<" "<<getx(map)<<"\n";
  std::cout<<"0x0600028A "<<mode<<" "<<width<<" "<<height<<" "<<gety(map)<<"\n";
  for(auto spec:std::vector<std::pair<const char*,Position>>{{"0x0600015B",grid},{"0x0600015C",center}}){float out[3]={42,43,44};spec.second(out,config,x,y);std::cout<<spec.first<<" "<<mode<<" "<<width<<" "<<height<<" "<<x<<" "<<y<<" "<<bits(out[0])<<" "<<bits(out[1])<<" "<<bits(out[2])<<"\n";}
 };
 std::vector<int32_t> edges={INT32_MIN,INT32_MIN+1,INT32_MAX,INT32_MAX-1,-9,-1,0,1,4,9,100};
 for(int mode:{0,1,2})for(int width:{0,1,2,5,9,100})for(int height:{0,1,2,5,9,100}){
  if(mode<2&&height!=0)continue;
  for(auto x:edges)for(auto y:edges)run(mode,width,height,x,y);
 }
 for(int i=0;i<8192;i++)run(2,rng()%101,rng()%101,(int32_t)rng(),(int32_t)rng());
 munmap(p,size);
}
