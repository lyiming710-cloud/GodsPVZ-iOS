using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

if (args.Length != 2) { Console.Error.WriteLine("Usage: GodsPVZ.HighFidelityPatch <input.dll> <output.dll>"); return 2; }
var input = Path.GetFullPath(args[0]); var output = Path.GetFullPath(args[1]);
using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { InMemory = true, ReadSymbols = false });

IEnumerable<TypeDefinition> All(IEnumerable<TypeDefinition> roots){ foreach(var t in roots){ yield return t; foreach(var n in All(t.NestedTypes)) yield return n; } }
TypeDefinition T(string n)=>All(module.Types).Single(x=>x.Name==n||x.FullName==n);
FieldDefinition F(TypeDefinition t,string n)=>t.Fields.Single(x=>x.Name==n);
MethodDefinition M(TypeDefinition t,string n,int pc){var q=t.Methods.Where(x=>x.Name==n&&x.Parameters.Count==pc).ToList();return q.Count==1?q[0]:throw new Exception($"Expected one {t.FullName}.{n}/{pc}, got {q.Count}");}
MethodBody Fresh(MethodDefinition m,bool init=false){var b=new MethodBody(m){InitLocals=init,MaxStackSize=64};m.Body=b;return b;}
void E(ILProcessor il,OpCode op,object? v=null){Instruction x=v switch{null=>Instruction.Create(op),string s=>Instruction.Create(op,s),int i=>Instruction.Create(op,i),float f=>Instruction.Create(op,f),MethodReference mr=>Instruction.Create(op,mr),FieldReference fr=>Instruction.Create(op,fr),TypeReference tr=>Instruction.Create(op,tr),ParameterDefinition pd=>Instruction.Create(op,pd),VariableDefinition vd=>Instruction.Create(op,vd),Instruction ins=>Instruction.Create(op,ins),_=>throw new NotSupportedException(v.GetType().FullName)};il.Append(x);}
IEnumerable<MethodReference> AllRefs()=>All(module.Types).SelectMany(t=>t.Methods).Where(m=>m.HasBody).SelectMany(m=>m.Body.Instructions).Select(i=>i.Operand).OfType<MethodReference>();
MethodReference AnyCall(string decl,string name,int pc){var q=AllRefs().Where(x=>x.DeclaringType.FullName.Contains(decl)&&x.Name==name&&x.Parameters.Count==pc).ToList();if(q.Count==0)throw new Exception($"Missing MethodRef {decl}.{name}/{pc}");return q[0];}
GenericInstanceMethod AnyGeneric(string decl,string name,string ga){var q=AllRefs().OfType<GenericInstanceMethod>().Where(x=>x.DeclaringType.FullName.Contains(decl)&&x.Name==name&&x.GenericArguments.Any(a=>a.Name==ga||a.FullName==ga)).ToList();if(q.Count==0)throw new Exception($"Missing Generic MethodRef {decl}.{name}<{ga}>");return q[0];}
MethodReference ListMethod(GenericInstanceType list,string name,TypeReference ret,params TypeReference[] ps){var mr=new MethodReference(name,ret,list){HasThis=true};foreach(var p in ps)mr.Parameters.Add(new ParameterDefinition(p));return mr;}

var plant=T("Plant"); var plantManager=T("PlantManager"); var board=T("Board"); var boardConfig=T("BoardConfig"); var grid=T("Grid"); var skill=T("Skill");
var card=T("Card"); var mouse=T("MouseManager"); var sunManager=T("SunManager"); var banks=T("Banks"); var globals=T("GlobalStaticVars");

PatchPlantAwake();
PatchPlantFixedUpdate();
PatchPlantPlanting();
PatchPlantTryPlanting();
PatchPlantManagerAddPlant();
PatchMouseDownUpdate();
PatchCardHelpers();
PatchCardUpdate();
PatchCardStateSet();
PatchCardOnClick();
PatchCardOfBankPlanting();

