internal static partial class Program
{
    private static byte[] N114_CreateIceCubeBody()
    {
        var b=new IL();
        b.Integer(3).New(0x060002FB).Store(0);
        b.Load(0).Ref(0x72,0x70005223).Set(0x04000454);
        b.Load(0).Float(6000).Set(0x0400045A);
        b.Load(0).Integer(2).Set(0x04000455);
        b.Load(0).Float(6000).Set(0x0400045B);
        // The original parameterized constructor includes its default DeviceData(-1).
        b.Arg(1).Arg(2).Load(0).Static(0x0A000013).New(0x0600017D).Store(1);
        b.New(0x060002FE).Store(2);b.Load(2).Integer(2).Set(0x0400045C);
        b.Integer(0).Integer(5).Call(0x0A00012B).Store(3);
        b.Load(3).Integer(1).Branch(0x3B,"id2");
        b.Load(3).Integer(2).Branch(0x3B,"id4");
        b.Load(3).Integer(3).Branch(0x3B,"id14");
        b.Load(3).Integer(4).Branch(0x3B,"id7");
        b.Integer(0).Store(3).Branch(0x38,"content");
        b.Label("id2").Integer(2).Store(3).Branch(0x38,"content");
        b.Label("id4").Integer(4).Store(3).Branch(0x38,"content");
        b.Label("id14").Integer(14).Store(3).Branch(0x38,"content");
        b.Label("id7").Integer(7).Store(3);
        b.Label("content").Load(2).Load(3).Set(0x0400045D);
        b.Load(0).Load(2).Set(0x04000459);
        b.Load(1).Load(0).Set(0x040001D3);
        b.Arg(0).Field(0x040001D7).Load(1).Virtual(PreDeviceAdd).Op(0x2A);
        return b.Finish();
    }

