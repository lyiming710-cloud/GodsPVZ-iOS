from pathlib import Path
p=Path('Tools/HighFidelityPatch/Program.cs')
s=p.read_text()
if 'HF2_NATIVE_PLANT_START_UPDATE' in s:
    print('HF2 already injected')
    raise SystemExit(0)
s=s.replace('PatchPlantFixedUpdate();\n', 'PatchPlantFixedUpdate();\nPatchPlantStart();\nPatchPlantUpdate();\n', 1)
s=s.replace('(\"Plant\",\"Awake\",0),(\"Plant\",\"FixedUpdate\",0),(\"Plant\",\"Planting\",2)', '(\"Plant\",\"Awake\",0),(\"Plant\",\"Start\",0),(\"Plant\",\"Update\",0),(\"Plant\",\"FixedUpdate\",0),(\"Plant\",\"Planting\",2)', 1)
s += r'''

// HF2_NATIVE_PLANT_START_UPDATE
void PatchPlantStart(){
    var m=M(plant,"Start",0); var old=m.Body?.Instructions.ToList() ?? new List<Instruction>();
    var b=Fresh(m,true); var il=b.GetILProcessor();
    var fProduce=F(plant,"produce_Brightness"); var fFlash=F(plant,"flash_Brightness"); var fBoard=F(plant,"board");
    var fPM=F(plant,"plantManager"); var fEM=F(plant,"elementManager"); var fUIs=F(plant,"elementUIControllers");
    var em=T("ElementManager"); var element=T("Element"); var ui=T("ElementUIController");
    var fElements=F(em,"elements"); var fElemType=F(element,"type"); var fElemUI=F(element,"UIController"); var fUIType=F(ui,"type");
    var unityBool=AllRefs().First(x=>x.DeclaringType.FullName=="UnityEngine.Object"&&x.Name=="op_Implicit"&&x.Parameters.Count==1);
    var genDef=M(em,"CreateNewElements",1); var createElements=new GenericInstanceMethod(genDef); createElements.GenericArguments.Add(plant);
    var listE=(GenericInstanceType)fElements.FieldType; var listUI=(GenericInstanceType)fUIs.FieldType;
    var countE=ListMethod(listE,"get_Count",module.TypeSystem.Int32); var itemE=ListMethod(listE,"get_Item",element,module.TypeSystem.Int32);
    var countUI=ListMethod(listUI,"get_Count",module.TypeSystem.Int32); var itemUI=ListMethod(listUI,"get_Item",ui,module.TypeSystem.Int32);
    var startChar=M(plant,"Start_Characteristic",0);
    var eLocal=new VariableDefinition(element); var uLocal=new VariableDefinition(ui); var iLocal=new VariableDefinition(module.TypeSystem.Int32); var jLocal=new VariableDefinition(module.TypeSystem.Int32);
    b.Variables.Add(eLocal);b.Variables.Add(uLocal);b.Variables.Add(iLocal);b.Variables.Add(jLocal);
    var skipBoard=Instruction.Create(OpCodes.Nop); var skipElements=Instruction.Create(OpCodes.Nop); var outerTest=Instruction.Create(OpCodes.Nop); var outerNext=Instruction.Create(OpCodes.Nop); var innerTest=Instruction.Create(OpCodes.Nop); var innerNext=Instruction.Create(OpCodes.Nop);
    E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldc_R4,1f);E(il,OpCodes.Stfld,fProduce); E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldc_R4,1f);E(il,OpCodes.Stfld,fFlash);
    E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fBoard);E(il,OpCodes.Call,unityBool);E(il,OpCodes.Brfalse,skipBoard);
    E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fBoard);E(il,OpCodes.Ldfld,F(board,"plantManager"));E(il,OpCodes.Stfld,fPM); il.Append(skipBoard);
    E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fEM);E(il,OpCodes.Brfalse,skipElements);
    E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fEM);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Callvirt,createElements);
    E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fEM);E(il,OpCodes.Ldfld,fElements);E(il,OpCodes.Brfalse,skipElements);
    E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fUIs);E(il,OpCodes.Brfalse,skipElements);
    E(il,OpCodes.Ldc_I4_0);E(il,OpCodes.Stloc,iLocal);E(il,OpCodes.Br,outerTest);
    var outerBody=Instruction.Create(OpCodes.Nop); il.Append(outerBody);
    E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fEM);E(il,OpCodes.Ldfld,fElements);E(il,OpCodes.Ldloc,iLocal);E(il,OpCodes.Callvirt,itemE);E(il,OpCodes.Stloc,eLocal);
    E(il,OpCodes.Ldloc,eLocal);E(il,OpCodes.Brfalse,outerNext);E(il,OpCodes.Ldc_I4_0);E(il,OpCodes.Stloc,jLocal);E(il,OpCodes.Br,innerTest);
    var innerBody=Instruction.Create(OpCodes.Nop);il.Append(innerBody);
    E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fUIs);E(il,OpCodes.Ldloc,jLocal);E(il,OpCodes.Callvirt,itemUI);E(il,OpCodes.Stloc,uLocal);
    E(il,OpCodes.Ldloc,uLocal);E(il,OpCodes.Brfalse,innerNext);E(il,OpCodes.Ldloc,eLocal);E(il,OpCodes.Ldfld,fElemType);E(il,OpCodes.Ldloc,uLocal);E(il,OpCodes.Ldfld,fUIType);E(il,OpCodes.Bne_Un,innerNext);
    E(il,OpCodes.Ldloc,eLocal);E(il,OpCodes.Ldloc,uLocal);E(il,OpCodes.Stfld,fElemUI);E(il,OpCodes.Br,outerNext);
    il.Append(innerNext);E(il,OpCodes.Ldloc,jLocal);E(il,OpCodes.Ldc_I4_1);E(il,OpCodes.Add);E(il,OpCodes.Stloc,jLocal);il.Append(innerTest);
    E(il,OpCodes.Ldloc,jLocal);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fUIs);E(il,OpCodes.Callvirt,countUI);E(il,OpCodes.Blt,innerBody);
    il.Append(outerNext);E(il,OpCodes.Ldloc,iLocal);E(il,OpCodes.Ldc_I4_1);E(il,OpCodes.Add);E(il,OpCodes.Stloc,iLocal);il.Append(outerTest);
    E(il,OpCodes.Ldloc,iLocal);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fEM);E(il,OpCodes.Ldfld,fElements);E(il,OpCodes.Callvirt,countE);E(il,OpCodes.Blt,outerBody);
    il.Append(skipElements);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,startChar);E(il,OpCodes.Ret);
    Console.WriteLine("HF2 Plant.Start @0x18035A490: native brightness/manager/element-UI pairing restored");
}

void PatchPlantUpdate(){
    var m=M(plant,"Update",0); var old=m.Body?.Instructions.ToList() ?? new List<Instruction>();
    var oldRefs=old.Select(i=>i.Operand).OfType<MethodReference>().ToList(); var oldFields=old.Select(i=>i.Operand).OfType<FieldReference>().ToList();
    MethodReference OldCall(string decl,string name,int pc)=>oldRefs.First(x=>x.DeclaringType.FullName.Contains(decl)&&x.Name==name&&x.Parameters.Count==pc);
    FieldReference VecField(string name)=>oldFields.First(x=>x.DeclaringType.FullName=="UnityEngine.Vector3"&&x.Name==name);
    var b=Fresh(m,true); var il=b.GetILProcessor();
    var fFlashTime=F(plant,"flash_Time");var fFlash=F(plant,"flash_Brightness");var fProduce=F(plant,"produce_Brightness");var fSelected=F(plant,"selected_Bright");
    var fActive=F(plant,"active");var fShadow=F(plant,"shadow");var fX=F(plant,"fX");var fY=F(plant,"fY");var fOn=F(plant,"isOnField");var fBoard=F(plant,"board");
    var fLiving=F(plant,"livingTime");var fRate=F(plant,"updateRate");var fSleep=F(plant,"isSleep");var fHPUI=F(plant,"hpUIController");var fEM=F(plant,"elementManager");
    var fDied=F(plant,"isDied");var fHP=F(plant,"healthPoint");var fID=F(plant,"ID");var fBarrel=F(plant,"pT_chomperviking_barrelsPoint");var fEaten=F(plant,"beEaten");var fState=F(plant,"state");
    var timeDelta=OldCall("UnityEngine.Time","get_deltaTime",0); var getTransform=OldCall("UnityEngine.Component","get_transform",0); var getPos=OldCall("UnityEngine.Transform","get_position",0);var vx=VecField("x");var vy=VecField("y");
    var max=oldRefs.First(x=>(x.DeclaringType.FullName=="System.Math"||x.DeclaringType.FullName=="System.MathF")&&x.Name=="Max"&&x.Parameters.Count==2&&x.Parameters[0].ParameterType.MetadataType==MetadataType.Single);
    var setRate=M(plant,"SetUpdateRate",0);var setBright=M(plant,"SetBrightness",1);var runtime=M(board,"BoardRuntime",0);var hpUpdate=M(T("HPUIController_Plant"),"Update_HPUI",0);var fight=M(plant,"Update_PlantFight",0);var emUpdate=M(T("ElementManager"),"Update",0);
    var healed=M(plant,"Healed",2);var barrels=M(plant,"PC_ChompervikingUpdateBarrels",0);var potato=M(plant,"PC_PotatoBoom",0);var die=M(plant,"PlantDie",0);
    var globals=T("GlobalStaticVars");var audioVol=M(globals,"AudioVolume",0);var createAudio=M(globals,"CreateAudioAtPoint",3);var zombieMgr=T("ZombieManager");var fZombie=F(board,"zombieManager");var fClips=F(zombieMgr,"audioClip_zombie");var clipList=(GenericInstanceType)fClips.FieldType;var getClip=ListMethod(clipList,"get_Item",clipList.GenericArguments[0],module.TypeSystem.Int32);
    var camMain=AllRefs().First(x=>x.DeclaringType.FullName=="UnityEngine.Camera"&&x.Name=="get_main"&&x.Parameters.Count==0);
    var vecType=getPos.ReturnType;var pos=new VariableDefinition(vecType);var a=new VariableDefinition(module.TypeSystem.Single);b.Variables.Add(pos);b.Variables.Add(a);
    var flashDone=Instruction.Create(OpCodes.Nop);var selectedFalse=Instruction.Create(OpCodes.Nop);var prodDone=Instruction.Create(OpCodes.Nop);var afterRuntime=Instruction.Create(OpCodes.Nop);var afterBarrel=Instruction.Create(OpCodes.Nop);var normalClear=Instruction.Create(OpCodes.Nop);var ret=Instruction.Create(OpCodes.Ret);
    E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,setRate);
    E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fFlashTime);E(il,OpCodes.Ldc_R4,0f);E(il,OpCodes.Ble_Un,flashDone);
    E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fFlash);E(il,OpCodes.Call,timeDelta);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fFlashTime);E(il,OpCodes.Div);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fFlash);E(il,OpCodes.Ldc_R4,1f);E(il,OpCodes.Sub);E(il,OpCodes.Mul);E(il,OpCodes.Sub);E(il,OpCodes.Stfld,fFlash);
    E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fFlashTime);E(il,OpCodes.Call,timeDelta);E(il,OpCodes.Sub);E(il,OpCodes.Stfld,fFlashTime);il.Append(flashDone);
    E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fFlash);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fProduce);E(il,OpCodes.Call,max);E(il,OpCodes.Stloc,a);
    E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fSelected);E(il,OpCodes.Brfalse,selectedFalse);E(il,OpCodes.Ldloc,a);E(il,OpCodes.Ldc_R4,3f);E(il,OpCodes.Call,max);E(il,OpCodes.Stloc,a);il.Append(selectedFalse);
    E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldloc,a);E(il,OpCodes.Call,setBright);
    E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fProduce);E(il,OpCodes.Ldc_R4,1f);E(il,OpCodes.Ble_Un,prodDone);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fProduce);E(il,OpCodes.Call,timeDelta);E(il,OpCodes.Ldc_R4,2f);E(il,OpCodes.Mul);E(il,OpCodes.Sub);E(il,OpCodes.Stfld,fProduce);il.Append(prodDone);
    E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldc_I4_0);E(il,OpCodes.Stfld,fSelected);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fActive);E(il,OpCodes.Brfalse,ret);
    E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fShadow);E(il,OpCodes.Callvirt,getTransform);E(il,OpCodes.Callvirt,getPos);E(il,OpCodes.Stloc,pos);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldloca,pos);E(il,OpCodes.Ldfld,vx);E(il,OpCodes.Stfld,fX);
    E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fShadow);E(il,OpCodes.Callvirt,getTransform);E(il,OpCodes.Callvirt,getPos);E(il,OpCodes.Stloc,pos);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldloca,pos);E(il,OpCodes.Ldfld,vy);E(il,OpCodes.Stfld,fY);
    E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fOn);E(il,OpCodes.Brfalse,afterRuntime);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fBoard);E(il,OpCodes.Callvirt,runtime);E(il,OpCodes.Brfalse,afterRuntime);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fBoard);E(il,OpCodes.Ldfld,F(board,"isFinished"));E(il,OpCodes.Brtrue,afterRuntime);
    E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fLiving);E(il,OpCodes.Call,timeDelta);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fRate);E(il,OpCodes.Mul);E(il,OpCodes.Add);E(il,OpCodes.Stfld,fLiving);
    E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fSleep);E(il,OpCodes.Brtrue,afterRuntime);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fHPUI);E(il,OpCodes.Callvirt,hpUpdate);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,fight);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fEM);E(il,OpCodes.Callvirt,emUpdate);il.Append(afterRuntime);
    E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fDied);E(il,OpCodes.Brtrue,normalClear);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fHP);E(il,OpCodes.Ldc_R4,0f);E(il,OpCodes.Bge_Un,normalClear);
    E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fID);E(il,OpCodes.Ldc_I4_6);E(il,OpCodes.Bne_Un,afterBarrel);
    E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fHP);E(il,OpCodes.Neg);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fBarrel);E(il,OpCodes.Bgt_Un,afterBarrel);
    E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fBarrel);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,healed);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldc_R4,0f);E(il,OpCodes.Stfld,fBarrel);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,barrels);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fHP);E(il,OpCodes.Ldc_R4,0f);E(il,OpCodes.Bge_Un,normalClear);il.Append(afterBarrel);
    var noAudio=Instruction.Create(OpCodes.Nop);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fEaten);E(il,OpCodes.Brfalse,noAudio);
    E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fBoard);E(il,OpCodes.Ldfld,fZombie);E(il,OpCodes.Ldfld,fClips);E(il,OpCodes.Ldc_I4,24);E(il,OpCodes.Callvirt,getClip);
    E(il,OpCodes.Call,camMain);E(il,OpCodes.Callvirt,getTransform);E(il,OpCodes.Callvirt,getPos);E(il,OpCodes.Call,audioVol);E(il,OpCodes.Call,createAudio);E(il,OpCodes.Pop);il.Append(noAudio);
    var notPotato=Instruction.Create(OpCodes.Nop);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fID);E(il,OpCodes.Ldc_I4_4);E(il,OpCodes.Bne_Un,notPotato);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fState);E(il,OpCodes.Ldc_I4_0);E(il,OpCodes.Bne_Un,notPotato);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,potato);E(il,OpCodes.Br,ret);il.Append(notPotato);
    E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,die);il.Append(normalClear);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldc_I4_0);E(il,OpCodes.Stfld,fEaten);il.Append(ret);
    Console.WriteLine("HF2 Plant.Update @0x18035C4D0: native brightness/runtime/death/audio branches restored");
}
'''
p.write_text(s)
print('Injected HF2 source', len(s))
