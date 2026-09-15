using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

// Dormant coverage-expansion harness. It is intentionally not wired to a workflow.
// Use only after the strict Board blocker stream is clean and a targeted
// Plant_DetaiPage.Update_SkillProgress coverage run is explicitly warranted.
public class Stage9SkillProgressRuntime
{
    static readonly BindingFlags F = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

    static object RF(Type t, object o, string n)
    {
        var f = t.GetField(n, F);
        return f == null ? null : f.GetValue(o);
    }

    static void SF(Type t, object o, string n, object v)
    {
        var f = t.GetField(n, F);
        Assert.IsNotNull(f, "field " + n);
        f.SetValue(o, v);
    }

    static Component FSC(Type t, string scene)
    {
        if (t == null) return null;
        foreach (var o in Resources.FindObjectsOfTypeAll(t))
        {
            var c = o as Component;
            if (c != null && c.gameObject.scene.IsValid() && c.gameObject.scene.name == scene) return c;
        }
        return null;
    }

    static int CountList(object o)
    {
        var c = o as ICollection;
        return c == null ? -1 : c.Count;
    }

    [UnityTest]
    public IEnumerator Gate()
    {
        LogAssert.ignoreFailingMessages = true;

        var op = SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
        while (!op.isDone) yield return null;
        yield return new WaitForSecondsRealtime(2f);
        for (int i = 0; i < 30; i++) yield return null;

        var rt = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == "GodsPVZRuntime1");
        Assert.IsNotNull(rt);
        var gs = rt.GetType("GlobalStaticVars");
        var lawn = RF(gs, null, "gLawnApp");
        var saves = RF(lawn.GetType(), lawn, "savesManager");
        var p = RF(saves.GetType(), saves, "playerSave");
        if (p == null)
        {
            var m = saves.GetType().GetMethod("CreateNewPlayerSave", F, null, new[] { typeof(string) }, null);
            Assert.IsNotNull(m);
            try { m.Invoke(saves, new object[] { "Stage9SkillProgress" }); }
            catch (TargetInvocationException ex) { throw ex.InnerException ?? ex; }
            for (int i = 0; i < 10; i++) yield return null;
        }
        Debug.Log("STAGE9_SKILLPROGRESS_SAVE_READY ok=1");

        op = SceneManager.LoadSceneAsync("Board", LoadSceneMode.Single);
        while (!op.isDone) yield return null;
        yield return new WaitForSecondsRealtime(2f);
        for (int i = 0; i < 30; i++) yield return null;

        var prepare = FSC(rt.GetType("PrepareUIController"), "Board");
        Assert.IsNotNull(prepare);
        var gameStart = prepare.GetType().GetMethod("GameStart", F, null, Type.EmptyTypes, null);
        Assert.IsNotNull(gameStart);
        try { gameStart.Invoke(prepare, null); }
        catch (TargetInvocationException ex) { throw ex.InnerException ?? ex; }
        yield return new WaitForSecondsRealtime(2f);
        for (int i = 0; i < 30; i++) yield return null;
        Debug.Log("STAGE9_SKILLPROGRESS_BOARD_READY ok=1");

        var detailT = rt.GetType("Plant_DetaiPage");
        var skillT = rt.GetType("Skill");
        var plantT = rt.GetType("Plant");
        var detail = FSC(detailT, "Board");
        Assert.IsNotNull(detail);
        Assert.IsNotNull(skillT);
        Assert.IsNotNull(plantT);

        var logos = RF(detailT, detail, "skillLogo");
        var texts = RF(detailT, detail, "skillChargeTexts");
        var layerText = RF(detailT, detail, "text_chargeLayer");
        int logoCount = CountList(logos);
        int textCount = texts is Array a ? a.Length : CountList(texts);
        Debug.Log($"STAGE9_SKILLPROGRESS_UI detail=1 logos={logoCount} texts={textCount} layerText={(layerText != null ? 1 : 0)}");
        Assert.GreaterOrEqual(logoCount, 4);
        Assert.GreaterOrEqual(textCount, 3);
        Assert.IsNotNull(layerText);

        var skill = Activator.CreateInstance(skillT);
        Assert.IsNotNull(skill);
        var chargeTypeField = skillT.GetField("ChargeType", F);
        Assert.IsNotNull(chargeTypeField);

