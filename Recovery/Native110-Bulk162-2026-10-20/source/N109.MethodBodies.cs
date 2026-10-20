internal static partial class Program
{
    private static byte[] N109_DeviceEventUpdateBody()
    {
        var b = new IL();
        b.Arg(0).Field(0x0400013C).Store(0);
        b.Load(0).Float(0).Branch(0x43,"return"); // <= or NaN: no Time call, no subtype work.
        b.Load(0).Call(0x0A00001A).Op(0x59).Store(0);
        b.Arg(0).Load(0).Set(0x0400013C);
        b.Load(0).Float(0).Branch(0x42,"climb"); // > or NaN: skip end/removal work.
        b.Arg(0).Float(0).Set(0x0400013C);
        b.Arg(0).Ref(0x75,0x02000028).Store(1); // HoleTeleport (plain managed object).
        b.Load(1).Branch(0x39,"remove");b.Load(1).Call(0x06000118);
        b.Label("remove").Arg(0).Field(0x0400013D).Field(0x04000484).Arg(0).Virtual(N109_DeviceEventListAdd);
        // Derived private fields stay in the public derived helper; base emits no private ldfld.
        b.Label("climb").Arg(0).Ref(0x75,0x02000029).Store(2);
        b.Load(2).Branch(0x39,"return");b.Load(2).Call(0x0600011A);
        b.Label("return").Op(0x2A);
        return b.Finish();
    }

    private static byte[] N109_ClimbLadderUpdateBody()
    {
        var b = new IL();
        b.Arg(0).Field(0x04000141).Op(0x14).Call(0x0A00000C).Branch(0x39,"return");
        b.Arg(0).Field(0x0400013C).Store(1);
        b.Arg(0).Field(0x04000141).Store(0); // re-read after Unity inequality callback.
        b.Load(1).Float(1).Branch(0x3D,"upper");
        b.Load(0).Load(1).Float(110).Op(0x5A).Set(0x040005AF).Branch(0x38,"return"); // fZ
        b.Label("upper").Load(0).Float(2).Arg(0).Field(0x0400013C).Op(0x59).Float(110).Op(0x5A).Set(0x040005AF);
        b.Arg(0).Field(0x04000141).Store(0);
        b.Load(0).Field(0x040005AD).Store(2); // capture fX BEFORE Time.get_deltaTime.
        b.Load(0).Call(0x0A00001A).Float(-40).Op(0x5A).Load(2).Op(0x58).Set(0x040005AD);
        b.Label("return").Op(0x2A);
        return b.Finish();
    }

    private static byte[] N109_PreviewMouseDownBody()
    {
        var b = new IL();
        b.Arg(0).Arg(0).Field(0x04000232).Call(0x06000541).Set(0x04000232);
        b.Arg(0).Field(0x04000232).Call(0x0A00000B).Branch(0x39,"prepare");
        b.Arg(0).Field(0x04000232).Virtual(0x0A000009).Virtual(0x0A000154).Branch(0x3A,"false");
        b.Label("prepare").Arg(0).Call(0x060001CC);
        b.Arg(0).Field(0x04000231).Call(0x0A00000B).Branch(0x39,"false");
        b.Arg(0).Field(0x04000231).Field(0x04000894).Store(0);
        b.Load(0).Call(0x0A00000B).Branch(0x39,"false");
        b.Load(0).Virtual(0x0A000009).Virtual(0x0A000154).Branch(0x39,"false");
        b.Load(0).Field(0x04000793).Virtual(0x0A000009).Virtual(0x0A000154).Branch(0x3A,"false");
        b.Integer(0).Call(0x0A00001F).Branch(0x39,"false");
        b.Ref(0x72,0x700066CF).Call(0x0A000010);
        b.Arg(0).Integer(-1).Call(0x060001E0).Store(1);
        b.Load(1).Call(0x0A00000B).Branch(0x39,"false");
        // 0x18030E190 and indexed getter 0x1803A8270 load the SAME class/static-field slot.
        b.Call(0x06000677).Field(0x04000894).Load(1).Virtual(0x060005DB);
        b.Integer(1).Op(0x2A);
        b.Label("false").Integer(0).Op(0x2A);
        return b.Finish();
    }
}
