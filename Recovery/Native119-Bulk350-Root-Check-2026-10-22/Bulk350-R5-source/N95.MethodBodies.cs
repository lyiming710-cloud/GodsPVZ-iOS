internal static partial class Program
{
    private static byte[] N95_ChangeRangeBody()
    {
        var b = new IL();
        b.Arg(0).Field(0x04000878).Store(0); // old skillRange.
        b.Arg(0).Field(0x04000879); // native captures range image before flipping state.
        b.Arg(0).Load(0).Integer(0).Op(0xFE, 0x01).Set(0x04000878);
        b.Load(0).Branch(0x3A, "white");
        b.Float(1f).Float(0.6f).Float(0f).Float(1f).New(0x0A0000BE)
            .Virtual(0x0A0001DE).Branch(0x38, "plant");
        b.Label("white").Float(1f).Float(1f).Float(1f).Float(1f).New(0x0A0000BE).Virtual(0x0A0001DE);
        b.Label("plant").Arg(0).Field(0x0400086B).Op(0x14).Call(0x0A000012).Branch(0x3A, "return");
        b.Arg(0).Field(0x0400086B).Field(0x040004AF).Virtual(0x060005BE);
        b.Arg(0).Field(0x0400086B).Store(5); // reread after CollapseView callback.
        b.Load(0).Branch(0x39, "skill");
        b.Load(5).Field(0x040004D2).Branch(0x3A, "skill");
        b.Load(5).Field(0x040004AF).Integer(0).Integer(0).Virtual(0x060005BF).Op(0x2A);
        b.Label("skill").Load(5).Field(0x040004AF).Arg(0).Field(0x0400086C).Field(0x0400068E);
        b.Load(0).Integer(0).Op(0xFE, 0x01).Virtual(0x060005BF);
        b.Label("return").Op(0x2A);
        return b.Finish();
    }

    private static byte[] N95_NutHotBody()
    {
        var b = new IL();
        b.Arg(0).Integer(1).Set(0x040005DD);
        b.Ref(0x72, 0x70003D42).Float(3f).Integer(1).New(0x060000F6).Store(0);
        b.Load(0).Ref(0x72, 0x70013980).Set(0x04000105);
        b.Load(0).Integer(1).Set(0x04000106);
        // Capture each list before its respective child constructor.
        b.Load(0).Field(0x0400010D).Ref(0x72, 0x70003D3C).Float(2f).Integer(1)
            .New(0x060000F6).Virtual(0x0A0000BF);
        b.Load(0).Field(0x0400010D).New(0x060000FC).Store(9);
        b.Load(9).Float(0.02f).Set(0x04000112);
        b.Load(9).Integer(1).Set(0x04000114);
        b.Load(9).Virtual(0x0A0000BF);
        b.Arg(0).Field(0x0400061A).Load(0).Virtual(0x06000102).Op(0x2A);
        return b.Finish();
    }

    private static byte[] N95_LoadSuppliesBody()
    {
        var b = new IL();
        b.Arg(0).Arg(1).Set(0x04000590);
        // Capture each UI receiver before evaluating its argument/callback.
        b.Arg(0).Field(0x04000594).Arg(1).Field(0x04000678).Call(0x0600022C).Virtual(0x0A000041);
        b.Arg(0).Field(0x04000596).Arg(1).Ref(0x7C, 0x0400067A).Call(0x0A00002A).Virtual(0x0A000062);
        b.Arg(0).Field(0x04000595).Virtual(0x0A000009).Integer(0).Virtual(0x0A000007);
        b.Arg(0).Field(0x04000598).Arg(0).Ref(0x7C, 0x04000591).Call(0x0A00002A).Virtual(0x0A000062);
        b.Arg(0).Field(0x04000597).Virtual(0x0A000009).Integer(0).Virtual(0x0A000007);
        b.Arg(1).Field(0x04000678).Integer(50).Branch(0x3B, "portrait");
        b.Arg(1).Field(0x04000678).Integer(51).Branch(0x40, "regular");
        b.Arg(0).Field(0x04000594).Static(0x040002D1).Arg(1).Field(0x0400067B)
            .Virtual(0x0A000040).Virtual(0x0A000041);
        b.Label("regular").Arg(0).Field(0x04000599).Integer(0).Virtual(0x0A000007).Op(0x2A);
        b.Label("portrait").Arg(0).Field(0x04000599).Integer(1).Virtual(0x0A000007);
        b.Arg(0).Field(0x0400059A).Static(0x040002D7).Arg(1).Field(0x0400067B)
            .Virtual(0x0A000040).Virtual(0x0A000041).Op(0x2A);
        return b.Finish();
    }

    private static byte[] N95_SelectSkillBody()
    {
        var b = new IL();
        b.Arg(0).Field(0x040008DF).Arg(1).Branch(0x3B, "return");
        b.Arg(0).Field(0x040008DB).Store(2); // captured Button.
        b.Arg(1).Branch(0x3A, "select");
        b.Load(2).Integer(1).Virtual(0x0A000018);
        b.Arg(0).Field(0x040008DE).Virtual(0x0A000009).Integer(1).Virtual(0x0A000007);
        b.Arg(0).Arg(1).Set(0x040008DF).Op(0x2A);
        // Use existing public property, not private Selectable.m_Interactable.
        b.Label("select").Load(2).Virtual(0x0A00043B).Branch(0x39, "return");
        b.Load(2).Integer(0).Virtual(0x0A000018);
        b.Arg(0).Field(0x040008DE).Virtual(0x0A000009).Integer(0).Virtual(0x0A000007);
        b.Arg(0).Field(0x040008D9).Store(8);
        b.Load(8).Field(0x04000725).Branch(0x39, "store"); // plain PlantSave null test.
        b.Load(8).Field(0x04000731).Branch(0x3A, "preview");
        b.Load(8).Field(0x04000725).Arg(0).Field(0x040008DA).Integer(1).Op(0x58)
            .Virtual(0x060004CB).Store(9);
        b.Arg(0).Field(0x040008D9).Load(9).Float(1f).Float(1f).Float(0.2f).Float(1f)
            .New(0x0A0000BE).Virtual(0x06000591).Branch(0x38, "store");
        b.Label("preview").Load(8).Arg(0).Field(0x040008DA).Integer(1).Op(0x58).Set(0x04000732);
        b.Arg(0).Field(0x040008D9).Virtual(0x0600058F);
        b.Label("store").Arg(0).Arg(1).Set(0x040008DF);
        b.Label("return").Op(0x2A);
        return b.Finish();
    }
}
