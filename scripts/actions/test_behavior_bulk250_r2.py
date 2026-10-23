#!/usr/bin/env python3
"""Bounded build-once runner. A successful process is only a scoped CLR fixture result."""
from pathlib import Path
import argparse,hashlib,json,os,shutil,subprocess,sys,time

PIN="4732b0c1ee64be4224649ee43c88ddd805f5e1ea461e43ecf52febd7a756f903"
CECIL="831dca77470d85cb6ffbea3072daa7a3df5b7c9fcfd9c3f43674a9be99d4bfcf"
TARGETS={"0x0600001C","0x06000183","0x06000287","0x0600060A","0x060004BF","0x06000187","0x0600026F","0x060001A8",
         "0x0600027B","0x060001A1","0x060006BB"}
HELPERS={"0x06000158","0x06000303"}

def sha(path):return hashlib.sha256(Path(path).read_bytes()).hexdigest()
def save(path,value):
    Path(path).write_text(json.dumps(value,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
def validate_report(report):
    s=report["summary"];rows=report["fifty_methods_behavior"]
    assert report["candidate_sha256"]==PIN and report["run_mode"]=="normal"
    assert len(rows)==50 and s["total_50_methods"]==50
    passed={k for k,v in rows.items() if v["status"]=="PASS"}
    pending={k for k,v in rows.items() if v["status"]=="NOT_YET_EXECUTED"}
    unsupported={k for k,v in rows.items() if v["status"]=="UNSUPPORTED_ENGINE_CONTEXT"}
    assert passed==TARGETS and len(pending)==17 and len(unsupported)==22
    assert passed|pending|unsupported==set(rows) and not (passed&pending or passed&unsupported or pending&unsupported)
    assert s["methods_genuinely_passed"]==11 and s["methods_failed"]==0 and s["failed_assertions"]==[]
    assert s["methods_not_yet_executed"]==s["pending_after"]==17 and s["v6_pending_before"]==20
    assert s["methods_unsupported_engine_context"]==22
    cases=report["cases_evaluated"];mutants=report["mutants_evaluated"]
    assert len(cases)==s["total_cases_executed"]==214 and all(c["passed"] is True for c in cases)
    assert len({(c["target"],c["case_id"]) for c in cases})==len(cases)
    assert len(mutants)==s["total_mutants_tested"]==s["total_mutants_detected"]==27
    assert all(m["detected"] is True and m.get("unexpected_exception") is None for m in mutants)
    assert all(rows[t]["cases_executed"]==sum(c["target"]==t for c in cases) for t in TARGETS)
    assert all(rows[t]["mutants_detected"]==sum(m["target"]==t for m in mutants) for t in TARGETS)
    assert all(report["getter_executions"][t]["status"]=="PASS" for t in HELPERS)
    assert len(report["execution_input_identities"])==13 and len(report["mutant_execution_input_identities"])==27
    assert report["runtime_identity"]["cecil_sha256"]==CECIL
    scope=report["evidence_scope"]
    assert scope["native_original_executed"] is False and scope["full_assembly_qualification"] is False
    assert scope["native_semantic_equivalence"]=="UNPROVEN" and scope["promotion_status"]=="NOT_FORMALLY_PROMOTED"
    assert scope["ipa_produced"] is False and s["overall_status"]=="PARTIAL_CHAIN_VERIFIED_STAGE2"

def main():
    if not __debug__:raise RuntimeError("Python optimization disables gate assertions; refusing to run")
    p=argparse.ArgumentParser()
    p.add_argument("--candidate-dll",required=True);p.add_argument("--output-json",required=True)
    p.add_argument("--fixture-project",required=True);p.add_argument("--dotnet",default="dotnet")
    a=p.parse_args();candidate=Path(a.candidate_dll).resolve();out=Path(a.output_json).resolve()
    project=Path(a.fixture_project).resolve()
    if sha(candidate)!=PIN:raise ValueError("Input candidate identity mismatch")
    logs=out.parent/(out.stem+"-logs")
    if out.exists() or logs.exists():raise FileExistsError("Never overwrite a prior gate run")
    logs.mkdir(parents=True);runs=[]
    env=os.environ.copy();env.update(DOTNET_NOLOGO="1",DOTNET_CLI_TELEMETRY_OPTOUT="1",DOTNET_DbgEnableMiniDump="0")
    def run(name,cmd,timeout):
        start=time.monotonic()
        try:
            proc=subprocess.run(cmd,capture_output=True,timeout=timeout,env=env)
            rc=proc.returncode;stdout=proc.stdout;stderr=proc.stderr;state="PROCESS_COMPLETED"
        except subprocess.TimeoutExpired as e:
            rc=None;stdout=e.stdout or b"";stderr=e.stderr or b"";state="TIMED_OUT_NOT_A_PASS"
        (logs/(name+".stdout.log")).write_bytes(stdout);(logs/(name+".stderr.log")).write_bytes(stderr)
        row=dict(name=name,argv=cmd,cwd=os.getcwd(),timeout_seconds=timeout,state=state,exit_code=rc,
            elapsed_seconds=time.monotonic()-start,stdout_bytes=len(stdout),stderr_bytes=len(stderr),
            stdout_sha256=hashlib.sha256(stdout).hexdigest(),stderr_sha256=hashlib.sha256(stderr).hexdigest())
        save(logs/(name+".json"),row);runs.append(row)
        return rc,stdout.decode("utf-8","replace"),stderr.decode("utf-8","replace")
    try:
        rc,so,se=run("00-build",[a.dotnet,"build",str(project),"-c","Release"],180)
        if rc!=0:raise RuntimeError("Fixture build failed: "+so[-3000:]+se[-1000:])
        binary=project.parent/"bin/Release/net10.0"/(project.stem+".dll")
        sources=[project]+sorted(project.parent.glob("*.cs"))
        save(logs/"execution-sources.json",{
            "candidate":{"bytes":candidate.stat().st_size,"sha256":sha(candidate)},
            "sources":[{"path":str(f),"sha256":sha(f)} for f in sources],
            "fixture":{"path":str(binary),"sha256":sha(binary)},
            "runner_sha256":sha(__file__),"dotnet":a.dotnet})
        controls=[]
        modes=[
            ("neg_assertion_failure",10,None),("neg_escaped_mutant",12,None),
            ("neg_unsupported_opcode",None,"NotSupportedException"),
            ("neg_unexpected_exception",None,"DivideByZeroException"),
            ("neg_wrong_sha",3,"Candidate SHA-256 mismatch"),
            ("neg_additional_opcode",None,"unmodeled opcode"),
            ("neg_additional_member",None,"call binding"),
            ("neg_additional_local",None,"unmapped candidate type System.Object"),
            ("neg_additional_eh",None,"unmodeled EH layout"),
            ("neg_v6_crossing_eh",None,"mismatched endfinally"),
            ("neg_v6_unknown_field",None,"field binding"),
            ("neg_v6_ctor_signature",None,"candidate signature mismatch"),
            ("neg_v6_branch_target",None,"branch target outside method"),
        ]
        for n,(mode,expected,needle) in enumerate(modes,1):
            rc,so,se=run(f"{n:02d}-{mode}",[a.dotnet,str(binary),str(candidate),str(logs/(mode+".report.json")),"--mode="+mode],30)
            passed=rc is not None and rc!=0 and (expected is None or rc==expected) and (needle is None or needle in se)
            controls.append(dict(mode=mode,expected_exit=expected,stderr_required=needle,observed_exit=rc,passed=passed))
            print(mode+": "+("rejected as required" if passed else "FAILED CONTROL"),flush=True)
        # Challenge the normal SHA gate with a physically different input, without changing the frozen input.
        wrong=logs/"wrong-candidate.dll";shutil.copyfile(candidate,wrong)
        with wrong.open("r+b") as stream:
            stream.seek(-1,2);last=stream.read(1);stream.seek(-1,2);stream.write(bytes([last[0]^1]))
        rc,so,se=run("14-physical-wrong-candidate",[a.dotnet,str(binary),str(wrong),str(logs/"physical-wrong.report.json"),"--mode=normal"],30)
        controls.append(dict(mode="physical_wrong_candidate",sha256=sha(wrong),mutation="xor last byte with 1",
            observed_exit=rc,passed=rc==3 and "Candidate SHA-256 mismatch" in se))
        save(logs/"gate-negative-controls.json",controls)
        if not all(c["passed"] for c in controls):raise RuntimeError("One or more gate controls failed")
        raw=logs/"normal.fixture.json"
        rc,so,se=run("15-normal",[a.dotnet,str(binary),str(candidate),str(raw),"--mode=normal"],30)
        if rc!=0:raise RuntimeError("Normal fixture failed: "+so[-2000:]+se[-2000:])
        report=json.loads(raw.read_text(encoding="utf-8"));validate_report(report)
        if report["runtime_identity"]["fixture_sha256"]!=sha(binary) or sha(candidate)!=PIN:
            raise RuntimeError("Execution binary or immutable candidate identity changed")
        report["gate_negative_controls"]=controls
        report["runner_source_sha256"]=sha(__file__)
        report["runner_result"]="PASS_CONTROLLED_CLR_SCOPE_ONLY"
        report["normal_fixture_raw_sha256"]=sha(raw)
        report["scope_note"]="No native equivalence, Unity, full-assembly, Xcode, iOS or IPA qualification."
        save(out,report);save(logs/"gate-state.json",dict(state="COMPLETE_SCOPED_EXECUTION",output_sha256=sha(out)))
        print(json.dumps(report["summary"],indent=2))
    except Exception as e:
        save(logs/"gate-state.json",dict(state="FAILED_NOT_A_PASS",exception=repr(e)))
        raise
    finally:
        save(logs/"processes.json",runs)
        manifest=[dict(path=str(f.relative_to(logs)),bytes=f.stat().st_size,sha256=sha(f))
            for f in sorted(logs.rglob("*")) if f.is_file() and f.suffix!=".dll" and f.name!="LOGS-MANIFEST.json"]
        save(logs/"LOGS-MANIFEST.json",manifest)
if __name__=="__main__":main()
