namespace GodsPVZ.Actions.Verification;
// Controlled engine/service dependencies; not original/native implementations.
public struct MockTeam {public List<int> ID,skill;}
public class MockSkill {public int id;}
public struct MockColor
{
    public float r,g,b,a;
    public MockColor(float r,float g,float b,float a){this.r=r;this.g=g;this.b=b;this.a=a;}
}
public class MockUiGameObject
{
    public string label;
    public bool active=true;
    public Action<bool> onSetActive;
    public void SetActive(bool value){TraceDisposeCollector.Record("active:"+label+":"+value);active=value;onSetActive?.Invoke(value);}
}
public class MockUiComponent
{
    public string label;
    public MockUiTransform transform;
    public MockUiGameObject gameObject;
    public MockUiImage componentImage;
    public Func<MockUiImage> onGetImage;
    public MockUiImage GetImage(){TraceDisposeCollector.Record("image:"+label);return onGetImage==null?componentImage:onGetImage();}
    public MockUiTransform get_transform(){TraceDisposeCollector.Record("transform:"+label);return transform;}
    public MockUiGameObject get_gameObject(){TraceDisposeCollector.Record("object:"+label);return gameObject;}
}
public class MockUiTransform:MockUiComponent
{
    public List<MockUiTransform> children=new();
    public Func<int,MockUiTransform> onChild;
    public MockUiTransform GetChild(int index){TraceDisposeCollector.Record("child:"+label+":"+index);return onChild==null?children[index]:onChild(index);}
}
public class MockUiGraphic:MockUiComponent
{
    public MockColor color;
    public Func<MockColor> onGetColor;
    public Action<MockColor> onSetColor;
    public MockColor get_color(){TraceDisposeCollector.Record("color.get:"+label);return onGetColor==null?color:onGetColor();}
    public void set_color(MockColor value){TraceDisposeCollector.Record("color.set:"+label+":"+BitConverter.SingleToInt32Bits(value.a));color=value;onSetColor?.Invoke(value);}
}
public class MockUiImage:MockUiGraphic {}
public class MockTmpText:MockUiComponent
{
    public string text;
    public Action<string> onSetText;
    public void set_text(string value){TraceDisposeCollector.Record("text:"+label+":"+value);text=value;onSetText?.Invoke(value);}
}
public class MockUiText:MockTmpText {}
public class MockPlant:MockUnityObject
{
    public int plantType;
    public Func<int,bool> bearCallback;
    public bool CanBear(int slot){TraceDisposeCollector.Record("plant.bear:"+slot);return bearCallback?.Invoke(slot)??true;}
}
public static partial class CandidateCilTranslator
{
    public static Action<MockSeedChooser,MockTeam> CompileLoadTeam6B6(Mono.Cecil.MethodDefinition m)=>
        (Action<MockSeedChooser,MockTeam>)EmitAdditional(m,"060006B6",typeof(void),new[]{typeof(MockSeedChooser),typeof(MockTeam)})
            .CreateDelegate(typeof(Action<MockSeedChooser,MockTeam>));
    public static Action<MockCardChoose> CompileReset2F5(Mono.Cecil.MethodDefinition m)=>
        (Action<MockCardChoose>)EmitAdditional(m,"060002F5",typeof(void),new[]{typeof(MockCardChoose)})
            .CreateDelegate(typeof(Action<MockCardChoose>));
    public static Func<MockGrid,MockDevice,bool> CompileCanPlacing297(Mono.Cecil.MethodDefinition m)=>
        (Func<MockGrid,MockDevice,bool>)EmitAdditional(m,"06000297",typeof(bool),new[]{typeof(MockGrid),typeof(MockDevice)})
            .CreateDelegate(typeof(Func<MockGrid,MockDevice,bool>));
}
