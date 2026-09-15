using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

// Dormant targeted coverage/qualification harness.
// Do not wire this to a workflow until the current runtime-qualified base is fixed
// and Zombie::.ctor is explicitly selected for observation or qualification.
public class Stage9ZombieCtorRuntime
{
    static readonly BindingFlags F = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

    static object RF(Type t, object o, string n)
    {
        var f = t.GetField(n, F);
        Assert.IsNotNull(f, "field " + n);
        return f.GetValue(o);
    }

    static int Count(object o)
    {
        var c = o as ICollection;
        return c == null ? -1 : c.Count;
    }

    static bool Near(float a, float b) => Mathf.Abs(a - b) < 0.0001f;
    static bool Zero(Vector3 v) => Near(v.x, 0f) && Near(v.y, 0f) && Near(v.z, 0f);
    static bool White(Color c) => Near(c.r, 1f) && Near(c.g, 1f) && Near(c.b, 1f) && Near(c.a, 1f);

    [UnityTest]
    public IEnumerator Gate()
    {
        LogAssert.ignoreFailingMessages = true;
        var rt = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == "GodsPVZRuntime1");
        var zt = rt?.GetType("Zombie");
        Debug.Log($"STAGE9_ZOMBIE_CTOR_INPUT runtime={(rt != null ? 1 : 0)} zombieType={(zt != null ? 1 : 0)}");
        Assert.IsNotNull(rt);
        Assert.IsNotNull(zt);

        var go = new GameObject("Stage9ZombieCtorProbe");
        go.SetActive(false);
        Component z = null;
        try
        {
            try
            {
                z = go.AddComponent(zt);
                Debug.Log("STAGE9_ZOMBIE_CTOR_CREATE ok=1");
            }
            catch (Exception ex)
            {
                var inner = ex is TargetInvocationException tie ? (tie.InnerException ?? tie) : ex;
                Debug.LogError("STAGE9_ZOMBIE_CTOR_CREATE ok=0 inner=" + inner);
                throw inner;
            }

            Assert.IsNotNull(z);
            float attackPoint = Convert.ToSingle(RF(zt, z, "attackPoint"));
            bool isStant = Convert.ToBoolean(RF(zt, z, "isStant"));
            bool isOnBoard = Convert.ToBoolean(RF(zt, z, "isOnBoard"));
            int camp = Convert.ToInt32(RF(zt, z, "camp"));
            float waitingTime = Convert.ToSingle(RF(zt, z, "waitingTime"));
            float updateRate = Convert.ToSingle(RF(zt, z, "updateRate"));
            float bright = Convert.ToSingle(RF(zt, z, "brightIntensity"));
            float brightArmor2 = Convert.ToSingle(RF(zt, z, "brightIntensity_Armor2"));
            var rSpeed = (Vector3)RF(zt, z, "rSpeed");
            var rDirection = (Vector3)RF(zt, z, "rDirection");
            var previousPosition = (Vector3)RF(zt, z, "previousPosition");
            var color = (Color)RF(zt, z, "color");
            var prePath = RF(zt, z, "prePath");
            var path = RF(zt, z, "path");
            var animationSprites = RF(zt, z, "animationSprites");
            var elementUIControllers = RF(zt, z, "elementUIControllers");
            var elementManager = RF(zt, z, "elementManager");
            var hpUIController = RF(zt, z, "hpUIController");
            var buffManager = RF(zt, z, "buffManager");

            Debug.Log(
                $"STAGE9_ZOMBIE_CTOR_STATE attack={attackPoint} stant={(isStant?1:0)} onBoard={(isOnBoard?1:0)} camp={camp} " +
                $"waiting={waitingTime} updateRate={updateRate} bright={bright} brightArmor2={brightArmor2} " +
                $"zeroSpeed={(Zero(rSpeed)?1:0)} zeroDirection={(Zero(rDirection)?1:0)} zeroPrevious={(Zero(previousPosition)?1:0)} white={(White(color)?1:0)} " +
                $"prePath={(prePath!=null?1:0)}:{Count(prePath)} path={(path!=null?1:0)}:{Count(path)} animation={(animationSprites!=null?1:0)}:{Count(animationSprites)} elements={(elementUIControllers!=null?1:0)}:{Count(elementUIControllers)} " +
                $"elementManager={(elementManager!=null?1:0)} hpUI={(hpUIController!=null?1:0)} buff={(buffManager!=null?1:0)}");

            Assert.IsTrue(Near(attackPoint, 100f));
            Assert.IsTrue(isStant);
            Assert.IsTrue(isOnBoard);
            Assert.AreEqual(2, camp);
            Assert.IsTrue(Near(waitingTime, 3f));
            Assert.IsTrue(Near(updateRate, 1f));
            Assert.IsTrue(Near(bright, 1f));
            Assert.IsTrue(Near(brightArmor2, 1f));
            Assert.IsTrue(Zero(rSpeed));
            Assert.IsTrue(Zero(rDirection));
            Assert.IsTrue(Zero(previousPosition));
            Assert.IsTrue(White(color));
            Assert.AreEqual(0, Count(prePath));
            Assert.AreEqual(0, Count(path));
            Assert.AreEqual(0, Count(animationSprites));
            Assert.AreEqual(0, Count(elementUIControllers));
            Assert.IsNotNull(elementManager);
            Assert.IsNotNull(hpUIController);
            Assert.IsNotNull(buffManager);
            Debug.Log("STAGE9_ZOMBIE_CTOR_VERIFY pass=1");
        }
        finally
        {
            UnityEngine.Object.DestroyImmediate(go);
        }
        yield return null;
    }
}
