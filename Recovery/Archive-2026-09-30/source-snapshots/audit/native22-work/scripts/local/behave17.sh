set -u
export DOTNET_CLI_TELEMETRY_OPTOUT=1
B=/workspaces/GodsPVZ-native19/.validation/native22
S=/workspaces/GodsPVZ-native19/scripts/codespaces
cd "$B" || exit 1

echo "################ 1. site shape census ################"
python3 "$S/native23_shapes.py" 2>&1 | tail -50

echo
echo "################ 2. concrete execution of the repaired methods ################"
python3 "$S/native23_behave.py" 2>&1 | tail -60

echo
echo "################ 3. C++ counterexample: what a WRONG flip looks like ################"
mkdir -p "$B/behave/cpp"
cat > "$B/behave/cpp/wrong.cpp" <<'CPPEOF'
#include <cstdint>
struct Il2CppObject { int x; };
/* A CORRECT flip: the other operand is a reference, so nullptr is right. */
static int correct(Il2CppObject* p) { return (p == nullptr) ? 1 : 0; }
/* A WRONG flip: the other operand is a genuine integer, so nullptr is wrong.
   If the rule were bad, this is what il2cpp would emit. */
static int wrong_int(int v)          { return (v == nullptr) ? 1 : 0; }
int main() { return correct(nullptr) + wrong_int(0); }
CPPEOF
cd "$B/behave/cpp" || exit 1
echo "--- compile the counterexample (expect: 'correct' OK, 'wrong_int' REJECTED) ---"
clang++ -std=c++11 -fsyntax-only wrong.cpp 2>&1 | head -8
echo "---"

echo
echo "################ 4. counterexamples at the IL level (rule must refuse) ################"
cat > "$B/behave/counter.il.txt" <<'ILEOF'
The rule must REFUSE all of these.  Each is a shape that a sloppy
"replace every zero literal next to a comparison" rule would corrupt.

 C1  int field:      ldfld  N:System.Int32 ; ldc.i4.0 ; ceq
                     -> genuine integer zero test.  Flipping gives `N == null`.
                        Real CLR (measured): ceq cannot tell the difference,
                        so NOTHING at IL runtime exposes this mistake.
                        C++ layer (measured): `int == nullptr` does not compile.
 C2  int argument:   ldarg  n:System.Int32 ; ldc.i4.0 ; ceq            (same)
 C3  int local:      ldloc  i:System.Int32 ; ldc.i4.0 ; ceq            (same)
 C4  bool/enum:      ldfld  flag:System.Boolean ; ldc.i4.0 ; ceq       (same)
 C5  genuine int 0 as a call argument:
                     ldarg.0 ; ldc.i4.0 ; call Foo(System.Int32)
                     -> Real CLR: `Consume(int) <- ldnull` throws
                        InvalidProgramException (MEASURED).  Observable.
 C6  genuine int 0 returned from an int method:
                     ldc.i4.0 ; ret          (ret type System.Int32)
                     -> flipping gives `ldnull; ret` in an int method.
 C7  non-zero literal:  ldfld p ; ldc.i4 3 ; ceq
                     -> rule's `is_zero_lit` refuses it (value != 0).
 C8  reference compared with a reference:  ldfld p ; ldnull ; ceq
                     -> already correct, nothing to flip.
 C9  zero used in arithmetic:  ldfld n:System.Int32 ; ldc.i4.0 ; add
                     -> no clash, so it is never a candidate.
ILEOF
cat "$B/behave/counter.il.txt"
