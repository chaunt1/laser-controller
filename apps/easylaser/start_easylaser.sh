#!/usr/bin/env bash
set -e

DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
cd "$DIR"

echo "=========================================================="
echo "  Starting Rhothor EasyLaser Web Interface on port 8080..."
echo "=========================================================="

python3 apps/easylaser/server.py 8080
