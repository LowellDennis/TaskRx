@echo off
setlocal enableextensions
pushd

:: Validate directory
if "%~1"=="" goto Usage

:: Validate generation
if "%~2"=="" goto Usage
if /I "%~2"=="gnext" goto Command
if /I "%~2"=="g9" goto Command
if /I "%~2"=="g8" goto Command
echo Invalid generation: %~2
goto Usage

:: Validate command
:Command
set URL=https://romsvn.americas.hpqcorp.net:8443/svn/ROM/UEFI/trunk/%~2
if "%~3"=="" goto Usage
if /I "%~3"=="checkout" goto Target 
if /I "%~3"=="update" goto Directory
echo Invalid command: %~3
goto Usage

:: Validate target-directory
:Target
if not "%~4"=="" goto Checkout

:: Display usage
:Usage
echo Runs SVN commands on GNext repo
echo Usage: RunSvn.cmd directory generation command [arguments]
echo        directory  - directory from which to execute command
echo        generation - generation of GNext code on which to operate
echo        command    - svn command to execute
echo        arguments  - arguments for command
echo.
echo Command   checkout
echo Arguments target-directory
echo.
echo Command   update
echo Arguments -none-
echo.
echo Examples:
echo   RunSvn C:\HPE\Dev\ROMS\G10 G10 checkout trunk
echo   RunSvn C:\HPE\DEV\ROMS\G9\trunk G9 update
echo.
set CODE=-1
goto Exit

:: Make sure directory exists
:Checkout
if exist "%~1" goto exists
set MESSAGE=Invalid directory: %~1
set CODE=1
echo mkdir "%~1"
mkdir "%~1"
if ERRORLEVEL 1 goto Done

:: Execute the checkout command
:Exists
set MESSAGE=Checkout failed!
set CODE=2
call %~d0Patience
pushd "%~1"
echo svn checkout %URL% "%~4"
svn checkout %URL% "%~4"
popd
if ERRORLEVEL 1 goto Done
set CODE=0
goto Exit

:: Make sure the directory exists
:Directory
if exist "%~1" goto Update
echo %1 is not a valid existing directory 1>&2
goto Usage

:: Execute the update command (in the indecated directory
:Update
set MESSAGE=Update failed!
set CODE=4
call %~d0Patience
pushd "%~1"
echo svn update
svn update
popd
if ERRORLEVEL 1 goto Done
set CODE=0
goto Exit

:: Show completion message and exit with appropriate return code
:Done
echo %MESSAGE%
:Exit
popd
endlocal && exit /b %CODE%
