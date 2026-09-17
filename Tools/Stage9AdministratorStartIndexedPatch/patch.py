#!/usr/bin/env python3
from __future__ import annotations

import hashlib
import sys
from pathlib import Path

EXPECTED_INPUT_SHA = "f3d980abb6a7678bf3ac04587ec857f88d488a9c136d991323562beda943f3a2"
EXPECTED_METADATA_SHA = "733f78b3cafe85511475c3037106bad7ee36224aa30c071d10f07fecfeff273e"
EXPECTED_LOOP_SHA = "30f054420e10321e1464186d5fb6ad7a28db99b20e00c8fe19c98d7d3d913ef3"
EXPECTED_EH_SHA = "4bc9e37f55011a1fa3c9ab519a893234a19e7f329bc2bc7484907f2f42003497"

METHOD_FILE = 0x2B4
CODE_FILE = 0x2C0
CODE_SIZE = 541
NEXT_METHOD_FILE = 0x53C
METADATA_FILE = 0xB9BC4
METADATA_SIZE = 439400
LOOP_IL_START = 0x2F
LOOP_IL_END = 0x71
EH_FILE = 0x4E0
OLD_LOCAL_SIG = 0x11000101
NEW_LOCAL_SIG = 0x11000114  # existing baseline StandAloneSig blob: 07 01 08 => one Int32 local

SYSTEMS_FIELD = 0x04000003
SET_ACTIVE = 0x0A000016
LIST_GAMEOBJECT_COUNT = 0x0A0001EB
LIST_GAMEOBJECT_ITEM = 0x0A0001EC
BAD_ENUMERATOR_TOKENS = (0x0A0000FB, 0x0A0000FC, 0x0A0000FD, 0x0A000126)


def sha(b: bytes) -> str:
    return hashlib.sha256(b).hexdigest()


def token(n: int) -> bytes:
    return n.to_bytes(4, "little")


def build_loop_region() -> bytes:
    # Preserve the original IL_002F..IL_0070 footprint exactly (66 bytes).
    # The protected region is widened to include initialization and the loop.
    # Local V_0 is the existing baseline Int32 signature 0x11000114.
    region = bytearray(LOOP_IL_END - LOOP_IL_START)
    base = LOOP_IL_START

    def put(il: int, payload: bytes) -> None:
        rel = il - base
        if rel < 0 or rel + len(payload) > len(region):
            raise RuntimeError(f"patch range overflow IL_{il:04X}")
        region[rel : rel + len(payload)] = payload

    # int i = 0; goto condition;
    put(0x2F, bytes([0x16, 0x0A, 0x2B, 0x21]))  # ldc.i4.0; stloc.0; br.s IL_0054

    # body: this.systems[i].SetActive(false); i++;
    body = (
        bytes([0x02, 0x7B]) + token(SYSTEMS_FIELD) +
        bytes([0x06, 0x6F]) + token(LIST_GAMEOBJECT_ITEM) +
        bytes([0x16, 0x28]) + token(SET_ACTIVE) +
        bytes([0x06, 0x17, 0x58, 0x0A])
    )
    if len(body) != 22:
        raise RuntimeError("indexed body length drift")
    put(0x3E, body)

    # condition: if (i < this.systems.Count) goto body; leave IL_0071;
    condition = (
        bytes([0x06, 0x02, 0x7B]) + token(SYSTEMS_FIELD) +
        bytes([0x6F]) + token(LIST_GAMEOBJECT_COUNT) +
        bytes([0x32, 0xDC, 0xDE, 0x0D])  # blt.s IL_003E; leave.s IL_0071
    )
    if len(condition) != 16:
        raise RuntimeError("indexed condition length drift")
    put(0x54, condition)

    # Keep the existing finally clause layout but make it empty. This avoids
    # moving any later IL while removing all dependency on List<T>.Enumerator.
    put(0x67, bytes([0x00] * 9 + [0xDC]))  # nop*9; endfinally

    if len(region) != 66:
        raise RuntimeError("loop region length drift")
    return bytes(region)


