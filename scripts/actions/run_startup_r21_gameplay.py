"""Drive a visible licensed Unity Editor via real X11/XTest mouse input.

The Editor helper requests points derived from the actual GameView and records
Input.mousePosition/GetMouseButton observations. This host never edits a game
save, unit, map, sun balance or assembly. Run inside xvfb-run, without -batchmode.
"""
from pathlib import Path
import argparse, hashlib, json, os, subprocess, time


def atomic_json(path, value):
    temp = path.with_suffix('.tmp')
    temp.write_text(json.dumps(value, indent=2) + '\n', 'utf-8')
    temp.replace(path)


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('--unity', required=True)
    parser.add_argument('--project', required=True)
    parser.add_argument('--output', required=True)
    args = parser.parse_args()
    output = Path(args.output).resolve()
    output.mkdir(parents=True, exist_ok=False)
    plugin = Path(args.project) / 'Assets/Plugins/GodsPVZRuntime1.dll'
    expected = '75160a3233f1e1ab13a44f712efb4b47fdeb9b57bd38fa25f180276bb7b50e15'
    assert hashlib.sha256(plugin.read_bytes()).hexdigest() == expected
    env = dict(os.environ, GODSPVZ_GAMEPLAY_OUTPUT=str(output))
    command = [args.unity, '-force-glcore', '-projectPath', args.project,
               '-executeMethod', 'GodsPVZGameplayAudit.Begin',
               '-logFile', str(output / 'gameplay-unity.log')]
    assert '-batchmode' not in command and '-quit' not in command
    (output / 'INVOCATION.json').write_text(json.dumps(dict(
        scope='REAL_VISIBLE_EDITOR_AND_XTEST_INPUT_NOT_IOS',
        command=command, display=env.get('DISPLAY'), candidate_sha256=expected,
        secret_values_recorded=False), indent=2) + '\n', 'utf-8')
    logs = []

    def xdo(*parameters):
        before = time.monotonic()
        result = subprocess.run(['xdotool', *map(str, parameters)],
                                env=env, capture_output=True, text=True,
                                timeout=15)
        logs.append(dict(command=['xdotool', *map(str, parameters)],
                         exit_code=result.returncode, stdout=result.stdout,
                         stderr=result.stderr,
                         duration_seconds=round(time.monotonic() - before, 4)))
        if result.returncode:
            raise RuntimeError('XTest input command failed: ' + result.stderr)
        return result.stdout

    process = None
    last_sequence = 0
    timeout = False
    exception = None
    start = time.monotonic()
    try:
        with (output / 'gameplay-process.stdout.log').open('wb') as stdout, \
             (output / 'gameplay-process.stderr.log').open('wb') as stderr:
            process = subprocess.Popen(command, env=env, stdout=stdout, stderr=stderr,
                                       start_new_session=True)
            while process.poll() is None:
                if time.monotonic() - start > 500:
                    timeout = True
                    break
                request_path = output / 'INPUT-REQUEST.json'
                try:
                    request = json.loads(request_path.read_bytes()) if request_path.exists() else None
                except (json.JSONDecodeError, OSError):
                    time.sleep(.1)
                    continue
                if not request or request['sequence'] <= last_sequence:
                    time.sleep(.1)
                    continue
                last_sequence = request['sequence']
                assert request['kind'] in ('move', 'click')
                assert 0 <= request['x'] < 1600 and 0 <= request['y'] < 1000
                # The Editor helper focuses its real GameView. Avoid EWMH
                # activation commands: Xvfb has no window manager by default.
                xdo('mousemove', '--sync', request['x'], request['y'])
                if request['kind'] == 'click':
                    xdo('mousedown', 1)
                    atomic_json(output / 'INPUT-ACK.json', dict(
                        sequence=last_sequence, phase='pressed', exit_code=0,
                        detail='XTest mouse button held until actual Unity observes two frames'))
                    release = output / f'INPUT-RELEASE-{last_sequence}.txt'
                    held = time.monotonic()
                    while not release.exists() and process.poll() is None and time.monotonic() - held < 6:
                        time.sleep(.05)
                    xdo('mouseup', 1)
                    detail = 'UNITY_OBSERVED_RELEASE' if release.exists() else 'HOST_SIX_SECOND_SAFETY_RELEASE_NOT_INPUT_ACCEPTANCE'
                else:
                    time.sleep(.6)  # Give the actual Editor player loop time to observe the move.
                    detail = 'XTest move delivered; actual Unity position checked by helper'
                location = xdo('getmouselocation', '--shell')
                atomic_json(output / 'INPUT-ACK.json', dict(
                    sequence=last_sequence, phase='complete', exit_code=0,
                    detail=detail + '\n' + location))
            if timeout:
                process.terminate()
            try:
                process.wait(timeout=25)
            except subprocess.TimeoutExpired:
                process.kill()
                process.wait(timeout=15)
    except Exception as error:
        exception = repr(error)
        if process is not None and process.poll() is None:
            process.terminate()
            try:
                process.wait(timeout=20)
            except subprocess.TimeoutExpired:
                process.kill()
                process.wait(timeout=15)
    finally:
        # Release even if a game error interrupts the input handshake.
        try:
            xdo('mouseup', 1)
        except Exception as release_error:
            logs.append(dict(cleanup_error=repr(release_error)))
        (output / 'OS-INPUT-COMMANDS.json').write_text(json.dumps(logs, indent=2) + '\n', 'utf-8')
        unchanged = hashlib.sha256(plugin.read_bytes()).hexdigest() == expected
        (output / 'HOST-RESULT.json').write_text(json.dumps(dict(
            scope='HOST_COLLECTION_ONLY_NOT_GAMEPLAY_PASS',
            unity_exit=None if process is None else process.returncode,
            timeout=timeout, exception=exception, candidate_unchanged=unchanged,
            last_input_sequence=last_sequence,
            elapsed_seconds=round(time.monotonic() - start, 3)), indent=2) + '\n', 'utf-8')
    # Exit 0 means evidence exists and collection ran. It does not mean the
    # game passed. The workflow separately evaluates GAMEPLAY-RESULT.json.
    if timeout or exception or not unchanged or process is None or process.returncode:
        raise SystemExit(79)


if __name__ == '__main__':
    main()
