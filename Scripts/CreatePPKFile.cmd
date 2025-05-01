@echo off
setlocal

:: Handle usage invocations
if not "%~1"=="" goto Good

:: Display usage
:Usage
echo Creates a Putty Private Key file from an existing SSH Private Key file
echo Usage: CreatePPKFile.cmd Work^|Personal
echo        Work     - Use id_ed25519          to create id_ed25519.ppk
echo        Personal - Use id_ed25519_personal to create id_ed25519_personal
echo.
echo Examples:
echo   CreatePPKFile Work
echo   CreatePPKFile Personal
echo.
set CODE=-1
goto Exit

:: Assume this is for work
:Good
set WHICH=Work
set SUFFIX=
if "%1"=="Work" goto CheckExists

:: Setup for personal
set WHICH=Personal
set SUFFIX=_personal

:: Make sure Private Key file exists
:CheckExists
set CODE=1
set MESSAGE=%WHICH% Private Key not found.
if not exist "%USERPROFILE%\.ssh\id_ed25519%SUFFIX%" goto Done

:: Check to see if targeted PPK file already exists
set CODE=0
set MESSAGE=%WHICH% PPK file already exists.
if exist "%USERPROFILE%\.ssh\id_ed25519%SUFFIX%.ppk" goto Done

:: Create PPK file
echo Creating new %WHICH% PPK file ...
set CODE=2
set MESSAGE=Error creating %WHICH% PPK file.
powershell.exe -ExecutionPolicy Bypass -File RunPuTTYGen.ps1 "%USERPROFILE%\.ssh\id_ed25519%SUFFIX%" "%USERPROFILE%\.ssh\id_ed25519%SUFFIX%.ppk"
if ERRORLEVEL 1 goto Done

:: PPK file was created
set CODE=0
set MESSAGE=%WHICH% PPK file successfully created!
goto Done

:: Show completion message and exit with appropriate return code
:Done
echo %MESSAGE%
:Exit
endlocal && exit /b %CODE%
