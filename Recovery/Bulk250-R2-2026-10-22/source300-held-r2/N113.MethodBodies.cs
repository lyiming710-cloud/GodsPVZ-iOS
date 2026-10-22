internal static partial class Program
{
    private static byte[] N113_DeviceStartAnimationBody()
    {
        var b=new IL();
        b.Arg(0).Field(0x0400046F).Integer(3).Branch(0x40,"return");
        b.Arg(0).Field(0x04000470).Branch(0x39,"return");
        b.Integer(6).Ref(0x8D,0x0100000B).Store(0);
        int[] strings=[0x70008CCD,0x7000BCA1,0x7000BCB9,0x7000BCD5,0x7000BCF9,0x7000BD1D];
        for(int i=0;i<strings.Length;i++)b.Load(0).Integer(i).Ref(0x72,strings[i]).Op(0xA2);
        // The switch table at 18034B2F4 is data, not additional instructions.
        b.Arg(0).Field(0x04000470).Field(0x0400045C).Store(1);
        foreach(int noHide in new[]{1,3,4})b.Load(1).Integer(noHide).Branch(0x3B,"return");
        b.Load(1).Integer(2).Branch(0x3B,"type2");
        b.Load(1).Integer(8).Branch(0x3B,"type8");
        foreach(int i in new[]{2,3,4,5})b.Arg(0).Field(0x04000488).Load(0).Integer(i).Op(0x9A).Call(0x06000145);
        b.Branch(0x38,"return");
        b.Label("type2");
        foreach(int i in new[]{0,1,4,5})b.Arg(0).Field(0x04000488).Load(0).Integer(i).Op(0x9A).Call(0x06000145);
        b.Branch(0x38,"return");
        b.Label("type8");
        foreach(int i in new[]{0,1,2,3})b.Arg(0).Field(0x04000488).Load(0).Integer(i).Op(0x9A).Call(0x06000145);
        b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N113_PropUpdateBody()
    {
        var b=new IL();
        // Input is evaluated after Camera.main, before its receiver null check.
        b.Call(0x0A000004).Call(0x0A000023).Virtual(0x0A00001C).Store(0);
        b.Arg(0).FieldAddress(0x04000586).Address(0).Field(0x0A00001D).Set(0x0A0001D4);
        b.Arg(0).FieldAddress(0x04000586).Address(0).Field(0x0A00008B).Set(0x0A0001D5);
        b.Arg(0).Field(0x04000578).Op(0x14).Call(0x0A000012).Branch(0x3A,"return");
        b.Arg(0).Field(0x04000578).Field(0x040003BA).Branch(0x39,"return");
        b.Arg(0).Field(0x04000579).Integer(0).Branch(0x3B,"cooldown");
        b.Arg(0).Field(0x04000579).Integer(2).Branch(0x40,"return");
        b.Arg(0).FieldAddress(0x04000586).Field(0x0A0001D4).Store(2);
        b.Arg(0).FieldAddress(0x04000586).Field(0x0A0001D5).Store(3);
        b.Arg(0).Field(0x0400057A).Integer(5).Branch(0x40,"drag");
        b.Load(2).Float(50).Op(0x58).Store(2);b.Load(3).Float(50).Op(0x58).Store(3);
        b.Label("drag").Arg(0).Field(0x04000589).Virtual(0x0A00002E).Load(2).Load(3).Float(0).New(0x0A00009C).Virtual(0x0A000038).Branch(0x38,"return");
        b.Label("cooldown").Arg(0).Field(0x0400057E).Store(1);
        b.Arg(0).Load(1).Call(0x0A00001A).Op(0x58).Set(0x0400057E);
        b.Arg(0).Field(0x04000582).Arg(0).Field(0x0400057D).Arg(0).Field(0x0400057E).Op(0x59).Arg(0).Field(0x0400057D).Op(0x5B).Virtual(0x0A0000F0);
        b.Arg(0).Field(0x04000582).Store(4);
        b.Arg(0).Field(0x04000583).Float(1).Load(4).Virtual(0x0A000299).Op(0x59).Virtual(0x0A0000F0);
        b.Arg(0).Field(0x0400057D).Arg(0).Field(0x0400057E).Op(0x59).Store(1);
        b.Arg(0).Field(0x04000584).Address(1).Ref(0x72,0x7000B8C9).Call(0x0A000125).Virtual(0x0A000062);
        b.Arg(0).Field(0x0400057E).Arg(0).Field(0x0400057D).Branch(0x3C,"ready").Branch(0x38,"return");
        b.Label("ready").Arg(0).Integer(1).Set(0x04000579);
        b.Arg(0).Call(0x06000406);
        b.Arg(0).Arg(0).Field(0x04000581).Virtual(0x0A00002E).Virtual(0x0A00020D).Set(0x04000587);
        b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N113_ClosePlantDetailBody()
    {
        var b=new IL();
        b.Arg(0).Field(0x040006C4).Store(0);
        b.Load(0).Field(0x040006FE).Load(0).Field(0x040006F5).Field(0x04000662).Branch(0x40,"restoreLevel");
        b.Load(0).Field(0x0400070C).Load(0).Field(0x040006F5).Field(0x04000661).Branch(0x40,"closeOrder");
        b.Load(0).Field(0x040006FF).Load(0).Field(0x040006F5).Field(0x04000663).Branch(0x40,"closeSkill");
        b.Arg(0).Field(0x040006CB).Call(0x0A00000B).Branch(0x39,"board");
        b.Arg(0).Field(0x040006CB).Integer(1).Virtual(0x06000643);
        b.Label("board").Arg(0).Field(0x040006CC).Call(0x0A00000B).Branch(0x39,"home");
        b.Arg(0).Field(0x040006CD).Integer(1).Branch(0x40,"home");
        b.Arg(0).Field(0x040006CC).Field(0x040001B7).Store(1);
        b.Load(1).Field(0x040008BE).Field(0x040008C0).Arg(0).Field(0x040006BF).Virtual(0x060002F7);
        b.Load(1).Field(0x040008BC).Load(1).Field(0x040008BE).Field(0x040008C0).Field(0x04000429).Virtual(0x06000683);
        b.Arg(0).Virtual(0x0A000009).Integer(0).Virtual(0x0A000007);
        b.Load(1).Virtual(0x0A000009).Integer(1).Virtual(0x0A000007).Branch(0x38,"return");
        b.Label("home").Arg(0).Field(0x040006C1).Integer(1).Virtual(0x0A000007);
        b.Arg(0).Field(0x040006C4).Virtual(0x0A000009).Integer(0).Virtual(0x0A000007);
        b.Arg(0).Field(0x040006CA).Virtual(0x0A000009).Integer(1).Virtual(0x0A000007);
        b.Arg(0).Field(0x040006C8).Virtual(0x0A0002AA).Integer(0).Branch(0x3D,"return");
        b.Arg(0).Call(0x06000547).Branch(0x38,"return");
        b.Label("restoreLevel").Load(0).Load(0).Field(0x040006F5).Field(0x04000662).Virtual(0x06000582).Branch(0x38,"return");
        b.Label("closeOrder").Load(0).Virtual(0x06000578).Branch(0x38,"return");
        b.Label("closeSkill").Load(0).Virtual(0x06000579);
        b.Label("return").Op(0x2A);return b.Finish();
    }

    private static void N113_SaveNameColor(IL b,bool current)
    {
        b.Arg(0).Field(0x040008B5).Float(current?0:1).Float(1).Float(current?0:1).Float(1).New(0x0A0000BE).Virtual(0x0A0001DC);
    }

    private static byte[] N113_SaveRowStartBody()
    {
        var b=new IL();
        b.Arg(0).Field(0x040008B4).Integer(-1).Branch(0x3B,"emptySlot");
        b.Arg(0).Field(0x040008B2).Branch(0x39,"return");
        b.Arg(0).Field(0x040008B2).Field(0x04000311).Field(0x0400064E).Arg(0).Field(0x040008B4).Virtual(0x0A000194).Store(0);
        b.Load(0).Branch(0x39,"missingSave");
        b.Arg(0).Field(0x040008B5).Load(0).Field(0x04000627).Integer(1).Virtual(0x0A00004B);
        b.Arg(0).Field(0x040008B2).Field(0x04000310).Load(0).Branch(0x3B,"currentSave");
        N113_SaveNameColor(b,false);
        b.Arg(0).Field(0x040008B6).Integer(1).Virtual(0x0A000018).Branch(0x38,"return");
        b.Label("currentSave");N113_SaveNameColor(b,true);
        b.Arg(0).Field(0x040008B6).Integer(0).Virtual(0x0A000018).Branch(0x38,"return");
        b.Label("missingSave").Arg(0).Field(0x040008B5).Ref(0x72,0x7001BE41).Integer(1).Virtual(0x0A00004B);
        b.Arg(0).Field(0x040008B6).Integer(0).Virtual(0x0A000018);
        N113_SaveNameColor(b,false);b.Branch(0x38,"return");
        b.Label("emptySlot").Arg(0).Field(0x040008B5).Ref(0x72,0x7001BE4B).Integer(1).Virtual(0x0A00004B);
        b.Arg(0).Field(0x040008B5).Float(42).Virtual(0x0A0002A3);
        b.Arg(0).Field(0x040008B6).Integer(1).Virtual(0x0A000018);
        N113_SaveNameColor(b,false);
        b.Arg(0).Field(0x040008B7).Integer(0).Virtual(0x0A000007);
        b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N113_ChooseCardBody()
    {
        var b=new IL();
        b.Arg(0).Field(0x04000448).Branch(0x3A,"return");
        b.Arg(0).Field(0x04000445).Virtual(0x060006BF).Store(0);
        b.Load(0).Arg(0).Field(0x04000445).Field(0x040008BC).Virtual(0x0600067F).Branch(0x3C,"return");
        b.Arg(0).Integer(1).Set(0x04000448);
        b.Arg(0).Field(0x04000436).Virtual(0x2B000054).Store(1);
        // Capture r, g and b from three distinct getter calls on the same Image.
        int[] channels=[0x0A0000EE,0x0A0000EF,0x0A00021B];
        for(int i=0;i<3;i++)
        {
            b.Load(1).Virtual(0x0A0001DD).Store(2);
            b.Address(2).Field(channels[i]).Store(3+i);
        }
        b.Load(1).Load(3).Load(4).Load(5).Float(1).New(0x0A0000BE).Virtual(0x0A0001DE);
        b.Arg(0).Field(0x04000436).Virtual(0x0A000001).Integer(0).Virtual(0x0A000002).Virtual(0x0A000009).Integer(1).Virtual(0x0A000007);
        b.Arg(0).Field(0x04000444).Virtual(0x0A000009).Integer(1).Virtual(0x0A000007);
        b.Arg(0).Load(0).Integer(1).Op(0x58).Set(0x04000449);
        b.Arg(0).Field(0x04000444).Arg(0).FieldAddress(0x04000449).Call(0x0A00002A).Virtual(0x0A000062);
        b.Arg(0).Field(0x04000445).Field(0x040008BE).Arg(0).Virtual(0x060006C2);
        b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N113_StarfruitBody()
    {
        var b=new IL();
        b.Arg(0).Field(0x040004FF).Ref(0x72,0x7000CD66).Virtual(0x06000107).Store(0);
        b.Arg(0).Field(0x040004D2).Branch(0x3A,"formation");
        b.Load(0).Branch(0x39,"health");
        b.Load(0).Field(0x04000110).Float(2).Branch(0x3F,"growBuff").Branch(0x38,"health");
        b.Label("growBuff").Load(0).Field(0x04000110).Store(1);
        b.Arg(0).Field(0x040004C9).Store(2);
        b.Call(0x0A00001A).Load(2).Op(0x5A).Float(0.04f).Op(0x5A).Load(1).Op(0x58).Store(3);
        b.Load(0).Load(3).Set(0x04000110);
        b.Load(3).Float(2).Branch(0x3D,"capBuff").Branch(0x38,"health");
        b.Label("capBuff").Load(0).Float(2).Set(0x04000110);
        b.Label("health").Arg(0).Field(0x040004B4).Integer(1).Branch(0x3F,"formation");
        b.Arg(0).Field(0x040004B8).Store(1);
        b.Load(1).Arg(0).Field(0x040004B7).Branch(0x3D,"heal").Branch(0x38,"formation");
        b.Label("heal").Arg(0).Field(0x040004C9).Store(2);
        b.Load(1).Float(0.02f).Op(0x5A).Load(2).Op(0x5A).Call(0x0A00001A).Op(0x5A).Arg(0).Field(0x040004B7).Op(0x58).Store(3);
        b.Arg(0).Load(3).Set(0x040004B7);
        b.Load(3).Arg(0).Field(0x040004B8).Branch(0x3D,"capHealth").Branch(0x38,"formation");
        b.Label("capHealth").Arg(0).Arg(0).Field(0x040004B8).Set(0x040004B7);
        b.Label("formation").Arg(0).Field(0x040004FC).Store(1);
        b.Load(1).Float(0).Branch(0x3D,"tickFormation").Branch(0x38,"animator");
        b.Label("tickFormation").Arg(0).Load(1).Call(0x0A00001A).Arg(0).Field(0x040004C9).Op(0x5A,0x59).Set(0x040004FC);
        b.Arg(0).Field(0x040004FD).Store(1);
        b.Load(1).Call(0x0A00001A).Arg(0).Field(0x040004C9).Op(0x5A,0x58).Store(3);
        b.Arg(0).Load(3).Set(0x040004FD);
        b.Arg(0).Field(0x040004FC).Float(0).Branch(0x3F,"capFormation").Branch(0x38,"summonTest");
        b.Label("capFormation").Arg(0).Float(0).Set(0x040004FC);
        b.Label("summonTest").Load(3).Float(0.3f).Branch(0x3D,"summon").Branch(0x38,"animator");
        b.Label("summon").Arg(0).Call(0x06000391);
        b.Label("animator").Arg(0).Field(0x040004E3).Op(0x14).Call(0x0A000012).Branch(0x3A,"return");
        b.Arg(0).Field(0x040004FF).Ref(0x72,0x7000CD66).Virtual(0x06000107).Store(0);
        b.Load(0).Branch(0x39,"return");
        b.Load(0).Field(0x04000110).Store(1);
        b.Arg(0).Field(0x040004E3).Store(4);
        b.Load(1).Float(2).Branch(0x3C,"state2");
        b.Load(1).Float(1).Branch(0x3C,"state1");
        b.Integer(0).Store(5).Branch(0x38,"setState");
        b.Label("state1").Integer(1).Store(5).Branch(0x38,"setState");
        b.Label("state2").Integer(2).Store(5);
        b.Label("setState").Load(4).Ref(0x72,0x7000E0A9).Load(5).Virtual(0x0A0001FD);
        b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N113_SnowbeastWallBody()
    {
        var b=new IL();
        b.Arg(0).Field(0x040005D9).Branch(0x3A,"return");
        b.Arg(0).Field(0x0400061A).Ref(0x72,0x7000FD17).Virtual(0x06000106).Ref(0x75,0x0200001D).Store(0);
        b.Load(0).Branch(0x39,"resetDirection");
        b.Load(0).Float(-1).Set(0x04000110);
        b.Label("resetDirection").Arg(0).Call(0x0A0000FC).Set(0x040005FC);
        b.Static(0x040002F2).Integer(12).Virtual(0x0A000084).Store(1);
        b.Call(0x0A000004).Virtual(0x0A000001).Virtual(0x0A000037).Store(2);
        b.Load(1).Load(2).Call(0x06000138).Call(0x0600013D).Op(0x26);
        b.Arg(0).Field(0x040005DA).Float(9999999).Op(0x59).Store(3);
        b.Arg(0).Load(3).Set(0x040005DA);
        b.Load(3).Float(0).Branch(0x3E,"rest").Branch(0x38,"hp");
        b.Label("rest").Arg(0).Float(0).Set(0x040005DA);
        b.Arg(0).Integer(1).Set(0x040005D9);
        b.Arg(0).Field(0x040005FE).Ref(0x72,0x7000FD73).Integer(1).Virtual(0x0A0001F3);
        b.Arg(0).Float(0.02f).Call(0x06000484);
        b.Label("hp").Arg(0).Field(0x04000616).Call(0x0A00000B).Branch(0x39,"return");
        b.Arg(0).Field(0x04000616).Virtual(0x06000627);
        b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N113_SnowbeastHitTestBody()
    {
        var b=new IL();
        b.Arg(0).Field(0x040005D9).Branch(0x3A,"return");
        b.Arg(0).Field(0x040005D4).Branch(0x3A,"return");
        b.Arg(0).Field(0x040005AA).Field(0x040003D5).Field(0x04000362).Virtual(0x0A0000D9).Store(0);
        b.Label("try").Branch(0x38,"move");
        b.Label("body").Address(0).Call(0x0A0000DA).Store(1);
        b.Load(1).Arg(0).Call(0x0A000012).Branch(0x3A,"move");
        b.Load(1).Virtual(0x06000431).Branch(0x39,"move");
        b.Load(1).Field(0x040005DB).Float(0).Branch(0x3D,"move");
        b.Arg(0).Field(0x040005B1).Load(1).Field(0x040005B1).Op(0x58);
        b.Load(1).Field(0x040005AD).Arg(0).Field(0x040005AD).Op(0x59).Call(0x0A000217).Float(2).Op(0x5A,0xFE,0x02).Branch(0x39,"move");
        b.Arg(0).Field(0x040005B2).Load(1).Field(0x040005B2).Op(0x58);
        b.Load(1).Field(0x040005AE).Arg(0).Field(0x040005AE).Op(0x59).Call(0x0A000217).Float(2).Op(0x5A,0xFE,0x02).Branch(0x39,"move");
        b.Arg(0).Load(1).Call(0x060004A9);
        b.Label("move").Address(0).Call(0x0A0000DC).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(0).Call(0x0A00015E).Op(0xDC);
        b.Label("after");b.Label("return").Op(0x2A);
        RegisterFinally(0x060004A8,b,"try","finally","after");return b.Finish();
    }
}
