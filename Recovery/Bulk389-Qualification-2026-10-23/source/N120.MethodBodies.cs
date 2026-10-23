// Native120 source proposal only. No candidate has been constructed from this file.
internal static partial class Program
{
    private static byte[] N120_SetSortingGroup()
    {
        var b = new IL();
        // Native 18037C628/645: Unity fake-null semantics, not reference-only null.
        b.Arg(0).Field(0x04000570).Op(0x14).Call(0x0A000012).Branch(0x39, "haveGroup");
        b.Arg(0).Arg(0).Virtual(0x0A000009).Virtual(0x2B000057).Set(0x04000570);
        // Reload after AddComponent and after the setter: retain native callback order.
        b.Label("haveGroup").Arg(0).Field(0x04000570).Ref(0x72, 0x70001E24).Virtual(0x0A00015A);
        // mulss / cvttss2si / sub edx,eax: -10 - trunc_i32(fY * 10.0f).
        // Do not replace this with rounding, double arithmetic or -(10 + fY)*10.
        b.Arg(0).Field(0x04000570).Integer(-10).Arg(0).Field(0x04000553)
            .Float(10.0f).Op(0x5A).Op(0x69).Op(0x59).Virtual(0x0A00015B).Op(0x2A);
        return b.Finish();
    }
}
