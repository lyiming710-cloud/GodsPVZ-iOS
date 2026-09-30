from pathlib import Path
import subprocess,json,shutil
p=Path('/workspaces/GodsPVZ-native19/.validation/native23-independent-full-review');proj=p/'runtime-fixture';proj.mkdir(exist_ok=True)
source='__RUNTIME_SOURCE__'
(proj/'Program.cs').write_text(source)
(proj/'Runtime.csproj').write_text('<Project Sdk="Microsoft.NET.Sdk"><PropertyGroup><OutputType>Exe</OutputType><TargetFramework>net10.0</TargetFramework></PropertyGroup><ItemGroup><Reference Include="Mono.Cecil" HintPath="/workspaces/GodsPVZ-native19/Tools/Stage9Native4Recovery/tools/lib/netstandard2.0/Mono.Cecil.dll" /></ItemGroup></Project>')
with (p/'runtime-fixture.log').open('w') as f:
 rc=subprocess.run(['dotnet','run','--project',str(proj/'Runtime.csproj'),'-c','Release','--',str(p/'batch1.dll'),str(p/'runtime-fixture-results.json')],stdout=f,stderr=subprocess.STDOUT).returncode
print(json.dumps({'exit':rc,'log':(p/'runtime-fixture.log').read_text()[-4500:],'result':json.loads((p/'runtime-fixture-results.json').read_text()) if (p/'runtime-fixture-results.json').exists() else None},indent=2))
