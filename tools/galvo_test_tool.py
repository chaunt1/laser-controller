#!/usr/bin/env python3
"""
Standalone Galvo Hardware Tester & Diagnostics Tool.
Allows engineers to test Rhothor Galvo connection, kinematics, and optics
independently without vendor software.

Usage:
    python tools/galvo_test_tool.py               # Launch GUI Test Suite
    python tools/galvo_test_tool.py --info        # Query hardware registers
    python tools/galvo_test_tool.py --selftest    # Run automated diagnostic self-test
    python tools/galvo_test_tool.py --test box    # Test 50x50 mm box (Laser OFF)
    python tools/galvo_test_tool.py --test origin # Position mirrors at (0, 0)
"""

import sys
import os
import argparse

# Ensure project root is in sys.path
PROJECT_ROOT = os.path.abspath(os.path.join(os.path.dirname(__file__), ".."))
if PROJECT_ROOT not in sys.path:
    sys.path.insert(0, PROJECT_ROOT)

from sdk.python.rhothor import RhothorController, ERR_OK, LIST_START


def run_cli_diagnostics(args):
    print("=" * 60)
    print("  RHOTHOR GALVO HARDWARE DIAGNOSTICS & TEST TOOL")
    print("  Publisher: chaunt <chaunt.dev@gmail.com>")
    print("=" * 60)

    with RhothorController(serial_nr=args.serial, name=args.name, simulate=args.simulate) as galvo:
        info = galvo.get_device_info()
        print(f"\n[+] Connection Status: {info['mode']}")
        print(f"    - Device Identifier:    {info['name']}")
        print(f"    - Serial Number:        {info['serial']}")
        print(f"    - DSP Firmware Version: {info['version']}")
        print(f"    - Optical Field Size:   {info['field_size_mm']:.2f} mm")
        print(f"    - Maximum Galvo Speed:  {info['max_speed_mm_s']:.0f} mm/s")
        print(f"    - Scanner Delay:        {info['scanner_delay_us']} µs")

        if args.selftest:
            print("\n[*] Running Automated Hardware Self-Test Suite...")
            tests = [
                ("1. Thư viện driver rhothor.dll (v7.3/v7.5 LPKF Legacy)", True if galvo.simulate or (galvo.native and galvo.native.is_loaded) else False),
                ("2. Đọc mã Serial phần cứng", info["serial"] == args.serial or info["serial"] > 0),
                ("3. Kiểm tra trường quét quang học", info["field_size_mm"] > 50.0),
                ("4. Kiểm tra giới hạn tốc độ galvo", info["max_speed_mm_s"] >= 1000.0),
            ]

            # Test list buffer
            try:
                ms = getattr(galvo, "max_speed", 2000.0)
                galvo.list_open(1)
                galvo.set_speed(mark_speed=min(800.0, ms * 0.5), jump_speed=min(1500.0, ms * 0.75))
                galvo.jump_to(0.0, 0.0)
                galvo.list_close()
                tests.append(("5. Kiểm tra bộ đệm lệnh DSP (List Buffer)", True))
            except Exception as e:
                tests.append(("5. Kiểm tra bộ đệm lệnh DSP (List Buffer)", False))

            all_ok = True
            for desc, passed in tests:
                tag = "[PASS] ✅" if passed else "[FAIL] ❌"
                print(f"    {tag} {desc}")
                if not passed:
                    all_ok = False

            print(f"\n[+] Self-Test Result: {'ALL TESTS PASSED' if all_ok else 'SOME CHECKS FAILED'}")

        if args.test:
            pattern = args.test.lower()
            print(f"\n[*] Executing Galvo Motion Pattern '{pattern}' (Laser OFF)...")
            ms = getattr(galvo, "max_speed", 2000.0)
            galvo.list_open(1)
            galvo.set_speed(mark_speed=min(1200.0, ms * 0.6), jump_speed=min(1500.0, ms * 0.75))

            if pattern in ("origin", "zero"):
                print("  -> Positioning mirrors at Center (0, 0)...")
                galvo.jump_to(0.0, 0.0)
            elif pattern in ("box", "square"):
                hs = 25.0
                print("  -> Tracing 50x50 mm boundary box...")
                galvo.jump_to(-hs, -hs)
                galvo.move_to(hs, -hs)
                galvo.move_to(hs, hs)
                galvo.move_to(-hs, hs)
                galvo.move_to(-hs, -hs)
            elif pattern in ("cross", "crosshair"):
                print("  -> Tracing 40 mm crosshair...")
                galvo.jump_to(-20.0, 0.0)
                galvo.move_to(20.0, 0.0)
                galvo.jump_to(0.0, -20.0)
                galvo.move_to(0.0, 20.0)
            elif pattern in ("circle", "circles"):
                print("  -> Tracing concentric circles (r=10, 20, 30 mm)...")
                for r in [10.0, 20.0, 30.0]:
                    galvo.jump_to(-r, 0.0)
                    galvo.arc_to(r, 0.0, 1.0)
                    galvo.arc_to(-r, 0.0, 1.0)

            galvo.list_close()
            print("[+] Motion Test Completed Successfully.")

        if getattr(args, "test_io", False):
            from tools.test_io_ports import run_auto_step_test
            step_time = getattr(args, "step_time", 5.0)
            run_auto_step_test(galvo, step_time=step_time, loop=False)


def main():
    parser = argparse.ArgumentParser(description="Rhothor Galvo Hardware Diagnostics & Testing Tool")
    parser.add_argument("--serial", type=int, default=4281, help="Hardware serial number (default: 4281)")
    parser.add_argument("--name", type=str, default="LPKF", help="Device identifier name (default: LPKF)")
    parser.add_argument("--simulate", "-s", action="store_true", default=False, help="Force simulation mode")
    parser.add_argument("--info", "-i", action="store_true", help="Display hardware status and exit")
    parser.add_argument("--selftest", action="store_true", help="Execute complete automated self-test")
    parser.add_argument("--test", "-t", type=str, choices=["origin", "box", "cross", "circle"], help="Run test pattern")
    parser.add_argument("--test-io", action="store_true", help="Test digital I/O ports 1/14 to 7/20 sequentially (5s each)")
    parser.add_argument("--step-time", type=float, default=5.0, help="Step time in seconds for --test-io (default: 5.0)")
    parser.add_argument("--gui", "-g", action="store_true", help="Launch interactive GUI test workstation")
    args = parser.parse_args()

    if args.info or args.selftest or args.test or args.test_io:
        run_cli_diagnostics(args)
    else:
        # Default: Launch GUI application with Test Suite tab in front
        from apps.rhothor_studio.main import run_studio
        run_studio(serial=args.serial, name=args.name)


if __name__ == "__main__":
    main()
