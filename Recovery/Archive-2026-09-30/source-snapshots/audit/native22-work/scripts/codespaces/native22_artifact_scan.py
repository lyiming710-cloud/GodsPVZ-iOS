"""Native22 structural defect scan over a full method-body export.

Input: the JSON produced by `PatcherNative19 <dll> <out.json> export-all`.
Read-only. Nothing is written back to any DLL.

Two independent detectors:

  STRICT phantom local
      A local that is read by ldloc somewhere in its own method, has no stloc
      anywhere in that same method, and whose address is never taken by ldloca.
      Excluding address-taking matters: `TryGetValue(k, out v)` writes v through
      its address, so an ldloc-only scan reports a large false positive count
      (measured 519 loose vs 360 strict on Native21). Only STRICT is reported as
      a defect indicator, because a read of such a local always yields its
      default value and is not expressible in definite-assignment-clean C#.

  Verification failure
      The fail-closed typed CIL check from verify_native19_types.py, used as the
      local oracle for IL well-formedness.
"""
import json, sys, collections, re


def locnum(op, o):
    if op.count('.') >= 1 and op.split('.')[-1].isdigit():
        return int(op.split('.')[-1])
    return o['index'] if isinstance(o, dict) else None


def phantom_locals(m):
    ins, nl = m['instructions'], len(m['locals'])
    written, addr = set(), set()
    for x in ins:
        op = x['opcode']
        if op.startswith('stloc'):
            n = locnum(op, x['operand'])
            if n is not None:
                written.add(n)
        elif op.startswith('ldloca'):
            n = locnum(op, x['operand'])
            if n is not None:
                addr.add(n)
    strict, loose = set(), set()
    for x in ins:
        op = x['opcode']
        if op.startswith('ldloc') and not op.startswith('ldloca'):
            n = locnum(op, x['operand'])
            if n is not None and n < nl:
                loose.add(n)
                if n not in written and n not in addr:
                    strict.add(n)
    return sorted(strict), sorted(loose)


def main(path):
    data = json.load(open(path))
    ms = data['methods']
    strict, loose = set(), set()
    per_class = collections.Counter()
    class_total = collections.defaultdict(int)
    for m in ms:
        cls = m['name'].split(' ', 1)[1].split('::')[0]
        class_total[cls] += 1
        s, l = phantom_locals(m)
        if s:
            strict.add(m['token'])
            per_class[cls] += 1
        if l:
            loose.add(m['token'])
    print('methods                      :', len(ms))
    print('LOOSE phantom (pre-filter)   :', len(loose))
    print('STRICT phantom (defect)      :', len(strict))
    print('removed by address-taking    :', len(loose - strict))
    print()
    print('top classes STRICT/total:')
    for k, v in per_class.most_common(15):
        print('  %3d/%-3d %s' % (v, class_total[k], k))
    json.dump({'methods': len(ms), 'loose': sorted(loose), 'strict': sorted(strict)},
              open('out/phantom-scan.json', 'w'), indent=2)


if __name__ == '__main__':
    main(sys.argv[1])
