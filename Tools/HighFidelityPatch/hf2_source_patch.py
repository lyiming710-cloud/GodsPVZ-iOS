from pathlib import Path
p=Path('Tools/HighFidelityPatch/Program.cs')
s=p.read_text()
if 'HF2_EXACT_NATIVE_PLANT_CORE' in s:
    print('HF2 exact already injected'); raise SystemExit(0)
# add calls after Awake
s=s.replace('PatchPlantAwake();\nPatchPlantFixedUpdate();', 'PatchPlantAwake();\nPatchPlantLoopAddAnimation();\nPatchPlantStart();\nPatchPlantStartCharacteristic();\nPatchPlantUpdate();\nPatchPlantFixedUpdate();')
# add verify specs
s=s.replace('("Plant","Awake",0),("Plant","FixedUpdate",0),("Plant","Planting",2)', '("Plant","Awake",0),("Plant","LoopAddAnimation",1),("Plant","Start",0),("Plant","Start_Characteristic",0),("Plant","Update",0),("Plant","FixedUpdate",0),("Plant","Planting",2)')
# insert functions before FixedUpdate
marker='void PatchPlantFixedUpdate(){\n'
assert marker in s
hf2=r'''
// HF2_EXACT_NATIVE_PLANT_CORE
void PatchPlantLoopAddAnimation(){
 var m=M(plant,"LoopAddAnimation",1); var b=Fresh(m,true); var il=b.GetILProcessor();
 var parent=m.Parameters[0]; var sprites=F(plant,"animationSprites"); var list=(GenericInstanceType)sprites.FieldType;
 var spriteRenderer=T("UnityEngine.SpriteRenderer");
 var getRenderer=AnyGeneric("UnityEngine.Component","GetComponent","SpriteRenderer");
 var objImplicit=AllRefs().First(x=>x.DeclaringType.FullName=="UnityEngine.Object"&&x.Name=="op_Implicit"&&x.Parameters.Count==1);
 var getGO=AnyCall("UnityEngine.Component","get_gameObject",0); var add=ListMethod(list,"Add",module.TypeSystem.Void,list.GenericArguments[0]);
 var getEnum=AnyCall("UnityEngine.Transform","GetEnumerator",0); var ienum=getEnum.ReturnType;
 var moveNext=new MethodReference("MoveNext",module.TypeSystem.Boolean,ienum){HasThis=true};
 var getCurrent=new MethodReference("get_Current",module.TypeSystem.Object,ienum){HasThis=true};
 var renderer=new VariableDefinition(spriteRenderer); var en=new VariableDefinition(ienum); var child=new VariableDefinition(parent.ParameterType);
 b.Variables.Add(renderer);b.Variables.Add(en);b.Variables.Add(child);
 var skipAdd=Instruction.Create(OpCodes.Nop); var loopCheck=Instruction.Create(OpCodes.Nop); var loopBody=Instruction.Create(OpCodes.Nop); var ret=Instruction.Create(OpCodes.Ret);
 E(il,OpCodes.Ldarg,parent);E(il,OpCodes.Callvirt,getRenderer);E(il,OpCodes.Stloc,renderer);
 E(il,OpCodes.Ldloc,renderer);E(il,OpCodes.Call,objImplicit);E(il,OpCodes.Brfalse,skipAdd);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,sprites);E(il,OpCodes.Ldarg,parent);E(il,OpCodes.Callvirt,getGO);E(il,OpCodes.Callvirt,add);
 il.Append(skipAdd);
 E(il,OpCodes.Ldarg,parent);E(il,OpCodes.Callvirt,getEnum);E(il,OpCodes.Stloc,en);E(il,OpCodes.Br,loopCheck);
 il.Append(loopBody);E(il,OpCodes.Ldloc,en);E(il,OpCodes.Callvirt,getCurrent);E(il,OpCodes.Castclass,parent.ParameterType);E(il,OpCodes.Stloc,child);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldloc,child);E(il,OpCodes.Call,M(plant,"LoopAddAnimation",1));
 il.Append(loopCheck);E(il,OpCodes.Ldloc,en);E(il,OpCodes.Callvirt,moveNext);E(il,OpCodes.Brtrue,loopBody);il.Append(ret);
 Console.WriteLine("HF2 Plant.LoopAddAnimation @0x180351AC0: SpriteRenderer collect + recursive Transform enumeration");
}

void PatchPlantStart(){
 var m=M(plant,"Start",0); var b=Fresh(m,true); var il=b.GetILProcessor();
 var fProduce=F(plant,"produce_Brightness"), fFlash=F(plant,"flash_Brightness"), fBoard=F(plant,"board"), fPM=F(plant,"plantManager"), fEM=F(plant,"elementManager"), fUIs=F(plant,"elementUIControllers");
 var em=T("ElementManager"), elem=T("Element"), ui=T("ElementUIController");
 var fElems=F(em,"elements"), fElemType=F(elem,"type"), fElemUI=F(elem,"UIController"), fUIType=F(ui,"type");
 var boardPM=F(board,"plantManager");
 var objImplicit=AllRefs().First(x=>x.DeclaringType.FullName=="UnityEngine.Object"&&x.Name=="op_Implicit"&&x.Parameters.Count==1);
 var createDef=M(em,"CreateNewElements",1); var create=new GenericInstanceMethod(createDef); create.GenericArguments.Add(plant);
 var elemsList=(GenericInstanceType)fElems.FieldType; var uiList=(GenericInstanceType)fUIs.FieldType;
 var elemCount=ListMethod(elemsList,"get_Count",module.TypeSystem.Int32); var elemGet=ListMethod(elemsList,"get_Item",elemsList.GenericArguments[0],module.TypeSystem.Int32);
 var uiCount=ListMethod(uiList,"get_Count",module.TypeSystem.Int32); var uiGet=ListMethod(uiList,"get_Item",uiList.GenericArguments[0],module.TypeSystem.Int32);
 var i=new VariableDefinition(module.TypeSystem.Int32), j=new VariableDefinition(module.TypeSystem.Int32), e=new VariableDefinition(elem), u=new VariableDefinition(ui);b.Variables.Add(i);b.Variables.Add(j);b.Variables.Add(e);b.Variables.Add(u);
 var afterBoard=Instruction.Create(OpCodes.Nop), outerCheck=Instruction.Create(OpCodes.Nop), outerBody=Instruction.Create(OpCodes.Nop), innerCheck=Instruction.Create(OpCodes.Nop), innerBody=Instruction.Create(OpCodes.Nop), noMatch=Instruction.Create(OpCodes.Nop), afterLoops=Instruction.Create(OpCodes.Nop);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldc_R4,1f);E(il,OpCodes.Stfld,fProduce);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldc_R4,1f);E(il,OpCodes.Stfld,fFlash);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fBoard);E(il,OpCodes.Call,objImplicit);E(il,OpCodes.Brfalse,afterBoard);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fBoard);E(il,OpCodes.Ldfld,boardPM);E(il,OpCodes.Stfld,fPM);il.Append(afterBoard);
 // Native dereferences elementManager; absence is intentionally not hidden.
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fEM);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Callvirt,create);
 E(il,OpCodes.Ldc_I4_0);E(il,OpCodes.Stloc,i);E(il,OpCodes.Br,outerCheck);
 il.Append(outerBody);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fEM);E(il,OpCodes.Ldfld,fElems);E(il,OpCodes.Ldloc,i);E(il,OpCodes.Callvirt,elemGet);E(il,OpCodes.Stloc,e);E(il,OpCodes.Ldc_I4_0);E(il,OpCodes.Stloc,j);E(il,OpCodes.Br,innerCheck);
 il.Append(innerBody);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fUIs);E(il,OpCodes.Ldloc,j);E(il,OpCodes.Callvirt,uiGet);E(il,OpCodes.Stloc,u);E(il,OpCodes.Ldloc,e);E(il,OpCodes.Ldfld,fElemType);E(il,OpCodes.Ldloc,u);E(il,OpCodes.Ldfld,fUIType);E(il,OpCodes.Bne_Un,noMatch);E(il,OpCodes.Ldloc,e);E(il,OpCodes.Ldloc,u);E(il,OpCodes.Stfld,fElemUI);il.Append(noMatch);E(il,OpCodes.Ldloc,j);E(il,OpCodes.Ldc_I4_1);E(il,OpCodes.Add);E(il,OpCodes.Stloc,j);
 il.Append(innerCheck);E(il,OpCodes.Ldloc,j);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fUIs);E(il,OpCodes.Callvirt,uiCount);E(il,OpCodes.Blt,innerBody);E(il,OpCodes.Ldloc,i);E(il,OpCodes.Ldc_I4_1);E(il,OpCodes.Add);E(il,OpCodes.Stloc,i);
 il.Append(outerCheck);E(il,OpCodes.Ldloc,i);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fEM);E(il,OpCodes.Ldfld,fElems);E(il,OpCodes.Callvirt,elemCount);E(il,OpCodes.Blt,outerBody);il.Append(afterLoops);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,M(plant,"Start_Characteristic",0));E(il,OpCodes.Ret);
 Console.WriteLine("HF2 Plant.Start @0x18035A490: brightness, board manager, element creation/UI binding, characteristic init");
}

void PatchPlantStartCharacteristic(){
 var m=M(plant,"Start_Characteristic",0); var b=Fresh(m,true); var il=b.GetILProcessor();
 var fID=F(plant,"ID"), fOrder=F(plant,"order"), fAtk=F(plant,"attackable"), fBlock=F(plant,"blockable"), fBarrel=F(plant,"pT_chomperviking_barrelsPoint"), fSprites=F(plant,"animationSprites"), fUIs=F(plant,"UI_Characteristic"), fBuffMgr=F(plant,"buffManager"), fBoard=F(plant,"board"), fAnimator=F(plant,"animator");
 var stats=T("StatsIncreased"), buff=T("Buff"), buffMgr=T("BuffManager"); var ctor=M(stats,".ctor",3); var addBuff=M(buffMgr,"AddBuff",1); var fBuffType=F(buff,"buffType"), fBuffName=F(buff,"name");
 var hide=M(globals,"HideSprite",2); var updateBarrels=M(plant,"PC_ChompervikingUpdateBarrels",0); var setBool=AnyCall("UnityEngine.Animator","SetBool",2); var objEq=AllRefs().First(x=>x.DeclaringType.FullName=="UnityEngine.Object"&&x.Name=="op_Equality"&&x.Parameters.Count==2); var setActive=AnyCall("UnityEngine.GameObject","SetActive",1);
 var uiList=(GenericInstanceType)fUIs.FieldType; var uiCount=ListMethod(uiList,"get_Count",module.TypeSystem.Int32); var uiGet=ListMethod(uiList,"get_Item",uiList.GenericArguments[0],module.TypeSystem.Int32);
 var st=new VariableDefinition(stats);b.Variables.Add(st);
 var id5=Instruction.Create(OpCodes.Nop), id6=Instruction.Create(OpCodes.Nop), id49=Instruction.Create(OpCodes.Nop), ret=Instruction.Create(OpCodes.Ret), id6NoOrder=Instruction.Create(OpCodes.Nop), id6AfterUI=Instruction.Create(OpCodes.Nop), id4After=Instruction.Create(OpCodes.Nop);
 // ID 4
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fID);E(il,OpCodes.Ldc_I4_4);E(il,OpCodes.Bne_Un,id5);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldc_I4_0);E(il,OpCodes.Stfld,fAtk);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldc_I4_0);E(il,OpCodes.Stfld,fBlock);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fBoard);E(il,OpCodes.Ldnull);E(il,OpCodes.Call,objEq);E(il,OpCodes.Brfalse,ret);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fAnimator);E(il,OpCodes.Ldstr,"prepared");E(il,OpCodes.Ldc_I4_1);E(il,OpCodes.Callvirt,setBool);E(il,OpCodes.Br,ret);
 // ID 5
 il.Append(id5);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fID);E(il,OpCodes.Ldc_I4_5);E(il,OpCodes.Bne_Un,id6);
 foreach(var n in new[]{"SnowPea_3","SnowPea_5","SnowPea_6","SnowPea_8","SnowPea_9","SnowPea_10"}){E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fSprites);E(il,OpCodes.Ldstr,n);E(il,OpCodes.Call,hide);}E(il,OpCodes.Br,ret);
 // ID 6
 il.Append(id6);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fID);E(il,OpCodes.Ldc_I4_6);E(il,OpCodes.Bne_Un,id49);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fOrder);E(il,OpCodes.Ldc_I4_1);E(il,OpCodes.Blt,id6NoOrder);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldc_R4,0f);E(il,OpCodes.Stfld,fBarrel);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,updateBarrels);E(il,OpCodes.Br,ret);
 il.Append(id6NoOrder);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fSprites);E(il,OpCodes.Ldstr,"VikingChomper_Barrels");E(il,OpCodes.Call,hide);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fUIs);E(il,OpCodes.Brfalse,id6AfterUI);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fUIs);E(il,OpCodes.Callvirt,uiCount);E(il,OpCodes.Ldc_I4_0);E(il,OpCodes.Ble,id6AfterUI);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fUIs);E(il,OpCodes.Ldc_I4_0);E(il,OpCodes.Callvirt,uiGet);E(il,OpCodes.Ldc_I4_0);E(il,OpCodes.Callvirt,setActive);il.Append(id6AfterUI);E(il,OpCodes.Br,ret);
 // ID 49
 il.Append(id49);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fID);E(il,OpCodes.Ldc_I4,49);E(il,OpCodes.Bne_Un,ret);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldc_I4_0);E(il,OpCodes.Stfld,fAtk);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldc_I4_0);E(il,OpCodes.Stfld,fBlock);
 E(il,OpCodes.Ldstr,"Atk");E(il,OpCodes.Ldc_R4,0f);E(il,OpCodes.Ldc_I4_1);E(il,OpCodes.Newobj,ctor);E(il,OpCodes.Stloc,st);E(il,OpCodes.Ldloc,st);E(il,OpCodes.Ldc_I4_1);E(il,OpCodes.Stfld,fBuffType);E(il,OpCodes.Ldloc,st);E(il,OpCodes.Ldstr,"SSI_Characteristic_Atk");E(il,OpCodes.Stfld,fBuffName);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fBuffMgr);E(il,OpCodes.Ldloc,st);E(il,OpCodes.Callvirt,addBuff);il.Append(ret);
 Console.WriteLine("HF2 Plant.Start_Characteristic @0x18035A160: IDs 4/5/6/49 native branches restored, including attackable+blockable pair clears");
}

void PatchPlantUpdate(){
 var m=M(plant,"Update",0); var b=Fresh(m,true); var il=b.GetILProcessor();
 var fFlashT=F(plant,"flash_Time"), fFlashB=F(plant,"flash_Brightness"), fProduce=F(plant,"produce_Brightness"), fSelected=F(plant,"selected_Bright"), fActive=F(plant,"active"), fShadow=F(plant,"shadow"), fX=F(plant,"fX"), fY=F(plant,"fY"), fOn=F(plant,"isOnField"), fBoard=F(plant,"board"), fLiving=F(plant,"livingTime"), fRate=F(plant,"updateRate"), fSleep=F(plant,"isSleep"), fHpUI=F(plant,"hpUIController"), fEM=F(plant,"elementManager"), fDied=F(plant,"isDied"), fHP=F(plant,"healthPoint"), fID=F(plant,"ID"), fBarrel=F(plant,"pT_chomperviking_barrelsPoint"), fEaten=F(plant,"beEaten"), fState=F(plant,"state");
 var timeDelta=AnyCall("UnityEngine.Time","get_deltaTime",0); var setRate=M(plant,"SetUpdateRate",0); var setBright=M(plant,"SetBrightness",1); var mathMax=AllRefs().First(x=>x.DeclaringType.FullName=="System.Math"&&x.Name=="Max"&&x.Parameters.Count==2&&x.ReturnType.MetadataType==MetadataType.Single&&x.Parameters.All(p=>p.ParameterType.MetadataType==MetadataType.Single));
 var getTransform=AnyCall("UnityEngine.Component","get_transform",0); var getPos=AnyCall("UnityEngine.Transform","get_position",0); var v3=getPos.ReturnType; var vx=new FieldReference("x",module.TypeSystem.Single,v3), vy=new FieldReference("y",module.TypeSystem.Single,v3);
 var boardRuntime=M(board,"BoardRuntime",0); var boardFinished=F(board,"isFinished"); var hpUpdate=M(T("HPUIController_Plant"),"Update_HPUI",0); var fight=M(plant,"Update_PlantFight",0); var elemUpdate=M(T("ElementManager"),"Update",0); var healed=M(plant,"Healed",2); var barrelUpdate=M(plant,"PC_ChompervikingUpdateBarrels",0); var potato=M(plant,"PC_PotatoBoom",0); var die=M(plant,"PlantDie",0);
 var zm=T("ZombieManager"); var fZM=F(board,"zombieManager"), fAudio=F(zm,"audioClip_zombie"); var camMain=AnyCall("UnityEngine.Camera","get_main",0); var audioVol=M(globals,"AudioVolume",0); var createAudio=M(globals,"CreateAudioAtPoint",3);
 var a=new VariableDefinition(module.TypeSystem.Single), bb=new VariableDefinition(module.TypeSystem.Single), pos=new VariableDefinition(v3);b.Variables.Add(a);b.Variables.Add(bb);b.Variables.Add(pos);
 var noFlash=Instruction.Create(OpCodes.Nop), noSelected=Instruction.Create(OpCodes.Nop), noProduce=Instruction.Create(OpCodes.Nop), activeRet=Instruction.Create(OpCodes.Ret), afterRuntime=Instruction.Create(OpCodes.Nop), deathReset=Instruction.Create(OpCodes.Nop), notChomp=Instruction.Create(OpCodes.Nop), afterHeal=Instruction.Create(OpCodes.Nop), noEaten=Instruction.Create(OpCodes.Nop), normalDeath=Instruction.Create(OpCodes.Nop), reset=Instruction.Create(OpCodes.Nop), ret=Instruction.Create(OpCodes.Ret);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,setRate);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fFlashT);E(il,OpCodes.Ldc_R4,0f);E(il,OpCodes.Ble_Un,noFlash);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fFlashB);E(il,OpCodes.Call,timeDelta);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fFlashT);E(il,OpCodes.Div);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fFlashB);E(il,OpCodes.Ldc_R4,1f);E(il,OpCodes.Sub);E(il,OpCodes.Mul);E(il,OpCodes.Sub);E(il,OpCodes.Stfld,fFlashB);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fFlashT);E(il,OpCodes.Call,timeDelta);E(il,OpCodes.Sub);E(il,OpCodes.Stfld,fFlashT);il.Append(noFlash);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fFlashB);E(il,OpCodes.Stloc,a);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fProduce);E(il,OpCodes.Stloc,bb);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fSelected);E(il,OpCodes.Brfalse,noSelected);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fFlashB);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fProduce);E(il,OpCodes.Call,mathMax);E(il,OpCodes.Stloc,a);E(il,OpCodes.Ldc_R4,3f);E(il,OpCodes.Stloc,bb);il.Append(noSelected);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldloc,a);E(il,OpCodes.Ldloc,bb);E(il,OpCodes.Call,mathMax);E(il,OpCodes.Call,setBright);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fProduce);E(il,OpCodes.Ldc_R4,1f);E(il,OpCodes.Ble_Un,noProduce);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fProduce);E(il,OpCodes.Call,timeDelta);E(il,OpCodes.Ldc_R4,2f);E(il,OpCodes.Mul);E(il,OpCodes.Sub);E(il,OpCodes.Stfld,fProduce);il.Append(noProduce);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldc_I4_0);E(il,OpCodes.Stfld,fSelected);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fActive);E(il,OpCodes.Brfalse,activeRet);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fShadow);E(il,OpCodes.Callvirt,getTransform);E(il,OpCodes.Callvirt,getPos);E(il,OpCodes.Stloc,pos);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldloca,pos);E(il,OpCodes.Ldfld,vx);E(il,OpCodes.Stfld,fX);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldloca,pos);E(il,OpCodes.Ldfld,vy);E(il,OpCodes.Stfld,fY);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fOn);E(il,OpCodes.Brfalse,afterRuntime);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fBoard);E(il,OpCodes.Callvirt,boardRuntime);E(il,OpCodes.Brfalse,afterRuntime);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fBoard);E(il,OpCodes.Ldfld,boardFinished);E(il,OpCodes.Brtrue,afterRuntime);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fLiving);E(il,OpCodes.Call,timeDelta);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fRate);E(il,OpCodes.Mul);E(il,OpCodes.Add);E(il,OpCodes.Stfld,fLiving);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fSleep);E(il,OpCodes.Brtrue,afterRuntime);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fHpUI);E(il,OpCodes.Callvirt,hpUpdate);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,fight);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fEM);E(il,OpCodes.Callvirt,elemUpdate);il.Append(afterRuntime);
 // if already dead or HP >= 0, common active-path beEaten reset
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fDied);E(il,OpCodes.Brtrue,reset);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fHP);E(il,OpCodes.Ldc_R4,0f);E(il,OpCodes.Bge_Un,reset);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fID);E(il,OpCodes.Ldc_I4_6);E(il,OpCodes.Bne_Un,notChomp);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fHP);E(il,OpCodes.Neg);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fBarrel);E(il,OpCodes.Bgt_Un,notChomp);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fBarrel);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,healed);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldc_R4,0f);E(il,OpCodes.Stfld,fBarrel);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,barrelUpdate);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fHP);E(il,OpCodes.Ldc_R4,0f);E(il,OpCodes.Bge_Un,reset);il.Append(notChomp);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fEaten);E(il,OpCodes.Brfalse,noEaten);
 // audioClip_zombie[24]
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fBoard);E(il,OpCodes.Ldfld,fZM);E(il,OpCodes.Ldfld,fAudio);
 if(fAudio.FieldType is ArrayType){E(il,OpCodes.Ldc_I4,24);E(il,OpCodes.Ldelem_Ref);}else{var l=(GenericInstanceType)fAudio.FieldType;E(il,OpCodes.Ldc_I4,24);E(il,OpCodes.Callvirt,ListMethod(l,"get_Item",l.GenericArguments[0],module.TypeSystem.Int32));}
 E(il,OpCodes.Call,camMain);E(il,OpCodes.Callvirt,getTransform);E(il,OpCodes.Callvirt,getPos);E(il,OpCodes.Call,audioVol);E(il,OpCodes.Call,createAudio);if(createAudio.ReturnType.MetadataType!=MetadataType.Void)E(il,OpCodes.Pop);il.Append(noEaten);
 E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fID);E(il,OpCodes.Ldc_I4_4);E(il,OpCodes.Bne_Un,normalDeath);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldfld,fState);E(il,OpCodes.Brtrue,normalDeath);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,potato);E(il,OpCodes.Br,ret);
 il.Append(normalDeath);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Call,die);il.Append(reset);E(il,OpCodes.Ldarg_0);E(il,OpCodes.Ldc_I4_0);E(il,OpCodes.Stfld,fEaten);E(il,OpCodes.Br,ret);il.Append(activeRet);il.Append(ret);
 Console.WriteLine("HF2 Plant.Update @0x18035C4D0: native brightness/runtime/fight/death/chomper/audio/potato flow restored");
}

'''
s=s.replace(marker,hf2+marker)
p.write_text(s)
print('updated',p,'bytes',p.stat().st_size)