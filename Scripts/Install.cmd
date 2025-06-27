::@echo off
setlocal

:: Make sure necessary items are defined
if not defined NAME    goto Usage
if not defined EXE     goto Usage
if not defined PROGRAM goto Usage
goto CheckExe

:Usage
echo Generic Install script
echo Upon execution the following environment variables are used:
echo NAME:        Name of the item being installed (Human readable)
echo EXE:         Path to installed executable (if already installed)
echo              (this is expected to be surrounded by quotes to allow spaces in the path)
echo URL:         URL from which to download the installer (does not NAME)
echo              (set this to nothing if it has already been copied to the download directory)
echo              (this is expected to be surrounded by quotes to avoid consequences of special characters)
echo PROGRAM:     Name of the installer executable
echo              (DO NOT surround this in quotes)
echo LICENSE:     Name of license key file (in same directory as this script)
echo              (set this to nothing if no license file is to be used)
echo              (DO NOT surround this in quotes)
echo OPTIONS:     Options to use when running the installer
echo              (set this to nothing if no options are needed)
echo ALTINSTALL   If set this will be run instead of the installer
exit /b -1

:: See if the executable already exists
:CheckExe
if not exist %EXE% goto SetDestination
echo %NAME% already installed!
exit /b 0

:: The downloads directory seems an appropriate destination
:SetDestination
set DESTINATION=%USERPROFILE%\Downloads

:: Download the installer
if not defined URL goto CheckLicense
call "%~dp0Patience.cmd"
echo Downloading %NAME% ...
if exist "%DESTINATION%\%PROGRAM%" del "%DESTINATION%\%PROGRAM%" > NUL
curl -o "%DESTINATION%\%PROGRAM%" %URL% 2>&1
if ERRORLEVEL 1 exit /b 1

:: Check for need to copy License Key file
:CheckLicense
if not defined LICENSE goto RunInstaller
echo Copying License Key file ...
if exist "%DESTINATION%\%LICENSE%" del "%DESTINATION%\%LICENSE%" > NUL
echo copy "%~dp0%LICENSE%" "%DESTINATION%"
copy "%~dp0\%LICENSE%" "%DESTINATION%" > NUL
if ERRORLEVEL 1 exit /b 2

:: Run installer
:RunInstaller
echo Installing %NAME% ...
if defined ALTINSTALL goto AltInstall
pushd "%DESTINATION%"
echo %PROGRAM% %OPTIONS%
%PROGRAM% %OPTIONS%
popd
if ERRORLEVEL 1 exit /b 3
exit /b 0

:AltInstall
echo %ALTINSTALL%
%ALTINSTALL%
if ERRORLEVEL 1 exit /b 4
exit /b 0
