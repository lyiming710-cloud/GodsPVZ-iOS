internal static partial class Program
{
    private static byte[] N115_SetPlantDataBody()
    {
        var b=new IL();b.Arg(0).Arg(1).Set(0x04000868);
        b.Static(0x0A000013).Store(1);b.Arg(0).Virtual(0x0A00003C).Store(0);
        b.Load(0).Call(0x0600090C).Store(3);
        var formats=new[]{
            (Hash:0x7ADA1982u,Name:0x7001B9DF,Format:0x7001B9E5),
            (Hash:0xBAABD2D5u,Name:0x7001B9F9,Format:0x7001BA01),
            (Hash:0x67029C76u,Name:0x7001BA0B,Format:0x7001BA11),
            (Hash:0x70EB52E5u,Name:0x7001BA1F,Format:0x7001BA25),
            (Hash:0x4A0EFFACu,Name:0x7001BA31,Format:0x7001BA39),
            (Hash:0x529C2493u,Name:0x7001BA45,Format:0x7001BA4D),
            (Hash:0x549C27B9u,Name:0x7001BA61,Format:0x7001BA69)};
        for(int i=0;i<formats.Length;i++)b.Load(3).Integer(unchecked((int)formats[i].Hash)).Branch(0x3B,"format"+i);
        b.Branch(0x38,"text");
        for(int i=0;i<formats.Length;i++)
        {
            b.Label("format"+i).Load(0).Ref(0x72,formats[i].Name).Call(0x0A00003A).Branch(0x39,"text");
            b.Ref(0x72,formats[i].Format).Store(1).Branch(0x38,"text");
        }
        b.Label("text").Arg(0).Field(0x04000866).Store(2);
        b.Load(2).ArgAddress(1).Load(1).Call(0x0A000125).Integer(1).Virtual(0x0A00004B);
        b.Float(0).Store(5);b.Float(0).Store(6);for(int i=7;i<=12;i++)b.Float(1).Store(i);
        foreach(int text in new[]{0x7001BA0B,0x7001BA45,0x7001B9DF})
            b.Arg(0).Virtual(0x0A00003C).Ref(0x72,text).Call(0x0A00003A).Branch(0x3A,"hide");
        b.Arg(1).Arg(0).Field(0x04000869).Op(0x5B,0x6B).Store(4);
        b.Load(4).Float(0).Branch(0x44,"range1");b.Load(4).Float(1).Branch(0x42,"range1");b.Load(4).Store(5);
        b.Label("range1").Load(4).Float(1).Branch(0x43,"range3");b.Load(4).Float(3).Branch(0x42,"range3");
        b.Float(1).Store(5);b.Load(4).Float(1).Op(0x59).Float(.5f).Op(0x5A).Store(6);
        b.Float(.61f).Store(10);b.Float(.34f).Store(12);
        b.Label("range3").Load(4).Float(3).Branch(0x43,"range7");b.Load(4).Float(7).Branch(0x42,"range7");
        b.Float(1).Store(5);b.Load(4).Float(3).Op(0x59).Float(.25f).Op(0x5A).Store(6);
        b.Float(.61f).Store(7);b.Float(.34f).Store(9);b.Float(.28f).Store(10);b.Float(.9f).Store(11);b.Float(1).Store(12);
        b.Label("range7").Load(4).Float(7).Branch(0x43,"range15");b.Load(4).Float(15).Branch(0x41,"range15");
        b.Float(1).Store(5);b.Load(4).Float(7).Op(0x59).Float(.125f).Op(0x5A).Store(6);
        b.Float(.28f).Store(7);b.Float(.9f).Store(8);b.Float(1).Store(9);
        b.Float(.51f).Store(10);b.Float(.455f).Store(11);b.Float(1).Store(12);
        b.Label("range15").Load(4).Float(15).Branch(0x44,"clamp");
        b.Float(0).Store(5);b.Float(1).Store(6);b.Float(1).Store(10);b.Float(.95f).Store(11);b.Float(0).Store(12);
        b.Label("clamp").Load(5).Float(0).Float(1).Call(0x0A0000BD).Store(5);
        b.Load(6).Float(0).Float(1).Call(0x0A0000BD).Store(6);
        b.Arg(0).Field(0x04000863).Load(5).Virtual(0x0A0000F0);b.Arg(0).Field(0x04000864).Load(6).Virtual(0x0A0000F0);
        b.Arg(0).Field(0x04000863).Load(7).Load(8).Load(9).Float(1).New(0x0A0000BE).Virtual(0x0A0001DE);
        b.Arg(0).Field(0x04000864).Load(10).Load(11).Load(12).Float(1).New(0x0A0000BE).Virtual(0x0A0001DE).Branch(0x38,"return");
        b.Label("hide").Arg(0).Field(0x04000863).Float(0).Virtual(0x0A0000F0);
        b.Arg(0).Field(0x04000864).Float(0).Virtual(0x0A0000F0);
        b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N115_CameraLateUpdateBody()
    {
        var b=new IL();b.Arg(0).Call(0x0600075D);
        b.Arg(0).Field(0x04000967).Op(0x14).Call(0x0A00000C).Branch(0x39,"return");
        b.Arg(0).Field(0x0400096F).Integer(1).Branch(0x3B,"world");
        b.Arg(0).Field(0x0400096F).Integer(0).Branch(0x40,"movement");
        b.Integer(0).Store(3).Branch(0x38,"offset");b.Label("world").Integer(1).Store(3);
        b.Label("offset").Arg(0).Field(0x04000967).Virtual(0x0A000037).Store(0);
        b.Arg(0).Field(0x0400096B).Store(4);b.Arg(0).Field(0x0400096E).Store(5);
        b.Load(3).Branch(0x3A,"euler");b.Arg(0).Field(0x04000967).Store(2);
        // Public Euler(float,float,float) performs the same float32 degree-to-radian conversion.
        b.Label("euler").Load(4).Load(5).Float(0).Call(0x0A00020E).Store(6);
        b.Load(6).Float(0).Float(0).Arg(0).Field(0x04000968).Op(0x65).New(0x0A00009C).Call(0x0A00020F).Store(1);
        b.Load(3).Branch(0x3A,"desired");b.Load(2).Load(1).Virtual(0x0A000313).Store(1);
        b.Label("desired").Arg(0);
        foreach(int axis in new[]{0x0A00001D,0x0A00008B,0x0A00009D})b.Address(0).Field(axis).Address(1).Field(axis).Op(0x58,0x6B);
        b.New(0x0A00009C).Set(0x04000977);
        b.Label("movement").Arg(0).Field(0x04000970).Store(3);b.Arg(0).Field(0x04000965).Store(2);
        b.Load(3).Branch(0x3A,"smooth");b.Arg(0).Field(0x04000977).Store(1).Branch(0x38,"position");
        b.Label("smooth").Load(2).Virtual(0x0A000037).Store(0);
        b.Arg(0).Field(0x04000973).Store(4);b.Arg(0).Field(0x04000977).Store(1);
        b.Call(0x0A0001D2).Load(4).Op(0x5A,0x6B).Store(4);b.Call(0x0A00001A).Store(5);
        b.Load(0).Load(1).Arg(0).FieldAddress(0x04000976).Load(4).Float(float.PositiveInfinity).Load(5).Call(0x0A000314).Store(1);
        b.Label("position").Load(2).Load(1).Virtual(0x0A000038);
        b.Arg(0).Field(0x04000971).Store(3);b.Arg(0).Field(0x04000965).Store(2);
        b.Load(3).Branch(0x3A,"rotation");b.Load(2).Arg(0).Field(0x04000967).Virtual(0x0A000315).Branch(0x38,"return");
        b.Label("rotation").Load(2).Virtual(0x0A000316).Store(6);
        b.Arg(0).Field(0x04000967).Virtual(0x0A000037).Store(0);
        b.Arg(0).Field(0x04000965).Virtual(0x0A000037).Store(1);
        foreach(int axis in new[]{0x0A00001D,0x0A00008B,0x0A00009D})b.Address(0).Field(axis).Address(1).Field(axis).Op(0x59,0x6B);
        b.New(0x0A00009C).Call(0x0A000317).Store(7);
        b.Arg(0).Field(0x04000974).Store(4);b.Call(0x0A00001A).Load(4).Op(0x5A,0x6B).Store(4);
        b.Load(2).Load(6).Load(7).Load(4).Call(0x0A000318).Virtual(0x0A000319);
        b.Label("return").Op(0x2A);return b.Finish();
    }

    private static void N115_Yellow(IL b) => b.Float(1).Float(1).Float(0).Float(1).New(0x0A0000BE);

    private static byte[] N115_TextSpawnerBody()
    {
        var b=new IL();b.Integer(0).Store(0).Branch(0x38,"test");
        b.Label("loop").Arg(0).Field(0x040009DD).Branch(0x39,"tmp");
        b.New(0x0A0000E2).Store(1);N115_SpawnerPosition(b);
        b.Load(1).Virtual(0x2B000099).Store(3);
        b.Load(3).Call(0x2B00001F).Arg(0).Field(0x040009DF).Virtual(0x0A0002F5).Virtual(N115_SharedMaterial);
        b.Load(3).Arg(0).Field(0x040009DF).Virtual(0x0A0002FF);
        b.Load(3).Integer(7).Virtual(0x0A0002F8);b.Load(3).Integer(96).Virtual(0x0A0002F7);
        b.Load(3);N115_Yellow(b);b.Virtual(0x0A000303);b.Load(3).Ref(0x72,0x7001F57F).Virtual(0x0A0002FD);
        b.Arg(0).Load(1).Virtual(0x2B00009F).Set(0x040009E0);
        b.Arg(0).Field(0x040009E0).Integer(1).Set(0x040009C5).Branch(0x38,"next");
        b.Label("tmp").New(0x0A0000E2).Store(1);N115_SpawnerPosition(b);
        b.Load(1).Virtual(0x2B00009A).Store(4);
        b.Load(4).Float(96).Virtual(0x0A0002A3);b.Load(4).Ref(0x72,0x7001F57F).Virtual(0x0A000062);
        b.Load(4);N115_Yellow(b);b.Virtual(0x0A0001DC);
        b.Arg(0).Load(1).Virtual(0x2B00009F).Set(0x040009E0);b.Arg(0).Field(0x040009E0).Integer(0).Set(0x040009C5);
        b.Label("next").Load(0).Integer(1).Op(0x58).Store(0);
        b.Label("test").Load(0).Arg(0).Field(0x040009DE).Branch(0x3F,"loop").Op(0x2A);return b.Finish();
    }

    private static void N115_SpawnerPosition(IL b)
    {
        b.Load(1).Virtual(0x0A00002E).Store(2);
        b.Float(-95).Float(95).Call(0x0A000083).Store(5);b.Float(-95).Float(95).Call(0x0A000083).Store(6);
        b.Load(2).Load(5).Float(.5f).Load(6).New(0x0A00009C).Virtual(0x0A000038);
    }

    private static void N115_BenchmarkPosition(IL b,float height)
    {
        b.Load(1).Virtual(0x0A00002E).Store(6);
        b.Float(-95).Float(95).Call(0x0A000083).Store(4);b.Float(-95).Float(95).Call(0x0A000083).Store(5);
        b.Load(6).Load(4).Float(height).Load(5).New(0x0A00009C).Virtual(0x0A000038);
    }

    private static byte[] N115_BenchmarkBody()
    {
        var b=new IL();b.Integer(0).Store(0).Branch(0x38,"test");
        b.Label("loop").Arg(0).Field(0x04000953).Integer(0).Branch(0x3B,"tmp");
        b.Arg(0).Field(0x04000953).Integer(1).Branch(0x3B,"legacy");
        b.Arg(0).Field(0x04000953).Integer(2).Branch(0x3B,"canvas").Branch(0x38,"next");
        b.Label("canvas").New(0x0A0000E2).Store(1);
        b.Load(1).Virtual(0x2B00009E).Call(0x0A000004).Virtual(0x0A000005);
        b.Load(1).Virtual(0x0A00002E).Float(.1f).Float(.1f).Float(.1f).New(0x0A00009C).Virtual(0x0A0000E5);
        N115_BenchmarkPosition(b,5);
        b.New(0x0A0000E2).Store(2);b.Load(2).Virtual(0x2B00009D).Store(3);
        b.Load(3).Virtual(0x0A0002E3).Load(1).Virtual(0x0A00002E).Integer(0).Virtual(0x0A00002F);
        b.Load(3);N115_Yellow(b);b.Virtual(0x0A0001DC);
        b.Load(3).Integer(1026).Virtual(0x0A0002FA);b.Load(3).Float(96).Virtual(0x0A0002A3);
        b.Load(3).Ref(0x72,0x7001F57F).Virtual(0x0A000062);
        b.Arg(0).Load(1).Virtual(0x2B00009F).Set(0x04000956);b.Arg(0).Field(0x04000956).Integer(0).Set(0x040009C5).Branch(0x38,"next");
        b.Label("legacy").New(0x0A0000E2).Store(1);N115_BenchmarkPosition(b,.25f);
        b.Load(1).Virtual(0x2B000099).Store(7);
        b.Load(7).Ref(0x72,0x7001E504).Call(0x2B0000A0).Virtual(0x0A0002FF);
        b.Load(7).Call(0x2B00001F).Load(7).Virtual(0x0A0002F4).Virtual(0x0A0002F5).Virtual(N115_SharedMaterial);
        b.Load(7).Integer(7).Virtual(0x0A0002F8);b.Load(7).Integer(96).Virtual(0x0A0002F7);
        b.Load(7);N115_Yellow(b);b.Virtual(0x0A000303);b.Load(7).Ref(0x72,0x7001F57F).Virtual(0x0A0002FD);
        b.Arg(0).Load(1).Virtual(0x2B00009F).Set(0x04000956);b.Arg(0).Field(0x04000956).Integer(1).Set(0x040009C5).Branch(0x38,"next");
        b.Label("tmp").New(0x0A0000E2).Store(1);N115_BenchmarkPosition(b,.25f);
        b.Load(1).Virtual(0x2B00009A).Store(3);b.Load(3).Integer(1).Virtual(0x0A000304);
        b.Address(8).Op(0xFE).Ref(0x15,0x0100004B);
        b.Address(8).Float(.5f).Set(0x0A0001D4);
        b.Load(3).Virtual(0x0A0002E3).Load(8).Virtual(0x0A000305);
        return N115_FinishBenchmark(b);
    }

    private static byte[] N115_FinishBenchmark(IL b)
    {
        b.Load(3).Integer(1026).Virtual(0x0A0002FA);b.Load(3).Float(96).Virtual(0x0A0002A3);
        b.Load(3).Integer(0).Virtual(0x0A000306);b.Load(3);N115_Yellow(b);b.Virtual(0x0A0001DC);
        b.Load(3).Ref(0x72,0x7001F57F).Virtual(0x0A000062);
        b.Load(3).Arg(0).Field(0x04000955).Virtual(0x0A000307);
        b.Arg(0).Load(1).Virtual(0x2B00009F).Set(0x04000956);
        b.Arg(0).Field(0x04000956).Integer(0).Set(0x040009C5);
        b.Arg(0).Field(0x04000956).Arg(0).Field(0x04000955).Set(0x040009C6);
        b.Label("next").Load(0).Integer(1).Op(0x58).Store(0);
        b.Label("test").Load(0).Arg(0).Field(0x04000954).Branch(0x3F,"loop").Op(0x2A);return b.Finish();
    }

    private static void N115_MeshArray(IL b,bool cached,int field,int local)
    {
        if(cached)b.Arg(0).Field(0x04000A18);
        else b.Arg(0).Field(0x04000A10).Virtual(0x0A0002B8).Field(0x0A0002BE);
        b.Load(0).Ref(0x8F,0x0100006A).Field(field).Store(local);
    }

    private static void N115_CopyQuad(IL b,int source,int target,int index,int element)
    {
        for(int i=0;i<4;i++)
        {
            b.Load(target).Load(index);if(i!=0)b.Integer(i).Op(0x58);
            b.Load(source).Load(index);if(i!=0)b.Integer(i).Op(0x58);
            b.Ref(0xA3,element).Ref(0xA4,element);
        }
    }

    private static byte[] N115_RestoreVertexBody()
    {
        var b=new IL();b.Arg(1).Integer(-1).Branch(0x3B,"return");
        b.Arg(1).Arg(0).Field(0x04000A10).Virtual(0x0A0002B8).Field(0x0A000339).Integer(1).Op(0x59).Branch(0x3D,"return");
        b.Arg(0).Field(0x04000A10).Virtual(0x0A0002B8).Field(0x0A0002BB).Arg(1).Ref(0x8F,0x01000068).Field(0x0A0002BC).Store(0);
        b.Arg(0).Field(0x04000A10).Virtual(0x0A0002B8).Field(0x0A0002BB).Arg(1).Ref(0x8F,0x01000068).Field(0x0A0002BD).Store(1);
        N115_MeshArray(b,true,0x0A00033E,2);N115_MeshArray(b,false,0x0A00033E,3);N115_CopyQuad(b,2,3,1,0x01000013);
        N115_MeshArray(b,false,0x0A0002BF,5);N115_MeshArray(b,true,0x0A0002BF,4);N115_CopyQuad(b,4,5,1,0x01000069);
        N115_MeshArray(b,true,N115_Uvs0,6);N115_MeshArray(b,false,N115_Uvs0,7);N115_CopyQuad(b,6,7,1,0x0100004B);
        N115_MeshArray(b,true,N115_Uvs2,8);N115_MeshArray(b,false,N115_Uvs2,9);N115_CopyQuad(b,8,9,1,0x0100004B);
        b.Load(2).Op(0x8E,0x69).Integer(4).Op(0x5B).Integer(4).Op(0x5A).Integer(4).Op(0x59).Store(10);
        N115_CopyQuad(b,2,3,10,0x01000013);
        N115_MeshArray(b,true,0x0A0002BF,4);N115_MeshArray(b,false,0x0A0002BF,5);N115_CopyQuad(b,4,5,10,0x01000069);
        N115_MeshArray(b,true,N115_Uvs0,6);N115_MeshArray(b,false,N115_Uvs0,7);N115_CopyQuad(b,6,7,10,0x0100004B);
        N115_MeshArray(b,true,N115_Uvs2,8);N115_MeshArray(b,false,N115_Uvs2,9);N115_CopyQuad(b,8,9,10,0x0100004B);
        b.Arg(0).Field(0x04000A10).Integer(255).Virtual(0x0A000383);
        b.Label("return").Op(0x2A);return b.Finish();
    }
}
