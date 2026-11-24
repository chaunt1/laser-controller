#!/usr/bin/env python3
"""
Example 03: 3D Dynamic Focusing with ELEVAthor EL-S2
Demonstrates true 3D vector marking on stepped and sloped workpieces
without mechanical Z-table repositioning.
"""

import sys
import os

sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), "..")))
from sdk.python.rhothor import RhothorController


def main():
    print("=== Example 03: 3D Dynamic Focusing (ELEVAthor EL-S2) ===")
    
    with RhothorController(serial_nr=4281, name="LPKF") as galvo:
        galvo.set_speed(mark_speed=1500.0, jump_speed=4000.0)
        galvo.set_laser_delays(gate_on_delay_us=50, gate_off_delay_us=50)
        
        galvo.list_open(1)
        
        # 1. Multi-level stepped squares (different focal planes: Z = -10, 0, +10 mm)
        print("Marking stepped planes at Z = -10, 0, +10 mm...")
        for z, size in [(-10.0, 30.0), (0.0, 40.0), (10.0, 50.0)]:
            hs = size / 2.0
            galvo.jump_to_3d(-hs, -hs, z)
            galvo.line_to_3d(hs, -hs, z)
            galvo.line_to_3d(hs, hs, z)
            galvo.line_to_3d(-hs, hs, z)
            galvo.line_to_3d(-hs, -hs, z)

        # 2. Sloped 3D Ramp Line (marking continuously from Z = -15 mm to Z = +15 mm)
        print("Marking continuous sloped 3D ramp...")
        galvo.jump_to_3d(-30.0, -30.0, -15.0)
        galvo.line_to_3d(30.0, 30.0, 15.0)

        galvo.list_close()
        print("3D command list successfully sent.")
        
        if galvo.simulate:
            stats = galvo.get_simulation_stats()
            print(f"Total 3D Mark Distance: {stats['total_mark_distance_mm']} mm")
            print(f"Estimated Execution Time: {stats['estimated_time_seconds']} s")


if __name__ == "__main__":
    main()
