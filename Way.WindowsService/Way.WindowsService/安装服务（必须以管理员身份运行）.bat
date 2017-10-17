%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\installutil.exe %~dp0\Way.WindowsService.exe
Net Start Way.WindowsService.Test
sc config Way.WindowsService.Test start=auto
pause