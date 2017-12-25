%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\installutil.exe %~dp0Way.WindowsService.exe
Net Start SunRiz.Server3
sc config SunRiz.Server3 start=auto
pause