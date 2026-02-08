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
#define DotnetRuntimeInstallerName "windowsdesktop-runtime-6.0.36-win-x64.exe"

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
Source: "{#SourcePath}\CaptionAll.deps.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\CaptionAll.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion


[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; IconFilename: "{app}\{#IconFilename}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"; IconFilename: "{app}\{#IconFilename}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; IconFilename: "{app}\{#IconFilename}"; Tasks: desktopicon

[Run]
Filename: "{tmp}\windowsdesktop-runtime-10.0.2-win-x64.exe"; Parameters: "/install /quiet /norestart"; Check: NeedsDotNet
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Code]
{ Auto-generated code }
function GetLineCount(const S: string): Integer;
var
  I: Integer;
begin
  Result := 1;
  for I := 1 to Length(S) do
  begin
    if S[I] = #10 then
      Result := Result + 1;
  end;
end;

{ Auto-generated code }
function GetLine(const S: string; LineNumber: Integer): string;
var
  I, Count: Integer;
  StartPos: Integer;
begin
  Count := 1;
  StartPos := 1;

  for I := 1 to Length(S) do
  begin
    if S[I] = #10 then
    begin
      if Count = LineNumber then
      begin
        Result := Trim(Copy(S, StartPos, I - StartPos));
        Exit;
      end;

      Count := Count + 1;
      StartPos := I + 1;
    end;
  end;

  if Count = LineNumber then
    Result := Trim(Copy(S, StartPos, Length(S) - StartPos + 1))
  else
    Result := '';
end;

{ Auto-generated code }
function PosEx(const SubStr, S: string; Offset: Integer): Integer;
var
  I: Integer;
begin
  Result := 0;

  if Offset < 1 then
    Offset := 1;

  for I := Offset to (Length(S) - Length(SubStr) + 1) do
  begin
    if Copy(S, I, Length(SubStr)) = SubStr then
    begin
      Result := I;
      Exit;
    end;
  end;
end;

{ Auto-generated code }
function ExecAndCaptureOutput(const Cmd, Params: string; var Output: string): Boolean;
var
  TempFile: string;
  ResultCode: Integer;
  Buffer: AnsiString;
begin
  Log('ExecAndCaptureOutput...');

  TempFile := ExpandConstant('{tmp}\dotnet_output.txt');

  Log(' Preparing to call ' + Cmd + ' ' + Params + ' > "' + TempFile + '" 2>&1');

  { // Inno Setup Exec signature: }
  { // Exec(Filename, Params, WorkingDir, ShowCmd, Wait, ResultCode) }
  {
    if Exec(Cmd, Params + ' > "' + TempFile + '" 2>&1', '', SW_HIDE,
          ewWaitUntilTerminated, ResultCode) then
  }
  {
    if Exec('cmd.exe', '/k ' + Cmd + ' ' + Params + ' > "' + TempFile + '" 2>&1', '', SW_SHOW,
          ewWaitUntilTerminated, ResultCode) then
  }
  if Exec('cmd.exe',
    '/c ' + Cmd + ' ' + Params + ' > "' + TempFile + '" 2>&1"',
    '',
    SW_HIDE,
    ewWaitUntilTerminated,
    ResultCode) then
  begin
    if LoadStringFromFile(TempFile, Buffer) then
    begin
      Output := Buffer;
      Result := True;
      Exit;
    end;
  end;

  Output := '';
  Result := False;
end;

{ Auto-generated code }
function ExtractMajorVersionFromInstaller(const FileName: string): Integer;
var
  VersionStart, DotPos: Integer;
  VersionStr: string;
begin
  Log('ExtractMajorVersionFromInstaller...');

  {
    Version begins at position 24 in patterns like:
    windowsdesktop-runtime-6.0.36-win-x64.exe
    windowsdesktop-runtime-10.0.2-win-x64.exe
  }
  VersionStart := 24;

  { Find the first '.' after the version start }
  DotPos := PosEx('.', FileName, VersionStart);

  if (DotPos > VersionStart) then
  begin
    VersionStr := Copy(FileName, VersionStart, DotPos - VersionStart);
    Result := StrToIntDef(VersionStr, 0);
  end
  else
    { Fallback if pattern unexpected. }
    Result := 0;
end;

{ Auto-generated code }
function IsDotnetRuntimeInstalled(MajorVersion: Integer): Boolean;
var
  Output, Line: string;
  I: Integer;
begin
  Result := False;

  Log('IsDotnetRuntimeInstalled...');

  if not ExecAndCaptureOutput('dotnet', '--list-runtimes', Output) then
  begin
    Log(' Could not capture output from dotnet --list-runtimes');
    Exit;
  end;

  I := 1;
  while I <= GetLineCount(Output) do
  begin
    Line := GetLine(Output, I);

    Log('Reading line: ' + Line);

    if Pos('Microsoft.WindowsDesktop.App ' + IntToStr(MajorVersion) + '.', Line) = 1 then
    begin
      Log(' Line found...');
      Result := True;
      Exit;
    end;

    I := I + 1;
  end;
end;

{ Auto-generated code }
function NeedsDotNet(): Boolean;
var
  Major: Integer;
begin
  Log('NeedsDotNet...');

  Major := ExtractMajorVersionFromInstaller('{#DotnetRuntimeInstallerName}');
  Log(Format(' Major .NET Version: %d', [Major]));

  if Major = 0 then
  begin
    Log('Error: Unable to determine .NET major version from installer name: ' + '{#DotnetRuntimeInstallerName}');
    Result := True;
    Exit;
  end;

  Result := not IsDotnetRuntimeInstalled(Major);
end;
