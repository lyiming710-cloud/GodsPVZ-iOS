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
struct V3{float x,y,z;};
alignas(16)char prop[256],game[64],otherGame[64],transform[64],cardA[512],cardB[512],method[32];
int token,mode,hook,errorKind,gets,sets,components;uint32_t bx,by,bz;uint64_t trace;bool argsOK;void*selected;std::jmp_buf jump;
void event(int n){trace=(trace<<4)|n;}
extern "C" void __attribute__((ms_abi)) nre(){event(4);errorKind=1;std::longjmp(jump,1);}
extern "C" void* __attribute__((ms_abi)) get_transform(void*r,void*){event(1);gets++;argsOK&=r==game;if(hook==1){put<uint32_t>(prop,0x70,0x44fa0000);put<uint32_t>(prop,0x74,0x457a0000);put<int>(prop,0x2c,0);put<void*>(prop,0x90,otherGame);}return mode==2?nullptr:transform;}
extern "C" void __attribute__((ms_abi)) set_position(void*r,V3*v,void*){event(2);sets++;argsOK&=r==transform;std::memcpy(&bx,&v->x,4);std::memcpy(&by,&v->y,4);std::memcpy(&bz,&v->z,4);if(hook==2)selected=cardB;}
extern "C" void* __attribute__((ms_abi)) get_component(void*r,void*m){event(3);components++;argsOK&=r==game&&m==method;if(hook==3)selected=cardB;return mode==3?nullptr:selected;}
void dependency(uint64_t va,void*f){unsigned char bytes[12]={};bytes[0]=0x48;bytes[1]=0xb8;bytes[10]=0xff;bytes[11]=0xe0;std::memcpy(bytes+2,&f,8);std::memcpy((void*)va,bytes,12);if(std::memcmp((void*)va,bytes,12)!=0)throw std::runtime_error("trampoline bytes");}
int main(int argc,char**argv){if(argc!=2)return 2;std::ifstream f(argv[1],std::ios::binary);std::vector<char>b((std::istreambuf_iterator<char>(f)),{});auto nt=at<uint32_t>(b,60);if(at<uint32_t>(b,nt)!=0x4550||at<uint16_t>(b,nt+24)!=0x20b)throw std::runtime_error("PE64 required");auto base=at<uint64_t>(b,nt+48);auto size=at<uint32_t>(b,nt+80);auto ns=at<uint16_t>(b,nt+6),opt=at<uint16_t>(b,nt+20);auto p=mmap((void*)base,size,PROT_READ|PROT_WRITE,MAP_PRIVATE|MAP_ANONYMOUS|MAP_FIXED_NOREPLACE,-1,0);if(p==MAP_FAILED||reinterpret_cast<uint64_t>(p)!=base)throw std::runtime_error("base");std::memcpy(p,b.data(),at<uint32_t>(b,nt+84));for(int i=0;i<ns;i++){auto s=nt+24+opt+40*i;auto rva=at<uint32_t>(b,s+12),len=at<uint32_t>(b,s+16),raw=at<uint32_t>(b,s+20);if(uint64_t(raw)+len>b.size()||uint64_t(rva)+len>size)throw std::runtime_error("bounds");std::memcpy((char*)p+rva,b.data()+raw,len);}



 put<uint8_t>((void*)0x181ce4f0f,0,1);put<void*>((void*)0x181ba4978,0,method);
 dependency(0x18131c070,(void*)&get_transform);dependency(0x18132fbb0,(void*)&set_position);dependency(0x18132f910,(void*)&set_position);dependency(0x18046dc80,(void*)&get_component);dependency(0x180250150,(void*)&nre);
 if(mprotect(p,size,PROT_READ|PROT_EXEC))throw std::runtime_error("mprotect");std::cerr<<"Two unchanged original coordinate callers; supplied GameObject/Transform/MethodInfo/Card layouts, ready Seed metadata. Recorded Transform getter/setter/GetComponent/exception dependencies. Native SSE coordinate arithmetic and signed int32 divide/remainder/wrap retained; getter mutations stress precomputed coordinates. No original Unity or throw engine/iOS.\n";
 using PropFn=void(__attribute__((ms_abi))*)(void*,void*);using SeedFn=void(__attribute__((ms_abi))*)(void*,void*,int,void*);std::mt19937 rng(0x502026);uint32_t edges[]={0,0x80000000,0x3f800000,0xbf800000,0x7f800000,0xff800000,0x7fc12345,0x7f812345,0xffc12345,0x00000001,0x80000001,0x7f7fffff};int indexes[]={INT32_MIN,INT32_MIN+1,-10000000,-4,-3,-2,-1,0,1,2,3,4,10000000,INT32_MAX-1,INT32_MAX};
 for(int t:{0x06000404,0x060006BE})for(int k=0;k<4096;k++){token=t;mode=k%4;hook=(k/4)%4;uint32_t x=k<12?edges[k]:rng(),y=k<12?edges[11-k]:rng();int type=k%7,index=k<15?indexes[k]:(int)rng();std::memset(prop,0x55,sizeof prop);put<uint32_t>(prop,0x70,x);put<uint32_t>(prop,0x74,y);put<int>(prop,0x2c,type);put<void*>(prop,0x90,mode==1?nullptr:game);put<int>(cardA,0x100,7);put<int>(cardB,0x100,8);selected=cardA;trace=0;errorKind=gets=sets=components=0;bx=by=bz=0xdeadbeef;argsOK=true;
  if(setjmp(jump)==0){if(t==0x06000404)((PropFn)0x18037e600)(prop,nullptr);else((SeedFn)0x1803aba00)(nullptr,mode==1?nullptr:game,index,nullptr);}
  int pa,pb;std::memcpy(&pa,cardA+0x100,4);std::memcpy(&pb,cardB+0x100,4);std::cout<<(t==0x06000404?"0x06000404":"0x060006BE")<<" "<<x<<" "<<y<<" "<<type<<" "<<(uint32_t)index<<" "<<mode<<" "<<hook<<" "<<trace<<" "<<errorKind<<" "<<gets<<" "<<sets<<" "<<components<<" "<<bx<<" "<<by<<" "<<bz<<" "<<pa<<" "<<pb<<" "<<argsOK<<"\n";
 }munmap(p,size);
}
