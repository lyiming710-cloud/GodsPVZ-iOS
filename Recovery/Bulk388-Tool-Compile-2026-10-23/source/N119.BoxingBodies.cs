internal static partial class Program
{
    // The three damage factories below are native inlines, not calls to damaged
    // managed pseudo-code. Six original dispatch tables are recorded separately.
    private static void N119_BoxingDamage(IL b,string prefix)
    {
        b.New(0x06000112).Store(14);b.Load(14).Arg(0).Field(0x040005F1).Set(0x04000124);
        b.Arg(0).Field(0x040005B9).Store(15);
        foreach(int id in new[]{3,11,16})b.Load(15).Integer(id).Branch(0x3B,prefix+"attribute");
        b.Load(15).Integer(13).Branch(0x3B,prefix+"direction");b.Load(15).Integer(18).Branch(0x40,prefix+"range");
        b.Load(14).Integer(1).Set(0x04000128);b.Load(14).Integer(1).Set(0x04000129);b.Load(14).Integer(1).Set(0x0400012C).Branch(0x38,prefix+"range");
        b.Label(prefix+"direction").Arg(0).Virtual(0x0A000001).Virtual(0x0A000036).Store(18);
        b.Load(14).Address(18).Field(0x0A00001D).Float(0).Op(0xFE,0x04).Set(0x04000128).Branch(0x38,prefix+"range");
        b.Label(prefix+"attribute").Load(14).Integer(3).Set(0x04000125);
        foreach(int f in new[]{0x04000129,0x0400012A,0x0400012B})b.Load(14).Integer(1).Set(f);
        b.Label(prefix+"range").Op(0x14).Store(17);b.Arg(0).Field(0x040005B9).Store(15);
        b.Load(15).Integer(3).Branch(0x3B,prefix+"nullImplicit");b.Load(15).Integer(17).Branch(0x3B,prefix+"nullImplicit");
        b.Load(15).Integer(11).Branch(0x3B,prefix+"range215");b.Load(15).Integer(16).Branch(0x40,prefix+"rangeDone");
        b.Op(0x14).Call(0x0A00000B).Branch(0x3A,prefix+"rangeDone");
        b.Arg(0).Arg(0).Virtual(0x0A000001).Float(270).Call(ZombieCircleRange).Store(17).Branch(0x38,prefix+"rangeDone");
        b.Label(prefix+"range215").Arg(0).Arg(0).Virtual(0x0A000001).Float(215).Call(ZombieCircleRange).Store(17).Branch(0x38,prefix+"rangeDone");
        b.Label(prefix+"nullImplicit").Op(0x14).Call(0x0A00000B).Op(0x26);
        b.Label(prefix+"rangeDone").Load(14).Load(17).Set(0x0400012E);b.Arg(0).Virtual(0x06000441).Store(16);
        b.Arg(0).Field(0x040005B9).Store(15);b.Load(15).Integer(11).Branch(0x3B,prefix+"atk15");b.Load(15).Integer(13).Branch(0x3B,prefix+"atkMS");
        b.Load(15).Integer(16).Branch(0x3B,prefix+"atk9");b.Load(15).Integer(18).Branch(0x3B,prefix+"atk8").Branch(0x38,prefix+"atkDone");
        b.Label(prefix+"atk15").Load(16).Float(15).Op(0x5A,0x6B).Store(16).Branch(0x38,prefix+"atkDone");
        b.Label(prefix+"atkMS").Load(16).Arg(0).Virtual(0x0600044B).Op(0x5A,0x6B).Store(16);b.Load(16).Load(16).Op(0x58,0x6B).Store(16).Branch(0x38,prefix+"atkDone");
        b.Label(prefix+"atk9").Load(16).Float(9).Op(0x5A,0x6B).Store(16).Branch(0x38,prefix+"atkDone");b.Label(prefix+"atk8").Load(16).Float(8).Op(0x5A,0x6B).Store(16);
        b.Label(prefix+"atkDone").Load(14).Load(16).Set(0x04000123);b.Load(14).Arg(0).Set(0x04000135);b.Load(14).Arg(0).Field(0x040005AA).Set(0x04000137);
    }
    private static byte[] N119_BoxingStep3()
    {
        var b=new IL();b.Arg(0).Field(0x040005AA).Op(0x14).Call(0x0A000012).Branch(0x3A,"return");
        b.Arg(0).Field(0x040005AA).Store(0);b.Load(0).Field(0x040003D4).Store(1);b.Load(0).Field(0x040003D8).Store(2);b.Load(0).Field(0x040003D5).Store(3);
        b.Arg(0).Integer(-1).Op(0x14).Integer(1).Virtual(0x06000443).Store(4);
        b.New(N119_BoxPlantCtor).Store(5);b.New(N119_ZombieListCtor).Store(6);b.New(0x0A0000CB).Store(7);
        var kinds=new[]{
            (Name:"p",Manager:1,List:0x040002B1,Output:5,Enum:8,Item:9,Get:N119_PlantEnum,Current:N119_PlantCurrent,Move:N119_PlantMove,Dispose:N119_PlantDispose,Can:0x06000359,X:0x040004A0,Y:0x040004A1,Add:N119_BoxPlantAdd,Take:0x060003B3),
            (Name:"d",Manager:2,List:0x040001D6,Output:7,Enum:10,Item:11,Get:N119_DeviceEnum,Current:N119_DeviceCurrent,Move:N119_DeviceMove,Dispose:N119_DeviceDispose,Can:0x06000310,X:0x04000464,Y:0x04000465,Add:0x0A0000CE,Take:0x06000336),
            (Name:"z",Manager:3,List:0x04000362,Output:6,Enum:12,Item:13,Get:N119_ZombieEnum,Current:N119_ZombieCurrent,Move:N119_ZombieMove,Dispose:N119_ZombieDispose,Can:0x06000431,X:0x040005AB,Y:0x040005AC,Add:N119_BoxZombieAdd,Take:0x0600047F)};
        foreach(var k in kinds)
        {
            string n=k.Name;b.Load(k.Manager).Field(k.List).Virtual(k.Get).Store(k.Enum);b.Label(n+"Try").Branch(0x38,n+"Move");
            b.Label(n+"Body").Address(k.Enum).Call(k.Current).Store(k.Item);b.Load(k.Item).Op(0x14).Call(0x0A000012).Branch(0x3A,n+"Move");
            if(n=="z")b.Load(k.Item).Arg(0).Call(0x0A000012).Branch(0x3A,n+"Move");
            b.Load(k.Item);if(n=="d")b.Load(4);b.Virtual(k.Can).Branch(0x39,n+"Move");
            if(n=="d")b.Load(k.Item).Field(0x0400046F).Integer(13).Branch(0x3B,n+"Move");
            b.Load(k.Item).Field(k.Y).Arg(0).Field(0x04000606).Branch(0x40,n+"Move");
            b.Load(k.Item).Field(k.X).Arg(0).Field(0x04000605).Branch(0x3D,n+"Move");
            b.Load(k.Item).Field(k.X).Arg(0).Field(0x04000605).Integer(2).Op(0x59).Branch(0x3F,n+"Move");
            b.Load(k.Output).Load(k.Item).Virtual(k.Add);
            b.Label(n+"Move").Address(k.Enum).Call(k.Move).Branch(0x3A,n+"Body").Branch(0xDD,n+"Done");b.Label(n+"Finally").Address(k.Enum).Call(k.Dispose).Op(0xDC);b.Label(n+"Done");
        }
        // Preserve native hit ordering: all selected plants, then zombies, then devices.
        foreach(int index in new[]{0,2,1})
        {
            var k=kinds[index];string n=k.Name+"Hit";b.Load(k.Output).Virtual(k.Get).Store(k.Enum);b.Label(n+"Try").Branch(0x38,n+"Move");b.Label(n+"Body").Address(k.Enum).Call(k.Current).Store(k.Item);
            N119_BoxingDamage(b,n);b.Load(k.Item).Load(14).Op(0x14).Virtual(k.Take);
            b.Label(n+"Move").Address(k.Enum).Call(k.Move).Branch(0x3A,n+"Body").Branch(0xDD,n+"Done");b.Label(n+"Finally").Address(k.Enum).Call(k.Dispose).Op(0xDC);b.Label(n+"Done");
        }
        b.Label("return").Op(0x2A);N119_Finally(b,0x06000492,("pTry","pFinally","pDone"),("dTry","dFinally","dDone"),("zTry","zFinally","zDone"),("pHitTry","pHitFinally","pHitDone"),("zHitTry","zHitFinally","zHitDone"),("dHitTry","dHitFinally","dHitDone"));return b.Finish();
    }
}
