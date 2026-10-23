// Source authoring only. No candidate or project test has been executed by the author.
internal static partial class Program
{
    private static void N118_Finally(IL b,int token,params (string Try,string Handler,string After)[] regions)
    {
        Require(N118_EhTargets.Contains(token),"Native118 explicit EH scope");
        FinallyBodies.Add(token,regions.Select(x=>new FinallyClause(b.OffsetOf(x.Try),b.OffsetOf(x.Handler),b.OffsetOf(x.Handler),b.OffsetOf(x.After))).ToArray());
    }

    private static void N118_SelectLiteral(IL b,int index,int target,int[] literals,string label)
    {
        for(int i=0;i<literals.Length;i++)b.Load(index).Integer(i).Branch(0x3B,label+i);
        b.Ref(0x72,N118_Empty).Store(target).Branch(0x38,label+"End");
        for(int i=0;i<literals.Length;i++)b.Label(label+i).Ref(0x72,literals[i]).Store(target).Branch(0x38,label+"End");
        b.Label(label+"End");
    }

    private static byte[] N118_PopupDescription()
    {
        var b=new IL();b.Arg(0).Field(0x0400087C).Store(1);b.Static(0x0A000013).Store(0);
        b.Load(1).Integer(1).Branch(0x40,"afterMode");b.Arg(0).Field(0x04000884).Store(1);
        N118_SelectLiteral(b,1,2,[N118_Adventure,N118_Rescue,N118_HighDifficulty],"mode");
        b.Ref(0x72,N118_PopupPrefix).Load(2).Ref(0x72,N118_PopupSuffix).Call(0x0A00010B).Store(0);
        b.Label("afterMode").Arg(0).Field(0x0400087C).Integer(3).Branch(0x40,"return");
        b.Arg(0).Field(0x04000884).Store(1);N118_SelectLiteral(b,1,0,N118_PopupLiterals,"description");
        b.Label("return").Load(0).Op(0x2A);return b.Finish();
    }

    private static void N118_ArmorMetric(IL b,int method,int textField,int prefix)
    {
        b.Arg(1).Call(method).Store(1);b.Arg(0).Field(textField).Store(3);
        b.Ref(0x72,prefix).Address(1).Call(0x0A000043).Call(0x0A000051).Store(2);
        b.Load(3).Load(2).Integer(1).Virtual(0x0A00004B);
    }
    private static byte[] N118_Armor1Info()
    {
        var b=new IL();b.Arg(0).Field(0x04000741).Field(0x04000180).Store(0);
        N118_SelectLiteral(b,0,2,N118_ArmorNamesLiterals,"name");
        b.Arg(0).Field(0x04000744).Store(3);b.Ref(0x72,N118_ArmorPrefix).Load(2).Call(0x0A000051).Store(2);
        b.Load(3).Load(2).Integer(1).Virtual(0x0A00004B);
        N118_ArmorMetric(b,0x060004B6,0x04000753,N118_HpPrefix);
        N118_ArmorMetric(b,0x060004B7,0x04000754,N118_ToughPrefix);
        N118_ArmorMetric(b,0x060004B5,0x04000755,N118_DefPrefix);
        b.Arg(1).Store(0);N118_SelectLiteral(b,0,2,N118_ArmorDescriptionsLiterals,"effect");
        b.Arg(0).Field(0x04000756).Load(2).Integer(1).Virtual(0x0A00004B).Op(0x2A);return b.Finish();
    }

    private static void N118_PeaUi(IL b)=>b.Arg(0).Field(0x040004EF).Integer(1).Virtual(0x0A000006);
    private static void N118_ShowPeaUi(IL b)
    {
        N118_PeaUi(b);b.Call(0x0A00000B).Branch(0x39,"return");
        N118_PeaUi(b);b.Integer(1).Virtual(0x0A000007);
    }
    private static byte[] N118_Peasniper()
    {
        var b=new IL();b.Arg(0).Call(0x06000360).Store(3);b.Arg(0).Load(3).Set(0x040004DB);
        b.Load(3).Op(0x14).Call(0x0A00000C).Branch(0x3A,"zombie");
        b.Arg(0).Call(0x06000363).Store(4);b.Arg(0).Load(4).Set(0x040004DC);
        b.Load(4).Op(0x14).Call(0x0A00000C).Branch(0x3A,"device");
        N118_PeaUi(b);b.Call(0x0A00000B).Branch(0x39,"return");
        N118_PeaUi(b);b.Integer(0).Virtual(0x0A000007).Branch(0x38,"return");
        b.Label("device");N118_ShowPeaUi(b);
        b.Arg(0).Field(0x040004DC).Virtual(0x0A000001).Virtual(0x0A000037).Store(1);
        b.Address(1).Address(1).Field(0x0A00008B).Float(40).Op(0x58,0x6B).Set(0x0A00008B).Branch(0x38,"position");
        b.Label("zombie");N118_ShowPeaUi(b);
        b.Arg(0).Field(0x040004DB).Virtual(0x0A000001).Virtual(0x0A000037).Store(1);
        b.Address(1).Address(1).Field(0x0A00008B).Float(80).Op(0x58,0x6B).Set(0x0A00008B);
        b.Address(1).Address(1).Field(0x0A00001D).Float(10).Op(0x59,0x6B).Set(0x0A00001D);
        b.Label("position");N118_PeaUi(b);b.Virtual(0x0A00002E).Load(1).Virtual(0x0A000038);
        N118_PeaUi(b);b.Virtual(0x0A00002E).Store(2);
        b.Load(2).Float(0).Float(0).Float(1).New(0x0A00009C).Call(0x0A00001A).Float(60).Op(0x5A,0x6B).Integer(0).Virtual(0x0A0001FC);
        b.Label("return").Op(0x2A);return b.Finish();
    }

    private static byte[] N118_LoadProps()
    {
        var b=new IL();b.New(N118_PropChooseCtor).Store(0);
        b.Arg(1).Field(0x0400089D).Virtual(N118_PropSelectorEnum).Store(1);
        b.Label("try").Branch(0x38,"move");b.Label("body").Address(1).Call(N118_PropSelectorCurrent).Store(2);
        b.Load(2).Field(0x040008A4).Op(0x14).Call(0x0A00000C).Branch(0x39,"move");
        b.Load(0).Load(2).Field(0x040008A4).Virtual(N118_PropChooseAdd);
        b.Label("move").Address(1).Call(N118_PropSelectorMove).Branch(0x3A,"body").Branch(0xDD,"after");
        b.Label("finally").Address(1).Call(N118_PropSelectorDispose).Op(0xDC);
        b.Label("after").Arg(0).Field(0x040000E7).Field(0x040003DC).Store(3);
        b.Load(0).Virtual(N118_PropChooseCount).Integer(1).Op(0x59).Store(4);
        b.Integer(0).Store(7);b.Integer(135).Store(8).Branch(0x38,"test");
        b.Label("loop").Load(0).Load(4).Virtual(N118_PropChooseItem).Store(5);
        b.Arg(0).Field(0x040000F2).Call(0x2B000015).Store(6);
        b.Load(6).Call(0x0A00000B).Branch(0x39,"next");
        b.Load(6).Arg(0).Field(0x040000E7).Set(0x04000578);b.Load(6).Load(7).Set(0x0400057C);
        b.Load(6).Virtual(0x0A000001).Store(9);
        b.Load(9).Load(3).Field(0x040000EE).Virtual(0x0A00002E).Integer(0).Virtual(0x0A00002F);
        b.Load(6).Virtual(0x0A000001).Load(8).Op(0x6B).Float(-15).Float(0).New(0x0A00009C).Virtual(0x0A000042);
        b.Load(7).Integer(1).Op(0x58).Store(7);b.Load(8).Integer(135).Op(0x59).Store(8);
        b.Load(3).Field(0x040000F1).Integer(0).Load(6).Virtual(N118_PropInsert);b.Load(6).Load(5).Virtual(0x06000403);
        b.Label("next").Load(4).Integer(1).Op(0x59).Store(4);
        b.Label("test").Load(4).Integer(0).Branch(0x3C,"loop");
        b.Load(3).Field(0x040000EF).Virtual(0x0A000001).Float(200).Load(7).Op(0x6B).Float(135).Op(0x5A,0x59,0x6B).Float(0).Float(0).New(0x0A00009C).Virtual(0x0A000042).Op(0x2A);
        N118_Finally(b,0x060000E8,("try","finally","after"));return b.Finish();
    }
    private static byte[] N118_SelectorPreviews()
    {
        var b=new IL();b.Arg(0).Field(0x04000207).Virtual(N118_ZombieSelectCount).Store(0);
        b.Arg(0).Field(0x04000207).Virtual(N118_ZombieSelectEnum).Store(1);
        b.Label("tryCount").Branch(0x38,"countMove");b.Label("countBody").Address(1).Call(N118_ZombieSelectCurrent).Field(0x04000201).Integer(3).Branch(0x3C,"countMove");
        b.Load(0).Integer(1).Op(0x58).Store(0);b.Label("countMove").Address(1).Call(N118_ZombieSelectMove).Branch(0x3A,"countBody").Branch(0xDD,"afterCount");
        b.Label("finallyCount").Address(1).Call(N118_ZombieSelectDispose).Op(0xDC);
        b.Label("afterCount").New(N118_EnemyCtor).Store(3);b.Arg(0).Field(0x04000207).Virtual(N118_ZombieSelectEnum).Store(1);
        b.Label("tryEach").Branch(0x38,"eachMove");b.Label("eachBody").Address(1).Call(N118_ZombieSelectCurrent).Store(2);
        b.New(0x060001A0).Store(4);b.Load(4).Load(2).Field(0x040001FF).Set(0x040001F7);
        b.Load(4).Float(BitConverter.Int32BitsToSingle(0x4B18967F)).Set(0x040001FC);b.Load(3).Load(4).Virtual(N118_EnemyAdd);
        b.Label("eachMove").Address(1).Call(N118_ZombieSelectMove).Branch(0x3A,"eachBody").Branch(0xDD,"afterEach");
        b.Label("finallyEach").Address(1).Call(N118_ZombieSelectDispose).Op(0xDC);
        b.Label("afterEach").Float(0).Store(5);b.Arg(0).Field(0x04000207).Virtual(N118_ZombieSelectEnum).Store(1);
        b.Label("tryWeight").Branch(0x38,"weightMove");b.Label("weightBody").Load(5).Address(1).Call(N118_ZombieSelectCurrent).Field(0x04000202).Op(0x58,0x6B).Store(5);
        b.Label("weightMove").Address(1).Call(N118_ZombieSelectMove).Branch(0x3A,"weightBody").Branch(0xDD,"afterWeight");
        b.Label("finallyWeight").Address(1).Call(N118_ZombieSelectDispose).Op(0xDC);b.Label("afterWeight").Branch(0x38,"fillTest");
        b.Label("fill").Arg(0).Arg(0).Field(0x04000207).Load(5).Call(0x060001AC).Store(2);b.Load(2).Branch(0x39,"missing");
        b.New(0x060001A0).Store(4);b.Load(4).Load(2).Field(0x040001FF).Set(0x040001F7);
        b.Load(4).Float(BitConverter.Int32BitsToSingle(0x4B18967F)).Set(0x040001FC);b.Load(3).Load(4).Virtual(N118_EnemyAdd);
        b.Label("fillTest").Load(0).Op(0x6C).Op(0x23).Op(BitConverter.GetBytes(1.5d)).Op(0x5A).Load(3).Virtual(N118_EnemyCount).Op(0x6C).Branch(0x3D,"fill").Branch(0x38,"return");
        b.Label("missing").Ref(0x72,0x70005C1C).Call(0x0A000010);b.Label("return").Load(3).Op(0x2A);
        N118_Finally(b,0x060001A7,("tryCount","finallyCount","afterCount"),("tryEach","finallyEach","afterEach"),("tryWeight","finallyWeight","afterWeight"));return b.Finish();
    }

    private static byte[] N118_BoardEnemyPreviews()
    {
        var b=new IL();b.Arg(0).Field(0x04000782).Virtual(0x060001A7).Store(0);b.Call(0x06000234).Op(0x26);b.Integer(0).Store(2).Branch(0x38,"test");
        b.Label("loop").Load(0).Load(2).Virtual(N118_EnemyItem).Store(1);
        b.Ref(0x72,0x7001A46A).Address(2).Call(0x0A00002A).Ref(0x72,0x7001A476).Call(0x0A00010B).Call(0x0A000010);
        b.Load(1).Integer(1).Integer(6).Call(0x0A00012B).Set(0x040001F8);
        b.Float(680).Float(980).Call(0x0A000083).Store(3);
        b.Float(286).Load(1).Field(0x040001F8).Op(0x6B).Float(134).Op(0x5A,0x59,0x6B).Store(4);
        b.Float(-20).Float(100).Call(0x0A000083).Store(5);
        b.Load(1).Load(3).Load(5).Load(4).Op(0x58,0x6B).Float(0).New(0x0A00009C).Set(0x040001FA);
        b.Arg(0).Load(1).Call(0x060005D4);b.Load(2).Integer(1).Op(0x58).Store(2);
        b.Label("test").Load(2).Load(0).Virtual(N118_EnemyCount).Branch(0x3F,"loop");
        b.New(N118_EntryListCtor).Store(6);b.Arg(0).Field(0x04000781).Field(0x040001A0).Virtual(N118_EntryEnum).Store(7);
        b.Label("tryEntries").Branch(0x38,"entryMove");b.Label("entry").Address(7).Call(N118_EntryCurrent).Store(8);
        b.Load(8).Field(0x040007FA).Integer(22).Branch(0x40,"selected");
        b.Load(8).Field(0x040007FC).Branch(0x3A,"entryMove");b.Load(8).Field(0x040007FD).Branch(0x3A,"entryMove");
        b.Load(6).Load(8).Virtual(N118_EntryListAdd);
        b.Label("selected").Load(8).Field(0x040007FA).Integer(23).Branch(0x40,"always");
        b.Load(8).Field(0x040007FC).Branch(0x3A,"addSelected");b.Load(8).Field(0x040007FD).Branch(0x39,"entryMove");
        b.Label("addSelected").Load(6).Load(8).Virtual(N118_EntryListAdd);
        b.Label("always").Load(8).Field(0x040007FA).Integer(25).Branch(0x40,"entryMove");b.Load(6).Load(8).Virtual(N118_EntryListAdd);
        b.Label("entryMove").Address(7).Call(N118_EntryMove).Branch(0x3A,"entry").Branch(0xDD,"afterEntries");
        b.Label("finallyEntries").Address(7).Call(N118_EntryDispose).Op(0xDC);
        b.Label("afterEntries").Arg(0).Field(0x04000781).Field(0x0400019D).Virtual(N118_EnemyEnum).Store(9);
        b.Label("tryEnemies").Branch(0x38,"enemyMove");b.Label("enemy").Address(9).Call(N118_EnemyCurrent).Store(1);
        b.Load(1).Field(0x040001FE).Static(0x0A000013).Call(0x0A0000B0).Branch(0x39,"spawn");
        b.Load(6).Load(1).Field(0x040001FE).Call(0x0600060A).Branch(0x39,"spawn");b.Ref(0x72,0x700060A7).Call(0x0A000010).Branch(0x38,"enemyMove");
        b.Label("spawn").Load(1).Arg(0).Field(0x04000781).Load(1).Field(0x040001F9).Load(1).Field(0x040001F8).Virtual(0x0600015E).Set(0x040001FA);
        b.Arg(0).Load(1).Call(0x060005D4);
        b.Label("enemyMove").Address(9).Call(N118_EnemyMove).Branch(0x3A,"enemy").Branch(0xDD,"done");
        b.Label("finallyEnemies").Address(9).Call(N118_EnemyDispose).Op(0xDC);b.Label("done").Op(0x2A);
        N118_Finally(b,0x060005D3,("tryEntries","finallyEntries","afterEntries"),("tryEnemies","finallyEnemies","done"));return b.Finish();
    }
    private static void N118_InputCase(IL b,int nameToken,string next)
        =>b.Load(0).Ref(0x72,nameToken).Call(0x0A00003A).Branch(0x39,next);
    private static void N118_ParseInt(IL b)=>b.Arg(1).Address(1).Call(0x0A00003B).Branch(0x39,"bad");
    private static void N118_ParseFloat(IL b)=>b.Arg(1).Address(2).Call(0x0A000044).Branch(0x39,"bad");

    private static byte[] N118_ZombieInputString()
    {
        var b=new IL();b.Arg(1).Call(0x0A000010);b.Arg(0).Field(0x04000099).Store(0);
        N118_InputCase(b,0x70000C3E,"name");N118_ParseInt(b);b.Arg(0).Load(1).Call(0x06000081).Branch(0x38,"done");
        b.Label("name");N118_InputCase(b,0x70001741,"weight");
        b.Arg(0).Arg(1).Set(0x04000097);b.Arg(0).Field(0x04000096).Arg(1).Set(0x0400017C).Branch(0x38,"done");
        b.Label("weight");N118_InputCase(b,0x70001701,"int0");N118_ParseFloat(b);
        b.Arg(0).Field(0x04000096).Load(2).Set(0x04000187).Branch(0x38,"done");
        var fields=new[]{(Name:0x700016A7,Field:0x04000182,Float:true),(Name:0x700016BF,Field:0x04000189,Float:false),(Name:0x700016CF,Field:0x0400017F,Float:true),(Name:0x700016E7,Field:0x04000183,Float:true),(Name:0x70001719,Field:0x04000184,Float:true),(Name:0x70001757,Field:0x04000186,Float:false)};
        for(int i=0;i<fields.Length;i++)
        {
            b.Label("int"+i);N118_InputCase(b,fields[i].Name,"int"+(i+1));N118_ParseInt(b);
            b.Arg(0).Field(0x04000096).Load(1);if(fields[i].Float)b.Op(0x6B);
            b.Set(fields[i].Field).Branch(0x38,"done");
        }
        b.Label("int"+fields.Length).Branch(0x38,"done");b.Label("bad").Ref(0x72,0x7000090C).Call(0x0A000010);
        b.Label("done").Op(0x2A);return b.Finish();
    }

    private static byte[] N118_BoardInputString()
    {
        var b=new IL();b.Arg(1).Call(0x0A000010);b.Arg(0).Field(0x040000BB).Store(0);
        var fields=new[]{(Name:0x70001A66,Field:0x0400019B),(Name:0x70001A80,Field:0x04000199),(Name:0x70001757,Field:0x04000197),(Name:0x70001A92,Field:0x0400019A)};
        for(int i=0;i<fields.Length;i++)
        {
            b.Label("int"+i);N118_InputCase(b,fields[i].Name,"int"+(i+1));N118_ParseInt(b);
            b.Arg(0).Field(0x040000A6).Load(1).Set(fields[i].Field).Branch(0x38,"done");
        }
        b.Label("int"+fields.Length);N118_InputCase(b,0x70001AA4,"height");N118_ParseFloat(b);
        b.Arg(0).Field(0x040000A6).Virtual(0x06000158).Load(2).Set(0x0400037F).Branch(0x38,"done");
        b.Label("height");N118_InputCase(b,0x70001ABA,"width");N118_ParseInt(b);
        b.Arg(0).Field(0x040000A6).Virtual(0x06000158).Store(3);
        b.Load(3).Load(3).Field(0x0400037D).Integer(0).Virtual(0x0A000065).Field(0x04000381).Virtual(N117_GridCount).Load(1).Virtual(0x06000291).Branch(0x38,"refresh");
        b.Label("width");N118_InputCase(b,0x70001AC4,"done");N118_ParseInt(b);
        b.Arg(0).Field(0x040000A6).Virtual(0x06000158).Store(3);
        b.Load(3).Load(1).Load(3).Field(0x0400037D).Virtual(N117_RowCount).Virtual(0x06000291);
        b.Label("refresh").Arg(0).Call(0x0600008B).Branch(0x38,"done");
        b.Label("bad").Ref(0x72,0x7000090C).Call(0x0A000010);b.Label("done").Op(0x2A);return b.Finish();
    }
    private static void N118_DeviceChecked(IL b)=>b.Arg(0).Field(0x04000023).Field(0x04000035);
    private static byte[] N118_DeviceEditorInputString()
    {
        var b=new IL();b.Arg(1).Call(0x0A000010);b.Arg(0).Field(0x04000032).Store(0);
        N118_InputCase(b,0x700006A0,"number0");N118_DeviceChecked(b);b.Arg(1).Set(0x040001D2).Branch(0x38,"preview");
        var cases=new[]{
            (Name:0x700006B2,Path:new[]{0x040001D3},Field:0x04000456),
            (Name:0x7000070E,Path:Array.Empty<int>(),Field:0x040001D1),
            (Name:0x70000764,Path:new[]{0x040001D3},Field:0x04000458),
            (Name:0x700007D0,Path:new[]{0x040001D3,0x04000459},Field:0x0400045D),
            (Name:0x7000082E,Path:new[]{0x040001D3,0x04000459,0x0400045E},Field:0x04000461),
            (Name:0x70000894,Path:new[]{0x040001D3},Field:0x04000457),
            (Name:0x70000900,Path:Array.Empty<int>(),Field:0x040001D0),
            (Name:0x70000966,Path:new[]{0x040001D3},Field:0x04000455)};
        for(int i=0;i<cases.Length;i++)
        {
            b.Label("number"+i);N118_InputCase(b,cases[i].Name,"number"+(i+1));N118_ParseInt(b);
            N118_DeviceChecked(b);foreach(int f in cases[i].Path)b.Field(f);b.Load(1).Set(cases[i].Field).Branch(0x38,"preview");
        }
        b.Label("number"+cases.Length).Branch(0x38,"refresh");
        b.Label("preview").Arg(0).Field(0x04000023).Virtual(0x06000039).Branch(0x38,"refresh");
        b.Label("bad").Ref(0x72,0x7000090C).Call(0x0A000010);
        b.Label("refresh").Arg(0).Arg(0).Field(0x04000023).Call(0x06000030).Op(0x2A);return b.Finish();
    }

