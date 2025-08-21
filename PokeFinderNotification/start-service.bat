@ECHO OFF


sc query "PokeFinderNotification" %1 | findstr RUNNING
if %ERRORLEVEL% == 2 goto trouble
if %ERRORLEVEL% == 1 goto stopped
if %ERRORLEVEL% == 0 goto started
echo unknown status
goto end
:trouble
echo There are some issues
goto end
:started
echo "PokeFinderNotification" is started
goto end
:stopped
echo "PokeFinderNotification" is stopped
echo Starting service
net start "PokeFinderNotification"
goto end
:erro
echo Error please check command
goto end

:end
timeout 30
exit
