@ECHO OFF
CLS

:: Set variables
SET HOME=%~dp0
SET WAIT=ping 123.45.67.89 -n 1 -w 500 
SET BUILDTOOLS=%HOME%build tools\
SET SOURCE=%HOME%StreamDeckMonitor\bin\x86\Release\
SET DESTINATION=%HOME%Release\
SET COPYRESULT=copyresult.txt
SET DIRCOMPAREFILE=dirlist.txt
SET FAILLOG=faillog.txt

:: Copy newly built StreamDeckMonitor to Release folder
ECHO.
ECHO.
ECHO.
ECHO.
ECHO                      -------------------------------------------------------------
ECHO                      -------------------------------------------------------------
ECHO                                  Copy StreamDeckMonitor Build Folders
ECHO                      -------------------------------------------------------------
ECHO                      -------------------------------------------------------------
ECHO.
ECHO                      Preparing and Copying Folders ...
XCOPY %SOURCE%* %DESTINATION% /s /q /y > %COPYRESULT% 2>&1
IF EXIST %DESTINATION%*.zip (
    DEL /s /q /f %DESTINATION%*.zip >NUL
)
%WAIT% >nul

:: Check if copy was successful
ECHO                      Verifying File Copy ...
%WAIT% >nul
>NUL find "0 File(s)" %COPYRESULT% && (
	ECHO                      Copy NOT Successful! 	
	ECHO.
	SETLOCAL enableextensions enabledelayedexpansion
	FOR /f "delims=" %%i IN (%COPYRESULT%) DO (
		ECHO                        Error: " %%i "
		GOTO :copyerror
	)
	:copyerror
	ENDLOCAL	
	%WAIT% >nul
	SET RESULT=NOT Completed!
	GOTO statusexit		
) || (
	ECHO                      Copy Successful! 	
	ECHO.
	SETLOCAL enableextensions enabledelayedexpansion
	FOR /f "delims=" %%i IN (%COPYRESULT%) DO (
		ECHO                      Status: " %%i "
		GOTO :copysuccess
	)
	:copysuccess
	ENDLOCAL
	%WAIT% >nul
	GOTO mainmenu
)

:: Zip choice menu
:mainmenu
CLS
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
:: Set the Version variable according to the AssemblyInfo value
FOR /f delims^=^"^ tokens^=2 %%i IN ('findstr "AssemblyVersion" %1 %HOME%StreamDeckMonitor\Properties\AssemblyInfo.cs') DO SET VERSION=%%i
ECHO                            Package Release Build: StreamDeckMonitor_v%VERSION% ??
ECHO.
"%BUILDTOOLS%Choose32.exe" -c ^Aq [ -d ^A -q -n -p "                                   (ENTER to continue, Q to quit)"
ECHO.
ECHO.
IF %ERRORLEVEL% == 1 ( 
	GOTO zip
)
IF %ERRORLEVEL% == 2 (
	GOTO nostatusexit
)	
IF %ERRORLEVEL% == 255 (
	GOTO nostatusexit
)

:: Validation and zipping
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
%WAIT% >nul
IF EXIST %DESTINATION%*.zip (
    DEL /s /q /f %DESTINATION%*.zip >NUL
) 

:: Check for previous directory comparison file and delete
ECHO                      Gathering Information ...
%WAIT% >nul
IF EXIST %DIRCOMPAREFILE% (
    DEL /s /q /f %DIRCOMPAREFILE% >NUL
) 

:: Create new directory comparison file
XCOPY /L /y /d /s "%SOURCE%*" "%DESTINATION%" > %DIRCOMPAREFILE%

:: Validate directory comparison file
ECHO                      Validating Files ...
%WAIT% >nul
>NUL find "0 File(s)" %DIRCOMPAREFILE% && (
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
	%WAIT% >nul
	:: Check Integrity of the zipped release build
	"%BUILDTOOLS%7z.exe" t %DESTINATION%StreamDeckMonitor_v%VERSION%.zip		
	SET RESULT=Completed
	GOTO statusexit
) || (
	ECHO                      Files NOT Validated! 
	ECHO.
	:: Copy the directory comparison file to faillog.txt and delete original directory comparison file
	>NUL COPY /b %DIRCOMPAREFILE% %FAILLOG%
	ECHO                      Check 'faillog.txt' To see which files failed Validation	
	SET RESULT=NOT Completed!
	GOTO statusexit	
)

:: Show status, Clean up the directory comparison file and exit
:statusexit
ECHO.
ECHO.
ECHO.
ECHO                      Cleaning Up ...
ECHO.
IF EXIST %COPYRESULT% (
    DEL /s /q /f %COPYRESULT% >NUL
)
IF EXIST %DIRCOMPAREFILE% (
	DEL /s /q /f %DIRCOMPAREFILE% >NUL
)
%WAIT% >nul
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

:: Quick cleanup and exit
:nostatusexit
IF EXIST %COPYRESULT% (
	DEL /s /q /f %COPYRESULT% >NUL
)
IF EXIST %DIRCOMPAREFILE% (
	DEL /s /q /f %DIRCOMPAREFILE% >NUL
) 
EXIT
