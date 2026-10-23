internal static partial class Program
{
    private static void N119_Char(IL b,int info,int index)=>b.Arg(0).Field(info).Field(0x0A0002BB).Load(index).Ref(0x8F,0x01000068);
    private static void N119_MeshField(IL b,int info,int material,int field)=>b.Arg(0).Field(info).Field(0x0A0002BE).Load(material).Ref(0x8F,0x0100006A).Field(field);
    private static void N119_QuadCenter(IL b,int source,int vertex,int center)
    {
        foreach(int axis in new[]{0x0A00001D,0x0A00008B})b.Load(source).Load(vertex).Integer(2).Op(0x58).Ref(0x8F,0x01000013).Field(axis).Load(source).Load(vertex).Ref(0x8F,0x01000013).Field(axis).Op(0x58,0x6B).Float(.5f).Op(0x5A,0x6B);
        b.Float(0).New(0x0A00009C).Store(center);
    }
    private static void N119_QuadSubtract(IL b,int source,int dest,int vertex,int center)
    {
        for(int k=0;k<4;k++)
        {
            b.Load(dest).Load(vertex).Integer(k).Op(0x58);
            foreach(int axis in new[]{0x0A00001D,0x0A00008B,0x0A00009D})b.Load(source).Load(vertex).Integer(k).Op(0x58).Ref(0x8F,0x01000013).Field(axis).Address(center).Field(axis).Op(0x59,0x6B);
            b.New(0x0A00009C).Ref(0xA4,0x01000013);
        }
    }
    private static void N119_QuadTransform(IL b,int dest,int vertex,int matrix,int center)
    {
        for(int k=0;k<4;k++)b.Load(dest).Load(vertex).Integer(k).Op(0x58).Address(matrix).Load(dest).Load(vertex).Integer(k).Op(0x58).Ref(0xA3,0x01000013).Call(0x0A000349).Ref(0xA4,0x01000013);
        for(int k=0;k<4;k++)
        {
            b.Load(dest).Load(vertex).Integer(k).Op(0x58);
            foreach(int axis in new[]{0x0A00001D,0x0A00008B,0x0A00009D})b.Address(center).Field(axis).Load(dest).Load(vertex).Integer(k).Op(0x58).Ref(0x8F,0x01000013).Field(axis).Op(0x58,0x6B);
            b.New(0x0A00009C).Ref(0xA4,0x01000013);
        }
    }
    private static void N119_MeshUpload(IL b,int info,int component,int index,string label)
    {
        b.Integer(0).Store(index).Branch(0x38,label+"Test");b.Label(label);N119_MeshField(b,info,index,0x0A00036F);N119_MeshField(b,info,index,0x0A00033E);b.Virtual(0x0A000387);
        b.Load(0).Field(component);N119_MeshField(b,info,index,0x0A00036F);b.Load(index).Virtual(0x0A000388);b.Load(index).Integer(1).Op(0x58).Store(index);b.Label(label+"Test").Load(index).Arg(0).Field(info).Field(0x0A0002BE).Op(0x8E,0x69).Branch(0x3F,label);
    }
    private static byte[] N119_VertexJitter()
    {
        var b=new IL();b.Arg(0).Field(0x04000A37).Store(0);b.Arg(0).Field(0x04000A35).Store(1);b.Load(1).Branch(0x39,"initial");b.Load(1).Integer(1).Branch(0x3B,"resume");b.Load(1).Integer(2).Branch(0x40,"false");
        b.Label("resume").Arg(0).Integer(-1).Set(0x04000A35).Branch(0x38,"changed");b.Label("initial").Arg(0).Integer(-1).Set(0x04000A35);b.Load(0).Field(0x04000A30).Integer(0).Integer(0).Virtual(0x0A000338);
        b.Arg(0).Load(0).Field(0x04000A30).Virtual(0x0A0002B8).Set(0x04000A38);b.Arg(0).Integer(0).Set(0x04000A39);b.Load(0).Integer(1).Set(0x04000A31);b.Arg(0).Integer(1024).Ref(0x8D,0x0200010F).Set(0x04000A3A);
        b.Integer(0).Store(1);b.Label("initAnim").Arg(0).Field(0x04000A3A).Load(1).Ref(0x8F,0x0200010F).Float(10).Float(25).Call(0x0A000083).Set(0x04000A32);b.Arg(0).Field(0x04000A3A).Load(1).Ref(0x8F,0x0200010F).Float(1).Float(3).Call(0x0A000083).Set(0x04000A34);b.Load(1).Integer(1).Op(0x58).Store(1);b.Load(1).Integer(1024).Branch(0x3F,"initAnim");b.Arg(0).Arg(0).Field(0x04000A38).Virtual(0x0A00037A).Set(0x04000A3B);
        b.Label("changed").Load(0).Field(0x04000A31).Branch(0x39,"count");b.Arg(0).Arg(0).Field(0x04000A38).Virtual(0x0A00037A).Set(0x04000A3B);b.Load(0).Integer(0).Set(0x04000A31);
        b.Label("count").Arg(0).Field(0x04000A38).Field(0x0A000339).Store(2);b.Load(2).Branch(0x39,"empty");b.Integer(0).Store(1).Branch(0x38,"charTest");b.Label("char");N119_Char(b,0x04000A38,1);b.Field(0x0A00033D).Branch(0x39,"charNext");
        b.Arg(0).Field(0x04000A3A).Load(1).Ref(0xA3,0x0200010F).Store(5);N119_Char(b,0x04000A38,1);b.Field(0x0A0002BC).Store(3);N119_Char(b,0x04000A38,1);b.Field(0x0A0002BD).Store(4);
        b.Arg(0).Field(0x04000A3B).Load(3).Ref(0x8F,0x0100006A).Field(0x0A00033E).Store(6);N119_QuadCenter(b,6,4,8);N119_MeshField(b,0x04000A38,3,0x0A00033E);b.Store(7);N119_QuadSubtract(b,6,7,4,8);
        b.Address(5).Address(5).Field(0x04000A32).Op(0x65).Address(5).Field(0x04000A32).Arg(0).Field(0x04000A39).Op(0x6B).Float(25).Op(0x5B,0x6B).Address(5).Field(0x04000A34).Op(0x5A,0x6B).Float(1).Call(N119_PingPong).Call(N119_SmoothStep).Set(0x04000A33);
        b.Float(-.25f).Float(.25f).Call(0x0A000083).Store(14);b.Float(-.25f).Float(.25f).Call(0x0A000083).Store(15);b.Load(0).Field(0x04000A2F).Store(16);b.Load(14).Load(16).Op(0x5A,0x6B).Load(15).Load(16).Op(0x5A,0x6B).Load(16).Float(0).Op(0x5A,0x6B).New(0x0A00009C).Store(9);
        b.Float(0).Float(0).Float(-5).Float(5).Call(0x0A000083).Load(0).Field(0x04000A2D).Op(0x5A,0x6B).Call(0x0A00020E).Store(10);b.Load(9).Load(10).Float(1).Float(1).Float(1).New(0x0A00009C).Call(0x0A000348).Store(11);N119_QuadTransform(b,7,4,11,8);
        b.Arg(0).Field(0x04000A3A).Load(1).Load(5).Ref(0xA4,0x0200010F);b.Label("charNext").Load(1).Integer(1).Op(0x58).Store(1);b.Label("charTest").Load(1).Load(2).Branch(0x3F,"char");N119_MeshUpload(b,0x04000A38,0x04000A30,3,"upload");
        b.Arg(0).Arg(0).Field(0x04000A39).Integer(1).Op(0x58).Set(0x04000A39);b.Arg(0).Float(.1f).New(0x0A00034B).Set(0x04000A36);b.Arg(0).Integer(2).Set(0x04000A35);b.Integer(1).Op(0x2A);
        b.Label("empty").Arg(0).Float(.25f).New(0x0A00034B).Set(0x04000A36);b.Arg(0).Integer(1).Set(0x04000A35);b.Integer(1).Op(0x2A);b.Label("false").Integer(0).Op(0x2A);return b.Finish();
    }
    private static byte[] N119_VertexZoom()
    {
        var b=new IL();b.Arg(0).Field(0x04000A5A).Store(0);b.Arg(0).Field(0x04000A58).Store(1);b.Load(1).Branch(0x39,"initial");b.Load(1).Integer(1).Branch(0x3B,"resume");b.Load(1).Integer(2).Branch(0x40,"false");
        b.Label("resume").Arg(0).Integer(-1).Set(0x04000A58).Branch(0x38,"changed");b.Label("initial").Arg(0).Integer(-1).Set(0x04000A58);b.Arg(0).New(0x0600080D).Set(0x04000A5B);b.Load(0).Field(0x04000A54).Integer(0).Integer(0).Virtual(0x0A000338);
        b.Arg(0).Load(0).Field(0x04000A54).Virtual(0x0A0002B8).Set(0x04000A5C);b.Arg(0).Arg(0).Field(0x04000A5C).Virtual(0x0A00037A).Set(0x04000A5D);b.Arg(0).Field(0x04000A5B).New(0x0A000094).Set(0x04000A56);b.Arg(0).New(0x0A000110).Set(0x04000A5E);b.Load(0).Integer(1).Set(0x04000A55);
        b.Label("changed").Load(0).Field(0x04000A55).Branch(0x39,"count");b.Arg(0).Arg(0).Field(0x04000A5C).Virtual(0x0A00037A).Set(0x04000A5D);b.Load(0).Integer(0).Set(0x04000A55);
        b.Label("count").Arg(0).Field(0x04000A5C).Field(0x0A000339).Store(2);b.Load(2).Branch(0x39,"empty");b.Arg(0).Field(0x04000A5B).Field(0x04000A56).Virtual(N119_FloatClear);b.Arg(0).Field(0x04000A5E).Virtual(N119_IntClear);
        b.Integer(0).Store(1).Branch(0x38,"charTest");b.Label("char");N119_Char(b,0x04000A5C,1);b.Field(0x0A00033D).Branch(0x39,"charNext");N119_Char(b,0x04000A5C,1);b.Field(0x0A0002BC).Store(3);N119_Char(b,0x04000A5C,1);b.Field(0x0A0002BD).Store(4);
        b.Arg(0).Field(0x04000A5D).Load(3).Ref(0x8F,0x0100006A).Field(0x0A00033E).Store(6);N119_QuadCenter(b,6,4,8);N119_MeshField(b,0x04000A5C,3,0x0A00033E);b.Store(7);N119_QuadSubtract(b,6,7,4,8);
        b.Float(1).Float(1.5f).Call(0x0A000083).Store(5);b.Arg(0).Field(0x04000A5B).Field(0x04000A56).Load(5).Virtual(0x0A000092);b.Arg(0).Field(0x04000A5E).Arg(0).Field(0x04000A5B).Field(0x04000A56).Virtual(N119_FloatCount).Integer(1).Op(0x59).Virtual(0x0A000291);
        b.Float(0).Float(0).Float(0).New(0x0A00009C).Float(0).Float(0).Float(0).Float(1).New(N119_QuaternionCtor).Load(5).Load(5).Load(5).New(0x0A00009C).Call(0x0A000348).Store(9);N119_QuadTransform(b,7,4,9,8);
        foreach((int field,int type,int sl,int dl) in new[]{(N115_Uvs0,0x0100004B,10,11),(0x0A0002BF,0x01000069,12,13)})
        {
            b.Arg(0).Field(0x04000A5D).Load(3).Ref(0x8F,0x0100006A).Field(field).Store(sl);N119_MeshField(b,0x04000A5C,3,field);b.Store(dl);
            for(int k=0;k<4;k++)b.Load(dl).Load(4).Integer(k).Op(0x58).Load(sl).Load(4).Integer(k).Op(0x58).Ref(0xA3,type).Ref(0xA4,type);
        }
        b.Label("charNext").Load(1).Integer(1).Op(0x58).Store(1);b.Label("charTest").Load(1).Load(2).Branch(0x3F,"char");
        b.Integer(0).Store(3).Branch(0x38,"uploadTest");b.Label("upload").Arg(0).Field(0x04000A5E).Store(14);b.Arg(0).Field(0x04000A5B).Field(0x04000A57).Store(15);b.Load(15).Branch(0x3A,"comparison");
        b.Arg(0).Field(0x04000A5B).Op(0xFE).Ref(0x06,0x0600080E).New(0x0A00038F).Store(15);b.Arg(0).Field(0x04000A5B).Load(15).Set(0x04000A57);b.Label("comparison").Load(14).Load(15).Virtual(0x0A000390);
        b.Arg(0).Field(0x04000A5C).Field(0x0A0002BE).Load(3).Ref(0x8F,0x0100006A).Arg(0).Field(0x04000A5E).Call(0x0A000391);
        foreach((int field,int setter) in new[]{(0x0A00033E,0x0A000387),(N115_Uvs0,0x0A000392),(0x0A0002BF,0x0A000370)}){N119_MeshField(b,0x04000A5C,3,0x0A00036F);N119_MeshField(b,0x04000A5C,3,field);b.Virtual(setter);}
        b.Load(0).Field(0x04000A54);N119_MeshField(b,0x04000A5C,3,0x0A00036F);b.Load(3).Virtual(0x0A000388);b.Load(3).Integer(1).Op(0x58).Store(3);b.Label("uploadTest").Load(3).Arg(0).Field(0x04000A5C).Field(0x0A0002BE).Op(0x8E,0x69).Branch(0x3F,"upload");
        b.Arg(0).Float(.1f).New(0x0A00034B).Set(0x04000A59);b.Arg(0).Integer(2).Set(0x04000A58);b.Integer(1).Op(0x2A);b.Label("empty").Arg(0).Float(.25f).New(0x0A00034B).Set(0x04000A59);b.Arg(0).Integer(1).Set(0x04000A58);b.Integer(1).Op(0x2A);b.Label("false").Integer(0).Op(0x2A);return b.Finish();
    }
    private static void N119_ShakeRefresh(IL b,int info,int copies,int changed,string label)
    {
        b.Load(0).Field(changed).Branch(0x39,label+"done");b.Arg(0).Field(copies).Op(0x8E,0x69).Arg(0).Field(info).Field(0x0A0002BE).Op(0x8E,0x69).Branch(0x3C,label+"sizeOk");b.Arg(0).Arg(0).Field(info).Field(0x0A0002BE).Op(0x8E,0x69).Ref(0x8D,0x1B00008E).Set(copies);
        b.Label(label+"sizeOk").Integer(0).Store(3).Branch(0x38,label+"test");b.Label(label).Arg(0).Field(copies).Load(3);N119_MeshField(b,info,3,0x0A00033E);b.Op(0x8E,0x69).Ref(0x8D,0x01000013).Op(0xA2);b.Load(3).Integer(1).Op(0x58).Store(3);b.Label(label+"test").Load(3).Arg(0).Field(info).Field(0x0A0002BE).Op(0x8E,0x69).Branch(0x3F,label);b.Load(0).Integer(0).Set(changed);b.Label(label+"done");
    }
    private static void N119_ShakeUpload(IL b,int info,int copies,int component)
    {
        b.Integer(0).Store(3).Branch(0x38,"uploadTest");b.Label("upload");N119_MeshField(b,info,3,0x0A00036F);b.Arg(0).Field(copies).Load(3).Op(0x9A).Virtual(0x0A000387);b.Load(0).Field(component);N119_MeshField(b,info,3,0x0A00036F);b.Load(3).Virtual(0x0A000388);b.Load(3).Integer(1).Op(0x58).Store(3);b.Label("uploadTest").Load(3).Arg(0).Field(info).Field(0x0A0002BE).Op(0x8E,0x69).Branch(0x3F,"upload");
    }
    private static byte[] N119_ShakeA()
    {
        var b=new IL();b.Arg(0).Field(0x04000A44).Store(0);b.Arg(0).Field(0x04000A42).Store(1);b.Load(1).Branch(0x39,"initial");b.Load(1).Integer(1).Branch(0x3B,"resume");b.Load(1).Integer(2).Branch(0x40,"false");
        b.Label("resume").Arg(0).Integer(-1).Set(0x04000A42).Branch(0x38,"changed");b.Label("initial").Arg(0).Integer(-1).Set(0x04000A42);b.Load(0).Field(0x04000A40).Integer(0).Integer(0).Virtual(0x0A000338);b.Arg(0).Load(0).Field(0x04000A40).Virtual(0x0A0002B8).Set(0x04000A45);b.Arg(0).Integer(0).Ref(0x8D,0x1B00008E).Set(0x04000A46);b.Load(0).Integer(1).Set(0x04000A41);
        b.Label("changed");N119_ShakeRefresh(b,0x04000A45,0x04000A46,0x04000A41,"refresh");b.Arg(0).Field(0x04000A45).Field(0x0A000339).Branch(0x39,"empty");b.Arg(0).Field(0x04000A45).Field(0x0A00038D).Store(2);b.Integer(0).Store(1).Branch(0x38,"lineTest");
        b.Label("line").Arg(0).Field(0x04000A45).Field(0x0A0002E7).Load(1).Ref(0x8F,0x01000079).Field(N119_LineStart).Store(4);b.Arg(0).Field(0x04000A45).Field(0x0A0002E7).Load(1).Ref(0x8F,0x01000079).Field(N119_LineEnd).Store(5);
        N119_Char(b,0x04000A45,4);b.Field(0x0A000375).Store(13);N119_Char(b,0x04000A45,5);b.Field(0x0A000342).Store(14);
        foreach(int axis in new[]{0x0A00001D,0x0A00008B,0x0A00009D})b.Address(14).Field(axis).Address(13).Field(axis).Op(0x58,0x6B).Float(.5f).Op(0x5A,0x6B);b.New(0x0A00009C).Store(8);
        b.Float(0).Float(0).Float(-.25f).Float(.25f).Call(0x0A000083).Load(0).Field(0x04000A3F).Op(0x5A,0x6B).Call(0x0A00020E).Store(12);b.Load(4).Store(15).Branch(0x38,"charTest");
        b.Label("char");N119_Char(b,0x04000A45,15);b.Field(0x0A00033D).Branch(0x39,"charNext");N119_Char(b,0x04000A45,15);b.Field(0x0A0002BC).Store(3);N119_Char(b,0x04000A45,15);b.Field(0x0A0002BD).Store(9);N119_MeshField(b,0x04000A45,3,0x0A00033E);b.Store(6);b.Arg(0).Field(0x04000A46).Load(3).Op(0x9A).Store(7);N119_QuadSubtract(b,6,7,9,8);
        b.Float(.995f).Load(0).Field(0x04000A3E).Float(.001f).Op(0x5A,0x6B,0x59,0x6B).Load(0).Field(0x04000A3E).Float(.001f).Op(0x5A,0x6B).Float(1.005f).Op(0x58,0x6B).Call(0x0A000083).Store(10);
        // Original PC uses Vector3.one as the TRS translation in this example.
        b.Float(1).Float(1).Float(1).New(0x0A00009C).Load(12).Load(10).Load(10).Load(10).New(0x0A00009C).Call(0x0A000348).Store(11);N119_QuadTransform(b,7,9,11,8);
        b.Label("charNext").Load(15).Integer(1).Op(0x58).Store(15);b.Label("charTest").Load(15).Load(5).Branch(0x3E,"char");b.Load(1).Integer(1).Op(0x58).Store(1);b.Label("lineTest").Load(1).Load(2).Branch(0x3F,"line");N119_ShakeUpload(b,0x04000A45,0x04000A46,0x04000A40);
        b.Arg(0).Float(.1f).New(0x0A00034B).Set(0x04000A43);b.Arg(0).Integer(2).Set(0x04000A42);b.Integer(1).Op(0x2A);b.Label("empty").Arg(0).Float(.25f).New(0x0A00034B).Set(0x04000A43);b.Arg(0).Integer(1).Set(0x04000A42);b.Integer(1).Op(0x2A);b.Label("false").Integer(0).Op(0x2A);return b.Finish();
    }
    private static byte[] N119_ShakeB()
    {
        var b=new IL();b.Arg(0).Field(0x04000A4E).Store(0);b.Arg(0).Field(0x04000A4C).Store(1);b.Load(1).Branch(0x39,"initial");b.Load(1).Integer(1).Branch(0x3B,"resume");b.Load(1).Integer(2).Branch(0x40,"false");
        b.Label("resume").Arg(0).Integer(-1).Set(0x04000A4C).Branch(0x38,"changed");b.Label("initial").Arg(0).Integer(-1).Set(0x04000A4C);b.Load(0).Field(0x04000A4A).Integer(0).Integer(0).Virtual(0x0A000338);b.Arg(0).Load(0).Field(0x04000A4A).Virtual(0x0A0002B8).Set(0x04000A4F);b.Arg(0).Integer(0).Ref(0x8D,0x1B00008E).Set(0x04000A50);b.Load(0).Integer(1).Set(0x04000A4B);
        b.Label("changed");N119_ShakeRefresh(b,0x04000A4F,0x04000A50,0x04000A4B,"refresh");b.Arg(0).Field(0x04000A4F).Field(0x0A000339).Branch(0x39,"empty");b.Arg(0).Field(0x04000A4F).Field(0x0A00038D).Store(2);b.Integer(0).Store(1).Branch(0x38,"lineTest");
        b.Label("line").Arg(0).Field(0x04000A4F).Field(0x0A0002E7).Load(1).Ref(0x8F,0x01000079).Field(N119_LineStart).Store(4);b.Arg(0).Field(0x04000A4F).Field(0x0A0002E7).Load(1).Ref(0x8F,0x01000079).Field(N119_LineEnd).Store(5);
        N119_Char(b,0x04000A4F,4);b.Field(0x0A000375).Store(13);N119_Char(b,0x04000A4F,5);b.Field(0x0A000342).Store(14);
        foreach(int axis in new[]{0x0A00001D,0x0A00008B,0x0A00009D})b.Address(14).Field(axis).Address(13).Field(axis).Op(0x58,0x6B).Float(.5f).Op(0x5A,0x6B);b.New(0x0A00009C).Store(8);
        b.Float(0).Float(0).Float(-.25f).Float(.25f).Call(0x0A000083).Call(0x0A00020E).Store(12);b.Load(4).Store(15).Branch(0x38,"charTest");
        b.Label("char");N119_Char(b,0x04000A4F,15);b.Field(0x0A00033D).Branch(0x39,"charNext");N119_Char(b,0x04000A4F,15);b.Field(0x0A0002BC).Store(3);N119_Char(b,0x04000A4F,15);b.Field(0x0A0002BD).Store(9);N119_MeshField(b,0x04000A4F,3,0x0A00033E);b.Store(6);b.Arg(0).Field(0x04000A50).Load(3).Op(0x9A).Store(7);// Glyph center uses all three coordinates, unlike the jitter example.
        b.Load(6).Load(9).Ref(0xA3,0x01000013).Store(13);b.Load(6).Load(9).Integer(2).Op(0x58).Ref(0xA3,0x01000013).Store(14);
        foreach(int axis in new[]{0x0A00001D,0x0A00008B,0x0A00009D})b.Address(14).Field(axis).Address(13).Field(axis).Op(0x58,0x6B).Float(.5f).Op(0x5A,0x6B);b.New(0x0A00009C).Store(16);
        N119_QuadSubtract(b,6,7,9,16);
        b.Float(.95f).Float(1.05f).Call(0x0A000083).Store(10);
        // Original PC uses Vector3.one as the TRS translation in this example.
        b.Float(1).Float(1).Float(1).New(0x0A00009C).Float(0).Float(0).Float(0).Float(1).New(N119_QuaternionCtor).Load(10).Load(10).Load(10).New(0x0A00009C).Call(0x0A000348).Store(11);N119_QuadTransform(b,7,9,11,16);
        N119_QuadSubtract(b,7,7,9,8);b.Float(1).Float(1).Float(1).New(0x0A00009C).Load(12).Float(1).Float(1).Float(1).New(0x0A00009C).Call(0x0A000348).Store(11);N119_QuadTransform(b,7,9,11,8);
        b.Label("charNext").Load(15).Integer(1).Op(0x58).Store(15);b.Label("charTest").Load(15).Load(5).Branch(0x3E,"char");b.Load(1).Integer(1).Op(0x58).Store(1);b.Label("lineTest").Load(1).Load(2).Branch(0x3F,"line");N119_ShakeUpload(b,0x04000A4F,0x04000A50,0x04000A4A);
        b.Arg(0).Float(.1f).New(0x0A00034B).Set(0x04000A4D);b.Arg(0).Integer(2).Set(0x04000A4C);b.Integer(1).Op(0x2A);b.Label("empty").Arg(0).Float(.25f).New(0x0A00034B).Set(0x04000A4D);b.Arg(0).Integer(1).Set(0x04000A4C);b.Integer(1).Op(0x2A);b.Label("false").Integer(0).Op(0x2A);return b.Finish();
    }
}
