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
using Armor=float(__attribute__((ms_abi))*)(int32_t);
using Wave=int32_t(__attribute__((ms_abi))*)(void*,int32_t,int32_t);
using Out=bool(__attribute__((ms_abi))*)(void*);
using Settings=bool(__attribute__((ms_abi))*)(void*,void*);
using Hash=uint32_t(__attribute__((ms_abi))*)(void*);
using Rect=bool(__attribute__((ms_abi))*)(void*,float,float,float,float);
int main(int argc,char**argv){
 if(argc!=2)return 2;
 std::ifstream f(argv[1],std::ios::binary);std::vector<char>b((std::istreambuf_iterator<char>(f)),{});
 auto nt=at<uint32_t>(b,0x3c);if(at<uint32_t>(b,nt)!=0x4550||at<uint16_t>(b,nt+24)!=0x20b)throw std::runtime_error("PE64 required");
 auto base=at<uint64_t>(b,nt+48);auto size=at<uint32_t>(b,nt+80);auto ns=at<uint16_t>(b,nt+6),opt=at<uint16_t>(b,nt+20);
 auto p=mmap((void*)base,size,PROT_READ|PROT_WRITE,MAP_PRIVATE|MAP_ANONYMOUS|MAP_FIXED_NOREPLACE,-1,0);
 if(p==MAP_FAILED||reinterpret_cast<uint64_t>(p)!=base)throw std::runtime_error("preferred base unavailable");
 std::memcpy(p,b.data(),at<uint32_t>(b,nt+84));
 for(int i=0;i<ns;i++){auto s=nt+24+opt+40*i;auto rva=at<uint32_t>(b,s+12),len=at<uint32_t>(b,s+16),raw=at<uint32_t>(b,s+20);if(uint64_t(raw)+len>b.size()||uint64_t(rva)+len>size)throw std::runtime_error("section bounds");std::memcpy((char*)p+rva,b.data()+raw,len);}
 // RectXTest precondition only: explicitly initialized Mathf class. Its body
 // reads only class+0xE0, no Epsilon/static fields. Do not call Unity startup.
 alignas(16) unsigned char klass[1024]={};put<int32_t>(klass,0xe0,1);
 put<void*>((void*)0x181bacbf8,0,klass);put<uint8_t>((void*)0x181ce4d7d,0,1);
 alignas(16) unsigned char statics[64]={};put<void*>(klass,0xb8,statics);put<void*>((void*)0x181bacca8,0,klass);put<uint8_t>((void*)0x181ce4e1b,0,1);
 if(mprotect(p,size,PROT_READ|PROT_EXEC))throw std::runtime_error("mprotect");
 std::cerr<<"MXCSR="<<_mm_getcsr()<<" Rect Mathf init flag shim; no DllMain/imports/null throw calls\n";
 std::mt19937 rng(0x3202026);
 std::vector<int32_t> ints={INT32_MIN,INT32_MAX,INT32_MIN+1,INT32_MAX-1};for(int i=-256;i<=256;i++)ints.push_back(i);for(int i=0;i<512;i++)ints.push_back((int32_t)rng());
 auto armor=(Armor)0x18036d300;
 for(auto i:ints)std::cout<<"0x060004B5 "<<i<<" "<<bits(armor(i))<<"\n";
 auto wave=(Wave)0x18031a2f0;
 auto wc=[&](int32_t flag,int32_t w,int32_t baseval){alignas(16)char obj[128]={};put<int32_t>(obj,0x18,baseval);std::cout<<"0x060001AB "<<flag<<" "<<w<<" "<<baseval<<" "<<wave(obj,flag,w)<<"\n";};
 std::vector<int32_t> edges={INT32_MIN,INT32_MAX,-1,0,1,3,9,10,19,100000};
 for(auto a:edges)for(auto c:edges)for(auto d:edges)wc(a,c,d);
 for(int i=0;i<4096;i++)wc((int32_t)rng(),(int32_t)rng(),(int32_t)rng());
 std::vector<uint32_t> fs={0,0x80000000,0x3f800000,0xbf800000,0x40000000,0xc0000000,0x7f800000,0xff800000,0x7fc12345,0xffc98765,1,0x80000001,0x7f7fffff,0xff7fffff,0x44070000};
 auto out=(Out)0x18037ca40;
 auto oc=[&](uint32_t x,uint32_t y,uint32_t sizeval){alignas(16)char projectile[512]={},board[128]={},map[128]={};put<void*>(projectile,0xe8,board);put<void*>(board,0x30,map);put<float>(projectile,0x44,fl(x));put<float>(projectile,0x48,fl(y));put<float>(map,0x1c,fl(sizeval));std::cout<<"0x060003FC "<<x<<" "<<y<<" "<<sizeval<<" "<<int(out(projectile))<<"\n";};
 for(auto a:fs)for(auto c:fs)for(auto d:fs)oc(a,c,d);
 for(int i=0;i<8192;i++)oc(rng(),rng(),rng());
 for(float v:{0.f,540.f,1080.f,-540.f}){float lx=(v/540.f)*1160.f,ly=(v/540.f)*670.f;for(float x:{-lx,lx})for(float y:{-ly,ly})oc(bits(x),bits(y),bits(v));}
 auto rect=(Rect)0x180366b50;
 auto rc=[&](uint32_t x1,uint32_t w1,uint32_t x2,uint32_t w2){std::cout<<"0x06000470 "<<x1<<" "<<w1<<" "<<x2<<" "<<w2<<" "<<int(rect(nullptr,fl(x1),fl(w1),fl(x2),fl(w2)))<<"\n";};
 for(auto a:fs)for(auto c:fs)for(auto d:fs)for(auto e:fs)rc(a,c,d,e);
 for(int i=0;i<8192;i++)rc(rng(),rng(),rng(),rng());
 auto settings=(Settings)0x1803ca2c0;
 auto sc=[&](uint32_t epsilon,uint32_t ap,uint32_t bp,int change,int value){
  put<float>(statics,0,fl(epsilon));alignas(16)char aa[32]={},bb[32]={};
  put<int32_t>(aa,0,1024);put<int32_t>(bb,0,1024);put<int32_t>(aa,4,2);put<int32_t>(bb,4,2);put<float>(aa,8,fl(ap));put<float>(bb,8,fl(bp));
  if(change==0)put<int32_t>(bb,0,value);if(change==1)put<int32_t>(bb,4,value);if(change>=2&&change<=5)put<uint8_t>(bb,change+10,value);if(change==6)put<int32_t>(bb,16,value);if(change==7)put<int32_t>(bb,20,value);
  std::cout<<"0x060008A2 "<<epsilon<<" "<<ap<<" "<<bp<<" "<<change<<" "<<value<<" "<<int(settings(aa,bb))<<"\n";
 };
 for(uint32_t epsilon:{1u,0x00800000u}){
  for(auto a:fs)for(auto c:fs)sc(epsilon,a,c,-1,0);
  for(int i=0;i<8192;i++)sc(epsilon,rng(),rng(),-1,0);
  for(int change=0;change<8;change++)for(int v:{0,1,2,1024,-1,INT32_MIN,INT32_MAX})sc(epsilon,0x3f800000,0x3f800000,change,change>=2&&change<=5?(v!=0):v);
  for(float a:{0.f,1.f,-1.f,1000000.f}){uint32_t u=bits(a);for(int delta=-12;delta<=12;delta++)sc(epsilon,u,u+delta,-1,0);}
 }
 munmap(p,size);
}
