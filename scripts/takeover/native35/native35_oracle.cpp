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
using Hash=uint32_t(__attribute__((ms_abi))*)(void*);
using Word=void(__attribute__((ms_abi))*)(void*,void*,int32_t,int32_t);
using Link=void(__attribute__((ms_abi))*)(void*,void*,void*,int32_t);
struct Trace {int count=0,receiver=0;void* a=nullptr;void* b=nullptr;int32_t x=0,y=0;void* mi=nullptr;} trace;
extern "C" void __attribute__((ms_abi)) record_word(void*e,void*s,int32_t x,int32_t y,void*mi){trace.count++;trace.receiver=*(int32_t*)((char*)e+0x10);trace.a=s;trace.x=x;trace.y=y;trace.mi=mi;}
extern "C" void __attribute__((ms_abi)) record_link(void*e,void*s,void*t,int32_t x,void*mi){trace.count++;trace.receiver=*(int32_t*)((char*)e+0x10);trace.a=s;trace.b=t;trace.x=x;trace.mi=mi;}
void recording_dependency(uint64_t va,void*callback){unsigned char jump[12]={0x48,0xb8,0,0,0,0,0,0,0,0,0xff,0xe0};std::memcpy(jump+2,&callback,8);std::memcpy((void*)va,jump,12);}
int main(int argc,char**argv){
 if(argc!=2)return 2;
 std::ifstream f(argv[1],std::ios::binary);std::vector<char>b((std::istreambuf_iterator<char>(f)),{});
 auto nt=at<uint32_t>(b,0x3c);if(at<uint32_t>(b,nt)!=0x4550||at<uint16_t>(b,nt+24)!=0x20b)throw std::runtime_error("PE64 required");
 auto base=at<uint64_t>(b,nt+48);auto size=at<uint32_t>(b,nt+80);auto ns=at<uint16_t>(b,nt+6),opt=at<uint16_t>(b,nt+20);
 auto p=mmap((void*)base,size,PROT_READ|PROT_WRITE,MAP_PRIVATE|MAP_ANONYMOUS|MAP_FIXED_NOREPLACE,-1,0);
 if(p==MAP_FAILED||reinterpret_cast<uint64_t>(p)!=base)throw std::runtime_error("preferred base unavailable");
 std::memcpy(p,b.data(),at<uint32_t>(b,nt+84));
 for(int i=0;i<ns;i++){auto s=nt+24+opt+40*i;auto rva=at<uint32_t>(b,s+12),len=at<uint32_t>(b,s+16),raw=at<uint32_t>(b,s+20);if(uint64_t(raw)+len>b.size()||uint64_t(rva)+len>size)throw std::runtime_error("section bounds");std::memcpy((char*)p+rva,b.data()+raw,len);}
 // Target instructions and hash get_Chars helper are unchanged. Only the
 // UnityEvent engine dependency entries are replaced by recording doubles.
 alignas(16)char wordMI[32]={},linkMI[32]={};
 for(uint64_t flag:{0x181ce4f52ULL,0x181ce4f53ULL,0x181ce4f54ULL})put<uint8_t>((void*)flag,0,1);
 put<void*>((void*)0x181baca48,0,wordMI);put<void*>((void*)0x181bacba8,0,linkMI);
 recording_dependency(0x180a66460,(void*)&record_word);recording_dependency(0x180a660f0,(void*)&record_link);
 if(mprotect(p,size,PROT_READ|PROT_EXEC))throw std::runtime_error("mprotect");
 std::cerr<<"Hash and get_Chars original machine code unchanged. Event caller bodies unchanged; metadata initialized preconditions supplied, two UnityEvent.Invoke dependencies replaced with recording doubles. No Unity startup/import/throw helpers. Callback behavior scope is call trace only.\n";
 std::mt19937 rng(0x3502026);auto hash=(Hash)0x1803ccf20;
 auto hc=[&](const std::vector<uint16_t>&chars,bool isnull=false){std::vector<char>s(0x16+2*chars.size());put<int32_t>(s.data(),0x10,chars.size());for(size_t i=0;i<chars.size();i++)put<uint16_t>(s.data(),0x14+2*i,chars[i]);auto value=hash(isnull?nullptr:s.data());std::cout<<"0x0600090C "<<(isnull?-1:(int)chars.size());for(auto c:chars)std::cout<<" "<<c;std::cout<<" "<<value<<"\n";};
 hc({},true);hc({});
 for(int c=0;c<65536;c++)hc({(uint16_t)c});
 for(auto s:std::vector<std::vector<uint16_t>>{{0,0},{65,0,66},{0xd800,0xdc00},{0xd800,0xd800},{0xffff,0,0xffff},{0xdc00,65}})hc(s);
 for(int i=0;i<8192;i++){std::vector<uint16_t>s(rng()%257);for(auto&c:s)c=rng();hc(s);}
 alignas(16)char strings[3][32]={},events[3][32]={};for(int i=0;i<3;i++)put<int32_t>(events[i],0x10,i+1);
 void* ptrs[]={nullptr,strings[0],strings[1],strings[2]};auto label=[&](void*p){for(int i=0;i<4;i++)if(ptrs[i]==p)return i;throw std::runtime_error("Unknown string identity");};
 std::vector<int32_t>ints={INT32_MIN,INT32_MAX,-1,0,1,17};
 auto ec=[&](int k,bool enabled,int a,int b,int32_t x,int32_t y){alignas(16)char handler[128]={};for(int i=0;i<3;i++)put<void*>(handler,0x30+8*i,i==k&&!enabled?nullptr:events[i]);trace={};
  if(k==2)((Link)0x1803b7ec0)(handler,ptrs[a],ptrs[b],x);else((Word)(k==0?0x1803b7fb0:0x1803b7e40))(handler,ptrs[a],x,y);
  std::cout<<(k==0?"0x0600073A":k==1?"0x0600073B":"0x0600073C")<<" "<<enabled<<" "<<a<<" "<<b<<" "<<x<<" "<<y<<" "<<trace.count<<" "<<trace.receiver<<" "<<label(trace.a)<<" "<<label(trace.b)<<" "<<trace.x<<" "<<trace.y<<" "<<(trace.mi==(k==2?(void*)linkMI:(void*)wordMI))<<"\n";
 };
 for(int k=0;k<3;k++)for(int enabled:{0,1})for(int a=0;a<4;a++)for(int b=0;b<4;b++)for(auto x:ints)for(auto y:ints)ec(k,enabled,a,b,x,y);
 for(int k=0;k<3;k++)for(int i=0;i<4096;i++)ec(k,rng()&1,rng()%4,rng()%4,(int32_t)rng(),(int32_t)rng());
 munmap(p,size);
}
