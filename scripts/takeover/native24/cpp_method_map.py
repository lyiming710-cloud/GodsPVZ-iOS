"""Attribute diagnostics only to balanced function definitions, never prototypes."""
import bisect
import collections
import hashlib
import json
import pathlib
import re


def sha(path):
    return hashlib.sha256(path.read_bytes()).hexdigest()


def masked_source(text):
    # Preserve positions/newlines while hiding comments and quoted braces.
    pattern = r'//[^\n]*|/\*.*?\*/|"(?:\\.|[^"\\])*"|\'(?:\\.|[^\'\\])*\''
    return re.sub(pattern, lambda m: ''.join('\n' if c == '\n' else ' ' for c in m[0]),
                  text, flags=re.S)


def balanced_end(text, start, opening, closing):
    depth = 0
    for i in range(start, len(text)):
        if text[i] == opening:
            depth += 1
        elif text[i] == closing:
            depth -= 1
            if depth == 0:
                return i
    raise ValueError(f'unclosed {opening} at {start}')


def definitions(text):
    masked = masked_source(text)
    matches = re.finditer(r'^IL2CPP_EXTERN_C\s+IL2CPP_METHOD_ATTR[^\n;{]*?\b([A-Za-z_][A-Za-z0-9_]*)\s*\(',
                          masked, re.M)
    for match in matches:
        end = balanced_end(masked, match.end()-1, '(', ')')+1
        while end < len(masked) and masked[end].isspace():
            end += 1
        if end < len(masked) and masked[end] == ';':
            continue
        if end == len(masked) or masked[end] != '{':
            raise ValueError(f'unrecognized declaration suffix for {match[1]}')
        stop = balanced_end(masked, end, '{', '}')
        yield match[1], text.count('\n', 0, match.start())+1, text.count('\n', 0, stop)+1


def pointer_table(path):
    text = path.read_text(encoding='utf-8-sig')
    match = re.search(r's_methodPointers\[(\d+)\]\s*=\s*\{(.*?)\n\};', text, re.S)
    if not match:
        raise ValueError('missing method pointer table')
    rows = [x.strip() for x in masked_source(match[2]).split(',') if x.strip()]
    if len(rows) != int(match[1]) or len(rows) != 2317:
        raise ValueError('MethodDef pointer-table invariant failed')
    out = collections.defaultdict(list)
    for index, symbol in enumerate(rows):
        if symbol not in ('NULL', '0'):
            if not re.fullmatch(r'[A-Za-z_][A-Za-z0-9_]*', symbol):
                raise ValueError('unrecognized pointer '+symbol)
            out[symbol].append(f'0x{0x06000001+index:08X}')
    return out


def attribute(cpp, logs, manifest):
    """Require exact source hashes and one compiler log for every translation unit."""
    cpp, logs = pathlib.Path(cpp), pathlib.Path(logs)
    table = pointer_table(cpp/'GodsPVZRuntime1_CodeGen.c')
    bodies, counts, outside = {}, collections.Counter(), []
    seen_symbols = set()
    units = sorted(cpp.glob('*.cpp'))
    if {f.name for f in units} != set(manifest):
        raise ValueError('manifest translation unit set differs')
    for source in units:
        actual = sha(source)
        if actual != manifest[source.name]['sha256']:
            raise ValueError('source hash mismatch: '+source.name)
        regions = list(definitions(source.read_text(encoding='utf-8-sig')))
        relevant = [(sym, start, end) for sym, start, end in regions if sym in table]
        for symbol, start, end in relevant:
            if symbol in seen_symbols:
                raise ValueError('duplicate definition: '+symbol)
            seen_symbols.add(symbol)
            for token in table[symbol]:
                bodies[token] = {'symbol': symbol, 'file': source.name, 'start': start, 'end': end,
                                 'file_sha256': actual, 'errors': []}
        starts = [r[1] for r in relevant]
        log = logs/(source.name+'.log')
        if not log.is_file():
            raise ValueError('missing compiler log: '+source.name)
        for line in log.read_text(errors='replace').splitlines():
            m = re.match(r'^(.+?):(\d+):(\d+): (?:fatal )?error: (.*)', line)
            if not m:
                continue
            counts['total_errors'] += 1
            path, number = pathlib.Path(m[1]), int(m[2])
            if path.name != source.name:
                counts['included_file_errors'] += 1
                outside.append({'tu': source.name, 'file': m[1], 'line': number, 'message': m[4]})
                continue
            counts['cpp_errors'] += 1
            index = bisect.bisect_right(starts, number)-1
            if index < 0 or not relevant[index][1] <= number <= relevant[index][2]:
                counts['cpp_errors_outside_game_methods'] += 1
                outside.append({'tu': source.name, 'file': m[1], 'line': number, 'message': m[4]})
                continue
            for token in table[relevant[index][0]]:
                bodies[token]['errors'].append({'line': number, 'col': int(m[3]), 'message': m[4]})
    return {'summary': {**counts, 'translation_units': len(units), 'mapped_method_tokens': len(bodies),
                        'methods_with_errors': sum(bool(v['errors']) for v in bodies.values())},
            'methods': bodies, 'unattributed_or_included_diagnostics': outside}
