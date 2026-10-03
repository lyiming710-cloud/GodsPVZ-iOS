#include <sys/mman.h>
#include <cstdint>
#include <cstring>
#include <fstream>
#include <iostream>
#include <vector>
#include <random>
#include <stdexcept>
#include <csetjmp>
template<class T>T at(const std::vector<char>&b,size_t p){T x;std::memcpy(&x,b.data()+p,sizeof x);return x;}template<class T>void put(void*p,size_t off,T x){std::memcpy(static_cast<char*>(p)+off,&x,sizeof x);}
struct B6{uint32_t v[6];};
alignas(16)char self[1024],text[64],textB[64],textClass[2048],formatted[64],formatName[64],group[64],groupB[64],game[64],frame[64],mesh[64],renderer[64],rendererB[64],readyClass[512],layerName[64];
int token,mode,hook,err,order,setCalls,updateCalls;uint64_t trace;bool argsOK;uint32_t input,formattedValue;B6 boundsValue,alternate,output;
std::jmp_buf jump;
template<class T>T get(void*p,size_t off){T x;std::memcpy(&x,(char*)p+off,sizeof x);return x;}
int id(void*p){void*all[]={nullptr,self,text,textB,formatted,group,groupB,game,frame,mesh,renderer,rendererB};for(int i=0;i<12;i++)if(p==all[i])return i;throw std::runtime_error("unknown identity");}void event(int e,void*r=nullptr){trace=trace*131+e*32+id(r);}extern "C" void __attribute__((ms_abi)) nre(){event(30);err=1;std::longjmp(jump,1);}void failure(){event(32);err=3;std::longjmp(jump,1);}
extern "C" void* __attribute__((ms_abi)) format(void*p,void*s,void*){event(1);argsOK&=p==self+0x28&&s==formatName;formattedValue=get<uint32_t>(p,0);if(hook==1)put<void*>(self,0x20,textB);if(hook==2)put<void*>(self,0x20,nullptr);if(hook==3)put<uint32_t>(self,0x28,0x87654321);if(mode==3)failure();return mode==4?nullptr:formatted;}
extern "C" void __attribute__((ms_abi)) setText(void*r,void*s,void*){event(2,r);argsOK&=s==(mode==4?nullptr:formatted);setCalls++;if(hook==4)put<uint32_t>(self,0x28,0x12345678);if(mode==5)failure();}
extern "C" bool __attribute__((ms_abi)) equality(void*r,void*other,void*){event(3,r);argsOK&=other==nullptr;if(hook==1)put<void*>(self,0x160,groupB);if(hook==2)put<void*>(self,0x160,nullptr);if(mode==6)failure();return r==nullptr||mode==7;}
extern "C" void* __attribute__((ms_abi)) gameObject(void*r,void*){event(4,r);return mode==8?nullptr:game;}
extern "C" void* __attribute__((ms_abi)) addGroup(void*r,void*){event(5,r);if(mode==9)failure();return mode==10?nullptr:groupB;}
extern "C" void __attribute__((ms_abi)) barrier(void*,void*){}
extern "C" void __attribute__((ms_abi)) layer(void*r,void*s,void*){event(6,r);argsOK&=s==layerName;if(hook==3)put<void*>(self,0x160,groupB);if(hook==4)put<void*>(self,0x160,nullptr);if(hook==5)put<uint32_t>(self,0x34,0x87654321);if(mode==11)failure();}
extern "C" void __attribute__((ms_abi)) setOrder(void*r,int value,void*){event(7,r);order=value;setCalls++;if(mode==12)failure();}
extern "C" void* __attribute__((ms_abi)) getFrame(void*r,void*){event(8,r);if(mode==13)failure();return mode==1?nullptr:frame;}
extern "C" void* __attribute__((ms_abi)) cachedMesh(void*r,void*){event(9,r);if(mode==14)failure();return mode==2?nullptr:mesh;}
extern "C" B6* __attribute__((ms_abi)) bounds(B6*out,void*r,void*){event(10,r);*out=r==rendererB?alternate:boundsValue;if(mode==15)failure();return out;}
extern "C" void __attribute__((ms_abi)) update(void*r,void*){event(11,r);updateCalls++;if(hook==1)put<void*>(self,0x28,rendererB);if(hook==2)put<void*>(self,0x28,nullptr);if(mode==16)failure();}
extern "C" bool __attribute__((ms_abi)) truth(void*r,void*){event(12,r);if(hook==3)put<void*>(self,0x28,rendererB);if(hook==4)put<void*>(self,0x28,nullptr);if(mode==17)failure();return r!=nullptr&&mode!=7;}
void dependency(uint64_t va,void*f){unsigned char bytes[12]={};bytes[0]=0x48;bytes[1]=0xb8;bytes[10]=0xff;bytes[11]=0xe0;std::memcpy(bytes+2,&f,8);std::memcpy((void*)va,bytes,12);if(std::memcmp((void*)va,bytes,12))throw std::runtime_error("trampoline");}
int main(int argc,char**argv){if(argc!=2)return 2;std::ifstream f(argv[1],std::ios::binary);std::vector<char>b((std::istreambuf_iterator<char>(f)),{});auto nt=at<uint32_t>(b,60);if(at<uint32_t>(b,nt)!=0x4550||at<uint16_t>(b,nt+24)!=0x20b)throw std::runtime_error("PE64 required");auto base=at<uint64_t>(b,nt+48);auto size=at<uint32_t>(b,nt+80);auto ns=at<uint16_t>(b,nt+6),opt=at<uint16_t>(b,nt+20);auto p=mmap((void*)base,size,PROT_READ|PROT_WRITE,MAP_PRIVATE|MAP_ANONYMOUS|MAP_FIXED_NOREPLACE,-1,0);if(p==MAP_FAILED||reinterpret_cast<uint64_t>(p)!=base)throw std::runtime_error("base");std::memcpy(p,b.data(),at<uint32_t>(b,nt+84));for(int i=0;i<ns;i++){auto s=nt+24+opt+40*i;auto rva=at<uint32_t>(b,s+12),len=at<uint32_t>(b,s+16),raw=at<uint32_t>(b,s+20);if(uint64_t(raw)+len>b.size()||uint64_t(rva)+len>size)throw std::runtime_error("bounds");std::memcpy((char*)p+rva,b.data()+raw,len);}





std::vector<std::pair<uint64_t,std::vector<char>>> originals;
originals.push_back({0x180311e40,std::vector<char>((char*)0x180311e40,(char*)0x180311eb4)});
originals.push_back({0x180367440,std::vector<char>((char*)0x180367440,(char*)0x180367532)});
originals.push_back({0x1803c8820,std::vector<char>((char*)0x1803c8820,(char*)0x1803c889a)});
originals.push_back({0x1803c88a0,std::vector<char>((char*)0x1803c88a0,(char*)0x1803c896c)});
for(auto va:{0x181ce4b8cULL,0x181ce4d83ULL,0x181ce4fc6ULL})put<uint8_t>((void*)va,0,1);put<int>(readyClass,0xe0,1);put<void*>((void*)0x181bb0500,0,readyClass);put<void*>((void*)0x181bba7a8,0,formatName);put<void*>((void*)0x181bc0da0,0,layerName);put<void*>(text,0,textClass);put<void*>(textB,0,textClass);put<void*>(textClass,0x558,(void*)&setText);put<void*>(textClass,0x560,nullptr);
dependency(0x180cc5a50,(void*)&format);dependency(0x18131f760,(void*)&equality);dependency(0x1813044d0,(void*)&gameObject);dependency(0x18046d280,(void*)&addGroup);dependency(0x18024f360,(void*)&barrier);dependency(0x181348060,(void*)&layer);dependency(0x1813480b0,(void*)&setOrder);dependency(0x1803c7ad0,(void*)&getFrame);dependency(0x1803c3fb0,(void*)&cachedMesh);dependency(0x1812f3be0,(void*)&bounds);dependency(0x1812fb100,(void*)&bounds);dependency(0x1803c7c30,(void*)&update);dependency(0x18131f870,(void*)&truth);dependency(0x180250150,(void*)&nre);
for(const auto&r:originals)if(std::memcmp((void*)r.first,r.second.data(),r.second.size()))throw std::runtime_error("Caller changed");if(mprotect(p,size,PROT_READ|PROT_EXEC))throw std::runtime_error("mprotect");
std::cerr<<"Four unchanged original native callers, initialized metadata and supplied recording formatter/text/Unity/mesh/frame/update/GC/exception dependencies. No complete helper or game/iOS fidelity proof. Native float-to-integer instruction executes unchanged for edge values.\n";
using Void=void(__attribute__((ms_abi))*)(void*,void*);using Get=B6*(__attribute__((ms_abi))*)(B6*,void*,void*);std::mt19937 rng(0x582026);uint32_t edges[]={0,0x80000000,0x3f800000,0xbf800000,0x7f800000,0xff800000,0x7fc12345,0x7f812345,0x4ccccccc,0x4ccccccd,0xcccccccc,0xcccccccd,1,0x80000001,0x7f7fffff,0xffc54321};
for(int t:{0x0600017B,0x06000477,0x06000837,0x06000838})for(int k=0;k<4096;k++){
token=t;mode=k%32;hook=(k/32)%8;input=k<512?edges[k/32]:(uint32_t)rng();for(int i=0;i<6;i++){boundsValue.v[i]=(uint32_t)rng();alternate.v[i]=(uint32_t)rng();output.v[i]=0xdeadbeef;}trace=0;err=setCalls=updateCalls=0;order=(int)0xdeadbeef;formattedValue=0xdeadbeef;argsOK=true;std::memset(self,0,sizeof self);if(t==0x0600017B){put<void*>(self,0x20,mode==1?nullptr:text);put<uint32_t>(self,0x28,input);}if(t==0x06000477){put<void*>(self,0x160,mode==1?nullptr:group);put<uint32_t>(self,0x34,input);}if(t==0x06000838)put<void*>(self,0x28,mode==1?nullptr:renderer);
if(setjmp(jump)==0){if(t==0x0600017B)((Void)0x180311e40)(self,nullptr);if(t==0x06000477)((Void)0x180367440)(self,nullptr);if(t==0x06000837){B6 result;auto r=((Get)0x1803c8820)(&result,self,nullptr);argsOK&=r==&result;output=result;}if(t==0x06000838){B6 result;auto r=((Get)0x1803c88a0)(&result,self,nullptr);argsOK&=r==&result;output=result;}}
std::cout<<"0x"<<std::hex<<t<<std::dec<<" "<<mode<<" "<<hook<<" "<<input;for(int i=0;i<6;i++)std::cout<<" "<<boundsValue.v[i];for(int i=0;i<6;i++)std::cout<<" "<<alternate.v[i];std::cout<<" "<<trace<<" "<<err<<" "<<argsOK;for(int i=0;i<6;i++)std::cout<<" "<<output.v[i];std::cout<<" "<<(t==0x0600017B?get<uint32_t>(self,0x28):0)<<" "<<(t==0x0600017B?id(get<void*>(self,0x20)):0)<<" "<<(t==0x06000477?get<uint32_t>(self,0x34):0)<<" "<<(t==0x06000477?id(get<void*>(self,0x160)):0)<<" "<<(uint32_t)order<<" "<<setCalls<<" "<<updateCalls<<" "<<(t==0x06000838?id(get<void*>(self,0x28)):0)<<" "<<formattedValue<<"\n";
}munmap(p,size);}
