@echo off
setlocal

:: See if git is installed
git --version > NUL 2>&1
if ERRORLEVEL 1 goto done

:: Git is installed
set BCPATH="C:/Program Files/Beyond Compare 4/BComp.exe"

:: Use for git diff tool
git config --global diff.tool bc
git config --global difftool.bc.path %BCPATH%
git config --global difftool.bc.trustExitCode true
git config --global difftool.prompt false

:: Use for git merge tool
git config --global merge.tool bc
git config --global mergetool.bc.path %BCPATH%
git config --global mergetool.bc.trustExitCode true
git config --global mergetool.keepBackup false

:Done
exit /b 0
