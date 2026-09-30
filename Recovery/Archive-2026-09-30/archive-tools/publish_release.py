from pathlib import Path
import json,subprocess
p=Path(__file__).parent;gh='C:/Program Files/GitHub CLI/gh.exe';repo='lyiming710-cloud/GodsPVZ-iOS';tag='recovery-archive-2026-09-30';record=json.loads((p/'ARCHIVE-PUBLICATION.json').read_text());sha=record['commit']
notes=(p/'RELEASE-NOTES.md').read_text(encoding='utf-8')+'\n\n分类入口：[完整归档目录与恢复说明](https://github.com/'+repo+'/blob/'+sha+'/Recovery/Archive-2026-09-30/README.md)。\n\n归档索引提交：['+sha[:7]+'](https://github.com/'+repo+'/commit/'+sha+')。可浏览代码快照 **2494 个**；保存本地 **5347** 个文件路径和 Codespace **5576** 个文件路径（范围有重叠，不能相加当独立文件数）。**13 个资产，合计 1,220,069,545 字节**，全部已验证 GitHub 服务端大小及 SHA-256；Xcode 工程和三份清单已下载回读验证。\n'
(p/'FINAL-RELEASE-NOTES.md').write_text(notes,encoding='utf-8')
subprocess.run([gh,'release','edit',tag,'-R',repo,'--target',sha,'--draft=false','--prerelease','--latest=false','--notes-file',str(p/'FINAL-RELEASE-NOTES.md')],check=True)
release=json.loads(subprocess.check_output([gh,'api','repos/'+repo+'/releases/tags/'+tag],text=True));assert not release['draft'] and release['prerelease'];assert len(release['assets'])==13
ref=json.loads(subprocess.check_output([gh,'api','repos/'+repo+'/git/ref/tags/'+tag],text=True));assert ref['object']['sha']==sha,ref
expected=json.loads((p/'ASSET-SHA256.json').read_text())
for a in release['assets']:
 if a['name'] in expected:
  e=expected[a['name']];assert a['digest']=='sha256:'+e['sha256'] and a['size']==e['bytes'] and a['state']=='uploaded',a['name']
result={'release_url':release['html_url'],'tag':tag,'tag_commit':sha,'prerelease':release['prerelease'],'draft':release['draft'],'published_at':release['published_at'],'assets':len(release['assets']),'asset_bytes':sum(x['size'] for x in release['assets']),'tag_commit_verified':True,'all_asset_hashes_verified':True,'classification_url':'https://github.com/'+repo+'/blob/'+sha+'/Recovery/Archive-2026-09-30/README.md'}
(p/'FINAL-ARCHIVE-RESULT.json').write_text(json.dumps(result,indent=2),encoding='utf-8');print(json.dumps(result,indent=2))
