@echo off
setlocal

set NAME=SlickEdit Pro 2017
set EXE="C:\Program Files\SlickEdit Pro 22.0.2\win\vs.exe"
set PROGRAM=se_22000201_win64.msi
set URL=
set LICENSE=
set OPTIONS=
set ALTINSTALL=%~dp0InstallSlickEdit.exe
set SOURCE=\\romqacorp.americas.hpqcorp.net\Shared Apps\Slick Edit\%PROGRAM%

:: Copy SLick Edit installer to downloads folder
set TARGET=%USERPROFILE%\Downloads\%PROGRAM%
if exist "%TARGET%" goto NoCopy
echo copy "%SOURCE%" "%TARGET%"
copy "%SOURCE%" "%TARGET%" > NUL
:NoCopy

call %~dp0Install.cmd
endlocal && exit /b %ERRORLEVEL%
