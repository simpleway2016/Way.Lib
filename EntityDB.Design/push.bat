rem nuget pack EntityDB.Design.csproj -Prop Configuration=Release

nuget pack EntityDB.Design.csproj
nuget push EntityDB.Design.1.0.0.8.nupkg -Source https://www.nuget.org/api/v2/package
pause