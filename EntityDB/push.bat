nuget pack EntityDB.csproj
nuget push EntityDB.1.0.0.2.nupkg -Source https://www.nuget.org/api/v2/package
del EntityDB.1.0.0.2.nupkg
pause