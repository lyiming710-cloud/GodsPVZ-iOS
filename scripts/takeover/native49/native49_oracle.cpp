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
alignas(16)char resourceClass[512],objectClass[512],debugClass[512],fields[512],method[32],listA[256],listB[256],objA[32],objB[32],objC[32],prefix[32],suffix[32],missing[32],formatted[32],message[32];
void*itemsA[2],*itemsB[2];int hook,eqValue,input,getCalls,logs,formatValue,indexOK,listOK,concatOK,errorKind;uint64_t trace;std::jmp_buf jump;
void event(unsigned x){trace=(trace<<4)|x;}
void*current(){void*p;std::memcpy(&p,fields+0x1a0,8);return p;}void setCurrent(void*p){put<void*>(fields,0x1a0,p);}
void mutate(int stage){if(hook==stage)setCurrent(listB);if(hook==stage+2)setCurrent(nullptr);if(hook==stage+4){put<int>(listB,0x18,0);setCurrent(listB);}}
extern "C" void __attribute__((ms_abi)) nre(){event(6);errorKind=1;std::longjmp(jump,1);}
extern "C" void* __attribute__((ms_abi)) get_item(void*r,int ix,void*m){event(1);getCalls++;indexOK&=ix==0;listOK&=(r==listA||r==listB)&&m==method;int count;std::memcpy(&count,(char*)r+0x18,4);if((unsigned)ix>=(unsigned)count){event(7);errorKind=2;std::longjmp(jump,1);}return r==listA?itemsA[ix]:itemsB[ix];}
extern "C" bool __attribute__((ms_abi)) equality(void*r,void*other,void*){event(2);listOK&=r==itemsA[0]&&other==nullptr;mutate(1);return eqValue!=0;}
extern "C" void* __attribute__((ms_abi)) format_int(int*r,void*){event(3);formatValue=*r;return formatted;}
extern "C" void* __attribute__((ms_abi)) concat3(void*a,void*b,void*c,void*){event(4);concatOK&=a==prefix&&b==formatted&&c==suffix;return message;}
extern "C" void __attribute__((ms_abi)) log_object(void*p,void*){event(5);logs++;concatOK&=p==(getCalls?message:missing);if(getCalls)mutate(2);}
int id(void*p){return p==nullptr?0:p==objA?1:p==objB?2:p==objC?3:-1;}
void dependency(uint64_t va,void*f){unsigned char bytes[12]={};bytes[0]=0x48;bytes[1]=0xb8;bytes[10]=0xff;bytes[11]=0xe0;std::memcpy(bytes+2,&f,8);std::memcpy((void*)va,bytes,12);if(std::memcmp((void*)va,bytes,12)!=0)throw std::runtime_error("trampoline bytes");}
int main(int argc,char**argv){if(argc!=2)return 2;std::ifstream f(argv[1],std::ios::binary);std::vector<char>b((std::istreambuf_iterator<char>(f)),{});auto nt=at<uint32_t>(b,60);if(at<uint32_t>(b,nt)!=0x4550||at<uint16_t>(b,nt+24)!=0x20b)throw std::runtime_error("PE64 required");auto base=at<uint64_t>(b,nt+48);auto size=at<uint32_t>(b,nt+80);auto ns=at<uint16_t>(b,nt+6),opt=at<uint16_t>(b,nt+20);auto p=mmap((void*)base,size,PROT_READ|PROT_WRITE,MAP_PRIVATE|MAP_ANONYMOUS|MAP_FIXED_NOREPLACE,-1,0);if(p==MAP_FAILED||reinterpret_cast<uint64_t>(p)!=base)throw std::runtime_error("base");std::memcpy(p,b.data(),at<uint32_t>(b,nt+84));for(int i=0;i<ns;i++){auto s=nt+24+opt+40*i;auto rva=at<uint32_t>(b,s+12),len=at<uint32_t>(b,s+16),raw=at<uint32_t>(b,s+20);if(uint64_t(raw)+len>b.size()||uint64_t(rva)+len>size)throw std::runtime_error("bounds");std::memcpy((char*)p+rva,b.data()+raw,len);}


 put<uint8_t>((void*)0x181ce4c12,0,1);for(void*c:{(void*)resourceClass,(void*)objectClass,(void*)debugClass})put<int>(c,0xe0,1);put<void*>(resourceClass,0xb8,fields);
 put<void*>((void*)0x181bb6478,0,resourceClass);put<void*>((void*)0x181bb0500,0,objectClass);put<void*>((void*)0x181b9ad48,0,debugClass);put<void*>((void*)0x181bc3f28,0,method);
 put<void*>((void*)0x181bcb4f0,0,prefix);put<void*>((void*)0x181ba5468,0,suffix);put<void*>((void*)0x181ba4da8,0,missing);
 dependency(0x180825710,(void*)&get_item);dependency(0x18131f760,(void*)&equality);dependency(0x180ca3820,(void*)&format_int);dependency(0x180b73f50,(void*)&concat3);dependency(0x1812e68f0,(void*)&log_object);dependency(0x180250150,(void*)&nre);
 if(mprotect(p,size,PROT_READ|PROT_EXEC))throw std::runtime_error("mprotect");
 std::cerr<<"Unchanged original GetZombie_charred caller; ready class-init/metadata identities; recorded public-list indexer, Unity equality, formatter, concatenation, log and exception dependencies. Native direct count/static field reads retained. No original Unity equality/class-init/throw engine or iOS. Input ID intentionally unused.\n";
 using Fn=void*(__attribute__((ms_abi))*)(int,void*);std::mt19937 rng(0x492026);
 for(int k=0;k<4096;k++){input=k==0?0:k==1?INT32_MIN:k==2?INT32_MAX:(int)rng();int initial=k%3;hook=(k/3)%7;eqValue=(k/21)%2;int first=(k/42)%4,second=(k/168)%4;void*pool[]={nullptr,objA,objB,objC};itemsA[0]=pool[first];itemsA[1]=objC;itemsB[0]=pool[second];itemsB[1]=objA;put<int>(listA,0x18,initial==1?0:2);put<int>(listB,0x18,2);setCurrent(initial==0?nullptr:listA);trace=0;getCalls=logs=errorKind=0;formatValue=-1;indexOK=listOK=concatOK=1;void*r=nullptr;if(setjmp(jump)==0)r=((Fn)0x18032ffe0)(input,nullptr);
  std::cout<<"0x06000219 "<<(uint32_t)input<<" "<<initial<<" "<<hook<<" "<<eqValue<<" "<<first<<" "<<second<<" "<<trace<<" "<<getCalls<<" "<<logs<<" "<<(uint32_t)formatValue<<" "<<errorKind<<" "<<id(r)<<" "<<indexOK<<" "<<listOK<<" "<<concatOK<<"\n";
 }munmap(p,size);
}
