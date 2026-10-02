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
alignas(16)char transformObject[64]={};void**slot=nullptr;void*alternate=nullptr;void*zombiePtr=nullptr;bool swapRef=false,receiverOK=true;int getterCalls=0,scaleCalls=0;
extern "C" void* __attribute__((ms_abi)) transform_dependency(void*z,void*){getterCalls++;receiverOK&=z==zombiePtr;if(swapRef)*slot=alternate;return transformObject;}
extern "C" void* __attribute__((ms_abi)) scale_dependency(void*result,void*t,void*){scaleCalls++;receiverOK&=t==transformObject;std::memcpy(result,transformObject+0x20,12);return result;}
void dependency(uint64_t va,void*f){unsigned char b[12]={0x48,0xb8,0,0,0,0,0,0,0,0,0xff,0xe0};std::memcpy(b+2,&f,8);std::memcpy((void*)va,b,12);}
int main(int argc,char**argv){if(argc!=2)return 2;std::ifstream f(argv[1],std::ios::binary);std::vector<char>b((std::istreambuf_iterator<char>(f)),{});auto nt=at<uint32_t>(b,60);if(at<uint32_t>(b,nt)!=0x4550||at<uint16_t>(b,nt+24)!=0x20b)throw std::runtime_error("PE64 required");auto base=at<uint64_t>(b,nt+48);auto size=at<uint32_t>(b,nt+80);auto ns=at<uint16_t>(b,nt+6),opt=at<uint16_t>(b,nt+20);auto p=mmap((void*)base,size,PROT_READ|PROT_WRITE,MAP_PRIVATE|MAP_ANONYMOUS|MAP_FIXED_NOREPLACE,-1,0);if(p==MAP_FAILED||reinterpret_cast<uint64_t>(p)!=base)throw std::runtime_error("base");std::memcpy(p,b.data(),at<uint32_t>(b,nt+84));for(int i=0;i<ns;i++){auto s=nt+24+opt+40*i;auto rva=at<uint32_t>(b,s+12),len=at<uint32_t>(b,s+16),raw=at<uint32_t>(b,s+20);if(uint64_t(raw)+len>b.size()||uint64_t(rva)+len>size)throw std::runtime_error("bounds");std::memcpy((char*)p+rva,b.data()+raw,len);}
 dependency(0x181304510,(void*)&transform_dependency);dependency(0x18132f1b0,(void*)&scale_dependency);if(mprotect(p,size,PROT_READ|PROT_EXEC))throw std::runtime_error("mprotect");_mm_setcsr(8064);
 std::cerr<<"Original GetDamageType body and jump table execute unchanged. Transform/localScale dependencies are recording doubles, with optional caller ref Damage replacement during getter to verify captured receiver identity. No Unity/native throw/allocator/GC engine executed; valid layouts and MXCSR8064.\n";
 std::mt19937 rng(0x4002026);std::vector<int32_t>ids;for(int i=-8;i<=31;i++)ids.push_back(i);ids.push_back(INT32_MIN);ids.push_back(INT32_MAX);std::vector<int32_t>attacks={INT32_MIN,-1,0,1,INT32_MAX},states={-1,0,1,2};std::vector<uint32_t>scales={0,0x80000000,1,0x80000001,0x3f800000,0xbf800000,0x7f7fffff,0xff7fffff,0x7f800000,0xff800000,0x7fc00001,0x7fa00001,0xffc00012,0x3fc00000,0xbfc00000};
 auto flags=[](const char*d){int out=0;for(int i=0;i<8;i++)out|=uint8_t(d[0x1c+i])<<i;return out;};using Fn=void(__attribute__((ms_abi))*)(void*,void**,int32_t,int32_t);alignas(16)char zombie[256]={},damage[128]={},alt[128]={};zombiePtr=zombie;alternate=alt;
 for(int32_t id:ids)for(int32_t attack:attacks)for(int32_t state:states)for(uint32_t bits:scales){bool right=(id>=6&&id<=9)||(id>=19&&id<=22),action=id==3||id==11||id==16||id==17&&attack>=0||id==18&&(state==0||state==1)||id==13||right&&attack>=0;bool present=action||(rng()&1);swapRef=rng()&1;int32_t camp=(int32_t)rng(),attr=(int32_t)rng();int initial=rng()%256;for(int i=0;i<128;i++){damage[i]=(char)rng();alt[i]=(char)rng();}put<int32_t>(damage,0x14,camp);put<int32_t>(damage,0x18,attr);for(int i=0;i<8;i++)put<uint8_t>(damage,0x1c+i,(initial>>i)&1);auto before=std::vector<char>(damage,damage+128),altBefore=std::vector<char>(alt,alt+128);put<int32_t>(zombie,0x60,id);put<uint32_t>(transformObject,0x20,bits);put<uint32_t>(transformObject,0x24,0x40400000);put<uint32_t>(transformObject,0x28,0x40800000);void*ref=present?damage:nullptr;slot=&ref;getterCalls=scaleCalls=0;receiverOK=true;((Fn)0x180360b00)(zombie,&ref,attack,state);bool untouched=true;for(int i=0;i<128;i++)if((i<0x14||i>=0x24)&&damage[i]!=before[i])untouched=false;bool altOK=std::memcmp(alt,altBefore.data(),128)==0;
  std::cout<<"0x06000447 "<<id<<" "<<attack<<" "<<state<<" "<<bits<<" "<<present<<" "<<swapRef<<" "<<camp<<" "<<attr<<" "<<initial<<" "<<*(int32_t*)(damage+0x14)<<" "<<*(int32_t*)(damage+0x18)<<" "<<flags(damage)<<" "<<getterCalls<<" "<<scaleCalls<<" "<<(ref==alt)<<" "<<altOK<<" "<<untouched<<" "<<receiverOK<<"\n";
 }
 munmap(p,size);
}
