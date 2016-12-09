nuget pack AppLib.csproj
nuget push AppLib.1.0.0.1.nupkg -Source https://www.nuget.org/api/v2/package
del AppLib.1.0.0.1.nupkg
pause