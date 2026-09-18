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
        // Do not let AssetRipper-restored ProjectSettings or the editor's startup
        // platform leak an obsolete ARMv7 selection into the iOS post-processor.
        // The exact 2022.3.44f1c1 iOS module was independently validated before
        // this gate, so an inability to activate iOS here is a hard failure.
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS)
        {
            var switched = EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
            if (!switched || EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS)
                throw new InvalidOperationException("Unable to activate the iOS build target before Stage9.1 export.");
        }

        PlayerSettings.companyName = "TipsGodsStudio";
        PlayerSettings.productName = "GodsPVZ";
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, "com.tipsGodsStudio.godsPVZ");
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
        PlayerSettings.iOS.targetDevice = iOSTargetDevice.iPhoneAndiPad;
        PlayerSettings.iOS.targetOSVersionString = "12.0";

        // The recovered Assembly-CSharp has already passed the Stage9.1 runtime
        // qualification gates.  Do not ask UnityLinker to perform more managed
        // stripping than IL2CPP requires while producing the iOS player: the
        // failing export dies inside UnityTypeReferenceStep while traversing
        // type/interface references in ManagedStripped.  Keep this as a build
        // configuration change only; the qualified gameplay DLL is untouched.
        PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.iOS, ManagedStrippingLevel.Minimal);
        var stripping = PlayerSettings.GetManagedStrippingLevel(BuildTargetGroup.iOS);
        Debug.Log($"STAGE9_IOS_STRIPPING level={stripping}");
        if (stripping != ManagedStrippingLevel.Minimal)
            throw new InvalidOperationException($"Stage9.1 iOS managed stripping lock failed: expected Minimal, got {stripping}.");

        // Unity's documented architecture values are 0=None, 1=ARM64,
        // 2=Universal. Xcode 12+ no longer accepts the ARMv7 half of the
        // recovered project's legacy iOS architecture state, so lock ARM64.
        PlayerSettings.SetArchitecture(BuildTargetGroup.iOS, 1);
        var architecture = PlayerSettings.GetArchitecture(BuildTargetGroup.iOS);
        Debug.Log($"STAGE9_IOS_TARGET active={EditorUserBuildSettings.activeBuildTarget} architecture={architecture}");
        if (architecture != 1)
            throw new InvalidOperationException($"Stage9.1 iOS architecture lock failed: expected ARM64(1), got {architecture}.");

        EditorUserBuildSettings.symlinkLibraries = false;
        AssetDatabase.SaveAssets();

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