    private static byte[] N114_DeviceCharacteristicBody()
    {
        var b=new IL();
        b.Arg(0).Field(0x0400047C).Branch(0x39,"return");
        b.Arg(0).Field(0x04000463).Op(0x14).Call(0x0A000012).Branch(0x3A,"return");
        b.Arg(0).Field(0x04000463).Field(0x040003BA).Branch(0x39,"return");
        b.Arg(0).Field(0x0400046F).Integer(2).Branch(0x40,"id9");
        b.Arg(0).Call(0x06000324).Call(0x0A00000B).Branch(0x39,"id9");
        b.Arg(0).Field(0x04000486).Ref(0x72,0x7000C2E1).Virtual(0x0A000087);
        b.Label("id9").Arg(0).Field(0x0400046F).Integer(9).Branch(0x40,"id10");
        b.Arg(0).Field(0x04000474).Store(0);
        b.Load(0).Float(0).Branch(0x43,"id10");
        b.Arg(0).Load(0).Call(0x0A00001A).Float(10).Op(0x5A,0x59).Set(0x04000474);
        b.Arg(0).Call(0x0600031F);
        b.Arg(0).Field(0x04000474).Float(0).Branch(0x42,"bonfireScale");
        b.Arg(0).Float(0).Set(0x04000474);b.Arg(0).Call(0x0600030F);
        b.Label("bonfireScale").Arg(0).Field(0x04000474).Arg(0).Field(0x04000475).Op(0x5B,0x6B).Store(0);
        // Native divss precedes conversion to double for Math.Pow(..., .5).
        b.Load(0).Op(0x6C).Float(.5f).Op(0x6C).Call(0x0A0001A9).Op(0x6B).Store(0);
        b.Arg(0).Field(0x04000485).Virtual(0x0A00002E).Integer(1).Virtual(0x0A000002);
        b.Load(0).Load(0).Float(0).New(0x0A00009C).Virtual(0x0A0000E5);
        b.Label("id10").Arg(0).Field(0x0400046F).Integer(10).Branch(0x40,"id13");
        b.Arg(0).Call(0x06000321);
        b.Label("id13").Arg(0).Field(0x0400046F).Integer(13).Branch(0x40,"id14");
        b.Arg(0).Field(0x0400047D).Branch(0x3A,"id14");
        b.Arg(0).Field(0x0400048A).Op(0x14).Call(0x0A000012).Branch(0x3A,"portalBroken");
        b.Arg(0).Field(0x0400048A).Virtual(0x06000464).Branch(0x3A,"portalBroken");
        b.Arg(0).Field(0x0400047F).Store(0);b.Load(0).Float(0).Branch(0x43,"portalHealth");
        b.Load(0).Call(0x0A00001A).Op(0x59,0x6B).Store(0);b.Arg(0).Load(0).Set(0x0400047F);
        b.Load(0).Float(0).Branch(0x42,"portalHealth");
        b.Arg(0).Arg(0).Field(0x04000480).Set(0x0400047F);b.Arg(0).Call(0x0600031A);
        b.Label("portalHealth").Arg(0).Field(0x0400048A).Field(0x040005C1).Integer(3).Branch(0x3C,"id14");
        b.Arg(0).Field(0x04000474).Store(0);b.Load(0).Float(0).Branch(0x43,"id14");
        b.Load(0).Call(0x0A00001A).Float(80).Op(0x5A,0x59,0x6B).Store(0);
        b.Arg(0).Load(0).Set(0x04000474);b.Load(0).Float(0).Branch(0x42,"id14");
        b.Arg(0).Float(0).Set(0x04000474);
        b.Label("portalBroken").Arg(0).Call(0x0600030F);
        b.Label("id14").Arg(0).Field(0x0400046F).Integer(14).Branch(0x40,"return");
        b.Arg(0).Field(0x0400047E).Float(3).Branch(0x3D,"expired");
        b.Arg(0).Field(0x04000481).Branch(0x39,"secondary");
        b.Arg(0).Field(0x0400047E).Float(1).Branch(0x43,"secondary");
        b.Arg(0).Call(0x06000316);b.Arg(0).Integer(0).Set(0x04000481);
        b.Label("secondary").Arg(0).Field(0x0400047E).Float(1).Branch(0x43,"return");
        b.Arg(0).Field(0x0400047F).Store(0);b.Load(0).Float(0).Branch(0x43,"return");
        b.Load(0).Call(0x0A00001A).Op(0x59,0x6B).Store(0);b.Arg(0).Load(0).Set(0x0400047F);
        b.Load(0).Float(0).Branch(0x42,"return");
        for(int i=0;i<3;i++)b.Arg(0).Call(0x06000317);
        b.Arg(0).Arg(0).Field(0x04000480).Set(0x0400047F).Branch(0x38,"return");
        b.Label("expired").Arg(0).Call(0x0600030F);
        b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N114_PlantAttackBody()
    {
        var b=new IL();
        b.Arg(0).Field(0x040004CA).Float(0).Branch(0x43,"return");
        b.Arg(0).Field(0x040004C3).Branch(0x39,"return");
        b.Arg(0).Field(0x040004F3).Field(0x040003B9).Branch(0x39,"countdown");
        b.Arg(0).Field(0x040004B1).Integer(1).Branch(0x3B,"return");
        b.Arg(0).Field(0x040004B1).Integer(9).Branch(0x3B,"return");
        b.Label("countdown").Arg(0).Field(0x040004CB).Store(0);b.Load(0).Float(0).Branch(0x43,"return");
        b.Arg(0).Field(0x040004B1).Integer(1).Branch(0x3B,"producer");
        b.Arg(0).Field(0x040004B1).Integer(9).Branch(0x3B,"producer");
        b.Load(0).Call(0x0A00001A).Arg(0).Call(0x06000366).Op(0x5A,0x59,0x6B).Store(1);
        b.Arg(0).Load(1).Set(0x040004CB);b.Load(1).Float(0).Branch(0x42,"return");
        b.Arg(0).Call(0x06000396).Branch(0x39,"restoreOld");
        b.Arg(0).Arg(0).Field(0x040004CA).Set(0x040004CB).Branch(0x38,"return");
        // Original xmm9 retains the pre-decrement value across PlantAttackStartTry.
        b.Label("restoreOld").Arg(0).Load(0).Set(0x040004CB).Branch(0x38,"return");
        b.Label("producer").Arg(0).Field(0x040004F3).Field(0x040003B9).Branch(0x3A,"return");
        b.Load(0).Call(0x0A00001A).Arg(0).Call(0x06000366).Op(0x5A,0x59,0x6B).Store(1);
        b.Arg(0).Load(1).Set(0x040004CB);b.Load(1).Float(1).Branch(0x42,"producerDue");
        b.Arg(0).Float(3).Load(1).Load(1).Op(0x58,0x59).Set(0x040004F5);
        b.Label("producerDue").Load(1).Float(0).Branch(0x42,"return");
        b.Arg(0).Field(0x040004B1).Integer(1).Branch(0x40,"sound");
        b.Arg(0).Field(0x040004DF).Ref(0x72,0x7000CD94).Call(0x06000141).Store(2);
        b.Arg(0).Field(0x040004F3).Field(0x040003D7).Integer(1).Integer(0).Load(2).Virtual(0x06000201).Store(3);
        b.Arg(0).Field(0x040004B9).Store(1);N102_EmitCvtt(b,1,"sunValue");b.Store(4);
        b.Load(3).Load(4).Virtual(0x060003CE);b.Load(3).Arg(0).Call(0x2B000059);
        b.Load(3).Virtual(0x060003CC);
        b.Arg(0).Field(0x040004B4).Integer(1).Branch(0x3F,"sound");b.Arg(0).Call(0x06000394);
        b.Label("sound").Arg(0).Field(0x040004F1).Integer(0).Op(0x9A).Store(5);
        b.Load(5).Call(0x06000138).Virtual(0x0A0000F5);
        b.Arg(0).Field(0x040004F1).Integer(0).Op(0x9A).Virtual(0x0A0000F8);
        b.Arg(0).Field(0x040004D2).Store(6);b.Arg(0).Arg(0).Field(0x040004CA).Set(0x040004CB);
        b.Load(6).Branch(0x39,"return");b.Arg(0).Field(0x040004D1).Store(7);
        b.Load(7).Field(0x0400068E).Integer(1).Branch(0x40,"return");b.Load(7).Arg(0).Virtual(0x060004E0);
        b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N114_ProjectCollectBody()
    {
        var b=new IL();
        b.Arg(0).Virtual(0x0A000001).Virtual(0x0A000037).Store(0);
        b.Arg(0).Field(0x04000521).Store(1);
        foreach(var a in new[]{(Field:0x0A00001D,Local:3),(Field:0x0A00008B,Local:4),(Field:0x0A00009D,Local:5)})
            b.Address(0).Field(a.Field).Address(1).Field(a.Field).Op(0x59,0x6B).Store(a.Local);
        b.Load(4).Load(4).Op(0x5A,0x6B).Load(3).Load(3).Op(0x5A,0x6B,0x58,0x6B);
        b.Load(5).Load(5).Op(0x5A,0x6B,0x58,0x6B,0x6C).Call(0x0A00009F).Op(0x6B).Store(3);
        b.Load(3).Float(1).Branch(0x3E,"collect");
        foreach(int id in new[]{0,1,2,3,8})b.Arg(0).Field(0x04000516).Integer(id).Branch(0x3B,"near");
        b.Branch(0x38,"return");
        b.Label("near").Load(3).Float(25).Branch(0x3E,"scale").Branch(0x38,"return");
        b.Label("scale").Arg(0).Virtual(0x0A000001).Store(2);b.Arg(0).Field(0x04000518).Store(6);
        b.Call(0x0A000209).Store(1);
        foreach(var a in new[]{(Field:0x0A00001D,Local:4),(Field:0x0A00008B,Local:5),(Field:0x0A00009D,Local:6)})
        {
            // Retain size separately until every component has been evaluated.
            b.Address(1).Field(a.Field).Load(6).Op(0x5A,0x6B).Load(3).Op(0x5A,0x6B).Float(25).Op(0x5B,0x6B);
        }
        b.New(0x0A00009C).Store(1);b.Load(2).Load(1).Virtual(0x0A0000E5);
        b.Integer(0).Store(7).Branch(0x38,"spriteTest");
        b.Label("sprite").Arg(0).Field(0x04000526).Load(7).Virtual(0x0A000006).Call(0x2B000011).Store(8);
        b.Load(8).Float(1).Float(1).Float(1).Load(3).Float(25).Op(0x5B).New(0x0A0000BE).Virtual(0x0A000097);
        b.Load(7).Integer(1).Op(0x58).Store(7);
        b.Label("spriteTest").Load(7).Arg(0).Field(0x04000526).Virtual(0x0A000008).Branch(0x3F,"sprite").Branch(0x38,"return");
        b.Label("collect").Arg(0).Field(0x04000515).Store(7);
        b.Load(7).Integer(0).Branch(0x3B,"sun");b.Load(7).Integer(4).Branch(0x3B,"stop");
        foreach(int t in new[]{1,2,8})b.Load(7).Integer(t).Branch(0x3B,"project");
        b.Branch(0x38,"return");
        b.Label("sun").Arg(0).Field(0x0400051E).Field(0x040003DB).Arg(0).Virtual(0x06000264).Branch(0x38,"destroy");
        b.Label("project").Arg(0).Field(0x0400051E).Field(0x040003D7).Arg(0).Virtual(0x06000200);
        b.Label("destroy").Arg(0).Virtual(0x0A000009).Call(0x0A000025).Branch(0x38,"return");
        b.Label("stop").Arg(0).Call(0x0A0000FC).Set(0x04000522);
        b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N114_LightSaberExplosionBody()
    {
        var b=new IL();
        b.Arg(0).Field(0x04000571).Op(0x14).Call(0x0A000012).Branch(0x3A,"return");
        b.Arg(0).Field(0x04000571).Arg(0).Integer(21).Integer(1).Virtual(0x0600036D).Virtual(0x0600010C).Op(0x26);
        b.Arg(0).Virtual(0x0A000001).Virtual(0x0A000037).Store(0);
        b.Address(0).Address(0).Field(0x0A00008B).Float(20).Op(0x58).Set(0x0A00008B);
        b.Call(0x060001E5).Integer(18).Virtual(0x060001EA).Store(1);
        b.Load(1).Op(0x14).Call(0x0A00000C).Branch(0x39,"audio");
        b.Load(1).Virtual(0x0A000019).Virtual(0x0A00002E).Load(0).Virtual(0x0A000038);
        b.Load(1).Call(0x2B000057).Ref(0x72,0x700044D1).Virtual(0x0A00015A);
        b.Label("audio").Static(0x040002F1).Integer(27).Virtual(0x0A000084).Store(2);
        b.Call(0x0A000004).Virtual(0x0A000001).Virtual(0x0A000037).Store(0);
        b.Load(2).Load(0).Call(0x06000138).Float(.75f).Op(0x5A).Call(0x0600013D).Op(0x26);
        // Capture the current list before Random.Range; its callback may replace the static.
        b.Static(0x040002F1).Store(3);b.Integer(0).Integer(2).Call(0x0A00012B).Integer(17).Op(0x58).Store(4);
        b.Load(3).Load(4).Virtual(0x0A000084).Store(2);
        b.Call(0x0A000004).Virtual(0x0A000001).Virtual(0x0A000037).Store(0);
        b.Load(2).Load(0).Call(0x06000138).Float(1.25f).Op(0x5A).Call(0x0600013D).Op(0x26);
        b.Arg(0).Call(0x060003EC);b.Arg(0).Field(0x04000571).Virtual(0x0600038E);
        b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N114_Armor2StatusBody()
    {
        var b=new IL();
        b.Arg(0).Field(0x040005C8).Branch(0x39,"kind");
        b.Arg(0).Field(0x040005C9).Float(0).Branch(0x3E,"drop");
        b.Label("kind").Arg(0).Field(0x040005C8).Integer(1).Branch(0x3B,"screen");
        b.Arg(0).Field(0x040005C8).Integer(3).Branch(0x3B,"heavy");
        b.Arg(0).Field(0x040005C8).Integer(4).Branch(0x3B,"ladder");b.Branch(0x38,"return");
        foreach(var c in new[]{(Label:"screen",Text:0x700102A9,Severe:11,Mild:10),(Label:"heavy",Text:0x70010275,Severe:31,Mild:30),(Label:"ladder",Text:0x70008458,Severe:35,Mild:34)})
        {
            b.Label(c.Label).Arg(0).Field(0x04000609).Ref(0x72,c.Text).Call(0x06000142).Store(0);
            b.Op(0x14).Store(1);b.Load(0).Call(0x0A00000B).Branch(0x39,c.Label+"levels");
            b.Load(0).Call(0x2B000011).Store(1);
            b.Label(c.Label+"levels").Arg(0).Field(0x040005CD).Integer(2).Branch(0x3C,c.Label+"mild");
            b.Arg(0).Field(0x040005CA).Float(3).Op(0x5B,0x6B).Arg(0).Field(0x040005C9).Branch(0x3D,c.Label+"severe");
            b.Label(c.Label+"mild").Arg(0).Field(0x040005CD).Integer(1).Branch(0x3C,"return");
            b.Arg(0).Field(0x040005CA).Store(2);b.Load(2).Load(2).Op(0x58,0x6B).Float(3).Op(0x5B,0x6B);
            b.Arg(0).Field(0x040005C9).Branch(0x3D,c.Label+"mildSet").Branch(0x38,"return");
            b.Label(c.Label+"severe").Load(1).Static(0x040002CE).Integer(c.Severe).Call(0x0600014B).Branch(0x38,"return");
            b.Label(c.Label+"mildSet").Load(1).Static(0x040002CE).Integer(c.Mild).Call(0x0600014B).Branch(0x38,"return");
        }
        b.Label("drop").Arg(0).Arg(1).Call(0x0600043A);
        b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N114_SwfPropertyBlockBody()
    {
        var b=new IL();
        b.Arg(0).Field(0x04000A6F).Call(0x0A00000B).Branch(0x39,"return");
        b.Arg(0).Field(0x04000A73).Branch(0x3A,"getBlock");
        b.Arg(0).New(0x0A00039E).Set(0x04000A73);
        b.Label("getBlock").Arg(0).Field(0x04000A6F).Arg(0).Field(0x04000A73).Virtual(N114_GetPropertyBlock);
        b.Arg(0).Field(0x04000A73).Store(0);b.Call(0x060008FA);
        b.Load(0).Static(0x04000AD5).Arg(0).Field(0x04000A79).Virtual(0x0A0003A0);
        b.Op(0x14).Store(1);b.Arg(0).Field(0x04000A7A).Call(0x0A00000B).Branch(0x39,"mainTexture");
        b.Arg(0).Field(0x04000A7A).Field(0x04000A7E).Store(1);
        b.Label("mainTexture").Load(1).Call(0x0A00000B).Branch(0x39,"white");
        b.Load(1).Virtual(0x0A0003A1).Call(0x0A00000B).Branch(0x39,"white");
        b.Load(1).Virtual(0x0A0003A1).Store(2).Branch(0x38,"alpha");
        b.Label("white").Call(0x0A0003A2).Store(2);
        b.Label("alpha").Op(0x14).Store(3);b.Load(1).Call(0x0A00000B).Branch(0x39,"setMain");
        b.Load(1).Virtual(0x0A0003A3).Store(3);
        b.Label("setMain").Arg(0).Field(0x04000A73).Store(0);b.Call(0x060008FA);b.Static(0x04000AD6).Store(4);
        b.Load(2).Call(0x0A00000B).Branch(0x3A,"mainReady");b.Call(0x0A0003A2).Store(2);
        b.Label("mainReady").Load(0).Load(4).Load(2).Virtual(N114_SetTexture);
        b.Load(3).Call(0x0A00000B).Branch(0x3A,"external");
        b.Arg(0).Field(0x04000A73).Store(0);b.Call(0x060008FA);
        b.Load(0).Static(0x04000AD7).Call(0x0A0003A2).Virtual(N114_SetTexture);
        b.Arg(0).Field(0x04000A73).Store(0);b.Call(0x060008FA);
        b.Load(0).Static(0x04000AD8).Float(0).Virtual(N114_SetFloat).Branch(0x38,"apply");
        b.Label("external").Arg(0).Field(0x04000A73).Store(0);b.Call(0x060008FA);
        b.Load(0).Static(0x04000AD7).Load(3).Virtual(N114_SetTexture);
        b.Arg(0).Field(0x04000A73).Store(0);b.Call(0x060008FA);
        b.Load(0).Static(0x04000AD8).Float(1).Virtual(N114_SetFloat);
        b.Label("apply").Arg(0).Field(0x04000A6F).Arg(0).Field(0x04000A73).Virtual(N114_SetPropertyBlock);
        b.Label("return").Op(0x2A);return b.Finish();
    }
}
