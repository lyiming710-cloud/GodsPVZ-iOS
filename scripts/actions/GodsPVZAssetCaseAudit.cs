#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
public static class GodsPVZAssetCaseAudit
{
    [Serializable] public sealed class Expected { public string guid; public string path; public string name; }
    [Serializable] public sealed class Input { public Expected[] rows; }
    [Serializable] public sealed class Row { public string guid; public string expected_path; public string observed_path; public string expected_name; public string observed_name; public string type; public bool pass; }
    [Serializable] public sealed class Output { public string scope="ACTUAL_UNITY_IMPORTED_RELOCATED_ASSETS_GUID_AND_NAME"; public int assets; public int failed; public Row[] rows; public bool game_runtime_or_device_acceptance=false; }
    public static void Run()
    {
        var input=Environment.GetEnvironmentVariable("GODSPVZ_ASSET_CASE_IMPORT_NAMES");
        var output=Environment.GetEnvironmentVariable("GODSPVZ_ASSET_CASE_AUDIT_OUTPUT");
        if(String.IsNullOrWhiteSpace(input)||String.IsNullOrWhiteSpace(output))throw new InvalidOperationException("Explicit asset case audit input/output required");
        var expected=JsonUtility.FromJson<Input>(File.ReadAllText(input));
        if(expected==null||expected.rows==null||expected.rows.Length!=12)throw new InvalidOperationException("Exactly12 relocated original assets required");
        var rows=expected.rows.Select(e=>{
            var path=AssetDatabase.GUIDToAssetPath(e.guid);var obj=AssetDatabase.LoadMainAssetAtPath(path);
            return new Row { guid=e.guid,expected_path=e.path,observed_path=path,expected_name=e.name,observed_name=obj==null?null:obj.name,type=obj==null?null:obj.GetType().FullName,pass=path==e.path&&obj!=null&&obj.name==e.name&&AssetDatabase.AssetPathToGUID(path)==e.guid };
        }).ToArray();
        var result=new Output { assets=rows.Length,failed=rows.Count(r=>!r.pass),rows=rows };
        Directory.CreateDirectory(Path.GetDirectoryName(output));File.WriteAllText(output,JsonUtility.ToJson(result,true));
        if(result.failed!=0)throw new InvalidOperationException("Relocated asset GUID/name audit failed: "+result.failed);
        Debug.Log("STARTUP_R20_CASE_ASSETS_GUID_AND_NAME_PASS assets="+result.assets);
    }
}
#endif
