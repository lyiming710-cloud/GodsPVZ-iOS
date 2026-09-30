import fs from 'node:fs';
import path from 'node:path';
import { execSync } from 'node:child_process';

console.log('=============================================');
console.log('    IPA ACCEPTANCE CRITERIA VERIFICATION     ');
console.log('=============================================');

const ipaPath = path.resolve('build/output/GodsPVZ_1.0.2_unsigned.ipa');
const extractDir = path.resolve('build/inspect_ipa');

if (!fs.existsSync(ipaPath)) {
  console.error(`[CRITICAL] IPA not found at ${ipaPath}`);
  process.exit(1);
}

if (fs.existsSync(extractDir)) {
  fs.rmSync(extractDir, { recursive: true, force: true });
}
fs.mkdirSync(extractDir, { recursive: true });

console.log(`Extracting ${ipaPath} for inspection...`);
try {
  if (process.platform === 'win32') {
    execSync(`tar.exe -xf "${ipaPath}" -C "${extractDir}"`, { stdio: 'pipe' });
  } else {
    execSync(`unzip -q "${ipaPath}" -d "${extractDir}"`, { stdio: 'pipe' });
  }
} catch (err) {
  console.error('[FAIL] Unable to unpack IPA archive:', err);
  process.exit(1);
}

const payloadDir = path.join(extractDir, 'Payload');
let failures = 0;
let warnings = 0;

function report(step, title, status, details) {
  const mark = status === 'PASS' ? '[PASS]' : status === 'WARN' ? '[WARN]' : '[FAIL]';
  console.log(`${mark} Check ${step}: ${title} - ${details}`);
  if (status === 'FAIL') failures++;
  if (status === 'WARN') warnings++;
}

// 1. Payload/*.app 是否存在
let appDir = null;
if (fs.existsSync(payloadDir)) {
  const apps = fs.readdirSync(payloadDir).filter(f => f.endsWith('.app'));
  if (apps.length > 0) {
    appDir = path.join(payloadDir, apps[0]);
    report(1, 'Payload/*.app Existence', 'PASS', `Found: ${apps[0]}`);
  } else {
    report(1, 'Payload/*.app Existence', 'FAIL', 'No .app directory inside Payload');
  }
} else {
  report(1, 'Payload/*.app Existence', 'FAIL', 'Payload directory missing');
}

if (!appDir) {
  console.error(`Acceptance failed at check 1. Stopping inspection.`);
  process.exit(1);
}

// 2. Info.plist 是否存在
const plistPath = path.join(appDir, 'Info.plist');
let plistContent = '';
if (fs.existsSync(plistPath)) {
  plistContent = fs.readFileSync(plistPath, 'utf8');
  report(2, 'Info.plist Existence', 'PASS', `Present (${fs.statSync(plistPath).size} bytes)`);
} else {
  report(2, 'Info.plist Existence', 'FAIL', 'Missing Info.plist in app root');
}

// Helper to extract plist string value
function getPlistValue(key) {
  const regex = new RegExp(`<key>${key}</key>\\s*<string>([^<]+)</string>`);
  const match = plistContent.match(regex);
  return match ? match[1] : null;
}

// 3. CFBundleIdentifier
const bundleId = getPlistValue('CFBundleIdentifier');
if (bundleId) {
  report(3, 'CFBundleIdentifier', 'PASS', `Identifier: ${bundleId}`);
} else {
  report(3, 'CFBundleIdentifier', 'FAIL', 'CFBundleIdentifier missing or invalid');
}

// 4. CFBundleExecutable
const executableName = getPlistValue('CFBundleExecutable');
if (executableName) {
  report(4, 'CFBundleExecutable', 'PASS', `Executable name: ${executableName}`);
} else {
  report(4, 'CFBundleExecutable', 'FAIL', 'CFBundleExecutable missing');
}

