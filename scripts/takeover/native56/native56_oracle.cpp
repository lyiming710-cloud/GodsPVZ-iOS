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
alignas(16)char self[1024],card[512],cardB[512],camera[64],clip[64],clipB[64],game[64],gameB[64],track[64],trackB[64],sprite[64],spriteB[64],transform[64],transformB[64],transformC[64],transformD[64],renderer[64],listG[64],listGB[64],listC[64],listCB[64],audio[64],plant[1024],plantB[1024],animator[64],animatorB[64],readyClass[512],resourceClass[512],resourceStatic[512],zeroClass[512],zeroStatic[128],nameAttack[64];
int token,mode,hook,err,itemCalls,truthCalls,positionCalls,randomCalls,audioCalls,parentCalls;uint64_t trace;bool argsOK;V3 value,alternate,spare,output,scale;C4 color;uint32_t scalar,ra,rb,capturedVolume,eulerX,eulerY;void*audioClip;void*parentValue;void*cardGame;
std::jmp_buf jump;
template<class T>T get(void*p,size_t off){T x;std::memcpy(&x,(char*)p+off,sizeof x);return x;}
int id(void*p){void*all[]={nullptr,self,card,cardB,camera,clip,clipB,game,gameB,track,trackB,sprite,spriteB,transform,transformB,transformC,transformD,renderer,listG,listGB,listC,listCB,audio,plant,plantB,animator,animatorB};for(int i=0;i<27;i++)if(p==all[i])return i;throw std::runtime_error("unknown identity");}
void event(int e,void*r=nullptr){trace=trace*131+e*32+id(r);}extern "C" void __attribute__((ms_abi)) nre(){event(30);err=1;std::longjmp(jump,1);}void failure(){event(32);err=3;std::longjmp(jump,1);}void bounds(){event(31);err=2;std::longjmp(jump,1);}float fl(uint32_t b){float f;std::memcpy(&f,&b,4);return f;}uint32_t bits(float f){uint32_t b;std::memcpy(&b,&f,4);return b;}
extern "C" void* __attribute__((ms_abi)) mainCamera(void*){event(1);return mode==10?nullptr:camera;}
extern "C" void* __attribute__((ms_abi)) componentTransform(void*r,void*){event(2,r);if(token==0x060000EC){if(hook==1)put<void*>(self,0xa8,gameB);if(hook==2)put<void*>(self,0xa8,nullptr);}if(mode==4)return nullptr;return r==card?transformC:r==cardB?transformD:token==0x06000331&&r==self?transformB:transform;}
extern "C" void* __attribute__((ms_abi)) gameTransform(void*r,void*){event(3,r);if(token==0x06000331||token==0x060003D4){size_t off=token==0x06000331?0xd0:0xd8;if(hook==1)put<void*>(self,off,gameB);if(hook==2)put<void*>(self,off,nullptr);}if(token==0x060003D7){if(hook==5)put<void*>(self,0xc8,spriteB);if(hook==6)put<void*>(self,0xe0,nullptr);}if(mode==3)return nullptr;return r==gameB||r==spriteB?transformB:r==track?transformC:r==trackB?transformD:transform;}
extern "C" void* __attribute__((ms_abi)) componentGame(void*r,void*){event(4,r);return mode==15?nullptr:cardGame;}
extern "C" V3* __attribute__((ms_abi)) position(V3*out,void*r,void*){event(5,r);if(mode==5)failure();positionCalls++;*out=r==transformB||r==transformD?alternate:value;if(token==0x06000331||token==0x060003D4){size_t off=token==0x06000331?0xd0:0xd8;if(hook==7)put<void*>(self,off,gameB);if(hook==8)put<void*>(self,off,nullptr);}if(hook==11)value=spare;return out;}
extern "C" void __attribute__((ms_abi)) setPosition(void*r,V3*p,void*){event(6,r);output=*p;if(mode==6)failure();}
extern "C" void __attribute__((ms_abi)) setLocalPosition(void*r,V3*p,void*){event(7,r);output=*p;if(mode==6)failure();}
extern "C" void __attribute__((ms_abi)) setScale(void*r,V3*p,void*){event(8,r);scale=*p;if(token==0x060003D4){if(hook==9)put<void*>(self,0xd8,gameB);if(hook==10)put<void*>(self,0xd8,nullptr);}if(mode==6)failure();}
extern "C" void __attribute__((ms_abi)) setParent(void*r,void*p,bool world,void*){event(9,r);argsOK&=!world;parentValue=p;parentCalls++;if(hook==3)cardGame=gameB;if(mode==6)failure();}
extern "C" bool __attribute__((ms_abi)) implicitObject(void*r,void*){event(10,r);truthCalls++;if(token==0x060003D7){if(truthCalls==1){if(hook==1)put<void*>(self,0xe0,trackB);if(hook==2)put<void*>(self,0xe0,nullptr);}else{if(hook==3)put<void*>(self,0xc8,spriteB);if(hook==4)put<void*>(self,0xc8,nullptr);}}if(token==0x06000482){if(hook==1)put<void*>(self,0x170,plantB);if(hook==2)put<void*>(self,0x170,nullptr);}if(token==0x0600067C){if(hook==1)put<void*>(resourceStatic,0x108,listCB);if(hook==2)put<void*>(resourceStatic,0x108,nullptr);}if(mode==21)failure();return r&&mode!=7;}
extern "C" bool __attribute__((ms_abi)) inequality(void*r,void*other,void*){argsOK&=other==nullptr;return implicitObject(r,nullptr);}
extern "C" float __attribute__((ms_abi)) range(float a,float b,void*){event(11);argsOK&=randomCalls==0?bits(a)==bits(-540.f)&&bits(b)==bits(400.f):bits(a)==bits(330.f)&&bits(b)==bits(360.f);uint32_t v=randomCalls++==0?ra:rb;if(hook==1)put<int>(self,0x58,7);if(mode==22)failure();return fl(v);}
extern "C" float __attribute__((ms_abi)) ortho(void*r,void*){event(12,r);if(mode==11)failure();return fl(scalar);}
extern "C" float __attribute__((ms_abi)) delta(void*){event(13);if(hook==1)put<uint32_t>(self,0x40,alternate.x);if(mode==23)failure();return fl(scalar);}
extern "C" void* __attribute__((ms_abi)) getItem(void*r,int index,void*){event(14,r);itemCalls++;argsOK&=index==(token==0x0600066E?9:0);if(mode==14)bounds();if(r==listG||r==listGB){if(token==0x060003C7&&hook==3)put<uint32_t>(self,0x40,alternate.x);return mode==17?nullptr:r==listGB?gameB:game;}return mode==18?nullptr:r==listCB?clipB:clip;}
extern "C" void* __attribute__((ms_abi)) getRenderer(void*r,void*){event(15,r);if(token==0x060003C7&&hook==4)put<uint32_t>(self,0x40,alternate.y);return mode==13?nullptr:renderer;}
extern "C" void __attribute__((ms_abi)) setColor(void*r,C4*p,void*){event(16,r);color=*p;if(mode==6)failure();}
extern "C" float __attribute__((ms_abi)) volume(void*){event(17);if(hook==5)value=alternate;if(mode==12)failure();return fl(scalar);}
extern "C" void __attribute__((ms_abi)) playClip(void*c,V3*p,float v,void*){event(18,c);audioCalls++;audioClip=c;output=*p;capturedVolume=bits(v);if(mode==16)failure();}
extern "C" void* __attribute__((ms_abi)) createSingle(void*c,void*){event(19,c);audioCalls++;audioClip=c;if(mode==16)failure();return mode==24?nullptr:audio;}
extern "C" bool __attribute__((ms_abi)) outOfMap(void*r,void*){event(20,r);if(hook==1)put<uint32_t>(self,0x4c,alternate.z);if(mode==25)failure();return mode!=26;}
extern "C" void __attribute__((ms_abi)) setEuler(void*r,float a,float b,void*){event(21,r);eulerX=bits(a);eulerY=bits(b);argsOK&=eulerX==0&&eulerY==bits(-90.f);if(hook==2)put<V3>(self,0xb8,alternate);if(mode==27)failure();}
extern "C" void __attribute__((ms_abi)) block(void*r,void*z,bool attack,void*){event(22,r);argsOK&=z==self&&!attack;if(hook==3)put<void*>(self,0x158,animatorB);if(hook==4)put<void*>(self,0x158,nullptr);if(mode==28)failure();}
extern "C" void __attribute__((ms_abi)) setBool(void*r,void*s,bool b,void*){event(23,r);argsOK&=s==nameAttack&&!b;if(mode==29)failure();}
extern "C" V3* __attribute__((ms_abi)) moveDirection(V3*out,void*r,void*){event(24,r);*out=value;if(hook==6)value=alternate;if(mode==30)failure();return out;}
extern "C" void __attribute__((ms_abi)) setSpeed(void*r,V3*p,void*){event(25,r);output=*p;if(mode==31)failure();}
extern "C" void __attribute__((ms_abi)) resetSpeed(void*r,void*){event(26,r);put<uint8_t>(self,0xbb,1);if(hook==7)failure();}
void dependency(uint64_t va,void*f){unsigned char bytes[12]={};bytes[0]=0x48;bytes[1]=0xb8;bytes[10]=0xff;bytes[11]=0xe0;std::memcpy(bytes+2,&f,8);std::memcpy((void*)va,bytes,12);if(std::memcmp((void*)va,bytes,12))throw std::runtime_error("trampoline");}
int main(int argc,char**argv){if(argc!=2)return 2;std::ifstream f(argv[1],std::ios::binary);std::vector<char>b((std::istreambuf_iterator<char>(f)),{});auto nt=at<uint32_t>(b,60);if(at<uint32_t>(b,nt)!=0x4550||at<uint16_t>(b,nt+24)!=0x20b)throw std::runtime_error("PE64 required");auto base=at<uint64_t>(b,nt+48);auto size=at<uint32_t>(b,nt+80);auto ns=at<uint16_t>(b,nt+6),opt=at<uint16_t>(b,nt+20);auto p=mmap((void*)base,size,PROT_READ|PROT_WRITE,MAP_PRIVATE|MAP_ANONYMOUS|MAP_FIXED_NOREPLACE,-1,0);if(p==MAP_FAILED||reinterpret_cast<uint64_t>(p)!=base)throw std::runtime_error("base");std::memcpy(p,b.data(),at<uint32_t>(b,nt+84));for(int i=0;i<ns;i++){auto s=nt+24+opt+40*i;auto rva=at<uint32_t>(b,s+12),len=at<uint32_t>(b,s+16),raw=at<uint32_t>(b,s+20);if(uint64_t(raw)+len>b.size()||uint64_t(rva)+len>size)throw std::runtime_error("bounds");std::memcpy((char*)p+rva,b.data()+raw,len);}





std::vector<std::pair<uint64_t,std::vector<char>>> originals;
originals.push_back({0x180301990,std::vector<char>((char*)0x180301990,(char*)0x180301a78)});
originals.push_back({0x18034ab50,std::vector<char>((char*)0x18034ab50,(char*)0x18034acc0)});
originals.push_back({0x1803772f0,std::vector<char>((char*)0x1803772f0,(char*)0x18037741f)});
originals.push_back({0x1803780d0,std::vector<char>((char*)0x1803780d0,(char*)0x1803781bb)});
originals.push_back({0x18037ced0,std::vector<char>((char*)0x18037ced0,(char*)0x18037d033)});
originals.push_back({0x18037d3d0,std::vector<char>((char*)0x18037d3d0,(char*)0x18037d4c6)});
originals.push_back({0x18037c990,std::vector<char>((char*)0x18037c990,(char*)0x18037ca3e)});
originals.push_back({0x18036a370,std::vector<char>((char*)0x18036a370,(char*)0x18036a45a)});
originals.push_back({0x1803a5ce0,std::vector<char>((char*)0x1803a5ce0,(char*)0x1803a5dd7)});
originals.push_back({0x1803a69b0,std::vector<char>((char*)0x1803a69b0,(char*)0x1803a6b3d)});
originals.push_back({0x181312e00,std::vector<char>((char*)0x181312e00,(char*)0x181312f80)});originals.push_back({0x181313310,std::vector<char>((char*)0x181313310,(char*)0x181313360)});
for(auto va:{0x181ce4dbeULL,0x181ce4dc5ULL,0x181ce4dc6ULL,0x181ce4b36ULL,0x181ce4d8cULL,0x181ce4edeULL,0x181ce4ee9ULL,0x181ceafa8ULL,0x181ceaf9fULL})put<uint8_t>((void*)va,0,1);
put<int>(readyClass,0xe0,1);put<int>(resourceClass,0xe0,1);put<void*>(resourceClass,0xb8,resourceStatic);put<void*>(zeroClass,0xb8,zeroStatic);put<void*>((void*)0x181bc27e8,0,zeroClass);put<void*>((void*)0x181bb0500,0,readyClass);put<void*>((void*)0x181bacbf8,0,readyClass);put<void*>((void*)0x181ba2018,0,readyClass);put<void*>((void*)0x181bb6478,0,resourceClass);put<void*>((void*)0x181bbab00,0,nameAttack);
dependency(0x1812e27c0,(void*)&mainCamera);dependency(0x181304510,(void*)&componentTransform);dependency(0x18131c070,(void*)&gameTransform);dependency(0x1813044d0,(void*)&componentGame);dependency(0x18132f3a0,(void*)&position);dependency(0x18132f070,(void*)&position);dependency(0x18132fbb0,(void*)&setPosition);dependency(0x18132f910,(void*)&setLocalPosition);dependency(0x18132fa50,(void*)&setScale);dependency(0x18132e900,(void*)&setParent);dependency(0x18131f870,(void*)&implicitObject);dependency(0x18131f900,(void*)&inequality);dependency(0x18130d350,(void*)&range);dependency(0x1812e2870,(void*)&ortho);dependency(0x18132c2f0,(void*)&delta);dependency(0x180825710,(void*)&getItem);dependency(0x18046dc80,(void*)&getRenderer);dependency(0x1813290a0,(void*)&setColor);dependency(0x18031aea0,(void*)&volume);dependency(0x1812dc040,(void*)&playClip);dependency(0x18031b160,(void*)&createSingle);dependency(0x18037ca40,(void*)&outOfMap);dependency(0x18037c120,(void*)&setEuler);dependency(0x18034df60,(void*)&block);dependency(0x1812dba00,(void*)&setBool);dependency(0x180361440,(void*)&moveDirection);dependency(0x180368280,(void*)&setSpeed);dependency(0x180366d90,(void*)&resetSpeed);dependency(0x180250150,(void*)&nre);
for(const auto&r:originals)if(std::memcmp((void*)r.first,r.second.data(),r.second.size()))throw std::runtime_error("Caller changed");if(mprotect(p,size,PROT_READ|PROT_EXEC))throw std::runtime_error("mprotect");
using CoreInit=void(__attribute__((ms_abi))*)(void*);using CoreZero=V3*(__attribute__((ms_abi))*)(V3*,void*);std::memset(zeroStatic,0xAB,sizeof zeroStatic);((CoreInit)0x181312e00)(nullptr);V3 zeroProof{};if(((CoreZero)0x181313310)(&zeroProof,nullptr)!=&zeroProof||zeroProof.x||zeroProof.y||zeroProof.z)throw std::runtime_error("Original zero initializer/getter");
std::cerr<<"Ten unchanged original callers and unchanged Core zero initializer/accessor. Ready metadata, supplied recording Unity/list/random/time/audio/exception dependencies. Math, arguments, null order, callbacks, branch choices and field mutations compared. No whole engine/class-init timing/game/iOS proof. Schema records only fields declared for each actual self type.\n";
using Void=void(__attribute__((ms_abi))*)(void*,void*);using Bank=void(__attribute__((ms_abi))*)(void*,void*,int,void*);using Device=void(__attribute__((ms_abi))*)(void*,V3*,void*);
std::mt19937 rng(0x562026);uint32_t edge[]={0,0x80000000,0x3f800000,0xbf800000,0x7f800000,0xff800000,0x7fc12345,0x7f812345,1,0x80000001,0x7f7fffff,0xffc54321};int ints[]={INT32_MIN,INT32_MAX,-2,-1,0,1,2,3,4,9,100};
for(int t:{0x060000EC,0x06000331,0x060003C7,0x060003CB,0x060003D4,0x060003D7,0x060003FB,0x06000482,0x0600066E,0x0600067C})for(int k=0;k<4096;k++){
token=t;mode=k%32;hook=(k/32)%12;int argument=ints[(k/384)%11];uint32_t initial=k<384?edge[k/32]:(uint32_t)rng();V3 input{(uint32_t)rng(),(uint32_t)rng(),(uint32_t)rng()};value={k<384?edge[k/32]:(uint32_t)rng(),k<384?edge[11-k/32]:(uint32_t)rng(),(uint32_t)rng()};alternate={(uint32_t)rng(),(uint32_t)rng(),(uint32_t)rng()};spare={(uint32_t)rng(),(uint32_t)rng(),(uint32_t)rng()};auto original=value;
// The by-value Device argument and first getter return are separate, deterministic inputs.
if(t==0x06000331)input={spare.x,spare.y,spare.z};uint32_t fw=(uint32_t)rng(),fd=(uint32_t)rng(),sz=(uint32_t)rng();scalar=k<384?edge[11-k/32]:(uint32_t)rng();ra=k<384?edge[k/32]:(uint32_t)rng();rb=(uint32_t)rng();trace=0;err=itemCalls=truthCalls=positionCalls=randomCalls=audioCalls=parentCalls=0;argsOK=true;output={0xdeadbeef,0xdeadbeef,0xdeadbeef};scale=output;color={0xdeadbeef,0xdeadbeef,0xdeadbeef,0xdeadbeef};audioClip=parentValue=nullptr;capturedVolume=eulerX=eulerY=0xdeadbeef;cardGame=game;
std::memset(self,0,sizeof self);put<void*>(resourceStatic,0x108,mode==9?nullptr:listC);put<void*>(resourceStatic,0x120,mode==9?nullptr:listC);
if(t==0x060000EC)put<void*>(self,0xa8,mode==2?nullptr:game);
if(t==0x06000331){put<void*>(self,0xd0,mode==1?nullptr:game);put<uint32_t>(self,0x38,0x12345678);put<uint32_t>(self,0x3c,0x87654321);}
if(t==0x060003C7){put<uint32_t>(self,0x40,initial);put<void*>(self,0x98,mode==9?nullptr:listG);}
if(t==0x060003CB)put<int>(self,0x58,argument);
if(t==0x060003D4){put<void*>(self,0xd8,mode==1?nullptr:game);put<uint32_t>(self,0x54,fw);put<uint32_t>(self,0x58,fd);put<uint32_t>(self,0x50,sz);}
if(t==0x060003D7){put<void*>(self,0xe0,mode==1?nullptr:track);put<void*>(self,0xc8,mode==2?nullptr:sprite);}
if(t==0x060003FB){put<uint32_t>(self,0x44,0x12345678);put<uint32_t>(self,0x48,0x87654321);put<uint32_t>(self,0x4c,initial);put<V3>(self,0x88,alternate);put<uint32_t>(self,0xa4,0x11111111);put<uint32_t>(self,0xa8,0x22222222);put<V3>(self,0xb8,spare);}
if(t==0x06000482){put<void*>(self,0x170,mode==19?nullptr:plant);put<void*>(self,0x158,mode==20?nullptr:animator);put<uint8_t>(self,0xbb,1);}
if(t==0x0600066E)put<int>(self,0x20,argument);
if(setjmp(jump)==0){if(t==0x060000EC)((Bank)0x180301990)(self,mode==1?nullptr:card,argument,nullptr);if(t==0x06000331)((Device)0x18034ab50)(self,&input,nullptr);if(t==0x060003C7)((Void)0x1803772f0)(self,nullptr);if(t==0x060003CB)((Void)0x1803780d0)(self,nullptr);if(t==0x060003D4)((Void)0x18037ced0)(self,nullptr);if(t==0x060003D7)((Void)0x18037d3d0)(self,nullptr);if(t==0x060003FB)((Void)0x18037c990)(self,nullptr);if(t==0x06000482)((Void)0x18036a370)(self,nullptr);if(t==0x0600066E)((Void)0x1803a5ce0)(self,nullptr);if(t==0x0600067C)((Void)0x1803a69b0)(self,nullptr);}
V3 speed=t==0x060003FB?get<V3>(self,0x88):V3{0,0,0};int child=t==0x060000EC?id(get<void*>(self,0xa8)):t==0x06000331?id(get<void*>(self,0xd0)):t==0x060003D4?id(get<void*>(self,0xd8)):t==0x060003D7?id(get<void*>(self,0xe0)):t==0x06000482?id(get<void*>(self,0x170)):0;int other=t==0x060003D7?id(get<void*>(self,0xc8)):t==0x06000482?id(get<void*>(self,0x158)):0;
std::cout<<"0x"<<std::hex<<t<<std::dec<<" "<<mode<<" "<<hook<<" "<<(uint32_t)argument<<" "<<initial<<" "<<original.x<<" "<<original.y<<" "<<original.z<<" "<<alternate.x<<" "<<alternate.y<<" "<<alternate.z<<" "<<spare.x<<" "<<spare.y<<" "<<spare.z<<" "<<fw<<" "<<fd<<" "<<sz<<" "<<scalar<<" "<<ra<<" "<<rb<<" "<<trace<<" "<<err<<" "<<argsOK<<" "<<output.x<<" "<<output.y<<" "<<output.z<<" "<<scale.x<<" "<<scale.y<<" "<<scale.z<<" "<<color.r<<" "<<color.g<<" "<<color.b<<" "<<color.a<<" "<<(t==0x06000331?get<uint32_t>(self,0x38):t==0x060003FB?get<uint32_t>(self,0x44):0)<<" "<<(t==0x06000331?get<uint32_t>(self,0x3c):t==0x060003FB?get<uint32_t>(self,0x48):0)<<" "<<(t==0x060003FB?get<uint32_t>(self,0x4c):0)<<" "<<speed.x<<" "<<speed.y<<" "<<speed.z<<" "<<(t==0x060003FB?get<uint32_t>(self,0xa4):0)<<" "<<(t==0x060003FB?get<uint32_t>(self,0xa8):0)<<" "<<(t==0x060003C7?get<uint32_t>(self,0x40):0)<<" "<<(t==0x06000482?(int)get<uint8_t>(self,0xbb):0)<<" "<<child<<" "<<other<<" "<<(t==0x0600067C?id(get<void*>(resourceStatic,0x108)):t==0x0600066E?id(get<void*>(resourceStatic,0x120)):0)<<" "<<(t==0x060003C7?id(get<void*>(self,0x98)):0)<<" "<<id(audioClip)<<" "<<capturedVolume<<" "<<audioCalls<<" "<<id(parentValue)<<" "<<parentCalls<<" "<<0<<" "<<eulerX<<" "<<eulerY<<" "<<(t==0x060000EC?id(cardGame):0)<<"\n";
}munmap(p,size);}
