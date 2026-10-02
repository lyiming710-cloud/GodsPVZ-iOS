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

alignas(16)char admin[256]={},board[128]={},component[128]={},model[64]={},list1[64]={},list2[64]={},list3[64]={},game[64]={},ui[128]={},otherAdmin[256]={},otherComponent[128]={},klass[512]={};
int kind=0,hook=0,calls=0,getCalls=0,destroyCalls=0,loadCalls=0;bool gamePresent=true,ok=true;int rcv[2],arg[2];bool returns[2];
int listid(void*p){return p==list1?1:p==list2?2:p==list3?3:-1;}
int valueid(void*p){return p==nullptr?0:p==model?1:p==component?4:p==otherComponent?5:-1;}
extern "C" bool __attribute__((ms_abi)) remove_dependency(void*p,void*v,void*){int i=calls++;if(i>1)throw std::runtime_error("calls");rcv[i]=listid(p);arg[i]=valueid(v);if(i==0&&hook==1)put<void*>(admin,kind%2?0x98:0x90,list3);if(i==0&&hook==2){put<void*>(ui,0x20,otherAdmin);put<void*>(ui,kind%2?0x30:0x28,otherComponent);}return returns[i];}
extern "C" void* __attribute__((ms_abi)) game_dependency(void*p,void*){getCalls++;ok&=p==component&&calls==2;if(hook==3){put<void*>(ui,0x20,otherAdmin);put<void*>(ui,kind%2?0x30:0x28,otherComponent);}return gamePresent?game:nullptr;}
extern "C" void __attribute__((ms_abi)) destroy_dependency(void*p,void*){destroyCalls++;ok&=p==(gamePresent?game:nullptr)&&getCalls==1&&calls==2;}
extern "C" void __attribute__((ms_abi)) load_dependency(void*p,void*v,void*){loadCalls++;ok&=p==ui&&v==nullptr&&destroyCalls==1;}
void dependency(uint64_t va,void*f){unsigned char b[12]={0x48,0xb8,0,0,0,0,0,0,0,0,0xff,0xe0};std::memcpy(b+2,&f,8);std::memcpy((void*)va,b,12);}
int main(int argc,char**argv){if(argc!=2)return 2;std::ifstream f(argv[1],std::ios::binary);std::vector<char>b((std::istreambuf_iterator<char>(f)),{});auto nt=at<uint32_t>(b,60);if(at<uint32_t>(b,nt)!=0x4550||at<uint16_t>(b,nt+24)!=0x20b)throw std::runtime_error("PE64 required");auto base=at<uint64_t>(b,nt+48);auto size=at<uint32_t>(b,nt+80);auto ns=at<uint16_t>(b,nt+6),opt=at<uint16_t>(b,nt+20);auto p=mmap((void*)base,size,PROT_READ|PROT_WRITE,MAP_PRIVATE|MAP_ANONYMOUS|MAP_FIXED_NOREPLACE,-1,0);if(p==MAP_FAILED||reinterpret_cast<uint64_t>(p)!=base)throw std::runtime_error("base");std::memcpy(p,b.data(),at<uint32_t>(b,nt+84));for(int i=0;i<ns;i++){auto s=nt+24+opt+40*i;auto rva=at<uint32_t>(b,s+12),len=at<uint32_t>(b,s+16),raw=at<uint32_t>(b,s+20);if(uint64_t(raw)+len>b.size()||uint64_t(rva)+len>size)throw std::runtime_error("bounds");std::memcpy((char*)p+rva,b.data()+raw,len);}

 for(uint64_t flag:{0x181ce4ab4ULL,0x181ce4ab7ULL})put<uint8_t>((void*)flag,0,1);
 put<void*>((void*)0x181bb0500,0,klass);put<int32_t>(klass,0xe0,1);
 dependency(0x18084e290,(void*)&remove_dependency);dependency(0x1813044d0,(void*)&game_dependency);dependency(0x18131e940,(void*)&destroy_dependency);dependency(0x180302a40,(void*)&load_dependency);dependency(0x1803041c0,(void*)&load_dependency);
 if(mprotect(p,size,PROT_READ|PROT_EXEC))throw std::runtime_error("mprotect");
 std::cerr<<"Four original caller bodies unchanged, including inline Admin delete in UI methods. Remove/get_gameObject/Destroy/LoadData are recording doubles; valid layouts, initialized metadata and Object class. Native throw/Unity/List/equality engines not executed. Hooks model comparer/engine callbacks; native records forwarding and read/call order, CLR separately executes public List.Remove.\n";
 uint64_t addresses[]={0x1802fc490,0x1802fc560,0x180302440,0x180303bf0};const char*tokens[]={"0x0600001B","0x0600001E","0x0600002B","0x06000040"};std::mt19937 rng(0x4102026);
 using Fn=void(__attribute__((ms_abi))*)(void*,void*,void*);
 for(kind=0;kind<4;kind++)for(int k=0;k<2048;k++){hook=k%4;bool modelPresent=hook?true:bool(rng()&1);gamePresent=rng()&1;returns[0]=rng()&1;returns[1]=rng()&1;calls=getCalls=destroyCalls=loadCalls=0;ok=true;rcv[0]=rcv[1]=arg[0]=arg[1]=-1;
  put<void*>(admin,0x28,board);put<void*>(board,kind%2?0x50:0x58,list1);put<void*>(admin,kind%2?0x98:0x90,list2);put<void*>(component,kind%2?0x38:0x30,modelPresent?model:nullptr);put<void*>(ui,0x20,admin);put<void*>(ui,kind%2?0x30:0x28,component);
  ((Fn)addresses[kind])(kind<2?admin:ui,kind<2?component:nullptr,nullptr);
  std::cout<<tokens[kind]<<" "<<hook<<" "<<modelPresent<<" "<<gamePresent<<" "<<returns[0]<<" "<<returns[1]<<" "<<calls<<" "<<rcv[0]<<" "<<arg[0]<<" "<<rcv[1]<<" "<<arg[1]<<" "<<getCalls<<" "<<destroyCalls<<" "<<loadCalls<<" "<<ok<<"\n";
 }
 munmap(p,size);
}
