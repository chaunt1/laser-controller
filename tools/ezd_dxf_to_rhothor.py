#!/usr/bin/env python3
"""
EzCad DXF/PLT to Rhothor Vector Player
Reads DXF or PLT (HPGL) vector files exported from EzCad, AutoCAD, or CorelDRAW,
and executes them directly on Newson Rhothor Galvo Controller via USB.
"""

import os
import sys
import math
import argparse

sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), "..")))
from sdk.python.rhothor import RhothorController, RhothorError


def parse_simple_dxf(filepath: str):
    """
    Lightweight DXF parser extracting LINE, CIRCLE, and ARC entities.
    No heavy dependencies required.
    """
    entities = []
    with open(filepath, "r", encoding="utf-8", errors="ignore") as f:
        lines = [line.strip() for line in f]

    i = 0
    n = len(lines)
    while i < n:
        if lines[i] == "0" and i + 1 < n:
            entity_type = lines[i + 1]
            i += 2
            props = {}
            while i < n and lines[i] != "0":
                code = lines[i]
                val = lines[i + 1] if i + 1 < n else ""
                props[code] = val
                i += 2

            if entity_type == "LINE":
                x1 = float(props.get("10", 0.0))
                y1 = float(props.get("20", 0.0))
                x2 = float(props.get("11", 0.0))
                y2 = float(props.get("21", 0.0))
                entities.append(("LINE", x1, y1, x2, y2))
            elif entity_type == "CIRCLE":
                cx = float(props.get("10", 0.0))
                cy = float(props.get("20", 0.0))
                r = float(props.get("40", 1.0))
                entities.append(("CIRCLE", cx, cy, r))
            elif entity_type == "ARC":
                cx = float(props.get("10", 0.0))
                cy = float(props.get("20", 0.0))
                r = float(props.get("40", 1.0))
                start_angle = float(props.get("50", 0.0))
                end_angle = float(props.get("51", 360.0))
                entities.append(("ARC", cx, cy, r, start_angle, end_angle))
        else:
            i += 1

    return entities


def execute_dxf_on_rhothor(filepath: str, speed: float = 1500.0,
                           jump_speed: float = 5000.0, simulate: bool = False,
                           export_svg: str = None):
    print(f"[*] Parsing DXF file: {filepath}")
    entities = parse_simple_dxf(filepath)
    print(f"[+] Loaded {len(entities)} vector entities from DXF.")

    if not entities:
        print("[-] Warning: No supported vector entities (LINE, CIRCLE, ARC) found.")
        return

    with RhothorController(serial_nr=4281, name="LPKF", simulate=simulate) as galvo:
        galvo.set_speed(mark_speed=speed, jump_speed=jump_speed)
        galvo.set_laser_delays(gate_on_delay_us=50, gate_off_delay_us=50)

        print("[*] Dispatching vector commands to Rhothor DSP...")
        galvo.list_open(1)

        for ent in entities:
            etype = ent[0]
            if etype == "LINE":
                _, x1, y1, x2, y2 = ent
                galvo.jump_to(x1, y1)
                galvo.line_to(x2, y2)
            elif etype == "CIRCLE":
                _, cx, cy, r = ent
                galvo.mark_circle(cx, cy, r)
            elif etype == "ARC":
                _, cx, cy, r, a1, a2 = ent
                # Arc segmentation into line chords
                rad1 = math.radians(a1)
                rad2 = math.radians(a2)
                if rad2 < rad1:
                    rad2 += 2 * math.pi
                steps = max(8, int(abs(rad2 - rad1) / (math.pi / 18)))
                galvo.jump_to(cx + r * math.cos(rad1), cy + r * math.sin(rad1))
                for s in range(1, steps + 1):
                    theta = rad1 + (rad2 - rad1) * (s / steps)
                    galvo.line_to(cx + r * math.cos(theta), cy + r * math.sin(theta))

        galvo.list_close()
        print("[+] Vector marking list successfully executed.")

        if galvo.simulate:
            stats = galvo.get_simulation_stats()
            print("\nKinematic Execution Summary:")
            print(f"  Total Distance: {stats['total_distance_mm']} mm")
            print(f"  Mark Distance:  {stats['total_mark_distance_mm']} mm")
            print(f"  Estimated Time: {stats['estimated_time_seconds']} s")

            if export_svg:
                galvo.export_simulation_svg(export_svg)
                print(f"  SVG Output:     {export_svg}")


def main():
    parser = argparse.ArgumentParser(description="EzCad DXF to Rhothor Vector Player")
    parser.add_argument("--file", "-f", type=str, required=True,
                        help="Path to .dxf vector file exported from EzCad")
    parser.add_argument("--speed", type=float, default=1500.0,
                        help="Marking speed in mm/s (default: 1500.0)")
    parser.add_argument("--jump-speed", type=float, default=5000.0,
                        help="Jump speed in mm/s (default: 5000.0)")
    parser.add_argument("--simulate", "-s", action="store_true", default=False,
                        help="Run in simulation mode without physical hardware")
    parser.add_argument("--export-svg", "-o", type=str, default="dxf_output.svg",
                        help="Export executed path to SVG file")

    args = parser.parse_args()

    if not os.path.exists(args.file):
        print(f"[-] Error: File not found: {args.file}")
        sys.exit(1)

    execute_dxf_on_rhothor(args.file, args.speed, args.jump_speed,
                           args.simulate, args.export_svg)


if __name__ == "__main__":
    main()
