internal static partial class Program
{
    // Source-only implementations. Actual execution and qualification belong to the verifier.
    private static byte[] N96_PreCardBody()
    {
        var b = new IL();
        b.Arg(1).Field(0x040003F3).Integer(-1).Branch(0x3B, "device");
        b.New(0x060004D4).Store(20);
        b.Load(20).Arg(1).Field(0x040003F3).Set(0x0400065E);
        b.Load(20).Arg(1).Field(0x040003F4).Field(0x040003F8).Set(0x04000662);
        b.Load(20).Arg(1).Field(0x040003F4).Field(0x040003F9).Set(0x04000661);
        b.Static(0x0400015E).Field(0x04000166).Field(0x04000312).Field(0x04000315)
            .Load(20).Field(0x0400065E).Virtual(0x0A000059).Load(20).Virtual(0x06000256);
        b.Arg(0).Load(20).Field(0x0400065C).Set(0x040003FC);
        // Capture this.plantData before GetSkill_ID, which may replace it.
        b.Arg(0).Field(0x040003FE).Load(20).Load(20)
            .Arg(1).Field(0x040003F4).Field(0x040003FA).Virtual(0x060004CB).Virtual(0x060002EC);
        b.Arg(0).Load(20).Field(0x04000665).Set(0x04000401);
        b.Arg(0).Load(20).Field(0x04000664).Set(0x04000406);
        b.Arg(0).Load(20).Set(0x040003FF);
        b.Arg(0).Field(0x04000408).Static(0x040002D7)
            .Arg(0).Field(0x040003FE).Field(0x0400040F).Virtual(0x0A000040).Virtual(0x0A000041);
        b.Arg(0).Field(0x04000407).Static(0x040002E8).Virtual(0x0A000041);
        // These are two independent ifs. Re-read PreCard after all plant callbacks.
        b.Label("device").Arg(1).Field(0x040003F5).Integer(-1).Branch(0x3B, "return");
        b.Arg(0).Field(0x04000400).Store(2);
        b.Arg(0).Arg(1).Field(0x040003F5).Set(0x040003FD);
        b.Arg(1).Field(0x040003F6).Store(1);
        foreach (int field in new[] { 0x04000421, 0x04000422, 0x04000423, 0x04000424,
            0x04000425, 0x04000426, 0x04000427 })
            b.Load(2).Load(1).Field(field).Set(field);
        b.Arg(0).Arg(1).Field(0x040003F6).Field(0x04000425).Set(0x04000401);
        b.Arg(0).Arg(1).Field(0x040003F6).Field(0x04000426).Set(0x04000406);
        // Keep the old Image receiver across the resource loader callback.
        b.Arg(0).Field(0x04000408).Call(0x06000224)
            .Arg(0).Field(0x04000400).Field(0x04000423).Virtual(0x0A000040).Virtual(0x0A000041);
        b.Arg(0).Field(0x04000407).Static(0x040002E9).Virtual(0x0A000041);
        b.Arg(0).Field(0x0400040D).Ref(0x72, 0x7000B8C5)
            .Arg(0).Field(0x04000400).Ref(0x7C, 0x04000427).Call(0x0A00002A)
            .Call(0x0A000051).Integer(1).Virtual(0x0A00004B);
        b.Arg(0).Field(0x0400040D).Virtual(0x0A000009).Integer(1).Virtual(0x0A000007);
        b.Label("return").Op(0x2A);
        return b.Finish();
    }

