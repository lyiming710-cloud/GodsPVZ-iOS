using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class Stage9StrictBoardRuntime
{
    static readonly BindingFlags F = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
    static object RF(Type t, object o, string n) { var f=t.GetField(n,F); return f==null?null:f.GetValue(o); }
    static int CW(Type t, out int total)
    {
        total=0; if(t==null) return 0; int a=0;
        foreach(var o in Resources.FindObjectsOfTypeAll(t))
        {
            total++; var c=o as Component;
            if(c!=null)
            {
                bool x=c.gameObject.scene.IsValid()&&c.gameObject.activeInHierarchy;
                if(x)a++;
                Debug.Log($"STAGE9_STRICT_WINDOW name={c.gameObject.name} active={(x?1:0)} scene={c.gameObject.scene.name}");
            }
        }
        return a;
    }
    static void Walk(GameObject g, ref int gos, ref int missing, ref int bs, ref int b)
    {
        gos++;
        foreach(var c in g.GetComponents<Component>())
        {
            if(c==null){missing++;continue;}
            if(c.GetType().Name=="BoardStart")bs++;
            if(c.GetType().Name=="Board")b++;
        }
        foreach(Transform ch in g.transform)Walk(ch.gameObject,ref gos,ref missing,ref bs,ref b);
    }

    [UnityTest]
    public IEnumerator Gate()
    {
        LogAssert.ignoreFailingMessages=true;
        var op=SceneManager.LoadSceneAsync("MainMenu",LoadSceneMode.Single);
        while(!op.isDone)yield return null;
        yield return new WaitForSecondsRealtime(2f);
        for(int i=0;i<30;i++)yield return null;

        var rt=AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a=>a.GetName().Name=="GodsPVZRuntime1");
        var gs=rt?.GetType("GlobalStaticVars");
        var lawn=gs==null?null:RF(gs,null,"gLawnApp");
        var saves=lawn==null?null:RF(lawn.GetType(),lawn,"savesManager");
        var sl=saves==null?null:RF(saves.GetType(),saves,"saveList");
        var p=saves==null?null:RF(saves.GetType(),saves,"playerSave");
        int total; int active=CW(rt?.GetType("Window_I"),out total);
        Debug.Log($"STAGE9_STRICT_MAINMENU scene={SceneManager.GetActiveScene().name} runtime={(rt!=null?1:0)} lawn={(lawn!=null?1:0)} saves={(saves!=null?1:0)} saveList={(sl!=null?1:0)} player={(p!=null?1:0)} windowI={total} activeWindowI={active}");
        Assert.IsNotNull(saves); Assert.IsNotNull(sl);

        if(p==null)
        {
            Assert.Greater(active,0); Debug.Log("STAGE9_STRICT_BRANCH no-save-popup");
            var m=saves.GetType().GetMethod("CreateNewPlayerSave",F,null,new[]{typeof(string)},null);
            try { m.Invoke(saves,new object[]{"Stage9Test"}); Debug.Log("STAGE9_STRICT_CREATE_INVOKE ok=1"); }
            catch(TargetInvocationException ex) { Debug.LogError("STAGE9_STRICT_CREATE_INVOKE ok=0 inner="+(ex.InnerException??ex)); throw ex.InnerException??ex; }
            for(int i=0;i<10;i++)yield return null;
            p=RF(saves.GetType(),saves,"playerSave");
        }

        sl=RF(saves.GetType(),saves,"saveList");
        var pn=p==null?null:RF(p.GetType(),p,"playerName");
        var dn=sl==null?null:RF(sl.GetType(),sl,"defaultPlayerName");
        Debug.Log($"STAGE9_STRICT_SAVE_READY player={(p!=null?1:0)} playerName={(pn??"<null>")} saveList={(sl!=null?1:0)} defaultName={(dn??"<null>")}");
        Assert.IsNotNull(p); Assert.AreEqual("Stage9Test",Convert.ToString(pn)); Assert.AreEqual("Stage9Test",Convert.ToString(dn));

        op=SceneManager.LoadSceneAsync("Board",LoadSceneMode.Single);
        while(!op.isDone)yield return null;
        yield return new WaitForSecondsRealtime(2f);
        for(int i=0;i<30;i++)yield return null;
        var s=SceneManager.GetActiveScene(); int gos=0,missing=0,bs=0,b=0;
        foreach(var r in s.GetRootGameObjects())Walk(r,ref gos,ref missing,ref bs,ref b);
        Debug.Log($"STAGE9_STRICT_BOARD scene={s.name} gos={gos} missing={missing} boardStart={bs} board={b}");
        Assert.AreEqual("Board",s.name); Assert.Greater(bs,0); Assert.Greater(b,0);
    }
}
