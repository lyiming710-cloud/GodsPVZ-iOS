#include <sys/mman.h>
#include <cstdint>
#include <cstring>
#include <fstream>
#include <iostream>
#include <vector>
#include <string>
#include <stdexcept>
#include <csetjmp>
template<class T>T at(const std::vector<char>&b,size_t p){T x;std::memcpy(&x,b.data()+p,sizeof x);return x;}
template<class T>void put(void*p,size_t off,T x){std::memcpy(static_cast<char*>(p)+off,&x,sizeof x);}
template<class T>T read(void*p,size_t off){T x;std::memcpy(&x,(char*)p+off,sizeof x);return x;}
alignas(16)char managers[3][96],altManager[96],home[64],board[512],lists[5][64],alternates[5][64],objects[5][8][32],methods[5][32],arrays[3][64],saveA[256],saveB[256],saves[64],lawn[64],globalClass[512],globalFields[64],objectClass[512],game[32];
void*itemsA[5][8],*itemsB[5][8];int mode,hook,errorKind,caseNo,token;bool argsOK;std::string trace;std::jmp_buf jump;
void ev(uint32_t x){trace+=std::to_string(x)+",";}
void setList(int t,void*p){if(t<3)put<void*>(managers[t],t==2?0x38:0x30,p);else put<void*>(home,0x20,p);}
void change(int t){if(hook==1){if(t<3){auto l=read<void*>(managers[t],t==2?0x38:0x30);if(l){int n=read<int>(l,0x18);put<int>(l,0x18,n>0?n-1:0);}}else setList(t,alternates[t]);}
 if(hook==2)setList(t,nullptr);if(hook==3){setList(t,alternates[t]);put<int>(alternates[t],0x18,0);}if(t>=3){if(hook==4)put<void*>(saveA,t==3?0x88:0x80,arrays[2]);if(hook==5)put<void*>(saves,0x10,nullptr);if(hook==6)put<void*>(saves,0x10,saveB);if(hook==7)put<void*>(saveA,t==3?0x88:0x80,nullptr);}
 else if(token==0x060002AA){if(t==1&&hook==5)put<void*>(board,0xf0,nullptr);if(t==2&&hook==6)put<void*>(board,0xf0,altManager);if(t==2&&hook==7)put<void*>(board,0x108,nullptr);}}
