@ECHO OFF
CLS

:: Set variables
SET HOME=%~dp0
SET BUILDTOOLS=%HOME%build tools\
SET SOURCE=%HOME%StreamDeckMonitor\bin\x86\Release\
SET DESTINATION=%HOME%Release\
SET DIRCOMPAREFILE=dirlist.txt
SET FAILLOG=faillog.txt

:: Copy the complete release from the VS Source (bin\x86\Release\) directory to the release dir
XCOPY %SOURCE%* %DESTINATION% /s /q
TIMEOUT 1 > NUL

:: Check if copy was successful
IF NOT EXIST %DESTINATION% (
	SET RESULT=NOT Completed!
	GOTO cleanexit		
) || (
	GOTO mainmenu
)

:: Main choice menu
:mainmenu
:: Set the Version variable according to the AssemblyInfo value
FOR /f delims^=^"^ tokens^=2 %%i IN ('findstr "AssemblyVersion" %1 %HOME%StreamDeckMonitor\Properties\AssemblyInfo.cs') DO SET VERSION=%%i
ECHO.
ECHO.
ECHO.
ECHO.
ECHO                            ------------------------------------------------
ECHO                            ------------------------------------------------
ECHO                            ------                                    ------
ECHO                            -------                                  -------
ECHO                            ------   Zip StreamDeckMonitor Build ??   ------ 
ECHO                            -------                                  -------	
ECHO                            --------                                --------
ECHO                            ------------------------------------------------
ECHO                            ------------------------------------------------
ECHO.
ECHO.
ECHO.
ECHO                            Package Release Build: StreamDeckMonitor_v%VERSION% ??
ECHO.
	"%BUILDTOOLS%Choose32.exe" -c ^Aq [ -d ^A -q -n -p "                                   (ENTER to continue, Q to quit)"
ECHO.
ECHO.

IF %ERRORLEVEL% == 1 ( 
	GOTO zip
)
IF %ERRORLEVEL% == 2 (
	EXIT
)
IF %ERRORLEVEL% == 255 (
	EXIT 
)

:: Main validation and zipping
:zip
CLS
ECHO.
ECHO.
ECHO.
ECHO.
ECHO                      -------------------------------------------------------------
ECHO                      -------------------------------------------------------------
ECHO                                Zipping up "StreamDeckMonitor_v%VERSION%.zip"
ECHO                      -------------------------------------------------------------
ECHO                      -------------------------------------------------------------
ECHO.

:: Check for previous zip and delete
ECHO                      Getting Ready ...
TIMEOUT 1 > NUL

IF EXIST %DESTINATION%*.zip (
    DEL /s /q /f %DESTINATION%*.zip > NUL
) 

:: Check for previous directory comparison file and delete
ECHO                      Gathering Information ...
TIMEOUT 1 > NUL

IF EXIST %DIRCOMPAREFILE% (
    DEL /s /q /f %DIRCOMPAREFILE% > NUL
) 

:: Create new directory comparison file
XCOPY /L /y /d /s "%SOURCE%*" "%DESTINATION%" > %DIRCOMPAREFILE%

:: Validate directory comparison file
ECHO                      Validating All Files ...
TIMEOUT 1 > NUL

:: If Files validated
> NUL find "0 File(s)" %DIRCOMPAREFILE% && (

	ECHO                      Zipping Release ...
	ECHO.
		:: Zip the release package
		"%BUILDTOOLS%7z.exe" a  -r %DESTINATION%StreamDeckMonitor_v%VERSION%.zip -w %DESTINATION%* -mem=AES256
	ECHO.
	ECHO.	
	ECHO                      -------------------------------------------------------------	
	ECHO                      -------------------------------------------------------------
	ECHO                                 Checking Integrity of new Zip file ...
	ECHO                      -------------------------------------------------------------
	ECHO                      -------------------------------------------------------------
	ECHO.
	TIMEOUT 1 > NUL
		:: Check Integrity of the zipped release build
		"%BUILDTOOLS%7z.exe" t %DESTINATION%StreamDeckMonitor_v%VERSION%.zip	
	
	SET RESULT=Completed
	GOTO cleanexit

) || (

	:: If Files are NOT validated
	ECHO                      Files NOT Validated! 
	ECHO.

	:: Copy the directory comparison file to faillog.txt and delete original directory comparison file
	> NUL COPY /b %DIRCOMPAREFILE% %FAILLOG%

	ECHO                      Check 'faillog.txt' To see which files failed Validation
	
	SET RESULT=NOT Completed!
	GOTO cleanexit	
)

:: Show validation result, Clean up the directory comparison file and exit
:cleanexit
ECHO.
ECHO.
ECHO                      Cleaning Up ...
ECHO.

IF EXIST %DIRCOMPAREFILE% (
	DEL /s /q /f %DIRCOMPAREFILE% > NUL
) 

TIMEOUT 1 > NUL
ECHO                      -------------------------------------------------------------
ECHO                      -------------------------------------------------------------
ECHO                                         Operation %RESULT%
ECHO                      -------------------------------------------------------------
ECHO                      -------------------------------------------------------------
ECHO.
ECHO.
ECHO.
PAUSE
EXIT
