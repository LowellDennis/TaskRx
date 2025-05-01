@echo off
setlocal

set NAME=Beyond Compare V4.4.7
set EXE=C:\Program Files\Beyond Compare 4\BCompare.exe
set URL=
set PROGRAM=BCompare-4.4.7.28397.exe
set LICENSE=BC4Key.txt
set OPTIONS=/SP- /VERYSILENT /NORESTART
set DESTINATION=%~dp0..\Data

call %~dp0Install.cmd
endlocal && exit /b %ERRORLEVEL%
