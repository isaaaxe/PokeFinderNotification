@echo off
set SERVICE_NAME=PokeFinderNotification
set SERVICE_EXE=%~dp0PokeFinderNotification.exe

sc query "%SERVICE_NAME%" >nul 2>&1

if %ERRORLEVEL% == 1060 (
    echo Service "%SERVICE_NAME%" does NOT exist.
    echo Creating service...
    sc create "%SERVICE_NAME%" binPath= "%SERVICE_EXE%" start= auto
    echo Service created.
    goto end
)

echo Service "%SERVICE_NAME%" already exists. Skipping creation.
goto end

:end
timeout 30
exit