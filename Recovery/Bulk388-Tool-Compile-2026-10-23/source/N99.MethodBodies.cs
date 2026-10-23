internal static partial class Program
{
    private static byte[] N99_WindowTBody()
    {
        var b = new IL();
        b.Static(0x04000302).Call(0x2B000095).Store(2);
        b.Load(2).Arg(0).Set(0x0400090A);
        b.Load(2).Virtual(0x0A000001).Arg(1).Integer(0).Virtual(0x0A00002F);
        b.Load(2).Op(0x2A);
        return b.Finish();
    }

    private static byte[] N99_PopupBoardBody()
    {
        var b = new IL();
        b.Static(0x04000304).Call(0x2B00008B).Store(3);
        b.Load(3).Arg(0).Set(0x0400087C);
        b.Load(3).Arg(1).Set(0x04000884);
        // Board receiver is used only after both popup fields were written.
        b.Arg(2).Load(3).Virtual(0x060002CC);
        b.Load(3).Op(0x2A);
        return b.Finish();
    }

    private static byte[] N99_PopupTransformBody()
    {
        var b = new IL();
        b.Static(0x04000304).Call(0x2B00008B).Store(3);
        b.Load(3).Arg(0).Set(0x0400087C);
        b.Load(3).Arg(1).Set(0x04000884);
        // Null parent is passed through; original checks only the child Transform.
        b.Load(3).Virtual(0x0A000001).Arg(2).Integer(0).Virtual(0x0A00002F);
        b.Load(3).Op(0x2A);
        return b.Finish();
    }

    private static byte[] N99_WindowQBody()
    {
        var b = new IL();
        b.Static(0x04000301).Call(0x2B000094).Store(2);
        b.Load(2).Arg(0).Set(0x04000900);
        b.Load(2).Integer(0).Set(0x04000907);
        b.Load(2).Op(0x14).Set(0x04000908);
        b.Load(2).Virtual(0x0A000001).Arg(1).Integer(0).Virtual(0x0A00002F);
        b.Load(2).Op(0x2A);
        return b.Finish();
    }
}
