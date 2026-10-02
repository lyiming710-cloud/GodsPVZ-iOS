#include <signal.h>
#include <ucontext.h>
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

alignas(16)char curveClass[512]={},input[128]={},created[128]={},alternate[128]={},array1[512]={},array2[512]={};void*latest=nullptr;void*returnArray=nullptr;bool swapLatest=false,ok=true;int allocCalls,ctorCalls,getCalls,setCalls;void*seenArray=nullptr;
void trap(int sig,siginfo_t*,void*context){auto*c=(ucontext_t*)context;std::cerr<<"signal="<<sig<<" rip="<<std::hex<<c->uc_mcontext.gregs[REG_RIP]<<" rdi="<<c->uc_mcontext.gregs[REG_RDI]<<" rbx="<<c->uc_mcontext.gregs[REG_RBX]<<std::dec<<" calls="<<allocCalls<<","<<ctorCalls<<","<<getCalls<<","<<setCalls<<"\n";_Exit(128+sig);}
extern "C" void* __attribute__((ms_abi)) alloc_dependency(void*klass){allocCalls++;ok&=klass==curveClass;latest=created;return created;}
extern "C" void __attribute__((ms_abi)) ctor_dependency(void*receiver,void*){ctorCalls++;ok&=receiver==created&&allocCalls==1;}
extern "C" void* __attribute__((ms_abi)) get_dependency(void*receiver,void*){getCalls++;ok&=receiver==input&&ctorCalls==1;if(swapLatest)latest=alternate;return returnArray;}
extern "C" void __attribute__((ms_abi)) set_dependency(void*receiver,void*array,void*){setCalls++;ok&=receiver==created&&getCalls==1;seenArray=array;}
void dependency(uint64_t va,void*f){unsigned char b[12]={};b[0]=0x48;b[1]=0xb8;b[10]=0xff;b[11]=0xe0;std::memcpy(b+2,&f,8);std::memcpy((void*)va,b,12);if(std::memcmp((void*)va,b,12)!=0||b[10]!=0xff||b[11]!=0xe0)throw std::runtime_error("dependency trampoline bytes");}
int main(int argc,char**argv){if(argc!=2)return 2;std::ifstream f(argv[1],std::ios::binary);std::vector<char>b((std::istreambuf_iterator<char>(f)),{});auto nt=at<uint32_t>(b,60);if(at<uint32_t>(b,nt)!=0x4550||at<uint16_t>(b,nt+24)!=0x20b)throw std::runtime_error("PE64 required");auto base=at<uint64_t>(b,nt+48);auto size=at<uint32_t>(b,nt+80);auto ns=at<uint16_t>(b,nt+6),opt=at<uint16_t>(b,nt+20);auto p=mmap((void*)base,size,PROT_READ|PROT_WRITE,MAP_PRIVATE|MAP_ANONYMOUS|MAP_FIXED_NOREPLACE,-1,0);if(p==MAP_FAILED||reinterpret_cast<uint64_t>(p)!=base)throw std::runtime_error("base");std::memcpy(p,b.data(),at<uint32_t>(b,nt+84));for(int i=0;i<ns;i++){auto s=nt+24+opt+40*i;auto rva=at<uint32_t>(b,s+12),len=at<uint32_t>(b,s+16),raw=at<uint32_t>(b,s+20);if(uint64_t(raw)+len>b.size()||uint64_t(rva)+len>size)throw std::runtime_error("bounds");std::memcpy((char*)p+rva,b.data()+raw,len);}

 struct sigaction action{};action.sa_sigaction=trap;action.sa_flags=SA_SIGINFO;sigaction(SIGTRAP,&action,nullptr);sigaction(SIGSEGV,&action,nullptr);
 for(uint64_t flag:{0x181ce4f6dULL,0x181ce4fb8ULL})put<uint8_t>((void*)flag,0,1);put<void*>((void*)0x181bcec68,0,curveClass);
 dependency(0x180250100,(void*)&alloc_dependency);dependency(0x1812de6c0,(void*)&ctor_dependency);dependency(0x1812de1d0,(void*)&get_dependency);dependency(0x1812de620,(void*)&set_dependency);if(mprotect(p,size,PROT_READ|PROT_EXEC))throw std::runtime_error("mprotect");
 std::cerr<<"Two unchanged original CopyAnimationCurve bodies. Allocation/empty ctor/get_keys/set_keys are recording doubles with initialized metadata and fake layouts. Original Unity curve allocator/keys engines/native throw not executed. Array pointer forwarding and allocation/ctor/get/set/return order only; CLR exercises actual candidate CIL with public curve doubles and real Unity Keyframe value arrays.\n";
 using Fn=void*(__attribute__((ms_abi))*)(void*,void*,void*);uint64_t addresses[]={0x1803b5400,0x1803ce2e0};const char*tokens[]={"0x06000771","0x06000817"};std::mt19937 rng(0x4202026);
 for(int method=0;method<2;method++)for(int k=0;k<4096;k++){int arr=rng()%3;bool hasThis=rng()&1;swapLatest=rng()&1;returnArray=arr==0?nullptr:arr==1?array1:array2;allocCalls=ctorCalls=getCalls=setCalls=0;seenArray=nullptr;ok=true;latest=nullptr;for(int i=0;i<128;i++){input[i]=(char)rng();created[i]=(char)rng();alternate[i]=(char)rng();}auto oldInput=std::vector<char>(input,input+128),oldCreated=std::vector<char>(created,created+128),oldAlt=std::vector<char>(alternate,alternate+128);void*out=((Fn)addresses[method])(hasThis?alternate:nullptr,input,nullptr);
  std::cout<<tokens[method]<<" "<<hasThis<<" "<<arr<<" "<<swapLatest<<" "<<allocCalls<<" "<<ctorCalls<<" "<<getCalls<<" "<<setCalls<<" "<<(out==created)<<" "<<(seenArray==returnArray)<<" "<<ok<<" "<<(std::memcmp(input,oldInput.data(),128)==0)<<" "<<(std::memcmp(created,oldCreated.data(),128)==0)<<" "<<(std::memcmp(alternate,oldAlt.data(),128)==0)<<"\n";
 }munmap(p,size);
}
