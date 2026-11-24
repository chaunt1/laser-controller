#!/usr/bin/env python3
"""
Example 01: Connection and Hardware Query
Demonstrates how to initialize Rhothor, select target serial, and query configuration.
"""

import sys
import os

sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), "..")))
from sdk.python.rhothor import RhothorController


def main():
    print("=== Example 01: Connect & Hardware Query ===")
    
    # Connect to device (Serial 4281 matching LPKF system in screenshot)
    with RhothorController(serial_nr=4281, name="LPKF") as galvo:
        info = galvo.get_device_info()
        print("\nSuccessfully connected to Rhothor Controller:")
        for k, v in info.items():
            print(f"  - {k:20s}: {v}")
            
    print("\nSession cleanly closed.")


if __name__ == "__main__":
    main()
