@ECHO OFF
SET BatchPath=%~dp0
pushd
cd "%BatchPath%"
powershell -ExecutionPolicy Bypass -File Setup.ps1
popd
pause