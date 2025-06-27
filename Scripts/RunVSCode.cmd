@echo off
setlocal

:: Make sure executable exists
set VSCODE="%USERPROFILE%\AppData\Local\Programs\Microsoft VS Code\bin\code.cmd"
if exist %VSCODE% goto Exists

:: Could not find VSCode!
echo Unable to locate Visual Studio Code 2>1
set CODE=1
goto :Exit

:: Execute VSCode with parameters passed in
:Exists
%VSCODE% %*
set CODE=%ERRORLEVEL%
:Exit
endlocal && exit /b %CODE%
