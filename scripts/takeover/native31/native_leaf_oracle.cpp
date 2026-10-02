#include <sys/mman.h>
#include <xmmintrin.h>
#include <cstdint>
#include <cstring>
#include <fstream>
#include <iostream>
#include <vector>
#include <random>
#include <stdexcept>
template<class T> T at(const std::vector<char>& b,size_t p){T x;std::memcpy(&x,b.data()+p,sizeof(x));return x;}
uint32_t bits(float x){uint32_t b;std::memcpy(&b,&x,4);return b;}
float fl(uint32_t x){float b;std::memcpy(&b,&x,4);return b;}
using I=int32_t(__attribute__((ms_abi))*)(int32_t);
using F=float(__attribute__((ms_abi))*)(int32_t);
using GetI=int32_t(__attribute__((ms_abi))*)(void*);
using Weight=float(__attribute__((ms_abi))*)(void*,float,float);
using Attack=bool(__attribute__((ms_abi))*)(void*,void*);
using Unpack=void(__attribute__((ms_abi))*)(uint32_t,uint32_t,float*,float*,float*,float*);
int main(int argc,char**argv){
 if(argc!=2)return 2;
 std::ifstream f(argv[1],std::ios::binary);std::vector<char>b((std::istreambuf_iterator<char>(f)),{});
 auto nt=at<uint32_t>(b,0x3c);if(at<uint32_t>(b,nt)!=0x4550||at<uint16_t>(b,nt+24)!=0x20b)throw std::runtime_error("PE64 required");
 auto base=at<uint64_t>(b,nt+24+24);auto size=at<uint32_t>(b,nt+24+56);auto nsec=at<uint16_t>(b,nt+6);auto opt=at<uint16_t>(b,nt+20);
 auto p=mmap(reinterpret_cast<void*>(base),size,PROT_READ|PROT_WRITE,MAP_PRIVATE|MAP_ANONYMOUS|MAP_FIXED_NOREPLACE,-1,0);
 if(p==MAP_FAILED||reinterpret_cast<uint64_t>(p)!=base)throw std::runtime_error("preferred base unavailable");
 std::memcpy(p,b.data(),at<uint32_t>(b,nt+24+60));
 for(int i=0;i<nsec;i++){auto s=nt+24+opt+40*i;auto rva=at<uint32_t>(b,s+12),len=at<uint32_t>(b,s+16),raw=at<uint32_t>(b,s+20);if(uint64_t(raw)+len>b.size()||uint64_t(rva)+len>size)throw std::runtime_error("section bounds");std::memcpy(static_cast<char*>(p)+rva,b.data()+raw,len);}
 if(mprotect(p,size,PROT_READ|PROT_EXEC))throw std::runtime_error("mprotect failed");
 // Only the twelve independently disassembled leaf bodies below are invoked.
 // No DllMain, IL2CPP startup, imports, Unity calls, or native null-throw path.
 std::cerr<<"MXCSR="<<_mm_getcsr()<<" IMAGE="<<base<<" SIZE="<<size<<"\n";
 std::mt19937 rng(0x3102026);
 std::vector<int32_t> ints={INT32_MIN,INT32_MAX,INT32_MIN+1,INT32_MAX-1};for(int i=-256;i<=256;i++)ints.push_back(i);for(int i=0;i<512;i++)ints.push_back(static_cast<int32_t>(rng()));
 for(auto n:ints){
  std::cout<<"0x060001A3 "<<n<<" "<<reinterpret_cast<I>(0x18031a830)(n)<<"\n";
  std::cout<<"0x060001A4 "<<n<<" "<<reinterpret_cast<I>(0x18031a850)(n)<<"\n";
  for(auto spec:std::vector<std::pair<const char*,uint64_t>>{{"0x0600026C",0x1803408d0},{"0x060004B6",0x18036d390},{"0x060004B7",0x18036d400},{"0x060004B8",0x18036d470},{"0x060004B9",0x18036d4d0},{"0x060004BA",0x18036d510}})
   std::cout<<spec.first<<" "<<n<<" "<<bits(reinterpret_cast<F>(spec.second)(n))<<"\n";
  alignas(16) unsigned char obj[512]={};std::memcpy(obj+0x7c,&n,4);
  std::cout<<"0x06000375 "<<n<<" "<<reinterpret_cast<GetI>(0x180351520)(obj)<<"\n";
 }
 std::vector<uint32_t> floats={0,0x80000000,0x3f800000,0xbf800000,0x40000000,0xc0000000,0x7f800000,0xff800000,0x7fc12345,0xffc98765,1,0x80000001,0x7f7fffff,0xff7fffff};
 auto weight=reinterpret_cast<Weight>(0x180379150);
 for(auto a:floats)for(auto c:floats)std::cout<<"0x060003DF "<<a<<" "<<c<<" "<<bits(weight(nullptr,fl(a),fl(c)))<<"\n";
 for(int i=0;i<2048;i++){auto a=rng(),c=rng();std::cout<<"0x060003DF "<<a<<" "<<c<<" "<<bits(weight(nullptr,fl(a),fl(c)))<<"\n";}
 auto attack=reinterpret_cast<Attack>(0x18035de50);
 for(int type=-10;type<=40;type++)for(int state=-4;state<=8;state++){
  alignas(16) unsigned char obj[512]={};std::memcpy(obj+0x7c,&type,4);std::memcpy(obj+0x90,&state,4);
  std::cout<<"0x0600048A "<<type<<" "<<state<<" "<<int(attack(nullptr,obj))<<"\n";
 }
 auto unpack=reinterpret_cast<Unpack>(0x1803ca6b0);
 for(uint32_t v=0;v<65536;v++){
  auto a=(v<<16)|v,c=((65535-v)<<16)|((v*109+17)&65535);float r,g,bb,aa;unpack(a,c,&r,&g,&bb,&aa);
  std::cout<<"0x060008F3 "<<a<<" "<<c<<" "<<bits(r)<<" "<<bits(g)<<" "<<bits(bb)<<" "<<bits(aa)<<"\n";
 }
 munmap(p,size);
}
