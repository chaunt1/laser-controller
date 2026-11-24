#!/usr/bin/env python3
"""
Example 05: External Laser Triggering & Digital I/O
Demonstrates configuring hardware oscillators on IO5, IO6, IO7 for Q-switched lasers,
and burst pulses.
"""

import sys
import os

sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), "..")))
from sdk.python.rhothor import RhothorController


def main():
    print("=== Example 05: Laser Triggering & Oscillators ===")
    
    with RhothorController(serial_nr=4281, name="LPKF") as galvo:
        galvo.set_speed(mark_speed=1000.0, jump_speed=5000.0)
        
        # Configure Oscillator 3 (Linked to IO7 for Q-switch external trigger):
        # Period = 20 µs (50 kHz pulse frequency)
        # Pulse width = 5 µs
        print("Configuring Oscillator 3 (50 kHz, 5 µs pulse width on IO7)...")
        galvo.set_oscillator(nr=3, period_us=20.0, pulse_width_us=5.0)
        
        galvo.list_open(1)
        
        # Stationary burst drilling test: 5 spots
        print("Programming 5 stationary burst drill points (100 µs burst each)...")
        points = [(-20, 0), (-10, 0), (0, 0), (10, 0), (20, 0)]
        for pt in points:
            galvo.jump_to(pt[0], pt[1])
            galvo.burst(time_us=100)  # Fire laser while stationary
            galvo.sleep(time_us=50)   # Cooling delay

        galvo.list_close()
        print("Triggering and burst sequence complete.")


if __name__ == "__main__":
    main()
