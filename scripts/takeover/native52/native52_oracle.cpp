#include <sys/mman.h>
#include <cstdint>
#include <cstring>
#include <fstream>
#include <iostream>
#include <vector>
#include <random>
#include <stdexcept>
#include <csetjmp>
template<class T>T at(const std::vector<char>&b,size_t p){T x;std::memcpy(&x,b.data()+p,sizeof x);return x;}
template<class T>void put(void*p,size_t off,T x){std::memcpy(static_cast<char*>(p)+off,&x,sizeof x);}
struct V3{uint32_t x,y,z;};alignas(16)char manager[160],first[32],second[32],transformA[32],transformB[32],objectClass[512],vectorClass[512],zero[16];int mode,hook,offset,errorKind;uint64_t trace;bool argsOK;V3 valueA,valueB;std::jmp_buf jump;
void event(int n){trace=(trace<<4)|n;}void failure(){event(5);errorKind=2;std::longjmp(jump,1);}extern "C" void __attribute__((ms_abi)) nre(){event(4);errorKind=1;std::longjmp(jump,1);}
extern "C" bool __attribute__((ms_abi)) equality(void*a,void*b,void*){event(1);argsOK&=a==((mode==2||mode==3)?nullptr:first)&&b==nullptr;if(mode==5)failure();if(hook==1)put<void*>(manager,offset,nullptr);if(hook==2)put<void*>(manager,offset,second);return mode==1||mode==2;}
extern "C" void* __attribute__((ms_abi)) get_transform(void*r,void*){event(2);argsOK&=r==(hook==2?second:first);if(mode==6)failure();return mode==4?nullptr:r==second?transformB:transformA;}
extern "C" V3* __attribute__((ms_abi)) get_position(V3*out,void*r,void*){event(3);argsOK&=r==(hook==2?transformB:transformA);if(mode==7)failure();*out=r==transformB?valueB:valueA;return out;}
void dependency(uint64_t va,void*f){unsigned char bytes[12]={};bytes[0]=0x48;bytes[1]=0xb8;bytes[10]=0xff;bytes[11]=0xe0;std::memcpy(bytes+2,&f,8);std::memcpy((void*)va,bytes,12);if(std::memcmp((void*)va,bytes,12))throw std::runtime_error("trampoline");}
int main(int argc,char**argv){if(argc!=2)return 2;std::ifstream f(argv[1],std::ios::binary);std::vector<char>b((std::istreambuf_iterator<char>(f)),{});auto nt=at<uint32_t>(b,60);if(at<uint32_t>(b,nt)!=0x4550||at<uint16_t>(b,nt+24)!=0x20b)throw std::runtime_error("PE64 required");auto base=at<uint64_t>(b,nt+48);auto size=at<uint32_t>(b,nt+80);auto ns=at<uint16_t>(b,nt+6),opt=at<uint16_t>(b,nt+20);auto p=mmap((void*)base,size,PROT_READ|PROT_WRITE,MAP_PRIVATE|MAP_ANONYMOUS|MAP_FIXED_NOREPLACE,-1,0);if(p==MAP_FAILED||reinterpret_cast<uint64_t>(p)!=base)throw std::runtime_error("base");std::memcpy(p,b.data(),at<uint32_t>(b,nt+84));for(int i=0;i<ns;i++){auto s=nt+24+opt+40*i;auto rva=at<uint32_t>(b,s+12),len=at<uint32_t>(b,s+16),raw=at<uint32_t>(b,s+20);if(uint64_t(raw)+len>b.size()||uint64_t(rva)+len>size)throw std::runtime_error("bounds");std::memcpy((char*)p+rva,b.data()+raw,len);}



std::vector<std::pair<uint64_t,std::vector<char>>> originals;originals.push_back({0x180300eb0,std::vector<char>((char*)0x180300eb0, (char*)0x180300f83)});originals.push_back({0x180323680,std::vector<char>((char*)0x180323680, (char*)0x180323753)});originals.push_back({0x180323760,std::vector<char>((char*)0x180323760, (char*)0x180323833)});
 for(auto va:{0x181ce4b2cULL,0x181ce4bf7ULL,0x181ce4bf8ULL,0x181ce4b36ULL})put<uint8_t>((void*)va,0,1);put<int>(objectClass,0xe0,1);put<void*>((void*)0x181bb0500,0,objectClass);put<void*>(vectorClass,0xb8,zero);put<void*>((void*)0x181bc27e8,0,vectorClass);
 dependency(0x18131f760,(void*)&equality);dependency(0x181304510,(void*)&get_transform);dependency(0x18131c070,(void*)&get_transform);dependency(0x18132f3a0,(void*)&get_position);dependency(0x180250150,(void*)&nre);
 for(const auto&r:originals)if(std::memcmp((void*)r.first,r.second.data(),r.second.size()))throw std::runtime_error("Caller changed");if(mprotect(p,size,PROT_READ|PROT_EXEC))throw std::runtime_error("mprotect");std::cerr<<"Three original bank-position callers unchanged. Initialized Object/Vector3 identities and default zero static fields; recording Unity equality/transform/position/exception dependencies. Original field reread, branching and raw Vector3 return copies retained. No original Unity equality/engine/class-init/throw implementation or iOS.\n";
 using Fn=V3*(__attribute__((ms_abi))*)(V3*,void*,void*);uint64_t starts[]={0x180300eb0,0x180323680,0x180323760};int tokens[]={0x060000E9,0x06000204,0x06000205},offs[]={0x48,0x40,0x58};std::mt19937 rng(0x522026);uint32_t edge[]={0,0x80000000,0x3f800000,0xbf800000,0x7f800000,0xff800000,0x7fc12345,0x7f812345,0x00000001,0x80000001,0x7f7fffff,0xffc54321};
 for(int t=0;t<3;t++)for(int k=0;k<4096;k++){mode=k%8;hook=(k/8)%3;offset=offs[t];valueA={k<12?edge[k]:(uint32_t)rng(),k<12?edge[11-k]:(uint32_t)rng(),(uint32_t)rng()};valueB={(uint32_t)rng(),(uint32_t)rng(),(uint32_t)rng()};put<void*>(manager,offset,(mode==2||mode==3)?nullptr:first);trace=0;errorKind=0;argsOK=true;V3 out{0xdeadbeef,0xdeadbeef,0xdeadbeef};void*ret=nullptr;if(setjmp(jump)==0)ret=((Fn)starts[t])(&out,manager,nullptr);
 std::cout<<"0x"<<std::hex<<tokens[t]<<std::dec<<" "<<mode<<" "<<hook<<" "<<valueA.x<<" "<<valueA.y<<" "<<valueA.z<<" "<<valueB.x<<" "<<valueB.y<<" "<<valueB.z<<" "<<trace<<" "<<errorKind<<" "<<out.x<<" "<<out.y<<" "<<out.z<<" "<<argsOK<<" "<<(errorKind?ret==nullptr:ret==&out)<<"\n";
 }munmap(p,size);
}
