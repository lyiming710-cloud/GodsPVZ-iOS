// Source only. Candidate construction and execution belong to the independent verifier.
internal static partial class Program
{
    private static void N117_State(IL b,int field,int value)=>b.Arg(0).Integer(value).Set(field);
    private static void N117_Yield(IL b,int state,int current,int next,float? seconds)
    {
        b.Arg(0);if(seconds.HasValue)b.Float(seconds.Value).New(0x0A00034B);else b.Op(0x14);
        b.Set(current);N117_State(b,state,next);b.Integer(1).Op(0x2A);
    }
    private static void N117_IncrementField(IL b,int field)=>b.Arg(0).Op(0x25).Field(field).Integer(1).Op(0x58).Set(field);
    private static void N117_Finally(IL b,int token,params (string Start,string Handler,string End)[] ranges)
    {
        Require(N117_EhTargets.Contains(token),"Native117 explicit EH scope");
        FinallyBodies.Add(token,ranges.Select(r=>new FinallyClause(b.OffsetOf(r.Start),b.OffsetOf(r.Handler),b.OffsetOf(r.Handler),b.OffsetOf(r.End))).ToArray());
    }
    private static byte[] N117_AnimateProperties()
    {
        var b=new IL();b.Arg(0).Field(0x04000994).Store(0);b.Arg(0).Field(0x04000996).Store(1);
        b.Load(0).Branch(0x39,"start").Load(0).Integer(1).Branch(0x3B,"resume").Integer(0).Op(0x2A);
        b.Label("start");N117_State(b,0x04000994,-1);
        b.Float(0).Float(1).Call(0x0A000083).Store(2);b.Load(1).Load(2).Set(0x04000993).Branch(0x38,"body");
        b.Label("resume");N117_State(b,0x04000994,-1);
        b.Label("body").Load(1).Field(0x04000992).Load(1).Field(0x04000993).Virtual(0x0A0001ED).Store(2);
        b.Load(1).Field(0x04000991).Static(0x0A00032C).Load(2).Virtual(N117_MaterialSetFloat);
        // Native captures the old frame before deltaTime, then draws a fresh random rate.
        b.Load(1).Field(0x04000993).Store(2);b.Call(0x0A00001A).Store(3);
        b.Float(.2f).Float(.3f).Call(0x0A000083).Load(3).Op(0x5A).Load(2).Op(0x58).Store(2);
        b.Load(1).Load(2).Set(0x04000993);b.Arg(0).New(0x0A00032E).Set(0x04000995);
        N117_State(b,0x04000994,1);b.Integer(1).Op(0x2A);return b.Finish();
    }
    private static byte[] N117_RevealCharacters()
    {
        var b=new IL();b.Arg(0).Field(0x040009AE).Store(0);b.Arg(0).Field(0x040009B1).Store(1);
        b.Load(0).Branch(0x39,"start").Load(0).Integer(1).Branch(0x3B,"resume1").Load(0).Integer(2).Branch(0x3B,"resume2").Integer(0).Op(0x2A);
        b.Label("start");N117_State(b,0x040009AE,-1);
        b.Arg(0).Field(0x040009B0).Integer(0).Integer(0).Virtual(0x0A000338);
        b.Arg(0).Arg(0).Field(0x040009B0).Virtual(0x0A0002B8).Set(0x040009B2);
        b.Arg(0).Arg(0).Field(0x040009B2).Field(0x0A000339).Set(0x040009B3);
        N117_State(b,0x040009B4,0);b.Branch(0x38,"checkChange");
        b.Label("resume1");N117_State(b,0x040009AE,-1);N117_State(b,0x040009B4,0);b.Branch(0x38,"render");
        b.Label("resume2");N117_State(b,0x040009AE,-1);
        b.Label("checkChange").Load(1).Field(0x040009AD).Branch(0x39,"checkCount");
        b.Arg(0).Arg(0).Field(0x040009B2).Field(0x0A000339).Set(0x040009B3);b.Load(1).Integer(0).Set(0x040009AD);
        b.Label("checkCount").Arg(0).Field(0x040009B4).Arg(0).Field(0x040009B3).Branch(0x3D,"pause");
        b.Label("render").Arg(0).Field(0x040009B0).Arg(0).Field(0x040009B4).Virtual(0x0A00034C);
        N117_IncrementField(b,0x040009B4);N117_Yield(b,0x040009AE,0x040009AF,2,null);
        b.Label("pause");N117_Yield(b,0x040009AE,0x040009AF,1,1f);return b.Finish();
    }
    private static byte[] N117_RevealWords()
    {
        var b=new IL();b.Arg(0).Field(0x040009B5).Store(0);
        b.Load(0).Branch(0x39,"start").Load(0).Integer(1).Branch(0x3B,"resume1").Load(0).Integer(2).Branch(0x3B,"resume2").Integer(0).Op(0x2A);
        b.Label("start");N117_State(b,0x040009B5,-1);
        b.Arg(0).Field(0x040009B7).Integer(0).Integer(0).Virtual(0x0A000338);
        b.Arg(0).Arg(0).Field(0x040009B7).Virtual(0x0A0002B8).Field(0x0A000351).Set(0x040009B8);
        b.Arg(0).Arg(0).Field(0x040009B7).Virtual(0x0A0002B8).Field(0x0A000339).Set(0x040009B9);
        N117_State(b,0x040009BA,0);N117_State(b,0x040009BB,0);b.Branch(0x38,"body");
        b.Label("resume1");N117_State(b,0x040009B5,-1);b.Branch(0x38,"increment");
        b.Label("resume2");N117_State(b,0x040009B5,-1);
        b.Label("body").Arg(0).Field(0x040009BA).Arg(0).Field(0x040009B8).Integer(1).Op(0x58,0x5D).Store(1);
        b.Load(1).Branch(0x39,"zero").Load(1).Arg(0).Field(0x040009B8).Branch(0x3F,"word");
        b.Load(1).Arg(0).Field(0x040009B8).Branch(0x40,"render");
        b.Arg(0).Arg(0).Field(0x040009B9).Set(0x040009BB).Branch(0x38,"render");
        b.Label("word").Arg(0).Arg(0).Field(0x040009B7).Virtual(0x0A0002B8).Field(0x0A0002E5);
        b.Load(1).Integer(1).Op(0x59).Ref(0x8F,N117_TmpWordType).Field(N117_WordLast).Integer(1).Op(0x58).Set(0x040009BB).Branch(0x38,"render");
        b.Label("zero");N117_State(b,0x040009BB,0);
        b.Label("render").Arg(0).Field(0x040009B7).Arg(0).Field(0x040009BB).Virtual(0x0A00034C);
        b.Arg(0).Field(0x040009BB).Arg(0).Field(0x040009B9).Branch(0x3C,"pause");
        b.Label("increment");N117_IncrementField(b,0x040009BA);N117_Yield(b,0x040009B5,0x040009B6,2,.1f);
        b.Label("pause");N117_Yield(b,0x040009B5,0x040009B6,1,1f);return b.Finish();
    }
    private static byte[] N117_TeleType()
    {
        var b=new IL();b.Arg(0).Field(0x040009A7).Store(0);b.Arg(0).Field(0x040009A9).Store(1);
        foreach(var s in new[]{0,1,2,3,4})b.Load(0).Integer(s).Branch(0x3B,"state"+s);
        b.Integer(0).Op(0x2A);
        b.Label("state0");N117_State(b,0x040009A7,-1);b.Branch(0x38,"initialize");
        b.Label("state1");N117_State(b,0x040009A7,-1);b.Load(1).Field(0x040009A6).Load(1).Field(0x040009A5).Virtual(0x0A000062);
        N117_Yield(b,0x040009A7,0x040009A8,2,1f);
        b.Label("state2");N117_State(b,0x040009A7,-1);b.Load(1).Field(0x040009A6).Load(1).Field(0x040009A4).Virtual(0x0A000062);
        N117_Yield(b,0x040009A7,0x040009A8,3,1f);
        b.Label("state3");N117_State(b,0x040009A7,-1);b.Branch(0x38,"increment");
        b.Label("state4");N117_State(b,0x040009A7,-1);b.Branch(0x38,"body");
        b.Label("initialize").Load(1).Field(0x040009A6).Integer(0).Integer(0).Virtual(0x0A000338);
        b.Arg(0).Load(1).Field(0x040009A6).Virtual(0x0A0002B8).Field(0x0A000339).Set(0x040009AA);N117_State(b,0x040009AB,0);
        b.Label("body").Arg(0).Field(0x040009AB).Arg(0).Field(0x040009AA).Integer(1).Op(0x58,0x5D).Store(2);
        b.Load(1).Field(0x040009A6).Load(2).Virtual(0x0A00034C);
        b.Load(2).Arg(0).Field(0x040009AA).Branch(0x3C,"pause");
        b.Label("increment");N117_IncrementField(b,0x040009AB);N117_Yield(b,0x040009A7,0x040009A8,4,.05f);
        b.Label("pause");N117_Yield(b,0x040009A7,0x040009A8,1,1f);return b.Finish();
    }
    private static byte[] N117_EnvMap()
    {
        var b=new IL();b.Arg(0).Field(0x04000922).Store(0);b.Arg(0).Field(0x04000924).Store(1);
        b.Load(0).Branch(0x39,"start").Load(0).Integer(1).Branch(0x3B,"resume").Integer(0).Op(0x2A);
        b.Label("start");N117_State(b,0x04000922,-1);b.Arg(0).FieldAddress(0x04000925).Op(0xFE).Ref(0x15,N117_MatrixType).Branch(0x38,"body");
        b.Label("resume");N117_State(b,0x04000922,-1);
        b.Label("body").Call(0x0A0000FC).Store(2);
        b.Call(0x0A0002D8).Store(3);b.Load(1).FieldAddress(0x0400091F).Field(0x0A00001D).Store(4);
        b.Call(0x0A0002D8).Store(5);b.Load(1).FieldAddress(0x0400091F).Field(0x0A00008B).Store(6);
        b.Call(0x0A0002D8).Store(7);b.Load(1).FieldAddress(0x0400091F).Field(0x0A00009D).Store(8);
        // All three clock calls and component reads retain the original order.
        b.Load(4).Load(3).Op(0x5A).Load(6).Load(5).Op(0x5A).Load(7).Load(8).Op(0x5A).Call(0x0A00020E).Store(9);
        b.Arg(0).FieldAddress(0x04000925).Load(2).Load(9).Call(0x0A000209).Call(0x0A0002DA);
        b.Load(1).Field(0x04000921).Ref(0x72,0x7001D8E0).Arg(0).Field(0x04000925).Virtual(0x0A0002D9);
        N117_Yield(b,0x04000922,0x04000923,1,null);return b.Finish();
    }
    private static byte[] N117_RandomGrids()
    {
        var b=new IL();b.New(0x0A0001BC).Store(0);b.Arg(0).Arg(2).Arg(3).Call(0x06000288).Store(1);
        b.Load(1).Virtual(N117_GridCount).Store(2);b.Arg(1).Load(2).Branch(0x3D,"return");
        b.New(0x0A0001BD).Store(3);b.Label("draw").Load(3).Virtual(0x0A0001C6).Arg(1).Branch(0x3C,"enumerate");
        b.Load(3).Integer(0).Load(2).Call(0x0A00012B).Virtual(0x0A0001BE).Op(0x26).Branch(0x38,"draw");
        b.Label("enumerate").Load(3).Virtual(0x0A0001C7).Store(4);
        b.Label("try").Branch(0x38,"move");b.Label("body").Load(1).Address(4).Call(0x0A0001C8).Virtual(0x0A000078).Store(5);
        b.Load(0).Load(5).Virtual(N116_GridAdd);
        b.Label("move").Address(4).Call(0x0A0001C2).Branch(0x3A,"body").Branch(0xDD,"return");
        b.Label("finally").Address(4).Call(0x0A0001C1).Op(0xDC);
        b.Label("return").Load(0).Op(0x2A);N117_Finally(b,0x0600028E,("try","finally","return"));return b.Finish();
    }
    private static byte[] N117_CreateZombie()
    {
        var b=new IL();b.Arg(0).Field(0x04000361).Arg(1).Virtual(N117_ZombieItem).Op(0x14).Call(0x0A000012).Branch(0x3A,"missing");
        // The original reads the prefab list a second time after Unity's null comparison.
        b.Arg(0).Field(0x04000361).Arg(1).Virtual(N117_ZombieItem).Call(0x2B00004F).Store(0);
        b.Load(0).Integer(1).Set(0x040005D7);b.Load(0).Arg(0).Field(0x0400035F).Set(0x040005AA);
        b.Load(0).Virtual(0x0A000009).Virtual(0x0A00002E).Float(0).Float(0).Float(0).New(0x0A00009C).Virtual(0x0A000038);
        b.Load(0).Virtual(0x0A000009).Virtual(0x0A00002E).Arg(0).Virtual(0x0A000001).Integer(1).Virtual(0x0A00002F);
        b.Load(0).Float(.01f).Set(0x040005F6);b.Arg(0).Field(0x04000362).Load(0).Virtual(N117_ZombieAdd);
        b.Load(0).Field(0x04000613).Virtual(0x0A000037).Store(1);b.Load(0).Address(1).Field(0x0A00001D).Set(0x040005AD);
        b.Load(0).Field(0x04000613).Virtual(0x0A000037).Store(1);b.Load(0).Address(1).Field(0x0A00008B).Set(0x040005AE);
        b.Load(0).Virtual(0x06000477).Load(0).Op(0x2A);
        b.Label("missing").Ref(0x72,0x7000A00C).Call(0x0A000010).Op(0x14,0x2A);return b.Finish();
    }
    private static byte[] N117_ExtendFlags()
    {
        var b=new IL();b.Label("flags").Arg(0).Field(0x0400020B).Virtual(N117_FlagCount).Arg(0).Field(0x04000210).Branch(0x3C,"outerStart");
        b.New(0x0600019E).Store(0);b.Load(0).New(0x0A00014D).Set(0x040001F3);
        b.Arg(0).Field(0x0400020B).Virtual(N117_FlagCount).Arg(0).Field(0x04000210).Integer(1).Op(0x59).Branch(0x40,"appendFlag");
        b.Load(0).Integer(1).Set(0x040001F4);
        b.Label("appendFlag").Arg(0).Field(0x0400020B).Load(0).Virtual(0x0A00014E).Branch(0x38,"flags");
        b.Label("outerStart").Integer(0).Store(2).Branch(0x38,"outerCheck");
        b.Label("outer").Arg(0).Field(0x0400020B).Load(2).Virtual(0x0A00013D).Store(0);
        b.Load(0).Field(0x040001F3).Branch(0x3A,"waves");b.Load(0).New(0x0A00014D).Set(0x040001F3);
        b.Label("waves").Load(0).Field(0x040001F4).Branch(0x3A,"last");
        b.Load(0).Field(0x040001F3).Virtual(N116_WaveCount).Integer(10).Branch(0x3C,"nextFlag").Branch(0x38,"newWave");
        b.Label("last").Load(0).Field(0x040001F3).Virtual(N116_WaveCount);
        b.Arg(0).Field(0x04000211).Integer(1).Op(0x59).Integer(10).Op(0x5D).Integer(1).Op(0x58).Branch(0x3C,"nextFlag");
        b.Label("newWave").New(0x0600019F).Store(1);b.Load(1).New(0x0A0000F1).Set(0x040001F5);
        b.Load(0).Field(0x040001F3).Virtual(N116_WaveCount).Integer(9).Branch(0x40,"appendWave");b.Load(1).Integer(1).Set(0x040001F6);
        b.Label("appendWave").Load(0).Field(0x040001F3).Load(1).Virtual(0x0A000150).Branch(0x38,"waves");
        b.Label("nextFlag").Load(2).Integer(1).Op(0x58).Store(2);
        b.Label("outerCheck").Load(2).Arg(0).Field(0x04000210).Branch(0x3F,"outer").Op(0x2A);return b.Finish();
    }
    private static byte[] N117_DeviceEvents()
    {
        var b=new IL();
        b.Arg(0).Field(0x04000483).Virtual(N117_EventEnum).Store(0);
        b.Label("tryAdd").Branch(0x38,"moveAdd");b.Label("bodyAdd").Address(0).Call(N117_EventCurrent).Store(1);
        b.Arg(0).Field(0x04000482).Load(1).Virtual(N117_EventAdd);
        b.Label("moveAdd").Address(0).Call(N117_EventMove).Branch(0x3A,"bodyAdd").Branch(0xDD,"afterAdd");
        b.Label("finallyAdd").Address(0).Call(N117_EventDispose).Op(0xDC);
        b.Label("afterAdd").Arg(0).Field(0x04000483).Virtual(N117_EventClear);
        b.Arg(0).Field(0x04000482).Virtual(N117_EventEnum).Store(0);
        b.Label("tryUpdate").Branch(0x38,"moveUpdate");b.Label("bodyUpdate").Address(0).Call(N117_EventCurrent).Virtual(0x06000115);
        b.Label("moveUpdate").Address(0).Call(N117_EventMove).Branch(0x3A,"bodyUpdate").Branch(0xDD,"afterUpdate");
        b.Label("finallyUpdate").Address(0).Call(N117_EventDispose).Op(0xDC);
        b.Label("afterUpdate").Arg(0).Field(0x04000484).Virtual(N117_EventEnum).Store(0);
        b.Label("tryRemove").Branch(0x38,"moveRemove");b.Label("bodyRemove").Address(0).Call(N117_EventCurrent).Store(1);
        b.Arg(0).Field(0x04000482).Load(1).Virtual(N117_EventRemove).Op(0x26);
        b.Label("moveRemove").Address(0).Call(N117_EventMove).Branch(0x3A,"bodyRemove").Branch(0xDD,"afterRemove");
        b.Label("finallyRemove").Address(0).Call(N117_EventDispose).Op(0xDC);
        b.Label("afterRemove").Arg(0).Field(0x04000484).Virtual(N117_EventClear).Op(0x2A);
        N117_Finally(b,0x0600030C,("tryAdd","finallyAdd","afterAdd"),("tryUpdate","finallyUpdate","afterUpdate"),("tryRemove","finallyRemove","afterRemove"));return b.Finish();
    }
    private static byte[] N117_ZombieBrightness()
    {
        var b=new IL();b.Arg(2).Ref(0x72,0x7000FD69).Call(0x0A00003A).Branch(0x39,"armorCheck");
        b.Arg(0).Field(0x04000609).Virtual(N117_GameEnum).Store(0);
        b.Label("tryBody").Branch(0x38,"moveBody");b.Label("body").Address(0).Call(N117_GameCurrent).Store(1);
        b.Arg(0).Field(0x0400060A).Load(1).Virtual(N117_GameIndex).Integer(-1).Branch(0x40,"moveBody");
        b.Load(1).Virtual(0x2B000011).Virtual(0x0A00021D).Static(0x04000622).Arg(1).Virtual(0x0A00003E);
        b.Label("moveBody").Address(0).Call(N117_GameMove).Branch(0x3A,"body").Branch(0xDD,"armorCheck");
        b.Label("finallyBody").Address(0).Call(N117_GameDispose).Op(0xDC);
        b.Label("armorCheck").Arg(2).Ref(0x72,0x7000FD5B).Call(0x0A00003A).Branch(0x39,"done");
        b.Arg(0).Field(0x0400060A).Virtual(N117_GameEnum).Store(0);
        b.Label("tryArmor").Branch(0x38,"moveArmor");b.Label("armor").Address(0).Call(N117_GameCurrent).Virtual(0x2B000011).Virtual(0x0A00021D).Static(0x04000622).Arg(1).Virtual(0x0A00003E);
        b.Label("moveArmor").Address(0).Call(N117_GameMove).Branch(0x3A,"armor").Branch(0xDD,"done");
        b.Label("finallyArmor").Address(0).Call(N117_GameDispose).Op(0xDC);b.Label("done").Op(0x2A);
        N117_Finally(b,0x06000475,("tryBody","finallyBody","armorCheck"),("tryArmor","finallyArmor","done"));return b.Finish();
    }
    private static void N117_IvhParticle(IL b,int id,int gameLocal)
    {
        b.Call(0x060001E5).Integer(id).Virtual(0x060001EA).Store(gameLocal);
        b.Load(gameLocal).Call(0x0A00000B).Branch(0x39,"done");
        b.Load(gameLocal).Virtual(0x0A00002E).Arg(0).Field(0x04000613).Virtual(0x0A000001).Virtual(0x0A000037).Virtual(0x0A000038);
        b.Label("done").Op(0x2A);
    }
    private static byte[] N117_IvhEnhance1()
    {
        var b=new IL();b.Arg(0).Field(0x040005AA).Field(0x040003D5).Field(0x04000362).Virtual(N116_ZombieEnum).Store(0);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(0).Call(N116_ZombieCurrent).Store(1);
        b.Load(1).Virtual(0x06000464).Branch(0x3A,"move");b.Load(1).Field(0x040005F1).Arg(0).Field(0x040005F1).Branch(0x40,"move");
        b.Ref(0x72,0x70000F30).Float(.5f).Integer(1).New(0x060000F6).Store(2);
        b.Load(2).Ref(0x72,0x70012343).Set(0x04000105);b.Load(2).Integer(0).Set(0x04000106);b.Load(2).Float(15).Set(0x04000107);
        // Capture childBuffs before constructing the child; constructor callbacks may mutate the parent.
        b.Load(2).Field(0x0400010D).Ref(0x72,0x70003D42).Float(1).Integer(1).New(0x060000F6).Virtual(N117_BuffAdd);
        b.Load(1).Field(0x0400061A).Load(2).Virtual(0x06000102);
        b.Label("move").Address(0).Call(N116_ZombieMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(0).Call(N116_ZombieDispose).Op(0xDC);b.Label("after");N117_IvhParticle(b,14,3);
        N117_Finally(b,0x0600048E,("try","finally","after"));return b.Finish();
    }
    private static byte[] N117_IvhEnhance2()
    {
        var b=new IL();b.Arg(0).Field(0x040005AA).Field(0x040003D5).Field(0x04000362).Virtual(N116_ZombieEnum).Store(0);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(0).Call(N116_ZombieCurrent).Store(1);
        b.Load(1).Virtual(0x06000464).Branch(0x3A,"move");b.Load(1).Field(0x040005F1).Arg(0).Field(0x040005F1).Branch(0x40,"move");
        // FindBuff returns a plain Buff reference: the native test is raw null, not Unity Object equality.
        b.Load(1).Field(0x0400061A).Ref(0x72,0x7001376D).Virtual(0x06000106).Branch(0x3A,"move");
        b.Load(1).Arg(0).Call(0x0A000012).Branch(0x39,"classify");
        b.Ref(0x72,0x7000450D).Float(2).Integer(1).New(0x060000F6).Store(2);
        b.Load(2).Ref(0x72,0x70013781).Set(0x04000105);b.Load(2).Integer(1).Set(0x04000106);
        b.Load(1).Field(0x0400061A).Load(2).Virtual(0x06000102);
        b.Label("classify").Load(1).Field(0x040005CF).Branch(0x3A,"boss").Load(1).Field(0x040005CE).Branch(0x3A,"boss");
        b.Ref(0x72,0x7000450D).Float(160).Integer(0).New(0x060000F6).Store(2).Branch(0x38,"apply");
        b.Label("boss").Ref(0x72,0x7000450D).Float(80).Integer(0).New(0x060000F6).Store(2);
        b.Label("apply").Load(2).Ref(0x72,0x7001376D).Set(0x04000105);b.Load(2).Integer(1).Set(0x04000106);
        b.Load(1).Field(0x0400061A).Load(2).Virtual(0x06000102);
        b.Label("move").Address(0).Call(N116_ZombieMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(0).Call(N116_ZombieDispose).Op(0xDC);b.Label("after");N117_IvhParticle(b,15,3);
        N117_Finally(b,0x06000493,("try","finally","after"));return b.Finish();
    }
    private static byte[] N117_PlantFight()
    {
        var b=new IL();b.Arg(0).Field(0x040004DD).Virtual(N116_ZombieCount).Integer(0).Branch(0x3E,"skills");
        b.Arg(0).Field(0x040004DD).Virtual(N116_ZombieCount).Integer(1).Op(0x59).Store(0).Branch(0x38,"backCheck");
        b.Label("back").Load(0).Arg(0).Field(0x040004DD).Virtual(N116_ZombieCount).Branch(0x3C,"skills");
        b.Arg(0).Field(0x040004DD).Load(0).Virtual(N117_ZombieItem).Store(1);
        b.Load(1).Op(0x14).Call(0x0A000012).Branch(0x3A,"remove");b.Load(1).Virtual(0x06000464).Branch(0x39,"decrement");
        b.Arg(0).Load(1).Integer(0).Call(0x06000358).Branch(0x38,"decrement");
        b.Label("remove").Arg(0).Field(0x040004DD).Load(1).Virtual(N117_ZombieRemove).Op(0x26);
        b.Label("decrement").Load(0).Integer(1).Op(0x59).Store(0);
        b.Label("backCheck").Load(0).Integer(0).Branch(0x3C,"back");
        b.Label("skills");foreach(var p in new[]{(Id:0,Call:0x06000385),(Id:3,Call:0x06000383),(Id:4,Call:0x06000387)})
        {b.Arg(0).Field(0x040004B1).Integer(p.Id).Branch(0x40,"next"+p.Id).Arg(0).Call(p.Call).Label("next"+p.Id);}
        b.Arg(0).Field(0x040004B1).Integer(5).Branch(0x40,"star");
        b.Arg(0).Field(0x040004B4).Integer(0).Branch(0x3E,"star");b.Arg(0).Field(0x040004D7).Integer(1).Branch(0x3E,"star");
        b.Arg(0).Field(0x040004D7).Integer(1).Branch(0x3E,"star");
        b.Arg(0).Field(0x040004CF).Store(2);b.Arg(0).Field(0x040004C9).Store(3);b.Call(0x0A00001A).Load(3).Op(0x5A).Load(2).Op(0x58).Store(2);
        b.Arg(0).Load(2).Set(0x040004CF);b.Load(2).Arg(0).Field(0x040004D0).Branch(0x44,"star");
        b.Arg(0).Field(0x040004E3).Static(0x04000500).Virtual(0x0A0001F4);b.Arg(0).Float(0).Set(0x040004CF);
        b.Label("star").Arg(0).Field(0x040004B1).Integer(49).Branch(0x40,"common");b.Arg(0).Call(0x0600038D);
        b.Label("common").Arg(0).Call(0x06000357);b.Arg(0).Call(0x0600034E);b.Arg(0).Field(0x040004FF).Arg(0).Virtual(0x2B00005A);
        b.Arg(0).Field(0x040004F3).Store(4);b.Load(4).Field(0x040003B4).Branch(0x39,"done");b.Load(4).Field(0x040003B5).Store(5).Load(5).Branch(0x39,"done");
        b.Float(0).Store(6);foreach(var p in new[]{(Id:1,Rate:25f),(Id:8,Rate:125f),(Id:10,Rate:150f)})
        {b.Load(5).Field(0x0400037E).Integer(p.Id).Branch(0x40,"scenario"+p.Id).Float(p.Rate).Store(6).Branch(0x38,"element").Label("scenario"+p.Id);}
        b.Label("element").Call(0x0A00001A).Load(6).Op(0x5A).Store(6);b.Integer(0).Load(6).Op(0x14).Integer(1).New(0x0600011C).Store(7);
        b.Arg(0).Field(0x040004D6).Load(7).Virtual(0x06000124);b.Label("done").Op(0x2A);return b.Finish();
    }
    private static byte[] N117_MouseSkill()
    {
        var b=new IL();b.Arg(0).Field(0x04000244).Op(0x14).Call(0x0A000012).Branch(0x3A,"noPlant");
        b.Arg(0).Field(0x04000244).Field(0x040004B1).Store(0).Branch(0x38,"seek");b.Label("noPlant").Integer(-1).Store(0);
        b.Label("seek").Arg(0).Load(0).Call(0x060001E0).Store(1);
        b.Arg(0).Field(0x04000244).Op(0x14).Integer(-1).Virtual(0x0600036C).Store(2);
        b.Arg(0).Integer(1).Load(2).Call(0x060001DC).Store(3);
        b.Arg(0).Field(0x04000244).Field(0x040004D1).Store(4);b.Arg(0).Field(0x04000244).Store(5);
        b.Load(5).Field(0x040004B1).Store(0);b.Load(0).Branch(0x39,"pea");b.Load(0).Integer(4).Branch(0x3B,"potato");b.Load(0).Integer(49).Branch(0x3B,"sword").Branch(0x38,"cleanup");
        b.Label("pea").Load(4).Field(0x0400068E).Integer(1).Branch(0x40,"cleanup");
        b.Load(1).Call(0x0A00000B).Branch(0x39,"tryDevice");
        b.Arg(0).Field(0x04000244).Load(1).Set(0x040004DB);b.Arg(0).Field(0x04000244).Op(0x14).Set(0x040004DC).Branch(0x38,"trigger");
        b.Label("tryDevice").Load(4).Field(0x0400068E).Integer(1).Branch(0x40,"cleanup");b.Load(3).Call(0x0A00000B).Branch(0x39,"cleanup");
        b.Arg(0).Field(0x04000244).Op(0x14).Set(0x040004DB);b.Arg(0).Field(0x04000244).Load(3).Set(0x040004DC).Branch(0x38,"trigger");
        b.Label("potato").Load(4).Field(0x0400068E).Integer(1).Branch(0x40,"cleanup");
        b.Load(5).Field(0x040004AF).Integer(1).Virtual(0x060005BD).Store(6);b.Arg(0).Field(0x04000244).Store(5);
        b.Load(6).Arg(0).Field(0x0400023A).Virtual(0x060000DD).Store(7);b.Load(5).Load(7).Set(0x040004DE).Branch(0x38,"trigger");
        b.Label("sword").Load(4).Field(0x0400068E).Integer(1).Branch(0x40,"cleanup");b.Load(5).Arg(0).Field(0x0400023A).Set(0x040004DE);
        b.Label("trigger").Arg(0).Field(0x04000244).Virtual(0x060003B8);
        b.Label("cleanup").Arg(0).Field(0x04000244).Field(0x040004AF).Virtual(0x060005BE);
        b.Arg(0).Field(0x04000240).Call(0x0A000025);b.Arg(0).Call(0x060001E1);b.Arg(0).Field(0x04000230).Virtual(0x060002B8).Op(0x2A);return b.Finish();
    }
    private static byte[] N117_TrySwordFormation()
    {
        var b=new IL();b.Arg(0).Field(0x040004B4).Integer(4).Branch(0x3F,"done");
        b.New(0x0A00016B).Store(0);b.Integer(0).Store(3);
        b.Arg(0).Field(0x040004F3).Field(0x040003D6).Field(0x040002B6).Virtual(N117_ProjectileEnum).Store(1);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(1).Call(N117_ProjectileCurrent).Store(2);
        b.Load(2).Virtual(0x0A000009).Virtual(0x0A000154).Branch(0x39,"move");
        b.Load(2).Field(0x0400054C).Integer(21).Branch(0x40,"move");b.Float(1).Load(2).Field(0x04000550).Branch(0x3D,"move");
        b.Load(2).Field(0x04000571).Arg(0).Call(0x0A00000C).Branch(0x3A,"move");
        b.Load(3).Integer(1).Op(0x58).Store(3);b.Load(0).Load(2).Virtual(0x0A000166);
        b.Label("move").Address(1).Call(N117_ProjectileMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(1).Call(N117_ProjectileDispose).Op(0xDC);
        b.Label("after").Load(3).Integer(7).Branch(0x3F,"done");
        b.Load(3).Integer(7).Op(0x59,0x6B).Float(.5f).Op(0x5A).Float(5).Op(0x58).Store(4);
        b.Arg(0).Load(3).Op(0x6B).Float(.5f).Op(0x5A).Float(1.5f).Op(0x58).Set(0x040004FC);
        b.Integer(0).Store(5).Branch(0x38,"check");
        b.Label("apply").Load(5).Integer(7).Branch(0x3C,"explode");b.Load(0).Load(5).Virtual(0x0A000201).Load(5).Load(4).Float(2).Op(0x58).Virtual(0x060003F5).Branch(0x38,"next");
        b.Label("explode").Load(0).Load(5).Virtual(0x0A000201).Virtual(0x060003F4);
        b.Label("next").Load(5).Integer(1).Op(0x58).Store(5);b.Label("check").Load(5).Load(3).Branch(0x3F,"apply");b.Label("done").Op(0x2A);
        N117_Finally(b,0x06000392,("try","finally","after"));return b.Finish();
    }
    private static byte[] N117_TurnScale()
    {
        var b=new IL();b.Ref(0x72,0x7000AE78).ArgAddress(1).Call(0x0A00002A).Ref(0x72,0x7000AE7E).ArgAddress(2).Call(0x0A00002A).Call(0x0A0000E0).Call(0x0A000010);
        b.Arg(0).Field(0x0400037D).Store(0);b.Load(0).Virtual(N117_RowCount).Arg(2).Branch(0x3E,"enum");
        b.Load(0).Arg(2).Load(0).Virtual(N117_RowCount).Arg(2).Op(0x59).Virtual(0x0A0001C9);
        b.Label("enum").Arg(0).Field(0x0400037D).Virtual(N116_RowEnum).Store(1);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(1).Call(N116_RowCurrent).Store(2);
        b.Load(2).Field(0x04000381).Store(3);b.Load(3).Virtual(N117_GridCount).Arg(1).Branch(0x3E,"fill");
        b.Load(3).Arg(1).Load(3).Virtual(N117_GridCount).Arg(1).Op(0x59).Virtual(0x0A0001CA);
        b.Label("fill").Load(2).Field(0x04000381).Store(3);b.Load(3).Virtual(N117_GridCount).Arg(1).Branch(0x3C,"move");
        b.Load(3).Load(3).Virtual(N117_GridCount).Load(2).Field(0x04000380).New(0x06000293).Virtual(0x0A0001B9).Branch(0x38,"fill");
        b.Label("move").Address(1).Call(N116_RowMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(1).Call(N116_RowDispose).Op(0xDC);
        b.Label("after").Arg(0).Field(0x0400037D).Store(0);b.Load(0).Virtual(N117_RowCount).Arg(2).Branch(0x3C,"done");
        b.Load(0).Arg(1).Load(0).Virtual(N117_RowCount).New(0x06000292).Virtual(N117_RowAdd).Branch(0x38,"after");
        b.Label("done").Op(0x2A);N117_Finally(b,0x06000291,("try","finally","after"));return b.Finish();
    }
    private static byte[] N117_ViewWave()
    {
        var b=new IL();b.Arg(0).Arg(1).Set(0x04000795);b.Integer(0).Store(0);
        b.Arg(0).Field(0x04000785).Virtual(N117_EnemyPreviewEnum).Store(1);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(1).Call(N117_EnemyPreviewCurrent).Store(2);
        b.Load(2).Field(N117_PreviewWave).Arg(0).Field(0x04000795).Branch(0x3B,"visible");
        b.Load(2).Virtual(0x0A000009).Integer(0).Virtual(0x0A000007).Branch(0x38,"move");
        b.Label("visible").Load(2).Virtual(0x0A000009).Integer(1).Virtual(0x0A000007);b.Integer(1).Store(0);
        b.Label("move").Address(1).Call(N117_EnemyPreviewMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(1).Call(N117_EnemyPreviewDispose).Op(0xDC);
        b.Label("after").Arg(0).Field(0x04000797).Store(3);b.Arg(0).Field(0x04000795).Integer(0).Op(0xFE,0x02).Store(4);
        b.Load(3).Virtual(0x0A000009).Load(4).Virtual(0x0A000007);
        b.Arg(0).Field(0x04000781).Store(5);b.Arg(0).Field(0x04000796).Store(3);
        b.Arg(0).Field(0x04000795).Load(5).Field(0x0400019A).Op(0xFE,0x04).Store(4);b.Load(3).Virtual(0x0A000009).Load(4).Virtual(0x0A000007);
        b.Arg(0).Field(0x04000798).Store(6);b.Arg(0).Field(0x04000795).Integer(0).Branch(0x3E,"initial");
        b.Arg(0).Field(0x04000795).Integer(1).Op(0x59).Integer(10).Op(0x5B).Integer(1).Op(0x58).Store(7);
        b.Arg(0).Field(0x04000795).Integer(1).Op(0x59).Integer(10).Op(0x5D).Integer(1).Op(0x58).Store(8);
        b.Address(7).Ref(0x72,0x7001A535).Call(0x0A000082).Address(8).Ref(0x72,0x7001A541).Call(0x0A000082).Call(0x0A000051).Store(9).Branch(0x38,"text");
        b.Label("initial").Ref(0x72,0x7001A547).Store(9);
        b.Label("text").Load(6).Load(9).Integer(1).Virtual(0x0A00004B).Load(0).Op(0x2A);
        N117_Finally(b,0x060005DC,("try","finally","after"));return b.Finish();
    }
    private static byte[] N117_Talents()
    {
        var b=new IL();b.Arg(0).Arg(1).Set(0x0400073B);b.Arg(1).Virtual(0x060004D1).Store(0);b.Arg(1).Virtual(0x060004CF).Store(1);
        b.Integer(0).Store(2);b.Label("loop").Arg(0).Field(0x04000739).Load(2).Integer(0).Virtual(0x0A000274);
        b.Arg(0).Field(0x04000737).Load(2).Virtual(0x0A000271).Integer(0).Virtual(0x0A000018);
        b.Load(0).Load(2).Branch(0x3D,"available");
        b.Arg(0).Field(0x04000737).Load(2).Virtual(0x0A000271).Virtual(0x0A000009).Integer(0).Virtual(0x0A000007);
        b.Arg(0).Field(0x04000737).Load(2).Virtual(0x0A000271).Integer(0).Virtual(0x0A000208).Branch(0x38,"next");
        b.Label("available").Arg(0).Field(0x04000737).Load(2).Virtual(0x0A000271).Virtual(0x0A000009).Integer(1).Virtual(0x0A000007);
        b.Load(1).Load(2).Branch(0x3D,"unlocked");
        b.Arg(0).Field(0x04000738).Load(2).Virtual(0x0A000060).Ref(0x72,0x700176D5).Integer(1).Virtual(0x0A00004B);
        b.Arg(0).Field(0x04000737).Load(2).Virtual(0x0A000271).Integer(0).Virtual(0x0A000208);
        b.Arg(0).Field(0x04000738).Load(2).Virtual(0x0A000060).Float(0).Float(0).Float(0).Float(.75f).New(0x0A0000BE).Virtual(0x0A0001DC).Branch(0x38,"next");
        b.Label("unlocked").Arg(0).Field(0x04000738).Load(2).Virtual(0x0A000060).Arg(1).Field(0x0400065A).Load(2).Op(0x9A).Integer(1).Virtual(0x0A00004B);
        b.Arg(0).Field(0x04000737).Load(2).Virtual(0x0A000271).Integer(1).Virtual(0x0A000208);
        b.Arg(0).Field(0x04000737).Load(2).Virtual(0x0A000271).Integer(1).Virtual(0x0A000018);
        b.Arg(0).Field(0x04000738).Load(2).Virtual(0x0A000060).Float(1).Float(1).Float(1).Float(1).New(0x0A0000BE).Virtual(0x0A0001DC);
        b.Label("next").Load(2).Integer(1).Op(0x58).Store(2);b.Load(2).Integer(3).Branch(0x3F,"loop");
        b.Arg(0).Field(0x0400073A).Ref(0x72,0x700196D1).Integer(1).Virtual(0x0A00004B);b.Arg(0).Integer(0).Call(0x0600059A).Op(0x2A);return b.Finish();
    }
    private static byte[] N117_SkillUp()
    {
        var b=new IL();b.Arg(0).Field(0x04000717).Virtual(N116_SuppliesItemEnum).Store(0);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(0).Call(N116_SuppliesItemCurrent).Store(1);
        b.Load(1).Field(0x04000590).Op(0x25).Field(0x0400067A).Load(1).Field(0x04000591).Op(0x59).Set(0x0400067A);
        b.Label("move").Address(0).Call(N116_SuppliesItemMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(0).Call(N116_SuppliesItemDispose).Op(0xDC);
        b.Label("after").Static(0x0400015E).Field(0x04000166).Field(0x04000310).Op(0x25).Field(0x0400063B).Arg(0).Field(0x0400071B).Op(0x59).Set(0x0400063B);
        b.Arg(0).Field(0x040006ED).Static(0x04000720).Integer(0).Virtual(0x0A0001F7);
        b.Arg(0).Field(0x040006F9).Store(2);b.Load(2).Field(0x04000725).Load(2).Field(0x04000732).Virtual(0x060004CB).Store(3);
        b.Load(2).Field(0x04000725).Op(0x25).Field(0x04000663).Integer(1).Op(0x58).Set(0x04000663);
        b.Static(0x0400015E).Field(0x04000166).Load(2).Field(0x04000725).Load(2).Field(0x04000725).Field(0x04000663).Virtual(0x0600024E);
        b.Load(2).Load(3).Float(1).Float(1).Float(1).Float(1).New(0x0A0000BE).Virtual(0x06000591);
        b.Load(2).Field(0x0400072F).Load(2).Field(0x04000725).Store(4);
        b.Load(4).Field(0x04000663).Load(4).Field(0x04000661).Load(4).Field(0x0400065D).Integer(1).Virtual(0x060005F2);
        b.Arg(0).Field(0x04000705).Integer(1).Virtual(0x0A000007);
        b.Arg(0).Field(0x04000713).Arg(0).Field(0x040006F5).Virtual(0x060004D2).Integer(0).Op(0xFE,0x01).Virtual(0x0A000018);
        b.Arg(0).Arg(0).Field(0x040006F5).Field(0x04000663).Call(0x06000584).Op(0x2A);
        N117_Finally(b,0x06000574,("try","finally","after"));return b.Finish();
    }
    private static byte[] N117_HardLocked()
    {
        var b=new IL();b.Integer(999999).Store(0);
        foreach(var p in new[]{(Level:1,Need:21),(Level:2,Need:21),(Level:3,Need:41),(Level:4,Need:41)})
        {b.Arg(0).Field(0x04000842).Integer(p.Level).Branch(0x40,"level"+p.Level).Integer(p.Need).Store(0).Branch(0x38,"compare").Label("level"+p.Level);}
        b.Label("compare").Arg(1).Field(0x04000628).Load(0).Branch(0x3C,"unlocked");
        b.Arg(0).Field(0x0400083A).Virtual(0x0A000009).Integer(1).Virtual(0x0A000007);
        b.Arg(0).Field(0x0400083A).Ref(0x72,0x70004533).Integer(1).Virtual(0x0A00004B);
        b.Arg(0).Field(0x0400083F).Virtual(0x0A000009).Integer(1).Virtual(0x0A000007);
        b.Arg(0).Field(0x0400083B).Integer(0).Virtual(0x0A000018);
        foreach(var p in new[]{(View:0x0400083C,Images:0x040002E0),(View:0x0400083D,Images:0x040002E1),(View:0x0400083E,Images:0x040002E2)})
            b.Arg(0).Field(p.View).Static(p.Images).Integer(0).Virtual(0x0A000040).Virtual(0x0A000041);
        b.Integer(1).Op(0x2A);
        b.Label("unlocked").Arg(0).Field(0x0400083A).Virtual(0x0A000009).Integer(1).Virtual(0x0A000007);
        b.Arg(0).Field(0x04000839).Field(0x040006AE).Virtual(N117_StringCount).Arg(0).Field(0x04000842).Branch(0x3E,"emptyTitle");
        b.Arg(0).Field(0x04000839).Field(0x040006AE).Arg(0).Field(0x04000842).Virtual(0x0A000293).Store(1).Branch(0x38,"title");
        b.Label("emptyTitle").Ref(0x72,0x70004533).Store(1);
        b.Label("title").Arg(0).Field(0x0400083A).Load(1).Integer(1).Virtual(0x0A00004B);
        b.Arg(0).Field(0x0400083F).Virtual(0x0A000009).Integer(0).Virtual(0x0A000007);b.Arg(0).Field(0x0400083B).Integer(1).Virtual(0x0A000018);
        b.Arg(0).Field(0x04000842).Store(2);b.Arg(0).Load(2).Set(0x04000842);
        foreach(var p in new[]{(View:0x0400083C,Images:0x040002E0),(View:0x0400083D,Images:0x040002E1),(View:0x0400083E,Images:0x040002E2)})
            b.Arg(0).Field(p.View).Static(p.Images).Load(2).Virtual(0x0A000040).Virtual(0x0A000041);
        b.Arg(0).Field(0x04000840).Virtual(0x0A000009).Integer(0).Virtual(0x0A000007).Integer(0).Op(0x2A);return b.Finish();
    }
    private static byte[] N117_ChallengeAwake()
    {
        var b=new IL();b.Integer(0).Store(0);b.Integer(0).Store(1).Branch(0x38,"check");
        b.Label("loop").Arg(0).Field(0x040007CF).Store(2);b.Load(1).Integer(5).Op(0x5D).Store(3);b.Load(1).Integer(5).Op(0x5B).Store(4);
        b.Load(2).Arg(0).Field(0x040007CE).Virtual(0x0A00002E).Call(0x2B000085).Store(5);
        b.Load(5).Integer(1).Virtual(0x0A000007);
        b.Load(5).Virtual(0x0A00002E).Load(3).Integer(160).Op(0x5A,0x6B).Load(4).Integer(-140).Op(0x5A,0x6B).Float(0).New(0x0A00009C).Virtual(0x0A000042);
        b.Load(5).Virtual(0x2B000086).Arg(0).Field(0x040007CD).Field(0x0400011A).Load(1).Virtual(0x0A00028B).Field(0x0400011C).Virtual(0x0A000041);
        b.Load(5).Virtual(0x2B000087).Arg(0).Field(0x040007CD).Field(0x0400011A).Load(1).Virtual(0x0A00028B).Field(0x0400011B).Virtual(0x0A000062);
        b.Load(1).Integer(1).Op(0x58).Store(1);
        b.Label("check").Load(1).Arg(0).Field(0x040007CD).Field(0x0400011A).Virtual(N117_ChallengeCount).Branch(0x3F,"loop");
        b.Arg(0).Field(0x040007CD).Field(0x0400011A).Virtual(N117_ChallengeCount).Integer(4).Op(0x58).Integer(5).Op(0x5B).Integer(140).Op(0x5A).Store(6);
        b.Arg(0).Field(0x040007CE).Virtual(0x0A00002E).Ref(0x74,0x01000056).Store(7);
        b.Load(7).Virtual(0x0A00028D).Store(8);b.Address(8).Load(6).Op(0x6B).Set(0x0A0001D5);b.Load(7).Load(8).Virtual(0x0A00028E);
        b.Arg(0).Field(0x040007D0).Store(9);b.Integer(0).Store(10);b.Address(10).Call(0x0A00002A).Store(11);
        b.Arg(0).Field(0x040007CD).Field(0x0400011A).Virtual(N117_ChallengeCount).Store(10);
        b.Load(9).Load(11).Ref(0x72,0x7001A557).Address(10).Call(0x0A00002A).Call(0x0A00010B).Virtual(0x0A000062).Op(0x2A);return b.Finish();
    }
    private static void N117_WindowTip(IL b,int literal)
    {
        b.Arg(0).Field(0x040008FA).Ref(0x72,literal).Integer(1).Virtual(0x0A00004B);
        b.Static(0x040002EC).Integer(2).Virtual(0x0A000084).Call(0x0600013B);
    }
    private static byte[] N117_RenamePlayer()
    {
        var b=new IL();b.Static(0x0400015E).Field(0x04000166).Store(0);b.Load(0).Branch(0x39,"done");
        b.Arg(0).Field(0x040008FE).Static(0x0A000013).Call(0x0A00003A).Branch(0x3A,"empty");
        b.Load(0).Field(0x04000310).Store(1);b.Load(0).Field(0x04000311).Field(0x0400064E).Virtual(N117_SaveEnum).Store(2);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(2).Call(N117_SaveCurrent).Store(3);
        b.Load(3).Load(1).Branch(0x3B,"move");b.Load(3).Field(0x04000627).Arg(0).Field(0x040008FE).Call(0x0A00003A).Branch(0x39,"move");
        N117_WindowTip(b,0x7001CE74);b.Branch(0xDD,"done");
        b.Label("move").Address(2).Call(N117_SaveMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(2).Call(N117_SaveDispose).Op(0xDC);
        b.Label("after").Arg(0).Field(0x040008FE).Load(1).Field(0x04000627).Call(0x0A00003A).Branch(0x3A,"unchanged");
        b.Load(0).Arg(0).Field(0x040008FE).Virtual(0x0600024A);b.Arg(0).Virtual(0x0A000009).Call(0x0A000025).Branch(0x38,"done");
        b.Label("unchanged");N117_WindowTip(b,0x7001CF22);b.Branch(0x38,"done");
        b.Label("empty");N117_WindowTip(b,0x7001CF12);b.Label("done").Op(0x2A);
        N117_Finally(b,0x060006F9,("try","finally","after"));return b.Finish();
    }
    private static byte[] N117_SuppliesInfo()
    {
        var b=new IL();b.Arg(0).Field(0x040008EF).Store(0);b.Arg(0).Field(0x040008EE).Branch(0x39,"empty");
        b.Load(0).Arg(0).Field(0x040008EE).Field(0x04000174).Integer(1).Virtual(0x0A00004B);
        foreach(var p in new[]{(Text:0x040008F0,Content:0x04000179),(Text:0x040008F1,Content:0x04000178),(Text:0x040008F2,Content:0x0400017A)})
            b.Arg(0).Field(p.Text).Arg(0).Field(0x040008EE).Field(p.Content).Integer(1).Virtual(0x0A00004B);
        b.Integer(0).Store(1).Branch(0x38,"starCheck");
        b.Label("star").Arg(0).Field(0x040008EE).Field(0x04000176).Store(2);
        b.Arg(0).Field(0x040008F3).Load(1).Virtual(0x0A000157).Store(3);
        b.Load(1).Load(2).Branch(0x3F,"white");
        b.Load(3).Float(0).Float(0).Float(0).Float(.75f).New(0x0A0000BE).Virtual(0x0A0001DE).Branch(0x38,"nextStar");
        b.Label("white").Load(3).Float(1).Float(1).Float(1).Float(1).New(0x0A0000BE).Virtual(0x0A0001DE);
        b.Label("nextStar").Load(1).Integer(1).Op(0x58).Store(1);
        b.Label("starCheck").Load(1).Arg(0).Field(0x040008F3).Virtual(N117_ImageCount).Branch(0x3F,"star");
        b.Arg(0).Field(0x040008F4).Virtual(0x0A000009).Integer(1).Virtual(0x0A000007);
        b.Arg(0).Field(0x040008F4).Arg(0).Field(0x040008EE).Field(0x04000175).Call(0x0600022C).Virtual(0x0A000041).Branch(0x38,"done");
        b.Label("empty").Load(0).Static(0x0A000013).Integer(1).Virtual(0x0A00004B);
        foreach(int t in new[]{0x040008F0,0x040008F1,0x040008F2})b.Arg(0).Field(t).Static(0x0A000013).Integer(1).Virtual(0x0A00004B);
        b.Arg(0).Field(0x040008F3).Virtual(N117_ImageEnum).Store(4);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(4).Call(N117_ImageCurrent).Float(0).Float(0).Float(0).Float(.75f).New(0x0A0000BE).Virtual(0x0A0001DE);
        b.Label("move").Address(4).Call(N117_ImageMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(4).Call(N117_ImageDispose).Op(0xDC);
        b.Label("after").Arg(0).Field(0x040008F4).Virtual(0x0A000009).Integer(0).Virtual(0x0A000007);b.Label("done").Op(0x2A);
        N117_Finally(b,0x060006F1,("try","finally","after"));return b.Finish();
    }
    private static byte[] N117_ChangeSequence()
    {
        var b=new IL();b.Arg(0).Op(0x14).Set(0x04000A72);
        b.Arg(0).Field(0x04000A7A).Call(0x0A00000B).Branch(0x39,"dirty");
        b.Arg(0).Field(0x04000A7A).Field(0x04000A81).Branch(0x39,"dirty");
        b.Arg(0).Field(0x04000A7B).Call(0x0A00000F).Branch(0x3A,"fallbackCheck");
        b.Arg(0).Field(0x04000A7A).Field(0x04000A81).Virtual(N117_SequenceCount).Store(1);b.Integer(0).Store(0).Branch(0x38,"namedCheck");
        b.Label("named").Arg(0).Field(0x04000A7A).Field(0x04000A81).Load(0).Virtual(0x0A00039C).Store(2);
        b.Load(2).Branch(0x39,"namedNext");b.Load(2).Field(0x04000A8D).Arg(0).Field(0x04000A7B).Call(0x0A00003A).Branch(0x39,"namedNext");
        b.Arg(0).Load(2).Set(0x04000A72).Branch(0x38,"namedDone");
        b.Label("namedNext").Load(0).Integer(1).Op(0x58).Store(0);b.Label("namedCheck").Load(0).Load(1).Branch(0x3F,"named");
        b.Label("namedDone").Arg(0).Field(0x04000A72).Branch(0x3A,"dirty");
        b.Integer(1).Ref(0x8D,0x0100001A).Store(3);b.Load(3).Integer(0).Arg(0).Field(0x04000A7B).Op(0xA2);
        b.Arg(0).Ref(0x72,0x7002B895).Load(3).Call(0x0A00039D);
        b.Label("fallbackCheck").Arg(0).Field(0x04000A72).Branch(0x3A,"dirty");
        b.Arg(0).Field(0x04000A7A).Field(0x04000A81).Virtual(N117_SequenceCount).Store(1);b.Integer(0).Store(0).Branch(0x38,"fallbackCount");
        b.Label("fallback").Arg(0).Field(0x04000A7A).Field(0x04000A81).Load(0).Virtual(0x0A00039C).Store(2);
        b.Load(2).Branch(0x39,"fallbackNext");
        b.Arg(0).Load(2).Field(0x04000A8D).Set(0x04000A7B);b.Arg(0).Load(2).Set(0x04000A72).Branch(0x38,"dirty");
        b.Label("fallbackNext").Load(0).Integer(1).Op(0x58).Store(0);b.Label("fallbackCount").Load(0).Load(1).Branch(0x3F,"fallback");
        b.Label("dirty").Arg(0).Integer(1).Set(0x04000A71);
        b.Arg(0).Field(0x04000A72).Branch(0x39,"zero");b.Arg(0).Field(0x04000A72).Field(0x04000A8E).Branch(0x39,"zero");
        b.Arg(0).Field(0x04000A72).Field(0x04000A8E).Virtual(N117_FrameCount).Integer(0).Branch(0x3E,"zero");
        b.Arg(0).Field(0x04000A7C).Store(0);
        b.Arg(0).Field(0x04000A72).Branch(0x39,"noFrames");b.Arg(0).Field(0x04000A72).Field(0x04000A8E).Branch(0x39,"noFrames");
        b.Arg(0).Field(0x04000A72).Field(0x04000A8E).Virtual(N117_FrameCount).Store(1).Branch(0x38,"clamp");
        b.Label("noFrames").Integer(0).Store(1);
        b.Label("clamp").Load(1).Integer(1).Op(0x59).Store(1);b.Load(0).Integer(0).Branch(0x3F,"zero");
        b.Load(0).Load(1).Branch(0x3E,"store");b.Load(1).Store(0).Branch(0x38,"store");
        b.Label("zero").Integer(0).Store(0);b.Label("store").Arg(0).Load(0).Set(0x04000A7C).Op(0x2A);return b.Finish();
    }
    private static byte[] N117_CloneTeam()
    {
        var b=new IL();b.Arg(0).ArgAddress(1).Field(0x04000648).Set(0x04000648);b.Arg(0).ArgAddress(1).Field(0x04000649).Set(0x04000649);
        foreach(var p in new[]{(Field:0x0400064A,Tag:"ids"),(Field:0x0400064B,Tag:"skills"),(Field:0x0400064C,Tag:"props")})
        {
            b.Arg(0).Field(p.Field).Virtual(N117_IntClear);
            b.ArgAddress(1).Field(p.Field).Virtual(N116_IntEnum).Store(0);
            b.Label("try"+p.Tag).Branch(0x38,"move"+p.Tag);
            b.Label("body"+p.Tag).Address(0).Call(N116_IntCurrent).Store(1);
            b.Arg(0).Field(p.Field);if(p.Field==0x0400064C)b.Arg(0).Field(0x04000649);else b.Load(1);
            // Original props loop appends this.id, not the enumerated prop. Preserve this observed behavior.
            b.Virtual(0x0A000291);
            b.Label("move"+p.Tag).Address(0).Call(N116_IntMove).Branch(0x3A,"body"+p.Tag).Branch(0xDD,"after"+p.Tag);
            b.Label("finally"+p.Tag).Address(0).Call(N116_IntDispose).Op(0xDC);b.Label("after"+p.Tag);
        }
        b.Op(0x2A);N117_Finally(b,0x060004C8,("tryids","finallyids","afterids"),("tryskills","finallyskills","afterskills"),("tryprops","finallyprops","afterprops"));return b.Finish();
    }
    private static byte[] N117_SetupPreDevices()
    {
        var b=new IL();b.New(N117_EntryListCtor).Store(0);b.Arg(1).Field(0x040001A0).Virtual(N116_BoardEntryEnum).Store(1);
        b.Label("tryEntries").Branch(0x38,"moveEntries");b.Label("entry").Address(1).Call(N116_BoardEntryCurrent).Store(2);
        b.Load(2).Field(0x040007FA).Integer(20).Branch(0x40,"entry21");
        b.Load(2).Field(0x040007FC).Branch(0x3A,"moveEntries");b.Load(2).Field(0x040007FD).Branch(0x3A,"moveEntries");b.Load(0).Load(2).Virtual(N117_EntryAdd);
        b.Label("entry21").Load(2).Field(0x040007FA).Integer(21).Branch(0x40,"entry24");
        b.Load(2).Field(0x040007FC).Branch(0x3A,"add21");b.Load(2).Field(0x040007FD).Branch(0x39,"moveEntries");
        b.Label("add21").Load(0).Load(2).Virtual(N117_EntryAdd);
        b.Label("entry24").Load(2).Field(0x040007FA).Integer(24).Branch(0x40,"moveEntries");
        b.Ref(0x72,0x70005201).Call(0x0A000010);b.Load(0).Load(2).Virtual(N117_EntryAdd);
        b.Label("moveEntries").Address(1).Call(N116_BoardEntryMove).Branch(0x3A,"entry").Branch(0xDD,"afterEntries");
        b.Label("finallyEntries").Address(1).Call(N116_BoardEntryDispose).Op(0xDC);
        b.Label("afterEntries").Arg(1).Field(0x0400019E).Virtual(N116_PreDeviceEnum).Store(3);
        b.Label("tryPre").Branch(0x38,"movePre");b.Label("pre").Address(3).Call(N116_PreDeviceCurrent).Store(4);
        b.Load(4).Field(0x040001D2).Static(0x0A000013).Call(0x0A0000B0).Branch(0x39,"appendPre");
        b.Load(0).Load(4).Field(0x040001D2).Call(0x0600060A).Branch(0x39,"appendPre");
        b.Ref(0x72,0x70005211).Call(0x0A000010).Branch(0x38,"movePre");
        b.Label("appendPre").Arg(0).Field(0x040001D7).Load(4).Virtual(N117_PreDeviceAdd);
        b.Label("movePre").Address(3).Call(N116_PreDeviceMove).Branch(0x3A,"pre").Branch(0xDD,"afterPre");
        b.Label("finallyPre").Address(3).Call(N116_PreDeviceDispose).Op(0xDC);
        b.Label("afterPre").Arg(2).Field(0x0400037E).Integer(8).Branch(0x40,"scenario10");b.Arg(0).Arg(1).Arg(2).Call(0x0600018D);
        b.Label("scenario10").Arg(2).Field(0x0400037E).Integer(10).Branch(0x40,"done");b.Arg(0).Arg(1).Arg(2).Call(0x0600018D);
        b.Label("done").Op(0x2A);N117_Finally(b,0x0600018B,("tryEntries","finallyEntries","afterEntries"),("tryPre","finallyPre","afterPre"));return b.Finish();
    }
    private static byte[] N117_DevicePreviews()
    {
        var b=new IL();b.Arg(0).Call(0x06000228).Set(0x04000787);b.New(N117_EntryListCtor).Store(0);b.Arg(0).Field(0x04000781).Field(0x040001A0).Virtual(N116_BoardEntryEnum).Store(1);
        b.Label("tryEntries").Branch(0x38,"moveEntries");b.Label("entry").Address(1).Call(N116_BoardEntryCurrent).Store(2);
        b.Load(2).Field(0x040007FA).Integer(20).Branch(0x40,"entry21");
        b.Load(2).Field(0x040007FC).Branch(0x3A,"moveEntries");b.Load(2).Field(0x040007FD).Branch(0x3A,"moveEntries");b.Load(0).Load(2).Virtual(N117_EntryAdd);
        b.Label("entry21").Load(2).Field(0x040007FA).Integer(21).Branch(0x40,"entry24");
        b.Load(2).Field(0x040007FC).Branch(0x3A,"add21");b.Load(2).Field(0x040007FD).Branch(0x39,"moveEntries");
        b.Label("add21").Load(0).Load(2).Virtual(N117_EntryAdd);
        b.Label("entry24").Load(2).Field(0x040007FA).Integer(24).Branch(0x40,"moveEntries");
        b.Load(0).Load(2).Virtual(N117_EntryAdd);
        b.Label("moveEntries").Address(1).Call(N116_BoardEntryMove).Branch(0x3A,"entry").Branch(0xDD,"afterEntries");
        b.Label("finallyEntries").Address(1).Call(N116_BoardEntryDispose).Op(0xDC);
        b.Label("afterEntries").Arg(1).Field(0x0400019E).Virtual(N116_PreDeviceEnum).Store(3);
        b.Label("tryPre").Branch(0x38,"movePre");b.Label("pre").Address(3).Call(N116_PreDeviceCurrent).Store(4);
        b.Load(4).Field(0x040001D2).Static(0x0A000013).Call(0x0A0000B0).Branch(0x39,"appendPre");
        b.Load(0).Load(4).Field(0x040001D2).Call(0x0600060A).Branch(0x39,"appendPre");
        b.Ref(0x72,0x70005211).Call(0x0A000010).Branch(0x38,"movePre");
        b.Label("appendPre").Arg(0).Field(0x040001D7).Load(4).Virtual(N117_PreDeviceAdd);
        b.Label("movePre").Address(3).Call(N116_PreDeviceMove).Branch(0x3A,"pre").Branch(0xDD,"afterPre");
        b.Label("finallyPre").Address(3).Call(N116_PreDeviceDispose).Op(0xDC);
        b.Label("afterPre").Op(0x2A);N117_Finally(b,0x060005D1,("tryEntries","finallyEntries","afterEntries"),("tryPre","finallyPre","afterPre"));return b.Finish();
    }
    private static byte[] N117_CreateLootList()
    {
        var b=new IL();b.New(0x0A00011E).Store(0);b.Arg(1).Field(0x0400019C).Virtual(N117_LootEnum).Store(1);
        b.Label("tryLoot").Branch(0x38,"moveLoot");b.Label("loot").Address(1).Call(N117_LootCurrent).Store(2);
        b.Load(2).Field(0x040001AB).Branch(0x39,"drop");
        b.Static(0x0400015E).Field(0x04000166).Field(0x04000310);
        b.Static(0x0400015E).Store(3);b.Load(3).Field(0x04000167).Load(3).Field(0x04000168).Virtual(0x060004C5).Branch(0x3A,"moveLoot");
        b.Label("drop").Load(2).Float(0).Virtual(0x06000168).Store(4);b.Load(4).Branch(0x39,"moveLoot");b.Load(0).Load(4).Virtual(0x0A00027F);
        b.Label("moveLoot").Address(1).Call(N117_LootMove).Branch(0x3A,"loot").Branch(0xDD,"afterLoot");
        b.Label("finallyLoot").Address(1).Call(N117_LootDispose).Op(0xDC);
        b.Label("afterLoot").Arg(1).Field(0x040001A0).Virtual(N116_BoardEntryEnum).Store(5);
        b.Label("tryEntry").Branch(0x38,"moveEntry");b.Label("entry").Address(5).Call(N116_BoardEntryCurrent).Store(6);
        b.Load(6).Field(0x040007FC).Branch(0x3A,"entryDrop");b.Load(6).Field(0x040007FD).Branch(0x39,"moveEntry");
        b.Label("entryDrop").Load(6).Arg(1).Virtual(0x06000609).Float(0).Virtual(0x06000168).Store(4);
        b.Load(4).Branch(0x39,"moveEntry");b.Load(0).Load(4).Virtual(0x0A00027F);
        b.Label("moveEntry").Address(5).Call(N116_BoardEntryMove).Branch(0x3A,"entry").Branch(0xDD,"afterEntry");
        b.Label("finallyEntry").Address(5).Call(N116_BoardEntryDispose).Op(0xDC);
        b.Label("afterEntry").Arg(2).Load(0).Virtual(0x06000163);b.Arg(0).Load(0).Call(0x060006D2).Op(0x2A);
        N117_Finally(b,0x060006D0,("tryLoot","finallyLoot","afterLoot"),("tryEntry","finallyEntry","afterEntry"));return b.Finish();
    }
    private static byte[] N117_LoadAlmanacPlants()
    {
        var b=new IL();b.Static(0x040002FC).Store(0);b.Load(0).Call(0x0A00000B).Branch(0x39,"missing");
        b.Arg(0).Field(0x040006BF).Field(0x04000639).Virtual(N116_PlantSaveEnum).Store(1);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(1).Call(N116_PlantSaveCurrent).Store(2);
        b.Load(0).Call(0x2B00007D).Store(3);b.Load(3).Arg(0).Set(0x04000446);
        b.Load(3).Virtual(0x0A000001).Arg(0).Field(0x040006C7).Virtual(0x0A00002E).Integer(0).Virtual(0x0A00002F);
        b.Arg(0).Field(0x040006C8).Virtual(N117_CardCount).Store(4);
        b.Load(4).Integer(2).Op(0x5B,0x6B).Float(240).Op(0x5A).Float(800).Op(0x59).Store(5);
        b.Float(170).Load(4).Integer(2).Op(0x5D,0x6B).Float(360).Op(0x5A,0x59).Store(6);
        b.Load(3).Virtual(0x0A000001).Load(5).Load(6).Float(0).New(0x0A00009C).Virtual(0x0A000042);
        b.Load(3).Load(2).Virtual(0x060002F2);b.Arg(0).Field(0x040006C8).Load(3).Virtual(N117_CardAdd);
        b.Label("move").Address(1).Call(N116_PlantSaveMove).Branch(0x3A,"body").Branch(0xDD,"done");
        b.Label("finally").Address(1).Call(N116_PlantSaveDispose).Op(0xDC);b.Label("done").Op(0x2A);
        b.Label("missing").Ref(0x72,0x70017234).Call(0x0A000010).Op(0x2A);
        N117_Finally(b,0x06000547,("try","finally","done"));return b.Finish();
    }
    private static byte[] N117_AlmanacHome(bool zombie)
    {
        var b=new IL();int prefab=zombie?0x04000308:0x04000309,portraits=zombie?0x0400075E:0x040006E2;
        int content=zombie?0x0400075F:0x040006E3,home=zombie?0x04000767:0x040006E5,image=zombie?0x0400076A:0x040006E8;
        b.Static(prefab).Store(0);b.Static(0x0400015E).Field(0x04000166).Field(0x04000310).Store(1);
        b.Load(1).Branch(0x39,"done");b.Arg(0).Call(zombie?0x06000232:0x06000227).Set(portraits);
        b.Load(0).Op(0x14).Call(0x0A000012).Branch(0x3A,"done");
        b.Ref(0xD0,zombie?0x0200005D:0x0200003D).Call(0x0A00004C).Call(0x0A00007D).Virtual(0x0A0000EB).Store(2);
        b.Label("try").Branch(0x38,"move");b.Label("body").Load(2).Virtual(0x0A0000ED).Ref(0xA5,zombie?0x0200005D:0x0200003D).Store(4);
        if(!zombie)b.Load(4).Integer(0).Branch(0x3F,"move");
        b.Load(0).Call(zombie?0x2B000083:0x2B00007E).Store(5);
        b.Arg(0).Field(portraits).Virtual(N117_SpriteCount).Load(4).Branch(0x3E,"noPortrait");
        b.Load(5).Field(image).Arg(0).Field(portraits).Load(4).Virtual(0x0A000040).Virtual(0x0A000041);
        b.Label("noPortrait").Load(5).Virtual(0x0A000001).Arg(0).Field(content).Virtual(0x0A00002E).Integer(0).Virtual(0x0A00002F);
        b.Load(5).Arg(0).Set(home);b.Load(5).Load(1).Load(4).Virtual(zombie?0x060005B9:0x06000569);
        b.Arg(0).Field(zombie?0x0400075D:0x040006E1).Load(5).Virtual(zombie?0x0A000278:0x0A000259);
        b.Label("move").Load(2).Virtual(0x0A0000EC).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally");N112_DisposeEnumerator(b,2,3);b.Op(0xDC);b.Label("after").Arg(0).Call(zombie?0x060005AC:0x06000562);
        if(zombie)foreach(var p in new[]{(Toggle:0x04000761,Value:0x04000764),(Toggle:0x04000762,Value:0x04000765),(Toggle:0x04000763,Value:0x04000766)})
            b.Arg(0).Field(p.Toggle).Arg(0).Field(p.Value).Virtual(0x0A000017);
        b.Label("done").Op(0x2A);N117_Finally(b,zombie?0x060005A9:0x0600055D,("try","finally","after"));return b.Finish();
    }
    private static void N117_Color(IL b,params uint[] bits)
    {
        foreach(uint value in bits)b.Float(BitConverter.Int32BitsToSingle(unchecked((int)value)));
        b.New(0x0A0000BE);
    }
    private static byte[] N117_TestSupplied()
    {
        var b=new IL();b.Integer(1).Store(0);b.Arg(0).Field(0x04000717).Virtual(N116_SuppliesItemEnum).Store(1);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(1).Call(N116_SuppliesItemCurrent).Store(2);
        b.Load(2).Field(0x04000597).Virtual(0x0A000009).Integer(1).Virtual(0x0A000007);
        b.Load(2).Field(0x04000598).Load(2).FieldAddress(0x04000591).Call(0x0A00002A).Integer(1).Virtual(0x0A00004B);
        b.Load(2).Field(0x04000597).Virtual(0x0A00023D).Store(3);b.Load(2).Field(0x04000591).Op(0x6C).Call(0x0A00023E).Op(0x69).Integer(14).Op(0x5A).Integer(39).Op(0x58,0x6B).Store(4);
        b.Load(3).Integer(0).Load(4).Virtual(0x0A00023F);
        b.Load(2).Field(0x04000590).Field(0x0400067A).Load(2).Field(0x04000591).Branch(0x3C,"supplied");
        b.Load(2).Field(0x04000597);N117_Color(b,N117_InsufficientColor);b.Virtual(0x0A0001DE);b.Integer(0).Store(0).Branch(0x38,"move");
        b.Label("supplied").Load(2).Field(0x04000597);N117_Color(b,N117_SufficientColor);b.Virtual(0x0A0001DE);
        b.Label("move").Address(1).Call(N116_SuppliesItemMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(1).Call(N116_SuppliesItemDispose).Op(0xDC);
        b.Label("after").Arg(0).Static(0x0400015E).Field(0x04000166).Field(0x04000310).Field(0x0400063B).Set(0x04000719);
        b.Arg(0).Field(0x0400071C).Store(5);b.Arg(0).Field(0x0400071B).Arg(0).Field(0x04000719).Branch(0x3D,"coinsMissing");
        b.Load(5);N117_Color(b,N117_SufficientColor);b.Virtual(0x0A0001DC).Branch(0x38,"texts");
        b.Label("coinsMissing").Load(5);N117_Color(b,N117_InsufficientColor);b.Virtual(0x0A0001DC);b.Integer(0).Store(0);
        b.Label("texts").Arg(0).Field(0x0400071A).Arg(0).FieldAddress(0x04000719).Ref(0x72,0x70018DA1).Call(0x0A000082).Integer(1).Virtual(0x0A00004B);
        b.Arg(0).Field(0x0400071C).Arg(0).FieldAddress(0x0400071B).Ref(0x72,0x70018DA1).Call(0x0A000082).Integer(1).Virtual(0x0A00004B).Load(0).Op(0x2A);
        N117_Finally(b,0x06000585,("try","finally","after"));return b.Finish();
    }
    private static void N117_MouseDistance(IL b,int componentLocal)
    {
        b.Arg(0).Field(0x0400023A).Store(3);b.Load(componentLocal).Virtual(0x0A000001).Virtual(0x0A000037).Store(4);
        foreach(var p in new[]{(Field:0x0A00001D,Local:5),(Field:0x0A00008B,Local:6),(Field:0x0A00009D,Local:7)})
            b.Address(3).Field(p.Field).Address(4).Field(p.Field).Op(0x59).Store(p.Local);
        // Both the inlined path and native helper 0x1802FB1B0 sum y*y + x*x + z*z in float32.
        b.Load(6).Load(6).Op(0x5A).Load(5).Load(5).Op(0x5A,0x58).Load(7).Load(7).Op(0x5A,0x58,0x6C).Call(0x0A00009F).Op(0x6B).Store(8);
    }
    private static byte[] N117_MousePlant()
    {
        var b=new IL();b.Arg(0).Field(0x04000230).Field(0x040003CF).Store(0);b.Load(0).Field(0x0400086B).Store(1);
        b.Arg(0).Call(0x060001DE).Store(2);b.Load(2).Op(0x14).Call(0x0A000012).Branch(0x3A,"return");
        b.Arg(0).Field(0x04000230).Field(0x040003D0).Branch(0x3A,"alreadyOpen");
        b.Arg(0).Field(0x04000230).Load(2).Integer(1).Virtual(0x060002BB);
        b.Arg(0).Field(0x04000230).Field(0x040003CE).Integer(3).Virtual(0x0600019C).Branch(0x38,"return");
        b.Label("alreadyOpen").Load(1).Load(2).Call(0x0A000012).Branch(0x3A,"samePlant");
        b.Load(0).Field(0x04000870).Store(9);N117_MouseDistance(b,9);b.Float(180).Load(8).Branch(0x3D,"none");
        b.Load(0).Field(0x04000871).Store(10);b.Arg(0).Field(0x0400023A).Store(3);
        b.Load(10).Integer(0).Virtual(0x0A000157).Virtual(0x0A000001).Virtual(0x0A000037).Store(4);
        N117_DistanceFromCapturedVectors(b);b.Float(60).Load(8).Branch(0x3D,"none");
        b.Load(0).Field(0x0400086F).Store(9);N117_MouseDistance(b,9);b.Float(50).Load(8).Branch(0x3D,"none");
        b.Arg(0).Field(0x04000230).Integer(1).Integer(0).Virtual(0x060002BA);
        b.Arg(0).Field(0x04000230).Load(2).Integer(0).Virtual(0x060002BB).Branch(0x38,"return");
        b.Label("samePlant").Arg(0).Field(0x04000230).Integer(1).Integer(1).Virtual(0x060002BA);
        b.Label("return").Load(2).Op(0x2A);b.Label("none").Op(0x14,0x2A);return b.Finish();
    }
    private static void N117_DistanceFromCapturedVectors(IL b)
    {
        foreach(var p in new[]{(Field:0x0A00001D,Local:5),(Field:0x0A00008B,Local:6),(Field:0x0A00009D,Local:7)})
            b.Address(3).Field(p.Field).Address(4).Field(p.Field).Op(0x59).Store(p.Local);
        b.Load(6).Load(6).Op(0x5A).Load(5).Load(5).Op(0x5A,0x58).Load(7).Load(7).Op(0x5A,0x58,0x6C).Call(0x0A00009F).Op(0x6B).Store(8);
    }
    private static byte[] N117_SnowbeastBait()
    {
        var b=new IL();b.Op(0x14).Store(0);b.Integer(int.MaxValue).Store(1);
        b.Arg(0).Field(0x040005AA).Field(0x040003D8).Field(N117_DeviceList).Virtual(N116_DeviceEnum).Store(2);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(2).Call(N116_DeviceCurrent).Store(3);
        b.Load(3).Field(0x0400046F).Integer(5).Branch(0x40,"move");
        b.Load(3).Field(0x04000464).Store(4);b.Load(3).Field(0x04000465).Store(5);
        b.Load(4).Load(5).Float(9999).Arg(0).Field(0x040005AA).Field(0x040003B4).New(0x060002A4).Store(6);
        b.Arg(0).Field(0x040005F9).Integer(0).Load(6).Virtual(N117_PathInsert);b.Arg(0).Call(0x0600046A).Branch(0x39,"remove");
        b.Arg(0).Field(0x040005FA).Arg(0).Field(0x040005AB).Arg(0).Field(0x040005AC).Call(0x060002A5).Store(7);
        b.Integer(6).Ref(0x8D,0x0100000B).Store(8);
        b.Load(8).Integer(0).Ref(0x72,0x70014900).Op(0xA2);
        b.Load(8).Integer(1).Load(3).FieldAddress(0x04000464).Call(0x0A00002A).Op(0xA2);
        b.Load(8).Integer(2).Ref(0x72,0x700044CD).Op(0xA2);
        b.Load(8).Integer(3).Load(3).FieldAddress(0x04000465).Call(0x0A00002A).Op(0xA2);
        b.Load(8).Integer(4).Ref(0x72,0x7001490A).Op(0xA2);
        b.Load(8).Integer(5).Address(7).Call(0x0A00002A).Op(0xA2);b.Load(8).Call(0x0A000230).Call(0x0A000010);
        b.Load(7).Load(1).Branch(0x3C,"remove");b.Load(7).Store(1);b.Load(3).Store(0);
        // This removal is deliberately not a finally: native exceptions leave the inserted path in place.
        b.Label("remove").Arg(0).Field(0x040005F9).Integer(0).Virtual(N117_PathRemoveAt);
        b.Label("move").Address(2).Call(N116_DeviceMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(2).Call(N116_DeviceDispose).Op(0xDC);b.Label("after").Load(0).Op(0x2A);
        N117_Finally(b,0x060004AC,("try","finally","after"));return b.Finish();
    }
    private static byte[] N117_LoadOrderData()
    {
        var b=new IL();b.Arg(0).Arg(1).Set(0x04000082);b.Arg(0).Field(0x04000083).ArgAddress(1).Call(0x0A00002A).Virtual(0x0A00002B);
        b.Arg(1).Integer(4).Branch(0x42,"empty");b.Arg(0).Field(0x0400006E).Branch(0x39,"empty");
        foreach(var p in new[]{(View:0x04000084,Data:0x0400031D,Integer:true),(View:0x04000085,Data:0x0400031E,Integer:false),(View:0x04000086,Data:0x04000326,Integer:false),(View:0x04000087,Data:0x04000323,Integer:false)})
        {
            b.Arg(0).Field(p.View).Arg(0).Field(0x0400006E).Field(p.Data).Arg(1).Ref(0x8F,p.Integer?0x0100000E:0x0100001F).Call(p.Integer?0x0A00002A:0x0A000043).Virtual(0x0A00002B);
        }
        foreach(int f in new[]{0x04000084,0x04000085,0x04000086,0x04000087})b.Arg(0).Field(f).Integer(1).Virtual(0x0A000018);
        b.Op(0x2A);b.Label("empty");foreach(int f in new[]{0x04000084,0x04000085,0x04000086,0x04000087})b.Arg(0).Field(f).Static(0x0A000013).Virtual(0x0A00002B);
        foreach(int f in new[]{0x04000084,0x04000085,0x04000086,0x04000087})b.Arg(0).Field(f).Integer(0).Virtual(0x0A000018);
        b.Op(0x2A);return b.Finish();
    }
    private static byte[] N117_EditorStart()
    {
        var b=new IL();b.Arg(0).Field(0x040000B5).Integer(1).Virtual(0x0A000018);
        b.Arg(0).Field(0x040000B4).Arg(0).Field(0x040000A8).Integer(0).Op(0xFE,0x02).Virtual(0x0A000018);
        b.Arg(0).Field(0x040000A6).Virtual(0x06000158).Field(0x0400037E).Store(0);b.Arg(0).Field(0x040000AA).Load(0).Virtual(0x0A000039);
        b.Arg(0).Field(0x040000A9).Integer(0).Virtual(0x0A000060).Ref(0x72,0x7000177D);
        b.Arg(0).FieldAddress(0x040000A7).Op(0xFE).Ref(0x16,0x02000037).Virtual(0x0A000061).Call(0x0A000051).Virtual(0x0A000062);
        b.Arg(0).Field(0x040000A9).Integer(1).Virtual(0x0A000060).Ref(0x72,0x70001789).Arg(0).FieldAddress(0x040000A8).Call(0x0A00002A).Call(0x0A000051).Virtual(0x0A000062);
        b.Arg(0).Field(0x040000AD).Arg(0).Field(0x040000A6).Virtual(0x06000158).Virtual(0x06000289).Store(1);b.Address(1).Call(0x0A00002A).Virtual(0x0A000063);
        b.Arg(0).Field(0x040000AE).Arg(0).Field(0x040000A6).Virtual(0x06000158).Field(0x0400037D).Virtual(N117_RowCount).Store(1);b.Address(1).Call(0x0A00002A).Virtual(0x0A000063);
        b.Arg(0).Field(0x040000AF).Arg(0).Field(0x040000A6).Virtual(0x06000158).FieldAddress(0x0400037F).Call(0x0A000043).Virtual(0x0A000063);
        foreach(var p in new[]{(View:0x040000B0,Field:0x04000199),(View:0x040000B1,Field:0x0400019A),(View:0x040000B2,Field:0x04000197),(View:0x040000B3,Field:0x0400019B)})
            b.Arg(0).Field(p.View).Arg(0).Field(0x040000A6).FieldAddress(p.Field).Call(0x0A00002A).Virtual(0x0A000063);
        b.Arg(0).Call(0x0600008B);b.Arg(0).Call(0x0600008A);b.Arg(0).Field(0x040000B6).Integer(0).Virtual(0x0A000007);
        b.Arg(0).Field(0x040000BA).Arg(0).Field(0x040000A6).Virtual(0x06000012).Op(0x2A);return b.Finish();
    }
    private static byte[] N117_InitializeBoardEditor()
    {
        var b=new IL();b.Arg(0).Field(0x0400001A).Virtual(N117_DeviceEditorEnum).Store(0);
        b.Label("tryDevices").Branch(0x38,"moveDevices");b.Label("device").Address(0).Call(N117_DeviceEditorCurrent).Virtual(0x0A000009).Call(0x0A000025);
        b.Label("moveDevices").Address(0).Call(N117_DeviceEditorMove).Branch(0x3A,"device").Branch(0xDD,"afterDevices");
        b.Label("finallyDevices").Address(0).Call(N117_DeviceEditorDispose).Op(0xDC);
        b.Label("afterDevices").Arg(0).Field(0x0400001A).Virtual(N117_DeviceEditorClear);
        b.Arg(0).Field(0x0400001B).Virtual(N117_EnemyEditorEnum).Store(1);
        b.Label("tryEnemies").Branch(0x38,"moveEnemies");b.Label("enemy").Address(1).Call(N117_EnemyEditorCurrent).Virtual(0x0A000009).Call(0x0A000025);
        b.Label("moveEnemies").Address(1).Call(N117_EnemyEditorMove).Branch(0x3A,"enemy").Branch(0xDD,"afterEnemies");
        b.Label("finallyEnemies").Address(1).Call(N117_EnemyEditorDispose).Op(0xDC);
        b.Label("afterEnemies").Arg(0).Field(0x0400001B).Virtual(N117_EnemyEditorClear);
        b.Arg(0).Arg(1).Set(0x0400000D);b.Arg(1).Virtual(0x06000158).Store(2);
        b.Call(0x0A000004).Load(2).Field(0x0400037F).Virtual(0x0A00000A);
        b.Load(2).Field(0x0400037E).Call(0x06000213).Store(3);b.Arg(0).Field(0x0400000E).Load(3).Call(0x2B000005).Virtual(0x0A000029);
        b.Arg(0).Call(0x06000017);b.Arg(0).Call(0x06000018);b.Arg(0).Integer(0).Call(0x06000013).Op(0x2A);
        N117_Finally(b,0x06000012,("tryDevices","finallyDevices","afterDevices"),("tryEnemies","finallyEnemies","afterEnemies"));return b.Finish();
    }
    private static byte[] N117_CreateGridConfig()
    {
        var b=new IL();b.Arg(0).Field(0x040000A6).Virtual(0x06000158).Store(0);b.Load(0).Virtual(0x06000289).Store(1);
        b.Load(0).Field(0x0400037D).Virtual(N117_RowCount).Load(1).Op(0x5A).Store(2);
        b.Arg(0).Field(0x040000AC).Virtual(N117_GridConfigCount).Load(2).Branch(0x3E,"fill");
        b.Arg(0).Field(0x040000AC).Virtual(N117_GridConfigCount).Integer(1).Op(0x59).Store(3).Branch(0x38,"trimCheck");
        b.Label("trim").Arg(0).Field(0x040000AC).Load(3).Virtual(0x0A000077).Virtual(0x0A000009).Call(0x0A000025);
        b.Arg(0).Field(0x040000AC).Op(0x25).Load(3).Virtual(0x0A000077).Virtual(N117_GridConfigRemove).Op(0x26);
        b.Load(3).Integer(1).Op(0x59).Store(3);b.Label("trimCheck").Load(3).Load(2).Branch(0x3C,"trim");
        b.Label("fill").Arg(0).Field(0x040000AC).Virtual(N117_GridConfigCount).Load(2).Branch(0x3C,"initialize");
        b.Static(0x0400030A).Call(0x2B00000B).Store(4);b.Load(4).Arg(0).Set(0x0400081D);
        b.Arg(0).Field(0x040000AC).Load(4).Virtual(N117_GridConfigAdd);
        b.Load(4).Virtual(0x0A000001).Arg(0).Field(0x040000AB).Virtual(0x0A00002E).Integer(0).Virtual(0x0A00002F).Branch(0x38,"fill");
        b.Label("initialize").Integer(0).Store(3).Branch(0x38,"check");
        b.Label("body").Arg(0).Field(0x040000AC).Load(3).Virtual(0x0A000077).Store(4);
        b.Load(3).Load(1).Op(0x5D).Store(5);b.Load(3).Load(1).Op(0x5B).Store(6);
        b.Load(4).Load(5).Set(0x04000818);b.Load(4).Load(6).Set(0x04000819);
        b.Load(0).Field(0x0400037D).Load(6).Virtual(0x0A000065).Field(0x04000381).Load(4).Field(0x04000818).Virtual(0x0A000078).Store(7);
        b.Load(4).Load(7).Field(0x04000383).Set(0x0400081A);b.Load(4).Load(7).Field(0x04000385).Set(0x0400081B);b.Load(4).Load(7).Field(0x04000384).Set(0x0400081C);
        b.Load(4).Virtual(0x0600061C);b.Load(3).Integer(1).Op(0x58).Store(3);
        b.Label("check").Load(3).Arg(0).Field(0x040000AC).Virtual(N117_GridConfigCount).Branch(0x3F,"body");
        b.Arg(0).Field(0x040000AB).Virtual(0x2B00000C).Load(1).Virtual(0x0A00007A).Op(0x2A);return b.Finish();
    }
    private static byte[] N117_Benchmark4()
    {
        var b=new IL();b.Arg(0).Arg(0).Virtual(0x0A000001).Set(0x04000964);
        b.Call(0x0A000004).Call(0x0A00030C).Integer(2).Op(0x5B,0x6B).Store(0);b.Load(0).Virtual(0x0A00000A);
        b.Call(0x0A00030D).Op(0x6B).Call(0x0A00030C).Op(0x6B,0x5B).Store(1);b.Float(0).Store(2);
        b.Arg(0).Field(0x04000961).Store(3).Branch(0x38,"check");
        b.Label("loop").Arg(0).Field(0x04000960).Branch(0x3A,"next");
        b.Ref(0x72,0x7001FAE7).Address(3).Call(0x0A00002A).Ref(0x72,0x7001FAF7).Call(0x0A00010B).New(0x0A0000F3).Store(4);
        b.Load(2).Load(0).Load(0).Op(0x58).Branch(0x3D,"done");
        b.Load(4).Virtual(0x0A00002E).Store(10);
        b.Load(0).Op(0x65).Load(1).Op(0x5A).Float(.975f).Op(0x5A).Store(7);b.Load(0).Float(.975f).Op(0x5A).Load(2).Op(0x59).Store(8);
        b.Arg(0).Field(0x04000964).Virtual(0x0A000037).Store(6);
        b.Load(10).Address(6).Field(0x0A00001D).Load(7).Op(0x58).Address(6).Field(0x0A00008B).Load(8).Op(0x58).Address(6).Field(0x0A00009D).Float(0).Op(0x58).New(0x0A00009C).Virtual(0x0A000038);
        b.Load(4).Virtual(0x2B00009A).Store(5);b.Address(9).Float(0).Set(0x0A0001D4);b.Address(9).Float(.5f).Set(0x0A0001D5);
        b.Load(5).Virtual(0x0A0002E3).Load(9).Virtual(0x0A000305);
        b.Load(5).Integer(0).Virtual(0x0A0002FC);b.Load(5).Integer(1).Virtual(0x0A0002FB);b.Load(5).Integer(1).Virtual(0x0A00030E);b.Load(5).Load(3).Op(0x6B).Virtual(0x0A0002A3);
        b.Load(5).Address(3).Call(0x0A00002A).Ref(0x72,0x7001FB01).Call(0x0A000051).Virtual(0x0A000062);
        b.Load(5).Float(1).Float(1).Float(1).Float(1).New(0x0A0000BE).Virtual(0x0A0001DC);
        b.Load(2).Load(3).Op(0x6B,0x58).Store(2);
        b.Label("next").Load(3).Arg(0).Field(0x04000963).Op(0x58).Store(3);
        b.Label("check").Load(3).Arg(0).Field(0x04000962).Branch(0x3E,"loop");b.Label("done").Op(0x2A);return b.Finish();
    }
    private static void N117_Distance2(IL b,int targetLocal,int xField,int yField)
    {
        // Capture both target components before both source components, matching native loads.
        b.Load(targetLocal).Field(xField).Store(4);b.Load(targetLocal).Field(yField).Store(5);
        b.Arg(0).Field(0x04000466).Store(6);b.Arg(0).Field(0x04000467).Store(7);
        b.Load(4).Load(6).Op(0x59).Store(4);b.Load(5).Load(7).Op(0x59).Store(5);
        b.Load(4).Load(4).Op(0x5A).Load(5).Load(5).Op(0x5A,0x58,0x6C).Call(0x0A00009F).Op(0x6B);
    }
    private static byte[] N117_Warm(bool stove)
    {
        var b=new IL();float radius=stove?325:205;
        b.Arg(0).Field(0x04000477).Float(-1).Op(0x5A).Store(0);
        b.Arg(0).Field(0x04000463).Field(0x040003D4).Field(0x040002B1).Virtual(N117_PlantEnum).Store(1);
        b.Label("tryPlant").Branch(0x38,"movePlant");b.Label("plant").Address(1).Call(N117_PlantCurrent).Store(2);
        N117_Distance2(b,2,0x040004A4,0x040004A5);b.Float(radius).Branch(0x3D,"movePlant");
        b.Load(2).Field(0x040004D6).Integer(0).Load(0).Virtual(0x06000122);
        b.Label("movePlant").Address(1).Call(N117_PlantMove).Branch(0x3A,"plant").Branch(0xDD,"afterPlant");
        b.Label("finallyPlant").Address(1).Call(N117_PlantDispose).Op(0xDC);
        b.Label("afterPlant").Arg(0).Field(0x04000477).Float(-.2f).Op(0x5A).Store(0);
        b.New(0x0A0000CB).Store(3);b.Arg(0).Field(0x04000463).Field(0x040003D8).Field(N117_DeviceList).Virtual(N116_DeviceEnum).Store(8);
        b.Label("tryChoose").Branch(0x38,"moveChoose");b.Label("choose").Address(8).Call(N116_DeviceCurrent).Store(9);
        N117_Distance2(b,9,0x04000466,0x04000467);b.Float(radius).Branch(0x3D,"moveChoose");
        b.Float(0).Load(9).Field(0x04000479).Branch(0x3C,"moveChoose");b.Load(3).Load(9).Virtual(0x0A0000CE);
        b.Label("moveChoose").Address(8).Call(N116_DeviceMove).Branch(0x3A,"choose").Branch(0xDD,"afterChoose");
        b.Label("finallyChoose").Address(8).Call(N116_DeviceDispose).Op(0xDC);
        b.Label("afterChoose").Load(3).Virtual(N116_DeviceEnum).Store(8);
        b.Label("tryApply").Branch(0x38,"moveApply");b.Label("apply").Address(8).Call(N116_DeviceCurrent).Store(9);
        b.Load(9).Field(0x04000479).Store(4);b.Call(0x0A00001A).Load(0).Op(0x5A).Load(4).Op(0x58).Store(4);b.Load(9).Load(4).Set(0x04000479);
        b.Label("moveApply").Address(8).Call(N116_DeviceMove).Branch(0x3A,"apply").Branch(0xDD,"done");
        b.Label("finallyApply").Address(8).Call(N116_DeviceDispose).Op(0xDC);b.Label("done").Op(0x2A);
        N117_Finally(b,stove?0x06000321:0x0600031F,("tryPlant","finallyPlant","afterPlant"),("tryChoose","finallyChoose","afterChoose"),("tryApply","finallyApply","done"));return b.Finish();
    }
    private static byte[] N117_Potatomortar()
    {
        var b=new IL();b.Arg(0).Field(0x040004CE).Float(0).Branch(0x43,"checkState");b.Arg(0).Field(0x040004B6).Integer(1).Branch(0x40,"checkState");
        b.Arg(0).Field(0x040004CD).Store(0);b.Call(0x0A00001A).Load(0).Op(0x58).Store(0);b.Arg(0).Load(0).Set(0x040004CD);
        b.Load(0).Arg(0).Field(0x040004CE).Branch(0x44,"checkState");b.Arg(0).Field(0x040004E3).Ref(0x72,0x7000CCDA).Integer(1).Virtual(0x0A0001F3);
        b.Arg(0).Float(0).Set(0x040004CD);b.Arg(0).Integer(2).Set(0x040004B6);
        b.Label("checkState").Arg(0).Field(0x040004B6).Branch(0x3A,"done");b.Op(0x14).Store(1);
        b.Arg(0).Field(0x040004F3).Field(0x040003D5).Field(0x04000362).Virtual(N116_ZombieEnum).Store(2);
        b.Label("tryAll").Branch(0x38,"moveAll");b.Label("zombie").Address(2).Call(N116_ZombieCurrent).Store(3);
        b.Load(3).Op(0x14).Call(0x0A00000C).Branch(0x39,"moveAll");b.Load(3).Virtual(0x06000464).Branch(0x3A,"moveAll");
        b.Load(3).Field(N117_ZombieId).Integer(13).Branch(0x3B,"bounds");b.Load(3).Virtual(0x06000431).Branch(0x39,"moveAll");
        b.Label("bounds").Arg(0).Field(0x040004A4).Arg(0).Field(0x040004A8).Float(.5f).Op(0x5A,0x59).Store(4);
        b.Arg(0).Field(0x040004A9).Float(.5f).Op(0x5A).Arg(0).Field(0x040004A5).Op(0x58).Store(5);
        b.Load(3).Field(0x040005AD).Load(3).Field(N117_ZombieWidth).Float(.5f).Op(0x5A,0x59).Store(6);
        b.Load(3).Field(N117_ZombieDepth).Float(.5f).Op(0x5A).Load(3).Field(0x040005AE).Op(0x58).Store(7);
        b.Load(6).Load(3).Field(N117_ZombieWidth).Op(0x58).Load(4).Branch(0x43,"moveAll");
        b.Load(4).Arg(0).Field(0x040004A8).Op(0x58).Load(6).Branch(0x43,"moveAll");
        b.Load(7).Load(3).Field(N117_ZombieDepth).Op(0x58).Load(5).Branch(0x43,"moveAll");
        b.Load(5).Arg(0).Field(0x040004A9).Op(0x58).Load(7).Branch(0x43,"moveAll");
        b.Load(3).Store(1).Branch(0xDD,"afterAll");
        b.Label("moveAll").Address(2).Call(N116_ZombieMove).Branch(0x3A,"zombie").Branch(0xDD,"afterAll");
        b.Label("finallyAll").Address(2).Call(N116_ZombieDispose).Op(0xDC);
        b.Label("afterAll").Arg(0).Field(0x040004DD).Virtual(N116_ZombieEnum).Store(2);
        b.Label("tryBlocked").Branch(0x38,"moveBlocked");b.Label("blocked").Address(2).Call(N116_ZombieCurrent).Store(3);
        b.Load(3).Op(0x14).Call(0x0A00000C).Branch(0x39,"moveBlocked");b.Load(3).Virtual(0x06000464).Branch(0x3A,"moveBlocked");b.Load(3).Store(1).Branch(0xDD,"afterBlocked");
        b.Label("moveBlocked").Address(2).Call(N116_ZombieMove).Branch(0x3A,"blocked").Branch(0xDD,"afterBlocked");
        b.Label("finallyBlocked").Address(2).Call(N116_ZombieDispose).Op(0xDC);
        b.Label("afterBlocked").Load(1).Call(0x0A00000B).Branch(0x39,"done");b.Arg(0).Call(0x06000386);
        b.Label("done").Op(0x2A);N117_Finally(b,0x06000387,("tryAll","finallyAll","afterAll"),("tryBlocked","finallyBlocked","afterBlocked"));return b.Finish();
    }
    private static byte[] N117_ZombiePortraits(bool card)
    {
        var b=new IL();int cache=card?0x040002E7:0x040002E6;
        b.Static(cache).Virtual(N117_SpriteCount).Integer(0).Branch(0x3D,"return");
        b.Ref(0x72,0x700072D4).Ref(0x72,0x70008852).Ref(0x72,0x70007F78).Ref(0x72,card?0x70007B1E:0x70008CA1).Call(0x0A000173).Store(0);
        b.Ref(0xD0,0x0200005D).Call(0x0A00004C).Call(0x0A00007D).Virtual(0x0A0000EB).Store(1);
        b.Label("try").Branch(0x38,"move");b.Label("body").Load(1).Virtual(0x0A0000ED).Ref(0xA5,0x0200005D).Store(3);
        b.Ref(0xD0,0x0200005D).Call(0x0A00004C).Load(3).Ref(0x8C,0x0200005D).Call(0x0A00015F).Store(4);
        b.Load(0).Load(4).Call(0x0A000174).Call(0x2B000005).Store(5);b.Load(5).Call(0x0A00000B).Branch(0x39,"move");
        b.Label("pad").Static(cache).Virtual(N117_SpriteCount).Load(3).Branch(0x3D,"set");
        b.Static(cache).Op(0x14).Virtual(0x0A00017E).Branch(0x38,"pad");
        b.Label("set").Static(cache).Load(3).Load(5).Virtual(N117_SpriteSetItem);
        b.Label("move").Load(1).Virtual(0x0A0000EC).Branch(0x3A,"body").Branch(0xDD,"return");
        b.Label("finally");N112_DisposeEnumerator(b,1,2);b.Op(0xDC);b.Label("return").Static(cache).Op(0x2A);
        N117_Finally(b,card?0x06000233:0x06000232,("try","finally","return"));return b.Finish();
    }
    private static byte[] N117_LoadPlantPreviews()
    {
        var b=new IL();b.Ref(0x72,0x70006E20).Ref(0x72,0x700091B1).Call(0x0A000174).Store(0);
        b.Ref(0xD0,0x0200004E).Call(0x0A00004C).Call(0x0A00007D).Virtual(0x0A0000EB).Store(1);
        b.Label("try").Branch(0x38,"move");b.Label("body").Load(1).Virtual(0x0A0000ED).Ref(0xA5,0x0200004E).Store(3);
        b.Ref(0xD0,0x0200004E).Call(0x0A00004C).Load(3).Ref(0x8C,0x0200004E).Call(0x0A00015F).Store(4);
        b.Load(0).Load(4).Call(0x0A000174).Call(0x2B000049).Store(5);
        // Native appends null/missing assets too; no cache-count early return or Clear exists here.
        b.Static(0x040002F6).Load(5).Virtual(N117_PlantPreviewAdd);
        b.Load(5).Call(0x0A00000B).Branch(0x3A,"loaded");
        b.Ref(0x72,0x70004A44).Load(0).Load(4).Call(0x0A000174).Ref(0x72,0x70004A52).Call(0x0A00010B).Branch(0x38,"log");
        b.Label("loaded").Ref(0x72,0x700093FF).Load(4).Ref(0x72,0x70009407).Call(0x0A00010B);
        b.Label("log").Call(0x0A000010);
        b.Label("move").Load(1).Virtual(0x0A0000EC).Branch(0x3A,"body").Branch(0xDD,"return");
        b.Label("finally");N112_DisposeEnumerator(b,1,2);b.Op(0xDC);b.Label("return").Static(0x040002F6).Op(0x2A);
        N117_Finally(b,0x0600022A,("try","finally","return"));return b.Finish();
    }
    private static byte[] N117_LoadDisplayPlant()
    {
        var b=new IL();b.Arg(0).Field(0x040006EE).Call(0x0A00000B).Branch(0x39,"load");
        b.Arg(0).Field(0x040006EE).Virtual(0x0A000009).Call(0x0A000025);
        b.Label("load").Arg(1).Call(0x0600020D).Call(0x2B000048).Store(0);b.Load(0).Call(0x0A00000B).Branch(0x3A,"instantiate");
        b.Ref(0x72,0x70018655).Call(0x0A000010).Op(0x2A);
        b.Label("instantiate").Arg(0).Load(0).Call(0x2B000029).Set(0x040006EE);
        b.Arg(0).Field(0x040006EE).Virtual(0x0A000001).Arg(0).Field(0x040006EF).Virtual(0x0A00002E).Virtual(N110_SetParent);
        b.Arg(0).Field(0x040006EE).Virtual(0x0A000001).Float(0).Float(0).Float(0).New(0x0A00009C).Virtual(0x0A000042);
        b.Arg(0).Field(0x040006EE).Virtual(0x2B00002A).Store(1);b.Load(1).Ref(0x72,0x700068A4).Virtual(0x0A00015A);b.Load(1).Integer(100).Virtual(0x0A00015B);
        b.Arg(0).Field(0x040006EE).Field(0x040004AF).Store(2);b.Load(2).Branch(0x39,"hide");b.Load(2).Virtual(0x060005BE);
        b.Label("hide").Arg(0).Field(0x040006EE).Field(0x040004EA).Integer(0).Virtual(0x0A000007);
        b.Arg(0).Field(0x040006EE).Field(0x040004EC).Integer(0).Virtual(0x0A000007);
        b.Arg(0).Field(0x040006EE).Field(0x040004EF).Virtual(N117_GameEnum).Store(3);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(3).Call(N117_GameCurrent).Store(4);b.Load(4).Branch(0x39,"move");b.Load(4).Integer(0).Virtual(0x0A000007);
        b.Label("move").Address(3).Call(N117_GameMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(3).Call(N117_GameDispose).Op(0xDC);
        b.Label("after").Arg(0).Field(0x040006EE).Virtual(0x0A000001).Float(2.5f).Float(2.5f).Float(2.5f).New(0x0A00009C).Virtual(0x0A0000E5).Op(0x2A);
        N117_Finally(b,0x0600057E,("try","finally","after"));return b.Finish();
    }
    private static void N117_LevelPrices(IL b)=>b.Static(0x0400015E).Field(0x04000166).Field(0x04000312).Field(0x04000316);
    private static byte[] N117_LevelUpSupplies()
    {
        var b=new IL();b.Arg(0).Field(0x040006F5).Store(11);b.Load(11).Field(0x04000662).Store(0);b.Load(11).Field(0x04000661).Store(1);b.Load(11).Field(0x0400065D).Store(2);
        b.Integer(0).Store(3);b.Integer(0).Store(4).Branch(0x38,"costCheck");
        b.Label("cost").Integer(-1).Store(5);
        foreach(var p in new[]{(Order:1,Offset:18),(Order:2,Offset:47),(Order:3,Offset:86),(Order:4,Offset:135)})
        {b.Load(1).Integer(p.Order).Branch(0x40,"order"+p.Order).Integer(p.Offset).Store(5).Branch(0x38,"priced").Label("order"+p.Order);}
        b.Label("priced").Load(0).Load(5).Op(0x58).Store(5);
        b.Load(3);N117_LevelPrices(b);b.Load(5).Virtual(0x0A000263).Field(0x04000329).Load(2).Op(0x94,0x58).Store(3);
        b.Load(4);N117_LevelPrices(b);b.Load(5).Virtual(0x0A000263).Field(0x0400032A).Load(2).Op(0x94,0x58).Store(4);
        b.Load(0).Integer(1).Op(0x58).Store(0);b.Label("costCheck").Load(0).Arg(0).Field(0x040006FE).Branch(0x3F,"cost");
        b.Integer(0).Store(6).Branch(0x38,"destroyCheck");
        b.Label("destroy").Arg(0).Field(0x04000717).Load(6).Virtual(0x0A000260).Virtual(0x0A000009).Call(0x0A000025);
        b.Load(6).Integer(1).Op(0x58).Store(6);b.Label("destroyCheck").Load(6).Arg(0).Field(0x04000717).Virtual(N117_SuppliesItemCount).Branch(0x3F,"destroy");
        b.Arg(0).Field(0x04000717).Virtual(N117_SuppliesItemClear);
        b.Static(0x0400015E).Field(0x04000166).Field(0x04000310).Load(1).Integer(2).Op(0x5B).Virtual(0x060004C4).Store(7);
        b.Arg(0).Field(0x04000717).Load(7).Integer(1).Virtual(0x060004D9).Virtual(N116_SuppliesItemAdd);
        b.Arg(0).Field(0x04000717).Virtual(N117_SuppliesItemCount).Integer(1).Op(0x59).Store(9);
        b.Arg(0).Field(0x04000717).Load(9).Virtual(0x0A000260).Virtual(0x0A000009).Virtual(0x0A00002E).Arg(0).Field(0x04000716).Virtual(0x0A00002E).Integer(0).Virtual(0x0A00002F);
        b.Float(320).Load(9).Op(0x6B).Float(200).Op(0x5A,0x59).Store(10);
        b.Arg(0).Field(0x04000717).Load(9).Virtual(0x0A000260).Virtual(0x0A000009).Virtual(0x0A00002E).Load(10).Float(35).Float(0).New(0x0A00009C).Virtual(0x0A000042);
        b.Arg(0).Field(0x04000717).Load(9).Virtual(0x0A000260).Load(4).Set(0x04000591);b.Arg(0).Load(3).Set(0x0400071B).Op(0x2A);return b.Finish();
    }
    private static byte[] N117_FindNeighbors()
    {
        var b=new IL();b.New(0x0A0001B2).Store(0);
        b.Arg(0).Virtual(N117_NodeCount).Arg(1).Arg(2).Op(0x5A).Branch(0x3B,"neighbors");b.Ref(0x72,0x7000B008).Call(0x0A000010);
        b.Label("neighbors");foreach(var p in new[]{(Tag:"up",Dx:0,Dy:-1),(Tag:"right",Dx:1,Dy:0),(Tag:"down",Dx:0,Dy:1),(Tag:"left",Dx:-1,Dy:0)})
        {
            if(p.Tag=="up")b.Arg(3).Field(0x0400039E).Integer(0).Branch(0x3E,"null"+p.Tag);
            if(p.Tag=="right")b.Arg(3).Field(0x0400039D).Arg(1).Integer(1).Op(0x59).Branch(0x3C,"null"+p.Tag);
            if(p.Tag=="down")b.Arg(3).Field(0x0400039E).Arg(2).Integer(1).Op(0x59).Branch(0x3C,"null"+p.Tag);
            if(p.Tag=="left")b.Arg(3).Field(0x0400039D).Integer(0).Branch(0x3E,"null"+p.Tag);
            // Native checks only x < width and flat index < Count, retaining negative-index exceptions.
            b.Arg(3).Field(0x0400039D).Integer(p.Dx).Op(0x58).Store(1);b.Arg(3).Field(0x0400039E).Integer(p.Dy).Op(0x58).Store(2);
            b.Load(1).Arg(1).Branch(0x3C,"null"+p.Tag);b.Load(2).Arg(1).Op(0x5A).Load(1).Op(0x58).Store(3);
            b.Load(3).Arg(0).Virtual(N117_NodeCount).Branch(0x3C,"null"+p.Tag);
            b.Arg(0).Load(3).Virtual(0x0A0001B0).Store(4).Branch(0x38,"add"+p.Tag);
            b.Label("null"+p.Tag).Op(0x14).Store(4);b.Label("add"+p.Tag).Load(0).Load(4).Virtual(0x0A0001B3);
        }
        b.Load(0).Op(0x2A);return b.Finish();
    }
    private static void N117_SeekingRange(IL b)
    {
        b.Arg(0).Field(0x040004AF).Store(0);b.Integer(0).Store(1);b.Arg(0).Field(0x040004D2).Branch(0x39,"range");
        b.Arg(0).Field(0x040004D1).Field(0x0400068E).Store(1);
        b.Label("range").Load(0).Load(1).Virtual(0x060005BD).Store(2);
    }
    private static void N117_SeekZombieLoop(IL b,string tag)
    {
        b.Op(0x14).Store(3);b.Float(9999).Store(4);b.Arg(0).Field(0x040004F3).Field(0x040003D5).Field(0x04000362).Virtual(N116_ZombieEnum).Store(5);
        b.Label("try"+tag).Branch(0x38,"move"+tag);b.Label("body"+tag).Address(5).Call(N116_ZombieCurrent).Store(6);
        b.Load(6).Virtual(0x06000431).Branch(0x39,"move"+tag);b.Load(6).Virtual(0x06000464).Branch(0x3A,"move"+tag);
        b.Load(6).Field(0x040005F1).Arg(0).Field(0x040004C8).Branch(0x3B,"move"+tag);b.Load(2).Load(6).Virtual(0x2B000055).Branch(0x39,"move"+tag);
        b.Load(4).Load(6).Field(0x040005AD).Branch(0x43,"move"+tag);b.Load(6).Store(3);b.Load(6).Field(0x040005AD).Store(4);
        b.Label("move"+tag).Address(5).Call(N116_ZombieMove).Branch(0x3A,"body"+tag).Branch(0xDD,"after"+tag);
        b.Label("finally"+tag).Address(5).Call(N116_ZombieDispose).Op(0xDC);
        b.Label("after"+tag).Arg(0).Load(3).Set(0x040004DB);b.Arg(0).Field(0x040004DB).Op(0x2A);
    }
    private static byte[] N117_SeekZombie()
    {
        var b=new IL();N117_SeekingRange(b);b.Arg(0).Field(0x040004B1).Branch(0x3A,"other");
        b.Arg(0).Field(0x040004D5).Branch(0x39,"generalPea");b.Arg(0).Field(0x040004D1).Field(0x0400068E).Integer(1).Branch(0x40,"generalPea");
        b.Arg(0).Field(0x040004DC).Op(0x14).Call(0x0A00000C).Branch(0x3A,"null");
        b.Arg(0).Field(0x040004DB).Call(0x0A00000B).Branch(0x39,"generalPea");b.Arg(0).Field(0x040004DB).Virtual(0x06000431).Branch(0x39,"generalPea");
        b.Arg(0).Field(0x040004DB).Op(0x2A);
        b.Label("generalPea").Arg(0).Field(0x040004AF).Integer(0).Virtual(0x060005BD).Store(2);N117_SeekZombieLoop(b,"Pea");
        b.Label("other").Arg(0).Field(0x040004B1).Integer(1).Branch(0x3B,"null");N117_SeekZombieLoop(b,"Other");
        b.Label("null").Op(0x14,0x2A);N117_Finally(b,0x06000360,("tryPea","finallyPea","afterPea"),("tryOther","finallyOther","afterOther"));return b.Finish();
    }
    private static void N117_SeekDeviceLoop(IL b,string tag,bool other)
    {
        b.Op(0x14).Store(3);b.Float(9999).Store(4);b.Arg(0).Field(0x040004F3).Field(0x040003D8).Field(N117_DeviceList).Virtual(N116_DeviceEnum).Store(5);
        b.Label("try"+tag).Branch(0x38,"move"+tag);b.Label("body"+tag).Address(5).Call(N116_DeviceCurrent).Store(6);
        b.Integer(-1).Store(7);
        if(other)foreach(var p in new[]{(Plant:4,Projectile:15),(Plant:5,Projectile:1),(Plant:7,Projectile:24)})
        {b.Arg(0).Field(0x040004B1).Integer(p.Plant).Branch(0x40,"id"+tag+p.Plant).Integer(p.Projectile).Store(7).Branch(0x38,"damage"+tag).Label("id"+tag+p.Plant);}
        b.Label("damage"+tag).Arg(0).Op(0x14).Load(7).Integer(0).Call(0x0600036D).Store(8);
        // GetDamage happens for every current element before any device exclusion or null dereference.
        foreach(int excluded in new[]{2,7,8,12,14})b.Load(6).Field(0x0400046F).Integer(excluded).Branch(0x3B,"move"+tag);
        b.Load(6).Field(0x0400046F).Integer(11).Branch(0x3B,"attribute"+tag);
        b.Load(6).Field(0x0400047D).Branch(0x3A,"move"+tag).Branch(0x38,"camp"+tag);
        b.Label("attribute"+tag).Load(8).Field(0x04000125).Integer(3).Branch(0x40,"move"+tag);
        b.Label("camp"+tag).Load(6).Field(0x0400046E).Arg(0).Field(0x040004C8).Branch(0x3B,"move"+tag);
        b.Load(2).Load(6).Virtual(0x2B00005C).Branch(0x39,"move"+tag);b.Load(4).Load(6).Field(0x04000466).Branch(0x43,"move"+tag);
        b.Load(6).Store(3);b.Load(6).Field(0x04000466).Store(4);
        b.Label("move"+tag).Address(5).Call(N116_DeviceMove).Branch(0x3A,"body"+tag).Branch(0xDD,"after"+tag);
        b.Label("finally"+tag).Address(5).Call(N116_DeviceDispose).Op(0xDC);
        b.Label("after"+tag).Arg(0).Load(3).Set(0x040004DC);b.Arg(0).Field(0x040004DC).Op(0x2A);
    }
    private static byte[] N117_SeekDevice()
    {
        var b=new IL();N117_SeekingRange(b);b.Arg(0).Field(0x040004B1).Branch(0x3A,"other");
        b.Arg(0).Field(0x040004D5).Branch(0x39,"generalPea");b.Arg(0).Field(0x040004D1).Field(0x0400068E).Integer(1).Branch(0x40,"generalPea");
        b.Arg(0).Field(0x040004DB).Op(0x14).Call(0x0A00000C).Branch(0x3A,"null");
        b.Arg(0).Field(0x040004DC).Call(0x0A00000B).Branch(0x39,"null");b.Arg(0).Field(0x040004DC).Store(3);
        b.Load(3).Field(0x0400047D).Branch(0x3A,"null");b.Load(3).Op(0x2A);
        b.Label("generalPea");N117_SeekDeviceLoop(b,"Pea",false);
        b.Label("other").Arg(0).Field(0x040004B1).Integer(1).Branch(0x3B,"null");N117_SeekDeviceLoop(b,"Other",true);
        b.Label("null").Op(0x14,0x2A);N117_Finally(b,0x06000363,("tryPea","finallyPea","afterPea"),("tryOther","finallyOther","afterOther"));return b.Finish();
    }
}
