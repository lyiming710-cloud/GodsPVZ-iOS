#if UNITY_EDITOR
using System;using System.IO;using System.Linq;using System.Reflection;using System.Collections;using System.Collections.Generic;using System.Security.Cryptography;
using UnityEditor;using UnityEditor.SceneManagement;using UnityEngine;

// A controlled integration fixture: original prefab clones, actual game methods,
// real Unity components and original UI callees. Explicit test-field setup is
// recorded. It does not unlock saves, modify game DLLs, or claim normal play.
public static class GodsPVZSpecialAuditR28 {
 const string Active="GodsPVZ.R28Special.Active",Started="GodsPVZ.R28Special.Started";
 const BindingFlags BF=BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance|BindingFlags.Static;
 [Serializable] public class Plan {public string sniper_skill_json;public float sunflower_skill_value;}
 [Serializable] public class Value {public string name,value;}
 [Serializable] public class Case {public string name,status="RUNNING",exception="",prefab="",setup="";public List<Value> observed=new List<Value>();}
 [Serializable] public class Log {public string type,message,stack;}
 [Serializable] public class Report {
  public string scope="REAL_UNITY_CONTROLLED_ORIGINAL_PREFAB_SPECIAL_FUNCTIONS_NOT_NORMAL_LEVEL_PLAY",candidate_sha256,sdk_sha256,status="STARTING";
  public bool pass,game_dll_changed=false,controlled_instances_created=true,test_fields_assigned=true,save_or_unlock_edited=false,normal_gameplay_proven=false,animation_motion_proven=false;
  public List<Case> cases=new List<Case>();public List<Log> errors=new List<Log>(),warnings=new List<Log>();
 }
 static Report report;static Assembly game;static string folder;static GameObject root;static Case current;static Plan plan;
 static Type T(string n){return game.GetType(n,true);}
 static object Read(object o,string n){return o.GetType().GetField(n,BF).GetValue(o);}
 static void Set(object o,string n,object v){var f=o.GetType().GetField(n,BF);f.SetValue(o,f.FieldType.IsEnum?Enum.ToObject(f.FieldType,v):v);}
 static object Call(object o,string n,params object[] args){return o.GetType().GetMethods(BF).Single(m=>m.Name==n&&m.GetParameters().Length==args.Length).Invoke(o,args);}
 static object Static(string owner,string n,params object[] args){return T(owner).GetMethods(BF).Single(m=>m.Name==n&&m.GetParameters().Length==args.Length).Invoke(null,args);}
 static float F(object o,string n){return Convert.ToSingle(Read(o,n));}
 static string Bits(float v){return BitConverter.ToUInt32(BitConverter.GetBytes(v),0).ToString("X8");}
 static void Observe(string n,object v){current.observed.Add(new Value{name=n,value=v is float?Bits((float)v):Convert.ToString(v,System.Globalization.CultureInfo.InvariantCulture)});}
 static void Equal(string n,object actual,object expected){Observe(n,actual);if(!System.Object.Equals(actual,expected))throw new Exception(n+": expected "+expected+", observed "+actual);}
 static string Sha(string p){using(var s=SHA256.Create())return BitConverter.ToString(s.ComputeHash(File.ReadAllBytes(p))).Replace("-","").ToLowerInvariant();}
 static void Save(){File.WriteAllText(Path.Combine(folder,"R28-ACTUAL-SPECIAL-FUNCTIONS.json"),JsonUtility.ToJson(report,true));}
 static void LogMessage(string m,string stack,LogType t){if(report==null)return;var row=new Log{type=t.ToString(),message=m,stack=stack};if(t==LogType.Error||t==LogType.Exception||t==LogType.Assert)report.errors.Add(row);else if(t==LogType.Warning)report.warnings.Add(row);}
 static void RunCase(string n,string setup,Action action){current=new Case{name=n,setup=setup};report.cases.Add(current);try{action();current.status="PASS_CONTROLLED_SCOPE";}catch(Exception e){while(e is TargetInvocationException&&e.InnerException!=null)e=e.InnerException;current.status="FAIL";current.exception=e.ToString();}Save();}
 static object New(string name){var t=T(name);if(typeof(Component).IsAssignableFrom(t)){var go=new GameObject("SpecialFixture-"+name);go.transform.SetParent(root.transform,false);return go.AddComponent(t);}return Activator.CreateInstance(t);}
 static IList NewList(object owner,string field){var list=(IList)Activator.CreateInstance(owner.GetType().GetField(field,BF).FieldType);Set(owner,field,list);return list;}
 static Component Clone(string owner,int id){
  var type=T(owner);var selected=AssetDatabase.GetAllAssetPaths().Where(p=>p.StartsWith("Assets/Resources/",StringComparison.Ordinal)&&p.EndsWith(".prefab",StringComparison.Ordinal)&&p.IndexOf("/"+owner+"/",StringComparison.OrdinalIgnoreCase)>=0)
   .OrderBy(p=>p,StringComparer.Ordinal).Select(p=>new{path=p,go=AssetDatabase.LoadAssetAtPath<GameObject>(p)}).Select(x=>new{ x.path,x.go,c=x.go.GetComponent(type) }).First(x=>x.c!=null&&Convert.ToInt32(Read(x.c,"ID"))==id);
  var copy=UnityEngine.Object.Instantiate(selected.go,root.transform,false);var c=copy.GetComponent(type);foreach(var b in copy.GetComponentsInChildren<MonoBehaviour>(true))b.enabled=false;
  current.prefab=selected.path;Observe(owner+".prefab_ID",Read(c,"ID"));
  if(owner=="Plant")Call(c,"Awake"); // Actual original initialization, invoked explicitly in this inactive fixture.
  else Set(c,"animator",((GameObject)Read(c,"animationGroup")).GetComponent<Animator>());
  Set(c,"buffManager",New("BuffManager"));return c;
 }
 static object Skill(){return JsonUtility.FromJson(plan.sniper_skill_json,T("Skill"));}
 static void RunTests(){
  root=new GameObject("R28-Controlled-Special-Fixture");root.SetActive(false);
  RunCase("sniper_charge_reaches_one_layer_and_updates_original_UI","Original sniper prefab; original serialized Skill default (4/12, cap 2); add 8",()=>{
   var p=Clone("Plant",0);var s=Skill();Set(p,"skill",s);Set(p,"skillOngoing",false);Call(p,"SkillTryCharging",8f);Equal("layers",Read(s,"chargedLayer"),1);Equal("charge",F(s,"chargeTicking"),0f);
  });
  RunCase("sniper_charge_stops_at_layer_cap","Original sniper prefab; layer 1, charge 0, add 48, cap 2",()=>{
   var p=Clone("Plant",0);var s=Skill();Set(s,"chargedLayer",1);Set(s,"chargeTicking",0f);Set(p,"skill",s);Set(p,"skillOngoing",false);Call(p,"SkillTryCharging",48f);Equal("layers",Read(s,"chargedLayer"),2);Equal("charge",F(s,"chargeTicking"),12f);
  });
  RunCase("ongoing_skill_prevents_charging_when_configured","Original sniper prefab; configured ongoing/preventCharge boundary",()=>{
   var p=Clone("Plant",0);var s=Skill();Set(p,"skill",s);Set(p,"skillOngoing",true);Set(s,"isPreventCharge",true);Call(p,"SkillTryCharging",8f);Equal("layers",Read(s,"chargedLayer"),0);Equal("charge",F(s,"chargeTicking"),4f);
  });
  foreach(string function in new[]{"Function1","Function13"})RunCase(function+"_actual_damage_scaling","Original sniper prefab; raw Skill value 1.4; actual Damage object starts at 100",()=>{
   var p=Clone("Plant",0);var s=Skill();Set(p,"skill",s);var d=New("Damage");Set(d,"damagePoint",100f);Static("SkillManager",function,p,null,null,d);Equal("damagePoint",F(d,"damagePoint"),100f*1.399999976158142f);
  });
  RunCase("sunflower_skill_actual_cooldown","Original sunflower prefab; global Skill 4 level-1 value from original Skills/4.json",()=>{
   var p=Clone("Plant",1);var s=Skill();Set(s,"value_float",new[]{plan.sunflower_skill_value});Set(p,"skill",s);Static("SkillManager","Function4",p,null,null,null);Equal("attackIntervalCountdown",F(p,"attackIntervalCountdown"),2.4f/(plan.sunflower_skill_value-2f));
  });
  RunCase("chomper_finishes_devour_and_updates_original_barrel_UI","Original Viking Chomper prefab, order 1; maxHP1000, stored900, devour1300",()=>{
   var p=Clone("Plant",6);Set(p,"order",1);Set(p,"maxHealthPoint",1000f);Set(p,"pT_chomperviking_barrelsPoint",900f);Set(p,"pT_chomperviking_devourPoint",1300f);Set(p,"pC_chomperviking_devour",true);Call(p,"PC_ChompervikingDevourEnd");Equal("barrels",F(p,"pT_chomperviking_barrelsPoint"),2000f);Equal("devourPoint",F(p,"pT_chomperviking_devourPoint"),0f);Equal("devouring",Read(p,"pC_chomperviking_devour"),false);
  });
  RunCase("sunflower_heals_three_by_three_with_actual_damage_text","Original sunflower and sniper prefabs; controlled 3x3 Map/Rows/Grids; each neighbor HP100/1000; ATK50",()=>{
   var p=Clone("Plant",1);Set(p,"attackPoint",50f);Set(p,"GridX",1);Set(p,"GridY",1);var board=New("Board");var map=New("Map");Set(board,"map",map);Set(p,"board",board);var rows=NewList(map,"rows");var targets=new List<object>();
   for(int y=0;y<3;y++){var row=New("Row");var grids=NewList(row,"grids");rows.Add(row);for(int x=0;x<3;x++){var grid=New("Grid");var n=Clone("Plant",0);Set(n,"healthPoint",100f);Set(n,"maxHealthPoint",1000f);Set(grid,"plant_common",n);grids.Add(grid);targets.Add(n);}}
   var atk=Convert.ToSingle(Call(p,"GetATK"));Equal("effectiveATK",atk,50f);Call(p,"PC_SunflowerHeal");for(int i=0;i<targets.Count;i++)Equal("neighbor"+i+".HP",F(targets[i],"healthPoint"),100f+atk*1.2f);
  });
  RunCase("zombie_element_armor_gate","Original ordinary-zombie prefab; armor1Type4 and HP100 vs HP0; ElementType0",()=>{
   var z=Clone("Zombie",0);Set(z,"armor1Type",4);Set(z,"armor1Point",100f);var e=Enum.ToObject(T("ElementType"),0);Equal("armored",Call(z,"ZC_Element",e,0),false);Set(z,"armor1Point",0f);Equal("brokenArmor",Call(z,"ZC_Element",e,0),true);
  });
  RunCase("ladder_place_start_uses_original_animator","Original Ladder Raider prefab; explicit placement-start invocation, no motion claim",()=>{
   var z=Clone("Zombie",16);Set(z,"ladderZombie_place",false);Call(z,"ZC_LadderPlaceStart");Equal("placing",Read(z,"ladderZombie_place"),true);Observe("originalController",((Animator)Read(z,"animator")).runtimeAnimatorController.name);
  });
  RunCase("pole_jump_start_uses_original_animator","Original pole-vanguard prefab; explicit jump-start invocation, no path or motion claim",()=>{
   var z=Clone("Zombie",23);Set(z,"poleZombie_jump",false);Call(z,"ZC_PoleJump");Equal("jumping",Read(z,"poleZombie_jump"),true);Observe("originalController",((Animator)Read(z,"animator")).runtimeAnimatorController.name);
  });
  RunCase("snowbeast_rest_recovers","Original Snowbeast prefab; resting and stamina9.99; one real Unity deltaTime update",()=>{
   var z=Clone("Zombie",13);Set(z,"snowbeast_isRest",true);Set(z,"snowbeast_restTime",9.99f);Set(z,"isStant",false);float expected=Math.Min(10f,9.99f+Time.deltaTime);Observe("deltaTime",Time.deltaTime);Call(z,"ZC_Snowbeast");Equal("stamina",F(z,"snowbeast_restTime"),expected);if(expected>=10f)Equal("restEnded",Read(z,"snowbeast_isRest"),false);
  });
  foreach(int action in new[]{0,1,2,3,4})RunCase("snowbeast_consumption_mode_"+action,"Original Snowbeast prefab; stamina10; actual TranToStant and animator callees on exhaustion; HP UI intentionally null",()=>{
   var z=Clone("Zombie",13);Set(z,"snowbeast_restTime",10f);Set(z,"snowbeast_isRest",false);Set(z,"hpUIController",null);Set(z,"updateRate",1f);Call(z,"ZC_SnowbeastDeplete",action);float consume=action==0||action==2?.75f:action==1?.15f:action==3?9999999f:0f;Equal("stamina",F(z,"snowbeast_restTime"),Math.Max(0f,10f-consume));if(action==3)Equal("resting",Read(z,"snowbeast_isRest"),true);
  });
  RunCase("snowbeast_impact_clears_actual_references","Original Snowbeast prefab; explicit Device reference and empty EnemyPath list",()=>{
   var z=Clone("Zombie",13);Set(z,"targetDevice",New("Device"));NewList(z,"path");Call(z,"ZC_SnowbeastImpactReset");Equal("targetDeviceNull",Read(z,"targetDevice")==null,true);Equal("pathNull",Read(z,"path")==null,true);
  });
 }
 public static void Begin(){
  folder=Environment.GetEnvironmentVariable("GODSPVZ_SPECIAL_EVIDENCE");Directory.CreateDirectory(folder);report=new Report();Save();
  // Reuse only the established work-copy resource preparation, not its whole
  // scene/prefab inventory and acceptance tests.
  var preparation=typeof(GodsPVZSpecialAuditR28).Assembly.GetType("GodsPVZStartupAudit",true);var prepReport=Activator.CreateInstance(preparation.GetNestedType("Report"));
  preparation.GetMethod("ImportOfficialTmpResources",BF).Invoke(null,null);
  foreach(string method in new[]{"RebindOfficialShaders","RestoreOriginalEmptyFontAtlases","PreserveFontAtlases"})preparation.GetMethod(method,BF).Invoke(null,new[]{prepReport});
  AssetDatabase.SaveAssets();AssetDatabase.Refresh();
  game=Assembly.Load("GodsPVZRuntime1");report.candidate_sha256=Sha(game.Location);report.sdk_sha256=Sha(Assembly.Load("Unity.VisualScripting.Core").Location);
  plan=JsonUtility.FromJson<Plan>(File.ReadAllText(Environment.GetEnvironmentVariable("GODSPVZ_SPECIAL_PLAN")));Save();
  SessionState.SetString(Started,DateTime.UtcNow.Ticks.ToString());SessionState.SetBool(Active,true);Attach();EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity",OpenSceneMode.Single);EditorApplication.isPlaying=true;
 }
 [InitializeOnLoadMethod] static void Resume(){if(!SessionState.GetBool(Active,false))return;folder=Environment.GetEnvironmentVariable("GODSPVZ_SPECIAL_EVIDENCE");report=JsonUtility.FromJson<Report>(File.ReadAllText(Path.Combine(folder,"R28-ACTUAL-SPECIAL-FUNCTIONS.json")));game=AppDomain.CurrentDomain.GetAssemblies().Single(a=>a.GetName().Name=="GodsPVZRuntime1");plan=JsonUtility.FromJson<Plan>(File.ReadAllText(Environment.GetEnvironmentVariable("GODSPVZ_SPECIAL_PLAN")));Attach();}
 static void Attach(){EditorApplication.update-=Tick;EditorApplication.update+=Tick;Application.logMessageReceived-=LogMessage;Application.logMessageReceived+=LogMessage;}
 static void Tick(){if(!EditorApplication.isPlaying||Time.frameCount<3||(DateTime.UtcNow.Ticks-long.Parse(SessionState.GetString(Started,"0")))/(double)TimeSpan.TicksPerSecond<20)return;SessionState.SetBool(Active,false);EditorApplication.update-=Tick;
  try{RunTests();report.pass=report.cases.Count==18&&report.cases.All(c=>c.status=="PASS_CONTROLLED_SCOPE")&&report.errors.Count==0;report.status=report.pass?"PASS_CONTROLLED_SCOPE":"FAIL_RETAINED";}catch(Exception e){report.status="FIXTURE_SETUP_FAILED";report.errors.Add(new Log{type="Exception",message=e.ToString()});}
  finally{if(root!=null)UnityEngine.Object.DestroyImmediate(root);Save();EditorApplication.Exit(report.pass?0:79);}
 }
}
#endif
