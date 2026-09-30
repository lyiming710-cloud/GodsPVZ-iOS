import fs from 'node:fs';
import path from 'node:path';

console.log('=== Environment Verification ===');
console.log('Platform:', process.platform);
console.log('Node Version:', process.version);
console.log('Architecture:', process.arch);

const checks = [
  { name: 'ios/Info.plist', path: 'ios/Info.plist' },
  { name: 'ios/LaunchScreen.storyboard', path: 'ios/LaunchScreen.storyboard' },
  { name: 'PORTING_STATUS.md', path: 'PORTING_STATUS.md' }
];

let allPassed = true;
for (const check of checks) {
  if (fs.existsSync(check.path)) {
    console.log(`[PASS] Found ${check.name}`);
  } else {
    console.error(`[FAIL] Missing ${check.name}`);
    allPassed = false;
  }
}

if (!allPassed) {
  process.exit(1);
}
console.log('Environment configuration verified successfully.');
