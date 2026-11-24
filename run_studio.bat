@echo off
title Rhothor Laser Studio Launcher
color 0B

cd /d "%~dp0"

:: 1. Kiem tra neu da build san ban .exe
if exist "dist\RhothorStudio.exe" (
    echo [INFO] Dang khoi chay Rhothor Laser Studio tu file thuc thi dist\RhothorStudio.exe...
    start "" "dist\RhothorStudio.exe"
    exit /b 0
)

:: 2. Neu chua build .exe, chay truc tiep qua Python
echo [INFO] Khong tim thay dist\RhothorStudio.exe, dang khoi chay qua Python...
where python >nul 2>nul
if %errorlevel% neq 0 (
    echo [ERROR] Khong tim thay Python! Vui long cai dat Python hoac chay build_windows_app.bat
    pause
    exit /b 1
)

python rhothor_studio.py
if %errorlevel% neq 0 (
    echo.
    echo [HUONG DAN] Neu gap loi thieu thu vien, vui long chay:
    echo    python -m pip install PyQt5
    pause
)
