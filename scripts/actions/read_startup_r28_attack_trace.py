"""Rebuild Unity profiler ancestry from raw immediate-child counts.

This reader does not trust the helper's parent links, classified hits or
summary counters. A stack observation does not expose a TakeDamage argument,
prove the damage implementation, prove a win or qualify an iOS build.
"""
from pathlib import Path
import argparse, hashlib, json, re

CANDIDATE = '9081408a2226a79e23991355108540a0260a3966a500cecf4524f3cba9dfb364'


def is_method(name, owner, method):
    name = str(name or '').replace('::', '.').replace(':', '.')
    return re.search(r'(?<![\w+])' + re.escape(owner + '.' + method) + r'(?=\(| \(|$)', name) is not None


def frame_stacks(raw):
    samples = raw['samples']
    if raw['sample_count'] != len(samples):
        return dict(valid=False, error='SAMPLE_COUNT', rows=[])
    parents, active, ends = [], [], {}
    valid = True
    for index, sample in enumerate(samples):
        while active and active[-1][1] == 0:
            ends[active.pop()[0]] = index
        if active:
            parent = active[-1][0]
            active[-1][1] -= 1
        else:
            parent = -1
        parents.append(parent)
        children = sample['children']
        if type(children) is not int or children < 0 or sample['index'] != index or sample['parent'] != parent:
            valid = False
        active.append([index, max(children, 0)])
    while active:
        node, remaining = active.pop()
        ends[node] = len(samples)
        if remaining:
            valid = False
    for index, sample in enumerate(samples):
        if ends.get(index, len(samples)) - index - 1 != sample['descendants']:
            valid = False
    if raw.get('tree_valid') is not True:
        valid = False
    rows = []
    if valid and raw.get('profiler_enabled') is True and raw.get('deep_profiling') is True:
        for index, sample in enumerate(samples):
            zombie = is_method(sample['name'], 'Zombie', 'TakeDamage')
            device = is_method(sample['name'], 'Device', 'TakeDamage')
            if not (zombie or device):
                continue
            ancestry, parent = [], parents[index]
            while parent >= 0:
                ancestry.append(samples[parent]['name'])
                parent = parents[parent]
            ancestry.reverse()
            plant = any(is_method(name, 'Plant', 'Shooting') or is_method(name, 'Plant', 'ST_Peasniper') for name in ancestry)
            if plant:
                rows.append(dict(frame=raw['frame'], thread=raw['thread'], sample_index=index,
                                 target_method='Zombie.TakeDamage' if zombie else 'Device.TakeDamage',
                                 sample_name=sample['name'], ancestors=ancestry,
                                 target_argument_identity_captured=False))
    return dict(valid=valid, rows=rows)


def read(folder):
    folder = Path(folder)
    summary = json.loads((folder / 'R28-ACTUAL-ATTACK-TRACE.json').read_bytes())
    assert summary['candidate_sha256'] == CANDIDATE, 'Wrong profiler candidate'
    assert not summary['game_method_bytes_changed'] and not summary['game_fields_written']
    raw_path = folder / 'ATTACK-RAW-PROFILER-FRAMES.jsonl'
    frames, seen, rows = [], set(), []
    if raw_path.exists():
        with raw_path.open(encoding='utf-8') as source:
            for line in source:
                if not line.strip():
                    continue
                raw = json.loads(line)
                identity = (raw['frame'], raw['thread'])
                assert identity not in seen, 'Repeated profiler frame/thread'
                seen.add(identity)
                result = frame_stacks(raw)
                rows.extend(result['rows'])
                frames.append(dict(frame=raw['frame'], thread=raw['thread'], samples=raw['sample_count'],
                                   valid=result['valid'], plant_damage_stacks=len(result['rows'])))
    raw_count_consistent = len(frames) == summary['raw_frames_saved']
    observed = bool(rows) and raw_count_consistent and not summary['error']
    reported = summary['plant_to_zombie_damage_stacks'] + summary['plant_to_device_damage_stacks']
    result = dict(scope='INDEPENDENT_RAW_UNITY_CALL_TREE_READBACK_NOT_DAMAGE_RESULT_OR_IPA',
                  candidate_sha256=CANDIDATE, plant_damage_call_stack_observed=observed,
                  raw_frame_count_consistent=raw_count_consistent,
                  helper_reported_plant_damage_stacks=reported,
                  raw_saved_plant_damage_stacks=len(rows),
                  summary_positive_without_raw_support=reported > 0 and not rows,
                  frames=frames, rows=rows, profiler_error=summary['error'],
                  target_argument_identity_captured=False, damage_calculation_accepted=False,
                  whole_level_accepted=False, whole_game_accepted=False, ipad_accepted=False,
                  raw_file_sha256=hashlib.sha256(raw_path.read_bytes()).hexdigest() if raw_path.exists() else None)
    return result


if __name__ == '__main__':
    parser = argparse.ArgumentParser()
    parser.add_argument('--folder', required=True)
    parser.add_argument('--output', required=True)
    args = parser.parse_args()
    output = Path(args.output)
    assert not output.exists(), 'Preserve existing readback'
    result = read(args.folder)
    output.write_text(json.dumps(result, indent=2) + '\n', encoding='utf-8')
    print(json.dumps({key: value for key, value in result.items() if key not in ['frames', 'rows']}))
    raise SystemExit(0 if result['plant_damage_call_stack_observed'] else 1)
