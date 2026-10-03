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
struct V3{uint32_t x,y,z;};struct C4{uint32_t r,g,b,a;};
alignas(16)char self[1024],plant[1024],plantB[1024],camera[64],clip[64],game[64],gameB[64],transform[64],transformB[64],image[64],imageB[64],imageClass[1024],button[256],buttonB[256],skill[128],skillB[128],buff[64],buffB[64],stats[128],audio[64],nameSSI[64],readyClass[512],zeroClass[512],zeroStatic[128];
int token,mode,hook,state,newstate,err,active,enabled,audioCalls;uint64_t trace;bool argsOK;V3 value,alternate,output;C4 color;uint32_t volume,capturedVolume,pitch;
std::jmp_buf jump;
template<class T>T get(void*p,size_t off){T x;std::memcpy(&x,(char*)p+off,sizeof x);return x;}
int id(void*p){void*all[]={nullptr,self,plant,plantB,camera,clip,game,gameB,transform,transformB,image,imageB,button,buttonB,skill,skillB,stats,buff,buffB,audio};for(int i=0;i<20;i++)if(p==all[i])return i;throw std::runtime_error("unknown identity");}
void event(int e,void*r=nullptr){trace=trace*131+e*32+id(r);}extern "C" void __attribute__((ms_abi)) nre(){event(14);err=1;std::longjmp(jump,1);}void failure(){event(15);err=3;std::longjmp(jump,1);}float fl(uint32_t b){float f;std::memcpy(&f,&b,4);return f;}uint32_t bits(float f){uint32_t b;std::memcpy(&b,&f,4);return b;}
extern "C" void* __attribute__((ms_abi)) mainCamera(void*){event(1);if(mode==12)failure();return mode==5?nullptr:camera;}
extern "C" void* __attribute__((ms_abi)) componentTransform(void*r,void*){event(2,r);if(mode==6)return nullptr;return r==plantB?transformB:transform;}
extern "C" void* __attribute__((ms_abi)) gameTransform(void*r,void*){event(16,r);if(mode==5)return nullptr;if(hook==3)value=alternate;return r==gameB?transformB:transform;}
extern "C" V3* __attribute__((ms_abi)) position(V3*out,void*r,void*){event(3,r);if(mode==7)failure();*out=value;if(hook==2){value=alternate;if(token==0x0600066A)put<void*>(self,0x38,gameB);}return out;}
extern "C" float __attribute__((ms_abi)) audioVolume(void*){event(4);if(hook==1)value=alternate;if(mode==8)failure();return fl(volume);}
extern "C" void* __attribute__((ms_abi)) create3(void*c,V3*p,float v,void*){event(5,c);audioCalls++;output=*p;capturedVolume=bits(v);pitch=0x3f800000;if(mode==9)failure();return mode==11?nullptr:audio;}
extern "C" void* __attribute__((ms_abi)) create4(void*c,V3*p,float v,float pch,void*){pitch=bits(pch);auto ret=create3(c,p,v,nullptr);pitch=bits(pch);return ret;}
extern "C" bool __attribute__((ms_abi)) implicitObject(void*r,void*){event(6,r);if(token==0x0600066A){if(hook==1)put<void*>(self,0x28,plantB);if(hook==4)put<void*>(self,0x28,nullptr);if(mode==3)failure();return r!=nullptr&&mode!=2;}if(mode==10)failure();return r!=nullptr&&mode!=4;}
extern "C" void* __attribute__((ms_abi)) componentGame(void*r,void*){event(7,r);if(mode==5)failure();return mode==2?nullptr:game;}
extern "C" void __attribute__((ms_abi)) setActive(void*r,bool b,void*){event(8,r);active=b;if(hook==2)put<void*>(self,0x38,imageB);if(hook==4)put<void*>(self,0x38,nullptr);if(mode==6)failure();}
extern "C" void __attribute__((ms_abi)) setColor(void*r,C4*p,void*){event(9,r);color=*p;size_t bo=token==0x06000568?0x50:0x60;if(hook==3)put<void*>(self,bo,buttonB);if(hook==5)put<void*>(self,bo,nullptr);if(mode==7)failure();}
extern "C" void __attribute__((ms_abi)) setEnabled(void*r,bool b,void*){event(10,r);enabled=b;if(mode==8)failure();}
extern "C" V3* __attribute__((ms_abi)) circle(V3*out,void*r,V3*v,void*){event(11,r);output=*v;if(mode==9)failure();*out=alternate;return out;}
extern "C" void __attribute__((ms_abi)) summon(void*r,void*){event(12,r);if(hook==1)put<void*>(self,0xe0,skillB);if(hook==2)put<void*>(self,0x220,buffB);if(mode==4)failure();}
extern "C" void* __attribute__((ms_abi)) findStats(void*r,void*s,void*){event(13,r);argsOK&=s==nameSSI;if(hook==3)put<uint32_t>(stats,0x60,0x87654321);if(mode==5)failure();return mode==3?nullptr:stats;}
extern "C" void __attribute__((ms_abi)) setPosition(void*r,V3*p,void*){event(17,r);output=*p;if(mode==8)failure();}
void dependency(uint64_t va,void*f){unsigned char bytes[12]={};bytes[0]=0x48;bytes[1]=0xb8;bytes[10]=0xff;bytes[11]=0xe0;std::memcpy(bytes+2,&f,8);std::memcpy((void*)va,bytes,12);if(std::memcmp((void*)va,bytes,12))throw std::runtime_error("trampoline");}
int main(int argc,char**argv){if(argc!=2)return 2;std::ifstream f(argv[1],std::ios::binary);std::vector<char>b((std::istreambuf_iterator<char>(f)),{});auto nt=at<uint32_t>(b,60);if(at<uint32_t>(b,nt)!=0x4550||at<uint16_t>(b,nt+24)!=0x20b)throw std::runtime_error("PE64 required");auto base=at<uint64_t>(b,nt+48);auto size=at<uint32_t>(b,nt+80);auto ns=at<uint16_t>(b,nt+6),opt=at<uint16_t>(b,nt+20);auto p=mmap((void*)base,size,PROT_READ|PROT_WRITE,MAP_PRIVATE|MAP_ANONYMOUS|MAP_FIXED_NOREPLACE,-1,0);if(p==MAP_FAILED||reinterpret_cast<uint64_t>(p)!=base)throw std::runtime_error("base");std::memcpy(p,b.data(),at<uint32_t>(b,nt+84));for(int i=0;i<ns;i++){auto s=nt+24+opt+40*i;auto rva=at<uint32_t>(b,s+12),len=at<uint32_t>(b,s+16),raw=at<uint32_t>(b,s+20);if(uint64_t(raw)+len>b.size()||uint64_t(rva)+len>size)throw std::runtime_error("bounds");std::memcpy((char*)p+rva,b.data()+raw,len);}




std::vector<std::pair<uint64_t,std::vector<char>>> originals;
originals.push_back({0x18030e030,std::vector<char>((char*)0x18030e030,(char*)0x18030e0d6)});
originals.push_back({0x1802ffa20,std::vector<char>((char*)0x1802ffa20,(char*)0x1802ffad4)});
originals.push_back({0x18031b160,std::vector<char>((char*)0x18031b160,(char*)0x18031b222)});
originals.push_back({0x180354740,std::vector<char>((char*)0x180354740,(char*)0x1803547b4)});
originals.push_back({0x18037ba70,std::vector<char>((char*)0x18037ba70,(char*)0x18037bae1)});
originals.push_back({0x180374b80,std::vector<char>((char*)0x180374b80,(char*)0x180374c7b)});
originals.push_back({0x18038a700,std::vector<char>((char*)0x18038a700,(char*)0x18038a7dc)});
originals.push_back({0x180394160,std::vector<char>((char*)0x180394160,(char*)0x18039423c)});
originals.push_back({0x18039e470,std::vector<char>((char*)0x18039e470,(char*)0x18039e56b)});
originals.push_back({0x1803a4540,std::vector<char>((char*)0x1803a4540,(char*)0x1803a4636)});
originals.push_back({0x181312e00,std::vector<char>((char*)0x181312e00,(char*)0x181312f80)});originals.push_back({0x181313310,std::vector<char>((char*)0x181313310,(char*)0x181313360)});put<uint8_t>((void*)0x181ceafa8,0,1);put<uint8_t>((void*)0x181ceaf9f,0,1);
for(auto va:{0x181ce4b1cULL,0x181ce4b65ULL,0x181ce4d1bULL,0x181ce4b36ULL,0x181ce4e17ULL,0x181ce4eb4ULL,0x181ce4edbULL})put<uint8_t>((void*)va,0,1);put<int>(readyClass,0xe0,1);put<void*>((void*)0x181ba2018,0,readyClass);put<void*>((void*)0x181bb0500,0,readyClass);put<void*>(zeroClass,0xb8,zeroStatic);put<void*>((void*)0x181bc27e8,0,zeroClass);put<void*>((void*)0x181baffd0,0,nameSSI);put<void*>(image,0,imageClass);put<void*>(imageB,0,imageClass);put<void*>(imageClass,0x2a8,(void*)&setColor);put<void*>(imageClass,0x2b0,nullptr);
dependency(0x1812e27c0,(void*)&mainCamera);dependency(0x181304510,(void*)&componentTransform);dependency(0x18131c070,(void*)&gameTransform);dependency(0x18132f3a0,(void*)&position);dependency(0x18031aea0,(void*)&audioVolume);dependency(0x18031b230,(void*)&create3);dependency(0x18031b350,(void*)&create4);dependency(0x18131f870,(void*)&implicitObject);dependency(0x1813044d0,(void*)&componentGame);dependency(0x18131bad0,(void*)&setActive);dependency(0x181302cb0,(void*)&setEnabled);dependency(0x1802ff490,(void*)&circle);dependency(0x180354ba0,(void*)&summon);dependency(0x1803103a0,(void*)&findStats);dependency(0x18132fbb0,(void*)&setPosition);dependency(0x180250150,(void*)&nre);
for(const auto&r:originals)if(std::memcmp((void*)r.first,r.second.data(),r.second.size()))throw std::runtime_error("Caller changed");if(mprotect(p,size,PROT_READ|PROT_EXEC))throw std::runtime_error("mprotect");std::cerr<<"Ten unchanged original callers; ready metadata/static zero and supplied recording Unity/audio/truth/color/buff/exception dependencies. Virtual color slot explicitly supplied. Caller math, arguments, branch priority, field reads and callback order only; no original helpers/engine/class-init/throw/game/iOS proof.\n";
using CoreInit=void(__attribute__((ms_abi))*)(void*);using CoreZero=V3*(__attribute__((ms_abi))*)(V3*,void*);std::memset(zeroStatic,0xAB,sizeof zeroStatic);((CoreInit)0x181312e00)(nullptr);V3 zeroProof{};if(((CoreZero)0x181313310)(&zeroProof,nullptr)!=&zeroProof||zeroProof.x||zeroProof.y||zeroProof.z)throw std::runtime_error("Original zero initializer/getter");std::cerr<<"Original Vector3 static initializer and public zero accessor also executed unchanged under explicitly ready metadata; initialized zero observed.\n";
using Void=void(__attribute__((ms_abi))*)(void*,void*);using One=void(__attribute__((ms_abi))*)(void*,void*,void*);using Bool=void(__attribute__((ms_abi))*)(void*,bool,void*);using Form=void(__attribute__((ms_abi))*)(void*,int,float,void*);using Vec=V3*(__attribute__((ms_abi))*)(V3*,void*,V3*,void*);using Audio=void*(__attribute__((ms_abi))*)(void*,void*);
std::mt19937 rng(0x552026);uint32_t edge[]={0,0x80000000,0x3f800000,0xbf800000,0x7f800000,0xff800000,0x7fc12345,0x7f812345,1,0x80000001,0x7f7fffff,0xffc54321};int ints[]={INT32_MIN,INT32_MAX,-2,-1,0,1,2,3,100};
for(int t:{0x060000CD,0x060000DD,0x0600013B,0x06000390,0x060003F5,0x06000532,0x06000568,0x060005B8,0x06000632,0x0600066A})for(int k=0;k<4096;k++){
token=t;mode=k%13;hook=(k/13)%6;state=t==0x06000568||t==0x060005B8?k%2:ints[(k/13)%9];newstate=ints[(k/117)%9];uint32_t life=k<12?edge[k]:(uint32_t)rng();value={k<12?edge[k]:(uint32_t)rng(),k<12?edge[11-k]:(uint32_t)rng(),(uint32_t)rng()};alternate={(uint32_t)rng(),(uint32_t)rng(),(uint32_t)rng()};auto original=value;volume=(uint32_t)rng();trace=0;err=audioCalls=0;argsOK=true;active=enabled=2;output={0xdeadbeef,0xdeadbeef,0xdeadbeef};color={0xdeadbeef,0xdeadbeef,0xdeadbeef,0xdeadbeef};capturedVolume=pitch=0xdeadbeef;void*ret=nullptr;V3 out{0xdeadbeef,0xdeadbeef,0xdeadbeef};
std::memset(self,0,sizeof self);put<int>(self,0x38,state);put<void*>(self,0x18,mode==1?nullptr:plant);put<void*>(self,0xe0,mode==1?nullptr:skill);put<void*>(self,0x220,mode==2?nullptr:buff);put<int>(skill,0x48,state);put<int>(skillB,0x48,newstate);put<uint32_t>(stats,0x60,0x12345678);put<int>(self,0x6c,-55);put<int>(self,0x74,-66);put<uint32_t>(self,0x78,0x11111111);put<uint32_t>(self,0x3c,0x22222222);put<V3>(self,0x88,alternate);void*c=mode==3?nullptr:clip;
if(t==0x06000532||t==0x06000632){put<void*>(self,t==0x06000532?0x40:0x30,mode==1?nullptr:button);put<uint8_t>(button,0xd8,mode!=2);}
if(t==0x06000568||t==0x060005B8){put<void*>(self,0x40,mode==1?nullptr:image);put<void*>(self,0x38,mode==3?nullptr:image);put<void*>(self,t==0x06000568?0x50:0x60,mode==4?nullptr:button);}
if(t==0x0600066A){put<void*>(self,0x28,mode==1?nullptr:plant);put<void*>(self,0x38,mode==4?nullptr:game);}
if(setjmp(jump)==0){if(t==0x060000CD)((One)0x18030e030)(self,c,nullptr);if(t==0x060000DD)ret=((Vec)0x1802ffa20)(&out,self,&original,nullptr);if(t==0x0600013B)ret=((Audio)0x18031b160)(c,nullptr);if(t==0x06000390)((Void)0x180354740)(self,nullptr);if(t==0x060003F5)((Form)0x18037ba70)(self,state,fl(life),nullptr);if(t==0x06000532)((One)0x180374b80)(self,c,nullptr);if(t==0x06000632)((One)0x18039e470)(self,c,nullptr);if(t==0x06000568)((Bool)0x18038a700)(self,state,nullptr);if(t==0x060005B8)((Bool)0x180394160)(self,state,nullptr);if(t==0x0600066A)((Void)0x1803a4540)(self,nullptr);}
auto result=t==0x060000DD?out:output;size_t childoff=t==0x0600066A?0x28:t==0x06000568||t==0x060005B8?0x38:0;size_t secoff=t==0x0600066A?0x38:t==0x06000568?0x50:t==0x060005B8?0x60:0;int child=childoff?id(get<void*>(self,childoff)):0,sec=secoff?id(get<void*>(self,secoff)):0;V3 speed=t==0x060003F5?get<V3>(self,0x88):V3{0,0,0};
std::cout<<"0x"<<std::hex<<t<<std::dec<<" "<<mode<<" "<<hook<<" "<<(uint32_t)state<<" "<<(uint32_t)newstate<<" "<<life<<" "<<original.x<<" "<<original.y<<" "<<original.z<<" "<<alternate.x<<" "<<alternate.y<<" "<<alternate.z<<" "<<volume<<" "<<trace<<" "<<err<<" "<<(t==0x0600013B?id(ret):0)<<" "<<result.x<<" "<<result.y<<" "<<result.z<<" "<<color.r<<" "<<color.g<<" "<<color.b<<" "<<color.a<<" "<<active<<" "<<enabled<<" "<<audioCalls<<" "<<capturedVolume<<" "<<pitch<<" "<<get<uint32_t>(stats,0x60)<<" "<<(t==0x060003F5?(uint32_t)get<int>(self,0x6c):0)<<" "<<(t==0x060003F5?(uint32_t)get<int>(self,0x74):0)<<" "<<(t==0x060003F5?get<uint32_t>(self,0x78):0)<<" "<<(t==0x060003F5?get<uint32_t>(self,0x3c):0)<<" "<<speed.x<<" "<<speed.y<<" "<<speed.z<<" "<<child<<" "<<sec<<" "<<argsOK<<"\n";
}munmap(p,size);}
