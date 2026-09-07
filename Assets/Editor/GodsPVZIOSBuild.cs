#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class GodsPVZIOSBuild
{
    [MenuItem("GodsPVZ/Build/Export unsigned iOS Xcode project")]
    public static void BuildIOS()
    {
        PlayerSettings.companyName = "TipsGodsStudio";
        PlayerSettings.productName = "GodsPVZ";
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, "com.tipsGodsStudio.godsPVZ");
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
        PlayerSettings.iOS.targetDevice = iOSTargetDevice.iPhoneAndiPad;
        PlayerSettings.iOS.targetOSVersionString = "12.0";
        PlayerSettings.SetArchitecture(BuildTargetGroup.iOS, 1);
        EditorUserBuildSettings.symlinkLibraries = false;

        var scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
        if (scenes.Length == 0) throw new InvalidOperationException("No enabled scenes in EditorBuildSettings.");
        Directory.CreateDirectory("Build");
        var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions {
            scenes = scenes,
            locationPathName = "Build/iOS",
            target = BuildTarget.iOS,
            targetGroup = BuildTargetGroup.iOS,
            options = BuildOptions.None
        });
        if (report.summary.result != BuildResult.Succeeded)
            throw new Exception($"iOS Unity build failed: {report.summary.result}, errors={report.summary.totalErrors}");
        Debug.Log($"GodsPVZ iOS Xcode export succeeded: {report.summary.outputPath}");
    }
}
#endif
