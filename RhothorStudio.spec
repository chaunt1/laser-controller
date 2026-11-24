# -*- mode: python ; coding: utf-8 -*-
"""
PyInstaller Specification File for Rhothor Laser Studio.
Builds a standalone, single-file Windows Executable (.exe) or directory bundle.
"""

import sys
import os

block_cipher = None

project_root = os.path.abspath(SPECPATH)

# Data files to bundle
datas = [
    (os.path.join(project_root, 'resources'), 'resources'),
    (os.path.join(project_root, 'examples', 'sample_ezcad.dxf'), 'examples'),
    (os.path.join(project_root, 'apps', 'easylaser', 'static'), os.path.join('apps', 'easylaser', 'static')),
]

# Binaries to bundle (Rhothor hardware DLLs)
binaries = []
dll_candidates = [
    (os.path.join(project_root, 'sdk', 'lib', 'x64', 'rhothor.dll'), os.path.join('sdk', 'lib', 'x64')),
    (os.path.join(project_root, 'sdk', 'lib', 'x86', 'rhothor.dll'), os.path.join('sdk', 'lib', 'x86')),
    (os.path.join(project_root, 'vendor', 'rhothor_exe', 'rhothor.dll'), '.'),
]

for src, dst in dll_candidates:
    if os.path.exists(src):
        binaries.append((src, dst))

# Hidden imports required by dynamic reflection
candidate_hidden = [
    'PyQt5',
    'PyQt5.QtCore',
    'PyQt5.QtGui',
    'PyQt5.QtWidgets',
    'PyQt5.QtSvg',
    'ctypes',
    'ctypes.wintypes',
    'sdk.python.rhothor',
    'sdk.python.simulator',
    'apps.easylaser.parsers',
    'apps.easylaser.server',
    'apps.rhothor_studio.main_window',
    'apps.rhothor_studio.canvas',
    'apps.rhothor_studio.workers',
    'apps.rhothor_studio.tabs.tab_connection',
    'apps.rhothor_studio.tabs.tab_2d_marking',
    'apps.rhothor_studio.tabs.tab_3d_dynamic',
    'apps.rhothor_studio.tabs.tab_timing',
    'apps.rhothor_studio.tabs.tab_io_trigger',
]
hiddenimports = []
for mod in candidate_hidden:
    try:
        __import__(mod)
        hiddenimports.append(mod)
    except Exception:
        hiddenimports.append(mod)

# Excluded packages to minimize binary size
excludes = [
    'tkinter',
    'matplotlib',
    'numpy',
    'scipy',
    'pandas',
    'IPython',
    'PIL',
    'pytest',
    'unittest',
]

a = Analysis(
    ['rhothor_studio.py'],
    pathex=[project_root],
    binaries=binaries,
    datas=datas,
    hiddenimports=hiddenimports,
    hookspath=[],
    hooksconfig={},
    runtime_hooks=[],
    excludes=excludes,
    win_no_prefer_redirects=False,
    win_private_assemblies=False,
    cipher=block_cipher,
    noarchive=False,
)

pyz = PYZ(a.pure, a.zipped_data, cipher=block_cipher)

icon_file = os.path.join(project_root, 'resources', 'icon.ico')
if not os.path.exists(icon_file):
    icon_file = None

exe = EXE(
    pyz,
    a.scripts,
    a.binaries,
    a.zipfiles,
    a.datas,
    [],
    name='RhothorStudio',
    debug=False,
    bootloader_ignore_signals=False,
    strip=False,
    upx=True,
    upx_exclude=[],
    runtime_tmpdir=None,
    console=False,  # Set to False so no command prompt opens on Windows
    disable_windowed_traceback=False,
    argv_emulation=False,
    target_arch=None,
    codesign_identity=None,
    entitlements_file=None,
    icon=icon_file,
)
