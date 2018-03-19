:: wait for StreamDeckMonitor.exe.config to be created before trying to zip the release ::

@echo OFF

:START
if not exist %1\StreamDeckMonitor\bin\x86\Release\StreamDeckMonitor.exe.config GOTO WAIT
GOTO ZIP

:WAIT
timeout /t 1 /nobreak
GOTO START

:ZIP
"C:\Program Files\7-Zip\7z.exe" a  -r %1\StreamDeckMonitor\bin\x86\Release\StreamDeckMonitor_vX.zip -w %1\StreamDeckMonitor\bin\x86\Release\* -mem=AES256