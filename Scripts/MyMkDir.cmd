@echo off
setlocal enableextensions

if exist "%~1\" goto Done
echo mkdir "%~1"
mkdir "%~1"
:Done
exit /b %ERRORLEVEL%