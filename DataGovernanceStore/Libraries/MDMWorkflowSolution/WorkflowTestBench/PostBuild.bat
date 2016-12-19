::check if Config folder exists in Project Directory, if not, create it
cd  %~1
if not exist "%~1Config" mkdir "%~1Config"
::Copy files of TargetDir/Config to ProjectDir/Config of Project Directory
xcopy "%~2Config" "%~1Config" /S /Y