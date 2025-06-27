::@echo off
setlocal

set NAME=Beyond Compare V4.4.7
set EXE="C:\Program Files\Beyond Compare 4\BCompare.exe"
set PROGRAM=BCompare-4.4.7.28397.exe
set URL="https://www.scootersoftware.com/files/%PROGRAM%"
set LICENSE=BC4Key.txt
set OPTIONS=/SP- /VERYSILENT /NORESTART
set INVOKE=

call "%~dp0Install.cmd"
endlocal && exit /b %ERRORLEVEL%
