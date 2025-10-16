@echo off
setlocal enableextensions
pushd

:: Validate directory
if "%~1"=="" goto Usage

:: Validate command
if "%~2"=="" goto Usage
if /I "%~2"=="clone" goto Command_Clone
if /I "%~2"=="worktree" goto Command_Worktree
if /I "%~2"=="pull" goto Command_Pull

:: Display usage
:Usage
echo Runs Git commands on GNext repo/branches
echo Usage: RunGit.cmd directory command [arguments]
echo        directory - directory from which to execute command 
echo        command   - git command to execute
echo        arguments - arguments for git command
echo.
echo Command   clone
echo Arguments repo-to-clone target-directory
echo.
echo Command   worktree
echo Arguments target-directory worktree=branch
echo.
echo Command   pull
echo Arguments --rebase
echo.
echo Examples:
echo   RunGit C:\HPE\DEV\ROMS clone GNext
echo   RunGit C:\HPE\DEV\ROMS\GNext worktree ..\G12 Gen12/main
echo   RunGit C:\HPE\DEV\ROMS\GNext pull --rebase
echo.
set CODE=-1
goto Exit

:: Make sure directory exists
:Command_Clone
if exist "%~1" goto Do_Clone
set MESSAGE=Invalid directory: %~1
set CODE=1
echo mkdir "%~1"
mkdir "%~1"
if ERRORLEVEL 1 goto Done

:: Execute the clone command
:Do_Clone
set MESSAGE=Clone failed!
set CODE=2
call "%~d0Patience"
pushd "%~1"
echo git clone git@github.hpe.com:HPE-ROM-TEAM/GNext.git "%~3"
git clone git@github.hpe.com:HPE-ROM-TEAM/GNext.git "%~3" 2>&1
popd
if ERRORLEVEL 1 goto Done
set CODE=0
goto Exit

:: Make sure directory exists
:Command_Worktree
if exist "%~1" goto Exist_Worktree
echo Invalid directory: %~1 1>&2
goto Usage

:: See if worktree already exists
:Exist_Worktree
set MESSAGE=Worktree already exists: %~3!
set CODE=3
if exist "%~3" goto Done

:: Create the worktree
set MESSAGE=Worktree creation failed!
set CODE=4
pushd "%~1"
echo git worktree add "%~3" "%~4"
git worktree add "%~3" "%~4" 2>&1
popd
if ERRORLEVEL 1 goto Done
set CODE=0
goto Exit

:: Make sure the directory exists
:Command_Pull
if exist "%~1" goto Do_Pull
echo Invalid directory: %~1 1>&2
goto Usage

:: Execute the pull command (in the indecated directory)
:Do_Pull
set MESSAGE=Pull failed!
set CODE=5
call "%~d0Patience"
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