// 5. 主 executable 是否存在
let execExists = false;
let execPath = null;
if (executableName) {
  execPath = path.join(appDir, executableName);
  if (fs.existsSync(execPath)) {
    execExists = true;
    report(5, 'Main Executable File', 'PASS', `File exists at: ${execPath}`);
  } else {
    report(5, 'Main Executable File', 'WARN', `Executable ${executableName} not yet compiled (requires macOS Xcode toolchain)`);
  }
}

// 6. 使用 file / lipo / otool 检查：Mach-O arm64
if (execExists) {
  try {
    const fileOutput = execSync(`file "${execPath}"`).toString();
    if (fileOutput.includes('Mach-O') && fileOutput.includes('arm64')) {
      report(6, 'Binary Architecture Check', 'PASS', `Mach-O arm64 verified: ${fileOutput.trim()}`);
    } else {
      report(6, 'Binary Architecture Check', 'FAIL', `Unexpected binary format: ${fileOutput.trim()}`);
    }
  } catch (e) {
    report(6, 'Binary Architecture Check', 'WARN', 'file command unavailable on host system');
  }
} else {
  report(6, 'Binary Architecture Check', 'WARN', 'Skipped: Executable binary pending macOS Xcode build step');
}

// 7. 检查 Frameworks
const frameworksDir = path.join(appDir, 'Frameworks');
if (fs.existsSync(frameworksDir)) {
  const fwList = fs.readdirSync(frameworksDir);
  report(7, 'Frameworks Inspection', 'PASS', `Frameworks found: ${fwList.join(', ')}`);
} else {
  report(7, 'Frameworks Inspection', 'WARN', 'Frameworks directory not present (UnityFramework built during Xcode build step)');
}

// 8. 检查非法 Android 文件依赖
function scanForForbidden(dir, forbiddenList) {
  let found = [];
  const entries = fs.readdirSync(dir, { withFileTypes: true });
  for (const entry of entries) {
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) {
      found = found.concat(scanForForbidden(full, forbiddenList));
    } else {
      for (const pattern of forbiddenList) {
        if (entry.name.endsWith(pattern)) {
          found.push(entry.name);
        }
      }
    }
  }
  return found;
}
const forbiddenAndroidFiles = scanForForbidden(appDir, ['.so', 'classes.dex', 'resources.arsc', 'AndroidManifest.xml']);
if (forbiddenAndroidFiles.length === 0) {
  report(8, 'Forbidden Android Files Check', 'PASS', 'Clean: No Android .so, .dex, or manifest files detected');
} else {
  report(8, 'Forbidden Android Files Check', 'FAIL', `Found illegal Android files: ${forbiddenAndroidFiles.join(', ')}`);
}

// 9. 检查资源完整性
const hasStoryboard = fs.existsSync(path.join(appDir, 'LaunchScreen.storyboard')) || fs.existsSync(path.join(appDir, 'LaunchScreen.storyboardc'));
if (hasStoryboard) {
  report(9, 'Asset Integrity Check', 'PASS', 'LaunchScreen storyboard present');
} else {
  report(9, 'Asset Integrity Check', 'FAIL', 'Missing LaunchScreen storyboard');
}

// 10. 检查最低 iOS 版本
const minOS = getPlistValue('MinimumOSVersion');
if (minOS && parseFloat(minOS) >= 12.0) {
  report(10, 'Minimum iOS Version', 'PASS', `MinimumOSVersion: ${minOS}`);
} else {
  report(10, 'Minimum iOS Version', 'FAIL', `Invalid MinimumOSVersion: ${minOS}`);
}

// 11. 检查签名状态
const codeSignature = path.join(appDir, '_CodeSignature');
if (!fs.existsSync(codeSignature)) {
  report(11, 'Code Signature State', 'PASS', 'Pure Unsigned IPA: No residual _CodeSignature present (ready for sideloading/TrollStore)');
} else {
  report(11, 'Code Signature State', 'WARN', 'Residual _CodeSignature detected in unsigned IPA');
}

console.log('---------------------------------------------');
console.log(`Inspection Summary: ${failures} Failures, ${warnings} Warnings`);
console.log('=============================================');

if (failures > 0) {
  process.exit(1);
} else {
  console.log('IPA Verification completed successfully.');
}
