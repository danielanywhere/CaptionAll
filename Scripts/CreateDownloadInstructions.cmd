:: CreateDownloadInstructions.cmd
:: Create the download instructions for a release.
:: This command is meant to be run from within the Scripts folder.
SET FAR=C:\Files\Dropbox\Develop\Shared\FindAndReplace\Source\FindAndReplace\bin\Debug\net6.0\FindAndReplace.exe
SET SOURCE=..\Docs\CaptionAll-InstallationGuide.odt
SET TARGET=..\Docs\CaptionAll-InstallationGuide.md
SET PATTERN=InstallationGuidePostProcessing.json

:: When the image has a URL assigned it isn't placed in the output. Use 'Image' or 'Banner' blocks.
PANDOC -t markdown_strict --embed-resources=false --wrap=none "%SOURCE%" -o "%TARGET%"
"%FAR%" /wait "/files:%TARGET%" "/patternfile:%PATTERN%"
