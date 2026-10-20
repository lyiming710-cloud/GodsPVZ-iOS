internal static partial class Program
{
    private static byte[] N97_CopyEnemyBody()
    {
        var b = new IL();
        b.Arg(1).Field(0x04000051).New(0x060001A1).Store(1);
        b.Arg(0).Field(0x0400001D).Call(0x2B000007).Store(4);
        b.Load(4).Virtual(0x0A000001).Arg(0).Field(0x04000016)
            .Virtual(0x0A00002E).Integer(0).Virtual(0x0A00002F);
        b.Load(4).Arg(0).Set(0x04000050);
        b.Load(4).Load(1).Virtual(0x0600004B);
        b.Arg(0).Field(0x0400001B).Load(4).Virtual(EnemyEditorAdd);
        b.Arg(0).Field(0x04000018).Load(4).Virtual(0x06000042);
        // This method appends the captured clone, even if the editor now points elsewhere.
        b.Arg(0).Field(0x0400000D).Field(0x0400019D).Load(1).Virtual(0x0A000134).Op(0x2A);
        return b.Finish();
    }

    private static byte[] N97_CreatePathBody()
    {
        var b = new IL();
        b.Arg(0).Field(0x04000055).Field(0x04000051).Store(2);
        b.Integer(0).Load(2).Field(0x040001F8).Float(0f).Op(0x14).New(0x060002A4).Store(13);
        b.Load(2).Field(0x040001FD).Load(13).Virtual(0x0A000151);
        b.Arg(0).Field(0x04000057).Call(0x2B000008).Store(6);
        b.Load(6).Arg(0).Field(0x04000055).Set(0x04000067);
        b.Load(6).Load(13).Set(0x04000068);
        b.Load(6).Virtual(0x06000060);
        // Child Transform is evaluated before re-reading the parent Enemy editor.
        b.Load(6).Virtual(0x0A000001).Arg(0).Field(0x04000055)
            .Virtual(0x0A000001).Integer(1).Virtual(0x0A00002F);
        b.Arg(0).Field(0x04000055).Field(0x04000054).Load(6).Virtual(PathEditorAdd);
        b.Arg(0).Arg(0).Field(0x04000055).Field(0x04000054)
            .Virtual(PathEditorCount).Integer(1).Op(0x59).Virtual(0x06000058).Op(0x2A);
        return b.Finish();
    }

    private static byte[] N97_CreateDeviceBody()
    {
        var b = new IL();
        b.Integer(0).New(0x060002FB).Store(1);
        b.Integer(0).Integer(0).Load(1).Static(0x0A000013).New(0x0600017D).Store(2);
        // One PreDevice constructor invocation, matching original PC.
        N97_DeviceEditorTail(b, 2, 4);
        return b.Finish();
    }

    private static byte[] N97_CopyDeviceBody()
    {
        var b = new IL();
        b.Arg(1).Field(0x04000035).Store(1);
        b.Load(1).Field(0x040001D3).Store(5);
        b.Load(5).Field(0x04000456).Load(5).New(0x060002FC).Store(12);
        // Captured old PreDevice, entryKey read after the DeviceData copy constructor.
        b.Integer(0).Integer(0).Load(12).Load(1).Field(0x040001D2).New(0x0600017D).Store(15);
        N97_DeviceEditorTail(b, 15, 6);
        return b.Finish();
    }

    private static void N97_DeviceEditorTail(IL b, int preDevice, int editor)
    {
        b.Arg(0).Field(0x0400001C).Call(0x2B000006).Store(editor);
        b.Load(editor).Virtual(0x0A000001).Arg(0).Field(0x04000015)
            .Virtual(0x0A00002E).Integer(0).Virtual(0x0A00002F);
        b.Load(editor).Arg(0).Set(0x04000034);
        b.Load(editor).Load(preDevice).Set(0x04000035);
        b.Load(editor).Virtual(0x06000039);
        b.Arg(0).Field(0x0400001A).Load(editor).Virtual(DeviceEditorAdd);
        b.Arg(0).Field(0x04000017).Load(editor).Virtual(0x06000030);
        // Unlike CopyEnemy, original PC uses the editor's current preDevice after callbacks.
        b.Arg(0).Field(0x0400000D).Field(0x0400019E)
            .Load(editor).Field(0x04000035).Virtual(PreDeviceAdd).Op(0x2A);
    }
}