Directory.CreateDirectory(Path.GetDirectoryName(output)!);
module.Write(output,new WriterParameters{WriteSymbols=false});
using(var verify=ModuleDefinition.ReadModule(output,new ReaderParameters{InMemory=true,ReadSymbols=false})){
 var specs=new[]{
  ("Plant","Awake",0),("Plant","FixedUpdate",0),("Plant","Planting",2),("Plant","TryPlanting",2),("PlantManager","AddPlant",1),
  ("MouseManager","MouseDownUpdate",0),("Card","Update",0),("Card","CardStateSet",0),("Card","CardOnClick",0),("Card","CardOfBankPlanting",0),
  ("Card","CoolingUpdate",0),("Card","WaitingSunUpdate",0),("Card","ReadyUpdate",0),("Card","OnHeadUpdate",0),("Card","SunUITextUpdate",0)
 };
 foreach(var s in specs){var tt=All(verify.Types).Single(t=>t.Name==s.Item1||t.FullName==s.Item1);var mm=tt.Methods.Single(m=>m.Name==s.Item2&&m.Parameters.Count==s.Item3);if(!mm.HasBody||mm.Body.Instructions.Count<2)throw new Exception($"Bad body {s}");Console.WriteLine($"VERIFY {s.Item1}.{s.Item2}: {mm.Body.Instructions.Count} IL");}
}
Console.WriteLine($"WROTE {output} ({new FileInfo(output).Length} bytes)");
return 0;

void PatchPlantAwake(){
 var m=M(plant,"Awake",0); var b=Fresh(m); var il=b.GetILProcessor();
 var animGroup=F(plant,"animationGroup"); var animator=F(plant,"animator"); var sprites=F(plant,"animationSprites"); var aniPart=F(plant,"ani_aniPart");
 var getAnimator=AnyGeneric("UnityEngine.Component","GetComponent","Animator"); var list=(GenericInstanceType)sprites.FieldType; var clear=ListMethod(list,"Clear",module.TypeSystem.Void); var getTransform=AnyCall("UnityEngine.Component","get_transform",0); var loop=M(plant,"LoopAddAnimation",1);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,animGroup);E(il,OpCodes.Callvirt,getAnimator);E(il,OpCodes.Stfld,animator);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,sprites);E(il,OpCodes.Callvirt,clear);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,aniPart);E(il,OpCodes.Callvirt,getTransform);E(il,OpCodes.Call,loop);E(il,OpCodes.Ret);
 Console.WriteLine("HF Plant.Awake @0x18034DEA0: GetComponent<Animator>, List.Clear, LoopAddAnimation");
}

void PatchPlantFixedUpdate(){
 var m=M(plant,"FixedUpdate",0); var b=Fresh(m); var il=b.GetILProcessor();
 var fBoard=F(plant,"board"); var fActive=F(plant,"active"); var fOn=F(plant,"isOnField"); var fSleep=F(plant,"isSleep"); var fID=F(plant,"ID"); var fState=F(plant,"state");
 var updateColor=M(plant,"Update_Color",0); var dith=M(plant,"Dithering_Animation",1); var gameStart=F(board,"gameStart"); var gamePause=F(board,"gamePause");
 var afterColor=Instruction.Create(OpCodes.Nop); var ret=Instruction.Create(OpCodes.Ret);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fBoard);E(il,OpCodes.Brfalse,afterColor);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,updateColor);il.Append(afterColor);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fActive);E(il,OpCodes.Brfalse,ret);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fOn);E(il,OpCodes.Brfalse,ret);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fBoard);E(il,OpCodes.Ldfld,gameStart);E(il,OpCodes.Brfalse,ret);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fBoard);E(il,OpCodes.Ldfld,gamePause);E(il,OpCodes.Brtrue,ret);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fSleep);E(il,OpCodes.Brtrue,ret);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fID);E(il,OpCodes.Ldc_I4_2);E(il,OpCodes.Bne_Un,ret);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fState);E(il,OpCodes.Ldc_I4_1);E(il,OpCodes.Bne_Un,ret);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldc_R4,2f);E(il,OpCodes.Call,dith);il.Append(ret);
 Console.WriteLine("HF Plant.FixedUpdate @0x1803500C0: exact color/dithering conditions");
}

