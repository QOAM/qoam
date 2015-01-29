.nuget\nuget.exe restore
msbuild QOAM.sln /t:Rebuild /p:Configuration=Release /p:OutDir="%~dp0\build" /p:GenerateProjectSpecificOutputFolder=True
msbuild build-scripts\Tests.msbuild