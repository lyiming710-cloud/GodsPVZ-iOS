using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Runtime.CompilerServices;

namespace GodsPVZ.Actions.Verification;
public partial class Program
{
    private static void RunV6Targets(ModuleDefinition module,List<Dictionary<string,object>> cases,
        List<Dictionary<string,object>> mutants,List<string> failures,Dictionary<string,bool> status,
        List<MethodDefinition> executed,List<MethodDefinition> mutantInputs)
    {
        const string kt="0x0600027B",et="0x060001A1",pt="0x060006BB";
        var km=(MethodDefinition)module.LookupToken(0x0600027B);
        var em=(MethodDefinition)module.LookupToken(0x060001A1);
        var pm=(MethodDefinition)module.LookupToken(0x060006BB);
        var kill=CandidateCilTranslator.CompileKillAll27B(km);
        var copy=CandidateCilTranslator.CompileEnemyCtor1A1(em);
        var prechoose=CandidateCilTranslator.CompilePrechoose6BB(pm);
        executed.AddRange(new[]{km,em,pm});foreach(var t in new[]{kt,et,pt})status[t]=true;
        object observed=null;
        void Case(string tok,string name,object input,Func<bool> check)
        {
            TraceDisposeCollector.Clear();MockEnemyPath.onConstruct=null;observed=null;
            bool pass=false;Exception unexpected=null;
            try{pass=check();}catch(Exception e){unexpected=e;}
            finally{TraceDisposeCollector.BeforeMove=null;MockEnemyPath.onConstruct=null;}
            cases.Add(new(){["target"]=tok,["case_id"]=name,["input"]=input,["passed"]=pass,
                ["observed"]=observed,["observed_trace"]=TraceDisposeCollector.Log.ToArray(),["unexpected_exception"]=unexpected?.ToString()});
            status[tok]&=pass;if(!pass)failures.Add(tok+" "+name+" "+unexpected?.GetType().FullName);
        }
        bool Trace(params string[] values)=>TraceDisposeCollector.Log.SequenceEqual(values);
        bool Throws<T>(Action action,params string[] trace)where T:Exception
        {
            try{action();return false;}catch(Exception e)
            {observed=new{exception=e.GetType().FullName,message=e.Message};return e.GetType()==typeof(T)&&Trace(trace);}
        }
        bool Bits(float a,float b)=>BitConverter.SingleToInt32Bits(a)==BitConverter.SingleToInt32Bits(b);
        bool DamageOK(MockDamage d)=>Bits(d.damagePoint,100000000f)&&d.damageAttribute==4&&d.anti_air_able&&d.anti_ground_able&&d.anti_submarine_able;
        MockZombie Zombie(string label,bool dead=false)=>new(){label=label,isDied=dead};
        object Hits(params MockZombie[] values)=>values.Select(z=>new{z.label,count=z.hits.Count,
            hits=z.hits.Select(h=>new{h.damage.damagePoint,h.damage.damageAttribute,h.damage.anti_air_able,
                h.damage.anti_ground_able,h.damage.anti_submarine_able,projectileNull=h.projectile==null}).ToArray()}).ToArray();
        Case(kt,"K.empty",new{zombies=0},()=>{kill(new(){zombieList=new()});return Trace("MockZombie_Disposed","MockZombie_Disposed");});
        Case(kt,"K.mixed.snapshot.order.damage",new{zombies=new[]{"aliveA","dead","aliveB"}},()=>
        {
            var a=Zombie("a");var b=Zombie("b");var d=Zombie("d",true);
            kill(new(){zombieList=new(){a,d,b}});observed=Hits(a,d,b);
            return a.hits.Count==1&&b.hits.Count==1&&d.hits.Count==0&&DamageOK(a.hits[0].damage)&&DamageOK(b.hits[0].damage)&&
                !ReferenceEquals(a.hits[0].damage,b.hits[0].damage)&&a.hits[0].projectile==null&&b.hits[0].projectile==null&&
                Trace("MockZombie_Disposed","hit:a","hit:b","MockZombie_Disposed");
        });
        Case(kt,"K.all.dead",new{zombies=2},()=>
        {var a=Zombie("a",true);var b=Zombie("b",true);kill(new(){zombieList=new(){a,b}});observed=Hits(a,b);
            return a.hits.Count+b.hits.Count==0&&Trace("MockZombie_Disposed","MockZombie_Disposed");});
        Case(kt,"K.duplicates.not.deduplicated",new{zombies=new[]{"same","same"}},()=>
        {var a=Zombie("a");kill(new(){zombieList=new(){a,a}});observed=Hits(a);
            return a.hits.Count==2&&!ReferenceEquals(a.hits[0].damage,a.hits[1].damage)&&Trace("MockZombie_Disposed","hit:a","hit:a","MockZombie_Disposed");});
        Case(kt,"K.callback.clears.source.not.snapshot",new{callback="first hit clears original list"},()=>
        {
            var a=Zombie("a");var b=Zombie("b");var list=new List<MockZombie>{a,b};a.onTakeDamage=(d,p)=>list.Clear();
            kill(new(){zombieList=list});observed=new{sourceCount=list.Count,hits=Hits(a,b)};
            return list.Count==0&&a.hits.Count==1&&b.hits.Count==1&&Trace("MockZombie_Disposed","hit:a","hit:b","MockZombie_Disposed");
        });
        Case(kt,"K.callback.replaces.field.and.kills.next",new{callback="replace zombieList and mark next dead"},()=>
        {
            var a=Zombie("a");var b=Zombie("b");var c=Zombie("c");var m=new MockZombieManager(){zombieList=new(){a,b}};
            a.onTakeDamage=(d,p)=>{m.zombieList=new(){c};b.isDied=true;};kill(m);observed=Hits(a,b,c);
            return b.isDied&&a.hits.Count==1&&b.hits.Count==1&&c.hits.Count==0;
        });
        Case(kt,"K.null.manager",new{manager="null"},()=>Throws<NullReferenceException>(()=>kill(null)));
        Case(kt,"K.null.list",new{list="null"},()=>Throws<NullReferenceException>(()=>kill(new())));
        Case(kt,"K.null.item.first.enumeration.cleanup",new{item="null"},()=>Throws<NullReferenceException>(()=>
            kill(new(){zombieList=new(){null}}),"MockZombie_Disposed"));
        Case(kt,"K.invalidated.source.cleanup",new{fault="source modified during MoveNext"},()=>
        {
            var m=new MockZombieManager(){zombieList=new(){Zombie("a")}};bool once=false;
            TraceDisposeCollector.BeforeMove=n=>{if(n=="MockZombie"&&!once){once=true;m.zombieList.Add(Zombie("b"));}};
            return Throws<InvalidOperationException>(()=>kill(m),"MockZombie_Disposed");
        });
        Case(kt,"K.damage.throws.second.cleanup",new{fault="TakeDamage"},()=>
        {var a=Zombie("a");a.onTakeDamage=(d,p)=>throw new ArithmeticException("controlled hit");
            return Throws<ArithmeticException>(()=>kill(new(){zombieList=new(){a,Zombie("b")}}),"MockZombie_Disposed","hit:a","MockZombie_Disposed");});

        MockEnemy Fresh()=>(MockEnemy)RuntimeHelpers.GetUninitializedObject(typeof(MockEnemy));
        MockEnemyPath Path(int x,int y,float wait,bool end)
        {var p=new MockEnemyPath(x,y,wait,null){isEnd=end};return p;}
        object EnemyState(MockEnemy e)=>new{e.id,e.row,e.gridX,e.wave,waitingTimeBits=BitConverter.SingleToInt32Bits(e.waitingTime),e.entryKey,
            positionBits=new[]{BitConverter.SingleToInt32Bits(e.position.x),BitConverter.SingleToInt32Bits(e.position.y),BitConverter.SingleToInt32Bits(e.position.z)},
            path=e.myPath?.Select(p=>p==null?null:new{p.gridX,p.gridY,waitingTimeBits=BitConverter.SingleToInt32Bits(p.waitingTime),p.isEnd,boardNull=p.receivedBoard==null}).ToArray()};
        Case(et,"E.null.copy.defaults",new{copy="null"},()=>
        {var target=Fresh();copy(target,null);observed=EnemyState(target);
            return target.entryKey==string.Empty&&target.myPath!=null&&target.myPath.Count==0&&target.id==0&&Bits(target.waitingTime,0f)&&Trace();});
        Case(et,"E.fields.and.deep.path",new{copy="fields, two path items"},()=>
        {
            var source=new MockEnemy(){id=31,row=-4,gridX=int.MaxValue,wave=9,waitingTime=-0f,entryKey="entry",
                position=new(1.25f,-2.5f,float.PositiveInfinity),myPath=new(){Path(2,7,.5f,true),Path(-8,3,-.25f,false)}};
            TraceDisposeCollector.Clear();var target=Fresh();copy(target,source);observed=EnemyState(target);
            return target.id==31&&target.row==-4&&target.gridX==int.MaxValue&&target.wave==9&&Bits(target.waitingTime,source.waitingTime)&&
                target.entryKey=="entry"&&Bits(target.position.x,source.position.x)&&Bits(target.position.y,source.position.y)&&Bits(target.position.z,source.position.z)&&
                !ReferenceEquals(target.myPath,source.myPath)&&target.myPath.Count==2&&
                !ReferenceEquals(target.myPath[0],source.myPath[0])&&target.myPath[0].gridY==7&&target.myPath[0].isEnd&&
                target.myPath[1].gridX==-8&&!target.myPath[1].isEnd&&target.myPath.All(p=>p.receivedBoard==null)&&
                Trace("path:new:2,7","path:new:-8,3","MockEnemyPath_Disposed");
        });
        Case(et,"E.duplicate.path.references.become.distinct",new{path=new[]{"same","same"}},()=>
        {
            var item=Path(2,9,3f,true);var source=new MockEnemy(){myPath=new(){item,item},entryKey=null};
            TraceDisposeCollector.Clear();var target=Fresh();copy(target,source);observed=EnemyState(target);
            return target.entryKey==null&&target.myPath.Count==2&&!ReferenceEquals(target.myPath[0],target.myPath[1])&&
                !ReferenceEquals(target.myPath[0],item)&&target.myPath.All(p=>p.isEnd);
        });
        Case(et,"E.empty.path",new{items=0},()=>
        {var source=new MockEnemy(){entryKey="x",myPath=new()};var target=Fresh();copy(target,source);observed=EnemyState(target);
            return !ReferenceEquals(source.myPath,target.myPath)&&target.myPath.Count==0&&Trace("MockEnemyPath_Disposed");});
        Case(et,"E.null.path.partial.fields",new{path="null"},()=>
        {
            var target=Fresh();bool pass=Throws<NullReferenceException>(()=>copy(target,new(){id=77,entryKey="copied",myPath=null}));
            observed=new{exceptionObserved=pass,state=EnemyState(target)};
            return pass&&target.id==77&&target.entryKey=="copied"&&target.myPath.Count==0;
        });
        Case(et,"E.null.path.item.cleanup",new{path=new[]{"null"}},()=>Throws<NullReferenceException>(()=>
            copy(Fresh(),new(){myPath=new(){null}}),"MockEnemyPath_Disposed"));
        Case(et,"E.path.constructor.throws.cleanup",new{fault="EnemyPath constructor"},()=>
        {
            var source=new MockEnemy(){myPath=new(){Path(4,8,1f,true)}};TraceDisposeCollector.Clear();
            MockEnemyPath.onConstruct=p=>throw new ArithmeticException("controlled path constructor");
            return Throws<ArithmeticException>(()=>copy(Fresh(),source),"path:new:4,8","MockEnemyPath_Disposed");
        });
        Case(et,"E.path.constructor.callback.changes.isEnd",new{initialEnd=false,afterCtor=true},()=>
        {
            var item=Path(4,8,1f,false);var source=new MockEnemy(){myPath=new(){item}};TraceDisposeCollector.Clear();
            MockEnemyPath.onConstruct=p=>item.isEnd=true;var target=Fresh();copy(target,source);observed=EnemyState(target);
            return target.myPath.Single().isEnd;
        });
        Case(et,"E.path.constructor.invalidates.source",new{fault="source list modified in constructor"},()=>
        {
            var source=new MockEnemy(){myPath=new(){Path(4,8,1f,false)}};TraceDisposeCollector.Clear();
            MockEnemyPath.onConstruct=p=>source.myPath.Add(null);
            return Throws<InvalidOperationException>(()=>copy(Fresh(),source),"path:new:4,8","MockEnemyPath_Disposed");
        });
        Case(et,"E.target.null",new{target="null"},()=>Throws<NullReferenceException>(()=>copy(null,null)));
        foreach(int pattern in new[]{unchecked((int)0x80000000),0x7FC12345,0x7F800000})
        {
            int bits=pattern;
            Case(et,"E.float.bits."+bits,new{float32_bits=bits},()=>
            {
                float value=BitConverter.Int32BitsToSingle(bits);
                var source=new MockEnemy(){waitingTime=value,position=new(value,value,value),myPath=new(){Path(1,2,value,true)}};
                TraceDisposeCollector.Clear();var target=Fresh();copy(target,source);observed=EnemyState(target);
                return Bits(target.waitingTime,value)&&Bits(target.position.x,value)&&Bits(target.position.y,value)&&
                    Bits(target.position.z,value)&&Bits(target.myPath[0].waitingTime,value);
            });
        }
        Case(et,"E.callback.replaces.source.path.field",new{callback="replace source myPath after first path allocation"},()=>
        {
            var source=new MockEnemy(){myPath=new(){Path(1,2,1f,true),Path(3,4,2f,false)}};TraceDisposeCollector.Clear();
            MockEnemyPath.onConstruct=p=>source.myPath=new();var target=Fresh();copy(target,source);observed=EnemyState(target);
            return source.myPath.Count==0&&target.myPath.Count==2&&target.myPath[1].gridX==3;
        });
        Case(et,"E.callback.replaces.target.path.field",new{callback="replace target myPath before first append"},()=>
        {
            var source=new MockEnemy(){myPath=new(){Path(1,2,1f,true),Path(3,4,2f,false)}};
            TraceDisposeCollector.Clear();var target=Fresh();var replacement=new List<MockEnemyPath>();bool once=false;
            MockEnemyPath.onConstruct=p=>{if(!once){once=true;target.myPath=replacement;}};
            copy(target,source);observed=EnemyState(target);return ReferenceEquals(target.myPath,replacement)&&replacement.Count==2;
        });
        for(int n=0;n<12;n++)
        {
            int value=n;
            Case(et,"E.numeric.fields."+n,new{id=n,x=-n,row=n*3,wave=n*7},()=>
            {
                var src=new MockEnemy(){id=value,gridX=-value,row=value*3,wave=value*7,waitingTime=value/8f,myPath=new(),position=new(value,-value,value*.5f)};
                var dst=Fresh();copy(dst,src);observed=EnemyState(dst);
                return dst.id==src.id&&dst.gridX==src.gridX&&dst.row==src.row&&dst.wave==src.wave&&Bits(dst.waitingTime,src.waitingTime)&&
                    Bits(dst.position.x,src.position.x)&&Bits(dst.position.y,src.position.y)&&Bits(dst.position.z,src.position.z);
            });
        }
        MockCardChoose Card(string label,int id,bool chosen=false)=>new(){label=label,plantID=id,isChosen=chosen};
        MockSeedChooser Screen(params MockCardChoose[] cards)=>new(){card_ChooseList=cards.ToList(),plantDetail=new()};
        Case(pt,"P.empty.lists",new{cards=0,requests=0},()=>
        {prechoose(Screen(),new());return Trace("MockCardChoose_Disposed","detail:null","MockCardChoose_Disposed");});
        Case(pt,"P.reset.chosen.only",new{chosen=new[]{true,false}},()=>
        {var a=Card("a",1,true);var b=Card("b",2);prechoose(Screen(a,b),new());observed=new{a.isChosen,bChosen=b.isChosen};
            return !a.isChosen&&!b.isChosen&&Trace("reset:a","MockCardChoose_Disposed","detail:null","MockCardChoose_Disposed");});
        Case(pt,"P.matches.all.duplicates.in.order",new{cards=new[]{1,2,1},requests=new[]{1,1}},()=>
        {
            var a=Card("a",1);var b=Card("b",2);var c=Card("c",1);var s=Screen(a,b,c);
            prechoose(s,new(){Card("request1",1),Card("request2",1)});observed=new{a.isChosen,bChosen=b.isChosen,cChosen=c.isChosen,detail=s.plantDetail.selected?.label};
            return a.isChosen&&!b.isChosen&&c.isChosen&&s.plantDetail.selected==c&&
                Trace("MockCardChoose_Disposed","detail:null","choose:a","detail:a","choose:c","detail:c","MockCardChoose_Disposed",
                    "choose:a","detail:a","choose:c","detail:c","MockCardChoose_Disposed","MockCardChoose_Disposed");
        });
        Case(pt,"P.nonmatch.no.choose",new{cards=new[]{1},requests=new[]{99}},()=>
        {var a=Card("a",1);prechoose(Screen(a),new(){Card("r",99)});return !a.isChosen&&Trace("MockCardChoose_Disposed","detail:null","MockCardChoose_Disposed","MockCardChoose_Disposed");});
        Case(pt,"P.null.screen",new{screen="null"},()=>Throws<NullReferenceException>(()=>prechoose(null,new())));
        Case(pt,"P.null.cards",new{cards="null"},()=>Throws<NullReferenceException>(()=>prechoose(new(){card_ChooseList=null},new())));
        Case(pt,"P.null.card.reset.cleanup",new{cards=new[]{"null"}},()=>Throws<NullReferenceException>(()=>prechoose(Screen(new MockCardChoose[]{null}),new()),"MockCardChoose_Disposed"));
        Case(pt,"P.reset.throws.cleanup",new{fault="Choose_Reset"},()=>
        {
            var a=Card("a",1,true);a.onReset=()=>throw new ArithmeticException();
            return Throws<ArithmeticException>(()=>prechoose(Screen(a),new()),"reset:a","MockCardChoose_Disposed");
        });
        Case(pt,"P.reset.invalidates.cards.cleanup",new{fault="list modified in reset"},()=>
        {
            var a=Card("a",1,true);var s=Screen(a);a.onReset=()=>s.card_ChooseList.Add(Card("b",2));
            return Throws<InvalidOperationException>(()=>prechoose(s,new()),"reset:a","MockCardChoose_Disposed");
        });
        Case(pt,"P.null.detail.after.reset.cleanup",new{detail="null"},()=>Throws<NullReferenceException>(()=>
            prechoose(new(){card_ChooseList=new(),plantDetail=null},new()),"MockCardChoose_Disposed"));
        Case(pt,"P.detail.null.call.throws",new{fault="Awake(null)"},()=>
        {
            var s=Screen();s.plantDetail.onAwake=c=>throw new ArithmeticException();
            return Throws<ArithmeticException>(()=>prechoose(s,new()),"MockCardChoose_Disposed","detail:null");
        });
        Case(pt,"P.null.request.list.after.initial.detail",new{requests="null"},()=>Throws<NullReferenceException>(()=>
            prechoose(Screen(),null),"MockCardChoose_Disposed","detail:null"));
        Case(pt,"P.null.request.with.no.cards.does.not.dereference",new{cards=0,requests=new[]{"null"}},()=>
        {prechoose(Screen(),new(){null});return Trace("MockCardChoose_Disposed","detail:null","MockCardChoose_Disposed","MockCardChoose_Disposed");});
        Case(pt,"P.null.request.with.cards.two.cleanups",new{cards=1,requests=new[]{"null"}},()=>Throws<NullReferenceException>(()=>
            prechoose(Screen(Card("a",1)),new(){null}),"MockCardChoose_Disposed","detail:null","MockCardChoose_Disposed","MockCardChoose_Disposed"));
        Case(pt,"P.choose.throws.two.cleanups",new{fault="Choose_Being"},()=>
        {
            var a=Card("a",1);a.onChoose=()=>throw new ArithmeticException();
            return Throws<ArithmeticException>(()=>prechoose(Screen(a),new(){Card("r",1)}),"MockCardChoose_Disposed","detail:null","choose:a","MockCardChoose_Disposed","MockCardChoose_Disposed");
        });
        Case(pt,"P.choose.replaces.detail.receiver",new{callback="replace plantDetail during Choose_Being"},()=>
        {
            var a=Card("a",1);var s=Screen(a);var old=s.plantDetail;var replacement=new MockPlantDetail();
            a.onChoose=()=>s.plantDetail=replacement;prechoose(s,new(){Card("r",1)});
            observed=new{oldSelected=old.selected?.label,newSelected=replacement.selected?.label};
            return old.selected==null&&replacement.selected==a;
        });
        Case(pt,"P.initial.detail.replaces.cards",new{callback="Awake(null) replaces card_ChooseList"},()=>
        {
            var old=Card("old",1);var newer=Card("new",1);var s=Screen(old);
            s.plantDetail.onAwake=c=>{if(c==null)s.card_ChooseList=new(){newer};};
            prechoose(s,new(){Card("r",1)});observed=new{oldChosen=old.isChosen,newChosen=newer.isChosen};
            return !old.isChosen&&newer.isChosen&&Trace("MockCardChoose_Disposed","detail:null","choose:new","detail:new","MockCardChoose_Disposed","MockCardChoose_Disposed");
        });
        Case(pt,"P.choose.invalidates.request.two.cleanups",new{fault="Choose_Being appends request"},()=>
        {
            var a=Card("a",1);var requests=new List<MockCardChoose>{Card("r",1)};a.onChoose=()=>requests.Add(Card("extra",2));
            return Throws<InvalidOperationException>(()=>prechoose(Screen(a),requests),"MockCardChoose_Disposed","detail:null","choose:a","detail:a","MockCardChoose_Disposed","MockCardChoose_Disposed");
        });
        Case(pt,"P.request.aliases.cards",new{requests="same List as cards"},()=>
        {
            var a=Card("a",1,true);var b=Card("b",2);var s=Screen(a,b);prechoose(s,s.card_ChooseList);
            observed=new{a.isChosen,bChosen=b.isChosen};return a.isChosen&&b.isChosen&&s.plantDetail.selected==b;
        });
        Case(pt,"P.choose.replaces.cards.next.request.reloads",new{callback="replace cards in first choice",requests=2},()=>
        {
            var a=Card("a",1);var b=Card("b",1);var c=Card("c",1);var s=Screen(a,b);
            a.onChoose=()=>s.card_ChooseList=new(){c};prechoose(s,new(){Card("r1",1),Card("r2",1)});
            observed=new{a.isChosen,bChosen=b.isChosen,cChosen=c.isChosen,detail=s.plantDetail.selected?.label};
            return a.isChosen&&b.isChosen&&c.isChosen&&s.plantDetail.selected==c&&
                Trace("MockCardChoose_Disposed","detail:null","choose:a","detail:a","choose:b","detail:b","MockCardChoose_Disposed",
                    "choose:c","detail:c","MockCardChoose_Disposed","MockCardChoose_Disposed");
        });
        Case(pt,"P.detail.invalidates.inner.cards",new{fault="Awake(card) modifies card list"},()=>
        {
            var a=Card("a",1);var s=Screen(a);s.plantDetail.onAwake=c=>{if(c!=null)s.card_ChooseList.Add(Card("b",2));};
            return Throws<InvalidOperationException>(()=>prechoose(s,new(){Card("r",1)}),
                "MockCardChoose_Disposed","detail:null","choose:a","detail:a","MockCardChoose_Disposed","MockCardChoose_Disposed");
        });
        Case(pt,"P.choose.nulls.detail.two.cleanups",new{fault="Choose_Being nulls plantDetail"},()=>
        {
            var a=Card("a",1);var s=Screen(a);a.onChoose=()=>s.plantDetail=null;
            return Throws<NullReferenceException>(()=>prechoose(s,new(){Card("r",1)}),
                "MockCardChoose_Disposed","detail:null","choose:a","MockCardChoose_Disposed","MockCardChoose_Disposed");
        });
        void Mutant(MethodDefinition original,string tok,string name,Action<MethodDefinition> change,Func<MethodDefinition,bool> check)
        {
            var clone=CloneMethod(original,name);change(clone);mutantInputs.Add(clone);TraceDisposeCollector.Clear();
            MockEnemyPath.onConstruct=null;observed=null;bool detected=false;Exception unexpected=null;
            try{detected=check(clone);}catch(Exception e){unexpected=e;}
            finally{TraceDisposeCollector.BeforeMove=null;MockEnemyPath.onConstruct=null;}
            mutants.Add(new(){["target"]=tok,["mutation_id"]=name,["detected"]=detected,["observed"]=observed,
                ["observed_trace"]=TraceDisposeCollector.Log.ToArray(),["unexpected_exception"]=unexpected?.ToString()});
            status[tok]&=detected;if(!detected)failures.Add(tok+" escaped mutant "+name);
        }
        Instruction At(MethodDefinition m,int offset,Mono.Cecil.Cil.OpCode expected)
        {var i=m.Body.Instructions.Single(i=>i.Offset==offset);if(i.OpCode!=expected)throw new InvalidProgramException("Pinned v6 mutant site");return i;}
        Mutant(km,kt,"K.alive.filter.inverted",m=>At(m,0x25,OpCodes.Brtrue).OpCode=OpCodes.Brfalse,m=>
        {var a=Zombie("a");var d=Zombie("d",true);CandidateCilTranslator.CompileKillAll27B(m)(new(){zombieList=new(){a,d}});observed=Hits(a,d);return a.hits.Count!=1||d.hits.Count!=0;});
        Mutant(km,kt,"K.damage.point.changed",m=>At(m,0x65,OpCodes.Ldc_R4).Operand=1f,m=>
        {var a=Zombie("a");CandidateCilTranslator.CompileKillAll27B(m)(new(){zombieList=new(){a}});observed=Hits(a);return !DamageOK(a.hits.Single().damage);});
        Mutant(km,kt,"K.ground.flag.disabled",m=>At(m,0x7E,OpCodes.Ldc_I4_1).OpCode=OpCodes.Ldc_I4_0,m=>
        {var a=Zombie("a");CandidateCilTranslator.CompileKillAll27B(m)(new(){zombieList=new(){a}});observed=Hits(a);return !DamageOK(a.hits.Single().damage);});
        Mutant(km,kt,"K.second.dispose.omitted",m=>{var i=At(m,0xA6,OpCodes.Call);i.OpCode=OpCodes.Pop;i.Operand=null;},m=>
        {CandidateCilTranslator.CompileKillAll27B(m)(new(){zombieList=new()});return !Trace("MockZombie_Disposed","MockZombie_Disposed");});
        Mutant(em,et,"E.wave.stored.in.id",m=>At(m,0x59,OpCodes.Stfld).Operand=module.LookupToken(0x040001F7),m=>
        {var target=Fresh();CandidateCilTranslator.CompileEnemyCtor1A1(m)(target,new(){id=7,wave=9,myPath=new()});observed=EnemyState(target);return target.id!=7||target.wave!=9;});
        Mutant(em,et,"E.path.y.reads.x",m=>At(m,0xA1,OpCodes.Ldfld).Operand=module.LookupToken(0x040003A5),m=>
        {var src=new MockEnemy(){myPath=new(){Path(2,7,1f,true)}};TraceDisposeCollector.Clear();var target=Fresh();
            CandidateCilTranslator.CompileEnemyCtor1A1(m)(target,src);observed=EnemyState(target);return target.myPath[0].gridY!=7;});
        Mutant(em,et,"E.dispose.omitted",m=>{var i=At(m,0xDE,OpCodes.Call);i.OpCode=OpCodes.Pop;i.Operand=null;},m=>
        {CandidateCilTranslator.CompileEnemyCtor1A1(m)(Fresh(),new(){myPath=new()});return !Trace("MockEnemyPath_Disposed");});
        Mutant(pm,pt,"P.match.inverted",m=>At(m,0x88,OpCodes.Bne_Un).OpCode=OpCodes.Beq,m=>
        {var a=Card("a",1);var b=Card("b",2);CandidateCilTranslator.CompilePrechoose6BB(m)(Screen(a,b),new(){Card("r",1)});
            observed=new{a.isChosen,bChosen=b.isChosen};return !a.isChosen||b.isChosen;});
        Mutant(pm,pt,"P.inner.dispose.omitted",m=>{var i=At(m,0xB2,OpCodes.Call);i.OpCode=OpCodes.Pop;i.Operand=null;},m=>
        {CandidateCilTranslator.CompilePrechoose6BB(m)(Screen(),new(){Card("r",1)});return TraceDisposeCollector.Log.Count(x=>x=="MockCardChoose_Disposed")!=3;});
        Mutant(pm,pt,"P.choose.call.omitted",m=>{var i=At(m,0x8E,OpCodes.Callvirt);i.OpCode=OpCodes.Pop;i.Operand=null;},m=>
        {var a=Card("a",1);CandidateCilTranslator.CompilePrechoose6BB(m)(Screen(a),new(){Card("r",1)});observed=new{a.isChosen};return !a.isChosen;});
        Mutant(pm,pt,"P.reset.filter.inverted",m=>At(m,0x1F,OpCodes.Brfalse).OpCode=OpCodes.Brtrue,m=>
        {var a=Card("a",1,true);var b=Card("b",2);CandidateCilTranslator.CompilePrechoose6BB(m)(Screen(a,b),new());
            observed=new{a.isChosen,bChosen=b.isChosen};return a.isChosen||!TraceDisposeCollector.Log.Contains("reset:a");});
    }
}
