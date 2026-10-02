#include <sys/mman.h>
#include <cstdint>
#include <cstring>
#include <fstream>
#include <iostream>
#include <vector>
#include <deque>
#include <random>
#include <stdexcept>
template<class T>T at(const std::vector<char>&b,size_t p){T x;std::memcpy(&x,b.data()+p,sizeof x);return x;}
template<class T>void put(void*p,size_t off,T x){std::memcpy(static_cast<char*>(p)+off,&x,sizeof x);}
struct D {void* klass;void* monitor=nullptr;std::vector<int> sequence;};
std::deque<D> delegates;void* expectedClass;D** selected;std::vector<int> competing;int inject=0,attempts=0,barriers=0;
D* make(const std::vector<int>&s){if(s.empty())return nullptr;delegates.push_back({expectedClass,nullptr,s});return &delegates.back();}
void compete(){attempts++;if(inject>0){*selected=make(competing);inject--;}}
extern "C" D* __attribute__((ms_abi)) combine(D*a,D*b,void*){compete();if(!a)return b;if(!b)return a;auto s=a->sequence;s.insert(s.end(),b->sequence.begin(),b->sequence.end());return make(s);}
extern "C" D* __attribute__((ms_abi)) remove_delegate(D*a,D*b,void*){compete();if(!a||!b)return a;auto s=a->sequence;auto t=b->sequence;if(t.size()>s.size())return a;for(int i=int(s.size()-t.size());i>=0;i--){if(std::equal(t.begin(),t.end(),s.begin()+i)){s.erase(s.begin()+i,s.begin()+i+t.size());return make(s);}}return a;}
extern "C" D* __attribute__((ms_abi)) cast_delegate(D*p,void*klass){if(!p||p->klass==klass)return p;throw std::runtime_error("wrong controlled delegate type");}
extern "C" void __attribute__((ms_abi)) record_barrier(void*,void*){barriers++;}
void dependency(uint64_t va,void*f){unsigned char b[12]={0x48,0xb8,0,0,0,0,0,0,0,0,0xff,0xe0};std::memcpy(b+2,&f,8);std::memcpy((void*)va,b,12);}
using Accessor=void(__attribute__((ms_abi))*)(void*,D*);
int main(int argc,char**argv){if(argc!=2)return 2;std::ifstream f(argv[1],std::ios::binary);std::vector<char>b((std::istreambuf_iterator<char>(f)),{});auto nt=at<uint32_t>(b,60);if(at<uint32_t>(b,nt)!=0x4550||at<uint16_t>(b,nt+24)!=0x20b)throw std::runtime_error("PE64 required");auto base=at<uint64_t>(b,nt+48);auto size=at<uint32_t>(b,nt+80);auto ns=at<uint16_t>(b,nt+6),opt=at<uint16_t>(b,nt+20);auto p=mmap((void*)base,size,PROT_READ|PROT_WRITE,MAP_PRIVATE|MAP_ANONYMOUS|MAP_FIXED_NOREPLACE,-1,0);if(p==MAP_FAILED||reinterpret_cast<uint64_t>(p)!=base)throw std::runtime_error("preferred base unavailable");std::memcpy(p,b.data(),at<uint32_t>(b,nt+84));for(int i=0;i<ns;i++){auto s=nt+24+opt+40*i;auto rva=at<uint32_t>(b,s+12),len=at<uint32_t>(b,s+16),raw=at<uint32_t>(b,s+20);if(uint64_t(raw)+len>b.size()||uint64_t(rva)+len>size)throw std::runtime_error("bounds");std::memcpy((char*)p+rva,b.data()+raw,len);}
 // Generated entries below pin flags, field offsets and pointers from all 12 native bodies.
 alignas(16)char clipClass[512]={},controllerClass[512]={};put<void*>((void*)0x181ba57d0,0,clipClass);put<void*>((void*)0x181ba5830,0,controllerClass);
 struct Entry{const char*token;uint64_t va;int offset;bool clip;};
std::vector<Entry>entries={{"0x06000822",0x1803c85c0,80,true},{"0x06000823",0x1803c8a90,80,true},{"0x06000824",0x1803c8720,88,true},{"0x06000825",0x1803c8bf0,88,true},{"0x06000826",0x1803c8670,96,true},{"0x06000827",0x1803c8b40,96,true},{"0x06000858",0x1803c6f70,48,false},{"0x06000859",0x1803c71d0,48,false},{"0x0600085A",0x1803c6e10,56,false},{"0x0600085B",0x1803c7070,56,false},{"0x0600085C",0x1803c6ec0,64,false},{"0x0600085D",0x1803c7120,64,false},};
put<uint8_t>((void*)0x181ce4fbd,0,1);put<uint8_t>((void*)0x181ce4fbe,0,1);put<uint8_t>((void*)0x181ce4fbf,0,1);put<uint8_t>((void*)0x181ce4fc0,0,1);put<uint8_t>((void*)0x181ce4fc1,0,1);put<uint8_t>((void*)0x181ce4fc2,0,1);put<uint8_t>((void*)0x181ce4fd8,0,1);put<uint8_t>((void*)0x181ce4fd9,0,1);put<uint8_t>((void*)0x181ce4fda,0,1);put<uint8_t>((void*)0x181ce4fdb,0,1);put<uint8_t>((void*)0x181ce4fdc,0,1);put<uint8_t>((void*)0x181ce4fdd,0,1);
 dependency(0x180cf8bc0,(void*)&combine);dependency(0x180cfa9d0,(void*)&remove_delegate);dependency(0x18024f380,(void*)&cast_delegate);dependency(0x18025ee40,(void*)&record_barrier);
 if(mprotect(p,size,PROT_READ|PROT_EXEC))throw std::runtime_error("mprotect");
 std::cerr<<"All 12 original event accessor instructions and original CAS helper 0x180256B90 (including lock cmpxchg) unchanged. Metadata flags/Action class pointers supplied. Delegate.Combine/Remove/type-cast and GC barrier engine dependencies are controlled doubles. Forced competing updates test retry CFG, including value-equal distinct references. No Unity/GC startup/import/throw helpers; not delegate-engine behavior proof.\n";
 std::mt19937 rng(0x3602026);
 auto test=[&](Entry e,std::vector<int>a,std::vector<int>v,int forced,std::vector<int>c){delegates.clear();expectedClass=e.clip?(void*)clipClass:(void*)controllerClass;alignas(16)char obj[160]={};for(int i=0;i<160;i++)obj[i]=(char)(i*7);D* initial=make(a);D* value=make(v);put<D*>(obj,e.offset,initial);char before[160];std::memcpy(before,obj,160);selected=(D**)(obj+e.offset);competing=c;inject=forced;attempts=barriers=0;((Accessor)e.va)(obj,value);if(attempts>forced+2||barriers!=attempts)throw std::runtime_error("unexpected retry/CAS count");std::vector<int>last=*selected?(*selected)->sequence:std::vector<int>{};std::memcpy(before+e.offset,obj+e.offset,8);if(std::memcmp(before,obj,160))throw std::runtime_error("non-target field write");std::cout<<e.token<<" "<<a.size();for(int z:a)std::cout<<" "<<z;std::cout<<" "<<v.size();for(int z:v)std::cout<<" "<<z;std::cout<<" "<<forced<<" "<<c.size();for(int z:c)std::cout<<" "<<z;std::cout<<" "<<attempts<<" "<<last.size();for(int z:last)std::cout<<" "<<z;std::cout<<"\n";};
 std::vector<std::vector<int>> sequences={{},{0},{1},{0,1},{1,0},{0,0},{0,1,0},{1,0,1},{0,1,0,1}};
 for(auto e:entries){for(auto a:sequences)for(auto v:sequences){test(e,a,v,0,{1});test(e,a,v,1,a.empty()?std::vector<int>{0}:a);test(e,a,v,3,{1,0,1});}for(int i=0;i<1024;i++){std::vector<int>a(rng()%9),v(rng()%5),c(1+rng()%8);for(auto&z:a)z=rng()%4;for(auto&z:v)z=rng()%4;for(auto&z:c)z=rng()%4;test(e,a,v,rng()%4,c);}}
 munmap(p,size);
}
