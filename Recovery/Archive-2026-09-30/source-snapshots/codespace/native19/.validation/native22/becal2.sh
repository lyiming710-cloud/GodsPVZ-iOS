export DOTNET_CLI_TELEMETRY_OPTOUT=1 DOTNET_NOLOGO=1 DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
B=/workspaces/GodsPVZ-native19/.validation/native22/behave
mkdir -p "$B"
cat > "$B/cal/Program.cs" <<'CSEOF'
#nullable disable
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

public class Box { public object F; public string S; public int N; }

public static class Program
{
    static readonly Type[] O = { typeof(object) };
    static readonly Type[] I = { typeof(int) };
    static readonly Type[] X = { typeof(Box) };
    static readonly Type[] B = { typeof(StringBuilder) };
    static readonly Type[] N = new Type[0];

    static string Run(string name, Type ret, Type[] sig, Action<ILGenerator> body, object[] vals)
    {
        try
        {
            var dm = new DynamicMethod(name, ret, sig, typeof(Program).Module, true);
            var il = dm.GetILGenerator();
            body(il);
            il.Emit(OpCodes.Ret);
            object r = dm.Invoke(null, vals);
            return "VALUE " + (r == null ? "<null>" : r.ToString());
        }
        catch (TargetInvocationException tie)
        { return "THROW " + (tie.InnerException == null ? "?" : tie.InnerException.GetType().FullName); }
        catch (Exception e) { return "THROW " + e.GetType().FullName; }
    }
    static string R(string n, Action<ILGenerator> body, object[] v) { return Run(n, typeof(int), N, body, v); }
    static void P(string tag, string r) { Console.WriteLine("  {0,-48} {1}", tag, r); }