void PatchPlantPlanting(){
 var m=M(plant,"Planting",2); var b=Fresh(m,true); var il=b.GetILProcessor();
 var fBoard=F(plant,"board"); var fBoardConfig=F(board,"boardConfig"); var fPM=F(plant,"plantManager"); var fGX=F(plant,"GridX"); var fGY=F(plant,"GridY"); var fAttackUI=F(plant,"attackRangeUIController"); var fShadow=F(plant,"shadow"); var fAnimator=F(plant,"animator"); var fRate=F(plant,"updateRate"); var fID=F(plant,"ID"); var fAttackable=F(plant,"attackable"); var fState=F(plant,"state"); var fOn=F(plant,"isOnField"); var fSkill=F(plant,"skill"); var fOrder=F(plant,"order");
 var getPos=M(boardConfig,"GetPlantingPosition",2); var setPos=M(plant,"SetTransformPosition",1); var setLayer=M(plant,"SetLayer",0); var collapse=M(T("AttackRangeUIController"),"CollapseView",0); var create=M(plant,"CreateParticleAndSound",1); var add=M(plantManager,"AddPlant",1); var getGrid=M(board,"GetGrid",2); var gridPlant=M(grid,"Planting",1); var trigger=M(plant,"Trigger_Skill",0); var getPassive=M(skill,"GetPassive",1); var taunt=M(plant,"PC_NutknightTaunt",0);
 var getTransform=AnyCall("UnityEngine.Component","get_transform",0); var setParent=AnyCall("UnityEngine.Transform","SetParent",2); var setActive=AnyCall("UnityEngine.GameObject","SetActive",1); var animFloat=AnyCall("UnityEngine.Animator","SetFloat",2); var animTrigger=AnyCall("UnityEngine.Animator","SetTrigger",1);
 var pos=new VariableDefinition(getPos.ReturnType); b.Variables.Add(pos);
 var afterAttack=Instruction.Create(OpCodes.Nop); var setFalse=Instruction.Create(OpCodes.Nop); var afterPassiveTrigger=Instruction.Create(OpCodes.Nop); var ret=Instruction.Create(OpCodes.Ret);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fBoard);E(il,OpCodes.Ldfld,fBoardConfig);E(il,OpCodes.Ldarg_1);E(il,OpCodes.Ldarg_2);E(il,OpCodes.Callvirt,getPos);E(il,OpCodes.Stloc,pos);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldloc,pos);E(il,OpCodes.Call,setPos);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,getTransform);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fPM);E(il,OpCodes.Callvirt,getTransform);E(il,OpCodes.Ldc_I4_0);E(il,OpCodes.Callvirt,setParent);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldarg_1);E(il,OpCodes.Stfld,fGX);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldarg_2);E(il,OpCodes.Stfld,fGY);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,setLayer);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fAttackUI);E(il,OpCodes.Callvirt,collapse);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fShadow);E(il,OpCodes.Ldc_I4_1);E(il,OpCodes.Callvirt,setActive);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fAnimator);E(il,OpCodes.Ldstr,"speed");E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fRate);E(il,OpCodes.Callvirt,animFloat);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fID);E(il,OpCodes.Ldc_I4_4);E(il,OpCodes.Beq,setFalse);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fID);E(il,OpCodes.Ldc_I4,49);E(il,OpCodes.Beq,setFalse);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldc_I4_1);E(il,OpCodes.Stfld,fAttackable);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fID);E(il,OpCodes.Ldc_I4_2);E(il,OpCodes.Bne_Un,afterAttack);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldc_I4_1);E(il,OpCodes.Stfld,fState);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fAnimator);E(il,OpCodes.Ldstr,"ExplodeTrigger");E(il,OpCodes.Callvirt,animTrigger);E(il,OpCodes.Br,afterAttack);
 il.Append(setFalse);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldc_I4_0);E(il,OpCodes.Stfld,fAttackable);il.Append(afterAttack);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldc_I4_1);E(il,OpCodes.Stfld,fOn);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldloc,pos);E(il,OpCodes.Call,create);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fBoard);E(il,OpCodes.Ldfld,F(board,"plantManager"));E(il,OpCodes.Ldarg_0);E(il,OpCodes.Callvirt,add);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fBoard);E(il,OpCodes.Ldarg_1);E(il,OpCodes.Ldarg_2);E(il,OpCodes.Callvirt,getGrid);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Callvirt,gridPlant);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fSkill);E(il,OpCodes.Ldfld,F(skill,"skillTriggerType"));E(il,OpCodes.Ldc_I4_2);E(il,OpCodes.Bne_Un,afterPassiveTrigger);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,trigger);il.Append(afterPassiveTrigger);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fSkill);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Callvirt,getPassive);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fID);E(il,OpCodes.Ldc_I4_3);E(il,OpCodes.Bne_Un,ret);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fOrder);E(il,OpCodes.Ldc_I4_1);E(il,OpCodes.Blt,ret);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,taunt);il.Append(ret);
 Console.WriteLine("HF Plant.Planting @0x180356530: native call sequence restored");
}

