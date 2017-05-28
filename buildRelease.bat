rem

copy PartWizard\bin\Release\PartWizard.dll GameData\PartWizard\Plugins
copy PartWizard\PartWizard.version GameData\PartWizard
copy README GameData\PartWizard

set RELEASEDIR=d:\Users\jbb\release
set ZIP="c:\Program Files\7-zip\7z.exe"



copy bin\Release\NRAP.dll ..\Output\GameData\NRAP\Plugins

set VERSIONFILE=a.version
copy GameData\PartWizard\PartWizard.version a.version
rem The following requires the JQ program, available here: https://stedolan.github.io/jq/download/
c:\local\jq-win64  ".VERSION.MAJOR" %VERSIONFILE% >tmpfile
set /P major=<tmpfile

c:\local\jq-win64  ".VERSION.MINOR"  %VERSIONFILE% >tmpfile
set /P minor=<tmpfile

c:\local\jq-win64  ".VERSION.PATCH"  %VERSIONFILE% >tmpfile
set /P patch=<tmpfile

c:\local\jq-win64  ".VERSION.BUILD"  %VERSIONFILE% >tmpfile
set /P build=<tmpfile
del tmpfile
set VERSION=%major%.%minor%.%patch%
if "%build%" NEQ "0"  set VERSION=%VERSION%.%build%
del a.version
echo %VERSION%
pause



set FILE="%RELEASEDIR%\PartWizard-%VERSION%.zip"
IF EXIST %FILE% del /F %FILE%
%ZIP% a -tzip %FILE% GameData

