// Runs the real game in a visible Editor GameView. Input comes from X11/XTest,
// not simulated EventSystem calls, unit Update calls, or seeded combat fields.
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class GodsPVZGameplayAudit
{
    const string Active="GodsPVZ.ComplexGameplay.Active", Stage="GodsPVZ.ComplexGameplay.Stage", Start="GodsPVZ.ComplexGameplay.Start", Next="GodsPVZ.ComplexGameplay.Next";
    const string Expected="75160a3233f1e1ab13a44f712efb4b47fdeb9b57bd38fa25f180276bb7b50e15";
    const BindingFlags BF=BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance|BindingFlags.Static;
    [Serializable] public class Check { public string name,detail;public bool pass; }
    [Serializable] public class ActionRow { public int stage,frame;public string utc,action,scene,detail; }
    [Serializable] public class InputRequest { public int sequence,x,y,frame;public float expected_screen_x,expected_screen_y;public string kind,context,utc; }
    [Serializable] public class InputAck { public int sequence,exit_code;public string phase,detail; }
    [Serializable] public class InputObservation { public int sequence,pressed_frame,completed_frame;public string context,kind;public bool button_seen,coordinate_match;public float wanted_x,wanted_y,observed_x,observed_y; }
    [Serializable] public class UnitRow { public string kind,utc,time,x,y,z,health,armor1,armor2,attack_countdown,attack_effect_countdown,hit_count,logical_x,logical_y,logical_z;public int frame,instance,ID,grid_x,grid_y,source_plant,target_zombie,path_count,path_grid_x,path_grid_y;public string path_x,path_y,path_waiting,waiting,stance,dead,on_field,wave,camp; }
    [Serializable] public class EventRow { public string utc,kind,detail;public int frame,unit,source_plant,target; }
    [Serializable] public class ErrorRow { public string message,stack;public int count; }
    [Serializable] public class BankRow {public int frame,instance,plant_id;public string utc,state,on_bank,sun_price,cd_time,al_time,path,world,local,scale,background,canvas,point,visibility;}
    [Serializable] public class Report {
        public string scope="REAL_UNITY_EDITOR_PLAYER_LOOP_AND_X11_OS_INPUT_COMPLEX_LEVEL_OBSERVATION_NOT_IOS_OR_SEMANTIC_EQUIVALENCE",candidate_sha256,workflow_head,status="RUNNING",entry_route="Public original GlobalStaticVars.EnterBoard(Adventure,19,null); bypasses save-level unlock only",level_name="裂鸣共振",original_config_sha256="da1d2483d8c436f7a439aa7904ee62e6a65c54990cf3ab95d6d3dfc70ef9e427";
        public int level=19,configured_zombie_types=14,configured_flags=3,configured_waves=30,configured_sun=1500,selected_card_instance,selected_plant_id=-1,bank_card_instance,planting_attempts,dialogue_clicks,calibration_attempts,last_input_sequence,last_frame,observed_frames,unit_samples,projectiles_with_live_plant_source,health_decrease_events,projectile_hit_count_events;
        public bool collection_complete,device_execution=false,game_fields_assigned=false,game_dll_changed=false,save_unlock_modified=false,level_data_modified=false,units_spawned_by_probe=false,unit_update_or_damage_called_by_probe=false,os_input_calibrated,selected_after_os_input,confirmed_after_os_input,board_started,plant_on_field_observed,plant_count_increased_after_input,complex_gameplay_pass=false,whole_level_completed=false,planting_blocked;
        public string planting_block_reason;
        public float coordinate_correction_x,coordinate_correction_y,board_start_time=-1,observed_game_seconds;
        public List<Check> checks=new List<Check>();public List<ActionRow> actions=new List<ActionRow>();public List<InputObservation> input_observations=new List<InputObservation>();public List<ErrorRow> runtime_errors=new List<ErrorRow>();public List<string> limits=new List<string>();public List<int> zombie_types_observed=new List<int>(),zombie_instances_observed=new List<int>(),plant_instances_observed=new List<int>();
    }
    static Report report;static EditorWindow view;static int lastObservedFrame=-1,pressedFrame=-1,plantsBeforePlacement;static float lastSample=-1,lastScreen=-1;static double lastSave;static InputRequest pending;static InputObservation pendingObservation;
    static readonly Dictionary<int,UnitRow> lastUnits=new Dictionary<int,UnitRow>();static readonly HashSet<int> projectileSources=new HashSet<int>();
    static string Folder { get {var p=Environment.GetEnvironmentVariable("GODSPVZ_GAMEPLAY_OUTPUT");if(String.IsNullOrEmpty(p))throw new InvalidOperationException("Gameplay output path required");return p;} }
    static string Output { get {return Path.Combine(Folder,"GAMEPLAY-RESULT.json");} }
    static Type TypeNamed(string name) {return AppDomain.CurrentDomain.GetAssemblies().Select(a=>a.GetType(name,false)).FirstOrDefault(t=>t!=null);}
    static object Read(object o,string name) {if(o==null)return null;var f=o.GetType().GetField(name,BF);return f==null?null:f.GetValue(o);}
    static bool Bool(object o,string name) {return System.Object.Equals(Read(o,name),true);}
    static string S(object o) {return o==null?"null":Convert.ToString(o,CultureInfo.InvariantCulture);}
    static string F(object o,string name) {return S(Read(o,name));}
    static int Int(object o,string name,int missing=-1) {object v=Read(o,name);return v==null?missing:Convert.ToInt32(v);}
    static bool Live(object o) {var u=o as UnityEngine.Object;return o is UnityEngine.Object?u!=null:o!=null;}
    static int Instance(object o) {var u=o as UnityEngine.Object;return u==null?0:u.GetInstanceID();}
    static MonoBehaviour[] All(string name) {var t=TypeNamed(name);return t==null?new MonoBehaviour[0]:UnityEngine.Object.FindObjectsOfType(t).OfType<MonoBehaviour>().Where(c=>c!=null&&c.gameObject.scene.IsValid()).ToArray();}
    static MonoBehaviour Find(string name) {return All(name).FirstOrDefault();}
    static object Call(object o,string name,params object[] args) {if(o==null)throw new InvalidOperationException("No live target: "+name);var a=o.GetType().GetMethods(BindingFlags.Public|BindingFlags.Instance).Where(m=>m.Name==name&&m.GetParameters().Length==args.Length).ToArray();if(a.Length!=1)throw new InvalidOperationException("Public callback missing/ambiguous: "+name);return a[0].Invoke(o,args);}
    static object Lawn() {var t=TypeNamed("GlobalStaticVars");return t.GetField("gLawnApp",BF).GetValue(null);}
    static void CheckNow(string name,bool pass,string detail) {report.checks.Add(new Check{name=name,pass=pass,detail=detail});}
    static string Sha(string path) {using(var h=System.Security.Cryptography.SHA256.Create())return BitConverter.ToString(h.ComputeHash(File.ReadAllBytes(path))).Replace("-","").ToLowerInvariant();}
    static void Save() {Directory.CreateDirectory(Folder);File.WriteAllText(Output,JsonUtility.ToJson(report,true));lastSave=EditorApplication.timeSinceStartup;}
    static void Log(string message,string stack,LogType type) {if(report==null||(type!=LogType.Error&&type!=LogType.Exception&&type!=LogType.Assert))return;var old=report.runtime_errors.FirstOrDefault(r=>r.message==message&&r.stack==stack);if(old!=null){old.count++;return;}if(report.runtime_errors.Count<200)report.runtime_errors.Add(new ErrorRow{message=message,stack=stack,count=1});}
    static void Event(string kind,int unit,int source,int target,string detail) {File.AppendAllText(Path.Combine(Folder,"COMBAT-EVENTS.jsonl"),JsonUtility.ToJson(new EventRow{utc=DateTime.UtcNow.ToString("o"),frame=Time.frameCount,kind=kind,unit=unit,source_plant=source,target=target,detail=detail})+"\n");}
    static void View() {
        if(view==null){var t=typeof(EditorWindow).Assembly.GetType("UnityEditor.GameView",true);view=EditorWindow.GetWindow(t,true,"GodsPVZ actual gameplay",true);view.position=new Rect(40,60,1360,860);view.Show();}
        view.Focus();
    }
    static Vector2 Desktop(Vector2 screen) {
        View();var p=view.GetType().GetProperty("targetInView",BF);if(p==null)throw new InvalidOperationException("Actual GameView targetInView API missing");Rect target=(Rect)p.GetValue(view,null);
        if(Screen.width<=0||Screen.height<=0||target.width<=0||target.height<=0)throw new InvalidOperationException("Actual GameView render extent unavailable");
        return new Vector2(view.position.x+target.x+screen.x*target.width/Screen.width+report.coordinate_correction_x,view.position.y+target.y+(Screen.height-screen.y)*target.height/Screen.height+report.coordinate_correction_y);
    }
    static Vector2 UiPoint(GameObject go) {
        if(go==null)throw new InvalidOperationException("No actual UI click target");var rt=go.transform as RectTransform;if(rt==null)throw new InvalidOperationException("Target is not a RectTransform: "+go.name);var canvas=go.GetComponentInParent<Canvas>();var cam=canvas==null||canvas.renderMode==RenderMode.ScreenSpaceOverlay?null:canvas.worldCamera;
        return RectTransformUtility.WorldToScreenPoint(cam,rt.TransformPoint(rt.rect.center));
    }
    static GameObject Callback(string name) {
        foreach(var c in Resources.FindObjectsOfTypeAll<MonoBehaviour>().Where(x=>x!=null&&x.gameObject.scene.IsValid()&&x.gameObject.activeInHierarchy)){
            var p=c.GetType().GetProperty("onClick");if(p==null)continue;var ev=p.GetValue(c,null);if(ev==null)continue;var count=ev.GetType().GetMethod("GetPersistentEventCount");if(count==null)continue;
            int n=(int)count.Invoke(ev,null);for(int i=0;i<n;i++)if((string)ev.GetType().GetMethod("GetPersistentMethodName").Invoke(ev,new object[]{i})==name)return c.gameObject;
        }
        return null;
    }
    static void Request(Vector2 screen,string context,bool click=true) {
        if(pending!=null)throw new InvalidOperationException("Input already pending");Vector2 desktop=Desktop(screen);if(screen.x<0||screen.y<0||screen.x>=Screen.width||screen.y>=Screen.height)throw new InvalidOperationException("Target outside rendered GameView: "+screen);
        pending=new InputRequest{sequence=++report.last_input_sequence,x=Mathf.RoundToInt(desktop.x),y=Mathf.RoundToInt(desktop.y),expected_screen_x=screen.x,expected_screen_y=screen.y,context=context,kind=click?"click":"move",frame=Time.frameCount,utc=DateTime.UtcNow.ToString("o")};
        pendingObservation=new InputObservation{sequence=pending.sequence,context=context,kind=pending.kind,wanted_x=screen.x,wanted_y=screen.y};pressedFrame=-1;File.WriteAllText(Path.Combine(Folder,"INPUT-REQUEST.json"),JsonUtility.ToJson(pending,true));Save();
    }
    static bool InputPending() {
        if(pending==null)return false;string ackPath=Path.Combine(Folder,"INPUT-ACK.json");InputAck ack=null;if(File.Exists(ackPath)){try{ack=JsonUtility.FromJson<InputAck>(File.ReadAllText(ackPath));}catch(IOException){return true;}catch(ArgumentException){return true;}}
        if(ack==null||ack.sequence!=pending.sequence)return true;
        Vector3 mouse=Input.mousePosition;pendingObservation.observed_x=mouse.x;pendingObservation.observed_y=mouse.y;pendingObservation.coordinate_match=Vector2.Distance(new Vector2(mouse.x,mouse.y),new Vector2(pending.expected_screen_x,pending.expected_screen_y))<=4;
        if(Input.GetMouseButton(0)){pendingObservation.button_seen=true;if(pressedFrame<0)pressedFrame=Time.frameCount;pendingObservation.pressed_frame=pressedFrame;}
        if(ack.phase=="pressed"&&pressedFrame>=0&&Time.frameCount>=pressedFrame+2)File.WriteAllText(Path.Combine(Folder,"INPUT-RELEASE-"+pending.sequence+".txt"),"actual Input.GetMouseButton(0) observed for at least two frames\n");
        if(ack.phase!="complete")return true;
        // Wait for the real player loop to process release and its UI click.
        if(pending.kind=="click"&&pressedFrame>=0&&(Time.frameCount<pressedFrame+4||Input.GetMouseButton(0)))return true;
        if(ack.exit_code!=0)throw new InvalidOperationException("OS input driver failed: "+ack.detail);
        pendingObservation.completed_frame=Time.frameCount;report.input_observations.Add(pendingObservation);
        if(pending.kind=="move"){
            if(pendingObservation.coordinate_match){report.os_input_calibrated=true;CheckNow("actual_unity_input_mouse_coordinates_match_os_move",true,"wanted="+pending.expected_screen_x+","+pending.expected_screen_y+" actual="+mouse);}
            else{Rect target=(Rect)view.GetType().GetProperty("targetInView",BF).GetValue(view,null);report.coordinate_correction_x+=(pending.expected_screen_x-mouse.x)*target.width/Screen.width;report.coordinate_correction_y-=(pending.expected_screen_y-mouse.y)*target.height/Screen.height;}
        }else CheckNow("os_click_observed_"+pending.sequence,pendingObservation.button_seen&&pendingObservation.coordinate_match,pending.context+" observed="+mouse+" button="+pendingObservation.button_seen);
        pending=null;pendingObservation=null;return false;
    }
    static void Screenshot(string context) {string path=Path.Combine(Folder,"screens",Time.frameCount.ToString("D7")+"-"+context+".png");Directory.CreateDirectory(Path.GetDirectoryName(path));ScreenCapture.CaptureScreenshot(path);}
    static void Observe() {
        if(Time.frameCount==lastObservedFrame)return;lastObservedFrame=Time.frameCount;report.last_frame=Time.frameCount;report.observed_frames++;
        var b=Find("Board");if(b==null||!Bool(b,"gameStart"))return;
        if(!report.board_started){report.board_started=true;report.board_start_time=Time.time;Event("REAL_BOARD_STARTED",Instance(b),0,0,"level="+F(b,"level")+" challenge="+F(b,"challengeType"));}
        report.observed_game_seconds=Time.time-report.board_start_time;
        if(Time.unscaledTime-lastScreen>=15){Screenshot("combat");lastScreen=Time.unscaledTime;}
        if(Time.unscaledTime-lastSample<0.2f)return;lastSample=Time.unscaledTime;
        foreach(string kind in new[]{"Zombie","Plant","Projectile"})foreach(var c in All(kind)){
            // Retain active scene units even if a damaged field says off-board.
            // Flags are observations, never a reason to hide a failed unit.
            Vector3 pos=c.transform.position;var row=new UnitRow{kind=kind,utc=DateTime.UtcNow.ToString("o"),time=Time.time.ToString("R",CultureInfo.InvariantCulture),frame=Time.frameCount,instance=c.GetInstanceID(),ID=Int(c,"ID"),x=pos.x.ToString("R",CultureInfo.InvariantCulture),y=pos.y.ToString("R",CultureInfo.InvariantCulture),z=pos.z.ToString("R",CultureInfo.InvariantCulture),health=F(c,"healthPoint"),armor1=F(c,"armor1Point"),armor2=F(c,"armor2Point"),logical_x=F(c,"fX"),logical_y=F(c,"fY"),logical_z=F(c,"fZ"),grid_x=Int(c,kind=="Plant"?"GridX":"gridX"),grid_y=Int(c,kind=="Plant"?"GridY":"gridY"),attack_countdown=F(c,"attackIntervalCountdown"),attack_effect_countdown=F(c,"attackEffectCountdown"),target_zombie=Instance(Read(c,"targetZombie")),source_plant=Instance(Read(c,"origin_Plant")),hit_count=F(c,"hitNum"),waiting=F(c,"waitingTime"),stance=F(c,"isStant"),dead=F(c,"isDied"),on_field=F(c,kind=="Plant"?"isOnField":"isOnBoard"),wave=F(c,"wave"),camp=F(c,"camp"),path_count=-1};
            var path=Read(c,"path") as IList;if(path!=null){row.path_count=path.Count;if(path.Count>0){var node=path[0];row.path_grid_x=Int(node,"gridX");row.path_grid_y=Int(node,"gridY");object point=Read(node,"position");if(point is Vector3){var p=(Vector3)point;row.path_x=p.x.ToString("R",CultureInfo.InvariantCulture);row.path_y=p.y.ToString("R",CultureInfo.InvariantCulture);}row.path_waiting=F(node,"waitingTime");}}
            if(kind=="Zombie"){if(!report.zombie_types_observed.Contains(row.ID))report.zombie_types_observed.Add(row.ID);if(!report.zombie_instances_observed.Contains(row.instance))report.zombie_instances_observed.Add(row.instance);}
            if(kind=="Plant"&&Bool(c,"isOnField")){report.plant_on_field_observed=true;if(!report.plant_instances_observed.Contains(row.instance))report.plant_instances_observed.Add(row.instance);}
            if(kind=="Projectile"&&row.source_plant!=0){var d=Read(c,"damage");int damageSource=Instance(Read(d,"sourcePlant"));if(projectileSources.Add(row.instance)){report.projectiles_with_live_plant_source++;Event("PLANT_SOURCED_PROJECTILE_OBSERVED",row.instance,row.source_plant,0,"damage.sourcePlant="+damageSource+" origin_Plant="+row.source_plant+" projectileID="+row.ID+" damagePoint="+F(d,"damagePoint"));}}
            UnitRow old;if(lastUnits.TryGetValue(row.instance,out old)){
                float before,after;if(kind=="Zombie"&&Single.TryParse(old.health,NumberStyles.Float,CultureInfo.InvariantCulture,out before)&&Single.TryParse(row.health,NumberStyles.Float,CultureInfo.InvariantCulture,out after)&&after<before){report.health_decrease_events++;Event("ZOMBIE_HP_DECREASE_SOURCE_UNPROVEN",row.instance,0,0,"before="+old.health+" after="+row.health+" devices can also damage zombies");}
                int h0,h1;if(kind=="Projectile"&&row.source_plant!=0&&Int32.TryParse(old.hit_count,out h0)&&Int32.TryParse(row.hit_count,out h1)&&h1>h0){report.projectile_hit_count_events++;Event("PLANT_PROJECTILE_HIT_COUNT_INCREASE",row.instance,row.source_plant,0,"before="+h0+" after="+h1+" exact hit target not instrumented");}
            }
            lastUnits[row.instance]=row;File.AppendAllText(Path.Combine(Folder,"UNIT-TRAJECTORIES.jsonl"),JsonUtility.ToJson(row)+"\n");report.unit_samples++;
        }
    }
    static MonoBehaviour CardOnBank() {return All("Card").Where(c=>Bool(c,"isOnBank")&&Int(c,"cardPlantType")>=0&&Int(c,"cardState")==2).OrderBy(c=>Int(c,"ordinalOnbank")).FirstOrDefault();}
    static MonoBehaviour ChosenCard() {return All("Card_Choose").FirstOrDefault(c=>Read(c,"seedChooserScreen")!=null&&c.gameObject.activeInHierarchy);}
    static int OnFieldCount() {return All("Plant").Count(c=>Bool(c,"isOnField"));}
    static bool BankPoint(MonoBehaviour card,out Vector2 point,out string detail) {
        point=Vector2.zero;var image=Read(card,"cardBackground")as Component;if(image==null){detail="Original cardBackground reference missing";return false;}var rect=image.transform as RectTransform;if(rect==null){detail="Original cardBackground is not a RectTransform";return false;}var canvas=image.GetComponentInParent<Canvas>();if(canvas==null||!canvas.isActiveAndEnabled||!image.gameObject.activeInHierarchy){detail="Actual background/canvas is inactive";return false;}
        var camera=canvas.renderMode==RenderMode.ScreenSpaceOverlay?null:canvas.worldCamera;point=RectTransformUtility.WorldToScreenPoint(camera,rect.TransformPoint(rect.rect.center));var corners=new Vector3[4];rect.GetWorldCorners(corners);var a=RectTransformUtility.WorldToScreenPoint(camera,corners[0]);var b=RectTransformUtility.WorldToScreenPoint(camera,corners[2]);bool visible=Mathf.Abs(b.x-a.x)>2&&Mathf.Abs(b.y-a.y)>2&&point.x>=0&&point.y>=0&&point.x<Screen.width&&point.y<Screen.height;var cr=image.GetComponent<CanvasRenderer>();if(cr!=null&&(cr.cull||cr.GetAlpha()<=0))visible=false;detail="background="+image.name+" canvas="+canvas.name+" mode="+canvas.renderMode+" camera="+(camera==null?"null":camera.name)+" point="+point+" extent="+(b-a)+" cull="+(cr==null?"unavailable":cr.cull.ToString());return visible;
    }
    static void ObserveBank() {
        foreach(var card in All("Card").Where(c=>Bool(c,"isOnBank"))){Vector2 point;string detail;bool visible=BankPoint(card,out point,out detail);var background=Read(card,"cardBackground")as Component;var canvas=background==null?null:background.GetComponentInParent<Canvas>();var row=new BankRow{frame=Time.frameCount,instance=card.GetInstanceID(),plant_id=Int(card,"cardPlantType"),utc=DateTime.UtcNow.ToString("o"),state=F(card,"cardState"),on_bank=F(card,"isOnBank"),sun_price=F(card,"sunPrice"),cd_time=F(card,"cdTime"),al_time=F(card,"alTime"),path=card.name,world=card.transform.position.ToString(),local=card.transform.localPosition.ToString(),scale=card.transform.lossyScale.ToString(),background=background==null?"null":background.name,canvas=canvas==null?"null":canvas.name,point=point.ToString(),visibility=visible+" "+detail};File.AppendAllText(Path.Combine(Folder,"BANK-OBSERVATIONS.jsonl"),JsonUtility.ToJson(row)+"\n");}
    }
    static Vector2 PlantingPoint() {
        var board=Find("Board");object config=Read(board,"boardConfig");if(config==null)throw new InvalidOperationException("Actual board config unavailable");var camera=Camera.main;if(camera==null)throw new InvalidOperationException("No actual main gameplay camera");
        // Select an actual unoccupied grass cell, without changing a map or grid.
        for(int x=2;x<=5;x++)for(int y=1;y<=5;y++){
            object grid=Call(board,"GetGrid",x,y);if(grid==null||Int(grid,"gridState")!=1||Live(Read(grid,"plant_common"))||Live(Read(grid,"plant_bottom"))||Live(Read(grid,"device_common")))continue;
            object hand=Read(Find("MouseManager"),"plantPrefabOnHand");if(!Live(hand)||!System.Object.Equals(Call(grid,"CanPlanting",hand),true))continue;
            Vector3 world=(Vector3)Call(config,"GetGridCenterPosition",x,y);Vector3 screen=camera.WorldToScreenPoint(world);if(screen.z<=0||screen.x<0||screen.y<0||screen.x>=Screen.width||screen.y>=Screen.height)continue;
            Event("ACTUAL_PLANTING_CELL_SELECTED",Instance(board),0,0,"grid="+x+","+y+" world="+world+" screen="+screen);return new Vector2(screen.x,screen.y);
        }
        throw new InvalidOperationException("No visible empty legal grass cell found; map unchanged");
    }
    static void Finish(bool complete,string status) {
        report.collection_complete=complete;report.status=status;report.game_dll_changed=Sha(Path.Combine(Application.dataPath,"Plugins/GodsPVZRuntime1.dll"))!=Expected;
        var b=Find("Board");report.whole_level_completed=b!=null&&Bool(b,"isWon");
        report.limits.Add("Existing Adventure 19 entry bypasses menu/save unlock. Normal unlock navigation is not accepted by this test.");
        report.limits.Add("Trajectory/attack observations are recorded samples, not a native behavioral equivalence proof. Preplaced devices can damage zombies.");
        report.limits.Add("HP loss alone does not attribute damage to plants. Projectile creation and hit counters are separate observations; exact collision target may remain unproven.");
        report.limits.Add("No iOS hardware, touch delivery, full-wave or whole-game acceptance is implied.");
        Save();SessionState.SetBool(Active,false);EditorApplication.update-=Tick;Application.logMessageReceived-=Log;
        // CaptureScreenshot is written by a rendered frame, so allow the final
        // requested image to finish instead of exiting in the same callback.
        double exitAfter=EditorApplication.timeSinceStartup+1.5;int code=complete?0:79;EditorApplication.CallbackFunction exit=null;exit=()=>{if(EditorApplication.timeSinceStartup<exitAfter)return;EditorApplication.update-=exit;EditorApplication.Exit(code);};EditorApplication.update+=exit;
    }
    public static void Begin() {
        if(SessionState.GetBool(Active,false)||File.Exists(Output))throw new InvalidOperationException("Never replace an existing gameplay session/result");
        if(Application.isBatchMode)throw new InvalidOperationException("Real GameView OS-input diagnostic requires a non-batch Editor");
        if(Sha(Path.Combine(Application.dataPath,"Plugins/GodsPVZRuntime1.dll"))!=Expected)throw new InvalidOperationException("Gameplay candidate hash mismatch");
        string config=Path.Combine(Application.streamingAssetsPath,"jsons/BoardData/Config/Adventure/19.json");if(!File.Exists(config)||Sha(config)!="da1d2483d8c436f7a439aa7904ee62e6a65c54990cf3ab95d6d3dfc70ef9e427")throw new InvalidOperationException("Actual Adventure 19 must match original PC config bytes");
        report=new Report{candidate_sha256=Expected,workflow_head=Environment.GetEnvironmentVariable("GITHUB_SHA")};Save();
        EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity",OpenSceneMode.Single);SessionState.SetBool(Active,true);SessionState.SetInt(Stage,0);SessionState.SetString(Start,DateTime.UtcNow.Ticks.ToString());SessionState.SetString(Next,DateTime.UtcNow.AddSeconds(8).Ticks.ToString());Attach();View();EditorApplication.isPlaying=true;
    }
    public static void Prepare() {if(Sha(Path.Combine(Application.dataPath,"Plugins/GodsPVZRuntime1.dll"))!=Expected)throw new InvalidOperationException("Wrong precompiled gameplay candidate");Debug.Log("GAMEPLAY_EDITOR_HELPER_PRECOMPILED_NO_GAME_EXECUTION");}
    [InitializeOnLoadMethod] static void Resume() {if(SessionState.GetBool(Active,false)){report=JsonUtility.FromJson<Report>(File.ReadAllText(Output));Attach();}}
    static void Attach() {EditorApplication.update-=Tick;EditorApplication.update+=Tick;Application.logMessageReceived-=Log;Application.logMessageReceived+=Log;}
    static void Tick() {
        if(!SessionState.GetBool(Active,false)||report==null)return;
        if(DateTime.UtcNow.Ticks-Int64.Parse(SessionState.GetString(Start,"0"))>TimeSpan.FromSeconds(420).Ticks){Finish(false,"WALL_TIMEOUT_RETAINED");return;}
        if(!EditorApplication.isPlaying)return;
        try{Observe();if(EditorApplication.timeSinceStartup-lastSave>5)Save();if(InputPending())return;}catch(Exception ex){Log("Gameplay observation/input error: "+ex.Message,ex.ToString(),LogType.Error);Finish(false,"PROBE_OR_INPUT_FAILED");return;}
        if(DateTime.UtcNow.Ticks<Int64.Parse(SessionState.GetString(Next,"0")))return;
        int stage=SessionState.GetInt(Stage,0),next=stage+1;string action="";
        try{
            switch(stage){
            case 0:action="Use existing game save; create a normal new player only if fresh CI has none";object saves=Read(Lawn(),"savesManager");if(Read(saves,"playerSave")==null){var popup=Find("Window_I");if(popup!=null&&(Int(popup,"I")==1||Int(popup,"I")==2)){Call(popup,"Input","R21-Complex-CI");Call(popup,"Yes");}else Call(saves,"CreateNewPlayerSave","R21-Complex-CI");}CheckNow("actual_game_player_save_exists",Read(saves,"playerSave")!=null,"No unlock, inventory or combat fields assigned");break;
            case 1:action="Enter original existing complex Adventure 19 using public normal level loader";var enter=TypeNamed("GlobalStaticVars").GetMethods(BindingFlags.Public|BindingFlags.Static).Single(m=>m.Name=="EnterBoard"&&m.GetParameters().Length==3);var ct=enter.GetParameters()[0].ParameterType;enter.Invoke(null,new object[]{Enum.Parse(ct,"Adventure"),19,null});break;
            case 2:action="Wait for real preparation scene; use normal SeedChoose navigation callback";var prep=Find("PrepareUIController");if(prep==null){next=2;break;}Call(prep,"SeedChoose");Screenshot("selection");break;
            case 3:action="Calibrate external OS mouse against actual Unity Input.mousePosition";if(!report.os_input_calibrated){if(report.calibration_attempts++>=3)throw new InvalidOperationException("GameView OS-input calibration failed");Request(new Vector2(Screen.width*.5f,Screen.height*.5f),"calibrate actual GameView",false);next=3;}break;
            case 4:action="Actual OS click on available normal seed-selection card";var chosen=ChosenCard();if(chosen==null)throw new InvalidOperationException("No actual seed-selection card");report.selected_card_instance=chosen.GetInstanceID();report.selected_plant_id=Int(chosen,"plantID");Request(UiPoint(chosen.gameObject),"choose plant ID "+report.selected_plant_id);break;
            case 5:action="Observe same seed card selection; OS click normal Determine button";var selected=All("Card_Choose").FirstOrDefault(c=>c.GetInstanceID()==report.selected_card_instance);report.selected_after_os_input=selected!=null&&Bool(selected,"isChosen");CheckNow("plant_selected_by_os_input",report.selected_after_os_input,"plantID="+report.selected_plant_id);if(!report.selected_after_os_input)throw new InvalidOperationException("Real seed selection did not occur");Request(UiPoint(Callback("Determine")),"confirm selected plants");break;
            case 6:action="Observe confirmed preparation; OS click normal GameStart button";report.confirmed_after_os_input=Find("SeedChooserScreen")==null&&Find("PrepareUIController")!=null;CheckNow("selection_confirmed_by_os_input",report.confirmed_after_os_input,"No Determine fallback called");if(!report.confirmed_after_os_input)throw new InvalidOperationException("Selection confirmation did not occur");Request(UiPoint(Callback("GameStart")),"start normal configured complex level");break;
            case 7:action="Wait for actual board and its real start countdown";if(!report.board_started){var dialogue=Find("DialogueManager_OnBoard");if(dialogue!=null&&Bool(dialogue,"isRunning")){if(report.dialogue_clicks++>=30)throw new InvalidOperationException("Unexpected endless dialogue");Request(UiPoint(Callback("NextDialogue")),"advance actual dialogue");}next=7;}else Screenshot("board-started");break;
            case 8:action="Observe actual card background/canvas, wait for visible Ready card, then send OS input";ObserveBank();var bank=CardOnBank();Vector2 bankPoint;string bankDetail="No normal Ready plant card";if(bank!=null&&BankPoint(bank,out bankPoint,out bankDetail)){report.bank_card_instance=bank.GetInstanceID();plantsBeforePlacement=OnFieldCount();Request(bankPoint,"take real bank plant card ID "+Int(bank,"cardPlantType"));}else if(report.observed_game_seconds<60){next=8;}else{report.planting_blocked=true;report.planting_block_reason=bankDetail;CheckNow("normal_visible_ready_card_available",false,bankDetail);Event("BANK_SELECTION_BLOCKED_CONTINUE_NATURAL_OBSERVATION",0,0,0,bankDetail);next=11;}break;
            case 9:action="Observe hand state, then OS click an actual empty grass cell";var mouse=Find("MouseManager");CheckNow("plant_prefab_on_hand_after_os_bank_click",Live(Read(mouse,"plantPrefabOnHand")),"itemType="+F(mouse,"handItemType")+" plant="+F(mouse,"plantTypeOnHand"));if(Live(Read(mouse,"plantPrefabOnHand"))){Request(PlantingPoint(),"normal planting click");report.planting_attempts++;}else Event("PLANT_SELECTION_BLOCKED",0,0,0,"No live plant prefab after real bank-card input; combat observation continues");break;
            case 10:action="Observe actual placement result and natural combat without invoking unit updates or damage";report.plant_count_increased_after_input=OnFieldCount()>plantsBeforePlacement;CheckNow("plant_count_increased_after_os_planting",report.plant_count_increased_after_input,"before="+plantsBeforePlacement+" now="+OnFieldCount());Screenshot("planting-result");break;
            case 11:action="Continue actual complex level observation";ObserveBank();var board=Find("Board");if(board==null)throw new InvalidOperationException("Board disappeared during combat");if(!Bool(board,"isFinished")&&report.observed_game_seconds<150){next=11;break;}Screenshot("final");CheckNow("multiple_complex_zombie_types_observed",report.zombie_types_observed.Count>=3,"types="+String.Join(",",report.zombie_types_observed));CheckNow("plant_sourced_projectiles_observed",report.projectiles_with_live_plant_source>0,"count="+report.projectiles_with_live_plant_source);Finish(true,"COLLECTED_REQUIRES_TRAJECTORY_AND_DAMAGE_REVIEW");return;
            default:throw new InvalidOperationException("Unexpected gameplay stage");
            }
        }catch(Exception ex){while(ex is TargetInvocationException&&ex.InnerException!=null)ex=ex.InnerException;Log("Gameplay stage "+stage+" failed: "+ex.Message,ex.ToString(),LogType.Error);report.actions.Add(new ActionRow{utc=DateTime.UtcNow.ToString("o"),stage=stage,frame=Time.frameCount,action=action,scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,detail=ex.ToString()});Screenshot("blocked-stage-"+stage);Finish(false,"GAMEPLAY_BLOCKED_AT_STAGE_"+stage);return;}
        // Stage 11 is a wait; store only occasional progress rather than duplicating every frame.
        if(stage!=11||report.actions.Count==0||report.actions.Last().stage!=11)report.actions.Add(new ActionRow{utc=DateTime.UtcNow.ToString("o"),stage=stage,frame=Time.frameCount,action=action,scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene().name});
        Save();SessionState.SetInt(Stage,next);SessionState.SetString(Next,DateTime.UtcNow.AddSeconds(stage==11?5:1.5).Ticks.ToString());
    }
}
