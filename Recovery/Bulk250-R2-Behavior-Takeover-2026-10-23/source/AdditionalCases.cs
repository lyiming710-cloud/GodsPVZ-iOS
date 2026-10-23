using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace GodsPVZ.Actions.Verification;

public partial class Program
{
    private static void RunAdditionalCases(ModuleDefinition module,List<Dictionary<string,object>> cases,
        List<string> failures,Dictionary<string,bool> methodStatus)
    {
        var flat=CandidateCilTranslator.CompileGetMapGrids287((MethodDefinition)module.LookupToken(0x06000287));
        var find=CandidateCilTranslator.CompileFindKey60A((MethodDefinition)module.LookupToken(0x0600060A));
        var sort=CandidateCilTranslator.CompileCompletenessTest4BF((MethodDefinition)module.LookupToken(0x060004BF));
        void Case(string token,string id,object input,Func<bool> check)
        {
            TraceDisposeCollector.Clear();bool passed=false;Exception unexpected=null;
            try { passed=check(); } catch(Exception e) { unexpected=e; }
            finally { TraceDisposeCollector.BeforeMove=null; }
            cases.Add(new(){["target"]=token,["case_id"]=id,["input"]=input,["passed"]=passed,
                ["observed_trace"]=TraceDisposeCollector.Log.ToArray(),["unexpected_exception"]=unexpected?.ToString()});
            methodStatus[token]=methodStatus.GetValueOrDefault(token)&&passed;
            if(!passed)failures.Add(token+" "+id+" "+unexpected?.GetType().FullName);
        }
        bool Throws<T>(Action action,params string[] trace) where T:Exception
        {
            try { action();return false; }
            catch(Exception e) { return e.GetType()==typeof(T)&&TraceDisposeCollector.Log.SequenceEqual(trace); }
        }
        const string map="0x06000287",entry="0x0600060A",save="0x060004BF";
        Case(map,"E1.extra.EmptyMap",new{rows=0},()=>flat(new MockMap()).Count==0&&TraceDisposeCollector.Log.SequenceEqual(new[]{"MockRow_Disposed"}));
        Case(map,"E1.extra.DuplicateAndNullElements",new{items=new[]{"g","null","g"}},()=>
        {
            var g=new MockGrid(7,2,3);var m=new MockMap();m.rows.Add(new(){grids=new(){g,null,g}});
            var result=flat(m);return result.Count==3&&ReferenceEquals(result[0],g)&&result[1]==null&&ReferenceEquals(result[2],g)&&
                !ReferenceEquals(result,m.rows[0].grids)&&TraceDisposeCollector.Log.SequenceEqual(new[]{"MockGrid_Disposed","MockRow_Disposed"});
        });
        Case(map,"E1.extra.NullRows",new{rows="null"},()=>Throws<NullReferenceException>(()=>flat(new MockMap(){rows=null})));
        Case(map,"E1.extra.NullRow",new{rows=new[]{"null"}},()=>
        {
            var m=new MockMap();m.rows.Add(null);return Throws<NullReferenceException>(()=>flat(m),"MockRow_Disposed");
        });
        Case(map,"E1.extra.InnerCollectionInvalidated",new{mutation="add during inner MoveNext"},()=>
        {
            var row=new MockRow(){grids=new(){new(1,0,0)}};var m=new MockMap();m.rows.Add(row);bool once=false;
            TraceDisposeCollector.BeforeMove=name=>{if(name=="MockGrid"&&!once){once=true;row.grids.Add(new(2,0,0));}};
            return Throws<InvalidOperationException>(()=>flat(m),"MockGrid_Disposed","MockRow_Disposed");
        });

        // Read each literal from the authenticated candidate, not from the mutant under test.
        var literals=((MethodDefinition)module.LookupToken(0x0600060A)).Body.Instructions
            .Where(i=>i.OpCode==OpCodes.Ldstr).Select(i=>(string)i.Operand).ToArray();
        if(literals.Length!=6)throw new NotSupportedException("FindKey requires six pinned suffix literals");
        for(int kindIndex=0;kindIndex<3;kindIndex++)
        for(int bits=0;bits<4;bits++)
        {
            int kind=new[]{24,25,36}[kindIndex];bool must=(bits&1)!=0,selected=(bits&2)!=0;
            string suffix=literals[kindIndex*2+((must||selected)?1:0)];
            Case(entry,$"E2.extra.Flags.{kind}.{bits}",new{kind,must,selected,suffix},()=>
            {
                var value=new MockBoardEntry(kind,"key",must,selected);var values=new List<MockBoardEntry>(){value};
                return ReferenceEquals(find(values,"key"+suffix),value)&&TraceDisposeCollector.Log.SequenceEqual(new[]{"MockBoardEntry_Disposed"});
            });
        }
        Case(entry,"E2.extra.Empty",new{count=0},()=>find(new(),"missing")==null&&TraceDisposeCollector.Log.SequenceEqual(new[]{"MockBoardEntry_Disposed"}));
        Case(entry,"E2.extra.FirstDuplicateWins",new{keys=new[]{"dup","dup"}},()=>
        {
            var a=new MockBoardEntry(0,"dup");var b=new MockBoardEntry(0,"dup");
            return ReferenceEquals(find(new(){a,b},"dup"),a)&&TraceDisposeCollector.Log.SequenceEqual(new[]{"MockBoardEntry_Disposed"});
        });
        Case(entry,"E2.extra.NullList",new{list="null"},()=>Throws<NullReferenceException>(()=>find(null,"key")));
        Case(entry,"E2.extra.NullItem",new{items=new[]{"null"}},()=>Throws<NullReferenceException>(()=>find(new(){null},"key"),"MockBoardEntry_Disposed"));
        Case(entry,"E2.extra.NullPlainKey",new{key="null",target="null"},()=>
        {
            var a=new MockBoardEntry(0,null);return ReferenceEquals(find(new(){a},null),a);
        });
        Case(entry,"E2.extra.CaseSensitive",new{key="Key",target="key"},()=>find(new(){new(0,"Key")},"key")==null);
        Case(entry,"E2.extra.NullKeyConcat",new{key="null",kind=24},()=>
        {
            var a=new MockBoardEntry(24,null);return ReferenceEquals(find(new(){a},literals[0]),a);
        });

        int[][] explicitIds=[[],[7],[3,2,2,1],[int.MaxValue,0,int.MinValue,-1],[4,3,2,1],[1,2,3,4],[2,2,2,2]];
        var rng=new Random(20261023);
        var arrays=explicitIds.Concat(Enumerable.Range(0,32).Select(_=>Enumerable.Range(0,rng.Next(0,16)).Select(_=>rng.Next(-5,6)).ToArray())).ToArray();
        for(int index=0;index<arrays.Length;index++)
        {
            int[] ids=arrays[index];
            Case(save,$"E3.extra.StableSort.{index}",new{ids},()=>
            {
                CandidateCilTranslator.gLawnApp=new();var s=new MockSave();
                var values=ids.Select((id,i)=>new MockSuppliesItem(id,"identity-"+i)).ToList();
                s.supplies=values.ToList();var expected=values.OrderBy(x=>x.ID).ToArray();
                sort(s);return s.supplies.SequenceEqual(expected)&&TraceDisposeCollector.Log.SequenceEqual(new[]{"MockPlantSave_Disposed"});
            });
        }
        Case(save,"E3.extra.PlantCallbackOrder",new{plants=new[]{11,12,13}},()=>
        {
            CandidateCilTranslator.gLawnApp=new();var s=new MockSave(){plantSaves=new(){new(11),new(12),new(13)}};
            sort(s);return CandidateCilTranslator.gLawnApp.savesManager.reloadedList.SequenceEqual(s.plantSaves)&&TraceDisposeCollector.Log.SequenceEqual(new[]{"MockPlantSave_Disposed"});
        });
        Case(save,"E3.extra.NullPlantList",new{plantSaves="null"},()=>
        {
            CandidateCilTranslator.gLawnApp=new();return Throws<NullReferenceException>(()=>sort(new(){plantSaves=null}));
        });
        Case(save,"E3.extra.NullSingleton",new{singleton="null",plants=1},()=>
        {
            CandidateCilTranslator.gLawnApp=null;return Throws<NullReferenceException>(()=>sort(new(){plantSaves=new(){new(1)}}),"MockPlantSave_Disposed");
        });
        Case(save,"E3.extra.EmptyPlantsSkipNullSingleton",new{singleton="null",plants=0},()=>
        {
            CandidateCilTranslator.gLawnApp=null;var s=new MockSave(){supplies=new(){new(2),new(1)}};sort(s);
            return s.supplies.Select(x=>x.ID).SequenceEqual(new[]{1,2});
        });
        Case(save,"E3.extra.PlantCollectionInvalidation",new{mutation="add plant in reload callback"},()=>
        {
            CandidateCilTranslator.gLawnApp=new();var s=new MockSave(){plantSaves=new(){new(1)}};
            CandidateCilTranslator.gLawnApp.savesManager.onReload=_=>s.plantSaves.Add(new(2));
            return Throws<InvalidOperationException>(()=>sort(s),"MockPlantSave_Disposed");
        });
        Case(save,"E3.extra.NullSuppliesAfterCleanup",new{supplies="null"},()=>
        {
            CandidateCilTranslator.gLawnApp=new();return Throws<NullReferenceException>(()=>sort(new(){supplies=null}),"MockPlantSave_Disposed");
        });
        CandidateCilTranslator.gLawnApp=new();
    }

