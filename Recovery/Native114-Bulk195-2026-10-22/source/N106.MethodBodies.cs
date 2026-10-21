internal static partial class Program
{
    // Same-type private state stays in SwfClip; the controller calls public ToNext/ToPrev.
    private static byte[] N106_SetSwfCurrentFrameBody()
    {
        var b=new IL();b.Arg(0).Arg(1).Set(0x04000A7C);b.Arg(0).Call(0x06000844);
        N106_EmitSwfFrameEvent(b,"return");b.Label("return").Op(0x2A);return b.Finish();
    }
    private static byte[] N106_GetSwfFrameCountBody()
    {
        var b=new IL();b.Arg(0).Field(0x04000A72).Branch(0x39,"zero");
        b.Arg(0).Field(0x04000A72).Field(0x04000A8E).Branch(0x39,"zero");
        b.Arg(0).Field(0x04000A72).Field(0x04000A8E).Virtual(SwfFramesCount).Op(0x2A);
        b.Label("zero").Integer(0).Op(0x2A);return b.Finish();
    }
    private static byte[] N106_PreviousSwfFrameBody()
    {
        var b=new IL();b.Arg(0).Field(0x04000A7C).Integer(0).Branch(0x3E,"false");
        b.Arg(0).Arg(0).Field(0x04000A7C).Integer(1).Op(0x59).Set(0x04000A7C);
        b.Arg(0).Call(0x06000844);N106_EmitSwfFrameEvent(b,"true");
        b.Label("true").Integer(1).Op(0x2A);b.Label("false").Integer(0).Op(0x2A);return b.Finish();
    }
    private static byte[] N106_NextSwfFrameBody()
    {
        var b=new IL();b.Arg(0).Field(0x04000A7C).Store(0);b.Arg(0).Call(0x06000834).Integer(1).Op(0x59).Store(1);
        b.Load(0).Load(1).Branch(0x3C,"false");
        b.Arg(0).Arg(0).Field(0x04000A7C).Integer(1).Op(0x58).Set(0x04000A7C);
        b.Arg(0).Call(0x06000844);N106_EmitSwfFrameEvent(b,"true");
        b.Label("true").Integer(1).Op(0x2A);b.Label("false").Integer(0).Op(0x2A);return b.Finish();
    }
    private static void N106_EmitSwfFrameEvent(IL b,string skip)
    {
        b.Arg(0).Field(0x04000A76).Branch(0x39,skip);
        b.Arg(0).Field(0x04000A76).Arg(0).Virtual(SwfFrameChangedInvoke);
    }
    private static byte[] N106_ChangeSwfCurrentFrameBody()
    {
        var b=new IL();b.Arg(0).Integer(1).Set(0x04000A71);
        b.Arg(0).Call(0x06000834).Integer(0).Branch(0x3E,"zero");
        b.Arg(0).Field(0x04000A7C).Store(0);b.Arg(0).Call(0x06000834).Integer(1).Op(0x59).Store(1);
        b.Load(0).Integer(0).Branch(0x3F,"zero");b.Load(0).Load(1).Branch(0x3E,"old");
        b.Arg(0).Load(1).Set(0x04000A7C).Op(0x2A);
        b.Label("old").Arg(0).Load(0).Set(0x04000A7C).Op(0x2A);
        b.Label("zero").Arg(0).Integer(0).Set(0x04000A7C).Op(0x2A);return b.Finish();
    }
    private static byte[] N106_ControllerNextSwfFrameBody()
    {
        var b=new IL();b.Arg(0).Field(0x04000A99).Store(0);b.Load(0).Branch(0x39,"forward");
        b.Load(0).Integer(1).Branch(0x3B,"backward");
        b.Ref(0x72,0x7002C8EB).Arg(0).Call(0x06000866).Ref(0x8C,0x02000122).Call(0x0A00026E).New(0x0A0003AE).Op(0x7A);
        b.Label("forward").Arg(0).Field(0x04000A8F).Call(0x0A00000B).Branch(0x39,"false");
        b.Arg(0).Field(0x04000A8F).Virtual(0x0600083C).Op(0x2A);
        b.Label("backward").Arg(0).Field(0x04000A8F).Call(0x0A00000B).Branch(0x39,"false");
        b.Arg(0).Field(0x04000A8F).Virtual(0x0600083B).Op(0x2A);
        b.Label("false").Integer(0).Op(0x2A);return b.Finish();
    }
}
