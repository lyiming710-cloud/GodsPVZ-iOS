// Stable Codespace channel v2: script is base64-encoded into the ssh argv so
// nothing is piped on stdin (more reliable than stdin redirection here).
//   node scripts/local/cs.js <remote-shell-script> [outfile]
const { execFileSync } = require('child_process');
const fs = require('fs');

const CS = 'glowing-train-p7j9gp74q6jwc76v6';
const scriptFile = process.argv[2];
const outFile = process.argv[3] || 'C:\\Users\\86136\\Desktop\\1.0.2PC\\audit\\native22-work\\_cs_out.txt';

let script = fs.readFileSync(scriptFile, 'utf8');
script = script.replace(/^#!.*\r?\n/, '').replace(/\r\n/g, '\n');
const b64 = Buffer.from(script, 'utf8').toString('base64');

let ghexe = 'gh';
for (const cand of [
  'C:\\Program Files\\GitHub CLI\\gh.exe',
  process.env.LOCALAPPDATA + '\\Programs\\GitHub CLI\\gh.exe',
]) {
  try { if (cand && fs.existsSync(cand)) { ghexe = cand; break; } } catch (_) {}
}

const psCmd =
  '$ProgressPreference="SilentlyContinue";' +
  '$env:HTTPS_PROXY="http://127.0.0.1:10808";$env:HTTP_PROXY="http://127.0.0.1:10808";' +
  '$env:https_proxy="http://127.0.0.1:10808";$env:http_proxy="http://127.0.0.1:10808";' +
  '& "' + ghexe + '" codespace ssh -c "' + CS + '" -- "bash -lc ' + "'" + 'echo ' + b64 + ' | base64 -d | bash -s' + "'" + '" 2>&1';

const psArgs = ['-NoProfile', '-NonInteractive', '-Command', psCmd];
let out;
try {
  out = execFileSync('powershell.exe', psArgs, {
    encoding: 'utf8', windowsHide: true, maxBuffer: 1024 * 1024 * 256,
    timeout: 10 * 60 * 1000,
  });
} catch (e) {
  out = 'EXIT=' + e.status + '\n' + (e.stdout || '') + '\nSTDERR:\n' + (e.stderr || '');
}
fs.writeFileSync(outFile, out, 'utf8');
console.log('WROTE ' + outFile + ' len=' + out.length);
