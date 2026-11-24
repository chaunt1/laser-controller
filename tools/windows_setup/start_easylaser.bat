@echo off
title Rhothor EasyLaser - Trinh Dieu Khien Khac Laser Don Gian
color 0B

echo ======================================================================
echo          RHOTHOR EASYLASER - PHAN MEM KHAC LASER TOI GIAN
echo               Tuong thich Windows 7, 10, 11 (32-bit & 64-bit)
echo ======================================================================
echo.

cd /d "%~dp0\..\.."

where python >nul 2>nul
if %errorlevel% neq 0 (
    echo [ERROR] Khong tim thay Python! Vui long cai dat Python 3.8+ tu python.org
    echo Nho tich chon: "Add Python to PATH"
    pause
    exit /b 1
)

echo Dang khoi dong Web Server tren http://localhost:8080 ...
start "" http://localhost:8080
python apps\easylaser\server.py 8080

pause
