#!/usr/bin/env python3
"""
Example 02: 2D Vector Marking
Demonstrates vector marking commands: JumpTo, LineTo, ArcTo, Hatching, and SVG export.
"""

import sys
import os

sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), "..")))
from sdk.python.rhothor import RhothorController


def main():
    print("=== Example 02: 2D Vector Marking ===")
    
    with RhothorController(serial_nr=4281, name="LPKF") as galvo:
        # Set dynamic speeds
        galvo.set_speed(mark_speed=2000.0, jump_speed=6000.0)
        
        # Set laser gate delays (microseconds)
        galvo.set_laser_delays(gate_on_delay_us=60, gate_off_delay_us=60)
        
        # Open command list (Double-buffered execution)
        galvo.list_open(1)
        
        # 1. Outer calibration border
        print("Adding border 100x100 mm...")
        galvo.mark_rectangle(0, 0, 100, 100)
        
        # 2. Concentric circles with ArcTo (bulge factor = 1.0 for semicircle)
        print("Adding concentric circles...")
        for r in [10, 20, 30, 40]:
            galvo.mark_circle(0, 0, r)
            
        # 3. Center alignment crosshair
        print("Adding center crosshair...")
        galvo.mark_crosshair(0, 0, 15)
        
        # 4. Dense hatched rectangle in upper quadrant
        print("Adding hatched test box...")
        x_min, x_max = 20, 40
        y_min, y_max = 20, 40
        pitch = 1.0
        y = y_min
        while y <= y_max:
            galvo.jump_to(x_min, y)
            galvo.line_to(x_max, y)
            y += pitch

        # Close and dispatch list
        galvo.list_close()
        print("Command list dispatched to DSP controller.")
        
        if galvo.simulate:
            stats = galvo.get_simulation_stats()
            print("\nKinematic Execution Stats:")
            print(f"  Total Distance: {stats['total_distance_mm']} mm")
            print(f"  Mark Distance:  {stats['total_mark_distance_mm']} mm")
            print(f"  Estimated Time: {stats['estimated_time_seconds']} s")
            
            svg_path = os.path.abspath("example_2d_output.svg")
            galvo.export_simulation_svg(svg_path)
            print(f"  SVG Preview:    {svg_path}")


if __name__ == "__main__":
    main()
