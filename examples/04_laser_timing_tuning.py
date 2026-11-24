#!/usr/bin/env python3
"""
Example 04: Laser Gate Timing & Delay Tuning
Calibrates gate_on_delay and gate_off_delay to avoid corner burns or tail cuts.
"""

import sys
import os

sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), "..")))
from sdk.python.rhothor import RhothorController


def main():
    print("=== Example 04: Laser Timing & Delay Calibration ===")
    
    with RhothorController(serial_nr=4281, name="LPKF") as galvo:
        galvo.set_speed(mark_speed=2500.0, jump_speed=6000.0)
        
        # Test series of parallel lines with varying Gate On/Off Delays
        # to find optimal delay settings on photographic paper or anodized aluminum
        delays_to_test = [20, 40, 60, 80, 100]  # microseconds
        
        galvo.list_open(1)
        y_pos = -40.0
        
        for delay_us in delays_to_test:
            print(f"Generating test line with delay = {delay_us} µs at Y = {y_pos:.1f} mm...")
            # Set delay in command stream
            galvo.set_laser_delays(gate_on_delay_us=delay_us, gate_off_delay_us=delay_us)
            
            # Mark line from X=-30 to X=+30 mm
            galvo.jump_to(-30.0, y_pos)
            galvo.line_to(30.0, y_pos)
            
            y_pos += 20.0

        galvo.list_close()
        print("\nTiming calibration matrix executed.")
        print("Inspect endpoints under microscope to determine optimal delay.")


if __name__ == "__main__":
    main()
