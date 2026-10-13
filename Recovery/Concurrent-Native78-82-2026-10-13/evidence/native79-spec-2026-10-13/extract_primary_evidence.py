"""Read-only PE / CLR evidence extraction. Does not execute candidate or game CIL."""
from pathlib import Path
import hashlib
import importlib.util
import json
import struct
import sys

sys.dont_write_bytecode = True
ROOT = Path(__file__).resolve().parents[2]
OUT = Path(__file__).resolve().parent
sys.path.insert(0, str(ROOT / "audit/stage9-native4/python-deps"))
import capstone
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
from dncil.clr.token import Token

probe_path = ROOT / "audit/stage9-native3/repository/Tools/Stage9NativeProbe/pc_method_probe.py"
spec = importlib.util.spec_from_file_location("pc_method_probe", probe_path)
probe = importlib.util.module_from_spec(spec)
sys.modules[spec.name] = probe
spec.loader.exec_module(probe)
native_path = ROOT / "audit/stage9-native3/inputs/GameAssembly.dll"
metadata_path = ROOT / "audit/stage9-native3/inputs/global-metadata.dat"
native = native_path.read_bytes()
metadata = metadata_path.read_bytes()
pe = probe.PE(native)
sha = lambda value: hashlib.sha256(value).hexdigest()
assert sha(native) == "9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d"
assert sha(metadata) == "ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9"
data = lambda va, count: native[pe.va_to_offset(va):pe.va_to_offset(va) + count]

result = {
    "scope": "Static original PE, original metadata, and resolver CLR analysis only; no tests, candidate CIL execution, runtime, or acceptance checks.",
    "GameAssembly_path": str(native_path),
    "GameAssembly_sha256": sha(native),
    "global_metadata_path": str(metadata_path),
    "global_metadata_sha256": sha(metadata),
    "pe_helper_sha256": sha(probe_path.read_bytes()),
}

slot = 0x181BA18C8
encoded = struct.unpack("<Q", data(slot, 8))[0]
usage = (encoded >> 29) & 7
index = (encoded & 0x1FFFFFFF) >> 1
magic, version, entries, entry_bytes, strings, string_bytes = struct.unpack_from("<IIIIII", metadata)
assert magic == 0xFAB11BAF and version == 31 and usage == 5 and encoded & 1
assert index < entry_bytes // 8
length, offset = struct.unpack_from("<II", metadata, entries + index * 8)
literal_bytes = metadata[strings + offset:strings + offset + length]
result["backup_literal"] = {
    "runtime_slot": hex(slot), "file_offset": pe.va_to_offset(slot), "encoded_usage": hex(encoded),
    "usage_type": usage, "literal_index": index, "literal_utf8_hex": literal_bytes.hex(),
    "literal": literal_bytes.decode("utf-8"), "utf8_length": length,
    "utf16_code_units": [f"U+{ord(c):04X}" for c in literal_bytes.decode("utf-8")],
}

result["float_constants"] = []
for va in (0x1815A7AC8, 0x1815A7B04, 0x1815A7C0C, 0x1815A7E1C, 0x1815A7E38, 0x181621CD0, 0x181621CC0):
    raw = data(va, 4)
    result["float_constants"].append({"va": hex(va), "file_offset": pe.va_to_offset(va),
         "bytes_le": raw.hex(), "bits": f"0x{struct.unpack('<I', raw)[0]:08X}", "single": struct.unpack("<f", raw)[0]})
raw = data(0x1815A7EE0, 16)
result["spin_fallback_color"] = {
    "va": "0x1815a7ee0", "bytes_le": raw.hex(),
    "channel_bits_rgba": [f"0x{x:08X}" for x in struct.unpack("<IIII", raw)],
    "channels_rgba": list(struct.unpack("<ffff", raw)),
}
icall_name = data(0x181621060, 160).split(b"\0", 1)[0].decode("ascii")
result["quaternion_shared_original_icall"] = {
    "function_pointer_slot": "0x181ceafc8", "icall_name_pointer": "0x181621060", "icall_name": icall_name,
    "private_wrapper_slot_load_instruction": "0x18130c808", "public_getter_slot_load_instruction": "0x18130ce3d",
    "private_wrapper_resolve_instruction": "0x18130c814", "public_getter_resolve_instruction": "0x18130ce4e",
}

source_path = ROOT / "audit/native30-continuation-2026-10-02/native55-native.json"
source = json.loads(source_path.read_text(encoding="utf-8"))
result["native55_source_sha256"] = sha(source_path.read_bytes())
result["native_bodies"] = []
for row in source["methods"]:
    if row["token"] not in ("0x06000238", "0x06000365", "0x0600075F"):
        continue
    start, end = int(row["start"], 16), int(row["end"], 16)
    raw = data(start, end-start)
    pointer = struct.unpack("<Q", data(probe.DEFAULT_TABLE_VA + ((int(row["token"],16) & 0xffffff)-1)*8, 8))[0]
    assert pointer == start and sha(raw) == row["native_sha256"]
    result["native_bodies"].append({"token": row["token"], "name": row["method"], "start": hex(start),
         "end": hex(end), "size": len(raw), "sha256": sha(raw), "matches_original_pointer_and_slice": True})
    (OUT / f'{row["token"]}-native55.asm').write_text(row["disassembly"], encoding="utf-8")