def main() -> int:
    if len(sys.argv) != 3:
        print("usage: patch.py <f3d980...-candidate.dll> <output.dll>", file=sys.stderr)
        return 2

    src = Path(sys.argv[1]).resolve()
    dst = Path(sys.argv[2]).resolve()
    original = src.read_bytes()
    if sha(original) != EXPECTED_INPUT_SHA:
        raise RuntimeError(f"input SHA drift actual={sha(original)} expected={EXPECTED_INPUT_SHA}")
    if len(original) <= NEXT_METHOD_FILE:
        raise RuntimeError("input unexpectedly short")

    # Lock the exact preimage before touching bytes.
    flags = int.from_bytes(original[METHOD_FILE : METHOD_FILE + 2], "little")
    header_size = ((flags >> 12) & 0xF) * 4
    code_size = int.from_bytes(original[METHOD_FILE + 4 : METHOD_FILE + 8], "little")
    local_sig = int.from_bytes(original[METHOD_FILE + 8 : METHOD_FILE + 12], "little")
    if (flags & 3) != 3 or header_size != 12 or code_size != CODE_SIZE or local_sig != OLD_LOCAL_SIG:
        raise RuntimeError(
            f"Start header drift flags=0x{flags:X} header={header_size} code={code_size} localsig=0x{local_sig:08X}"
        )

    old_loop = original[CODE_FILE + LOOP_IL_START : CODE_FILE + LOOP_IL_END]
    old_eh = original[EH_FILE : EH_FILE + 16]
    if sha(old_loop) != EXPECTED_LOOP_SHA:
        raise RuntimeError(f"foreach preimage drift sha={sha(old_loop)}")
    if sha(old_eh) != EXPECTED_EH_SHA:
        raise RuntimeError(f"EH preimage drift sha={sha(old_eh)}")
    if sha(original[METADATA_FILE : METADATA_FILE + METADATA_SIZE]) != EXPECTED_METADATA_SHA:
        raise RuntimeError("metadata preimage drift")

    out = bytearray(original)
    out[METHOD_FILE + 8 : METHOD_FILE + 12] = token(NEW_LOCAL_SIG)
    out[CODE_FILE + LOOP_IL_START : CODE_FILE + LOOP_IL_END] = build_loop_region()

    # Small EH clause: finally, try IL_002F..IL_0067, handler IL_0067..IL_0071.
    expected_eh_header = bytes.fromhex("011000000200")
    if bytes(out[EH_FILE : EH_FILE + 6]) != expected_eh_header:
        raise RuntimeError("EH header drift")
    out[EH_FILE + 6 : EH_FILE + 8] = (0x2F).to_bytes(2, "little")
    out[EH_FILE + 8] = 0x38

    changed = [i for i, (a, b) in enumerate(zip(original, out)) if a != b]
    if not changed:
        raise RuntimeError("no bytes changed")
    if any(i < METHOD_FILE or i >= NEXT_METHOD_FILE for i in changed):
        raise RuntimeError("diff escaped Administrator.Start method allocation")

    metadata_sha = sha(bytes(out[METADATA_FILE : METADATA_FILE + METADATA_SIZE]))
    if metadata_sha != EXPECTED_METADATA_SHA:
        raise RuntimeError(f"metadata changed sha={metadata_sha}")
    if int.from_bytes(out[METHOD_FILE + 8 : METHOD_FILE + 12], "little") != NEW_LOCAL_SIG:
        raise RuntimeError("local signature patch failed")
    if int.from_bytes(out[METHOD_FILE + 4 : METHOD_FILE + 8], "little") != CODE_SIZE:
        raise RuntimeError("code size changed")

    code = bytes(out[CODE_FILE : CODE_FILE + CODE_SIZE])
    for bad in BAD_ENUMERATOR_TOKENS:
        if token(bad) in code:
            raise RuntimeError(f"malformed enumerator token remains in Start: 0x{bad:08X}")
    if code.count(token(LIST_GAMEOBJECT_COUNT)) != 1:
        raise RuntimeError("List<GameObject>.get_Count token count != 1")
    if code.count(token(LIST_GAMEOBJECT_ITEM)) != 5:
        raise RuntimeError("List<GameObject>.get_Item token count != 5")

    final_eh = bytes(out[EH_FILE : EH_FILE + 16])
    if final_eh != bytes.fromhex("0110000002002f003867000a00000000"):
        raise RuntimeError(f"final EH shape drift {final_eh.hex()}")

    dst.parent.mkdir(parents=True, exist_ok=True)
    dst.write_bytes(out)
    output_sha = sha(bytes(out))
    print(
        "ADMINISTRATOR_START_INDEXED_PATCH_PASS "
        f"input={EXPECTED_INPUT_SHA} output={output_sha} diff_bytes={len(changed)} "
        f"diff_first=0x{min(changed):X} diff_last=0x{max(changed):X}"
    )
    print(
        "INDEXED_LOOP_GATE_PASS "
        f"local_sig=0x{NEW_LOCAL_SIG:08X} get_count=0x{LIST_GAMEOBJECT_COUNT:08X} "
        f"get_item=0x{LIST_GAMEOBJECT_ITEM:08X} enumerator_refs=0 empty_finally=1"
    )
    print(
        "METADATA_DIRECTORY_UNCHANGED_PASS "
        f"offset=0x{METADATA_FILE:X} size={METADATA_SIZE} sha256={metadata_sha}"
    )
    print("PORTABILITY_DEVIATION native_foreach=1 managed_indexed_loop=1 reason=malformed_recovered_List_GameObject_Enumerator_MemberRef")
    print(f"OUTPUT_SHA256 {output_sha}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
