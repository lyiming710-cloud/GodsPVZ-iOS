#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class GodsPVZIOSBuild
{
    private static readonly string[] LockedScenes = {
        "Assets/Scenes/MainMenu.unity",
        "Assets/Scenes/Board.unity"
    };

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

        foreach (var scene in LockedScenes)
            if (!File.Exists(scene))
                throw new FileNotFoundException("Locked Stage9.1 scene is missing", scene);

        // Stage9.1 export must not depend on whatever EditorBuildSettings happened
        // to be serialized by AssetRipper. Keep the already validated scene pair exact.
        EditorBuildSettings.scenes = LockedScenes
            .Select(path => new EditorBuildSettingsScene(path, true))
            .ToArray();

        var output = Environment.GetEnvironmentVariable("GODSPVZ_IOS_BUILD_PATH");
        if (String.IsNullOrWhiteSpace(output)) output = "Build/iOS";
        var parent = Path.GetDirectoryName(output);
        if (!String.IsNullOrEmpty(parent)) Directory.CreateDirectory(parent);

        var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions {
            scenes = LockedScenes,
            locationPathName = output,
            target = BuildTarget.iOS,
            targetGroup = BuildTargetGroup.iOS,
            options = BuildOptions.None
        });

        Debug.Log($"GodsPVZ iOS export summary: result={report.summary.result}, errors={report.summary.totalErrors}, warnings={report.summary.totalWarnings}, size={report.summary.totalSize}, output={report.summary.outputPath}");
        if (report.summary.result != BuildResult.Succeeded)
            throw new Exception($"iOS Unity build failed: {report.summary.result}, errors={report.summary.totalErrors}");

        if (!Directory.Exists(output))
            throw new DirectoryNotFoundException("Unity reported success but Xcode export directory is missing: " + output);

        Debug.Log("GodsPVZ iOS Xcode export succeeded: " + report.summary.outputPath);
    }
}
#endif
