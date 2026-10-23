using Mono.Cecil;
using Mono.Cecil.Cil;

namespace GodsPVZ.Actions.Verification;

public partial class Program
{
    private static void RunAdditionalTargets(ModuleDefinition module,List<Dictionary<string,object>> cases,
        List<Dictionary<string,object>> mutants,List<string> failures,Dictionary<string,bool> status,
        List<MethodDefinition> executed,List<MethodDefinition> mutantInputs)
    {
        const string dev="0x06000187",zom="0x0600026F",wave="0x060001A8";
        var md=(MethodDefinition)module.LookupToken(0x06000187);
        var mz=(MethodDefinition)module.LookupToken(0x0600026F);
        var mw=(MethodDefinition)module.LookupToken(0x060001A8);
        var fd=CandidateCilTranslator.CompileDeviceData187(md);
        var fz=CandidateCilTranslator.CompileZombieData26F(mz);
        var fw=CandidateCilTranslator.CompileFillWave1A8(mw);
        executed.AddRange(new[]{md,mz,mw});
        foreach(var tok in new[]{dev,zom,wave})status[tok]=true;
        object observed=null;
        void Case(string token,string id,object input,Func<bool> check)
        {
            TraceDisposeCollector.Clear();observed=null;bool pass=false;Exception error=null;
            try{pass=check();}catch(Exception e){error=e;}
            finally{TraceDisposeCollector.BeforeMove=null;}
            cases.Add(new(){["target"]=token,["case_id"]=id,["input"]=input,["passed"]=pass,
                ["observed"]=observed,["observed_trace"]=TraceDisposeCollector.Log.ToArray(),
                ["unexpected_exception"]=error?.ToString()});
            status[token]&=pass;if(!pass)failures.Add(token+" "+id+" "+error?.GetType().FullName);
        }
        bool Throws<T>(Action action,params string[] trace)where T:Exception
        {
            try{action();return false;}catch(Exception e)
            {observed=new{exception=e.GetType().FullName,message=e.Message};
             return e.GetType()==typeof(T)&&TraceDisposeCollector.Log.SequenceEqual(trace);}
        }
        bool Bits(float a,float b)=>BitConverter.SingleToInt32Bits(a)==BitConverter.SingleToInt32Bits(b);
        MockBoardEntry Entry(int kind,float value,bool must=true,bool selected=false,string key="7",Action hit=null)=>
            new(kind,key,must,selected){getValue=()=>{hit?.Invoke();return value;}};
        MockDeviceManager D(List<MockBoardEntry> entries=null)=>new(){board=new(),effectBoardEntries=entries??new(),
            boardEntryData=new(){magnification_Atk=1.25f,magnification_Hp=2.5f,magnification_Def=3.75f}};
        MockZombieManager Z(List<MockBoardEntry> entries=null)=>new(){board=new(),effectBoardEntries=entries??new(),
            boardEntryData=new(){magnification_Atk=1.25f,magnification_Hp=2.5f,magnification_Def=3.75f,toughness=4.5f,elementalResistance=5.5f}};
        float[] DA(MockDeviceData d)=>new[]{d.magnification_Atk,d.magnification_Hp,d.magnification_Def};
        float[] ZA(MockZombieData z)=>new[]{z.magnification_Atk,z.magnification_Hp,z.magnification_Def,z.toughness,z.elementalResistance};
        bool Equal(float[] a,float[] b)=>a.Zip(b).All(p=>Bits(p.First,p.Second))&&a.Length==b.Length;
        for(int kind=33;kind<=35;kind++)for(int flags=0;flags<4;flags++)
        {
            int k=kind,f=flags,count=0;var b=Entry(k,0.5f,(f&1)!=0,(f&2)!=0,hit:()=>count++);
            var manager=D(new(){b});var expected=new[]{1.25f,2.5f,3.75f};
            if(f!=0)expected[k==33?1:k==34?2:0]+=0.5f;
            Case(dev,$"D.flags.{k}.{f}",new{k,flags=f,expected},()=>
            {
                var r=fd(manager,7);observed=new{values=DA(r),getValueCalls=count,fresh=!ReferenceEquals(r,manager.boardEntryData)};
                return Equal(DA(r),expected)&&count==(f==0?0:1)&&!ReferenceEquals(r,manager.boardEntryData)&&
                    TraceDisposeCollector.Log.SequenceEqual(new[]{"MockBoardEntry_Disposed"});
            });
        }
        foreach(int kind in new[]{0,6,32})for(int flags=0;flags<4;flags++)
        {
            int k=kind,f=flags,count=0;var b=Entry(k,0.5f,(f&1)!=0,(f&2)!=0,hit:()=>count++);
            var manager=Z(new(){b});var expected=new[]{1.25f,2.5f,3.75f,4.5f,5.5f};
            if(f!=0){if(k==0){expected[0]+=0.5f;expected[1]+=0.5f;}else expected[k==6?3:4]+=0.5f;}
            Case(zom,$"Z.flags.{k}.{f}",new{k,flags=f,expected},()=>
            {
                var r=fz(manager,7);observed=new{values=ZA(r),getValueCalls=count};
                return Equal(ZA(r),expected)&&count==(f==0?0:k==0?2:1)&&!ReferenceEquals(r,manager.boardEntryData)&&
                    TraceDisposeCollector.Log.SequenceEqual(new[]{"MockBoardEntry_Disposed"});
            });
        }
        Case(dev,"D.ordered.float32.add",new{values=new[]{16777216f,1f,-16777216f},expectedAtk=0f},()=>
        {
            var m=D(new(){Entry(35,16777216f),Entry(35,1f),Entry(35,-16777216f)});
            m.boardEntryData.magnification_Atk=0f;var r=fd(m,7);observed=new{values=DA(r)};
            return Bits(r.magnification_Atk,0f)&&Bits(m.boardEntryData.magnification_Atk,0f);
        });
        Case(zom,"Z.separate.GetValue.calls",new{values=new[]{1f,2f},expectedAtk=2.25f,expectedHp=4.5f},()=>
        {
            int n=0;var e=new MockBoardEntry(0,"7",true){getValue=()=>++n};
            var r=fz(Z(new(){e}),7);observed=new{values=ZA(r),calls=n};
            return n==2&&Bits(r.magnification_Atk,2.25f)&&Bits(r.magnification_Hp,4.5f);
        });
        Case(dev,"D.key.and.kind.filtered",new{expectedGetValueCalls=0},()=>
        {
            int n=0;var m=D(new(){Entry(33,9,key:"07",hit:()=>n++),Entry(32,9,hit:()=>n++),Entry(35,9,key:null,hit:()=>n++)});
            var r=fd(m,7);observed=new{values=DA(r),calls=n};return n==0&&Equal(DA(r),new[]{1.25f,2.5f,3.75f});
        });
        Case(zom,"Z.key.and.kind.filtered",new{expectedGetValueCalls=0},()=>
        {
            int n=0;var m=Z(new(){Entry(0,9,key:"07",hit:()=>n++),Entry(33,9,hit:()=>n++),Entry(6,9,key:null,hit:()=>n++)});
            var r=fz(m,7);observed=new{values=ZA(r),calls=n};return n==0&&Equal(ZA(r),new[]{1.25f,2.5f,3.75f,4.5f,5.5f});
        });
        foreach(int id in new[]{int.MinValue,-7,0,int.MaxValue})
        {
            int input=id;
            Case(dev,"D.int32.key."+id,new{id,key=id.ToString()},()=>
            {var r=fd(D(new(){Entry(35,2,key:input.ToString())}),input);observed=new{values=DA(r)};return Bits(r.magnification_Atk,3.25f);});
        }
        for(int which=0;which<2;which++)
        {
            bool zombie=which==1;string tok=zombie?zom:dev;
            foreach(bool destroyed in new[]{false,true})
            {
                bool dead=destroyed;
                Case(tok,"Data.board.skips.entries."+dead,new{board=dead?"controlled destroyed":"null",entries="null"},()=>
                {
                    if(zombie){var m=Z();m.board=dead?new(){destroyed=true}:null;m.effectBoardEntries=null;var r=fz(m,7);observed=new{values=ZA(r)};return Equal(ZA(r),ZA(m.boardEntryData))&&TraceDisposeCollector.Log.Count==0;}
                    else{var m=D();m.board=dead?new(){destroyed=true}:null;m.effectBoardEntries=null;var r=fd(m,7);observed=new{values=DA(r)};return Equal(DA(r),DA(m.boardEntryData))&&TraceDisposeCollector.Log.Count==0;}
                });
            }
            Case(tok,"Data.null.manager",new{manager="null"},()=>Throws<NullReferenceException>(()=>{if(zombie)fz(null,7);else fd(null,7);}));
            Case(tok,"Data.null.data",new{data="null"},()=>Throws<NullReferenceException>(()=>
            {if(zombie){var m=Z();m.boardEntryData=null;fz(m,7);}else{var m=D();m.boardEntryData=null;fd(m,7);}}));
            Case(tok,"Data.null.entries",new{entries="null"},()=>Throws<NullReferenceException>(()=>
            {if(zombie){var m=Z();m.effectBoardEntries=null;fz(m,7);}else{var m=D();m.effectBoardEntries=null;fd(m,7);}}));
            Case(tok,"Data.null.entry.cleanup",new{entry="null"},()=>Throws<NullReferenceException>(()=>
            {if(zombie)fz(Z(new(){null}),7);else fd(D(new(){null}),7);},"MockBoardEntry_Disposed"));
            Case(tok,"Data.value.throws.cleanup",new{fault="GetValue"},()=>Throws<ArithmeticException>(()=>
            {
                var e=new MockBoardEntry(zombie?0:35,"7",true){getValue=()=>throw new ArithmeticException("controlled GetValue")};
                if(zombie)fz(Z(new(){e}),7);else fd(D(new(){e}),7);
            },"MockBoardEntry_Disposed"));
            Case(tok,"Data.collection.invalidated.cleanup",new{fault="add during GetValue"},()=>Throws<InvalidOperationException>(()=>
            {
                var entries=new List<MockBoardEntry>();entries.Add(Entry(zombie?6:35,1,hit:()=>entries.Add(Entry(999,0))));
                if(zombie)fz(Z(entries),7);else fd(D(entries),7);
            },"MockBoardEntry_Disposed"));
        }
        MockZombieSelect S(string name,int point,int minFlag=0,int minWave=0,float weight=1f)=>
            new(){label=name,point=point,minFlag=minFlag,minWave=minWave,weight=weight};
        var waveInputs=new[]{(b:3,flag:0,w:0,count:3),(b:3,flag:1,w:0,count:6),
            (b:3,flag:0,w:2,count:5),(b:3,flag:0,w:9,count:21),(b:3,flag:0,w:10,count:13),
            (b:3,flag:0,w:-1,count:2),(b:3,flag:-2,w:0,count:0),(b:0,flag:3,w:9,count:0)};
        foreach(var input in waveInputs)
        {
            Case(wave,$"W.budget.{input.b}.{input.flag}.{input.w}",new{input.b,input.flag,input.w,expected=input.count},()=>
            {
                var s=new MockEnemySelecter(){enemyPoint_Base=input.b,zombieSelects=new(){S("one",1,int.MinValue,int.MinValue)}};
                var w=new MockWave();fw(s,input.flag,input.w,w);observed=new{added=w.added.Count,calls=s.calls.ToArray()};
                return w.added.Count==input.count&&s.calls.Count==input.count*2&&TraceDisposeCollector.Log.Count==input.count;
            });
        }
        Case(wave,"W.filters.order.weights",new{expected=new[]{"a","b"},weight=1f,choose="null"},()=>
        {
            var a=S("a",2,0,0,1.25f);var b=S("b",3,0,0,-0.25f);
            var s=new MockEnemySelecter(){enemyPoint_Base=3,zombieSelects=new(){S("cost",4),S("flag",1,1),S("wave",1,0,1),a,b}};
            bool correct=false;s.choose=(pool,total)=>{correct=pool.SequenceEqual(new[]{a,b})&&Bits(total,1f);return null;};
            var w=new MockWave();fw(s,0,0,w);observed=new{calls=s.calls.ToArray(),added=w.added.Count};
            return correct&&w.added.Count==0&&TraceDisposeCollector.Log.SequenceEqual(new[]{"MockZombieSelect_Disposed"});
        });
        Case(wave,"W.duplicates.preserved",new{pool=new[]{"same","same"}},()=>
        {
            var a=S("same",1);var s=new MockEnemySelecter(){enemyPoint_Base=1,zombieSelects=new(){a,a}};
            bool correct=false;s.choose=(p,t)=>{correct=p.Count==2&&ReferenceEquals(p[0],p[1])&&Bits(t,2f);return a;};
            var w=new MockWave();fw(s,0,0,w);observed=new{calls=s.calls.ToArray()};return correct&&w.added.Count==1;
        });
        Case(wave,"W.empty.pool.no.choice",new{expectedChoiceCalls=0},()=>
        {var s=new MockEnemySelecter(){enemyPoint_Base=1};fw(s,0,0,new());observed=new{calls=s.calls.ToArray()};return s.calls.Count==0&&TraceDisposeCollector.Log.Count==1;});
        Case(wave,"W.null.collection.zero.budget",new{budget=0,collection="null"},()=>
        {var s=new MockEnemySelecter(){zombieSelects=null};fw(s,0,0,null);return TraceDisposeCollector.Log.Count==0;});
        Case(wave,"W.null.collection",new{budget=1,collection="null"},()=>Throws<NullReferenceException>(()=>
            fw(new(){enemyPoint_Base=1,zombieSelects=null},0,0,new())));
        Case(wave,"W.null.item.cleanup",new{budget=1,item="null"},()=>Throws<NullReferenceException>(()=>
            fw(new(){enemyPoint_Base=1,zombieSelects=new(){null}},0,0,new()),"MockZombieSelect_Disposed"));
        Case(wave,"W.choose.throws.after.cleanup",new{fault="choose"},()=>Throws<ArithmeticException>(()=>
            fw(new(){enemyPoint_Base=1,zombieSelects=new(){S("one",1)},choose=(p,t)=>throw new ArithmeticException()},0,0,new()),"MockZombieSelect_Disposed"));
        Case(wave,"W.add.throws.after.cleanup",new{fault="add"},()=>Throws<ArithmeticException>(()=>
            fw(new(){enemyPoint_Base=1,zombieSelects=new(){S("one",1)},add=(w,s)=>throw new ArithmeticException()},0,0,new()),"MockZombieSelect_Disposed"));
        Case(wave,"W.unchecked.int32.overflow.preserved",new{b=int.MaxValue,flag=1,wave=0,expectedBudget=-2},()=>
        {var s=new MockEnemySelecter(){enemyPoint_Base=int.MaxValue,zombieSelects=null};fw(s,1,0,null);return s.calls.Count==0&&TraceDisposeCollector.Log.Count==0;});
        Case(wave,"W.choice.mutates.point.before.subtract",new{initialPoint=1,pointAfterAdd=3,expectedAdds=1},()=>
        {
            var a=S("one",1);var s=new MockEnemySelecter(){enemyPoint_Base=3,zombieSelects=new(){a}};
            s.add=(w,v)=>{w.added.Add(v);v.point=3;};var w=new MockWave();fw(s,0,0,w);observed=new{calls=s.calls.ToArray(),added=w.added.Count};
            return w.added.Count==1;
        });
        void Mutant(MethodDefinition original,string token,string name,Action<MethodDefinition> change,Func<MethodDefinition,bool> detect)
        {
            var clone=CloneMethod(original,name);change(clone);mutantInputs.Add(clone);
            TraceDisposeCollector.Clear();observed=null;bool caught=false;Exception unexpected=null;
            try{caught=detect(clone);}catch(Exception e){unexpected=e;}
            mutants.Add(new(){["target"]=token,["mutation_id"]=name,["detected"]=caught,["observed"]=observed,
                ["observed_trace"]=TraceDisposeCollector.Log.ToArray(),["unexpected_exception"]=unexpected?.ToString()});
            status[token]&=caught;if(!caught)failures.Add(token+" escaped mutant "+name);
        }
        Mutant(md,dev,"D.hp.stores.atk",m=>
        {
            var i=m.Body.Instructions.Single(i=>i.Offset==0xCC);
            if(i.OpCode!=OpCodes.Stfld)throw new InvalidProgramException("Pinned HP store");
            i.Operand=module.LookupToken(0x040001DB);
        },m=>{var r=CandidateCilTranslator.CompileDeviceData187(m)(D(new(){Entry(33,2)}),7);observed=new{values=DA(r)};return !Bits(r.magnification_Hp,4.5f);});
        Mutant(md,dev,"D.dispose.omitted",m=>
        {var i=m.Body.Instructions.Single(i=>i.Offset==0x11B);i.OpCode=OpCodes.Pop;i.Operand=null;},
            m=>{CandidateCilTranslator.CompileDeviceData187(m)(D(),7);return TraceDisposeCollector.Log.Count==0;});
        Mutant(mz,zom,"Z.toughness.stores.def",m=>
        {var i=m.Body.Instructions.Single(i=>i.Offset==0x109);i.Operand=module.LookupToken(0x0400036C);},
            m=>{var r=CandidateCilTranslator.CompileZombieData26F(m)(Z(new(){Entry(6,2)}),7);observed=new{values=ZA(r)};return !Bits(r.toughness,6.5f);});
        Mutant(mz,zom,"Z.elemental.path.skipped",m=>
        {var i=m.Body.Instructions.Single(i=>i.Offset==0xBE);if(i.OpCode!=OpCodes.Ldc_I4)throw new InvalidProgramException("Pinned elemental kind");i.Operand=31;},
            m=>{var r=CandidateCilTranslator.CompileZombieData26F(m)(Z(new(){Entry(32,2)}),7);observed=new{values=ZA(r)};return !Bits(r.elementalResistance,7.5f);});
        Mutant(mw,wave,"W.wave.divisor.changed",m=>
        {var i=m.Body.Instructions.Single(i=>i.Offset==0x17);if(i.OpCode!=OpCodes.Ldc_I4_3)throw new InvalidProgramException("Pinned wave divisor");i.OpCode=OpCodes.Ldc_I4_2;},
            m=>{var s=new MockEnemySelecter(){enemyPoint_Base=3,zombieSelects=new(){S("one",1)}};var w=new MockWave();CandidateCilTranslator.CompileFillWave1A8(m)(s,0,2,w);observed=new{added=w.added.Count};return w.added.Count!=5;});
        Mutant(mw,wave,"W.weight.sum.subtracts",m=>
        {var i=m.Body.Instructions.Single(i=>i.Offset==0xA2);if(i.OpCode!=OpCodes.Add)throw new InvalidProgramException("Pinned weight sum");i.OpCode=OpCodes.Sub;},
            m=>{float weight=float.NaN;var s=new MockEnemySelecter(){enemyPoint_Base=1,zombieSelects=new(){S("one",1,weight:2f)},choose=(p,t)=>{weight=t;return null;}};
                CandidateCilTranslator.CompileFillWave1A8(m)(s,0,0,new());observed=new{weight};return !Bits(weight,2f);});
    }
}
