set -x
export DOTNET_CLI_TELEMETRY_OPTOUT=1 DOTNET_NOLOGO=1 DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
B=/workspaces/GodsPVZ-native19/.validation/native22/behave
rm -rf "$B"
mkdir -p "$B"
cd "$B" || exit 1
dotnet new console -o cal --force 2>&1 | tail -5
cat > "$B/cal/Program.cs" <<'CSEOF'
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

public class Box { public object F; public string S; public int N; }

public delegate int D0();
public delegate int D1(object a);
public delegate int DI(int a);
public delegate int DX(Box a);
public delegate int DS(string a);
public delegate int DB(StringBuilder a);
public delegate void V1(Box a);

public static class Program
{
    static string Result(string name, Type[] sig, Action<ILGenerator> body, object[] vals)
    {
        try
        {
            var dm = new DynamicMethod(name, typeof(int), sig, typeof(Program).Module, true);
            var il = dm.GetILGenerator();
            body(il);
            il.Emit(OpCodes.Ret);
            object r = dm.Invoke(null, vals);
            return "VALUE " + r;
        }
        catch (TargetInvocationException tie)
        {
            return "THROW " + (tie.InnerException == null ? "?" : tie.InnerException.GetType().FullName);
        }
        catch (Exception e) { return "THROW " + e.GetType().FullName; }
    }

    static string ResultV(string name, Type[] sig, Action<ILGenerator> body, object[] vals)
    {
        try
        {
            var dm = new DynamicMethod(name, typeof(void), sig, typeof(Program).Module, true);
            var il = dm.GetILGenerator();
            body(il);
            il.Emit(OpCodes.Ret);
            dm.Invoke(null, vals);
            return "VALUE <void>";
        }
        catch (TargetInvocationException tie)
        {
            return "THROW " + (tie.InnerException == null ? "?" : tie.InnerException.GetType().FullName);
        }
        catch (Exception e) { return "THROW " + e.GetType().FullName; }
    }

