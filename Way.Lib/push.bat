
dotnet pack --configuration Release
nuget push bin\Release\Way.Lib.1.0.0.5.nupkg -Source https://www.nuget.org/api/v2/package
pause