    private static void N118_EnemyChecked(IL b)=>b.Arg(0).Field(0x0400003B).Field(0x04000051);
    private static void N118_RevertEnemyInput(IL b,int inputField)
    {
        b.Arg(0).Field(0x0400003B).Store(3);b.Arg(0).Field(inputField).Store(4);
        b.Load(3).Field(0x04000051).FieldAddress(0x040001FB).Call(0x0A00002A).Store(5);
        b.Load(4).Load(5).Virtual(0x0A00002B).Branch(0x38,"finish");
    }
    private static byte[] N118_EnemyEditorInputString()
    {
        var b=new IL();b.Arg(1).Call(0x0A000010);b.Arg(0).Field(0x0400004D).Store(0);
        N118_InputCase(b,0x70000C14,"checkWave");N118_ParseInt(b);
        b.Load(1).Integer(0).Branch(0x3F,"revertWave");b.Load(1).Arg(0).Field(0x04000039).Field(0x0400000D).Field(0x0400019A).Branch(0x3D,"revertWave");
        N118_EnemyChecked(b);b.Load(1).Set(0x040001FB);b.Arg(0).Field(0x0400003B).Virtual(0x0600004C);
        b.Arg(0).Field(0x04000039).Op(0x25).Field(0x04000021).Virtual(0x06000013).Branch(0x38,"finish");
        b.Label("revertWave");N118_RevertEnemyInput(b,0x04000040);
        b.Label("checkWave");N118_InputCase(b,0x70000C2A,"wait");N118_ParseInt(b);
        b.Load(1).Integer(0).Branch(0x3F,"revertCheck");b.Load(1).Arg(0).Field(0x04000039).Field(0x0400000D).Field(0x0400019A).Branch(0x3D,"revertCheck");
        b.Arg(0).Field(0x04000039).Field(0x04000021).Load(1).Branch(0x3B,"finish");
        b.Arg(0).Field(0x04000039).Load(1).Virtual(0x06000013);b.Arg(0).Op(0x14).Call(0x06000042).Branch(0x38,"finish");
        b.Label("revertCheck");
        b.Arg(0).Field(0x0400003F).Store(4);b.Arg(0).Field(0x04000039).FieldAddress(0x04000021).Call(0x0A00002A).Store(5);
        b.Load(4).Load(5).Virtual(0x0A00002B).Branch(0x38,"finish");
        b.Label("wait");N118_InputCase(b,0x70000C50,"key");N118_ParseFloat(b);
        b.Load(2).Float(0).Branch(0x44,"finish");N118_EnemyChecked(b);b.Load(2).Set(0x040001FC).Branch(0x38,"finish");
        b.Label("key");N118_InputCase(b,0x700006A0,"int0");N118_EnemyChecked(b);b.Arg(1).Set(0x040001FE).Branch(0x38,"preview");
        var fields=new[]{(Name:0x7000070E,Field:0x040001F8),(Name:0x70000C3E,Field:0x040001F7),(Name:0x70000900,Field:0x040001F9)};
        for(int i=0;i<fields.Length;i++)
        {
            b.Label("int"+i);N118_InputCase(b,fields[i].Name,"int"+(i+1));N118_ParseInt(b);
            N118_EnemyChecked(b);b.Load(1).Set(fields[i].Field).Branch(0x38,"preview");
        }
        b.Label("int"+fields.Length).Branch(0x38,"finish");
        b.Label("preview").Arg(0).Field(0x0400003B).Virtual(0x0600004C).Branch(0x38,"finish");
        b.Label("bad").Ref(0x72,0x7000090C).Call(0x0A000010);
        b.Label("finish").Arg(0).Arg(0).Field(0x0400003B).Call(0x06000042).Op(0x2A);return b.Finish();
    }
    private static void N118_InputFieldText(IL b,int ui,int entity,int valueField,bool single=false)
    {
        b.Arg(0).Field(ui).Store(2);b.Load(entity).FieldAddress(valueField).Call(single?0x0A000043:0x0A00002A).Store(3);
        b.Load(2).Load(3).Virtual(0x0A00002B);
    }
    private static void N118_EnemyEditorControls(IL b,bool active)
    {
        foreach(int f in new[]{0x0400003E,0x0400003C,0x0400003D,0x04000040,0x04000041,0x04000042,0x04000045,0x04000046})b.Arg(0).Field(f).Integer(active?1:0).Virtual(0x0A000018);
        b.Arg(0).Field(0x0400003A).Virtual(0x0A000009).Integer(active?1:0).Virtual(0x0A000007);
        b.Arg(0).Field(0x0400003A);if(active)b.Arg(1);else b.Op(0x14);b.Set(0x04000055);
        b.Arg(0).Field(0x0400003A).Integer(-1).Virtual(0x06000058);
        foreach(int f in new[]{0x04000047,0x04000048,0x04000049,0x0400004A})b.Arg(0).Field(f).Integer(active?1:0).Virtual(0x0A000018);
    }
    private static byte[] N118_EnemyEditorLoadData()
    {
        var b=new IL();b.Arg(0).Field(0x0400003B).Op(0x14).Call(0x0A00000C).Branch(0x39,"assign");
        b.Arg(0).Field(0x0400003B).Virtual(0x0600004F);
        b.Label("assign").Arg(0).Arg(1).Set(0x0400003B);b.Arg(0).Field(0x0400003B).Op(0x14).Call(0x0A000012).Branch(0x3A,"empty");
        b.Arg(1).Virtual(0x0600004E);b.Arg(0).Field(0x0400003B).Field(0x04000051).Store(0);
        b.Load(0).Field(0x040001F7).Store(1);b.Arg(0).Field(0x0400003E).Store(2);b.Address(1).Call(0x0A00002A).Store(3);b.Load(2).Load(3).Virtual(0x0A00002B);
        N118_InputFieldText(b,0x0400003C,0,0x040001F9);N118_InputFieldText(b,0x0400003D,0,0x040001F8);
        N118_InputFieldText(b,0x04000040,0,0x040001FB);N118_InputFieldText(b,0x04000041,0,0x040001FC,true);
        b.Arg(0).Field(0x04000042).Load(0).Field(0x040001FE).Virtual(0x0A00002B);
        N118_EnemyEditorControls(b,true);b.Op(0x2A);
        b.Label("empty");foreach(int f in new[]{0x0400003E,0x0400003C,0x0400003D,0x04000040,0x04000041,0x04000042})b.Arg(0).Field(f).Static(0x0A000013).Virtual(0x0A00002B);
        N118_EnemyEditorControls(b,false);b.Op(0x2A);return b.Finish();
    }

