@ECHO OFF
ECHO.

SET HOME=%~dp0
SET BUILDTOOLS=%HOME%build tools\
SET SOURCE=%HOME%StreamDeckMonitor\bin\x86\Release\
SET DESTINATION=%HOME%Release\

xcopy %SOURCE%* %DESTINATION% /s /q

for /f delims^=^"^ tokens^=2 %%i in ('findstr "AssemblyVersion" %1 %HOME%StreamDeckMonitor\Properties\AssemblyInfo.cs') DO SET VERSION=%%i

ECHO.
ECHO.
ECHO.
ECHO                              ........................................
ECHO                              ....                                ....
ECHO                              .....                              .....
ECHO                              ....      Zip Release Build ??      .... 
ECHO                              .....                              .....	
ECHO                              ......                            ......
ECHO                              ........................................
ECHO.
ECHO.

:start
SET choice=
SET /p choice=  [Q] --   Package Release Build:  StreamDeckMonitor_v%VERSION% [N]: 
IF NOT '%choice%'=='' SET choice=%choice:~0,1%
IF '%choice%'=='Y' GOTO yes
IF '%choice%'=='y' GOTO yes
IF '%choice%'=='N' GOTO no
IF '%choice%'=='n' GOTO no
IF '%choice%'=='' GOTO no
ECHO "%choice%" is not valid
ECHO.
GOTO start

:no
EXIT

:yes
ECHO.
ECHO.
GOTO zip

:zip
ECHO.
ECHO.
ECHO  [I] --   Zipping up "StreamDeckMonitor_v%VERSION%.zip" ..
ECHO.
ECHO ...
ECHO .......
ECHO ............
ECHO .......
ECHO ...
"%BUILDTOOLS%7z.exe" a  -r %DESTINATION%StreamDeckMonitor_v%VERSION%.zip -w %DESTINATION%* -mem=AES256
ECHO.
ECHO.
ECHO.
PAUSE
EXIT