#!/usr/bin/env python3
"""
Rhothor CLI: Command-Line Control, Diagnostic & Test Tool
Supports both live hardware (via rhothor.dll) and offline simulation.
"""

import os
import sys
import json
import argparse

# Add SDK to sys.path
sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), "..")))

from sdk.python.rhothor import RhothorController, RhothorError


def print_banner():
    banner = r"""
  ____  _           _   _                ____   ____  _  __
 |  _ \| |__   ___ | |_| |__   ___  _ __/ ___| |  _ \| |/ /
 | |_) | '_ \ / _ \| __| '_ \ / _ \| '__\___ \ | | | | ' / 
 |  _ <| | | | (_) | |_| | | | (_) | |   ___) || |_| | . \ 
 |_| \_\_| |_|\___/ \__|_| |_|\___/|_|  |____(_|____/|_|\_\
  Newson Rhothor 2D & 3D Galvo Laser Controller Diagnostic Tool
    """
    print(banner)


def run_test_pattern(galvo: RhothorController, pattern: str, size: float = 50.0):
    print(f"\n[*] Executing Test Pattern: '{pattern}' (Size: {size} mm)...")
    galvo.list_open(1)
    
    if pattern in ("square", "all"):
        print(f"  -> Marking Square {size}x{size} mm at center (0, 0)")
        galvo.mark_rectangle(0, 0, size, size)
        
    if pattern in ("circle", "all"):
        radius = size / 2.0
        print(f"  -> Marking Circle radius {radius} mm at center (0, 0)")
        galvo.mark_circle(0, 0, radius)
        
    if pattern in ("cross", "all"):
        print(f"  -> Marking Crosshair {size} mm at center (0, 0)")
        galvo.mark_crosshair(0, 0, size)
        
    if pattern in ("grid", "all"):
        step = size / 5.0
        print(f"  -> Marking 5x5 Grid (Step: {step:.1f} mm)")
        half = size / 2.0
        for i in range(6):
            coord = -half + i * step
            # Horizontal line
            galvo.jump_to(-half, coord)
            galvo.line_to(half, coord)
            # Vertical line
            galvo.jump_to(coord, -half)
            galvo.line_to(coord, half)

    galvo.list_close()
    print("[+] Test Pattern Completed.")


def main():
    print_banner()
    parser = argparse.ArgumentParser(description="Rhothor Galvo Laser Scanner CLI")
    parser.add_argument("--config", "-c", type=str, default=None,
                        help="Path to hardware JSON profile (e.g., config/lpkf_ar180_els2.json)")
    parser.add_argument("--simulate", "-s", action="store_true", default=False,
                        help="Force software simulation mode without physical hardware")
    parser.add_argument("--info", "-i", action="store_true",
                        help="Query and display hardware status & configuration")
    parser.add_argument("--test", "-t", type=str, choices=["square", "circle", "cross", "grid", "all"],
                        help="Execute a test pattern")
    parser.add_argument("--size", type=float, default=50.0,
                        help="Size of the test pattern in mm (default: 50.0)")
    parser.add_argument("--speed", type=float, default=1500.0,
                        help="Marking speed in mm/sec (default: 1500.0)")
    parser.add_argument("--jump-speed", type=float, default=5000.0,
                        help="Jump speed in mm/sec (default: 5000.0)")
    parser.add_argument("--export-svg", "-o", type=str, default=None,
                        help="Export executed path to SVG file")

    args = parser.parse_args()

    # Load configuration if provided
    cfg = {}
    serial = 4281
    name = "LPKF"
    if args.config and os.path.exists(args.config):
        with open(args.config, "r", encoding="utf-8") as f:
            cfg = json.load(f)
            serial = cfg.get("serial_number", 4281)
            name = cfg.get("profile_name", "LPKF")
            print(f"[i] Loaded Profile: {name} (SN: {serial}) from {args.config}")

    try:
        with RhothorController(serial_nr=serial, name=name, simulate=args.simulate) as galvo:
            info = galvo.get_device_info()
            print("\n[+] Controller Connection Established:")
            print(f"    Mode:             {info['mode']}")
            print(f"    Profile/Name:     {info['name']}")
            print(f"    Serial Number:    {info['serial']}")
            print(f"    Firmware/DSP:     {info['version']}")
            print(f"    Working Field:    {info['field_size_mm']} x {info['field_size_mm']} mm")
            print(f"    Max Speed:        {info['max_speed_mm_s']} mm/s")
            print(f"    Scanner Delay:    {info['scanner_delay_us']} µs")

            if args.speed:
                galvo.set_speed(args.speed, args.jump_speed)

            if args.test:
                run_test_pattern(galvo, args.test, args.size)

            if galvo.simulate:
                stats = galvo.get_simulation_stats()
                print("\n[+] Kinematics & Timing Summary:")
                print(f"    Total Motion Distance: {stats['total_distance_mm']} mm")
                print(f"    - Laser Marking Dist:  {stats['total_mark_distance_mm']} mm ({stats['duty_cycle_percent']}%)")
                print(f"    - Galvo Jump Dist:     {stats['total_jump_distance_mm']} mm")
                print(f"    Estimated Job Time:    {stats['estimated_time_seconds']} s")

                if args.export_svg:
                    out = galvo.export_simulation_svg(args.export_svg)
                    print(f"\n[+] Saved Trajectory SVG: {out}")
                elif args.test:
                    default_svg = f"trajectory_{args.test}.svg"
                    out = galvo.export_simulation_svg(default_svg)
                    print(f"\n[+] Saved Trajectory SVG: {os.path.abspath(default_svg)}")

    except RhothorError as e:
        print(f"\n[-] Rhothor Error: {e}")
        sys.exit(1)
    except Exception as e:
        print(f"\n[-] Unexpected Error: {e}")
        sys.exit(1)


if __name__ == "__main__":
    main()
