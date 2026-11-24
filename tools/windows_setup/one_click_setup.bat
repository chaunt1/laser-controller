@echo off
title Rhothor Easy Setup for Windows 10 & 11
echo ========================================================
echo   Newson Rhothor Laser Controller - Easy Setup Wizard
echo   Compatible with Windows 10 & Windows 11 (64-bit)
echo ========================================================
echo.

:: 1. Check Administrator Privileges
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo [!] WARNING: Please right-click this .bat file and select "Run as Administrator"!
    echo.
    pause
    exit /b 1
)

echo [*] Step 1: Registering Rhothor USB Device GUID into Windows Registry...
reg import "%~dp0..\register_device_guid.reg" >nul 2>&1
if %errorLevel% equ 0 (
    echo [+] Registry GUID successfully registered!
) else (
    echo [-] Failed to import registry file. Please verify permissions.
)
echo.

echo [*] Step 2: Configuring Windows 7 Compatibility Mode for rhothor.exe & Config.exe...
set "RHOTHOR_DIR=%~dp0..\..\vendor\rhothor_exe\src"
if exist "%RHOTHOR_DIR%\rhothor.exe" (
    reg add "HKCU\Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers" /v "%RHOTHOR_DIR%\rhothor.exe" /d "~ WIN7RTM RUNASADMIN" /f >nul 2>&1
    echo [+] Set Compatibility Mode for rhothor.exe
)
if exist "%RHOTHOR_DIR%\Config.exe" (
    reg add "HKCU\Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers" /v "%RHOTHOR_DIR%\Config.exe" /d "~ WIN7RTM RUNASADMIN" /f >nul 2>&1
    echo [+] Set Compatibility Mode for Config.exe
)
echo.

echo [*] Step 3: Verifying DLL dependencies...
if exist "%RHOTHOR_DIR%\rhothor.dll" (
    echo [+] rhothor.dll found in application directory.
) else (
    echo [!] Copying rhothor.dll to executable directory...
    copy "%~dp0..\..\sdk\lib\x86\rhothor.dll" "%RHOTHOR_DIR%\" >nul
)
echo.

echo ========================================================
echo   SETUP COMPLETED SUCCESSFULLY!
echo ========================================================
echo.
echo Next steps:
echo   1. Connect Rhothor 12VDC power and USB cable.
echo   2. If driver is not yet installed, open Zadig and install WinUSB for RT-CU.
echo   3. Run 'launch_rhothor.bat' to start the control software.
echo.
pause
