internal static partial class Program
{
    private static byte[] N103_SubscribeStoppedBody()
    {
        var b = new IL();
        b.Arg(0).Call(0x060008BE);
        b.Arg(1).Call(0x0A00000B).Branch(0x39,"return");
        b.Arg(0).Arg(1).Set(0x04000ABF);
        // The existing pinned accessor owns the private event field and its CAS retry loop.
        b.Arg(1).Arg(0).MethodAddress(0x060008BF).New(0x0A0003CB).Virtual(0x0600085A);
        b.Label("return").Arg(0).Op(0x2A); return b.Finish();
    }

    private static byte[] N103_UnsubscribeStoppedBody()
    {
        var b = new IL();
        b.Arg(0).Field(0x04000ABF).Op(0x14).Call(0x0A00000C).Branch(0x39,"return");
        // Capture after the Unity inequality call, before delegate construction.
        b.Arg(0).Field(0x04000ABF).Store(0);
        b.Load(0).Arg(0).MethodAddress(0x060008BF).New(0x0A0003CB).Virtual(0x0600085B);
        b.Arg(0).Op(0x14).Set(0x04000ABF);
        b.Label("return").Op(0x2A); return b.Finish();
    }

    private static byte[] N103_UpdatePlantSkillBody()
    {
        var b = new IL();
        b.Arg(0).Field(0x040004D2).Branch(0x39,"charging");
        b.Arg(0).Field(0x040004D1).Arg(0).Virtual(0x060004DF);
        b.Arg(0).Field(0x040004D1).Field(0x04000696).Branch(0x3A,"charging");
        b.Arg(0).Field(0x040004D1).Field(0x04000691).Integer(2).Branch(0x40,"rate");
        b.Arg(0).Call(0x06000366).Store(0).Branch(0x38,"tick");
        b.Label("rate").Arg(0).Field(0x040004C9).Store(0);
        b.Label("tick").Call(0x0A00001A).Load(0).Op(0x5A).Store(1);
        b.Arg(0).Arg(0).Field(0x040004D3).Load(1).Op(0x59).Set(0x040004D3);
        b.Arg(0).Field(0x040004D3).Float(0).Branch(0x42,"charging");
        b.Arg(0).Field(0x040004D1).Arg(0).Op(0x14,0x14,0x14).Virtual(0x060004E4);
        b.Arg(0).Field(0x040004B1).Integer(49).Branch(0x40,"charging");
        b.Arg(0).Call(0x06000392);
        b.Arg(0).Field(0x040004D1).Field(0x0400069A).Integer(1).Branch(0x3C,"charging");
        b.Arg(0).Field(0x040004FF).Ref(0x72,0x7000CD66).Virtual(0x06000107).Store(2);
        b.Load(2).Branch(0x39,"charging"); b.Load(2).Float(0).Set(0x04000110);
        b.Label("charging").Arg(0).Field(0x040004D1).Field(0x04000690).Branch(0x3A,"ui");
        b.Arg(0).Field(0x040004C9).Store(0);
        b.Arg(0).Call(0x0A00001A).Load(0).Op(0x5A).Call(0x060003AF);
        b.Label("ui").Arg(0).Field(0x040004EB).Virtual(0x06000623).Op(0x2A);
        return b.Finish();
    }

