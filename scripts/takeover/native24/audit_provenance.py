"""Reclassify only the fixed 1623 historical sites. Do not emit any game patch."""
import argparse
import collections
import copy
import hashlib
import json
import pathlib
import sys
sys.dont_write_bytecode = True
import provenance as P
import reproduce_findings as R

ROOT = pathlib.Path(__file__).resolve().parents[3]


def errors(body, types):
    # A method outside this gate's instruction/EH subset remains explicitly unproved.
    return {(x['offset'], x['msg']): x for x in P.verify_prov(body, types)}


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('--work', type=pathlib.Path, default=ROOT/'.validation/native24')
    parser.add_argument('--output', type=pathlib.Path, required=True)
    args = parser.parse_args()
    data = json.loads((args.work/'review/all-methods.batch1.json').read_text())
    methods = {m['token']: m for m in data['methods']}
    edits = json.loads((args.work/'inputs/edits-batch2.json').read_text())['edits']
    assert len(edits) == 1623 and len({(e['token'], e['offset']) for e in edits}) == 1623
    baseline_errors, records = {}, []
    for n, edit in enumerate(edits):
        token, offset = edit['token'], edit['offset']
        method = methods[token]
        if token not in baseline_errors:
            baseline_errors[token] = errors(method, data['types'])
        old = baseline_errors[token]
        trial = copy.deepcopy(method)
        ins = next(x for x in trial['instructions'] if x['offset'] == offset)
        assert R.C.A.is_zero_lit(ins), (token, offset)
        ins['opcode'], ins['operand'] = 'ldnull', None
        new = errors(trial, data['types'])
        removed = [old[key] for key in old.keys()-new.keys()]
        added = [new[key] for key in new.keys()-old.keys()]
        observations = [R.C.classify(error, data['types']) for error in removed]
        supported = (bool(removed) and not added and all(R.C.is_afam(error, data['types']) for error in removed)
                     and all(verdict == 'ACCEPT' for verdict, _ in observations))
        record = {'token': token, 'name': method['name'], 'offset': offset,
                  'E2': 'SOURCE_SUPPORTED_ONLY' if supported else 'UNPROVED_QUARANTINE',
                  'removed': removed, 'added': added, 'observations': observations,
                  'E5': 'NOT_PROVED', 'E6': 'NOT_PROVED'}
        records.append(record)
        if (n+1) % 200 == 0:
            print(f'classified {n+1}/1623', flush=True)
    historical = json.loads((args.work/'review/analysis.json').read_text())
    risky = historical['actual_accepted_analysis']['all_direct_local_receiver_sites']
    risky_ids = {(s['token'], s['site']) for s in risky}
    assert len(risky_ids) == 225
    survivors = [s for s in records if (s['token'], s['offset']) in risky_ids and s['E2'] == 'SOURCE_SUPPORTED_ONLY']
    assert not survivors, 'local receiver was laundered again'
    counts = collections.Counter(x['E2'] for x in records)
    result = {'scope': 'All 1623 fixed historical Batch2 sites; source evidence only; no patch/promotion',
              'source_sha256': hashlib.sha256(pathlib.Path(P.__file__).read_bytes()).hexdigest(),
              'methods': len({x['token'] for x in records}), 'sites': len(records), 'counts': dict(counts),
              'reviewed_local_receiver_sites': 225, 'local_receiver_sites_supported': len(survivors),
              'EH_policy': 'Unmodeled exception source state stays unproved', 'records': records}
    args.output.write_text(json.dumps(result, indent=2)+'\n')
    print(json.dumps({k: v for k, v in result.items() if k != 'records'}))


if __name__ == '__main__':
    main()
