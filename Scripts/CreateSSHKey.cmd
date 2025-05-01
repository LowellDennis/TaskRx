@echo off
setlocal

:: Handle usage invocations
if "%~1"=="" goto Usage
if not "%~2"=="" goto Good

:: Display usage
:Usage
echo Creates an SSH Public/Private Key Pair
echo Usage: CreateSSHKey.cmd Work^|Personal Email
echo        Work     - create id_ed25519          and id_ed25519.pub
echo        Personal - create id_ed25519_personal and id_ed25519_personal.pub
echo        Email    - Email to be associated with the SSH Key pair
echo.
echo Examples:
echo   CreateSSHKey Work John.Doe@HPE.com
echo   CreateSSHKey Personal John.Doe@email.com
echo.
set CODE=-1
goto Exit

:: Assume this is for work
:Good
set WHICH=Work
set SUFFIX=
set GITHUB=github.hpe.com
if /I "%1"=="Work" goto CheckExists

:: Setup for personal
set WHICH=Personal
set SUFFIX=_personal
set GITHUB=github.com

:: Check to see if targeted SSH key already exists
:CheckExists
set CODE=0
set MESSAGE=%WHICH% SSH key already exists.
if exist "%USERPROFILE%\.ssh\id_ed25519%SUFFIX%" goto Done

:: Attempt to create PPK file
echo Creating new %WHICH% SSH key ...
set CODE=1
set MESSAGE=Error creating %WHICH% SSh key.
ssh-keygen -N "" -t ed25519 -f "%USERPROFILE%\.ssh\id_ed25519%SUFFIX%" -C "%2"
if ERRORLEVEL 1 goto Done

:: Copy public key to clipboard
set CODE=0
set MESSAGE=%WHICH% SSH key successfully created!
type "%USERPROFILE%\.ssh\id_ed25519%SUFFIX%".pub | clip

:: Prompt user to paste key into their GitHub account
echo Waiting for user to perform 'Next Steps' ... 1>&2
start https://%GITHUB%/settings/keys
powershell  "Add-Type -AssemblyName PresentationFramework;[System.Windows.MessageBox]::Show(\"SSH key copied to clipboard.`nGitHub opened in default browser.`n`n1. Complete GitHub login (if needed)`n2. Click 'New SSH Key'`n3. Type appropriate name in 'Title' text box`n4. Paste  key into 'Key' text box`n5. Click 'Add SSH key' to save.\", \"Next Steps\")"

:: Show completion message and exit with appropriate return code
:Done
echo %MESSAGE%
:Exit
endlocal && exit /b %CODE%
