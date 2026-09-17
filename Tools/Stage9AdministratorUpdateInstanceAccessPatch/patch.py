#!/usr/bin/env python3
from __future__ import annotations

import hashlib
import sys
from pathlib import Path

EXPECTED_INPUT_SHA = "2ba80feb6cd332148b950953675ac6e8d38b57f0c5944114503eea4caa442d83"
EXPECTED_OUTPUT_SHA = "b9eb85ee19a9d8b2c9c36c7dd5b9fbd16f24df6af9a0d5c2eeebfba702871b80"
EXPECTED_METADATA_SHA = "733f78b3cafe85511475c3037106bad7ee36224aa30c071d10f07fecfeff273e"

TARGET_TOKEN = 0x06000006
INSTANCE_FIELD_TOKEN = 0x040001AD
GET_INSTANCE_TOKEN = 0x06000169
METHOD_FILE = 0x55C
CODE_FILE = 0x568
CODE_SIZE = 492
NEXT_METHOD_FILE = 0x754
LOCAL_SIG = 0x11000005
PATCH_IL = 0x01DE
PATCH_FILE = CODE_FILE + PATCH_IL
METADATA_FILE = 0xB9BC4
METADATA_SIZE = 439400

OLD = bytes([0x7E]) + INSTANCE_FIELD_TOKEN.to_bytes(4, "little")  # ldsfld BoardManager::<Instance>k__BackingField
NEW = bytes([0x28]) + GET_INSTANCE_TOKEN.to_bytes(4, "little")   # call BoardManager::get_Instance()


def sha(data: bytes) -> str:
    return hashlib.sha256(data).hexdigest()


def main() -> int:
    if len(sys.argv) != 3:
        print("usage: patch.py <administrator-update-boardaccess.dll> <output.dll>", file=sys.stderr)
        return 2

    src = Path(sys.argv[1]).resolve()
    dst = Path(sys.argv[2]).resolve()
    original = src.read_bytes()
    input_sha = sha(original)
    if input_sha != EXPECTED_INPUT_SHA:
        raise RuntimeError(f"input SHA drift actual={input_sha} expected={EXPECTED_INPUT_SHA}")

    flags = int.from_bytes(original[METHOD_FILE : METHOD_FILE + 2], "little")
    header_size = ((flags >> 12) & 0xF) * 4
    code_size = int.from_bytes(original[METHOD_FILE + 4 : METHOD_FILE + 8], "little")
    local_sig = int.from_bytes(original[METHOD_FILE + 8 : METHOD_FILE + 12], "little")
    if (flags & 3) != 3 or header_size != 12 or code_size != CODE_SIZE or local_sig != LOCAL_SIG:
        raise RuntimeError(
            f"Update header drift flags=0x{flags:X} header={header_size} code={code_size} localsig=0x{local_sig:08X}"
        )
    if METHOD_FILE + header_size + code_size != NEXT_METHOD_FILE:
        raise RuntimeError("Update allocation boundary drift")

    if bytes(original[PATCH_FILE : PATCH_FILE + 5]) != OLD:
        raise RuntimeError(
            f"Update IL_01DE preimage drift actual={bytes(original[PATCH_FILE:PATCH_FILE+5]).hex()} expected={OLD.hex()}"
        )

    metadata_before = sha(original[METADATA_FILE : METADATA_FILE + METADATA_SIZE])
    if metadata_before != EXPECTED_METADATA_SHA:
        raise RuntimeError(f"metadata preimage drift sha={metadata_before}")

    out = bytearray(original)
    out[PATCH_FILE : PATCH_FILE + 5] = NEW

    changed = [i for i, (a, b) in enumerate(zip(original, out)) if a != b]
    if len(changed) != 3:
        raise RuntimeError(f"unexpected diff byte count {len(changed)}")
    if any(i < METHOD_FILE or i >= NEXT_METHOD_FILE for i in changed):
        raise RuntimeError("diff escaped Administrator.Update allocation")
    if bytes(out[PATCH_FILE : PATCH_FILE + 5]) != NEW:
        raise RuntimeError("get_Instance replacement failed")

    metadata_after = sha(bytes(out[METADATA_FILE : METADATA_FILE + METADATA_SIZE]))
    if metadata_after != EXPECTED_METADATA_SHA:
        raise RuntimeError(f"metadata changed sha={metadata_after}")

    output_sha = sha(bytes(out))
    if output_sha != EXPECTED_OUTPUT_SHA:
        raise RuntimeError(f"output SHA drift actual={output_sha} expected={EXPECTED_OUTPUT_SHA}")

    dst.parent.mkdir(parents=True, exist_ok=True)
    dst.write_bytes(out)
    print(
        "ADMINISTRATOR_UPDATE_INSTANCEACCESS_PATCH_PASS "
        f"target=0x{TARGET_TOKEN:08X} input={input_sha} output={output_sha} "
        f"il=0x{PATCH_IL:04X} file=0x{PATCH_FILE:X} diff_bytes={len(changed)}"
    )
    print(
        "INSTANCE_ACCESS_EQUIVALENCE_PASS "
        "old=ldsfld_BoardManager.InstanceBackingField new=call_BoardManager.get_Instance "
        "get_instance_body=br+ldsfld_InstanceBackingField+ret"
    )
    print(
        "METADATA_DIRECTORY_UNCHANGED_PASS "
        f"offset=0x{METADATA_FILE:X} size={METADATA_SIZE} sha256={metadata_after}"
    )
    print(f"OUTPUT_SHA256 {output_sha}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
