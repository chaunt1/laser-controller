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
        self.assertEqual(self.galvo.current_mark_speed, 2000.0)
        self.assertEqual(self.galvo.current_jump_speed, 5500.0)
        self.assertEqual(self.galvo.sim.mark_speed, 2000.0)
        self.assertEqual(self.galvo.sim.jump_speed, 5500.0)

    def test_deferred_speed_and_delays_on_list_open(self):
        # Configure before list_open
        self.galvo.set_speed(mark_speed=1800.0, jump_speed=4800.0)
        self.galvo.set_laser_delays(gate_on_delay_us=45, gate_off_delay_us=45)
        self.galvo.set_offsets(1.0, 2.0)

        # Ensure parameters are stored on controller
        self.assertEqual(self.galvo.current_mark_speed, 1800.0)
        self.assertEqual(self.galvo.current_jump_speed, 4800.0)
        self.assertEqual(self.galvo.current_laser_delays, (45, 45))
        self.assertEqual(self.galvo.current_offsets, (1.0, 2.0, 0.0))

        # Open and execute list
        self.galvo.list_open(LIST_START)
        self.assertTrue(self.galvo.is_list_open)
        self.galvo.jump_to(0, 0)
        self.galvo.line_to(10, 10)
        self.galvo.list_close()
        self.assertFalse(self.galvo.is_list_open)

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

    def test_dsp_firmware_version(self):
        v = self.galvo.get_version()
        self.assertIn("v7.5", v)

    def test_reconnect_sequence(self):
        self.galvo.disconnect()
        self.assertFalse(self.galvo.is_connected)
        self.galvo.connect()
        self.assertTrue(self.galvo.is_connected)
        self.galvo.list_open(LIST_START)
        self.galvo.jump_to(0, 0)
        self.galvo.list_close()

    def test_speed_clamping_to_hardware_max(self):
        self.galvo.max_speed = 2000.0
        self.galvo.set_speed(mark_speed=3000.0, jump_speed=5000.0)
        self.assertLessEqual(self.galvo.current_mark_speed, 2000.0)
        self.assertLessEqual(self.galvo.current_jump_speed, 2000.0)

    def test_digital_io_and_latch_pulse(self):
        self.galvo.set_io(0x55, 0xFF)
        self.assertEqual(self.galvo.get_io(), 0x55)
        self.galvo.send_latch_pulse(pin_bit=0)
        self.assertEqual(self.galvo.get_io(), 0x54)

    def test_individual_io_ports(self):
        # Test set_io_port for each of the 7 ports
        self.galvo.set_all_io_ports(False)
        self.assertEqual(self.galvo.get_io(), 0x00)

        for port in range(1, 8):
            self.galvo.set_io_port(port, True)
            self.assertEqual(self.galvo.get_io(), 1 << (port - 1))
            ports_state = self.galvo.get_io_ports()
            self.assertTrue(ports_state[port])
            self.galvo.set_io_port(port, False)
            self.assertEqual(self.galvo.get_io(), 0x00)

        # Test set_all_io_ports
        self.galvo.set_all_io_ports(True)
        self.assertEqual(self.galvo.get_io() & 0x7F, 0x7F)
        self.galvo.set_all_io_ports(False)
        self.assertEqual(self.galvo.get_io() & 0x7F, 0x00)


if __name__ == "__main__":
    unittest.main()
