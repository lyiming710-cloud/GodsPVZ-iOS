#if UNITY_EDITOR
using System;using System.Collections.Generic;using System.IO;using System.Linq;
using UnityEditor;using UnityEditor.Profiling;using UnityEditorInternal;using UnityEngine;

// Native Unity profiler samples only. No Harmony/detour, injected MethodBody,
// unit creation, forced attack, target assignment, health write or save edit.
public static class GodsPVZAttackTraceR28 {
    const string Active="GodsPVZ.R28AttackTrace.Active",FolderKey="GodsPVZ.R28AttackTrace.Folder",LastKey="GodsPVZ.R28AttackTrace.Last";
    [Serializable] public class Sample {public int index,parent,children,descendants,marker;public string name;}
    [Serializable] public class RawFrame {public int frame,thread,sample_count;public string thread_name;public bool tree_valid,profiler_enabled,deep_profiling;public Sample[] samples;}
    [Serializable] public class Hit {public int frame,thread,index,parent;public string name,kind;public bool tree_valid;public string[] ancestors;}
    [Serializable] public class Report {
        public string scope="ACTUAL_UNITY_NATIVE_PROFILER_STACKS_NO_GAME_METHOD_PATCH",candidate_sha256,status="COLLECTING",error="";
        public bool previous_enabled,previous_deep,profiler_enabled,deep_profiling,game_method_bytes_changed=false,game_fields_written=false;
        public int frames_read,frames_lost,invalid_trees,raw_frames_saved,samples_read,shooting_samples,sniper_samples,plant_to_zombie_damage_stacks,plant_to_device_damage_stacks,device_damage_stacks;
    }
    static Report report;static string folder;static int last=-1;
    public static bool PrepareBeforePlay(){const string prepared="GodsPVZ.R28AttackTrace.Prepared";if(SessionState.GetBool(prepared,false)&&ProfilerDriver.deepProfiling)return true;ProfilerDriver.deepProfiling=true;SessionState.SetBool(prepared,true);EditorUtility.RequestScriptReload();return false;}
    static string Output {get{return Path.Combine(folder,"R28-ACTUAL-ATTACK-TRACE.json");}}
    static void Save(){if(report!=null)File.WriteAllText(Output,JsonUtility.ToJson(report,true));}
    static bool Is(string name,string owner,string method){string n=(name??"").Replace("::",".").Replace(":",".");string p=owner+"."+method;int at=n.IndexOf(p,StringComparison.Ordinal);while(at>=0){int end=at+p.Length;bool before=at==0||!(Char.IsLetterOrDigit(n[at-1])||n[at-1]=='_'||n[at-1]=='+');bool after=end==n.Length||n.Substring(end).StartsWith("(",StringComparison.Ordinal)||n.Substring(end).StartsWith(" (",StringComparison.Ordinal);if(before&&after)return true;at=n.IndexOf(p,at+1,StringComparison.Ordinal);}return false;}
    public static void Begin(string output,string candidate) {
        folder=output;Directory.CreateDirectory(folder);if(File.Exists(Output))throw new InvalidOperationException("Never replace profiler evidence");
        report=new Report{candidate_sha256=candidate,previous_enabled=ProfilerDriver.enabled,previous_deep=ProfilerDriver.deepProfiling};
        SessionState.SetString(FolderKey,folder);SessionState.SetBool(Active,true);last=ProfilerDriver.lastFrameIndex;SessionState.SetInt(LastKey,last);Save();
        try{ProfilerDriver.deepProfiling=true;ProfilerDriver.enabled=true;report.profiler_enabled=ProfilerDriver.enabled;report.deep_profiling=ProfilerDriver.deepProfiling;Save();}
        catch(Exception ex){report.status="UNSUPPORTED_PROFILER_START";report.error=ex.ToString();Save();}
    }
    [InitializeOnLoadMethod] static void Resume(){if(!SessionState.GetBool(Active,false))return;folder=SessionState.GetString(FolderKey,"");if(String.IsNullOrEmpty(folder)||!File.Exists(Output))return;report=JsonUtility.FromJson<Report>(File.ReadAllText(Output));last=SessionState.GetInt(LastKey,-1);}
    public static void Capture() {
        if(!SessionState.GetBool(Active,false)||report==null||report.error.Length>0)return;
        try{
            int newest=ProfilerDriver.lastFrameIndex,first=ProfilerDriver.firstFrameIndex;if(newest<0)return;
            if(last+1<first){report.frames_lost+=first-last-1;last=first-1;}
            for(int frame=last+1;frame<=newest;frame++)using(var view=ProfilerDriver.GetRawFrameDataView(frame,0)){
                if(!view.valid){report.frames_lost++;continue;}
                int count=view.sampleCount;if(count<0||count>250000){report.frames_lost++;continue;}
                var samples=new Sample[count];var ends=new Stack<int>();var parents=new Stack<int>();var actualChildren=new int[count];bool valid=true;
                for(int i=0;i<count;i++){
                    while(ends.Count>0&&i>ends.Peek()){ends.Pop();parents.Pop();}
                    int parent=parents.Count==0?-1:parents.Peek(),children=view.GetSampleChildrenCount(i),descendants=view.GetSampleChildrenCountRecursive(i);
                    if(parent>=0)actualChildren[parent]++;
                    if(children<0||descendants<children||i+descendants>=count||(ends.Count>0&&i+descendants>ends.Peek()))valid=false;
                    samples[i]=new Sample{index=i,parent=parent,children=children,descendants=descendants,marker=view.GetSampleMarkerId(i),name=view.GetSampleName(i)};
                    if(descendants>0){parents.Push(i);ends.Push(i+descendants);}
                }
                for(int i=0;i<count;i++)if(actualChildren[i]!=samples[i].children)valid=false;
                report.frames_read++;report.samples_read+=count;if(!valid)report.invalid_trees++;
                bool saveRaw=report.raw_frames_saved==0,plantDamage=false;
                for(int i=0;i<count;i++){
                    var sample=samples[i];bool shooting=Is(sample.name,"Plant","Shooting"),sniper=Is(sample.name,"Plant","ST_Peasniper"),zombie=Is(sample.name,"Zombie","TakeDamage"),device=Is(sample.name,"Device","TakeDamage");
                    if(!shooting&&!sniper&&!zombie&&!device)continue;
                    var ancestors=new List<string>();int parent=sample.parent,steps=0;while(parent>=0&&steps++<=count){ancestors.Add(samples[parent].name);parent=samples[parent].parent;}ancestors.Reverse();
                    bool fromPlant=valid&&ProfilerDriver.enabled&&ProfilerDriver.deepProfiling&&ancestors.Any(n=>Is(n,"Plant","Shooting")||Is(n,"Plant","ST_Peasniper"));string kind=shooting?"PLANT_SHOOTING_SAMPLE":sniper?"SNIPER_SAMPLE":fromPlant&&zombie?"PLANT_TO_ZOMBIE_TAKEDAMAGE_STACK":fromPlant&&device?"PLANT_TO_DEVICE_TAKEDAMAGE_STACK":"DAMAGE_SAMPLE_SOURCE_NOT_PLANT_STACK";
                    if(shooting||sniper)saveRaw=true;
                    if(shooting)report.shooting_samples++;if(sniper)report.sniper_samples++;
                    if(fromPlant&&zombie){report.plant_to_zombie_damage_stacks++;plantDamage=true;}
                    if(fromPlant&&device){report.plant_to_device_damage_stacks++;plantDamage=true;}
                    if(valid&&!fromPlant&&(zombie||device)&&ancestors.Any(n=>Is(n,"Device","Attack")))report.device_damage_stacks++;
                    File.AppendAllText(Path.Combine(folder,"ATTACK-CALL-STACKS.jsonl"),JsonUtility.ToJson(new Hit{frame=frame,thread=0,index=i,parent=sample.parent,name=sample.name,kind=kind,tree_valid=valid,ancestors=ancestors.ToArray()})+"\n");
                }
                if(plantDamage)saveRaw=true;
                if(saveRaw&&report.raw_frames_saved<16){File.AppendAllText(Path.Combine(folder,"ATTACK-RAW-PROFILER-FRAMES.jsonl"),JsonUtility.ToJson(new RawFrame{frame=frame,thread=0,thread_name=view.threadName,sample_count=count,tree_valid=valid,profiler_enabled=ProfilerDriver.enabled,deep_profiling=ProfilerDriver.deepProfiling,samples=samples})+"\n");report.raw_frames_saved++;}
            }
            last=newest;SessionState.SetInt(LastKey,last);report.profiler_enabled=ProfilerDriver.enabled;report.deep_profiling=ProfilerDriver.deepProfiling;Save();
        }catch(Exception ex){report.status="UNSUPPORTED_PROFILER_CAPTURE";report.error=ex.ToString();Save();}
    }
    public static void Finish(){if(!SessionState.GetBool(Active,false)||report==null)return;Capture();if(report.error.Length==0)report.status=report.plant_to_zombie_damage_stacks+report.plant_to_device_damage_stacks>0?"ACTUAL_PLANT_DAMAGE_CALL_STACK_OBSERVED_TARGET_ARGUMENT_NOT_CAPTURED":"NO_PLANT_DAMAGE_CALL_STACK_OBSERVED";Save();SessionState.SetBool(Active,false);try{ProfilerDriver.enabled=report.previous_enabled;}catch{} }
}
#endif
