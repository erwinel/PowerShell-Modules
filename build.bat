@ECHO OFF
SET BatchPath=%~dp0
cd "%BatchPath%"
powershell -STA -ExecutionPolicy Bypass -File Build.ps1 %1 %2 %3 %4 %5 %6 %7 %8 %9
pause