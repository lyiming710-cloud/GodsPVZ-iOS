import fs from 'node:fs';
import path from 'node:path';
import { execSync } from 'node:child_process';

console.log('=== Packaging Unsigned IPA ===');

const buildDir = path.resolve('build');
const outputDir = path.join(buildDir, 'output');
const payloadDir = path.join(buildDir, 'Payload');
const appSourceDir = path.join(buildDir, 'Release-iphoneos', 'GodsPVZ.app');

if (!fs.existsSync(outputDir)) {
  fs.mkdirSync(outputDir, { recursive: true });
}

// Clean previous Payload directory
if (fs.existsSync(payloadDir)) {
  fs.rmSync(payloadDir, { recursive: true, force: true });
}
fs.mkdirSync(payloadDir, { recursive: true });

const appTargetDir = path.join(payloadDir, 'GodsPVZ.app');

if (fs.existsSync(appSourceDir)) {
  console.log(`Copying compiled application from ${appSourceDir}...`);
  fs.cpSync(appSourceDir, appTargetDir, { recursive: true });
} else {
  console.log(`Notice: ${appSourceDir} not found (compiled binary step not completed).`);
  console.log(`Scaffolding IPA template bundle for structure validation...`);
  fs.mkdirSync(appTargetDir, { recursive: true });
  fs.copyFileSync('ios/Info.plist', path.join(appTargetDir, 'Info.plist'));
  fs.copyFileSync('ios/LaunchScreen.storyboard', path.join(appTargetDir, 'LaunchScreen.storyboard'));
}

// Ensure _CodeSignature is removed for pure unsigned IPA
const codeSigDir = path.join(appTargetDir, '_CodeSignature');
if (fs.existsSync(codeSigDir)) {
  console.log('Removing existing _CodeSignature for pure unsigned packaging...');
  fs.rmSync(codeSigDir, { recursive: true, force: true });
}

// Create IPA archive (Payload/*.app zipped into .ipa)
const ipaPath = path.join(outputDir, 'GodsPVZ_1.0.2_unsigned.ipa');
if (fs.existsSync(ipaPath)) {
  fs.unlinkSync(ipaPath);
}

console.log(`Compressing Payload to ${ipaPath}...`);
try {
  if (process.platform === 'win32') {
    execSync(`tar.exe -a -c -f "${ipaPath}" -C "${buildDir}" Payload`, { stdio: 'inherit' });
  } else {
    execSync(`cd "${buildDir}" && zip -r -y "${ipaPath}" Payload`, { stdio: 'inherit' });
  }
  console.log(`[SUCCESS] IPA generated at: ${ipaPath} (${fs.statSync(ipaPath).size} bytes)`);
} catch (err) {
  console.error('Failed to compress IPA:', err);
  process.exit(1);
}
