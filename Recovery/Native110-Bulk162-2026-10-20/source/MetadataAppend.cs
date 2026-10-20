using System.Buffers.Binary;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

internal static partial class Program
{
    private const int CardIndexOf = 0x0A00044D;
    private const int CardRemoveAt = 0x0A00044E;
    private const int EnemyEditorAdd = 0x0A00044F;
    private const int PathEditorAdd = 0x0A000450;
    private const int DeviceEditorAdd = 0x0A000451;
    private const int PreDeviceAdd = 0x0A000452;
    private const int PathEditorCount = 0x0A000453;
    private const int SkillTryGetValue = 0x0A000454;
    private const int ResetLocals = 0x110006E6;
    private const int SkillLocals = 0x110006E7;
    private const int HighlightLocals = 0x110006E8;
    private const int RangeLocals = 0x110006E9;
    private const int RectListCount = 0x0A000455;
    private const int RectListItem = 0x0A000456;
    private const int RadiusListCount = 0x0A000457;
    private const int CenterListCount = 0x0A000458;
    private const int FlagListCount = 0x0A000459;
    private const int WaveListCount = 0x0A00045A;
    private const int ProjectListCount = 0x0A00045B;
    private const int SkillUpListCount = 0x0A00045C;
    private const int RectXMin = 0x0A00045D;
    private const int RectYMin = 0x0A00045E;
    private const int RectWidth = 0x0A00045F;
    private const int RectHeight = 0x0A000460;
    private const int Locals_SelectZombieTypesBody = 0x110006EA;
    private const int Locals_RectIntersectionBody = 0x110006EB;
    private const int Locals_CircleIntersectionBody = 0x110006EC;
    private const int Locals_ZombieUpdateRateBody = 0x110006ED;
    private const int Locals_SmoothAnglesBody = 0x110006EE;
    private const int Locals_EnemyWaveBody = 0x110006EF;
    private const int Locals_UpdateBlockBody = 0x110006F0;
    private const int Locals_SkillRunningBody = 0x110006F1;
    private const int Locals_PropCoolingBody = 0x110006F2;
    private const int Locals_DeviceLayerBody = 0x110006F3;
    private const int Locals_ChangeTypeBody = 0x110006EF;
    private const int Locals_ChangeChargeBody = 0x110006EF;
    private const int Locals_ChangeTriggerBody = 0x110006EF;
    private const int Locals_PoleCommanderBody = 0x110006F4;
    private const int Locals_DropLootBody = 0x110006F5;
    private const int Locals_ZombieDefenseBody = 0x110006F6;
    private const int Locals_TouchSunBody = 0x110006F7;
    private const int Locals_TouchProjectBody = 0x110006F7;
    private const int Locals_UnsubscribeStoppedBody = 0x110006F8;
    private const int Locals_UpdatePlantSkillBody = 0x110006F9;
    private const int Locals_SkillAwakeBody = 0x110006FA;
    private const int Locals_SkillEndBody = 0x110006FB;
    private const int Locals_SkillPassiveBody = 0x110006FC;
    private const int Locals_ProjectileRangeBody = 0x110006FD;
    private const int Locals_AccelerationBody = 0x110006FE;
    private const int Locals_NextChallengeBody = 0x110006FF;
    private const int Locals_SkillTargetButtonBody = 0x11000700;
    private const int Locals_ArmorAudioBody = 0x11000701;
    private const int Locals_EmptyChoiceCardBody = 0x11000702;
    private const int Locals_HighlightZombieBody = 0x11000703;
    private const int Locals_WalkZombieBody = 0x11000704;
    private const int Locals_ToggleEliteBody = 0x11000705;
    private const int Locals_ToggleBossBody = 0x11000705;
    private const int Locals_ToggleMooksBody = 0x11000705;
    private const int Locals_CheckDeviceBody = 0x11000706;
    private const int Locals_CheckZombieBody = 0x11000707;
    private const int AwakeTryGet = 0x0A000461;
    private const int EndTryGet = 0x0A000462;
    private const int PassiveTryGet = 0x0A000463;
    private const int OrderListCount = 0x0A000464;
    private const int ArmorClipCount = 0x0A000465;
    private const int DeviceWindowEnumerator = 0x0A000466;
    private const int DeviceWindowCurrent = 0x0A000467;
    private const int DeviceWindowMove = 0x0A000468;
    private const int ZombieWindowEnumerator = 0x0A000469;
    private const int ZombieWindowCurrent = 0x0A00046A;
    private const int ZombieWindowMove = 0x0A00046B;
    private const int ChallengeArrayIndexOf = 0x2B0000BA;
    private const int Locals_AllZombiePathsBody = 0x11000708;
    private const int Locals_CollectProjectBody = 0x11000709;
    private const int Locals_InitializePlantSaveBody = 0x1100070A;
    private const int Locals_FrameCounterPositionBody = 0x1100070B;
    private const int Locals_CollectAllProjectsBody = 0x1100070C;
    private const int Locals_FindZombiePathBody = 0x1100070D;
    private const int Locals_ClearPlantCardsBody = 0x1100070E;
    private const int Locals_ClearEntriesBody = 0x1100070F;
    private const int Locals_ClearSuppliesBody = 0x11000710;
    private const int Locals_CreateAstarNodesBody = 0x11000711;
    private const int Locals_SaveTeamCardsBody = 0x11000712;
    private const int Locals_CloseSkillPreviewBody = 0x11000713;
    private const int Locals_PathDistanceBody = 0x11000714;
    private const int Locals_CloneEnemyPathBody = 0x11000715;
    private const int Locals_OrderUpPlantBody = 0x11000716;
    private const int Locals_CloseVfxBody = 0x11000717;
    private const int SuppliesEnumerator = 0x0A00046C;
    private const int SuppliesCurrent = 0x0A00046D;
    private const int SuppliesMove = 0x0A00046E;
    private const int SuppliesClear = 0x0A00046F;
    private const int EntryEnumerator = 0x0A000470;
    private const int EntryCurrent = 0x0A000471;
    private const int EntryMove = 0x0A000472;
    private const int EntryDispose = 0x0A000473;
    private const int EntryClear = 0x0A000474;
    private const int SkillButtonsEnumerator = 0x0A000475;
    private const int SkillButtonsCurrent = 0x0A000476;
    private const int SkillButtonsMove = 0x0A000477;
    private const int IntListClear = 0x0A000478;
    private const int Locals_SpinObjectBody = 0x11000718;
    private const int Locals_RandomIceCubeBody = 0x11000719;
    private const int Locals_DeviceUnderMouseBody = 0x1100071A;
    private const int Locals_SeekTargetPlantBody = 0x1100071B;
    private const int Locals_MoveEnemyDataBody = 0x1100071C;
    private const int Locals_CreateDamageTextBody = 0x1100071D;
    private const int Locals_PlaceDeviceBody = 0x1100071E;
    private const int Locals_UpdateProjectileSpeedBody = 0x1100071F;
    private const int Locals_PlacePreDevicesBody = 0x11000720;
    private const int Locals_SelectPropBody = 0x11000721;
    private const int Locals_CreateSuppliesBody = 0x11000722;
    private const int Locals_RandomEnemyRowBody = 0x11000723;
    private const int Locals_GetMapGridsBody = 0x11000724;
    private const int Locals_PlantDamagePointBody = 0x11000725;
    private const int Locals_SunflowerHealBody = 0x11000726;
    private const int Locals_SunFallBody = 0x11000727;
    private const int Locals_PlantDamageTypeBody = 0x11000728;
    private const int Locals_ProjectSpeedStartBody = 0x11000729;
    private const int Locals_BlockZombieBody = 0x1100072A;
    private const int PreDeviceEnumerator = 0x0A000479;
    private const int PreDeviceCurrent = 0x0A00047A;
    private const int PreDeviceMove = 0x0A00047B;
    private const int PreDeviceCount = 0x0A00047C;
    private const int PropControllerEnumerator = 0x0A00047D;
    private const int PropControllerCurrent = 0x0A00047E;
    private const int PropControllerMove = 0x0A00047F;
    private const int PropControllerCount = 0x0A000480;
    private const int ZombieListContains = 0x0A000481;
    private const int Locals_SetSwfCurrentFrameBody = 0x1100072B;
    private const int Locals_GetSwfFrameCountBody = 0x1100072B;
    private const int Locals_PreviousSwfFrameBody = 0x1100072B;
    private const int Locals_NextSwfFrameBody = 0x1100072C;
    private const int Locals_ChangeSwfCurrentFrameBody = 0x1100072C;
    private const int Locals_ControllerNextSwfFrameBody = 0x1100072D;
    private const int SwfFramesCount = 0x0A000482;
    private const int SwfFrameChangedInvoke = 0x0A000483;
    private const int Locals_PassLawnLevelBody = 0x1100072E;
    private const int Locals_SetDeviceContentTypeBody = 0x1100072F;
    private const int Locals_ShovelPlantBody = 0x11000730;
    private const int Locals_RewindSwfBody = 0x11000731;
    private const int Locals_ZombieHeadPositionBody = 0x11000732;
    private const int Locals_SetHardAdventureStateBody = 0x11000733;
    private const int Locals_ReceiveSuppliesBody = 0x11000734;
    private const int Locals_ZombieDamageRangeBody = 0x11000735;
    private const int Locals_MouseCollectProjectBody = 0x11000736;
    private const int Locals_SeekDeviceZombieBody = 0x11000737;
    private const int Locals_CreateBoardEntriesBody = 0x11000738;
    private const int Locals_PlantSameOccupancyBody = 0x11000739;
    private const int Locals_CreateWarehouseSuppliesBody = 0x1100073A;
    private const int Locals_CheckEditorWaveBody = 0x1100073B;
    private const int Locals_LoadEnemyEditorBody = 0x1100073C;
    private const int Locals_TriggerPlantSkillBody = 0x1100073D;
    private const int Locals_GridPassablePointBody = 0x1100073E;
    private const int Locals_DeviceBrightnessBody = 0x1100073F;
    private const int Locals_UpdateMoneyBanksBody = 0x11000740;
    private const int Locals_SaveTeamPropsBody = 0x11000741;
    private const int SuppliesListRemove = 0x0A000484;
    private const int SavedSuppliesEnumerator = 0x0A000485;
    private const int SavedSuppliesCurrent = 0x0A000486;
    private const int SavedSuppliesMove = 0x0A000487;
    private const int EnemyEditorEnumerator = 0x0A000488;
    private const int EnemyEditorCurrent = 0x0A000489;
    private const int EnemyEditorMove = 0x0A00048A;
    private const int SpriteListCount = 0x0A00048B;
    private const int CardSunCost = 0x04000406;
    private const int MouseWorldPosition = 0x0400023A;
    private const int ZombieCircleRange = 0x2B0000BB;
    private const int HeadPea = 0x70032F12;
    private const int HeadPole = 0x70032F32;
    private const int HeadLadder = 0x70032F64;
    private const int N108_Function148Locals = 0x11000742;
    private const int N108_SkewConstructorLocals = 0x11000743;
    private const int N108_PreviewSkillUpLocals = 0x11000744;
    private const int N108_PreviewNewSkillLocals = 0x11000745;
    private const int N108_SkillButtonCount = 0x0A00048C;
    private const int N109_DeviceEventUpdateLocals = 0x11000746;
    private const int N109_ClimbLadderUpdateLocals = 0x11000747;
    private const int N109_PreviewMouseDownLocals = 0x11000748;
    private const int N109_DeviceEventListAdd = 0x0A00048D;
    private const int N110_DeviceListCount = 0x0A00048E;
    private const int N110_DeviceListAdd = 0x0A00048F;
    private const int N110_ZombieListAdd = 0x0A000490;
    private const int N110_SetParent = 0x0A000491;
    private const int N110_Vector4Ctor = 0x0A000492;
    private const int N110_Locals1 = 0x11000749;
    private const int N110_Locals2 = 0x1100074A;
    private const int N110_Locals3 = 0x1100074B;
    private const int N110_Locals4 = 0x1100074C;
    private const int N110_Locals5 = 0x1100074D;
    private const int N110_Locals6 = 0x1100074E;
    private const int N110_Locals7 = 0x1100074F;
    private const int N110_Locals8 = 0x11000750;
    private record MemberAddition(int Token, int ClassCode, string OwnerPin, int Template,
        string TemplateName, string Signature, string? NewName);
    private static readonly MemberAddition[] MemberAdditions = [
        new(0x0A00044D,252,"15120d011281b4",0xAD,"IndexOf","2001081300",null),
        new(0x0A00044E,252,"15120d011281b4",0x72,"RemoveAt","20010108",null),
        new(0x0A00044F,1332,"15120d011224",0xBF,"Add","2001011300",null),
        new(0x0A000450,100,"15120d01122c",0xBF,"Add","2001011300",null),
        new(0x0A000451,1324,"15120d01121c",0xBF,"Add","2001011300",null),
        new(0x0A000452,420,"15120d011280f8",0xBF,"Add","2001011300",null),
        new(0x0A000453,100,"15120d01122c",0xA1,"get_Count","200008",null),
        new(0x0A000454,788,"15128111020812824c",0x240,"TryGetValue","2002021300101301",null),
        new(0x0A000455,244,"15120d011180cd",0x144,"get_Count","200008",null),
        new(0x0A000456,244,"15120d011180cd",0x99,"get_Item","2001130008",null),
        new(0x0A000457,228,"15120d010c",0x144,"get_Count","200008",null),
        new(0x0A000458,236,"15120d01114d",0x144,"get_Count","200008",null),
        new(0x0A000459,508,"15120d01128110",0x144,"get_Count","200008",null),
        new(0x0A00045A,516,"15120d01128114",0x144,"get_Count","200008",null),
        new(0x0A00045B,532,"15120d011281ec",0x144,"get_Count","200008",null),
        new(0x0A00045C,644,"15120d01128160",0x144,"get_Count","200008",null),
        new(0x0A00045D,409,"UnityEngine.Rect",0x299,"get_fillAmount","20000c","get_xMin"),
        new(0x0A00045E,409,"UnityEngine.Rect",0x299,"get_fillAmount","20000c","get_yMin"),
        new(0x0A00045F,409,"UnityEngine.Rect",0x299,"get_fillAmount","20000c","get_width"),
        new(0x0A000460,409,"UnityEngine.Rect",0x299,"get_fillAmount","20000c","get_height"),
        new(0x0A000461,780,"151281110208128248",0x240,"TryGetValue","2002021300101301",null),
        new(0x0A000462,804,"151281110208128254",0x240,"TryGetValue","2002021300101301",null),
        new(0x0A000463,812,"151281110208128258",0x240,"TryGetValue","2002021300101301",null),
        new(0x0A000464,636,"15120d0112815c",0x144,"get_Count","200008",null),
        new(0x0A000465,212,"15120d011280b5",0x144,"get_Count","200008",null),
        new(0x0A000466,852,"15120d0112828c",0xB7,"GetEnumerator","2000151151011300",null),
        new(0x0A000467,860,"1511510112828c",0xB8,"get_Current","20001300",null),
        new(0x0A000468,860,"1511510112828c",0xB9,"MoveNext","200002",null),
        new(0x0A000469,916,"15120d011282a8",0xB7,"GetEnumerator","2000151151011300",null),
        new(0x0A00046A,924,"151151011282a8",0xB8,"get_Current","20001300",null),
        new(0x0A00046B,924,"151151011282a8",0xB9,"MoveNext","200002",null),
        new(0x0A00046C,868,"15120d01128204",0xB7,"GetEnumerator","2000151151011300",null),
        new(0x0A00046D,404,"15115101128204",0xB8,"get_Current","20001300",null),
        new(0x0A00046E,404,"15115101128204",0xB9,"MoveNext","200002",null),
        new(0x0A00046F,868,"15120d01128204",0xC0,"Clear","200001",null),
        new(0x0A000470,948,"15120d011282e4",0xB7,"GetEnumerator","2000151151011300",null),
        new(0x0A000471,1404,"151151011282e4",0xB8,"get_Current","20001300",null),
        new(0x0A000472,1404,"151151011282e4",0xB9,"MoveNext","200002",null),
        new(0x0A000473,1404,"151151011282e4",0x45,"Dispose","200001",null),
        new(0x0A000474,948,"15120d011282e4",0xC0,"Clear","200001",null),
        new(0x0A000475,884,"15120d0112833c",0xB7,"GetEnumerator","2000151151011300",null),
        new(0x0A000476,892,"1511510112833c",0xB8,"get_Current","20001300",null),
        new(0x0A000477,892,"1511510112833c",0xB9,"MoveNext","200002",null),
        new(0x0A000478,172,"15120d0108",0xC0,"Clear","200001",null),
        new(0x0A000479,420,"15120d011280f8",0xB7,"GetEnumerator","2000151151011300",null),
        new(0x0A00047A,60,"151151011280f8",0xB8,"get_Current","20001300",null),
        new(0x0A00047B,60,"151151011280f8",0xB9,"MoveNext","200002",null),
        new(0x0A00047C,420,"15120d011280f8",0x147,"get_Count","200008",null),
        new(0x0A00047D,972,"15120d0112831c",0xB7,"GetEnumerator","2000151151011300",null),
        new(0x0A00047E,964,"1511510112831c",0xB8,"get_Current","20001300",null),
        new(0x0A00047F,964,"1511510112831c",0xB9,"MoveNext","200002",null),
        new(0x0A000480,972,"15120d0112831c",0x147,"get_Count","200008",null),
        new(0x0A000481,164,"15120d01128210",0x1F6,"Contains","2001021300",null),
        new(0x0A000482,1156,"15120d0112847c",0x147,"get_Count","200008",null),
        new(0x0A000483,1180,"151282590112846c",0x3FE,"Invoke","2001011300",null),
        new(0x0A000484,452,"15120d01128234",0x36,"Remove","2001021300",null),
        new(0x0A000485,452,"15120d01128234",0xB7,"GetEnumerator","2000151151011300",null),
        new(0x0A000486,460,"15115101128234",0xB8,"get_Current","20001300",null),
        new(0x0A000487,460,"15115101128234",0xB9,"MoveNext","200002",null),
        new(0x0A000488,1332,"15120d011224",0xB7,"GetEnumerator","2000151151011300",null),
        new(0x0A000489,44,"151151011224",0xB8,"get_Current","20001300",null),
        new(0x0A00048A,44,"151151011224",0xB9,"MoveNext","200002",null),
        new(0x0A00048B,76,"15120d01125d",0x147,"get_Count","200008",null),
        new(0x0A00048C,884,"15120d0112833c",0x8,"get_Count","200008",null),
        new(0x0A00048D,724,"15120d0112809c",0xBF,"Add","2001011300",null),
        new(0x0A00048E,340,"15120d011281d4",0x8,"get_Count","200008",null),
        new(0x0A00048F,340,"15120d011281d4",0xBF,"Add","2001011300",null),
        new(0x0A000490,164,"15120d01128210",0xBF,"Add","2001011300",null),
        new(0x0A000491,41,"UnityEngine.Transform",0x6E,"set_parentInternal","2001011215","SetParent"),
        new(0x0A000492,1241,"UnityEngine.Vector4",0xBE,".ctor","2004010c0c0c0c",null),
    ];
    private static readonly string[] NewLocalBlobs = [
        "070212821808",
        "07020812824c",
        "07011281e0",
        "0707080c0c0c0c0c0c",
        "070315120d0112811c0808",
        "0708114d081180cd1180cd1180cd1180cd0c0c",
        "070708114d114d0c0c0c0c",
        "07020c1280ac",
        "07090c0c0c0c0c0c0c0c0c",
        "07020808",
        "070208128210",
        "07030c0c1274",
        "07040c0c0c1275",
        "07020c08",
        "0703127012740c",
        "07050c08080c128234",
        "07030c0c0d",
        "0707081281ec114d0c0c0c0c",
        "0701128484",
        "07030c0c1274",
        "070208128248",
        "070208128254",
        "070208128258",
        "0708080c0c0c0c0c0c0c",
        "07060c0c0c0c0c114d",
        "07021d1180dc08",
        "07050202021280850c",
        "0703081280b5114d",
        "07021d127508",
        "07021282101282b4",
        "07011280c1",
        "0704151151011282a81282a81280d408",
        "07031511510112828c12828c02",
        "0703151151011282a81282a802",
        "070215115101128210128210",
        "070108",
        "070212822808",
        "07050c0c081215121d",
        "0702151151011281ec1281ec",
        "070115120d0112819c",
        "0702151151011281c01281c0",
        "0702151151011282e41282e4",
        "070215115101128204128204",
        "070615120d01128198080812818c128190128198",
        "070411821c15120d010815120d010808",
        "07031511510112833c12833c128085",
        "0709080808080808080808",
        "070215120d0112819c1511510112819c",
        "070415115101128204128204128234128218",
        "070115120d011209",
        "07090c0c12150c0c0c0c114d08",
        "0707080808080d0d0d",
        "07051280fc1281901281d40808",
        "0705151151011281e01281e01281e00c0c",
        "070408081511510112819c12819c",
        "07021282d40e",
        "070312814c1282181281d4",
        "0704080c0c0c",
        "070408151151011280f81280f81280f8",
        "07021511510112831c12831c",
        "070312816c1280d0128234",
        "07051281880812818c081d02",
        "070615120d011281901511510112818c12818c080808",
        "07020c08",
        "070408081281900c",
        "07030c12750c",
        "070108",
        "070608114d114d0c0c0c",
        "0700",
        "0700",
        "07020808",
        "070108",
        "070215115101128204128204",
        "070212651181a0",
        "07041281b4080c1281ec",
        "07020812846c",
        "07020e08",
        "070408120902151151011209",
        "07050812821812823412822812822c",
        "0702126008",
        "070b1281ec128144114d08114d0c0c0c0c0c0c",
        "070512601282101282100c15115101128210",
        "0703151151011282dc1282dc1282e4",
        "07041280c91281e01281e01280d9",
        "070315115101128234128234128204",
        "0703151151011224122402",
        "070615120d01125d1275125d1280e0121515115101122c",
        "0703151151011281e01281e01274",
        "070108",
        "07021511510112091281dc",
        "07040c0c128085128119",
        "070411821c1511510112831c12831c08",
        "0701128094",
        "07011d118249",
        "07031282441511510112833c08",
        "0703080812833c",
        "07030c1280a01280a4",
        "07031282100c0c",
        "07021282b4128210",
        "07031280b51209114d",
        "07041281d4114d0815120d011281d4",
        "0703128210114d08",
        "07031280d402128210",
        "07011209",
        "070212808111812d",
        "070311812d1215121d",
        "07061d114d1209114d114d124c1281e0",
    ];

