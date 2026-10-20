internal static partial class Program
{
    private static byte[] N100_LootPreviewBody()
    {
        var b = new IL();
        b.Arg(0).Field(0x040001A6).Integer(50).Branch(0x3B, "special");
        b.Arg(0).Field(0x040001A6).Integer(51).Branch(0x3B, "special");
        b.Arg(0).Field(0x040001A6).Call(0x06000217).Store(2);
        b.Load(2).Arg(0).Field(0x040001A7).Set(0x0400067A);
        b.Load(2).Op(0x2A);
        b.Label("special").Arg(0).Field(0x040001A6).Call(0x06000217).Store(2);
        b.Load(2).Integer(1).Set(0x0400067A);
        b.Load(2).Arg(0).Field(0x040001AC).Set(0x0400067B);
        b.Load(2).Op(0x2A);
        return b.Finish();
    }

    private static byte[] N100_FlagMeterBody()
    {
        var b = new IL();
        b.Arg(0).Arg(1).Field(0x0400021E).Set(0x0400080D);
        b.Arg(1).Field(0x04000211).Store(7);
        b.Arg(0).Load(7).Set(0x04000810);
        // Native signed multiply-high 0x66666667, sar 2 and sign correction is truncating /10.
        b.Load(7).Integer(10).Op(0x5B).Store(7);
        b.Arg(0).Load(7).Set(0x0400080F);
        b.Load(7).Branch(0x39, "name");
        b.Arg(0).Load(7).Virtual(0x0600061A);
        b.Label("name").Arg(1).Field(0x0400021E).Field(0x040003B4).Field(0x04000194).Store(4);
        b.Arg(0).Field(0x0400080D).Store(3);
        b.Load(3).Field(0x040003B3).Store(7); // native reads level before challengeType.
        b.Load(3).Field(0x040003B2).Load(7).Call(0x06000174).Store(10);
        // UI receiver is read after GetBoardNumberString, before String.Concat.
        b.Arg(0).Field(0x04000816).Load(10).Load(4).Call(0x0A000051)
            .Integer(1).Virtual(0x0A00004B).Op(0x2A);
        return b.Finish();
    }

    private static byte[] N100_CreateDeviceBody()
    {
        var b = new IL();
        // Native has a direct instance call here, without an independent manager-this null check.
        b.Arg(0).Arg(1).Field(0x04000456).Call(0x06000189).Store(3);
        b.Load(3).Call(0x0A00000B).Branch(0x39, "null");
        b.Load(3).Call(0x2B000027).Store(4);
        b.Load(4).Arg(0).Field(0x040001D4).Set(0x04000463);
        b.Load(4).Arg(1).Virtual(0x0600032C);
        b.Load(4).Op(0x2A);
        b.Label("null").Op(0x14, 0x2A);
        return b.Finish();
    }

    private static byte[] N100_CopyDeviceDataBody()
    {
        var b = new IL();
        // Field initializers precede Object::.ctor, as in the original constructor.
        b.Arg(0).Integer(1).Set(0x04000455);
        b.Arg(0).Integer(-1).Set(0x04000457);
        b.Arg(0).Integer(-1).Set(0x04000458);
        b.Arg(0).Call(0x0A0000B6);
        b.Arg(0).Arg(2).Field(0x04000454).Set(0x04000454);
        b.Arg(0).Arg(2).Field(0x04000455).Set(0x04000455);
        b.Arg(0).Arg(1).Set(0x04000456);
        // This existing copy constructor recursively uses the valid ContentDeviceData copy ctor301.
        b.Arg(0).Arg(2).Field(0x04000459).New(0x060002FF).Set(0x04000459);
        b.Arg(0).Arg(2).Field(0x0400045A).Set(0x0400045A);
        b.Arg(0).Arg(2).Field(0x0400045B).Set(0x0400045B).Op(0x2A);
        return b.Finish();
    }
}