    private static object CilIdentity(MethodDefinition method)
    {
        var body=method.Body;var ins=body.Instructions;
        object Operand(object o)=>o switch
        {
            null=>null, Instruction i=>new{instruction_index=ins.IndexOf(i)},
            Instruction[] a=>new{instruction_indices=a.Select(i=>ins.IndexOf(i)).ToArray()},
            VariableDefinition v=>new{variable_index=v.Index,type=v.VariableType.FullName},
            ParameterDefinition p=>new{parameter_index=p.Index,type=p.ParameterType.FullName},
            MemberReference m=>new{token=$"0x{m.MetadataToken.ToInt32():X8}",member=m.FullName},_=>o
        };
        var canonical=new{method=method.Name,signature=method.FullName,locals=body.Variables.Select(v=>v.VariableType.FullName).ToArray(),
            instructions=ins.Select((i,n)=>new{index=n,opcode=i.OpCode.Name,operand=Operand(i.Operand)}).ToArray(),
            handlers=body.ExceptionHandlers.Select(e=>new{kind=e.HandlerType.ToString(),tryStart=ins.IndexOf(e.TryStart),tryEnd=e.TryEnd==null?ins.Count:ins.IndexOf(e.TryEnd),
                handlerStart=ins.IndexOf(e.HandlerStart),handlerEnd=e.HandlerEnd==null?ins.Count:ins.IndexOf(e.HandlerEnd)}).ToArray()};
        return new{canonical_cil=canonical,canonical_in_memory_cil_sha256=Convert.ToHexString(SHA256.HashData(JsonSerializer.SerializeToUtf8Bytes(canonical))).ToLowerInvariant(),
            note="Execution-input representation hash; not a serialized mutant DLL hash"};
    }
    private static object RuntimeIdentity()
    {
        string HashFile(string p)=>Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(p))).ToLowerInvariant();
        return new{framework=System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
            os=System.Runtime.InteropServices.RuntimeInformation.OSDescription,architecture=System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString(),
            fixture_sha256=HashFile(typeof(Program).Assembly.Location),cecil_sha256=HashFile(typeof(AssemblyDefinition).Assembly.Location),
            scope="Controlled dependency CLR fixture; no native oracle, Unity, Xcode or IPA equivalence claim"};
    }
}
