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

alignas(16)char camera[256],manager[256],vclass[512]={},vstatics[64],classes[5][512]={},infos[5][32]={},objects[6][128];int allocs,ctors,barriers,baseCalls;bool orderOK,receiversOK,baseStateOK;std::vector<int>kinds,ctorKinds;
int kind(void*p){for(int i=0;i<5;i++)if(p==classes[i]||p==infos[i])return i+1;return 0;}
extern "C" void* __attribute__((ms_abi)) allocation(void*c){int id=kind(c);if(allocs>=6)throw std::runtime_error("allocations");kinds.push_back(id);std::memset(objects[allocs],0,128);put<int>(objects[allocs],8,id);return objects[allocs++];}
extern "C" void __attribute__((ms_abi)) ctor_dependency(void*r,void*info){orderOK&=r==objects[ctors];ctorKinds.push_back(kind(info));ctors++;}
extern "C" void __attribute__((ms_abi)) barrier_dependency(void*dest,void*value){const int offsets[]={0x20,0x28,0x30,0x40,0x48,0x50};orderOK&=barriers<6&&dest==manager+offsets[barriers]&&value==objects[barriers]&&*(void**)dest==value;barriers++;}
extern "C" void __attribute__((ms_abi)) base_dependency(void*r,void*){baseCalls++;if(r==camera){baseStateOK&=std::memcmp(camera+0x68,vstatics,12)==0;const int offsets[]={0x38,0x3c,0x40,0x44,0x48,0x5c,0x60,0x64};const uint32_t values[]={0x41f00000,0x42c80000,0x40000000,0x41f00000,0x42aa0000,0x41c80000,0x40a00000,0x40000000};for(int i=0;i<8;i++)baseStateOK&=*(uint32_t*)(camera+offsets[i])==values[i];baseStateOK&=camera[0x58]==1;}else if(r==manager){const int offsets[]={0x20,0x28,0x30,0x40,0x48,0x50};for(int i=0;i<6;i++)baseStateOK&=*(void**)(manager+offsets[i])==objects[i];baseStateOK&=*(uint32_t*)(manager+0x3c)==0x3f800000&&allocs==6&&ctors==6&&barriers==6;}else receiversOK=false;}
void dependency(uint64_t va,void*f){unsigned char bytes[12]={};bytes[0]=0x48;bytes[1]=0xb8;bytes[10]=0xff;bytes[11]=0xe0;std::memcpy(bytes+2,&f,8);std::memcpy((void*)va,bytes,12);if(std::memcmp((void*)va,bytes,12)!=0)throw std::runtime_error("trampoline bytes");}
int main(int argc,char**argv){if(argc!=2)return 2;std::ifstream f(argv[1],std::ios::binary);std::vector<char>b((std::istreambuf_iterator<char>(f)),{});auto nt=at<uint32_t>(b,60);if(at<uint32_t>(b,nt)!=0x4550||at<uint16_t>(b,nt+24)!=0x20b)throw std::runtime_error("PE64 required");auto base=at<uint64_t>(b,nt+48);auto size=at<uint32_t>(b,nt+80);auto ns=at<uint16_t>(b,nt+6),opt=at<uint16_t>(b,nt+20);auto p=mmap((void*)base,size,PROT_READ|PROT_WRITE,MAP_PRIVATE|MAP_ANONYMOUS|MAP_FIXED_NOREPLACE,-1,0);if(p==MAP_FAILED||reinterpret_cast<uint64_t>(p)!=base)throw std::runtime_error("base");std::memcpy(p,b.data(),at<uint32_t>(b,nt+84));for(int i=0;i<ns;i++){auto s=nt+24+opt+40*i;auto rva=at<uint32_t>(b,s+12),len=at<uint32_t>(b,s+16),raw=at<uint32_t>(b,s+20);if(uint64_t(raw)+len>b.size()||uint64_t(rva)+len>size)throw std::runtime_error("bounds");std::memcpy((char*)p+rva,b.data()+raw,len);}

put<uint8_t>((void*)0x181ce4b36,0,1);put<void*>((void*)0x181bc27e8,0,vclass);put<void*>(vclass,0xb8,vstatics);put<uint8_t>((void*)0x181ce5000,0,1);
 const uint64_t classSlots[]={0x181bc6d88,0x181bc6de0,0x181bc6e38,0x181bb84a8,0x181bab630},infoSlots[]={0x181ba49b8,0x181ba4c58,0x181ba4e98,0x181b9ac10,0x181bb7d58};for(int i=0;i<5;i++){put<void*>((void*)classSlots[i],0,classes[i]);put<void*>((void*)infoSlots[i],0,infos[i]);}
 dependency(0x180250100,(void*)&allocation);dependency(0x1809d3f50,(void*)&ctor_dependency);dependency(0x1809d56d0,(void*)&ctor_dependency);dependency(0x1807abc10,(void*)&ctor_dependency);dependency(0x1805e8280,(void*)&ctor_dependency);dependency(0x18024f360,(void*)&barrier_dependency);dependency(0x1812e22a0,(void*)&base_dependency);if(mprotect(p,size,PROT_READ|PROT_EXEC))throw std::runtime_error("mprotect");
 std::cerr<<"Unchanged original CameraController/SwfManager callers. Camera zero static/class layouts supplied with arbitrary bits. Six allocation/collection constructors, GC barrier and base ctor recorded with supplied generic class/MethodInfo identity slots. No original collection/allocation/GC/Unity/base/throw engine.\n";
 using Fn=void(__attribute__((ms_abi))*)(void*,void*);std::mt19937 rng(0x4502026);
 for(int k=0;k<4096;k++){for(int i=0;i<256;i++)camera[i]=(char)rng();for(int i=0;i<3;i++)put<uint32_t>(vstatics,4*i,k?rng():0);auto before=std::vector<char>(camera,camera+256);baseCalls=0;baseStateOK=receiversOK=true;((Fn)0x1803b4560)(camera,nullptr);bool untouched=true;for(int i=0;i<256;i++)if(!((i>=0x38&&i<0x4c)||i==0x58||(i>=0x5c&&i<0x74))&&camera[i]!=before[i])untouched=false;std::cout<<"0x0600075E "<<baseCalls<<" "<<baseStateOK<<" "<<untouched<<" "<<(camera[0x58]==1);const int offsets[]={0x38,0x3c,0x40,0x44,0x48,0x5c,0x60,0x64,0x68,0x6c,0x70};for(int off:offsets)std::cout<<" "<<*(uint32_t*)(camera+off);std::cout<<"\n";}
 for(int k=0;k<4096;k++){for(int i=0;i<256;i++)manager[i]=(char)rng();auto before=std::vector<char>(manager,manager+256);allocs=ctors=barriers=baseCalls=0;orderOK=receiversOK=baseStateOK=true;kinds.clear();ctorKinds.clear();((Fn)0x1803c9f50)(manager,nullptr);bool untouched=true;for(int i=0;i<256;i++)if(!((i>=0x20&&i<0x38)||(i>=0x3c&&i<0x58))&&manager[i]!=before[i])untouched=false;std::cout<<"0x060008A0 "<<baseCalls<<" "<<allocs<<" "<<ctors<<" "<<barriers<<" "<<orderOK<<" "<<receiversOK<<" "<<baseStateOK<<" "<<untouched<<" "<<*(uint32_t*)(manager+0x3c);for(int id:kinds)std::cout<<" "<<id;for(int id:ctorKinds)std::cout<<" "<<id;std::cout<<"\n";}
 munmap(p,size);
}
