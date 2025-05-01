@echo off
setlocal

:: Make sure necessary items are defined
if not defined NAME    goto Usage
if not defined EXE     goto Usage
if not defined PROGRAM goto Usage
goto Good

:Usage
echo Generic Install script
echo Upon execution the following environment variables are used:
echo NAME:        Name of the item being installed (Human readable)
echo EXE:         Path to installed executable (if already installed)
echo URL:         URL from which to download the installer (does not NAME)
echo              (set this to nothing if it has already been copied to the download directory)
echo PROGRAM:     Name of the installer executable
echo LICENSE:     Name of license key file (in same directory as this script)
echo              (set this to nothing if no license file is to be used)
echo OPTIONS:     Options to use when running the installer
echo              (set this to "" if no additional options are needed)
echo DESTINATION: Path where the executable is (or has been) to be downloaded
echo              (if not set this will default to %USERPROFILE%\Downloads)
exit /b -1

:Good
:: See if the executable already exists
if not exist "%EXE%" goto Download
echo %NAME% already installed!
exit /b 0

:: The downloads directory seems appropriate
:download
if defined DESTINATION goto DestinationSet

:: Set default DESTINATION
set DESTINATION=%USERPROFILE%\Downloads

:: Download the installer
:DestinationSet
if "%URL%"=="" goto NoUrl
call "%~dp0Patience.cmd"
echo Downloading %NAME% ...
if exist "%DESTINATION%\%PROGRAM%" del "%DESTINATION%\%PROGRAM%" > NUL
curl -o "%DESTINATION%\%PROGRAM%" %URL%/%PROGRAM% 2>&1
if ERRORLEVEL 1 exit /b 1
:NoUrl

:: Check for need to copy License Key file
if "%LICENSE%"=="" goto NoLicense
if not exist "%DESTINATION%\%LICENSE%" goto CopyLicense
echo License Key file already exists!
goto RunInstaller

:: Copy License File
:CopyLicense
echo Copying License Key file ...
echo copy "%~dp0%LICENSE%" "%DESTINATION%"
copy "%~dp0\%LICENSE%" "%DESTINATION%" > NUL
if ERRORLEVEL 1 exit /b 2

:: Run installer
:RunInstaller
echo Installing %NAME% ...
pushd "%DESTINATION%"
echo "%PROGRAM%" %OPTIONS%
"%PROGRAM%" %OPTIONS%
popd
if ERRORLEVEL 1 exit /b 3
exit /b 0
