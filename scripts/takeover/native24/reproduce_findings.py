"""Execute historical source, without changing it or treating it as a gate."""
import hashlib
import json
import pathlib
import random
import sys

ROOT = pathlib.Path(__file__).resolve().parents[3]
sys.dont_write_bytecode = True
sys.path.insert(0, str(ROOT / "scripts/codespaces"))
import native23_prov as P
import native23_afam3 as C
import native23_behave as B
import native23_tiers as T

TYPES = {"Owner": {"baseType": "System.Object"}, "GameObject": {"baseType": "System.Object"},
         "System.Object": {}, "System.Int32": {"value": True}, "System.Boolean": {"value": True},
         "System.String": {}, "System.Void": {"value": True}}


def body(name, ops, locals=(), args=()):
    return {"token": "0x06000001", "name": name, "owner": "Owner", "hasThis": False,
            "args": list(args), "locals": list(locals), "ret": "System.Void", "handlers": [],
            "maxStack": 8, "instructions": [{"offset": 2*n, "opcode": op, "operand": a}
                                           for n, (op, a) in enumerate(ops)]}


field = {"owner": "Owner", "type": "GameObject", "identity": "GameObject Owner::f"}


def pollution_cases():
    return [
        body("invalid local receiver", [("ldc.i4.1", None), ("stloc", {"index": 0}),
             ("ldloc", {"index": 0}), ("ldfld", field), ("ldc.i4.0", None),
             ("ceq", None), ("pop", None), ("ret", None)], locals=["Owner"]),
        body("invalid starg", [("ldc.i4.1", None), ("starg", {"index": 0}),
             ("ldarg", {"index": 0}), ("ldc.i4.0", None), ("ceq", None),
             ("pop", None), ("ret", None)], args=["GameObject"]),
        body("invalid call argument", [("ldc.i4.1", None), ("call", {"owner": "Owner",
             "hasThis": False, "args": ["System.String"], "ret": "Owner", "name": "GetOwner",
             "identity": "Owner Owner::GetOwner(System.String)"}), ("ldfld", field),
             ("ldc.i4.0", None), ("ceq", None), ("pop", None), ("ret", None)]),
        body("incompatible merge", [("ldarg", {"index": 0}), ("brtrue", {"target": 8}),
             ("ldarg", {"index": 1}), ("br", {"target": 10}), ("ldc.i4.1", None),
             ("ldfld", field), ("ldc.i4.0", None), ("ceq", None), ("pop", None),
             ("ret", None)], args=["System.Boolean", "Owner"]),
    ]


def main():
    out = pathlib.Path(sys.argv[1])
    result = {"sources": {m.__name__: {"path": m.__file__, "sha256": hashlib.sha256(
                  pathlib.Path(m.__file__).read_bytes()).hexdigest()} for m in [P, C, B, T]}}
    result["pollution"] = [{"name": d["name"], "errors": P.verify_prov(d, TYPES),
                            "solver": C.solve(d, TYPES)} for d in pollution_cases()]
    assert all(any(r["verdict"] == "ACCEPT" for r in x["solver"]["recs"])
               for x in result["pollution"]), "historical counterexamples did not reproduce"
    d = body("field storage", [("ldarg", {"index": 0}), ("ldc.i4", 7),
              ("stfld", {"owner": "Owner", "type": "System.Int32", "identity": "System.Int32 Owner::n"}),
              ("ret", None)], args=["Owner"])
    machine = B.Machine(d, TYPES, B.WORLDS[0], set(), random.Random(0)).setup()
    machine.heap.append({"System.Int32 Owner::n": ("I4", 3, "init")})
    machine.args[0] = ("REF", 0, "test")
    machine.run()
    result["field_storage"] = {"expected": 7, "actual": machine.heap[0]["System.Int32 Owner::n"][1],
                               "status": machine.status}
    assert result["field_storage"]["actual"] == 3
    result["incorrect_pointer_patterns"] = {s: bool(T.RE_TEST.match(s) or T.RE_CMPQ.match(s))
                              for s in ["test %al,%al", "test %eax,%eax", "cmpq $0x20,(%rax)"]}
    assert all(result["incorrect_pointer_patterns"].values())
    out.write_text(json.dumps(result, indent=2)+"\n")
    print("REPRODUCED: 4 pollution accepts, failed field storage, 3 false pointer patterns")


if __name__ == "__main__":
    main()
