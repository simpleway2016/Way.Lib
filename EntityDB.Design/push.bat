nuget pack EntityDB.Design.csproj
nuget push EntityDB.Design.1.0.0.6.nupkg -Source https://www.nuget.org/api/v2/package
del EntityDB.Design.1.0.0.6.nupkg
pause