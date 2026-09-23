#!/usr/bin/env python3
"""Prepare locked GodsPVZ 1.0.2 PC native inputs from the original ZIP.

Read-only with respect to the source ZIP. Extracts exactly one GameAssembly.dll and
one global-metadata.dat only after the package hash is recognized, then verifies
both extracted payload hashes before emitting a manifest.
"""

from __future__ import annotations

import argparse
import hashlib
import json
import os
from pathlib import Path
import shutil
import sys
import zipfile

LOCKED_ZIP_SHA256 = "2f08b4e2243e2bd296f3db694b4a2c9656252d76202c80167bdf3340b9f65b48"
LOCKED_GAMEASSEMBLY_SHA256 = "9ebd7ca996a5b03fb4a766f7a2502b660d581ddbbb36af4d7bf2f06a211da39d"
LOCKED_METADATA_SHA256 = "ad992341add498bd1018ac980f42171e0737b0793bf945c8714ad70a445b36b9"


def sha256_file(path: Path) -> str:
    h = hashlib.sha256()
    with path.open("rb") as f:
        for chunk in iter(lambda: f.read(1024 * 1024), b""):
            h.update(chunk)
    return h.hexdigest()


def basename_matches(names: list[str], wanted: str) -> list[str]:
    wanted = wanted.lower()
    return [n for n in names if Path(n.rstrip("/")).name.lower() == wanted]


def extract_member(zf: zipfile.ZipFile, member: str, dst: Path) -> None:
    dst.parent.mkdir(parents=True, exist_ok=True)
    tmp = dst.with_suffix(dst.suffix + ".tmp")
    if tmp.exists():
        tmp.unlink()
    with zf.open(member, "r") as src, tmp.open("wb") as out:
        shutil.copyfileobj(src, out, length=1024 * 1024)
    os.replace(tmp, dst)


def main() -> int:
    ap = argparse.ArgumentParser()
    ap.add_argument("zip", type=Path, help="Original GodsPVZ_1.0.2.zip")
    ap.add_argument("out_dir", type=Path, help="Output directory")
    args = ap.parse_args()

    src = args.zip.resolve()
    out = args.out_dir.resolve()
    if not src.is_file():
        raise SystemExit(f"SOURCE_NOT_FOUND: {src}")

    zip_sha = sha256_file(src)
    if zip_sha != LOCKED_ZIP_SHA256:
        raise SystemExit(f"ZIP_SHA_MISMATCH expected={LOCKED_ZIP_SHA256} actual={zip_sha}")

    with zipfile.ZipFile(src, "r") as zf:
        names = zf.namelist()
        game = basename_matches(names, "GameAssembly.dll")
        meta = basename_matches(names, "global-metadata.dat")
        if len(game) != 1:
            raise SystemExit(f"GAMEASSEMBLY_COUNT expected=1 actual={len(game)} matches={game}")
        if len(meta) != 1:
            raise SystemExit(f"METADATA_COUNT expected=1 actual={len(meta)} matches={meta}")

        out.mkdir(parents=True, exist_ok=True)
        game_out = out / "GameAssembly.dll"
        meta_out = out / "global-metadata.dat"
        extract_member(zf, game[0], game_out)
        extract_member(zf, meta[0], meta_out)

    game_sha = sha256_file(game_out)
    meta_sha = sha256_file(meta_out)
    if game_sha != LOCKED_GAMEASSEMBLY_SHA256:
        raise SystemExit(
            f"GAMEASSEMBLY_SHA_MISMATCH expected={LOCKED_GAMEASSEMBLY_SHA256} actual={game_sha}"
        )
    if meta_sha != LOCKED_METADATA_SHA256:
        raise SystemExit(f"METADATA_SHA_MISMATCH expected={LOCKED_METADATA_SHA256} actual={meta_sha}")

    manifest = {
        "status": "PC_INPUT_PREP_OK",
        "source_zip": str(src),
        "source_zip_sha256": zip_sha,
        "zip_members": {
            "GameAssembly.dll": game[0],
            "global-metadata.dat": meta[0],
        },
        "outputs": {
            "GameAssembly.dll": {
                "path": str(game_out),
                "size": game_out.stat().st_size,
                "sha256": game_sha,
            },
            "global-metadata.dat": {
                "path": str(meta_out),
                "size": meta_out.stat().st_size,
                "sha256": meta_sha,
            },
        },
    }
    manifest_path = out / "pc-input-manifest.json"
    manifest_path.write_text(json.dumps(manifest, indent=2, sort_keys=True) + "\n", encoding="utf-8")
    print(json.dumps(manifest, indent=2, sort_keys=True))
    return 0


if __name__ == "__main__":
    sys.exit(main())