    private static void N118_PathEditors(IL b)=>b.Arg(0).Field(0x04000055).Field(0x04000054);
    private static void N118_PathIndicator(IL b,bool active)
        =>b.Arg(0).Field(0x04000056).Field(0x04000065).Virtual(0x0A000009).Integer(active?1:0).Virtual(0x0A000007);
    private static void N118_PathUiControls(IL b,bool active)
    {
        foreach(int f in new[]{0x0400005A,0x0400005B,0x0400005C,0x0400005D,0x04000060})b.Arg(0).Field(f).Integer(active?1:0).Virtual(0x0A000018);
    }
    private static byte[] N118_PathEditorLoadData()
    {
        var b=new IL();b.Arg(1).Store(0);b.Op(0x14).Store(1);
        b.Arg(0).Field(0x04000055).Op(0x14).Call(0x0A000012).Branch(0x3A,"noEnemy");
        b.Load(0).Integer(0).Branch(0x3C,"nonnegative");b.Integer(-1).Store(0);
        b.Label("nonnegative").Load(0);N118_PathEditors(b);b.Virtual(N118_PathEditorCount).Branch(0x3F,"bounded");
        N118_PathEditors(b);b.Virtual(N118_PathEditorCount).Integer(1).Op(0x59).Store(0);
        b.Label("bounded").Load(0).Integer(0).Branch(0x3F,"buttons");N118_PathEditors(b);b.Load(0).Virtual(N118_PathEditorItem).Store(1);
        b.Label("buttons").Arg(0).Field(0x04000061).Load(0);N118_PathEditors(b);b.Virtual(N118_PathEditorCount).Integer(1).Op(0x59).Op(0xFE,0x04).Virtual(0x0A000018);
        b.Arg(0).Field(0x04000062).Load(0).Integer(0).Op(0xFE,0x04).Integer(0).Op(0xFE,0x01).Virtual(0x0A000018);
        b.Integer(0).Store(2).Branch(0x38,"labelTest");
        b.Label("labelLoop");N118_PathEditors(b);b.Load(2).Virtual(N118_PathEditorItem).Field(0x04000066).Store(6);
        b.Load(2).Store(7);b.Load(6).Address(7).Call(0x0A00002A).Integer(1).Virtual(0x0A00004B);
        b.Load(2).Integer(1).Op(0x58).Store(2);
        b.Label("labelTest").Load(2);N118_PathEditors(b);b.Virtual(N118_PathEditorCount).Branch(0x3F,"labelLoop").Branch(0x38,"assign");
        b.Label("noEnemy").Integer(-1).Store(0);b.Arg(0).Field(0x04000061).Integer(0).Virtual(0x0A000018);b.Arg(0).Field(0x04000062).Integer(0).Virtual(0x0A000018);
        b.Label("assign").Arg(0).Load(0).Set(0x04000058);
        b.Arg(0).Field(0x04000056).Op(0x14).Call(0x0A00000C).Branch(0x39,"setCurrent");N118_PathIndicator(b,false);
        b.Label("setCurrent").Arg(0).Load(1).Set(0x04000056);
        b.Arg(0).Field(0x04000059).Store(4);b.Load(4).Address(0).Call(0x0A00002A).Virtual(0x0A00002B);
        b.Arg(0).Field(0x04000056).Op(0x14).Call(0x0A000012).Branch(0x3A,"emptyPath");
        b.Arg(0).Field(0x04000056).Field(0x04000068).Store(3);
        foreach(var x in new[]{(Ui:0x0400005A,Value:0x040003A5,Single:false),(Ui:0x0400005B,Value:0x040003A6,Single:false),(Ui:0x0400005C,Value:0x040003A8,Single:true)})
        {
            b.Arg(0).Field(x.Ui).Store(4);b.Load(3).FieldAddress(x.Value).Call(x.Single?0x0A000043:0x0A00002A).Store(5);b.Load(4).Load(5).Virtual(0x0A00002B);
        }
        b.Arg(0).Field(0x0400005D).Load(3).Field(0x040003AA).Virtual(0x0A000017);N118_PathUiControls(b,true);N118_PathIndicator(b,true);b.Op(0x2A);
        b.Label("emptyPath");foreach(int f in new[]{0x0400005A,0x0400005B,0x0400005C})b.Arg(0).Field(f).Static(0x0A000013).Virtual(0x0A00002B);
        b.Arg(0).Field(0x0400005D).Integer(0).Virtual(0x0A000017);N118_PathUiControls(b,false);b.Op(0x2A);return b.Finish();
    }
    private static void N118_DeviceValueText(IL b,int ui,int entity,int value)
    {
        b.Arg(0).Field(ui).Store(5);b.Load(entity).FieldAddress(value).Call(0x0A00002A).Store(6);b.Load(5).Load(6).Virtual(0x0A00002B);
    }
    private static void N118_UiInteract(IL b,int field,bool active)=>b.Arg(0).Field(field).Integer(active?1:0).Virtual(0x0A000018);
    private static void N118_UiClear(IL b,int field)=>b.Arg(0).Field(field).Static(0x0A000013).Virtual(0x0A00002B);
    private static void N118_UiDropdown(IL b,int field,int value)=>b.Arg(0).Field(field).Integer(value).Virtual(0x0A000039);
    private static byte[] N118_DeviceEditorLoadData()
    {
        var b=new IL();b.Arg(0).Arg(1).Set(0x04000023);b.Arg(0).Field(0x04000023).Op(0x14).Call(0x0A000012).Branch(0x3A,"empty");
        b.Arg(0).Field(0x04000023).Field(0x04000035).Store(0);b.Load(0).Field(0x040001D3).Store(1);b.Load(1).Field(0x04000459).Store(2);
        b.Load(1).Field(0x04000456).Store(3);b.Arg(0).Field(0x04000026).Store(5);b.Load(3).Store(4);b.Address(4).Call(0x0A00002A).Store(6);b.Load(5).Load(6).Virtual(0x0A00002B);
        N118_DeviceValueText(b,0x04000024,0,0x040001D0);N118_DeviceValueText(b,0x04000025,0,0x040001D1);
        b.Arg(0).Field(0x0400002B).Load(0).Field(0x040001D2).Virtual(0x0A00002B);
        b.Arg(0).Field(0x0400002E).Load(1).Field(0x04000455).Virtual(0x0A000039);
        foreach(int f in new[]{0x04000026,0x04000024,0x04000025,0x0400002B,0x0400002E,0x0400002E})N118_UiInteract(b,f,true);
        b.Load(3).Integer(8).Branch(0x40,"noAssociation");
        N118_UiInteract(b,0x04000027,true);N118_UiInteract(b,0x04000028,true);
        N118_DeviceValueText(b,0x04000027,1,0x04000457);N118_DeviceValueText(b,0x04000028,1,0x04000458);b.Branch(0x38,"content");
        b.Label("noAssociation");N118_UiInteract(b,0x04000027,false);N118_UiInteract(b,0x04000028,false);N118_UiClear(b,0x04000027);N118_UiClear(b,0x04000028);
        b.Label("content").Load(3).Integer(3).Branch(0x40,"noContent");
        N118_UiInteract(b,0x0400002C,true);b.Arg(0).Field(0x0400002C).Load(1).Field(0x0400045A).Float(0).Op(0xFE,0x02).Virtual(0x0A000017);
        N118_UiInteract(b,0x0400002D,true);b.Arg(0).Field(0x0400002D).Load(2).Field(0x0400045C).Virtual(0x0A000039);
        N118_UiInteract(b,0x04000029,true);N118_DeviceValueText(b,0x04000029,2,0x0400045D);
        b.Load(2).Field(0x0400045C).Integer(8).Branch(0x40,"clearCard");
        N118_UiInteract(b,0x0400002A,true);b.Load(2).Field(0x0400045E).Store(7);N118_DeviceValueText(b,0x0400002A,7,0x04000461);
        N118_UiInteract(b,0x0400002F,true);b.Arg(0).Field(0x0400002F).Load(2).Field(0x0400045E).Field(0x04000460).Virtual(0x0A000039).Branch(0x38,"buttons");
        b.Label("noContent");N118_UiInteract(b,0x0400002C,false);b.Arg(0).Field(0x0400002C).Integer(0).Virtual(0x0A000017);
        N118_UiInteract(b,0x0400002D,false);N118_UiDropdown(b,0x0400002D,0);N118_UiInteract(b,0x04000029,false);N118_UiClear(b,0x04000029);
        b.Label("clearCard");N118_UiInteract(b,0x0400002A,false);N118_UiClear(b,0x0400002A);N118_UiInteract(b,0x0400002F,false);N118_UiDropdown(b,0x0400002F,0);
        b.Label("buttons");N118_UiInteract(b,0x04000030,true);N118_UiInteract(b,0x04000031,true);b.Op(0x2A);
        b.Label("empty");foreach(int f in new[]{0x04000026,0x04000024,0x04000025,0x04000027,0x04000028,0x04000029,0x0400002A,0x0400002B})N118_UiClear(b,f);
        N118_UiDropdown(b,0x0400002E,0);N118_UiDropdown(b,0x0400002D,0);b.Arg(0).Field(0x0400002C).Integer(0).Virtual(0x0A000017);
        foreach(int f in new[]{0x04000026,0x04000024,0x04000025,0x0400002B,0x0400002E,0x0400002D,0x04000027,0x04000028,0x04000030,0x04000031,0x0400002C,0x0400002D,0x04000029,0x0400002A,0x0400002F})N118_UiInteract(b,f,false);
        b.Op(0x2A);return b.Finish();
    }
    private static void N118_CyclerCharacter(IL b)
        =>b.Arg(0).Field(0x04000A2B).Field(0x0A0002BB).Arg(0).Field(0x04000A2C).Ref(0x8F,N118_TmpCharType);
    private static byte[] N118_VertexColorCycler()
    {
        var b=new IL();b.Arg(0).Field(0x04000A28).Store(0);b.Arg(0).Field(0x04000A2A).Store(1);
        b.Load(0).Branch(0x39,"start").Load(0).Integer(1).Branch(0x3B,"resume").Load(0).Integer(2).Branch(0x3B,"resume").Integer(0).Op(0x2A);
        b.Label("start");N117_State(b,0x04000A28,-1);
        b.Load(1).Field(0x04000A27).Integer(0).Integer(0).Virtual(0x0A000338);
        b.Arg(0).Load(1).Field(0x04000A27).Virtual(0x0A0002B8).Set(0x04000A2B);
        N117_State(b,0x04000A2C,0);b.Load(1).Field(0x04000A27).Virtual(0x0A000356).Call(0x0A0002C0).Op(0x26).Branch(0x38,"body");
        b.Label("resume");N117_State(b,0x04000A28,-1);
        b.Label("body").Arg(0).Field(0x04000A2B).Field(0x0A000339).Store(2);b.Load(2).Branch(0x39,"empty");
        N118_CyclerCharacter(b);b.Field(0x0A0002BC).Store(3);
        b.Arg(0).Field(0x04000A2B).Field(0x0A0002BE).Load(3).Ref(0x8F,N118_TmpMeshType).Field(0x0A0002BF).Store(5);
        N118_CyclerCharacter(b);b.Field(0x0A0002BD).Store(4);N118_CyclerCharacter(b);b.Field(0x0A00033D).Branch(0x39,"advance");
        for(int i=0;i<3;i++)b.Integer(0).Integer(255).Call(0x0A00012B).Op(0xD2);
        b.Integer(255).New(0x0A00037F).Store(6);
        for(int i=0;i<4;i++)
        {
            b.Load(5).Load(4);if(i!=0)b.Integer(i).Op(0x58);b.Load(6).Ref(0xA4,N118_Color32Type);
        }
        b.Load(1).Field(0x04000A27).Integer(16).Virtual(0x0A0002C2);
        b.Label("advance").Arg(0).Arg(0).Field(0x04000A2C).Integer(1).Op(0x58).Load(2).Op(0x5D).Set(0x04000A2C);
        N117_Yield(b,0x04000A28,0x04000A29,2,.05f);b.Label("empty");N117_Yield(b,0x04000A28,0x04000A29,1,.25f);return b.Finish();
    }
    private static void N118_Double(IL b,double value)=>b.Op(0x23).Op(BitConverter.GetBytes(value));
    private static void N118_RoundGrowth(IL b)
    {
        // Match native tie handling and float32 add/subtract, including its -0 path.
        b.Load(9).Op(0x6C).Call(0x0A000283).Store(10);b.Load(9).Op(0x6C).Load(10).Op(0x59).Store(11);
        b.Load(9).Float(0).Branch(0x44,"roundNegative");b.Load(11);N118_Double(b,.5);b.Branch(0x40,"roundFloor");
        b.Load(10).Op(0x6B).Store(9);b.Load(10).Op(0x6A).Op(0x21).Op(BitConverter.GetBytes(1L)).Op(0x5F,0x69).Branch(0x39,"rounded");
        b.Load(9).Float(1).Op(0x58,0x6B).Store(9).Branch(0x38,"rounded");
        b.Label("roundFloor").Load(9).Float(.5f).Op(0x58,0x6B).Call(0x0A00004F).Store(9).Branch(0x38,"rounded");
        b.Label("roundNegative").Load(11);N118_Double(b,-.5);b.Branch(0x40,"roundCeiling");
        b.Load(10).Op(0x6B).Store(9);b.Load(10).Op(0x6A).Op(0x21).Op(BitConverter.GetBytes(1L)).Op(0x5F,0x69).Branch(0x39,"rounded");
        b.Load(9).Float(1).Op(0x59,0x6B).Store(9).Branch(0x38,"rounded");
        b.Label("roundCeiling").Load(9).Float(.5f).Op(0x59,0x6B).Call(0x0A000050).Store(9);b.Label("rounded");
    }
    private static byte[] N118_ComputePlantData()
    {
        var b=new IL();b.Integer(5).Ref(0x8D,0x0100000E).Store(0);
        var thresholds=new[]{20,30,40,50,60};for(int i=0;i<thresholds.Length;i++)b.Load(0).Integer(i).Integer(thresholds[i]).Op(0x9E);
        var choices=new[]{(Key:0x70001198,Base:0x04000071,Factor:0x04000072,Values:0x0400031F,DataFactor:0x04000320,Ui:0x0400007F),(Key:0x70000F30,Base:0x04000073,Factor:0x04000074,Values:0x04000321,DataFactor:0x04000322,Ui:0x04000080),(Key:0x70000F38,Base:0x04000075,Factor:0x04000076,Values:0x04000324,DataFactor:0x04000325,Ui:0x04000081)};
        for(int i=0;i<choices.Length;i++)
        {
            b.Label("choice"+i).Arg(1).Ref(0x72,choices[i].Key).Call(0x0A00003A).Branch(0x39,"choice"+(i+1));
            b.Arg(0).Field(0x0400006E).Store(1);b.Arg(0).Field(choices[i].Base).Store(2);b.Arg(0).Field(choices[i].Factor).Store(3);
            b.Load(1).Field(choices[i].Values).Store(4);b.Arg(0).Field(0x0400006E).Load(3).Set(choices[i].DataFactor);b.Arg(0).Field(choices[i].Ui).Store(5).Branch(0x38,"compute");
        }
        b.Label("choice"+choices.Length).Op(0x2A);
        b.Label("compute").Integer(0).Store(6);b.Integer(0).Store(7);
        b.Label("segment").Integer(0).Store(8).Branch(0x38,"stepTest");
        b.Label("step").Load(4).Load(7).Ref(0x8F,0x0100001F).Load(8).Op(0x6B).Load(3).Op(0x5B,0x6B,0x6C).Call(0x0A00004E).Op(0x6B).Load(2).Op(0x5A,0x6B,0x56);
        b.Load(7).Integer(1).Op(0x58).Store(7);b.Load(8).Integer(1).Op(0x58).Store(8);
        b.Label("stepTest").Load(8).Load(0).Load(6).Op(0x94).Branch(0x3F,"step");
        b.Load(4).Load(7).Integer(1).Op(0x59,0x98).Store(2);b.Load(6).Integer(1).Op(0x58).Store(6);b.Load(6).Integer(5).Branch(0x3F,"segment");
        b.Integer(0).Store(7).Branch(0x38,"roundTest");b.Label("round").Load(4).Load(7).Op(0x98).Store(9);N118_RoundGrowth(b);
        b.Load(4).Load(7).Load(9).Op(0xA0);b.Load(7).Integer(1).Op(0x58).Store(7);
        b.Label("roundTest").Load(7).Load(4).Op(0x8E,0x69).Branch(0x3F,"round");
        b.Ref(0x72,0x7000118C).Load(4).Integer(199).Ref(0x8F,0x0100001F).Call(0x0A000043).Call(0x0A000051).Store(12);
        b.Load(5).Load(12).Integer(1).Virtual(0x0A00004B).Op(0x2A);return b.Finish();
    }
    private static void N118_ClampUnit(IL b,int local,string label)
    {
        b.Load(local).Float(0).Branch(0x3F,label+"Low");b.Load(local).Float(1).Branch(0x3D,label+"High").Branch(0x38,label+"Done");
        b.Label(label+"Low").Float(0).Store(local).Branch(0x38,label+"Done");b.Label(label+"High").Float(1).Store(local);b.Label(label+"Done");
    }
    private static byte[] N118_SetDataUi()
    {
        var b=new IL();b.Arg(0).Field(0x040008CE).Store(0);b.Arg(0).Arg(1).Set(0x040008D4);
        foreach(int name in new[]{0x7001B9DF,0x7001BA45})b.Load(0).Ref(0x72,name).Call(0x0A00003A).Branch(0x3A,"seconds");
        foreach(int name in new[]{0x7001B9F9,0x7001BA0B,0x7001BA1F,0x7001BA31,0x7001BA61})b.Load(0).Ref(0x72,name).Call(0x0A00003A).Branch(0x3A,"number");
        b.Branch(0x38,"bars");b.Label("seconds").Ref(0x72,0x7001C9AD).Store(2).Branch(0x38,"format");
        b.Label("number").Ref(0x72,0x7001B406).Store(2);
        b.Label("format").Arg(0).Field(0x040008D3).Store(1);b.ArgAddress(1).Load(2).Call(0x0A000125).Store(2);b.Load(1).Load(2).Integer(1).Virtual(0x0A00004B);
        b.Label("bars");foreach(int name in new[]{0x7001BA0B,0x7001BA45,0x7001B9DF})b.Arg(0).Field(0x040008CE).Ref(0x72,name).Call(0x0A00003A).Branch(0x3A,"zeroBars");
        b.Arg(1).Arg(0).Field(0x040008D5).Op(0x5B,0x6B).Store(3);b.Float(0).Store(4);b.Float(0).Store(5);
        for(int i=6;i<=11;i++)b.Float(1).Store(i);
        b.Load(3).Float(0).Branch(0x44,"range1");b.Load(3).Float(1).Branch(0x42,"range1");b.Load(3).Store(4);
        b.Label("range1").Load(3).Float(1).Branch(0x43,"range3");b.Load(3).Float(3).Branch(0x42,"range3");
        b.Load(3).Float(1).Op(0x59).Float(.5f).Op(0x5A,0x6B).Store(5);b.Float(1).Store(4);b.Float(.61f).Store(9);b.Float(.34f).Store(11);
        b.Label("range3").Load(3).Float(3).Branch(0x43,"range7");b.Load(3).Float(7).Branch(0x42,"range7");
        b.Load(3).Float(3).Op(0x59).Float(.25f).Op(0x5A,0x6B).Store(5);b.Float(1).Store(4);b.Float(.61f).Store(6);b.Float(.34f).Store(8);b.Float(.28f).Store(9);b.Float(.9f).Store(10);b.Float(1).Store(11);
        b.Label("range7").Load(3).Float(7).Branch(0x43,"range15");b.Load(3).Float(15).Branch(0x41,"range15");
        b.Load(3).Float(7).Op(0x59).Float(.125f).Op(0x5A,0x6B).Store(5);b.Float(1).Store(4);b.Float(.28f).Store(6);b.Float(.9f).Store(7);b.Float(1).Store(8);b.Float(.51f).Store(9);b.Float(.455f).Store(10);b.Float(1).Store(11);
        b.Label("range15").Load(3).Float(15).Branch(0x44,"clamp");b.Float(0).Store(4);b.Float(1).Store(5);b.Float(1).Store(9);b.Float(.95f).Store(10);b.Float(0).Store(11);
        b.Label("clamp");N118_ClampUnit(b,4,"clamp0");N118_ClampUnit(b,5,"clamp1");
        b.Arg(0).Field(0x040008D1).Load(4).Virtual(0x0A0000F0);b.Arg(0).Field(0x040008D2).Load(5).Virtual(0x0A0000F0);
        b.Arg(0).Field(0x040008D1).Load(6).Load(7).Load(8).Float(1).New(0x0A0000BE).Virtual(0x0A0001DE);
        b.Arg(0).Field(0x040008D2).Load(9).Load(10).Load(11).Float(1).New(0x0A0000BE).Virtual(0x0A0001DE).Op(0x2A);
        b.Label("zeroBars").Arg(0).Field(0x040008D1).Float(0).Virtual(0x0A0000F0);b.Arg(0).Field(0x040008D2).Float(0).Virtual(0x0A0000F0).Op(0x2A);return b.Finish();
    }
    private static byte[] N118_AttackStartTry()
    {
        var b=new IL();b.Arg(0).Field(0x040004B1).Store(0);
        foreach(int id in new[]{0,3,4,5,6,7,49})b.Load(0).Integer(id).Branch(0x3B,"id"+id);
        b.Branch(0x38,"false");
        b.Label("id0").Arg(0).Field(0x040004B4).Integer(1).Branch(0x3F,"seek");
        b.Arg(0).Field(0x040004DB).Call(0x0A00000B).Branch(0x39,"existingDevice");
        b.Arg(0).Field(0x040004DB).Virtual(0x06000431).Branch(0x3A,"start");
        b.Label("existingDevice").Arg(0).Field(0x040004DC).Call(0x0A00000B).Branch(0x39,"seek");
        b.Arg(0).Field(0x040004DC).Field(0x0400047D).Branch(0x39,"start").Branch(0x38,"seek");
        b.Label("id4").Arg(0).Field(0x040004D2).Branch(0x39,"skill2");
        b.Arg(0).Field(0x040004D1).Field(0x0400068E).Integer(2).Branch(0x3B,"start");
        b.Label("skill2").Arg(0).Field(0x040004D1).Field(0x0400068E).Integer(2).Branch(0x3B,"seek").Branch(0x38,"false");
        b.Label("id5").Branch(0x38,"devour");b.Label("id6");
        b.Label("devour").Arg(0).Field(0x040004F9).Branch(0x39,"seek").Branch(0x38,"false");
        b.Label("id7").Arg(0).Field(0x040004B6).Integer(1).Branch(0x3B,"seek").Branch(0x38,"false");
        b.Label("id49").Arg(0).Field(0x040004D2).Branch(0x39,"false");
        b.Arg(0).Field(0x040004D1).Field(0x0400068E).Integer(1).Branch(0x3B,"false").Branch(0x38,"seek");
        b.Label("id3").Arg(0).Field(0x040004DD).Virtual(N118_BlockZombieCount).Integer(0).Branch(0x3E,"seekBlock");
        b.Arg(0).Field(0x040004DD).Virtual(N118_BlockZombieEnum).Store(1);b.Integer(0).Store(3);
        b.Label("tryBlock").Branch(0x38,"moveBlock");
        b.Label("eachBlock").Address(1).Call(N118_BlockZombieCurrent).Store(2);
        b.Load(2).Op(0x14).Call(0x0A000012).Branch(0x3A,"moveBlock");b.Load(2).Virtual(0x06000431).Branch(0x39,"moveBlock");
        b.Arg(0).Load(2).Set(0x040004DB);b.Integer(1).Store(3).Branch(0xDD,"afterBlock");
        b.Label("moveBlock").Address(1).Call(N118_BlockZombieMove).Branch(0x3A,"eachBlock").Branch(0xDD,"afterBlock");
        b.Label("finallyBlock").Address(1).Call(N118_BlockZombieDispose).Op(0xDC);
        b.Label("afterBlock").Load(3).Branch(0x3A,"start").Branch(0x38,"false");
        b.Label("seekBlock").Arg(0).Arg(0).Call(0x06000360).Set(0x040004DB);
        b.Arg(0).Field(0x040004DB).Call(0x0A00000B).Branch(0x3A,"start").Branch(0x38,"false");
        b.Label("seek").Arg(0).Arg(0).Call(0x06000360).Set(0x040004DB);
        b.Arg(0).Field(0x040004DB).Op(0x14).Call(0x0A00000C).Branch(0x3A,"clearDevice");
        b.Arg(0).Arg(0).Call(0x06000363).Set(0x040004DC);
        b.Arg(0).Field(0x040004DC).Op(0x14).Call(0x0A00000C).Branch(0x39,"false");b.Arg(0).Op(0x14).Set(0x040004DB).Branch(0x38,"start");
        b.Label("clearDevice").Arg(0).Op(0x14).Set(0x040004DC);
        b.Label("start").Arg(0).Call(0x06000395).Integer(1).Op(0x2A);b.Label("false").Integer(0).Op(0x2A);
        N118_Finally(b,0x06000396,("tryBlock","finallyBlock","afterBlock"));return b.Finish();
    }
    private static byte[] N118_GetDeviceFromBank()
    {
        var b=new IL();
        // Native checks the card before reading the board; preserve that fault order.
        b.Arg(1).Branch(0x3A,"card").New(0x0A000022).Op(0x7A);
        b.Label("card").Arg(0).Field(0x04000230).Field(0x040003D8).Arg(1).Field(0x04000400).Field(0x04000423).Virtual(0x06000189).Store(0);
        b.Load(0).Op(0x14).Call(0x0A000012).Branch(0x3A,"missing");
        b.Arg(0).Load(0).Call(0x2B000027).Set(0x0400023E);
        b.Arg(0).Field(0x0400023E).Virtual(0x0A000001).Virtual(0x0A000090).Store(1);
        b.Label("tryChildren").Branch(0x38,"moveChild");
        b.Label("child").Load(1).Virtual(0x0A0000ED).Ref(0x74,0x01000005).Virtual(0x0A00003C).Ref(0x72,0x70006CED).Call(0x0A00003A).Op(0x26);
        b.Label("moveChild").Load(1).Virtual(0x0A0000EC).Branch(0x3A,"child").Branch(0xDD,"afterChildren");
        b.Label("finallyChildren").Load(1).Ref(0x75,0x01000036).Store(2);b.Load(2).Branch(0x39,"finallyDone");b.Load(2).Virtual(0x0A0000D0);
        b.Label("finallyDone").Op(0xDC);
        b.Label("afterChildren").Arg(0).Field(0x0400023E).Arg(0).Field(0x04000230).Set(0x04000463);
        b.Arg(0).Field(0x0400023E).Arg(1).Arg(0).Virtual(0x06000333);
        b.Arg(0).Field(0x0400023E).Virtual(0x0600032D);
        b.Arg(0).Integer(1).Set(0x04000233);b.Arg(0).Arg(0).Field(0x0400023E).Field(0x0400046D).Set(0x04000242);b.Arg(0).Arg(1).Set(0x0400023B);
        b.Load(0).Call(0x2B000027).Store(3);b.Arg(0).Load(3).Set(0x0400023F);
        b.Load(3).Op(0x14).Call(0x0A000012).Branch(0x3A,"previewMissing");
        b.Arg(0).Field(0x0400023F).Arg(0).Virtual(0x06000334).Integer(1).Op(0x2A);
        b.Label("previewMissing").Ref(0x72,0x70006CFB).Call(0x0A000158).Integer(1).Op(0x2A);
        b.Label("missing").Ref(0x72,0x70006D0D).Call(0x0A000158).Integer(0).Op(0x2A);
        N118_Finally(b,0x060001DB,("tryChildren","finallyChildren","afterChildren"));return b.Finish();
    }
    private static byte[] N118_ProjectUpdate()
    {
        var b=new IL();b.Arg(0).Arg(0).Field(0x0400051A).Call(0x0A00001A).Op(0x58,0x6B).Set(0x0400051A);b.Arg(0).Call(0x060003C8);
        b.Arg(0).Field(0x04000515).Branch(0x3A,"tracks");
        b.Arg(0).Virtual(0x0A000001).Store(3);b.Call(0x0A00001A).Float(-18).Op(0x5A,0x6B).Store(0);
        b.Load(3).Float(0).Float(0).Float(1).New(0x0A00009C).Load(0).Integer(0).Virtual(0x0A0001FC);
        b.Label("tracks").Arg(0).Field(0x04000520).Integer(5).Branch(0x40,"scale");
        b.Arg(0).Arg(0).Field(0x0400051B).Call(0x0A00001A).Op(0x58,0x6B).Set(0x0400051B);
        b.Arg(0).Field(0x04000516).Integer(4).Branch(0x3B,"flicker");b.Arg(0).Field(0x04000516).Integer(5).Branch(0x40,"scale");
        b.Label("flicker").Arg(0).Arg(0).Field(0x0400051D).Call(0x0A00001A).Op(0x58,0x6B).Set(0x0400051D);
        b.Arg(0).Field(0x0400051D).Float(.7f).Branch(0x43,"flickerValue");b.Arg(0).Float(0).Set(0x0400051D);
        b.Label("flickerValue").Float(1).Arg(0).Field(0x0400051D).Float(.35f).Op(0x59,0x6B).Call(0x0A0000E9).Float(.35f).Op(0x5B,0x6B).Float(.6863f).Op(0x5A,0x6B,0x59,0x6B).Store(0);
        b.Arg(0).Field(0x04000526).Integer(0).Virtual(0x0A000006).Virtual(0x2B000011).Load(0).Load(0).Load(0).Float(1).New(0x0A0000BE).Virtual(0x0A000097);
        b.Label("scale").Arg(0).Field(0x04000520).Integer(1).Branch(0x40,"collect");
        b.Arg(0).Field(0x0400051A).Float(.5f).Branch(0x41,"collect");
        b.Arg(0).Field(0x0400051A).Float(.5f).Op(0x58,0x6B).Float(1).Call(0x0A00011F).Store(0);b.Arg(0).Virtual(0x0A000001).Store(3);
        b.Call(0x0A000209).Store(4);foreach(int component in new[]{0x0A00001D,0x0A00008B,0x0A00009D})
            b.Address(4).Address(4).Field(component).Arg(0).Field(0x04000518).Op(0x5A,0x6B).Load(0).Op(0x5A,0x6B).Set(component);
        b.Load(3).Load(4).Virtual(0x0A0000E5);
        b.Label("collect").Arg(0).Field(0x04000520).Integer(4).Branch(0x40,"destroy");b.Arg(0).Call(0x060003C4);
        b.Label("destroy").Arg(0).Field(0x0400051C).Float(0).Branch(0x43,"return");
        b.Arg(0).Arg(0).Field(0x0400051C).Call(0x0A00001A).Op(0x59,0x6B).Set(0x0400051C);
        b.Arg(0).Field(0x0400051C).Float(.25f).Branch(0x42,"expire");
        b.Integer(0).Store(5).Branch(0x38,"fadeTest");
        b.Label("fade").Arg(0).Field(0x04000526).Load(5).Virtual(0x0A000006).Virtual(0x2B000011).Store(6);
        b.Load(6).Float(1).Float(1).Float(1).Arg(0).Field(0x0400051C).Float(4).Op(0x5A,0x6B).New(0x0A0000BE).Virtual(0x0A000097);
        b.Load(5).Integer(1).Op(0x58).Store(5);b.Label("fadeTest").Load(5).Arg(0).Field(0x04000526).Virtual(N118_ProjectSpriteCount).Branch(0x3F,"fade");
        b.Label("expire").Arg(0).Field(0x0400051C).Float(0).Branch(0x42,"return");
        b.Arg(0).Field(0x0400051E).Call(0x0A00000B).Branch(0x39,"destroyObject");
        b.Arg(0).Field(0x04000515).Branch(0x39,"sun");
        b.Arg(0).Field(0x0400051E).Field(0x040003D7).Field(0x040002C2).Arg(0).Virtual(N118_ProjectRemove).Op(0x26).Branch(0x38,"destroyObject");
        b.Label("sun").Arg(0).Field(0x0400051E).Field(0x040003DB).Field(0x04000343).Arg(0).Virtual(N118_ProjectRemove).Op(0x26);
        b.Label("destroyObject").Arg(0).Virtual(0x0A000009).Call(0x0A000025);b.Label("return").Op(0x2A);return b.Finish();
    }
    private static void N118_ZombieUiParent(IL b)
        =>b.Arg(0).Field(0x040005AA).Field(0x040003C5).Virtual(0x0A00002E).Virtual(N110_SetParent);
    private static byte[] N118_ZombieUis()
    {
        var b=new IL();b.Arg(0).Field(0x040005AA).Op(0x14).Call(0x0A000012).Branch(0x3A,"return");
        b.Integer(0).Store(0).Branch(0x38,"uiTest");
        b.Label("eachObject").Arg(0).Field(0x04000614).Load(0).Op(0x9A,0x14).Call(0x0A00000C).Branch(0x39,"nextObject");
        b.Arg(0).Field(0x04000614).Load(0).Op(0x9A).Virtual(0x0A00002E);N118_ZombieUiParent(b);
        b.Label("nextObject").Load(0).Integer(1).Op(0x58).Store(0);
        b.Label("uiTest").Load(0).Arg(0).Field(0x04000614).Op(0x8E,0x69).Branch(0x3F,"eachObject");
        foreach(var entry in new[]{(Field:0x04000615,Op:0x0A00000C),(Field:0x04000617,Op:0x0A00000B),(Field:0x04000619,Op:0x0A00000B)})
        {
            b.Arg(0).Field(entry.Field);if(entry.Op==0x0A00000C)b.Op(0x14);
            b.Call(entry.Op).Branch(0x39,"afterObject"+entry.Field);
            b.Arg(0).Field(entry.Field).Virtual(0x0A00002E);N118_ZombieUiParent(b);b.Label("afterObject"+entry.Field);
        }
        b.Arg(0).Field(0x040005F4).Field(0x0400014F).Virtual(N118_ElementEnum).Store(1);
        b.Label("tryElements").Branch(0x38,"moveElement");
        b.Label("eachElement").Address(1).Call(N118_ElementCurrent).Store(3);
        b.Arg(0).Field(0x04000618).Virtual(N118_ElementUiEnum).Store(2);
        b.Label("tryUi").Branch(0x38,"moveUi");
        b.Label("eachUi").Address(2).Call(N118_ElementUiCurrent).Store(4);
        b.Load(3).Field(0x04000146).Load(4).Field(0x04000152).Branch(0x40,"moveUi");b.Load(3).Load(4).Set(0x04000148);
        b.Label("moveUi").Address(2).Call(N118_ElementUiMove).Branch(0x3A,"eachUi").Branch(0xDD,"afterUi");
        b.Label("finallyUi").Address(2).Call(N118_ElementUiDispose).Op(0xDC);
        b.Label("afterUi");b.Label("moveElement").Address(1).Call(N118_ElementMove).Branch(0x3A,"eachElement").Branch(0xDD,"return");
        b.Label("finallyElements").Address(1).Call(N118_ElementDispose).Op(0xDC);
        b.Label("return").Op(0x2A);
        N118_Finally(b,0x0600047A,("tryElements","finallyElements","return"),("tryUi","finallyUi","afterUi"));return b.Finish();
    }
    private static void N118_FlowerWait(IL b,int state)
    {
        b.Load(1).New(0x060008C6).Store(5);
        b.Arg(0).Load(5).Set(0x04000AEB);N117_State(b,0x04000AEA,state);b.Integer(1).Op(0x2A);
    }
    private static byte[] N118_FlowerCoroutine()
    {
        var b=new IL();b.Arg(0).Field(0x04000AEA).Store(0);
        b.Load(0).Branch(0x39,"fadeIn");b.Load(0).Integer(4).Branch(0x3B,"fadeIn");
        b.Load(0).Integer(1).Branch(0x3B,"firstIdle");b.Load(0).Integer(2).Branch(0x3B,"nextIdle");b.Load(0).Integer(3).Branch(0x3B,"wait");b.Integer(0).Op(0x2A);
        b.Label("fadeIn").Arg(0).Field(0x04000AEC).Store(1);N117_State(b,0x04000AEA,-1);
        b.Load(1).Static(0x04000AE8).Virtual(0x06000874);N118_FlowerWait(b,1);
        b.Label("firstIdle");N117_State(b,0x04000AEE,0);b.Branch(0x38,"idle");
        b.Label("nextIdle").Arg(0).Arg(0).Field(0x04000AEE).Integer(1).Op(0x58).Set(0x04000AEE);
        b.Label("idle").Arg(0).Field(0x04000AEC).Store(1);N117_State(b,0x04000AEA,-1);
        b.Arg(0).Field(0x04000AEE).Integer(3).Branch(0x3C,"fadeOut");
        b.Arg(0).Field(0x04000AED).Branch(0x3A,"ownerPresent").New(0x0A000022).Op(0x7A);
        b.Label("ownerPresent").Load(1).Virtual(0x0600086A).Virtual(0x06000830).Store(2);
        b.Label("choose").Integer(0).Static(0x04000AE7).Op(0x8E,0x69).Call(0x0A00012B).Store(4);
        b.Static(0x04000AE7).Load(4).Op(0x9A).Store(3);b.Load(3).Load(2).Call(0x0A0000B0).Branch(0x39,"choose");
        b.Arg(0).Field(0x04000AEC).Store(1);b.Load(1).Load(3).Virtual(0x06000874);N118_FlowerWait(b,2);
        b.Label("fadeOut").Load(1).Static(0x04000AE9).Virtual(0x06000874);N118_FlowerWait(b,3);
        b.Label("wait");N117_State(b,0x04000AEA,-1);N117_Yield(b,0x04000AEA,0x04000AEB,4,2);return b.Finish();
    }
    private static byte[] N118_MeshCacheInit()
    {
        var b=new IL();b.Integer(750).New(N118_MeshIndicesCtor).Ref(0x80,0x04000ADB);
        b.Call(0x0A0000FC).Ref(0x80,0x04000ADC);b.Integer(500).New(N118_MeshVerticesCtor).Ref(0x80,0x04000ADD);
        // Original Vector2.zero has the default all-zero value; no private Unity fields are accessed.
        b.Address(0).Op(0xFE).Ref(0x15,N118_Vector2Type);
        foreach(int f in new[]{0x04000ADE,0x04000ADF,0x04000AE0,0x04000AE1})b.Load(0).Ref(0x80,f);
        b.Integer(500).New(N118_MeshUvsCtor).Ref(0x80,0x04000AE2);
        b.Float(1).Float(1).Float(1).Float(1).New(N110_Vector4Ctor).Ref(0x80,0x04000AE3);
        b.Integer(500).New(N118_MeshAddColorsCtor).Ref(0x80,0x04000AE4);
        b.Float(1).Float(1).Float(1).Float(1).New(0x0A0000BE).Ref(0x80,0x04000AE5);
        b.Integer(500).New(N118_MeshMulColorsCtor).Ref(0x80,0x04000AE6).Op(0x2A);return b.Finish();
    }
    private static void N118_EditorValue(IL b,int source,int field,int ui,int conversion)
    {
        b.Arg(0).Field(source).Store(2);b.Arg(0).Field(ui).Store(0);
        b.Load(2).FieldAddress(field).Call(conversion).Store(1);b.Load(0).Load(1).Virtual(0x0A00002B);
    }
    private static void N118_InputArgId(IL b,int ui,int state)
    {
        b.Arg(0).Field(ui).Store(0);b.Arg(0).Arg(1).Set(state);b.ArgAddress(1).Call(0x0A00002A).Store(1);b.Load(0).Load(1).Virtual(0x0A00002B);
    }
    private static byte[] N118_LoadZombieInfo()
    {
        var b=new IL();b.Arg(1).Integer(0).Branch(0x3F,"negative");
        b.Arg(1).Arg(0).Field(0x04000095).Virtual(N118_ZombieInfoCount).Branch(0x3C,"outOfRange");
        N118_InputArgId(b,0x0400009A,0x04000098);
        b.Arg(0).Arg(0).Field(0x04000095).Arg(1).Virtual(N118_ZombieInfoItem).Set(0x04000096);
        b.Arg(0).Arg(0).Field(0x04000096).Field(0x0400017C).Set(0x04000097);
        b.Arg(0).Field(0x0400009B).Arg(0).Field(0x04000097).Virtual(0x0A00002B);
        foreach(var x in new[]{(Ui:0x0400009C,F:0x04000186,C:0x0A00002A),(Ui:0x0400009D,F:0x04000187,C:0x0A000043),(Ui:0x0400009E,F:0x04000189,C:0x0A00002A),(Ui:0x0400009F,F:0x04000182,C:0x0A000043),(Ui:0x040000A0,F:0x0400017F,C:0x0A000043),(Ui:0x040000A1,F:0x04000183,C:0x0A000043),(Ui:0x040000A2,F:0x04000184,C:0x0A000043)})N118_EditorValue(b,0x04000096,x.F,x.Ui,x.C);
        b.Arg(0).Field(0x04000096).Store(2);b.Arg(0).Field(0x040000A5).Load(2).Field(0x0400018D).Integer(1).Op(0x58).Virtual(0x0A000039);
        for(int f=0x0400009B;f<=0x040000A2;f++)N118_UiInteract(b,f,true);
        b.Arg(0).Field(0x04000096).Store(2);b.Arg(0).Field(0x040000A3).Load(2).Field(0x0400017D).Virtual(0x0A000017);
        b.Arg(0).Field(0x04000096).Store(2);b.Arg(0).Field(0x040000A4).Load(2).Field(0x0400017E).Virtual(0x0A000017);
        foreach(int f in new[]{0x040000A3,0x040000A4,0x040000A5})N118_UiInteract(b,f,true);b.Op(0x2A);
        b.Label("outOfRange").Arg(0).Arg(0).Field(0x04000098).Call(0x06000081).Op(0x2A);
        b.Label("negative").Arg(0).Integer(-1).Set(0x04000098);b.Arg(0).Field(0x0400009A).Ref(0x72,0x70001669).Virtual(0x0A00002B);
        b.Arg(0).Op(0x14).Set(0x04000096);b.Arg(0).Static(0x0A000013).Set(0x04000097);
        for(int f=0x0400009B;f<=0x040000A2;f++)N118_UiClear(b,f);
        for(int f=0x0400009B;f<=0x040000A2;f++)N118_UiInteract(b,f,false);
        b.Arg(0).Field(0x040000A3).Integer(0).Virtual(0x0A000017);b.Arg(0).Field(0x040000A4).Integer(0).Virtual(0x0A000017);
        foreach(int f in new[]{0x040000A3,0x040000A4,0x040000A5})N118_UiInteract(b,f,false);b.Op(0x2A);return b.Finish();
    }
    private static byte[] N118_LoadSkillEditor()
    {
        var b=new IL();N118_InputArgId(b,0x0400008D,0x04000088);
        b.Arg(1).Integer(1).Op(0x59).Integer(2).Branch(0x42,"invalid");
        b.Arg(0).Field(0x0400006F).Integer(0).Branch(0x3F,"noPlant");
        for(int id=1;id<=3;id++)b.Arg(1).Integer(id).Branch(0x3B,"slot"+id);
        b.Branch(0x38,"enabled");
        for(int id=1;id<=3;id++)
        {
            b.Label("slot"+id).Arg(0).Arg(0).Field(0x0400006B).Field(0x04000314).Arg(0).Field(0x0400006F).Virtual(N118_InitialPlantItem).Field(0x0400066D+id).Set(0x04000089).Branch(0x38,"enabled");
        }
        b.Label("enabled");N118_UiInteract(b,0x0400008A,true);
        b.Arg(0).Field(0x04000089).Field(0x0400068E).Branch(0x39,"emptySkill");
        b.Arg(0).Field(0x0400008A).Integer(1).Virtual(0x0A000017);
        N118_UiInteract(b,0x0400008E,true);b.Arg(0).Field(0x04000089).Store(2);b.Arg(0).Field(0x0400008E).Load(2).Field(0x0400068D).Virtual(0x0A00002B);
        N118_UiInteract(b,0x0400008F,true);N118_EditorValue(b,0x04000089,0x04000694,0x0400008F,0x0A000043);
        foreach(var x in new[]{(Ui:0x04000090,F:0x04000691),(Ui:0x04000091,F:0x04000690),(Ui:0x04000092,F:0x04000692)})
        {
            N118_UiInteract(b,x.Ui,true);b.Arg(0).Field(0x04000089).Store(2);b.Arg(0).Field(x.Ui).Load(2).Field(x.F).Integer(1).Op(0x58).Virtual(0x0A000039);
        }
        foreach(var x in new[]{(Ui:0x0400008B,F:0x04000696),(Ui:0x0400008C,F:0x04000697)})
        {
            N118_UiInteract(b,x.Ui,true);b.Arg(0).Field(0x04000089).Store(2);b.Arg(0).Field(x.Ui).Load(2).Field(x.F).Virtual(0x0A000017);
        }
        b.Op(0x2A);
        b.Label("emptySkill").Arg(0).Field(0x0400008A).Integer(0).Virtual(0x0A000017);
        foreach(int f in new[]{0x0400008E,0x0400008F}){N118_UiInteract(b,f,false);N118_UiClear(b,f);}
        foreach(int f in new[]{0x04000090,0x04000091,0x04000092}){N118_UiInteract(b,f,false);N118_UiDropdown(b,f,0);}
        foreach(int f in new[]{0x0400008B,0x0400008C}){N118_UiInteract(b,f,false);b.Arg(0).Field(f).Integer(0).Virtual(0x0A000017);}b.Op(0x2A);
        b.Label("noPlant").Arg(0).Integer(0).Call(0x06000075).Op(0x2A);
        b.Label("invalid").Arg(0).Op(0x14).Set(0x04000089);
        foreach(int f in new[]{0x0400008A,0x0400008E,0x0400008F})N118_UiInteract(b,f,false);
        foreach(int f in new[]{0x04000090,0x04000091,0x04000092}){N118_UiDropdown(b,f,0);N118_UiInteract(b,f,false);}
        foreach(int f in new[]{0x0400008B,0x0400008C})N118_UiInteract(b,f,false);b.Op(0x2A);return b.Finish();
    }
    private static byte[] N118_BankEntries()
    {
        var b=new IL();b.Static(0x04000151).Virtual(N118_StaticEntriesClear);
        for(int i=4;i<=11;i++)b.Float(i%2==0?1:0).Store(i);
        b.Arg(0).Field(0x040000E7).Field(0x040003B4).Field(0x040001A0).Virtual(N118_EntryEnum).Store(0);
        b.Label("tryEntries").Branch(0x38,"entryMove");
        b.Label("entry").Address(0).Call(N118_EntryCurrent).Store(1);b.Load(1).Field(0x040007FC).Branch(0x3A,"selected");b.Load(1).Field(0x040007FD).Branch(0x39,"entryMove");
        b.Label("selected");var kinds=new[]{(Kind:7,Local:4),(Kind:8,Local:5),(Kind:9,Local:6),(Kind:10,Local:7),(Kind:13,Local:8),(Kind:14,Local:9),(Kind:15,Local:10),(Kind:16,Local:11)};
        foreach(var x in kinds)b.Load(1).Field(0x040007FA).Integer(x.Kind).Branch(0x3B,"kind"+x.Kind);
        b.Load(1).Field(0x040007FA).Integer(26).Branch(0x40,"entryMove");b.Static(0x04000151).Load(1).Virtual(N118_StaticEntriesAdd).Branch(0x38,"entryMove");
        foreach(var x in kinds)
        {
            b.Label("kind"+x.Kind);
            if(x.Local is 6 or 7 or 10 or 11)b.Load(1).Virtual(0x06000607).Load(x.Local);
            else b.Load(x.Local).Load(1).Virtual(0x06000607);
            b.Op(0x58,0x6B).Store(x.Local).Branch(0x38,"entryMove");
        }
        b.Label("entryMove").Address(0).Call(N118_EntryMove).Branch(0x3A,"entry").Branch(0xDD,"afterEntries");
        b.Label("finallyEntries").Address(0).Call(N118_EntryDispose).Op(0xDC);
        b.Label("afterEntries");foreach(var x in new[]{(Value:6,Prefix:0x70003A89),(Value:7,Prefix:0x700037EC),(Value:10,Prefix:0x70003808),(Value:11,Prefix:0x70003834)})
            b.Ref(0x72,x.Prefix).Address(x.Value).Call(0x0A000043).Call(0x0A000051).Call(0x0A000010);
        b.Arg(0).Field(0x040000E9).Virtual(N118_CardEnum).Store(2);
        b.Label("tryCards").Branch(0x38,"cardMove");b.Label("card").Address(2).Call(N118_CardCurrent).Store(3);
        b.Load(3).Field(0x040003FC).Integer(-1).Branch(0x3B,"device");N118_ApplyCardModifiers(b,4,5,8,9,"plant");
        b.Label("device").Load(3).Field(0x040003FD).Integer(-1).Branch(0x3B,"cardMove");N118_ApplyCardModifiers(b,6,7,10,11,"device");
        b.Label("cardMove").Address(2).Call(N118_CardMove).Branch(0x3A,"card").Branch(0xDD,"return");b.Label("finallyCards").Address(2).Call(N118_CardDispose).Op(0xDC);
        b.Label("return").Op(0x2A);N118_Finally(b,0x060000E5,("tryEntries","finallyEntries","afterEntries"),("tryCards","finallyCards","return"));return b.Finish();
    }
    private static void N118_ApplyCardModifiers(IL b,int cm,int ca,int pm,int pa,string label)
    {
        b.Load(3).Load(3).Field(0x04000401).Load(cm).Op(0x5A,0x6B).Set(0x04000401);
        b.Load(3).Load(3).Field(0x04000401).Load(ca).Op(0x58,0x6B).Set(0x04000401);
        b.Load(3).Field(0x04000406).Op(0x6B).Load(pm).Op(0x5A,0x6B).Store(12);N102_EmitCvtt(b,12,label+"Price");b.Store(13);
        N102_EmitCvtt(b,pa,label+"Increase");b.Store(14);
        // Device native writes the multiplied integer before adding the increase.
        if(label=="device")b.Load(3).Load(13).Set(0x04000406);
        b.Load(3).Load(13).Load(14).Op(0x58).Set(0x04000406);
    }
    private static byte[] N118_SetPreCards()
    {
        var b=new IL();b.New(N118_EntryListCtor).Store(0);b.Arg(1).Field(0x040001A0).Virtual(N118_EntryEnum).Store(1);
        b.Label("tryFilter").Branch(0x38,"filterMove");b.Label("entry").Address(1).Call(N118_EntryCurrent).Store(2);
        b.Load(2).Field(0x040007FA).Integer(36).Branch(0x40,"filterMove");b.Load(0).Load(2).Virtual(N118_EntryListAdd);
        b.Label("filterMove").Address(1).Call(N118_EntryMove).Branch(0x3A,"entry").Branch(0xDD,"afterFilter");b.Label("finallyFilter").Address(1).Call(N118_EntryDispose).Op(0xDC);
        b.Label("afterFilter").New(N118_PreCardCtor).Store(3);b.Arg(1).Field(0x0400019F).Virtual(N118_PreCardEnum).Store(4);
        b.Label("tryPre").Branch(0x38,"preMove");b.Label("pre").Address(4).Call(N118_PreCardCurrent).Store(5);
        b.Load(5).Field(0x040003F7).Static(0x0A000013).Call(0x0A0000B0).Branch(0x39,"addPre");
        b.Load(0).Load(5).Field(0x040003F7).Call(0x0600060A).Branch(0x3A,"blocked");
        b.Label("addPre").Load(3).Load(5).Virtual(N118_PreCardAdd).Branch(0x38,"preMove");
        b.Label("blocked").Ref(0x72,0x70003D1C).Call(0x0A000010);
        b.Label("preMove").Address(4).Call(N118_PreCardMove).Branch(0x3A,"pre").Branch(0xDD,"afterPre");b.Label("finallyPre").Address(4).Call(N118_PreCardDispose).Op(0xDC);
        b.Label("afterPre").Load(3).Virtual(N118_PreCardEnum).Store(4);b.Label("tryCreate").Branch(0x38,"createMove");
        b.Label("create").Address(4).Call(N118_PreCardCurrent).Store(5);b.Static(0x040002FB).Call(0x2B000016).Store(6);
        b.Load(6).Arg(0).Field(0x040000E7).Set(0x040003FB);b.Load(6).Load(5).Virtual(0x060002DF);b.Load(6).Integer(1).Set(0x04000404);
        b.Arg(0).Field(0x040000E9).Virtual(N118_CardCount).Store(7);b.Arg(0).Field(0x040000E9).Load(6).Virtual(N118_CardAdd);
        b.Load(6).Load(7).Set(0x04000405);b.Arg(0).Load(6).Load(7).Call(0x060000EC);
        b.Arg(0).Field(0x040000E7).Call(0x0A00000B).Branch(0x39,"cardDone");b.Arg(0).Field(0x040000E7).Field(0x040003BA).Branch(0x39,"cardDone");
        b.Load(6).Integer(1).Virtual(0x060002D5);b.Load(6).Virtual(0x060002DA);
        b.Label("cardDone").Arg(0).Load(7).Integer(1).Op(0x58).Set(0x040000EC);b.Call(0x06000229).Op(0x26);
        b.Label("createMove").Address(4).Call(N118_PreCardMove).Branch(0x3A,"create").Branch(0xDD,"return");b.Label("finallyCreate").Address(4).Call(N118_PreCardDispose).Op(0xDC);
        b.Label("return").Op(0x2A);N118_Finally(b,0x060000ED,("tryFilter","finallyFilter","afterFilter"),("tryPre","finallyPre","afterPre"),("tryCreate","finallyCreate","return"));return b.Finish();
    }
    private static byte[] N118_RestrainCircle()
    {
        var b=new IL();b.Arg(0).Field(0x040000DD).Op(0x14).Call(0x0A000012).Branch(0x39,"transform");b.Call(0x0A0000FC).Op(0x2A);
        b.Label("transform").Arg(0).Field(0x040000DD).Virtual(0x0A000037).Store(0);b.New(0x0A000094).Store(1);b.New(0x0A000095).Store(2);
        b.Integer(0).Store(3).Branch(0x38,"centerTest");
        b.Label("center").Arg(0).Field(0x040000E5).Load(3).Virtual(N118_RangeCenterItem).Store(4);
        foreach(int f in new[]{0x0A00001D,0x0A00008B,0x0A00009D})b.Address(4).Address(0).Field(f).Address(4).Field(f).Op(0x58,0x6B).Set(f);
        b.Load(2).Load(4).Virtual(N118_RangeCentersAdd);b.Load(3).Integer(1).Op(0x58).Store(3);
        b.Label("centerTest").Load(3).Arg(0).Field(0x040000E4).Virtual(N118_RangeRadiusCount).Branch(0x3F,"center");
        b.Integer(0).Store(3).Branch(0x38,"distanceTest");
        b.Label("distance").Load(2).Load(3).Virtual(N118_RangeCenterItem).Store(4);
        int[] xyz=[0x0A00001D,0x0A00008B,0x0A00009D];for(int i=0;i<3;i++)b.ArgAddress(1).Field(xyz[i]).Address(4).Field(xyz[i]).Op(0x59,0x6B).Store(5+i);
        b.Load(6).Load(6).Op(0x5A,0x6B).Load(5).Load(5).Op(0x5A,0x6B,0x58,0x6B).Load(7).Load(7).Op(0x5A,0x6B,0x58,0x6B,0x6C).Call(0x0A00009F).Op(0x6B).Store(8);
        b.Load(1).Load(8).Virtual(N118_RangeDistanceAdd);
        b.Arg(0).Field(0x040000E4).Load(3).Virtual(0x0A000098).Load(1).Load(3).Virtual(0x0A000098).Branch(0x3C,"input");
        b.Load(3).Integer(1).Op(0x58).Store(3);b.Label("distanceTest").Load(3).Arg(0).Field(0x040000E4).Virtual(N118_RangeRadiusCount).Branch(0x3F,"distance");
        b.Load(1).Virtual(N118_RangeRadiusCount).Branch(0x39,"input");b.Integer(0).Store(9);b.Integer(1).Store(3).Branch(0x38,"minimumTest");
        b.Label("minimum").Load(1).Load(3).Virtual(0x0A000098).Store(8);b.Load(1).Load(9).Virtual(0x0A000098).Load(8).Branch(0x43,"minimumNext");b.Load(3).Store(9);
        b.Label("minimumNext").Load(3).Integer(1).Op(0x58).Store(3);b.Label("minimumTest").Load(3).Load(1).Virtual(N118_RangeRadiusCount).Branch(0x3F,"minimum");
        b.Load(2).Load(9).Virtual(N118_RangeCenterItem).Store(4);
        for(int i=0;i<3;i++)b.ArgAddress(1).Field(xyz[i]).Address(4).Field(xyz[i]).Op(0x59,0x6B).Store(5+i);
        b.Load(2).Load(9).Virtual(N118_RangeCenterItem).Store(4);b.Arg(0).Field(0x040000E4).Load(9).Virtual(0x0A000098).Load(1).Load(9).Virtual(0x0A000098).Op(0x5B,0x6B).Store(10);
        for(int i=0;i<3;i++)b.Address(4).Field(xyz[i]).Load(5+i).Load(10).Op(0x5A,0x6B,0x58,0x6B);
        b.New(0x0A00009C).Op(0x2A);b.Label("input").Arg(1).Op(0x2A);return b.Finish();
    }
    private static void N118_ShiftUi(IL b,bool loop)
    {
        // Captured transform and position are distinct from fresh owner field reads.
        b.Store(3);b.Load(3).Virtual(0x0A000037).Store(4);
        int[] xyz=[0x0A00001D,0x0A00008B,0x0A00009D];for(int i=0;i<3;i++)
        {
            b.Address(4);int first=loop||i==0?2:4,second=first==2?4:2;
            b.Address(first).Field(xyz[i]).Address(second).Field(xyz[i]).Op(0x58,0x6B).Set(xyz[i]);
        }
        b.Load(3).Load(4).Virtual(0x0A000038);
    }
    private static byte[] N118_Transplant()
    {
        var b=new IL();b.Arg(0).Virtual(0x0A000001).Virtual(0x0A000037).Store(0);
        b.Arg(0).Field(0x040004F3).Field(0x040003B4).Arg(1).Arg(2).Virtual(0x0600015F).Store(1);b.Arg(0).Load(1).Call(0x060003A3);
        foreach(int f in new[]{0x0A00001D,0x0A00008B,0x0A00009D})b.Address(2).Address(1).Field(f).Address(0).Field(f).Op(0x59,0x6B).Set(f);
        foreach(int f in new[]{0x040004EA,0x040004EC,0x040004EE}){b.Arg(0).Field(f).Virtual(0x0A00002E);N118_ShiftUi(b,false);}
        foreach(var x in new[]{(Field:0x040004EF,Name:"Characteristic"),(Field:0x040004F0,Name:"Particles")})
        {
            b.Arg(0).Field(x.Field).Virtual(N118_GameObjectEnum).Store(5);b.Label("try"+x.Name).Branch(0x38,"move"+x.Name);
            b.Label("each"+x.Name).Address(5).Call(N118_GameObjectCurrent).Store(6);b.Load(6).Call(0x0A00000B).Branch(0x39,"move"+x.Name);
            b.Load(6).Virtual(0x0A00002E);N118_ShiftUi(b,true);
            b.Label("move"+x.Name).Address(5).Call(N118_GameObjectMove).Branch(0x3A,"each"+x.Name).Branch(0xDD,"after"+x.Name);
            b.Label("finally"+x.Name).Address(5).Call(N118_GameObjectDispose).Op(0xDC);b.Label("after"+x.Name);
        }
        b.Arg(0).Field(0x040004F3).Arg(0).Field(0x040004A0).Arg(0).Field(0x040004A1).Virtual(0x060002C4).Arg(0).Virtual(0x0600029C);
        b.Arg(0).Arg(1).Set(0x040004A0);b.Arg(0).Arg(2).Set(0x040004A1);b.Arg(0).Call(0x060003A2);b.Arg(0).Integer(1).Set(0x040004BE);
        b.Arg(0).Load(1).Call(0x0600035B);b.Arg(0).Field(0x040004F3).Arg(1).Arg(2).Virtual(0x060002C4).Arg(0).Virtual(0x0600029A).Op(0x2A);
        N118_Finally(b,0x060003B5,("tryCharacteristic","finallyCharacteristic","afterCharacteristic"),("tryParticles","finallyParticles","afterParticles"));return b.Finish();
    }
    private static byte[] N118_PlaceDevice()
    {
        var b=new IL();b.Static(0x0400015E).Field(0x04000166).Field(0x04000310).Arg(0).Field(0x0400046F).Virtual(0x060004C6).Branch(0x3A,"recorded");
        b.Integer(3).Arg(0).Field(0x0400046F).Arg(0).Field(0x04000463).Call(0x06000672).Op(0x26);
        b.Label("recorded").Arg(0).Arg(1).Set(0x04000464);b.Arg(0).Arg(2).Set(0x04000465);
        b.Arg(0).Field(0x04000463).Arg(1).Arg(2).Virtual(0x060002C4).Store(0);
        b.Arg(0).Field(0x04000463).Field(0x040003B4).Arg(1).Arg(2).Virtual(0x0600015D).Store(1);
        b.Arg(0).Field(0x0400046F).Integer(11).Branch(0x40,"shadow");
        b.Load(0).Integer(2).Virtual(0x06000299).Store(2);b.Load(2).Op(0x14).Call(0x0A00000C).Branch(0x39,"offsetY");
        b.Load(2).Field(0x0400046F).Integer(6).Branch(0x3B,"offset54");b.Load(2).Field(0x0400046F).Integer(10).Branch(0x3B,"offset36");
        b.Float(28).Store(6).Branch(0x38,"offsetX");b.Label("offset54").Float(54).Store(6).Branch(0x38,"offsetX");b.Label("offset36").Float(36).Store(6);
        b.Label("offsetX").Address(1).Address(1).Field(0x0A00001D).Load(6).Op(0x58,0x6B).Set(0x0A00001D);
        b.Label("offsetY").Address(1).Address(1).Field(0x0A00008B).Float(5).Op(0x59,0x6B).Set(0x0A00008B);
        b.Label("shadow").Arg(0).Field(0x04000487).Virtual(0x0A00002E).Virtual(0x0A000037).Store(3);
        foreach(int f in new[]{0x0A00001D,0x0A00008B,0x0A00009D})b.Address(1).Address(1).Field(f).Address(3).Field(f).Op(0x59,0x6B).Set(f);
        b.Arg(0).Virtual(0x0A000001).Store(4);b.Load(4).Virtual(0x0A000037).Store(5);
        foreach(int f in new[]{0x0A00001D,0x0A00008B,0x0A00009D})b.Address(5).Address(1).Field(f).Address(5).Field(f).Op(0x58,0x6B).Set(f);
        b.Load(4).Load(5).Virtual(0x0A000038);
        b.Arg(0).Field(0x04000487).Virtual(0x0A00002E).Virtual(0x0A000037).Store(3);b.Arg(0).Address(3).Field(0x0A00001D).Set(0x04000466);
        b.Arg(0).Field(0x04000487).Virtual(0x0A00002E).Virtual(0x0A000037).Store(3);b.Arg(0).Address(3).Field(0x0A00008B).Set(0x04000467);
        b.Arg(0).Virtual(0x0A000001).Arg(0).Field(0x04000463).Field(0x040003D8).Virtual(0x0A000001).Virtual(N110_SetParent);
        // This original inlined sequence is the restored Device.SetLayer body already in scope.
        b.Arg(0).Call(0x06000332);b.Arg(0).Call(0x06000335);
        b.Arg(0).Field(0x04000487).Arg(0).Field(0x0400046F).Integer(4).Op(0xFE,0x01).Integer(0).Op(0xFE,0x01).Virtual(0x0A000007);
        b.Arg(0).Field(0x04000486).Call(0x0A00000B).Branch(0x39,"onField");b.Arg(0).Field(0x04000486).Ref(0x72,0x7000CA85).Float(1).Virtual(0x0A0001E5);
        b.Label("onField").Arg(0).Integer(1).Set(0x0400047C);b.Arg(0).Field(0x04000463).Field(0x040003D8).Arg(0).Virtual(0x06000181);
        b.Load(0).Virtual(0x06000295).Store(7);b.Load(0).Arg(0).Virtual(0x0600029D);b.Load(7).Op(0x6B).Store(6);
        b.Load(6).Load(0).Virtual(0x06000295).Op(0x6B).Branch(0x3B,"return");b.Arg(0).Field(0x04000463).Field(0x040003D5).Virtual(0x06000276);
        b.Label("return").Op(0x2A);return b.Finish();
    }
    private static byte[] N118_PlantSkillDetail()
    {
        var b=new IL();b.Arg(0).Field(0x040008C0).Arg(1).Virtual(0x060002F9);
        b.Arg(0).Field(0x040008C5).Integer(0).Virtual(N118_SkillTextItem).Arg(1).Field(0x0400068D).Integer(1).Virtual(0x0A00004B);
        b.Float(1).Float(1).Float(.2f).Float(1).New(0x0A0000BE).Call(0x0A00026C).Store(1);
        b.Arg(0).Field(0x040008C5).Integer(1).Virtual(N118_SkillTextItem).Store(0);
        b.Arg(0).Field(0x040008C0).Field(0x04000432).Field(0x0400068F).Store(2);b.Integer(10).Ref(0x8D,0x0100001A).Store(3);
        for(int i=0;i<9;i++)b.Load(3).Integer(i).Arg(1).Field(0x0400069C).Integer(i).Op(0x98).Ref(0x8C,0x0100001F).Op(0xA2);
        b.Load(3).Integer(9).Load(1).Op(0xA2);b.Load(2).Load(3).Call(0x0A00026D).Store(4);b.Load(0).Load(4).Integer(1).Virtual(0x0A00004B);
        b.Arg(0).Field(0x040008C5).Integer(2).Virtual(N118_SkillTextItem).Store(0);
        b.Ref(0x72,0x7001C99D).Arg(1).Field(0x04000698).Ref(0x8C,0x0100001F).Arg(1).Field(0x04000699).Ref(0x8C,0x0100001F).Call(0x0A00026F).Store(4);
        b.Load(0).Load(4).Integer(1).Virtual(0x0A00004B).Op(0x2A);return b.Finish();
    }
    private static void N118_PlayerSave(IL b)=>b.Static(0x0400015E).Field(0x04000166).Field(0x04000310);
    private static void N118_AppendSupplyItem(IL b)
    {
        b.Integer(1).Virtual(0x060004D9).Store(3);b.Arg(0).Field(0x04000717).Load(3).Virtual(N118_SuppliesItemAdd);
        b.Load(3).Virtual(0x0A000009).Virtual(0x0A00002E).Arg(0).Field(0x04000716).Virtual(0x0A00002E).Integer(0).Virtual(0x0A00002F);
    }
    private static byte[] N118_UpgradeSupplies(bool skill)
    {
        var b=new IL();b.Arg(0).Field(0x040006F5).Field(0x0400065D).Store(0);
        if(!skill)
        {
            foreach(int i in new[]{1,2,3,4})b.Arg(0).Field(0x0400070C).Integer(i).Branch(0x3B,"price"+i);
            b.Integer(0).Store(1).Branch(0x38,"clear");int[] prices=[0,500,1500,6000,25000];
            for(int i=1;i<=4;i++)b.Label("price"+i).Load(0).Integer(prices[i]).Op(0x5A).Store(1).Branch(0x38,"clear");
        }
        b.Label("clear").Integer(0).Store(2).Branch(0x38,"clearTest");
        b.Label("destroy").Arg(0).Field(0x04000717).Load(2).Virtual(N118_SuppliesItemItem).Virtual(0x0A000009).Call(0x0A000025);b.Load(2).Integer(1).Op(0x58).Store(2);
        b.Label("clearTest").Load(2).Arg(0).Field(0x04000717).Virtual(N118_SuppliesItemCount).Branch(0x3F,"destroy");b.Arg(0).Field(0x04000717).Virtual(N118_SuppliesItemClear);
        if(skill)
        {
            b.Arg(0).Field(0x040006F5).Field(0x04000663).Store(4);
            for(int i=1;i<=8;i++)b.Load(4).Integer(i).Branch(0x3B,"supplyType"+i);
            b.Integer(0).Store(5).Branch(0x38,"createMain");
            for(int i=1;i<=8;i++)b.Label("supplyType"+i).Integer(i<=2?28:i<=5?29:30).Store(5).Branch(0x38,"createMain");
        }
        b.Label("createMain");N118_PlayerSave(b);if(skill)b.Load(5);else b.Integer(5);b.Virtual(0x060004C4);N118_AppendSupplyItem(b);
        if(skill)
        {
            for(int i=1;i<=8;i++)b.Load(4).Integer(i).Branch(0x3B,"demand"+i);
            b.Integer(0).Store(5).Branch(0x38,"setDemand");
            b.Label("demand1").Load(0).Integer(2).Op(0x5B).Integer(1).Op(0x58).Store(5).Branch(0x38,"setDemand");
            foreach(int i in new[]{2,3})b.Label("demand"+i).Load(0).Integer(1).Op(0x58).Store(5).Branch(0x38,"setDemand");
            b.Label("demand4").Load(0).Integer(3).Op(0x5A).Integer(2).Op(0x5B).Integer(2).Op(0x58).Store(5).Branch(0x38,"setDemand");
            foreach(var x in new[]{(Id:5,Mul:2,Add:3),(Id:6,Mul:2,Add:1),(Id:7,Mul:3,Add:2),(Id:8,Mul:4,Add:3)})b.Label("demand"+x.Id).Load(0).Integer(x.Mul).Op(0x5A).Integer(x.Add).Op(0x58).Store(5).Branch(0x38,"setDemand");
        }
        else
        {
            for(int i=1;i<=4;i++)b.Arg(0).Field(0x0400070C).Integer(i).Branch(0x3B,"demand"+i);
            b.Integer(0).Store(5).Branch(0x38,"setDemand");
            b.Label("demand1").Load(0).Integer(2).Op(0x5B).Integer(1).Op(0x58).Store(5).Branch(0x38,"setDemand");
            foreach(var x in new[]{(Id:2,Mul:2),(Id:3,Mul:5),(Id:4,Mul:12)})b.Label("demand"+x.Id).Load(0).Integer(x.Mul).Op(0x5A).Store(5).Branch(0x38,"setDemand");
        }
        b.Label("setDemand").Load(3).Load(5).Set(0x04000591);if(!skill)b.Ref(0x72,0x7000CC9E).Call(0x0A000010);
        b.Static(0x0400015E).Field(0x04000166).Field(0x04000312).Arg(0).Field(0x040006F2);if(skill)b.Load(4);else b.Arg(0).Field(0x0400070C);
        b.Virtual(skill?0x06000253:0x06000252).Virtual(N118_SuppliesEnum).Store(6);
        b.Label("trySupplies").Branch(0x38,"moveSupply");b.Label("supply").Address(6).Call(N118_SuppliesCurrent).Store(7);
        N118_PlayerSave(b);b.Load(7).Field(0x04000678).Virtual(0x060004C4);N118_AppendSupplyItem(b);
        b.Load(3).Load(7).Field(0x0400067A).Set(0x04000591);
        b.Label("moveSupply").Address(6).Call(N118_SuppliesMove).Branch(0x3A,"supply").Branch(0xDD,"afterSupplies");b.Label("finallySupplies").Address(6).Call(N118_SuppliesDispose).Op(0xDC);
        b.Label("afterSupplies").Arg(0);if(skill)b.Integer(0);else b.Load(1);b.Set(0x0400071B).Op(0x2A);
        N118_Finally(b,skill?0x0600057C:0x0600057B,("trySupplies","finallySupplies","afterSupplies"));return b.Finish();
    }
    private static byte[] N118_WarpCtor()
    {
        var b=new IL();b.Integer(5).Ref(0x8D,0x01000092).Store(0);
        for(int i=0;i<5;i++)b.Load(0).Integer(i).Float(i*.25f).Float(i%2==0?0:2).New(0x0A000330).Ref(0xA4,0x01000092);
        b.Arg(0).Load(0).New(0x0A000331).Set(0x04000A60);
        foreach(int f in new[]{0x04000A61,0x04000A62,0x04000A63})b.Arg(0).Float(1).Set(f);
        b.Arg(0).Call(0x0A000014).Op(0x2A);return b.Finish();
    }
    private static byte[] N118_DevicePreviewState()
    {
        var b=new IL();b.Arg(0).Arg(1).Field(0x04000230).Set(0x04000463);b.Ref(0x72,0x7000CBAF).Call(0x0A000010);
        b.Arg(0).Field(0x04000488).Virtual(N118_GameObjectEnum).Store(0);b.Label("trySprites").Branch(0x38,"moveSprite");
        b.Label("sprite").Address(0).Call(N118_GameObjectCurrent).Store(1);b.Load(1).Virtual(0x2B000011).Virtual(0x0A0001E7).Store(2);
        b.Address(2).Address(2).Field(0x0A00021C).Float(.4f).Op(0x5A,0x6B).Set(0x0A00021C);
        b.Load(2).Ref(0x8C,0x01000034).Call(0x0A000010);b.Load(1).Virtual(0x2B000011).Load(2).Virtual(0x0A000097);
        b.Label("moveSprite").Address(0).Call(N118_GameObjectMove).Branch(0x3A,"sprite").Branch(0xDD,"afterSprites");b.Label("finallySprites").Address(0).Call(N118_GameObjectDispose).Op(0xDC);
        b.Label("afterSprites").Arg(0).Integer(0).Set(0x0400047C);
        b.Arg(0).Field(0x04000489).Op(0x14).Call(0x0A000012).Branch(0x39,"sorting");b.Arg(0).Arg(0).Field(0x04000485).Virtual(0x2B000057).Set(0x04000489);
        b.Label("sorting").Arg(0).Field(0x04000489).Ref(0x72,0x700068A4).Virtual(0x0A00015A);b.Arg(0).Field(0x04000489).Integer(9000).Virtual(0x0A00015B);
        b.Arg(0).Virtual(0x0A000001).Arg(1).Virtual(0x0A000001).Integer(0).Virtual(0x0A00002F);
        b.Arg(0).Field(0x04000487).Integer(0).Virtual(0x0A000007);
        b.Arg(0).Field(0x04000491).Call(0x0A00000B).Branch(0x39,"animator");b.Arg(0).Field(0x04000491).Integer(0).Virtual(0x0A000007);
        b.Label("animator").Arg(0).Field(0x04000486).Call(0x0A00000B).Branch(0x39,"return");b.Arg(0).Field(0x04000486).Integer(0).Virtual(0x0A0001E8).Store(4);
        b.Arg(0).Field(0x04000486).Store(5);b.Load(5).Address(4).Call(N118_AnimatorFullPathHash).Integer(0).Float(1f/60f).Virtual(0x0A0001E9);b.Arg(0).Field(0x04000486).Float(0).Virtual(0x0A0001EA);
        b.Label("return").Op(0x2A);N118_Finally(b,0x06000334,("trySprites","finallySprites","afterSprites"));return b.Finish();
    }
    private static void N118_SaveNameAudio(IL b)=>b.Static(0x040002EC).Integer(2).Virtual(0x0A000084).Call(0x0600013B).Op(0x26);
    private static byte[] N118_CreatePlayerSave()
    {
        var b=new IL();b.Static(0x0400015E).Field(0x04000166).Store(0);b.Load(0).Branch(0x39,"return");
        b.Arg(0).Field(0x040008FE).Static(0x0A000013).Call(0x0A00003A).Branch(0x3A,"empty");
        b.Load(0).Field(0x04000311).Field(0x0400064E).Virtual(N118_SaveEnum).Store(1);
        b.Label("tryNames").Branch(0x38,"moveName");b.Label("name").Address(1).Call(N118_SaveCurrent).Store(2);
        b.Load(2).Field(0x04000627).Arg(0).Field(0x040008FE).Call(0x0A00003A).Branch(0x39,"moveName");
        b.Arg(0).Field(0x040008FA).Ref(0x72,0x7001CE74).Integer(1).Virtual(0x0A00004B);N118_SaveNameAudio(b);b.Branch(0xDD,"return");
        b.Label("moveName").Address(1).Call(N118_SaveMove).Branch(0x3A,"name").Branch(0xDD,"afterNames");
        b.Label("finallyNames").Address(1).Call(N118_SaveDispose).Op(0xDC);
        b.Label("afterNames").Load(0).Arg(0).Field(0x040008FE).Virtual(0x0600023A);
        b.Call(0x06000636).Field(0x0400084B).Field(0x040008AC).Integer(0).Virtual(0x0A000007);
        b.Arg(0).Field(0x040008F5).Integer(2).Branch(0x40,"destroy");b.Call(0x06000636).Field(0x0400084E).Float(1).Virtual(0x0A0001EA);
        b.Label("destroy").Arg(0).Virtual(0x0A000009).Call(0x0A000025).Branch(0x38,"return");
        b.Label("empty").Arg(0).Field(0x040008FA).Ref(0x72,0x7001CF12).Integer(1).Virtual(0x0A00004B);N118_SaveNameAudio(b);
        b.Label("return").Op(0x2A);N118_Finally(b,0x060006F8,("tryNames","finallyNames","afterNames"));return b.Finish();
    }
    private static byte[] N118_PlantEditorLoad()
    {
        var b=new IL();b.Arg(1).Integer(0).Branch(0x3F,"negative");N118_InputArgId(b,0x04000077,0x0400006F);
        b.Arg(0).Arg(0).Field(0x0400006B).Field(0x04000315).Arg(1).Virtual(N118_PlantDataItem).Set(0x0400006E);
        b.Arg(0).Arg(0).Field(0x0400006E).Field(0x04000319).Set(0x04000070);b.Arg(0).Field(0x04000078).Arg(0).Field(0x04000070).Virtual(0x0A00002B);
        var stats=new[]{(Array:0x0400031F,Factor:0x04000320,Base:0x04000071,OwnFactor:0x04000072,BaseUi:0x04000079,FactorUi:0x0400007A,MaxUi:0x0400007F),(Array:0x04000321,Factor:0x04000322,Base:0x04000073,OwnFactor:0x04000074,BaseUi:0x0400007B,FactorUi:0x0400007C,MaxUi:0x04000080),(Array:0x04000324,Factor:0x04000325,Base:0x04000075,OwnFactor:0x04000076,BaseUi:0x0400007D,FactorUi:0x0400007E,MaxUi:0x04000081)};
        foreach(var x in stats)
        {
            b.Arg(0).Arg(0).Field(0x0400006E).Field(x.Array).Integer(0).Op(0x98).Set(x.Base);N118_OwnInputFloat(b,x.BaseUi,x.Base);
            b.Arg(0).Arg(0).Field(0x0400006E).Field(x.Factor).Set(x.OwnFactor);N118_OwnInputFloat(b,x.FactorUi,x.OwnFactor);
            b.Arg(0).Field(x.MaxUi).Store(3);b.Ref(0x72,0x7000118C).Arg(0).Field(0x0400006E).Field(x.Array).Integer(199).Ref(0x8F,0x0100001F).Call(0x0A000043).Call(0x0A000051).Store(1);
            b.Load(3).Load(1).Integer(1).Virtual(0x0A00004B);
        }
        for(int f=0x04000078;f<=0x0400007E;f++)N118_UiInteract(b,f,true);
        b.Arg(0).Arg(0).Field(0x04000082).Call(0x06000074);b.Arg(0).Arg(0).Field(0x04000088).Call(0x06000075);N118_UiInteract(b,0x04000093,true);N118_UiInteract(b,0x04000094,true);b.Op(0x2A);
        b.Label("negative").Arg(0).Integer(-1).Set(0x0400006F);b.Arg(0).Field(0x04000077).Ref(0x72,0x70001669).Virtual(0x0A00002B);b.Arg(0).Op(0x14).Set(0x0400006E);b.Arg(0).Static(0x0A000013).Set(0x04000070);N118_UiClear(b,0x04000078);
        foreach(var x in stats)
        {
            b.Arg(0).Float(0).Set(x.Base);N118_UiClear(b,x.BaseUi);b.Arg(0).Float(0).Set(x.OwnFactor);N118_UiClear(b,x.FactorUi);
            b.Arg(0).Field(x.MaxUi).Ref(0x72,0x7000118C).Integer(1).Virtual(0x0A00004B);
        }
        for(int f=0x04000078;f<=0x0400007E;f++)N118_UiInteract(b,f,false);
        b.Arg(0).Integer(-1).Call(0x06000074);b.Arg(0).Integer(0).Call(0x06000075);N118_UiInteract(b,0x04000093,false);N118_UiInteract(b,0x04000094,false);b.Op(0x2A);return b.Finish();
    }
    private static void N118_OwnInputFloat(IL b,int ui,int field)
    {
        b.Arg(0).Field(ui).Store(0);b.Arg(0).FieldAddress(field).Call(0x0A000043).Store(1);b.Load(0).Load(1).Virtual(0x0A00002B);
    }
    private static void N118_DetailButton(IL b)=>b.Arg(0).Field(0x040008C7).Load(2).Virtual(0x0A000006);
    private static void N118_DetailChild(IL b,int child)
    {
        N118_DetailButton(b);b.Virtual(0x0A00002E).Integer(child).Virtual(0x0A000002);
    }
    private static void N118_DetailChildActive(IL b,int child,bool active)
    {
        N118_DetailChild(b,child);b.Virtual(0x0A000009).Integer(active?1:0).Virtual(0x0A000007);
    }
    private static byte[] N118_PlantDetailLoad()
    {
        var b=new IL();b.Arg(0).Field(0x040008C0).Field(0x0400042F).Store(0);
        b.Arg(0).Field(0x040008C3).Load(0).Field(0x04000656).Integer(1).Virtual(0x0A00004B);
        for(int i=0;i<5;i++)b.Arg(0).Field(0x040008C4).Integer(i).Virtual(N118_PlantDataUiItem).Load(0).Field(0x04000667+i).Virtual(0x060006CB);
        b.Arg(0).Field(0x040008C4).Integer(5).Virtual(N118_PlantDataUiItem).Arg(0).Field(0x040008C0).Field(0x04000430).Op(0x6B).Virtual(0x060006CB);
        b.Arg(0).Field(0x040008C4).Integer(6).Virtual(N118_PlantDataUiItem).Load(0).Field(0x04000665).Virtual(0x060006CB);
        b.Arg(0).Field(0x040008C9).Integer(0).Virtual(N118_SkillTextItem).Load(0).Field(0x04000659).Integer(1).Virtual(0x0A00004B);
        b.Arg(0).Load(0).Call(0x060006C4);b.Arg(0).Arg(0).Field(0x040008C0).Field(0x04000432).Field(0x0400068E).Call(0x060006C5);
        b.Integer(0).Store(2);b.Label("skill").Arg(0).Field(0x040008C0).Field(0x0400042F).Load(2).Integer(1).Op(0x58).Virtual(0x060004CB).Store(1);
        b.Load(1).Branch(0x39,"nextSkill");b.Load(1).Field(0x04000693).Branch(0x39,"locked");b.Load(1).Field(0x0400068E).Branch(0x39,"locked");
        b.Load(0).Field(0x0400065E).Integer(3).Op(0x5A).Load(1).Field(0x0400068E).Op(0x58).Store(3);
        N118_DetailChild(b,1);b.Virtual(0x2B000054).Store(4);b.Load(4).Load(3).Load(1).Field(0x0400068D).Call(0x0600021E).Virtual(0x0A000041);
        N118_DetailChildActive(b,1,true);N118_DetailChildActive(b,4,false);N118_DetailButton(b);b.Virtual(0x2B000091).Integer(1).Virtual(0x0A000208);N118_DetailChildActive(b,5,true);
        N118_DetailChild(b,5);b.Virtual(0x2B000092).Store(5);b.Ref(0x72,0x70018DDD).Load(0).FieldAddress(0x04000663).Call(0x0A00002A).Call(0x0A000051).Store(6);b.Load(5).Load(6).Integer(1).Virtual(0x0A00004B).Branch(0x38,"nextSkill");
        b.Label("locked");N118_DetailChildActive(b,1,false);N118_DetailChildActive(b,4,true);N118_DetailButton(b);b.Virtual(0x2B000091).Integer(0).Virtual(0x0A000208);N118_DetailChildActive(b,5,false);
        b.Label("nextSkill").Load(2).Integer(1).Op(0x58).Store(2);b.Load(2).Integer(3).Branch(0x3F,"skill");
        b.Integer(0).Store(2).Branch(0x38,"infoTest");b.Label("info").Load(2).Branch(0x39,"skillInfo");b.Load(2).Integer(1).Branch(0x3B,"selectInfo");b.Load(2).Integer(2).Branch(0x40,"nextInfo");
        b.Arg(0).Field(0x040008C0).Field(0x0400042F).Virtual(0x060004CF).Integer(1).Branch(0x3C,"selectInfo").Branch(0x38,"nextInfo");
        b.Label("skillInfo").Arg(0).Field(0x040008C0).Field(0x0400042F).Virtual(0x060004CC).Integer(1).Branch(0x3C,"selectInfo");
        b.Label("nextInfo").Load(2).Integer(1).Op(0x58).Store(2);b.Label("infoTest").Load(2).Arg(0).Field(0x040008C1).Virtual(N118_PlantInfoCount).Branch(0x3F,"info").Branch(0x38,"return");
        b.Label("selectInfo").Arg(0).Load(2).Call(0x060006C7);b.Label("return").Op(0x2A);return b.Finish();
    }
    private static void N118_ZombieSprite(IL b,int name,bool appear=false)
        =>b.Arg(0).Field(0x04000609).Ref(0x72,name).Call(appear?0x0600013F:0x06000145);
    private static byte[] N118_DropArmor1()
    {
        var b=new IL();b.Arg(1).Branch(0x3A,"names");b.Arg(0).Field(0x040005C2).Store(0);
        foreach(int kind in new[]{1,2,5,6})b.Load(0).Integer(kind).Branch(0x3B,"particle"+kind);b.Branch(0x38,"names");
        foreach(var x in new[]{(Kind:1,Particle:3,Name:0x70010F54),(Kind:2,Particle:1,Name:0x70010F68),(Kind:5,Particle:8,Name:0x70010F68),(Kind:6,Particle:27,Name:N118_LadderHead)})
        {
            b.Label("particle"+x.Kind).Call(0x060001E5).Integer(x.Particle).Virtual(0x060001EA).Store(4);
            b.Arg(0).Field(0x04000609).Ref(0x72,x.Name).Call(0x06000141).Store(5);b.Load(4).Virtual(0x0A00002E).Load(5).Virtual(0x0A000038).Branch(0x38,"names");
        }
        b.Label("names").New(N118_StringListCtor).Store(1);b.Arg(0).Field(0x040005C2).Store(0);
        for(int kind=1;kind<=6;kind++)b.Load(0).Integer(kind).Branch(0x3B,"name"+kind);b.Branch(0x38,"enumerate");
        int[][] names=[[0x70010F54],[0x70010F68],[0x70010F80],[0x7000813C,0x7000814E],[0x70010F68],[0x7000857E,0x700084EE]];
        for(int kind=1;kind<=6;kind++)
        {
            b.Label("name"+kind);foreach(int name in names[kind-1])b.Load(1).Ref(0x72,name).Virtual(N118_StringListAdd);
            if(kind==6){b.Arg(0).Field(0x040005B9).Integer(16).Branch(0x40,"enumerate");b.Arg(0).Float(1).Set(0x040005E6);}
            b.Branch(0x38,"enumerate");
        }
        b.Label("enumerate").Load(1).Virtual(N118_StringEnum).Store(2);b.Label("tryNames").Branch(0x38,"move");
        b.Label("each").Address(2).Call(N118_StringCurrent).Store(3);b.Arg(0).Field(0x04000609).Load(3).Call(0x06000145);
        b.Label("move").Address(2).Call(N118_StringMove).Branch(0x3A,"each").Branch(0xDD,"afterNames");b.Label("finallyNames").Address(2).Call(N118_StringDispose).Op(0xDC);
        b.Label("afterNames");N118_ZombieSprite(b,0x7001025D,true);
        foreach(int f in new[]{0x040005C3,0x040005C4,0x040005C5})b.Arg(0).Float(0).Set(f);
        foreach(int f in new[]{0x040005C2,0x040005C7})b.Arg(0).Integer(0).Set(f);b.Op(0x2A);N118_Finally(b,0x06000439,("tryNames","finallyNames","afterNames"));return b.Finish();
    }
    private static byte[] N118_DropArmor2()
    {
        var b=new IL();b.Arg(1).Branch(0x3A,"sprites");b.Arg(0).Field(0x040005C8).Store(3);
        for(int kind=1;kind<=4;kind++)b.Load(3).Integer(kind).Branch(0x3B,"particle"+kind);b.Branch(0x38,"sprites");
        foreach(var x in new[]{(Kind:1,Particle:28,Name:0x700102A9),(Kind:2,Particle:20,Name:0x70004533),(Kind:3,Particle:9,Name:0x70010275),(Kind:4,Particle:17,Name:0x70008458)})
        {
            b.Label("particle"+x.Kind).Call(0x060001E5).Integer(x.Particle).Virtual(0x060001EA).Store(1);
            b.Arg(0).Field(0x04000609).Ref(0x72,x.Name).Call(0x06000141).Store(2);b.Load(1).Virtual(0x0A00002E).Load(2).Virtual(0x0A000038).Branch(0x38,"sprites");
        }
        b.Label("sprites").Arg(0).Field(0x040005C8).Store(3);b.Ref(0x72,0x70004533).Store(0);
        foreach(int kind in new[]{1,3,4})b.Load(3).Integer(kind).Branch(0x3B,"kind"+kind);b.Branch(0x38,"finishSprites");
        b.Label("kind4");foreach(int name in new[]{0x70008458,0x700102CD})N118_ZombieSprite(b,name);
        foreach(int name in new[]{0x700102FF,0x7000FEA3})N118_ZombieSprite(b,name,true);
        b.Arg(0).Field(0x040005FE).Ref(0x72,0x70008D27).Integer(0).Virtual(0x0A0001F3);b.Arg(0).Arg(0).Field(0x040005F8).Integer(1000).Op(0x59).Set(0x040005F8);b.Arg(0).Call(0x0600046A).Op(0x26).Branch(0x38,"finishSprites");
        b.Label("kind3");foreach(int name in new[]{0x70010329,0x7001035F,0x70010397})N118_ZombieSprite(b,name);
        foreach(int name in new[]{0x7000FFAB,0x700080F4,0x7000FF7F,0x700103CD,0x700103FB,0x70010429})N118_ZombieSprite(b,name,true);
        b.Ref(0x72,0x70010275).Store(0).Branch(0x38,"finishSprites");
        b.Label("kind1");N118_ZombieSprite(b,0x70010329);foreach(int name in new[]{0x7000FFAB,0x700080F4,0x7000FF7F})N118_ZombieSprite(b,name,true);
        b.Arg(0).Field(0x040005B9).Integer(1).Branch(0x3B,"screenDoor");b.Arg(0).Field(0x040005B9).Integer(10).Branch(0x3B,"screenDoor");
        foreach(int name in new[]{0x7001035F,0x70010397})N118_ZombieSprite(b,name);foreach(int name in new[]{0x700103CD,0x700103FB,0x70010429})N118_ZombieSprite(b,name,true);
        b.Label("screenDoor").Ref(0x72,0x700102A9).Store(0);
        b.Label("finishSprites").Arg(0).Field(0x04000609).Load(0).Call(0x06000145);
        foreach(int f in new[]{0x040005C9,0x040005CA,0x040005CB})b.Arg(0).Float(0).Set(f);foreach(int f in new[]{0x040005C8,0x040005CD})b.Arg(0).Integer(0).Set(f);
        b.Arg(0).Float(0).Float(1).Ref(0x72,0x7000FD5B).Call(0x0600043C);b.Arg(0).Field(0x040005D0).Branch(0x39,"return");b.Arg(0).Arg(1).Call(0x0600042F);
        b.Label("return").Op(0x2A);return b.Finish();
    }
    private static byte[] N118_Benchmark3()
    {
        var b=new IL();b.Op(0x14).Store(0);b.Arg(0).Field(0x04000958).Store(1);
        for(int mode=0;mode<=3;mode++)b.Load(1).Integer(mode).Branch(0x3B,"font"+mode);b.Branch(0x38,"samples");
        for(int mode=0;mode<=3;mode++)
        {
            b.Label("font"+mode).Arg(0).Field(0x04000959).Integer(90).Integer(9).Integer(mode==3?0x1015:0x1045).Integer(256).Integer(256).Integer(1).Integer(1).Call(0x0A000308).Store(0);
            if(mode==1||mode==2)b.Load(0).Field(0x0A00030A).Ref(0x72,mode==1?0x7001F826:0x7001F7F0).Call(0x0A000309).Virtual(0x0A00030B);
            b.Branch(0x38,"samples");
        }
        b.Label("samples").Integer(0).Store(2).Branch(0x38,"sampleTest");
        b.Label("sample").Arg(0).Field(0x04000958).Store(1);b.Load(1).Integer(3).Branch(0x44,"other");
        b.New(0x0A0000E2).Store(3);N118_BenchmarkPosition(b);
        b.Load(3).Virtual(0x2B00009A).Store(4);b.Load(4).Load(0).Virtual(0x0A0002F9);b.Load(4).Float(128).Virtual(0x0A0002A3);
        b.Load(4).Ref(0x72,0x7001FA48).Virtual(0x0A000062);b.Load(4).Integer(514).Virtual(0x0A0002FA);b.Load(4).Float(1).Float(1).Float(0).Float(1).New(0x0A0000BE).Virtual(0x0A0001DC);
        b.Arg(0).Field(0x04000958).Integer(3).Branch(0x40,"next");b.Load(4).Float(132).Virtual(0x0A0002A3).Branch(0x38,"next");
        b.Label("other").Load(1).Integer(4).Branch(0x40,"next");b.New(0x0A0000E2).Store(3);N118_BenchmarkPosition(b);
        b.Load(3).Virtual(0x2B000099).Store(5);b.Load(5).Virtual(0x2B00001F).Arg(0).Field(0x04000959).Virtual(0x0A0002F5).Virtual(N115_SharedMaterial);
        b.Load(5).Arg(0).Field(0x04000959).Virtual(0x0A0002FF);b.Load(5).Integer(4).Virtual(0x0A0002F8);b.Load(5).Integer(130).Virtual(0x0A0002F7);
        b.Load(5).Float(1).Float(1).Float(0).Float(1).New(0x0A0000BE).Virtual(0x0A000303);b.Load(5).Ref(0x72,0x7001FA48).Virtual(0x0A0002FD);
        b.Label("next").Load(2).Integer(1).Op(0x58).Store(2);b.Label("sampleTest").Load(2).Arg(0).Field(0x04000957).Branch(0x3F,"sample").Op(0x2A);return b.Finish();
    }
    private static void N118_BenchmarkPosition(IL b)=>b.Load(3).Virtual(0x0A00002E).Float(0).Float(1.2f).Float(0).New(0x0A00009C).Virtual(0x0A000038);
    private static void N118_FloatingStartPosition(IL b)
    {
        b.Arg(0).Arg(0).Field(0x040009BD).Virtual(0x0A00002E).Set(0x040009C1);b.Arg(0).Field(0x040009C1).Store(2);
        b.Arg(0).Field(0x040009C0).Virtual(0x0A000037).Store(0);
        foreach(int f in new[]{0x0A00001D,0x0A00008B,0x0A00009D})b.Address(0).Address(0).Field(f).Float(f==0x0A00008B?15:0).Op(0x58,0x6B).Set(f);
        b.Load(2).Load(0).Virtual(0x0A000038);
    }
    private static void N118_RandomColor(IL b)
    {
        for(int i=5;i<8;i++)b.Integer(0).Integer(255).Call(0x0A00012B).Op(0xD2).Store(i);
        for(int i=5;i<8;i++)b.Load(i).Op(0x6B).Float(255).Op(0x5B,0x6B);b.Float(1).New(0x0A0000BE);
    }
    private static byte[] N118_FloatingStart()
    {
        var b=new IL();b.Arg(0).Field(0x040009C5).Branch(0x39,"tmp");b.Arg(0).Field(0x040009C5).Integer(1).Branch(0x40,"return");
        N118_FloatingStartPosition(b);b.Arg(0).Arg(0).Field(0x040009BD).Virtual(0x2B000099).Set(0x040009BF);
        b.Arg(0).Field(0x040009BF).Ref(0x72,0x7001E504).Call(0x2B0000A0).Virtual(0x0A0002FF);
        b.Arg(0).Field(0x040009BF).Virtual(0x2B00001F).Arg(0).Field(0x040009BF).Virtual(0x0A0002F4).Virtual(0x0A0002F5).Virtual(N115_SharedMaterial);
        b.Arg(0).Field(0x040009BF).Store(3);b.Load(3);N118_RandomColor(b);b.Virtual(0x0A000303);
        b.Arg(0).Field(0x040009BF).Integer(7).Virtual(0x0A0002F8);b.Arg(0).Field(0x040009BF).Integer(24).Virtual(0x0A0002F7);
        b.Arg(0).Arg(0).Call(0x0600079A).Virtual(0x0A00032B).Op(0x26).Branch(0x38,"return");
        b.Label("tmp").Arg(0).Arg(0).Field(0x040009BD).Virtual(0x2B00009A).Set(0x040009BE);
        b.Address(1).Float(3).Set(0x0A0001D4);b.Address(1).Float(3).Set(0x0A0001D5);b.Arg(0).Field(0x040009BE).Virtual(0x0A0002E3).Load(1).Virtual(0x0A00028E);
        N118_FloatingStartPosition(b);b.Arg(0).Field(0x040009BE).Integer(514).Virtual(0x0A0002FA);b.Arg(0).Field(0x040009BE).Store(4);
        b.Load(4);N118_RandomColor(b);b.Virtual(0x0A0001DC);
        b.Arg(0).Field(0x040009BE).Float(24).Virtual(0x0A0002A3);b.Arg(0).Field(0x040009BE).Integer(0).Virtual(0x0A000306);
        b.Arg(0).Field(0x040009BE).Static(0x0A000013).Virtual(0x0A000062);b.Arg(0).Field(0x040009BE).Arg(0).Field(0x040009C6).Virtual(0x0A000307);
        b.Integer(0).New(0x060007A3).Store(8);b.Load(8).Arg(0).Set(0x040009D5);b.Arg(0).Load(8).Virtual(0x0A00032B).Op(0x26);
        b.Label("return").Op(0x2A);return b.Finish();
    }
    private static byte[] N118_SphShot()
    {
        var b=new IL();b.Arg(0).Field(0x040005AA).Field(0x040003D6).Integer(26).Virtual(0x060001F6).Store(0);
        b.Arg(0).Field(0x04000609).Ref(0x72,0x70014912).Call(0x06000141).Store(1);b.Arg(0).Integer(26).Load(0).Integer(0).Call(0x06000443).Store(2);
        b.Load(0).Load(1).Float(-1000).Float(0).Float(0).New(0x0A00009C).Call(0x0A0000FC).Float(430).Float(1880).Integer(5).Float(62).Float(0).Float(0).Arg(0).Virtual(N118_InitialFullZombie);
        foreach(var x in new[]{(Target:0x04000601,X:0x040004A4,Y:0x040004A5),(Target:0x04000603,X:0x04000466,Y:0x04000467),(Target:0x04000602,X:0x040005AD,Y:0x040005AE)})
        {
            b.Arg(0).Field(x.Target).Op(0x14).Call(0x0A00000C).Branch(0x39,"after"+x.Target);
            b.Load(0).FieldAddress(0x04000569).Arg(0).Field(x.Target).Field(x.X).Set(0x0A00001D);
            b.Load(0).FieldAddress(0x04000569).Arg(0).Field(x.Target).Field(x.Y).Set(0x0A00008B);b.Load(0).FieldAddress(0x04000569).Float(0).Set(0x0A00009D);
            b.Label("after"+x.Target);
        }
        b.Load(0).Load(2).Arg(0).Field(0x040005F1).Virtual(0x060003F7);
        b.Arg(0).Field(0x040005E3).Integer(1).Op(0x59).Store(3); // Counter is an Int32 local; no object/int aliasing.
        b.Arg(0).Integer(0).Set(0x040005E2);b.Arg(0).Load(3).Set(0x040005E3);b.Load(3).Integer(0).Branch(0x3D,"audio");
        b.Arg(0).Field(0x040005FE).Ref(0x72,0x7000FD4F).Integer(0).Virtual(0x0A0001F3);b.Arg(0).Integer(0).Set(0x040005E1);
        b.Label("audio").Static(0x040002EF).Integer(8).Virtual(0x0A000084).Store(4);b.Call(0x0A000004).Virtual(0x0A000001).Virtual(0x0A000037).Store(5);
        b.Call(0x06000138).Store(6);b.Load(4).Load(5).Load(6).Load(6).Op(0x58,0x6B).Call(0x0600013D).Op(0x26).Op(0x2A);return b.Finish();
    }
    private static byte[] N118_SummonSaber()
    {
        var b=new IL();b.Arg(0).Field(0x040004F3).Field(0x040003B5).Store(0);
        b.Arg(0).Field(0x040004F3).Field(0x040003B4).Integer(0).Integer(0).Virtual(0x0600015C).Store(1);
        b.Arg(0).Field(0x040004F3).Field(0x040003B4).Store(14); // Captured before both map getters.
        b.Load(0).Virtual(0x06000289).Integer(1).Op(0x59).Store(3);b.Load(0).Virtual(0x0600028A).Integer(1).Op(0x59).Store(4);
        b.Load(14).Load(3).Load(4).Virtual(0x0600015C).Store(2);
        b.Address(1).Field(0x0A00001D).Address(2).Field(0x0A00001D).Call(0x0A000083).Store(5);
        b.Address(2).Field(0x0A00008B).Address(1).Field(0x0A00008B).Call(0x0A000083).Store(6);
        b.Arg(0).Field(0x040004F3).Field(0x040003D6).Integer(30).Virtual(0x060001F6).Store(7);
        b.Load(6).Float(1800).Op(0x58,0x6B).Store(6);b.Arg(0).Load(7).Integer(30).Integer(0).Call(0x0600036D).Store(8);
        b.Integer(0).Integer(5).Call(0x0A00012B).Store(9);for(int i=1;i<=4;i++)b.Load(9).Integer(i).Branch(0x3B,"angle"+i);
        b.Float(0).Store(10).Branch(0x38,"trig");float[] angles=[0,-8.03f,-4f,4.42f,6.25f];for(int i=1;i<=4;i++)b.Label("angle"+i).Float(angles[i]).Store(10).Branch(0x38,"trig");
        b.Label("trig").Load(10).Op(0x6C);N118_Double(b,BitConverter.Int64BitsToDouble(0x3F91DF46A2529D39L));b.Op(0x5A).Store(11);
        b.Load(11).Call(0x0A0001FF).Op(0x6B).Float(4800).Op(0x5A,0x6B).Store(12);b.Load(11).Call(0x0A000200).Op(0x6B).Float(-4800).Op(0x5A,0x6B).Store(13);
        b.Load(7).Load(5).Load(6).Float(0).New(0x0A00009C).Load(12).Float(0).Float(0).New(0x0A00009C).Call(0x0A0000FC).Float(1800).Load(13).Integer(0).Load(10).Call(0x0A0000E9).Float(0).Float(0).Arg(0).Virtual(N118_InitialFullPlant);
        b.Load(7).Load(8).Arg(0).Field(0x040004C8).Virtual(0x060003F7);b.Load(7).Load(9).Set(0x0400055C);b.Arg(0).Float(0).Set(0x040004FD).Op(0x2A);return b.Finish();
    }
    private static byte[] N118_Repearbalester()
    {
        var b=new IL();b.Arg(0).Field(0x040004D2).Branch(0x3A,"skill");
        b.Arg(0).Field(0x040004DF).Ref(0x72,0x7000E6AA).Call(0x06000141).Store(0);b.Arg(0).Field(0x040004DF).Ref(0x72,0x7000E6D2).Call(0x06000141).Store(1);
        b.Arg(0).Field(0x040004F3).Field(0x040003D6).Integer(24).Virtual(0x060001F6).Store(2);b.Arg(0).Field(0x040004F3).Field(0x040003D6).Integer(24).Virtual(0x060001F6).Store(3);
        b.Arg(0).Load(2).Integer(24).Integer(0).Call(0x0600036D).Store(4);b.Arg(0).Load(3).Integer(24).Integer(0).Call(0x0600036D).Store(5);
        for(int i=0;i<2;i++)
        {
            b.Load(2+i).Load(i).Float(1555).Float(0).Float(0).New(0x0A00009C).Call(0x0A0000FC).Float(70).Integer(0).Arg(0).Virtual(N118_InitialShortPlant);
            b.Load(2+i).Load(4+i).Arg(0).Field(0x040004C8).Virtual(0x060003F7);
        }
        b.Arg(0).Call(0x06000360).Store(6);b.Load(6).Call(0x0A00000B).Branch(0x3A,"zombie");b.Arg(0).Call(0x06000363).Store(7);
        b.Load(7).Op(0x14).Call(0x0A000012).Branch(0x3A,"return");b.Load(7).Field(0x04000466).Store(9);b.Load(7).Field(0x04000467).Store(10).Branch(0x38,"aim");
        b.Label("zombie").Load(6).Field(0x040005AD).Store(9);b.Load(6).Field(0x040005AE).Store(10);
        b.Label("aim").Load(9).Load(2).Field(0x04000552).Op(0x59,0x6B).Load(10).Load(2).Field(0x04000553).Op(0x59,0x6B).Float(0).New(0x0A00009C).Store(8);
        b.Load(2).Load(8).Virtual(0x060003DC);b.Load(3).Load(8).Virtual(0x060003DC).Branch(0x38,"reset");
        b.Label("skill").Arg(0).Field(0x040004D1).Arg(0).Virtual(0x060004E0);b.Label("reset").Arg(0).Integer(0).Set(0x040004B6);
        b.Label("return").Op(0x2A);return b.Finish();
    }
    private static byte[] N118_SkillAwake10()
    {
        var b=new IL();b.Arg(0).Field(0x040004D1).Field(0x0400069C).Integer(0).Op(0x98).Float(3).Op(0x5A,0x6B).Store(0);
        b.Ref(0x72,0x700044E5).Load(0).Integer(0).New(0x060000F6).Store(1);
        b.Load(1).Integer(2).Set(0x04000106);b.Load(1).Integer(1).Set(0x04000108);b.Load(1).Arg(0).Set(0x04000109);
        b.Load(1).Field(0x0400010D).Store(2);b.Arg(0).Field(0x040004D1).Field(0x0400069C).Integer(1).Op(0x98).Float(3).Op(0x5A,0x6B).Store(0);
        b.Ref(0x72,0x7000450D).Load(0).Integer(1).New(0x060000F6).Store(3);b.Load(3).Integer(1).Set(0x04000108);b.Load(3).Arg(0).Set(0x04000109);
        b.Load(2).Load(3).Virtual(N118_ChildBuffAdd);b.Arg(0).Field(0x040004FF).Load(1).Virtual(0x06000102);
        b.Arg(0).Field(0x040004F3).Field(0x040003B4).Store(4);b.Call(0x060001E5).Integer(30).Virtual(0x060001EA).Store(5);
        b.Load(5).Virtual(0x2B000069).Arg(0).Integer(1).Float(0).Ref(0x72,0x70001D5D).Virtual(N118_BindVfxPlant);
        b.Load(5).Virtual(0x2B000069).Integer(30).Arg(0).Virtual(0x2B00006A);
        b.Load(5).Virtual(0x0A00002E).Store(6);b.Load(4).Arg(0).Field(0x040004A0).Integer(1).Op(0x58).Arg(0).Field(0x040004A1).Virtual(0x0600015B).Store(7);
        b.Address(7).Field(0x0A00001D).Store(8);b.Load(4).Arg(0).Field(0x040004A0).Arg(0).Field(0x040004A1).Virtual(0x0600015C).Store(7);
        b.Load(6).Load(8).Address(7).Field(0x0A00008B).Float(0).New(0x0A00009C).Virtual(0x0A000038).Op(0x2A);return b.Finish();
    }
    private static void N118_Skill149Damage(IL b,int target,int method,float height)
    {
        b.Arg(0).Op(0x14).Integer(-1).Virtual(0x0600036C).Store(3);
        b.Load(3).Arg(0).Field(0x040004D1).Field(0x0400069C).Integer(0).Op(0x98).Load(3).Field(0x04000123).Op(0x5A,0x6B).Set(0x04000123);
        b.Load(target).Load(3).Op(0x14).Virtual(method);b.Call(0x060001E5).Integer(34).Virtual(0x060001EA).Store(4);
        b.Load(target).Virtual(0x0A000001).Virtual(0x0A000037).Store(5);b.Address(5).Address(5).Field(0x0A00008B).Float(height).Op(0x58,0x6B).Set(0x0A00008B);
        b.Load(4).Virtual(0x0A00002E).Load(5).Virtual(0x0A000038);
    }
    private static byte[] N118_Skill149()
    {
        var b=new IL();b.Integer(0).Store(6);b.Arg(0).Integer(4).Virtual(0x06000361).Store(0);b.Load(0).Virtual(N118_TargetZombieCount).Integer(0).Branch(0x3E,"afterZombies");
        b.Load(0).Virtual(N118_TargetZombieEnum).Store(1);b.Label("tryZombies").Branch(0x38,"moveZombie");b.Label("zombie").Address(1).Call(N118_TargetZombieCurrent).Store(2);N118_Skill149Damage(b,2,0x0600047F,70);
        b.Label("moveZombie").Address(1).Call(N118_TargetZombieMove).Branch(0x3A,"zombie").Branch(0xDD,"zombiesDone");b.Label("finallyZombies").Address(1).Call(N118_TargetZombieDispose).Op(0xDC);
        b.Label("zombiesDone").Integer(1).Store(6);b.Label("afterZombies").Load(0).Virtual(N118_TargetZombieCount).Integer(4).Branch(0x3C,"checkAudio");
        b.Arg(0).Integer(4).Load(0).Virtual(N118_TargetZombieCount).Op(0x59).Virtual(0x06000364).Store(7);b.Load(7).Virtual(N118_TargetDeviceCount).Integer(0).Branch(0x3E,"checkAudio");
        b.Load(7).Virtual(N118_TargetDeviceEnum).Store(8);b.Label("tryDevices").Branch(0x38,"moveDevice");b.Label("device").Address(8).Call(N118_TargetDeviceCurrent).Store(9);N118_Skill149Damage(b,9,0x06000336,50);
        b.Label("moveDevice").Address(8).Call(N118_TargetDeviceMove).Branch(0x3A,"device").Branch(0xDD,"audio");b.Label("finallyDevices").Address(8).Call(N118_TargetDeviceDispose).Op(0xDC);
        b.Label("checkAudio").Load(6).Branch(0x39,"return");b.Label("audio").Static(0x040002F1).Integer(24).Virtual(0x0A000084).Store(10);
        b.Call(0x0A000004).Virtual(0x0A000001).Virtual(0x0A000037).Store(5);b.Call(0x06000138).Store(11);b.Load(10).Load(5).Load(11).Load(11).Op(0x58,0x6B).Call(0x0600013D).Op(0x26);
        b.Label("return").Op(0x2A);N118_Finally(b,0x06000518,("tryZombies","finallyZombies","zombiesDone"),("tryDevices","finallyDevices","checkAudio"));return b.Finish();
    }
    private static void N118_DeviceDetailActive(IL b,int field,bool active)=>b.Arg(0).Field(field).Virtual(0x0A000009).Integer(active?1:0).Virtual(0x0A000007);
    private static byte[] N118_DeviceDetailLoad()
    {
        var b=new IL();b.Arg(0).Field(0x040006DD).Op(0x14).Call(0x0A00000C).Branch(0x39,"assign");
        b.Arg(0).Field(0x040006DD).Virtual(0x0A000009).Call(0x0A000025);
        b.Label("assign").Arg(0).Arg(1).Set(0x040006D4);b.Arg(1).Branch(0x39,"empty");
        b.Arg(0).Arg(1).Field(0x0400016B).Set(0x040006D3);
        b.Static(0x0400015E).Field(0x04000166).Field(0x04000310).Field(0x0400063F).Arg(1).Field(0x0400016B).Op(0x91).Branch(0x39,"locked");
        b.Arg(0).Field(0x040006D5).Arg(1).Field(0x0400016A).Integer(1).Virtual(0x0A00004B);
        foreach(var x in new[]{(Text:0x040006D6,Value:0x0400016C),(Text:0x040006D7,Value:0x0400016D),(Text:0x040006D8,Value:0x0400016E),(Text:0x040006D9,Value:0x04000172),(Text:0x040006DA,Value:0x04000173)})
        {
            b.Arg(0).Field(x.Text).Store(0);b.Arg(1).FieldAddress(x.Value).Call(0x0A000043);
            if(x.Text==0x040006DA)b.Ref(0x72,0x700176D1).Call(0x0A000051);
            b.Store(1);b.Load(0).Load(1).Integer(1).Virtual(0x0A00004B);
        }
        b.Arg(0).Field(0x040006DB).Arg(1).Field(0x04000170).Integer(1).Virtual(0x0A00004B);
        N118_DeviceDetailActive(b,0x040006DE,false);N118_DeviceDetailActive(b,0x040006DF,false);
        b.Arg(0).Field(0x040006E0).Arg(0).Field(0x040006D3).Virtual(N118_DeviceDetailPrefabItem).Op(0x14).Call(0x0A00000C).Branch(0x39,"return");
        b.Arg(0).Arg(0).Field(0x040006E0).Arg(0).Field(0x040006D3).Virtual(N118_DeviceDetailPrefabItem).Call(0x2B000027).Set(0x040006DD);
        b.Arg(0).Field(0x040006DD).Virtual(0x0A000001).Arg(0).Field(0x040006DC).Virtual(0x0A00002E).Integer(0).Virtual(0x0A00002F).Branch(0x38,"return");
        b.Label("locked").Arg(0).Field(0x040006D5).Ref(0x72,0x700176D5).Integer(1).Virtual(0x0A00004B);
        for(int f=0x040006D6;f<=0x040006DB;f++)b.Arg(0).Field(f).Ref(0x72,0x700176DD).Integer(1).Virtual(0x0A00004B);
        b.Arg(0).Field(0x040006DE).Store(2);b.Call(0x06000227).Arg(0).Field(0x040006D3).Virtual(0x0A000040).Store(3);b.Load(2).Load(3).Virtual(0x0A000041);
        N118_DeviceDetailActive(b,0x040006DE,true);N118_DeviceDetailActive(b,0x040006DF,true);b.Branch(0x38,"return");
        b.Label("empty").Arg(0).Field(0x040006D5).Ref(0x72,0x700176E1).Integer(1).Virtual(0x0A00004B);
        for(int f=0x040006D6;f<=0x040006DB;f++)b.Arg(0).Field(f).Static(0x0A000013).Integer(1).Virtual(0x0A00004B);
        N118_DeviceDetailActive(b,0x040006DE,false);N118_DeviceDetailActive(b,0x040006DF,false);b.Label("return").Op(0x2A);return b.Finish();
    }
    private static void N118_ConfigRows(IL b)=>b.Arg(0).Field(0x040000A6).Virtual(0x06000158).Field(0x0400037D);
    private static void N118_ConfigInput(IL b,int local)=>b.Arg(0).Field(0x040000B9).Load(local).Virtual(N118_ConfigInputItem);
    private static byte[] N118_CreateEnemyConfig()
    {
        var b=new IL();b.Call(0x0600022F).Store(0);b.Load(0).Call(0x0A00000B).Branch(0x3A,"clear");b.Ref(0x72,N118_MissingSelectPrefab).Call(0x0A000010).Op(0x2A);
        b.Label("clear").Arg(0).Field(0x040000B7).Virtual(N118_ConfigSelectCount).Integer(1).Op(0x59).Store(1).Branch(0x38,"destroyTest");
        b.Label("destroy").Arg(0).Field(0x040000B7).Load(1).Virtual(N118_ConfigSelectItem).Virtual(0x0A000009).Call(0x0A000025);b.Load(1).Integer(1).Op(0x59).Store(1);
        b.Label("destroyTest").Load(1).Integer(0).Branch(0x3C,"destroy");b.Arg(0).Field(0x040000B7).Virtual(N118_ConfigSelectClear);
        b.Call(0x06000234).Store(5);b.Integer(0).Store(1).Branch(0x38,"selectTest");
        b.Label("select").Load(0).Call(0x2B000009).Store(4);b.Integer(0).Store(3);b.Integer(0).Store(2).Branch(0x38,"typeTest");
        b.Label("type").Arg(0).Field(0x040000A6).Field(0x04000196).Load(2).Virtual(0x0A00006B).Load(1).Branch(0x40,"nextType");b.Integer(1).Store(3);
        b.Label("nextType").Load(2).Integer(1).Op(0x58).Store(2);b.Label("typeTest").Load(2).Arg(0).Field(0x040000A6).Field(0x04000196).Virtual(N118_ConfigTypeCount).Branch(0x3F,"type");
        b.Load(4).Arg(0).Set(0x04000912);b.Load(4).Load(5).Load(1).Virtual(N118_ConfigZombieItem).Load(3).Virtual(0x06000711);
        b.Arg(0).Field(0x040000B7).Load(4).Virtual(N118_ConfigSelectAdd);b.Load(4).Virtual(0x0A000001).Arg(0).Field(0x040000B8).Virtual(0x0A00002E).Virtual(N110_SetParent);
        b.Load(1).Integer(1).Op(0x58).Store(1);b.Label("selectTest").Load(1).Load(5).Virtual(N118_ConfigZombieCount).Branch(0x3F,"select");
        b.Label("trimTest").Arg(0).Field(0x040000B9).Virtual(N118_ConfigInputCount);N118_ConfigRows(b);b.Virtual(N118_ConfigRowCount).Branch(0x3E,"createRows");
        b.Arg(0).Field(0x040000B9).Virtual(N118_ConfigInputCount).Integer(1).Op(0x59).Store(1);b.Load(1).Branch(0x39,"hideTemplate");
        N118_ConfigInput(b,1);b.Virtual(0x0A000009).Call(0x0A000025);b.Arg(0).Field(0x040000B9).Load(1).Virtual(N118_ConfigInputRemoveAt).Branch(0x38,"trimTest");
        b.Label("hideTemplate");N118_ConfigInput(b,1);b.Virtual(0x0A000009).Integer(0).Virtual(0x0A000007);
        b.Label("createRows").Integer(0).Store(1).Branch(0x38,"rowTest");b.Label("row").Load(1).Arg(0).Field(0x040000B9).Virtual(N118_ConfigInputCount).Branch(0x3F,"populate");
        b.Arg(0).Field(0x040000B9).Store(6);b.Load(6).Load(6).Integer(0).Virtual(N118_ConfigInputItem).Call(0x2B00000A).Virtual(N118_ConfigInputAdd);
        N118_ConfigInput(b,1);b.Virtual(0x0A000001).Arg(0).Field(0x040000B9).Integer(0).Virtual(N118_ConfigInputItem).Virtual(0x0A000001).Virtual(0x0A000074).Virtual(N110_SetParent);
        b.Label("populate");N118_ConfigInput(b,1);b.Virtual(0x0A000009).Integer(1).Virtual(0x0A000007);
        N118_ConfigInput(b,1);b.Store(7);N118_ConfigRows(b);b.Load(1).Virtual(N118_ConfigRowItem).FieldAddress(0x04000382).Call(0x0A00002A).Store(8);b.Load(7).Load(8).Virtual(0x0A000063);
        N118_ConfigInput(b,1);b.Store(7);b.Address(1).Call(0x0A00002A).Store(8);b.Load(7).Load(8).Virtual(0x0A000075);
        b.Load(1).Integer(1).Op(0x58).Store(1);b.Label("rowTest").Load(1);N118_ConfigRows(b);b.Virtual(N118_ConfigRowCount).Branch(0x3F,"row").Op(0x2A);return b.Finish();
    }
    private static void N118_FirstPrePath(IL b)=>b.Arg(0).Field(0x040005F9).Integer(0).Virtual(N118_PathItem);
    private static void N118_RemoveFirstPath(IL b)
    {
        b.Arg(0).Field(0x040005F9).Store(0);b.Load(0).Load(0).Integer(0).Virtual(N118_PathItem).Virtual(N118_PathRemove).Op(0x26);
        b.Arg(0).Field(0x040005FA).Virtual(N118_PathClear);
    }
    private static byte[] N118_PathTest()
    {
        var b=new IL();b.Arg(0).Field(0x040005F9).Branch(0x39,"false");b.Arg(0).Field(0x040005F9).Virtual(N118_PathCount).Branch(0x39,"false");
        b.Arg(0).Field(0x040005F9).Virtual(N118_PathCount).Integer(0).Branch(0x3E,"arrival");b.Arg(0).Field(0x040005FA).Branch(0x39,"true");
        b.Arg(0).Field(0x040005FA).Virtual(N118_PathCount).Branch(0x39,"true");
        b.Label("arrival");N118_FirstPrePath(b);b.Arg(0).Virtual(0x060002A3).Branch(0x3A,"arrived");
        b.Arg(0).Field(0x040005FA).Virtual(N118_PathEnum).Store(1);b.Label("tryPaths").Branch(0x38,"movePath");
        b.Label("path").Address(1).Call(N118_PathCurrent).Store(2);b.Load(2).Field(0x040003A9).Branch(0x3A,"movePath");
        b.Load(2).Arg(0).Virtual(0x060002A3).Branch(0x39,"movePath");b.Load(2).Integer(1).Set(0x040003A9);
        b.Label("movePath").Address(1).Call(N118_PathMove).Branch(0x3A,"path").Branch(0xDD,"pathsDone");b.Label("finallyPaths").Address(1).Call(N118_PathDispose).Op(0xDC);
        b.Label("pathsDone").Branch(0x38,"false");
        b.Label("arrived").Arg(0).Field(0x040005AA).Store(3);N118_FirstPrePath(b);b.Field(0x040003A5).Store(4);N118_FirstPrePath(b);b.Field(0x040003A6).Store(5);
        b.Load(3).Load(4).Load(5).Virtual(0x060002C4).Store(6);b.Load(6).Branch(0x39,"ordinary");
        b.Load(6).Integer(0).Virtual(0x06000299).Store(7);b.Load(7).Op(0x14).Call(0x0A00000C).Branch(0x39,"ordinary");
        b.Load(7).Field(0x0400046F).Integer(8).Branch(0x40,"ordinary");b.Arg(0).Float(9999).Virtual(0x06000484);N118_RemoveFirstPath(b);
        b.Load(7).Arg(0).Virtual(0x06000323).Branch(0x38,"false");
        b.Label("ordinary");N118_FirstPrePath(b);b.Field(0x040003A5).Store(4);N118_FirstPrePath(b);b.Field(0x040003A6).Store(5);
        b.Arg(0).Field(0x040005AA).Op(0x14).Call(0x0A000012).Branch(0x3A,"afterFail");
        b.Arg(0).Field(0x040005AA).Field(0x040003B6).Branch(0x3A,"afterFail");b.Arg(0).Field(0x040005AA).Field(0x040003B7).Branch(0x3A,"afterFail");b.Arg(0).Field(0x040005D1).Branch(0x3A,"afterFail");
        b.Arg(0).Field(0x040005AA).Load(4).Load(5).Virtual(0x060002C4).Store(8);b.Load(8).Branch(0x39,"afterFail");b.Load(8).Field(0x04000384).Branch(0x39,"afterFail");b.Arg(0).Field(0x040005AA).Virtual(0x060002BC);
        b.Label("afterFail");N118_FirstPrePath(b);b.Field(0x040003A8).Float(0).Branch(0x3D,"wait");N118_FirstPrePath(b);b.Field(0x040003AA).Branch(0x3A,"wait");
        N118_RemoveFirstPath(b);b.Branch(0x38,"true");
        b.Label("wait").Arg(0).Field(0x040005B9).Integer(17).Branch(0x3B,"endGrid");N118_FirstPrePath(b);b.Field(0x040003AA).Branch(0x39,"stand");
        b.Label("endGrid").Load(6).Branch(0x3A,"stand");b.Arg(0).Virtual(0x06000436).Branch(0x38,"false");
        b.Label("stand").Arg(0);N118_FirstPrePath(b);b.Field(0x040003A8).Virtual(0x06000484);N118_RemoveFirstPath(b);
        b.Label("false").Integer(0).Op(0x2A);b.Label("true").Integer(1).Op(0x2A);
        N118_Finally(b,0x0600046B,("tryPaths","finallyPaths","pathsDone"));return b.Finish();
    }
    private static void N118_BoxingPlantCoordinates(IL b)
    {
        b.Arg(0).Arg(0).Field(0x04000601).Field(0x040004A0).Set(0x04000605);b.Arg(0).Arg(0).Field(0x04000601).Field(0x040004A1).Set(0x04000606);
    }
    private static void N118_BoxingDeviceCoordinates(IL b,bool captured)
    {
        if(captured){b.Arg(0).Load(6).Field(0x04000464).Set(0x04000605);b.Arg(0).Load(6).Field(0x04000465).Set(0x04000606);}
        else{b.Arg(0).Arg(0).Field(0x04000603).Field(0x04000464).Set(0x04000605);b.Arg(0).Arg(0).Field(0x04000603).Field(0x04000465).Set(0x04000606);}
    }
    private static byte[] N118_Boxing()
    {
        var b=new IL();b.Arg(0).Field(0x040005AA).Op(0x14).Call(0x0A000012).Branch(0x3A,"return");
        b.Arg(0).Field(0x040005AA).Store(12);b.Load(12).Field(0x040003D4).Store(0);b.Load(12).Field(0x040003D8).Store(1);
        b.Arg(0).Integer(-1).Op(0x14).Integer(1).Virtual(0x06000443).Store(2);b.Load(0).Field(0x040002B1).Virtual(N118_TargetPlantEnum).Store(3);
        b.Label("tryPlants").Branch(0x38,"movePlant");b.Label("plant").Address(3).Call(N118_TargetPlantCurrent).Store(4);
        b.Load(4).Op(0x14).Call(0x0A000012).Branch(0x3A,"movePlant");b.Load(4).Field(0x040004C8).Arg(0).Field(0x040005F1).Branch(0x3B,"movePlant");
        b.Load(4).Virtual(0x06000359).Branch(0x39,"movePlant");b.Arg(0).Field(0x04000601).Op(0x14).Call(0x0A000012).Branch(0x3A,"choosePlant");
        b.Arg(0).Field(0x04000601).Field(0x040004A0).Load(4).Field(0x040004A0).Branch(0x3D,"movePlant");b.Label("choosePlant").Arg(0).Load(4).Set(0x04000601);
        b.Label("movePlant").Address(3).Call(N118_TargetPlantMove).Branch(0x3A,"plant").Branch(0xDD,"plantsDone");b.Label("finallyPlants").Address(3).Call(N118_TargetPlantDispose).Op(0xDC);
        b.Label("plantsDone").Load(1).Field(0x040001D6).Virtual(N118_TargetDeviceEnum).Store(5);b.Label("tryDevices").Branch(0x38,"moveDevice");
        b.Label("device").Address(5).Call(N118_TargetDeviceCurrent).Store(6);b.Load(6).Op(0x14).Call(0x0A000012).Branch(0x3A,"moveDevice");
        b.Load(6).Field(0x0400046E).Arg(0).Field(0x040005F1).Branch(0x3B,"moveDevice");b.Load(6).Load(2).Virtual(0x06000310).Branch(0x39,"moveDevice");
        // Original compares the existing PLANT target with this device; do not substitute targetDevice.
        b.Arg(0).Field(0x04000601).Op(0x14).Call(0x0A000012).Branch(0x3A,"chooseDevice");b.Arg(0).Field(0x04000601).Field(0x040004A0).Load(6).Field(0x04000464).Branch(0x3D,"moveDevice");
        b.Label("chooseDevice").Arg(0).Load(6).Set(0x04000603);b.Label("moveDevice").Address(5).Call(N118_TargetDeviceMove).Branch(0x3A,"device").Branch(0xDD,"devicesDone");
        b.Label("finallyDevices").Address(5).Call(N118_TargetDeviceDispose).Op(0xDC);b.Label("devicesDone").Arg(0).Field(0x04000601).Op(0x14).Call(0x0A000012).Branch(0x39,"selectCoordinates");
        b.Arg(0).Field(0x04000603).Op(0x14).Call(0x0A000012).Branch(0x3A,"return");
        b.Label("selectCoordinates").Arg(0).Field(0x04000601).Op(0x14).Call(0x0A00000C).Branch(0x39,"onlyDevice");
        b.Arg(0).Field(0x04000603).Op(0x14).Call(0x0A00000C).Branch(0x39,"onlyDevice");
        b.Arg(0).Field(0x04000603).Store(6);b.Load(6).Field(0x04000464).Arg(0).Field(0x04000601).Field(0x040004A0).Branch(0x3D,"deviceCoordinates");
        N118_BoxingPlantCoordinates(b);b.Branch(0x38,"onlyDevice");b.Label("deviceCoordinates");N118_BoxingDeviceCoordinates(b,true);
        b.Label("onlyDevice").Arg(0).Field(0x04000601).Op(0x14).Call(0x0A000012).Branch(0x39,"onlyPlant");
        b.Arg(0).Field(0x04000603).Op(0x14).Call(0x0A00000C).Branch(0x39,"onlyPlant");N118_BoxingDeviceCoordinates(b,false);
        b.Label("onlyPlant").Arg(0).Field(0x04000601).Op(0x14).Call(0x0A00000C).Branch(0x39,"particle");b.Arg(0).Field(0x04000603).Op(0x14).Call(0x0A000012).Branch(0x39,"particle");N118_BoxingPlantCoordinates(b);
        b.Label("particle").Arg(0).Field(0x04000605).Store(9);b.Arg(0).Field(0x04000606).Store(10);b.Arg(0).Field(0x040005AA).Field(0x040003B4).Load(9).Load(10).Virtual(0x0600015F).Store(7);
        b.Call(0x060001E5).Integer(13).Virtual(0x060001EA).Store(8);b.Load(8).Call(0x0A00000B).Branch(0x39,"clearTargets");b.Load(8).Virtual(0x0A00002E).Load(7).Virtual(0x0A000038);b.Load(8).Virtual(0x2B000067).Arg(0).Set(0x040000D6);
        b.Label("clearTargets").Arg(0).Op(0x14).Set(0x04000603);b.Arg(0).Op(0x14).Set(0x04000601);b.Label("return").Op(0x2A);
        N118_Finally(b,0x06000490,("tryPlants","finallyPlants","plantsDone"),("tryDevices","finallyDevices","devicesDone"));return b.Finish();
    }
}
