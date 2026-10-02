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

alignas(16)char resourceClass[512],appClass[512],pathClass[512],debugClass[512],deviceClass[512],zombieClass[512],methodDevice[32],methodZombie[32];
alignas(16)char root[32],jsons[32],devData[32],devInfo[32],zomData[32],zomInfo[32],suffix[32],parentPath[32],idText[32],fileName[32],fullPath[32],json[32],serialized[32],missingDev[32],generatedDev[32],missingZom[32],generatedZom[32],created[256],parsed[256];
int token,mode,id,formatted,logs,writes,parseCalls,constructorCalls,serializedID;uint64_t trace;bool argsOK,prettyOK;
void event(unsigned code){trace=(trace<<4)|code;}
extern "C" void* __attribute__((ms_abi)) get_root(void*){event(1);return root;}
extern "C" void* __attribute__((ms_abi)) combine4(void*a,void*b,void*c,void*d,void*){event(2);argsOK&=a==root&&b==jsons&&c==(token==0x06000225?devData:zomData)&&d==(token==0x06000225?devInfo:zomInfo);return parentPath;}
extern "C" void* __attribute__((ms_abi)) format_int(void*r,void*){event(3);formatted=*(int*)r;argsOK&=formatted==id;return idText;}
extern "C" void* __attribute__((ms_abi)) concat2(void*a,void*b,void*){event(4);argsOK&=a==idText&&b==suffix;return fileName;}
extern "C" void* __attribute__((ms_abi)) combine2(void*a,void*b,void*){event(5);argsOK&=a==parentPath&&b==fileName;return fullPath;}
extern "C" void* __attribute__((ms_abi)) read_json(void*p,void*){event(6);argsOK&=p==fullPath;return mode==0?nullptr:json;}
extern "C" bool __attribute__((ms_abi)) empty(void*p,void*){event(7);argsOK&=p==(mode==0?nullptr:json);return mode<=1;}
extern "C" void* __attribute__((ms_abi)) allocate(void*c){event(9);argsOK&=c==(token==0x06000225?deviceClass:zombieClass);return created;}
extern "C" void __attribute__((ms_abi)) ctor(void*r,void*){event(10);constructorCalls++;argsOK&=r==created;put<int>(created,token==0x06000225?0x18:0x10,-1);}
extern "C" void* __attribute__((ms_abi)) to_json(void*r,bool pretty,void*){event(11);prettyOK&=pretty;argsOK&=r==created;serializedID=*(int*)(created+(token==0x06000225?0x18:0x10));return serialized;}
extern "C" void __attribute__((ms_abi)) log_json(void*p,void*){event(logs==0?12:logs==1?13:15);argsOK&=logs==0?p==fullPath:logs==1?p==(token==0x06000225?missingDev:missingZom):p==(token==0x06000225?generatedDev:generatedZom);logs++;}
extern "C" void __attribute__((ms_abi)) write_json(void*p,void*j,void*){event(14);writes++;argsOK&=p==fullPath&&j==serialized;}
extern "C" void* __attribute__((ms_abi)) from_json(void*j,void*m){event(8);parseCalls++;argsOK&=j==json&&m==(token==0x06000225?methodDevice:methodZombie);return mode==4?nullptr:parsed;}
void dependency(uint64_t va,void*f){unsigned char bytes[12]={};bytes[0]=0x48;bytes[1]=0xb8;bytes[10]=0xff;bytes[11]=0xe0;std::memcpy(bytes+2,&f,8);std::memcpy((void*)va,bytes,12);if(std::memcmp((void*)va,bytes,12)!=0)throw std::runtime_error("trampoline bytes");}
int main(int argc,char**argv){if(argc!=2)return 2;std::ifstream f(argv[1],std::ios::binary);std::vector<char>b((std::istreambuf_iterator<char>(f)),{});auto nt=at<uint32_t>(b,60);if(at<uint32_t>(b,nt)!=0x4550||at<uint16_t>(b,nt+24)!=0x20b)throw std::runtime_error("PE64 required");auto base=at<uint64_t>(b,nt+48);auto size=at<uint32_t>(b,nt+80);auto ns=at<uint16_t>(b,nt+6),opt=at<uint16_t>(b,nt+20);auto p=mmap((void*)base,size,PROT_READ|PROT_WRITE,MAP_PRIVATE|MAP_ANONYMOUS|MAP_FIXED_NOREPLACE,-1,0);if(p==MAP_FAILED||reinterpret_cast<uint64_t>(p)!=base)throw std::runtime_error("base");std::memcpy(p,b.data(),at<uint32_t>(b,nt+84));for(int i=0;i<ns;i++){auto s=nt+24+opt+40*i;auto rva=at<uint32_t>(b,s+12),len=at<uint32_t>(b,s+16),raw=at<uint32_t>(b,s+20);if(uint64_t(raw)+len>b.size()||uint64_t(rva)+len>size)throw std::runtime_error("bounds");std::memcpy((char*)p+rva,b.data()+raw,len);}

 for(auto va:{0x181ce4c1d,0x181ce4c28,0x181ce4c0a,0x181ce4c09})put<uint8_t>((void*)va,0,1);
 put<int>(resourceClass,0xe0,1);put<int>(appClass,0xe0,1);put<int>(pathClass,0xe0,1);put<int>(debugClass,0xe0,1);
 put<void*>((void*)0x181bb6478,0,resourceClass);put<void*>((void*)0x181bcf210,0,appClass);put<void*>((void*)0x181bb18e8,0,pathClass);put<void*>((void*)0x181b9ad48,0,debugClass);put<void*>((void*)0x181b9d028,0,deviceClass);put<void*>((void*)0x181bc4678,0,zombieClass);put<void*>((void*)0x181bad1a0,0,methodDevice);put<void*>((void*)0x181bad358,0,methodZombie);
 put<void*>((void*)0x181bbc5d0,0,jsons);put<void*>((void*)0x181bbd770,0,devData);put<void*>((void*)0x181bbd820,0,devInfo);put<void*>((void*)0x181bcc9b8,0,zomData);put<void*>((void*)0x181bcca60,0,zomInfo);put<void*>((void*)0x181ba21c8,0,suffix);put<void*>((void*)0x181ba5dc8,0,missingDev);put<void*>((void*)0x181ba2b88,0,generatedDev);put<void*>((void*)0x181ba5d68,0,missingZom);put<void*>((void*)0x181ba2b28,0,generatedZom);
 dependency(0x1812df400,(void*)&get_root);dependency(0x180c84930,(void*)&combine4);dependency(0x180ca3820,(void*)&format_int);dependency(0x180b74270,(void*)&concat2);dependency(0x180c84600,(void*)&combine2);dependency(0x18031ce70,(void*)&read_json);dependency(0x180b78590,(void*)&empty);dependency(0x180250100,(void*)&allocate);dependency(0x18030f300,(void*)&ctor);dependency(0x1803251f0,(void*)&ctor);dependency(0x1813671c0,(void*)&to_json);dependency(0x1812e68f0,(void*)&log_json);dependency(0x180c820f0,(void*)&write_json);dependency(0x18047c030,(void*)&from_json);
 if(mprotect(p,size,PROT_READ|PROT_EXEC))throw std::runtime_error("mprotect");
 std::cerr<<"Two unchanged original resource-loading callers; class initialization marked complete, supplied string/class/MethodInfo identities. Path/formatter/read/emptiness/allocation/ctor/JSON/log/write dependencies recorded; nullable parse and missing-template call order/arguments observed. No original allocator/data constructor/JSON/Unity/filesystem/class-init/throw engine or iOS execution.\n";
 using Fn=void*(__attribute__((ms_abi))*)(int,void*);std::mt19937 rng(0x4802026);
 for(int t:{0x06000225,0x06000230})for(int k=0;k<4096;k++){token=t;mode=k%5;id=k==0?0:k==1?INT32_MIN:k==2?INT32_MAX:(int)rng();trace=0;formatted=logs=writes=parseCalls=constructorCalls=serializedID=0;argsOK=prettyOK=true;std::memset(created,0x55,sizeof created);std::memset(parsed,0x55,sizeof parsed);put<int>(parsed,t==0x06000225?0x18:0x10,777);void*r=((Fn)(t==0x06000225?0x1803388c0:0x18033ae30))(id,nullptr);int returnKind=r==nullptr?0:r==created?1:r==parsed?2:3;int resultID=r==nullptr?0:*(int*)((char*)r+(t==0x06000225?0x18:0x10));
  std::cout<<(t==0x06000225?"0x06000225":"0x06000230")<<" "<<(uint32_t)id<<" "<<mode<<" "<<trace<<" "<<(uint32_t)formatted<<" "<<returnKind<<" "<<(uint32_t)resultID<<" "<<logs<<" "<<writes<<" "<<parseCalls<<" "<<constructorCalls<<" "<<(uint32_t)serializedID<<" "<<argsOK<<" "<<prettyOK<<"\n";
 }
 munmap(p,size);
}
