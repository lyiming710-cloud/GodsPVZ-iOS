// Actual Editor UI diagnostic for the hash-bound, batch-tested candidate.
// Public game callbacks and real EventSystem raycasts only. No fixture data,
// game-field assignments, replacement resources, or exception suppression.
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class GodsPVZInteractionAuditR23
{
    const string Active="GodsPVZ.R20Interaction.Active", Stage="GodsPVZ.R20Interaction.Stage", Start="GodsPVZ.R20Interaction.Start", Next="GodsPVZ.R20Interaction.Next";
    const BindingFlags Fields=BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
    [Serializable] public class FieldRow { public string owner, instance, field, type, value; }
    [Serializable] public class EventRow { public string object_path, component, event_name, method, target; }
    [Serializable] public class Check { public string name, detail; public bool pass; }
    [Serializable] public class ActionRow { public int stage, frame; public string action, scene, exception; public List<FieldRow> fields=new List<FieldRow>(); }
    [Serializable] public class Report {
        public string scope="HASH_BOUND_ACTUAL_EDITOR_UI_PATH_DIAGNOSTIC_NOT_DEVICE_OR_GAME_ACCEPTANCE";
        public bool collection_complete, all_observed_checks_pass, device_execution=false, game_fields_seeded=false, replacement_game_code=false;
        public string save_scenario="FRESH_CI_SANDBOX_GAME_SAVE_CREATED_ONLY_BY_PUBLIC_GAME_API_IF_NEEDED", save_creation_route="Existing game state", candidate_sha256, workflow_head;
        public int first_plant_id=-1, first_selection_card_instance, dialogue_clicks, board_start_waits;
        public List<ActionRow> actions=new List<ActionRow>();
        public List<Check> checks=new List<Check>();
        public List<EventRow> registered_ui_events=new List<EventRow>();
        public List<string> runtime_errors=new List<string>(), environment_limits=new List<string>();
    }
    static Report report;
    static string Folder { get { var f=Environment.GetEnvironmentVariable("GODSPVZ_STARTUP_EVIDENCE"); if(String.IsNullOrEmpty(f))throw new InvalidOperationException("Evidence path required");return f; } }
    static string Output { get { string prefix=Environment.GetEnvironmentVariable("GODSPVZ_INTERACTION_PREFIX");if(String.IsNullOrEmpty(prefix))prefix="r23";if(prefix!="r23")throw new InvalidOperationException("Unexpected interaction evidence prefix");return Path.Combine(Folder,prefix+"-interaction-audit.json"); } }
    static Type TypeNamed(string name) { return AppDomain.CurrentDomain.GetAssemblies().Select(a=>a.GetType(name,false)).FirstOrDefault(t=>t!=null); }
    static object Read(object value,string field) { if(value==null)return null;var f=value.GetType().GetField(field,Fields);return f==null?null:f.GetValue(value); }
    static object Property(object value,string name) { if(value==null)return null;var p=value.GetType().GetProperty(name,BindingFlags.Public|BindingFlags.Instance|BindingFlags.Static);return p==null?null:p.GetValue(value,null); }
    static object Lawn() { var t=TypeNamed("GlobalStaticVars");return t==null?null:t.GetField("gLawnApp",Fields).GetValue(null); }
    static object Saves() { return Read(Lawn(),"savesManager"); }
    static MonoBehaviour[] SceneComponents() { return Resources.FindObjectsOfTypeAll<MonoBehaviour>().Where(c=>c!=null&&c.gameObject.scene.IsValid()).ToArray(); }
    static MonoBehaviour Find(string name,bool active=false) { return SceneComponents().Where(c=>c.GetType().FullName==name&&(!active||c.gameObject.activeInHierarchy)).OrderByDescending(c=>c.gameObject.activeInHierarchy).FirstOrDefault(); }
    static string PathOf(GameObject go) { if(go==null)return "null";var p=go.name;for(var t=go.transform.parent;t!=null;t=t.parent)p=t.name+"/"+p;return p; }
    static string Describe(object value) {
        if(value==null)return "null";
        var u=value as UnityEngine.Object;if(value is UnityEngine.Object)return u==null?"destroyed Unity object":u.GetType().FullName+":"+u.name+"#"+u.GetInstanceID();
        var list=value as IList;if(list!=null){var samples=new List<string>();for(int i=0;i<Math.Min(4,list.Count);i++){var x=list[i];samples.Add(x==null?"null":x.GetType().IsPrimitive||x is string?Convert.ToString(x,System.Globalization.CultureInfo.InvariantCulture):x.GetType().Name+"(ID="+Read(x,"ID")+",chosen="+Read(x,"isChosen")+")");}return "count="+list.Count+" ["+String.Join(",",samples.ToArray())+"]";}
        if(value.GetType().IsPrimitive||value is string||value.GetType().IsEnum)return Convert.ToString(value,System.Globalization.CultureInfo.InvariantCulture);
        return value.GetType().FullName+"#"+System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(value);
    }
    static void Observe(ActionRow row,object value,string owner,params string[] fields) {
        if(value==null){row.fields.Add(new FieldRow{owner=owner,field="<instance>",value="null"});return;}
        foreach(string name in fields){var f=value.GetType().GetField(name,Fields);object v=f==null?null:f.GetValue(value);row.fields.Add(new FieldRow{owner=owner,instance=Describe(value),field=name,type=f==null?"MISSING_FIELD":f.FieldType.FullName,value=Describe(v)});}
    }
    static void Snapshot(ActionRow row) {
        var saves=Saves();Observe(row,saves,"SavesManager","playerSave","saveList");
        var save=Read(saves,"playerSave");Observe(row,save,"Save","playerName","lenthOfSeedBank","adventureLevel","plantSaves","key_Tutorial","almanac_ZombieLock");
        Observe(row,Find("MainUIController"),"MainUIController","adventure","almanac","savesManager");
        Observe(row,Find("AlmanacUIController"),"AlmanacUIController","playerSave","plantList","plantDetail","zombieHomeUIController");
        Observe(row,Find("Almanac_PlantDetail"),"Almanac_PlantDetail","plantSave","plant","plantID","dataUIList");
        Observe(row,Find("Almanac_ZombieHome"),"Almanac_ZombieHome","zombieWindowList","zombiePortraits_Window");
        Observe(row,Find("PrepareUIController"),"PrepareUIController","playerSave","card_ChooseList","seedChooserScreen");
        Observe(row,Find("SeedChooserScreen"),"SeedChooserScreen","playerSave","prepareUIController","card_ChooseList");
        Observe(row,Find("Board"),"Board","gamePause","gameStart","gameSpeed","menuOpen","menu","enemyManager","zombieManager","mouseManager");
        Observe(row,Find("DialogueManager_OnBoard"),"DialogueManager_OnBoard","isRunning","dialogueIndex","dialogueTriggerType");
        Observe(row,Find("MouseManager"),"MouseManager","onBoard","operationCD","board","prepareUIController");
        foreach(var card in SceneComponents().Where(c=>c.GetType().FullName=="Card_Choose"&&c.gameObject.activeInHierarchy).Take(6))Observe(row,card,PathOf(card.gameObject),"plantID","isChosen","serialNumBeChosen","seedChooserScreen","almanacUIController");
        foreach(var window in SceneComponents().Where(c=>c.GetType().FullName=="Almanac_ZombieWindow"&&c.gameObject.activeInHierarchy).Take(3))Observe(row,window,PathOf(window.gameObject),"zombieID","zombiePortrait","questionMark","checkMark","eliteMark","bossMark","button");
        foreach(var c in SceneComponents().Where(c=>c.GetType().FullName=="Almanac_PlantDetail"||c.GetType().FullName=="Almanac_ZombieHome")){
            foreach(var f in c.GetType().GetFields(BindingFlags.Public|BindingFlags.Instance|BindingFlags.DeclaredOnly)){
                var value=f.GetValue(c);if(value==null||value is UnityEngine.Object||value is IList)row.fields.Add(new FieldRow{owner="DECLARED_REFERENCE_OBSERVATION",instance=Describe(c),field=f.Name,type=f.FieldType.FullName,value=Describe(value)});
            }
        }
        var resources=TypeNamed("ResourceManager");if(resources!=null)foreach(string name in new[]{"BGMs","boardClips","buttonClips","plantClips","deviceClips","zombieClips","projectClips","particleClips","mouseClips","dialogueClips"}){
            var f=resources.GetField(name,Fields);if(f!=null)row.fields.Add(new FieldRow{owner="ResourceManager",instance="static",field=name,type=f.FieldType.FullName,value=Describe(f.GetValue(null))});
        }
        foreach(var text in SceneComponents().Where(c=>c.gameObject.activeInHierarchy&&c.GetType().FullName=="TMPro.TextMeshProUGUI")){
            string value=Property(text,"text") as string;if(value!=null&&(value.Contains("NaN")||value.Contains("Infinity")))row.fields.Add(new FieldRow{owner="VISIBLE_NUMERIC_TEXT_ANOMALY",instance=PathOf(text.gameObject),field="text",type="System.String",value=value});
        }
    }
    static object Call(object target,string name,params object[] args) {
        if(target==null)throw new InvalidOperationException("No live target for "+name);
        var candidates=target.GetType().GetMethods(BindingFlags.Public|BindingFlags.Instance).Where(m=>m.Name==name&&m.GetParameters().Length==args.Length).ToArray();
        if(candidates.Length!=1)throw new InvalidOperationException("Public callback is ambiguous/missing: "+target.GetType().FullName+"::"+name);
        return candidates[0].Invoke(target,args);
    }
    static void CheckNow(string name,bool pass,string detail) { report.checks.Add(new Check{name=name,pass=pass,detail=detail}); }
    static bool ActiveObject(object value) { var c=value as Component;var g=value as GameObject;return c!=null?c.gameObject.activeInHierarchy:g!=null&&g.activeInHierarchy; }
    static MonoBehaviour Card(bool almanac) { return SceneComponents().FirstOrDefault(c=>c.GetType().FullName=="Card_Choose"&&c.gameObject.activeInHierarchy&&(almanac?Read(c,"almanacUIController")!=null:Read(c,"seedChooserScreen")!=null)); }
    static void RecordEvents() {
        foreach(var c in SceneComponents().Where(c=>c.gameObject.activeInHierarchy)) {
            var ev=Property(c,"onClick");if(ev!=null)Event(c,ev,"onClick");
            var triggers=Property(c,"triggers") as IEnumerable;if(triggers==null)continue;
            foreach(var entry in triggers){var callback=Read(entry,"callback");if(callback!=null)Event(c,callback,"EventTrigger:"+Read(entry,"eventID"));}
        }
    }
    static void Event(MonoBehaviour c,object ev,string name) {
        var count=ev.GetType().GetMethod("GetPersistentEventCount",System.Type.EmptyTypes);if(count==null)return;
        int n=(int)count.Invoke(ev,null);for(int i=0;i<n;i++){string method=(string)ev.GetType().GetMethod("GetPersistentMethodName").Invoke(ev,new object[]{i});var target=ev.GetType().GetMethod("GetPersistentTarget").Invoke(ev,new object[]{i});var row=new EventRow{object_path=PathOf(c.gameObject),component=c.GetType().FullName,event_name=name,method=method,target=Describe(target)};
            if(!report.registered_ui_events.Any(r=>r.object_path==row.object_path&&r.event_name==name&&r.method==method))report.registered_ui_events.Add(row);}
    }
    static bool HasCallback(object ev,string name) {
        if(ev==null)return false;var count=ev.GetType().GetMethod("GetPersistentEventCount",System.Type.EmptyTypes);if(count==null)return false;
        for(int i=0;i<(int)count.Invoke(ev,null);i++)if((string)ev.GetType().GetMethod("GetPersistentMethodName").Invoke(ev,new object[]{i})==name)return true;return false;
    }
    static GameObject CallbackObject(string name) {
        foreach(var c in SceneComponents().Where(c=>c.gameObject.activeInHierarchy)){
            if(HasCallback(Property(c,"onClick"),name))return c.gameObject;
            var entries=Property(c,"triggers") as IEnumerable;if(entries!=null)foreach(var e in entries)if(HasCallback(Read(e,"callback"),name))return c.gameObject;
        }
        return null;
    }
    static GameObject VisiblePointerTarget(string text) {
        var labels=SceneComponents().Where(c=>c.gameObject.activeInHierarchy&&System.Object.Equals(Property(c,"text"),text)).ToArray();
        if(labels.Length!=1)throw new InvalidOperationException("Visible label is missing/ambiguous: "+text);
        var click=TypeNamed("UnityEngine.EventSystems.IPointerClickHandler");var down=TypeNamed("UnityEngine.EventSystems.IPointerDownHandler");
        for(var t=labels[0].transform;t!=null;t=t.parent)if(t.GetComponents<MonoBehaviour>().Any(c=>c!=null&&((click!=null&&click.IsInstanceOfType(c))||(down!=null&&down.IsInstanceOfType(c)))))return t.gameObject;
        throw new InvalidOperationException("Visible label has no normal pointer handler: "+text);
    }
    static void RaycastClick(GameObject target,string label) {
        if(target==null)throw new InvalidOperationException("No live click target: "+label);
        var rect=target.GetComponent<RectTransform>();if(rect==null)throw new InvalidOperationException("Click target has no RectTransform");
        var canvas=target.GetComponentInParent<Canvas>();var camera=canvas==null||canvas.renderMode==RenderMode.ScreenSpaceOverlay?null:canvas.worldCamera;
        Vector2 point=RectTransformUtility.WorldToScreenPoint(camera,rect.TransformPoint(rect.rect.center));
        var esType=TypeNamed("UnityEngine.EventSystems.EventSystem");var es=esType==null?null:esType.GetProperty("current").GetValue(null,null);
        if(es==null)throw new InvalidOperationException("No active EventSystem");
        var dataType=TypeNamed("UnityEngine.EventSystems.PointerEventData");var data=Activator.CreateInstance(dataType,new object[]{es});
        dataType.GetProperty("position").SetValue(data,point,null);dataType.GetProperty("pressPosition").SetValue(data,point,null);
        dataType.GetProperty("button").SetValue(data,Enum.Parse(dataType.GetNestedType("InputButton"),"Left"),null);
        dataType.GetProperty("pointerId").SetValue(data,-1,null);dataType.GetProperty("eligibleForClick").SetValue(data,true,null);
        var resultType=TypeNamed("UnityEngine.EventSystems.RaycastResult");var resultList=(IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(resultType));
        esType.GetMethod("RaycastAll").Invoke(es,new object[]{data,resultList});
        var hit=resultList.Count==0?null:Property(resultList[0],"gameObject") as GameObject;
        if(hit==null&&resultList.Count>0)hit=Read(resultList[0],"gameObject") as GameObject;
        bool reaches=hit!=null&&(hit==target||hit.transform.IsChildOf(target.transform));
        CheckNow(label+"_raycast_reaches_card",reaches,"point="+point+" target="+PathOf(target)+" top="+PathOf(hit)+" module="+Describe(Property(es,"currentInputModule")));
        if(!reaches)return; // Never dispatch through an unrelated blocking overlay.
        var execute=TypeNamed("UnityEngine.EventSystems.ExecuteEvents");var method=execute.GetMethods(BindingFlags.Public|BindingFlags.Static).Single(m=>m.Name=="ExecuteHierarchy"&&m.IsGenericMethodDefinition);
        foreach(var pair in new[]{new[]{"IPointerDownHandler","pointerDownHandler"},new[]{"IPointerUpHandler","pointerUpHandler"},new[]{"IPointerClickHandler","pointerClickHandler"}}){var handler=TypeNamed("UnityEngine.EventSystems."+pair[0]);var functor=execute.GetProperty(pair[1],BindingFlags.Public|BindingFlags.Static).GetValue(null,null);method.MakeGenericMethod(handler).Invoke(null,new object[]{hit,data,functor});}
    }
    static void Log(string message,string stack,LogType type) {
        if(!EditorApplication.isPlaying||(type!=LogType.Error&&type!=LogType.Exception&&type!=LogType.Assert))return;
        if(message.StartsWith("FMOD failed to initialize the output device",StringComparison.Ordinal)){if(report.environment_limits.Count<10)report.environment_limits.Add(message);return;}
        if(report.runtime_errors.Count<120)report.runtime_errors.Add(message+"\n"+stack);
    }
    static void Save() { Directory.CreateDirectory(Folder);File.WriteAllText(Output,JsonUtility.ToJson(report,true)); }
    static void ObserveOriginalZombieBindings() {
        var prefab=Resources.Load<GameObject>("Prefabs/UI/SmallObjects/ZombieWindow");
        if(prefab==null){CheckNow("original_zombie_prefab_exists",false,"Missing normal Resource path");return;}
        var owner=prefab.GetComponents<MonoBehaviour>().SingleOrDefault(c=>c!=null&&c.GetType().FullName=="Almanac_ZombieWindow");
        if(owner==null){CheckNow("original_zombie_owner_exists",false,PathOf(prefab));return;}
        foreach(var binding in new[]{new[]{"zombiePortrait","Portrait","UnityEngine.UI.Image"},new[]{"questionMark","QMark","UnityEngine.UI.Image"},new[]{"checkMark","CMark","UnityEngine.UI.Image"},new[]{"eliteMark","EMark","UnityEngine.UI.Image"},new[]{"bossMark","BMark","UnityEngine.UI.Image"},new[]{"button","Portrait","UnityEngine.UI.Button"}}){
            var type=TypeNamed(binding[2]);var child=prefab.transform.Find(binding[1]);var expected=child==null||type==null?null:child.GetComponent(type);var field=owner.GetType().GetField(binding[0],Fields);var actual=field==null?null:field.GetValue(owner) as UnityEngine.Object;
            CheckNow("original_zombie_prefab_"+binding[0],field!=null&&field.FieldType==type&&expected!=null&&actual==expected,"actual="+Describe(actual)+" expected="+Describe(expected));
        }
        var manager=Resources.Load<GameObject>("prefabs/managers/ZombieManager");var ownerManager=manager==null?null:manager.GetComponents<MonoBehaviour>().FirstOrDefault(c=>c!=null&&c.GetType().FullName=="ZombieManager");string detail;bool audio=GodsPVZSerializedReferenceProbeR23.AudioMatches(Read(ownerManager,"audioClip_zombie"),out detail);CheckNow("original_zombie_manager_76_ordered_audio_slots",audio,detail);
    }
    public static void Begin() {
        if(SessionState.GetBool(Active,false))throw new InvalidOperationException("Existing interaction diagnostic active");
        if(File.Exists(Output))throw new InvalidOperationException("Never overwrite an earlier interaction report");
        string expected=Environment.GetEnvironmentVariable("GODSPVZ_INTERACTION_CANDIDATE_SHA256");if(String.IsNullOrEmpty(expected))expected="1d244776ee85083c2ed298c4f749f96b6c5ae820a78d50b0ce47480691775122";if(expected!="1d244776ee85083c2ed298c4f749f96b6c5ae820a78d50b0ce47480691775122")throw new InvalidOperationException("R23 fixed input required");
        var plugin=Path.Combine(Application.dataPath,"Plugins/GodsPVZRuntime1.dll");string actual;using(var h=System.Security.Cryptography.SHA256.Create())actual=BitConverter.ToString(h.ComputeHash(File.ReadAllBytes(plugin))).Replace("-","").ToLowerInvariant();
        if(actual!=expected)throw new InvalidOperationException("Interaction candidate hash mismatch");
        report=new Report{candidate_sha256=actual,workflow_head=Environment.GetEnvironmentVariable("GITHUB_SHA")};
        ObserveOriginalZombieBindings();
        Save();EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity",OpenSceneMode.Single);SessionState.SetBool(Active,true);SessionState.SetInt(Stage,0);SessionState.SetString(Start,DateTime.UtcNow.Ticks.ToString());SessionState.SetString(Next,DateTime.UtcNow.AddSeconds(8).Ticks.ToString());Attach();EditorApplication.isPlaying=true;
    }
    [InitializeOnLoadMethod] static void Resume() { if(SessionState.GetBool(Active,false)){report=File.Exists(Output)?JsonUtility.FromJson<Report>(File.ReadAllText(Output)):new Report();Attach();} }
    static void Attach() { EditorApplication.update-=Tick;EditorApplication.update+=Tick;Application.logMessageReceived-=Log;Application.logMessageReceived+=Log; }
    static void Finish(bool complete) {try{GodsPVZSerializedReferenceProbeR23.CaptureLoadedSupportAfterUI();}catch(Exception e){report.environment_limits.Add("Actual loaded support code observation failed: "+e);}report.collection_complete=complete;report.all_observed_checks_pass=complete&&report.checks.Count>0&&report.checks.All(c=>c.pass)&&report.runtime_errors.Count==0;Save();SessionState.SetBool(Active,false);EditorApplication.update-=Tick;Application.logMessageReceived-=Log;EditorApplication.Exit(complete?0:79); }
    static void Tick() {
        if(!SessionState.GetBool(Active,false))return;
        long now=DateTime.UtcNow.Ticks;if(now-Int64.Parse(SessionState.GetString(Start,"0"))>TimeSpan.FromSeconds(210).Ticks){report.runtime_errors.Add("Interaction diagnostic timeout");Finish(false);return;}
        if(!EditorApplication.isPlaying||now<Int64.Parse(SessionState.GetString(Next,"0")))return;
        int stage=SessionState.GetInt(Stage,0),next=stage+1;var row=new ActionRow{stage=stage,frame=Time.frameCount,scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene().name};
        try {
            switch(stage) {
            case 0:row.action="Observe actual startup and persistent UI registrations";RecordEvents();break;
            case 1:row.action="Use current save or normal public game save-creation path in fresh CI only";if(Read(Saves(),"playerSave")==null){var popup=Find("Window_I",true);if(popup!=null&&(System.Object.Equals(Read(popup,"I"),1)||System.Object.Equals(Read(popup,"I"),2))){report.save_creation_route="Actual Window_I.Input and Yes public callbacks";Call(popup,"Input","R20-Interaction-CI");Call(popup,"Yes");}else{report.save_creation_route="Separate real SavesManager.CreateNewPlayerSave public API diagnostic";Call(Saves(),"CreateNewPlayerSave","R20-Interaction-CI");}}break;
            case 2:row.action="Observe save initialization";CheckNow("live_player_save",Read(Saves(),"playerSave")!=null,Describe(Read(Saves(),"playerSave")));Call(Find("MainUIController"),"LoadData");break;
            case 3:row.action="Public MainUIController.AlmanacOnclick then normal OpenPlantHome";Call(Find("MainUIController"),"AlmanacOnclick");Call(Find("AlmanacUIController"),"OpenPlantHome");break;
            case 4:row.action="Real EventSystem raycast and pointer click on first live almanac card";RecordEvents();var card=Card(true);if(card==null)throw new InvalidOperationException("No live almanac card");report.first_plant_id=Convert.ToInt32(Read(card,"plantID"));RaycastClick(card.gameObject,"almanac");break;
            case 5:row.action="Observe almanac detail after pointer delivery";CheckNow("almanac_plant_detail_visible_after_pointer",ActiveObject(Find("Almanac_PlantDetail")),Describe(Find("Almanac_PlantDetail")));break;
            case 6:row.action="Separate public OpenPlant callback diagnostic with same observed plant ID";if(report.first_plant_id<0)throw new InvalidOperationException("No observed plant ID");Call(Find("AlmanacUIController"),"OpenPlant",report.first_plant_id);break;
            case 7:row.action="Observe direct OpenPlant result and return to almanac home";CheckNow("almanac_plant_detail_visible_after_public_callback",ActiveObject(Find("Almanac_PlantDetail")),Describe(Find("Almanac_PlantDetail")));Call(Find("AlmanacUIController"),"ReturnHome");break;
            case 8:row.action="Public AlmanacUIController.OpenZombieHome";Call(Find("AlmanacUIController"),"OpenZombieHome");break;
            case 9:row.action="Observe zombie windows and return to main menu";var windows=Read(Find("Almanac_ZombieHome"),"zombieWindowList") as IList;CheckNow("zombie_window_list_populated",windows!=null&&windows.Count>0,Describe(windows));Call(Find("AlmanacUIController"),"ReturnMain");break;
            case 10:row.action="Public MainUIController.AdventureButtonOnClick";Call(Find("MainUIController"),"AdventureButtonOnClick");break;
            case 11:row.action="Public AdventureScreen.Enter_Adventure";Call(Find("AdventureScreen"),"Enter_Adventure");break;
            case 12:row.action="Observe actual preparation UI, then public SeedChoose";CheckNow("prepare_screen_visible",ActiveObject(Find("PrepareUIController")),Describe(Find("PrepareUIController")));Call(Find("PrepareUIController"),"SeedChoose");break;
            case 13:row.action="Real EventSystem pointer click on first selection card";RecordEvents();var seed=Card(false);if(seed==null)throw new InvalidOperationException("No live seed-selection card");report.first_selection_card_instance=seed.GetInstanceID();RaycastClick(seed.gameObject,"seed_selection");break;
            case 14:row.action="Observe same clicked card isChosen then separate public ChooseThis diagnostic";var current=SceneComponents().FirstOrDefault(c=>c.GetInstanceID()==report.first_selection_card_instance);CheckNow("seed_selected_after_pointer",current!=null&&System.Object.Equals(Read(current,"isChosen"),true),Describe(Read(current,"isChosen")));if(current==null)throw new InvalidOperationException("Clicked seed card no longer exists");if(!System.Object.Equals(Read(current,"isChosen"),true))Call(current,"ChooseThis");break;
            case 15:row.action="Real EventSystem pointer click on registered Determine button";RaycastClick(CallbackObject("Determine"),"determine");break;
            case 16:row.action="Observe confirmation transition, separate public Determine if selection stayed visible";CheckNow("prepare_visible_after_pointer_confirmation",ActiveObject(Find("PrepareUIController")),Describe(Find("PrepareUIController")));CheckNow("selection_closed_after_pointer_confirmation",!ActiveObject(Find("SeedChooserScreen")),Describe(Find("SeedChooserScreen")));if(ActiveObject(Find("SeedChooserScreen")))Call(Find("SeedChooserScreen"),"Determine");break;
            case 17:row.action="Public PrepareUIController.GameStart";Call(Find("PrepareUIController"),"GameStart");break;
            case 18:row.action="Wait for actual board start or real running intro dialogue, without setting game fields";var boardReady=Find("Board",true);if(report.board_start_waits==0)CheckNow("board_exists_after_start",boardReady!=null,Describe(boardReady));var intro=Find("DialogueManager_OnBoard");bool began=System.Object.Equals(Read(boardReady,"gameStart"),true)||System.Object.Equals(Read(intro,"isRunning"),true);if(!began&&report.board_start_waits++<20)next=18;else CheckNow("board_start_or_dialogue_began",began,"waits="+report.board_start_waits);break;
            case 19:row.action="Public NextDialogue for actual running dialogue, bounded normal advancement";var dialogue=Find("DialogueManager_OnBoard");if(dialogue!=null&&System.Object.Equals(Read(dialogue,"isRunning"),true)&&report.dialogue_clicks<30){Call(dialogue,"NextDialogue");report.dialogue_clicks++;next=19;}break;
            case 20:row.action="Real EventSystem pointer click at original source-bound Board menu path";RecordEvents();var boardMenu=Find("Board",true);var sourceButton=boardMenu==null?null:boardMenu.transform.Find("Windows/Button/MenuButton");CheckNow("board_game_started_before_menu",System.Object.Equals(Read(boardMenu,"gameStart"),true),Describe(Read(boardMenu,"gameStart")));RaycastClick(sourceButton==null?CallbackObject("ButtonDown_Menu"):sourceButton.gameObject,"board_menu");break;
            case 21:row.action="Observe pointer menu result; separate public callback if menu stayed hidden";CheckNow("board_menu_visible_after_pointer",ActiveObject(Read(Find("Board"),"menu")),Describe(Read(Find("Board"),"menu")));if(!ActiveObject(Read(Find("Board"),"menu")))Call(Find("Board"),"ButtonDown_Menu");break;
            case 22:row.action="Observe direct menu result and request normal return-main confirmation";CheckNow("board_menu_visible_after_callback",ActiveObject(Read(Find("Board"),"menu")),Describe(Read(Find("Board"),"menu")));Call(Find("Board"),"ReturnMain");break;
            case 23:row.action="Public active Window_Q.Yes for game-created return confirmation";Call(Find("Window_Q",true),"Yes");break;
            case 24:row.action="Observe final main-menu scene";CheckNow("mainmenu_returned",UnityEngine.SceneManagement.SceneManager.GetActiveScene().name=="MainMenu",UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);Snapshot(row);report.actions.Add(row);Finish(true);return;
            default:throw new InvalidOperationException("Unexpected diagnostic stage");
            }
        } catch(Exception ex) { while(ex is TargetInvocationException&&ex.InnerException!=null)ex=ex.InnerException;row.exception=ex.ToString();CheckNow("stage_"+stage+"_callback_completed",false,ex.ToString()); }
        try {Snapshot(row);}catch(Exception ex){row.exception+=("\nObservation error: "+ex);}
        report.actions.Add(row);Save();SessionState.SetInt(Stage,next);SessionState.SetString(Next,DateTime.UtcNow.AddSeconds(2).Ticks.ToString());
    }
}