    private static byte[] N96_SuppliesInfoBody()
    {
        var b = new IL();
        // Captured before either branch writes suppliesID or invokes Int32.ToString.
        b.Arg(0).Field(0x040000C1);
        b.Arg(1).Integer(0).Branch(0x3F, "negative");
        b.Arg(0).Arg(1).Set(0x040000BD);
        b.Op(0x0F, 1).Call(0x0A00002A).Virtual(0x0A00002B); // ldarga.s 1
        b.Arg(0).Arg(0).Field(0x040000C0).Field(0x0400033D)
            .Arg(1).Virtual(0x0A000081).Set(0x040000BF);
        b.Arg(0).Arg(0).Field(0x040000BF).Field(0x04000174).Set(0x040000BE);
        b.Arg(0).Field(0x040000C2).Arg(0).Field(0x040000BE).Virtual(0x0A00002B);
        b.Arg(0).Field(0x040000BF).Store(4);
        b.Arg(0).Field(0x040000C3).Load(4).Ref(0x7C, 0x04000176)
            .Call(0x0A00002A).Virtual(0x0A00002B);
        foreach (var pair in new[] { (UI:0x040000C4, Field:0x04000177),
            (UI:0x040000C5, Field:0x04000178), (UI:0x040000C6, Field:0x0400017A),
            (UI:0x040000C7, Field:0x04000179) })
        {
            b.Arg(0).Field(0x040000BF).Store(4); // native captures data before UI receiver.
            b.Arg(0).Field(pair.UI).Load(4).Field(pair.Field).Virtual(0x0A00002B);
        }
        foreach (int ui in new[] { 0x040000C2, 0x040000C3, 0x040000C4, 0x040000C5, 0x040000C6, 0x040000C7 })
            b.Arg(0).Field(ui).Integer(1).Virtual(0x0A000018);
        b.Op(0x2A);
        b.Label("negative").Integer(-1).Op(0x10, 1); // native normalizes the actual argument slot.
        b.Arg(0).Integer(-1).Set(0x040000BD);
        b.Ref(0x72, 0x70001669).Virtual(0x0A00002B);
        b.Arg(0).Op(0x14).Set(0x040000BF);
        b.Arg(0).Static(0x0A000013).Set(0x040000BE);
        b.Arg(0).Field(0x040000C2).Static(0x0A000013).Virtual(0x0A00002B);
        b.Arg(0).Field(0x040000C3).Ref(0x72, 0x70001669).Virtual(0x0A00002B);
        foreach (int ui in new[] { 0x040000C4, 0x040000C5, 0x040000C6, 0x040000C7 })
            b.Arg(0).Field(ui).Static(0x0A000013).Virtual(0x0A00002B);
        foreach (int ui in new[] { 0x040000C2, 0x040000C3, 0x040000C4, 0x040000C5, 0x040000C6, 0x040000C7 })
            b.Arg(0).Field(ui).Integer(0).Virtual(0x0A000018);
        b.Op(0x2A);
        return b.Finish();
    }

    private static byte[] N96_RemoveCardBody()
    {
        var b = new IL();
        b.Arg(1).Field(0x04000404).Branch(0x39, "return");
        b.Arg(0).Field(0x040000E9).Arg(1).Virtual(CardIndexOf).Store(6);
        b.Load(6).Integer(0).Branch(0x3E, "return"); // index zero intentionally cannot be removed.
        b.Integer(0).Store(3);
        b.Label("condition").Load(3).Arg(0).Field(0x040000E9).Virtual(0x0A0000A1)
            .Branch(0x3C, "remove");
        b.Load(3).Load(6).Branch(0x3E, "increment");
        b.Arg(0).Arg(0).Field(0x040000E9).Load(3).Virtual(0x0A0000A2)
            .Load(3).Integer(1).Op(0x59).Virtual(0x060000EC);
        // Re-read both list and item after SetCardPosition: callbacks may replace them.
        b.Arg(0).Field(0x040000E9).Load(3).Virtual(0x0A0000A2)
            .Op(0x25).Field(0x04000405).Integer(1).Op(0x59).Set(0x04000405);
        b.Label("increment").Load(3).Integer(1).Op(0x58).Store(3).Branch(0x38, "condition");
        b.Label("remove").Arg(1).Virtual(0x0A000009).Call(0x0A000025);
        b.Arg(0).Field(0x040000E9).Load(6).Virtual(CardRemoveAt);
        b.Arg(0).Arg(0).Field(0x040000EC).Integer(1).Op(0x59).Set(0x040000EC);
        b.Label("return").Op(0x2A);
        return b.Finish();
    }