extern "C" void __attribute__((ms_abi)) nre(){ev(8);errorKind=1;std::longjmp(jump,1);}
extern "C" void __attribute__((ms_abi)) bounds(){ev(9);errorKind=2;std::longjmp(jump,1);}
extern "C" void* __attribute__((ms_abi)) get_item(void*r,int ix,void*m){int t=-1;for(int j=0;j<5;j++)if(m==methods[j])t=j;if(t<0)throw std::runtime_error("MI");bool alt=r==alternates[t];argsOK&=alt||r==lists[t];ev(1);ev(t);ev(ix);ev(alt);if(mode==12){ev(10);errorKind=3;std::longjmp(jump,1);}int count=read<int>(r,0x18);if((unsigned)ix>=(unsigned)count)bounds();return alt?itemsB[t][ix]:itemsA[t][ix];}
void destroy(void*r,int t){ev(5);ev(t);ev(read<int>(r,0));if(mode==14){ev(10);errorKind=3;std::longjmp(jump,1);}change(t);}
extern "C" void __attribute__((ms_abi)) device_destroy(void*r,void*){destroy(r,0);}
extern "C" void __attribute__((ms_abi)) plant_destroy(void*r,void*){destroy(r,1);}
extern "C" void __attribute__((ms_abi)) zombie_destroy(void*r,void*){destroy(r,2);}
void state(void*r,bool v,int t){ev(6);ev(t);ev(read<int>(r,0));ev(v);if(mode==15){ev(10);errorKind=3;std::longjmp(jump,1);}change(t);}
extern "C" void __attribute__((ms_abi)) device_state(void*r,bool v,void*){state(r,v,3);}
extern "C" void __attribute__((ms_abi)) zombie_state(void*r,bool v,void*){state(r,v,4);}
extern "C" void* __attribute__((ms_abi)) get_game(void*r,void*){ev(2);argsOK&=r==(token==0x060002AA?(void*)board:(void*)home);return mode==8?nullptr:game;}
extern "C" void __attribute__((ms_abi)) set_active(void*r,bool v,void*){ev(3);ev(v);argsOK&=r==game&&v;if(mode==13)put<void*>(saves,0x10,saveB);}
extern "C" void __attribute__((ms_abi)) destroy_game(void*r,void*){ev(7);ev(r==nullptr);argsOK&=r==game||r==nullptr;}
extern "C" void __attribute__((ms_abi)) check_device(void*r,int id,void*){ev(4);ev(3);ev(id);argsOK&=r==home;}
extern "C" void __attribute__((ms_abi)) check_zombie(void*r,int id,void*){ev(4);ev(4);ev(id);argsOK&=r==home;}
void dependency(uint64_t va,void*f){unsigned char bytes[12]={};bytes[0]=0x48;bytes[1]=0xb8;bytes[10]=0xff;bytes[11]=0xe0;std::memcpy(bytes+2,&f,8);std::memcpy((void*)va,bytes,12);if(std::memcmp((void*)va,bytes,12))throw std::runtime_error("trampoline");}
int main(int argc,char**argv){if(argc!=2)return 2;std::ifstream f(argv[1],std::ios::binary);std::vector<char>b((std::istreambuf_iterator<char>(f)),{});auto nt=at<uint32_t>(b,60);if(at<uint32_t>(b,nt)!=0x4550||at<uint16_t>(b,nt+24)!=0x20b)throw std::runtime_error("PE64 required");auto base=at<uint64_t>(b,nt+48);auto size=at<uint32_t>(b,nt+80);auto ns=at<uint16_t>(b,nt+6),opt=at<uint16_t>(b,nt+20);auto p=mmap((void*)base,size,PROT_READ|PROT_WRITE,MAP_PRIVATE|MAP_ANONYMOUS|MAP_FIXED_NOREPLACE,-1,0);if(p==MAP_FAILED||reinterpret_cast<uint64_t>(p)!=base)throw std::runtime_error("base");std::memcpy(p,b.data(),at<uint32_t>(b,nt+84));for(int i=0;i<ns;i++){auto s=nt+24+opt+40*i;auto rva=at<uint32_t>(b,s+12),len=at<uint32_t>(b,s+16),raw=at<uint32_t>(b,s+20);if(uint64_t(raw)+len>b.size()||uint64_t(rva)+len>size)throw std::runtime_error("bounds");std::memcpy((char*)p+rva,b.data()+raw,len);}




 std::vector<std::pair<uint64_t,std::vector<char>>> callerBytes;callerBytes.push_back({0x180312460,std::vector<char>((char*)0x180312460, (char*)0x1803124e2)});callerBytes.push_back({0x180322660,std::vector<char>((char*)0x180322660, (char*)0x1803226e2)});callerBytes.push_back({0x180341900,std::vector<char>((char*)0x180341900, (char*)0x180341982)});callerBytes.push_back({0x18038a320,std::vector<char>((char*)0x18038a320, (char*)0x18038a464)});callerBytes.push_back({0x18038a470,std::vector<char>((char*)0x18038a470, (char*)0x18038a57b)});callerBytes.push_back({0x180393820,std::vector<char>((char*)0x180393820, (char*)0x180393964)});callerBytes.push_back({0x180393970,std::vector<char>((char*)0x180393970, (char*)0x180393a7b)});callerBytes.push_back({0x180327410,std::vector<char>((char*)0x180327410, (char*)0x180327522)});
 for(auto va:{0x181ce4b92ULL,0x181ce4be6ULL,0x181ce4c5eULL,0x181ce4c81ULL,0x181ce4e33ULL,0x181ce4e65ULL})put<uint8_t>((void*)va,0,1);
 uint64_t slots[]={0x181ba91c8,0x181bb4418,0x181bc3848,0x181ba3978,0x181ba3b58};for(int t=0;t<5;t++)put<void*>((void*)slots[t],0,methods[t]);put<int>(globalClass,0xe0,1);put<void*>(globalClass,0xb8,globalFields);put<void*>((void*)0x181ba2018,0,globalClass);put<int>(objectClass,0xe0,1);put<void*>((void*)0x181bb0500,0,objectClass);
 dependency(0x180825710,(void*)&get_item);dependency(0x180348830,(void*)&device_destroy);dependency(0x18034e540,(void*)&plant_destroy);dependency(0x18035e470,(void*)&zombie_destroy);dependency(0x18038a700,(void*)&device_state);dependency(0x180394160,(void*)&zombie_state);dependency(0x1813044d0,(void*)&get_game);dependency(0x18131bad0,(void*)&set_active);dependency(0x18131e940,(void*)&destroy_game);dependency(0x18038a000,(void*)&check_device);dependency(0x1803936b0,(void*)&check_zombie);dependency(0x180250150,(void*)&nre);dependency(0x180250140,(void*)&bounds);
 for(const auto& record:callerBytes)if(std::memcmp((void*)record.first,record.second.data(),record.second.size()))throw std::runtime_error("Original caller changed");
 if(mprotect(p,size,PROT_READ|PROT_EXEC))throw std::runtime_error("mprotect");std::cerr<<"Eight unchanged original callers, including original nested ClearPlants/ClearDevices in Board.Quit. Ready metadata/class init; recorded get_Item/destroy/SetState/Unity/check/exception dependencies. Native list counts, array reads, loop gates and object rereads retained. No original engine/dependency implementation/throw engine or iOS proof.\n";
 uint64_t starts[]={0x180312460,0x180322660,0x180341900,0x18038a320,0x18038a470,0x180393820,0x180393970,0x180327410};int tokens[]={0x06000182,0x060001EF,0x06000279,0x06000560,0x06000563,0x060005AF,0x060005B0,0x060002AA};
 using Fn=void(__attribute__((ms_abi))*)(void*,int,void*);
 for(int target=0;target<8;target++)for(int k=0;k<2048;k++){token=tokens[target];caseNo=k;mode=k%16;hook=(k/16)%8;int count=(k/128)%5;for(int t=0;t<5;t++){put<int>(lists[t],0x18,count);put<int>(alternates[t],0x18,3);for(int j=0;j<8;j++){put<int>(objects[t][j],0,10*t+j+1);itemsA[t][j]=(mode==2||mode==3)&&j==0?nullptr:objects[t][j];itemsB[t][j]=objects[t][7-j];}if(t<3)setList(t,mode==1?nullptr:lists[t]);}
 int ht=target<5?3:4;put<void*>(home,0x20,mode==1?nullptr:lists[ht]);for(int a=0;a<3;a++){put<int>(arrays[a],0x18,a==2?2:mode==3?0:8);for(int j=0;j<8;j++)put<uint8_t>(arrays[a],0x20+j,((k+j+a)%2)!=0);}for(int t=3;t<5;t++){put<void*>(saveA,t==3?0x88:0x80,mode==4?nullptr:arrays[0]);put<void*>(saveB,t==3?0x88:0x80,arrays[1]);}
 put<void*>(saves,0x10,mode==7?nullptr:saveA);put<void*>(lawn,0x28,mode==6?nullptr:saves);put<void*>(globalFields,0,mode==5?nullptr:lawn);
 put<void*>(board,0xe8,mode==9?nullptr:managers[1]);put<void*>(board,0xf0,mode==10?nullptr:managers[2]);put<void*>(board,0x108,mode==11?nullptr:managers[0]);put<void*>(altManager,0x38,alternates[2]);trace="";errorKind=0;argsOK=true;int id=k==0?INT32_MIN:k==1?INT32_MAX:k*7919;
 if(setjmp(jump)==0)((Fn)starts[target])(target<3?(void*)managers[target]:target==7?(void*)board:(void*)home,id,nullptr);
 std::cout<<"0x"<<std::hex<<token<<std::dec<<" "<<k<<" "<<(uint32_t)id<<" "<<errorKind<<" "<<argsOK<<" "<<(trace.empty()?"-":trace)<<"\n";
 }munmap(p,size);
}
