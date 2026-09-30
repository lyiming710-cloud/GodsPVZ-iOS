// Stable local shell channel: PowerShell output is written to a file then read back.
// Usage: node scripts/ps.js "<powershell command>" [outfile]
const { execFileSync } = require('child_process');
const fs = require('fs');
const cmd = process.argv[2];
const out = process.argv[3] || 'C:\\Users\\86136\\Desktop\\1.0.2PC\\audit\\native22-work\\_ps_out.txt';
let res;
try {
  res = execFileSync('powershell.exe', ['-NoProfile', '-NonInteractive', '-Command', cmd], {
    encoding: 'utf8', windowsHide: true, maxBuffer: 1024 * 1024 * 512,
  });
} catch (e) {
  res = 'EXIT=' + e.status + '\n' + (e.stdout || '') + '\nSTDERR:\n' + (e.stderr || '');
}
fs.writeFileSync(out, res, 'utf8');
console.log('WROTE ' + out + ' len=' + res.length);
