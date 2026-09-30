#!/usr/bin/env python3
import argparse, hashlib, io, json, os, shutil, urllib.error, urllib.request, zipfile
from pathlib import Path

REPO = "lyiming710-cloud/GodsPVZ-iOS"
API = f"https://api.github.com/repos/{REPO}"
TOKEN = os.environ.get("GH_TOKEN") or os.environ.get("GITHUB_TOKEN")
if not TOKEN:
    raise SystemExit("GH_TOKEN/GITHUB_TOKEN is required")

HEADERS = {
    "Authorization": f"Bearer {TOKEN}",
    "Accept": "application/vnd.github+json",
    "User-Agent": "GodsPVZ-takeover-artifact-fetch",
}

class NoRedirect(urllib.request.HTTPRedirectHandler):
    def redirect_request(self, *args, **kwargs):
        return None


def api_json(url: str):
    req = urllib.request.Request(url, headers=HEADERS)
    with urllib.request.urlopen(req, timeout=90) as r:
        return json.loads(r.read().decode("utf-8"))


def artifact_meta(artifact_id: int):
    return api_json(f"{API}/actions/artifacts/{artifact_id}")


def download_zip(artifact_id: int) -> bytes:
    url = f"{API}/actions/artifacts/{artifact_id}/zip"
    req = urllib.request.Request(url, headers=HEADERS)
    opener = urllib.request.build_opener(NoRedirect)
    try:
        with opener.open(req, timeout=90) as r:
            return r.read()
    except urllib.error.HTTPError as e:
        if e.code not in (301, 302, 303, 307, 308):
            raise
        with urllib.request.urlopen(e.headers["Location"], timeout=300) as r:
            return r.read()


def expected_digest(meta: dict):
    d = meta.get("digest") or ""
    if d.startswith("sha256:"):
        return d.split(":", 1)[1].lower()
    return None


def fetch_one(artifact_id: int, dest: Path, explicit_digest: str | None = None):
    meta = artifact_meta(artifact_id)
    data = download_zip(artifact_id)
    got = hashlib.sha256(data).hexdigest()
    exp = (explicit_digest or expected_digest(meta) or "").removeprefix("sha256:").lower()
    if exp and got != exp:
        raise SystemExit(f"artifact {artifact_id} ZIP SHA mismatch: {got} != {exp}")
    dest.mkdir(parents=True, exist_ok=True)
    with zipfile.ZipFile(io.BytesIO(data)) as z:
        z.extractall(dest)
    print(f"ARTIFACT_FETCH_PASS id={artifact_id} name={meta.get('name')} zip_sha256={got} dest={dest}")
    return meta, got


def main():
    p = argparse.ArgumentParser()
    g = p.add_mutually_exclusive_group(required=True)
    g.add_argument("--artifact-id", type=int)
    g.add_argument("--run-id", type=int)
    p.add_argument("--prefix")
    p.add_argument("--dest", required=True)
    p.add_argument("--zip-sha256")
    a = p.parse_args()
    dest = Path(a.dest)
    if a.artifact_id:
        fetch_one(a.artifact_id, dest, a.zip_sha256)
        return
    if not a.prefix:
        raise SystemExit("--prefix is required with --run-id")
    listing = api_json(f"{API}/actions/runs/{a.run_id}/artifacts?per_page=100")
    hits = sorted((x for x in listing.get("artifacts", []) if x.get("name", "").startswith(a.prefix)), key=lambda x: x["name"])
    if not hits:
        raise SystemExit(f"no artifacts in run {a.run_id} with prefix {a.prefix!r}")
    print(f"RUN_PREFIX_MATCH run={a.run_id} prefix={a.prefix} count={len(hits)}")
    for x in hits:
        fetch_one(int(x["id"]), dest)

if __name__ == "__main__":
    main()
