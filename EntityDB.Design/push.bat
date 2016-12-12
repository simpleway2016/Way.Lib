nuget pack EntityDB.Design.csproj -Prop Configuration=Release
nuget push EntityDB.Design.1.0.0.7.nupkg -Source https://www.nuget.org/api/v2/package
pause