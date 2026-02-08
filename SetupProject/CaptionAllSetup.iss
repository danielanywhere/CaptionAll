#define MyAppName "CaptionAll"
#define MyAppVersion "26.2207.4649"

#define MyAppPublisher "Daniel Patterson, MCSD (danielanywhere)"
#define MyAppURL "https://powermake.systems"
#define MyAppExeName "CaptionAll.exe"
#define DevelopPath "C:\Files\Dropbox\Develop\Shared\" + MyAppName
#define ProjectPath "C:\Files\Dropbox\Develop\Shared\" + MyAppName
#define SourcePath DevelopPath + "\Source\" + MyAppName + "\bin\Release\net6.0-windows\win-x64"
#define IconPath DevelopPath + "\Source\" + MyAppName
#define IconFilename "CaptionAllIcon.ico"
#define OutputPath "C:\Files\Dropbox\Setups"

[Setup]
PrivilegesRequired=admin
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{9D012159-AA85-493A-B4AE-F7FDA3030DDD}
AppName={#MyAppName}
AppVersion={#MyAppVersion}


VersionInfoVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppCopyright="Copyright (c) 2026 {#MyAppPublisher}"
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={commonpf}\CaptionAll
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
LicenseFile="{#ProjectPath}\Docs\EULA2026.rtf"
OutputDir="{#OutputPath}"
OutputBaseFilename=CaptionAllSetup
Compression=lzma
SolidCompression=yes
SignedUninstaller=yes
SignedUninstallerDir="{#ProjectPath}\SetupProject"
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64
SetupIconFile="{#IconPath}\{#IconFilename}"
UninstallDisplayIcon="{app}\{#IconFilename}"

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"

[Files]
Source: "{#SourcePath}\CaptionAll.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\CaptionAll.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\CaptionAllIcon.ico"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\CaptionBubbleEditorWF.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\DocumentFormat.OpenXml.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\DocumentFormat.OpenXml.Framework.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\dotExcel.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\MediaPlayerWPFUC.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\Microsoft.CognitiveServices.Speech.core.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\Microsoft.CognitiveServices.Speech.csharp.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\Microsoft.CognitiveServices.Speech.extension.audio.sys.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\Microsoft.CognitiveServices.Speech.extension.codec.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\Microsoft.CognitiveServices.Speech.extension.kws.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\Microsoft.CognitiveServices.Speech.extension.kws.ort.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\Microsoft.CognitiveServices.Speech.extension.lu.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\Microsoft.Win32.SystemEvents.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\NAudio.Asio.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\NAudio.Core.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\NAudio.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\NAudio.Midi.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\NAudio.Wasapi.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\NAudio.WaveFormRenderer.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\NAudio.WinForms.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\NAudio.WinMM.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\ScrollPanelWFCoreVirtual.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Drawing.Common.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.IO.Packaging.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\taglib-sharp.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\TitleBarWF.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\windowsdesktop-runtime-10.0.2-win-x64.exe"; DestDir: "{tmp}"; Flags: deleteafterinstall
Source: "{#SourcePath}\WPFSVLCore.dll"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; IconFilename: "{app}\{#IconFilename}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"; IconFilename: "{app}\{#IconFilename}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; IconFilename: "{app}\{#IconFilename}"; Tasks: desktopicon

[Run]
Filename: "{tmp}\windowsdesktop-runtime-10.0.2-win-x64.exe"; Parameters: "/install /quiet /norestart"; Check: NeedsDotNet
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