    private static readonly string[] AddedTypeBlobs = ["1d1180dc","151151011282e4"];
    private static readonly string[] AddedUserStrings = ["PeaShooter_Head","Zombie_polevaulter__head","LadderSaboteurs_head"];
    private static byte[] AppendMetadata(byte[] before, PEReader pe, out object evidence)
    {
        var h = pe.PEHeaders; var md = pe.GetMetadataReader();
        const int baseMembers = 1100, baseLocals = 1765;
        Require(md.GetTableRowCount(TableIndex.MemberRef)==baseMembers && md.GetTableRowSize(TableIndex.MemberRef)==6 &&
            md.GetTableRowCount(TableIndex.StandAloneSig)==baseLocals && md.GetTableRowSize(TableIndex.StandAloneSig)==2,
            "Pinned parent table counts and index widths");
        Require(baseMembers+MemberAdditions.Length<2048 && baseLocals+NewLocalBlobs.Length<2048,
            "No metadata coded-index width transition");
        Require(md.GetTableRowCount(TableIndex.TypeSpec)==173 && md.GetTableRowSize(TableIndex.TypeSpec)==2 &&
            md.GetTableRowCount(TableIndex.MethodSpec)==185 && md.GetTableRowSize(TableIndex.MethodSpec)==4,"Pinned additional tables");
        int oldRoot=FileOffset(h,h.CorHeader!.MetadataDirectory.RelativeVirtualAddress), oldLength=h.CorHeader.MetadataDirectory.Size;
        byte[] old=before[oldRoot..checked(oldRoot+oldLength)]; Require(Read32(old,0)==0x424A5342,"Metadata root signature");
        int p=Align(checked(16+Read32(old,12)),4), count=BinaryPrimitives.ReadUInt16LittleEndian(old.AsSpan(p+2,2));p+=4;
        var streams=new List<(int Header,string Name,int Offset,int Size)>();
        for(int i=0;i<count;i++)
        {
            int end=Array.IndexOf(old,(byte)0,p+8);Require(end>=p+8,"Terminated stream name");
            streams.Add((p,System.Text.Encoding.UTF8.GetString(old,p+8,end-p-8),Read32(old,p),Read32(old,p+4)));p=Align(end+1,4);
        }
        Require(streams.Select(s=>s.Name).Distinct().Count()==count && streams.All(s=>s.Offset>=p && s.Size>=0 && s.Offset<=oldLength-s.Size),
            "Unique bounded metadata streams");
        var ordered=streams.OrderBy(s=>s.Offset).ToArray();
        for(int i=1;i<ordered.Length;i++)Require(ordered[i-1].Offset+ordered[i-1].Size<=ordered[i].Offset,"Non-overlapping streams");
        var tables=streams.Single(s=>s.Name=="#~");var blobs=streams.Single(s=>s.Name=="#Blob");var strings=streams.Single(s=>s.Name=="#Strings");
        Require((old[tables.Offset+6]&0x47)==0,"Two-byte heaps and no ExtraData");
        ulong valid=BinaryPrimitives.ReadUInt64LittleEndian(old.AsSpan(tables.Offset+8,8));
        ulong sorted=BinaryPrimitives.ReadUInt64LittleEndian(old.AsSpan(tables.Offset+16,8));
        Require((valid&(1UL<<10))!=0 && (valid&(1UL<<17))!=0 && (sorted&(1UL<<10))==0,"Appendable tables");
        int memberStart=md.GetTableMetadataOffset(TableIndex.MemberRef),memberEnd=checked(memberStart+baseMembers*6);
        int sigEnd=checked(md.GetTableMetadataOffset(TableIndex.StandAloneSig)+baseLocals*2),tableEnd=checked(tables.Offset+tables.Size);
        Require(memberStart>=tables.Offset+24 && memberEnd<=sigEnd && sigEnd<=tableEnd,"Known table order");
        Require((valid&(1UL<<27))!=0 && (valid&(1UL<<43))!=0 && (sorted&(1UL<<43))==0,"Appendable generic tables");
        int typeEnd=checked(md.GetTableMetadataOffset(TableIndex.TypeSpec)+173*2);
        int methodSpecEnd=checked(md.GetTableMetadataOffset(TableIndex.MethodSpec)+185*4);
        Require(sigEnd<=typeEnd && typeEnd<=methodSpecEnd && methodSpecEnd<=tableEnd,"Additional table order");
        byte[] memberRows=new byte[MemberAdditions.Length*6];var names=new List<byte>();
        for(int i=0;i<MemberAdditions.Length;i++)
        {
            var d=MemberAdditions[i];Require(d.Token==0x0A000000+baseMembers+i+1,"Append-only member RID");
            var t=md.GetMemberReference(MetadataTokens.MemberReferenceHandle(d.Template));
            Require(md.GetString(t.Name)==d.TemplateName && md.GetBlobBytes(t.Signature).SequenceEqual(Convert.FromHexString(d.Signature)),
                "Canonical member name/signature pin");
            int tag=d.ClassCode&7,rid=d.ClassCode>>3;
            if(tag==4)
            {
                Require(rid>0 && rid<=173+AddedTypeBlobs.Length,"Bounded old/new declaring TypeSpec RID");
                byte[] parentBlob=rid<=173 ? md.GetBlobBytes(md.GetTypeSpecification(MetadataTokens.TypeSpecificationHandle(rid)).Signature)
                    : Convert.FromHexString(AddedTypeBlobs[rid-174]);
                Require(parentBlob.SequenceEqual(Convert.FromHexString(d.OwnerPin)),"Declaring TypeSpec pin including appended rows");
            }
            else
            {
                Require(tag==1,"Explicit supported MemberRefParent kind");var ty=md.GetTypeReference(MetadataTokens.TypeReferenceHandle(rid));
                Require(md.GetString(ty.Namespace)+"."+md.GetString(ty.Name)==d.OwnerPin,"Declaring TypeRef pin");
            }
            old.AsSpan(memberStart+(d.Template-1)*6,6).CopyTo(memberRows.AsSpan(i*6));
            BinaryPrimitives.WriteUInt16LittleEndian(memberRows.AsSpan(i*6,2),checked((ushort)d.ClassCode));
            if(d.NewName is not null)
            {
                int handle=checked(strings.Size+names.Count);names.AddRange(System.Text.Encoding.UTF8.GetBytes(d.NewName));names.Add(0);
                BinaryPrimitives.WriteUInt16LittleEndian(memberRows.AsSpan(i*6+2,2),checked((ushort)handle));
            }
        }
        var blobSuffix=new List<byte>();byte[] sigRows=new byte[NewLocalBlobs.Length*2];
        for(int i=0;i<NewLocalBlobs.Length;i++)
        {
            byte[] data=Convert.FromHexString(NewLocalBlobs[i]);Require(data.Length<128 && data.Length>=2 && data[0]==7,"Short local signature blob");
            BinaryPrimitives.WriteUInt16LittleEndian(sigRows.AsSpan(i*2,2),checked((ushort)(blobs.Size+blobSuffix.Count)));
            blobSuffix.Add(checked((byte)data.Length));blobSuffix.AddRange(data);
        }
        byte[] typeRows=new byte[checked(AddedTypeBlobs.Length*2)];
        for(int i=0;i<AddedTypeBlobs.Length;i++)
        {
            byte[] typeBlob=Convert.FromHexString(AddedTypeBlobs[i]);Require(typeBlob.Length is >0 and <128,"Short appended TypeSpec blob");
            BinaryPrimitives.WriteUInt16LittleEndian(typeRows.AsSpan(i*2,2),checked((ushort)(blobs.Size+blobSuffix.Count)));
            blobSuffix.Add(checked((byte)typeBlob.Length));blobSuffix.AddRange(typeBlob);
        }
        var ms=md.GetMethodSpecification(MetadataTokens.MethodSpecificationHandle(13));
        Require(MetadataTokens.GetToken(ms.Method)==0x0A00007E &&
            md.GetBlobBytes(ms.Signature).SequenceEqual(Convert.FromHexString("0a011180b1")),"Canonical Array.IndexOf MethodSpec pin");
        var indexOf=md.GetMemberReference(MetadataTokens.MemberReferenceHandle(126));
        Require(md.GetString(indexOf.Name)=="IndexOf" && md.GetBlobBytes(indexOf.Signature).SequenceEqual(Convert.FromHexString("100102081d1e001e00")),"Generic Array.IndexOf signature");
        byte[] methodRows=old[(md.GetTableMetadataOffset(TableIndex.MethodSpec)+12*4)..(md.GetTableMetadataOffset(TableIndex.MethodSpec)+13*4)];
        BinaryPrimitives.WriteUInt16LittleEndian(methodRows.AsSpan(2),checked((ushort)(blobs.Size+blobSuffix.Count)));
        byte[] instantiation=Convert.FromHexString("0a011180dc");blobSuffix.Add(checked((byte)instantiation.Length));blobSuffix.AddRange(instantiation);
        var circle=md.GetMethodSpecification(MetadataTokens.MethodSpecificationHandle(86));
        Require(MetadataTokens.GetToken(circle.Method)==0x0A0001E4 &&
            md.GetBlobBytes(circle.Signature).SequenceEqual(Convert.FromHexString("0a011281f4")),"Pinned old Plant circle MethodSpec");
        var circleMember=md.GetMemberReference(MetadataTokens.MemberReferenceHandle(484));
        Require(MetadataTokens.GetToken(circleMember.Parent)==0x02000018 && md.GetString(circleMember.Name)=="NewCircleRange" &&
            md.GetBlobBytes(circleMember.Signature).SequenceEqual(Convert.FromHexString("10010312601e0012150c")),"Pinned generic circle definition");
        byte[] bothMethodRows=new byte[8];methodRows.CopyTo(bothMethodRows,0);
        old.AsSpan(md.GetTableMetadataOffset(TableIndex.MethodSpec)+85*4,4).CopyTo(bothMethodRows.AsSpan(4));
        BinaryPrimitives.WriteUInt16LittleEndian(bothMethodRows.AsSpan(6),checked((ushort)(blobs.Size+blobSuffix.Count)));
        byte[] zombieInstantiation=Convert.FromHexString("0a01128210");blobSuffix.Add(checked((byte)zombieInstantiation.Length));blobSuffix.AddRange(zombieInstantiation);
        methodRows=bothMethodRows;
        var userStream=streams.Single(s=>s.Name=="#US");Require(userStream.Size==208658,"Pinned original user string heap size");
        var userSuffix=new List<byte>();int[] userTokens=[HeadPea,HeadPole,HeadLadder];
        for(int i=0;i<AddedUserStrings.Length;i++)
        {
            string value=AddedUserStrings[i];Require(value.All(ch=>ch is >= 'A' and <= 'Z' or >= 'a' and <= 'z' or '_'),"Simple pinned UTF16 literal with terminal flag zero");
            int offset=checked(userStream.Size+userSuffix.Count);Require(offset<0x1000000 && userTokens[i]==0x70000000+offset,"Pinned append-only user string token");
            byte[] literal=System.Text.Encoding.Unicode.GetBytes(value);Require(literal.Length+1<128,"Short user string payload");
            userSuffix.Add(checked((byte)(literal.Length+1)));userSuffix.AddRange(literal);userSuffix.Add(0);
        }
        Require(blobs.Size+blobSuffix.Count<65536 && strings.Size+names.Count<65536,"No heap index widening");
        using var tableStream=new MemoryStream();
        tableStream.Write(old,tables.Offset,memberEnd-tables.Offset);tableStream.Write(memberRows);
        tableStream.Write(old,memberEnd,sigEnd-memberEnd);tableStream.Write(sigRows);tableStream.Write(old,sigEnd,typeEnd-sigEnd);tableStream.Write(typeRows);
        tableStream.Write(old,typeEnd,methodSpecEnd-typeEnd);tableStream.Write(methodRows);tableStream.Write(old,methodSpecEnd,tableEnd-methodSpecEnd);
        byte[] tablePayload=tableStream.ToArray();int rowCountAt=24;
        for(int id=0;id<64;id++)
        {
            if((valid&(1UL<<id))==0)continue;
            if(id==10){Require(Read32(tablePayload,rowCountAt)==baseMembers,"Original member row count");Write32(tablePayload,rowCountAt,baseMembers+MemberAdditions.Length);}
            if(id==17){Require(Read32(tablePayload,rowCountAt)==baseLocals,"Original local row count");Write32(tablePayload,rowCountAt,baseLocals+NewLocalBlobs.Length);}
            if(id==27){Require(Read32(tablePayload,rowCountAt)==173,"Original TypeSpec count");Write32(tablePayload,rowCountAt,173+AddedTypeBlobs.Length);}
            if(id==43){Require(Read32(tablePayload,rowCountAt)==185,"Original MethodSpec count");Write32(tablePayload,rowCountAt,187);}
            rowCountAt+=4;
        }
        var payloads=streams.ToDictionary(s=>s.Name,s=>old[s.Offset..checked(s.Offset+s.Size)]);
        payloads["#~"]=tablePayload;payloads["#Blob"]=[..payloads["#Blob"],..blobSuffix];payloads["#Strings"]=[..payloads["#Strings"],..names];
        payloads["#US"]=[..payloads["#US"],..userSuffix];
        using var rootStream=new MemoryStream();rootStream.Write(old,0,ordered[0].Offset);
        var newHeaders=new List<(int At,int Offset,int Size)>();
        for(int i=0;i<ordered.Length;i++)
        {
            var s=ordered[i];while(rootStream.Position%4!=0)rootStream.WriteByte(0);
            newHeaders.Add((s.Header,checked((int)rootStream.Position),payloads[s.Name].Length));rootStream.Write(payloads[s.Name]);
            int oldEnd=s.Offset+s.Size,next=i+1<ordered.Length?ordered[i+1].Offset:oldLength;
            rootStream.Write(old,oldEnd,next-oldEnd); // Preserve original inter-stream/trailing padding too.
        }
        while(rootStream.Position%4!=0)rootStream.WriteByte(0);
        byte[] root=rootStream.ToArray();foreach(var s in newHeaders){Write32(root,s.At,s.Offset);Write32(root,s.At+4,s.Size);}
        int at=Align(before.Length,4),meaningfulEnd=checked(at+root.Length);
        byte[] image=new byte[Align(meaningfulEnd,h.PEHeader!.FileAlignment)];before.CopyTo(image,0);root.CopyTo(image,at);
        ExtendReadonlySection(image,h,meaningfulEnd);var section=h.SectionHeaders.Last();
        Write32(image,h.CorHeaderStartOffset+8,checked(section.VirtualAddress+at-section.PointerToRawData));Write32(image,h.CorHeaderStartOffset+12,root.Length);
        evidence=new { old_root_bytes=oldLength,new_root_bytes=root.Length,appended_memberrefs=MemberAdditions.Length,
            appended_standalone_signatures=NewLocalBlobs.Length,appended_string_payload_bytes=names.Count,appended_blob_payload_bytes=blobSuffix.Count,
            member_row_bytes=memberRows.Length,local_row_bytes=sigRows.Length,appended_typespecs=AddedTypeBlobs.Length,appended_methodspecs=2,appended_user_strings=AddedUserStrings.Length,appended_user_string_payload_bytes=userSuffix.Count,changed_existing_memberrefs=0,changed_existing_local_blobs=0,
            original_root_slot_preserved=true,stream_repacking="Preserved stream payload prefixes/old gaps; four-byte alignment; independent re-open required" };
        return image;
    }
}
