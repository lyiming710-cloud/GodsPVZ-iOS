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
 auto unpack=reinterpret_cast<Unpack>(0x1803ca6b0);
 int aliases[5][4]={{0,0,0,0},{0,0,1,1},{0,1,0,1},{0,1,1,0},{0,0,0,1}};
 uint32_t inputs[7][2]={{0,0},{0x80007fff,0xffff0001},{0x7fff8000,0x0001ffff},{0x12345678,0x9abcdef0},{0xffffffff,0x80008000},{0xffff0000,0x0000ffff},{0x5555aaaa,0xaaaa5555}};
 for(int mode=0;mode<5;mode++)for(auto &pair:inputs){float cells[4]={11,12,13,14};unpack(pair[0],pair[1],&cells[aliases[mode][0]],&cells[aliases[mode][1]],&cells[aliases[mode][2]],&cells[aliases[mode][3]]);std::cout<<mode<<" "<<pair[0]<<" "<<pair[1];for(auto c:cells)std::cout<<" "<<bits(c);std::cout<<"\n";}munmap(p,size);
 }
