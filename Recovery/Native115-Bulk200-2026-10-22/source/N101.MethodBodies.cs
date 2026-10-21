internal static partial class Program
{
    private static byte[] N101_MinNodeBody()
    {
        var b = new IL();
        b.Arg(0).Virtual(0x0A000407).Integer(0).Branch(0x3E, "null");
        b.Integer(int.MaxValue).Store(13); b.Integer(int.MaxValue).Store(12); b.Integer(0).Store(3);
        b.Label("loop").Load(3).Arg(0).Virtual(0x0A000407).Branch(0x3C, "end");
        b.Arg(0).Load(3).Virtual(0x0A0001B0).Field(0x040003A2).Load(13).Branch(0x3C, "next");
        // Preserve the second indexer read on an improving item.
        b.Load(3).Store(12); b.Arg(0).Load(3).Virtual(0x0A0001B0).Field(0x040003A2).Store(13);
        b.Label("next").Load(3).Integer(1).Op(0x58).Store(3).Branch(0x38, "loop");
        b.Label("end").Load(12).Arg(0).Virtual(0x0A000407).Branch(0x3C, "null");
        b.Arg(0).Load(12).Virtual(0x0A0001B0).Op(0x2A);
        b.Label("null").Op(0x14, 0x2A);
        return b.Finish();
    }

    private static byte[] N101_ResetPassBody()
    {
        // New locals: Save V0, int V1. Do not change the old shared local blob.
        var b = new IL();
        b.Static(0x0400015E).Field(0x04000166).Field(0x04000310).Store(0);
        b.Integer(0).Store(1);
        b.Label("rescue").Load(1).Load(0).Field(0x04000633).Op(0x8E, 0x69).Branch(0x3C, "second");
        b.Load(0).Field(0x04000633).Load(1).Integer(0).Op(0x9E);
        b.Load(1).Integer(1).Op(0x58).Store(1).Branch(0x38, "rescue");
        b.Label("second").Integer(0).Store(1);
        b.Label("minigame").Load(1).Load(0).Field(0x04000634).Op(0x8E, 0x69).Branch(0x3C, "return");
        b.Load(0).Field(0x04000634).Load(1).Integer(0).Op(0x9E);
        b.Load(1).Integer(1).Op(0x58).Store(1).Branch(0x38, "minigame");
        b.Label("return").Op(0x2A);
        return b.Finish();
    }

    private static byte[] N101_UpdateSkillBody()
    {
        // New locals: int V0, SkillUpdata V1. dictionary_Updata is Dictionary<int,SkillUpdata>.
        var b = new IL(); b.Integer(0).Store(0);
        b.Arg(1).Call(0x0A00000B).Branch(0x39, "lookup");
        b.Arg(1).Field(0x040004B1).Integer(3).Op(0x5A).Arg(0).Field(0x0400068E).Op(0x58).Store(0);
        b.Label("lookup").Static(0x040006A0).Load(0).Address(1).Virtual(SkillTryGetValue).Op(0x26);
        b.Load(1).Branch(0x39, "return");
        b.Load(1).Arg(1).Virtual(0x060004EE);
        b.Label("return").Op(0x2A);
        return b.Finish();
    }

    private static byte[] N101_GetIntParameterBody()
    {
        var b = new IL();
        b.Arg(0).Field(0x040004FE).Store(2);
        b.Label("loop").Load(2).Virtual(0x0A000144).Arg(1).Branch(0x3C, "get");
        b.Arg(0).Field(0x040004FE).Integer(0).Virtual(0x0A000291);
        b.Arg(0).Field(0x040004FE).Store(2).Branch(0x38, "loop");
        // The original uses Count < index, not Count <= index. Retain its boundary exception.
        b.Label("get").Arg(0).Field(0x040004FE).Arg(1).Virtual(0x0A00006B).Op(0x2A);
        return b.Finish();
    }

    private static byte[] N101_EndDeviceEventBody()
    {
        var b = new IL();
        b.Arg(0).Ref(0x75, 0x02000028).Store(7);
        b.Load(7).Branch(0x39, "append"); b.Load(7).Call(0x06000118);
        b.Label("append").Arg(0).Field(0x0400013D).Field(0x04000484).Arg(0).Virtual(0x0A000412).Op(0x2A);
        return b.Finish();
    }

    private static byte[] N101_CameraPositionBody()
    {
        var b = new IL();
        b.Call(0x0A000004).Virtual(0x0A000170).Store(3);
        foreach (int component in new[] { 0x0A00001D, 0x0A00008B, 0x0A00009D })
            b.Address(0).ArgAddress(1).Field(component).Load(3).Op(0x5A).Float(540f).Op(0x5B).Set(component);
        // Preserve three separate Camera.main reads and two position reads; no receiver caching.
        b.Address(0).Address(0).Field(0x0A00001D)
            .Call(0x0A000004).Virtual(0x0A000001).Virtual(0x0A000037).Store(16);
        b.Address(16).Field(0x0A00001D).Op(0x58).Set(0x0A00001D);
        b.Address(0).Address(0).Field(0x0A00008B)
            .Call(0x0A000004).Virtual(0x0A000001).Virtual(0x0A000037).Store(18);
        b.Address(18).Field(0x0A00008B).Op(0x58).Set(0x0A00008B);
        b.Load(0).Op(0x2A);
        return b.Finish();
    }

    private static byte[] N101_GridPassableBody()
    {
        var b = new IL();
        b.Arg(0).Field(0x0400037D).Branch(0x39, "zero");
        b.Arg(0).Field(0x0400037D).Store(3);
        b.Load(3).Virtual(0x0A000147).Branch(0x39, "zero");
        b.Arg(2).Load(3).Virtual(0x0A000147).Branch(0x3C, "zero");
        b.Arg(2).Integer(0).Branch(0x3F, "zero");
        // The x bound is checked using row zero, even when another row is requested.
        b.Load(3).Integer(0).Virtual(0x0A000065).Field(0x04000381).Store(5);
        b.Arg(1).Load(5).Virtual(0x0A0001B8).Branch(0x3C, "zero");
        b.Arg(1).Integer(0).Branch(0x3F, "zero");
        b.Arg(0).Field(0x0400037D).Arg(2).Virtual(0x0A000065).Field(0x04000381)
            .Arg(1).Virtual(0x0A000078).Virtual(0x06000295).Op(0x2A);
        b.Label("zero").Integer(0).Op(0x2A);
        return b.Finish();
    }

    private static byte[] N101_HighlightPlantBody()
    {
        // New local: Plant V0. Native jump table has only hand types 5 and 6 active.
        var b = new IL(); b.Op(0x14).Store(0);
        b.Arg(0).Field(0x04000233).Integer(5).Branch(0x3B, "seek");
        b.Arg(0).Field(0x04000233).Integer(6).Branch(0x40, "highlight");
        b.Arg(0).Field(0x04000243).Op(0x14).Call(0x0A000012).Branch(0x39, "highlight");
        b.Label("seek").Arg(0).Call(0x060001DE).Store(0);
        b.Label("highlight").Load(0).Op(0x14).Call(0x0A00000C).Branch(0x39, "return");
        b.Load(0).Virtual(0x060003A1);
        b.Label("return").Op(0x2A);
        return b.Finish();
    }

    private static byte[] N101_ResetSnowSpeedBody()
    {
        var b = new IL();
        b.Arg(0).Call(0x0A0000FC).Set(0x040005FC);
        b.Arg(0).Call(0x0A0000FC).Set(0x040005FB);
        b.Arg(0).Field(0x0400061A).Ref(0x72, 0x7000FD17).Virtual(0x06000106)
            .Ref(0x75, 0x0200001D).Op(0x25).Branch(0x39, "empty");
        b.Float(-0.7f).Set(0x04000110).Op(0x2A);
        b.Label("empty").Op(0x26, 0x2A);
        return b.Finish();
    }

    private static byte[] N101_UnlockedSkillsBody()
    {
        var b = new IL(); b.Integer(0).Store(3);
        foreach (int skill in new[] { 0x0400066E, 0x0400066F, 0x04000670 })
            b.Load(3).Arg(0).Field(skill).Field(0x0400068E).Integer(0).Op(0xFE, 0x02)
                .Integer(0).Op(0xFE, 0x01, 0x58).Store(3);
        b.Arg(0).Field(0x0400065D).Store(19);
        foreach (int stars in new[] { 2, 3, 4, 5, 6 })
            b.Load(19).Integer(stars).Branch(0x3B, "stars" + stars);
        b.Integer(0).Op(0x2A);
        b.Label("stars2").Load(3).Integer(1).Branch(0x40, "zero");
        b.Arg(1).Integer(2).Branch(0x3F, "zero"); b.Integer(1).Op(0x2A);
        b.Label("stars3").Load(3).Integer(2).Branch(0x40, "one");
        b.Arg(1).Integer(2).Branch(0x3F, "one"); b.Integer(2).Op(0x2A);
        b.Label("stars4").Arg(1).Integer(2).Branch(0x3F, "one"); b.Integer(2).Op(0x2A);
        b.Label("stars5").Integer(1).Store(2);
        b.Arg(1).Integer(2).Branch(0x3F, "checkThird5"); b.Integer(2).Store(2);
        b.Label("checkThird5").Load(3).Integer(3).Branch(0x40, "count");
        b.Arg(1).Integer(4).Branch(0x3F, "count"); b.Load(2).Integer(1).Op(0x58).Store(2);
        b.Branch(0x38, "count");
        b.Label("stars6").Integer(1).Store(2);
        b.Arg(1).Integer(2).Branch(0x3F, "checkThird6"); b.Integer(2).Store(2);
        b.Label("checkThird6").Arg(1).Integer(4).Branch(0x3F, "count");
        b.Load(2).Integer(1).Op(0x58).Store(2);
        b.Label("count").Load(2).Op(0x2A);
        b.Label("one").Integer(1).Op(0x2A);
        b.Label("zero").Integer(0).Op(0x2A);
        return b.Finish();
    }

    private static byte[] N101_PlantRangeBody() => N101_RangeBody(0x040004A4, 0x040004A5, 0x040004A8, 0x040004A9);
    private static byte[] N101_ZombieRangeBody() => N101_RangeBody(0x040005AD, 0x040005AE, 0x040005B1, 0x040005B2);
    private static byte[] N101_RangeBody(int fx, int fy, int fw, int fd)
    {
        // Fresh locals: int range, float left,bottom,width,depth,centerX,centerY.
        var b = new IL();
        b.Arg(1).Field(fw).Float(0.5f).Op(0x5A).Store(1);
        b.Arg(0).Field(0x040000E2).Store(0);
        b.Arg(1).Field(fx).Load(1).Op(0x59).Store(1);
        b.Arg(1).Field(fy).Arg(1).Field(fd).Float(0.5f).Op(0x5A, 0x59).Store(2);
        b.Arg(1).Field(fw).Store(3); b.Arg(1).Field(fd).Store(4);
        b.Arg(1).Field(fx).Store(5); b.Arg(1).Field(fy).Store(6);
        b.Load(0).Integer(1).Branch(0x3B, "rectOnly");
        b.Load(0).Integer(2).Branch(0x3B, "circle");
        b.Load(0).Integer(3).Branch(0x3B, "either");
        b.Load(0).Integer(4).Branch(0x3B, "true");
        b.Integer(0).Op(0x2A);
        b.Label("either"); N101_EmitRangeRect(b); b.Branch(0x3A, "true"); b.Branch(0x38, "circle");
        b.Label("rectOnly"); N101_EmitRangeRect(b); b.Op(0x2A);
        b.Label("circle").Arg(0).Load(5).Load(6).Float(0f).New(0x0A00009C).Call(0x060000D4).Op(0x2A);
        b.Label("true").Integer(1).Op(0x2A);
        return b.Finish();
    }
    private static void N101_EmitRangeRect(IL b)
    { b.Arg(0).Load(1).Load(2).Load(3).Load(4).New(0x0A00009B).Call(0x060000D7); }

    private static byte[] N101_PreviewSelecterBody()
    {
        var b = new IL(); b.New(0x060001AE).Store(0);
        b.Call(0x06000212).Call(0x06000154).Store(2);
        b.Load(2).Call(0x0A00000F).Branch(0x3A, "default");
        b.Load(2).Call(0x2B000084).Store(4);
        b.Arg(0).Field(0x04000781).Store(7);
        b.Load(4).Load(7).Field(0x04000197).Set(0x04000208);
        b.Load(4).Arg(0).Field(0x04000781).Field(0x04000196).Call(0x060001AD);
        b.Load(4).Op(0x2A);
        b.Label("default").Load(0).Op(0x2A);
        return b.Finish();
    }

    private static byte[] N101_TmpCounterBody()
    {
        var b = new IL(); b.Arg(0).Field(0x040009ED).Branch(0x3A, "return");
        b.Arg(0).Field(0x040009EE).Ref(0x72, 0x700218D8)
            .Arg(0).Field(0x040009F0).Integer(1000).Op(0x5D, 0x6B).Virtual(0x0A0002FE);
        // SetText may mutate count. Increment its post-call value, with Int32 wraparound.
        b.Arg(0).Arg(0).Field(0x040009F0).Integer(1).Op(0x58).Set(0x040009F0);
        b.Label("return").Op(0x2A);
        return b.Finish();
    }

    private static byte[] N101_SnowDepleteBody()
    {
        var b = new IL();
        b.Arg(1).Integer(0).Branch(0x3B, "regular");
        b.Arg(1).Integer(2).Branch(0x3B, "regular");
        b.Arg(1).Integer(1).Branch(0x3B, "small");
        b.Arg(1).Integer(3).Branch(0x3B, "all"); b.Branch(0x38, "rest");
        b.Label("regular").Float(0.75f).Store(5).Branch(0x38, "subtract");
        b.Label("small").Float(0.15f).Store(5).Branch(0x38, "subtract");
        b.Label("all").Float(9999999f).Store(5);
        b.Label("subtract").Arg(0).Arg(0).Field(0x040005DA).Load(5).Op(0x59).Set(0x040005DA);
        // Ordered <= 0: NaN must skip the rest transition, just as COMISS/JB does.
        b.Label("rest").Arg(0).Field(0x040005DA).Float(0f).Branch(0x42, "hp");
        b.Arg(0).Float(0f).Set(0x040005DA); b.Arg(0).Integer(1).Set(0x040005D9);
        b.Arg(0).Field(0x040005FE).Ref(0x72, 0x7000FD73).Integer(1).Virtual(0x0A0001F3);
        b.Arg(0).Float(0.02f).Call(0x06000484);
        b.Label("hp").Arg(0).Field(0x04000616).Call(0x0A00000B).Branch(0x39, "return");
        b.Arg(0).Field(0x04000616).Virtual(0x06000627);
        b.Label("return").Op(0x2A);
        return b.Finish();
    }

    private static byte[] N101_ProjectileMoveBody()
    {
        var b = new IL();
        b.Arg(0).FieldAddress(0x04000561).Field(0x0A00001D).Store(1);
        b.Arg(0).Field(0x04000552).Store(2);
        b.Call(0x0A00001A).Load(1).Op(0x5A).Load(2).Op(0x58).Store(3);
        b.Arg(0).Load(3).Set(0x04000552);
        b.Arg(0).Field(0x04000553).Store(2);
        b.Arg(0).FieldAddress(0x04000561).Field(0x0A00008B).Store(1);
        b.Call(0x0A00001A).Load(1).Op(0x5A).Load(2).Op(0x58).Store(3);
        b.Arg(0).Field(0x04000561).Store(8);
        b.Arg(0).FieldAddress(0x04000562).Field(0x0A00001D).Store(4);
        b.Arg(0).FieldAddress(0x04000562).Field(0x0A00008B).Store(5);
        b.Arg(0).FieldAddress(0x04000562).Field(0x0A00009D).Store(6);
        b.Arg(0).Load(3).Set(0x04000553);
        b.Call(0x0A00001A).Store(3);
        foreach (var component in new[] { (Field:0x0A00001D, Accel:4), (Field:0x0A00008B, Accel:5), (Field:0x0A00009D, Accel:6) })
            b.Address(8).Address(8).Field(component.Field).Load(component.Accel).Load(3).Op(0x5A, 0x58).Set(component.Field);
        b.Arg(0).Load(8).Set(0x04000561);
        b.Arg(0).Field(0x04000554).Store(2); b.Arg(0).Field(0x04000564).Store(1);
        b.Call(0x0A00001A).Load(1).Op(0x5A).Load(2).Op(0x58).Store(3);
        b.Arg(0).Field(0x04000565).Store(4); b.Arg(0).Field(0x04000564).Store(5);
        b.Arg(0).Load(3).Set(0x04000554);
        b.Call(0x0A00001A).Load(4).Op(0x5A).Load(5).Op(0x58).Store(3);
        b.Arg(0).Load(3).Set(0x04000564);
        b.Arg(0).Call(0x060003F1).Op(0x2A);
        return b.Finish();
    }
}
