internal static partial class Program
{
    private static byte[] N111_PlantColorBody()
    {
        var b=new IL();
        for(int i=0;i<3;i++)b.Float(1).Store(i);
        b.Arg(0).Field(0x040004F3).Field(0x040003D9).Field(0x04000243).Arg(0).Call(0x0A000012).Branch(0x39,"element");
        for(int i=0;i<3;i++)b.Float(0.4f).Store(i);
        b.Label("element").Arg(0).Field(0x040004D6).Virtual(0x06000128).Store(3);
        foreach(var p in new[]{(Local:0,Field:0x0A0000EE),(Local:1,Field:0x0A0000EF),(Local:2,Field:0x0A00021B)})
            b.Load(p.Local).Address(3).Field(p.Field).Op(0x5A).Store(p.Local);
        b.Arg(0).Field(0x040004DF).Virtual(0x0A0000FD).Store(4);
        b.Label("try").Branch(0x38,"move");
        b.Label("body").Address(4).Call(0x0A0000FE).Store(5);
        // Two component lookups, retaining alpha from the first lookup before the second callback.
        b.Load(5).Virtual(0x2B000011).Virtual(0x0A0001E7).Store(3);
        b.Load(5).Virtual(0x2B000011).Load(0).Load(1).Load(2).Address(3).Field(0x0A00021C).New(0x0A0000BE).Virtual(0x0A000097);
        b.Label("move").Address(4).Call(0x0A0000FF).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(4).Call(0x0A000128).Op(0xDC);
        b.Label("after").Op(0x2A);RegisterFinally(0x06000352,b,"try","finally","after");return b.Finish();
    }

    private static byte[] N111_DeviceLoopAnimationBody()
    {
        var b=new IL();
        b.Arg(1).Virtual(0x2B000012).Call(0x0A00000B).Branch(0x39,"enumerate");
        // Capture the list receiver before get_gameObject, matching the original call order.
        b.Arg(0).Field(0x04000488).Arg(1).Virtual(0x0A000009).Virtual(0x0A000086);
        b.Label("enumerate").Arg(1).Virtual(0x0A000090).Store(0);
        b.Label("try").Branch(0x38,"move");
        b.Label("body").Arg(0).Load(0).Virtual(0x0A0000ED).Ref(0x74,0x01000005).Call(0x06000308);
        b.Label("move").Load(0).Virtual(0x0A0000EC).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Load(0).Ref(0x75,0x01000036).Store(1);
        b.Load(1).Branch(0x39,"endFinally");b.Load(1).Virtual(0x0A0000D0);
        b.Label("endFinally").Op(0xDC);b.Label("after").Op(0x2A);
        RegisterFinally(0x06000308,b,"try","finally","after");return b.Finish();
    }

