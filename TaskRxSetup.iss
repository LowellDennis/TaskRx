#define MyAppName "TaskRx"
#define MyAppVersion "1.0"
#define MyAppPublisher "HPE
#define MyAppExeName "TaskRx.exe"

[Setup]
; Basic setup information
AppId={{A1B2C3D4-E5F6-4A5B-9C8D-7E6F5A4B3C2D}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
; Uncomment the following line to run in non administrative install mode (install for current user only.)
;PrivilegesRequired=lowest
OutputDir=.
OutputBaseFilename=TaskRxSetup
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
; Main executable and DLLs - adjust the source path as needed
Source: "bin\Release\net8.0-windows\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\net8.0-windows\*.dll"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs
Source: "bin\Release\net8.0-windows\*.json"; DestDir: "{app}"; Flags: ignoreversion
; Scripts folder
Source: "Scripts\*"; DestDir: "{app}\Scripts"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent