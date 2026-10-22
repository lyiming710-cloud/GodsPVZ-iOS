internal static partial class Program
{
    private static void N119_EventInput(IL b)=>b.Arg(0).Field(0x04000930).Call(0x0A000023).Arg(0).Field(0x04000931);
    private static void N119_CharacterInfo(IL b)=>b.Arg(0).Field(0x04000930).Virtual(0x0A0002B8).Field(0x0A0002BB);
    private static byte[] N119_TextEvents()
    {
        var b=new IL();b.Arg(0).Field(0x04000930).Virtual(0x0A0002E3).Call(0x0A000023).Arg(0).Field(0x04000931).Call(0x0A0002EB).Branch(0x3A,"character");
        foreach(int f in new[]{0x04000933,0x04000935,0x04000934,0x04000936})b.Arg(0).Integer(-1).Set(f);b.Op(0x2A);
        b.Label("character");N119_EventInput(b);b.Integer(1).Call(0x0A0002EC).Store(0);b.Load(0).Integer(-1).Branch(0x3B,"word");b.Load(0).Arg(0).Field(0x04000934).Branch(0x3B,"word");b.Arg(0).Load(0).Set(0x04000934);
        N119_CharacterInfo(b);b.Load(0).Ref(0x8F,0x01000068).Field(N119_CharacterElement).Store(1);b.Load(1).Branch(0x39,"textChar");b.Load(1).Integer(1).Branch(0x40,"word");
        N119_CharacterInfo(b);b.Load(0).Ref(0x8F,0x01000068).Field(N119_CharacterChar).Store(2);b.Arg(0).Field(0x0400092C).Store(3).Branch(0x38,"invokeCharacter");
        b.Label("textChar");N119_CharacterInfo(b);b.Load(0).Ref(0x8F,0x01000068).Field(N119_CharacterChar).Store(2);b.Arg(0).Field(0x0400092B).Store(3);
        b.Label("invokeCharacter").Load(3).Branch(0x39,"word");b.Load(3).Load(2).Load(0).Virtual(0x0A0002ED);
        b.Label("word");N119_EventInput(b);b.Call(0x0A0002EE).Store(0);b.Load(0).Integer(-1).Branch(0x3B,"line");b.Load(0).Arg(0).Field(0x04000935).Branch(0x3B,"line");b.Arg(0).Load(0).Set(0x04000935);
        b.Arg(0).Field(0x04000930).Virtual(0x0A0002B8).Field(0x0A0002E5).Load(0).Ref(0xA3,0x01000078).Store(4);b.Address(4).Call(0x0A0002E6).Store(5);b.Address(4).Field(0x0A000373).Store(6);b.Address(4).Field(0x0A000374).Store(7);
        b.Arg(0).Field(0x0400092D).Store(8);b.Load(8).Branch(0x39,"line");b.Load(8).Load(5).Load(6).Load(7).Virtual(N119_WordInvoke);
        b.Label("line");N119_EventInput(b);b.Call(0x0A0002EF).Store(0);b.Load(0).Integer(-1).Branch(0x3B,"link");b.Load(0).Arg(0).Field(0x04000936).Branch(0x3B,"link");b.Arg(0).Load(0).Set(0x04000936);
        b.Arg(0).Field(0x04000930).Virtual(0x0A0002B8).Field(0x0A0002E7).Load(0).Ref(0xA3,0x01000079).Store(9);b.Address(9).Field(N119_LineCount).Store(7);b.Address(9).Field(N119_LineStart).Store(6);b.Load(7).Ref(0x8D,0x01000076).Store(10);b.Integer(0).Store(11).Branch(0x38,"lineTest");
        b.Label("lineChar").Load(10).Load(11);N119_CharacterInfo(b);b.Load(6).Load(11).Op(0x58).Ref(0x8F,0x01000068).Field(N119_CharacterChar).Op(0x9D);b.Load(11).Integer(1).Op(0x58).Store(11);
        b.Label("lineTest").Load(11).Load(7).Branch(0x3C,"lineDone");b.Load(11);N119_CharacterInfo(b);b.Op(0x8E,0x69).Branch(0x3F,"lineChar");b.Label("lineDone").Load(10).New(N119_StringChars).Store(5);
        b.Arg(0).Field(0x0400092E).Store(8);b.Load(8).Branch(0x39,"link");b.Load(8).Load(5).Load(6).Load(7).Virtual(N119_WordInvoke);
        b.Label("link");N119_EventInput(b);b.Call(0x0A0002B5).Store(0);b.Load(0).Integer(-1).Branch(0x3B,"return");b.Load(0).Arg(0).Field(0x04000933).Branch(0x3B,"return");b.Arg(0).Load(0).Set(0x04000933);
        b.Arg(0).Field(0x04000930).Virtual(0x0A0002B8).Field(0x0A0002B9).Load(0).Ref(0xA3,0x01000065).Store(12);b.Address(12).Call(0x0A0002B6).Store(5);b.Address(12).Call(0x0A0002E9).Store(13);b.Arg(0).Field(0x0400092F).Store(14);b.Load(14).Branch(0x39,"return");b.Load(14).Load(5).Load(13).Load(0).Virtual(0x0A0002EA);
        b.Label("return").Op(0x2A);return b.Finish();
    }
}
