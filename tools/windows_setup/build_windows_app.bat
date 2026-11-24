@echo off
title Rhothor Laser Studio - Windows App Build Tool
color 0A

echo ======================================================================
echo       RHOTHOR LASER STUDIO - WINDOWS STANDALONE APP BUILDER
echo              Dong goi ung dung thanh file RhothorStudio.exe
echo ======================================================================
echo.

cd /d "%~dp0\..\.."

:: 1. Kiem tra Python
where python >nul 2>nul
if %errorlevel% neq 0 (
    echo [ERROR] Khong tim thay Python!
    echo Vui long cai dat Python 3.8 - 3.12 tu https://www.python.org/downloads/
    echo LUU Y: Nho tich chon checkbox "Add Python to PATH" khi cai dat.
    pause
    exit /b 1
)

echo [1/4] Kiem tra phien ban Python...
python --version

:: 2. Cai dat thu vien can thiet
echo.
echo [2/4] Cai dat thu vien PyQt5 va PyInstaller...
python -m pip install --upgrade pip
python -m pip install PyQt5 pyinstaller

:: 3. Chay PyInstaller de dong goi file .exe
echo.
echo [3/4] Dang dong goi ung dung voi PyInstaller (RhothorStudio.spec)...
python -m PyInstaller --clean RhothorStudio.spec

if %errorlevel% neq 0 (
    echo.
    echo [ERROR] Qua trinh dong goi bi loi! Vui long kiem tra thong bao ben tren.
    pause
    exit /b 1
)

:: 4. Kiem tra file dau ra
echo.
echo [4/4] Kiem tra file thuc thi...
if exist "dist\RhothorStudio.exe" (
    echo.
    echo ======================================================================
    echo  [THANH CONG] File ung dung da duoc tao thanh cong tai:
    echo     dist\RhothorStudio.exe
    echo ======================================================================
    echo.
    echo Ban co the:
    echo  - Copy file 'dist\RhothorStudio.exe' vao bat ky may tinh Windows nao
    echo  - Chay truc tiep khong can cai dat Python hay bat ky thu vien nao!
    echo.
) else (
    echo [WARNING] Khong tim thay dist\RhothorStudio.exe. Vui long kiem tra thu muc dist.
)

pause
