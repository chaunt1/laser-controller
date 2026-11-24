#!/usr/bin/env python3
"""
Entry Point for Rhothor Laser Studio Desktop Application.
Supports standalone launch, command-line arguments, and high-DPI displays.
"""

import sys
import os
import argparse

# Ensure project root is in sys.path
PROJECT_ROOT = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", ".."))
if PROJECT_ROOT not in sys.path:
    sys.path.insert(0, PROJECT_ROOT)

from PyQt5.QtWidgets import QApplication
from PyQt5.QtCore import Qt

from apps.rhothor_studio.main_window import MainWindow


def run_studio(serial: int = 4281, name: str = "LPKF", initial_file: str = ""):
    # Enable high-DPI awareness on Windows 10/11
    if hasattr(Qt, "AA_EnableHighDpiScaling"):
        QApplication.setAttribute(Qt.AA_EnableHighDpiScaling, True)
    if hasattr(Qt, "AA_UseHighDpiPixmaps"):
        QApplication.setAttribute(Qt.AA_UseHighDpiPixmaps, True)

    app = QApplication(sys.argv)
    app.setApplicationName("Rhothor Laser Studio")
    app.setOrganizationName("chaunt")
    app.setOrganizationDomain("chaunt.dev")
    app.setProperty("publisher", "chaunt <chaunt.dev@gmail.com>")

    window = MainWindow(serial=serial, name=name)

    if initial_file and os.path.exists(initial_file):
        window.tab_2d.load_file(initial_file)

    window.show()
    sys.exit(app.exec_())


def main():
    parser = argparse.ArgumentParser(description="Rhothor Laser Studio - Desktop Galvo Marking Application")
    parser.add_argument("--serial", type=int, default=4281, help="Hardware serial number (default: 4281)")
    parser.add_argument("--name", type=str, default="LPKF", help="Hardware device identifier name (default: LPKF)")
    parser.add_argument("--file", type=str, default="", help="Path to initial DXF/PLT file to load")
    args = parser.parse_args()

    run_studio(serial=args.serial, name=args.name, initial_file=args.file)


if __name__ == "__main__":
    main()