    public static void Main()
    {
        Console.WriteLine("runtime: " + Environment.Version + "  "
            + System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription);
        FieldInfo FF = typeof(Box).GetField("F");
        FieldInfo FN = typeof(Box).GetField("N");
        object obj = new object();
        var sb = new StringBuilder();
        var boxNull = new Box { F = null, N = 0 };
        var boxObj = new Box { F = new object(), N = 7 };
        ConstructorInfo nreCtor = typeof(NullReferenceException).GetConstructor(Type.EmptyTypes);

        Console.WriteLine();
        Console.WriteLine("=== 1. ceq: reference vs the zero constant (the repaired family) ===");
        P("ldnull;              ldnull;    ceq", R("a1", il => { il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Ceq); }, null));
        P("ldnull;              ldc.i4.0;  ceq", R("a2", il => { il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); }, null));
        P("obj(non-null);       ldnull;    ceq", Run("a3", typeof(int), O, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Ceq); }, new object[] { obj }));
        P("obj(non-null);       ldc.i4.0;  ceq  [DAMAGED]", Run("a4", typeof(int), O, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); }, new object[] { obj }));
        P("obj(null);           ldnull;    ceq", Run("a5", typeof(int), O, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Ceq); }, new object[] { null }));
        P("obj(null);           ldc.i4.0;  ceq  [DAMAGED]", Run("a6", typeof(int), O, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); }, new object[] { null }));
        P("ldfld F(null);       ldnull;    ceq", Run("a7", typeof(int), X, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, FF); il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Ceq); }, new object[] { boxNull }));
        P("ldfld F(null);       ldc.i4.0;  ceq  [DAMAGED]", Run("a8", typeof(int), X, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, FF); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); }, new object[] { boxNull }));
        P("ldfld F(non-null);   ldc.i4.0;  ceq  [DAMAGED]", Run("a9", typeof(int), X, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, FF); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); }, new object[] { boxObj }));

        Console.WriteLine();
        Console.WriteLine("=== 2. ceq on a genuine INT operand: would a wrong flip be visible? ===");
        P("int 5;  ldc.i4.0;  ceq   (correct as-is)", Run("i1", typeof(int), I, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); }, new object[] { 5 }));
        P("int 0;  ldc.i4.0;  ceq   (correct as-is)", Run("i2", typeof(int), I, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); }, new object[] { 0 }));
        P("int 5;  ldnull;    ceq   [WRONG FLIP]", Run("i3", typeof(int), I, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Ceq); }, new object[] { 5 }));
        P("int 0;  ldnull;    ceq   [WRONG FLIP]", Run("i4", typeof(int), I, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Ceq); }, new object[] { 0 }));
        P("ldfld N(int 7); ldc.i4.0; ceq (correct)", Run("i5", typeof(int), X, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, FN); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); }, new object[] { boxObj }));
        P("ldfld N(int 7); ldnull;   ceq [WRONG FLIP]", Run("i6", typeof(int), X, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, FN); il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Ceq); }, new object[] { boxObj }));
        P("ldfld N(int 0); ldnull;   ceq [WRONG FLIP]", Run("i7", typeof(int), X, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, FN); il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Ceq); }, new object[] { boxNull }));

        Console.WriteLine();
        Console.WriteLine("=== 3. the constant zero as a RETURN value ===");
        P("ret(object) <- ldnull", Run("r1", typeof(object), N, il => { il.Emit(OpCodes.Ldnull); }, null));
        P("ret(object) <- ldc.i4.0  [DAMAGED]", Run("r2", typeof(object), N, il => { il.Emit(OpCodes.Ldc_I4_0); }, null));
        P("ret(int)    <- ldc.i4.0", Run("r3", typeof(int), N, il => { il.Emit(OpCodes.Ldc_I4_0); }, null));
        P("ret(int)    <- ldnull     [WRONG FLIP]", Run("r4", typeof(int), N, il => { il.Emit(OpCodes.Ldnull); }, null));

        Console.WriteLine();
        Console.WriteLine("=== 4. the constant zero as a CALL ARGUMENT ===");
        MethodInfo consumeObj = typeof(Program).GetMethod("ConsumeObj");
        MethodInfo consumeInt = typeof(Program).GetMethod("ConsumeInt");
        P("Consume(object) <- ldnull", Run("c1", typeof(int), N, il => { il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Call, consumeObj); }, null));
        P("Consume(object) <- ldc.i4.0 [DAMAGED]", Run("c2", typeof(int), N, il => { il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Call, consumeObj); }, null));
        P("Consume(int)    <- ldc.i4.0", Run("c3", typeof(int), N, il => { il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Call, consumeInt); }, null));
        P("Consume(int)    <- ldnull    [WRONG FLIP]", Run("c4", typeof(int), N, il => { il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Call, consumeInt); }, null));
        Console.WriteLine("   (ConsumeObj returns 11 when it received null, 12 otherwise;"
            + " ConsumeInt returns 21 when it received 0, 22 otherwise)");

        Console.WriteLine();
        Console.WriteLine("=== 5. stfld of the constant zero ===");
        P("stfld F(object) <- ldnull", Run("s1", typeof(void), X, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Stfld, FF); }, new object[] { boxNull }));
        P("stfld F(object) <- ldc.i4.0 [DAMAGED]", Run("s2", typeof(void), X, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Stfld, FF); }, new object[] { boxNull }));
        P("stfld N(int)    <- ldc.i4.0", Run("s3", typeof(void), X, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Stfld, FN); }, new object[] { boxNull }));
        P("stfld N(int)    <- ldnull    [WRONG FLIP]", Run("s4", typeof(void), X, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Stfld, FN); }, new object[] { boxNull }));

        Console.WriteLine();
        Console.WriteLine("=== 6. full guard shape, isolating why section 6 failed before ===");
        Console.WriteLine("   NullReferenceException ctor found: " + (nreCtor != null));
        P("g0 no local, no throw: ldnull", Run("g0a", typeof(int), O, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Ceq); }, new object[] { obj }));
        P("g1 local+stloc+ldloc:  ldnull", Run("g1a", typeof(int), O, il =>
        {
            var l = il.DeclareLocal(typeof(bool));
            il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Ceq);
            il.Emit(OpCodes.Stloc, l); il.Emit(OpCodes.Ldloc, l);
        }, new object[] { obj }));
        P("g2 +brtrue+throw:      ldnull", Run("g2a", typeof(int), O, il =>
        {
            var ok = il.DefineLabel();
            var l = il.DeclareLocal(typeof(bool));
            il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Ceq);
            il.Emit(OpCodes.Stloc, l); il.Emit(OpCodes.Ldloc, l); il.Emit(OpCodes.Brtrue, ok);
            il.Emit(OpCodes.Ldc_I4, 42); il.Emit(OpCodes.Ret);
            il.MarkLabel(ok); il.Emit(OpCodes.Ldc_I4, 99);
        }, new object[] { obj }));
        P("g3 +brtrue+newobj+throw: ldnull", Run("g3a", typeof(int), O, il =>
        {
            var ok = il.DefineLabel();
            var l = il.DeclareLocal(typeof(bool));
            il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Ceq);
            il.Emit(OpCodes.Stloc, l); il.Emit(OpCodes.Ldloc, l); il.Emit(OpCodes.Brfalse, ok);
            il.Emit(OpCodes.Newobj, nreCtor); il.Emit(OpCodes.Throw);
            il.MarkLabel(ok); il.Emit(OpCodes.Ldc_I4, 42);
        }, new object[] { obj }));
        P("g3 same, arg = null            ", Run("g3b", typeof(int), O, il =>
        {
            var ok = il.DefineLabel();
            var l = il.DeclareLocal(typeof(bool));
            il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Ceq);
            il.Emit(OpCodes.Stloc, l); il.Emit(OpCodes.Ldloc, l); il.Emit(OpCodes.Brfalse, ok);
            il.Emit(OpCodes.Newobj, nreCtor); il.Emit(OpCodes.Throw);
            il.MarkLabel(ok); il.Emit(OpCodes.Ldc_I4, 42);
        }, new object[] { null }));
        P("g4 identical but ldc.i4.0 [DAMAGED], arg=obj", Run("g4a", typeof(int), O, il =>
        {
            var ok = il.DefineLabel();
            var l = il.DeclareLocal(typeof(bool));
            il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq);
            il.Emit(OpCodes.Stloc, l); il.Emit(OpCodes.Ldloc, l); il.Emit(OpCodes.Brfalse, ok);
            il.Emit(OpCodes.Newobj, nreCtor); il.Emit(OpCodes.Throw);
            il.MarkLabel(ok); il.Emit(OpCodes.Ldc_I4, 42);
        }, new object[] { obj }));
        P("g4 identical but ldc.i4.0 [DAMAGED], arg=null", Run("g4b", typeof(int), O, il =>
        {
            var ok = il.DefineLabel();
            var l = il.DeclareLocal(typeof(bool));
            il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq);
            il.Emit(OpCodes.Stloc, l); il.Emit(OpCodes.Ldloc, l); il.Emit(OpCodes.Brfalse, ok);
            il.Emit(OpCodes.Newobj, nreCtor); il.Emit(OpCodes.Throw);
            il.MarkLabel(ok); il.Emit(OpCodes.Ldc_I4, 42);
        }, new object[] { null }));
    }

    public static int ConsumeObj(object p) { return p == null ? 11 : 12; }
    public static int ConsumeInt(int p) { return p == 0 ? 21 : 22; }
}
CSEOF
cd "$B/cal" || exit 1
dotnet run -c Release 2>&1 | grep -v "warning CS"
echo "=================================================================="
echo "=== C++ layer: what IL2CPP emits for the two forms, compiled+run ==="
echo "=================================================================="
mkdir -p "$B/cpp"
cat > "$B/cpp/harness.cpp" <<'CPPEOF'
#include <cstdio>
#include <cstdint>
struct Il2CppObject { int x; };

