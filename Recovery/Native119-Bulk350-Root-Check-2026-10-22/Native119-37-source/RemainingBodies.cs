internal static partial class Program
{
    private static byte[] N119_DeviceReward()
    {
        var b=new IL();b.Arg(0).Field(0x0400045C).Integer(5).Branch(0x40,"zombie");
        b.Static(0x0400015E).Field(0x04000166).Field(0x04000310).Arg(0).Field(0x0400045D).Virtual(0x060004C1).Branch(0x3A,"return");
        b.Static(0x0400015E).Field(0x04000166).Arg(0).Field(0x0400045D).Virtual(0x0600023E).Store(0);b.Arg(1).Field(0x04000463).Load(0).Virtual(0x060002B1);
        b.Static(0x040002FB).Call(0x2B000016).Store(1);b.Load(1).Arg(1).Field(0x04000463).Set(0x040003FB);b.Load(1).Load(0).Virtual(0x060002DE);b.Arg(1).Field(0x04000463).Field(0x040003DC).Load(1).Virtual(0x060000E3);
        b.Label("zombie").Arg(0).Field(0x0400045C).Integer(2).Branch(0x40,"device");b.Arg(1).Field(0x04000463).Field(0x040003D5).Arg(0).Field(0x0400045D).Arg(1).Field(0x04000464).Arg(1).Field(0x04000465).Virtual(0x0600027C).Op(0x26);
        b.Label("device").Arg(0).Field(0x0400045C).Integer(8).Branch(0x40,"return");b.New(0x060002EF).Store(2);
        b.Load(2).Arg(0).Field(0x0400045E).Field(0x0400045F).Set(0x04000421);b.Load(2).Arg(0).Field(0x0400045D).Set(0x04000423);b.Load(2).Arg(0).Field(0x0400045E).Field(0x04000460).Set(0x04000422);
        b.Load(2).Arg(0).Field(0x0400045D).Call(0x0600018F).Set(0x04000426);b.Load(2).Arg(0).Field(0x0400045D).Call(0x0600018E).Set(0x04000425);b.Load(2).Arg(0).Field(0x0400045E).Field(0x04000461).Set(0x04000427);
        b.New(0x060002D2).Store(3);b.Load(3).Load(2).Field(0x04000423).Set(0x040003F5);b.Load(3).Load(2).Set(0x040003F6);
        b.Static(0x040002FB).Call(0x2B000016).Store(1);b.Load(1).Arg(1).Field(0x04000463).Set(0x040003FB);b.Load(1).Load(3).Virtual(0x060002DF);b.Arg(1).Field(0x04000463).Field(0x040003DC).Load(1).Virtual(0x060000E3);
        b.Label("return").Op(0x2A);return b.Finish();
    }
    private static byte[] N119_BoardKeys()
    {
        var b=new IL();b.Integer(9).Call(0x0A0001D8).Branch(0x39,"pause");b.Arg(0).Arg(0).Field(0x040003E3).Integer(0).Op(0xFE,0x01).Set(0x040003E3);
        foreach(int f in new[]{0x040003C1,0x040003C4,0x040003C5,0x040003C3})b.Arg(0).Field(f).Arg(0).Field(0x040003E3).Virtual(0x0A000007);
        b.Label("pause").Arg(0).Field(0x040003BB).Branch(0x3A,"qualification");b.Call(0x06000143).Store(0);b.Integer(-1).Store(1);b.Integer(-1).Store(2);b.Load(0).Branch(0x39,"mappingDone");
        b.Load(0).Ref(0x8C,0x0100000A).Virtual(0x0A000061).Store(3);b.Load(0).Store(4);b.Ref(0x72,N119_KeyPrefix).Load(3).Ref(0x72,N119_Equals).Address(4).Call(0x0A00002A).Call(0x0A0000E0).Call(0x0A000010);
        b.Load(0).Integer(49).Op(0x59).Integer(6).Branch(0x42,"letters");b.Load(0).Integer(49).Op(0x59).Store(1).Branch(0x38,"props");
        b.Label("letters");int[] keys=[101,113,119,114,116,117,121];int[] slots=[9,7,10,11,13,8,12];for(int i=0;i<keys.Length;i++){b.Load(0).Integer(keys[i]).Branch(0x40,"mapNext"+i);b.Integer(slots[i]).Store(1).Branch(0x38,"props");b.Label("mapNext"+i);}
        b.Label("props");int[] pk=[97,100,115];int[] pi=[0,2,1];for(int i=0;i<3;i++){b.Load(0).Integer(pk[i]).Branch(0x40,"propNext"+i);b.Integer(pi[i]).Store(2);b.Label("propNext"+i);}
        b.Label("mappingDone").Load(1).Integer(0).Branch(0x3F,"propSelect");b.Arg(0).Field(0x040003D9).Virtual(0x060001C8);b.Arg(0).Field(0x040003DC).Field(0x040000E9).Virtual(N119_KeyCardCount).Load(1).Branch(0x3E,"propSelect");b.Arg(0).Field(0x040003DC).Field(0x040000E9).Load(1).Virtual(N119_KeyCardItem).Store(5);b.Load(5).Branch(0x39,"propSelect");b.Load(5).Virtual(0x060002D9);
        b.Label("propSelect").Load(2).Integer(0).Branch(0x3F,"special");b.Arg(0).Field(0x040003D9).Virtual(0x060001C8);b.Arg(0).Field(0x040003DC).Field(0x040000F1).Virtual(N119_KeyPropCount).Load(2).Branch(0x3E,"special");b.Arg(0).Field(0x040003DC).Field(0x040000F1).Load(2).Virtual(N119_KeyPropItem).Store(6);b.Load(6).Branch(0x39,"special");b.Load(6).Virtual(0x06000405);
        b.Label("special").Load(0).Integer(102).Branch(0x40,"speed");b.Arg(0).Field(0x040003D0).Branch(0x39,"qualification");b.Arg(0).Field(0x040003CF).Field(0x0400086B).Op(0x14).Call(0x0A00000C).Branch(0x39,"qualification");b.Arg(0).Field(0x040003CF).Virtual(0x06000666).Branch(0x38,"qualification");
        b.Label("speed").Load(0).Integer(120).Branch(0x40,"slow");b.Arg(0).Arg(0).Field(0x040003CD).Virtual(0x060002CF).Branch(0x38,"qualification");
        b.Label("slow").Load(0).Integer(122).Branch(0x40,"qualification");b.Call(0x0A0000F9).Float(.5f).Branch(0x40,"slowOn");b.Arg(0).Field(0x040003BC).Branch(0x38,"setScale");b.Label("slowOn").Float(.5f);b.Label("setScale").Call(0x0A0000F2);
        b.Label("qualification").Static(0x0400015E).Field(0x04000166).Field(0x04000310).Field(0x04000626).Call(0x06000148).Branch(0x39,"space");
        b.Integer(106).Call(0x0A0001D8).Branch(0x39,"kill");b.Arg(0).Field(0x040003DA).Integer(1).Set(0x04000219);b.Arg(0).Virtual(0x060002D0).Store(7);b.Arg(0).Virtual(0x060002C0);b.Load(7).Branch(0x39,"kill");
        b.Arg(0).Float(2).Set(0x040003B8);b.Arg(0).Float(1).Set(0x040003BC);b.Float(1).Call(0x0A0000F2);b.Arg(0).Field(0x040003CA).Integer(0).Virtual(0x0A000007);b.Arg(0).Integer(1).Set(0x040003B7);
        b.Static(0x040002EB).Integer(10).Virtual(0x0A000084).Store(8);b.Arg(0).Field(0x040003E5).Store(9);b.Load(8).Load(9).Call(0x06000138).Call(0x0600013D).Op(0x26);
        b.Label("kill").Integer(107).Call(0x0A0001D8).Branch(0x39,"reset");b.Arg(0).Field(0x040003D5).Virtual(0x0600027B);
        b.Label("reset").Integer(108).Call(0x0A0001D8).Branch(0x39,"paths");b.Arg(0).Field(0x040003DC).Call(0x0A00000B).Branch(0x39,"paths");b.Arg(0).Field(0x040003DC).Field(0x040000E9).Branch(0x39,"resetProps");
        b.Arg(0).Field(0x040003DC).Field(0x040000E9).Virtual(N119_KeyCardEnum).Store(10);b.Label("tryCards").Branch(0x38,"cardMove");b.Label("card").Address(10).Call(N119_KeyCardCurrent).Store(5);b.Load(5).Float(0).Set(0x04000401);b.Load(5).Integer(0).Set(0x04000406);
        b.Label("cardMove").Address(10).Call(N119_KeyCardMove).Branch(0x3A,"card").Branch(0xDD,"cardsDone");b.Label("finallyCards").Address(10).Call(N119_KeyCardDispose).Op(0xDC);b.Label("cardsDone");
        b.Label("resetProps").Arg(0).Field(0x040003DC).Field(0x040000F1).Branch(0x39,"paths");b.Arg(0).Field(0x040003DC).Field(0x040000F1).Virtual(N119_KeyPropEnum).Store(11);b.Label("tryProps").Branch(0x38,"propMove");b.Label("prop").Address(11).Call(N119_KeyPropCurrent).Float(0).Set(0x0400057D);
        b.Label("propMove").Address(11).Call(N119_KeyPropMove).Branch(0x3A,"prop").Branch(0xDD,"propsDone");b.Label("finallyProps").Address(11).Call(N119_KeyPropDispose).Op(0xDC);b.Label("propsDone");
        b.Label("paths").Integer(109).Call(0x0A0001D8).Branch(0x39,"wave");b.Arg(0).Field(0x040003D5).Virtual(0x06000276);b.Label("wave").Integer(110).Call(0x0A0001D8).Branch(0x39,"space");b.Arg(0).Field(0x040003DA).Integer(2).Virtual(0x060001BA);
        b.Label("space").Integer(32).Call(0x0A0001D8).Branch(0x39,"escape");b.Arg(0).Virtual(0x060002CE);b.Label("escape").Integer(27).Call(0x0A0001D8).Branch(0x39,"release");b.Arg(0).Virtual(0x060002B9);
        b.Label("release").Integer(32).Call(N119_KeyUp).Branch(0x39,"releaseEscape");b.Arg(0).Integer(0).Set(0x040003D2);b.Label("releaseEscape").Integer(27).Call(N119_KeyUp).Branch(0x39,"return");b.Arg(0).Integer(0).Set(0x040003D3);
        b.Label("return").Op(0x2A);N119_Finally(b,0x060002AC,("tryCards","finallyCards","cardsDone"),("tryProps","finallyProps","propsDone"));return b.Finish();
    }
}
