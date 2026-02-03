#define MyAppName "CaptionAll"
#define MyAppVersion "26.2201.4241"












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
Source: "{#SourcePath}\Accessibility.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-core-console-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-core-console-l1-2-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-core-datetime-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-core-debug-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-core-errorhandling-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-core-fibers-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-core-file-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-core-file-l1-2-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-core-file-l2-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-core-handle-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-core-heap-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-core-interlocked-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-core-libraryloader-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-core-localization-l1-2-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-core-memory-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-core-namedpipe-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-core-processenvironment-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-core-processthreads-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-core-processthreads-l1-1-1.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-core-profile-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-core-rtlsupport-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-core-string-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-core-synch-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-core-synch-l1-2-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-core-sysinfo-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-core-timezone-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-core-util-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-crt-conio-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-crt-convert-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-crt-environment-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-crt-filesystem-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-crt-heap-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-crt-locale-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-crt-math-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-crt-multibyte-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-crt-private-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-crt-process-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-crt-runtime-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-crt-stdio-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-crt-string-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-crt-time-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\api-ms-win-crt-utility-l1-1-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\CaptionAll.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\CaptionAll.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\CaptionAllIcon.ico"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\CaptionBubbleEditorWF.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\clretwrc.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\clrjit.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\coreclr.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\createdump.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\D3DCompiler_47_cor3.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\dbgshim.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\DirectWriteForwarder.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\DocumentFormat.OpenXml.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\DocumentFormat.OpenXml.Framework.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\dotExcel.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\hostfxr.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\hostpolicy.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\MediaPlayerWPFUC.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\Microsoft.CognitiveServices.Speech.core.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\Microsoft.CognitiveServices.Speech.csharp.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\Microsoft.CognitiveServices.Speech.extension.audio.sys.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\Microsoft.CognitiveServices.Speech.extension.codec.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\Microsoft.CognitiveServices.Speech.extension.kws.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\Microsoft.CognitiveServices.Speech.extension.kws.ort.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\Microsoft.CognitiveServices.Speech.extension.lu.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\Microsoft.CSharp.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\Microsoft.DiaSymReader.Native.amd64.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\Microsoft.VisualBasic.Core.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\Microsoft.VisualBasic.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\Microsoft.VisualBasic.Forms.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\Microsoft.Win32.Primitives.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\Microsoft.Win32.Registry.AccessControl.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\Microsoft.Win32.Registry.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\Microsoft.Win32.SystemEvents.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\mscordaccore.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\mscordaccore_amd64_amd64_6.0.2824.12007.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\mscordbi.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\mscorlib.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\mscorrc.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\msquic.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\NAudio.Asio.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\NAudio.Core.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\NAudio.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\NAudio.Midi.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\NAudio.Wasapi.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\NAudio.WaveFormRenderer.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\NAudio.WinForms.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\NAudio.WinMM.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\netstandard.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\PenImc_cor3.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\PresentationCore.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\PresentationFramework-SystemCore.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\PresentationFramework-SystemData.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\PresentationFramework-SystemDrawing.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\PresentationFramework-SystemXml.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\PresentationFramework-SystemXmlLinq.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\PresentationFramework.Aero.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\PresentationFramework.Aero2.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\PresentationFramework.AeroLite.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\PresentationFramework.Classic.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\PresentationFramework.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\PresentationFramework.Luna.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\PresentationFramework.Royale.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\PresentationNative_cor3.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\PresentationUI.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\ReachFramework.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\ScrollPanelWFCoreVirtual.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.AppContext.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Buffers.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.CodeDom.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Collections.Concurrent.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Collections.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Collections.Immutable.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Collections.NonGeneric.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Collections.Specialized.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.ComponentModel.Annotations.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.ComponentModel.DataAnnotations.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.ComponentModel.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.ComponentModel.EventBasedAsync.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.ComponentModel.Primitives.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.ComponentModel.TypeConverter.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Configuration.ConfigurationManager.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Configuration.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Console.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Core.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Data.Common.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Data.DataSetExtensions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Data.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Design.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Diagnostics.Contracts.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Diagnostics.Debug.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Diagnostics.DiagnosticSource.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Diagnostics.EventLog.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Diagnostics.EventLog.Messages.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Diagnostics.FileVersionInfo.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Diagnostics.PerformanceCounter.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Diagnostics.Process.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Diagnostics.StackTrace.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Diagnostics.TextWriterTraceListener.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Diagnostics.Tools.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Diagnostics.TraceSource.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Diagnostics.Tracing.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.DirectoryServices.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Drawing.Common.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Drawing.Design.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Drawing.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Drawing.Primitives.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Dynamic.Runtime.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Formats.Asn1.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Globalization.Calendars.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Globalization.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Globalization.Extensions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.IO.Compression.Brotli.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.IO.Compression.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.IO.Compression.FileSystem.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.IO.Compression.Native.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.IO.Compression.ZipFile.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.IO.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.IO.FileSystem.AccessControl.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.IO.FileSystem.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.IO.FileSystem.DriveInfo.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.IO.FileSystem.Primitives.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.IO.FileSystem.Watcher.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.IO.IsolatedStorage.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.IO.MemoryMappedFiles.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.IO.Packaging.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.IO.Pipes.AccessControl.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.IO.Pipes.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.IO.UnmanagedMemoryStream.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Linq.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Linq.Expressions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Linq.Parallel.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Linq.Queryable.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Memory.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Net.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Net.Http.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Net.Http.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Net.HttpListener.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Net.Mail.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Net.NameResolution.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Net.NetworkInformation.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Net.Ping.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Net.Primitives.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Net.Quic.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Net.Requests.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Net.Security.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Net.ServicePoint.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Net.Sockets.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Net.WebClient.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Net.WebHeaderCollection.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Net.WebProxy.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Net.WebSockets.Client.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Net.WebSockets.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Numerics.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Numerics.Vectors.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.ObjectModel.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Printing.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Private.CoreLib.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Private.DataContractSerialization.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Private.Uri.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Private.Xml.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Private.Xml.Linq.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Reflection.DispatchProxy.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Reflection.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Reflection.Emit.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Reflection.Emit.ILGeneration.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Reflection.Emit.Lightweight.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Reflection.Extensions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Reflection.Metadata.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Reflection.Primitives.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Reflection.TypeExtensions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Resources.Extensions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Resources.Reader.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Resources.ResourceManager.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Resources.Writer.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Runtime.CompilerServices.Unsafe.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Runtime.CompilerServices.VisualC.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Runtime.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Runtime.Extensions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Runtime.Handles.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Runtime.InteropServices.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Runtime.InteropServices.RuntimeInformation.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Runtime.Intrinsics.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Runtime.Loader.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Runtime.Numerics.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Runtime.Serialization.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Runtime.Serialization.Formatters.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Runtime.Serialization.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Runtime.Serialization.Primitives.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Runtime.Serialization.Xml.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Security.AccessControl.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Security.Claims.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Security.Cryptography.Algorithms.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Security.Cryptography.Cng.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Security.Cryptography.Csp.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Security.Cryptography.Encoding.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Security.Cryptography.OpenSsl.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Security.Cryptography.Pkcs.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Security.Cryptography.Primitives.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Security.Cryptography.ProtectedData.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Security.Cryptography.X509Certificates.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Security.Cryptography.Xml.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Security.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Security.Permissions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Security.Principal.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Security.Principal.Windows.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Security.SecureString.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.ServiceModel.Web.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.ServiceProcess.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Text.Encoding.CodePages.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Text.Encoding.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Text.Encoding.Extensions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Text.Encodings.Web.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Text.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Text.RegularExpressions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Threading.AccessControl.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Threading.Channels.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Threading.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Threading.Overlapped.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Threading.Tasks.Dataflow.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Threading.Tasks.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Threading.Tasks.Extensions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Threading.Tasks.Parallel.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Threading.Thread.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Threading.ThreadPool.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Threading.Timer.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Transactions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Transactions.Local.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.ValueTuple.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Web.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Web.HttpUtility.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Windows.Controls.Ribbon.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Windows.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Windows.Extensions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Windows.Forms.Design.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Windows.Forms.Design.Editors.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Windows.Forms.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Windows.Forms.Primitives.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Windows.Input.Manipulations.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Windows.Presentation.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Xaml.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Xml.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Xml.Linq.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Xml.ReaderWriter.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Xml.Serialization.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Xml.XDocument.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Xml.XmlDocument.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Xml.XmlSerializer.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Xml.XPath.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\System.Xml.XPath.XDocument.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\taglib-sharp.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\TitleBarWF.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\ucrtbase.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\UIAutomationClient.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\UIAutomationClientSideProviders.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\UIAutomationProvider.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\UIAutomationTypes.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\vcruntime140_cor3.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\WindowsBase.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\WindowsFormsIntegration.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\wpfgfx_cor3.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourcePath}\WPFSVLCore.dll"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; IconFilename: "{app}\{#IconFilename}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"; IconFilename: "{app}\{#IconFilename}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; IconFilename: "{app}\{#IconFilename}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent