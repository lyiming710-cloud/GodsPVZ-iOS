// Native119: source reconstruction only. Ordering follows original PC instructions.
internal static partial class Program
{
    private static void N119_Text(IL b,int text,int literal)=>b.Arg(0).Field(text).Ref(0x72,literal).Integer(1).Virtual(0x0A00004B);
    private static void N119_Active(IL b,int field,bool value)=>b.Arg(0).Field(field).Virtual(0x0A000009).Integer(value?1:0).Virtual(0x0A000007);
    private static byte[] N119_LoadPlantData()
    {
        var b=new IL();b.Arg(0).Arg(0).Field(0x040006F5).Field(0x0400065E).Set(0x040006F2);
        b.Arg(0).Field(0x040006F6).Arg(0).Field(0x040006F5).Field(0x04000656).Integer(1).Virtual(0x0A00004B);
        b.Arg(0).Field(0x040006F7).Arg(0).Field(0x040006F5).Ref(0x7C,0x04000662).Call(0x0A00002A).Integer(1).Virtual(0x0A00004B);
        int[] values=[0x04000667,0x04000668,0x04000669,0x0400066A,0x0400066B,0x04000664,0x04000665];
        for(int i=0;i<7;i++){b.Arg(0).Field(0x040006F8).Integer(i).Virtual(0x0A000264).Arg(0).Field(0x040006F5).Field(values[i]);if(i==5)b.Op(0x6B);b.Virtual(0x0600065C);}
        b.Arg(0).Arg(0).Field(0x040006F5).Virtual(0x060004CA).Set(0x04000706);
        b.Arg(0).Field(0x0400070B).Ref(0x7E,0x040002DA).Arg(0).Field(0x040006F5).Field(0x0400065F).Virtual(0x0A000040).Virtual(0x0A000041);
        b.Integer(0).Store(0).Branch(0x38,"starTest");b.Label("star");
        // Capture list before reading stars; every iteration rereads mutable fields.
        b.Arg(0).Field(0x0400070A).Store(1);b.Arg(0).Field(0x040006F5).Field(0x0400065D).Load(0).Op(0xFE,0x02).Store(2);
        b.Load(1).Load(0).Virtual(0x0A000157).Virtual(0x0A000009).Load(2).Virtual(0x0A000007);b.Load(0).Integer(1).Op(0x58).Store(0);
        b.Label("starTest").Load(0).Arg(0).Field(0x0400070A).Virtual(N119_PlantStarsCount).Branch(0x3F,"star");
        N119_Active(b,0x04000710,false);N119_Active(b,0x04000713,true);
        b.Integer(0).Store(0).Branch(0x38,"orderTest");b.Label("order");
        b.Arg(0).Field(0x04000709).Load(0).Virtual(0x0A000157).Virtual(0x2B000081).Store(3);
        b.Arg(0).Field(0x040006F5).Field(0x04000661).Load(0).Branch(0x3D,"obtained");
        b.Arg(0).Field(0x04000709).Load(0).Virtual(0x0A000157).Virtual(0x0A000009).Store(4);
        b.Arg(0).Field(0x040006F5).Virtual(0x060004D3).Store(5);b.Load(4).Load(0).Load(5).Op(0xFE,0x04).Virtual(0x0A000007);
        b.Arg(0).Field(0x040006F5).Field(0x04000661).Load(0).Branch(0x40,"disabled");b.Arg(0).Field(0x040006F5).Virtual(0x060004D2).Branch(0x39,"disabled");
        b.Load(3).Integer(1).Virtual(0x0A000208);b.Load(3).Integer(1).Virtual(0x0A000018);N119_Active(b,0x04000710,true);b.Arg(0).Field(0x04000713).Integer(0).Virtual(0x0A000018).Branch(0x38,"orderNext");
        b.Label("disabled").Load(3).Integer(1).Virtual(0x0A000208);b.Load(3).Integer(0).Virtual(0x0A000018).Branch(0x38,"orderNext");
        b.Label("obtained");b.Arg(0).Field(0x04000709).Load(0).Virtual(0x0A000157).Virtual(0x0A000009).Integer(1).Virtual(0x0A000007);
        b.Arg(0).Field(0x04000709).Load(0).Virtual(0x0A000157).Float(1).Float(1).Float(1).Float(1).New(N119_ColorCtor).Virtual(0x0A0001DE);b.Load(3).Integer(0).Virtual(0x0A000208);
        b.Label("orderNext").Load(0).Integer(1).Op(0x58).Store(0);b.Label("orderTest").Load(0).Arg(0).Field(0x04000709).Virtual(N119_PlantStarsCount).Branch(0x3F,"order");
        // Snapshot current order before GetMaxOrder, which is allowed to have side effects.
        b.Arg(0).Field(0x040006F5).Field(0x04000661).Store(5);b.Arg(0).Field(0x040006F5).Virtual(0x060004D3).Store(6);
        b.Load(5).Load(6).Branch(0x3C,"maxOrder");b.Arg(0).Field(0x04000710).Integer(1).Virtual(0x0A000018);N119_Text(b,0x04000711,N119_Deify);N119_Text(b,0x04000714,N119_DeifyPreview);b.Branch(0x38,"systems");
        b.Label("maxOrder").Arg(0).Field(0x04000710).Integer(0).Virtual(0x0A000018);N119_Text(b,0x04000711,N119_Max);b.Arg(0).Field(0x04000713).Integer(0).Virtual(0x0A000018);N119_Text(b,0x04000714,N119_DeifyMax);
        b.Label("systems").Arg(0).Field(0x040006F9).Arg(0).Field(0x040006F5).Virtual(0x0600058A);
        b.Arg(0).Field(0x040006FA).Arg(0).Field(0x040006F5).Virtual(0x06000597);
        b.Arg(0).Field(0x040006FB).Arg(0).Field(0x040006F5).Virtual(0x06000556);
        b.Arg(0).Field(0x040006FC).Arg(0).Field(0x040006F5).Virtual(0x0600056D).Op(0x2A);return b.Finish();
    }
    private static void N119_CopyPreviewIdentity(IL b,bool copyOrder)
    {
        int[] fs=copyOrder?[0x04000656,0x0400065C,0x0400065E,0x04000661]:[0x04000656,0x0400065C,0x0400065E];
        foreach(int f in fs)b.Arg(0).Field(0x04000706).Arg(0).Field(0x040006F5).Field(f).Set(f);
    }
    private static void N119_SkillState(IL b,bool enabled)=>b.Arg(0).Field(0x0400070D).Arg(0).Field(0x040006F5).Field(0x04000663).Arg(0).Field(0x040006F5).Field(0x04000661).Arg(0).Field(0x040006F5).Field(0x0400065D).Integer(enabled?1:0).Virtual(0x060005F2);
    private static void N119_PreviewData(IL b,int index,int field)=>b.Arg(0).Field(0x040006F8).Integer(index).Virtual(0x0A000264).Arg(0).Field(0x04000706).Field(field).Virtual(0x0600065D);
    private static void N119_ClosePreviews(IL b)
    {
        b.Arg(0).Field(0x040006F8).Virtual(N119_PreviewEnum).Store(0);b.Label("tryClose").Branch(0x38,"closeMove");
        b.Label("close").Address(0).Call(N119_PreviewCurrent).Virtual(0x0600065E);b.Label("closeMove").Address(0).Call(N119_PreviewMove).Branch(0x3A,"close").Branch(0xDD,"closed");
        b.Label("finallyClose").Address(0).Call(N119_PreviewDispose).Op(0xDC);b.Label("closed");
    }
    private static void N119_ClearSupplies(IL b)
    {
        b.Integer(0).Store(1).Branch(0x38,"supplyTest");b.Label("supply").Arg(0).Field(0x04000717).Load(1).Virtual(0x0A000260).Virtual(0x0A000009).Call(0x0A000025);b.Load(1).Integer(1).Op(0x58).Store(1);
        b.Label("supplyTest").Load(1).Arg(0).Field(0x04000717).Virtual(N119_PreviewSupplyCount).Branch(0x3F,"supply");b.Arg(0).Field(0x04000717).Virtual(N119_PreviewSupplyClear);
    }
    private static void N119_ClonePreviewData(IL b,string missing)
    {
        b.Ref(0x7E,0x0400015E).Field(0x04000166).Field(0x04000312).Branch(0x39,missing);
        b.Ref(0x7E,0x0400015E).Field(0x04000166).Field(0x04000312).Field(0x04000315).Arg(0).Field(0x04000706).Field(0x0400065E).Virtual(0x0A000059).Arg(0).Field(0x04000706).Virtual(0x06000256);
    }
    private static byte[] N119_LevelPreview()
    {
        var b=new IL();b.Arg(0).Field(0x040006F5).Field(0x04000662).Store(2);b.Arg(0).Field(0x040006F5).Field(0x04000661).Integer(2).Op(0x58).Integer(10).Op(0x5A).Store(3);
        b.Load(2).Load(3).Branch(0x3E,"rangeOk");b.Load(2).Load(3).Call(0x2B000082);b.Label("rangeOk");
        b.Arg(1).Load(2).Branch(0x3F,"min");b.Arg(1).Load(3).Branch(0x3D,"clamped");b.Arg(1).Store(3).Branch(0x38,"clamped");b.Label("min").Load(2).Store(3);b.Label("clamped");
        N119_CopyPreviewIdentity(b,true);b.Arg(0).Load(3).Set(0x040006FE);b.Arg(0).Field(0x04000707).Address(3).Call(0x0A00002A).Integer(1).Virtual(0x0A00004B);
        b.Load(3).Arg(0).Field(0x040006F5).Field(0x04000662).Branch(0x3B,"sameLevel");
        b.Arg(0).Field(0x04000704).Integer(1).Virtual(0x0A000018);N119_SkillState(b,false);b.Arg(0).Field(0x04000706).Load(3).Set(0x04000662);
        N119_ClonePreviewData(b,"missing");N119_PreviewData(b,0,0x04000667);N119_PreviewData(b,1,0x04000668);N119_PreviewData(b,3,0x0400066A);
        b.Arg(0).Virtual(0x0600057A);b.Arg(0).Virtual(0x06000585).Store(4);b.Arg(0).Field(0x04000704).Load(4).Virtual(0x0A000018);b.Arg(0).Field(0x04000713).Integer(0).Virtual(0x0A000018).Branch(0x38,"animate");
        b.Label("missing").Ref(0x72,0x7000CC9E).Call(0x0A000010);b.Label("animate").Arg(0).Field(0x040006ED).Ref(0x7E,0x0400071F).Integer(1).Virtual(0x0A0001F7).Branch(0x38,"buttons");
        b.Label("sameLevel");N119_ClosePreviews(b);b.Arg(0).Field(0x04000704).Integer(0).Virtual(0x0A000018);N119_SkillState(b,true);N119_ClearSupplies(b);
        b.Arg(0).Field(0x040006ED).Ref(0x7E,0x0400071F).Integer(0).Virtual(0x0A0001F7);b.Arg(0).Field(0x04000713).Arg(0).Field(0x040006F5).Virtual(0x060004D2).Integer(0).Op(0xFE,0x01).Virtual(0x0A000018);
        b.Label("buttons");foreach(int f in new[]{0x04000701,0x04000703})b.Arg(0).Field(f).Virtual(0x0A000009).Load(3).Arg(0).Field(0x040006F5).Field(0x04000662).Op(0xFE,0x02).Virtual(0x0A000007);
        foreach(int f in new[]{0x04000700,0x04000702})b.Arg(0).Field(f).Virtual(0x0A000009).Load(3).Arg(0).Field(0x040006F5).Field(0x04000661).Integer(2).Op(0x58).Integer(10).Op(0x5A,0xFE,0x04).Virtual(0x0A000007);
        b.Op(0x2A);N119_Finally(b,0x06000582,("tryClose","finallyClose","closed"));return b.Finish();
    }
    private static byte[] N119_OrderPreview()
    {
        var b=new IL();b.Arg(0).Arg(1).Set(0x0400070C);N119_CopyPreviewIdentity(b,false);
        b.Arg(1).Arg(0).Field(0x040006F5).Field(0x04000661).Branch(0x3B,"sameOrder");b.Arg(0).Field(0x0400070F).Integer(1).Virtual(0x0A000018);N119_SkillState(b,false);
        b.Arg(0).Field(0x04000706).Arg(1).Set(0x04000661);b.Arg(0).Field(0x04000706).Integer(1).Set(0x04000662);N119_ClonePreviewData(b,"missing");
        N119_PreviewData(b,2,0x04000669);N119_PreviewData(b,4,0x0400066B);
        b.Arg(0).Field(0x040006F8).Integer(5).Virtual(0x0A000264).Arg(0).Field(0x04000706).Field(0x04000664).Op(0x6B).Virtual(0x0600065D);N119_PreviewData(b,6,0x04000665);
        b.Arg(0).Virtual(0x0600057B);b.Arg(0).Virtual(0x06000585).Store(4);N119_Active(b,0x0400070F,true);b.Arg(2).Branch(0x3A,"locked");
        b.Arg(0).Field(0x0400070F).Load(4).Virtual(0x0A000018);b.Load(4).Branch(0x39,"insufficient");N119_Text(b,0x04000708,N119_Confirm);b.Branch(0x38,"labelDone");b.Label("insufficient");N119_Text(b,0x04000708,N119_Insufficient);b.Label("labelDone");N119_Active(b,0x04000710,false);N119_Active(b,0x04000712,true);b.Branch(0x38,"levelOff");
        b.Label("locked").Arg(0).Field(0x0400070F).Integer(0).Virtual(0x0A000018);N119_Text(b,0x04000708,N119_LowLevel);N119_Active(b,0x04000710,false);N119_Active(b,0x04000712,false);b.Arg(0).Field(0x04000705).Integer(0).Virtual(0x0A000007);N119_Active(b,0x04000715,true);
        b.Label("levelOff");N119_Active(b,0x04000704,false);b.Branch(0x38,"previewOn");b.Label("missing").Ref(0x72,0x7000CC9E).Call(0x0A000010);
        b.Label("previewOn").Arg(0).Field(0x040006F9).Integer(1).Virtual(0x0600058E);b.Arg(0).Field(0x040006FA).Integer(1).Virtual(0x06000598).Op(0x2A);
        b.Label("sameOrder");N119_ClosePreviews(b);b.Arg(0).Field(0x040006F5).Virtual(0x060004D2).Store(4);b.Arg(0).Field(0x0400070F).Integer(0).Virtual(0x0A000018);N119_Active(b,0x0400070F,false);N119_SkillState(b,true);N119_Active(b,0x04000704,true);
        b.Arg(0).Field(0x04000713).Load(4).Integer(0).Op(0xFE,0x01).Virtual(0x0A000018);b.Arg(0).Field(0x04000710).Virtual(0x0A000009).Load(4).Virtual(0x0A000007);
        b.Arg(2).Branch(0x39,"selectorDone");b.Arg(0).Field(0x04000705).Integer(1).Virtual(0x0A000007);N119_Active(b,0x04000715,false);b.Label("selectorDone");N119_Active(b,0x04000712,false);N119_ClearSupplies(b);
        // The inlined PreView_OrderUp(false) is restored as the original managed call.
        b.Arg(0).Field(0x040006F9).Integer(0).Virtual(0x0600058E);b.Arg(0).Field(0x040006FA).Integer(0).Virtual(0x06000598).Op(0x2A);
        N119_Finally(b,0x06000583,("tryClose","finallyClose","closed"));return b.Finish();
    }
    private static void N119_ZombiePlaceholders(IL b,bool unknown)
    {
        int[] fields=[0x04000742,0x04000743,0x04000746,0x04000747,0x04000744,0x04000745,0x04000748];
        int[] literals=unknown?[N119_Unknown,N119_HpUnknown,N119_AttackUnknown,N119_DefenseUnknown,N119_Armor1Unknown,N119_Armor2Unknown,N119_SpeedUnknown]:[N119_Dash,N119_HpDash,N119_AttackDash,N119_DefenseDash,N119_Armor1Dash,N119_Armor2Dash,N119_SpeedDash];
        for(int i=0;i<fields.Length;i++)N119_Text(b,fields[i],literals[i]);
    }
    private static byte[] N119_ZombieAlmanac()
    {
        var b=new IL();b.Arg(0).Arg(1).Set(0x04000741);b.Arg(1).Branch(0x39,"noInfo");
        b.Arg(0).Arg(1).Field(0x0400017B).Set(0x04000740);
        b.Ref(0x7E,0x0400015E).Field(0x04000166).Field(0x04000310).Field(0x0400063E).Arg(0).Field(0x04000740).Op(0x91).Branch(0x39,"lockedInfo");
        b.Arg(0).Field(0x04000742).Arg(1).Field(0x0400017C).Integer(1).Virtual(0x0A00004B);
        int[] texts=[0x04000743,0x04000746,0x04000747];int[] values=[0x0400017F,0x04000182,0x04000183];int[] prefixes=[N119_HpPrefix,N119_AttackPrefix,N119_DefensePrefix];
        for(int i=0;i<3;i++)b.Arg(0).Field(texts[i]).Ref(0x72,prefixes[i]).Arg(1).Ref(0x7C,values[i]).Call(0x0A000043).Call(0x0A000051).Integer(1).Virtual(0x0A00004B);
        for(int i=0;i<2;i++)
        {
            int field=0x04000180+i;b.Arg(0).Arg(1).Field(field).Virtual(0x060005A0+i);
            b.Arg(0).Field(0x0400074B+i).Virtual(0x0A000009).Arg(1).Field(field).Integer(0).Op(0xFE,0x03).Virtual(0x0A000007);
        }
        // The painter is looked up before the destination TMP field is captured.
        b.Arg(1).Field(0x0400018D).Call(0x06000144).Store(0);b.Arg(0).Field(0x0400075C).Ref(0x72,N119_PainterPrefix).Load(0).Call(0x0A000051).Integer(1).Virtual(0x0A00004B);
        b.Arg(0).Field(0x0400075B).Arg(1).Field(0x0400018C).Integer(1).Virtual(0x0A00004B);N119_Active(b,0x0400074D,true);
        b.Integer(7).Ref(0x8D,0x0100000B).Store(1);int[] speed=[N119_Speed0,N119_Speed1,N119_Speed2,N119_Speed3,N119_Speed4,N119_Speed5,N119_Speed6];
        for(int i=0;i<7;i++)b.Load(1).Integer(i).Ref(0x72,speed[i]).Op(0xA2);
        b.Integer(0).Store(2);b.Label("speedTest").Arg(1).Field(0x04000185).Load(2).Integer(2).Op(0x5A).Integer(2).Op(0x58).Branch(0x3F,"speedChosen");b.Load(2).Integer(5).Branch(0x3B,"speedFastest");b.Load(2).Integer(1).Op(0x58).Store(2).Branch(0x38,"speedTest");
        b.Label("speedFastest").Integer(6).Store(2);b.Label("speedChosen").Arg(0).Field(0x04000748).Ref(0x72,N119_SpeedPrefix).Load(1).Load(2).Op(0x9A).Call(0x0A000051).Integer(1).Virtual(0x0A00004B);
        b.Arg(0).Field(0x04000752).Arg(1).Field(0x0400018B).Integer(1).Virtual(0x0A00004B).Branch(0x38,"rating");
        b.Label("lockedInfo");N119_ZombiePlaceholders(b,true);b.Arg(0).Field(0x04000752).Ref(0x7E,0x0A000013).Integer(1).Virtual(0x0A00004B);foreach(int f in new[]{0x0400074B,0x0400074C,0x0400074D})N119_Active(b,f,false);
        b.Label("rating");N119_Active(b,0x04000749,false);
        b.Arg(1).Field(0x0400017D).Branch(0x39,"boss");N119_Text(b,0x04000749,N119_Elite);N119_Active(b,0x04000749,true);
        b.Label("boss").Arg(1).Field(0x0400017E).Branch(0x39,"check");N119_Text(b,0x04000749,N119_Boss);N119_Active(b,0x04000749,true);b.Branch(0x38,"check");
        b.Label("noInfo").Arg(0).Integer(-1).Set(0x04000740);N119_ZombiePlaceholders(b,false);N119_Active(b,0x04000749,false);b.Arg(0).Field(0x04000752).Ref(0x7E,0x0A000013).Integer(1).Virtual(0x0A00004B);foreach(int f in new[]{0x0400074B,0x0400074C,0x0400074D})N119_Active(b,f,false);
        b.Label("check").Arg(0).Virtual(0x060005A4).Op(0x2A);return b.Finish();
    }
    private static void N119_ClassifyLoot(IL b,string label)
    {
        b.Load(4).Virtual(0x06000167).Store(5);b.Load(4).Field(0x040001AB).Branch(0x3A,label+"first");b.Load(4).Field(0x040001A8).Float(1).Branch(0x3C,label+"sure");b.Load(2).Load(5).Virtual(N119_LootSupplyAdd).Branch(0x38,label+"done");
        b.Label(label+"sure").Load(1).Load(5).Virtual(N119_LootSupplyAdd).Branch(0x38,label+"done");b.Label(label+"first").Load(0).Load(5).Virtual(N119_LootSupplyAdd);b.Label(label+"done");
    }
    private static byte[] N119_LootList()
    {
        var b=new IL();for(int i=0;i<3;i++)b.New(0x0A00011E).Store(i);
        b.Arg(0).Field(0x04000771).Field(0x0400019C).Virtual(N119_LootEnum).Store(3);b.Label("tryLoot").Branch(0x38,"lootMove");b.Label("loot").Address(3).Call(N119_LootCurrent).Store(4);N119_ClassifyLoot(b,"plain");
        b.Label("lootMove").Address(3).Call(N119_LootMove).Branch(0x3A,"loot").Branch(0xDD,"lootDone");b.Label("finallyLoot").Address(3).Call(N119_LootDispose).Op(0xDC);b.Label("lootDone");
        b.Arg(0).Field(0x04000771).Field(0x040001A0).Virtual(N119_LootEntryEnum).Store(6);b.Label("tryEntries").Branch(0x38,"entryMove");b.Label("entry").Address(6).Call(N119_LootEntryCurrent).Store(7);b.Load(7).Field(0x040007FC).Branch(0x3A,"entryLoot");b.Load(7).Field(0x040007FD).Branch(0x39,"entryMove");
        b.Label("entryLoot").Load(7).Arg(0).Field(0x04000771).Virtual(0x06000609).Store(4);N119_ClassifyLoot(b,"entry");b.Label("entryMove").Address(6).Call(N119_LootEntryMove).Branch(0x3A,"entry").Branch(0xDD,"entriesDone");b.Label("finallyEntries").Address(6).Call(N119_LootEntryDispose).Op(0xDC);b.Label("entriesDone");
        for(int group=0;group<3;group++)
        {
            string p="display"+group;b.Load(group).Virtual(N119_LootSupplyEnum).Store(8);b.Label(p+"Try").Branch(0x38,p+"Move");b.Label(p).Address(8).Call(N119_LootSupplyCurrent).Integer(group<2?1:0).Virtual(0x060004D9).Store(9);
            b.Load(9).Virtual(0x0A000009).Virtual(0x0A00002E).Float(1).Float(1).Float(1).New(0x0A00009C).Virtual(0x0A0000E5);
            b.Load(9).Virtual(0x0A000009).Virtual(0x0A00002E).Arg(0).Field(0x04000780).Integer(group).Virtual(0x0A000006).Virtual(0x0A00002E).Integer(0).Virtual(0x0A00002F);
            b.Label(p+"Move").Address(8).Call(N119_LootSupplyMove).Branch(0x3A,p).Branch(0xDD,p+"Done");b.Label(p+"Finally").Address(8).Call(N119_LootSupplyDispose).Op(0xDC);b.Label(p+"Done");
        }
        b.Op(0x2A);N119_Finally(b,0x060005C8,("tryLoot","finallyLoot","lootDone"),("tryEntries","finallyEntries","entriesDone"),("display0Try","display0Finally","display0Done"),("display1Try","display1Finally","display1Done"),("display2Try","display2Finally","display2Done"));return b.Finish();
    }
    private static byte[] N119_ChooseDetermine()
    {
        var b=new IL();b.Arg(0).Field(0x040008BC).Field(0x04000887).Store(0);b.Load(0).Virtual(N119_ChooseEnum).Store(1);b.Label("tryDestroy").Branch(0x38,"destroyMove");b.Label("destroy").Address(1).Call(N119_ChooseCurrent).Virtual(0x0A000009).Call(0x0A000025);
        b.Label("destroyMove").Address(1).Call(N119_ChooseMove).Branch(0x3A,"destroy").Branch(0xDD,"destroyed");b.Label("finallyDestroy").Address(1).Call(N119_ChooseDispose).Op(0xDC);b.Label("destroyed").Load(0).Virtual(N119_ChooseClear);
        b.Arg(0).Field(0x040008B8).Virtual(N119_ChooseEnum).Store(1);b.Label("tryClone").Branch(0x38,"cloneMove");b.Label("clone").Address(1).Call(N119_ChooseCurrent).Store(2);b.Load(2).Field(0x04000448).Branch(0x39,"cloneMove");
        b.Load(2).Call(0x2B00007D).Store(2);b.Load(0).Load(2).Virtual(0x0A0002A5);b.Load(2).Virtual(0x0A000001).Float(1).Float(1).Float(1).New(0x0A00009C).Virtual(0x0A0000E5);N119_ChooseParent(b);
        b.Label("cloneMove").Address(1).Call(N119_ChooseMove).Branch(0x3A,"clone").Branch(0xDD,"cloned");b.Label("finallyClone").Address(1).Call(N119_ChooseDispose).Op(0xDC);b.Label("cloned");
        b.Integer(0).Store(3).Branch(0x38,"sortOuterTest");b.Label("sortOuter").Load(0).Load(3).Virtual(0x0A00029B).Store(2);b.Load(3).Integer(1).Op(0x58).Store(4).Branch(0x38,"sortInnerTest");
        b.Label("sortInner").Load(2).Field(0x04000449).Load(0).Load(4).Virtual(0x0A00029B).Field(0x04000449).Branch(0x3E,"sortNext");b.Load(0).Load(4).Virtual(0x0A00029B).Store(2);b.Load(0).Load(4).Load(0).Load(3).Virtual(0x0A00029B).Virtual(N119_ChooseSetItem);b.Load(0).Load(3).Load(2).Virtual(N119_ChooseSetItem);
        b.Label("sortNext").Load(4).Integer(1).Op(0x58).Store(4);b.Label("sortInnerTest").Load(4).Load(0).Virtual(N119_ChooseCount).Branch(0x3F,"sortInner");b.Load(3).Integer(1).Op(0x58).Store(3);b.Label("sortOuterTest").Load(3).Load(0).Virtual(N119_ChooseCount).Branch(0x3F,"sortOuter").Branch(0x38,"fillTest");
        b.Label("fill").Arg(0).Field(0x040008BA).Call(0x2B000027).Virtual(0x2B00008F).Store(2);b.Load(0).Load(2).Virtual(0x0A0002A5);b.Load(2).Load(0).Virtual(N119_ChooseCount).Set(0x04000449);N119_ChooseParent(b);b.Load(2).Virtual(0x060002F8);
        b.Load(0).Virtual(N119_ChooseCount).Store(3);b.Load(3).Static(0x0400015E).Field(0x04000166).Field(0x04000310).Field(0x04000629).Branch(0x3E,"fillTest");b.Load(2).Virtual(0x060002F6);
        b.Label("fillTest").Load(0).Virtual(N119_ChooseCount).Integer(14).Branch(0x3F,"fill");b.Arg(0).Field(0x040008BC).Virtual(0x0600068B);
        b.Load(0).Virtual(N119_ChooseEnum).Store(1);b.Label("tryLayout").Branch(0x38,"layoutMove");b.Label("layout").Address(1).Call(N119_ChooseCurrent).Store(2);
        b.Load(2).Field(0x04000449).Integer(1).Op(0x59).Store(3);b.Load(3).Integer(2).Op(0x5B).Integer(208).Op(0x5A,0x6B).Float(820).Op(0x59,0x6B).Store(5);b.Float(225).Load(3).Integer(2).Op(0x5D).Integer(340).Op(0x5A,0x6B,0x59,0x6B).Store(6);
        b.Load(2).Virtual(0x0A000001).Load(5).Load(6).Float(0).New(0x0A00009C).Virtual(0x0A000038);b.Load(2).Integer(0).Set(0x04000447);b.Load(2).Field(0x04000429).Integer(-1).Branch(0x3B,"layoutMove");b.Load(2).Virtual(0x060002F5);
        b.Label("layoutMove").Address(1).Call(N119_ChooseMove).Branch(0x3A,"layout").Branch(0xDD,"laidOut");b.Label("finallyLayout").Address(1).Call(N119_ChooseDispose).Op(0xDC);b.Label("laidOut").Arg(0).Field(0x040008BC).Virtual(0x0A000009).Integer(1).Virtual(0x0A000007);b.Arg(0).Virtual(0x0A000009).Integer(0).Virtual(0x0A000007).Op(0x2A);
        N119_Finally(b,0x060006B4,("tryDestroy","finallyDestroy","destroyed"),("tryClone","finallyClone","cloned"),("tryLayout","finallyLayout","laidOut"));return b.Finish();
    }
    private static void N119_ChooseParent(IL b)=>b.Load(2).Virtual(0x0A000001).Arg(0).Field(0x040008BC).Field(0x04000888).Virtual(0x0A00002E).Integer(0).Virtual(0x0A00002F);
    private static byte[] N119_SkillTexts(bool board=false)
    {
        var b=new IL();if(!board){b.Arg(0).Arg(1).Field(0x0400068E).Set(0x04000732);b.Arg(0).Field(0x04000725).Branch(0x39,"return");}b.Arg(0).Field((board?0x040007A8:0x04000726)).Arg(1).Field(0x0400068D).Integer(1).Virtual(0x0A00004B);
        if(board)b.Float(1).Float(1).Float(1).Float(1).New(0x0A0000BE);else b.Arg(2);b.Call(0x0A00026C).Store(0);b.Arg(0).Field((board?0x040007A9:0x04000727)).Store(1);b.Arg(1).Field(0x0400068F).Store(2);b.Integer(10).Ref(0x8D,0x0100001A).Store(3);
        for(int i=0;i<9;i++)b.Load(3).Integer(i).Arg(1).Field(0x0400069C).Integer(i).Op(0x98).Ref(0x8C,0x0100001F).Op(0xA2);b.Load(3).Integer(9).Load(0).Op(0xA2);b.Load(1).Load(2).Load(3).Call(0x0A00026D).Integer(1).Virtual(0x0A00004B);
        N119_Active(b,(board?0x040007A0:0x04000729),false);N119_Active(b,(board?0x040007A1:0x0400072A),false);if(!board)b.Arg(1).Field(0x04000696).Branch(0x3A,"perpetual");for(int i=0;i<4;i++)b.Arg(1).Field(0x04000691).Integer(i).Branch(0x3B,"duration"+i);b.Branch(0x38,"chargeType");
        b.Label("duration3");N119_Text(b,(board?0x040007AD:0x04000728),N119_Permanent);b.Branch(0x38,"chargeType");b.Label("duration2");N119_Text(b,(board?0x040007AD:0x04000728),N119_PowerHit);b.Branch(0x38,"chargeType");
        b.Label("duration1").Arg(0).Field((board?0x040007AD:0x04000728)).Ref(0x72,N119_DurationFormat).Arg(1).Field(0x04000694).Ref(0x8C,0x0100001F).Call(0x0A00026E).Integer(1).Virtual(0x0A00004B);N119_Active(b,(board?0x040007A0:0x04000729),true);b.Branch(0x38,"chargeType");
        b.Label("duration0").Arg(0).Field((board?0x040007AD:0x04000728)).Ref(0x72,N119_AmmoFormat).Arg(1).Field(0x04000694).Ref(0x8C,0x0100001F).Call(0x0A00026E).Integer(1).Virtual(0x0A00004B);N119_Active(b,(board?0x040007A1:0x0400072A),true);b.Branch(0x38,"chargeType");
        b.Label("perpetual");N119_Text(b,(board?0x040007AD:0x04000728),N119_InfiniteDuration);N119_Active(b,(board?0x040007A0:0x04000729),true);
        b.Label("chargeType");int[] charges=[N119_ChargeNatural,N119_ChargeAttack,N119_ChargeHit,N119_ChargeNone];for(int i=0;i<4;i++){b.Arg(1).Field(0x04000690).Integer(i).Branch(0x40,"chargeNext"+i);N119_Text(b,(board?0x040007B0:0x0400072D),charges[i]);b.Branch(0x38,"trigger");b.Label("chargeNext"+i);}b.Arg(1).Field(0x04000690).Integer(-1).Branch(0x40,"trigger");N119_Text(b,(board?0x040007B0:0x0400072D),N119_FourDashes);
        b.Label("trigger");int[] triggers=[N119_TriggerAuto,N119_TriggerManual,N119_TriggerImmediate];for(int i=0;i<3;i++){b.Arg(1).Field(0x04000692).Integer(i).Branch(0x40,"triggerNext"+i);N119_Text(b,(board?0x040007B1:0x0400072E),triggers[i]);b.Branch(0x38,"charge");b.Label("triggerNext"+i);}b.Arg(1).Field(0x04000692).Integer(-1).Branch(0x40,"charge");N119_Text(b,(board?0x040007B1:0x0400072E),N119_FourDashes);
        b.Label("charge").Arg(0).Field((board?0x040007AE:0x0400072B)).Store(1);b.Arg(1).Field(0x04000699).Float(0).Branch(0x40,"chargeNumbers");b.Load(1).Ref(0x72,N119_ChargeEmpty).Integer(1).Virtual(0x0A00004B).Branch(0x38,"layers");
        b.Label("chargeNumbers").Load(1).Ref(0x72,board?N119_MaxChargeFormat:N119_ChargeFormat);if(!board)b.Arg(1).Field(0x04000698).Ref(0x8C,0x0100001F);b.Arg(1).Field(0x04000699).Ref(0x8C,0x0100001F).Call(board?0x0A00026E:0x0A00026F).Integer(1).Virtual(0x0A00004B);
        b.Label("layers").Arg(1).Field(0x0400069B).Integer(1).Branch(0x3D,"showLayers");N119_Active(b,(board?0x040007AF:0x0400072C),false);b.Branch(0x38,"return");b.Label("showLayers");N119_Active(b,(board?0x040007AF:0x0400072C),true);b.Arg(0).Field((board?0x040007AF:0x0400072C)).Arg(1).Ref(0x7C,0x0400069B).Ref(0x72,N119_LayerFormat).Call(0x0A000082).Integer(1).Virtual(0x0A00004B);
        b.Label("return").Op(0x2A);return b.Finish();
    }
}
