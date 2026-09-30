// Run a PowerShell script file and write its output to a result file.
//   node scripts/local/ps2.js <psFile> [outFile]
const { execFileSync } = require('child_process');
const fs = require('fs');
const psFile = process.argv[2];
const outFile = process.argv[3] || psFile + '.out.txt';
let out;
try {
  out = execFileSync('powershell.exe', ['-NoProfile', '-NonInteractive', '-ExecutionPolicy', 'Bypass', '-File', psFile], {
    encoding: 'utf8', windowsHide: true, maxBuffer: 1024 * 1024 * 256, timeout: 20 * 60 * 1000,
  });
} catch (e) {
  out = 'EXIT=' + e.status + '\n' + (e.stdout || '') + '\nSTDERR:\n' + (e.stderr || '');
}
fs.writeFileSync(outFile, out, 'utf8');
console.log('OK len=' + out.length);
