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

alignas(16)char grid[256],drop[64],input1[64],input2[64],toggle1[64],toggle2[64],text[32],clip[256],vclass[512]={},qclass[512]={},vstatics[64],qstatics[64];
int mode,alt,step,dropValue,stringValue,inputId,toggleId,baseCalls;bool toggleValue,orderOK,addressOK,baseStateOK;
extern "C" void __attribute__((ms_abi)) set_value(void*r,int v,void*){orderOK&=++step==1&&r==drop;dropValue=v;if(mode==1){put<int>(grid,0x2c,alt);put<void*>(grid,0x48,input2);grid[0x30]^=1;}}
extern "C" void* __attribute__((ms_abi)) int_string(void*r,void*){orderOK&=++step==2;addressOK&=r==grid+0x2c;stringValue=*(int*)r;if(mode==3){put<void*>(grid,0x48,input2);*(int*)r=alt;}return text;}
extern "C" void __attribute__((ms_abi)) set_text(void*r,void*t,void*){orderOK&=++step==3&&t==text;inputId=r==input1?1:r==input2?2:0;if(mode==2){put<void*>(grid,0x50,toggle2);grid[0x30]^=1;}}
extern "C" void __attribute__((ms_abi)) set_on(void*r,bool v,void*){orderOK&=++step==4;toggleId=r==toggle1?1:r==toggle2?2:0;toggleValue=v;}
extern "C" void __attribute__((ms_abi)) base_dependency(void*r,void*){baseCalls++;baseStateOK&=r==clip&&std::memcmp(clip+0x58,vstatics,12)==0&&std::memcmp(clip+0x64,qstatics,16)==0;}
void dependency(uint64_t va,void*f){unsigned char bytes[12]={};bytes[0]=0x48;bytes[1]=0xb8;bytes[10]=0xff;bytes[11]=0xe0;std::memcpy(bytes+2,&f,8);std::memcpy((void*)va,bytes,12);if(std::memcmp((void*)va,bytes,12)!=0)throw std::runtime_error("trampoline bytes");}
int main(int argc,char**argv){if(argc!=2)return 2;std::ifstream f(argv[1],std::ios::binary);std::vector<char>b((std::istreambuf_iterator<char>(f)),{});auto nt=at<uint32_t>(b,60);if(at<uint32_t>(b,nt)!=0x4550||at<uint16_t>(b,nt+24)!=0x20b)throw std::runtime_error("PE64 required");auto base=at<uint64_t>(b,nt+48);auto size=at<uint32_t>(b,nt+80);auto ns=at<uint16_t>(b,nt+6),opt=at<uint16_t>(b,nt+20);auto p=mmap((void*)base,size,PROT_READ|PROT_WRITE,MAP_PRIVATE|MAP_ANONYMOUS|MAP_FIXED_NOREPLACE,-1,0);if(p==MAP_FAILED||reinterpret_cast<uint64_t>(p)!=base)throw std::runtime_error("base");std::memcpy(p,b.data(),at<uint32_t>(b,nt+84));for(int i=0;i<ns;i++){auto s=nt+24+opt+40*i;auto rva=at<uint32_t>(b,s+12),len=at<uint32_t>(b,s+16),raw=at<uint32_t>(b,s+20);if(uint64_t(raw)+len>b.size()||uint64_t(rva)+len>size)throw std::runtime_error("bounds");std::memcpy((char*)p+rva,b.data()+raw,len);}

 put<uint8_t>((void*)0x181ce4b36,0,1);put<uint8_t>((void*)0x181ce4faf,0,1);put<void*>((void*)0x181bc27e8,0,vclass);put<void*>((void*)0x181bb3e60,0,qclass);put<void*>(vclass,0xb8,vstatics);put<void*>(qclass,0xb8,qstatics);
 dependency(0x181180950,(void*)&set_value);dependency(0x180ca3820,(void*)&int_string);dependency(0x181197d70,(void*)&set_text);dependency(0x181573290,(void*)&set_on);dependency(0x1812e22a0,(void*)&base_dependency);if(mprotect(p,size,PROT_READ|PROT_EXEC))throw std::runtime_error("mprotect");
 std::cerr<<"Two unchanged original callers. UI setters/int ToString/base constructor recorded; exact integer field address/receiver capture/order observed. Vector3 and Quaternion static-field layouts supplied, values include arbitrary float bits; caller copy instructions unchanged. No original formatter/UI/Unity/base engine or native throw execution.\n";
 using Fn=void(__attribute__((ms_abi))*)(void*,void*);std::mt19937 rng(0x4402026);
 for(int k=0;k<4096;k++){std::memset(grid,0x55,sizeof grid);int gs=(int)rng(),pv=(int)rng();bool home=(k%2)!=0;mode=k%4;alt=(int)rng();put<int>(grid,0x28,gs);put<int>(grid,0x2c,pv);put<uint8_t>(grid,0x30,home);put<void*>(grid,0x40,drop);put<void*>(grid,0x48,input1);put<void*>(grid,0x50,toggle1);step=0;orderOK=addressOK=true;dropValue=stringValue=inputId=toggleId=0;toggleValue=false;((Fn)0x18039d5b0)(grid,nullptr);
  std::cout<<"0x0600061C "<<(uint32_t)gs<<" "<<(uint32_t)pv<<" "<<home<<" "<<mode<<" "<<(uint32_t)alt<<" "<<(uint32_t)dropValue<<" "<<(uint32_t)stringValue<<" "<<inputId<<" "<<toggleId<<" "<<toggleValue<<" "<<(*(void**)(grid+0x48)==input2?2:1)<<" "<<orderOK<<" "<<addressOK<<" "<<(uint32_t)*(int*)(grid+0x2c)<<"\n";
 }
 for(int k=0;k<4096;k++){for(int i=0;i<256;i++)clip[i]=(char)rng();for(int i=0;i<3;i++)put<uint32_t>(vstatics,4*i,k?rng():0);for(int i=0;i<4;i++)put<uint32_t>(qstatics,4*i,k?rng():(i==3?0x3f800000:0));auto before=std::vector<char>(clip,clip+256);baseCalls=0;baseStateOK=true;((Fn)0x1803bc970)(clip,nullptr);bool untouched=true;for(int i=0;i<256;i++)if(!(i>=0x58&&i<0x74)&&clip[i]!=before[i])untouched=false;
  std::cout<<"0x0600079B "<<baseCalls<<" "<<baseStateOK<<" "<<untouched;for(int i=0;i<7;i++)std::cout<<" "<<*(uint32_t*)(clip+0x58+4*i);std::cout<<"\n";
 }munmap(p,size);
}
