@echo off
setlocal

set NAME=010 Binary Editor V3.0.6
set EXE=C:\Program Files (x86)\010 Editor v3\010Editor.exe
set URL=
set PROGRAM=010EditorInstaller306.exe
set LICENSE=010license.txt
set OPTIONS=/VERYSILENT
set DESTINATION=%~dp0..\Data

call "%~dp0Install.cmd"
endlocal && exit /b %ERRORLEVEL%