    private static byte[] N103_SkillAwakeBody()
    {
        var b = new IL(); b.Ref(0x72,0x700161F5).Call(0x0A000010); b.Integer(0).Store(0);
        b.Arg(1).Call(0x0A00000B).Branch(0x39,"lookup");
        b.Arg(1).Field(0x040004B1).Integer(3).Op(0x5A).Arg(0).Field(0x0400068E).Op(0x58).Store(0);
        b.Arg(1).Integer(1).Set(0x040004D2);
        b.Arg(1).Arg(0).Field(0x04000694).Set(0x040004D3);
        b.Arg(1).Arg(1).Field(0x040004D4).Integer(1).Op(0x58).Set(0x040004D4);
        b.Arg(1).Field(0x040004E3).Static(0x0400069D).Integer(1).Virtual(0x0A0001F7);
        b.Label("lookup").Static(0x0400069F).Load(0).Address(1).Virtual(AwakeTryGet).Op(0x26);
        b.Load(1).Branch(0x39,"return"); b.Load(1).Arg(1).Arg(2).Arg(3).Arg(4).Virtual(0x060004EA);
        b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N103_SkillEndBody()
    {
        var b = new IL(); b.Ref(0x72,0x70016452).Call(0x0A000010);b.Integer(0).Store(0);
        b.Arg(1).Call(0x0A00000B).Branch(0x39,"lookup");
        b.Arg(1).Field(0x040004B1).Integer(3).Op(0x5A).Arg(0).Field(0x0400068E).Op(0x58).Store(0);
        b.Arg(1).Integer(0).Set(0x040004D2);b.Arg(1).Float(0).Set(0x040004D3);
        b.Arg(1).Field(0x040004E3).Static(0x0400069D).Integer(0).Virtual(0x0A0001F7);
        b.Arg(0).Arg(0).Field(0x0400069A).Integer(1).Op(0x59).Set(0x0400069A);
        b.Arg(1).Integer(-1).Call(0x060003B2);
        b.Label("lookup").Static(0x040006A2).Load(0).Address(1).Virtual(EndTryGet).Op(0x26);
        b.Load(1).Branch(0x39,"return");b.Load(1).Arg(1).Arg(2).Arg(3).Arg(4).Virtual(0x060004F6);
        b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N103_SkillPassiveBody()
    {
        var b = new IL();b.Ref(0x72,0x700165E4).Call(0x0A000010);b.Integer(0).Store(0);
        b.Arg(1).Call(0x0A00000B).Branch(0x39,"lookup");
        b.Arg(1).Field(0x040004B1).Integer(3).Op(0x5A).Arg(0).Field(0x0400068E).Op(0x58).Store(0);
        b.Arg(1).Field(0x040004E3).Static(0x0400069E).Arg(0).Field(0x0400068E).Virtual(0x0A000241);
        b.Label("lookup").Static(0x040006A3).Load(0).Address(1).Virtual(PassiveTryGet).Op(0x26);
        b.Load(1).Branch(0x39,"return");b.Load(1).Arg(1).Virtual(0x060004FA);
        b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N103_ProjectileRangeBody()
    {
        var b = new IL();b.Ref(0x72,0x70002CE8).Call(0x0A000010);
        b.Arg(1).Field(0x04000556).Float(.5f).Op(0x5A).Store(7);
        b.Arg(0).Field(0x040000E2).Store(0);
        b.Arg(1).Field(0x04000552).Store(1);b.Arg(1).Field(0x04000553).Store(2);
        b.Arg(1).Field(0x04000556).Store(3);
        b.Load(1).Load(7).Op(0x59).Store(1);
        b.Arg(1).Field(0x04000557).Float(.5f).Op(0x5A).Store(4);
        b.Arg(1).Field(0x04000552).Store(5);
        b.Load(2).Load(4).Op(0x59).Store(2);b.Arg(1).Field(0x04000557).Store(4);
        b.Arg(1).Field(0x04000553).Store(6);
        b.Load(0).Integer(1).Branch(0x3B,"rect");b.Load(0).Integer(2).Branch(0x3B,"circle");
        b.Load(0).Integer(3).Branch(0x3B,"either");b.Load(0).Integer(4).Branch(0x3B,"true");b.Integer(0).Op(0x2A);
        b.Label("either");N103_EmitProjectileRect(b);b.Branch(0x3A,"true").Branch(0x38,"circle");
        b.Label("rect");N103_EmitProjectileRect(b);b.Op(0x2A);
        b.Label("circle").Arg(0).Load(5).Load(6).Float(0).New(0x0A00009C).Call(0x060000D4).Op(0x2A);
        b.Label("true").Integer(1).Op(0x2A);return b.Finish();
    }
    private static void N103_EmitProjectileRect(IL b)
    {b.Arg(0).Load(1).Load(2).Load(3).Load(4).New(0x0A00009B).Call(0x060000D7);}

    private static byte[] N103_AccelerationBody()
    {
        // The PC body ignores argument 2; its discriminant subtracts the literal .01f.
        var b = new IL();
        N103_EmitNorm(b,0,0);N103_EmitNorm(b,1,2);
        b.ArgAddress(1).Field(0x0A00008B).ArgAddress(0).Field(0x0A00008B).Op(0x5A,0x6B).Store(1);
        b.Load(1).ArgAddress(1).Field(0x0A00001D).ArgAddress(0).Field(0x0A00001D).Op(0x5A,0x6B,0x58,0x6B).Store(1);
        b.Load(1).ArgAddress(1).Field(0x0A00009D).ArgAddress(0).Field(0x0A00009D).Op(0x5A,0x6B,0x58,0x6B).Store(1);
        b.Load(2).Float(.01f).Op(0x59,0x6B).Load(0).Op(0x5A,0x6B).Store(3);
        b.Load(1).Load(1).Op(0x5A,0x6B).Load(3).Op(0x59,0x6B).Store(3);
        b.Load(3).Float(0).Branch(0x3F,"zero");
        b.Load(3).Call(0x0A00044A).Load(1).Op(0x58,0x6B).Load(0).Op(0x5B,0x6B).Store(4);
        foreach(int f in new[]{0x0A00001D,0x0A00008B,0x0A00009D})
            b.Address(5).ArgAddress(0).Field(f).Load(4).Op(0x5A,0x6B).ArgAddress(1).Field(f).Op(0x59,0x6B).Set(f);
        b.Load(5).Op(0x2A);b.Label("zero").Call(0x0A0000FC).Op(0x2A);return b.Finish();
    }
    private static void N103_EmitNorm(IL b,int argument,int local)
    {
        b.ArgAddress(argument).Field(0x0A00001D).ArgAddress(argument).Field(0x0A00001D).Op(0x5A,0x6B).Store(local);
        foreach(int f in new[]{0x0A00008B,0x0A00009D})
            b.Load(local).ArgAddress(argument).Field(f).ArgAddress(argument).Field(f).Op(0x5A,0x6B,0x58,0x6B).Store(local);
    }

    private static byte[] N103_NextChallengeBody()
    {
        var b = new IL();
        b.Ref(0xD0,0x02000037).Call(0x0A00004C).Call(0x0A00007D).Ref(0x74,0x1B0000AE).Store(0);
        b.Load(0).Arg(0).Field(0x040000A7).Call(ChallengeArrayIndexOf).Integer(1).Op(0x58).Store(1);
        // Native uses next < Length-1; the final enum entry is skipped by this method.
        b.Load(1).Load(0).Op(0x8E,0x69).Integer(1).Op(0x59).Branch(0x3F,"chosen");b.Integer(0).Store(1);
        b.Label("chosen").Arg(0).Load(0).Load(1).Op(0x94).Set(0x040000A7);
        b.Arg(0).Arg(0).Field(0x040000A7).Arg(0).Field(0x040000A8).Call(0x0600020B).Call(0x06000095).Op(0x26);
        b.Arg(0).Call(0x06000088).Op(0x2A);return b.Finish();
    }

    private static byte[] N103_OrderSuppliesBody()
    {
        var b = new IL();
        b.Arg(0).Field(0x04000317).Virtual(OrderListCount).Arg(1).Branch(0x3E,"none");
        for(int order=1;order<=4;order++)b.Arg(2).Integer(order).Branch(0x3B,"order"+order);
        b.New(0x0A00011E).Op(0x2A);
        for(int order=1;order<=4;order++)
            b.Label("order"+order).Arg(0).Field(0x04000317).Arg(1).Virtual(0x0A0001A7).Field(0x0400032B+order).Op(0x2A);
        b.Label("none").Op(0x14,0x2A);return b.Finish();
    }

    private static byte[] N103_SkillTargetButtonBody()
    {
        var b = new IL();
        b.Arg(1).Arg(2).Integer(2).Op(0x5A,0xFE,0x02).Store(0);
        b.Arg(1).Integer(9).Branch(0x3C,"max");b.Arg(1).Arg(3).Integer(2).Op(0x5A).Branch(0x3D,"max");
        b.Arg(0).Field(0x040007B9).Ref(0x72,0x7001B2AA).Integer(1).Virtual(0x0A00004B);
        b.Arg(4).Store(1).Branch(0x38,"tips");
        b.Label("max").Arg(0).Field(0x040007B9).Ref(0x72,0x7001B2B4).Integer(1).Virtual(0x0A00004B);
        b.Integer(0).Store(0);b.Integer(0).Store(1);
        b.Label("tips").Arg(0).Field(0x040007BA).Virtual(0x0A000009).Load(0).Virtual(0x0A000007);
        b.Arg(0).Field(0x040007BB).Virtual(0x0A000009).Load(0).Virtual(0x0A000007);
        b.Integer(0).Store(2);b.Load(0).Branch(0x3A,"button");b.Load(1).Store(2);
        b.Label("button").Arg(0).Field(0x040007B8).Load(2).Virtual(0x0A000018);
        b.Arg(0).Field(0x040007B9).Store(3);b.Load(2).Branch(0x3A,"white");
        b.Float(.5f).Store(4).Branch(0x38,"color");b.Label("white").Float(1).Store(4);
        b.Label("color").Load(3).Load(4).Load(4).Load(4).Float(1).New(0x0A0000BE).Virtual(0x0A0001DC).Op(0x2A);
        return b.Finish();
    }

    private static byte[] N103_ArmorAudioBody()
    {
        var b = new IL();
        foreach(int v in new[]{1,3,4})b.Arg(0).Field(0x040005C8).Integer(v).Branch(0x3B,"play");
        b.Branch(0x38,"return");b.Label("play").Integer(2).Integer(4).Call(0x0A00012B).Store(0);
        b.Load(0).Integer(0).Branch(0x3F,"return");
        b.Load(0).Static(0x040002EF).Virtual(ArmorClipCount).Branch(0x3C,"return");
        b.Static(0x040002EF).Load(0).Virtual(0x0A000084).Store(1);
        b.Call(0x0A000004).Virtual(0x0A000001).Virtual(0x0A000037).Store(2);
        b.Load(1).Load(2).Call(0x06000138).Call(0x0600013D).Op(0x26);
        b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N103_EmptyChoiceCardBody()
    {
        var b = new IL();
        b.Arg(0).Field(0x0400043D).Virtual(0x0A000009).Integer(0).Virtual(0x0A000007);
        b.Arg(0).Integer(-1).Set(0x04000429);
        foreach(int f in new[]{0x04000440,0x04000443,0x04000442})
            b.Arg(0).Field(f).Virtual(0x0A000009).Integer(0).Virtual(0x0A000007);
        foreach(var a in new[]{(Field:0x0400043B,Name:"orders"),(Field:0x0400043C,Name:"stars")})
        {
            b.Arg(0).Field(a.Field).Store(0);b.Integer(0).Store(1);
            b.Label(a.Name).Load(1).Load(0).Op(0x8E,0x69).Branch(0x3C,a.Name+"End");
            b.Load(0).Load(1).Op(0x9A).Virtual(0x0A000009).Integer(0).Virtual(0x0A000007);
            b.Load(1).Integer(1).Op(0x58).Store(1).Branch(0x38,a.Name);b.Label(a.Name+"End");
        }
        b.Arg(0).Field(0x04000441).Virtual(0x0A000009).Integer(0).Virtual(0x0A000007).Op(0x2A);
        return b.Finish();
    }

    private static byte[] N103_HighlightZombieBody()
    {
        var b = new IL();
        b.Arg(0).Arg(0).Field(0x04000232).Call(0x06000541).Set(0x04000232);
        b.Arg(0).Field(0x04000232).Call(0x0A00000B).Branch(0x39,"prepare");
        b.Arg(0).Field(0x04000232).Virtual(0x0A000009).Virtual(0x0A000154).Branch(0x3A,"return");
        b.Label("prepare").Op(0x14).Store(0);b.Arg(0).Call(0x060001CC);
        b.Arg(0).Field(0x04000231).Call(0x0A00000B).Branch(0x39,"highlight");
        b.Arg(0).Field(0x04000231).Field(0x04000894).Store(1);
        b.Load(1).Call(0x0A00000B).Branch(0x39,"highlight");
        b.Load(1).Virtual(0x0A000009).Virtual(0x0A000154).Branch(0x39,"highlight");
        b.Load(1).Field(0x04000793).Virtual(0x0A000009).Virtual(0x0A000154).Branch(0x3A,"highlight");
        b.Arg(0).Integer(-1).Call(0x060001E0).Store(0);
        b.Label("highlight").Load(0).Op(0x14).Call(0x0A00000C).Branch(0x39,"return");
        b.Load(0).Integer(1).Set(0x04000611);b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N103_WalkZombieBody()
    {
        var b = new IL();
        b.Arg(0).Field(0x040005FA).Branch(0x39,"wait");
        b.Arg(0).Field(0x040005FA).Virtual(0x0A000435).Branch(0x39,"wait");
        foreach(int id in new[]{0,2,4,5,14,15})b.Arg(0).Field(0x040005B9).Integer(id).Branch(0x3B,"group");
        b.Arg(0).Call(0x06000467).Branch(0x39,"stance");
        b.Label("group").Arg(0).Field(0x040005FE).Store(0);
        b.Load(0).Ref(0x72,0x7000FCD7).Integer(0).Integer(2).Call(0x0A00012B).Virtual(0x0A0001FD);
        b.Label("stance").Arg(0).Field(0x040005FE).Static(0x0400061E).Integer(0).Virtual(0x0A0001F7);
        b.Arg(0).Arg(0).Call(0x0600044F).Call(0x06000479);b.Arg(0).Call(0x06000472);
        b.Arg(0).Integer(0).Set(0x040005D4);
        b.Arg(0).Field(0x040005B9).Integer(17).Branch(0x40,"return");
        b.Arg(0).Field(0x040005FE).Ref(0x72,0x7000FD4F).Integer(0).Virtual(0x0A0001F3);
        b.Arg(0).Integer(0).Set(0x040005E1).Branch(0x38,"return");
        b.Label("wait").Arg(0).Float(.02f).Set(0x040005F6);b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N103_CheckDeviceBody() => N103_CheckFirstBody(0x06000562,true);
    private static byte[] N103_CheckZombieBody() => N103_CheckFirstBody(0x060005AC,false);
    private static byte[] N103_CheckFirstBody(int token,bool device)
    {
        var b = new IL(); b.Integer(0).Store(2);
        b.Arg(0).Field(device?0x040006E1:0x0400075D).Virtual(device?DeviceWindowEnumerator:ZombieWindowEnumerator).Store(0);
        b.Label("try").Branch(0x38,"move");
        b.Label("body").Address(0).Call(device?DeviceWindowCurrent:ZombieWindowCurrent).Store(1);
        b.Load(1).Virtual(0x0A000009).Virtual(0x0A000154).Branch(0x39,"move");
        b.Arg(0).Load(1).Field(device?0x040006E6:0x04000768).Call(device?0x06000561:0x060005AD);
        b.Integer(1).Store(2).Branch(0xDD,"after");
        b.Label("move").Address(0).Call(device?DeviceWindowMove:ZombieWindowMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(0).Call(device?0x0A00025C:0x0A000279).Op(0xDC);
        b.Label("after").Load(2).Branch(0x3A,"return");
        b.Arg(0).Field(device?0x040006E4:0x04000760).Op(0x14).Virtual(device?0x0600055B:0x0600059F);
        b.Label("return").Op(0x2A);RegisterFinally(token,b,"try","finally","after");return b.Finish();
    }

    private static byte[] N103_ToggleMooksBody() => N103_ToggleCategoryBody(0x060005B1,0);
    private static byte[] N103_ToggleEliteBody() => N103_ToggleCategoryBody(0x060005B2,1);
    private static byte[] N103_ToggleBossBody() => N103_ToggleCategoryBody(0x060005B3,2);
    private static byte[] N103_ToggleCategoryBody(int token,int category)
    {
        var b = new IL();b.Arg(0).Arg(1).Set(0x04000764+category);
        b.Arg(0).Field(0x0400075D).Virtual(ZombieWindowEnumerator).Store(0);
        b.Label("try").Branch(0x38,"move");
        b.Label("body").Address(0).Call(ZombieWindowCurrent).Store(1);
        b.Load(1).Field(0x04000769).Store(2);b.Load(2).Branch(0x39,"move");
        if(category==0)
        {b.Load(2).Field(0x0400017D).Branch(0x3A,"move");b.Load(2).Field(0x0400017E).Branch(0x3A,"move");}
        else b.Load(2).Field(category==1?0x0400017D:0x0400017E).Branch(0x39,"move");
        b.Load(1).Virtual(0x0A000009).Arg(1).Virtual(0x0A000007);
        b.Label("move").Address(0).Call(ZombieWindowMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(0).Call(0x0A000279).Op(0xDC);
        b.Label("after").Arg(0).Field(0x04000760).Field(0x04000740).Store(3);
        b.Load(3).Integer(0).Branch(0x3F,"check");b.Arg(1).Branch(0x3A,"return");
        b.Arg(0).Field(0x0400075D).Load(3).Virtual(0x0A00027A).Field(0x04000769).Store(2);
        b.Load(2).Branch(0x39,"return");
        if(category==0)
        {b.Load(2).Field(0x0400017D).Branch(0x3A,"return");b.Load(2).Field(0x0400017E).Branch(0x3A,"return");}
        else b.Load(2).Field(category==1?0x0400017D:0x0400017E).Branch(0x39,"return");
        b.Label("check").Arg(0).Call(0x060005AC);b.Label("return").Op(0x2A);
        RegisterFinally(token,b,"try","finally","after");return b.Finish();
    }
}
