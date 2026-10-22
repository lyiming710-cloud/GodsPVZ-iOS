// Native119 source construction; not behavioral acceptance.
internal static partial class Program
{
    private static void N119_UncheckedAbs(IL b,string label)=>b.Op(0x25).Integer(0).Branch(0x3C,label).Op(0x65).Label(label);
    private static void N119_ProcessOpen(IL b,bool initial)
    {
        b.Arg(0).Address(2);if(initial)b.Arg(2);else b.Load(6);
        b.Arg(4).Arg(5).Arg(6).Arg(7).Virtual(0x06000283);
    }
    private static void N119_NodePath(IL b,int node,int path,int config)
    {
        b.Load(node).Field(0x0400039D).Load(node).Field(0x0400039E).Float(0).Arg(config).New(0x060002A4).Store(path);
    }
    private static byte[] N119_AstarFinding()
    {
        var b=new IL();b.Arg(2).Arg(3).Branch(0x3B,"same");b.Arg(2).Branch(0x39,"same");b.Arg(4).Virtual(N119_NodeEnum).Store(0);
        b.Label("tryReset").Branch(0x38,"resetMove");b.Label("reset").Address(0).Call(N119_NodeCurrent).Store(1);
        b.Load(1).Integer(int.MaxValue).Set(0x040003A0);
        b.Arg(3).Field(0x0400039D).Load(1).Field(0x0400039D).Op(0x59);N119_UncheckedAbs(b,"absX");b.Store(10);
        b.Arg(3).Field(0x0400039E).Load(1).Field(0x0400039E).Op(0x59);N119_UncheckedAbs(b,"absY");b.Load(10).Op(0x58).Store(10);
        b.Load(1).Load(10).Set(0x040003A1);b.Load(1).Integer(int.MaxValue).Set(0x040003A2);b.Load(1).Op(0x14).Set(0x040003A4);b.Load(1).Integer(0).Set(0x040003A3);
        b.Label("resetMove").Address(0).Call(N119_NodeMove).Branch(0x3A,"reset").Branch(0xDD,"resetDone");b.Label("finallyReset").Address(0).Call(N119_NodeDispose).Op(0xDC);
        b.Label("resetDone").Arg(2).Integer(0).Set(0x040003A0);b.New(N119_NodeCtor).Store(2);N119_ProcessOpen(b,true);b.Branch(0x38,"openTest");
        b.Label("open").Integer(int.MaxValue).Store(4);b.Integer(int.MaxValue).Store(5);b.Integer(0).Store(3).Branch(0x38,"minimumTest");
        b.Label("minimum").Load(2).Load(3).Virtual(N119_NodeItem).Field(0x040003A2).Load(4).Branch(0x3C,"minimumNext");
        b.Load(3).Store(5);b.Load(2).Load(3).Virtual(N119_NodeItem).Field(0x040003A2).Store(4);
        b.Label("minimumNext").Load(3).Integer(1).Op(0x58).Store(3);b.Label("minimumTest").Load(3).Load(2).Virtual(N119_NodeCount).Branch(0x3F,"minimum");
        b.Load(5).Load(2).Virtual(N119_NodeCount).Branch(0x3C,"noMinimum");b.Load(2).Load(5).Virtual(N119_NodeItem).Store(6).Branch(0x38,"process");
        b.Label("noMinimum").Op(0x14).Store(6);b.Label("process");N119_ProcessOpen(b,false);
        b.Label("openTest").Load(2).Virtual(N119_NodeCount).Branch(0x3A,"open");
        b.Arg(3).Field(0x040003A4).Branch(0x39,"clear");b.Arg(1).Virtual(N119_PathCount).Store(7);b.Arg(3).Store(8);
        b.Label("chain");N119_NodePath(b,8,9,8);b.Arg(1).Load(7).Load(9).Virtual(N119_PathInsert);b.Load(8).Field(0x040003A4).Store(8);b.Load(8).Arg(2).Branch(0x40,"chain");
        b.Arg(2).Store(8);N119_NodePath(b,8,9,8);b.Arg(1).Virtual(N119_PathCount).Integer(2).Op(0x59).Store(3).Branch(0x38,"smoothTest");
        b.Label("smooth").Arg(0).Arg(1).Load(3).Load(9).Arg(4).Arg(5).Arg(7).Virtual(0x06000284).Branch(0x39,"smoothNext");b.Arg(1).Load(3).Virtual(N119_PathRemoveAt);
        b.Label("smoothNext").Load(3).Integer(1).Op(0x59).Store(3);b.Label("smoothTest").Load(3).Load(7).Branch(0x3C,"smooth").Branch(0x38,"return");
        b.Label("clear").Arg(1).Virtual(N119_PathClear).Branch(0x38,"return");
        b.Label("same").Arg(1).Virtual(N119_PathCount).Store(7);b.Arg(3).Arg(8).Virtual(0x0600029F).Store(9);b.Arg(1).Load(7).Load(9).Virtual(N119_PathInsert);
        b.Label("return").Op(0x2A);N119_Finally(b,0x06000282,("tryReset","finallyReset","resetDone"));return b.Finish();
    }
    private static void N119_Finally(IL b,int token,params (string Try,string Handler,string After)[] regions)
    {
        Require(N119_EhTargets.Contains(token),"Native119 explicit EH scope");
        FinallyBodies.Add(token,regions.Select(x=>new FinallyClause(b.OffsetOf(x.Try),b.OffsetOf(x.Handler),b.OffsetOf(x.Handler),b.OffsetOf(x.After))).ToArray());
    }
    private static byte[] N119_UpdatePlantInitial()
    {
        var b=new IL();
        var groups=new[]{(Field:0x04000314,Ctor:0x060004D5,Limit:128,Count:N119_InitialCount,Add:N119_InitialAdd,NewList:0),(Field:0x04000315,Ctor:0x06000255,Limit:128,Count:N119_DataCount,Add:N119_DataAdd,NewList:0),(Field:0x04000316,Ctor:0x06000257,Limit:195,Count:N119_LevelCount,Add:N119_LevelAdd,NewList:N119_LevelCtor),(Field:0x04000317,Ctor:0x06000258,Limit:128,Count:N119_OrderCount,Add:N119_OrderAdd,NewList:N119_OrderCtor),(Field:0x04000318,Ctor:0x06000259,Limit:128,Count:N119_SkillCount,Add:N119_SkillAdd,NewList:0)};
        for(int i=0;i<groups.Length;i++)
        {
            var g=groups[i];string next="next"+i,test="test"+i;
            if(g.NewList!=0){b.Arg(0).Field(g.Field).Branch(0x3A,test);b.Arg(0).New(g.NewList).Set(g.Field);}
            b.Label(test).Arg(0).Field(g.Field).Virtual(g.Count).Integer(g.Limit).Branch(0x3C,next);
            b.Arg(0).Field(g.Field).Store(i);b.Load(i).Virtual(g.Count).Store(5);
            if(i==2){b.Load(5).Integer(1).Op(0x58).New(g.Ctor).Store(6);b.Arg(0).Field(g.Field).Load(6).Virtual(g.Add);}
            else b.Load(i).Load(5).New(g.Ctor).Virtual(g.Add);
            b.Branch(0x38,test).Label(next);
        }
        b.Op(0x2A);return b.Finish();
    }
    private static void N119_PathNodeFind(IL b,int x,int y,int target)=>b.Load(5).Load(1).Load(x).Load(y).Call(0x060002A0).Store(target);
    private static void N119_CheckLinePoint(IL b,int x,int y)=>b.Arg(4).Arg(5).Load(x).Load(y).Call(0x060002A0).Field(0x0400039F).Arg(6).Branch(0x3D,"false");
    private static void N119_CheckLineOffset(IL b,int nx,int ny,int extraX,int extraY)
    {
        b.Load(nx);if(extraX!=0)b.Integer(extraX).Op(0x58);b.Load(7).Op(0x5A).Load(3).Op(0x58).Store(20);
        b.Load(ny);if(extraY!=0)b.Integer(extraY).Op(0x58);b.Load(8).Op(0x5A).Load(4).Op(0x58).Store(21);N119_CheckLinePoint(b,20,21);
    }
    private static byte[] N119_AstarText()
    {
        var b=new IL();b.Arg(3).Store(0);b.Arg(2).Integer(0).Branch(0x3E,"previousDone");b.Arg(1).Arg(2).Integer(1).Op(0x59).Virtual(N119_PathItem).Store(0);
        b.Label("previousDone").Arg(1).Arg(2).Virtual(N119_PathItem).Store(1);b.Arg(1).Arg(2).Integer(1).Op(0x58).Virtual(N119_PathItem).Store(2);
        b.Load(1).Field(0x040003A8).Float(0).Branch(0x3D,"false");b.Load(1).Field(0x040003AA).Branch(0x3A,"false");
        b.Load(0).Field(0x040003A6).Store(4);b.Load(0).Field(0x040003A5).Store(3);
        b.Load(2).Field(0x040003A5).Load(1).Field(0x040003A5).Op(0x59).Store(22);b.Load(1).Field(0x040003A5).Load(3).Op(0x59).Store(23);
        b.Load(2).Field(0x040003A6).Load(1).Field(0x040003A6).Op(0x59).Store(25);b.Load(1).Field(0x040003A6).Load(4).Op(0x59).Store(24);
        b.Load(22).Load(23).Op(0x5A).Branch(0x3A,"verticalTest");b.Load(24).Branch(0x3A,"verticalTest");b.Load(25).Branch(0x39,"true");
        b.Label("verticalTest").Load(25).Load(24).Op(0x5A).Branch(0x3A,"directions");b.Load(23).Branch(0x3A,"directions");b.Load(22).Branch(0x39,"true");
        b.Label("directions").Load(2).Field(0x040003A5).Load(3).Op(0x59).Store(5);b.Load(5).Call(0x0A0001B6).Store(7);
        b.Load(2).Field(0x040003A6).Load(0).Field(0x040003A6).Op(0x59).Store(6);b.Load(6).Call(0x0A0001B6).Store(8);
        b.Load(6);N119_UncheckedAbs(b,"absLineY");b.Store(10);b.Load(5);N119_UncheckedAbs(b,"absLineX");b.Store(9);
        b.Load(6).Op(0x6B).Load(5).Op(0x6B,0x5B,0x6B).Store(11);b.Load(9).Load(10).Branch(0x3B,"diagonal");
        b.Integer(0).Store(13);b.Integer(0).Store(14);b.Integer(1).Store(12);
        b.Load(11).Call(0x0A0000E9).Float(1).Branch(0x3F,"xMajor");b.Float(1).Load(11).Op(0x5B,0x6B).Store(26);b.Load(10).Integer(2).Op(0x5A).Store(27).Branch(0x38,"yTest");
        b.Label("yStep").Load(12).Op(0x6B).Load(8).Op(0x6B).Float(.6f).Op(0x5A,0x6B,0x59,0x6B).Load(26).Op(0x5A,0x6B).Call(0x0A0000E9).Store(19);
        b.Load(13).Integer(2).Op(0x5A).Integer(1).Op(0x58,0x6B).Store(18);b.Load(13).Store(15);b.Load(19).Load(18).Branch(0x44,"yNewXDone");b.Load(13).Integer(1).Op(0x58).Store(15);
        b.Label("yNewXDone").Load(12).Integer(2).Op(0x5B).Store(17);b.Load(17).Load(14).Branch(0x3B,"ySame");
        b.Load(17).Store(14);b.Load(15).Store(13);N119_CheckLineOffset(b,13,14,0,0);b.Branch(0x38,"yNext");
        b.Label("ySame").Load(18).Load(19).Branch(0x44,"ySecond");N119_CheckLineOffset(b,13,14,0,1);
        b.Label("ySecond").Load(15).Load(13).Branch(0x3B,"yNext");N119_CheckLineOffset(b,15,14,0,0);
        b.Label("yNext").Load(12).Integer(1).Op(0x58).Store(12);b.Label("yTest").Load(12).Load(27).Branch(0x3F,"yStep").Branch(0x38,"true");
        b.Label("xMajor").Load(9).Integer(2).Op(0x5A).Store(27).Branch(0x38,"xTest");
        b.Label("xStep").Load(12).Op(0x6B).Load(11).Op(0x5A,0x6B).Call(0x0A0000E9).Load(8).Op(0x6B).Float(.6f).Op(0x5A,0x6B,0x58,0x6B).Store(19);
        b.Load(14).Integer(2).Op(0x5A).Integer(1).Op(0x58,0x6B).Store(18);b.Load(14).Store(16);b.Load(19).Load(18).Branch(0x44,"xNewYDone");b.Load(14).Integer(1).Op(0x58).Store(16);
        b.Label("xNewYDone").Load(12).Integer(2).Op(0x5B).Store(17);b.Load(17).Load(13).Branch(0x3B,"xSame");
        b.Load(17).Store(13);b.Load(16).Store(14);N119_CheckLineOffset(b,13,14,0,0);b.Branch(0x38,"xNext");
        b.Label("xSame").Load(18).Load(19).Branch(0x44,"xSecond");N119_CheckLineOffset(b,13,14,1,0);
        b.Label("xSecond").Load(16).Load(14).Branch(0x3B,"xNext");N119_CheckLineOffset(b,13,16,0,0);
        b.Label("xNext").Load(12).Integer(1).Op(0x58).Store(12);b.Label("xTest").Load(12).Load(27).Branch(0x3F,"xStep").Branch(0x38,"true");
        b.Label("diagonal").Integer(0).Store(12).Branch(0x38,"diagonalTest");b.Label("diagonalStep");N119_CheckLinePoint(b,3,4);
        b.Load(3).Load(7).Op(0x58).Store(20);N119_CheckLinePoint(b,20,4);b.Load(4).Load(8).Op(0x58).Store(21);N119_CheckLinePoint(b,3,21);
        b.Load(20).Store(3);b.Load(21).Store(4);b.Load(12).Integer(1).Op(0x58).Store(12);b.Label("diagonalTest").Load(12).Load(9).Branch(0x3F,"diagonalStep");
        b.Label("true").Integer(1).Op(0x2A);b.Label("false").Integer(0).Op(0x2A);return b.Finish();
    }
    private static byte[] N119_PathFinding()
    {
        var b=new IL();b.Arg(1).Field(0x040005AA).Store(0);b.Arg(1).Field(0x040005F9).Branch(0x39,"null");b.Arg(1).Field(0x040005F9).Virtual(N119_PathCount).Branch(0x39,"null");
        b.Arg(0).Virtual(0x06000289).Store(1);b.Arg(0).Field(0x0400037D).Branch(0x39,"zeroRows");b.Arg(0).Field(0x0400037D).Virtual(N119_RowCount).Store(2).Branch(0x38,"dimensions");
        b.Label("zeroRows").Integer(0).Store(2);b.Label("dimensions").Load(1).Branch(0x39,"null");b.Load(2).Branch(0x39,"null");
        b.Arg(1).Field(0x040005AB).Store(3);b.Arg(1).Field(0x040005AC).Store(4);b.Load(3).Integer(0).Branch(0x3F,"null");
        b.New(N119_NodeCtor).Store(5);b.Integer(0).Store(8).Branch(0x38,"rowTest");
        b.Label("row").Arg(0).Field(0x0400037D).Load(8).Virtual(N119_RowItem).Store(6);b.Integer(0).Store(7).Branch(0x38,"columnTest");
        b.Label("column").Load(6).Field(0x04000381).Load(7).Virtual(N119_GridItem).Store(9);b.New(0x0600029E).Store(10);
        b.Load(10).Integer(int.MaxValue).Set(0x040003A0);b.Load(10).Load(7).Set(0x0400039D);b.Load(10).Load(8).Set(0x0400039E);b.Load(10).Load(9).Virtual(0x06000295).Set(0x0400039F);b.Load(5).Load(10).Virtual(N119_NodeAdd);
        b.Load(7).Integer(1).Op(0x58).Store(7);b.Label("columnTest").Load(7).Load(1).Branch(0x3F,"column");b.Load(8).Integer(1).Op(0x58).Store(8);b.Label("rowTest").Load(8).Load(2).Branch(0x3F,"row");
        b.New(N119_PathCtor).Store(11);b.Load(3).Load(1).Branch(0x3C,"outside");N119_PathNodeFind(b,3,4,12);b.Branch(0x38,"target");
        b.Label("outside").Integer(-1).Store(15);b.Integer(int.MaxValue).Store(16);b.Integer(0).Store(8).Branch(0x38,"edgeTest");
        b.Label("edge").Arg(0).Field(0x0400037D).Load(8).Virtual(N119_RowItem).Field(0x04000381).Load(1).Integer(1).Op(0x59).Virtual(N119_GridItem).Virtual(0x06000295).Arg(1).Field(0x040005F8).Branch(0x3D,"edgeNext");
        b.Load(8).Load(4).Op(0x59);N119_UncheckedAbs(b,"edgeAbs");b.Store(17);b.Load(17).Load(16).Branch(0x3C,"edgeNext");b.Load(8).Store(15);b.Load(17).Store(16);
        b.Label("edgeNext").Load(8).Integer(1).Op(0x58).Store(8);b.Label("edgeTest").Load(8).Load(2).Branch(0x3F,"edge");b.Load(15).Integer(0).Branch(0x3F,"null");
        b.Load(1).Integer(1).Op(0x59).Store(7);N119_PathNodeFind(b,7,15,12);b.Load(12).Branch(0x39,"missing");b.Load(12).Load(0).Field(0x040003B4).Virtual(0x0600029F).Store(13);b.Load(11).Load(13).Virtual(N119_PathAdd);
        b.Label("target").Arg(1).Field(0x040005F9).Integer(0).Virtual(N119_PathItem).Store(13);b.Load(13).Field(0x040003A5).Store(7);b.Load(13).Field(0x040003A6).Store(8);
        N119_PathNodeFind(b,7,8,14);b.Load(14).Branch(0x39,"outsideTarget");
        b.Arg(0).Load(11).Load(12).Load(14).Load(5).Load(1).Load(2).Arg(1).Field(0x040005F8).Load(0).Field(0x040003B4).Virtual(0x06000282);
        b.Load(5).Load(1).Load(13).Field(0x040003A5).Load(13).Field(0x040003A6).Call(0x060002A0).Op(0x26).Branch(0x38,"returnPath");
        b.Label("outsideTarget").Load(13).Field(0x040003A5).Load(13).Field(0x040003A6).Float(0).Load(0).Field(0x040003B4).New(0x060002A4).Store(13);b.Load(11).Load(13).Virtual(N119_PathAdd);
        b.Label("returnPath").Load(11).Op(0x2A);b.Label("missing").Ref(0x72,N119_MissingMapEntry).Call(0x0A000010);b.Label("null").Op(0x14,0x2A);return b.Finish();
    }
    private static byte[] N119_SphSeek()
    {
        var b=new IL();b.Arg(0).Field(0x040005E1).Branch(0x39,"clearAll");b.Arg(0).Field(0x040005E2).Branch(0x3A,"return");
        for(int i=0;i<3;i++)b.Op(0x14).Store(i);
        b.Arg(0).Field(0x040005AA).Field(0x040003D4).Field(0x040002B1).Virtual(N119_PlantEnum).Store(3);
        b.Label("tryPlants").Branch(0x38,"plantMove");b.Label("plant").Address(3).Call(N119_PlantCurrent).Store(4);
        b.Load(4).Virtual(0x06000359).Branch(0x39,"plantMove");b.Load(4).Field(0x040004C8).Arg(0).Field(0x040005F1).Branch(0x3B,"plantMove");b.Load(4).Store(0);
        b.Label("plantMove").Address(3).Call(N119_PlantMove).Branch(0x3A,"plant").Branch(0xDD,"plantsDone");b.Label("finallyPlants").Address(3).Call(N119_PlantDispose).Op(0xDC);
        b.Label("plantsDone").Arg(0).Load(0).Set(0x04000601);b.Load(0).Op(0x14).Call(0x0A000012).Branch(0x39,"aimPlant");
        b.Arg(0).Field(0x040005AA).Field(0x040003D8).Field(0x040001D6).Virtual(N119_DeviceEnum).Store(5);
        b.Label("tryDevices").Branch(0x38,"deviceMove");b.Label("device").Address(5).Call(N119_DeviceCurrent).Store(6);
        b.Arg(0).Integer(26).Op(0x14).Integer(0).Virtual(0x06000443).Store(9);b.Load(6).Load(9).Virtual(0x06000310).Branch(0x39,"deviceMove");
        b.Load(6).Field(0x0400046E).Arg(0).Field(0x040005F1).Branch(0x3B,"deviceMove");b.Load(6).Store(1);
        b.Label("deviceMove").Address(5).Call(N119_DeviceMove).Branch(0x3A,"device").Branch(0xDD,"devicesDone");b.Label("finallyDevices").Address(5).Call(N119_DeviceDispose).Op(0xDC);
        b.Label("devicesDone").Arg(0).Load(1).Set(0x04000603);b.Load(0).Op(0x14).Call(0x0A000012).Branch(0x39,"aimDevice");b.Load(1).Op(0x14).Call(0x0A000012).Branch(0x39,"aimDevice");
        b.Arg(0).Field(0x040005AA).Field(0x040003D5).Field(0x04000362).Virtual(N119_ZombieEnum).Store(7);
        b.Label("tryZombies").Branch(0x38,"zombieMove");b.Label("zombie").Address(7).Call(N119_ZombieCurrent).Store(8);
        b.Load(8).Virtual(0x0A000009).Virtual(0x0A000154).Branch(0x39,"zombieMove");
        foreach(int f in new[]{0x040005D8,0x040005D2,0x040005D3})b.Load(8).Field(f).Branch(0x3A,"zombieMove");
        b.Load(8).Field(0x040005B9).Integer(3).Branch(0x3B,"jumper");b.Load(8).Field(0x040005B9).Integer(23).Branch(0x3B,"jumper");b.Load(8).Field(0x040005B9).Integer(13).Branch(0x3B,"zombieMove").Branch(0x38,"zombieCamp");
        b.Label("jumper").Load(8).Field(0x040005DF).Branch(0x3A,"zombieMove");b.Label("zombieCamp").Load(8).Field(0x040005F1).Arg(0).Field(0x040005F1).Branch(0x3B,"zombieMove");b.Load(8).Store(2);
        b.Label("zombieMove").Address(7).Call(N119_ZombieMove).Branch(0x3A,"zombie").Branch(0xDD,"zombiesDone");b.Label("finallyZombies").Address(7).Call(N119_ZombieDispose).Op(0xDC);
        b.Label("zombiesDone").Arg(0).Load(2).Set(0x04000602);b.Load(0).Op(0x14).Call(0x0A000012).Branch(0x39,"aimZombie");b.Load(1).Op(0x14).Call(0x0A000012).Branch(0x39,"aimZombie");b.Load(2).Op(0x14).Call(0x0A000012).Branch(0x3A,"hide");
        b.Label("aimZombie").Load(2).Field(0x040005AD).Store(10);b.Load(2).Field(0x040005AE).Store(11).Branch(0x38,"shoot");
        b.Label("aimDevice").Arg(0).Op(0x14).Set(0x04000602);b.Load(1).Field(0x04000466).Store(10);b.Load(1).Field(0x04000467).Store(11).Branch(0x38,"shoot");
        b.Label("aimPlant").Arg(0).Op(0x14).Set(0x04000603);b.Arg(0).Op(0x14).Set(0x04000602);b.Load(0).Field(0x040004A4).Store(10);b.Load(0).Field(0x040004A5).Store(11);
        b.Label("shoot").Arg(0).Load(10).Load(11).Float(0).New(0x0A00009C).Virtual(0x060004B1).Branch(0x38,"return");
        b.Label("clearAll");foreach(int f in new[]{0x04000601,0x04000603,0x04000602})b.Arg(0).Op(0x14).Set(f);
        b.Label("hide").Arg(0).Field(0x04000614).Integer(0).Op(0x9A).Integer(0).Virtual(0x0A000007);b.Label("return").Op(0x2A);
        N119_Finally(b,0x060004B2,("tryPlants","finallyPlants","plantsDone"),("tryDevices","finallyDevices","devicesDone"),("tryZombies","finallyZombies","zombiesDone"));return b.Finish();
    }
    private static void N119_SnowAudioRest(IL b,string label)
    {
        b.Static(0x040002F2).Integer(12).Virtual(0x0A000084).Store(4);b.Call(0x0A000004).Virtual(0x0A000001).Virtual(0x0A000037).Store(5);b.Call(0x06000138).Store(6);
        b.Load(4).Load(5).Load(6).Call(0x0600013D).Op(0x26);b.Arg(0).Arg(0).Field(0x040005DA).Float(.75f).Op(0x59,0x6B).Set(0x040005DA);
        b.Arg(0).Field(0x040005DA).Float(0).Branch(0x42,label+"Hp");b.Arg(0).Float(0).Set(0x040005DA);b.Arg(0).Integer(1).Set(0x040005D9);
        b.Arg(0).Field(0x040005FE).Ref(0x72,0x7000FD73).Integer(1).Virtual(0x0A0001F3);b.Arg(0).Float(.02f).Virtual(0x06000484);
        b.Label(label+"Hp").Arg(0).Field(0x04000616).Call(0x0A00000B).Branch(0x39,label+"End");b.Arg(0).Field(0x04000616).Virtual(0x06000627);b.Label(label+"End");
    }
    private static void N119_SnowTarget(IL b,bool plant)
    {
        string p=plant?"plantHit":"deviceHit";int first=plant?0x0400038A:0x0400038F,second=plant?0x04000389:0x0400038D,local=plant?8:7,target=plant?0x04000601:0x04000603;
        b.Load(0).Field(first).Call(0x0A00000B).Branch(0x3A,p+"First");b.Load(0).Field(second).Call(0x0A00000B).Branch(0x39,p+"Done");
        b.Load(0).Field(second).Store(local);b.Integer(second).Store(9).Branch(0x38,p+"Compare");b.Label(p+"First").Load(0).Field(first).Store(local);b.Integer(first).Store(9);
        b.Label(p+"Compare").Load(local).Arg(0).Field(target).Call(0x0A000012).Branch(0x39,p+"Damage");
        // Original plant branch also clears targetDevice, not targetPlant.
        b.Arg(0).Op(0x14).Set(0x04000603);b.Arg(0).Op(0x14).Set(0x040005FA);
        b.Label(p+"Damage").Load(9).Integer(first).Branch(0x3B,p+"DamageFirst");b.Load(0).Field(second).Branch(0x38,p+"Take");b.Label(p+"DamageFirst").Load(0).Field(first);
        b.Label(p+"Take").Load(1).Op(0x14).Virtual(plant?0x060003B3:0x06000336);N119_SnowAudioRest(b,p);b.Label(p+"Done");
    }
    private static byte[] N119_SnowImpact()
    {
        var b=new IL();b.Arg(0).Field(0x040005FA).Branch(0x39,"grid");b.Arg(0).Field(0x040005FA).Virtual(N119_EnemyPathEnum).Store(2);
        b.Label("tryPath").Branch(0x38,"pathMove");b.Label("path").Address(2).Call(N119_EnemyPathCurrent).Store(3);b.Load(3).Branch(0x39,"pathMove");b.Load(3).Field(0x040003A9).Branch(0x3A,"pathMove");
        b.Arg(1).Load(3).Field(0x040003A5).Branch(0x40,"pathMove");b.Arg(2).Load(3).Field(0x040003A6).Branch(0x40,"pathMove");b.Load(3).Integer(1).Set(0x040003A9);
        b.Label("pathMove").Address(2).Call(N119_EnemyPathMove).Branch(0x3A,"path").Branch(0xDD,"pathDone");b.Label("finallyPath").Address(2).Call(N119_EnemyPathDispose).Op(0xDC);
        b.Label("pathDone");b.Label("grid").Arg(0).Field(0x040005AA).Arg(1).Arg(2).Virtual(0x060002C4).Store(0);b.Load(0).Branch(0x39,"return");
        b.Load(0).Field(0x04000384).Branch(0x39,"damage");b.Arg(0).Field(0x040005AA).Virtual(0x060002BC);
        b.Label("damage").Arg(0).Integer(-1).Op(0x14).Integer(0).Virtual(0x06000443).Store(1);N119_SnowTarget(b,false);N119_SnowTarget(b,true);
        b.Label("return").Op(0x2A);N119_Finally(b,0x060004AA,("tryPath","finallyPath","pathDone"));return b.Finish();
    }
    private static byte[] N119_CopyBoardConfig()
    {
        var b=new IL();b.Arg(0).Field(0x04000198).Virtual(0x06000289).Store(1);b.Arg(0).Field(0x04000198).Virtual(0x0600028A).Store(2);b.Load(1).Load(2).New(0x06000155).Store(0);
        foreach(int f in new[]{0x04000194,0x04000195,0x04000197,0x04000199,0x0400019A,0x0400019B})b.Load(0).Arg(0).Field(f).Set(f);
        var groups=new[]{(Field:0x04000196,Enum:N119_CopyTypesEnum,Current:N119_CopyTypesCurrent,Move:N119_CopyTypesMove,Dispose:N119_CopyTypesDispose,Add:N119_CopyTypesAdd),(Field:0x0400019C,Enum:N119_CopyLootEnum,Current:N119_CopyLootCurrent,Move:N119_CopyLootMove,Dispose:N119_CopyLootDispose,Add:N119_CopyLootAdd),(Field:0x0400019D,Enum:N119_CopyEnemiesEnum,Current:N119_CopyEnemiesCurrent,Move:N119_CopyEnemiesMove,Dispose:N119_CopyEnemiesDispose,Add:N119_CopyEnemiesAdd),(Field:0x0400019E,Enum:N119_CopyDevicesEnum,Current:N119_CopyDevicesCurrent,Move:N119_CopyDevicesMove,Dispose:N119_CopyDevicesDispose,Add:N119_CopyDevicesAdd),(Field:0x0400019F,Enum:N119_CopyCardsEnum,Current:N119_CopyCardsCurrent,Move:N119_CopyCardsMove,Dispose:N119_CopyCardsDispose,Add:N119_CopyCardsAdd),(Field:0x040001A0,Enum:N119_CopyEntriesEnum,Current:N119_CopyEntriesCurrent,Move:N119_CopyEntriesMove,Dispose:N119_CopyEntriesDispose,Add:N119_CopyEntriesAdd)};
        var eh=new List<(string,string,string)>();
        for(int i=0;i<groups.Length;i++)
        {
            var g=groups[i];int en=3+i*2,item=en+1;string t="try"+i,h="finally"+i,a="after"+i,move="move"+i,each="each"+i;
            if(i==1)b.Load(0).Arg(0).Field(0x04000198).Virtual(0x06000285).Set(0x04000198);
            b.Arg(0).Field(g.Field).Virtual(g.Enum).Store(en);b.Label(t).Branch(0x38,move);b.Label(each).Address(en).Call(g.Current).Store(item);b.Load(0).Field(g.Field).Load(item).Virtual(g.Add);
            b.Label(move).Address(en).Call(g.Move).Branch(0x3A,each).Branch(0xDD,a);b.Label(h).Address(en).Call(g.Dispose).Op(0xDC);b.Label(a);eh.Add((t,h,a));
        }
        b.Load(0).Op(0x2A);N119_Finally(b,0x06000156,eh.ToArray());return b.Finish();
    }
    private static void N119_CopySaveScalars(IL b,params int[] fields)
    {
        foreach(int f in fields)b.Arg(0).Arg(1).Field(f).Set(f);
    }
    private static void N119_CopySaveArray(IL b,int field,bool boolean)
    {
        string loop="array"+field,test="arrayTest"+field;int arr=boolean?15:14;
        b.Integer(0).Store(0).Branch(0x38,test);b.Label(loop).Arg(0).Field(field).Store(arr);b.Load(arr).Load(0).Arg(1).Field(field).Load(0).Op(boolean?(byte)0x91:(byte)0x94);
        if(boolean)b.Integer(0).Op(0xFE,0x01).Integer(0).Op(0xFE,0x01);
        b.Op(boolean?(byte)0x9C:(byte)0x9E);b.Load(0).Integer(1).Op(0x58).Store(0);b.Label(test).Load(0).Arg(0).Field(field).Op(0x8E,0x69).Branch(0x3F,loop);
    }
    private static byte[] N119_CopySave()
    {
        var b=new IL();N119_CopySaveScalars(b,Enumerable.Range(0x04000626,13).ToArray());
        foreach(int f in new[]{0x04000633,0x04000634,0x04000635})N119_CopySaveArray(b,f,false);
        N119_CopySaveScalars(b,0x04000637,0x04000638);b.Arg(0).Field(0x04000639).Virtual(N119_SavePlantClear);
        b.Arg(1).Field(0x04000639).Virtual(N119_SavePlantEnum).Store(1);b.Label("tryPlants").Branch(0x38,"plantMove");
        b.Label("plant").Arg(0).Field(0x04000639).Store(3);b.Address(1).Call(N119_SavePlantCurrent).Store(2);b.Load(3).Load(2).Virtual(0x060004CA).Virtual(N119_SavePlantAdd);
        b.Label("plantMove").Address(1).Call(N119_SavePlantMove).Branch(0x3A,"plant").Branch(0xDD,"plantsDone");b.Label("finallyPlants").Address(1).Call(N119_SavePlantDispose).Op(0xDC);
        b.Label("plantsDone").Arg(1).Field(0x0400063A).Virtual(N119_SavePropEnum).Store(4);b.Label("tryProps").Branch(0x38,"propMove");
        b.Label("prop").Arg(0).Field(0x0400063A).Store(7);b.Address(4).Call(N119_SavePropCurrent).Store(5);b.Load(5).Field(0x04000674).New(0x060004D6).Store(6);
        b.Load(6).Load(5).Field(0x04000675).Set(0x04000675);b.Load(7).Load(6).Virtual(N119_SavePropAdd);
        b.Label("propMove").Address(4).Call(N119_SavePropMove).Branch(0x3A,"prop").Branch(0xDD,"propsDone");b.Label("finallyProps").Address(4).Call(N119_SavePropDispose).Op(0xDC);
        b.Label("propsDone");N119_CopySaveScalars(b,0x0400063B,0x0400063C);b.Arg(1).Field(0x0400063D).Virtual(N119_SaveSupplyEnum).Store(8);b.Label("trySupplies").Branch(0x38,"supplyMove");
        b.Label("supply").Arg(0).Field(0x0400063D).Store(11);b.Address(8).Call(N119_SaveSupplyCurrent).Store(9);b.Op(0x14).New(0x060004DB).Store(10);
        foreach(int f in new[]{0x04000678,0x04000677,0x04000679,0x0400067A,0x0400067B})b.Load(10).Load(9).Field(f).Set(f);b.Load(11).Load(10).Virtual(N119_SaveSupplyAdd);
        b.Label("supplyMove").Address(8).Call(N119_SaveSupplyMove).Branch(0x3A,"supply").Branch(0xDD,"suppliesDone");b.Label("finallySupplies").Address(8).Call(N119_SaveSupplyDispose).Op(0xDC);
        b.Label("suppliesDone");foreach(int f in new[]{0x0400063E,0x0400063F,0x04000640})N119_CopySaveArray(b,f,true);
        N119_CopySaveScalars(b,0x04000641);b.Integer(0).Store(0).Branch(0x38,"teamTest");
        b.Label("team").Arg(0).Field(0x04000642).Load(0).Virtual(N119_SaveTeamItem).Store(12);b.Arg(1).Field(0x04000642).Load(0).Virtual(N119_SaveTeamItem).Store(13);
        // get_Item returns a Team value. Clone mutates this local copy, with no set_Item in the original.
        b.Address(12).Load(13).Call(0x060004C8);b.Load(0).Integer(1).Op(0x58).Store(0);b.Label("teamTest").Load(0).Arg(1).Field(0x04000642).Virtual(N119_SaveTeamCount).Branch(0x3F,"team");
        N119_CopySaveScalars(b,0x04000643,0x04000644,0x04000645,0x04000646,0x04000647);b.Op(0x2A);
        N119_Finally(b,0x060004C0,("tryPlants","finallyPlants","plantsDone"),("tryProps","finallyProps","propsDone"),("trySupplies","finallySupplies","suppliesDone"));return b.Finish();
    }
    private static void N119_CollectCreate(IL b,int kind)
    {
        b.Arg(0).Field(0x0400051F).Store(6);b.Arg(0).Virtual(0x0A000001).Virtual(0x0A000037).Store(2);b.Load(6).Integer(6).Integer(kind).Load(2).Virtual(0x06000201).Store(0);
    }
    private static void N119_CollectAdd(IL b)=>b.Arg(0).Field(0x0400051F).Field(0x040002C2).Load(0).Virtual(N119_CollectProjectAdd);
    private static void N119_CollectTrigVector(IL b,int field,float factor,bool negate)
    {
        b.Load(4).Call(0x0A000200).Op(0x6B);if(negate)b.Op(0x65);b.Float(factor).Op(0x5A,0x6B).Store(8);
        b.Load(4).Call(0x0A0001FF).Op(0x6B);if(negate)b.Op(0x65);b.Float(factor).Op(0x5A,0x6B).Store(9);
        b.Load(0).Load(8).Load(9).Float(0).New(0x0A00009C).Set(field);
    }
    private static void N119_CollectBurst(IL b,string label,int kind)
    {
        b.Arg(0).Field(0x04000517).Integer(1).Branch(0x3B,label+"Single");b.Integer(0).Store(3).Branch(0x38,label+"Test");
        b.Label(label+"Loop").Integer(120).Arg(0).Field(0x04000517).Integer(1).Op(0x59,0x5B).Load(3).Op(0x5A).Integer(30).Op(0x58,0x6C);
        N118_Double(b,Math.PI);b.Op(0x5A);N118_Double(b,180);b.Op(0x5B).Store(4);N119_CollectCreate(b,kind);b.Load(0).Call(0x0A00000B).Branch(0x39,label+"Next");
        N119_CollectTrigVector(b,0x04000522,400,false);N119_CollectTrigVector(b,0x04000524,500,true);N119_CollectTrigVector(b,0x04000523,400,false);N119_CollectAdd(b);
        b.Load(0).Virtual(0x0A000001).Arg(0).Field(0x0400051F).Virtual(0x0A000001).Integer(0).Virtual(0x0A00002F);
        b.Label(label+"Next").Load(3).Integer(1).Op(0x58).Store(3);b.Label(label+"Test").Load(3).Arg(0).Field(0x04000517).Branch(0x3F,label+"Loop").Branch(0x38,label+"Done");
        // Both original singleton branches request kind2, including the ID5 branch.
        b.Label(label+"Single");N119_CollectCreate(b,2);b.Load(0).Call(0x0A00000B).Branch(0x39,label+"Done");
        b.Load(0).Float(0).Float(1).Float(0).New(0x0A00009C).Set(0x04000522);b.Load(0).Arg(0).Field(0x04000522).Set(0x04000523);
        b.Load(0).Field(0x04000522).Store(7);b.Load(0);foreach(int f in new[]{0x0A00001D,0x0A00008B,0x0A00009D})b.Address(7).Field(f).Float(200).Op(0x5A,0x6B);b.New(0x0A00009C).Set(0x04000522);N119_CollectAdd(b);b.Label(label+"Done");
    }
    private static byte[] N119_CollectProject()
    {
        var b=new IL();b.Arg(0).Field(0x04000520).Integer(1).Branch(0x40,"track");b.Arg(0).Field(0x0400051A).Float(.5f).Branch(0x41,"track");
        b.Arg(0).Virtual(0x0A000001).Store(1);b.Arg(0).Field(0x04000518).Store(5);b.Load(1);for(int i=0;i<3;i++)b.Float(1).Load(5).Op(0x5A,0x6B);b.New(0x0A00009C).Virtual(0x0A0000E5);
        b.Label("track").Arg(0).Integer(4).Set(0x04000520);
        foreach(int id in new[]{1,2,8})b.Arg(0).Field(0x04000516).Integer(id).Branch(0x3B,"idle");b.Branch(0x38,"treasure");
        b.Label("idle").Arg(0).Field(0x04000525).Virtual(0x2B000061).Ref(0x72,0x7000EC98).Integer(0).Float(0).Virtual(0x0A00020B);b.Arg(0).Field(0x04000525).Virtual(0x2B000061).Float(0).Virtual(0x0A0001EA);
        b.Label("treasure").Arg(0).Field(0x04000516).Integer(4).Branch(0x3B,"four");b.Arg(0).Field(0x04000516).Integer(5).Branch(0x40,"finish").Branch(0x38,"five");
        b.Label("four").Arg(0).Field(0x04000527).Integer(0).Virtual(0x0A000007);N119_CollectBurst(b,"coin",2);
        b.Label("five").Arg(0).Field(0x04000516).Integer(5).Branch(0x40,"particle");N119_CollectBurst(b,"gem",3);
        b.Label("particle").Call(0x060001E5).Integer(25).Virtual(0x060001EA).Virtual(0x0A00002E).Store(1);b.Arg(0).Virtual(0x0A000001).Virtual(0x0A000037).Store(2);b.Load(1).Load(2).Virtual(0x0A000038);
        b.Arg(0).Field(0x04000526).Integer(0).Virtual(0x0A000006).Virtual(0x2B000011).Float(1).Float(1).Float(1).Float(1).New(0x0A0000BE).Virtual(0x0A000097);
        b.Arg(0).Field(0x0400051F).Virtual(0x060001FF);b.Arg(0).Field(0x04000525).Virtual(0x2B000061).Integer(1).Virtual(0x0A000208);b.Arg(0).Field(0x0400051E).Float(5.99f).Virtual(0x060002C3);
        b.Label("finish").Arg(0).Virtual(0x060003C5);b.Arg(0).Virtual(0x060003CC);b.Arg(0).Float(0).Set(0x0400051C).Op(0x2A);return b.Finish();
    }
    private static void N119_ArmorRenderer(IL b,int item,int renderer,bool optional,string label)
    {
        if(optional){b.Op(0x14).Store(renderer);b.Load(item).Call(0x0A00000B).Branch(0x39,label);}
        b.Load(item).Virtual(0x2B000011).Store(renderer);if(optional)b.Label(label);
    }
    private static void N119_ArmorSprite(IL b,int renderer,int index)=>b.Load(renderer).Static(0x040002CE).Integer(index).Call(0x0600014B);
    private static byte[] N119_InjuryArmor1()
    {
        var b=new IL();b.Arg(0).Field(0x040005C2).Branch(0x39,"dispatch");b.Arg(0).Field(0x040005C3).Float(0).Branch(0x42,"dispatch");b.Arg(0).Arg(1).Virtual(0x06000439).Op(0x2A);
        b.Label("dispatch");for(int kind=1;kind<=6;kind++)b.Arg(0).Field(0x040005C2).Integer(kind).Branch(0x3B,"kind"+kind);b.Branch(0x38,"return");
        int[] names=[0,N119_AnimCone,N119_AnimBucket,N119_AnimBrick,N119_IceCube1,N119_AnimBucket,N119_LadderHelmet];
        int[] low=[0,1,4,7,16,27,37],high=[0,2,5,8,18,28,38];
        for(int kind=1;kind<=6;kind++)
        {
            string p="kind"+kind;b.Label(p).Arg(0).Field(0x04000609).Ref(0x72,names[kind]).Call(0x06000142).Store(2);
            if(kind is 4 or 6)b.Arg(0).Field(0x04000609).Ref(0x72,kind==4?N119_IceCube2:N119_LadderMask).Call(0x06000142).Store(3);
            N119_ArmorRenderer(b,2,0,kind!=6,p+"Renderer0");if(kind is 4 or 6)N119_ArmorRenderer(b,3,1,kind!=6,p+"Renderer1");
            b.Arg(0).Field(0x040005C7).Integer(2).Branch(0x3C,p+"Low");b.Arg(0).Field(0x040005C4).Float(3).Op(0x5B,0x6B).Arg(0).Field(0x040005C3).Branch(0x43,p+"Low");
            N119_ArmorSprite(b,0,high[kind]);if(kind is 4 or 6)N119_ArmorSprite(b,1,kind==4?17:41);
            if(kind==6){b.Arg(0).Field(0x040005E5).Arg(0).Field(0x040005C7).Op(0x59).Integer(2).Op(0x58).Store(4);b.Arg(0).Integer(2).Set(0x040005C7);b.Arg(0).Load(4).Set(0x040005E5);}
            else b.Arg(0).Integer(2).Set(0x040005C7);b.Branch(0x38,"return");
            b.Label(p+"Low").Arg(0).Field(0x040005C7).Integer(1).Branch(0x3C,"return");b.Arg(0).Field(0x040005C4).Op(0x25,0x58,0x6B).Float(3).Op(0x5B,0x6B).Arg(0).Field(0x040005C3).Branch(0x43,"return");
            N119_ArmorSprite(b,0,low[kind]);if(kind is 4 or 6)N119_ArmorSprite(b,1,kind==4?15:40);
            if(kind==6)b.Arg(0).Arg(0).Field(0x040005E5).Arg(0).Field(0x040005C7).Op(0x59).Integer(1).Op(0x58).Set(0x040005E5);
            b.Arg(0).Integer(1).Set(0x040005C7).Branch(0x38,"return");
        }
        b.Label("return").Op(0x2A);return b.Finish();
    }
    private static void N119_BrokenPathUpdate(IL b,string label)
    {
        b.Arg(0).Integer(0).Set(0x0400047B);b.Load(3).Virtual(0x06000295).Load(4).Branch(0x3B,label);b.Arg(0).Field(0x04000463).Field(0x040003D5).Virtual(0x06000276);b.Label(label);
    }
    private static byte[] N119_BrokenDevice()
    {
        var b=new IL();b.Arg(0).Integer(1).Set(0x0400047D);b.Op(0x14).Store(0);b.Call(0x06000138).Store(1);b.Float(1).Store(2);
        b.Arg(0).Field(0x04000463).Arg(0).Field(0x04000464).Arg(0).Field(0x04000465).Virtual(0x060002C4).Store(3);b.Load(3).Virtual(0x06000295).Store(4);
        b.Arg(0).Field(0x0400046F).Store(5);foreach(int id in new[]{0,1,2,3,4,6,9,13})b.Load(5).Integer(id).Branch(0x3B,"id"+id);b.Branch(0x38,"destroy");
        foreach(int id in new[]{0,1,2})b.Label("id"+id).Branch(0x38,"audio");
        b.Label("id3").Arg(0).Field(0x04000486).Op(0x14).Call(0x0A00000C).Branch(0x39,"openDone");b.Arg(0).Field(0x04000486).Ref(0x72,N119_Open).Integer(1).Virtual(0x0A0001F3);b.Label("openDone");N119_BrokenPathUpdate(b,"vasePathDone");
        b.Arg(0).Field(0x04000470).Branch(0x39,"audio");b.Arg(0).Field(0x04000470).Field(0x0400045C).Store(12);
        foreach(int t in new[]{1,2})b.Load(12).Integer(t).Branch(0x3B,"vaseParticle6");foreach(int t in new[]{3,4,5,8})b.Load(12).Integer(t).Branch(0x3B,"vaseParticle25");b.Branch(0x38,"audio");
        b.Label("vaseParticle6").Call(0x060001E5).Integer(6).Virtual(0x060001EA).Store(6);b.Arg(0).Virtual(0x0A000001).Virtual(0x0A000037).Store(7);b.Address(7).Address(7).Field(0x0A00008B).Float(30).Op(0x58,0x6B).Set(0x0A00008B);
        b.Load(6).Virtual(0x0A00002E).Load(7).Virtual(0x0A000038);b.Static(0x040002F1).Integer(14).Virtual(0x0A000084).Store(0).Branch(0x38,"vaseVolume");
        b.Label("vaseParticle25").Call(0x060001E5).Integer(25).Virtual(0x060001EA).Store(6);b.Arg(0).Virtual(0x0A000001).Virtual(0x0A000037).Store(7);b.Load(6).Virtual(0x0A00002E).Load(7).Virtual(0x0A000038);b.Static(0x040002F1).Integer(4).Virtual(0x0A000084).Store(0);
        b.Label("vaseVolume").Load(1).Float(1.2f).Op(0x5A,0x6B).Store(1).Branch(0x38,"audio");
        b.Label("id4").Call(0x060001E5).Integer(26).Virtual(0x060001EA).Store(6);b.Load(6).Call(0x0A00000B).Branch(0x39,"id4Clip");b.Load(6).Virtual(0x0A00002E).Store(8);b.Arg(0).Virtual(0x0A000001).Virtual(0x0A000037).Store(7);b.Load(8).Load(7).Virtual(0x0A000038);
        b.Label("id4Clip").Static(0x040002F1).Integer(12).Virtual(0x0A000084).Store(0);b.Load(1).Float(1.6f).Op(0x5A,0x6B).Store(1);b.Float(.7f).Store(2).Branch(0x38,"destroy");
        b.Label("id6").Call(0x060001E5).Integer(2).Virtual(0x060001EA).Store(6);b.Load(6).Call(0x0A00000B).Branch(0x39,"bombClip");
        b.Load(6).Virtual(0x0A00002E).Arg(0).Field(0x04000466).Arg(0).Field(0x04000468).Arg(0).Field(0x04000467).Op(0x58,0x6B).Float(0).New(0x0A00009C).Virtual(0x0A000038);
        b.Load(6).Virtual(0x0A00002E).Float(1.2f).Float(1.2f).Float(1.2f).New(0x0A00009C).Virtual(0x0A0000E5);
        b.Label("bombClip").Static(0x040002F1).Integer(1).Virtual(0x0A000084).Store(0);b.Float(.9f).Float(1.1f).Call(0x0A000083).Store(2);b.Arg(0).Field(0x04000463).Float(.15f).Float(8).Virtual(0x060002CD);
        b.Arg(0).Op(0x14).Virtual(0x06000326).Virtual(0x0600010C).Op(0x26).Branch(0x38,"destroy");
        b.Label("id9");N119_BrokenPathUpdate(b,"extinguishPathDone");b.Arg(0).Field(0x04000488).Virtual(N119_BrokenObjectEnum).Store(9);b.Label("trySprites").Branch(0x38,"spriteMove");
        b.Label("sprite").Address(9).Call(N119_BrokenObjectCurrent).Store(10);b.Load(10).Integer(0).Virtual(0x0A000007);b.Label("spriteMove").Address(9).Call(N119_BrokenObjectMove).Branch(0x3A,"sprite").Branch(0xDD,"spritesDone");
        b.Label("finallySprites").Address(9).Call(N119_BrokenObjectDispose).Op(0xDC);b.Label("spritesDone").Arg(0).Field(0x04000488).Ref(0x72,N119_Extinguish).Call(0x0600013F).Branch(0x38,"audio");
        b.Label("id13").Arg(0).Field(0x04000486).Op(0x14).Call(0x0A00000C).Branch(0x39,"audio");b.Arg(0).Field(0x04000486).Ref(0x72,N119_Broken).Integer(1).Virtual(0x0A0001F3).Branch(0x38,"audio");
        b.Label("destroy").Arg(0).Virtual(0x06000322);b.Label("audio").Load(0).Call(0x0A00000B).Branch(0x39,"return");b.Call(0x0A000004).Virtual(0x0A000001).Virtual(0x0A000037).Store(7);b.Load(0).Load(7).Load(1).Load(2).Call(0x0600013E).Op(0x26);
        b.Label("return").Op(0x2A);N119_Finally(b,0x0600030F,("trySprites","finallySprites","spritesDone"));return b.Finish();
    }
}
