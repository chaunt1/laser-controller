#!/usr/bin/env python3
"""
Rhothor Laser Studio Root Launcher.
Run this script to launch the full Qt desktop application.
Usage:
    python rhothor_studio.py
    python rhothor_studio.py --serial 4281 --name LPKF
"""

import sys
import os

PROJECT_ROOT = os.path.dirname(os.path.abspath(__file__))
if PROJECT_ROOT not in sys.path:
    sys.path.insert(0, PROJECT_ROOT)

from apps.rhothor_studio.main import main

if __name__ == "__main__":
    main()