void PatchPlantTryPlanting(){
 var m=M(plant,"TryPlanting",2);var b=Fresh(m,true);var il=b.GetILProcessor();var g=new VariableDefinition(grid);b.Variables.Add(g);var falseI=Instruction.Create(OpCodes.Ldc_I4_0);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,F(plant,"board"));E(il,OpCodes.Ldarg_1);E(il,OpCodes.Ldarg_2);E(il,OpCodes.Callvirt,M(board,"GetGrid",2));E(il,OpCodes.Stloc,g);E(il,OpCodes.Ldloc,g);E(il,OpCodes.Brfalse,falseI);E(il,OpCodes.Ldloc,g);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Callvirt,M(grid,"CanPlanting",1));E(il,OpCodes.Brfalse,falseI);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldarg_1);E(il,OpCodes.Ldarg_2);E(il,OpCodes.Call,M(plant,"Planting",2));E(il,OpCodes.Ldc_I4_1);E(il,OpCodes.Ret);il.Append(falseI);E(il,OpCodes.Ret);
 Console.WriteLine("HF Plant.TryPlanting @0x18035B390: GetGrid/CanPlanting/Planting");
}

void PatchPlantManagerAddPlant(){
 var m=M(plantManager,"AddPlant",1);var b=Fresh(m);var il=b.GetILProcessor();var fPlants=F(plantManager,"plants");var list=(GenericInstanceType)fPlants.FieldType;var add=ListMethod(list,"Add",module.TypeSystem.Void,plant);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fPlants);E(il,OpCodes.Ldarg_1);E(il,OpCodes.Callvirt,add);E(il,OpCodes.Ldarg_1);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,F(plantManager,"board"));E(il,OpCodes.Stfld,F(plant,"board"));E(il,OpCodes.Ret);
 Console.WriteLine("HF PlantManager.AddPlant @0x1803225A0: plants.Add + plant.board only");
}

void PatchMouseDownUpdate(){
 var m=M(mouse,"MouseDownUpdate",0);var b=Fresh(m);var il=b.GetILProcessor();var op=F(mouse,"operationCD");var onBoard=F(mouse,"onBoard");var delta=AnyCall("UnityEngine.Time","get_deltaTime",0);var preview=M(mouse,"MouseDownUpdateOnBoardPreview",0);var boardClick=M(mouse,"MouseDownUpdateOnBoard",0);var skipCd=Instruction.Create(OpCodes.Nop);var ret=Instruction.Create(OpCodes.Ret);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,op);E(il,OpCodes.Ldc_R4,0f);E(il,OpCodes.Ble_Un,skipCd);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,op);E(il,OpCodes.Call,delta);E(il,OpCodes.Sub);E(il,OpCodes.Stfld,op);il.Append(skipCd);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,preview);E(il,OpCodes.Brtrue,ret);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,onBoard);E(il,OpCodes.Brfalse,ret);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,boardClick);il.Append(ret);
 Console.WriteLine("HF MouseManager.MouseDownUpdate @0x18031FAE0: operationCD + preview/board dispatch");
}

