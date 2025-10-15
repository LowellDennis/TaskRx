@echo off
setlocal enableextensions

:: Validate arguments
if "%~1"=="" goto Usage
if "%~2"=="" goto Usage
if "%~3"=="" goto Usage
goto Good

:: Display usage
:Usage
echo Configures git global settingsRuns Git commands on GNext repo/branches
echo Usage: GitGlobal.cmd first last email
echo        first - first name to be associated with git account
echo        last  - last  name to be associated with git account
echo        email - email      to be associated with git account
echo.
echo Examples:
echo   GitGlobal John Doe john.doe@hpe.com
echo.
set ERRORLEVEL=-1
goto Exit

:: See if .gitignore file exists
:Good
if not exist "%USERPROFILE%\.gitignore" goto GitIgnore
echo %USERPROFILE%\.gitignore already exists
goto Continue

:: Copy default .gitignore
:GitIgnore
echo copy "%~dp0.gitignore" "%USERPROFILE%"
copy "%~dp0.gitignore" "%USERPROFILE%"
if ERRORLEVEL 1 goto Exit

:: Set Git to use given first and last name, email, and .gitignore file
:: NOTE: No need to check if .gitconfig exists
::       This will create it if it does not.
::       This will update it if it does.
:Continue
echo git config --global user.name "%~1 %~2"
git config --global user.name "%~1 %~2"
if ERRORLEVEL 1 goto Exit
echo git config --global user.email "%~3"
git config --global user.email "%~3"
if ERRORLEVEL 1 goto Exit
echo git config --global core.excludefiles "%USERPROFILE%\.gitignore"
git config --global core.excludefiles "%USERPROFILE%\.gitignore"
if ERRORLEVEL 1 goto Exit

:: Set the default merge of master branch to "--squash"
git config branch.master.mergeOptions "--squash"

:: Enable long paths
git config --system core.longpaths true

:Exit
endlocal && exit /b %ERRORLEVEL%