method_map = json.loads((ROOT / "audit/stage9-native3/evidence/native-method-map.json").read_text())
fields = json.loads((ROOT / "audit/stage9-native3/evidence/native-fields.json").read_text())
result["plant_position_fields"] = [f for f in fields if f["type"] == "Plant" and f["offset"] in (48, 52, 56)]
targets = {0x18130C7F0, 0x18130C710, 0x18130CE20}
pdata = pe.section(".pdata")
disassembler = capstone.Cs(capstone.CS_ARCH_X86, capstone.CS_MODE_64)
result["original_quaternion_native_helpers"] = []
for start in sorted(targets):
    ranges = []
    for o in range(pdata.raw_offset, pdata.raw_offset+pdata.raw_size-11, 12):
        lo, hi, unwind = struct.unpack_from("<III", native, o)
        if pe.image_base + lo == start:
            ranges.append((pe.image_base+lo, pe.image_base+hi))
    assert len(ranges) <= 1
    if ranges:
        lo, hi = ranges[0]
        boundary_scope = "exact .pdata runtime function"
    else:
        lo = start
        hi = min(int(m["va"], 16) for m in method_map if int(m["va"], 16) > start)
        boundary_scope = "leaf helper through next mapped native pointer; includes alignment padding"
    raw = data(lo, hi-lo)
    identities = [m for m in method_map if int(m["va"],16) == start]
    instructions = [f"{i.address:016x}: {i.bytes.hex():<32} {i.mnemonic} {i.op_str}" for i in disassembler.disasm(raw, lo)]
    result["original_quaternion_native_helpers"].append({"identities": identities, "start": hex(lo), "end": hex(hi), "boundary_scope": boundary_scope, "sha256": sha(raw), "disassembly": instructions})

resolver = ROOT / "audit/stage9-native3/resolver/UnityEngine.CoreModule.dll"
clr = dnfile.dnPE(str(resolver))
tables = clr.net.mdtables
method_owners = {}
for typedef in tables.TypeDef.rows:
    fullname = str(typedef.TypeNamespace) + "." + str(typedef.TypeName)
    for method_index in typedef.MethodList:
        method_owners[method_index.row_index] = fullname

def token_identity(token):
    table, rid = token >> 24, token & 0xffffff
    if table == 6:
        row = tables.MethodDef.rows[rid-1]
        return {"token": f"0x{token:08X}", "owner": method_owners[rid], "name": str(row.Name), "signature_hex": row.Signature.value.hex()}
    if table == 10:
        row = tables.MemberRef.rows[rid-1]
        owner = row.Class.row
        return {"token": f"0x{token:08X}", "owner": str(getattr(owner,"TypeNamespace", "")) + "." + str(getattr(owner,"TypeName", "")), "name": str(row.Name), "signature_hex": row.Signature.value.hex()}
    return {"token": f"0x{token:08X}"}

result["resolver_quaternion_bodies"] = {"path": str(resolver), "sha256": sha(resolver.read_bytes()), "methods": []}
for rid, row in enumerate(tables.MethodDef.rows, 1):
    quaternion_target = method_owners[rid] == "UnityEngine.Quaternion" and str(row.Name) in ("get_eulerAngles", "Internal_ToEulerRad", "Internal_MakePositive")
    multiply_target = method_owners[rid] == "UnityEngine.Vector3" and rid == 0x177E
    if not (quaternion_target or multiply_target):
        continue
    item = {**token_identity(0x06000000+rid), "rva": row.Rva, "flags": str(row.Flags)}
    if row.Rva:
        body = read_method_body_from_bytes(clr.get_data(row.Rva))
        item["body_sha256"] = sha(body.get_bytes())
        item["raw_body_hex"] = body.get_bytes().hex()
        item["instructions"] = []
        for instruction in body.instructions:
            operand = instruction.operand
            if isinstance(operand, Token):
                operand = token_identity(operand.value)
            elif isinstance(operand, int) and str(instruction.opcode.name).startswith(("br", "beq", "bne", "bge", "bgt", "ble", "blt", "leave")):
                operand = {"target_cil_offset": operand-body.header_size}
            elif operand is not None:
                operand = str(operand)
            item["instructions"].append({"offset": instruction.offset-body.header_size, "opcode": str(instruction.opcode.name), "operand": operand})
    result["resolver_quaternion_bodies"]["methods"].append(item)

(OUT / "PRIMARY-EVIDENCE.json").write_text(json.dumps(result, indent=2, ensure_ascii=False), encoding="utf-8")
print(json.dumps({k:v for k,v in result.items() if k not in ("original_quaternion_native_helpers", "resolver_quaternion_bodies")}, indent=2, ensure_ascii=False))
print("QUATERNION_EVIDENCE " + json.dumps(result["resolver_quaternion_bodies"], indent=2, ensure_ascii=False))
for helper in result["original_quaternion_native_helpers"]:
    print("NATIVE_HELPER " + json.dumps(helper, indent=2))
