internal static partial class Program
{
    // Source only. Native105 is not a candidate and has not been compiled or executed.
    private static byte[] N105_SpinObjectBody()
    {
        var b=new IL();b.Arg(0).Field(0x04000982).Store(0);b.Arg(0).Field(0x0400098B).Store(8);
        b.Load(8).Branch(0x39,"rotate");b.Arg(0).Field(0x04000985).Store(1);
        b.Load(8).Integer(1).Branch(0x3B,"oscillateRotation");
        b.Call(0x0A00001A).Store(3);b.Arg(0).Field(0x04000984).Store(2);
        b.Load(3).Load(0).Op(0x5A).Load(1).Op(0x58).Store(3);b.Arg(0).Load(3).Set(0x04000985);
        // Original 0x1803B4F65 calls cosf (0x1802CC3D0); exact CRT rounding remains unproven.
        b.Load(3).Float(0.95f).Op(0x5A).Call(0x0A00044B).Float(15).Op(0x5A);
        b.Arg(0).FieldAddress(0x04000988).Field(0x0A00001D).Op(0x58).Store(4);
        b.Arg(0).FieldAddress(0x04000988).Field(0x0A00008B).Float(0).Op(0x58).Store(5);
        b.Arg(0).FieldAddress(0x04000988).Field(0x0A00009D).Float(10).Op(0x58).Store(6);
        b.Load(2).Load(4).Load(5).Load(6).New(0x0A00009C).Virtual(0x0A000038);
        b.Arg(0).Field(0x04000984).Virtual(0x0A000037).Store(7);
        b.Arg(0).Arg(0).Field(0x0400098A).Integer(1).Op(0x58).Set(0x0400098A);
        b.Arg(0).Load(7).Set(0x04000986).Op(0x2A);
        b.Label("oscillateRotation").Call(0x0A00001A).Store(3);b.Arg(0).Field(0x04000984).Store(2);
        b.Load(3).Load(0).Op(0x5A).Load(1).Op(0x58).Store(3);b.Arg(0).Load(3).Set(0x04000985);
        b.Arg(0).FieldAddress(0x04000987).Field(0x0A00001D).Store(4);
        // Original 0x1803B5028 calls sinf (0x1802CCEF0).
        b.Load(3).Call(0x0A00044C).Arg(0).Field(0x04000983).Op(0x6B,0x5A);
        b.Arg(0).FieldAddress(0x04000987).Field(0x0A00008B).Op(0x58).Store(5);
        b.Arg(0).FieldAddress(0x04000987).Field(0x0A00009D).Store(6);
        // Public Euler performs the degree-to-radian conversion; never call private CoreModule internals.
        b.Load(2).Load(4).Load(5).Load(6).Call(0x0A00020E).Virtual(0x0A000319).Op(0x2A);
        b.Label("rotate").Arg(0).Field(0x04000984).Store(2);b.Call(0x0A00001A).Load(0).Op(0x5A).Store(3);
        b.Load(2).Float(0).Load(3).Float(0).Virtual(0x0A00032A).Op(0x2A);return b.Finish();
    }

