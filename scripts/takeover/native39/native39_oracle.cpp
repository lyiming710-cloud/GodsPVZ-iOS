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
alignas(16)char boxes[4][32]={},intClass[256]={},debugClass[256]={},formatObject[64]={},returnObject[64]={};int boxCalls=0,debugCalls=0,formatCalls=0;int32_t values[4]={},logged=0,seenState=0;void*mapPtr=nullptr;bool classOK=true,fmtOK=true;
extern "C" void* __attribute__((ms_abi)) boxing_dependency(void*c,void*v){if(boxCalls>=4)throw std::runtime_error("box count");classOK&=c==intClass;int i=boxCalls++;values[i]=*(int32_t*)v;put<int32_t>(boxes[i],0x10,values[i]);return boxes[i];}
extern "C" void __attribute__((ms_abi)) debug_dependency(void*o,void*){debugCalls++;logged=*(int32_t*)((char*)o+0x10);seenState=*(int32_t*)((char*)mapPtr+0x18);}
extern "C" void* __attribute__((ms_abi)) format_dependency(void*fmt,void*a,void*b,void*c,void*){formatCalls++;fmtOK&=fmt==formatObject&&*(int32_t*)((char*)a+0x10)==1&&*(int32_t*)((char*)b+0x10)==3&&*(int32_t*)((char*)c+0x10)==15;return returnObject;}
void dependency(uint64_t va,void*f){unsigned char b[12]={0x48,0xb8,0,0,0,0,0,0,0,0,0xff,0xe0};std::memcpy(b+2,&f,8);std::memcpy((void*)va,b,12);}
int main(int argc,char**argv){if(argc!=2)return 2;std::ifstream f(argv[1],std::ios::binary);std::vector<char>b((std::istreambuf_iterator<char>(f)),{});auto nt=at<uint32_t>(b,60);if(at<uint32_t>(b,nt)!=0x4550||at<uint16_t>(b,nt+24)!=0x20b)throw std::runtime_error("PE64 required");auto base=at<uint64_t>(b,nt+48);auto size=at<uint32_t>(b,nt+80);auto ns=at<uint16_t>(b,nt+6),opt=at<uint16_t>(b,nt+20);auto p=mmap((void*)base,size,PROT_READ|PROT_WRITE,MAP_PRIVATE|MAP_ANONYMOUS|MAP_FIXED_NOREPLACE,-1,0);if(p==MAP_FAILED||reinterpret_cast<uint64_t>(p)!=base)throw std::runtime_error("base");std::memcpy(p,b.data(),at<uint32_t>(b,nt+84));for(int i=0;i<ns;i++){auto s=nt+24+opt+40*i;auto rva=at<uint32_t>(b,s+12),len=at<uint32_t>(b,s+16),raw=at<uint32_t>(b,s+20);if(uint64_t(raw)+len>b.size()||uint64_t(rva)+len>size)throw std::runtime_error("bounds");std::memcpy((char*)p+rva,b.data()+raw,len);}
 put<uint8_t>((void*)0x181ce4af6,0,1);put<uint8_t>((void*)0x181ce5001,0,1);put<void*>((void*)0x181ba95a8,0,intClass);put<void*>((void*)0x181b9ad48,0,debugClass);put<int32_t>(debugClass,0xe0,1);
 // Supply the exact managed user-string literal from the original metadata
 // operand; native caller forwards its identity to recording Format dependency.
 const char*literal="{0}.{1}.{2}";put<int32_t>(formatObject,0x10,11);for(int i=0;i<11;i++)put<uint16_t>(formatObject,0x14+2*i,literal[i]);put<void*>((void*)0x181b9c520,0,formatObject);
 dependency(0x18024f340,(void*)&boxing_dependency);dependency(0x1812e68f0,(void*)&debug_dependency);dependency(0x180b76eb0,(void*)&format_dependency);
 if(mprotect(p,size,PROT_READ|PROT_EXEC))throw std::runtime_error("mprotect");
 std::cerr<<"Original caller instructions unchanged. Boxing, Debug.Log and String.Format use recording doubles; initialized metadata/class flags and exact managed format literal supplied. Caller argument/state/order evidence only; no original allocation, log/format engine, native throw or Unity startup proof.\n";
 alignas(16)char host[64]={},config[128]={},map[64]={};put<void*>(host,0x20,config);put<void*>(config,0x30,map);mapPtr=map;std::mt19937 rng(0x3902026);using Set=void(__attribute__((ms_abi))*)(void*,int32_t);for(int k=0;k<8192;k++){int32_t value=(int32_t)rng(),initial=(int32_t)rng();if(k<4)value=k==0?0:k==1?-1:k==2?INT32_MIN:INT32_MAX;put<int32_t>(map,0x18,initial);boxCalls=debugCalls=0;classOK=true;((Set)0x18030c780)(host,value);std::cout<<"0x06000091 "<<value<<" "<<initial<<" "<<boxCalls<<" "<<debugCalls<<" "<<logged<<" "<<seenState<<" "<<*(int32_t*)(map+0x18)<<" "<<classOK<<"\n";}
 using Version=void*(__attribute__((ms_abi))*)(void*);for(int k=0;k<16;k++){boxCalls=formatCalls=0;classOK=fmtOK=true;void*out=((Version)0x1803ca860)(nullptr);std::cout<<"0x060008A5 "<<boxCalls<<" "<<values[0]<<" "<<values[1]<<" "<<values[2]<<" "<<formatCalls<<" "<<classOK<<" "<<fmtOK<<" "<<(out==returnObject)<<"\n";}
 munmap(p,size);
}
