@echo off
setlocal enableextensions
pushd

:: Validate directory
if "%~1"=="" goto Usage

:: Validate command
if "%~2"=="" goto Usage
if /I "%~2"=="pull" goto Directory
if not "%~2"=="clone" goto Usage
if not "%~3"=="" goto Clone

:: Display usage
:Usage
echo Runs Git commands on GNext repo/branches
echo Usage: RunGit.cmd directory command [arguments]
echo        directory - directory from which to execute command 
echo        command   - git command to execute
echo        arguments - arguments for git command
echo.
echo Command   clone
echo Arguments repo-to-clone target-directory [branch-to-checkout]]
echo.
echo Command   pull
echo Arguments --rebase
echo.
echo Examples:
echo   RunGit C:\HPE\Dev\ROMS\G13 clone main G13/main
echo   RunGit C:\HPE\DEV\ROMS\G12\master pull --rebase
echo.
set CODE=-1
goto Exit

:: Make sure directory exists
:Clone
if exist "%~1" goto Exists
set MESSAGE=Invalid directory: %~1
set CODE=1
echo mkdir "%~1"
mkdir "%~1"
if ERRORLEVEL 1 goto Done

:: Execute the clone command
:Exists
set MESSAGE=Clone failed!
set CODE=2
call %~d0Patience
pushd "%~1"
echo git clone git@github.hpe.com:HPE-ROM-TEAM/GNext.git "%~3"
git clone git@github.hpe.com:HPE-ROM-TEAM/GNext.git "%~3" 2>&1
popd
if ERRORLEVEL 1 goto Done

:: See if checkout is needed
set CODE=0
if "%~4"=="" goto Exit

:: Handle checkout
set MESSAGE=Checkout failed!
set CODE=3
pushd "%~1\%~3"
echo git checkout %~4
git checkout %~4
popd
if ERRORLEVEL 1 goto Done
set CODE=0
goto Exit

:: Make sure the directory exists
:Directory
if exist "%~1" goto Pull
echo %1 is not a valid existing directory 1>&2
goto Usage

:: Execute the pull command (in the indecated directory
:Pull
set MESSAGE=Pull failed!
set CODE=4
call %~d0Patience
pushd "%~1"
echo git pull %~3
git pull %~3 2>&1
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
