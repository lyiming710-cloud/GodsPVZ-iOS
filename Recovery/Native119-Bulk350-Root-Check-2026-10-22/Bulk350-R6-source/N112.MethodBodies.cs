internal static partial class Program
{
    private static void N112_DisposeEnumerator(IL b,int enumerator,int disposable)
    {
        b.Load(enumerator).Ref(0x75,0x01000036).Store(disposable);
        b.Load(disposable).Branch(0x39,"endFinally");
        b.Load(disposable).Virtual(0x0A0000D0);
        b.Label("endFinally").Op(0xDC);
    }

    private static byte[] N112_VfxLoopBody()
    {
        var b=new IL();
        b.Arg(1).Virtual(0x2B000012).Call(0x0A00000B).Branch(0x39,"enumerate");
        // Keep the incoming list receiver on the stack before get_gameObject.
        b.Arg(2).Arg(1).Virtual(0x0A000009).Virtual(0x0A000086);
        b.Label("enumerate").Arg(1).Virtual(0x0A000090).Store(0);
        b.Label("try").Branch(0x38,"move");
        b.Label("body").Arg(0).Load(0).Virtual(0x0A0000ED).Ref(0x74,0x01000005).Arg(2).Call(0x060000C7);
        b.Label("move").Load(0).Virtual(0x0A0000EC).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally");N112_DisposeEnumerator(b,0,1);
        b.Label("after").Op(0x2A);RegisterFinally(0x060000C7,b,"try","finally","after");return b.Finish();
    }

    private static byte[] N112_BuffImagesBody()
    {
        var b=new IL();
        b.Arg(0).Field(0x040007B5).Virtual(0x0A000090).Store(0);
        b.Label("try").Branch(0x38,"move");
        b.Label("body").Load(0).Virtual(0x0A0000ED).Ref(0x74,0x01000005).Store(2);
        b.Load(2).Virtual(0x2B000054).Op(0x14).Call(0x0A00000C).Branch(0x39,"move");
        // The original performs the component lookup twice, with list capture between them.
        b.Arg(0).Field(0x040007B6).Load(2).Virtual(0x2B000054).Virtual(0x0A000286);
        b.Label("move").Load(0).Virtual(0x0A0000EC).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally");N112_DisposeEnumerator(b,0,1);
        b.Label("after").Op(0x2A);RegisterFinally(0x060005E5,b,"try","finally","after");return b.Finish();
    }

    private static byte[] N112_ZombieTransformBody()
    {
        var b=new IL();
        b.Arg(1).Virtual(0x0A00002E).Virtual(0x0A000090).Store(0);
        b.Label("try").Branch(0x38,"move");
        b.Label("body").Load(0).Virtual(0x0A0000ED).Ref(0x74,0x01000005).Store(2);
        b.Load(2).Virtual(0x0A00003C).Ref(0x72,0x70006CED).Call(0x0A00003A).Branch(0x39,"move");
        b.Load(2).Virtual(0x0A000037).Store(3);
        foreach(int f in new[]{0x0A00001D,0x0A00008B,0x0A00009D})
            b.ArgAddress(2).Field(f).Address(3).Field(f).Op(0x59);
        b.New(0x0A00009C).Store(4);
        b.Arg(1).Virtual(0x0A00002E).Store(5);
        b.Load(5).Virtual(0x0A000037).Store(6);
        b.Load(5);
        foreach(int f in new[]{0x0A00001D,0x0A00008B,0x0A00009D})
            b.Address(6).Field(f).Address(4).Field(f).Op(0x58);
        b.New(0x0A00009C).Virtual(0x0A000038);
        b.Label("move").Load(0).Virtual(0x0A0000EC).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally");N112_DisposeEnumerator(b,0,1);
        b.Label("after").Op(0x2A);RegisterFinally(0x0600047C,b,"try","finally","after");return b.Finish();
    }

    private static byte[] N112_ZombieRowBody()
    {
        var b=new IL();
        b.Call(0x0A0000FC).Store(3);
        b.Arg(0).Field(0x040005D2).Branch(0x3A,"return");
        b.Arg(0).Field(0x040005D6).Branch(0x3A,"return");
        b.Float(286).Arg(0).Field(0x040005AC).Op(0x6B).Float(134).Op(0x5A,0x59).Store(4);
        b.Arg(0).Virtual(0x0A000001).Virtual(0x0A000090).Store(0);
        b.Label("try").Branch(0x38,"move");
        b.Label("body").Load(0).Virtual(0x0A0000ED).Ref(0x74,0x01000005).Store(2);
        b.Load(2).Virtual(0x0A00003C).Ref(0x72,0x70006CED).Call(0x0A00003A).Branch(0x39,"move");
        b.Load(2).Virtual(0x0A000037).Store(3);
        b.Arg(0).Address(3).Field(0x0A00001D).Set(0x040005AD);
        b.Arg(0).Address(3).Field(0x0A00008B).Set(0x040005AE);
        b.Label("move").Load(0).Virtual(0x0A0000EC).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally");N112_DisposeEnumerator(b,0,1);
        b.Label("after").Load(4).Address(3).Field(0x0A00008B).Branch(0x3D,"increase");
        b.Address(3).Field(0x0A00008B).Load(4).Branch(0x3D,"decrease").Branch(0x38,"apply");
        b.Label("decrease").Address(3).Address(3).Field(0x0A00008B).Call(0x0A00001A).Float(72).Op(0x5A,0x59).Set(0x0A00008B);
        b.Load(4).Address(3).Field(0x0A00008B).Branch(0x3D,"clamp").Branch(0x38,"apply");
        b.Label("increase").Address(3).Address(3).Field(0x0A00008B).Call(0x0A00001A).Float(72).Op(0x5A,0x58).Set(0x0A00008B);
        b.Address(3).Field(0x0A00008B).Load(4).Branch(0x3D,"clamp").Branch(0x38,"apply");
        b.Label("clamp").Address(3).Load(4).Set(0x0A00008B);
        b.Label("apply").Arg(0).Arg(0).Virtual(0x0A000009).Load(3).Call(0x0600047C);
        b.Label("return").Op(0x2A);RegisterFinally(0x06000478,b,"try","finally","after");return b.Finish();
    }

    private static byte[] N112_FillTrianglesBody()
    {
        var b=new IL();
        b.Static(0x04000ADB).Virtual(IntListClear);
        b.Static(0x04000ADB).Virtual(0x0A0003E9).Arg(1).Branch(0x3C,"ready");
        b.Static(0x04000ADB).Arg(1).Integer(2).Op(0x5A).Virtual(0x0A0003EA);
        b.Label("ready").Integer(0).Store(0);
        b.Arg(1).Integer(0).Branch(0x3E,"return");
        b.Arg(0).Integer(2).Op(0x58).Store(1);
        b.Label("body");
        // Native winding is [base+2,base+1,base,base,base+3,base+2].
        foreach(int delta in new[]{0,-1,-2,-2,1,0})
        {
            b.Static(0x04000ADB).Load(1);
            if(delta!=0)b.Integer(delta).Op(0x58);
            b.Virtual(0x0A000291);
        }
        b.Load(1).Integer(4).Op(0x58).Store(1);
        b.Load(0).Integer(6).Op(0x58).Store(0);
        b.Load(0).Arg(1).Branch(0x3F,"body");
        b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N112_FillVerticesBody()
    {
        var b=new IL();
        b.Static(0x04000ADD).Virtual(N112_Vector3ListClear);
        b.Static(0x04000ADD).Virtual(0x0A0003EB).Arg(0).Op(0x8E,0x69).Branch(0x3C,"ready");
        b.Static(0x04000ADD).Arg(0).Op(0x8E,0x69).Integer(2).Op(0x5A).Virtual(0x0A0003EC);
        b.Label("ready").Arg(0).Op(0x8E,0x69).Store(1);b.Integer(0).Store(0).Branch(0x38,"test");
        b.Label("body").Arg(0).Load(0).Ref(0x8F,0x0100004B).Field(0x0A0001D4).Store(2);
        b.Arg(0).Load(0).Ref(0x8F,0x0100004B).Field(0x0A0001D5).Store(3);
        // Only x/y change; the pre-existing static Vertex.z must survive.
        b.Ref(0x7F,0x04000ADC).Load(2).Set(0x0A00001D);
        b.Ref(0x7F,0x04000ADC).Load(3).Set(0x0A00008B);
        b.Static(0x04000ADD).Static(0x04000ADC).Virtual(0x0A000093);
        b.Load(0).Integer(1).Op(0x58).Store(0);
        b.Label("test").Load(0).Load(1).Branch(0x3F,"body").Op(0x2A);return b.Finish();
    }

    private static void N112_UnsignedUvHalf(IL b,int packed,bool upper,int destination)
    {
        b.Load(packed);
        if(upper)b.Integer(16).Op(0x64);else b.Integer(65535).Op(0x5F);
        b.Op(0x6C,0x6B).Float(65535).Op(0x5B).Store(destination);
    }

    private static byte[] N112_FillUvsBody()
    {
        var b=new IL();
        b.Static(0x04000AE2).Virtual(N112_Vector2ListClear);
        b.Static(0x04000AE2).Virtual(0x0A0003ED).Arg(0).Op(0x8E,0x69).Integer(2).Op(0x5A).Branch(0x3C,"ready");
        b.Static(0x04000AE2).Arg(0).Op(0x8E,0x69).Integer(4).Op(0x5A).Virtual(0x0A0003EE);
        b.Label("ready").Arg(0).Op(0x8E,0x69).Store(1);b.Integer(0).Store(0).Branch(0x38,"test");
        b.Label("body").Arg(0).Load(0).Op(0x95).Store(2);
        N112_UnsignedUvHalf(b,2,true,4);N112_UnsignedUvHalf(b,2,false,5);
        b.Arg(0).Load(0).Integer(1).Op(0x58,0x95).Store(3);
        N112_UnsignedUvHalf(b,3,true,6);N112_UnsignedUvHalf(b,3,false,7);
        foreach(var v in new[]{(Field:0x04000ADE,X:4,Y:5),(Field:0x04000ADF,X:6,Y:5),(Field:0x04000AE0,X:6,Y:7),(Field:0x04000AE1,X:4,Y:7)})
        {
            b.Ref(0x7F,v.Field).Load(v.X).Set(0x0A0001D4);
            b.Ref(0x7F,v.Field).Load(v.Y).Set(0x0A0001D5);
        }
        foreach(int field in new[]{0x04000ADE,0x04000ADF,0x04000AE0,0x04000AE1})
            b.Static(0x04000AE2).Static(field).Virtual(N112_Vector2ListAdd);
        b.Load(0).Integer(2).Op(0x58).Store(0);
        b.Label("test").Load(0).Load(1).Branch(0x3F,"body").Op(0x2A);return b.Finish();
    }

    private static byte[] N112_FillColorsBody(bool multiply)
    {
        int list=multiply?0x04000AE6:0x04000AE4;
        int value=multiply?0x04000AE5:0x04000AE3;
        int clear=multiply?N112_ColorListClear:N112_Vector4ListClear;
        int add=multiply?N112_ColorListAdd:N112_Vector4ListAdd;
        int capacity=multiply?0x0A0003F3:0x0A0003F0;
        int setCapacity=multiply?0x0A0003F4:0x0A0003F1;
        int[] fields=multiply?[0x0A0000EE,0x0A0000EF,0x0A00021B,0x0A00021C]:[N112_Vector4X,N112_Vector4Y,N112_Vector4Z,N112_Vector4W];
        var b=new IL();
        b.Static(list).Virtual(clear);
        b.Static(list).Virtual(capacity).Arg(0).Op(0x8E,0x69).Integer(2).Op(0x5A).Branch(0x3C,"ready");
        b.Static(list).Arg(0).Op(0x8E,0x69).Integer(4).Op(0x5A).Virtual(setCapacity);
        b.Label("ready").Arg(0).Op(0x8E,0x69).Store(1);b.Integer(0).Store(0).Branch(0x38,"test");
        b.Label("body").Arg(0).Load(0).Op(0x94).Store(2);
        b.Arg(0).Load(0).Integer(1).Op(0x58,0x94).Store(3);
        for(int i=0;i<4;i++)
        {
            b.Ref(0x7F,value).Load(i<2?2:3);
            if((i&1)==0)b.Integer(16).Op(0x63);else b.Op(0x68);
            b.Op(0x6B).Float(0.001953125f).Op(0x5A).Set(fields[i]);
        }
        for(int i=0;i<4;i++)b.Static(list).Static(value).Virtual(add);
        b.Load(0).Integer(2).Op(0x58).Store(0);
        b.Label("test").Load(0).Load(1).Branch(0x3F,"body").Op(0x2A);return b.Finish();
    }

    private static byte[] N112_EnemyTargetBody()
    {
        var b=new IL();
        b.Arg(0).Field(0x0400021E).Field(0x040003B5).Virtual(0x06000289).Store(0);
        b.Arg(0).Field(0x0400021E).Field(0x040003B5).Virtual(0x0600028A).Store(1);
        b.Integer(-1).Store(2).Integer(-1).Store(3).Integer(int.MaxValue).Store(4);
        b.Integer(0).Store(6).Branch(0x38,"outerTest");
        b.Label("outer").Integer(0).Store(5).Branch(0x38,"innerTest");
        b.Label("inner").Arg(0).Field(0x0400021E).Field(0x040003B5).Field(0x0400037D).Load(6).Virtual(0x0A000065).Field(0x04000381).Load(5).Virtual(0x0A000078).Field(0x04000384).Branch(0x39,"nextX");
        b.Load(6).Arg(1).Field(0x040001F8).Op(0x59).Store(7);
        b.Load(5).Arg(1).Field(0x040001F9).Op(0x59).Store(8);
        // Native inline absolute value wraps INT_MIN; Math.Abs(int) would throw instead.
        foreach(int local in new[]{7,8})
        {
            string label="abs"+local;
            b.Load(local).Integer(0).Branch(0x3C,label);
            b.Load(local).Op(0x65).Store(local);b.Label(label);
        }
        b.Load(7).Load(8).Op(0x58).Store(7);
        b.Load(7).Load(4).Branch(0x3C,"nextX");
        b.Load(7).Store(4).Load(5).Store(2).Load(6).Store(3);
        b.Label("nextX").Load(5).Integer(1).Op(0x58).Store(5);
        b.Label("innerTest").Load(5).Load(0).Branch(0x3F,"inner");
        b.Load(6).Integer(1).Op(0x58).Store(6);
        b.Label("outerTest").Load(6).Load(1).Branch(0x3F,"outer");
        b.Load(2).Load(3).Float(0).Arg(0).Field(0x0400021E).Field(0x040003B4).New(0x060002A4).Store(9);
        b.Load(9).Integer(1).Set(0x040003AA);
        b.Arg(1).Field(0x040001FD).Load(9).Virtual(0x0A000151).Op(0x2A);return b.Finish();
    }

    private static byte[] N112_SkillUiBody()
    {
        var b=new IL();
        b.Arg(0).Field(0x04000821).Field(0x040004EA).Op(0x14).Call(0x0A000012).Branch(0x3A,"return");
        b.Arg(0).Field(0x04000821).Field(0x040004D2).Branch(0x3A,"ongoing");
        b.Arg(0).Field(0x04000823).Float(1).Float(1).Float(1).Float(1).New(0x0A0000BE).Virtual(0x0A0001DE);
        b.Arg(0).Field(0x04000821).Field(0x040004D1).Field(0x0400069A).Integer(0).Op(0xFE,0x02).Store(0);
        b.Arg(0).Field(0x04000825).Virtual(0x0A000009).Load(0).Virtual(0x0A000007);
        b.Arg(0).Field(0x04000821).Field(0x040004D1).Field(0x04000690).Store(1);
        b.Load(1).Integer(-1).Branch(0x3B,"zero");
        b.Load(1).Integer(3).Branch(0x40,"normalCharge");
        b.Arg(0).Field(0x04000823).Arg(0).Field(0x04000821).Field(0x040004D2).Op(0x6B).Virtual(0x0A0000F0).Branch(0x38,"return");
        b.Label("normalCharge").Arg(0).Field(0x04000821).Field(0x040004D1).Store(3);
        b.Load(3).Field(0x0400069A).Load(3).Field(0x0400069B).Branch(0x3C,"one");
        b.Arg(0).Field(0x04000823).Store(2);
        b.Arg(0).Field(0x04000821).Field(0x040004D1).Store(3);
        b.Load(2).Load(3).Field(0x04000698).Load(3).Field(0x04000699).Op(0x5B).Virtual(0x0A0000F0).Branch(0x38,"return");
        b.Label("ongoing").Arg(0).Field(0x04000825).Virtual(0x0A000009);
        b.Arg(0).Field(0x04000821).Field(0x040004D1).Field(0x0400069A).Integer(1).Op(0xFE,0x02).Virtual(0x0A000007);
        b.Arg(0).Field(0x04000823).Float(1).Float(0.4f).Float(0).Float(1).New(0x0A0000BE).Virtual(0x0A0001DE);
        b.Arg(0).Field(0x04000821).Field(0x040004D1).Field(0x04000691).Integer(3).Branch(0x3B,"return");
        b.Arg(0).Field(0x04000823).Arg(0).Field(0x04000821).Op(0x25).Field(0x040004D3);
        // Save the numerator so duration is read from the same plant captured after the UI callback.
        b.Store(4).Field(0x040004D1).Field(0x04000694).Store(5);
        b.Load(4).Load(5).Op(0x5B).Virtual(0x0A0000F0).Branch(0x38,"return");
        b.Label("zero").Arg(0).Field(0x04000823).Float(0).Virtual(0x0A0000F0).Branch(0x38,"return");
        b.Label("one").Arg(0).Field(0x04000823).Float(1).Virtual(0x0A0000F0);
        b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N112_PropClickBody()
    {
        var b=new IL();
        b.Arg(0).Field(0x04000578).Store(0);
        b.Load(0).Field(0x040003D9).Field(0x04000233).Branch(0x3A,"return");
        b.Load(0).Field(0x040003BB).Branch(0x3A,"return");
        b.Arg(0).Field(0x04000579).Integer(1).Branch(0x40,"return");
        b.Load(0).Field(0x040003D0).Branch(0x39,"take");
        b.Load(0).Integer(1).Integer(1).Virtual(0x060002BA);
        b.Label("take").Arg(0).Field(0x04000578).Field(0x040003D9).Arg(0).Field(0x0400057A).Set(0x04000233);
        b.Arg(0).Field(0x04000578).Field(0x040003D9).Arg(0).Set(0x04000245);
        // AudioClip is captured before changing state or invoking Camera.main.
        b.Arg(0).Field(0x0400058A).Store(1);
        b.Arg(0).Integer(2).Set(0x04000579);
        b.Call(0x0A000004).Virtual(0x0A000001).Virtual(0x0A000037).Store(2);
        b.Load(1).Load(2).Call(0x06000138).Call(0x0600013D).Op(0x26);
        b.Arg(0).Arg(0).Field(0x04000588).Virtual(0x0A000009).Call(0x2B000028).Set(0x04000589);
        b.Arg(0).Field(0x04000589).Virtual(0x0A00002E);
        b.Arg(0).Field(0x04000578).Field(0x040003C6).Virtual(0x0A00002E).Virtual(N110_SetParent);
        b.Arg(0).Call(0x06000406);
        b.Arg(0).Field(0x04000578).Field(0x040003D9).Virtual(0x060001E3);
        b.Label("return").Op(0x2A);return b.Finish();
    }
}
