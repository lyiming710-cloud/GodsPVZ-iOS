using Mono.Cecil;
using Mono.Cecil.Cil;
namespace GodsPVZ.Actions.Verification;
public partial class Program
{
    private static void RunV7Targets(ModuleDefinition module,List<Dictionary<string,object>> cases,
        List<Dictionary<string,object>> mutants,List<string> failures,Dictionary<string,bool> status,
        List<MethodDefinition> executed,List<MethodDefinition> mutantInputs)
    {
        const string lt="0x060006B6",rt="0x060002F5",gt="0x06000297";
        var lm=(MethodDefinition)module.LookupToken(0x060006B6);var rm=(MethodDefinition)module.LookupToken(0x060002F5);var gm=(MethodDefinition)module.LookupToken(0x06000297);
        var load=CandidateCilTranslator.CompileLoadTeam6B6(lm);var reset=CandidateCilTranslator.CompileReset2F5(rm);var place=CandidateCilTranslator.CompileCanPlacing297(gm);
        executed.AddRange(new[]{lm,rm,gm});foreach(var t in new[]{lt,rt,gt})status[t]=true;
        object observed=null;
        void Case(string t,string id,object input,Func<bool> test)
        {
            TraceDisposeCollector.Clear();observed=null;bool pass=false;Exception error=null;
            try{pass=test();}catch(Exception e){error=e;}finally{TraceDisposeCollector.BeforeMove=null;}
            cases.Add(new(){["target"]=t,["case_id"]=id,["input"]=input,["passed"]=pass,["observed"]=observed,
                ["observed_trace"]=TraceDisposeCollector.Log.ToArray(),["unexpected_exception"]=error?.ToString()});
            status[t]&=pass;if(!pass)failures.Add(t+" "+id+" "+error?.GetType().FullName);
        }
        bool Throws<T>(Action action,int disposals=0)where T:Exception
        {
            try{action();return false;}catch(Exception e)
            {observed=new{exception=e.GetType().FullName,message=e.Message};return e.GetType()==typeof(T)&&TraceDisposeCollector.Log.Count(x=>x=="MockCardChoose_Disposed")==disposals;}
        }
        bool Bits(float a,float b)=>BitConverter.SingleToInt32Bits(a)==BitConverter.SingleToInt32Bits(b);
        MockCardChoose C(string name,int id,bool chosen=false)=>new(){label=name,plantID=id,isChosen=chosen,plantSave=new(id)};
        MockSeedChooser Screen(params MockCardChoose[] cards)=>new(){card_ChooseList=cards.ToList(),plantDetail=new()};
        MockTeam Team(int[] ids,int[] skills)=>new(){ID=ids?.ToList(),skill=skills?.ToList()};
        object Cards(params MockCardChoose[] cards)=>cards.Select(c=>new{c.label,c.isChosen,c.serialNumBeChosen,skill=c.currentSkill?.id,text=c.serialNumBeChosenText?.text}).ToArray();
        Case(lt,"L.empty.ids.resets.all",new{chosen=new[]{true,false},ids=0},()=>
        {var a=C("a",1,true);var b=C("b",2);load(Screen(a,b),Team([],[]));observed=Cards(a,b);return !a.isChosen&&!b.isChosen&&TraceDisposeCollector.Log.SequenceEqual(new[]{"reset:a","reset:b","MockCardChoose_Disposed"});});
        Case(lt,"L.duplicates.order.and.skill.index",new{cards=new[]{1,1,2},ids=new[]{1,1},skills=new[]{11,22}},()=>
        {
            var a=C("a",1);var b=C("b",1);var c=C("c",2);load(Screen(a,b,c),Team([1,1],[11,22]));observed=Cards(a,b,c);
            return a.isChosen&&b.isChosen&&!c.isChosen&&a.currentSkill.id==22&&b.currentSkill.id==22&&
                TraceDisposeCollector.Log.Where(s=>s.StartsWith("skill.set:")).SequenceEqual(new[]{"skill.set:a:11","skill.set:b:11","skill.set:a:22","skill.set:b:22"});
        });
        Case(lt,"L.short.skills.keeps.second.skill",new{ids=new[]{1,2},skills=new[]{7}},()=>
        {var a=C("a",1);var b=C("b",2);b.currentSkill=new(){id=99};load(Screen(a,b),Team([1,2],[7]));observed=Cards(a,b);return a.isChosen&&b.isChosen&&a.currentSkill.id==7&&b.currentSkill.id==99;});
        Case(lt,"L.empty.skills.still.chooses",new{ids=new[]{1},skills=0},()=>
        {var a=C("a",1);load(Screen(a),Team([1],[]));observed=Cards(a);return a.isChosen&&a.currentSkill==null&&!TraceDisposeCollector.Log.Any(x=>x.StartsWith("skill.get:"));});
        Case(lt,"L.null.skills.no.match.skips.read",new{ids=new[]{99},skills="null"},()=>
        {var a=C("a",1);load(Screen(a),Team([99],null));return !a.isChosen;});
        Case(lt,"L.null.ids.after.reset",new{ids="null"},()=>Throws<NullReferenceException>(()=>load(Screen(C("a",1)),Team(null,[])),1));
        Case(lt,"L.null.skills.after.choose.cleanup",new{skills="null"},()=>Throws<NullReferenceException>(()=>load(Screen(C("a",1)),Team([1],null)),2));
        Case(lt,"L.null.screen",new{screen="null"},()=>Throws<NullReferenceException>(()=>load(null,Team([],[]))));
        Case(lt,"L.null.card",new{card="null"},()=>Throws<NullReferenceException>(()=>load(Screen(new MockCardChoose[]{null}),Team([],[])),1));
        Case(lt,"L.null.plantSave.cleanup",new{save="null"},()=>
        {var a=C("a",1);a.plantSave=null;return Throws<NullReferenceException>(()=>load(Screen(a),Team([1],[2])),2);});
        Case(lt,"L.getSkill.throws.cleanup",new{fault="GetSkill_ID"},()=>
        {var a=C("a",1);a.plantSave.getSkill=i=>throw new ArithmeticException();return Throws<ArithmeticException>(()=>load(Screen(a),Team([1],[2])),2);});
        Case(lt,"L.setSkill.throws.cleanup",new{fault="Set_Skill"},()=>
        {var a=C("a",1);a.onSetSkill=i=>throw new ArithmeticException();return Throws<ArithmeticException>(()=>load(Screen(a),Team([1],[2])),2);});
        Case(lt,"L.choose.replaces.plantSave",new{callback="replace save before lookup"},()=>
        {var a=C("a",1);var replacement=new MockPlantSave(9){getSkill=i=>new(){id=77}};a.onChoose=()=>a.plantSave=replacement;
            load(Screen(a),Team([1],[2]));observed=Cards(a);return a.currentSkill.id==77&&TraceDisposeCollector.Log.Contains("skill.get:9:2");});
        Case(lt,"L.id.list.grows.in.callback",new{initialIDs=new[]{1},append=2},()=>
        {var a=C("a",1);var b=C("b",2);var team=Team([1],[11,22]);bool once=false;a.onChoose=()=>{if(!once){once=true;team.ID.Add(2);}};
            load(Screen(a,b),team);observed=Cards(a,b);return a.isChosen&&b.isChosen&&b.currentSkill.id==22;});
        Case(lt,"L.team.struct.field.copy",new{initialIDs=new[]{1,3},callerReplacement=new[]{2}},()=>
        {var a=C("a",1);var b=C("b",2);var c=C("c",3);var team=Team([1,3],[]);a.onChoose=()=>team.ID=new(){2};
            load(Screen(a,b,c),team);observed=new{callerIDs=team.ID,cards=Cards(a,b,c)};return a.isChosen&&!b.isChosen&&c.isChosen&&team.ID.SequenceEqual(new[]{2});});
        Case(lt,"L.reset.replaces.cards",new{callback="replace field during reset"},()=>
        {var a=C("old",1);var b=C("new",1);var s=Screen(a);a.onReset=()=>s.card_ChooseList=new(){b};load(s,Team([1],[]));observed=Cards(a,b);return !a.isChosen&&b.isChosen;});
        Case(lt,"L.choose.invalidates.card.enumeration",new{fault="add card during choose"},()=>
        {var a=C("a",1);var s=Screen(a);a.onChoose=()=>s.card_ChooseList.Add(C("b",2));
            return Throws<InvalidOperationException>(()=>load(s,Team([1],[])),2);});
        Case(lt,"L.getSkill.null.result.passed.to.setter",new{skillResult="null"},()=>
        {var a=C("a",1);a.plantSave.getSkill=i=>null;load(Screen(a),Team([1],[2]));observed=Cards(a);return a.isChosen&&a.currentSkill==null&&TraceDisposeCollector.Log.Contains("skill.set:a:null");});

        MockUiImage Image(string name)
        {
            var child=new MockUiTransform(){label=name+".child",gameObject=new(){label=name+".marker"}};
            var image=new MockUiImage(){label=name,color=new(.1f,.2f,.3f,.9f),transform=new(){label=name+".root",children=new(){child}}};
            image.componentImage=image;return image;
        }
        MockCardChoose UI(string name,int id,int serial=1,bool selected=true)
        {
            var card=C(name,id,selected);card.serialNumBeChosen=serial;card.chosen=Image(name);
            card.serialNumBeChosenText=new(){label=name+".text",gameObject=new(){label=name+".textObject"}};return card;
        }
        MockSeedChooser Link(params MockCardChoose[] cards)
        {var s=Screen(cards);foreach(var c in cards)c.seedChooserScreen=s;return s;}
        object UIState(MockCardChoose c)=>new{card=Cards(c),alphaBits=BitConverter.SingleToInt32Bits(c.chosen.color.a),
            marker=c.chosen.transform.children[0].gameObject.active,textActive=c.serialNumBeChosenText.gameObject.active};
        Case(rt,"R.normal.ranks.and.UI",new{self=1,peerRanks=new[]{2,0,3}},()=>
        {
            var a=UI("a",1,1);var b=UI("b",2,2);var c=UI("c",3,0);var d=UI("d",4,3,false);var s=Link(a,b,c,d);
            reset(a);observed=new{self=UIState(a),others=Cards(b,c,d)};
            return !a.isChosen&&a.serialNumBeChosen==0&&b.serialNumBeChosen==1&&b.serialNumBeChosenText.text=="1"&&
                c.serialNumBeChosen==0&&d.serialNumBeChosen==3&&Bits(a.chosen.color.a,.002f)&&
                !a.chosen.transform.children[0].gameObject.active&&!a.serialNumBeChosenText.gameObject.active&&
                TraceDisposeCollector.Log.Count(x=>x=="MockCardChoose_Disposed")==1&&s.plantDetail.selected==null;
        });
        Case(rt,"R.color.three.separate.reads",new{reads=3,expectedRGB=new[]{1f,20f,300f}},()=>
        {
            var a=UI("a",1);Link(a);int n=0;a.chosen.onGetColor=()=>++n switch{1=>new(1,2,3,4),2=>new(10,20,30,40),_=>new(100,200,300,400)};
            reset(a);observed=new{reads=n,r=a.chosen.color.r,g=a.chosen.color.g,b=a.chosen.color.b,alphaBits=BitConverter.SingleToInt32Bits(a.chosen.color.a)};
            return n==3&&Bits(a.chosen.color.r,1)&&Bits(a.chosen.color.g,20)&&Bits(a.chosen.color.b,300);
        });
        Case(rt,"R.chosen.field.reload.after.color.callback",new{callback="replace chosen on first get_color"},()=>
        {
            var a=UI("a",1);Link(a);var old=a.chosen;var replacement=Image("new");int n=0;old.onGetColor=()=>{n++;a.chosen=replacement;return new(1,2,3,4);};
            reset(a);observed=new{reads=n,oldAlphaBits=BitConverter.SingleToInt32Bits(old.color.a),oldMarker=old.transform.children[0].gameObject.active,newMarker=replacement.transform.children[0].gameObject.active};
            return n==3&&Bits(old.color.a,.002f)&&old.transform.children[0].gameObject.active&&!replacement.transform.children[0].gameObject.active;
        });
        Case(rt,"R.null.chosen.partial.write",new{chosen="null"},()=>
        {var a=UI("a",1,9);a.chosen=null;bool p=Throws<NullReferenceException>(()=>reset(a));observed=new{exception=p,a.isChosen,a.serialNumBeChosen};return p&&!a.isChosen&&a.serialNumBeChosen==9;});
        Case(rt,"R.null.component.result",new{component="null"},()=>
        {var a=UI("a",1);a.chosen.onGetImage=()=>null;return Throws<NullReferenceException>(()=>reset(a));});
        for(int count=1;count<=3;count++)
        {
            int stop=count;
            Case(rt,"R.color.read.fault."+count,new{throwOnRead=count},()=>
            {var a=UI("a",1);int n=0;a.chosen.onGetColor=()=>{if(++n==stop)throw new ArithmeticException();return new(1,2,3,4);};return Throws<ArithmeticException>(()=>reset(a))&&!a.isChosen;});
        }
        Case(rt,"R.color.write.fault",new{fault="set_color"},()=>
        {var a=UI("a",1);a.chosen.onSetColor=c=>throw new ArithmeticException();return Throws<ArithmeticException>(()=>reset(a));});
        Case(rt,"R.child.adapter.fault",new{fault="controlled GetChild throws"},()=>
        {var a=UI("a",1);a.chosen.transform.onChild=i=>throw new ArithmeticException();return Throws<ArithmeticException>(()=>reset(a));});
        Case(rt,"R.null.text.after.marker.hidden",new{text="null"},()=>
        {var a=UI("a",1);a.serialNumBeChosenText=null;bool p=Throws<NullReferenceException>(()=>reset(a));return p&&!a.chosen.transform.children[0].gameObject.active;});
        Case(rt,"R.null.screen.after.UI",new{screen="null"},()=>
        {var a=UI("a",1,9);bool p=Throws<NullReferenceException>(()=>reset(a));return p&&a.serialNumBeChosen==9&&!a.serialNumBeChosenText.gameObject.active;});
        Case(rt,"R.null.cards.before.try",new{cards="null"},()=>
        {var a=UI("a",1);var s=Link(a);s.card_ChooseList=null;return Throws<NullReferenceException>(()=>reset(a));});
        Case(rt,"R.null.peer.cleanup",new{peer="null"},()=>
        {var a=UI("a",1);a.seedChooserScreen=Screen(a,null);return Throws<NullReferenceException>(()=>reset(a),1);});
        Case(rt,"R.rank.text.fault.partial.decrement",new{fault="set_text"},()=>
        {var a=UI("a",1,1);var b=UI("b",2,2);Link(a,b);b.serialNumBeChosenText.onSetText=s=>throw new ArithmeticException();
            bool p=Throws<ArithmeticException>(()=>reset(a),1);observed=new{exception=p,cards=Cards(a,b)};return p&&b.serialNumBeChosen==1&&a.serialNumBeChosen==1;});
        Case(rt,"R.rank.text.invalidates.list",new{fault="add card in set_text"},()=>
        {var a=UI("a",1,1);var b=UI("b",2,2);var s=Link(a,b);b.serialNumBeChosenText.onSetText=v=>s.card_ChooseList.Add(UI("c",3));
            return Throws<InvalidOperationException>(()=>reset(a),1);});
        Case(rt,"R.final.detail.field.reload",new{callback="replace seedChooserScreen in set_text"},()=>
        {var a=UI("a",1,1);var b=UI("b",2,2);var s=Link(a,b);s.plantDetail.selected=b;var replacement=Screen();replacement.plantDetail.selected=b;
            b.serialNumBeChosenText.onSetText=v=>a.seedChooserScreen=replacement;reset(a);observed=new{oldDetail=s.plantDetail.selected?.label,newDetail=replacement.plantDetail.selected?.label};
            return s.plantDetail.selected==b&&replacement.plantDetail.selected==null;});
        Case(rt,"R.null.detail.after.self.rank.zero",new{detail="null"},()=>
        {var a=UI("a",1,9);var s=Link(a);s.plantDetail=null;bool p=Throws<NullReferenceException>(()=>reset(a),1);return p&&a.serialNumBeChosen==0;});
        foreach(var pair in new[]{(self:2,peer:2,expected:2),(self:2,peer:1,expected:1),(self:int.MinValue,peer:int.MaxValue,expected:int.MaxValue-1)})
        {
            var input=pair;
            Case(rt,"R.rank.boundary."+input.self+"."+input.peer,input,()=>
            {var a=UI("a",1,input.self);var b=UI("b",2,input.peer);Link(a,b);reset(a);observed=Cards(a,b);return b.serialNumBeChosen==input.expected;});
        }
        Case(lt,"L.chain.real.candidate.reset",new{chain=new[]{"LoadTeam_Card","Choose_Reset"}},()=>
        {
            var a=UI("a",1,1);var s=Link(a);a.candidateReset=reset;load(s,Team([1],[7]));observed=UIState(a);
            return a.isChosen&&a.serialNumBeChosen==0&&a.currentSkill.id==7&&Bits(a.chosen.color.a,.002f)&&
                !a.serialNumBeChosenText.gameObject.active&&TraceDisposeCollector.Log.Any(x=>x.StartsWith("color.set:a:"));
        });
        Case(rt,"R.chain.prechoose.real.candidate.reset",new{chain=new[]{"SetPrechooseList","Choose_Reset"}},()=>
        {
            var a=UI("a",1,1);var s=Link(a);a.candidateReset=reset;
            CandidateCilTranslator.CompilePrechoose6BB((MethodDefinition)module.LookupToken(0x060006BB))(s,new(){C("request",1)});
            observed=UIState(a);return a.isChosen&&a.serialNumBeChosen==0&&Bits(a.chosen.color.a,.002f)&&
                TraceDisposeCollector.Log.Count(x=>x=="detail:null")==2;
        });

        MockPlant Plant(bool destroyed=false,int type=0)=>new(){destroyed=destroyed,plantType=type};
        MockDevice Device(int slot=1,bool destroyed=false)=>new(){occupyState=slot,destroyed=destroyed};
        for(int plantSlot=0;plantSlot<4;plantSlot++)for(int inputSlot=0;inputSlot<6;inputSlot++)
        {
            int occupied=plantSlot,incoming=inputSlot;
            Case(gt,$"G.plant.slot.{occupied}.incoming.{incoming}",new{occupied,incoming,expected=occupied!=incoming},()=>
            {
                var grid=new MockGrid();var p=Plant();
                switch(occupied){case 0:grid.plant_bottom=p;break;case 1:grid.plant_common=p;break;case 2:grid.plant_sheath=p;break;case 3:grid.plant_top=p;break;}
                bool result=place(grid,Device(incoming));observed=new{result};return result==(occupied!=incoming);
            });
        }
        for(int deviceSlot=0;deviceSlot<5;deviceSlot++)for(int inputSlot=0;inputSlot<6;inputSlot++)
        {
            int occupied=deviceSlot,incoming=inputSlot;
            Case(gt,$"G.device.slot.{occupied}.incoming.{incoming}",new{occupied,incoming,expected=occupied!=incoming},()=>
            {
                var grid=new MockGrid();var d=Device();
                switch(occupied){case 0:grid.device_bottom=d;break;case 1:grid.device_common=d;break;case 2:grid.device_sheath=d;break;case 3:grid.device_top=d;break;case 4:grid.device_ladder=d;break;}
                bool result=place(grid,Device(incoming));observed=new{result};return result==(occupied!=incoming);
            });
        }
        foreach(int state in new[]{-1,0,1,2,4,5,99})
        {
            int s=state;Case(gt,"G.state."+state,new{state,expected=state!=5},()=>{bool result=place(new(){gridState=s},Device());observed=new{result};return result==(s!=5);});
        }
        Case(gt,"G.wall.null.device.short.circuit",new{state=5,device="null"},()=>!place(new(){gridState=5},null));
        Case(gt,"G.null.grid",new{grid="null"},()=>Throws<NullReferenceException>(()=>place(null,Device())));
        Case(gt,"G.null.device.land",new{device="null"},()=>Throws<NullReferenceException>(()=>place(new(),null)));
        Case(gt,"G.null.device.pool",new{device="null"},()=>Throws<NullReferenceException>(()=>place(new(){gridState=3},null)));
        Case(gt,"G.pool.nonwater.empty",new{state=3,onWater=false},()=>!place(new(){gridState=3},Device()));
        Case(gt,"G.pool.water.empty",new{state=3,onWater=true},()=>place(new(){gridState=3},new(){onWater=true,occupyState=1}));
        Case(gt,"G.pool.lilypad.support",new{state=3,plantType=16},()=>place(new(){gridState=3,plant_bottom=Plant(type:16)},Device()));
        Case(gt,"G.pool.other.plant.reject",new{state=3,plantType=15},()=>!place(new(){gridState=3,plant_bottom=Plant(type:15)},Device()));
        Case(gt,"G.pool.device.support",new{state=3,bottomDevice=true},()=>place(new(){gridState=3,device_bottom=Device()},Device()));
        Case(gt,"G.pool.water.existing.support.reject",new{state=3,onWater=true},()=>!place(new(){gridState=3,plant_bottom=Plant(type:16)},new(){onWater=true,occupyState=1}));
        Case(gt,"G.controlled.destroyed.occupants",new{destroyed=true},()=>place(new(){plant_common=Plant(true),device_common=Device(destroyed:true)},Device()));
        Case(gt,"G.controlled.destroyed.support.skips.callback",new{destroyed=true},()=>
        {var d=Device(destroyed:true);d.bearCallback=(i,s)=>throw new ArithmeticException();return place(new(){device_bottom=d},Device());});
        Case(gt,"G.device.support.reject.short.circuits.plant",new{deviceBear=false},()=>
        {var d=Device();d.bearCallback=(i,s)=>false;var p=Plant();p.bearCallback=i=>throw new ArithmeticException();return !place(new(){device_bottom=d,plant_bottom=p},Device())&&!TraceDisposeCollector.Log.Any(x=>x.StartsWith("plant.bear:"));});
        Case(gt,"G.plant.support.reject",new{plantBear=false},()=>
        {var p=Plant();p.bearCallback=i=>false;return !place(new(){plant_bottom=p},Device());});
        Case(gt,"G.support.callback.reloads.plant.and.slot",new{beforeSlot=1,afterSlot=3},()=>
        {
            var incoming=Device(1);var d=Device();var grid=new MockGrid(){device_bottom=d};
            d.bearCallback=(i,s)=>{incoming.occupyState=3;grid.plant_bottom=new(){bearCallback=j=>j!=3};return true;};
            bool result=place(grid,incoming);observed=new{result};return !result&&TraceDisposeCollector.Log.SequenceEqual(new[]{"device.bear:1:device","plant.bear:3"});
        });
        Case(gt,"G.water.callback.does.not.recheck.wall",new{beforeState=3,afterState=5},()=>
        {var grid=new MockGrid(){gridState=3};var incoming=Device();incoming.waterCallback=()=>{grid.gridState=5;return true;};return place(grid,incoming);});
        foreach(int slot in new[]{int.MinValue,int.MaxValue})
        {int value=slot;Case(gt,"G.unknown.slot."+slot,new{slot},()=>place(new(),Device(value)));}
        Case(gt,"G.bear.adapter.throws",new{fault="device CanBear"},()=>
        {var d=Device();d.bearCallback=(i,s)=>throw new ArithmeticException();return Throws<ArithmeticException>(()=>place(new(){device_bottom=d},Device()));});

        void Mutant(MethodDefinition original,string tok,string name,Action<MethodDefinition> change,Func<MethodDefinition,bool> check)
        {
            var clone=CloneMethod(original,name);change(clone);mutantInputs.Add(clone);TraceDisposeCollector.Clear();observed=null;
            bool caught=false;Exception error=null;try{caught=check(clone);}catch(Exception e){error=e;}
            mutants.Add(new(){["target"]=tok,["mutation_id"]=name,["detected"]=caught,["observed"]=observed,
                ["observed_trace"]=TraceDisposeCollector.Log.ToArray(),["unexpected_exception"]=error?.ToString()});
            status[tok]&=caught;if(!caught)failures.Add(tok+" escaped mutant "+name);
        }
        Instruction At(MethodDefinition m,int offset,Mono.Cecil.Cil.OpCode op)
        {var i=m.Body.Instructions.Single(i=>i.Offset==offset);if(i.OpCode!=op)throw new InvalidProgramException("Pinned v7 mutant");return i;}
        Mutant(lm,lt,"L.match.inverted",m=>At(m,0x78,OpCodes.Bne_Un).OpCode=OpCodes.Beq,m=>
        {var a=C("a",1);var b=C("b",2);CandidateCilTranslator.CompileLoadTeam6B6(m)(Screen(a,b),Team([1],[]));observed=Cards(a,b);return !a.isChosen||b.isChosen;});
        Mutant(lm,lt,"L.skill.setter.omitted",m=>
        {var i=At(m,0xB2,OpCodes.Callvirt);i.OpCode=OpCodes.Pop;i.Operand=null;
            m.Body.Instructions.Insert(m.Body.Instructions.IndexOf(i)+1,Instruction.Create(OpCodes.Pop));},m=>
        {var a=C("a",1);CandidateCilTranslator.CompileLoadTeam6B6(m)(Screen(a),Team([1],[7]));observed=Cards(a);return a.currentSkill==null;});
        Mutant(lm,lt,"L.second.dispose.omitted",m=>{var i=At(m,0xCA,OpCodes.Call);i.OpCode=OpCodes.Pop;i.Operand=null;},m=>
        {CandidateCilTranslator.CompileLoadTeam6B6(m)(Screen(),Team([1],[]));return TraceDisposeCollector.Log.Count(x=>x=="MockCardChoose_Disposed")!=2;});
        Mutant(rm,rt,"R.alpha.changed",m=>At(m,0x3E,OpCodes.Ldc_R4).Operand=.5f,m=>
        {var a=UI("a",1);Link(a);CandidateCilTranslator.CompileReset2F5(m)(a);observed=UIState(a);return !Bits(a.chosen.color.a,.002f);});
        Mutant(rm,rt,"R.rgb.red.used.for.green",m=>At(m,0x32,OpCodes.Ldfld).Operand=module.LookupToken(0x0A0000EE),m=>
        {var a=UI("a",1);Link(a);CandidateCilTranslator.CompileReset2F5(m)(a);observed=new{a.chosen.color.r,a.chosen.color.g};return !Bits(a.chosen.color.g,.2f);});
        Mutant(rm,rt,"R.rank.incremented",m=>At(m,0xC2,OpCodes.Sub).OpCode=OpCodes.Add,m=>
        {var a=UI("a",1,1);var b=UI("b",2,2);Link(a,b);CandidateCilTranslator.CompileReset2F5(m)(a);observed=Cards(a,b);return b.serialNumBeChosen!=1;});
        Mutant(rm,rt,"R.dispose.omitted",m=>{var i=At(m,0xF3,OpCodes.Call);i.OpCode=OpCodes.Pop;i.Operand=null;},m=>
        {var a=UI("a",1);Link(a);CandidateCilTranslator.CompileReset2F5(m)(a);return !TraceDisposeCollector.Log.Contains("MockCardChoose_Disposed");});
        Mutant(gm,gt,"G.wall.constant.changed",m=>At(m,0x85,OpCodes.Ldc_I4_5).OpCode=OpCodes.Ldc_I4_4,m=>
        {bool result=CandidateCilTranslator.CompileCanPlacing297(m)(new(){gridState=5},Device());observed=new{result};return result;});
        Mutant(gm,gt,"G.common.device.reads.top",m=>At(m,0x104,OpCodes.Ldfld).Operand=module.LookupToken(0x04000390),m=>
        {bool result=CandidateCilTranslator.CompileCanPlacing297(m)(new(){device_common=Device()},Device(1));observed=new{result};return result;});
        Mutant(gm,gt,"G.lilypad.constant.changed",m=>At(m,0x49,OpCodes.Ldc_I4).Operand=15,m=>
        {bool result=CandidateCilTranslator.CompileCanPlacing297(m)(new(){gridState=3,plant_bottom=Plant(type:16)},Device());observed=new{result};return !result;});
        Mutant(gm,gt,"G.plant.support.branch.inverted",m=>At(m,0x1A6,OpCodes.Brfalse).OpCode=OpCodes.Brtrue,m=>
        {bool result=CandidateCilTranslator.CompileCanPlacing297(m)(new(){plant_bottom=Plant()},Device());observed=new{result};return !result;});
    }
}
