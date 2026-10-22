internal static partial class Program
{
    private static byte[] N119_Update10()
    {
        var b=new IL();b.Arg(0).Field(0x040004F3).Store(0);b.New(0x0A00016B).Store(1);b.Arg(0).Field(0x040004AF).Integer(1).Virtual(0x060005BD).Store(2);
        b.Load(0).Field(0x040003D6).Field(0x040002B6).Virtual(N119_ProjectileEnum).Store(3);b.Label("tryFilter").Branch(0x38,"filterMove");b.Label("filter").Address(3).Call(N119_ProjectileCurrent).Store(4);
        b.Load(4).Virtual(0x0A000009).Virtual(0x0A000154).Branch(0x39,"filterMove");b.Load(4).Field(0x0400055A).Integer(5).Branch(0x3B,"filterMove");b.Load(4).Field(0x0400054D).Integer(2).Branch(0x40,"filterMove");b.Load(2).Load(4).Virtual(0x2B00006B).Branch(0x39,"filterMove");b.Load(1).Load(4).Virtual(0x0A000166);
        b.Label("filterMove").Address(3).Call(N119_ProjectileMove).Branch(0x3A,"filter").Branch(0xDD,"filtered");b.Label("finallyFilter").Address(3).Call(N119_ProjectileDispose).Op(0xDC);b.Label("filtered").Float(.8f).Store(5);
        b.Load(1).Virtual(N119_ProjectileEnum).Store(3);b.Label("tryDestroy").Branch(0x38,"destroyMove");b.Label("destroy").Address(3).Call(N119_ProjectileCurrent).Store(4);
        b.Load(4).Field(0x0400054C).Store(6);b.Load(6).Integer(9).Op(0x59).Integer(2).Branch(0x43,"explode");b.Load(6).Integer(15).Branch(0x3B,"explode");b.Load(6).Integer(26).Op(0x59).Integer(6).Branch(0x42,"collision");
        b.Label("explode").Load(4).Field(0x0400054E).Virtual(0x0600010C).Op(0x26);b.Label("collision").Load(4).Virtual(0x060003EB);b.Load(4).Virtual(0x060003EC);
        b.Float(.9f).Float(1.1f).Call(0x0A000083).Store(7);b.Static(0x040002EF).Integer(2).Integer(4).Call(0x0A00012B).Virtual(0x0A000084).Store(8);b.Call(0x0A000004).Virtual(0x0A000001).Virtual(0x0A000037).Store(9);
        b.Load(8).Load(9).Call(0x06000138).Load(5).Op(0x5A,0x6B).Load(7).Call(0x0600013E).Op(0x26);b.Load(5).Float(.9f).Op(0x5A,0x6B).Store(5);
        b.Label("destroyMove").Address(3).Call(N119_ProjectileMove).Branch(0x3A,"destroy").Branch(0xDD,"destroyed");b.Label("finallyDestroy").Address(3).Call(N119_ProjectileDispose).Op(0xDC);b.Label("destroyed");
        b.Load(0).Field(0x040003D5).Field(0x04000362).Virtual(N119_ZombieEnum).Store(10);b.Label("trySlow").Branch(0x38,"slowMove");b.Label("slow").Address(10).Call(N119_ZombieCurrent).Store(11);
        b.Load(11).Virtual(0x06000431).Branch(0x39,"slowMove");b.Load(11).Field(0x040005F1).Arg(0).Field(0x040004C8).Branch(0x3B,"slowMove");b.Load(2).Load(11).Virtual(0x2B000055).Branch(0x39,"slowMove");
        b.Load(11).Field(0x0400061A).Ref(0x72,N119_SlowName).Virtual(0x06000106).Branch(0x39,"newSlow");b.Load(11).Field(0x0400061A).Ref(0x72,N119_SlowName).Virtual(0x06000106).Field(0x04000109).Arg(0).Call(0x0A000012).Branch(0x39,"newSlow");
        b.Load(11).Field(0x0400061A).Ref(0x72,N119_SlowName).Virtual(0x06000106).Load(2).Set(0x0400010C).Branch(0x38,"slowMove");
        b.Label("newSlow").Ref(0x72,N119_MoveSpeed).Arg(0).Field(0x040004D1).Field(0x0400069C).Integer(2).Op(0x98).Float(-1).Op(0x5A,0x6B).Integer(1).New(0x060000F6).Store(12);
        b.Load(12).Ref(0x72,N119_SlowName).Set(0x04000105);b.Load(12).Integer(2).Set(0x04000106);b.Load(12).Integer(1).Set(0x04000108);b.Load(12).Arg(0).Set(0x04000109);b.Load(12).Load(2).Set(0x0400010C);
        b.Load(12).Call(0x060001E5).Integer(29).Virtual(0x060001EA).Set(0x0400010E);b.Load(11).Field(0x0400061A).Load(12).Virtual(0x06000102);
        b.Label("slowMove").Address(10).Call(N119_ZombieMove).Branch(0x3A,"slow").Branch(0xDD,"slowed");b.Label("finallySlow").Address(10).Call(N119_ZombieDispose).Op(0xDC);b.Label("slowed").Op(0x2A);
        N119_Finally(b,0x06000507,("tryFilter","finallyFilter","filtered"),("tryDestroy","finallyDestroy","destroyed"),("trySlow","finallySlow","slowed"));return b.Finish();
    }
    private static void N119_AimVector(IL b,int index,int vector)
    {
        // Both directions use projectile one as the origin, including the second target.
        b.Load(7).Integer(index).Virtual(N119_VectorItem).Store(11);b.Address(11).Field(0x0A00001D).Load(3).Field(0x04000552).Op(0x59,0x6B).Store(12);
        b.Load(7).Integer(index).Virtual(N119_VectorItem).Store(11);b.Address(11).Field(0x0A00008B).Load(3).Field(0x04000553).Op(0x59,0x6B).Store(13);b.Load(12).Load(13).Float(0).New(0x0A00009C).Store(vector);
    }
    private static byte[] N119_Function22()
    {
        var b=new IL();b.Arg(0).Field(0x040004F3).Store(0);b.Arg(0).Field(0x040004C8).Store(1);b.Arg(0).Field(0x040004DF).Ref(0x72,N119_Crossbow1).Call(0x06000141).Store(2);b.Arg(0).Field(0x040004DF).Ref(0x72,N119_Crossbow2).Call(0x06000141).Store(14);
        b.Load(0).Field(0x040003D6).Integer(25).Virtual(0x060001F6).Store(3);b.Load(0).Field(0x040003D6).Integer(25).Virtual(0x060001F6).Store(4);
        b.Arg(0).Load(3).Integer(25).Virtual(0x0600036C).Store(5);b.Arg(0).Load(4).Integer(25).Virtual(0x0600036C).Store(6);
        foreach(int d in new[]{5,6})b.Load(d).Load(d).Field(0x04000123).Arg(0).Field(0x040004D1).Field(0x0400069C).Integer(0).Op(0x98,0x5A,0x6B).Set(0x04000123);
        foreach(int d in new[]{5,6})b.Load(d).Load(d).Field(0x0400012F).Arg(0).Field(0x040004D1).Field(0x0400069C).Integer(1).Op(0x98,0x58,0x6B).Set(0x0400012F);
        for(int i=0;i<2;i++)
        {
            b.Load(3+i).Load(i==0?2:14).Float(1555).Float(0).Float(0).New(0x0A00009C).Float(0).Float(0).Float(0).New(0x0A00009C).Float(70).Integer(0).Arg(0).Virtual(N119_CrossbowInitial);
            b.Load(3+i).Load(5+i).Set(0x0400054E);b.Load(3+i).Load(1).Set(0x0400054D);
        }
        b.New(0x0A000095).Store(7);b.Arg(0).Integer(2).Virtual(0x06000361).Store(8);b.Integer(0).Store(10).Branch(0x38,"zTest");b.Label("z").Load(7);
        for(int f=0x040005AD;f<=0x040005AF;f++)b.Load(8).Load(10).Virtual(0x0A00006C).Field(f);
        b.New(0x0A00009C).Virtual(N119_VectorAdd);b.Load(10).Integer(1).Op(0x58).Store(10);b.Label("zTest").Load(10).Load(8).Virtual(N119_ZombieListCount).Branch(0x3F,"z");
        b.Load(7).Virtual(N119_VectorCount).Integer(2).Branch(0x3C,"aim");b.Arg(0).Integer(2).Load(7).Virtual(N119_VectorCount).Op(0x59).Virtual(0x06000364).Store(9);
        b.Integer(0).Store(10).Branch(0x38,"dTest");b.Label("d").Load(7);for(int f=0x04000466;f<=0x04000468;f++)b.Load(9).Load(10).Virtual(0x0A000127).Field(f);
        b.New(0x0A00009C).Virtual(N119_VectorAdd);b.Load(10).Integer(1).Op(0x58).Store(10);b.Label("dTest").Load(10).Load(9).Virtual(N119_DeviceListCount).Branch(0x3F,"d");
        b.Label("aim").Load(7).Virtual(N119_VectorCount).Integer(1).Branch(0x40,"two");N119_AimVector(b,0,15);b.Load(3).Load(15).Virtual(0x060003DC);b.Load(4).Load(15).Virtual(0x060003DC);
        b.Label("two").Load(7).Virtual(N119_VectorCount).Integer(2).Branch(0x40,"return");N119_AimVector(b,0,15);N119_AimVector(b,1,16);b.Load(3).Load(15).Virtual(0x060003DC);b.Load(4).Load(16).Virtual(0x060003DC);
        b.Label("return").Op(0x2A);return b.Finish();
    }
    private static void N119_Skill150Audio(IL b,int index,float scale)
    {
        b.Static(0x040002F1).Integer(index).Virtual(0x0A000084).Store(10);b.Call(0x0A000004).Virtual(0x0A000001).Virtual(0x0A000037).Store(11);
        b.Load(10).Load(11).Call(0x06000138);if(scale!=1)b.Op(0x25,0x58,0x6B);b.Call(0x0600013D).Op(0x26);
    }
    private static void N119_Skill150Damage(IL b,int target,int method)
    {
        b.Arg(0).Op(0x14).Integer(-1).Virtual(0x0600036C).Store(4);b.Load(4).Arg(0).Field(0x040004D1).Field(0x0400069C).Integer(0).Op(0x98).Load(4).Field(0x04000123).Op(0x5A,0x6B).Set(0x04000123);
        b.Load(target).Load(4).Op(0x14).Virtual(method);
    }
    private static byte[] N119_Function150()
    {
        var b=new IL();b.Arg(3).Branch(0x39,"burst");b.Arg(3).Arg(0).Field(0x040004D1).Field(0x0400069C).Integer(0).Op(0x98).Arg(3).Field(0x04000123).Op(0x5A,0x6B).Set(0x04000123);
        b.Arg(0).Field(0x040004AF).Integer(3).Virtual(0x060005BD).Op(0x26);b.Arg(0).Integer(999).Virtual(0x06000361).Store(0);b.Load(0).Virtual(N119_ZombieListCount).Integer(0).Branch(0x3E,"devices");
        b.Load(0).Virtual(N119_ZombieEnum).Store(1);b.Label("tryZ").Branch(0x38,"zMove");b.Label("z").Address(1).Call(N119_ZombieCurrent).Store(2);N119_Skill150Damage(b,2,0x0600047F);
        b.Label("zMove").Address(1).Call(N119_ZombieMove).Branch(0x3A,"z").Branch(0xDD,"zDone");b.Label("finallyZ").Address(1).Call(N119_ZombieDispose).Op(0xDC);b.Label("zDone");N119_Skill150Audio(b,25,2);
        b.Label("devices").Load(0).Virtual(N119_ZombieListCount).Integer(999).Branch(0x3C,"return");b.Arg(0).Integer(999).Load(0).Virtual(N119_ZombieListCount).Op(0x59).Virtual(0x06000364).Store(3);b.Load(3).Virtual(N119_DeviceListCount).Integer(0).Branch(0x3E,"return");
        b.Load(3).Virtual(N119_DeviceEnum).Store(5);b.Label("tryD").Branch(0x38,"dMove");b.Label("d").Address(5).Call(N119_DeviceCurrent).Store(6);N119_Skill150Damage(b,6,0x06000336);b.Static(0x040002F1).Integer(21).Virtual(0x0A000084).Call(0x0600013B).Op(0x26);
        b.Label("dMove").Address(5).Call(N119_DeviceMove).Branch(0x3A,"d").Branch(0xDD,"dDone");b.Label("finallyD").Address(5).Call(N119_DeviceDispose).Op(0xDC);b.Label("dDone").Branch(0x38,"return");
        b.Label("burst").Arg(0).Virtual(0x0A000001).Virtual(0x0A000037).Store(7);b.Address(7).Op(0x25).Field(0x0A00008B).Float(30).Op(0x58,0x6B).Set(0x0A00008B);
        // PrivateImplementationDetails input pins the five exact angles (degrees).
        b.Integer(5).Ref(0x8D,0x0100001F).Store(8);float[] angles=[30,90,180,270,330];for(int i=0;i<5;i++)b.Load(8).Integer(i).Float(angles[i]).Op(0xA0);
        b.Integer(0).Store(9);b.Label("shot").Float(0).Float(0).Load(8).Load(9).Op(0x98).Call(0x0A00020E).Float(888).Float(0).Float(0).New(0x0A00009C).Call(0x0A00020F).Store(12);
        b.Arg(0).Field(0x040004F3).Field(0x040003D6).Integer(23).Virtual(0x060001F6).Store(13);b.Arg(0).Load(13).Integer(23).Virtual(0x0600036C).Store(4);
        b.Load(4).Arg(0).Field(0x040004D1).Field(0x0400069C).Integer(1).Op(0x98).Load(4).Field(0x04000123).Op(0x5A,0x6B).Set(0x04000123);
        b.Load(13).Load(7).Load(12).Float(0).Float(0).Float(0).New(0x0A00009C).Float(30).Integer(0).Arg(0).Virtual(N119_CrossbowInitial);
        // Native reads camp after Initial, then installs the already captured damage.
        b.Arg(0).Field(0x040004C8).Store(14);b.Load(13).Load(4).Set(0x0400054E);b.Load(13).Load(14).Set(0x0400054D);b.Load(13).Load(12).Virtual(0x060003DC);
        b.Load(9).Integer(1).Op(0x58).Store(9);b.Load(9).Integer(5).Branch(0x3F,"shot");N119_Skill150Audio(b,26,1);
        b.Label("return").Op(0x2A);N119_Finally(b,0x0600051B,("tryZ","finallyZ","zDone"),("tryD","finallyD","dDone"));return b.Finish();
    }
    private static byte[] N119_ChomperViking()
    {
        var b=new IL();b.Arg(0).Op(0x14).Integer(-1).Integer(0).Virtual(0x0600036D).Store(0);b.Load(0).Field(0x04000123).Store(1);b.New(N119_ZombieListCtor).Store(2);b.New(0x0A0000CB).Store(3);b.New(N119_ZombieListCtor).Store(4);
        b.Arg(0).Field(0x040004AF).Store(11);b.Arg(0).Field(0x040004D2).Branch(0x39,"normalRange");b.Arg(0).Field(0x040004D1).Field(0x0400068E).Store(12).Branch(0x38,"range");b.Label("normalRange").Integer(0).Store(12);b.Label("range").Load(11).Load(12).Virtual(0x060005BD).Store(5);
        b.Arg(0).Field(0x040004F3).Field(0x040003D5).Field(0x04000362).Virtual(N119_ZombieEnum).Store(6);b.Label("tryZ").Branch(0x38,"zMove");b.Label("z").Address(6).Call(N119_ZombieCurrent).Store(7);
        b.Load(7).Virtual(0x06000431).Branch(0x39,"zMove");b.Load(7).Field(0x040005D2).Branch(0x3A,"zMove");b.Arg(0).Field(0x040004DD).Load(7).Virtual(N119_ZombieIndexOf).Integer(0).Branch(0x3C,"zCamp");b.Load(5).Load(7).Virtual(0x2B000055).Branch(0x39,"zMove");
        b.Label("zCamp").Load(7).Field(0x040005F1).Arg(0).Field(0x040004C8).Branch(0x3B,"zMove");b.Arg(0).Load(2).Load(4).Load(7).Load(0).Virtual(0x060003AA);
        b.Label("zMove").Address(6).Call(N119_ZombieMove).Branch(0x3A,"z").Branch(0xDD,"zDone");b.Label("finallyZ").Address(6).Call(N119_ZombieDispose).Op(0xDC);b.Label("zDone");
        b.Arg(0).Field(0x040004F3).Field(0x040003D8).Field(0x040001D6).Virtual(N119_DeviceEnum).Store(8);b.Label("tryD").Branch(0x38,"dMove");b.Label("d").Address(8).Call(N119_DeviceCurrent).Store(9);
        // GetDamage occurs even for subsequently excluded device IDs.
        b.Arg(0).Op(0x14).Integer(-1).Integer(0).Virtual(0x0600036D).Store(10);foreach(int id in new[]{2,7,8,12,14})b.Load(9).Field(0x0400046F).Integer(id).Branch(0x3B,"dMove");
        b.Load(9).Field(0x0400046F).Integer(11).Branch(0x40,"unbroken");b.Load(10).Field(0x04000125).Integer(3).Branch(0x40,"dMove").Branch(0x38,"dRange");b.Label("unbroken").Load(9).Field(0x0400047D).Branch(0x3A,"dMove");
        b.Label("dRange").Load(5).Load(9).Virtual(0x2B00005C).Branch(0x39,"dMove");b.Load(9).Field(0x0400046E).Arg(0).Field(0x040004C8).Branch(0x3B,"dMove");b.Load(3).Load(9).Virtual(0x0A0000CE);
        b.Label("dMove").Address(8).Call(N119_DeviceMove).Branch(0x3A,"d").Branch(0xDD,"dDone");b.Label("finallyD").Address(8).Call(N119_DeviceDispose).Op(0xDC);b.Label("dDone");
        b.Load(2).Virtual(N119_ZombieEnum).Store(6);b.Label("tryHit").Branch(0x38,"hitMove");b.Label("hit").Address(6).Call(N119_ZombieCurrent).Store(7);b.Load(0).Load(1).Set(0x04000123);b.Load(7).Load(0).Op(0x14).Virtual(0x0600047F);
        b.Label("hitMove").Address(6).Call(N119_ZombieMove).Branch(0x3A,"hit").Branch(0xDD,"hitDone");b.Label("finallyHit").Address(6).Call(N119_ZombieDispose).Op(0xDC);b.Label("hitDone");
        b.Load(4).Virtual(N119_ZombieEnum).Store(6);b.Label("tryDevour").Branch(0x38,"devourMove");b.Label("devour").Address(6).Call(N119_ZombieCurrent).Store(7);
        b.Arg(0).Field(0x040004E3).Ref(0x72,N119_DevourTrigger).Virtual(0x0A000087);b.Arg(0).Integer(1).Set(0x040004F9);b.Arg(0).Field(0x040004B4).Integer(1).Branch(0x3F,"destroyDevoured");
        b.Arg(0).Load(7).Field(0x040005BD).Float(.4f).Op(0x5A,0x6B).Arg(0).Field(0x040004FA).Op(0x58,0x6B).Set(0x040004FA);b.Label("destroyDevoured").Load(7).Virtual(0x06000436);
        b.Label("devourMove").Address(6).Call(N119_ZombieMove).Branch(0x3A,"devour").Branch(0xDD,"devourDone");b.Label("finallyDevour").Address(6).Call(N119_ZombieDispose).Op(0xDC);b.Label("devourDone");
        b.Load(3).Virtual(N119_DeviceEnum).Store(8);b.Label("tryHitD").Branch(0x38,"hitDMove");b.Label("hitD").Address(8).Call(N119_DeviceCurrent).Store(9);b.Load(0).Load(1).Set(0x04000123);b.Load(9).Load(0).Op(0x14).Virtual(0x06000336);
        b.Label("hitDMove").Address(8).Call(N119_DeviceMove).Branch(0x3A,"hitD").Branch(0xDD,"hitDDone");b.Label("finallyHitD").Address(8).Call(N119_DeviceDispose).Op(0xDC);b.Label("hitDDone").Op(0x2A);
        N119_Finally(b,0x060003A9,("tryZ","finallyZ","zDone"),("tryD","finallyD","dDone"),("tryHit","finallyHit","hitDone"),("tryDevour","finallyDevour","devourDone"),("tryHitD","finallyHitD","hitDDone"));return b.Finish();
    }
}
