rem nuget pack Way.EntityDB.xproj -Prop Configuration=Release


dotnet pack
rem nuget push Way.EntityDB.1.0.0.1.nupkg -Source https://www.nuget.org/api/v2/package
pause