void PatchCardHelpers(){
 var fBoard=F(card,"board");var fState=F(card,"cardState");var fCd=F(card,"cdTime");var fAl=F(card,"alTime");var fSun=F(card,"sunPrice");var fCDcover=F(card,"CDcover");var fSuncover=F(card,"Suncover");var fCDText=F(card,"CDText");var fSunText=F(card,"sunPriceText");var fSunMgr=F(board,"sunManager");var fSunPoint=F(sunManager,"sunPoint");
 var delta=AnyCall("UnityEngine.Time","get_deltaTime",0);var setFill=AnyCall("UnityEngine.UI.Image","set_fillAmount",1);var setText=AnyCall("TMPro.TMP_Text","set_text",1);var singleToString=AnyCall("System.Single","ToString",1);var intToString=AnyCall("System.Int32","ToString",0);var stateSet=M(card,"CardStateSet",0);
 var m=M(card,"CoolingUpdate",0);var b=Fresh(m,true);var il=b.GetILProcessor();var remain=new VariableDefinition(module.TypeSystem.Single);b.Variables.Add(remain);var ret=Instruction.Create(OpCodes.Ret);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fAl);E(il,OpCodes.Call,delta);E(il,OpCodes.Add);E(il,OpCodes.Stfld,fAl);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fCd);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fAl);E(il,OpCodes.Sub);E(il,OpCodes.Stloc,remain);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fCDcover);E(il,OpCodes.Ldloc,remain);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fCd);E(il,OpCodes.Div);E(il,OpCodes.Callvirt,setFill);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fSuncover);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fAl);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fCd);E(il,OpCodes.Div);E(il,OpCodes.Callvirt,setFill);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fCDText);E(il,OpCodes.Ldloca,remain);E(il,OpCodes.Ldstr,"0.0s");E(il,OpCodes.Call,singleToString);E(il,OpCodes.Callvirt,setText);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fAl);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fCd);E(il,OpCodes.Blt,ret);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldc_I4_1);E(il,OpCodes.Stfld,fState);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,stateSet);il.Append(ret);
 m=M(card,"WaitingSunUpdate",0);b=Fresh(m);il=b.GetILProcessor();ret=Instruction.Create(OpCodes.Ret);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fBoard);E(il,OpCodes.Ldfld,fSunMgr);E(il,OpCodes.Ldfld,fSunPoint);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fSun);E(il,OpCodes.Blt,ret);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldc_I4_2);E(il,OpCodes.Stfld,fState);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,stateSet);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fSuncover);E(il,OpCodes.Ldc_R4,0f);E(il,OpCodes.Callvirt,setFill);il.Append(ret);
 m=M(card,"ReadyUpdate",0);b=Fresh(m);il=b.GetILProcessor();ret=Instruction.Create(OpCodes.Ret);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fBoard);E(il,OpCodes.Ldfld,fSunMgr);E(il,OpCodes.Ldfld,fSunPoint);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fSun);E(il,OpCodes.Bge,ret);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldc_I4_1);E(il,OpCodes.Stfld,fState);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,stateSet);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fSuncover);E(il,OpCodes.Ldc_R4,1f);E(il,OpCodes.Callvirt,setFill);il.Append(ret);
 m=M(card,"OnHeadUpdate",0);b=Fresh(m);il=b.GetILProcessor();E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fCDcover);E(il,OpCodes.Ldc_R4,1f);E(il,OpCodes.Callvirt,setFill);E(il,OpCodes.Ret);
 m=M(card,"SunUITextUpdate",0);b=Fresh(m);il=b.GetILProcessor();E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fSunText);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldflda,fSun);E(il,OpCodes.Call,intToString);E(il,OpCodes.Callvirt,setText);E(il,OpCodes.Ret);
 Console.WriteLine("HF Card helper state methods: native-visible arithmetic/UI restored");
}

