internal static partial class Program
{
    // Source only; no execution or acceptance is inherited from earlier batches.
    private static byte[] N108_Function148Body()
    {
        var b = new IL();
        b.Arg(3).Branch(0x39,"createDamage");
        // Native reads damagePoint before traversing Plant.skill.value_float.
        b.Arg(3).Arg(3).Field(0x04000123).Arg(0).Field(0x040004D1).Field(0x0400069C)
            .Integer(1).Op(0x98,0x5A).Set(0x04000123);
        b.Arg(3).Virtual(0x0600010C).Op(0x26);
        b.Arg(0).Call(0x0A0000FC).Set(0x040004DE);
        b.Arg(0).Field(0x040004F3).Float(0.15f).Float(8f).Virtual(0x060002CD).Op(0x2A);

        // Original 0x180381C0D clears EDX: GetDamage receives null, never arg2.
        b.Label("createDamage").Arg(0).Op(0x14).Integer(-1).Integer(0).Virtual(0x0600036D).Store(0);
        b.Load(0).Load(0).Field(0x04000123).Arg(0).Field(0x040004D1).Field(0x0400069C)
            .Integer(0).Op(0x98,0x5A).Set(0x04000123);
        b.Load(0).Virtual(0x0600010C).Branch(0x39,"return");
        // Capture clip and position before AudioVolume, which may change the camera or resource list.
        b.Static(0x040002F1).Integer(25).Virtual(0x0A000084);
        b.Call(0x0A000004).Virtual(0x0A000001).Virtual(0x0A000037);
        b.Call(0x06000138).Op(0x25,0x58).Call(0x0600013D).Op(0x26);
        b.Label("return").Op(0x2A);
        return b.Finish();
    }

    private static byte[] N108_SkewConstructorBody()
    {
        var b = new IL();
        b.Integer(5).Ref(0x8D,0x01000092).Store(0); // Keyframe[]
        float[] times = [0f,0.25f,0.5f,0.75f,1f];
        for (int i=0;i<times.Length;i++)
            b.Load(0).Integer(i).Float(times[i]).Float((i&1)==0?0f:2f)
                .New(0x0A000330).Ref(0xA4,0x01000092); // stelem Keyframe, by value
        b.Arg(0).Load(0).New(0x0A000331).Set(0x0400099B);
        b.Arg(0).Float(1f).Set(0x0400099C);
        b.Arg(0).Float(1f).Set(0x0400099D);
        b.Arg(0).Call(0x0A000014).Op(0x2A);
        return b.Finish();
    }

    private static byte[] N108_PreviewSkillUpBody()
    {
        var b = new IL();
        b.Arg(0).Field(0x04000725).Arg(0).Field(0x04000732)
            .Virtual(0x060004CB).Virtual(0x060004DD).Store(0);
        b.Static(0x0400015E).Field(0x04000166).Load(0);
        b.Arg(0).Field(0x04000725).Field(0x0400065E);
        b.Arg(0).Field(0x04000725).Field(0x04000663).Integer(1).Op(0x58).Virtual(0x0600024D);
        b.Arg(0).Load(0).Float(1f).Float(0.65f).Float(0f).Float(1f)
            .New(0x0A0000BE).Virtual(0x06000591);
        b.Arg(0).Field(0x04000724).Virtual(0x0A000475).Store(1);
        b.Label("try").Branch(0x38,"move");
        // Capture current.text_Level before re-reading plantSave.skillLevel.
        b.Label("body").Address(1).Call(0x0A000476).Field(0x040008E0).Ref(0x72,0x70018DDD);
        b.Arg(0).Field(0x04000725).Field(0x04000663).Integer(1).Op(0x58).Store(2);
        b.Address(2).Call(0x0A00002A).Call(0x0A000051).Integer(1).Virtual(0x0A00004B);
        b.Label("move").Address(1).Call(0x0A000477).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(1).Call(0x0A00026B).Op(0xDC);
        b.Label("after").Op(0x2A);
        RegisterFinally(0x0600058F,b,"try","finally","after");
        return b.Finish();
    }

    private static byte[] N108_PreviewNewSkillBody()
    {
        var b = new IL();
        // Native uses unsigned (skillID-1) <= 2, with wrapping subtraction.
        b.Arg(1).Integer(1).Op(0x59).Integer(2).Branch(0x42,"cancel");
        b.Arg(0).Field(0x04000732).Store(0);b.Integer(0).Store(1);
        b.Label("validLoop").Load(1).Arg(0).Field(0x04000724).Virtual(N108_SkillButtonCount).Branch(0x3C,"validEnd");
        b.Load(1).Arg(1).Branch(0x3C,"validNext");
        b.Load(1).Arg(1).Integer(1).Op(0x59).Branch(0x3B,"preview");
        b.Arg(0).Field(0x04000724).Load(1).Virtual(0x0A000266).Store(2);
        b.Load(2).Branch(0x39,"lock");b.Load(2).Integer(0).Virtual(0x060006DA);
        // Re-read list/item after Select(false), even when the first item was null.
        b.Label("lock").Arg(0).Field(0x04000724).Load(1).Virtual(0x0A000266).Integer(1).Virtual(0x060006DC);
        b.Branch(0x38,"validNext");
        b.Label("preview").Arg(0).Arg(0).Field(0x04000725).Arg(1).Virtual(0x060004CB);
        b.Float(1f).Float(1f).Float(0.2f).Float(1f).New(0x0A0000BE).Virtual(0x06000591);
        b.Label("validNext").Load(1).Integer(1).Op(0x58).Store(1).Branch(0x38,"validLoop");
        b.Label("validEnd").Arg(0).Load(0).Set(0x04000732).Branch(0x38,"commit");

        b.Label("cancel").Integer(0).Store(1);
        b.Label("unlockLoop").Load(1).Arg(0).Field(0x04000724).Virtual(N108_SkillButtonCount).Branch(0x3C,"reselect");
        b.Load(1).Arg(0).Field(0x04000733).Branch(0x3C,"unlockNext");
        b.Load(1).Arg(0).Field(0x04000733).Integer(1).Op(0x59).Branch(0x3B,"unlockNext");
        b.Arg(0).Field(0x04000724).Load(1).Virtual(0x0A000266).Integer(0).Virtual(0x060006DC);
        b.Label("unlockNext").Load(1).Integer(1).Op(0x58).Store(1).Branch(0x38,"unlockLoop");
        b.Label("reselect").Arg(0).Field(0x04000732).Integer(1).Op(0x59).Store(0);b.Integer(0).Store(1);
        b.Label("selectLoop").Load(1).Arg(0).Field(0x04000724).Virtual(N108_SkillButtonCount).Branch(0x3C,"commit");
        b.Arg(0).Field(0x04000724).Load(1).Virtual(0x0A000266).Store(2);
        b.Load(2).Branch(0x39,"selectNext");b.Load(2).Load(1).Load(0).Op(0xFE,0x01).Virtual(0x060006DA);
        b.Label("selectNext").Load(1).Integer(1).Op(0x58).Store(1).Branch(0x38,"selectLoop");
        b.Label("commit").Arg(0).Arg(1).Set(0x04000733).Op(0x2A);
        return b.Finish();
    }
}