    private static byte[] N111_ZombiePortalBody(bool summonFlag)
    {
        var b=new IL();
        b.Arg(0).Field(0x040005AA).Op(0x25).Field(0x040003B5).Store(0).Field(0x040003D8).Store(1);
        b.Load(1).Field(0x040001D5).Integer(13).Virtual(0x0A000127).Store(2);
        if(summonFlag)
        {
            // Both GetMapX invocations remain; the second call can observe callback mutation.
            b.Load(0).Virtual(0x06000289).Integer(1).Op(0x59).Store(3);
            b.Load(0).Integer(999).Load(2).Load(3).Load(0).Virtual(0x06000289).Integer(1).Op(0x59).Virtual(N111_MapPlaceDevice);
        }
        else
        {
            b.Arg(0).Field(0x040005C1).Integer(2).Op(0xFE,0x04).Integer(2).Op(0x58).Store(3);
            b.Load(0).Load(3).Load(2).Integer(4).Load(0).Virtual(0x06000289).Integer(2).Op(0x59).Virtual(N111_MapPlaceDevice);
        }
        b.Virtual(N111_GridEnumerator).Store(4);
        b.Label("try").Branch(0x38,"move");
        b.Label("body").Address(4).Call(N111_GridCurrent).Store(5);
        b.Integer(13).New(0x060002FB).Store(6);
        b.Load(6).Arg(0).Field(0x040005F1).Set(0x04000455);
        b.Load(1).Load(6).Load(5).Field(0x04000386).Load(5).Field(0x04000387).Virtual(0x0600018A).Store(7);
        b.Load(7).Op(0x14).Call(0x0A00000C).Branch(0x39,"postPlace");
        b.Load(7).Arg(0).Set(0x0400048A);
        b.Label("postPlace");
        // The flag variant calls this even when Unity equality reports a destroyed object.
        if(summonFlag)b.Load(7).Virtual(0x0600031B);
        b.Label("move").Address(4).Call(N111_GridMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(4).Call(0x0A0001BA).Op(0xDC);b.Label("after").Op(0x2A);
        RegisterFinally(summonFlag?0x06000494:0x0600048F,b,"try","finally","after");return b.Finish();
    }

    private static byte[] N111_ZombieTryShootingBody()
    {
        var b=new IL();
        b.Arg(0).Field(0x04000614).Integer(0).Op(0x9A).Arg(0).Field(0x040005E1).Virtual(0x0A000007);
        b.Arg(0).Field(0x04000614).Integer(0).Op(0x9A).Virtual(0x0A00002E).Arg(1).Virtual(0x0A000038);
        b.Arg(0).Field(0x040005FE).Op(0x14).Call(0x0A00000C).Branch(0x39,"trigger");
        b.Arg(0).Field(0x040005FE).Integer(0).Virtual(0x0A000231).Op(0x8E,0x69).Branch(0x39,"trigger");
        b.Arg(0).Field(0x040005FE).Integer(0).Virtual(0x0A0001E8).Store(0);
        b.Address(0).Call(N111_NormalizedTime).Store(1);
        b.Arg(0).Field(0x04000614).Integer(0).Op(0x9A).Virtual(0x0A00002E).Integer(1).Virtual(0x0A000002).Virtual(0x2B000054).Store(2);
        b.Load(2).Load(1).Float(1).Op(0x5D).Virtual(0x0A0000F0);
        b.Arg(0).Field(0x04000614).Integer(0).Op(0x9A).Virtual(0x2B000061).Store(3);
        b.Load(3).Op(0x14).Call(0x0A00000C).Branch(0x39,"trigger");
        b.Load(3).Integer(0).Integer(0).Load(1).Virtual(0x0A0001E9);
        b.Label("trigger").Arg(0).Field(0x040005E1).Branch(0x39,"return");
        b.Arg(0).Field(0x040005E0).Branch(0x39,"return");
        b.Arg(0).Field(0x040005E2).Branch(0x3A,"return");
        b.Arg(0).Field(0x040005FE).Ref(0x72,0x7000EBD9).Virtual(0x0A000087);
        b.Arg(0).Integer(0).Set(0x040005E0);b.Arg(0).Integer(1).Set(0x040005E2);
        b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N111_PotatomortarBody()
    {
        var b=new IL();
        b.Arg(0).Field(0x040004DF).Ref(0x72,0x70001CE5).Call(0x06000141).Store(0);
        b.Arg(0).Field(0x040004DB).Op(0x14).Call(0x0A00000C).Branch(0x39,"device");
        b.Arg(0).Field(0x040004DB).Float(1.05f).Virtual(0x06000450).Store(1).Branch(0x38,"saveTarget");
        b.Label("device").Arg(0).Field(0x040004DC).Op(0x14).Call(0x0A00000C).Branch(0x39,"velocity");
        b.Arg(0).Field(0x040004DC).Virtual(0x0A000001).Virtual(0x0A000037).Store(1);
        b.Label("saveTarget").Arg(0).Load(1).Set(0x040004DE);
        b.Label("velocity").Arg(0).Field(0x040004DE).Store(1);
        foreach(int f in new[]{0x0A00001D,0x0A00008B,0x0A00009D})b.Address(1).Field(f).Address(0).Field(f).Op(0x59).Float(1.05f).Op(0x5B);
        b.New(0x0A00009C).Store(2);
        b.Arg(0).Field(0x040004F3).Field(0x040003D6).Integer(15).Virtual(0x060001F6).Store(3);
        b.Load(3).Load(0).Load(2).Call(0x0A0000FC).Float(50).Float(1012.5f).Integer(1).Arg(0).Virtual(N111_ProjectileInitialPlant);
        b.Arg(0).Load(3).Integer(15).Integer(0).Call(0x0600036D).Store(4);
        b.Arg(0).Field(0x040004D5).Branch(0x39,"damage");
        b.Arg(0).Field(0x040004D1).Store(5);b.Load(5).Field(0x0400068E).Integer(1).Branch(0x40,"damage");
        b.Load(5).Arg(0).Load(4).Virtual(0x060004E1);b.Arg(0).Integer(0).Set(0x040004D5);
        b.Label("damage").Load(3).Load(4).Arg(0).Field(0x040004C8).Virtual(0x060003F7);
        b.Arg(0).Field(0x040004DF).Ref(0x72,0x70001CE5).Call(0x06000145);
        b.Arg(0).Field(0x040004FE).Store(6);
        b.Load(6).Integer(0).Load(6).Integer(0).Virtual(0x0A00006B).Integer(1).Op(0x59).Virtual(0x0A0001FA).Op(0x2A);
        return b.Finish();
    }
}
