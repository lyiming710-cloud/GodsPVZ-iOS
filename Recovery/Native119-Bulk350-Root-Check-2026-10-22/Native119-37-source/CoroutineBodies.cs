internal static partial class Program
{
    private static byte[] N119_BenchmarkStart(bool ui)
    {
        int state=ui?0x0400094F:0x04000941,current=state+1,owner=state+2,index=state+3;
        int mode=ui?0x04000945:0x04000937,tmp=ui?0x04000949:0x0400093A,tm=ui?0x0400094A:0x0400093C,font=ui?0x04000947:0x04000938,oldFont=ui?0x04000948:0x04000939,mat1=ui?0x0400094D:0x0400093F,mat2=ui?0x0400094E:0x04000940;
        var b=new IL();b.Arg(0).Field(owner).Store(0);b.Arg(0).Field(state).Store(1);b.Load(1).Branch(0x39,"initial");b.Load(1).Integer(1).Branch(0x3B,"resume");b.Load(1).Integer(2).Branch(0x40,"false");b.Arg(0).Integer(-1).Set(state).Branch(0x38,"false");
        b.Label("resume").Arg(0).Integer(-1).Set(state);b.Arg(0).Arg(0).Field(index).Integer(1).Op(0x58).Set(index).Branch(0x38,"loopTest");
        b.Label("initial").Arg(0).Integer(-1).Set(state);b.Load(0).Field(mode).Branch(0x39,"tmpInit");b.Load(0).Field(mode).Integer(1).Branch(0x40,"startLoop");
        b.Load(0).Load(0).Virtual(0x0A000009).Virtual(ui?0x2B00009C:0x2B000099).Set(tm);
        b.Load(0).Field(oldFont).Op(0x14).Call(0x0A00000C).Branch(0x39,ui?"oldFontDone":"defaultFont");b.Load(0).Field(tm).Load(0).Field(oldFont).Virtual(ui?0x0A000300:0x0A0002FF).Branch(0x38,"oldFontDone");
        if(!ui)b.Label("defaultFont").Load(0).Field(tm).Ref(0x72,N119_Arial).Ref(0xD0,0x0100007D).Call(0x0A00004C).Call(0x0A0002F3).Ref(0x75,0x0100007D).Virtual(0x0A0002FF);
        b.Label("oldFontDone");if(!ui)b.Load(0).Field(tm).Virtual(0x2B00001F).Load(0).Field(tm).Virtual(0x0A0002F4).Virtual(0x0A0002F5).Virtual(N115_SharedMaterial);
        b.Load(0).Field(tm).Integer(48).Virtual(ui?0x0A000301:0x0A0002F7);b.Load(0).Field(tm).Integer(4).Virtual(ui?0x0A000302:0x0A0002F8).Branch(0x38,"startLoop");
        b.Label("tmpInit").Load(0).Load(0).Virtual(0x0A000009).Virtual(ui?0x2B00009D:0x2B00009A).Set(tmp);if(!ui)b.Load(0).Field(tmp).Integer(1).Virtual(0x0A00030E);
        b.Load(0).Field(font).Op(0x14).Call(0x0A00000C).Branch(0x39,"tmpFontDone");b.Load(0).Field(tmp).Load(0).Field(font).Virtual(0x0A0002F9);
        b.Label("tmpFontDone").Load(0).Field(tmp).Float(48).Virtual(0x0A0002A3);b.Load(0).Field(tmp).Integer(514).Virtual(0x0A0002FA);b.Load(0).Field(tmp).Integer(1).Virtual(0x0A0002FB);if(!ui)b.Load(0).Field(tmp).Integer(0).Virtual(0x0A0002FC);
        b.Load(0).Load(0).Field(tmp).Virtual(N119_TmpFont).Field(0x0A00030A).Set(mat1);b.Load(0).Ref(0x72,ui?N119_Bevel:N119_DropShadow).Call(0x2B00009B).Set(mat2);
        b.Label("startLoop").Arg(0).Integer(0).Set(index);b.Label("loopTest").Arg(0).Field(index).Integer(1000000).Branch(0x3D,"lastYield");b.Load(0).Field(mode).Branch(0x39,"tmpCount");b.Load(0).Field(mode).Integer(1).Branch(0x40,"yield");
        b.Load(0).Field(tm).Ref(0x72,N119_CountLong).Arg(0).Field(index).Integer(1000).Op(0x5D).Store(2);b.Address(2).Call(0x0A00002A).Call(0x0A000051).Virtual(ui?N119_UiText:N119_TextMeshText).Branch(0x38,"yield");
        b.Label("tmpCount").Load(0).Field(tmp).Ref(0x72,ui?N119_CountShort:N119_CountFormat);b.Arg(0).Field(index).Integer(1000).Op(0x5D);
        if(ui){b.Store(2);b.Address(2).Call(0x0A00002A).Call(0x0A000051).Virtual(0x0A000062);}else b.Op(0x6B).Virtual(0x0A0002FE);
        b.Arg(0).Field(index).Integer(1000).Op(0x5D).Integer(999).Branch(0x40,"yield");b.Load(0).Field(tmp).Store(3);b.Load(3).Virtual(0x0A0002D7).Load(0).Field(mat1).Call(0x0A000012).Branch(0x39,"material1");b.Load(0).Field(mat2).Store(4).Branch(0x38,"setMaterial");b.Label("material1").Load(0).Field(mat1).Store(4);
        b.Label("setMaterial").Load(0).Field(tmp).Load(4).Virtual(0x0A000359);b.Load(3).Load(4).Virtual(0x0A000359);
        b.Label("yield").Arg(0).Op(0x14).Set(current);b.Arg(0).Integer(1).Set(state);b.Integer(1).Op(0x2A);
        b.Label("lastYield").Arg(0).Op(0x14).Set(current);b.Arg(0).Integer(2).Set(state);b.Integer(1).Op(0x2A);b.Label("false").Integer(0).Op(0x2A);return b.Finish();
    }
    private static byte[] N119_FloatingText(bool tmp)
    {
        int state=tmp?0x040009D3:0x040009C9,current=state+1,owner=state+2,duration=state+3,starting=state+4,count=state+5,startPos=state+6,startColor=state+7,alpha=state+8,fade=state+9,text=tmp?0x040009BE:0x040009BF;
        var b=new IL();b.Arg(0).Field(owner).Store(0);b.Arg(0).Field(state).Store(1);b.Load(1).Branch(0x39,"initial");b.Load(1).Integer(1).Branch(0x3B,"resume");b.Load(1).Integer(2).Branch(0x40,"false");b.Arg(0).Integer(-1).Set(state);
        b.Load(0).Field(0x040009C1).Arg(0).Field(startPos).Virtual(0x0A000038);b.Load(0).Load(0).Virtual(tmp?0x06000799:0x0600079A).Virtual(0x0A00032B).Op(0x26).Branch(0x38,"false");
        b.Label("resume").Arg(0).Integer(-1).Set(state).Branch(0x38,"loop");b.Label("initial").Arg(0).Integer(-1).Set(state);b.Arg(0).Float(2).Set(duration);b.Arg(0).Float(5).Float(20).Call(0x0A000083).Set(starting);b.Arg(0).Arg(0).Field(starting).Set(count);
        b.Arg(0).Load(0).Field(0x040009C1).Virtual(0x0A000037).Set(startPos);b.Arg(0).Load(0).Field(text).Virtual(tmp?0x0A000356:0x0A000352).Call(0x0A0002C0).Set(startColor);b.Arg(0).Float(255).Set(alpha);b.Arg(0).Float(3).Arg(0).Field(starting).Op(0x5B,0x6B).Arg(0).Field(duration).Op(0x5A,0x6B).Set(fade);
        b.Label("loop").Arg(0).Field(count).Float(0).Branch(0x43,"wait");b.Arg(0).Arg(0).Field(count).Call(0x0A00001A).Arg(0).Field(duration).Op(0x5B,0x6B).Arg(0).Field(starting).Op(0x5A,0x6B,0x59,0x6B).Set(count);
        b.Arg(0).Field(count).Float(3).Branch(0x42,"text");b.Arg(0).Field(alpha).Call(0x0A00001A).Arg(0).Field(fade).Op(0x5B,0x6B).Float(255).Op(0x5A,0x6B,0x59,0x6B).Store(2);
        b.Load(2).Float(0).Branch(0x3F,"alpha0");b.Load(2).Float(255).Branch(0x3D,"alpha255");b.Branch(0x38,"alphaSet");b.Label("alpha0").Float(0).Store(2).Branch(0x38,"alphaSet");b.Label("alpha255").Float(255).Store(2);b.Label("alphaSet").Arg(0).Load(2).Set(alpha);
        b.Label("text").Arg(0).Field(count).Op(0x69).Store(3);b.Load(0).Field(text).Address(3).Call(0x0A00002A).Virtual(tmp?0x0A000062:0x0A0002FD);
        b.Load(0).Field(text);foreach(int color in new[]{0x0A00036B,0x0A00036C,0x0A00036D})b.Arg(0).Ref(0x7C,startColor).Field(color).Op(0x6B).Float(255).Op(0x5B,0x6B);
        b.Arg(0).Field(alpha).Op(0x69,0xD2,0x6B).Float(255).Op(0x5B,0x6B).New(0x0A0000BE).Virtual(tmp?0x0A0001DC:0x0A000303);
        b.Load(0).Field(0x040009C1).Store(4);b.Load(4).Virtual(0x0A000037).Store(5);b.Arg(0).Field(starting).Store(2);b.Call(0x0A00001A).Load(2).Op(0x5A,0x6B).Store(2);
        b.Load(4).Address(5).Field(0x0A00001D).Float(0).Op(0x58,0x6B).Address(5).Field(0x0A00008B).Load(2).Op(0x58,0x6B).Address(5).Field(0x0A00009D).Float(0).Op(0x58,0x6B).New(0x0A00009C).Virtual(0x0A000038);
        b.Load(0).Field(0x040009C3).Load(0).Field(0x040009C2).Virtual(0x0A000037).Integer(1000).Call(0x0A000353).Branch(0x39,"cameraChanged");b.Load(0).Field(0x040009C4).Load(0).Field(0x040009C2).Virtual(0x0A000316).Integer(1000).Call(0x0A000354).Branch(0x3A,"yield");
        b.Label("cameraChanged").Load(0).Load(0).Field(0x040009C2).Virtual(0x0A000037).Set(0x040009C3);b.Load(0).Load(0).Field(0x040009C2).Virtual(0x0A000316).Set(0x040009C4);b.Load(0).Field(0x040009C1).Load(0).Field(0x040009C4).Virtual(0x0A000319);
        b.Load(0).Field(0x040009C0).Virtual(0x0A000037).Store(5);b.Load(0).Field(0x040009C0).Address(5).Field(0x0A00001D).Load(0).Ref(0x7C,0x040009C3).Field(0x0A00001D).Op(0x59,0x6B).Float(0).Address(5).Field(0x0A00009D).Load(0).Ref(0x7C,0x040009C3).Field(0x0A00009D).Op(0x59,0x6B).New(0x0A00009C).Virtual(0x0A000355);
        b.Label("yield").Arg(0).Static(0x040009C7).Set(current);b.Arg(0).Integer(1).Set(state);b.Integer(1).Op(0x2A);
        // The two original coroutines intentionally have different exclusive upper bounds.
        b.Label("wait").Arg(0).Static(0x040009C8).Integer(0).Integer(tmp?19:20).Call(0x0A00012B).Op(0x9A).Set(current);b.Arg(0).Integer(2).Set(state);b.Integer(1).Op(0x2A);b.Label("false").Integer(0).Op(0x2A);return b.Finish();
    }
}
