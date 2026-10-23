internal static partial class Program
{
    // Native 0x180307DA0: numeric parsing is TryParse, including current-culture behavior.
    // The string-hash dispatch is expressed as ordinal equality cases; no case body runs
    // for null/unknown names. Capture inputFieldName once before invoking other methods.
    private static byte[] N119_InputString()
    {
        var b=new IL();b.Arg(1).Call(0x0A000010);b.Arg(0).Field(0x0400006D).Store(0);
        var cases=new (string Name,int Literal,int Field)[]{
            ("HealthBase",N119_InputHealthBase,0x04000071),("HealthFactor",N119_InputHealthFactor,0x04000072),
            ("AttackBase",N119_InputAttackBase,0x04000073),("AttackFactor",N119_InputAttackFactor,0x04000074),
            ("DefenseBase",N119_InputDefenseBase,0x04000075),("DefenseFactor",N119_InputDefenseFactor,0x04000076),
            ("PlantName",N119_InputPlantName,0),("PlantID",N119_InputPlantID,0),
            ("SkillName",N119_InputSkillName,0),("SkillID",N119_InputSkillID,0),("Order",N119_InputOrder,0),
            ("SunPrice",N119_InputSunPrice,0x0400031D),("ArmorPoint",N119_InputArmorPoint,0x04000323),
            ("SkillDuration",N119_InputSkillDuration,0x04000694),("AttackInterva",N119_InputAttackInterva,0x04000326),
            ("ReplantCD",N119_InputReplantCD,0x0400031E)};
        foreach(var c in cases)b.Load(0).Ref(0x72,c.Literal).Call(0x0A00003A).Branch(0x3A,c.Name);
        b.Branch(0x38,"return");
        foreach(var c in cases)
        {
            b.Label(c.Name);
            if(c.Field>=0x04000071&&c.Field<=0x04000076)
            {
                b.Arg(1).Address(1).Call(0x0A000044).Branch(0x39,"invalid");b.Arg(0).Load(1).Set(c.Field);
            }
            else if(c.Name=="PlantName")
            {
                b.Arg(0).Arg(1).Set(0x04000070);b.Arg(0).Field(0x0400006E).Arg(1).Set(0x04000319);
                // Native performs three independent indexed lookups, in this order.
                foreach(int field in new[]{0x04000656,0x0400065C,0x0400065E})
                {
                    b.Arg(0).Field(0x0400006B).Field(0x04000314).Arg(0).Field(0x0400006F).Virtual(0x0A000058);
                    if(field==0x04000656)b.Arg(0).Field(0x04000070);else b.Arg(0).Field(0x0400006F);
                    b.Set(field);
                }
            }
            else if(c.Name=="SkillName")
            {
                b.Arg(0).Field(0x04000089).Arg(1).Set(0x0400068D);
                b.Arg(0).Field(0x0400006F).Arg(0).Field(0x04000088).Call(0x060004E7).Store(2);
                b.Arg(0).Field(0x0400006C).Field(0x040006A5).Load(2).Virtual(0x0A000057).Arg(1).Set(0x040006A6);
            }
            else if(c.Name=="PlantID"||c.Name=="SkillID"||c.Name=="Order")
            {
                b.Arg(1).Address(2).Call(0x0A00003B).Branch(0x39,"invalid");b.Arg(0).Load(2).Call(c.Name=="PlantID"?0x06000073:0x06000074);
            }
            else
            {
                bool integer=c.Name=="SunPrice";
                b.Arg(1).Address(integer?2:1).Call(integer?0x0A00003B:0x0A000044).Branch(0x39,"invalid");
                b.Arg(0).Field(c.Name=="SkillDuration"?0x04000089:0x0400006E);
                if(c.Name=="SkillDuration")b.Load(1).Set(c.Field);
                else b.Field(c.Field).Arg(0).Field(0x04000082).Load(integer?2:1).Op(integer?(byte)0x9E:(byte)0xA0);
            }
            b.Branch(0x38,"return");
        }
        b.Label("invalid").Ref(0x72,N119_InputInvalid).Call(0x0A000010);b.Label("return").Op(0x2A);return b.Finish();
    }
}
