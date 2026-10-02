#include <sys/mman.h>
#include <cstdint>
#include <cstring>
#include <fstream>
#include <iostream>
#include <vector>
#include <random>
#include <stdexcept>
template<class T>T at(const std::vector<char>&b,size_t p){T x;std::memcpy(&x,b.data()+p,sizeof x);return x;}
template<class T>void put(void*p,size_t off,T x){std::memcpy(static_cast<char*>(p)+off,&x,sizeof x);}
using Add=void(__attribute__((ms_abi))*)(void*,void*);
using Insert=void(__attribute__((ms_abi))*)(void*,int32_t,void*);
extern "C" void __attribute__((ms_abi)) barrier(void*,void*){}
int calls=0;
extern "C" void __attribute__((ms_abi)) insert_dependency(void*p,int32_t index,void*value,void*){calls++;int count=*(int32_t*)((char*)p+0x18);void*arr=*(void**)((char*)p+0x10);for(int i=count;i>index;i--)put<void*>(arr,0x20+8*i,*(void**)((char*)arr+0x20+8*(i-1)));put<void*>(arr,0x20+8*index,value);put<int32_t>(p,0x18,count+1);put<uint32_t>(p,0x1c,*(uint32_t*)((char*)p+0x1c)+1);}
void dependency(uint64_t va,void*f){unsigned char b[12]={0x48,0xb8,0,0,0,0,0,0,0,0,0xff,0xe0};std::memcpy(b+2,&f,8);std::memcpy((void*)va,b,12);}
int main(int argc,char**argv){if(argc!=2)return 2;std::ifstream f(argv[1],std::ios::binary);std::vector<char>b((std::istreambuf_iterator<char>(f)),{});auto nt=at<uint32_t>(b,60);if(at<uint32_t>(b,nt)!=0x4550||at<uint16_t>(b,nt+24)!=0x20b)throw std::runtime_error("PE64 required");auto base=at<uint64_t>(b,nt+48);auto size=at<uint32_t>(b,nt+80);auto ns=at<uint16_t>(b,nt+6),opt=at<uint16_t>(b,nt+20);auto p=mmap((void*)base,size,PROT_READ|PROT_WRITE,MAP_PRIVATE|MAP_ANONYMOUS|MAP_FIXED_NOREPLACE,-1,0);if(p==MAP_FAILED||reinterpret_cast<uint64_t>(p)!=base)throw std::runtime_error("base");std::memcpy(p,b.data(),at<uint32_t>(b,nt+84));for(int i=0;i<ns;i++){auto s=nt+24+opt+40*i;auto rva=at<uint32_t>(b,s+12),len=at<uint32_t>(b,s+16),raw=at<uint32_t>(b,s+20);if(uint64_t(raw)+len>b.size()||uint64_t(rva)+len>size)throw std::runtime_error("bounds");std::memcpy((char*)p+rva,b.data()+raw,len);}
 for(uint64_t flag:{0x181ce4b3fULL,0x181ce4b91ULL,0x181ce4c5aULL,0x181ce4d75ULL})put<uint8_t>((void*)flag,0,1);
 dependency(0x18025ee40,(void*)&barrier);dependency(0x18084d9c0,(void*)&insert_dependency);
 if(mprotect(p,size,PROT_READ|PROT_EXEC))throw std::runtime_error("mprotect");
 std::cerr<<"Three original List.Add inlined caller bodies execute unchanged with spare capacity; no resize/allocator path. Original InsertPassNode caller executes unchanged with controlled List.Insert dependency. Metadata flags supplied and GC-barrier dependency suppressed. No native throw/Unity startup; not full collection/GC engine proof.\n";
 alignas(16)char objects[5][64]={},boards[4][32]={};void*values[]={nullptr,objects[1],objects[2],objects[3],objects[4]};void*boardValues[]={nullptr,boards[1],boards[2],boards[3]};auto id=[&](void*v){for(int i=0;i<5;i++)if(values[i]==v)return i;throw std::runtime_error("unknown identity");};std::mt19937 rng(0x3702026);
 struct E{const char*token;uint64_t va;int offset;bool device;};
 for(auto e:std::vector<E>{{"0x06000103",0x180310580,0x20,false},{"0x06000181",0x1803123a0,0x30,true},{"0x06000275",0x1803414f0,0x38,false}})for(int k=0;k<4096;k++){
  int count=rng()%65,cap=count+1+rng()%8,value=e.device?1+rng()%4:rng()%5,board=rng()%4,oldBoard=rng()%4;uint32_t version=rng();alignas(16)char manager[256]={},list[64]={};std::vector<char>arr(0x20+8*cap);put<uint64_t>(arr.data(),0x18,cap);put<void*>(list,0x10,arr.data());put<int32_t>(list,0x18,count);put<uint32_t>(list,0x1c,version);put<void*>(manager,e.offset,list);put<void*>(manager,0x20,e.device?boardValues[board]:list);std::vector<int>before;for(int i=0;i<count;i++){int v=rng()%5;before.push_back(v);put<void*>(arr.data(),0x20+8*i,values[v]);}if(value)put<void*>(values[value],0x28,boardValues[oldBoard]);((Add)e.va)(manager,values[value]);std::cout<<e.token<<" "<<count<<" "<<cap<<" "<<version;for(int v:before)std::cout<<" "<<v;std::cout<<" "<<value<<" "<<board<<" "<<oldBoard<<" "<<*(int32_t*)(list+0x18)<<" "<<*(uint32_t*)(list+0x1c);for(int i=0;i<=count;i++)std::cout<<" "<<id(*(void**)(arr.data()+0x20+8*i));std::cout<<" "<<(e.device?*(void**)((char*)values[value]+0x28)==boardValues[board]:true)<<"\n";
 }
 for(int k=0;k<4096;k++){
  bool present=rng()&1;int count=rng()%33,index=present?rng()%(count+1):(int32_t)rng(),value=rng()%5;uint32_t version=rng();alignas(16)char zombie[512]={},list[64]={};std::vector<char>arr(0x20+8*(count+1));put<uint64_t>(arr.data(),0x18,count+1);put<void*>(list,0x10,arr.data());put<int32_t>(list,0x18,count);put<uint32_t>(list,0x1c,version);put<void*>(zombie,0x120,present?list:nullptr);std::vector<int>before;for(int i=0;i<count;i++){int v=rng()%5;before.push_back(v);put<void*>(arr.data(),0x20+8*i,values[v]);}calls=0;((Insert)0x180365970)(zombie,index,values[value]);std::cout<<"0x06000462 "<<present<<" "<<count<<" "<<index<<" "<<value<<" "<<version;for(int v:before)std::cout<<" "<<v;std::cout<<" "<<calls<<" "<<*(int32_t*)(list+0x18)<<" "<<*(uint32_t*)(list+0x1c);for(int i=0;i<*(int32_t*)(list+0x18);i++)std::cout<<" "<<id(*(void**)(arr.data()+0x20+8*i));std::cout<<"\n";
 }
 munmap(p,size);
}
