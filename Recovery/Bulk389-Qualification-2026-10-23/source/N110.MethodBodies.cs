internal static partial class Program
{
    // The two original PC bodies have identical observable operations and constants.
    private static byte[] N110_ZombieBoomBody()
    {
        var b=new IL();
        b.Arg(0).Integer(-1).Op(0x14).Integer(0).Call(0x06000443).Virtual(0x0600010C).Op(0x26);
        b.Arg(0).Call(0x06000436);
        b.Static(0x040002F1).Integer(1).Virtual(0x0A000084).Store(0);
        b.Load(0).Call(0x0A00000B).Branch(0x39,"particle");
        b.Call(0x0A000004).Virtual(0x0A000001).Virtual(0x0A000037).Store(2);
        b.Load(0).Load(2).Call(0x06000138).Float(0.9f).Float(1.1f).Call(0x0A000083).Call(0x0600013E).Op(0x26);
        b.Label("particle").Call(0x060001E5).Integer(2).Virtual(0x060001EA).Store(1);
        b.Load(1).Call(0x0A00000B).Branch(0x39,"return");
        b.Load(1).Virtual(0x0A00002E).Arg(0).Field(0x040005AD);
        b.Arg(0).Field(0x040005AE).Arg(0).Field(0x040005AF).Op(0x58).Float(0).New(0x0A00009C).Virtual(0x0A000038);
        b.Load(1).Virtual(0x0A00002E).Float(1.2f).Float(1.2f).Float(1.2f).New(0x0A00009C).Virtual(0x0A0000E5);
        b.Arg(0).Field(0x040005AA).Float(0.15f).Float(8).Virtual(0x060002CD);
        b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N110_DevicePreviewBody()
    {
        var b=new IL();
        b.Arg(1).Field(0x040001D3).Field(0x04000456).Store(2);
        b.Arg(0).Field(0x04000787).Store(3);
        // The PC comparison is Count < ID, not Count <= ID. ID==Count must throw in get_Item.
        b.Load(3).Virtual(N110_DeviceListCount).Load(2).Branch(0x3F,"noPrefab");
        b.Load(3).Load(2).Virtual(0x0A000127).Store(0);
        b.Load(0).Op(0x14).Call(0x0A00000C).Branch(0x3A,"prefab");
        b.Label("noPrefab").Op(0x14).Store(0);
        b.Label("prefab").Load(0).Call(0x0A00000B).Branch(0x39,"return");
        b.Load(0).Call(0x2B000027).Store(0);
        b.Load(0).Op(0x14).Set(0x04000463);
        b.Load(0).Arg(1).Field(0x040001D3).Virtual(0x0600032C);
        b.Arg(0).Field(0x04000781).Arg(1).Field(0x040001D0).Arg(1).Field(0x040001D1).Virtual(0x0600015F).Store(1);
        b.Address(1).Address(1).Field(0x0A00001D).Float(52).Op(0x59).Set(0x0A00001D);
        b.Load(0).Load(1).Virtual(0x06000331);
        b.Load(0).Virtual(0x0A000001).Arg(0).Virtual(0x0A000001).Virtual(N110_SetParent);
        b.Load(0).Arg(1).Field(0x040001D0).Set(0x04000464);
        b.Load(0).Arg(1).Field(0x040001D1).Set(0x04000465);
        b.Load(0).Virtual(0x06000332);b.Load(0).Virtual(0x06000335);b.Load(0).Virtual(0x0600032F);
        b.Load(0).Integer(0).Set(0x0400047C);
        b.Arg(0).Field(0x04000786).Load(0).Virtual(N110_DeviceListAdd);
        b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N110_ZombiePreviewBody()
    {
        var b=new IL();
        b.Static(0x040002F7).Arg(1).Field(0x040001F7).Virtual(0x0A00006C).Store(0);
        b.Load(0).Op(0x14).Call(0x0A000012).Branch(0x3A,"missing");
        // Re-read the dictionary and key after the Unity equality callback, as in PC native.
        b.Static(0x040002F7).Arg(1).Field(0x040001F7).Virtual(0x0A00006C).Call(0x2B00004F).Store(0);
        b.Arg(1).Field(0x040001FA).Store(1);
        b.Address(1).Address(1).Field(0x0A00001D).Float(52).Op(0x59).Set(0x0A00001D);
        b.Load(0).Virtual(0x0A000001).Load(1).Virtual(0x0A000038);
        b.Load(0).Virtual(0x0A000009).Virtual(0x0A00002E).Arg(0).Field(0x0400078A).Integer(0).Virtual(0x0A00002F);
        b.Load(0).Arg(1).Field(0x040001F8).Set(0x040005AC);
        b.Load(0).Arg(1).Field(0x040001FC).Set(0x040005F6);
        b.Load(0).Op(0x14).Set(0x040005F9);
        b.Load(0).Arg(1).Field(0x040001FB).Set(0x040005BA);
        b.Arg(0).Field(0x04000785).Load(0).Virtual(N110_ZombieListAdd);
        // Two separate position reads: a callback in the first getter may replace shadow.
        b.Load(0).Field(0x04000613).Virtual(0x0A000037).Store(1);
        b.Load(0).Address(1).Field(0x0A00001D).Set(0x040005AD);
        b.Load(0).Field(0x04000613).Virtual(0x0A000037).Store(1);
        b.Load(0).Address(1).Field(0x0A00008B).Set(0x040005AE);
        b.Load(0).Virtual(0x06000477);b.Load(0).Integer(0).Set(0x040005D7);
        b.Static(0x0400015E).Field(0x04000166).Field(0x04000310).Field(0x0400063E);
        b.Arg(1).Field(0x040001F7).Integer(1).Op(0x9C).Op(0x2A);
        b.Label("missing").Arg(1).Field(0x040001F7).Store(2);
        b.Ref(0x72,0x7001A482).Address(2).Call(0x0A00002A).Ref(0x72,0x7001A48E).Call(0x0A00010B).Call(0x0A000010).Op(0x2A);
        return b.Finish();
    }

    private static byte[] N110_ZombieSelectInitializeBody()
    {
        var b=new IL();
        b.Arg(1).Op(0x14).Call(0x0A000012).Branch(0x3A,"empty");
        b.Arg(0).Arg(1).Call(0x2B00004F).Set(0x04000913);
        b.Arg(0).Field(0x04000913).Integer(0).Set(0x040005D7);
        b.Arg(0).Field(0x04000913).Field(0x040005FF).Call(0x0A00000B).Branch(0x3A,"sorting");
        b.Arg(0).Field(0x04000913).Store(2);
        b.Load(2).Load(2).Call(0x2B00002A).Set(0x040005FF);
        b.Label("sorting").Arg(0).Field(0x04000913).Field(0x040005FF).Ref(0x72,0x70017224).Virtual(0x0A00015A);
        b.Arg(0).Field(0x04000913).Field(0x040005FF).Integer(10000).Virtual(0x0A00015B);
        b.Arg(0).Field(0x04000913).Virtual(0x0A000001).Arg(0).Virtual(0x0A000001).Virtual(N110_SetParent);
        b.Arg(0).Field(0x04000913).Virtual(0x0A000001).Call(0x0A0000FC).Virtual(0x0A000042);
        b.Arg(0).Arg(2).Set(0x04000916);
        b.Arg(1).Field(0x040005B9).Call(0x06000230).Store(0);
        b.Load(0).Branch(0x39,"image");
        b.Load(0).Field(0x0400017D).Branch(0x3A,"locked");
        b.Load(0).Field(0x0400017E).Store(1).Branch(0x38,"lockUI");
        b.Label("locked").Integer(1).Store(1);
        b.Label("lockUI").Arg(0).Field(0x04000914).Load(1).Integer(0).Op(0xFE,0x01).Virtual(0x0A000018);
        b.Arg(0).Field(0x04000915).Virtual(0x0A000009).Load(1).Virtual(0x0A000007).Branch(0x38,"image");
        b.Label("empty").Arg(0).Integer(0).Set(0x04000916);
        b.Arg(0).Call(0x0600070F); // Native has this early call AND the common tail call.
        b.Arg(0).Field(0x04000914).Integer(0).Virtual(0x0A000018);
        b.Arg(0).Field(0x04000915).Virtual(0x0A000009).Integer(1).Virtual(0x0A000007);
        b.Label("image").Arg(0).Call(0x0600070F).Op(0x2A);return b.Finish();
    }

    private static byte[] N110_InstructionOverlayAwakeBody()
    {
        var b=new IL();
        b.Arg(0).Virtual(0x0A000272).Branch(0x39,"return");
        b.Arg(0).Call(0x0A000004).Set(0x040009E6);
        b.Ref(0x72,0x7002165B).New(0x0A0000F3).Store(0);
        b.Arg(0).Load(0).Virtual(0x0A00002E).Set(0x040009E5);
        b.Arg(0).Field(0x040009E5).Arg(0).Field(0x040009E6).Virtual(0x0A000001).Virtual(0x0A000357);
        b.Arg(0).Field(0x040009E5).Call(0x0A00037E).Virtual(0x0A000358);
        b.Arg(0).Load(0).Virtual(0x2B00009A).Set(0x040009E3);
        b.Arg(0).Field(0x040009E3).Ref(0x72,0x70021677).Call(0x2B0000A2).Virtual(0x0A0002F9);
        b.Arg(0).Field(0x040009E3).Ref(0x72,0x700216C1).Call(0x2B00009B).Virtual(0x0A000359);
        b.Arg(0).Field(0x040009E3).Float(30).Virtual(0x0A0002A3);
        b.Arg(0).Field(0x040009E3).Integer(1).Virtual(0x0A00035A);
        b.Arg(0).Load(0).Virtual(0x2B0000A3).Set(0x040009E4);
        b.Arg(0).Arg(0).Field(0x040009E1).Call(0x060007AD);
        b.Arg(0).Field(0x040009E3).Ref(0x72,0x7002171F).Virtual(0x0A000062);
        b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N110_FrameRateAwakeBody()
    {
        var b=new IL();
        b.Arg(0).Virtual(0x0A000272).Branch(0x39,"return");
        b.Arg(0).Call(0x0A000004).Set(0x040009FC);b.Integer(9999).Call(0x0A000251);
        b.Ref(0x72,0x7002165B).New(0x0A0000F3).Store(0);
        b.Arg(0).Load(0).Virtual(0x2B00009A).Set(0x040009FA);
        b.Arg(0).Field(0x040009FA).Ref(0x72,0x70021677).Call(0x2B0000A2).Virtual(0x0A0002F9);
        b.Arg(0).Field(0x040009FA).Ref(0x72,0x700216C1).Call(0x2B00009B).Virtual(0x0A000359);
        b.Arg(0).Load(0).Virtual(0x0A00002E).Set(0x040009FB);
        b.Arg(0).Field(0x040009FB).Arg(0).Field(0x040009FC).Virtual(0x0A000001).Virtual(N110_SetParent);
        b.Arg(0).Field(0x040009FB).Call(0x0A00037E).Virtual(0x0A000358);
        b.Arg(0).Field(0x040009FA).Integer(0).Virtual(0x0A0002FC);
        b.Arg(0).Field(0x040009FA).Float(24).Virtual(0x0A0002A3);
        b.Arg(0).Arg(0).Field(0x040009F7).Call(0x060007B5);
        b.Arg(0).Arg(0).Field(0x040009F7).Set(0x040009FD);
        b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N110_ExampleAwakeBody()
    {
        var b=new IL();
        b.Arg(0).Field(0x040009EC).Branch(0x39,"mesh");
        b.Arg(0).Virtual(0x2B000092).Store(0);b.Load(0).Branch(0x3A,"text");
        b.Arg(0).Virtual(0x0A000009).Virtual(0x2B00009D).Store(0).Branch(0x38,"text");
        b.Label("mesh").Arg(0).Virtual(0x2B0000A4).Store(0);b.Load(0).Branch(0x3A,"text");
        b.Arg(0).Virtual(0x0A000009).Virtual(0x2B00009A).Store(0);
        b.Label("text").Arg(0).Load(0).Set(0x040009EE);
        b.Arg(0).Field(0x040009EE).Ref(0x72,0x700217B6).Call(0x2B0000A2).Virtual(0x0A0002F9);
        b.Arg(0).Field(0x040009EE).Ref(0x72,0x700217EE).Call(0x2B00009B).Virtual(0x0A000359);
        b.Arg(0).Field(0x040009EE).Float(120).Virtual(0x0A0002A3);
        b.Arg(0).Field(0x040009EE).Ref(0x72,0x70021842).Virtual(0x0A000062);
        b.Arg(0).Field(0x040009EE).Float(float.PositiveInfinity).Float(float.PositiveInfinity).Virtual(0x0A00035D).Store(1);
        b.Arg(0).Field(0x040009EE).Virtual(0x0A0002E3).Load(1).Virtual(0x0A00028E).Op(0x2A);
        return b.Finish();
    }

    private static byte[] N110_FrameCounterPositionBody()
    {
        var b=new IL();
        b.Arg(0).Field(0x040009FA).Float(1).Float(1).Float(1).Float(1).New(N110_Vector4Ctor).Virtual(0x0A000360);
        for(int a=0;a<4;a++)b.Arg(1).Integer(a).Branch(0x3B,"anchor"+a);
        b.Op(0x2A);
        foreach(var p in new[]{(Arg:0,Align:257,X:0f,Y:1f),(Arg:1,Align:1025,X:0f,Y:0f),(Arg:2,Align:260,X:1f,Y:1f),(Arg:3,Align:1028,X:1f,Y:0f)})
        {
            b.Label("anchor"+p.Arg).Arg(0).Field(0x040009FA).Integer(p.Align).Virtual(0x0A0002FA);
            b.Address(0).Float(p.X).Set(0x0A0001D4);b.Address(0).Float(p.Y).Set(0x0A0001D5);
            b.Arg(0).Field(0x040009FA).Virtual(0x0A0002E3).Load(0).Virtual(0x0A000305).Branch(0x38,"position");
        }
        b.Label("position").Arg(0).Field(0x040009FC).Store(2);
        b.Arg(0).Field(0x040009FB).Store(1);
        b.Load(1).Load(2).Address(0).Field(0x0A0001D4).Address(0).Field(0x0A0001D5).Float(100).New(0x0A00009C).Virtual(0x0A00035C).Virtual(0x0A000038).Op(0x2A);
        return b.Finish();
    }

    private static byte[] N110_CreateSwordCircleBody()
    {
        var b=new IL();
        b.Arg(0).Field(0x040000CD).Op(0x14).Call(0x0A000012).Branch(0x3A,"return");
        b.Arg(0).Field(0x040000CD).Store(5);
        b.Load(5).Field(0x040004B4).Integer(0).Branch(0x3E,"return");
        b.Load(5).Field(0x040004D7).Integer(2).Branch(0x3F,"return");
        b.Integer(6).Ref(0x8D,0x01000013).Store(0);
        var offsets=new[]{(-80f,120f),(-57.5f,97.5f),(-57.5f,52.5f),(-80f,30f),(-102.5f,52.5f),(-102.5f,97.5f)};
        for(int i=0;i<offsets.Length;i++)b.Load(0).Integer(i).Float(offsets[i].Item1).Float(offsets[i].Item2).Float(0).New(0x0A00009C).Ref(0xA4,0x01000013);
        foreach(int i in new[]{0,2,4})b.Arg(1).Integer(i).Branch(0x3B,"level3");
        b.Branch(0x38,"create");
        b.Label("level3").Arg(0).Field(0x040000CD).Field(0x040004D7).Integer(3).Branch(0x3F,"return");
        b.Label("create").Call(0x060001E5).Integer(11).Virtual(0x060001EA).Store(1);
        b.Arg(0).Field(0x040000CD).Virtual(0x0A000001).Virtual(0x0A000037).Store(2);
        // Keep the bounds check AFTER particle creation and the position getter.
        b.Load(0).Arg(1).Ref(0xA3,0x01000013).Store(3);
        foreach(int f in new[]{0x0A00001D,0x0A00008B,0x0A00009D})b.Address(3).Field(f).Address(2).Field(f).Op(0x58);
        b.New(0x0A00009C).Store(2);
        b.Load(1).Call(0x0A00000B).Branch(0x39,"return");
        b.Load(1).Virtual(0x0A00002E).Load(2).Virtual(0x0A000038);
        b.Load(1).Virtual(0x2B00000F).Store(4);
        b.Load(4).Call(0x0A00000B).Branch(0x39,"return");
        b.Load(4).Arg(0).Field(0x040000CD).Set(0x040000CD);
        b.Label("return").Op(0x2A);return b.Finish();
    }
}