void PatchCardUpdate(){
 var m=M(card,"Update",0);var b=Fresh(m);var il=b.GetILProcessor();var fBoard=F(card,"board");var fState=F(card,"cardState");var ret=Instruction.Create(OpCodes.Ret);var c1=Instruction.Create(OpCodes.Nop);var c2=Instruction.Create(OpCodes.Nop);var c3=Instruction.Create(OpCodes.Nop);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,M(card,"SunUITextUpdate",0));E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fBoard);E(il,OpCodes.Brfalse,ret);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fBoard);E(il,OpCodes.Ldfld,F(board,"gameStart"));E(il,OpCodes.Brfalse,ret);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fState);E(il,OpCodes.Brtrue,c1);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,M(card,"CoolingUpdate",0));E(il,OpCodes.Br,ret);il.Append(c1);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fState);E(il,OpCodes.Ldc_I4_1);E(il,OpCodes.Bne_Un,c2);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,M(card,"WaitingSunUpdate",0));E(il,OpCodes.Br,ret);il.Append(c2);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fState);E(il,OpCodes.Ldc_I4_2);E(il,OpCodes.Bne_Un,c3);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,M(card,"ReadyUpdate",0));E(il,OpCodes.Br,ret);il.Append(c3);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fState);E(il,OpCodes.Ldc_I4_3);E(il,OpCodes.Bne_Un,ret);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,M(card,"OnHeadUpdate",0));il.Append(ret);
 Console.WriteLine("HF Card.Update @0x180344F10: sun UI + Cooling/Waiting/Ready/OnHead state machine");
}

void PatchCardStateSet(){
 var m=M(card,"CardStateSet",0);var b=Fresh(m);var il=b.GetILProcessor();var fState=F(card,"cardState");var getGO=AnyCall("UnityEngine.Component","get_gameObject",0);var setActive=AnyCall("UnityEngine.GameObject","SetActive",1);void Set(FieldDefinition f,bool v){E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,f);E(il,OpCodes.Callvirt,getGO);E(il,v?OpCodes.Ldc_I4_1:OpCodes.Ldc_I4_0);E(il,OpCodes.Callvirt,setActive);}var s1=Instruction.Create(OpCodes.Nop);var s2=Instruction.Create(OpCodes.Nop);var s3=Instruction.Create(OpCodes.Nop);var s4=Instruction.Create(OpCodes.Nop);var ret=Instruction.Create(OpCodes.Ret);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fState);E(il,OpCodes.Ldc_I4_0);E(il,OpCodes.Bne_Un,s1);Set(F(card,"CDcover"),true);Set(F(card,"Suncover"),true);Set(F(card,"cardBackground"),true);Set(F(card,"cardFace"),true);Set(F(card,"CDText"),true);Set(F(card,"sunPriceText"),true);E(il,OpCodes.Br,ret);
 il.Append(s1);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fState);E(il,OpCodes.Ldc_I4_1);E(il,OpCodes.Bne_Un,s2);Set(F(card,"CDcover"),false);Set(F(card,"Suncover"),true);Set(F(card,"cardBackground"),true);Set(F(card,"cardFace"),true);Set(F(card,"CDText"),false);Set(F(card,"sunPriceText"),true);E(il,OpCodes.Br,ret);
 il.Append(s2);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fState);E(il,OpCodes.Ldc_I4_2);E(il,OpCodes.Bne_Un,s3);Set(F(card,"CDcover"),false);Set(F(card,"Suncover"),false);Set(F(card,"cardBackground"),true);Set(F(card,"cardFace"),true);Set(F(card,"CDText"),false);Set(F(card,"sunPriceText"),true);E(il,OpCodes.Br,ret);
 il.Append(s3);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fState);E(il,OpCodes.Ldc_I4_3);E(il,OpCodes.Bne_Un,s4);Set(F(card,"CDcover"),true);Set(F(card,"Suncover"),false);Set(F(card,"cardBackground"),true);Set(F(card,"cardFace"),true);Set(F(card,"CDText"),false);Set(F(card,"sunPriceText"),true);E(il,OpCodes.Br,ret);
 il.Append(s4);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fState);E(il,OpCodes.Ldc_I4_4);E(il,OpCodes.Bne_Un,ret);Set(F(card,"CDcover"),false);Set(F(card,"Suncover"),false);Set(F(card,"cardBackground"),false);Set(F(card,"cardFace"),false);Set(F(card,"CDText"),false);Set(F(card,"sunPriceText"),false);il.Append(ret);
 Console.WriteLine("HF Card.CardStateSet @0x180343140: native jump-table UI states 0..4");
}

