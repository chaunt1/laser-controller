#!/usr/bin/env python3
"""
Unit Test Suite for Rhothor SDK & Simulator
"""

import os
import sys
import unittest

sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), "..")))
from sdk.python.rhothor import (
    RhothorController, RhothorError,
    ERR_OK, ERR_DATA, ERR_JOB, LIST_START, AUTO_START
)


class TestRhothorSDK(unittest.TestCase):

    def setUp(self):
        self.galvo = RhothorController(serial_nr=4281, name="LPKF", simulate=True)
        self.galvo.connect()

    def tearDown(self):
        self.galvo.disconnect()

    def test_connection_and_info(self):
        info = self.galvo.get_device_info()
        self.assertEqual(info["serial"], 4281)
        self.assertEqual(info["name"], "LPKF")
        self.assertEqual(info["field_size_mm"], 201.65)
        self.assertEqual(info["max_speed_mm_s"], 6000.0)
        self.assertEqual(info["scanner_delay_us"], 60)

    def test_speed_configuration(self):
        self.galvo.set_speed(mark_speed=2000.0, jump_speed=5500.0)
        self.assertEqual(self.galvo.sim.mark_speed, 2000.0)
        self.assertEqual(self.galvo.sim.jump_speed, 5500.0)

    def test_invalid_speed_raises_error(self):
        with self.assertRaises(RhothorError):
            self.galvo.set_speed(mark_speed=-500.0)

    def test_list_buffering_and_shapes(self):
        self.galvo.list_open(LIST_START)
        self.galvo.mark_rectangle(0, 0, 40, 40)
        self.galvo.mark_circle(0, 0, 15)
        self.galvo.list_close()

        stats = self.galvo.get_simulation_stats()
        self.assertGreater(stats["total_mark_distance_mm"], 200.0)
        self.assertGreater(stats["estimated_time_seconds"], 0.0)

    def test_3d_motion(self):
        self.galvo.list_open(LIST_START)
        self.galvo.jump_to_3d(0, 0, 10.0)
        self.galvo.line_to_3d(10.0, 10.0, 20.0)
        self.galvo.list_close()

        self.assertEqual(self.galvo.sim.current_x, 10.0)
        self.assertEqual(self.galvo.sim.current_y, 10.0)
        self.assertEqual(self.galvo.sim.current_z, 20.0)


if __name__ == "__main__":
    unittest.main()
