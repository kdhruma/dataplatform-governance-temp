::check if Config folder exists in Project Directory, if not, create it
cd  %~1
if not exist "%~1Config" mkdir "%~1Config"
::Copy files of TargetDir/Config to ProjectDir/Config of Project Directory
xcopy "%~2Config" "%~1Config" /S /Y

::Data Files
::check if data folder exists in Project Directory, if not, create it
cd  %~1
if not exist "%~1Data" mkdir "%~1Data"
::Copy files of TargetDir/Data to ProjectDir/Data of Project Directory
xcopy "%~2Data" "%~1Data" /S /Y /F
::check if Config folder exists in Project Directory, if not, create it

::Copy BR files
::check if Config\BR folder exists in Project Directory, if not, create it
if not exist "%~1Config\BR" mkdir "%~1Config\BR"
xcopy "%~2BR" "%~1Config\BR" /S /Y
::check if Config\BR\Out folder exists in Project Directory, if not, create it
if not exist "%~1Config\BR\Out" mkdir "%~1Config\BR\Out"
xcopy "%~2BR\Out" "%~1Config\BR\Out" /S /Y
::check if Config\BR\bin folder exists in Project Directory, if not, create it
if not exist "%~1Config\BR\bin" mkdir "%~1Config\BR\bin"