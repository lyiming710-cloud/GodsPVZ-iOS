// Source construction only. No body in this file has been built or executed by its author.
internal static partial class Program
{
    private static void N116_Finally(IL b,int token,params (string Start,string Handler,string End)[] ranges)
    {
        // Supports the two distinct/nested native cleanup regions in this batch.
        Require(N116_EhTargets.Contains(token),"Native116 explicit EH scope");
        FinallyBodies.Add(token,ranges.Select(r=>new FinallyClause(b.OffsetOf(r.Start),b.OffsetOf(r.Handler),b.OffsetOf(r.Handler),b.OffsetOf(r.End))).ToArray());
    }

    private static byte[] N116_DevicePreviews()
    {
        var b=new IL();b.Arg(0).Field(0x0400000D).Field(0x0400019E).Virtual(N116_PreDeviceEnum).Store(0);
        b.Label("try").Branch(0x38,"move");
        b.Label("body").Address(0).Call(N116_PreDeviceCurrent).Store(1);
        b.Arg(0).Field(0x0400001C).Call(0x2B000006).Store(2);
        b.Load(2).Virtual(0x0A000001).Arg(0).Field(0x04000015).Virtual(0x0A00002E).Integer(0).Virtual(0x0A00002F);
        b.Load(2).Arg(0).Set(0x04000034);b.Load(2).Load(1).Set(0x04000035);b.Load(2).Virtual(0x06000039);
        b.Arg(0).Field(0x0400001A).Load(2).Virtual(N116_DeviceEditorAdd);
        b.Label("move").Address(0).Call(N116_PreDeviceMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(0).Call(0x0A000034).Op(0xDC);b.Label("after").Op(0x2A);
        N116_Finally(b,0x06000017,("try","finally","after"));return b.Finish();
    }

    private static byte[] N116_EnemyPreviews()
    {
        var b=new IL();b.Call(0x06000232).Op(0x26);
        b.Arg(0).Field(0x0400000D).Field(0x0400019D).Virtual(N116_EnemyEnum).Store(0);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(0).Call(N116_EnemyCurrent).Store(1);
        b.Arg(0).Field(0x0400001D).Call(0x2B000007).Store(2);
        b.Load(2).Virtual(0x0A000001).Arg(0).Field(0x04000016).Virtual(0x0A00002E).Integer(0).Virtual(0x0A00002F);
        b.Load(2).Arg(0).Set(0x04000050);b.Load(2).Load(1).Virtual(0x0600004B);
        b.Arg(0).Field(0x0400001B).Load(2).Virtual(N116_EnemyEditorAdd);
        b.Label("move").Address(0).Call(N116_EnemyMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(0).Call(0x0A000035).Op(0xDC);b.Label("after").Op(0x2A);
        N116_Finally(b,0x06000018,("try","finally","after"));return b.Finish();
    }

    private static byte[] N116_CreateEnemy()
    {
        var b=new IL();b.New(0x060001A0).Store(0);
        b.Load(0).Arg(0).Field(0x0400000D).Field(0x04000198).Virtual(0x06000289).Set(0x040001F9);
        b.Load(0).Arg(0).Field(0x0400000D).Field(0x04000198).Virtual(0x0600028A).Integer(2).Op(0x5B).Set(0x040001F8);
        b.Load(0).Arg(0).Field(0x04000021).Set(0x040001FB);
        b.Arg(0).Field(0x0400001D).Call(0x2B000007).Store(1);
        b.Load(1).Virtual(0x0A000001).Arg(0).Field(0x04000016).Virtual(0x0A00002E).Integer(0).Virtual(0x0A00002F);
        b.Load(1).Arg(0).Set(0x04000050);b.Load(1).Load(0).Virtual(0x0600004B);
        b.Arg(0).Field(0x0400001B).Load(1).Virtual(N116_EnemyEditorAdd);
        b.Arg(0).Field(0x04000018).Load(1).Virtual(0x06000042);
        b.Arg(0).Field(0x0400000D).Field(0x0400019D).Load(1).Field(0x04000051).Virtual(N116_EnemyAdd);
        b.Op(0x2A);return b.Finish();
    }

    private static byte[] N116_EnemyEditorInitialize()
    {
        var b=new IL();b.Arg(0).Arg(1).Set(0x04000051);b.Arg(0).Call(0x0600004C);
        b.Arg(1).Field(0x040001FD).Virtual(N116_PathEnum).Store(0);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(0).Call(N116_PathCurrent).Store(1);
        b.Arg(0).Field(0x04000050).Field(0x0400001E).Call(0x2B000008).Store(2);
        b.Load(2).Arg(0).Set(0x04000067);b.Load(2).Load(1).Set(0x04000068);b.Load(2).Virtual(0x06000060);
        b.Load(2).Virtual(0x0A000001).Arg(0).Virtual(0x0A000001).Integer(1).Virtual(0x0A00002F);
        b.Arg(0).Field(0x04000054).Load(2).Virtual(N116_PathEditorAdd);
        b.Load(2).Virtual(0x0A000009).Integer(0).Virtual(0x0A000007);
        b.Label("move").Address(0).Call(N116_PathMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(0).Call(0x0A000045).Op(0xDC);b.Label("after").Op(0x2A);
        N116_Finally(b,0x0600004B,("try","finally","after"));return b.Finish();
    }

    private static byte[] N116_GetBoardEntry(bool zombie)
    {
        var b=new IL();int field=zombie?0x04000369:0x040001DA;
        int[] values=zombie?[0x0400036A,0x0400036B,0x0400036C,0x0400036D,0x0400036E]:[0x040001DB,0x040001DC,0x040001DD];
        b.Arg(0).Field(field).Store(0);b.New(zombie?0x0600027F:0x06000191).Store(1);
        foreach(int f in values)b.Load(1).Load(0).Field(f).Set(f);
        b.Arg(0).Field(zombie?0x0400035F:0x040001D4).Op(0x14).Call(0x0A00000C).Branch(0x39,"after");
        b.Arg(0).Field(zombie?0x04000368:0x040001D9).Virtual(N116_BoardEntryEnum).Store(2);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(2).Call(N116_BoardEntryCurrent).Store(3);
        b.Load(3).Field(0x040007FC).Branch(0x3A,"key");b.Load(3).Field(0x040007FD).Branch(0x39,"move");
        b.Label("key").Load(3).Field(0x040007FE).ArgAddress(1).Call(0x0A00002A).Call(0x0A00003A).Branch(0x39,"move");
        var cases=zombie?new[]{(Type:0,Fields:new[]{values[0],values[1]}),(Type:6,Fields:new[]{values[3]}),(Type:32,Fields:new[]{values[4]})}
            :new[]{(Type:33,Fields:new[]{values[1]}),(Type:34,Fields:new[]{values[2]}),(Type:35,Fields:new[]{values[0]})};
        foreach(var p in cases)b.Load(3).Field(0x040007FA).Integer(p.Type).Branch(0x3B,"case"+p.Type);
        b.Branch(0x38,"move");
        foreach(var p in cases)
        {
            b.Label("case"+p.Type);
            foreach(int f in p.Fields)b.Load(1).Load(1).Field(f).Load(3).Virtual(0x06000607).Op(0x58,0x6B).Set(f);
            b.Branch(0x38,"move");
        }
        b.Label("move").Address(2).Call(N116_BoardEntryMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(2).Call(0x0A0000A7).Op(0xDC);b.Label("after").Load(1).Op(0x2A);
        N116_Finally(b,zombie?0x0600026F:0x06000187,("try","finally","after"));return b.Finish();
    }

    private static byte[] N116_StartZombieBoardEntry()
    {
        var b=new IL();b.Arg(0).Field(0x0400035F).Op(0x14).Call(0x0A00000C).Branch(0x39,"after");
        b.Arg(0).Field(0x0400035F).Field(0x040003B4).Field(0x040001A0).Virtual(N116_BoardEntryEnum).Store(0);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(0).Call(N116_BoardEntryCurrent).Store(1);
        b.Load(1).Field(0x040007FC).Branch(0x3A,"add");b.Load(1).Field(0x040007FD).Branch(0x39,"move");
        b.Label("add").Arg(0).Field(0x04000368).Load(1).Virtual(0x0A0000A6);
        b.Load(1).Field(0x040007FE).Static(0x0A000013).Call(0x0A00003A).Branch(0x39,"move");
        foreach(int id in new[]{0,6,32})b.Load(1).Field(0x040007FA).Integer(id).Branch(0x3B,"case"+id);
        b.Branch(0x38,"move");
        foreach(var p in new[]{(Type:0,Fields:new[]{0x0400036A,0x0400036B}),(Type:6,Fields:new[]{0x0400036D}),(Type:32,Fields:new[]{0x0400036E})})
        {
            b.Label("case"+p.Type);
            foreach(int f in p.Fields)
            {
                // Re-read manager.boardEntryData for each GetValue callback, capturing that object and old field first.
                b.Arg(0).Field(0x04000369).Store(2);b.Load(2).Load(2).Field(f).Load(1).Virtual(0x06000607).Op(0x58,0x6B).Set(f);
            }
            b.Branch(0x38,"move");
        }
        b.Label("move").Address(0).Call(N116_BoardEntryMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(0).Call(0x0A0000A7).Op(0xDC);b.Label("after").Op(0x2A);
        N116_Finally(b,0x06000272,("try","finally","after"));return b.Finish();
    }

    private static byte[] N116_FillWave()
    {
        var b=new IL();b.Arg(0).Field(0x04000208).Arg(1).Op(0x5A).Arg(0).Field(0x04000208).Op(0x58);
        b.Arg(0).Field(0x04000208).Arg(2).Op(0x5A).Integer(3).Op(0x5B,0x58).Store(0);
        b.Arg(2).Integer(10).Op(0x5D).Integer(9).Branch(0x40,"outer");
        b.Load(0).Arg(0).Field(0x04000208).Op(0x58).Arg(0).Field(0x04000208).Integer(2).Op(0x5A,0x58).Store(0);
        b.Label("outer").Load(0).Integer(0).Branch(0x3E,"after");b.New(0x0A000139).Store(1);b.Float(0).Store(2);
        b.Arg(0).Field(N116_ZombieSelectsField).Virtual(N116_ZombieSelectEnum).Store(3);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(3).Call(N116_ZombieSelectCurrent).Store(4);
        b.Load(4).Field(0x04000201).Load(0).Branch(0x3D,"move");b.Load(4).Field(0x04000204).Arg(2).Branch(0x3D,"move");
        b.Load(4).Field(0x04000203).Arg(1).Branch(0x3D,"move");b.Load(1).Load(4).Virtual(0x0A00013A);
        b.Load(2).Load(4).Field(0x04000202).Op(0x58,0x6B).Store(2);
        b.Label("move").Address(3).Call(N116_ZombieSelectMove).Branch(0x3A,"body").Branch(0xDD,"choose");
        b.Label("finally").Address(3).Call(0x0A000138).Op(0xDC);
        b.Label("choose").Load(1).Virtual(N116_ZombieSelectCount).Integer(0).Branch(0x3E,"after");
        b.Arg(0).Load(1).Load(2).Call(0x060001AC).Store(4);b.Load(4).Branch(0x39,"after");
        b.Arg(0).Arg(3).Load(4).Call(0x060001A6);b.Load(0).Load(4).Field(0x04000201).Op(0x59).Store(0).Branch(0x38,"outer");
        b.Label("after").Op(0x2A);N116_Finally(b,0x060001A8,("try","finally","choose"));return b.Finish();
    }

    private static byte[] N116_UnfoldProps()
    {
        var b=new IL();b.Arg(0).Arg(1).Set(0x0400089E);b.Arg(0).Field(0x0400089B).Integer(1).Virtual(0x0A000007);
        b.Static(0x040002F2).Integer(9).Virtual(0x0A000084).Call(0x0600013B).Op(0x26);
        b.Arg(0).Field(0x0400089A).Virtual(N116_PropChooseEnum).Store(0);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(0).Call(N116_PropChooseCurrent).Store(1);
        b.Arg(0).Field(0x0400089D).Arg(0).Field(0x0400089E).Virtual(N116_PropBankItem).Field(N116_PropChooseSelectedField).Store(2);
        b.Load(1).Load(2).Call(0x0A00000C).Store(3);
        b.Load(1).Virtual(0x0A000009).Load(3).Virtual(0x0A000007);
        b.Label("move").Address(0).Call(N116_PropChooseMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(0).Call(N116_PropChooseDispose).Op(0xDC);b.Label("after").Op(0x2A);
        N116_Finally(b,0x06000697,("try","finally","after"));return b.Finish();
    }

    private static byte[] N116_LootIcons()
    {
        var b=new IL();b.Arg(1).Virtual(N116_SuppliesEnum).Store(0);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(0).Call(N116_SuppliesCurrent).Integer(1).Virtual(0x060004D9).Store(1);
        b.Load(1).Virtual(0x0A000009).Virtual(0x0A00002E).Float(1).Float(1).Float(1).New(0x0A00009C).Virtual(N116_SetLocalScale);
        b.Load(1).Virtual(0x0A000009).Virtual(0x0A00002E).Arg(0).Field(0x040008D7).Virtual(0x0A00002E).Integer(0).Virtual(0x0A00002F);
        b.Arg(0).Field(0x040008D6).Load(1).Virtual(N116_SuppliesItemAdd);
        b.Label("move").Address(0).Call(N116_SuppliesMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(0).Call(N116_SuppliesDispose).Op(0xDC);b.Label("after").Op(0x2A);
        N116_Finally(b,0x060006D2,("try","finally","after"));return b.Finish();
    }

    private static byte[] N116_MapGrids()
    {
        var b=new IL();b.New(0x0A0001BC).Store(0);b.Arg(0).Field(0x0400037D).Virtual(N116_RowEnum).Store(1);
        b.Label("outerTry").Branch(0x38,"outerMove");b.Label("outerBody").Address(1).Call(N116_RowCurrent).Field(0x04000381).Virtual(N111_GridEnumerator).Store(2);
        b.Label("innerTry").Branch(0x38,"innerMove");b.Label("innerBody").Load(0).Address(2).Call(N111_GridCurrent).Virtual(N116_GridAdd);
        b.Label("innerMove").Address(2).Call(N111_GridMove).Branch(0x3A,"innerBody").Branch(0xDD,"outerMove");
        b.Label("innerFinally").Address(2).Call(0x0A0001BA).Op(0xDC);
        b.Label("outerMove").Address(1).Call(N116_RowMove).Branch(0x3A,"outerBody").Branch(0xDD,"after");
        b.Label("outerFinally").Address(1).Call(N116_RowDispose).Op(0xDC);b.Label("after").Load(0).Op(0x2A);
        N116_Finally(b,0x06000287,("innerTry","innerFinally","outerMove"),("outerTry","outerFinally","after"));return b.Finish();
    }

    private static byte[] N116_KillAll()
    {
        var b=new IL();b.New(0x0A0000D8).Store(0);b.Arg(0).Field(N116_ZombieListField).Virtual(N116_ZombieEnum).Store(1);
        b.Label("try1").Branch(0x38,"move1");b.Label("body1").Address(1).Call(0x0A0000DA).Store(2);
        b.Load(2).Field(0x040005D2).Branch(0x3A,"move1");b.Load(0).Load(2).Virtual(0x0A0000DB);
        b.Label("move1").Address(1).Call(0x0A0000DC).Branch(0x3A,"body1").Branch(0xDD,"second");
        b.Label("finally1").Address(1).Call(0x0A00015E).Op(0xDC);
        b.Label("second").Load(0).Virtual(N116_ZombieEnum).Store(1);
        b.Label("try2").Branch(0x38,"move2");b.Label("body2").Address(1).Call(0x0A0000DA).Store(2);
        b.New(0x06000112).Store(3);b.Load(3).Float(100000000).Set(0x04000123);b.Load(3).Integer(4).Set(0x04000125);
        // Native word store at +0x1F covers two separate Boolean fields. Do not store 257 in one bool.
        foreach(int f in new[]{0x04000129,0x0400012A,0x0400012B})b.Load(3).Integer(1).Set(f);
        b.Load(2).Load(3).Op(0x14).Virtual(0x0600047F);
        b.Label("move2").Address(1).Call(0x0A0000DC).Branch(0x3A,"body2").Branch(0xDD,"after");
        b.Label("finally2").Address(1).Call(0x0A00015E).Op(0xDC);b.Label("after").Op(0x2A);
        N116_Finally(b,0x0600027B,("try1","finally1","second"),("try2","finally2","after"));return b.Finish();
    }

    private static byte[] N116_DestroyPlant()
    {
        var b=new IL();int i=0;
        foreach(int f in new[]{0x040004EA,0x040004EC,0x040004EE})
        {
            b.Arg(0).Field(f).Call(0x0A00000B).Branch(0x39,"next"+i);b.Arg(0).Field(f).Call(0x0A000025);b.Label("next"+i++);
        }
        b.Arg(0).Field(0x040004EF).Virtual(0x0A0000FD).Store(0);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(0).Call(0x0A0000FE).Call(0x0A000025);
        b.Label("move").Address(0).Call(0x0A0000FF).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(0).Call(0x0A000128).Op(0xDC);
        b.Label("after").Arg(0).Virtual(0x0A000009).Call(0x0A000025);
        b.Arg(0).Field(0x040004F3).Call(0x0A00000B).Branch(0x39,"return");
        b.Arg(0).Field(0x040004F3).Field(0x040003D4).Field(0x040002B1).Arg(0).Virtual(N116_PlantRemove).Op(0x26);
        b.Label("return").Op(0x2A);N116_Finally(b,0x0600035C,("try","finally","after"));return b.Finish();
    }

    private static byte[] N116_Flags()
    {
        var b=new IL();b.Float(N116_FlagSpan).Arg(1).Op(0x6B,0x5B,0x6B).Store(0);b.Integer(0).Store(1);
        b.Label("createTest").Load(1).Arg(1).Branch(0x3C,"placeStart");
        // Capture list before Instantiate: the callback may replace the owner field.
        b.Arg(0).Field(0x04000815).Static(0x0400030E).Call(0x2B000028).Virtual(0x0A000086);
        b.Load(1).Integer(1).Op(0x58).Store(1).Branch(0x38,"createTest");
        b.Label("placeStart").Arg(1).Integer(1).Op(0x59).Store(1);b.Integer(0).Store(2);
        b.Label("placeTest").Load(1).Integer(0).Branch(0x3F,"after");
        b.Arg(0).Field(0x04000815).Load(1).Virtual(0x0A000006).Virtual(0x0A00002E);
        b.Arg(0).Field(0x04000814).Virtual(0x0A00002E).Integer(0).Virtual(0x0A00002F);
        b.Load(2).Op(0x6B).Load(0).Op(0x5A).Float(N116_FlagOrigin).Op(0x59,0x6B).Store(3);
        b.Arg(0).Field(0x04000815).Load(1).Virtual(0x0A000006).Virtual(0x0A00002E).Load(3).Float(N116_FlagY).Float(0).New(0x0A00009C).Virtual(0x0A000042);
        b.Load(2).Integer(1).Op(0x58).Store(2);b.Load(1).Integer(1).Op(0x59).Store(1).Branch(0x38,"placeTest");
        b.Label("after").Op(0x2A);return b.Finish();
    }

    private static byte[] N116_RescueLevel()
    {
        var b=new IL();b.Arg(0).Field(0x040008A8).Virtual(N116_LevelEnum).Store(0);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(0).Call(N116_LevelCurrent).Store(1);
        b.Load(1).Field(0x0400082F).Arg(1).Branch(0x40,"move");b.Load(1).Virtual(0x0600062C).Branch(0xDD,"return");
        b.Label("move").Address(0).Call(N116_LevelMove).Branch(0x3A,"body").Branch(0xDD,"create");
        b.Label("finally").Address(0).Call(N116_LevelDispose).Op(0xDC);
        b.Label("create").Arg(0).Field(0x040008A7).Call(0x2B00008E).Store(1);
        b.Load(1).Virtual(0x0A000001).Arg(0).Field(0x040008A6).Virtual(0x0A00002E).Integer(0).Virtual(0x0A00002F);
        b.Load(1).Arg(1).Integer(1).Arg(0).Field(0x040008A9).Arg(1).Virtual(0x0A000293).Virtual(0x0600062B);
        b.Arg(0).Field(0x040008A8).Load(1).Virtual(N116_LevelAdd);b.Label("return").Op(0x2A);
        N116_Finally(b,0x0600069F,("try","finally","create"));return b.Finish();
    }

    private static byte[] N116_HardLevel()
    {
        var b=new IL();b.Static(0x0400015E).Field(0x04000166).Field(0x04000310).Store(0);b.Load(0).Branch(0x39,"return");
        b.Arg(0).Field(0x040006AF).Virtual(N116_HardLevelEnum).Store(1);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(1).Call(N116_HardLevelCurrent).Store(2);
        b.Load(2).Field(0x04000842).Arg(1).Branch(0x40,"move");b.Load(2).Load(0).Virtual(0x06000633).Branch(0xDD,"return");
        b.Label("move").Address(1).Call(N116_HardLevelMove).Branch(0x3A,"body").Branch(0xDD,"create");
        b.Label("finally").Address(1).Call(N116_HardLevelDispose).Op(0xDC);
        b.Label("create").Arg(0).Field(0x040006B0).Call(0x2B00007B).Store(2);
        b.Load(2).Virtual(0x0A000001).Arg(0).Field(0x040006B1).Virtual(0x0A00002E).Integer(0).Virtual(0x0A00002F);
        b.Load(2).Arg(0).Set(0x04000839);b.Load(2).Arg(1).Virtual(0x06000631);b.Load(2).Load(0).Virtual(0x06000633);
        b.Arg(0).Field(0x040006AF).Load(2).Virtual(N116_HardLevelAdd);b.Label("return").Op(0x2A);
        N116_Finally(b,0x0600052A,("try","finally","create"));return b.Finish();
    }

    private static byte[] N116_SetPrechoose()
    {
        var b=new IL();b.Arg(0).Field(0x040008B8).Virtual(N116_CardChooseEnum).Store(0);
        b.Label("resetTry").Branch(0x38,"resetMove");b.Label("resetBody").Address(0).Call(N116_CardChooseCurrent).Store(3);
        b.Load(3).Field(0x04000448).Branch(0x39,"resetMove");b.Load(3).Virtual(0x060002F5);
        b.Label("resetMove").Address(0).Call(N116_CardChooseMove).Branch(0x3A,"resetBody").Branch(0xDD,"afterReset");
        b.Label("resetFinally").Address(0).Call(0x0A0001DF).Op(0xDC);
        b.Label("afterReset").Arg(0).Field(0x040008BE).Op(0x14).Virtual(0x060006C2);
        b.Arg(1).Virtual(N116_CardChooseEnum).Store(0);
        b.Label("outerTry").Branch(0x38,"outerMove");b.Label("outerBody").Address(0).Call(N116_CardChooseCurrent).Store(2);
        b.Arg(0).Field(0x040008B8).Virtual(N116_CardChooseEnum).Store(1);
        b.Label("innerTry").Branch(0x38,"innerMove");b.Label("innerBody").Address(1).Call(N116_CardChooseCurrent).Store(3);
        b.Load(3).Field(0x04000429).Load(2).Field(0x04000429).Branch(0x40,"innerMove");
        b.Load(3).Virtual(0x060002F4);b.Arg(0).Field(0x040008BE).Load(3).Virtual(0x060006C2);
        b.Label("innerMove").Address(1).Call(N116_CardChooseMove).Branch(0x3A,"innerBody").Branch(0xDD,"outerMove");
        b.Label("innerFinally").Address(1).Call(0x0A0001DF).Op(0xDC);
        b.Label("outerMove").Address(0).Call(N116_CardChooseMove).Branch(0x3A,"outerBody").Branch(0xDD,"after");
        b.Label("outerFinally").Address(0).Call(0x0A0001DF).Op(0xDC);b.Label("after").Op(0x2A);
        N116_Finally(b,0x060006BB,("resetTry","resetFinally","afterReset"),("innerTry","innerFinally","outerMove"),("outerTry","outerFinally","after"));return b.Finish();
    }

    private static byte[] N116_LoadTeamCard()
    {
        var b=new IL();b.Arg(0).Field(0x040008B8).Virtual(N116_CardChooseEnum).Store(0);
        b.Label("resetTry").Branch(0x38,"resetMove");b.Label("resetBody").Address(0).Call(N116_CardChooseCurrent).Virtual(0x060002F5);
        b.Label("resetMove").Address(0).Call(N116_CardChooseMove).Branch(0x3A,"resetBody").Branch(0xDD,"start");
        b.Label("resetFinally").Address(0).Call(0x0A0001DF).Op(0xDC);
        b.Label("start").Integer(0).Store(1);
        b.Label("test").Load(1).ArgAddress(1).Field(0x0400064A).Virtual(N116_IntCount).Branch(0x3C,"after");
        b.ArgAddress(1).Field(0x0400064A).Load(1).Virtual(0x0A00006B).Store(2);
        b.Arg(0).Field(0x040008B8).Virtual(N116_CardChooseEnum).Store(0);
        b.Label("chooseTry").Branch(0x38,"chooseMove");b.Label("chooseBody").Address(0).Call(N116_CardChooseCurrent).Store(3);
        b.Load(3).Field(0x04000429).Load(2).Branch(0x40,"chooseMove");b.Load(3).Virtual(0x060002F4);
        b.ArgAddress(1).Field(0x0400064B).Virtual(N116_IntCount).Load(1).Branch(0x3E,"chooseMove");
        // PlantSave receiver is captured before reading the selected skill ID.
        b.Load(3).Field(0x0400042F).Store(4);
        b.Load(3).Load(4).ArgAddress(1).Field(0x0400064B).Load(1).Virtual(0x0A00006B).Virtual(0x060004CB).Virtual(0x060002F9);
        b.Label("chooseMove").Address(0).Call(N116_CardChooseMove).Branch(0x3A,"chooseBody").Branch(0xDD,"next");
        b.Label("chooseFinally").Address(0).Call(0x0A0001DF).Op(0xDC);
        b.Label("next").Load(1).Integer(1).Op(0x58).Store(1).Branch(0x38,"test");b.Label("after").Op(0x2A);
        N116_Finally(b,0x060006B6,("resetTry","resetFinally","start"),("chooseTry","chooseFinally","next"));return b.Finish();
    }

    private static byte[] N116_FindBoardEntryKey()
    {
        var b=new IL();b.Op(0x14).Store(3);b.Arg(0).Virtual(N116_BoardEntryEnum).Store(0);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(0).Call(N116_BoardEntryCurrent).Store(1);
        foreach(int kind in new[]{24,25,36})b.Load(1).Field(0x040007FA).Integer(kind).Branch(0x3B,"suffix"+kind);
        b.Load(1).Field(0x040007FE).Store(2).Branch(0x38,"compare");
        foreach(var x in new[]{(Kind:24,Chosen:0x7001B7B4,Other:0x7001B7BC),(Kind:25,Chosen:0x7001B7C4,Other:0x7001B7CC),(Kind:36,Chosen:0x7001B7D4,Other:0x7001B7DC)})
        {
            b.Label("suffix"+x.Kind).Load(1).Field(0x040007FC).Branch(0x3A,"chosen"+x.Kind);
            b.Load(1).Field(0x040007FD).Branch(0x3A,"chosen"+x.Kind);
            b.Load(1).Field(0x040007FE).Ref(0x72,x.Other).Call(0x0A000051).Store(2).Branch(0x38,"compare");
            b.Label("chosen"+x.Kind).Load(1).Field(0x040007FE).Ref(0x72,x.Chosen).Call(0x0A000051).Store(2).Branch(0x38,"compare");
        }
        b.Label("compare").Load(2).Arg(1).Call(0x0A00003A).Branch(0x39,"move");b.Load(1).Store(3).Branch(0xDD,"after");
        b.Label("move").Address(0).Call(N116_BoardEntryMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(0).Call(0x0A0000A7).Op(0xDC);b.Label("after").Load(3).Op(0x2A);
        N116_Finally(b,0x0600060A,("try","finally","after"));return b.Finish();
    }

    private static byte[] N116_EnemyManagerStart()
    {
        var b=new IL();b.Arg(0).Field(0x0400021E).Field(0x040003B4).Field(0x040001A0).Virtual(N116_BoardEntryEnum).Store(0);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(0).Call(N116_BoardEntryCurrent).Store(1);
        b.Load(1).Field(0x040007FA).Store(6);
        b.Load(6).Integer(25).Branch(0x3B,"add");b.Load(6).Integer(22).Branch(0x3B,"unselected");
        b.Load(6).Integer(1).Branch(0x3B,"selected");b.Load(6).Integer(23).Branch(0x40,"move");
        b.Label("selected").Load(1).Field(0x040007FC).Branch(0x3A,"add");b.Load(1).Field(0x040007FD).Branch(0x3A,"add").Branch(0x38,"move");
        b.Label("unselected").Load(1).Field(0x040007FC).Branch(0x3A,"move");b.Load(1).Field(0x040007FD).Branch(0x3A,"move");
        b.Label("add").Arg(0).Field(0x0400020F).Load(1).Virtual(0x0A0000A6);
        b.Label("move").Address(0).Call(N116_BoardEntryMove).Branch(0x3A,"body").Branch(0xDD,"setup");
        b.Label("finally").Address(0).Call(0x0A0000A7).Op(0xDC);
        b.Label("setup").Arg(0).Call(0x060001B2).Store(2);b.Arg(0).Call(0x060001B7);b.Integer(0).Store(3);
        b.Label("flagTest").Load(3).Arg(0).Field(0x04000210).Branch(0x3C,"doneFlags");
        b.Arg(0).Field(0x0400020B).Load(3).Virtual(0x0A00013D).Store(4);b.Integer(0).Store(5);
        b.Label("waveTest").Load(5).Load(4).Field(0x040001F3).Virtual(N116_WaveCount).Branch(0x3C,"nextFlag");
        b.Load(4).Field(0x040001F3).Load(5).Virtual(0x0A00013F);N116_NonNull(b,"waveNotNull");b.Store(7);
        b.Load(2).Load(3).Load(5).Load(7).Virtual(0x060001A8);b.Load(5).Integer(1).Op(0x58).Store(5).Branch(0x38,"waveTest");
        b.Label("nextFlag").Load(3).Integer(1).Op(0x58).Store(3).Branch(0x38,"flagTest");
        b.Label("doneFlags").Arg(0).Integer(0).Set(0x04000212);b.Arg(0).Arg(0).Field(0x0400021A).Set(0x04000216);b.Arg(0).Call(0x060001BD).Op(0x2A);
        N116_Finally(b,0x060001AF,("try","finally","setup"));return b.Finish();
    }

    private static void N116_NonNull(IL b,string label)
    { b.Op(0x25).Branch(0x3A,label).Op(0x26).New(0x0A000022).Op(0x7A).Label(label); }

    private static byte[] N116_AstarProcess()
    {
        var b=new IL();b.Arg(3).Arg(4).Arg(5).Arg(2).Call(0x060002A1).Virtual(N116_NodeEnum).Store(0);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(0).Call(N116_NodeCurrent).Store(1);
        b.Load(1).Branch(0x39,"move");b.Load(1).Field(0x040003A3).Branch(0x3A,"move");b.Load(1).Field(0x0400039F).Arg(6).Branch(0x3D,"move");
        b.Arg(1).Op(0x50).Load(1).Virtual(N116_NodeIndexOf).Integer(-1).Branch(0x40,"cost");b.Arg(1).Op(0x50).Load(1).Virtual(0x0A0001B3);
        b.Label("cost").Arg(2).Field(0x040003A0).Integer(1).Op(0x58).Store(2);b.Load(2).Load(1).Field(0x040003A0).Branch(0x3C,"move");
        b.Load(1).Load(2).Set(0x040003A0);b.Load(1).Load(2).Load(1).Field(0x040003A1).Op(0x58).Set(0x040003A2);b.Load(1).Arg(2).Set(0x040003A4);
        b.Label("move").Address(0).Call(N116_NodeMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(0).Call(0x0A0001B4).Op(0xDC);
        b.Label("after").Arg(1).Op(0x50).Arg(2).Virtual(N116_NodeIndexOf).Integer(0).Branch(0x3F,"close");
        b.Arg(1).Op(0x50).Arg(2).Virtual(N116_NodeRemove).Op(0x26);
        b.Label("close").Arg(2).Integer(1).Set(0x040003A3).Op(0x2A);
        N116_Finally(b,0x06000283,("try","finally","after"));return b.Finish();
    }

    private static byte[] N116_SaveCompleteness()
    {
        var b=new IL();b.Arg(0).Field(0x04000639).Virtual(N116_PlantSaveEnum).Store(0);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(0).Call(N116_PlantSaveCurrent).Store(1);
        b.Static(0x0400015E).Field(0x04000166).Load(1).Virtual(0x06000249);
        b.Label("move").Address(0).Call(N116_PlantSaveMove).Branch(0x3A,"body").Branch(0xDD,"sort");
        b.Label("finally").Address(0).Call(0x0A000053).Op(0xDC);
        b.Label("sort").Integer(0).Store(2);b.Integer(0).Store(3);
        b.Label("sortTest").Load(3).Arg(0).Field(0x0400063D).Virtual(0x0A0000C2).Integer(1).Op(0x59).Branch(0x3C,"sortEnd");
        b.Arg(0).Field(0x0400063D).Load(3).Virtual(0x0A0000C3).Field(0x04000678).Store(4);
        b.Arg(0).Field(0x0400063D).Load(3).Integer(1).Op(0x58).Virtual(0x0A0000C3).Field(0x04000678).Load(4).Branch(0x3C,"sortNext");
        b.Arg(0).Field(0x0400063D).Load(3).Virtual(0x0A0000C3).Store(5);
        b.Arg(0).Field(0x0400063D).Store(6);b.Load(6).Load(3).Load(6).Load(3).Integer(1).Op(0x58).Virtual(0x0A0000C3).Virtual(N116_SuppliesSetItem);
        b.Arg(0).Field(0x0400063D).Load(3).Integer(1).Op(0x58).Load(5).Virtual(N116_SuppliesSetItem);b.Integer(1).Store(2);
        b.Label("sortNext").Load(3).Integer(1).Op(0x58).Store(3).Branch(0x38,"sortTest");
        b.Label("sortEnd").Load(2).Branch(0x3A,"sort").Op(0x2A);
        N116_Finally(b,0x060004BF,("try","finally","sort"));return b.Finish();
    }

    private static byte[] N116_Warehouse()
    {
        var b=new IL();b.Arg(0).Field(0x04000849).Call(0x0A00000B).Branch(0x3A,"existing");
        b.Static(0x04000307).Store(0);b.Load(0).Call(0x0A00000B).Branch(0x3A,"create");
        b.Ref(0x72,0x7001B985).Call(0x0A000010).Op(0x2A);
        b.Label("create").Arg(0).Load(0).Call(0x2B00008A).Set(0x04000849).Branch(0x38,"setup");
        b.Label("existing").Arg(0).Integer(1).Call(0x06000643);
        b.Label("setup").Arg(0).Field(0x04000849).Virtual(0x0A000009).Integer(1).Virtual(0x0A000007);
        b.Arg(0).Field(0x04000849).Arg(0).Set(0x040008E8);
        b.Arg(0).Field(0x04000849).Store(1);b.Static(0x0400015E).Field(0x04000166).Field(0x04000310).Store(2);
        b.Load(1);N116_NonNull(b,"warehouseNotNull");b.Op(0x26);b.Load(1).Virtual(0x060006EA);
        b.Load(1).Field(0x040008EB).Store(3);b.Load(3).Op(0x14).Set(0x040008ED);b.Load(3).Op(0x14).Set(0x040008EE);b.Load(3).Virtual(0x060006F1);
        b.Load(1).Load(2).Set(0x040008E7);
        b.Load(1).Field(0x040008EC).Load(2).FieldAddress(0x0400063B).Ref(0x72,0x70018DA1).Call(0x0A000082).Integer(1).Virtual(0x0A00004B);
        b.Load(1).Virtual(0x060006EB);b.Arg(0).Integer(1).Call(0x06000643).Op(0x2A);return b.Finish();
    }

    private static byte[] N116_SetPreZombie()
    {
        var b=new IL();b.Arg(0).Field(0x0400021E).Field(0x040003B4).Field(0x0400019D).Virtual(N116_EnemyEnum).Store(0);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(0).Call(N116_EnemyCurrent).Store(1);
        b.Load(1).Field(0x040001FE).Static(0x0A000013).Call(0x0A0000B0).Branch(0x39,"selectWave");
        b.Arg(0).Field(0x0400020F).Load(1).Field(0x040001FE).Call(0x0600060A).Branch(0x39,"selectWave");
        b.Ref(0x72,0x700060A7).Call(0x0A000010).Branch(0x38,"move");
        b.Label("selectWave").Op(0x14).Store(2);b.Load(1).Field(0x040001FB).Branch(0x39,"position");
        b.Arg(0).Load(1).Field(0x040001FB).Call(0x060001BB).Store(2);b.Load(2).Branch(0x39,"move");
        b.Label("position").Arg(0).Field(0x0400021E).Field(0x040003B4).Load(1).Field(0x040001F9).Load(1).Field(0x040001F8).Virtual(0x0600015B).Store(3);
        b.Address(3).Op(0x25).Field(0x0A00001D).Float(57).Op(0x58,0x6B).Set(0x0A00001D);
        b.Address(3).Op(0x25).Field(0x0A00008B).Float(28).Op(0x58,0x6B).Set(0x0A00008B);b.Load(1).Load(3).Set(0x040001FA);
        b.Load(1).Field(0x040001FB).Branch(0x39,"dispatch");b.Load(2).Field(0x040001F5).Load(1).Virtual(N116_EnemyAdd).Branch(0x38,"move");
        b.Label("dispatch").Arg(0).Load(1).Call(0x060001B6).Op(0x26);
        b.Label("move").Address(0).Call(N116_EnemyMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(0).Call(0x0A000035).Op(0xDC);b.Label("after").Op(0x2A);
        N116_Finally(b,0x060001BD,("try","finally","after"));return b.Finish();
    }

    private static byte[] N116_SeekManyZombies()
    {
        var b=new IL();b.New(0x0A0000D8).Store(0);b.Arg(0).Field(0x040004F3).Field(0x040003D5).Field(0x04000362).Virtual(N116_ZombieEnum).Store(1);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(1).Call(0x0A0000DA).Store(2);
        b.Load(2).Virtual(0x06000431).Branch(0x39,"move");b.Load(2).Virtual(0x06000464).Branch(0x3A,"move");
        b.Load(2).Field(0x040005F1).Arg(0).Field(0x040004C8).Branch(0x3B,"move");b.Arg(2).Load(2).Virtual(0x2B000055).Branch(0x39,"move");
        b.Load(0).Virtual(N116_ZombieCount).Arg(1).Branch(0x3F,"add");b.Integer(0).Store(3);
        b.Label("insertTest").Load(3).Arg(1).Branch(0x3C,"move");b.Load(2).Field(0x040005AD).Store(4);
        b.Load(0).Load(3).Virtual(0x0A00006C).Field(0x040005AD).Load(4).Branch(0x43,"insertNext");
        b.Load(0).Load(3).Virtual(0x0A00006C).Store(5);b.Load(0).Load(3).Load(2).Virtual(N116_ZombieSetItem);b.Load(5).Store(2);
        b.Label("insertNext").Load(3).Integer(1).Op(0x58).Store(3).Branch(0x38,"insertTest");
        b.Label("add").Load(0).Load(2).Virtual(0x0A0000DB);
        b.Label("move").Address(1).Call(0x0A0000DC).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(1).Call(0x0A00015E).Op(0xDC);b.Label("after").Load(0).Op(0x2A);
        N116_Finally(b,0x06000362,("try","finally","after"));return b.Finish();
    }

    private static byte[] N116_SeekManyDevices()
    {
        var b=new IL();b.Arg(0).Field(0x040004AF).Store(0);b.Integer(0).Store(1);
        b.Arg(0).Field(0x040004D2).Branch(0x39,"range");b.Arg(0).Field(0x040004D1).Field(0x0400068E).Store(1);
        b.Label("range").Load(0).Load(1).Virtual(0x060005BD).Store(2);b.New(0x0A0000CB).Store(3);
        b.Arg(0).Field(0x040004F3).Field(0x040003D8).Field(0x040001D6).Virtual(N116_DeviceEnum).Store(4);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(4).Call(N116_DeviceCurrent).Store(5);
        b.Integer(-1).Store(1);
        foreach(var x in new[]{(Plant:4,Damage:15),(Plant:5,Damage:1),(Plant:7,Damage:24)})
        {b.Arg(0).Field(0x040004B1).Integer(x.Plant).Branch(0x40,"nextID"+x.Plant);b.Integer(x.Damage).Store(1).Branch(0x38,"damage").Label("nextID"+x.Plant);}
        b.Label("damage").Arg(0).Op(0x14).Load(1).Integer(0).Call(0x0600036D).Store(6);
        foreach(int id in new[]{2,7,8,12,14})b.Load(5).Field(0x0400046F).Integer(id).Branch(0x3B,"move");
        b.Load(5).Field(0x0400046F).Integer(11).Branch(0x3B,"bonfire");b.Load(5).Field(0x0400047D).Branch(0x3A,"move").Branch(0x38,"camp");
        b.Label("bonfire").Load(6).Field(0x04000125).Integer(3).Branch(0x40,"move");
        b.Label("camp").Load(5).Field(0x0400046E).Arg(0).Field(0x040004C8).Branch(0x3B,"move");b.Load(2).Load(5).Virtual(0x2B00005C).Branch(0x39,"move");
        b.Load(3).Virtual(N116_DeviceCount).Arg(1).Branch(0x3F,"add");b.Integer(0).Store(7);
        b.Label("insertTest").Load(7).Load(3).Virtual(N116_DeviceCount).Branch(0x3C,"move");b.Load(5).Field(0x04000466).Store(8);
        b.Load(3).Load(7).Virtual(0x0A000127).Field(0x04000466).Load(8).Branch(0x43,"insertNext");
        b.Load(3).Load(7).Load(5).Virtual(N116_DeviceSetItem).Branch(0x38,"move");
        b.Label("insertNext").Load(7).Integer(1).Op(0x58).Store(7).Branch(0x38,"insertTest");
        b.Label("add").Load(3).Load(5).Virtual(0x0A0000CE);
        b.Label("move").Address(4).Call(N116_DeviceMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(4).Call(0x0A000129).Op(0xDC);b.Label("after").Load(3).Op(0x2A);
        N116_Finally(b,0x06000364,("try","finally","after"));return b.Finish();
    }

    private static byte[] N116_SkillFunction()
    {
        var b=new IL();b.Ref(0x72,0x70016385).Call(0x0A000010);b.Integer(0).Store(0);
        b.Arg(1).Call(0x0A00000B).Branch(0x39,"lookup");b.Arg(1).Field(0x040004B1).Integer(3).Op(0x5A).Arg(0).Field(0x0400068E).Op(0x58).Store(0);
        b.Label("lookup").Static(0x040006A1).Load(0).Address(1).Virtual(N116_FunctionTryGet).Op(0x26);
        b.Load(1).Branch(0x39,"duration");b.Load(1).Arg(1).Arg(2).Arg(3).Arg(4).Virtual(0x060004F2);
        b.Label("duration").Arg(1).Call(0x0A00000B).Branch(0x39,"after");b.Arg(0).Field(0x04000691).Branch(0x3A,"after");
        b.Arg(1).Field(0x040004D3).Float(1).Op(0x59,0x6B).Store(2);b.Arg(1).Load(2).Set(0x040004D3);
        b.Load(2).Float(0).Branch(0x42,"after");
        // Original native inlines Skill.End here. Its preserved restored body owns the same ordered side effects.
        b.Arg(0).Arg(1).Arg(2).Arg(3).Arg(4).Call(0x060004E4);b.Label("after").Op(0x2A);return b.Finish();
    }

    private static byte[] N116_LoadReward()
    {
        var b=new IL();b.Arg(0).Field(0x040001A2).Virtual(N116_SuppliesEnum).Store(0);
        b.Label("try1").Branch(0x38,"move1");b.Label("body1").Arg(1).Address(0).Call(N116_SuppliesCurrent).Virtual(0x0A0000BF);
        b.Label("move1").Address(0).Call(N116_SuppliesMove).Branch(0x3A,"body1").Branch(0xDD,"second");
        b.Label("finally1").Address(0).Call(0x0A00011C).Op(0xDC);
        b.Label("second").Arg(0).Field(0x040001A3).Virtual(N116_PlantSaveEnum).Store(1);
        b.Label("try2").Branch(0x38,"move2");b.Label("body2").Address(1).Call(N116_PlantSaveCurrent).Call(0x060004DA).Store(2);b.Arg(1).Load(2).Virtual(0x0A0000BF);
        b.Label("move2").Address(1).Call(N116_PlantSaveMove).Branch(0x3A,"body2").Branch(0xDD,"after");
        b.Label("finally2").Address(1).Call(0x0A000053).Op(0xDC);b.Label("after").Op(0x2A);
        N116_Finally(b,0x06000163,("try1","finally1","second"),("try2","finally2","after"));return b.Finish();
    }

    private static byte[] N116_DeviceDamage()
    {
        var b=new IL();b.New(0x06000112).Store(0);b.Load(0).Arg(0).Field(0x0400046E).Set(0x04000124);
        b.Arg(0).Field(0x0400046F).Integer(2).Branch(0x40,"not2");b.Load(0).Integer(4).Set(0x04000125).Branch(0x38,"range");
        b.Label("not2").Arg(0).Field(0x0400046F).Integer(6).Branch(0x3B,"fire");
        b.Arg(0).Field(0x0400046F).Integer(14).Branch(0x40,"range");b.Arg(1).Op(0x14).Call(0x0A00000C).Branch(0x39,"range");
        b.Arg(1).Field(0x0400054C).Integer(31).Branch(0x40,"projectile32");
        b.Load(0).Integer(3).Set(0x04000125);b.Load(0).Integer(1).Set(0x04000129);b.Load(0).Integer(1).Set(0x0400012B);b.Load(0).Integer(0).Set(0x04000124);
        b.Label("projectile32").Arg(1).Field(0x0400054C).Integer(32).Branch(0x40,"range");b.Load(0).Integer(0).Set(0x04000124);
        b.Label("fire").Load(0).Integer(1).Set(0x0400012B);b.Load(0).Integer(1).Set(0x04000129);b.Load(0).Integer(3).Set(0x04000125);
        b.Label("range").Op(0x14).Store(1);
        b.Arg(0).Field(0x0400046F).Integer(2).Branch(0x40,"range6");
        b.Arg(0).Field(0x04000494).Integer(0).Virtual(0x060005BD).Store(1).Branch(0x38,"rangeDone");
        b.Label("range6").Arg(0).Field(0x0400046F).Integer(6).Branch(0x40,"range14");
        b.Arg(0).Arg(0).Virtual(0x0A000001).Float(225).Call(N116_DeviceCircle).Store(1).Branch(0x38,"rangeDone");
        b.Label("range14").Arg(0).Field(0x0400046F).Integer(14).Branch(0x40,"rangeDone");
        b.Arg(1).Op(0x14).Call(0x0A00000C).Branch(0x39,"rangeDone");
        b.Arg(1).Field(0x0400054C).Integer(31).Branch(0x40,"range32");
        b.Arg(1).Arg(1).Virtual(0x0A000001).Float(N116_Range31).Call(0x2B000056).Store(1);
        b.Label("range32").Arg(1).Field(0x0400054C).Integer(32).Branch(0x40,"rangeDone");
        b.Arg(1).Arg(1).Virtual(0x0A000001).Float(N116_Range32).Call(0x2B000056).Store(1);
        b.Label("rangeDone").Load(0).Load(1).Set(0x0400012E);b.Arg(0).Field(0x04000477).Store(2);
        b.Arg(0).Field(0x0400046F).Integer(14).Branch(0x40,"amountDone");
        b.Arg(1).Op(0x14).Call(0x0A00000C).Branch(0x39,"amountDone");
        b.Arg(1).Field(0x0400054C).Integer(31).Branch(0x40,"amount32");b.Float(N116_Damage31).Store(2).Branch(0x38,"amountDone");
        b.Label("amount32").Arg(1).Field(0x0400054C).Integer(32).Branch(0x40,"amountDone");b.Float(N116_Damage32).Store(2);
        b.Label("amountDone").Load(0).Load(2).Set(0x04000123);b.Load(0).Arg(0).Set(0x04000136);b.Load(0).Arg(0).Field(0x04000463).Set(0x04000137);
        b.Load(0).Op(0x2A);return b.Finish();
    }

    private static byte[] N116_Awake148()
    {
        var b=new IL();b.Arg(0).Integer(1).Set(0x040004C4);b.Arg(0).Integer(1).Set(0x040004D5);b.Arg(0).Virtual(0x06000395);
        b.Static(0x04000278).Integer(36).Virtual(0x060001EA).Store(0);
        b.Load(0).Op(0x14).Call(0x0A00000C).Branch(0x39,"after");
        b.Load(0).Virtual(0x0A00002E).Arg(0).Field(0x040004DE).Virtual(0x0A000038);
        b.Load(0).Virtual(0x2B00000F).Arg(0).Set(0x040000CD);
        b.Arg(0).Virtual(0x0A000001).Store(1);b.Arg(0).Field(0x040004DE).Store(2);
        b.Arg(0).Virtual(0x0A000001).Virtual(0x0A000037).Store(3);
        b.Arg(0).Integer(999).Arg(0).Load(1).Float(225).Load(2).Load(3).Call(0x0A0001F9).Call(N116_PlantCircleOffset).Virtual(0x06000362).Virtual(N116_ZombieEnum).Store(4);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(4).Call(0x0A0000DA).Float(3).Virtual(0x0600047E);
        b.Label("move").Address(4).Call(0x0A0000DC).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(4).Call(0x0A00015E).Op(0xDC);b.Label("after").Op(0x2A);
        N116_Finally(b,0x06000514,("try","finally","after"));return b.Finish();
    }

    private static byte[] N116_CreateSuppliesItem()
    {
        var b=new IL();b.Static(0x040002FD).Op(0x14).Call(0x0A00000C).Branch(0x3A,"create");b.Op(0x14,0x2A);
        b.Label("create").Static(0x040002FD).Call(0x2B000068).Store(0);b.Load(0).Arg(0).Virtual(0x06000413);
        b.Load(0).Field(0x04000590).Field(0x0400067A).Store(1);
        b.Load(0).Field(0x04000596).Address(1).Call(0x0A00002A).Virtual(0x0A000062);
        b.Load(0).Field(0x04000595).Virtual(0x0A00023D).Store(2);
        // The original performs Log10 even for zero and negatives, then unchecked cvttsd2si and Int32 arithmetic.
        b.Load(2);N116_NonNull(b,"rectNotNull");b.Op(0x26);
        b.Load(2).Integer(0).Load(1).Op(0x6C).Call(0x0A00023E).Op(0x69).Integer(14).Op(0x5A).Integer(39).Op(0x58,0x6B).Virtual(0x0A00023F);
        b.Load(1).Branch(0x3A,"countVisibility");
        foreach(int f in new[]{0x04000592,0x04000593,0x04000594})
            b.Load(0).Field(f).Float(N116_EmptyColorR).Float(N116_EmptyColorG).Float(N116_EmptyColorB).Float(N116_EmptyColorA).New(0x0A0000BE).Virtual(0x0A0001DE);
        b.Label("countVisibility").Load(0).Field(0x04000595).Virtual(0x0A000009).Arg(1).Virtual(0x0A000007);
        b.Arg(0).Field(0x04000678).Integer(50).Branch(0x3B,"hide");b.Arg(0).Field(0x04000678).Integer(51).Branch(0x40,"after");
        b.Label("hide").Load(0).Field(0x04000595).Virtual(0x0A000009).Integer(0).Virtual(0x0A000007);
        b.Label("after").Load(0).Op(0x2A);return b.Finish();
    }

    private static byte[] N116_HoleTeleport()
    {
        var b=new IL();b.Arg(0).Field(0x0400013E).Field(0x040005AA).Field(0x040003B4).Arg(0).Field(0x0400013F).Arg(0).Field(0x04000140).Virtual(0x0600015B).Store(0);
        b.Address(0).Op(0x25).Field(0x0A00001D).Float(57).Op(0x58,0x6B).Set(0x0A00001D);
        b.Address(0).Op(0x25).Field(0x0A00008B).Float(28).Op(0x58,0x6B).Set(0x0A00008B);
        b.Ref(0x72,0x700044C3).Arg(0).FieldAddress(0x0400013F).Call(0x0A00002A).Ref(0x72,0x700044CD).Arg(0).FieldAddress(0x04000140).Call(0x0A00002A).Call(0x0A0000E0).Call(0x0A000010);
        b.Load(0).Ref(0x8C,0x01000013).Call(0x0A000010);
        b.Arg(0).Field(0x0400013E).Load(0).Address(0).Field(0x0A00009D).Integer(1).Virtual(0x06000480);
        b.Integer(0).Arg(0).Field(0x0400013E).New(0x060000FD).Store(1);
        b.Load(1).Float(.5f).Set(0x04000107);b.Load(1).Integer(0).Set(0x04000106);b.Load(1).Integer(0).Set(0x04000108);
        b.Arg(0).Field(0x0400013E).Field(0x0400061A).Load(1).Virtual(0x06000102);
        b.Static(0x040002EF).Integer(6).Virtual(0x0A000084).Store(2);
        b.Call(0x0A000004).Virtual(0x0A000001).Virtual(0x0A000037).Store(3);
        b.Load(2).Load(3).Call(0x06000138).Float(N116_TeleportVolume).Op(0x5A,0x6B).Float(N116_TeleportPitch).Call(0x0600013E).Op(0x26,0x2A);return b.Finish();
    }

    private static byte[] N116_LoadInfoAll(bool zombie)
    {
        var b=new IL();b.New(zombie?0x0A000016:0x0A000187).Store(0);
        b.Ref(0xD0,zombie?0x0200005D:0x0200003D).Call(0x0A00004C).Call(0x0A00007D).Virtual(0x0A0000EB).Store(1);
        b.Label("try").Branch(0x38,"move");
        b.Label("body").Load(1).Virtual(0x0A0000ED).Ref(0xA5,zombie?0x0200005D:0x0200003D).Call(zombie?0x06000230:0x06000225).Store(3);
        b.Load(0).Load(3).Virtual(zombie?N116_ZombieInfoAdd:N116_DeviceInfoAdd);
        b.Label("move").Load(1).Virtual(0x0A0000EC).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally");N112_DisposeEnumerator(b,1,2);b.Op(0xDC);b.Label("after").Load(0).Op(0x2A);
        N116_Finally(b,zombie?0x06000231:0x06000226,("try","finally","after"));return b.Finish();
    }

    private static byte[] N116_ExpandRangeView()
    {
        var b=new IL();b.Arg(0).Field(0x040000DD).Op(0x14).Call(0x0A00000C).Branch(0x39,"after");
        b.Arg(0).Field(0x040000DD).Virtual(0x0A000009).Integer(1).Virtual(0x0A000007);
        b.Arg(0).Field(0x040000DD).Virtual(0x0A000090).Store(0);
        b.Label("try").Branch(0x38,"move");b.Label("body").Load(0).Virtual(0x0A0000ED).Ref(0x74,0x01000005).Virtual(0x2B000012).Store(2);
        b.Load(2).Call(0x0A00000B).Branch(0x39,"move");
        b.Arg(1).Branch(0x3A,"selected");b.Float(.5f).Store(3).Branch(0x38,"color");b.Label("selected").Float(0).Store(3);
        b.Label("color").Load(2).Float(1).Load(3).Float(0).Float(.4f).New(0x0A0000BE).Virtual(0x0A000097);
        b.Label("move").Load(0).Virtual(0x0A0000EC).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally");N112_DisposeEnumerator(b,0,1);b.Op(0xDC);b.Label("after").Op(0x2A);
        N116_Finally(b,0x060000D3,("try","finally","after"));return b.Finish();
    }

    private static byte[] N116_DisplayBannedDevice()
    {
        var b=new IL();b.Arg(0).Field(0x040001D6).Virtual(N116_DeviceEnum).Store(0);
        b.Label("outerTry").Branch(0x38,"outerMove");b.Label("outerBody").Address(0).Call(N116_DeviceCurrent).Store(1);
        b.Load(1).Field(0x0400046F).Integer(12).Branch(0x40,"outerMove");b.Load(1).Field(0x04000488).Virtual(0x0A0000FD).Store(2);
        b.Label("innerTry").Branch(0x38,"innerMove");b.Label("innerBody").Address(2).Call(0x0A0000FE).Store(3);
        b.Arg(0).Field(0x040001D8).Branch(0x3A,"shown");b.Float(.5f).Store(4).Branch(0x38,"render");b.Label("shown").Float(1).Store(4);
        b.Label("render").Load(3).Virtual(0x2B000011).Float(1).Float(1).Float(1).Load(4).New(0x0A0000BE).Virtual(0x0A000097);
        b.Label("innerMove").Address(2).Call(0x0A0000FF).Branch(0x3A,"innerBody").Branch(0xDD,"outerMove");
        b.Label("innerFinally").Address(2).Call(0x0A000128).Op(0xDC);
        b.Label("outerMove").Address(0).Call(N116_DeviceMove).Branch(0x3A,"outerBody").Branch(0xDD,"after");
        b.Label("outerFinally").Address(0).Call(0x0A000129).Op(0xDC);b.Label("after").Op(0x2A);
        N116_Finally(b,0x06000183,("innerTry","innerFinally","outerMove"),("outerTry","outerFinally","after"));return b.Finish();
    }

    private static byte[] N116_EditorMouse()
    {
        var b=new IL();b.Call(0x0A000004).Call(0x0A000023).Virtual(0x0A00001C).Store(0);
        b.Arg(0).Field(0x0400000D).Address(0).Field(0x0A00001D).Address(0).Field(0x0A00008B).Virtual(0x06000159).Store(1);
        b.Arg(0).Field(0x0400000D).Address(0).Field(0x0A00001D).Address(0).Field(0x0A00008B).Virtual(0x0600015A).Store(2);
        b.Arg(0).Field(0x04000012).Virtual(N116_ToggleIsOn).Branch(0x39,"after");
        b.Arg(0).Field(0x0400001A).Virtual(N116_DeviceEditorEnum).Store(3);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(3).Call(N116_DeviceEditorCurrent).Store(4);
        b.Load(4).Field(0x04000035).Field(0x040001D0).Load(1).Branch(0x40,"move");
        b.Load(4).Field(0x04000035).Field(0x040001D1).Load(2).Branch(0x40,"move");
        b.Integer(0).Call(0x0A00001F).Branch(0x39,"move");b.Arg(0).Field(0x04000017).Load(4).Virtual(0x06000030).Branch(0xDD,"after");
        b.Label("move").Address(3).Call(N116_DeviceEditorMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(3).Call(N116_DeviceEditorDispose).Op(0xDC);b.Label("after").Op(0x2A);
        N116_Finally(b,0x06000011,("try","finally","after"));return b.Finish();
    }

    private static byte[] N116_ResetChosenCard()
    {
        var b=new IL();b.Arg(0).Integer(0).Set(0x04000448);
        b.Arg(0).Field(0x04000436).Virtual(0x2B000054).Store(0);
        b.Load(0).Virtual(0x0A0001DD).Store(1);b.Load(0).Virtual(0x0A0001DD).Store(2);b.Load(0).Virtual(0x0A0001DD).Store(3);
        b.Load(0).Address(1).Field(0x0A0000EE).Address(2).Field(0x0A0000EF).Address(3).Field(0x0A00021B).Float(.002f).New(0x0A0000BE).Virtual(0x0A0001DE);
        b.Arg(0).Field(0x04000436).Virtual(0x0A000001).Integer(0).Virtual(0x0A000002).Virtual(0x0A000009).Integer(0).Virtual(0x0A000007);
        b.Arg(0).Field(0x04000444).Virtual(0x0A000009).Integer(0).Virtual(0x0A000007);
        b.Arg(0).Field(0x04000445).Field(0x040008B8).Virtual(N116_CardChooseEnum).Store(4);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(4).Call(N116_CardChooseCurrent).Store(5);
        b.Load(5).Field(0x04000448).Branch(0x39,"move");b.Load(5).Field(0x04000449).Arg(0).Field(0x04000449).Branch(0x3E,"move");
        b.Load(5).Load(5).Field(0x04000449).Integer(1).Op(0x59).Set(0x04000449);
        b.Load(5).Field(0x04000444).Load(5).FieldAddress(0x04000449).Call(0x0A00002A).Virtual(0x0A000062);
        b.Label("move").Address(4).Call(N116_CardChooseMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(4).Call(0x0A0001DF).Op(0xDC);
        b.Label("after").Arg(0).Integer(0).Set(0x04000449);b.Arg(0).Field(0x04000445).Field(0x040008BE).Op(0x14).Virtual(0x060006C2).Op(0x2A);
        N116_Finally(b,0x060002F5,("try","finally","after"));return b.Finish();
    }

    private static byte[] N116_CopyEnemy()
    {
        var b=new IL();b.Arg(0).Static(0x0A000013).Set(0x040001FE);b.Arg(0).Call(0x0A0000B6);
        b.Arg(0).New(0x0A000130).Set(0x040001FD);b.Arg(1).Branch(0x39,"after");
        foreach(int f in new[]{0x040001F7,0x040001F8,0x040001F9,0x040001FA,0x040001FB,0x040001FC,0x040001FE})b.Arg(0).Arg(1).Field(f).Set(f);
        b.Arg(0).Field(0x040001FD).Virtual(N116_PathClear);b.Arg(1).Field(0x040001FD).Virtual(N116_PathEnum).Store(0);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(0).Call(N116_PathCurrent).Store(1);
        b.Load(1).Field(0x040003A5).Load(1).Field(0x040003A6).Load(1).Field(0x040003A8).Op(0x14).New(0x060002A4).Store(2);
        b.Load(2).Load(1).Field(0x040003AA).Set(0x040003AA);b.Arg(0).Field(0x040001FD).Load(2).Virtual(N116_PathAdd);
        b.Label("move").Address(0).Call(N116_PathMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(0).Call(0x0A000045).Op(0xDC);b.Label("after").Op(0x2A);
        N116_Finally(b,0x060001A1,("try","finally","after"));return b.Finish();
    }

    private static byte[] N116_NutTaunt()
    {
        var b=new IL();b.Static(0x04000278).Integer(16).Virtual(0x060001EA).Virtual(0x0A00002E);
        b.Arg(0).Field(0x040004F3).Field(0x040003B4).Arg(0).Field(0x040004A0).Integer(1).Op(0x58).Arg(0).Field(0x040004A1).Virtual(0x0600015C).Virtual(0x0A000038);
        b.Arg(0).Field(0x040004F3).Field(0x040003D5).Field(0x04000362).Virtual(N116_ZombieEnum).Store(0);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(0).Call(0x0A0000DA).Store(1);
        b.Load(1).Virtual(0x06000431).Branch(0x39,"move");b.Load(1).Virtual(0x06000417).Branch(0x3A,"move");
        b.Load(1).Field(0x040005AB).Arg(0).Field(0x040004A0).Branch(0x3F,"move");
        b.Load(1).Field(0x040005AB).Arg(0).Field(0x040004A0).Op(0x59).Integer(2).Branch(0x3D,"move");
        b.Load(1).Field(0x040005AC).Arg(0).Field(0x040004A1).Op(0x59).Store(2);
        // Original neg/cmov leaves Int32.MinValue negative; Math.Abs(int) would add an exception.
        b.Load(2).Integer(0).Branch(0x3C,"absolute");b.Load(2).Op(0x65).Store(2);
        b.Label("absolute").Load(2).Integer(1).Branch(0x3D,"move");
        b.Arg(0).Field(0x040004A0).Store(3);b.Arg(0).Field(0x040004A1).Store(4);b.Load(1).Field(0x040005F6).Store(5);
        b.Load(3).Load(4).Load(5).Arg(0).Field(0x040004F3).Field(0x040003B4).New(0x060002A4).Store(6);
        b.Load(6).Integer(1).Set(0x040003AB);b.Load(6).Arg(0).Set(0x040003AD);b.Load(1).Float(.01f).Set(0x040005F6);
        b.Load(1).Load(6).Virtual(0x06000463);b.Load(1).Virtual(0x0600046A);
        b.Label("move").Address(0).Call(0x0A0000DC).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(0).Call(0x0A00015E).Op(0xDC);b.Label("after").Op(0x2A);
        N116_Finally(b,0x06000384,("try","finally","after"));return b.Finish();
    }

    private static byte[] N116_GridCanPlace()
    {
        var b=new IL();b.Arg(0).Field(0x04000383).Integer(3).Branch(0x40,"notWater");
        b.Arg(1).Virtual(0x06000312).Branch(0x3A,"waterDevice");
        b.Arg(0).Field(0x04000388).Op(0x14).Call(0x0A00000C).Branch(0x3A,"lily");
        b.Arg(0).Field(0x0400038C).Op(0x14).Call(0x0A00000C).Branch(0x39,"false").Branch(0x38,"slots");
        b.Label("lily").Arg(0).Field(0x04000388).Field(0x040004B0).Integer(16).Branch(0x40,"false").Branch(0x38,"slots");
        b.Label("waterDevice").Arg(0).Field(0x04000388).Op(0x14).Call(0x0A00000C).Branch(0x3A,"false");
        b.Arg(0).Field(0x0400038C).Op(0x14).Call(0x0A00000C).Branch(0x3A,"false").Branch(0x38,"slots");
        b.Label("notWater").Arg(0).Field(0x04000383).Integer(5).Branch(0x3B,"false");
        b.Label("slots").Op(0x14).Store(0);b.Op(0x14).Store(1);b.Arg(1).Field(0x04000473).Store(2);
        foreach(var x in new[]{(Layer:0,Field:0x04000388),(Layer:1,Field:0x04000389),(Layer:2,Field:0x0400038A),(Layer:3,Field:0x0400038B)})
        {b.Load(2).Integer(x.Layer).Branch(0x40,"plantNext"+x.Layer);b.Arg(0).Field(x.Field).Store(0).Branch(0x38,"deviceSlot").Label("plantNext"+x.Layer);}
        b.Label("deviceSlot").Arg(1).Field(0x04000473).Store(2);
        foreach(var x in new[]{(Layer:0,Field:0x0400038C),(Layer:1,Field:0x0400038D),(Layer:2,Field:0x0400038F),(Layer:3,Field:0x04000390),(Layer:4,Field:0x0400038E)})
        {b.Load(2).Integer(x.Layer).Branch(0x40,"deviceNext"+x.Layer);b.Arg(0).Field(x.Field).Store(1).Branch(0x38,"testSlots").Label("deviceNext"+x.Layer);}
        b.Label("testSlots").Load(0).Op(0x14).Call(0x0A00000C).Branch(0x3A,"false");b.Load(1).Op(0x14).Call(0x0A00000C).Branch(0x3A,"false");
        b.Arg(0).Field(0x0400038C).Store(1);b.Load(1).Call(0x0A00000B).Branch(0x39,"plantBear");
        b.Load(1).Arg(1).Field(0x04000473).Virtual(0x06000311).Branch(0x39,"false");
        b.Label("plantBear").Arg(0).Field(0x04000388).Store(0);b.Load(0).Call(0x0A00000B).Branch(0x39,"true");
        b.Load(0).Arg(1).Field(0x04000473).Virtual(N116_PlantCanBear).Branch(0x39,"false");
        b.Label("true").Integer(1).Op(0x2A);b.Label("false").Integer(0).Op(0x2A);return b.Finish();
    }

    private static byte[] N116_SelectZombie()
    {
        var b=new IL();b.Arg(0).Arg(0).Field(0x04000916).Integer(0).Op(0xFE,0x01).Set(0x04000916);
        b.Arg(0).Field(0x04000913).Op(0x14).Call(0x0A00000C).Branch(0x39,"after");
        b.Arg(0).Field(0x04000912).Field(0x040000A6).Field(0x04000196).Store(0);b.Integer(-1).Store(1);b.Integer(0).Store(2);
        b.Label("find").Load(2).Load(0).Virtual(N116_IntCount).Branch(0x3C,"found");
        b.Load(0).Load(2).Virtual(0x0A00006B).Arg(0).Field(0x04000913).Field(0x040005B9).Branch(0x40,"findNext");b.Load(2).Store(1);
        b.Label("findNext").Load(2).Integer(1).Op(0x58).Store(2).Branch(0x38,"find");
        b.Label("found").Arg(0).Field(0x04000916).Branch(0x39,"remove");b.Load(1).Integer(-1).Branch(0x40,"after");
        b.Load(0).Virtual(N116_IntCount).Branch(0x3A,"sortStart");b.Load(0).Arg(0).Field(0x04000913).Field(0x040005B9).Virtual(0x0A000291).Branch(0x38,"after");
        b.Label("sortStart").Integer(0).Store(2);
        b.Label("sort").Load(2).Load(0).Virtual(N116_IntCount).Branch(0x3C,"after");b.Load(2).Branch(0x3A,"notFirst");
        b.Load(0).Integer(0).Virtual(0x0A00006B).Arg(0).Field(0x04000913).Field(0x040005B9).Branch(0x3D,"insertFirst");
        b.Label("notFirst").Load(2).Load(0).Virtual(N116_IntCount).Integer(1).Op(0x59).Branch(0x40,"between");
        b.Load(0).Load(2).Virtual(0x0A00006B).Arg(0).Field(0x04000913).Field(0x040005B9).Branch(0x3E,"append");
        b.Label("between").Load(0).Load(2).Virtual(0x0A00006B).Arg(0).Field(0x04000913).Field(0x040005B9).Branch(0x3D,"sortNext");
        b.Load(0).Load(2).Integer(1).Op(0x58).Virtual(0x0A00006B).Arg(0).Field(0x04000913).Field(0x040005B9).Branch(0x3F,"sortNext");
        b.Load(0).Load(2).Integer(1).Op(0x58).Arg(0).Field(0x04000913).Field(0x040005B9).Virtual(0x0A0002CA).Branch(0x38,"after");
        b.Label("sortNext").Load(2).Integer(1).Op(0x58).Store(2).Branch(0x38,"sort");
        b.Label("insertFirst").Load(0).Integer(0).Arg(0).Field(0x04000913).Field(0x040005B9).Virtual(0x0A0002CA).Branch(0x38,"after");
        b.Label("append").Load(0).Arg(0).Field(0x04000913).Field(0x040005B9).Virtual(0x0A000291).Branch(0x38,"after");
        b.Label("remove").Load(1).Integer(-1).Branch(0x3B,"after");b.Load(0).Load(1).Virtual(0x0A0002C9);
        b.Label("after").Arg(0).Call(0x0600070F).Op(0x2A);return b.Finish();
    }

    private static byte[] N116_SkillButton()
    {
        var b=new IL();b.Arg(0).Field(0x040008C7).Arg(1).Integer(1).Op(0x59).Virtual(0x0A000006).Virtual(0x0A00002E).Integer(2).Virtual(0x0A000002).Virtual(0x0A000009).Integer(0).Virtual(0x0A000007);
        b.Arg(0).Field(0x040008C0).Op(0x14).Call(0x0A00000C).Branch(0x39,"after");
        b.Arg(0).Field(0x040008C0).Field(0x0400042F).Arg(1).Virtual(0x060004CB).Store(0);b.Load(0).Branch(0x39,"after");
        b.Load(0).Field(0x0400068E).Branch(0x39,"after");b.Load(0).Field(0x04000693).Branch(0x39,"after");
        b.Integer(0).Store(1);b.Label("buttons").Load(1).Integer(3).Branch(0x3C,"load");
        b.Arg(0).Field(0x040008C7).Load(1).Virtual(0x0A000006).Virtual(0x0A00002E).Integer(2).Virtual(0x0A000002).Virtual(0x0A000009);
        b.Load(1).Arg(1).Integer(1).Op(0x59,0xFE,0x01).Integer(0).Op(0xFE,0x01).Virtual(0x0A000007);
        b.Load(1).Integer(1).Op(0x58).Store(1).Branch(0x38,"buttons");
        b.Label("load").Arg(0).Field(0x040008C0).Field(0x0400042F).Branch(0x39,"showTexts");b.Arg(0).Load(0).Call(0x060006C6);
        b.Label("showTexts").Arg(0).Field(0x040008C5).Virtual(N116_TmpEnum).Store(2);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(2).Call(N116_TmpCurrent).Virtual(0x0A000009).Integer(1).Virtual(0x0A000007);
        b.Label("move").Address(2).Call(N116_TmpMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(2).Call(0x0A0002B0).Op(0xDC);b.Label("after").Op(0x2A);
        N116_Finally(b,0x060006C5,("try","finally","after"));return b.Finish();
    }

    private static byte[] N116_DeviceUis()
    {
        var b=new IL();b.Arg(0).Field(0x04000493).Arg(0).Virtual(0x0600033C);
        b.Arg(0).Field(0x04000492).Store(0);b.Arg(0).Field(0x04000479).Store(1);b.Arg(0).Field(0x0400047A).Store(2);
        b.Load(0).Field(0x0400049C).Op(0x14).Call(0x0A00000C).Branch(0x39,"board");
        b.Load(1).Float(0).Branch(0x3E,"hideIce");b.Load(0).Field(0x0400049C).Integer(1).Virtual(0x0A000007);
        b.Arg(0).Field(0x04000486).Call(0x0A00000B).Branch(0x39,"updateIce");b.Arg(0).Field(0x04000486).Float(0).Virtual(0x0A0001EA);
        b.Label("updateIce").Load(0).Load(1).Load(2).Arg(0).Integer(0).Virtual(0x06000342).Branch(0x38,"board");
        b.Label("hideIce").Load(0).Field(0x0400049C).Integer(0).Virtual(0x0A000007);
        b.Label("board").Arg(0).Field(0x04000463).Op(0x14).Call(0x0A00000C).Branch(0x39,"after");b.Integer(0).Store(3);
        b.Label("loop").Load(3).Arg(0).Field(0x04000490).Op(0x8E,0x69).Branch(0x3C,"hp");
        b.Arg(0).Field(0x04000490).Load(3).Op(0x9A,0x14).Call(0x0A00000C).Branch(0x39,"next");
        b.Arg(0).Field(0x04000490).Load(3).Op(0x9A).Virtual(0x0A00002E).Arg(0).Field(0x04000463).Field(0x040003C5).Virtual(0x0A00002E).Virtual(0x0A000491);
        b.Label("next").Load(3).Integer(1).Op(0x58).Store(3).Branch(0x38,"loop");
        b.Label("hp").Arg(0).Field(0x04000491).Op(0x14).Call(0x0A00000C).Branch(0x39,"after");
        b.Arg(0).Field(0x04000491).Virtual(0x0A00002E).Arg(0).Field(0x04000463).Field(0x040003C5).Virtual(0x0A00002E).Virtual(0x0A000491);
        b.Label("after").Op(0x2A);return b.Finish();
    }

    private static byte[] N116_CreatePropList()
    {
        var b=new IL();b.Integer(0).Store(0);
        b.Label("banks").Arg(0).Field(0x0400089D).Load(0).Virtual(0x0A00029D).Store(1);
        b.Load(1).Field(0x0400089F).Load(0).Arg(1).Field(0x0400062A).Op(0xFE,0x04).Virtual(0x0A000007);
        b.Load(1).Op(0x14).Virtual(0x0600069A);b.Load(0).Integer(1).Op(0x58).Store(0);b.Load(0).Integer(3).Branch(0x3F,"banks");
        b.Integer(0).Store(0);b.Arg(1).Field(0x0400063A).Virtual(N116_PropSaveEnum).Store(2);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(2).Call(N116_PropSaveCurrent).Store(3);
        b.Arg(0).Field(0x0400089C).Call(0x2B000028).Virtual(0x2B00008D).Store(4);
        b.Load(4).Arg(0).Field(0x04000899).Set(0x0400058E);b.Load(4).Arg(0).Set(0x0400058F);b.Load(4).Load(3).Virtual(0x0600040E);
        b.Arg(0).Field(0x0400089A).Load(4).Virtual(N116_PropChooseAdd);
        b.Load(4).Virtual(0x0A000001).Arg(0).Field(0x0400089B).Virtual(0x0A00002E).Integer(0).Virtual(0x0A00002F);
        b.Load(0).Op(0x6B).Float(175).Op(0x5A).Float(430).Op(0x59,0x6B).Store(5);
        b.Load(4).Virtual(0x0A000001).Load(5).Float(0).Float(0).New(0x0A00009C).Virtual(0x0A000042);
        b.Load(0).Integer(1).Op(0x58).Store(0);
        b.Label("move").Address(2).Call(N116_PropSaveMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(2).Call(0x0A000234).Op(0xDC);b.Label("after").Arg(0).Field(0x0400089B).Integer(0).Virtual(0x0A000007).Op(0x2A);
        N116_Finally(b,0x06000692,("try","finally","after"));return b.Finish();
    }

    private static byte[] N116_LoadTeamProp()
    {
        var b=new IL();b.Arg(0).Field(0x0400089D).Virtual(N116_PropBankEnum).Store(0);
        b.Label("resetTry").Branch(0x38,"resetMove");b.Label("resetBody").Address(0).Call(N116_PropBankCurrent).Op(0x14).Virtual(0x0600069A);
        b.Label("resetMove").Address(0).Call(N116_PropBankMove).Branch(0x3A,"resetBody").Branch(0xDD,"start");
        b.Label("resetFinally").Address(0).Call(N116_PropBankDispose).Op(0xDC);
        b.Label("start").Integer(0).Store(1);b.ArgAddress(1).Field(0x0400064C).Virtual(N116_IntEnum).Store(2);
        b.Label("outerTry").Branch(0x38,"outerMove");b.Label("outerBody").Address(2).Call(N116_IntCurrent).Store(3);
        b.Arg(0).Field(0x0400089A).Virtual(N116_PropChooseEnum).Store(4);
        b.Label("innerTry").Branch(0x38,"innerMove");b.Label("innerBody").Address(4).Call(N116_PropChooseCurrent).Store(5);
        b.Load(1).Arg(2).Field(0x0400062A).Branch(0x3C,"innerLeave");
        b.Load(3).Load(5).Field(0x0400058C).Branch(0x40,"innerMove");b.Arg(0).Load(5).Load(1).Call(0x06000698);b.Load(1).Integer(1).Op(0x58).Store(1).Branch(0x38,"innerLeave");
        b.Label("innerMove").Address(4).Call(N116_PropChooseMove).Branch(0x3A,"innerBody");
        b.Label("innerLeave").Branch(0xDD,"outerMove");b.Label("innerFinally").Address(4).Call(N116_PropChooseDispose).Op(0xDC);
        b.Label("outerMove").Address(2).Call(N116_IntMove).Branch(0x3A,"outerBody").Branch(0xDD,"after");
        b.Label("outerFinally").Address(2).Call(N116_IntDispose).Op(0xDC);b.Label("after").Op(0x2A);
        N116_Finally(b,0x06000693,("resetTry","resetFinally","start"),("innerTry","innerFinally","outerMove"),("outerTry","outerFinally","after"));return b.Finish();
    }

    private static byte[] N116_InputSupplies()
    {
        var b=new IL();b.Arg(1).Call(0x0A000010);b.Arg(0).Field(0x040000BC).Store(0);b.Load(0).Call(0x0600090C).Store(1);
        for(int i=0;i<N116_InputNames.Length;i++)b.Load(1).Integer(unchecked((int)N116_InputNames[i].Hash)).Branch(0x3B,"case"+i);
        b.Branch(0x38,"after");
        for(int i=0;i<N116_InputNames.Length;i++)
        {
            var x=N116_InputNames[i];b.Label("case"+i).Load(0).Ref(0x72,x.String).Call(0x0A00003A).Branch(0x39,"after");
            if(x.Kind==0)b.Arg(0).Field(0x040000BF).Arg(1).Set(x.Field);
            else if(x.Kind==1){b.Arg(0).Arg(1).Set(0x040000BE);b.Arg(0).Field(0x040000BF).Arg(1).Set(0x04000174);}
            else
            {
                b.Arg(1).Address(2).Call(0x0A00003B).Branch(0x3A,"parsed"+i);b.Ref(0x72,0x7000090C).Call(0x0A000010).Branch(0x38,"after");
                b.Label("parsed"+i);
                if(x.Kind==2)b.Arg(0).Field(0x040000BF).Load(2).Set(0x04000176);else b.Arg(0).Load(2).Call(0x0600009E);
            }
            b.Branch(0x38,"after");
        }
        b.Label("after").Op(0x2A);return b.Finish();
    }

    private static byte[] N116_LevelState()
    {
        var b=new IL();b.Static(0x0400015E).Field(0x04000166).Field(0x04000310).Store(0);b.Load(0).Branch(0x39,"after");
        b.Arg(0).Field(0x04000830).Integer(1).Branch(0x40,"after");
        b.Arg(0).Field(0x04000835).Virtual(0x0A000009).Load(0).Field(0x04000633).Arg(0).Field(0x0400082F).Op(0x94).Integer(0).Op(0xFE,0x02).Virtual(0x0A000007);
        b.Arg(0).Field(0x04000836).Virtual(0x0A000009).Load(0).Arg(0).Field(0x04000838).Arg(0).Field(0x0400082F).Virtual(0x0A00006B).Virtual(0x060004C1).Op(0x14,0xFE,0x03).Virtual(0x0A000007);
        b.Arg(0).Field(0x04000832).Ref(0x72,0x7001B91F).Integer(0).Arg(0).Field(0x0400082F).Integer(4).Op(0x5A).Call(0x06000174).Ref(0x72,0x7001B925).Call(0x0A00010B).Integer(1).Virtual(0x0A00004B);
        b.Load(0).Field(0x04000628).Arg(0).Field(0x0400082F).Integer(4).Op(0x5A).Op(0xFE,0x02).Store(1);
        b.Arg(0).Field(0x04000837).Virtual(0x0A000009).Load(1).Integer(0).Op(0xFE,0x01).Virtual(0x0A000007);
        b.Arg(0).Field(0x04000833).Load(1).Virtual(0x0A000018);
        b.Arg(0).Field(0x04000831).Virtual(0x0A000009).Load(1).Virtual(0x0A000007);
        b.Arg(0).Field(0x04000832).Virtual(0x0A000009).Load(1).Integer(0).Op(0xFE,0x01).Virtual(0x0A000007);
        b.Load(1).Branch(0x3A,"white");b.Float(.35f).Store(2).Branch(0x38,"color");b.Label("white").Float(1).Store(2);
        b.Label("color").Arg(0).Field(0x04000834).Load(2).Load(2).Load(2).Float(1).New(0x0A0000BE).Virtual(0x0A0001DE);
        b.Label("after").Op(0x2A);return b.Finish();
    }

    private static byte[] N116_PlantLevelUp()
    {
        var b=new IL();b.Arg(0).Field(0x04000717).Virtual(N116_SuppliesItemEnum).Store(0);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(0).Call(N116_SuppliesItemCurrent).Store(1);
        b.Load(1).Field(0x04000590).Store(2);b.Load(2).Load(2).Field(0x0400067A).Load(1).Field(0x04000591).Op(0x59).Set(0x0400067A);
        b.Label("move").Address(0).Call(N116_SuppliesItemMove).Branch(0x3A,"body").Branch(0xDD,"afterCost");
        b.Label("finally").Address(0).Call(N116_SuppliesItemDispose).Op(0xDC);
        b.Label("afterCost").Static(0x0400015E).Field(0x04000166).Field(0x04000310).Store(3);
        b.Load(3).Load(3).Field(0x0400063B).Arg(0).Field(0x0400071B).Op(0x59).Set(0x0400063B);
        b.Arg(0).Field(0x040006F5).Arg(0).Field(0x040006FE).Set(0x04000662);
        foreach(int f in new[]{0x04000667,0x04000668,0x04000669,0x0400066A,0x0400066B,0x04000664,0x04000665})
        {
            b.Arg(0).Field(0x040006F5).Arg(0).Field(0x04000706).Field(f).Set(f);
            if(f==0x04000667)b.Arg(0).Field(0x040006F5).Op(0x25).Field(0x04000667).Set(N116_PlantSaveHealth);
        }
        b.Arg(0).Call(0x0600057F);b.Arg(0).Arg(0).Field(0x040006F5).Field(0x04000662).Call(0x06000582);
        b.Arg(0).Field(0x040006F5).Store(4);
        b.Load(4).Field(0x04000662).Load(4).Field(0x04000661).Integer(2).Op(0x58).Integer(10).Op(0x5A).Branch(0x3F,"after");
        b.Arg(0).Field(0x04000704).Integer(0).Virtual(0x0A000018);b.Label("after").Op(0x2A);
        N116_Finally(b,0x06000572,("try","finally","afterCost"));return b.Finish();
    }

    private static void N116_PlantAudio(IL b,int index)
    {
        b.Static(0x040002F1).Integer(index).Virtual(0x0A000084).Store(0);
        b.Call(0x0A000004).Virtual(0x0A000001).Virtual(0x0A000037).Store(1);
        b.Load(0).Load(1).Call(0x06000138).Call(0x0600013D).Op(0x26);
    }

    private static byte[] N116_PlantParticleAudio()
    {
        var b=new IL();b.Arg(0).Field(0x040004F3).Arg(0).Field(0x040004A0).Arg(0).Field(0x040004A1).Virtual(0x060002C4).Field(0x04000383).Integer(3).Branch(0x40,"particle");
        N116_PlantAudio(b,6);
        b.Label("particle").Static(0x04000278).Integer(22).Virtual(0x060001EA).Store(2);
        b.Load(2).Op(0x14).Call(0x0A00000C).Branch(0x39,"after");N116_PlantAudio(b,5);
        b.Load(2).Virtual(0x0A00002E).Arg(1).Virtual(0x0A000038);
        b.Load(2).Virtual(0x0A00002E).Static(0x04000278).Virtual(0x0A000001).Virtual(0x0A000491);
        b.Load(2).Virtual(0x2B000057).Op(0x26);b.Load(2).Virtual(0x2B00005B).Ref(0x72,0x700044D1).Virtual(0x0A00015A);
        b.Load(2).Virtual(0x2B00005B).Integer(0).Virtual(0x0A00015B);b.Label("after").Op(0x2A);return b.Finish();
    }
}
