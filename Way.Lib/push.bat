rem nuget pack Jack.Sms.csproj -Prop Configuration=Release
rem dotnet pack --configuration Release
nuget push bin\Release\Way.Lib.2.0.1.2.nupkg -Source https://www.nuget.org/api/v2/package
pause