    public static void Main()
    {
        Console.WriteLine("runtime: " + Environment.Version + "  " + System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription);
        Type[] o = new Type[] { typeof(object) };
        Type[] i = new Type[] { typeof(int) };
        Type[] x = new Type[] { typeof(Box) };
        Type[] s = new Type[] { typeof(string) };
        Type[] b = new Type[] { typeof(StringBuilder) };
        Type[] n = new Type[0];
        FieldInfo FF = typeof(Box).GetField("F");
        FieldInfo FN = typeof(Box).GetField("N");
        object obj = new object();
        var sb = new StringBuilder();
        var boxNull = new Box { F = null, N = 0 };
        var boxObj = new Box { F = new object(), N = 7 };

        void P(string tag, string r) => Console.WriteLine("  {0,-46} {1}", tag, r);

        Console.WriteLine();
        Console.WriteLine("=== 1. reference vs null constant (ceq) ===");
        P("ldnull; ldnull; ceq", Result("a1", n, il => { il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Ceq); }, null));
        P("ldnull; ldc.i4.0; ceq", Result("a2", n, il => { il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); }, null));
        P("obj(non-null); ldnull; ceq", Result("a3", o, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Ceq); }, new object[] { obj }));
        P("obj(non-null); ldc.i4.0; ceq   [DAMAGED]", Result("a4", o, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); }, new object[] { obj }));
        P("obj(null); ldnull; ceq", Result("a5", o, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Ceq); }, new object[] { null }));
        P("obj(null); ldc.i4.0; ceq       [DAMAGED]", Result("a6", o, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); }, new object[] { null }));
        P("StringBuilder(non-null); ldc.i4.0; ceq [DAMAGED]", Result("a7", b, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); }, new object[] { sb }));
        P("StringBuilder(null); ldc.i4.0; ceq     [DAMAGED]", Result("a8", b, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); }, new object[] { null }));

        Console.WriteLine();
        Console.WriteLine("=== 2. genuine integer comparison (must NOT be flipped) ===");
        P("int 5; ldc.i4.0; ceq", Result("i1", i, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); }, new object[] { 5 }));
        P("int 0; ldc.i4.0; ceq", Result("i2", i, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); }, new object[] { 0 }));
        P("int 5; ldnull; ceq     [WRONG FLIP]", Result("i3", i, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Ceq); }, new object[] { 5 }));
        P("int 0; ldnull; ceq     [WRONG FLIP]", Result("i4", i, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Ceq); }, new object[] { 0 }));

        Console.WriteLine();
        Console.WriteLine("=== 3. ldfld of a reference field (the game shape) ===");
        P("Box.F=null; ldfld F; ldnull; ceq", Result("f1", x, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, FF); il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Ceq); }, new object[] { boxNull }));
        P("Box.F=null; ldfld F; ldc.i4.0; ceq [DAMAGED]", Result("f2", x, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, FF); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); }, new object[] { boxNull }));
        P("Box.F=obj;  ldfld F; ldnull; ceq", Result("f3", x, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, FF); il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Ceq); }, new object[] { boxObj }));
        P("Box.F=obj;  ldfld F; ldc.i4.0; ceq [DAMAGED]", Result("f4", x, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, FF); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); }, new object[] { boxObj }));

        Console.WriteLine();
        Console.WriteLine("=== 4. ldfld of an int field (wrong-flip target) ===");
        P("Box.N=0; ldfld N; ldc.i4.0; ceq", Result("n1", x, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, FN); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); }, new object[] { boxNull }));
        P("Box.N=7; ldfld N; ldc.i4.0; ceq", Result("n2", x, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, FN); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Ceq); }, new object[] { boxObj }));
        P("Box.N=0; ldfld N; ldnull; ceq   [WRONG FLIP]", Result("n3", x, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, FN); il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Ceq); }, new object[] { boxNull }));
        P("Box.N=7; ldfld N; ldnull; ceq   [WRONG FLIP]", Result("n4", x, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldfld, FN); il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Ceq); }, new object[] { boxObj }));

        Console.WriteLine();
        Console.WriteLine("=== 5. stfld: storing the constant zero into a field ===");
        P("stfld F(object) <- ldnull", ResultV("s1", x, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Stfld, FF); }, new object[] { boxNull }));
        P("stfld F(object) <- ldc.i4.0  [DAMAGED]", ResultV("s2", x, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Stfld, FF); }, new object[] { boxNull }));
        P("stfld N(int)    <- ldc.i4.0", ResultV("s3", x, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldc_I4_0); il.Emit(OpCodes.Stfld, FN); }, new object[] { boxNull }));
        P("stfld N(int)    <- ldnull     [WRONG FLIP]", ResultV("s4", x, il => { il.Emit(OpCodes.Ldarg_0); il.Emit(OpCodes.Ldnull); il.Emit(OpCodes.Stfld, FN); }, new object[] { boxNull }));

        Console.WriteLine();
        Console.WriteLine("=== 6. the full game shape: ceq -> stloc -> brtrue -> throw ===");
        // bool b = (p == null); if (b) throw new NullReferenceException(); return 42;
        Func<OpCode, Action<ILGenerator>> mk = cmp => il =>
        {
            var ok = il.DefineLabel();
            var loc = il.DeclareLocal(typeof(bool));
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(cmp);
            il.Emit(OpCodes.Stloc, loc);
            il.Emit(OpCodes.Ldloc, loc);
            il.Emit(OpCodes.Brfalse, ok);
            ConstructorInfo ci = typeof(NullReferenceException).GetConstructor(new Type[0]);
            il.Emit(OpCodes.Newobj, ci);
            il.Emit(OpCodes.Throw);
            il.MarkLabel(ok);
            il.Emit(OpCodes.Ldc_I4, 42);
        };
        P("guard ldnull,  arg=non-null", Result("g1", o, mk(OpCodes.Ldnull), new object[] { obj }));
        P("guard ldc.i4.0, arg=non-null [DAMAGED]", Result("g2", o, mk(OpCodes.Ldc_I4_0), new object[] { obj }));
        P("guard ldnull,  arg=null", Result("g3", o, mk(OpCodes.Ldnull), new object[] { null }));
        P("guard ldc.i4.0, arg=null      [DAMAGED]", Result("g4", o, mk(OpCodes.Ldc_I4_0), new object[] { null }));
    }
}
CSEOF
cd "$B/cal" || exit 1
dotnet run -c Release 2>&1 | tail -60
