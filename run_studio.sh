#!/usr/bin/env bash
set -e

DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$DIR"

echo "=========================================================="
echo "  Starting Rhothor Laser Studio Workstation (PyQt5)..."
echo "=========================================================="

python3 rhothor_studio.py "$@"
