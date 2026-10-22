internal static partial class Program
{
    private static void N119_CameraClamp(IL b,int value,int minimum,int maximum,string p)
    {
        b.Load(value).Arg(0).Field(minimum).Branch(0x3F,p+"min");b.Load(value).Arg(0).Field(maximum).Branch(0x43,p+"done");b.Arg(0).Field(maximum).Store(value).Branch(0x38,p+"done");b.Label(p+"min").Arg(0).Field(minimum).Store(value);b.Label(p+"done");
    }
    private static void N119_CameraAngle(IL b,int delta,float sensitivity,bool mouse,string p)
    {
        b.Load(delta).Float(.01f).Branch(0x3D,p+"yes");b.Load(delta).Float(-.01f).Branch(0x41,p+"done");b.Label(p+"yes");
        b.Arg(0).Field(0x0400096B).Load(delta);if(mouse)b.Arg(0).Field(0x04000975);else b.Float(sensitivity);b.Op(0x5A,0x6B,0x59,0x6B).Store(3);N119_CameraClamp(b,3,0x0400096D,0x0400096C,p+"clamp");b.Arg(0).Load(3).Set(0x0400096B);b.Label(p+"done");
    }
    private static void N119_CameraOrbit(IL b,int delta,float sensitivity,bool mouse,string p)
    {
        b.Load(delta).Float(.01f).Branch(0x3D,p+"yes");b.Load(delta).Float(-.01f).Branch(0x41,p+"done");b.Label(p+"yes").Arg(0).Load(delta);if(mouse)b.Arg(0).Field(0x04000975);else b.Float(sensitivity);
        b.Op(0x5A,0x6B).Arg(0).Field(0x0400096E).Op(0x58,0x6B).Set(0x0400096E);b.Arg(0).Field(0x0400096E).Float(360).Branch(0x43,p+"negative");b.Arg(0).Arg(0).Field(0x0400096E).Float(360).Op(0x59,0x6B).Set(0x0400096E);
        b.Label(p+"negative").Arg(0).Field(0x0400096E).Float(0).Branch(0x41,p+"done");b.Arg(0).Arg(0).Field(0x0400096E).Float(360).Op(0x58,0x6B).Set(0x0400096E);b.Label(p+"done");
    }
    private static void N119_Vector2Diff(IL b,int a,int z,int result)
    {
        b.Address(a).Field(0x0A0001D4).Address(z).Field(0x0A0001D4).Op(0x59,0x6B).Address(a).Field(0x0A0001D5).Address(z).Field(0x0A0001D5).Op(0x59,0x6B).New(N119_Vector2Ctor).Store(result);
    }
    private static byte[] N119_CameraInput()
    {
        var b=new IL();b.Arg(0).Float(0).Float(0).Float(0).New(0x0A00009C).Set(0x0400097A);b.Arg(0).Ref(0x72,N119_MouseWheel).Call(N119_GetAxis).Set(0x0400097B);b.Call(0x0A000326).Store(0);
        b.Integer(304).Call(0x0A00037B).Branch(0x3A,"controls");b.Integer(303).Call(0x0A00037B).Branch(0x3A,"controls");b.Load(0).Integer(0).Branch(0x3E,"pinch");
        b.Label("controls").Arg(0).Arg(0).Field(0x0400097B).Float(10).Op(0x5A,0x6B).Set(0x0400097B);
        b.Integer(105).Call(0x0A0001D8).Branch(0x39,"f");b.Arg(0).Integer(1).Set(0x0400096F);b.Label("f").Integer(102).Call(0x0A0001D8).Branch(0x39,"s");b.Arg(0).Integer(0).Set(0x0400096F);b.Label("s").Integer(115).Call(0x0A0001D8).Branch(0x39,"rotate");b.Arg(0).Arg(0).Field(0x04000970).Integer(0).Op(0xFE,0x01).Set(0x04000970);
        b.Label("rotate").Integer(1).Call(0x0A000287).Branch(0x39,"touch");b.Arg(0).Ref(0x72,N119_MouseY).Call(N119_GetAxis).Set(0x04000979);b.Arg(0).Ref(0x72,N119_MouseX).Call(N119_GetAxis).Set(0x04000978);b.Arg(0).Field(0x04000979).Store(1);N119_CameraAngle(b,1,0,true,"mouseAngle");b.Arg(0).Field(0x04000978).Store(2);N119_CameraOrbit(b,2,0,true,"mouseOrbit");
        b.Label("touch").Load(0).Integer(1).Branch(0x40,"select");b.Integer(0).Call(0x0A00031A).Store(4);b.Address(4).Call(0x0A00031E).Integer(1).Branch(0x40,"select");b.Integer(0).Call(0x0A00031A).Store(4);b.Address(4).Call(N119_TouchDelta).Store(5);b.Address(5).Field(0x0A0001D5).Store(1);b.Address(5).Field(0x0A0001D4).Store(2);N119_CameraAngle(b,1,.1f,false,"touchAngle");N119_CameraOrbit(b,2,.1f,false,"touchOrbit");
        b.Label("select").Integer(0).Call(0x0A000287).Branch(0x39,"pan");b.Call(0x0A000004).Call(0x0A000023).Virtual(0x0A00031F).Address(6).Float(300).Integer(0x5C00).Call(0x0A000320).Branch(0x39,"pan");b.Address(6).Call(0x0A000322).Arg(0).Field(0x04000967).Call(0x0A000012).Branch(0x3A,"orbitZero");
        b.Arg(0).Address(6).Call(0x0A000322).Set(0x04000967);b.Arg(0).Arg(0).Field(0x04000972).Set(0x04000970);b.Label("orbitZero").Arg(0).Float(0).Set(0x0400096E);
        b.Label("pan").Integer(2).Call(0x0A000287).Branch(0x39,"pinch");b.Arg(0).Field(0x04000966).Op(0x14).Call(0x0A000012).Branch(0x3A,"newDummy");b.Arg(0).Field(0x04000966).Arg(0).Field(0x04000967).Call(0x0A00000C).Branch(0x39,"drag").Branch(0x38,"copyTarget");
        b.Label("newDummy").Arg(0).Ref(0x72,N119_CameraTarget).New(0x0A0000F3).Virtual(0x0A00002E).Set(0x04000966);
        b.Label("copyTarget").Arg(0).Field(0x04000966).Arg(0).Field(0x04000967).Virtual(0x0A000037).Virtual(0x0A000038);b.Arg(0).Field(0x04000966).Arg(0).Field(0x04000967).Virtual(0x0A000316).Virtual(0x0A000319);b.Arg(0).Arg(0).Field(0x04000966).Set(0x04000967);b.Arg(0).Arg(0).Field(0x04000970).Set(0x04000972);b.Arg(0).Integer(0).Set(0x04000970);
        b.Label("drag").Arg(0).Ref(0x72,N119_MouseY).Call(N119_GetAxis).Set(0x04000979);b.Arg(0).Ref(0x72,N119_MouseX).Call(N119_GetAxis).Set(0x04000978);b.Arg(0).Arg(0).Field(0x04000965).Arg(0).Field(0x04000978).Arg(0).Field(0x04000979).Float(0).Virtual(0x0A000323).Set(0x0400097A);
        b.Arg(0).Field(0x04000966);foreach(int f in new[]{0x0A00001D,0x0A00008B,0x0A00009D})b.Arg(0).Ref(0x7C,0x0400097A).Field(f).Op(0x65);b.New(0x0A00009C).Integer(0).Virtual(0x0A000324);
        b.Label("pinch").Load(0).Integer(2).Branch(0x40,"wheel");b.Integer(0).Call(0x0A00031A).Store(4);b.Integer(1).Call(0x0A00031A).Store(7);
        b.Address(4).Call(N119_TouchPosition).Store(8);b.Address(4).Call(N119_TouchDelta).Store(9);N119_Vector2Diff(b,8,9,10);b.Address(7).Call(N119_TouchPosition).Store(8);b.Address(7).Call(N119_TouchDelta).Store(9);N119_Vector2Diff(b,8,9,11);N119_Vector2Diff(b,10,11,12);b.Address(12).Call(N119_Vector2Magnitude).Store(1);
        b.Address(4).Call(N119_TouchPosition).Store(8);b.Address(7).Call(N119_TouchPosition).Store(9);N119_Vector2Diff(b,8,9,12);b.Load(1).Address(12).Call(N119_Vector2Magnitude).Op(0x59,0x6B).Store(1);
        b.Load(1).Float(.01f).Branch(0x3D,"pinchAdjust");b.Load(1).Float(-.01f).Branch(0x41,"wheel");b.Label("pinchAdjust").Load(1).Float(.25f).Op(0x5A,0x6B).Arg(0).Field(0x04000968).Op(0x58,0x6B).Store(3);N119_CameraClamp(b,3,0x0400096A,0x04000969,"pinchClamp");b.Arg(0).Load(3).Set(0x04000968);
        b.Label("wheel").Arg(0).Field(0x0400097B).Float(-.01f).Branch(0x3F,"wheelAdjust");b.Arg(0).Field(0x0400097B).Float(.01f).Branch(0x43,"return");b.Label("wheelAdjust").Arg(0).Field(0x04000968).Arg(0).Field(0x0400097B).Float(5).Op(0x5A,0x6B,0x59,0x6B).Store(3);N119_CameraClamp(b,3,0x0400096A,0x04000969,"wheelClamp");b.Arg(0).Load(3).Set(0x04000968);
        b.Label("return").Op(0x2A);return b.Finish();
    }
}
