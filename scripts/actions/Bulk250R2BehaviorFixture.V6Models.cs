namespace GodsPVZ.Actions.Verification;

// All dependencies in this file are controlled adapters, not original/native implementations.
// Their side effects are recorded explicitly and remain UNPROVEN in the original engine.
public struct MockVector3 { public float x,y,z; public MockVector3(float x,float y,float z){this.x=x;this.y=y;this.z=z;} }
public class MockProjectile {}
public class MockDamage
{
    public float damagePoint;
    public int damageAttribute;
    public bool anti_air_able,anti_ground_able,anti_submarine_able;
}
public class MockZombie
{
    public string label;
    public bool isDied;
    public List<(MockDamage damage,MockProjectile projectile)> hits=new();
    public Action<MockDamage,MockProjectile> onTakeDamage;
    public void TakeDamage(MockDamage damage,MockProjectile projectile)
    {
        hits.Add((damage,projectile));TraceDisposeCollector.Record("hit:"+label);
        onTakeDamage?.Invoke(damage,projectile);
    }
}
public class MockEnemyPath
{
    public int gridX,gridY;
    public float waitingTime;
    public bool isEnd;
    public MockBoardConfig receivedBoard;
    public static Action<MockEnemyPath> onConstruct;
    public MockEnemyPath(int gridX,int gridY,float waitingTime,MockBoardConfig boardConfig)
    {
        this.gridX=gridX;this.gridY=gridY;this.waitingTime=waitingTime;receivedBoard=boardConfig;
        TraceDisposeCollector.Record("path:new:"+gridX+","+gridY);onConstruct?.Invoke(this);
    }
}
public class MockCardChoose
{
    public string label;
    public int plantID;
    public bool isChosen;
    public int serialNumBeChosen;
    public MockUiImage chosen;
    public MockUiText serialNumBeChosenText;
    public MockSeedChooser seedChooserScreen;
    public MockPlantSave plantSave;
    public MockSkill currentSkill;
    public Action<MockSkill> onSetSkill;
    public Action<MockCardChoose> candidateReset;
    public Action onReset,onChoose;
    public void Choose_Reset()
    {
        if(candidateReset!=null){candidateReset(this);return;}
        TraceDisposeCollector.Record("reset:"+label);isChosen=false;onReset?.Invoke();
    }
    public void Set_Skill(MockSkill value)
    {TraceDisposeCollector.Record("skill.set:"+label+":"+(value?.id.ToString()??"null"));currentSkill=value;onSetSkill?.Invoke(value);}
    public void Choose_Being()
    {
        TraceDisposeCollector.Record("choose:"+label);isChosen=true;onChoose?.Invoke();
    }
}
public class MockPlantDetail
{
    public MockCardChoose selected;
    public Action<MockCardChoose> onAwake;
    public void Awake(MockCardChoose card_Choose)
    {
        TraceDisposeCollector.Record("detail:"+(card_Choose?.label??"null"));selected=card_Choose;onAwake?.Invoke(card_Choose);
    }
}
public class MockSeedChooser
{
    public List<MockCardChoose> card_ChooseList;
    public MockPlantDetail plantDetail;
}

public static partial class CandidateCilTranslator
{
    public static Action<MockZombieManager> CompileKillAll27B(Mono.Cecil.MethodDefinition m)=>
        (Action<MockZombieManager>)EmitAdditional(m,"0600027B",typeof(void),new[]{typeof(MockZombieManager)})
            .CreateDelegate(typeof(Action<MockZombieManager>));
    public static Action<MockEnemy,MockEnemy> CompileEnemyCtor1A1(Mono.Cecil.MethodDefinition m)=>
        (Action<MockEnemy,MockEnemy>)EmitAdditional(m,"060001A1",typeof(void),new[]{typeof(MockEnemy),typeof(MockEnemy)})
            .CreateDelegate(typeof(Action<MockEnemy,MockEnemy>));
    public static Action<MockSeedChooser,List<MockCardChoose>> CompilePrechoose6BB(Mono.Cecil.MethodDefinition m)=>
        (Action<MockSeedChooser,List<MockCardChoose>>)EmitAdditional(m,"060006BB",typeof(void),
            new[]{typeof(MockSeedChooser),typeof(List<MockCardChoose>)})
            .CreateDelegate(typeof(Action<MockSeedChooser,List<MockCardChoose>>));
}
