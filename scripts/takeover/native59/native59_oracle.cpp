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
struct V3{uint32_t x,y,z;};
alignas(16)char self[1024],board[1024],boardB[1024],config[64],map[64],manager[64],managerB[64],zombieManager[64],list[128],listB[128],clip[64],source[64],camera[64],transform[64],textUI[64],textB[64],game[64],mesh[64],meshB[64],data[64],dataB[64],element[64],readyClass[512],statics[1024],meshClass[64],elementClass[64],keyA[64],keyS[64],keyD[64],keyEmpty[64],strInitial[256],strChanged[256],strChar[256],strResult[256];
int token,mode,hook,indexInput,scenario,position,textChoice,err,result,activeCalls,audioCalls,textCode,hideFlags,fillCalls,elementType,elementBool;uint64_t trace;bool argsOK;uint32_t dtBits,volumeBits,pitchBits,scaleBits,volumeValue,pitchValue,elementFloat;uint16_t character;void*textRef;V3 pos,alternate,audioPos;
std::jmp_buf jump;
template<class T>T get(void*p,size_t off){T x;std::memcpy(&x,(char*)p+off,sizeof x);return x;}
float F(uint32_t x){float f;std::memcpy(&f,&x,4);return f;}uint32_t B(float f){uint32_t x;std::memcpy(&x,&f,4);return x;}
int id(void*p){void*all[]={nullptr,self,board,boardB,config,map,manager,managerB,zombieManager,list,listB,clip,source,camera,transform,textUI,textB,game,mesh,meshB,data,dataB,element};for(int i=0;i<23;i++)if(p==all[i])return i;throw std::runtime_error("unknown identity");}void event(int e,void*r=nullptr){trace=trace*131+e*32+id(r);}extern "C" void __attribute__((ms_abi)) nre(){event(30);err=1;std::longjmp(jump,1);}void failure(){event(32);err=3;std::longjmp(jump,1);}void check(void*r){if(!r)nre();}
void makeString(void*p,const std::vector<uint16_t>&s){put<int>(p,0x10,s.size());for(size_t i=0;i<s.size();i++)put<uint16_t>(p,0x14+i*2,s[i]);}std::vector<uint16_t>stringValue(void*p){if(!p)return {};int n=get<int>(p,0x10);if(n<0||n>80)throw std::runtime_error("string length");std::vector<uint16_t>s;for(int i=0;i<n;i++)s.push_back(get<uint16_t>(p,0x14+2*i));return s;}uint64_t stringHash(void*p){if(!p)return 0;uint64_t h=1;for(auto c:stringValue(p))h=h*131+c;return h;}
extern "C" float __attribute__((ms_abi)) delta(void*){event(1);if(hook==1)put<void*>(self,0xf8,managerB);if(hook==2)put<void*>(self,0xf8,nullptr);if(hook==3)put<void*>(self,0x1e8,nullptr);if(mode==4)failure();return F(dtBits);}
extern "C" void* __attribute__((ms_abi)) alloc(void*c){if(c==elementClass)return element;if(c==meshClass)return mode==10?nullptr:meshB;throw std::runtime_error("allocation class");}
extern "C" void __attribute__((ms_abi)) elementCtor(void*r,int type,float amount,void*d,bool yes,void*){event(2,r);elementType=type;elementFloat=B(amount);elementBool=yes;argsOK&=d==nullptr;if(mode==5)failure();}
extern "C" void __attribute__((ms_abi)) effect(void*r,void*e,void*){event(3,r);argsOK&=e==element;if(mode==6)failure();}
extern "C" void __attribute__((ms_abi)) barrier(void*,void*){}
extern "C" void __attribute__((ms_abi)) setText(void*r,void*s,bool yes,void*){event(4,r);argsOK&=yes;void*names[]={keyA,keyS,keyD,keyEmpty};textCode=-1;for(int i=0;i<4;i++)if(s==names[i])textCode=i;argsOK&=textCode>=0;if(hook==1)put<void*>(self,0x68,textB);if(hook==2)put<void*>(self,0x68,nullptr);if(mode==4)failure();}
extern "C" void* __attribute__((ms_abi)) gameObject(void*r,void*){event(5,r);return mode==5?nullptr:game;}
extern "C" void __attribute__((ms_abi)) active(void*r,bool yes,void*){event(6,r);argsOK&=yes;activeCalls++;if(mode==6)failure();}
extern "C" int __attribute__((ms_abi)) rangeInt(int a,int b,void*){event(7);argsOK&=a==61&&b==63;if(hook==1)put<void*>(zombieManager,0x28,listB);if(hook==2)put<void*>(zombieManager,0x28,nullptr);if(mode==4)failure();return indexInput;}
extern "C" void* __attribute__((ms_abi)) item(void*r,int i,void*){event(8,r);argsOK&=i==(token==0x0600046e?indexInput:1);if(mode==7||i<0||i>=64){event(31);err=2;std::longjmp(jump,1);}if(mode==8)failure();return mode==9?nullptr:clip;}
extern "C" float __attribute__((ms_abi)) volume(void*){event(9);if(token==0x0600046f){if(hook==1)put<int>(self,0x60,6);if(hook==2)put<int>(self,0x60,9);if(hook==3)put<void*>(statics,0x110,listB);if(hook==4)put<void*>(statics,0x110,nullptr);}if(hook==5)pos=alternate;if(mode==10)failure();return F(volumeBits);}
extern "C" float __attribute__((ms_abi)) rangeFloat(float a,float b,void*){bool pitch=a==0.9f&&b==1.1f;event(pitch?10:11);argsOK&=pitch||(a==0.6f&&b==0.8f);if(mode==(pitch?11:12))failure();return F(pitch?pitchBits:scaleBits);}
extern "C" void* __attribute__((ms_abi)) mainCamera(void*){event(12);return mode==13?nullptr:camera;}
extern "C" void* __attribute__((ms_abi)) getTransform(void*r,void*){event(13,r);return mode==14?nullptr:transform;}
extern "C" V3* __attribute__((ms_abi)) getPos(V3*out,void*r,void*){event(14,r);*out=pos;if(mode==15)failure();return out;}
extern "C" void* __attribute__((ms_abi)) audio(void*c,V3*p,float v,void*){event(15,c);audioCalls++;audioPos=*p;volumeValue=B(v);pitchValue=B(1);if(mode==16)failure();return source;}
extern "C" void* __attribute__((ms_abi)) audioPitch(void*c,V3*p,float v,float pitch,void*){event(15,c);audioCalls++;audioPos=*p;volumeValue=B(v);pitchValue=B(pitch);if(mode==16)failure();return source;}
extern "C" bool __attribute__((ms_abi)) truth(void*r,void*){event(16,r);if(hook==1)put<void*>(self,0x28,meshB);if(hook==2)put<void*>(self,0x28,nullptr);if(mode==4)failure();return r!=nullptr&&mode!=7;}
extern "C" void __attribute__((ms_abi)) meshCtor(void*r,void*){event(17,r);if(hook==3)put<void*>(self,0x18,dataB);if(mode==5)failure();}
extern "C" void __attribute__((ms_abi)) hide(void*r,int v,void*){event(18,r);hideFlags=v;if(hook==4)put<void*>(self,0x28,mesh);if(hook==5)put<void*>(self,0x28,nullptr);if(hook==6)put<void*>(self,0x18,dataB);if(mode==6)failure();}
extern "C" void __attribute__((ms_abi)) fill(void*r,void*d,void*){event(19,r);argsOK&=d==get<void*>(self,0x18);fillCalls++;if(hook==7)put<void*>(self,0x28,mesh);if(mode==8)failure();}
extern "C" void* __attribute__((ms_abi)) charFormat(uint16_t*p,void*){event(20);makeString(strChar,{*p});if(hook==1)textRef=strChanged;if(hook==2)position=12345;if(hook==3)*p='7';if(mode==13)failure();return strChar;}
extern "C" void* __attribute__((ms_abi)) concat(void*a,void*b,void*){event(21);auto s=stringValue(a),q=stringValue(b);s.insert(s.end(),q.begin(),q.end());makeString(strResult,s);if(hook==4)position=-1;if(hook==5)textRef=nullptr;if(mode==14)failure();return strResult;}
void dependency(uint64_t va,void*f){unsigned char bytes[12]={};bytes[0]=0x48;bytes[1]=0xb8;bytes[10]=0xff;bytes[11]=0xe0;std::memcpy(bytes+2,&f,8);std::memcpy((void*)va,bytes,12);if(std::memcmp((void*)va,bytes,12))throw std::runtime_error("trampoline");}
int main(int argc,char**argv){if(argc!=2)return 2;std::ifstream f(argv[1],std::ios::binary);std::vector<char>b((std::istreambuf_iterator<char>(f)),{});auto nt=at<uint32_t>(b,60);if(at<uint32_t>(b,nt)!=0x4550||at<uint16_t>(b,nt+24)!=0x20b)throw std::runtime_error("PE64 required");auto base=at<uint64_t>(b,nt+48);auto size=at<uint32_t>(b,nt+80);auto ns=at<uint16_t>(b,nt+6),opt=at<uint16_t>(b,nt+20);auto p=mmap((void*)base,size,PROT_READ|PROT_WRITE,MAP_PRIVATE|MAP_ANONYMOUS|MAP_FIXED_NOREPLACE,-1,0);if(p==MAP_FAILED||reinterpret_cast<uint64_t>(p)!=base)throw std::runtime_error("base");std::memcpy(p,b.data(),at<uint32_t>(b,nt+84));for(int i=0;i<ns;i++){auto s=nt+24+opt+40*i;auto rva=at<uint32_t>(b,s+12),len=at<uint32_t>(b,s+16),raw=at<uint32_t>(b,s+20);if(uint64_t(raw)+len>b.size()||uint64_t(rva)+len>size)throw std::runtime_error("bounds");std::memcpy((char*)p+rva,b.data()+raw,len);}





std::vector<std::pair<uint64_t,std::vector<char>>> originals;
originals.push_back({0x18035c240,std::vector<char>((char*)0x18035c240,(char*)0x18035c33a)});
originals.push_back({0x18037ea40,std::vector<char>((char*)0x18037ea40,(char*)0x18037eb05)});
originals.push_back({0x1803667c0,std::vector<char>((char*)0x1803667c0,(char*)0x1803668cc)});
originals.push_back({0x1803668d0,std::vector<char>((char*)0x1803668d0,(char*)0x180366a77)});
originals.push_back({0x1803b5810,std::vector<char>((char*)0x1803b5810,(char*)0x1803b58bd)});
originals.push_back({0x1803c3fb0,std::vector<char>((char*)0x1803c3fb0,(char*)0x1803c4070)});
for(auto va:{0x181ce4cf5ULL,0x181ce4ddcULL,0x181ce4d7bULL,0x181ce4d7cULL,0x181ce4f4cULL,0x181ce4fd6ULL})put<uint8_t>((void*)va,0,1);put<int>(readyClass,0xe0,1);put<void*>(readyClass,0xb8,statics);for(auto va:{0x181ba2018ULL,0x181bb6478ULL,0x181bb0500ULL,0x181bd3460ULL})put<void*>((void*)va,0,readyClass);put<void*>((void*)0x181bad540,0,meshClass);put<void*>((void*)0x181b9e168,0,elementClass);
put<void*>((void*)0x181ba9058,0,keyA);put<void*>((void*)0x181baf738,0,keyS);put<void*>((void*)0x181bba228,0,keyD);put<void*>((void*)0x181bcf088,0,keyEmpty);
dependency(0x18132c2f0,(void*)&delta);dependency(0x180250100,(void*)&alloc);dependency(0x180316940,(void*)&elementCtor);dependency(0x180315390,(void*)&effect);dependency(0x18024f360,(void*)&barrier);dependency(0x1811af070,(void*)&setText);dependency(0x1813044d0,(void*)&gameObject);dependency(0x18131bad0,(void*)&active);dependency(0x18130d310,(void*)&rangeInt);dependency(0x180825710,(void*)&item);dependency(0x18031aea0,(void*)&volume);dependency(0x18130d350,(void*)&rangeFloat);dependency(0x1812e27c0,(void*)&mainCamera);dependency(0x181304510,(void*)&getTransform);dependency(0x18132f3a0,(void*)&getPos);dependency(0x18031b230,(void*)&audio);dependency(0x18031b350,(void*)&audioPitch);dependency(0x18131f870,(void*)&truth);dependency(0x1812f3b20,(void*)&meshCtor);dependency(0x18131fa10,(void*)&hide);dependency(0x1803ca4a0,(void*)&fill);dependency(0x180b85b80,(void*)&charFormat);dependency(0x180b74270,(void*)&concat);dependency(0x180250150,(void*)&nre);
for(const auto&r:originals)if(std::memcmp((void*)r.first,r.second.data(),r.second.size()))throw std::runtime_error("Caller changed");if(mprotect(p,size,PROT_READ|PROT_EXEC))throw std::runtime_error("mprotect");
std::cerr<<"Six unchanged native callers, ready metadata and explicit recording helpers. Full ushort domain digit validation; ref string/index mutation and ordering. No complete helper/engine/game/iOS proof.\n";
using Void=void(__attribute__((ms_abi))*)(void*,void*);using Key=void(__attribute__((ms_abi))*)(void*,int,void*);using Mesh=void*(__attribute__((ms_abi))*)(void*,void*);using Digit=uint16_t(__attribute__((ms_abi))*)(void*,void**,int*,uint16_t,void*);
std::mt19937 rng(0x592026);uint32_t edges[]={0,0x80000000,0x3f800000,0xbf800000,0x7f800000,0xff800000,0x7fc12345,0x7f812345,0x3f000000,1,0x80000001,0x7f7fffff,0xffc54321};int special[]={-2147483647-1,-1,0,1,2,3,6,7,8,9,10,18,19,20,21,22,2147483647};
for(int t:{0x06000356,0x06000408,0x0600046E,0x0600046F,0x06000726,0x06000856})for(int k=0;k<(t==0x06000726?69632:4096);k++){
token=t;mode=k%32;hook=(k/32)%8;indexInput=special[(k/256)%17];scenario=(k/256)%23;dtBits=k<512?edges[(k/32)%13]:rng();volumeBits=edges[(k/32)%13];pitchBits=edges[(k/64)%13];scaleBits=edges[(k/128)%13];pos={(uint32_t)rng(),(uint32_t)rng(),(uint32_t)rng()};alternate={(uint32_t)rng(),(uint32_t)rng(),(uint32_t)rng()};V3 initialPos=pos;
character=t==0x06000726?(uint16_t)(k<65536?k:48+((k-65536)/256)%10):0;position=(k%3==0)?2147483647:((k%3==1)?-1:(int)rng());textChoice=(k/8)%4;int initialPosition=position;uint16_t inputChar=character;
makeString(strInitial,textChoice==1?std::vector<uint16_t>{}:textChoice==2?std::vector<uint16_t>{'a','b','c'}:std::vector<uint16_t>{0x3a9});makeString(strChanged,{'c','h','a','n','g','e','d'});textRef=textChoice==0?nullptr:strInitial;
trace=0;err=activeCalls=audioCalls=fillCalls=0;result=-1;textCode=hideFlags=elementType=elementBool=-1;elementFloat=volumeValue=pitchValue=0xdeadbeef;audioPos={0xdeadbeef,0xdeadbeef,0xdeadbeef};argsOK=true;std::memset(self,0,sizeof self);
if(t==0x06000356){put<void*>(self,0x1e8,mode==1?nullptr:board);put<void*>(board,0x28,mode==2?nullptr:config);put<void*>(board,0x30,mode==3?nullptr:map);put<int>(map,0x18,scenario);put<void*>(self,0xf8,mode==7?nullptr:manager);}
if(t==0x06000408)put<void*>(self,0x68,mode==1?nullptr:textUI);
if(t==0x0600046E){put<void*>(self,0x20,mode==1?nullptr:board);put<void*>(board,0xf0,mode==2?nullptr:zombieManager);put<void*>(zombieManager,0x28,mode==3?nullptr:list);}
if(t==0x0600046F){put<int>(self,0x60,scenario);put<void*>(statics,0x110,mode==3?nullptr:list);}
if(t==0x06000856){put<void*>(self,0x28,mode==1?nullptr:mesh);put<void*>(self,0x18,mode==2?nullptr:data);}
if(setjmp(jump)==0){if(t==0x06000356)((Void)0x18035c240)(self,nullptr);if(t==0x06000408)((Key)0x18037ea40)(self,indexInput,nullptr);if(t==0x0600046E)((Void)0x1803667c0)(self,nullptr);if(t==0x0600046F)((Void)0x1803668d0)(self,nullptr);if(t==0x06000856)result=id(((Mesh)0x1803c3fb0)(self,nullptr));if(t==0x06000726)result=((Digit)0x1803b5810)(self,&textRef,&position,character,nullptr);}
std::cout<<"0x"<<std::hex<<t<<std::dec<<" "<<mode<<" "<<hook<<" "<<(uint32_t)indexInput<<" "<<scenario<<" "<<dtBits<<" "<<volumeBits<<" "<<pitchBits<<" "<<scaleBits<<" "<<initialPos.x<<" "<<initialPos.y<<" "<<initialPos.z<<" "<<alternate.x<<" "<<alternate.y<<" "<<alternate.z<<" "<<inputChar<<" "<<(uint32_t)initialPosition<<" "<<textChoice;
std::cout<<" "<<trace<<" "<<err<<" "<<argsOK<<" "<<(uint32_t)result<<" "<<(t==0x06000356?id(get<void*>(self,0xf8)):0)<<" "<<((t==0x06000356||t==0x0600046E)?id(get<void*>(self,t==0x06000356?0x1e8:0x20)):0)<<" "<<(t==0x06000408?id(get<void*>(self,0x68)):0)<<" "<<(t==0x06000856?id(get<void*>(self,0x28)):0)<<" "<<(t==0x06000856?id(get<void*>(self,0x18)):0)<<" "<<(t==0x0600046F?get<uint32_t>(self,0x60):0)<<" "<<stringHash(textRef)<<" "<<(uint32_t)position<<" "<<(uint32_t)textCode<<" "<<activeCalls<<" "<<audioCalls<<" "<<volumeValue<<" "<<pitchValue<<" "<<audioPos.x<<" "<<audioPos.y<<" "<<audioPos.z<<" "<<(uint32_t)elementType<<" "<<elementFloat<<" "<<(uint32_t)elementBool<<" "<<(uint32_t)hideFlags<<" "<<fillCalls<<"\n";
}munmap(p,size);}