        object plant = null;
        string plantSource = "none";
        GameObject tempPlantGo = null;
        foreach (var o in Resources.FindObjectsOfTypeAll(plantT))
        {
            var c = o as Component;
            if (c != null && c.gameObject.scene.IsValid() && c.gameObject.scene.name == "Board")
            {
                plant = o;
                plantSource = "scene";
                break;
            }
        }
        if (plant == null)
        {
            // Plant derives from MonoBehaviour. A FormatterServices shell has no native
            // UnityEngine.Object backing and is not a valid runtime probe target.
            // Create a real inactive component instead; the GameObject never becomes
            // active, so gameplay Update does not run while the two plain fields below
            // are exercised by Update_SkillProgress.
            tempPlantGo = new GameObject("Stage9SkillProgressPlant");
            tempPlantGo.SetActive(false);
            plant = tempPlantGo.AddComponent(plantT);
            plantSource = "inactive_component";
        }
        Assert.IsNotNull(plant);

        var oldPSkill = RF(detailT, detail, "p_skill");
        var oldPlant = RF(detailT, detail, "plant");
        var oldSkillOngoing = RF(plantT, plant, "skillOngoing");
        var oldSkillRemainTime = RF(plantT, plant, "skillRemainTime");
        Debug.Log("STAGE9_SKILLPROGRESS_PLANT_SOURCE source=" + plantSource);
        SF(detailT, detail, "p_skill", skill);
        SF(detailT, detail, "plant", plant);

        var target = detailT.GetMethod("Update_SkillProgress", F, null, Type.EmptyTypes, null);
        Assert.IsNotNull(target);

        void InvokePhase(string phase)
        {
            try
            {
                target.Invoke(detail, null);
                Debug.Log("STAGE9_SKILLPROGRESS_PHASE phase=" + phase + " ok=1");
            }
            catch (TargetInvocationException ex)
            {
                Debug.LogError("STAGE9_SKILLPROGRESS_PHASE phase=" + phase + " ok=0 inner=" + (ex.InnerException ?? ex));
                throw ex.InnerException ?? ex;
            }
        }

        try
        {
            SF(skillT, skill, "chargedLayer", 1);
            SF(plantT, plant, "skillOngoing", false);
            try
            {
                target.Invoke(detail, null);
                Debug.Log("STAGE9_SKILLPROGRESS_READY_INVOKE ok=1");
                Debug.Log("STAGE9_SKILLPROGRESS_PHASE phase=ready ok=1");
            }
            catch (TargetInvocationException ex)
            {
                Debug.LogError("STAGE9_SKILLPROGRESS_READY_INVOKE ok=0 inner=" + (ex.InnerException ?? ex));
                Debug.LogError("STAGE9_SKILLPROGRESS_PHASE phase=ready ok=0 inner=" + (ex.InnerException ?? ex));
                throw ex.InnerException ?? ex;
            }

            SF(skillT, skill, "chargedLayer", 0);
            chargeTypeField.SetValue(skill, Enum.ToObject(chargeTypeField.FieldType, 0));
            SF(skillT, skill, "chargeTicking", 0.25f);
            SF(skillT, skill, "maxChargeTicking", 0.5f);
            SF(plantT, plant, "skillOngoing", false);
            InvokePhase("active_charge");

            chargeTypeField.SetValue(skill, Enum.ToObject(chargeTypeField.FieldType, 3));
            SF(skillT, skill, "chargedLayer", 0);
            SF(plantT, plant, "skillOngoing", false);
            InvokePhase("passive");

            SF(skillT, skill, "chargedLayer", 2);
            SF(skillT, skill, "duration", 0.5f);
            SF(plantT, plant, "skillOngoing", true);
            SF(plantT, plant, "skillRemainTime", 0.25f);
            InvokePhase("ongoing_high");

            SF(skillT, skill, "chargedLayer", 1);
            SF(plantT, plant, "skillOngoing", true);
            SF(plantT, plant, "skillRemainTime", 0.25f);
            InvokePhase("ongoing_low");

            Debug.Log("STAGE9_SKILLPROGRESS_ALL_PHASES_PASS phases=5");
        }
        finally
        {
            SF(detailT, detail, "p_skill", oldPSkill);
            SF(detailT, detail, "plant", oldPlant);
            if (plant != null)
            {
                SF(plantT, plant, "skillOngoing", oldSkillOngoing);
                SF(plantT, plant, "skillRemainTime", oldSkillRemainTime);
            }
            if (tempPlantGo != null) UnityEngine.Object.DestroyImmediate(tempPlantGo);
        }

        yield return null;
    }
}
