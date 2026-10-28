using System;using System.IO;using System.Linq;using System.Reflection;using System.Security.Cryptography;using System.Collections.Generic;using UnityEngine;using UnityEngine.Rendering;
// Isolated real-engine SDK objects, never a combat unit or game save mutation.
public class GodsPVZNonComponentHolderR26:ScriptableObject {}
public static class GodsPVZSdkHolderAuditR26 {
 const string Expected="51f72bcd146c5571f642f3c4149f0545eb704fd325d83eb0f7c4ca2deec14c2b";
 [Serializable] public class Check{public string name,detail;public bool pass;}
 [Serializable] public class Report{public string scope="ACTUAL_LOADED_SDK_REAL_UNITY_OBJECT_HELPERS_NOT_GAMEPLAY",loaded_sha256,assembly_path;public bool pass,game_fields_written=false,combat_units_created=false;public List<Check> checks=new List<Check>();}
 static object Invoke(MethodInfo m,UnityEngine.Object value){try{return m.Invoke(null,new object[]{value});}catch(TargetInvocationException ex){if(ex.InnerException!=null)throw ex.InnerException;throw;}}
 static string Sha(string p){using(var s=SHA256.Create())return BitConverter.ToString(s.ComputeHash(File.ReadAllBytes(p))).Replace("-","").ToLowerInvariant();}
 public static void Run(string folder){
  Directory.CreateDirectory(folder);var r=new Report();GameObject go=null,componentOwner=null,dead=null;GodsPVZNonComponentHolderR26 other=null;
  Action<string,bool,string> check=(n,v,d)=>r.checks.Add(new Check{name=n,pass=v,detail=d});
  Action<string,Action> unsupported=(n,f)=>{try{f();check(n,false,"No exception");}catch(NotSupportedException){check(n,true,"Original NotSupportedException");}};
  try{
   var loaded=AppDomain.CurrentDomain.GetAssemblies().Where(a=>a.GetName().Name=="Unity.VisualScripting.Core").ToArray();if(loaded.Length>1)throw new InvalidOperationException("Ambiguous actually loaded Core assemblies");var asm=loaded.Length==1?loaded[0]:Assembly.Load("Unity.VisualScripting.Core");r.assembly_path=asm.Location;r.loaded_sha256=Sha(asm.Location);check("actual_loaded_sdk_hash",r.loaded_sha256==Expected,r.loaded_sha256);if(r.loaded_sha256!=Expected)throw new InvalidOperationException("Wrong actually loaded Core DLL");
   var t=asm.GetType("Unity.VisualScripting.ComponentHolderProtocol",true);var get=t.GetMethod("GameObject",BindingFlags.Public|BindingFlags.Static);var add=t.GetMethods(BindingFlags.Public|BindingFlags.Static).Single(m=>m.Name=="AddComponent"&&m.IsGenericMethodDefinition&&m.GetParameters().Length==1).MakeGenericMethod(typeof(SortingGroup));
   go=new GameObject("R26-Isolated-SDK-GameObject");go.hideFlags=HideFlags.HideAndDontSave;componentOwner=new GameObject("R26-Isolated-SDK-ComponentOwner");componentOwner.hideFlags=HideFlags.HideAndDontSave;var component=componentOwner.transform;other=ScriptableObject.CreateInstance<GodsPVZNonComponentHolderR26>();other.hideFlags=HideFlags.HideAndDontSave;
   check("getter_preserves_gameobject_identity",System.Object.ReferenceEquals(Invoke(get,go),go),"Actual engine GameObject");check("getter_resolves_component_owner",System.Object.ReferenceEquals(Invoke(get,component),componentOwner),"Actual engine Transform owner");check("getter_null_returns_null",Invoke(get,null)==null,"CLR null");check("getter_non_component_returns_null",Invoke(get,other)==null,"Actual ScriptableObject");
   var g1=Invoke(add,go)as SortingGroup;check("generic_add_on_gameobject",g1!=null&&g1.gameObject==go,"Actual native SortingGroup component");var g2=Invoke(add,component)as SortingGroup;check("generic_add_on_component_owner",g2!=null&&g2.gameObject==componentOwner,"Actual native component on Transform owner");unsupported("generic_add_null_throws",()=>Invoke(add,null));unsupported("generic_add_non_component_throws",()=>Invoke(add,other));
   dead=new GameObject("R26-Isolated-SDK-Destroyed");UnityEngine.Object.DestroyImmediate(dead);check("destroyed_gameobject_retains_managed_identity",System.Object.ReferenceEquals(Invoke(get,dead),dead),"Original is-type semantics, independent of Unity null equality");
   r.pass=r.checks.All(c=>c.pass);
  }catch(Exception ex){check("probe_execution",false,ex.ToString());r.pass=false;}
  finally{if(go!=null)UnityEngine.Object.DestroyImmediate(go);if(componentOwner!=null)UnityEngine.Object.DestroyImmediate(componentOwner);if(other!=null)UnityEngine.Object.DestroyImmediate(other);File.WriteAllText(Path.Combine(folder,"R26-ACTUAL-SDK-HELPERS.json"),JsonUtility.ToJson(r,true));}
  if(!r.pass)throw new InvalidOperationException("Actual SDK engine helper probe failed; see retained raw report");
 }
}