/* REPAIRED: ldnull -> nullptr_t.  This is what il2cpp emits after the fix. */
static int repaired_check(Il2CppObject* p) { return (p == nullptr) ? 1 : 0; }
/* The exact statement il2cpp emits from `ldc.i4 0` in this position: */
static int repaired_check_int(int v)      { return (v == 0) ? 1 : 0; }

int main() {
    Il2CppObject o; o.x = 7;
    Il2CppObject* nonnull = &o;
    Il2CppObject* isnull  = nullptr;
    printf("repaired(p=non-null) -> %d   (expect 0)\n", repaired_check(nonnull));
    printf("repaired(p=null)     -> %d   (expect 1)\n", repaired_check(isnull));
    printf("int-form(v=7)        -> %d   (expect 0)\n", repaired_check_int(7));
    printf("int-form(v=0)        -> %d   (expect 1)\n", repaired_check_int(0));
    return 0;
}
CPPEOF
cat > "$B/cpp/damaged.cpp" <<'CPPEOF'
#include <cstdint>
struct Il2CppObject { int x; };
/* DAMAGED: il2cpp turns `ldc.i4 0` into a *cast* integer zero, which is NOT a
   null pointer constant in C++11 and later.  This is the diagnostic we counted. */
static int damaged_check(Il2CppObject* p) { return (p == (int32_t)0) ? 1 : 0; }
int main() { return damaged_check(nullptr); }
CPPEOF
cd "$B/cpp" || exit 1
echo "--- clang++ harness.cpp (REPAIRED form): compile + RUN ---"
clang++ -std=c++11 -O0 -o harness harness.cpp 2>&1 && ./harness
echo "--- clang++ damaged.cpp (DAMAGED form): expect a compile ERROR ---"
clang++ -std=c++11 -fsyntax-only damaged.cpp 2>&1 | head -6
echo "damaged compile rc=$?"
