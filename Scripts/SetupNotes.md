# Setup Notes

This article describes the work being done on the CaptionAll Setup project to automate its process as much as possible.

As of 12/26/2023, use the following process for building a new version of CaptionAllSetup.

 - Make sure CaptionAll.csproj Project / PropertyGroup contains the entry <code>&lt;RuntimeIdentifiers&gt;win-x64&lt;/RuntimeIdentifiers&gt;</code>.
 - Run the script <code>Scripts/VersionCompilePublish.cmd</code>.

As of 12/01/2023, the process of bulding a new version of CaptionAllSetup is the following.

 - Set the application version using the <code>Scripts/SetAppVersion.cmd</code> command. This applies the same version number to the C# project and the InnoSetup package configuration file.
 - Compile the Visual Studio project.
     - In **Solution Explorer**, right-click the **CaptionAll** project and from the context menu, select **Publish**.
     - Click **Publish**.
 - Prepare the uninstall application.
     - Delete the existing file from <code>../../Develop/CaptionAll/Setup-SignedUninstaller</code>.
     - Run the InnoSetup project at <code>Scripts/BuildCaptionAllSetup.cmd</code> to create the uninstaller.
     - You will receive an error that the uninstall application has not yet been signed. When given the chance to continue on the command line, close the terminal.
     - In the folder <code>../../Develop/CaptionAll/Setup-SignedUninstaller</code>, copy the name of the file and paste it into <code>Scripts/SignSignedUninstaller.cmd</code> at the variable named **UNINSTALLERNAME**.
     - Run the command <code>Scripts/SignSignedUninstaller.cmd</code> to sign the uninstallation file.
 - Create the setup application by re-running the command <code>Scripts/BuildCaptionAllSetup.cmd</code>. When prompted, press any key to complete the process.
