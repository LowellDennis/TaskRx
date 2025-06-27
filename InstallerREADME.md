# TaskRx Installer Setup

This document provides instructions for building and using the TaskRx installer.

## Prerequisites

1. **Inno Setup**: Download and install from [innosetup.com](https://jrsoftware.org/isdl.php)
2. **.NET SDK**: Make sure you have the .NET SDK installed to build the application

## Building the Installer

1. First, build the TaskRx application using the .NET SDK:
   ```
   dotnet publish -c Release
   ```

2. Run the Inno Setup Compiler on the script:
   ```
   iscc TaskRxSetup.iss
   ```
   
   Alternatively, you can use the provided batch file:
   ```
   BuildInstaller.bat
   ```

3. The installer will be created in the project root directory as `TaskRxSetup.exe`

## Installer Contents

The installer will:
1. Install the TaskRx.exe and all required DLLs
2. Create a Scripts subfolder with all script files
3. Create start menu shortcuts
4. Optionally create a desktop shortcut

## Customizing the Installer

To customize the installer, edit the `TaskRxSetup.iss` file:

- Change the application name, version, or publisher
- Modify file inclusions or exclusions
- Add additional installation steps
- Change the installation directory

## Troubleshooting

- If the build fails, make sure the paths in the .iss file match your actual build output paths
- The default configuration assumes a build output in `bin\Release\net8.0-windows`
- If your build output is in a different location, update the Source paths in the [Files] section