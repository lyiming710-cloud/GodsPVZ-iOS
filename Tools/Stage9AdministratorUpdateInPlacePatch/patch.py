#!/usr/bin/env python3
from __future__ import annotations

import hashlib
import sys
from pathlib import Path

EXPECTED_INPUT_SHA = "cda6bc387f0376b044ca3c95b40bcadf9d7e1ae16bed2fcc5030baaa176c7b01"
EXPECTED_OUTPUT_SHA = "ad2743f5fc185ccb3b13bd5b4149db341377aebb016ed9f376f5b52b8ce3329f"
EXPECTED_METADATA_SHA = "733f78b3cafe85511475c3037106bad7ee36224aa30c071d10f07fecfeff273e"

TARGET_TOKEN = 0x06000006
MODE_FIELD_TOKEN = 0x0400000A
METHOD_FILE = 0x55C
CODE_FILE = 0x568
CODE_SIZE = 492
NEXT_METHOD_FILE = 0x754
LOCAL_SIG = 0x11000005
PATCH_IL = 0xD4
PATCH_FILE = CODE_FILE + PATCH_IL
METADATA_FILE = 0xB9BC4
METADATA_SIZE = 439400

OLD = bytes.fromhex("21 ff ff ff ff 00 00 00 00")  # ldc.i8 4294967295
NEW = bytes.fromhex("15 00 00 00 00 00 00 00 00")  # ldc.i4.m1 + nop * 8
STFLD_MODE = bytes([0x7D]) + MODE_FIELD_TOKEN.to_bytes(4, "little")


def sha(data: bytes) -> str:
    return hashlib.sha256(data).hexdigest()


def main() -> int:
    if len(sys.argv) != 3:
        print("usage: patch.py <administrator-start-indexed.dll> <output.dll>", file=sys.stderr)
        return 2

    src = Path(sys.argv[1]).resolve()
    dst = Path(sys.argv[2]).resolve()
    original = src.read_bytes()
    input_sha = sha(original)
    if input_sha != EXPECTED_INPUT_SHA:
        raise RuntimeError(f"input SHA drift actual={input_sha} expected={EXPECTED_INPUT_SHA}")
    if len(original) < NEXT_METHOD_FILE:
        raise RuntimeError("input unexpectedly short")

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

    if bytes(original[PATCH_FILE : PATCH_FILE + len(OLD)]) != OLD:
        raise RuntimeError(
            "Update IL_00D4 preimage drift actual="
            + bytes(original[PATCH_FILE : PATCH_FILE + len(OLD)]).hex()
        )
    if bytes(original[PATCH_FILE + len(OLD) : PATCH_FILE + len(OLD) + len(STFLD_MODE)]) != STFLD_MODE:
        raise RuntimeError("Update IL_00DD stfld mode preimage drift")

    metadata_before = sha(original[METADATA_FILE : METADATA_FILE + METADATA_SIZE])
    if metadata_before != EXPECTED_METADATA_SHA:
        raise RuntimeError(f"metadata preimage drift sha={metadata_before}")

    out = bytearray(original)
    out[PATCH_FILE : PATCH_FILE + len(OLD)] = NEW

    changed = [i for i, (a, b) in enumerate(zip(original, out)) if a != b]
    if len(changed) != 5:
        raise RuntimeError(f"unexpected diff byte count {len(changed)}")
    if any(i < METHOD_FILE or i >= NEXT_METHOD_FILE for i in changed):
        raise RuntimeError("diff escaped Administrator.Update allocation")
    if bytes(out[PATCH_FILE : PATCH_FILE + len(NEW)]) != NEW:
        raise RuntimeError("Update replacement write failed")
    if bytes(out[PATCH_FILE + len(NEW) : PATCH_FILE + len(NEW) + len(STFLD_MODE)]) != STFLD_MODE:
        raise RuntimeError("stfld mode moved or changed")

    metadata_after = sha(bytes(out[METADATA_FILE : METADATA_FILE + METADATA_SIZE]))
    if metadata_after != EXPECTED_METADATA_SHA:
        raise RuntimeError(f"metadata changed sha={metadata_after}")

    output_sha = sha(bytes(out))
    if output_sha != EXPECTED_OUTPUT_SHA:
        raise RuntimeError(f"output SHA drift actual={output_sha} expected={EXPECTED_OUTPUT_SHA}")

    dst.parent.mkdir(parents=True, exist_ok=True)
    dst.write_bytes(out)
    print(
        "ADMINISTRATOR_UPDATE_INPLACE_PATCH_PASS "
        f"target=0x{TARGET_TOKEN:08X} input={input_sha} output={output_sha} "
        f"il=0x{PATCH_IL:04X} file=0x{PATCH_FILE:X} diff_bytes={len(changed)}"
    )
    print(
        "ADMINISTRATOR_UPDATE_NATIVE_SEMANTICS_PASS "
        "old=ldc.i8_4294967295 new=ldc.i4.m1+nop8 stfld_mode=0x0400000A"
    )
    print(
        "METADATA_DIRECTORY_UNCHANGED_PASS "
        f"offset=0x{METADATA_FILE:X} size={METADATA_SIZE} sha256={metadata_after}"
    )
    print(f"OUTPUT_SHA256 {output_sha}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
