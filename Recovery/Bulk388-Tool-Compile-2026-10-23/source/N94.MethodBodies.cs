internal static partial class Program
{
    private static byte[] N94_SnowbeastBody()
    {
        var b = new IL();
        // Native calls GetDamage even when the target zombie is null.
        b.Arg(0).Integer(-1).Op(0x14).Integer(0).Call(0x06000443).Store(2);
        b.Arg(1).Load(2).Op(0x14).Virtual(0x0600047F);
        b.Arg(1).Float(1f).Set(0x040005DB);
        b.Arg(0).Arg(0).Field(0x040005DA).Float(0.15f).Op(0x59, 0x6B, 0x25).Store(3).Set(0x040005DA);
        // comiss 0,restTime; jb: positive or unordered skips the rest transition.
        b.Float(0f).Load(3).Branch(0x44, "hp");
        // Capture animator before the state stores; fault on it after both stores.
        b.Arg(0).Field(0x040005FE);
        b.Arg(0).Float(0f).Set(0x040005DA);
        b.Arg(0).Integer(1).Set(0x040005D9);
        b.Ref(0x72, 0x7000FD73).Integer(1).Virtual(0x0A0001F3);
        b.Arg(0).Float(0.02f).Call(0x06000484);
        b.Label("hp").Arg(0).Field(0x04000616).Call(0x0A00000B).Branch(0x39, "audio");
        b.Arg(0).Field(0x04000616).Virtual(0x06000627);
        b.Label("audio").Static(0x040002F2).Integer(12).Virtual(0x0A000084).Store(11);
        b.Call(0x0A000004).Virtual(0x0A000001).Virtual(0x0A000037).Store(12);
        b.Load(11).Load(12).Call(0x06000138).Call(0x0600013D).Op(0x26, 0x2A);
        return b.Finish();
    }

    private static byte[] N94_Awake19Body()
    {
        var b = new IL();
        b.Ref(0x72, 0x70000F30).Arg(0).Field(0x040004D1).Field(0x0400069C)
            .Integer(0).Op(0x98).Integer(1).New(0x060000F6).Store(8);
        b.Load(8).Integer(2).Set(0x04000106);
        b.Load(8).Integer(1).Set(0x04000108);
        b.Load(8).Arg(0).Set(0x04000109);
        // Skill is captured first; childBuffs is then captured before creating
        // the child. Keeping the List<Buff> on the stack avoids a new local sig.
        b.Arg(0).Field(0x040004D1).Store(4);
        b.Load(8).Field(0x0400010D);
        b.Ref(0x72, 0x70003D3C).Load(4).Field(0x0400069C).Integer(1).Op(0x98)
            .Integer(1).New(0x060000F6).Store(9);
        b.Load(9).Integer(1).Set(0x04000108);
        b.Load(9).Arg(0).Set(0x04000109);
        b.Load(9).Virtual(0x0A0000BF);
        b.Arg(0).Field(0x040004FF).Load(8).Virtual(0x06000102).Op(0x2A);
        return b.Finish();
    }

    private static byte[] N94_Passive10Body()
    {
        var b = new IL();
        // Public Plant.AnimationSprites() is pinned to the original three-
        // instruction field getter. Do not read Plant's private field here.
        b.Arg(0).Virtual(0x060003B9);
        b.Arg(0).Integer(0).Set(0x040004C3);
        b.Ref(0x72, 0x7000DB4E).Call(0x06000145);
        b.Arg(0).Virtual(0x060003B9).Ref(0x72, 0x7000DB76).Call(0x0600013F);
        b.Ref(0x72, 0x700044E5).Arg(0).Field(0x040004D1).Field(0x0400069C)
            .Integer(0).Op(0x98).Integer(0).New(0x060000F6).Store(7);
        b.Load(7).Integer(1).Set(0x04000106);
        b.Arg(0).Field(0x040004D1).Store(3);
        b.Load(7).Field(0x0400010D);
        b.Ref(0x72, 0x7000450D).Load(3).Field(0x0400069C).Integer(1).Op(0x98)
            .Integer(1).New(0x060000F6).Virtual(0x0A0000BF);
        b.Arg(0).Field(0x040004FF).Load(7).Virtual(0x06000102).Op(0x2A);
        return b.Finish();
    }

    private static byte[] N94_DamageAudioBody()
    {
        var b = new IL();
        // This call occurs before the first Device field read in the original.
        b.Call(0x06000138).Store(6);
        b.Op(0x14).Store(13); // typed AudioClip local, not UnityEngine.Object V5.
        b.Float(1f).Store(7);
        b.Arg(0).Field(0x04000479).Float(0f).Branch(0x3D, "ice"); // ordered >.
        b.Arg(0).Field(0x0400046F).Store(8); // one captured ID for 4 / 6 / 11.
        b.Load(8).Integer(4).Branch(0x3B, "idFour");
        b.Load(8).Integer(6).Branch(0x3B, "idSixEleven");
        b.Load(8).Integer(11).Branch(0x40, "selected");
        b.Label("idSixEleven");
        // Capture the list before RNG, and allow RNG to run before a null-list
        // failure. Original range is int [2,4), followed by float [0.9,1.1].
        b.Static(0x040002EF).Integer(2).Integer(4).Call(0x0A00012B).Virtual(0x0A000084).Store(13);
        b.Float(0.9f).Float(1.1f).Call(0x0A000083).Store(7).Branch(0x38, "selected");
        b.Label("idFour").Static(0x040002F1).Integer(12).Virtual(0x0A000084).Store(13);
        b.Float(1.8f).Float(1.9f).Call(0x0A000083).Store(7).Branch(0x38, "selected");
        b.Label("ice").Ref(0x72, 0x7000CC9E).Call(0x0A000010);
        b.Static(0x040002F2).Integer(6).Virtual(0x0A000084).Store(13);
        b.Load(6).Float(0.8f).Op(0x5A, 0x6B).Store(6);
        b.Float(0.7f).Float(0.9f).Call(0x0A000083).Store(7);
        b.Label("selected").Load(13).Call(0x0A00000B).Branch(0x39, "return");
        b.Call(0x0A000004).Virtual(0x0A000001).Virtual(0x0A000037).Store(18);
        b.Load(13).Load(18).Load(6).Load(7).Call(0x0600013E).Op(0x26);
        b.Label("return").Op(0x2A);
        return b.Finish();
    }
}
