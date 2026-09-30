// Local fs helper using pure node (avoids the broken shell channels).
// Usage: node scripts/local/lsd.js <dir> [maxDepth]
const fs = require('fs');
const path = require('path');
const root = process.argv[2];
const maxDepth = parseInt(process.argv[3] || '3', 10);
const lines = [];
function walk(d, depth) {
  let ents;
  try { ents = fs.readdirSync(d, { withFileTypes: true }); } catch (e) { return; }
  ents.sort((a, b) => a.name.localeCompare(b.name));
  for (const e of ents) {
    if (e.name === '.git' || e.name === 'node_modules') continue;
    const p = path.join(d, e.name);
    if (e.isDirectory()) {
      lines.push('  '.repeat(depth) + '[D] ' + e.name);
      if (depth < maxDepth) walk(p, depth + 1);
    } else {
      let sz = -1;
      try { sz = fs.statSync(p).size; } catch (_) {}
      lines.push('  '.repeat(depth) + sz.toString().padStart(10) + '  ' + e.name);
    }
  }
}
walk(root, 0);
console.log(lines.join('\n'));
