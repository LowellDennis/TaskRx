@echo off
setlocal

:: Handle usage invocations
if "%~1"=="" goto Usage
if not "%~2"=="" goto Good

:: Display usage
:Usage
echo Install or update software packages using winget
echo Usage: RunWinget.cmd install^|upgrade WinGetPackageId ["Custom Options"]
echo.
echo Examples:
echo   RunWinget.cmd install PuTTY.PuTTY
echo   RunWinget.cmd upgrade Microsoft.VisualStudioCode "-Argument \"/VERYSILENT /MERGETASKS=!runcode\""
echo.
echo Note: Custom options should be enclosed in quotes if they contain spaces
echo.
set CODE=-1

:: Indicate what is going on
:Good
set OPERATION=Installing
if /I "%~1"=="update" set OPERATION=Updating
echo %OPERATION% package %~2 ...

call "%~dp0Patience.cmd"

:: Handle install/update with custom options
set CUSTOM=
if not "%~3"=="" set CUSTOM=-custom "%~3"
echo winget %~1 %~2 --source winget --silent --accept-source-agreements --accept-package-agreements --disable-interactivity %CUSTOM%
winget %~1 %~2 --source winget --silent --accept-source-agreements --accept-package-agreements --disable-interactivity %CUSTOM%
set CODE=%ERRORLEVEL%

:: Co-opt Winget's no update available exit code
if "%CODE%"=="-1978335189" set CODE=0

:Exit
endlocal && exit /b %CODE%
