internal static partial class Program
{
    private static byte[] N104_AllZombiePathsBody()
    {
        var b=new IL();b.Arg(0).Field(0x04000362).Virtual(0x0A0000D9).Store(0);
        b.Label("try").Branch(0x38,"move");
        b.Label("body").Address(0).Call(0x0A0000DA).Store(1);
        b.Load(1).Virtual(0x0A000009).Virtual(0x0A000154).Branch(0x39,"move");
        b.Load(1).Virtual(0x06000464).Branch(0x3A,"move");
        b.Load(1).Field(0x040005D4).Branch(0x3A,"move");
        b.Load(1).Field(0x040005B9).Integer(13).Branch(0x3B,"move");
        b.Load(1).Virtual(0x0600046A).Op(0x26);
        b.Label("move").Address(0).Call(0x0A0000DC).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(0).Call(0x0A00015E).Op(0xDC);b.Label("after").Op(0x2A);
        RegisterFinally(0x06000276,b,"try","finally","after");return b.Finish();
    }

    private static byte[] N104_CollectProjectBody()
    {
        var b=new IL();
        b.Arg(0).Field(0x040002C2).Arg(1).Virtual(0x0A000405).Op(0x26);
        b.Arg(1).Field(0x04000515).Store(0);
        foreach(int v in new[]{1,2,8})b.Load(0).Integer(v).Branch(0x3B,"coin");
        b.Load(0).Integer(3).Branch(0x3B,"diamond").Branch(0x38,"return");
        b.Label("coin").Arg(0).Arg(1).Field(0x04000517).Call(0x060001FD);
        b.Arg(0).Field(0x040002C3).Call(0x0A00000B).Branch(0x39,"coinTimer");
        b.Arg(0).Field(0x040002C3).Integer(1).Virtual(0x0A000007);
        b.Label("coinTimer").Arg(0).Float(7).Set(0x040002C9).Branch(0x38,"return");
        b.Label("diamond").Arg(0).Arg(0).Field(0x040002C7).Arg(1).Field(0x04000517).Op(0x58).Set(0x040002C7);
        b.Arg(0).Field(0x040002C6).Call(0x0A00000B).Branch(0x39,"diamondTimer");
        b.Arg(0).Field(0x040002C6).Integer(1).Virtual(0x0A000007);
        b.Label("diamondTimer").Arg(0).Float(7).Set(0x040002CA);
        b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N104_InitializePlantSaveBody()
    {
        var b=new IL();
        b.Arg(0).Field(0x04000312).Field(0x04000314).Arg(1).Virtual(0x0A000058).Virtual(0x060004CA).Store(0);
        b.Load(0).Integer(0).Set(0x04000671);b.Load(0).Arg(2).Set(0x04000661);b.Load(0).Arg(3).Set(0x04000662);
        b.Load(0).Virtual(0x060004CC).Store(1);
        for(int i=0;i<3;i++)
        {b.Load(1).Integer(i).Branch(0x3E,"skillsDone");b.Load(0).Field(0x0400066E+i).Integer(1).Set(0x04000693);}
        b.Label("skillsDone").Load(0).Arg(4).Set(0x04000663);
        b.Arg(0).Load(0).Arg(4).Call(0x0600024E);
        b.Arg(0).Field(0x04000312).Field(0x04000315).Arg(1).Virtual(0x0A000059).Load(0).Virtual(0x06000256);
        b.Load(0).Op(0x2A);return b.Finish();
    }

    private static byte[] N104_FrameCounterPositionBody()
    {
        var b=new IL();
        for(int a=0;a<4;a++)b.Arg(1).Integer(a).Branch(0x3B,"anchor"+a);
        b.Op(0x2A);
        foreach(var p in new[]{(Arg:0,Anchor:0,X:0f,Y:1f),(Arg:1,Anchor:6,X:0f,Y:0f),(Arg:2,Anchor:2,X:1f,Y:1f),(Arg:3,Anchor:8,X:1f,Y:0f)})
        {
            b.Label("anchor"+p.Arg).Arg(0).Field(0x040009E4).Integer(p.Anchor).Virtual(0x0A00035B);
            b.Float(p.X).Store(0);b.Float(p.Y).Store(1).Branch(0x38,"position");
        }
        b.Label("position").Arg(0).Field(0x040009E6).Store(4);
        b.Arg(0).Field(0x040009E5).Store(3);
        b.Load(3).Load(4).Load(0).Load(1).Float(100).New(0x0A00009C).Virtual(0x0A00035C).Virtual(0x0A000038).Op(0x2A);
        return b.Finish();
    }

    private static byte[] N104_CollectAllProjectsBody()
    {
        var b=new IL();b.Arg(0).Field(0x040002C2).Virtual(0x0A000422).Store(0);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(0).Call(0x0A000425).Store(1);
        b.Load(1).Field(0x04000520).Integer(4).Branch(0x3B,"move");
        b.Load(1).Field(0x04000520).Integer(6).Branch(0x3B,"move");b.Load(1).Virtual(0x060003C3);
        b.Label("move").Address(0).Call(0x0A000423).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(0).Call(0x0A000424).Op(0xDC);
        b.Label("after").Arg(0).Field(0x040002BF).Field(0x040003DB).Call(0x0A00000B).Branch(0x39,"return");
        b.Arg(0).Field(0x040002BF).Field(0x040003DB).Virtual(0x06000263);
        b.Label("return").Op(0x2A);RegisterFinally(0x060001FF,b,"try","finally","after");return b.Finish();
    }

    private static byte[] N104_FindZombiePathBody()
    {
        var b=new IL();
        b.Arg(0).Field(0x040005AA).Op(0x14).Call(0x0A000012).Branch(0x3A,"false");
        b.Arg(0).Field(0x040005AA).Field(0x040003B5).Branch(0x39,"false");
        b.Arg(0).Field(0x040005AA).Field(0x040003B5).Arg(0).Virtual(0x0600028C).Store(0);
        b.Load(0).Branch(0x39,"empty");b.Load(0).Virtual(0x0A000435).Branch(0x39,"empty");
        b.Arg(0).Load(0).Set(0x040005FA);b.Integer(1).Op(0x2A);
        b.Label("empty").Arg(0).Field(0x040005FA).Branch(0x3A,"clear");
        b.Arg(0).New(0x0A000130).Set(0x040005FA);
        b.Label("clear").Arg(0).Field(0x040005FA).Virtual(0x0A000224);
        b.Arg(0).Field(0x040005F9).Branch(0x39,"false");
        b.Arg(0).Field(0x040005F9).Virtual(0x0A000435).Integer(0).Branch(0x3E,"false");
        b.Arg(0).Field(0x040005AA).Field(0x040003D5).Virtual(0x0600027D);
        b.Label("false").Integer(0).Op(0x2A);return b.Finish();
    }

    private static byte[] N104_ClearPlantCardsBody() => N104_DestroyListBody(0x06000549,0x040006C8,0x0A000426,0x0A000429,0x0A000427,0x0A000428,0x0A0002AB);
    private static byte[] N104_ClearEntriesBody() => N104_DestroyListBody(0x0600060E,0x04000802,EntryEnumerator,EntryCurrent,EntryMove,EntryDispose,EntryClear);
    private static byte[] N104_ClearSuppliesBody() => N104_DestroyListBody(0x060006EA,0x040008EA,SuppliesEnumerator,SuppliesCurrent,SuppliesMove,0x0A00010C,SuppliesClear);
    private static byte[] N104_DestroyListBody(int token,int field,int get,int current,int move,int dispose,int clear)
    {
        var b=new IL();b.Arg(0).Field(field).Virtual(get).Store(0);
        b.Label("try").Branch(0x38,"move");
        b.Label("body").Address(0).Call(current).Store(1);b.Load(1).Virtual(0x0A000009).Call(0x0A000025);
        b.Label("move").Address(0).Call(move).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(0).Call(dispose).Op(0xDC);
        // Clear the post-callback field, not the original enumerated list.
        b.Label("after").Arg(0).Field(field).Virtual(clear).Op(0x2A);
        RegisterFinally(token,b,"try","finally","after");return b.Finish();
    }

    private static byte[] N104_CreateAstarNodesBody()
    {
        var b=new IL();b.New(0x0A0001B2).Store(0);b.Integer(0).Store(1);
        b.Label("rowLoop").Load(1).Arg(2).Branch(0x3C,"return");
        b.Arg(0).Field(0x0400037D).Load(1).Virtual(0x0A000065).Store(3);b.Integer(0).Store(2);
        b.Label("gridLoop").Load(2).Arg(1).Branch(0x3C,"nextRow");
        b.Load(3).Field(0x04000381).Load(2).Virtual(0x0A000078).Store(4);
        b.New(0x0600029E).Store(5);b.Load(5).Integer(int.MaxValue).Set(0x040003A0);
        b.Load(5).Load(2).Set(0x0400039D);b.Load(5).Load(1).Set(0x0400039E);
        b.Load(5).Load(4).Virtual(0x06000295).Set(0x0400039F);
        b.Load(0).Load(5).Virtual(0x0A0001B3);
        b.Load(2).Integer(1).Op(0x58).Store(2).Branch(0x38,"gridLoop");
        b.Label("nextRow").Load(1).Integer(1).Op(0x58).Store(1).Branch(0x38,"rowLoop");
        b.Label("return").Load(0).Op(0x2A);return b.Finish();
    }

    private static byte[] N104_SaveTeamCardsBody()
    {
        var b=new IL();
        b.Arg(0).Field(0x0400088E).Field(0x04000642).Arg(0).Field(0x0400088D).Integer(1).Op(0x59).Virtual(0x0A000235).Store(0);
        b.Address(0).Field(0x0400064A).Store(1);b.Load(1).Virtual(IntListClear);
        b.Address(0).Field(0x0400064B).Store(2);b.Load(2).Virtual(IntListClear);
        b.Integer(0).Store(3);
        b.Label("loop").Load(3).Arg(0).Field(0x04000887).Virtual(0x0A0002AA).Branch(0x3C,"return");
        b.Arg(0).Field(0x04000887).Load(3).Virtual(0x0A00029B).Field(0x04000429).Integer(-1).Branch(0x3B,"next");
        b.Load(1).Arg(0).Field(0x04000887).Load(3).Virtual(0x0A00029B).Field(0x04000429).Virtual(0x0A000413);
        b.Load(2).Arg(0).Field(0x04000887).Load(3).Virtual(0x0A00029B).Field(0x04000432).Field(0x0400068E).Virtual(0x0A000413);
        b.Label("next").Load(3).Integer(1).Op(0x58).Store(3).Branch(0x38,"loop");
        b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N104_CloseSkillPreviewBody()
    {
        var b=new IL();
        b.Arg(0).Arg(0).Field(0x04000725).Arg(0).Field(0x04000732).Virtual(0x060004CB);
        b.Float(1).Float(1).Float(.2f).Float(1).New(0x0A0000BE).Call(0x06000591);
        b.Arg(0).Field(0x04000724).Virtual(SkillButtonsEnumerator).Store(0);
        b.Label("try").Branch(0x38,"move");
        b.Label("body").Address(0).Call(SkillButtonsCurrent).Store(1);
        b.Load(1).Field(0x040008E0).Store(2);
        b.Load(2).Ref(0x72,0x70018DDD).Arg(0).Field(0x04000725).FieldAddress(0x04000663).Call(0x0A00002A).Call(0x0A000051).Integer(1).Virtual(0x0A00004B);
        b.Label("move").Address(0).Call(SkillButtonsMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(0).Call(0x0A00026B).Op(0xDC);b.Label("after").Op(0x2A);
        RegisterFinally(0x0600058B,b,"try","finally","after");return b.Finish();
    }

    private static byte[] N104_PathDistanceBody()
    {
        // V0 sum, V1 index, V2/3 initial XY, V4..7 adjacent coordinates, V8 abs temporary.
        var b=new IL();b.Integer(0).Store(0);
        b.Arg(0).Branch(0x39,"startLoop");b.Arg(0).Virtual(0x0A000435).Integer(0).Branch(0x3E,"startLoop");
        b.Arg(0).Integer(0).Virtual(0x0A0001B5).Field(0x040003A5).Store(2);
        b.Arg(0).Integer(0).Virtual(0x0A0001B5).Field(0x040003A6).Arg(2).Op(0x59).Store(8);N104_EmitWrappingAbs(b,"firstY");b.Store(0);
        b.Load(2).Arg(1).Op(0x59).Store(8);N104_EmitWrappingAbs(b,"firstX");b.Load(0).Op(0x58).Store(0);
        b.Label("startLoop").Integer(1).Store(1);
        b.Label("loop").Load(1).Arg(0).Virtual(0x0A000435).Branch(0x3C,"return");
        b.Arg(0).Load(1).Virtual(0x0A0001B5).Field(0x040003A5).Store(4);
        b.Arg(0).Load(1).Integer(1).Op(0x59).Virtual(0x0A0001B5).Field(0x040003A5).Store(5);
        b.Arg(0).Load(1).Virtual(0x0A0001B5).Field(0x040003A6).Store(6);
        b.Arg(0).Load(1).Integer(1).Op(0x59).Virtual(0x0A0001B5).Field(0x040003A6).Store(7);
        b.Load(4).Load(5).Op(0x59).Store(8);N104_EmitWrappingAbs(b,"deltaX");b.Load(0).Op(0x58).Store(0);
        b.Load(6).Load(7).Op(0x59).Store(8);N104_EmitWrappingAbs(b,"deltaY");b.Load(0).Op(0x58).Store(0);
        b.Load(1).Integer(1).Op(0x58).Store(1).Branch(0x38,"loop");
        b.Label("return").Load(0).Op(0x2A);return b.Finish();
    }
    private static void N104_EmitWrappingAbs(IL b,string label)
    {
        b.Load(8).Integer(0).Branch(0x3C,label);b.Load(8).Op(0x65).Store(8);
        b.Label(label).Load(8); // Int32.MinValue remains MinValue, as native NEG/CMOVS does.
    }

    private static byte[] N104_CloneEnemyPathBody()
    {
        var b=new IL();b.New(0x0A000130).Store(0);
        b.Arg(0).Field(0x040001FD).Virtual(0x0A00022A).Store(1);
        b.Label("try").Branch(0x38,"move");
        b.Label("body").Load(0).Address(1).Call(0x0A00022B).Virtual(0x0A000151);
        b.Label("move").Address(1).Call(0x0A00022C).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(1).Call(0x0A000045).Op(0xDC);
        b.Label("after").Load(0).Op(0x2A);RegisterFinally(0x060001A2,b,"try","finally","after");return b.Finish();
    }

    private static byte[] N104_OrderUpPlantBody()
    {
        var b=new IL();b.Arg(0).Field(0x04000717).Virtual(SuppliesEnumerator).Store(0);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(0).Call(SuppliesCurrent).Store(1);
        b.Load(1).Field(0x04000590).Store(2);
        b.Load(2).Load(2).Field(0x0400067A).Load(1).Field(0x04000591).Op(0x59).Set(0x0400067A);
        b.Label("move").Address(0).Call(SuppliesMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(0).Call(0x0A00010C).Op(0xDC);
        b.Label("after").Static(0x0400015E).Field(0x04000166).Field(0x04000310).Store(3);
        b.Load(3).Load(3).Field(0x0400063B).Arg(0).Field(0x0400071B).Op(0x59).Set(0x0400063B);
        b.Arg(0).Call(0x06000581);b.Arg(0).Call(0x06000578);
        b.Arg(0).Field(0x040006FA).Integer(-1).Virtual(0x06000599).Op(0x2A);
        RegisterFinally(0x06000573,b,"try","finally","after");return b.Finish();
    }

    private static byte[] N104_CloseVfxBody()
    {
        var b=new IL();
        b.Arg(0).Field(0x040000D0).Op(0x14).Call(0x0A00000C).Branch(0x39,"kind");
        b.Arg(0).Field(0x040000D0).Field(0x040004F0).Store(0);
        b.Load(0).Arg(0).Virtual(0x0A000009).Virtual(0x0A000089).Op(0x26);
        b.Label("kind").Arg(0).Field(0x040000CF).Integer(30).Branch(0x3B,"short");
        b.Arg(0).Field(0x040000CF).Integer(35).Branch(0x3B,"long");
        b.Arg(0).Virtual(0x0A000009).Call(0x0A000025).Op(0x2A);
        b.Label("short").Arg(0).Virtual(0x2B000010).Ref(0x72,0x70001E0C).Virtual(0x0A000087);
        b.Arg(0).Virtual(0x0A000009).Float(.4f).Call(0x0A000088).Op(0x2A);
        b.Label("long").Arg(0).Virtual(0x2B000010).Ref(0x72,0x70001E0C).Virtual(0x0A000087);
        b.Arg(0).Virtual(0x0A000009).Float(2).Call(0x0A000088).Op(0x2A);return b.Finish();
    }
}
