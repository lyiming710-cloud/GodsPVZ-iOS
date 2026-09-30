set -u
B=/workspaces/GodsPVZ-native19/.validation/native22
S=/workspaces/GodsPVZ-native19/scripts/codespaces
cd "$B" || exit 1

echo "################ concrete execution, wider world set ################"
date +%s
python3 "$S/native23_behave.py" 2>&1 | tail -60
date +%s

echo
echo "################ C++ counterexample for the ASSIGNMENT-shaped sites ################"
mkdir -p "$B/behave/cpp"
cat > "$B/behave/cpp/assign.cpp" <<'CPPEOF'
#include <cstdint>
struct Il2CppObject { int x; };
/* The 49 quarantine sites of the `I4 is not assignable to X` family store the
   constant into a reference slot.  Correct: */
static void correct_assign(Il2CppObject** out) { *out = nullptr; }
/* Damaged, i.e. what il2cpp emits from `ldc.i4 0`: */
static void damaged_assign(Il2CppObject** out) { *out = (int32_t)0; }
void run(Il2CppObject** p) { correct_assign(p); damaged_assign(p); }
CPPEOF
cd "$B/behave/cpp" || exit 1
echo "--- expect: correct_assign OK, damaged_assign REJECTED ---"
clang++ -std=c++11 -fsyntax-only assign.cpp 2>&1 | head -10
echo "---"
