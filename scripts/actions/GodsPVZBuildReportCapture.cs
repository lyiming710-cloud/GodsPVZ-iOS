#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Build.Reporting;
using UnityEngine;

// Observes the original build helper. Its success/error decisions remain unchanged.
public static class GodsPVZBuildReportCapture
{
    private static BuildReport lastReport;
    public static void Record(BuildReport report) {
        if (report == null || lastReport != null) throw new InvalidOperationException("Exactly one actual BuildPlayer report required");
        lastReport = report;
    }
    [Serializable] public sealed class Message { public string type; public string content; }
    [Serializable] public sealed class Step { public string name; public int depth; public double seconds; public Message[] messages; }
    [Serializable] public sealed class Log { public string type; public string condition; public string stack; }
    [Serializable] public sealed class Report {
        public string scope = "ACTUAL_UNITY_BUILD_REPORT_NO_ERROR_SUPPRESSION";
        public bool build_report_found;
        public string result;
        public int total_errors;
        public int total_warnings;
        public ulong total_size;
        public string output;
        public string exception;
        public Step[] steps;
        public Log[] logs;
        public bool original_build_helper_unchanged = true;
        public bool errors_ignored = false;
    }
    public static void ExportIOS()
    {
        var folder = Environment.GetEnvironmentVariable("GODSPVZ_BUILD_EVIDENCE_DIR");
        if (String.IsNullOrWhiteSpace(folder)) throw new InvalidOperationException("Explicit build evidence directory required");
        Directory.CreateDirectory(folder);
        var logs = new List<Log>();
        var sync = new object();
        Application.LogCallback callback = (condition, stack, type) => {
            if (type != LogType.Error && type != LogType.Exception && type != LogType.Assert) return;
            lock (sync) logs.Add(new Log { type=type.ToString(), condition=condition, stack=stack });
        };
        Application.logMessageReceivedThreaded += callback;
        lastReport = null;
        Exception failure = null;
        try {
            GodsPVZIOSBuild.BuildIOS();
            if (lastReport == null) throw new InvalidOperationException("Original build helper did not record its actual report");
        }
        catch (Exception ex) { failure = ex; throw; }
        finally {
            Application.logMessageReceivedThreaded -= callback;
            var r = lastReport;
            var result = new Report { build_report_found=r!=null, exception=failure==null ? null : failure.ToString() };
            if (r != null) {
                result.result=r.summary.result.ToString(); result.total_errors=r.summary.totalErrors;
                result.total_warnings=r.summary.totalWarnings; result.total_size=r.summary.totalSize;
                result.output=r.summary.outputPath;
                result.steps=r.steps.Select(s=>new Step {
                    name=s.name, depth=s.depth, seconds=s.duration.TotalSeconds,
                    messages=s.messages.Select(m=>new Message { type=m.type.ToString(), content=m.content }).ToArray()
                }).ToArray();
            }
            lock (sync) result.logs=logs.ToArray();
            File.WriteAllText(Path.Combine(folder,"actual-unity-build-report.json"),JsonUtility.ToJson(result,true));
        }
    }
}
#endif
