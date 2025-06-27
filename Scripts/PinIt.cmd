@echo off
setlocal

:: Validate arguments
if "%~1"=="" goto Usage
goto Good

:: Display usage
:Usage
echo Pins an item to the taskbar 1>&2
echo Usage: PinIt.cmd <item> 1>&2
echo        item - path of item to be pinned 1>&2
echo. 1>&2
set CODE=-1
goto Exit

:: See if item exists
:Good
set CODE=1
set MESSAGE=%1 not found
if not exist "%~1" goto Error

:: Pint item to the taskbar
:Exists
set CODE=1
set MESSAGE=Error pinning %1 tot taskbar
syspin.exe "%~1" 5386
if ERRORLEVEL 1 goto Error
set CODE=0
echo %~1 pinned to taskbar!
goto :Exit

:Error
echo %MESSAGE% 1>&2
:Exit
endlocal && exit /b %CODE%
