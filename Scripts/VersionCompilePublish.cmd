:: UpdatePackageFiles.cmd
:: Update the [Files] section in the InnoScript file.
SET CONFIGFILENAME=InnoSetupToolsVersionCompilePublish.json
SET IST=C:\Files\Dropbox\Develop\Active\InnoSetupTools\InnoSetupTools\bin\Debug\net7.0\InnoSetupTools.exe

"%IST%" /wait "/config:%CONFIGFILENAME%"
