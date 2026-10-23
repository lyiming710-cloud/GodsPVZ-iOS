internal static partial class Program
{
    private static byte[] N102_SelectZombieTypesBody()
    {
        var b = new IL();
        b.Arg(1).Branch(0x3A,"input"); b.New(0x0A000022).Op(0x7A);
        b.Label("input").Integer(0).Store(1);
        b.Label("mark").Arg(0).Field(0x04000207).Store(0);
        b.Load(1).Arg(1).Virtual(0x0A000144).Branch(0x3C,"pruneStart");
        b.Load(0).Arg(1).Load(1).Virtual(0x0A00006B).Virtual(0x0A00013C).Integer(1).Set(0x04000200);
        b.Load(1).Integer(1).Op(0x58).Store(1).Branch(0x38,"mark");
        b.Label("pruneStart").Integer(0).Store(2);
        b.Label("prune").Load(2).Load(0).Virtual(0x0A000146).Branch(0x3C,"return");
        b.Arg(0).Field(0x04000207).Load(2).Virtual(0x0A00013C).Field(0x04000200).Branch(0x3A,"next");
        b.Arg(0).Field(0x04000207).Load(2).Virtual(0x0A000145);
        b.Load(2).Integer(1).Op(0x59).Store(2);
        b.Label("next").Arg(0).Field(0x04000207).Store(0);
        b.Load(2).Integer(1).Op(0x58).Store(2).Branch(0x38,"prune");
        b.Label("return").Op(0x2A);
        return b.Finish();
    }

    private static byte[] N102_RectIntersectionBody()
    {
        // V0 world position; V1 index; V2..5 four independently read rects; V6/7 left/bottom.
        var b = new IL();
        b.Arg(0).Field(0x040000DD).Op(0x14).Call(0x0A000012).Branch(0x3A,"false");
        b.Arg(0).Field(0x040000DD).Virtual(0x0A000037).Store(0); b.Integer(0).Store(1);
        b.Label("loop").Load(1).Arg(0).Field(0x040000E3).Virtual(RectListCount).Branch(0x3C,"false");
        for(int local=2;local<=5;local++) b.Arg(0).Field(0x040000E3).Load(1).Virtual(RectListItem).Store(local);
        b.Address(2).Call(RectXMin).Address(0).Field(0x0A00001D).Op(0x58).Store(6);
        b.Address(3).Call(RectYMin).Address(0).Field(0x0A00008B).Op(0x58).Store(7);
        b.ArgAddress(1).Call(RectXMin).ArgAddress(1).Call(RectWidth).Op(0x58).Load(6).Branch(0x43,"next");
        b.Load(6).Address(4).Call(RectWidth).Op(0x58).ArgAddress(1).Call(RectXMin).Branch(0x43,"next");
        b.ArgAddress(1).Call(RectYMin).ArgAddress(1).Call(RectHeight).Op(0x58).Load(7).Branch(0x43,"next");
        b.Load(7).Address(5).Call(RectHeight).Op(0x58).ArgAddress(1).Call(RectYMin).Branch(0x3D,"true");
        b.Label("next").Load(1).Integer(1).Op(0x58).Store(1).Branch(0x38,"loop");
        b.Label("true").Integer(1).Op(0x2A); b.Label("false").Integer(0).Op(0x2A);
        return b.Finish();
    }

    private static byte[] N102_CircleIntersectionBody()
    {
        // V0 index, V1 center, V2 optional offset, V3 distance, V4..6 delta components.
        var b = new IL(); b.Integer(0).Store(0);
        b.Label("loop").Load(0).Arg(0).Field(0x040000E4).Virtual(RadiusListCount).Branch(0x3C,"false");
        b.Arg(0).Field(0x040000DD).Op(0x14).Call(0x0A000012).Branch(0x3A,"false");
        b.Arg(0).Field(0x040000DD).Virtual(0x0A000037).Store(1);
        b.Arg(0).Field(0x040000E5).Virtual(CenterListCount).Load(0).Branch(0x3E,"distance");
        b.Arg(0).Field(0x040000E5).Load(0).Virtual(0x0A000099).Store(2);
        foreach(int f in new[]{0x0A00001D,0x0A00008B,0x0A00009D})
            b.Address(1).Address(1).Field(f).Address(2).Field(f).Op(0x58).Set(f);
        b.Label("distance");
        N102_EmitDistance(b, x=>x.ArgAddress(1), x=>x.Address(1), 4,5,6,3);
        b.Arg(0).Field(0x040000E4).Load(0).Virtual(0x0A000098).Load(3).Branch(0x3C,"true");
        b.Load(0).Integer(1).Op(0x58).Store(0).Branch(0x38,"loop");
        b.Label("true").Integer(1).Op(0x2A); b.Label("false").Integer(0).Op(0x2A);
        return b.Finish();
    }

    private static void N102_EmitDistance(IL b, Action<IL> a, Action<IL> c, int dx,int dy,int dz,int result)
    {
        foreach(var v in new[]{(Field:0x0A00001D,Local:dx),(Field:0x0A00008B,Local:dy),(Field:0x0A00009D,Local:dz)})
        { a(b); b.Field(v.Field); c(b); b.Field(v.Field).Op(0x59).Store(v.Local); }
        // Original private helper sums y*y + x*x, then z*z, in float; sqrt is double, cast back.
        b.Load(dy).Load(dy).Op(0x5A).Load(dx).Load(dx).Op(0x5A,0x58)
            .Load(dz).Load(dz).Op(0x5A,0x58,0x6C).Call(0x0A00009F).Op(0x6B).Store(result);
    }

    private static byte[] N102_ZombieUpdateRateBody()
    {
        var b = new IL(); b.Float(1f).Store(0);
        b.Arg(0).Field(0x0400061B).Float(0f).Branch(0x3D,"zero");
        b.Arg(0).Field(0x040005F4).Integer(0).Virtual(0x06000127).Store(1);
        b.Load(1).Branch(0x39,"compare");
        b.Load(1).Field(0x04000147).Float(0f).Branch(0x43,"compare");
        b.Load(1).Field(0x04000149).Float(0f).Branch(0x3D,"zero");
        b.Load(1).Field(0x04000147).Float(1000f).Op(0x5B).Call(0x0A000050).Float(0.05f).Op(0x5A).Store(0);
        b.Float(0.05f).Float(1f).Load(0).Op(0x59).Call(0x0A0001AC).Store(0).Branch(0x38,"compare");
        b.Label("zero").Float(0f).Store(0);
        b.Label("compare").Arg(0).Field(0x0400061D).Load(0).Branch(0x3B,"return");
        b.Arg(0).Load(0).Call(0x06000474);
        b.Label("return").Op(0x2A); return b.Finish();
    }

    private static byte[] N102_SmoothAnglesBody()
    {
        var b = new IL();
        b.Arg(1).Float(0f).Branch(0x3B,"b"); b.Arg(2).Float(0f).Branch(0x3B,"a");
        b.Arg(1).Call(0x0A0001CE).Store(2); b.Arg(2).Call(0x0A0001CE).Store(3);
        b.Arg(2).Arg(1).Op(0x59).Call(0x0A0001CE).Store(4);
        N102_EmitSseMax(b,2,3,5,"abs");
        b.Load(5).Float(0.000001f).Op(0x5A).Store(5);
        b.Static(0x0A000219).Float(8f).Op(0x5A).Store(6);
        N102_EmitSseMax(b,5,6,6,"tolerance"); b.Load(6).Load(4).Branch(0x3D,"a");
        b.Load(3).Load(2).Branch(0x3D,"numeratorA"); b.Arg(2).Store(7).Branch(0x38,"divide");
        b.Label("numeratorA").Arg(1).Store(7);
        b.Label("divide").Load(7).Arg(1).Arg(2).Op(0x58,0x5B).Store(7);
        b.Float(0f).Load(7).Branch(0x3D,"clamp0"); b.Load(7).Float(1f).Branch(0x3D,"clamp1");
        b.Branch(0x38,"blend"); b.Label("clamp0").Float(0f).Store(7).Branch(0x38,"blend");
        b.Label("clamp1").Float(1f).Store(7);
        b.Label("blend").Arg(2).Arg(1).Op(0x59).Load(7).Op(0x5A).Arg(1).Op(0x58,0x2A);
        b.Label("a").Arg(1).Op(0x2A); b.Label("b").Arg(2).Op(0x2A);
        return b.Finish();
    }
    private static void N102_EmitSseMax(IL b,int a,int c,int dst,string label)
    {
        b.Load(a).Load(c).Branch(0x3D,label+"First"); b.Load(c).Store(dst).Branch(0x38,label+"End");
        b.Label(label+"First").Load(a).Store(dst); b.Label(label+"End");
    }

    private static byte[] N102_EnemyWaveBody()
    {
        var b = new IL(); b.Arg(1).Branch(0x39,"zero");
        b.Arg(1).Integer(1).Op(0x59).Integer(10).Op(0x5B).Store(0);
        b.Arg(1).Integer(1).Op(0x59).Integer(10).Op(0x5D).Store(1).Branch(0x38,"lookup");
        b.Label("zero").Integer(-1).Store(0); b.Integer(-1).Store(1);
        b.Label("lookup").Arg(0).Field(0x0400020B).Virtual(FlagListCount).Load(0).Branch(0x3E,"null");
        b.Arg(0).Field(0x0400020B).Load(0).Virtual(0x0A00013D).Field(0x040001F3)
            .Virtual(WaveListCount).Load(1).Branch(0x3E,"null");
        b.Arg(0).Field(0x0400020B).Load(0).Virtual(0x0A00013D).Field(0x040001F3).Load(1).Virtual(0x0A00013F).Op(0x2A);
        b.Label("null").Op(0x14,0x2A); return b.Finish();
    }

    private static byte[] N102_UpdateBlockBody()
    {
        var b = new IL(); b.Arg(0).Field(0x040004DD).Virtual(0x0A0000DD).Integer(0).Branch(0x3E,"return");
        b.Arg(0).Field(0x040004DD).Virtual(0x0A0000DD).Integer(1).Op(0x59).Store(0);
        b.Label("loop").Load(0).Integer(0).Branch(0x3F,"return");
        b.Load(0).Arg(0).Field(0x040004DD).Virtual(0x0A0000DD).Branch(0x3C,"return");
        b.Arg(0).Field(0x040004DD).Load(0).Virtual(0x0A00006C).Store(1);
        b.Load(1).Op(0x14).Call(0x0A000012).Branch(0x3A,"remove");
        b.Load(1).Virtual(0x06000464).Branch(0x39,"next");
        b.Arg(0).Load(1).Integer(0).Call(0x06000358).Branch(0x38,"next");
        b.Label("remove").Arg(0).Field(0x040004DD).Load(1).Virtual(0x0A000225).Op(0x26);
        b.Label("next").Load(0).Integer(1).Op(0x59).Store(0).Branch(0x38,"loop");
        b.Label("return").Op(0x2A); return b.Finish();
    }

    private static byte[] N102_SkillRunningBody()
    {
        var b = new IL(); b.Arg(0).Field(0x040004D1).Arg(0).Virtual(0x060004DF);
        b.Arg(0).Field(0x040004D1).Field(0x04000696).Branch(0x3A,"return");
        b.Arg(0).Field(0x040004D1).Field(0x04000691).Integer(2).Branch(0x40,"rate");
        b.Arg(0).Call(0x06000366).Store(0).Branch(0x38,"time");
        b.Label("rate").Arg(0).Field(0x040004C9).Store(0);
        b.Label("time").Call(0x0A00001A).Load(0).Op(0x5A).Store(1);
        b.Arg(0).Arg(0).Field(0x040004D3).Load(1).Op(0x59).Set(0x040004D3);
        b.Arg(0).Field(0x040004D3).Float(0f).Branch(0x42,"return");
        b.Arg(0).Field(0x040004D1).Arg(0).Op(0x14,0x14,0x14).Virtual(0x060004E4);
        b.Arg(0).Field(0x040004B1).Integer(49).Branch(0x40,"return");
        b.Arg(0).Call(0x06000392);
        b.Arg(0).Field(0x040004D1).Field(0x0400069A).Integer(1).Branch(0x3C,"return");
        b.Arg(0).Field(0x040004FF).Ref(0x72,0x7000CD66).Virtual(0x06000107).Store(2);
        b.Load(2).Branch(0x39,"return"); b.Load(2).Float(0f).Set(0x04000110);
        b.Label("return").Op(0x2A); return b.Finish();
    }

    private static byte[] N102_PropCoolingBody()
    {
        var b = new IL(); b.Arg(0).Field(0x0400057E).Store(0);
        b.Call(0x0A00001A).Load(0).Op(0x58).Store(1);
        b.Arg(0).Field(0x04000582).Store(3); b.Arg(0).Load(1).Set(0x0400057E);
        b.Load(3).Arg(0).Field(0x0400057D).Load(1).Op(0x59).Arg(0).Field(0x0400057D).Op(0x5B).Virtual(0x0A0000F0);
        b.Arg(0).Field(0x04000583).Float(1f).Arg(0).Field(0x04000582).Virtual(0x0A000299).Op(0x59).Virtual(0x0A0000F0);
        b.Arg(0).Field(0x0400057D).Arg(0).Field(0x0400057E).Op(0x59).Store(2);
        b.Arg(0).Field(0x04000584).Address(2).Ref(0x72,0x7000B8C9).Call(0x0A000125).Virtual(0x0A000062);
        b.Arg(0).Field(0x0400057E).Arg(0).Field(0x0400057D).Branch(0x44,"return");
        b.Arg(0).Integer(1).Set(0x04000579); b.Arg(0).Call(0x06000406);
        b.Arg(0).Arg(0).Field(0x04000581).Virtual(0x0A00002E).Virtual(0x0A00020D).Set(0x04000587);
        b.Label("return").Op(0x2A); return b.Finish();
    }

    private static byte[] N102_DeviceLayerBody()
    {
        var b = new IL(); b.Arg(0).Field(0x04000489).Op(0x14).Call(0x0A000012).Branch(0x39,"name");
        b.Arg(0).Arg(0).Call(0x0A000009).Virtual(0x2B000057).Set(0x04000489);
        b.Label("name").Arg(0).Field(0x04000489).Ref(0x72,0x70001E24).Virtual(0x0A00015A);
        b.Arg(0).Field(0x04000467).Float(-10f).Op(0x5A).Store(0); N102_EmitCvtt(b,0,"y"); b.Store(1);
        b.Arg(0).Field(0x04000473).Integer(0).Branch(0x3B,"below");
        b.Arg(0).Field(0x04000473).Integer(1).Branch(0x3B,"above");
        b.Arg(0).Field(0x04000473).Integer(3).Branch(0x40,"set");
        b.Load(1).Integer(10).Op(0x58).Store(1).Branch(0x38,"set");
        b.Label("below").Arg(0).Field(0x0400046B).Float(10f).Op(0x5A).Float(-0.5f).Op(0x5A).Store(0).Branch(0x38,"depth");
        b.Label("above").Arg(0).Field(0x0400046B).Float(10f).Op(0x5A).Float(0.5f).Op(0x5A).Store(0);
        b.Label("depth"); N102_EmitCvtt(b,0,"d"); b.Load(1).Op(0x58).Store(1);
        b.Label("set").Arg(0).Field(0x04000489).Load(1).Virtual(0x0A00015B).Op(0x2A);
        return b.Finish();
    }
    private static void N102_EmitCvtt(IL b,int local,string label)
    {
        b.Load(local).Float(2147483648f).Branch(0x41,label+"Invalid");
        b.Load(local).Float(-2147483648f).Branch(0x3F,label+"Invalid");
        b.Load(local).Op(0x69).Branch(0x38,label+"Done");
        b.Label(label+"Invalid").Integer(int.MinValue); b.Label(label+"Done");
    }

    private static byte[] N102_ChangeTypeBody()=>N102_ChangeEnumBody(0x0200008F,0x04000691);
    private static byte[] N102_ChangeChargeBody()=>N102_ChangeEnumBody(0x0200008E,0x04000690);
    private static byte[] N102_ChangeTriggerBody()=>N102_ChangeEnumBody(0x02000090,0x04000692);
    private static byte[] N102_ChangeEnumBody(int type,int field)
    {
        var b = new IL(); b.Arg(0).Field(0x04000089).Branch(0x39,"return");
        b.Arg(1).Integer(1).Op(0x59).Store(0); b.Integer(-1).Store(1);
        b.Ref(0xD0,type).Call(0x0A00004C).Load(0).Ref(0x8C,0x0100000E).Call(0x0A00004D).Branch(0x3A,"valid");
        b.Ref(0x72,0x70000E8D).Call(0x0A000010).Branch(0x38,"set");
        b.Label("valid").Load(0).Store(1);
        b.Label("set").Arg(0).Field(0x04000089).Load(1).Set(field);
        b.Label("return").Op(0x2A); return b.Finish();
    }

    private static byte[] N102_PoleCommanderBody()
    {
        var b = new IL(); b.Arg(0).Field(0x040005DE).Branch(0x39,"jumpTest");
        b.Arg(0).Field(0x040005D4).Branch(0x3A,"return");
        b.Arg(0).Field(0x0400061A).Ref(0x72,0x7000FCEF).Virtual(0x06000106).Store(0);
        b.Load(0).Branch(0x39,"return"); b.Load(0).Ref(0x75,0x0200001D).Store(1);
        b.Load(1).Branch(0x39,"jumpTest");
        b.Load(1).Field(0x04000110).Float(1.5f).Branch(0x41,"resetSpeed");
        b.Load(1).Field(0x04000110).Store(2);
        b.Load(1).Call(0x0A00001A).Float(0.2f).Op(0x5A).Arg(0).Field(0x0400061D).Op(0x5A).Load(2).Op(0x58).Set(0x04000110);
        b.Label("resetSpeed").Arg(0).Call(0x06000472);
        b.Label("jumpTest").Arg(0).Call(0x060004A3).Branch(0x39,"return");
        b.Arg(0).Field(0x040005FE).Arg(0).Integer(1).Set(0x040005DF).Ref(0x72,0x7000FD97).Virtual(0x0A000087);
        b.Label("return").Op(0x2A); return b.Finish();
    }

    private static byte[] N102_DropLootBody()
    {
        var b = new IL(); b.Arg(0).Field(0x040001A6).Integer(50).Branch(0x3B,"special");
        b.Arg(0).Field(0x040001A6).Integer(51).Branch(0x3B,"special");
        b.Arg(1).Float(0.01f).Op(0x5A).Float(1f).Op(0x58).Arg(0).Field(0x040001A8).Op(0x5A).Float(1f).Call(0x0A00011F).Store(0);
        b.Integer(0).Store(1); b.Integer(0).Store(2);
        b.Arg(0).Field(0x040001A9).Integer(0).Branch(0x3E,"null");
        b.Label("loop").Call(0x0A000120).Store(3); b.Load(0).Load(3).Branch(0x44,"next");
        b.Load(1).Arg(0).Field(0x040001A7).Op(0x58).Store(1);
        b.Load(1).Arg(0).Field(0x040001AA).Branch(0x3C,"finish");
        b.Label("next").Load(2).Integer(1).Op(0x58).Store(2);
        b.Load(2).Arg(0).Field(0x040001A9).Branch(0x3F,"loop");
        b.Label("finish").Load(1).Branch(0x39,"null");
        b.Arg(0).Field(0x040001A6).Call(0x06000217).Store(4); b.Load(4).Load(1).Set(0x0400067A); b.Load(4).Op(0x2A);
        b.Label("special").Arg(0).Field(0x040001A6).Call(0x06000217).Store(4);
        b.Load(4).Integer(1).Set(0x0400067A); b.Load(4).Arg(0).Field(0x040001AC).Set(0x0400067B); b.Load(4).Op(0x2A);
        b.Label("null").Op(0x14,0x2A); return b.Finish();
    }

    private static byte[] N102_FindGridBody()
    {
        var b = new IL();
        b.Arg(0).Field(0x040000A6).Virtual(0x06000158).Field(0x0400037D).Virtual(0x0A000147).Arg(2).Branch(0x3E,"outside");
        b.Arg(0).Field(0x040000A6).Virtual(0x06000158).Field(0x0400037D).Arg(2).Virtual(0x0A000065).Field(0x04000381)
            .Virtual(0x0A0001B8).Arg(1).Branch(0x3E,"outside");
        b.Arg(0).Field(0x040000A6).Virtual(0x06000158).Field(0x0400037D).Arg(2).Virtual(0x0A000065).Field(0x04000381)
            .Arg(1).Virtual(0x0A000078).Op(0x2A);
        b.Label("outside").Ref(0x72,0x70001913).Call(0x0A000010).Op(0x14,0x2A); return b.Finish();
    }

    private static byte[] N102_SkillUpSuppliesBody()
    {
        var b = new IL(); b.Arg(0).Field(0x04000318).Branch(0x39,"empty");
        b.Arg(0).Field(0x04000318).Virtual(SkillUpListCount).Arg(1).Branch(0x3E,"empty");
        N102_EmitSupplyArray(b); b.Arg(2).Integer(1).Op(0x59,0x9A).Branch(0x39,"empty");
        N102_EmitSupplyArray(b); b.Arg(2).Integer(1).Op(0x59,0x9A).Field(0x04000332).Op(0x2A);
        b.Label("empty").New(0x0A00011E).Op(0x2A); return b.Finish();
    }
    private static void N102_EmitSupplyArray(IL b)
    { b.Arg(0).Field(0x04000318).Arg(1).Virtual(0x0A0001A8).Field(0x04000331); }

    private static byte[] N102_ZombieDefenseBody()
    {
        var b = new IL(); b.Float(0f).Store(0);
        b.Arg(1).Integer(0).Branch(0x3B,"health"); b.Arg(1).Integer(1).Branch(0x3B,"armor1");
        b.Arg(1).Integer(2).Branch(0x40,"buff"); N102_EmitDefense(b,0x040005C9,0x040005CA,0x040005CC); b.Branch(0x38,"buff");
        b.Label("armor1"); N102_EmitDefense(b,0x040005C3,0x040005C4,0x040005C6); b.Branch(0x38,"buff");
        b.Label("health"); N102_EmitDefense(b,0x040005BD,0x040005BE,0x040005BF);
        b.Label("buff").Arg(0).Field(0x0400061A).Load(0).Ref(0x72,0x7000450D).Virtual(0x06000104)
            .Load(0).Op(0x58).Store(1);
        N102_EmitNativeRound(b,1,2);
        b.Float(0f).Call(0x0A0001AC).Op(0x2A);
        return b.Finish();
    }
    private static void N102_EmitDefense(IL b,int current,int maximum,int defense)
    { b.Arg(0).Field(current).Arg(0).Field(maximum).Op(0x5B).Arg(0).Field(defense).Op(0x5A).Store(0); }

    private static void N102_EmitNativeRound(IL b,int value,int integral)
    {
        // Original 180003010 is not universally MathF.Round: the non-half path adds/subtracts
        // 0.5 in float BEFORE Floor/Ceiling, including large representable odd integers.
        b.Load(value).Float(0f).Branch(0x44,"roundNegative");
        b.Load(value).Op(0x6C).Call(0x0A000283).Store(integral);
        b.Load(value).Op(0x6C).Load(integral).Op(0x59).Float(0.5f).Op(0x6C).Branch(0x40,"roundPositiveOther");
        b.Load(integral).Op(0x6A).Integer(1).Op(0x6A,0x5F).Branch(0x39,"roundEven");
        b.Load(integral).Op(0x6B).Float(1f).Op(0x58).Branch(0x38,"roundDone");
        b.Label("roundPositiveOther").Load(value).Float(0.5f).Op(0x58).Store(value);
        b.Load(value).Call(0x0A00004F).Branch(0x38,"roundDone");
        b.Label("roundNegative").Load(value).Op(0x6C).Call(0x0A000283).Store(integral);
        b.Load(value).Op(0x6C).Load(integral).Op(0x59).Float(-0.5f).Op(0x6C).Branch(0x40,"roundNegativeOther");
        b.Load(integral).Op(0x6A).Integer(1).Op(0x6A,0x5F).Branch(0x39,"roundEven");
        b.Load(integral).Op(0x6B).Float(1f).Op(0x59).Branch(0x38,"roundDone");
        b.Label("roundNegativeOther").Load(value).Float(0.5f).Op(0x59).Store(value);
        b.Load(value).Call(0x0A000050).Branch(0x38,"roundDone");
        b.Label("roundEven").Load(integral).Op(0x6B);
        b.Label("roundDone");
    }

    private static byte[] N102_TouchSunBody()=>N102_TouchBody(0x04000343,false);
    private static byte[] N102_TouchProjectBody()=>N102_TouchBody(0x040002C2,true);
    private static byte[] N102_TouchBody(int list,bool stopAtSpecial)
    {
        var b = new IL(); b.Integer(0).Store(0);
        b.Label("loop").Load(0).Arg(0).Field(list).Virtual(ProjectListCount).Branch(0x3C,"null");
        b.Arg(0).Field(list).Load(0).Virtual(0x0A000155).Store(1);
        b.Load(1).Field(0x04000520).Integer(4).Branch(0x3B,stopAtSpecial?"null":"next");
        if(stopAtSpecial)b.Load(1).Field(0x04000520).Integer(6).Branch(0x3B,"null");
        b.Load(1).Virtual(0x0A000001).Virtual(0x0A000037).Store(2);
        N102_EmitDistance(b,x=>x.Address(2),x=>x.ArgAddress(1),4,5,6,3);
        b.Load(1).Field(0x04000519).Load(3).Branch(0x3C,"found");
        b.Label("next").Load(0).Integer(1).Op(0x58).Store(0).Branch(0x38,"loop");
        b.Label("found").Load(1).Op(0x2A); b.Label("null").Op(0x14,0x2A); return b.Finish();
    }
}
