@echo off
echo Building TaskRx application...
dotnet publish -c Release

echo.
echo Creating installer...
echo Please make sure Inno Setup is installed and iscc.exe is in your PATH

REM Update the path to your Inno Setup Compiler if needed
iscc TaskRxSetup.iss

echo.
echo Done! The installer should be created in the current directory as TaskRxSetup.exe
pause