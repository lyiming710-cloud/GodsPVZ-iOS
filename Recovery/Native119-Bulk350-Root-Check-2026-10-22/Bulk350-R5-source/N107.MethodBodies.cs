internal static partial class Program
{
    // Source proposal only: no patcher/candidate/fixture has been executed by the author.
    private static byte[] N107_PassLawnLevelBody()
    {
        var b=new IL();b.Arg(0).Field(0x04000166).Branch(0x39,"loot");
        b.Arg(0).Field(0x04000166).Arg(0).Field(0x04000167).Arg(0).Field(0x04000168).Arg(2).Virtual(0x06000248);
        b.Label("loot").Arg(1).Field(0x040008D6).Virtual(SuppliesEnumerator).Store(0);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(0).Call(SuppliesCurrent).Store(1);
        b.Arg(0).Field(0x04000166).Load(1).Field(0x04000590).Virtual(0x0600023D);
        b.Label("move").Address(0).Call(SuppliesMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(0).Call(0x0A00010C).Op(0xDC);
        b.Label("after").Arg(0).Field(0x04000166).Virtual(0x0600024B);
        b.Arg(0).Field(0x04000167).Integer(2).Branch(0x3B,"return");
        b.Static(0x0400015F).Field(0x04000169).Branch(0x39,"return");b.Static(0x0400015F).Op(0x14).Set(0x04000169);
        b.Label("return").Op(0x2A);RegisterFinally(0x0600014C,b,"try","finally","after");return b.Finish();
    }

    private static byte[] N107_SetDeviceContentTypeBody()
    {
        var b=new IL();b.Arg(0).Field(0x04000023).Op(0x14).Call(0x0A000012).Branch(0x3A,"return");
        N107_DeviceContentReceiver(b);b.Arg(1).Set(0x0400045C);b.Arg(1).Integer(8).Branch(0x40,"return");
        N107_DeviceContentReceiver(b);b.Field(0x0400045E).Branch(0x3A,"data");N107_DeviceContentReceiver(b);b.New(0x06000300).Set(0x0400045E);
        b.Label("data").Arg(0).Field(0x0400002A).Integer(1).Virtual(0x0A000018);
        b.Arg(0).Field(0x0400002A).Store(0);b.Load(0);N107_DeviceContentReceiver(b);
        b.Field(0x0400045E).FieldAddress(0x04000461).Call(0x0A00002A).Virtual(0x0A00002B);
        b.Arg(0).Field(0x0400002F).Integer(1).Virtual(0x0A000018);
        N107_DeviceContentReceiver(b);b.Field(0x0400045E).Field(0x04000460).Store(1);
        b.Arg(0).Field(0x0400002F).Load(1).Virtual(0x0A000039);b.Label("return").Op(0x2A);return b.Finish();
    }
    private static void N107_DeviceContentReceiver(IL b) => b.Arg(0).Field(0x04000023).Field(0x04000035).Field(0x040001D3).Field(0x04000459);

    private static byte[] N107_ShovelPlantBody()
    {
        var b=new IL();b.Arg(0).Call(0x06000397);b.Arg(0).Field(0x040004F4).Call(0x0A00000B).Branch(0x39,"return");
        b.Arg(0).Field(0x040004F4).Store(0);b.Arg(2).Integer(1).Branch(0x40,"refund");
        b.Load(0).Load(0).Field(0x04000401).Arg(1).Op(0x6B).Float(0.1f).Op(0x5A).Float(0.2f).Op(0x58,0x5A);
        b.Load(0).Field(0x04000402).Op(0x58).Set(0x04000402).Branch(0x38,"return");
        b.Label("refund").Load(0).Load(0).Field(0x04000401).Float(0.2f).Op(0x5A).Load(0).Field(0x04000402).Op(0x58).Set(0x04000402);
        b.Arg(2).Integer(2).Branch(0x40,"baseRefund");b.Arg(1).Op(0x6B).Float(0.1f).Op(0x5A).Float(0.2f).Op(0x58).Store(2).Branch(0x38,"cost");
        b.Label("baseRefund").Float(0.2f).Store(2);
        b.Label("cost").Arg(0).Field(0x040004F4).Field(CardSunCost).Op(0x6B).Load(2).Op(0x5A).Store(2);
        // Preserve x64 CVTTSS2SI's indefinite result on NaN/out-of-range, across target CPUs.
        b.Load(2).Float(-2147483648f).Branch(0x44,"indefinite");b.Load(2).Float(2147483648f).Branch(0x3C,"indefinite");
        b.Load(2).Op(0x69).Store(1).Branch(0x38,"amount");b.Label("indefinite").Integer(int.MinValue).Store(1);
        b.Label("amount").Load(1).Integer(0).Branch(0x3E,"return");
        b.Arg(0).Field(0x040004F3).Field(0x040003D7).Integer(2).Integer(0);
        b.Arg(0).Field(0x040004A4).Arg(0).Field(0x040004A6).Arg(0).Field(0x040004A5).Op(0x58).Float(40).Op(0x58).Float(0).New(0x0A00009C);
        b.Virtual(0x06000201).Store(3);b.Load(3).Load(1).Virtual(0x060003CE);b.Load(3).Arg(0).Virtual(0x2B000059);
        b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N107_RewindSwfBody()
    {
        var b=new IL();b.Arg(0).Field(0x04000A99).Store(0);b.Load(0).Branch(0x39,"forward");b.Load(0).Integer(1).Branch(0x3B,"backward");
        b.Ref(0x72,0x7002C8EB).Arg(0).Call(0x06000866).Ref(0x8C,0x02000122).Call(0x0A00026E).New(0x0A0003AE).Op(0x7A);
        b.Label("forward").Arg(0).Field(0x04000A8F).Call(0x0A00000B).Branch(0x39,"event");
        b.Arg(0).Field(0x04000A8F).Integer(0).Virtual(0x06000833).Branch(0x38,"event");
        b.Label("backward").Arg(0).Field(0x04000A8F).Call(0x0A00000B).Branch(0x39,"event");b.Arg(0).Field(0x04000A8F).Store(1);
        b.Load(1).Virtual(0x06000834).Integer(0).Branch(0x3E,"zero");
        b.Load(1).Load(1).Virtual(0x06000834).Integer(1).Op(0x59).Virtual(0x06000833).Branch(0x38,"event");
        b.Label("zero").Load(1).Integer(0).Virtual(0x06000833);
        b.Label("event").Arg(0).Field(0x04000A90).Branch(0x39,"return");b.Arg(0).Field(0x04000A94).Branch(0x39,"return");
        b.Arg(0).Field(0x04000A94).Arg(0).Virtual(0x0A0003FE);b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N107_ZombieHeadPositionBody()
    {
        var b=new IL();b.Arg(0).Field(0x040005B9).Store(1);b.Static(0x0A000013).Store(0);
        foreach(int id in new[]{0,1,2,4,5,10,14,15})b.Load(1).Integer(id).Branch(0x3B,"zombie");
        foreach(int id in new[]{6,7,8,19,20,21})b.Load(1).Integer(id).Branch(0x3B,"pea");
        foreach(int id in new[]{3,23})b.Load(1).Integer(id).Branch(0x3B,"pole");
        b.Load(1).Integer(11).Branch(0x3B,"wall");b.Load(1).Integer(16).Branch(0x3B,"ladder").Branch(0x38,"selected");
        b.Label("pea").Ref(0x72,HeadPea).Store(0).Branch(0x38,"selected");b.Label("pole").Ref(0x72,HeadPole).Store(0).Branch(0x38,"selected");
        b.Label("wall").Ref(0x72,0x7000819E).Store(0).Branch(0x38,"selected");b.Label("ladder").Ref(0x72,HeadLadder).Store(0).Branch(0x38,"selected");
        b.Label("zombie").Ref(0x72,0x70010C24).Store(0).Branch(0x38,"position");
        b.Label("selected").Load(0).Static(0x0A000013).Call(0x0A00003A).Branch(0x3A,"zero");
        b.Label("position").Arg(0).Field(0x04000609).Load(0).Call(0x06000141).Op(0x2A);
        b.Label("zero").Float(0).Float(0).Float(0).New(0x0A00009C).Op(0x2A);return b.Finish();
    }

    private static byte[] N107_SetHardAdventureStateBody()
    {
        var b=new IL();b.Arg(1).Branch(0x39,"return");b.Arg(0).Arg(1).Call(0x06000634).Branch(0x3A,"locked");b.Integer(0).Store(0);
        b.Label("star").Load(0).Arg(0).Field(0x04000841).Virtual(0x0A000008).Branch(0x3C,"trophy");
        b.Arg(0).Field(0x04000841).Load(0).Virtual(0x0A000006).Store(1);
        b.Load(0).Arg(1).Field(0x04000635).Arg(0).Field(0x04000842).Op(0x94,0xFE,0x04).Store(2);
        b.Load(1).Load(2).Virtual(0x0A000007);b.Load(0).Integer(1).Op(0x58).Store(0).Branch(0x38,"star");
        b.Label("trophy").Arg(0).Field(0x04000840).Virtual(0x0A000009).Arg(1).Integer(2).Arg(0).Field(0x04000842);
        b.Virtual(0x060004C5).Virtual(0x0A000007).Branch(0x38,"return");
        b.Label("locked").Arg(0).Field(0x04000841).Virtual(0x0A0000FD).Store(3);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(3).Call(0x0A0000FE).Integer(0).Virtual(0x0A000007);
        b.Label("move").Address(3).Call(0x0A0000FF).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(3).Call(0x0A000128).Op(0xDC);b.Label("after").Branch(0x38,"return");
        b.Label("return").Op(0x2A);RegisterFinally(0x06000633,b,"try","finally","after");return b.Finish();
    }

    private static byte[] N107_ReceiveSuppliesBody()
    {
        var b=new IL();b.Arg(1).Field(0x04000678).Store(0);b.Arg(0).Field(0x04000310).Store(1);
        b.Load(0).Integer(50).Branch(0x3B,"plant");b.Load(0).Integer(51).Branch(0x3B,"prop");
        b.Load(1).Load(0).Virtual(0x060004C4).Store(2);b.Load(2).Field(0x0400067A).Branch(0x3A,"quantity");
        b.Arg(0).Field(0x04000310).Field(0x0400063D).Load(2).Virtual(SuppliesListRemove).Op(0x26);
        b.Arg(0).Field(0x04000310).Field(0x0400063D).Load(2).Virtual(0x0A00027F);
        b.Label("quantity").Load(2).Load(2).Field(0x0400067A).Arg(1).Field(0x0400067A).Op(0x58).Set(0x0400067A).Op(0x2A);
        b.Label("plant").Load(1).Arg(1).Field(0x0400067B).Virtual(0x060004C1).Branch(0x3A,"return");
        b.Arg(0).Arg(1).Field(0x0400067B).Call(0x0600023E).Store(3);b.Arg(0).Field(0x04000310).Load(3).Virtual(0x060004C2).Op(0x2A);
        b.Label("prop").Load(1).Arg(1).Field(0x0400067B).Virtual(0x060004C3).Branch(0x3A,"return");
        b.Arg(1).Field(0x0400067B).New(0x060004D6).Store(4);b.Arg(0).Field(0x04000310).Field(0x0400063A).Load(4).Virtual(0x0A000196);
        b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N107_ZombieDamageRangeBody()
    {
        var b=new IL();b.Op(0x14).Store(0);b.Arg(0).Field(0x040005B9).Store(1);
        b.Load(1).Integer(3).Branch(0x3B,"id3");b.Load(1).Integer(11).Branch(0x3B,"id11");
        b.Load(1).Integer(16).Branch(0x3B,"id16");b.Load(1).Integer(17).Branch(0x3B,"id17");b.Load(1).Integer(18).Branch(0x3B,"id18").Branch(0x38,"write");
        b.Label("id3").Arg(3).Call(0x0A00000B).Branch(0x39,"write");b.Arg(2).Integer(9).Branch(0x40,"write");
        N107_EmitPlantCircle(b,195);b.Branch(0x38,"write");b.Label("id11");N107_EmitZombieCircle(b,215);b.Branch(0x38,"write");
        b.Label("id16").Arg(3).Call(0x0A00000B).Branch(0x3A,"id16Target");b.Arg(2).Integer(-1).Branch(0x40,"write");
        N107_EmitZombieCircle(b,270);b.Branch(0x38,"write");
        b.Label("id16Target").Arg(2).Integer(27).Branch(0x3B,"range165");b.Arg(2).Integer(28).Branch(0x40,"write");
        b.Label("range165");N107_EmitPlantCircle(b,165);b.Branch(0x38,"write");
        b.Label("id17").Arg(3).Call(0x0A00000B).Branch(0x39,"write");b.Arg(2).Integer(26).Branch(0x40,"write");N107_EmitPlantCircle(b,350);b.Branch(0x38,"write");
        b.Label("id18").Arg(4).Branch(0x3A,"write");N107_EmitZombieCircle(b,285);
        b.Label("write").Arg(1).Op(0x50).Load(0).Set(0x0400012E).Op(0x2A);return b.Finish();
    }
    private static void N107_EmitPlantCircle(IL b,float radius) => b.Arg(3).Arg(3).Virtual(0x0A000001).Float(radius).Call(0x2B000056).Store(0);
    private static void N107_EmitZombieCircle(IL b,float radius) => b.Arg(0).Arg(0).Virtual(0x0A000001).Float(radius).Call(ZombieCircleRange).Store(0);

    private static byte[] N107_MouseCollectProjectBody()
    {
        var b=new IL();b.Arg(0).Field(0x04000230).Field(0x040003DB).Arg(0).Field(MouseWorldPosition).Virtual(0x06000266).Store(0);
        b.Load(0).Call(0x0A00000B).Branch(0x3A,"click");
        b.Arg(0).Field(0x04000230).Field(0x040003D7).Store(1);b.Arg(0).Field(MouseWorldPosition).Store(2);b.Integer(0).Store(3);
        b.Label("loop").Load(3).Load(1).Field(0x040002C2).Virtual(ProjectListCount).Branch(0x3C,"none");
        b.Load(1).Field(0x040002C2).Load(3).Virtual(0x0A000155).Store(0);
        b.Load(0).Field(0x04000520).Integer(4).Branch(0x3B,"none");b.Load(0).Field(0x04000520).Integer(6).Branch(0x3B,"none");
        b.Load(0).Virtual(0x0A000001).Virtual(0x0A000037).Store(4);
        foreach(var a in new[]{(Field:0x0A00001D,Local:5),(Field:0x0A00008B,Local:6),(Field:0x0A00009D,Local:7)})
            b.Address(4).Field(a.Field).Address(2).Field(a.Field).Op(0x59).Store(a.Local);
        // Native helper sums y*y + x*x + z*z in float, then sqrt in double and rounds to float.
        b.Load(6).Load(6).Op(0x5A).Store(8);b.Load(5).Load(5).Op(0x5A).Store(9);b.Load(7).Load(7).Op(0x5A).Store(10);
        b.Load(8).Load(9).Op(0x58).Store(8);b.Load(8).Load(10).Op(0x58).Store(8);b.Load(8).Op(0x6C).Call(0x0A00009F).Op(0x6B).Store(8);
        b.Load(0).Field(0x04000519).Load(8).Branch(0x3C,"selected");b.Load(3).Integer(1).Op(0x58).Store(3).Branch(0x38,"loop");
        b.Label("none").Op(0x14).Store(0);b.Label("selected").Load(0).Call(0x0A00000B).Branch(0x39,"return");
        b.Label("click").Load(0).Arg(0).Virtual(0x060003C2);b.Label("return").Load(0).Op(0x2A);return b.Finish();
    }

    private static byte[] N107_SeekDeviceZombieBody()
    {
        var b=new IL();b.Arg(0).Field(0x04000494).Integer(0).Virtual(0x060005BD).Store(0);b.Op(0x14).Store(2);b.Float(9999).Store(3);
        b.Arg(0).Field(0x04000463).Field(0x040003D5).Field(0x04000362).Virtual(0x0A0000D9).Store(4);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(4).Call(0x0A0000DA).Store(1);
        b.Load(1).Virtual(0x06000431).Branch(0x39,"move");b.Load(1).Virtual(0x06000464).Branch(0x3A,"move");
        b.Load(1).Field(0x040005F1).Arg(0).Field(0x0400046E).Branch(0x3B,"move");b.Load(0).Load(1).Virtual(0x2B000055).Branch(0x39,"move");
        b.Load(1).Field(0x040005AD).Load(3).Branch(0x41,"move");b.Load(1).Store(2);b.Load(1).Field(0x040005AD).Store(3);
        b.Label("move").Address(4).Call(0x0A0000DC).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(4).Call(0x0A00015E).Op(0xDC);
        b.Label("after").Arg(0).Load(2).Set(0x0400048A);b.Arg(0).Field(0x0400048A).Op(0x2A);
        RegisterFinally(0x06000324,b,"try","finally","after");return b.Finish();
    }

    private static byte[] N107_CreateBoardEntriesBody()
    {
        var b=new IL();b.Arg(0).Field(0x04000804).Branch(0x39,"return");
        b.Arg(0).Field(0x04000804).Field(0x040001A0).Virtual(0x0A0000E6).Store(0);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(0).Call(0x0A0000E7).Store(1);
        b.Arg(0).Field(0x04000803).Call(0x2B000089).Store(2);b.Load(2).Op(0x14).Call(0x0A000012).Branch(0x3A,"leave");
        b.Load(2).Arg(0).Set(0x04000806);b.Load(2).Load(1).Virtual(0x06000613);
        b.Load(2).Virtual(0x0A000001).Arg(0).Field(0x04000805).Virtual(0x0A00002E).Integer(1).Virtual(0x0A00002F);
        b.Arg(0).Field(0x04000802).Load(2).Virtual(0x0A00028F);
        b.Label("move").Address(0).Call(0x0A0000E8).Branch(0x3A,"body");b.Label("leave").Branch(0xDD,"after");
        b.Label("finally").Address(0).Call(0x0A0000A7).Op(0xDC);b.Label("after").Branch(0x38,"return");
        b.Label("return").Op(0x2A);RegisterFinally(0x0600060F,b,"try","finally","after");return b.Finish();
    }

    private static byte[] N107_PlantSameOccupancyBody()
    {
        var b=new IL();b.Op(0x14).Store(2);b.Arg(0).Virtual(0x0A000001).Virtual(0x0A000090).Store(0);
        b.Label("try").Branch(0x38,"move");b.Label("body").Load(0).Virtual(0x0A0000ED).Ref(0x74,0x01000005).Virtual(0x2B00000E).Store(1);
        b.Load(1).Field(0x040004A0).Arg(1).Branch(0x40,"move");b.Load(1).Field(0x040004A1).Arg(2).Branch(0x40,"move");
        b.Load(1).Field(0x040004DA).Arg(3).Branch(0x40,"move");b.Load(1).Store(2).Branch(0xDD,"after");
        b.Label("move").Load(0).Virtual(0x0A0000EC).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Load(0).Ref(0x75,0x01000036).Store(3);b.Load(3).Branch(0x39,"endfinally");b.Load(3).Virtual(0x0A0000D0);
        b.Label("endfinally").Op(0xDC);b.Label("after").Load(2).Op(0x2A);
        RegisterFinally(0x060001F0,b,"try","finally","after");return b.Finish();
    }

    private static byte[] N107_CreateWarehouseSuppliesBody()
    {
        var b=new IL();b.Arg(0).Field(0x040008E7).Field(0x0400063D).Virtual(SavedSuppliesEnumerator).Store(0);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(0).Call(SavedSuppliesCurrent).Store(1);
        b.Load(1).Field(0x0400067A).Integer(0).Branch(0x3E,"move");b.Load(1).Integer(1).Virtual(0x060004D9).Store(2);
        b.Load(2).Arg(0).Set(0x0400059B);b.Load(2).Virtual(0x0A000001).Arg(0).Field(0x040008E9).Virtual(0x0A00002E).Integer(0).Virtual(0x0A00002F);
        b.Load(2).Virtual(0x0A000001).Float(0.9f).Float(0.9f).Float(1).New(0x0A00009C).Virtual(0x0A0000E5);
        b.Arg(0).Field(0x040008EA).Load(2).Virtual(0x0A0002C3);
        b.Label("move").Address(0).Call(SavedSuppliesMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(0).Call(0x0A00011C).Op(0xDC);b.Label("after").Op(0x2A);
        RegisterFinally(0x060006EB,b,"try","finally","after");return b.Finish();
    }

    private static byte[] N107_CheckEditorWaveBody()
    {
        var b=new IL();b.Arg(0).Field(0x04000018).Field(0x0400004C).Virtual(0x0A000009).Arg(1).Integer(0).Op(0xFE,0x02).Virtual(0x0A000007);
        b.Arg(0).Field(0x04000018).Field(0x0400004B).Virtual(0x0A000009).Arg(1).Arg(0).Field(0x0400000D).Field(0x0400019A).Op(0xFE,0x04).Virtual(0x0A000007);
        b.Arg(0).Arg(1).Set(0x04000021);b.Arg(0).Field(0x04000018).Field(0x0400003F).Arg(0).FieldAddress(0x04000021).Call(0x0A00002A).Virtual(0x0A00002B);
        b.Arg(0).Field(0x0400001B).Virtual(EnemyEditorEnumerator).Store(0);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(0).Call(EnemyEditorCurrent).Store(1);
        b.Load(1).Field(0x04000051).Field(0x040001FB).Arg(1).Op(0xFE,0x01).Store(2);b.Load(1).Virtual(0x0A000009).Load(2).Virtual(0x0A000007);
        b.Label("move").Address(0).Call(EnemyEditorMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(0).Call(0x0A00002C).Op(0xDC);b.Label("after").Op(0x2A);
        RegisterFinally(0x06000013,b,"try","finally","after");return b.Finish();
    }

    private static byte[] N107_LoadEnemyEditorBody()
    {
        var b=new IL();b.Arg(0).Field(0x04000051).Branch(0x39,"return");b.Call(0x06000232).Store(0);b.Arg(0).Field(0x0400004E).Store(1);
        b.Load(0).Virtual(SpriteListCount).Arg(0).Field(0x04000051).Field(0x040001F7).Branch(0x3E,"noSprite");
        b.Load(0).Arg(0).Field(0x04000051).Field(0x040001F7).Virtual(0x0A000040).Store(2).Branch(0x38,"image");
        b.Label("noSprite").Op(0x14).Store(2);b.Label("image").Load(1).Load(2).Virtual(0x0A000041);
        b.Arg(0).Field(0x04000050).Field(0x0400000D).Store(3);b.Arg(0).Virtual(0x0A000001).Store(4);
        b.Load(4).Load(3).Arg(0).Field(0x04000051).Field(0x040001F9).Arg(0).Field(0x04000051).Field(0x040001F8).Virtual(0x0600015C).Virtual(0x0A000042);
        b.Arg(0).Field(0x04000054).Virtual(0x0A000419).Store(5);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(5).Call(0x0A00041C).Virtual(0x06000060);
        b.Label("move").Address(5).Call(0x0A00041A).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(5).Call(0x0A000046).Op(0xDC);b.Label("after").Branch(0x38,"return");
        b.Label("return").Op(0x2A);RegisterFinally(0x0600004C,b,"try","finally","after");return b.Finish();
    }

    private static byte[] N107_TriggerPlantSkillBody()
    {
        var b=new IL();b.Arg(0).Field(0x040004D1).Arg(0).Op(0x14,0x14,0x14).Virtual(0x060004DE);
        b.Arg(0).Field(0x040004B2).Integer(1).Branch(0x40,"return");
        b.Arg(0).Field(0x040004F2).Field(0x040002B1).Virtual(0x0A0000D3).Store(0);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(0).Call(0x0A0000D4).Store(1);
        b.Load(1).Field(0x040004B1).Integer(7).Branch(0x40,"move");b.Load(1).Field(0x040004B4).Integer(1).Branch(0x3F,"move");
        b.Ref(0x72,0x70003D3C).Float(0.15f).Integer(1).New(0x060000F6).Store(2);
        b.Load(2).Ref(0x72,0x7000EAF3).Set(0x04000105);b.Load(2).Integer(0).Set(0x04000106);b.Load(2).Float(10).Set(0x04000107);
        b.Load(1).Field(0x040004FF).Load(2).Virtual(0x06000102);
        b.Label("move").Address(0).Call(0x0A0000D6).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(0).Call(0x0A00015D).Op(0xDC);b.Label("after").Branch(0x38,"return");
        b.Label("return").Op(0x2A);RegisterFinally(0x060003B8,b,"try","finally","after");return b.Finish();
    }

    private static byte[] N107_GridPassablePointBody()
    {
        var b=new IL();b.Arg(0).Field(0x04000385).Store(0);b.Arg(0).Field(0x0400038E).Op(0x14).Call(0x0A00000C).Branch(0x39,"layers");
        b.Load(0).Arg(0).Field(0x0400038E).Field(0x0400047B).Call(0x0A0001CB).Op(0x2A);
        b.Label("layers");foreach(int f in new[]{0x0400038C,0x0400038D,0x0400038F,0x04000390})
        {b.Arg(0).Field(f).Op(0x14).Call(0x0A00000C).Branch(0x39,"next"+f);b.Load(0).Arg(0).Field(f).Field(0x0400047B).Call(0x0A0001CB).Store(0);b.Label("next"+f);}
        b.Load(0).Op(0x2A);return b.Finish();
    }

    private static byte[] N107_DeviceBrightnessBody()
    {
        var b=new IL();b.Arg(0).Field(0x04000479).Float(0).Branch(0x3D,"ice");b.Arg(0).Field(0x04000488).Virtual(0x0A0000FD).Store(0);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(0).Call(0x0A0000FE).Call(0x2B000011).Virtual(0x0A00021D).Static(0x04000495).Arg(1).Virtual(0x0A00003E);
        b.Label("move").Address(0).Call(0x0A0000FF).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(0).Call(0x0A000128).Op(0xDC);b.Label("after").Branch(0x38,"return");
        b.Label("ice").Arg(0).Field(0x04000492).Store(1);
        foreach(int f in new[]{0x0400049D,0x0400049E})b.Load(1).Field(f).Virtual(0x0A00021D).Ref(0x72,0x70004535).Arg(1).Virtual(0x0A00003E);
        b.Label("return").Op(0x2A);RegisterFinally(0x06000330,b,"try","finally","after");return b.Finish();
    }

    private static byte[] N107_UpdateMoneyBanksBody()
    {
        var b=new IL();N107_EmitMoneyBank(b,true);N107_EmitMoneyBank(b,false);b.Op(0x2A);return b.Finish();
    }
    private static void N107_EmitMoneyBank(IL b,bool coin)
    {
        int bank=coin?0x040002C3:0x040002C6,timer=coin?0x040002C9:0x040002CA,text=coin?0x040002C5:0x040002C8,money=coin?0x040002C4:0x040002C7;
        string id=coin?"coin":"diamond";
        b.Arg(0).Field(bank).Op(0x14).Call(0x0A00000C).Branch(0x39,id+"End");b.Arg(0).Field(timer).Store(0);
        b.Load(0).Float(0).Branch(0x3D,id+"Tick").Branch(0x38,id+"End");
        b.Label(id+"Tick").Call(0x0A00001A).Store(1);b.Arg(0).Field(text).Store(2);
        b.Arg(0).Load(0).Load(1).Op(0x59).Set(timer);b.Load(2).Call(0x0A00000B).Branch(0x39,id+"Expire");b.Arg(0).Field(text).Store(2);
        if(coin)
        {b.Ref(0x72,0x70007070).Call(0x0A00016D).Store(3);b.Load(2).Arg(0).FieldAddress(money).Ref(0x72,0x7000707C).Load(3).Call(0x0A00016E).Virtual(0x0A000062);}
        else b.Load(2).Ref(0x72,0x70007082).Arg(0).FieldAddress(money).Call(0x0A00002A).Call(0x0A000051).Virtual(0x0A000062);
        b.Label(id+"Expire").Arg(0).Field(timer).Float(0).Branch(0x42,id+"End");b.Arg(0).Float(0).Set(timer);b.Arg(0).Field(bank).Integer(0).Virtual(0x0A000007);
        b.Label(id+"End");
    }

    private static byte[] N107_SaveTeamPropsBody()
    {
        var b=new IL();N107_EmitCurrentTeam(b);b.Store(0);b.Address(0).Field(0x0400064C).Virtual(IntListClear);
        b.Arg(0).Field(0x0400088B).Field(0x0400089D).Virtual(PropControllerEnumerator).Store(1);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(1).Call(PropControllerCurrent).Store(2);
        b.Load(2).Field(0x040008A4).Op(0x14).Call(0x0A00000C).Branch(0x39,"move");
        N107_EmitCurrentTeam(b);b.Store(0);b.Load(2).Field(0x040008A4).Field(0x0400058C).Store(3);b.Address(0).Field(0x0400064C).Load(3).Virtual(0x0A000413);
        b.Label("move").Address(1).Call(PropControllerMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(1).Call(0x0A00029C).Op(0xDC);b.Label("after").Op(0x2A);
        RegisterFinally(0x0600068C,b,"try","finally","after");return b.Finish();
    }
    private static void N107_EmitCurrentTeam(IL b) => b.Arg(0).Field(0x0400088E).Field(0x04000642).Arg(0).Field(0x0400088D).Integer(1).Op(0x59).Virtual(0x0A000235);
}
