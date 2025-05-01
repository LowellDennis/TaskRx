@echo off
setlocal

:: Handle usage invocations
if "%~1"=="" goto Usage
if not "%~2"=="" goto Good

:: Display usage
:Usage
echo Creates an SSH Config File
echo Usage: CreateSSHConfig Work_Email Personal_Email
echo.
echo Example:
echo   CreateSSHConfig John.Doe@HPE.com John.Doe@email.com
echo.
set CODE=-1
goto Exit

:: Check to see if SSH config file already exists
:Good
set CONFIG=%USERPROFILE%\.ssh\config
set CODE=0
set MESSAGE=SSH Config file already exists.
if exist "%CONFIG%" goto Done

:: Create SSH config File
echo Creating SSH Config file ...
set CODE=1
set MESSAGE=Error creating SSH Config file.

echo # Work GitHub Account>%CONFIG%
if ERRORLEVEL 1 goto Done
echo Host github.hpe.com>>%CONFIG%
if ERRORLEVEL 1 goto Done
echo  HostName github.hpe.com>>%CONFIG%
if ERRORLEVEL 1 goto Done
echo  IdentityFile ~/.ssh/id_ed25519>>%CONFIG%
if ERRORLEVEL 1 goto Done
echo  User %~1>>%CONFIG%
if ERRORLEVEL 1 goto Done
echo  PreferredAuthentications publickey>>%CONFIG%
if ERRORLEVEL 1 goto Done
echo  TCPKeepAlive yes>>%CONFIG%
if ERRORLEVEL 1 goto Done
echo  IdentitiesOnly yes>>%CONFIG%
if ERRORLEVEL 1 goto Done
echo.>>%CONFIG%
if ERRORLEVEL 1 goto Done
echo # Personal Github Account>>%CONFIG%
if ERRORLEVEL 1 goto Done
echo Host github.com>>%CONFIG%
if ERRORLEVEL 1 goto Done
echo  HostName github.com>>%CONFIG%
if ERRORLEVEL 1 goto Done
echo  IdentityFile ~/.ssh/id_ed25519_personal>>%CONFIG%
if ERRORLEVEL 1 goto Done
echo  User %~2>>%CONFIG%
if ERRORLEVEL 1 goto Done
echo  PreferredAuthentications publickey>>%CONFIG%
if ERRORLEVEL 1 goto Done
echo  TCPKeepAlive yes>>%CONFIG%
if ERRORLEVEL 1 goto Done
echo  IdentitiesOnly yes>>%CONFIG%
if ERRORLEVEL 1 goto Done
set CODE=0
set MESSAGE=SSH Config created!

:: Show completion message and exit with appropriate return code
:Done
echo %MESSAGE%
:Exit
endlocal && exit /b %CODE%
