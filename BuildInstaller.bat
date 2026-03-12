@echo off
echo Building TaskRx application...
dotnet publish -c Release -r win-x64 --self-contained true

echo.
echo Creating installer...
echo Please make sure Inno Setup is installed and iscc.exe is in your PATH

REM Create Release directory if it doesn't exist
if not exist Release mkdir Release

REM Update the path to your Inno Setup Compiler if needed
"%LOCALAPPDATA%\Programs\Inno Setup 6\iscc" TaskRxSetup.iss

echo.
echo Done! The installer should be created in the Release directory as TaskRxSetup{#MyAppVersion}.exe