void PatchCardOnClick(){
 var m=M(card,"CardOnClick",0);var b=Fresh(m,true);var il=b.GetILProcessor();var fBoard=F(card,"board");var fState=F(card,"cardState");var fPlantType=F(card,"cardPlantType");var fDeviceType=F(card,"cardDeviceType");var fOnBank=F(card,"isOnBank");var fMouse=F(board,"mouseManager");var fHand=F(mouse,"handItemType");var mm=new VariableDefinition(mouse);var ok=new VariableDefinition(module.TypeSystem.Boolean);b.Variables.Add(mm);b.Variables.Add(ok);var audio=M(mouse,"AudioPlay",1);var getPlant=M(mouse,"GetPlantFromCardbank",1);var getDevice=M(mouse,"GetDeviceFromCardbank",1);var autoSlow=M(globals,"AutoTimeSlow",2);var stateSet=M(card,"CardStateSet",0);var opCd=M(mouse,"SetOperationCD",0);var getGO=AnyCall("UnityEngine.Component","get_gameObject",0);var destroy=AnyCall("UnityEngine.Object","Destroy",1);var not0=Instruction.Create(OpCodes.Nop);var not1=Instruction.Create(OpCodes.Nop);var not2=Instruction.Create(OpCodes.Nop);var notPlant=Instruction.Create(OpCodes.Nop);var notDevice=Instruction.Create(OpCodes.Nop);var success=Instruction.Create(OpCodes.Nop);var onBank=Instruction.Create(OpCodes.Nop);var ret=Instruction.Create(OpCodes.Ret);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fBoard);E(il,OpCodes.Ldfld,fMouse);E(il,OpCodes.Stloc,mm);E(il,OpCodes.Ldloc,mm);E(il,OpCodes.Ldfld,fHand);E(il,OpCodes.Brtrue,ret);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fBoard);E(il,OpCodes.Ldfld,F(board,"gamePause"));E(il,OpCodes.Brtrue,ret);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fState);E(il,OpCodes.Brtrue,not0);E(il,OpCodes.Ldloc,mm);E(il,OpCodes.Ldc_I4_2);E(il,OpCodes.Callvirt,audio);E(il,OpCodes.Br,ret);il.Append(not0);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fState);E(il,OpCodes.Ldc_I4_1);E(il,OpCodes.Bne_Un,not1);E(il,OpCodes.Ldloc,mm);E(il,OpCodes.Ldc_I4_2);E(il,OpCodes.Callvirt,audio);E(il,OpCodes.Br,ret);
 il.Append(not1);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fState);E(il,OpCodes.Ldc_I4_2);E(il,OpCodes.Bne_Un,not2);E(il,OpCodes.Ldc_I4_0);E(il,OpCodes.Stloc,ok);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fPlantType);E(il,OpCodes.Ldc_I4_M1);E(il,OpCodes.Beq,notPlant);E(il,OpCodes.Ldloc,mm);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Callvirt,getPlant);E(il,OpCodes.Stloc,ok);il.Append(notPlant);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fDeviceType);E(il,OpCodes.Ldc_I4_M1);E(il,OpCodes.Beq,notDevice);E(il,OpCodes.Ldloc,mm);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Callvirt,getDevice);E(il,OpCodes.Stloc,ok);il.Append(notDevice);E(il,OpCodes.Ldloc,ok);E(il,OpCodes.Brtrue,success);E(il,OpCodes.Br,ret);il.Append(success);
 E(il,OpCodes.Ldc_R4,0.25f);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fBoard);E(il,OpCodes.Call,autoSlow);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fOnBank);E(il,OpCodes.Brtrue,onBank);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,getGO);E(il,OpCodes.Call,destroy);il.Append(onBank);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldc_I4_3);E(il,OpCodes.Stfld,fState);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,stateSet);E(il,OpCodes.Ldloc,mm);E(il,OpCodes.Ldc_I4_8);E(il,OpCodes.Callvirt,audio);E(il,OpCodes.Ldloc,mm);E(il,OpCodes.Callvirt,opCd);E(il,OpCodes.Br,ret);
 il.Append(not2);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fState);E(il,OpCodes.Ldc_I4_3);E(il,OpCodes.Bne_Un,ret);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldc_I4_2);E(il,OpCodes.Stfld,fState);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,stateSet);il.Append(ret);
 Console.WriteLine("HF Card.CardOnClick @0x180342F10: native jump-table behavior including device/slow/audio/CD");
}

