internal static partial class Program
{
    private static byte[] N98_InfoButtonBody()
    {
        var b = new IL();
        // Cases 1 and all out-of-range indices never dereference this.
        b.Arg(1).Integer(0).Branch(0x3B, "talent");
        b.Arg(1).Integer(1).Branch(0x3B, "true");
        b.Arg(1).Integer(2).Branch(0x3B, "skill");
        b.Integer(0).Op(0x2A);
        b.Label("talent").Arg(0).Field(0x040008C0).Field(0x0400042F)
            .Virtual(0x060004CF).Integer(0).Op(0xFE, 0x02, 0x2A);
        b.Label("skill").Arg(0).Field(0x040008C0).Field(0x0400042F)
            .Virtual(0x060004CC).Integer(0).Op(0xFE, 0x02, 0x2A);
        b.Label("true").Integer(1).Op(0x2A);
        return b.Finish();
    }

    private static byte[] N98_NewPlantSuppliesBody()
    {
        var b = new IL();
        // Factory executes before any access to the PlantSave argument.
        b.Integer(50).Call(0x06000217).Store(1);
        b.Load(1).Arg(0).Field(0x0400065D).Set(0x04000679);
        b.Load(1).Integer(1).Set(0x0400067A);
        b.Load(1).Arg(0).Field(0x0400065E).Set(0x0400067B);
        b.Load(1).Op(0x2A);
        return b.Finish();
    }

    private static byte[] N98_MoveDeviceBody()
    {
        var b = new IL();
        b.Integer(0).Store(12).Integer(0).Store(13);
        b.Arg(1).Integer(0).Branch(0x3B, "up");
        b.Arg(1).Integer(1).Branch(0x3B, "right");
        b.Arg(1).Integer(2).Branch(0x3B, "down");
        b.Arg(1).Integer(3).Branch(0x3B, "left").Branch(0x38, "apply");
        b.Label("up").Integer(-1).Store(13).Branch(0x38, "apply");
        b.Label("right").Integer(1).Store(12).Branch(0x38, "apply");
        b.Label("down").Integer(1).Store(13).Branch(0x38, "apply");
        b.Label("left").Integer(-1).Store(12);
        // Invalid direction still performs both +=0 writes and LoadData.
        b.Label("apply").Arg(0).Field(0x04000023).Field(0x04000035)
            .Op(0x25).Field(0x040001D0).Load(12).Op(0x58).Set(0x040001D0);
        b.Arg(0).Field(0x04000023).Field(0x04000035)
            .Op(0x25).Field(0x040001D1).Load(13).Op(0x58).Set(0x040001D1);
        b.Arg(0).Field(0x04000023).Virtual(0x06000039).Op(0x2A);
        return b.Finish();
    }

    private static byte[] N98_TryUseSkillBody()
    {
        var b = new IL();
        b.Arg(0).Field(0x040004D1).Field(0x04000695).Branch(0x3A, "manual");
        b.Arg(0).Call(0x060003B8).Integer(0).Op(0x2A);
        b.Label("manual").Arg(0).Field(0x040004F3).Field(0x040003D9)
            .Arg(0).Virtual(0x060001DF);
        b.Arg(0).Field(0x040004F3).Virtual(0x060002C5);
        b.Arg(0).Field(0x040004B1).Integer(4).Branch(0x3B, "range");
        b.Integer(0).Op(0x2A);
        b.Label("range").Arg(0).Field(0x040004AF).Integer(1).Integer(0).Virtual(0x060005BF);
        b.Integer(1).Op(0x2A);
        return b.Finish();
    }
}