    private static byte[] N96_InjuryBody()
    {
        var b = new IL();
        b.Arg(0).Field(0x040005B9).Integer(11).Branch(0x3B, "nut");
        b.Arg(0).Field(0x040005B9).Integer(12).Branch(0x3B, "return");
        b.Arg(0).Field(0x040005B9).Integer(17).Branch(0x3B, "shooter");
        b.Arg(0).Field(0x040005B9).Integer(18).Branch(0x3B, "skill");
        b.Arg(0).Field(0x040005D0).Branch(0x3A, "default_head");
        b.Arg(0).Field(0x040005BE).Op(0x25, 0x58, 0x6B).Float(3f).Op(0x5B, 0x6B)
            .Arg(0).Field(0x040005BD).Branch(0x43, "default_head");
        b.Arg(0).Arg(1).Call(0x0600042F);
        b.Label("default_head").Arg(0).Field(0x040005D1).Branch(0x3A, "death");
        b.Arg(0).Field(0x040005BE).Float(3f).Op(0x5B, 0x6B)
            .Arg(0).Field(0x040005BD).Branch(0x43, "death");
        b.Arg(0).Arg(1).Call(0x06000453);
        b.Arg(0).Call(0x06000438).Branch(0x38, "death");

        b.Label("skill").Arg(0).Arg(0).Field(0x040005BE).Arg(0).Field(0x040005BD)
            .Op(0x59, 0x6B).Float(4f).Op(0x5A, 0x6B)
            .Arg(0).Field(0x040005BE).Op(0x5B, 0x6B, 0x6C).Call(0x0A0001FB);
        // Original 0x180365479 is CVTTSD2SI after floor(double).
        // Keep [this, flooredDouble] below both comparisons (peak stack 4).
        // NaN and out-of-range values produce the x64 indefinite result, not saturation.
        b.Op(0x25,0x23).Op(BitConverter.GetBytes(-2147483648d)).Branch(0x44,"skillIndefinite");
        b.Op(0x25,0x23).Op(BitConverter.GetBytes(2147483648d)).Branch(0x3C,"skillIndefinite");
        b.Op(0x69).Branch(0x38,"skillAmount");
        b.Label("skillIndefinite").Op(0x26).Integer(int.MinValue);
        b.Label("skillAmount").Set(0x040005C1);
        b.Arg(0).Field(0x040005C1).Integer(3).Branch(0x3F, "death");
        b.Arg(0).Field(0x040005EB).Branch(0x39, "death");
        b.Arg(0).Arg(0).Field(0x040005BE).Float(0.25f).Op(0x5A, 0x6B).Set(0x040005BD);
        b.Arg(0).Field(0x040005FE); // capture animator before clearing IVH_skill3.
        b.Arg(0).Integer(0).Set(0x040005EB);
        b.Call(0x0A00000B).Branch(0x39, "invincible");
        b.Arg(0).Field(0x040005FE); // re-read BEFORE setting skillOngoning.
        b.Arg(0).Integer(1).Set(0x040005EC);
        b.Ref(0x72, 0x70010F96).Virtual(0x0A000087);
        b.Label("invincible").Arg(0).Integer(1).Set(0x040005D8).Branch(0x38, "death");

        b.Label("shooter").Integer(0).Store(1);
        b.Arg(0).Field(0x040005BE).Op(0x25, 0x58, 0x6B).Float(3f).Op(0x5B, 0x6B)
            .Arg(0).Field(0x040005BD).Branch(0x43, "shooter_second");
        b.Integer(1).Store(1);
        b.Label("shooter_second").Arg(0).Field(0x040005BE).Float(3f).Op(0x5B, 0x6B)
            .Arg(0).Field(0x040005BD).Branch(0x43, "shooter_anim");
        b.Integer(2).Store(1);
        b.Label("shooter_anim").Arg(0).Field(0x040005FE).Call(0x0A00000B).Branch(0x39, "death");
        b.Arg(0).Field(0x040005FE).Ref(0x72, 0x70010FB2).Load(1).Virtual(0x0A0001FD);
        b.Load(1).Integer(2).Branch(0x44, "shooter_commit");
        b.Arg(0).Field(0x040005C1).Integer(2).Branch(0x3C, "shooter_ready");
        b.Arg(0).Field(0x040005F9).Virtual(0x0A000224);
        b.Arg(0).Call(0x06000433);
        b.Arg(0).Call(0x0600046A).Branch(0x39, "shooter_ready");
        b.Arg(0).Call(0x06000485);
        b.Label("shooter_ready").Arg(0).Field(0x040005FE).Ref(0x72, 0x7000FD4F).Integer(0).Virtual(0x0A0001F3);
        b.Arg(0).Integer(0).Set(0x040005E1);
        b.Label("shooter_commit").Arg(0).Load(1).Set(0x040005C1).Branch(0x38, "death");

        b.Label("nut").Float(1f).Arg(0).Field(0x040005BD).Arg(0).Field(0x040005BE)
            .Op(0x5B, 0x6B, 0x59, 0x6B).Store(0);
        b.Integer(0).Store(2).Integer(-1).Store(3);
        b.Arg(0).Field(0x040005D0).Branch(0x3A, "nut_head");
        b.Load(0).Float(0.6f).Branch(0x43, "nut_head");
        b.Float(0.8f).Load(0).Branch(0x44, "nut_arm");
        b.Arg(0).Call(0x0600049E).Integer(1).Store(2);
        b.Label("nut_arm").Arg(0).Arg(1).Call(0x0600042F);
        b.Label("nut_head").Arg(0).Field(0x040005D1).Branch(0x3A, "nut_death");
        b.Load(0).Float(0.8f).Branch(0x43, "nut_death");
        b.Arg(0).Arg(1).Call(0x06000453);
        b.Arg(0).Call(0x06000438);
        b.Arg(0).Field(0x040005DD).Branch(0x39, "nut_death");
        b.Arg(0).Call(0x0600049D);
        b.Label("nut_death").Arg(0).Field(0x040005D2).Branch(0x3A, "nut_ranges");
        b.Float(0f).Arg(0).Field(0x040005BD).Branch(0x44, "nut_ranges");
        b.Arg(0).Arg(1).Integer(0).Call(0x06000437);
        b.Label("nut_ranges").Load(0).Float(0f).Branch(0x44, "nut_range2");
        b.Float(0.2f).Load(0).Branch(0x44, "nut_range2");
        b.Arg(0).Field(0x040005C1).Branch(0x39, "nut_index19");
        b.Arg(0).Integer(0).Set(0x040005C1).Integer(1).Store(2);
        b.Label("nut_index19").Integer(19).Store(3);
        b.Label("nut_range2").Load(0).Float(0.2f).Branch(0x43, "nut_range3");
        b.Float(0.4f).Load(0).Branch(0x44, "nut_range3");
        b.Arg(0).Field(0x040005C1).Integer(1).Branch(0x3B, "nut_index20");
        // Native really writes ZERO in this and the next range; do not normalize to 1/2.
        b.Arg(0).Integer(0).Set(0x040005C1).Integer(1).Store(2);
        b.Label("nut_index20").Integer(20).Store(3);
        b.Label("nut_range3").Load(0).Float(0.4f).Branch(0x43, "nut_sprite");
        b.Arg(0).Field(0x040005C1).Integer(2).Branch(0x3B, "nut_index21");
        b.Arg(0).Integer(0).Set(0x040005C1).Integer(1).Store(2);
        b.Label("nut_index21").Integer(21).Store(3);
        b.Label("nut_sprite").Load(3).Integer(-1).Branch(0x3B, "return");
        b.Load(2).Branch(0x39, "return");
        b.Arg(0).Field(0x040005DD).Branch(0x39, "sprite_find");
        b.Load(3).Integer(3).Op(0x58).Store(3);
        b.Label("sprite_find").Op(0x14).Store(5);
        b.Arg(0).Field(0x04000609).Ref(0x72, 0x7000819E).Call(0x06000142).Store(4);
        b.Load(4).Branch(0x39, "renderer_check"); // raw reference check, not Unity equality.
        b.Load(4).Virtual(0x2B000011).Store(5); // GameObject.GetComponent<SpriteRenderer>.
        b.Label("renderer_check").Load(5).Call(0x0A00000B).Branch(0x39, "return");
        b.Load(5).Static(0x040002CE).Load(3).Virtual(0x0A000040).Virtual(0x0A000029).Branch(0x38, "return");

        b.Label("death").Float(0f).Arg(0).Field(0x040005BD).Branch(0x44, "return");
        b.Arg(0).Field(0x040005D2).Branch(0x3A, "return");
        b.Arg(0).Arg(1).Integer(0).Call(0x06000437);
        b.Label("return").Arg(0).Call(0x06000464).Op(0x2A);
        return b.Finish();
    }
}