void PatchCardOfBankPlanting(){
 var m=M(card,"CardOfBankPlanting",0);var b=Fresh(m);var il=b.GetILProcessor();var fBoard=F(card,"board");var fSun=F(card,"sunPrice");var fPlantType=F(card,"cardPlantType");var fDeviceType=F(card,"cardDeviceType");var fDeviceData=F(card,"deviceData");var fState=F(card,"cardState");var fAl=F(card,"alTime");var fNumText=F(card,"numText");var deviceData=card.NestedTypes.Single(x=>x.Name=="CardDeviceData");var fNumber=F(deviceData,"number");var sub=M(sunManager,"SubSunpoint",1);var remove=M(banks,"RemoveCardFromBank",1);var setText=AnyCall("TMPro.TMP_Text","SetText",2);var intToString=AnyCall("System.Int32","ToString",0);var concat=AnyCall("System.String","Concat",2);var noPlant=Instruction.Create(OpCodes.Nop);var noDevice=Instruction.Create(OpCodes.Nop);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fBoard);E(il,OpCodes.Ldfld,F(board,"sunManager"));E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fSun);E(il,OpCodes.Callvirt,sub);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fPlantType);E(il,OpCodes.Ldc_I4_M1);E(il,OpCodes.Beq,noPlant);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldc_I4_4);E(il,OpCodes.Stfld,fState);il.Append(noPlant);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fDeviceType);E(il,OpCodes.Ldc_I4_M1);E(il,OpCodes.Beq,noDevice);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldc_I4_0);E(il,OpCodes.Stfld,fState);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fDeviceData);E(il,OpCodes.Dup);E(il,OpCodes.Ldfld,fNumber);E(il,OpCodes.Ldc_I4_1);E(il,OpCodes.Sub);E(il,OpCodes.Stfld,fNumber);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldc_R4,0f);E(il,OpCodes.Stfld,fAl);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fNumText);E(il,OpCodes.Ldstr,"X");E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fDeviceData);E(il,OpCodes.Ldflda,fNumber);E(il,OpCodes.Call,intToString);E(il,OpCodes.Call,concat);E(il,OpCodes.Ldc_I4_1);E(il,OpCodes.Callvirt,setText);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fDeviceData);E(il,OpCodes.Ldfld,fNumber);E(il,OpCodes.Ldc_I4_0);E(il,OpCodes.Bgt,noDevice);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fBoard);E(il,OpCodes.Ldfld,F(board,"banks"));E(il,OpCodes.Ldarg_0);E(il,OpCodes.Callvirt,remove);il.Append(noDevice);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,M(card,"CardStateSet",0));E(il,OpCodes.Ret);
 Console.WriteLine("HF Card.CardOfBankPlanting @0x180342E00: sun/card/device state and inventory restored");
}
