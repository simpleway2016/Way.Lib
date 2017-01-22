
dotnet pack --configuration Release
nuget push bin\Release\Way.EntityDB.1.0.0.2.nupkg -Source https://www.nuget.org/api/v2/package
pause