    private static byte[] N105_RandomIceCubeBody()
    {
        var b=new IL();b.Arg(2).Virtual(0x06000289).Integer(1).Op(0x59).Store(0);
        b.Arg(2).Virtual(0x0600028A).Store(1);b.Integer(0).Store(2);
        b.Label("column").Load(2).Load(0).Branch(0x3C,"return");
        b.Load(2).Op(0x6C).Op(0x23).Op(BitConverter.GetBytes(2.75)).Op(0x59);
        b.Op(0x23).Op(BitConverter.GetBytes(4d)).Op(0x5A).Call(0x0A00012C).Store(4);
        b.Load(4).Load(4).Op(0x23).Op(BitConverter.GetBytes(1d)).Op(0x58);
        b.Op(0x23).Op(BitConverter.GetBytes(5d)).Op(0x5A,0x5B).Store(5);b.Integer(0).Store(3);
        b.Label("row").Load(3).Load(1).Branch(0x3C,"nextColumn");
        b.Float(0).Float(1).Call(0x0A000083).Op(0x6C).Store(6);
        b.Load(5).Load(6).Branch(0x44,"skip"); // blt.un: below OR unordered => do not create.
        b.Arg(0).Load(2).Load(3).Arg(1).Call(0x0600018C);
        b.Label("skip").Load(3).Integer(1).Op(0x58).Store(3).Branch(0x38,"row");
        b.Label("nextColumn").Load(2).Integer(1).Op(0x58).Store(2).Branch(0x38,"column");
        b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N105_DeviceUnderMouseBody()
    {
        var b=new IL();b.Arg(0).Field(0x04000230).Field(0x040003D8).Store(0);
        b.Arg(0).Field(0x04000234).Store(3);b.Arg(0).Field(0x04000235).Store(4);
        b.Load(0).Field(0x040001D4).Load(3).Load(4).Virtual(0x060002C4).Store(1);
        b.Op(0x14).Store(2);b.Load(1).Branch(0x39,"selected");
        foreach(int f in new[]{0x04000390,0x0400038F,0x0400038D,0x0400038C})
        {
            b.Load(1).Field(f).Call(0x0A00000B).Branch(0x39,"next"+f);
            b.Load(1).Field(f).Store(2).Branch(0x38,"selected");b.Label("next"+f);
        }
        b.Label("selected").Load(2).Op(0x14).Call(0x0A000012).Branch(0x3A,"null");
        b.Arg(1).Branch(0x39,"return");b.Load(2).Arg(2).Virtual(0x06000310).Branch(0x39,"null");
        b.Label("return").Load(2).Op(0x2A);b.Label("null").Op(0x14,0x2A);return b.Finish();
    }

    private static byte[] N105_SeekTargetPlantBody()
    {
        var b=new IL();b.Arg(0).Field(0x040005B9).Integer(13).Branch(0x3B,"null");
        b.Arg(0).Field(0x040005AA).Field(0x040003D4).Field(0x040002B1).Virtual(0x0A0000D3).Store(0);
        b.Op(0x14).Store(2);b.Label("try").Branch(0x38,"move");
        b.Label("body").Address(0).Call(0x0A0000D4).Store(1);
        b.Load(1).Field(0x040004B1).Integer(4).Branch(0x40,"eligible");
        b.Load(1).Field(0x040004B6).Integer(2).Branch(0x3B,"move");
        b.Label("eligible").Load(1).Field(0x040004C4).Branch(0x39,"move");
        b.Load(1).Field(0x040004A1).Arg(0).Field(0x040005AC).Branch(0x40,"move");
        b.Arg(0).Field(0x040005B5).Arg(0).Field(0x040005B4).Op(0x59).Store(3);
        b.Arg(0).Field(0x040005B5).Arg(0).Field(0x040005B4).Op(0x58).Float(0.5f).Op(0x5A);
        b.Arg(0).Field(0x040005AD).Op(0x58).Store(4);
        b.Arg(0).Load(1).Field(0x040004A4).Load(1).Field(0x040004A8).Load(4).Load(3).Call(0x06000470).Branch(0x39,"move");
        b.Load(1).Store(2).Branch(0xDD,"after");
        b.Label("move").Address(0).Call(0x0A0000D6).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(0).Call(0x0A00015D).Op(0xDC);b.Label("after").Load(2).Op(0x2A);
        b.Label("null").Op(0x14,0x2A);RegisterFinally(0x06000487,b,"try","finally","after");return b.Finish();
    }

    private static byte[] N105_MoveEnemyDataBody()
    {
        var b=new IL();b.Integer(0).Store(0);b.Integer(0).Store(1);
        for(int i=0;i<4;i++)b.Arg(1).Integer(i).Branch(0x3B,"direction"+i);b.Branch(0x38,"translate");
        for(int i=0;i<4;i++)
        {b.Label("direction"+i).Integer(i is 0 or 3 ? -1:1).Store(i is 0 or 2 ? 1:0).Branch(0x38,"translate");}
        b.Label("translate").Arg(0).Field(0x0400003B).Field(0x04000051).Op(0x25).Field(0x040001F9).Load(0).Op(0x58).Set(0x040001F9);
        b.Arg(0).Field(0x0400003B).Field(0x04000051).Op(0x25).Field(0x040001F8).Load(1).Op(0x58).Set(0x040001F8);
        b.Arg(0).Field(0x0400003B).Field(0x04000051).Field(0x040001FD).Virtual(0x0A00022A).Store(2);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(2).Call(0x0A00022B).Store(3);
        b.Load(3).Field(0x040003AA).Branch(0x3A,"move"); // native +0x29 is isEnd, not original (+0x2A).
        b.Load(3).Op(0x25).Field(0x040003A5).Load(0).Op(0x58).Set(0x040003A5);
        b.Load(3).Op(0x25).Field(0x040003A6).Load(1).Op(0x58).Set(0x040003A6);
        b.Label("move").Address(2).Call(0x0A00022C).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(2).Call(0x0A000045).Op(0xDC);
        b.Label("after").Arg(0).Field(0x0400003B).Virtual(0x0600004C).Op(0x2A);
        RegisterFinally(0x06000041,b,"try","finally","after");return b.Finish();
    }

    private static byte[] N105_CreateDamageTextBody()
    {
        var b=new IL();b.Static(0x04000300).Call(0x2B000088).Store(0);
        b.Load(0).Op(0x14).Call(0x0A00000C).Branch(0x3A,"initialize");
        b.Ref(0x72,0x7001B3F8).Call(0x0A000010).Load(0).Op(0x2A);
        b.Label("initialize").Load(0).Field(0x040007D2).Ref(0x72,0x7000F735).Arg(1).Virtual(0x0A0001FD);
        b.Arg(1).Branch(0x39,"damage");b.Arg(1).Integer(1).Branch(0x40,"position");
        b.Ref(0x72,0x7001B40A).Store(1).Branch(0x38,"text");b.Label("damage").Ref(0x72,0x7001B406).Store(1);
        b.Label("text").Load(0).Field(0x040007D1).ArgAddress(0).Load(1).Call(0x0A000125).Integer(1).Virtual(0x0A00004B);
        b.Label("position").Load(0).Virtual(0x0A000001).Arg(2).Virtual(0x0A000038).Load(0).Op(0x2A);return b.Finish();
    }

    private static byte[] N105_PlaceDeviceBody()
    {
        var b=new IL();b.Static(0x0400015E).Field(0x04000166).Store(0);
        b.Arg(1).Branch(0x3A,"data");b.New(0x0A000022).Op(0x7A);
        b.Label("data").Load(0).Field(0x04000310).Store(1);
        b.Load(1).Branch(0x3A,"save");b.New(0x0A000022).Op(0x7A);
        b.Label("save").Load(1).Arg(1).Field(0x04000456).Virtual(0x060004C6).Branch(0x3A,"prefab");
        b.Integer(3).Arg(1).Field(0x04000456).Arg(0).Field(0x040001D4).Call(0x06000672).Op(0x26);
        b.Label("prefab").Arg(0).Arg(1).Field(0x04000456).Call(0x06000189).Store(2);
        b.Load(2).Call(0x0A00000B).Branch(0x3A,"instantiate");b.New(0x0A000022).Op(0x7A);
        b.Label("instantiate").Load(2).Call(0x2B000027).Store(2);
        b.Load(2).Arg(0).Field(0x040001D4).Set(0x04000463);b.Load(2).Arg(1).Virtual(0x0600032C);
        b.Load(2).Arg(2).Arg(3).Virtual(0x06000339).Branch(0x3A,"return");
        b.Load(2).Virtual(0x0A000009).Call(0x0A000025).Op(0x14,0x2A);
        b.Label("return").Load(2).Op(0x2A);return b.Finish();
    }

    private static byte[] N105_UpdateProjectileSpeedBody()
    {
        var b=new IL();b.Arg(0).Field(0x0400055A).Store(0);
        b.Load(0).Integer(3).Branch(0x3B,"tracking");b.Load(0).Integer(4).Branch(0x3B,"arc");
        b.Load(0).Integer(6).Branch(0x3B,"height");b.Load(0).Integer(7).Branch(0x40,"return");
        b.Arg(0).Call(0x060003DB).Op(0x2A);
        b.Label("height").Arg(0).Arg(0).Field(0x04000554).Float(75).Op(0x59).Float(-5.2f).Op(0x5A).Set(0x04000564).Op(0x2A);
        b.Label("arc").Float(1.5f).Arg(0).Field(0x04000550).Branch(0x3C,"calculate");
        b.Arg(0).Float(0).Set(0x04000564);b.Arg(0).FieldAddress(0x04000561).Float(1500).Set(0x0A00001D);
        b.Arg(0).Call(0x060003D8);b.Arg(0).Integer(0).Set(0x0400055A).Op(0x2A);
        b.Label("calculate").Op(0x23).Op(BitConverter.GetBytes(10d)).Call(0x0A00012C).Op(0x6B).Store(1);
        b.Ref(0x72,0x7000F729).Address(1).Call(0x0A000043).Call(0x0A000051).Call(0x0A000010);
        b.Float(95).Arg(0).Field(0x04000554).Op(0x59).Store(2);
        b.Ref(0x72,0x7000F72F).Address(2).Call(0x0A000043).Call(0x0A000051).Call(0x0A000010);
        b.Arg(0).Field(0x04000550).Float(6).Op(0x5A).Load(1).Op(0x5A).Load(1).Op(0x58).Store(3);
        b.Arg(0).Load(2).Float(6).Op(0x5A).Load(3).Op(0x5B).Set(0x04000564);
        b.Label("tracking").Arg(0).Call(0x060003D8);b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N105_PlacePreDevicesBody()
    {
        var b=new IL();b.Integer(0).Store(0);
        b.Label("outer").Arg(0).Field(0x040001D7).Branch(0x39,"return");
        b.Arg(0).Field(0x040001D7).Virtual(PreDeviceCount).Integer(0).Branch(0x3E,"return");
        b.Op(0x14).Store(2);b.Arg(0).Field(0x040001D7).Virtual(PreDeviceEnumerator).Store(1);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(1).Call(PreDeviceCurrent).Store(3);
        b.Load(3).Field(0x040001D0).Load(0).Branch(0x40,"move");b.Load(3).Store(2).Branch(0xDD,"after");
        b.Label("move").Address(1).Call(PreDeviceMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(1).Call(0x0A000034).Op(0xDC);
        b.Label("after").Load(2).Branch(0x3A,"place");b.Load(0).Integer(1).Op(0x58).Store(0).Branch(0x38,"outer");
        b.Label("place").Arg(0).Load(2).Field(0x040001D3).Load(2).Field(0x040001D0).Load(2).Field(0x040001D1).Call(0x0600018A).Op(0x26);
        b.Arg(0).Field(0x040001D7).Load(2).Virtual(0x0A000408).Op(0x26).Branch(0x38,"outer");
        b.Label("return").Op(0x2A);RegisterFinally(0x06000184,b,"try","finally","after");return b.Finish();
    }

    private static byte[] N105_SelectPropBody()
    {
        var b=new IL();b.Arg(0).Field(0x0400089D).Virtual(PropControllerEnumerator).Store(0);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(0).Call(PropControllerCurrent).Store(1);
        b.Load(1).Field(0x040008A4).Arg(1).Call(0x0A000012).Branch(0x39,"move");b.Load(1).Op(0x14).Virtual(0x0600069A);
        b.Label("move").Address(0).Call(PropControllerMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(0).Call(0x0A00029C).Op(0xDC);
        b.Label("after").Arg(2).Integer(0).Branch(0x3F,"return");
        b.Arg(2).Arg(0).Field(0x0400089D).Virtual(PropControllerCount).Branch(0x3C,"return");
        b.Arg(0).Field(0x0400089D).Arg(2).Virtual(0x0A00029D).Arg(1).Virtual(0x0600069A);
        b.Label("return").Op(0x2A);RegisterFinally(0x06000698,b,"try","finally","after");return b.Finish();
    }

    private static byte[] N105_CreateSuppliesBody()
    {
        var b=new IL();b.Static(0x0400030F).Store(0);b.Load(0).Branch(0x39,"invalid");b.Arg(0).Integer(0).Branch(0x3F,"invalid");
        b.Load(0).Field(0x0400033D).Virtual(0x0A00017F).Arg(0).Branch(0x3E,"invalid");
        b.Load(0).Field(0x0400033D).Arg(0).Virtual(0x0A000081).Store(1);b.Load(1).Branch(0x39,"invalid");
        b.Load(1).New(0x060004DB).Store(2);b.Load(2).Integer(0).Set(0x0400067A);b.Load(2).Op(0x2A);
        b.Label("invalid").Op(0x14).New(0x060004DB).Store(2);
        b.Load(2).Ref(0x72,0x70007876).ArgAddress(0).Call(0x0A00002A).Ref(0x72,0x7000787E).Call(0x0A00010B).Set(0x04000677);
        b.Load(2).Op(0x2A);return b.Finish();
    }

    private static byte[] N105_RandomEnemyRowBody()
    {
        var b=new IL();b.Arg(0).Field(0x04000209).Op(0x14).Call(0x0A000012).Branch(0x39,"map");
        b.Ref(0x72,0x70005E8D).Call(0x0A000010).Integer(-1).Op(0x2A);
        b.Label("map").Arg(0).Field(0x04000209).Field(0x040003B5).Store(0);b.Integer(0).Store(1);
        b.Label("scan").Load(1).Load(0).Field(0x0400037D).Virtual(0x0A000147).Branch(0x3C,"none");
        b.Load(0).Field(0x0400037D).Load(1).Virtual(0x0A000065).Store(2);
        b.Arg(1).Field(0x04000206).Load(2).Field(0x04000382).Op(0x91).Branch(0x3A,"draw");
        b.Load(1).Integer(1).Op(0x58).Store(1).Branch(0x38,"scan");
        b.Label("draw").Load(1).Load(0).Field(0x0400037D).Virtual(0x0A000147).Call(0x0A00012B).Store(3);
        b.Arg(1).Field(0x04000206).Store(4);b.Load(0).Field(0x0400037D).Load(3).Virtual(0x0A000065).Store(2);
        b.Load(4).Load(2).Field(0x04000382).Op(0x91).Branch(0x39,"draw");b.Load(3).Integer(-1).Branch(0x3B,"draw");
        b.Load(3).Op(0x2A);b.Label("none").Integer(-1).Op(0x2A);return b.Finish();
    }

    private static byte[] N105_GetMapGridsBody()
    {
        var b=new IL();b.New(0x0A0001BC).Store(0);b.Arg(0).Field(0x0400037D).Virtual(0x0A0001C3).Store(1);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(1).Call(0x0A0001C5).Store(2);b.Arg(1).Store(3);
        b.Label("column").Arg(0).Call(0x06000289).Store(4);b.Arg(2).Integer(1).Op(0x58).Store(5);
        b.Load(4).Load(5).Branch(0x3F,"limit");b.Load(5).Store(4);b.Label("limit").Load(3).Load(4).Branch(0x3C,"move");
        b.Load(0).Load(2).Field(0x04000381).Load(3).Virtual(0x0A000078).Virtual(0x0A0001B9);
        b.Load(3).Integer(1).Op(0x58).Store(3).Branch(0x38,"column");
        b.Label("move").Address(1).Call(0x0A0001C4).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(1).Call(0x0A0001BB).Op(0xDC);b.Label("after").Load(0).Op(0x2A);
        RegisterFinally(0x06000288,b,"try","finally","after");return b.Finish();
    }

    private static byte[] N105_PlantDamagePointBody()
    {
        var b=new IL();b.Arg(0).Call(0x06000367).Store(0);b.Arg(0).Field(0x040004B1).Store(1);
        foreach(int id in new[]{2,4,5,7,49})b.Load(1).Integer(id).Branch(0x3B,"id"+id);b.Branch(0x38,"write");
        b.Label("id2").Arg(3).Integer(28).Branch(0x40,"write");N105_ScaleDamage(b,0.2f);b.Branch(0x38,"write");
        b.Label("id4").Arg(2).Op(0x14).Call(0x0A000012).Branch(0x39,"write");N105_ScaleDamage(b,8);b.Branch(0x38,"write");
        b.Label("id5").Arg(2).Call(0x0A00000B).Branch(0x39,"write");b.Arg(3).Integer(20).Branch(0x40,"write");
        N105_ScaleDamage(b,1.2f);b.Arg(1).Float(15).Set(0x0400012F).Branch(0x38,"write");
        b.Label("id7").Arg(2).Call(0x0A00000B).Branch(0x39,"write");b.Arg(1).Float(20).Set(0x0400012F).Branch(0x38,"write");
        b.Label("id49").Arg(3).Integer(0).Branch(0x3F,"write");b.Arg(2).Call(0x0A00000B).Branch(0x39,"write");
        b.Arg(3).Integer(30).Branch(0x40,"type21");N105_ScaleDamage(b,2.8f);b.Branch(0x38,"write");
        b.Label("type21").Arg(3).Integer(21).Branch(0x40,"write");b.Arg(4).Branch(0x39,"variant0");
        b.Arg(4).Integer(1).Branch(0x40,"write");N105_ScaleDamage(b,1.15f);b.Branch(0x38,"write");
        b.Label("variant0");N105_ScaleDamage(b,0.3f);
        b.Label("write").Arg(1).Load(0).Set(0x04000123).Op(0x2A);return b.Finish();
    }
    private static void N105_ScaleDamage(IL b,float v) => b.Load(0).Float(v).Op(0x5A).Store(0);

    private static byte[] N105_SunflowerHealBody()
    {
        var b=new IL();b.Integer(-1).Store(0);b.Label("x").Load(0).Integer(1).Branch(0x3D,"return");b.Integer(-1).Store(1);
        b.Label("y").Load(1).Integer(1).Branch(0x3D,"nextX");
        b.Arg(0).Field(0x040004F3).Arg(0).Field(0x040004A0).Load(0).Op(0x58).Arg(0).Field(0x040004A1).Load(1).Op(0x58).Virtual(0x060002C4).Store(2);
        b.Load(2).Branch(0x39,"nextY");b.Arg(0).Call(0x06000367).Float(1.2f).Op(0x5A).Store(3);
        foreach(int f in new[]{0x04000388,0x04000389,0x0400038A,0x0400038B})
        {b.Load(2).Field(f).Call(0x0A00000B).Branch(0x39,"next"+f);b.Load(2).Field(f).Load(3).Arg(0).Virtual(0x06000378);b.Label("next"+f);}
        b.Label("nextY").Load(1).Integer(1).Op(0x58).Store(1).Branch(0x38,"y");
        b.Label("nextX").Load(0).Integer(1).Op(0x58).Store(0).Branch(0x38,"x");b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N105_SunFallBody()
    {
        var b=new IL();b.Float(0).Arg(0).Field(0x04000341).Branch(0x3C,"return");
        b.Arg(0).Field(0x0400033F).Store(0);b.Load(0).Float(0).Branch(0x3D,"advance").Branch(0x38,"return");
        b.Label("advance").Load(0).Call(0x0A00001A).Op(0x59).Store(0);b.Arg(0).Load(0).Set(0x0400033F);
        b.Load(0).Float(0).Branch(0x42,"ui"); // bgt.un: positive OR unordered skips creation.
        b.Arg(0).Call(0x06000268);b.Arg(0).Field(0x04000344).Field(0x040003D7).Integer(0).Integer(0).Call(0x0A0000FC).Virtual(0x06000201).Virtual(0x060003CB);
        b.Arg(0).Arg(0).Field(0x04000342).Integer(1).Op(0x58).Set(0x04000342);
        b.Label("ui").Arg(0).Field(0x04000344).Field(0x040003DC).Call(0x0A00000B).Branch(0x39,"return");
        b.Arg(0).Field(0x04000344).Field(0x040003DC).Field(0x040000F6).Call(0x0A00000B).Branch(0x39,"return");
        b.Arg(0).Field(0x04000344).Field(0x040003DC).Field(0x040000F6).Store(1);
        b.Float(1).Arg(0).Field(0x0400033F).Arg(0).Field(0x04000340).Op(0x5B,0x59).Store(2);
        b.Load(2).Float(0).Branch(0x3F,"zero");b.Load(2).Float(1).Branch(0x3D,"one").Branch(0x38,"fill");
        b.Label("zero").Float(0).Store(2).Branch(0x38,"fill");b.Label("one").Float(1).Store(2);
        b.Label("fill").Load(1).Load(2).Virtual(0x0A0000F0);b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N105_PlantDamageTypeBody()
    {
        var b=new IL();b.Arg(0).Field(0x040004B1).Store(0);
        foreach(int id in new[]{0,2,3,4,5,7,49})b.Load(0).Integer(id).Branch(0x3B,"id"+id);b.Branch(0x38,"return");
        b.Label("id0");N105_DamageAttribute(b,0);N105_DamageFlag(b,0x04000127);N105_DamageFlag(b,0x04000129);b.Branch(0x38,"return");
        b.Label("id2");N105_DamageAttribute(b,3);N105_DamageFlag(b,0x0400012B);N105_DamageFlag(b,0x0400012C);
        b.Arg(2).Integer(28).Branch(0x3B,"return");N105_DamageFlag(b,0x04000129);b.Branch(0x38,"return");
        b.Label("id3");N105_DamageAttribute(b,0);b.Branch(0x38,"return");
        b.Label("id4");N105_DamageAttribute(b,2);N105_DamageFlag(b,0x0400012B);N105_DamageFlag(b,0x0400012C);N105_DamageFlag(b,0x04000129);b.Branch(0x38,"return");
        b.Label("id5").Arg(2).Integer(19).Branch(0x40,"sub20");N105_DamageAttribute(b,1);b.Branch(0x38,"return");
        b.Label("sub20").Arg(2).Integer(20).Branch(0x40,"return");b.Branch(0x38,"id7");
        b.Label("id7");N105_DamageAttribute(b,1);N105_DamageFlag(b,0x04000129);b.Branch(0x38,"return");
        b.Label("id49").Arg(2).Integer(0).Branch(0x3F,"skill");
        b.Arg(2).Integer(21).Branch(0x40,"sub23");N105_DamageAttribute(b,2);b.Arg(3).Integer(1).Branch(0x40,"return");N105_DamageFlag(b,0x04000129);b.Branch(0x38,"return");
        b.Label("sub23").Arg(2).Integer(23).Branch(0x40,"sub29");N105_DamageAttribute(b,0);b.Branch(0x38,"return");
        b.Label("sub29").Arg(2).Integer(29).Branch(0x3B,"type2");b.Arg(2).Integer(30).Branch(0x40,"return");
        b.Label("type2");N105_DamageAttribute(b,2);N105_DamageFlag(b,0x0400012B);N105_DamageFlag(b,0x04000129);b.Branch(0x38,"return");
        b.Label("skill").Arg(0).Field(0x040004D1).Field(0x0400068E).Integer(1).Branch(0x40,"skill2");
        b.Arg(3).Integer(1).Branch(0x3B,"skillOneVariant");N105_DamageAttribute(b,0);b.Branch(0x38,"skill2");
        b.Label("skillOneVariant");N105_DamageAttribute(b,2);N105_DamageFlag(b,0x04000129);N105_DamageFlag(b,0x0400012B);
        b.Label("skill2").Arg(0).Field(0x040004D1).Field(0x0400068E).Integer(2).Branch(0x40,"skill3");N105_DamageAttribute(b,0);N105_DamageFlag(b,0x04000129);
        b.Label("skill3").Arg(0).Field(0x040004D1).Field(0x0400068E).Integer(3).Branch(0x40,"return");N105_DamageAttribute(b,0);
        b.Label("return").Op(0x2A);return b.Finish();
    }
    private static void N105_DamageAttribute(IL b,int value) => b.Arg(1).Integer(value).Set(0x04000125);
    private static void N105_DamageFlag(IL b,int field) => b.Arg(1).Integer(1).Set(field);

    private static byte[] N105_ProjectSpeedStartBody()
    {
        var b=new IL();b.Arg(0).Field(0x04000520).Store(0);
        foreach(int mode in new[]{0,1,2,4})b.Load(0).Integer(mode).Branch(0x3B,"mode"+mode);b.Op(0x2A);
        b.Label("mode0").Arg(0).Float(0).Float(-100).Float(0).New(0x0A00009C).Set(0x04000522);
        b.Arg(0).Float(0).Float(0).Float(0).New(0x0A00009C).Set(0x04000524).Op(0x2A);
        foreach(var m in new[]{(Mode:1,X:50f,Y0:245f,Y1:490f,A:-1280f),(Mode:2,X:70f,Y0:427f,Y1:712f,A:-2140f)})
        {
            b.Label("mode"+m.Mode).Arg(0).FieldAddress(0x04000522).Float(-m.X).Float(m.X).Call(0x0A000083).Set(0x0A00001D);
            b.Arg(0).FieldAddress(0x04000522).Float(m.Y0).Float(m.Y1).Call(0x0A000083).Set(0x0A00008B);
            b.Arg(0).FieldAddress(0x04000522).Float(0).Set(0x0A00009D);
            b.Arg(0).Float(0).Float(m.A).Float(0).New(0x0A00009C).Set(0x04000524).Op(0x2A);
        }
        b.Label("mode4").Arg(0).Field(0x04000516).Integer(4).Branch(0x3B,"target");b.Arg(0).Field(0x04000516).Integer(5).Branch(0x40,"return");
        b.Label("target").Arg(0).Field(0x04000521).Store(1);b.Arg(0).Virtual(0x0A000001).Virtual(0x0A000037).Store(2);
        foreach(var axis in new[]{(Field:0x0A00001D,Local:3),(Field:0x0A00008B,Local:4),(Field:0x0A00009D,Local:5)})
            b.Address(1).Field(axis.Field).Address(2).Field(axis.Field).Op(0x59).Float(0.35f).Op(0x5A).Store(axis.Local);
        b.Load(3).Load(4).Load(5).New(0x0A00009C).Store(1);
        b.Arg(0).Load(1).Set(0x04000522);b.Arg(0).Load(1).Set(0x04000523);b.Arg(0).Call(0x0A0000FC).Set(0x04000524);
        b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N105_BlockZombieBody()
    {
        var b=new IL();b.Arg(2).Branch(0x39,"remove");b.Arg(1).Op(0x14).Call(0x0A00000C).Branch(0x39,"remove");
        b.Arg(0).Field(0x040004DD).Arg(1).Virtual(ZombieListContains).Branch(0x3A,"animateTrue");
        b.Arg(0).Field(0x040004DD).Arg(1).Virtual(0x0A0000DB);
        b.Label("animateTrue").Arg(0).Field(0x040004E3).Static(0x04000501).Integer(1).Virtual(0x0A0001F7).Op(0x2A);
        b.Label("remove").Arg(1).Op(0x14).Call(0x0A00000C).Branch(0x39,"count");
        b.Arg(0).Field(0x040004DD).Arg(1).Virtual(0x0A000225).Op(0x26);
        b.Label("count").Arg(0).Field(0x040004DD).Virtual(0x0A0000DD).Integer(0).Branch(0x3D,"return");
        b.Arg(0).Field(0x040004E3).Call(0x0A00000B).Branch(0x39,"return");
        b.Arg(0).Field(0x040004E3).Static(0x04000501).Integer(0).Virtual(0x0A0001F7);
        b.Label("return").Op(0x2A);return b.Finish();
    }
}
